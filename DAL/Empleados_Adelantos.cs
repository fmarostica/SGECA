using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace SGECA.DAL
{
    public class Empleados_Adelantos : LogManager.IObserver, LogManager.ISubject
    {
        public int adelanto_id { get; set; }
        public int empleado_id { get; set; }
        public string asignado_por { get; set; }
        public string asignado_por_empleado { get; set; }
        public string empleado { get; set; }
        public string descripcion { get; set; }
        public string fecha { get; set; }
        public decimal importe { get; set; }

        public LogManager.Mensaje UltimoMensaje { get; set; }

        public Empleados_Adelantos()
        {
            Subscribe(new LogManager.Log());
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

        public List<Empleados_Adelantos> obtener(int id = 0)
        {
            string where = "";
            if (id != 0)
            {
                where = "where adelanto_id=@id";
            }

            List<Empleados_Adelantos> ret = new List<Empleados_Adelantos>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select empleados_adelantos.adelanto_id, empleados_adelantos.fecha, empleados_adelantos.empleado_id, empleados_adelantos.descripcion, " +
                "empleados_adelantos.importe, empleados.nombre, empleados.apellido from empleados_adelantos join empleados on empleados_adelantos.empleado_id = empleados.id " + where;
            if (id != 0) cmd.Parameters.AddWithValue("@id", id);

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Empleados_Adelantos bar = new Empleados_Adelantos();
                    bar.Subscribe(this);
                    bar.adelanto_id = Convert.ToInt32(reader["adelanto_id"].ToString());
                    bar.fecha = Convert.ToDateTime(reader["fecha"]).ToShortDateString();
                    bar.empleado_id = Convert.ToInt32(reader["empleado_id"]);
                    bar.empleado = reader["apellido"].ToString() + ", " + reader["nombre"].ToString();
                    bar.descripcion = reader["descripcion"].ToString();
                    bar.importe = Convert.ToDecimal(reader["importe"].ToString());

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

        public Empleados_Adelantos obtener_datos(string id)
        {
            Empleados_Adelantos emp_adelanto = new Empleados_Adelantos();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = @"SELECT empleados_adelantos.*, empleados.nombre, empleados.apellido 
            FROM empleados_adelantos join empleados on empleados_adelantos.empleado_id = empleados.id where adelanto_id=@id";
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    emp_adelanto.adelanto_id = Convert.ToInt32(reader["adelanto_id"].ToString());
                    emp_adelanto.fecha = Convert.ToDateTime(reader["fecha"]).ToShortDateString();
                    emp_adelanto.empleado_id = Convert.ToInt32(reader["empleado_id"]);
                    emp_adelanto.empleado = reader["apellido"].ToString() + ", " + reader["nombre"].ToString();
                    emp_adelanto.descripcion = reader["descripcion"].ToString();
                    emp_adelanto.importe = Convert.ToDecimal(reader["importe"].ToString());
                    emp_adelanto.asignado_por = reader["asignado_por"].ToString();
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

            return emp_adelanto;
        }

        public bool borrar(int id)
        {
            bool resultado = true;

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();

            comando.CommandText = "Delete from empleados_adelantos where adelanto_id= @id";
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

        public List<Empleados_Adelantos> obtenerFiltrado(ItemFiltro[] itemFiltro,
            ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros)
        {
            List<Empleados_Adelantos> ret = new List<Empleados_Adelantos>();
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

            comando.CommandText = @"SELECT count(*) FROM (select base.*, concat(empleados.apellido, ', ', empleados.nombre) as asignado_por_empleado from (SELECT CONCAT(empleados.apellido, ', ', empleados.nombre) AS empleado, empleados_adelantos.* 
                FROM empleados_adelantos join empleados on empleados_adelantos.empleado_id=empleados.id) as base left join empleados on base.asignado_por = empleados.id) as base2  " + where;
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

                cadenaOrden = Varios.armarCadenaOrden(orden, cadenaOrden, "empleado_id");

                //TODO: Hacer Paginacion

                double rowcount = fin - (inicio - 1);

                comando.CommandText = "SELECT * from (select base.*, concat (empleados.apellido, ', ', empleados.nombre) as asignado_por_empleado "+
                    "FROM (select empleados_adelantos.*, CONCAT(empleados.apellido, ', ', empleados.nombre) AS empleado, empleados.fecha_cierre from empleados_adelantos "+
                    "JOIN empleados on empleados_adelantos.empleado_id=empleados.id) as base "+
                    "LEFT JOIN empleados on base.asignado_por = empleados.id) as base2 " +
                                       where + " "
                                       + cadenaOrden + ""
                                       + " LIMIT " + (inicio - 1) + ", " + rowcount;
                comando.Parameters.AddWithValue("@fecha", DateTime.Now);

                MySqlDataReader dr = comando.ExecuteReader();

                while (dr.Read())
                {
                    Empleados_Adelantos bar = new Empleados_Adelantos();
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

        public DataTable ObtenerDataTable(DateTime desde, DateTime hasta, string tabla, List<DAL.Empleados> ids)
        {
            List<Empleados_Adelantos> lista_adelantos = crear_tmp_list(desde, hasta, ids);
            DataTable retorno = Varios.ConvertToDataTable(lista_adelantos);
            return retorno;
        }

        private List<Empleados_Adelantos> crear_tmp_list(DateTime desde, DateTime hasta, List<DAL.Empleados> lista_asignados)
        {
            TimeSpan ts = hasta - desde;
            int dias = ts.Days;
            

            List<Empleados_Adelantos> lista_adelantos = new List<Empleados_Adelantos>();
            List<Empleados_Adelantos> lista_tmp = new List<Empleados_Adelantos>();
            Empleados empleados = new Empleados();

            try
            {
                for (int i = 0; i <= dias; i++)
                {
                    DateTime fecha = desde.AddDays(i);
                    Empleados_Adelantos adelanto = new Empleados_Adelantos();
                    lista_adelantos = adelanto.obtener_registro_por_fecha(fecha, lista_asignados);
                    if(lista_adelantos.Count>0)
                        lista_tmp.AddRange(lista_adelantos);
                    else
                        lista_tmp.Add(new Empleados_Adelantos { fecha = fecha.ToShortDateString() });
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
                UltimoMensaje.StackTrace = ex.StackTrace;
                Notify(UltimoMensaje);
            }

            return lista_tmp;
        }

        public List<Empleados_Adelantos> obtener_registro_por_fecha(DateTime fecha, List<DAL.Empleados> lista_asignados)
        {
            List<string> ids = new List<string>();
            foreach (DAL.Empleados item in lista_asignados)
            {
                ids.Add(item.Id.ToString());
            }
            string[] array_id = ids.ToArray();
            string lista_ids = string.Join(",", array_id);

            List<Empleados_Adelantos> retorno = new List<Empleados_Adelantos>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            string consulta = @"
                SELECT
	                base.*, concat(empleados.apellido, ', ', empleados.nombre) as asignado_por_empleado
                FROM
	            (
		            SELECT
			            empleados_adelantos.*, CONCAT(empleados.apellido,', ',empleados.nombre) as empleado
		            FROM
			            empleados_adelantos
		            JOIN empleados on empleados_adelantos.empleado_id = empleados.id
	            ) AS base left join empleados on base.asignado_por = empleados.id
            WHERE
	            empleado_id in (" + lista_ids + @") 
            AND fecha BETWEEN @fecha
            AND @fecha order by empleado_id";

            MySqlCommand comando = new MySqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@fecha", fecha);

            comando.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    Empleados_Adelantos ret = new Empleados_Adelantos();
                    cargarDatos(ret, reader);


                    retorno.Add(ret);
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

            return retorno;
        }

        public List<Empleados_Adelantos> obtener_registro_por_fecha(DateTime fecha, string empledo_id)
        {
            List<Empleados_Adelantos> retorno = new List<Empleados_Adelantos>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            string consulta = @"
                SELECT
	                base.*, concat(empleados.apellido, ', ', empleados.nombre) as asignado_por_empleado
                FROM
	            (
		            SELECT
			            empleados_adelantos.*, CONCAT(empleados.apellido,', ',empleados.nombre) as empleado
		            FROM
			            empleados_adelantos
		            JOIN empleados on empleados_adelantos.empleado_id = empleados.id
	            ) AS base left join empleados on base.asignado_por = empleados.id
            WHERE
	            empleado_id = @empleado_id 
            AND fecha BETWEEN @fecha
            AND @fecha order by empleado_id";

            MySqlCommand comando = new MySqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@fecha", fecha);
            comando.Parameters.AddWithValue("@empleado_id", empledo_id);
            comando.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    Empleados_Adelantos ret = new Empleados_Adelantos();
                    cargarDatos(ret, reader);
                    retorno.Add(ret);
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

            return retorno;
        }

        private static void cargarDatos(Empleados_Adelantos adelanto, MySqlDataReader dr)
        {
            adelanto.adelanto_id = Convert.ToInt32(dr["adelanto_id"].ToString());
            adelanto.fecha = Convert.ToDateTime(dr["fecha"].ToString()).ToShortDateString();
            adelanto.empleado_id = Convert.ToInt32(dr["empleado_id"].ToString());
            adelanto.empleado = dr["empleado"].ToString();
            adelanto.descripcion = dr["descripcion"].ToString();

            decimal dTmp = 0;
            decimal.TryParse(dr["importe"].ToString(), out dTmp);
            adelanto.importe = dTmp;
            
            adelanto.asignado_por = dr["asignado_por"].ToString();
            adelanto.asignado_por_empleado = dr["asignado_por_empleado"].ToString();
        }

        public void Guardar(bool nuevo)
        {
            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();

            if (nuevo)
            {
                cmd.CommandText = "INSERT INTO empleados_adelantos (fecha, empleado_id, descripcion, importe, asignado_por) values  " +
                                       "(@fecha,@empleado_id, @descripcion,@importe, @asignado_por)";
            }
            else
            {
                cmd.CommandText = "UPDATE empleados_adelantos SET fecha = @fecha," +
                                        " empleado_id = @empleado_id, descripcion = @descripcion, importe=@importe, asignado_por=@asignado_por " +
                                       "where adelanto_id = @id";
                cmd.Parameters.AddWithValue("@id", adelanto_id);
            }

            cmd.Parameters.AddWithValue("@fecha", Convert.ToDateTime(fecha));
            cmd.Parameters.AddWithValue("@empleado_id", empleado_id);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);
            cmd.Parameters.AddWithValue("@importe", importe);
            cmd.Parameters.AddWithValue("@asignado_por", asignado_por);

            cmd.Transaction = Database.obtenerTransaccion();
            cmd.Connection = conexion;

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                var valor = cmd.ExecuteScalar();
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
                cmd.Parameters.Clear();
                if (Database.obtenerTransaccion() == null)
                    if (conexion.State != ConnectionState.Closed)
                        conexion.Close();
            }
        }

        public decimal obtener_importe(DateTime fecha, int empleado_id)
        {
            decimal ret = 0;

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select importe from empleados_adelantos where fecha=@fecha and empleado_id=@empleado_id";
            cmd.Parameters.AddWithValue("@fecha", fecha);
            cmd.Parameters.AddWithValue("@empleado_id", empleado_id);
            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    decimal.TryParse(reader["importe"].ToString(), out ret);
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
