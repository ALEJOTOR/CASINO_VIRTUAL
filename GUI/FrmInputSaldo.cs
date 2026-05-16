using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmInputSaldo : Form
    {
        public decimal NuevoSaldo { get; private set; }

        public FrmInputSaldo(string username, decimal saldoActual)
        {
            InitializeComponent();
            lblInfo.Text  = $"Usuario: {username}\nSaldo actual: ${saldoActual:N2}\n\nIngrese el nuevo saldo:";
            txtSaldo.Text = saldoActual.ToString("F2");
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtSaldo.Text, out decimal val) || val < 0)
            { MessageBox.Show("Ingrese un monto valido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            NuevoSaldo   = val;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        { DialogResult = DialogResult.Cancel; this.Close(); }
    }
}
