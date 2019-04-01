using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neolution.NeoPicking.Gestion.Clases
{
    public class ListItemFiltro
    {


        public Clases.ListItem.TipoCampo Tipo { get; set; }
        public DAL.TipoFiltro Value { get; set; }
        public string Text {get;set;}



        public ListItemFiltro(string text, DAL.TipoFiltro value, Clases.ListItem.TipoCampo tipo)
        {
            Text = text;
            Value = value;
            Tipo = tipo;
        }
    }
}
