using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGECA.Forms.RRHH
{
    public partial class frmGastos : System.Web.UI.Page
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
            this.EnableViewState = true;
            if (!this.IsPostBack)
            {
                frmSeleccionar.Visible = false;

                txtFecha.Text = DateTime.Now.ToShortDateString();

                txtBuscar.Attributes.Add("placeholder", "Ingrese el texto a buscar");

                txtFiltroTipo.Items.Add("TODOS");
                txtFiltroTipo.Items.Add("COMBUSTIBLE");
                txtFiltroTipo.Items.Add("ESTACIONAMIENTO");
                txtFiltroTipo.Items.Add("PEAJE");
                txtFiltroTipo.Items.Add("HERRAMIENTAS");
                txtFiltroTipo.Items.Add("LAVADERO");
                txtFiltroTipo.Items.Add("GASTOS BANCARIOS");
                txtFiltroTipo.Items.Add("OTROS");

                txtTipo.Items.Add("");
                txtTipo.Items.Add("COMBUSTIBLE");
                txtTipo.Items.Add("ESTACIONAMIENTO");
                txtTipo.Items.Add("PEAJE");
                txtTipo.Items.Add("HERRAMIENTAS");
                txtTipo.Items.Add("LAVADERO");
                txtTipo.Items.Add("GASTOS BANCARIOS");
                txtTipo.Items.Add("OTROS");

                this.Page.Master.EnableViewState = true;

                cargar_grupos();
                cargar_empleados_asignadores();

                if(Session["usr"].ToString()!="telesoluciones")
                {
                    txtAsignadoPor.Enabled = false;
                    txtAsignadoPor.SelectedValue = Session["id"].ToString();
                }
            }
        }

        void cargar_grupos()
        {
            DAL.EmpleadosGrupos grupos = new DAL.EmpleadosGrupos();
            List<DAL.EmpleadosGrupos> lista_grupos = new List<DAL.EmpleadosGrupos>();
            lista_grupos.Add(new DAL.EmpleadosGrupos { Id = "0", Nombre = "" });
            lista_grupos.AddRange(grupos.obtener());

            txtGrupo.DataSource = lista_grupos;
            txtGrupo.DataValueField = "id";
            txtGrupo.DataTextField = "nombre";
            txtGrupo.DataBind();
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

        void limpiar()
        {
            txtFecha.Text = DateTime.Now.ToShortDateString();
            txtGrupo.SelectedIndex = 0;
            txtImporte.Text = "";
            txtTipo.SelectedIndex = 0;
            if (Session["id"] != null)
                txtAsignadoPor.SelectedValue = Session["id"].ToString();
            else
                txtAsignadoPor.SelectedIndex = 0;
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

            DAL.Empleados_Gastos VistaTareas = new DAL.Empleados_Gastos();

            //VistaTareas.Subscribe(this);

            double cantidadRegistros = 0;

            List<DAL.Empleados_Gastos> datosObtenidos = VistaTareas.obtenerFiltrado(filtro,
                                                   orden,
                                                   true,
                                                   registroInicio,
                                                   registroFin,
                                                   out  cantidadRegistros);
            if (VistaTareas.UltimoMensaje != null)
            {
                UltimoMensaje = VistaTareas.UltimoMensaje;
                Notify(UltimoMensaje);
                return;
            }

            cargarGrilla(datosObtenidos);
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
            fil.itemBusqueda.campo = "nombre";
            fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
            fil.itemBusqueda.Value = "nombre";
            fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
            fil.tipoFiltroTexto.value = DAL.TipoFiltro.Like;
            fil.textoBusqueda = txtBuscar.Text;
            items.Add(fil);

            if (txtFiltroTipo.Text != "TODOS")
            {
                fil = new DAL.ItemFiltro();
                fil.itemBusqueda = new DAL.ItemBusqueda();
                fil.itemBusqueda.campo = "detalles";
                fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
                fil.itemBusqueda.Value = "detalles";
                fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
                fil.tipoFiltroTexto.value = DAL.TipoFiltro.Equal;
                fil.textoBusqueda = txtFiltroTipo.Text;
                items.Add(fil);
            }

            if (txtBuscarDesde.Text != "" || txtBuscarHasta.Text != "")
            {
                DateTime desde = DateTime.Now;
                DateTime hasta = DateTime.Now;

                DateTime.TryParse(txtBuscarDesde.Text, out desde);
                DateTime.TryParse(txtBuscarHasta.Text, out hasta);

                fil = new DAL.ItemFiltro();
                fil.itemBusqueda = new DAL.ItemBusqueda();
                fil.itemBusqueda.campo = "fecha";
                fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._datetime;
                fil.itemBusqueda.Value = "fecha";
                fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
                fil.tipoFiltroTexto.value = DAL.TipoFiltro.Between;
                fil.textoBusqueda = desde.ToShortDateString();
                fil.textoBusqueda2 = hasta.ToShortDateString();
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
                orden[0].Campo = "detalles";
                orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                ViewState["orden[0].TipoOrden"] = DAL.TipoOrden.Ascendente.ToString();
                ViewState["orden[0].Campo"] = orden[0].Campo;
            }
            return new DAL.ItemOrden[0];

        }

        void cargarGrilla(List<DAL.Empleados_Gastos> lista)
        {
            grdEstados.DataSource = lista;
            grdEstados.DataBind();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            bool errores = false;

            if (confirmValue == "Si")
            {
                if (ViewState["id"] != null)
                {
                    DAL.Empleados_Gastos gastos = new DAL.Empleados_Gastos();
                    gastos = gastos.obtener_gastos(ViewState["id"].ToString());

                    DAL.EmpleadosGrupos grupo = new DAL.EmpleadosGrupos();
                    List<DAL.Empleados> lista_empleados = grupo.obtener_miembros(gastos.grupo_id);

                    foreach (DAL.Empleados item in lista_empleados)
                    {
                        try 
	                    {
                            DateTime fecha_cierre = Convert.ToDateTime(item.Fecha_Cierre);
                            if (fecha_cierre >= Convert.ToDateTime(gastos.fecha))
                            {
                                errores = true;
                                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("No se puede borrar una operacion anterior a la fecha de cierre establecida"), true);
                            }
	                    }
	                    catch (Exception)
	                    {

	                    }
                    }
                    
                    if(!errores)
                    {
                        gastos.borrar(ViewState["id"].ToString());
                        limpiar();
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Operacion realizada!", 3000), true);
                        obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe seleccionar un registro para poder borrarlo", 3000), true);
                }
            }
        }

        void guardar(bool nuevo)
        {
            decimal importe = 0;
            decimal.TryParse(txtImporte.Text.Replace(".",","), out importe);
            DateTime fecha = DateTime.Now;
            bool errores = false;

            if (txtFecha.Text == "")
            {
                if (txtFecha.Text == "") lblFecha.ForeColor = Color.Red;
                errores = true;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Los campos señalados en rojo no pueden estar vacíos", 3000), true);
            }
            else
            {
                try
                {
                    fecha = Convert.ToDateTime(txtFecha.Text);
                }
                catch
                {
                    errores = true;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("La fecha debe ser válida!", 3000), true);
                }
            }

            if(importe<=0)
            {
                errores = true;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("El importe debe ser mayor a cero", 3000), true);
            }

            if(txtAsignadoPor.SelectedValue.ToString()=="0")
            {
                errores = true;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe seleccionar la persona que esta asignando", 3000), true);
            }

            if(txtTipo.SelectedIndex==0)
            {
                errores = true;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe especificar el gasto", 3000), true);
            }

            if (txtGrupo.SelectedValue.ToString() == "0")
            {
                errores = true;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe especificar el grupo", 3000), true);
            }

            DAL.EmpleadosGrupos grupo = new DAL.EmpleadosGrupos();
            List<DAL.Empleados> lista_empleados = grupo.obtener_miembros(txtGrupo.SelectedValue.ToString());

            foreach (DAL.Empleados item in lista_empleados)
            {
                try
                {
                    if (Convert.ToDateTime(item.Fecha_Cierre) >= fecha)
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

            if (!errores)
            {
                DAL.Empleados_Gastos gastos = new DAL.Empleados_Gastos();
                if(!nuevo) gastos.id = ViewState["id"].ToString();
                gastos.fecha = txtFecha.Text;
                gastos.detalles = txtTipo.Text;
                gastos.importe = importe;
                gastos.grupo_id = txtGrupo.SelectedItem.Value;
                gastos.asignado_por = txtAsignadoPor.SelectedValue.ToString();

                if (gastos.Guardar(nuevo))
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Operacion realizada!", 3000), true);
                else
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Error al realizar el proceso", 3000), true);

                limpiar();
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            guardar(true);
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
        }

        protected void btnClosefrmAgregar_Click(object sender, EventArgs e)
        {

        }


        protected void grdEstados_Sorting(object sender, GridViewSortEventArgs e)
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

        protected void pagPaginador_Anterior()
        {
            pagPaginador.setPaginaAnterior();
            obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
        }

        protected void pagPaginador_Fin()
        {
            pagPaginador.setPaginaFinal();
            obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
        }

        protected void pagPaginador_Inicio()
        {
            pagPaginador.setPaginaInicial();
            obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
        }

        protected void pagPaginador_Proxima()
        {
            pagPaginador.setPaginaSiguiente();
            obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
        }

        protected void pagPaginador_PaginaSeleccionada(int paginaActual)
        {
            obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            if (ViewState["id"] != null)
                guardar(false);
            else
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe seleccionar un registro para poder editarlo", 3000), true);
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
            frmSeleccionar.Visible = true;
        }

        protected void btnSeleccionarClose_Click(object sender, EventArgs e)
        {
            frmSeleccionar.Visible = false;
        }

        protected void grdEstados_Sorted(object sender, EventArgs e)
        {

        }

        protected void grdEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            frmSeleccionar.Visible = false;

            ViewState["id"] = grdEstados.SelectedDataKey.Value.ToString();

            DAL.Empleados_Gastos gastos = new DAL.Empleados_Gastos();
            gastos = gastos.obtener_gastos(ViewState["id"].ToString());

            txtFecha.Text = gastos.fecha;
            txtImporte.Text = gastos.importe.ToString();
            txtTipo.SelectedValue = gastos.detalles;
            txtGrupo.SelectedValue = gastos.grupo_id;
            if(gastos.asignado_por==null || gastos.asignado_por=="")
                txtAsignadoPor.SelectedIndex=0;
            else
                txtAsignadoPor.SelectedValue = gastos.asignado_por;
        }

        protected void grdEstados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grdEstados, "Select$" + e.Row.RowIndex.ToString()));
            }
        }

        protected void txtFiltroTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
        }

        protected void btnFiltar_Click(object sender, EventArgs e)
        {
            obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
        }
    }
}