using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SGECA.ControlesComunes
{
    public class ComboActivo
    {
        public string Nombre { get; set; }
        public int Valor { get; set; }

        public ComboActivo(string nombre, int valor)
        {
            Nombre = nombre;
            Valor = valor;
        }
    }
}
