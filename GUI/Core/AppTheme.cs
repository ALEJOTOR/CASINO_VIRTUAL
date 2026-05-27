using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GUI
{
    public static class AppTheme
    {
        // ── Palette ──
        public static Color BgPrincipal  = ColorTranslator.FromHtml("#0D1117");
        public static Color BgCard       = ColorTranslator.FromHtml("#161B27");
        public static Color BgHover      = ColorTranslator.FromHtml("#1E2535");
        public static Color BgNavbar     = ColorTranslator.FromHtml("#0B1120");
        public static Color BgElevated   = ColorTranslator.FromHtml("#1C2536");
        public static Color BgInput      = ColorTranslator.FromHtml("#0F1729");

        public static Color Dorado       = ColorTranslator.FromHtml("#FBBF24");
        public static Color Verde        = ColorTranslator.FromHtml("#22C55E");
        public static Color Rojo         = ColorTranslator.FromHtml("#EF4444");
        public static Color Azul         = ColorTranslator.FromHtml("#3B82F6");
        public static Color Violeta      = ColorTranslator.FromHtml("#8B5CF6");
        public static Color TextoPrimario   = ColorTranslator.FromHtml("#F1F5F9");
        public static Color TextoSecundario = ColorTranslator.FromHtml("#94A3B8");
        public static Color TexoMuted       = ColorTranslator.FromHtml("#64748B");
        public static Color Borde           = ColorTranslator.FromHtml("#1E293B");
        public static Color BordeClaro      = ColorTranslator.FromHtml("#2A3A52");

        // ── Typography ──
        public static Font TituloGrande  = new Font("Segoe UI", 20, FontStyle.Bold);
        public static Font Subtitulo     = new Font("Segoe UI", 10, FontStyle.Regular);
        public static Font Cuerpo        = new Font("Segoe UI", 11, FontStyle.Regular);
        public static Font Valor         = new Font("Segoe UI", 13, FontStyle.Bold);
        public static Font ValorChico    = new Font("Segoe UI", 9, FontStyle.Bold);
        public static Font LabelAccion   = new Font("Segoe UI", 10, FontStyle.Bold);

        public const int CardRadius = 12;
        public const int NavbarHeight = 56;

        // ── Form ──
        public static void ApplyForm(Form form)
        {
            form.BackColor = BgPrincipal;
            form.Font = Cuerpo;
            EnableFadeIn(form);
        }

        public static void ApplyView(Control root)
        {
            root.BackColor = BgPrincipal;
            root.Font = Cuerpo;
        }

        // ── Navbar ──
        public static void ApplyNavbar(Panel panel, Control separator = null)
        {
            panel.BackColor = BgNavbar;
            panel.Height = NavbarHeight;
            // subtle bottom border drawn via paint
            panel.Paint += (s, e) =>
            {
                using (Pen p = new Pen(Borde, 1))
                    e.Graphics.DrawLine(p, 0, panel.Height - 1, panel.Width, panel.Height - 1);
            };
        }

        public static void ApplyNavbarItem(ToolStripMenuItem item)
        {
            item.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            item.ForeColor = TextoSecundario;
            item.Margin = new Padding(4, 0, 4, 0);
            item.Padding = new Padding(8, 0, 8, 0);
            item.MouseEnter += (s, e) => item.ForeColor = TextoPrimario;
            item.MouseLeave += (s, e) => item.ForeColor = TextoSecundario;
        }

        // ── Labels ──
        public static void ApplyTitle(Label label)
        {
            label.Font = TituloGrande;
            label.ForeColor = Dorado;
        }

        public static void ApplySubtitle(Label label)
        {
            label.Font = Subtitulo;
            label.ForeColor = TextoSecundario;
        }

        public static void ApplyBody(Label label)
        {
            label.Font = Cuerpo;
            label.ForeColor = TextoPrimario;
        }

        // ── Buttons ──
        public static void ApplyPrimaryButton(Button button, Color? color = null)
        {
            Color actual = color ?? Dorado;
            button.BackColor = actual;
            button.ForeColor = actual == Dorado ? Color.Black : Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = LabelAccion;
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
            ApplyRoundedRegion(button, 8);
            AttachButtonHover(button);
        }

        public static void ApplySecondaryButton(Button button)
        {
            button.BackColor = BgElevated;
            button.ForeColor = TextoPrimario;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = BordeClaro;
            button.Font = LabelAccion;
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
            ApplyRoundedRegion(button, 8);
            button.MouseEnter += (s, e) => button.BackColor = BgHover;
            button.MouseLeave += (s, e) => button.BackColor = BgElevated;
        }

        public static void ApplySmallButton(Button button, Color color)
        {
            button.BackColor = color;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = ValorChico;
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
            ApplyRoundedRegion(button, 6);
        }

        // ── Inputs ──
        public static void ApplyTextBox(TextBox textBox)
        {
            textBox.BackColor = BgInput;
            textBox.ForeColor = TextoPrimario;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = Cuerpo;
        }

        public static void ApplyCombo(ComboBox combo)
        {
            combo.BackColor = BgInput;
            combo.ForeColor = TextoPrimario;
            combo.FlatStyle = FlatStyle.Flat;
            combo.Font = Cuerpo;
        }

        // ── Cards ──
        public static void ApplyCard(Panel panel, int radius = CardRadius)
        {
            panel.BackColor = BgCard;
            panel.Padding = new Padding(24);
            ApplyRoundedRegion(panel, radius);
        }

        public static void ApplyCardKPI(Panel panel, Color accent)
        {
            panel.BackColor = BgCard;
            ApplyRoundedRegion(panel, CardRadius);
            // accent top bar and subtle border via paint
            panel.Paint += (s, e) =>
            {
                int barH = 3;
                using (SolidBrush bar = new SolidBrush(accent))
                    e.Graphics.FillRectangle(bar, 0, 0, panel.Width, barH);
                using (Pen p = new Pen(Color.FromArgb(40, 255, 255, 255), 1))
                    e.Graphics.DrawRectangle(p, 0, 0, panel.Width - 1, panel.Height - 1);
            };
        }

        public static void ApplySaldoLabel(Label label)
        {
            label.BackColor = BgCard;
            label.ForeColor = Dorado;
            label.Font = Valor;
            label.TextAlign = ContentAlignment.MiddleRight;
            label.Padding = new Padding(12, 0, 12, 0);
            label.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(BordeClaro, 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, label.Width - 1, label.Height - 1);
            };
            AttachSaldoFlash(label);
        }

        // ── DataGridView ──
        public static void ApplyDataGrid(DataGridView grid)
        {
            grid.BackgroundColor = BgCard;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = Borde;
            grid.EnableHeadersVisualStyles = false;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ReadOnly = true;

            grid.ColumnHeadersDefaultCellStyle.BackColor = BgCard;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Dorado;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            grid.ColumnHeadersHeight = 38;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

            grid.DefaultCellStyle.BackColor = BgPrincipal;
            grid.DefaultCellStyle.ForeColor = TextoPrimario;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            grid.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            grid.DefaultCellStyle.SelectionBackColor = BgElevated;
            grid.DefaultCellStyle.SelectionForeColor = TextoPrimario;
            grid.RowTemplate.Height = 34;

            grid.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#111827");
        }

        public static void ConfigurarColumnasDgv(DataGridView dgv, int maxFilas = 15, int maxAltura = 500)
        {
            dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            foreach (DataGridViewColumn col in dgv.Columns)
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            foreach (DataGridViewColumn col in dgv.Columns)
                if (col.Visible && col.Width < 80)
                    col.MinimumWidth = Math.Max(col.Width, 80);
            dgv.ScrollBars = ScrollBars.Both;

            int header = dgv.ColumnHeadersHeight;
            int row = dgv.RowTemplate.Height;
            int filas = Math.Min(dgv.Rows.Count, maxFilas);
            int h = header + (row * filas) + 4;
            if (h < 120) h = 120;
            if (h > maxAltura) h = maxAltura;
            dgv.Height = h;
        }

        // ── Charts ──
        public static void EstiloAreaChart(ChartArea area)
        {
            area.BackColor = Color.Transparent;
            area.BorderColor = Color.Transparent;
            area.AxisX.LabelStyle.ForeColor = TexoMuted;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
            area.AxisY.LabelStyle.ForeColor = TexoMuted;
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8F);
            area.AxisX.MajorGrid.LineColor = Color.FromArgb(30, 45, 70);
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(30, 45, 70);
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            area.AxisX.MajorTickMark.Enabled = false;
            area.AxisY.MajorTickMark.Enabled = false;
            area.AxisX.LineColor = BordeClaro;
            area.AxisY.LineColor = BordeClaro;
        }

        public static void EstiloChartSerie(Series serie, Color fill, Color border, int borderWidth = 2)
        {
            serie.Color = Color.FromArgb(60, fill);
            serie.BorderColor = border;
            serie.BorderWidth = borderWidth;
        }

        public static Legend CrearLegend()
        {
            return new Legend
            {
                Docking = Docking.Bottom,
                BackColor = Color.Transparent,
                ForeColor = TextoSecundario,
                Font = new Font("Segoe UI", 9F),
                Alignment = StringAlignment.Center
            };
        }

        // ── Effects ──
        public static void EnableFadeIn(Form form)
        {
            form.Opacity = 0;
            Timer timer = new Timer { Interval = 30 };
            timer.Tick += (s, e) =>
            {
                form.Opacity += 0.1;
                if (form.Opacity >= 1)
                {
                    form.Opacity = 1;
                    timer.Stop();
                    timer.Dispose();
                }
            };
            form.Shown += (s, e) => timer.Start();
        }

        public static void AttachButtonHover(Button button)
        {
            Color original = button.BackColor;
            Color hover = Darken(original, 0.12F);
            button.MouseEnter += (s, e) => button.BackColor = hover;
            button.MouseLeave += (s, e) => button.BackColor = original;
        }

        public static void AttachSaldoFlash(Label label)
        {
            bool listo = false;
            label.HandleCreated += (s, e) => listo = true;
            label.TextChanged += (s, e) =>
            {
                if (!listo) return;
                label.ForeColor = Color.White;
                Timer timer = new Timer { Interval = 500 };
                timer.Tick += (sender, args) =>
                {
                    label.ForeColor = Dorado;
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();
            };
        }

        // ── Geometry ──
        public static void ApplyRoundedRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0) return;
            using (GraphicsPath path = RoundedPath(new Rectangle(0, 0, control.Width, control.Height), radius))
                control.Region = new Region(path);
        }

        private static Color Darken(Color color, float amount)
        {
            return Color.FromArgb(
                Math.Max(0, (int)(color.R * (1 - amount))),
                Math.Max(0, (int)(color.G * (1 - amount))),
                Math.Max(0, (int)(color.B * (1 - amount))));
        }

        private static GraphicsPath RoundedPath(Rectangle bounds, int radius)
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
    }
}
