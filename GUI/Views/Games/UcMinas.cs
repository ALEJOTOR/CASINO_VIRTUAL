using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class UcMinas : UserControl
    {
        public event EventHandler SaldoActualizado;

        private readonly Usuario _usuario;
        private readonly PartidaServicio _servicio = new PartidaServicio();
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();

        private int _filas = 5;
        private int _cols = 5;
        private int _total = 25;
        private readonly List<Button> _botonesMinas = new List<Button>();
        private readonly List<Button> _botonesCuadricula = new List<Button>();
        private readonly List<Label> _labelsMultiplicadores = new List<Label>();
        private FlowLayoutPanel _panelMinasRapidas;
        private FlowLayoutPanel _panelCuadriculas;
        private FlowLayoutPanel _panelMultiplicadores;
        private Label _lblMultiplicadoresTitulo;
        private Label _lblCuadricula;

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
            ConfigurarVista();
            CrearTablero();
            ActualizarLblSaldo();
        }

        // ── Tablero ───────────────────────────────────────────────

        private void ConfigurarVista()
        {
            DoubleBuffered = true;
            CasinoTheme.StylePage(this);
            CasinoTheme.StyleHeader(panelHeader);
            CasinoTheme.StyleTitle(lblTitulo, 23F);
            CasinoTheme.StyleInput(txtApuesta);
            CasinoTheme.StyleInput(txtMinas);
            CasinoTheme.StyleActionButton(btnIniciar, CasinoTheme.Green);
            CasinoTheme.StyleActionButton(btnRetirar, CasinoTheme.Gold);
            btnRetirar.ForeColor = Color.FromArgb(15, 23, 42);
            panelTablero.BackColor = Color.FromArgb(9, 48, 77);

            txtApuesta.Text = "1000";
            lblEstado.AutoSize = false;
            lblMultiplicador.AutoSize = false;
            lblEstado.TextAlign = ContentAlignment.TopLeft;
            lblMinas.Text = "Numero de minas";
            lblApuesta.Text = "Apuesta ($)";
            CasinoTheme.StyleLabel(lblApuesta, CasinoTheme.Text, 10F, FontStyle.Bold);
            CasinoTheme.StyleLabel(lblMinas, CasinoTheme.Text, 10F, FontStyle.Bold);
            CasinoTheme.StyleLabel(lblMultiplicador, CasinoTheme.Gold, 11F, FontStyle.Bold);
            CasinoTheme.StyleLabel(lblEstado, CasinoTheme.Muted, 10F, FontStyle.Bold);

            CrearControlesRapidos();
            panelTablero.Paint += panelTablero_Paint;
            Paint += UcMinas_Paint;
            Resize += (s, e) => AplicarLayout();
            panelTablero.Resize += (s, e) => ReubicarCeldas();
            AplicarLayout();
        }

        private void CrearControlesRapidos()
        {
            _panelMinasRapidas = CrearPanelOpciones();
            _panelCuadriculas = CrearPanelOpciones();
            _panelMultiplicadores = CrearPanelOpciones();
            _panelMultiplicadores.WrapContents = false;
            _panelMultiplicadores.AutoScroll = false;
            _panelMultiplicadores.BackColor = Color.Transparent;

            _lblCuadricula = CrearEtiqueta("Tamano de cuadricula");
            _lblMultiplicadoresTitulo = CrearEtiqueta("Proximos multiplicadores");

            Controls.Add(_lblCuadricula);
            Controls.Add(_lblMultiplicadoresTitulo);
            Controls.Add(_panelMinasRapidas);
            Controls.Add(_panelCuadriculas);
            Controls.Add(_panelMultiplicadores);

            foreach (int minas in new[] { 1, 3, 5, 7 })
                _botonesMinas.Add(CrearBotonOpcion(minas.ToString(), minas, BotonMinas_Click));

            _botonesMinas.Add(CrearBotonOpcion("Personal", 0, (s, e) => txtMinas.Focus()));

            foreach (int tamano in new[] { 3, 5, 7, 9 })
                _botonesCuadricula.Add(CrearBotonOpcion($"{tamano}x{tamano}", tamano, BotonCuadricula_Click));

            foreach (Button boton in _botonesMinas)
                _panelMinasRapidas.Controls.Add(boton);

            foreach (Button boton in _botonesCuadricula)
                _panelCuadriculas.Controls.Add(boton);

            MarcarBotonActivo(_botonesMinas, "5");
            MarcarBotonActivo(_botonesCuadricula, "5x5");
            ActualizarBarraMultiplicadores();
        }

        private FlowLayoutPanel CrearPanelOpciones()
        {
            return new FlowLayoutPanel
            {
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };
        }

        private Label CrearEtiqueta(string texto)
        {
            return new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(203, 213, 225),
                Text = texto
            };
        }

        private Button CrearBotonOpcion(string texto, int valor, EventHandler click)
        {
            Button boton = new Button
            {
                BackColor = Color.FromArgb(15, 23, 42),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(68, 30),
                Tag = valor,
                Text = texto,
                UseVisualStyleBackColor = false
            };

            boton.FlatAppearance.BorderColor = Color.FromArgb(51, 65, 85);
            boton.FlatAppearance.BorderSize = 1;
            boton.Click += click;
            return boton;
        }

        private void CrearTablero()
        {
            panelTablero.Controls.Clear();
            _celdas = new Button[_filas, _cols];
            for (int i = 0; i < _filas; i++)
                for (int j = 0; j < _cols; j++)
                {
                    var btn = new Button
                    {
                        Text = "?",
                        Tag = i * _cols + j,
                        Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                        BackColor = Color.FromArgb(31, 78, 121),
                        ForeColor = Color.White,
                        Enabled = false,
                        FlatStyle = FlatStyle.Flat,
                        UseVisualStyleBackColor = false
                    };
                    btn.FlatAppearance.BorderColor = Color.FromArgb(56, 189, 248);
                    btn.FlatAppearance.BorderSize = 1;
                    btn.Click += Celda_Click;
                    _celdas[i, j] = btn;
                    panelTablero.Controls.Add(btn);
                }

            ReubicarCeldas();
        }

        private void AplicarLayout()
        {
            if (Width <= 0 || Height <= 0) return;

            int margen = 32;
            int top = panelHeader.Bottom + 28;
            int panelLateral = Math.Min(360, Math.Max(280, Width / 4));
            int tableroMax = Math.Min(620, Math.Min(Width - panelLateral - margen * 3, Height - top - margen));
            int tableroSize = Math.Max(330, tableroMax);

            lblApuesta.SetBounds(margen, top + 18, panelLateral - margen, 24);
            txtApuesta.SetBounds(margen, top + 46, panelLateral - margen * 2, 32);

            lblMinas.SetBounds(margen, top + 94, panelLateral - margen, 22);
            _panelMinasRapidas.SetBounds(margen, top + 120, panelLateral - margen * 2, 70);
            txtMinas.SetBounds(margen, top + 194, 90, 32);

            _lblCuadricula.SetBounds(margen, top + 238, panelLateral - margen, 22);
            _panelCuadriculas.SetBounds(margen, top + 264, panelLateral - margen * 2, 70);

            btnIniciar.SetBounds(margen, top + 356, panelLateral - margen * 2, 42);
            btnRetirar.SetBounds(margen, top + 408, panelLateral - margen * 2, 42);

            lblMultiplicador.SetBounds(margen, top + 470, panelLateral - margen * 2, 28);
            lblEstado.SetBounds(margen, top + 506, panelLateral - margen * 2, 82);
            lblSaldo.SetBounds(margen, top + 594, panelLateral - margen * 2, 24);

            int xTablero = panelLateral + margen * 2;
            _lblMultiplicadoresTitulo.SetBounds(xTablero, top - 2, Math.Min(tableroSize, Width - xTablero - margen), 22);
            _panelMultiplicadores.SetBounds(xTablero, top + 24, Math.Min(tableroSize, Width - xTablero - margen), 42);

            int yTablero = Math.Max(top + 78, panelHeader.Bottom + (Height - panelHeader.Height - tableroSize) / 2 + 40);
            panelTablero.SetBounds(xTablero, yTablero, Math.Min(tableroSize, Width - xTablero - margen), Math.Min(tableroSize, Height - yTablero - margen));
            ActualizarBarraMultiplicadores();
            ReubicarCeldas();
            Invalidate();
        }

        private void ReubicarCeldas()
        {
            if (_celdas == null || panelTablero.ClientSize.Width <= 0) return;

            int padding = 18;
            int sep = Math.Max(5, panelTablero.ClientSize.Width / 80);
            int celda = Math.Min(
                (panelTablero.ClientSize.Width - padding * 2 - sep * (_cols - 1)) / _cols,
                (panelTablero.ClientSize.Height - padding * 2 - sep * (_filas - 1)) / _filas);

            celda = Math.Max(34, celda);
            int totalW = celda * _cols + sep * (_cols - 1);
            int totalH = celda * _filas + sep * (_filas - 1);
            int startX = (panelTablero.ClientSize.Width - totalW) / 2;
            int startY = (panelTablero.ClientSize.Height - totalH) / 2;

            for (int i = 0; i < _filas; i++)
                for (int j = 0; j < _cols; j++)
                {
                    Button celdaBtn = _celdas[i, j];
                    celdaBtn.SetBounds(startX + j * (celda + sep), startY + i * (celda + sep), celda, celda);
                    celdaBtn.Font = new Font("Segoe UI", Math.Max(16F, celda / 3F), FontStyle.Bold);
                }
        }

        // ── Reglas para número mínimo de minas según apuesta ──────

        private int MinasMinimasPorApuesta(decimal apuesta, decimal saldo)
        {
            return 1;
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
            if (!int.TryParse(txtMinas.Text, out _nMinas) || _nMinas < 1 || _nMinas >= _total)
            {
                MostrarError($"El número de minas debe estar entre 1 y {_total - 1}.");
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

            _nMinas = Math.Min(_nMinas, _total - 1);
            txtMinas.Text = _nMinas.ToString();
            MarcarBotonMinasDesdeTexto();

            // Validación: no se puede jugar si no hay celdas seguras
            if (_nMinas >= _total)
            {
                MostrarError($"El máximo de minas para un tablero de {_total} celdas es {_total - 1}.");
                return;
            }

            // Todo válido — iniciar partida
            _destapadas = 0;
            _multiplicador = ObtenerMultiplicador(0);
            _activa = true;

            ActualizarLblMultiplicador();
            ActualizarBarraMultiplicadores();
            lblEstado.Text = "¡Elige una celda!";
            lblEstado.ForeColor = Color.FromArgb(56, 189, 248);

            // Bloquear controles de configuración durante la partida
            txtApuesta.Enabled = false;
            txtMinas.Enabled = false;
            HabilitarOpciones(false);
            btnIniciar.Enabled = false;
            btnRetirar.Enabled = true;

            ColocarMinas();
            HabilitarCeldas(true);
        }

        // ── Lógica de minas ───────────────────────────────────────

        private void ColocarMinas()
        {
            _esMina = new bool[_filas, _cols];
            var rnd = new Random();
            var usadas = new List<int>();

            while (usadas.Count < _nMinas)
            {
                int p = rnd.Next(_total);
                if (!usadas.Contains(p)) usadas.Add(p);
            }

            foreach (int p in usadas)
                _esMina[p / _cols, p % _cols] = true;

            // Resetear visual de todas las celdas
            for (int i = 0; i < _filas; i++)
                for (int j = 0; j < _cols; j++)
                {
                    _celdas[i, j].Text = "?";
                    _celdas[i, j].BackColor = Color.FromArgb(31, 78, 121);
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

            if (_esMina[idx / _cols, idx % _cols])
            {
                // Mina — mostrar X en rojo y terminar
                btn.Text = "✕";
                btn.BackColor = Color.FromArgb(220, 38, 38);
                btn.ForeColor = Color.White;
                TerminarPartida(false);
            }
            else
            {
                _destapadas++;
                btn.Text = "★";
                btn.BackColor = Color.FromArgb(34, 197, 94);
                btn.ForeColor = Color.White;

                _multiplicador = ObtenerMultiplicador(_destapadas);
                ActualizarLblMultiplicador();
                ActualizarBarraMultiplicadores();

                // Ganancia proyectada visible en lblEstado mientras juega
                decimal proyectado = Math.Round(_apuesta * _multiplicador, 2);
                lblEstado.Text = $"Ganancia potencial: ${proyectado:N2}";
                lblEstado.ForeColor = Color.FromArgb(34, 197, 94);

                // Destapó todas las celdas seguras — gana automáticamente
                if (_destapadas >= _total - _nMinas)
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
                IdJuego = _servicio.ObtenerIdJuegoPorNombre("Minas"),
                IdEstado = gano ? 2 : 3,
                Apuesta = _apuesta,
                Ganancia = ganancia,
                Resultado = gano ? "gano" : "perdio"
            };

            if (p.IdJuego == 0)
            {
                MostrarError("No se encontro el juego Minas en la base de datos.");
                RestablecerControles();
                return;
            }

            string resultado = _servicio.RegistrarPartida(p);

            // Si la BLL rechazó la partida (saldo insuficiente, etc.)
            if (resultado != "Guardado correctamente.")
            {
                MostrarError($"Error al registrar partida: {resultado}");
                RestablecerControles();
                return;
            }

            Usuario actualizado = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario);
            if (actualizado != null) _usuario.Saldo = actualizado.Saldo;
            ActualizarLblSaldo();
            SaldoActualizado?.Invoke(this, EventArgs.Empty);
            ActualizarBarraMultiplicadores();

            if (gano)
            {
                lblEstado.Text = $"¡Ganaste ${ganancia:N2}!";
                lblEstado.ForeColor = Color.FromArgb(34, 197, 94);
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
            for (int i = 0; i < _filas; i++)
                for (int j = 0; j < _cols; j++)
                    if (_esMina[i, j] && _celdas[i, j].Text == "?")
                    {
                        _celdas[i, j].Text = "✕";
                        _celdas[i, j].BackColor = Color.FromArgb(220, 38, 38);
                        _celdas[i, j].ForeColor = Color.White;
                    }
        }

        private void HabilitarCeldas(bool v)
        {
            for (int i = 0; i < _filas; i++)
                for (int j = 0; j < _cols; j++)
                    _celdas[i, j].Enabled = v;
        }

        private void RestablecerControles()
        {
            btnIniciar.Enabled = true;
            btnRetirar.Enabled = false;
            txtApuesta.Enabled = true;
            txtMinas.Enabled = true;
            HabilitarOpciones(true);
            ActualizarBarraMultiplicadores();
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

        private void BotonMinas_Click(object sender, EventArgs e)
        {
            if (_activa) return;

            Button boton = (Button)sender;
            int minas = (int)boton.Tag;
            txtMinas.Text = Math.Min(minas, _total - 1).ToString();
            MarcarBotonActivo(_botonesMinas, boton.Text);
            ActualizarBarraMultiplicadores();
        }

        private void BotonCuadricula_Click(object sender, EventArgs e)
        {
            if (_activa) return;

            Button boton = (Button)sender;
            int tamano = (int)boton.Tag;
            _filas = tamano;
            _cols = tamano;
            _total = tamano * tamano;

            if (int.TryParse(txtMinas.Text, out int minasActuales) && minasActuales >= _total)
                txtMinas.Text = (_total - 1).ToString();

            MarcarBotonMinasDesdeTexto();
            MarcarBotonActivo(_botonesCuadricula, boton.Text);
            CrearTablero();
            AplicarLayout();
            ActualizarBarraMultiplicadores();
        }

        private void MarcarBotonActivo(List<Button> botones, string texto)
        {
            foreach (Button boton in botones)
            {
                bool activo = boton.Text == texto;
                boton.BackColor = activo ? Color.FromArgb(37, 99, 235) : Color.FromArgb(15, 23, 42);
                boton.FlatAppearance.BorderColor = activo ? Color.FromArgb(125, 211, 252) : Color.FromArgb(51, 65, 85);
                boton.FlatAppearance.BorderSize = activo ? 2 : 1;
            }
        }

        private void MarcarBotonMinasDesdeTexto()
        {
            MarcarBotonActivo(_botonesMinas, txtMinas.Text);
        }

        private void HabilitarOpciones(bool habilitar)
        {
            foreach (Button boton in _botonesMinas)
                boton.Enabled = habilitar;

            foreach (Button boton in _botonesCuadricula)
                boton.Enabled = habilitar;
        }

        private void ActualizarBarraMultiplicadores()
        {
            if (_panelMultiplicadores == null) return;

            _panelMultiplicadores.Controls.Clear();
            _labelsMultiplicadores.Clear();

            int minas = 1;
            int.TryParse(txtMinas.Text, out minas);
            minas = Math.Max(1, Math.Min(minas, _total - 1));

            int seguras = _total - minas;
            int inicio = _activa ? _destapadas + 1 : 1;
            int fin = Math.Min(seguras, inicio + 5);
            int cantidad = Math.Max(1, fin - inicio + 1);
            int anchoDisponible = Math.Max(360, _panelMultiplicadores.ClientSize.Width);
            int anchoChip = Math.Max(64, Math.Min(92, (anchoDisponible - cantidad * 8) / cantidad));

            for (int reveladas = inicio; reveladas <= fin; reveladas++)
            {
                decimal mult = CalcularMultiplicador(_total, minas, reveladas);
                bool siguiente = reveladas == _destapadas + 1 && _activa;
                Label label = new Label
                {
                    BackColor = siguiente
                        ? Color.FromArgb(234, 179, 8)
                        : Color.FromArgb(30, 41, 59),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = siguiente
                        ? Color.FromArgb(17, 24, 39)
                        : Color.White,
                    Margin = new Padding(4),
                    Size = new Size(anchoChip, 30),
                    Text = $"{mult:N2}x",
                    TextAlign = ContentAlignment.MiddleCenter
                };

                _labelsMultiplicadores.Add(label);
                _panelMultiplicadores.Controls.Add(label);
            }
        }

        private decimal ObtenerMultiplicador(int reveladas)
        {
            if (reveladas <= 0) return 1m;
            return CalcularMultiplicador(_total, _nMinas, reveladas);
        }

        private decimal CalcularMultiplicador(int total, int minas, int reveladas)
        {
            if (reveladas <= 0) return 1m;

            int seguras = total - minas;
            if (seguras <= 0 || reveladas > seguras) return 0m;

            decimal probabilidad = 1m;
            for (int i = 0; i < reveladas; i++)
                probabilidad *= (seguras - i) / (decimal)(total - i);

            if (probabilidad <= 0m) return 0m;

            return Math.Round(0.99m / probabilidad, 2, MidpointRounding.AwayFromZero);
        }

        private void panelTablero_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.FromArgb(8, 47, 73));

            Rectangle area = new Rectangle(10, 10, panelTablero.ClientSize.Width - 21, panelTablero.ClientSize.Height - 21);
            using (LinearGradientBrush fondo = new LinearGradientBrush(area,
                Color.FromArgb(12, 74, 110), Color.FromArgb(17, 24, 39), 35f))
            {
                e.Graphics.FillRectangle(fondo, area);
            }

            using (Pen borde = new Pen(Color.FromArgb(56, 189, 248), 3))
                e.Graphics.DrawRectangle(borde, area);

            using (Pen brillo = new Pen(Color.FromArgb(80, Color.White), 1))
                e.Graphics.DrawRectangle(brillo, new Rectangle(18, 18, panelTablero.ClientSize.Width - 37, panelTablero.ClientSize.Height - 37));
        }

        private void UcMinas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int ancho = Math.Min(360, Math.Max(280, Width / 4));
            Rectangle panelConfig = new Rectangle(24, panelHeader.Bottom + 22, ancho, Math.Max(420, Height - panelHeader.Bottom - 54));
            CasinoTheme.DrawBorderedPanel(e.Graphics, panelConfig, CasinoTheme.Surface, CasinoTheme.Border);
        }

        private void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}


