using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;
using MySql.Data;


namespace SGECA.DAL
{
    public class Parametros : LogManager.ISubject, LogManager.IObserver
    {
        public int Id { get; set; }
        public string nombre_pc { get; set; }
        private static string ruta_origen;
        public string ruta_compartida { get; set; }
        public string usuario { get; set; }


        public LogManager.Mensaje UltimoMensaje { get; set; }

        public Parametros()
        {
            //suscripción al Log
            Subscribe(new LogManager.Log());
        }

        /// <summary>
        /// Limpio los datos de los atributos de la instancia
        /// </summary>
        private void limpiaDatos()
        {
            this.Id = 0;
            UltimoMensaje = null;
        }

        public static string obtenerRutaOrigen()
        {
            if (ruta_origen == null)
            {
                MySqlConnection conexion = Database.obtenerConexion(true);

                MySqlCommand comando = new MySqlCommand(
                    "SELECT * " +
                    "FROM parametros " +
                    "WHERE par_pc = @par_pc"
                    , conexion);



                comando.Parameters.AddWithValue("@par_pc", Environment.MachineName);
                comando.Transaction = Database.obtenerTransaccion();

                try
                {
                    if (Database.obtenerTransaccion() == null)
                        conexion.Open();
                    MySqlDataReader dr = comando.ExecuteReader();

                    if (dr.Read())
                    {
                        ruta_origen = dr["par_rutaCompartida"].ToString();
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {

                    LogManager.Mensaje UltimoMensaje = GestionErrores.obtenerError(ex);
                    UltimoMensaje.cargar(
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(),
                        System.Reflection.MethodBase.GetCurrentMethod().ToString(),
                        new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());
                    UltimoMensaje.EsError = true;
                    UltimoMensaje.StackTrace = ex.StackTrace;
                    UltimoMensaje.TipoMensaje = LogManager.EMensaje.Critico;

                    LogManager.Log.log(UltimoMensaje);

                }
                finally
                {
                    comando.Parameters.Clear();
                    if (Database.obtenerTransaccion() == null)
                        if (conexion.State != ConnectionState.Closed)
                            conexion.Close();
                }
            }
            return ruta_origen;


        }

        /// <summary>
        /// Carga en la instacia actual los atributos del Id pasado por parámetro
        /// </summary>
        /// <param name="id">ID a buscar</param>
        public void obtener()
        {
            limpiaDatos();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT * " +
                "FROM parametros " +
                "WHERE par_pc = @par_pc"
                , conexion);

            nombre_pc = Environment.MachineName;

            comando.Parameters.AddWithValue("@par_pc", nombre_pc);
            comando.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader dr = comando.ExecuteReader();

                if (dr.Read())
                {
                    cargarDatos(this, dr);
                }
                dr.Close();
            }
            catch (Exception ex)
            {

                UltimoMensaje = GestionErrores.obtenerError(ex);
                UltimoMensaje.cargar(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(),
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(),
                    new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());
                UltimoMensaje.EsError = true;
                UltimoMensaje.StackTrace = ex.StackTrace;
                Notify(UltimoMensaje);
            }
            finally
            {
                comando.Parameters.Clear();
                if (Database.obtenerTransaccion() == null)
                    if (conexion.State != ConnectionState.Closed)
                        conexion.Close();
            }
        }

        /// <summary>
        /// Carga los datos del datareader en los atributos de una instancia
        /// </summary>
        /// <param name="objeto">instancia donde se cargaran los datos</param>
        /// <param name="dr">DataReader con los datos a cargar</param>
        public void cargarDatos(Parametros objeto, MySqlDataReader dr)
        {
            int id = 0;
            int.TryParse(dr["par_id"].ToString(), out id);
            objeto.Id = id;

            objeto.nombre_pc = dr["par_pc"].ToString();

            objeto.ruta_compartida = dr["par_rutaCompartida"].ToString();
        }



        /// <summary>
        /// Carga en la instacia actual los atributos del código pasado por parámetro
        /// </summary>
        /// <param name="codigo">Codigo a buscar</param>


        /// <summary>
        /// Elimina de la BD los datos correspondientes al ID de la instancia actual
        /// </summary>
        public void eliminar()
        {

            UltimoMensaje = null;
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();


            comando.CommandText = "delete from parametros " +
                                   "where id= @id";
            comando.Parameters.AddWithValue("@id", Id);

            comando.Transaction = Database.obtenerTransaccion();
            comando.Connection = conexion;

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                comando.ExecuteNonQuery();
                limpiaDatos();
            }
            catch (Exception ex)
            {

                UltimoMensaje = GestionErrores.obtenerError(ex);
                UltimoMensaje.cargar(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(),
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(),
                    new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());
                UltimoMensaje.EsError = true;
                UltimoMensaje.StackTrace = ex.StackTrace;
                Notify(UltimoMensaje);
            }
            finally
            {
                comando.Parameters.Clear();
                if (Database.obtenerTransaccion() == null)
                    if (conexion.State != ConnectionState.Closed)
                        conexion.Close();
            }
        }

        /// <summary>
        /// Método encargado de conectarse con la BD para insertar o actualizar.
        /// </summary>
        public int guardar()
        {
            int retorno = 0;
            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();


            if (Id == 0)
                comando.CommandText = "INSERT INTO parametros (par_pc,par_rutaCompartida) values  " +
                                       "(@par_pc,@par_rutaCompartida); " +
                                       " SELECT  Last_insert_id()";
            else
            {
                comando.CommandText = "UPDATE parametros SET par_rutaCompartida=@par_rutaCompartida " +
                                       "where par_id= @id; SELECT @id";

                comando.Parameters.AddWithValue("@id", Id);
            }


            comando.Parameters.AddWithValue("@par_pc", nombre_pc);
            comando.Parameters.AddWithValue("@par_rutaCompartida", ruta_compartida);

            comando.Transaction = Database.obtenerTransaccion();
            comando.Connection = conexion;

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                var valor = comando.ExecuteScalar();

                if (valor == null && Id != 0)
                    throw new Exception("El registro que está intentando modificar ya no existe en la base de datos");
                else
                {

                    Id = int.Parse(valor.ToString());

                    retorno = Id;
                }
            }
            catch (Exception ex)
            {

                UltimoMensaje = GestionErrores.obtenerError(ex);
                UltimoMensaje.cargar(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(),
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(),
                    new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());
                UltimoMensaje.StackTrace = ex.StackTrace;
                UltimoMensaje.EsError = true;
                Notify(UltimoMensaje);
            }
            finally
            {
                comando.Parameters.Clear();
                if (Database.obtenerTransaccion() == null)
                    if (conexion.State != ConnectionState.Closed)
                        conexion.Close();
            }

            return retorno;
        }

        /// <summary>
        /// Método encargado de conectarse con la BD para obtener los registros filtrados.
        /// </summary>
        /// <param name="itemFiltro">Items que van a utilizarse para generar la consulta (WHERE)</param>
        /// <param name="orden">Items que van a utilizarse para generar el orden (ORDER BY)</param>
        /// <param name="busquedaAnd">Indica si los terminos se conectan con AND o OR (true = AND)</param>
        /// <param name="inicio">Primer registro a mostrar</param>
        /// <param name="fin">Último registro a mostrar, o -1 para traer todos</param>
        /// <param name="totalRegistros">Total de registros que contiene el total de la consulta</param>
        /// <returns>Lista de los registros involucrados</returns>
        /// 
        /// <summary>
        public List<Parametros> obtenerFiltrado(ItemFiltro[] itemFiltro,
            ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros)
        {
            List<Parametros> ret = new List<Parametros>();
            UltimoMensaje = null;
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();
            comando.Connection = conexion;
            comando.Transaction = Database.obtenerTransaccion();

            totalRegistros = 0;
            int parameterCount = 0;
            string where = "";
            string tipoBusqueda = " AND ";

            if (!busquedaAnd)
                tipoBusqueda = " OR  ";


            Varios.armarConsultaFiltros(itemFiltro, comando, ref parameterCount, ref where, tipoBusqueda);


            string cadenaOrden = "";

            comando.CommandText = "SELECT count(*) FROM parametros " + where;
            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                double.TryParse(comando.ExecuteScalar().ToString(), out totalRegistros);

                if (inicio < 0)
                    inicio = 0;

                if (inicio > totalRegistros)
                    inicio = totalRegistros - 1;


                if (fin > totalRegistros || fin == -1)
                    fin = totalRegistros;

                if (inicio < 1)
                    inicio = 1;

                if (fin < 1)
                    fin = 1;

                cadenaOrden = Varios.armarCadenaOrden(orden, cadenaOrden, "id");

                //TODO: Hacer Paginacion

                double rowcount = fin - (inicio - 1);

                comando.CommandText = "  SELECT *   FROM parametros " + where + " "
                                       + cadenaOrden
                                       + " LIMIT " + (inicio - 1) + ", " + rowcount;





                MySqlDataReader dr = comando.ExecuteReader();


                while (dr.Read())
                {
                    Parametros bar = new Parametros();
                    bar.Subscribe(this);

                    cargarDatos(bar, dr);


                    ret.Add(bar);

                }

                dr.Close();

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
                if (Database.obtenerTransaccion() == null)
                    if (conexion.State != ConnectionState.Closed)
                        conexion.Close();
            }

            return ret;
        }


        /// <summary>
        /// Método encargado de conectarse con la BD para actualizar el estado activo "si"/"no".
        /// </summary>




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




        public List<string> accesoCliente()
        {
            List<string> acceso = new List<string>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand("SELECT * FROM parametros WHERE parametro='acceso' ", conexion);

            comando.Transaction = Database.obtenerTransaccion();
            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader dr = comando.ExecuteReader();


                while (dr.Read())
                {

                    acceso.Add(dr["acceso"].ToString());

                }


                return acceso;



                dr.Close();
            }
            catch (Exception ex)
            {

                UltimoMensaje = GestionErrores.obtenerError(ex);
                UltimoMensaje.cargar(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(),
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(),
                    new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());
                UltimoMensaje.EsError = true;
                UltimoMensaje.StackTrace = ex.StackTrace;
                Notify(UltimoMensaje);
                return acceso;

            }
            finally
            {
                comando.Parameters.Clear();
                if (Database.obtenerTransaccion() == null)
                    if (conexion.State != ConnectionState.Closed)
                        conexion.Close();

            }

        }









    }
}

