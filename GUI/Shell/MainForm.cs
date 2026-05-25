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
            CasinoTheme.StylePage(this);
            CasinoTheme.StyleHeader(panelNavbar);
            CasinoTheme.StyleActionButton(btnCerrarSesion, CasinoTheme.Red);
            mainLayout.BackColor = CasinoTheme.Page;
            pnlContenido.BackColor = CasinoTheme.Page;
            panelNavbar.Margin = Padding.Empty;
            pnlContenido.Margin = Padding.Empty;
            mainLayout.Margin = Padding.Empty;
            _lblMarca.BringToFront();
            lblSaldo.BringToFront();
            btnCerrarSesion.BringToFront();
            lblSaldo.BackColor = CasinoTheme.SurfaceAlt;
            lblSaldo.ForeColor = CasinoTheme.Green;
            lblSaldo.Font = CasinoTheme.UiFont(12F, FontStyle.Bold);
            lblSaldo.TextAlign = ContentAlignment.MiddleRight;
            menuStrip.RenderMode = ToolStripRenderMode.Professional;
            menuStrip.Renderer = new ToolStripProfessionalRenderer(new CasinoMenuColors());
            foreach (ToolStripMenuItem item in menuStrip.Items)
            {
                item.Font = CasinoTheme.UiFont(11F, FontStyle.Bold);
                item.ForeColor = CasinoTheme.Text;
                item.Margin = new Padding(6, 0, 6, 0);
            }
        }

        private void PrepararNavegacion()
        {
            inicioToolStripMenuItem.Click += (s, e) => MostrarInicio();
            historialToolStripMenuItem.Click += (s, e) => MostrarHistorial();
            billeteraToolStripMenuItem.Click += (s, e) => MostrarBilletera();
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
            public override Color MenuItemSelected => CasinoTheme.SurfaceAlt;
            public override Color MenuItemBorder => CasinoTheme.Border;
            public override Color ToolStripDropDownBackground => CasinoTheme.Header;
            public override Color ImageMarginGradientBegin => CasinoTheme.Header;
            public override Color ImageMarginGradientMiddle => CasinoTheme.Header;
            public override Color ImageMarginGradientEnd => CasinoTheme.Header;
        }
    }
}
