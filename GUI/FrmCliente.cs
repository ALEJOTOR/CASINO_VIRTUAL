using System.Windows.Forms;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class FrmCliente : Form
    {
        private readonly Usuario         _usuario;
        private readonly PartidaServicio _partidaSvc  = new PartidaServicio();
        private readonly UsuarioServicio _usuarioSvc  = new UsuarioServicio();

        public FrmCliente(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            Usuario u = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario);
            if (u != null) _usuario.Saldo = u.Saldo;
            lblBienvenida.Text = $"Bienvenido, {_usuario.Nombre1} {_usuario.Apellido1}";
            lblSaldo.Text      = $"Saldo disponible: ${_usuario.Saldo:N2}";
            CargarHistorial();
            CargarTransacciones();
        }

        private void CargarHistorial()
        {
            dgvPartidas.DataSource = null;
            dgvPartidas.DataSource = _partidaSvc.ObtenerPorUsuario(_usuario.IdUsuario);
        }

        private void CargarTransacciones()
        {
            dgvTransacciones.DataSource = null;
            dgvTransacciones.DataSource = _partidaSvc.ObtenerTransaccionesPorUsuario(_usuario.IdUsuario);
        }

        private void btnDepositar_Click(object sender, System.EventArgs e)
        {
            if (!decimal.TryParse(txtMonto.Text, out decimal monto) || monto <= 0)
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
                    MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                if (ok) { txtMonto.Clear(); CargarDatos(); }
            }
            finally
            {
                btnDepositar.Enabled = true;
            }
        }

        private void btnJugarMinas_Click(object sender, System.EventArgs e)
        {
            new FrmMinas(_usuario).ShowDialog();
            CargarDatos();
        }

        private void btnCerrarSesion_Click(object sender, System.EventArgs e)
        {
            new FrmLogin().Show();
            this.Close();
        }
    }
}
