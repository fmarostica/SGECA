<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Produccion.master" AutoEventWireup="true" CodeBehind="frmReporteTareasGrupos.aspx.cs" Inherits="SGECA.Forms.Produccion.frmReporteTareasGrupos" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/Controles/Paginador.ascx" TagName="Paginador" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <script>
        $("#mTitle").text("REPORTE DE TAREAS");

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

                        <asp:Button ID="btnGenerar" Text="GENERAR" CssClass="buttonMenu" runat="server" OnClick="btnGenerar_Click" />


                        <div class="main_contenido">
                            <asp:Panel ID="contenedor_principal" runat="server">
                                <asp:Label ID="lblDesde" Text="Desde : " CssClass="lblField" runat="server"></asp:Label>
                                <asp:TextBox ID="txtDesde" ClientIDMode="Static" CssClass="input1" runat="server" Width="80px"></asp:TextBox>
                                <asp:Label ID="lblHasta" Text="Hasta : " CssClass="lblField" runat="server"></asp:Label>
                                <asp:TextBox ID="txtHasta" ClientIDMode="Static" CssClass="input1" runat="server" Width="80px"></asp:TextBox>
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
