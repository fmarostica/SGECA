using System;
namespace SGECA.DAL
{
    public interface ITipoResponsable
    {
        System.Collections.Generic.List<TipoResponsable> obtenerFiltrado(ItemFiltro[] itemFiltro, ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros);
        void eliminar();
        void guardar();
        void obtener(string codigo);
        LogManager.Mensaje UltimoMensaje { get; set; }
        string Codigo { get; set; }
        string Nombre { get; set; }
    }
}
