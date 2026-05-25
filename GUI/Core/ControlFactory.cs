using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    internal static class ControlFactory
    {
        public static Label CrearLabel(string texto, Color color, Font fuente, ContentAlignment alineacion)
        {
            return new Label
            {
                AutoSize = false,
                BackColor = Color.Transparent,
                Font = fuente,
                ForeColor = color,
                Text = texto,
                TextAlign = alineacion
            };
        }

        public static Label CrearBadge(string texto)
        {
            return new Label
            {
                AutoSize = false,
                BackColor = Color.FromArgb(30, 41, 59),
                Font = CasinoTheme.UiFont(8.5F, FontStyle.Bold),
                ForeColor = CasinoTheme.Cyan,
                Text = texto,
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        public static DataGridView CrearGridHistorial()
        {
            DataGridView grid = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = CasinoTheme.Page,
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = CasinoTheme.SurfaceAlt;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = CasinoTheme.Text;
            grid.ColumnHeadersDefaultCellStyle.Font = CasinoTheme.UiFont(10F, FontStyle.Bold);
            grid.DefaultCellStyle.BackColor = CasinoTheme.Surface;
            grid.DefaultCellStyle.ForeColor = CasinoTheme.Text;
            grid.DefaultCellStyle.SelectionBackColor = CasinoTheme.Blue;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(13, 23, 42);
            grid.GridColor = CasinoTheme.Border;
            return grid;
        }

        public static Button CrearBotonLateral(string texto)
        {
            Button boton = new Button
            {
                BackColor = CasinoTheme.Surface,
                Dock = DockStyle.Top,
                FlatStyle = FlatStyle.Flat,
                Font = CasinoTheme.UiFont(10F, FontStyle.Bold),
                ForeColor = CasinoTheme.Text,
                Height = 48,
                Text = texto,
                TextAlign = ContentAlignment.MiddleLeft,
                UseVisualStyleBackColor = false
            };
            boton.FlatAppearance.BorderSize = 0;
            boton.Padding = new Padding(18, 0, 0, 0);
            return boton;
        }

        public static Button CrearChipPeriodo(string texto, bool activo)
        {
            Button chip = new Button
            {
                BackColor = activo ? CasinoTheme.SurfaceAlt : Color.FromArgb(14, 27, 43),
                FlatStyle = FlatStyle.Flat,
                Font = CasinoTheme.UiFont(9.5F, FontStyle.Bold),
                ForeColor = CasinoTheme.Text,
                Height = 40,
                Width = 96,
                Text = texto,
                UseVisualStyleBackColor = false
            };
            chip.FlatAppearance.BorderSize = 0;
            chip.Margin = new Padding(0, 0, 10, 0);
            return chip;
        }

        public static TextBox CrearInputFecha()
        {
            return new TextBox
            {
                BackColor = Color.FromArgb(8, 20, 34),
                BorderStyle = BorderStyle.FixedSingle,
                Font = CasinoTheme.UiFont(10F, FontStyle.Bold),
                ForeColor = CasinoTheme.Text
            };
        }

        public static ComboBox CrearComboFiltro()
        {
            ComboBox combo = new ComboBox
            {
                BackColor = Color.FromArgb(8, 20, 34),
                DrawMode = DrawMode.OwnerDrawFixed,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Font = CasinoTheme.UiFont(10F, FontStyle.Bold),
                ForeColor = CasinoTheme.Text,
                ItemHeight = 28
            };
            combo.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;
                bool seleccionado = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                using (SolidBrush fondo = new SolidBrush(seleccionado ? CasinoTheme.SurfaceAlt : Color.FromArgb(8, 20, 34)))
                    e.Graphics.FillRectangle(fondo, e.Bounds);
                using (SolidBrush texto = new SolidBrush(CasinoTheme.Text))
                    e.Graphics.DrawString(combo.Items[e.Index].ToString(), combo.Font, texto, e.Bounds.X + 8, e.Bounds.Y + 6);
            };
            return combo;
        }
    }
}
