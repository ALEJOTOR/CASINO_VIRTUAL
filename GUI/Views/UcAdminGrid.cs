using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class UcAdminGrid : UserControl
    {
        public UcAdminGrid()
        {
            InitializeComponent();
            // Problema visual que resuelve: las tablas administrativas quedan legibles y alineadas con la paleta del sistema.
            AppTheme.ApplyView(this);
            AppTheme.ApplyTitle(lblTitulo);
            AppTheme.ApplyDataGrid(dgvDatos);
        }

        public void Configurar(string titulo, string[] columnasVisibles, string[] encabezados)
        {
            lblTitulo.Text = titulo;
            for (int i = 0; i < columnasVisibles.Length && i < encabezados.Length; i++)
            {
                if (dgvDatos.Columns.Contains(columnasVisibles[i]))
                {
                    dgvDatos.Columns[columnasVisibles[i]].HeaderText = encabezados[i];
                }
            }
        }

        public void CargarDatos(object data)
        {
            dgvDatos.DataSource = null;
            dgvDatos.DataSource = data;
        }

        public void FormatearColumnas()
        {
            foreach (DataGridViewColumn col in dgvDatos.Columns)
            {
                if (col.ValueType == typeof(decimal))
                    col.DefaultCellStyle.Format = "C2";
                else if (col.ValueType == typeof(DateTime))
                    col.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                else if (col.ValueType == typeof(int))
                    col.DefaultCellStyle.Format = "N0";
            }
        }
    }
}
