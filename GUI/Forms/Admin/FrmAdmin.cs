using BLL;
using ENTITY;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmAdmin : Form
    {
        private readonly Usuario _admin;
        private readonly PartidaServicio _partidaSvc = new PartidaServicio();
        private readonly TransaccionServicio _transSvc = new TransaccionServicio();
        private ToolStripMenuItem _ultimoItemActivo;
        private Panel _indicadorActivo;

        public FrmAdmin(Usuario admin)
        {
            _admin = admin;
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            AplicarEstiloVisual();
            PrepararNavegacion();
            lblAdminNombre.Text = $"Administrador: {_admin.Nombre1} {_admin.Apellido1}";
            MostrarDashboard();
        }

        private void AplicarEstiloVisual()
        {
            AppTheme.ApplyForm(this);
            AppTheme.ApplyNavbar(panelNavbar);
            AppTheme.ApplyPrimaryButton(btnCerrarSesion, Color.FromArgb(220, 50, 50));
            mainLayout.BackColor = AppTheme.BgPrincipal;
            pnlContenido.BackColor = AppTheme.BgPrincipal;
            panelNavbar.Margin = Padding.Empty;
            pnlContenido.Margin = Padding.Empty;
            mainLayout.Margin = Padding.Empty;
            AppTheme.ApplyTitle(lblAdminNombre);

            menuStrip.RenderMode = ToolStripRenderMode.Professional;
            menuStrip.Renderer = new ToolStripProfessionalRenderer(new CasinoMenuColors());
            menuStrip.BackColor = AppTheme.BgNavbar;
            menuStrip.Padding = new Padding(370, 0, 0, 0);

            foreach (ToolStripMenuItem item in menuStrip.Items)
                AppTheme.ApplyNavbarItem(item);

            // logout button subtle border rounding
            btnCerrarSesion.Font = AppTheme.ValorChico;
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.Text = "Salir";

            // active indicator
            _indicadorActivo = new Panel
            {
                Height = 3,
                BackColor = AppTheme.Dorado,
                Width = 0,
                Location = new Point(0, panelNavbar.Height - 3)
            };
            panelNavbar.Controls.Add(_indicadorActivo);
            _indicadorActivo.BringToFront();
        }

        private void ActivarItemMenu(ToolStripMenuItem item)
        {
            _ultimoItemActivo = item;
            if (item == null) return;
            // reset all items
            foreach (ToolStripMenuItem i in menuStrip.Items)
            {
                i.ForeColor = AppTheme.TextoSecundario;
                i.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            }
            item.ForeColor = AppTheme.Dorado;
            item.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            // move indicator
            Point screen = menuStrip.PointToScreen(item.ContentRectangle.Location);
            Point local = panelNavbar.PointToClient(screen);
            _indicadorActivo.Location = new Point(local.X - 8, panelNavbar.Height - 3);
            _indicadorActivo.Width = item.ContentRectangle.Width + 16;
            _indicadorActivo.BackColor = AppTheme.Dorado;
            _indicadorActivo.BringToFront();
        }

        private void PrepararNavegacion()
        {
            dashboardToolStripMenuItem.Click += (s, e) => { ActivarItemMenu(dashboardToolStripMenuItem); MostrarDashboard(); };
            usuariosToolStripMenuItem.Click += (s, e) => { ActivarItemMenu(usuariosToolStripMenuItem); MostrarUsuarios(); };
            partidasToolStripMenuItem.Click += (s, e) => { ActivarItemMenu(partidasToolStripMenuItem); MostrarPartidas(); };
            transaccionesToolStripMenuItem.Click += (s, e) => { ActivarItemMenu(transaccionesToolStripMenuItem); MostrarTransacciones(); };
            reportesToolStripMenuItem.Click += (s, e) => { ActivarItemMenu(reportesToolStripMenuItem); MostrarReportes(); };
        }

        private void MostrarVista(UserControl vista)
        {
            pnlContenido.Controls.Clear();
            vista.Dock = DockStyle.Fill;
            pnlContenido.Controls.Add(vista);
        }

        private void MostrarDashboard()
        {
            MostrarVista(new UcDashboardAdmin());
        }

        private void MostrarUsuarios()
        {
            MostrarVista(new UcGestionUsuarios());
        }

        private void MostrarPartidas()
        {
            UcAdminGrid grid = new UcAdminGrid();
            grid.CargarDatos(_partidaSvc.ObtenerTodasConNombres());
            MostrarVista(grid);
            grid.Configurar("Partidas - Registro completo",
                new[] { "IdPartida", "Usuario", "Juego", "Estado", "Fecha", "Apuesta", "Ganancia" },
                new[] { "Partida", "Usuario", "Juego", "Estado", "Fecha", "Apuesta", "Ganancia" });
            grid.FormatearColumnas();
        }

        private void MostrarTransacciones()
        {
            UcAdminGrid grid = new UcAdminGrid();
            grid.CargarDatos(_transSvc.ObtenerTodasConNombres());
            MostrarVista(grid);
            grid.Configurar("Transacciones - Movimientos financieros",
                new[] { "IdTransaccion", "Usuario", "Tipo", "Monto", "Fecha", "Descripcion" },
                new[] { "Movimiento", "Usuario", "Tipo", "Monto", "Fecha", "Descripcion" });
            grid.FormatearColumnas();
        }

        private void MostrarReportes()
        {
            MostrarVista(new UcAdminReportes());
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            new FrmLogin().Show();
            Close();
        }

        private sealed class CasinoMenuColors : ProfessionalColorTable
        {
            public override Color MenuItemSelected => AppTheme.BgElevated;
            public override Color MenuItemBorder => Color.Transparent;
            public override Color ToolStripDropDownBackground => AppTheme.BgNavbar;
            public override Color MenuBorder => AppTheme.Borde;
            public override Color MenuItemPressedGradientBegin => AppTheme.BgHover;
            public override Color MenuItemPressedGradientEnd => AppTheme.BgHover;
            public override Color ImageMarginGradientBegin => AppTheme.BgNavbar;
            public override Color ImageMarginGradientMiddle => AppTheme.BgNavbar;
            public override Color ImageMarginGradientEnd => AppTheme.BgNavbar;
        }
    }
}
