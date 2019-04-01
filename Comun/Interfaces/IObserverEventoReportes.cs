using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SGECA.Comun.Interfaces
{
    public interface IObserverEventoReportes
    {
        /// <summary>
        /// Método encargado de indicar que el estado del proceso debe actualizarse para que 
        /// indique a los observadores "algo".
        /// </summary>
        /// <param name="error">Indicamos la interfaz a quién se le envía la notificación.</param>
        void UpdateState(System.Windows.Forms.DataGridView grdDatos, string fileName);
    }
}
