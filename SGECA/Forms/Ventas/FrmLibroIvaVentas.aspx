<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Ventas.master" AutoEventWireup="true" CodeBehind="FrmLibroIvaVentas.aspx.cs" Inherits="SGECA.Forms.Ventas.FrmLibroIvaVentas" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 222px;
        }
    </style>
    <script src="/scripts/jquery-1.7.1.min.js" type="text/javascript"></script>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>


    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="updateProgress" runat="server">
                <ProgressTemplate>
                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                        <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Imagenes/loading.gif" AlternateText="Loading ..." Width="200" Height="200" ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 20%; left: 40%;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>





            <table style="width: 100%; margin: 0 auto;">
                <tr>
                    <td>
                        <div style="text-align: center">
                            <fieldset>
                                <legend>Libro IVA Ventas</legend>
                                <asp:Panel runat="server" ID="panel1">
                                    <div style="font-weight: bold; background-color: ThreeDShadow;">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td align="left">Opciones de generación del informe
                        <asp:Label runat="server" ID="textLabel" />
                                                </td>
                                                <td align="right" width="10px">
                                                    <asp:Image ID="Image1" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </asp:Panel>

                                <asp:Panel runat="server" ID="panel2" Width="98%" HorizontalAlign="center">
                                    <table cellspacing="10" style="margin: 0 auto; text-align: left;">
                                        <tr>
                                            <td>Desde:</td>
                                            <td>

                                                <input type="text" required="required" placeholder="dd/mm/aaaa" id="txtDesde" class="input1" runat="server" />
                                            </td>
                                            <td>Hasta:</td>
                                            <td class="auto-style1">
                                                <input required="required" type="text" placeholder="dd/mm/aaaa" id="txtHasta" class="input1" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Titulo:</td>
                                            <td>
                                                <asp:TextBox ID="txtTitulo" runat="server" CssClass="input1 input1Altura">Libro IVA Ventas</asp:TextBox>
                                            </td>
                                            <td style="width: 80px; position: relative; top: -3px;">Ultimo Folio Impreso:</td>
                                            <td class="auto-style1">
                                                <asp:TextBox ID="txtNumFolio" runat="server" AutoPostBack="True" CssClass="input1 input1Altura" onkeypress="return verificarNumeroReal(this,event);" title="Ingrese el ultimo Numero de Folio Impreso"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <div style="text-align: center">
                                                    <table style="margin: 0 auto;">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnGenerar" runat="server" CssClass="buttonMenu" OnClick="btnGenerar_Click" Text="Generar" Width="100px" />
                                                                <asp:Button ID="btnCancelar" runat="server" CssClass="buttonMenu" OnClick="btnCancelar_Click" Text="Cancelar" Width="100px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>

                                    </table>
                                </asp:Panel>

                                <ajaxToolkit:CollapsiblePanelExtender runat="server" ID="cpeOpcionesInforme" TargetControlID="panel2" CollapseControlID="panel1" ExpandControlID="panel1" CollapsedSize="0" ExpandedSize="170" ExpandedText="(Ocultar...)" CollapsedText="(Mostrar...)" TextLabelID="textLabel" ImageControlID="Image1" ExpandedImage="~/Imagenes/collapse.jpg" CollapsedImage="~/Imagenes/expand.jpg">
                                </ajaxToolkit:CollapsiblePanelExtender>

                                <rsweb:ReportViewer ID="rv1" runat="server"
                                    Font-Names="Verdana" Font-Size="8pt"
                                    WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                                    Height="500px" Width="100%" ShowPrintButton="false"
                                    PageCountMode="Actual">
                                    <LocalReport ReportPath="Reports\LibroIvaVentas.rdlc">
                                        <DataSources>
                                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                                        </DataSources>
                                    </LocalReport>
                                </rsweb:ReportViewer>
                                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="facturadorDataSetTableAdapters.vw_libroivaventasTableAdapter"></asp:ObjectDataSource>



                            </fieldset>

                        </div>
                    </td>
                </tr>
            </table>



        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>
