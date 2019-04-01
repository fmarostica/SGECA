<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Gestion.master" AutoEventWireup="true" CodeBehind="frmGastos.aspx.cs" Inherits="SGECA.Forms.Gestion.frmGastos" EnableEventValidation="false" %>
<%@ Register Src="~/Controles/Paginador.ascx" TagName="Paginador" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <script>
        $("#mTitle").text("TIPOS DE GASTOS");

        var confirm_value = document.createElement("INPUT");
        confirm_value.type = "hidden";
        confirm_value.name = "confirm_value";

        function Confirm() {
            if (confirm("¿Esta seguro que desea eliminar el registro?")) {
                confirm_value.value = "Si";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>

    <asp:ScriptManager ID="ScriptManager3" runat="server"></asp:ScriptManager>

        <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdEstados" />
            </Triggers>
            <ContentTemplate>
                <div class="botonera2">
                    <asp:Button ID="btnLimpiar" CssClass="buttonMenu" Text="LIMPIAR" runat="server" OnClick="btnLimpiar_Click" />
                    <asp:Button ID="btnNuevo" CssClass="buttonMenu" Text="AGREGAR" runat="server" OnClick="btnNuevo_Click" />
                    <asp:Button ID="btnGrabar" CssClass="buttonMenu" Text="GRABAR" runat="server" OnClick="btnGrabar_Click" />
                    <asp:Button ID="btnEliminar" CssClass="buttonMenu" Text="ELIMINAR" runat="server" OnClick="btnEliminar_Click" OnClientClick="Confirm()" />
                    <asp:Button ID="btnVer" CssClass="buttonMenu" Text="LISTAR" runat="server" OnClick="btnVer_Click" />
                </div>
                <br />
                <div class="main_contenido">
                    <table class="TablaDetalles">
                        <tr>
                            <td style="width: 120px;"><asp:Label ID="lblID" CssClass="lblField" Text="Gasto : " runat="server"></asp:Label></td>
                            <td><asp:TextBox ID="txtDetalles" ClientIDMode="Static" CssClass="input1" Width="180px" runat="server" ></asp:TextBox></td>
                        </tr>
                    </table>
                </div>
            
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnNuevo" />
                <asp:AsyncPostBackTrigger ControlID="btnGrabar" />
                <asp:AsyncPostBackTrigger ControlID="btnEliminar" />
                <asp:AsyncPostBackTrigger ControlID="txtBuscar" />
                <asp:AsyncPostBackTrigger ControlID="btnVer" />
                <asp:AsyncPostBackTrigger ControlID="grdEstados" />
            </Triggers>
            <ContentTemplate>
                <asp:Panel ID="frmSeleccionar" runat="server" CssClass="modal_form">
                <div class="modal_title">Gastos<asp:Button ID="btnSeleccionarClose" runat="server" Text="X" CssClass="buttonClose" OnClick="btnSeleccionarClose_Click"/></div>
                <div class="main_contenido">
                    <asp:Label CssClass="lblField" runat="server">Buscar : </asp:Label>
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="input1" OnTextChanged="txtBuscar_TextChanged" AutoPostBack="True"></asp:TextBox>
                    <asp:Label runat="server"></asp:Label>
                    <div class="CSSTableGenerator">
                            <asp:GridView ID="grdEstados" DataKeyNames="id" CssClass="columna" runat="server"
                            AutoGenerateColumns="False" Width="100%"
                            OnSorted="grdEstados_Sorted"
                            OnSorting="grdEstados_Sorting"
                            OnSelectedIndexChanged="grdEstados_SelectedIndexChanged" ViewStateMode="Enabled" OnRowDataBound="grdEstados_RowDataBound" EnablePersistedSelection="True" AllowSorting="True">
                            <Columns>
                                <asp:BoundField HeaderText="ID" DataField="id" Visible="True" ItemStyle-Width="7%" SortExpression="id" >
                                <ItemStyle Width="2%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="DETALLES" DataField="detalles" ItemStyle-Width="10%" SortExpression="detalles" >
                                <ItemStyle Width="10%" />
                                </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                <br />
                <uc2:Paginador ID="pagPaginador" runat="server" OnAnterior="pagPaginador_Anterior"
                            OnFin="pagPaginador_Fin" OnInicio="pagPaginador_Inicio" OnProxima="pagPaginador_Proxima"
                            OnPaginaSeleccionada="pagPaginador_PaginaSeleccionada" ViewStateMode="Enabled"   />
                        <br />
                        
                </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>
