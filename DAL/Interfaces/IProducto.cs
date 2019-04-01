using System;
namespace SGECA.DAL
{
    public interface IProducto : IGeneral
    {
        System.Collections.Generic.List<AlicuotaIva> obtenerFiltrado(ItemFiltro[] itemFiltro, ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros);

        int Id { get; set; }
        string Codigo { get; set; }
        string CodigoFabricante { get; set; }
        string Descripcion { get; set; }
        decimal Costo { get; set; }

        void obtener(int id);
        void obtener(string Codigo);
    }
}
