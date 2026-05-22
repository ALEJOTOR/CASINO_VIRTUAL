using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class UcMinas : UserControl
    {
        private readonly Usuario _usuario;
        private readonly PartidaServicio _servicio = new PartidaServicio();

        private const int FILAS = 5, COLS = 5, TOTAL = 25;
        private const int TAM = 68, SEP = 4;

        private Button[,] _celdas;
        private bool[,] _esMina;
        private bool _activa;
        private decimal _apuesta;
        private int _nMinas;
        private int _destapadas;
        private decimal _multiplicador;

        public UcMinas(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
            CrearTablero();

            // Mostrar saldo actual al abrir el formulario
            ActualizarLblSaldo();
        }

        // ── Tablero ───────────────────────────────────────────────

        private void CrearTablero()
        {
            _celdas = new Button[FILAS, COLS];
            int xBase = 86, yBase = 264;
            for (int i = 0; i < FILAS; i++)
                for (int j = 0; j < COLS; j++)
                {
                    var btn = new Button
                    {
                        Text = "?",
                        Tag = i * COLS + j,
                        Location = new Point(xBase + j * (TAM + SEP), yBase + i * (TAM + SEP)),
                        Size = new Size(TAM, TAM),
                        Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                        BackColor = Color.FromArgb(30, 64, 175),
                        ForeColor = Color.White,
                        Enabled = false,
                        FlatStyle = FlatStyle.Flat
                    };
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Click += Celda_Click;
                    _celdas[i, j] = btn;
                    this.Controls.Add(btn);
                }
        }

        // ── Reglas para número mínimo de minas según apuesta ──────

        private int MinasMinimasPorApuesta(decimal apuesta, decimal saldo)
        {
            if (saldo == 0) return 1;

            decimal porcentaje = apuesta / saldo;

            if (porcentaje >= 0.7m) return 10;  // all-in o arriesgado: mínimo 10 minas
            if (porcentaje >= 0.5m) return 8;
            if (porcentaje >= 0.3m) return 5;
            if (porcentaje >= 0.1m) return 3;
            return 1; // apuestas bajas, deja jugar con una mina
        }

        // ── Iniciar partida ───────────────────────────────────────

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            // Validación 1: apuesta es un número válido y mayor a 0
            if (!decimal.TryParse(txtApuesta.Text, out _apuesta) || _apuesta <= 0)
            {
                MostrarError("Ingresa una apuesta válida mayor a $0.");
                txtApuesta.Focus();
                return;
            }

            // Validación 2: apuesta no supera el saldo disponible
            if (_apuesta > _usuario.Saldo)
            {
                MostrarError($"Saldo insuficiente. Tu saldo es ${_usuario.Saldo:N2}.");
                txtApuesta.Text = _usuario.Saldo.ToString("F2");
                txtApuesta.Focus();
                return;
            }

            // Validación 3: número de minas
            if (!int.TryParse(txtMinas.Text, out _nMinas) || _nMinas < 1 || _nMinas > 20)
            {
                MostrarError("El número de minas debe estar entre 1 y 20.");
                txtMinas.Focus();
                return;
            }

            // Nueva validación: acorde a valor apostado
            int minObligatorio = MinasMinimasPorApuesta(_apuesta, _usuario.Saldo);
            if (_nMinas < minObligatorio)
            {
                MostrarError(
                    $"Para una apuesta de ${_apuesta:N2}, el mínimo de minas permitido es {minObligatorio}.\n" +
                    $"(A mayor apuesta, mayor dificultad requerida)"
                );
                txtMinas.Text = minObligatorio.ToString();
                txtMinas.Focus();
                return;
            }

            // Validación: no se puede jugar si no hay celdas seguras
            if (_nMinas >= TOTAL)
            {
                MostrarError($"El máximo de minas para un tablero de {TOTAL} celdas es {TOTAL - 1}.");
                return;
            }

            // Todo válido — iniciar partida
            _destapadas = 0;
            _multiplicador = 1m;
            _activa = true;

            ActualizarLblMultiplicador();
            lblEstado.Text = "¡Elige una celda!";
            lblEstado.ForeColor = Color.DarkGreen;

            // Bloquear controles de configuración durante la partida
            txtApuesta.Enabled = false;
            txtMinas.Enabled = false;
            btnIniciar.Enabled = false;
            btnRetirar.Enabled = true;

            ColocarMinas();
            HabilitarCeldas(true);
        }

        // ── Lógica de minas ───────────────────────────────────────

        private void ColocarMinas()
        {
            _esMina = new bool[FILAS, COLS];
            var rnd = new Random();
            var usadas = new List<int>();

            while (usadas.Count < _nMinas)
            {
                int p = rnd.Next(TOTAL);
                if (!usadas.Contains(p)) usadas.Add(p);
            }

            foreach (int p in usadas)
                _esMina[p / COLS, p % COLS] = true;

            // Resetear visual de todas las celdas
            for (int i = 0; i < FILAS; i++)
                for (int j = 0; j < COLS; j++)
                {
                    _celdas[i, j].Text = "?";
                    _celdas[i, j].BackColor = Color.FromArgb(30, 64, 175);
                    _celdas[i, j].ForeColor = Color.White;
                    _celdas[i, j].Enabled = false;
                }
        }

        private void Celda_Click(object sender, EventArgs e)
        {
            if (!_activa) return;

            var btn = (Button)sender;
            int idx = (int)btn.Tag;
            btn.Enabled = false;

            if (_esMina[idx / COLS, idx % COLS])
            {
                // Mina — mostrar X en rojo y terminar
                btn.Text = "✕";
                btn.BackColor = Color.Crimson;
                btn.ForeColor = Color.White;
                TerminarPartida(false);
            }
            else
            {
                _destapadas++;
                btn.Text = "★";
                btn.BackColor = Color.LimeGreen;
                btn.ForeColor = Color.White;

                // Multiplicador progresivo, recompensa más si hay más minas
                decimal incremento = 1m + ((_nMinas / (decimal)TOTAL) * 0.4m);
                _multiplicador *= incremento;
                ActualizarLblMultiplicador();

                // Ganancia proyectada visible en lblEstado mientras juega
                decimal proyectado = Math.Round(_apuesta * _multiplicador, 2);
                lblEstado.Text = $"Ganancia potencial: ${proyectado:N2}";
                lblEstado.ForeColor = Color.DarkGreen;

                // Destapó todas las celdas seguras — gana automáticamente
                if (_destapadas >= TOTAL - _nMinas)
                    TerminarPartida(true);
            }
        }

        // ── Retirar ───────────────────────────────────────────────

        private void btnRetirar_Click(object sender, EventArgs e)
        {
            if (!_activa) return;

            if (_destapadas == 0)
            {
                MostrarError("Destapa al menos una celda antes de retirar.");
                return;
            }

            // Confirmar antes de retirar — evita clics accidentales
            decimal proyectado = Math.Round(_apuesta * _multiplicador, 2);
            var confirmacion = MessageBox.Show(
                $"¿Deseas retirar ${proyectado:N2}?\n\nSi continúas jugando podrías ganar más, pero también perder todo.",
                "Confirmar retiro",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
                TerminarPartida(true);
        }

        // ── Fin de partida ────────────────────────────────────────

        private void TerminarPartida(bool gano)
        {
            _activa = false;
            HabilitarCeldas(false);
            MostrarMinas();

            decimal ganancia = gano ? Math.Round(_apuesta * _multiplicador, 2) : 0m;

            Partida p = new Partida
            {
                IdUsuario = _usuario.IdUsuario,
                IdJuego = 1,
                IdEstado = gano ? 2 : 3,
                Apuesta = _apuesta,
                Ganancia = ganancia,
                Resultado = gano ? "gano" : "perdio"
            };

            string resultado = _servicio.RegistrarPartida(p);

            // Si la BLL rechazó la partida (saldo insuficiente, etc.)
            if (resultado != "Guardado correctamente.")
            {
                MostrarError($"Error al registrar partida: {resultado}");
                RestablecerControles();
                return;
            }

            if (gano)
                _usuario.Saldo = _usuario.Saldo - _apuesta + ganancia;
            else
                _usuario.Saldo = _usuario.Saldo - _apuesta;

            ActualizarLblSaldo();

            if (gano)
            {
                lblEstado.Text = $"¡Ganaste ${ganancia:N2}!";
                lblEstado.ForeColor = Color.DarkGreen;
                MessageBox.Show(
                    $"¡Felicidades!\n\nGanaste ${ganancia:N2} con un multiplicador de x{_multiplicador:N2}.\nTu nuevo saldo: ${_usuario.Saldo:N2}",
                    "¡Victoria!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                lblEstado.Text = $"¡Mina! Perdiste ${_apuesta:N2}";
                lblEstado.ForeColor = Color.Crimson;
                MessageBox.Show(
                    $"¡Encontraste una mina!\n\nPerdiste ${_apuesta:N2}.\nTu nuevo saldo: ${_usuario.Saldo:N2}",
                    "Perdiste",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            RestablecerControles();
        }

        // ── Helpers visuales ──────────────────────────────────────

        private void MostrarMinas()
        {
            for (int i = 0; i < FILAS; i++)
                for (int j = 0; j < COLS; j++)
                    if (_esMina[i, j] && _celdas[i, j].Text == "?")
                    {
                        _celdas[i, j].Text = "✕";
                        _celdas[i, j].BackColor = Color.OrangeRed;
                        _celdas[i, j].ForeColor = Color.White;
                    }
        }

        private void HabilitarCeldas(bool v)
        {
            for (int i = 0; i < FILAS; i++)
                for (int j = 0; j < COLS; j++)
                    _celdas[i, j].Enabled = v;
        }

        private void RestablecerControles()
        {
            btnIniciar.Enabled = true;
            btnRetirar.Enabled = false;
            txtApuesta.Enabled = true;
            txtMinas.Enabled = true;
        }

        private void ActualizarLblSaldo()
        {
            lblSaldo.Text = $"Saldo disponible: ${_usuario.Saldo:N2}";
            lblSaldo.ForeColor = _usuario.Saldo > 0 ? Color.DarkGreen : Color.Crimson;
        }

        private void ActualizarLblMultiplicador()
        {
            lblMultiplicador.Text = $"Multiplicador: x{_multiplicador:N2}";
        }

        private void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}

