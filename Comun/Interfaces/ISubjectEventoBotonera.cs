using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SGECA.Comun.Interfaces
{
    public interface ISubjectEventoBotonera
    {
        /// <summary>
        /// Método encargado de notificar a todos y cada uno de los observadores que ha 
        /// sucedido "algo".
        /// Esto se realiza recorriendo todos los observadores subscritos y ejecutando por 
        /// cada uno de ellos el método UpdateState() implementado de IObserver.
        /// </summary>
        void Notify(IEventoBotonera evento);

        /// <summary>
        /// Método encargado de subscribir un observador para que reciba las notificaciones.
        /// </summary>
        /// <param name="observer">Interfaz IObserver que indica el observador.</param>
        void Subscribe(IObserverEventoBotonera observer);

        /// <summary>
        /// Método encargado de desubscribir un observador para que no reciba más 
        /// notificaciones.
        /// </summary>
        /// <param name="observer">Interfaz IObserver que indica el observador.</param>
        void Unsubscribe(IObserverEventoBotonera observer);

    } // ISubject
}
