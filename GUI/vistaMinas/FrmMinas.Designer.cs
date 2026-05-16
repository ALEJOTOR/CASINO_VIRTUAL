namespace GUI
{
    partial class FrmMinas
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.lblApuesta       = new System.Windows.Forms.Label();
            this.txtApuesta       = new System.Windows.Forms.TextBox();
            this.lblMinas         = new System.Windows.Forms.Label();
            this.txtMinas         = new System.Windows.Forms.TextBox();
            this.btnIniciar       = new System.Windows.Forms.Button();
            this.btnRetirar       = new System.Windows.Forms.Button();
            this.lblMultiplicador = new System.Windows.Forms.Label();
            this.lblEstado        = new System.Windows.Forms.Label();
            this.SuspendLayout();

            this.lblApuesta.AutoSize = true;
            this.lblApuesta.Location = new System.Drawing.Point(12, 14);
            this.lblApuesta.Name     = "lblApuesta";
            this.lblApuesta.Text     = "Apuesta:";

            this.txtApuesta.Location = new System.Drawing.Point(82, 11);
            this.txtApuesta.Name     = "txtApuesta";
            this.txtApuesta.Size     = new System.Drawing.Size(100, 23);
            this.txtApuesta.TabIndex = 0;

            this.lblMinas.AutoSize = true;
            this.lblMinas.Location = new System.Drawing.Point(198, 14);
            this.lblMinas.Name     = "lblMinas";
            this.lblMinas.Text     = "Minas (1-20):";

            this.txtMinas.Location = new System.Drawing.Point(295, 11);
            this.txtMinas.Name     = "txtMinas";
            this.txtMinas.Size     = new System.Drawing.Size(48, 23);
            this.txtMinas.TabIndex = 1;
            this.txtMinas.Text     = "5";

            this.btnIniciar.Location = new System.Drawing.Point(12, 45);
            this.btnIniciar.Name     = "btnIniciar";
            this.btnIniciar.Size     = new System.Drawing.Size(90, 30);
            this.btnIniciar.TabIndex = 2;
            this.btnIniciar.Text     = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);

            this.btnRetirar.Enabled  = false;
            this.btnRetirar.Location = new System.Drawing.Point(112, 45);
            this.btnRetirar.Name     = "btnRetirar";
            this.btnRetirar.Size     = new System.Drawing.Size(140, 30);
            this.btnRetirar.TabIndex = 3;
            this.btnRetirar.Text     = "Retirar ganancias";
            this.btnRetirar.UseVisualStyleBackColor = true;
            this.btnRetirar.Click += new System.EventHandler(this.btnRetirar_Click);

            this.lblMultiplicador.AutoSize = true;
            this.lblMultiplicador.Font     = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMultiplicador.Location = new System.Drawing.Point(12, 86);
            this.lblMultiplicador.Name     = "lblMultiplicador";
            this.lblMultiplicador.Text     = "Multiplicador: x1.00";

            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(200, 86);
            this.lblEstado.Name     = "lblEstado";
            this.lblEstado.Text     = "Configura y presiona Iniciar.";

            // Tablero 5x5: cada boton 68px + 4px sep = 72px por celda
            // 5 * 72 = 360px ancho desde x=12 → 12+360+12=384 → usar 390
            // Alto: yBase=118, 5*72=360, +12 margen = 490 → usar 500
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(390, 500);
            this.Controls.Add(this.lblApuesta);
            this.Controls.Add(this.txtApuesta);
            this.Controls.Add(this.lblMinas);
            this.Controls.Add(this.txtMinas);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.btnRetirar);
            this.Controls.Add(this.lblMultiplicador);
            this.Controls.Add(this.lblEstado);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.Name            = "FrmMinas";
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text            = "Juego - Minas";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.Label   lblApuesta;
        private System.Windows.Forms.TextBox txtApuesta;
        private System.Windows.Forms.Label   lblMinas;
        private System.Windows.Forms.TextBox txtMinas;
        private System.Windows.Forms.Button  btnIniciar;
        private System.Windows.Forms.Button  btnRetirar;
        private System.Windows.Forms.Label   lblMultiplicador;
        private System.Windows.Forms.Label   lblEstado;
    }
}
