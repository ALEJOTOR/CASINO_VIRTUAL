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
