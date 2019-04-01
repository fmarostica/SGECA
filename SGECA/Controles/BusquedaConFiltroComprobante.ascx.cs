using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGECA.Controles
{

    public partial class BusquedaConFiltroComprobante : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Master.EnableViewState = true;

            if (!IsPostBack)
                cargarDatos();
        }

        private void cargarDatos()
        {

            DAL.Cliente cli = new DAL.Cliente();
            cmbCliente.DataSource = cli.obtener();
            cmbCliente.DataTextField = "RazonSocial";
            cmbCliente.DataValueField = "Id";
            cmbCliente.DataBind();
            cmbCliente.Items.Insert(0, new ListItem("Todos los clientes", "0"));

            cmbSucursal.DataSource = DAL.PuntoVenta.obtener();
            cmbSucursal.DataTextField = "Nombre";
            cmbSucursal.DataValueField = "codigo";
            cmbSucursal.DataBind();
            cmbSucursal.Items.Insert(0, new ListItem("Todos los puntos de venta", "0"));

            cmbComprobante.DataSource = DAL.TipoComprobante.obtener();
            cmbComprobante.DataTextField = "Descripcion";
            cmbComprobante.DataValueField = "codigo";
            cmbComprobante.DataBind();
            cmbComprobante.Items.Insert(0, new ListItem("Todos", "0"));

            DAL.ComprobanteEncabezado ce = new DAL.ComprobanteEncabezado();
            DateTime dMin = DateTime.MinValue, dMax = DateTime.MaxValue;
            ce.obtenerFechasTope(out dMin, out dMax);

            //pongo fecha mínima seteada al primer día del mes corriente
            dMin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);


            txtDesde.Text = dMin.ToString("yyyy-MM-dd");
            txtHasta.Text = dMax.ToString("yyyy-MM-dd");

            txtNumero.Text = "";

        }

        public delegate void CollapseBoxClickedEventHandler(object sender);
        public event CollapseBoxClickedEventHandler CollapseBoxClickedEvent;

        public string campoDescripcion { get; set; }

        //public string LabelDescripcion
        //{
        //    get
        //    {
        //        return lblDescripcion.Text;
        //    }
        //    set
        //    {
        //        lblDescripcion.Text = value;
        //    }
        //}

        //public string descripcion
        //{
        //    get
        //    {
        //        return txtDescripcion.Text;
        //    }
        //    set
        //    {
        //        txtDescripcion.Text = value;
        //    }
        //}

        //public List<DAL.ItemBusqueda> campos
        //{
        //    set
        //    {
        //        Session["bcf_ItemBusqueda"] = value;

        //        cboCampo.Items.Clear();
        //        cboCampo.DataSource = value;
        //        cboCampo.DataTextField = "Text";
        //        cboCampo.DataValueField = "Value";
        //        cboCampo.DataBind();
        //        cboCampo.Items.Insert(0, new ListItem("Seleccione un campo", "0"));
        //    }
        //}

        //public string campoActual
        //{
        //    get
        //    {
        //        return cboCampo.SelectedValue;
        //    }
        //    set
        //    {
        //        foreach (ListItem item in cboCampo.Items)
        //        {
        //            if (value.ToUpper() == item.Text.ToUpper())
        //            {
        //                cboCampo.SelectedValue = item.Value;
        //                seleccionarTipoFiltro();
        //                break;
        //            }
        //        }
        //    }
        //}

        //public DAL.TipoFiltro tipoFiltroActual
        //{
        //    get
        //    {
        //        return DAL.TipoFiltro.NotLike;
        //    }
        //    set
        //    {
        //        foreach (ListItem item in cboCondicion.Items)
        //        {
        //            if (value.ToString() == item.Value)
        //            {
        //                cboCondicion.SelectedValue = item.Value;
        //                break;
        //            }
        //        }
        //    }
        //}

        public event filtrar Filtrar;

        public delegate void filtrar(DAL.ItemFiltro[] itemFiltro, bool busquedaAnd);



        public void btnBuscar_Click(object sender, EventArgs e)
        {
            iniciaFiltrado();
        }

        private void iniciaFiltrado()
        {
            if (Filtrar != null)
            {
                
                Filtrar(ObtenerItemFiltro(), true);

            }
        }




        //protected void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    seleccionarTipoFiltro();
        //}

        private void seleccionarTipoFiltro()
        {
            //cboCondicion.Items.Clear();


            //foreach (DAL.ItemBusqueda item in (List<DAL.ItemBusqueda>)Session["bcf_ItemBusqueda"])
            //{
            //    if (item.Value == cboCampo.SelectedValue)
            //    {
            //        foreach (DAL.TipoFiltro cond in item.condiciones)
            //        {
            //            DAL.TipoFiltroTexto tft = new DAL.TipoFiltroTexto();
            //            tft.value = cond;

            //            cboCondicion.Items.Add(new ListItem(tft.text, tft.value.ToString()));
            //        }

            //        break;

            //    }

            //}

        }



        private void BusquedaConFiltro_Load(object sender, EventArgs e)
        {
            //grpBusquedaDescripcion.ForeColor = this.ForeColor;
            //grpFiltrar.ForeColor = this.ForeColor;
        }

        //protected override void OnHandleCreated(EventArgs e)
        //{

        //    nombreInstanciaActual = base.Name;


        //    base.OnHandleCreated(e);
        //}

        public void setLstLista()
        {
            //lstLista.Items.Clear();
        }



        private void groupBox1_CollapseBoxClickedEvent(object sender)
        {
            if (CollapseBoxClickedEvent != null)
                CollapseBoxClickedEvent(this);
        }



        //private void grpFiltrar_Enter(object sender, EventArgs e)
        //{

        //}

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {

            cargarDatos();

        }

        //protected void btnAgregar_Click(object sender, EventArgs e)
        //{
        //if (txtValor.Text.Trim().Length > 0 &&
        //    cboCondicion.SelectedItem != null &&
        //    cboCampo.SelectedItem != null)
        //{
        //    DAL.ItemBusqueda itemBusqueda = null;
        //    foreach (DAL.ItemBusqueda item in (List<DAL.ItemBusqueda>)Session["bcf_ItemBusqueda"])
        //    {
        //        if (item.Value == cboCampo.SelectedValue)
        //        {
        //            itemBusqueda = item;
        //            break;

        //        }

        //    }

        //    DAL.TipoFiltroTexto tipoFiltroTexto = null;

        //    foreach (DAL.TipoFiltro cond in itemBusqueda.condiciones)
        //    {
        //        if (cboCondicion.SelectedValue == cond.ToString())
        //        {
        //            tipoFiltroTexto = new DAL.TipoFiltroTexto();
        //            tipoFiltroTexto.value = cond;
        //            break;
        //        }
        //    }


        //    DAL.ItemFiltro itemFiltro = new DAL.ItemFiltro(itemBusqueda, tipoFiltroTexto, txtValor.Text);


        //    List<DAL.ItemFiltro> lst = new List<DAL.ItemFiltro>();

        //    if (Session["bcf_ItemFiltro"] != null)
        //        lst = (List<DAL.ItemFiltro>)Session["bcf_ItemFiltro"];

        //    lst.Add(itemFiltro);

        //    Session["bcf_ItemFiltro"] = lst;

        //}


        //cargarListaFiltro();
        //txtValor.Text = "";
        //}

        private void cargarListaFiltro()
        {

            //lstLista.Items.Clear();

            //List<DAL.ItemFiltro> lst = new List<DAL.ItemFiltro>();

            //if (Session["bcf_ItemFiltro"] != null)
            //    lst = (List<DAL.ItemFiltro>)Session["bcf_ItemFiltro"];

            //for (int i = 0; i < lst.Count; i++)
            //{
            //    lstLista.Items.Add(new ListItem(lst[i].ToString(), i.ToString()));
            //}

        }

        //protected void btnQuitar_Click(object sender, EventArgs e)
        //{

        //if (lstLista.SelectedValue != null)
        //{

        //    List<DAL.ItemFiltro> lst = new List<DAL.ItemFiltro>();

        //    if (Session["bcf_ItemFiltro"] != null)
        //        lst = (List<DAL.ItemFiltro>)Session["bcf_ItemFiltro"];

        //    if (lstLista.SelectedValue != "")
        //    {
        //        lst.RemoveAt(int.Parse(lstLista.SelectedValue));
        //    }
        //    Session["bcf_ItemFiltro"] = lst;


        //    cargarListaFiltro();
        //}

        //}

        //protected void btnQuitarTodos_Click(object sender, EventArgs e)
        //{
        //Session["bcf_ItemFiltro"] = null;
        //cargarListaFiltro();
        //txtValor.Focus();

        //}

        //protected void btnAplicar_Click(object sender, EventArgs e)
        //{

        //    if (Filtrar != null && lstLista.Items.Count > 0)
        //    {

        //        DAL.ItemFiltro[] items = ((List<DAL.ItemFiltro>)Session["bcf_ItemFiltro"]).ToArray();
        //        Filtrar(items, radAnd.Checked);
        //    }
        //}

        //protected void txtDescripcion_TextChanged(object sender, EventArgs e)
        //{
        //    if (txtDescripcion.Text.Trim().Length > 0)
        //        iniciaFiltrado();
        //}




        internal DAL.ItemFiltro[] ObtenerItemFiltro()
        {
            List<DAL.ItemFiltro> items = new List<DAL.ItemFiltro>();


            DAL.ItemFiltro fil = new DAL.ItemFiltro();
            fil.itemBusqueda = new DAL.ItemBusqueda();
            fil.itemBusqueda.campo = "cen_fecha";
            fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._datetime;
            fil.itemBusqueda.Value = "cen_fecha";

            if (DateTime.Parse(txtDesde.Text) < DateTime.Parse(txtHasta.Text))
            {
                fil.textoBusqueda = txtDesde.Text;
                fil.textoBusqueda2 = txtHasta.Text + " 23:59:59.99";
            }
            else
            {
                fil.textoBusqueda = txtHasta.Text;
                fil.textoBusqueda2 = txtDesde.Text + " 23:59:59.99";
            }





            fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
            fil.tipoFiltroTexto.value = DAL.TipoFiltro.Between;

            items.Add(fil);

            if (cmbCliente.SelectedValue != "0")
            {
                fil = new DAL.ItemFiltro();
                fil.itemBusqueda = new DAL.ItemBusqueda();
                fil.itemBusqueda.campo = "cli_id";
                fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._int;
                fil.itemBusqueda.Value = "cli_id";
                fil.textoBusqueda = cmbCliente.SelectedValue;
                fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
                fil.tipoFiltroTexto.value = DAL.TipoFiltro.Equal;

                items.Add(fil);
            }

            if (cmbSucursal.SelectedValue != "0")
            {
                fil = new DAL.ItemFiltro();
                fil.itemBusqueda = new DAL.ItemBusqueda();
                fil.itemBusqueda.campo = "pvt_codigo";
                fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
                fil.itemBusqueda.Value = "pvt_codigo";
                fil.textoBusqueda = cmbSucursal.SelectedValue;
                fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
                fil.tipoFiltroTexto.value = DAL.TipoFiltro.Equal;

                items.Add(fil);
            }

            if (txtNumero.Text.Trim() != "")
            {
                fil = new DAL.ItemFiltro();
                fil.itemBusqueda = new DAL.ItemBusqueda();
                fil.itemBusqueda.campo = "cen_numero";
                fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._int;
                fil.itemBusqueda.Value = "cen_numero";
                fil.textoBusqueda = txtNumero.Text;
                fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
                fil.tipoFiltroTexto.value = DAL.TipoFiltro.Equal;

                items.Add(fil);
            }


            if (cmbComprobante.SelectedValue != "0")
            {
                fil = new DAL.ItemFiltro();
                fil.itemBusqueda = new DAL.ItemBusqueda();
                fil.itemBusqueda.campo = "tco_codigo";
                fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
                fil.itemBusqueda.Value = "tco_codigo";
                fil.textoBusqueda = cmbComprobante.SelectedValue;
                fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
                fil.tipoFiltroTexto.value = DAL.TipoFiltro.Equal;

                items.Add(fil);
            }

            return items.ToArray<DAL.ItemFiltro>();
        }
    }
}