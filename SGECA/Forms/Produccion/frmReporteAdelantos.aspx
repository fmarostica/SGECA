<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Produccion.master" AutoEventWireup="true" CodeBehind="frmReporteAdelantos.aspx.cs" Inherits="SGECA.Forms.Produccion.frmReporteAdelantos" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/Controles/Paginador.ascx" TagName="Paginador" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <script>
        $("#mTitle").text("REPORTE DE ADELANTOS");

        function pageLoad(sender, args) //Permite mostrar el JQueryUI Picker despues del postback
        {
            $(document).ready(function () {
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

                //funciones jQuery para fecha de alta
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

        function doOpen() {
            $find("cpe")._doOpen();
        }

        function doClose() {
            $find("cpe")._doClose();
        }
    </script>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="updateProgress" runat="server">
                        <ProgressTemplate>
                            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Imagenes/loading.gif" AlternateText="Loading ..." Width="200" Height="200" ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 20%; left: 40%;" />
                            </div>
                        </ProgressTemplate>
                        </asp:UpdateProgress>

                        <asp:Panel id="frmBuscarEmpleado" runat="server" CssClass="modal_form">
                            <div class="modal_title">Seleccion de Personal<asp:Button ID="btnSeleccionarClose" runat="server" Text="X" CssClass="buttonClose" OnClick="btnSeleccionarClose_Click"/></div>
                            <div class="main_contenido">
                                <asp:Button ID="btnEmpleadosDialogoSeleccionar" runat="server" Text="Agregar seleccionados" CssClass="buttonModal" OnClick="btnEmpleadosDialogoSeleccionar_Click" />
                                <br />
                                <div class="CSSTableGenerator" style="overflow-y: scroll;">
                                    <asp:GridView ID="grdEmpleados" DataKeyNames="id" CssClass="columna" runat="server"
                                    AutoGenerateColumns="False" Width="100%" ViewStateMode="Enabled">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Seleccionar" ItemStyle-Width="6%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbox" ClientIDMode="Static" CssClass="cbox" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="ID" DataField="id" Visible="True" ItemStyle-Width="7%" SortExpression="id" >
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Apellido y Nombre" DataField="ApellidoyNombre" ItemStyle-Width="70%" SortExpression="nombre" >
                                        </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </asp:Panel>

                        <asp:Button ID="btnTodos" Text="TODOS" CssClass="buttonMenu" runat="server" OnClick="btnTodos_Click" />
                        <asp:Button ID="btnLimpiar" Text="LIMPIAR" CssClass="buttonMenu" runat="server" OnClick="btnLimpiar_Click" />
                        <asp:Button ID="btnGenerar" Text="GENERAR" CssClass="buttonMenu" runat="server" OnClick="btnGenerar_Click" OnClientClick="doClose();" />
                        <asp:Button ID="btnExpandir" Text="OPCIONES" CssClass="buttonMenu" runat="server" />

                        <div class="main_contenido">

                            <asp:Panel ID="dropDown" runat="server">
                            </asp:Panel>

                            <ajaxToolkit:CollapsiblePanelExtender ID="cpe" runat="Server"
                            TargetControlID="panelMain"
                            BehaviorID="cpe"
                            ClientIDMode="Static"
                            CollapsedSize="0"
                            ExpandedSize="300"
                            Collapsed="True"
                            ExpandControlID="btnExpandir"
                            CollapseControlID="btnExpandir"
                            AutoCollapse="False"
                            AutoExpand="False"
                            ScrollContents="False"
                            TextLabelID="btnExpandir"
                            CollapsedText="OPCIONES"
                            ExpandedText="OPCIONES" 
                            ImageControlID="Image1"
                            ExpandDirection="Vertical" />

                            <asp:Panel ID="panelMain" runat="server">
                                <asp:Label runat="server" Text="Empleado" CssClass="lblField"></asp:Label>
                                <asp:TextBox ID="txtEmpleado" ClientIDMode="Static" CssClass="input1" runat="server" AutoPostBack="true" OnTextChanged="txtEmpleado_TextChanged"></asp:TextBox>
                                <asp:Label runat="server" Text="Grupo" CssClass="lblField"></asp:Label>
                                <asp:DropDownList ID="txtGrupo" ClientIDMode="Static" CssClass="input1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="txtGrupo_SelectedIndexChanged"></asp:DropDownList>
                                <asp:Label ID="lblDesde" Text="Desde : " CssClass="lblField" runat="server"></asp:Label>
                                <asp:TextBox ID="txtDesde" ClientIDMode="Static" CssClass="input1" runat="server" Width="80px"></asp:TextBox>
                                <asp:Label ID="lblHasta" Text="Hasta : " CssClass="lblField" runat="server"></asp:Label>
                                <asp:TextBox ID="txtHasta" ClientIDMode="Static" CssClass="input1" runat="server" Width="80px"></asp:TextBox>

                                <div class="CSSTableGenerator" style="min-height: 50px; height:200px; overflow-y: scroll;">
                                    <asp:GridView ID="grdAsignados" DataKeyNames="id" CssClass="columna" runat="server"
                                    AutoGenerateColumns="False" Width="100%" ViewStateMode="Enabled" ShowHeaderWhenEmpty="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Seleccionar" ItemStyle-Width="6%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbox" ClientIDMode="Static" CssClass="cbox" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="ID" DataField="id" Visible="True" ItemStyle-Width="7%" SortExpression="id" >
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Apellido y Nombre" DataField="ApellidoyNombre" ItemStyle-Width="70%" SortExpression="nombre" >
                                        </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <br />
                            </asp:Panel>
                        <rsweb:ReportViewer ID="rv1" runat="server"
                                    Font-Names="Verdana" Font-Size="8pt"
                                    WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                                    Height="450px" Width="100%"
                                    PageCountMode="Actual">
                                    <LocalReport ReportPath="Reports\TareasReporte.rdlc">
                                        <DataSources>
                                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet2" />
                                        </DataSources>
                                    </LocalReport>
                                </rsweb:ReportViewer>
                                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="facturadorDataSetTableAdapters.vw_libroivaventasTableAdapter"></asp:ObjectDataSource>
                        </div>

                            </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>
