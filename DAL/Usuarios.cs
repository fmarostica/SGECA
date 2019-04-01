using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Security.Cryptography;


namespace SGECA.DAL
{
    public class Usuarios : LogManager.ISubject, LogManager.IObserver
    {
        public int Id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string usuario { get; set; }
        public string usu_alta { get; set; }
        public string usu_baja { get; set; }
        public string usu_modificacion { get; set; }
        public bool Activo { get; set; }
        public bool chk_alta_doc { get; set; }
        public bool chk_baja_doc { get; set; }
        public bool chk_modificacion_doc { get; set; }  
        public string cliente { get; set; }
        public EAcceso acceso { get; set; }
        public EANivel nivel { get; set; }
        public string dir_local { get; set; }

        public string nombre_pc { get; set; }
        public string ruta_origen { get; set; }
        public string ruta_compartida { get; set; }

        public LogManager.Mensaje UltimoMensaje { get; set; }

        public Usuarios()
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
            this.cliente = null;
            this.nombre = null;
            this.apellido = null;
            this.email = null;
            this.Activo = true;
            this.password = null;
            this.usuario = null;
            this.usu_alta = null;
            this.usu_baja = null;
            this.usu_modificacion = null;
            this.chk_alta_doc = false;
            this.chk_baja_doc = false;
            chk_modificacion_doc = false;
            this.ruta_compartida = null;
            this.ruta_origen = null;
            this.nombre_pc = null;
            this.acceso = EAcceso.Sin_Acceso;
            this.nivel = EANivel.Usuario;

            UltimoMensaje = null;
            this.dir_local = null;
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

       

        private void cargarDatosDireccion(Usuarios objeto, MySqlDataReader dr)
        {
            objeto.ruta_origen = dr["usu_rutaOrigen"].ToString();
            objeto.ruta_compartida = dr["usu_rutaCompartida"].ToString();
            objeto.nombre_pc = dr["usu_nombrePC"].ToString(); 
        }

        public void obtenerDireccion(int id)
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
                    cargarDatosDireccion(this, dr);
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
        private void cargarDatos(Usuarios objeto, MySqlDataReader dr)
        {
            int id = 0;
            int.TryParse(dr["usu_id"].ToString(), out id);
            objeto.Id = id;

            objeto.cliente = dr["cli_cliente"].ToString();
            

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
                "FROM usuarios " +
                "WHERE  usu_email = @email "
                , conexion);

            comando.Parameters.AddWithValue("@email", codigo);
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
        public int guardarPrivilegio(int id)
        {
            Id = id;
            int retorno = 0;
            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();



            comando.CommandText = "UPDATE usuarios SET usu_alta = @usu_alta ,usu_baja = @usu_baja, usu_modificacion = @usu_modificacion" +
                                    " where usu_id= @usu_id; SELECT @usu_id";
            comando.Parameters.AddWithValue("@usu_id", Id);


            comando.Parameters.AddWithValue("@usu_alta", chk_alta_doc);
            comando.Parameters.AddWithValue("@usu_baja", chk_baja_doc);
            comando.Parameters.AddWithValue("@usu_modificacion", chk_modificacion_doc);
            

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

       

        

        

        public int guardar()
        {
            int retorno = 0;
            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();


            password = Comun.CryptoUtil.GetMd5Hash("AESA2013");


            if (Id == 0)

                comando.CommandText = "INSERT INTO usuarios (usu_nombre,usu_apellido,usu_email,usu_password,usu_activo," +
                                    "usu_usuario,usu_acceso,usu_nivel,cli_cliente) values (@nombre, @apellido,@email,@password," + 
                                    "@activo,@usuario,@acceso,@usu_nivel,@cli_cliente);" +
                                       " SELECT  Last_insert_id()";
            else
            {
                comando.CommandText = "UPDATE usuarios SET usu_email = @email,usu_nombre=@nombre,usu_apellido=@apellido," +
                                    " usu_acceso=@acceso, usu_nivel = @usu_nivel where usu_id= @usu_id; SELECT @usu_id";
                comando.Parameters.AddWithValue("@usu_id", Id);
            }

            comando.Parameters.AddWithValue("@nombre", nombre);
            comando.Parameters.AddWithValue("@apellido", apellido);
            comando.Parameters.AddWithValue("@email", email);
            comando.Parameters.AddWithValue("@activo", Activo);
            comando.Parameters.AddWithValue("@password", password);
            comando.Parameters.AddWithValue("@usuario", usuario);
            comando.Parameters.AddWithValue("@acceso", acceso);
            comando.Parameters.AddWithValue("@usu_nivel", nivel);
            comando.Parameters.AddWithValue("@cli_cliente", cliente);

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

        public int guardarContraseña(int id,string pass)
        {
            Id = id;
            int retorno = 0;
            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();


          
              comando.CommandText = "UPDATE usuarios SET usu_password= @usu_password  where usu_id= @usu_id; SELECT @usu_id";
               
            comando.Parameters.AddWithValue("@usu_id", Id);
            comando.Parameters.AddWithValue("@usu_password", pass);          
          

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
        public List<Usuarios> obtenerFiltrado(ItemFiltro[] itemFiltro,
            ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros)
        {
            List<Usuarios> ret = new List<Usuarios>();
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

            comando.CommandText = "SELECT count(*) FROM usuarios " + where;
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

                cadenaOrden = Varios.armarCadenaOrden(orden, cadenaOrden, "id_usuario");

                //TODO: Hacer Paginacion

                double rowcount = fin - (inicio - 1);

                comando.CommandText = "  SELECT *   FROM usuarios " + where + " "
                                       + cadenaOrden
                                       + " LIMIT " + (inicio - 1) + ", " + rowcount;

                MySqlDataReader dr = comando.ExecuteReader();


                while (dr.Read())
                {
                    Usuarios bar = new Usuarios();
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
                    comando.CommandText = "UPDATE usuarios set usu_activo= 'true'  " +
                                           "where usu_id= @usu_id";
                    comando.Parameters.AddWithValue("@usu_id", Id);
                }
            }
            else
            {
                if (Activo == true)
                {
                    comando.CommandText = "UPDATE usuarios set usu_activo= 'false'  " +
                                           "where usu_id= @usu_id";
                    comando.Parameters.AddWithValue("@usu_id", Id);
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

        public void obtener(string usuario, string contraseña)
        {
            limpiaDatos();

            MySqlConnection conexion = Database.obtenerConexion(true);

            ;

            MySqlCommand comando = new MySqlCommand(
                "SELECT * " +
                "FROM usuarios " +
                "WHERE  usu_usuario = @usu_usuario and usu_password = @usu_password"
                , conexion);

            comando.Parameters.AddWithValue("@usu_usuario", usuario);
            comando.Parameters.AddWithValue("@usu_password", Comun.CryptoUtil.GetMd5Hash(contraseña));

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


        public List<string> accesoCliente()
        {
            List<string> acceso = new List<string>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand("SELECT * FROM clientes", conexion);

            comando.Transaction = Database.obtenerTransaccion();
            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader dr = comando.ExecuteReader();


                while (dr.Read())
                {

                    acceso.Add(dr["cli_cliente"].ToString());

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

