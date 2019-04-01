using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace  SGECA.LogManager
{
    public interface IMensaje
    {
        DateTime TimeStamp { get; set; }
        string Origen { get; set; }
        string Metodo { get; set; }
        int Linea { get; set; }
        string TextoMensaje { get; set; }
        string TextoMensajeAmpliado { get; set; }
        string TextoMensajeDepuracion { get; set; }
        string StackTrace { get; set; }
        bool EsError { get; set; }
        EMensaje TipoMensaje { get; set; }
        string ToString(bool formatoXML);
        MessageBoxIcon icono { get; set; }
        bool Logueado { get; set; }
        bool Mostrado { get; set; }
        void cargar(string origen, string metodo, int linea, string mensaje,
                string mensajeAmpliado, string mensajeDepuracion, bool esError,
                EMensaje tipoMensaje);

        void cargar(string origen, string metodo, int linea);

        void cargar(string mensaje, string mensajeAmpliado, string mensajeDepuracion,
                    bool esError, EMensaje tipoMensaje);
    }
}
