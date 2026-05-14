using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class FrmMinas : Form
    {
        private readonly Usuario    _usuario;
        private readonly PartidaBLL _bll = new PartidaBLL();

        private const int FILAS = 5;
        private const int COLS  = 5;
        private const int TOTAL = FILAS * COLS;

        private Button[,] _celdas;
        private bool[,]   _esMina;
        private bool      _partidaActiva   = false;
        private decimal   _apuesta;
        private int       _minasCantidad;
        private int       _celdasDestapadas;
        private decimal   _multiplicador;

        public FrmMinas(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
            CrearTablero();
        }

        private void CrearTablero()
        {
            _celdas = new Button[FILAS, COLS];
            int tam = 65, xBase = 10, yBase = 110;

            for (int i = 0; i < FILAS; i++)
                for (int j = 0; j < COLS; j++)
                {
                    var btn = new Button
                    {
                        Text      = "?",
                        Tag       = i * COLS + j,
                        Location  = new Point(xBase + j * (tam + 4), yBase + i * (tam + 4)),
                        Size      = new Size(tam, tam),
                        Font      = new Font("Segoe UI", 13F),
                        BackColor = Color.SteelBlue,
                        ForeColor = Color.White,
                        Enabled   = false,
                        FlatStyle = FlatStyle.Flat
                    };
                    btn.Click += Celda_Click;
                    _celdas[i, j] = btn;
                    this.Controls.Add(btn);
                }
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtApuesta.Text, out _apuesta) || _apuesta <= 0)
            {
                MessageBox.Show("Ingrese una apuesta válida.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtMinas.Text, out _minasCantidad) || _minasCantidad < 1 || _minasCantidad > 20)
            {
                MessageBox.Show("Las minas deben estar entre 1 y 20.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _celdasDestapadas     = 0;
            _multiplicador        = 1m;
            _partidaActiva        = true;
            lblEstado.Text        = "Partida en curso... ¡Elige una celda!";
            lblMultiplicador.Text = "Multiplicador: x1.00";

            ColocarMinas();
            HabilitarCeldas(true);
            btnIniciar.Enabled = false;
            btnRetirar.Enabled = true;
        }

        private void ColocarMinas()
        {
            _esMina = new bool[FILAS, COLS];
            var rnd = new Random();
            var usadas = new List<int>();

            while (usadas.Count < _minasCantidad)
            {
                int p = rnd.Next(TOTAL);
                if (!usadas.Contains(p)) usadas.Add(p);
            }

            foreach (int p in usadas)
                _esMina[p / COLS, p % COLS] = true;

            for (int i = 0; i < FILAS; i++)
                for (int j = 0; j < COLS; j++)
                {
                    _celdas[i, j].Text      = "?";
                    _celdas[i, j].BackColor = Color.SteelBlue;
                    _celdas[i, j].Enabled   = false;
                }
        }

        private void Celda_Click(object sender, EventArgs e)
        {
            if (!_partidaActiva) return;
            var btn  = (Button)sender;
            int idx  = (int)btn.Tag;
            int fila = idx / COLS;
            int col  = idx % COLS;

            btn.Enabled = false;

            if (_esMina[fila, col])
            {
                btn.Text      = "💣";
                btn.BackColor = Color.Crimson;
                TerminarPartida(false);
            }
            else
            {
                _celdasDestapadas++;
                btn.Text      = "💎";
                btn.BackColor = Color.LimeGreen;

                decimal factor = 1m + (_minasCantidad * 0.05m);
                _multiplicador *= factor;
                lblMultiplicador.Text = $"Multiplicador: x{_multiplicador:N2}";

                int seguras = TOTAL - _minasCantidad;
                if (_celdasDestapadas >= seguras)
                    TerminarPartida(true);
            }
        }

        private void btnRetirar_Click(object sender, EventArgs e)
        {
            if (!_partidaActiva || _celdasDestapadas == 0)
            {
                MessageBox.Show("Destapa al menos una celda antes de retirar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            TerminarPartida(true);
        }

        private void TerminarPartida(bool gano)
        {
            _partidaActiva     = false;
            btnIniciar.Enabled = true;
            btnRetirar.Enabled = false;
            HabilitarCeldas(false);
            MostrarMinas();

            decimal ganancia = gano ? Math.Round(_apuesta * _multiplicador, 2) : 0;

            var partida = new Partida
            {
                IdUsuario = _usuario.IdUsuario,
                IdJuego   = 1,
                IdEstado  = gano ? 2 : 3,
                Apuesta   = _apuesta,
                Ganancia  = ganancia,
                Resultado = gano ? "ganó" : "perdió"
            };

            _bll.RegistrarPartida(partida);

            if (gano)
            {
                lblEstado.Text = $"¡Ganaste! Cobras ${ganancia:N2}";
                MessageBox.Show($"¡Ganaste ${ganancia:N2}!", "Victoria",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lblEstado.Text = $"💣 ¡Mina! Perdiste ${_apuesta:N2}";
                MessageBox.Show($"Tocaste una mina. Perdiste ${_apuesta:N2}.", "Perdiste",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarMinas()
        {
            for (int i = 0; i < FILAS; i++)
                for (int j = 0; j < COLS; j++)
                    if (_esMina[i, j] && _celdas[i, j].Text == "?")
                    {
                        _celdas[i, j].Text      = "💣";
                        _celdas[i, j].BackColor = Color.OrangeRed;
                    }
        }

        private void HabilitarCeldas(bool estado)
        {
            for (int i = 0; i < FILAS; i++)
                for (int j = 0; j < COLS; j++)
                    _celdas[i, j].Enabled = estado;
        }
    }
}
