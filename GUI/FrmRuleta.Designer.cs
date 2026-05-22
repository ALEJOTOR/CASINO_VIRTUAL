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
            this.panelControles = new System.Windows.Forms.Panel();
            this.btnGirarRuleta = new System.Windows.Forms.Button();
            this.btnLimpiarMesa = new System.Windows.Forms.Button();
            this.lblTotalApostado = new System.Windows.Forms.Label();
            this.numNumero = new System.Windows.Forms.NumericUpDown();
            this.lblNumero = new System.Windows.Forms.Label();
            this.cboTipoApuesta = new System.Windows.Forms.ComboBox();
            this.lblTipoApuesta = new System.Windows.Forms.Label();
            this.txtApuesta = new System.Windows.Forms.TextBox();
            this.lblApuesta = new System.Windows.Forms.Label();
            this.panelTapete = new System.Windows.Forms.Panel();
            this.lblDocena3 = new System.Windows.Forms.Label();
            this.lblDocena2 = new System.Windows.Forms.Label();
            this.lblDocena1 = new System.Windows.Forms.Label();
            this.lblNegro = new System.Windows.Forms.Label();
            this.lblRojo = new System.Windows.Forms.Label();
            this.lblCero = new System.Windows.Forms.Label();
            this.panelRueda = new System.Windows.Forms.Panel();
            this.lblNumeroGanador = new System.Windows.Forms.Label();
            this.lblMesa = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.panelJuego.SuspendLayout();
            this.panelControles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNumero)).BeginInit();
            this.panelTapete.SuspendLayout();
            this.panelRueda.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(7, 12, 24);
            this.panelTop.Controls.Add(this.lblTitulo);
            this.panelTop.Controls.Add(this.lblSaldo);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1060, 78);
            this.panelTop.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Georgia", 25F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.Gold;
            this.lblTitulo.Location = new System.Drawing.Point(24, 18);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(280, 39);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "RULETA ROYAL";
            // 
            // lblSaldo
            // 
            this.lblSaldo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSaldo.BackColor = System.Drawing.Color.FromArgb(18, 30, 52);
            this.lblSaldo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblSaldo.ForeColor = System.Drawing.Color.FromArgb(74, 222, 128);
            this.lblSaldo.Location = new System.Drawing.Point(780, 18);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.lblSaldo.Size = new System.Drawing.Size(250, 40);
            this.lblSaldo.TabIndex = 1;
            this.lblSaldo.Text = "Saldo: $0.00";
            this.lblSaldo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelJuego
            // 
            this.panelJuego.BackColor = System.Drawing.Color.FromArgb(10, 16, 28);
            this.panelJuego.Controls.Add(this.lblPremio);
            this.panelJuego.Controls.Add(this.lblResultado);
            this.panelJuego.Controls.Add(this.panelControles);
            this.panelJuego.Controls.Add(this.panelTapete);
            this.panelJuego.Controls.Add(this.panelRueda);
            this.panelJuego.Controls.Add(this.lblMesa);
            this.panelJuego.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelJuego.Location = new System.Drawing.Point(0, 78);
            this.panelJuego.Name = "panelJuego";
            this.panelJuego.Size = new System.Drawing.Size(1060, 572);
            this.panelJuego.TabIndex = 1;
            // 
            // lblPremio
            // 
            this.lblPremio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPremio.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblPremio.ForeColor = System.Drawing.Color.Gold;
            this.lblPremio.Location = new System.Drawing.Point(24, 496);
            this.lblPremio.Name = "lblPremio";
            this.lblPremio.Size = new System.Drawing.Size(1006, 48);
            this.lblPremio.TabIndex = 5;
            this.lblPremio.Text = "Hagan sus apuestas";
            this.lblPremio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblResultado
            // 
            this.lblResultado.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResultado.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblResultado.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblResultado.Location = new System.Drawing.Point(24, 456);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(1006, 32);
            this.lblResultado.TabIndex = 4;
            this.lblResultado.Text = "Mesa abierta. Color paga x2 | Numero exacto paga x36";
            this.lblResultado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelControles
            // 
            this.panelControles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControles.BackColor = System.Drawing.Color.FromArgb(18, 30, 52);
            this.panelControles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelControles.Controls.Add(this.btnGirarRuleta);
            this.panelControles.Controls.Add(this.btnLimpiarMesa);
            this.panelControles.Controls.Add(this.lblTotalApostado);
            this.panelControles.Controls.Add(this.numNumero);
            this.panelControles.Controls.Add(this.lblNumero);
            this.panelControles.Controls.Add(this.cboTipoApuesta);
            this.panelControles.Controls.Add(this.lblTipoApuesta);
            this.panelControles.Controls.Add(this.txtApuesta);
            this.panelControles.Controls.Add(this.lblApuesta);
            this.panelControles.Location = new System.Drawing.Point(760, 72);
            this.panelControles.Name = "panelControles";
            this.panelControles.Size = new System.Drawing.Size(270, 354);
            this.panelControles.TabIndex = 3;
            // 
            // btnGirarRuleta
            // 
            this.btnGirarRuleta.BackColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this.btnGirarRuleta.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGirarRuleta.FlatAppearance.BorderSize = 0;
            this.btnGirarRuleta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGirarRuleta.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnGirarRuleta.ForeColor = System.Drawing.Color.White;
            this.btnGirarRuleta.Location = new System.Drawing.Point(24, 286);
            this.btnGirarRuleta.Name = "btnGirarRuleta";
            this.btnGirarRuleta.Size = new System.Drawing.Size(220, 42);
            this.btnGirarRuleta.TabIndex = 7;
            this.btnGirarRuleta.Text = "GIRAR RULETA";
            this.btnGirarRuleta.UseVisualStyleBackColor = false;
            this.btnGirarRuleta.Click += new System.EventHandler(this.btnGirarRuleta_Click);
            // 
            // btnLimpiarMesa
            // 
            this.btnLimpiarMesa.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.btnLimpiarMesa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpiarMesa.FlatAppearance.BorderSize = 0;
            this.btnLimpiarMesa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiarMesa.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnLimpiarMesa.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnLimpiarMesa.Location = new System.Drawing.Point(24, 332);
            this.btnLimpiarMesa.Name = "btnLimpiarMesa";
            this.btnLimpiarMesa.Size = new System.Drawing.Size(220, 24);
            this.btnLimpiarMesa.TabIndex = 9;
            this.btnLimpiarMesa.Text = "Limpiar mesa";
            this.btnLimpiarMesa.UseVisualStyleBackColor = false;
            this.btnLimpiarMesa.Click += new System.EventHandler(this.btnLimpiarMesa_Click);
            // 
            // lblTotalApostado
            // 
            this.lblTotalApostado.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTotalApostado.ForeColor = System.Drawing.Color.Gold;
            this.lblTotalApostado.Location = new System.Drawing.Point(24, 246);
            this.lblTotalApostado.Name = "lblTotalApostado";
            this.lblTotalApostado.Size = new System.Drawing.Size(220, 24);
            this.lblTotalApostado.TabIndex = 8;
            this.lblTotalApostado.Text = "Total apostado: $0";
            this.lblTotalApostado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numNumero
            // 
            this.numNumero.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.numNumero.Location = new System.Drawing.Point(24, 224);
            this.numNumero.Maximum = new decimal(new int[] { 36, 0, 0, 0 });
            this.numNumero.Name = "numNumero";
            this.numNumero.Size = new System.Drawing.Size(220, 29);
            this.numNumero.TabIndex = 6;
            // 
            // lblNumero
            // 
            this.lblNumero.AutoSize = true;
            this.lblNumero.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNumero.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblNumero.Location = new System.Drawing.Point(20, 200);
            this.lblNumero.Name = "lblNumero";
            this.lblNumero.Size = new System.Drawing.Size(108, 19);
            this.lblNumero.TabIndex = 5;
            this.lblNumero.Text = "Haz clic sobre la mesa";
            // 
            // cboTipoApuesta
            // 
            this.cboTipoApuesta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTipoApuesta.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cboTipoApuesta.FormattingEnabled = true;
            this.cboTipoApuesta.Items.AddRange(new object[] {
            "Rojo",
            "Negro",
            "Par",
            "Impar",
            "1 - 18",
            "19 - 36",
            "1ra docena (1 - 12)",
            "2da docena (13 - 24)",
            "3ra docena (25 - 36)",
            "Numero exacto"});
            this.cboTipoApuesta.Location = new System.Drawing.Point(24, 132);
            this.cboTipoApuesta.Name = "cboTipoApuesta";
            this.cboTipoApuesta.Size = new System.Drawing.Size(220, 29);
            this.cboTipoApuesta.TabIndex = 3;
            this.cboTipoApuesta.SelectedIndexChanged += new System.EventHandler(this.cboTipoApuesta_SelectedIndexChanged);
            // 
            // lblTipoApuesta
            // 
            this.lblTipoApuesta.AutoSize = true;
            this.lblTipoApuesta.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTipoApuesta.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblTipoApuesta.Location = new System.Drawing.Point(20, 108);
            this.lblTipoApuesta.Name = "lblTipoApuesta";
            this.lblTipoApuesta.Size = new System.Drawing.Size(116, 19);
            this.lblTipoApuesta.TabIndex = 2;
            this.lblTipoApuesta.Text = "Fichas disponibles";
            // 
            // txtApuesta
            // 
            this.txtApuesta.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtApuesta.Location = new System.Drawing.Point(24, 52);
            this.txtApuesta.Name = "txtApuesta";
            this.txtApuesta.ReadOnly = true;
            this.txtApuesta.Size = new System.Drawing.Size(220, 29);
            this.txtApuesta.TabIndex = 1;
            this.txtApuesta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblApuesta
            // 
            this.lblApuesta.AutoSize = true;
            this.lblApuesta.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblApuesta.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblApuesta.Location = new System.Drawing.Point(20, 28);
            this.lblApuesta.Name = "lblApuesta";
            this.lblApuesta.Size = new System.Drawing.Size(85, 19);
            this.lblApuesta.TabIndex = 0;
            this.lblApuesta.Text = "Ficha seleccionada";
            // 
            // panelTapete
            // 
            this.panelTapete.BackColor = System.Drawing.Color.FromArgb(5, 91, 53);
            this.panelTapete.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTapete.Controls.Add(this.lblDocena3);
            this.panelTapete.Controls.Add(this.lblDocena2);
            this.panelTapete.Controls.Add(this.lblDocena1);
            this.panelTapete.Controls.Add(this.lblNegro);
            this.panelTapete.Controls.Add(this.lblRojo);
            this.panelTapete.Controls.Add(this.lblCero);
            this.panelTapete.Location = new System.Drawing.Point(370, 72);
            this.panelTapete.Name = "panelTapete";
            this.panelTapete.Size = new System.Drawing.Size(360, 354);
            this.panelTapete.TabIndex = 2;
            this.panelTapete.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTapete_Paint);
            // 
            // lblDocena3
            // 
            this.lblDocena3.BackColor = System.Drawing.Color.FromArgb(6, 78, 59);
            this.lblDocena3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDocena3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDocena3.ForeColor = System.Drawing.Color.White;
            this.lblDocena3.Location = new System.Drawing.Point(238, 262);
            this.lblDocena3.Name = "lblDocena3";
            this.lblDocena3.Size = new System.Drawing.Size(104, 34);
            this.lblDocena3.TabIndex = 5;
            this.lblDocena3.Text = "25 - 36";
            this.lblDocena3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDocena2
            // 
            this.lblDocena2.BackColor = System.Drawing.Color.FromArgb(6, 78, 59);
            this.lblDocena2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDocena2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDocena2.ForeColor = System.Drawing.Color.White;
            this.lblDocena2.Location = new System.Drawing.Point(126, 262);
            this.lblDocena2.Name = "lblDocena2";
            this.lblDocena2.Size = new System.Drawing.Size(104, 34);
            this.lblDocena2.TabIndex = 4;
            this.lblDocena2.Text = "13 - 24";
            this.lblDocena2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDocena1
            // 
            this.lblDocena1.BackColor = System.Drawing.Color.FromArgb(6, 78, 59);
            this.lblDocena1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDocena1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDocena1.ForeColor = System.Drawing.Color.White;
            this.lblDocena1.Location = new System.Drawing.Point(14, 262);
            this.lblDocena1.Name = "lblDocena1";
            this.lblDocena1.Size = new System.Drawing.Size(104, 34);
            this.lblDocena1.TabIndex = 3;
            this.lblDocena1.Text = "1 - 12";
            this.lblDocena1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNegro
            // 
            this.lblNegro.BackColor = System.Drawing.Color.FromArgb(12, 12, 14);
            this.lblNegro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNegro.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblNegro.ForeColor = System.Drawing.Color.White;
            this.lblNegro.Location = new System.Drawing.Point(182, 304);
            this.lblNegro.Name = "lblNegro";
            this.lblNegro.Size = new System.Drawing.Size(54, 30);
            this.lblNegro.TabIndex = 2;
            this.lblNegro.Text = "NEGRO";
            this.lblNegro.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRojo
            // 
            this.lblRojo.BackColor = System.Drawing.Color.FromArgb(185, 28, 28);
            this.lblRojo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRojo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblRojo.ForeColor = System.Drawing.Color.White;
            this.lblRojo.Location = new System.Drawing.Point(126, 304);
            this.lblRojo.Name = "lblRojo";
            this.lblRojo.Size = new System.Drawing.Size(54, 30);
            this.lblRojo.TabIndex = 1;
            this.lblRojo.Text = "ROJO";
            this.lblRojo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCero
            // 
            this.lblCero.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.lblCero.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCero.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblCero.ForeColor = System.Drawing.Color.White;
            this.lblCero.Location = new System.Drawing.Point(14, 16);
            this.lblCero.Name = "lblCero";
            this.lblCero.Size = new System.Drawing.Size(54, 230);
            this.lblCero.TabIndex = 0;
            this.lblCero.Text = "0";
            this.lblCero.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelRueda
            // 
            this.panelRueda.BackColor = System.Drawing.Color.FromArgb(32, 18, 11);
            this.panelRueda.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRueda.Controls.Add(this.lblNumeroGanador);
            this.panelRueda.Location = new System.Drawing.Point(28, 72);
            this.panelRueda.Name = "panelRueda";
            this.panelRueda.Size = new System.Drawing.Size(312, 354);
            this.panelRueda.TabIndex = 1;
            this.panelRueda.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRueda_Paint);
            // 
            // lblNumeroGanador
            // 
            this.lblNumeroGanador.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblNumeroGanador.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNumeroGanador.Font = new System.Drawing.Font("Georgia", 26F, System.Drawing.FontStyle.Bold);
            this.lblNumeroGanador.ForeColor = System.Drawing.Color.Gold;
            this.lblNumeroGanador.Location = new System.Drawing.Point(101, 148);
            this.lblNumeroGanador.Name = "lblNumeroGanador";
            this.lblNumeroGanador.Size = new System.Drawing.Size(110, 58);
            this.lblNumeroGanador.TabIndex = 0;
            this.lblNumeroGanador.Text = "--";
            this.lblNumeroGanador.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMesa
            // 
            this.lblMesa.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMesa.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblMesa.ForeColor = System.Drawing.Color.White;
            this.lblMesa.Location = new System.Drawing.Point(24, 24);
            this.lblMesa.Name = "lblMesa";
            this.lblMesa.Size = new System.Drawing.Size(1006, 32);
            this.lblMesa.TabIndex = 0;
            this.lblMesa.Text = "Mesa europea | 0 a 36";
            this.lblMesa.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmRuleta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(10, 16, 28);
            this.ClientSize = new System.Drawing.Size(1060, 650);
            this.Controls.Add(this.panelJuego);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(980, 640);
            this.Name = "FrmRuleta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Casino Virtual - Ruleta";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelJuego.ResumeLayout(false);
            this.panelControles.ResumeLayout(false);
            this.panelControles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNumero)).EndInit();
            this.panelTapete.ResumeLayout(false);
            this.panelRueda.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Panel panelJuego;
        private System.Windows.Forms.Label lblMesa;
        private System.Windows.Forms.Panel panelRueda;
        private System.Windows.Forms.Label lblNumeroGanador;
        private System.Windows.Forms.Panel panelTapete;
        private System.Windows.Forms.Label lblCero;
        private System.Windows.Forms.Label lblRojo;
        private System.Windows.Forms.Label lblNegro;
        private System.Windows.Forms.Label lblDocena1;
        private System.Windows.Forms.Label lblDocena2;
        private System.Windows.Forms.Label lblDocena3;
        private System.Windows.Forms.Panel panelControles;
        private System.Windows.Forms.Label lblApuesta;
        private System.Windows.Forms.TextBox txtApuesta;
        private System.Windows.Forms.Label lblTipoApuesta;
        private System.Windows.Forms.ComboBox cboTipoApuesta;
        private System.Windows.Forms.Label lblNumero;
        private System.Windows.Forms.NumericUpDown numNumero;
        private System.Windows.Forms.Button btnGirarRuleta;
        private System.Windows.Forms.Button btnLimpiarMesa;
        private System.Windows.Forms.Label lblTotalApostado;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.Label lblPremio;
    }
}
