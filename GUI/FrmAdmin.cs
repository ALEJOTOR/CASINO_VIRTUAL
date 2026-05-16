using System;
using System.Windows.Forms;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class FrmAdmin : Form
    {
        private readonly Usuario    _admin;
        private readonly UsuarioBLL _usuarioBll = new UsuarioBLL();
        private readonly PartidaBLL _partidaBll = new PartidaBLL();

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
            dgvUsuarios.DataSource = _usuarioBll.ObtenerTodos();
        }

        private void CargarPartidas()
        {
            dgvPartidas.DataSource = null;
            dgvPartidas.DataSource = _partidaBll.ObtenerTodasPartidas();
        }

        private void CargarTransacciones()
        {
            dgvTransacciones.DataSource = null;
            dgvTransacciones.DataSource = _partidaBll.ObtenerTodasTransacciones();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabPartidas)      CargarPartidas();
            if (tabControl.SelectedTab == tabTransacciones) CargarTransacciones();
        }

        private void btnEliminarUsuario_Click(object sender, EventArgs e)
        {
            var u = ObtenerSeleccionado();
            if (u == null) return;
            if (u.IdRol == 1) { MessageBox.Show("No se puede eliminar a un administrador.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (MessageBox.Show($"Eliminar al usuario '{u.Username}'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            var (ok, msg) = _usuarioBll.Eliminar(u.IdUsuario);
            MessageBox.Show(msg, ok ? "Exito" : "Error", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            if (ok) CargarUsuarios();
        }

        private void btnSuspender_Click(object sender, EventArgs e)
        {
            var u = ObtenerSeleccionado();
            if (u == null) return;
            if (u.IdRol == 1) { MessageBox.Show("No se puede suspender a un administrador.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            string nuevoEstado = u.Estado == "suspendido" ? "activo" : "suspendido";
            var (ok, msg) = _usuarioBll.CambiarEstado(u.IdUsuario, nuevoEstado);
            MessageBox.Show(msg, ok ? "Exito" : "Error", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            if (ok) CargarUsuarios();
        }

        private void btnModificarSaldo_Click(object sender, EventArgs e)
        {
            var u = ObtenerSeleccionado();
            if (u == null) return;
            using (var frm = new FrmInputSaldo(u.Username, u.Saldo))
            {
                if (frm.ShowDialog() != DialogResult.OK) return;
                var (ok, msg) = _usuarioBll.ModificarSaldoAdmin(u.IdUsuario, frm.NuevoSaldo);
                MessageBox.Show(msg, ok ? "Exito" : "Error", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                if (ok) CargarUsuarios();
            }
        }

        private void btnCambiarPassword_Click(object sender, EventArgs e)
        {
            var u = ObtenerSeleccionado();
            if (u == null) return;
            using (var frm = new FrmInputTexto("Cambiar contrasena", $"Nueva contrasena para '{u.Username}':"))
            {
                if (frm.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(frm.Valor)) return;
                var (ok, msg) = _usuarioBll.CambiarPassword(u.IdUsuario, frm.Valor);
                MessageBox.Show(msg, ok ? "Exito" : "Error", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e) => CargarUsuarios();

        private void btnReporteUsuarios_Click(object sender, EventArgs e)
            => txtReporte.Text = _usuarioBll.GenerarReporteUsuarios();

        private void btnReportePartidas_Click(object sender, EventArgs e)
            => txtReporte.Text = _partidaBll.GenerarReportePartidas();

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            new FrmLogin().Show();
            this.Close();
        }

        private Usuario ObtenerSeleccionado()
        {
            if (dgvUsuarios.CurrentRow == null)
            { MessageBox.Show("Seleccione un usuario.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return null; }
            return dgvUsuarios.CurrentRow.DataBoundItem as Usuario;
        }
    }
}
