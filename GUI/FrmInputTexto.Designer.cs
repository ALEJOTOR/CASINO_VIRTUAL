namespace GUI
{
    partial class FrmInputTexto
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.lblInfo     = new System.Windows.Forms.Label();
            this.txtValor    = new System.Windows.Forms.TextBox();
            this.btnAceptar  = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();

            var anchorLR = System.Windows.Forms.AnchorStyles.Left |
                           System.Windows.Forms.AnchorStyles.Right |
                           System.Windows.Forms.AnchorStyles.Top;

            this.lblInfo.Anchor   = anchorLR;
            this.lblInfo.Location = new System.Drawing.Point(12, 12);
            this.lblInfo.Size     = new System.Drawing.Size(290, 40);

            this.txtValor.Anchor       = anchorLR;
            this.txtValor.Location     = new System.Drawing.Point(12, 58);
            this.txtValor.PasswordChar = '*';
            this.txtValor.Size         = new System.Drawing.Size(290, 22);

            this.btnAceptar.Anchor   = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.btnAceptar.Location = new System.Drawing.Point(12, 94);
            this.btnAceptar.Size     = new System.Drawing.Size(95, 30);
            this.btnAceptar.Text     = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);

            this.btnCancelar.Anchor   = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnCancelar.Location = new System.Drawing.Point(207, 94);
            this.btnCancelar.Size     = new System.Drawing.Size(95, 30);
            this.btnCancelar.Text     = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(316, 138);
            this.MinimumSize         = new System.Drawing.Size(280, 165);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.txtValor);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.Name            = "FrmInputTexto";
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text            = "Entrada";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.Label   lblInfo;
        private System.Windows.Forms.TextBox txtValor;
        private System.Windows.Forms.Button  btnAceptar;
        private System.Windows.Forms.Button  btnCancelar;
    }
}
