using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SGECA.DAL
{
    public class EmpleadosGrupos : LogManager.IObserver, LogManager.ISubject
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string lider { get; set; }

        public EmpleadosGrupos()
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

        public int contar_registros()
        {
            int registros = 0;

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select count(*) from empleados_grupos";
            try
            {
                if (Database.obtenerTransaccion() == null) conexion.Open();
                int.TryParse(cmd.ExecuteScalar().ToString(), out registros);
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

            return registros;
        }

        public List<Empleados> obtener_miembros(string grupo_id)
        {
            string where = "";

            if (grupo_id != "") where = " where grupo=@grupo";

            List<Empleados> ret = new List<Empleados>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from empleados" + where + " order by apellido";
            cmd.Parameters.AddWithValue("@grupo", grupo_id);

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Empleados bar = new Empleados();
                    bool percibe_adelantos = false;

                    bar.Id = Convert.ToInt32(reader["id"].ToString());
                    bar.Apellido = reader["apellido"].ToString();
                    bar.ApellidoyNombre = reader["apellido"].ToString() + ", " + reader["nombre"].ToString();
                    bar.Nombre = reader["nombre"].ToString();
                    bar.Mail = reader["mail"].ToString();
                    bar.Fecha_Alta = reader["f_alta"].ToString();
                    bar.Fecha_Baja = reader["f_baja"].ToString();
                    bar.Fecha_Cierre = reader["fecha_cierre"].ToString();
                    bar.Grupo = reader["grupo"].ToString();
                    bar.Tel_laboral = reader["tel_laboral"].ToString();
                    bar.Tel_personal = reader["tel_personal"].ToString();
                    bar.Password = reader["password"].ToString();
                    bar.ApellidoyNombre = reader["apellido"].ToString() + ", " + reader["nombre"].ToString();
                    bar.CUIL = reader["cuil"].ToString();
                    bar.Domicilio = reader["domicilio"].ToString();
                    bar.Estado_Civil = reader["estado_civil_id"].ToString();
                    bar.Fecha_Nacimiento = reader["f_nacimiento"].ToString();
                    bar.hijos = Convert.ToInt32(reader["hijos"].ToString());
                    bar.Provincia = reader["provincia_id"].ToString();
                    bar.Tarea = reader["tarea_id"].ToString();
                    bar.Tel_Alternativo = reader["tel_alternativo"].ToString();
                    bar.Persona_contacto = reader["contacto"].ToString();
                    bool.TryParse(reader["percibe_adelantos"].ToString(), out percibe_adelantos);
                    bar.PercibeAdelantos = percibe_adelantos;

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

        public bool existe_en_otros_registros(string id)
        {
            bool ret = false;

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();

            comando.CommandText = "select count(*) from empleados where grupo = @id";
            comando.Parameters.AddWithValue("@id", id);

            comando.Transaction = Database.obtenerTransaccion();
            comando.Connection = conexion;

            int registros = 0;

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                int.TryParse(comando.ExecuteScalar().ToString(), out registros);
                if (registros > 0) ret = true;
            }
            catch (Exception ex)
            {
                ret = true;
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


            return ret;
        }

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

        public int contar_miembros(string grupo_id)
        {
            int miembros = 0;

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select count(*) from empleados where grupo=@grupo_id";
            cmd.Parameters.Add("@grupo_id", grupo_id);
            try
            {
                if (Database.obtenerTransaccion() == null) conexion.Open();
                int.TryParse(cmd.ExecuteScalar().ToString(), out miembros);
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

            return miembros;

        }

        public List<EmpleadosGrupos> obtenerFiltrado(ItemFiltro[] itemFiltro,
            ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros)
        {
            List<EmpleadosGrupos> ret = new List<EmpleadosGrupos>();
            UltimoMensaje = null;
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();
            comando.Connection = conexion;
            comando.Transaction = Database.obtenerTransaccion();

            totalRegistros = 0;
            int parameterCount = 0;
            string where = "";
            string tipoBusqueda = " AND ";

            if (!busquedaAnd) tipoBusqueda = " OR  ";


            Varios.armarConsultaFiltros(itemFiltro, comando, ref parameterCount, ref where, tipoBusqueda);

            string cadenaOrden = "";

            comando.CommandText = "SELECT count(*) FROM empleados_grupos " + where;
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

                cadenaOrden = Varios.armarCadenaOrden(orden, cadenaOrden, "nombre");

                //TODO: Hacer Paginacion

                double rowcount = fin - (inicio - 1);

                comando.CommandText = "  SELECT *   FROM empleados_grupos " + where + " "
                                       + cadenaOrden
                                       + " LIMIT " + (inicio - 1) + ", " + rowcount;

                MySqlDataReader dr = comando.ExecuteReader();

                while (dr.Read())
                {
                    EmpleadosGrupos bar = new EmpleadosGrupos();
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

        public bool borrar(string id)
        {
            bool resultado = true;

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();

            comando.CommandText = "Delete from empleados_grupos where id=@id";
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

        private static void cargarDatos(EmpleadosGrupos objeto, MySqlDataReader dr)
        {
            objeto.Id = dr["id"].ToString();
            objeto.Nombre = dr["nombre"].ToString();
            objeto.lider = dr["lider"].ToString();
        }

        public bool Guardar(bool nuevo, out int last_id)
        {
            bool resultado = true;

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);
            MySqlCommand comando = new MySqlCommand();
            if (nuevo)
            {
                if (Id != "") // se ingreso un id manualmente
                    comando.CommandText = "INSERT INTO empleados_grupos (id, nombre, lider)" +
                                          " values  " +
                                          "(@id, @nombre, @lider); ";
                else //id automatico
                    comando.CommandText = "INSERT INTO empleados_grupos (nombre, lider) values  " +
                                          "(@nombre, @lider); select last_insert_id()";
            }
            else
            {
                comando.CommandText = "UPDATE empleados_grupos SET nombre = @nombre, lider=@lider where id = @id";

            }
            comando.Parameters.AddWithValue("@id", Id);
            comando.Parameters.AddWithValue("@nombre", Nombre);
            comando.Parameters.AddWithValue("@lider", lider);

            comando.Transaction = Database.obtenerTransaccion();
            comando.Connection = conexion;

            int ultimo_id = 0;

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                
                if(nuevo)
                    int.TryParse(comando.ExecuteScalar().ToString(), out ultimo_id);
                else
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

            last_id = ultimo_id;

            return resultado;
        }

        public override string ToString()
        {
            return Id + "-" + Nombre;
        }

        public bool existe_grupo(string codigo)
        {
            bool ret = false;

            int registros = 0;

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select count(*) from empleados_grupos where id=@codigo";
            cmd.Parameters.Add("@codigo", codigo);
            try
            {
                if (Database.obtenerTransaccion() == null) conexion.Open();
                int.TryParse(cmd.ExecuteScalar().ToString(), out registros);
                if (registros > 0) ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
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

        public List<EmpleadosGrupos> obtener(string codigo = "")
        {
            string where = "";
            if (codigo != "") where += " where id=@codigo ";

            List<EmpleadosGrupos> ret = new List<EmpleadosGrupos>();

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from empleados_grupos" + where + " order by nombre";
            cmd.Parameters.AddWithValue("@codigo", codigo);

            try
            {
                if (Database.obtenerTransaccion() == null) conexion.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    EmpleadosGrupos bar = new EmpleadosGrupos();
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

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from empleados_grupos where id=@codigo";
            cmd.Parameters.AddWithValue("@codigo", id);

            try
            {
                if (Database.obtenerTransaccion() == null) conexion.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    EmpleadosGrupos bar = new EmpleadosGrupos();
                    ret = reader["Nombre"].ToString();
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
