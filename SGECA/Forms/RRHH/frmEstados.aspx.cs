using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGECA.Forms.Empleados
{
    public partial class frmEstados : System.Web.UI.Page
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
            txtBuscar.Attributes.Add("placeholder", "Ingrese el texto a buscar");
            if(!this.IsPostBack)
            {
                frmSeleccionar.Visible = false;
                txtFiltroTipo.Items.Add("TODOS");
                txtFiltroTipo.Items.Add("No Trabajado");
                txtFiltroTipo.Items.Add("Trabajado");

                this.Page.Master.EnableViewState = true;
                obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
            }
        }

        internal DAL.ItemFiltro[] ObtenerItemFiltro()
        {
            List<DAL.ItemFiltro> items = new List<DAL.ItemFiltro>();

            DAL.ItemFiltro fil = new DAL.ItemFiltro();
            fil.itemBusqueda = new DAL.ItemBusqueda();
            fil.itemBusqueda.campo = "tarea_estado_nombre";
            fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
            fil.itemBusqueda.Value = "tarea_estado_nombre";
            fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
            fil.tipoFiltroTexto.value = DAL.TipoFiltro.Like;
            fil.textoBusqueda = txtBuscar.Text;
            items.Add(fil);

            if(txtFiltroTipo.Text!="TODOS")
            {
                fil = new DAL.ItemFiltro();
                fil.itemBusqueda = new DAL.ItemBusqueda();
                fil.itemBusqueda.campo = "tarea_estado_tipo";
                fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
                fil.itemBusqueda.Value = "tarea_estado_tipo";
                fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
                fil.tipoFiltroTexto.value = DAL.TipoFiltro.Equal;
                fil.textoBusqueda = txtFiltroTipo.Text;
                items.Add(fil);
            }

            return items.ToArray<DAL.ItemFiltro>();
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

            DAL.TareasEstados VistaTareas = new DAL.TareasEstados();

            double cantidadRegistros = 0;

            List<DAL.TareasEstados> datosObtenidos = VistaTareas.obtenerFiltrado(filtro,
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

            ArrayList lista = new ArrayList();
            foreach (DAL.TareasEstados item in datosObtenidos)
            {
                var itemLista = new
                {
                    Codigo = item.Id,
                    Nombre = item.Nombre,
                    Tipo = item.Tipo
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
                orden[0].Campo = "tarea_estado_nombre";
                orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                ViewState["orden[0].TipoOrden"] = DAL.TipoOrden.Ascendente.ToString();
                ViewState["orden[0].Campo"] = orden[0].Campo;
            }
            return new DAL.ItemOrden[0];

        }

        void cargarGrilla(ArrayList lista)
        {
            grdEstados.DataSource = lista;
            grdEstados.DataBind();
        }

        protected void grdEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            DAL.TareasEstados tareas = new DAL.TareasEstados();
            string codigo = grdEstados.SelectedDataKey.Value.ToString();
            List<DAL.TareasEstados> datos = tareas.obtener(codigo);
            foreach (var item in datos)
            {
                if (ViewState["EstadoID"] != null)
                    ViewState["EstadoID"] = item.Id;
                else
                    ViewState.Add("EstadoID", item.Id);

                txtCodigo.Text = item.Id.ToString();
                txtNombre.Text = item.Nombre;
                txtTipo.Text = item.Tipo;
            }

            frmSeleccionar.Visible = false;
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

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        void limpiar()
        {
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtTipo.SelectedIndex = 0;

            lblCodigo.ForeColor = Color.Black;
            lblNombre.ForeColor = Color.Black;

            ViewState["EstadoID"] = null;
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            guardar(true);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ViewState["EstadoID"] != null)
                guardar(false);
            else
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("Debe seleccionar un registro para poder guardarlo"), true);
        }

        void guardar(bool nuevo)
        {
            bool errores = false;

            if (nuevo)
            {
                DAL.TareasEstados estados = new DAL.TareasEstados();
                int registros = estados.obtener(txtCodigo.Text, "").Count;

                if (registros > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("La operación no pudo realizarse ya que el código ingresado existe en la base de datos!.", 3000), true);
                    errores = true;
                }
            }

            if(txtCodigo.Text=="" || txtNombre.Text=="")
            {
                if (txtNombre.Text == "") lblNombre.ForeColor = Color.Red;
                if (txtCodigo.Text == "") lblCodigo.ForeColor = Color.Red;
                errores = true;

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Los campos señalados en rojo no pueden estar vacíos", 3000), true);
            }

            if(!errores)
            {
                lblNombre.ForeColor = Color.Black;

                DAL.TareasEstados estados = new DAL.TareasEstados();
                estados.Id = txtCodigo.Text;
                estados.Nombre = txtNombre.Text;
                estados.Tipo = txtTipo.Text;

                if (estados.Guardar(nuevo))
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Operacion realizada!", 3000), true);
                else
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Error al realizar el proceso", 3000), true);

                limpiar();
            }
            
        }

        protected void grdEstados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow) 
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grdEstados, "Select$" + e.Row.RowIndex.ToString()));
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

            obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (ViewState["EstadoID"] != null)
            {
                DAL.TareasEstados estados = new DAL.TareasEstados();
                if (!estados.tiene_registros_relacionados(ViewState["EstadoID"].ToString()))
                {
                    if (estados.borrar(ViewState["EstadoID"].ToString()))
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Estado borrado!", 3000), true);
                        obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
                        limpiar();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("El registro no puede ser borrado ya que posee registros relacionados.", 3000), true);
                }
            }
            else
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe seleccionar un registro para poder borrarlo", 3000), true);
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

    }
}