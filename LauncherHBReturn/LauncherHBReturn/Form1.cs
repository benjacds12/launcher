using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LauncherHBReturn
{
    public partial class Form1 : Form
    {
        // ---- Configuracion del servidor ----
        const string URL_MANIFEST = "http://38.180.186.144:2848/manifest.json";
        const string EJECUTABLE   = "Helbreath Return BETA.exe";
        const string ARCHIVO_ZIP  = "update.zip";
        const string ARCHIVO_VER  = ".launcher_ver";   // archivo oculto con version local

        // Para arrastrar la ventana sin borde
        private Point _dragOffset;

        public Form1()
        {
            InitializeComponent();
        }

        // -----------------------------------------------------------
        // Inicio
        // -----------------------------------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            btnJugar.Enabled = false;
            ColorearBarra(barraProgreso, Color.FromArgb(184, 134, 11)); // dorado

            // Arrastrar ventana borderless desde cualquier zona
            SubscribirDrag(this);
            SubscribirDrag(panelBottom);
            SubscribirDrag(lblTitulo);

            Task.Run(() => ComprobarActualizacion());
        }

        // -----------------------------------------------------------
        // Logica principal de actualizacion
        // -----------------------------------------------------------
        private void ComprobarActualizacion()
        {
            try
            {
                SetStatus("Conectando al servidor...");
                Manifest m;
                using (var wc = new WebClient())
                {
                    string json = wc.DownloadString(URL_MANIFEST);
                    m = ParseManifest(json);
                }

                SetVersion("v" + m.Version);
                int local = LeerVersionLocal();

                if (local < m.Version)
                {
                    SetStatus("Nueva version disponible (v" + m.Version + "). Descargando...");
                    Descargar(m.ClientUrl, m.Version);
                }
                else
                {
                    SetStatus("Todo al dia. Bienvenido!");
                    SetProgress(100);
                    HabilitarJugar(true);
                }
            }
            catch (Exception ex)
            {
                SetStatus("Error al conectar: " + ex.Message);
                // Si ya hay una version local instalada, permite jugar igual
                if (LeerVersionLocal() > 0)
                {
                    SetStatus("Sin conexion. Usando version local.");
                    HabilitarJugar(true);
                }
            }
        }

        private void Descargar(string url, int nuevaVersion)
        {
            try
            {
                using (var wc = new WebClient())
                {
                    wc.DownloadProgressChanged += (s, e) =>
                    {
                        SetProgress(e.ProgressPercentage);
                        long mb    = e.BytesReceived / 1024 / 1024;
                        long total = e.TotalBytesToReceive > 0 ? e.TotalBytesToReceive / 1024 / 1024 : 0;
                        SetStatus("Descargando... " + mb + " MB / " + total + " MB  (" + e.ProgressPercentage + "%)");
                    };

                    wc.DownloadFileCompleted += (s, e) =>
                    {
                        if (e.Error != null)
                        {
                            SetStatus("Error al descargar: " + e.Error.Message);
                            return;
                        }
                        Instalar(nuevaVersion);
                    };

                    wc.DownloadFileAsync(new Uri(url), ARCHIVO_ZIP);
                }
            }
            catch (Exception ex)
            {
                SetStatus("Error: " + ex.Message);
            }
        }

        private void Instalar(int nuevaVersion)
        {
            try
            {
                SetStatus("Instalando archivos...");
                SetProgress(0);

                using (ZipArchive zip = ZipFile.OpenRead(ARCHIVO_ZIP))
                {
                    int total = zip.Entries.Count;
                    int i = 0;
                    foreach (ZipArchiveEntry entrada in zip.Entries)
                    {
                        string destino = Path.Combine(Application.StartupPath, entrada.FullName);
                        if (string.IsNullOrEmpty(entrada.Name))
                        {
                            Directory.CreateDirectory(destino);
                        }
                        else
                        {
                            string dir = Path.GetDirectoryName(destino);
                            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                            if (File.Exists(destino)) File.Delete(destino);
                            entrada.ExtractToFile(destino);
                        }
                        i++;
                        SetProgress(i * 100 / total);
                    }
                }

                if (File.Exists(ARCHIVO_ZIP)) File.Delete(ARCHIVO_ZIP);
                GuardarVersionLocal(nuevaVersion);
                SetVersion("v" + nuevaVersion);
                SetStatus("Actualizacion instalada correctamente!");
                SetProgress(100);
                HabilitarJugar(true);
            }
            catch (Exception ex)
            {
                SetStatus("Error al instalar: " + ex.Message);
            }
        }

        // -----------------------------------------------------------
        // Version local  (archivo oculto .launcher_ver)
        // -----------------------------------------------------------
        private int LeerVersionLocal()
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, ARCHIVO_VER);
                if (File.Exists(path) && int.TryParse(File.ReadAllText(path).Trim(), out int v))
                    return v;
            }
            catch { }
            return 0;
        }

        private void GuardarVersionLocal(int version)
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, ARCHIVO_VER);
                File.WriteAllText(path, version.ToString());
                File.SetAttributes(path, FileAttributes.Hidden);
            }
            catch { }
        }

        // -----------------------------------------------------------
        // Parseo de manifest.json sin dependencias externas
        // -----------------------------------------------------------
        private Manifest ParseManifest(string json)
        {
            var m = new Manifest();
            var verMatch = Regex.Match(json, "\"version\"\\s*:\\s*(\\d+)");
            var urlMatch = Regex.Match(json, "\"clientUrl\"\\s*:\\s*\"([^\"]+)\"");
            if (verMatch.Success) m.Version   = int.Parse(verMatch.Groups[1].Value);
            if (urlMatch.Success) m.ClientUrl = urlMatch.Groups[1].Value;
            return m;
        }

        // -----------------------------------------------------------
        // Helpers thread-safe para actualizar controles de UI
        // -----------------------------------------------------------
        private void SetStatus(string texto)
        {
            if (lblEstado.InvokeRequired)
                lblEstado.Invoke(new Action(() => lblEstado.Text = texto));
            else
                lblEstado.Text = texto;
        }

        private void SetProgress(int valor)
        {
            if (barraProgreso.InvokeRequired)
                barraProgreso.Invoke(new Action(() => barraProgreso.Value = Math.Max(0, Math.Min(valor, 100))));
            else
                barraProgreso.Value = Math.Max(0, Math.Min(valor, 100));
        }

        private void SetVersion(string texto)
        {
            if (lblVersion.InvokeRequired)
                lblVersion.Invoke(new Action(() => lblVersion.Text = texto));
            else
                lblVersion.Text = texto;
        }

        private void HabilitarJugar(bool habilitar)
        {
            if (btnJugar.InvokeRequired)
                btnJugar.Invoke(new Action(() => btnJugar.Enabled = habilitar));
            else
                btnJugar.Enabled = habilitar;
        }

        // -----------------------------------------------------------
        // Color personalizado en la ProgressBar (via WinAPI)
        // -----------------------------------------------------------
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        private void ColorearBarra(ProgressBar pb, Color color)
        {
            SendMessage(pb.Handle, 0x0409 /*PBM_SETBARCOLOR*/, IntPtr.Zero,
                        (IntPtr)System.Drawing.ColorTranslator.ToWin32(color));
            SendMessage(pb.Handle, 0x2001 /*PBM_SETBKCOLOR*/, IntPtr.Zero,
                        (IntPtr)System.Drawing.ColorTranslator.ToWin32(Color.FromArgb(35, 35, 50)));
        }

        // -----------------------------------------------------------
        // Drag ventana sin borde
        // -----------------------------------------------------------
        private void SubscribirDrag(Control c)
        {
            c.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                    _dragOffset = new Point(-e.X, -e.Y);
            };
            c.MouseMove += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    Point p = Control.MousePosition;
                    p.Offset(_dragOffset);
                    Location = p;
                }
            };
        }

        // -----------------------------------------------------------
        // Eventos de botones
        // -----------------------------------------------------------
        private void btnJugar_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Application.StartupPath, EJECUTABLE);
            if (File.Exists(path))
            {
                Process.Start(path, "BenjaminSecretKey");
                Application.Exit();
            }
            else
            {
                MessageBox.Show("No se encuentra el ejecutable del juego.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
    }

    class Manifest
    {
        public int    Version   { get; set; }
        public string ClientUrl { get; set; }
    }
}
