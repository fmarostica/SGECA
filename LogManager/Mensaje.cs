using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SGECA.LogManager
{
    [Serializable]
    public class Mensaje : IMensaje
    {

        private EMensaje eMensaje;

        /// <summary>
        /// Fecha y Hora exacta de grabado.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Namespace donde se origina la transacción.
        /// </summary>
        public string Origen { get; set; }

        /// <summary>
        /// Nombre del método
        /// </summary>
        public string Metodo { get; set; }

        /// <summary>
        /// Número de línea.
        /// </summary>
        public int Linea { get; set; }

        /// <summary>
        /// Información del mensaje visible al usuario.
        /// </summary>
        public string TextoMensaje { get; set; }

        /// <summary>
        /// Detalle del mensaje.
        /// </summary>
        public string TextoMensajeAmpliado { get; set; }

        /// <summary>
        /// Información técnica del mensaje.
        /// </summary>
        public string TextoMensajeDepuracion { get; set; }

        /// <summary>
        /// Traza de la pila
        /// </summary>
        public string StackTrace { get; set; }

        public bool EsError { get; set; }

        /// <summary>
        /// Informativo, Critico ó Advertencia.
        /// </summary>
        public EMensaje TipoMensaje { get; set; }

        public MessageBoxIcon icono { get; set; }

        public bool Logueado { get; set; }

        public bool Mostrado { get; set; }

        public Mensaje()
        {
            TimeStamp = DateTime.Now;
        }

        public Mensaje(string origen, string metodo, int linea, string mensaje,
                        string mensajeAmpliado, string mensajeDepuracion, bool esError,
                        EMensaje tipoMensaje, string stackTrace)
        {
            TimeStamp = DateTime.Now;
            Origen = origen;
            Metodo = metodo;
            Linea = linea;
            TextoMensaje = mensaje;
            TextoMensajeAmpliado = mensajeAmpliado;
            TextoMensajeDepuracion = mensajeDepuracion;
            EsError = esError;
            TipoMensaje = tipoMensaje;
            StackTrace = stackTrace;
        }



        public Mensaje(string origen, string metodo, int linea, string mensaje,
                bool esError, EMensaje tipoMensaje)
        {
            TimeStamp = DateTime.Now;
            Origen = origen;
            Metodo = metodo;
            Linea = linea;
            TextoMensaje = mensaje;

            EsError = esError;
            TipoMensaje = tipoMensaje;
        }

        public Mensaje(string mensaje, bool esError, EMensaje tipoMensaje)
        {
            TimeStamp = DateTime.Now;
            TextoMensaje = mensaje;
            EsError = esError;
            TipoMensaje = tipoMensaje;
        }




        public void cargar(string origen, string metodo, int linea, string mensaje,
                string mensajeAmpliado, string mensajeDepuracion, bool esError,
                EMensaje tipoMensaje)
        {
            if (origen != "")
                Origen = origen;
            if (metodo != "")
                Metodo = metodo;
            if (linea != 0)
                Linea = linea;
            if (mensaje != "")
                TextoMensaje = mensaje;
            if (mensajeAmpliado != "")
                TextoMensajeAmpliado = mensajeAmpliado;
            if (mensajeDepuracion != "")
                TextoMensajeDepuracion = mensajeDepuracion;
            EsError = esError;
            TipoMensaje = tipoMensaje;
        }

        public void cargar(string mensaje, string mensajeAmpliado, string mensajeDepuracion,
                    bool esError, EMensaje tipoMensaje)
        {

            if (mensaje != "")
                TextoMensaje = mensaje;
            if (mensajeAmpliado != "")
                TextoMensajeAmpliado = mensajeAmpliado;
            if (mensajeDepuracion != "")
                TextoMensajeDepuracion = mensajeDepuracion;
            EsError = esError;
            TipoMensaje = tipoMensaje;
        }

        public override string ToString()
        {
            string cadena = "";
            foreach (var item in this.GetType().GetProperties())
            {
                if (item.GetValue(this, null) != null)
                {
                    string valor = item.GetValue(this, null).ToString();
                    if (typeof(DateTime).IsAssignableFrom(item.PropertyType))
                        valor = ((DateTime)item.GetValue(this, null)).ToString("yyyy-MM-dd HH:mm:ss");

                    cadena += item.Name + " = " + valor + "\r\n";
                }
            }

            cadena += "------------------------------------------ \r\n";

            return cadena;
        }
        /// <summary>
        /// Método encargado de armar la cadena para almacenar la transacción en el log
        /// </summary>
        public string ToString(bool formatoXML)
        {
            string cadena = "<mensaje>" + "\r\n";
            foreach (var item in this.GetType().GetProperties())
            {
                if (item.GetValue(this, null) != null)
                {
                    string valor = item.GetValue(this, null).ToString();
                    if (typeof(DateTime).IsAssignableFrom(item.PropertyType))
                        valor = ((DateTime)item.GetValue(this, null)).ToString("yyyy-MM-dd HH:mm:ss");

                    cadena += "\t<" + item.Name + ">" + valor + "</" + item.Name + ">" + "\r\n";
                }
            }
            cadena += "</mensaje>" + "\r\n";
            return cadena;
        }


        public void cargar(string origen, string metodo, int linea)
        {
            if (origen != "")
                Origen = origen;
            if (metodo != "")
                Metodo = metodo;
            if (linea != 0)
                Linea = linea;
        }
    }
}
