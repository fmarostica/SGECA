using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGECA.Forms.Empleados
{
    public partial class frmAsignacionTareas : System.Web.UI.Page, LogManager.IObserver, LogManager.ISubject
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!this.IsPostBack)
            {
                List<DAL.Empleados> empleados_asignados = new List<DAL.Empleados>();
                List<DAL.Empleados> lista_empleados = new List<DAL.Empleados>();
                List<DAL.Tareas> lista_tareas = new List<DAL.Tareas>();

                Session["lista_asignados"] = empleados_asignados;
                Session["lista_empleados"] = lista_empleados;
                Session["lista_tareas"] = lista_tareas;

                frmBuscarSitio.Visible = false;
                frmBuscarEmpleado.Visible = false;
                frmSeleccionarTarea.Visible = false;
                frmModificarViatico.Visible = false;

                txtFechaInicio.Text = DateTime.Now.ToShortDateString();

                this.EnableViewState = true;

                ViewState["ordenAsignados_tipo"] = "Ascendente";
                ViewState["ordenAsignados_campo"] = "id";

                cargar_empleadosAsignados();
                cargar_empleados_combo();
                cargar_tareas_estados();
                cargar_viaticos();
                cargar_grupos();

                if(Session["usr"].ToString()!="telesoluciones")
                {
                    txtEmpleado.Enabled = false;
                    txtEmpleado.SelectedValue = Session["id"].ToString();
                }

                cargar_tareas(false, obtenerOrdenActual_Tareas(), ObtenerItemFiltro_Tareas());
            }
        }

        LogManager.Mensaje UltimoMensaje { get; set; }

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

        void cargar_viaticos()
        {
            DAL.Viaticos viaticos = new DAL.Viaticos();
            List<DAL.Viaticos> lista_viaticos = viaticos.obtener();
            txtViatico.DataSource = lista_viaticos;
            txtViatico.DataTextField = "DescripcionImporte";
            txtViatico.DataValueField = "Id";
            txtViatico.DataBind();
        }

        private void calcularTotalPaginas(int tamañoPagina, double cantidadRegistros)
        {
            int totalPaginas = (int)Math.Ceiling(int.Parse(cantidadRegistros.ToString()) / (double)tamañoPagina);

            grdTareasPaginador.setCantidadRegistros(cantidadRegistros);
            grdTareasPaginador.setTotalPaginas(totalPaginas);
        }

        private void calcularTotalPaginasSitios(int tamañoPagina, double cantidadRegistros)
        {
            int totalPaginas = (int)Math.Ceiling(int.Parse(cantidadRegistros.ToString()) / (double)tamañoPagina);

            gvSitiosPaginador.setCantidadRegistros(cantidadRegistros);
            gvSitiosPaginador.setTotalPaginas(totalPaginas);
        }
                   
        void cargar_empleados()
        {
            List<DAL.Empleados> lista_empleados = (List<DAL.Empleados>)Session["lista_empleados"];
            if(lista_empleados!=null) lista_empleados.Clear();

            double totalregs = 0;

            DAL.Empleados emp = new DAL.Empleados();
            lista_empleados = emp.obtenerFiltrado(ObtenerItemFiltro_Empleados(),obtenerOrdenActual_Empleados(),false,0,-1, out totalregs, true);
            Session["lista_empleados"] = lista_empleados;

            if(totalregs>1)
            {
                frmBuscarEmpleado.Visible = true;
                grdEmpleados.DataSource = lista_empleados;
                grdEmpleados.DataBind();
            }
            if(totalregs==1)
            {
                List<DAL.Empleados> empleados_asignados = Session["lista_asignados"] as List<DAL.Empleados>;
                empleados_asignados.AddRange(lista_empleados);
                Session["lista_asignados"] = empleados_asignados;
            }
            if(totalregs<=0)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("No se encontraron registros"), true);
            }

            txtBuscarEmpleado.Text = "";

            cargar_empleadosAsignados();
        }

        void cargar_empleadosAsignados()
        {
            List<DAL.Empleados> empleados_asignados = Session["lista_asignados"] as List<DAL.Empleados>;
            grdEmpleadosAsignados.DataSource = empleados_asignados;
            grdEmpleadosAsignados.DataBind();
        }

        void cargar_grupos()
        {
            DAL.EmpleadosGrupos grupos = new DAL.EmpleadosGrupos();
            List<DAL.EmpleadosGrupos> lista_grupos = new List<DAL.EmpleadosGrupos>();

            lista_grupos.Add(new DAL.EmpleadosGrupos { Id = "", Nombre = "" });

            lista_grupos.AddRange(grupos.obtener());

            txtEmpleadoGrupo.DataSource = lista_grupos;
            txtEmpleadoGrupo.DataValueField = "id";
            txtEmpleadoGrupo.DataTextField = "nombre";
            txtEmpleadoGrupo.DataBind();
        }

        void cargar_empleados_combo()
        {
            DAL.Empleados emp = new DAL.Empleados();
            List<DAL.Empleados> lista_empleados = emp.Obtener_lista("ACTIVOS");
            txtEmpleado.DataSource = lista_empleados;
            txtEmpleado.DataTextField = "ApellidoyNombre";
            txtEmpleado.DataValueField = "Id";
            txtEmpleado.DataBind();
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
            fil.textoBusqueda = txtBuscarEmpleado.Text;
            items.Add(fil);

            fil = new DAL.ItemFiltro();
            fil.itemBusqueda = new DAL.ItemBusqueda();
            fil.itemBusqueda.campo = "nombre";
            fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
            fil.itemBusqueda.Value = "nombre";
            fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
            fil.tipoFiltroTexto.value = DAL.TipoFiltro.Like;
            fil.textoBusqueda = txtBuscarEmpleado.Text;
            items.Add(fil);

            return items.ToArray<DAL.ItemFiltro>();
        }

        internal DAL.ItemFiltro[] ObtenerItemFiltro_Tareas()
        {
            List<DAL.ItemFiltro> items = new List<DAL.ItemFiltro>();

            DAL.ItemFiltro fil = new DAL.ItemFiltro();
            fil.itemBusqueda = new DAL.ItemBusqueda();
            fil.itemBusqueda.campo = "empleado";
            fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
            fil.itemBusqueda.Value = "empleado";
            fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
            fil.tipoFiltroTexto.value = DAL.TipoFiltro.Like;
            fil.textoBusqueda = txtBuscar.Text;
            items.Add(fil);

            fil = new DAL.ItemFiltro();
            fil.itemBusqueda = new DAL.ItemBusqueda();
            fil.itemBusqueda.campo = "sitio_id";
            fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
            fil.itemBusqueda.Value = "sitio_id";
            fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
            fil.tipoFiltroTexto.value = DAL.TipoFiltro.Like;
            fil.textoBusqueda = txtBuscarSitioID.Text;
            items.Add(fil);

            if(txtBuscarDesde.Text!="" || txtBuscarHasta.Text!="")
            {
                DateTime desde = DateTime.Now;
                DateTime hasta = DateTime.Now;

                DateTime.TryParse(txtBuscarDesde.Text, out desde);
                DateTime.TryParse(txtBuscarHasta.Text, out hasta);

                fil = new DAL.ItemFiltro();
                fil.itemBusqueda = new DAL.ItemBusqueda();
                fil.itemBusqueda.campo = "fecha_inicio";
                fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._datetime;
                fil.itemBusqueda.Value = "fecha_inicio";
                fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
                fil.tipoFiltroTexto.value = DAL.TipoFiltro.Between;
                fil.textoBusqueda = desde.ToShortDateString();
                fil.textoBusqueda2 = hasta.ToShortDateString();
                items.Add(fil);
            }

            return items.ToArray<DAL.ItemFiltro>();
        }

        internal DAL.ItemFiltro[] ObtenerItemFiltro_Sitios()
        {
            List<DAL.ItemFiltro> items = new List<DAL.ItemFiltro>();

            DAL.ItemFiltro fil = new DAL.ItemFiltro();
            fil.itemBusqueda = new DAL.ItemBusqueda();
            fil.itemBusqueda.campo = "nombre";
            fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
            fil.itemBusqueda.Value = "nombre";
            fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
            fil.tipoFiltroTexto.value = DAL.TipoFiltro.Like;
            fil.textoBusqueda = txtBuscarSitioDialogo.Text;
            items.Add(fil);

            fil = new DAL.ItemFiltro();
            fil.itemBusqueda = new DAL.ItemBusqueda();
            fil.itemBusqueda.campo = "CellID";
            fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
            fil.itemBusqueda.Value = "CellID";
            fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
            fil.tipoFiltroTexto.value = DAL.TipoFiltro.Like;
            fil.textoBusqueda = txtBuscarSitioDialogo.Text;
            items.Add(fil);

            return items.ToArray<DAL.ItemFiltro>();
        }

        internal DAL.ItemFiltro[] ObtenerItemFiltro_Viaticos()
        {
            List<DAL.ItemFiltro> items = new List<DAL.ItemFiltro>();

            DAL.ItemFiltro fil = new DAL.ItemFiltro();
            fil.itemBusqueda = new DAL.ItemBusqueda();
            fil.itemBusqueda.campo = "viatico_descripcion";
            fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
            fil.itemBusqueda.Value = "viatico_descripcion";
            fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
            fil.tipoFiltroTexto.value = DAL.TipoFiltro.Like;
            fil.textoBusqueda = txtBuscarSitioDialogo.Text;
            items.Add(fil);

            fil = new DAL.ItemFiltro();
            fil.itemBusqueda = new DAL.ItemBusqueda();
            fil.itemBusqueda.campo = "viatico_codigo";
            fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
            fil.itemBusqueda.Value = "viatico_codigo";
            fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
            fil.tipoFiltroTexto.value = DAL.TipoFiltro.Equal;
            fil.textoBusqueda = txtBuscarSitioDialogo.Text;
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

        private DAL.ItemOrden[] obtenerOrdenActual_Sitios()
        {
            DAL.ItemOrden[] orden = new DAL.ItemOrden[1];
            orden[0] = new DAL.ItemOrden();

            if (ViewState["ordenSitio[0].TipoOrden"] != null)
            {
                if (ViewState["ordenSitio[0].TipoOrden"].ToString() == DAL.TipoOrden.Ascendente.ToString())
                    orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                else
                    orden[0].TipoOrden = DAL.TipoOrden.Descendente;

                orden[0].Campo = ViewState["ordenSitio[0].Campo"].ToString();

                return orden;
            }
            else
            {
                //Seteo el orden por defecto
                orden[0].Campo = "Nombre";
                orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                ViewState["ordenSitio[0].TipoOrden"] = DAL.TipoOrden.Ascendente.ToString();
                ViewState["ordenSitio[0].Campo"] = orden[0].Campo;
            }
            return new DAL.ItemOrden[0];

        }

        private DAL.ItemOrden[] obtenerOrdenActual_Viaticos()
        {
            DAL.ItemOrden[] orden = new DAL.ItemOrden[1];
            orden[0] = new DAL.ItemOrden();

            if (ViewState["ordenViatico[0].TipoOrden"] != null)
            {
                if (ViewState["ordenViatico[0].TipoOrden"].ToString() == DAL.TipoOrden.Ascendente.ToString())
                    orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                else
                    orden[0].TipoOrden = DAL.TipoOrden.Descendente;

                orden[0].Campo = ViewState["ordenViatico[0].Campo"].ToString();

                return orden;
            }
            else
            {
                //Seteo el orden por defecto
                orden[0].Campo = "viatico_descripcion";
                orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                ViewState["ordenViatico[0].TipoOrden"] = DAL.TipoOrden.Ascendente.ToString();
                ViewState["ordenViatico[0].Campo"] = orden[0].Campo;
            }
            return new DAL.ItemOrden[0];

        }

        private DAL.ItemOrden[] obtenerOrdenActual_Tareas()
        {
            DAL.ItemOrden[] orden = new DAL.ItemOrden[1];
            orden[0] = new DAL.ItemOrden();

            if (ViewState["ordenTarea[0].TipoOrden"] != null)
            {
                if (ViewState["ordenTarea[0].TipoOrden"].ToString() == DAL.TipoOrden.Ascendente.ToString())
                    orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                else
                    orden[0].TipoOrden = DAL.TipoOrden.Descendente;

                orden[0].Campo = ViewState["ordenTarea[0].Campo"].ToString();

                return orden;
            }
            else
            {
                //Seteo el orden por defecto
                orden[0].Campo = "fecha_inicio";
                orden[0].TipoOrden = DAL.TipoOrden.Descendente;
                ViewState["ordenTarea[0].TipoOrden"] = DAL.TipoOrden.Descendente;
                ViewState["ordenTarea[0].Campo"] = orden[0].Campo;
            }
            return new DAL.ItemOrden[0];

        }

        void cargar_tareas_estados(string tipo="Trabajado")
        {
            DAL.TareasEstados estados = new DAL.TareasEstados();
            List<DAL.TareasEstados> lista_estados = new List<DAL.TareasEstados>();

            lista_estados.Add(new DAL.TareasEstados { Id = "", Nombre = "" });
            lista_estados.AddRange(estados.obtener("", tipo));

            txtEstado.DataSource = lista_estados;
            txtEstado.DataValueField = "Id";
            txtEstado.DataTextField = "Nombre";
            txtEstado.DataBind();
        }


        void cargar_tareas(bool todos, DAL.ItemOrden[] orden, DAL.ItemFiltro[] filtro)
        {
            int paginaActual = grdTareasPaginador.obtenerPaginaActual();

            int tamañoPagina = grdTareasPaginador.obtenerRegistrosMostrar();

            int registroInicio = ((paginaActual - 1) * tamañoPagina) + 1;

            int registroFin;
            if (todos)
                registroFin = -1;
            else
                registroFin = tamañoPagina * paginaActual;

            DAL.Tareas tareas = new DAL.Tareas();
            
            tareas.Subscribe(this);

            double cantidadRegistros = 0;

            List<DAL.Tareas> lista_tareas = tareas.obtenerFiltrado(filtro,
                                                   orden,
                                                   true,
                                                   registroInicio,
                                                   registroFin,
                                                   out cantidadRegistros);

            Session["lista_tareas"] = lista_tareas;

            if (tareas.UltimoMensaje != null)
            {
                UltimoMensaje = tareas.UltimoMensaje;
                Notify(UltimoMensaje);
                return;
            }
            
            calcularTotalPaginas(tamañoPagina, cantidadRegistros);

            grdTareasPaginador.setPaginaActual(paginaActual);

            grdTareas.DataSource = lista_tareas;
            grdTareas.DataBind();
        }

        void cargar_sitio()
        {
            DAL.Sitios sitios = new DAL.Sitios();
            int registros = 0;

            List<DAL.Sitios> lista_sitios = sitios.obtener(txtSitio.Text);
            registros = lista_sitios.Count;

            if (registros <= 0) 
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("No se encontraron registros"), true);
            if (registros == 1)
            {
                lblSitio.Text = lista_sitios[0].Nombre;
                txtSitio.Text = lista_sitios[0].Codigo;
                txtViatico.ClearSelection();
                try
                {
                    txtViatico.Items.FindByValue(lista_sitios[0].Viatico_ID).Selected = true;
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Se produjo un error: No se encontro el codigo de viatico " + lista_sitios[0].Viatico_ID), true);
                }

                //Control del clima


            }
                
            if(registros>1)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Se encontraron mas de un registro"), true);
                frmBuscarSitio.Visible = true;
                txtBuscarSitioDialogo.Text = txtSitio.Text;
                cargar_sitios_grid(false, obtenerOrdenActual_Sitios(), ObtenerItemFiltro_Sitios());
            }

            if(txtViatico.SelectedValue.ToString()=="2")
            {
                List<DAL.Empleados> empleados_asignados = Session["lista_asignados"] as List<DAL.Empleados>;
                foreach (DAL.Empleados item in empleados_asignados)
                {
                    if (item.Provincia == "2")
                    {
                        txtViatico.SelectedValue = "5";
                    }
                }
            }
        }

        void cargar_sitios_grid(bool todos, DAL.ItemOrden[] orden, DAL.ItemFiltro[] filtro)
        {
            int paginaActual = gvSitiosPaginador.obtenerPaginaActual();

            int tamañoPagina = gvSitiosPaginador.obtenerRegistrosMostrar();

            int registroInicio = ((paginaActual - 1) * tamañoPagina) + 1;

            int registroFin;
            if (todos)
                registroFin = -1;
            else
                registroFin = tamañoPagina * paginaActual;

            DAL.Sitios sitios = new DAL.Sitios();

            sitios.Subscribe(this);

            double cantidadRegistros = 0;

            List<DAL.Sitios> lista_sitios = sitios.obtenerFiltrado(filtro,
                                                   orden,
                                                   false,
                                                   registroInicio,
                                                   registroFin,
                                                   out  cantidadRegistros);

            if (sitios.UltimoMensaje != null)
            {
                UltimoMensaje = sitios.UltimoMensaje;
                Notify(UltimoMensaje);
                return;
            }

            calcularTotalPaginasSitios(tamañoPagina, cantidadRegistros);

            gvSitiosPaginador.setPaginaActual(paginaActual);

            gvSitios.DataSource = lista_sitios;
            gvSitios.DataBind();
        }

        protected void radioTrabajo_CheckedChanged(object sender, EventArgs e)
        {
            if (radioTrabajo.Checked == true)
            {
                txtSitio.Enabled = true;
                txtViatico.Enabled = true;
                cargar_tareas_estados("Trabajado");
                cargar_viaticos();
            }
        }

        protected void radioNoTrabajo_CheckedChanged(object sender, EventArgs e)
        {
            if (radioNoTrabajo.Checked == true)
            {
                txtSitio.Text = "";
                txtSitio.Enabled = false;
                lblSitio.Text = "";
                txtViatico.SelectedIndex = 0;
                txtViatico.Enabled = false;
                cargar_tareas_estados("No Trabajado");
                txtViatico.Items.Clear();
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        void limpiar()
        {
            foreach (GridViewRow row in grdEmpleadosAsignados.Rows)
            {
                CheckBox chk = (row.Cells[0].FindControl("cbox") as CheckBox);
                if (chk.Checked == true) chk.Checked = false;
            }

            txtFechaInicio.Text = DateTime.Now.ToShortDateString();
            txtViatico.SelectedIndex = 0;
            txtSitio.Text = "";
            lblSitio.Text = "";
            txtObservaciones.Text = "";

            radioTrabajo.Checked = true;
            txtEstado.SelectedIndex = 0;

            List<DAL.Empleados> empleados_asignados = new List<DAL.Empleados>();
            Session["lista_asignados"] = empleados_asignados;
            cargar_empleadosAsignados();
        }

        protected void btnTodos_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdEmpleadosAsignados.Rows)
            {
                CheckBox chk = (row.Cells[0].FindControl("cbox") as CheckBox);
                if (chk.Checked == false) chk.Checked = true;
            }
        }
                
        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            DAL.Tareas tareas = new DAL.Tareas();
            bool errores = false;

            DateTime Fecha1=DateTime.Now;
            DateTime Fecha2=DateTime.Now;

            List<DAL.Empleados> empleados_asignados = (List<DAL.Empleados>)Session["lista_asignados"];

            if (empleados_asignados.Count > 0)
            {
                try
                {
                    Fecha1 = Convert.ToDateTime(txtFechaInicio.Text);
                    Fecha2 = Fecha1;
                }
                catch
                {
                    errores = true;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("La fecha de inicio debe ser una fecha válida"), true);
                }

                TimeSpan ts = Fecha2 - Fecha1;
                int dias = ts.Days;

                //Busca superposición de fechas
                for (int i = 0; i <= dias; i++)
                {
                    foreach (var item in empleados_asignados)
	                {
		                DateTime fecha = Fecha1.AddDays(i);
                        if(tareas.esta_superpuesto(fecha,item.Id.ToString()))
                        {
                            errores = true;
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("No se pudo completar ya que el rango de fechas se superponen a otras ya asignadas"), true);
                            break;
                        }
	                }
                    
                }

                //Busca empleados con fecha de cierre
                foreach (DAL.Empleados item in empleados_asignados)
                {
                    try
                    {
                        if (Convert.ToDateTime(item.Fecha_Cierre) > Fecha1)
                        {
                            errores = true;
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("No se puede cargar una operacion anterior a la fecha de cierre establecida"), true);
                            break;
                        }
                    }
                    catch
                    {

                    }
                }

                if(txtEstado.SelectedValue.ToString()=="")
                {
                    errores = true;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe especificar una tarea"), true);
                }

                if(!errores)
                {
                    foreach (GridViewRow row in grdEmpleadosAsignados.Rows)
                    {
                        string trabajado = "";

                        if (radioTrabajo.Checked)
                            trabajado = "SI";
                        else
                            trabajado = "NO";

                        tareas.Emp_id = row.Cells[1].Text;
                        tareas.Empleado = row.Cells[3].Text + "; " + row.Cells[2].Text;
                        tareas.FechaInicio = txtFechaInicio.Text;
                        tareas.FechaFin = txtFechaInicio.Text;
                        tareas.Sitio_id = txtSitio.Text;
                        tareas.Sitio = "";
                        tareas.Trabajado = trabajado;
                        tareas.Observaciones = txtObservaciones.Text;
                        tareas.Estado = txtEstado.SelectedItem.Text;
                        tareas.Asignado_por = txtEmpleado.SelectedItem.Value;

                        tareas.Estado_id = txtEstado.SelectedItem.Value;
                        tareas.Viatico_id = txtViatico.SelectedItem.Value;

                        DAL.Viaticos viatico = new DAL.Viaticos();
                        tareas.Viatico = viatico.obtener_importe(txtViatico.SelectedItem.Value);

                        tareas.Guardar(true);
                    }

                    
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Se asignaron " + empleados_asignados.Count.ToString() + " empleados"), true);
                    
                    limpiar();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe seleccionar empleados para poder asignarlos"), true);
            }
        }

        protected void btnTodas_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdTareas.Rows)
            {
                CheckBox chk = (row.Cells[0].FindControl("cbox") as CheckBox);
                if (chk.Checked == false) chk.Checked = true;
            }
        }

        protected void btnDesasignar_Click(object sender, EventArgs e)
        {
            int desasignados = 0;
            bool errores = false;

            

            //Comprobar cantidad de registros a desasignar.
            foreach (GridViewRow row in grdTareas.Rows)
            {
                DateTime Inicio = Convert.ToDateTime(row.Cells[5].Text);
                CheckBox chkRow = (row.Cells[0].FindControl("cbox") as CheckBox);
                if (row.RowType == DataControlRowType.DataRow)
                    if (chkRow.Checked) desasignados++;
            }

            //Si hay elementos para desasignar
            if (desasignados > 0)
            {
                if(!errores)
                {
                    List<DAL.Tareas> lista_tareas = (List<DAL.Tareas>)Session["lista_tareas"];

                    foreach (GridViewRow row in grdTareas.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {

                            CheckBox chkRow = (row.Cells[0].FindControl("cbox") as CheckBox);
                            if (chkRow.Checked)
                            {
                                string id = (string)grdTareas.DataKeys[row.RowIndex].Values["TareaID"];

                                DAL.Tareas tareas = new DAL.Tareas();
                                tareas = tareas.obtener(id);
                                if(Session["usr"].ToString()!="telesoluciones")
                                {
                                    if (Session["id"].ToString() != tareas.Asignado_por)
                                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Error al intentar borrar la tarea, solo el usuario que la creo puede desasignarla"), true);
                                    else
                                        tareas.borrar(lista_tareas[row.RowIndex].TareaID);
                                }
                                else
                                {
                                    tareas.borrar(lista_tareas[row.RowIndex].TareaID);
                                }
                            }
                        }
                    }
                    cargar_tareas(false, obtenerOrdenActual_Tareas(), ObtenerItemFiltro_Tareas());
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Se desasignaron " + desasignados.ToString() + " tareas"), true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("No se pueden desasignar tareas con fechas ya cerradas"), true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe seleccionar tareas para poder desasignarlas"), true);
            }
        }

        protected void grdEmpleados_Sorting(object sender, GridViewSortEventArgs e)
        {
            
        }

        protected void grdTareasPaginador_Anterior()
        {
            grdTareasPaginador.setPaginaAnterior();
            cargar_tareas(false, obtenerOrdenActual_Tareas(), ObtenerItemFiltro_Tareas());
        }

        protected void grdTareasPaginador_Fin()
        {
            grdTareasPaginador.setPaginaFinal();
            cargar_tareas(false, obtenerOrdenActual_Tareas(), ObtenerItemFiltro_Tareas());
        }

        protected void grdTareasPaginador_Inicio()
        {
            grdTareasPaginador.setPaginaInicial();
            cargar_tareas(false, obtenerOrdenActual_Tareas(), ObtenerItemFiltro_Tareas());
        }

        protected void grdTareasPaginador_Proxima()
        {
            grdTareasPaginador.setPaginaSiguiente();
            cargar_tareas(false, obtenerOrdenActual_Tareas(), ObtenerItemFiltro_Tareas());
        }

        protected void grdTareasPaginador_PaginaSeleccionada(int paginaActual)
        {
            cargar_tareas(false, obtenerOrdenActual_Tareas(), ObtenerItemFiltro_Tareas());
        }

        protected void lnkName_Click(object sender, EventArgs e)
        {
            sorting("nombre");
        }

        protected void gvSitiosPaginador_Anterior()
        {
            gvSitiosPaginador.setPaginaAnterior();
            cargar_sitios_grid(false, obtenerOrdenActual_Sitios(), ObtenerItemFiltro_Sitios());
        }

        protected void gvSitiosPaginador_Fin()
        {
            gvSitiosPaginador.setPaginaFinal();
            cargar_sitios_grid(false, obtenerOrdenActual_Sitios(), ObtenerItemFiltro_Sitios());
        }

        protected void gvSitiosPaginador_Inicio()
        {
            gvSitiosPaginador.setPaginaInicial();
            cargar_sitios_grid(false, obtenerOrdenActual_Sitios(), ObtenerItemFiltro_Sitios());
        }

        protected void gvSitiosPaginador_Proxima()
        {
            gvSitiosPaginador.setPaginaSiguiente();
            cargar_sitios_grid(false, obtenerOrdenActual_Sitios(), ObtenerItemFiltro_Sitios());
        }

        protected void gvSitiosPaginador_PaginaSeleccionada(int paginaActual)
        {
            cargar_sitios_grid(false, obtenerOrdenActual_Sitios(), ObtenerItemFiltro_Sitios());
        }

        void sorting(string expression)
        {
            List<DAL.Empleados> empleados_asignados = (List<DAL.Empleados>)Session["lista_asignados"];
            if (ViewState["ordenAsignados_tipo"].ToString() == "Ascendente")
                ViewState["ordenAsignados_tipo"] = "Descendente";
            else
                ViewState["ordenAsignados_tipo"] = "Ascendente";

            if (ViewState["ordenAsignados_tipo"] != null)
            {
                if (ViewState["ordenAsignados_tipo"].ToString() == "Ascendente")
                {
                    if (expression == "id") empleados_asignados = empleados_asignados.OrderByDescending(o => o.Id).ToList();
                    if (expression == "grupo") empleados_asignados = empleados_asignados.OrderByDescending(o => o.Grupo).ToList();
                    if (expression == "apellido") empleados_asignados = empleados_asignados.OrderByDescending(o => o.Apellido).ToList();
                }
                else
                {
                    if (expression == "id") empleados_asignados = empleados_asignados.OrderBy(o => o.Id).ToList();
                    if (expression == "grupo") empleados_asignados = empleados_asignados.OrderBy(o => o.Grupo).ToList();
                    if (expression == "apellido") empleados_asignados = empleados_asignados.OrderBy(o => o.Apellido).ToList();
                }
            }

            cargar_empleadosAsignados();
        }

        void sortingTareas(string expression)
        {
            DAL.ItemOrden[] orden = new DAL.ItemOrden[1];
            orden[0] = new DAL.ItemOrden();

            if (ViewState["ordenTarea[0].TipoOrden"] != null)
            {
                orden[0].Campo = expression;
                if (ViewState["ordenTarea[0].Campo"].ToString() == expression)
                {
                    if (ViewState["ordenTarea[0].TipoOrden"].ToString() == DAL.TipoOrden.Ascendente.ToString())
                        orden[0].TipoOrden = DAL.TipoOrden.Descendente;
                    else
                        orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                }
                else
                {
                    orden[0].Campo = expression;
                    orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                }
            }
            else
            {
                orden[0] = new DAL.ItemOrden();
                orden[0].Campo = expression;
                orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
            }

            ViewState["ordenTarea[0].TipoOrden"] = orden[0].TipoOrden.ToString();
            ViewState["ordenTarea[0].Campo"] = orden[0].Campo;

            cargar_tareas(false, obtenerOrdenActual_Tareas(), ObtenerItemFiltro_Tareas());
        }

        protected void txtBuscarEmpleado_TextChanged(object sender, EventArgs e)
        {
            if(txtBuscarEmpleado.Text!="")
                cargar_empleados();
        }

        protected void lnkApellido_Click(object sender, EventArgs e)
        {
            sorting("apellido");
        }

        protected void lnkEmpleadoID_Click(object sender, EventArgs e)
        {
            sorting("id");
        }

        protected void btnLimpiarTareas_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdTareas.Rows)
            {
                CheckBox chk = (row.Cells[0].FindControl("cbox") as CheckBox);
                if (chk.Checked == true) chk.Checked = false;
            }
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            cargar_tareas(false, obtenerOrdenActual_Tareas(), ObtenerItemFiltro_Tareas());
        }

        protected void lnkTarea_ID_Click(object sender, EventArgs e)
        {
            sortingTareas("tarea_id");
        }

        protected void lnkTarea_empleado_Click(object sender, EventArgs e)
        {
            sortingTareas("empleado");
        }

        protected void lnkTarea_FechaInicio_Click(object sender, EventArgs e)
        {
            sortingTareas("fecha_inicio");
        }

        protected void lnkTarea_FechaFin_Click(object sender, EventArgs e)
        {
            sortingTareas("fecha_fin");
        }

        protected void lnkTarea_Sitio_Click(object sender, EventArgs e)
        {
            sortingTareas("sitio");
        }

        protected void lnkTarea_Viatico_Click(object sender, EventArgs e)
        {
            sortingTareas("viatico");
        }

        protected void txtSitio_TextChanged(object sender, EventArgs e)
        {
            if (txtSitio.Text != "")
                cargar_sitio();
            else
                lblSitio.Text = "";

            
        }

        protected void btnCerrarDialogoSitios_Click(object sender, EventArgs e)
        {
            frmBuscarSitio.Visible = false;
        }
        protected void btnCerrarDialogoEmpleados_Click(object sender, EventArgs e)
        {
            frmBuscarEmpleado.Visible = false;
            txtBuscarEmpleado.Text = "";
            txtEmpleadoGrupo.Text = "";
        }

        protected void txtBuscarSitioDialogo_TextChanged(object sender, EventArgs e)
        {
            cargar_sitios_grid(false, obtenerOrdenActual_Sitios(), ObtenerItemFiltro_Sitios());
        }

        protected void gvSitios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvSitios, "Select$" + e.Row.RowIndex.ToString()));
        }

        protected void gvSitios_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = gvSitios.SelectedDataKey.Value.ToString();
            txtSitio.Text = id;
            cargar_sitio();
            frmBuscarSitio.Visible = false;
        }

        protected void txtViatico_TextChanged(object sender, EventArgs e)
        {
            if (txtViatico.Text != "") cargar_viaticos();
        }

        protected void btnEmpleadosDialogoAsignar_Click(object sender, EventArgs e)
        {
            List<DAL.Empleados> lista_empleados = (List<DAL.Empleados>)Session["lista_empleados"];
            List<DAL.Empleados> empleados_asignados = Session["lista_asignados"] as List<DAL.Empleados>;
            int count = 0;
            int count_repetidos = 0; //se usa solo para el msg_toast

            foreach (GridViewRow row in grdEmpleados.Rows)
            {
                CheckBox chk = (row.Cells[0].FindControl("cbox") as CheckBox);
                string id = row.Cells[1].Text;
                if (chk.Checked == true)
                {
                    int index = empleados_asignados.FindIndex(item => item.Id.ToString() == id);
                    if (index < 0)
                        empleados_asignados.Add(lista_empleados[count]);
                    else
                        count_repetidos++;
                }
                count++;
            }

            if(count_repetidos==1)
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Un empleado no se agrego ya que existe en la lista de asignados"), true);
            if(count_repetidos>1)
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Algunos empleados no se agregaron ya que existen en la lista de asignados"), true);

            if(txtViatico.SelectedValue.ToString()=="2")
            {
                foreach (DAL.Empleados item in empleados_asignados)
                {
                    if (item.Provincia == "2")
                    {
                        txtViatico.SelectedValue = "5";
                    }
                }
            }

            frmBuscarEmpleado.Visible = false;
            cargar_empleadosAsignados();
        }

        protected void btnEmpleadosDialogoTodos_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdEmpleados.Rows)
            {
                CheckBox chk = (row.Cells[0].FindControl("cbox") as CheckBox);
                if (chk.Checked == false) chk.Checked = true;
            }
        }

        protected void btnEmpleadosDialogoLimpiar_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdEmpleados.Rows)
            {
                CheckBox chk = (row.Cells[0].FindControl("cbox") as CheckBox);
                if (chk.Checked == true) chk.Checked = false;
            }
        }

        protected void btnQuitar_Click(object sender, EventArgs e)
        {
            List<DAL.Empleados> empleados_asignados = (List<DAL.Empleados>)Session["lista_asignados"];
            foreach (GridViewRow row in grdEmpleadosAsignados.Rows)
            {
                CheckBox chk = (row.Cells[0].FindControl("cbox") as CheckBox);
                string id = row.Cells[1].Text;
                if (chk.Checked == true)
                {
                    int index = empleados_asignados.FindIndex(item => item.Id.ToString() == id);
                    if (index >= 0)
                        empleados_asignados.RemoveAt(index);
                }
            }

            frmBuscarEmpleado.Visible = false;
            cargar_empleadosAsignados();
        }

        protected void lnkEmpleadoGrupo_Click(object sender, EventArgs e)
        {
            sorting("grupo");
        }

        protected void btnCloseFrmSeleccionarTarea_Click(object sender, EventArgs e)
        {
            frmSeleccionarTarea.Visible = false;
        }

        protected void btnListar_Click(object sender, EventArgs e)
        {
            cargar_tareas(false, obtenerOrdenActual_Tareas(), ObtenerItemFiltro_Tareas());
            frmSeleccionarTarea.Visible = true;
        }

        protected void grdTareas_Sorting(object sender, GridViewSortEventArgs e)
        {
            DAL.ItemOrden[] ordenTarea = new DAL.ItemOrden[1];
            ordenTarea[0] = new DAL.ItemOrden();

            if (ViewState["ordenTarea[0].TipoOrden"] != null)
            {
                ordenTarea[0].Campo = e.SortExpression;
                if (ViewState["ordenTarea[0].Campo"].ToString() == e.SortExpression)
                {

                    if (ViewState["ordenTarea[0].TipoOrden"].ToString() == DAL.TipoOrden.Ascendente.ToString())
                        ordenTarea[0].TipoOrden = DAL.TipoOrden.Descendente;
                    else
                        ordenTarea[0].TipoOrden = DAL.TipoOrden.Ascendente;
                }
                else
                {
                    ordenTarea[0].Campo = e.SortExpression;
                    ordenTarea[0].TipoOrden = DAL.TipoOrden.Ascendente;
                }
            }
            else
            {
                ordenTarea[0] = new DAL.ItemOrden();
                ordenTarea[0].Campo = e.SortExpression;
                ordenTarea[0].TipoOrden = DAL.TipoOrden.Ascendente;
            }

            ViewState["ordenTarea[0].TipoOrden"] = ordenTarea[0].TipoOrden.ToString();
            ViewState["ordenTarea[0].Campo"] = ordenTarea[0].Campo;

            cargar_tareas(false, obtenerOrdenActual_Tareas(), ObtenerItemFiltro_Tareas());
        }

        protected void txtEmpleadoGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtEmpleadoGrupo.SelectedValue.ToString()!="")
            {
                List<DAL.Empleados> lista_empleados = (List<DAL.Empleados>)Session["lista_empleados"];
                DAL.Empleados emp = new DAL.Empleados();
                lista_empleados = emp.Obtener_miembros_grupos(txtEmpleadoGrupo.Text, true);
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
                    txtEmpleadoGrupo.Text = "";
                }
                txtEmpleadoGrupo.SelectedIndex = 0;
            }
        }

        protected void grdTareas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grdTareas, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.Cells[1].Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grdTareas, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.Cells[2].Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grdTareas, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.Cells[3].Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grdTareas, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.Cells[4].Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grdTareas, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.Cells[5].Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grdTareas, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.Cells[6].Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grdTareas, "Select$" + e.Row.RowIndex.ToString()));
            }
                
          
        }

        protected void grdTareas_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            int.TryParse(grdTareas.SelectedDataKey.Value.ToString(), out id);
            if (id != 0)
            {
                DAL.Tareas tarea = new DAL.Tareas();
                tarea = tarea.obtener(id.ToString());

                if (Session["usr"].ToString() == "telesoluciones")
                {
                    if (ViewState["TareaID"] != null)
                        ViewState["TareaID"] = id.ToString();
                    else
                        ViewState.Add("TareaID", id.ToString());

                    frmModificarViatico.Visible = true;
                    lblEmpleadoModViatico.Text = tarea.Empleado;
                    txtModViatico_Importe.Text = "";
                }
                
            }
        }

        protected void frmModificarViatico_Aceptar_Click(object sender, EventArgs e)
        {
            decimal importe =0;
            try 
	        {	        
		        importe = Convert.ToDecimal(txtModViatico_Importe.Text);
	        }
	        catch
	        {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe ingresar un valor numerico válido"), true);
	        }

            if(importe>0)
            {
                DAL.Tareas tarea = new DAL.Tareas();
                tarea = tarea.obtener(ViewState["TareaID"].ToString());

                tarea.Viatico = importe;
                tarea.Guardar(false);
                cargar_tareas(false, obtenerOrdenActual_Tareas(), ObtenerItemFiltro_Tareas());
                frmModificarViatico.Visible = false;
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("El importe debe ser mayor a cero"), true);
            }
        }

        protected void frmModificarViatico_Cerrar_Click(object sender, EventArgs e)
        {
            frmModificarViatico.Visible = false;
        }

        protected void cbox_CheckedChanged(object sender, EventArgs e)
        {
            //frmModificarViatico.Visible = false;
        }

        protected void btnFiltar_Click(object sender, EventArgs e)
        {
            cargar_tareas(false, obtenerOrdenActual_Tareas(), ObtenerItemFiltro_Tareas());
        }
        
    }
}