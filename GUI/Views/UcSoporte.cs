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
            CargarFotoBot();
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

        private void ReaplicarBordes()
        {
            AppTheme.ApplyRoundedRegion(pnlHero, 14);
            AppTheme.ApplyRoundedRegion(pnlBot, 14);
            AppTheme.ApplyRoundedRegion(pnlComandos, 14);
            AppTheme.ApplyRoundedRegion(pnlQr, 14);
            AppTheme.ApplyRoundedRegion(picFotoBot, 18);
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

            int fotoSize = Math.Min(172, Math.Max(128, pnlBot.Height - 72));
            picFotoBot.SetBounds(30, (pnlBot.Height - fotoSize) / 2, fotoSize, fotoSize);
            int textX = picFotoBot.Right + 24;
            lblBotTitulo.SetBounds(textX, 30, Math.Max(260, pnlBot.Width - textX - 28), 38);
            lblBotDescripcion.SetBounds(textX + 2, 78, Math.Max(260, pnlBot.Width - textX - 34), 70);
            btnAbrirBot.SetBounds(textX + 2, Math.Max(158, pnlBot.Height - 88), Math.Min(252, pnlBot.Width - textX - 34), 42);
            lblBotUrl.SetBounds(textX + 2, btnAbrirBot.Bottom + 10, Math.Max(260, pnlBot.Width - textX - 34), 24);

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

        private void CargarFotoBot()
        {
            // Problema visual que resuelve: el asistente deja de ser un icono generico y usa una identidad visual propia.
            string rutaFoto = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "foto-bot.png");
            if (!File.Exists(rutaFoto)) return;

            using (Image imagen = Image.FromFile(rutaFoto))
                picFotoBot.Image = new Bitmap(imagen);
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

            if (panel == pnlBot && picFotoBot.Image != null)
            {
                using (Pen marco = new Pen(Color.FromArgb(170, AppTheme.Dorado), 2))
                {
                    Rectangle foto = new Rectangle(picFotoBot.Left - 2, picFotoBot.Top - 2, picFotoBot.Width + 4, picFotoBot.Height + 4);
                    e.Graphics.DrawRectangle(marco, foto);
                }
            }
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
