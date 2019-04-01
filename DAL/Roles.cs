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
    public class Roles : LogManager.ISubject, LogManager.IObserver
    {
        public int Id { get; set; }
        public string usuario { get; set; }

        public bool usu_alta { get; set; }
        public bool usu_baja { get; set; }
        public bool usu_modificacion { get; set; }

        public bool Activo { get; set; }

        public LogManager.Mensaje UltimoMensaje { get; set; }

        public Roles()
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
            this.usuario = null;

            this.usu_alta = false;
            this.usu_baja = false;
            this.usu_modificacion = false;

            this.Activo = false;
            UltimoMensaje = null;
        }

        /// <summary>
        /// Carga en la instacia actual los atributos del Id pasado por parámetro
        /// </summary>
        /// <param name="id">ID a buscar</param>
        public void obtener(int id)
        {
            limpiaDatos();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT * " +
                "FROM usuarios " +
                "WHERE usu_id = @usu_id "
                , conexion);

            comando.Parameters.AddWithValue("@usu_id", id);
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
        private void cargarDatos(Roles objeto, MySqlDataReader dr)
        {
            int id = 0;
            int.TryParse(dr["usu_id"].ToString(), out id);
            objeto.Id = id;
          
            bool usu_alta = false;
            bool.TryParse(dr["usu_alta"].ToString(), out usu_alta);
            objeto.usu_alta = usu_alta;

            bool usu_baja = false;
            bool.TryParse(dr["usu_baja"].ToString(), out usu_baja);
            objeto.usu_baja = usu_baja;

            bool usu_modificacion = false;
            bool.TryParse(dr["usu_modificacion"].ToString(), out usu_modificacion);
            objeto.usu_modificacion = usu_modificacion;

            bool act = false;
            bool.TryParse(dr["usu_activo"].ToString(), out act);
            objeto.Activo = act;

            

        }

        /// <summary>
        /// Carga en la instacia actual los atributos del código pasado por parámetro
        /// </summary>
        /// <param name="codigo">Codigo a buscar</param>
        public void Existe(string codigo)
        {
            limpiaDatos();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT * " +
                "FROM usuarios " +
                "WHERE usu_usuario = @usu_usuario "
                , conexion);

            comando.Parameters.AddWithValue("@usu_usuario", codigo);
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

        public void obtener(string codigo)
        {
            limpiaDatos();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT *, " +
                "FROM usuarios " +
                "WHERE usu_id = @usu_id "
                , conexion);

            comando.Parameters.AddWithValue("@usu_id", codigo);
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


            comando.CommandText = "delete from usuarios " +
                                   "where usu_id= @usu_id";
            comando.Parameters.AddWithValue("@usu_id", Id);

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



            /*if (Id == 0)
                comando.CommandText = "INSERT INTO roles (rol_usuario, rol_baja_doc, rol_alta_doc, rol_modif_doc,"+
                                    " rol_vista_doc, rol_alta_usu, rol_baja_usu, rol_modif_usu,"+
                                    " rol_vista_usu) values (@rol_usuario, @rol_baja_doc,@rol_alta_doc," +
                                    "@rol_modif_doc,@rol_vista_doc, @rol_alta_usu,@rol_baja_usu,@rol_modif_usu,@rol_vista_usu); " +
                                     " SELECT  Last_insert_id()";
            else
            { */
                comando.CommandText = "UPDATE usuarios SET usu_alta = @usu_alta, usu_baja = @usu_baja, usu_modificacion = @usu_modificacion" +
                                     " where usu_id= @usu_id; SELECT @usu_id";
                comando.Parameters.AddWithValue("@usu_id", Id);
            //}

                comando.Parameters.AddWithValue("@usu_alta", usu_alta);
                comando.Parameters.AddWithValue("@usu_baja", usu_baja);
                comando.Parameters.AddWithValue("@usu_modificacion", usu_modificacion);
            
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

        public bool ObtenerAcceso(string codigo, string acceso)
        {

            limpiaDatos();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT * " +
                "FROM roles " +
                "WHERE rol_usuario = @rol_usuario "
                , conexion);

            comando.Parameters.AddWithValue("@rol_usuario", codigo);
            comando.Parameters.AddWithValue("@rol_acceso", acceso);

            
            
            comando.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader dr = comando.ExecuteReader();

                if (dr.Read())
                {
                    string est = dr[acceso].ToString();

                    if (est == "1")
                    {

                        return true;

                    }
                    else 
                    {
                        return false;
                    }
                
                }
                return true;
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

                return false;
            }
            

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
        public List<Roles> obtenerFiltrado(ItemFiltro[] itemFiltro,
            ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros)
        {
            List<Roles> ret = new List<Roles>();
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

            comando.CommandText = "SELECT count(*) FROM roles " + where;
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

                cadenaOrden = Varios.armarCadenaOrden(orden, cadenaOrden, "rol_id");

                //TODO: Hacer Paginacion

                double rowcount = fin - (inicio - 1);

                comando.CommandText = "  SELECT *   FROM roles " + where + " "
                                       + cadenaOrden
                                       + " LIMIT " + (inicio - 1) + ", " + rowcount;





                MySqlDataReader dr = comando.ExecuteReader();


                while (dr.Read())
                {
                    Roles bar = new Roles();
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
        public void habdeshabilitar(bool habilitar)
        {

            MySqlConnection conexion = Database.obtenerConexion(true);

            UltimoMensaje = null;

            MySqlCommand comando = new MySqlCommand();

            if (habilitar)
            {
                if (Activo == false)
                {
                    comando.CommandText = "UPDATE roles set rol_activo= 'true'  " +
                                           "where rol_id= @rol_id";
                    comando.Parameters.AddWithValue("@rol_id", Id);
                }
            }
            else
            {
                if (Activo == true)
                {
                    comando.CommandText = "UPDATE roles set rol_activo= 'false'  " +
                                           "where rol_id= @rol_id";
                    comando.Parameters.AddWithValue("@rol_id", Id);
                }
            }

            comando.Connection = conexion;
            comando.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                comando.ExecuteScalar();

            }
            catch (Exception ex)
            {

                UltimoMensaje = GestionErrores.obtenerError(ex);
                UltimoMensaje.cargar(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(),
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(),
                    new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());

                UltimoMensaje.EsError = true;

                string parametros = "";
                foreach (System.Reflection.ParameterInfo item in System.Reflection.MethodBase.GetCurrentMethod().GetParameters())
                {
                    parametros += item.ParameterType.ToString() + ",";
                }

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
    }
}

