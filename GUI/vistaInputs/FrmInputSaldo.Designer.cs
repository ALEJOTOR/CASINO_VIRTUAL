namespace GUI
{
    partial class FrmInputSaldo
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.lblInfo     = new System.Windows.Forms.Label();
            this.txtSaldo    = new System.Windows.Forms.TextBox();
            this.btnAceptar  = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.lblInfo.Location = new System.Drawing.Point(12, 12);
            this.lblInfo.Name     = "lblInfo";
            this.lblInfo.Size     = new System.Drawing.Size(280, 70);
            this.lblInfo.Text     = "";

            this.txtSaldo.Location = new System.Drawing.Point(12, 90);
            this.txtSaldo.Name     = "txtSaldo";
            this.txtSaldo.Size     = new System.Drawing.Size(280, 23);
            this.txtSaldo.TabIndex = 0;

            this.btnAceptar.Location = new System.Drawing.Point(12, 124);
            this.btnAceptar.Name     = "btnAceptar";
            this.btnAceptar.Size     = new System.Drawing.Size(90, 28);
            this.btnAceptar.Text     = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);

            this.btnCancelar.Location = new System.Drawing.Point(112, 124);
            this.btnCancelar.Name     = "btnCancelar";
            this.btnCancelar.Size     = new System.Drawing.Size(90, 28);
            this.btnCancelar.Text     = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(308, 166);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.txtSaldo);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.Name            = "FrmInputSaldo";
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text            = "Modificar saldo";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.Label   lblInfo;
        private System.Windows.Forms.TextBox txtSaldo;
        private System.Windows.Forms.Button  btnAceptar;
        private System.Windows.Forms.Button  btnCancelar;
    }
}
