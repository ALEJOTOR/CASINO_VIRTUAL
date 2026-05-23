using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GUI
{
    internal static class CasinoTheme
    {
        public static readonly Color Page = Color.FromArgb(8, 13, 24);
        public static readonly Color Header = Color.FromArgb(10, 16, 30);
        public static readonly Color Surface = Color.FromArgb(17, 28, 50);
        public static readonly Color SurfaceAlt = Color.FromArgb(23, 37, 63);
        public static readonly Color Border = Color.FromArgb(51, 65, 85);
        public static readonly Color Text = Color.FromArgb(241, 245, 249);
        public static readonly Color Muted = Color.FromArgb(203, 213, 225);
        public static readonly Color Gold = Color.FromArgb(250, 204, 21);
        public static readonly Color Green = Color.FromArgb(34, 197, 94);
        public static readonly Color Red = Color.FromArgb(220, 38, 38);
        public static readonly Color Blue = Color.FromArgb(37, 99, 235);
        public static readonly Color Purple = Color.FromArgb(126, 34, 206);
        public static readonly Color Cyan = Color.FromArgb(56, 189, 248);

        public static Font TitleFont(float size)
        {
            return new Font("Georgia", size, FontStyle.Bold);
        }

        public static Font UiFont(float size, FontStyle style = FontStyle.Regular)
        {
            return new Font("Segoe UI", size, style);
        }

        public static void StylePage(Control control)
        {
            control.BackColor = Page;
            control.Font = UiFont(9F);
        }

        public static void StyleHeader(Panel panel)
        {
            panel.BackColor = Header;
        }

        public static void StyleTitle(Label label, float size = 25F)
        {
            label.Font = TitleFont(size);
            label.ForeColor = Gold;
        }

        public static void StyleLabel(Label label, Color? color = null, float size = 10F, FontStyle style = FontStyle.Bold)
        {
            label.Font = UiFont(size, style);
            label.ForeColor = color ?? Text;
        }

        public static void StyleInput(TextBox textBox)
        {
            textBox.BackColor = Color.FromArgb(248, 250, 252);
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = UiFont(11F);
        }

        public static void StyleActionButton(Button button, Color color)
        {
            button.BackColor = color;
            button.Cursor = Cursors.Hand;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = UiFont(10F, FontStyle.Bold);
            button.ForeColor = Color.White;
            button.UseVisualStyleBackColor = false;
        }

        public static void StyleSecondaryButton(Button button)
        {
            StyleActionButton(button, Color.FromArgb(51, 65, 85));
            button.ForeColor = Text;
        }

        public static void DrawBorderedPanel(Graphics graphics, Rectangle area, Color fill, Color border)
        {
            using (SolidBrush brush = new SolidBrush(fill))
                graphics.FillRectangle(brush, area);

            using (Pen pen = new Pen(border, 1))
                graphics.DrawRectangle(pen, area.X, area.Y, area.Width - 1, area.Height - 1);
        }

        public static void DrawSubtleGradient(Graphics graphics, Rectangle area)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                area,
                Color.FromArgb(12, 20, 38),
                Color.FromArgb(8, 13, 24),
                25F))
            {
                graphics.FillRectangle(brush, area);
            }
        }
    }
}
