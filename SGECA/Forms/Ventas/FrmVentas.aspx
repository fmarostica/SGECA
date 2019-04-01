<%@ Page Title="Venta - Ventas" Language="C#" MasterPageFile="~/MasterPages/Ventas.master" AutoEventWireup="true" CodeBehind="FrmVentas.aspx.cs" Inherits="SGECA.Forms.Ventas.FrmVentas" EnableEventValidation="false" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function menuOpciones() {
            $('#<%=menuOpciones.ClientID%>').css('display', 'block');

            $('#<%=menuOpciones.ClientID%>').animate({ marginRight: "200px" });
        }

        function menuOpcionesOculto() {
            $('#<%=menuOpciones.ClientID%>').animate({ right: "-400px" });
        }

        function deshabilitoControles() {
            var nodes = document.getElementById("<%=divBotones.ClientID%>").getElementsByTagName('*');
            for (var i = 0; i < nodes.length; i++) {
                nodes[i].hidden = true;
            }
            return true;
        }

        function habilitoControles() {
            var nodes = document.getElementById("<%=divBotones.ClientID%>").getElementsByTagName('*');
            for (var i = 0; i < nodes.length; i++) {
                nodes[i].hidden = false;
            }
            return true;
        }

        function ConfirmaGuardar() {
            var ret = confirm("Está seguro de guardar el documento sin generar CAE?");
            if (ret) {
                var nodes = document.getElementById("<%=divBotones.ClientID%>").getElementsByTagName('*');
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].hidden = true;
                }
                return true;
            } else
                return false;
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="cmbCliente" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnAgregar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="txtCantidad" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="txtCodigo" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="txtImporte" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnSube" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnBaja" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnElimina" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnGuardarConCAE" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="grdItems" />
            <asp:AsyncPostBackTrigger ControlID="cmbComprobante" />
        </Triggers>
        <ContentTemplate>
            <asp:UpdateProgress ID="updateProgress" runat="server">
                <ProgressTemplate>
                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                        <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/Imagenes/loading.gif" AlternateText="Loading ..." Width="200" Height="200" ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 20%; left: 40%;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <div id="menuOpciones" runat="server"
                style="width: 60px; height: 120px; border-radius: 5px 0px 0px 5px; position: fixed; top: 280px; right: -200px; margin-right: 200px; display: block;"
                visible="false">
                <asp:ImageButton ID="btnSube" runat="server" AccessKey="S" CommandName="btnSube" OnClick="btnSube_Click" ImageUrl="~/Imagenes/arriba.png" Width="30" Height="31" />
                <asp:ImageButton ID="btnBaja" runat="server" AccessKey="B" CommandName="btnBaja" OnClick="btnBaja_Click" ImageUrl="~/Imagenes/abajo.png" Width="30" Height="31" />
                <asp:ImageButton ID="btnElimina" runat="server" AccessKey="E" CommandName="btnElimina" OnClick="btnElimina_Click" ImageUrl="~/Imagenes/eliminar.png" Width="30" Height="31" />
            </div>


            <table style="width: 100%;">
                <tr>
                    <td style="width: 70%;">
                        <fieldset style="height:190px">
                            <legend>Datos Clientes</legend>
                            <table style="width: 100%;">
                                <tr>
                                    <td><asp:Label runat="server" CssClass="lblField">Cliente:</asp:Label></td>
                                    <td>
                                        <asp:DropDownList CssClass="input1" ID="cmbCliente" AutoPostBack="true" Width="382px" OnTextChanged="cmbCliente_TextChanged" runat="server" OnSelectedIndexChanged="cmbCliente_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td><asp:Label runat="server" CssClass="lblField">Fecha:</asp:Label></td>
                                    <td>
                                        <asp:TextBox CssClass="input1" ID="txtFecha" runat="server" Width="152px" EnableViewState="True"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label runat="server" CssClass="lblField">T. Resp:</asp:Label></td>
                                    <td>
                                        <asp:DropDownList CssClass="input1" ID="cmbTipoResponsable" runat="server" Width="382px" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                    <td><asp:Label runat="server" CssClass="lblField">C.U.I.T.</asp:Label></td>
                                    <td>
                                        <asp:TextBox CssClass="input1" ID="txtCuit" AutoPostBack="True" onkeypress="return verificarNumeroReal(this,event);" runat="server" Width="152px" MaxLength="11"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label runat="server" CssClass="lblField">R. Social:</asp:Label></td>
                                    <td>
                                        <asp:TextBox CssClass="input1" AutoPostBack="True" ID="txtrazonSocial" Style="position: relative; top: 3px;" runat="server" Width="378px"></asp:TextBox>
                                    </td>
                                    <td><asp:Label runat="server" CssClass="lblField">IIBB:</asp:Label></td>
                                    <td>
                                        <asp:TextBox CssClass="input1" ID="txtingbruto" runat="server" Width="152px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label runat="server" CssClass="lblField">Dirección</asp:Label></td>
                                    <td>
                                        <asp:TextBox CssClass="input1" AutoPostBack="True" ID="txtDireccion" runat="server" Style="position: relative; top: 3px;" Width="378px"></asp:TextBox></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td><asp:Label runat="server" CssClass="lblField">Localidad:</asp:Label></td>
                                    <td>
                                        <asp:TextBox CssClass="input1" ID="txtLocalidad" runat="server" Style="position: relative; top: 3px;" Width="378px"></asp:TextBox></td>
                                    <td><asp:Label runat="server" CssClass="lblField">Provincia:</asp:Label></td>
                                    <td>
                                        <asp:TextBox CssClass="input1" AutoPostBack="True" ID="txtProvincia" runat="server" Width="152px" MaxLength="11"></asp:TextBox></td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td>
                        <fieldset style="height:190px">
                            <legend>
                                Datos Comprobantes
                                <asp:Label ID="lblID" runat="server" />
                            </legend>
                            <table style="width: 100%; text-align: left">
                                <tr>
                                    <td><asp:Label runat="server" CssClass="lblField">Comprobante</asp:Label></td>
                                    <td>
                                        <asp:DropDownList CssClass="input1" ID="cmbComprobante" runat="server" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="cmbComprobante_SelectedIndexChanged"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td><asp:Label runat="server" CssClass="lblField">Sucursal:</asp:Label></td>
                                    <td>
                                        <asp:DropDownList CssClass="input1" ID="cmbSucursal" Width="200px" runat="server"></asp:DropDownList></td>
                                </tr>

                                <tr>
                                    <td><asp:Label runat="server" CssClass="lblField">Tipo Doc.:</asp:Label></td>
                                    <td>
                                        <asp:DropDownList CssClass="input1" ID="cmbTipo" runat="server" Width="200px"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td><asp:Label runat="server" CssClass="lblField">ID/Numero:</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtNumero" CssClass="input1" runat="server" Width="196px" Enabled="False"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>



            <asp:Panel ID="formularioItem" runat="server" Style="background-color: #147687; height: 25px;">
                <table>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtCodigo" runat="server" placeholder="Código" CssClass="CodigoProducto" AutoPostBack="True" Width="63px" OnTextChanged="txtCodigo_TextChanged" EnableViewState="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescripción" runat="server" placeholder="Descripción" AutoPostBack="True" OnTextChanged="txtDescripción_TextChanged" Width="378px" EnableViewState="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCantidad" CssClass="inputFrom" onkeypress="return numerodecimal(this,event);" runat="server" Numeric="True" placeholder="Cantidad" AutoPostBack="True" Width="90px" OnTextChanged="txtCantidad_TextChanged" EnableViewState="True"></asp:TextBox></td>

                        <td>
                            <asp:TextBox ID="txtImporte" Width="88px" CssClass="inputFrom" onkeyup="return verificarNumeroRealySubmit(this,event);" runat="server" placeholder="Importe" AutoPostBack="True" OnTextChanged="txtImporte_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescuento" Width="80px" onkeypress="return verificarNumeroReal(this,event);" runat="server" placeholder="Descuento" AutoPostBack="True" OnTextChanged="txtDescuento_TextChanged" Visible="true" disabled="disabled"></asp:TextBox></td>
                        <td>

                            <asp:TextBox ID="txtIVA" Width="74px" runat="server" AutoPostBack="True" Visible="true"></asp:TextBox></td>

                        <td>
                            <asp:Button ID="btnAgregar" Width="70px" runat="server" Text="Agregar" OnClick="btnAgregar_Click" CssClass="btnAgr" UseSubmitBehavior="True" Style="top: -0.5px"></asp:Button></td>
                        <td>
                            <asp:TextBox ID="txtSubTotal" CssClass="inputFrom" Width="124px" runat="server" placeholder="Neto" Enabled="false" AutoPostBack="True"></asp:TextBox>
                        </td>
                    </tr>



                </table>

            </asp:Panel>

            <div class="1CSSTableGenerator" style="height: 240px; overflow-y: auto;" id="divGrdVentas" runat="server">
                <asp:GridView ID="grdItems" CssClass="columna" runat="server"
                    AutoGenerateColumns="False" ViewStateMode="Disabled" Width="100%"
                    OnRowDeleted="grdItems_RowDeleted" OnRowDeleting="grdItems_RowDeleting"
                    ForeColor="#272727" Font-Size="11px" Height="10px" OnRowCancelingEdit="grdItems_RowCancelingEdit" OnRowEditing="grdItems_RowEditing" OnRowUpdating="grdItems_RowUpdating" OnDataBound="grdItems_DataBound" OnSelectedIndexChanged="grdItems_SelectedIndexChanged" OnSelectedIndexChanging="grdItems_SelectedIndexChanging">
                    <Columns>
                        <asp:BoundField HeaderText="Codigo" DataField="Codigo" ItemStyle-Width="69px" ControlStyle-Width="65px">
                            <ControlStyle Width="65px" />
                            <ItemStyle Width="69px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Descripcion" DataField="Descripcion" ItemStyle-Width="382px" ControlStyle-Width="378px">
                            <ControlStyle Width="378px" />
                            <ItemStyle Width="382px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Cantidad" ItemStyle-Width="95px" ControlStyle-Width="90px" ItemStyle-HorizontalAlign="Right" DataField="Cantidad" DataFormatString="{0:F2} " ApplyFormatInEditMode="True">
                            <ControlStyle Width="90px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="P.Unit" ItemStyle-Width="95px" ControlStyle-Width="90px" ItemStyle-HorizontalAlign="Right" DataField="Precio" DataFormatString="{0:F2}" ApplyFormatInEditMode="True">
                            <ControlStyle Width="90px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Id" HeaderText="Id" Visible="False" />
                        <asp:BoundField DataField="AlicuotaPorcentaje" ItemStyle-Width="81px" ControlStyle-Width="76px" ItemStyle-HorizontalAlign="Right" HeaderText="%IVA" DataFormatString="{0:F2}" ApplyFormatInEditMode="True">
                            <ControlStyle Width="76px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <%--                        <asp:BoundField DataField="Descuento" ItemStyle-HorizontalAlign="Right" HeaderText="Descuento" DataFormatString="{0:F2}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>--%>
                        <asp:BoundField HeaderText="SubTotal" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="75px" ControlStyle-Width="73px" DataField="Subtotalneto" DataFormatString="{0:F2}" ApplyFormatInEditMode="True">
                            <ControlStyle Width="73px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>

                        <asp:CommandField ShowSelectButton="True" ItemStyle-Height="5" ButtonType="Image" SelectImageUrl="~/Images/list-16.png" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle Height="5px" HorizontalAlign="Center" Width="20px" />
                        </asp:CommandField>

                        <asp:CommandField ShowEditButton="True" CancelImageUrl="~/Images/cross-16.png" CancelText="" EditImageUrl="~/Images/edit-16.png" UpdateImageUrl="~/Images/check-16.png" UpdateText="" ButtonType="Image" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                        </asp:CommandField>

                    </Columns>
                    <HeaderStyle BackColor="#00CC00" />
                    <RowStyle BackColor="#FFCC66" ForeColor="Black" Height="5px" />
                    <SelectedRowStyle BackColor="#99CCFF" BorderColor="#000066" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" />
                </asp:GridView>
            </div>


            <table style="width: 100%; margin: 0 auto;">
                <tr>
                    <td style="width: 70%;">
                        <fieldset>
                            <legend>Otros Datos</legend>
                            <table style="width: 720px; text-align: left">
                                <tr>
                                    <td>Lista de Precios</td>
                                    <td>
                                        <asp:DropDownList CssClass="input1" ID="cmbListaPrecio" Width="200px" runat="server"></asp:DropDownList></td>
                                    <td>Vendedor</td>
                                    <td>
                                        <asp:DropDownList CssClass="input1" ID="cmbVendedor" Width="200px" runat="server"></asp:DropDownList></td>
                                </tr>
                    </td>
                </tr>
                <tr>
                    <td>Concepto Venta</td>
                    <td>
                        <asp:DropDownList CssClass="input1" ID="cmbConceptoventa" Width="200px" runat="server"></asp:DropDownList></td>

                    <td>Caja</td>
                    <td>
                        <asp:DropDownList CssClass="input1" ID="cmbCaja" Width="200px" runat="server"></asp:DropDownList></td>

                </tr>
                <tr>
                    <td>Condicion de Pago</td>
                    <td>
                        <asp:DropDownList CssClass="input1" ID="cmbCondPago" Width="200px" runat="server"></asp:DropDownList></td>
                    <td></td>
                    <td>
                        <%--<asp:Button ID="btnSube" runat="server" AccessKey="S" CommandName="btnSube" OnClick="btnSube_Click" Text="Sube" />
                        <asp:Button ID="btnBaja" runat="server" AccessKey="B" CommandName="btnBaja" CssClass="buttonMenu" OnClick="btnBaja_Click" Text="Baja" />
                        <asp:Button ID="btnElimina" runat="server" AccessKey="B" CommandName="btnBaja" CssClass="buttonMenu" OnClick="btnElimina_Click" Text="Elimina" />--%>
                    </td>
                </tr>

            </table>
            </fieldset>
                        </td>

                        <td>
                            <fieldset>
                                <legend>Totales</legend>
                                <table style="width: 279px; text-align: left">
                                    <tr>
                                        <td>Neto:</td>
                                        <td>
                                            <asp:TextBox CssClass="input1" ID="txtSubTotalNeto" runat="server" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Iva:</td>
                                        <td>
                                            <asp:TextBox CssClass="input1" ID="txtIvatotal" runat="server" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Total:</td>
                                        <td>
                                            <asp:TextBox CssClass="input1" ID="txtTotal" runat="server" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
            </tr>
                </table>
                 <div id="divBotones" runat="server" class="botonera">
                     <asp:Button AccessKey="G" CssClass="buttonMenu" ID="btnGuardar" runat="server" Text="Guardar [G]" OnClick="btnGuardar_Click" OnClientClick="return ConfirmaGuardar();" />
                     <asp:Button ID="btnGuardarConCAE" runat="server" AccessKey="E" CssClass="buttonMenu" Text="Guardar con CAE [E]" OnClick="btnGuardarConCAE_Click" OnClientClick="return deshabilitoControles();" />
                     <asp:Button ID="btnReimprimir" runat="server" AccessKey="R" CssClass="buttonMenu" OnClick="btnReimprimir_Click" Text="Reimprimir [R]" />
                     <asp:Label ID="lblCantReg" runat="server" ForeColor="White" Text="Cantidad Registros: 0"></asp:Label>
                     <asp:Button CssClass="buttonMenu" ID="btnCancelar" runat="server" Text="Cancelar [F4]" OnClick="btnCancelar_Click" />
                 </div>
            <ajax:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtIVA"
                MinimumPrefixLength="1" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100" ServiceMethod="ObtenerAlicuotasIVA">
            </ajax:AutoCompleteExtender>



        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

