using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.IO;
using System.Configuration;


namespace SGECA.DAL
{
    public class Documentos : LogManager.ISubject, LogManager.IObserver
    {
        public int Id { get; set; }
        public string num_ident { get; set; }
        public string cliente { get; set; }
        public string text_ident { get; set; }
        public string descrip_adic { get; set; }
        public string fecha_carga { get; set; }
        public string fecha_ident { get; set; }
        public string fecha_mod { get; set; }
        public string direccion { get; set; }
        public string nombre_pc { get; set; }
        public string ip_pc { set; get; }
        public string usu_usuario { get; set; }
        public string direccion_trans { get; set; }
        public string direccionServidor { get; set; }
        public int usu_id { get; set; }
        private static string cli_id;
        public bool tra_incluirCarpeta { get; set; }

        //Variables SubidorArchivos
        public string tra_origen { get; set; }
        public string tra_destino { get; set; }
        public int tra_id_doc { get; set; }
        public int tra_id { get; set; }
        public int tra_cli_cliente { get; set; }
        public string par_rutaComp { get; set; }
        public bool incluirCarpeta { get; set; }
        
        


        public bool Activo1 { get; set; }
        public bool Activo { get; set; }
        public LogManager.Mensaje UltimoMensaje { get; set; }

        public Documentos()
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
            this.num_ident = null;
            this.cliente = null;
            this.text_ident = null;
            this.descrip_adic = null;
            this.descrip_adic = null;
            this.fecha_carga = null;
            this.fecha_ident = null;
            this.fecha_mod = null;
            this.direccion = null;
            this.nombre_pc = null;
            this.ip_pc = null;
            this.usu_usuario = null;
            this.Activo1 = false;
            this.tra_incluirCarpeta = false;
            this.usu_id = 0;
            this.direccionServidor = null;
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
                "FROM documentos " +
                "WHERE id_documento = @id_documento "
                , conexion);

            comando.Parameters.AddWithValue("@id_documento", id);
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

        public void obtenerDoc()
        {
            this.tra_destino = null;
            this.tra_origen = null;
            this.tra_id_doc = 0;
            string tra_fechaFin = null;
            this.tra_incluirCarpeta = false;

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT * FROM transacciones " +
                "WHERE tra_fechaFin IS NULL "
                , conexion);

            comando.Parameters.AddWithValue("@tra_fechaFin", tra_fechaFin);

            comando.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader dr = comando.ExecuteReader();

                if (dr.Read())
                {
                    tra_destino = dr["tra_destino"].ToString();
                    tra_origen = dr["tra_origen"].ToString();
                    par_rutaComp = dr["par_rutaCompartida"].ToString();

                    int tra_id_d = 0;
                    int.TryParse(dr["id_documento"].ToString(), out tra_id_d);
                    tra_id_doc = tra_id_d;

                    int tra_id_tra = 0;
                    int.TryParse(dr["tra_id"].ToString(), out tra_id_tra);
                    tra_id = tra_id_tra;

                    int tra_id_cli = 0;
                    int.TryParse(dr["cli_id"].ToString(), out tra_id_cli);
                    tra_cli_cliente = tra_id_cli;

                    bool inc = false;
                    bool.TryParse(dr["tra_incluirCarpeta"].ToString(), out inc);
                    incluirCarpeta = inc;
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
        private void cargarDatos(Documentos objeto, MySqlDataReader dr)
        {
            int id = 0;
            int.TryParse(dr["id_documento"].ToString(), out id);
            objeto.Id = id;

            int usu_id = 0;
            int.TryParse(dr["usu_id"].ToString(), out usu_id);
            objeto.usu_id = usu_id;

            objeto.num_ident = dr["num_ident"].ToString();
            objeto.cliente = dr["cli_id"].ToString();
            objeto.text_ident = dr["text_ident"].ToString();
            objeto.descrip_adic = dr["descrip_adic"].ToString();
            objeto.fecha_carga = dr["fecha_carga"].ToString();
            objeto.fecha_ident = dr["fecha_ident"].ToString();
            objeto.fecha_mod = dr["fecha_mod"].ToString();
            objeto.direccion = dr["direccion"].ToString();
            objeto.direccionServidor = dr["direccion_servidor"].ToString();

            bool act = false;
            bool.TryParse(dr["activo"].ToString(), out act);
            objeto.Activo1 = act;

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
                "FROM documento " +
                "WHERE  num_ident = @num_ident "
                , conexion);

            comando.Parameters.AddWithValue("@num_ident", codigo);
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


        public static string obtenerIdCliente(string cli_cliente)
        {
                MySqlConnection conexion = Database.obtenerConexion(true);

                MySqlCommand comando = new MySqlCommand(
                    "SELECT * " +
                    "FROM clientes " +
                    "WHERE cli_cliente = @cli_cliente"
                    , conexion);



                comando.Parameters.AddWithValue("@cli_cliente", cli_cliente );
                comando.Transaction = Database.obtenerTransaccion();

                try
                {
                    if (Database.obtenerTransaccion() == null)
                        conexion.Open();
                    MySqlDataReader dr = comando.ExecuteReader();

                    if (dr.Read())
                    {
                        cli_id = dr["cli_id"].ToString();
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    comando.Parameters.Clear();
                    if (Database.obtenerTransaccion() == null)
                        if (conexion.State != ConnectionState.Closed)
                            conexion.Close();
                }

                return cli_id;


        }


        /// <summary>
        /// Elimina de la BD los datos correspondientes al ID de la instancia actual
        /// </summary>
        public void eliminar()
        {

            UltimoMensaje = null;
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();


            comando.CommandText = "UPDATE documentos SET activo = true WHERE id_documento = @id_documento";
            comando.Parameters.AddWithValue("@id_documento", Id);

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

       

        public void actualizar(int id)
        {

            UltimoMensaje = null;
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();
            
            string toFormat = "yyyy-MM-dd HH':'mm':'ss";
            DateTime fecha = DateTime.Now;
            string fecha_conv = fecha.ToString(toFormat);


            comando.CommandText = "UPDATE transacciones SET tra_fechaFin = @fecha WHERE tra_id = @id";

            comando.Parameters.AddWithValue("@id", id);
            comando.Parameters.AddWithValue("@fecha", fecha_conv);

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


        public void actualizarError(int id)
        {

            UltimoMensaje = null;
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();

            string toFormat = "yyyy-MM-dd HH':'mm':'ss";
            DateTime fecha = DateTime.Now;
            string fecha_conv = fecha.ToString(toFormat);


            comando.CommandText = "UPDATE transacciones SET tra_estado = true,tra_fechaFin='0000-00-00 00:00:00' WHERE tra_id = @id";

            comando.Parameters.AddWithValue("@id", id);
            
            

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
                comando.CommandText = "INSERT INTO documentos (num_ident,cli_id,text_ident,descrip_adic," +
                                       " fecha_carga,fecha_ident,fecha_mod,direccion,activo,usu_id) values  " +
                                       "(@num_ident, @cli_id,@text_ident,@descrip_adic,@fecha_carga, @fecha_ident," +
                                       "@fecha_mod,@direccion,@activo,@usu_id); " +
                                       " SELECT  Last_insert_id()";

                //transaccionDocumentos(direccion);
            }
            else
            {
                //comando.CommandText = "UPDATE documentos SET fecha_mod=@fecha_mod,usu_usuario_modif=@usu_usuario " +
                //                     "where id_documento= @id_documento; SELECT @id_documento";
                //comando.Parameters.AddWithValue("@id_documento", Id);

                //cargaArchivos(direccion);
            }

            comando.Parameters.AddWithValue("@num_ident", num_ident);
            comando.Parameters.AddWithValue("@cli_id", cliente);
            comando.Parameters.AddWithValue("@text_ident", text_ident);
            comando.Parameters.AddWithValue("@descrip_adic", descrip_adic);
            comando.Parameters.AddWithValue("@fecha_carga", fecha_carga);
            comando.Parameters.AddWithValue("@fecha_ident", fecha_ident);
            comando.Parameters.AddWithValue("@fecha_mod", fecha_mod);
            comando.Parameters.AddWithValue("@direccion", direccion);
            comando.Parameters.AddWithValue("@usu_id", usu_id);
            comando.Parameters.AddWithValue("@activo", Activo1);

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



        public void guardarTransaccion(int id_doc)
        {
            int retorno = 0;
            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);

            string rutaComp = DAL.Parametros.obtenerRutaOrigen();

            MySqlCommand comando = new MySqlCommand();

            comando.CommandText = "INSERT INTO transacciones (tra_origen,tra_destino,usu_id,id_documento,cli_id,par_rutaCompartida,tra_incluirCarpeta) " +
                                  " VALUES (@tra_origen,@tra_destino,@usu_id,@id_documento,@cli_od,@par_rutaCompartida,@tra_incluirCarpeta); " +
                                       " SELECT  Last_insert_id()";

            comando.Parameters.AddWithValue("@tra_origen", direccion_trans);
            comando.Parameters.AddWithValue("@id_documento", id_doc);
            comando.Parameters.AddWithValue("@tra_destino", direccionServidor);
            comando.Parameters.AddWithValue("@usu_id", usu_id);
            comando.Parameters.AddWithValue("@cli_od", cliente);
            comando.Parameters.AddWithValue("@par_rutaCompartida", rutaComp);
            comando.Parameters.AddWithValue("@tra_incluirCarpeta", tra_incluirCarpeta);
            

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
        public List<Documentos> obtenerFiltrado(ItemFiltro[] itemFiltro,
            ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros)
        {
            List<Documentos> ret = new List<Documentos>();
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

            comando.CommandText = "SELECT count(*) FROM documentos " + where;
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

                cadenaOrden = Varios.armarCadenaOrden(orden, cadenaOrden, "num_ident");

                //TODO: Hacer Paginacion

                double rowcount = fin - (inicio - 1);

                comando.CommandText = "  SELECT *   FROM documentos " + where + " "
                                       + cadenaOrden
                                       + " LIMIT " + (inicio - 1) + ", " + rowcount;





                MySqlDataReader dr = comando.ExecuteReader();


                while (dr.Read())
                {
                    Documentos bar = new Documentos();
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
                if (Activo1 == false)
                {
                    comando.CommandText = "UPDATE documentos set activo= 'true'  " +
                                           "where id_documento= @id_documento";
                    comando.Parameters.AddWithValue("@id_documento", Id);
                }
            }
            else
            {
                if (Activo1 == true)
                {
                    comando.CommandText = "UPDATE documentos set activo= 'false'  " +
                                           "where id_documentos= @id_documentos";
                    comando.Parameters.AddWithValue("@id_documentos", Id);
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

