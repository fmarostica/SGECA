using System;
namespace SGECA.DAL
{
    public interface IVistaComprobantes : IGeneral
    {
        System.Collections.Generic.List<VistaComprobantes> obtenerFiltrado(ItemFiltro[] itemFiltro, ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros);
        int Id { get; set; }
        string NumeroCompleto { get; set; }
        LogManager.Mensaje UltimoMensaje { get; set; }
        string Fecha { get; set; }
        string IVA { get; set; }
        string RazonSocial { get; set; }
        string Neto { get; set; }
        string Total { get; set; }
    }
}
