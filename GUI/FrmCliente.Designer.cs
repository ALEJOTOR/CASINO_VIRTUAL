namespace GUI
{
    partial class FrmCliente
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.pnlTop          = new System.Windows.Forms.Panel();
            this.lblBienvenida   = new System.Windows.Forms.Label();
            this.lblSaldo        = new System.Windows.Forms.Label();
            this.btnCerrarSesion = new System.Windows.Forms.Button();
            this.pnlAcciones     = new System.Windows.Forms.Panel();
            this.lblMonto        = new System.Windows.Forms.Label();
            this.txtMonto        = new System.Windows.Forms.TextBox();
            this.btnDepositar    = new System.Windows.Forms.Button();
            this.btnJugarMinas   = new System.Windows.Forms.Button();
            this.tabControl      = new System.Windows.Forms.TabControl();
            this.tabPartidas     = new System.Windows.Forms.TabPage();
            this.dgvPartidas     = new System.Windows.Forms.DataGridView();
            this.tabTransacciones = new System.Windows.Forms.TabPage();
            this.dgvTransacciones = new System.Windows.Forms.DataGridView();
            this.pnlTop.SuspendLayout();
            this.pnlAcciones.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPartidas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).BeginInit();
            this.tabTransacciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransacciones)).BeginInit();
            this.SuspendLayout();

            // pnlTop
            this.pnlTop.Dock     = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height   = 56;
            this.pnlTop.Controls.Add(this.lblBienvenida);
            this.pnlTop.Controls.Add(this.lblSaldo);
            this.pnlTop.Controls.Add(this.btnCerrarSesion);

            // lblBienvenida
            this.lblBienvenida.Font     = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblBienvenida.Location = new System.Drawing.Point(8, 6);
            this.lblBienvenida.Size     = new System.Drawing.Size(400, 22);
            this.lblBienvenida.Text     = "Bienvenido";

            // lblSaldo
            this.lblSaldo.Font     = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSaldo.Location = new System.Drawing.Point(8, 30);
            this.lblSaldo.Size     = new System.Drawing.Size(300, 20);
            this.lblSaldo.Text     = "Saldo disponible: $0.00";

            // btnCerrarSesion
            this.btnCerrarSesion.Anchor   = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnCerrarSesion.Size     = new System.Drawing.Size(120, 30);
            this.btnCerrarSesion.Location = new System.Drawing.Point(520, 12);
            this.btnCerrarSesion.Text     = "Cerrar sesión";
            this.btnCerrarSesion.UseVisualStyleBackColor = true;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);

            // pnlAcciones
            this.pnlAcciones.Dock   = System.Windows.Forms.DockStyle.Top;
            this.pnlAcciones.Height = 46;
            this.pnlAcciones.Controls.Add(this.lblMonto);
            this.pnlAcciones.Controls.Add(this.txtMonto);
            this.pnlAcciones.Controls.Add(this.btnDepositar);
            this.pnlAcciones.Controls.Add(this.btnJugarMinas);

            // lblMonto
            this.lblMonto.AutoSize = true;
            this.lblMonto.Location = new System.Drawing.Point(8, 14);
            this.lblMonto.Text     = "Monto a depositar:";

            // txtMonto
            this.txtMonto.Location = new System.Drawing.Point(150, 11);
            this.txtMonto.Size     = new System.Drawing.Size(110, 22);

            // btnDepositar
            this.btnDepositar.Location = new System.Drawing.Point(270, 9);
            this.btnDepositar.Size     = new System.Drawing.Size(100, 28);
            this.btnDepositar.Text     = "Depositar";
            this.btnDepositar.UseVisualStyleBackColor = true;
            this.btnDepositar.Click += new System.EventHandler(this.btnDepositar_Click);

            // btnJugarMinas
            this.btnJugarMinas.Location = new System.Drawing.Point(380, 9);
            this.btnJugarMinas.Size     = new System.Drawing.Size(130, 28);
            this.btnJugarMinas.Text     = "Jugar Minas";
            this.btnJugarMinas.UseVisualStyleBackColor = true;
            this.btnJugarMinas.Click += new System.EventHandler(this.btnJugarMinas_Click);

            // tabControl
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Controls.Add(this.tabPartidas);
            this.tabControl.Controls.Add(this.tabTransacciones);
            this.tabControl.SelectedIndex = 0;

            // tabPartidas
            this.tabPartidas.Controls.Add(this.dgvPartidas);
            this.tabPartidas.Text = "Mis partidas";

            // dgvPartidas
            this.dgvPartidas.AllowUserToAddRows    = false;
            this.dgvPartidas.AllowUserToDeleteRows = false;
            this.dgvPartidas.AutoSizeColumnsMode   = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPartidas.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.dgvPartidas.ReadOnly  = true;

            // tabTransacciones
            this.tabTransacciones.Controls.Add(this.dgvTransacciones);
            this.tabTransacciones.Text = "Mis transacciones";

            // dgvTransacciones
            this.dgvTransacciones.AllowUserToAddRows    = false;
            this.dgvTransacciones.AllowUserToDeleteRows = false;
            this.dgvTransacciones.AutoSizeColumnsMode   = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTransacciones.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.dgvTransacciones.ReadOnly  = true;

            // FrmCliente
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(660, 480);
            this.MinimumSize         = new System.Drawing.Size(500, 380);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlAcciones);
            this.Controls.Add(this.pnlTop);
            this.Name          = "FrmCliente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text          = "Casino Virtual - Cliente";
            this.pnlTop.ResumeLayout(false);
            this.pnlAcciones.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPartidas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).EndInit();
            this.tabTransacciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransacciones)).EndInit();
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.Panel          pnlTop;
        private System.Windows.Forms.Panel          pnlAcciones;
        private System.Windows.Forms.Label          lblBienvenida;
        private System.Windows.Forms.Label          lblSaldo;
        private System.Windows.Forms.Label          lblMonto;
        private System.Windows.Forms.TextBox        txtMonto;
        private System.Windows.Forms.Button         btnDepositar;
        private System.Windows.Forms.Button         btnJugarMinas;
        private System.Windows.Forms.Button         btnCerrarSesion;
        private System.Windows.Forms.TabControl     tabControl;
        private System.Windows.Forms.TabPage        tabPartidas;
        private System.Windows.Forms.TabPage        tabTransacciones;
        private System.Windows.Forms.DataGridView   dgvPartidas;
        private System.Windows.Forms.DataGridView   dgvTransacciones;
    }
}
