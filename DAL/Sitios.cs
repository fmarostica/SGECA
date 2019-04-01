using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace SGECA.DAL
{
    public class Sitios : LogManager.ISubject, LogManager.IObserver
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string IDNombre { get; set; }
        public string Viatico_ID { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Pais { get; set; }
        public string Provincia { get; set; }
        public string Localidad { get; set; }
        public string Departamento { get; set; }

        private static string dbCampoCodigo = "CellID";
        private static string dbCampoNombre = "Nombre";
        private static string dbTabla = "sitios";

        public Sitios()
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

        public List<Sitios> obtenerFiltrado(ItemFiltro[] itemFiltro,
            ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros)
        {
            List<Sitios> ret = new List<Sitios>();
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

            comando.CommandText = "SELECT count(*) FROM " + dbTabla + " " + where;
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

                cadenaOrden = Varios.armarCadenaOrden(orden, cadenaOrden, dbCampoNombre);

                //TODO: Hacer Paginacion

                double rowcount = fin - (inicio - 1);

                comando.CommandText = "  SELECT *   FROM " + dbTabla + " " + where + " "
                                       + cadenaOrden
                                       + " LIMIT " + (inicio - 1) + ", " + rowcount;

                MySqlDataReader dr = comando.ExecuteReader();

                while (dr.Read())
                {
                    Sitios bar = new Sitios();
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

        public bool guardar(bool nuevo)
        {
            bool resultado = true;

            UltimoMensaje = null;
            DAL.Database db = new Database();
            MySqlConnection conexion = Database.obtenerConexion(true);
            MySqlCommand cmd = new MySqlCommand();
            if (nuevo)
            {
                cmd.CommandText = @"INSERT INTO sitios (CellID, Nombre, C, Latitud, Longitud, Localidad, Departamento, Provincia, Pais, 'Codigo Viatico')
                    VALUES (@id, @Nombre, @C, @Latitud, @Longitud, @Localidad, @Departamento, @Provincia, @Pais, @CodigoViatico); ";
            }
            else
            {
                cmd.CommandText = @"UPDATE sitios SET Nombre = @Nombre, C=@C, Latitud=@Latitud, Longitud=@Longitud, Localidad=@Localidad, Departamento=@Departamento,
                        Provincia=@Provincia, Pais=@Pais, `Codigo Viatico`=@CodigoViatico WHERE CellID = @id";
            }

            cmd.Parameters.AddWithValue("@id", Codigo);
            cmd.Parameters.AddWithValue("@Nombre", Nombre);
            cmd.Parameters.AddWithValue("@C", Nombre.ToUpper());
            cmd.Parameters.AddWithValue("@Latitud", Latitud);
            cmd.Parameters.AddWithValue("@Longitud", Longitud);
            cmd.Parameters.AddWithValue("@Localidad", Localidad);
            cmd.Parameters.AddWithValue("@Departamento", Departamento);
            cmd.Parameters.AddWithValue("@Provincia", Provincia);
            cmd.Parameters.AddWithValue("@Pais", Pais);
            cmd.Parameters.AddWithValue("@CodigoViatico", Viatico_ID);

            cmd.Transaction = Database.obtenerTransaccion();
            cmd.Connection = conexion;

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                var valor = cmd.ExecuteNonQuery();
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

        private static void cargarDatos(Sitios objeto, MySqlDataReader dr)
        {
            objeto.Codigo = dr[dbCampoCodigo].ToString();
            objeto.Nombre = dr[dbCampoNombre].ToString();
        }

        public List<Sitios> obtener(string busqueda="")
        {
            List<Sitios> ret = new List<Sitios>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from sitios where CellID like @filtro OR Nombre like @filtro order by CellID";
            cmd.Parameters.AddWithValue("@filtro", "%"+busqueda+"%");

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Sitios bar = new Sitios();
                    
                    bar.Subscribe(this);

                    bar.Codigo = reader["CellID"].ToString();
                    bar.Nombre = reader["Nombre"].ToString();
                    bar.Viatico_ID = reader["Codigo Viatico"].ToString();
                    bar.IDNombre = bar.Codigo + " - " + bar.Nombre;
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

        public Sitios obtener_datos(string CellID)
        {
            Sitios bar = new Sitios();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();
            cmd.CommandText = "select * from sitios where CellID = @filtro";
            cmd.Parameters.AddWithValue("@filtro", CellID);

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    bar.Codigo = reader["CellID"].ToString();
                    bar.Nombre = reader["Nombre"].ToString();
                    bar.Viatico_ID = reader["Codigo Viatico"].ToString();
                    bar.Longitud = reader["longitud"].ToString();
                    bar.Latitud = reader["latitud"].ToString();
                    bar.Pais = reader["pais"].ToString();
                    bar.Provincia = reader["provincia"].ToString();
                    bar.Departamento = reader["departamento"].ToString();
                    bar.Localidad = reader["localidad"].ToString();

                    bar.IDNombre = bar.Codigo + " - " + bar.Nombre;
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

            return bar;
        }

        public void borrar(string id)
        {
            Tipos_Gastos bar = new Tipos_Gastos();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Database.obtenerTransaccion();

            cmd.CommandText = "delete from sitios where CellID=@id";

            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                cmd.ExecuteNonQuery();
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
        }



    }
}
