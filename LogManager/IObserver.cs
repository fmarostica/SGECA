﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  SGECA.LogManager
{
    /// <summary>
    /// Interface del patrón Observer.
    /// </summary>
    public interface IObserver
    {

        /// <summary>
        /// Método encargado de indicar que el estado del proceso debe actualizarse para que 
        /// indique a los observadores "algo".
        /// </summary>
        /// <param name="error">Indicamos la interfaz a quién se le envía la notificación.</param>
        void UpdateState(IMensaje mensaje);


    } // IObserver
}
