using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using System.Globalization;

namespace Antenas.DAL
{
    public class Database : SGECA.LogManager.ISubject, SGECA.LogManager.IObserver
    {
        private static string connectionString = "";

        public SGECA.LogManager.Mensaje UltimoMensaje { get; set; }



        #region Singleton pattern
        private static Database instance;

        public static Database obtenerInstancia()
        {
            if (instance == null)
                instance = new Database();

            return instance;
        }

        private static MySqlConnection connection;

        public static MySqlConnection obtenerConexion()
        {
            if (connection == null)
                connection = new MySqlConnection(ConnectionString);


            return connection;

        }




        private static MySqlTransaction transaccion;

        public static MySqlTransaction obtenerTransaccion()
        {
            return transaccion;
        }

        #endregion

        #region Transacciones
        public static void BeginTransaction(string nombreTransaccion)
        {
            if (connection == null)
                connection = new MySqlConnection(ConnectionString);

            connection.Open();

            transaccion = connection.BeginTransaction(IsolationLevel.RepeatableRead);

        }

        public static void CommitTransaction()
        {


            transaccion.Commit();
            transaccion = null;
            connection.Close();
        }

        public static void RollbackTransaction()
        {
            transaccion.Rollback();
            transaccion = null;
            connection.Close();
        }

        #endregion

        /// <summary>
        /// Obtiene una conexion al servidor de datos
        /// </summary>
        /// <param name="nueva">Indica que si no hay transaccion obtengo una nueva conexion</param>
        /// <returns></returns>
        public static MySqlConnection obtenerConexion(bool nueva)
        {
            if (nueva)
                if (transaccion == null)
                    return new MySqlConnection(ConnectionString);
                else
                    return connection;
            else
                return obtenerConexion();

        }

        public static int CommandTimeout()
        {
            int timeout = 60;

            if (ConnectionString.Contains("timeout="))
            {
                try
                {
                    string[] valores = ConnectionString.Split(';');
                    foreach (string item in valores)
                    {
                        try
                        {
                            string[] par = item.Split('=');
                            if (par[0].ToLower() == "timeout")
                            {
                                timeout = int.Parse(par[1]);
                                break;
                            }
                        }
                        catch { }
                    }
                }
                catch { }
            }

            return timeout;
        }


        public static string ConnectionString
        {
            get
            {
                if (connectionString != "")
                    return connectionString;

                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings["Antenas"].ConnectionString;


                }
                catch { }

                return connectionString;
            }
        }

        public Database()
        {
            //suscripción al Log

        }

        public static MySqlParameter cargaParametro(string nombre, object dato)
        {
            MySqlCommand retorno = new MySqlCommand();
            if (dato == null)
                retorno.Parameters.AddWithValue(nombre, DBNull.Value);
            else
                retorno.Parameters.AddWithValue(nombre, dato);

            MySqlParameter p = new MySqlParameter();

            p.DbType = retorno.Parameters[0].DbType;
            p.ParameterName = retorno.Parameters[0].ParameterName;
            p.Precision = retorno.Parameters[0].Precision;
            p.Scale = retorno.Parameters[0].Scale;
            p.Size = retorno.Parameters[0].Size;
            //p.SqlValue = retorno.Parameters[0].SqlValue;
            //p.TypeName = retorno.Parameters[0].TypeName;
            //p.UdtTypeName = retorno.Parameters[0].UdtTypeName;
            p.Value = retorno.Parameters[0].Value;

           
            return p;


        }

       


        public MySqlCommand Comando(string Texto)
        {
            MySqlConnection Conexion = obtenerConexion();
            MySqlCommand comando = new MySqlCommand(Texto, Conexion);
            return comando;
        }

        public MySqlCommand Comando(string Texto, MySqlConnection Conexion)
        {
            MySqlCommand comando = new MySqlCommand(Texto, Conexion);
            return comando;
        }



        public static Exception chequeoBaseDeDatos()
        {
            string sDato = "";
            Exception exError = new Exception("void");

            MySqlConnection conexion = obtenerConexion(true);
            MySqlCommand comando = new MySqlCommand("select * from parametro", conexion);

            try
            {

                conexion.Open();
                sDato = comando.ExecuteScalar().ToString();
                conexion.Close();
            }
            catch (Exception ex)
            {

                exError = ex;

            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return exError;

        }




        public int ejecutarNonQuery(string consulta, MySqlConnection conexion, bool cierroConexion, params MySqlParameter[] parametros)
        {
            UltimoMensaje = null;
            int retorno = 0;

            MySqlCommand comando = new MySqlCommand(consulta, conexion);

            foreach (MySqlParameter item in parametros)
            {
                comando.Parameters.Add(item);
            }

            if (conexion.State != ConnectionState.Open)
                conexion.Open();


            comando.CommandTimeout = 0;

            try
            {
                retorno = comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                UltimoMensaje = null;
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
                comando.Parameters.Clear();
                if (conexion.State != ConnectionState.Closed && cierroConexion)
                    conexion.Close();
            }

            return retorno;
        }

        public int ejecutarNonQuery(string consulta, params MySqlParameter[] parametros)
        {
            MySqlConnection conexion = obtenerConexion();
            return ejecutarNonQuery(consulta, conexion, true, parametros);


        }

        public object ejecutarEscalar(string consulta, MySqlConnection conexion, bool cierroConexion, params MySqlParameter[] parametros)
        {
            UltimoMensaje = null;
            MySqlCommand comando = new MySqlCommand(consulta, conexion);

            object retorno = new object();
 
            foreach (MySqlParameter item in parametros)
            {
                comando.Parameters.Add(item);
            }

            if (conexion.State != ConnectionState.Open)
                conexion.Open();


            comando.CommandTimeout = 0;


            try
            {
                retorno = comando.ExecuteScalar();

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
                comando.Parameters.Clear();
                if (conexion.State != ConnectionState.Closed && cierroConexion)
                    conexion.Close();
            }

            return retorno;
        }

        public object ejecutarEscalar(string consulta, params MySqlParameter[] parametros)
        {
            MySqlConnection conexion = obtenerConexion();
            return ejecutarEscalar(consulta, conexion, true, parametros);
        }



        #region Observer Pattern
        private List<object> Observers = new List<object>();

        /// <summary>
        /// Método encargado de recibir notificaciones del subscriptor donde  ha sucedido un evento que 
        /// requiere su atención.
        /// </summary>
        public void UpdateState(SGECA.LogManager.IMensaje mensaje)
        {
            Notify(mensaje);
        }

        /// <summary>
        /// Método encargado de notificar al subscriptor que ha sucedido un evento que 
        /// requiere su atención.
        /// </summary>
        public void Notify(SGECA.LogManager.IMensaje mensaje)
        {
            // Recorremos cada uno de los observadores para notificarles el evento.
            foreach (SGECA.LogManager.IObserver observer in this.Observers)
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
        public void Subscribe(SGECA.LogManager.IObserver observer)
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
        public void Unsubscribe(SGECA.LogManager.IObserver observer)
        {
            // Eliminamos el subscriptor de la lista de subscriptores del publicador.
            this.Observers.Remove(observer);
        } // Unsubscribe

        #endregion
    }
}
