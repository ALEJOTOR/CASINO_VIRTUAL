namespace GUI
{
    partial class UcAdminGrid
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
            this.dgvDatos = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitulo.Font = new System.Drawing.Font("Georgia", 24F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(250, 204, 21);
            this.lblTitulo.Location = new System.Drawing.Point(34, 24);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(932, 58);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Titulo";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvDatos
            // 
            this.dgvDatos.AllowUserToAddRows = false;
            this.dgvDatos.AllowUserToDeleteRows = false;
            this.dgvDatos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDatos.BackgroundColor = System.Drawing.Color.FromArgb(8, 13, 24);
            this.dgvDatos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDatos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDatos.Location = new System.Drawing.Point(34, 82);
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.ReadOnly = true;
            this.dgvDatos.RowHeadersVisible = false;
            this.dgvDatos.RowTemplate.Height = 38;
            this.dgvDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvDatos.ColumnHeadersHeight = 44;
            this.dgvDatos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDatos.Size = new System.Drawing.Size(932, 484);
            this.dgvDatos.TabIndex = 1;
            this.dgvDatos.EnableHeadersVisualStyles = false;
            this.dgvDatos.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(23, 37, 63);
            this.dgvDatos.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.dgvDatos.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.dgvDatos.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvDatos.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.dgvDatos.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.dgvDatos.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dgvDatos.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.dgvDatos.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.dgvDatos.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvDatos.GridColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.dgvDatos.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(23, 37, 63);
            this.dgvDatos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            // 
            // UcAdminGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(8, 13, 24);
            this.Controls.Add(this.dgvDatos);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "UcAdminGrid";
            this.Padding = new System.Windows.Forms.Padding(34, 24, 34, 34);
            this.Size = new System.Drawing.Size(1000, 600);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvDatos;
    }
}
