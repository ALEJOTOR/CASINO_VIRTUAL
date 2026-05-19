using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class MainForm : Form
    {
        private readonly Usuario _usuario;
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();

        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        public MainForm(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            cargarDatos();
        }

        private void btnJugarAhora_Click(object sender, EventArgs e)
        {
            MessageBox.Show("¡Jugar ahora!");
        }

        private void btnMinas_Click(object sender, EventArgs e)
        {
            new FrmMinas(_usuario).ShowDialog();
        }

        private void btnRuleta_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Entrar a Ruleta");
        }

        private void btnSlot_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Entrar a Tragamonedas");
        }

        private void cargarDatos()
        {
            Usuario u = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario);
            if (u != null) _usuario.Saldo = u.Saldo;
            lblBienvenido.Text = $"Bienvenido, {_usuario.Nombre1} {_usuario.Apellido1}";
            lblSaldo.Text = $"Saldo: ${_usuario.Saldo:N2}";
        }

    }
}
