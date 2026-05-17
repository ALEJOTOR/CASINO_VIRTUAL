namespace GUI
{
    partial class FrmLogin
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
            this.lblTitulo   = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnIngresar = new System.Windows.Forms.Button();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblTitulo.Font      = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Height    = 50;
            this.lblTitulo.Name      = "lblTitulo";
            this.lblTitulo.TabIndex  = 0;
            this.lblTitulo.Text      = "Casino Virtual";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(20, 68);
            this.lblUsername.Name     = "lblUsername";
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text     = "Usuario:";

            // txtUsername
            this.txtUsername.Anchor   = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtUsername.Location = new System.Drawing.Point(130, 65);
            this.txtUsername.Name     = "txtUsername";
            this.txtUsername.Size     = new System.Drawing.Size(240, 22);
            this.txtUsername.TabIndex = 2;

            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(20, 106);
            this.lblPassword.Name     = "lblPassword";
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text     = "Contraseña:";

            // txtPassword
            this.txtPassword.Anchor       = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtPassword.Location     = new System.Drawing.Point(130, 103);
            this.txtPassword.Name         = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size         = new System.Drawing.Size(240, 22);
            this.txtPassword.TabIndex     = 4;

            // btnIngresar
            this.btnIngresar.Anchor   = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.btnIngresar.Location = new System.Drawing.Point(60, 148);
            this.btnIngresar.Name     = "btnIngresar";
            this.btnIngresar.Size     = new System.Drawing.Size(110, 32);
            this.btnIngresar.TabIndex = 5;
            this.btnIngresar.Text     = "Ingresar";
            this.btnIngresar.UseVisualStyleBackColor = true;
            this.btnIngresar.Click += new System.EventHandler(this.btnIngresar_Click);

            // btnRegistrar
            this.btnRegistrar.Anchor   = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnRegistrar.Location = new System.Drawing.Point(200, 148);
            this.btnRegistrar.Name     = "btnRegistrar";
            this.btnRegistrar.Size     = new System.Drawing.Size(120, 32);
            this.btnRegistrar.TabIndex = 6;
            this.btnRegistrar.Text     = "Registrarse";
            this.btnRegistrar.UseVisualStyleBackColor = true;
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);

            // FrmLogin
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(400, 200);
            this.MinimumSize         = new System.Drawing.Size(350, 230);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnIngresar);
            this.Controls.Add(this.btnRegistrar);
            this.Name          = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text          = "Casino Virtual - Login";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.Label   lblTitulo;
        private System.Windows.Forms.Label   lblUsername;
        private System.Windows.Forms.Label   lblPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button  btnIngresar;
        private System.Windows.Forms.Button  btnRegistrar;
    }
}
