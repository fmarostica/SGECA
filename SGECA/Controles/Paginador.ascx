<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Paginador.ascx.cs" Inherits="SGECA.Controles.Paginador" %>
<style type="text/css">
    .margin-top {
        margin-top: 10px;
    }
</style>
<table class="paginador">
    <tr>
        <td>
            <table>
                <tr>
                    <td>Mostrar:</td>
                    <td>
                        <asp:DropDownList ID="cboMostrar" Width="50px" runat="server" OnSelectedIndexChanged="cboMostrar_SelectedIndexChanged">
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>30</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>Registros</td>
                    <td>|</td>
                    <td>Página</td>
                    <td>
                        <asp:DropDownList ID="cboPagina" Width="50px" runat="server" OnSelectedIndexChanged="cboPagina_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>de</td>
                    <td>
                        <asp:Label ID="lblPaginas" runat="server" Text="####"></asp:Label>
                    </td>
                    <td>|</td>
                    <td>
                        <asp:Button ID="btnInicio" runat="server" OnClick="btnInicio_Click" Text="&lt;&lt; Inicio" Enabled="False" />
                    </td>
                    <td>
                        <asp:Button ID="btnAnterior" runat="server" OnClick="btnAnterior_Click" Text="&lt;" Enabled="False" />
                    </td>
                    <td>
                        <asp:Button ID="btnProxima" runat="server" OnClick="btnProxima_Click" Text="&gt;" Enabled="False" />
                    </td>
                    <td style="font-weight: 700">
                        <asp:Button ID="btnFin" runat="server" Text="Fin &gt;&gt;" Enabled="False" />
                    </td>
                    <td>Registros Totales:</td>
                    <td>
                        <asp:Label ID="lblRegistros" runat="server" Text="0"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
