﻿using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using SGECA.DAL.Interfaces;

namespace SGECA.DAL
{
    [Serializable]
    public class VistaComprobantes : LogManager.ISubject, LogManager.IObserver, IVistaComprobantes
    {
        #region declaración de Propiedades
        public int Id { get; set; }
        public string NumeroCompleto { get; set; }
        public LogManager.Mensaje UltimoMensaje { get; set; }
        public string Fecha { get; set; }
        public string IVA { get; set; }
        public string RazonSocial { get; set; }
        public string Neto { get; set; }
        public string Total { get; set; }
        #endregion

        #region declaracion de Variables

        //las variables dbCampoId, dbCampoNombre y dbTabla deberían estar (casi) siempre cargados para
        //facilitar la reutilización de código ya que la mayoria de las tablas tienen ID y nombre.
        //los otros campos los ponemos directamente en el código y no como variable para no complicar 
        //la programación
        private static string dbCampoId = "cen_id";
        private static string dbCampoCae = "cen_cae";
        private static string dbCampoCliente = "Cli_RazonSocial";
        private static string dbCampoNeto = "cen_neto";
        private static string dbCampoIVA = "cen_ivatotal";
        private static string dbCampoTotal = "cen_total";
        private static string dbCampoNumeroCompleto = "cen_numerocompletoconletra";
        private static string dbCampoFecha = "cen_fecha_string";
        private static string dbTabla = "vw_comprobanteencabezado";

        #endregion


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

        public VistaComprobantes()
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
            this.NumeroCompleto = null;
            UltimoMensaje = null;
        }

        /// <summary>
        /// Carga en la instacia actual los atributos del Id pasado por parámetro
        /// </summary>
        public static List<IVistaComprobantes> obtener()
        {
            //limpiaDatos();
            List<IVistaComprobantes> retorno = new List<IVistaComprobantes>();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT * " +
                "FROM " + dbTabla
                , conexion);

            comando.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader dr = comando.ExecuteReader();

                while (dr.Read())
                {
                    VistaComprobantes p = new VistaComprobantes();
                    cargarDatos(p, dr);
                    retorno.Add(p);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                retorno = null;
                LogManager.Mensaje UltimoMensaje = GestionErrores.obtenerError(ex);
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

            return retorno;
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
                "FROM  " + dbTabla + " " +
                "WHERE  " + dbCampoId + " = @" + dbCampoId
                , conexion);

            comando.Parameters.AddWithValue(dbCampoId, id);
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
        /// Carga los datos del datareader en los atributos de una instancia
        /// </summary>
        /// <param name="objeto">instancia donde se cargaran los datos</param>
        /// <param name="dr">DataReader con los datos a cargar</param>
        private static void cargarDatos(VistaComprobantes objeto, MySqlDataReader dr)
        {
            int id = 0;
            int.TryParse(dr[dbCampoId].ToString(), out id);
            objeto.Id = id;

            objeto.NumeroCompleto = dr[dbCampoNumeroCompleto].ToString();

            objeto.Neto = dr[dbCampoNeto].ToString();

            objeto.IVA = dr[dbCampoIVA].ToString();

            objeto.RazonSocial = dr[dbCampoCliente].ToString();

            objeto.Total = dr[dbCampoTotal].ToString();

          
            objeto.Fecha = DateTime.Parse( dr["cen_fecha"].ToString()).ToString("dd/MM/yy HH:mm:ss");





        }

        /// <summary>
        /// Carga en la instacia actual los atributos del código pasado por parámetro
        /// </summary>
        /// <param name="nombre">Codigo a buscar</param>
        public void obtener(string nombre)
        {
            limpiaDatos();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT * " +
                "FROM " + dbTabla + " " +
                "WHERE  " + dbCampoNumeroCompleto + " = @" + dbCampoNumeroCompleto
                , conexion);

            comando.Parameters.AddWithValue(dbCampoNumeroCompleto, nombre);
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


            comando.CommandText = "delete from  " + dbTabla + " " +
                                  "where " + dbCampoId + " = @" + dbCampoId;
            comando.Parameters.AddWithValue(dbCampoId, Id);

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
            throw new NotImplementedException();
            //int retorno = 0;
            //UltimoMensaje = null;
            //DAL.Database db = new Database();
            //MySqlConnection conexion = Database.obtenerConexion(true);

            //MySqlCommand comando = new MySqlCommand();



            //if (Id == 0)
            //    comando.CommandText = "INSERT INTO " + dbTabla + " (" + dbCampoNumeroCompleto + " ) values  " +
            //                           "(@ " + dbCampoNumeroCompleto + "); " +
            //                           " SELECT  Last_insert_id()";
            //else
            //{
            //    comando.CommandText = "UPDATE " + dbTabla + " set " + dbCampoNumeroCompleto + "= @" + dbCampoNumeroCompleto + " ,  " +
            //                           dbCampoId + " = @" + dbCampoId + " " +
            //                           "where " + dbCampoId + "= @" + dbCampoId + "; SELECT @" + dbCampoId;
            //    comando.Parameters.AddWithValue(dbCampoId, Id);
            //}

            //comando.Parameters.AddWithValue(dbCampoNumeroCompleto, NumeroCompleto);
            //comando.Parameters.AddWithValue(dbCampoId, Id);
            //comando.Transaction = Database.obtenerTransaccion();
            //comando.Connection = conexion;

            //try
            //{
            //    if (Database.obtenerTransaccion() == null)
            //        conexion.Open();
            //    var valor = comando.ExecuteScalar();

            //    if (valor == null && Id != 0)
            //        throw new Exception("El registro que está intentando modificar ya no existe en la base de datos");
            //    else
            //    {

            //        Id = int.Parse(valor.ToString());

            //        retorno = Id;
            //    }
            //}
            //catch (Exception ex)
            //{

            //    UltimoMensaje = GestionErrores.obtenerError(ex);
            //    UltimoMensaje.cargar(
            //        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(),
            //        System.Reflection.MethodBase.GetCurrentMethod().ToString(),
            //        new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());
            //    UltimoMensaje.StackTrace = ex.StackTrace;
            //    UltimoMensaje.EsError = true;
            //    Notify(UltimoMensaje);
            //}
            //finally
            //{
            //    comando.Parameters.Clear();
            //    if (Database.obtenerTransaccion() == null)
            //        if (conexion.State != ConnectionState.Closed)
            //            conexion.Close();
            //}

            //return retorno;
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
        public List<VistaComprobantes> obtenerFiltrado(ItemFiltro[] itemFiltro,
            ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros)
        {
            List<VistaComprobantes> ret = new List<VistaComprobantes>();
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

                cadenaOrden = Varios.armarCadenaOrden(orden, cadenaOrden, dbCampoNumeroCompleto);

                //TODO: Hacer Paginacion

                double rowcount = fin - (inicio - 1);

                comando.CommandText = "  SELECT *   FROM " + dbTabla + " " + where + " "
                                       + cadenaOrden
                                       + " LIMIT " + (inicio - 1) + ", " + rowcount;





                MySqlDataReader dr = comando.ExecuteReader();


                while (dr.Read())
                {
                    VistaComprobantes bar = new VistaComprobantes();
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



        public string ToString(bool conId)
        {
            return Id + "- " + NumeroCompleto;
        }


    }
}

