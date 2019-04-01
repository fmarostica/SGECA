using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.ComponentModel;

namespace SGECA.DAL
{
    public class Tareas : LogManager.IObserver, LogManager.ISubject
    {
        public string TareaID { get; set; }
        public string tmp_fecha { get; set; } //Usado para reportes
        public string Fecha_cierre { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Emp_id { get; set; }
        public string Empleado { get; set; }
        public string Empleado_grupo { get; set; }
        public string Empleado_grupo_nombre { get; set; }
        public string Sitio_id { get; set; }
        public string Sitio { get; set; }
        public string Estado_id { get; set; }
        public string Estado { get; set; }
        public string Trabajado { get; set; }
        public string Viatico_id { get; set; }
        public decimal Viatico { get; set; }
        public decimal tmp_adelantos { get; set; } //usado para reportes
        public string Observaciones { get; set; }
        public string Asignado_por { get; set; }
        public string Asignado_por_empleado { get; set; }

        public LogManager.Mensaje UltimoMensaje { get; set; }

        public Tareas()
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

        public DataTable ObtenerDataTable(DateTime desde, DateTime hasta, string tabla, List<string> ids)
        {
            List<Tareas> lista_tareas = crear_tmp_list2(desde,hasta,ids);
            DataTable retorno = Varios.ConvertToDataTable(lista_tareas);
            return retorno;
        }

        public DataTable ObtenerDataTable_Grupos(DateTime desde, DateTime hasta)
        {
            List<Tareas> lista_tareas = crear_tmp_list_grupos(desde, hasta);
            DataTable retorno = Varios.ConvertToDataTable(lista_tareas);
            return retorno;
        }

        private List<Tareas> crear_tmp_list_grupos(DateTime desde, DateTime hasta)
        {
            TimeSpan ts = hasta - desde;
            int dias = ts.Days;

            EmpleadosGrupos grupos = new EmpleadosGrupos();
            List<EmpleadosGrupos> lista_grupos = grupos.obtener();
            int registros_grupos = grupos.contar_registros();

            Empleados empleados = new Empleados();

            List<Tareas> lista_tareas = new List<Tareas>();
            List<string> ids = new List<string>();

            foreach (EmpleadosGrupos item in lista_grupos)
            {
                List<Empleados> lista_empleados = new List<Empleados>();
                lista_empleados = item.obtener_miembros(item.Id);
                foreach (Empleados item_emp in lista_empleados)
                {
                    ids.Add(item_emp.Id.ToString());
                }
            }

            try
            {
                for (int i = 0; i <= dias; i++)
                {
                    DateTime fecha = desde.AddDays(i);

                    List<Tareas> lista_tareas_tmp = obtener_registro_por_fecha(fecha, ids);

                    foreach (string item in ids)
                    {
                        Tareas tarea = new Tareas();
                        tarea.tmp_fecha = fecha.ToShortDateString();
                        tarea.Empleado = empleados.obtener_nombre(item);
                        tarea.Empleado_grupo = empleados.obtener_grupo(item);
                        tarea.Empleado_grupo_nombre = empleados.obtener_grupo_nombre(tarea.Empleado_grupo);

                        foreach (Tareas lista_item in lista_tareas_tmp)
                        {
                            if (item == lista_item.Emp_id)
                            {
                                tarea.Sitio = lista_item.Sitio;
                                tarea.Sitio_id = lista_item.Sitio_id;
                                tarea.Viatico = lista_item.Viatico;
                                tarea.Trabajado = lista_item.Trabajado;
                                tarea.Asignado_por = lista_item.Asignado_por;
                                tarea.Asignado_por_empleado = lista_item.Asignado_por_empleado;
                                tarea.Observaciones = lista_item.Observaciones;
                                tarea.Estado = lista_item.Estado;
                            }
                        }
                        if (tarea.Trabajado != "SI")
                        {
                            Trabajado = "NO";
                            tarea.Sitio = "NO TRABAJO";
                        }
                        tarea.tmp_adelantos = 0;
                        lista_tareas.Add(tarea);
                    }
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

            return lista_tareas;
        }

        public string obtener_sitio(DateTime fecha, string empleado_id)
        {
            string sitio = "NO TRABAJO";

            MySqlConnection conexion = Database.obtenerConexion(true);

            string consulta = @"
                SELECT sitio_id, trabajo, sitios.Nombre as sitio_nombre from empleados_tareas left join sitios on sitio_id = sitios.CellID 
                WHERE
	                empleadoId=@empleado_id
                    AND (@fecha BETWEEN fecha_inicio
                    AND fecha_fin) AND trabajo='SI'";

            MySqlCommand cmd = new MySqlCommand(consulta, conexion);
            cmd.Parameters.AddWithValue("@fecha", fecha);
            cmd.Parameters.AddWithValue("@empleado_id", empleado_id);

            cmd.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    sitio = reader["sitio_id"] + " - " + reader["sitio_nombre"].ToString();
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
                cmd.Parameters.Clear();
                if (Database.obtenerTransaccion() == null)
                    if (conexion.State != ConnectionState.Closed)
                        conexion.Close();
            }

            return sitio;
        }

        public Tareas obtener(string tarea_id)
        {
            Tareas tarea = new Tareas();

            MySqlConnection conexion = Database.obtenerConexion(true);

            string consulta = @"SELECT base.*, concat(empleados.apellido, ',', empleados.nombre) as asignado from (SELECT empleados_tareas.*, concat(empleados.apellido, ', ', empleados.nombre) as empleado, sitios.nombre as sitio from empleados_tareas 
                LEFT JOIN sitios on sitio_id = sitios.CellID 
                LEFT JOIN empleados on empleadoId = empleados.id) as base 
                LEFT JOIN empleados on asignado_por = empleados.id
                WHERE base.tarea_id=@tarea_id";

            MySqlCommand cmd = new MySqlCommand(consulta, conexion);
            cmd.Parameters.AddWithValue("@tarea_id", tarea_id);

            cmd.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tarea.TareaID = reader["tarea_id"].ToString();
                    tarea.FechaInicio = reader["fecha_inicio"].ToString();
                    tarea.FechaFin = reader["fecha_fin"].ToString();
                    tarea.Emp_id = reader["empleadoId"].ToString();
                    tarea.Empleado = reader["empleado"].ToString();
                    tarea.Sitio_id = reader["sitio_id"].ToString();
                    tarea.Observaciones = reader["observaciones"].ToString();
                    tarea.Sitio = reader["sitio"].ToString();
                    tarea.Estado_id = reader["tarea_estado_id"].ToString();
                    tarea.Trabajado = reader["trabajo"].ToString();
                    tarea.Asignado_por = reader["asignado_por"].ToString();
                    decimal dTmp = 0;
                    decimal.TryParse(reader["viatico"].ToString(), out dTmp);
                    tarea.Viatico = dTmp;
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
                cmd.Parameters.Clear();
                if (Database.obtenerTransaccion() == null)
                    if (conexion.State != ConnectionState.Closed)
                        conexion.Close();
            }

            return tarea;
        }
                        
        private List<Tareas> crear_tmp_list2(DateTime desde, DateTime hasta, List<string> ids)
        {
            TimeSpan ts = hasta - desde;
            int dias = ts.Days;

            List<Tareas> lista_tareas = new List<Tareas>();
            Empleados empleados = new Empleados();
            DAL.Empleados_Adelantos adelanto = new DAL.Empleados_Adelantos();

            try
            {
                for (int i = 0; i <= dias; i++)
                {
                    DateTime fecha = desde.AddDays(i);

                    List<Tareas> lista_tareas_tmp = obtener_registro_por_fecha(fecha, ids);

                    foreach (string item in ids)
                    {
                        Tareas tarea = new Tareas();
                        tarea.tmp_fecha = fecha.ToShortDateString();
                        tarea.Empleado = empleados.obtener_nombre(item);
                        tarea.Empleado_grupo = empleados.obtener_grupo(item);
                        foreach (Tareas lista_item in lista_tareas_tmp)
                        {
                            if(item==lista_item.Emp_id)
                            {
                                tarea.Sitio = lista_item.Sitio;
                                tarea.Sitio_id = lista_item.Sitio_id;
                                tarea.Viatico = lista_item.Viatico;
                                tarea.Trabajado = lista_item.Trabajado;
                                tarea.Asignado_por = lista_item.Asignado_por;
                                tarea.Asignado_por_empleado = lista_item.Asignado_por_empleado;
                                tarea.Observaciones = lista_item.Observaciones;
                            }
                        }
                        if (tarea.Trabajado != "SI")
                        {
                            Trabajado = "NO";
                            tarea.Sitio = "NO TRABAJO";
                        }

                        List<Empleados_Adelantos> lista_adelantos = adelanto.obtener_registro_por_fecha(fecha, item);
                        foreach (Empleados_Adelantos item_adelanto in lista_adelantos)
                        {
                            tarea.tmp_adelantos += item_adelanto.importe;
                        }
                        //tarea.tmp_adelantos = adelanto.obtener_importe(fecha, Convert.ToInt32(item));
                        lista_tareas.Add(tarea);
                    }
                    
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

            return lista_tareas;
        }


        public bool esta_superpuesto(DateTime fecha, string empleado_id)
        {
            int registros=0;
            bool resultado=false;

            MySqlConnection conexion = Database.obtenerConexion(true);

            string consulta = @"
                SELECT
	            count(*)
                FROM
	            (
		            SELECT
			            empleados_tareas.*, CONCAT(empleados.apellido,', ',empleados.nombre) as empleado
		            FROM
			            empleados_tareas
		            JOIN empleados on empleados_tareas.empleadoId = empleados.id
	            ) AS base
            WHERE
	            empleadoId=@empleado_id
            AND @fecha BETWEEN fecha_inicio
            AND fecha_fin ";

            MySqlCommand comando = new MySqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@fecha", fecha);
            comando.Parameters.AddWithValue("@empleado_id", empleado_id);

            comando.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                int.TryParse(comando.ExecuteScalar().ToString(), out registros);
                if (registros > 0)
                    resultado = true;
                else
                    resultado = false;
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
            return resultado;
        }

        public Tareas obtener_registro_por_fecha (DateTime fecha, string empleado_id)
        {
            Tareas ret = new Tareas();

            MySqlConnection conexion = Database.obtenerConexion(true);

            string consulta = @"
                SELECT
	                *
                FROM
	                (
		                SELECT
			                base.*, CONCAT(
				                empleados.apellido,
				                ', ',
				                empleados.nombre
			                ) AS asignado_por_empleado
		                FROM
			                (
				                SELECT
					                empleados_tareas.*, CONCAT(
						                empleados.apellido,
						                ', ',
						                empleados.nombre
					                ) AS empleado,
					                sitios.Nombre AS sitio,
					                tareas_estados.tarea_estado_nombre,
					                empleados_adelantos.importe AS adelanto
				                FROM
					                empleados_tareas
				                LEFT JOIN sitios ON empleados_tareas.sitio_id = sitios.CellID
				                JOIN empleados ON empleados_tareas.empleadoId = empleados.id
				                LEFT JOIN empleados_adelantos ON @fecha = empleados_adelantos.fecha
				                AND empleados_tareas.empleadoId = empleados_adelantos.empleado_id
				                LEFT JOIN tareas_estados ON empleados_tareas.tarea_estado_id = tareas_estados.tarea_estado_codigo
			                ) AS base
		                LEFT JOIN empleados ON base.asignado_por = empleados.id
	                ) AS base2
                WHERE
	                empleadoId = @empleado_id
                AND @fecha BETWEEN fecha_inicio
                AND fecha_fin";

            MySqlCommand comando = new MySqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@fecha", fecha);
            comando.Parameters.AddWithValue("@empleado_id", empleado_id);

            comando.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = comando.ExecuteReader();
                while(reader.Read())
                {
                    ret.TareaID = reader["tarea_id"].ToString();
                    ret.Emp_id = reader["empleadoId"].ToString();
                    ret.Empleado = reader["empleado"].ToString();
                    ret.Estado = reader["tarea_estado_nombre"].ToString();
                    ret.Estado_id = reader["tarea_estado_id"].ToString();
                    ret.Sitio_id = reader["sitio_id"].ToString();
                    ret.Sitio= reader["sitio"].ToString();
                    ret.Trabajado = reader["trabajo"].ToString();
                    ret.Asignado_por = reader["asignado_por_empleado"].ToString();

                    decimal viatico = 0;
                    decimal.TryParse(reader["viatico"].ToString(), out viatico);
                    ret.Viatico = viatico;

                    decimal dTmp = 0;
                    decimal.TryParse(reader["adelanto"].ToString(), out dTmp);
                    ret.tmp_adelantos = dTmp;

                    ret.Observaciones = reader["observaciones"].ToString();
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

            return ret;

        }

        public List<Tareas> obtener_registro_por_fecha_grupo(DateTime fecha)
        {
            List<Tareas> tmp_list = new List<Tareas>();
            Tareas ret = new Tareas();

            MySqlConnection conexion = Database.obtenerConexion(true);

            string consulta = @"
                SELECT
	                *
                FROM
	                (
		                SELECT
			                base.*, empleados_grupos.nombre AS grupo_nombre,
			                CONCAT(
				                empleados.apellido,
				                ', ',
				                empleados.nombre
			                ) AS asignado_por_nombre
		                FROM
			                (
				                SELECT
					                empleados_tareas.*, CONCAT(
						                empleados.apellido,
						                ', ',
						                empleados.nombre
					                ) AS empleado,
					                empleados.grupo,
					                sitios.Nombre AS sitio
				                FROM
					                empleados_tareas
				                LEFT JOIN sitios ON empleados_tareas.sitio_id = sitios.CellID
				                JOIN empleados ON empleados_tareas.empleadoId = empleados.id
			                ) AS base
		                LEFT JOIN empleados_grupos ON base.grupo = empleados_grupos.id
		                LEFT JOIN empleados ON base.asignado_por = empleados.id
	                ) AS base2
                WHERE
	                @fecha BETWEEN fecha_inicio
                AND fecha_fin";

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
                    ret.TareaID = reader["tarea_id"].ToString();
                    ret.Emp_id = reader["empleadoId"].ToString();
                    ret.Empleado = reader["empleado"].ToString();
                    ret.Empleado_grupo = reader["grupo"].ToString();
                    ret.Empleado_grupo_nombre = reader["grupo_nombre"].ToString();
                    ret.Estado = "";
                    ret.Estado_id = "";
                    ret.Sitio_id = reader["sitio_id"].ToString();
                    ret.Sitio = reader["sitio"].ToString();
                    ret.Trabajado = reader["trabajo"].ToString();
                    ret.Viatico = Convert.ToDecimal(reader["viatico"].ToString());
                    ret.Observaciones = "";
                    tmp_list.Add(ret);
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

            return tmp_list;

        }

        public List<Tareas> obtener_registro_por_fecha(DateTime fecha, List<String> ids)
        {
            string[] array_id = ids.ToArray();
            
            
            List<Tareas> retorno = new List<Tareas>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            string lista_ids = string.Join(",", array_id);

            string consulta = @"
                SELECT
	                base.* ,concat(empleados.apellido, ', ', empleados.nombre) as asignado_por_empleado, empleados_grupos.nombre as empleado_grupo_nombre
                FROM
	            (
		            SELECT
			            empleados_tareas.*, CONCAT(empleados.apellido,', ',empleados.nombre) as empleado, empleados.fecha_cierre, empleados.grupo as empleado_grupo, 
			            sitios.Nombre as sitio, tareas_estados.tarea_estado_nombre, 
                        empleados_adelantos.importe as adelanto
		            FROM
			            empleados_tareas
		            LEFT JOIN sitios on empleados_tareas.sitio_id = sitios.CellID
		            JOIN empleados on empleados_tareas.empleadoId = empleados.id
                    LEFT JOIN empleados_adelantos on @fecha=empleados_adelantos.fecha and empleados_tareas.empleadoId = empleados_adelantos.empleado_id
                    LEFT JOIN tareas_estados on tarea_estado_id = tareas_estados.tarea_estado_codigo
	            ) AS base 
                LEFT JOIN empleados on base.asignado_por = empleados.id
                LEFT JOIN empleados_grupos on base.empleado_grupo=empleados_grupos.id 
                
            WHERE
	            empleadoId in (" + lista_ids+@")
            AND @fecha BETWEEN fecha_inicio
            AND fecha_fin ";

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
                    Tareas ret = new Tareas();
                    ret.Sitio = "NO TRABAJO";

                    ret.TareaID = reader["tarea_id"].ToString();
                    ret.Emp_id = reader["empleadoId"].ToString();
                    ret.Empleado = reader["empleado"].ToString();
                    ret.Estado = reader["tarea_estado_nombre"].ToString();
                    ret.Estado_id = "";
                    ret.Sitio_id = reader["sitio_id"].ToString();
                    ret.Sitio = reader["sitio_id"] + " " + reader["sitio"].ToString();
                    ret.Trabajado = reader["trabajo"].ToString();

                    decimal vTmp = 0;
                    decimal.TryParse(reader["viatico"].ToString(), out vTmp);
                    ret.Viatico = vTmp;

                    ret.Observaciones = reader["observaciones"].ToString();
                    ret.Asignado_por = reader["asignado_por"].ToString();
                    ret.Asignado_por_empleado = reader["asignado_por_empleado"].ToString();
                    ret.Empleado_grupo_nombre = reader["empleado_grupo_nombre"].ToString();

                    decimal dTmp = 0;
                    decimal.TryParse(reader["adelanto"].ToString(), out dTmp);
                    ret.tmp_adelantos = dTmp;

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

        public List<Tareas> obtenerFiltrado(ItemFiltro[] itemFiltro,
            ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros)
        {
            List<Tareas> ret = new List<Tareas>();
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

            comando.CommandText = @"SELECT count(*) FROM (SELECT empleados_tareas.*, CONCAT(e1.apellido,', ',e1.nombre) as empleado, e1.fecha_cierre, sitios.Nombre as sitio, 
                    CONCAT(e2.apellido,', ',e2.nombre) as asignado, tareas_estados.tarea_estado_nombre as estado 
                    FROM empleados_tareas 
                    LEFT JOIN empleados e1 on empleadoId = e1.id
                    LEFT JOIN empleados e2 on asignado_por = e2.id
                    LEFT JOIN tareas_estados on empleados_tareas.tarea_estado_id = tareas_estados.tarea_estado_codigo 
                    LEFT JOIN sitios on sitio_id=sitios.CellID) as base " + where;
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

                cadenaOrden = Varios.armarCadenaOrden(orden, cadenaOrden, "fecha_inicio");

                //TODO: Hacer Paginacion

                double rowcount = fin - (inicio - 1);

                comando.CommandText = @"SELECT * FROM (SELECT empleados_tareas.*, CONCAT(e1.apellido,', ',e1.nombre) as empleado, e1.fecha_cierre, sitios.Nombre as sitio, 
                    CONCAT(e2.apellido,', ',e2.nombre) as asignado, tareas_estados.tarea_estado_nombre as estado 
                    FROM empleados_tareas 
                    LEFT JOIN empleados e1 on empleadoId = e1.id 
                    LEFT JOIN empleados e2 on asignado_por = e2.id 
                    LEFT JOIN tareas_estados on empleados_tareas.tarea_estado_id = tareas_estados.tarea_estado_codigo 
                    LEFT JOIN sitios on sitio_id=sitios.CellID) as base " + where + " "
                                       + cadenaOrden
                                       + " LIMIT " + (inicio - 1) + ", " + rowcount;

                MySqlDataReader dr = comando.ExecuteReader();

                while (dr.Read())
                {
                    Tareas bar = new Tareas();
                    bar.Subscribe(this);

                    cargarDatos(bar, dr);

                    if(bar.Fecha_cierre!="")
                    {
                        if (Convert.ToDateTime(bar.FechaInicio) >= Convert.ToDateTime(bar.Fecha_cierre))
                            ret.Add(bar);
                    }
                    else
                    {
                        ret.Add(bar);
                    }
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

        private static void cargarDatos(Tareas tarea, MySqlDataReader reader)
        {
            tarea.TareaID = reader["tarea_id"].ToString();
            tarea.FechaInicio = reader["fecha_inicio"].ToString();
            tarea.FechaFin = reader["fecha_fin"].ToString();
            tarea.Emp_id = reader["empleadoId"].ToString();
            tarea.Empleado = reader["empleado"].ToString();
            tarea.Sitio_id = reader["sitio_id"].ToString();
            tarea.Sitio = reader["sitio"].ToString();
            tarea.Asignado_por = reader["asignado_por"].ToString() + " - " + reader["asignado"].ToString();
            tarea.Fecha_cierre = reader["fecha_cierre"].ToString();
            tarea.Estado = reader["estado"].ToString();
            decimal dTmp = 0;
            decimal.TryParse(reader["viatico"].ToString(), out dTmp);
            tarea.Viatico = dTmp;
        }
                
        public bool Guardar(bool nuevo)
        {
            bool resultado = true;

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);
            MySqlCommand cmd = new MySqlCommand();

            if(nuevo)
                cmd.CommandText = "insert into empleados_tareas (fecha_inicio, fecha_fin, empleadoId, sitio_id, tarea_estado_id, trabajo, observaciones, viatico_id, viatico, asignado_por) values (" +
                "@fecha_inicio, @fecha_fin, @empleadoId, @sitio_id, @tarea_estado_id, @trabajo, @observaciones, @viatico_id, @viatico_importe, @asignado_por);";
            else
                cmd.CommandText = @"update empleados_tareas set fecha_inicio=@fecha_inicio, fecha_fin=@fecha_fin, empleadoId=@empleadoId, sitio_id=@sitio_id, 
                tarea_estado_id=@tarea_estado_id, trabajo=@trabajo, observaciones=@observaciones, viatico_id=@viatico_id, viatico=@viatico, asignado_por=@asignado_por 
                where tarea_id=@tarea_id";

            cmd.Parameters.AddWithValue("@tarea_id", TareaID);
            cmd.Parameters.AddWithValue("@fecha_inicio", Convert.ToDateTime(FechaInicio));
            cmd.Parameters.AddWithValue("@fecha_fin", Convert.ToDateTime(FechaFin));
            cmd.Parameters.AddWithValue("@empleadoId", Emp_id);
            cmd.Parameters.AddWithValue("@sitio_id", Sitio_id);
            cmd.Parameters.AddWithValue("@tarea_estado_id", Estado_id);
            cmd.Parameters.AddWithValue("@trabajo", Trabajado);
            cmd.Parameters.AddWithValue("@observaciones", Observaciones);
            cmd.Parameters.AddWithValue("@viatico", Viatico);
            cmd.Parameters.AddWithValue("@viatico_id", Viatico_id);
            cmd.Parameters.AddWithValue("@viatico_importe", Viatico);
            cmd.Parameters.AddWithValue("@asignado_por", Asignado_por);

            cmd.Transaction = Database.obtenerTransaccion();
            cmd.Connection = conexion;

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                cmd.ExecuteNonQuery();
                
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
                cmd.Parameters.Clear();
                if (Database.obtenerTransaccion() == null)
                    if (conexion.State != ConnectionState.Closed)
                        conexion.Close();
            }

            return resultado;
        }

        public bool borrar(string id)
        {
            bool resultado = true;

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();

            comando.CommandText = "Delete from empleados_tareas where tarea_id= @id";
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

    }
}
