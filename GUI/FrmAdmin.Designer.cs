namespace GUI
{
    partial class FrmAdmin
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
            this.pnlHeader       = new System.Windows.Forms.Panel();
            this.lblAdminNombre  = new System.Windows.Forms.Label();
            this.btnCerrarSesion = new System.Windows.Forms.Button();
            this.tabControl      = new System.Windows.Forms.TabControl();
            this.tabUsuarios     = new System.Windows.Forms.TabPage();
            this.dgvUsuarios     = new System.Windows.Forms.DataGridView();
            this.pnlBotonesUsr   = new System.Windows.Forms.Panel();
            this.btnEliminarUsuario  = new System.Windows.Forms.Button();
            this.btnSuspender        = new System.Windows.Forms.Button();
            this.btnModificarSaldo   = new System.Windows.Forms.Button();
            this.btnCambiarPassword  = new System.Windows.Forms.Button();
            this.btnRefrescar        = new System.Windows.Forms.Button();
            this.tabPartidas     = new System.Windows.Forms.TabPage();
            this.dgvPartidas     = new System.Windows.Forms.DataGridView();
            this.tabTransacciones = new System.Windows.Forms.TabPage();
            this.dgvTransacciones = new System.Windows.Forms.DataGridView();
            this.tabReportes     = new System.Windows.Forms.TabPage();
            this.pnlReporteBtns  = new System.Windows.Forms.Panel();
            this.btnReporteUsuarios = new System.Windows.Forms.Button();
            this.btnReportePartidas = new System.Windows.Forms.Button();
            this.txtReporte      = new System.Windows.Forms.TextBox();
            this.pnlHeader.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabUsuarios.SuspendLayout();
            this.pnlBotonesUsr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            this.tabPartidas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).BeginInit();
            this.tabTransacciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransacciones)).BeginInit();
            this.tabReportes.SuspendLayout();
            this.pnlReporteBtns.SuspendLayout();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.Dock   = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 44;
            this.pnlHeader.Controls.Add(this.lblAdminNombre);
            this.pnlHeader.Controls.Add(this.btnCerrarSesion);

            // lblAdminNombre
            this.lblAdminNombre.Font     = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblAdminNombre.Location = new System.Drawing.Point(8, 10);
            this.lblAdminNombre.Size     = new System.Drawing.Size(460, 22);
            this.lblAdminNombre.Text     = "Administrador";

            // btnCerrarSesion
            this.btnCerrarSesion.Anchor   = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnCerrarSesion.Size     = new System.Drawing.Size(120, 28);
            this.btnCerrarSesion.Location = new System.Drawing.Point(600, 8);
            this.btnCerrarSesion.Text     = "Cerrar sesión";
            this.btnCerrarSesion.UseVisualStyleBackColor = true;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);

            // tabControl
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Controls.Add(this.tabUsuarios);
            this.tabControl.Controls.Add(this.tabPartidas);
            this.tabControl.Controls.Add(this.tabTransacciones);
            this.tabControl.Controls.Add(this.tabReportes);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);

            // ── Tab Usuarios ────────────────────────────────────────────
            this.tabUsuarios.Text = "Usuarios";
            this.tabUsuarios.Controls.Add(this.dgvUsuarios);
            this.tabUsuarios.Controls.Add(this.pnlBotonesUsr);

            this.dgvUsuarios.AllowUserToAddRows    = false;
            this.dgvUsuarios.AllowUserToDeleteRows = false;
            this.dgvUsuarios.AutoSizeColumnsMode   = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsuarios.Dock                  = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsuarios.ReadOnly              = true;

            this.pnlBotonesUsr.Dock   = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBotonesUsr.Height = 40;
            this.pnlBotonesUsr.Controls.Add(this.btnEliminarUsuario);
            this.pnlBotonesUsr.Controls.Add(this.btnSuspender);
            this.pnlBotonesUsr.Controls.Add(this.btnModificarSaldo);
            this.pnlBotonesUsr.Controls.Add(this.btnCambiarPassword);
            this.pnlBotonesUsr.Controls.Add(this.btnRefrescar);

            this.btnEliminarUsuario.Location = new System.Drawing.Point(4, 6);
            this.btnEliminarUsuario.Size     = new System.Drawing.Size(100, 28);
            this.btnEliminarUsuario.Text     = "Eliminar";
            this.btnEliminarUsuario.UseVisualStyleBackColor = true;
            this.btnEliminarUsuario.Click += new System.EventHandler(this.btnEliminarUsuario_Click);

            this.btnSuspender.Location = new System.Drawing.Point(112, 6);
            this.btnSuspender.Size     = new System.Drawing.Size(140, 28);
            this.btnSuspender.Text     = "Suspender/Activar";
            this.btnSuspender.UseVisualStyleBackColor = true;
            this.btnSuspender.Click += new System.EventHandler(this.btnSuspender_Click);

            this.btnModificarSaldo.Location = new System.Drawing.Point(260, 6);
            this.btnModificarSaldo.Size     = new System.Drawing.Size(130, 28);
            this.btnModificarSaldo.Text     = "Modificar saldo";
            this.btnModificarSaldo.UseVisualStyleBackColor = true;
            this.btnModificarSaldo.Click += new System.EventHandler(this.btnModificarSaldo_Click);

            this.btnCambiarPassword.Location = new System.Drawing.Point(398, 6);
            this.btnCambiarPassword.Size     = new System.Drawing.Size(145, 28);
            this.btnCambiarPassword.Text     = "Cambiar contraseña";
            this.btnCambiarPassword.UseVisualStyleBackColor = true;
            this.btnCambiarPassword.Click += new System.EventHandler(this.btnCambiarPassword_Click);

            this.btnRefrescar.Anchor   = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnRefrescar.Location = new System.Drawing.Point(551, 6);
            this.btnRefrescar.Size     = new System.Drawing.Size(90, 28);
            this.btnRefrescar.Text     = "Refrescar";
            this.btnRefrescar.UseVisualStyleBackColor = true;
            this.btnRefrescar.Click += new System.EventHandler(this.btnRefrescar_Click);

            // ── Tab Partidas ────────────────────────────────────────────
            this.tabPartidas.Text = "Partidas";
            this.tabPartidas.Controls.Add(this.dgvPartidas);

            this.dgvPartidas.AllowUserToAddRows    = false;
            this.dgvPartidas.AllowUserToDeleteRows = false;
            this.dgvPartidas.AutoSizeColumnsMode   = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPartidas.Dock                  = System.Windows.Forms.DockStyle.Fill;
            this.dgvPartidas.ReadOnly              = true;

            // ── Tab Transacciones ───────────────────────────────────────
            this.tabTransacciones.Text = "Transacciones";
            this.tabTransacciones.Controls.Add(this.dgvTransacciones);

            this.dgvTransacciones.AllowUserToAddRows    = false;
            this.dgvTransacciones.AllowUserToDeleteRows = false;
            this.dgvTransacciones.AutoSizeColumnsMode   = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTransacciones.Dock                  = System.Windows.Forms.DockStyle.Fill;
            this.dgvTransacciones.ReadOnly              = true;

            // ── Tab Reportes ────────────────────────────────────────────
            this.tabReportes.Text = "Reportes";
            this.tabReportes.Controls.Add(this.txtReporte);
            this.tabReportes.Controls.Add(this.pnlReporteBtns);

            this.pnlReporteBtns.Dock   = System.Windows.Forms.DockStyle.Top;
            this.pnlReporteBtns.Height = 42;
            this.pnlReporteBtns.Controls.Add(this.btnReporteUsuarios);
            this.pnlReporteBtns.Controls.Add(this.btnReportePartidas);

            this.btnReporteUsuarios.Location = new System.Drawing.Point(6, 6);
            this.btnReporteUsuarios.Size     = new System.Drawing.Size(160, 30);
            this.btnReporteUsuarios.Text     = "Reporte Usuarios";
            this.btnReporteUsuarios.UseVisualStyleBackColor = true;
            this.btnReporteUsuarios.Click += new System.EventHandler(this.btnReporteUsuarios_Click);

            this.btnReportePartidas.Location = new System.Drawing.Point(174, 6);
            this.btnReportePartidas.Size     = new System.Drawing.Size(160, 30);
            this.btnReportePartidas.Text     = "Reporte Partidas";
            this.btnReportePartidas.UseVisualStyleBackColor = true;
            this.btnReportePartidas.Click += new System.EventHandler(this.btnReportePartidas_Click);

            this.txtReporte.Dock       = System.Windows.Forms.DockStyle.Fill;
            this.txtReporte.Font       = new System.Drawing.Font("Courier New", 10F);
            this.txtReporte.Multiline  = true;
            this.txtReporte.ReadOnly   = true;
            this.txtReporte.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            // FrmAdmin
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(740, 530);
            this.MinimumSize         = new System.Drawing.Size(600, 430);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlHeader);
            this.Name          = "FrmAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text          = "Casino Virtual - Administrador";
            this.pnlHeader.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabUsuarios.ResumeLayout(false);
            this.pnlBotonesUsr.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            this.tabPartidas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).EndInit();
            this.tabTransacciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransacciones)).EndInit();
            this.tabReportes.ResumeLayout(false);
            this.pnlReporteBtns.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.Panel          pnlHeader;
        private System.Windows.Forms.Label          lblAdminNombre;
        private System.Windows.Forms.Button         btnCerrarSesion;
        private System.Windows.Forms.TabControl     tabControl;
        private System.Windows.Forms.TabPage        tabUsuarios;
        private System.Windows.Forms.DataGridView   dgvUsuarios;
        private System.Windows.Forms.Panel          pnlBotonesUsr;
        private System.Windows.Forms.Button         btnEliminarUsuario;
        private System.Windows.Forms.Button         btnSuspender;
        private System.Windows.Forms.Button         btnModificarSaldo;
        private System.Windows.Forms.Button         btnCambiarPassword;
        private System.Windows.Forms.Button         btnRefrescar;
        private System.Windows.Forms.TabPage        tabPartidas;
        private System.Windows.Forms.DataGridView   dgvPartidas;
        private System.Windows.Forms.TabPage        tabTransacciones;
        private System.Windows.Forms.DataGridView   dgvTransacciones;
        private System.Windows.Forms.TabPage        tabReportes;
        private System.Windows.Forms.Panel          pnlReporteBtns;
        private System.Windows.Forms.Button         btnReporteUsuarios;
        private System.Windows.Forms.Button         btnReportePartidas;
        private System.Windows.Forms.TextBox        txtReporte;
    }
}
