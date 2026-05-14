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

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabPartidas)
                CargarPartidas();
        }

        private void btnEliminarUsuario_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null) return;

            var usuario = (Usuario)dgvUsuarios.CurrentRow.DataBoundItem;
            if (usuario == null) return;

            if (usuario.IdRol == 1)
            {
                MessageBox.Show("No se puede eliminar a un administrador.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"¿Eliminar al usuario '{usuario.Username}'?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            var (ok, msg) = _usuarioBll.Eliminar(usuario.IdUsuario);
            MessageBox.Show(msg, ok ? "Éxito" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (ok) CargarUsuarios();
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            new FrmLogin().Show();
            this.Close();
        }
    }
}
