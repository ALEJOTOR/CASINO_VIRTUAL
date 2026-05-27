using System.Windows.Forms;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class FrmLogin : Form
    {
        private readonly UsuarioServicio _servicio = new UsuarioServicio();

        public FrmLogin()
        {
            InitializeComponent();
            // Problema visual que resuelve: el login queda centrado en una tarjeta oscura con tipografia y botones uniformes.
            AppTheme.ApplyForm(this);
            AppTheme.ApplyCard(panelFormulario, 12);
            AppTheme.ApplyTitle(lblTitulo);
            AppTheme.ApplySubtitle(lblSubtitulo);
            AppTheme.ApplyTitle(lblMarca);
            AppTheme.ApplySubtitle(lblMarcaDetalle);
            AppTheme.ApplyTextBox(txtUsername);
            AppTheme.ApplyTextBox(txtPassword);
            AppTheme.ApplyPrimaryButton(btnIngresar);
            AppTheme.ApplyPrimaryButton(btnRegistrar, AppTheme.BgHover);
        }

        private void btnIngresar_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Complete todos los campos.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Usuario u = _servicio.Login(txtUsername.Text.Trim(), txtPassword.Text);
            if (u == null)
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (u.IdRol == 1)
                new FrmAdmin(u).Show();
            else
                new MainForm(u).Show();

            this.Hide();
        }

        private void btnRegistrar_Click(object sender, System.EventArgs e)
        {
            new FrmRegistro().ShowDialog();
        }
    }
}
