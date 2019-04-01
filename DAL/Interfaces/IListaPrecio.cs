using System;
namespace SGECA.DAL
{
    public interface IListaPrecio : IGeneral
    {
        System.Collections.Generic.List<ListaPrecio> obtenerFiltrado(ItemFiltro[] itemFiltro, ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros);

        int Codigo { get; set; }
        string Nombre { get; set; }
    }
}
