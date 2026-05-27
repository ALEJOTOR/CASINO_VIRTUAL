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
            ReubicarLogin();
        }

        private void ReubicarLogin()
        {
            if (ClientSize.Width <= 0 || ClientSize.Height <= 0) return;

            int marcaW = Math.Max(280, Math.Min(340, ClientSize.Width / 2 - 70));
            panelMarca.SetBounds(0, 0, marcaW, ClientSize.Height);

            int cardW = Math.Min(390, ClientSize.Width - marcaW - 70);
            int cardH = 348;
            panelFormulario.SetBounds(
                marcaW + (ClientSize.Width - marcaW - cardW) / 2,
                (ClientSize.Height - cardH) / 2,
                cardW,
                cardH);

            lblMarca.SetBounds(36, Math.Max(76, ClientSize.Height / 2 - 118), marcaW - 72, 112);
            lblMarcaDetalle.SetBounds(40, lblMarca.Bottom + 18, marcaW - 82, 92);

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

            AppTheme.ApplyRoundedRegion(panelFormulario, 14);
            AppTheme.ApplyRoundedRegion(btnIngresar, 8);
            AppTheme.ApplyRoundedRegion(btnRegistrar, 8);
            panelMarca.Invalidate();
            panelFormulario.Invalidate();
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
            new FrmRegistro().ShowDialog();
        }
    }
}
