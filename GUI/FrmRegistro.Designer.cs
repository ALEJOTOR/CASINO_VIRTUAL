namespace GUI
{
    partial class FrmRegistro
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
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblNombre1 = new System.Windows.Forms.Label();
            this.lblNombre2 = new System.Windows.Forms.Label();
            this.lblApellido1 = new System.Windows.Forms.Label();
            this.lblApellido2 = new System.Windows.Forms.Label();
            this.lblCorreo = new System.Windows.Forms.Label();
            this.lblFechaNac = new System.Windows.Forms.Label();

            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtNombre1 = new System.Windows.Forms.TextBox();
            this.txtNombre2 = new System.Windows.Forms.TextBox();
            this.txtApellido1 = new System.Windows.Forms.TextBox();
            this.txtApellido2 = new System.Windows.Forms.TextBox();
            this.txtCorreo = new System.Windows.Forms.TextBox();

            this.dtpFechaNac = new System.Windows.Forms.DateTimePicker();

            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(20, 20);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(56, 16);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Usuario:";

            // txtUsername
            this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)
                (((System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));

            this.txtUsername.Location = new System.Drawing.Point(175, 18);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(210, 22);
            this.txtUsername.TabIndex = 1;

            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(20, 55);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(85, 16);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Contraseña:";

            // txtPassword
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)
                (((System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));

            this.txtPassword.Location = new System.Drawing.Point(175, 53);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(210, 22);
            this.txtPassword.TabIndex = 3;

            // lblNombre1
            this.lblNombre1.AutoSize = true;
            this.lblNombre1.Location = new System.Drawing.Point(20, 90);
            this.lblNombre1.Name = "lblNombre1";
            this.lblNombre1.Size = new System.Drawing.Size(98, 16);
            this.lblNombre1.TabIndex = 4;
            this.lblNombre1.Text = "Primer nombre:";

            // txtNombre1
            this.txtNombre1.Anchor = ((System.Windows.Forms.AnchorStyles)
                (((System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));

            this.txtNombre1.Location = new System.Drawing.Point(175, 88);
            this.txtNombre1.Name = "txtNombre1";
            this.txtNombre1.Size = new System.Drawing.Size(210, 22);
            this.txtNombre1.TabIndex = 5;

            // lblNombre2
            this.lblNombre2.AutoSize = true;
            this.lblNombre2.Location = new System.Drawing.Point(20, 125);
            this.lblNombre2.Name = "lblNombre2";
            this.lblNombre2.Size = new System.Drawing.Size(115, 16);
            this.lblNombre2.TabIndex = 6;
            this.lblNombre2.Text = "Segundo nombre:";

            // txtNombre2
            this.txtNombre2.Anchor = ((System.Windows.Forms.AnchorStyles)
                (((System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));

            this.txtNombre2.Location = new System.Drawing.Point(175, 123);
            this.txtNombre2.Name = "txtNombre2";
            this.txtNombre2.Size = new System.Drawing.Size(210, 22);
            this.txtNombre2.TabIndex = 7;

            // lblApellido1
            this.lblApellido1.AutoSize = true;
            this.lblApellido1.Location = new System.Drawing.Point(20, 160);
            this.lblApellido1.Name = "lblApellido1";
            this.lblApellido1.Size = new System.Drawing.Size(99, 16);
            this.lblApellido1.TabIndex = 8;
            this.lblApellido1.Text = "Primer apellido:";

            // txtApellido1
            this.txtApellido1.Anchor = ((System.Windows.Forms.AnchorStyles)
                (((System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));

            this.txtApellido1.Location = new System.Drawing.Point(175, 158);
            this.txtApellido1.Name = "txtApellido1";
            this.txtApellido1.Size = new System.Drawing.Size(210, 22);
            this.txtApellido1.TabIndex = 9;

            // lblApellido2
            this.lblApellido2.AutoSize = true;
            this.lblApellido2.Location = new System.Drawing.Point(20, 195);
            this.lblApellido2.Name = "lblApellido2";
            this.lblApellido2.Size = new System.Drawing.Size(116, 16);
            this.lblApellido2.TabIndex = 10;
            this.lblApellido2.Text = "Segundo apellido:";

            // txtApellido2
            this.txtApellido2.Anchor = ((System.Windows.Forms.AnchorStyles)
                (((System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));

            this.txtApellido2.Location = new System.Drawing.Point(175, 193);
            this.txtApellido2.Name = "txtApellido2";
            this.txtApellido2.Size = new System.Drawing.Size(210, 22);
            this.txtApellido2.TabIndex = 11;

            // lblCorreo
            this.lblCorreo.AutoSize = true;
            this.lblCorreo.Location = new System.Drawing.Point(20, 230);
            this.lblCorreo.Name = "lblCorreo";
            this.lblCorreo.Size = new System.Drawing.Size(52, 16);
            this.lblCorreo.TabIndex = 12;
            this.lblCorreo.Text = "Correo:";

            // txtCorreo
            this.txtCorreo.Anchor = ((System.Windows.Forms.AnchorStyles)
                (((System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));

            this.txtCorreo.Location = new System.Drawing.Point(175, 228);
            this.txtCorreo.Name = "txtCorreo";
            this.txtCorreo.Size = new System.Drawing.Size(210, 22);
            this.txtCorreo.TabIndex = 13;

            // lblFechaNac
            this.lblFechaNac.AutoSize = true;
            this.lblFechaNac.Location = new System.Drawing.Point(20, 265);
            this.lblFechaNac.Name = "lblFechaNac";
            this.lblFechaNac.Size = new System.Drawing.Size(122, 16);
            this.lblFechaNac.TabIndex = 14;
            this.lblFechaNac.Text = "Fecha nacimiento:";

            // dtpFechaNac
            this.dtpFechaNac.Anchor = ((System.Windows.Forms.AnchorStyles)
                (((System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));

            this.dtpFechaNac.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaNac.Location = new System.Drawing.Point(175, 263);
            this.dtpFechaNac.Name = "dtpFechaNac";
            this.dtpFechaNac.Size = new System.Drawing.Size(210, 22);
            this.dtpFechaNac.TabIndex = 15;

            // btnGuardar
            this.btnGuardar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.btnGuardar.Location = new System.Drawing.Point(175, 315);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(100, 32);
            this.btnGuardar.TabIndex = 16;
            this.btnGuardar.Text = "Registrar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);

            // btnCancelar
            this.btnCancelar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnCancelar.Location = new System.Drawing.Point(285, 315);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 32);
            this.btnCancelar.TabIndex = 17;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            // FrmRegistro
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 370);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblNombre1);
            this.Controls.Add(this.txtNombre1);
            this.Controls.Add(this.lblNombre2);
            this.Controls.Add(this.txtNombre2);
            this.Controls.Add(this.lblApellido1);
            this.Controls.Add(this.txtApellido1);
            this.Controls.Add(this.lblApellido2);
            this.Controls.Add(this.txtApellido2);
            this.Controls.Add(this.lblCorreo);
            this.Controls.Add(this.txtCorreo);
            this.Controls.Add(this.lblFechaNac);
            this.Controls.Add(this.dtpFechaNac);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCancelar);

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(450, 420);
            this.Name = "FrmRegistro";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Registro de usuario";

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblNombre1;
        private System.Windows.Forms.Label lblNombre2;
        private System.Windows.Forms.Label lblApellido1;
        private System.Windows.Forms.Label lblApellido2;
        private System.Windows.Forms.Label lblCorreo;
        private System.Windows.Forms.Label lblFechaNac;

        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtNombre1;
        private System.Windows.Forms.TextBox txtNombre2;
        private System.Windows.Forms.TextBox txtApellido1;
        private System.Windows.Forms.TextBox txtApellido2;
        private System.Windows.Forms.TextBox txtCorreo;

        private System.Windows.Forms.DateTimePicker dtpFechaNac;

        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}