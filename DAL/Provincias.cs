using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SGECA.DAL
{
    public class Provincias: LogManager.IObserver, LogManager.ISubject
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public string pais_id { get; set; }

        public Provincias()
        {

        }

        public LogManager.Mensaje UltimoMensaje { get; set; }

        #region Observer Pattern
        private List<object> Observers = new List<object>();

        /// <summary>
        /// Método encargado de recibir notificaciones del subscriptor donde  ha sucedido un evento que 
        /// requiere su atención.
        /// </summary>
        public void UpdateState(LogManager.IMensaje mensaje)
        {
            Notify(mensaje);
        }

        /// <summary>
        /// Método encargado de notificar al subscriptor que ha sucedido un evento que 
        /// requiere su atención.
        /// </summary>
        public void Notify(LogManager.IMensaje mensaje)
        {
            // Recorremos cada uno de los observadores para notificarles el evento.
            foreach (LogManager.IObserver observer in this.Observers)
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
        public void Subscribe(LogManager.IObserver observer)
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
        public void Unsubscribe(LogManager.IObserver observer)
        {
            // Eliminamos el subscriptor de la lista de subscriptores del publicador.
            this.Observers.Remove(observer);
        } // Unsubscribe

        #endregion

        public List<Provincias> Obtener(int id = 0)
        {
            string where = "";

            if (id != 0) where = " where id=@id";

            List<Provincias> ret = new List<Provincias>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from provincia " + where + " order by pro_Nombre";
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Provincias bar = new Provincias();
                    bar.Subscribe(this);

                    bar.id = reader["pro_Id"].ToString();
                    bar.nombre = reader["pro_Nombre"].ToString();
                    bar.pais_id = reader["pai_Id"].ToString();
                    ret.Add(bar);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                UltimoMensaje = GestionErrores.obtenerError(ex);
                UltimoMensaje.cargar(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(),
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(),
                    new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());
                UltimoMensaje.EsError = true;
                Notify(UltimoMensaje);
            }
            finally
            {
                cmd.Parameters.Clear();
                if (Database.obtenerTransaccion() == null)
                    if (conexion.State != ConnectionState.Closed)
                        conexion.Close();
            }

            return ret;
        }

        public List<Provincias> Obtener_por_pais(string pais_id)
        {
            string where = "";

            if (pais_id != "") where = " where pai_Id=@id";

            List<Provincias> ret = new List<Provincias>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from provincia " + where + " order by pro_Nombre";
            cmd.Parameters.AddWithValue("@id", pais_id);

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Provincias bar = new Provincias();
                    bar.Subscribe(this);

                    bar.id = reader["pro_Id"].ToString();
                    bar.nombre = reader["pro_Nombre"].ToString().ToUpper();
                    bar.pais_id = reader["pai_Id"].ToString();
                    ret.Add(bar);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                UltimoMensaje = GestionErrores.obtenerError(ex);
                UltimoMensaje.cargar(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(),
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(),
                    new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());
                UltimoMensaje.EsError = true;
                Notify(UltimoMensaje);
            }
            finally
            {
                cmd.Parameters.Clear();
                if (Database.obtenerTransaccion() == null)
                    if (conexion.State != ConnectionState.Closed)
                        conexion.Close();
            }

            return ret;
        }

    }
}
