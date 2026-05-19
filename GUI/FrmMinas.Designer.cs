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
            this.lblSaldo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblApuesta
            // 
            this.lblApuesta.AutoSize = true;
            this.lblApuesta.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApuesta.Location = new System.Drawing.Point(12, 14);
            this.lblApuesta.Name = "lblApuesta";
            this.lblApuesta.Size = new System.Drawing.Size(96, 20);
            this.lblApuesta.TabIndex = 0;
            this.lblApuesta.Text = "Apuesta ($):";
            // 
            // txtApuesta
            // 
            this.txtApuesta.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApuesta.Location = new System.Drawing.Point(114, 11);
            this.txtApuesta.Name = "txtApuesta";
            this.txtApuesta.Size = new System.Drawing.Size(147, 26);
            this.txtApuesta.TabIndex = 1;
            // 
            // lblMinas
            // 
            this.lblMinas.AutoSize = true;
            this.lblMinas.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinas.Location = new System.Drawing.Point(267, 14);
            this.lblMinas.Name = "lblMinas";
            this.lblMinas.Size = new System.Drawing.Size(101, 20);
            this.lblMinas.TabIndex = 2;
            this.lblMinas.Text = "Minas (1-20):";
            // 
            // txtMinas
            // 
            this.txtMinas.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMinas.Location = new System.Drawing.Point(374, 11);
            this.txtMinas.Name = "txtMinas";
            this.txtMinas.Size = new System.Drawing.Size(50, 26);
            this.txtMinas.TabIndex = 3;
            this.txtMinas.Text = "5";
            // 
            // btnIniciar
            // 
            this.btnIniciar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIniciar.Location = new System.Drawing.Point(12, 45);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(90, 30);
            this.btnIniciar.TabIndex = 4;
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // btnRetirar
            // 
            this.btnRetirar.Enabled = false;
            this.btnRetirar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRetirar.Location = new System.Drawing.Point(112, 45);
            this.btnRetirar.Name = "btnRetirar";
            this.btnRetirar.Size = new System.Drawing.Size(150, 30);
            this.btnRetirar.TabIndex = 5;
            this.btnRetirar.Text = "Retirar ganancias";
            this.btnRetirar.UseVisualStyleBackColor = true;
            this.btnRetirar.Click += new System.EventHandler(this.btnRetirar_Click);
            // 
            // lblMultiplicador
            // 
            this.lblMultiplicador.AutoSize = true;
            this.lblMultiplicador.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMultiplicador.Location = new System.Drawing.Point(12, 86);
            this.lblMultiplicador.Name = "lblMultiplicador";
            this.lblMultiplicador.Size = new System.Drawing.Size(143, 20);
            this.lblMultiplicador.TabIndex = 6;
            this.lblMultiplicador.Text = "Multiplicador: x1.00";
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstado.Location = new System.Drawing.Point(200, 86);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(204, 20);
            this.lblEstado.TabIndex = 7;
            this.lblEstado.Text = "Configura y presiona Iniciar.";
            // 
            // lblSaldo
            // 
            this.lblSaldo.AutoSize = true;
            this.lblSaldo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaldo.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblSaldo.Location = new System.Drawing.Point(12, 110);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(173, 20);
            this.lblSaldo.TabIndex = 8;
            this.lblSaldo.Text = "Saldo disponible: $0.00";
            // 
            // FrmMinas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(469, 535);
            this.Controls.Add(this.lblApuesta);
            this.Controls.Add(this.txtApuesta);
            this.Controls.Add(this.lblMinas);
            this.Controls.Add(this.txtMinas);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.btnRetirar);
            this.Controls.Add(this.lblMultiplicador);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.lblSaldo);
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
        private System.Windows.Forms.Label lblSaldo;   // NUEVO
    }
}