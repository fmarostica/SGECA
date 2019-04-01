using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGECA.Forms.Empleados
{
    public partial class frmViaticos : System.Web.UI.Page, LogManager.IObserver, LogManager.ISubject
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Master.EnableViewState = true;

            if(!this.IsPostBack)
            {
                frmSeleccionar.Visible = false;
                txtFecha.Text = DateTime.Now.ToShortDateString();
                cargar_empleados();
                cargar_empleados_asignadores();

                if (Session["usr"].ToString() != "telesoluciones")
                {
                    txtAsignadoPor.Enabled = false;
                    txtAsignadoPor.SelectedValue = Session["id"].ToString();
                }
            }
        }

        void cargar_empleados_asignadores()
        {
            DAL.Empleados emp = new DAL.Empleados();
            List<DAL.Empleados> lista_empleados = new List<DAL.Empleados>();

            lista_empleados.Add(new DAL.Empleados { Id = 0, ApellidoyNombre = "" });

            lista_empleados.AddRange(emp.Obtener_lista("ACTIVOS"));

            txtAsignadoPor.DataSource = lista_empleados;
            txtAsignadoPor.DataTextField = "ApellidoyNombre";
            txtAsignadoPor.DataValueField = "Id";
            txtAsignadoPor.DataBind();
        }

        void cargar_empleados()
        {
            DAL.Empleados emp = new DAL.Empleados();
            List<DAL.Empleados> lista_empleados = new List<DAL.Empleados>();

            lista_empleados.Add(new DAL.Empleados { Id = 0, ApellidoyNombre = "" });

            lista_empleados.AddRange(emp.Obtener_lista_adelantos(true));

            txtEmpleado.DataSource = lista_empleados;
            txtEmpleado.DataTextField = "ApellidoyNombre";
            txtEmpleado.DataValueField = "Id";
            txtEmpleado.DataBind();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        void limpiar()
        {
            ViewState["adelanto_id"] = null;
            txtDescripcion.Text = "";
            txtFecha.Text = DateTime.Now.ToShortDateString();
            txtImporte.Text = "";
            txtEmpleado.SelectedIndex = 0;
            txtAsignadoPor.SelectedIndex = 0;
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {

            guardarDatos(true);
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            if(ViewState["adelanto_id"]!=null)
                guardarDatos(false);
            else
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe seleccionar un registro para poder guardar los datos"), true);
        }

        void guardarDatos(bool nuevo)
        {
            DateTime fecha = DateTime.Now;
            decimal importe = 0;
            bool errores = false;

            try
            {
                fecha = Convert.ToDateTime(txtFecha.Text);

                
            }
            catch
            {
                errores = true;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe ingresar una fecha válida."), true);
            }
            

            if(!decimal.TryParse(txtImporte.Text.Replace(".",","), out importe))
            {
                errores = true;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe ingresar valor númerico válido en el campo importe."), true);
            }

            if (txtEmpleado.SelectedValue.ToString() == "0")
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe especificar un empleado."), true);
                errores = true;
            }

            if (txtAsignadoPor.SelectedValue.ToString() == "0")
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("El campo Asignado Por no puede ser nulo"), true);
                errores = true;
            }

            DAL.Empleados emp = new DAL.Empleados();
            emp = emp.Obtener(txtEmpleado.SelectedValue.ToString());

            if(emp.Fecha_Cierre!=null && emp.Fecha_Cierre!="")
            {
                if (Convert.ToDateTime(emp.Fecha_Cierre) >= fecha)
                {
                    errores = true;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("No se puede cargar una operacion anterior a la fecha de cierre establecida"), true);
                }
            }

            if (!errores)
            {
                DAL.Empleados_Adelantos emp_adelantos = new DAL.Empleados_Adelantos();
                if(!nuevo) emp_adelantos.adelanto_id = Convert.ToInt32(ViewState["adelanto_id"].ToString());
                emp_adelantos.fecha = txtFecha.Text;
                emp_adelantos.empleado_id = Convert.ToInt32(txtEmpleado.SelectedItem.Value);
                emp_adelantos.descripcion = txtDescripcion.Text;
                emp_adelantos.importe = importe;
                emp_adelantos.asignado_por = txtAsignadoPor.SelectedValue.ToString();

                emp_adelantos.Guardar(nuevo);

                limpiar();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("Operacion Realizada", 3000), true);
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];

            if (confirmValue == "Si")
            {
                if (ViewState["adelanto_id"] != null)
                {
                    DAL.Empleados_Adelantos emp_adelanto = new DAL.Empleados_Adelantos();
                    emp_adelanto = emp_adelanto.obtener_datos(ViewState["adelanto_id"].ToString());

                    DAL.Empleados emp = new DAL.Empleados();
                    emp = emp.Obtener(emp_adelanto.empleado_id.ToString());

                    if(emp.Fecha_Cierre!=null && emp.Fecha_Cierre!="")
                    {
                        DateTime fecha_cierre = Convert.ToDateTime(emp.Fecha_Cierre);
                        DateTime fecha_adelanto = Convert.ToDateTime(emp_adelanto.fecha);

                        if(fecha_adelanto>fecha_cierre)
                        {
                            emp_adelanto.borrar(Convert.ToInt32(ViewState["adelanto_id"]));
                            obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
                            limpiar();
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("Operacion Realizada", 3000), true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("El registro no puede ser borrado ya que la fecha de registro es inferior a la fecha de cierre", 3000), true);
                        }
                    }
                    else
                    {
                        emp_adelanto.borrar(Convert.ToInt32(ViewState["adelanto_id"]));
                        obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
                        limpiar();
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("Operacion Realizada", 3000), true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("Debe seleccionar un registro para poder borrarlo", 3000), true);
                }
            }
        }

        protected void grdAdelantos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grdAdelantos, "Select$" + e.Row.RowIndex.ToString()));
        }

        protected void grdAdelantos_SelectedIndexChanged(object sender, EventArgs e)
        {
            frmSeleccionar.Visible = false;
            int id = 0;
            int.TryParse(grdAdelantos.SelectedDataKey.Value.ToString(), out id);
            if (id != 0)
            {
                DAL.Empleados_Adelantos emp_adelanto = new DAL.Empleados_Adelantos();
                emp_adelanto = emp_adelanto.obtener_datos(id.ToString());

                if (ViewState["adelanto_id"] != null)
                    ViewState.Add("adelanto_id", emp_adelanto.adelanto_id);
                else
                    ViewState.Add("adelanto_id", emp_adelanto.adelanto_id);

                txtDescripcion.Text = emp_adelanto.descripcion;
                txtEmpleado.SelectedValue = emp_adelanto.empleado_id.ToString();
                txtFecha.Text = emp_adelanto.fecha;
                txtImporte.Text = emp_adelanto.importe.ToString("F2");
                if (emp_adelanto.asignado_por != null && emp_adelanto.asignado_por!="")
                    txtAsignadoPor.SelectedValue = emp_adelanto.asignado_por.ToString();
                else
                    txtAsignadoPor.SelectedIndex = 0;
                    
            }
        }

        protected void grdAdelantos_Sorting(object sender, GridViewSortEventArgs e)
        {
            DAL.ItemOrden[] orden = new DAL.ItemOrden[1];
            orden[0] = new DAL.ItemOrden();

            if (ViewState["orden[0].TipoOrden"] != null)
            {
                orden[0].Campo = e.SortExpression;
                if (ViewState["orden[0].Campo"].ToString() == e.SortExpression)
                {

                    if (ViewState["orden[0].TipoOrden"].ToString() == DAL.TipoOrden.Ascendente.ToString())
                        orden[0].TipoOrden = DAL.TipoOrden.Descendente;
                    else
                        orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                }
                else
                {
                    orden[0].Campo = e.SortExpression;
                    orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                }
            }
            else
            {
                orden[0] = new DAL.ItemOrden();
                orden[0].Campo = e.SortExpression;
                orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
            }

            ViewState["orden[0].TipoOrden"] = orden[0].TipoOrden.ToString();
            ViewState["orden[0].Campo"] = orden[0].Campo;

            obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
        }

        private void obtenerDatosFiltrados(bool todos, DAL.ItemOrden[] orden, DAL.ItemFiltro[] filtro)
        {
            int paginaActual = pagPaginador.obtenerPaginaActual();

            int tamañoPagina = pagPaginador.obtenerRegistrosMostrar();

            int registroInicio = ((paginaActual - 1) * tamañoPagina) + 1;

            int registroFin;
            if (todos)
                registroFin = -1;
            else
                registroFin = tamañoPagina * paginaActual;

            DAL.Empleados_Adelantos adelantos = new DAL.Empleados_Adelantos();

            adelantos.Subscribe(this);

            double cantidadRegistros = 0;

            List<DAL.Empleados_Adelantos> datosObtenidos = adelantos.obtenerFiltrado(filtro,
                                                   orden,
                                                   true,
                                                   registroInicio,
                                                   registroFin,
                                                   out  cantidadRegistros);
            if (adelantos.UltimoMensaje != null)
            {
                UltimoMensaje = adelantos.UltimoMensaje;
                Notify(UltimoMensaje);
                return;
            }

            grdAdelantos.DataSource = datosObtenidos;
            grdAdelantos.DataBind();

            calcularTotalPaginas(tamañoPagina, cantidadRegistros);

            pagPaginador.setPaginaActual(paginaActual);
        }

        private void calcularTotalPaginas(int tamañoPagina, double cantidadRegistros)
        {
            int totalPaginas = (int)Math.Ceiling(int.Parse(cantidadRegistros.ToString()) / (double)tamañoPagina);

            pagPaginador.setCantidadRegistros(cantidadRegistros);
            pagPaginador.setTotalPaginas(totalPaginas);
        }


        internal DAL.ItemFiltro[] ObtenerItemFiltro()
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

            if(txtDesde.Text!="" && txtHasta.Text!="")
            {
                fil = new DAL.ItemFiltro();
                fil.itemBusqueda = new DAL.ItemBusqueda();
                fil.itemBusqueda.campo = "fecha";
                fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._datetime;
                fil.itemBusqueda.Value = "fecha";
                fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
                fil.tipoFiltroTexto.value = DAL.TipoFiltro.Between;
                fil.textoBusqueda = txtDesde.Text;
                fil.textoBusqueda2 = txtHasta.Text;
                items.Add(fil);
            }

            return items.ToArray<DAL.ItemFiltro>();
        }



        private DAL.ItemOrden[] obtenerOrdenActual()
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
                orden[0].Campo = "fecha";
                orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                ViewState["orden[0].TipoOrden"] = DAL.TipoOrden.Ascendente.ToString();
                ViewState["orden[0].Campo"] = orden[0].Campo;
            }
            return new DAL.ItemOrden[0];

        }

        protected void pagPaginador_Fin()
        {
            pagPaginador.setPaginaFinal();
            obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void pagPaginador_Inicio()
        {
            pagPaginador.setPaginaInicial();
            obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void pagPaginador_Anterior()
        {
            pagPaginador.setPaginaAnterior();
            obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void pagPaginador_Proxima()
        {

            pagPaginador.setPaginaSiguiente();
            obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void pagPaginador_PaginaSeleccionada(int paginaActual)
        {
            obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void txtDesde_TextChanged(object sender, EventArgs e)
        {
            
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void btnSeleccionarClose_Click(object sender, EventArgs e)
        {

        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
            frmSeleccionar.Visible = true;
        }

        protected void btnSeleccionarClose_Click1(object sender, EventArgs e)
        {
            frmSeleccionar.Visible = false;
        }

        protected void btnSeleccionarClose_Click2(object sender, EventArgs e)
        {
            frmSeleccionar.Visible = false;
        }

    }
}