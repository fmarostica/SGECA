using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGECA
{
    public partial class frmForgotPass : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRecuperar_Click(object sender, EventArgs e)
        {
            DAL.Empleados emp = new DAL.Empleados();
            emp = emp.Obtener_por_mail(txtUsuario.Text);

            if(emp.Id>0)
            {
                if(emp.Fecha_Baja=="" || emp.Fecha_Baja==null)
                {
                    string key = DAL.Varios.randomString(20);
                    emp.recovery_key = key;
                    emp.Guardar(false);

                    MailMessage message = new MailMessage("info@grovell.com", txtUsuario.Text, "Recuperacion de clave", "Para recuperar su clave haga click en el siguiente enlace: http://181.28.4.63:8082/recupera_clave.aspx?key=" + key);
                    SmtpClient client = new SmtpClient("grovell.com");
                    client.Credentials = new NetworkCredential("info@grovell.com", "Galletita2602");
                    client.EnableSsl = false;
                    client.Port = 25;

                    client.Send(message);

                    lblinfo.Text = "Hemos enviado un correo a su casilla";
                    txtUsuario.Text = "";
                }
                else
                {
                    lblinfo.Text = "No se encontro ningun usuario con ese mail";
                }
            }
            else
            {
                lblinfo.Text = "No se encontro ningun usuario con ese mail";
            }
            
        }
    }
}