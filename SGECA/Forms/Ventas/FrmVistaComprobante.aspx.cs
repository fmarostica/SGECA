using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Drawing;
using System.Data;

namespace SGECA.Forms.Ventas
{
    public partial class FrmVistaComprobante : System.Web.UI.Page, LogManager.IObserver, LogManager.ISubject
    {

        LogManager.Mensaje UltimoMensaje { get; set; }

        public void Page_Load(object sender, EventArgs e)
        {
            //DAL.ComprobanteEncabezado enc = new DAL.ComprobanteEncabezado();

            //this.Page.Master.EnableViewState = true;



            //recuperaVariables();

            BusquedaConFiltroComprobante.Error -= BusquedaConFiltro_Error;
            BusquedaConFiltroComprobante.Filtrar -= BusquedaConFiltro_Filtrar;
            BusquedaConFiltroComprobante.Error += BusquedaConFiltro_Error;
            BusquedaConFiltroComprobante.Filtrar += BusquedaConFiltro_Filtrar;

            //if (ViewState["datosObtenidos"] != null)
            //    cargarGrilla();


            //{
            //grdComprobantes.DataSource = enc.obtener();
            //    grdComprobantes.DataKeyNames = new string[] { "id" };
            //    grdComprobantes.DataBind();
            //}

        }



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


        protected void grdComprobantes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            int.TryParse(grdComprobantes.SelectedDataKey.Value.ToString(), out id);
            if (id != 0)
            {
                DAL.ComprobanteEncabezado cmp = new DAL.ComprobanteEncabezado();
                cmp.obtener(id);

                Response.Redirect("FrmVentas.aspx?action=view&cmp=" + cmp.Id);
                //imprimirComprobante(id);
            }



        }

        public void imprimirComprobante(int id)
        {
            //DAL.ComprobanteEncabezado imp = new DAL.ComprobanteEncabezado();
            //imp.imprimir(id);

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('El documento fue puesto en cola de impresión...');", true);
        }




        void BusquedaConFiltro_Filtrar(DAL.ItemFiltro[] itemFiltro, bool busquedaAnd)
        {




            obtenerDatosFiltrados(false, obtenerOrdenActual(), BusquedaConFiltroComprobante.ObtenerItemFiltro());
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

            DAL.IVistaComprobantes VistaComprobantes = new DAL.VistaComprobantes();

            VistaComprobantes.Subscribe(this);

            double cantidadRegistros = 0;

            List<DAL.VistaComprobantes> datosObtenidos = VistaComprobantes.obtenerFiltrado(filtro,
                                                   orden,
                                                   true,
                                                   registroInicio,
                                                   registroFin,
                                                   out  cantidadRegistros);
            if (VistaComprobantes.UltimoMensaje != null)
            {
                UltimoMensaje = VistaComprobantes.UltimoMensaje;
                Notify(UltimoMensaje);
                return;
            }

            ArrayList lista = new ArrayList();
            foreach (DAL.IVistaComprobantes item in datosObtenidos)
            {
                var itemLista = new
                {
                    ID = item.Id,
                    NumeroCompleto = item.NumeroCompleto,
                    RazonSocial = item.RazonSocial,
                    IVA = item.IVA,
                    Neto = item.Neto,
                    Total = item.Total,
                    Fecha = item.Fecha
                };
                lista.Add(itemLista);
            }



            cargarGrilla(lista);

            calcularTotalPaginas(tamañoPagina, cantidadRegistros);


            //verificarPosibilidadPaginacion();

            pagPaginador.setPaginaActual(paginaActual);

            //if (datosObtenidos.Count > 0)
            //    interfaz.habilitaExportacion();
            //else
            //    interfaz.dehabilitaExportacion();
        }

        private void cargarGrilla(ArrayList lista)
        {
            grdComprobantes.DataSource = lista;
            grdComprobantes.DataBind();
            grdComprobantes.Visible = true;
        }

        private void calcularTotalPaginas(int tamañoPagina, double cantidadRegistros)
        {
            int totalPaginas = (int)Math.Ceiling(int.Parse(cantidadRegistros.ToString()) / (double)tamañoPagina);

            pagPaginador.setCantidadRegistros(cantidadRegistros);
            pagPaginador.setTotalPaginas(totalPaginas);
        }


        void BusquedaConFiltro_Error(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }


        private void cargaGrilla(object origenDatos)
        {
            grdComprobantes.DataSource = origenDatos;
            grdComprobantes.DataKeyNames = new string[] { "Id" };
            grdComprobantes.DataBind();
        }

        protected void grdGrilla_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdComprobantes.SelectedValue != null)
            {
                DAL.VistaComprobantes p = new DAL.VistaComprobantes();
                p.obtener((int)grdComprobantes.SelectedValue);
                TextBox t = new TextBox();
                t.Text = p.NumeroCompleto;
                this.Form.Controls.Add(t);
            }
        }

        protected void grdGrilla_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            grdComprobantes.SelectedIndex = e.NewSelectedIndex;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ViewState["VistaComprobante"] = null;
            cargaGrilla(DAL.VistaComprobantes.obtener());
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            DAL.VistaComprobantes p = new DAL.VistaComprobantes();
            //p.NumeroCompleto = DateTime.Now.ToString();
            List<DAL.VistaComprobantes> misVistaComprobante = new List<DAL.VistaComprobantes>();

            if (ViewState["VistaComprobante"] != null && ViewState["VistaComprobante"] is List<DAL.VistaComprobantes>)
            {
                misVistaComprobante = (List<DAL.VistaComprobantes>)ViewState["VistaComprobante"];
            }

            misVistaComprobante.Add(p);

            ViewState["VistaComprobante"] = misVistaComprobante;

            cargaGrilla(misVistaComprobante);

        }


        protected void grdGrilla_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdComprobantes.PageIndex = e.NewPageIndex;
        }

        protected void pagPaginador_Anterior()
        {
            pagPaginador.setPaginaAnterior();
            obtenerDatosFiltrados(false, obtenerOrdenActual(),
                BusquedaConFiltroComprobante.ObtenerItemFiltro());

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
                
                orden[0].Campo = "cen_fecha";
                orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                ViewState["orden[0].TipoOrden"] = DAL.TipoOrden.Ascendente.ToString();
                ViewState["orden[0].Campo"] = orden[0].Campo;
            }
            return new DAL.ItemOrden[0];


        }

        protected void pagPaginador_Fin()
        {

            pagPaginador.setPaginaFinal();
            obtenerDatosFiltrados(false, obtenerOrdenActual(),
                BusquedaConFiltroComprobante.ObtenerItemFiltro());
        }

        protected void pagPaginador_Inicio()
        {
            pagPaginador.setPaginaInicial();
            obtenerDatosFiltrados(false, obtenerOrdenActual(),
                BusquedaConFiltroComprobante.ObtenerItemFiltro());
        }

        protected void pagPaginador_Proxima()
        {

            pagPaginador.setPaginaSiguiente();
            obtenerDatosFiltrados(false, obtenerOrdenActual(),
                BusquedaConFiltroComprobante.ObtenerItemFiltro());
        }

        protected void pagPaginador_PaginaSeleccionada(int paginaActual)
        {

            obtenerDatosFiltrados(false, obtenerOrdenActual(),
                BusquedaConFiltroComprobante.ObtenerItemFiltro());
        }

        protected void grdComprobantes_Sorted(object sender, EventArgs e)
        {

        }

        protected void grdComprobantes_Sorting(object sender, GridViewSortEventArgs e)
        {
            DAL.ItemOrden[] orden = new DAL.ItemOrden[1];


            if (ViewState["orden[0].TipoOrden"] != null)
            {
                if (ViewState["orden[0].Campo"].ToString() == e.SortExpression.ToUpper())
                    if (ViewState["orden[0].TipoOrden"].ToString() == DAL.TipoOrden.Ascendente.ToString())
                        orden[0].TipoOrden = DAL.TipoOrden.Descendente;
                    else
                        orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
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


            obtenerDatosFiltrados(false, orden,
                BusquedaConFiltroComprobante.ObtenerItemFiltro());
        }





    }
}