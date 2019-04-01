<%@ Page Title="Administracion Personas" Language="C#" MasterPageFile="~/MasterPages/rrhh.master" AutoEventWireup="true" CodeBehind="frmPersonal.aspx.cs" Inherits="SGECA.Forms.Empleados.frmPersonas" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/MasterPages/rrhh.master" %>
<%@ Register Src="~/Controles/Paginador.ascx" TagName="Paginador" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <script type="text/javascript">
        $("#mTitle").text("PERSONAL");

        //funciones jQuery para fecha de alta
        function pageLoad(sender, args) //Permite mostrar el JQueryUI Picker despues del postback
        {
            //funciones jQuery para fecha de alta
            $(document).ready(function () {
                $("#<%=txtFechaAlta.ID %>").datepicker({
                    showOn: 'focus',
                    dateFormat: "dd/mm/yy",
                    buttonImageOnly: true,
                    buttonImage: '/Content/themes/base/images/calendar.png',
                    dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                    monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                    nextText: "Siguiente",
                    prevText: "Anterior"
                });

                //funciones jQuery para fecha de baja
                $("#<%=txtFechaBaja.ID %>").datepicker({
                    showOn: 'focus',
                    dateFormat: "dd/mm/yy",
                    buttonImageOnly: true,
                    buttonImage: '/Content/themes/base/images/calendar.png',
                    dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                    monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                    nextText: "Siguiente",
                    prevText: "Anterior"
                });

                //funciones jQuery para fecha de nacimiento
                $("#<%=txtFechaNacimiento.ID %>").datepicker({
                    showOn: 'focus',
                    dateFormat: "dd/mm/yy",
                    buttonImageOnly: true,
                    buttonImage: '/Content/themes/base/images/calendar.png',
                    dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                    monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                    nextText: "Siguiente",
                    prevText: "Anterior"
                });

                //funciones jQuery para fecha de cierre
                $("#<%=txtFechaCierre.ID %>").datepicker({
                    showOn: 'focus',
                    dateFormat: "dd/mm/yy",
                    buttonImageOnly: true,
                    buttonImage: '/Content/themes/base/images/calendar.png',
                    dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                    monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                    nextText: "Siguiente",
                    prevText: "Anterior"
                });

                //Solo numericos
                $('#<%=txtCodigo.ID %>').keydown(function (e) {
                    if (e.shiftKey || e.ctrlKey || e.altKey) {
                        e.preventDefault();
                    } else {
                        var key = e.keyCode;
                        if (!((key == 8) || (key == 46) || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
                            e.preventDefault();
                        }
                    }
                });

                //Solo numericos
                $('#<%=txtHijos.ID %>').keydown(function (e) {
                    if (e.shiftKey || e.ctrlKey || e.altKey) {
                        e.preventDefault();
                    } else {
                        var key = e.keyCode;
                        if (!((key == 8) || (key == 46) || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
                            e.preventDefault();
                        }
                    }
                });
            });
                    
        }

        function Confirmar() {
            if ($("#<%=txtCodigo.ID%>").val() != "")
            {
                if (confirm("¿Esta seguro que desea eliminar este registro?")) {
                    return true;
                } else {
                    return false;
                }
            }
            else
            {
                $.jGrowl("Debe seleccionar un registro para poder borrarlo!", { theme: 'default', position: 'center', life: 3000 });
                return false;
            }
        }
        </script>
        <asp:ScriptManager ID="ScriptManager3" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdEmpleados" />
                <asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                <asp:AsyncPostBackTrigger ControlID="btnNuevo" />
                <asp:AsyncPostBackTrigger ControlID="btnEliminar" />
                <asp:AsyncPostBackTrigger ControlID="txtGrupo" />
                <asp:AsyncPostBackTrigger ControlID="txtCUIL" />
            </Triggers>
            <ContentTemplate>
                <div class="botonera2">
                    <asp:Button ID="btnLimpiar" runat="server" CssClass="buttonMenu" OnClick="btnLimpiar_Click" Text="LIMPIAR" />
                    <asp:Button ID="btnNuevo" runat="server" CssClass="buttonMenu" OnClick="btnNuevo_Click" Text="AGREGAR" />
                    <asp:Button ID="btnGuardar" runat="server" CssClass="buttonMenu" OnClick="btnGuardar_Click" Text="GRABAR" />
                    <asp:Button ID="btnEliminar" runat="server" CssClass="buttonMenu" OnClick="btnEliminar_Click" OnClientClick="return Confirmar()" Text="ELIMINAR" />
                    <asp:Button ID="btnVer" runat="server" CssClass="buttonMenu" OnClick="btnVer_Click" Text="LISTAR" />
                </div>
                <br />
                <input style="display:none" type="text" name="fakeusernameremembered"/>
                <input style="display:none" type="password" name="fakepasswordremembered"/>
                <div class="main_contenido">
                    <table class="TablaDetalles">
                <tr>
                    <td><asp:Label ID="lblCodigo" runat="server" CssClass="lblField">Legajo :</asp:Label></td>
                    <td><asp:TextBox ID="txtCodigo" MaxLength="6" Width="60" ClientIDMode="Static" CssClass="input1" runat="server"></asp:TextBox></td>
                    <td><asp:Label ID="lblFechaCierre" runat="server" CssClass="lblField">Fecha de cierre :</asp:Label></td>
                    <td><asp:TextBox ID="txtFechaCierre" MaxLength="6" Width="100" ClientIDMode="Static" CssClass="input1" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label ID="lblApellido" CssClass="lblField" runat="server" Height="16px">Apellido :</asp:Label></td>
                    <td><asp:TextBox ID="txtApellido" MaxLength="25" CssClass="input1" runat="server"></asp:TextBox></td>
                    <td><asp:Label ID="lblNombre" CssClass="lblField" runat="server">Nombre :</asp:Label></td>
                    <td><asp:TextBox ID="txtNombre" MaxLength="25" CssClass="input1" runat="server"></asp:TextBox></td>
                    <td><asp:Label ID="lblCUIL" CssClass="lblField" runat="server">C.U.I.L. :</asp:Label></td>
                    <td><asp:TextBox ID="txtCUIL" MaxLength="50" CssClass="input1" runat="server" AutoPostBack="true" OnTextChanged="txtCUIL_TextChanged"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label6" CssClass="lblField" runat="server">Estado Civil :</asp:Label></td>
                    <td><asp:DropDownList ID="txtEstadoCivil" MaxLength="50" CssClass="input1" runat="server"></asp:DropDownList></td>
                    <td><asp:Label ID="Label5" CssClass="lblField" runat="server">Hijos :</asp:Label></td>
                    <td><asp:TextBox ID="txtHijos" MaxLength="2" Width="20" ClientIDMode="Static" CssClass="input1" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label3" CssClass="lblField" runat="server">Domicilio :</asp:Label></td>
                    <td><asp:TextBox ID="txtDomicilio" MaxLength="50" CssClass="input1" runat="server"></asp:TextBox></td>
                    <td><asp:Label ID="Label4" CssClass="lblField" runat="server">Provincia :</asp:Label></td>
                    <td><asp:DropDownList ID="txtProvincia" MaxLength="50" CssClass="input1" runat="server"></asp:DropDownList></td>
                    
                </tr>
                <tr>
                    <td><asp:Label ID="lblTelLaboral" CssClass="lblField" runat="server">Tel. Laboral :</asp:Label></td>
                    <td><asp:TextBox ID="txtTelLaboral" MaxLength="50" CssClass="input1" runat="server"></asp:TextBox></td>
                    <td><asp:Label ID="lblTelPersonal" CssClass="lblField" runat="server">Tel. Personal :</asp:Label></td>
                    <td><asp:TextBox ID="txtTelPersonal" MaxLength="50" CssClass="input1" runat="server"></asp:TextBox></td>
                    <td><asp:Label ID="lblMail" CssClass="lblField" runat="server">Mail :</asp:Label></td>
                    <td><asp:TextBox ID="txtMail" MaxLength="50" CssClass="input1" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label8" CssClass="lblField" runat="server">Tel. Alternativo :</asp:Label></td>
                    <td><asp:TextBox ID="txtTelAlternativo" MaxLength="50" CssClass="input1" runat="server"></asp:TextBox></td>
                    <td><asp:Label ID="Label9" CssClass="lblField" runat="server">Persona de contacto :</asp:Label></td>
                    <td><asp:TextBox ID="txtContacto" MaxLength="50" CssClass="input1" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label2" CssClass="lblField" runat="server">Fecha de Nacimiento :</asp:Label></td>
                    <td><asp:TextBox ID="txtFechaNacimiento" ClientIDMode="Static" CssClass="input1" Width="100" runat="server"></asp:TextBox></td> <!-- Se cambia ClientIDMode a static para poder usar jQueryUI DatePicker-->
                    <td><asp:Label ID="lblFechaAlta" CssClass="lblField" runat="server">F. Alta :</asp:Label></td>
                    <td><asp:TextBox ID="txtFechaAlta" ClientIDMode="Static" CssClass="input1" Width="100" runat="server"></asp:TextBox></td> <!-- Se cambia ClientIDMode a static para poder usar jQueryUI DatePicker-->
                    <td><asp:Label ID="lblFechaBaja" CssClass="lblField" runat="server" >F. Baja :</asp:Label></td>
                    <td><asp:TextBox ID="txtFechaBaja" ClientIDMode="Static" CssClass="input1" Width="100" runat="server" AutoCompleteType="None"></asp:TextBox></td> <!-- Se cambia ClientIDMode a static para poder usar jQueryUI DatePicker-->
                </tr>
                <tr>
                    <td><asp:Label ID="lblGrupo" CssClass="lblField" runat="server">Grupo :</asp:Label></td>
                    <td><asp:DropDownList ID="txtGrupo" CssClass="input1" runat="server" /></td>
                    <td><asp:Label ID="lblPassword" CssClass="lblField" runat="server">Password :</asp:Label></td>
                    <td><asp:TextBox ID="txtPassword" TextMode="Password" MaxLength="20" CssClass="input1" runat="server" AutoCompleteType="None"></asp:TextBox></td>
                    <td><asp:Label ID="Label7" CssClass="lblField" runat="server">Tarea :</asp:Label></td>
                    <td><asp:TextBox ID="txtTarea" MaxLength="20" CssClass="input1" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label1" CssClass="lblField" runat="server">Percibe adelantos :</asp:Label></td>
                    <td><asp:CheckBox ID="chPercibeAdelantos" runat="server" /></td>
                </tr>
                </table>
                </div>
                
            </ContentTemplate>
        </asp:UpdatePanel>
            

            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                     <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtBuscar" />
                        <asp:AsyncPostBackTrigger ControlID="pagPaginador"  />
                        <asp:AsyncPostBackTrigger ControlID="btnVer" />
                        <asp:AsyncPostBackTrigger ControlID="btnSeleccionarClose" />
                        <asp:AsyncPostBackTrigger ControlID="grdEmpleados" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Panel ID="frmSeleccionar" runat="server" CssClass="modal_form">
                        <div class="modal_title">PERSONAL<asp:Button ID="btnSeleccionarClose" runat="server" Text="X" CssClass="buttonClose" OnClick="btnSeleccionarClose_Click"/></div>
                        <div class="main_contenido">
                            <div style="text-align: left">
                            <asp:Label CssClass="lblField" runat="server">Buscar</asp:Label>
                            <asp:TextBox ID="txtBuscar" runat="server" OnTextChanged="txtBuscar_TextChanged" CssClass="input1" AutoPostBack="True"></asp:TextBox>
                        </div>
                            <div class="CSSTableGenerator">
                            <asp:GridView ID="grdEmpleados" DataKeyNames="Id" CssClass="columna" runat="server"
                            AutoGenerateColumns="False" Width="100%"
                            OnSorted="grdEmpleados_Sorted"
                            OnSorting="grdEmpleados_Sorting"
                            OnSelectedIndexChanged="grdEmpleados_SelectedIndexChanged" ViewStateMode="Enabled" OnRowDataBound="grdEmpleados_RowDataBound" EnablePersistedSelection="True" AllowSorting="True">
                            <Columns>
                                <asp:BoundField HeaderText="ID" DataField="Id" Visible="True" ItemStyle-Width="7%" SortExpression="Id" >
                                <ItemStyle Width="2%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="NOMBRE" DataField="Nombre" ItemStyle-Width="10%" SortExpression="Nombre" >
                                <ItemStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="APELLIDO" DataField="Apellido" ItemStyle-Width="10%" SortExpression="apellido" >
                                <ItemStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="MAIL" DataField="Mail" ItemStyle-Width="10%" SortExpression="Mail" >
                                <ItemStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="GRUPO" DataField="Grupo" ItemStyle-Width="10%" SortExpression="Grupo" >
                                <ItemStyle Width="10%" />
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

