using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class FrmLogin : Form
    {
        private readonly UsuarioServicio _servicio = new UsuarioServicio();
        private bool _modoRegistro;
        private Label _lblIdentificacion;
        private Label _lblNombre1;
        private Label _lblNombre2;
        private Label _lblApellido1;
        private Label _lblApellido2;
        private Label _lblCorreo;
        private Label _lblFechaNac;
        private TextBox _txtIdentificacion;
        private TextBox _txtNombre1;
        private TextBox _txtNombre2;
        private TextBox _txtApellido1;
        private TextBox _txtApellido2;
        private TextBox _txtCorreo;
        private DateTimePicker _dtpFechaNac;

        public FrmLogin()
        {
            InitializeComponent();
            // Problema visual que resuelve: el login queda centrado en una tarjeta oscura con tipografia y botones uniformes.
            AppTheme.ApplyForm(this);
            AppTheme.ApplyCard(panelFormulario, 12);
            AppTheme.ApplyTitle(lblTitulo);
            AppTheme.ApplySubtitle(lblSubtitulo);
            AppTheme.ApplyTitle(lblMarca);
            AppTheme.ApplySubtitle(lblMarcaDetalle);
            AppTheme.ApplyTextBox(txtUsername);
            AppTheme.ApplyTextBox(txtPassword);
            AppTheme.ApplyPrimaryButton(btnIngresar);
            AppTheme.ApplyPrimaryButton(btnRegistrar, AppTheme.BgHover);
            ConfigurarPresentacion();
        }

        private void ConfigurarPresentacion()
        {
            // Problema visual que resuelve: organiza el login como pantalla de entrada moderna y no como formulario fijo.
            Text = "Casino Royal - Iniciar sesion";
            BackColor = AppTheme.BgPrincipal;
            panelMarca.BackColor = AppTheme.BgNavbar;
            panelFormulario.BackColor = AppTheme.BgCard;
            lblMarca.Font = new Font("Georgia", 28F, FontStyle.Bold);
            lblMarca.ForeColor = AppTheme.Dorado;
            lblMarcaDetalle.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            lblMarcaDetalle.ForeColor = AppTheme.TextoSecundario;
            lblTitulo.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblSubtitulo.Font = AppTheme.Subtitulo;
            lblUsername.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblPassword.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblUsername.ForeColor = AppTheme.TextoPrimario;
            lblPassword.ForeColor = AppTheme.TextoPrimario;
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.PasswordChar = '*';
            btnIngresar.Text = "Ingresar";
            btnRegistrar.Text = "Crear cuenta";

            panelMarca.Paint += panelMarca_Paint;
            panelFormulario.Paint += panelFormulario_Paint;
            Resize += (s, e) => ReubicarLogin();
            CrearControlesRegistro();
            ReubicarLogin();
        }

        private void CrearControlesRegistro()
        {
            // Problema visual que resuelve: permite crear cuenta dentro de la misma ventana sin abrir un dialogo separado.
            _lblIdentificacion = CrearLabelRegistro("Identificacion");
            _lblNombre1 = CrearLabelRegistro("Primer nombre");
            _lblNombre2 = CrearLabelRegistro("Segundo nombre");
            _lblApellido1 = CrearLabelRegistro("Primer apellido");
            _lblApellido2 = CrearLabelRegistro("Segundo apellido");
            _lblCorreo = CrearLabelRegistro("Correo");
            _lblFechaNac = CrearLabelRegistro("Fecha nacimiento");

            _txtIdentificacion = CrearTextBoxRegistro();
            _txtNombre1 = CrearTextBoxRegistro();
            _txtNombre2 = CrearTextBoxRegistro();
            _txtApellido1 = CrearTextBoxRegistro();
            _txtApellido2 = CrearTextBoxRegistro();
            _txtCorreo = CrearTextBoxRegistro();
            _dtpFechaNac = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Font = AppTheme.Cuerpo,
                CalendarFont = AppTheme.Cuerpo
            };

            Control[] controles =
            {
                _lblIdentificacion, _txtIdentificacion,
                _lblNombre1, _txtNombre1,
                _lblNombre2, _txtNombre2,
                _lblApellido1, _txtApellido1,
                _lblApellido2, _txtApellido2,
                _lblCorreo, _txtCorreo,
                _lblFechaNac, _dtpFechaNac
            };

            foreach (Control control in controles)
            {
                control.Visible = false;
                panelFormulario.Controls.Add(control);
            }
        }

        private Label CrearLabelRegistro(string texto)
        {
            return new Label
            {
                Text = texto,
                AutoSize = false,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = AppTheme.TextoPrimario,
                BackColor = Color.Transparent
            };
        }

        private TextBox CrearTextBoxRegistro()
        {
            TextBox textBox = new TextBox();
            AppTheme.ApplyTextBox(textBox);
            textBox.BorderStyle = BorderStyle.FixedSingle;
            return textBox;
        }

        private void ReubicarLogin()
        {
            if (ClientSize.Width <= 0 || ClientSize.Height <= 0) return;

            int marcaW = Math.Max(280, Math.Min(340, ClientSize.Width / 2 - 70));
            panelMarca.SetBounds(0, 0, marcaW, ClientSize.Height);

            int cardW = _modoRegistro ? Math.Min(610, ClientSize.Width - marcaW - 70) : Math.Min(390, ClientSize.Width - marcaW - 70);
            int cardH = _modoRegistro ? Math.Min(560, ClientSize.Height - 70) : 348;
            panelFormulario.SetBounds(
                marcaW + (ClientSize.Width - marcaW - cardW) / 2,
                (ClientSize.Height - cardH) / 2,
                cardW,
                cardH);

            lblMarca.SetBounds(36, Math.Max(76, ClientSize.Height / 2 - 118), marcaW - 72, 112);
            lblMarcaDetalle.SetBounds(40, lblMarca.Bottom + 18, marcaW - 82, 92);

            if (_modoRegistro)
                ReubicarRegistro();
            else
                ReubicarIngreso();

            AppTheme.ApplyRoundedRegion(panelFormulario, 14);
            AppTheme.ApplyRoundedRegion(btnIngresar, 8);
            AppTheme.ApplyRoundedRegion(btnRegistrar, 8);
            panelMarca.Invalidate();
            panelFormulario.Invalidate();
        }

        private void ReubicarIngreso()
        {
            int x = 34;
            int w = panelFormulario.ClientSize.Width - x * 2;
            lblTitulo.SetBounds(x, 26, w, 42);
            lblSubtitulo.SetBounds(x, 74, w, 30);
            lblUsername.SetBounds(x, 122, w, 22);
            txtUsername.SetBounds(x, 148, w, 32);
            lblPassword.SetBounds(x, 194, w, 22);
            txtPassword.SetBounds(x, 220, w, 32);

            int gap = 12;
            int btnW = (w - gap) / 2;
            btnIngresar.SetBounds(x, 284, btnW, 42);
            btnRegistrar.SetBounds(x + btnW + gap, 284, w - btnW - gap, 42);
        }

        private void ReubicarRegistro()
        {
            int x = 34;
            int w = panelFormulario.ClientSize.Width - x * 2;
            int gap = 18;
            int colW = (w - gap) / 2;

            lblTitulo.SetBounds(x, 24, w, 42);
            lblSubtitulo.SetBounds(x, 70, w, 34);

            UbicarCampo(_lblIdentificacion, _txtIdentificacion, x, 116, colW);
            UbicarCampo(lblUsername, txtUsername, x + colW + gap, 116, colW);
            UbicarCampo(lblPassword, txtPassword, x, 188, colW);
            UbicarCampo(_lblCorreo, _txtCorreo, x + colW + gap, 188, colW);
            UbicarCampo(_lblNombre1, _txtNombre1, x, 260, colW);
            UbicarCampo(_lblNombre2, _txtNombre2, x + colW + gap, 260, colW);
            UbicarCampo(_lblApellido1, _txtApellido1, x, 332, colW);
            UbicarCampo(_lblApellido2, _txtApellido2, x + colW + gap, 332, colW);

            _lblFechaNac.SetBounds(x, 404, w, 22);
            _dtpFechaNac.SetBounds(x, 430, colW, 32);

            int btnY = panelFormulario.ClientSize.Height - 66;
            int btnW = (w - gap) / 2;
            btnIngresar.SetBounds(x, btnY, btnW, 42);
            btnRegistrar.SetBounds(x + btnW + gap, btnY, w - btnW - gap, 42);
        }

        private void UbicarCampo(Label label, Control input, int x, int y, int w)
        {
            label.SetBounds(x, y, w, 22);
            input.SetBounds(x, y + 26, w, 32);
        }

        private void panelMarca_Paint(object sender, PaintEventArgs e)
        {
            // Problema visual que resuelve: el bloque de marca gana profundidad sin usar imagen externa.
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (LinearGradientBrush fondo = new LinearGradientBrush(panelMarca.ClientRectangle,
                Color.FromArgb(8, 13, 24), Color.FromArgb(19, 30, 54), 90F))
                e.Graphics.FillRectangle(fondo, panelMarca.ClientRectangle);

            using (SolidBrush brillo = new SolidBrush(Color.FromArgb(28, AppTheme.Dorado)))
                e.Graphics.FillEllipse(brillo, -panelMarca.Width / 2, 40, panelMarca.Width, panelMarca.Width);
        }

        private void panelFormulario_Paint(object sender, PaintEventArgs e)
        {
            // Problema visual que resuelve: la tarjeta del formulario queda delimitada y con borde dorado sutil.
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = new Rectangle(0, 0, panelFormulario.Width - 1, panelFormulario.Height - 1);
            using (GraphicsPath path = RoundedPath(area, 14))
            using (LinearGradientBrush fondo = new LinearGradientBrush(area,
                Color.FromArgb(22, 27, 39), Color.FromArgb(13, 19, 32), 90F))
                e.Graphics.FillPath(fondo, path);

            using (GraphicsPath path = RoundedPath(area, 14))
            using (Pen borde = new Pen(Color.FromArgb(110, AppTheme.Dorado), 1))
                e.Graphics.DrawPath(borde, path);
        }

        private GraphicsPath RoundedPath(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void btnIngresar_Click(object sender, System.EventArgs e)
        {
            if (_modoRegistro)
            {
                RegistrarUsuarioDesdeLogin();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Complete todos los campos.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Usuario u = _servicio.Login(txtUsername.Text.Trim(), txtPassword.Text);
            if (u == null)
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (u.IdRol == 1)
                new FrmAdmin(u).Show();
            else
                new MainForm(u).Show();

            this.Hide();
        }

        private void btnRegistrar_Click(object sender, System.EventArgs e)
        {
            if (_modoRegistro)
                MostrarModoLogin();
            else
                MostrarModoRegistro();
        }

        private void MostrarModoRegistro()
        {
            // Problema visual que resuelve: el usuario cambia a registro sin perder el contexto de la pantalla de acceso.
            _modoRegistro = true;
            MinimumSize = new Size(996, 678);
            ClientSize = new Size(980, 640);
            CenterToScreen();

            lblTitulo.Text = "Crear cuenta";
            lblSubtitulo.Text = "Completa tus datos para activar tu acceso a Casino Royal.";
            btnIngresar.Text = "Registrar cuenta";
            btnRegistrar.Text = "Volver";
            lblUsername.Text = "Usuario";
            lblPassword.Text = "Contrasena";

            foreach (Control control in new Control[]
            {
                _lblIdentificacion, _txtIdentificacion, _lblNombre1, _txtNombre1,
                _lblNombre2, _txtNombre2, _lblApellido1, _txtApellido1,
                _lblApellido2, _txtApellido2, _lblCorreo, _txtCorreo,
                _lblFechaNac, _dtpFechaNac
            })
                control.Visible = true;

            ReubicarLogin();
        }

        private void MostrarModoLogin()
        {
            _modoRegistro = false;
            MinimumSize = new Size(776, 469);
            ClientSize = new Size(760, 430);
            CenterToScreen();

            lblTitulo.Text = "Iniciar sesion";
            lblSubtitulo.Text = "Ingresa con tus credenciales para continuar.";
            btnIngresar.Text = "Ingresar";
            btnRegistrar.Text = "Crear cuenta";
            lblUsername.Text = "Usuario:";
            lblPassword.Text = "Contrasena:";

            foreach (Control control in new Control[]
            {
                _lblIdentificacion, _txtIdentificacion, _lblNombre1, _txtNombre1,
                _lblNombre2, _txtNombre2, _lblApellido1, _txtApellido1,
                _lblApellido2, _txtApellido2, _lblCorreo, _txtCorreo,
                _lblFechaNac, _dtpFechaNac
            })
                control.Visible = false;

            ReubicarLogin();
        }

        private void RegistrarUsuarioDesdeLogin()
        {
            if (string.IsNullOrWhiteSpace(_txtIdentificacion.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(_txtNombre1.Text) ||
                string.IsNullOrWhiteSpace(_txtApellido1.Text) ||
                string.IsNullOrWhiteSpace(_txtCorreo.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.",
                    "Campos incompletos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(_txtIdentificacion.Text.Trim(), out int identificacion))
            {
                MessageBox.Show("La identificacion debe ser numerica.",
                    "Dato invalido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            Usuario usuario = new Usuario
            {
                IdUsuario = identificacion,
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text,
                Nombre1 = _txtNombre1.Text.Trim(),
                Nombre2 = _txtNombre2.Text.Trim(),
                Apellido1 = _txtApellido1.Text.Trim(),
                Apellido2 = _txtApellido2.Text.Trim(),
                Correo = _txtCorreo.Text.Trim(),
                FechaNacimiento = _dtpFechaNac.Value,
                Saldo = 0,
                IdRol = 2
            };

            if (!usuario.EsMayorDeEdad())
            {
                MessageBox.Show("Debes ser mayor de edad para registrarte.",
                    "Edad insuficiente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string resultado = _servicio.Registrar(usuario);
            bool ok = resultado == "Guardado correctamente.";
            MessageBox.Show(ok ? "Usuario registrado correctamente." : resultado,
                ok ? "Exito" : "Error",
                MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (ok)
            {
                LimpiarRegistro();
                MostrarModoLogin();
            }
        }

        private void LimpiarRegistro()
        {
            _txtIdentificacion.Clear();
            _txtNombre1.Clear();
            _txtNombre2.Clear();
            _txtApellido1.Clear();
            _txtApellido2.Clear();
            _txtCorreo.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            _dtpFechaNac.Value = DateTime.Today;
        }
    }
}
