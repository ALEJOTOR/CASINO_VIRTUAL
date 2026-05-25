namespace GUI
{
    partial class UcAdminReportes
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.pnlBotones = new System.Windows.Forms.Panel();
            this.btnReporteUsuarios = new System.Windows.Forms.Button();
            this.btnReportePartidas = new System.Windows.Forms.Button();
            this.txtReporte = new System.Windows.Forms.TextBox();
            this.pnlBotones.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Georgia", 26F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(250, 204, 21);
            this.lblTitulo.Location = new System.Drawing.Point(34, 16);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(600, 44);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Reportes del Sistema";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBotones
            // 
            this.pnlBotones.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.pnlBotones.Controls.Add(this.btnReporteUsuarios);
            this.pnlBotones.Controls.Add(this.btnReportePartidas);
            this.pnlBotones.Location = new System.Drawing.Point(34, 70);
            this.pnlBotones.Name = "pnlBotones";
            this.pnlBotones.Size = new System.Drawing.Size(1248, 70);
            this.pnlBotones.TabIndex = 1;
            // 
            // btnReporteUsuarios
            // 
            this.btnReporteUsuarios.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnReporteUsuarios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReporteUsuarios.FlatAppearance.BorderSize = 0;
            this.btnReporteUsuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReporteUsuarios.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnReporteUsuarios.ForeColor = System.Drawing.Color.White;
            this.btnReporteUsuarios.Location = new System.Drawing.Point(18, 18);
            this.btnReporteUsuarios.Name = "btnReporteUsuarios";
            this.btnReporteUsuarios.Size = new System.Drawing.Size(220, 36);
            this.btnReporteUsuarios.TabIndex = 0;
            this.btnReporteUsuarios.Text = "Reporte Usuarios";
            this.btnReporteUsuarios.UseVisualStyleBackColor = false;
            this.btnReporteUsuarios.Click += new System.EventHandler(this.btnReporteUsuarios_Click);
            // 
            // btnReportePartidas
            // 
            this.btnReportePartidas.BackColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this.btnReportePartidas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReportePartidas.FlatAppearance.BorderSize = 0;
            this.btnReportePartidas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReportePartidas.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnReportePartidas.ForeColor = System.Drawing.Color.White;
            this.btnReportePartidas.Location = new System.Drawing.Point(254, 18);
            this.btnReportePartidas.Name = "btnReportePartidas";
            this.btnReportePartidas.Size = new System.Drawing.Size(220, 36);
            this.btnReportePartidas.TabIndex = 1;
            this.btnReportePartidas.Text = "Reporte Partidas";
            this.btnReportePartidas.UseVisualStyleBackColor = false;
            this.btnReportePartidas.Click += new System.EventHandler(this.btnReportePartidas_Click);
            // 
            // txtReporte
            // 
            this.txtReporte.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.txtReporte.Font = new System.Drawing.Font("Courier New", 10F);
            this.txtReporte.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.txtReporte.Location = new System.Drawing.Point(34, 150);
            this.txtReporte.Multiline = true;
            this.txtReporte.Name = "txtReporte";
            this.txtReporte.ReadOnly = true;
            this.txtReporte.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReporte.Size = new System.Drawing.Size(1248, 600);
            this.txtReporte.TabIndex = 2;
            // 
            // UcAdminReportes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(8, 13, 24);
            this.Controls.Add(this.txtReporte);
            this.Controls.Add(this.pnlBotones);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "UcAdminReportes";
            this.Size = new System.Drawing.Size(1316, 786);
            this.pnlBotones.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel pnlBotones;
        private System.Windows.Forms.Button btnReporteUsuarios;
        private System.Windows.Forms.Button btnReportePartidas;
        private System.Windows.Forms.TextBox txtReporte;
    }
}
