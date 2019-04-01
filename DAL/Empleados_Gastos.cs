using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SGECA.DAL
{
    public class Empleados_Gastos : LogManager.ISubject, LogManager.IObserver
    {
        public string id { get; set; }
        public string fecha { get; set; }
        public string asignado_por { get; set; }
        public string asignado_por_empleado { get; set; }
        public string grupo_id { get; set; }
        public string grupo_nombre { get; set; }
        public string detalles { get; set; }
        public decimal importe { get; set; }
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

        public Empleados_Gastos()
        {
            Subscribe(new LogManager.Log());
        }

        public DataTable ObtenerDataTable(string desde, string hasta, List<string> ids)
        {
            List<Empleados_Gastos> lista_gastos = crear_lista_gastos(desde, hasta, ids);
            DataTable retorno = Varios.ConvertToDataTable(lista_gastos);
            return retorno;
        }

        public Empleados_Gastos obtener_gastos(string id)
        {
            Empleados_Gastos tmp = new Empleados_Gastos();

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = @"select * from gastos where id=@id";
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                if (Database.obtenerTransaccion() == null) conexion.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tmp.fecha = Convert.ToDateTime(reader["fecha"].ToString()).ToShortDateString();
                    tmp.grupo_id = reader["grupo"].ToString();
                    tmp.importe = Convert.ToDecimal(reader["importe"].ToString());
                    tmp.detalles = reader["detalles"].ToString();
                    tmp.asignado_por = reader["asignado_por"].ToString();
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

            return tmp;
        }

        private List<Empleados_Gastos> crear_lista_gastos(string desde, string hasta, List<string> ids)
        {
            List<Empleados_Gastos> ret = new List<Empleados_Gastos>();
            string[] array_id = ids.ToArray();
            string lista_ids = string.Join(",", array_id);

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = @"select * from (SELECT empleados_grupos.*, gastos.fecha, gastos.detalles, gastos.importe FROM empleados_grupos 
                                LEFT JOIN gastos on gastos.grupo = empleados_grupos.id) as base 
                                where (fecha BETWEEN @fecha1 AND @fecha2) and id in (" + lista_ids + @")";
            cmd.Parameters.AddWithValue("@fecha1", Convert.ToDateTime(desde));
            cmd.Parameters.AddWithValue("@fecha2", Convert.ToDateTime(hasta));

            DateTime fecha = Convert.ToDateTime(desde);

            try
            {
                if (Database.obtenerTransaccion() == null) conexion.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Empleados_Gastos bar = new Empleados_Gastos();
                    bar.grupo_nombre = reader["nombre"].ToString();
                    if (reader["fecha"].ToString()!=null && reader["fecha"].ToString()!="")
                        bar.fecha = Convert.ToDateTime(reader["fecha"].ToString()).ToShortDateString();
                    decimal tmp_importe = 0;
                    decimal.TryParse(reader["importe"].ToString(), out tmp_importe);
                    bar.importe = tmp_importe;
                    bar.detalles = reader["detalles"].ToString();
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

        public List<Empleados_Gastos> obtenerFiltrado(ItemFiltro[] itemFiltro,
            ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros)
        {
            List<Empleados_Gastos> ret = new List<Empleados_Gastos>();
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

            comando.CommandText = @"SELECT count(*) FROM (select base.*, concat(empleados.apellido, ', ', empleados.nombre) as asignado_por_empleado 
            from (select gastos.*, empleados_grupos.nombre from gastos 
            left join empleados_grupos on gastos.grupo = empleados_grupos.id) as base left join empleados on base.asignado_por = empleados.id) as base2 " + where;
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

                cadenaOrden = Varios.armarCadenaOrden(orden, cadenaOrden, "fecha");

                //TODO: Hacer Paginacion

                double rowcount = fin - (inicio - 1);

                comando.CommandText = @"SELECT * FROM (select base.*, concat(empleados.apellido, ', ', empleados.nombre) as asignado_por_empleado 
                from (select gastos.*, empleados_grupos.nombre from gastos 
                left join empleados_grupos on gastos.grupo = empleados_grupos.id) as base left join empleados on base.asignado_por = empleados.id) as base2 " + where + " "
                                       + cadenaOrden
                                       + " LIMIT " + (inicio - 1) + ", " + rowcount;

                MySqlDataReader dr = comando.ExecuteReader();

                while (dr.Read())
                {
                    Empleados_Gastos bar = new Empleados_Gastos();
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

        public List<Empleados_Gastos> obtener(string id)
        {
            string where = "";
            if (fecha != "") where += " where id=@id";

            List<Empleados_Gastos> ret = new List<Empleados_Gastos>();

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from gastos" + where;
            cmd.Parameters.AddWithValue("@fecha", fecha);

            try
            {
                if (Database.obtenerTransaccion() == null) conexion.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Empleados_Gastos bar = new Empleados_Gastos();
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

        public bool Guardar(bool nuevo)
        {
            bool resultado = true;

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);
            MySqlCommand comando = new MySqlCommand();
            if (nuevo)
            {
                comando.CommandText = "INSERT INTO gastos (fecha, detalles, importe, grupo, asignado_por)" +
                                      " values  " +
                                      "(@fecha, @detalles, @importe, @grupo, @asignado_por); ";
            }
            else
            {
                comando.CommandText = @"UPDATE gastos set 
                    fecha=@fecha,
                    detalles=@detalles,
                    importe=@importe,
                    grupo=@grupo,
                    asignado_por=@asignado_por 
                    where id=@id";

            }
            comando.Parameters.AddWithValue("@id", id);
            comando.Parameters.AddWithValue("@fecha", Convert.ToDateTime(fecha));
            comando.Parameters.AddWithValue("@detalles", detalles);
            comando.Parameters.AddWithValue("@importe", Convert.ToDecimal(importe));
            comando.Parameters.AddWithValue("@grupo", grupo_id);
            comando.Parameters.AddWithValue("@asignado_por", asignado_por);

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

        public void borrar(string id)
        {
            bool resultado = true;

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);
            MySqlCommand comando = new MySqlCommand();

            comando.CommandText = "delete from gastos where id=@id";
                        comando.Parameters.AddWithValue("@id", id);

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
        }

        private static void cargarDatos(Empleados_Gastos gasto, MySqlDataReader dr)
        {
            gasto.id = dr["id"].ToString();
            gasto.fecha = Convert.ToDateTime(dr["fecha"].ToString()).ToShortDateString();
            gasto.grupo_id = dr["grupo"].ToString();
            gasto.grupo_nombre = dr["nombre"].ToString();
            gasto.importe = Convert.ToDecimal(dr["importe"].ToString());
            gasto.detalles = dr["detalles"].ToString();
            gasto.asignado_por = dr["asignado_por"].ToString();
            gasto.asignado_por_empleado = dr["asignado_por_empleado"].ToString();
        }



    }
}
