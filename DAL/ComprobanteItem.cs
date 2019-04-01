using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace SGECA.DAL
{
    [Serializable]
    public class ComprobanteItem : LogManager.ISubject, LogManager.IObserver
    {
        public int Id { get; set; }
        public int EncabezaoId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Costo { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Descuento { get; set; }
        public decimal AlicuotaPorcentaje { get; set; }
        public int AlicuotaId { get; set; }
        public int TareaOrdencompraId { get; set; }
        public int Orden { get; set; }

        public decimal Subtotal
        {
            get
            {
                decimal retorno = 0;

                retorno = Subtotalneto;

                retorno = retorno * (1 + (AlicuotaPorcentaje / 100));

                return Math.Round(retorno, 2);
            }



        }
        public decimal Subtotalneto
        {
            get
            {
                decimal retorno = 0;
                retorno = Cantidad * Precio;
                retorno = retorno * (1 + (Descuento / 100));

                return Math.Round(retorno, 2);
            }


        }


        public decimal MontoIVA
        {
            get
            {
                return Math.Round(Subtotal - Subtotalneto, 2);
            }
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

        public ComprobanteItem()
        {
            //suscripción al Log
            Subscribe(new LogManager.Log());
        }
        private void limpiaDatos()
        {
            Id = 0;
            EncabezaoId = 0;
            Codigo = null;
            Descripcion = null;
            Costo = 0;
            Cantidad = 0;
            Precio = 0;
            Descuento = 0;
            AlicuotaPorcentaje = 0;
            AlicuotaId = 0;
            UltimoMensaje = null;
            TareaOrdencompraId = 0;

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
                "FROM clientes " +
                "WHERE cli_id = @cli_id "
                , conexion);

            comando.Parameters.AddWithValue("@cli_id", id);
            comando.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader dr = comando.ExecuteReader();

                if (dr.Read())
                {

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
        private void cargarDatos(ComprobanteItem objeto, MySqlDataReader dr)
        {
            int intTemp = 0;
            decimal decTemp = 0;

            intTemp = 0;
            int.TryParse(dr["cim_id"].ToString(), out intTemp);
            objeto.Id = intTemp;

            intTemp = 0;
            int.TryParse(dr["cen_id"].ToString(), out intTemp);
            objeto.EncabezaoId = intTemp;
            intTemp = 0;

            int.TryParse(dr["toc_id"].ToString(), out intTemp);
            objeto.TareaOrdencompraId = intTemp;


            objeto.Codigo = dr["pro_codigo"].ToString();
            objeto.Descripcion = dr["pro_descripcion"].ToString();


            //objeto.telefono = dr["cli_telefono"].ToString();
            //objeto.direccion = dr["cli_direccion"].ToString();
            //objeto.observaciones = dr["cli_observaciones"].ToString();

            decTemp = 0;
            decimal.TryParse(dr["cim_Cantidad"].ToString(), out decTemp);
            objeto.Cantidad = decTemp;

            decTemp = 0;
            decimal.TryParse(dr["pro_precio"].ToString(), out decTemp);
            objeto.Precio = decTemp;

            decTemp = 0;
            decimal.TryParse(dr["pro_costo"].ToString(), out decTemp);
            objeto.Costo = decTemp;

            decTemp = 0;
            decimal.TryParse(dr["cim_descuento"].ToString(), out decTemp);
            objeto.Descuento = decTemp;

            intTemp = 0;
            int.TryParse(dr["ali_id"].ToString(), out intTemp);
            objeto.AlicuotaId = intTemp;

            decTemp = 0;
            decimal.TryParse(dr["ali_porcentaje"].ToString(), out decTemp);
            objeto.AlicuotaPorcentaje = decTemp;




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
                "FROM clientes " +
                "WHERE  cli_cliente = @cli_cliente "
                , conexion);

            comando.Parameters.AddWithValue("@cli_cliente", codigo);
            comando.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader dr = comando.ExecuteReader();

                if (dr.Read())
                {

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


            comando.CommandText = "delete from clientes " +
                                   "where cli_id= @cli_id";
            comando.Parameters.AddWithValue("@cli_id", Id);

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
                comando.CommandText = "INSERT INTO comprobanteitem (cen_id,pro_Codigo,pro_descripcion,pro_costo,pro_precio," +
                                      "cim_cantidad,cim_descuento,ali_id,ali_porcentaje,cim_neto,cim_total,toc_id,cim_orden) VALUES" +
                                      "(@cen_id,@pro_Codigo,@pro_descripcion, @pro_costo,@pro_precio,@cim_cantidad,@cim_descuento," +
                                      "@ali_id,@ali_porcentaje,@cim_neto,@cim_total,@toc_id,@cim_orden);" +
                                      " SELECT  Last_insert_id()";
            }
            else
            {
                comando.CommandText = "Update  comprobanteitem SET " +
                                       "cen_id= @cen_id, " +
                                       "pro_Codigo=@pro_Codigo, " +
                                       "pro_descripcion=@pro_descripcion, " +
                                       "pro_costo= @pro_costo, " +
                                       "pro_precio=@pro_precio, " +
                                       "cim_cantidad=@cim_cantidad, " +
                                       "cim_descuento=@cim_descuento, " +
                                       "ali_id=@ali_id, " +
                                       "ali_porcentaje=@ali_porcentaje, " +
                                       "cim_neto=@cim_neto, " +
                                       "cim_total = @cim_total, " +
                                       "toc_id=@toc_id, " +
                                       "cim_orden=@cim_orden " +
                                       "WHERE " +
                                       "cim_id = @cim_id; " +
                                       "SELECT @cim_id";
                comando.Parameters.AddWithValue("@cim_id", Id);
            }


            comando.Parameters.AddWithValue("@cen_id", EncabezaoId);
            comando.Parameters.AddWithValue("@pro_Codigo", Codigo);
            comando.Parameters.AddWithValue("@pro_descripcion", Descripcion);
            comando.Parameters.AddWithValue("@pro_costo", Precio);
            comando.Parameters.AddWithValue("@pro_precio", Precio);
            comando.Parameters.AddWithValue("@cim_cantidad", Cantidad);
            comando.Parameters.AddWithValue("@cim_descuento", Descuento);
            comando.Parameters.AddWithValue("@ali_id", AlicuotaId);
            comando.Parameters.AddWithValue("@ali_porcentaje", AlicuotaPorcentaje);
            comando.Parameters.AddWithValue("@cim_neto", Subtotalneto);
            comando.Parameters.AddWithValue("@cim_total", Subtotal);
            if (TareaOrdencompraId != 0)
                comando.Parameters.AddWithValue("@toc_id", TareaOrdencompraId);
            else
                comando.Parameters.AddWithValue("@toc_id", DBNull.Value);
            comando.Parameters.AddWithValue("@cim_orden",Orden);


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
        public List<ComprobanteItem> obtenerFiltrado(ItemFiltro[] itemFiltro,
            ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros)
        {
            List<ComprobanteItem> ret = new List<ComprobanteItem>();
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

            comando.CommandText = "SELECT count(*) FROM ComprobanteItem " + where;
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

                cadenaOrden = Varios.armarCadenaOrden(orden, cadenaOrden, "cim_orden");

                //TODO: Hacer Paginacion

                double rowcount = fin - (inicio - 1);

                comando.CommandText = "  SELECT *   FROM comprobanteitem " + where + " "
                                       + cadenaOrden
                                       + " LIMIT " + (inicio - 1) + ", " + rowcount;





                MySqlDataReader dr = comando.ExecuteReader();


                while (dr.Read())
                {
                    ComprobanteItem bar = new ComprobanteItem();
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

            //if (habilitar)
            //{
            //    if (Activo == false)
            //    {
            //        comando.CommandText = "UPDATE clientes set cli_activo= 'true'  " +
            //                               "where cli_id= @cli_id";
            //        comando.Parameters.AddWithValue("@cli_id", Id);
            //    }
            //}
            //else
            //{
            //    if (Activo == true)
            //    {
            //        comando.CommandText = "UPDATE clientes set cli_activo= 'false'  " +
            //                               "where cli_id= @cli_id";
            //        comando.Parameters.AddWithValue("@cli_id", Id);
            //    }
            //}

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






        public List<ComprobanteItem> obtener(ComprobanteEncabezado comprobanteEncabezado)
        {
            limpiaDatos();

            List<ComprobanteItem> ret = new List<ComprobanteItem>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT * " +
                "FROM comprobanteitem " +
                "WHERE  cen_id = @cen_id " +
                "Order by cim_orden"
                , conexion);

            comando.Parameters.AddWithValue("@cen_id", comprobanteEncabezado.Id);

            comando.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader dr = comando.ExecuteReader();

                while (dr.Read())
                {
                    ComprobanteItem ci = new ComprobanteItem();
                    cargarDatos(ci, dr);
                    ret.Add(ci);
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
            return ret;
        }



        public static void LimpiarItemsEncabezado(int Id)
        {
           
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();


            comando.CommandText = "delete from comprobanteitem " +
                                   "where cen_id= @cen_id";
            comando.Parameters.AddWithValue("@cen_id", Id);

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

                LogManager.Mensaje  UltimoMensaje = GestionErrores.obtenerError(ex);
                UltimoMensaje.cargar(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(),
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(),
                    new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());
                UltimoMensaje.EsError = true;
                UltimoMensaje.StackTrace = ex.StackTrace;
                LogManager.Log.log(UltimoMensaje);
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
