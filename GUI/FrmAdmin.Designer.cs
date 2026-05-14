namespace GUI
{
    partial class FrmAdmin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblAdminNombre      = new System.Windows.Forms.Label();
            this.btnCerrarSesion     = new System.Windows.Forms.Button();
            this.tabControl          = new System.Windows.Forms.TabControl();
            this.tabUsuarios         = new System.Windows.Forms.TabPage();
            this.dgvUsuarios         = new System.Windows.Forms.DataGridView();
            this.btnEliminarUsuario  = new System.Windows.Forms.Button();
            this.btnRefrescar        = new System.Windows.Forms.Button();
            this.tabPartidas         = new System.Windows.Forms.TabPage();
            this.dgvPartidas         = new System.Windows.Forms.DataGridView();
            this.tabControl.SuspendLayout();
            this.tabUsuarios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            this.tabPartidas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).BeginInit();
            this.SuspendLayout();

            // lblAdminNombre
            this.lblAdminNombre.AutoSize = true;
            this.lblAdminNombre.Font     = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblAdminNombre.Location = new System.Drawing.Point(12, 12);
            this.lblAdminNombre.Name     = "lblAdminNombre";
            this.lblAdminNombre.Size     = new System.Drawing.Size(300, 20);
            this.lblAdminNombre.Text     = "Administrador";

            // btnCerrarSesion
            this.btnCerrarSesion.Location = new System.Drawing.Point(548, 10);
            this.btnCerrarSesion.Name     = "btnCerrarSesion";
            this.btnCerrarSesion.Size     = new System.Drawing.Size(110, 28);
            this.btnCerrarSesion.TabIndex = 0;
            this.btnCerrarSesion.Text     = "Cerrar sesión";
            this.btnCerrarSesion.UseVisualStyleBackColor = true;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);

            // tabControl
            this.tabControl.Controls.Add(this.tabUsuarios);
            this.tabControl.Controls.Add(this.tabPartidas);
            this.tabControl.Location = new System.Drawing.Point(12, 48);
            this.tabControl.Name     = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size     = new System.Drawing.Size(650, 380);
            this.tabControl.TabIndex = 1;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);

            // tabUsuarios
            this.tabUsuarios.Controls.Add(this.btnEliminarUsuario);
            this.tabUsuarios.Controls.Add(this.btnRefrescar);
            this.tabUsuarios.Controls.Add(this.dgvUsuarios);
            this.tabUsuarios.Location = new System.Drawing.Point(4, 24);
            this.tabUsuarios.Name     = "tabUsuarios";
            this.tabUsuarios.Size     = new System.Drawing.Size(642, 352);
            this.tabUsuarios.TabIndex = 0;
            this.tabUsuarios.Text     = "Usuarios";
            this.tabUsuarios.UseVisualStyleBackColor = true;

            // dgvUsuarios
            this.dgvUsuarios.AllowUserToAddRows    = false;
            this.dgvUsuarios.AllowUserToDeleteRows = false;
            this.dgvUsuarios.AutoSizeColumnsMode   = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsuarios.Location  = new System.Drawing.Point(6, 6);
            this.dgvUsuarios.Name      = "dgvUsuarios";
            this.dgvUsuarios.ReadOnly  = true;
            this.dgvUsuarios.Size      = new System.Drawing.Size(630, 300);
            this.dgvUsuarios.TabIndex  = 0;

            // btnEliminarUsuario
            this.btnEliminarUsuario.Location = new System.Drawing.Point(6, 314);
            this.btnEliminarUsuario.Name     = "btnEliminarUsuario";
            this.btnEliminarUsuario.Size     = new System.Drawing.Size(130, 28);
            this.btnEliminarUsuario.TabIndex = 1;
            this.btnEliminarUsuario.Text     = "Eliminar usuario";
            this.btnEliminarUsuario.UseVisualStyleBackColor = true;
            this.btnEliminarUsuario.Click += new System.EventHandler(this.btnEliminarUsuario_Click);

            // btnRefrescar
            this.btnRefrescar.Location = new System.Drawing.Point(146, 314);
            this.btnRefrescar.Name     = "btnRefrescar";
            this.btnRefrescar.Size     = new System.Drawing.Size(90, 28);
            this.btnRefrescar.TabIndex = 2;
            this.btnRefrescar.Text     = "Refrescar";
            this.btnRefrescar.UseVisualStyleBackColor = true;
            this.btnRefrescar.Click += new System.EventHandler(this.btnRefrescar_Click);

            // tabPartidas
            this.tabPartidas.Controls.Add(this.dgvPartidas);
            this.tabPartidas.Location = new System.Drawing.Point(4, 24);
            this.tabPartidas.Name     = "tabPartidas";
            this.tabPartidas.Size     = new System.Drawing.Size(642, 352);
            this.tabPartidas.TabIndex = 1;
            this.tabPartidas.Text     = "Partidas";
            this.tabPartidas.UseVisualStyleBackColor = true;

            // dgvPartidas
            this.dgvPartidas.AllowUserToAddRows    = false;
            this.dgvPartidas.AllowUserToDeleteRows = false;
            this.dgvPartidas.AutoSizeColumnsMode   = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPartidas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPartidas.Location  = new System.Drawing.Point(6, 6);
            this.dgvPartidas.Name      = "dgvPartidas";
            this.dgvPartidas.ReadOnly  = true;
            this.dgvPartidas.Size      = new System.Drawing.Size(630, 340);
            this.dgvPartidas.TabIndex  = 0;

            // FrmAdmin
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(674, 442);
            this.Controls.Add(this.lblAdminNombre);
            this.Controls.Add(this.btnCerrarSesion);
            this.Controls.Add(this.tabControl);
            this.Name          = "FrmAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text          = "Casino Virtual - Administrador";
            this.tabControl.ResumeLayout(false);
            this.tabUsuarios.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            this.tabPartidas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label        lblAdminNombre;
        private System.Windows.Forms.Button       btnCerrarSesion;
        private System.Windows.Forms.TabControl   tabControl;
        private System.Windows.Forms.TabPage      tabUsuarios;
        private System.Windows.Forms.TabPage      tabPartidas;
        private System.Windows.Forms.DataGridView dgvUsuarios;
        private System.Windows.Forms.DataGridView dgvPartidas;
        private System.Windows.Forms.Button       btnEliminarUsuario;
        private System.Windows.Forms.Button       btnRefrescar;
    }
}
