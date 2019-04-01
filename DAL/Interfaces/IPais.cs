using System;
namespace SGECA.DAL
{
    public interface IPais : IGeneral
    {
        System.Collections.Generic.List<Pais> obtenerFiltrado(ItemFiltro[] itemFiltro, ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros);

        int Id { get; set; }
        string Nombre { get; set; }
    }
}
