using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class UcTragamonedas : UserControl, IVistaJuego
    {
        public event EventHandler SaldoActualizado;

        public void InicializarJuego(Usuario usuario)
        {
        }

        private readonly Usuario _usuario;
        private readonly PartidaServicio _servicio = new PartidaServicio();
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();
        private readonly Random _random = new Random();
        private readonly string[] _simbolos = { "7", "$", "BAR", "DIAM", "CEREZA" };
        private readonly List<Button> _botonesApuesta = new List<Button>();
        private readonly decimal[] _apuestasRapidas = { 500m, 1000m, 2500m, 5000m, 10000m, 25000m };

        public UcTragamonedas()
        {
            InitializeComponent();
            ConfigurarVista();
        }

        public UcTragamonedas(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
            ConfigurarVista();
            ActualizarSaldo();
        }

        private async void btnGirar_Click(object sender, EventArgs e)
        {
            if (_usuario == null)
            {
                MessageBox.Show("Debe iniciar sesion para jugar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtApuesta.Text, out decimal apuesta) || apuesta <= 0)
            {
                MessageBox.Show("Ingrese una apuesta valida.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (apuesta > _usuario.Saldo)
            {
                MessageBox.Show($"Saldo insuficiente. Tu saldo es ${_usuario.Saldo:N2}.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnGirar.Enabled = false;

            try
            {
                lblResultado.ForeColor = Color.Gold;
                lblResultado.Text = "Girando...";

                for (int i = 0; i < 14; i++)
                {
                    MostrarSimbolos(
                        _simbolos[_random.Next(_simbolos.Length)],
                        _simbolos[_random.Next(_simbolos.Length)],
                        _simbolos[_random.Next(_simbolos.Length)]);
                    await Task.Delay(45 + i * 6);
                }

                string s1 = _simbolos[_random.Next(_simbolos.Length)];
                string s2 = _simbolos[_random.Next(_simbolos.Length)];
                string s3 = _simbolos[_random.Next(_simbolos.Length)];

                decimal ganancia = CalcularGanancia(apuesta, s1, s2, s3);
                bool gano = ganancia > 0;
                int multiplicador = ObtenerMultiplicador(s1, s2, s3);
                MostrarSimbolos(s1, s2, s3);
                lblResultado.ForeColor = gano ? Color.FromArgb(34, 197, 94) : Color.FromArgb(248, 113, 113);
                lblResultado.Text = gano ? $"Ganaste ${ganancia:N2}" : $"Perdiste ${apuesta:N2}";
                lblJackpot.Text = ObtenerTextoResultado(multiplicador);
                Application.DoEvents();

                Partida partida = new Partida
                {
                    IdUsuario = _usuario.IdUsuario,
                    IdJuego = _servicio.ObtenerIdJuegoPorNombre("Tragamonedas"),
                    IdEstado = gano ? 2 : 3,
                    Apuesta = apuesta,
                    Ganancia = ganancia
                };

                if (partida.IdJuego == 0)
                {
                    MessageBox.Show("No se encontro el juego Tragamonedas en la base de datos. Reinicia la aplicacion para inicializarlo.",
                        "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string resultado = _servicio.RegistrarPartida(partida);
                if (resultado != "Guardado correctamente.")
                {
                    MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Usuario actualizado = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario);
                if (actualizado != null) _usuario.Saldo = actualizado.Saldo;
                ActualizarSaldo();
                SaldoActualizado?.Invoke(this, EventArgs.Empty);
            }
            finally
            {
                btnGirar.Enabled = true;
            }
        }

        private void ConfigurarVista()
        {
            DoubleBuffered = true;
            // Problema visual que resuelve: tragamonedas comparte navbar, saldo y controles del tema global con acento azul.
            AppTheme.ApplyView(this);
            AppTheme.ApplyNavbar(panelTop);
            AppTheme.ApplyTitle(lblTitulo);
            AppTheme.ApplySaldoLabel(lblSaldo);
            AppTheme.ApplyTextBox(txtApuesta);
            AppTheme.ApplyPrimaryButton(btnGirar, AppTheme.Azul);
            panelJuego.BackColor = AppTheme.BgPrincipal;
            panelMaquina.BackColor = Color.FromArgb(10, 30, 65);
            panelMaquina.BorderStyle = BorderStyle.None;
            panelRollos.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            panelRollos.BackColor = Color.Transparent;
            lblApuesta.BackColor = Color.Transparent;
            lblReglas.BackColor = Color.Transparent;
            lblResultado.BorderStyle = BorderStyle.None;
            lblJackpot.BorderStyle = BorderStyle.None;
            lblPalanca.BorderStyle = BorderStyle.None;
            lblReglas.ForeColor = AppTheme.TextoPrimario;
            lblJackpot.ForeColor = AppTheme.Dorado;
            lblResultado.BackColor = Color.Transparent;
            lblJackpot.BackColor = Color.Transparent;
            lblPalanca.BackColor = Color.Transparent;
            lblPalanca.Text = "APUESTA";
            lblReglas.Text = "2 iguales pagan x2  |  3 iguales pagan x8";

            txtApuesta.Text = "1000";
            lblJackpot.Text = "JACKPOT ROYAL";
            panelJuego.Paint += panelJuego_Paint;
            panelJuego.Resize += (s, e) => AplicarLayout();
            panelMaquina.Paint += panelMaquina_Paint;
            panelRollos.Paint += panelRollos_Paint;
            lblJackpot.Paint += Banner_Paint;
            lblResultado.Paint += Banner_Paint;
            lblPalanca.Paint += Palanca_Paint;
            lblRollo1.Paint += Rollo_Paint;
            lblRollo2.Paint += Rollo_Paint;
            lblRollo3.Paint += Rollo_Paint;
            CrearBotonesApuesta();
            AplicarLayout();
            MostrarSimbolos("7", "BAR", "$");
        }

        private void CrearBotonesApuesta()
        {
            if (_botonesApuesta.Count > 0) return;

            for (int i = 0; i < _apuestasRapidas.Length; i++)
            {
                Button boton = new Button
                {
                    BackColor = Color.FromArgb(17, 34, 64),
                    Cursor = Cursors.Hand,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9.25F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Name = "btnApuesta" + i,
                    Tag = _apuestasRapidas[i],
                    Text = FormatearValor(_apuestasRapidas[i]),
                    UseVisualStyleBackColor = false
                };

                boton.FlatAppearance.BorderColor = Color.FromArgb(234, 179, 8);
                boton.FlatAppearance.BorderSize = 1;
                boton.Click += ApuestaRapida_Click;
                _botonesApuesta.Add(boton);
                panelMaquina.Controls.Add(boton);
                boton.BringToFront();
            }
        }

        private void ApuestaRapida_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            txtApuesta.Text = ((decimal)boton.Tag).ToString("N0");

            foreach (Button item in _botonesApuesta)
                item.FlatAppearance.BorderSize = item == boton ? 3 : 1;
        }

        private void AplicarLayout()
        {
            if (panelJuego.ClientSize.Width <= 0 || panelJuego.ClientSize.Height <= 0) return;

            int margen = 24;
            int ancho = Math.Min(1120, panelJuego.ClientSize.Width - margen * 2);
            int alto = Math.Min(620, panelJuego.ClientSize.Height - margen * 2);
            ancho = Math.Max(780, ancho);
            alto = Math.Max(540, alto);

            panelMaquina.SetBounds(
                Math.Max(20, (panelJuego.ClientSize.Width - ancho) / 2),
                Math.Max(18, (panelJuego.ClientSize.Height - alto) / 2),
                Math.Min(ancho, panelJuego.ClientSize.Width - 40),
                Math.Min(alto, panelJuego.ClientSize.Height - 36));

            int inner = 46;
            int machineW = panelMaquina.ClientSize.Width;
            int machineH = panelMaquina.ClientSize.Height;

            // Problema visual que resuelve: los elementos de la maquina se distribuyen por zonas fijas y no se pisan al redimensionar.
            lblReglas.SetBounds(inner, 30, machineW - inner * 2, 28);
            lblJackpot.SetBounds(inner + 110, 70, machineW - (inner + 110) * 2, 64);
            int rollosH = Math.Min(220, Math.Max(165, machineH / 3));
            panelRollos.SetBounds(inner + 18, 158, machineW - inner * 2 - 36, rollosH);

            int controlsY = panelRollos.Bottom + 26;
            int apuestaW = Math.Min(230, Math.Max(190, machineW / 5));
            lblApuesta.SetBounds(inner, controlsY, apuestaW, 24);
            txtApuesta.SetBounds(inner, controlsY + 30, apuestaW, 36);

            int chipX = txtApuesta.Right + 22;
            int chipW = Math.Max(82, (machineW - chipX - inner - 232) / 3);
            for (int i = 0; i < _botonesApuesta.Count; i++)
            {
                _botonesApuesta[i].SetBounds(chipX + (i % 3) * (chipW + 8),
                    controlsY + (i / 3) * 42, chipW, 36);
                AppTheme.ApplyRoundedRegion(_botonesApuesta[i], 8);
            }

            btnGirar.SetBounds(machineW - inner - 220, controlsY + 4, 220, 78);
            int resultadoY = Math.Min(machineH - inner - 58, btnGirar.Bottom + 20);
            lblPalanca.SetBounds(inner, resultadoY, 170, 54);
            lblResultado.SetBounds(lblPalanca.Right + 22, resultadoY,
                Math.Max(260, machineW - lblPalanca.Right - inner - 22), 54);
            AppTheme.ApplyRoundedRegion(btnGirar, 12);

            panelJuego.Invalidate();
            panelMaquina.Invalidate();
            panelRollos.Invalidate();
            lblJackpot.Invalidate();
            lblResultado.Invalidate();
            lblPalanca.Invalidate();
        }

        private void MostrarSimbolos(string s1, string s2, string s3)
        {
            lblRollo1.Tag = s1;
            lblRollo2.Tag = s2;
            lblRollo3.Tag = s3;
            lblRollo1.Text = string.Empty;
            lblRollo2.Text = string.Empty;
            lblRollo3.Text = string.Empty;

            AplicarColorSimbolo(lblRollo1);
            AplicarColorSimbolo(lblRollo2);
            AplicarColorSimbolo(lblRollo3);

            lblRollo1.Invalidate();
            lblRollo2.Invalidate();
            lblRollo3.Invalidate();
        }

        private void AplicarColorSimbolo(Label label)
        {
            string simbolo = Convert.ToString(label.Tag);
            if (simbolo == "7") label.ForeColor = Color.FromArgb(220, 38, 38);
            else if (simbolo == "$") label.ForeColor = Color.FromArgb(202, 138, 4);
            else if (simbolo == "BAR") label.ForeColor = Color.FromArgb(15, 23, 42);
            else if (simbolo == "DIAM") label.ForeColor = Color.FromArgb(37, 99, 235);
            else label.ForeColor = Color.FromArgb(190, 18, 60);
        }

        private string FormatearValor(decimal valor)
        {
            if (valor >= 1000m)
            {
                decimal miles = valor / 1000m;
                return miles % 1 == 0 ? $"{miles:N0}K" : $"{miles:0.#}K";
            }

            return valor.ToString("N0");
        }

        private void panelJuego_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = panelJuego.ClientRectangle;
            using (LinearGradientBrush fondo = new LinearGradientBrush(area,
                Color.FromArgb(5, 9, 18), Color.FromArgb(12, 22, 42), 90F))
                e.Graphics.FillRectangle(fondo, area);

            using (SolidBrush glowAzul = new SolidBrush(Color.FromArgb(28, AppTheme.Azul)))
                e.Graphics.FillEllipse(glowAzul, panelJuego.Width / 2 - 360, 20, 720, 360);
            using (SolidBrush glowDorado = new SolidBrush(Color.FromArgb(18, AppTheme.Dorado)))
                e.Graphics.FillEllipse(glowDorado, panelJuego.Width / 2 - 250, panelJuego.Height - 170, 500, 220);
        }

        private void panelMaquina_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle area = new Rectangle(6, 6, panelMaquina.ClientSize.Width - 13, panelMaquina.ClientSize.Height - 13);
            using (GraphicsPath path = RoundedPath(area, 18))
            using (LinearGradientBrush fondo = new LinearGradientBrush(area,
                Color.FromArgb(14, 39, 90), Color.FromArgb(7, 11, 26), 90f))
            {
                e.Graphics.FillPath(fondo, path);
            }

            using (GraphicsPath path = RoundedPath(area, 18))
            using (Pen borde = new Pen(AppTheme.Dorado, 6))
                e.Graphics.DrawPath(borde, path);

            Rectangle marquee = new Rectangle(area.X + 84, area.Y + 18, area.Width - 168, 76);
            using (GraphicsPath path = RoundedPath(marquee, 16))
            using (LinearGradientBrush brush = new LinearGradientBrush(marquee,
                Color.FromArgb(118, 21, 21), Color.FromArgb(28, 11, 24), 90F))
            {
                e.Graphics.FillPath(brush, path);
                using (Pen pen = new Pen(AppTheme.Dorado, 4))
                    e.Graphics.DrawPath(pen, path);
            }

            for (int i = 0; i < 20; i++)
            {
                int x = area.X + 20 + i * Math.Max(1, (area.Width - 40) / 19);
                Color color = i % 2 == 0 ? AppTheme.Dorado : Color.FromArgb(59, 130, 246);
                using (SolidBrush luz = new SolidBrush(color))
                    e.Graphics.FillEllipse(luz, x - 5, area.Y + 12, 10, 10);
                using (SolidBrush glow = new SolidBrush(Color.FromArgb(55, color)))
                    e.Graphics.FillEllipse(glow, x - 9, area.Y + 8, 18, 18);
            }

            Rectangle baseArea = new Rectangle(area.X + 34, area.Bottom - 112, area.Width - 68, 74);
            using (GraphicsPath path = RoundedPath(baseArea, 14))
            using (LinearGradientBrush baseBrush = new LinearGradientBrush(baseArea,
                Color.FromArgb(18, 31, 55), Color.FromArgb(8, 14, 28), 90F))
            {
                e.Graphics.FillPath(baseBrush, path);
                using (Pen pen = new Pen(Color.FromArgb(110, AppTheme.Dorado), 1))
                    e.Graphics.DrawPath(pen, path);
            }

            int leverX = area.Right - 34;
            int leverY = area.Y + area.Height / 2;
            using (Pen stick = new Pen(Color.FromArgb(230, 180, 70), 8))
                e.Graphics.DrawLine(stick, leverX - 2, leverY - 40, leverX + 28, leverY - 88);
            using (SolidBrush knob = new SolidBrush(Color.FromArgb(220, 38, 38)))
                e.Graphics.FillEllipse(knob, leverX + 12, leverY - 110, 38, 38);
            using (Pen knobBorder = new Pen(AppTheme.Dorado, 3))
                e.Graphics.DrawEllipse(knobBorder, leverX + 12, leverY - 110, 38, 38);

            using (Pen brillo = new Pen(Color.FromArgb(120, Color.White), 1))
            using (GraphicsPath path = RoundedPath(new Rectangle(18, 18, panelMaquina.ClientSize.Width - 37, panelMaquina.ClientSize.Height - 37), 14))
                e.Graphics.DrawPath(brillo, path);
        }

        private void Banner_Paint(object sender, PaintEventArgs e)
        {
            Label label = (Label)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = new Rectangle(0, 0, label.Width - 1, label.Height - 1);
            using (GraphicsPath path = RoundedPath(area, 12))
            using (LinearGradientBrush fondo = new LinearGradientBrush(area,
                Color.FromArgb(15, 23, 42), Color.FromArgb(7, 11, 20), 90F))
            {
                e.Graphics.FillPath(fondo, path);
                using (Pen pen = new Pen(Color.FromArgb(100, AppTheme.Dorado), 1))
                    e.Graphics.DrawPath(pen, path);
            }

            TextRenderer.DrawText(e.Graphics, label.Text, label.Font, label.ClientRectangle, label.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private void Palanca_Paint(object sender, PaintEventArgs e)
        {
            Label label = (Label)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = new Rectangle(0, 0, label.Width - 1, label.Height - 1);
            using (GraphicsPath path = RoundedPath(area, 12))
            using (LinearGradientBrush fondo = new LinearGradientBrush(area,
                Color.FromArgb(245, 158, 11), Color.FromArgb(234, 179, 8), 90F))
            {
                e.Graphics.FillPath(fondo, path);
                using (Pen pen = new Pen(Color.FromArgb(120, 53, 15), 2))
                    e.Graphics.DrawPath(pen, path);
            }

            TextRenderer.DrawText(e.Graphics, label.Text, new Font("Segoe UI", 12F, FontStyle.Bold),
                label.ClientRectangle, Color.FromArgb(17, 24, 39),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void panelRollos_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle area = new Rectangle(0, 0, panelRollos.ClientSize.Width - 1, panelRollos.ClientSize.Height - 1);
            using (GraphicsPath path = RoundedPath(area, 14))
            using (LinearGradientBrush fondo = new LinearGradientBrush(area, Color.FromArgb(8, 13, 26), Color.FromArgb(42, 50, 68), 90F))
            {
                e.Graphics.FillPath(fondo, path);
                using (Pen borde = new Pen(AppTheme.Dorado, 5))
                    e.Graphics.DrawPath(borde, path);
            }

            int colW = panelRollos.ClientSize.Width / 3;
            using (Pen divisor = new Pen(Color.FromArgb(120, AppTheme.Dorado), 3))
            {
                e.Graphics.DrawLine(divisor, colW, 12, colW, panelRollos.Height - 12);
                e.Graphics.DrawLine(divisor, colW * 2, 12, colW * 2, panelRollos.Height - 12);
            }
        }

        private void Rollo_Paint(object sender, PaintEventArgs e)
        {
            Label rollo = (Label)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.Transparent);

            Rectangle area = new Rectangle(14, 14, rollo.ClientSize.Width - 28, rollo.ClientSize.Height - 28);
            using (GraphicsPath path = RoundedPath(area, 12))
            using (LinearGradientBrush fondo = new LinearGradientBrush(area, Color.White, Color.FromArgb(226, 232, 240), 90f))
                e.Graphics.FillPath(fondo, path);

            using (GraphicsPath path = RoundedPath(area, 12))
            using (Pen borde = new Pen(Color.FromArgb(203, 213, 225), 3))
                e.Graphics.DrawPath(borde, path);

            string simbolo = Convert.ToString(rollo.Tag);
            if (simbolo == "7")
                DibujarSiete(e.Graphics, area);
            else if (simbolo == "$")
                DibujarMoneda(e.Graphics, area);
            else if (simbolo == "BAR")
                DibujarBar(e.Graphics, area);
            else if (simbolo == "DIAM")
                DibujarDiamante(e.Graphics, area);
            else
                DibujarCereza(e.Graphics, area);
        }

        private GraphicsPath RoundedPath(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void DibujarSiete(Graphics g, Rectangle area)
        {
            using (Font fuente = new Font("Georgia", Math.Max(32, area.Height / 2.4F), FontStyle.Bold))
            {
                TextRenderer.DrawText(g, "7", fuente, area, Color.FromArgb(220, 38, 38),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private void DibujarMoneda(Graphics g, Rectangle area)
        {
            int lado = Math.Min(area.Width, area.Height) - 34;
            Rectangle moneda = new Rectangle(area.X + (area.Width - lado) / 2, area.Y + (area.Height - lado) / 2, lado, lado);
            using (SolidBrush relleno = new SolidBrush(Color.FromArgb(250, 204, 21)))
                g.FillEllipse(relleno, moneda);
            using (Pen borde = new Pen(Color.FromArgb(180, 83, 9), 5))
                g.DrawEllipse(borde, moneda);
            using (Font fuente = new Font("Segoe UI", Math.Max(24, lado / 3), FontStyle.Bold))
                TextRenderer.DrawText(g, "$", fuente, moneda, Color.FromArgb(120, 53, 15),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DibujarBar(Graphics g, Rectangle area)
        {
            Rectangle barra = new Rectangle(area.X + 22, area.Y + area.Height / 3, area.Width - 44, area.Height / 3);
            using (SolidBrush relleno = new SolidBrush(Color.FromArgb(15, 23, 42)))
                g.FillRectangle(relleno, barra);
            using (Pen borde = new Pen(Color.FromArgb(234, 179, 8), 3))
                g.DrawRectangle(borde, barra);
            using (Font fuente = new Font("Segoe UI", Math.Max(20, area.Height / 5), FontStyle.Bold))
                TextRenderer.DrawText(g, "BAR", fuente, barra, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DibujarDiamante(Graphics g, Rectangle area)
        {
            Point centro = new Point(area.X + area.Width / 2, area.Y + area.Height / 2);
            int w = Math.Min(area.Width, area.Height) / 3;
            int h = Math.Min(area.Width, area.Height) / 3;
            Point[] puntos =
            {
                new Point(centro.X, centro.Y - h),
                new Point(centro.X + w, centro.Y),
                new Point(centro.X, centro.Y + h),
                new Point(centro.X - w, centro.Y)
            };

            using (SolidBrush relleno = new SolidBrush(Color.FromArgb(56, 189, 248)))
                g.FillPolygon(relleno, puntos);
            using (Pen borde = new Pen(Color.FromArgb(37, 99, 235), 4))
                g.DrawPolygon(borde, puntos);
            using (Pen brillo = new Pen(Color.White, 2))
                g.DrawLine(brillo, centro.X - w / 2, centro.Y, centro.X, centro.Y - h / 2);
        }

        private void DibujarCereza(Graphics g, Rectangle area)
        {
            int r = Math.Max(16, Math.Min(area.Width, area.Height) / 7);
            Point c1 = new Point(area.X + area.Width / 2 - r, area.Y + area.Height / 2 + r / 3);
            Point c2 = new Point(area.X + area.Width / 2 + r, area.Y + area.Height / 2 + r / 4);

            using (Pen tallo = new Pen(Color.FromArgb(21, 128, 61), 4))
            {
                g.DrawCurve(tallo, new[] { c1, new Point(c1.X + r / 2, c1.Y - r * 2), new Point(c2.X, c2.Y - r * 2), c2 });
            }

            using (SolidBrush cereza = new SolidBrush(Color.FromArgb(220, 38, 38)))
            {
                g.FillEllipse(cereza, new Rectangle(c1.X - r, c1.Y - r, r * 2, r * 2));
                g.FillEllipse(cereza, new Rectangle(c2.X - r, c2.Y - r, r * 2, r * 2));
            }

            using (SolidBrush brillo = new SolidBrush(Color.FromArgb(254, 202, 202)))
            {
                g.FillEllipse(brillo, new Rectangle(c1.X - r / 2, c1.Y - r / 2, r / 2, r / 2));
                g.FillEllipse(brillo, new Rectangle(c2.X - r / 2, c2.Y - r / 2, r / 2, r / 2));
            }
        }

        private decimal CalcularGanancia(decimal apuesta, string s1, string s2, string s3)
        {
            int multiplicador = ObtenerMultiplicador(s1, s2, s3);
            if (multiplicador > 0) return apuesta * multiplicador;
            return 0m;
        }

        private int ObtenerMultiplicador(string s1, string s2, string s3)
        {
            if (s1 == s2 && s2 == s3) return 8;
            if (s1 == s2 || s1 == s3 || s2 == s3) return 2;
            return 0;
        }

        private string ObtenerTextoResultado(int multiplicador)
        {
            if (multiplicador == 8) return "JACKPOT: 3 iguales x8";
            if (multiplicador == 2) return "Premio menor: 2 iguales x2";
            return "Sin combinacion ganadora";
        }

        private void ActualizarSaldo()
        {
            lblSaldo.Text = $"Saldo: ${_usuario.Saldo:N2}";
        }
    }
}

