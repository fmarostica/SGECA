using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGECA.Forms.Produccion
{
    public partial class frmReporteTareasGrupos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                txtDesde.Text = DateTime.Now.ToShortDateString();
                txtHasta.Text = DateTime.Now.ToShortDateString();
            }
        }

        protected void btnSeleccionarClose_Click(object sender, EventArgs e)
        {

        }

        protected void btnEmpleadosDialogoSeleccionar_Click(object sender, EventArgs e)
        {

        }

        protected void btnTodos_Click(object sender, EventArgs e)
        {

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {

        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            bool error = false;
            DateTime aux1, aux2;

            //lib.Subscribe(LogManager.IObserver);
            
            try
            {
                aux1 = Convert.ToDateTime(txtDesde.Text);
                aux2 = Convert.ToDateTime(txtHasta.Text);
                if (aux1 > aux2)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("La fecha de inicio no puede ser mayor a la fecha final"), true);
                    error = true;
                }
            }
            catch
            {
                error = true;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe ingresar fechas válidas"), true);
            }

            if (!error)
            {
                rv1.Visible = false;
                DateTime.TryParse(txtDesde.Text, out aux1);
                DateTime.TryParse(txtHasta.Text, out aux2);
                mostrar(aux1, aux2, "Reporte de tareas");
            }
        }

        public void mostrar(DateTime Desde, DateTime Hasta, string Titulo)
        {
            try
            {
                DAL.Tareas tareas = new DAL.Tareas();

                DataTable dt = tareas.ObtenerDataTable_Grupos(Desde, Hasta);
                rv1.LocalReport.ReportPath = Server.MapPath("~/Reports/TareasGrupos.rdlc");
                rv1.ProcessingMode = ProcessingMode.Local;

                ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
                rv1.LocalReport.DataSources.Clear();
                rv1.LocalReport.DataSources.Add(datasource);

                rv1.LocalReport.SetParameters(new ReportParameter("Desde", Desde.ToString("dd/MM/yyyy")));
                rv1.LocalReport.SetParameters(new ReportParameter("Hasta", Hasta.ToString("dd/MM/yyyy")));

                rv1.Enabled = true;
                rv1.Visible = true;
                rv1.ExportContentDisposition = ContentDisposition.AlwaysInline;
            }
            catch (Exception Exception)
            {
                LogManager.Mensaje m = new LogManager.Mensaje("SGECA.Forms.FrmReporteTareasGrupos",
                                            "mostrar",
                                            0,
                                            "Error al intentar mostrar el documento.",
                                            Exception.Message,
                                            "",
                                            true,
                                            LogManager.EMensaje.Critico,
                                            Exception.StackTrace);

                //Notify(m);

            }
        }

        protected void txtEmpleado_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}