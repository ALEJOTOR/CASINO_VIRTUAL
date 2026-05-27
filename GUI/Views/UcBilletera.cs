using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class UcBilletera : UserControl
    {
        public event EventHandler SaldoActualizado;

        private readonly Usuario _usuario;
        private readonly TransaccionServicio _transSvc = new TransaccionServicio();
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();

        public UcBilletera(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
            // Problema visual que resuelve: la billetera abre con el fondo y la tipografia del tema antes de cargar datos.
            AppTheme.ApplyView(this);
            CargarVista();
        }

        public UcBilletera()
        {
            InitializeComponent();
            // Problema visual que resuelve: la vista vacia de billetera tambien respeta el tema en el Designer.
            AppTheme.ApplyView(this);
        }

        private void CargarVista()
        {
            // Problema visual que resuelve: la billetera usa la misma base oscura y tipografia del lobby.
            AppTheme.ApplyView(this);

            if (_usuario == null) return;

            IList<Transaccion> movimientos = new List<Transaccion>();
            try
            {
                movimientos = _transSvc.ObtenerPorUsuario(_usuario.IdUsuario);
            }
            catch
            {
                movimientos = new List<Transaccion>();
            }

            decimal totalDepositos = movimientos
                .Where(t => (t.Tipo ?? "").ToLower().Contains("deposito"))
                .Sum(t => t.Monto);
            decimal totalRetiros = movimientos
                .Where(t => (t.Tipo ?? "").ToLower().Contains("retiro"))
                .Sum(t => t.Monto);
            int cantidadMovimientos = movimientos.Count;

            pnlMetricas.Controls.Clear();
            pnlMetricas.ColumnCount = 4;
            pnlMetricas.ColumnStyles.Clear();
            pnlMetricas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
            pnlMetricas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23F));
            pnlMetricas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23F));
            pnlMetricas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23F));

            pnlMetricas.Controls.Add(CrearMetricaBilletera("Saldo disponible", $"${_usuario.Saldo:N2}", _usuario.Saldo > 0 ? "Cuenta activa" : "Sin saldo", AppTheme.Verde), 0, 0);
            pnlMetricas.Controls.Add(CrearMetricaBilletera("Depositos", $"${totalDepositos:N2}", "Total registrado", AppTheme.Azul), 1, 0);
            pnlMetricas.Controls.Add(CrearMetricaBilletera("Retiros", $"${totalRetiros:N2}", "Total retirado", AppTheme.Rojo), 2, 0);
            pnlMetricas.Controls.Add(CrearMetricaBilletera("Movimientos", cantidadMovimientos.ToString(), "En transacciones", AppTheme.Dorado), 3, 0);

            CargarTarjetasAccion();
        }

        private Panel CrearMetricaBilletera(string titulo, string valor, string detalle, Color acento)
        {
            Panel tarjeta = new Panel
            {
                BackColor = AppTheme.BgCard,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 14, 0)
            };
            tarjeta.Paint += (s, e) =>
            {
                CasinoTheme.DrawBorderedPanel(e.Graphics, new Rectangle(0, 0, tarjeta.Width, tarjeta.Height), CasinoTheme.Surface, CasinoTheme.Border);
                using (SolidBrush brush = new SolidBrush(acento))
                    e.Graphics.FillRectangle(brush, 0, 0, tarjeta.Width, 5);
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(18, acento)))
                    e.Graphics.FillRectangle(brush, 1, 5, tarjeta.Width - 2, tarjeta.Height - 6);
            };

            Label lblTitulo = ControlFactory.CrearLabel(titulo, AppTheme.TextoSecundario, AppTheme.Subtitulo, ContentAlignment.MiddleLeft);
            Label lblValor = ControlFactory.CrearLabel(valor, acento, AppTheme.TituloGrande, ContentAlignment.MiddleLeft);
            Label lblDetalle = ControlFactory.CrearLabel(detalle, AppTheme.TextoSecundario, AppTheme.Subtitulo, ContentAlignment.MiddleLeft);

            tarjeta.Controls.Add(lblTitulo);
            tarjeta.Controls.Add(lblValor);
            tarjeta.Controls.Add(lblDetalle);
            tarjeta.Resize += (s, e) =>
            {
                int pad = 18;
                lblTitulo.SetBounds(pad, 18, tarjeta.Width - pad * 2, 22);
                lblValor.SetBounds(pad, 44, tarjeta.Width - pad * 2, 38);
                lblDetalle.SetBounds(pad, 86, tarjeta.Width - pad * 2, 24);
            };

            return tarjeta;
        }

        private void CargarTarjetasAccion()
        {
            pnlAcciones.Controls.Clear();
            pnlAcciones.ColumnCount = 2;
            pnlAcciones.ColumnStyles.Clear();
            pnlAcciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            pnlAcciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            Panel tarjetaDeposito = CrearTarjetaBilletera(
                "Depositar saldo",
                "Agrega dinero a tu cuenta y dejalo listo para jugar.",
                "Monto a depositar",
                "Depositar",
                AppTheme.Verde,
                monto => _transSvc.RealizarDeposito(_usuario.IdUsuario, monto));
            tarjetaDeposito.Margin = new Padding(0, 0, 10, 0);

            Panel tarjetaRetiro = CrearTarjetaBilletera(
                "Retirar saldo",
                "Retira una parte de tu saldo disponible.",
                "Monto a retirar",
                "Retirar",
                AppTheme.Rojo,
                monto => _transSvc.RealizarRetiro(_usuario.IdUsuario, monto));
            tarjetaRetiro.Margin = new Padding(10, 0, 0, 0);

            pnlAcciones.Controls.Add(tarjetaDeposito, 0, 0);
            pnlAcciones.Controls.Add(tarjetaRetiro, 1, 0);
        }

        private Panel CrearTarjetaBilletera(string titulo, string descripcion, string etiquetaMonto, string textoBoton, Color acento, Func<decimal, string> accion)
        {
            Panel tarjeta = new Panel
            {
                BackColor = AppTheme.BgCard,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 14),
                Padding = new Padding(24)
            };
            tarjeta.Paint += (s, e) =>
            {
                CasinoTheme.DrawBorderedPanel(e.Graphics, new Rectangle(0, 0, tarjeta.Width, tarjeta.Height), CasinoTheme.Surface, CasinoTheme.Border);
                using (SolidBrush brush = new SolidBrush(acento))
                    e.Graphics.FillRectangle(brush, 0, 0, tarjeta.Width, 6);
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(24, acento)))
                    e.Graphics.FillRectangle(brush, 1, 6, tarjeta.Width - 2, 54);
            };

            Label lblTitulo = ControlFactory.CrearLabel(titulo, AppTheme.Dorado, AppTheme.TituloGrande, ContentAlignment.MiddleLeft);
            Label lblDescripcion = ControlFactory.CrearLabel(descripcion, AppTheme.TextoSecundario, AppTheme.Subtitulo, ContentAlignment.MiddleLeft);
            Label lblMonto = ControlFactory.CrearLabel(etiquetaMonto, AppTheme.TextoPrimario, AppTheme.Cuerpo, ContentAlignment.MiddleLeft);
            TextBox txtMonto = new TextBox();
            Button btnAccion = new Button { Text = textoBoton };

            AppTheme.ApplyTextBox(txtMonto);
            txtMonto.Font = AppTheme.Valor;
            AppTheme.ApplyPrimaryButton(btnAccion, acento);

            btnAccion.Click += (s, e) =>
            {
                if (!decimal.TryParse(txtMonto.Text, out decimal monto) || monto <= 0)
                {
                    MessageBox.Show("Ingrese un monto valido.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnAccion.Enabled = false;
                try
                {
                    string resultado = accion(monto);
                    bool ok = resultado == "Deposito realizado correctamente." ||
                              resultado == "Guardado correctamente.";

                    MessageBox.Show(resultado, ok ? "Exito" : "Error",
                        MessageBoxButtons.OK,
                        ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                    if (ok)
                    {
                        txtMonto.Clear();
                        RecargarDatos();
                        CargarVista();
                    }
                }
                finally
                {
                    btnAccion.Enabled = true;
                }
            };

            tarjeta.Controls.Add(lblTitulo);
            tarjeta.Controls.Add(lblDescripcion);
            tarjeta.Controls.Add(lblMonto);
            tarjeta.Controls.Add(txtMonto);
            tarjeta.Controls.Add(btnAccion);
            tarjeta.Resize += (s, e) =>
            {
                int pad = 24;
                lblTitulo.SetBounds(pad, 24, tarjeta.Width - pad * 2, 30);
                lblDescripcion.SetBounds(pad, 62, tarjeta.Width - pad * 2, 38);
                lblMonto.SetBounds(pad, 118, tarjeta.Width - pad * 2, 20);
                txtMonto.SetBounds(pad, 146, Math.Min(300, tarjeta.Width - pad * 2), 36);
                btnAccion.SetBounds(pad, 198, Math.Min(220, tarjeta.Width - pad * 2), 42);
            };

            return tarjeta;
        }

        private void RecargarDatos()
        {
            if (_usuario == null) return;
            Usuario actualizado = _usuarioSvc.ObtenerPorId(_usuario.IdUsuario);
            if (actualizado != null) _usuario.Saldo = actualizado.Saldo;
            SaldoActualizado?.Invoke(this, EventArgs.Empty);
        }
    }
}
