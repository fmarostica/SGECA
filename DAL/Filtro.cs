using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SGECA.DAL
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


  

        private object analizarCampo(ItemFiltro item)
        {
            switch (item.itemBusqueda.Tipo)
            {
                case ItemBusqueda.TipoCampo._int:
                    int iRet = 0;
                    if (int.TryParse(item.textoBusqueda.Replace(".", "").Replace(",", ""), out iRet))
                        return iRet;
                    else
                        return null;
                    break;
                case ItemBusqueda.TipoCampo._long:
                    long lRet = 0;
                    if (long.TryParse(item.textoBusqueda.Replace(",", ""), out lRet))
                        return lRet;
                    else
                        return null;
                    break;
                case ItemBusqueda.TipoCampo._double:
                    double dRet = 0;
                    if (double.TryParse(item.textoBusqueda.Replace(",", ""), out dRet))
                        return dRet;
                    else
                        return null;
                    break;
                case ItemBusqueda.TipoCampo._decimal:
                    decimal cRet = 0;
                    if (decimal.TryParse(item.textoBusqueda.Replace(",", ""), out cRet))
                        return cRet;
                    else
                        return null;
                    break;
                case ItemBusqueda.TipoCampo._string:
                    return item.textoBusqueda;
                    break;
                case ItemBusqueda.TipoCampo._bool:
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
        }

   
    }
}
