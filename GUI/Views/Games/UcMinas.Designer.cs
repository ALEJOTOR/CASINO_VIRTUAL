namespace GUI
{
    partial class UcMinas
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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelTablero = new System.Windows.Forms.Panel();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(12)))), ((int)(((byte)(24)))));
            this.panelHeader.Controls.Add(this.lblTitulo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(560, 72);
            this.panelHeader.TabIndex = 9;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Georgia", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.Gold;
            this.lblTitulo.Location = new System.Drawing.Point(22, 17);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(250, 35);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "MINAS ROYAL";
            // 
            // lblApuesta
            // 
            this.lblApuesta.AutoSize = true;
            this.lblApuesta.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblApuesta.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblApuesta.Location = new System.Drawing.Point(24, 96);
            this.lblApuesta.Name = "lblApuesta";
            this.lblApuesta.Size = new System.Drawing.Size(96, 20);
            this.lblApuesta.TabIndex = 0;
            this.lblApuesta.Text = "Apuesta ($):";
            // 
            // txtApuesta
            // 
            this.txtApuesta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtApuesta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtApuesta.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtApuesta.Location = new System.Drawing.Point(130, 92);
            this.txtApuesta.Name = "txtApuesta";
            this.txtApuesta.Size = new System.Drawing.Size(130, 27);
            this.txtApuesta.TabIndex = 1;
            // 
            // lblMinas
            // 
            this.lblMinas.AutoSize = true;
            this.lblMinas.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMinas.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMinas.Location = new System.Drawing.Point(290, 96);
            this.lblMinas.Name = "lblMinas";
            this.lblMinas.Size = new System.Drawing.Size(101, 20);
            this.lblMinas.TabIndex = 2;
            this.lblMinas.Text = "Minas (1-20):";
            // 
            // txtMinas
            // 
            this.txtMinas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtMinas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMinas.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtMinas.Location = new System.Drawing.Point(398, 92);
            this.txtMinas.Name = "txtMinas";
            this.txtMinas.Size = new System.Drawing.Size(50, 26);
            this.txtMinas.TabIndex = 3;
            this.txtMinas.Text = "5";
            // 
            // btnIniciar
            // 
            this.btnIniciar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.btnIniciar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIniciar.FlatAppearance.BorderSize = 0;
            this.btnIniciar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIniciar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnIniciar.ForeColor = System.Drawing.Color.White;
            this.btnIniciar.Location = new System.Drawing.Point(24, 136);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(112, 34);
            this.btnIniciar.TabIndex = 4;
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = false;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // btnRetirar
            // 
            this.btnRetirar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(179)))), ((int)(((byte)(8)))));
            this.btnRetirar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRetirar.Enabled = false;
            this.btnRetirar.FlatAppearance.BorderSize = 0;
            this.btnRetirar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRetirar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRetirar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.btnRetirar.Location = new System.Drawing.Point(148, 136);
            this.btnRetirar.Name = "btnRetirar";
            this.btnRetirar.Size = new System.Drawing.Size(170, 34);
            this.btnRetirar.TabIndex = 5;
            this.btnRetirar.Text = "Retirar ganancias";
            this.btnRetirar.UseVisualStyleBackColor = false;
            this.btnRetirar.Click += new System.EventHandler(this.btnRetirar_Click);
            // 
            // lblMultiplicador
            // 
            this.lblMultiplicador.AutoSize = true;
            this.lblMultiplicador.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMultiplicador.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(204)))), ((int)(((byte)(21)))));
            this.lblMultiplicador.Location = new System.Drawing.Point(24, 186);
            this.lblMultiplicador.Name = "lblMultiplicador";
            this.lblMultiplicador.Size = new System.Drawing.Size(143, 20);
            this.lblMultiplicador.TabIndex = 6;
            this.lblMultiplicador.Text = "Multiplicador: x1.00";
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.lblEstado.Location = new System.Drawing.Point(210, 186);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(204, 20);
            this.lblEstado.TabIndex = 7;
            this.lblEstado.Text = "Configura y presiona Iniciar.";
            // 
            // lblSaldo
            // 
            this.lblSaldo.AutoSize = true;
            this.lblSaldo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSaldo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(222)))), ((int)(((byte)(128)))));
            this.lblSaldo.Location = new System.Drawing.Point(24, 214);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(173, 20);
            this.lblSaldo.TabIndex = 8;
            this.lblSaldo.Text = "Saldo disponible: $0.00";
            this.lblSaldo.Visible = false;
            // 
            // panelTablero
            // 
            this.panelTablero.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(47)))), ((int)(((byte)(73)))));
            this.panelTablero.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTablero.Location = new System.Drawing.Point(170, 250);
            this.panelTablero.Name = "panelTablero";
            this.panelTablero.Size = new System.Drawing.Size(360, 360);
            this.panelTablero.TabIndex = 10;
            // 
            // UcMinas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.Size = new System.Drawing.Size(560, 640);
            this.Controls.Add(this.panelTablero);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.lblApuesta);
            this.Controls.Add(this.txtApuesta);
            this.Controls.Add(this.lblMinas);
            this.Controls.Add(this.txtMinas);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.btnRetirar);
            this.Controls.Add(this.lblMultiplicador);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.lblSaldo);
            this.Name = "UcMinas";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
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
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelTablero;
    }
}

