using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Win32; // Para tocar el Registro de Windows

namespace LauncherHBReturn
{
    public partial class Form1 : Form
    {
        // --- Configuración del Servidor ---
        string urlVersion = "http://38.180.186.144:2848/version.txt";
        string urlZip = "http://38.180.186.144:2848/update.zip";
        string urlHashes = "http://38.180.186.144:2848/hashes.txt"; // Acá lee la seguridad
        string archivoZipLocal = "update.zip";
        string archivoVersionLocal = "version.txt";
        string ejecutableJuego = "Helbreath Return BETA.exe";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnJugar.Enabled = false;
            ComprobarActualizacion();
        }

        private void ComprobarActualizacion()
        {
            try
            {
                // 1. Intentar leer del Registro
                object regValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\LauncherHBReturn", "Version", null);
                int versionLocal = 0;

                if (regValue != null)
                {
                    versionLocal = (int)regValue;
                }
                else if (File.Exists(archivoVersionLocal))
                {
                    // 2. Si no hay registro, intentar leer del archivo físico
                    int.TryParse(File.ReadAllText(archivoVersionLocal), out versionLocal);
                }

                using (WebClient cliente = new WebClient())
                {
                    string versionServerTexto = cliente.DownloadString(urlVersion).Trim();
                    int versionServer = int.Parse(versionServerTexto);

                    if (versionLocal < versionServer)
                    {
                        lblEstado.Text = "Actualizando a la versión " + versionServer + "...";
                        cliente.DownloadFileCompleted += (s, ev) => { FinalizarActualizacion(); };
                        cliente.DownloadFileAsync(new Uri(urlZip), archivoZipLocal);
                    }
                    else
                    {
                        ValidarIntegridad();
                    }
                }
            }
            catch { ValidarIntegridad(); }
        }

        private void ValidarIntegridad()
        {
            try
            {
                lblEstado.Text = "Verificando seguridad remota...";
                Dictionary<string, string> hashesRemotos = new Dictionary<string, string>();

                using (WebClient cliente = new WebClient())
                {
                    // Descargamos la lista de códigos del VPS
                    string contenido = cliente.DownloadString(urlHashes);
                    string[] lineas = contenido.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string linea in lineas)
                    {
                        string[] partes = linea.Split(':');
                        if (partes.Length == 2) hashesRemotos.Add(partes[0].Trim(), partes[1].Trim());
                    }
                }

                bool todoOk = true;
                foreach (var item in hashesRemotos)
                {
                    // Comparamos la carpeta local con el código del VPS
                    if (ChecksumCarpeta(item.Key) != item.Value)
                    {
                        lblEstado.Text = "Error en carpeta: " + item.Key + ". Reparando...";
                        todoOk = false;
                        break;
                    }
                }

                if (!todoOk)
                {
                    btnJugar.Enabled = false;
                    MessageBox.Show("Archivos modificados detectados. Se requiere reinstalar el cliente.");
                    // FinalizarActualizacion(); // Descomentá esto si querés que descargue el ZIP automáticamente al detectar cheat
                }
                else
                {
                    lblEstado.Text = "Archivos verificados. ¡Bienvenido!";
                    btnJugar.Enabled = true;
                }
            }
            catch (Exception)
            {
                lblEstado.Text = "Error al verificar seguridad.";
                btnJugar.Enabled = false;
            }
        }

        private void FinalizarActualizacion()
        {
            try
            {
                lblEstado.Text = "Instalando archivos...";
                using (ZipArchive archivo = ZipFile.OpenRead(archivoZipLocal))
                {
                    foreach (ZipArchiveEntry entrada in archivo.Entries)
                    {
                        string rutaDestino = Path.Combine(Application.StartupPath, entrada.FullName);
                        if (string.IsNullOrEmpty(entrada.Name)) { Directory.CreateDirectory(rutaDestino); continue; }
                        if (File.Exists(rutaDestino)) File.Delete(rutaDestino);
                        entrada.ExtractToFile(rutaDestino);
                    }
                }

                if (File.Exists(archivoZipLocal)) File.Delete(archivoZipLocal);

                // Guardamos la nueva versión tanto en el Registro como en un archivo local
                using (WebClient cliente = new WebClient())
                {
                    string versionServerTexto = cliente.DownloadString(urlVersion).Trim();
                    int versionServer = int.Parse(versionServerTexto);

                    // 1. Guardar en el Registro de Windows (lo que ya tenías)
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\LauncherHBReturn", "Version", versionServer);

                    // 2. CREAR EL ARCHIVO FISICO (Para que aparezca en la carpeta)
                    File.WriteAllText(archivoVersionLocal, versionServer.ToString());

                    // 3. OPCIONAL: Hacer que el archivo sea OCULTO para protegerlo
                    File.SetAttributes(archivoVersionLocal, FileAttributes.Hidden);
                }

                lblEstado.Text = "¡Actualización terminada!";
                ValidarIntegridad();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al instalar: " + ex.Message);
            }
        }

        private string ChecksumCarpeta(string ruta)
        {
            if (!Directory.Exists(ruta)) return "0";
            var archivos = Directory.GetFiles(ruta, "*.*", SearchOption.AllDirectories).OrderBy(p => p).ToList();
            using (var md5 = MD5.Create())
            {
                foreach (var archivo in archivos)
                {
                    byte[] contentBytes = File.ReadAllBytes(archivo);
                    md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
                }
                md5.TransformFinalBlock(new byte[0], 0, 0);
                return BitConverter.ToString(md5.Hash).Replace("-", "").ToLower();
            }
        }

        private void btnJugar_Click(object sender, EventArgs e)
        {
            if (File.Exists(ejecutableJuego))
            {
                Process.Start(ejecutableJuego, "BenjaminSecretKey");
                Application.Exit();
            }
            else { MessageBox.Show("No se encuentra el ejecutable."); }
        }

        private void btnJugar_Click_1(object sender, EventArgs e) { btnJugar_Click(sender, e); }
        private void lblEstado_Click(object sender, EventArgs e) { }
    }
}