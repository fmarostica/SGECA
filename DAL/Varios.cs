using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Configuration;
using System.Collections;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Data;
using System.ComponentModel;
using System.Security.Cryptography;

namespace SGECA.DAL
{
    public class Varios
    {
        public string obtenerProximoCodigoNumerico(string nombreEntidad, string nombreCampo)
        {
            string codigo;
            int largoCodigoNumerico = 5, maximo = 0;


            maximo++;


            if (ConfigurationManager.AppSettings["largoCodigoNumerico"] != null)
                int.TryParse(ConfigurationManager.AppSettings["largoCodigoNumerico"], out largoCodigoNumerico);

            codigo = maximo.ToString().PadLeft(largoCodigoNumerico, '0');
            return codigo;
        }

        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }

        public static string randomString(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static bool verificaCampoExisteNoNulo(MySqlDataReader dataReader, string campo)
        {
            try
            {
                if ((dataReader.GetOrdinal(campo) >= 0 ||
                    dataReader.GetSchemaTable().Columns.Contains(campo))
                    && !DBNull.Value.Equals(campo))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }

        }

        public static bool CUITValido(string strCUIT)
		{
		    if (strCUIT.Length == 0) return false;
			
            string CUITValidado = string.Empty;

			bool Valido = false;

			char Ch;
			for(int i = 0; i < strCUIT.Length; i++)
			{
			    Ch = strCUIT[i];
			    if ((Ch > 47) && (Ch < 58))
			        CUITValidado = CUITValidado + Ch;
            }
 
			strCUIT = CUITValidado;
			Valido = (strCUIT.Length == 11);
			if (Valido)
			{
			    int Verificador = EncontrarVerificador(strCUIT);
				Valido = (strCUIT[10].ToString() == Verificador.ToString());
            }
 
			return Valido;
        }
        

            
 
		private static int EncontrarVerificador(string CUIT)
		{
			int Sumador = 0;
			int Producto = 0;
			int Coeficiente = 0;
			int Resta = 5;
			for (int i = 0; i < 10; i++)
			{
				if (i == 4) Resta = 11;
				Producto = CUIT[i];
				Producto -= 48;
				Coeficiente = Resta - i;
				Producto = Producto * Coeficiente;	
				Sumador = Sumador + Producto;
			}
 
			int Resultado = Sumador - (11 * (Sumador/11));
			Resultado = 11 - Resultado;
 
			if (Resultado == 11) return 0;
			else return Resultado;
        }


        public static MySqlParameter obtenerParametro(string nombre, string valor, bool nullSiVacio)
        {
            MySqlParameter param = null;
            if ((valor == null || valor == "") && nullSiVacio)
                param = new MySqlParameter(nombre, DBNull.Value);
            else
                param = new MySqlParameter(nombre, valor);

            return param;
        }

        public static MySqlParameter obtenerParametro(string nombre, int valor, bool nullSiCero)
        {
            MySqlParameter param = null;
            if (valor == 0 && nullSiCero)
                param = new MySqlParameter(nombre, DBNull.Value);
            else
                param = new MySqlParameter(nombre, valor);

            return param;
        }

        public static MySqlParameter obtenerParametro(string nombre, DateTime valor)
        {
            MySqlParameter param = null;
            if (valor < new DateTime(1900, 1, 1))
                param = new MySqlParameter(nombre, new DateTime(1900, 1, 1));
            else if (valor == null)
                param = new MySqlParameter(nombre, DBNull.Value);
            else
                param = new MySqlParameter(nombre, valor);

            return param;
        }

        internal static void armarConsultaFiltros(ItemFiltro[] itemFiltro, MySqlCommand comando, ref int parameterCount, ref string where, string tipoBusqueda)
        {
            //itero en todos los item de filtrado para armar la consulta
            foreach (ItemFiltro item in itemFiltro)
            {
                parameterCount++;

                switch (item.tipoFiltroTexto.value)
                {
                    case TipoFiltro.Like:
                        where += tipoBusqueda +
                               item.itemBusqueda.Value +
                               " LIKE CONCAT('%', " + "@parameter" +
                               parameterCount.ToString() + ",'%')";
                        comando.Parameters.AddWithValue("@parameter" +
                            parameterCount.ToString(), item.textoBusqueda);
                        break;
                    case TipoFiltro.NotLike:
                        where += tipoBusqueda +
                               item.itemBusqueda.Value +
                               " NOT LIKE CONCAT('%', " + "@parameter" +
                               parameterCount.ToString() + ",'%')";
                        comando.Parameters.AddWithValue("@parameter" +
                            parameterCount.ToString(), item.textoBusqueda);
                        break;
                    case TipoFiltro.GreaterThan:
                        where += tipoBusqueda +
                               item.itemBusqueda.Value +
                               " >  @parameter" +
                               parameterCount.ToString();
                        comando.Parameters.AddWithValue("@parameter" +
                            parameterCount.ToString(), item.textoBusqueda);
                        break;
                    case TipoFiltro.GreaterThanOrEqual:
                        where += tipoBusqueda +
                               item.itemBusqueda.Value +
                               " <  @parameter" +
                               parameterCount.ToString();
                        comando.Parameters.AddWithValue("@parameter" +
                            parameterCount.ToString(), item.textoBusqueda);
                        break;
                    case TipoFiltro.LessThan:
                        where += tipoBusqueda +
                               item.itemBusqueda.Value +
                               " >  @parameter" +
                               parameterCount.ToString();
                        comando.Parameters.AddWithValue("@parameter" +
                            parameterCount.ToString(), item.textoBusqueda);
                        break;
                    case TipoFiltro.Equal:
                        where += tipoBusqueda +
                               item.itemBusqueda.Value +
                               " =  @parameter" +
                               parameterCount.ToString();
                        comando.Parameters.AddWithValue("@parameter" +
                            parameterCount.ToString(), item.textoBusqueda);
                        break;
                    case TipoFiltro.NotEqual:
                        where += tipoBusqueda +
                               item.itemBusqueda.Value +
                               " <>  @parameter" +
                               parameterCount.ToString();
                        comando.Parameters.AddWithValue("@parameter" +
                            parameterCount.ToString(), item.textoBusqueda);
                        break;
                    case TipoFiltro.In:
                        where += tipoBusqueda +
                               item.itemBusqueda.Value +
                               " IN  (@parameter" +
                               parameterCount.ToString() + ")";
                        comando.Parameters.AddWithValue("@parameter" +
                            parameterCount.ToString(), item.textoBusqueda);
                        break;
                    case TipoFiltro.NotIn:
                        where += tipoBusqueda +
                               item.itemBusqueda.Value +
                               " NOT IN  (@parameter" +
                               parameterCount.ToString() + ")";
                        comando.Parameters.AddWithValue("@parameter" +
                            parameterCount.ToString(), item.textoBusqueda);
                        break;
                    case TipoFiltro.LessThanOrEqual:
                        where += tipoBusqueda +
                               item.itemBusqueda.Value +
                               " <=  (@parameter)" +
                               parameterCount.ToString();
                        comando.Parameters.AddWithValue("@parameter" +
                            parameterCount.ToString(), item.textoBusqueda);
                        break;
                    case TipoFiltro.Between:
                        where += tipoBusqueda +
                               item.itemBusqueda.Value +
                               " BETWEEN  (@parameter" +
                               parameterCount.ToString() + ")";

                        comando.Parameters.AddWithValue("@parameter" +
                          parameterCount.ToString(), Convert.ToDateTime(item.textoBusqueda));


                        parameterCount++;

                        where +=" AND (@parameter" +
                               parameterCount.ToString() + ")";
                        
                        comando.Parameters.AddWithValue("@parameter" +
                          parameterCount.ToString(), Convert.ToDateTime(item.textoBusqueda2));

                        break;
                    case TipoFiltro.None:
                        where += " ";
                        break;
                    //default:
                    //    break;
                }
            }

            //quito el 1er AND
            if (where.Length > 0)
                where = "WHERE " + where.Substring(4, where.Length - 4);

        }


        internal static string armarCadenaOrden(ItemOrden[] orden, string cadenaOrden, string campoOrdenPorDefecto)
        {
            cadenaOrden = "";
            foreach (ItemOrden item in orden)
            {
                string tipoOrden = "";
                if (item.TipoOrden == TipoOrden.Ascendente)
                    tipoOrden = "";
                else
                    tipoOrden = "DESC";
                cadenaOrden += " " + item.Campo + " " + tipoOrden + ", ";
            }

            if (orden.Length > 0)
                cadenaOrden = "ORDER BY " + cadenaOrden.Substring(0, cadenaOrden.Length - 2) + " ";
            else
                cadenaOrden = "ORDER BY " + campoOrdenPorDefecto + " ";

            return cadenaOrden;
        }

        public static string crear_mensaje(string mensaje, int duracion=5000)
        {
            string script= @"
            $(document).ready(function() {
            $.jGrowl('" + mensaje + "', {theme: 'default', position: 'center', life: " + duracion + "});});";

            return script;
        }

    }
}
