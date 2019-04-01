<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Gestion.master" AutoEventWireup="true" CodeBehind="frmLogs.aspx.cs" Inherits="SGECA.Forms.Gestion.frmLogs" EnableEventValidation="false" %>
<%@ Register Src="~/Controles/Paginador.ascx" TagName="Paginador" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="main_contenido">
        <asp:Label CssClass="lblField" runat="server">Buscar : </asp:Label>
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="input1" OnTextChanged="txtBuscar_TextChanged" AutoPostBack="True"></asp:TextBox>
                    <asp:Label runat="server"></asp:Label>
                    <div class="CSSTableGenerator">
                            <asp:GridView ID="grdLogs" DataKeyNames="fecha" CssClass="columna" runat="server"
                            AutoGenerateColumns="False" Width="100%"
                            OnSorted="grdLogs_Sorted"
                            OnSorting="grdLogs_Sorting"
                            OnSelectedIndexChanged="grdLogs_SelectedIndexChanged" ViewStateMode="Enabled" OnRowDataBound="grdLogs_RowDataBound" EnablePersistedSelection="True" AllowSorting="True">
                            <Columns>
                                <asp:BoundField HeaderText="FECHA" DataField="fecha" Visible="True" ItemStyle-Width="7%" SortExpression="fecha" >
                                <ItemStyle Width="2%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="HORA" DataField="hora" ItemStyle-Width="10%" SortExpression="hora" >
                                <ItemStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="USUARIO" DataField="usuario" ItemStyle-Width="10%" SortExpression="usuario" >
                                <ItemStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="MODULO" DataField="modulo" ItemStyle-Width="10%" SortExpression="modulo" >
                                <ItemStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="DETALLES" DataField="accion" ItemStyle-Width="10%" SortExpression="accion" >
                                <ItemStyle Width="10%" />
                                </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                <br />
                <uc2:Paginador ID="pagPaginador" runat="server" OnAnterior="pagPaginador_Anterior"
                            OnFin="pagPaginador_Fin" OnInicio="pagPaginador_Inicio" OnProxima="pagPaginador_Proxima"
                            OnPaginaSeleccionada="pagPaginador_PaginaSeleccionada" ViewStateMode="Enabled"   />
                        <br />
                        
                </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphPie" runat="server">
</asp:Content>
