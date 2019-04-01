using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace SGECA.Forms.Ventas
{
    public partial class FrmLibroIvaVentas : System.Web.UI.Page, LogManager.ISubject, LogManager.IObserver
    {
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
            // Recorremos cada uno de los observadores para notificarles el evento.
            foreach (LogManager.IObserver observer in this.Observers)
            {
                // Indicamos a cada uno de los subscriptores la actualización del 
                // estado (evento) producido.
                observer.UpdateState(mensaje);
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
        } // Unsubscribe

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            txtHasta.Attributes["type"] = "date";
            txtDesde.Attributes["type"] = "date";
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            string aux_hasta = txtHasta.Value;
            string aux_desde = txtDesde.Value;

            DateTime aux1, aux2;

            //lib.Subscribe(LogManager.IObserver);

            try
            {
                rv1.Visible = false;
                DateTime.TryParse(aux_desde, out aux1);
                DateTime.TryParse(aux_hasta, out aux2);

                if (aux1 > aux2)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", string.Format("alert('{0}');", "Disculpe, las fechas no son correctas"), true);
                    return;
                }

                cpeOpcionesInforme.ClientState = "true";
                cpeOpcionesInforme.Collapsed = true;
                cpeOpcionesInforme.AutoExpand = false; 
                mostrar(aux1, aux2, txtTitulo.Text, txtNumFolio.Text);



            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", string.Format("alert('{0}');", "Ha ocurrido un error.\\n " + ex.Message + "\\n Intente nuevamente."), true);
                txtDesde.Focus();
            }


        }

        public void mostrar(DateTime desde, DateTime hasta, string titulo, string folio)
        {
            try
            {
                if (titulo == "")
                    titulo = "Libro IVA Ventas";

                if (folio == "")
                    folio = "0";

                DataTable dt = null;
                string dtName = "DataSet1";
                DAL.LibroIvaVentas liv = new DAL.LibroIvaVentas();
                hasta = hasta.AddMinutes(1439).AddSeconds(59).AddMilliseconds(99);
                dt = liv.obtener(desde, hasta, dtName);


                rv1.LocalReport.ReportPath = Server.MapPath("~/Reports/LibroIvaVentas.rdlc");
                rv1.ProcessingMode = ProcessingMode.Local;

                ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
                rv1.LocalReport.DataSources.Clear();
                rv1.LocalReport.DataSources.Add(datasource);

                rv1.LocalReport.SetParameters(new ReportParameter("Titulo", titulo));
                rv1.LocalReport.SetParameters(new ReportParameter("Folio", folio));
                rv1.LocalReport.SetParameters(new ReportParameter("Desde", desde.ToString("dd/MM/yyyy")));
                rv1.LocalReport.SetParameters(new ReportParameter("Hasta", hasta.ToString("dd/MM/yyyy")));
                //ReportViewer1.LocalReport.Refresh();
                rv1.Enabled = true;
                rv1.Visible = true;
                rv1.ExportContentDisposition = ContentDisposition.AlwaysInline;
            }
            catch (Exception Exception)
            {

                LogManager.Mensaje m = new LogManager.Mensaje("SGECA.Forms.FrmLibroIvaVentas",
                                            "mostrar",
                                            0,
                                            "Error al intentar mostrar el documento.",
                                            Exception.Message,
                                            "",
                                            true,
                                            LogManager.EMensaje.Critico,
                                            Exception.StackTrace);

                Notify(m);

            }


        }
        private void limpiarCampos()
        {
            txtDesde.Value = null;
            txtHasta.Value = null;
            txtTitulo.Text = null;
            txtNumFolio.Text = null;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiarCampos();
        }

    }
}