using BLL;
using ENTITY;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace GUI
{
    public partial class UcInicio : UserControl
    {
        public event EventHandler JugarMinasSolicitado;
        public event EventHandler JugarRuletaSolicitado;
        public event EventHandler JugarSlotSolicitado;

        private readonly Usuario _usuario;
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();

        public UcInicio(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
            AplicarEstiloVisual();
            CargarDatos();
        }

        public UcInicio()
        {
            InitializeComponent();
        }

        public void RecargarSaldo()
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            if (_usuario == null) return;
            Usuario u = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario);
            if (u != null) _usuario.Saldo = u.Saldo;
            lblBienvenido.Text = $"Bienvenido, {_usuario.Nombre1} {_usuario.Apellido1}";
        }

        private void AplicarEstiloVisual()
        {
            CasinoTheme.StylePage(this);
            layoutInicio.BackColor = CasinoTheme.Page;
            panelHero.BackColor = CasinoTheme.Page;
            panelFooter.BackColor = CasinoTheme.Header;
            tlpStats.BackColor = CasinoTheme.Header;
            layoutInicio.Margin = Padding.Empty;
            panelHero.Margin = Padding.Empty;
            tlpStats.Margin = Padding.Empty;
            panelFooter.Margin = Padding.Empty;

            CrearElementosVisualesInicio();
            _badgeMinas.BringToFront();
            _badgeRuleta.BringToFront();
            _badgeSlot.BringToFront();

            CasinoTheme.StyleTitle(lblBienvenido, 28F);
            CasinoTheme.StyleLabel(lblDescripcion, CasinoTheme.Muted, 12F, FontStyle.Regular);
            CasinoTheme.StyleLabel(lblUsuarios, CasinoTheme.Gold, 14F, FontStyle.Bold);
            CasinoTheme.StyleLabel(lblPremios, CasinoTheme.Gold, 14F, FontStyle.Bold);
            CasinoTheme.StyleLabel(lblSoporte, CasinoTheme.Gold, 14F, FontStyle.Bold);
            lblBienvenido.AutoSize = false;
            lblDescripcion.AutoSize = false;
            lblUsuarios.Text = "3 juegos\nActivos";
            lblPremios.Text = "Saldo\nEn tiempo real";
            lblSoporte.Text = "Movimientos\nOrganizados";
            lblMinas.Text = "Minas";
            lblDescripcionMinas.Text = "";
            lblRuleta.Text = "Ruleta";
            lblDescripcionRuleta.Text = "";
            lblSlot.Text = "Tragamonedas";
            lblDescripcionSlot.Text = "";
            lblDescripcionMinas.Visible = false;
            lblDescripcionRuleta.Visible = false;
            lblDescripcionSlot.Visible = false;

            StyleGameCard(panelMinas, btnMinas, lblMinas, lblDescripcionMinas, CasinoTheme.Green);
            StyleGameCard(panelRuleta, btnRuleta, lblRuleta, lblDescripcionRuleta, CasinoTheme.Red);
            StyleGameCard(panelSlot, btnSlot, lblSlot, lblDescripcionSlot, CasinoTheme.Blue);

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.BackColor = Color.FromArgb(8, 47, 73);
            pictureBox2.BackColor = Color.FromArgb(49, 18, 77);
            pictureBox3.BackColor = Color.FromArgb(34, 18, 12);
            pictureBox1.Image = CargarImagenJuego("Assets\\minas.png", CrearImagenMinas);
            pictureBox3.Image = CargarImagenJuego("Assets\\ruleta.png", CrearImagenRuleta);
            pictureBox2.Image = CargarImagenJuego("Assets\\tragamonedas.png", CrearImagenTragamonedas);

            panelHero.Paint += (s, e) => CasinoTheme.DrawSubtleGradient(e.Graphics, panelHero.ClientRectangle);
            foreach (Panel panel in new[] { panelMinas, panelRuleta, panelSlot })
            {
                panel.Paint += GameCard_Paint;
                panel.Resize += (s, e) => ReubicarTarjeta((Panel)s);
            }
            tlpStats.Paint += Stats_Paint;

            Resize += (s, e) => ReubicarInicio();
            ReubicarInicio();
        }

        private void CrearElementosVisualesInicio()
        {
            if (_lblHeroTag == null) return;

            _lblHeroTag.BringToFront();
            _lblSeccionJuegos.BringToFront();
        }

        private void StyleGameCard(Panel panel, Button button, Label title, Label description, Color accent)
        {
            panel.BackColor = CasinoTheme.Surface;
            panel.BorderStyle = BorderStyle.None;
            panel.Padding = Padding.Empty;
            CasinoTheme.StyleActionButton(button, accent);
            CasinoTheme.StyleLabel(title, CasinoTheme.Gold, 15F, FontStyle.Bold);
            CasinoTheme.StyleLabel(description, CasinoTheme.Muted, 9.5F, FontStyle.Regular);
            button.Text = "Jugar ahora";
            button.Dock = DockStyle.None;
            title.Dock = DockStyle.None;
            description.Dock = DockStyle.None;

            PictureBox image = ObtenerImagenTarjeta(panel);
            if (image != null) image.Dock = DockStyle.None;
            ReubicarTarjeta(panel);
        }

        private void GameCard_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = new Rectangle(0, 0, panel.ClientSize.Width, panel.ClientSize.Height);
            using (LinearGradientBrush brush = new LinearGradientBrush(area,
                CasinoTheme.SurfaceAlt, CasinoTheme.Surface, 90F))
            {
                e.Graphics.FillRectangle(brush, area);
            }

            using (Pen pen = new Pen(CasinoTheme.Border))
                e.Graphics.DrawRectangle(pen, 0, 0, area.Width - 1, area.Height - 1);

            Color accent = panel == panelMinas ? CasinoTheme.Green : panel == panelRuleta ? CasinoTheme.Red : CasinoTheme.Blue;
            using (SolidBrush brush = new SolidBrush(accent))
                e.Graphics.FillRectangle(brush, 0, 0, panel.ClientSize.Width, 4);

            using (SolidBrush glow = new SolidBrush(Color.FromArgb(22, accent)))
                e.Graphics.FillRectangle(glow, 1, 4, panel.ClientSize.Width - 2, 72);
        }

        private void ReubicarInicio()
        {
            if (Width <= 0) return;

            int ancho = Width;
            bool compacto = ancho < 1050;

            int heroW = panelHero.ClientSize.Width;
            int margen = compacto ? 22 : 32;

            _lblHeroTag.SetBounds(margen, 24, 240, 20);
            lblBienvenido.SetBounds(margen, 50, Math.Max(360, heroW - margen * 2), 50);
            lblDescripcion.SetBounds(margen + 2, 106, Math.Max(360, Math.Min(760, heroW - margen * 2)), 58);
            lblDescripcion.MaximumSize = new Size(lblDescripcion.Width, 0);

            if (compacto)
            {
                layoutInicio.RowStyles[0].Height = 230;
                layoutInicio.RowStyles[2].Height = 76;
                tlpJuegos.Padding = new Padding(20, 24, 20, 14);
            }
            else
            {
                layoutInicio.RowStyles[0].Height = 220;
                layoutInicio.RowStyles[2].Height = 88;
                tlpJuegos.Padding = new Padding(34, 28, 34, 18);
            }

            ReubicarTarjeta(panelMinas);
            ReubicarTarjeta(panelRuleta);
            ReubicarTarjeta(panelSlot);
            panelHero.Invalidate();
            tlpStats.Invalidate();
        }

        private void ReubicarTarjeta(Panel panel)
        {
            if (panel.ClientSize.Width <= 0) return;

            Label titulo = panel == panelMinas ? lblMinas : panel == panelRuleta ? lblRuleta : lblSlot;
            Label descripcion = panel == panelMinas ? lblDescripcionMinas : panel == panelRuleta ? lblDescripcionRuleta : lblDescripcionSlot;
            Button boton = panel == panelMinas ? btnMinas : panel == panelRuleta ? btnRuleta : btnSlot;
            Label badge = panel == panelMinas ? _badgeMinas : panel == panelRuleta ? _badgeRuleta : _badgeSlot;
            PictureBox image = ObtenerImagenTarjeta(panel);
            if (badge == null || image == null) return;

            int pad = 18;
            titulo.SetBounds(pad, 16, panel.Width - pad * 2 - 94, 30);
            badge.SetBounds(panel.Width - pad - 86, 18, 86, 24);
            descripcion.Visible = false;
            image.SetBounds(pad, 62, panel.Width - pad * 2, Math.Max(120, panel.Height - 126));
            boton.SetBounds(pad, panel.Height - 48, panel.Width - pad * 2, 34);
        }

        private PictureBox ObtenerImagenTarjeta(Panel panel)
        {
            if (panel == panelMinas) return pictureBox1;
            if (panel == panelRuleta) return pictureBox3;
            if (panel == panelSlot) return pictureBox2;
            return null;
        }

        private Image CargarImagenJuego(string rutaRelativa, Func<Image> crearFallback)
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutaRelativa);
            if (File.Exists(ruta))
            {
                using (Image imagen = Image.FromFile(ruta))
                    return new Bitmap(imagen);
            }
            return crearFallback();
        }

        private Image CrearImagenMinas()
        {
            Bitmap bitmap = new Bitmap(900, 430);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                using (LinearGradientBrush fondo = new LinearGradientBrush(area,
                    Color.FromArgb(6, 44, 69), Color.FromArgb(13, 88, 120), 35F))
                {
                    g.FillRectangle(fondo, area);
                }
                using (Pen lineas = new Pen(Color.FromArgb(45, CasinoTheme.Cyan), 2))
                {
                    for (int x = 40; x < bitmap.Width; x += 76) g.DrawLine(lineas, x, 0, x - 160, bitmap.Height);
                    for (int y = 30; y < bitmap.Height; y += 54) g.DrawLine(lineas, 0, y, bitmap.Width, y + 90);
                }
                int celda = 62;
                int sep = 12;
                int startX = 220;
                int startY = 74;
                for (int fila = 0; fila < 4; fila++)
                    for (int col = 0; col < 6; col++)
                    {
                        Rectangle r = new Rectangle(startX + col * (celda + sep), startY + fila * (celda + sep), celda, celda);
                        using (LinearGradientBrush b = new LinearGradientBrush(r, Color.FromArgb(52, 95, 130), Color.FromArgb(23, 47, 78), 90F))
                            g.FillRectangle(b, r);
                        using (Pen p = new Pen(Color.FromArgb(120, CasinoTheme.Cyan), 2))
                            g.DrawRectangle(p, r);
                    }
                DibujarMinaLobby(g, new Point(450, 208), 54);
                DibujarDiamanteLobby(g, new Rectangle(300, 150, 54, 54), CasinoTheme.Cyan);
                DibujarDiamanteLobby(g, new Rectangle(594, 224, 54, 54), CasinoTheme.Gold);
                using (Font title = CasinoTheme.TitleFont(38F))
                    TextRenderer.DrawText(g, "MINES", title, new Rectangle(40, 54, 180, 70), Color.White,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                using (Font sub = CasinoTheme.UiFont(15F, FontStyle.Bold))
                    TextRenderer.DrawText(g, "Encuentra gemas, evita minas", sub, new Rectangle(42, 128, 210, 60), CasinoTheme.Muted,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.WordBreak);
            }
            return bitmap;
        }

        private Image CrearImagenRuleta()
        {
            Bitmap bitmap = new Bitmap(900, 430);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                using (LinearGradientBrush fondo = new LinearGradientBrush(area, Color.FromArgb(28, 16, 8), Color.FromArgb(4, 84, 52), 20F))
                    g.FillRectangle(fondo, area);
                Rectangle rueda = new Rectangle(54, 48, 330, 330);
                using (SolidBrush madera = new SolidBrush(Color.FromArgb(95, 49, 18)))
                    g.FillEllipse(madera, rueda);
                using (Pen dorado = new Pen(CasinoTheme.Gold, 12))
                    g.DrawEllipse(dorado, rueda);
                Rectangle inner = new Rectangle(104, 98, 230, 230);
                for (int i = 0; i < 18; i++)
                {
                    using (SolidBrush b = new SolidBrush(i % 2 == 0 ? CasinoTheme.Red : Color.FromArgb(8, 10, 14)))
                        g.FillPie(b, inner, i * 20, 20);
                }
                using (SolidBrush green = new SolidBrush(Color.FromArgb(22, 163, 74)))
                    g.FillPie(green, inner, 260, 20);
                using (SolidBrush centro = new SolidBrush(Color.FromArgb(225, 164, 20)))
                    g.FillEllipse(centro, new Rectangle(176, 170, 86, 86));
                using (Pen blanco = new Pen(Color.White, 2))
                {
                    for (int i = 0; i < 18; i++)
                    {
                        float ang = (float)(Math.PI * 2 * i / 18);
                        Point c = new Point(219, 213);
                        g.DrawLine(blanco, c.X, c.Y, c.X + (int)(115 * Math.Cos(ang)), c.Y + (int)(115 * Math.Sin(ang)));
                    }
                }
                using (SolidBrush ball = new SolidBrush(Color.White))
                    g.FillEllipse(ball, new Rectangle(298, 122, 28, 28));
                Rectangle mesa = new Rectangle(430, 72, 400, 272);
                using (SolidBrush tapete = new SolidBrush(Color.FromArgb(6, 95, 70)))
                    g.FillRectangle(tapete, mesa);
                using (Pen borde = new Pen(Color.FromArgb(148, 163, 184), 2))
                    g.DrawRectangle(borde, mesa);
                int cellW = 88;
                int cellH = 44;
                int n = 1;
                for (int col = 0; col < 4; col++)
                    for (int row = 0; row < 3; row++)
                    {
                        Rectangle cell = new Rectangle(510 + col * cellW, 98 + row * cellH, cellW, cellH);
                        using (SolidBrush b = new SolidBrush((n + row) % 2 == 0 ? Color.FromArgb(12, 12, 14) : CasinoTheme.Red))
                            g.FillRectangle(b, cell);
                        using (Pen p = new Pen(Color.White, 1))
                            g.DrawRectangle(p, cell);
                        using (Font f = CasinoTheme.UiFont(16F, FontStyle.Bold))
                            TextRenderer.DrawText(g, n.ToString(), f, cell, Color.White,
                                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                        n += 3;
                    }
                using (SolidBrush cero = new SolidBrush(Color.FromArgb(22, 163, 74)))
                    g.FillRectangle(cero, new Rectangle(450, 98, 48, cellH * 3));
                using (Font f = CasinoTheme.UiFont(24F, FontStyle.Bold))
                    TextRenderer.DrawText(g, "0", f, new Rectangle(450, 98, 48, cellH * 3), Color.White,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                using (Font title = CasinoTheme.TitleFont(30F))
                    TextRenderer.DrawText(g, "RULETA", title, new Rectangle(500, 236, 260, 52), CasinoTheme.Gold,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
            return bitmap;
        }

        private Image CrearImagenTragamonedas()
        {
            Bitmap bitmap = new Bitmap(900, 430);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                using (LinearGradientBrush fondo = new LinearGradientBrush(area, Color.FromArgb(88, 28, 135), Color.FromArgb(14, 22, 42), 45F))
                    g.FillRectangle(fondo, area);
                Rectangle maquina = new Rectangle(90, 48, 720, 326);
                using (SolidBrush body = new SolidBrush(Color.FromArgb(76, 29, 149)))
                    g.FillRectangle(body, maquina);
                using (Pen gold = new Pen(CasinoTheme.Gold, 8))
                    g.DrawRectangle(gold, maquina);
                using (Font title = CasinoTheme.TitleFont(32F))
                    TextRenderer.DrawText(g, "FORTUNE SPINS", title, new Rectangle(120, 68, 660, 54), CasinoTheme.Gold,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                int rollW = 170;
                int rollH = 150;
                int startX = 180;
                for (int i = 0; i < 3; i++)
                {
                    Rectangle roll = new Rectangle(startX + i * (rollW + 22), 148, rollW, rollH);
                    using (LinearGradientBrush b = new LinearGradientBrush(roll, Color.White, Color.FromArgb(226, 232, 240), 90F))
                        g.FillRectangle(b, roll);
                    using (Pen p = new Pen(Color.FromArgb(203, 213, 225), 4))
                        g.DrawRectangle(p, roll);
                }
                using (Font seven = CasinoTheme.TitleFont(54F))
                    TextRenderer.DrawText(g, "7", seven, new Rectangle(startX, 148, rollW, rollH), CasinoTheme.Red,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                DibujarDiamanteLobby(g, new Rectangle(startX + rollW + 58, 188, 90, 70), CasinoTheme.Cyan);
                DibujarMonedaLobby(g, new Rectangle(startX + (rollW + 22) * 2 + 45, 178, 84, 84));
                using (SolidBrush boton = new SolidBrush(CasinoTheme.Green))
                    g.FillRectangle(boton, new Rectangle(330, 324, 240, 38));
                using (Font f = CasinoTheme.UiFont(16F, FontStyle.Bold))
                    TextRenderer.DrawText(g, "GIRAR", f, new Rectangle(330, 324, 240, 38), Color.White,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                using (SolidBrush luz = new SolidBrush(Color.FromArgb(180, CasinoTheme.Gold)))
                {
                    for (int x = 120; x < 780; x += 42)
                    {
                        g.FillEllipse(luz, new Rectangle(x, 384, 12, 12));
                        g.FillEllipse(luz, new Rectangle(x, 32, 12, 12));
                    }
                }
            }
            return bitmap;
        }

        private void DibujarMinaLobby(Graphics g, Point centro, int radio)
        {
            using (SolidBrush sombra = new SolidBrush(Color.FromArgb(70, Color.Black)))
                g.FillEllipse(sombra, centro.X - radio + 8, centro.Y - radio + 12, radio * 2, radio * 2);
            using (SolidBrush mina = new SolidBrush(Color.FromArgb(15, 23, 42)))
                g.FillEllipse(mina, centro.X - radio, centro.Y - radio, radio * 2, radio * 2);
            using (Pen pin = new Pen(Color.FromArgb(30, 41, 59), 8))
            {
                for (int i = 0; i < 8; i++)
                {
                    float ang = (float)(Math.PI * 2 * i / 8);
                    g.DrawLine(pin, centro.X, centro.Y,
                        centro.X + (int)((radio + 20) * Math.Cos(ang)),
                        centro.Y + (int)((radio + 20) * Math.Sin(ang)));
                }
            }
            using (SolidBrush brillo = new SolidBrush(CasinoTheme.Red))
                g.FillEllipse(brillo, centro.X - 20, centro.Y - 20, 40, 40);
        }

        private void DibujarDiamanteLobby(Graphics g, Rectangle area, Color color)
        {
            Point[] puntos =
            {
                new Point(area.X + area.Width / 2, area.Y),
                new Point(area.Right, area.Y + area.Height / 2),
                new Point(area.X + area.Width / 2, area.Bottom),
                new Point(area.X, area.Y + area.Height / 2)
            };
            using (SolidBrush b = new SolidBrush(color))
                g.FillPolygon(b, puntos);
            using (Pen p = new Pen(Color.White, 2))
                g.DrawPolygon(p, puntos);
        }

        private void DibujarMonedaLobby(Graphics g, Rectangle area)
        {
            using (SolidBrush b = new SolidBrush(CasinoTheme.Gold))
                g.FillEllipse(b, area);
            using (Pen p = new Pen(Color.FromArgb(180, 83, 9), 5))
                g.DrawEllipse(p, area);
            using (Font f = CasinoTheme.UiFont(28F, FontStyle.Bold))
                TextRenderer.DrawText(g, "$", f, area, Color.FromArgb(120, 53, 15),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void Stats_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.FromArgb(30, 41, 59)))
            {
                int col = tlpStats.Width / 3;
                e.Graphics.DrawLine(pen, col, 14, col, tlpStats.Height - 14);
                e.Graphics.DrawLine(pen, col * 2, 14, col * 2, tlpStats.Height - 14);
            }
        }

        private void btnMinas_Click(object sender, EventArgs e)
        {
            JugarMinasSolicitado?.Invoke(this, EventArgs.Empty);
        }

        private void btnRuleta_Click(object sender, EventArgs e)
        {
            JugarRuletaSolicitado?.Invoke(this, EventArgs.Empty);
        }

        private void btnSlot_Click(object sender, EventArgs e)
        {
            JugarSlotSolicitado?.Invoke(this, EventArgs.Empty);
        }
    }
}
