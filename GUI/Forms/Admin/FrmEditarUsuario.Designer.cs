using System;

namespace GUI
{
    partial class FrmEditarUsuario
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            this.lblId = new System.Windows.Forms.Label();
            this.lblIdValor = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblNombre1 = new System.Windows.Forms.Label();
            this.txtNombre1 = new System.Windows.Forms.TextBox();
            this.lblNombre2 = new System.Windows.Forms.Label();
            this.txtNombre2 = new System.Windows.Forms.TextBox();
            this.lblApellido1 = new System.Windows.Forms.Label();
            this.txtApellido1 = new System.Windows.Forms.TextBox();
            this.lblApellido2 = new System.Windows.Forms.Label();
            this.txtApellido2 = new System.Windows.Forms.TextBox();
            this.lblCorreo = new System.Windows.Forms.Label();
            this.txtCorreo = new System.Windows.Forms.TextBox();
            this.lblFechaNac = new System.Windows.Forms.Label();
            this.dtpFechaNac = new System.Windows.Forms.DateTimePicker();
            this.lblEstado = new System.Windows.Forms.Label();
            this.cboEstado = new System.Windows.Forms.ComboBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lblPasswordHint = new System.Windows.Forms.Label();
            this.SuspendLayout();

            var anchorLR = System.Windows.Forms.AnchorStyles.Left |
                           System.Windows.Forms.AnchorStyles.Right |
                           System.Windows.Forms.AnchorStyles.Top;

            int controlX = 120;
            int controlW = 220;
            int rowH = 28;
            int pad = 6;
            int y = 16;

            // IdUsuario
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(16, y + 4);
            this.lblId.Size = new System.Drawing.Size(80, 15);
            this.lblId.Text = "ID Usuario:";

            this.lblIdValor.AutoSize = true;
            this.lblIdValor.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblIdValor.ForeColor = System.Drawing.Color.FromArgb(250, 204, 21);
            this.lblIdValor.Location = new System.Drawing.Point(controlX, y + 4);
            this.lblIdValor.Size = new System.Drawing.Size(100, 19);
            this.lblIdValor.Text = "0";
            y += rowH + pad;

            // Username
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(16, y + 4);
            this.lblUsername.Size = new System.Drawing.Size(80, 15);
            this.lblUsername.Text = "Username:";
            this.txtUsername.Anchor = anchorLR;
            this.txtUsername.Location = new System.Drawing.Point(controlX, y);
            this.txtUsername.Size = new System.Drawing.Size(controlW, 22);
            y += rowH + pad;

            // Password
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(16, y + 4);
            this.lblPassword.Size = new System.Drawing.Size(80, 15);
            this.lblPassword.Text = "Contraseña:";
            this.txtPassword.Anchor = anchorLR;
            this.txtPassword.Location = new System.Drawing.Point(controlX, y);
            this.txtPassword.Size = new System.Drawing.Size(controlW, 22);
            this.txtPassword.UseSystemPasswordChar = true;

            this.lblPasswordHint.AutoSize = true;
            this.lblPasswordHint.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblPasswordHint.Location = new System.Drawing.Point(controlX + controlW + 8, y + 3);
            this.lblPasswordHint.Size = new System.Drawing.Size(180, 15);
            this.lblPasswordHint.Text = "(dejar vacío para mantener actual)";
            y += rowH + pad;

            // Nombre1
            this.lblNombre1.AutoSize = true;
            this.lblNombre1.Location = new System.Drawing.Point(16, y + 4);
            this.lblNombre1.Size = new System.Drawing.Size(80, 15);
            this.lblNombre1.Text = "Primer nombre:";
            this.txtNombre1.Anchor = anchorLR;
            this.txtNombre1.Location = new System.Drawing.Point(controlX, y);
            this.txtNombre1.Size = new System.Drawing.Size(controlW, 22);
            y += rowH + pad;

            // Nombre2
            this.lblNombre2.AutoSize = true;
            this.lblNombre2.Location = new System.Drawing.Point(16, y + 4);
            this.lblNombre2.Size = new System.Drawing.Size(80, 15);
            this.lblNombre2.Text = "Segundo nombre:";
            this.txtNombre2.Anchor = anchorLR;
            this.txtNombre2.Location = new System.Drawing.Point(controlX, y);
            this.txtNombre2.Size = new System.Drawing.Size(controlW, 22);
            y += rowH + pad;

            // Apellido1
            this.lblApellido1.AutoSize = true;
            this.lblApellido1.Location = new System.Drawing.Point(16, y + 4);
            this.lblApellido1.Size = new System.Drawing.Size(80, 15);
            this.lblApellido1.Text = "Primer apellido:";
            this.txtApellido1.Anchor = anchorLR;
            this.txtApellido1.Location = new System.Drawing.Point(controlX, y);
            this.txtApellido1.Size = new System.Drawing.Size(controlW, 22);
            y += rowH + pad;

            // Apellido2
            this.lblApellido2.AutoSize = true;
            this.lblApellido2.Location = new System.Drawing.Point(16, y + 4);
            this.lblApellido2.Size = new System.Drawing.Size(80, 15);
            this.lblApellido2.Text = "Segundo apellido:";
            this.txtApellido2.Anchor = anchorLR;
            this.txtApellido2.Location = new System.Drawing.Point(controlX, y);
            this.txtApellido2.Size = new System.Drawing.Size(controlW, 22);
            y += rowH + pad;

            // Correo
            this.lblCorreo.AutoSize = true;
            this.lblCorreo.Location = new System.Drawing.Point(16, y + 4);
            this.lblCorreo.Size = new System.Drawing.Size(80, 15);
            this.lblCorreo.Text = "Correo:";
            this.txtCorreo.Anchor = anchorLR;
            this.txtCorreo.Location = new System.Drawing.Point(controlX, y);
            this.txtCorreo.Size = new System.Drawing.Size(controlW, 22);
            y += rowH + pad;

            // FechaNacimiento
            this.lblFechaNac.AutoSize = true;
            this.lblFechaNac.Location = new System.Drawing.Point(16, y + 3);
            this.lblFechaNac.Size = new System.Drawing.Size(80, 15);
            this.lblFechaNac.Text = "Fecha nacimiento:";
            this.dtpFechaNac.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaNac.Location = new System.Drawing.Point(controlX, y);
            this.dtpFechaNac.Size = new System.Drawing.Size(controlW, 22);
            y += rowH + pad;

            // Estado
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(16, y + 4);
            this.lblEstado.Size = new System.Drawing.Size(80, 15);
            this.lblEstado.Text = "Estado:";
            this.cboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEstado.Location = new System.Drawing.Point(controlX, y);
            this.cboEstado.Size = new System.Drawing.Size(controlW, 23);
            y += rowH + 12;

            // Botones
            this.btnAceptar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.btnAceptar.Location = new System.Drawing.Point(16, y);
            this.btnAceptar.Size = new System.Drawing.Size(120, 32);
            this.btnAceptar.Text = "Guardar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);

            this.btnCancelar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnCancelar.Location = new System.Drawing.Point(controlX + controlW - 120, y);
            this.btnCancelar.Size = new System.Drawing.Size(120, 32);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            int formW = controlX + controlW + 210;
            int formH = y + 56;

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(formW, formH);
            this.MinimumSize = new System.Drawing.Size(formW, formH);
            this.MaximumSize = new System.Drawing.Size(formW + 40, formH + 40);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.lblIdValor);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPasswordHint);
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
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.cboEstado);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEditarUsuario";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Editar usuario";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblIdValor;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPasswordHint;
        private System.Windows.Forms.Label lblNombre1;
        private System.Windows.Forms.TextBox txtNombre1;
        private System.Windows.Forms.Label lblNombre2;
        private System.Windows.Forms.TextBox txtNombre2;
        private System.Windows.Forms.Label lblApellido1;
        private System.Windows.Forms.TextBox txtApellido1;
        private System.Windows.Forms.Label lblApellido2;
        private System.Windows.Forms.TextBox txtApellido2;
        private System.Windows.Forms.Label lblCorreo;
        private System.Windows.Forms.TextBox txtCorreo;
        private System.Windows.Forms.Label lblFechaNac;
        private System.Windows.Forms.DateTimePicker dtpFechaNac;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.ComboBox cboEstado;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
