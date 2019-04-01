using System;
namespace SGECA.DAL
{
    public interface IGeneral : LogManager.ISubject, LogManager.IObserver
    {
        void eliminar();
        int guardar();
        void obtener(int id);
        void obtener(string nombre);
        LogManager.Mensaje UltimoMensaje { get; set; }
    }
}
