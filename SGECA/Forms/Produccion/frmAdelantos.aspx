<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Produccion.master" AutoEventWireup="true" CodeBehind="frmAdelantos.aspx.cs" Inherits="SGECA.Forms.Empleados.frmViaticos" EnableEventValidation="false" %>
<%@ Register Src="~/Controles/Paginador.ascx" TagName="Paginador" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <script>
        $("#mTitle").text("ADELANTOS");

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

        function pageLoad(sender, args) //Permite mostrar el JQueryUI Picker despues del postback
        {
            $(document).ready(function () {

                $("#<%=txtFecha.ID %>").datepicker({
                    showOn: 'focus',
                    dateFormat: "dd/mm/yy",
                    buttonImageOnly: true,
                    buttonImage: '/Content/themes/base/images/calendar.png',
                    dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                    monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                    nextText: "Siguiente",
                    prevText: "Anterior"
                });

                $("#<%=txtDesde.ID %>").datepicker({
                    showOn: 'focus',
                    dateFormat: "dd/mm/yy",
                    buttonImageOnly: true,
                    buttonImage: '/Content/themes/base/images/calendar.png',
                    dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                    monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                    nextText: "Siguiente",
                    prevText: "Anterior"
                });

                $("#<%=txtHasta.ID %>").datepicker({
                    showOn: 'focus',
                    dateFormat: "dd/mm/yy",
                    buttonImageOnly: true,
                    buttonImage: '/Content/themes/base/images/calendar.png',
                    dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                    monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                    nextText: "Siguiente",
                    prevText: "Anterior"
                });
            });
        }
    </script>
    <asp:ScriptManager ID="ScriptManager3" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>

                <div class="botonera2">
                <asp:Button ID="btnLimpiar" CssClass="buttonMenu" Text="LIMPIAR" runat="server" OnClick="btnLimpiar_Click" UseSubmitBehavior="False" />
                <asp:Button ID="btnNuevo" CssClass="buttonMenu" Text="AGREGAR" runat="server" OnClick="btnNuevo_Click" UseSubmitBehavior="False" />
                <asp:Button ID="btnGrabar" CssClass="buttonMenu" Text="GRABAR" runat="server" OnClick="btnGrabar_Click" UseSubmitBehavior="False" />
                <asp:Button ID="btnEliminar" CssClass="buttonMenu" Text="ELIMINAR" runat="server" OnClick="btnEliminar_Click" OnClientClick="Confirm()" UseSubmitBehavior="False" />
                    <asp:Button ID="btnVer" CssClass="buttonMenu" Text="LISTAR" runat="server" OnClick="btnVer_Click" UseSubmitBehavior="False" />
                </div>
                <br />
                <div class="main_contenido">
                    <asp:Label CssClass="lblField_fix_size" ID="lblFecha" Text="Fecha : " runat="server"></asp:Label><asp:TextBox ID="txtFecha" CssClass="input1" ClientIDMode="Static" Width="100px" runat="server"></asp:TextBox>
                    <br /><asp:Label CssClass="lblField_fix_size" ID="Label1" Text="Empleado : " runat="server"></asp:Label><asp:DropDownList ID="txtEmpleado" CssClass="input1" ClientIDMode="Static" Width="230px" runat="server"></asp:DropDownList>
                    <br /><asp:Label CssClass="lblField_fix_size" ID="lblImporte" Text="Importe : " runat="server"></asp:Label><asp:TextBox ID="txtImporte" CssClass="input1" Width="50px" runat="server"></asp:TextBox>
                    <br /><asp:Label CssClass="lblField_fix_size" ID="lblDescripcion" Text="Detalles : " runat="server"></asp:Label><asp:TextBox ID="txtDescripcion" CssClass="input1" Width="200px" runat="server"></asp:TextBox>
                    <br /><asp:Label CssClass="lblField_fix_size" ID="Label2" Text="Asignado por : " runat="server"></asp:Label><asp:DropDownList ID="txtAsignadoPor" CssClass="input1" Width="230px" runat="server"></asp:DropDownList>
                </div>
                
            
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdAdelantos" />
                <asp:AsyncPostBackTrigger ControlID="txtBuscar" />
                <asp:AsyncPostBackTrigger ControlID="btnNuevo" />
                <asp:AsyncPostBackTrigger ControlID="btnGrabar" />
                <asp:AsyncPostBackTrigger ControlID="btnEliminar" />
                <asp:AsyncPostBackTrigger ControlID="btnVer" />
            </Triggers>
            <ContentTemplate>
                <asp:Panel ID="frmSeleccionar" runat="server" CssClass="modal_form">
                    <div class="modal_title">ADELANTOS<asp:Button ID="btnSeleccionarClose" runat="server" Text="X" CssClass="buttonClose" OnClick="btnSeleccionarClose_Click2"/></div>
                        <div class="main_contenido">
                            <asp:Label Text="Buscar : " CssClass="lblField" runat="server" />
                            <asp:TextBox runat="server" CssClass="input1" ID="txtBuscar" AutoPostBack="true" OnTextChanged="txtBuscar_TextChanged" /> 
                            <asp:Label ID="lblDesde" CssClass="lblField" Text="Desde : " runat="server" />
                            <asp:TextBox ID="txtDesde" runat="server" CssClass="input1" Width="100" ClientIDMode="Static" />
                            <asp:Label ID="lblHasta" CssClass="lblField" Text="Hasta : " runat="server" />
                            <asp:TextBox ID="txtHasta" runat="server" CssClass="input1" Width="100" ClientIDMode="Static" />
                            <asp:Button ID="btnFiltrar" Text="Filtrar" runat="server" CssClass="buttonModal" Width="100" OnClick="btnFiltrar_Click" />
                            <div class="CSSTableGenerator">
                            <asp:GridView ID="grdAdelantos" DataKeyNames="adelanto_id" CssClass="columna" runat="server"
                                AutoGenerateColumns="False" Width="100%" ViewStateMode="Enabled" OnRowDataBound="grdAdelantos_RowDataBound" OnSelectedIndexChanged="grdAdelantos_SelectedIndexChanged" AllowSorting="True" OnSorting="grdAdelantos_Sorting">
                            <Columns>
                                <asp:BoundField HeaderText="ID" DataField="adelanto_id" Visible="True" ItemStyle-Width="5%" SortExpression="adelanto_id" >
                                </asp:BoundField>
                                <asp:BoundField HeaderText="FECHA" DataField="fecha" ItemStyle-Width="5%" SortExpression="fecha" >
                                </asp:BoundField>
                                <asp:BoundField HeaderText="EMPLEADO" DataField="empleado" ItemStyle-Width="20%" SortExpression="empleado" >
                                </asp:BoundField>
                                <asp:BoundField HeaderText="IMPORTE" DataField="importe" ItemStyle-Width="10%" SortExpression="importe" >
                                </asp:BoundField>
                                <asp:BoundField HeaderText="DETALLES" DataField="descripcion" ItemStyle-Width="40%" SortExpression="detalles" >
                                </asp:BoundField>
                                <asp:BoundField HeaderText="ASIGNADO POR" DataField="asignado_por_empleado" ItemStyle-Width="40%" SortExpression="detalles" >
                                </asp:BoundField>
                            </Columns>
                            </asp:GridView>
                    </div>
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
