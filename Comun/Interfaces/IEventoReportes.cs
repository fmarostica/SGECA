using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace SGECA.Comun.Interfaces
{
    interface IEventoReportes
    {
        DataGridView grdDatos { get; set; }
        string fileName { get; set; }
    }
}
