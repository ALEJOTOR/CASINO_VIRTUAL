using BLL;
using ENTITY;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmRuleta : Form
    {
        private readonly Usuario _usuario;
        private readonly PartidaServicio _servicio = new PartidaServicio();
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();
        private readonly Random _random = new Random();

        public FrmRuleta()
        {
            InitializeComponent();
        }

        public FrmRuleta(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
            ActualizarSaldo();
        }

        private void btnApostarColor_Click(object sender, EventArgs e)
        {
            if (!ValidarApuesta(out decimal apuesta)) return;

            int numero = _random.Next(0, 37);
            string color = ObtenerColor(numero);
            string elegido = cboColor.SelectedItem == null ? "Rojo" : cboColor.SelectedItem.ToString();
            bool gano = color == elegido;
            decimal ganancia = gano ? apuesta * 2m : 0m;

            RegistrarResultado(apuesta, ganancia, gano, $"Salio {numero} {color}. Apostaste a {elegido}.");
        }

        private void btnApostarNumero_Click(object sender, EventArgs e)
        {
            if (!ValidarApuesta(out decimal apuesta)) return;

            int elegido = (int)numNumero.Value;
            int numero = _random.Next(0, 37);
            bool gano = numero == elegido;
            decimal ganancia = gano ? apuesta * 36m : 0m;

            RegistrarResultado(apuesta, ganancia, gano, $"Salio {numero}. Apostaste al {elegido}.");
        }

        private bool ValidarApuesta(out decimal apuesta)
        {
            if (_usuario == null)
            {
                apuesta = 0;
                MessageBox.Show("Debe iniciar sesion para jugar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtApuesta.Text, out apuesta) || apuesta <= 0)
            {
                MessageBox.Show("Ingrese una apuesta valida.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (apuesta > _usuario.Saldo)
            {
                MessageBox.Show($"Saldo insuficiente. Tu saldo es ${_usuario.Saldo:N2}.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            btnApostarColor.Enabled = false;
            btnApostarNumero.Enabled = false;
            return true;
        }

        private void RegistrarResultado(decimal apuesta, decimal ganancia, bool gano, string detalle)
        {
            try
            {
                Partida partida = new Partida
                {
                    IdUsuario = _usuario.IdUsuario,
                    IdJuego = _servicio.ObtenerIdJuegoPorNombre("Ruleta"),
                    IdEstado = gano ? 2 : 3,
                    Apuesta = apuesta,
                    Ganancia = ganancia,
                    Resultado = gano ? "gano" : "perdio"
                };

                if (partida.IdJuego == 0)
                {
                    MessageBox.Show("No se encontro el juego Ruleta en la base de datos. Reinicia la aplicacion para inicializarlo.",
                        "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string resultado = _servicio.RegistrarPartida(partida);
                if (resultado != "Guardado correctamente.")
                {
                    MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Usuario actualizado = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario);
                if (actualizado != null) _usuario.Saldo = actualizado.Saldo;
                ActualizarSaldo();

                lblResultado.Text = detalle;
                lblResultado.ForeColor = gano ? Color.FromArgb(34, 197, 94) : Color.FromArgb(248, 113, 113);
                lblPremio.Text = gano ? $"Ganaste ${ganancia:N2}" : $"Perdiste ${apuesta:N2}";
            }
            finally
            {
                btnApostarColor.Enabled = true;
                btnApostarNumero.Enabled = true;
            }
        }

        private string ObtenerColor(int numero)
        {
            if (numero == 0) return "Verde";

            int[] rojos = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
            foreach (int rojo in rojos)
                if (numero == rojo) return "Rojo";

            return "Negro";
        }

        private void ActualizarSaldo()
        {
            lblSaldo.Text = $"Saldo: ${_usuario.Saldo:N2}";
        }
    }
}
