<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Produccion.master" AutoEventWireup="true" CodeBehind="frmAsignacionTareas.aspx.cs" Inherits="SGECA.Forms.Empleados.frmAsignacionTareas" EnableEventValidation="false" %>
<%@ Register Src="~/Controles/Paginador.ascx" TagName="Paginador" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:ScriptManager ID="ScriptManager3" runat="server"></asp:ScriptManager>
    <script type="text/javascript">
        function pageLoad(sender, args) //Permite mostrar el JQueryUI Picker despues del postback
        {
            $(document).ready(function () {
                $("#<%=txtFechaInicio.ID %>").datepicker({
                    showOn: 'focus',
                    dateFormat: "dd/mm/yy",
                    buttonImageOnly: true,
                    buttonImage: '/Content/themes/base/images/calendar.png',
                    dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                    monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                    nextText: "Siguiente",
                    prevText: "Anterior"
                });

                $("#<%=txtBuscarDesde.ID %>").datepicker({
                    showOn: 'focus',
                    dateFormat: "dd/mm/yy",
                    buttonImageOnly: true,
                    buttonImage: '/Content/themes/base/images/calendar.png',
                    dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                    monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                    nextText: "Siguiente",
                    prevText: "Anterior"
                });

                $("#<%=txtBuscarHasta.ID %>").datepicker({
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
    

            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnTodos" />
                        <asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
                        <asp:AsyncPostBackTrigger ControlID="btnEmpleadosDialogoAsignar" />
                        <asp:AsyncPostBackTrigger ControlID="gvSitios" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="botonera2">
                            <asp:Button ID="btnLimpiar" Text="LIMPIAR" CssClass="buttonMenu" runat="server" OnClick="btnLimpiar_Click" OnClientClick="unselect_all()" />
                            <asp:Button ID="btnTodos" Text="TODOS" CssClass="buttonMenu" OnClientClick="select_all()" runat="server" OnClick="btnTodos_Click" />
                            <asp:Button ID="btnQuitar" Text="QUITAR" CssClass="buttonMenu" runat="server" OnClick="btnQuitar_Click" />
                            <asp:Button ID="btnAsignar" Text="ASIGNAR" CssClass="buttonMenu" runat="server" OnClick="btnAsignar_Click" />
                            <asp:Button ID="btnListar" Text="LISTAR" CssClass="buttonMenu" runat="server" OnClick="btnListar_Click" />
                        </div>
                        <br />
                        <div class="main_contenido">
                            <asp:Label runat="server" ID="lblBuscarEmpleado" CssClass="lblField" Text="Buscar : "></asp:Label>
                            <asp:TextBox runat ="server" ID="txtBuscarEmpleado" CssClass="input1" AutoPostBack="true" OnTextChanged="txtBuscarEmpleado_TextChanged"></asp:TextBox>
                            <asp:Label runat="server" CssClass="lblField" ID="lblEmpleadoGrupo" Text="Grupo: "></asp:Label>
                            <asp:DropDownList runat="server" ID="txtEmpleadoGrupo" CssClass="input1" OnSelectedIndexChanged="txtEmpleadoGrupo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <table class="TablaDetalles">
                <tr>
                    <td><asp:RadioButton ID="radioTrabajo" CssClass="lblField" GroupName="Trabajado" Checked="true" Text="Trabajo" runat="server" OnCheckedChanged="radioTrabajo_CheckedChanged" AutoPostBack="True" /></td>
                    <td><asp:RadioButton ID="radioNoTrabajo" CssClass="lblField" GroupName="Trabajado" Text="No Trabajo" runat="server" OnCheckedChanged="radioNoTrabajo_CheckedChanged" AutoPostBack="True" /></td>
                </tr>
                    <tr>
                        <td><asp:Label Text="Tarea :" CssClass="lblField" runat="server"></asp:Label></td>
                        <td><asp:DropDownList ID="txtEstado" CssClass="input1" runat="server" Width="350"></asp:DropDownList></td>
                        <td><asp:Label Text="Inicio :" CssClass="lblField" runat="server"></asp:Label></td>
                        <td><asp:TextBox CssClass="input1" runat="server" ID="txtFechaInicio" ClientIDMode="Static" Width="100"></asp:TextBox></td>
                    </tr>
                <tr>
                    <td><asp:Label Text="Sitio :" CssClass="lblField" runat="server"></asp:Label></td>
                    <td>
                        <asp:TextBox runat="server" ID="txtSitio" CssClass="input1" AutoPostBack="true" OnTextChanged="txtSitio_TextChanged" Width="165"></asp:TextBox>
                        <asp:Label ID="lblSitio" CssClass="lblField" Width="250" Text="" runat="server"></asp:Label>
                    </td>
                    <td><asp:Label Text="Viatico :" CssClass="lblField" runat="server"></asp:Label></td>
                    <td>
                        <asp:DropDownList ID="txtViatico" runat="server" CssClass="input1" AutoPostBack="true" Width="250"></asp:DropDownList>
                    </td>
                    <td><asp:Label Text="Observaciones :" CssClass="lblField" runat="server"></asp:Label></td>
                    <td colspan="6"><asp:TextBox CssClass="input1" runat="server" ID="txtObservaciones"></asp:TextBox></td>
                </tr>
                    <tr>
                        <td>
                            <asp:Label CssClass="lblField" ID="Label1" Text="Asignado por : " runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="txtEmpleado" CssClass="input1" ClientIDMode="Static" Width="300" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
                        </div>
                        <div class="CSSTableGenerator" style="max-height: 50px; overflow-y: scroll;">
                            <asp:GridView ID="grdEmpleadosAsignados" DataKeyNames="id" CssClass="columna" runat="server"
                            AutoGenerateColumns="False" Width="100%" ViewStateMode="Enabled" AllowSorting="True" OnSorting="grdEmpleados_Sorting" ShowHeaderWhenEmpty="True">
                            <Columns>
                                <asp:TemplateField HeaderText="SELECCIONAR" ItemStyle-Width="6%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbox" ClientIDMode="Static" CssClass="cbox" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="ID" DataField="id" Visible="True" ItemStyle-Width="7%" SortExpression="id" >
                                </asp:BoundField>
                                <asp:BoundField HeaderText="GRUPO ID" DataField="Grupo" Visible="True" ItemStyle-Width="7%" SortExpression="id" >
                                </asp:BoundField>
                                <asp:BoundField HeaderText="APELLIDO Y NOMBRE" DataField="ApellidoyNombre" ItemStyle-Width="60%" SortExpression="nombre" >
                                </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
            </asp:UpdatePanel>

        <asp:UpdatePanel runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="radioTrabajo" />
                <asp:AsyncPostBackTrigger ControlID="radioNoTrabajo" />
                <asp:AsyncPostBackTrigger ControlID="btnAsignar" />
                <asp:AsyncPostBackTrigger ControlID="btnTodos" />
                <asp:AsyncPostBackTrigger ControlID="txtSitio" />
                <asp:AsyncPostBackTrigger ControlID="gvSitios" />
                <asp:AsyncPostBackTrigger ControlID="txtViatico" />
            </Triggers>
            <ContentTemplate>
                
            </ContentTemplate>
        </asp:UpdatePanel>
        
    <asp:UpdatePanel runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtBuscarEmpleado" />
            <asp:AsyncPostBackTrigger ControlID="txtEmpleadoGrupo" />
            <asp:AsyncPostBackTrigger ControlID="txtSitio" />
            <asp:AsyncPostBackTrigger ControlID="txtBuscarSitioDialogo" />
            <asp:AsyncPostBackTrigger ControlID="btnCerrarDialogoSitios" />
            <asp:AsyncPostBackTrigger ControlID="btnCerrarDialogoEmpleados" />
            <asp:AsyncPostBackTrigger ControlID="gvSitiosPaginador" />
            <asp:AsyncPostBackTrigger ControlID="gvSitios" />
            <asp:AsyncPostBackTrigger ControlID="btnEmpleadosDialogoTodos" />
            <asp:AsyncPostBackTrigger ControlID="btnEmpleadosDialogoLimpiar" />
            <asp:AsyncPostBackTrigger ControlID="btnEmpleadosDialogoAsignar" />
        </Triggers>
        <ContentTemplate>
            
            <asp:Panel ID="frmBuscarSitio" runat="server" CssClass="modal_form">
                <div class="modal_title">SELECCIONAR SITIO<asp:Button ID="btnCerrarDialogoSitios" runat="server" Text="X" CssClass="buttonClose" OnClick="btnCerrarDialogoSitios_Click"/></div>
                <div class="main_contenido">
                    <asp:Label runat="server" Text="Buscar : " CssClass="lblField"></asp:Label>
                    <asp:TextBox runat="server" ID="txtBuscarSitioDialogo" CssClass="input1" AutoPostBack="true" OnTextChanged="txtBuscarSitioDialogo_TextChanged"></asp:TextBox>
                
                <br />
                <div class="CSSTableGenerator">
                    <asp:GridView ID="gvSitios" DataKeyNames="Codigo" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvSitios_RowDataBound" 
                        OnSelectedIndexChanged="gvSitios_SelectedIndexChanged" Width="100%">
                        <Columns>
                                <asp:BoundField HeaderText="ID" DataField="Codigo" ItemStyle-Width="10%" SortExpression="codigo" >
                                </asp:BoundField>
                                <asp:BoundField HeaderText="NOMBRE" DataField="Nombre" ItemStyle-Width="60%" SortExpression="nombre" >
                                </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
                <uc2:Paginador ID="gvSitiosPaginador" runat="server" OnAnterior="gvSitiosPaginador_Anterior"
                            OnFin="gvSitiosPaginador_Fin" OnInicio="gvSitiosPaginador_Inicio" OnProxima="gvSitiosPaginador_Proxima"
                            OnPaginaSeleccionada="gvSitiosPaginador_PaginaSeleccionada" ViewStateMode="Enabled"   />
                    </div>
            </asp:Panel>

            <asp:Panel ID="frmBuscarEmpleado" runat="server" CssClass="modal_form">
                <div class="modal_title">SELECCIONAR PERSONAL<asp:Button ID="btnCerrarDialogoEmpleados" runat="server" Text="X" CssClass="buttonClose" OnClick="btnCerrarDialogoEmpleados_Click"/></div>
                <div class="main_contenido">
                    <asp:Label runat="server" Text="Buscar : " CssClass="lblField"></asp:Label>
                    <asp:TextBox runat="server" ID="TextBox1" CssClass="input1" AutoPostBack="true" OnTextChanged="txtBuscarSitioDialogo_TextChanged"></asp:TextBox>
                    <div class="CSSTableGenerator">
                            <asp:GridView ID="grdEmpleados" DataKeyNames="id" CssClass="columna" runat="server"
                            AutoGenerateColumns="False" Width="100%" ViewStateMode="Enabled" AllowSorting="True" OnSorting="grdEmpleados_Sorting">
                            <Columns>
                                <asp:TemplateField HeaderText="SELECCIONAR" ItemStyle-Width="6%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbox" ClientIDMode="Static" CssClass="cbox" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="ID" DataField="id" Visible="True" ItemStyle-Width="7%" SortExpression="id" >
                                </asp:BoundField>
                                <asp:BoundField HeaderText="GRUPO ID" DataField="Grupo" Visible="True" ItemStyle-Width="7%" SortExpression="id" >
                                </asp:BoundField>
                                <asp:BoundField HeaderText="APELLIDO y NOMBRE" DataField="ApellidoyNombre" ItemStyle-Width="60%" SortExpression="nombre" >
                                </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                <br />
                <div style="text-align: right;">
                    <asp:Button ID="btnEmpleadosDialogoLimpiar" runat="server" Text="Limpiar" Width="100" OnClick="btnEmpleadosDialogoLimpiar_Click" />
                    <asp:Button ID="btnEmpleadosDialogoTodos" runat="server" Text="Todos" Width="100" OnClick="btnEmpleadosDialogoTodos_Click" />
                    <asp:Button ID="btnEmpleadosDialogoAsignar" runat="server" Text="Asignar" Width="100" OnClick="btnEmpleadosDialogoAsignar_Click" />
                </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdTareas" />
                        <asp:AsyncPostBackTrigger ControlID="btnDesasignar" />
                        <asp:AsyncPostBackTrigger ControlID="btnAsignar" />
                        <asp:AsyncPostBackTrigger ControlID="btnTodas" />
                        <asp:AsyncPostBackTrigger ControlID="grdTareasPaginador" />
                        <asp:AsyncPostBackTrigger ControlID="btnLimpiarTareas" />
                        <asp:AsyncPostBackTrigger ControlID="txtBuscar" />
                        <asp:AsyncPostBackTrigger ControlID="btnListar"/>
                        <asp:AsyncPostBackTrigger ControlID="btnCloseFrmSeleccionarTarea"/>
                        <asp:AsyncPostBackTrigger ControlID="frmModificarViatico_Aceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnFiltar" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Panel ID="frmSeleccionarTarea" runat="server" CssClass="modal_form">
                        <div class="modal_title">TAREAS<asp:Button ID="btnCloseFrmSeleccionarTarea" runat="server" Text="X" CssClass="buttonClose" OnClick="btnCloseFrmSeleccionarTarea_Click"/></div>
                        <div class="main_contenido">

                        <asp:Label runat="server" ID="lblBuscarTarea" CssClass="lblField" Text="Empleado : "></asp:Label>
                        <asp:TextBox CssClass="input1" runat="server" ID="txtBuscar" OnTextChanged="txtBuscar_TextChanged" AutoPostBack="true"></asp:TextBox>
                        
                        <asp:Label runat="server" ID="Label4" CssClass="lblField" Text="Cell ID : "></asp:Label>
                        <asp:TextBox CssClass="input1" runat="server" ID="txtBuscarSitioID" OnTextChanged="txtBuscar_TextChanged" AutoPostBack="true"></asp:TextBox>
                        <asp:Label runat="server" ID="Label2" CssClass="lblField" Text="Desde : "></asp:Label>
                        <asp:TextBox CssClass="input1" runat="server" ID="txtBuscarDesde" Width="80" AutoPostBack="true" ClientIDMode="Static"></asp:TextBox>
                        <asp:Label runat="server" ID="Label3" CssClass="lblField" Text="Hasta : "></asp:Label>
                        <asp:TextBox CssClass="input1" runat="server" ID="txtBuscarHasta" Width="80" AutoPostBack="true" ClientIDMode="Static"></asp:TextBox>
                        
                        <asp:Button CssClass="buttonModal" Width="100px" Text="Filtrar" runat="server" ID="btnFiltar" OnClick="btnFiltar_Click" />
                        <asp:Button CssClass="buttonModal" Width="100px" Text="Limpiar" runat="server" ID="btnLimpiarTareas" OnClick="btnLimpiarTareas_Click" />
                        <asp:Button CssClass="buttonModal" Width="100px" Text="Todas" runat="server" ID="btnTodas" OnClick="btnTodas_Click" />
                        <asp:Button CssClass="buttonModal" Width="100px" Text="Desasignar" runat="server" ID="btnDesasignar" OnClick="btnDesasignar_Click" />

                        <div class="CSSTableGenerator">
                            <asp:GridView ID="grdTareas" DataKeyNames="TareaID" CssClass="columna" runat="server" AllowSorting="true"
                            AutoGenerateColumns="False" Width="100%" ViewStateMode="Enabled" EmptyDataText="No se encontraron registros" OnSorting="grdTareas_Sorting"
                                OnRowDataBound="grdTareas_RowDataBound" OnSelectedIndexChanged="grdTareas_SelectedIndexChanged">
                            <Columns>
                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbox" runat="server"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="TAREA ID" DataField="TareaID" SortExpression="tarea_id" Visible="false">
                                <ItemStyle CssClass="hidden"/>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="CELL ID" DataField="Sitio_id" SortExpression="Sitio_id" >
                                <ItemStyle Width="2%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="SITIO" DataField="Sitio" SortExpression="Sitio" >
                                <ItemStyle Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="TAREA" DataField="Estado" SortExpression="Estado">
                                <ItemStyle Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="FECHA" DataField="FechaInicio" Visible="True" SortExpression="fecha_inicio" >
                                <ItemStyle Width="7%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="APELLIDO Y NOMBRE" DataField="empleado" SortExpression="empleado" >
                                <ItemStyle Width="20%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="VIATICO" DataField="Viatico"  SortExpression="Viatico" >
                                <ItemStyle Width="7%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="ASIGNADO POR" DataField="Asignado_por"  SortExpression="Asignado_por" >
                                <ItemStyle Width="20%" />
                                </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <uc2:Paginador ID="grdTareasPaginador" runat="server" OnAnterior="grdTareasPaginador_Anterior"
                            OnFin="grdTareasPaginador_Fin" OnInicio="grdTareasPaginador_Inicio" OnProxima="grdTareasPaginador_Proxima"
                            OnPaginaSeleccionada="grdTareasPaginador_PaginaSeleccionada" ViewStateMode="Enabled"/>
                        <br />
                        
                        </div>
                            </asp:Panel>
                    </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdTareas" />
                        <asp:AsyncPostBackTrigger ControlID="btnDesasignar" />
                        <asp:AsyncPostBackTrigger ControlID="btnAsignar" />
                        <asp:AsyncPostBackTrigger ControlID="btnTodas" />
                        <asp:AsyncPostBackTrigger ControlID="grdTareasPaginador" />
                        <asp:AsyncPostBackTrigger ControlID="btnLimpiarTareas" />
                        <asp:AsyncPostBackTrigger ControlID="txtBuscar" />
                        <asp:AsyncPostBackTrigger ControlID="btnListar"/>
                        <asp:AsyncPostBackTrigger ControlID="btnCloseFrmSeleccionarTarea"/>
                    </Triggers>
                    <ContentTemplate>
                        <asp:Panel ID="frmModificarViatico" runat="server" CssClass="modal_form" Height="400px">
                        <div class="modal_title">MODIFICAR TAREA<asp:Button ID="frmModificarViatico_Cerrar" runat="server" Text="X" CssClass="buttonClose" OnClick="frmModificarViatico_Cerrar_Click"/></div>
                        <div class="main_contenido">
                        <asp:Button CssClass="buttonModal" Width="100px" Text="ACEPTAR" runat="server" ID="frmModificarViatico_Aceptar" OnClick="frmModificarViatico_Aceptar_Click" />
                            <br />
                            <asp:Label runat="server" Text="Empleado: " CssClass="lblField"></asp:Label>
                            <asp:Label runat="server" ID="lblEmpleadoModViatico" Text="" CssClass="label"></asp:Label>
                            <br />
                            <asp:Label runat="server" Text="Viatico: " CssClass="lblField"></asp:Label>
                            <asp:TextBox ID="txtModViatico_Importe" runat="server"></asp:TextBox>
                        </div>
                            </asp:Panel>
                    </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>
