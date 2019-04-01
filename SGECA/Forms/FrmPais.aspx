<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="FrmPais.aspx.cs" Inherits="SGECA.Forms.FrmPais" %>

<%@ Register Src="~/Controles/BusquedaConFiltro.ascx" TagPrefix="uc1" TagName="BusquedaConFiltro" %>



<%@ Register src="../Controles/Paginador.ascx" tagname="Paginador" tagprefix="uc2" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:GridView ID="grdGrilla" runat="server" 
                 AllowSorting="True"  OnSelectedIndexChanged="grdGrilla_SelectedIndexChanged"
                 OnSelectedIndexChanging="grdGrilla_SelectedIndexChanging" 
                OnPageIndexChanged="grdGrilla_PageIndexChanged" 
                OnPageIndexChanging="grdGrilla_PageIndexChanging"
                OnSorted="grdGrilla_Sorted"
                OnSorting="grdGrilla_Sorting"
                AutoGenerateColumns="false">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="pai_id" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="pai_nombre" />
                </Columns>
            </asp:GridView>

            <uc2:Paginador ID="pagPaginador" runat="server" OnAnterior="pagPaginador_Anterior"
                OnFin="pagPaginador_Fin" OnInicio="pagPaginador_Inicio"  OnProxima="pagPaginador_Proxima"
                 OnPaginaSeleccionada="pagPaginador_PaginaSeleccionada" />
            <br />

            <uc1:BusquedaConFiltro 
                runat="server" 
                id="BusquedaConFiltro" 
                
                />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
