namespace GUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.panelNavbar = new System.Windows.Forms.Panel();
            this.btnCerrarSesion = new System.Windows.Forms.Button();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.inicioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.juegosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.promocionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.soporteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlContenido = new System.Windows.Forms.Panel();
            this.layoutInicio = new System.Windows.Forms.TableLayoutPanel();
            this.panelHero = new System.Windows.Forms.Panel();
            this.lblDeposito = new System.Windows.Forms.Label();
            this.txtMontoDeposito = new System.Windows.Forms.TextBox();
            this.btnDepositar = new System.Windows.Forms.Button();
            this.btnHistorial = new System.Windows.Forms.Button();
            this.lblBienvenido = new System.Windows.Forms.Label();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.tlpJuegos = new System.Windows.Forms.TableLayoutPanel();
            this.panelMinas = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnMinas = new System.Windows.Forms.Button();
            this.lblDescripcionMinas = new System.Windows.Forms.Label();
            this.lblMinas = new System.Windows.Forms.Label();
            this.panelRuleta = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.btnRuleta = new System.Windows.Forms.Button();
            this.lblDescripcionRuleta = new System.Windows.Forms.Label();
            this.lblRuleta = new System.Windows.Forms.Label();
            this.panelSlot = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnSlot = new System.Windows.Forms.Button();
            this.lblDescripcionSlot = new System.Windows.Forms.Label();
            this.lblSlot = new System.Windows.Forms.Label();
            this.tlpStats = new System.Windows.Forms.TableLayoutPanel();
            this.lblUsuarios = new System.Windows.Forms.Label();
            this.lblPremios = new System.Windows.Forms.Label();
            this.lblSoporte = new System.Windows.Forms.Label();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.lblFooter = new System.Windows.Forms.Label();
            this.mainLayout.SuspendLayout();
            this.panelNavbar.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.pnlContenido.SuspendLayout();
            this.layoutInicio.SuspendLayout();
            this.panelHero.SuspendLayout();
            this.tlpJuegos.SuspendLayout();
            this.panelMinas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelRuleta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panelSlot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tlpStats.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(14)))), ((int)(((byte)(26)))));
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainLayout.Controls.Add(this.panelNavbar, 0, 0);
            this.mainLayout.Controls.Add(this.pnlContenido, 0, 1);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 2;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Size = new System.Drawing.Size(1081, 670);
            this.mainLayout.TabIndex = 0;
            // 
            // panelNavbar
            // 
            this.panelNavbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(18)))), ((int)(((byte)(32)))));
            this.panelNavbar.Controls.Add(this.btnCerrarSesion);
            this.panelNavbar.Controls.Add(this.lblSaldo);
            this.panelNavbar.Controls.Add(this.menuStrip);
            this.panelNavbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNavbar.Location = new System.Drawing.Point(3, 3);
            this.panelNavbar.Name = "panelNavbar";
            this.panelNavbar.Size = new System.Drawing.Size(1075, 54);
            this.panelNavbar.TabIndex = 0;
            // 
            // btnCerrarSesion
            // 
            this.btnCerrarSesion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrarSesion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnCerrarSesion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrarSesion.FlatAppearance.BorderSize = 0;
            this.btnCerrarSesion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarSesion.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnCerrarSesion.ForeColor = System.Drawing.Color.White;
            this.btnCerrarSesion.Location = new System.Drawing.Point(952, 12);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Size = new System.Drawing.Size(112, 30);
            this.btnCerrarSesion.TabIndex = 4;
            this.btnCerrarSesion.Text = "Cerrar sesion";
            this.btnCerrarSesion.UseVisualStyleBackColor = false;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);
            // 
            // lblSaldo
            // 
            this.lblSaldo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSaldo.AutoSize = true;
            this.lblSaldo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(36)))), ((int)(((byte)(60)))));
            this.lblSaldo.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.lblSaldo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(222)))), ((int)(((byte)(128)))));
            this.lblSaldo.Location = new System.Drawing.Point(760, 6);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Padding = new System.Windows.Forms.Padding(10);
            this.lblSaldo.Size = new System.Drawing.Size(166, 41);
            this.lblSaldo.TabIndex = 3;
            this.lblSaldo.Text = "Saldo: $250.000";
            this.lblSaldo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(18)))), ((int)(((byte)(32)))));
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inicioToolStripMenuItem,
            this.juegosToolStripMenuItem,
            this.promocionesToolStripMenuItem,
            this.soporteToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1075, 54);
            this.menuStrip.TabIndex = 1;
            // 
            // inicioToolStripMenuItem
            // 
            this.inicioToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.inicioToolStripMenuItem.Name = "inicioToolStripMenuItem";
            this.inicioToolStripMenuItem.Size = new System.Drawing.Size(67, 50);
            this.inicioToolStripMenuItem.Text = "Inicio";
            // 
            // juegosToolStripMenuItem
            // 
            this.juegosToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.juegosToolStripMenuItem.Name = "juegosToolStripMenuItem";
            this.juegosToolStripMenuItem.Size = new System.Drawing.Size(85, 50);
            this.juegosToolStripMenuItem.Text = "Juegos";
            // 
            // promocionesToolStripMenuItem
            // 
            this.promocionesToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.promocionesToolStripMenuItem.Name = "promocionesToolStripMenuItem";
            this.promocionesToolStripMenuItem.Size = new System.Drawing.Size(135, 50);
            this.promocionesToolStripMenuItem.Text = "Promociones";
            // 
            // soporteToolStripMenuItem
            // 
            this.soporteToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.soporteToolStripMenuItem.Name = "soporteToolStripMenuItem";
            this.soporteToolStripMenuItem.Size = new System.Drawing.Size(90, 50);
            this.soporteToolStripMenuItem.Text = "Soporte";
            // 
            // pnlContenido
            // 
            this.pnlContenido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(14)))), ((int)(((byte)(26)))));
            this.pnlContenido.Controls.Add(this.layoutInicio);
            this.pnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContenido.Location = new System.Drawing.Point(3, 63);
            this.pnlContenido.Name = "pnlContenido";
            this.pnlContenido.Size = new System.Drawing.Size(1075, 604);
            this.pnlContenido.TabIndex = 1;
            // 
            // layoutInicio
            // 
            this.layoutInicio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(14)))), ((int)(((byte)(26)))));
            this.layoutInicio.ColumnCount = 1;
            this.layoutInicio.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutInicio.Controls.Add(this.panelHero, 0, 0);
            this.layoutInicio.Controls.Add(this.tlpJuegos, 0, 1);
            this.layoutInicio.Controls.Add(this.tlpStats, 0, 2);
            this.layoutInicio.Controls.Add(this.panelFooter, 0, 3);
            this.layoutInicio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutInicio.Location = new System.Drawing.Point(0, 0);
            this.layoutInicio.Name = "layoutInicio";
            this.layoutInicio.RowCount = 4;
            this.layoutInicio.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.layoutInicio.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutInicio.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.layoutInicio.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutInicio.Size = new System.Drawing.Size(1075, 604);
            this.layoutInicio.TabIndex = 0;
            // 
            // panelHero
            // 
            this.panelHero.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(14)))), ((int)(((byte)(26)))));
            this.panelHero.Controls.Add(this.lblDeposito);
            this.panelHero.Controls.Add(this.txtMontoDeposito);
            this.panelHero.Controls.Add(this.btnDepositar);
            this.panelHero.Controls.Add(this.btnHistorial);
            this.panelHero.Controls.Add(this.lblBienvenido);
            this.panelHero.Controls.Add(this.lblDescripcion);
            this.panelHero.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHero.Location = new System.Drawing.Point(3, 3);
            this.panelHero.Name = "panelHero";
            this.panelHero.Size = new System.Drawing.Size(1069, 154);
            this.panelHero.TabIndex = 1;
            // 
            // lblDeposito
            // 
            this.lblDeposito.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDeposito.AutoSize = true;
            this.lblDeposito.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDeposito.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDeposito.Location = new System.Drawing.Point(781, 24);
            this.lblDeposito.Name = "lblDeposito";
            this.lblDeposito.Size = new System.Drawing.Size(132, 19);
            this.lblDeposito.TabIndex = 2;
            this.lblDeposito.Text = "Recargar saldo ($)";
            // 
            // txtMontoDeposito
            // 
            this.txtMontoDeposito.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMontoDeposito.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtMontoDeposito.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMontoDeposito.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtMontoDeposito.Location = new System.Drawing.Point(785, 50);
            this.txtMontoDeposito.Name = "txtMontoDeposito";
            this.txtMontoDeposito.Size = new System.Drawing.Size(130, 27);
            this.txtMontoDeposito.TabIndex = 3;
            // 
            // btnDepositar
            // 
            this.btnDepositar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDepositar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.btnDepositar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDepositar.FlatAppearance.BorderSize = 0;
            this.btnDepositar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDepositar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDepositar.ForeColor = System.Drawing.Color.White;
            this.btnDepositar.Location = new System.Drawing.Point(925, 49);
            this.btnDepositar.Name = "btnDepositar";
            this.btnDepositar.Size = new System.Drawing.Size(125, 29);
            this.btnDepositar.TabIndex = 4;
            this.btnDepositar.Text = "Depositar";
            this.btnDepositar.UseVisualStyleBackColor = false;
            this.btnDepositar.Click += new System.EventHandler(this.btnDepositar_Click);
            // 
            // btnHistorial
            // 
            this.btnHistorial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHistorial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnHistorial.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHistorial.FlatAppearance.BorderSize = 0;
            this.btnHistorial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistorial.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnHistorial.ForeColor = System.Drawing.Color.White;
            this.btnHistorial.Location = new System.Drawing.Point(785, 91);
            this.btnHistorial.Name = "btnHistorial";
            this.btnHistorial.Size = new System.Drawing.Size(265, 34);
            this.btnHistorial.TabIndex = 5;
            this.btnHistorial.Text = "Ver historial";
            this.btnHistorial.UseVisualStyleBackColor = false;
            this.btnHistorial.Click += new System.EventHandler(this.btnHistorial_Click);
            // 
            // lblBienvenido
            // 
            this.lblBienvenido.AutoSize = true;
            this.lblBienvenido.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblBienvenido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(204)))), ((int)(((byte)(21)))));
            this.lblBienvenido.Location = new System.Drawing.Point(20, 18);
            this.lblBienvenido.Name = "lblBienvenido";
            this.lblBienvenido.Size = new System.Drawing.Size(441, 45);
            this.lblBienvenido.TabIndex = 0;
            this.lblBienvenido.Text = "Bienvenido al Casino Virtual";
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblDescripcion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.lblDescripcion.Location = new System.Drawing.Point(22, 64);
            this.lblDescripcion.MaximumSize = new System.Drawing.Size(700, 0);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(678, 42);
            this.lblDescripcion.TabIndex = 1;
            this.lblDescripcion.Text = "Disfruta de los mejores juegos online, apuesta y vive la experiencia de un casino" +
    " moderno desde cualquier lugar.";
            // 
            // tlpJuegos
            // 
            this.tlpJuegos.ColumnCount = 3;
            this.tlpJuegos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpJuegos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpJuegos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpJuegos.Controls.Add(this.panelMinas, 0, 0);
            this.tlpJuegos.Controls.Add(this.panelRuleta, 1, 0);
            this.tlpJuegos.Controls.Add(this.panelSlot, 2, 0);
            this.tlpJuegos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpJuegos.Location = new System.Drawing.Point(3, 163);
            this.tlpJuegos.Name = "tlpJuegos";
            this.tlpJuegos.Padding = new System.Windows.Forms.Padding(20, 18, 20, 10);
            this.tlpJuegos.RowCount = 1;
            this.tlpJuegos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpJuegos.Size = new System.Drawing.Size(1069, 323);
            this.tlpJuegos.TabIndex = 2;
            // 
            // panelMinas
            // 
            this.panelMinas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(34)))), ((int)(((byte)(56)))));
            this.panelMinas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMinas.Controls.Add(this.pictureBox1);
            this.panelMinas.Controls.Add(this.btnMinas);
            this.panelMinas.Controls.Add(this.lblDescripcionMinas);
            this.panelMinas.Controls.Add(this.lblMinas);
            this.panelMinas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMinas.Location = new System.Drawing.Point(28, 26);
            this.panelMinas.Margin = new System.Windows.Forms.Padding(8);
            this.panelMinas.Name = "panelMinas";
            this.panelMinas.Padding = new System.Windows.Forms.Padding(10);
            this.panelMinas.Size = new System.Drawing.Size(329, 285);
            this.panelMinas.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(10, 88);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(307, 150);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // btnMinas
            // 
            this.btnMinas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.btnMinas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinas.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnMinas.FlatAppearance.BorderSize = 0;
            this.btnMinas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinas.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinas.ForeColor = System.Drawing.Color.White;
            this.btnMinas.Location = new System.Drawing.Point(10, 238);
            this.btnMinas.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.btnMinas.Name = "btnMinas";
            this.btnMinas.Size = new System.Drawing.Size(307, 35);
            this.btnMinas.TabIndex = 0;
            this.btnMinas.Text = "Entrar al Juego";
            this.btnMinas.UseVisualStyleBackColor = false;
            this.btnMinas.Click += new System.EventHandler(this.btnMinas_Click);
            // 
            // lblDescripcionMinas
            // 
            this.lblDescripcionMinas.AutoSize = true;
            this.lblDescripcionMinas.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDescripcionMinas.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescripcionMinas.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDescripcionMinas.Location = new System.Drawing.Point(10, 40);
            this.lblDescripcionMinas.MaximumSize = new System.Drawing.Size(220, 48);
            this.lblDescripcionMinas.Name = "lblDescripcionMinas";
            this.lblDescripcionMinas.Size = new System.Drawing.Size(219, 48);
            this.lblDescripcionMinas.TabIndex = 1;
            this.lblDescripcionMinas.Text = "Encuentra las casillas seguras y evita las minas para multiplicar tus ganancias.";
            // 
            // lblMinas
            // 
            this.lblMinas.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMinas.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblMinas.ForeColor = System.Drawing.Color.Gold;
            this.lblMinas.Location = new System.Drawing.Point(10, 10);
            this.lblMinas.Name = "lblMinas";
            this.lblMinas.Size = new System.Drawing.Size(307, 30);
            this.lblMinas.TabIndex = 2;
            this.lblMinas.Text = "Busca Minas";
            // 
            // panelRuleta
            // 
            this.panelRuleta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(34)))), ((int)(((byte)(56)))));
            this.panelRuleta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRuleta.Controls.Add(this.pictureBox3);
            this.panelRuleta.Controls.Add(this.btnRuleta);
            this.panelRuleta.Controls.Add(this.lblDescripcionRuleta);
            this.panelRuleta.Controls.Add(this.lblRuleta);
            this.panelRuleta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRuleta.Location = new System.Drawing.Point(373, 26);
            this.panelRuleta.Margin = new System.Windows.Forms.Padding(8);
            this.panelRuleta.Name = "panelRuleta";
            this.panelRuleta.Padding = new System.Windows.Forms.Padding(10);
            this.panelRuleta.Size = new System.Drawing.Size(329, 285);
            this.panelRuleta.TabIndex = 1;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(10, 88);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(307, 150);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 5;
            this.pictureBox3.TabStop = false;
            // 
            // btnRuleta
            // 
            this.btnRuleta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnRuleta.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRuleta.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnRuleta.FlatAppearance.BorderSize = 0;
            this.btnRuleta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRuleta.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRuleta.ForeColor = System.Drawing.Color.White;
            this.btnRuleta.Location = new System.Drawing.Point(10, 238);
            this.btnRuleta.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.btnRuleta.Name = "btnRuleta";
            this.btnRuleta.Size = new System.Drawing.Size(307, 35);
            this.btnRuleta.TabIndex = 0;
            this.btnRuleta.Text = "Entrar al Juego";
            this.btnRuleta.UseVisualStyleBackColor = false;
            this.btnRuleta.Click += new System.EventHandler(this.btnRuleta_Click);
            // 
            // lblDescripcionRuleta
            // 
            this.lblDescripcionRuleta.AutoSize = true;
            this.lblDescripcionRuleta.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDescripcionRuleta.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescripcionRuleta.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDescripcionRuleta.Location = new System.Drawing.Point(10, 40);
            this.lblDescripcionRuleta.MaximumSize = new System.Drawing.Size(220, 48);
            this.lblDescripcionRuleta.Name = "lblDescripcionRuleta";
            this.lblDescripcionRuleta.Size = new System.Drawing.Size(216, 48);
            this.lblDescripcionRuleta.TabIndex = 1;
            this.lblDescripcionRuleta.Text = "Apuesta al color o al número y prueba tu suerte en la ruleta clásica.";
            // 
            // lblRuleta
            // 
            this.lblRuleta.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRuleta.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblRuleta.ForeColor = System.Drawing.Color.Gold;
            this.lblRuleta.Location = new System.Drawing.Point(10, 10);
            this.lblRuleta.Name = "lblRuleta";
            this.lblRuleta.Size = new System.Drawing.Size(307, 30);
            this.lblRuleta.TabIndex = 2;
            this.lblRuleta.Text = "Ruleta";
            // 
            // panelSlot
            // 
            this.panelSlot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(34)))), ((int)(((byte)(56)))));
            this.panelSlot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSlot.Controls.Add(this.pictureBox2);
            this.panelSlot.Controls.Add(this.btnSlot);
            this.panelSlot.Controls.Add(this.lblDescripcionSlot);
            this.panelSlot.Controls.Add(this.lblSlot);
            this.panelSlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSlot.Location = new System.Drawing.Point(718, 26);
            this.panelSlot.Margin = new System.Windows.Forms.Padding(8);
            this.panelSlot.Name = "panelSlot";
            this.panelSlot.Padding = new System.Windows.Forms.Padding(10);
            this.panelSlot.Size = new System.Drawing.Size(329, 285);
            this.panelSlot.TabIndex = 2;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(10, 88);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(307, 150);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // btnSlot
            // 
            this.btnSlot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnSlot.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSlot.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSlot.FlatAppearance.BorderSize = 0;
            this.btnSlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSlot.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSlot.ForeColor = System.Drawing.Color.White;
            this.btnSlot.Location = new System.Drawing.Point(10, 238);
            this.btnSlot.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.btnSlot.Name = "btnSlot";
            this.btnSlot.Size = new System.Drawing.Size(307, 35);
            this.btnSlot.TabIndex = 0;
            this.btnSlot.Text = "Entrar al Juego";
            this.btnSlot.UseVisualStyleBackColor = false;
            this.btnSlot.Click += new System.EventHandler(this.btnSlot_Click);
            // 
            // lblDescripcionSlot
            // 
            this.lblDescripcionSlot.AutoSize = true;
            this.lblDescripcionSlot.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDescripcionSlot.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescripcionSlot.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDescripcionSlot.Location = new System.Drawing.Point(10, 40);
            this.lblDescripcionSlot.MaximumSize = new System.Drawing.Size(220, 48);
            this.lblDescripcionSlot.Name = "lblDescripcionSlot";
            this.lblDescripcionSlot.Size = new System.Drawing.Size(217, 48);
            this.lblDescripcionSlot.TabIndex = 1;
            this.lblDescripcionSlot.Text = "Gira los rodillos y consigue combinaciones ganadoras para obtener premios.";
            // 
            // lblSlot
            // 
            this.lblSlot.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSlot.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblSlot.ForeColor = System.Drawing.Color.Gold;
            this.lblSlot.Location = new System.Drawing.Point(10, 10);
            this.lblSlot.Name = "lblSlot";
            this.lblSlot.Size = new System.Drawing.Size(307, 30);
            this.lblSlot.TabIndex = 2;
            this.lblSlot.Text = "Tragamonedas";
            // 
            // tlpStats
            // 
            this.tlpStats.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(18)))), ((int)(((byte)(32)))));
            this.tlpStats.ColumnCount = 3;
            this.tlpStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpStats.Controls.Add(this.lblUsuarios, 0, 0);
            this.tlpStats.Controls.Add(this.lblPremios, 1, 0);
            this.tlpStats.Controls.Add(this.lblSoporte, 2, 0);
            this.tlpStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpStats.Location = new System.Drawing.Point(3, 489);
            this.tlpStats.Name = "tlpStats";
            this.tlpStats.RowCount = 1;
            this.tlpStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpStats.Size = new System.Drawing.Size(1069, 74);
            this.tlpStats.TabIndex = 3;
            // 
            // lblUsuarios
            // 
            this.lblUsuarios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUsuarios.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold);
            this.lblUsuarios.ForeColor = System.Drawing.Color.Gold;
            this.lblUsuarios.Location = new System.Drawing.Point(3, 0);
            this.lblUsuarios.Name = "lblUsuarios";
            this.lblUsuarios.Size = new System.Drawing.Size(352, 74);
            this.lblUsuarios.TabIndex = 0;
            this.lblUsuarios.Text = "+10K\nUsuarios";
            this.lblUsuarios.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPremios
            // 
            this.lblPremios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPremios.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold);
            this.lblPremios.ForeColor = System.Drawing.Color.Gold;
            this.lblPremios.Location = new System.Drawing.Point(361, 0);
            this.lblPremios.Name = "lblPremios";
            this.lblPremios.Size = new System.Drawing.Size(352, 74);
            this.lblPremios.TabIndex = 1;
            this.lblPremios.Text = "+500K\nPremios";
            this.lblPremios.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSoporte
            // 
            this.lblSoporte.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSoporte.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold);
            this.lblSoporte.ForeColor = System.Drawing.Color.Gold;
            this.lblSoporte.Location = new System.Drawing.Point(719, 0);
            this.lblSoporte.Name = "lblSoporte";
            this.lblSoporte.Size = new System.Drawing.Size(353, 74);
            this.lblSoporte.TabIndex = 2;
            this.lblSoporte.Text = "24/7\nSoporte";
            this.lblSoporte.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(17)))), ((int)(((byte)(32)))));
            this.panelFooter.Controls.Add(this.lblFooter);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFooter.Location = new System.Drawing.Point(3, 572);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(1069, 29);
            this.panelFooter.TabIndex = 4;
            // 
            // lblFooter
            // 
            this.lblFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFooter.Font = new System.Drawing.Font("Arial", 10F);
            this.lblFooter.ForeColor = System.Drawing.Color.LightSlateGray;
            this.lblFooter.Location = new System.Drawing.Point(0, 0);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(1075, 29);
            this.lblFooter.TabIndex = 0;
            this.lblFooter.Text = "© 2026 Casino Royal - Todos los derechos reservados";
            this.lblFooter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(14)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(1081, 670);
            this.Controls.Add(this.mainLayout);
            this.MinimumSize = new System.Drawing.Size(800, 670);
            this.Name = "MainForm";
            this.Text = "Casino Virtual";
            this.mainLayout.ResumeLayout(false);
            this.panelNavbar.ResumeLayout(false);
            this.panelNavbar.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.pnlContenido.ResumeLayout(false);
            this.layoutInicio.ResumeLayout(false);
            this.panelHero.ResumeLayout(false);
            this.panelHero.PerformLayout();
            this.tlpJuegos.ResumeLayout(false);
            this.panelMinas.ResumeLayout(false);
            this.panelMinas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelRuleta.ResumeLayout(false);
            this.panelRuleta.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panelSlot.ResumeLayout(false);
            this.panelSlot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tlpStats.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Panel pnlContenido;
        private System.Windows.Forms.TableLayoutPanel layoutInicio;

        private System.Windows.Forms.Panel panelHero;
        private System.Windows.Forms.Label lblDeposito;
        private System.Windows.Forms.TextBox txtMontoDeposito;
        private System.Windows.Forms.Button btnDepositar;
        private System.Windows.Forms.Button btnHistorial;
        private System.Windows.Forms.Label lblBienvenido;
        private System.Windows.Forms.Label lblDescripcion;

        private System.Windows.Forms.TableLayoutPanel tlpJuegos;
        private System.Windows.Forms.Panel panelMinas;
        private System.Windows.Forms.Label lblMinas;
        private System.Windows.Forms.Label lblDescripcionMinas;
        private System.Windows.Forms.Button btnMinas;
        private System.Windows.Forms.Panel panelRuleta;
        private System.Windows.Forms.Label lblRuleta;
        private System.Windows.Forms.Label lblDescripcionRuleta;
        private System.Windows.Forms.Button btnRuleta;
        private System.Windows.Forms.Panel panelSlot;
        private System.Windows.Forms.Label lblSlot;
        private System.Windows.Forms.Label lblDescripcionSlot;
        private System.Windows.Forms.Button btnSlot;

        private System.Windows.Forms.TableLayoutPanel tlpStats;
        private System.Windows.Forms.Label lblUsuarios;
        private System.Windows.Forms.Label lblPremios;
        private System.Windows.Forms.Label lblSoporte;

        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Label lblFooter;
        private System.Windows.Forms.Panel panelNavbar;
        private System.Windows.Forms.Button btnCerrarSesion;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem inicioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem juegosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem promocionesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem soporteToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}
