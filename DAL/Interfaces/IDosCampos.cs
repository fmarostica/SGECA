using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGECA.DAL.Interfaces
{
    public interface IDosCampos : LogManager.ISubject
    {
        string Codigo { get; set; }
        string Descripcion { get; set; }
        int Id { get; set; }

    }
}
