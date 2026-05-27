using BLL;
using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class UcAdminReportes : UserControl
    {
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();
        private readonly PartidaServicio _partidaSvc = new PartidaServicio();

        public UcAdminReportes()
        {
            InitializeComponent();
            // Problema visual que resuelve: reportes deja de verse como area plana y usa botones/textarea del tema.
            AppTheme.ApplyView(this);
            AppTheme.ApplyTitle(lblTitulo);
            AppTheme.ApplyCard(pnlBotones, 8);
            AppTheme.ApplyPrimaryButton(btnReporteUsuarios, AppTheme.Azul);
            AppTheme.ApplyPrimaryButton(btnReportePartidas, AppTheme.Verde);
            AppTheme.ApplyTextBox(txtReporte);
        }

        private void btnReporteUsuarios_Click(object sender, EventArgs e)
        {
            txtReporte.Text = _usuarioSvc.GenerarReporte();
        }

        private void btnReportePartidas_Click(object sender, EventArgs e)
        {
            txtReporte.Text = _partidaSvc.GenerarReporte();
        }
    }
}
