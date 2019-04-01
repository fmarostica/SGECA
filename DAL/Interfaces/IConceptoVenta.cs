using System;
namespace SGECA.DAL
{
    public interface IConceptoVenta 
    {
        System.Collections.Generic.List<ConceptoVenta> obtenerFiltrado(ItemFiltro[] itemFiltro, ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros);

        void eliminar();
        void guardar();
        void obtener(string codigo);
        LogManager.Mensaje UltimoMensaje { get; set; }
        string Codigo { get; set; }
        string Nombre { get; set; }
    }
}
