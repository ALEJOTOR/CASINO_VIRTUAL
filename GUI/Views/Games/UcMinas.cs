using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class UcMinas : UserControl, IVistaJuego
    {
        public event EventHandler SaldoActualizado;

        public void InicializarJuego(Usuario usuario)
        {
        }

        private readonly Usuario _usuario;
        private readonly PartidaServicio _servicio = new PartidaServicio();
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();

        private int _filas = 5;
        private int _cols = 5;
        private int _total = 25;
        private readonly List<Button> _botonesApuestas = new List<Button>();
        private readonly List<Button> _botonesMinas = new List<Button>();
        private readonly List<Button> _botonesCuadricula = new List<Button>();
        private readonly List<Label> _labelsMultiplicadores = new List<Label>();
        private FlowLayoutPanel _panelApuestasRapidas;
        private FlowLayoutPanel _panelMinasRapidas;
        private FlowLayoutPanel _panelCuadriculas;
        private FlowLayoutPanel _panelMultiplicadores;
        private Label _lblMultiplicadoresTitulo;
        private Label _lblCuadricula;
        private Button _btnApuestaMenos;
        private Button _btnApuestaMas;

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
            // Problema visual que resuelve: Minas adopta el tema global y mantiene su identidad verde.
            AppTheme.ApplyView(this);
            AppTheme.ApplyNavbar(panelHeader);
            AppTheme.ApplyTitle(lblTitulo);
            AppTheme.ApplyTextBox(txtApuesta);
            AppTheme.ApplyTextBox(txtMinas);
            AppTheme.ApplySaldoLabel(lblSaldo);
            AppTheme.ApplyPrimaryButton(btnIniciar, AppTheme.Verde);
            AppTheme.ApplyPrimaryButton(btnRetirar, AppTheme.Dorado);
            // Problema funcional que resuelve: el boton principal refleja siempre el estado real de la ronda.
            ActualizarBotonAccionMinas();
            btnIniciar.SizeChanged += (s, e) => AppTheme.ApplyRoundedRegion(btnIniciar, 10);
            btnIniciar.MouseEnter += (s, e) =>
            {
                if (_activa) btnIniciar.BackColor = Color.FromArgb(245, 158, 11);
            };
            btnIniciar.MouseLeave += (s, e) => ActualizarBotonAccionMinas();
            btnRetirar.Text = "Cobrar ganancias";
            btnRetirar.ForeColor = Color.FromArgb(15, 23, 42);
            btnRetirar.Visible = false;
            panelTablero.BackColor = Color.FromArgb(9, 35, 52);
            panelTablero.BorderStyle = BorderStyle.None;

            txtApuesta.Text = "1000";
            txtApuesta.Visible = true;
            txtMinas.Visible = false;
            lblEstado.AutoSize = false;
            lblMultiplicador.AutoSize = false;
            lblEstado.TextAlign = ContentAlignment.TopLeft;
            lblMinas.Text = "Numero de minas";
            lblApuesta.Text = "Apuesta ($)";
            // Problema visual que resuelve: los textos laterales se leen como panel de juego online y no como formulario basico.
            lblApuesta.ForeColor = AppTheme.TextoSecundario;
            lblMinas.ForeColor = AppTheme.TextoSecundario;
            lblMultiplicador.ForeColor = AppTheme.Dorado;
            lblEstado.ForeColor = AppTheme.TextoPrimario;
            lblApuesta.BackColor = Color.Transparent;
            lblMinas.BackColor = Color.Transparent;
            lblMultiplicador.BackColor = Color.Transparent;
            lblEstado.BackColor = Color.Transparent;
            lblApuesta.Font = AppTheme.Subtitulo;
            lblMinas.Font = AppTheme.Subtitulo;
            lblMultiplicador.Font = AppTheme.Valor;
            lblEstado.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);

            CrearControlesRapidos();
            panelTablero.Paint += panelTablero_Paint;
            Paint += UcMinas_Paint;
            Resize += (s, e) => AplicarLayout();
            panelTablero.Resize += (s, e) => ReubicarCeldas();
            AplicarLayout();
        }

        private void CrearControlesRapidos()
        {
            _panelApuestasRapidas = CrearPanelOpciones();
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
            Controls.Add(_panelApuestasRapidas);
            Controls.Add(_panelMinasRapidas);
            Controls.Add(_panelCuadriculas);
            Controls.Add(_panelMultiplicadores);

            // Problema funcional que resuelve: la apuesta se edita como en un casino online, con campo unico y controles +/-.
            _btnApuestaMenos = CrearBotonOpcion("-", -1, BotonAjustarApuesta_Click);
            _btnApuestaMas = CrearBotonOpcion("+", 1, BotonAjustarApuesta_Click);
            _panelApuestasRapidas.Controls.Add(_btnApuestaMenos);
            _panelApuestasRapidas.Controls.Add(txtApuesta);
            _panelApuestasRapidas.Controls.Add(_btnApuestaMas);

            foreach (int minas in ObtenerMinasRecomendadas())
                _botonesMinas.Add(CrearBotonOpcion($"{minas}", minas, BotonMinas_Click));
            _botonesMinas.Add(CrearBotonOpcion("Personal", 0, BotonMinas_Click));

            foreach (int tamano in new[] { 3, 5, 7, 9 })
                _botonesCuadricula.Add(CrearBotonOpcion($"{tamano}x{tamano}", tamano, BotonCuadricula_Click));

            foreach (Button boton in _botonesMinas)
                _panelMinasRapidas.Controls.Add(boton);

            foreach (Button boton in _botonesCuadricula)
                _panelCuadriculas.Controls.Add(boton);

            MarcarBotonActivo(_botonesMinas, "5");
            MarcarBotonActivo(_botonesCuadricula, "5x5");
            ActualizarBotonesMinasPermitidos();
            ActualizarBarraMultiplicadores();
        }

        private void AgregarBotonApuesta(string texto, int valor)
        {
            Button boton = CrearBotonOpcion(texto, valor, BotonApuesta_Click);
            boton.Size = new Size(92, 36);
            _botonesApuestas.Add(boton);
        }

        private FlowLayoutPanel CrearPanelOpciones()
        {
            return new FlowLayoutPanel
            {
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = Padding.Empty,
                Margin = Padding.Empty,
                AutoScroll = false,
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
                BackColor = Color.Transparent,
                Text = texto
            };
        }

        private Button CrearBotonOpcion(string texto, int valor, EventHandler click)
        {
            // Problema visual que resuelve: los selectores imitan chips de configuracion de casino online.
            Button boton = new Button
            {
                BackColor = Color.FromArgb(18, 27, 42),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.25F, FontStyle.Bold),
                ForeColor = AppTheme.TextoPrimario,
                Size = new Size(76, 38),
                Tag = valor,
                Text = texto,
                TextAlign = ContentAlignment.MiddleCenter,
                UseVisualStyleBackColor = false
            };

            boton.FlatAppearance.BorderColor = Color.FromArgb(44, 62, 82);
            boton.FlatAppearance.BorderSize = 1;
            boton.Margin = new Padding(0, 0, 6, 6);
            boton.Padding = Padding.Empty;
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
                        Text = "",
                        Tag = i * _cols + j,
                        Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                        BackColor = Color.FromArgb(28, 55, 72),
                        ForeColor = Color.White,
                        Enabled = false,
                        FlatStyle = FlatStyle.Flat,
                        UseVisualStyleBackColor = false
                    };
                    // Problema visual que resuelve: las casillas se ven como tiles modernos, no como botones clasicos.
                    btn.FlatAppearance.BorderColor = Color.FromArgb(47, 78, 98);
                    btn.FlatAppearance.BorderSize = 2;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(35, 76, 96);
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(22, 163, 74);
                    btn.Click += Celda_Click;
                    _celdas[i, j] = btn;
                    panelTablero.Controls.Add(btn);
                }

            ReubicarCeldas();
        }

        private void AplicarLayout()
        {
            if (Width <= 0 || Height <= 0) return;

            // Problema visual que resuelve: separa claramente panel de apuestas y tablero central estilo Stake/Mines.
            int margen = 28;
            int top = panelHeader.Bottom + 26;
            int panelLateral = Math.Min(430, Math.Max(376, Width / 3));
            int tableroMax = Math.Min(680, Math.Min(Width - panelLateral - margen * 3, Height - top - margen));
            int tableroSize = Math.Max(380, tableroMax);
            int controlX = margen + 20;
            int controlW = panelLateral - 70;

            lblApuesta.SetBounds(controlX, top + 20, controlW, 22);
            _panelApuestasRapidas.SetBounds(controlX, top + 48, controlW, 90);

            lblMinas.SetBounds(controlX, top + 162, controlW, 22);
            _panelMinasRapidas.SetBounds(controlX, top + 190, controlW, 50);
            txtMinas.SetBounds(controlX, top + 182, 1, 1);

            _lblCuadricula.SetBounds(controlX, top + 250, controlW, 22);
            _panelCuadriculas.SetBounds(controlX, top + 278, controlW, 46);

            btnIniciar.SetBounds(controlX, top + 354, controlW, 52);
            AppTheme.ApplyRoundedRegion(btnIniciar, 10);
            btnRetirar.SetBounds(controlX, top + 410, controlW, 1);

            lblMultiplicador.SetBounds(controlX, top + 482, controlW, 32);
            lblEstado.SetBounds(controlX, top + 524, controlW, 82);
            lblSaldo.SetBounds(controlX, top + 616, controlW, 28);

            int xTablero = panelLateral + margen * 2;
            _lblMultiplicadoresTitulo.SetBounds(xTablero, top - 2, Math.Min(tableroSize, Width - xTablero - margen), 24);
            _panelMultiplicadores.SetBounds(xTablero, top + 28, Math.Min(tableroSize, Width - xTablero - margen), 44);

            int yTablero = Math.Max(top + 78, panelHeader.Bottom + (Height - panelHeader.Height - tableroSize) / 2 + 40);
            panelTablero.SetBounds(xTablero, yTablero, Math.Min(tableroSize, Width - xTablero - margen), Math.Min(tableroSize, Height - yTablero - margen));
            ActualizarBarraMultiplicadores();
            ReubicarCeldas();
            AjustarBotonesConfiguracion(controlW);
            Invalidate();
        }

        private void AjustarBotonesConfiguracion(int anchoDisponible)
        {
            // Problema visual que resuelve: los chips caben en una sola fila por seccion y el texto no queda cortado.
            int anchoCuatro = Math.Max(62, (anchoDisponible - 24) / 4);
            int anchoTres = Math.Max(82, (anchoDisponible - 18) / 3);

            if (_btnApuestaMenos != null && _btnApuestaMas != null)
            {
                // Problema visual que resuelve: la apuesta queda en una sola linea editable, sin una parrilla de fichas que confundia la accion principal.
                _btnApuestaMenos.Size = new Size(48, 42);
                _btnApuestaMas.Size = new Size(48, 42);
                txtApuesta.Size = new Size(Math.Max(140, anchoDisponible - 116), 42);
                txtApuesta.Height = 42;
                txtApuesta.Margin = new Padding(0, 0, 6, 0);
                txtApuesta.TextAlign = HorizontalAlignment.Center;
                txtApuesta.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            }

            foreach (Button boton in _botonesMinas)
            {
                boton.Size = new Size(Math.Max(66, (anchoDisponible - 30) / 5), 38);
                boton.Font = new Font("Segoe UI", boton.Text == "Personal" ? 8.25F : 10F, FontStyle.Bold);
                boton.TextAlign = ContentAlignment.MiddleCenter;
                boton.Padding = Padding.Empty;
                boton.Margin = new Padding(0, 0, 6, 0);
            }

            foreach (Button boton in _botonesCuadricula)
            {
                boton.Size = new Size(anchoCuatro, 38);
                boton.Font = new Font("Segoe UI", 9.25F, FontStyle.Bold);
                boton.TextAlign = ContentAlignment.MiddleCenter;
                boton.Padding = Padding.Empty;
                boton.Margin = new Padding(0, 0, 6, 0);
            }
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

        private int MinasMinimasPorCuadricula()
        {
            // Problema funcional que resuelve: cada tamano de tablero exige una dificultad minima coherente con sus casillas.
            return 1;
        }

        private int[] ObtenerMinasRecomendadas()
        {
            // Problema funcional que resuelve: los cuatro accesos rapidos cambian segun el tamano de cuadricula.
            if (_total <= 9) return new[] { 1, 2, 3, 5 };
            if (_total <= 25) return new[] { 1, 3, 5, 7 };
            if (_total <= 49) return new[] { 3, 5, 7, 10 };
            return new[] { 1, 3, 5, 10 };
        }

        private void ReconstruirBotonesMinas()
        {
            _panelMinasRapidas.Controls.Clear();
            _botonesMinas.Clear();

            foreach (int minas in ObtenerMinasRecomendadas())
                _botonesMinas.Add(CrearBotonOpcion($"{minas}", minas, BotonMinas_Click));

            _botonesMinas.Add(CrearBotonOpcion("Personal", 0, BotonMinas_Click));

            foreach (Button boton in _botonesMinas)
                _panelMinasRapidas.Controls.Add(boton);

            MarcarBotonMinasDesdeTexto();
            ActualizarBotonesMinasPermitidos();
        }

        private void AjustarMinasAlTamano()
        {
            int minimo = MinasMinimasPorCuadricula();
            if (!int.TryParse(txtMinas.Text, out int minasActuales) || minasActuales < minimo)
                txtMinas.Text = minimo.ToString();

            if (int.TryParse(txtMinas.Text, out minasActuales) && minasActuales >= _total)
                txtMinas.Text = (_total - 1).ToString();

            MarcarBotonMinasDesdeTexto();
            ActualizarBotonesMinasPermitidos();
        }

        // ── Iniciar partida ───────────────────────────────────────

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            if (_activa)
            {
                CobrarPartidaActiva();
                return;
            }
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
            int minObligatorio = Math.Max(MinasMinimasPorApuesta(_apuesta, _usuario.Saldo), MinasMinimasPorCuadricula());
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
            // Problema funcional que resuelve: un solo boton controla la ronda, como en Mines real: primero apuesta y luego cobra.
            ActualizarBotonAccionMinas();
            btnRetirar.Enabled = false;

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

            // Problema visual que resuelve: las celdas cerradas se ven limpias como tiles de Mines real, sin signos "?". 
            for (int i = 0; i < _filas; i++)
                for (int j = 0; j < _cols; j++)
                {
                    _celdas[i, j].Text = "";
                    _celdas[i, j].BackColor = Color.FromArgb(28, 55, 72);
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
                ActualizarBotonAccionMinas();

                // Destapó todas las celdas seguras — gana automáticamente
                if (_destapadas >= _total - _nMinas)
                    TerminarPartida(true);
            }
        }

        // ── Retirar ───────────────────────────────────────────────

        private void btnRetirar_Click(object sender, EventArgs e)
        {
            CobrarPartidaActiva();
        }

        private void CobrarPartidaActiva()
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
                Ganancia = ganancia
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
                    if (_esMina[i, j] && string.IsNullOrEmpty(_celdas[i, j].Text))
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
            ActualizarBotonAccionMinas();
            btnRetirar.Enabled = false;
            txtApuesta.Enabled = true;
            txtMinas.Enabled = true;
            HabilitarOpciones(true);
            ActualizarBarraMultiplicadores();
        }

        private void ActualizarBotonAccionMinas()
        {
            if (btnIniciar == null) return;

            if (_activa)
            {
                decimal proyectado = _destapadas > 0 ? Math.Round(_apuesta * _multiplicador, 2) : 0m;
                btnIniciar.Text = _destapadas > 0 ? $"Cobrar ${proyectado:N2}" : "Cobrar";
                btnIniciar.BackColor = AppTheme.Dorado;
                btnIniciar.ForeColor = Color.FromArgb(15, 23, 42);
                btnIniciar.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                btnIniciar.TextAlign = ContentAlignment.MiddleCenter;
                btnIniciar.FlatAppearance.BorderColor = Color.FromArgb(253, 224, 71);
                btnIniciar.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 158, 11);
                AppTheme.ApplyRoundedRegion(btnIniciar, 10);
                return;
            }

            btnIniciar.Text = "Apostar";
            btnIniciar.BackColor = AppTheme.Verde;
            btnIniciar.ForeColor = Color.FromArgb(7, 18, 13);
            btnIniciar.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnIniciar.TextAlign = ContentAlignment.MiddleCenter;
            btnIniciar.FlatAppearance.BorderColor = Color.FromArgb(134, 239, 172);
            btnIniciar.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 163, 74);
            AppTheme.ApplyRoundedRegion(btnIniciar, 10);
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

        private void BotonApuesta_Click(object sender, EventArgs e)
        {
            if (_activa) return;

            // Problema visual que resuelve: la apuesta cambia desde fichas visibles, sin campos manuales.
            Button boton = (Button)sender;
            txtApuesta.Text = ((int)boton.Tag).ToString();
            MarcarBotonActivo(_botonesApuestas, boton.Text);
        }

        private void BotonAjustarApuesta_Click(object sender, EventArgs e)
        {
            if (_activa) return;

            Button boton = (Button)sender;
            int direccion = (int)boton.Tag;
            if (!decimal.TryParse(txtApuesta.Text, out decimal valor))
                valor = 1000m;

            decimal paso = valor < 5000m ? 500m : 1000m;
            valor = Math.Max(500m, valor + direccion * paso);
            txtApuesta.Text = valor.ToString("0");
        }

        private void BotonMinas_Click(object sender, EventArgs e)
        {
            if (_activa) return;

            Button boton = (Button)sender;
            int minas = (int)boton.Tag;
            if (minas == 0)
            {
                minas = PedirMinasPersonalizadas();
                if (minas == 0) return;
            }

            if (minas > 0 && minas < MinasMinimasPorCuadricula())
            {
                // Problema funcional que resuelve: impide elegir chips de minas por debajo del minimo de la cuadricula.
                MostrarError($"En tablero {_filas}x{_cols} debes usar minimo {MinasMinimasPorCuadricula()} minas.");
                txtMinas.Text = MinasMinimasPorCuadricula().ToString();
                MarcarBotonMinasDesdeTexto();
                ActualizarBarraMultiplicadores();
                return;
            }

            txtMinas.Text = Math.Min(minas, _total - 1).ToString();
            MarcarBotonMinasDesdeTexto();
            ActualizarBarraMultiplicadores();
        }

        private int PedirMinasPersonalizadas()
        {
            using (Form dialogo = new Form())
            using (Label etiqueta = new Label())
            using (TextBox input = new TextBox())
            using (Button aceptar = new Button())
            using (Button cancelar = new Button())
            {
                // Problema funcional que resuelve: el quinto boton permite ingresar minas personalizadas sin dejar un campo visible permanente.
                dialogo.Text = "Minas personalizadas";
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.MaximizeBox = false;
                dialogo.MinimizeBox = false;
                dialogo.ClientSize = new Size(320, 150);
                dialogo.BackColor = AppTheme.BgCard;

                etiqueta.Text = $"Cantidad de minas (1 a {_total - 1})";
                etiqueta.ForeColor = AppTheme.TextoPrimario;
                etiqueta.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                etiqueta.SetBounds(18, 18, 280, 24);

                input.BackColor = AppTheme.BgInput;
                input.ForeColor = AppTheme.TextoPrimario;
                input.BorderStyle = BorderStyle.FixedSingle;
                input.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                input.Text = txtMinas.Text;
                input.SetBounds(18, 52, 280, 32);

                aceptar.Text = "Aceptar";
                aceptar.SetBounds(68, 104, 104, 34);
                cancelar.Text = "Cancelar";
                cancelar.SetBounds(184, 104, 104, 34);
                AppTheme.ApplyPrimaryButton(aceptar, AppTheme.Verde);
                AppTheme.ApplySecondaryButton(cancelar);
                aceptar.DialogResult = DialogResult.OK;
                cancelar.DialogResult = DialogResult.Cancel;

                dialogo.Controls.Add(etiqueta);
                dialogo.Controls.Add(input);
                dialogo.Controls.Add(aceptar);
                dialogo.Controls.Add(cancelar);
                dialogo.AcceptButton = aceptar;
                dialogo.CancelButton = cancelar;

                if (dialogo.ShowDialog(this) != DialogResult.OK)
                    return 0;

                if (!int.TryParse(input.Text, out int minas) || minas < 1 || minas >= _total)
                {
                    MostrarError($"El numero de minas debe estar entre 1 y {_total - 1}.");
                    return 0;
                }

                return minas;
            }
        }

        private void BotonCuadricula_Click(object sender, EventArgs e)
        {
            if (_activa) return;

            Button boton = (Button)sender;
            int tamano = (int)boton.Tag;
            _filas = tamano;
            _cols = tamano;
            _total = tamano * tamano;

            // Problema funcional que resuelve: al cambiar el tamano, la cantidad de minas se corrige automaticamente.
            ReconstruirBotonesMinas();
            AjustarMinasAlTamano();
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
                // Problema visual que resuelve: el estado activo queda claro sin saturar la pantalla.
                boton.BackColor = activo ? AppTheme.Verde : Color.FromArgb(18, 27, 42);
                boton.ForeColor = activo ? Color.FromArgb(7, 18, 13) : AppTheme.TextoPrimario;
                boton.FlatAppearance.BorderColor = activo ? Color.FromArgb(134, 239, 172) : Color.FromArgb(44, 62, 82);
                boton.FlatAppearance.BorderSize = activo ? 2 : 1;
            }
        }

        private void MarcarBotonMinasDesdeTexto()
        {
            bool coincideRapido = false;
            foreach (Button boton in _botonesMinas)
            {
                bool activo = boton.Tag is int valor && valor > 0 && valor.ToString() == txtMinas.Text;
                if (activo) coincideRapido = true;
                boton.BackColor = activo ? AppTheme.Verde : Color.FromArgb(18, 27, 42);
                boton.ForeColor = activo ? Color.FromArgb(7, 18, 13) : AppTheme.TextoPrimario;
                boton.FlatAppearance.BorderColor = activo ? Color.FromArgb(134, 239, 172) : Color.FromArgb(44, 62, 82);
                boton.FlatAppearance.BorderSize = activo ? 2 : 1;
            }

            if (!coincideRapido)
            {
                Button personal = _botonesMinas.Find(b => b.Tag is int valor && valor == 0);
                if (personal != null)
                {
                    personal.BackColor = AppTheme.Verde;
                    personal.ForeColor = Color.FromArgb(7, 18, 13);
                    personal.FlatAppearance.BorderColor = Color.FromArgb(134, 239, 172);
                    personal.FlatAppearance.BorderSize = 2;
                }
            }
        }

        private void HabilitarOpciones(bool habilitar)
        {
            if (_btnApuestaMenos != null) _btnApuestaMenos.Enabled = habilitar;
            if (_btnApuestaMas != null) _btnApuestaMas.Enabled = habilitar;

            foreach (Button boton in _botonesApuestas)
                boton.Enabled = habilitar;

            foreach (Button boton in _botonesMinas)
                boton.Enabled = habilitar;

            foreach (Button boton in _botonesCuadricula)
                boton.Enabled = habilitar;
        }

        private bool EsBotonMinasPermitido(Button boton)
        {
            if (!(boton.Tag is int minas)) return true;
            if (minas == 0) return true;
            return minas >= MinasMinimasPorCuadricula();
        }

        private void ActualizarBotonesMinasPermitidos()
        {
            // Problema visual que resuelve: las opciones no permitidas se apagan en vez de parecer clicables.
            foreach (Button boton in _botonesMinas)
            {
                bool permitido = EsBotonMinasPermitido(boton);
                boton.Enabled = !_activa;
                if (!permitido)
                {
                    boton.BackColor = Color.FromArgb(15, 23, 42);
                    boton.ForeColor = Color.FromArgb(148, 163, 184);
                    boton.FlatAppearance.BorderColor = Color.FromArgb(30, 41, 59);
                }
            }
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
            int anchoChip = Math.Max(74, Math.Min(104, (anchoDisponible - cantidad * 10) / cantidad));

            for (int reveladas = inicio; reveladas <= fin; reveladas++)
            {
                decimal mult = CalcularMultiplicador(_total, minas, reveladas);
                bool siguiente = reveladas == _destapadas + 1 && _activa;
                Label label = new Label
                {
                    BackColor = siguiente
                        ? AppTheme.Dorado
                        : Color.FromArgb(20, 32, 48),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = siguiente
                        ? Color.FromArgb(17, 24, 39)
                        : AppTheme.TextoPrimario,
                    Margin = new Padding(0, 0, 8, 0),
                    Size = new Size(anchoChip, 34),
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

            // Problema funcional que resuelve: los multiplicadores reflejan el margen de casino de Mines real; con poco riesgo pueden iniciar por debajo de x1.
            return Math.Round(0.96m / probabilidad, 2, MidpointRounding.AwayFromZero);
        }

        private void panelTablero_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.Transparent);

            Rectangle area = new Rectangle(10, 10, panelTablero.ClientSize.Width - 21, panelTablero.ClientSize.Height - 21);
            // Problema visual que resuelve: el tablero gana profundidad y se acerca a un canvas de juego online.
            using (GraphicsPath path = RoundedPath(area, 18))
            using (LinearGradientBrush fondo = new LinearGradientBrush(area,
                Color.FromArgb(17, 49, 65), Color.FromArgb(7, 23, 34), 90f))
            {
                e.Graphics.FillPath(fondo, path);
            }

            using (GraphicsPath path = RoundedPath(area, 18))
            using (Pen borde = new Pen(Color.FromArgb(48, 116, 143), 3))
                e.Graphics.DrawPath(borde, path);

            using (SolidBrush luz = new SolidBrush(Color.FromArgb(18, AppTheme.Verde)))
                e.Graphics.FillEllipse(luz, -panelTablero.Width / 5, -panelTablero.Height / 4, panelTablero.Width, panelTablero.Height);
        }

        private void UcMinas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int ancho = Math.Min(430, Math.Max(376, Width / 3));
            Rectangle panelConfig = new Rectangle(24, panelHeader.Bottom + 22, ancho, Math.Max(560, Height - panelHeader.Bottom - 54));
            // Problema visual que resuelve: el panel de apuesta queda como sidebar de casino online.
            using (GraphicsPath path = RoundedPath(panelConfig, 14))
            using (LinearGradientBrush fondo = new LinearGradientBrush(panelConfig,
                Color.FromArgb(22, 31, 43), Color.FromArgb(12, 18, 30), 90F))
            {
                e.Graphics.FillPath(fondo, path);
                using (Pen borde = new Pen(Color.FromArgb(64, 84, 105), 1))
                    e.Graphics.DrawPath(borde, path);
            }
        }

        private GraphicsPath RoundedPath(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}


