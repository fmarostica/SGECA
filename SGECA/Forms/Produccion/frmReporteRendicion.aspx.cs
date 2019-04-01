using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGECA.Forms.Empleados
{
    public partial class frmReportes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!this.IsPostBack)
            {
                List<DAL.Empleados> empleados_asignados = new List<DAL.Empleados>();
                Session["empleados_asignados"] = empleados_asignados;

                ViewState["todos"] = false;

                frmBuscarEmpleado.Visible = false;

                empleados_asignados = new List<DAL.Empleados>();

                txtDesde.Text = DateTime.Now.ToShortDateString();
                txtHasta.Text = DateTime.Now.ToShortDateString();

                cargar_grupos();
            }
        }

        void cargar_empleados()
        {
            double totalregs = 0;

            DAL.Empleados emp = new DAL.Empleados();
            List<DAL.Empleados> lista_empleados = new List<DAL.Empleados>();
            List<DAL.Empleados> empleados_asignados = (List<DAL.Empleados>)Session["empleados_asignados"];

            lista_empleados = emp.obtenerFiltrado(ObtenerItemFiltro_Empleados(), obtenerOrdenActual_Empleados(), false, 0, -1, out totalregs, false);

            if (totalregs > 1)
            {
                frmBuscarEmpleado.Visible = true;
                grdEmpleados.DataSource = lista_empleados;
                grdEmpleados.DataBind();
            }
            if (totalregs == 1)
            {
                empleados_asignados.AddRange(lista_empleados);
            }
            if (totalregs <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("No se encontraron registros"), true);
            }

            txtEmpleado.Text = "";

            cargar_empleadosAsignados();
        }

        void cargar_grupos()
        {
            DAL.EmpleadosGrupos grupos = new DAL.EmpleadosGrupos();
            List<DAL.EmpleadosGrupos> lista_grupos = new List<DAL.EmpleadosGrupos>();

            lista_grupos.Add(new DAL.EmpleadosGrupos { Id = "", Nombre = "" });

            lista_grupos.AddRange(grupos.obtener());

            txtGrupo.DataSource = lista_grupos;
            txtGrupo.DataValueField = "id";
            txtGrupo.DataTextField = "nombre";
            txtGrupo.DataBind();
        }

        internal DAL.ItemFiltro[] ObtenerItemFiltro_Empleados()
        {
            List<DAL.ItemFiltro> items = new List<DAL.ItemFiltro>();

            DAL.ItemFiltro fil = new DAL.ItemFiltro();
            fil.itemBusqueda = new DAL.ItemBusqueda();
            fil.itemBusqueda.campo = "apellido";
            fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
            fil.itemBusqueda.Value = "apellido";
            fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
            fil.tipoFiltroTexto.value = DAL.TipoFiltro.Like;
            fil.textoBusqueda = txtEmpleado.Text;
            items.Add(fil);

            fil = new DAL.ItemFiltro();
            fil.itemBusqueda = new DAL.ItemBusqueda();
            fil.itemBusqueda.campo = "nombre";
            fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
            fil.itemBusqueda.Value = "nombre";
            fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
            fil.tipoFiltroTexto.value = DAL.TipoFiltro.Like;
            fil.textoBusqueda = txtEmpleado.Text;
            items.Add(fil);

            return items.ToArray<DAL.ItemFiltro>();
        }

        private DAL.ItemOrden[] obtenerOrdenActual_Empleados()
        {
            DAL.ItemOrden[] orden = new DAL.ItemOrden[1];
            orden[0] = new DAL.ItemOrden();

            if (ViewState["orden[0].TipoOrden"] != null)
            {
                if (ViewState["orden[0].TipoOrden"].ToString() == DAL.TipoOrden.Ascendente.ToString())
                    orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                else
                    orden[0].TipoOrden = DAL.TipoOrden.Descendente;

                orden[0].Campo = ViewState["orden[0].Campo"].ToString();

                return orden;
            }
            else
            {
                //Seteo el orden por defecto
                orden[0].Campo = "apellido";
                orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                ViewState["orden[0].TipoOrden"] = DAL.TipoOrden.Ascendente.ToString();
                ViewState["orden[0].Campo"] = orden[0].Campo;
            }
            return new DAL.ItemOrden[0];

        }

        void cargar_empleadosAsignados()
        {
            List<DAL.Empleados> empleados_asignados = (List<DAL.Empleados>)Session["empleados_asignados"];
            grdAsignados.DataSource = empleados_asignados;
            grdAsignados.DataBind();
        }

        protected void btnTodos_Click(object sender, EventArgs e)
        {
            bool todos = Convert.ToBoolean(ViewState["todos"].ToString());
            if (todos)
            {
                foreach (GridViewRow row in grdAsignados.Rows)
                {
                    CheckBox chk = (row.Cells[0].FindControl("cbox") as CheckBox);
                    if (chk.Checked == true) chk.Checked = false;
                }
                todos = false;
            }
            else
            {
                foreach (GridViewRow row in grdEmpleados.Rows)
                {
                    CheckBox chk = (row.Cells[0].FindControl("cbox") as CheckBox);
                    if (chk.Checked == false) chk.Checked = true;
                }
                todos = true;
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            bool todos = Convert.ToBoolean(ViewState["todos"].ToString());

            txtDesde.Text = "";
            txtHasta.Text = "";
            foreach (GridViewRow row in grdEmpleados.Rows)
            {
                CheckBox chk = (row.Cells[0].FindControl("cbox") as CheckBox);
                if (chk.Checked == true) chk.Checked = false;
            }
            todos = false;
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            bool error = false;
            DateTime aux1, aux2;

            List<DAL.Empleados> empleados_asignados = (List<DAL.Empleados>)Session["empleados_asignados"];

            if(empleados_asignados.Count<=0)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe seleccionar al menos un empleado para generar un reporte"), true);
                error = true;
            }

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

            DateTime fecha_cierre = DateTime.Now;

            if(txtFechaCierre.Text!="")
            {
                try
                {
                    fecha_cierre = Convert.ToDateTime(txtFechaCierre.Text);

                    foreach (DAL.Empleados item in empleados_asignados)
                    {
                        if(Convert.ToDateTime(item.Fecha_Cierre)>fecha_cierre)
                        {
                            error = true;
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Se esta estableciendo una fecha de cierra anterior a la establecida actualmente"), true);
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    error = true;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("La fecha de cierre debe ser una fecha válida"), true);
                }
            }

            if(!error)
            {
                if(txtFechaCierre.Text!="") // Guarda en los empleados asignados la fecha de cierre
                {
                    int incorrectos = 0;

                    foreach (DAL.Empleados item in empleados_asignados)
                    {
                        if(item.Fecha_Cierre!=null && item.Fecha_Cierre!="")
                        {
                            DateTime fecha_cierre_empleado = Convert.ToDateTime(item.Fecha_Cierre);
                            if(fecha_cierre>fecha_cierre_empleado)
                            {
                                item.Fecha_Cierre = txtFechaCierre.Text;
                                item.Guardar(false);
                            }
                            else
                                incorrectos++;
                        }
                        else
                        {
                            item.Fecha_Cierre = txtFechaCierre.Text;
                            item.Guardar(false);
                        }
                    }
                }

                rv1.Visible = false;
                DateTime.TryParse(txtDesde.Text, out aux1);
                DateTime.TryParse(txtHasta.Text, out aux2);
                mostrar(aux1, aux2, "Reporte de tareas");
            }
        }
        public void mostrar(DateTime Desde, DateTime Hasta, string Titulo)
        {
            List<string> ids = new List<string>();
            List<string> ids_grupos = new List<string>();

            List<DAL.Empleados> empleados_asignados = (List<DAL.Empleados>)Session["empleados_asignados"];
            foreach (DAL.Empleados item in empleados_asignados)
            {
                ids.Add(item.Id.ToString());

                var gid = ids_grupos.Where(t => t.Contains(item.Grupo.ToString()));
                if (gid.Count()<=0) ids_grupos.Add(item.Grupo.ToString());
            }

            try
            {
                DAL.Tareas tareas = new DAL.Tareas();

                DataTable dt = tareas.ObtenerDataTable(Desde, Hasta, "DataTable1", ids);
                rv1.LocalReport.ReportPath = Server.MapPath("~/Reports/TareasReporte.rdlc");
                rv1.ProcessingMode = ProcessingMode.Local;

                ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
                rv1.LocalReport.DataSources.Clear();
                rv1.LocalReport.DataSources.Add(datasource);

                DAL.Empleados_Gastos gastos = new DAL.Empleados_Gastos();
                dt = gastos.ObtenerDataTable(Desde.ToShortDateString(), Hasta.ToShortDateString(), ids_grupos);
                datasource = new ReportDataSource("DataSet2", dt);
                rv1.LocalReport.DataSources.Add(datasource);

                rv1.LocalReport.SetParameters(new ReportParameter("Desde", Desde.ToString("dd/MM/yyyy")));
                rv1.LocalReport.SetParameters(new ReportParameter("Hasta", Hasta.ToString("dd/MM/yyyy")));

                if(ids_grupos.Count()==1)
                {
                    DAL.EmpleadosGrupos grupo = new DAL.EmpleadosGrupos();
                    rv1.LocalReport.SetParameters(new ReportParameter("Titulo", "REPORTE DE RENDICIÓN DE " + grupo.obtener_nombre(ids_grupos[0])));
                }
                else
                    rv1.LocalReport.SetParameters(new ReportParameter("Titulo", "REPORTE DE RENDICIÓN"));

                rv1.Enabled = true;
                rv1.Visible = true;
                rv1.ExportContentDisposition = ContentDisposition.AlwaysInline;
            }
            catch (Exception Exception)
            {
                LogManager.Mensaje m = new LogManager.Mensaje("SGECA.Forms.FrmReporteVentas",
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
            if (txtEmpleado.Text != "")
                cargar_empleados();
        }

        protected void btnSeleccionarClose_Click(object sender, EventArgs e)
        {
            frmBuscarEmpleado.Visible = false;
        }

        protected void txtGrupo_TextChanged(object sender, EventArgs e)
        {
            if (txtGrupo.Text != "")
            {

                DAL.Empleados emp = new DAL.Empleados();
                List<DAL.Empleados> lista_empleados = emp.Obtener_miembros_grupos(txtGrupo.Text, false);

                if (lista_empleados.Count > 0)
                {
                    frmBuscarEmpleado.Visible = true;
                    grdEmpleados.DataSource = lista_empleados;
                    grdEmpleados.DataBind();
                    txtGrupo.Text = "";

                    foreach (GridViewRow row in grdEmpleados.Rows)
                    {
                        CheckBox chk = (row.Cells[0].FindControl("cbox") as CheckBox);
                        if (chk.Checked == false) chk.Checked = true;
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("No se encontraron registros"), true);
                    txtGrupo.Text = "";
                }

            }
        }

        protected void btnEmpleadosDialogoSeleccionar_Click(object sender, EventArgs e)
        {
            int count = 0;
            int count_repetidos = 0; //se usa solo para el msg_toast
            List<DAL.Empleados> empleados_asignados = (List<DAL.Empleados>)Session["empleados_asignados"];

            foreach (GridViewRow row in grdEmpleados.Rows)
            {
                CheckBox chk = (row.Cells[0].FindControl("cbox") as CheckBox);
                string id = row.Cells[1].Text;
                if (chk.Checked == true)
                {
                    int index = empleados_asignados.FindIndex(item => item.Id.ToString() == id);
                    if (index < 0)
                    {
                        DAL.Empleados emp = new DAL.Empleados();
                        emp = emp.Obtener(id);
                        empleados_asignados.Add(emp);
                    }
                    else
                        count_repetidos++;
                }
                count++;
            }

            if (count_repetidos == 1)
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Un empleado no se agrego ya que existe en la lista de asignados"), true);
            if (count_repetidos > 1)
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Algunos empleados no se agregaron ya que existen en la lista de asignados"), true);

            frmBuscarEmpleado.Visible = false;
            cargar_empleadosAsignados();
        }

        protected void txtGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtGrupo.SelectedValue.ToString() != "")
            {
                DAL.Empleados emp = new DAL.Empleados();
                List<DAL.Empleados> lista_empleados = emp.Obtener_miembros_grupos(txtGrupo.Text, false);

                if (lista_empleados.Count > 0)
                {
                    frmBuscarEmpleado.Visible = true;
                    grdEmpleados.DataSource = lista_empleados;
                    grdEmpleados.DataBind();

                    foreach (GridViewRow row in grdEmpleados.Rows)
                    {
                        CheckBox chk = (row.Cells[0].FindControl("cbox") as CheckBox);
                        if (chk.Checked == false) chk.Checked = true;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("No se encontraron registros"), true);
                    txtGrupo.Text = "";
                }
                txtGrupo.SelectedIndex = 0;
            }
        }

    }
}