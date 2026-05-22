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
        private Control _vistaInicio;

        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            PrepararNavegacion();
        }

        public MainForm(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            PrepararNavegacion();
            cargarDatos();
        }

        private void btnJugarAhora_Click(object sender, EventArgs e)
        {
            MessageBox.Show("¡Jugar ahora!");
        }

        private void btnMinas_Click(object sender, EventArgs e)
        {
            MostrarVista(new UcMinas(_usuario));
        }

        private void btnRuleta_Click(object sender, EventArgs e)
        {
            MostrarVista(new UcRuleta(_usuario));
        }

        private void btnSlot_Click(object sender, EventArgs e)
        {
            MostrarVista(new UcTragamonedas(_usuario));
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
            MostrarFormulario(new FrmCliente(_usuario));
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            new FrmLogin().Show();
            this.Close();
        }

        private void cargarDatos()
        {
            if (_usuario == null) return;

            Usuario u = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario);
            if (u != null) _usuario.Saldo = u.Saldo;
            lblBienvenido.Text = $"Bienvenido, {_usuario.Nombre1} {_usuario.Apellido1}";
            lblSaldo.Text = $"Saldo: ${_usuario.Saldo:N2}";
        }

        private void PrepararNavegacion()
        {
            _vistaInicio = layoutInicio;
            inicioToolStripMenuItem.Click += (s, e) => MostrarInicio();
            juegosToolStripMenuItem.Click += (s, e) => MostrarInicio();
        }

        private void MostrarInicio()
        {
            pnlContenido.Controls.Clear();
            _vistaInicio.Dock = DockStyle.Fill;
            pnlContenido.Controls.Add(_vistaInicio);
            cargarDatos();
        }

        private void MostrarVista(UserControl vista)
        {
            if (_usuario == null)
            {
                vista.Dispose();
                return;
            }

            pnlContenido.Controls.Clear();
            vista.Dock = DockStyle.Fill;
            pnlContenido.Controls.Add(vista);
            cargarDatos();
        }

        private void MostrarFormulario(Form formulario)
        {
            pnlContenido.Controls.Clear();
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            pnlContenido.Controls.Add(formulario);
            formulario.Show();
            cargarDatos();
        }

    }
}
