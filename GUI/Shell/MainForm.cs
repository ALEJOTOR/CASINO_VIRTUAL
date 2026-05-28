using BLL;
using ENTITY;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class MainForm : Form
    {
        private readonly Usuario _usuario;
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();

        public MainForm(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            AplicarEstiloVisual();
            PrepararNavegacion();
            ActualizarSaldoNavbar();
            MostrarInicio();
        }

        private void AplicarEstiloVisual()
        {
            // Problema visual que resuelve: unifica el fondo general y prepara el fade-in de la ventana principal.
            AppTheme.ApplyForm(this);
            AppTheme.ApplyNavbar(panelNavbar);
            AppTheme.ApplyPrimaryButton(btnCerrarSesion, ColorTranslator.FromHtml("#E53E3E"));
            mainLayout.BackColor = AppTheme.BgPrincipal;
            pnlContenido.BackColor = AppTheme.BgPrincipal;
            panelNavbar.Margin = Padding.Empty;
            pnlContenido.Margin = Padding.Empty;
            mainLayout.Margin = Padding.Empty;
            _lblMarca.BringToFront();
            lblSaldo.BringToFront();
            btnCerrarSesion.BringToFront();
            // Problema visual que resuelve: el saldo queda como indicador importante con borde dorado y flash al cambiar.
            AppTheme.ApplySaldoLabel(lblSaldo);
            AppTheme.ApplyTitle(_lblMarca);
            // Problema visual que resuelve: evita que el saldo y cerrar sesion queden fuera de lugar al maximizar o abrir en el Designer.
            panelNavbar.Resize += (s, e) => ReubicarNavbar();
            ReubicarNavbar();
            menuStrip.RenderMode = ToolStripRenderMode.Professional;
            menuStrip.Renderer = new ToolStripProfessionalRenderer(new CasinoMenuColors());
            foreach (ToolStripMenuItem item in menuStrip.Items)
            {
                // Problema visual que resuelve: los enlaces de navegacion quedan legibles y consistentes con el navbar.
                item.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                item.ForeColor = AppTheme.TextoPrimario;
                item.Margin = new Padding(6, 0, 6, 0);
            }
        }

        private void ReubicarNavbar()
        {
            if (panelNavbar.ClientSize.Width <= 0) return;

            int margen = 16;
            int btnW = 134;
            int saldoW = 248;
            btnCerrarSesion.Text = "Cerrar sesion";
            btnCerrarSesion.SetBounds(panelNavbar.ClientSize.Width - margen - btnW, 14, btnW, 32);
            lblSaldo.SetBounds(btnCerrarSesion.Left - 16 - saldoW, 10, saldoW, 40);
            _lblMarca.SetBounds(24, 0, 210, 60);
            menuStrip.Padding = new Padding(250, 10, 0, 8);
            menuStrip.Size = new Size(Math.Max(300, lblSaldo.Left - 260), 60);
            AppTheme.ApplyRoundedRegion(lblSaldo, 12);
        }

        private void PrepararNavegacion()
        {
            inicioToolStripMenuItem.Click += (s, e) => MostrarInicio();
            historialToolStripMenuItem.Click += (s, e) => MostrarHistorial();
            billeteraToolStripMenuItem.Click += (s, e) => MostrarBilletera();
            soporteToolStripMenuItem.Click += (s, e) => MostrarSoporte();
        }

        private void ActualizarSaldoNavbar()
        {
            if (_usuario == null) return;
            Usuario u = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario);
            if (u != null) _usuario.Saldo = u.Saldo;
            lblSaldo.Text = $"Saldo: ${_usuario.Saldo:N2}";
        }

        private void MostrarInicio()
        {
            UcInicio vista = new UcInicio(_usuario);
            vista.JugarMinasSolicitado += (s, e) => AbrirJuego(new UcMinas(_usuario));
            vista.JugarRuletaSolicitado += (s, e) => AbrirJuego(new UcRuleta(_usuario));
            vista.JugarSlotSolicitado += (s, e) => AbrirJuego(new UcTragamonedas(_usuario));
            MostrarVista(vista);
        }

        private void MostrarHistorial()
        {
            MostrarVista(new UcHistorial(_usuario));
        }

        private void MostrarBilletera()
        {
            UcBilletera vista = new UcBilletera(_usuario);
            vista.SaldoActualizado += (s, e) => ActualizarSaldoNavbar();
            MostrarVista(vista);
        }

        private void MostrarSoporte()
        {
            MostrarVista(new UcSoporte());
        }

        private void AbrirJuego(UserControl juego)
        {
            if (juego is IVistaJuego vistaJuego)
                vistaJuego.SaldoActualizado += (s, e) => ActualizarSaldoNavbar();
            MostrarVista(juego);
        }

        private void MostrarVista(UserControl vista)
        {
            pnlContenido.Controls.Clear();
            vista.Dock = DockStyle.Fill;
            pnlContenido.Controls.Add(vista);
            ActualizarSaldoNavbar();
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
