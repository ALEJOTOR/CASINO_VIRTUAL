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

        private const int FILAS = 5, COLS = 5, TOTAL = 25;
        private const int TAM = 68, SEP = 4;

        private Button[,] _celdas;
        private bool[,]   _esMina;
        private bool      _activa;
        private decimal   _apuesta;
        private int       _nMinas;
        private int       _destapadas;
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
            int xBase = 12, yBase = 118;
            for (int i = 0; i < FILAS; i++)
                for (int j = 0; j < COLS; j++)
                {
                    var btn = new Button
                    {
                        Text      = "?",
                        Tag       = i * COLS + j,
                        Location  = new Point(xBase + j * (TAM + SEP), yBase + i * (TAM + SEP)),
                        Size      = new Size(TAM, TAM),
                        Font      = new Font("Segoe UI", 14F),
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
            { MessageBox.Show("Apuesta invalida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!int.TryParse(txtMinas.Text, out _nMinas) || _nMinas < 1 || _nMinas > 20)
            { MessageBox.Show("Minas: entre 1 y 20.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            _destapadas = 0; _multiplicador = 1m; _activa = true;
            lblEstado.Text = "Elige una celda!";
            lblMultiplicador.Text = "Multiplicador: x1.00";
            btnIniciar.Enabled = false;
            btnRetirar.Enabled = true;
            ColocarMinas();
            HabilitarCeldas(true);
        }

        private void ColocarMinas()
        {
            _esMina = new bool[FILAS, COLS];
            var rnd = new Random();
            var usadas = new List<int>();
            while (usadas.Count < _nMinas) { int p = rnd.Next(TOTAL); if (!usadas.Contains(p)) usadas.Add(p); }
            foreach (int p in usadas) _esMina[p / COLS, p % COLS] = true;
            for (int i = 0; i < FILAS; i++)
                for (int j = 0; j < COLS; j++)
                { _celdas[i, j].Text = "?"; _celdas[i, j].BackColor = Color.SteelBlue; _celdas[i, j].Enabled = false; }
        }

        private void Celda_Click(object sender, EventArgs e)
        {
            if (!_activa) return;
            var btn = (Button)sender;
            int idx = (int)btn.Tag;
            btn.Enabled = false;
            if (_esMina[idx / COLS, idx % COLS])
            { btn.Text = "X"; btn.BackColor = Color.Crimson; TerminarPartida(false); }
            else
            {
                _destapadas++;
                btn.Text = "*"; btn.BackColor = Color.LimeGreen;
                _multiplicador *= 1m + (_nMinas * 0.05m);
                lblMultiplicador.Text = $"Multiplicador: x{_multiplicador:N2}";
                if (_destapadas >= TOTAL - _nMinas) TerminarPartida(true);
            }
        }

        private void btnRetirar_Click(object sender, EventArgs e)
        {
            if (!_activa || _destapadas == 0)
            { MessageBox.Show("Destapa al menos una celda antes de retirar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            TerminarPartida(true);
        }

        private void TerminarPartida(bool gano)
        {
            _activa = false;
            btnIniciar.Enabled = true;
            btnRetirar.Enabled = false;
            HabilitarCeldas(false);
            MostrarMinas();
            decimal ganancia = gano ? Math.Round(_apuesta * _multiplicador, 2) : 0;
            var p = new Partida { IdUsuario = _usuario.IdUsuario, IdJuego = 1, IdEstado = gano ? 2 : 3,
                                  Apuesta = _apuesta, Ganancia = ganancia, Resultado = gano ? "gano" : "perdio" };
            _bll.RegistrarPartida(p);
            lblEstado.Text = gano ? $"Ganaste ${ganancia:N2}!" : $"Mina! Perdiste ${_apuesta:N2}";
            MessageBox.Show(lblEstado.Text, gano ? "Victoria" : "Perdiste",
                MessageBoxButtons.OK, gano ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private void MostrarMinas()
        {
            for (int i = 0; i < FILAS; i++)
                for (int j = 0; j < COLS; j++)
                    if (_esMina[i, j] && _celdas[i, j].Text == "?")
                    { _celdas[i, j].Text = "X"; _celdas[i, j].BackColor = Color.OrangeRed; }
        }

        private void HabilitarCeldas(bool v)
        {
            for (int i = 0; i < FILAS; i++)
                for (int j = 0; j < COLS; j++)
                    _celdas[i, j].Enabled = v;
        }
    }
}
