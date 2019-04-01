using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SGECA.Forms
{
    public partial class FrmPais : System.Web.UI.Page, LogManager.IObserver, LogManager.ISubject
    {
        string pagina = "FrmPais";
        int paginaActual = 1;
        int totalPaginas;
        DAL.ItemOrden[] orden;
        DAL.ItemFiltro[] itemFiltro;
        bool busquedaAnd;
        IList datosObtenidos;
        double registroInicio, registroFin, cantidadRegistros;
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

            if (!IsPostBack)
                cargarDatosFiltro();


            recuperaVariables();

            BusquedaConFiltro.LabelDescripcion = "Buscar por nombre:";
            BusquedaConFiltro.campoDescripcion = "pai_nombre";
            BusquedaConFiltro.Error -= BusquedaConFiltro_Error;
            BusquedaConFiltro.Filtrar -= BusquedaConFiltro_Filtrar;
            BusquedaConFiltro.Error += BusquedaConFiltro_Error;
            BusquedaConFiltro.Filtrar += BusquedaConFiltro_Filtrar;

            if (Session[pagina + "datosObtenidos"] != null)
                cargarGrilla();
        }

        private void recuperaVariables()
        {

            if (Session[pagina + "paginaActual"] != null)
                paginaActual = Convert.ToInt32(Session[pagina + "paginaActual"]);
            if (Session[pagina + "totalPaginas"] != null)
                totalPaginas = Convert.ToInt32(Session[pagina + "totalPaginas"]);
            if (Session[pagina + "orden"] != null)
                orden = (DAL.ItemOrden[])Session[pagina + "orden"];
            if (Session[pagina + "itemFiltro"] != null)
                itemFiltro = (DAL.ItemFiltro[])Session[pagina + "itemFiltro"];
            if (Session[pagina + "busquedaAnd"] != null)
                busquedaAnd = (bool)Session[pagina + "busquedaAnd"];
            if (Session[pagina + "datosObtenidos"] != null)
                datosObtenidos = (IList)Session[pagina + "datosObtenidos"];
            if (Session[pagina + "registroInicio"] != null)
                registroInicio = (double)Session[pagina + "registroInicio"];
            if (Session[pagina + "registroFin"] != null)
                registroFin = (double)Session[pagina + "registroFin"];
            if (Session[pagina + "cantidadRegistros"] != null)
                cantidadRegistros = (double)Session[pagina + "cantidadRegistros"];


        }

        public void cargarDatosFiltro()
        {
            List<DAL.ItemBusqueda> items = new List<DAL.ItemBusqueda>();
            DAL.ItemBusqueda li = new DAL.ItemBusqueda("Nombre", "pai_nombre",
                                                                    DAL.ItemBusqueda.TipoCampo._string);
            items.Add(li);


            BusquedaConFiltro.campos = items;
            BusquedaConFiltro.campoActual = "Nombre";
            BusquedaConFiltro.tipoFiltroActual = DAL.TipoFiltro.Like;



        }

        void BusquedaConFiltro_Filtrar(DAL.ItemFiltro[] itemFiltro, bool busquedaAnd)
        {
            Session[pagina + "paginaActual"] = 1;

            Session[pagina + "itemFiltro"] = itemFiltro;
            Session[pagina + "busquedaAnd"] = busquedaAnd;

            DAL.ItemOrden[] orden = new DAL.ItemOrden[1];
            orden[0] = new DAL.ItemOrden();
            orden[0].Campo = "pai_nombre";
            orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
            Session[pagina + "orden"] = orden;

            obtenerDatosFiltrados(false);
        }


        private void obtenerDatosFiltrados(bool todos)
        {
            paginaActual = (int)Session[pagina + "paginaActual"];
            int tamañoPagina = pagPaginador.obtenerRegistrosMostrar();

            registroInicio = ((paginaActual - 1) * tamañoPagina) + 1;

            if (todos)
                registroFin = -1;
            else
                registroFin = tamañoPagina * paginaActual;

            DAL.IPais pais = new DAL.Pais();

            pais.Subscribe(this);

            datosObtenidos = pais.obtenerFiltrado((DAL.ItemFiltro[])Session[pagina + "itemFiltro"],
                                                  (DAL.ItemOrden[])Session[pagina + "orden"],
                                                  (bool)Session[pagina + "busquedaAnd"],
                                                  registroInicio,
                                                  registroFin,
                                                  out  cantidadRegistros);
            if (pais.UltimoMensaje != null)
            {
                UltimoMensaje = pais.UltimoMensaje;
                Notify(UltimoMensaje);
                return;
            }

            ArrayList lista = new ArrayList();
            foreach (DAL.IPais item in datosObtenidos)
            {
                var itemLista = new
                {
                    ID = item.Id,
                    Nombre = item.Nombre
                };
                lista.Add(itemLista);
            }




            Session[pagina + "datosObtenidos"] = datosObtenidos;
            Session[pagina + "totalPaginas"] = totalPaginas;
            Session[pagina + "totalPaginas"] = totalPaginas;
            cargarGrilla();

            calcularTotalPaginas(tamañoPagina);

            //verificarPosibilidadPaginacion();

            pagPaginador.setPaginaActual(paginaActual);

            //if (datosObtenidos.Count > 0)
            //    interfaz.habilitaExportacion();
            //else
            //    interfaz.dehabilitaExportacion();
        }

        private void cargarGrilla()
        {
            grdGrilla.DataSource = Session[pagina + "datosObtenidos"];
            grdGrilla.DataBind();
        }

        private void calcularTotalPaginas(int tamañoPagina)
        {
            totalPaginas = (int)Math.Ceiling(int.Parse(cantidadRegistros.ToString()) / (double)tamañoPagina);
            Session[pagina + "totalPaginas"] = totalPaginas;

            pagPaginador.setCantidadRegistros(cantidadRegistros);
            pagPaginador.setTotalPaginas(totalPaginas);
        }


        void BusquedaConFiltro_Error(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }


        private void cargaGrilla(object origenDatos)
        {
            grdGrilla.DataSource = origenDatos;
            grdGrilla.DataKeyNames = new string[] { "Id" };
            grdGrilla.DataBind();
        }

        protected void grdGrilla_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdGrilla.SelectedValue != null)
            {
                DAL.Pais p = new DAL.Pais();
                p.obtener((int)grdGrilla.SelectedValue);
                TextBox t = new TextBox();
                t.Text = p.Nombre;
                this.Form.Controls.Add(t);
            }
        }

        protected void grdGrilla_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            grdGrilla.SelectedIndex = e.NewSelectedIndex;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Session["Paises"] = null;
            cargaGrilla(DAL.Pais.obtener());
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            DAL.Pais p = new DAL.Pais();
            p.Nombre = DateTime.Now.ToString();
            List<DAL.Pais> misPaises = new List<DAL.Pais>();

            if (Session["Paises"] != null && Session["Paises"] is List<DAL.Pais>)
            {
                misPaises = (List<DAL.Pais>)Session["Paises"];
            }

            misPaises.Add(p);

            Session["Paises"] = misPaises;

            cargaGrilla(misPaises);

        }

        protected void grdGrilla_PageIndexChanged(object sender, EventArgs e)
        {


            cargaGrilla(Session["Paises"]);
        }

        protected void grdGrilla_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdGrilla.PageIndex = e.NewPageIndex;
        }

        protected void pagPaginador_Anterior()
        {
            Session[pagina + "paginaActual"] = ((int)Session[pagina + "paginaActual"]) - 1;
            obtenerDatosFiltrados(false);

            cargarGrilla();
        }

        protected void pagPaginador_Fin()
        {

            Session[pagina + "paginaActual"] = Session[pagina + "totalPaginas"];
            obtenerDatosFiltrados(false);

            cargarGrilla();
        }

        protected void pagPaginador_Inicio()
        {
            Session[pagina + "paginaActual"] = 1;
            obtenerDatosFiltrados(false);

            cargarGrilla();
        }

        protected void pagPaginador_Proxima()
        {

            Session[pagina + "paginaActual"] = ((int)Session[pagina + "paginaActual"]) + 1;
            obtenerDatosFiltrados(false);

            cargarGrilla();
        }

        protected void pagPaginador_PaginaSeleccionada(int paginaActual)
        {

            Session[pagina + "paginaActual"] = paginaActual;
            obtenerDatosFiltrados(false);

            cargarGrilla();
        }

        protected void grdGrilla_Sorted(object sender, EventArgs e)
        {

        }

        protected void grdGrilla_Sorting(object sender, GridViewSortEventArgs e)
        {
            Session[pagina + "paginaActual"] = 1;

            Session[pagina + "itemFiltro"] = itemFiltro;
            Session[pagina + "busquedaAnd"] = busquedaAnd;

            if (this.orden != null)
            {
                if (this.orden[0].Campo.ToUpper() == e.SortExpression.ToUpper())
                    if (this.orden[0].TipoOrden == DAL.TipoOrden.Ascendente)
                        this.orden[0].TipoOrden = DAL.TipoOrden.Descendente;
                    else
                        this.orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                else
                {
                    orden[0].Campo = e.SortExpression;
                    orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                }
            }
            else
            {

                this.orden = new DAL.ItemOrden[1];
                orden[0] = new DAL.ItemOrden();
                orden[0].Campo = e.SortExpression;
                orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
            }


            Session[pagina + "orden"] = orden;

            obtenerDatosFiltrados(false);

        }


    }
}