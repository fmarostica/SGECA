using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SGECA.ControlesComunes
{
    public partial class BusquedaConFiltro : UserControl
    {
        private string nombreClaseInstanciadora = "";
        private string nombreInstanciaActual = "";
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
                cboCampo.Items.Clear();
                foreach (DAL.ItemBusqueda item in value)
                {
                    cboCampo.Items.Add(item);
                }
                cboCampo.DisplayMember = "Text";
                cboCampo.ValueMember = "Value";
            }
        }

        public string campoActual
        {
            get
            {
                return cboCampo.SelectedText;
            }
            set
            {
                foreach (DAL.ItemBusqueda item in cboCampo.Items)
                {
                    if (value.ToUpper() == item.Value.ToUpper())
                    {
                        cboCampo.SelectedItem = item;
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
                foreach (DAL.TipoFiltroTexto item in cboCondicion.Items)
                {
                    if (value == item.value)
                    {
                        cboCondicion.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        public event filtrar Filtrar;

        public delegate void filtrar(DAL.ItemFiltro[] itemFiltro, bool busquedaAnd);

        public BusquedaConFiltro()
        {
            InitializeComponent();


            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();

            foreach (System.Diagnostics.StackFrame item in stackTrace.GetFrames())
            {
                Console.WriteLine(item.GetMethod().ReflectedType.FullName);
                if (item.GetMethod().ReflectedType.FullName.Contains("AesaDigitalizacion.Maestros") ||
                    item.GetMethod().ReflectedType.FullName.Contains("AesaDigitalizacion.Gestion"))
                {
                    nombreClaseInstanciadora = item.GetMethod().ReflectedType.FullName;
                    break;
                }

            }


        }



        public void btnBuscar_Click(object sender, EventArgs e)
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

        private void txtDescripcion_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnBuscar_Click(this, new EventArgs());
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtDescripcion.Text = "";
            txtDescripcion.Focus();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboCondicion.Items.Clear();
            foreach (DAL.TipoFiltro item in (List<DAL.TipoFiltro>)((DAL.ItemBusqueda)cboCampo.
                SelectedItem).condiciones)
            {
                DAL.TipoFiltroTexto tft = new DAL.TipoFiltroTexto();
                tft.value = item;

                cboCondicion.Items.Add(tft);

            }

            cboCondicion.DisplayMember = "Text";
            cboCondicion.ValueMember = "Value";
            cboCondicion.Text = "Que contiene";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (txtValor.Text.Trim().Length > 0 &&
                cboCondicion.SelectedItem != null &&
                cboCampo.SelectedItem != null)
            {
                DAL.ItemFiltro itemFiltro = new DAL.ItemFiltro((DAL.ItemBusqueda)cboCampo.
                    SelectedItem, (DAL.TipoFiltroTexto)cboCondicion.SelectedItem, txtValor.Text);
                lstLista.Items.Add(itemFiltro);
            }
            txtValor.Text = "";

        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (lstLista.SelectedItem != null)
                lstLista.Items.Remove(lstLista.SelectedItem);

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {

            if (Filtrar != null && lstLista.Items.Count > 0)
            {
                DAL.ItemFiltro[] items = new DAL.ItemFiltro[lstLista.Items.Count];
                lstLista.Items.CopyTo(items, 0);
                Filtrar(items, radAnd.Checked);
            }
        }

        private void BusquedaConFiltro_Load(object sender, EventArgs e)
        {
            grpBusquedaDescripcion.ForeColor = this.ForeColor;
            grpFiltrar.ForeColor = this.ForeColor;
        }

        protected override void OnHandleCreated(EventArgs e)
        {

            nombreInstanciaActual = base.Name;
          

            base.OnHandleCreated(e);
        }

        public void setLstLista()
        {
            lstLista.Items.Clear();
        }

        private void txtDescripcion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)8)
            {
                btnBuscar.PerformClick();
            }
        }

        private void btnQuitarTodos_Click(object sender, EventArgs e)
        {
            if (lstLista.Items.Count > 0)
            {
                lstLista.Items.Clear();
                txtValor.Focus();
            }
        }

        private void groupBox1_CollapseBoxClickedEvent(object sender)
        {
            if (CollapseBoxClickedEvent != null)
                CollapseBoxClickedEvent(this);
        }

       

        private void grpFiltrar_Enter(object sender, EventArgs e)
        {

        }
    }
}
