namespace GUI
{
    partial class UcPerfil
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.tblPerfil = new System.Windows.Forms.TableLayoutPanel();
            this.pnlDatosPersonales = new System.Windows.Forms.Panel();
            this.lblNombreValor = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblUsernameValor = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblCorreoValor = new System.Windows.Forms.Label();
            this.lblCorreo = new System.Windows.Forms.Label();
            this.lblSaldoValor = new System.Windows.Forms.Label();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.pnlEstadisticas = new System.Windows.Forms.Panel();
            this.lblJuegoFavoritoValor = new System.Windows.Forms.Label();
            this.lblJuegoFavorito = new System.Windows.Forms.Label();
            this.lblRachaValor = new System.Windows.Forms.Label();
            this.lblRacha = new System.Windows.Forms.Label();
            this.lblGananciaNetaValor = new System.Windows.Forms.Label();
            this.lblGananciaNeta = new System.Windows.Forms.Label();
            this.lblTotalGanadoValor = new System.Windows.Forms.Label();
            this.lblTotalGanado = new System.Windows.Forms.Label();
            this.lblTotalApostadoValor = new System.Windows.Forms.Label();
            this.lblTotalApostado = new System.Windows.Forms.Label();
            this.lblPerdidasValor = new System.Windows.Forms.Label();
            this.lblPerdidas = new System.Windows.Forms.Label();
            this.lblGanadasValor = new System.Windows.Forms.Label();
            this.lblGanadas = new System.Windows.Forms.Label();
            this.lblTotalPartidasValor = new System.Windows.Forms.Label();
            this.lblTotalPartidas = new System.Windows.Forms.Label();
            this.tblPerfil.SuspendLayout();
            this.pnlDatosPersonales.SuspendLayout();
            this.pnlEstadisticas.SuspendLayout();
            this.SuspendLayout();

            this.lblTitulo.Font = new System.Drawing.Font("Georgia", 26F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(250, 204, 21);
            this.lblTitulo.Location = new System.Drawing.Point(34, 28);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(400, 44);
            this.lblTitulo.Text = "Mi Perfil";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblSubtitulo.Location = new System.Drawing.Point(34, 72);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new System.Drawing.Size(400, 24);
            this.lblSubtitulo.Text = "Datos personales y estadisticas de juego";
            this.lblSubtitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.tblPerfil.BackColor = System.Drawing.Color.FromArgb(8, 13, 24);
            this.tblPerfil.ColumnCount = 2;
            this.tblPerfil.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPerfil.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPerfil.Controls.Add(this.pnlDatosPersonales, 0, 0);
            this.tblPerfil.Controls.Add(this.pnlEstadisticas, 1, 0);
            this.tblPerfil.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblPerfil.Location = new System.Drawing.Point(0, 100);
            this.tblPerfil.Name = "tblPerfil";
            this.tblPerfil.Padding = new System.Windows.Forms.Padding(34, 24, 34, 34);
            this.tblPerfil.RowCount = 1;
            this.tblPerfil.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblPerfil.Size = new System.Drawing.Size(1000, 500);
            this.tblPerfil.TabIndex = 0;

            this.pnlDatosPersonales.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.pnlDatosPersonales.Controls.Add(this.lblNombreValor);
            this.pnlDatosPersonales.Controls.Add(this.lblNombre);
            this.pnlDatosPersonales.Controls.Add(this.lblUsernameValor);
            this.pnlDatosPersonales.Controls.Add(this.lblUsername);
            this.pnlDatosPersonales.Controls.Add(this.lblCorreoValor);
            this.pnlDatosPersonales.Controls.Add(this.lblCorreo);
            this.pnlDatosPersonales.Controls.Add(this.lblSaldoValor);
            this.pnlDatosPersonales.Controls.Add(this.lblSaldo);
            this.pnlDatosPersonales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDatosPersonales.Location = new System.Drawing.Point(37, 27);
            this.pnlDatosPersonales.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.pnlDatosPersonales.Name = "pnlDatosPersonales";
            this.pnlDatosPersonales.Padding = new System.Windows.Forms.Padding(24);
            this.pnlDatosPersonales.Size = new System.Drawing.Size(449, 436);
            this.pnlDatosPersonales.TabIndex = 0;

            int dpY = 24;
            int dpStep = 60;

            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNombre.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblNombre.Location = new System.Drawing.Point(24, dpY);
            this.lblNombre.Size = new System.Drawing.Size(180, 20);
            this.lblNombre.Text = "Nombre completo";
            this.lblNombreValor.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblNombreValor.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblNombreValor.Location = new System.Drawing.Point(24, dpY + 22);
            this.lblNombreValor.Size = new System.Drawing.Size(400, 24);
            this.lblNombreValor.Text = "-";

            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblUsername.Location = new System.Drawing.Point(24, dpY + dpStep);
            this.lblUsername.Size = new System.Drawing.Size(180, 20);
            this.lblUsername.Text = "Username";
            this.lblUsernameValor.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblUsernameValor.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblUsernameValor.Location = new System.Drawing.Point(24, dpY + dpStep + 22);
            this.lblUsernameValor.Size = new System.Drawing.Size(400, 24);
            this.lblUsernameValor.Text = "-";

            this.lblCorreo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCorreo.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblCorreo.Location = new System.Drawing.Point(24, dpY + dpStep * 2);
            this.lblCorreo.Size = new System.Drawing.Size(180, 20);
            this.lblCorreo.Text = "Correo electronico";
            this.lblCorreoValor.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblCorreoValor.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblCorreoValor.Location = new System.Drawing.Point(24, dpY + dpStep * 2 + 22);
            this.lblCorreoValor.Size = new System.Drawing.Size(400, 24);
            this.lblCorreoValor.Text = "-";

            this.lblSaldo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSaldo.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblSaldo.Location = new System.Drawing.Point(24, dpY + dpStep * 3);
            this.lblSaldo.Size = new System.Drawing.Size(180, 20);
            this.lblSaldo.Text = "Saldo actual";
            this.lblSaldoValor.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblSaldoValor.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this.lblSaldoValor.Location = new System.Drawing.Point(24, dpY + dpStep * 3 + 22);
            this.lblSaldoValor.Size = new System.Drawing.Size(400, 24);
            this.lblSaldoValor.Text = "$0.00";

            this.pnlEstadisticas.BackColor = System.Drawing.Color.FromArgb(17, 28, 50);
            this.pnlEstadisticas.Controls.Add(this.lblJuegoFavoritoValor);
            this.pnlEstadisticas.Controls.Add(this.lblJuegoFavorito);
            this.pnlEstadisticas.Controls.Add(this.lblRachaValor);
            this.pnlEstadisticas.Controls.Add(this.lblRacha);
            this.pnlEstadisticas.Controls.Add(this.lblGananciaNetaValor);
            this.pnlEstadisticas.Controls.Add(this.lblGananciaNeta);
            this.pnlEstadisticas.Controls.Add(this.lblTotalGanadoValor);
            this.pnlEstadisticas.Controls.Add(this.lblTotalGanado);
            this.pnlEstadisticas.Controls.Add(this.lblTotalApostadoValor);
            this.pnlEstadisticas.Controls.Add(this.lblTotalApostado);
            this.pnlEstadisticas.Controls.Add(this.lblPerdidasValor);
            this.pnlEstadisticas.Controls.Add(this.lblPerdidas);
            this.pnlEstadisticas.Controls.Add(this.lblGanadasValor);
            this.pnlEstadisticas.Controls.Add(this.lblGanadas);
            this.pnlEstadisticas.Controls.Add(this.lblTotalPartidasValor);
            this.pnlEstadisticas.Controls.Add(this.lblTotalPartidas);
            this.pnlEstadisticas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEstadisticas.Location = new System.Drawing.Point(509, 27);
            this.pnlEstadisticas.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.pnlEstadisticas.Name = "pnlEstadisticas";
            this.pnlEstadisticas.Padding = new System.Windows.Forms.Padding(24);
            this.pnlEstadisticas.Size = new System.Drawing.Size(451, 436);
            this.pnlEstadisticas.TabIndex = 1;

            int esY = 24;
            int esStep = 46;
            int statW = 200;

            this.lblTotalPartidas.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalPartidas.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblTotalPartidas.Location = new System.Drawing.Point(24, esY);
            this.lblTotalPartidas.Size = new System.Drawing.Size(180, 20);
            this.lblTotalPartidas.Text = "Total partidas";
            this.lblTotalPartidasValor.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalPartidasValor.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblTotalPartidasValor.Location = new System.Drawing.Point(statW, esY);
            this.lblTotalPartidasValor.Size = new System.Drawing.Size(220, 20);
            this.lblTotalPartidasValor.Text = "0";

            this.lblGanadas.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblGanadas.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblGanadas.Location = new System.Drawing.Point(24, esY + esStep);
            this.lblGanadas.Size = new System.Drawing.Size(180, 20);
            this.lblGanadas.Text = "Ganadas";
            this.lblGanadasValor.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblGanadasValor.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this.lblGanadasValor.Location = new System.Drawing.Point(statW, esY + esStep);
            this.lblGanadasValor.Size = new System.Drawing.Size(220, 20);
            this.lblGanadasValor.Text = "0";

            this.lblPerdidas.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPerdidas.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblPerdidas.Location = new System.Drawing.Point(24, esY + esStep * 2);
            this.lblPerdidas.Size = new System.Drawing.Size(180, 20);
            this.lblPerdidas.Text = "Perdidas";
            this.lblPerdidasValor.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblPerdidasValor.ForeColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this.lblPerdidasValor.Location = new System.Drawing.Point(statW, esY + esStep * 2);
            this.lblPerdidasValor.Size = new System.Drawing.Size(220, 20);
            this.lblPerdidasValor.Text = "0";

            this.lblTotalApostado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalApostado.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblTotalApostado.Location = new System.Drawing.Point(24, esY + esStep * 3);
            this.lblTotalApostado.Size = new System.Drawing.Size(180, 20);
            this.lblTotalApostado.Text = "Total apostado";
            this.lblTotalApostadoValor.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalApostadoValor.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblTotalApostadoValor.Location = new System.Drawing.Point(statW, esY + esStep * 3);
            this.lblTotalApostadoValor.Size = new System.Drawing.Size(220, 20);
            this.lblTotalApostadoValor.Text = "$0";

            this.lblTotalGanado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalGanado.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblTotalGanado.Location = new System.Drawing.Point(24, esY + esStep * 4);
            this.lblTotalGanado.Size = new System.Drawing.Size(180, 20);
            this.lblTotalGanado.Text = "Total ganado";
            this.lblTotalGanadoValor.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalGanadoValor.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this.lblTotalGanadoValor.Location = new System.Drawing.Point(statW, esY + esStep * 4);
            this.lblTotalGanadoValor.Size = new System.Drawing.Size(220, 20);
            this.lblTotalGanadoValor.Text = "$0";

            this.lblGananciaNeta.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblGananciaNeta.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblGananciaNeta.Location = new System.Drawing.Point(24, esY + esStep * 5);
            this.lblGananciaNeta.Size = new System.Drawing.Size(180, 20);
            this.lblGananciaNeta.Text = "Ganancia neta";
            this.lblGananciaNetaValor.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblGananciaNetaValor.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this.lblGananciaNetaValor.Location = new System.Drawing.Point(statW, esY + esStep * 5);
            this.lblGananciaNetaValor.Size = new System.Drawing.Size(220, 20);
            this.lblGananciaNetaValor.Text = "$0";

            this.lblRacha.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblRacha.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblRacha.Location = new System.Drawing.Point(24, esY + esStep * 6);
            this.lblRacha.Size = new System.Drawing.Size(180, 20);
            this.lblRacha.Text = "Racha actual";
            this.lblRachaValor.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRachaValor.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblRachaValor.Location = new System.Drawing.Point(statW, esY + esStep * 6);
            this.lblRachaValor.Size = new System.Drawing.Size(220, 20);
            this.lblRachaValor.Text = "0";

            this.lblJuegoFavorito.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblJuegoFavorito.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblJuegoFavorito.Location = new System.Drawing.Point(24, esY + esStep * 7);
            this.lblJuegoFavorito.Size = new System.Drawing.Size(180, 20);
            this.lblJuegoFavorito.Text = "Juego favorito";
            this.lblJuegoFavoritoValor.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblJuegoFavoritoValor.ForeColor = System.Drawing.Color.FromArgb(250, 204, 21);
            this.lblJuegoFavoritoValor.Location = new System.Drawing.Point(statW, esY + esStep * 7);
            this.lblJuegoFavoritoValor.Size = new System.Drawing.Size(220, 20);
            this.lblJuegoFavoritoValor.Text = "N/A";

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(8, 13, 24);
            this.Controls.Add(this.tblPerfil);
            this.Controls.Add(this.lblSubtitulo);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "UcPerfil";
            this.Size = new System.Drawing.Size(1000, 600);
            this.tblPerfil.ResumeLayout(false);
            this.pnlDatosPersonales.ResumeLayout(false);
            this.pnlEstadisticas.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.TableLayoutPanel tblPerfil;
        private System.Windows.Forms.Panel pnlDatosPersonales;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblNombreValor;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblUsernameValor;
        private System.Windows.Forms.Label lblCorreo;
        private System.Windows.Forms.Label lblCorreoValor;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Label lblSaldoValor;
        private System.Windows.Forms.Panel pnlEstadisticas;
        private System.Windows.Forms.Label lblTotalPartidas;
        private System.Windows.Forms.Label lblTotalPartidasValor;
        private System.Windows.Forms.Label lblGanadas;
        private System.Windows.Forms.Label lblGanadasValor;
        private System.Windows.Forms.Label lblPerdidas;
        private System.Windows.Forms.Label lblPerdidasValor;
        private System.Windows.Forms.Label lblTotalApostado;
        private System.Windows.Forms.Label lblTotalApostadoValor;
        private System.Windows.Forms.Label lblTotalGanado;
        private System.Windows.Forms.Label lblTotalGanadoValor;
        private System.Windows.Forms.Label lblGananciaNeta;
        private System.Windows.Forms.Label lblGananciaNetaValor;
        private System.Windows.Forms.Label lblRacha;
        private System.Windows.Forms.Label lblRachaValor;
        private System.Windows.Forms.Label lblJuegoFavorito;
        private System.Windows.Forms.Label lblJuegoFavoritoValor;
    }
}
