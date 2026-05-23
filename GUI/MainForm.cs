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

        private void btnDepositar_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtMontoDeposito.Text, out decimal monto) || monto <= 0)
            {
                MessageBox.Show("Ingrese un monto valido.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnDepositar.Enabled = false;

            try
            {
                string resultado = _partidaSvc.RealizarDeposito(_usuario.IdUsuario, monto);
                bool ok = resultado == "Deposito realizado correctamente.";

                MessageBox.Show(resultado, ok ? "Exito" : "Error",
                    MessageBoxButtons.OK,
                    ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                if (ok)
                {
                    txtMontoDeposito.Clear();
                    cargarDatos();
                }
            }
            finally
            {
                btnDepositar.Enabled = true;
            }
        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            MostrarFormulario(new FrmCliente(_usuario));
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
            juegosToolStripMenuItem.Click += (s, e) => MostrarInicio();
        }

        private void AplicarEstiloVisual()
        {
            CasinoTheme.StylePage(this);
            CasinoTheme.StyleHeader(panelNavbar);
            CasinoTheme.StyleInput(txtMontoDeposito);
            CasinoTheme.StyleActionButton(btnDepositar, CasinoTheme.Green);
            CasinoTheme.StyleActionButton(btnHistorial, CasinoTheme.Blue);
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
            _panelWallet.BringToFront();
            _badgeMinas.BringToFront();
            _badgeRuleta.BringToFront();
            _badgeSlot.BringToFront();

            CasinoTheme.StyleTitle(lblBienvenido, 24F);
            CasinoTheme.StyleLabel(lblDescripcion, CasinoTheme.Muted, 11.5F, FontStyle.Regular);
            CasinoTheme.StyleLabel(lblDeposito, CasinoTheme.Text, 9.5F, FontStyle.Bold);
            CasinoTheme.StyleLabel(lblUsuarios, CasinoTheme.Gold, 14F, FontStyle.Bold);
            CasinoTheme.StyleLabel(lblPremios, CasinoTheme.Gold, 14F, FontStyle.Bold);
            CasinoTheme.StyleLabel(lblSoporte, CasinoTheme.Gold, 14F, FontStyle.Bold);
            lblBienvenido.AutoSize = false;
            lblDescripcion.AutoSize = false;
            lblDeposito.AutoSize = false;
            lblUsuarios.Text = "3 juegos\nDisponibles";
            lblPremios.Text = "Saldo real\nCon historial";
            lblSoporte.Text = "24/7\nSoporte";

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
            _panelWallet.Paint += Wallet_Paint;

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
            _lblHeroDato = CrearLabel("Minas, ruleta y tragamonedas en una sola ventana.", CasinoTheme.Text, CasinoTheme.UiFont(10F, FontStyle.Bold), ContentAlignment.MiddleLeft);
            _lblSeccionJuegos = CrearLabel("Juegos destacados", CasinoTheme.Text, CasinoTheme.UiFont(16F, FontStyle.Bold), ContentAlignment.MiddleLeft);

            panelHero.Controls.Add(_lblHeroTag);
            panelHero.Controls.Add(_lblHeroDato);
            panelHero.Controls.Add(_lblSeccionJuegos);

            _panelWallet = new Panel
            {
                BackColor = CasinoTheme.Surface,
                Name = "panelWallet"
            };

            _lblWalletTitulo = CrearLabel("Billetera", CasinoTheme.Gold, CasinoTheme.UiFont(12F, FontStyle.Bold), ContentAlignment.MiddleLeft);
            _lblWalletAyuda = CrearLabel("Recarga saldo y consulta tus movimientos.", CasinoTheme.Muted, CasinoTheme.UiFont(9F), ContentAlignment.MiddleLeft);

            panelHero.Controls.Remove(lblDeposito);
            panelHero.Controls.Remove(txtMontoDeposito);
            panelHero.Controls.Remove(btnDepositar);
            panelHero.Controls.Remove(btnHistorial);

            _panelWallet.Controls.Add(_lblWalletTitulo);
            _panelWallet.Controls.Add(_lblWalletAyuda);
            _panelWallet.Controls.Add(lblDeposito);
            _panelWallet.Controls.Add(txtMontoDeposito);
            _panelWallet.Controls.Add(btnDepositar);
            _panelWallet.Controls.Add(btnHistorial);
            panelHero.Controls.Add(_panelWallet);
            _panelWallet.BringToFront();

            _badgeMinas = CrearBadge("Estrategia");
            _badgeRuleta = CrearBadge("Clasico");
            _badgeSlot = CrearBadge("Rapido");
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
            button.Text = "Entrar";
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
        }

        private void ReubicarInicio()
        {
            if (pnlContenido.ClientSize.Width <= 0) return;

            int ancho = pnlContenido.ClientSize.Width;
            bool compacto = ancho < 1050;

            layoutInicio.RowStyles[0].Height = compacto ? 220 : 190;
            layoutInicio.RowStyles[2].Height = compacto ? 74 : 86;
            tlpJuegos.Padding = compacto ? new Padding(20, 22, 20, 14) : new Padding(34, 26, 34, 18);

            int heroW = panelHero.ClientSize.Width;
            int margen = compacto ? 22 : 32;
            int walletW = compacto ? Math.Min(330, heroW - margen * 2) : 350;

            _lblHeroTag.SetBounds(margen, 20, 220, 20);
            lblBienvenido.SetBounds(margen, 44, Math.Max(360, heroW - walletW - margen * 3), 45);
            lblDescripcion.SetBounds(margen + 2, 92, Math.Max(360, heroW - walletW - margen * 3), 52);
            lblDescripcion.MaximumSize = new Size(lblDescripcion.Width, 0);
            _lblHeroDato.SetBounds(margen + 2, 142, Math.Max(360, heroW - walletW - margen * 3), 26);
            _lblSeccionJuegos.SetBounds(margen, panelHero.ClientSize.Height - 34, 260, 28);

            if (compacto)
                _panelWallet.SetBounds(heroW - walletW - margen, 42, walletW, 132);
            else
                _panelWallet.SetBounds(heroW - walletW - margen, 24, walletW, 138);

            _lblWalletTitulo.SetBounds(18, 12, _panelWallet.Width - 36, 24);
            _lblWalletAyuda.SetBounds(18, 38, _panelWallet.Width - 36, 20);
            lblDeposito.SetBounds(18, 68, 140, 20);
            txtMontoDeposito.SetBounds(18, 92, 128, 28);
            btnDepositar.SetBounds(158, 91, 86, 30);
            btnHistorial.SetBounds(252, 91, Math.Max(78, _panelWallet.Width - 270), 30);

            _lblMarca.SetBounds(18, 0, 170, panelNavbar.Height);
            menuStrip.Padding = new Padding(190, 0, 0, 0);
            lblSaldo.SetBounds(Math.Max(420, panelNavbar.Width - 304), 10, 174, 34);
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
            descripcion.SetBounds(pad, 52, panel.Width - pad * 2, 48);
            image.SetBounds(pad, 108, panel.Width - pad * 2, Math.Max(90, panel.Height - 172));
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

        private void Wallet_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = new Rectangle(0, 0, _panelWallet.Width, _panelWallet.Height);
            CasinoTheme.DrawBorderedPanel(e.Graphics, area, CasinoTheme.Surface, CasinoTheme.Border);
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(30, CasinoTheme.Green)))
                e.Graphics.FillRectangle(brush, 1, 1, area.Width - 2, 4);
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

        private void MostrarInicio()
        {
            pnlContenido.Controls.Clear();
            _vistaInicio.Dock = DockStyle.Fill;
            pnlContenido.Controls.Add(_vistaInicio);
            cargarDatos();
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

        private void MostrarFormulario(Form formulario)
        {
            pnlContenido.Controls.Clear();
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            pnlContenido.Controls.Add(formulario);
            formulario.Show();
            cargarDatos();
        }

    }
}
