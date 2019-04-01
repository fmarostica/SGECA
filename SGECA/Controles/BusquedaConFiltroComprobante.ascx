<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BusquedaConFiltroComprobante.ascx.cs" Inherits="SGECA.Controles.BusquedaConFiltroComprobante" %>
<fieldset class="Panel1021">
    <legend>Búsqueda y/o Filtrado</legend>
    <asp:Panel ID="Panel1" runat="server">
        <table>
            <tr>
                <td style="width: 10%">Desde</td>
                <td style="width: 30%">
                    <asp:TextBox placeholder="dd/mm/aaaa" ID="txtDesde" class="input1" runat="server" TextMode="Date" Height="25px"/></td>
                <td style="width: 10%">Punto de Venta</td>
                <td style="width: 10%">
                    <asp:DropDownList ID="cmbSucursal" runat="server" class="input1" Height="25px" Width="378px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Hasta</td>
                <td>
                    <asp:TextBox ID="txtHasta" runat="server" class="input1" placeholder="dd/mm/aaaa" TextMode="Date" Height="25px"/>
                </td>
                <td>Tipo</td>
                <td>
                    <asp:DropDownList ID="cmbComprobante" runat="server" class="input1" Height="25px" Width="378px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Cliente</td>
                <td>
                    <asp:DropDownList ID="cmbCliente" runat="server" AutoPostBack="true" class="input1" Height="25px" Width="378px">
                    </asp:DropDownList>
                </td>
                <td>Número</td>
                <td>
                    <asp:TextBox ID="txtNumero" runat="server" class="input1" Height="25px" TextMode="Number" Width="200px" />
                </td>
            </tr>

            <tr>
                <td>&nbsp;</td>
                <td colspan="3">
                    <asp:Button ID="btnBuscar" runat="server" CssClass="buttonMenu " OnClick="btnBuscar_Click" Text="Buscar" Width="100px" />
                    &nbsp;
                    <asp:Button ID="btnLimpiar" runat="server" CssClass="buttonMenu alineacion2" OnClick="btnLimpiar_Click" Text="Limpiar" Width="100px" />
                </td>
            </tr>

        </table>


    </asp:Panel>
</fieldset>





