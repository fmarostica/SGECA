using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SGECA.DAL
{
    public class TipoComprobante : LogManager.ISubject, LogManager.IObserver, ITipoComprobante
    {
        #region declaración de Propiedades
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Letra { get; set; }
        public string Nemonico { get; set; }
        public bool EsConCAE { get; set; }
        public LogManager.Mensaje UltimoMensaje { get; set; }
        //todo:terminar campos faltantes
        #endregion

        #region declaracion de Variables

        //las variables dbCampoId, dbCampoNombre y dbTabla deberían estar (casi) siempre cargados para
        //facilitar la reutilización de código ya que la mayoria de las tablas tienen ID y nombre.
        //los otros campos los ponemos directamente en el código y no como variable para no complicar 
        //la programación
        private static string dbCampoCodigo = "tco_codigo";
        private static string dbCampoDescripcion = "tco_descripcion";
        private static string dbCampoLetra = "tco_letra";
        private static string dbCampoNemonico = "tco_nemonico";
        private static string dbTabla = "TipoComprobante";
        //todo:terminar campos faltantes
        #endregion


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

        public TipoComprobante()
        {
            //suscripción al Log
            Subscribe(new LogManager.Log());
        }

        /// <summary>
        /// Limpio los datos de los atributos de la instancia
        /// </summary>
        private void limpiaDatos()
        {
            this.Codigo = null;
            this.Descripcion = null;
            this.Letra = null;
            this.Nemonico = null;
            EsConCAE = false;
            UltimoMensaje = null;

            //todo:terminar campos faltantes
        }

        /// <summary>
        /// Carga en la instacia actual los atributos del Id pasado por parámetro
        /// </summary>
        public static List<ITipoComprobante> obtener()
        {
            //limpiaDatos();
            List<ITipoComprobante> retorno = new List<ITipoComprobante>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT * " +
                "FROM " + dbTabla +
                " WHERE  tco_activo = 1"
                , conexion);

            comando.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader dr = comando.ExecuteReader();

                while (dr.Read())
                {
                    TipoComprobante p = new TipoComprobante();
                    cargarDatos(p, dr);
                    retorno.Add(p);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                retorno = null;
                LogManager.Mensaje UltimoMensaje = GestionErrores.obtenerError(ex);
                UltimoMensaje.cargar(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(),
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(),
                    new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());
                UltimoMensaje.EsError = true;
                UltimoMensaje.StackTrace = ex.StackTrace;
                LogManager.Log.log(UltimoMensaje);
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
        /// Carga en la instacia actual los atributos del Id pasado por parámetro
        /// </summary>
        /// <param name="id">ID a buscar</param>
        public void obtener(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Carga los datos del datareader en los atributos de una instancia
        /// </summary>
        /// <param name="objeto">instancia donde se cargaran los datos</param>
        /// <param name="dr">DataReader con los datos a cargar</param>
        private static void cargarDatos(TipoComprobante objeto, MySqlDataReader dr)
        {

            objeto.Codigo = dr[dbCampoCodigo].ToString();

            objeto.Descripcion = dr[dbCampoDescripcion].ToString();
            objeto.Letra = dr[dbCampoLetra].ToString();
            objeto.Nemonico = dr[dbCampoNemonico].ToString();
            bool btmp = false;
            bool.TryParse(dr["tco_EsConCAE"].ToString().Replace("1", "true").Replace("0", "false"), out btmp);
            objeto.EsConCAE = btmp;
            //todo:terminar campos faltantes


        }

        /// <summary>
        /// Carga en la instacia actual los atributos del código pasado por parámetro
        /// </summary>
        /// <param name="nombre">Codigo a buscar</param>
        public void obtener(string nombre)
        {
            limpiaDatos();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT * " +
                "FROM " + dbTabla + " " +
                "WHERE  " + dbCampoCodigo + " = @" + dbCampoCodigo
                , conexion);

            comando.Parameters.AddWithValue(dbCampoCodigo, nombre);
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
        /// Elimina de la BD los datos correspondientes al ID de la instancia actual
        /// </summary>
        public void eliminar()
        {

            UltimoMensaje = null;
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();


            comando.CommandText = "delete from  " + dbTabla + " " +
                                  "where " + dbCampoCodigo + " = @" + dbCampoCodigo;
            comando.Parameters.AddWithValue(dbCampoCodigo, Codigo);

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
            //UltimoMensaje = null;
            //DAL.Database db = new Database();
            //MySqlConnection conexion = Database.obtenerConexion(true);

            //MySqlCommand comando = new MySqlCommand();



            //if (Id == 0)
            //    comando.CommandText = "INSERT INTO " + dbTabla + " (" + dbCampoNombre + " ) values  " +
            //                           "(@ " + dbCampoNombre + "); " +
            //                           " SELECT  Last_insert_id()";
            //else
            //{
            //    comando.CommandText = "UPDATE " + dbTabla + " set " + dbCampoNombre + "= @" + dbCampoNombre + " ,  " +
            //                           dbCampoId + " = @" + dbCampoId + " " +
            //                           "where " + dbCampoId + "= @" + dbCampoId + "; SELECT @" + dbCampoId;
            //    comando.Parameters.AddWithValue(dbCampoId, Id);
            //}

            //comando.Parameters.AddWithValue(dbCampoNombre, Nombre);
            //comando.Parameters.AddWithValue(dbCampoId, Id);
            //comando.Transaction = Database.obtenerTransaccion();
            //comando.Connection = conexion;

            //try
            //{
            //    if (Database.obtenerTransaccion() == null)
            //        conexion.Open();
            //    var valor = comando.ExecuteScalar();

            //    if (valor == null && Id != 0)
            //        throw new Exception("El registro que está intentando modificar ya no existe en la base de datos");
            //    else
            //    {

            //        Id = int.Parse(valor.ToString());

            //        retorno = Id;
            //    }
            //}
            //catch (Exception ex)
            //{

            //    UltimoMensaje = GestionErrores.obtenerError(ex);
            //    UltimoMensaje.cargar(
            //        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(),
            //        System.Reflection.MethodBase.GetCurrentMethod().ToString(),
            //        new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());
            //    UltimoMensaje.StackTrace = ex.StackTrace;
            //    UltimoMensaje.EsError = true;
            //    Notify(UltimoMensaje);
            //}
            //finally
            //{
            //    comando.Parameters.Clear();
            //    if (Database.obtenerTransaccion() == null)
            //        if (conexion.State != ConnectionState.Closed)
            //            conexion.Close();
            //}

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
        public List<TipoComprobante> obtenerFiltrado(ItemFiltro[] itemFiltro,
            ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros)
        {
            List<TipoComprobante> ret = new List<TipoComprobante>();
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

            comando.CommandText = "SELECT count(*) FROM " + dbTabla + " " + where;
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

                cadenaOrden = Varios.armarCadenaOrden(orden, cadenaOrden, dbCampoDescripcion);

                //TODO: Hacer Paginacion

                double rowcount = fin - (inicio - 1);

                comando.CommandText = "  SELECT *   FROM " + dbTabla + " " + where + " "
                                       + cadenaOrden
                                       + " LIMIT " + (inicio - 1) + ", " + rowcount;





                MySqlDataReader dr = comando.ExecuteReader();


                while (dr.Read())
                {
                    TipoComprobante bar = new TipoComprobante();
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



        public string ToString(bool conId)
        {
            return Codigo + "- " + Descripcion;
        }


    }
}

