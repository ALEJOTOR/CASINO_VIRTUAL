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
            this.lblApuesta = new System.Windows.Forms.Label();
            this.txtApuesta = new System.Windows.Forms.TextBox();
            this.lblMinas = new System.Windows.Forms.Label();
            this.txtMinas = new System.Windows.Forms.TextBox();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.btnRetirar = new System.Windows.Forms.Button();
            this.lblMultiplicador = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // lblApuesta
            this.lblApuesta.AutoSize = true;
            this.lblApuesta.Location = new System.Drawing.Point(12, 14);
            this.lblApuesta.Name = "lblApuesta";
            this.lblApuesta.TabIndex = 0;
            this.lblApuesta.Text = "Apuesta:";
            // txtApuesta
            this.txtApuesta.Location = new System.Drawing.Point(82, 11);
            this.txtApuesta.Name = "txtApuesta";
            this.txtApuesta.Size = new System.Drawing.Size(100, 20);
            this.txtApuesta.TabIndex = 1;
            // lblMinas
            this.lblMinas.AutoSize = true;
            this.lblMinas.Location = new System.Drawing.Point(198, 14);
            this.lblMinas.Name = "lblMinas";
            this.lblMinas.TabIndex = 2;
            this.lblMinas.Text = "Minas (1-20):";
            // txtMinas
            this.txtMinas.Location = new System.Drawing.Point(295, 11);
            this.txtMinas.Name = "txtMinas";
            this.txtMinas.Size = new System.Drawing.Size(48, 20);
            this.txtMinas.TabIndex = 3;
            this.txtMinas.Text = "5";
            // btnIniciar
            this.btnIniciar.Location = new System.Drawing.Point(12, 45);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(90, 30);
            this.btnIniciar.TabIndex = 4;
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // btnRetirar
            this.btnRetirar.Enabled = false;
            this.btnRetirar.Location = new System.Drawing.Point(112, 45);
            this.btnRetirar.Name = "btnRetirar";
            this.btnRetirar.Size = new System.Drawing.Size(140, 30);
            this.btnRetirar.TabIndex = 5;
            this.btnRetirar.Text = "Retirar ganancias";
            this.btnRetirar.UseVisualStyleBackColor = true;
            this.btnRetirar.Click += new System.EventHandler(this.btnRetirar_Click);
            // lblMultiplicador
            this.lblMultiplicador.AutoSize = true;
            this.lblMultiplicador.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMultiplicador.Location = new System.Drawing.Point(12, 86);
            this.lblMultiplicador.Name = "lblMultiplicador";
            this.lblMultiplicador.TabIndex = 6;
            this.lblMultiplicador.Text = "Multiplicador: x1.00";
            // lblEstado
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(200, 86);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.TabIndex = 7;
            this.lblEstado.Text = "Configura y presiona Iniciar.";
            // FrmMinas
            // Tablero: 5 botones x (68+4)=72px = 360px ancho, desde x=12 → total 384 → 390
            // Alto: yBase=118, 5x72=360, +20 margen → 498 → 510
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 510);
            this.Controls.Add(this.lblApuesta);
            this.Controls.Add(this.txtApuesta);
            this.Controls.Add(this.lblMinas);
            this.Controls.Add(this.txtMinas);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.btnRetirar);
            this.Controls.Add(this.lblMultiplicador);
            this.Controls.Add(this.lblEstado);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmMinas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Juego - Minas";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.Label lblApuesta;
        private System.Windows.Forms.TextBox txtApuesta;
        private System.Windows.Forms.Label lblMinas;
        private System.Windows.Forms.TextBox txtMinas;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Button btnRetirar;
        private System.Windows.Forms.Label lblMultiplicador;
        private System.Windows.Forms.Label lblEstado;
    }
}
