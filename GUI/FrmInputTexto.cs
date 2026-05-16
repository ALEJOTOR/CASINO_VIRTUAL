using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmInputTexto : Form
    {
        public string Valor { get; private set; }

        public FrmInputTexto(string titulo, string pregunta)
        {
            InitializeComponent();
            this.Text    = titulo;
            lblInfo.Text = pregunta;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        { Valor = txtValor.Text; DialogResult = DialogResult.OK; this.Close(); }

        private void btnCancelar_Click(object sender, EventArgs e)
        { DialogResult = DialogResult.Cancel; this.Close(); }
    }
}
