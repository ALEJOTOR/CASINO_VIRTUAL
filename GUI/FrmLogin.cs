using System;
using System.Windows.Forms;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class FrmLogin : Form
    {
        private readonly UsuarioBLL _bll = new UsuarioBLL();

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Complete todos los campos.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Usuario u = _bll.Login(user, pass);

            if (u == null)
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (u.Rol?.NombreRol == "administrador")
                new FrmAdmin(u).Show();
            else
                new FrmCliente(u).Show();

            this.Hide();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            new FrmRegistro().ShowDialog();
        }
    }
}
