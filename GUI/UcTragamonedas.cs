using BLL;
using ENTITY;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class UcTragamonedas : UserControl
    {
        private readonly Usuario _usuario;
        private readonly PartidaServicio _servicio = new PartidaServicio();
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();
        private readonly Random _random = new Random();
        private readonly string[] _simbolos = { "7", "$", "BAR", "DIAM", "CEREZA" };

        public UcTragamonedas()
        {
            InitializeComponent();
        }

        public UcTragamonedas(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
            ActualizarSaldo();
        }

        private void btnGirar_Click(object sender, EventArgs e)
        {
            if (_usuario == null)
            {
                MessageBox.Show("Debe iniciar sesion para jugar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtApuesta.Text, out decimal apuesta) || apuesta <= 0)
            {
                MessageBox.Show("Ingrese una apuesta valida.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (apuesta > _usuario.Saldo)
            {
                MessageBox.Show($"Saldo insuficiente. Tu saldo es ${_usuario.Saldo:N2}.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnGirar.Enabled = false;

            try
            {
                string s1 = _simbolos[_random.Next(_simbolos.Length)];
                string s2 = _simbolos[_random.Next(_simbolos.Length)];
                string s3 = _simbolos[_random.Next(_simbolos.Length)];

                lblRollo1.Text = s1;
                lblRollo2.Text = s2;
                lblRollo3.Text = s3;

                decimal ganancia = CalcularGanancia(apuesta, s1, s2, s3);
                bool gano = ganancia > 0;

                Partida partida = new Partida
                {
                    IdUsuario = _usuario.IdUsuario,
                    IdJuego = _servicio.ObtenerIdJuegoPorNombre("Tragamonedas"),
                    IdEstado = gano ? 2 : 3,
                    Apuesta = apuesta,
                    Ganancia = ganancia,
                    Resultado = gano ? "gano" : "perdio"
                };

                if (partida.IdJuego == 0)
                {
                    MessageBox.Show("No se encontro el juego Tragamonedas en la base de datos. Reinicia la aplicacion para inicializarlo.",
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

                lblResultado.ForeColor = gano ? Color.FromArgb(34, 197, 94) : Color.FromArgb(248, 113, 113);
                lblResultado.Text = gano ? $"Ganaste ${ganancia:N2}" : $"Perdiste ${apuesta:N2}";
            }
            finally
            {
                btnGirar.Enabled = true;
            }
        }

        private decimal CalcularGanancia(decimal apuesta, string s1, string s2, string s3)
        {
            if (s1 == s2 && s2 == s3) return apuesta * 8m;
            if (s1 == s2 || s1 == s3 || s2 == s3) return apuesta * 2m;
            return 0m;
        }

        private void ActualizarSaldo()
        {
            lblSaldo.Text = $"Saldo: ${_usuario.Saldo:N2}";
        }
    }
}

