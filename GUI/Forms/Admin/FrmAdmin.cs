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
            // Problema visual que resuelve: el panel admin comparte fondo, animacion inicial y navbar con el lobby.
            AppTheme.ApplyForm(this);
            AppTheme.ApplyNavbar(panelNavbar);
            AppTheme.ApplyPrimaryButton(btnCerrarSesion, ColorTranslator.FromHtml("#E53E3E"));
            mainLayout.BackColor = AppTheme.BgPrincipal;
            pnlContenido.BackColor = AppTheme.BgPrincipal;
            panelNavbar.Margin = Padding.Empty;
            pnlContenido.Margin = Padding.Empty;
            mainLayout.Margin = Padding.Empty;
            menuStrip.RenderMode = ToolStripRenderMode.Professional;
            menuStrip.Renderer = new ToolStripProfessionalRenderer(new CasinoMenuColors());
            AppTheme.ApplyTitle(lblAdminNombre);
            foreach (ToolStripMenuItem item in menuStrip.Items)
            {
                // Problema visual que resuelve: los enlaces administrativos tienen la misma lectura y hover que el lobby.
                item.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                item.ForeColor = AppTheme.TextoPrimario;
                item.Margin = new Padding(6, 0, 6, 0);
            }
        }

        private void PrepararNavegacion()
        {
            dashboardToolStripMenuItem.Click += (s, e) => MostrarDashboard();
            usuariosToolStripMenuItem.Click += (s, e) => MostrarUsuarios();
            partidasToolStripMenuItem.Click += (s, e) => MostrarPartidas();
            transaccionesToolStripMenuItem.Click += (s, e) => MostrarTransacciones();
            reportesToolStripMenuItem.Click += (s, e) => MostrarReportes();
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
                new[] { "IdPartida", "Usuario", "Juego", "Estado", "Fecha", "Apuesta", "Ganancia", "Resultado" },
                new[] { "Partida", "Usuario", "Juego", "Estado", "Fecha", "Apuesta", "Ganancia", "Resultado" });
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
            public override Color MenuItemSelected => AppTheme.BgHover;
            public override Color MenuItemBorder => AppTheme.Dorado;
            public override Color ToolStripDropDownBackground => AppTheme.BgNavbar;
            public override Color ImageMarginGradientBegin => AppTheme.BgNavbar;
            public override Color ImageMarginGradientMiddle => AppTheme.BgNavbar;
            public override Color ImageMarginGradientEnd => AppTheme.BgNavbar;
        }
    }
}
