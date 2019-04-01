using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


namespace SGECA.ControlesComunes
{
    public class DatoFiltro
    {
        public object Texto { get; set; }
        public string TipoFiltro { get; set; }
        public string Campo { get; set; }

        public DatoFiltro()
        {
        }

        public DatoFiltro(object texto, string tipoFiltro, string campo)
        {
            Texto = texto;
            TipoFiltro = tipoFiltro.ToLower();
            Campo = campo;
        }

    }

    public class Filtro
    {


        public IList obtenerFiltrado(DatoFiltro[] datosFiltro, string nombreEntidad, string orden)
        {


            return new List<string>();
        }

        public IList obtenerFiltrado(object texto, DAL.TipoFiltro tipoFiltro, string campo, string nombreEntidad, string orden)
        {

            return new List<string>();
        }

        /*private object analizarCampo(DAL.ItemFiltro item)
        {
            switch (item.itemBusqueda.Tipo)
            {
                case DAL.ItemBusqueda.TipoCampo._int:
                    int iRet = 0;
                    if (int.TryParse(item.textoBusqueda.Replace(".", "").Replace(",", ""), out iRet))
                        return iRet;
                    else
                        return null;
                    break;
                case DAL.ItemBusqueda.TipoCampo._long:
                    long lRet = 0;
                    if (long.TryParse(item.textoBusqueda.Replace(",", ""), out lRet))
                        return lRet;
                    else
                        return null;
                    break;
                case DAL.ItemBusqueda.TipoCampo._double:
                    double dRet = 0;
                    if (double.TryParse(item.textoBusqueda.Replace(",", ""), out dRet))
                        return dRet;
                    else
                        return null;
                    break;
                case DAL.ItemBusqueda.TipoCampo._decimal:
                    decimal cRet = 0;
                    if (decimal.TryParse(item.textoBusqueda.Replace(",", ""), out cRet))
                        return cRet;
                    else
                        return null;
                    break;
                case DAL.ItemBusqueda.TipoCampo._string:
                    return item.textoBusqueda;
                    break;
                case DAL.ItemBusqueda.TipoCampo._bool:
                    bool bRet = false;
                    if (bool.TryParse(item.textoBusqueda, out bRet))
                        return bRet;
                    else
                        return null;

                    break;
                default:
                    return null;
                    break;
            }
        }*/

        public IList obtenerFiltrado(DAL.ItemFiltro[] itemFiltro, bool busquedaAnd, string nombreEntidad)
        {
            return new List<string>();
        }
    }
}
