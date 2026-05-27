using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class FrmRegistro : Form
    {
        private readonly UsuarioServicio _servicio = new UsuarioServicio();

        public FrmRegistro()
        {
            InitializeComponent();
            // Problema visual que resuelve: el registro se agrupa en una tarjeta central y evita campos dispersos.
            AppTheme.ApplyForm(this);
            AppTheme.ApplyTitle(lblTitulo);
            AppTheme.ApplyTypography(this);
            AppTheme.ApplyPrimaryButton(btnGuardar);
            AppTheme.ApplyPrimaryButton(btnCancelar, AppTheme.BgHover);
            CrearTarjetaRegistro();
        }

        private void CrearTarjetaRegistro()
        {
            // Problema visual que resuelve: agrega margen externo de 24px y un contenedor redondeado sin alterar la logica de registro.
            Panel card = new Panel
            {
                BackColor = AppTheme.BgCard,
                Location = new Point(24, 24),
                Size = new Size(ClientSize.Width - 48, ClientSize.Height - 48),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            Controls.Add(card);
            card.SendToBack();
            AppTheme.ApplyCard(card, 12);

            foreach (Control control in Controls.Cast<Control>().Where(c => c != card).ToList())
            {
                Point screen = control.PointToScreen(Point.Empty);
                Point local = card.PointToClient(screen);
                Controls.Remove(control);
                control.Location = local;
                card.Controls.Add(control);
            }
        }

        private void btnGuardar_Click(object sender, System.EventArgs e)
        {
            if(txtIdentificacion.Text.Trim() == "" || txtUsername.Text.Trim() == "" || txtPassword.Text == "" ||
               txtNombre1.Text.Trim() == "" || txtApellido1.Text.Trim() == "" || txtCorreo.Text.Trim() == "")
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.",
                    "Campos incompletos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            Usuario u = new Usuario
            {
                IdUsuario       = int.Parse(txtIdentificacion.Text.Trim()), 
                Username        = txtUsername.Text.Trim(),
                Password        = txtPassword.Text,
                Nombre1         = txtNombre1.Text.Trim(),
                Nombre2         = txtNombre2.Text.Trim(),
                Apellido1       = txtApellido1.Text.Trim(),
                Apellido2       = txtApellido2.Text.Trim(),
                Correo          = txtCorreo.Text.Trim(),
                FechaNacimiento = dtpFechaNac.Value,
                Saldo           = 0,
                IdRol           = 2
            };

            if (!u.EsMayorDeEdad())
            {
                MessageBox.Show("Debes ser mayor de edad para registrarte.",
                    "Edad insuficiente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string resultado = _servicio.Registrar(u);
            bool ok = resultado == "Guardado correctamente.";
            MessageBox.Show(ok ? "Usuario registrado correctamente." : resultado,
                ok ? "Éxito" : "Error",
                MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            if (ok) this.Close();
        }

        private void btnCancelar_Click(object sender, System.EventArgs e) => this.Close();
    }
}
