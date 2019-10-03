<%@ Page Title="" Language="C#" MasterPageFile="~/COTIZADOR/Cotizador.Master" AutoEventWireup="true" CodeBehind="Lista_de_Precios.aspx.cs" Inherits="COTIZADOR.Lista_de_Precios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CONTENIDO_BODY" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <section class="horizontal-grid" id="horizontal-grid">
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header">
                        <h4>Lista de Precios</h4>
                    </div>
                    <div class="card-content collapse show">
                        <div class="card-body">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div class="form form-horizontal">
                                        <div class="form-body">
                                            <h4 class="form-section"><i class="fa fa-edit text-danger"></i>Detalle de la Cotización</h4>
                                            <div class="form-group row">
                                                <label class="col-md-3 label-control" for="projectinput1">Columnas de Precio</label>
                                                <div class="col-sm-6">
                                                    <asp:DropDownList runat="server" CssClass="form-control input-sm" ID="CB_BODEGA"></asp:DropDownList>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:LinkButton runat="server" ID="B_FILTRAR_PRECIOS" OnClick="B_FILTRAR_PRECIOS_Click" CssClass="btn btn-success btn-sm">MOSTRAR LISTA</asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="form-group row">
                                                <div class="col-sm-12">
                                                    <div class="table-responsive">
                                                        <asp:Label runat="server" ID="LB_FECHA_PRECIOS" Font-Size="Medium" Font-Bold="true" CssClass="text-center"></asp:Label>
                                                        <br />
                                                        <asp:GridView DataKeyNames="cod_producto, producto, id_categoria, precioventa1, precioventa2, precioventa3, preciounitario1, preciounitario2, preciounitario3, unidad_equivale" runat="server" ID="G_PRODUCTOS" CssClass="table table-xs hide-columns-dynamically table-bordered compact" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                            <HeaderStyle CssClass="thead-dark" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Seleccione" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox runat="server" ID="CHK_ENVIAR" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="id_categoria" DataField="id_categoria" Visible="false" HeaderStyle-CssClass="stick" />
                                                                <asp:BoundField HeaderText="Codigo" DataField="cod_producto" HeaderStyle-CssClass="stick" />
                                                                <asp:BoundField HeaderText="Nombre Producto" DataField="producto" HeaderStyle-CssClass="stick" />
                                                                <asp:BoundField HeaderText="Stock" DataField="stock" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="stick" DataFormatString="{0:N0}" />
                                                                <asp:BoundField HeaderText=" " DataField="medida" HeaderStyle-CssClass="stick" ItemStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField HeaderText="" DataField="precioventa1" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C0}" />
                                                                <asp:BoundField HeaderText="" DataField="preciounitario1" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C0}" />
                                                                <asp:BoundField HeaderText="" DataField="precioventa2" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C0}" Visible="false" />
                                                                <asp:BoundField HeaderText="" DataField="preciounitario2" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C0}" Visible="false" />
                                                                <asp:BoundField HeaderText="" DataField="precioventa3" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C0}" Visible="false" />
                                                                <asp:BoundField HeaderText="." DataField="preciounitario3" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C0}" Visible="false" />
                                                                <asp:BoundField HeaderText="Categoría" DataField="nom_categ" />
                                                                <asp:BoundField HeaderText="unidad_equivale" DataField="unidad_equivale" Visible="false" />
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                No se encontraron resultados.
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CODIGO_JAVASCRIPT" runat="server">
    <script>
        var table_productos;

        $(document).ready(function () {
            Datatables();
        });

        function Datatables() {
            relojito(false);
            try {
                table_productos = $('#<%= G_PRODUCTOS.ClientID %>').DataTable({
                     "language": {
                         "decimal": ",",
                         "thousands": "."
                     },
                     "order": [[1, "asc"]],
                     destroy: true,
                     "paging": false

                 });
                 filtros_por_columna('<%= G_PRODUCTOS.ClientID %>', table_productos);
            }
            catch (x) {

            }
        }

    </script>
</asp:Content>
