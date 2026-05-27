using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GUI
{
    public partial class UcRuleta : UserControl, IVistaJuego
    {
        public event EventHandler SaldoActualizado;

        public void InicializarJuego(Usuario usuario)
        {
        }

        private readonly Usuario _usuario;
        private readonly PartidaServicio _servicio = new PartidaServicio();
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();
        private readonly Random _random = new Random();
        private readonly List<ApuestaRuleta> _apuestas = new List<ApuestaRuleta>();
        private readonly List<Label> _zonasNumero = new List<Label>();
        private readonly List<Label> _zonasExternas = new List<Label>();
        private decimal _fichaSeleccionada = 50m;
        private int _numeroGanadorActual = -1;

        public UcRuleta()
        {
            InitializeComponent();
            ConfigurarApuestasIniciales();
        }

        public UcRuleta(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
            ConfigurarApuestasIniciales();
            ActualizarSaldo();
        }

        private void ConfigurarApuestasIniciales()
        {
            // Problema visual que resuelve: la ruleta queda integrada al tema global sin perder su acento rojo de casino.
            AppTheme.ApplyView(this);
            AppTheme.ApplyNavbar(panelTop);
            AppTheme.ApplyTitle(lblTitulo);
            AppTheme.ApplySaldoLabel(lblSaldo);
            AppTheme.ApplyPrimaryButton(btnGirarRuleta, AppTheme.Rojo);
            AppTheme.ApplyPrimaryButton(btnLimpiarMesa, AppTheme.BgHover);
            AppTheme.ApplyTextBox(txtApuesta);
            panelJuego.BackColor = AppTheme.BgPrincipal;
            panelControles.BackColor = AppTheme.BgCard;
            panelControles.BorderStyle = BorderStyle.None;
            panelTapete.BorderStyle = BorderStyle.None;
            panelRueda.BorderStyle = BorderStyle.None;
            lblMesa.ForeColor = AppTheme.TextoPrimario;
            lblResultado.ForeColor = AppTheme.TextoSecundario;
            lblPremio.ForeColor = AppTheme.Dorado;

            txtApuesta.Text = FormatearFicha(_fichaSeleccionada);
            cboTipoApuesta.Visible = false;
            numNumero.Visible = false;
            lblNumero.Text = "Selecciona una ficha y toca la mesa";

            CrearBotonesFichas();
            CrearZonasApuesta();
            PrepararApuestasExternas();
            panelJuego.Resize += (s, e) => AplicarLayoutRuleta();
            panelTapete.Resize += (s, e) => ReubicarZonasMesa();
            panelRueda.Resize += (s, e) => ReubicarNumeroGanador();
            // Problema visual que resuelve: transforma el fondo plano en una mesa con profundidad de casino real.
            panelJuego.Paint += panelJuego_Paint;
            panelControles.Paint += panelControles_Paint;
            AplicarLayoutRuleta();
            ActualizarTotalApostado();
        }

        private void cboTipoApuesta_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnGirarRuleta_Click(object sender, EventArgs e)
        {
            if (!ValidarGiro(out decimal totalApostado)) return;

            int numero = _random.Next(0, 37);
            string color = ObtenerColor(numero);
            decimal ganancia = 0m;

            foreach (ApuestaRuleta apuesta in _apuestas)
                if (EsApuestaGanadora(apuesta, numero, color))
                    ganancia += apuesta.Monto * ObtenerMultiplicador(apuesta);

            MostrarNumeroEnRueda(numero, color);
            RegistrarResultado(totalApostado, ganancia, ganancia > 0,
                $"Salio {numero} {color}. Fichas jugadas: {_apuestas.Count}. Total apostado: ${totalApostado:N0}.");
        }

        private void btnLimpiarMesa_Click(object sender, EventArgs e)
        {
            LimpiarApuestas();
            lblResultado.Text = "Mesa limpia. Selecciona una ficha y toca la mesa.";
            lblResultado.ForeColor = Color.WhiteSmoke;
            lblPremio.Text = "Hagan sus apuestas";
            lblPremio.ForeColor = Color.Gold;
        }

        private bool ValidarGiro(out decimal totalApostado)
        {
            if (_usuario == null)
            {
                totalApostado = 0;
                MessageBox.Show("Debe iniciar sesion para jugar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            totalApostado = CalcularTotalApostado();
            if (totalApostado <= 0)
            {
                MessageBox.Show("Selecciona una ficha y colocala sobre la mesa antes de girar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (totalApostado > _usuario.Saldo)
            {
                MessageBox.Show($"Saldo insuficiente. Tu saldo es ${_usuario.Saldo:N2}.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            btnGirarRuleta.Enabled = false;
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
                SaldoActualizado?.Invoke(this, EventArgs.Empty);

                lblResultado.Text = detalle;
                lblResultado.ForeColor = gano ? Color.FromArgb(34, 197, 94) : Color.FromArgb(248, 113, 113);
                lblPremio.Text = gano ? $"Ganaste ${ganancia:N2}" : $"Perdiste ${apuesta:N2}";
                LimpiarApuestas();
            }
            finally
            {
                btnGirarRuleta.Enabled = true;
            }
        }

        private void CrearBotonesFichas()
        {
            decimal[] valores = { 50m, 500m, 2500m, 5000m, 25000m, 100000m, 500000m, 5000000m };
            Color[] colores =
            {
                Color.FromArgb(120, 113, 108),
                Color.FromArgb(180, 83, 9),
                Color.FromArgb(107, 114, 128),
                Color.FromArgb(202, 138, 4),
                Color.FromArgb(185, 28, 28),
                Color.FromArgb(2, 132, 199),
                Color.FromArgb(31, 41, 55),
                Color.FromArgb(37, 99, 235)
            };

            for (int i = 0; i < valores.Length; i++)
            {
                Button ficha = new Button
                {
                    BackColor = colores[i],
                    Cursor = Cursors.Hand,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(24 + (i % 4) * 55, 122 + (i / 4) * 42),
                    Name = "btnFicha" + i,
                    Size = new Size(48, 34),
                    Tag = valores[i],
                    Text = FormatearFicha(valores[i]),
                    UseVisualStyleBackColor = false
                };

                ficha.FlatAppearance.BorderColor = Color.Gold;
                ficha.FlatAppearance.BorderSize = valores[i] == _fichaSeleccionada ? 2 : 0;
                ficha.Click += Ficha_Click;
                panelControles.Controls.Add(ficha);
                ficha.BringToFront();
            }
        }

        private void CrearZonasApuesta()
        {
            int x = 76;
            int y = 16;
            int w = 66;
            int h = 20;
            int numero = 1;

            for (int fila = 0; fila < 12; fila++)
            {
                for (int col = 0; col < 3; col++)
                {
                    int n = numero++;
                    Label zona = new Label
                    {
                        BackColor = Color.Transparent,
                        Cursor = Cursors.Hand,
                        Location = new Point(x + col * w, y + (11 - fila) * (h - 1)),
                        Size = new Size(w, h),
                        Tag = new ApuestaRuleta { Tipo = "Numero", Numero = n }
                    };

                    zona.Click += ZonaApuesta_Click;
                    panelTapete.Controls.Add(zona);
                    _zonasNumero.Add(zona);
                    zona.BringToFront();
                }
            }
        }

        private void PrepararApuestasExternas()
        {
            CrearZonaExterna("1-18", "1-18", new Rectangle(14, 304, 54, 30), Color.FromArgb(6, 78, 59));
            CrearZonaExterna("PAR", "Par", new Rectangle(70, 304, 54, 30), Color.FromArgb(6, 78, 59));
            CrearZonaExterna("IMPAR", "Impar", new Rectangle(238, 304, 54, 30), Color.FromArgb(6, 78, 59));
            CrearZonaExterna("19-36", "19-36", new Rectangle(294, 304, 54, 30), Color.FromArgb(6, 78, 59));

            lblCero.Cursor = Cursors.Hand;
            lblRojo.Cursor = Cursors.Hand;
            lblNegro.Cursor = Cursors.Hand;
            lblDocena1.Cursor = Cursors.Hand;
            lblDocena2.Cursor = Cursors.Hand;
            lblDocena3.Cursor = Cursors.Hand;

            lblCero.Tag = new ApuestaRuleta { Tipo = "Numero", Numero = 0 };
            lblRojo.Tag = new ApuestaRuleta { Tipo = "Rojo" };
            lblNegro.Tag = new ApuestaRuleta { Tipo = "Negro" };
            lblDocena1.Tag = new ApuestaRuleta { Tipo = "Docena1" };
            lblDocena2.Tag = new ApuestaRuleta { Tipo = "Docena2" };
            lblDocena3.Tag = new ApuestaRuleta { Tipo = "Docena3" };

            lblCero.Click += ZonaApuesta_Click;
            lblRojo.Click += ZonaApuesta_Click;
            lblNegro.Click += ZonaApuesta_Click;
            lblDocena1.Click += ZonaApuesta_Click;
            lblDocena2.Click += ZonaApuesta_Click;
            lblDocena3.Click += ZonaApuesta_Click;
        }

        private void CrearZonaExterna(string texto, string tipo, Rectangle ubicacion, Color color)
        {
            Label zona = new Label
            {
                BackColor = color,
                BorderStyle = BorderStyle.None,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = ubicacion.Location,
                Size = ubicacion.Size,
                Tag = new ApuestaRuleta { Tipo = tipo },
                Text = texto,
                TextAlign = ContentAlignment.MiddleCenter
            };

            zona.Click += ZonaApuesta_Click;
            zona.Paint += ZonaExterna_Paint;
            panelTapete.Controls.Add(zona);
            _zonasExternas.Add(zona);
            zona.BringToFront();
        }

        private void ZonaExterna_Paint(object sender, PaintEventArgs e)
        {
            Label zona = (Label)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, zona.Width - 1, zona.Height - 1);
            using (SolidBrush brush = new SolidBrush(zona.BackColor))
                e.Graphics.FillRectangle(brush, r);
            using (Pen pen = new Pen(Color.FromArgb(180, 241, 245, 249), 1))
                e.Graphics.DrawRectangle(pen, r);
            TextRenderer.DrawText(e.Graphics, zona.Text, zona.Font, zona.ClientRectangle, zona.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void AplicarLayoutRuleta()
        {
            if (panelJuego.ClientSize.Width <= 0 || panelJuego.ClientSize.Height <= 0) return;

            int margen = 28;
            int espacio = 24;
            int yContenido = 68;
            int altoMensajes = 110;
            int anchoControles = 300;
            int altoDisponible = Math.Max(430, panelJuego.ClientSize.Height - yContenido - altoMensajes);

            lblMesa.SetBounds(margen, 18, panelJuego.ClientSize.Width - margen * 2, 38);

            panelControles.SetBounds(
                panelJuego.ClientSize.Width - margen - anchoControles,
                yContenido,
                anchoControles,
                Math.Min(500, altoDisponible));

            int anchoZonaJuego = panelControles.Left - margen - espacio;
            int anchoRueda = Math.Min(430, Math.Max(310, (int)(anchoZonaJuego * 0.30)));
            panelRueda.SetBounds(margen, yContenido, anchoRueda, altoDisponible);

            int xTapete = panelRueda.Right + espacio;
            int anchoTapete = panelControles.Left - espacio - xTapete;
            if (anchoTapete < 520)
            {
                int ajuste = 520 - anchoTapete;
                anchoRueda = Math.Max(260, anchoRueda - ajuste);
                panelRueda.SetBounds(margen, yContenido, anchoRueda, altoDisponible);
                xTapete = panelRueda.Right + espacio;
                anchoTapete = panelControles.Left - espacio - xTapete;
            }

            anchoTapete = Math.Max(280, anchoTapete);
            panelTapete.SetBounds(xTapete, yContenido, anchoTapete, altoDisponible);

            lblResultado.SetBounds(margen, panelJuego.ClientSize.Height - 92, panelJuego.ClientSize.Width - margen * 2, 32);
            lblPremio.SetBounds(margen, panelJuego.ClientSize.Height - 58, panelJuego.ClientSize.Width - margen * 2, 46);

            ReubicarPanelControles();
            ReubicarZonasMesa();
            ReubicarNumeroGanador();
            panelRueda.Invalidate();
            panelTapete.Invalidate();
        }

        private void ReubicarPanelControles()
        {
            int margen = 24;
            int ancho = panelControles.ClientSize.Width - margen * 2;

            lblApuesta.Location = new Point(margen, 24);
            txtApuesta.SetBounds(margen, 50, ancho, 32);
            // Problema visual que resuelve: el selector de fichas queda como panel compacto y no como formulario viejo.
            lblTipoApuesta.Text = "Fichas disponibles";
            lblTipoApuesta.SetBounds(margen, 98, ancho, 24);

            int fichaW = (ancho - 18) / 4;
            int fichaH = 38;
            int i = 0;
            foreach (Control control in panelControles.Controls)
            {
                if (control is Button boton && boton.Name.StartsWith("btnFicha"))
                {
                    boton.SetBounds(margen + (i % 4) * (fichaW + 6), 132 + (i / 4) * (fichaH + 10), fichaW, fichaH);
                    i++;
                }
            }

            lblNumero.SetBounds(margen, 226, ancho, 42);
            numNumero.SetBounds(margen, 278, ancho, 32);
            cboTipoApuesta.SetBounds(margen, 278, ancho, 32);
            lblTotalApostado.SetBounds(margen, 292, ancho, 34);
            btnGirarRuleta.SetBounds(margen, panelControles.ClientSize.Height - 104, ancho, 52);
            btnLimpiarMesa.SetBounds(margen, panelControles.ClientSize.Height - 44, ancho, 30);
        }

        private void ReubicarZonasMesa()
        {
            if (panelTapete.ClientSize.Width <= 0 || panelTapete.ClientSize.Height <= 0) return;

            int pad = 22;
            int zeroW = Math.Max(78, panelTapete.ClientSize.Width / 11);
            int gridX = pad + zeroW + 12;
            int gridW = panelTapete.ClientSize.Width - gridX - pad;
            int top = 22;
            int bottomH = 42;
            int docenaH = 48;
            int gap = 8;
            int gridH = panelTapete.ClientSize.Height - top - pad - bottomH - gap - docenaH - gap;
            int cellW = gridW / 12;
            int cellH = gridH / 3;

            lblCero.SetBounds(pad, top, zeroW, gridH);

            foreach (Label zona in _zonasNumero)
            {
                ApuestaRuleta datos = (ApuestaRuleta)zona.Tag;
                int col = (datos.Numero - 1) / 3;
                int row = 2 - ((datos.Numero - 1) % 3);
                int anchoCelda = col == 11 ? gridW - cellW * 11 : cellW;
                zona.SetBounds(gridX + col * cellW, top + row * cellH, anchoCelda, cellH);
            }

            int docY = top + gridH + gap;
            lblDocena1.SetBounds(gridX, docY, cellW * 4, docenaH);
            lblDocena2.SetBounds(gridX + cellW * 4, docY, cellW * 4, docenaH);
            lblDocena3.SetBounds(gridX + cellW * 8, docY, gridW - cellW * 8, docenaH);

            int extY = docY + docenaH + gap;
            int extW = gridW / 6;
            foreach (Label zona in _zonasExternas)
            {
                if (zona.Text == "1-18") zona.SetBounds(gridX, extY, extW, bottomH);
                if (zona.Text == "PAR") zona.SetBounds(gridX + extW, extY, extW, bottomH);
                if (zona.Text == "IMPAR") zona.SetBounds(gridX + extW * 4, extY, extW, bottomH);
                if (zona.Text == "19-36") zona.SetBounds(gridX + extW * 5, extY, gridW - extW * 5, bottomH);
            }

            lblRojo.SetBounds(gridX + extW * 2, extY, extW, bottomH);
            lblNegro.SetBounds(gridX + extW * 3, extY, extW, bottomH);

            foreach (ApuestaRuleta apuesta in _apuestas)
                ReubicarFicha(apuesta);
        }

        private void ReubicarNumeroGanador()
        {
            int w = Math.Min(140, Math.Max(96, panelRueda.ClientSize.Width / 3));
            int h = Math.Min(70, Math.Max(56, panelRueda.ClientSize.Height / 7));
            lblNumeroGanador.SetBounds((panelRueda.ClientSize.Width - w) / 2,
                (panelRueda.ClientSize.Height - h) / 2, w, h);
        }

        private void Ficha_Click(object sender, EventArgs e)
        {
            Button ficha = (Button)sender;
            _fichaSeleccionada = (decimal)ficha.Tag;
            txtApuesta.Text = FormatearFicha(_fichaSeleccionada);

            foreach (Control control in panelControles.Controls)
                if (control is Button boton && boton.Name.StartsWith("btnFicha"))
                    boton.FlatAppearance.BorderSize = boton == ficha ? 2 : 0;
        }

        private void ZonaApuesta_Click(object sender, EventArgs e)
        {
            Control zona = (Control)sender;
            ApuestaRuleta datos = (ApuestaRuleta)zona.Tag;

            if (_usuario != null && CalcularTotalApostado() + _fichaSeleccionada > _usuario.Saldo)
            {
                MessageBox.Show("No tienes saldo suficiente para colocar esa ficha.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ApuestaRuleta existente = BuscarApuesta(datos.Tipo, datos.Numero);
            if (existente != null)
            {
                existente.Monto += _fichaSeleccionada;
                existente.Ficha.Text = FormatearFicha(existente.Monto);
                existente.Ficha.Invalidate();
                existente.Ficha.BringToFront();
                ActualizarTotalApostado();
                return;
            }

            string conflicto = ObtenerConflictoApuesta(datos.Tipo);
            if (!string.IsNullOrEmpty(conflicto))
            {
                MessageBox.Show($"Ya tienes una apuesta activa en {conflicto}. Limpia la mesa si quieres cambiar esa seleccion.",
                    "Apuesta no permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ApuestaRuleta apuesta = new ApuestaRuleta
            {
                Tipo = datos.Tipo,
                Numero = datos.Numero,
                Monto = _fichaSeleccionada
            };

            Label ficha = CrearFichaVisual(zona, apuesta);
            apuesta.Ficha = ficha;
            _apuestas.Add(apuesta);
            panelTapete.Controls.Add(ficha);
            ficha.Click += ZonaApuesta_Click;
            ficha.BringToFront();
            ActualizarTotalApostado();
        }

        private ApuestaRuleta BuscarApuesta(string tipo, int numero)
        {
            foreach (ApuestaRuleta apuesta in _apuestas)
                if (apuesta.Tipo == tipo && apuesta.Numero == numero)
                    return apuesta;

            return null;
        }

        private string ObtenerConflictoApuesta(string tipo)
        {
            string grupo = ObtenerGrupoApuesta(tipo);
            if (string.IsNullOrEmpty(grupo)) return null;

            foreach (ApuestaRuleta apuesta in _apuestas)
            {
                if (ObtenerGrupoApuesta(apuesta.Tipo) == grupo && apuesta.Tipo != tipo)
                    return NombreGrupoApuesta(grupo);
            }

            return null;
        }

        private string ObtenerGrupoApuesta(string tipo)
        {
            if (tipo == "Rojo" || tipo == "Negro") return "Color";
            if (tipo == "Par" || tipo == "Impar") return "Paridad";
            if (tipo == "1-18" || tipo == "19-36") return "Mitad";
            if (tipo == "Docena1" || tipo == "Docena2" || tipo == "Docena3") return "Docena";
            return null;
        }

        private string NombreGrupoApuesta(string grupo)
        {
            if (grupo == "Color") return "color";
            if (grupo == "Paridad") return "par/impar";
            if (grupo == "Mitad") return "rango";
            if (grupo == "Docena") return "docena";
            return "la mesa";
        }

        private Label CrearFichaVisual(Control zona, ApuestaRuleta apuesta)
        {
            int tamano = ObtenerTamanoFicha();
            Point centro = new Point(zona.Left + zona.Width / 2 - tamano / 2, zona.Top + zona.Height / 2 - tamano / 2);

            Label ficha = new Label
            {
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = centro,
                Size = new Size(tamano, tamano),
                Tag = new ApuestaRuleta { Tipo = apuesta.Tipo, Numero = apuesta.Numero },
                Text = FormatearFicha(apuesta.Monto),
                TextAlign = ContentAlignment.MiddleCenter
            };

            ficha.Paint += FichaVisual_Paint;
            return ficha;
        }

        private int ObtenerTamanoFicha()
        {
            return Math.Max(34, Math.Min(52, panelTapete.ClientSize.Width / 16));
        }

        private void ReubicarFicha(ApuestaRuleta apuesta)
        {
            if (apuesta.Ficha == null) return;

            Control zona = BuscarZonaControl(apuesta.Tipo, apuesta.Numero);
            if (zona == null) return;

            int tamano = ObtenerTamanoFicha();
            apuesta.Ficha.Size = new Size(tamano, tamano);
            apuesta.Ficha.Location = new Point(
                zona.Left + zona.Width / 2 - tamano / 2,
                zona.Top + zona.Height / 2 - tamano / 2);
            apuesta.Ficha.BringToFront();
        }

        private Control BuscarZonaControl(string tipo, int numero)
        {
            if (tipo == "Numero")
            {
                if (numero == 0) return lblCero;

                foreach (Label zona in _zonasNumero)
                {
                    ApuestaRuleta datos = (ApuestaRuleta)zona.Tag;
                    if (datos.Numero == numero) return zona;
                }
            }

            if (tipo == "Rojo") return lblRojo;
            if (tipo == "Negro") return lblNegro;
            if (tipo == "Docena1") return lblDocena1;
            if (tipo == "Docena2") return lblDocena2;
            if (tipo == "Docena3") return lblDocena3;

            foreach (Label zona in _zonasExternas)
            {
                ApuestaRuleta datos = (ApuestaRuleta)zona.Tag;
                if (datos.Tipo == tipo) return zona;
            }

            return null;
        }

        private void FichaVisual_Paint(object sender, PaintEventArgs e)
        {
            Label ficha = (Label)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle circulo = new Rectangle(2, 2, ficha.Width - 5, ficha.Height - 5);
            using (SolidBrush relleno = new SolidBrush(Color.Gold))
                e.Graphics.FillEllipse(relleno, circulo);
            using (Pen borde = new Pen(Color.FromArgb(120, 53, 15), 3))
                e.Graphics.DrawEllipse(borde, circulo);
            using (Pen brillo = new Pen(Color.White, 1))
                e.Graphics.DrawEllipse(brillo, new Rectangle(6, 6, ficha.Width - 13, ficha.Height - 13));

            TextRenderer.DrawText(e.Graphics, ficha.Text, ficha.Font, ficha.ClientRectangle,
                Color.FromArgb(15, 23, 42),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void ActualizarTotalApostado()
        {
            lblTotalApostado.Text = $"Total apostado: ${CalcularTotalApostado():N0}";
        }

        private decimal CalcularTotalApostado()
        {
            decimal total = 0m;
            foreach (ApuestaRuleta apuesta in _apuestas)
                total += apuesta.Monto;

            return total;
        }

        private void LimpiarApuestas()
        {
            foreach (ApuestaRuleta apuesta in _apuestas)
                if (apuesta.Ficha != null)
                    panelTapete.Controls.Remove(apuesta.Ficha);

            _apuestas.Clear();
            ActualizarTotalApostado();
        }

        private bool EsApuestaGanadora(ApuestaRuleta apuesta, int numero, string color)
        {
            switch (apuesta.Tipo)
            {
                case "Rojo":
                    return color == "Rojo";
                case "Negro":
                    return color == "Negro";
                case "Docena1":
                    return numero >= 1 && numero <= 12;
                case "Docena2":
                    return numero >= 13 && numero <= 24;
                case "Docena3":
                    return numero >= 25 && numero <= 36;
                case "1-18":
                    return numero >= 1 && numero <= 18;
                case "19-36":
                    return numero >= 19 && numero <= 36;
                case "Par":
                    return numero != 0 && numero % 2 == 0;
                case "Impar":
                    return numero % 2 == 1;
                case "Numero":
                    return numero == apuesta.Numero;
                default:
                    return false;
            }
        }

        private int ObtenerMultiplicador(ApuestaRuleta apuesta)
        {
            if (apuesta.Tipo == "Numero") return apuesta.Numero == 0 ? 36 : 36;
            if (apuesta.Tipo.StartsWith("Docena")) return 3;
            return 2;
        }

        private string FormatearFicha(decimal valor)
        {
            if (valor >= 1000000m)
            {
                decimal millones = valor / 1000000m;
                return millones % 1 == 0 ? $"{millones:N0}M" : $"{millones:0.#}M";
            }

            if (valor >= 1000m)
            {
                decimal miles = valor / 1000m;
                return miles % 1 == 0 ? $"{miles:N0}K" : $"{miles:0.#}K";
            }

            return valor.ToString("N0");
        }

        private class ApuestaRuleta
        {
            public string Tipo { get; set; }
            public int Numero { get; set; }
            public decimal Monto { get; set; }
            public Label Ficha { get; set; }
        }

        private string ObtenerColor(int numero)
        {
            if (numero == 0) return "Verde";

            int[] rojos = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
            foreach (int rojo in rojos)
                if (numero == rojo) return "Rojo";

            return "Negro";
        }

        private void MostrarNumeroEnRueda(int numero, string color)
        {
            _numeroGanadorActual = numero;
            lblNumeroGanador.Text = numero.ToString();

            if (color == "Rojo")
                lblNumeroGanador.BackColor = Color.FromArgb(185, 28, 28);
            else if (color == "Negro")
                lblNumeroGanador.BackColor = Color.FromArgb(12, 12, 14);
            else
                lblNumeroGanador.BackColor = Color.FromArgb(22, 163, 74);

            panelRueda.Invalidate();
        }

        private void panelJuego_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = panelJuego.ClientRectangle;
            using (LinearGradientBrush fondo = new LinearGradientBrush(area,
                Color.FromArgb(6, 10, 18), Color.FromArgb(14, 22, 36), 90F))
            {
                e.Graphics.FillRectangle(fondo, area);
            }

            using (SolidBrush brillo = new SolidBrush(Color.FromArgb(28, AppTheme.Dorado)))
                e.Graphics.FillRectangle(brillo, 0, 0, area.Width, 2);
        }

        private void panelControles_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = new Rectangle(0, 0, panelControles.Width - 1, panelControles.Height - 1);
            using (LinearGradientBrush fondo = new LinearGradientBrush(area,
                Color.FromArgb(22, 27, 39), Color.FromArgb(11, 17, 30), 90F))
            {
                e.Graphics.FillRectangle(fondo, area);
            }
            using (Pen borde = new Pen(Color.FromArgb(130, AppTheme.Dorado), 1))
                e.Graphics.DrawRectangle(borde, area);
            using (SolidBrush luz = new SolidBrush(Color.FromArgb(20, AppTheme.Dorado)))
                e.Graphics.FillRectangle(luz, 1, 1, area.Width - 2, 44);
        }

        private void panelRueda_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle panel = panelRueda.ClientRectangle;
            using (LinearGradientBrush mesa = new LinearGradientBrush(panel,
                Color.FromArgb(42, 20, 10), Color.FromArgb(12, 8, 6), 90F))
            {
                e.Graphics.FillRectangle(mesa, panel);
            }

            using (Pen marco = new Pen(Color.FromArgb(110, AppTheme.Dorado), 1))
                e.Graphics.DrawRectangle(marco, 0, 0, panelRueda.Width - 1, panelRueda.Height - 1);

            int lado = Math.Min(panelRueda.ClientSize.Width, panelRueda.ClientSize.Height) - 42;
            lado = Math.Max(220, lado);
            Rectangle rueda = new Rectangle((panelRueda.ClientSize.Width - lado) / 2, 20, lado, lado);
            using (LinearGradientBrush madera = new LinearGradientBrush(rueda,
                Color.FromArgb(124, 62, 22), Color.FromArgb(45, 25, 13), 45F))
            {
                e.Graphics.FillEllipse(madera, rueda);
            }

            using (Pen bordeDorado = new Pen(AppTheme.Dorado, Math.Max(7, lado / 42)))
                e.Graphics.DrawEllipse(bordeDorado, rueda);
            using (Pen bordeOscuro = new Pen(Color.FromArgb(75, 36, 14), Math.Max(5, lado / 70)))
                e.Graphics.DrawEllipse(bordeOscuro, new Rectangle(rueda.X + 12, rueda.Y + 12, rueda.Width - 24, rueda.Height - 24));

            int margenExterior = lado / 9;
            Rectangle exterior = new Rectangle(rueda.X + margenExterior, rueda.Y + margenExterior,
                rueda.Width - margenExterior * 2, rueda.Height - margenExterior * 2);
            int[] secuenciaEuropea = { 0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 36, 11, 30, 8, 23, 10, 5, 24, 16, 33, 1, 20, 14, 31, 9, 22, 18, 29, 7, 28, 12, 35, 3, 26 };
            float angulo = -90f;
            float paso = 360f / secuenciaEuropea.Length;
            using (Pen divisor = new Pen(Color.FromArgb(210, 230, 230, 230), 1))
            {
                for (int i = 0; i < secuenciaEuropea.Length; i++)
                {
                    int numero = secuenciaEuropea[i];
                    Color color = numero == 0
                        ? Color.FromArgb(22, 163, 74)
                        : ObtenerColor(numero) == "Rojo" ? Color.FromArgb(185, 28, 28) : Color.FromArgb(12, 12, 14);
                    using (SolidBrush brush = new SolidBrush(color))
                        e.Graphics.FillPie(brush, exterior, angulo, paso);
                    e.Graphics.DrawPie(divisor, exterior, angulo, paso);

                    double rad = (angulo + paso / 2) * Math.PI / 180.0;
                    Point centroTexto = new Point(
                        exterior.X + exterior.Width / 2 + (int)((exterior.Width * 0.39) * Math.Cos(rad)),
                        exterior.Y + exterior.Height / 2 + (int)((exterior.Height * 0.39) * Math.Sin(rad)));
                    using (Font f = new Font("Segoe UI", Math.Max(6F, lado / 42F), FontStyle.Bold))
                    {
                        TextRenderer.DrawText(e.Graphics, numero.ToString(), f,
                            new Rectangle(centroTexto.X - 14, centroTexto.Y - 9, 28, 18),
                            Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    }
                    angulo += paso;
                }
            }

            using (SolidBrush sombra = new SolidBrush(Color.FromArgb(28, Color.Black)))
                e.Graphics.FillEllipse(sombra, new Rectangle(rueda.X + lado / 5, rueda.Y + lado / 5,
                    lado - (lado / 5) * 2, lado - (lado / 5) * 2));

            int centroLado = lado / 3;
            Rectangle centro = new Rectangle(rueda.X + (rueda.Width - centroLado) / 2,
                rueda.Y + (rueda.Height - centroLado) / 2, centroLado, centroLado);
            using (LinearGradientBrush centroBrush = new LinearGradientBrush(centro,
                Color.FromArgb(255, 218, 75), Color.FromArgb(160, 92, 18), 45F))
            {
                e.Graphics.FillEllipse(centroBrush, centro);
            }
            using (Pen centroPen = new Pen(Color.WhiteSmoke, Math.Max(3, lado / 80)))
                e.Graphics.DrawEllipse(centroPen, centro);

            double bolaAngulo = (_numeroGanadorActual >= 0 ? Array.IndexOf(secuenciaEuropea, _numeroGanadorActual) * paso : 4 * paso) - 90 + paso / 2;
            double bolaRad = bolaAngulo * Math.PI / 180.0;
            Point centroRueda = new Point(rueda.X + rueda.Width / 2, rueda.Y + rueda.Height / 2);
            // Problema visual que resuelve: la bola queda en el carril exterior y no tapa el numero ganador.
            int radioBola = exterior.Width / 2 + lado / 22;
            int bolaTam = Math.Max(10, lado / 30);
            Rectangle bolaRect = new Rectangle(
                centroRueda.X + (int)(radioBola * Math.Cos(bolaRad)) - bolaTam / 2,
                centroRueda.Y + (int)(radioBola * Math.Sin(bolaRad)) - bolaTam / 2,
                bolaTam,
                bolaTam);
            using (SolidBrush sombraBola = new SolidBrush(Color.FromArgb(90, Color.Black)))
                e.Graphics.FillEllipse(sombraBola, bolaRect.X + 3, bolaRect.Y + 4, bolaRect.Width, bolaRect.Height);
            using (SolidBrush bola = new SolidBrush(Color.White))
                e.Graphics.FillEllipse(bola, bolaRect);
        }

        private void panelTapete_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = panelTapete.ClientRectangle;
            using (LinearGradientBrush fieltro = new LinearGradientBrush(area,
                Color.FromArgb(7, 112, 64), Color.FromArgb(2, 63, 41), 90F))
            {
                e.Graphics.FillRectangle(fieltro, area);
            }

            using (SolidBrush brillo = new SolidBrush(Color.FromArgb(20, Color.White)))
                e.Graphics.FillEllipse(brillo, new Rectangle(-panelTapete.Width / 4, -panelTapete.Height / 3,
                    panelTapete.Width, panelTapete.Height));

            using (Pen lineaDecorativa = new Pen(Color.FromArgb(130, AppTheme.Dorado), 2))
                e.Graphics.DrawRectangle(lineaDecorativa, new Rectangle(10, 10,
                    panelTapete.ClientSize.Width - 21, panelTapete.ClientSize.Height - 21));
            using (Pen lineaInterna = new Pen(Color.FromArgb(80, 241, 245, 249), 1))
                e.Graphics.DrawRectangle(lineaInterna, new Rectangle(18, 18,
                    panelTapete.ClientSize.Width - 37, panelTapete.ClientSize.Height - 37));

            using (Font font = new Font("Segoe UI", Math.Max(10F, panelTapete.ClientSize.Height / 35F), FontStyle.Bold))
            using (Pen borde = new Pen(Color.WhiteSmoke, 1))
            {
                foreach (Label zona in _zonasNumero)
                {
                    ApuestaRuleta datos = (ApuestaRuleta)zona.Tag;
                    Rectangle celda = zona.Bounds;
                    Color fondo = ObtenerColor(datos.Numero) == "Rojo" ? Color.FromArgb(185, 28, 28) : Color.FromArgb(12, 12, 14);
                    using (SolidBrush brush = new SolidBrush(fondo))
                        e.Graphics.FillRectangle(brush, celda);
                    e.Graphics.DrawRectangle(borde, celda);
                    TextRenderer.DrawText(e.Graphics, datos.Numero.ToString(), font, celda, Color.White,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }
        }

        private void ActualizarSaldo()
        {
            lblSaldo.Text = $"Saldo: ${_usuario.Saldo:N2}";
        }
    }
}

