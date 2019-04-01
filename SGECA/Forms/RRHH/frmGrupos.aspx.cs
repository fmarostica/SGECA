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
    public partial class frmGrupos : System.Web.UI.Page, LogManager.IObserver, LogManager.ISubject
    {
        DAL.Empleados_logs logs = new DAL.Empleados_logs();

        protected void Page_Load(object sender, EventArgs e)
        {
            txtBuscar.Attributes.Add("placeholder", "Ingrese el texto a buscar");
            if (!this.IsPostBack)
            {
                frmSeleccionar.Visible = false;
                this.Page.Master.EnableViewState = true;
                cargar_empleados();
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

            DAL.EmpleadosGrupos grupo = new DAL.EmpleadosGrupos();

            //VistaTareas.Subscribe(this);

            double cantidadRegistros = 0;

            List<DAL.EmpleadosGrupos> datosObtenidos = grupo.obtenerFiltrado(filtro,
                                                   orden,
                                                   true,
                                                   registroInicio,
                                                   registroFin,
                                                   out  cantidadRegistros);
            if (grupo.UltimoMensaje != null)
            {
                UltimoMensaje = grupo.UltimoMensaje;
                Notify(UltimoMensaje);
                return;
            }

            ArrayList lista = new ArrayList();
            foreach (DAL.EmpleadosGrupos item in datosObtenidos)
            {
                var itemLista = new
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                };
                lista.Add(itemLista);
            }

            cargarGrilla(lista);
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
                orden[0].Campo = "nombre";
                orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                ViewState["orden[0].TipoOrden"] = DAL.TipoOrden.Ascendente.ToString();
                ViewState["orden[0].Campo"] = orden[0].Campo;
            }
            return new DAL.ItemOrden[0];

        }

        void cargarGrilla(ArrayList lista)
        {
            grdGrupos.DataSource = lista;
            grdGrupos.DataBind();
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

        void limpiar()
        {
            txtCodigo.Text = "";
            txtDescripcion.Text = "";
            ViewState["GrupoID"] = null;
            txtEmpleado.SelectedIndex = 0;

            cargar_empleados();
        }

        void cargar_empleados()
        {
            DAL.Empleados emp = new DAL.Empleados();
            List<DAL.Empleados> lista_empleados = new List<DAL.Empleados>();

            lista_empleados.Add(new DAL.Empleados { Apellido = "", ApellidoyNombre = "", Id = 0 });

            lista_empleados.AddRange(emp.Obtener_lista("ACTIVOS"));

            txtEmpleado.DataSource = lista_empleados;
            txtEmpleado.DataTextField = "ApellidoyNombre";
            txtEmpleado.DataValueField = "Id";
            txtEmpleado.DataBind();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            guardar(true);
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            guardar(false);
        }

        void guardar(bool nuevo)
        {
            bool errores = false;

            if (nuevo)
            {
                DAL.EmpleadosGrupos grupos = new DAL.EmpleadosGrupos();
                int registros = 0;
                
                if(txtCodigo.Text!="") registros = grupos.obtener(txtCodigo.Text).Count;

                if (registros > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("La operación no pudo realizarse ya que el código ingresado existe en la base de datos!.", 3000), true);
                    errores = true;
                }
            }

            if (ViewState["GrupoID"] == null || ViewState["GrupoID"].ToString()=="")
            {
                errores = true;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe seleccionar un grupo para poder editarlo", 3000), true);
            }

            if(txtEmpleado.SelectedValue==null || txtEmpleado.SelectedValue.ToString()=="0")
            {
                errores = true;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe seleccionar un lider para el grupo", 3000), true);
            }

            if (txtDescripcion.Text == "")
            {
                if (txtDescripcion.Text == "") lblDescripcion.ForeColor = Color.Red;
                errores = true;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Los campos señalados en rojo no pueden estar vacíos", 3000), true);
            }

            DAL.Empleados emp = new DAL.Empleados();
            emp = emp.Obtener(txtEmpleado.SelectedValue.ToString());

            if(emp.es_lider(emp.Id.ToString()))
            {
                errores = true;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("El empleado seleccionado ya es lider de un grupo, elimine el grupo o bien cambie de lider e intentelo nuevamente", 3000), true);
            }



            int last_id = 0;

            if (!errores)
            {
                lblDescripcion.ForeColor = Color.Black;

                DAL.EmpleadosGrupos grupos = new DAL.EmpleadosGrupos();
                grupos.Id = txtCodigo.Text;
                grupos.Nombre = txtDescripcion.Text;
                grupos.lider = txtEmpleado.SelectedValue.ToString();

                if (grupos.Guardar(nuevo, out last_id))
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Operacion realizada!", 3000), true);
                else
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Error al realizar el proceso", 3000), true);

                if(nuevo)
                {
                    logs.modulo = "RRHH - GRUPOS";
                    logs.accion = "AGREGO EL GRUPO: " + grupos.Nombre.ToUpper();
                    logs.usuario = Session["usr"].ToString();
                    if (Session["id"] != null) logs.empleado_id = Session["id"].ToString();
                    logs.guardar();

                    emp.Grupo = last_id.ToString();
                    emp.Guardar(false);
                }
                else
                {
                    logs.modulo = "RRHH - GRUPOS";
                    logs.accion = "MODIFICO EL GRUPO: " + grupos.Nombre.ToUpper();
                    logs.usuario = Session["usr"].ToString();
                    if (Session["id"] != null) logs.empleado_id = Session["id"].ToString();
                    logs.guardar();

                    emp.Grupo = txtCodigo.Text;
                    emp.Guardar(false);
                }

                limpiar();
            }

        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (ViewState["GrupoID"] != null)
            {
                DAL.EmpleadosGrupos grupos = new DAL.EmpleadosGrupos();
                if (!grupos.existe_en_otros_registros(ViewState["GrupoID"].ToString()))
                {
                    grupos.borrar(ViewState["GrupoID"].ToString());
                    obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
                    limpiar();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("No se pudo completar la operacion ya que existe en otros registros", 3000), true);
                }
                
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe seleccionar un registro para poder borrarlo", 3000), true);
            }
        }

        protected void grdGrupos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grdGrupos, "Select$" + e.Row.RowIndex.ToString()));
        }

        protected void grdGrupos_SelectedIndexChanged(object sender, EventArgs e)
        {
            frmSeleccionar.Visible = false;
            DAL.EmpleadosGrupos grupos = new DAL.EmpleadosGrupos();
            string codigo = grdGrupos.SelectedDataKey.Value.ToString();
            List<DAL.EmpleadosGrupos> datos = grupos.obtener(codigo);
            foreach (var item in datos)
            {
                if (ViewState["GrupoID"] != null)
                    ViewState["GrupoID"] = item.Id;
                else
                    ViewState.Add("GrupoID", item.Id);

                txtCodigo.Text = item.Id.ToString();
                txtDescripcion.Text = item.Nombre;
                if (item.lider != null && item.lider != "")
                    txtEmpleado.SelectedValue = item.lider;
                else
                    txtEmpleado.SelectedIndex = 0;
            }
        }

        protected void btnSeleccionarClose_Click(object sender, EventArgs e)
        {
            frmSeleccionar.Visible = false;
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
            frmSeleccionar.Visible = true;
        }

        protected void txtEmpleado_SelectedIndexChanged(object sender, EventArgs e)
        {
            DAL.Empleados emp = new DAL.Empleados();
            emp = emp.Obtener(txtEmpleado.SelectedValue.ToString());

            txtDescripcion.Text = emp.Apellido.ToUpper();
        }
    }
}