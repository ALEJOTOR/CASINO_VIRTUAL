namespace GUI
{
    partial class FrmLogin
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
            this.lblTitulo    = new System.Windows.Forms.Label();
            this.lblUsername  = new System.Windows.Forms.Label();
            this.lblPassword  = new System.Windows.Forms.Label();
            this.txtUsername  = new System.Windows.Forms.TextBox();
            this.txtPassword  = new System.Windows.Forms.TextBox();
            this.btnIngresar  = new System.Windows.Forms.Button();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.AutoSize  = true;
            this.lblTitulo.Font      = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location  = new System.Drawing.Point(90, 20);
            this.lblTitulo.Name      = "lblTitulo";
            this.lblTitulo.Size      = new System.Drawing.Size(200, 30);
            this.lblTitulo.Text      = "Casino Virtual";

            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(50, 75);
            this.lblUsername.Name     = "lblUsername";
            this.lblUsername.Text     = "Usuario:";

            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(50, 115);
            this.lblPassword.Name     = "lblPassword";
            this.lblPassword.Text     = "Contraseña:";

            // txtUsername
            this.txtUsername.Location = new System.Drawing.Point(150, 72);
            this.txtUsername.Name     = "txtUsername";
            this.txtUsername.Size     = new System.Drawing.Size(180, 23);
            this.txtUsername.TabIndex = 0;

            // txtPassword
            this.txtPassword.Location     = new System.Drawing.Point(150, 112);
            this.txtPassword.Name         = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size         = new System.Drawing.Size(180, 23);
            this.txtPassword.TabIndex     = 1;

            // btnIngresar
            this.btnIngresar.Location = new System.Drawing.Point(80, 158);
            this.btnIngresar.Name     = "btnIngresar";
            this.btnIngresar.Size     = new System.Drawing.Size(100, 30);
            this.btnIngresar.TabIndex = 2;
            this.btnIngresar.Text     = "Ingresar";
            this.btnIngresar.UseVisualStyleBackColor = true;
            this.btnIngresar.Click   += new System.EventHandler(this.btnIngresar_Click);

            // btnRegistrar
            this.btnRegistrar.Location = new System.Drawing.Point(200, 158);
            this.btnRegistrar.Name     = "btnRegistrar";
            this.btnRegistrar.Size     = new System.Drawing.Size(110, 30);
            this.btnRegistrar.TabIndex = 3;
            this.btnRegistrar.Text     = "Registrarse";
            this.btnRegistrar.UseVisualStyleBackColor = true;
            this.btnRegistrar.Click   += new System.EventHandler(this.btnRegistrar_Click);

            // FrmLogin
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(400, 215);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnIngresar);
            this.Controls.Add(this.btnRegistrar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.Name            = "FrmLogin";
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text            = "Casino Virtual - Login";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label    lblTitulo;
        private System.Windows.Forms.Label    lblUsername;
        private System.Windows.Forms.Label    lblPassword;
        private System.Windows.Forms.TextBox  txtUsername;
        private System.Windows.Forms.TextBox  txtPassword;
        private System.Windows.Forms.Button   btnIngresar;
        private System.Windows.Forms.Button   btnRegistrar;
    }
}
