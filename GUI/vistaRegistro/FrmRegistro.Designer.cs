namespace GUI
{
    partial class FrmRegistro
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
            this.lblUsername  = new System.Windows.Forms.Label();
            this.lblPassword  = new System.Windows.Forms.Label();
            this.lblNombre1   = new System.Windows.Forms.Label();
            this.lblNombre2   = new System.Windows.Forms.Label();
            this.lblApellido1 = new System.Windows.Forms.Label();
            this.lblApellido2 = new System.Windows.Forms.Label();
            this.lblCorreo    = new System.Windows.Forms.Label();
            this.lblFechaNac  = new System.Windows.Forms.Label();
            this.txtUsername  = new System.Windows.Forms.TextBox();
            this.txtPassword  = new System.Windows.Forms.TextBox();
            this.txtNombre1   = new System.Windows.Forms.TextBox();
            this.txtNombre2   = new System.Windows.Forms.TextBox();
            this.txtApellido1 = new System.Windows.Forms.TextBox();
            this.txtApellido2 = new System.Windows.Forms.TextBox();
            this.txtCorreo    = new System.Windows.Forms.TextBox();
            this.dtpFechaNac  = new System.Windows.Forms.DateTimePicker();
            this.btnGuardar   = new System.Windows.Forms.Button();
            this.btnCancelar  = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.lblUsername.AutoSize  = true; this.lblUsername.Location  = new System.Drawing.Point(20, 18);  this.lblUsername.Text  = "Usuario:";
            this.lblPassword.AutoSize  = true; this.lblPassword.Location  = new System.Drawing.Point(20, 53);  this.lblPassword.Text  = "Contraseña:";
            this.lblNombre1.AutoSize   = true; this.lblNombre1.Location   = new System.Drawing.Point(20, 88);  this.lblNombre1.Text   = "Primer nombre:";
            this.lblNombre2.AutoSize   = true; this.lblNombre2.Location   = new System.Drawing.Point(20, 123); this.lblNombre2.Text   = "Segundo nombre:";
            this.lblApellido1.AutoSize = true; this.lblApellido1.Location = new System.Drawing.Point(20, 158); this.lblApellido1.Text = "Primer apellido:";
            this.lblApellido2.AutoSize = true; this.lblApellido2.Location = new System.Drawing.Point(20, 193); this.lblApellido2.Text = "Segundo apellido:";
            this.lblCorreo.AutoSize    = true; this.lblCorreo.Location    = new System.Drawing.Point(20, 228); this.lblCorreo.Text    = "Correo:";
            this.lblFechaNac.AutoSize  = true; this.lblFechaNac.Location  = new System.Drawing.Point(20, 263); this.lblFechaNac.Text  = "Fecha nacimiento:";

            int tx = 165, w = 200, h = 23;
            this.txtUsername.Location  = new System.Drawing.Point(tx, 15);  this.txtUsername.Name  = "txtUsername";  this.txtUsername.Size  = new System.Drawing.Size(w, h); this.txtUsername.TabIndex  = 0;
            this.txtPassword.Location  = new System.Drawing.Point(tx, 50);  this.txtPassword.Name  = "txtPassword";  this.txtPassword.Size  = new System.Drawing.Size(w, h); this.txtPassword.TabIndex  = 1; this.txtPassword.PasswordChar = '*';
            this.txtNombre1.Location   = new System.Drawing.Point(tx, 85);  this.txtNombre1.Name   = "txtNombre1";   this.txtNombre1.Size   = new System.Drawing.Size(w, h); this.txtNombre1.TabIndex   = 2;
            this.txtNombre2.Location   = new System.Drawing.Point(tx, 120); this.txtNombre2.Name   = "txtNombre2";   this.txtNombre2.Size   = new System.Drawing.Size(w, h); this.txtNombre2.TabIndex   = 3;
            this.txtApellido1.Location = new System.Drawing.Point(tx, 155); this.txtApellido1.Name = "txtApellido1"; this.txtApellido1.Size = new System.Drawing.Size(w, h); this.txtApellido1.TabIndex = 4;
            this.txtApellido2.Location = new System.Drawing.Point(tx, 190); this.txtApellido2.Name = "txtApellido2"; this.txtApellido2.Size = new System.Drawing.Size(w, h); this.txtApellido2.TabIndex = 5;
            this.txtCorreo.Location    = new System.Drawing.Point(tx, 225); this.txtCorreo.Name    = "txtCorreo";    this.txtCorreo.Size    = new System.Drawing.Size(w, h); this.txtCorreo.TabIndex    = 6;

            this.dtpFechaNac.Format   = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaNac.Location = new System.Drawing.Point(tx, 260);
            this.dtpFechaNac.Name     = "dtpFechaNac";
            this.dtpFechaNac.Size     = new System.Drawing.Size(w, h);
            this.dtpFechaNac.TabIndex = 7;

            this.btnGuardar.Location = new System.Drawing.Point(tx, 300);
            this.btnGuardar.Name     = "btnGuardar";
            this.btnGuardar.Size     = new System.Drawing.Size(95, 30);
            this.btnGuardar.TabIndex = 8;
            this.btnGuardar.Text     = "Registrar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);

            this.btnCancelar.Location = new System.Drawing.Point(tx + 105, 300);
            this.btnCancelar.Name     = "btnCancelar";
            this.btnCancelar.Size     = new System.Drawing.Size(95, 30);
            this.btnCancelar.TabIndex = 9;
            this.btnCancelar.Text     = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(390, 348);
            this.Controls.Add(this.lblUsername);  this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);  this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblNombre1);   this.Controls.Add(this.txtNombre1);
            this.Controls.Add(this.lblNombre2);   this.Controls.Add(this.txtNombre2);
            this.Controls.Add(this.lblApellido1); this.Controls.Add(this.txtApellido1);
            this.Controls.Add(this.lblApellido2); this.Controls.Add(this.txtApellido2);
            this.Controls.Add(this.lblCorreo);    this.Controls.Add(this.txtCorreo);
            this.Controls.Add(this.lblFechaNac);  this.Controls.Add(this.dtpFechaNac);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.Name            = "FrmRegistro";
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text            = "Registro de usuario";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.Label          lblUsername, lblPassword, lblNombre1, lblNombre2;
        private System.Windows.Forms.Label          lblApellido1, lblApellido2, lblCorreo, lblFechaNac;
        private System.Windows.Forms.TextBox        txtUsername, txtPassword, txtNombre1, txtNombre2;
        private System.Windows.Forms.TextBox        txtApellido1, txtApellido2, txtCorreo;
        private System.Windows.Forms.DateTimePicker dtpFechaNac;
        private System.Windows.Forms.Button         btnGuardar, btnCancelar;
    }
}
