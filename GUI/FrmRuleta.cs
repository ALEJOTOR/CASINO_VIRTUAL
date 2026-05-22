using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmRuleta : Form
    {
        private readonly Usuario _usuario;
        private readonly PartidaServicio _servicio = new PartidaServicio();
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();
        private readonly Random _random = new Random();
        private readonly List<ApuestaRuleta> _apuestas = new List<ApuestaRuleta>();
        private decimal _fichaSeleccionada = 50m;

        public FrmRuleta()
        {
            InitializeComponent();
            ConfigurarApuestasIniciales();
        }

        public FrmRuleta(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
            ConfigurarApuestasIniciales();
            ActualizarSaldo();
        }

        private void ConfigurarApuestasIniciales()
        {
            txtApuesta.Text = FormatearFicha(_fichaSeleccionada);
            cboTipoApuesta.Visible = false;
            numNumero.Visible = false;
            lblNumero.Text = "Selecciona una ficha y toca la mesa";

            CrearBotonesFichas();
            CrearZonasApuesta();
            PrepararApuestasExternas();
            ActualizarTotalApostado();
        }

        private void cboTipoApuesta_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnGirarRuleta_Click(object sender, EventArgs e)
        {
            if (!ValidarGiro(out decimal totalApostado)) return;

            int numero = _random.Next(0, 37);
            string color = ObtenerColor(numero);
            decimal ganancia = 0m;

            foreach (ApuestaRuleta apuesta in _apuestas)
                if (EsApuestaGanadora(apuesta, numero, color))
                    ganancia += apuesta.Monto * ObtenerMultiplicador(apuesta);

            MostrarNumeroEnRueda(numero, color);
            RegistrarResultado(totalApostado, ganancia, ganancia > 0,
                $"Salio {numero} {color}. Fichas jugadas: {_apuestas.Count}. Total apostado: ${totalApostado:N0}.");
        }

        private void btnLimpiarMesa_Click(object sender, EventArgs e)
        {
            LimpiarApuestas();
            lblResultado.Text = "Mesa limpia. Selecciona una ficha y toca la mesa.";
            lblResultado.ForeColor = Color.WhiteSmoke;
            lblPremio.Text = "Hagan sus apuestas";
            lblPremio.ForeColor = Color.Gold;
        }

        private bool ValidarGiro(out decimal totalApostado)
        {
            if (_usuario == null)
            {
                totalApostado = 0;
                MessageBox.Show("Debe iniciar sesion para jugar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            totalApostado = CalcularTotalApostado();
            if (totalApostado <= 0)
            {
                MessageBox.Show("Selecciona una ficha y colocala sobre la mesa antes de girar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (totalApostado > _usuario.Saldo)
            {
                MessageBox.Show($"Saldo insuficiente. Tu saldo es ${_usuario.Saldo:N2}.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            btnGirarRuleta.Enabled = false;
            return true;
        }

        private void RegistrarResultado(decimal apuesta, decimal ganancia, bool gano, string detalle)
        {
            try
            {
                Partida partida = new Partida
                {
                    IdUsuario = _usuario.IdUsuario,
                    IdJuego = _servicio.ObtenerIdJuegoPorNombre("Ruleta"),
                    IdEstado = gano ? 2 : 3,
                    Apuesta = apuesta,
                    Ganancia = ganancia,
                    Resultado = gano ? "gano" : "perdio"
                };

                if (partida.IdJuego == 0)
                {
                    MessageBox.Show("No se encontro el juego Ruleta en la base de datos. Reinicia la aplicacion para inicializarlo.",
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

                lblResultado.Text = detalle;
                lblResultado.ForeColor = gano ? Color.FromArgb(34, 197, 94) : Color.FromArgb(248, 113, 113);
                lblPremio.Text = gano ? $"Ganaste ${ganancia:N2}" : $"Perdiste ${apuesta:N2}";
                LimpiarApuestas();
            }
            finally
            {
                btnGirarRuleta.Enabled = true;
            }
        }

        private void CrearBotonesFichas()
        {
            decimal[] valores = { 50m, 500m, 2500m, 5000m, 25000m, 100000m, 500000m, 5000000m };
            Color[] colores =
            {
                Color.FromArgb(120, 113, 108),
                Color.FromArgb(180, 83, 9),
                Color.FromArgb(107, 114, 128),
                Color.FromArgb(202, 138, 4),
                Color.FromArgb(185, 28, 28),
                Color.FromArgb(2, 132, 199),
                Color.FromArgb(31, 41, 55),
                Color.FromArgb(37, 99, 235)
            };

            for (int i = 0; i < valores.Length; i++)
            {
                Button ficha = new Button
                {
                    BackColor = colores[i],
                    Cursor = Cursors.Hand,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(24 + (i % 4) * 55, 122 + (i / 4) * 42),
                    Name = "btnFicha" + i,
                    Size = new Size(48, 34),
                    Tag = valores[i],
                    Text = FormatearFicha(valores[i]),
                    UseVisualStyleBackColor = false
                };

                ficha.FlatAppearance.BorderColor = Color.Gold;
                ficha.FlatAppearance.BorderSize = valores[i] == _fichaSeleccionada ? 2 : 0;
                ficha.Click += Ficha_Click;
                panelControles.Controls.Add(ficha);
                ficha.BringToFront();
            }
        }

        private void CrearZonasApuesta()
        {
            int x = 76;
            int y = 16;
            int w = 66;
            int h = 20;
            int numero = 1;

            for (int fila = 0; fila < 12; fila++)
            {
                for (int col = 0; col < 3; col++)
                {
                    int n = numero++;
                    Label zona = new Label
                    {
                        BackColor = Color.Transparent,
                        Cursor = Cursors.Hand,
                        Location = new Point(x + col * w, y + (11 - fila) * (h - 1)),
                        Size = new Size(w, h),
                        Tag = new ApuestaRuleta { Tipo = "Numero", Numero = n }
                    };

                    zona.Click += ZonaApuesta_Click;
                    panelTapete.Controls.Add(zona);
                    zona.BringToFront();
                }
            }
        }

        private void PrepararApuestasExternas()
        {
            CrearZonaExterna("1-18", "1-18", new Rectangle(14, 304, 54, 30), Color.FromArgb(6, 78, 59));
            CrearZonaExterna("PAR", "Par", new Rectangle(70, 304, 54, 30), Color.FromArgb(6, 78, 59));
            CrearZonaExterna("IMPAR", "Impar", new Rectangle(238, 304, 54, 30), Color.FromArgb(6, 78, 59));
            CrearZonaExterna("19-36", "19-36", new Rectangle(294, 304, 54, 30), Color.FromArgb(6, 78, 59));

            lblCero.Cursor = Cursors.Hand;
            lblRojo.Cursor = Cursors.Hand;
            lblNegro.Cursor = Cursors.Hand;
            lblDocena1.Cursor = Cursors.Hand;
            lblDocena2.Cursor = Cursors.Hand;
            lblDocena3.Cursor = Cursors.Hand;

            lblCero.Tag = new ApuestaRuleta { Tipo = "Numero", Numero = 0 };
            lblRojo.Tag = new ApuestaRuleta { Tipo = "Rojo" };
            lblNegro.Tag = new ApuestaRuleta { Tipo = "Negro" };
            lblDocena1.Tag = new ApuestaRuleta { Tipo = "Docena1" };
            lblDocena2.Tag = new ApuestaRuleta { Tipo = "Docena2" };
            lblDocena3.Tag = new ApuestaRuleta { Tipo = "Docena3" };

            lblCero.Click += ZonaApuesta_Click;
            lblRojo.Click += ZonaApuesta_Click;
            lblNegro.Click += ZonaApuesta_Click;
            lblDocena1.Click += ZonaApuesta_Click;
            lblDocena2.Click += ZonaApuesta_Click;
            lblDocena3.Click += ZonaApuesta_Click;
        }

        private void CrearZonaExterna(string texto, string tipo, Rectangle ubicacion, Color color)
        {
            Label zona = new Label
            {
                BackColor = color,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = ubicacion.Location,
                Size = ubicacion.Size,
                Tag = new ApuestaRuleta { Tipo = tipo },
                Text = texto,
                TextAlign = ContentAlignment.MiddleCenter
            };

            zona.Click += ZonaApuesta_Click;
            panelTapete.Controls.Add(zona);
            zona.BringToFront();
        }

        private void Ficha_Click(object sender, EventArgs e)
        {
            Button ficha = (Button)sender;
            _fichaSeleccionada = (decimal)ficha.Tag;
            txtApuesta.Text = FormatearFicha(_fichaSeleccionada);

            foreach (Control control in panelControles.Controls)
                if (control is Button boton && boton.Name.StartsWith("btnFicha"))
                    boton.FlatAppearance.BorderSize = boton == ficha ? 2 : 0;
        }

        private void ZonaApuesta_Click(object sender, EventArgs e)
        {
            Control zona = (Control)sender;
            ApuestaRuleta datos = (ApuestaRuleta)zona.Tag;

            if (_usuario != null && CalcularTotalApostado() + _fichaSeleccionada > _usuario.Saldo)
            {
                MessageBox.Show("No tienes saldo suficiente para colocar esa ficha.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ApuestaRuleta existente = BuscarApuesta(datos.Tipo, datos.Numero);
            if (existente != null)
            {
                existente.Monto += _fichaSeleccionada;
                existente.Ficha.Text = FormatearFicha(existente.Monto);
                existente.Ficha.Invalidate();
                existente.Ficha.BringToFront();
                ActualizarTotalApostado();
                return;
            }

            ApuestaRuleta apuesta = new ApuestaRuleta
            {
                Tipo = datos.Tipo,
                Numero = datos.Numero,
                Monto = _fichaSeleccionada
            };

            Label ficha = CrearFichaVisual(zona, apuesta);
            apuesta.Ficha = ficha;
            _apuestas.Add(apuesta);
            panelTapete.Controls.Add(ficha);
            ficha.Click += ZonaApuesta_Click;
            ficha.BringToFront();
            ActualizarTotalApostado();
        }

        private ApuestaRuleta BuscarApuesta(string tipo, int numero)
        {
            foreach (ApuestaRuleta apuesta in _apuestas)
                if (apuesta.Tipo == tipo && apuesta.Numero == numero)
                    return apuesta;

            return null;
        }

        private Label CrearFichaVisual(Control zona, ApuestaRuleta apuesta)
        {
            Point centro = new Point(zona.Left + zona.Width / 2 - 18, zona.Top + zona.Height / 2 - 18);

            Label ficha = new Label
            {
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 7F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = centro,
                Size = new Size(36, 36),
                Tag = new ApuestaRuleta { Tipo = apuesta.Tipo, Numero = apuesta.Numero },
                Text = FormatearFicha(apuesta.Monto),
                TextAlign = ContentAlignment.MiddleCenter
            };

            ficha.Paint += FichaVisual_Paint;
            return ficha;
        }

        private void FichaVisual_Paint(object sender, PaintEventArgs e)
        {
            Label ficha = (Label)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle circulo = new Rectangle(2, 2, ficha.Width - 5, ficha.Height - 5);
            using (SolidBrush relleno = new SolidBrush(Color.Gold))
                e.Graphics.FillEllipse(relleno, circulo);
            using (Pen borde = new Pen(Color.FromArgb(120, 53, 15), 3))
                e.Graphics.DrawEllipse(borde, circulo);
            using (Pen brillo = new Pen(Color.White, 1))
                e.Graphics.DrawEllipse(brillo, new Rectangle(6, 6, ficha.Width - 13, ficha.Height - 13));

            TextRenderer.DrawText(e.Graphics, ficha.Text, ficha.Font, ficha.ClientRectangle,
                Color.FromArgb(15, 23, 42),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void ActualizarTotalApostado()
        {
            lblTotalApostado.Text = $"Total apostado: ${CalcularTotalApostado():N0}";
        }

        private decimal CalcularTotalApostado()
        {
            decimal total = 0m;
            foreach (ApuestaRuleta apuesta in _apuestas)
                total += apuesta.Monto;

            return total;
        }

        private void LimpiarApuestas()
        {
            foreach (ApuestaRuleta apuesta in _apuestas)
                if (apuesta.Ficha != null)
                    panelTapete.Controls.Remove(apuesta.Ficha);

            _apuestas.Clear();
            ActualizarTotalApostado();
        }

        private bool EsApuestaGanadora(ApuestaRuleta apuesta, int numero, string color)
        {
            switch (apuesta.Tipo)
            {
                case "Rojo":
                    return color == "Rojo";
                case "Negro":
                    return color == "Negro";
                case "Docena1":
                    return numero >= 1 && numero <= 12;
                case "Docena2":
                    return numero >= 13 && numero <= 24;
                case "Docena3":
                    return numero >= 25 && numero <= 36;
                case "1-18":
                    return numero >= 1 && numero <= 18;
                case "19-36":
                    return numero >= 19 && numero <= 36;
                case "Par":
                    return numero != 0 && numero % 2 == 0;
                case "Impar":
                    return numero % 2 == 1;
                case "Numero":
                    return numero == apuesta.Numero;
                default:
                    return false;
            }
        }

        private int ObtenerMultiplicador(ApuestaRuleta apuesta)
        {
            if (apuesta.Tipo == "Numero") return apuesta.Numero == 0 ? 36 : 36;
            if (apuesta.Tipo.StartsWith("Docena")) return 3;
            return 2;
        }

        private string FormatearFicha(decimal valor)
        {
            if (valor >= 1000000m) return $"{valor / 1000000m:N0}M";
            if (valor >= 1000m) return $"{valor / 1000m:N0}K";
            return valor.ToString("N0");
        }

        private class ApuestaRuleta
        {
            public string Tipo { get; set; }
            public int Numero { get; set; }
            public decimal Monto { get; set; }
            public Label Ficha { get; set; }
        }

        private string ObtenerColor(int numero)
        {
            if (numero == 0) return "Verde";

            int[] rojos = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
            foreach (int rojo in rojos)
                if (numero == rojo) return "Rojo";

            return "Negro";
        }

        private void MostrarNumeroEnRueda(int numero, string color)
        {
            lblNumeroGanador.Text = numero.ToString();

            if (color == "Rojo")
                lblNumeroGanador.BackColor = Color.FromArgb(185, 28, 28);
            else if (color == "Negro")
                lblNumeroGanador.BackColor = Color.FromArgb(12, 12, 14);
            else
                lblNumeroGanador.BackColor = Color.FromArgb(22, 163, 74);

            panelRueda.Invalidate();
        }

        private void panelRueda_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rueda = new Rectangle(32, 18, 248, 248);
            using (SolidBrush madera = new SolidBrush(Color.FromArgb(92, 48, 21)))
                e.Graphics.FillEllipse(madera, rueda);

            using (Pen bordeDorado = new Pen(Color.Gold, 6))
                e.Graphics.DrawEllipse(bordeDorado, rueda);

            Rectangle exterior = new Rectangle(56, 42, 200, 200);
            float angulo = -90f;
            for (int i = 0; i < 18; i++)
            {
                Color color = i % 2 == 0 ? Color.FromArgb(185, 28, 28) : Color.FromArgb(12, 12, 14);
                using (SolidBrush brush = new SolidBrush(color))
                    e.Graphics.FillPie(brush, exterior, angulo, 20f);
                angulo += 20f;
            }

            using (SolidBrush verde = new SolidBrush(Color.FromArgb(22, 163, 74)))
                e.Graphics.FillPie(verde, exterior, -100f, 20f);

            Rectangle centro = new Rectangle(112, 98, 88, 88);
            using (SolidBrush centroBrush = new SolidBrush(Color.FromArgb(239, 184, 16)))
                e.Graphics.FillEllipse(centroBrush, centro);
            using (Pen centroPen = new Pen(Color.WhiteSmoke, 4))
                e.Graphics.DrawEllipse(centroPen, centro);

            using (SolidBrush sombra = new SolidBrush(Color.FromArgb(28, Color.Black)))
                e.Graphics.FillEllipse(sombra, new Rectangle(78, 70, 156, 156));

            using (Pen divisor = new Pen(Color.FromArgb(230, 230, 230), 1))
            {
                Point centroPunto = new Point(156, 142);
                for (int i = 0; i < 18; i++)
                {
                    double rad = (-90 + i * 20) * Math.PI / 180.0;
                    Point borde = new Point(
                        centroPunto.X + (int)(99 * Math.Cos(rad)),
                        centroPunto.Y + (int)(99 * Math.Sin(rad)));
                    e.Graphics.DrawLine(divisor, centroPunto, borde);
                }
            }

            using (SolidBrush bola = new SolidBrush(Color.White))
                e.Graphics.FillEllipse(bola, new Rectangle(220, 80, 18, 18));
        }

        private void panelTapete_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            int x = 76;
            int y = 16;
            int w = 66;
            int h = 20;
            int numero = 1;

            using (Font font = new Font("Segoe UI", 10F, FontStyle.Bold))
            using (Pen borde = new Pen(Color.WhiteSmoke, 1))
            {
                for (int fila = 0; fila < 12; fila++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        int n = numero++;
                        Rectangle celda = new Rectangle(x + col * w, y + (11 - fila) * (h - 1), w, h);
                        Color fondo = ObtenerColor(n) == "Rojo" ? Color.FromArgb(185, 28, 28) : Color.FromArgb(12, 12, 14);
                        using (SolidBrush brush = new SolidBrush(fondo))
                            e.Graphics.FillRectangle(brush, celda);
                        e.Graphics.DrawRectangle(borde, celda);
                        TextRenderer.DrawText(e.Graphics, n.ToString(), font, celda, Color.White,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    }
                }
            }
        }

        private void ActualizarSaldo()
        {
            lblSaldo.Text = $"Saldo: ${_usuario.Saldo:N2}";
        }
    }
}
