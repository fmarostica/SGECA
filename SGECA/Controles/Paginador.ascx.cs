using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGECA.Controles
{
    public partial class Paginador : System.Web.UI.UserControl
    {

        public event inicio Inicio;
        public event anterior Anterior;
        public event proxima Proxima;
        public event fin Fin;
        public event paginaSeleccionada PaginaSeleccionada;

        public delegate void inicio();
        public delegate void anterior();
        public delegate void proxima();
        public delegate void fin();
        public delegate void paginaSeleccionada(int pagina);


        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Master.EnableViewState = true;

        }


        public void inicializar()
        {

            deshabilitaPaginaAnterior();
            deshabilitaPaginaInicial();
            deshabilitaPaginaFinal();
            deshabilitaPaginaProxima();
            cboMostrar.SelectedIndex = 0;
            lblRegistros.Text = "0";
            lblPaginas.Text = "0";
            cboPagina.Enabled = false;
        }

        protected void cboMostrar_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void cboPagina_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PaginaSeleccionada != null)
                PaginaSeleccionada(int.Parse(cboPagina.SelectedValue));
        }

        protected void btnInicio_Click(object sender, EventArgs e)
        {
            if (Inicio != null)
                Inicio();
        }

        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            if (Anterior != null)
                Anterior();
        }

        protected void btnProxima_Click(object sender, EventArgs e)
        {
            if (Proxima != null)
                Proxima();
        }

        protected void btnFin_Click(object sender, EventArgs e)
        {
            if (Fin != null)
                Fin();
        }


        public void deshabilitaPaginaInicial()
        {
            btnInicio.Enabled = false;
        }

        public void deshabilitaPaginaAnterior()
        {
            btnAnterior.Enabled = false;
        }

        public void habilitaPaginaInicial()
        {
            btnInicio.Enabled = true;
        }

        public void habilitaPaginaAnterior()
        {
            btnAnterior.Enabled = true;
        }

        public void deshabilitaPaginaProxima()
        {
            btnProxima.Enabled = false;
        }

        public void deshabilitaPaginaFinal()
        {
            btnFin.Enabled = false;
        }

        public void habilitaPaginaProxima()
        {
            btnProxima.Enabled = true;
        }

        public void habilitaPaginaFinal()
        {
            btnFin.Enabled = true;
        }

        public int obtenerRegistrosMostrar()
        {
            return int.Parse(cboMostrar.Text);
        }

        public void setCantidadRegistros(double cantidadRegistros)
        {
            lblRegistros.Text = cantidadRegistros.ToString();
        }

        public void setTotalPaginas(int totalPaginas)
        {
            lblPaginas.Text = totalPaginas.ToString();
            cboPagina.SelectedIndexChanged -= cboPagina_SelectedIndexChanged;
            cboPagina.Items.Clear();
            for (int i = 1; i <= totalPaginas; i++)
            {
                cboPagina.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            if (cboPagina.Items.Count > 1)
                cboPagina.Enabled = true;
            else
                cboPagina.Enabled = false;
            cboPagina.SelectedIndexChanged += cboPagina_SelectedIndexChanged;
        }

        public void setPaginaActual(int paginaActual)
        {
            if (paginaActual < 1)
                paginaActual = 1;
            cboPagina.SelectedIndexChanged -= cboPagina_SelectedIndexChanged;
            cboPagina.SelectedValue = paginaActual.ToString();
            cboPagina.SelectedIndexChanged += cboPagina_SelectedIndexChanged;

            if (paginaActual > 1)
            {
                habilitaPaginaAnterior();
                habilitaPaginaInicial();
            }
            else
            {
                deshabilitaPaginaAnterior();
                deshabilitaPaginaInicial();
            }

            if (paginaActual < cboPagina.Items.Count)
            {
                habilitaPaginaFinal();
                habilitaPaginaProxima();
            }
            else
            {
                deshabilitaPaginaFinal();
                deshabilitaPaginaProxima();
            }

        }


        internal int obtenerPaginaActual()
        {
            if (cboPagina.SelectedValue == null || cboPagina.SelectedValue == "")
                return 1;
            else
                return int.Parse(cboPagina.SelectedValue);


        }

        internal void setPaginaAnterior()
        {
            setPaginaActual(int.Parse(cboPagina.SelectedValue) - 1);
        }

        internal void setPaginaSiguiente()
        {
            setPaginaActual(int.Parse(cboPagina.SelectedValue) + 1);
        }

        internal void setPaginaInicial()
        {
            setPaginaActual(0);
        }

        internal void setPaginaFinal()
        {
            setPaginaActual(cboPagina.Items.Count);
        }

    }
}