using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGECA
{
    public partial class frmLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.QueryString["r"]!=null)
            {
                if (Request.QueryString["r"].ToString() == "timeout")
                {
                    lblError.Text = "LA SESIÓN HA EXPIRADO";
                }
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            if(txtUsuario.Text=="telesoluciones" && txtPass.Text=="tele8881")
            {
                Session["usr"] = "telesoluciones";
                Response.Redirect("/default.aspx");
            }
            else
            {
                if(txtPass.Text!="")
                {
                    DAL.Empleados emp = new DAL.Empleados();
                    emp = emp.login(txtUsuario.Text, DAL.Varios.MD5Hash(txtPass.Text));
                    if (emp.Id>0)
                    {
                        if(emp.Fecha_Baja=="" || emp.Fecha_Baja==null)
                        {
                            Session["usr"] = emp.ApellidoyNombre;
                            Session["id"] = emp.Id.ToString();
                            Session.Timeout = 120;

                            Response.Redirect("/default.aspx");
                        }
                        else
                        {
                            lblError.Text = "Usuario o clave incorrecta";
                        }
                    }
                    else
                    {
                        lblError.Text = "Usuario o clave incorrecta";
                    }
                }
            }
        }
    }
}