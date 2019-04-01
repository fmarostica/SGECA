using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGECA
{
    public partial class recupera_clave : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DAL.Empleados emp = new DAL.Empleados();
            emp = emp.Obtener_por_key(Request.QueryString["key"]);
            if(emp.Id>0)
            {
                panel_clave.Visible = true;
            }
            else
            {
                lblinfo.Text = "La clave solicitada no existe";
                panel_clave.Visible = false;
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            DAL.Empleados emp = new DAL.Empleados();
            emp = emp.Obtener_por_key(Request.QueryString["key"]);
            if (emp.Id > 0)
            {
                if (txtClave.Text != "")
                {
                    if (txtClave.Text == txtRepetirClave.Text)
                    {
                        emp.Password = txtClave.Text;
                        emp.recovery_key = "";
                        emp.Guardar(false);

                        Response.Redirect("frmLogin.aspx");
                    }
                    else
                    {
                        lblinfo.Text = "Las claves no coinciden";
                    }
                }
                else
                {
                    lblinfo.Text = "La clave no puede estar vacía";
                }
            }
            else
            {
                lblinfo.Text = "La clave solicitada no existe";
                panel_clave.Visible = false;
            }
        }
    }
}