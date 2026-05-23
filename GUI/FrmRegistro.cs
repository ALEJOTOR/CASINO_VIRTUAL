using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class FrmRegistro : Form
    {
        private readonly UsuarioServicio _servicio = new UsuarioServicio();

        public FrmRegistro()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, System.EventArgs e)
        {
            if(txtIdentificacion.Text.Trim() == "" || txtUsername.Text.Trim() == "" || txtPassword.Text == "" ||
               txtNombre1.Text.Trim() == "" || txtApellido1.Text.Trim() == "" || txtCorreo.Text.Trim() == "")
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.",
                    "Campos incompletos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            Usuario u = new Usuario
            {
                IdUsuario       = int.Parse(txtIdentificacion.Text.Trim()), 
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

            if (!u.EsMayorDeEdad())
            {
                MessageBox.Show("Debes ser mayor de edad para registrarte.",
                    "Edad insuficiente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string resultado = _servicio.Registrar(u);
            bool ok = resultado == "Guardado correctamente.";
            MessageBox.Show(ok ? "Usuario registrado correctamente." : resultado,
                ok ? "Éxito" : "Error",
                MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            if (ok) this.Close();
        }

        private void btnCancelar_Click(object sender, System.EventArgs e) => this.Close();
    }
}
