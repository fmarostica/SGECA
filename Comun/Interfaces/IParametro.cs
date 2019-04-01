using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SGECA.Comun.Interfaces
{
    public interface IParametro : LogManager.ISubject, LogManager.IObserver
    {
        int Par_CantidadRegistrosConsultas { get; set; }
        void obtenetParametros();
        LogManager.Mensaje UltimoMensaje { get; set; }
    }
}
