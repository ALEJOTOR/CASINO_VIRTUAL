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
            Resize += (s, e) => ReaplicarBordes();
            ReaplicarBordes();
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
            lblComandos.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
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
            using (LinearGradientBrush brush = new LinearGradientBrush(area, AppTheme.BgCard, Color.FromArgb(18, 30, 52), 0F))
                e.Graphics.FillRectangle(brush, area);
            using (Pen pen = new Pen(Color.FromArgb(70, AppTheme.Dorado), 1))
                e.Graphics.DrawRectangle(pen, 0, 0, area.Width - 1, area.Height - 1);
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
