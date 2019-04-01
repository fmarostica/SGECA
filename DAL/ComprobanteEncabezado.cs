using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace SGECA.DAL
{
    [Serializable]
    public class ComprobanteEncabezado : LogManager.ISubject, LogManager.IObserver
    {
        public int Id { get; set; }
        public int cen_numero { get; set; }
        public decimal cen_neto { get; set; }
        public decimal cen_iva_total { get; set; }
        public decimal cen_IVA01 { get; set; } //IVA 21%
        public decimal cen_IVA01porc { get; set; } //IVA 21%
        public decimal cen_IVA01neto { get; set; } //IVA 21%
        public decimal cen_IVA02 { get; set; } //IVA 10.5%
        public decimal cen_IVA02porc { get; set; } //IVA 10.5%
        public decimal cen_IVA02neto { get; set; } //IVA 10.5%
        public decimal cen_IVA03 { get; set; } //IVA 27%
        public decimal cen_IVA03porc { get; set; } //IVA 27%
        public decimal cen_IVA03neto { get; set; } //IVA 27%
        public decimal cen_IVA04 { get; set; } //IVA 0%
        public decimal cen_IVA04porc { get; set; } //IVA 0%
        public decimal cen_IVA04neto { get; set; } //IVA 0%
        public decimal cen_total { get; set; }
        public DateTime cen_fecha { get; set; }



        //Cliente
        public int ClienteID { get; set; }
        public string ClienteRazonSocial { get; set; }
        public string Cli_Codigo { get; set; }
        public long Cli_Cuit { get; set; }
        public string Cli_IngresosBrutos { get; set; }
        public string Cli_Direccion { get; set; }
        public string Cli_Provincia { get; set; }
        public string Cli_Localidad { get; set; }


        //Tipo Responsable
        public string tre_Codigo { get; set; }
        public string tre_Nombre { get; set; }

        //Lista de precio
        public int lpr_id { get; set; }
        public string lpr_nombre { get; set; }

        //Concepto Venta
        public string cvt_codigo { get; set; }
        public string cvt_nombre { get; set; }

        //Tipo de documento
        public string tdo_codigo { get; set; }
        public string tdo_nombre { get; set; }

        //Punto de venta
        public string pvt_codigo { get; set; }


        ////Relacion con antenas
        //public string toc_orden { get; set; }

        //condicion venta
        public string cva_codigo { get; set; }
        public string cva_nombre { get; set; }

        public string tco_codigo { get; set; }

        //AFIP CAE

        public string cen_Cae { get; set; }
        public DateTime cen_CaeFechaVencimiento { get; set; }
        public string cen_AfipReq { get; set; }
        public string cen_AfipRes { get; set; }

        public string cen_CuitContribuyente { get; set; }

        public string cen_numeroCompleto
        {
            get
            {
                TipoComprobante t = new TipoComprobante();
                t.obtener(tco_codigo);
                if (cen_numero == 0)
                    return "";
                return t.Nemonico + " " + pvt_codigo.PadLeft(4, '0') + "-" + cen_numero.ToString().PadLeft(8, '0') + "/" + t.Letra;
            }
        }

        public string cen_cae_i2o5
        {
            get
            {

                string micae = cen_CuitContribuyente +
                                cvt_codigo.PadLeft(2, '0') +
                                pvt_codigo.PadLeft(4, '0') +
                                cen_Cae +
                                cen_CaeFechaVencimiento.ToString("yyyyMMdd")
                                ;

                int totalpares = 0, totalimpares = 0, digito = 0;
                long x = 0;
                for (int j = 0; j < micae.Length; j += 2)
                {
                    totalimpares += int.Parse(micae.Substring(j, 1));
                }

                totalimpares = totalimpares * 3;

                for (int k = 1; k < micae.Length; k += 2)
                {
                    totalpares += int.Parse(micae.Substring(k, 1));
                }
                x = totalimpares + totalpares;

                while (((x / 10) * 10) != x)
                {
                    x++;
                    digito++;
                }

                micae += digito;

                return stringAI2of5(micae, false);
            }


        }

        private string stringAI2of5(string cadena, bool calculaDigitoVerificador)
        {
            char lcStart, lcStop, lcCheck;
            int lnLong, lnSum, lnCount, lnAux, lnI;
            string lcRet, lcCar;
            lcStart = (char)(40);
            lcStop = (char)(41);
            lcRet = cadena.Trim();
            // Genero dígito de control
            lnLong = lcRet.Length;
            lnSum = 0;
            lnCount = 1;

            if (calculaDigitoVerificador)
            {
                for (int i = (lnLong - 1); i > 0; i--)
                {
                    int multiplicador = 1;
                    if ((lnCount % 2) == 0)
                        multiplicador = 3;
                    lnSum = lnSum + int.Parse(lcRet.Substring(i, 1)) * multiplicador;
                    lnCount++;
                }

                lnAux = lnSum % 10;

                if (lnAux != 0)
                    lcRet += (10 - lnAux);
                else
                    lcRet += "0";
            }
            lnLong = lcRet.Length;
            //La longitud debe ser par
            if (lnLong % 2 != 0)
            {
                lcRet = "0" + lcRet;
                lnLong = lcRet.Length;
            }
            //Convierto los pares a caracteres
            lcCar = "";

            for (lnI = 0; lnI < lnLong; lnI += 2)
            {

                //FOR lnI = 1 TO lnLong STEP 2
                if (int.Parse(lcRet.Substring(lnI, 2)) < 50)
                    lcCar = lcCar + (char)(int.Parse(lcRet.Substring(lnI, 2)) + 48);
                else
                    lcCar = lcCar + (char)(int.Parse(lcRet.Substring(lnI, 2)) + 142);

            }
            ///Armo código
            lcRet = lcStart + lcCar + lcStop;
            return lcRet;
        }
        public List<ComprobanteItem> itemsComprobante = new List<ComprobanteItem>();

        public LogManager.Mensaje UltimoMensaje { get; set; }

        public ComprobanteEncabezado()
        {
            //suscripción al Log
            Subscribe(new LogManager.Log());
        }
        private void limpiaDatos()
        {
            Id = 0;
            cen_numero = 0;
            cen_neto = 0;
            cen_iva_total = 0;
            cen_IVA01 = 0;
            cen_IVA01porc = 0;
            cen_IVA01neto = 0;
            cen_IVA02 = 0;
            cen_IVA02porc = 0;
            cen_IVA02neto = 0;
            cen_IVA03 = 0;
            cen_IVA03porc = 0;
            cen_IVA03neto = 0;
            cen_IVA04 = 0;
            cen_IVA04porc = 0;
            cen_IVA04neto = 0;
            cen_total = 0;
            cen_fecha = DateTime.MinValue;
            ClienteID = 0;
            ClienteRazonSocial = null;
            Cli_Codigo = null;
            Cli_Cuit = 0;
            Cli_IngresosBrutos = null;
            Cli_Direccion = null;
            Cli_Provincia = null;
            Cli_Localidad = null;
            tre_Codigo = null;
            tre_Nombre = null;
            lpr_id = 0;
            lpr_nombre = null;
            cvt_codigo = null;
            cvt_nombre = null;
            tdo_codigo = null;
            tdo_nombre = null;
            pvt_codigo = null;
            //toc_orden = null;
            cva_codigo = null;
            cva_nombre = null;
            tco_codigo = null;
            cen_Cae = null;
            cen_CaeFechaVencimiento = DateTime.MinValue;
            cen_AfipReq = null;
            cen_AfipRes = null;
            cen_CuitContribuyente = null;
        }


        public void obtenerFechasTope(out DateTime minima, out DateTime maxima)
        {
            limpiaDatos();

            minima = new DateTime(2014, 01, 01);
            maxima = DateTime.Now.AddYears(1);

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT MIN(cen_fecha) AS minimo, MAX(cen_fecha) AS maximo  FROM comprobanteencabezado "
                , conexion);

            comando.Transaction = Database.obtenerTransaccion();

            List<ComprobanteEncabezado> comprobantes = new List<ComprobanteEncabezado>();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader dr = comando.ExecuteReader();


                //TODO: Este metodo no está bien hecho, hay que utilizar cargar datos!!!!!

                if (dr.Read())
                {
                    DateTime dtemp = new DateTime(2014, 01, 01);
                    if (DateTime.TryParse(dr["minimo"].ToString(), out dtemp))
                        minima = dtemp;

                    dtemp = DateTime.Now.AddYears(1);
                    if (DateTime.TryParse(dr["maximo"].ToString(), out dtemp))
                        maxima = dtemp;

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


        public List<ComprobanteEncabezado> obtener()
        {
            limpiaDatos();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT * " +
                "FROM comprobanteencabezado "
                , conexion);

            comando.Transaction = Database.obtenerTransaccion();

            List<ComprobanteEncabezado> comprobantes = new List<ComprobanteEncabezado>();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                MySqlDataReader dr = comando.ExecuteReader();


                //TODO: Este metodo no está bien hecho, hay que utilizar cargar datos!!!!!

                while (dr.Read())
                {
                    ComprobanteEncabezado st = new ComprobanteEncabezado();
                    cargarDatos(st, dr);



                    comprobantes.Add(st);

                }
                return comprobantes;
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
                return comprobantes;
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
        private void cargarDatos(ComprobanteEncabezado objeto, MySqlDataReader dr)
        {
            int intAux = 0;
            DateTime dateAux = DateTime.MinValue;

            int.TryParse(dr["cen_id"].ToString(), out intAux);
            objeto.Id = intAux;



            //Tipo Responsable
            objeto.tre_Codigo = dr["tre_Codigo"].ToString();
            objeto.tre_Nombre = dr["tre_Nombre"].ToString();

            //Lista de precio
            objeto.lpr_nombre = dr["lpr_nombre"].ToString();

            intAux = 0;
            int.TryParse(dr["lpr_id"].ToString(), out intAux);
            objeto.lpr_id = intAux;


            //Concepto Venta
            objeto.cvt_codigo = dr["cvt_codigo"].ToString();
            objeto.cvt_nombre = dr["cvt_nombre"].ToString();

            //Tipo de documento
            objeto.tdo_codigo = dr["tdo_codigo"].ToString();
            objeto.tdo_nombre = dr["tdo_nombre"].ToString();

            //relacion con antenas
            //objeto.toc_orden = dr["toc_orden"].ToString();

            //Punto de venta
            objeto.pvt_codigo = dr["pvt_codigo"].ToString();

            //condicion venta
            objeto.cva_codigo = dr["cva_codigo"].ToString();
            objeto.cva_nombre = dr["cva_nombre"].ToString();

            objeto.tco_codigo = dr["tco_codigo"].ToString();

            //AFIP CAE
            objeto.cen_Cae = dr["cen_Cae"].ToString();

            dateAux = DateTime.MinValue;
            DateTime.TryParse(dr["cen_CaeFechaVencimiento"].ToString(), out dateAux);
            objeto.cen_CaeFechaVencimiento = dateAux;


            objeto.cen_AfipReq = dr["cen_AfipReq"].ToString();
            objeto.cen_AfipRes = dr["cen_AfipRes"].ToString();


            //objeto.cen_CuitContribuyente= dr["cen_CuitContribuyente"].ToString();


            dateAux = DateTime.MinValue;
            DateTime.TryParse(dr["cen_fecha"].ToString(), out dateAux);
            objeto.cen_fecha = dateAux;

            intAux = 0;
            int.TryParse(dr["cen_numero"].ToString(), out intAux);
            objeto.cen_numero = intAux;


            intAux = 0;
            int.TryParse(dr["cli_id"].ToString(), out intAux);
            objeto.ClienteID = intAux;
            objeto.ClienteRazonSocial = dr["cli_RazonSocial"].ToString();
            objeto.Cli_Codigo = dr["Cli_Codigo"].ToString();

            long lngAux = 0;
            long.TryParse(dr["Cli_Cuit"].ToString(), out lngAux);
            objeto.Cli_Cuit = lngAux;


            objeto.Cli_IngresosBrutos = dr["cli_IngresosBrutos"].ToString(); ;

            objeto.Cli_Direccion = dr["Cli_Direccion"].ToString();
            objeto.Cli_Provincia = dr["Cli_Provincia"].ToString();
            objeto.Cli_Localidad = dr["Cli_Localidad"].ToString();


            decimal aux = 0;
            Decimal.TryParse(dr["cen_neto"].ToString(), out aux);
            objeto.cen_neto = aux;

            Decimal.TryParse(dr["cen_total"].ToString(), out aux);
            objeto.cen_total = aux;

            objeto.tco_codigo = dr["tco_codigo"].ToString();
            objeto.pvt_codigo = dr["pvt_codigo"].ToString();


            //IVA        
            aux = 0;
            Decimal.TryParse(dr["cen_IVA01"].ToString(), out aux);
            objeto.cen_IVA01 = aux;

            aux = 0;
            Decimal.TryParse(dr["cen_IVA01porc"].ToString(), out aux);
            objeto.cen_IVA01porc = aux;

            aux = 0;
            Decimal.TryParse(dr["cen_IVA01neto"].ToString(), out aux);
            objeto.cen_IVA01neto = aux;

            aux = 0;
            Decimal.TryParse(dr["cen_IVA02"].ToString(), out aux);
            objeto.cen_IVA02 = aux;

            aux = 0;
            Decimal.TryParse(dr["cen_IVA02porc"].ToString(), out aux);
            objeto.cen_IVA02porc = aux;

            aux = 0;
            Decimal.TryParse(dr["cen_IVA02neto"].ToString(), out aux);
            objeto.cen_IVA02neto = aux;

            aux = 0;
            Decimal.TryParse(dr["cen_IVA03"].ToString(), out aux);
            objeto.cen_IVA03 = aux;

            aux = 0;
            Decimal.TryParse(dr["cen_IVA03porc"].ToString(), out aux);
            objeto.cen_IVA03porc = aux;

            aux = 0;
            Decimal.TryParse(dr["cen_IVA03neto"].ToString(), out aux);
            objeto.cen_IVA03neto = aux;

            aux = 0;
            Decimal.TryParse(dr["cen_IVA04"].ToString(), out aux);
            objeto.cen_IVA04 = aux;

            aux = 0;
            Decimal.TryParse(dr["cen_IVA04porc"].ToString(), out aux);
            objeto.cen_IVA04porc = aux;

            aux = 0;
            Decimal.TryParse(dr["cen_IVA04neto"].ToString(), out aux);
            objeto.cen_IVA04neto = aux;


            ComprobanteItem ci = new ComprobanteItem();
            itemsComprobante = ci.obtener(this);


        }

        /// <summary>
        /// Carga en la instacia actual los atributos del código pasado por parámetro
        /// </summary>
        /// <param name="codigo">Codigo a buscar</param>
        public void obtener(int puntoVenta, int numero, string tipoComprobante)
        {
            limpiaDatos();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT * " +
                "FROM comprobanteencabezado " +
                "WHERE  tco_codigo = @tco_codigo and  " +
                " pvt_codigo = @pvt_codigo and  " +
                " cen_numero = @cen_numero  "
                , conexion);

            comando.Parameters.AddWithValue("@tco_codigo", tipoComprobante);
            comando.Parameters.AddWithValue("@pvt_codigo", puntoVenta);
            comando.Parameters.AddWithValue("@cen_numero", numero);
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
        /// Carga en la instacia actual los atributos del código pasado por parámetro
        /// </summary>
        /// <param name="codigo">Codigo a buscar</param>
        public void obtener(int Id)
        {
            limpiaDatos();

            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT * " +
                "FROM comprobanteencabezado " +
                "WHERE  cen_id = @cen_id "
                , conexion);

            comando.Parameters.AddWithValue("@cen_id", Id);

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
        public void eliminar(int id)
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
        /// Setea a null los datos correspondientes al ID de la instancia actual
        /// </summary>
        public void imprimir()
        {

            UltimoMensaje = null;
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand();


            comando.CommandText = "UPDATE comprobanteencabezado set cen_estadoimpresion = null, cen_fechaimpresion = null, " +
                                  "cen_cae_i2o5  = @cen_cae_i2o5 " +
                                   "WHERE cen_id= @cen_id";

            comando.Parameters.AddWithValue("@cen_id", Id);
            comando.Parameters.AddWithValue("@cen_cae_i2o5", cen_cae_i2o5);

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
                comando.CommandText = "INSERT INTO comprobanteencabezado (" +
                "tco_codigo, " +
                "pvt_codigo, " +
                "cen_numero, " +
                "cen_fecha, " +
                "tre_codigo, " +
                "tre_nombre, " +
                "cli_id, " +
                "cli_Codigo, " +
                "cli_RazonSocial, " +
                "cli_CUIT, " +
                "cli_IngresosBrutos, " +
                "cli_direccion, " +
                "cli_localidad, " +
                "cli_provincia, " +
                "lpr_Id, " +
                "lpr_Nombre, " +
                "cen_neto, " +
                "cen_IVA01, " +
                "cen_IVA01porc, " +
                "cen_IVA01neto, " +
                "cen_IVA02, " +
                "cen_IVA02porc, " +
                "cen_IVA02neto, " +
                "cen_IVA03, " +
                "cen_IVA03porc, " +
                "cen_IVA03neto, " +
                "cen_IVA04, " +
                "cen_IVA04porc, " +
                "cen_IVA04neto, " +
                "cen_total, " +
                "cvt_codigo, " +
                "cvt_nombre, " +
                "tdo_codigo, " +
                "tdo_nombre, " +
                "cva_codigo, " +
                "cva_nombre, " +
                "cen_cae, " +
                "cen_caefechavencimiento, " +
                "cen_afipreq, " +
                "cen_afipres, " +
                "cen_cae_i2o5, " +
                "gim_id" +
                ") values  (" +

                "@tco_codigo, " +
                "@pvt_codigo, " +
                "@cen_numero, " +
                "@cen_fecha, " +
                "@tre_codigo, " +
                "@tre_nombre, " +
                "@cli_id, " +
                "@cli_Codigo, " +
                "@cli_RazonSocial, " +
                "@cli_CUIT, " +
                "@cli_IngresosBrutos, " +
                "@cli_direccion, " +
                "@cli_localidad, " +
                "@cli_provincia, " +
                "@lpr_Id, " +
                "@lpr_Nombre, " +
                "@cen_neto, " +
                "@cen_IVA01, " +
                "@cen_IVA01porc, " +
                "@cen_IVA01neto, " +
                "@cen_IVA02, " +
                "@cen_IVA02porc, " +
                "@cen_IVA02neto, " +
                "@cen_IVA03, " +
                "@cen_IVA03porc, " +
                "@cen_IVA03neto, " +
                "@cen_IVA04, " +
                "@cen_IVA04porc, " +
                "@cen_IVA04neto, " +
                "@cen_total, " +
                "@cvt_codigo, " +
                "@cvt_nombre, " +
                "@tdo_codigo, " +
                "@tdo_nombre, " +
                "@cva_codigo, " +
                "@cva_nombre, " +
                "@cen_cae, " +
                "@cen_caefechavencimiento, " +
                "@cen_afipreq, " +
                "@cen_afipres, " +
                "@cen_cae_i2o5," +
                "@gim_id" +
                "); " +
                " SELECT  Last_insert_id()";


            }
            else
            {
                comando.CommandText = "UPDATE comprobanteencabezado set " +
                "tco_codigo = @tco_codigo, " +
                "pvt_codigo = @pvt_codigo, " +
                "cen_numero = @cen_numero,  " +
                "cen_fecha = @cen_fecha,  " +
                "tre_codigo = @tre_codigo,  " +
                "tre_nombre = @tre_nombre, " +
                "cli_id = @cli_id,  " +
                "cli_Codigo = @cli_Codigo, " +
                "cli_RazonSocial = @cli_RazonSocial, " +
                "cli_CUIT =  @cli_CUIT, " +
                "cli_IngresosBrutos = @cli_IngresosBrutos, " +
                "cli_direccion = @cli_direccion, " +
                "cli_localidad = @cli_localidad, " +
                "cli_provincia =  @cli_provincia, " +
                "lpr_Id = @lpr_Id, " +
                "lpr_Nombre = @lpr_Nombre, " +
                "cen_neto = @cen_neto, " +
                "cen_IVA01 = @cen_IVA01, " +
                "cen_IVA01porc =  @cen_IVA01porc, " +
                "cen_IVA01neto =  @cen_IVA01neto, " +
                "cen_IVA02 = @cen_IVA02, " +
                "cen_IVA02porc =  @cen_IVA02porc, " +
                "cen_IVA02neto = @cen_IVA02neto, " +
                "cen_IVA03 = @cen_IVA03, " +
                "cen_IVA03porc = @cen_IVA03porc, " +
                "cen_IVA03neto = @cen_IVA03neto, " +
                "cen_IVA04 = @cen_IVA04, " +
                "cen_IVA04porc = @cen_IVA04porc, " +
                "cen_IVA04neto = @cen_IVA04neto, " +
                "cen_total = @cen_total, " +
                "cvt_codigo = @cvt_codigo, " +
                "cvt_nombre = @cvt_nombre, " +
                "tdo_codigo = @tdo_codigo, " +
                "tdo_nombre = @tdo_nombre, " +
                "cva_codigo = @cva_codigo, " +
                "cva_nombre = @cva_nombre, " +
                "cen_cae = @cen_cae, " +
                "cen_caefechavencimiento =  @cen_caefechavencimiento, " +
                "cen_afipreq = @cen_afipreq, " +
                "cen_afipres =   @cen_afipres, " +
                "cen_cae_i2o5 = @cen_cae_i2o5, " +
                "gim_id = @gim_id " +

                "WHERE  " +
                "cen_id = @cen_id;" +
                " SELECT @cen_id";

                comando.Parameters.AddWithValue("@cen_id", Id);
            }


            comando.Parameters.AddWithValue("tco_codigo", tco_codigo);
            comando.Parameters.AddWithValue("pvt_codigo", pvt_codigo);
            comando.Parameters.AddWithValue("cen_numero", cen_numero);
            comando.Parameters.AddWithValue("cen_fecha", cen_fecha);
            comando.Parameters.AddWithValue("tre_codigo", tre_Codigo);
            comando.Parameters.AddWithValue("tre_nombre", tre_Nombre);
            comando.Parameters.AddWithValue("cli_id", ClienteID);
            comando.Parameters.AddWithValue("cli_Codigo", Cli_Codigo);
            comando.Parameters.AddWithValue("cli_RazonSocial", ClienteRazonSocial);
            if (Cli_Cuit != 0)
                comando.Parameters.AddWithValue("cli_CUIT", Cli_Cuit);
            else
                comando.Parameters.AddWithValue("cli_CUIT", DBNull.Value);

            comando.Parameters.AddWithValue("cli_IngresosBrutos", Cli_IngresosBrutos);
            comando.Parameters.AddWithValue("cli_direccion", Cli_Direccion);
            comando.Parameters.AddWithValue("cli_localidad", Cli_Localidad);
            comando.Parameters.AddWithValue("cli_provincia", Cli_Provincia);
            comando.Parameters.AddWithValue("lpr_Id", lpr_id);
            comando.Parameters.AddWithValue("lpr_Nombre", lpr_nombre);
            comando.Parameters.AddWithValue("cen_neto", cen_neto);
            comando.Parameters.AddWithValue("cen_IVA01", cen_IVA01);
            comando.Parameters.AddWithValue("cen_IVA01porc", cen_IVA01porc);
            comando.Parameters.AddWithValue("cen_IVA01neto", cen_IVA01neto);
            comando.Parameters.AddWithValue("cen_IVA02", cen_IVA02);
            comando.Parameters.AddWithValue("cen_IVA02porc", cen_IVA02porc);
            comando.Parameters.AddWithValue("cen_IVA02neto", cen_IVA02neto);
            comando.Parameters.AddWithValue("cen_IVA03", cen_IVA03);
            comando.Parameters.AddWithValue("cen_IVA03porc", cen_IVA03porc);
            comando.Parameters.AddWithValue("cen_IVA03neto", cen_IVA03neto);
            comando.Parameters.AddWithValue("cen_IVA04", cen_IVA04);
            comando.Parameters.AddWithValue("cen_IVA04porc", cen_IVA04porc);
            comando.Parameters.AddWithValue("cen_IVA04neto", cen_IVA04neto);
            comando.Parameters.AddWithValue("cen_total", cen_total);
            comando.Parameters.AddWithValue("cvt_codigo", cvt_codigo);
            comando.Parameters.AddWithValue("cvt_nombre", cvt_nombre);
            comando.Parameters.AddWithValue("tdo_codigo", tdo_codigo);
            comando.Parameters.AddWithValue("tdo_nombre", tdo_nombre);
            comando.Parameters.AddWithValue("cva_codigo", cva_codigo);
            comando.Parameters.AddWithValue("cva_nombre", cva_nombre);
            comando.Parameters.AddWithValue("cen_cae", cen_Cae);
            comando.Parameters.AddWithValue("cen_afipreq", cen_AfipReq);

            if (cen_Cae == null || cen_Cae.Trim() == "")
            {
                comando.Parameters.AddWithValue("cen_caefechavencimiento", DBNull.Value);
                comando.Parameters.AddWithValue("cen_afipres", DBNull.Value);
                comando.Parameters.AddWithValue("cen_cae_i2o5", DBNull.Value);

            }
            else
            {
                comando.Parameters.AddWithValue("cen_caefechavencimiento", cen_CaeFechaVencimiento);
                comando.Parameters.AddWithValue("cen_afipres", cen_AfipRes);
                comando.Parameters.AddWithValue("cen_cae_i2o5", cen_cae_i2o5);
            }

            comando.Parameters.AddWithValue("gim_id", 1);

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
        public List<Cliente> obtenerFiltrado(ItemFiltro[] itemFiltro,
            ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros)
        {
            List<Cliente> ret = new List<Cliente>();
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

            comando.CommandText = "SELECT count(*) FROM clientes " + where;
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

                cadenaOrden = Varios.armarCadenaOrden(orden, cadenaOrden, "cli_cliente");

                //TODO: Hacer Paginacion

                double rowcount = fin - (inicio - 1);

                comando.CommandText = "  SELECT *   FROM clientes " + where + " "
                                       + cadenaOrden
                                       + " LIMIT " + (inicio - 1) + ", " + rowcount;





                MySqlDataReader dr = comando.ExecuteReader();


                while (dr.Read())
                {
                    Cliente bar = new Cliente();
                    bar.Subscribe(this);

                    //cargarDatos(bar, dr);


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


        public int ali_id(decimal alicuota)
        {
            int retorno = 0;
            MySqlConnection conexion = Database.obtenerConexion(true);

            MySqlCommand comando = new MySqlCommand(
                "SELECT ali_id " +
                "FROM alicuota " +
                "WHERE  ali_porcentaje = @alicuota "
                , conexion);

            comando.Parameters.AddWithValue("@alicuota", alicuota);
            comando.Transaction = Database.obtenerTransaccion();

            try
            {
                if (Database.obtenerTransaccion() == null)
                    conexion.Open();
                retorno = int.Parse(comando.ExecuteScalar().ToString());

                return retorno;
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
                return 0;
            }
            finally
            {
                comando.Parameters.Clear();
                if (Database.obtenerTransaccion() == null)
                    if (conexion.State != ConnectionState.Closed)
                        conexion.Close();
            }
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




        public int cen_ivaTotal { get; set; }

        public void LimpiarItems()
        {
            throw new NotImplementedException();
        }
    }
}
