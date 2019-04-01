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

namespace SGECA.Forms.Ventas
{
    public partial class FrmVentas : System.Web.UI.Page, LogManager.IObserver, LogManager.ISubject
    {


        private Color colorBien = Color.White;
        private Color colorMal = Color.Yellow;
        public int cantReg { get; set; }



        public void Page_Load(object sender, EventArgs e)
        {

            Subscribe(new LogManager.Log());


            if (ViewState["ComprobanteEncabezado"] == null)
                ViewState["ComprobanteEncabezado"] = new DAL.ComprobanteEncabezado();

            cargarGrilla();
            //verificarSiGrillaVacia();
            if (!IsPostBack)
            {
                txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFecha.Enabled = false;

                cargaCombo();
                txtIVA.Text = "21%";
                VerificarAccion();
                //cargarOrdenesCompra();
            }




        }



        public void Page_Init(object sender, EventArgs e)
        {
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {

        }



        private void VerificarAccion()
        {

            if (Request["action"] == null || Request["action"] == "new")
            {
                ViewState["ComprobanteEncabezado"] = null;
                ModoEdicion(true);
                Response.Redirect("FrmVentas.aspx?action=edit");
            }

            int doc = 0;
            if (Request["action"] != null && Request["action"] == "view"
                && Request["cmp"] != null && int.TryParse(Request["cmp"], out doc))
            {
                DAL.ComprobanteEncabezado encab = new DAL.ComprobanteEncabezado();
                encab.obtener(doc);

                if (encab.Id == 0)
                    return;

                ViewState["ComprobanteEncabezado"] = encab;

                recargarDatos();

                cargarGrilla();
            }
            //else
            //    ModoEdicion(true);
        }


        private DAL.ComprobanteEncabezado obtenerComprobante()
        {
            DAL.ComprobanteEncabezado encab = new DAL.ComprobanteEncabezado();

            if (ViewState["ComprobanteEncabezado"] != null &&
                 ViewState["ComprobanteEncabezado"] is DAL.ComprobanteEncabezado)
            {
                encab = (DAL.ComprobanteEncabezado)ViewState["ComprobanteEncabezado"];

            }

            return encab;
        }

        private void recargarDatos()
        {

            DAL.ComprobanteEncabezado encab = obtenerComprobante();


            cmbCliente.SelectedValue = encab.ClienteID.ToString();
            cmbComprobante.SelectedValue = encab.tco_codigo;
            cmbConceptoventa.SelectedValue = encab.cvt_codigo;
            cmbCondPago.SelectedValue = encab.cva_codigo;
            cmbListaPrecio.SelectedValue = encab.lpr_id.ToString();
            cmbTipo.SelectedValue = encab.tdo_codigo;
            cmbTipoResponsable.SelectedValue = encab.tre_Codigo;
            txtCuit.Text = encab.Cli_Cuit.ToString();
            txtDireccion.Text = encab.Cli_Direccion;
            txtFecha.Text = encab.cen_fecha.ToString("dd/MM/yyyy");
            txtingbruto.Text = encab.Cli_IngresosBrutos;
            txtLocalidad.Text = encab.Cli_Localidad;
            txtProvincia.Text = encab.Cli_Provincia;
            txtrazonSocial.Text = encab.ClienteRazonSocial;
            txtNumero.Text = encab.cen_numeroCompleto;
            cmbSucursal.SelectedValue = encab.pvt_codigo;
            lblID.Text = "(ID:" + encab.Id.ToString() + ")";
            CalcularTotales();
            //cmbOrdenCompra.Enabled = false;
            lblCantReg.Text = "Cantidad Items: " + encab.itemsComprobante.Count.ToString();






            if (encab.cen_numero == 0)
            {
                btnGuardar.Visible = true;
                btnGuardarConCAE.Visible = true;
                btnReimprimir.Visible = false;
            }
            else
            {
                ModoEdicion(false);
                btnGuardar.Visible = false;
                btnGuardarConCAE.Visible = false;
                btnReimprimir.Visible = true;
            }
        }

        private void ModoEdicion(bool estado)
        {
            cmbCliente.Enabled = estado;
            cmbComprobante.Enabled = estado;
            cmbConceptoventa.Enabled = estado;
            cmbCondPago.Enabled = estado;
            cmbListaPrecio.Enabled = estado;
            cmbTipo.Enabled = estado;
            cmbTipoResponsable.Enabled = estado;
            txtCuit.Enabled = estado;
            txtDireccion.Enabled = estado;
            txtFecha.Enabled = estado;
            txtingbruto.Enabled = estado;
            txtLocalidad.Enabled = estado;
            txtProvincia.Enabled = estado;
            txtrazonSocial.Enabled = estado;
            //txtNumero.Enabled = estado;
            formularioItem.Enabled = estado;
            cmbCaja.Enabled = estado;
            cmbVendedor.Enabled = estado;
            cmbSucursal.Enabled = estado;
            //btnCancelar.Visible = estado;

            grdItems.Enabled = estado;
        }


        public bool verificarParaGrabar()
        {
            DAL.ComprobanteEncabezado encab = obtenerComprobante();


            bool estado = true;
            estado &= verificaCuit(txtCuit);
            estado &= verificaRequerido(txtrazonSocial);

            if (!estado)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Por favor complete los campos requeridos');", true);
                return false;
            }

            if (encab.itemsComprobante.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Disculpe no se puede grabar un comprobante sin ITEMS');", true);
                return false;
            }

            if (encab.cen_total <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Disculpe No se puede grabar un comprobante con importe 0');", true);
                return false;
            }



            return true;
        }
        private void cargaCombo()
        {
            cmbCondPago.DataSource = DAL.CondicionVenta.obtener();
            cmbCondPago.DataTextField = "Nombre";
            cmbCondPago.DataValueField = "codigo";
            cmbCondPago.DataBind();

            cmbTipoResponsable.DataSource = DAL.TipoResponsable.obtener();
            cmbTipoResponsable.DataTextField = "Nombre";
            cmbTipoResponsable.DataValueField = "codigo";
            cmbTipoResponsable.DataBind();

            cmbListaPrecio.DataSource = DAL.ListaPrecio.obtener();
            cmbListaPrecio.DataTextField = "Nombre";
            cmbListaPrecio.DataValueField = "codigo";
            cmbListaPrecio.DataBind();

            cmbComprobante.DataSource = DAL.TipoComprobante.obtener();
            cmbComprobante.DataTextField = "Descripcion";
            cmbComprobante.DataValueField = "codigo";
            cmbComprobante.DataBind();
            cmbComprobante.SelectedIndex = -1;



            cmbSucursal.DataSource = DAL.PuntoVenta.obtener();
            cmbSucursal.DataTextField = "Nombre";
            cmbSucursal.DataValueField = "codigo";
            cmbSucursal.DataBind();

            cmbTipo.DataSource = DAL.TipoDocumento.obtener();
            cmbTipo.DataTextField = "Nombre";
            cmbTipo.DataValueField = "codigo";
            cmbTipo.DataBind();

            foreach (ListItem item3 in cmbTipo.Items)
            {
                if (item3.Text == "CUIT")
                {
                    item3.Selected = true;
                    break;
                }
            }

            cmbConceptoventa.DataSource = DAL.ConceptoVenta.obtener();
            cmbConceptoventa.DataTextField = "Nombre";
            cmbConceptoventa.DataValueField = "codigo";
            cmbConceptoventa.DataBind();
            foreach (ListItem item1 in cmbConceptoventa.Items)
            {
                if (item1.Text == "Productos")
                {
                    item1.Selected = true;
                    break;
                }
            }

            //cmbConceptoventa.Items.FindByValue("21").Selected = true;

            //cmbIva.DataSource = DAL.AlicuotaIva.obtener();
            //cmbIva.DataTextField = "nombre";
            //cmbIva.DataValueField = "ali_porcentaje";
            //cmbIva.DataBind();
            //foreach (ListItem item in cmbIva.Items)
            //{
            //    if (item.Text == "21%")
            //    {
            //        item.Selected = true;
            //        break;
            //    }
            //}

            DAL.Cliente cli = new DAL.Cliente();
            cmbCliente.Items.Clear();
            cmbCliente.DataSource = cli.obtener();
            cmbCliente.DataTextField = "RazonSocial";
            cmbCliente.DataValueField = "Id";
            cmbCliente.DataBind();
            cmbCliente.Items.Insert(0, new ListItem("Seleccione un cliente", "0"));
            
        }


        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> ObtenerAlicuotasIVA(string prefixText)
        {
            List<DAL.IAlicuotaIva> ai = DAL.AlicuotaIva.obtenerLista(prefixText);
            List<string> ret = new List<string>();

            if (ai != null)
                foreach (DAL.IAlicuotaIva item in ai)
                {
                    ret.Add(item.Nombre);
                }

            return ret;
        }




        private void HookOnFocus(Control CurrentControl)
        {
            //checks if control is one of TextBox, DropDownList, ListBox or Button
            if ((CurrentControl is TextBox) ||
                (CurrentControl is DropDownList) ||
                (CurrentControl is ListBox) ||
                (CurrentControl is Button))
                //adds a script which saves active control on receiving focus 
                //in the hidden field __LASTFOCUS.
                (CurrentControl as WebControl).Attributes.Add(
                   "onfocus",
                   "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");
            //checks if the control has children
            if (CurrentControl.HasControls())
                //if yes do them all recursively
                foreach (Control CurrentChildControl in CurrentControl.Controls)
                    HookOnFocus(CurrentChildControl);
        }


        private void verificarSiGrillaVacia()
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string a = Request.Form["fecha"];
        }

        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            DAL.Producto p = new DAL.Producto();
            p.Subscribe(this);
            p.obtener(txtCodigo.Text);
            if (p.Id != 0)
            {
                //txtDescripción.TextChanged += txtDescripción_TextChanged;
                if (p.Costo != 0)
                    txtImporte.Text = p.Costo.ToString("0.00").Replace(",", ".");
                txtDescripción.Text = p.Descripcion;
                txtCantidad.Focus();

                //txtDescripción.TextChanged += txtDescripción_TextChanged;
            }
            else
            {

                txtDescripción.Focus();
            }

        }

        protected void txtDescripción_TextChanged(object sender, EventArgs e)
        {
            txtCantidad.Focus();

        }

        protected void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            txtImporte.Focus();
            calcularSubtotalItem();
        }

        protected void txtImporte_TextChanged(object sender, EventArgs e)
        {

            calcularSubtotalItem();
            btnAgregar.Focus();
        }

        private void calcularSubtotalItem()
        {
            DAL.ComprobanteItem ci = obtenerItem();
            if (ci != null)
            {
                txtSubTotal.Text = ci.Subtotalneto.ToString();
            }
            else
                txtSubTotal.Text = "";
        }

        private DAL.ComprobanteItem obtenerItem()
        {

            DAL.ComprobanteItem ci = new DAL.ComprobanteItem();

            decimal cant = 0;
            decimal impo = 0;
            decimal desc = 0;
            decimal pIva = 0;
            decimal subT = 0;

            DAL.AlicuotaIva iva = new DAL.AlicuotaIva();
            iva.obtener(txtIVA.Text);

            if (iva.Codigo == null)
            {
                return null;
                //todo: no se encontro alicuota
            }
            Decimal.TryParse(txtCantidad.Text.Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator), out cant);
            Decimal.TryParse(txtImporte.Text.Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator), out impo);
            Decimal.TryParse(txtDescuento.Text.Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator), out desc);
            //Decimal.TryParse(cmbIva.SelectedValue.Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator), out pIva);

            ci.Codigo = (txtCodigo.Text).ToString();
            ci.Descripcion = txtDescripción.Text;
            ci.Cantidad = cant;
            ci.Precio = impo;
            ci.Descuento = desc;
            ci.AlicuotaPorcentaje = iva.ali_porcentaje;

            return ci;
        }
        private int calcularPrecio()
        {
            int subTotal = Convert.ToInt32(txtCantidad.Text) * Convert.ToInt32(txtImporte.Text);

            return subTotal;
        }



        private void agregarItemaComprobante()
        {
            if (ViewState["ComprobanteEncabezado"] == null)
                return;

            DAL.ComprobanteItem ci = obtenerItem();

            DAL.ComprobanteEncabezado encab = (DAL.ComprobanteEncabezado)(ViewState["ComprobanteEncabezado"]);



            encab.itemsComprobante.Add(ci);

            controlCantidadItems(encab);

            ViewState["ComprobanteEncabezado"] = encab;

            cargarGrilla();
            preparaNuevaCarga();

        }

        private void controlCantidadItems(DAL.ComprobanteEncabezado encab)
        {
            cantReg = encab.itemsComprobante.Count;
            if (cantReg == int.Parse(ConfigurationManager.AppSettings["cantidadItems"]))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Disculpe, usted ha alcanzado la cantidad maxima de registros permitidos.');", true);
                formularioItem.Enabled = false;
            }
            else
            {
                formularioItem.Enabled = true;
            }
            lblCantReg.Text = "Cantidad Items: " + cantReg.ToString();
        }

        private void cargarDatosEncabezado()
        {
            DAL.ComprobanteEncabezado encab = (DAL.ComprobanteEncabezado)(ViewState["ComprobanteEncabezado"]);

            if (encab.Id == 0)
                encab.cen_fecha = DateTime.Now;


            long cuit = 0;
            long.TryParse(txtCuit.Text, out cuit);
            encab.Cli_Cuit = cuit;

            int intTemp = 0;
            if (int.TryParse(cmbCliente.SelectedValue, out intTemp))
            {
                encab.ClienteID = intTemp;

                DAL.Cliente c = new DAL.Cliente();
                c.obtener(encab.ClienteID);
                encab.Cli_Codigo = c.Codigo;
            }

            encab.Cli_Direccion = txtDireccion.Text;
            encab.Cli_Localidad = txtLocalidad.Text;
            encab.Cli_Provincia = txtProvincia.Text;
            encab.Cli_IngresosBrutos = txtingbruto.Text;
            encab.ClienteRazonSocial = txtrazonSocial.Text;

            encab.tre_Nombre = cmbTipoResponsable.SelectedItem.ToString();
            encab.tre_Codigo = cmbTipoResponsable.SelectedValue;

            encab.lpr_id = int.Parse(cmbListaPrecio.SelectedValue);
            encab.lpr_nombre = cmbListaPrecio.SelectedItem.ToString();

            encab.cvt_nombre = cmbConceptoventa.SelectedItem.ToString();
            encab.cvt_codigo = cmbConceptoventa.SelectedValue;

            encab.tdo_nombre = cmbTipo.SelectedItem.ToString();
            encab.tdo_codigo = cmbTipo.SelectedValue;

            encab.pvt_codigo = cmbSucursal.SelectedValue.ToString();

            encab.cva_nombre = cmbCondPago.SelectedItem.ToString();
            encab.cva_codigo = cmbCondPago.SelectedValue;

            encab.tco_codigo = cmbComprobante.SelectedValue;

            Dictionary<decimal, decimal> ivas = new Dictionary<decimal, decimal>();



            encab.cen_IVA01 = 0;        //IVA 21%
            encab.cen_IVA01neto = 0;    //IVA 21%
            encab.cen_IVA01porc = 0;    //IVA 21%
            encab.cen_IVA02 = 0;        //IVA 10.5%
            encab.cen_IVA02neto = 0;    //IVA 10.5%
            encab.cen_IVA02porc = 0;    //IVA 10.5%
            encab.cen_IVA03 = 0;        //IVA 27%
            encab.cen_IVA03neto = 0;    //IVA 27%
            encab.cen_IVA03porc = 0;    //IVA 27%
            encab.cen_IVA04 = 0;        //IVA 0%
            encab.cen_IVA04neto = 0;    //IVA 0%
            encab.cen_IVA04porc = 0;    //IVA 0%

            encab.cen_neto = 0;
            encab.cen_total = 0;

            foreach (DAL.ComprobanteItem item in encab.itemsComprobante)
            {
                encab.cen_neto += item.Subtotalneto;
                encab.cen_total += item.Subtotal;


                if (item.AlicuotaPorcentaje == 21m)
                {
                    encab.cen_IVA01 += item.MontoIVA;
                    encab.cen_IVA01neto += item.Subtotalneto;
                    encab.cen_IVA01porc = item.AlicuotaPorcentaje;
                }
                else if (item.AlicuotaPorcentaje == 10.5m)
                {
                    encab.cen_IVA02 += item.MontoIVA;
                    encab.cen_IVA02neto += item.Subtotalneto;
                    encab.cen_IVA02porc = item.AlicuotaPorcentaje;
                }
                else if (item.AlicuotaPorcentaje == 27m)
                {
                    encab.cen_IVA03 += item.MontoIVA;
                    encab.cen_IVA03neto += item.Subtotalneto;
                    encab.cen_IVA03porc = item.AlicuotaPorcentaje;
                }
                else if (item.AlicuotaPorcentaje == 0m)
                {
                    encab.cen_IVA04 += item.MontoIVA;
                    encab.cen_IVA04neto += item.Subtotalneto;
                    encab.cen_IVA04porc = item.AlicuotaPorcentaje;
                }
            }



        }

        private void calculoPrecioFinal()
        {
        }

        private void preparaNuevaCarga()
        {



            txtCodigo.Text = null;
            txtDescripción.Text = null;
            txtCantidad.Text = null;
            txtImporte.Text = null;
            txtDescuento.Text = null;

            txtSubTotal.Text = null;
            txtIvatotal.Text = null;
            txtCodigo.Focus();
        }

        private void cargarGrilla()
        {
            grdItems.DataSource = null;
            grdItems.DataBind();
            CalcularTotales();

            DAL.ComprobanteEncabezado encab = obtenerComprobante();

            grdItems.DataSource = encab.itemsComprobante;
            grdItems.DataBind();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                "scrollBottom",
                "var objDiv = document.getElementById(\"" + divGrdVentas.ClientID + "\");" +
                "objDiv.scrollTop = objDiv.scrollHeight;", true);

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            if (GuardarComprobante(true))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
              "redir",
              "location.href = \"FrmVentas.aspx?action=new\";", true);


               // Response.Redirect("FrmVentas.aspx?action=new");
            }



        }

        private bool GuardarComprobante(bool mostrarMensajes)
        {
            int compr = 0;

            cargarDatosEncabezado();
            DAL.ComprobanteEncabezado encab = obtenerComprobante();

            if (!verificarParaGrabar())
            {
                return false;
            }
            else
            {



                DAL.Database.BeginTransaction("fact");

                if (encab.Id == 0)
                    encab.cen_fecha = DateTime.Now;

                encab.guardar();
                if (encab.UltimoMensaje != null)
                {
                    DAL.Database.RollbackTransaction();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No se ha podido grabar. Intente nuevamente');", true);


                    ScriptManager.RegisterClientScriptBlock(this,
                         this.GetType(), "habilitoCtrls",
                         "habilitoControles();", true);

                    //todo:hubo error
                    return false;
                }

                if (encab.Id != 0)
                {
                    DAL.ComprobanteItem.LimpiarItemsEncabezado(encab.Id);

                    int ordenItem = 0;
                    foreach (DAL.ComprobanteItem item in encab.itemsComprobante)
                    {
                        item.Id = 0;
                        item.Orden = ordenItem;
                        item.EncabezaoId = encab.Id;

                        item.guardar();
                        if (item.UltimoMensaje != null)
                        {
                            DAL.Database.RollbackTransaction();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Uno o mas items no se han podido guardar. Intente nuevamente');", true);


                            ScriptManager.RegisterClientScriptBlock(this,
                                 this.GetType(), "habilitoCtrls",
                                 "habilitoControles();", true);


                            //todo:hubo error
                            return false;
                        }
                        ordenItem++;
                    }
                }

                //solo si no hubo error
                DAL.Database.CommitTransaction();

                if (mostrarMensajes)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Se ha guardado correctamente.');", true);

                    ScriptManager.RegisterClientScriptBlock(this,
                         this.GetType(), "habilitoCtrls",
                         "habilitoControles();", true);
                }
                return true;


            }
        }

        private int obtenerNumeroComprobante(int pvt_codigo, int tco_codigo)
        {
            int compr = 0;
            string comprobanteNro = null;
            string extension = ".fecua";
            string dirConsulta = ConfigurationManager.AppSettings["archivoconsulta"] +
                @"\P" + pvt_codigo +
                "-T" + tco_codigo + ".wsfe";


            try
            {
                if (File.Exists(dirConsulta + ".res"))
                    File.Delete(dirConsulta + ".res");

                if (File.Exists(dirConsulta + ".tou"))
                    File.Delete(dirConsulta + ".tou");

                if (File.Exists(dirConsulta + ".ecx"))
                    File.Delete(dirConsulta + ".ecx");

            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No se ha podido obtener el próximo número de comprobante ya que hay una solicitud anterior. Intente nuevamente');", true);

                return 0;
            }



            StreamWriter consultaCompr = null;
            try
            {
                consultaCompr = new StreamWriter(dirConsulta + extension);


            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No se ha podido obtener el próximo número de comprobante. Intente nuevamente');", true);

                return 0;
            }
            finally
            {
                consultaCompr.Close();
            }


            DateTime ini = DateTime.Now;
            int tiempoEspera = Convert.ToInt32(ConfigurationManager.AppSettings["tiempoespera"]);
            while (true)
            {
                TimeSpan ts = DateTime.Now - ini;

                if (ts.TotalMilliseconds > tiempoEspera)
                    break;

                System.Threading.Thread.Sleep(500);

                if (File.Exists(dirConsulta + ".res") ||
                   File.Exists(dirConsulta + ".tou") ||
                    File.Exists(dirConsulta + ".ecx"))
                {
                    System.Threading.Thread.Sleep(200);
                    break;

                }

            }


            if (File.Exists(dirConsulta + ".res"))
            {
                string line;
                Dictionary<string, string> arrText = new Dictionary<string, string>();

                StreamReader file = new StreamReader(dirConsulta + ".res");
                while ((line = file.ReadLine()) != null)
                {
                    string[] cadena = line.Split('=');
                    arrText.Add(cadena[0], cadena[1]);
                }

                comprobanteNro = arrText["CbteNro"];
                string comprobanteerror = arrText["Errors"]; ;
                string comprobanteEvento = arrText["Events"]; ;




                file.Close();
                File.Delete(dirConsulta + ".res");
                compr = int.Parse(comprobanteNro) + 1;

                if (comprobanteerror != "")
                {
                    compr = 0;

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", string.Format("alert('{0}');", comprobanteerror), true);
                }
                else
                {
                    List<string> verificados = new List<string>();
                    if (ViewState["comprobantesVerificados"] != null && ViewState["comprobantesVerificados"] is List<string>)
                        verificados = (List<string>)ViewState["comprobantesVerificados"];
                    verificados.Add("P" + pvt_codigo + "-T" + tco_codigo);
                    ViewState["comprobantesVerificados"] = verificados;

                }

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No se ha podido obtener el próximo número de comprobante. Intente nuevamente');", true);

            }

            return compr;




        }

        private string crearXml(DAL.ComprobanteEncabezado encab, int numero, out string AfipReq)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode FeCAEReq = doc.CreateElement("FeCAEReq");
            doc.AppendChild(FeCAEReq);

            XmlNode productNode = doc.CreateElement("FeCabReq");
            FeCAEReq.AppendChild(productNode);

            XmlNode cantReg = doc.CreateElement("CantReg");
            cantReg.AppendChild(doc.CreateTextNode("1"));
            productNode.AppendChild(cantReg);
            XmlNode PtoVta = doc.CreateElement("PtoVta");
            PtoVta.AppendChild(doc.CreateTextNode(cmbSucursal.SelectedValue));
            productNode.AppendChild(PtoVta);
            XmlNode CbteTipo = doc.CreateElement("CbteTipo");
            CbteTipo.AppendChild(doc.CreateTextNode(cmbComprobante.SelectedValue));
            productNode.AppendChild(CbteTipo);

            // Create and add another product node.
            XmlNode productNode1 = doc.CreateElement("FeDetReq");
            FeCAEReq.AppendChild(productNode1);

            XmlNode productNode2 = doc.CreateElement("FECAEDetRequest");
            productNode1.AppendChild(productNode2);

            XmlNode Concepto = doc.CreateElement("Concepto");
            Concepto.AppendChild(doc.CreateTextNode(cmbConceptoventa.SelectedValue));
            productNode2.AppendChild(Concepto);
            XmlNode DocTipo = doc.CreateElement("DocTipo");
            DocTipo.AppendChild(doc.CreateTextNode(cmbTipo.SelectedValue));
            productNode2.AppendChild(DocTipo);
            XmlNode DocNro = doc.CreateElement("DocNro");
            DocNro.AppendChild(doc.CreateTextNode(encab.Cli_Cuit.ToString()));
            productNode2.AppendChild(DocNro);

            XmlNode CbteDesde = doc.CreateElement("CbteDesde");
            CbteDesde.AppendChild(doc.CreateTextNode(numero.ToString()));
            productNode2.AppendChild(CbteDesde);
            XmlNode CbteHasta = doc.CreateElement("CbteHasta");
            CbteHasta.AppendChild(doc.CreateTextNode(numero.ToString()));
            productNode2.AppendChild(CbteHasta);
            XmlNode CbteFch = doc.CreateElement("CbteFch");
            string fecha = DateTime.Now.ToString("yyyyMMdd");
            CbteFch.AppendChild(doc.CreateTextNode(fecha));
            productNode2.AppendChild(CbteFch);

            XmlNode ImpTotal = doc.CreateElement("ImpTotal");
            ImpTotal.AppendChild(doc.CreateTextNode(encab.cen_total.ToString("0.00", CultureInfo.InvariantCulture)));
            productNode2.AppendChild(ImpTotal);
            XmlNode ImpTotConc = doc.CreateElement("ImpTotConc");
            ImpTotConc.AppendChild(doc.CreateTextNode("0"));
            productNode2.AppendChild(ImpTotConc);
            XmlNode ImpNeto = doc.CreateElement("ImpIVA");
            ImpNeto.AppendChild(doc.CreateTextNode((encab.cen_total - encab.cen_neto).ToString("0.00", CultureInfo.InvariantCulture)));
            productNode2.AppendChild(ImpNeto);

            XmlNode ImpOpEx = doc.CreateElement("ImpOpEx");
            ImpOpEx.AppendChild(doc.CreateTextNode("0"));
            productNode2.AppendChild(ImpOpEx);
            XmlNode ImpIVA = doc.CreateElement("ImpNeto");
            ImpIVA.AppendChild(doc.CreateTextNode(encab.cen_neto.ToString("0.00", CultureInfo.InvariantCulture)));
            productNode2.AppendChild(ImpIVA);
            XmlNode ImpTrib = doc.CreateElement("ImpTrib");
            ImpTrib.AppendChild(doc.CreateTextNode("0"));
            productNode2.AppendChild(ImpTrib);

            XmlNode FchServDesde = doc.CreateElement("FchServDesde");//DUDA
            FchServDesde.AppendChild(doc.CreateTextNode(""));
            productNode2.AppendChild(FchServDesde);
            XmlNode FchServHasta = doc.CreateElement("FchServHasta");//DUDA
            FchServHasta.AppendChild(doc.CreateTextNode(""));
            productNode2.AppendChild(FchServHasta);
            XmlNode FchVtoPago = doc.CreateElement("FchVtoPago");//DUDA
            FchVtoPago.AppendChild(doc.CreateTextNode(""));
            productNode2.AppendChild(FchVtoPago);

            XmlNode MonId = doc.CreateElement("MonId");
            MonId.AppendChild(doc.CreateTextNode("PES"));
            productNode2.AppendChild(MonId);
            XmlNode MonCotiz = doc.CreateElement("MonCotiz");
            MonCotiz.AppendChild(doc.CreateTextNode("1"));
            productNode2.AppendChild(MonCotiz);

            XmlNode productNode3 = doc.CreateElement("IVA");
            productNode2.AppendChild(productNode3);




            if (encab.cen_IVA01neto > 0)
            {
                XmlNode productNode4 = doc.CreateElement("AlicIva");
                productNode3.AppendChild(productNode4);
                generarNodosIVA(encab.ali_id(encab.cen_IVA01porc),
                                encab.cen_IVA01porc,
                                encab.cen_IVA01neto,
                                encab.cen_IVA01,
                                doc, productNode4);
            }

            if (encab.cen_IVA02neto > 0)
            {
                XmlNode productNode4 = doc.CreateElement("AlicIva");
                productNode3.AppendChild(productNode4);
                generarNodosIVA(encab.ali_id(encab.cen_IVA02porc),
                                encab.cen_IVA02porc,
                                encab.cen_IVA02neto,
                                encab.cen_IVA02,
                                doc, productNode4);
            }

            if (encab.cen_IVA03neto > 0)
            {
                XmlNode productNode4 = doc.CreateElement("AlicIva");
                productNode3.AppendChild(productNode4);
                generarNodosIVA(encab.ali_id(encab.cen_IVA03porc),
                                encab.cen_IVA03porc,
                                encab.cen_IVA03neto,
                                encab.cen_IVA03,
                                doc, productNode4);
            }

            if (encab.cen_IVA04neto > 0)
            {
                XmlNode productNode4 = doc.CreateElement("AlicIva");
                productNode3.AppendChild(productNode4);
                generarNodosIVA(encab.ali_id(encab.cen_IVA04porc),
                                encab.cen_IVA04porc,
                                encab.cen_IVA04neto,
                                encab.cen_IVA04,
                                doc, productNode4);
            }


            string dirConsulta = ConfigurationManager.AppSettings["archivoconsulta"] + @"\" +
               encab.Id.ToString() + ".wsfe";


            StreamWriter nuevo = new StreamWriter(dirConsulta + ".req");
            doc.Save(nuevo);

            AfipReq = doc.InnerXml;
            nuevo.Close();

            return dirConsulta;
        }

        private static void generarNodosIVA(int ali_id, decimal cen_IVAporc, decimal cen_IVAneto, decimal cen_IVA, XmlDocument doc, XmlNode productNode4)
        {
            XmlNode Id = doc.CreateElement("Id");
            Id.AppendChild(doc.CreateTextNode(ali_id.ToString()));
            productNode4.AppendChild(Id);
            XmlNode BaseImp = doc.CreateElement("BaseImp");
            BaseImp.AppendChild(doc.CreateTextNode(cen_IVAneto.ToString("0.00", CultureInfo.InvariantCulture)));
            productNode4.AppendChild(BaseImp);
            XmlNode Importe = doc.CreateElement("Importe");
            Importe.AppendChild(doc.CreateTextNode(cen_IVA.ToString("0.00", CultureInfo.InvariantCulture)));
            productNode4.AppendChild(Importe);
        }



        protected void cmbCliente_TextChanged(object sender, EventArgs e)
        {
            cmbTipoResponsable.Focus();
        }

        protected void condicionIva_TextChanged(object sender, EventArgs e)
        {
        }

        protected void txtSubTotal_TextChanged(object sender, EventArgs e)
        {
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //preparaNuevaCarga();
            //limpiarEncabezado();
            //ViewState["ComprobanteEncabezado"] = null;
            //cargarGrilla();
            Response.Redirect("FrmVentas.aspx?action=new");
        }



        private void limpiarEncabezado()
        {

            cmbTipoResponsable.Text = null;
            cmbCliente.SelectedIndex = 0;
            txtrazonSocial.Text = null;
            txtingbruto.Text = null;
            txtCuit.Text = null;
            txtDireccion.Text = null;
            txtLocalidad.Text = null;
            txtProvincia.Text = null;
            cmbComprobante.Text = null;
            cmbSucursal.Text = null;
            txtNumero.Text = null;
            cmbTipo.Text = null;
            txtSubTotalNeto.Text = null;
            txtDescuento.Text = null;
            txtTotal.Text = null;
            cantReg = 0;

        }

        protected void txtDescuento_TextChanged(object sender, EventArgs e)
        {
            txtIVA.Focus();
        }

        protected void txtIVA_TextChanged(object sender, EventArgs e)
        {

            btnAgregar.Focus();
        }


        private bool verificaRequerido(TextBox objeto)
        {
            if (objeto.Text.Trim() != "")
            {
                objeto.BackColor = colorBien;
                return true;
            }
            else
            {
                objeto.BackColor = colorMal;
                return false;
            }

        }

        private bool verificaCuit(TextBox objeto)
        {
            if (Comun.UtilAFIP.ValidaCuit(objeto.Text))
            {
                objeto.BackColor = colorBien;
                return true;
            }
            else
            {
                objeto.BackColor = colorMal;
                return false;
            }

        }
        private void agregaItem()
        {
            grdItems.EditIndex = -1;
            grdItems.SelectedIndex = -1;
            menuOpciones.Visible = false;


            bool estado = true;
            estado &= verificaRequerido(txtDescripción);

            if (txtCantidad.Text.Trim().Length > 0)
                estado &= verificaRequerido(txtImporte);

            if (txtImporte.Text.Trim().Length > 0)
                estado &= verificaRequerido(txtCantidad);


            if (!estado)
            {
                return;
            }

            agregarItemaComprobante();




            CalcularTotales();

            grdItems.SelectedIndex = -1;

            txtCodigo.Focus();


        }


        private void CalcularTotales()
        {
            DAL.ComprobanteEncabezado encab = obtenerComprobante();
            decimal subtotalNeto = 0, total = 0, iva = 0;

            foreach (DAL.ComprobanteItem item in encab.itemsComprobante)
            {
                total += item.Subtotal;
                subtotalNeto += item.Subtotalneto;
                iva += item.MontoIVA;
            }

            txtTotal.Text = total.ToString("0.00", CultureInfo.InvariantCulture);
            txtSubTotalNeto.Text = subtotalNeto.ToString("0.00", CultureInfo.InvariantCulture);
            txtIvatotal.Text = iva.ToString("0.00", CultureInfo.InvariantCulture); ;
        }

        protected void grdItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grdItems_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {

        }



        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            agregaItem();
            txtCodigo.Focus();
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
        }
        // Unsubscribe

        #endregion

        protected void cmbComprobante_SelectedIndexChanged(object sender, EventArgs e)
        {
            //verificar si es con CAE
            DAL.TipoComprobante tc = new DAL.TipoComprobante();
            tc.obtener(cmbComprobante.SelectedValue);
            if (tc.EsConCAE)
                btnGuardarConCAE.Visible = true;
            else
                btnGuardarConCAE.Visible = false;




        }

        private void verficarProximoNumero()
        {

            List<string> verificados = new List<string>();
            if (ViewState["comprobantesVerificados"] != null && ViewState["comprobantesVerificados"] is List<string>)
                verificados = (List<string>)ViewState["comprobantesVerificados"];

            if (verificados.Contains("P" + int.Parse(cmbSucursal.SelectedValue.ToString()) +
                                      "-T" + int.Parse(cmbComprobante.SelectedValue)))
            {
                return;
            }

            int compr = obtenerNumeroComprobante(int.Parse(cmbSucursal.SelectedValue.ToString()),
                                     int.Parse(cmbComprobante.SelectedValue));

            DAL.ComprobanteEncabezado enc = new DAL.ComprobanteEncabezado();
            enc.obtener(int.Parse(cmbSucursal.SelectedValue.ToString()),
                        compr,
                        cmbComprobante.SelectedValue);

            if (enc.Id != 0)
            {
                string auxMsj = "Comprobante:  " + compr + "\\n Tipo:  " + cmbComprobante.SelectedItem + "\\n Punto Venta:  " + cmbSucursal.SelectedValue;

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", string.Format("alert('El próximo número de comprobante a generar ya existe en la base de datos \\n\\n {0}');", auxMsj), true);
                formularioItem.Enabled = false;
            }
            else
                formularioItem.Enabled = true;

        }

        protected void cmbCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            DAL.Cliente client = new DAL.Cliente();

            int aux = 0;
            int.TryParse(cmbCliente.SelectedValue, out aux);

            if (aux > 0)
            {

                client.obtener(aux);
                cmbTipoResponsable.SelectedValue = client.TipoResponsableCodigo;
                txtCuit.Text = client.CUIT.ToString();
                txtrazonSocial.Text = client.RazonSocial.ToString();

                txtingbruto.Text = client.IngresosBrutos;
                txtDireccion.Text = client.Domicilio;
                txtLocalidad.Text = client.Localidad;
            }
            else
            {
                txtCuit.Text = null;
                txtrazonSocial.Text = null;
                txtingbruto.Text = null;
                aux = 0;
            }


        }

        protected void btnSube_Click(object sender, EventArgs e)
        {
            int posActual = grdItems.SelectedIndex;

            if (grdItems.SelectedIndex > 0)
            {
                DAL.ComprobanteEncabezado ce = (DAL.ComprobanteEncabezado)ViewState["ComprobanteEncabezado"];

                List<DAL.ComprobanteItem> lista = ce.itemsComprobante;
                Comun.GenericsUtils p = new Comun.GenericsUtils();
                p.Move(lista, posActual, posActual - 1);

                ViewState["ComprobanteEncabezado"] = ce;

                cargarGrilla();
                grdItems.SelectedIndex = posActual - 1;
            }
            menuOpciones.Style.Add("margin-right", "200px");
        }

        protected void grdItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdItems.SelectedIndex == -1)
            {
                menuOpciones.Visible = false;
            }
        }

        protected void grdItems_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            grdItems.EditIndex = -1;
            if (grdItems.SelectedIndex == e.NewSelectedIndex)
            {
                grdItems.SelectedIndex = -1;
                e.Cancel = true;
                menuOpciones.Visible = false;
            }
            else
            {
                menuOpciones.Visible = true;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "menuOpciones();", true);
            }
        }

        protected void btnElimina_Click(object sender, EventArgs e)
        {
            DAL.ComprobanteEncabezado encab = (DAL.ComprobanteEncabezado)(ViewState["ComprobanteEncabezado"]);

            encab.itemsComprobante.RemoveAt(grdItems.SelectedIndex);

            ViewState["ComprobanteEncabezado"] = encab;

            controlCantidadItems(encab);

            cargarGrilla();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "menuOpcionesOculto();", true);
            menuOpciones.Style.Add("margin-right", "-=200px");

            preparaNuevaCarga();
            CalcularTotales();
            grdItems.SelectedIndex = -1;
        }

        protected void btnBaja_Click(object sender, EventArgs e)
        {
            int posActual = grdItems.SelectedIndex;

            if (grdItems.SelectedIndex < (grdItems.Rows.Count - 1))
            {
                DAL.ComprobanteEncabezado ce = (DAL.ComprobanteEncabezado)ViewState["ComprobanteEncabezado"];

                List<DAL.ComprobanteItem> lista = ce.itemsComprobante;
                Comun.GenericsUtils p = new Comun.GenericsUtils();
                p.Move(lista, posActual, posActual + 1);

                ViewState["ComprobanteEncabezado"] = ce;

                cargarGrilla();
                grdItems.SelectedIndex = posActual + 1;
            }
            menuOpciones.Style.Add("margin-right", "200px");
        }

        protected void btnReimprimir_Click(object sender, EventArgs e)
        {
            DAL.ComprobanteEncabezado ce = (DAL.ComprobanteEncabezado)ViewState["ComprobanteEncabezado"];
            if (ce.cen_numero != 0)
            {
                ce.imprimir();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('El documento fue puesto en cola de impresión...');", true);
            }
        }

        protected void btnGuardarConCAE_Click(object sender, EventArgs e)
        {

            DateTime inicio = DateTime.Now;
            int compr = 0;
            try
            {

                if (GuardarComprobante(false))
                {
                    DAL.ComprobanteEncabezado encab = (DAL.ComprobanteEncabezado)(ViewState["ComprobanteEncabezado"]);



                    DirectoryInfo di = new DirectoryInfo(ConfigurationManager.AppSettings["archivoconsulta"]);
                    FileInfo[] fi = di.GetFiles(encab.Id.ToString() + ".*");

                    bool hayRespuestaPrevia = false;

                    string archivo = "";

                    foreach (FileInfo item in fi)
                    {
                        if (item.Extension.ToLower() == ".req")
                        {
                            ScriptManager.RegisterClientScriptBlock(this,
                                this.GetType(), "alertMessage",
                                "alert('Disculpe, aún hay un pedido de CAE pendiente para este documento, " +
                                "por favor verifique el correcto funcionamiento del aplicativo para facturación " +
                                "electrónica');", true);

                            ScriptManager.RegisterClientScriptBlock(this,
                               this.GetType(), "habilitoCtrls",
                               "habilitoControles();", true);

                            //todo:hubo error
                            return;
                        }

                        if (item.Extension.ToLower() == ".tou")
                        {
                            item.Delete();
                        }


                        if (item.Extension.ToLower() == ".ecx")
                        {
                            item.Delete();
                        }


                        if (item.Extension.ToLower() == ".res")
                        {
                            archivo = item.FullName.Remove(item.FullName.Length - 4, 4);
                            hayRespuestaPrevia = true;

                        }
                    }

                    if (!hayRespuestaPrevia)
                        compr = obtenerNumeroComprobante(int.Parse(encab.pvt_codigo), int.Parse(encab.tco_codigo));

                    if (hayRespuestaPrevia || compr != 0)
                    {
                        DAL.Xml xml = new DAL.Xml();

                        string AfipReq = null;

                        if (!hayRespuestaPrevia)
                        {
                            archivo = crearXml(encab, compr, out AfipReq);
                            encab.cen_AfipReq = AfipReq;
                            encab.guardar();


                            string dirConsulta = ConfigurationManager.AppSettings["archivoconsulta"] + @"\" +
                                                encab.Id.ToString() + ".wsfe";

                            int tiempoEspera = Convert.ToInt32(ConfigurationManager.AppSettings["tiempoespera"]);

                            DateTime ini = DateTime.Now;
                            while (true)
                            {
                                TimeSpan ts = DateTime.Now - ini;

                                if (ts.TotalMilliseconds > tiempoEspera)
                                    break;

                                System.Threading.Thread.Sleep(500);

                                if (File.Exists(dirConsulta + ".res") ||
                                   File.Exists(dirConsulta + ".tou") ||
                                    File.Exists(dirConsulta + ".ecx"))
                                {
                                    System.Threading.Thread.Sleep(200);
                                    break;

                                }

                            }
                        }


                        if (!xml.leerXML(archivo))
                        {

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                "alertMessage", "alert('No se ha podido leer la respuesta desde AFIP. Intente nuevamente');", true);

                            ScriptManager.RegisterClientScriptBlock(this,
                               this.GetType(), "habilitoCtrls",
                               "habilitoControles();", true);

                            //todo:hubo error
                            return;
                        }



                        if (xml.huboTimeout)
                        {
                            //DAL.Database.RollbackTransaction();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No se ha podido grabar. Intente nuevamente');", true);

                            ScriptManager.RegisterClientScriptBlock(this,
                               this.GetType(), "habilitoCtrls",
                               "habilitoControles();", true);

                            //todo:hubo error
                            return;
                        }

                        if (xml.CAE != null && xml.Resultado == "A")
                        {
                            if (AfipReq != null)
                                encab.cen_AfipReq = AfipReq;
                            encab.cen_numero = int.Parse(xml.CbteDesde);
                            encab.cen_AfipRes = xml.RespuestaAFIP;
                            encab.cen_Cae = xml.CAE;
                            encab.cen_CuitContribuyente = xml.Cuit;
                            encab.cen_CaeFechaVencimiento = DateTime.ParseExact(xml.CAEFchVto, "yyyyMMdd", CultureInfo.InvariantCulture);

                            DAL.Database.BeginTransaction("fact");
                            //encab.Subscribe(this);
                            encab.cen_fecha = DateTime.ParseExact(xml.FchProceso, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                            encab.guardar();
                            if (encab.UltimoMensaje != null)
                            {
                                DAL.Database.RollbackTransaction();
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No se ha podido grabar. Intente nuevamente');", true);

                                ScriptManager.RegisterClientScriptBlock(this,
                                   this.GetType(), "habilitoCtrls",
                                   "habilitoControles();", true);

                                //todo:hubo error
                                return;
                            }

                            //solo si no hubo error
                            DAL.Database.CommitTransaction();


                            TimeSpan ts = DateTime.Now - inicio;

                            string auxMsj = "Comprobante:  " +
                                encab.cen_numeroCompleto + "\\nCAE:  " +
                                encab.cen_Cae + "\\nFecha Vencimiento:  " +
                                encab.cen_CaeFechaVencimiento.ToString("dd/MM/yy") +
                                "\\nTiempo respuesta:" + ts.TotalSeconds + "seg.";
                            string obse = "";
                            if (xml.Observaciones != null && xml.Observaciones.Count > 0)
                            {
                                obse = "Observaciones:\\n";
                                foreach (System.Collections.Generic.KeyValuePair<string, string> item in xml.Observaciones)
                                {
                                    obse += item.Key + " " + item.Value + "\\n";
                                }
                                auxMsj += "\\n\\n" + obse;
                            }


                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Se ha generado correctamente \\n\\n" + auxMsj + "');", true);

                            ScriptManager.RegisterClientScriptBlock(this,
                               this.GetType(), "habilitoCtrls",
                               "habilitoControles();", true);

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                             "redir",
                             "location.href = \"FrmVentas.aspx?action=new\";", true);

                            return;
                        }
                        else
                        {
                            DAL.Database.RollbackTransaction();
                            string obs = "";
                            if (xml.Observaciones != null && xml.Observaciones.Count > 0)
                            {
                                obs = "Observaciones:\\n";
                                foreach (System.Collections.Generic.KeyValuePair<string, string> item in xml.Observaciones)
                                {
                                    obs += item.Key + " " + item.Value + "\\n";
                                }
                            }
                            string err = "";
                            if (xml.Errores != null && xml.Errores.Count > 0)
                            {
                                err = "Errores:\\n";
                                foreach (System.Collections.Generic.KeyValuePair<string, string> item in xml.Errores)
                                {
                                    err += item.Key + " " + item.Value + "\\n";
                                }
                            }

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "btnGuardarConCAE_Click_3", string.Format("alert('No se ha podido grabar.\\n\\n {0}');", obs), true);

                            try
                            {
                                if (File.Exists(archivo + ".res"))
                                    File.Move(archivo + ".res", archivo + ".res.conError");
                            }
                            catch { }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DAL.Database.RollbackTransaction();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No se ha podido grabar. Intente nuevamente');", true);

                ScriptManager.RegisterClientScriptBlock(this,
                   this.GetType(), "habilitoCtrls",
                   "habilitoControles();", true);

                //todo:hubo error
                return;
            }
        }



        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {

            DAL.ComprobanteEncabezado encab = (DAL.ComprobanteEncabezado)(ViewState["ComprobanteEncabezado"]);

            encab.itemsComprobante.RemoveAt(grdItems.SelectedIndex);

            ViewState["ComprobanteEncabezado"] = encab;

            controlCantidadItems(encab);

            cargarGrilla();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "menuOpcionesOculto();", true);
            menuOpciones.Style.Add("margin-right", "-=200px");

            preparaNuevaCarga();
            CalcularTotales();
            grdItems.SelectedIndex = -1;

        }

        protected void grdItems_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdItems.SelectedIndex = -1;
            menuOpciones.Visible = false;
            //Set the edit index.
            grdItems.EditIndex = e.NewEditIndex;
            //Bind data to the GridView control.
            cargarGrilla();
        }

        protected void grdItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdItems.EditIndex = -1;
            //Bind data to the GridView control.
            cargarGrilla();
        }

        protected void grdItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DAL.ComprobanteEncabezado encab = (DAL.ComprobanteEncabezado)(ViewState["ComprobanteEncabezado"]);
            if (e.NewValues["Codigo"] != null)
                encab.itemsComprobante[e.RowIndex].Codigo = e.NewValues["Codigo"].ToString();

            if (e.NewValues["Descripcion"] != null)
                encab.itemsComprobante[e.RowIndex].Descripcion = e.NewValues["Descripcion"].ToString();

            decimal dTemp = 0;

            if (e.NewValues["Cantidad"] != null &&
                decimal.TryParse(e.NewValues["Cantidad"]
                        .ToString()
                        .Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                        .Replace(",", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                        , out dTemp))
                encab.itemsComprobante[e.RowIndex].Cantidad = dTemp;
            dTemp = 0;

            if (e.NewValues["Precio"] != null &&
                decimal.TryParse(e.NewValues["Precio"]
                       .ToString()
                       .Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                       .Replace(",", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                       , out dTemp))
                encab.itemsComprobante[e.RowIndex].Precio = dTemp;

            if (e.NewValues["AlicuotaPorcentaje"] != null &&
                decimal.TryParse(e.NewValues["AlicuotaPorcentaje"]
                       .ToString()
                       .Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                       .Replace(",", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                       , out dTemp))
            {
                DAL.IAlicuotaIva ai = new DAL.AlicuotaIva();
                ai.obtener(dTemp);
                if (ai.Codigo != null)
                    encab.itemsComprobante[e.RowIndex].AlicuotaPorcentaje = dTemp;
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", string.Format("alert('Alicuota de iva no válida.');"), true);
                }
            }

            ViewState["ComprobanteEncabezado"] = encab;

            grdItems.EditIndex = -1;
            cargarGrilla();
        }

        protected void grdItems_DataBound(object sender, EventArgs e)
        {
            if (grdItems.EditIndex > -1)
                grdItems.Columns[6].Visible = false;
            else
                grdItems.Columns[6].Visible = true;

        }







    }
}

