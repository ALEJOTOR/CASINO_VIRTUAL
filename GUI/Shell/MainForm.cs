using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class MainForm : Form
    {
        private readonly Usuario _usuario;
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();
        private readonly PartidaServicio _partidaSvc = new PartidaServicio();
        private Control _vistaInicio;

        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            AplicarEstiloVisual();
            PrepararNavegacion();
        }

        public MainForm(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            AplicarEstiloVisual();
            PrepararNavegacion();
            cargarDatos();
        }

        private void btnJugarAhora_Click(object sender, EventArgs e)
        {
            MessageBox.Show("¡Jugar ahora!");
        }

        private void btnMinas_Click(object sender, EventArgs e)
        {
            UcMinas vista = new UcMinas(_usuario);
            vista.SaldoActualizado += (s, args) => cargarDatos();
            MostrarVista(vista);
        }

        private void btnRuleta_Click(object sender, EventArgs e)
        {
            UcRuleta vista = new UcRuleta(_usuario);
            vista.SaldoActualizado += (s, args) => cargarDatos();
            MostrarVista(vista);
        }

        private void btnSlot_Click(object sender, EventArgs e)
        {
            UcTragamonedas vista = new UcTragamonedas(_usuario);
            vista.SaldoActualizado += (s, args) => cargarDatos();
            MostrarVista(vista);
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            new FrmLogin().Show();
            this.Close();
        }

        private void cargarDatos()
        {
            if (_usuario == null) return;

            Usuario u = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario);
            if (u != null) _usuario.Saldo = u.Saldo;
            lblBienvenido.Text = $"Bienvenido, {_usuario.Nombre1} {_usuario.Apellido1}";
            lblSaldo.Text = $"Saldo: ${_usuario.Saldo:N2}";
        }

        private void PrepararNavegacion()
        {
            _vistaInicio = layoutInicio;
            inicioToolStripMenuItem.Click += (s, e) => MostrarInicio();
            historialToolStripMenuItem.Click += (s, e) => MostrarHistorial();
            billeteraToolStripMenuItem.Click += (s, e) => MostrarBilletera();
        }

        private void AplicarEstiloVisual()
        {
            CasinoTheme.StylePage(this);
            CasinoTheme.StyleHeader(panelNavbar);
            CasinoTheme.StyleActionButton(btnCerrarSesion, CasinoTheme.Red);

            mainLayout.BackColor = CasinoTheme.Page;
            pnlContenido.BackColor = CasinoTheme.Page;
            layoutInicio.BackColor = CasinoTheme.Page;
            panelHero.BackColor = CasinoTheme.Page;
            panelFooter.BackColor = CasinoTheme.Header;
            tlpStats.BackColor = CasinoTheme.Header;
            mainLayout.Margin = Padding.Empty;
            panelNavbar.Margin = Padding.Empty;
            pnlContenido.Margin = Padding.Empty;
            layoutInicio.Margin = Padding.Empty;
            panelHero.Margin = Padding.Empty;
            tlpStats.Margin = Padding.Empty;
            panelFooter.Margin = Padding.Empty;

            CrearElementosVisualesInicio();
            _lblMarca.BringToFront();
            lblSaldo.BringToFront();
            btnCerrarSesion.BringToFront();
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

            lblSaldo.BackColor = CasinoTheme.SurfaceAlt;
            lblSaldo.ForeColor = CasinoTheme.Green;
            lblSaldo.Font = CasinoTheme.UiFont(12F, FontStyle.Bold);
            lblSaldo.AutoSize = false;
            lblSaldo.TextAlign = ContentAlignment.MiddleRight;

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
            panelNavbar.Paint += Navbar_Paint;

            menuStrip.RenderMode = ToolStripRenderMode.Professional;
            menuStrip.Renderer = new ToolStripProfessionalRenderer(new CasinoMenuColors());
            foreach (ToolStripMenuItem item in menuStrip.Items)
            {
                item.Font = CasinoTheme.UiFont(11F, FontStyle.Bold);
                item.ForeColor = CasinoTheme.Text;
                item.Margin = new Padding(6, 0, 6, 0);
            }

            Resize += (s, e) => ReubicarInicio();
            ReubicarInicio();
        }

        private void CrearElementosVisualesInicio()
        {
            if (_lblMarca != null) return;

            _lblMarca = CrearLabel("CASINO ROYAL", CasinoTheme.Gold, CasinoTheme.TitleFont(18F), ContentAlignment.MiddleLeft);
            panelNavbar.Controls.Add(_lblMarca);
            _lblMarca.BringToFront();

            _lblHeroTag = CrearLabel("LOBBY PRINCIPAL", CasinoTheme.Cyan, CasinoTheme.UiFont(9F, FontStyle.Bold), ContentAlignment.MiddleLeft);
            _lblSeccionJuegos = CrearLabel("", CasinoTheme.Text, CasinoTheme.UiFont(16F, FontStyle.Bold), ContentAlignment.MiddleLeft);
            _lblSeccionJuegos.Visible = false;

            panelHero.Controls.Add(_lblHeroTag);
            panelHero.Controls.Add(_lblSeccionJuegos);

            _badgeMinas = CrearBadge("Estrategia");
            _badgeRuleta = CrearBadge("Mesa real");
            _badgeSlot = CrearBadge("Jackpot");
            panelMinas.Controls.Add(_badgeMinas);
            panelRuleta.Controls.Add(_badgeRuleta);
            panelSlot.Controls.Add(_badgeSlot);
            _badgeMinas.BringToFront();
            _badgeRuleta.BringToFront();
            _badgeSlot.BringToFront();
        }

        private Label CrearLabel(string texto, Color color, Font fuente, ContentAlignment alineacion)
        {
            return new Label
            {
                AutoSize = false,
                BackColor = Color.Transparent,
                Font = fuente,
                ForeColor = color,
                Text = texto,
                TextAlign = alineacion
            };
        }

        private Label CrearBadge(string texto)
        {
            return new Label
            {
                AutoSize = false,
                BackColor = Color.FromArgb(30, 41, 59),
                Font = CasinoTheme.UiFont(8.5F, FontStyle.Bold),
                ForeColor = CasinoTheme.Cyan,
                Text = texto,
                TextAlign = ContentAlignment.MiddleCenter
            };
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
            if (pnlContenido.ClientSize.Width <= 0) return;

            int ancho = pnlContenido.ClientSize.Width;
            bool compacto = ancho < 1050;

            layoutInicio.RowStyles[0].Height = compacto ? 230 : 220;
            layoutInicio.RowStyles[2].Height = compacto ? 76 : 88;
            tlpJuegos.Padding = compacto ? new Padding(20, 24, 20, 14) : new Padding(34, 28, 34, 18);

            int heroW = panelHero.ClientSize.Width;
            int margen = compacto ? 22 : 32;

            _lblHeroTag.SetBounds(margen, 24, 240, 20);
            lblBienvenido.SetBounds(margen, 50, Math.Max(360, heroW - margen * 2), 50);
            lblDescripcion.SetBounds(margen + 2, 106, Math.Max(360, Math.Min(760, heroW - margen * 2)), 58);
            lblDescripcion.MaximumSize = new Size(lblDescripcion.Width, 0);

            _lblMarca.SetBounds(18, 0, 170, panelNavbar.Height);
            menuStrip.Padding = new Padding(190, 0, 0, 0);
            lblSaldo.SetBounds(Math.Max(420, panelNavbar.Width - 304), 10, 174, 40);
            btnCerrarSesion.SetBounds(panelNavbar.Width - 120, 12, 106, 30);

            ReubicarTarjeta(panelMinas);
            ReubicarTarjeta(panelRuleta);
            ReubicarTarjeta(panelSlot);
            panelHero.Invalidate();
            panelNavbar.Invalidate();
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
                {
                    for (int col = 0; col < 6; col++)
                    {
                        Rectangle r = new Rectangle(startX + col * (celda + sep), startY + fila * (celda + sep), celda, celda);
                        using (LinearGradientBrush b = new LinearGradientBrush(r,
                            Color.FromArgb(52, 95, 130), Color.FromArgb(23, 47, 78), 90F))
                            g.FillRectangle(b, r);
                        using (Pen p = new Pen(Color.FromArgb(120, CasinoTheme.Cyan), 2))
                            g.DrawRectangle(p, r);
                    }
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
                using (LinearGradientBrush fondo = new LinearGradientBrush(area,
                    Color.FromArgb(28, 16, 8), Color.FromArgb(4, 84, 52), 20F))
                {
                    g.FillRectangle(fondo, area);
                }

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
                {
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
                using (LinearGradientBrush fondo = new LinearGradientBrush(area,
                    Color.FromArgb(88, 28, 135), Color.FromArgb(14, 22, 42), 45F))
                {
                    g.FillRectangle(fondo, area);
                }

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

        private void Navbar_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.FromArgb(30, 41, 59)))
                e.Graphics.DrawLine(pen, 0, panelNavbar.Height - 1, panelNavbar.Width, panelNavbar.Height - 1);
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

        private sealed class CasinoMenuColors : ProfessionalColorTable
        {
            public override Color MenuItemSelected => CasinoTheme.SurfaceAlt;
            public override Color MenuItemBorder => CasinoTheme.Border;
            public override Color ToolStripDropDownBackground => CasinoTheme.Header;
            public override Color ImageMarginGradientBegin => CasinoTheme.Header;
            public override Color ImageMarginGradientMiddle => CasinoTheme.Header;
            public override Color ImageMarginGradientEnd => CasinoTheme.Header;
        }

        private Panel CrearVistaBase(string titulo, string subtitulo)
        {
            Panel vista = new Panel
            {
                BackColor = CasinoTheme.Page,
                Dock = DockStyle.Fill,
                Padding = new Padding(34, 28, 34, 34)
            };

            Label lblTitulo = CrearLabel(titulo, CasinoTheme.Gold, CasinoTheme.TitleFont(26F), ContentAlignment.MiddleLeft);
            Label lblSubtitulo = CrearLabel(subtitulo, CasinoTheme.Muted, CasinoTheme.UiFont(11F), ContentAlignment.MiddleLeft);

            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Height = 44;
            lblSubtitulo.Dock = DockStyle.Top;
            lblSubtitulo.Height = 32;

            vista.Controls.Add(lblSubtitulo);
            vista.Controls.Add(lblTitulo);
            lblTitulo.BringToFront();
            lblSubtitulo.BringToFront();

            return vista;
        }

        private Panel CrearTarjetaCatalogo(string titulo, string descripcion, Image imagen, Color acento, EventHandler click)
        {
            Panel tarjeta = new Panel
            {
                BackColor = CasinoTheme.Surface,
                Dock = DockStyle.Fill,
                Margin = new Padding(12),
                Padding = new Padding(20)
            };
            tarjeta.Paint += (s, e) =>
            {
                Rectangle area = new Rectangle(0, 0, tarjeta.Width, tarjeta.Height);
                CasinoTheme.DrawBorderedPanel(e.Graphics, area, CasinoTheme.Surface, CasinoTheme.Border);
                using (SolidBrush brush = new SolidBrush(acento))
                    e.Graphics.FillRectangle(brush, 0, 0, tarjeta.Width, 5);
            };

            Label lblTitulo = CrearLabel(titulo, CasinoTheme.Gold, CasinoTheme.UiFont(18F, FontStyle.Bold), ContentAlignment.MiddleLeft);
            Label lblDescripcion = CrearLabel(descripcion, CasinoTheme.Muted, CasinoTheme.UiFont(10F), ContentAlignment.TopLeft);
            PictureBox picture = new PictureBox
            {
                Image = imagen,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(11, 18, 32)
            };
            Button boton = new Button { Text = "Entrar al juego" };
            CasinoTheme.StyleActionButton(boton, acento);
            boton.Click += click;

            tarjeta.Controls.Add(lblTitulo);
            tarjeta.Controls.Add(lblDescripcion);
            tarjeta.Controls.Add(picture);
            tarjeta.Controls.Add(boton);
            tarjeta.Resize += (s, e) =>
            {
                int pad = 20;
                lblTitulo.SetBounds(pad, 18, tarjeta.Width - pad * 2, 34);
                lblDescripcion.SetBounds(pad, 58, tarjeta.Width - pad * 2, 56);
                picture.SetBounds(pad, 126, tarjeta.Width - pad * 2, Math.Max(180, tarjeta.Height - 204));
                boton.SetBounds(pad, tarjeta.Height - 58, tarjeta.Width - pad * 2, 38);
            };

            return tarjeta;
        }

        private Panel CrearTarjetaBilletera(string titulo, string descripcion, string etiquetaMonto, string textoBoton, Color acento, Func<decimal, string> accion)
        {
            Panel tarjeta = new Panel
            {
                BackColor = CasinoTheme.Surface,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 14),
                Padding = new Padding(24)
            };
            tarjeta.Paint += (s, e) =>
            {
                CasinoTheme.DrawBorderedPanel(e.Graphics, new Rectangle(0, 0, tarjeta.Width, tarjeta.Height), CasinoTheme.Surface, CasinoTheme.Border);
                using (SolidBrush brush = new SolidBrush(acento))
                    e.Graphics.FillRectangle(brush, 0, 0, tarjeta.Width, 6);

                using (SolidBrush brush = new SolidBrush(Color.FromArgb(24, acento)))
                    e.Graphics.FillRectangle(brush, 1, 6, tarjeta.Width - 2, 54);
            };

            Label lblTitulo = CrearLabel(titulo, CasinoTheme.Gold, CasinoTheme.UiFont(16F, FontStyle.Bold), ContentAlignment.MiddleLeft);
            Label lblDescripcion = CrearLabel(descripcion, CasinoTheme.Muted, CasinoTheme.UiFont(10F), ContentAlignment.MiddleLeft);
            Label lblMonto = CrearLabel(etiquetaMonto, CasinoTheme.Text, CasinoTheme.UiFont(9.5F, FontStyle.Bold), ContentAlignment.MiddleLeft);
            TextBox txtMonto = new TextBox();
            Button btnAccion = new Button { Text = textoBoton };

            CasinoTheme.StyleInput(txtMonto);
            txtMonto.BackColor = CasinoTheme.SurfaceAlt;
            txtMonto.ForeColor = CasinoTheme.Text;
            txtMonto.BorderStyle = BorderStyle.FixedSingle;
            txtMonto.Font = CasinoTheme.UiFont(13F, FontStyle.Bold);
            CasinoTheme.StyleActionButton(btnAccion, acento);

            btnAccion.Click += (s, e) =>
            {
                if (!decimal.TryParse(txtMonto.Text, out decimal monto) || monto <= 0)
                {
                    MessageBox.Show("Ingrese un monto valido.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnAccion.Enabled = false;
                try
                {
                    string resultado = accion(monto);
                    bool ok = resultado == "Deposito realizado correctamente." ||
                              resultado == "Guardado correctamente.";

                    MessageBox.Show(resultado, ok ? "Exito" : "Error",
                        MessageBoxButtons.OK,
                        ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                    if (ok)
                    {
                        txtMonto.Clear();
                        cargarDatos();
                        MostrarBilletera();
                    }
                }
                finally
                {
                    btnAccion.Enabled = true;
                }
            };

            tarjeta.Controls.Add(lblTitulo);
            tarjeta.Controls.Add(lblDescripcion);
            tarjeta.Controls.Add(lblMonto);
            tarjeta.Controls.Add(txtMonto);
            tarjeta.Controls.Add(btnAccion);
            tarjeta.Resize += (s, e) =>
            {
                int pad = 24;
                lblTitulo.SetBounds(pad, 24, tarjeta.Width - pad * 2, 30);
                lblDescripcion.SetBounds(pad, 62, tarjeta.Width - pad * 2, 38);
                lblMonto.SetBounds(pad, 118, tarjeta.Width - pad * 2, 20);
                txtMonto.SetBounds(pad, 146, Math.Min(300, tarjeta.Width - pad * 2), 36);
                btnAccion.SetBounds(pad, 198, Math.Min(220, tarjeta.Width - pad * 2), 42);
            };

            return tarjeta;
        }

        private Panel CrearMetricaBilletera(string titulo, string valor, string detalle, Color acento)
        {
            Panel tarjeta = new Panel
            {
                BackColor = CasinoTheme.Surface,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 14, 0)
            };
            tarjeta.Paint += (s, e) =>
            {
                CasinoTheme.DrawBorderedPanel(e.Graphics, new Rectangle(0, 0, tarjeta.Width, tarjeta.Height), CasinoTheme.Surface, CasinoTheme.Border);
                using (SolidBrush brush = new SolidBrush(acento))
                    e.Graphics.FillRectangle(brush, 0, 0, tarjeta.Width, 5);
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(18, acento)))
                    e.Graphics.FillRectangle(brush, 1, 5, tarjeta.Width - 2, tarjeta.Height - 6);
            };

            Label lblTitulo = CrearLabel(titulo, CasinoTheme.Muted, CasinoTheme.UiFont(9.5F, FontStyle.Bold), ContentAlignment.MiddleLeft);
            Label lblValor = CrearLabel(valor, acento, CasinoTheme.UiFont(20F, FontStyle.Bold), ContentAlignment.MiddleLeft);
            Label lblDetalle = CrearLabel(detalle, CasinoTheme.Muted, CasinoTheme.UiFont(9F), ContentAlignment.MiddleLeft);

            tarjeta.Controls.Add(lblTitulo);
            tarjeta.Controls.Add(lblValor);
            tarjeta.Controls.Add(lblDetalle);
            tarjeta.Resize += (s, e) =>
            {
                int pad = 18;
                lblTitulo.SetBounds(pad, 18, tarjeta.Width - pad * 2, 22);
                lblValor.SetBounds(pad, 44, tarjeta.Width - pad * 2, 38);
                lblDetalle.SetBounds(pad, 86, tarjeta.Width - pad * 2, 24);
            };

            return tarjeta;
        }

        private DataGridView CrearGridHistorial()
        {
            DataGridView grid = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = CasinoTheme.Page,
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = CasinoTheme.SurfaceAlt;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = CasinoTheme.Text;
            grid.ColumnHeadersDefaultCellStyle.Font = CasinoTheme.UiFont(10F, FontStyle.Bold);
            grid.DefaultCellStyle.BackColor = CasinoTheme.Surface;
            grid.DefaultCellStyle.ForeColor = CasinoTheme.Text;
            grid.DefaultCellStyle.SelectionBackColor = CasinoTheme.Blue;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(13, 23, 42);
            grid.GridColor = CasinoTheme.Border;
            return grid;
        }

        private TabPage CrearTab(string texto, Control contenido)
        {
            TabPage tab = new TabPage(texto)
            {
                BackColor = CasinoTheme.Page,
                Padding = new Padding(8)
            };
            tab.Controls.Add(contenido);
            return tab;
        }

        private void AjustarGridPartidas(DataGridView grid)
        {
            if (grid.Columns.Count == 0) return;

            grid.Columns["IdPartida"].HeaderText = "Partida";
            grid.Columns["IdUsuario"].Visible = false;
            grid.Columns["NombreJuego"].HeaderText = "Juego";
            grid.Columns["Estado"].HeaderText = "Estado";
            grid.Columns["Fecha"].HeaderText = "Fecha";
            grid.Columns["Apuesta"].HeaderText = "Apuesta";
            grid.Columns["Ganancia"].HeaderText = "Ganancia";
            grid.Columns["Resultado"].HeaderText = "Resultado";
            grid.Columns["Apuesta"].DefaultCellStyle.Format = "C2";
            grid.Columns["Ganancia"].DefaultCellStyle.Format = "C2";
            grid.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        }

        private void AjustarGridTransacciones(DataGridView grid)
        {
            if (grid.Columns.Count == 0) return;

            grid.Columns["IdTransaccion"].HeaderText = "Movimiento";
            grid.Columns["IdUsuario"].Visible = false;
            grid.Columns["Tipo"].HeaderText = "Tipo";
            grid.Columns["Monto"].HeaderText = "Monto";
            grid.Columns["Fecha"].HeaderText = "Fecha";
            grid.Columns["Descripcion"].HeaderText = "Descripcion";
            grid.Columns["Monto"].DefaultCellStyle.Format = "C2";
            grid.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        }

        private Button CrearBotonLateral(string texto)
        {
            Button boton = new Button
            {
                BackColor = CasinoTheme.Surface,
                Dock = DockStyle.Top,
                FlatStyle = FlatStyle.Flat,
                Font = CasinoTheme.UiFont(10F, FontStyle.Bold),
                ForeColor = CasinoTheme.Text,
                Height = 48,
                Text = texto,
                TextAlign = ContentAlignment.MiddleLeft,
                UseVisualStyleBackColor = false
            };
            boton.FlatAppearance.BorderSize = 0;
            boton.Padding = new Padding(18, 0, 0, 0);
            return boton;
        }

        private void CargarVistaTransacciones(Panel contenedor, string categoria, int dias)
        {
            contenedor.Controls.Clear();

            Panel marco = new Panel
            {
                BackColor = CasinoTheme.Surface,
                Dock = DockStyle.Fill,
                Padding = new Padding(16)
            };
            marco.Paint += (s, e) => CasinoTheme.DrawBorderedPanel(e.Graphics,
                new Rectangle(0, 0, marco.Width, marco.Height), CasinoTheme.Surface, CasinoTheme.Border);

            Panel filtros = new Panel
            {
                Dock = DockStyle.Top,
                Height = 128,
                BackColor = Color.FromArgb(13, 26, 42),
                Padding = new Padding(14)
            };

            Button btn7 = CrearChipPeriodo("7 dias", dias == 7);
            Button btn14 = CrearChipPeriodo("14 dias", dias == 14);
            Button btn30 = CrearChipPeriodo("30 dias", dias == 30);
            ComboBox cboFiltro = CrearComboFiltro();
            TextBox desde = CrearInputFecha();
            TextBox hasta = CrearInputFecha();
            Button buscar = new Button { Text = "Buscar" };
            Label lblPeriodo = CrearLabel("Periodo", CasinoTheme.Muted, CasinoTheme.UiFont(8.5F, FontStyle.Bold), ContentAlignment.MiddleLeft);
            Label lblFiltro = CrearLabel("Filtro", CasinoTheme.Muted, CasinoTheme.UiFont(8.5F, FontStyle.Bold), ContentAlignment.MiddleLeft);
            Label lblDesde = CrearLabel("Desde", CasinoTheme.Muted, CasinoTheme.UiFont(8.5F, FontStyle.Bold), ContentAlignment.MiddleLeft);
            Label lblHasta = CrearLabel("Hasta", CasinoTheme.Muted, CasinoTheme.UiFont(8.5F, FontStyle.Bold), ContentAlignment.MiddleLeft);

            CasinoTheme.StyleActionButton(buscar, CasinoTheme.SurfaceAlt);
            CargarOpcionesFiltro(cboFiltro, categoria);
            hasta.Text = DateTime.Today.ToString("dd/MM/yyyy");
            desde.Text = DateTime.Today.AddDays(-dias).ToString("dd/MM/yyyy");

            FlowLayoutPanel chips = new FlowLayoutPanel
            {
                BackColor = Color.Transparent,
                Location = new Point(0, 18),
                Size = new Size(360, 50),
                WrapContents = false,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            chips.Controls.Add(btn7);
            chips.Controls.Add(btn14);
            chips.Controls.Add(btn30);

            filtros.Controls.Add(lblPeriodo);
            filtros.Controls.Add(lblFiltro);
            filtros.Controls.Add(lblDesde);
            filtros.Controls.Add(lblHasta);
            filtros.Controls.Add(chips);
            filtros.Controls.Add(cboFiltro);
            filtros.Controls.Add(desde);
            filtros.Controls.Add(hasta);
            filtros.Controls.Add(buscar);
            filtros.Resize += (s, e) =>
            {
                UbicarFiltrosTransacciones(filtros, lblPeriodo, chips, lblFiltro, cboFiltro, lblDesde, desde, lblHasta, hasta, buscar);
            };
            UbicarFiltrosTransacciones(filtros, lblPeriodo, chips, lblFiltro, cboFiltro, lblDesde, desde, lblHasta, hasta, buscar);

            Panel lista = new Panel
            {
                AutoScroll = true,
                BackColor = CasinoTheme.Surface,
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 12, 0, 0)
            };

            Action cargarLista = () =>
            {
                if (!DateTime.TryParse(desde.Text, out DateTime fechaDesde) ||
                    !DateTime.TryParse(hasta.Text, out DateTime fechaHasta))
                {
                    MessageBox.Show("Ingrese fechas validas con formato dia/mes/año.", "Fechas invalidas",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                PintarListaMovimientos(lista, categoria, cboFiltro.Text, fechaDesde.Date, fechaHasta.Date.AddDays(1).AddTicks(-1));
            };
            buscar.Click += (s, e) => cargarLista();
            btn7.Click += (s, e) => CargarVistaTransacciones(contenedor, categoria, 7);
            btn14.Click += (s, e) => CargarVistaTransacciones(contenedor, categoria, 14);
            btn30.Click += (s, e) => CargarVistaTransacciones(contenedor, categoria, 30);

            marco.Controls.Add(lista);
            marco.Controls.Add(filtros);
            contenedor.Controls.Add(marco);
            cargarLista();
        }

        private Button CrearChipPeriodo(string texto, bool activo)
        {
            Button chip = new Button
            {
                BackColor = activo ? CasinoTheme.SurfaceAlt : Color.FromArgb(14, 27, 43),
                FlatStyle = FlatStyle.Flat,
                Font = CasinoTheme.UiFont(9.5F, FontStyle.Bold),
                ForeColor = CasinoTheme.Text,
                Height = 40,
                Width = 96,
                Text = texto,
                UseVisualStyleBackColor = false
            };
            chip.FlatAppearance.BorderSize = 0;
            chip.Margin = new Padding(0, 0, 10, 0);
            return chip;
        }

        private TextBox CrearInputFecha()
        {
            TextBox input = new TextBox
            {
                BackColor = Color.FromArgb(8, 20, 34),
                BorderStyle = BorderStyle.FixedSingle,
                Font = CasinoTheme.UiFont(10F, FontStyle.Bold),
                ForeColor = CasinoTheme.Text
            };
            return input;
        }

        private ComboBox CrearComboFiltro()
        {
            ComboBox combo = new ComboBox
            {
                BackColor = Color.FromArgb(8, 20, 34),
                DrawMode = DrawMode.OwnerDrawFixed,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Font = CasinoTheme.UiFont(10F, FontStyle.Bold),
                ForeColor = CasinoTheme.Text,
                ItemHeight = 28
            };
            combo.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;
                bool seleccionado = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                using (SolidBrush fondo = new SolidBrush(seleccionado ? CasinoTheme.SurfaceAlt : Color.FromArgb(8, 20, 34)))
                    e.Graphics.FillRectangle(fondo, e.Bounds);
                using (SolidBrush texto = new SolidBrush(CasinoTheme.Text))
                    e.Graphics.DrawString(combo.Items[e.Index].ToString(), combo.Font, texto, e.Bounds.X + 8, e.Bounds.Y + 6);
            };
            return combo;
        }

        private void CargarOpcionesFiltro(ComboBox combo, string categoria)
        {
            combo.Items.Clear();

            if (categoria == "Apuestas")
                combo.Items.AddRange(new object[] { "Todas", "Montos apostados", "Premios ganados" });
            else if (categoria == "Depositos")
                combo.Items.AddRange(new object[] { "Todos", "Depositos", "Retiros" });
            else
                combo.Items.AddRange(new object[] { "Todos", "Depositos", "Retiros", "Apuestas", "Ganancias" });

            combo.SelectedIndex = 0;
        }

        private void UbicarFiltrosTransacciones(Panel filtros, Label lblPeriodo, FlowLayoutPanel chips, Label lblFiltro, ComboBox filtro, Label lblDesde, TextBox desde, Label lblHasta, TextBox hasta, Button buscar)
        {
            int ancho = filtros.ClientSize.Width;
            bool compacto = ancho < 920;

            if (compacto)
            {
                filtros.Height = 196;
                int disponible = Math.Max(280, ancho - 28);
                int anchoFecha = Math.Max(130, (disponible - 134) / 2);

                lblPeriodo.SetBounds(14, 10, 220, 18);
                chips.SetBounds(14, 32, Math.Min(330, disponible), 42);

                lblFiltro.SetBounds(14, 82, 220, 18);
                filtro.SetBounds(14, 104, disponible, 34);

                lblDesde.SetBounds(14, 148, anchoFecha, 18);
                desde.SetBounds(14, 168, anchoFecha, 30);
                lblHasta.SetBounds(14 + anchoFecha + 10, 148, anchoFecha, 18);
                hasta.SetBounds(14 + anchoFecha + 10, 168, anchoFecha, 30);
                buscar.SetBounds(ancho - 130, 160, 116, 38);
                return;
            }

            filtros.Height = 126;
            lblPeriodo.SetBounds(14, 18, 220, 18);
            chips.SetBounds(14, 44, 330, 42);

            int derecha = ancho - 14;
            lblFiltro.SetBounds(derecha - 610, 18, 220, 18);
            filtro.SetBounds(derecha - 610, 44, 180, 34);
            lblDesde.SetBounds(derecha - 420, 18, 140, 18);
            desde.SetBounds(derecha - 420, 44, 140, 30);
            lblHasta.SetBounds(derecha - 268, 18, 140, 18);
            hasta.SetBounds(derecha - 268, 44, 140, 30);
            buscar.SetBounds(derecha - 116, 38, 116, 40);
        }

        private void PintarListaMovimientos(Panel lista, string categoria, string filtro, DateTime desde, DateTime hasta)
        {
            lista.Controls.Clear();

            IList<MovimientoResumen> movimientos = ObtenerMovimientos(categoria)
                .Where(m => m.Fecha >= desde && m.Fecha <= hasta)
                .Where(m => CumpleFiltroMovimiento(categoria, filtro, m))
                .OrderByDescending(m => m.Fecha)
                .ToList();

            if (movimientos.Count == 0)
            {
                Label vacio = CrearLabel("No hay movimientos para los filtros seleccionados.", CasinoTheme.Muted, CasinoTheme.UiFont(11F, FontStyle.Bold), ContentAlignment.MiddleCenter);
                vacio.Dock = DockStyle.Top;
                vacio.Height = 70;
                lista.Controls.Add(vacio);
                return;
            }

            int y = 8;
            foreach (MovimientoResumen mov in movimientos)
            {
                Panel fila = CrearFilaMovimiento(mov);
                fila.SetBounds(0, y, Math.Max(560, lista.ClientSize.Width - 26), 62);
                fila.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                lista.Controls.Add(fila);
                y += 72;
            }
        }

        private bool CumpleFiltroMovimiento(string categoria, string filtro, MovimientoResumen movimiento)
        {
            if (filtro == "Todos" || filtro == "Todas") return true;

            if (categoria == "Apuestas")
            {
                if (filtro == "Montos apostados") return movimiento.Tipo == "perdida";
                if (filtro == "Premios ganados") return movimiento.Tipo == "ganancia";
            }

            if (categoria == "Depositos")
            {
                if (filtro == "Depositos") return movimiento.Tipo == "deposito";
                if (filtro == "Retiros") return movimiento.Tipo == "retiro";
            }

            if (filtro == "Depositos") return movimiento.Tipo == "deposito";
            if (filtro == "Retiros") return movimiento.Tipo == "retiro";
            if (filtro == "Apuestas") return movimiento.Tipo == "perdida";
            if (filtro == "Ganancias") return movimiento.Tipo == "ganancia";

            return true;
        }

        private IList<MovimientoResumen> ObtenerMovimientos(string categoria)
        {
            List<MovimientoResumen> movimientos = new List<MovimientoResumen>();
            Dictionary<int, string> nombresJuegos = ObtenerMapaNombresJuegos();

            if (categoria == "Todos" || categoria == "Depositos")
            {
                foreach (Transaccion t in _partidaSvc.ObtenerTransaccionesPorUsuario(_usuario.IdUsuario))
                {
                    if (categoria == "Depositos" && t.Tipo != "deposito" && t.Tipo != "retiro") continue;

                    bool entrada = t.Tipo == "deposito" || t.Tipo == "ganancia";
                    movimientos.Add(new MovimientoResumen
                    {
                        Titulo = ResolverDescripcionMovimiento(t.Descripcion ?? t.Tipo, nombresJuegos),
                        Tipo = t.Tipo,
                        Fecha = t.Fecha,
                        Monto = entrada ? t.Monto : -t.Monto
                    });
                }
                return movimientos;
            }

            foreach (HistorialPartida p in _partidaSvc.ObtenerHistorialPorUsuario(_usuario.IdUsuario))
            {
                string juego = ResolverNombreJuego(p.NombreJuego, nombresJuegos);
                movimientos.Add(new MovimientoResumen
                {
                    Titulo = $"{juego}: monto apostado",
                    Tipo = "perdida",
                    Fecha = p.Fecha,
                    Monto = -p.Apuesta
                });

                if (p.Ganancia > 0)
                {
                    movimientos.Add(new MovimientoResumen
                    {
                        Titulo = $"{juego}: resultado de la apuesta",
                        Tipo = "ganancia",
                        Fecha = p.Fecha,
                        Monto = p.Ganancia
                    });
                }
            }

            return movimientos;
        }

        private string ResolverDescripcionMovimiento(string descripcion, Dictionary<int, string> nombresJuegos)
        {
            if (string.IsNullOrWhiteSpace(descripcion)) return "Movimiento";

            string texto = descripcion.Trim();
            string lower = texto.ToLower();
            int indiceJuego = lower.IndexOf("juego");
            if (indiceJuego < 0) return texto;

            int inicioNumero = indiceJuego + "juego".Length;
            while (inicioNumero < texto.Length && texto[inicioNumero] == ' ')
                inicioNumero++;

            int finNumero = inicioNumero;
            while (finNumero < texto.Length && char.IsDigit(texto[finNumero]))
                finNumero++;

            if (finNumero == inicioNumero) return texto;

            string numero = texto.Substring(inicioNumero, finNumero - inicioNumero);
            if (!int.TryParse(numero, out int idJuego)) return texto;

            string nombre = ResolverNombreJuego(numero, nombresJuegos);
            if (nombre == numero || nombre.ToLower().Contains("juego"))
                nombre = NombreJuegoConocido(idJuego);

            string reemplazo = texto.Substring(indiceJuego, finNumero - indiceJuego);
            return texto.Replace(reemplazo, nombre);
        }

        private Dictionary<int, string> ObtenerMapaNombresJuegos()
        {
            try
            {
                return _partidaSvc.ObtenerJuegos()
                    .GroupBy(j => j.IdJuego)
                    .ToDictionary(g => g.Key, g => g.First().Nombre);
            }
            catch
            {
                return new Dictionary<int, string>();
            }
        }

        private string ResolverNombreJuego(string valor, Dictionary<int, string> nombresJuegos)
        {
            if (string.IsNullOrWhiteSpace(valor)) return "Juego";

            string texto = valor.Trim();
            string lower = texto.ToLower();
            int id = 0;

            if (lower.Contains("juego"))
            {
                string digitos = new string(texto.Where(char.IsDigit).ToArray());
                if (int.TryParse(digitos, out id) && nombresJuegos.ContainsKey(id))
                    return nombresJuegos[id];
            }

            if (int.TryParse(texto, out id) && nombresJuegos.ContainsKey(id))
                return nombresJuegos[id];

            return texto;
        }

        private string NombreJuegoConocido(int idJuego)
        {
            if (idJuego == 1) return "Minas";
            if (idJuego == 2) return "Ruleta";
            if (idJuego == 3) return "Tragamonedas";
            if (idJuego == 4) return "Tragamonedas";
            return "Juego";
        }

        private Panel CrearFilaMovimiento(MovimientoResumen movimiento)
        {
            bool entrada = movimiento.Monto >= 0;
            Panel fila = new Panel
            {
                BackColor = Color.FromArgb(28, 43, 58),
                Height = 62
            };
            fila.Paint += (s, e) =>
            {
                using (SolidBrush brush = new SolidBrush(entrada ? Color.FromArgb(18, 78, 55) : Color.FromArgb(58, 42, 48)))
                    e.Graphics.FillRectangle(brush, 0, 0, 5, fila.Height);
                using (Pen pen = new Pen(Color.FromArgb(41, 59, 77)))
                    e.Graphics.DrawRectangle(pen, 0, 0, fila.Width - 1, fila.Height - 1);
            };

            Label icono = CrearLabel(entrada ? "+" : "-", entrada ? CasinoTheme.Green : Color.FromArgb(203, 213, 225), CasinoTheme.UiFont(18F, FontStyle.Bold), ContentAlignment.MiddleCenter);
            Label titulo = CrearLabel(movimiento.Titulo, CasinoTheme.Text, CasinoTheme.UiFont(9.5F, FontStyle.Bold), ContentAlignment.BottomLeft);
            Label fecha = CrearLabel(movimiento.Fecha.ToString("hh:mm:ss tt dd/MM/yyyy"), CasinoTheme.Muted, CasinoTheme.UiFont(8.5F), ContentAlignment.TopLeft);
            Label monto = CrearLabel($"{(entrada ? "+" : "-")}{Math.Abs(movimiento.Monto):N2}$", entrada ? CasinoTheme.Green : CasinoTheme.Text, CasinoTheme.UiFont(11F, FontStyle.Bold), ContentAlignment.MiddleRight);
            titulo.AutoEllipsis = true;
            fecha.AutoEllipsis = true;

            fila.Controls.Add(icono);
            fila.Controls.Add(titulo);
            fila.Controls.Add(fecha);
            fila.Controls.Add(monto);
            fila.Resize += (s, e) =>
            {
                icono.SetBounds(18, 0, 34, fila.Height);
                int anchoMonto = Math.Min(180, Math.Max(120, fila.Width / 4));
                int anchoTexto = Math.Max(150, fila.Width - anchoMonto - 98);
                titulo.SetBounds(64, 10, anchoTexto, 23);
                fecha.SetBounds(64, 34, anchoTexto, 18);
                monto.SetBounds(fila.Width - anchoMonto - 20, 0, anchoMonto, fila.Height);
            };
            return fila;
        }

        private sealed class MovimientoResumen
        {
            public string Titulo { get; set; }
            public string Tipo { get; set; }
            public DateTime Fecha { get; set; }
            public decimal Monto { get; set; }
        }

        private void MostrarControl(Control control)
        {
            pnlContenido.Controls.Clear();
            control.Dock = DockStyle.Fill;
            pnlContenido.Controls.Add(control);
            cargarDatos();
        }

        private void MostrarInicio()
        {
            pnlContenido.Controls.Clear();
            _vistaInicio.Dock = DockStyle.Fill;
            pnlContenido.Controls.Add(_vistaInicio);
            _lblHeroTag.Text = "LOBBY PRINCIPAL";
            _lblSeccionJuegos.Text = "";
            lblDescripcion.Text = "Entra a tus juegos favoritos desde un lobby mas limpio, con saldo visible y acceso directo a transacciones y billetera.";
            cargarDatos();
        }

        private void MostrarJuegos()
        {
            Panel vista = CrearVistaBase("Juegos", "Elige una mesa y entra directamente a la experiencia de juego.");

            TableLayoutPanel grid = new TableLayoutPanel
            {
                ColumnCount = 3,
                RowCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 24, 0, 0),
                BackColor = CasinoTheme.Page
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            grid.Controls.Add(CrearTarjetaCatalogo("Minas", "Configura cuadricula y minas. Encuentra gemas sin tocar una mina.", pictureBox1.Image, CasinoTheme.Green, btnMinas_Click), 0, 0);
            grid.Controls.Add(CrearTarjetaCatalogo("Ruleta", "Apuesta con fichas sobre numeros, colores y zonas de la mesa europea.", pictureBox3.Image, CasinoTheme.Red, btnRuleta_Click), 1, 0);
            grid.Controls.Add(CrearTarjetaCatalogo("Tragamonedas", "Gira los rodillos, busca combinaciones y consulta tu resultado al instante.", pictureBox2.Image, CasinoTheme.Blue, btnSlot_Click), 2, 0);

            vista.Controls.Add(grid);
            MostrarControl(vista);
        }

        private void MostrarHistorial()
        {
            if (_usuario == null) return;

            Panel vista = CrearVistaBase("Transacciones", "Consulta movimientos, apuestas y resultados de casino.");

            Panel cuerpo = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CasinoTheme.Page,
                Padding = new Padding(0, 24, 0, 0)
            };

            Panel menuLateral = new Panel
            {
                Dock = DockStyle.Left,
                Width = 176,
                BackColor = CasinoTheme.Surface,
                Padding = new Padding(0, 10, 0, 0)
            };

            Panel contenido = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CasinoTheme.Page,
                Padding = new Padding(24, 0, 0, 0)
            };

            Button btnMovimientos = CrearBotonLateral("Todos");
            Button btnApuestas = CrearBotonLateral("Apuestas");
            Button btnDepositos = CrearBotonLateral("Depositos");

            menuLateral.Controls.Add(btnDepositos);
            menuLateral.Controls.Add(btnApuestas);
            menuLateral.Controls.Add(btnMovimientos);

            btnMovimientos.Click += (s, e) => CargarVistaTransacciones(contenido, "Todos", 30);
            btnApuestas.Click += (s, e) => CargarVistaTransacciones(contenido, "Apuestas", 30);
            btnDepositos.Click += (s, e) => CargarVistaTransacciones(contenido, "Depositos", 30);

            cuerpo.Controls.Add(contenido);
            cuerpo.Controls.Add(menuLateral);
            vista.Controls.Add(cuerpo);
            MostrarControl(vista);
            CargarVistaTransacciones(contenido, "Todos", 30);
        }

        private void MostrarBilletera()
        {
            if (_usuario == null) return;

            Panel vista = CrearVistaBase("Billetera", "Administra depositos, retiros y saldo disponible.");
            IList<Transaccion> movimientos = new List<Transaccion>();
            try
            {
                movimientos = _partidaSvc.ObtenerTransaccionesPorUsuario(_usuario.IdUsuario);
            }
            catch
            {
                movimientos = new List<Transaccion>();
            }

            decimal totalDepositos = movimientos
                .Where(t => (t.Tipo ?? "").ToLower().Contains("deposito"))
                .Sum(t => t.Monto);
            decimal totalRetiros = movimientos
                .Where(t => (t.Tipo ?? "").ToLower().Contains("retiro"))
                .Sum(t => t.Monto);
            int cantidadMovimientos = movimientos.Count;

            Panel cuerpo = new Panel
            {
                BackColor = CasinoTheme.Page,
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 24, 0, 0)
            };

            TableLayoutPanel metricas = new TableLayoutPanel
            {
                BackColor = CasinoTheme.Page,
                ColumnCount = 4,
                Dock = DockStyle.Top,
                Height = 126,
                Margin = new Padding(0)
            };
            metricas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
            metricas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23F));
            metricas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23F));
            metricas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23F));
            metricas.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            metricas.Controls.Add(CrearMetricaBilletera("Saldo disponible", $"${_usuario.Saldo:N2}", _usuario.Saldo > 0 ? "Cuenta activa" : "Sin saldo", CasinoTheme.Green), 0, 0);
            metricas.Controls.Add(CrearMetricaBilletera("Depositos", $"${totalDepositos:N2}", "Total registrado", CasinoTheme.Cyan), 1, 0);
            metricas.Controls.Add(CrearMetricaBilletera("Retiros", $"${totalRetiros:N2}", "Total retirado", CasinoTheme.Red), 2, 0);
            metricas.Controls.Add(CrearMetricaBilletera("Movimientos", cantidadMovimientos.ToString(), "En transacciones", CasinoTheme.Gold), 3, 0);

            Panel barra = new Panel
            {
                BackColor = CasinoTheme.Page,
                Dock = DockStyle.Top,
                Height = 58,
                Padding = new Padding(0, 16, 0, 0)
            };
            Label lblAyuda = CrearLabel("Los depositos y retiros quedan registrados automaticamente en Transacciones.", CasinoTheme.Muted, CasinoTheme.UiFont(10F), ContentAlignment.MiddleLeft);
            Button btnVerTransacciones = new Button { Text = "Ver transacciones" };
            CasinoTheme.StyleSecondaryButton(btnVerTransacciones);
            btnVerTransacciones.Click += (s, e) => MostrarHistorial();
            barra.Controls.Add(lblAyuda);
            barra.Controls.Add(btnVerTransacciones);
            barra.Resize += (s, e) =>
            {
                int botonW = 172;
                btnVerTransacciones.SetBounds(barra.Width - botonW, 16, botonW, 36);
                lblAyuda.SetBounds(0, 16, Math.Max(220, barra.Width - botonW - 20), 36);
            };

            TableLayoutPanel acciones = new TableLayoutPanel
            {
                BackColor = CasinoTheme.Page,
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                RowCount = 1,
                Padding = new Padding(0, 10, 0, 0)
            };
            acciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            acciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            acciones.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel tarjetaDeposito = CrearTarjetaBilletera(
                "Depositar saldo",
                "Agrega dinero a tu cuenta y dejalo listo para jugar.",
                "Monto a depositar",
                "Depositar",
                CasinoTheme.Green,
                monto => _partidaSvc.RealizarDeposito(_usuario.IdUsuario, monto));
            tarjetaDeposito.Margin = new Padding(0, 0, 10, 0);

            Panel tarjetaRetiro = CrearTarjetaBilletera(
                "Retirar saldo",
                "Retira una parte de tu saldo disponible.",
                "Monto a retirar",
                "Retirar",
                CasinoTheme.Red,
                monto => _partidaSvc.RealizarRetiro(_usuario.IdUsuario, monto));
            tarjetaRetiro.Margin = new Padding(10, 0, 0, 0);

            acciones.Controls.Add(tarjetaDeposito, 0, 0);
            acciones.Controls.Add(tarjetaRetiro, 1, 0);

            cuerpo.Controls.Add(acciones);
            cuerpo.Controls.Add(barra);
            cuerpo.Controls.Add(metricas);

            vista.Controls.Add(cuerpo);
            MostrarControl(vista);
        }

        private void MostrarVista(UserControl vista)
        {
            if (_usuario == null)
            {
                vista.Dispose();
                return;
            }

            pnlContenido.Controls.Clear();
            vista.Dock = DockStyle.Fill;
            pnlContenido.Controls.Add(vista);
            cargarDatos();
        }

    }
}
