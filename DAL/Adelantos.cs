using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace SGECA.DAL
{
    public class Adelantos : LogManager.IObserver, LogManager.ISubject
    {
        public int adelanto_id { get; set; }
        public int empleado_id { get; set; }
        public string empleado { get; set; }
        public string descripcion { get; set; }
        public string fecha { get; set; }
        public decimal importe { get; set; }

        private static string dbCampoAdelantoID = "adelanto_id";
        private static string dbCampoEmpleadoID = "empleado_id";
        private static string dbCampoEmpleado = "empleado";
        private static string dbCampoDescripcion = "descripcion";
        private static string dbCampoImporte = "importe";
        private static string dbCampoFecha = "fecha";
        private static string dbTabla = "empleados_adelantos";

        public LogManager.Mensaje UltimoMensaje { get; set; }

        public Adelantos()
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

        public DataTable ObtenerDataTable(DateTime desde, DateTime hasta, string tabla, List<DAL.Empleados> ids)
        {
            List<Adelantos> lista_adelantos = crear_tmp_list(desde, hasta, ids);
            DataTable retorno = Varios.ConvertToDataTable(lista_adelantos);
            return retorno;
        }

        private List<Adelantos> crear_tmp_list(DateTime desde, DateTime hasta, List<DAL.Empleados> lista_asignados)
        {
            TimeSpan ts = hasta - desde;
            int dias = ts.Days;
            List<Adelantos> lista_adelantos = new List<Adelantos>();
            List<Adelantos> lista_tmp = new List<Adelantos>();

            Empleados empleados = new Empleados();

            try
            {
                for (int i = 0; i <= dias; i++)
                {
                    DateTime fecha = desde.AddDays(i);

                    Adelantos adelanto = new Adelantos();
                    lista_adelantos = adelanto.obtener_registro_por_fecha(fecha, lista_asignados);

                    lista_tmp.AddRange(lista_adelantos);
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

        public List<Adelantos> obtener_registro_por_fecha(DateTime fecha, List<DAL.Empleados> lista_asignados)
        {
            List<string> ids = new List<string>();
            foreach (DAL.Empleados item in lista_asignados)
            {
                ids.Add(item.Id.ToString());
            }
            string[] array_id = ids.ToArray();
            string lista_ids = string.Join(",", array_id);

            List<Adelantos> retorno = new List<Adelantos>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            string consulta = @"
                SELECT
	            *
                FROM
	            (
		            SELECT
			            empleados_adelantos.*, CONCAT(empleados.apellido,', ',empleados.nombre) as empleado
		            FROM
			            empleados_adelantos
		            JOIN empleados on empleados_adelantos.empleado_id = empleados.id
	            ) AS base
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
                    Adelantos ret = new Adelantos();
                    ret.fecha = Convert.ToDateTime(reader["fecha"].ToString()).ToShortDateString();
                    ret.empleado_id = Convert.ToInt32(reader["empleado_id"].ToString());
                    ret.empleado = reader["empleado"].ToString();
                    ret.descripcion = reader["descripcion"].ToString();

                    decimal dTmp = 0;
                    decimal.TryParse(reader["importe"].ToString(), out dTmp);
                    ret.importe = dTmp;

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

        public List<Adelantos> obtenerFiltrado(ItemFiltro[] itemFiltro,
            ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros)
        {
            List<Adelantos> ret = new List<Adelantos>();
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

            comando.CommandText = "SELECT count(*) FROM (SELECT CONCAT(empleados.apellido, ', ', empleados.nombre) AS empleado, empleados_adelantos.adelanto_id, empleados_adelantos.empleado_id, empleados_adelantos.fecha, empleados_adelantos.descripcion, " +
                    "empleados_adelantos.importe FROM empleados_adelantos join empleados on empleados_adelantos.empleado_id=empleados.id) as base " + where;
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

                cadenaOrden = Varios.armarCadenaOrden(orden, cadenaOrden, dbCampoEmpleadoID);

                //TODO: Hacer Paginacion

                double rowcount = fin - (inicio - 1);

                comando.CommandText = "SELECT * from (select empleados_adelantos.adelanto_id, empleados_adelantos.empleado_id, "+
                    "CONCAT(empleados.apellido, ', ', empleados.nombre) AS empleado, empleados_adelantos.fecha, empleados_adelantos.descripcion, "+
                    "empleados_adelantos.importe from empleados_adelantos join empleados on empleados_adelantos.empleado_id=empleados.id) as base " +
                                       where + " "
                                       + cadenaOrden + ""
                                       + " LIMIT " + (inicio - 1) + ", " + rowcount;

                MySqlDataReader dr = comando.ExecuteReader();

                while (dr.Read())
                {
                    Adelantos bar = new Adelantos();
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

        private static void cargarDatos(Adelantos objeto, MySqlDataReader dr)
        {
            objeto.adelanto_id = Convert.ToInt32(dr[dbCampoAdelantoID].ToString());
            objeto.fecha = Convert.ToDateTime(dr[dbCampoFecha].ToString()).ToShortDateString();
            objeto.empleado_id = Convert.ToInt32(dr[dbCampoEmpleadoID].ToString());
            objeto.empleado = dr[dbCampoEmpleado].ToString(); 
            objeto.descripcion = dr[dbCampoDescripcion].ToString();
            objeto.importe = Convert.ToDecimal(dr[dbCampoImporte].ToString());
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

        public void Guardar(int id)
        {
            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();

            if (id==0)
            {
                comando.CommandText = "INSERT INTO adelantos (descripcion, importe) values  " +
                                       "(@descripcion, @importe)";
            }
            else
            {
                comando.CommandText = "UPDATE adelantos SET " +
                                        "descripcion = @descripcion, importe=@importe " +
                                       "where id = @id";
                comando.Parameters.AddWithValue("@id", id);
            }

            comando.Parameters.AddWithValue("@descripcion", descripcion);
            comando.Parameters.AddWithValue("@importe", Convert.ToDecimal(importe));

            comando.Transaction = Database.obtenerTransaccion();
            comando.Connection = conexion;

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                var valor = comando.ExecuteScalar();
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
    }
}
