using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SGECA.ControlesComunes
{
    public class ordenarTextoDosCampos : IComparer
    {

        CaseInsensitiveComparer _comparer = new CaseInsensitiveComparer();

        
        public int Compare(object x, object y)
        {
            return _comparer.Compare(((DAL.Interfaces.IDosCampos)x).Descripcion, ((DAL.Interfaces.IDosCampos)y).Descripcion);


         
        }
    }
}
