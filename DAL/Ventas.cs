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
    public class Ventas : LogManager.ISubject, LogManager.IObserver
    {
        public int Id { get; set; }
        public string Empresa { get; set; }
        public string correo { get; set; }
        public bool Activo { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public string observaciones { get; set; }
        public int cli_id { get; set; }

        public LogManager.Mensaje UltimoMensaje { get; set; }

        public Ventas()
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
            this.Empresa = null;
            this.correo = null;
            this.Activo = false;
            this.direccion = null;
            this.telefono = null;
            this.observaciones = null;
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
                "FROM clientes " +
                "WHERE cli_id = @cli_id "
                , conexion);

            comando.Parameters.AddWithValue("@cli_id", id);
            comando.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader dr = comando.ExecuteReader();

                if (dr.Read())
                {
                    
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

        public void obtenerComprobante()
        {
            //limpiaDatos();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT * " +
                "FROM comprobanteencabezado "
                , conexion);

            
            comando.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader dr = comando.ExecuteReader();

                if (dr.Read())
                {
                    
                    

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
        private void cargarDatos(Cliente objeto, MySqlDataReader dr)
        {
            //int id = 0;
            //int.TryParse(dr["cli_id"].ToString(), out id);
            //objeto.Id = id;

            //objeto.Cli_RazonSocial = dr["cli_cliente"].ToString();
            //objeto.correo = dr["cli_correo"].ToString();
            //objeto.telefono = dr["cli_telefono"].ToString();
            //objeto.direccion = dr["cli_direccion"].ToString();
            //objeto.observaciones = dr["cli_observaciones"].ToString();

            //bool act = false;
            //bool.TryParse(dr["cli_activo"].ToString(), out act);
            //objeto.Activo = act;





        }

        /// <summary>
        /// Carga en la instacia actual los atributos del código pasado por parámetro
        /// </summary>
        /// <param name="codigo">Codigo a buscar</param>
        public void obtener(string codigo)
        {
            limpiaDatos();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT * " +
                "FROM clientes " +
                "WHERE  cli_cliente = @cli_cliente "
                , conexion);

            comando.Parameters.AddWithValue("@cli_cliente", codigo);
            comando.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader dr = comando.ExecuteReader();

                if (dr.Read())
                {
                    
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


            comando.CommandText = "delete from clientes " +
                                   "where cli_id= @cli_id";
            comando.Parameters.AddWithValue("@cli_id", Id);

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
            {
                comando.CommandText = "INSERT INTO clientes (cli_cliente,cli_telefono,cli_direccion,cli_correo,cli_activo," +
                                       "cli_observaciones) values  " +
                                       "(@cli_cliente,@cli_telefono,@cli_direccion, @cli_correo,@cli_activo,@cli_observaciones); " +
                                       " SELECT  Last_insert_id()";


            }
            else
            {
                comando.CommandText = "UPDATE clientes SET cli_correo=@cli_correo,cli_telefono = @cli_telefono," +
                                        " cli_direccion = @cli_direccion, cli_observaciones = @cli_observaciones " +
                                       "where cli_id= @cli_id; SELECT @cli_id";
                comando.Parameters.AddWithValue("@cli_id", Id);
            }

            comando.Parameters.AddWithValue("@cli_cliente", Empresa);
            comando.Parameters.AddWithValue("@cli_correo", correo);
            comando.Parameters.AddWithValue("@cli_activo", Activo);
            comando.Parameters.AddWithValue("@cli_direccion", direccion);
            comando.Parameters.AddWithValue("@cli_telefono", telefono);
            comando.Parameters.AddWithValue("@cli_observaciones", observaciones);
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
        public List<Cliente> obtenerFiltrado(ItemFiltro[] itemFiltro,
            ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros)
        {
            List<Cliente> ret = new List<Cliente>();
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

            comando.CommandText = "SELECT count(*) FROM clientes " + where;
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

                cadenaOrden = Varios.armarCadenaOrden(orden, cadenaOrden, "cli_cliente");

                //TODO: Hacer Paginacion

                double rowcount = fin - (inicio - 1);

                comando.CommandText = "  SELECT *   FROM clientes " + where + " "
                                       + cadenaOrden
                                       + " LIMIT " + (inicio - 1) + ", " + rowcount;





                MySqlDataReader dr = comando.ExecuteReader();


                while (dr.Read())
                {
                    Cliente bar = new Cliente();
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
                    comando.CommandText = "UPDATE clientes set cli_activo= 'true'  " +
                                           "where cli_id= @cli_id";
                    comando.Parameters.AddWithValue("@cli_id", Id);
                }
            }
            else
            {
                if (Activo == true)
                {
                    comando.CommandText = "UPDATE clientes set cli_activo= 'false'  " +
                                           "where cli_id= @cli_id";
                    comando.Parameters.AddWithValue("@cli_id", Id);
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




        public List<string> clientesComboBox()
        {
            List<string> cliente = new List<string>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand("SELECT * FROM clientes ", conexion);

            comando.Transaction = Database.obtenerTransaccion();
            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader dr = comando.ExecuteReader();


                while (dr.Read())
                {
                    cliente.Add(dr["cli_cliente"].ToString());
                }


                return cliente;



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
                return cliente;

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

