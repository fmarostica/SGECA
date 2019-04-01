using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGECA.Forms.RRHH
{
    public partial class frmReportePersonal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void mostrar()
        {
            List<string> ids = new List<string>();

            try
            {
                DAL.Empleados empleados = new DAL.Empleados();

                DataTable dt = empleados.ObtenerDataTable("DataTable1", txtEmpleadoEstado.Text);
                rv1.LocalReport.ReportPath = Server.MapPath("~/Reports/PersonalReporte.rdlc");
                rv1.ProcessingMode = ProcessingMode.Local;

                ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
                rv1.LocalReport.DataSources.Clear();
                rv1.LocalReport.DataSources.Add(datasource);

                rv1.Enabled = true;
                rv1.Visible = true;
                rv1.ExportContentDisposition = ContentDisposition.AlwaysInline;
            }
            catch (Exception Exception)
            {
                LogManager.Mensaje m = new LogManager.Mensaje("SGECA.Forms.FrmReporteAdelantos",
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

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            mostrar();
        }
    }
}