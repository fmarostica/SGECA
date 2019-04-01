using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SGECA.Comun.Interfaces
{
    public interface IMessageBox
    {
        string TextoMensaje { get; set; }

        string TituloMensaje { get; set; }

        System.Windows.Forms.MessageBoxButtons MessageBoxButtons { get; set; }

        System.Windows.Forms.MessageBoxIcon MessageBoxIcon { get; set; }

        System.Windows.Forms.MessageBoxDefaultButton
            MessageBoxDefaultButton { get; set; }

        void MostrarMessageBox(string mensaje, string titulo);
        bool MostrarMessageBoxConfirmacion(string mensaje, string titulo);
    }
}
