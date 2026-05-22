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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblBienvenida = new System.Windows.Forms.Label();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.btnCerrarSesion = new System.Windows.Forms.Button();
            this.pnlAcciones = new System.Windows.Forms.Panel();
            this.lblMonto = new System.Windows.Forms.Label();
            this.txtMonto = new System.Windows.Forms.TextBox();
            this.btnDepositar = new System.Windows.Forms.Button();
            this.btnJugarMinas = new System.Windows.Forms.Button();
            this.btnJugarRuleta = new System.Windows.Forms.Button();
            this.btnJugarSlot = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPartidas = new System.Windows.Forms.TabPage();
            this.dgvPartidas = new System.Windows.Forms.DataGridView();
            this.tabTransacciones = new System.Windows.Forms.TabPage();
            this.dgvTransacciones = new System.Windows.Forms.DataGridView();
            this.pnlTop.SuspendLayout();
            this.pnlAcciones.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPartidas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).BeginInit();
            this.tabTransacciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransacciones)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(12, 18, 32);
            this.pnlTop.Controls.Add(this.lblBienvenida);
            this.pnlTop.Controls.Add(this.lblSaldo);
            this.pnlTop.Controls.Add(this.btnCerrarSesion);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(780, 72);
            this.pnlTop.TabIndex = 0;
            // 
            // lblBienvenida
            // 
            this.lblBienvenida.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblBienvenida.ForeColor = System.Drawing.Color.Gold;
            this.lblBienvenida.Location = new System.Drawing.Point(14, 10);
            this.lblBienvenida.Name = "lblBienvenida";
            this.lblBienvenida.Size = new System.Drawing.Size(440, 28);
            this.lblBienvenida.TabIndex = 0;
            this.lblBienvenida.Text = "Bienvenido";
            // 
            // lblSaldo
            // 
            this.lblSaldo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSaldo.ForeColor = System.Drawing.Color.FromArgb(74, 222, 128);
            this.lblSaldo.Location = new System.Drawing.Point(15, 40);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(330, 23);
            this.lblSaldo.TabIndex = 1;
            this.lblSaldo.Text = "Saldo disponible: $0.00";
            // 
            // btnCerrarSesion
            // 
            this.btnCerrarSesion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrarSesion.BackColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this.btnCerrarSesion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrarSesion.FlatAppearance.BorderSize = 0;
            this.btnCerrarSesion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarSesion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCerrarSesion.ForeColor = System.Drawing.Color.White;
            this.btnCerrarSesion.Location = new System.Drawing.Point(635, 20);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Size = new System.Drawing.Size(125, 32);
            this.btnCerrarSesion.TabIndex = 2;
            this.btnCerrarSesion.Text = "Cerrar sesion";
            this.btnCerrarSesion.UseVisualStyleBackColor = false;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);
            // 
            // pnlAcciones
            // 
            this.pnlAcciones.BackColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.pnlAcciones.Controls.Add(this.lblMonto);
            this.pnlAcciones.Controls.Add(this.txtMonto);
            this.pnlAcciones.Controls.Add(this.btnDepositar);
            this.pnlAcciones.Controls.Add(this.btnJugarMinas);
            this.pnlAcciones.Controls.Add(this.btnJugarRuleta);
            this.pnlAcciones.Controls.Add(this.btnJugarSlot);
            this.pnlAcciones.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAcciones.Location = new System.Drawing.Point(0, 72);
            this.pnlAcciones.Name = "pnlAcciones";
            this.pnlAcciones.Size = new System.Drawing.Size(780, 62);
            this.pnlAcciones.TabIndex = 1;
            // 
            // lblMonto
            // 
            this.lblMonto.AutoSize = true;
            this.lblMonto.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMonto.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMonto.Location = new System.Drawing.Point(15, 21);
            this.lblMonto.Name = "lblMonto";
            this.lblMonto.Size = new System.Drawing.Size(134, 19);
            this.lblMonto.TabIndex = 0;
            this.lblMonto.Text = "Monto a depositar:";
            // 
            // txtMonto
            // 
            this.txtMonto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMonto.Location = new System.Drawing.Point(155, 18);
            this.txtMonto.Name = "txtMonto";
            this.txtMonto.Size = new System.Drawing.Size(110, 25);
            this.txtMonto.TabIndex = 1;
            // 
            // btnDepositar
            // 
            this.btnDepositar.BackColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this.btnDepositar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDepositar.FlatAppearance.BorderSize = 0;
            this.btnDepositar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDepositar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDepositar.ForeColor = System.Drawing.Color.White;
            this.btnDepositar.Location = new System.Drawing.Point(276, 17);
            this.btnDepositar.Name = "btnDepositar";
            this.btnDepositar.Size = new System.Drawing.Size(96, 28);
            this.btnDepositar.TabIndex = 2;
            this.btnDepositar.Text = "Depositar";
            this.btnDepositar.UseVisualStyleBackColor = false;
            this.btnDepositar.Click += new System.EventHandler(this.btnDepositar_Click);
            // 
            // btnJugarMinas
            // 
            this.btnJugarMinas.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnJugarMinas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnJugarMinas.FlatAppearance.BorderSize = 0;
            this.btnJugarMinas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJugarMinas.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnJugarMinas.ForeColor = System.Drawing.Color.White;
            this.btnJugarMinas.Location = new System.Drawing.Point(394, 17);
            this.btnJugarMinas.Name = "btnJugarMinas";
            this.btnJugarMinas.Size = new System.Drawing.Size(82, 28);
            this.btnJugarMinas.TabIndex = 3;
            this.btnJugarMinas.Text = "Minas";
            this.btnJugarMinas.UseVisualStyleBackColor = false;
            this.btnJugarMinas.Click += new System.EventHandler(this.btnJugarMinas_Click);
            // 
            // btnJugarRuleta
            // 
            this.btnJugarRuleta.BackColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this.btnJugarRuleta.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnJugarRuleta.FlatAppearance.BorderSize = 0;
            this.btnJugarRuleta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJugarRuleta.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnJugarRuleta.ForeColor = System.Drawing.Color.White;
            this.btnJugarRuleta.Location = new System.Drawing.Point(486, 17);
            this.btnJugarRuleta.Name = "btnJugarRuleta";
            this.btnJugarRuleta.Size = new System.Drawing.Size(82, 28);
            this.btnJugarRuleta.TabIndex = 4;
            this.btnJugarRuleta.Text = "Ruleta";
            this.btnJugarRuleta.UseVisualStyleBackColor = false;
            this.btnJugarRuleta.Click += new System.EventHandler(this.btnJugarRuleta_Click);
            // 
            // btnJugarSlot
            // 
            this.btnJugarSlot.BackColor = System.Drawing.Color.FromArgb(124, 58, 237);
            this.btnJugarSlot.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnJugarSlot.FlatAppearance.BorderSize = 0;
            this.btnJugarSlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJugarSlot.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnJugarSlot.ForeColor = System.Drawing.Color.White;
            this.btnJugarSlot.Location = new System.Drawing.Point(578, 17);
            this.btnJugarSlot.Name = "btnJugarSlot";
            this.btnJugarSlot.Size = new System.Drawing.Size(82, 28);
            this.btnJugarSlot.TabIndex = 5;
            this.btnJugarSlot.Text = "Slots";
            this.btnJugarSlot.UseVisualStyleBackColor = false;
            this.btnJugarSlot.Click += new System.EventHandler(this.btnJugarSlot_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPartidas);
            this.tabControl.Controls.Add(this.tabTransacciones);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControl.Location = new System.Drawing.Point(0, 134);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(780, 386);
            this.tabControl.TabIndex = 2;
            // 
            // tabPartidas
            // 
            this.tabPartidas.BackColor = System.Drawing.Color.White;
            this.tabPartidas.Controls.Add(this.dgvPartidas);
            this.tabPartidas.Location = new System.Drawing.Point(4, 26);
            this.tabPartidas.Name = "tabPartidas";
            this.tabPartidas.Padding = new System.Windows.Forms.Padding(3);
            this.tabPartidas.Size = new System.Drawing.Size(772, 356);
            this.tabPartidas.TabIndex = 0;
            this.tabPartidas.Text = "Mis partidas";
            // 
            // dgvPartidas
            // 
            this.dgvPartidas.AllowUserToAddRows = false;
            this.dgvPartidas.AllowUserToDeleteRows = false;
            this.dgvPartidas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPartidas.BackgroundColor = System.Drawing.Color.White;
            this.dgvPartidas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPartidas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPartidas.ReadOnly = true;
            this.dgvPartidas.RowHeadersVisible = false;
            this.dgvPartidas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // tabTransacciones
            // 
            this.tabTransacciones.BackColor = System.Drawing.Color.White;
            this.tabTransacciones.Controls.Add(this.dgvTransacciones);
            this.tabTransacciones.Location = new System.Drawing.Point(4, 26);
            this.tabTransacciones.Name = "tabTransacciones";
            this.tabTransacciones.Padding = new System.Windows.Forms.Padding(3);
            this.tabTransacciones.Size = new System.Drawing.Size(772, 356);
            this.tabTransacciones.TabIndex = 1;
            this.tabTransacciones.Text = "Mis transacciones";
            // 
            // dgvTransacciones
            // 
            this.dgvTransacciones.AllowUserToAddRows = false;
            this.dgvTransacciones.AllowUserToDeleteRows = false;
            this.dgvTransacciones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTransacciones.BackgroundColor = System.Drawing.Color.White;
            this.dgvTransacciones.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTransacciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTransacciones.ReadOnly = true;
            this.dgvTransacciones.RowHeadersVisible = false;
            this.dgvTransacciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // FrmCliente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.ClientSize = new System.Drawing.Size(780, 520);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlAcciones);
            this.Controls.Add(this.pnlTop);
            this.MinimumSize = new System.Drawing.Size(720, 420);
            this.Name = "FrmCliente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Casino Virtual - Cliente";
            this.pnlTop.ResumeLayout(false);
            this.pnlAcciones.ResumeLayout(false);
            this.pnlAcciones.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPartidas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).EndInit();
            this.tabTransacciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransacciones)).EndInit();
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlAcciones;
        private System.Windows.Forms.Label lblBienvenida;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Label lblMonto;
        private System.Windows.Forms.TextBox txtMonto;
        private System.Windows.Forms.Button btnDepositar;
        private System.Windows.Forms.Button btnJugarMinas;
        private System.Windows.Forms.Button btnJugarRuleta;
        private System.Windows.Forms.Button btnJugarSlot;
        private System.Windows.Forms.Button btnCerrarSesion;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPartidas;
        private System.Windows.Forms.TabPage tabTransacciones;
        private System.Windows.Forms.DataGridView dgvPartidas;
        private System.Windows.Forms.DataGridView dgvTransacciones;
    }
}
