using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace SGECA.DAL
{
    [Serializable]
    public class Empleados : LogManager.IObserver, LogManager.ISubject
    {
        public int Id { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string CUIL { get; set; }
        public string Tel_laboral { get; set; }
        public string Tel_personal { get; set; }
        public string Mail { get; set; }
        public string Fecha_Alta { get; set; }
        public string Fecha_Baja { get; set; }
        public string Fecha_Nacimiento { get; set; }
        public string Fecha_Cierre { get; set; }
        public string Estado_Civil { get; set; }
        public string Domicilio { get; set; }
        public string Provincia { get; set; }
        public int hijos { get; set; }
        public string Grupo { get; set; }
        public string Password { get; set; }
        public string ApellidoyNombre { get; set; }
        public string Tarea { get; set; }
        public string Tel_Alternativo { get; set; }
        public string Persona_contacto { get; set; }
        public bool PercibeAdelantos { get; set; }
        public string recovery_key { get; set; }

        public Empleados()
        {
            Subscribe(new LogManager.Log());
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

        public List<Empleados> Obtener_lista(string Estado="TODOS")
        {
            string where = "";

            if (Estado == "ACTIVO" || Estado == "ACTIVOS") where = " where f_baja IS NULL or f_baja = '' ";
            if (Estado == "BAJA") where = " where f_baja IS NOT NULL or f_baja <> '' ";

            List<Empleados> ret = new List<Empleados>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from empleados "+where+" order by apellido";

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Empleados bar = new Empleados();
                    bar.Subscribe(this);
                    cargarDatos(bar, reader);
                    
                    ret.Add(bar);
                }

                reader.Close();
            }
            catch(Exception ex)
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

        public bool es_lider (string empleado_id)
        {
            bool ret = false;

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select count(*) from empleados_grupos where lider=@empleado_id";
            cmd.Parameters.AddWithValue("@empleado_id", empleado_id);
            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                int registros = 0;
                int.TryParse(cmd.ExecuteScalar().ToString(), out registros);
                if (registros > 0) ret = true;
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

        public List<Empleados> Obtener_lista(bool en_grupo, string Estado = "TODOS")
        {
            string where = "";

            if (Estado == "ACTIVO" || Estado == "ACTIVOS") where = " where (f_baja IS NULL or f_baja = '')";
            if (Estado == "BAJA") where = " where (f_baja IS NOT NULL or f_baja <> '')";

            if (where != "")
                where += " AND ";
            else
                where += " WHERE ";

            if (en_grupo)
                where += " (grupo <>'' and grupo IS NOT NULL) ";
            else
                where += " (grupo='' or grupo IS NULL) ";
                

            List<Empleados> ret = new List<Empleados>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from empleados " + where + " order by apellido";

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Empleados bar = new Empleados();
                    bar.Subscribe(this);
                    cargarDatos(bar, reader);

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

        public Empleados Obtener(string id)
        {
            Empleados ret = new Empleados();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from empleados where id=@id";
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cargarDatos(ret, reader);
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

        public Empleados Obtener_por_mail(string mail)
        {
            Empleados ret = new Empleados();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from empleados where mail=@mail";
            cmd.Parameters.AddWithValue("@mail", mail);

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cargarDatos(ret, reader);
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

        public Empleados Obtener_por_key(string key)
        {
            Empleados ret = new Empleados();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from empleados where recovery_key=@key";
            cmd.Parameters.AddWithValue("@key", key);

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cargarDatos(ret, reader);
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

        public bool existe_id(string id)
        {
            bool ret = false;

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select count(*) from empleados where id=@id";

            try
            {
                if (Database.obtenerTransaccion() == null) conexion.Open();
                int registros = 0;
                int.TryParse(cmd.ExecuteScalar().ToString(), out registros);
                if (registros > 0) ret = true;
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

        public DataTable ObtenerDataTable(string tabla, string estado)
        {
            Empleados emp = new Empleados();
            List<Empleados> lista_empleados = emp.Obtener_lista(estado);
            DataTable retorno = Varios.ConvertToDataTable(lista_empleados);
            return retorno;
        }

        public Empleados login(string usuario, string pass)
        {
            Empleados res = new Empleados();

            if(!usuario.Contains("@"))
            {
                usuario += "@telesoluciones.net";
            }
            

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from empleados where mail=@usuario and password=@password";
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@password", pass);

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    cargarDatos(res, reader);
                }
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
            
            return res;
        }

        
        public List<Empleados> Obtener_miembros_grupos(string grupo, bool solos_activos)
        {
            string where = "";

            if (solos_activos) where = " and f_baja is null";

            List<Empleados> ret = new List<Empleados>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from empleados where grupo=@grupo " + where + " order by apellido";
            cmd.Parameters.AddWithValue("@grupo", grupo);

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Empleados bar = new Empleados();
                    bar.Subscribe(this);
                    cargarDatos(bar, reader);

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

        public List<Empleados> Obtener_lista_adelantos(bool percibe_adelanto, string filtro="")
        {
            string where = "";
            if (filtro != "") where = " and (apellido like @filtro or nombre like @filtro) ";

            List<Empleados> ret = new List<Empleados>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from empleados where percibe_adelantos like @percibe_adelanto " + where + " order by apellido and f_baja IS NULL";
            cmd.Parameters.AddWithValue("@percibe_adelanto", "%" + percibe_adelanto + "%");
            cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Empleados bar = new Empleados();
                    bar.Subscribe(this);
                    cargarDatos(bar, reader);
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

        public string obtener_nombre(string id)
        {
            string ret = "";

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from empleados where id=@id";
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ret = reader["apellido"].ToString() + ", " + reader["nombre"].ToString();
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



        public string obtener_grupo(string id)
        {
            string ret = "";

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from empleados where id=@id";
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ret = reader["grupo"].ToString();
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

        public string obtener_grupo_nombre(string id)
        {
            string ret = "";

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from empleados_grupos where id=@id";
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ret = reader["nombre"].ToString();
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

        public List<Empleados> obtenerFiltrado(ItemFiltro[] itemFiltro,
            ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros, bool solo_activos)
        {
            List<Empleados> ret = new List<Empleados>();
            UltimoMensaje = null;
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();
            comando.Connection = conexion;
            comando.Transaction = Database.obtenerTransaccion();

            totalRegistros = 0;
            int parameterCount = 0;
            string where = "";
            if (solo_activos) where = "";
            string tipoBusqueda = " AND ";

            if (!busquedaAnd) tipoBusqueda = " OR  ";

            
            Varios.armarConsultaFiltros(itemFiltro, comando, ref parameterCount, ref where, tipoBusqueda);
            
            string cadenaOrden = "";

            comando.CommandText = "SELECT count(*) FROM empleados " + where;
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

                cadenaOrden = Varios.armarCadenaOrden(orden, cadenaOrden, "apellido");

                //TODO: Hacer Paginacion

                double rowcount = fin - (inicio - 1);

                comando.CommandText = "  SELECT *   FROM empleados " + where + " "
                                       + cadenaOrden
                                       + " LIMIT " + (inicio - 1) + ", " + rowcount;
                
                MySqlDataReader dr = comando.ExecuteReader();
                
                while (dr.Read())
                {
                    Empleados bar = new Empleados();
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

        public void Guardar(bool nuevo)
        {
            bool clave_cambiada = false;

            if(Id>0)
            {
                Empleados emptmp = new Empleados();
                emptmp = emptmp.Obtener(Id.ToString());
                if (emptmp.Password.ToString() != Password.ToString()) clave_cambiada = true;
            }
            

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();

            if (nuevo)
            {
                if(Id!=0) // se ingreso un id manualmente
                    comando.CommandText = @"INSERT INTO empleados (id, apellido,nombre,tel_laboral,
                                            tel_personal, mail, f_alta, f_baja, grupo, 
                                            domicilio, f_nacimiento, fecha_cierre, provincia_id, hijos, tarea_id, password, cuil, estado_civil_id, tel_alternativo, contacto, percibe_adelantos) values  
                                            (@id,@apellido, @nombre,@tel_laboral,@tel_personal, @mail, @f_alta, @f_baja, @grupo,
                                            @domicilio, @f_nacimiento, @fecha_cierre, @provincia_id, @hijos, @tarea_id, @password, @cuil, @estado_civil, @tel_alternativo, @contacto, @percibe_adelantos); ";
                else //id automatico
                    comando.CommandText = @"INSERT INTO empleados (apellido,nombre,tel_laboral,
                                          tel_personal, mail, f_alta, f_baja, grupo,
                                          domicilio, f_nacimiento, fecha_cierre, provincia_id, hijos, tarea_id, password, cuil, estado_civil_id, tel_alternativo, contacto, percibe_adelantos) values
                                          (@apellido, @nombre,@tel_laboral,@tel_personal, @mail, @f_alta, @f_baja, @grupo,
                                          @domicilio, @f_nacimiento, @fecha_cierre, @provincia_id, @hijos, @tarea_id, @password, @cuil, @estado_civil,  @tel_alternativo, @contacto, @percibe_adelantos); ";
            }
            else
            {
                comando.CommandText = @"UPDATE empleados SET
                                        apellido = @apellido, 
                                        nombre = @nombre, 
                                        cuil = @cuil,
                                        estado_civil_id=@estado_civil,
                                        hijos=@hijos,
                                        tel_laboral=@tel_laboral, 
                                        tel_personal=@tel_personal, 
                                        mail=@mail, 
                                        f_alta=@f_alta,
                                        grupo=@grupo,
                                        domicilio=@domicilio,
                                        provincia_id=@provincia_id,
                                        tarea_id=@tarea_id,
                                        f_nacimiento=@f_nacimiento,
                                        fecha_cierre=@fecha_cierre,
                                        password=@password,
                                        tel_alternativo=@tel_alternativo,
                                        contacto=@contacto,
                                        f_baja=@f_baja,
                                        percibe_adelantos=@percibe_adelantos,
                                        recovery_key=@recovery_key 
                                        where id=@id";
            }

            DateTime FechaAlta=DateTime.Now;
            DateTime FechaBaja=DateTime.Now;
            DateTime FechaNacimiento = DateTime.Now;
            DateTime FechaCierre = DateTime.Now;

            if (Fecha_Alta == "")
                Fecha_Alta = null;
            else
                DateTime.TryParse(Fecha_Alta, out FechaAlta);

            if (Fecha_Baja == "") 
                Fecha_Baja = null;
            else
                DateTime.TryParse(Fecha_Baja, out FechaBaja);

            if (Fecha_Nacimiento == "")
                Fecha_Nacimiento = null;
            else
                DateTime.TryParse(Fecha_Nacimiento, out FechaNacimiento);

            if (Fecha_Cierre == "")
                Fecha_Cierre = null;
            else
                DateTime.TryParse(Fecha_Cierre, out FechaCierre);

            comando.Parameters.AddWithValue("@id", Id);
            comando.Parameters.AddWithValue("@apellido", Apellido);
            comando.Parameters.AddWithValue("@nombre", Nombre);
            comando.Parameters.AddWithValue("@tel_laboral", Tel_laboral);
            comando.Parameters.AddWithValue("@tel_personal", Tel_personal);
            comando.Parameters.AddWithValue("@mail", Mail);
            comando.Parameters.AddWithValue("@cuil", CUIL);
            comando.Parameters.AddWithValue("@estado_civil", Estado_Civil);
            comando.Parameters.AddWithValue("@hijos", hijos);
            comando.Parameters.AddWithValue("@domicilio", Domicilio);
            comando.Parameters.AddWithValue("@provincia_id", Provincia);
            comando.Parameters.AddWithValue("@recovery_key", recovery_key);

            if(!clave_cambiada)
                comando.Parameters.AddWithValue("@password", Password);
            else
                comando.Parameters.AddWithValue("@password", DAL.Varios.MD5Hash(Password));

            comando.Parameters.AddWithValue("@tarea_id", Tarea);
            comando.Parameters.AddWithValue("@grupo", Grupo);
            comando.Parameters.AddWithValue("@tel_alternativo", Tel_Alternativo);
            comando.Parameters.AddWithValue("@contacto", Persona_contacto);
            comando.Parameters.AddWithValue("@percibe_adelantos", PercibeAdelantos.ToString());
            if (Fecha_Alta == null)
                comando.Parameters.AddWithValue("@f_alta", Fecha_Alta);
            else
                comando.Parameters.AddWithValue("@f_alta", Convert.ToDateTime(Fecha_Alta));

            if (Fecha_Baja == null)
                comando.Parameters.AddWithValue("@f_baja", Fecha_Baja);
            else
                comando.Parameters.AddWithValue("@f_baja", Convert.ToDateTime(Fecha_Baja));

            if (Fecha_Nacimiento == null)
                comando.Parameters.AddWithValue("@f_nacimiento", Fecha_Nacimiento);
            else
                comando.Parameters.AddWithValue("@f_nacimiento", Convert.ToDateTime(Fecha_Nacimiento));

            if (Fecha_Cierre == null)
                comando.Parameters.AddWithValue("@fecha_cierre", Fecha_Cierre);
            else
                comando.Parameters.AddWithValue("@fecha_cierre", Convert.ToDateTime(Fecha_Cierre));
            

            comando.Transaction = Database.obtenerTransaccion();
            comando.Connection = conexion;

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                comando.ExecuteNonQuery();
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

        public bool tiene_tareas_asignadas(int id)
        {
            bool res = false;

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();

            comando.CommandText = "select count(*) from empleados_tareas where empleadoId=@id";
            comando.Parameters.AddWithValue("@id", id);

            comando.Transaction = Database.obtenerTransaccion();
            comando.Connection = conexion;

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                int registros = 0;
                registros = int.Parse(comando.ExecuteScalar().ToString());
                if (registros > 0) res = true;
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

            return res;
        }

        public bool tiene_adelantos_asignados(int id)
        {
            bool res = false;

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();

            comando.CommandText = "select count(*) from empleados_adelantos where empleado_id=@id";
            comando.Parameters.AddWithValue("@id", id);

            comando.Transaction = Database.obtenerTransaccion();
            comando.Connection = conexion;

            
            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                int registros = 0;
                registros = int.Parse(comando.ExecuteScalar().ToString());
                if (registros > 0) res = true;
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

            return res;
        }

        public bool borrar(int id)
        {
            bool resultado = true;

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();
           
            comando.CommandText = "Delete from empleados where id= @id";
            comando.Parameters.AddWithValue("@id", id);

            comando.Transaction = Database.obtenerTransaccion();
            comando.Connection = conexion;

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                resultado = false;
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

            return resultado;
        }

        private static void cargarDatos(Empleados bar, MySqlDataReader dr)
        {
            bool percibe_adelantos = false;

            bar.Id = Convert.ToInt32(dr["id"].ToString());
            bar.Apellido = dr["apellido"].ToString();
            string apellidoynombre = dr["apellido"].ToString() + ", " + dr["nombre"].ToString();
            bar.ApellidoyNombre = apellidoynombre.ToUpper();
            bar.Nombre = dr["nombre"].ToString();
            bar.Mail = dr["mail"].ToString();
            bar.Fecha_Alta = dr["f_alta"].ToString();
            bar.Fecha_Baja = dr["f_baja"].ToString();
            bar.Fecha_Cierre = dr["fecha_cierre"].ToString();
            bar.Grupo = dr["grupo"].ToString();
            bar.Tel_laboral = dr["tel_laboral"].ToString();
            bar.Tel_personal = dr["tel_personal"].ToString();
            bar.Password = dr["password"].ToString();
            bar.ApellidoyNombre = dr["apellido"].ToString() + ", " + dr["nombre"].ToString();
            bar.CUIL = dr["cuil"].ToString();
            bar.Domicilio = dr["domicilio"].ToString();
            bar.Estado_Civil = dr["estado_civil_id"].ToString();
            bar.Fecha_Nacimiento = dr["f_nacimiento"].ToString();
            bar.hijos = Convert.ToInt32(dr["hijos"].ToString());
            bar.Provincia = dr["provincia_id"].ToString();
            bar.Tarea = dr["tarea_id"].ToString();
            bar.Tel_Alternativo = dr["tel_alternativo"].ToString();
            bar.Persona_contacto = dr["contacto"].ToString();
            bool.TryParse(dr["percibe_adelantos"].ToString(), out percibe_adelantos);
            bar.PercibeAdelantos = percibe_adelantos;
        }
    }
}
