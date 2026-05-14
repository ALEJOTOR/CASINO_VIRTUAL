namespace GUI
{
    partial class FrmMinas
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
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

            // lblApuesta
            this.lblApuesta.AutoSize = true;
            this.lblApuesta.Location = new System.Drawing.Point(10, 14);
            this.lblApuesta.Name     = "lblApuesta";
            this.lblApuesta.Text     = "Apuesta:";

            // txtApuesta
            this.txtApuesta.Location = new System.Drawing.Point(80, 11);
            this.txtApuesta.Name     = "txtApuesta";
            this.txtApuesta.Size     = new System.Drawing.Size(100, 23);
            this.txtApuesta.TabIndex = 0;

            // lblMinas
            this.lblMinas.AutoSize = true;
            this.lblMinas.Location = new System.Drawing.Point(195, 14);
            this.lblMinas.Name     = "lblMinas";
            this.lblMinas.Text     = "Nº Minas (1-20):";

            // txtMinas
            this.txtMinas.Location = new System.Drawing.Point(310, 11);
            this.txtMinas.Name     = "txtMinas";
            this.txtMinas.Size     = new System.Drawing.Size(50, 23);
            this.txtMinas.TabIndex = 1;
            this.txtMinas.Text     = "5";

            // btnIniciar
            this.btnIniciar.Location = new System.Drawing.Point(10, 44);
            this.btnIniciar.Name     = "btnIniciar";
            this.btnIniciar.Size     = new System.Drawing.Size(90, 30);
            this.btnIniciar.TabIndex = 2;
            this.btnIniciar.Text     = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);

            // btnRetirar
            this.btnRetirar.Enabled  = false;
            this.btnRetirar.Location = new System.Drawing.Point(110, 44);
            this.btnRetirar.Name     = "btnRetirar";
            this.btnRetirar.Size     = new System.Drawing.Size(100, 30);
            this.btnRetirar.TabIndex = 3;
            this.btnRetirar.Text     = "Retirar ganancias";
            this.btnRetirar.UseVisualStyleBackColor = true;
            this.btnRetirar.Click += new System.EventHandler(this.btnRetirar_Click);

            // lblMultiplicador
            this.lblMultiplicador.AutoSize = true;
            this.lblMultiplicador.Font     = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMultiplicador.Location = new System.Drawing.Point(10, 82);
            this.lblMultiplicador.Name     = "lblMultiplicador";
            this.lblMultiplicador.Text     = "Multiplicador: x1.00";

            // lblEstado
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(200, 82);
            this.lblEstado.Name     = "lblEstado";
            this.lblEstado.Size     = new System.Drawing.Size(200, 15);
            this.lblEstado.Text     = "Configura y presiona Iniciar.";

            // FrmMinas
            // Nota: el tablero de botones se crea dinámicamente en CrearTablero()
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(355, 460);
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
