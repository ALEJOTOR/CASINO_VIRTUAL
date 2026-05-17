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

            int col1 = 20, col2 = 175, ancho = 210, fila = 18, sep = 35;
            var anchor = System.Windows.Forms.AnchorStyles.Top |
                         System.Windows.Forms.AnchorStyles.Left |
                         System.Windows.Forms.AnchorStyles.Right;

            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(col1, fila);
            this.lblUsername.Text     = "Usuario:";

            this.txtUsername.Anchor   = anchor;
            this.txtUsername.Location = new System.Drawing.Point(120, 48);
            this.txtUsername.Size     = new System.Drawing.Size(ancho, 22);
            fila += sep;

            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(col1, fila);
            this.lblPassword.Text     = "Contraseña:";

            this.txtPassword.Anchor       = anchor;
            this.txtPassword.Location     = new System.Drawing.Point(col2, fila - 2);
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size         = new System.Drawing.Size(ancho, 22);
            fila += sep;

            // lblNombre1
            this.lblNombre1.AutoSize = true;
            this.lblNombre1.Location = new System.Drawing.Point(col1, fila);
            this.lblNombre1.Text     = "Primer nombre:";

            this.txtNombre1.Anchor   = anchor;
            this.txtNombre1.Location = new System.Drawing.Point(col2, fila - 2);
            this.txtNombre1.Size     = new System.Drawing.Size(ancho, 22);
            fila += sep;

            // lblNombre2
            this.lblNombre2.AutoSize = true;
            this.lblNombre2.Location = new System.Drawing.Point(col1, fila);
            this.lblNombre2.Text     = "Segundo nombre:";

            this.txtNombre2.Anchor   = anchor;
            this.txtNombre2.Location = new System.Drawing.Point(col2, fila - 2);
            this.txtNombre2.Size     = new System.Drawing.Size(ancho, 22);
            fila += sep;

            // lblApellido1
            this.lblApellido1.AutoSize = true;
            this.lblApellido1.Location = new System.Drawing.Point(col1, fila);
            this.lblApellido1.Text     = "Primer apellido:";

            this.txtApellido1.Anchor   = anchor;
            this.txtApellido1.Location = new System.Drawing.Point(col2, fila - 2);
            this.txtApellido1.Size     = new System.Drawing.Size(ancho, 22);
            fila += sep;

            // lblApellido2
            this.lblApellido2.AutoSize = true;
            this.lblApellido2.Location = new System.Drawing.Point(col1, fila);
            this.lblApellido2.Text     = "Segundo apellido:";

            this.txtApellido2.Anchor   = anchor;
            this.txtApellido2.Location = new System.Drawing.Point(col2, fila - 2);
            this.txtApellido2.Size     = new System.Drawing.Size(ancho, 22);
            fila += sep;

            // lblCorreo
            this.lblCorreo.AutoSize = true;
            this.lblCorreo.Location = new System.Drawing.Point(col1, fila);
            this.lblCorreo.Text     = "Correo:";

            this.txtCorreo.Anchor   = anchor;
            this.txtCorreo.Location = new System.Drawing.Point(col2, fila - 2);
            this.txtCorreo.Size     = new System.Drawing.Size(ancho, 22);
            fila += sep;

            // lblFechaNac
            this.lblFechaNac.AutoSize = true;
            this.lblFechaNac.Location = new System.Drawing.Point(col1, fila);
            this.lblFechaNac.Text     = "Fecha nacimiento:";

            this.dtpFechaNac.Anchor   = anchor;
            this.dtpFechaNac.Format   = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaNac.Location = new System.Drawing.Point(col2, fila - 2);
            this.dtpFechaNac.Size     = new System.Drawing.Size(ancho, 22);
            fila += sep + 8;

            // Botones
            this.btnGuardar.Anchor   = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.btnGuardar.Location = new System.Drawing.Point(col2, fila);
            this.btnGuardar.Size     = new System.Drawing.Size(100, 32);
            this.btnGuardar.Text     = "Registrar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);

            this.btnCancelar.Anchor   = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnCancelar.Location = new System.Drawing.Point(col2 + 110, fila);
            this.btnCancelar.Size     = new System.Drawing.Size(100, 32);
            this.btnCancelar.Text     = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(410, fila + 50);
            this.MinimumSize         = new System.Drawing.Size(360, fila + 80);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox     = false;
            this.Name            = "FrmRegistro";
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text            = "Registro de usuario";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.Label          lblUsername;
        private System.Windows.Forms.Label          lblPassword;
        private System.Windows.Forms.Label          lblNombre1;
        private System.Windows.Forms.Label          lblNombre2;
        private System.Windows.Forms.Label          lblApellido1;
        private System.Windows.Forms.Label          lblApellido2;
        private System.Windows.Forms.Label          lblCorreo;
        private System.Windows.Forms.Label          lblFechaNac;
        private System.Windows.Forms.TextBox        txtUsername;
        private System.Windows.Forms.TextBox        txtPassword;
        private System.Windows.Forms.TextBox        txtNombre1;
        private System.Windows.Forms.TextBox        txtNombre2;
        private System.Windows.Forms.TextBox        txtApellido1;
        private System.Windows.Forms.TextBox        txtApellido2;
        private System.Windows.Forms.TextBox        txtCorreo;
        private System.Windows.Forms.DateTimePicker dtpFechaNac;
        private System.Windows.Forms.Button         btnGuardar;
        private System.Windows.Forms.Button         btnCancelar;
    }
}
