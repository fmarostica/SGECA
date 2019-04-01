using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SGECA.ControlesComunes
{
    public partial class Paginador : UserControl
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
        public delegate void paginaSeleccionada(double pagina);

        public Paginador()
        {
            InitializeComponent();
            deshabilitaPaginaAnterior();
            deshabilitaPaginaInicial();
            deshabilitaPaginaFinal();
            deshabilitaPaginaProxima();
            cboMostrar.SelectedIndex = 0;
            lblRegistros.Text = "0";
            lblPaginas.Text = "0";
            cboPagina.Enabled = false;

        }


        void btnAnterior_Click(object sender, EventArgs e)
        {
            if (Anterior != null)
                Anterior();

        }

        void btnProxima_Click(object sender, EventArgs e)
        {
            if (Proxima != null)
                Proxima();
        }

        void btnInicio_Click(object sender, EventArgs e)
        {
            if (Inicio != null)
                Inicio();
        }

        void btnFin_Click(object sender, EventArgs e)
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

        public void setTotalPaginas(double totalPaginas)
        {
            lblPaginas.Text = totalPaginas.ToString();
            cboPagina.SelectedIndexChanged -= cboPagina_SelectedIndexChanged;
            cboPagina.Items.Clear();
            for (double i = 1; i <= totalPaginas; i++)
            {
                cboPagina.Items.Add(i);
            }
            if (cboPagina.Items.Count > 1)
                cboPagina.Enabled = true;
            else
                cboPagina.Enabled = false;
            cboPagina.SelectedIndexChanged += cboPagina_SelectedIndexChanged;
        }

        private void cboPagina_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PaginaSeleccionada != null)
                PaginaSeleccionada((double)cboPagina.SelectedItem);
        }

        public void setPaginaActual(double paginaActual)
        {
            cboPagina.SelectedIndexChanged -= cboPagina_SelectedIndexChanged;
            cboPagina.SelectedItem = paginaActual;
            cboPagina.SelectedIndexChanged += cboPagina_SelectedIndexChanged;
        }


    }
}
