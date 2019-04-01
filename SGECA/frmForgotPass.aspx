<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmForgotPass.aspx.cs" Inherits="SGECA.frmForgotPass" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Telesoluciones - Iniciar sesión</title>
    <link href="../../Estilos/StyleSheet.css" rel="stylesheet" />
    <style>
        .bg{
            background: #ffdddd; /* Old browsers */
            background: -moz-linear-gradient(top,  #ffdddd 0%, #ffffff 18%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#ffdddd), color-stop(18%,#ffffff)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top,  #ffdddd 0%,#ffffff 18%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top,  #ffdddd 0%,#ffffff 18%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top,  #ffdddd 0%,#ffffff 18%); /* IE10+ */
            background: linear-gradient(to bottom,  #ffdddd 0%,#ffffff 18%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffdddd', endColorstr='#ffffff',GradientType=0 ); /* IE6-9 */
        }
    </style>
</head>
<body class="bg">

    <div style="background: #ff0000; height: 20px"></div>
    <form id="form1" runat="server">
    <div class="cuadro800">
        <img src="../../Imagenes/telesoluciones.png" style="height: 120px"/>

        <img src="../../Imagenes/sombra.png" />
        <div class="cuadroLogin" >
            <div class="cuadroLogin_contenido">
                <b>Recuperación de clave</b>
                <br /><br />
                Ingrese la dirección de correo asociada a su usuario, le enviaremos un correo con los pasos correspondientes para la recuperación de su clave.
                <hr style="width: 90%; margin-top: 15px; margin-bottom: 25px" />
                <table>
                    <tr>
                        <td style="text-align: right;">E-mail:</td>
                        <td><asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox></td>
                    </tr>
                </table>
                <br />
                <asp:Label runat="server" CssClass="lblError" ID="lblinfo" Text=""></asp:Label>
                <br />
                <asp:Button ID="btnRecuperar" runat="server" CssClass="buttonModal" Text="Recuperar" OnClick="btnRecuperar_Click"/>
            </div>
        </div>
        <img src="../../Imagenes/sombra_fin.png" />

    </div>
    </form>
</body>
</html>

