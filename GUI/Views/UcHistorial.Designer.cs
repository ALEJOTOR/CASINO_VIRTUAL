namespace GUI
{
    partial class UcHistorial
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.btnDepositos = new System.Windows.Forms.Button();
            this.btnApuestas = new System.Windows.Forms.Button();
            this.btnTodos = new System.Windows.Forms.Button();
            this.pnlContenido = new System.Windows.Forms.Panel();
            this.pnlSidebar.SuspendLayout();
            this.SuspendLayout();

            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.pnlSidebar.Controls.Add(this.btnDepositos);
            this.pnlSidebar.Controls.Add(this.btnApuestas);
            this.pnlSidebar.Controls.Add(this.btnTodos);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlSidebar.Size = new System.Drawing.Size(176, 600);
            this.pnlSidebar.TabIndex = 0;

            this.btnDepositos.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.btnDepositos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDepositos.FlatAppearance.BorderSize = 0;
            this.btnDepositos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDepositos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDepositos.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.btnDepositos.Height = 48;
            this.btnDepositos.Name = "btnDepositos";
            this.btnDepositos.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.btnDepositos.Text = "Depositos";
            this.btnDepositos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDepositos.UseVisualStyleBackColor = false;
            this.btnDepositos.Click += new System.EventHandler(this.btnDepositos_Click);

            this.btnApuestas.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.btnApuestas.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnApuestas.FlatAppearance.BorderSize = 0;
            this.btnApuestas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApuestas.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnApuestas.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.btnApuestas.Height = 48;
            this.btnApuestas.Name = "btnApuestas";
            this.btnApuestas.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.btnApuestas.Text = "Apuestas";
            this.btnApuestas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApuestas.UseVisualStyleBackColor = false;
            this.btnApuestas.Click += new System.EventHandler(this.btnApuestas_Click);

            this.btnTodos.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.btnTodos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTodos.FlatAppearance.BorderSize = 0;
            this.btnTodos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTodos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTodos.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.btnTodos.Height = 48;
            this.btnTodos.Name = "btnTodos";
            this.btnTodos.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.btnTodos.Text = "Todos";
            this.btnTodos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTodos.UseVisualStyleBackColor = false;
            this.btnTodos.Click += new System.EventHandler(this.btnTodos_Click);

            this.pnlContenido.BackColor = System.Drawing.Color.FromArgb(8, 13, 24);
            this.pnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContenido.Location = new System.Drawing.Point(176, 0);
            this.pnlContenido.Name = "pnlContenido";
            this.pnlContenido.Size = new System.Drawing.Size(824, 600);
            this.pnlContenido.TabIndex = 1;

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(8, 13, 24);
            this.Controls.Add(this.pnlContenido);
            this.Controls.Add(this.pnlSidebar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "UcHistorial";
            this.Size = new System.Drawing.Size(1000, 600);
            this.pnlSidebar.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Button btnTodos;
        private System.Windows.Forms.Button btnApuestas;
        private System.Windows.Forms.Button btnDepositos;
        private System.Windows.Forms.Panel pnlContenido;
    }
}
