using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;

namespace SGECA.DAL
{
    public class Xml : LogManager.ISubject, LogManager.IObserver
    {
        public LogManager.Mensaje UltimoMensaje { get; set; }
        public string xml_resultado { get; set; }
        public string xml_desde { get; set; }
        public string xml_cae { get; set; }

        public bool huboTimeout { get; set; }

        public string Cuit { get; set; }
        public string CbteTipo { get; set; }
        public string FchProceso { get; set; }
        public string CantReg { get; set; }

        public string Concepto { get; set; }
        public string DocTipo { get; set; }
        public string DocNro { get; set; }
        public string CbteDesde { get; set; }
        public string CbteHasta { get; set; }
        public string Resultado { get; set; }
        public string CAE { get; set; }
        public string CbteFch { get; set; }
        public string CAEFchVto { get; set; }

        public string RespuestaAFIP { get; set; }

        public Dictionary<string, string> Observaciones { get; set; }
        public Dictionary<string, string> Errores { get; set; }

        public bool leerXML(string archivo)
        {
            XmlDocument file = new XmlDocument();
            string extension = ".res";


        

            if (File.Exists(archivo + ".tou"))
            {
                try
                {
                    File.Delete(archivo + ".tou");
                    File.Delete(archivo + extension);
                    StreamWriter nuevo = new StreamWriter(archivo + extension);

                }
                catch (Exception)
                {

                }
            }
            else if (File.Exists(archivo + ".res"))
            {
                file.Load(archivo + ".res");

                RespuestaAFIP = file.InnerXml;

                XmlNodeList FECAESolicitarResult = file.GetElementsByTagName("FECAESolicitarResult");
                XmlNodeList FeCabResp = ((XmlElement)FECAESolicitarResult[0]).GetElementsByTagName("FeCabResp");
                if (FeCabResp.Count == 1)
                {
                    Cuit = ((XmlElement)FeCabResp[0]).GetElementsByTagName("Cuit").Item(0).InnerText;
                    CbteTipo = ((XmlElement)FeCabResp[0]).GetElementsByTagName("CbteTipo").Item(0).InnerText;
                    FchProceso = ((XmlElement)FeCabResp[0]).GetElementsByTagName("FchProceso").Item(0).InnerText;
                    CantReg = ((XmlElement)FeCabResp[0]).GetElementsByTagName("CantReg").Item(0).InnerText;
                }

                XmlNodeList FEDetResponse = ((XmlElement)FECAESolicitarResult[0]).GetElementsByTagName("FEDetResponse");
                XmlNodeList Obs = ((XmlElement)FECAESolicitarResult[0]).GetElementsByTagName("Obs");
                XmlNodeList Errors = ((XmlElement)FECAESolicitarResult[0]).GetElementsByTagName("Errors");
                if (FEDetResponse.Count == 1)
                {
                    Concepto = ((XmlElement)FEDetResponse[0]).GetElementsByTagName("Concepto").Item(0).InnerText;
                    DocTipo = ((XmlElement)FEDetResponse[0]).GetElementsByTagName("DocTipo").Item(0).InnerText;
                    DocNro = ((XmlElement)FEDetResponse[0]).GetElementsByTagName("DocNro").Item(0).InnerText;
                    CbteDesde = ((XmlElement)FEDetResponse[0]).GetElementsByTagName("CbteDesde").Item(0).InnerText;
                    CbteHasta = ((XmlElement)FEDetResponse[0]).GetElementsByTagName("CbteHasta").Item(0).InnerText;
                    Resultado = ((XmlElement)FEDetResponse[0]).GetElementsByTagName("Resultado").Item(0).InnerText;
                    CAE = ((XmlElement)FEDetResponse[0]).GetElementsByTagName("CAE").Item(0).InnerText;
                    CbteFch = ((XmlElement)FEDetResponse[0]).GetElementsByTagName("CbteFch").Item(0).InnerText;
                    CAEFchVto = ((XmlElement)FEDetResponse[0]).GetElementsByTagName("CAEFchVto").Item(0).InnerText;

                }
                if (Obs.Count > 0)
                {
                    Observaciones = new Dictionary<string, string>();

                    for (int o = 0; o < Obs.Count; o++)
                    {

                        XmlNodeList obsa = ((XmlElement)Obs[o]).GetElementsByTagName("Observaciones");

                        Observaciones.Add(
                            ((XmlElement)obsa[0]).GetElementsByTagName("Code").Item(0).InnerText,
                            ((XmlElement)obsa[0]).GetElementsByTagName("Msg").Item(0).InnerText);
                    }


                }

                if (Errors.Count > 0)
                {
                    Errores = new Dictionary<string, string>();
                    for (int e = 0; e < Errors.Count; e++)
                    {

                        XmlNodeList erra = ((XmlElement)Errors[e]).GetElementsByTagName("Err");

                        Errores.Add(
                            ((XmlElement)erra[0]).GetElementsByTagName("Code").Item(0).InnerText,
                            ((XmlElement)erra[0]).GetElementsByTagName("Msg").Item(0).InnerText);
                    }
                }


                return true;
            }

            return false;
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


    }
}
