using System.Windows.Forms;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class FrmAdmin : Form
    {
        private readonly Usuario         _admin;
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();
        private readonly PartidaServicio _partidaSvc = new PartidaServicio();

        public FrmAdmin(Usuario admin)
        {
            _admin = admin;
            InitializeComponent();
            lblAdminNombre.Text = $"Administrador: {_admin.Nombre1} {_admin.Apellido1}";
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = _usuarioSvc.ObtenerTodos();
        }

        private void CargarPartidas()
        {
            dgvPartidas.DataSource = null;
            dgvPartidas.DataSource = _partidaSvc.ObtenerTodas();
        }

        private void CargarTransacciones()
        {
            dgvTransacciones.DataSource = null;
            dgvTransacciones.DataSource = _partidaSvc.ObtenerTodasTransacciones();
        }

        private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (tabControl.SelectedTab == tabPartidas)      CargarPartidas();
            if (tabControl.SelectedTab == tabTransacciones) CargarTransacciones();
        }

        private void btnEliminarUsuario_Click(object sender, System.EventArgs e)
        {
            Usuario u = ObtenerSeleccionado();
            if (u == null) return;
            if (u.IdRol == 1)
            {
                MessageBox.Show("No se puede eliminar a un administrador.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"¿Eliminar al usuario '{u.Username}'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            string resultado = _usuarioSvc.Eliminar(u.IdUsuario);
            bool ok = resultado == "Guardado correctamente.";
            MessageBox.Show(ok ? "Usuario eliminado." : resultado,
                ok ? "Éxito" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            if (ok) CargarUsuarios();
        }

        private void btnSuspender_Click(object sender, System.EventArgs e)
        {
            Usuario u = ObtenerSeleccionado();
            if (u == null) return;
            if (u.IdRol == 1)
            {
                MessageBox.Show("No se puede suspender a un administrador.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string nuevoEstado = u.Estado == "suspendido" ? "activo" : "suspendido";
            string resultado   = _usuarioSvc.CambiarEstado(u.IdUsuario, nuevoEstado);
            bool ok = resultado == "Guardado correctamente.";
            MessageBox.Show(ok ? $"Estado cambiado a '{nuevoEstado}'." : resultado,
                ok ? "Éxito" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            if (ok) CargarUsuarios();
        }

        private void btnModificarSaldo_Click(object sender, System.EventArgs e)
        {
            Usuario u = ObtenerSeleccionado();
            if (u == null) return;
            using (FrmInputSaldo frm = new FrmInputSaldo(u.Username, u.Saldo))
            {
                if (frm.ShowDialog() != DialogResult.OK) return;
                string resultado = _usuarioSvc.ModificarSaldoAdmin(u.IdUsuario, frm.NuevoSaldo);
                bool ok = resultado == "Guardado correctamente.";
                MessageBox.Show(ok ? "Saldo modificado correctamente." : resultado,
                    ok ? "Éxito" : "Error", MessageBoxButtons.OK,
                    ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                if (ok) CargarUsuarios();
            }
        }

        private void btnCambiarPassword_Click(object sender, System.EventArgs e)
        {
            Usuario u = ObtenerSeleccionado();
            if (u == null) return;
            using (FrmInputTexto frm = new FrmInputTexto("Cambiar contraseña",
                $"Nueva contraseña para '{u.Username}':"))
            {
                if (frm.ShowDialog() != DialogResult.OK ||
                    string.IsNullOrWhiteSpace(frm.Valor)) return;
                string resultado = _usuarioSvc.CambiarPassword(u.IdUsuario, frm.Valor);
                bool ok = resultado == "Guardado correctamente.";
                MessageBox.Show(ok ? "Contraseña cambiada." : resultado,
                    ok ? "Éxito" : "Error", MessageBoxButtons.OK,
                    ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
        }

        private void btnRefrescar_Click(object sender, System.EventArgs e) => CargarUsuarios();

        private void btnReporteUsuarios_Click(object sender, System.EventArgs e)
            => txtReporte.Text = _usuarioSvc.GenerarReporte();

        private void btnReportePartidas_Click(object sender, System.EventArgs e)
            => txtReporte.Text = _partidaSvc.GenerarReporte();

        private void btnCerrarSesion_Click(object sender, System.EventArgs e)
        {
            new FrmLogin().Show();
            this.Close();
        }

        private Usuario ObtenerSeleccionado()
        {
            if (dgvUsuarios.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            return dgvUsuarios.CurrentRow.DataBoundItem as Usuario;
        }
    }
}
