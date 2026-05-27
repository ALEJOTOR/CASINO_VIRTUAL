using BLL;
using ENTITY;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmEditarUsuario : Form
    {
        private readonly Usuario _usuarioOriginal;
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();

        public FrmEditarUsuario(Usuario usuario)
        {
            _usuarioOriginal = usuario;
            InitializeComponent();

            AppTheme.ApplyForm(this);
            AppTheme.ApplySubtitle(lblId);
            AppTheme.ApplySubtitle(lblUsername);
            AppTheme.ApplySubtitle(lblPassword);
            AppTheme.ApplySubtitle(lblNombre1);
            AppTheme.ApplySubtitle(lblNombre2);
            AppTheme.ApplySubtitle(lblApellido1);
            AppTheme.ApplySubtitle(lblApellido2);
            AppTheme.ApplySubtitle(lblCorreo);
            AppTheme.ApplySubtitle(lblFechaNac);
            AppTheme.ApplySubtitle(lblEstado);
            AppTheme.ApplyTextBox(txtUsername);
            AppTheme.ApplyTextBox(txtPassword);
            AppTheme.ApplyTextBox(txtNombre1);
            AppTheme.ApplyTextBox(txtNombre2);
            AppTheme.ApplyTextBox(txtApellido1);
            AppTheme.ApplyTextBox(txtApellido2);
            AppTheme.ApplyTextBox(txtCorreo);
            AppTheme.ApplyCombo(cboEstado);
            AppTheme.ApplyPrimaryButton(btnAceptar);
            AppTheme.ApplyPrimaryButton(btnCancelar, AppTheme.BgHover);

            CargarEstados();
            CargarDatosUsuario();
            this.Text = $"Editando: {usuario.Username}";
        }

        private void CargarEstados()
        {
            try
            {
                var estados = _usuarioSvc.ObtenerEstados();

                cboEstado.DataSource = estados;
                cboEstado.DisplayMember = "Nombre";
                cboEstado.ValueMember = "Nombre";
            }
            catch
            {
                cboEstado.Items.Clear();
                cboEstado.Items.Add("activo");
                cboEstado.Items.Add("suspendido");
                cboEstado.Items.Add("inactivo");
            }
        }

        private void CargarDatosUsuario()
        {
            lblIdValor.Text = _usuarioOriginal.IdUsuario.ToString();
            txtUsername.Text = _usuarioOriginal.Username;
            txtPassword.Text = "";
            txtNombre1.Text = _usuarioOriginal.Nombre1;
            txtNombre2.Text = _usuarioOriginal.Nombre2 ?? "";
            txtApellido1.Text = _usuarioOriginal.Apellido1;
            txtApellido2.Text = _usuarioOriginal.Apellido2 ?? "";
            txtCorreo.Text = _usuarioOriginal.Correo;

            if (_usuarioOriginal.FechaNacimiento != default)
                dtpFechaNac.Value = _usuarioOriginal.FechaNacimiento;

            if (!string.IsNullOrEmpty(_usuarioOriginal.Estado))
                cboEstado.SelectedValue = _usuarioOriginal.Estado;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("El username es obligatorio.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtNombre1.Text))
            {
                MessageBox.Show("El primer nombre es obligatorio.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtApellido1.Text))
            {
                MessageBox.Show("El primer apellido es obligatorio.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                MessageBox.Show("El correo es obligatorio.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int edad = DateTime.Today.Year - dtpFechaNac.Value.Year;
            if (dtpFechaNac.Value.Date > DateTime.Today.AddYears(-edad))
                edad--;
            if (edad < 18)
            {
                MessageBox.Show("El usuario debe ser mayor de edad (18+).", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            EstadoUsuario estadoSel = cboEstado.SelectedItem as EstadoUsuario;
            if (estadoSel == null)
            {
                MessageBox.Show("Seleccione un estado.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _usuarioOriginal.Username = txtUsername.Text.Trim();
            if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                _usuarioOriginal.Password = HashPassword(txtPassword.Text);
            _usuarioOriginal.Nombre1 = txtNombre1.Text.Trim();
            _usuarioOriginal.Nombre2 = string.IsNullOrWhiteSpace(txtNombre2.Text) ? null : txtNombre2.Text.Trim();
            _usuarioOriginal.Apellido1 = txtApellido1.Text.Trim();
            _usuarioOriginal.Apellido2 = string.IsNullOrWhiteSpace(txtApellido2.Text) ? null : txtApellido2.Text.Trim();
            _usuarioOriginal.Correo = txtCorreo.Text.Trim();
            _usuarioOriginal.FechaNacimiento = dtpFechaNac.Value;
            _usuarioOriginal.Estado = estadoSel.Nombre;

            string resultado = _usuarioSvc.Actualizar(_usuarioOriginal);

            if (resultado == "Guardado correctamente.")
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
