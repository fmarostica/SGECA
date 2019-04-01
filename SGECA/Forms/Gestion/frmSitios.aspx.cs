using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGECA.Forms.Gestion
{
    public partial class frmSitios : System.Web.UI.Page
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
            if(!this.IsPostBack)
            {
                frmSeleccionar.Visible = false;
                cargar_paises();
                txtPais.SelectedValue = "14";
                cargar_provincias(txtPais.SelectedValue.ToString());
                cargar_viaticos();
            }
        }

        private void cargar_paises()
        {
            List<DAL.IPais> lista_paises = DAL.Pais.obtener();

            txtPais.DataTextField = "Nombre".ToUpper();
            txtPais.DataValueField = "Id";

            txtPais.DataSource = lista_paises;
            txtPais.DataBind();
        }

        private void cargar_provincias(string pais_id)
        {
            DAL.Provincias prov = new DAL.Provincias();
            List<DAL.Provincias> lista_provincias = new List<DAL.Provincias>();

            lista_provincias.Add(new DAL.Provincias { id = "0", nombre = "", pais_id = "" });

            lista_provincias.AddRange(prov.Obtener_por_pais(pais_id));

            txtProvincia.DataTextField = "Nombre";
            txtProvincia.DataValueField = "Id";

            txtProvincia.DataSource = lista_provincias;
            txtProvincia.DataBind();
        }

        void cargar_viaticos()
        {
            DAL.Viaticos viaticos = new DAL.Viaticos();
            List<DAL.Viaticos> lista_viaticos = new List<DAL.Viaticos>();

            lista_viaticos = viaticos.obtener();

            txtViatico.DataTextField = "Descripcion";
            txtViatico.DataValueField = "Id";

            txtViatico.DataSource = lista_viaticos;
            txtViatico.DataBind();
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

            DAL.Sitios VistaTareas = new DAL.Sitios();

            //VistaTareas.Subscribe(this);

            double cantidadRegistros = 0;

            List<DAL.Sitios> datosObtenidos = VistaTareas.obtenerFiltrado(filtro,
                                                   orden,
                                                   false,
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

        void cargarGrilla(List<DAL.Sitios> lista)
        {
            grdEstados.DataSource = lista;
            grdEstados.DataBind();
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

            fil.itemBusqueda = new DAL.ItemBusqueda();
            fil.itemBusqueda.campo = "CellID";
            fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
            fil.itemBusqueda.Value = "CellID";
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
                orden[0].Campo = "CellID";
                orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                ViewState["orden[0].TipoOrden"] = DAL.TipoOrden.Ascendente.ToString();
                ViewState["orden[0].Campo"] = orden[0].Campo;
            }
            return new DAL.ItemOrden[0];

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            guardar(true);
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            if(ViewState["SitioID"]!=null)
                guardar(false);
            else
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("Debe seleccionar un registro para poder guardarlo"), true);
        }

        void guardar(bool nuevo)
        {
            bool errores = false;

            if(txtNombre.Text=="" || txtProvincia.Text=="" || txtDepartamento.Text=="" || txtLatitud.Text=="" || txtLongitud.Text=="" || txtLocalidad.Text=="")
            {
                errores = true;
                if (txtNombre.Text == "") lblNombre.ForeColor = Color.Red;
                if (txtProvincia.Text == "") lblProvincia.ForeColor = Color.Red;
                if (txtDepartamento.Text == "") lblDepartamento.ForeColor = Color.Red;
                if (txtLatitud.Text == "") lblLatitud.ForeColor = Color.Red;
                if (txtLongitud.Text == "") lblLongitud.ForeColor = Color.Red;
                if (txtLocalidad.Text == "") lblLocalidad.ForeColor = Color.Red;

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("Los campos marcados en rojo son obligatorios y no pueden estar vacíos"), true);
            }

            if(!errores)
            {
                DAL.Sitios sitio = new DAL.Sitios();

                if (nuevo)
                    sitio.Codigo = txtCellID.Text;
                else
                    sitio.Codigo = ViewState["SitioID"].ToString();

                sitio.Departamento = txtDepartamento.Text;
                sitio.Latitud = txtLatitud.Text;
                sitio.Longitud = txtLongitud.Text;
                sitio.Localidad = txtLocalidad.Text;
                sitio.Nombre = txtNombre.Text;
                sitio.Pais = txtPais.Text;
                sitio.Provincia = txtProvincia.Text;
                sitio.Viatico_ID = txtViatico.SelectedValue.ToString();

                sitio.guardar(nuevo);

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("Operación realizada!"), true);

                Limpiar();
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if(ViewState["SitioID"]!=null)
            {
                DAL.Sitios sitio = new DAL.Sitios();
                sitio.borrar(ViewState["SitioID"].ToString());
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("Operación realizada!"), true);
                Limpiar();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("Debe seleccionar un registro para poder borralo!"), true);
            }
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

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
        }

        protected void grdEstados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grdEstados, "Select$" + e.Row.RowIndex.ToString()));
        }

        protected void grdEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            frmSeleccionar.Visible = false;

            Limpiar();
            string id = grdEstados.SelectedDataKey.Value.ToString();
            if (id != "")
            {
                DAL.Sitios emp = new DAL.Sitios();
                emp = emp.obtener_datos(id);

                if (ViewState["SitioID"] != null)
                    ViewState["SitioID"] = emp.Codigo;
                else
                    ViewState.Add("SitioID", emp.Codigo);

                txtCellID.Text = emp.Codigo.ToString();
                txtLatitud.Text = emp.Latitud;
                txtLongitud.Text = emp.Longitud;
                txtNombre.Text = emp.Nombre;

                txtPais.ClearSelection();
                txtPais.Items.FindByText(emp.Pais).Selected = true;

                txtProvincia.ClearSelection();
                txtProvincia.Items.FindByText(emp.Provincia).Selected = true;

                txtDepartamento.Text = emp.Departamento;
                txtLocalidad.Text = emp.Localidad;

                txtViatico.SelectedValue = emp.Viatico_ID;
            }
        }

        void Limpiar()
        {
            lblNombre.ForeColor = Color.Black;
            lblProvincia.ForeColor = Color.Black;
            lblDepartamento.ForeColor = Color.Black;
            lblLatitud.ForeColor = Color.Black;
            lblLongitud.ForeColor = Color.Black;
            lblLocalidad.ForeColor = Color.Black;

            ViewState["SitioID"] = null;
            txtCellID.Text = "";
            txtLatitud.Text = "";
            txtLongitud.Text = "";
            txtNombre.Text = "";
            txtDepartamento.Text = "";
            txtLocalidad.Text = "";

            txtPais.SelectedValue = "14";
            cargar_provincias("14");
            txtProvincia.SelectedIndex = 0;
            txtViatico.SelectedIndex = 0;
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
            obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
        }

        protected void grdEstados_Sorted(object sender, EventArgs e)
        {
            //obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
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

        protected void txtPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtProvincia.Items.Clear();
            cargar_provincias(txtPais.SelectedValue.ToString());
        }

    }
}