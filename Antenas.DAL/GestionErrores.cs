using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antenas.DAL
{
    public class GestionErrores
    {


        public static SGECA.LogManager.Mensaje obtenerError(Exception excepcion)
        {
            SGECA.LogManager.Mensaje retorno = new SGECA.LogManager.Mensaje();

            if (excepcion is System.Data.SqlClient.SqlException)
                obtenerSqlException((System.Data.SqlClient.SqlException)excepcion, retorno);

            else if (excepcion is Exception)
                obtenerException(excepcion, retorno);

            return retorno;
        }


        private static void obtenerException(Exception excepcion, SGECA.LogManager.Mensaje mensaje)
        {
            mensaje.TextoMensaje = excepcion.GetType().ToString() + " - " + excepcion.Message;
            mensaje.TipoMensaje = SGECA.LogManager.EMensaje.Critico;

            if (excepcion is ArgumentException)
            {
                mensaje.TextoMensaje = "Uno de los argumentos proporcionados no es correcto.";
                mensaje.TipoMensaje = SGECA.LogManager.EMensaje.Critico;
            }

            if (excepcion is InvalidCastException)
            {
                mensaje.TextoMensaje = "Conversión de tipos invalida.";
                mensaje.TipoMensaje = SGECA.LogManager.EMensaje.Critico;
            }

            if (excepcion is NullReferenceException)
            {
                mensaje.TextoMensaje = "El objeto especificado no puede ser nulo.";
                mensaje.TipoMensaje = SGECA.LogManager.EMensaje.Critico;
            }

            if (excepcion is ArgumentNullException)
            {
                mensaje.TextoMensaje = "El argumento especificado no puede ser nulo.";
                mensaje.TipoMensaje = SGECA.LogManager.EMensaje.Critico;
            }

            if (excepcion is ArgumentOutOfRangeException)
            {
                mensaje.TextoMensaje = "El argumento especificado está fuera de rango.";
                mensaje.TipoMensaje = SGECA.LogManager.EMensaje.Critico;
            }

            if (excepcion is IndexOutOfRangeException)
            {
                mensaje.TextoMensaje = "El indice especificado está fuera de rango.";
                mensaje.TipoMensaje = SGECA.LogManager.EMensaje.Critico;
            }

            mensaje.TextoMensajeAmpliado = excepcion.Message;
            mensaje.TextoMensajeDepuracion = excepcion.StackTrace;
        }

        private static void obtenerSqlException(System.Data.SqlClient.SqlException excepcion, SGECA.LogManager.Mensaje mensaje)
        {



            if (excepcion.Number == 547 &&
                excepcion.State == 0 &&
                excepcion.Class == 16)
            {
                mensaje.TextoMensaje = "El registro que intenta eliminar está relacionado a otro registro y no se puede quitar.";
                mensaje.TipoMensaje = SGECA.LogManager.EMensaje.Advertencia;
            }
            else if (excepcion.Number == 2601 &&
                excepcion.State == 1 &&
                excepcion.Class == 14)
            {
                mensaje.TextoMensaje = "El Código que intenta almacenar ya se encuentra asignado en la base de datos por lo que no puede ser duplicado.";
                mensaje.TipoMensaje = SGECA.LogManager.EMensaje.Advertencia;
            }
            else if (excepcion.Number == 2627 &&
                excepcion.State == 1 &&
                excepcion.Class == 14)
            {
                mensaje.TextoMensaje = "El Código que intenta almacenar ya se encuentra asignado en la base de datos por lo que no puede ser duplicado.";
                mensaje.TipoMensaje = SGECA.LogManager.EMensaje.Advertencia;
            }
            else if (excepcion.Number == 207 &&
                    excepcion.State == 1 &&
                    excepcion.Class == 16)
            {
                mensaje.TextoMensaje = "La tabla donde se intento acceder no tiene la estructura correcta, por favor notifique al área de sistemas.";
                mensaje.TipoMensaje = SGECA.LogManager.EMensaje.Critico;
            }
            else if (excepcion.Number == 2 &&
                     excepcion.State == 0 &&
                     excepcion.Class == 20)
            {
                mensaje.TextoMensaje = "No se pudo abrir una conexión con SQL Server.";
                mensaje.TipoMensaje = SGECA.LogManager.EMensaje.Critico;
            }
            else
            {
                mensaje.TextoMensaje = "Error de SQL Server";
            }

            mensaje.TextoMensajeAmpliado = excepcion.Message;
            mensaje.TextoMensajeDepuracion = "Number = " + excepcion.Number + "\r\n" +
                "State = " + excepcion.State + "\r\n" +
                "Class = " + excepcion.Class + "\r\n" +
                excepcion.StackTrace;


        }
    }
}
