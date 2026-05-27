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
    public partial class UcAdminReportes : UserControl
    {
        private readonly AdminServicio _adminSvc = new AdminServicio();
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();
        private readonly PartidaServicio _partidaSvc = new PartidaServicio();
        private string _reporteActivo;

        public UcAdminReportes()
        {
            InitializeComponent();
            AppTheme.ApplyView(this);
            AppTheme.ApplyTitle(lblTitulo);
            AppTheme.ApplyCard(pnlBotones, AppTheme.CardRadius);
            AppTheme.ApplySmallButton(btnReporteUsuarios, AppTheme.Azul);
            AppTheme.ApplySmallButton(btnReportePartidas, AppTheme.Verde);
            AppTheme.ApplySmallButton(btnReporteFinanciero, AppTheme.Dorado);
            btnReporteFinanciero.ForeColor = Color.FromArgb(15, 23, 42);
            AppTheme.ApplySmallButton(btnTopJugadores, AppTheme.Violeta);
            AppTheme.ApplySmallButton(btnExportarTxt, AppTheme.BgElevated);
            lblFechaHora.ForeColor = AppTheme.TextoSecundario;
            lblFechaHora.Font = AppTheme.Subtitulo;
            _reporteActivo = null;
        }

        private void ActualizarFechaHora()
        {
            lblFechaHora.Text = "Generado: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
        }

        private void ActivarExportar(bool activo)
        {
            btnExportarTxt.Enabled = activo;
            btnExportarTxt.BackColor = activo ? Color.FromArgb(37, 99, 235) : Color.FromArgb(71, 85, 105);
        }

        private FlowLayoutPanel CrearFlowContenedor()
        {
            return new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Top,
                Padding = new Padding(16),
                BackColor = Color.Transparent
            };
        }



        private Panel CrearCardKPI(string titulo, string valor, Color color)
        {
            Panel card = new Panel
            {
                Width = 280,
                Height = 80,
                Margin = new Padding(6, 6, 6, 6),
                BackColor = Color.FromArgb(17, 28, 50)
            };

            Label lblValor = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = color,
                Text = valor,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(16, 0, 0, 0)
            };

            Label lblTit = new Label
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(148, 163, 184),
                Height = 24,
                Text = titulo.ToUpper(),
                TextAlign = ContentAlignment.BottomLeft,
                Padding = new Padding(16, 8, 0, 0)
            };

            card.Controls.Add(lblValor);
            card.Controls.Add(lblTit);
            return card;
        }

        private Label CrearSeccionTitulo(string texto)
        {
            return new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = AppTheme.TextoPrimario,
                Text = texto,
                Margin = new Padding(6, 16, 6, 4)
            };
        }

        private Label CrearLabelInfo(string texto, Color? color = null)
        {
            return new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10F),
                ForeColor = color ?? AppTheme.TextoSecundario,
                Text = texto,
                Margin = new Padding(6, 2, 6, 2)
            };
        }

        private DataGridView CrearGrid()
        {
            DataGridView dgv = new DataGridView();
            AppTheme.ApplyDataGrid(dgv);
            dgv.Width = pnlContenido.ClientSize.Width - 48;
            return dgv;
        }

        private void LimpiarContenido()
        {
            pnlContenido.Controls.Clear();
        }

        // ── REPORTE USUARIOS ──────────────────────────────────

        private void btnReporteUsuarios_Click(object sender, EventArgs e)
        {
            _reporteActivo = "usuarios";
            ActivarExportar(true);
            ActualizarFechaHora();
            try
            {
                MostrarReporteUsuarios();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void MostrarReporteUsuarios()
        {
            LimpiarContenido();
            IList<Usuario> lista = _usuarioSvc.ObtenerTodos();
            FlowLayoutPanel flp = CrearFlowContenedor();

            int total = lista.Count;
            int activos = lista.Count(u => u.Estado == "activo");
            int inactivos = lista.Count(u => u.Estado == "inactivo");
            int suspendidos = lista.Count(u => u.Estado == "suspendido");

            FlowLayoutPanel kpiRow = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 8)
            };
            kpiRow.Controls.Add(CrearCardKPI("Total Usuarios", total.ToString("N0"), AppTheme.Dorado));
            kpiRow.Controls.Add(CrearCardKPI("Activos", activos.ToString("N0"), AppTheme.Verde));
            kpiRow.Controls.Add(CrearCardKPI("Inactivos", inactivos.ToString("N0"), AppTheme.TextoSecundario));
            kpiRow.Controls.Add(CrearCardKPI("Suspendidos", suspendidos.ToString("N0"), AppTheme.Rojo));
            flp.Controls.Add(kpiRow);

            flp.Controls.Add(CrearSeccionTitulo("Distribucion de Saldos"));
            decimal saldoTotal = lista.Sum(u => u.Saldo);
            decimal saldoProm = total > 0 ? saldoTotal / total : 0;
            var conSaldo = lista.Where(u => u.Estado == "activo").ToList();
            Usuario mayorSaldo = conSaldo.OrderByDescending(u => u.Saldo).FirstOrDefault();
            Usuario menorSaldo = conSaldo.Where(u => u.Saldo >= 0).OrderBy(u => u.Saldo).FirstOrDefault();

            flp.Controls.Add(CrearLabelInfo($"Saldo total combinado: ${saldoTotal:N2}"));
            flp.Controls.Add(CrearLabelInfo($"Saldo promedio por usuario: ${saldoProm:N2}"));
            if (mayorSaldo != null)
                flp.Controls.Add(CrearLabelInfo($"Mayor saldo: {mayorSaldo.Nombre1} {mayorSaldo.Apellido1} - ${mayorSaldo.Saldo:N2}", AppTheme.Verde));
            if (menorSaldo != null)
                flp.Controls.Add(CrearLabelInfo($"Menor saldo (activo): {menorSaldo.Nombre1} {menorSaldo.Apellido1} - ${menorSaldo.Saldo:N2}", AppTheme.Rojo));

            flp.Controls.Add(CrearSeccionTitulo("Listado de Usuarios"));
            DataGridView dgv = CrearGrid();
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowTemplate.Height = 38;

            var filas = lista.Select(u => new
            {
                ID = u.IdUsuario,
                Nombre = $"{u.Nombre1} {u.Apellido1}".Trim(),
                Username = u.Username,
                Estado = u.Estado,
                Saldo = u.Saldo,
                Registro = u.FechaRegistro.ToString("dd/MM/yyyy HH:mm")
            }).ToList();
            dgv.DataSource = filas;

            foreach (DataGridViewColumn col in dgv.Columns)
                col.Visible = false;
            if (dgv.Columns["ID"] != null) dgv.Columns["ID"].Visible = true;
            if (dgv.Columns["Nombre"] != null) dgv.Columns["Nombre"].Visible = true;
            if (dgv.Columns["Username"] != null) dgv.Columns["Username"].Visible = true;
            if (dgv.Columns["Estado"] != null) dgv.Columns["Estado"].Visible = true;
            if (dgv.Columns["Saldo"] != null)
            {
                dgv.Columns["Saldo"].Visible = true;
                dgv.Columns["Saldo"].DefaultCellStyle.Format = "N2";
            }
            if (dgv.Columns["Registro"] != null) dgv.Columns["Registro"].Visible = true;

            int hUsuarios = dgv.ColumnHeadersHeight + (38 * Math.Min(lista.Count, 18)) + 4;
            if (hUsuarios > 700) hUsuarios = 700;
            if (hUsuarios < 200) hUsuarios = 200;
            dgv.Height = hUsuarios;

            dgv.CellFormatting += (s, ev) =>
            {
                if (dgv.Columns[ev.ColumnIndex].Name == "Estado" && ev.Value != null)
                {
                    string estado = ev.Value.ToString().ToLower();
                    ev.CellStyle.ForeColor = estado == "activo" ? AppTheme.Verde :
                                              estado == "suspendido" ? AppTheme.Rojo :
                                              AppTheme.TextoSecundario;
                    ev.CellStyle.Font = AppTheme.ValorChico;
                }
            };

            flp.Controls.Add(dgv);
            pnlContenido.Controls.Add(flp);
        }

        // ── REPORTE PARTIDAS ──────────────────────────────────

        private void btnReportePartidas_Click(object sender, EventArgs e)
        {
            _reporteActivo = "partidas";
            ActivarExportar(true);
            ActualizarFechaHora();
            try
            {
                MostrarReportePartidas();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void MostrarReportePartidas()
        {
            LimpiarContenido();
            IList<Partida> todas = _partidaSvc.ObtenerTodas();
            IList<PartidaDisplayDto> conNombres = _partidaSvc.ObtenerTodasConNombres();
            FlowLayoutPanel flp = CrearFlowContenedor();

            int total = todas.Count;
            int ganadas = todas.Count(p => p.IdEstado == 2);
            int perdidas = todas.Count(p => p.IdEstado == 3);
            decimal totalApostado = todas.Sum(p => p.Apuesta);
            decimal totalGanado = todas.Sum(p => p.Ganancia);
            decimal gananciaCasa = totalApostado - totalGanado;

            FlowLayoutPanel kpiRow = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 8)
            };
            kpiRow.Controls.Add(CrearCardKPI("Total Partidas", total.ToString("N0"), AppTheme.Dorado));
            kpiRow.Controls.Add(CrearCardKPI("Ganadas", ganadas.ToString("N0"), AppTheme.Verde));
            kpiRow.Controls.Add(CrearCardKPI("Perdidas", perdidas.ToString("N0"), AppTheme.Rojo));
            kpiRow.Controls.Add(CrearCardKPI("Total Apostado", $"${totalApostado:N0}", AppTheme.Azul));
            kpiRow.Controls.Add(CrearCardKPI("Total Ganado", $"${totalGanado:N0}", AppTheme.TextoPrimario));
            flp.Controls.Add(kpiRow);

            flp.Controls.Add(CrearSeccionTitulo("Ganancia de la Casa"));
            flp.Controls.Add(CrearLabelInfo(
                $"${gananciaCasa:N2}",
                gananciaCasa >= 0 ? AppTheme.Dorado : AppTheme.Rojo));

            TableLayoutPanel tlpChartDgv = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Width = pnlContenido.ClientSize.Width - 64,
                AutoSize = true,
                BackColor = Color.Transparent
            };

            // ── Left: Chart ──
            Chart chart = new Chart
            {
                Width = 340,
                Height = 340,
                BackColor = Color.Transparent,
                Margin = new Padding(6, 4, 16, 8),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            ChartArea area = new ChartArea();
            area.BackColor = Color.Transparent;
            area.BorderColor = Color.Transparent;
            chart.ChartAreas.Add(area);

            Series serie = new Series
            {
                ChartType = SeriesChartType.Doughnut,
                BackGradientStyle = GradientStyle.None
            };

            Dictionary<string, int> partidasPorJuego = _adminSvc.ObtenerPartidasPorJuego();
            Color[] colores = { AppTheme.Azul, AppTheme.Dorado, AppTheme.Verde };
            int idx = 0;
            foreach (var kvp in partidasPorJuego)
            {
                int pt = serie.Points.AddXY(kvp.Key, kvp.Value);
                serie.Points[pt].Color = colores[idx % colores.Length];
                serie.Points[pt].ToolTip = $"{kvp.Key}: {kvp.Value}";
                idx++;
            }
            chart.Series.Add(serie);
            chart.Legends.Add(new Legend
            {
                Docking = Docking.Bottom,
                BackColor = Color.Transparent,
                ForeColor = AppTheme.TextoSecundario,
                Font = new Font("Segoe UI", 9F),
                Alignment = StringAlignment.Center
            });

            flp.Controls.Add(CrearSeccionTitulo("Partidas por Juego y Ultimas Partidas"));
            tlpChartDgv.Controls.Add(chart, 0, 0);

            // ── Right: DGV ──
            DataGridView dgv = CrearGrid();
            dgv.Width = tlpChartDgv.Width - chart.Width - 40;
            dgv.Height = chart.Height;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            var ultimas = conNombres.Take(20).ToList();
            var filas = ultimas.Select(p => new
            {
                Jugador = p.Usuario,
                Juego = p.Juego,
                Estado = p.Estado,
                Apuesta = p.Apuesta,
                Ganancia = p.Ganancia,
                Fecha = p.Fecha.ToString("dd/MM/yyyy HH:mm")
            }).ToList();
            dgv.DataSource = filas;

            foreach (DataGridViewColumn col in dgv.Columns)
                col.Visible = false;
            if (dgv.Columns["Jugador"] != null) dgv.Columns["Jugador"].Visible = true;
            if (dgv.Columns["Juego"] != null) dgv.Columns["Juego"].Visible = true;
            if (dgv.Columns["Estado"] != null) dgv.Columns["Estado"].Visible = true;
            if (dgv.Columns["Apuesta"] != null)
            {
                dgv.Columns["Apuesta"].Visible = true;
                dgv.Columns["Apuesta"].DefaultCellStyle.Format = "N2";
            }
            if (dgv.Columns["Ganancia"] != null)
            {
                dgv.Columns["Ganancia"].Visible = true;
                dgv.Columns["Ganancia"].DefaultCellStyle.Format = "N2";
            }
            if (dgv.Columns["Fecha"] != null) dgv.Columns["Fecha"].Visible = true;

            dgv.CellFormatting += (s, ev) =>
            {
                if (ev.RowIndex < 0) return;
                string estado = dgv.Rows[ev.RowIndex].Cells["Estado"]?.Value?.ToString()?.ToLower() ?? "";
                if (estado.Contains("gan"))
                    ev.CellStyle.ForeColor = AppTheme.Verde;
                else if (estado.Contains("perd"))
                    ev.CellStyle.ForeColor = AppTheme.Rojo;
            };

            tlpChartDgv.Controls.Add(dgv, 1, 0);
            tlpChartDgv.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, chart.Width + 16));
            tlpChartDgv.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            flp.Controls.Add(tlpChartDgv);
            pnlContenido.Controls.Add(flp);
        }

        // ── REPORTE FINANCIERO ────────────────────────────────

        private void btnReporteFinanciero_Click(object sender, EventArgs e)
        {
            _reporteActivo = "financiero";
            ActivarExportar(true);
            ActualizarFechaHora();
            try
            {
                MostrarReporteFinanciero();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void MostrarReporteFinanciero()
        {
            LimpiarContenido();
            FlowLayoutPanel flp = CrearFlowContenedor();

            decimal totalDepositos = _adminSvc.ObtenerTotalTransacciones("deposito");
            decimal totalRetiros = _adminSvc.ObtenerTotalTransacciones("retiro");
            decimal balanceNeto = totalDepositos - totalRetiros;

            FlowLayoutPanel kpiRow = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 8)
            };
            kpiRow.Controls.Add(CrearCardKPI("Total Depositos", $"${totalDepositos:N0}", AppTheme.Verde));
            kpiRow.Controls.Add(CrearCardKPI("Total Retiros", $"${totalRetiros:N0}", AppTheme.Rojo));
            kpiRow.Controls.Add(CrearCardKPI("Balance Neto", $"${balanceNeto:N0}", balanceNeto >= 0 ? AppTheme.Dorado : AppTheme.Rojo));
            flp.Controls.Add(kpiRow);

            // ── Top depositantes ──
            flp.Controls.Add(CrearSeccionTitulo("Top Depositantes"));
            var topDep = _adminSvc.ObtenerTopDepositantes(5);
            DataGridView dgvDep = CrearGrid();
            dgvDep.DataSource = topDep.Select((t, i) => new
            {
                Pos = i + 1,
                UsuarioID = t.IdUsuario,
                TotalDepositado = t.TotalDepositado,
                NumDepositos = t.NumDepositos
            }).ToList();

            foreach (DataGridViewColumn col in dgvDep.Columns)
                col.Visible = false;
            if (dgvDep.Columns["Pos"] != null)
            {
                dgvDep.Columns["Pos"].Visible = true;
                dgvDep.Columns["Pos"].Width = 45;
                dgvDep.Columns["Pos"].MinimumWidth = 45;
            }
            if (dgvDep.Columns["UsuarioID"] != null) dgvDep.Columns["UsuarioID"].Visible = true;
            if (dgvDep.Columns["TotalDepositado"] != null)
            {
                dgvDep.Columns["TotalDepositado"].Visible = true;
                dgvDep.Columns["TotalDepositado"].DefaultCellStyle.Format = "N2";
            }
            if (dgvDep.Columns["NumDepositos"] != null) dgvDep.Columns["NumDepositos"].Visible = true;

            AppTheme.ConfigurarColumnasDgv(dgvDep, maxAltura: 350);
            dgvDep.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDep.Width = pnlContenido.ClientSize.Width - 64;
            flp.Controls.Add(dgvDep);

            // ── Rentabilidad por juego ──
            flp.Controls.Add(CrearSeccionTitulo("Rentabilidad por Juego"));
            var rentabilidad = _adminSvc.ObtenerRentabilidadPorJuego();
            DataGridView dgvRent = CrearGrid();
            dgvRent.DataSource = rentabilidad.Select(r => new
            {
                r.Juego,
                r.Partidas,
                TotalApostado = r.TotalApostado,
                GananciaCasa = r.GananciaCasa,
                Margen = r.Margen
            }).ToList();

            foreach (DataGridViewColumn col in dgvRent.Columns)
                col.Visible = false;
            if (dgvRent.Columns["Juego"] != null) dgvRent.Columns["Juego"].Visible = true;
            if (dgvRent.Columns["Partidas"] != null) dgvRent.Columns["Partidas"].Visible = true;
            if (dgvRent.Columns["TotalApostado"] != null)
            {
                dgvRent.Columns["TotalApostado"].Visible = true;
                dgvRent.Columns["TotalApostado"].DefaultCellStyle.Format = "N2";
            }
            if (dgvRent.Columns["GananciaCasa"] != null)
            {
                dgvRent.Columns["GananciaCasa"].Visible = true;
                dgvRent.Columns["GananciaCasa"].DefaultCellStyle.Format = "N2";
            }
            if (dgvRent.Columns["Margen"] != null)
            {
                dgvRent.Columns["Margen"].Visible = true;
                dgvRent.Columns["Margen"].DefaultCellStyle.Format = "0.0";
            }

            AppTheme.ConfigurarColumnasDgv(dgvRent, maxAltura: 350);
            dgvRent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRent.Width = pnlContenido.ClientSize.Width - 64;
            flp.Controls.Add(dgvRent);

            // ── Grafico barras 6 meses ──
            flp.Controls.Add(CrearSeccionTitulo("Movimientos por Mes (Ultimos 6 Meses)"));
            Chart chart = new Chart
            {
                Width = pnlContenido.ClientSize.Width - 80,
                Height = 280,
                BackColor = Color.Transparent,
                Margin = new Padding(6, 4, 6, 8)
            };

            ChartArea ca = new ChartArea();
            AppTheme.EstiloAreaChart(ca);
            ca.AxisY.LabelStyle.Format = "C0";
            chart.ChartAreas.Add(ca);

            Dictionary<string, (decimal Depositos, decimal Retiros)> movimientos = _adminSvc.ObtenerMovimientosPorMes(6);

            Series sDep = new Series { Name = "Depositos", ChartType = SeriesChartType.StackedColumn };
            sDep.Color = AppTheme.Verde;
            sDep.ToolTip = "#VALX: Depositos $#VALY";

            Series sRet = new Series { Name = "Retiros", ChartType = SeriesChartType.StackedColumn };
            sRet.Color = Color.FromArgb(239, 68, 68);
            sRet.ToolTip = "#VALX: Retiros $#VALY";

            foreach (var kvp in movimientos)
            {
                sDep.Points.AddXY(kvp.Key, (double)kvp.Value.Depositos);
                sRet.Points.AddXY(kvp.Key, (double)kvp.Value.Retiros);
            }

            chart.Series.Add(sDep);
            chart.Series.Add(sRet);
            chart.Legends.Add(AppTheme.CrearLegend());
            flp.Controls.Add(chart);

            // ── Stats summary ──
            var rentaTotal = rentabilidad.Sum(r => r.GananciaCasa);
            var totalApost = rentabilidad.Sum(r => r.TotalApostado);
            flp.Controls.Add(CrearLabelInfo($"Ganancia total de la casa por juego: ${rentaTotal:N2}"));
            flp.Controls.Add(CrearLabelInfo($"Total apostado en todos los juegos: ${totalApost:N2}"));
            if (rentabilidad.Any())
            {
                var masRentable = rentabilidad.OrderByDescending(r => r.Margen).First();
                flp.Controls.Add(CrearLabelInfo(
                    $"Juego mas rentable: {masRentable.Juego} ({masRentable.Margen:F1}% de margen)", AppTheme.Verde));
            }

            pnlContenido.Controls.Add(flp);
        }

        // ── TOP JUGADORES ─────────────────────────────────────

        private void btnTopJugadores_Click(object sender, EventArgs e)
        {
            _reporteActivo = "top";
            ActivarExportar(true);
            ActualizarFechaHora();
            try
            {
                MostrarTopJugadores();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void MostrarTopJugadores()
        {
            LimpiarContenido();
            IList<EstadisticasUsuario> top = _adminSvc.ObtenerTopJugadores(10);
            FlowLayoutPanel flp = CrearFlowContenedor();

            flp.Controls.Add(CrearSeccionTitulo("Top 10 Jugadores"));
            DataGridView dgv = CrearGrid();

            int pos = 1;
            var filas = top.Select(t => new
            {
                Pos = pos++,
                Jugador = t.NombreUsuario,
                Username = t.Username,
                Partidas = t.TotalPartidas,
                Ganadas = t.PartidasGanadas,
                Perdidas = t.PartidasPerdidas,
                PctVictoria = t.TotalPartidas > 0
                    ? Math.Round((decimal)t.PartidasGanadas / t.TotalPartidas * 100, 1)
                    : 0m,
                TotalApostado = t.TotalApostado,
                GananciaNeta = t.GananciaNeta,
                Saldo = t.Saldo
            }).ToList();
            dgv.DataSource = filas;

            foreach (DataGridViewColumn col in dgv.Columns)
                col.Visible = false;
            if (dgv.Columns["Pos"] != null)
            {
                dgv.Columns["Pos"].Visible = true;
                dgv.Columns["Pos"].Width = 45;
                dgv.Columns["Pos"].MinimumWidth = 45;
            }
            if (dgv.Columns["Jugador"] != null) dgv.Columns["Jugador"].Visible = true;
            if (dgv.Columns["Username"] != null) dgv.Columns["Username"].Visible = true;
            if (dgv.Columns["Partidas"] != null) dgv.Columns["Partidas"].Visible = true;
            if (dgv.Columns["Ganadas"] != null) dgv.Columns["Ganadas"].Visible = true;
            if (dgv.Columns["Perdidas"] != null) dgv.Columns["Perdidas"].Visible = true;
            if (dgv.Columns["PctVictoria"] != null)
            {
                dgv.Columns["PctVictoria"].Visible = true;
                dgv.Columns["PctVictoria"].HeaderText = "% Victoria";
                dgv.Columns["PctVictoria"].DefaultCellStyle.Format = "0.0";
            }
            if (dgv.Columns["TotalApostado"] != null)
            {
                dgv.Columns["TotalApostado"].Visible = true;
                dgv.Columns["TotalApostado"].DefaultCellStyle.Format = "N2";
            }
            if (dgv.Columns["GananciaNeta"] != null)
            {
                dgv.Columns["GananciaNeta"].Visible = true;
                dgv.Columns["GananciaNeta"].DefaultCellStyle.Format = "N2";
            }
            if (dgv.Columns["Saldo"] != null)
            {
                dgv.Columns["Saldo"].Visible = true;
                dgv.Columns["Saldo"].DefaultCellStyle.Format = "N2";
            }

            AppTheme.ConfigurarColumnasDgv(dgv, maxFilas: 10, maxAltura: 650);
            if (dgv.Columns["Pos"] != null)
            {
                dgv.Columns["Pos"].Width = 45;
                dgv.Columns["Pos"].MinimumWidth = 45;
            }
            dgv.Width = pnlContenido.ClientSize.Width - 64;

            dgv.CellFormatting += (s, ev) =>
            {
                if (ev.RowIndex < 0) return;
                if (ev.RowIndex == 0)
                {
                    ev.CellStyle.BackColor = Color.FromArgb(180, 140, 0);
                    ev.CellStyle.ForeColor = Color.Black;
                    ev.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
                if (dgv.Columns[ev.ColumnIndex].Name == "GananciaNeta" && ev.Value != null)
                {
                    try
                    {
                        decimal val = Convert.ToDecimal(ev.Value);
                        if (ev.RowIndex != 0)
                            ev.CellStyle.ForeColor = val >= 0 ? AppTheme.Verde : AppTheme.Rojo;
                    }
                    catch { }
                }
            };

            flp.Controls.Add(dgv);

            var masActivo = top.OrderByDescending(t => t.TotalPartidas).FirstOrDefault();
            double promedioPct = top.Count > 0 ? top.Average(t => t.TotalPartidas > 0 ? (double)t.PartidasGanadas / t.TotalPartidas * 100 : 0) : 0;

            if (masActivo != null)
            {
                flp.Controls.Add(CrearLabelInfo(
                    $"Jugador mas activo: {masActivo.NombreUsuario} con {masActivo.TotalPartidas} partidas."));
            }
            flp.Controls.Add(CrearLabelInfo(
                $"Tasa de victoria promedio del top 10: {promedioPct:F1}%"));

            pnlContenido.Controls.Add(flp);
        }

        // ── EXPORTAR TXT ──────────────────────────────────────

        private void btnExportarTxt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_reporteActivo)) return;

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Archivo de texto (*.txt)|*.txt",
                FileName = $"reporte_{_reporteActivo}_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                string contenido = GenerarTextoExportar();
                System.IO.File.WriteAllText(sfd.FileName, contenido);
                MessageBox.Show($"Reporte exportado correctamente.{Environment.NewLine}{sfd.FileName}",
                    "Exportacion exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerarTextoExportar()
        {
            string nl = Environment.NewLine;
            string sep = new string('-', 70);

            switch (_reporteActivo)
            {
                case "usuarios":
                    return GenerarTxtUsuarios(nl, sep);
                case "partidas":
                    return GenerarTxtPartidas(nl, sep);
                case "financiero":
                    return GenerarTxtFinanciero(nl, sep);
                case "top":
                    return GenerarTxtTop(nl, sep);
                default:
                    return "Sin reporte activo.";
            }
        }

        private string GenerarTxtUsuarios(string nl, string sep)
        {
            IList<Usuario> lista = _usuarioSvc.ObtenerTodos();
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("REPORTE DE USUARIOS");
            sb.AppendLine($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine(sep);
            sb.AppendLine($"Total: {lista.Count}  |  Activos: {lista.Count(u => u.Estado == "activo")}  " +
                          $"|  Inactivos: {lista.Count(u => u.Estado == "inactivo")}  " +
                          $"|  Suspendidos: {lista.Count(u => u.Estado == "suspendido")}");
            sb.AppendLine($"Saldo total: ${lista.Sum(u => u.Saldo):N2}");
            sb.AppendLine(sep);
            sb.AppendLine($"{"ID",-6}{"Nombre",-30}{"Username",-16}{"Estado",-12}{"Saldo",-12}");
            sb.AppendLine(sep);
            foreach (Usuario u in lista)
                sb.AppendLine($"{u.IdUsuario,-6}{$"{u.Nombre1} {u.Apellido1}",-30}{u.Username,-16}{u.Estado,-12}${u.Saldo,9:N2}");
            return sb.ToString();
        }

        private string GenerarTxtPartidas(string nl, string sep)
        {
            IList<Partida> todas = _partidaSvc.ObtenerTodas();
            IList<PartidaDisplayDto> conNombres = _partidaSvc.ObtenerTodasConNombres();
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("REPORTE DE PARTIDAS");
            sb.AppendLine($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine(sep);
            sb.AppendLine($"Total: {todas.Count}  |  Ganadas: {todas.Count(p => p.IdEstado == 2)}  " +
                          $"|  Perdidas: {todas.Count(p => p.IdEstado == 3)}");
            sb.AppendLine($"Total Apostado: ${todas.Sum(p => p.Apuesta):N2}  " +
                          $"|  Total Ganado: ${todas.Sum(p => p.Ganancia):N2}");
            sb.AppendLine($"Ganancia Casa: ${(todas.Sum(p => p.Apuesta) - todas.Sum(p => p.Ganancia)):N2}");
            sb.AppendLine(sep);
            sb.AppendLine($"{"Jugador",-22}{"Juego",-16}{"Estado",-12}{"Apuesta",-14}{"Ganancia",-14}");
            sb.AppendLine(sep);
            foreach (var p in conNombres.Take(50))
                sb.AppendLine($"{p.Usuario,-22}{p.Juego,-16}{p.Estado,-12}${p.Apuesta,10:N2}  ${p.Ganancia,10:N2}");
            return sb.ToString();
        }

        private string GenerarTxtFinanciero(string nl, string sep)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("REPORTE FINANCIERO");
            sb.AppendLine($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine(sep);

            decimal totalDep = _adminSvc.ObtenerTotalTransacciones("deposito");
            decimal totalRet = _adminSvc.ObtenerTotalTransacciones("retiro");
            sb.AppendLine($"Total Depositos: ${totalDep:N2}");
            sb.AppendLine($"Total Retiros: ${totalRet:N2}");
            sb.AppendLine($"Balance Neto: ${(totalDep - totalRet):N2}");
            sb.AppendLine(sep);

            sb.AppendLine("Rentabilidad por Juego:");
            var rentabilidad = _adminSvc.ObtenerRentabilidadPorJuego();
            sb.AppendLine($"{"Juego",-16}{"Partidas",-10}{"Apostado",-16}{"Ganancia Casa",-16}{"Margen",-10}");
            sb.AppendLine(sep);
            foreach (var r in rentabilidad)
                sb.AppendLine($"{r.Juego,-16}{r.Partidas,-10}${r.TotalApostado,10:N2}  ${r.GananciaCasa,10:N2}  {r.Margen,6:F1}%");

            sb.AppendLine(sep);
            sb.AppendLine("Movimientos por Mes (Ultimos 6):");
            var movimientos = _adminSvc.ObtenerMovimientosPorMes(6);
            sb.AppendLine($"{"Mes",-12}{"Depositos",-16}{"Retiros",-16}");
            sb.AppendLine(sep);
            foreach (var kvp in movimientos)
                sb.AppendLine($"{kvp.Key,-12}${kvp.Value.Depositos,10:N2}  ${kvp.Value.Retiros,10:N2}");

            return sb.ToString();
        }

        private string GenerarTxtTop(string nl, string sep)
        {
            IList<EstadisticasUsuario> top = _adminSvc.ObtenerTopJugadores(10);
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("TOP 10 JUGADORES");
            sb.AppendLine($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine(sep);
            sb.AppendLine($"{"#",-4}{"Jugador",-24}{"Username",-16}{"Partidas",-10}{"Ganadas",-10}{"Perdidas",-10}{"% Victoria",-12}{"Gan. Neta",-14}");
            sb.AppendLine(sep);
            int pos = 0;
            foreach (EstadisticasUsuario t in top)
            {
                pos++;
                decimal pct = t.TotalPartidas > 0 ? (decimal)t.PartidasGanadas / t.TotalPartidas * 100 : 0;
                sb.AppendLine($"{pos,-4}{t.NombreUsuario,-24}{t.Username,-16}{t.TotalPartidas,-10}{t.PartidasGanadas,-10}" +
                              $"{t.PartidasPerdidas,-10}{pct,7:F1}%    ${t.GananciaNeta,9:N2}");
            }
            return sb.ToString();
        }

        // ── ERROR ─────────────────────────────────────────────

        private void MostrarError(Exception ex)
        {
            LimpiarContenido();
            Label lblError = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = AppTheme.Rojo,
                Text = $"Error al cargar reporte: {ex.Message}",
                Margin = new Padding(20, 20, 0, 0)
            };
            pnlContenido.Controls.Add(lblError);
        }
    }
}
