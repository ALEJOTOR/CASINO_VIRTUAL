using BLL;
using ENTITY;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class UcPerfil : UserControl
    {
        private readonly Usuario _usuario;
        private readonly PartidaServicio _partidaSvc = new PartidaServicio();

        public UcPerfil(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
            // Problema visual que resuelve: perfil usa tarjetas, textos y colores comunes del tema global.
            AppTheme.ApplyView(this);
            AppTheme.ApplyCard(pnlDatosPersonales, 12);
            AppTheme.ApplyCard(pnlEstadisticas, 12);
            CargarDatos();
        }

        public UcPerfil()
        {
            InitializeComponent();
            // Problema visual que resuelve: el perfil conserva la misma estetica cuando se abre desde el Designer.
            AppTheme.ApplyView(this);
        }

        private void CargarDatos()
        {
            if (_usuario == null) return;

            lblNombreValor.Text = $"{_usuario.Nombre1} {_usuario.Nombre2} {_usuario.Apellido1} {_usuario.Apellido2}".Replace("  ", " ");
            lblUsernameValor.Text = _usuario.Username;
            lblCorreoValor.Text = _usuario.Correo;
            lblSaldoValor.Text = $"${_usuario.Saldo:N2}";
            lblSaldoValor.ForeColor = _usuario.Saldo > 0 ? CasinoTheme.Green : CasinoTheme.Red;

            try
            {
                EstadisticasUsuario stats = _partidaSvc.ObtenerEstadisticasUsuario(_usuario.IdUsuario);

                lblTotalPartidasValor.Text = stats.TotalPartidas.ToString();
                lblGanadasValor.Text = stats.PartidasGanadas.ToString();
                lblPerdidasValor.Text = stats.PartidasPerdidas.ToString();
                lblTotalApostadoValor.Text = $"${stats.TotalApostado:N2}";
                lblTotalGanadoValor.Text = $"${stats.TotalGanado:N2}";
                lblGananciaNetaValor.Text = $"${stats.GananciaNeta:N2}";
                lblGananciaNetaValor.ForeColor = stats.GananciaNeta >= 0 ? CasinoTheme.Green : CasinoTheme.Red;
                lblJuegoFavoritoValor.Text = stats.JuegoFavorito;
                lblRachaValor.Text = $"{stats.RachaActual} {(stats.TipoRacha == "ganando" ? "ganadas" : stats.TipoRacha == "perdiendo" ? "perdidas" : "")}";
                lblRachaValor.ForeColor = stats.TipoRacha == "ganando" ? CasinoTheme.Green : stats.TipoRacha == "perdiendo" ? CasinoTheme.Red : CasinoTheme.Muted;
            }
            catch
            {
                lblTotalPartidasValor.Text = "0";
                lblGanadasValor.Text = "0";
                lblPerdidasValor.Text = "0";
                lblTotalApostadoValor.Text = "$0";
                lblTotalGanadoValor.Text = "$0";
                lblGananciaNetaValor.Text = "$0";
                lblJuegoFavoritoValor.Text = "N/A";
                lblRachaValor.Text = "0";
            }
        }
    }
}
