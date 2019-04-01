using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SGECA.DAL
{
    [Serializable]
    public enum TipoOrden
    {
        Ascendente,
        Descendente
    }

    [Serializable]
    public class ItemOrden
    {

        public string Campo { get; set; }
        public TipoOrden TipoOrden { get; set; }
    }
}
