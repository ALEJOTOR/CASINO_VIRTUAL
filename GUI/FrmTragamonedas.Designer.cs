namespace GUI
{
    partial class FrmTragamonedas
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
            this.lblResultado = new System.Windows.Forms.Label();
            this.btnGirar = new System.Windows.Forms.Button();
            this.txtApuesta = new System.Windows.Forms.TextBox();
            this.lblApuesta = new System.Windows.Forms.Label();
            this.panelRollos = new System.Windows.Forms.TableLayoutPanel();
            this.lblRollo1 = new System.Windows.Forms.Label();
            this.lblRollo2 = new System.Windows.Forms.Label();
            this.lblRollo3 = new System.Windows.Forms.Label();
            this.lblReglas = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.panelJuego.SuspendLayout();
            this.panelRollos.SuspendLayout();
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
            this.lblTitulo.Size = new System.Drawing.Size(261, 41);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Tragamonedas";
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
            this.panelJuego.Controls.Add(this.lblResultado);
            this.panelJuego.Controls.Add(this.btnGirar);
            this.panelJuego.Controls.Add(this.txtApuesta);
            this.panelJuego.Controls.Add(this.lblApuesta);
            this.panelJuego.Controls.Add(this.panelRollos);
            this.panelJuego.Controls.Add(this.lblReglas);
            this.panelJuego.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelJuego.Location = new System.Drawing.Point(0, 76);
            this.panelJuego.Name = "panelJuego";
            this.panelJuego.Size = new System.Drawing.Size(760, 424);
            this.panelJuego.TabIndex = 1;
            // 
            // lblResultado
            // 
            this.lblResultado.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblResultado.ForeColor = System.Drawing.Color.Gold;
            this.lblResultado.Location = new System.Drawing.Point(30, 331);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(700, 44);
            this.lblResultado.TabIndex = 5;
            this.lblResultado.Text = "Listo para girar";
            this.lblResultado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnGirar
            // 
            this.btnGirar.BackColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this.btnGirar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGirar.FlatAppearance.BorderSize = 0;
            this.btnGirar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGirar.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnGirar.ForeColor = System.Drawing.Color.White;
            this.btnGirar.Location = new System.Drawing.Point(300, 274);
            this.btnGirar.Name = "btnGirar";
            this.btnGirar.Size = new System.Drawing.Size(160, 42);
            this.btnGirar.TabIndex = 4;
            this.btnGirar.Text = "Girar";
            this.btnGirar.UseVisualStyleBackColor = false;
            this.btnGirar.Click += new System.EventHandler(this.btnGirar_Click);
            // 
            // txtApuesta
            // 
            this.txtApuesta.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtApuesta.Location = new System.Drawing.Point(300, 226);
            this.txtApuesta.Name = "txtApuesta";
            this.txtApuesta.Size = new System.Drawing.Size(160, 29);
            this.txtApuesta.TabIndex = 3;
            // 
            // lblApuesta
            // 
            this.lblApuesta.AutoSize = true;
            this.lblApuesta.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblApuesta.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblApuesta.Location = new System.Drawing.Point(296, 201);
            this.lblApuesta.Name = "lblApuesta";
            this.lblApuesta.Size = new System.Drawing.Size(97, 20);
            this.lblApuesta.TabIndex = 2;
            this.lblApuesta.Text = "Apuesta ($)";
            // 
            // panelRollos
            // 
            this.panelRollos.ColumnCount = 3;
            this.panelRollos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.panelRollos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.panelRollos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.panelRollos.Controls.Add(this.lblRollo1, 0, 0);
            this.panelRollos.Controls.Add(this.lblRollo2, 1, 0);
            this.panelRollos.Controls.Add(this.lblRollo3, 2, 0);
            this.panelRollos.Location = new System.Drawing.Point(100, 78);
            this.panelRollos.Name = "panelRollos";
            this.panelRollos.RowCount = 1;
            this.panelRollos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelRollos.Size = new System.Drawing.Size(560, 100);
            this.panelRollos.TabIndex = 1;
            // 
            // lblRollo1
            // 
            this.lblRollo1.BackColor = System.Drawing.Color.White;
            this.lblRollo1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRollo1.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblRollo1.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblRollo1.Location = new System.Drawing.Point(8, 0);
            this.lblRollo1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblRollo1.Name = "lblRollo1";
            this.lblRollo1.Size = new System.Drawing.Size(170, 100);
            this.lblRollo1.TabIndex = 0;
            this.lblRollo1.Text = "7";
            this.lblRollo1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRollo2
            // 
            this.lblRollo2.BackColor = System.Drawing.Color.White;
            this.lblRollo2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRollo2.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblRollo2.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblRollo2.Location = new System.Drawing.Point(194, 0);
            this.lblRollo2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblRollo2.Name = "lblRollo2";
            this.lblRollo2.Size = new System.Drawing.Size(170, 100);
            this.lblRollo2.TabIndex = 1;
            this.lblRollo2.Text = "BAR";
            this.lblRollo2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRollo3
            // 
            this.lblRollo3.BackColor = System.Drawing.Color.White;
            this.lblRollo3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRollo3.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblRollo3.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblRollo3.Location = new System.Drawing.Point(380, 0);
            this.lblRollo3.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lblRollo3.Name = "lblRollo3";
            this.lblRollo3.Size = new System.Drawing.Size(172, 100);
            this.lblRollo3.TabIndex = 2;
            this.lblRollo3.Text = "$";
            this.lblRollo3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReglas
            // 
            this.lblReglas.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblReglas.ForeColor = System.Drawing.Color.White;
            this.lblReglas.Location = new System.Drawing.Point(30, 28);
            this.lblReglas.Name = "lblReglas";
            this.lblReglas.Size = new System.Drawing.Size(700, 32);
            this.lblReglas.TabIndex = 0;
            this.lblReglas.Text = "Dos iguales pagan x2 | Tres iguales pagan x8";
            this.lblReglas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmTragamonedas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.ClientSize = new System.Drawing.Size(760, 500);
            this.Controls.Add(this.panelJuego);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(720, 500);
            this.Name = "FrmTragamonedas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Casino Virtual - Tragamonedas";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelJuego.ResumeLayout(false);
            this.panelJuego.PerformLayout();
            this.panelRollos.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Panel panelJuego;
        private System.Windows.Forms.Label lblReglas;
        private System.Windows.Forms.TableLayoutPanel panelRollos;
        private System.Windows.Forms.Label lblRollo1;
        private System.Windows.Forms.Label lblRollo2;
        private System.Windows.Forms.Label lblRollo3;
        private System.Windows.Forms.Label lblApuesta;
        private System.Windows.Forms.TextBox txtApuesta;
        private System.Windows.Forms.Button btnGirar;
        private System.Windows.Forms.Label lblResultado;
    }
}
