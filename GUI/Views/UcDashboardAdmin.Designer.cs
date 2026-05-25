namespace GUI
{
    partial class UcDashboardAdmin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.tlpKPIs = new System.Windows.Forms.TableLayoutPanel();
            this.cardTotalUsuarios = new System.Windows.Forms.Panel();
            this.lblTotalUsuariosValor = new System.Windows.Forms.Label();
            this.lblTotalUsuarios = new System.Windows.Forms.Label();
            this.cardPartidasHoy = new System.Windows.Forms.Panel();
            this.lblPartidasHoyValor = new System.Windows.Forms.Label();
            this.lblPartidasHoy = new System.Windows.Forms.Label();
            this.cardIngresosHoy = new System.Windows.Forms.Panel();
            this.lblIngresosHoyValor = new System.Windows.Forms.Label();
            this.lblIngresosHoy = new System.Windows.Forms.Label();
            this.cardGananciaCasaHoy = new System.Windows.Forms.Panel();
            this.lblGananciaCasaHoyValor = new System.Windows.Forms.Label();
            this.lblGananciaCasaHoy = new System.Windows.Forms.Label();
            this.chartIngresos = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.bottomTlp = new System.Windows.Forms.TableLayoutPanel();
            this.chartPartidas = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pnlTopJugadores = new System.Windows.Forms.Panel();
            this.dgvTopJugadores = new System.Windows.Forms.DataGridView();
            this.lblTopJugadores = new System.Windows.Forms.Label();
            this.pnlScroll = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.tlpKPIs.SuspendLayout();
            this.cardTotalUsuarios.SuspendLayout();
            this.cardPartidasHoy.SuspendLayout();
            this.cardIngresosHoy.SuspendLayout();
            this.cardGananciaCasaHoy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartIngresos)).BeginInit();
            this.bottomTlp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPartidas)).BeginInit();
            this.pnlTopJugadores.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopJugadores)).BeginInit();
            this.pnlScroll.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblTitulo.Location = new System.Drawing.Point(44, 28);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(200, 32);
            this.lblTitulo.TabIndex = 3;
            this.lblTitulo.Text = "Dashboard";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tlpKPIs
            // 
            this.tlpKPIs.BackColor = System.Drawing.Color.FromArgb(8, 13, 24);
            this.tlpKPIs.ColumnCount = 4;
            this.tlpKPIs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpKPIs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpKPIs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpKPIs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpKPIs.Controls.Add(this.cardTotalUsuarios, 0, 0);
            this.tlpKPIs.Controls.Add(this.cardPartidasHoy, 1, 0);
            this.tlpKPIs.Controls.Add(this.cardIngresosHoy, 2, 0);
            this.tlpKPIs.Controls.Add(this.cardGananciaCasaHoy, 3, 0);
            this.tlpKPIs.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpKPIs.Location = new System.Drawing.Point(0, 0);
            this.tlpKPIs.Margin = new System.Windows.Forms.Padding(0);
            this.tlpKPIs.Name = "tlpKPIs";
            this.tlpKPIs.Padding = new System.Windows.Forms.Padding(44, 80, 44, 16);
            this.tlpKPIs.RowCount = 1;
            this.tlpKPIs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 112));
            this.tlpKPIs.Size = new System.Drawing.Size(1270, 208);
            this.tlpKPIs.TabIndex = 1;
            // 
            // cardTotalUsuarios
            // 
            this.cardTotalUsuarios.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.cardTotalUsuarios.Controls.Add(this.lblTotalUsuariosValor);
            this.cardTotalUsuarios.Controls.Add(this.lblTotalUsuarios);
            this.cardTotalUsuarios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardTotalUsuarios.Location = new System.Drawing.Point(47, 83);
            this.cardTotalUsuarios.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.cardTotalUsuarios.Name = "cardTotalUsuarios";
            this.cardTotalUsuarios.Padding = new System.Windows.Forms.Padding(20, 14, 20, 10);
            this.cardTotalUsuarios.Size = new System.Drawing.Size(279, 68);
            this.cardTotalUsuarios.TabIndex = 0;
            // 
            // lblTotalUsuariosValor
            // 
            this.lblTotalUsuariosValor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalUsuariosValor.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblTotalUsuariosValor.ForeColor = System.Drawing.Color.FromArgb(56, 189, 248);
            this.lblTotalUsuariosValor.Location = new System.Drawing.Point(20, 34);
            this.lblTotalUsuariosValor.Name = "lblTotalUsuariosValor";
            this.lblTotalUsuariosValor.Size = new System.Drawing.Size(239, 24);
            this.lblTotalUsuariosValor.TabIndex = 0;
            this.lblTotalUsuariosValor.Text = "0";
            // 
            // lblTotalUsuarios
            // 
            this.lblTotalUsuarios.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTotalUsuarios.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblTotalUsuarios.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblTotalUsuarios.Location = new System.Drawing.Point(20, 14);
            this.lblTotalUsuarios.Name = "lblTotalUsuarios";
            this.lblTotalUsuarios.Size = new System.Drawing.Size(239, 16);
            this.lblTotalUsuarios.TabIndex = 1;
            this.lblTotalUsuarios.Text = "USUARIOS REGISTRADOS";
            this.lblTotalUsuarios.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // cardPartidasHoy
            // 
            this.cardPartidasHoy.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.cardPartidasHoy.Controls.Add(this.lblPartidasHoyValor);
            this.cardPartidasHoy.Controls.Add(this.lblPartidasHoy);
            this.cardPartidasHoy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardPartidasHoy.Location = new System.Drawing.Point(345, 83);
            this.cardPartidasHoy.Margin = new System.Windows.Forms.Padding(6, 3, 12, 3);
            this.cardPartidasHoy.Name = "cardPartidasHoy";
            this.cardPartidasHoy.Padding = new System.Windows.Forms.Padding(20, 14, 20, 10);
            this.cardPartidasHoy.Size = new System.Drawing.Size(279, 68);
            this.cardPartidasHoy.TabIndex = 4;
            // 
            // lblPartidasHoyValor
            // 
            this.lblPartidasHoyValor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPartidasHoyValor.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblPartidasHoyValor.ForeColor = System.Drawing.Color.FromArgb(250, 204, 21);
            this.lblPartidasHoyValor.Location = new System.Drawing.Point(20, 34);
            this.lblPartidasHoyValor.Name = "lblPartidasHoyValor";
            this.lblPartidasHoyValor.Size = new System.Drawing.Size(239, 24);
            this.lblPartidasHoyValor.TabIndex = 0;
            this.lblPartidasHoyValor.Text = "0";
            // 
            // lblPartidasHoy
            // 
            this.lblPartidasHoy.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPartidasHoy.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblPartidasHoy.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblPartidasHoy.Location = new System.Drawing.Point(20, 14);
            this.lblPartidasHoy.Name = "lblPartidasHoy";
            this.lblPartidasHoy.Size = new System.Drawing.Size(239, 16);
            this.lblPartidasHoy.TabIndex = 1;
            this.lblPartidasHoy.Text = "PARTIDAS HOY";
            this.lblPartidasHoy.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // cardIngresosHoy
            // 
            this.cardIngresosHoy.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.cardIngresosHoy.Controls.Add(this.lblIngresosHoyValor);
            this.cardIngresosHoy.Controls.Add(this.lblIngresosHoy);
            this.cardIngresosHoy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardIngresosHoy.Location = new System.Drawing.Point(642, 83);
            this.cardIngresosHoy.Margin = new System.Windows.Forms.Padding(6, 3, 12, 3);
            this.cardIngresosHoy.Name = "cardIngresosHoy";
            this.cardIngresosHoy.Padding = new System.Windows.Forms.Padding(20, 14, 20, 10);
            this.cardIngresosHoy.Size = new System.Drawing.Size(279, 68);
            this.cardIngresosHoy.TabIndex = 2;
            // 
            // lblIngresosHoyValor
            // 
            this.lblIngresosHoyValor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIngresosHoyValor.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblIngresosHoyValor.ForeColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.lblIngresosHoyValor.Location = new System.Drawing.Point(20, 34);
            this.lblIngresosHoyValor.Name = "lblIngresosHoyValor";
            this.lblIngresosHoyValor.Size = new System.Drawing.Size(239, 24);
            this.lblIngresosHoyValor.TabIndex = 0;
            this.lblIngresosHoyValor.Text = "$0";
            // 
            // lblIngresosHoy
            // 
            this.lblIngresosHoy.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblIngresosHoy.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblIngresosHoy.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblIngresosHoy.Location = new System.Drawing.Point(20, 14);
            this.lblIngresosHoy.Name = "lblIngresosHoy";
            this.lblIngresosHoy.Size = new System.Drawing.Size(239, 16);
            this.lblIngresosHoy.TabIndex = 1;
            this.lblIngresosHoy.Text = "INGRESOS HOY";
            this.lblIngresosHoy.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // cardGananciaCasaHoy
            // 
            this.cardGananciaCasaHoy.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.cardGananciaCasaHoy.Controls.Add(this.lblGananciaCasaHoyValor);
            this.cardGananciaCasaHoy.Controls.Add(this.lblGananciaCasaHoy);
            this.cardGananciaCasaHoy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardGananciaCasaHoy.Location = new System.Drawing.Point(949, 83);
            this.cardGananciaCasaHoy.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.cardGananciaCasaHoy.Name = "cardGananciaCasaHoy";
            this.cardGananciaCasaHoy.Padding = new System.Windows.Forms.Padding(20, 14, 20, 10);
            this.cardGananciaCasaHoy.Size = new System.Drawing.Size(318, 68);
            this.cardGananciaCasaHoy.TabIndex = 5;
            // 
            // lblGananciaCasaHoyValor
            // 
            this.lblGananciaCasaHoyValor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGananciaCasaHoyValor.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblGananciaCasaHoyValor.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this.lblGananciaCasaHoyValor.Location = new System.Drawing.Point(20, 34);
            this.lblGananciaCasaHoyValor.Name = "lblGananciaCasaHoyValor";
            this.lblGananciaCasaHoyValor.Size = new System.Drawing.Size(278, 24);
            this.lblGananciaCasaHoyValor.TabIndex = 0;
            this.lblGananciaCasaHoyValor.Text = "$0";
            // 
            // lblGananciaCasaHoy
            // 
            this.lblGananciaCasaHoy.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGananciaCasaHoy.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblGananciaCasaHoy.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblGananciaCasaHoy.Location = new System.Drawing.Point(20, 14);
            this.lblGananciaCasaHoy.Name = "lblGananciaCasaHoy";
            this.lblGananciaCasaHoy.Size = new System.Drawing.Size(278, 16);
            this.lblGananciaCasaHoy.TabIndex = 1;
            this.lblGananciaCasaHoy.Text = "GANANCIA CASA HOY";
            this.lblGananciaCasaHoy.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // chartIngresos
            // 
            this.chartIngresos.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.chartIngresos.BorderlineColor = System.Drawing.Color.Transparent;
            this.chartIngresos.Dock = System.Windows.Forms.DockStyle.Top;
            this.chartIngresos.Location = new System.Drawing.Point(44, 170);
            this.chartIngresos.Name = "chartIngresos";
            this.chartIngresos.Size = new System.Drawing.Size(1182, 480);
            this.chartIngresos.TabIndex = 0;
            // 
            // bottomTlp
            // 
            this.bottomTlp.BackColor = System.Drawing.Color.FromArgb(8, 13, 24);
            this.bottomTlp.ColumnCount = 2;
            this.bottomTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.bottomTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.bottomTlp.Controls.Add(this.chartPartidas, 0, 0);
            this.bottomTlp.Controls.Add(this.pnlTopJugadores, 1, 0);
            this.bottomTlp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomTlp.Location = new System.Drawing.Point(44, 650);
            this.bottomTlp.Name = "bottomTlp";
            this.bottomTlp.Padding = new System.Windows.Forms.Padding(0, 12, 0, 12);
            this.bottomTlp.RowCount = 1;
            this.bottomTlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bottomTlp.Size = new System.Drawing.Size(1182, 320);
            this.bottomTlp.TabIndex = 2;
            // 
            // chartPartidas
            // 
            this.chartPartidas.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.chartPartidas.BorderlineColor = System.Drawing.Color.Transparent;
            this.chartPartidas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartPartidas.Location = new System.Drawing.Point(3, 15);
            this.chartPartidas.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.chartPartidas.Name = "chartPartidas";
            this.chartPartidas.Size = new System.Drawing.Size(578, 290);
            this.chartPartidas.TabIndex = 1;
            // 
            // pnlTopJugadores
            // 
            this.pnlTopJugadores.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.pnlTopJugadores.Controls.Add(this.dgvTopJugadores);
            this.pnlTopJugadores.Controls.Add(this.lblTopJugadores);
            this.pnlTopJugadores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTopJugadores.Location = new System.Drawing.Point(594, 15);
            this.pnlTopJugadores.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.pnlTopJugadores.Name = "pnlTopJugadores";
            this.pnlTopJugadores.Padding = new System.Windows.Forms.Padding(24, 18, 24, 18);
            this.pnlTopJugadores.Size = new System.Drawing.Size(585, 290);
            this.pnlTopJugadores.TabIndex = 2;
            // 
            // dgvTopJugadores
            // 
            this.dgvTopJugadores.AllowUserToAddRows = false;
            this.dgvTopJugadores.AllowUserToDeleteRows = false;
            this.dgvTopJugadores.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTopJugadores.BackgroundColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.dgvTopJugadores.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTopJugadores.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvTopJugadores.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.dgvTopJugadores.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvTopJugadores.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.dgvTopJugadores.ColumnHeadersHeight = 34;
            this.dgvTopJugadores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvTopJugadores.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.dgvTopJugadores.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dgvTopJugadores.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.dgvTopJugadores.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dgvTopJugadores.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.dgvTopJugadores.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvTopJugadores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTopJugadores.EnableHeadersVisualStyles = false;
            this.dgvTopJugadores.GridColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.dgvTopJugadores.Location = new System.Drawing.Point(24, 48);
            this.dgvTopJugadores.Name = "dgvTopJugadores";
            this.dgvTopJugadores.ReadOnly = true;
            this.dgvTopJugadores.RowHeadersVisible = false;
            this.dgvTopJugadores.RowTemplate.Height = 30;
            this.dgvTopJugadores.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTopJugadores.Size = new System.Drawing.Size(537, 224);
            this.dgvTopJugadores.TabIndex = 0;
            // 
            // lblTopJugadores
            // 
            this.lblTopJugadores.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTopJugadores.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTopJugadores.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblTopJugadores.Location = new System.Drawing.Point(24, 18);
            this.lblTopJugadores.Name = "lblTopJugadores";
            this.lblTopJugadores.Size = new System.Drawing.Size(537, 24);
            this.lblTopJugadores.TabIndex = 1;
            this.lblTopJugadores.Text = "Mejores Jugadores";
            // 
            // pnlScroll
            // 
            this.pnlScroll.AutoScroll = true;
            this.pnlScroll.BackColor = System.Drawing.Color.FromArgb(8, 13, 24);
            this.pnlScroll.Controls.Add(this.pnlContent);
            this.pnlScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScroll.Location = new System.Drawing.Point(0, 0);
            this.pnlScroll.Name = "pnlScroll";
            this.pnlScroll.Size = new System.Drawing.Size(1316, 786);
            this.pnlScroll.TabIndex = 0;
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(8, 13, 24);
            this.pnlContent.Controls.Add(this.bottomTlp);
            this.pnlContent.Controls.Add(this.chartIngresos);
            this.pnlContent.Controls.Add(this.tlpKPIs);
            this.pnlContent.Controls.Add(this.lblTitulo);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(0, 0, 46, 0);
            this.pnlContent.Size = new System.Drawing.Size(1316, 1008);
            this.pnlContent.TabIndex = 0;
            // 
            // UcDashboardAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(8, 13, 24);
            this.Controls.Add(this.pnlScroll);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "UcDashboardAdmin";
            this.Size = new System.Drawing.Size(1316, 786);
            this.tlpKPIs.ResumeLayout(false);
            this.cardTotalUsuarios.ResumeLayout(false);
            this.cardPartidasHoy.ResumeLayout(false);
            this.cardIngresosHoy.ResumeLayout(false);
            this.cardGananciaCasaHoy.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartIngresos)).EndInit();
            this.bottomTlp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartPartidas)).EndInit();
            this.pnlTopJugadores.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopJugadores)).EndInit();
            this.pnlScroll.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.TableLayoutPanel tlpKPIs;
        private System.Windows.Forms.Panel cardTotalUsuarios;
        private System.Windows.Forms.Label lblTotalUsuarios;
        private System.Windows.Forms.Label lblTotalUsuariosValor;
        private System.Windows.Forms.Panel cardPartidasHoy;
        private System.Windows.Forms.Label lblPartidasHoy;
        private System.Windows.Forms.Label lblPartidasHoyValor;
        private System.Windows.Forms.Panel cardIngresosHoy;
        private System.Windows.Forms.Label lblIngresosHoy;
        private System.Windows.Forms.Label lblIngresosHoyValor;
        private System.Windows.Forms.Panel cardGananciaCasaHoy;
        private System.Windows.Forms.Label lblGananciaCasaHoy;
        private System.Windows.Forms.Label lblGananciaCasaHoyValor;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartIngresos;
        private System.Windows.Forms.TableLayoutPanel bottomTlp;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPartidas;
        private System.Windows.Forms.Panel pnlTopJugadores;
        private System.Windows.Forms.Label lblTopJugadores;
        private System.Windows.Forms.DataGridView dgvTopJugadores;
        private System.Windows.Forms.Panel pnlScroll;
        private System.Windows.Forms.Panel pnlContent;
    }
}
