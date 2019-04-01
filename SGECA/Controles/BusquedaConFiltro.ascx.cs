using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGECA.Controles
{

    public partial class BusquedaConFiltro : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Master.EnableViewState = true;

            if (!IsPostBack)
                cargarListaFiltro();
        }

        public delegate void CollapseBoxClickedEventHandler(object sender);
        public event CollapseBoxClickedEventHandler CollapseBoxClickedEvent;

        public string campoDescripcion { get; set; }

        public string LabelDescripcion
        {
            get
            {
                return lblDescripcion.Text;
            }
            set
            {
                lblDescripcion.Text = value;
            }
        }

        public string descripcion
        {
            get
            {
                return txtDescripcion.Text;
            }
            set
            {
                txtDescripcion.Text = value;
            }
        }

        public List<DAL.ItemBusqueda> campos
        {
            set
            {
                Session["bcf_ItemBusqueda"] = value;

                cboCampo.Items.Clear();
                cboCampo.DataSource = value;
                cboCampo.DataTextField = "Text";
                cboCampo.DataValueField = "Value";
                cboCampo.DataBind();
                cboCampo.Items.Insert(0, new ListItem("Seleccione un campo", "0"));
            }
        }

        public string campoActual
        {
            get
            {
                return cboCampo.SelectedValue;
            }
            set
            {
                foreach (ListItem item in cboCampo.Items)
                {
                    if (value.ToUpper() == item.Text.ToUpper())
                    {
                        cboCampo.SelectedValue = item.Value;
                        seleccionarTipoFiltro();
                        break;
                    }
                }
            }
        }

        public DAL.TipoFiltro tipoFiltroActual
        {
            get
            {
                return DAL.TipoFiltro.NotLike;
            }
            set
            {
                foreach (ListItem item in cboCondicion.Items)
                {
                    if (value.ToString() == item.Value)
                    {
                        cboCondicion.SelectedValue = item.Value;
                        break;
                    }
                }
            }
        }

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
                DAL.ItemFiltro[] items = new DAL.ItemFiltro[1];
                items[0] = new DAL.ItemFiltro();
                items[0].itemBusqueda = new DAL.ItemBusqueda(txtDescripcion.Text, txtDescripcion.Text, DAL.ItemBusqueda.TipoCampo._string);
                items[0].itemBusqueda.campo = campoDescripcion;
                items[0].itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
                items[0].itemBusqueda.Value = campoDescripcion;
                items[0].textoBusqueda = txtDescripcion.Text;
                items[0].tipoFiltroTexto = new DAL.TipoFiltroTexto();
                items[0].tipoFiltroTexto.value = DAL.TipoFiltro.Like;

                Filtrar(items, radAnd.Checked);

            }
        }




        protected void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            seleccionarTipoFiltro();
        }

        private void seleccionarTipoFiltro()
        {
            cboCondicion.Items.Clear();


            foreach (DAL.ItemBusqueda item in (List<DAL.ItemBusqueda>)Session["bcf_ItemBusqueda"])
            {
                if (item.Value == cboCampo.SelectedValue)
                {
                    foreach (DAL.TipoFiltro cond in item.condiciones)
                    {
                        DAL.TipoFiltroTexto tft = new DAL.TipoFiltroTexto();
                        tft.value = cond;

                        cboCondicion.Items.Add(new ListItem(tft.text, tft.value.ToString()));
                    }

                    break;

                }

            }

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
            lstLista.Items.Clear();
        }



        private void groupBox1_CollapseBoxClickedEvent(object sender)
        {
            if (CollapseBoxClickedEvent != null)
                CollapseBoxClickedEvent(this);
        }



        private void grpFiltrar_Enter(object sender, EventArgs e)
        {

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtDescripcion.Text = "";
            txtDescripcion.Focus();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (txtValor.Text.Trim().Length > 0 &&
                cboCondicion.SelectedItem != null &&
                cboCampo.SelectedItem != null)
            {
                DAL.ItemBusqueda itemBusqueda = null;
                foreach (DAL.ItemBusqueda item in (List<DAL.ItemBusqueda>)Session["bcf_ItemBusqueda"])
                {
                    if (item.Value == cboCampo.SelectedValue)
                    {
                        itemBusqueda = item;
                        break;

                    }

                }

                DAL.TipoFiltroTexto tipoFiltroTexto = null;

                foreach (DAL.TipoFiltro cond in itemBusqueda.condiciones)
                {
                    if (cboCondicion.SelectedValue == cond.ToString())
                    {
                        tipoFiltroTexto = new DAL.TipoFiltroTexto();
                        tipoFiltroTexto.value = cond;
                        break;
                    }
                }


                DAL.ItemFiltro itemFiltro = new DAL.ItemFiltro(itemBusqueda, tipoFiltroTexto, txtValor.Text);


                List<DAL.ItemFiltro> lst = new List<DAL.ItemFiltro>();

                if (Session["bcf_ItemFiltro"] != null)
                    lst = (List<DAL.ItemFiltro>)Session["bcf_ItemFiltro"];

                lst.Add(itemFiltro);

                Session["bcf_ItemFiltro"] = lst;

            }


            cargarListaFiltro();
            txtValor.Text = "";
        }

        private void cargarListaFiltro()
        {

            lstLista.Items.Clear();

            List<DAL.ItemFiltro> lst = new List<DAL.ItemFiltro>();

            if (Session["bcf_ItemFiltro"] != null)
                lst = (List<DAL.ItemFiltro>)Session["bcf_ItemFiltro"];

            for (int i = 0; i < lst.Count; i++)
            {
                lstLista.Items.Add(new ListItem(lst[i].ToString(), i.ToString()));
            }

        }

        protected void btnQuitar_Click(object sender, EventArgs e)
        {

            if (lstLista.SelectedValue != null)
            {

                List<DAL.ItemFiltro> lst = new List<DAL.ItemFiltro>();

                if (Session["bcf_ItemFiltro"] != null)
                    lst = (List<DAL.ItemFiltro>)Session["bcf_ItemFiltro"];

                if (lstLista.SelectedValue != "")
                {
                    lst.RemoveAt(int.Parse(lstLista.SelectedValue));
                }
                Session["bcf_ItemFiltro"] = lst;


                cargarListaFiltro();
            }

        }

        protected void btnQuitarTodos_Click(object sender, EventArgs e)
        {
            Session["bcf_ItemFiltro"] = null;
            cargarListaFiltro();
            txtValor.Focus();

        }

        protected void btnAplicar_Click(object sender, EventArgs e)
        {

            if (Filtrar != null && lstLista.Items.Count > 0)
            {

                DAL.ItemFiltro[] items = ((List<DAL.ItemFiltro>)Session["bcf_ItemFiltro"]).ToArray();
                Filtrar(items, radAnd.Checked);
            }
        }

        protected void txtDescripcion_TextChanged(object sender, EventArgs e)
        {
            if (txtDescripcion.Text.Trim().Length > 0)
                iniciaFiltrado();
        }



    }
}