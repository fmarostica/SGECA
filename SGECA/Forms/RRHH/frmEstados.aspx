<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/rrhh.master" AutoEventWireup="true" CodeBehind="frmEstados.aspx.cs" Inherits="SGECA.Forms.Empleados.frmEstados" EnableEventValidation="false" %>
<%@ Register Src="~/Controles/Paginador.ascx" TagName="Paginador" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <script>
        $("#mTitle").text("TAREAS");

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
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="botonera2">
                        <asp:Button ID="btnLimpiar" runat="server" CssClass="buttonMenu" OnClick="btnLimpiar_Click" Text="LIMPIAR"/>
                        <asp:Button ID="btnNuevo" runat="server" CssClass="buttonMenu" OnClick="btnNuevo_Click" Text="AGREGAR"/>
                        <asp:Button ID="btnGuardar" runat="server" CssClass="buttonMenu" OnClick="btnGuardar_Click" Text="GRABAR"/>
                        <asp:Button ID="btnEliminar" runat="server" CssClass="buttonMenu" OnClick="btnEliminar_Click" OnClientClick="return Confirmar()" Text="ELIMINAR"/>
                        <asp:Button ID="btnVer" runat="server" CssClass="buttonMenu" OnClick="btnVer_Click" Text="LISTAR"/>
                </div>
                <br />
                <div class="main_contenido">

                <asp:Label ID="lblCodigo" runat="server" CssClass="lblField">Código :</asp:Label><asp:TextBox ID="txtCodigo" MaxLength="6" CssClass="input1" Width="60" runat="server"></asp:TextBox>
                <asp:Label ID="lblNombre" CssClass="lblField" runat="server">Nombre :</asp:Label><asp:TextBox ID="txtNombre" MaxLength="50" Width="350" CssClass="input1" runat="server"></asp:TextBox>
                <asp:Label CssClass="lblField" runat="server">Tipo: </asp:Label>
                        <asp:DropDownList ID="txtTipo" CssClass="input1" runat="server">
                        <asp:ListItem>Trabajado</asp:ListItem>
                        <asp:ListItem>No Trabajado</asp:ListItem>
                        </asp:DropDownList>
                    
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
            

            <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                     <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtBuscar" />
                        <asp:AsyncPostBackTrigger ControlID="pagPaginador"  />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnNuevo" />
                        <asp:AsyncPostBackTrigger ControlID="grdEstados" />
                        <asp:AsyncPostBackTrigger ControlID="txtFiltroTipo" />
                         <asp:AsyncPostBackTrigger ControlID="btnEliminar" />
                         <asp:AsyncPostBackTrigger ControlID="btnVer" />
                         <asp:AsyncPostBackTrigger ControlID="btnSeleccionarClose" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Panel ID="frmSeleccionar" runat="server" CssClass="modal_form">
                        <div class="modal_title">TAREAS<asp:Button ID="btnSeleccionarClose" runat="server" Text="X" CssClass="buttonClose" OnClick="btnSeleccionarClose_Click"/></div>
                            <div class="main_contenido">
                            <asp:Label CssClass="lblField" runat="server">Buscar : </asp:Label>
                            <asp:TextBox ID="txtBuscar" runat="server" CssClass="input1" OnTextChanged="txtBuscar_TextChanged" AutoPostBack="True"></asp:TextBox>
                            <asp:Label CssClass="lblField" runat="server">Tipo : </asp:Label>
                            <asp:DropDownList ID="txtFiltroTipo" runat="server" CssClass="input1" OnTextChanged="txtBuscar_TextChanged" AutoPostBack="True"></asp:DropDownList>
                            
                            <div class="CSSTableGenerator">
                            <asp:GridView ID="grdEstados" DataKeyNames="Codigo" CssClass="columna" runat="server"
                            AutoGenerateColumns="False" Width="100%"
                            OnSelectedIndexChanged="grdEstados_SelectedIndexChanged" ViewStateMode="Enabled" OnRowDataBound="grdEstados_RowDataBound" AllowSorting="True" OnSorting="grdEstados_Sorting">
                            <Columns>
                                <asp:BoundField HeaderText="CODIGO" DataField="Codigo" Visible="True" SortExpression="tarea_estado_codigo" >
                                <ItemStyle Width="5%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="NOMBRE" DataField="Nombre" SortExpression="tarea_estado_nombre" >
                                <ItemStyle Width="15%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="TIPO" DataField="Tipo" Visible="True" SortExpression="tarea_estado_tipo" >
                                <ItemStyle Width="75%" />
                                </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                            </div>
                            <uc2:Paginador ID="pagPaginador" runat="server" OnAnterior="pagPaginador_Anterior"
                            OnFin="pagPaginador_Fin" OnInicio="pagPaginador_Inicio" OnProxima="pagPaginador_Proxima"
                            OnPaginaSeleccionada="pagPaginador_PaginaSeleccionada" ViewStateMode="Enabled"   />
                        </asp:Panel>
                        </div>
                        
                    </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>
