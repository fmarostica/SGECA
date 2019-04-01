using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SGECA.ControlesComunes
{
    public partial class ControlTransacciones : UserControl
    {
        delegate void SetTextCallback(ListViewItem mensaje);

        public ControlTransacciones()
        {
            InitializeComponent();
            listBox1.Columns[0].Width = this.Width - 5;
        }

        public void agregarLinea(string texto, LogManager.EMensaje tipoMensaje, string textoAmpliado)
        {
            ListViewItem i = new ListViewItem();

            i.ToolTipText = textoAmpliado;
            i.Text = (listBox1.Items.Count + 1).ToString().PadLeft(5, '0') + " - " + texto;

            switch (tipoMensaje)
            {
                case LogManager.EMensaje.Informativo:
                    i.ForeColor = Color.Green;
                    break;
                case LogManager.EMensaje.Critico:
                    i.ForeColor = Color.Red;
                    break;
                case LogManager.EMensaje.Advertencia:
                    i.ForeColor = Color.Orange;
                    break;
                default:
                    break;
            }


            if (listBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(agregarLineaenLista);
                try
                {
                    this.Invoke(d, i);
                }
                catch (Exception ex)
                {
                }

            }
            else
            {
                agregarLineaenLista(i); ;
            }

        }


        public void agregarLinea(LogManager.IMensaje mensaje)
        {
            ListViewItem i = new ListViewItem();

            i.ToolTipText = mensaje.TextoMensajeAmpliado;
            i.Text = (listBox1.Items.Count + 1).ToString().PadLeft(5, '0') + " - " +
                     DateTime.Now.ToString("HH:mm:ss") + " - " + mensaje.TextoMensaje;
            i.Tag = mensaje;
            switch (mensaje.TipoMensaje)
            {
                case LogManager.EMensaje.Informativo:
                    i.ForeColor = Color.Green;
                    break;
                case LogManager.EMensaje.Critico:
                    i.ForeColor = Color.Red;
                    break;
                case LogManager.EMensaje.Advertencia:
                    i.ForeColor = Color.Orange;
                    break;
                case LogManager.EMensaje.Debug:
                    i.ForeColor = Color.DarkGray;
                    break;
                default:
                    break;
            }


            if (listBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(agregarLineaenLista);
                try
                {
                    this.Invoke(d, i);
                }
                catch (Exception ex)
                {
                }

            }
            else
            {
                agregarLineaenLista(i); ;
            }


        }

        private void agregarLineaenLista(ListViewItem l)
        {
            listBox1.Items.Add(l);
            listBox1.Sort();
            listBox1.ShowItemToolTips = true;
            listBox1.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            limpiarToolStripMenuItem.Enabled = true;
        }

        public void limpiarLstbMensajesTransacciones()
        {
            listBox1.Items.Clear();
            eliminarToolStripMenuItem.Enabled = false;
            limpiarToolStripMenuItem.Enabled = false;
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Comun.MessageBox msjBox = new Comun.MessageBox();

            if (msjBox.MostrarMessageBoxConfirmacion("Esta seguro que desea eliminiar " +
                " el mensaje seleccionado?", Comun.Enums.
                EMessageBoxTitulo.Confirmación.ToString()))
            {
                foreach (ListViewItem item in listBox1.SelectedItems)
                {
                    listBox1.Items.Remove(item);
                }

            }
        }

        private void limpiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Comun.MessageBox msjBox = new Comun.MessageBox();

            if (msjBox.MostrarMessageBoxConfirmacion("Esta seguro que desea limpiar todos los " +
                "mensajes?", Comun.Enums.
                EMessageBoxTitulo.Confirmación.ToString()))
            {

                limpiarLstbMensajesTransacciones();

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
                eliminarToolStripMenuItem.Enabled = true;
            if (listBox1.Items.Count > 0)
                limpiarToolStripMenuItem.Enabled = true;
        }

        private void copiarAPortapapelesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItems.Count == 1)
                {
                    if (listBox1.SelectedItems[0].Tag is LogManager.IMensaje)
                        Clipboard.SetText(((LogManager.IMensaje)listBox1.SelectedItems[0].Tag).ToString());
                    else
                        Clipboard.SetText(listBox1.SelectedItems[0].Text);
                }
            }
            catch { }
        }
    }
}
