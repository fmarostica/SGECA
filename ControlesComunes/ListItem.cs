using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGECA.ControlesComunes
{
    public class ListItem
    {
        public enum TipoCampo
        {
            _int,
            _long,
            _double,
            _decimal,
            _string,
            _bool
        }

        public TipoCampo Tipo {get; set;}


        public string Value {get; set;}


        public string Text {get; set;}



        public ListItem(string text, string value, TipoCampo tipo)
        {
            Text = text;
            Value = value;
            Tipo = tipo;
        }
    }
}
