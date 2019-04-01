
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Schema;

namespace SGECA.Comun
{
    public class XML : LogManager.ISubject
    {

        bool errorEnValidacion;
        List<LogManager.IObserver> observers = new List<LogManager.IObserver>();


        public static string obtenerValor(XmlNode item, string nombre)
        {
            foreach (XmlNode hijo in item.ChildNodes)
            {
                if (hijo.Name.ToLower() == nombre.ToLower())
                    return hijo.InnerText;
            }

            return "";

        }

        public static bool obtenerValorBooleano(XmlNode item, string xpath)
        {
            bool retorno = false;
            if (obtenerValor(item, xpath) == "1")
                retorno = true;

            return retorno;
        }

        public static int obtenerValorInteger(XmlNode item, string xpath)
        {
            int retorno = 0;
            if (!int.TryParse(obtenerValor(item, xpath), out retorno))
                return 0;

            return retorno;
        }

        public static DateTime obtenerValorDateTime(XmlNode item, string xpath)
        {
            DateTime retorno = new DateTime(1900, 1, 1);
            if (!DateTime.TryParseExact(
                obtenerValor(item, xpath),
                "yyyy-MM-dd HH:mm:ss",
                null,
                System.Globalization.DateTimeStyles.None,
                out retorno))
                return new DateTime(1900, 1, 1);

            return retorno;
        }

        /// <summary>
        /// ESTE METODO PERMITE VERIFICAR SI LA ESTRUCTURA DE UN ARCHIVO XML ES VALIDA
        /// </summary>
        /// <param name="xmlUri">ACA SE ENVIA EL ARCHVO XML A CONTROLAR CON SU RUTA DE ACCESO COMPLETA</param> 
        /// <param name="xsdUri">ACA SE ENVIA EL ARCHVO XSD QUE ES LA PLANTILLA QUE SE USA PARA CONTROLAR</param> 
        /// <param name="espacioDeNombre">ACA SE ENVIA UN ESPACIO DE NOMBRE QUE DEBE COINCIDIR CON EL DECLARADO EN EL atributo xmlns DEL ARCHIVO XML</param> 
        /// <returns></returns>
        public bool validarXmlConPlantilla(string xmlUri, string xsdUri, string espacioDeNombre)
        {
            errorEnValidacion = false;

            XmlTextReader tr = new XmlTextReader(xmlUri);
            XmlValidatingReader vr = new XmlValidatingReader(tr);
            try
            {
                vr.ValidationType = ValidationType.Schema;
                vr.Schemas.Add("", xsdUri);
                vr.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

                while (vr.Read())
                {
                    //PrintTypeInfo(vr);
                    //if (vr.NodeType == XmlNodeType.Element)
                    //{
                    //    while (vr.MoveToNextAttribute())
                    //        PrintTypeInfo(vr);
                    //}
                }
            }
            catch
            {

            }

            if (vr.ReadState != ReadState.Closed)
                vr.Close();

            if (tr.ReadState != ReadState.Closed)
                tr.Close();

            return !errorEnValidacion;
        }

        public void PrintTypeInfo(XmlValidatingReader vr)
        {
            if (vr.SchemaType != null)
            {
                if (vr.SchemaType is XmlSchemaDatatype || vr.SchemaType is XmlSchemaSimpleType)
                {
                    object value = vr.ReadTypedValue();
                    Console.WriteLine("{0}({1},{2}):{3}", vr.NodeType, vr.Name, value.GetType().Name, value);
                }
                else if (vr.SchemaType is XmlSchemaComplexType)
                {
                    XmlSchemaComplexType sct = (XmlSchemaComplexType)vr.SchemaType;
                    Console.WriteLine("{0}({1},{2})", vr.NodeType, vr.Name, sct.Name);
                }
            }
        }

        public void ValidationHandler(object sender, ValidationEventArgs args)
        {
            LogManager.IMensaje error = new LogManager.Mensaje();
            error.TextoMensaje = args.Severity + ": Archivo XML no válido";
            error.TextoMensajeAmpliado = "Línea :" + args.Exception.LineNumber + " - " + args.Message;
            error.TextoMensajeDepuracion = args.Exception.Message;
            error.EsError = false;
            error.Origen = "SGECA.Comun.XML";
            error.Metodo = "ValidationHandler";
            error.TipoMensaje = LogManager.EMensaje.Advertencia;

            Notify(error);
            errorEnValidacion = true;
        }

        public void Notify(LogManager.IMensaje mensaje)
        {
            foreach (LogManager.IObserver item in observers)
            {
                item.UpdateState(mensaje);
            }
        }

        public void Subscribe(LogManager.IObserver observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);

        }

        public void Unsubscribe(LogManager.IObserver observer)
        {
            if (observers.Contains(observer))
                observers.Remove(observer);
        }
    }
}
