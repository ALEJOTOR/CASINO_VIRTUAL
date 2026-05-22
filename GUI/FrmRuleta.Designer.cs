namespace GUI
{
    partial class FrmRuleta
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.panelJuego = new System.Windows.Forms.Panel();
            this.lblPremio = new System.Windows.Forms.Label();
            this.lblResultado = new System.Windows.Forms.Label();
            this.btnApostarNumero = new System.Windows.Forms.Button();
            this.numNumero = new System.Windows.Forms.NumericUpDown();
            this.lblNumero = new System.Windows.Forms.Label();
            this.btnApostarColor = new System.Windows.Forms.Button();
            this.cboColor = new System.Windows.Forms.ComboBox();
            this.lblColor = new System.Windows.Forms.Label();
            this.txtApuesta = new System.Windows.Forms.TextBox();
            this.lblApuesta = new System.Windows.Forms.Label();
            this.lblMesa = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.panelJuego.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNumero)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(12, 18, 32);
            this.panelTop.Controls.Add(this.lblTitulo);
            this.panelTop.Controls.Add(this.lblSaldo);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(760, 76);
            this.panelTop.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.Gold;
            this.lblTitulo.Location = new System.Drawing.Point(22, 17);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(221, 41);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Ruleta Royal";
            // 
            // lblSaldo
            // 
            this.lblSaldo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSaldo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblSaldo.ForeColor = System.Drawing.Color.FromArgb(74, 222, 128);
            this.lblSaldo.Location = new System.Drawing.Point(480, 21);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(250, 34);
            this.lblSaldo.TabIndex = 1;
            this.lblSaldo.Text = "Saldo: $0.00";
            this.lblSaldo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelJuego
            // 
            this.panelJuego.BackColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.panelJuego.Controls.Add(this.lblPremio);
            this.panelJuego.Controls.Add(this.lblResultado);
            this.panelJuego.Controls.Add(this.btnApostarNumero);
            this.panelJuego.Controls.Add(this.numNumero);
            this.panelJuego.Controls.Add(this.lblNumero);
            this.panelJuego.Controls.Add(this.btnApostarColor);
            this.panelJuego.Controls.Add(this.cboColor);
            this.panelJuego.Controls.Add(this.lblColor);
            this.panelJuego.Controls.Add(this.txtApuesta);
            this.panelJuego.Controls.Add(this.lblApuesta);
            this.panelJuego.Controls.Add(this.lblMesa);
            this.panelJuego.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelJuego.Location = new System.Drawing.Point(0, 76);
            this.panelJuego.Name = "panelJuego";
            this.panelJuego.Size = new System.Drawing.Size(760, 424);
            this.panelJuego.TabIndex = 1;
            // 
            // lblPremio
            // 
            this.lblPremio.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblPremio.ForeColor = System.Drawing.Color.Gold;
            this.lblPremio.Location = new System.Drawing.Point(30, 322);
            this.lblPremio.Name = "lblPremio";
            this.lblPremio.Size = new System.Drawing.Size(700, 44);
            this.lblPremio.TabIndex = 10;
            this.lblPremio.Text = "Elige color o numero para jugar";
            this.lblPremio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblResultado
            // 
            this.lblResultado.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblResultado.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblResultado.Location = new System.Drawing.Point(30, 281);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(700, 30);
            this.lblResultado.TabIndex = 9;
            this.lblResultado.Text = "Mesa lista.";
            this.lblResultado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnApostarNumero
            // 
            this.btnApostarNumero.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnApostarNumero.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApostarNumero.FlatAppearance.BorderSize = 0;
            this.btnApostarNumero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApostarNumero.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnApostarNumero.ForeColor = System.Drawing.Color.White;
            this.btnApostarNumero.Location = new System.Drawing.Point(430, 213);
            this.btnApostarNumero.Name = "btnApostarNumero";
            this.btnApostarNumero.Size = new System.Drawing.Size(200, 38);
            this.btnApostarNumero.TabIndex = 8;
            this.btnApostarNumero.Text = "Apostar al numero";
            this.btnApostarNumero.UseVisualStyleBackColor = false;
            this.btnApostarNumero.Click += new System.EventHandler(this.btnApostarNumero_Click);
            // 
            // numNumero
            // 
            this.numNumero.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.numNumero.Location = new System.Drawing.Point(430, 169);
            this.numNumero.Maximum = new decimal(new int[] { 36, 0, 0, 0 });
            this.numNumero.Name = "numNumero";
            this.numNumero.Size = new System.Drawing.Size(200, 29);
            this.numNumero.TabIndex = 7;
            // 
            // lblNumero
            // 
            this.lblNumero.AutoSize = true;
            this.lblNumero.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblNumero.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblNumero.Location = new System.Drawing.Point(426, 144);
            this.lblNumero.Name = "lblNumero";
            this.lblNumero.Size = new System.Drawing.Size(114, 20);
            this.lblNumero.TabIndex = 6;
            this.lblNumero.Text = "Numero 0 a 36";
            // 
            // btnApostarColor
            // 
            this.btnApostarColor.BackColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this.btnApostarColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApostarColor.FlatAppearance.BorderSize = 0;
            this.btnApostarColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApostarColor.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnApostarColor.ForeColor = System.Drawing.Color.White;
            this.btnApostarColor.Location = new System.Drawing.Point(130, 213);
            this.btnApostarColor.Name = "btnApostarColor";
            this.btnApostarColor.Size = new System.Drawing.Size(200, 38);
            this.btnApostarColor.TabIndex = 5;
            this.btnApostarColor.Text = "Apostar al color";
            this.btnApostarColor.UseVisualStyleBackColor = false;
            this.btnApostarColor.Click += new System.EventHandler(this.btnApostarColor_Click);
            // 
            // cboColor
            // 
            this.cboColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboColor.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cboColor.FormattingEnabled = true;
            this.cboColor.Items.AddRange(new object[] { "Rojo", "Negro" });
            this.cboColor.Location = new System.Drawing.Point(130, 169);
            this.cboColor.Name = "cboColor";
            this.cboColor.Size = new System.Drawing.Size(200, 29);
            this.cboColor.TabIndex = 4;
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblColor.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblColor.Location = new System.Drawing.Point(126, 144);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(103, 20);
            this.lblColor.TabIndex = 3;
            this.lblColor.Text = "Color a elegir";
            // 
            // txtApuesta
            // 
            this.txtApuesta.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtApuesta.Location = new System.Drawing.Point(280, 89);
            this.txtApuesta.Name = "txtApuesta";
            this.txtApuesta.Size = new System.Drawing.Size(200, 29);
            this.txtApuesta.TabIndex = 2;
            // 
            // lblApuesta
            // 
            this.lblApuesta.AutoSize = true;
            this.lblApuesta.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblApuesta.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblApuesta.Location = new System.Drawing.Point(276, 64);
            this.lblApuesta.Name = "lblApuesta";
            this.lblApuesta.Size = new System.Drawing.Size(97, 20);
            this.lblApuesta.TabIndex = 1;
            this.lblApuesta.Text = "Apuesta ($)";
            // 
            // lblMesa
            // 
            this.lblMesa.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblMesa.ForeColor = System.Drawing.Color.White;
            this.lblMesa.Location = new System.Drawing.Point(30, 20);
            this.lblMesa.Name = "lblMesa";
            this.lblMesa.Size = new System.Drawing.Size(700, 32);
            this.lblMesa.TabIndex = 0;
            this.lblMesa.Text = "Color paga x2 | Numero exacto paga x36";
            this.lblMesa.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmRuleta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.ClientSize = new System.Drawing.Size(760, 500);
            this.Controls.Add(this.panelJuego);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(720, 500);
            this.Name = "FrmRuleta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Casino Virtual - Ruleta";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelJuego.ResumeLayout(false);
            this.panelJuego.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNumero)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Panel panelJuego;
        private System.Windows.Forms.Label lblMesa;
        private System.Windows.Forms.Label lblApuesta;
        private System.Windows.Forms.TextBox txtApuesta;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.ComboBox cboColor;
        private System.Windows.Forms.Button btnApostarColor;
        private System.Windows.Forms.Label lblNumero;
        private System.Windows.Forms.NumericUpDown numNumero;
        private System.Windows.Forms.Button btnApostarNumero;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.Label lblPremio;
    }
}
