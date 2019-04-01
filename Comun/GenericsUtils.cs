using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGECA.Comun
{
  public  class GenericsUtils
    {
     /// <summary>
     /// Mover un elemento dentro de una lista
     /// </summary>
     /// <param name="lista"></param>
     /// <param name="posicionActual"></param>
     /// <param name="nuevaPosicion"></param>
      public void Move(IList lista, int posicionActual, int nuevaPosicion)
      {
          var item = lista[posicionActual];
          lista.RemoveAt(posicionActual);
          lista.Insert(nuevaPosicion, item);
      }
    }
}
