using System;
using System.Collections.Generic;
using System.Text;


namespace SGECA.DAL
{
    [Serializable]
    public class ItemFiltro
    {
        public ItemBusqueda itemBusqueda { get; set; }
        public TipoFiltroTexto tipoFiltroTexto { get; set; }
        public string textoBusqueda { get; set; }
        public string textoBusqueda2 { get; set; }
        public DateTime fechaBusqueda1 { get; set; }
        public DateTime fechaBusqueda2 { get; set; }

        public ItemFiltro()
        {
        }

        public ItemFiltro(ItemBusqueda itemBusqueda, TipoFiltroTexto tipoFiltroTexto, string textoBusqueda)
        {
            this.itemBusqueda = itemBusqueda;
            this.tipoFiltroTexto = tipoFiltroTexto;

            if (tipoFiltroTexto.value == TipoFiltro.NotIn || tipoFiltroTexto.value == TipoFiltro.NotIn)
            {
                string[] texto = textoBusqueda.Split(',');

                textoBusqueda = "";
                foreach (string item in texto)
                {
                    if (textoBusqueda.Length > 0)
                        textoBusqueda += ",";
                    textoBusqueda += "'" + item + "'";
                }
            }

            this.textoBusqueda = textoBusqueda;
        }


        public override string ToString()
        {
            return "'" + itemBusqueda.Text + "' " + tipoFiltroTexto.text + ": " + textoBusqueda; ;
        }
    }
}
