using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Xml;

namespace SGECA.ColaDeImpresion
{
    public partial class FrmPrincipal : Form, SGECA.LogManager.IObserver, SGECA.LogManager.ISubject
    {
        List<MapeoImpresora> lstMapeoImpresora = new List<MapeoImpresora>();
        bool imprimiendo = false;
        builders.Documento documentos = new builders.Documento();
        builders.LibroDeIva libroDeIva = new builders.LibroDeIva();

        public FrmPrincipal()
        {
            InitializeComponent();
            this.Subscribe(new LogManager.Log());
            documentos.Subscribe(this);
            libroDeIva.Subscribe(this);
        }



        private int checkPrint;
        private void btnPageSetup_Click(object sender, System.EventArgs e)
        {
            pageSetupDialog1.ShowDialog();
        }

        private void btnPrintPreview_Click(object sender, System.EventArgs e)
        {
            if (!imprimiendo)
            {
                if (clbTrabajosPendientes.CheckedItems.Count > 0)
                {
                    try
                    {
                        prepararImpresion();
                        if (richTextBoxPrintCtrl1.Text.Length > 0)
                            printPreviewDialog1.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        LogManager.Mensaje m =
                            new LogManager.Mensaje("SGECA.ColaDeImpresion",
                                 "btnPrintPreview_Click",
                                 0,
                                 "Impresora invalida (si es una impresora de red verificar que este encendida)" + printDocument1.PrinterSettings.PrinterName,
                                 ex.Message,
                                 "",
                                 true,
                                 LogManager.EMensaje.Critico,
                                 ex.StackTrace);

                        Notify(m);
                    }
                }
            }
        }

        private void prepararImpresion()
        {

            richTextBoxPrintCtrl1.Rtf = "";
            try
            {
                if (clbTrabajosPendientes.SelectedValue.ToString().StartsWith(documentos.classId + "_"))
                {
                    this.Cursor = Cursors.WaitCursor;
                    richTextBoxPrintCtrl1.Rtf = documentos.prepararTexto(clbTrabajosPendientes.SelectedValue.ToString(),
                                               Application.StartupPath);



                    if (richTextBoxPrintCtrl1.Text.Length > 0)
                    {

                        dynamic descripcion;
                        descripcion = clbTrabajosPendientes.SelectedItem;


                        printDocument1.DocumentName = "SGECA - " + descripcion.desc;

                        MapeoImpresora impre = lstMapeoImpresora.Find(i => i.NombreGrupo == descripcion.impresora);

                        printDocument1.PrinterSettings.PrinterName = impre.NombreImpresora;
                        printDocument1.PrinterSettings.Copies = short.Parse(descripcion.copias);
                        printDocument1.DefaultPageSettings.Margins.Bottom = 0;
                        printDocument1.DefaultPageSettings.Margins.Left = 0;
                        printDocument1.DefaultPageSettings.Margins.Right = 0;
                        printDocument1.DefaultPageSettings.Margins.Top = 0;

                    }
                    this.Cursor = Cursors.Default;
                }


                if (clbTrabajosPendientes.SelectedValue.ToString().StartsWith("LIV_"))
                {
                    int id = int.Parse(clbTrabajosPendientes.SelectedValue.ToString().Replace("LIV_", ""));
                    FrmLibroIva frmLibroIva = new FrmLibroIva();
                    frmLibroIva.Subscribe(this);
                    frmLibroIva.mostrar(id);
                    if (MessageBox.Show("¿Desea marcar el libro de iva como impreso y quiralo de la cola de impresión?",
                                   "Atención",
                                   MessageBoxButtons.YesNo,
                                   MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        libroDeIva.quitarDocumento(clbTrabajosPendientes.SelectedValue.ToString());
                }
            }
            catch (Exception ex)
            {
                LogManager.Mensaje m =
                    new LogManager.Mensaje("SGECA.ColaDeImpresion",
                         "prepararImpresion",
                         0,
                         "Se produjo un error al generar la impresión. Verifique la impresora. ",
                         ex.Message,
                         "",
                         true,
                         LogManager.EMensaje.Critico,
                         ex.StackTrace);

                Notify(m);
            }


        }

        private void btnPrint_Click(object sender, System.EventArgs e)
        {
            if (!imprimiendo)
            {
                if (clbTrabajosPendientes.CheckedItems.Count > 0)
                {
                    prepararImpresion();
                    if (richTextBoxPrintCtrl1.Text.Length > 0)
                        if (printDialog1.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                printDocument1.Print();
                            }
                            catch (Exception ex)
                            {

                                LogManager.Mensaje m =
                                   new LogManager.Mensaje("SGECA.ColaDeImpresion",
                                        "btnPrint_Click",
                                        0,
                                        "Error al intentar Imprimir: " + printDocument1.PrinterSettings.PrinterName,
                                        ex.Message,
                                        "",
                                        true,
                                        LogManager.EMensaje.Critico,
                                        ex.StackTrace);
                                Notify(m);
                            }


                        }
                }
            }
        }

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            imprimiendo = true;
            checkPrint = 0;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Print the content of RichTextBox. Store the last character printed.
            checkPrint = richTextBoxPrintCtrl1.Print(checkPrint, richTextBoxPrintCtrl1.TextLength, e);

            // Check for more pages
            if (checkPrint < richTextBoxPrintCtrl1.TextLength)
                e.HasMorePages = true;
            else
                e.HasMorePages = false;
        }



        private void FrmPrincipal_Load(object sender, EventArgs e)
        {


            // this.WindowState = FormWindowState.Minimized;
            leerConfiguracionesImpresoras();
            if (lstMapeoImpresora.Count < 1)
            {
                detenerTimer();
                btnIniciarDetener.Enabled = false;

                LogManager.Mensaje m = new LogManager.Mensaje("SGECA.ColaDeImpresion",
                                         "leerConfiguracionesImpresoras",
                                         0,
                                         "No encontre mapeos de impresoras. Verifique y reinicie este programa.",
                                         "",
                                         "",
                                         true,
                                         LogManager.EMensaje.Critico,
                                         "");

                btnEliminar.Enabled = false;
                btnPrint.Enabled = false;
                btnPrintPreview.Enabled = false;
                btnRefrescar.Enabled = false;

                Notify(m);
            }
            else
            {
                int tiempoLectura = 10000;
                int.TryParse(ConfigurationManager.AppSettings["TiempoLectura"], out tiempoLectura);
                tmrTiempoLectura.Interval = tiempoLectura;
                detenerTimer();
                nicTray.Icon = this.Icon;
                refrescarLista();
            }
        }

        private void leerConfiguracionesImpresoras()
        {
            string archivo = Application.StartupPath + @"\MapeoImpresoras.xml";
            try
            {
                XmlDocument docConfig = new XmlDocument();
                docConfig.Load(archivo);

                XmlNode raiz = docConfig.SelectSingleNode("Impresoras");
                XmlNodeList impresoras = raiz.SelectNodes("Impresora");
                foreach (XmlNode item in impresoras)
                {
                    MapeoImpresora m = new MapeoImpresora();
                    m.NombreGrupo = item.SelectSingleNode("nombreGrupo").InnerText;
                    m.NombreImpresora = item.SelectSingleNode("nombreImpresora").InnerText;
                    lstMapeoImpresora.Add(m);
                }
            }
            catch (Exception ex)
            {
                LogManager.Mensaje m =
                    new LogManager.Mensaje("SGECA.ColaDeImpresion",
                         "leerConfiguracionesImpresoras",
                         0,
                         "No pude leer la configuración de mapeo de impresoras. " +
                         archivo + " exista y sea válido.",
                         ex.Message,
                         "",
                         true,
                         LogManager.EMensaje.Critico,
                         ex.StackTrace);

                Notify(m);
            }
        }


        private void tmrTiempoLectura_Tick(object sender, EventArgs e)
        {
            dispararImpresionAutomática();
        }

        private void dispararImpresionAutomática()
        {
            tmrTiempoLectura.Enabled = true;
            tmrTiempoLectura.Stop();
            impresionAutomática();
            tmrTiempoLectura.Start();
        }

        private void impresionAutomática()
        {
            if (!imprimiendo)
            {
                refrescarLista();
                if (clbTrabajosPendientes.Items.Count > 0)
                {
                    destildarTodosItemsLista();
                    imprimirProximo();
                }
            }
        }

        private void imprimirProximo()
        {
            try
            {
                clbTrabajosPendientes.SetItemChecked(0, true);
                prepararImpresion();
                if (richTextBoxPrintCtrl1.Text.Length > 0)
                    printDocument1.Print();
            }
            catch (Exception ex)
            {
                LogManager.Mensaje m =
                    new LogManager.Mensaje("SGECA.ColaDeImpresion",
                         "imprimirProximo",
                         0,
                         "Impresora invalida (si es una impresora de red verificar que este encendida)" + printDocument1.PrinterSettings.PrinterName,
                         ex.Message,
                         "",
                         true,
                         LogManager.EMensaje.Critico,
                         ex.StackTrace);

                Notify(m);
            }



        }



        private void refrescarLista()
        {
            this.Cursor = Cursors.WaitCursor;

            List<object> docs = documentos.obtener();


            verificarLibroDeIva(docs);



            if (docs.Count > 0)
            {
                ((ListBox)clbTrabajosPendientes).DataSource = docs;
                ((ListBox)clbTrabajosPendientes).ValueMember = "id";
                ((ListBox)clbTrabajosPendientes).DisplayMember = "desc";


                destildarTodosItemsLista();
            }
            else
                ((ListBox)clbTrabajosPendientes).DataSource = null;




            this.Cursor = Cursors.Default;
        }

        private void verificarLibroDeIva(List<object> items)
        {
            try
            {
                DateTime fechaDesde = DateTime.Now, fechaHasta = DateTime.Now;

                MySqlConnection cone = new MySqlConnection(ConfigurationManager.ConnectionStrings["ce"].ConnectionString);
                MySqlCommand cmdlvt = new MySqlCommand("select * from libroivaventas " +
                    "JOIN grupoimpresion on grupoimpresion.gim_id =  libroivaventas.gim_id " +
                    "where lvt_estadoimpresion is null ", cone);

                cone.Open();
                MySqlDataReader dr = cmdlvt.ExecuteReader();
                while (dr.Read())
                {
                    DateTime.TryParse(dr["lvt_desde"].ToString(), out fechaDesde);
                    DateTime.TryParse(dr["lvt_hasta"].ToString(), out fechaHasta);
                    fechaHasta = fechaHasta.AddMinutes(1439).AddSeconds(59).AddMilliseconds(99);

                    var item = new
                    {
                        id = "LIV" + "_" + dr["lvt_id"].ToString(),
                        desc = "Libro de IVA Ventas - Desde: " + fechaDesde.ToString("dd-MM-yyyy") + " Hasta: " +
                               fechaHasta.ToString("dd-MM-yyyy") + " - Título: " +
                               dr["lvt_titulo"].ToString() + " - Último Folio: " + dr["lvt_folio"].ToString() + " --> @" +
                               dr["gim_impresora"].ToString(),
                        impresora = dr["gim_impresora"].ToString(),
                        copias = dr["gim_copias"].ToString()

                    };
                    items.Add(item);
                }
                cone.Close();



            }
            catch (Exception Exception)
            {

                LogManager.Mensaje m = new LogManager.Mensaje("SGECA.ColaDeImpresion",
                                            "verificarLibroDeIva",
                                            0,
                                            "Error al buscar datos de impresión de libros de iva.",
                                            Exception.Message,
                                            "",
                                            true,
                                            LogManager.EMensaje.Critico,
                                            Exception.StackTrace);

                Notify(m);
            }
        }

        private void destildarTodosItemsLista()
        {
            foreach (int i in clbTrabajosPendientes.CheckedIndices)
            {
                clbTrabajosPendientes.SetItemCheckState(i, CheckState.Unchecked);
            }
        }

        private void btnIniciarDetener_Click(object sender, EventArgs e)
        {
            if (tmrTiempoLectura.Enabled)
                detenerTimer();
            else
                iniciarTimer();
        }

        private void detenerTimer()
        {
            tmrTiempoLectura.Stop();
            tmrTiempoLectura.Enabled = false;
            clbTrabajosPendientes.Enabled = true;
            btnIniciarDetener.Text = "&Iniciar";
            btnEliminar.Enabled = true;
            btnPrint.Enabled = true;
            btnPrintPreview.Enabled = true;
            btnRefrescar.Enabled = true;

        }

        private void iniciarTimer()
        {

            clbTrabajosPendientes.Enabled = false;
            btnIniciarDetener.Text = "&Detener";
            btnEliminar.Enabled = false;
            btnPrint.Enabled = false;
            btnPrintPreview.Enabled = false;
            btnRefrescar.Enabled = false;
            dispararImpresionAutomática();
        }

        private void btnPrintPreview_Click_1(object sender, EventArgs e)
        {

        }

        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tmrTiempoLectura.Enabled)
            {
                e.Cancel = true;
                verificarMinimizado();
            }
            else if (MessageBox.Show("Si cierra este programa no podrá seguir imprimiendo " +
                                   "los documentos originados por el sistema.\n" +
                                   "¿Está seguro que desea cerrarlo?", "Atención",
                                   MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                   MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                e.Cancel = true;
            }

        }



        private void FrmPrincipal_Resize(object sender, EventArgs e)
        {
            verificarMinimizado();

        }

        private void verificarMinimizado()
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                nicTray.Visible = true;
                nicTray.ShowBalloonTip(500);
                this.ShowInTaskbar = false;
            }
        }

        private void nicTray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            restaurarVentana();
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            refrescarLista();
        }

        private void printDocument1_EndPrint(object sender, PrintEventArgs e)
        {
            LogManager.Mensaje m = new LogManager.Mensaje("SGECA.ColaDeImpresion",
                                                   "printDocument1_EndPrint",
                                                   0,
                                                   "Se imprimio: " + printDocument1.DocumentName,
                                                   "",
                                                   "",
                                                   true,
                                                   LogManager.EMensaje.Informativo,
                                                   "");

            Notify(m);

            if (!btnRefrescar.Enabled)
                documentos.quitarDocumento(clbTrabajosPendientes.SelectedValue.ToString());
            else
                if (MessageBox.Show("El documento fue enviado a la impresora.\n\n" +
                                    "¿Desea marcarlo como impreso y quiralo de la cola de impresión?",
                                    "Atención",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    documentos.quitarDocumento(clbTrabajosPendientes.SelectedValue.ToString());


            imprimiendo = false;



        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!imprimiendo)

                if (clbTrabajosPendientes.CheckedItems.Count > 0)
                {
                    try
                    {
                        dynamic descripcion;
                        descripcion = clbTrabajosPendientes.SelectedItem;


                        if (MessageBox.Show(descripcion.desc +
                                            "\n\n¿Está seguro que desea quitar del la cola de impresión este documento?", "Atención",
                                               MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                               MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                        {
                            documentos.eliminar(clbTrabajosPendientes.SelectedValue.ToString());
                            libroDeIva.eliminar(clbTrabajosPendientes.SelectedValue.ToString());
                            refrescarLista();
                        }
                    }
                    catch (Exception Exception)
                    {

                        LogManager.Mensaje m = new LogManager.Mensaje("SGECA.ColaDeImpresion",
                                                    "btnEliminar_Click",
                                                    0,
                                                    "Error al intentar quitar documento de la cola de impresión.",
                                                    Exception.Message,
                                                    "",
                                                    true,
                                                    LogManager.EMensaje.Critico,
                                                    Exception.StackTrace);

                        Notify(m);
                    }
                }
        }

        private void restaurarVentana()
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            nicTray.Visible = false;
        }
        #region Observer Pattern
        private List<object> Observers = new List<object>();

        /// <summary>
        /// Método encargado de recibir notificaciones del subscriptor donde  ha sucedido un evento que 
        /// requiere su atención.
        /// </summary>
        public void UpdateState(LogManager.IMensaje mensaje)
        {
            Notify(mensaje);
        }

        /// <summary>
        /// Método encargado de notificar al subscriptor que ha sucedido un evento que 
        /// requiere su atención.
        /// </summary>
        public void Notify(LogManager.IMensaje mensaje)
        {
            controlTransacciones1.agregarLinea(mensaje);
            // Recorremos cada uno de los observadores para notificarles el evento.
            foreach (LogManager.IObserver observer in this.Observers)
            {
                // Indicamos a cada uno de los subscriptores la actualización del 
                // estado (evento) producido.
                observer.UpdateState(mensaje);
            }

            if (mensaje.TipoMensaje >= LogManager.EMensaje.Advertencia && this.WindowState == FormWindowState.Minimized)
            {
                restaurarVentana();
            }

        } // Notify



        /// <summary>
        /// Método encargado de agregar un observador para que el subscriptor le 
        /// pueda notificar al subscriptor el evento.
        /// </summary>
        /// <param name="observer">Interfaz IObserver que indica el observador.</param>
        public void Subscribe(LogManager.IObserver observer)
        {
            if (!this.Observers.Contains(observer))
                // Agregamos el subscriptor a la lista de subscriptores del publicador.
                this.Observers.Add(observer);
        } // Subscribe


        /// <summary>
        /// Método encargado de eliminar un observador para que el subscriptor no le 
        /// notifique ningún evento más al que era su subscriptor.
        /// </summary>
        /// <param name="observer">Interfaz IObserver que indica el observador.</param>
        public void Unsubscribe(LogManager.IObserver observer)
        {
            // Eliminamos el subscriptor de la lista de subscriptores del publicador.
            this.Observers.Remove(observer);
        }


        // Unsubscribe

        #endregion

        private void clbTrabajosPendientes_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
                for (int ix = 0; ix < clbTrabajosPendientes.Items.Count; ++ix)
                    if (e.Index != ix) clbTrabajosPendientes.SetItemChecked(ix, false);
        }
    }
}
