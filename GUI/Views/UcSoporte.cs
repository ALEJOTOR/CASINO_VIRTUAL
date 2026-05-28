using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace GUI
{
    public partial class UcSoporte : UserControl
    {
        private const string BotUrl = "https://t.me/CasinoRoyalAsistenteBot";
        private const string BotDeepLink = "tg://resolve?domain=CasinoRoyalAsistenteBot";

        public UcSoporte()
        {
            InitializeComponent();
            AplicarEstiloVisual();
        }

        private void AplicarEstiloVisual()
        {
            // Problema visual que resuelve: la vista de soporte queda integrada al lobby y no parece una ventana externa.
            AppTheme.ApplyView(this);
            layoutPrincipal.BackColor = AppTheme.BgPrincipal;
            pnlContenido.BackColor = AppTheme.BgPrincipal;

            AppTheme.ApplyTitle(lblTitulo);
            AppTheme.ApplySubtitle(lblSubtitulo);
            AppTheme.ApplyCard(pnlHero, 14);
            AppTheme.ApplyCard(pnlBot, 14);
            AppTheme.ApplyCard(pnlComandos, 14);
            AppTheme.ApplyCard(pnlQr, 14);

            EstilizarTexto();
            EstilizarBoton();
            EstilizarIcono();
            CargarQrBot();

            pnlHero.Paint += PnlHero_Paint;
            pnlBot.Paint += CardFuturista_Paint;
            pnlComandos.Paint += CardFuturista_Paint;
            pnlQr.Paint += CardFuturista_Paint;
            Resize += (s, e) => ReaplicarBordes();
            Resize += (s, e) => ReubicarSoporte();
            ReaplicarBordes();
            ReubicarSoporte();
        }

        private void EstilizarTexto()
        {
            // Problema visual que resuelve: organiza la informacion del bot con jerarquia clara para soporte.
            lblBotTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblBotTitulo.ForeColor = AppTheme.Dorado;
            lblBotDescripcion.Font = AppTheme.Cuerpo;
            lblBotDescripcion.ForeColor = AppTheme.TextoSecundario;
            lblBotUrl.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblBotUrl.ForeColor = AppTheme.Azul;

            lblComandosTitulo.Font = AppTheme.Valor;
            lblComandosTitulo.ForeColor = AppTheme.Dorado;
            lblComandos.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblComandos.ForeColor = AppTheme.TextoPrimario;

            lblQrTitulo.Font = AppTheme.Valor;
            lblQrTitulo.ForeColor = AppTheme.Dorado;
            lblQrTexto.Font = AppTheme.Cuerpo;
            lblQrTexto.ForeColor = AppTheme.TextoSecundario;
        }

        private void EstilizarBoton()
        {
            // Problema visual que resuelve: el acceso al bot queda como accion principal y facil de encontrar.
            AppTheme.ApplyPrimaryButton(btnAbrirBot, AppTheme.Azul);
            btnAbrirBot.Text = "Abrir bot en Telegram";
        }

        private void EstilizarIcono()
        {
            // Problema visual que resuelve: agrega un punto visual reconocible sin depender de imagenes externas.
            lblIconoBot.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblIconoBot.ForeColor = Color.White;
            lblIconoBot.BackColor = AppTheme.Azul;
            AppTheme.ApplyRoundedRegion(lblIconoBot, 46);
        }

        private void ReaplicarBordes()
        {
            AppTheme.ApplyRoundedRegion(pnlHero, 14);
            AppTheme.ApplyRoundedRegion(pnlBot, 14);
            AppTheme.ApplyRoundedRegion(pnlComandos, 14);
            AppTheme.ApplyRoundedRegion(pnlQr, 14);
            AppTheme.ApplyRoundedRegion(lblIconoBot, 46);
            AppTheme.ApplyRoundedRegion(btnAbrirBot, 8);
        }

        private void ReubicarSoporte()
        {
            if (Width <= 0) return;

            // Problema visual que resuelve: distribuye el soporte como una pantalla completa y no como tarjetas sueltas.
            bool compacto = Width < 1050;
            layoutPrincipal.Padding = compacto ? new Padding(22, 22, 22, 24) : new Padding(36, 30, 36, 34);
            layoutPrincipal.RowStyles[0].Height = compacto ? 126 : 150;

            lblTitulo.SetBounds(28, 24, Math.Max(420, pnlHero.Width - 56), 44);
            lblSubtitulo.SetBounds(30, 74, Math.Max(420, pnlHero.Width - 60), 42);

            lblIconoBot.SetBounds(30, 32, 94, 94);
            lblBotTitulo.SetBounds(150, 30, Math.Max(260, pnlBot.Width - 180), 36);
            lblBotDescripcion.SetBounds(152, 76, Math.Max(260, pnlBot.Width - 190), 70);
            btnAbrirBot.SetBounds(152, Math.Max(154, pnlBot.Height - 88), Math.Min(240, pnlBot.Width - 190), 42);
            lblBotUrl.SetBounds(152, btnAbrirBot.Bottom + 10, Math.Max(260, pnlBot.Width - 190), 24);

            lblComandosTitulo.SetBounds(28, 26, Math.Max(300, pnlComandos.Width - 56), 32);
            lblComandos.SetBounds(30, 80, Math.Max(300, pnlComandos.Width - 60), Math.Max(220, pnlComandos.Height - 110));

            lblQrTitulo.SetBounds(28, 22, Math.Max(220, pnlQr.Width - 220), 32);
            lblQrTexto.SetBounds(30, 66, Math.Max(220, pnlQr.Width - 230), 72);
            int qrSize = Math.Min(156, Math.Max(118, pnlQr.Height - 34));
            picQrBot.SetBounds(pnlQr.Width - qrSize - 32, (pnlQr.Height - qrSize) / 2, qrSize, qrSize);

            ReaplicarBordes();
            Invalidate();
        }

        private void CargarQrBot()
        {
            // Problema visual que resuelve: permite abrir el bot desde celular sin depender del navegador del PC.
            string rutaQr = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "qr-bot.jpeg");
            if (!File.Exists(rutaQr)) return;

            using (Image imagen = Image.FromFile(rutaQr))
                picQrBot.Image = new Bitmap(imagen);
        }

        private void PnlHero_Paint(object sender, PaintEventArgs e)
        {
            // Problema visual que resuelve: el encabezado de soporte gana profundidad sin romper la paleta del casino.
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = pnlHero.ClientRectangle;
            using (LinearGradientBrush brush = new LinearGradientBrush(area, Color.FromArgb(12, 22, 42), Color.FromArgb(18, 36, 70), 0F))
                e.Graphics.FillRectangle(brush, area);
            using (SolidBrush glow = new SolidBrush(Color.FromArgb(34, AppTheme.Azul)))
                e.Graphics.FillEllipse(glow, area.Width - 260, -90, 320, 220);
            using (Pen pen = new Pen(Color.FromArgb(70, AppTheme.Dorado), 1))
                e.Graphics.DrawRectangle(pen, 0, 0, area.Width - 1, area.Height - 1);
        }

        private void CardFuturista_Paint(object sender, PaintEventArgs e)
        {
            // Problema visual que resuelve: las tarjetas de soporte tienen profundidad y acento tecnologico.
            Panel panel = (Panel)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = panel.ClientRectangle;
            using (LinearGradientBrush fondo = new LinearGradientBrush(area, Color.FromArgb(18, 29, 48), Color.FromArgb(9, 15, 28), 90F))
                e.Graphics.FillRectangle(fondo, area);

            Color acento = panel == pnlBot ? AppTheme.Azul : panel == pnlComandos ? AppTheme.Dorado : AppTheme.Verde;
            using (SolidBrush brillo = new SolidBrush(Color.FromArgb(26, acento)))
                e.Graphics.FillEllipse(brillo, area.Width - 160, -60, 240, 150);
            using (Pen borde = new Pen(Color.FromArgb(90, acento), 1))
                e.Graphics.DrawRectangle(borde, 0, 0, area.Width - 1, area.Height - 1);
            using (SolidBrush barra = new SolidBrush(acento))
                e.Graphics.FillRectangle(barra, 0, 0, area.Width, 3);
        }

        private void btnAbrirBot_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = BotDeepLink,
                    UseShellExecute = true
                });
            }
            catch
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = BotUrl,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo abrir Telegram: " + ex.Message, "Casino Royal",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
