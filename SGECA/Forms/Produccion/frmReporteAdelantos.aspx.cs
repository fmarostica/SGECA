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
    public partial class frmReporteAdelantos : System.Web.UI.Page
    {
        

        private static bool todos = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                List<DAL.Empleados> lista_asignados = new List<DAL.Empleados>();
                List<DAL.Empleados> lista_empleados = new List<DAL.Empleados>();

                Session["lista_asignados"] = lista_asignados;
                Session["lista_empleados"] = lista_empleados;

                frmBuscarEmpleado.Visible = false;
                txtDesde.Text = DateTime.Now.ToShortDateString();
                txtHasta.Text = DateTime.Now.ToShortDateString();

                cargar_grupos();
            }
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

        void cargar_empleados()
        {
            List<DAL.Empleados> lista_empleados = (List<DAL.Empleados>)Session["lista_empleados"];
            List<DAL.Empleados> lista_asignados = (List<DAL.Empleados>)Session["lista_asignados"];

            if (lista_empleados != null) lista_empleados.Clear();

            double totalregs = 0;

            DAL.Empleados emp = new DAL.Empleados();
            lista_empleados = emp.Obtener_lista_adelantos(true, txtEmpleado.Text);

            totalregs = lista_empleados.Count;

            if (totalregs > 1)
            {
                frmBuscarEmpleado.Visible = true;
                grdEmpleados.DataSource = lista_empleados;
                grdEmpleados.DataBind();
            }
            if (totalregs == 1)
            {
                lista_asignados.AddRange(lista_empleados);
                Session["lista_asignados"] = lista_asignados;
            }
            if (totalregs <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("No se encontraron registros"), true);
            }

            txtEmpleado.Text = "";

            cargar_empleadosAsignados();
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
            List<DAL.Empleados> lista_asignados = (List<DAL.Empleados>)Session["lista_asignados"];
            grdAsignados.DataSource = lista_asignados;
            grdAsignados.DataBind();
        }

        protected void btnTodos_Click(object sender, EventArgs e)
        {
            int count = 0, count_repetidos = 0;

            DAL.Empleados emp = new DAL.Empleados();
            List<DAL.Empleados> lista_empleados = emp.Obtener_lista_adelantos(true);
            List<DAL.Empleados> lista_asignados = (List<DAL.Empleados>)Session["lista_asignados"];

            foreach (DAL.Empleados row in lista_empleados)
            {
                int index = -1;
                index = lista_asignados.FindIndex(item => item.Id.ToString() == row.Id.ToString());
                
                if (index < 0)
                {
                    lista_asignados.Add(lista_empleados[count]);
                    Session["lista_asignados"] = lista_asignados;
                    count++;
                }
                else
                    count_repetidos++;
            }

            cargar_empleadosAsignados();

            if (count_repetidos == 1)
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Un empleado no se agrego ya que existe en la lista de asignados"), true);
            if (count_repetidos > 1)
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Algunos empleados no se agregaron ya que existen en la lista de asignados"), true);

            if(todos)
            {
                foreach (GridViewRow row in grdEmpleados.Rows)
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
            int asignados = 0;
            DateTime aux1, aux2;

            List<DAL.Empleados> lista_asignados = (List<DAL.Empleados>)Session["lista_asignados"];
            asignados = lista_asignados.Count;

            if (asignados <= 0)
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

            if (!error)
            {
                rv1.Visible = false;
                DateTime.TryParse(txtDesde.Text, out aux1);
                DateTime.TryParse(txtHasta.Text, out aux2);
                mostrar(aux1, aux2, "Reporte de adelantos");
            }
        }
        public void mostrar(DateTime Desde, DateTime Hasta, string Titulo)
        {
            List<string> ids = new List<string>();
            List<DAL.Empleados> lista_asignados = (List<DAL.Empleados>)Session["lista_asignados"];

            try
            {
                DAL.Empleados_Adelantos adelantos = new DAL.Empleados_Adelantos();

                DataTable dt = adelantos.ObtenerDataTable(Desde, Hasta, "DataTable1", lista_asignados);
                rv1.LocalReport.ReportPath = Server.MapPath("~/Reports/AdelantosReporte.rdlc");
                rv1.ProcessingMode = ProcessingMode.Local;

                ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
                rv1.LocalReport.DataSources.Clear();
                rv1.LocalReport.DataSources.Add(datasource);

                rv1.LocalReport.SetParameters(new ReportParameter("Desde", Desde.ToString("dd/MM/yyyy")));
                rv1.LocalReport.SetParameters(new ReportParameter("Hasta", Hasta.ToString("dd/MM/yyyy")));

                rv1.Enabled = true;
                rv1.Visible = true;
                //rv1.ExportContentDisposition = ContentDisposition.AlwaysInline;
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

        protected void btnSeleccionarClose_Click(object sender, EventArgs e)
        {
            frmBuscarEmpleado.Visible = false;
        }

        protected void btnEmpleadosDialogoSeleccionar_Click(object sender, EventArgs e)
        {
            List<DAL.Empleados> lista_asignados = (List<DAL.Empleados>)Session["lista_asignados"];
            List<DAL.Empleados> lista_empleados = (List<DAL.Empleados>)Session["lista_empleados"];

            int count = 0;
            int count_repetidos = 0; //se usa solo para el msg_toast

            foreach (GridViewRow row in grdEmpleados.Rows)
            {
                CheckBox chk = (row.Cells[0].FindControl("cbox") as CheckBox);
                string id = row.Cells[1].Text;
                if (chk.Checked == true)
                {
                    int index = lista_asignados.FindIndex(item => item.Id.ToString() == id);
                    if (index < 0)
                    {
                        lista_asignados.Add(lista_empleados[count]);
                        Session["lista_asignados"] = lista_asignados;
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

        protected void txtEmpleado_TextChanged(object sender, EventArgs e)
        {
            if (txtEmpleado.Text != "") cargar_empleados();
        }

        protected void txtGrupo_TextChanged(object sender, EventArgs e)
        {
            List<DAL.Empleados> lista_empleados = (List<DAL.Empleados>)Session["lista_empleados"];
            if (txtGrupo.Text != "")
            {
                DAL.Empleados emp = new DAL.Empleados();
                lista_empleados = emp.Obtener_miembros_grupos(txtGrupo.Text, false);

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

        protected void txtGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<DAL.Empleados> lista_empleados = (List<DAL.Empleados>)Session["lista_empleados"];

            if (txtGrupo.SelectedValue.ToString() != "")
            {
                DAL.Empleados emp = new DAL.Empleados();
                lista_empleados = emp.Obtener_miembros_grupos(txtGrupo.Text, false);
                Session["lista_empleados"] = lista_empleados;

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

        protected void btnExpandir_Click(object sender, EventArgs e)
        {

        }
    }
}