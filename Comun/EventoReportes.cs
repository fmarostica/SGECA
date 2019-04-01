using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SGECA.Comun
{
    public class EventoReportes : Interfaces.IEventoReportes
    {
        public DataGridView grdDatos { get; set; }
        public string fileName { get; set; }
    }
}
