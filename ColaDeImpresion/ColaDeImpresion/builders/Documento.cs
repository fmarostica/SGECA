using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SGECA.ColaDeImpresion.builders
{
    public class Documento
    {
        public string classId = "D";


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
        }


        // Unsubscribe

        #endregion

        /// <summary>
        /// Utilizado para quitar un documento por impresión correcta
        /// </summary>
        /// <param name="idDocumento">ClassId + _ + Id del registro a quitar </param>
        public void quitarDocumento(string idDocumento)
        {
            if (!idDocumento.StartsWith(classId + "_"))
                return;

            int id = int.Parse(idDocumento.Replace(classId + "_", ""));

            //int id = int.Parse(clbTrabajosPendientes.SelectedValue.ToString());
            MySqlConnection cone = new MySqlConnection(ConfigurationManager.ConnectionStrings["ce"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand("UPDATE comprobanteencabezado SET cen_estadoimpresion = 1, " +
                                                "cen_fechaimpresion = NOW() " +
                                                "WHERE cen_id=@cen_id", cone);
            cmd.Parameters.AddWithValue("@cen_id", id);
            try
            {
                cone.Open();
                cmd.ExecuteNonQuery();

                LogManager.Mensaje m = new LogManager.Mensaje("SGECA.ColaDeImpresion.builders.Documento",
                                           "quitarDocumento",
                                           0,
                                           "El documento se quito correctamente de la cola de impresión.",
                                           "",
                                           "",
                                           false,
                                           LogManager.EMensaje.Informativo,
                                           null);

                Notify(m);
                //refrescarLista();
            }
            catch (Exception Exception)
            {

                LogManager.Mensaje m = new LogManager.Mensaje("SGECA.ColaDeImpresion.builders.Documento",
                                            "quitarDocumento",
                                            0,
                                            "Error al intentar quitar el documento de la cola de impresión.",
                                            Exception.Message,
                                            "",
                                            true,
                                            LogManager.EMensaje.Critico,
                                            Exception.StackTrace);

                Notify(m);
            }
            finally
            {
                if (cone.State != ConnectionState.Closed)
                    cone.Close();
            }
        }



        public List<object> obtener()
        {

            List<object> trabajosPendientes = new List<object>();
            MySqlConnection cone = new MySqlConnection(ConfigurationManager.ConnectionStrings["ce"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand("select * from vw_comprobanteencabezado where cen_estadoimpresion is null and cen_numero >0", cone);

            try
            {
                cone.Open();
                MySqlDataReader dr = cmd.ExecuteReader();


                while (dr.Read())
                {
                    var item = new
                    {
                        id = classId + "_" + dr["cen_id"].ToString(),
                        desc = dr["tco_descripcion"].ToString() + " - " +
                              dr["cen_numerocompleto"].ToString() + " - " +
                              dr["cli_razonsocial"].ToString() + " --> @" +
                               dr["gim_impresora"].ToString(),
                        impresora = dr["gim_impresora"].ToString(),
                        copias = dr["gim_copias"].ToString()
                    };
                    trabajosPendientes.Add(item);
                }

                LogManager.Mensaje m = new LogManager.Mensaje("SGECA.ColaDeImpresion.builders.Documento",
                                         "obtener",
                                         0,
                                         "Obtención correcta de documentos a imprimir.",
                                         "",
                                         "",
                                         false,
                                         LogManager.EMensaje.Informativo,
                                         null);

                Notify(m);


            }
            catch (Exception Exception)
            {

                LogManager.Mensaje m = new LogManager.Mensaje("SGECA.ColaDeImpresion.builders.Documento",
                                            "obtener",
                                            0,
                                            "Error al intentar obtener información desde la base de datos.",
                                            Exception.Message,
                                            "",
                                            true,
                                            LogManager.EMensaje.Critico,
                                            Exception.StackTrace);

                Notify(m);
            }
            finally
            {
                if (cone.State != ConnectionState.Closed)
                    cone.Close();
            }
            return trabajosPendientes;
        }

        /// <summary>
        /// Utilizado para quitar un documento en forma manual
        /// </summary>
        /// <param name="idDocumento">ClassId + _ + Id del registro a eliminar </param>

        public void eliminar(string idDocumento)
        {
            if (!idDocumento.StartsWith(classId + "_"))
                return;

            int id = int.Parse(idDocumento.Replace(classId + "_", ""));


            MySqlConnection cone = new MySqlConnection(ConfigurationManager.ConnectionStrings["ce"].ConnectionString);
            try
            {


                MySqlCommand cmd = new MySqlCommand("UPDATE comprobanteencabezado SET cen_estadoimpresion = 2, " +
                                                    "cen_fechaimpresion = NOW() " +
                                                    "WHERE cen_id=@cen_id", cone);
                cmd.Parameters.AddWithValue("@cen_id", id);
                cone.Open();
                cmd.ExecuteNonQuery();

                LogManager.Mensaje m = new LogManager.Mensaje("SGECA.ColaDeImpresion.builders.Documento",
                                         "eliminar",
                                         0,
                                         "El documento se quito correctamente de la cola de impresión.",
                                         "",
                                         "",
                                         false,
                                         LogManager.EMensaje.Informativo,
                                         null);

                Notify(m);
            }
            catch (Exception Exception)
            {

                LogManager.Mensaje m = new LogManager.Mensaje("SGECA.ColaDeImpresion.builders.Documento",
                                            "eliminar",
                                            0,
                                            "Error al intentar quitar documento de la cola de impresión.",
                                            Exception.Message,
                                            "",
                                            true,
                                            LogManager.EMensaje.Critico,
                                            Exception.StackTrace);

                Notify(m);
            }
            finally
            {
                if (cone.State != ConnectionState.Closed)
                    cone.Close();
            }

        }

        public string prepararTexto(string idDocumento, string appPath)
        {
            if (!idDocumento.StartsWith(classId + "_"))
                return "";

            int id = int.Parse(idDocumento.Replace(classId + "_", ""));

            try
            {
                Dictionary<string, CamposImpresion> camposImpresionPorCodigo;
                Dictionary<string, CamposImpresion> camposImpresionPorCampo = obtenerCamposImpresion(out camposImpresionPorCodigo);


                if (camposImpresionPorCodigo == null || camposImpresionPorCampo == null)
                {
                    LogManager.Mensaje m =
                        new LogManager.Mensaje("SGECA.ColaDeImpresion",
                        "prepararTexto(int)",
                        0,
                        "No puedo imprimir un documento si no hay campos definidos. Id: " + id,
                        "",
                        "",
                        true,
                        LogManager.EMensaje.Advertencia,
                       "");

                    Notify(m);

                    return "";
                }

                Dictionary<string, string> valEnc = obtenerValoresEncabezado(camposImpresionPorCampo, id);

                if (valEnc == null)
                {
                    LogManager.Mensaje m =
                        new LogManager.Mensaje("SGECA.ColaDeImpresion",
                        "prepararTexto(int)",
                        0,
                        "No puedo imprimir un documento si no hay valores de encabezado. Id: " + id,
                        "",
                        "",
                        true,
                        LogManager.EMensaje.Advertencia,
                       "");

                    Notify(m);

                    return "";
                }

                List<Dictionary<string, string>> valItems = obtenerValoresItems(camposImpresionPorCampo, id);

                if (valItems == null)
                {
                    LogManager.Mensaje m =
                        new LogManager.Mensaje("SGECA.ColaDeImpresion",
                        "prepararTexto(int)",
                        0,
                        "No puedo imprimir un documento si no hay valores de items. Id: " + id,
                        "",
                        "",
                        true,
                        LogManager.EMensaje.Advertencia,
                       "");

                    Notify(m);

                    return "";
                }

                RichTextBoxPrintCtrl richTextBoxPrintCtrl = new RichTextBoxPrintCtrl();

                richTextBoxPrintCtrl.LoadFile(appPath + @"\Templates\1.rtf");

                string texto = richTextBoxPrintCtrl.Rtf;


                int position = 0, posicionAnterior = 0;

                while (texto.IndexOf('#') > 0)
                {


                    position = texto.IndexOf('#');

                    int indiceCambioRegistroItem = texto.Substring(posicionAnterior, position - posicionAnterior).IndexOf('~');
                    if (indiceCambioRegistroItem > 0)
                    {
                        StringBuilder sb = new StringBuilder(texto);
                        sb[posicionAnterior + indiceCambioRegistroItem] = ' ';
                        texto = sb.ToString();
                        if (valItems.Count > 0)
                            valItems.RemoveAt(0);
                    }




                    if (position < 0)
                        break;

                    int nPos = position + 2;

                    while (true)
                    {
                        char caracter = texto.Substring(nPos, 1)[0];

                        if (caracter == '-' || caracter == '('
                            || caracter == ')' || ((int)caracter > 47 && (int)caracter < 58))
                        {

                            nPos++;
                        }
                        else
                            break;
                    }



                    string variable = texto.Substring(position + 1, nPos - position - 1).Trim();

                    string textoReemplazo = "";
                    if (!valEnc.TryGetValue(variable.Replace("-", "").Replace("(", "").Replace(")", "").ToUpper(), out textoReemplazo))
                        if (valItems.Count > 0)
                            valItems[0].TryGetValue(variable.Replace("-", "").Replace("(", "").Replace(")", "").ToUpper(), out textoReemplazo);





                    if (textoReemplazo == null)
                        textoReemplazo = "";

                    CamposImpresion myValue = camposImpresionPorCodigo[variable.Replace("-", "").Replace("(", "").Replace(")", "").ToUpper()];

                    switch (myValue.Alineación)
                    {
                        case "D":
                            textoReemplazo = textoReemplazo.PadLeft(nPos - position, ' ');
                            break;
                        case "I":
                            textoReemplazo = textoReemplazo.PadRight(nPos - position, ' ');
                            break;
                        case "C":
                            textoReemplazo = centrarCadena(textoReemplazo, nPos - position);
                            break;
                        default:
                            textoReemplazo = textoReemplazo.PadLeft(nPos - position, ' ');
                            break;
                    }


                    texto = texto.Substring(0, position) + textoReemplazo + texto.Substring(nPos, texto.Length - nPos);

                    posicionAnterior = position + textoReemplazo.Length;

                }
                return texto;

            }
            catch (Exception ex)
            {
                LogManager.Mensaje m =
                    new LogManager.Mensaje("SGECA.ColaDeImpresion",
                         "prepararTexto(int)",
                         0,
                         "Error al intentar preparar el documento a imprimir. Id: " + id,
                         ex.Message,
                         "",
                         true,
                         LogManager.EMensaje.Critico,
                         ex.StackTrace);

                Notify(m);
                return "";
            }
        }


        private string centrarCadena(string stringToCenter, int totalLength)
        {
            return stringToCenter.PadLeft(((totalLength - stringToCenter.Length) / 2)
                                + stringToCenter.Length)
                       .PadRight(totalLength);
        }

        private Dictionary<string, CamposImpresion> obtenerCamposImpresion(out Dictionary<string, CamposImpresion> porCodigo)
        {
            Dictionary<string, CamposImpresion> camposImpresion = new Dictionary<string, CamposImpresion>();
            porCodigo = new Dictionary<string, CamposImpresion>();

            try
            {
                MySqlConnection cone = new MySqlConnection(ConfigurationManager.ConnectionStrings["ce"].ConnectionString);
                MySqlCommand cmd = new MySqlCommand("select * from camposImpresion inner join grupoimpresion  ", cone);
                cone.Open();
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    CamposImpresion c = new CamposImpresion();
                    c.Campo = dr["cim_campo"].ToString().ToUpper();
                    c.Codigo = dr["cim_codigo"].ToString().ToUpper();
                    c.Formato = dr["cim_formato"].ToString();
                    int copias = 1;
                    int.TryParse(dr["gim_copias"].ToString(), out copias);
                    c.Copias = copias;

                    c.Impresora = dr["gim_impresora"].ToString();

                    c.Tipo = dr["cim_tipo"].ToString().ToUpper();
                    c.Alineación = dr["cim_alineacion"].ToString().ToUpper();

                    camposImpresion.Add(c.Campo, c);
                    porCodigo.Add(c.Codigo, c);

                }
                cone.Close();
            }
            catch (Exception ex)
            {
                porCodigo = null;
                camposImpresion = null;
                LogManager.Mensaje m =
                    new LogManager.Mensaje("SGECA.ColaDeImpresion",
                         "obtenerCamposImpresion(out Dictionary)",
                         0,
                         "Error al intentar obtener los campos de impresión.",
                         ex.Message,
                         "",
                         true,
                         LogManager.EMensaje.Critico,
                         ex.StackTrace);

                Notify(m);
            }
            return camposImpresion;
        }

        private Dictionary<string, string> obtenerValoresEncabezado(Dictionary<string, CamposImpresion> camposImpresion, int Id)
        {

            Dictionary<string, string> retorno = new Dictionary<string, string>();
            MySqlConnection cone = new MySqlConnection(ConfigurationManager.ConnectionStrings["ce"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand("select * from vw_comprobanteEncabezado " +
            "where cen_id = @cen_id", cone);
            try
            {
                cmd.Parameters.AddWithValue("@cen_id", Id);
                //lstDatos = new List<datos>();
                cone.Open();
                MySqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        datos d = new SGECA.ColaDeImpresion.datos();
                        d.Ordinal = dr.GetSchemaTable().Rows[i][0].ToString().ToUpper();
                        d.Valor = dr[i];
                        d.Tipo = dr.GetDataTypeName(i).ToUpper();

                        CamposImpresion c;

                        if (camposImpresion.TryGetValue(d.Ordinal, out c))
                        {
                            c.Datos = d;

                            string key = c.Codigo;
                            string valor = formatearDatos(c);

                            retorno.Add(key, valor);

                        }
                    }


                }
                cone.Close();
            }

            catch (Exception Exception)
            {

                LogManager.Mensaje m = new LogManager.Mensaje("SGECA.ColaDeImpresion.builders.Documento",
                                            "obtenerValoresEncabezado",
                                            0,
                                            "Error al intentar obtener los valores del encabezado.",
                                            Exception.Message,
                                            "",
                                            true,
                                            LogManager.EMensaje.Critico,
                                            Exception.StackTrace);

                Notify(m);
                retorno = null;
            }
            finally
            {
                if (cone.State != ConnectionState.Closed)
                    cone.Close();
            }
            return retorno;
        }
        private List<Dictionary<string, string>> obtenerValoresItems(Dictionary<string, CamposImpresion> camposImpresion, int Id)
        {
            List<Dictionary<string, string>> retorno = new List<Dictionary<string, string>>();

            MySqlConnection cone = new MySqlConnection(ConfigurationManager.ConnectionStrings["ce"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand("select * from vw_comprobanteitem " +
            "where cen_id = @cen_id", cone);
            try
            {
                cmd.Parameters.AddWithValue("@cen_id", Id);
                //lstDatos = new List<datos>();
                cone.Open();
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Dictionary<string, string> item = new Dictionary<string, string>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        datos d = new SGECA.ColaDeImpresion.datos();
                        d.Ordinal = dr.GetSchemaTable().Rows[i][0].ToString().ToUpper();
                        d.Valor = dr[i];
                        d.Tipo = dr.GetDataTypeName(i).ToUpper();

                        CamposImpresion c;

                        if (camposImpresion.TryGetValue(d.Ordinal, out c))
                        {
                            c.Datos = d;

                            string key = c.Codigo;
                            string valor = formatearDatos(c);

                            item.Add(key, valor);

                        }
                    }
                    retorno.Add(item);

                }
                cone.Close();
            }

            catch (Exception Exception)
            {

                LogManager.Mensaje m = new LogManager.Mensaje("SGECA.ColaDeImpresion.builders.Documento",
                                            "obtenerValoresItems",
                                            0,
                                            "Error al intentar obtener los valores de items.",
                                            Exception.Message,
                                            "",
                                            true,
                                            LogManager.EMensaje.Critico,
                                            Exception.StackTrace);

                Notify(m);
                retorno = null;
            }
            finally
            {
                if (cone.State != ConnectionState.Closed)
                    cone.Close();
            }
            return retorno;
        }

        private string formatearDatos(CamposImpresion c)
        {
            string valor = "";
            try
            {
                switch (c.Datos.Tipo.ToUpper())
                {
                    case "INT":

                        int t;
                        if (int.TryParse(c.Datos.Valor.ToString(), out t))
                        {
                            if (c.Formato.Contains("-"))
                            {
                                string[] h = c.Formato.Split('-');

                                int inicio = 0;

                                for (int j = 0; j < h.Length; j++)
                                {
                                    if (valor.Length > 0)
                                        valor += "-";
                                    valor += c.Datos.Valor.ToString().Substring(inicio, h[j].Length);

                                    inicio += h[j].Length;
                                }


                            }
                            else
                            {
                                valor = t.ToString();
                                if (c.Formato.Length > 0)
                                    valor = valor.Trim().PadLeft(c.Formato.Length, '0').Substring(0, c.Formato.Length);
                            }
                        }
                        break;
                    case "BIGINT":
                        long l;
                        if (long.TryParse(c.Datos.Valor.ToString(), out l))
                        {
                            if (c.Formato.Contains("-"))
                            {
                                string[] h = c.Formato.Split('-');

                                int inicio = 0;

                                for (int j = 0; j < h.Length; j++)
                                {
                                    if (valor.Length > 0)
                                        valor += "-";
                                    valor += c.Datos.Valor.ToString().Substring(inicio, h[j].Length);

                                    inicio += h[j].Length;
                                }


                            }
                            else
                            {
                                valor = l.ToString();
                                if (c.Formato.Length > 0)
                                    valor = valor.Trim().PadLeft(c.Formato.Length, '0').Substring(0, c.Formato.Length);
                            }

                        }
                        break;
                    case "DATETIME":
                        DateTime dt;
                        if (DateTime.TryParse(c.Datos.Valor.ToString(), out dt))
                        {
                            valor = dt.ToString(c.Formato);
                        }
                        break;
                    case "DATE":
                        DateTime dat;
                        if (DateTime.TryParse(c.Datos.Valor.ToString(), out dat))
                        {
                            valor = dat.ToString(c.Formato);
                        }
                        break;
                    case "DECIMAL":
                        decimal dc;
                        if (Decimal.TryParse(c.Datos.Valor.ToString(), out dc))
                        {
                            valor = dc.ToString(c.Formato);
                        }
                        break;
                    case "VARCHAR":

                        valor = c.Datos.Valor.ToString();
                        if (c.Formato.Length > 0)
                            if (c.Formato.Contains("#"))
                                valor = valor.Trim().PadLeft(c.Formato.Length, '0').Substring(0, c.Formato.Length);
                            else
                                if (c.Formato.Length > 0)
                                    valor = valor.Trim().PadLeft(c.Formato.Length, '0').Substring(0, c.Formato.Length);

                        break;
                    case "BLOB":

                        valor = c.Datos.Valor.ToString();
                        if (c.Formato.Length > 0)
                            if (c.Formato == "*")
                            {

                                if (c.Datos.Valor is byte[])
                                {
                                    valor = System.Text.Encoding.Default.GetString((byte[])c.Datos.Valor);
                                }
                            }


                        break;
                    default:
                        valor = c.Datos.Valor.ToString();
                        break;
                }
            }

            catch (Exception Exception)
            {

                LogManager.Mensaje m = new LogManager.Mensaje("SGECA.ColaDeImpresion.builders.Documento",
                                            "formatearDatos",
                                            0,
                                            "Error al intentar dar formato a los datos.",
                                            Exception.Message,
                                            "",
                                            true,
                                            LogManager.EMensaje.Critico,
                                            Exception.StackTrace);

                Notify(m);
                valor = null;
            }

            return valor;
        }

    }
}
