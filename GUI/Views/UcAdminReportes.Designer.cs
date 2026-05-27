using System;

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
            this.lblFechaHora = new System.Windows.Forms.Label();
            this.pnlBotones = new System.Windows.Forms.Panel();
            this.btnReporteUsuarios = new System.Windows.Forms.Button();
            this.btnReportePartidas = new System.Windows.Forms.Button();
            this.btnReporteFinanciero = new System.Windows.Forms.Button();
            this.btnTopJugadores = new System.Windows.Forms.Button();
            this.btnExportarTxt = new System.Windows.Forms.Button();
            this.pnlContenido = new System.Windows.Forms.Panel();
            this.pnlBotones.SuspendLayout();
            this.SuspendLayout();

            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitulo.Font = new System.Drawing.Font("Georgia", 25F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(250, 204, 21);
            this.lblTitulo.Location = new System.Drawing.Point(34, 22);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(1248, 46);
            this.lblTitulo.Text = "Reportes del Sistema";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.lblFechaHora.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFechaHora.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.lblFechaHora.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblFechaHora.Location = new System.Drawing.Point(34, 68);
            this.lblFechaHora.Name = "lblFechaHora";
            this.lblFechaHora.Size = new System.Drawing.Size(1248, 18);
            this.lblFechaHora.Text = "";

            this.pnlBotones.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.pnlBotones.Controls.Add(this.btnReporteUsuarios);
            this.pnlBotones.Controls.Add(this.btnReportePartidas);
            this.pnlBotones.Controls.Add(this.btnReporteFinanciero);
            this.pnlBotones.Controls.Add(this.btnTopJugadores);
            this.pnlBotones.Controls.Add(this.btnExportarTxt);
            this.pnlBotones.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBotones.Location = new System.Drawing.Point(34, 86);
            this.pnlBotones.Name = "pnlBotones";
            this.pnlBotones.Size = new System.Drawing.Size(1248, 58);
            this.pnlBotones.TabIndex = 1;

            int btnY = 12;
            int btnH = 36;
            int btnW = 170;
            int gap = 10;

            this.btnReporteUsuarios.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnReporteUsuarios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReporteUsuarios.FlatAppearance.BorderSize = 0;
            this.btnReporteUsuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReporteUsuarios.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReporteUsuarios.ForeColor = System.Drawing.Color.White;
            this.btnReporteUsuarios.Location = new System.Drawing.Point(18, btnY);
            this.btnReporteUsuarios.Name = "btnReporteUsuarios";
            this.btnReporteUsuarios.Size = new System.Drawing.Size(btnW, btnH);
            this.btnReporteUsuarios.Text = "Usuarios";
            this.btnReporteUsuarios.UseVisualStyleBackColor = false;
            this.btnReporteUsuarios.Click += new System.EventHandler(this.btnReporteUsuarios_Click);

            this.btnReportePartidas.BackColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this.btnReportePartidas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReportePartidas.FlatAppearance.BorderSize = 0;
            this.btnReportePartidas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReportePartidas.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReportePartidas.ForeColor = System.Drawing.Color.White;
            this.btnReportePartidas.Location = new System.Drawing.Point(18 + (btnW + gap) * 1, btnY);
            this.btnReportePartidas.Name = "btnReportePartidas";
            this.btnReportePartidas.Size = new System.Drawing.Size(btnW, btnH);
            this.btnReportePartidas.Text = "Partidas";
            this.btnReportePartidas.UseVisualStyleBackColor = false;
            this.btnReportePartidas.Click += new System.EventHandler(this.btnReportePartidas_Click);

            this.btnReporteFinanciero.BackColor = System.Drawing.Color.FromArgb(250, 204, 21);
            this.btnReporteFinanciero.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReporteFinanciero.FlatAppearance.BorderSize = 0;
            this.btnReporteFinanciero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReporteFinanciero.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReporteFinanciero.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.btnReporteFinanciero.Location = new System.Drawing.Point(18 + (btnW + gap) * 2, btnY);
            this.btnReporteFinanciero.Name = "btnReporteFinanciero";
            this.btnReporteFinanciero.Size = new System.Drawing.Size(btnW, btnH);
            this.btnReporteFinanciero.Text = "Reporte Financiero";
            this.btnReporteFinanciero.UseVisualStyleBackColor = false;
            this.btnReporteFinanciero.Click += new System.EventHandler(this.btnReporteFinanciero_Click);

            this.btnTopJugadores.BackColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this.btnTopJugadores.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTopJugadores.FlatAppearance.BorderSize = 0;
            this.btnTopJugadores.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTopJugadores.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTopJugadores.ForeColor = System.Drawing.Color.White;
            this.btnTopJugadores.Location = new System.Drawing.Point(18 + (btnW + gap) * 3, btnY);
            this.btnTopJugadores.Name = "btnTopJugadores";
            this.btnTopJugadores.Size = new System.Drawing.Size(btnW, btnH);
            this.btnTopJugadores.Text = "Top Jugadores";
            this.btnTopJugadores.UseVisualStyleBackColor = false;
            this.btnTopJugadores.Click += new System.EventHandler(this.btnTopJugadores_Click);

            this.btnExportarTxt.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.btnExportarTxt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportarTxt.Enabled = false;
            this.btnExportarTxt.FlatAppearance.BorderSize = 0;
            this.btnExportarTxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarTxt.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExportarTxt.ForeColor = System.Drawing.Color.White;
            this.btnExportarTxt.Location = new System.Drawing.Point(1248 - 18 - btnW, btnY);
            this.btnExportarTxt.Name = "btnExportarTxt";
            this.btnExportarTxt.Size = new System.Drawing.Size(btnW, btnH);
            this.btnExportarTxt.Text = "Exportar TXT";
            this.btnExportarTxt.UseVisualStyleBackColor = false;
            this.btnExportarTxt.Click += new System.EventHandler(this.btnExportarTxt_Click);

            this.pnlContenido.AutoScroll = true;
            this.pnlContenido.BackColor = System.Drawing.Color.FromArgb(8, 13, 24);
            this.pnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContenido.Location = new System.Drawing.Point(34, 144);
            this.pnlContenido.Name = "pnlContenido";
            this.pnlContenido.Size = new System.Drawing.Size(1248, 608);
            this.pnlContenido.TabIndex = 2;

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(8, 13, 24);
            this.Controls.Add(this.pnlContenido);
            this.Controls.Add(this.pnlBotones);
            this.Controls.Add(this.lblFechaHora);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "UcAdminReportes";
            this.Padding = new System.Windows.Forms.Padding(34, 22, 34, 34);
            this.Size = new System.Drawing.Size(1316, 786);
            this.pnlBotones.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblFechaHora;
        private System.Windows.Forms.Panel pnlBotones;
        private System.Windows.Forms.Button btnReporteUsuarios;
        private System.Windows.Forms.Button btnReportePartidas;
        private System.Windows.Forms.Button btnReporteFinanciero;
        private System.Windows.Forms.Button btnTopJugadores;
        private System.Windows.Forms.Button btnExportarTxt;
        private System.Windows.Forms.Panel pnlContenido;
    }
}
