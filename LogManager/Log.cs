using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Windows.Forms;

namespace SGECA.LogManager
{
    [Serializable]
    public class Log : IObserver
    {
        private static string rutaLog = "";
        private static int habilitaLog = -1;
        private static object locked = new object();
        private static string nivelLog = "";
        private static string diasLog = "";

        private static string RutaLog
        {
            get
            {
                rutaLog = obtenerString("RutaLog", rutaLog);
                if (!verificaDirectorio(rutaLog))
                {
                    rutaLog = null;
                }
                return rutaLog;
            }

        }

        private static int NivelLog
        {
            get
            {

                int nl = 2;
                if (int.TryParse(obtenerString("NivelLog", nivelLog), out nl))
                    nivelLog = nl.ToString();
                return nl;
            }
        }

        private static int DiasLog
        {
            get
            {

                int nl = 0;
                if (int.TryParse(obtenerString("DiasLog", diasLog), out nl))
                    diasLog = nl.ToString();
                return nl;
            }
        }

        private static string obtenerString(string clave, string variable)
        {
            if (variable != "")
                return variable;

            string sValor = null;

            try
            {
                sValor = ConfigurationManager.AppSettings[clave].ToString().ToUpper();
            }
            catch (Exception ex)
            {
                if (habilitaLog == 1)
                    LogManager.Log.log(new LogManager.Mensaje("LogManager.Log",
                           "obtenerString(string, string)", 0,
                           "Problema al obtener los datos",
                           "", ex.Message,
                           true, LogManager.EMensaje.Critico, ex.StackTrace));

                System.Windows.Forms.MessageBox.Show("No se encontro la clave \"" + clave + "\" en el archivo de configuración de la aplicación", "Atención", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            variable = sValor;
            return sValor;
        }

        private static bool verificaDirectorio(string Path)
        {
            try
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Path);

                if (!di.Exists)
                {
                    di.Create();
                }

                return true;

            }
            catch
            {
                return false;
            }

        }

        public static void log(IMensaje mensaje)
        {

            try
            {
                if (HabilitaLog && !mensaje.Logueado)
                {


                    mensaje.Logueado = true;

                    switch (mensaje.TipoMensaje)
                    {
                        case EMensaje.Debug:
                            mensaje.icono = MessageBoxIcon.None;
                            if (NivelLog > 0)
                                return;
                            break;
                        case EMensaje.Informativo:
                            mensaje.icono = MessageBoxIcon.Information;
                            if (NivelLog > 1)
                                return;
                            break;
                        case EMensaje.Critico:
                            mensaje.icono = MessageBoxIcon.Error;
                            copiarMensajeAClipboard(mensaje);
                            break;
                        case EMensaje.Advertencia:
                            mensaje.icono = MessageBoxIcon.Exclamation;
                            if (NivelLog > 2)
                                return;

                            break;
                        default:
                            break;
                    }

                    borrarLogsViejos();


                    string nombreArchivo = RutaLog + "\\";
                    if (mensaje.Origen != "")
                        nombreArchivo += DateTime.Now.ToString("yyyyMMdd") + "_" + mensaje.Origen;

                    nombreArchivo += "_log.xml";

                    nombreArchivo = nombreArchivo.Replace("\\\\", "\\");


                    if (!File.Exists(nombreArchivo))
                    {
                        XmlTextWriter writer = new XmlTextWriter(nombreArchivo, null);
                        writer.WriteStartElement("Mensajes");
                        writer.WriteEndElement();
                        writer.Close();
                    }

                    // Load existing clients and add new 
                    XElement xml = XElement.Load(nombreArchivo);
                    xml.Add(new XElement("Mensaje",
                    new XAttribute("TimeStamp", mensaje.TimeStamp),
                    new XElement("TipoMensaje", mensaje.TipoMensaje),
                    new XElement("EsError", mensaje.EsError),
                    new XElement("Origen", mensaje.Origen),
                    new XElement("Metodo", mensaje.Metodo),
                    new XElement("Linea", mensaje.Linea),
                    new XElement("TextoMensaje", mensaje.TextoMensaje),
                    new XElement("TextoMensajeAmpliado", mensaje.TextoMensajeAmpliado),
                    new XElement("TextoMensajeDepuracion", mensaje.TextoMensajeDepuracion),
                    new XElement("StackTrace", mensaje.StackTrace)


                    ));
                    //mensaje.ToString(true)
                  
                    xml.Save(nombreArchivo);


                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private static void borrarLogsViejos()
        {
            try
            {
                DirectoryInfo oDirectorio = new DirectoryInfo(RutaLog); ;
                FileInfo[] oArchivos = oDirectorio.GetFiles("*.xml");

                foreach (FileInfo item in oArchivos)
                {
                    DateTime fechaLog = DateTime.Now;

                    if (DateTime.TryParseExact(item.Name.Substring(0, 8),
                                               "yyyyMMdd", null, System.Globalization.DateTimeStyles.None,
                                                out fechaLog))
                    {
                        try
                        {
                            TimeSpan ts = DateTime.Now - fechaLog;
                            if (ts.Days > DiasLog)
                                item.Delete();
                        }
                        catch { }
                    }

                }
            }
            catch { }

        }

        private static void copiarMensajeAClipboard(IMensaje mensaje)
        {
            try
            {
                string msj = "Mensaje: " + Environment.NewLine +
                    "TimeStamp: " + mensaje.TimeStamp + Environment.NewLine +
                    "TipoMensaje: " + mensaje.TipoMensaje + Environment.NewLine +
                    "EsError: " + mensaje.EsError + Environment.NewLine +
                    "Origen: " + mensaje.Origen + Environment.NewLine +
                    "Metodo: " + mensaje.Metodo + Environment.NewLine +
                    "Linea: " + mensaje.Linea + Environment.NewLine +
                    "TextoMensaje: " + mensaje.TextoMensaje + Environment.NewLine +
                    "TextoMensajeAmpliado: " + mensaje.TextoMensajeAmpliado + Environment.NewLine +
                    "TextoMensajeDepuracion: " + mensaje.TextoMensajeDepuracion + Environment.NewLine +
                    "StackTrace: " + mensaje.StackTrace;


                Clipboard.SetText(msj);
            }
            catch { }
        }

        private static bool HabilitaLog
        {
            get
            {
                bool fHabilitaLog = false;
                try
                {
                    if (habilitaLog != -1)
                        if (habilitaLog == 0)
                            return false;
                        else
                            return true;


                    string sHabilitaLog = "";

                    sHabilitaLog = obtenerString("HabilitaLog", sHabilitaLog);

                    fHabilitaLog = (sHabilitaLog.ToLower() == "si" || sHabilitaLog == "1") ? true : false;

                    if (fHabilitaLog)
                        habilitaLog = 1;
                    else
                        habilitaLog = 0;
                }
                catch (Exception ex)
                {
                    habilitaLog = 0;
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }

                return fHabilitaLog;

            }

        }


        public static void mostrar(IMensaje mensaje, string titulo)
        {
            if (!mensaje.Mostrado)
            {
                if (mensaje.TipoMensaje == LogManager.EMensaje.Advertencia ||
                    mensaje.TipoMensaje == LogManager.EMensaje.Critico)
                    MessageBox.Show(mensaje.TextoMensaje + "\n\n" +
                        "Origen: " + mensaje.Origen + "[" + mensaje.Metodo + "]\n\n" +
                        "Los datos del error fueron copiados al portapapeles."
                        , "Atención:" + titulo,
                        MessageBoxButtons.OK,
                        mensaje.icono);
                mensaje.Mostrado = true;
            }
        }

        #region Observer Pattern
        private List<object> Observers = new List<object>();

        /// <summary>
        /// Método encargado de recibir notificaciones del subscriptor donde  ha sucedido un evento que 
        /// requiere su atención.
        /// </summary>
        public void UpdateState(IMensaje mensaje)
        {
            Log.log(mensaje);
        }

        /// <summary>
        /// Método encargado de notificar al subscriptor que ha sucedido un evento que 
        /// requiere su atención.
        /// </summary>
        public void Notify(IMensaje mensaje)
        {
            // Recorremos cada uno de los observadores para notificarles el evento.
            foreach (IObserver observer in this.Observers)
            {
                // Indicamos a cada uno de los subscriptores la actualización del 
                // estado (evento) producido.
                observer.UpdateState(mensaje);
            }
        } // Notify

        /// <summary>
        /// Método encargado de agregar un observador para que el subscriptor le 
        /// pueda notificar al subscriptor el evento.
        /// </summary>
        /// <param name="observer">Interfaz IObserver que indica el observador.</param>
        public void Subscribe(IObserver observer)
        {
            if (!this.Observers.Contains(observer))
                // Agregamos el subscriptor a la lista de subscriptores del publicador.
                this.Observers.Add(observer);
        } // Subscribe


        /// <summary>
        /// Método encargado de eliminar un observador para que el subscriptor no le 
        /// notifique ningún evento más al que era su subscriptor.
        /// </summary>
        /// <param name="observer">Interfaz IObserver que indica el observador.</param>
        public void Unsubscribe(IObserver observer)
        {
            // Eliminamos el subscriptor de la lista de subscriptores del publicador.
            this.Observers.Remove(observer);
        } // Unsubscribe

        #endregion
    }
}
