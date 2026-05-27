using System.Windows.Forms;

namespace GUI
{
    public partial class FrmInputTexto : Form
    {
        public string Valor { get; private set; }

        public FrmInputTexto(string titulo, string pregunta)
        {
            InitializeComponent();
            // Problema visual que resuelve: el input de texto comparte la estetica oscura y botones destacados del admin.
            AppTheme.ApplyForm(this);
            AppTheme.ApplySubtitle(lblInfo);
            AppTheme.ApplyTextBox(txtValor);
            AppTheme.ApplyPrimaryButton(btnAceptar);
            AppTheme.ApplyPrimaryButton(btnCancelar, AppTheme.BgHover);
            this.Text    = titulo;
            lblInfo.Text = pregunta;
        }

        private void btnAceptar_Click(object sender, System.EventArgs e)
        {
            Valor        = txtValor.Text;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
