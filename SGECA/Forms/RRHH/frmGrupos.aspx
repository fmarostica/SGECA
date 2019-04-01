<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/rrhh.master" AutoEventWireup="true" CodeBehind="frmGrupos.aspx.cs" Inherits="SGECA.Forms.RRHH.frmGrupos" EnableEventValidation="false" %>
<%@ Register Src="~/Controles/Paginador.ascx" TagName="Paginador" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <script>
        $("#mTitle").text("GRUPOS");

        function Confirmar() {
            if ($("#<%=txtCodigo.ID%>").val() != "") {
                if (confirm("¿Esta seguro que desea eliminar este registro?")) {
                    return true;
                } else {
                    return false;
                }
            }
            else {
                $.jGrowl("Debe seleccionar un registro para poder borrarlo!", { theme: 'default', position: 'center', life: 3000 });
                return false;
            }
        }
    </script>
    <asp:ScriptManager ID="ScriptManager3" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdGrupos" />
                <asp:AsyncPostBackTrigger ControlID="txtEmpleado" />
            </Triggers>
            <ContentTemplate>
                <div class="botonera2">
                    
                <asp:Button ID="btnLimpiar" CssClass="buttonMenu" Text="LIMPIAR" runat="server" OnClick="btnLimpiar_Click" />
                <asp:Button ID="btnNuevo" CssClass="buttonMenu" Text="AGREGAR" runat="server" OnClick="btnNuevo_Click" />
                <asp:Button ID="btnGrabar" CssClass="buttonMenu" Text="GRABAR" runat="server" OnClick="btnGrabar_Click" />
                <asp:Button ID="btnEliminar" CssClass="buttonMenu" Text="ELIMINAR" runat="server" OnClientClick="return Confirmar()" OnClick="btnEliminar_Click" />
                <asp:Button ID="btnVer" CssClass="buttonMenu" Text="LISTAR" runat="server" OnClick="btnVer_Click" />
                </div>
                <br />
                <div class="main_contenido">
                    <asp:Label CssClass="lblField" ID="lblCodigo" Text="Código : " runat="server"></asp:Label>
                    <asp:TextBox ID="txtCodigo" CssClass="input1" Width="50px" runat="server"></asp:TextBox>
                    <asp:Label ID="lblDescripcion" CssClass="lblField" Text="Lider : " runat="server"></asp:Label>
                    <asp:DropDownList ID="txtEmpleado" CssClass="input1" ClientIDMode="Static" Width="300" runat="server" AutoPostBack="true" OnSelectedIndexChanged="txtEmpleado_SelectedIndexChanged"></asp:DropDownList>
                    <asp:Label ID="Label1" CssClass="lblField" Text="Nombre del grupo : " runat="server"></asp:Label>
                    <asp:TextBox ID="txtDescripcion" CssClass="input1" Width="200px" runat="server"></asp:TextBox>
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
            </Triggers>
            <ContentTemplate>
                <asp:Panel ID="frmSeleccionar" runat="server" CssClass="modal_form">
                        <div class="modal_title">Grupos<asp:Button ID="btnSeleccionarClose" runat="server" Text="X" CssClass="buttonClose" OnClick="btnSeleccionarClose_Click"/></div>
                <div class="main_contenido">
                    <div style="text-align: left">
                    <asp:Label CssClass="lblField" runat="server">Buscar : </asp:Label>
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="input1" OnTextChanged="txtBuscar_TextChanged" AutoPostBack="True"></asp:TextBox>
                </div>
                <div class="CSSTableGenerator">
                    <asp:GridView ID="grdGrupos" DataKeyNames="Id" CssClass="columna" runat="server"
                        AutoGenerateColumns="False" Width="100%" ViewStateMode="Enabled" OnRowDataBound="grdGrupos_RowDataBound" OnSelectedIndexChanged="grdGrupos_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField HeaderText="ID" DataField="Id" Visible="True" ItemStyle-Width="5%" SortExpression="Id" >
                        </asp:BoundField>
                        <asp:BoundField HeaderText="NOMBRE" DataField="Nombre" ItemStyle-Width="20%" SortExpression="Nombre" >
                        </asp:BoundField>
                        
                        </Columns>
                        </asp:GridView>
                </div>
                <br />
                <uc2:Paginador ID="pagPaginador" runat="server" OnAnterior="pagPaginador_Anterior"
                            OnFin="pagPaginador_Fin" OnInicio="pagPaginador_Inicio" OnProxima="pagPaginador_Proxima"
                            OnPaginaSeleccionada="pagPaginador_PaginaSeleccionada" ViewStateMode="Enabled"   />
                </div>
             </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
    
</asp:Content>
