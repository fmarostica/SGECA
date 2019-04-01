using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SGECA.Comun
{
    public class MessageBox : Interfaces.IMessageBox
    {
        public string TextoMensaje { get; set; }

        public string TituloMensaje { get; set; }

        public MessageBoxButtons MessageBoxButtons { get; set; }

        public MessageBoxIcon MessageBoxIcon { get; set; }

        public MessageBoxDefaultButton
            MessageBoxDefaultButton { get; set; }

        /// <summary>
        /// Método encargado de filtrar el tipo de mensaje asignando sus propiedades 
        /// correspondientes. Si usuario responde YES, devuelve TRUE.
        /// </summary>
        /// <param name="mensaje">String. Corresponde al cuerpo del mensaje</param>
        /// <param name="titulo">String. Se asigna a través de la enumeración 
        /// "Común.Enums.EMessageBoxTitulo." asignando finalmente la propiedad "ToString()" 
        /// para su conversión. </param>
        public bool MostrarMessageBoxConfirmacion(string mensaje, string titulo)
        {
            switch (titulo)
            {
                case "Confirmación":
                    TextoMensaje = mensaje;
                    TituloMensaje = Enums.EMessageBoxTitulo.Confirmación.ToString();
                    MessageBoxButtons = MessageBoxButtons.YesNo;
                    MessageBoxIcon = MessageBoxIcon.Exclamation;
                    MessageBoxDefaultButton = MessageBoxDefaultButton.
                        Button1;
                    break;
            }

            switch (titulo)
            {
                case "Confirmación":
                    if (System.Windows.Forms.MessageBox
                        .Show(TextoMensaje, TituloMensaje, MessageBoxButtons,
                        MessageBoxIcon, MessageBoxDefaultButton) == DialogResult.Yes)
                    {
                        return true;
                    }
                    else
                    {
                        return false;

                    }
                default:
                    return false;
            }
        }

        /// <summary>
        /// Método encargado de filtrar el tipo de mensaje 
        /// asignando sus propiedades correspondientes.
        /// </summary>
        /// <param name="mensaje">String. Corresponde al cuerpo del mensaje</param>
        /// <param name="titulo">String. Se asigna a través de la enumeración 
        /// "Común.Enums.EMessageBoxTitulo." asignando finalmente la propiedad "ToString()" 
        /// para su conversión. </param>
        public void MostrarMessageBox(string mensaje, string titulo)
        {
            switch (titulo)
            {
                case "Atención":
                    TextoMensaje = mensaje;
                    TituloMensaje = Enums.EMessageBoxTitulo.Atención.ToString();
                    MessageBoxButtons = MessageBoxButtons.OK;
                    MessageBoxIcon = MessageBoxIcon.Exclamation;
                    break;
                case "Información":
                    TextoMensaje = mensaje;
                    TituloMensaje = Enums.EMessageBoxTitulo.Información.ToString();
                    MessageBoxButtons = MessageBoxButtons.OK;
                    MessageBoxIcon = MessageBoxIcon.Information;
                    break;
                case "Error":
                    TextoMensaje = mensaje;
                    TituloMensaje = Enums.EMessageBoxTitulo.Error.ToString();
                    MessageBoxButtons = MessageBoxButtons.OK;
                    MessageBoxIcon = MessageBoxIcon.Error;
                    break;
            }

            System.Windows.Forms.MessageBox.
                Show(TextoMensaje, TituloMensaje, MessageBoxButtons, MessageBoxIcon);
        }
    }

}
