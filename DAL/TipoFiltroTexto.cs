using System;
using System.Collections.Generic;
using System.Text;

namespace SGECA.DAL
{
   public class TipoFiltroTexto
    {
        public TipoFiltro value { get; set; }

        public string text
        {

            get
            {
                string retorno = "";
                switch (value)
                {
                    case TipoFiltro.Like:
                        retorno = "Que contiene";
                        break;
                    case TipoFiltro.NotLike:
                        retorno = "Que no contiene";
                        break;
                    case TipoFiltro.GreaterThan:
                        retorno = "Mayor a";
                        break;
                    case TipoFiltro.GreaterThanOrEqual:
                        retorno = "Mayor o igual a";
                        break;
                    case TipoFiltro.LessThan:
                        retorno = "Menor a";
                        break;
                    case TipoFiltro.Equal:
                        retorno = "Igual a";
                        break;
                    case TipoFiltro.NotEqual:
                        retorno = "Distinto a";
                        break;
                    case TipoFiltro.In:
                        retorno = "En la siguiente lista";
                        break;
                    case TipoFiltro.NotIn:
                        retorno = "No en la siguiente lista";
                        break;
                    case TipoFiltro.LessThanOrEqual:
                        retorno = "Menor o igual a";
                        break;
                    case TipoFiltro.Between:
                        retorno = "Entre";
                        break;
                    case TipoFiltro.None:
                        retorno = "Ninguno";
                        break;
                    default:
                        retorno = "";
                        break;
                }

                return retorno;
            }
        }
    }
}
