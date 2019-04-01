using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGECA.Forms.RRHH
{
    public partial class frmCambiarClave : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DAL.Empleados emp = new DAL.Empleados();
            emp = emp.Obtener(Session["id"].ToString());
            if(emp.Password.ToString()==DAL.Varios.MD5Hash(txtClaveAnterior.Text))
            {
                if(txtClaveNueva.Text==txtRepetirClave.Text)
                {
                    emp.Password = txtClaveNueva.Text;
                    emp.Guardar(false);
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("Operación realizada!", 3000), true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("Las claves no coinciden", 3000), true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "notification", DAL.Varios.crear_mensaje("La clave anterior no es válida", 3000), true);
            }
        }
    }
}