using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace SGECA.DAL
{
    public class EmpleadosAdelantos : LogManager.IObserver, LogManager.ISubject
    {
        public int adelanto_id { get; set; }
        public int empleado_id { get; set; }
        public string empleado { get; set; }
        public string descripcion { get; set; }
        public string fecha { get; set; }
        public decimal importe { get; set; }

        public LogManager.Mensaje UltimoMensaje { get; set; }

        public EmpleadosAdelantos()
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

        public List<EmpleadosAdelantos> obtener(int id=0)
        {
            string where = "";
            if(id!=0)
            {
                where = "where adelanto_id=@id";
            }

            List<EmpleadosAdelantos> ret = new List<EmpleadosAdelantos>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select empleados_adelantos.adelanto_id, empleados_adelantos.fecha, empleados_adelantos.empleado_id, empleados_adelantos.descripcion, "+
                "empleados_adelantos.importe, empleados.nombre, empleados.apellido from empleados_adelantos join empleados on empleados_adelantos.empleado_id = empleados.id "+where;
            if (id != 0) cmd.Parameters.AddWithValue("@id", id);

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    EmpleadosAdelantos bar = new EmpleadosAdelantos();
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

        public EmpleadosAdelantos obtener_datos(string id)
        {
            EmpleadosAdelantos emp_adelanto = new EmpleadosAdelantos();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select empleados_adelantos.adelanto_id, empleados_adelantos.fecha, empleados_adelantos.empleado_id, empleados_adelantos.descripcion, " +
                "empleados_adelantos.importe, empleados.nombre, empleados.apellido from empleados_adelantos join empleados on empleados_adelantos.empleado_id = empleados.id where adelanto_id=@id";
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

        public void Guardar(int id)
        {
            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();

            if (id==0)
            {
                comando.CommandText = "INSERT INTO empleados_adelantos (fecha, empleado_id, descripcion, importe) values  " +
                                       "(@fecha,@empleado_id, @descripcion,@importe)";
            }
            else
            {
                comando.CommandText = "UPDATE empleados_adelantos SET fecha = @fecha," +
                                        " empleado_id = @empleado_id, descripcion = @descripcion, importe=@importe " +
                                       "where adelanto_id = @id";
                comando.Parameters.AddWithValue("@id", id);
            }

            comando.Parameters.AddWithValue("@fecha", Convert.ToDateTime(fecha));
            comando.Parameters.AddWithValue("@empleado_id", empleado_id);
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
