using System;
using System.Collections.Generic;
using System.Text;

namespace SGECA.DAL
{
    [Serializable]
    public class ItemBusqueda
    {
        public string campo { get; set; }
        public List<TipoFiltro> condiciones { get; set; }

        public enum TipoCampo
        {
            _int,
            _long,
            _double,
            _decimal,
            _string,
            _bool,
            _datetime
        }

        public TipoCampo Tipo { get; set; }


        public string Value { get; set; }


        public string Text { get; set; }


        public ItemBusqueda() { }

        public ItemBusqueda(string text, string value, TipoCampo tipo)
        {
            Text = text;
            Value = value;
            Tipo = tipo;
            condiciones = new List<TipoFiltro>();

            switch (tipo)
            {
                case TipoCampo._datetime:
                case TipoCampo._decimal:
                case TipoCampo._double:
                case TipoCampo._long:
                case TipoCampo._int:
                    condiciones.Add(TipoFiltro.Equal);
                    condiciones.Add(TipoFiltro.GreaterThan);
                    condiciones.Add(TipoFiltro.GreaterThanOrEqual);
                    condiciones.Add(TipoFiltro.In);
                    condiciones.Add(TipoFiltro.LessThan);
                    condiciones.Add(TipoFiltro.LessThanOrEqual);
                    condiciones.Add(TipoFiltro.Like);
                    condiciones.Add(TipoFiltro.NotEqual);
                    condiciones.Add(TipoFiltro.NotIn);
                    condiciones.Add(TipoFiltro.NotLike);
                    condiciones.Add(TipoFiltro.Between);
                    break;
                case TipoCampo._string:
                    condiciones.Add(TipoFiltro.Equal);
                    condiciones.Add(TipoFiltro.In);
                    condiciones.Add(TipoFiltro.Like);
                    condiciones.Add(TipoFiltro.NotEqual);
                    condiciones.Add(TipoFiltro.NotIn);
                    condiciones.Add(TipoFiltro.NotLike);
                    break;
                case TipoCampo._bool:
                    condiciones.Add(TipoFiltro.Equal);
                    condiciones.Add(TipoFiltro.NotEqual);
                    break;
                default:
                    break;
            }
        }
    }
}
