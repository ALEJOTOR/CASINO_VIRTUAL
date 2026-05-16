using System;
using System.Windows.Forms;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class FrmRegistro : Form
    {
        private readonly UsuarioBLL _bll = new UsuarioBLL();

        public FrmRegistro()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var u = new Usuario
            {
                Username        = txtUsername.Text.Trim(),
                Password        = txtPassword.Text,
                Nombre1         = txtNombre1.Text.Trim(),
                Nombre2         = txtNombre2.Text.Trim(),
                Apellido1       = txtApellido1.Text.Trim(),
                Apellido2       = txtApellido2.Text.Trim(),
                Correo          = txtCorreo.Text.Trim(),
                FechaNacimiento = dtpFechaNac.Value,
                Saldo           = 0,
                IdRol           = 2
            };

            var (ok, msg) = _bll.Registrar(u);
            MessageBox.Show(msg, ok ? "Éxito" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            if (ok) this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e) => this.Close();
    }
}
