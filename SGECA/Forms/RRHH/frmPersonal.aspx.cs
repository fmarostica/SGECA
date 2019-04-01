using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Drawing;
using System.Data;
using System.Text.RegularExpressions;

namespace SGECA.Forms.Empleados
{
    public partial class frmPersonas : System.Web.UI.Page, LogManager.ISubject, LogManager.IObserver
    {
        DAL.Empleados_logs logs = new DAL.Empleados_logs();
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

            if(!this.IsPostBack)
            {
                txtFechaAlta.Text = DateTime.Now.ToShortDateString();
                txtFechaBaja.Text = "";
                txtPassword.Text = "";

                frmSeleccionar.Visible = false;
                cargar_provincias();
                cargar_estado_civil();
                cargar_grupos();
            }
        }

        void cargar_grupos()
        {
            DAL.EmpleadosGrupos grupos = new DAL.EmpleadosGrupos();
            List<DAL.EmpleadosGrupos> lista_grupos = new List<DAL.EmpleadosGrupos>();

            lista_grupos.Add(new DAL.EmpleadosGrupos { Id = "", Nombre = "" });

            lista_grupos.AddRange(grupos.obtener());

            txtGrupo.DataSource = lista_grupos;
            txtGrupo.DataValueField = "id";
            txtGrupo.DataTextField = "nombre";
            txtGrupo.DataBind();
        }

        void cargar_provincias()
        {
            DAL.Provincias provincias = new DAL.Provincias();
            List<DAL.Provincias> lista_provincias = provincias.Obtener();
            txtProvincia.DataSource = lista_provincias;
            txtProvincia.DataValueField = "id";
            txtProvincia.DataTextField = "nombre";
            txtProvincia.DataBind();
            txtProvincia.SelectedIndex = 5;
        }

        void cargar_estado_civil()
        {
            DAL.Estado_Civil eCivil = new DAL.Estado_Civil();
            List<DAL.Estado_Civil> lista_estados_civil = eCivil.Obtener();
            txtEstadoCivil.DataSource = lista_estados_civil;
            txtEstadoCivil.DataValueField = "id";
            txtEstadoCivil.DataTextField = "nombre";
            txtEstadoCivil.DataBind();

            txtEstadoCivil.SelectedIndex = 4;
        }

        internal DAL.ItemFiltro[] ObtenerItemFiltro()
        {
            List<DAL.ItemFiltro> items = new List<DAL.ItemFiltro>();

            DAL.ItemFiltro fil = new DAL.ItemFiltro();
            fil.itemBusqueda = new DAL.ItemBusqueda();
            fil.itemBusqueda.campo = "apellido";
            fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
            fil.itemBusqueda.Value = "apellido"; 
            fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
            fil.tipoFiltroTexto.value = DAL.TipoFiltro.Like;
            fil.textoBusqueda = txtBuscar.Text;
            items.Add(fil);

            fil = new DAL.ItemFiltro();
            fil.itemBusqueda = new DAL.ItemBusqueda();
            fil.itemBusqueda.campo = "nombre";
            fil.itemBusqueda.Tipo = DAL.ItemBusqueda.TipoCampo._string;
            fil.itemBusqueda.Value = "nombre";
            fil.tipoFiltroTexto = new DAL.TipoFiltroTexto();
            fil.tipoFiltroTexto.value = DAL.TipoFiltro.Like;
            fil.textoBusqueda = txtBuscar.Text;
            items.Add(fil);

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

            DAL.Empleados VistaEmpleados = new DAL.Empleados();

            VistaEmpleados.Subscribe(this);

            double cantidadRegistros = 0;

            List<DAL.Empleados> datosObtenidos = VistaEmpleados.obtenerFiltrado(filtro,
                                                   orden,
                                                   false,
                                                   registroInicio,
                                                   registroFin,
                                                   out  cantidadRegistros, false);
            if (VistaEmpleados.UltimoMensaje != null)
            {
                UltimoMensaje = VistaEmpleados.UltimoMensaje;
                Notify(UltimoMensaje);
                return;
            }

            ArrayList lista = new ArrayList();
            foreach (DAL.Empleados item in datosObtenidos)
            {
                var itemLista = new
                {
                    Id = item.Id,
                    Apellido = item.Apellido,
                    Nombre = item.Nombre,
                    Mail = item.Mail,
                    Grupo = item.Grupo
                };
                lista.Add(itemLista);
            }

            cargarGrilla(lista);
            calcularTotalPaginas(tamañoPagina, cantidadRegistros);

            pagPaginador.setPaginaActual(paginaActual);
        }

        void cargarGrilla(ArrayList lista)
        {
            grdEmpleados.DataSource = lista;
            grdEmpleados.DataBind();
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
                orden[0].Campo = "apellido";
                orden[0].TipoOrden = DAL.TipoOrden.Ascendente;
                ViewState["orden[0].TipoOrden"] = DAL.TipoOrden.Ascendente.ToString();
                ViewState["orden[0].Campo"] = orden[0].Campo;
            }
            return new DAL.ItemOrden[0];

        }

        protected void pagPaginador_Fin()
        {
            pagPaginador.setPaginaFinal();
            obtenerDatosFiltrados(false, obtenerOrdenActual(),this.ObtenerItemFiltro());
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

        protected void grdEmpleados_SelectedIndexChanged(object sender, EventArgs e)
        {
            frmSeleccionar.Visible = false;

            LimpiarControles();
            int id = 0;
            int.TryParse(grdEmpleados.SelectedDataKey.Value.ToString(), out id);
            if (id != 0)
            {
                DAL.Empleados emp = new DAL.Empleados();
                emp = emp.Obtener(id.ToString());
                
                if(ViewState["EmpleadoID"]!=null)
                    ViewState["EmpleadoID"] = emp.Id;
                else
                    ViewState.Add("EmpleadoID", emp.Id);

                txtCodigo.Text = emp.Id.ToString();
                txtApellido.Text = emp.Apellido;
                txtNombre.Text = emp.Nombre;
                txtFechaAlta.Text = emp.Fecha_Alta;
                txtFechaBaja.Text = emp.Fecha_Baja;
                txtFechaCierre.Text = emp.Fecha_Cierre;
                txtGrupo.SelectedValue = emp.Grupo;
                txtTelLaboral.Text = emp.Tel_laboral;
                txtTelPersonal.Text = emp.Tel_personal;
                txtPassword.Text = emp.Password;
                txtPassword.Attributes.Add("value", emp.Password);
                txtMail.Text = emp.Mail;
                txtFechaNacimiento.Text = emp.Fecha_Nacimiento;
                txtDomicilio.Text = emp.Domicilio;
                txtCUIL.Text = emp.CUIL;
                txtTarea.Text = emp.Tarea;
                txtHijos.Text = emp.hijos.ToString();
                txtContacto.Text = emp.Persona_contacto;
                txtTelAlternativo.Text = emp.Tel_Alternativo;

                txtProvincia.ClearSelection();
                //txtProvincia.Items.FindByValue(emp.Provincia).Selected = true;
                txtProvincia.SelectedValue = emp.Provincia;

                txtEstadoCivil.ClearSelection();
                //txtEstadoCivil.Items.FindByValue(emp.Estado_Civil).Selected = true;
                txtEstadoCivil.SelectedValue = emp.Estado_Civil;

                chPercibeAdelantos.Checked = emp.PercibeAdelantos;
            }
        }

        protected void grdEmpleados_Sorted(object sender, EventArgs e)
        {
            //obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void grdEmpleados_Sorting(object sender, GridViewSortEventArgs e)
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

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            obtenerDatosFiltrados(false, obtenerOrdenActual(), this.ObtenerItemFiltro());
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("Controles limpiados", 3000), true);
        }

        void LimpiarControles()
        {
            ViewState["EmpleadoID"] = null;
            txtApellido.Text = "";
            txtCodigo.Text = "";
            txtFechaAlta.Text = "";
            txtFechaBaja.Text = "";
            txtFechaCierre.Text = "";
            txtMail.Text = "";
            txtNombre.Text = "";
            txtPassword.Text = "";
            txtTelLaboral.Text = "";
            txtTelPersonal.Text = "";
            txtCUIL.Text = "";
            txtDomicilio.Text = "";
            txtFechaNacimiento.Text = "";
            txtHijos.Text = "";
            txtTarea.Text = "";
            txtPassword.Attributes.Add("value", "");
            txtTelAlternativo.Text = "";
            txtContacto.Text = "";

            txtEstadoCivil.ClearSelection();
            txtProvincia.Items.FindByValue("5").Selected = true;
            txtProvincia.ClearSelection();
            txtProvincia.Items.FindByValue("6").Selected = true;

            lblNombre.ForeColor = Color.Black;
            lblApellido.ForeColor = Color.Black;
            lblFechaAlta.ForeColor = Color.Black;
            lblFechaBaja.ForeColor = Color.Black;
            lblFechaCierre.ForeColor = Color.Black;

            txtGrupo.SelectedIndex = 0;

            chPercibeAdelantos.Checked = false;
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            if(!DAL.Varios.CUITValido(txtCUIL.Text))
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("Error al realizar la operacion, el número de CUIL debe ser válido"), true);
                lblCUIL.ForeColor = Color.Red;
            }
            else
            {
                lblCUIL.ForeColor = Color.Black;
                GuardarEmpleado(true);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!DAL.Varios.CUITValido(txtCUIL.Text))
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("Error al realizar la operacion, el número de CUIL debe ser válido"), true);
                lblCUIL.ForeColor = Color.Red;
            }
            else
            {
                if (ViewState["EmpleadoID"] != null)
                    GuardarEmpleado(false);
                else
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("Debe seleccionar un registro para poder guardarlo"), true);
            }
        }

        void GuardarEmpleado(bool nuevo)
        {
            DAL.Empleados emp = new DAL.Empleados();
            

            bool errores = false; //bandera utilizada para controlar errores y continuar o no con la actulizacion de registros

            int id = 0;
            if(txtCodigo.Text!="") // comprueba que en caso de que se ingrese un codigo manualmente
            {
                try
                {
                    id = Convert.ToInt32(txtCodigo.Text); //el codigo manual debe ser numerico
                }
                catch //caso contrario se da un error e informa al usuario
                {
                    errores = true;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("El campo código debe ser un valor númerico mayor a cero"), true);
                }
            }

            if (ViewState["EmpleadoID"] != null) //Si se esta actualizando
            {
                if (txtCodigo.Text != ViewState["EmpleadoID"].ToString())
                {
                    errores = true;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("El legajo no puede ser modificado!"), true);
                }
            }

            if(txtGrupo.Text!="")
            {
                DAL.EmpleadosGrupos grupo = new DAL.EmpleadosGrupos();
                if (!grupo.existe_grupo(txtGrupo.Text))
                {
                    errores = true;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("El grupo especificado no existe"), true);
                }
            }

            if(nuevo && id>0) //Si se ingresa un id manual y se trata de un nuevo registro comprueba que dicho id no exista en la bd
            {
                if(emp.existe_id(id.ToString()))
                {
                    errores = true;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("La operación no pudo completarse porque el codigo ingresado ya existe. Si lo desea puede dejar este campo en blanco para generar un código automaticamente."), true);
                }
            }

            if (txtNombre.Text == "" || txtApellido.Text=="" || txtFechaAlta.Text == "") //Verifico que los campos nombre, apellido y fecha de alta esten completos
            {
                if (txtNombre.Text == "") lblNombre.ForeColor = Color.Red;
                if (txtApellido.Text == "") lblApellido.ForeColor = Color.Red;
                if (txtFechaAlta.Text == "") lblFechaAlta.ForeColor = Color.Red;

                errores = true;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("Los campos señalados en rojo son requeridos y no pueden estar vacío"), true);
            }

            try
            {
                if(txtFechaAlta.Text!="")  Convert.ToDateTime(txtFechaAlta.Text);
            }
            catch
            {
                errores = true;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("La fecha de alta debe ser una fecha válida"), true);
                lblFechaAlta.ForeColor = Color.Red;
            }

            try
            {
                if (txtFechaCierre.Text != "") Convert.ToDateTime(txtFechaCierre.Text);
            }
            catch
            {
                errores = true;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("La fecha de cierre debe ser una fecha válida"), true);
                lblFechaCierre.ForeColor = Color.Red;
            }

            try
            {
                if (txtFechaBaja.Text != "") Convert.ToDateTime(txtFechaBaja.Text);
            }
            catch
            {
                errores = true;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("La fecha de baja debe ser una fecha válida"), true);
                lblFechaBaja.ForeColor = Color.Red;
            }

            if(txtMail.Text!="")
            {
                string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

                if (!Regex.IsMatch(txtMail.Text, pattern))
                {
                    errores = true;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("La direccion de mail no es correcta!. Ingrese una dirección válida"), true);
                }
            }

            if(!errores) //si no se presentan errores continua el guardado/actualizacion de registros
            {
                int hijos = 0;
                int.TryParse(txtHijos.Text, out hijos);

                emp.Id = id;
                emp.Apellido = txtApellido.Text;
                emp.Nombre = txtNombre.Text;
                emp.Fecha_Alta = txtFechaAlta.Text;
                emp.Fecha_Baja = txtFechaBaja.Text;
                emp.Fecha_Cierre = txtFechaCierre.Text;
                emp.Mail = txtMail.Text;
                emp.Grupo = txtGrupo.SelectedItem.Value;
                if (nuevo)
                {
                    if (txtPassword.Text != "") emp.Password = DAL.Varios.MD5Hash(txtPassword.Text);
                }
                else
                {
                    emp.Password = txtPassword.Text;
                }
                emp.Tel_laboral = txtTelLaboral.Text;
                emp.Tel_personal = txtTelPersonal.Text;
                emp.CUIL = txtCUIL.Text;
                emp.Domicilio = txtDomicilio.Text;
                emp.Tel_Alternativo = txtTelAlternativo.Text;
                emp.Persona_contacto = txtContacto.Text;

                emp.Provincia = txtProvincia.SelectedItem.Value;
                emp.Estado_Civil = txtEstadoCivil.SelectedItem.Value;
                
                emp.hijos = hijos;
                emp.Tarea = txtTarea.Text;
                emp.Fecha_Nacimiento = txtFechaNacimiento.Text;

                emp.PercibeAdelantos = chPercibeAdelantos.Checked;

                emp.Guardar(nuevo);
                
                if(nuevo)
                {
                    logs.modulo = "RRHH - PERSONAL";
                    logs.accion = "ALTA DE EMPLEADO: " + txtApellido.Text.ToUpper() + ", " + txtNombre.Text.ToUpper();
                    logs.usuario = Session["usr"].ToString();
                    if(Session["id"]!=null) logs.empleado_id = Session["id"].ToString();
                }
                else
                {
                    logs.modulo = "RRHH - PERSONAL";
                    logs.accion = "MODIFICACION DE EMPLEADO: " + txtApellido.Text.ToUpper() + ", " + txtNombre.Text.ToUpper();
                    logs.usuario = Session["usr"].ToString();
                    if (Session["id"] != null) logs.empleado_id = Session["id"].ToString();
                }

                logs.guardar();

                LimpiarControles();

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Operacion realizada!",3000), true);
            }
        }

        protected void grdEmpleados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(grdEmpleados, "Select$" + e.Row.RowIndex.ToString()));
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (ViewState["EmpleadoID"] != null)
            {
                DAL.Empleados emp = new DAL.Empleados();
                if (!emp.tiene_tareas_asignadas(Convert.ToInt32(ViewState["EmpleadoID"])) 
                    && !emp.tiene_adelantos_asignados(Convert.ToInt32(ViewState["EmpleadoID"])))
                {
                    if (emp.borrar(Convert.ToInt32(ViewState["EmpleadoID"])))
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Empleado borrado!", 3000), true);
                        obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());

                        logs.modulo = "RRHH - PERSONAL";
                        logs.accion = "BORRO EL EMPLEADO: " + emp.Apellido.ToUpper() + ", " + emp.Nombre.ToUpper();
                        logs.usuario = Session["usr"].ToString();
                        if (Session["id"] != null) logs.empleado_id = Session["id"].ToString();
                        logs.guardar();

                        LimpiarControles();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("El empleado no puede ser borrado ya que posee registros relacionados.", 3000), true);
                }
            }
            else
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "toast", DAL.Varios.crear_mensaje("Debe seleccionar un empleado para poder borrarlo", 3000), true);
        }

        protected void txtCUIL_TextChanged(object sender, EventArgs e)
        {
            
        }

        protected void btnSeleccionarClose_Click(object sender, EventArgs e)
        {
            frmSeleccionar.Visible = false;
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            obtenerDatosFiltrados(false, obtenerOrdenActual(), ObtenerItemFiltro());
            frmSeleccionar.Visible = true;
        }
              

    }
}