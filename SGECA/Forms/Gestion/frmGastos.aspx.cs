using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGECA.Forms.Gestion
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
            if (!this.IsPostBack)
            {
                frmSeleccionar.Visible = false;
            }
        }

        void cargarGrilla(List<DAL.Tipos_Gastos> lista)
        {
            grdEstados.DataSource = lista;
            grdEstados.DataBind();
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

            DAL.Tipos_Gastos VistaTareas = new DAL.Tipos_Gastos();

            //VistaTareas.Subscribe(this);

            double cantidadRegistros = 0;

            List<DAL.Tipos_Gastos> datosObtenidos = VistaTareas.obtenerFiltrado(filtro,
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
            fil.itemBusqueda.campo = "detalles";
            fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
            fil.itemBusqueda.Value = "detalles";
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
                orden[0].Campo = "detalles";
                orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                ViewState["orden[0].TipoOrden"] = DAL.TipoOrden.Ascendente.ToString();
                ViewState["orden[0].Campo"] = orden[0].Campo;
            }
            return new DAL.ItemOrden[0];

        }

        void limpiar()
        {
            ViewState["TipoGastoID"] = null;
            txtDetalles.Text = "";
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
            if(ViewState["TipoGastoID"]!=null)
                guardar(false);
            else
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe seleccionar un registro para poder modificarlo.", 3000), true);
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (ViewState["TipoGastoID"] != null)
            {
                DAL.Tipos_Gastos gasto = new DAL.Tipos_Gastos();
                gasto.borrar(ViewState["TipoGastoID"].ToString());
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Operación realizada!.", 3000), true);
                limpiar();
            }
            else
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe seleccionar un registro para poder borrarlo.", 3000), true);
        }

        void guardar(bool nuevo)
        {
            if(txtDetalles.Text!="")
            {
                DAL.Tipos_Gastos gasto = new DAL.Tipos_Gastos();
                if (!nuevo) gasto.id = Convert.ToInt32(ViewState["TipoGastoID"].ToString());
                gasto.detalles = txtDetalles.Text;

                gasto.guardar(nuevo);

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Operación realizada!.", 3000), true);
                limpiar();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe escribir el nombre del gasto.", 3000), true);
            }
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            frmSeleccionar.Visible = true;
            obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void btnSeleccionarClose_Click(object sender, EventArgs e)
        {
            frmSeleccionar.Visible = false;
        }

        protected void pagPaginador_Anterior()
        {
            pagPaginador.setPaginaAnterior();
            obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void pagPaginador_Inicio()
        {
            pagPaginador.setPaginaInicial();
            obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void pagPaginador_Fin()
        {
            pagPaginador.setPaginaFinal();
            obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void pagPaginador_Proxima()
        {
            pagPaginador.setPaginaSiguiente();
            obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void pagPaginador_PaginaSeleccionada()
        {

        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void grdEstados_Sorted(object sender, EventArgs e)
        {

        }

        protected void grdEstados_Sorting(object sender, GridViewSortEventArgs e)
        {
            obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void grdEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            frmSeleccionar.Visible = false;
            DAL.Tipos_Gastos tipo_gasto = new DAL.Tipos_Gastos();
            string codigo = grdEstados.SelectedDataKey.Value.ToString();
            tipo_gasto = tipo_gasto.obtener(codigo);

            if (ViewState["TipoGastoID"] != null)
                ViewState["TipoGastoID"] = tipo_gasto.id;
            else
                ViewState.Add("TipoGastoID", tipo_gasto.id);

            txtDetalles.Text = tipo_gasto.detalles;
        }

        protected void grdEstados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grdEstados, "Select$" + e.Row.RowIndex.ToString()));
        }

        protected void pagPaginador_PaginaSeleccionada(int paginaActual)
        {

        }

    }
}