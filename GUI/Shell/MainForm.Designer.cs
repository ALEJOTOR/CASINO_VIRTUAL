namespace GUI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.panelNavbar = new System.Windows.Forms.Panel();
            this.btnCerrarSesion = new System.Windows.Forms.Button();
            this.lblSaldo = new System.Windows.Forms.Label();
            this._lblMarca = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.inicioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.historialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.billeteraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlContenido = new System.Windows.Forms.Panel();
            this.mainLayout.SuspendLayout();
            this.panelNavbar.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(14)))), ((int)(((byte)(26)))));
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainLayout.Controls.Add(this.panelNavbar, 0, 0);
            this.mainLayout.Controls.Add(this.pnlContenido, 0, 1);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 2;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Size = new System.Drawing.Size(1219, 729);
            this.mainLayout.TabIndex = 0;
            // 
            // panelNavbar
            // 
            this.panelNavbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(18)))), ((int)(((byte)(32)))));
            this.panelNavbar.Controls.Add(this.btnCerrarSesion);
            this.panelNavbar.Controls.Add(this.lblSaldo);
            this.panelNavbar.Controls.Add(this._lblMarca);
            this.panelNavbar.Controls.Add(this.menuStrip);
            this.panelNavbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNavbar.Location = new System.Drawing.Point(3, 3);
            this.panelNavbar.Name = "panelNavbar";
            this.panelNavbar.Size = new System.Drawing.Size(1213, 69);
            this.panelNavbar.TabIndex = 0;
            // 
            // btnCerrarSesion
            // 
            this.btnCerrarSesion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrarSesion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnCerrarSesion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrarSesion.FlatAppearance.BorderSize = 0;
            this.btnCerrarSesion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarSesion.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnCerrarSesion.ForeColor = System.Drawing.Color.White;
            this.btnCerrarSesion.Location = new System.Drawing.Point(1090, 12);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Size = new System.Drawing.Size(112, 30);
            this.btnCerrarSesion.TabIndex = 4;
            this.btnCerrarSesion.Text = "Cerrar sesion";
            this.btnCerrarSesion.UseVisualStyleBackColor = false;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);
            // 
            // lblSaldo
            // 
            this.lblSaldo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSaldo.AutoSize = true;
            this.lblSaldo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(36)))), ((int)(((byte)(60)))));
            this.lblSaldo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaldo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(222)))), ((int)(((byte)(128)))));
            this.lblSaldo.Location = new System.Drawing.Point(890, 22);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Padding = new System.Windows.Forms.Padding(10);
            this.lblSaldo.Size = new System.Drawing.Size(150, 39);
            this.lblSaldo.TabIndex = 2;
            this.lblSaldo.Text = "Saldo: $250.000";
            this.lblSaldo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lblMarca
            // 
            this._lblMarca.BackColor = System.Drawing.Color.Transparent;
            this._lblMarca.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Bold);
            this._lblMarca.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(204)))), ((int)(((byte)(21)))));
            this._lblMarca.Location = new System.Drawing.Point(9, 12);
            this._lblMarca.Name = "_lblMarca";
            this._lblMarca.Size = new System.Drawing.Size(170, 54);
            this._lblMarca.TabIndex = 5;
            this._lblMarca.Text = "CASINO ROYAL";
            this._lblMarca.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(18)))), ((int)(((byte)(32)))));
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inicioToolStripMenuItem,
            this.historialToolStripMenuItem,
            this.billeteraToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(190, 0, 0, 0);
            this.menuStrip.Size = new System.Drawing.Size(1213, 69);
            this.menuStrip.TabIndex = 1;
            // 
            // inicioToolStripMenuItem
            // 
            this.inicioToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.inicioToolStripMenuItem.Name = "inicioToolStripMenuItem";
            this.inicioToolStripMenuItem.Size = new System.Drawing.Size(67, 69);
            this.inicioToolStripMenuItem.Text = "Inicio";
            // 
            // historialToolStripMenuItem
            // 
            this.historialToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.historialToolStripMenuItem.Name = "historialToolStripMenuItem";
            this.historialToolStripMenuItem.Size = new System.Drawing.Size(145, 69);
            this.historialToolStripMenuItem.Text = "Transacciones";
            // 
            // billeteraToolStripMenuItem
            // 
            this.billeteraToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.billeteraToolStripMenuItem.Name = "billeteraToolStripMenuItem";
            this.billeteraToolStripMenuItem.Size = new System.Drawing.Size(90, 69);
            this.billeteraToolStripMenuItem.Text = "Billetera";
            // 
            // pnlContenido
            // 
            this.pnlContenido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(14)))), ((int)(((byte)(26)))));
            this.pnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContenido.Location = new System.Drawing.Point(3, 78);
            this.pnlContenido.Name = "pnlContenido";
            this.pnlContenido.Size = new System.Drawing.Size(1213, 648);
            this.pnlContenido.TabIndex = 1;
            // 
            // MainForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(14)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(1219, 729);
            this.Controls.Add(this.mainLayout);
            this.MinimumSize = new System.Drawing.Size(800, 670);
            this.Name = "MainForm";
            this.Text = "Casino Virtual";
            this.mainLayout.ResumeLayout(false);
            this.panelNavbar.ResumeLayout(false);
            this.panelNavbar.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Panel pnlContenido;
        private System.Windows.Forms.Panel panelNavbar;
        private System.Windows.Forms.Label _lblMarca;
        private System.Windows.Forms.Button btnCerrarSesion;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem inicioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem historialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem billeteraToolStripMenuItem;
    }
}
