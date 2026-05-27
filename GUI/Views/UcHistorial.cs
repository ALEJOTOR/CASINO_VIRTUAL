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
                Height = 128,
                BackColor = Color.FromArgb(13, 26, 42),
                Padding = new Padding(14)
            };

            Button btn7 = ControlFactory.CrearChipPeriodo("7 dias", dias == 7);
            Button btn14 = ControlFactory.CrearChipPeriodo("14 dias", dias == 14);
            Button btn30 = ControlFactory.CrearChipPeriodo("30 dias", dias == 30);
            ComboBox cboFiltro = ControlFactory.CrearComboFiltro();
            TextBox desde = ControlFactory.CrearInputFecha();
            TextBox hasta = ControlFactory.CrearInputFecha();
            Button buscar = new Button { Text = "Buscar" };
            Label lblPeriodo = ControlFactory.CrearLabel("Periodo", CasinoTheme.Muted, CasinoTheme.UiFont(8.5F, FontStyle.Bold), ContentAlignment.MiddleLeft);
            Label lblFiltro = ControlFactory.CrearLabel("Filtro", CasinoTheme.Muted, CasinoTheme.UiFont(8.5F, FontStyle.Bold), ContentAlignment.MiddleLeft);
            Label lblDesde = ControlFactory.CrearLabel("Desde", CasinoTheme.Muted, CasinoTheme.UiFont(8.5F, FontStyle.Bold), ContentAlignment.MiddleLeft);
            Label lblHasta = ControlFactory.CrearLabel("Hasta", CasinoTheme.Muted, CasinoTheme.UiFont(8.5F, FontStyle.Bold), ContentAlignment.MiddleLeft);

            AppTheme.ApplyPrimaryButton(buscar, AppTheme.BgHover);
            CargarOpcionesFiltro(cboFiltro, categoria);
            hasta.Text = DateTime.Today.ToString("dd/MM/yyyy");
            desde.Text = DateTime.Today.AddDays(-dias).ToString("dd/MM/yyyy");

            FlowLayoutPanel chips = new FlowLayoutPanel
            {
                BackColor = Color.Transparent,
                Location = new Point(0, 18),
                Size = new Size(360, 50),
                WrapContents = false,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            chips.Controls.Add(btn7);
            chips.Controls.Add(btn14);
            chips.Controls.Add(btn30);

            filtros.Controls.Add(lblPeriodo);
            filtros.Controls.Add(lblFiltro);
            filtros.Controls.Add(lblDesde);
            filtros.Controls.Add(lblHasta);
            filtros.Controls.Add(chips);
            filtros.Controls.Add(cboFiltro);
            filtros.Controls.Add(desde);
            filtros.Controls.Add(hasta);
            filtros.Controls.Add(buscar);
            filtros.Resize += (s, e) =>
            {
                UbicarFiltrosTransacciones(filtros, lblPeriodo, chips, lblFiltro, cboFiltro, lblDesde, desde, lblHasta, hasta, buscar);
            };
            UbicarFiltrosTransacciones(filtros, lblPeriodo, chips, lblFiltro, cboFiltro, lblDesde, desde, lblHasta, hasta, buscar);

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

        private void UbicarFiltrosTransacciones(Panel filtros, Label lblPeriodo, FlowLayoutPanel chips, Label lblFiltro, ComboBox filtro, Label lblDesde, TextBox desde, Label lblHasta, TextBox hasta, Button buscar)
        {
            int ancho = filtros.ClientSize.Width;
            bool compacto = ancho < 920;

            if (compacto)
            {
                filtros.Height = 196;
                int disponible = Math.Max(280, ancho - 28);
                int anchoFecha = Math.Max(130, (disponible - 134) / 2);

                lblPeriodo.SetBounds(14, 10, 220, 18);
                chips.SetBounds(14, 32, Math.Min(330, disponible), 42);

                lblFiltro.SetBounds(14, 82, 220, 18);
                filtro.SetBounds(14, 104, disponible, 34);

                lblDesde.SetBounds(14, 148, anchoFecha, 18);
                desde.SetBounds(14, 168, anchoFecha, 30);
                lblHasta.SetBounds(14 + anchoFecha + 10, 148, anchoFecha, 18);
                hasta.SetBounds(14 + anchoFecha + 10, 168, anchoFecha, 30);
                buscar.SetBounds(ancho - 130, 160, 116, 38);
                return;
            }

            filtros.Height = 126;
            lblPeriodo.SetBounds(14, 18, 220, 18);
            chips.SetBounds(14, 44, 330, 42);

            int derecha = ancho - 14;
            lblFiltro.SetBounds(derecha - 610, 18, 220, 18);
            filtro.SetBounds(derecha - 610, 44, 180, 34);
            lblDesde.SetBounds(derecha - 420, 18, 140, 18);
            desde.SetBounds(derecha - 420, 44, 140, 30);
            lblHasta.SetBounds(derecha - 268, 18, 140, 18);
            hasta.SetBounds(derecha - 268, 44, 140, 30);
            buscar.SetBounds(derecha - 116, 38, 116, 40);
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
