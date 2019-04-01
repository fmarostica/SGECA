<%@ Page Title="Comprobantes - Ventas" Language="C#" MasterPageFile="~/MasterPages/Ventas.master" AutoEventWireup="true" CodeBehind="FrmVistaNotaVenta.aspx.cs" Inherits="SGECA.Forms.Ventas.FrmVistaNotaVenta" %>

<%@ Register Src="~/Controles/BusquedaConFiltro.ascx" TagPrefix="uc1" TagName="BusquedaConFiltro" %>
<%@ Register Src="~/Controles/Paginador.ascx" TagName="Paginador" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">


    <asp:ScriptManager ID="ScriptManager3" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>

            <table style="width: 1021px; height: 380px; overflow: auto; margin: 0 auto;">
                <tr>
                    <td>

                        <div class="CSSTableGenerator" style="height: 100%; width: 99%; overflow: auto;">

                            <asp:GridView ID="grdComprobantes" DataKeyNames="ID" CssClass="columna" runat="server"
                                AutoGenerateColumns="False" Width="100%"
                                OnSorted="grdComprobantes_Sorted"
                                OnSorting="grdComprobantes_Sorting"
                                OnSelectedIndexChanged="grdComprobantes_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField HeaderText="ID" DataField="ID" Visible="True" SortExpression="cen_id" />
                                    <asp:BoundField HeaderText="Fecha" DataField="Fecha" ItemStyle-Width="10%" SortExpression="cen_fecha" />
                                    <asp:BoundField HeaderText="Comprobante" DataField="NumeroCompleto" ItemStyle-Width="12%" SortExpression="cen_numerocompleto" />
                                    <asp:BoundField HeaderText="Cliente" DataField="RazonSocial" SortExpression="Cli_RazonSocial" />
                                    <asp:BoundField HeaderText="Neto" DataField="Neto" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" SortExpression="cen_neto" />
                                    <asp:BoundField HeaderText="IVA" DataField="IVA" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" SortExpression="cen_ivatotal" />
                                    <asp:BoundField DataField="Total" HeaderText="Total" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" SortExpression="cen_total" />
                                    <asp:CommandField HeaderText="Seleccionar" ShowSelectButton="True" ItemStyle-Width="7%" />
                                </Columns>
                            </asp:GridView>

                        </div>

                    </td>
                </tr>
            </table>


            <uc2:Paginador ID="pagPaginador" runat="server" OnAnterior="pagPaginador_Anterior"
                OnFin="pagPaginador_Fin" OnInicio="pagPaginador_Inicio" OnProxima="pagPaginador_Proxima"
                OnPaginaSeleccionada="pagPaginador_PaginaSeleccionada" />


            <uc1:BusquedaConFiltro
                runat="server"
                ID="BusquedaConFiltro" />


            <br />





        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
