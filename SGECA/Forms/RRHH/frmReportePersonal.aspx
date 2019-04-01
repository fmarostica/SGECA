<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/rrhh.master" AutoEventWireup="true" CodeBehind="frmReportePersonal.aspx.cs" Inherits="SGECA.Forms.RRHH.frmReportePersonal" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:Label runat="server" CssClass="lblField" Text="Estado: "></asp:Label>
    <asp:DropDownList ID="txtEmpleadoEstado" runat ="server" Width="150">
        <asp:ListItem Text="TODOS" />
        <asp:ListItem Text="ACTIVO" />
        <asp:ListItem Text="BAJA" />
    </asp:DropDownList>
    <asp:Button runat="server" ID="btnGenerar" OnClick="btnGenerar_Click" CssClass="buttonMenu" Text="Generar" />

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
            <rsweb:ReportViewer ID="rv1" runat="server"
                Font-Names="Verdana" Font-Size="8pt"
                WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                Height="450px" Width="100%"
                PageCountMode="Actual">
                <LocalReport ReportPath="Reports\TareasReporte.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="DataSet2" />
                </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="facturadorDataSetTableAdapters.vw_libroivaventasTableAdapter"></asp:ObjectDataSource>
        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>
