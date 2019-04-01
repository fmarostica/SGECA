<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BusquedaConFiltro.ascx.cs" Inherits="SGECA.Controles.BusquedaConFiltro" %>
<style type="text/css">
    .auto-style1 {
        height: 26px;
    }

    .eventFieldset {
        border-color: #0D4E5D;
    }
</style>
<table style="width: 1021px; overflow: auto; margin: 0 auto;">
    <tr>
        <td>
            <fieldset class="eventFieldset" style="text-align: left; width: 99%;">
                <legend class="legend">Búsqueda y/o Filtrado</legend>
                <asp:Panel ID="Panel1" runat="server">

                    <table style="width: 100%;">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblDescripcion" runat="server">Buscar por Descripción:</asp:Label>
                            </td>
                            <td style="position: relative; left: 15px;">Campo</td>
                            <td colspan="3">
                                <asp:DropDownList CssClass="input1" ID="cboCampo" runat="server" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="cboCampo_SelectedIndexChanged"></asp:DropDownList></td>
                            <td colspan="2" rowspan="3" style="position: relative; left: 20px;">
                                <asp:ListBox ID="lstLista" runat="server" Width="300px"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1" colspan="2">
                                <asp:TextBox ID="txtDescripcion" CssClass="input1" runat="server" Width="250px" OnTextChanged="txtDescripcion_TextChanged"></asp:TextBox>
                            </td>
                            <td style="position: relative; left: 15px;">Condicion</td>
                            <td class="auto-style1" colspan="3">
                                <asp:DropDownList ID="cboCondicion" CssClass="input1" Width="250px" runat="server"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td style="position: relative; left: 15px;">Valor</td>
                            <td colspan="3">
                                <asp:TextBox ID="txtValor" CssClass="input1" runat="server" Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnBuscar" CssClass="buttonMenu " Width="100px" runat="server" OnClick="btnBuscar_Click" Text="Buscar" />
                            </td>
                            <td>
                                <asp:Button ID="btnLimpiar" runat="server" CssClass="buttonMenu alineacion2" Width="100px" Text="Limpiar" OnClick="btnLimpiar_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnAgregar" CssClass="buttonMenu alineacion" Width="70px" runat="server" OnClick="btnAgregar_Click" Text="Agregar" />
                            </td>
                            <td>
                                <asp:Button ID="btnQuitar" CssClass="buttonMenu alineacion" Width="70px" runat="server" OnClick="btnQuitar_Click" Text="Quitar" />
                            </td>
                            <td>
                                <asp:Button ID="btnQuitarTodo" CssClass="buttonMenu" Width="70px" runat="server" OnClick="btnQuitarTodos_Click" Text="Quitar Todo" />
                            </td>
                            <td>
                                <asp:Button ID="btnAplicar" CssClass="buttonMenu" Width="70px" runat="server" OnClick="btnAplicar_Click" Text="Aplicar" />
                            </td>
                            <td>
                                <table style="width: 300px; position: relative; left: 40px;">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="radAnd" runat="server" Text="Todas las condic." Checked="true" />
                                        </td>

                                        <td>
                                            <asp:RadioButton ID="radOr" runat="server" Text="Cualquier cond." />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td></td>
                        </tr>
                    </table>

                </asp:Panel>
            </fieldset>
        </td>
    </tr>
</table>

