using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GUI
{
    public static class AppTheme
    {
        // Problema visual que resuelve: centraliza colores y fuentes para que toda la GUI conserve una identidad consistente.
        public static Color BgPrincipal = ColorTranslator.FromHtml("#0D1117");
        public static Color BgCard = ColorTranslator.FromHtml("#161B27");
        public static Color BgHover = ColorTranslator.FromHtml("#1E2535");
        public static Color BgNavbar = ColorTranslator.FromHtml("#0A0F1E");
        public static Color Dorado = ColorTranslator.FromHtml("#FFD700");
        public static Color Verde = ColorTranslator.FromHtml("#22C55E");
        public static Color Rojo = ColorTranslator.FromHtml("#EF4444");
        public static Color Azul = ColorTranslator.FromHtml("#3B82F6");
        public static Color TextoPrimario = ColorTranslator.FromHtml("#F1F5F9");
        public static Color TextoSecundario = ColorTranslator.FromHtml("#94A3B8");
        public static Font TituloGrande = new Font("Segoe UI", 20, FontStyle.Bold);
        public static Font Subtitulo = new Font("Segoe UI", 10, FontStyle.Regular);
        public static Font Cuerpo = new Font("Segoe UI", 11, FontStyle.Regular);
        public static Font Valor = new Font("Segoe UI", 13, FontStyle.Bold);

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
            ApplyTypography(root);
        }

        public static void ApplyNavbar(Panel panel)
        {
            panel.BackColor = BgNavbar;
            panel.Height = 60;
        }

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

        public static void ApplyPrimaryButton(Button button, Color? color = null)
        {
            button.BackColor = color ?? Dorado;
            button.ForeColor = (color == null || color.Value == Dorado) ? Color.Black : Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
            ApplyRoundedRegion(button, 8);
            AttachButtonHover(button);
        }

        public static void ApplyTextBox(TextBox textBox)
        {
            textBox.BackColor = BgHover;
            textBox.ForeColor = TextoPrimario;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = Cuerpo;
            AttachTextBoxFocusBorder(textBox);
        }

        public static void ApplyCombo(ComboBox combo)
        {
            combo.BackColor = BgHover;
            combo.ForeColor = TextoPrimario;
            combo.FlatStyle = FlatStyle.Flat;
            combo.Font = Cuerpo;
        }

        public static void ApplyCard(Panel panel, int radius = 12)
        {
            panel.BackColor = BgCard;
            panel.Padding = new Padding(24);
            panel.Resize += (s, e) => ApplyRoundedRegion(panel, radius);
            ApplyRoundedRegion(panel, radius);
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
                using (Pen pen = new Pen(Dorado, 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, label.Width - 1, label.Height - 1);
            };
            AttachSaldoFlash(label);
        }

        public static void ApplyDataGrid(DataGridView grid)
        {
            grid.BackgroundColor = BgCard;
            grid.BorderStyle = BorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = BgCard;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Dorado;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grid.DefaultCellStyle.BackColor = BgPrincipal;
            grid.DefaultCellStyle.ForeColor = TextoPrimario;
            grid.DefaultCellStyle.SelectionBackColor = BgHover;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = BgCard;
            grid.GridColor = BgHover;
        }

        public static void ApplyTypography(Control root)
        {
            foreach (Control control in root.Controls)
            {
                if (control is Label label)
                {
                    string name = label.Name.ToLowerInvariant();
                    if (name.Contains("titulo") || name.Contains("bienvenido") || name.Contains("marca"))
                        ApplyTitle(label);
                    else if (name.Contains("subtitulo") || name.Contains("descripcion") || name.Contains("footer"))
                        ApplySubtitle(label);
                    else if (name.Contains("valor") || name.Contains("saldo") || name.Contains("total") || name.Contains("premio"))
                        label.Font = Valor;
                    else
                        ApplyBody(label);
                }
                else if (control is TextBox textBox)
                {
                    ApplyTextBox(textBox);
                }
                else if (control is ComboBox combo)
                {
                    ApplyCombo(combo);
                }
                else if (control is Button button)
                {
                    ApplyPrimaryButton(button, button.BackColor == Color.Empty ? Dorado : button.BackColor);
                }
                else if (control is DataGridView grid)
                {
                    ApplyDataGrid(grid);
                }

                if (control.HasChildren)
                    ApplyTypography(control);
            }
        }

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
            Padding originalPadding = button.Padding;
            Color originalColor = button.BackColor;
            Color hoverColor = Darken(originalColor, 0.15F);

            button.MouseEnter += (s, e) => AnimateButton(button, originalPadding + new Padding(2), hoverColor);
            button.MouseLeave += (s, e) => AnimateButton(button, originalPadding, originalColor);
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

        public static void ApplyRoundedRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0) return;
            using (GraphicsPath path = RoundedPath(new Rectangle(0, 0, control.Width, control.Height), radius))
            {
                control.Region = new Region(path);
            }
        }

        private static void AttachTextBoxFocusBorder(TextBox textBox)
        {
            textBox.Enter += (s, e) => textBox.Parent?.Invalidate();
            textBox.Leave += (s, e) => textBox.Parent?.Invalidate();
            if (textBox.Parent != null)
                textBox.Parent.Paint += (s, e) => DrawFocusedTextBoxBorder(e.Graphics, textBox);
            textBox.ParentChanged += (s, e) =>
            {
                if (textBox.Parent != null)
                    textBox.Parent.Paint += (sender, args) => DrawFocusedTextBoxBorder(args.Graphics, textBox);
            };
        }

        private static void DrawFocusedTextBoxBorder(Graphics graphics, TextBox textBox)
        {
            if (!textBox.Focused) return;
            Rectangle r = new Rectangle(textBox.Left - 1, textBox.Top - 1, textBox.Width + 1, textBox.Height + 1);
            using (Pen pen = new Pen(Dorado, 2))
                graphics.DrawRectangle(pen, r);
        }

        private static void AnimateButton(Button button, Padding targetPadding, Color targetColor)
        {
            Timer timer = new Timer { Interval = 30 };
            int ticks = 0;
            timer.Tick += (s, e) =>
            {
                ticks++;
                button.Padding = targetPadding;
                button.BackColor = targetColor;
                if (ticks >= 5)
                {
                    timer.Stop();
                    timer.Dispose();
                }
            };
            timer.Start();
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
