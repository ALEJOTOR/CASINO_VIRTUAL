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
            this.lblAdminNombre = new System.Windows.Forms.Label();
            this.btnCerrarSesion = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabUsuarios = new System.Windows.Forms.TabPage();
            this.dgvUsuarios = new System.Windows.Forms.DataGridView();
            this.btnEliminarUsuario = new System.Windows.Forms.Button();
            this.btnSuspender = new System.Windows.Forms.Button();
            this.btnModificarSaldo = new System.Windows.Forms.Button();
            this.btnCambiarPassword = new System.Windows.Forms.Button();
            this.btnRefrescar = new System.Windows.Forms.Button();
            this.tabPartidas = new System.Windows.Forms.TabPage();
            this.dgvPartidas = new System.Windows.Forms.DataGridView();
            this.tabTransacciones = new System.Windows.Forms.TabPage();
            this.dgvTransacciones = new System.Windows.Forms.DataGridView();
            this.tabReportes = new System.Windows.Forms.TabPage();
            this.btnReporteUsuarios = new System.Windows.Forms.Button();
            this.btnReportePartidas = new System.Windows.Forms.Button();
            this.txtReporte = new System.Windows.Forms.TextBox();
            this.tabControl.SuspendLayout();
            this.tabUsuarios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            this.tabPartidas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).BeginInit();
            this.tabTransacciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransacciones)).BeginInit();
            this.tabReportes.SuspendLayout();
            this.SuspendLayout();
            // lblAdminNombre
            this.lblAdminNombre.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblAdminNombre.Location = new System.Drawing.Point(12, 12);
            this.lblAdminNombre.Name = "lblAdminNombre";
            this.lblAdminNombre.Size = new System.Drawing.Size(460, 22);
            this.lblAdminNombre.TabIndex = 0;
            this.lblAdminNombre.Text = "Administrador";
            // btnCerrarSesion
            this.btnCerrarSesion.Location = new System.Drawing.Point(578, 10);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Size = new System.Drawing.Size(110, 28);
            this.btnCerrarSesion.TabIndex = 1;
            this.btnCerrarSesion.Text = "Cerrar sesion";
            this.btnCerrarSesion.UseVisualStyleBackColor = true;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);
            // tabControl
            this.tabControl.Controls.Add(this.tabUsuarios);
            this.tabControl.Controls.Add(this.tabPartidas);
            this.tabControl.Controls.Add(this.tabTransacciones);
            this.tabControl.Controls.Add(this.tabReportes);
            this.tabControl.Location = new System.Drawing.Point(12, 46);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(678, 430);
            this.tabControl.TabIndex = 2;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // tabUsuarios
            this.tabUsuarios.Controls.Add(this.dgvUsuarios);
            this.tabUsuarios.Controls.Add(this.btnEliminarUsuario);
            this.tabUsuarios.Controls.Add(this.btnSuspender);
            this.tabUsuarios.Controls.Add(this.btnModificarSaldo);
            this.tabUsuarios.Controls.Add(this.btnCambiarPassword);
            this.tabUsuarios.Controls.Add(this.btnRefrescar);
            this.tabUsuarios.Location = new System.Drawing.Point(4, 22);
            this.tabUsuarios.Name = "tabUsuarios";
            this.tabUsuarios.Size = new System.Drawing.Size(670, 404);
            this.tabUsuarios.TabIndex = 0;
            this.tabUsuarios.Text = "Usuarios";
            this.tabUsuarios.UseVisualStyleBackColor = true;
            // dgvUsuarios
            this.dgvUsuarios.AllowUserToAddRows = false;
            this.dgvUsuarios.AllowUserToDeleteRows = false;
            this.dgvUsuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsuarios.Location = new System.Drawing.Point(6, 6);
            this.dgvUsuarios.Name = "dgvUsuarios";
            this.dgvUsuarios.ReadOnly = true;
            this.dgvUsuarios.Size = new System.Drawing.Size(658, 352);
            this.dgvUsuarios.TabIndex = 0;
            // btnEliminarUsuario
            this.btnEliminarUsuario.Location = new System.Drawing.Point(6, 364);
            this.btnEliminarUsuario.Name = "btnEliminarUsuario";
            this.btnEliminarUsuario.Size = new System.Drawing.Size(100, 28);
            this.btnEliminarUsuario.TabIndex = 1;
            this.btnEliminarUsuario.Text = "Eliminar";
            this.btnEliminarUsuario.UseVisualStyleBackColor = true;
            this.btnEliminarUsuario.Click += new System.EventHandler(this.btnEliminarUsuario_Click);
            // btnSuspender
            this.btnSuspender.Location = new System.Drawing.Point(116, 364);
            this.btnSuspender.Name = "btnSuspender";
            this.btnSuspender.Size = new System.Drawing.Size(135, 28);
            this.btnSuspender.TabIndex = 2;
            this.btnSuspender.Text = "Suspender/Activar";
            this.btnSuspender.UseVisualStyleBackColor = true;
            this.btnSuspender.Click += new System.EventHandler(this.btnSuspender_Click);
            // btnModificarSaldo
            this.btnModificarSaldo.Location = new System.Drawing.Point(261, 364);
            this.btnModificarSaldo.Name = "btnModificarSaldo";
            this.btnModificarSaldo.Size = new System.Drawing.Size(120, 28);
            this.btnModificarSaldo.TabIndex = 3;
            this.btnModificarSaldo.Text = "Modificar saldo";
            this.btnModificarSaldo.UseVisualStyleBackColor = true;
            this.btnModificarSaldo.Click += new System.EventHandler(this.btnModificarSaldo_Click);
            // btnCambiarPassword
            this.btnCambiarPassword.Location = new System.Drawing.Point(391, 364);
            this.btnCambiarPassword.Name = "btnCambiarPassword";
            this.btnCambiarPassword.Size = new System.Drawing.Size(135, 28);
            this.btnCambiarPassword.TabIndex = 4;
            this.btnCambiarPassword.Text = "Cambiar contrasena";
            this.btnCambiarPassword.UseVisualStyleBackColor = true;
            this.btnCambiarPassword.Click += new System.EventHandler(this.btnCambiarPassword_Click);
            // btnRefrescar
            this.btnRefrescar.Location = new System.Drawing.Point(536, 364);
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Size = new System.Drawing.Size(80, 28);
            this.btnRefrescar.TabIndex = 5;
            this.btnRefrescar.Text = "Refrescar";
            this.btnRefrescar.UseVisualStyleBackColor = true;
            this.btnRefrescar.Click += new System.EventHandler(this.btnRefrescar_Click);
            // tabPartidas
            this.tabPartidas.Controls.Add(this.dgvPartidas);
            this.tabPartidas.Location = new System.Drawing.Point(4, 22);
            this.tabPartidas.Name = "tabPartidas";
            this.tabPartidas.Size = new System.Drawing.Size(670, 404);
            this.tabPartidas.TabIndex = 1;
            this.tabPartidas.Text = "Partidas";
            this.tabPartidas.UseVisualStyleBackColor = true;
            // dgvPartidas
            this.dgvPartidas.AllowUserToAddRows = false;
            this.dgvPartidas.AllowUserToDeleteRows = false;
            this.dgvPartidas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPartidas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPartidas.Name = "dgvPartidas";
            this.dgvPartidas.ReadOnly = true;
            this.dgvPartidas.TabIndex = 0;
            // tabTransacciones
            this.tabTransacciones.Controls.Add(this.dgvTransacciones);
            this.tabTransacciones.Location = new System.Drawing.Point(4, 22);
            this.tabTransacciones.Name = "tabTransacciones";
            this.tabTransacciones.Size = new System.Drawing.Size(670, 404);
            this.tabTransacciones.TabIndex = 2;
            this.tabTransacciones.Text = "Transacciones";
            this.tabTransacciones.UseVisualStyleBackColor = true;
            // dgvTransacciones
            this.dgvTransacciones.AllowUserToAddRows = false;
            this.dgvTransacciones.AllowUserToDeleteRows = false;
            this.dgvTransacciones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTransacciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTransacciones.Name = "dgvTransacciones";
            this.dgvTransacciones.ReadOnly = true;
            this.dgvTransacciones.TabIndex = 0;
            // tabReportes
            this.tabReportes.Controls.Add(this.btnReporteUsuarios);
            this.tabReportes.Controls.Add(this.btnReportePartidas);
            this.tabReportes.Controls.Add(this.txtReporte);
            this.tabReportes.Location = new System.Drawing.Point(4, 22);
            this.tabReportes.Name = "tabReportes";
            this.tabReportes.Size = new System.Drawing.Size(670, 404);
            this.tabReportes.TabIndex = 3;
            this.tabReportes.Text = "Reportes";
            this.tabReportes.UseVisualStyleBackColor = true;
            // btnReporteUsuarios
            this.btnReporteUsuarios.Location = new System.Drawing.Point(6, 6);
            this.btnReporteUsuarios.Name = "btnReporteUsuarios";
            this.btnReporteUsuarios.Size = new System.Drawing.Size(150, 30);
            this.btnReporteUsuarios.TabIndex = 0;
            this.btnReporteUsuarios.Text = "Reporte Usuarios";
            this.btnReporteUsuarios.UseVisualStyleBackColor = true;
            this.btnReporteUsuarios.Click += new System.EventHandler(this.btnReporteUsuarios_Click);
            // btnReportePartidas
            this.btnReportePartidas.Location = new System.Drawing.Point(166, 6);
            this.btnReportePartidas.Name = "btnReportePartidas";
            this.btnReportePartidas.Size = new System.Drawing.Size(150, 30);
            this.btnReportePartidas.TabIndex = 1;
            this.btnReportePartidas.Text = "Reporte Partidas";
            this.btnReportePartidas.UseVisualStyleBackColor = true;
            this.btnReportePartidas.Click += new System.EventHandler(this.btnReportePartidas_Click);
            // txtReporte
            this.txtReporte.Font = new System.Drawing.Font("Courier New", 10F);
            this.txtReporte.Location = new System.Drawing.Point(6, 46);
            this.txtReporte.Multiline = true;
            this.txtReporte.Name = "txtReporte";
            this.txtReporte.ReadOnly = true;
            this.txtReporte.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReporte.Size = new System.Drawing.Size(658, 352);
            this.txtReporte.TabIndex = 2;
            // FrmAdmin
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 492);
            this.Controls.Add(this.lblAdminNombre);
            this.Controls.Add(this.btnCerrarSesion);
            this.Controls.Add(this.tabControl);
            this.Name = "FrmAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Casino Virtual - Administrador";
            this.tabControl.ResumeLayout(false);
            this.tabUsuarios.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            this.tabPartidas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).EndInit();
            this.tabTransacciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransacciones)).EndInit();
            this.tabReportes.ResumeLayout(false);
            this.tabReportes.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.Label lblAdminNombre;
        private System.Windows.Forms.Button btnCerrarSesion;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabUsuarios;
        private System.Windows.Forms.DataGridView dgvUsuarios;
        private System.Windows.Forms.Button btnEliminarUsuario;
        private System.Windows.Forms.Button btnSuspender;
        private System.Windows.Forms.Button btnModificarSaldo;
        private System.Windows.Forms.Button btnCambiarPassword;
        private System.Windows.Forms.Button btnRefrescar;
        private System.Windows.Forms.TabPage tabPartidas;
        private System.Windows.Forms.DataGridView dgvPartidas;
        private System.Windows.Forms.TabPage tabTransacciones;
        private System.Windows.Forms.DataGridView dgvTransacciones;
        private System.Windows.Forms.TabPage tabReportes;
        private System.Windows.Forms.Button btnReporteUsuarios;
        private System.Windows.Forms.Button btnReportePartidas;
        private System.Windows.Forms.TextBox txtReporte;
    }
}
