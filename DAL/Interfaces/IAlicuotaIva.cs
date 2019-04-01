using System;
namespace SGECA.DAL
{
    public interface IAlicuotaIva : IGeneral
    {
        System.Collections.Generic.List<AlicuotaIva> obtenerFiltrado(ItemFiltro[] itemFiltro, ItemOrden[] orden, bool busquedaAnd, double inicio, double fin, out double totalRegistros);

        int Codigo { get; set; }
        string Nombre { get; set; }
        decimal ali_porcentaje { get; set; }

        void obtener(decimal alicuota);
    }
}
