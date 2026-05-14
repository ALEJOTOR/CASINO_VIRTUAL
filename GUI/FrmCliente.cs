using System;
using System.Windows.Forms;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class FrmCliente : Form
    {
        private readonly Usuario    _usuario;
        private readonly PartidaBLL _partidaBll = new PartidaBLL();
        private readonly UsuarioBLL _usuarioBll = new UsuarioBLL();

        public FrmCliente(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            var u = _usuarioBll.ObtenerPorId(_usuario.IdUsuario);
            if (u != null) _usuario.Saldo = u.Saldo;

            lblBienvenida.Text = $"Bienvenido, {_usuario.Nombre1} {_usuario.Apellido1}";
            lblSaldo.Text      = $"Saldo: ${_usuario.Saldo:N2}";
            CargarHistorial();
        }

        private void CargarHistorial()
        {
            var partidas = _partidaBll.ObtenerPartidasUsuario(_usuario.IdUsuario);
            dgvPartidas.DataSource = null;
            dgvPartidas.DataSource = partidas;
        }

        private void btnDepositar_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtMonto.Text, out decimal monto) || monto <= 0)
            {
                MessageBox.Show("Ingrese un monto válido.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var (ok, msg) = _partidaBll.RealizarDeposito(_usuario.IdUsuario, monto);
            MessageBox.Show(msg, ok ? "Éxito" : "Error", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (ok)
            {
                txtMonto.Clear();
                CargarDatos();
            }
        }

        private void btnJugarMinas_Click(object sender, EventArgs e)
        {
            new FrmMinas(_usuario).ShowDialog();
            CargarDatos();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            new FrmLogin().Show();
            this.Close();
        }
    }
}
