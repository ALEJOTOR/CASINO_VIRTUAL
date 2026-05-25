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
                    Ganancia = ganancia,
                    Resultado = gano ? "gano" : "perdio"
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
            CasinoTheme.StylePage(this);
            CasinoTheme.StyleHeader(panelTop);
            CasinoTheme.StyleTitle(lblTitulo);
            CasinoTheme.StyleInput(txtApuesta);
            CasinoTheme.StyleActionButton(btnGirar, CasinoTheme.Green);
            panelJuego.BackColor = CasinoTheme.Page;
            panelMaquina.BackColor = CasinoTheme.Purple;
            lblReglas.ForeColor = CasinoTheme.Text;
            lblJackpot.ForeColor = CasinoTheme.Gold;

            txtApuesta.Text = "1000";
            lblJackpot.Text = "Tabla de pagos: 2 iguales x2 | 3 iguales x8";
            panelJuego.Resize += (s, e) => AplicarLayout();
            panelMaquina.Paint += panelMaquina_Paint;
            panelRollos.Paint += panelRollos_Paint;
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
                    BackColor = Color.FromArgb(30, 41, 59),
                    Cursor = Cursors.Hand,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
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

            int margen = 34;
            int ancho = Math.Min(1060, panelJuego.ClientSize.Width - margen * 2);
            int alto = Math.Min(560, panelJuego.ClientSize.Height - margen * 2);
            ancho = Math.Max(760, ancho);
            alto = Math.Max(500, alto);

            panelMaquina.SetBounds(
                Math.Max(20, (panelJuego.ClientSize.Width - ancho) / 2),
                Math.Max(18, (panelJuego.ClientSize.Height - alto) / 2),
                Math.Min(ancho, panelJuego.ClientSize.Width - 40),
                Math.Min(alto, panelJuego.ClientSize.Height - 36));

            int inner = 42;
            int machineW = panelMaquina.ClientSize.Width;
            int machineH = panelMaquina.ClientSize.Height;

            lblReglas.SetBounds(inner, 28, machineW - inner * 2, 32);
            lblJackpot.SetBounds(inner + 80, 76, machineW - (inner + 80) * 2, 56);
            panelRollos.SetBounds(inner, 158, machineW - inner * 2, Math.Max(130, machineH / 4));

            int controlsY = panelRollos.Bottom + 28;
            lblApuesta.SetBounds(inner, controlsY, 160, 24);
            txtApuesta.SetBounds(inner, controlsY + 30, 180, 34);

            int chipX = txtApuesta.Right + 24;
            int chipW = Math.Max(74, (machineW - chipX - inner - 220) / 3);
            for (int i = 0; i < _botonesApuesta.Count; i++)
            {
                _botonesApuesta[i].SetBounds(chipX + (i % 3) * (chipW + 8),
                    controlsY + (i / 3) * 40, chipW, 34);
            }

            btnGirar.SetBounds(machineW - inner - 190, controlsY + 10, 190, 58);
            lblPalanca.SetBounds(inner, btnGirar.Bottom + 22, 150, 46);
            lblResultado.SetBounds(lblPalanca.Right + 22, btnGirar.Bottom + 22,
                machineW - lblPalanca.Right - inner - 22, 46);

            panelMaquina.Invalidate();
            panelRollos.Invalidate();
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

        private void panelMaquina_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle area = new Rectangle(8, 8, panelMaquina.ClientSize.Width - 17, panelMaquina.ClientSize.Height - 17);
            using (LinearGradientBrush fondo = new LinearGradientBrush(area,
                Color.FromArgb(88, 28, 135), Color.FromArgb(30, 41, 59), 35f))
            {
                e.Graphics.FillRectangle(fondo, area);
            }

            using (Pen borde = new Pen(Color.Gold, 4))
                e.Graphics.DrawRectangle(borde, area);

            using (Pen brillo = new Pen(Color.FromArgb(120, Color.White), 1))
                e.Graphics.DrawRectangle(brillo, new Rectangle(18, 18, panelMaquina.ClientSize.Width - 37, panelMaquina.ClientSize.Height - 37));
        }

        private void panelRollos_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (Pen borde = new Pen(Color.FromArgb(234, 179, 8), 5))
                e.Graphics.DrawRectangle(borde, new Rectangle(2, 2, panelRollos.ClientSize.Width - 5, panelRollos.ClientSize.Height - 5));
        }

        private void Rollo_Paint(object sender, PaintEventArgs e)
        {
            Label rollo = (Label)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.FromArgb(255, 250, 240));

            Rectangle area = new Rectangle(14, 14, rollo.ClientSize.Width - 28, rollo.ClientSize.Height - 28);
            using (LinearGradientBrush fondo = new LinearGradientBrush(area, Color.White, Color.FromArgb(226, 232, 240), 90f))
                e.Graphics.FillRectangle(fondo, area);

            using (Pen borde = new Pen(Color.FromArgb(203, 213, 225), 3))
                e.Graphics.DrawRectangle(borde, area);

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

