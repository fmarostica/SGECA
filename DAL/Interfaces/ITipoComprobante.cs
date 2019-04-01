using System;
namespace SGECA.DAL
{
    public interface ITipoComprobante : IGeneral
    {
        System.Collections.Generic.List<TipoComprobante> obtenerFiltrado(ItemFiltro[] itemFiltro, ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros);

        string Codigo { get; set; }
        string Descripcion { get; set; }
        string Letra { get; set; }
        string Nemonico { get; set; }
    }
}
