namespace GUI
{
    partial class UcSoporte
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.layoutPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHero = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.pnlContenido = new System.Windows.Forms.TableLayoutPanel();
            this.pnlBot = new System.Windows.Forms.Panel();
            this.lblIconoBot = new System.Windows.Forms.Label();
            this.lblBotTitulo = new System.Windows.Forms.Label();
            this.lblBotDescripcion = new System.Windows.Forms.Label();
            this.btnAbrirBot = new System.Windows.Forms.Button();
            this.lblBotUrl = new System.Windows.Forms.Label();
            this.pnlComandos = new System.Windows.Forms.Panel();
            this.lblComandosTitulo = new System.Windows.Forms.Label();
            this.lblComandos = new System.Windows.Forms.Label();
            this.pnlQr = new System.Windows.Forms.Panel();
            this.lblQrTitulo = new System.Windows.Forms.Label();
            this.picQrBot = new System.Windows.Forms.PictureBox();
            this.lblQrTexto = new System.Windows.Forms.Label();
            this.layoutPrincipal.SuspendLayout();
            this.pnlHero.SuspendLayout();
            this.pnlContenido.SuspendLayout();
            this.pnlBot.SuspendLayout();
            this.pnlComandos.SuspendLayout();
            this.pnlQr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picQrBot)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutPrincipal
            // 
            this.layoutPrincipal.ColumnCount = 1;
            this.layoutPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPrincipal.Controls.Add(this.pnlHero, 0, 0);
            this.layoutPrincipal.Controls.Add(this.pnlContenido, 0, 1);
            this.layoutPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPrincipal.Location = new System.Drawing.Point(0, 0);
            this.layoutPrincipal.Name = "layoutPrincipal";
            this.layoutPrincipal.Padding = new System.Windows.Forms.Padding(32, 28, 32, 32);
            this.layoutPrincipal.RowCount = 2;
            this.layoutPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 145F));
            this.layoutPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPrincipal.Size = new System.Drawing.Size(1100, 660);
            this.layoutPrincipal.TabIndex = 0;
            // 
            // pnlHero
            // 
            this.pnlHero.Controls.Add(this.lblTitulo);
            this.pnlHero.Controls.Add(this.lblSubtitulo);
            this.pnlHero.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHero.Location = new System.Drawing.Point(32, 28);
            this.pnlHero.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.pnlHero.Name = "pnlHero";
            this.pnlHero.Size = new System.Drawing.Size(1036, 129);
            this.pnlHero.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = false;
            this.lblTitulo.Location = new System.Drawing.Point(24, 22);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(650, 42);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Soporte Casino Royal";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSubtitulo
            // 
            this.lblSubtitulo.AutoSize = false;
            this.lblSubtitulo.Location = new System.Drawing.Point(26, 68);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new System.Drawing.Size(800, 42);
            this.lblSubtitulo.TabIndex = 1;
            this.lblSubtitulo.Text = "Conecta con el asistente de Telegram para consultar reglas, saldo, movimientos, historial y preguntas frecuentes.";
            // 
            // pnlContenido
            // 
            this.pnlContenido.ColumnCount = 2;
            this.pnlContenido.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56F));
            this.pnlContenido.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44F));
            this.pnlContenido.Controls.Add(this.pnlBot, 0, 0);
            this.pnlContenido.Controls.Add(this.pnlComandos, 1, 0);
            this.pnlContenido.Controls.Add(this.pnlQr, 0, 1);
            this.pnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContenido.Location = new System.Drawing.Point(32, 173);
            this.pnlContenido.Margin = new System.Windows.Forms.Padding(0);
            this.pnlContenido.Name = "pnlContenido";
            this.pnlContenido.RowCount = 2;
            this.pnlContenido.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58F));
            this.pnlContenido.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42F));
            this.pnlContenido.Size = new System.Drawing.Size(1036, 455);
            this.pnlContenido.TabIndex = 1;
            // 
            // pnlBot
            // 
            this.pnlBot.Controls.Add(this.lblIconoBot);
            this.pnlBot.Controls.Add(this.lblBotTitulo);
            this.pnlBot.Controls.Add(this.lblBotDescripcion);
            this.pnlBot.Controls.Add(this.btnAbrirBot);
            this.pnlBot.Controls.Add(this.lblBotUrl);
            this.pnlBot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBot.Location = new System.Drawing.Point(0, 0);
            this.pnlBot.Margin = new System.Windows.Forms.Padding(0, 0, 16, 16);
            this.pnlBot.Name = "pnlBot";
            this.pnlBot.Size = new System.Drawing.Size(564, 247);
            this.pnlBot.TabIndex = 0;
            // 
            // lblIconoBot
            // 
            this.lblIconoBot.AutoSize = false;
            this.lblIconoBot.Location = new System.Drawing.Point(28, 26);
            this.lblIconoBot.Name = "lblIconoBot";
            this.lblIconoBot.Size = new System.Drawing.Size(92, 92);
            this.lblIconoBot.TabIndex = 0;
            this.lblIconoBot.Text = "TG";
            this.lblIconoBot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBotTitulo
            // 
            this.lblBotTitulo.AutoSize = false;
            this.lblBotTitulo.Location = new System.Drawing.Point(140, 28);
            this.lblBotTitulo.Name = "lblBotTitulo";
            this.lblBotTitulo.Size = new System.Drawing.Size(360, 34);
            this.lblBotTitulo.TabIndex = 1;
            this.lblBotTitulo.Text = "Asistente de Telegram";
            // 
            // lblBotDescripcion
            // 
            this.lblBotDescripcion.AutoSize = false;
            this.lblBotDescripcion.Location = new System.Drawing.Point(142, 70);
            this.lblBotDescripcion.Name = "lblBotDescripcion";
            this.lblBotDescripcion.Size = new System.Drawing.Size(370, 72);
            this.lblBotDescripcion.TabIndex = 2;
            this.lblBotDescripcion.Text = "Abre el bot desde tu celular o navegador y escribe los comandos disponibles para recibir soporte rapido.";
            // 
            // btnAbrirBot
            // 
            this.btnAbrirBot.Location = new System.Drawing.Point(142, 158);
            this.btnAbrirBot.Name = "btnAbrirBot";
            this.btnAbrirBot.Size = new System.Drawing.Size(210, 42);
            this.btnAbrirBot.TabIndex = 3;
            this.btnAbrirBot.Text = "Abrir bot en Telegram";
            this.btnAbrirBot.UseVisualStyleBackColor = true;
            this.btnAbrirBot.Click += new System.EventHandler(this.btnAbrirBot_Click);
            // 
            // lblBotUrl
            // 
            this.lblBotUrl.AutoSize = false;
            this.lblBotUrl.Location = new System.Drawing.Point(142, 206);
            this.lblBotUrl.Name = "lblBotUrl";
            this.lblBotUrl.Size = new System.Drawing.Size(360, 22);
            this.lblBotUrl.TabIndex = 4;
            this.lblBotUrl.Text = "t.me/CasinoRoyalAsistenteBot";
            // 
            // pnlComandos
            // 
            this.pnlComandos.Controls.Add(this.lblComandosTitulo);
            this.pnlComandos.Controls.Add(this.lblComandos);
            this.pnlComandos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlComandos.Location = new System.Drawing.Point(580, 0);
            this.pnlComandos.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.pnlComandos.Name = "pnlComandos";
            this.pnlComandos.Size = new System.Drawing.Size(456, 247);
            this.pnlComandos.TabIndex = 1;
            // 
            // lblComandosTitulo
            // 
            this.lblComandosTitulo.AutoSize = false;
            this.lblComandosTitulo.Location = new System.Drawing.Point(24, 22);
            this.lblComandosTitulo.Name = "lblComandosTitulo";
            this.lblComandosTitulo.Size = new System.Drawing.Size(360, 30);
            this.lblComandosTitulo.TabIndex = 0;
            this.lblComandosTitulo.Text = "Comandos disponibles";
            // 
            // lblComandos
            // 
            this.lblComandos.AutoSize = false;
            this.lblComandos.Location = new System.Drawing.Point(26, 66);
            this.lblComandos.Name = "lblComandos";
            this.lblComandos.Size = new System.Drawing.Size(390, 150);
            this.lblComandos.TabIndex = 1;
            this.lblComandos.Text = "/start - Menu principal\r\n/reglas - Reglas de juegos\r\n/saldo - Consulta de saldo\r\n/transacciones - Movimientos\r\n/historial - Partidas recientes\r\n/faq - Preguntas frecuentes";
            // 
            // pnlQr
            // 
            this.pnlQr.Controls.Add(this.lblQrTitulo);
            this.pnlQr.Controls.Add(this.picQrBot);
            this.pnlQr.Controls.Add(this.lblQrTexto);
            this.pnlQr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlQr.Location = new System.Drawing.Point(0, 263);
            this.pnlQr.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.pnlQr.Name = "pnlQr";
            this.pnlQr.Size = new System.Drawing.Size(564, 192);
            this.pnlQr.TabIndex = 2;
            // 
            // lblQrTitulo
            // 
            this.lblQrTitulo.AutoSize = false;
            this.lblQrTitulo.Location = new System.Drawing.Point(24, 22);
            this.lblQrTitulo.Name = "lblQrTitulo";
            this.lblQrTitulo.Size = new System.Drawing.Size(300, 30);
            this.lblQrTitulo.TabIndex = 0;
            this.lblQrTitulo.Text = "Escanea el codigo QR";
            // 
            // picQrBot
            // 
            this.picQrBot.Location = new System.Drawing.Point(360, 18);
            this.picQrBot.Name = "picQrBot";
            this.picQrBot.Size = new System.Drawing.Size(150, 150);
            this.picQrBot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picQrBot.TabIndex = 1;
            this.picQrBot.TabStop = false;
            // 
            // lblQrTexto
            // 
            this.lblQrTexto.AutoSize = false;
            this.lblQrTexto.Location = new System.Drawing.Point(26, 62);
            this.lblQrTexto.Name = "lblQrTexto";
            this.lblQrTexto.Size = new System.Drawing.Size(300, 74);
            this.lblQrTexto.TabIndex = 2;
            this.lblQrTexto.Text = "Tambien puedes abrir el asistente desde tu celular escaneando este codigo.";
            // 
            // UcSoporte
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutPrincipal);
            this.Name = "UcSoporte";
            this.Size = new System.Drawing.Size(1100, 660);
            this.layoutPrincipal.ResumeLayout(false);
            this.pnlHero.ResumeLayout(false);
            this.pnlContenido.ResumeLayout(false);
            this.pnlBot.ResumeLayout(false);
            this.pnlComandos.ResumeLayout(false);
            this.pnlQr.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picQrBot)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel layoutPrincipal;
        private System.Windows.Forms.Panel pnlHero;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.TableLayoutPanel pnlContenido;
        private System.Windows.Forms.Panel pnlBot;
        private System.Windows.Forms.Label lblIconoBot;
        private System.Windows.Forms.Label lblBotTitulo;
        private System.Windows.Forms.Label lblBotDescripcion;
        private System.Windows.Forms.Button btnAbrirBot;
        private System.Windows.Forms.Label lblBotUrl;
        private System.Windows.Forms.Panel pnlComandos;
        private System.Windows.Forms.Label lblComandosTitulo;
        private System.Windows.Forms.Label lblComandos;
        private System.Windows.Forms.Panel pnlQr;
        private System.Windows.Forms.Label lblQrTitulo;
        private System.Windows.Forms.PictureBox picQrBot;
        private System.Windows.Forms.Label lblQrTexto;
    }
}
