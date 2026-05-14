namespace GUI
{
    partial class FrmCliente
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
            this.lblBienvenida   = new System.Windows.Forms.Label();
            this.lblSaldo        = new System.Windows.Forms.Label();
            this.lblMontoDeposito = new System.Windows.Forms.Label();
            this.txtMonto        = new System.Windows.Forms.TextBox();
            this.btnDepositar    = new System.Windows.Forms.Button();
            this.btnJugarMinas   = new System.Windows.Forms.Button();
            this.btnCerrarSesion = new System.Windows.Forms.Button();
            this.lblHistorial    = new System.Windows.Forms.Label();
            this.dgvPartidas     = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).BeginInit();
            this.SuspendLayout();

            // lblBienvenida
            this.lblBienvenida.AutoSize = true;
            this.lblBienvenida.Font     = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblBienvenida.Location = new System.Drawing.Point(12, 12);
            this.lblBienvenida.Name     = "lblBienvenida";
            this.lblBienvenida.Size     = new System.Drawing.Size(300, 21);
            this.lblBienvenida.Text     = "Bienvenido";

            // lblSaldo
            this.lblSaldo.AutoSize = true;
            this.lblSaldo.Font     = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSaldo.Location = new System.Drawing.Point(12, 42);
            this.lblSaldo.Name     = "lblSaldo";
            this.lblSaldo.Size     = new System.Drawing.Size(150, 19);
            this.lblSaldo.Text     = "Saldo: $0.00";

            // lblMontoDeposito
            this.lblMontoDeposito.AutoSize = true;
            this.lblMontoDeposito.Location = new System.Drawing.Point(12, 78);
            this.lblMontoDeposito.Name     = "lblMontoDeposito";
            this.lblMontoDeposito.Text     = "Monto a depositar:";

            // txtMonto
            this.txtMonto.Location = new System.Drawing.Point(135, 75);
            this.txtMonto.Name     = "txtMonto";
            this.txtMonto.Size     = new System.Drawing.Size(120, 23);
            this.txtMonto.TabIndex = 0;

            // btnDepositar
            this.btnDepositar.Location = new System.Drawing.Point(265, 74);
            this.btnDepositar.Name     = "btnDepositar";
            this.btnDepositar.Size     = new System.Drawing.Size(90, 25);
            this.btnDepositar.TabIndex = 1;
            this.btnDepositar.Text     = "Depositar";
            this.btnDepositar.UseVisualStyleBackColor = true;
            this.btnDepositar.Click += new System.EventHandler(this.btnDepositar_Click);

            // btnJugarMinas
            this.btnJugarMinas.Location = new System.Drawing.Point(12, 112);
            this.btnJugarMinas.Name     = "btnJugarMinas";
            this.btnJugarMinas.Size     = new System.Drawing.Size(130, 32);
            this.btnJugarMinas.TabIndex = 2;
            this.btnJugarMinas.Text     = "Jugar Minas";
            this.btnJugarMinas.UseVisualStyleBackColor = true;
            this.btnJugarMinas.Click += new System.EventHandler(this.btnJugarMinas_Click);

            // btnCerrarSesion
            this.btnCerrarSesion.Location = new System.Drawing.Point(490, 12);
            this.btnCerrarSesion.Name     = "btnCerrarSesion";
            this.btnCerrarSesion.Size     = new System.Drawing.Size(110, 28);
            this.btnCerrarSesion.TabIndex = 3;
            this.btnCerrarSesion.Text     = "Cerrar sesión";
            this.btnCerrarSesion.UseVisualStyleBackColor = true;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);

            // lblHistorial
            this.lblHistorial.AutoSize = true;
            this.lblHistorial.Location = new System.Drawing.Point(12, 158);
            this.lblHistorial.Name     = "lblHistorial";
            this.lblHistorial.Text     = "Historial de partidas:";

            // dgvPartidas
            this.dgvPartidas.AllowUserToAddRows    = false;
            this.dgvPartidas.AllowUserToDeleteRows = false;
            this.dgvPartidas.AutoSizeColumnsMode   = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPartidas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPartidas.Location  = new System.Drawing.Point(12, 178);
            this.dgvPartidas.Name      = "dgvPartidas";
            this.dgvPartidas.ReadOnly  = true;
            this.dgvPartidas.Size      = new System.Drawing.Size(590, 220);
            this.dgvPartidas.TabIndex  = 4;

            // FrmCliente
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(614, 414);
            this.Controls.Add(this.lblBienvenida);
            this.Controls.Add(this.lblSaldo);
            this.Controls.Add(this.lblMontoDeposito);
            this.Controls.Add(this.txtMonto);
            this.Controls.Add(this.btnDepositar);
            this.Controls.Add(this.btnJugarMinas);
            this.Controls.Add(this.btnCerrarSesion);
            this.Controls.Add(this.lblHistorial);
            this.Controls.Add(this.dgvPartidas);
            this.Name          = "FrmCliente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text          = "Casino Virtual - Cliente";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label       lblBienvenida;
        private System.Windows.Forms.Label       lblSaldo;
        private System.Windows.Forms.Label       lblMontoDeposito;
        private System.Windows.Forms.Label       lblHistorial;
        private System.Windows.Forms.TextBox     txtMonto;
        private System.Windows.Forms.Button      btnDepositar;
        private System.Windows.Forms.Button      btnJugarMinas;
        private System.Windows.Forms.Button      btnCerrarSesion;
        private System.Windows.Forms.DataGridView dgvPartidas;
    }
}
