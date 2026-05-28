using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class UcHistorial : UserControl
    {
        private readonly Usuario _usuario;
        private readonly TransaccionServicio _transSvc = new TransaccionServicio();

        public UcHistorial(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
            // Problema visual que resuelve: transacciones comparte fondo, tipografia y botones del resto del lobby.
            AppTheme.ApplyView(this);
            AppTheme.ApplyPrimaryButton(btnTodos, AppTheme.BgHover);
            AppTheme.ApplyPrimaryButton(btnApuestas, AppTheme.BgHover);
            AppTheme.ApplyPrimaryButton(btnDepositos, AppTheme.BgHover);
            CargarVista("Todos", 30);
        }

        public UcHistorial()
        {
            InitializeComponent();
            // Problema visual que resuelve: mantiene consistente la vista en el Designer y en ejecucion.
            AppTheme.ApplyView(this);
        }

        private void CargarVista(string categoria, int dias)
        {
            pnlContenido.Controls.Clear();

            Panel marco = new Panel
            {
                BackColor = CasinoTheme.Surface,
                Dock = DockStyle.Fill,
                Padding = new Padding(16)
            };
            marco.Paint += (s, e) => CasinoTheme.DrawBorderedPanel(e.Graphics,
                new Rectangle(0, 0, marco.Width, marco.Height), CasinoTheme.Surface, CasinoTheme.Border);

            Panel filtros = new Panel
            {
                Dock = DockStyle.Top,
                Height = 154,
                BackColor = AppTheme.BgCard,
                Padding = new Padding(18, 16, 18, 16)
            };
            filtros.Paint += (s, e) =>
            {
                // Problema visual que resuelve: la zona de filtros deja de verse plana y separa claramente las acciones de la lista.
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Pen borde = new Pen(AppTheme.BordeClaro, 1))
                    e.Graphics.DrawRectangle(borde, 0, 0, filtros.Width - 1, filtros.Height - 1);
                using (Pen brillo = new Pen(Color.FromArgb(80, AppTheme.Dorado), 1))
                    e.Graphics.DrawLine(brillo, 18, 0, Math.Max(18, filtros.Width - 18), 0);
            };

            Button btn7 = ControlFactory.CrearChipPeriodo("7 dias", dias == 7);
            Button btn14 = ControlFactory.CrearChipPeriodo("14 dias", dias == 14);
            Button btn30 = ControlFactory.CrearChipPeriodo("30 dias", dias == 30);
            ComboBox cboFiltro = ControlFactory.CrearComboFiltro();
            TextBox desde = ControlFactory.CrearInputFecha();
            TextBox hasta = ControlFactory.CrearInputFecha();
            Button buscar = new Button { Text = "Buscar" };
            Label lblPeriodo = CrearEtiquetaFiltro("Rango rapido");
            Label lblFiltro = CrearEtiquetaFiltro("Filtro");
            Label lblDesde = CrearEtiquetaFiltro("Desde");
            Label lblHasta = CrearEtiquetaFiltro("Hasta");
            Label lblAccion = CrearEtiquetaFiltro("Accion");

            AppTheme.ApplyPrimaryButton(buscar, AppTheme.BgHover);
            CargarOpcionesFiltro(cboFiltro, categoria);
            hasta.Text = DateTime.Today.ToString("dd/MM/yyyy");
            desde.Text = DateTime.Today.AddDays(-dias).ToString("dd/MM/yyyy");

            FlowLayoutPanel chips = new FlowLayoutPanel
            {
                BackColor = Color.Transparent,
                Dock = DockStyle.Fill,
                WrapContents = false,
                Margin = Padding.Empty,
                Padding = new Padding(0, 2, 0, 0)
            };
            chips.Controls.Add(btn7);
            chips.Controls.Add(btn14);
            chips.Controls.Add(btn30);

            TableLayoutPanel barraFiltros = new TableLayoutPanel
            {
                // Problema visual que resuelve: evita coordenadas manuales que cortaban labels y sacaban el boton Buscar del contenedor.
                BackColor = Color.Transparent,
                ColumnCount = 4,
                Dock = DockStyle.Fill,
                RowCount = 2
            };
            barraFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            barraFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 21F));
            barraFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 21F));
            barraFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
            barraFiltros.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            barraFiltros.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel grupoPeriodo = CrearGrupoFiltro(lblPeriodo, chips);
            Panel grupoFiltro = CrearGrupoFiltro(lblFiltro, cboFiltro);
            Panel grupoDesde = CrearGrupoFiltro(lblDesde, desde);
            Panel grupoHasta = CrearGrupoFiltro(lblHasta, hasta);
            Panel grupoBuscar = CrearGrupoFiltro(lblAccion, buscar);

            buscar.TextAlign = ContentAlignment.MiddleCenter;
            buscar.SizeChanged += (s, e) => AppTheme.ApplyRoundedRegion(buscar, 8);

            barraFiltros.Controls.Add(grupoPeriodo, 0, 0);
            barraFiltros.SetColumnSpan(grupoPeriodo, 4);
            barraFiltros.Controls.Add(grupoFiltro, 0, 1);
            barraFiltros.Controls.Add(grupoDesde, 1, 1);
            barraFiltros.Controls.Add(grupoHasta, 2, 1);
            barraFiltros.Controls.Add(grupoBuscar, 3, 1);
            filtros.Controls.Add(barraFiltros);

            Panel lista = new Panel
            {
                AutoScroll = true,
                BackColor = CasinoTheme.Surface,
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 12, 0, 0)
            };

            Action cargarLista = () =>
            {
                if (!DateTime.TryParse(desde.Text, out DateTime fechaDesde) ||
                    !DateTime.TryParse(hasta.Text, out DateTime fechaHasta))
                {
                    MessageBox.Show("Ingrese fechas validas con formato dia/mes/año.", "Fechas invalidas",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                PintarListaMovimientos(lista, categoria, cboFiltro.Text, fechaDesde.Date, fechaHasta.Date.AddDays(1).AddTicks(-1));
            };

            buscar.Click += (s, e) => cargarLista();
            btn7.Click += (s, e) => CargarVista(categoria, 7);
            btn14.Click += (s, e) => CargarVista(categoria, 14);
            btn30.Click += (s, e) => CargarVista(categoria, 30);

            marco.Controls.Add(lista);
            marco.Controls.Add(filtros);
            pnlContenido.Controls.Add(marco);
            cargarLista();
        }

        private Label CrearEtiquetaFiltro(string texto)
        {
            return ControlFactory.CrearLabel(
                texto,
                AppTheme.TextoSecundario,
                new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ContentAlignment.MiddleLeft);
        }

        private Panel CrearGrupoFiltro(Label etiqueta, Control control)
        {
            Panel grupo = new Panel
            {
                // Problema visual que resuelve: cada filtro queda en una columna propia, con texto completo y sin montarse sobre otros controles.
                BackColor = Color.Transparent,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 12, 0)
            };

            etiqueta.Dock = DockStyle.Top;
            etiqueta.Height = 22;
            control.Dock = DockStyle.Top;
            control.Height = control is Button ? 38 : 34;
            control.Margin = Padding.Empty;

            if (control is TextBox caja)
            {
                caja.BackColor = AppTheme.BgInput;
                caja.ForeColor = AppTheme.TextoPrimario;
                caja.BorderStyle = BorderStyle.FixedSingle;
                caja.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            }
            else if (control is ComboBox combo)
            {
                combo.BackColor = AppTheme.BgInput;
                combo.ForeColor = AppTheme.TextoPrimario;
                combo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            }

            grupo.Controls.Add(control);
            grupo.Controls.Add(etiqueta);
            return grupo;
        }

        private void CargarOpcionesFiltro(ComboBox combo, string categoria)
        {
            combo.Items.Clear();

            if (categoria == "Apuestas")
                combo.Items.AddRange(new object[] { "Todas", "Montos apostados", "Premios ganados" });
            else if (categoria == "Depositos")
                combo.Items.AddRange(new object[] { "Todos", "Depositos", "Retiros" });
            else
                combo.Items.AddRange(new object[] { "Todos", "Depositos", "Retiros", "Apuestas", "Ganancias" });

            combo.SelectedIndex = 0;
        }

        private void PintarListaMovimientos(Panel lista, string categoria, string filtro, DateTime desde, DateTime hasta)
        {
            lista.Controls.Clear();

            IList<MovimientoResumen> movimientos = _transSvc.ObtenerMovimientosResumen(
                _usuario.IdUsuario, categoria, filtro, desde, hasta);

            if (movimientos.Count == 0)
            {
                Label vacio = ControlFactory.CrearLabel(
                    "No hay movimientos para los filtros seleccionados.",
                    CasinoTheme.Muted, CasinoTheme.UiFont(11F, FontStyle.Bold), ContentAlignment.MiddleCenter);
                vacio.Dock = DockStyle.Top;
                vacio.Height = 70;
                lista.Controls.Add(vacio);
                return;
            }

            int y = 8;
            foreach (MovimientoResumen mov in movimientos)
            {
                Panel fila = CrearFilaMovimiento(mov);
                fila.SetBounds(0, y, Math.Max(560, lista.ClientSize.Width - 26), 62);
                fila.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                lista.Controls.Add(fila);
                y += 72;
            }
        }

        private Panel CrearFilaMovimiento(MovimientoResumen movimiento)
        {
            bool entrada = movimiento.Monto >= 0;
            Panel fila = new Panel
            {
                BackColor = Color.FromArgb(28, 43, 58),
                Height = 62
            };
            fila.Paint += (s, e) =>
            {
                using (SolidBrush brush = new SolidBrush(entrada ? Color.FromArgb(18, 78, 55) : Color.FromArgb(58, 42, 48)))
                    e.Graphics.FillRectangle(brush, 0, 0, 5, fila.Height);
                using (Pen pen = new Pen(Color.FromArgb(41, 59, 77)))
                    e.Graphics.DrawRectangle(pen, 0, 0, fila.Width - 1, fila.Height - 1);
            };

            Label icono = ControlFactory.CrearLabel(entrada ? "+" : "-", entrada ? CasinoTheme.Green : Color.FromArgb(203, 213, 225), CasinoTheme.UiFont(18F, FontStyle.Bold), ContentAlignment.MiddleCenter);
            Label titulo = ControlFactory.CrearLabel(movimiento.Titulo, CasinoTheme.Text, CasinoTheme.UiFont(9.5F, FontStyle.Bold), ContentAlignment.BottomLeft);
            Label fecha = ControlFactory.CrearLabel(movimiento.Fecha.ToString("hh:mm:ss tt dd/MM/yyyy"), CasinoTheme.Muted, CasinoTheme.UiFont(8.5F), ContentAlignment.TopLeft);
            Label monto = ControlFactory.CrearLabel($"{(entrada ? "+" : "-")}{Math.Abs(movimiento.Monto):N2}$", entrada ? CasinoTheme.Green : CasinoTheme.Text, CasinoTheme.UiFont(11F, FontStyle.Bold), ContentAlignment.MiddleRight);
            titulo.AutoEllipsis = true;
            fecha.AutoEllipsis = true;

            fila.Controls.Add(icono);
            fila.Controls.Add(titulo);
            fila.Controls.Add(fecha);
            fila.Controls.Add(monto);
            fila.Resize += (s, e) =>
            {
                icono.SetBounds(18, 0, 34, fila.Height);
                int anchoMonto = Math.Min(180, Math.Max(120, fila.Width / 4));
                int anchoTexto = Math.Max(150, fila.Width - anchoMonto - 98);
                titulo.SetBounds(64, 10, anchoTexto, 23);
                fecha.SetBounds(64, 34, anchoTexto, 18);
                monto.SetBounds(fila.Width - anchoMonto - 20, 0, anchoMonto, fila.Height);
            };
            return fila;
        }

        private void btnTodos_Click(object sender, EventArgs e)
        {
            MarcarBotonActivo(btnTodos);
            CargarVista("Todos", 30);
        }

        private void btnApuestas_Click(object sender, EventArgs e)
        {
            MarcarBotonActivo(btnApuestas);
            CargarVista("Apuestas", 30);
        }

        private void btnDepositos_Click(object sender, EventArgs e)
        {
            MarcarBotonActivo(btnDepositos);
            CargarVista("Depositos", 30);
        }

        private void MarcarBotonActivo(Button activo)
        {
            foreach (Button btn in new[] { btnTodos, btnApuestas, btnDepositos })
            {
                btn.BackColor = btn == activo ? CasinoTheme.SurfaceAlt : CasinoTheme.Surface;
            }
        }
    }
}
