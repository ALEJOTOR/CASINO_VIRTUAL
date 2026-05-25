using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                foreach (var t in targets) t.Text = "—";
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Dashboard",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static void EstiloArea(ChartArea area)
        {
            area.BackColor = Color.Transparent;
            area.BorderColor = Color.Transparent;
            area.AxisX.LabelStyle.ForeColor = CasinoTheme.Muted;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
            area.AxisY.LabelStyle.ForeColor = CasinoTheme.Muted;
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8F);
            area.AxisX.MajorGrid.LineColor = Color.FromArgb(30, 45, 70);
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(30, 45, 70);
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            area.AxisX.MajorTickMark.Enabled = false;
            area.AxisY.MajorTickMark.Enabled = false;
            area.AxisX.LineColor = Color.FromArgb(51, 65, 85);
            area.AxisY.LineColor = Color.FromArgb(51, 65, 85);
        }

        private void CargarGraficoIngresos()
        {
            chartIngresos.Series.Clear();
            chartIngresos.Titles.Clear();
            chartIngresos.ChartAreas.Clear();

            ChartArea area = new ChartArea();
            EstiloArea(area);
            area.AxisY.LabelStyle.Format = "C0";
            chartIngresos.ChartAreas.Add(area);

            chartIngresos.Titles.Add(new Title("Ingresos y Ganancia — Últimos 30 Días",
                Docking.Top, new Font("Segoe UI", 11F, FontStyle.Bold), CasinoTheme.Text));

            Dictionary<DateTime, decimal> ingresos = _adminSvc.ObtenerIngresosPorDia(30);
            Dictionary<DateTime, decimal> ganancia = _adminSvc.ObtenerGananciaCasaPorDia(30);

            Series s1 = new Series { Name = "Ingresos", ChartType = SeriesChartType.SplineArea };
            s1.Color = Color.FromArgb(50, 37, 99, 235);
            s1.BorderColor = Color.FromArgb(59, 130, 246);
            s1.BorderWidth = 2;
            s1.ToolTip = "#VALX: $#VALY";
            foreach (var kvp in ingresos)
                s1.Points.AddXY(kvp.Key.ToString("dd/MM"), (double)kvp.Value);

            Series s2 = new Series { Name = "Ganancia Casa", ChartType = SeriesChartType.Spline };
            s2.Color = Color.FromArgb(250, 204, 21);
            s2.BorderWidth = 2;
            s2.ToolTip = "#VALX: $#VALY";
            foreach (var kvp in ganancia)
                s2.Points.AddXY(kvp.Key.ToString("dd/MM"), (double)kvp.Value);

            chartIngresos.Series.Add(s1);
            chartIngresos.Series.Add(s2);

            chartIngresos.Legends.Add(new Legend
            {
                Docking = Docking.Bottom,
                BackColor = Color.Transparent,
                ForeColor = CasinoTheme.Text,
                Font = new Font("Segoe UI", 9F),
                Alignment = StringAlignment.Center
            });
        }

        private void CargarGraficoPartidas()
        {
            chartPartidas.Series.Clear();
            chartPartidas.Titles.Clear();
            chartPartidas.ChartAreas.Clear();

            ChartArea area = new ChartArea();
            EstiloArea(area);
            chartPartidas.ChartAreas.Add(area);

            chartPartidas.Titles.Add(new Title("Partidas por Juego",
                Docking.Top, new Font("Segoe UI", 10F, FontStyle.Bold), CasinoTheme.Text));

            Series serie = new Series
            {
                ChartType = SeriesChartType.Doughnut,
                BackGradientStyle = GradientStyle.None
            };

            Dictionary<string, int> datos = _adminSvc.ObtenerPartidasPorJuego();
            Color[] colores = { Color.FromArgb(59, 130, 246), Color.FromArgb(250, 204, 21), Color.FromArgb(34, 197, 94) };
            int i = 0;
            foreach (var kvp in datos)
            {
                int idx = serie.Points.AddXY(kvp.Key, kvp.Value);
                serie.Points[idx].Color = colores[i % colores.Length];
                serie.Points[idx].ToolTip = $"{kvp.Key}: {kvp.Value}";
                i++;
            }

            chartPartidas.Series.Add(serie);

            chartPartidas.Legends.Add(new Legend
            {
                Docking = Docking.Bottom,
                BackColor = Color.Transparent,
                ForeColor = CasinoTheme.Text,
                Font = new Font("Segoe UI", 9F),
                Alignment = StringAlignment.Center
            });
        }

        private void CargarTopJugadores()
        {
            dgvTopJugadores.DataSource = null;
            dgvTopJugadores.Rows.Clear();
            try
            {
                IList<EstadisticasUsuario> top = _adminSvc.ObtenerTopJugadores(5);
                dgvTopJugadores.DataSource = top;

                if (dgvTopJugadores.Columns.Count <= 0) return;

                foreach (DataGridViewColumn col in dgvTopJugadores.Columns)
                {
                    col.Visible = false;
                }

                string[][] cols = {
                    new[] { "NombreUsuario", "Jugador" },
                    new[] { "TotalPartidas", "Partidas" },
                    new[] { "GananciaNeta", "Ganancia Neta" }
                };

                foreach (var c in cols)
                {
                    if (dgvTopJugadores.Columns.Contains(c[0]))
                    {
                        dgvTopJugadores.Columns[c[0]].Visible = true;
                        dgvTopJugadores.Columns[c[0]].HeaderText = c[1];
                        if (c[0] == "GananciaNeta")
                            dgvTopJugadores.Columns[c[0]].DefaultCellStyle.Format = "C2";
                    }
                }
            }
            catch
            {
            }
        }
    }
}
