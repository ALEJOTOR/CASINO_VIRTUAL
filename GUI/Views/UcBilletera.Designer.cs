namespace GUI
{
    partial class UcBilletera
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlMetricas = new System.Windows.Forms.TableLayoutPanel();
            this.pnlAcciones = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // pnlMetricas
            // 
            this.pnlMetricas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(13)))), ((int)(((byte)(24)))));
            this.pnlMetricas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlMetricas.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMetricas.Location = new System.Drawing.Point(0, 0);
            this.pnlMetricas.Margin = new System.Windows.Forms.Padding(0);
            this.pnlMetricas.Name = "pnlMetricas";
            this.pnlMetricas.Size = new System.Drawing.Size(1000, 126);
            this.pnlMetricas.TabIndex = 0;
            // 
            // pnlAcciones
            // 
            this.pnlAcciones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(13)))), ((int)(((byte)(24)))));
            this.pnlAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlAcciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAcciones.Location = new System.Drawing.Point(0, 126);
            this.pnlAcciones.Name = "pnlAcciones";
            this.pnlAcciones.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlAcciones.Size = new System.Drawing.Size(1000, 474);
            this.pnlAcciones.TabIndex = 1;
            // 
            // UcBilletera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(13)))), ((int)(((byte)(24)))));
            this.Controls.Add(this.pnlAcciones);
            this.Controls.Add(this.pnlMetricas);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "UcBilletera";
            this.Size = new System.Drawing.Size(1000, 600);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TableLayoutPanel pnlMetricas;
        private System.Windows.Forms.TableLayoutPanel pnlAcciones;
    }
}
