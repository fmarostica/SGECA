using System;
using System.Collections.Generic;
using System.Text;

namespace  SGECA.ColaDeImpresion
{
    public class CamposImpresion
    {
        public string Codigo { get; set; }
        public string Campo { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public string Formato { get; set; }
        public int Copias { get; set; }

        public string Alineación { get; set; }

        public datos Datos { get; set; }

        public string Impresora { get; set; }
    }
}
