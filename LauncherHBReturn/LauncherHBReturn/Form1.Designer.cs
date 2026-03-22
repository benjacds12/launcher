namespace LauncherHBReturn
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Codigo generado por el Disenador de Windows Forms

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(Form1));

            this.panelBottom  = new System.Windows.Forms.Panel();
            this.btnJugar     = new System.Windows.Forms.Button();
            this.barraProgreso= new System.Windows.Forms.ProgressBar();
            this.lblEstado    = new System.Windows.Forms.Label();
            this.lblVersion   = new System.Windows.Forms.Label();
            this.btnCerrar    = new System.Windows.Forms.Button();
            this.btnMinimizar = new System.Windows.Forms.Button();
            this.lblTitulo    = new System.Windows.Forms.Label();

            this.panelBottom.SuspendLayout();
            this.SuspendLayout();

            // --------------------------------------------------
            // panelBottom  —  barra inferior oscura
            // --------------------------------------------------
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(18, 18, 28);
            this.panelBottom.Dock      = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Height    = 90;
            this.panelBottom.Name      = "panelBottom";
            this.panelBottom.Controls.Add(this.barraProgreso);
            this.panelBottom.Controls.Add(this.lblEstado);
            this.panelBottom.Controls.Add(this.lblVersion);
            this.panelBottom.Controls.Add(this.btnJugar);

            // --------------------------------------------------
            // barraProgreso  —  barra de descarga
            // --------------------------------------------------
            this.barraProgreso.Location  = new System.Drawing.Point(20, 14);
            this.barraProgreso.Size      = new System.Drawing.Size(646, 10);
            this.barraProgreso.Maximum   = 100;
            this.barraProgreso.Name      = "barraProgreso";
            this.barraProgreso.TabIndex  = 3;

            // --------------------------------------------------
            // lblEstado
            // --------------------------------------------------
            this.lblEstado.Location  = new System.Drawing.Point(20, 30);
            this.lblEstado.Size      = new System.Drawing.Size(540, 20);
            this.lblEstado.ForeColor = System.Drawing.Color.FromArgb(180, 180, 200);
            this.lblEstado.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.lblEstado.BackColor = System.Drawing.Color.Transparent;
            this.lblEstado.Text      = "Conectando...";
            this.lblEstado.Name      = "lblEstado";
            this.lblEstado.TabIndex  = 4;

            // --------------------------------------------------
            // lblVersion
            // --------------------------------------------------
            this.lblVersion.Location  = new System.Drawing.Point(20, 55);
            this.lblVersion.Size      = new System.Drawing.Size(200, 18);
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(90, 90, 110);
            this.lblVersion.Font      = new System.Drawing.Font("Segoe UI", 8F);
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Text      = "v-";
            this.lblVersion.Name      = "lblVersion";
            this.lblVersion.TabIndex  = 5;

            // --------------------------------------------------
            // btnJugar  —  boton dorado grande
            // --------------------------------------------------
            this.btnJugar.Location                   = new System.Drawing.Point(676, 20);
            this.btnJugar.Size                        = new System.Drawing.Size(114, 50);
            this.btnJugar.BackColor                   = System.Drawing.Color.FromArgb(184, 134, 11);
            this.btnJugar.ForeColor                   = System.Drawing.Color.White;
            this.btnJugar.FlatStyle                   = System.Windows.Forms.FlatStyle.Flat;
            this.btnJugar.FlatAppearance.BorderSize   = 0;
            this.btnJugar.Font                        = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnJugar.Text                        = "JUGAR";
            this.btnJugar.Cursor                      = System.Windows.Forms.Cursors.Hand;
            this.btnJugar.Name                        = "btnJugar";
            this.btnJugar.TabIndex                    = 0;
            this.btnJugar.UseVisualStyleBackColor      = false;
            this.btnJugar.Click += new System.EventHandler(this.btnJugar_Click);

            // --------------------------------------------------
            // btnCerrar  —  X arriba a la derecha
            // --------------------------------------------------
            this.btnCerrar.Location                  = new System.Drawing.Point(762, 8);
            this.btnCerrar.Size                       = new System.Drawing.Size(30, 28);
            this.btnCerrar.Text                       = "✕";
            this.btnCerrar.FlatStyle                  = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.FlatAppearance.BorderSize  = 0;
            this.btnCerrar.BackColor                  = System.Drawing.Color.Transparent;
            this.btnCerrar.ForeColor                  = System.Drawing.Color.White;
            this.btnCerrar.Font                       = new System.Drawing.Font("Segoe UI", 11F);
            this.btnCerrar.Cursor                     = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.Name                       = "btnCerrar";
            this.btnCerrar.TabIndex                   = 1;
            this.btnCerrar.UseVisualStyleBackColor     = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);

            // --------------------------------------------------
            // btnMinimizar  —  _ arriba a la derecha
            // --------------------------------------------------
            this.btnMinimizar.Location                  = new System.Drawing.Point(730, 8);
            this.btnMinimizar.Size                       = new System.Drawing.Size(30, 28);
            this.btnMinimizar.Text                       = "─";
            this.btnMinimizar.FlatStyle                  = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizar.FlatAppearance.BorderSize  = 0;
            this.btnMinimizar.BackColor                  = System.Drawing.Color.Transparent;
            this.btnMinimizar.ForeColor                  = System.Drawing.Color.White;
            this.btnMinimizar.Font                       = new System.Drawing.Font("Segoe UI", 11F);
            this.btnMinimizar.Cursor                     = System.Windows.Forms.Cursors.Hand;
            this.btnMinimizar.Name                       = "btnMinimizar";
            this.btnMinimizar.TabIndex                   = 2;
            this.btnMinimizar.UseVisualStyleBackColor     = false;
            this.btnMinimizar.Click += new System.EventHandler(this.btnMinimizar_Click);

            // --------------------------------------------------
            // lblTitulo  —  nombre del juego arriba a la izquierda
            // --------------------------------------------------
            this.lblTitulo.Location  = new System.Drawing.Point(16, 12);
            this.lblTitulo.Size      = new System.Drawing.Size(400, 28);
            this.lblTitulo.Text      = "HELBREATH RETURN";
            this.lblTitulo.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblTitulo.Font      = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.lblTitulo.Name      = "lblTitulo";
            this.lblTitulo.TabIndex  = 6;

            // --------------------------------------------------
            // Form1
            // --------------------------------------------------
            this.AutoScaleDimensions  = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode        = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage      = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout= System.Windows.Forms.ImageLayout.Zoom;
            this.BackColor            = System.Drawing.Color.FromArgb(10, 10, 18);
            this.ClientSize           = new System.Drawing.Size(800, 460);
            this.FormBorderStyle      = System.Windows.Forms.FormBorderStyle.None;
            this.Name                 = "Form1";
            this.Text                 = "Helbreath Return BETA - Launcher";

            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.btnMinimizar);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.panelBottom);

            this.Load += new System.EventHandler(this.Form1_Load);

            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel   panelBottom;
        private System.Windows.Forms.Button  btnJugar;
        private System.Windows.Forms.ProgressBar barraProgreso;
        private System.Windows.Forms.Label   lblEstado;
        private System.Windows.Forms.Label   lblVersion;
        private System.Windows.Forms.Button  btnCerrar;
        private System.Windows.Forms.Button  btnMinimizar;
        private System.Windows.Forms.Label   lblTitulo;
    }
}
