<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/rrhh.master" AutoEventWireup="true" CodeBehind="frmCambiarClave.aspx.cs" Inherits="SGECA.Forms.RRHH.frmCambiarClave" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
        <div class="main_contenido">
            <table class="TablaDetalles">
                <tr>
                    <td style="width: 120px;"><asp:Label ID="lblClaveAnterior" CssClass="lblField" Text="Clave Anterior : " runat="server"></asp:Label></td>
                    <td><asp:TextBox ID="txtClaveAnterior" CssClass="input1" TextMode="Password" Width="150px" runat="server" ></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 120px;"><asp:Label ID="Label1" CssClass="lblField" Text="Clave Nueva : " runat="server"></asp:Label></td>
                    <td><asp:TextBox ID="txtClaveNueva" CssClass="input1" TextMode="Password" Width="150px" runat="server" ></asp:TextBox></td>
                </tr> 
                <tr>
                    <td style="width: 120px;"><asp:Label ID="Label2" CssClass="lblField" Text="Repetir Clave : " runat="server"></asp:Label></td>
                    <td><asp:TextBox ID="txtRepetirClave" CssClass="input1" TextMode="Password" Width="150px" runat="server" ></asp:TextBox></td>
                </tr>
            </table>
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar cambios" CssClass="buttonModal" OnClick="btnGuardar_Click" UseSubmitBehavior="false" />
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>
