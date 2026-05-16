namespace GUI
{
    partial class FrmCliente
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
            this.lblBienvenida    = new System.Windows.Forms.Label();
            this.lblSaldo         = new System.Windows.Forms.Label();
            this.lblMonto         = new System.Windows.Forms.Label();
            this.txtMonto         = new System.Windows.Forms.TextBox();
            this.btnDepositar     = new System.Windows.Forms.Button();
            this.btnJugarMinas    = new System.Windows.Forms.Button();
            this.btnCerrarSesion  = new System.Windows.Forms.Button();
            this.tabControl       = new System.Windows.Forms.TabControl();
            this.tabPartidas      = new System.Windows.Forms.TabPage();
            this.dgvPartidas      = new System.Windows.Forms.DataGridView();
            this.tabTransacciones = new System.Windows.Forms.TabPage();
            this.dgvTransacciones = new System.Windows.Forms.DataGridView();
            this.tabControl.SuspendLayout();
            this.tabPartidas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).BeginInit();
            this.tabTransacciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransacciones)).BeginInit();
            this.SuspendLayout();

            this.lblBienvenida.Font     = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblBienvenida.Location = new System.Drawing.Point(12, 12);
            this.lblBienvenida.Name     = "lblBienvenida";
            this.lblBienvenida.Size     = new System.Drawing.Size(420, 24);
            this.lblBienvenida.Text     = "Bienvenido";

            this.lblSaldo.Font     = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSaldo.Location = new System.Drawing.Point(12, 40);
            this.lblSaldo.Name     = "lblSaldo";
            this.lblSaldo.Size     = new System.Drawing.Size(280, 20);
            this.lblSaldo.Text     = "Saldo disponible: $0.00";

            this.lblMonto.AutoSize = true;
            this.lblMonto.Location = new System.Drawing.Point(12, 76);
            this.lblMonto.Name     = "lblMonto";
            this.lblMonto.Text     = "Monto a depositar:";

            this.txtMonto.Location = new System.Drawing.Point(145, 73);
            this.txtMonto.Name     = "txtMonto";
            this.txtMonto.Size     = new System.Drawing.Size(110, 23);
            this.txtMonto.TabIndex = 0;

            this.btnDepositar.Location = new System.Drawing.Point(265, 72);
            this.btnDepositar.Name     = "btnDepositar";
            this.btnDepositar.Size     = new System.Drawing.Size(90, 26);
            this.btnDepositar.TabIndex = 1;
            this.btnDepositar.Text     = "Depositar";
            this.btnDepositar.UseVisualStyleBackColor = true;
            this.btnDepositar.Click += new System.EventHandler(this.btnDepositar_Click);

            this.btnJugarMinas.Location = new System.Drawing.Point(12, 110);
            this.btnJugarMinas.Name     = "btnJugarMinas";
            this.btnJugarMinas.Size     = new System.Drawing.Size(135, 32);
            this.btnJugarMinas.TabIndex = 2;
            this.btnJugarMinas.Text     = "Jugar Minas";
            this.btnJugarMinas.UseVisualStyleBackColor = true;
            this.btnJugarMinas.Click += new System.EventHandler(this.btnJugarMinas_Click);

            this.btnCerrarSesion.Location = new System.Drawing.Point(530, 12);
            this.btnCerrarSesion.Name     = "btnCerrarSesion";
            this.btnCerrarSesion.Size     = new System.Drawing.Size(110, 28);
            this.btnCerrarSesion.TabIndex = 3;
            this.btnCerrarSesion.Text     = "Cerrar sesion";
            this.btnCerrarSesion.UseVisualStyleBackColor = true;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);

            this.tabControl.Controls.Add(this.tabPartidas);
            this.tabControl.Controls.Add(this.tabTransacciones);
            this.tabControl.Location      = new System.Drawing.Point(12, 152);
            this.tabControl.Name          = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size          = new System.Drawing.Size(630, 260);
            this.tabControl.TabIndex      = 4;

            this.tabPartidas.Controls.Add(this.dgvPartidas);
            this.tabPartidas.Location = new System.Drawing.Point(4, 24);
            this.tabPartidas.Name     = "tabPartidas";
            this.tabPartidas.Size     = new System.Drawing.Size(622, 232);
            this.tabPartidas.Text     = "Mis partidas";
            this.tabPartidas.UseVisualStyleBackColor = true;

            this.dgvPartidas.AllowUserToAddRows    = false;
            this.dgvPartidas.AllowUserToDeleteRows = false;
            this.dgvPartidas.AutoSizeColumnsMode   = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPartidas.Dock     = System.Windows.Forms.DockStyle.Fill;
            this.dgvPartidas.Name     = "dgvPartidas";
            this.dgvPartidas.ReadOnly = true;
            this.dgvPartidas.TabIndex = 0;

            this.tabTransacciones.Controls.Add(this.dgvTransacciones);
            this.tabTransacciones.Location = new System.Drawing.Point(4, 24);
            this.tabTransacciones.Name     = "tabTransacciones";
            this.tabTransacciones.Size     = new System.Drawing.Size(622, 232);
            this.tabTransacciones.Text     = "Mis transacciones";
            this.tabTransacciones.UseVisualStyleBackColor = true;

            this.dgvTransacciones.AllowUserToAddRows    = false;
            this.dgvTransacciones.AllowUserToDeleteRows = false;
            this.dgvTransacciones.AutoSizeColumnsMode   = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTransacciones.Dock     = System.Windows.Forms.DockStyle.Fill;
            this.dgvTransacciones.Name     = "dgvTransacciones";
            this.dgvTransacciones.ReadOnly = true;
            this.dgvTransacciones.TabIndex = 0;

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(654, 428);
            this.Controls.Add(this.lblBienvenida);
            this.Controls.Add(this.lblSaldo);
            this.Controls.Add(this.lblMonto);
            this.Controls.Add(this.txtMonto);
            this.Controls.Add(this.btnDepositar);
            this.Controls.Add(this.btnJugarMinas);
            this.Controls.Add(this.btnCerrarSesion);
            this.Controls.Add(this.tabControl);
            this.Name          = "FrmCliente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text          = "Casino Virtual - Cliente";
            this.tabControl.ResumeLayout(false);
            this.tabPartidas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).EndInit();
            this.tabTransacciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransacciones)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.Label        lblBienvenida;
        private System.Windows.Forms.Label        lblSaldo;
        private System.Windows.Forms.Label        lblMonto;
        private System.Windows.Forms.TextBox      txtMonto;
        private System.Windows.Forms.Button       btnDepositar;
        private System.Windows.Forms.Button       btnJugarMinas;
        private System.Windows.Forms.Button       btnCerrarSesion;
        private System.Windows.Forms.TabControl   tabControl;
        private System.Windows.Forms.TabPage      tabPartidas;
        private System.Windows.Forms.TabPage      tabTransacciones;
        private System.Windows.Forms.DataGridView dgvPartidas;
        private System.Windows.Forms.DataGridView dgvTransacciones;
    }
}
