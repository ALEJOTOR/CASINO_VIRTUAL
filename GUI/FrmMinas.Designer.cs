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
            this.lblSaldo = new System.Windows.Forms.Label(); // NUEVO
            this.SuspendLayout();

            // lblApuesta
            this.lblApuesta.AutoSize = true;
            this.lblApuesta.Location = new System.Drawing.Point(12, 14);
            this.lblApuesta.Text = "Apuesta ($):";

            // txtApuesta
            this.txtApuesta.Location = new System.Drawing.Point(95, 11);
            this.txtApuesta.Size = new System.Drawing.Size(100, 22);

            // lblMinas
            this.lblMinas.AutoSize = true;
            this.lblMinas.Location = new System.Drawing.Point(210, 14);
            this.lblMinas.Text = "Minas (1-20):";

            // txtMinas
            this.txtMinas.Location = new System.Drawing.Point(305, 11);
            this.txtMinas.Size = new System.Drawing.Size(50, 22);
            this.txtMinas.Text = "5";

            // btnIniciar
            this.btnIniciar.Location = new System.Drawing.Point(12, 45);
            this.btnIniciar.Size = new System.Drawing.Size(90, 30);
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);

            // btnRetirar
            this.btnRetirar.Enabled = false;
            this.btnRetirar.Location = new System.Drawing.Point(112, 45);
            this.btnRetirar.Size = new System.Drawing.Size(150, 30);
            this.btnRetirar.Text = "Retirar ganancias";
            this.btnRetirar.UseVisualStyleBackColor = true;
            this.btnRetirar.Click += new System.EventHandler(this.btnRetirar_Click);

            // lblMultiplicador
            this.lblMultiplicador.AutoSize = true;
            this.lblMultiplicador.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMultiplicador.Location = new System.Drawing.Point(12, 86);
            this.lblMultiplicador.Text = "Multiplicador: x1.00";

            // lblEstado
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(200, 86);
            this.lblEstado.Text = "Configura y presiona Iniciar.";

            // lblSaldo — NUEVO
            // Muestra el saldo del usuario en tiempo real.
            // Se ubica en la segunda fila del encabezado, visible siempre.
            this.lblSaldo.AutoSize = true;
            this.lblSaldo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSaldo.Location = new System.Drawing.Point(12, 110);
            this.lblSaldo.Text = "Saldo disponible: $0.00";
            this.lblSaldo.ForeColor = System.Drawing.Color.DarkGreen;

            // FrmMinas
            // Cálculo del tamaño correcto:
            //   Ancho: xBase(12) + 5 celdas*(TAM68+SEP4) - SEP4 + margen(16) = 12+360-4+16 = 384 → 392 con borde
            //   Alto:  yBase(138) + 5 filas*(68+4) - SEP4 + margen(20) = 138+360-4+20 = 514 → 535 con borde
            //
            // AutoScaleMode.Dpi en lugar de Font evita que el formulario
            // reescale los controles estáticos a una resolución diferente
            // de la que usan las celdas dinámicas creadas en CrearTablero(),
            // lo que causaba que el tablero se viera cortado.
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(392, 535);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Controls.Add(this.lblApuesta);
            this.Controls.Add(this.txtApuesta);
            this.Controls.Add(this.lblMinas);
            this.Controls.Add(this.txtMinas);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.btnRetirar);
            this.Controls.Add(this.lblMultiplicador);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.lblSaldo);   // NUEVO
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