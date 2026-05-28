using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GUI
{
    public partial class UcDashboardAdmin : UserControl
    {
        private readonly AdminServicio _adminSvc = new AdminServicio();

        public UcDashboardAdmin()
        {
            InitializeComponent();
            AppTheme.ApplyView(this);
            AppTheme.ApplyTitle(lblTitulo);
            foreach (Panel card in new[] { cardTotalUsuarios, cardPartidasHoy, cardIngresosHoy, cardGananciaCasaHoy, pnlTopJugadores })
                AppTheme.ApplyCard(card, 10);
            AppTheme.ApplyDataGrid(dgvTopJugadores);
            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                ResumenAdmin resumen = _adminSvc.ObtenerResumenGeneral();
                lblTotalUsuariosValor.Text = resumen.TotalUsuarios.ToString("N0");
                lblPartidasHoyValor.Text = resumen.PartidasHoy.ToString("N0");
                lblIngresosHoyValor.Text = $"${resumen.IngresosHoy:N2}";
                lblGananciaCasaHoyValor.Text = $"${resumen.GananciaCasaHoy:N2}";
                CargarGraficoIngresos();
                CargarGraficoPartidas();
                CargarTopJugadores();
            }
            catch (Exception ex)
            {
                Label[] targets = { lblTotalUsuariosValor, lblPartidasHoyValor, lblIngresosHoyValor, lblGananciaCasaHoyValor };
                foreach (var t in targets) t.Text = "--";
                MessageBox.Show($"{ex.GetType().Name}: {ex.Message}\n\n{ex.StackTrace}",
                    "Dashboard - Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CargarGraficoIngresos()
        {
            chartIngresos.Series.Clear();
            chartIngresos.Titles.Clear();
            chartIngresos.ChartAreas.Clear();

            ChartArea area = new ChartArea();
            AppTheme.EstiloAreaChart(area);
            area.AxisY.LabelStyle.Format = "C0";
            chartIngresos.ChartAreas.Add(area);

            chartIngresos.Titles.Add(new Title("Ingresos y Ganancia — Ultimos 30 Dias",
                Docking.Top, new Font("Segoe UI", 11F, FontStyle.Bold), Color.FromArgb(148, 163, 184)));

            Dictionary<DateTime, decimal> ingresos = _adminSvc.ObtenerIngresosPorDia(30);
            Dictionary<DateTime, decimal> ganancia = _adminSvc.ObtenerGananciaCasaPorDia(30);

            Series s1 = new Series { Name = "Ingresos", ChartType = SeriesChartType.SplineArea };
            AppTheme.EstiloChartSerie(s1, AppTheme.Azul, Color.FromArgb(59, 130, 246));
            s1.ToolTip = "#VALX: $#VALY";
            foreach (var kvp in ingresos)
                s1.Points.AddXY(kvp.Key.ToString("dd/MM"), (double)kvp.Value);

            Series s2 = new Series { Name = "Ganancia Casa", ChartType = SeriesChartType.Spline };
            s2.Color = Color.FromArgb(251, 191, 36);
            s2.BorderWidth = 2;
            s2.ToolTip = "#VALX: $#VALY";
            foreach (var kvp in ganancia)
                s2.Points.AddXY(kvp.Key.ToString("dd/MM"), (double)kvp.Value);

            chartIngresos.Series.Add(s1);
            chartIngresos.Series.Add(s2);
            chartIngresos.Legends.Add(AppTheme.CrearLegend());
        }

        private void CargarGraficoPartidas()
        {
            chartPartidas.Series.Clear();
            chartPartidas.Titles.Clear();
            chartPartidas.ChartAreas.Clear();

            ChartArea area = new ChartArea();
            AppTheme.EstiloAreaChart(area);
            chartPartidas.ChartAreas.Add(area);

            chartPartidas.Titles.Add(new Title("Partidas por Juego",
                Docking.Top, new Font("Segoe UI", 10F, FontStyle.Bold), Color.FromArgb(148, 163, 184)));

            Series serie = new Series
            {
                ChartType = SeriesChartType.Doughnut,
                BackGradientStyle = GradientStyle.None
            };

            Dictionary<string, int> datos = _adminSvc.ObtenerPartidasPorJuego();
            Color[] colores = { AppTheme.Azul, AppTheme.Dorado, AppTheme.Verde };
            int i = 0;
            foreach (var kvp in datos)
            {
                int idx = serie.Points.AddXY(kvp.Key, kvp.Value);
                serie.Points[idx].Color = colores[i % colores.Length];
                serie.Points[idx].ToolTip = $"{kvp.Key}: {kvp.Value} partidas";
                i++;
            }

            chartPartidas.Series.Add(serie);
            chartPartidas.Legends.Add(AppTheme.CrearLegend());
        }

        private void CargarTopJugadores()
        {
            dgvTopJugadores.DataSource = null;
            dgvTopJugadores.Rows.Clear();
            try
            {
                IList<EstadisticasUsuario> top = _adminSvc.ObtenerTopJugadores(5);
                int pos = 1;
                var filas = top.Select(t => new
                {
                    Pos = pos++,
                    Jugador = t.NombreUsuario,
                    Partidas = t.TotalPartidas,
                    Ganadas = t.PartidasGanadas,
                    Ganancia = t.GananciaNeta
                }).ToList();

                dgvTopJugadores.DataSource = filas;

                foreach (DataGridViewColumn col in dgvTopJugadores.Columns)
                {
                    col.Visible = false;
                }
                if (dgvTopJugadores.Columns["Pos"] != null) dgvTopJugadores.Columns["Pos"].Visible = true;
                if (dgvTopJugadores.Columns["Jugador"] != null) dgvTopJugadores.Columns["Jugador"].Visible = true;
                if (dgvTopJugadores.Columns["Partidas"] != null) dgvTopJugadores.Columns["Partidas"].Visible = true;
                if (dgvTopJugadores.Columns["Ganadas"] != null) dgvTopJugadores.Columns["Ganadas"].Visible = true;
                if (dgvTopJugadores.Columns["Ganancia"] != null)
                {
                    dgvTopJugadores.Columns["Ganancia"].Visible = true;
                    dgvTopJugadores.Columns["Ganancia"].DefaultCellStyle.Format = "N2";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.GetType().Name} en CargarTopJugadores: {ex.Message}\n\n{ex.StackTrace}",
                    "Dashboard - Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
