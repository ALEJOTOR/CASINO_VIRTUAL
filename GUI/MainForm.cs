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
        private readonly PartidaServicio _partidaSvc = new PartidaServicio();

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
            cargarDatos();
        }

        private void btnRuleta_Click(object sender, EventArgs e)
        {
            new FrmRuleta(_usuario).ShowDialog();
            cargarDatos();
        }

        private void btnSlot_Click(object sender, EventArgs e)
        {
            new FrmTragamonedas(_usuario).ShowDialog();
            cargarDatos();
        }

        private void btnDepositar_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtMontoDeposito.Text, out decimal monto) || monto <= 0)
            {
                MessageBox.Show("Ingrese un monto valido.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnDepositar.Enabled = false;

            try
            {
                string resultado = _partidaSvc.RealizarDeposito(_usuario.IdUsuario, monto);
                bool ok = resultado == "Deposito realizado correctamente.";

                MessageBox.Show(resultado, ok ? "Exito" : "Error",
                    MessageBoxButtons.OK,
                    ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                if (ok)
                {
                    txtMontoDeposito.Clear();
                    cargarDatos();
                }
            }
            finally
            {
                btnDepositar.Enabled = true;
            }
        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            new FrmCliente(_usuario).ShowDialog();
            cargarDatos();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            new FrmLogin().Show();
            this.Close();
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
