using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            PrepararEncabezado();

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
            pnlMetricas.RowStyles.Clear();
            pnlMetricas.RowCount = 1;
            pnlMetricas.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
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

        private void PrepararEncabezado()
        {
            // Problema visual que resuelve: la billetera gana un encabezado propio y deja de parecer una seccion vacia.
            pnlHeader.BackColor = AppTheme.BgPrincipal;
            pnlMetricas.BackColor = AppTheme.BgPrincipal;
            pnlAcciones.BackColor = AppTheme.BgPrincipal;
            AppTheme.ApplyTitle(lblTitulo);
            AppTheme.ApplySubtitle(lblSubtitulo);
            pnlHeader.Paint -= PnlHeader_Paint;
            pnlHeader.Paint += PnlHeader_Paint;
            Resize -= UcBilletera_Resize;
            Resize += UcBilletera_Resize;
            ReubicarEncabezado();
        }

        private void UcBilletera_Resize(object sender, EventArgs e)
        {
            ReubicarEncabezado();
        }

        private void ReubicarEncabezado()
        {
            if (Width <= 0) return;
            int margen = Width < 900 ? 24 : 34;
            pnlHeader.Height = Width < 900 ? 104 : 118;
            lblTitulo.SetBounds(margen, 22, Math.Max(360, pnlHeader.Width - margen * 2), 42);
            lblSubtitulo.SetBounds(margen + 2, 68, Math.Max(360, pnlHeader.Width - margen * 2), 30);
            pnlHeader.Invalidate();
        }

        private void PnlHeader_Paint(object sender, PaintEventArgs e)
        {
            // Problema visual que resuelve: el encabezado se siente premium y separa visualmente la zona financiera.
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = pnlHeader.ClientRectangle;
            using (LinearGradientBrush fondo = new LinearGradientBrush(area, Color.FromArgb(10, 18, 34), Color.FromArgb(18, 31, 52), 0F))
                e.Graphics.FillRectangle(fondo, area);
            using (SolidBrush glow = new SolidBrush(Color.FromArgb(28, AppTheme.Dorado)))
                e.Graphics.FillEllipse(glow, area.Width - 280, -80, 360, 220);
            using (Pen linea = new Pen(Color.FromArgb(40, AppTheme.Dorado), 1))
                e.Graphics.DrawLine(linea, 28, area.Height - 1, area.Width - 28, area.Height - 1);
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
                // Problema visual que resuelve: las metricas se leen como tarjetas financieras consistentes.
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle area = new Rectangle(0, 0, tarjeta.Width - 1, tarjeta.Height - 1);
                using (LinearGradientBrush fondo = new LinearGradientBrush(area, Color.FromArgb(19, 29, 48), Color.FromArgb(12, 18, 32), 90F))
                    e.Graphics.FillRectangle(fondo, area);
                using (SolidBrush brush = new SolidBrush(acento))
                    e.Graphics.FillRectangle(brush, 0, 0, tarjeta.Width, 4);
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(18, acento)))
                    e.Graphics.FillEllipse(brush, tarjeta.Width - 120, -48, 180, 130);
                using (Pen borde = new Pen(Color.FromArgb(72, acento), 1))
                    e.Graphics.DrawRectangle(borde, 0, 0, tarjeta.Width - 1, tarjeta.Height - 1);
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
                AppTheme.ApplyRoundedRegion(tarjeta, 12);
            };

            return tarjeta;
        }

        private void CargarTarjetasAccion()
        {
            pnlAcciones.Controls.Clear();
            pnlAcciones.ColumnCount = 2;
            pnlAcciones.RowCount = 1;
            pnlAcciones.ColumnStyles.Clear();
            pnlAcciones.RowStyles.Clear();
            pnlAcciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            pnlAcciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            pnlAcciones.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

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
                // Problema visual que resuelve: deposito y retiro se ven como modulos financieros modernos y no formularios planos.
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle area = new Rectangle(0, 0, tarjeta.Width - 1, tarjeta.Height - 1);
                using (LinearGradientBrush fondo = new LinearGradientBrush(area, Color.FromArgb(18, 29, 48), Color.FromArgb(8, 13, 24), 90F))
                    e.Graphics.FillRectangle(fondo, area);
                using (SolidBrush brush = new SolidBrush(acento))
                    e.Graphics.FillRectangle(brush, 0, 0, tarjeta.Width, 5);
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(24, acento)))
                    e.Graphics.FillEllipse(brush, tarjeta.Width - 200, -80, 320, 220);
                using (Pen borde = new Pen(Color.FromArgb(90, acento), 1))
                    e.Graphics.DrawRectangle(borde, 0, 0, tarjeta.Width - 1, tarjeta.Height - 1);
            };

            Label lblIcono = ControlFactory.CrearLabel(textoBoton == "Depositar" ? "+" : "-", Color.White, new Font("Segoe UI", 22F, FontStyle.Bold), ContentAlignment.MiddleCenter);
            Label lblTitulo = ControlFactory.CrearLabel(titulo, AppTheme.Dorado, AppTheme.TituloGrande, ContentAlignment.MiddleLeft);
            Label lblDescripcion = ControlFactory.CrearLabel(descripcion, AppTheme.TextoSecundario, AppTheme.Subtitulo, ContentAlignment.MiddleLeft);
            Label lblMonto = ControlFactory.CrearLabel(etiquetaMonto, AppTheme.TextoPrimario, AppTheme.Cuerpo, ContentAlignment.MiddleLeft);
            TextBox txtMonto = new TextBox();
            Button btnAccion = new Button { Text = textoBoton };
            TableLayoutPanel pnlRapidos = new TableLayoutPanel
            {
                BackColor = Color.Transparent,
                ColumnCount = 2,
                RowCount = 2,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            pnlRapidos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            pnlRapidos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            pnlRapidos.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            pnlRapidos.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            AppTheme.ApplyTextBox(txtMonto);
            txtMonto.Font = AppTheme.Valor;
            txtMonto.TextAlign = HorizontalAlignment.Right;
            AppTheme.ApplyPrimaryButton(btnAccion, acento);
            btnAccion.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            EstilizarIconoAccion(lblIcono, acento);

            decimal[] montosRapidos = { 10000m, 50000m, 100000m, 500000m };
            for (int i = 0; i < montosRapidos.Length; i++)
            {
                decimal montoRapido = montosRapidos[i];
                Button btnRapido = CrearBotonMontoRapido(montoRapido, acento);
                btnRapido.Click += (s, e) =>
                {
                    txtMonto.Text = montoRapido.ToString("0");
                    MarcarMontoRapido(pnlRapidos, btnRapido, acento);
                };
                pnlRapidos.Controls.Add(btnRapido, i % 2, i / 2);
            }

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

            tarjeta.Controls.Add(lblIcono);
            tarjeta.Controls.Add(lblTitulo);
            tarjeta.Controls.Add(lblDescripcion);
            tarjeta.Controls.Add(lblMonto);
            tarjeta.Controls.Add(txtMonto);
            tarjeta.Controls.Add(pnlRapidos);
            tarjeta.Controls.Add(btnAccion);
            tarjeta.Resize += (s, e) =>
            {
                int pad = 24;
                int icono = 56;
                lblIcono.SetBounds(pad, 26, icono, icono);
                lblTitulo.SetBounds(pad + icono + 16, 24, tarjeta.Width - pad * 2 - icono - 16, 34);
                lblDescripcion.SetBounds(pad + icono + 18, 64, tarjeta.Width - pad * 2 - icono - 18, 42);
                lblMonto.SetBounds(pad, 128, tarjeta.Width - pad * 2, 22);
                txtMonto.SetBounds(pad, 156, tarjeta.Width - pad * 2, 38);
                pnlRapidos.SetBounds(pad, 208, tarjeta.Width - pad * 2, 92);
                AjustarBotonesRapidos(pnlRapidos);
                btnAccion.SetBounds(pad, Math.Max(304, tarjeta.Height - 70), tarjeta.Width - pad * 2, 46);
                AppTheme.ApplyRoundedRegion(tarjeta, 14);
                AppTheme.ApplyRoundedRegion(lblIcono, 16);
                AppTheme.ApplyRoundedRegion(btnAccion, 10);
            };

            return tarjeta;
        }

        private void EstilizarIconoAccion(Label icono, Color acento)
        {
            // Problema visual que resuelve: cada accion tiene un simbolo claro y consistente con su color.
            icono.BackColor = acento;
            icono.ForeColor = acento == AppTheme.Dorado ? Color.Black : Color.White;
        }

        private Button CrearBotonMontoRapido(decimal monto, Color acento)
        {
            Button boton = new Button
            {
                Text = FormatearMontoCorto(monto),
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 10, 10),
                TextAlign = ContentAlignment.MiddleCenter
            };
            AppTheme.ApplySecondaryButton(boton);
            boton.FlatAppearance.BorderColor = Color.FromArgb(115, acento);
            boton.ForeColor = AppTheme.TextoPrimario;
            boton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            return boton;
        }

        private void AjustarBotonesRapidos(TableLayoutPanel panel)
        {
            // Problema visual que resuelve: los accesos rapidos quedan en una grilla estable y no se enciman.
            foreach (Control control in panel.Controls)
            {
                control.Dock = DockStyle.Fill;
                AppTheme.ApplyRoundedRegion(control, 8);
            }
        }

        private void MarcarMontoRapido(TableLayoutPanel panel, Button seleccionado, Color acento)
        {
            // Problema visual que resuelve: al elegir un monto rapido queda claro cual se aplico al campo.
            foreach (Control control in panel.Controls)
            {
                if (control is Button boton)
                {
                    boton.BackColor = boton == seleccionado ? acento : AppTheme.BgElevated;
                    boton.ForeColor = boton == seleccionado && acento == AppTheme.Dorado ? Color.Black : AppTheme.TextoPrimario;
                    boton.FlatAppearance.BorderColor = boton == seleccionado ? acento : Color.FromArgb(115, acento);
                }
            }
        }

        private string FormatearMontoCorto(decimal monto)
        {
            if (monto >= 1000000m) return $"{monto / 1000000m:0.#}M";
            if (monto >= 1000m) return $"{monto / 1000m:0.#}K";
            return monto.ToString("0");
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
