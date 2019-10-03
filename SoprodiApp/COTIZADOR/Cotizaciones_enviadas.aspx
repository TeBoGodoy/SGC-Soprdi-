<%@ Page Title="" Language="C#" MasterPageFile="~/COTIZADOR/Cotizador.Master" AutoEventWireup="true" CodeBehind="Cotizaciones_enviadas.aspx.cs" Inherits="COTIZADOR.Cotizaciones_enviadas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CONTENIDO_BODY" runat="server">
    <style>
        .stick {
            position: sticky;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <section>
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h4 class="card-title">SOPRODI | Cotizaciones Enviadas </h4>
                        <a class="heading-elements-toggle"><i class="fa fa-ellipsis-v font-medium-3"></i></a>
                        <div class="heading-elements">
                            <ul class="list-inline mb-0">
                                <li><a data-action="expand"><i class="ft-maximize"></i></a></li>
                            </ul>
                        </div>
                    </div>
                    <div class="card-content collapse show">
                        <div class="card-body card-dashboard">
                            <asp:UpdatePanel runat="server" ID="UP_LISTA" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="G_PRINCIPAL" />
                                    <asp:AsyncPostBackTrigger ControlID="G_DETALLE_COTIZACION" />
                                    <asp:AsyncPostBackTrigger ControlID="G_SERVICIOS" />
                                    <asp:AsyncPostBackTrigger ControlID="B_VOLVER" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="PANEL_PRINCIPAL">
                                        <div class="table-responsive">
                                            <asp:GridView DataKeyNames="id_cotizacion_log, fecha_envio, correo_cliente, nombre cliente" runat="server" ID="G_PRINCIPAL" CssClass="table table-bordered compact table-xs" OnRowCommand="G_PRINCIPAL_RowCommand" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                <HeaderStyle CssClass="thead-dark" />
                                                <Columns>
                                                    <asp:BoundField HeaderText="Id" DataField="id_cotizacion_log" />
                                                    <asp:BoundField HeaderText="Fecha Envío" DataField="fecha_envio" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                                    <asp:BoundField HeaderText="Nombre" DataField="nombre_cotizacion" />
                                                    <asp:BoundField HeaderText="Cliente" DataField="nombre cliente" />
                                                    <asp:BoundField HeaderText="Correo" DataField="correo_cliente" />
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Opciones" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ClientIDMode="AutoID" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Ver">
                                                                   <i class="icon-pencil"></i>
                                                            </asp:LinkButton>                                                           
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    No se encontraron resultados.
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="PANEL_DETALLE" Visible="false">
                                        <div style="display: none">
                                            <asp:TextBox runat="server" ID="T_ID" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                        <div class="form form-horizontal">
                                            <div class="form-body">
                                                <h4 class="form-section"><i class="fa fa-edit text-danger"></i>Detalle de la Cotización</h4>
                                                <div class="form-group row">
                                                    <div class="col-sm-3">
                                                        <small>CLIENTE</small>
                                                        <asp:TextBox runat="server" ID="T_CLIENTE" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <small>FECHA</small>
                                                        <asp:TextBox runat="server" ID="T_FECHA" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <small>CORREO</small>
                                                        <asp:TextBox runat="server" ID="T_CORREO" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <div class="col-md-12" style="overflow-x: auto">
                                                        <div class="table-responsive">
                                                            <button style="display: none" id="btn_abre_modal" type="button" data-toggle="modal" data-target="#modalproducto" name="#focusable"></button>
                                                            <asp:GridView DataKeyNames="" runat="server" ID="G_DETALLE_COTIZACION" CssClass="table table-bordered table-xs compact" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" Width="100%">
                                                                <HeaderStyle CssClass="thead-dark" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Nº">
                                                                        <ItemTemplate>
                                                                            <%# Container.DataItemIndex + 1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="id_cotizacion_log" DataField="id_cotizacion" Visible="false" />
                                                                    <asp:BoundField HeaderText="id_cotizacion_det_log" DataField="id_cotizacion_det" Visible="false" />
                                                                    <asp:BoundField HeaderText="Cod. Producto" DataField="producto" />
                                                                    <asp:BoundField HeaderText="Producto" DataField="nom_producto" />
                                                                    <asp:TemplateField HeaderText="Precio / Descuento" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="#ffffcc">
                                                                        <ItemTemplate>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <small>Precio</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_precio_1" Style="width: 100px;" ReadOnly="true" Text='<%# Eval("precio_1")%>'></asp:TextBox>
                                                                                        <small>Precio Unitario</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="precio_unitario_1" Style="width: 100px;" ReadOnly="true" Text='<%# Eval("precio_unitario_1")%>'></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Cantidad</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-center" ID="det_cantidad_1" Style="width: 80px;" ReadOnly="true" Text='<%# Eval("cantidad_1")%>'></asp:TextBox>
                                                                                        <span style="display: none"><%# Eval("cantidad_1")%></span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Descuento (%)</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_descuento_1" Style="width: 100px;" ReadOnly="true" Text='<%# Eval("descuento_1")%>'></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Precio (%)</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_precio_descuento_1" Style="width: 100px;" Text='<%# Eval("precio_con_descuento_1")%>' ReadOnly="true"></asp:TextBox>
                                                                                        <small>Precio Unit.(%)</small><br />
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="precio_unitario_desc_1" Style="width: 100px;" Text='<%# Eval("precio_con_descuento_unitario_1")%>' ReadOnly="true"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Total</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_total_1" Style="width: 100px;" Text='<%# Eval("total_1")%>' ReadOnly="true"></asp:TextBox>
                                                                                        <small>Total + IVA</small><br />
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="total_iva_1" Style="width: 100px;" Text='<%# Eval("total_iva_1")%>' ReadOnly="true"></asp:TextBox>

                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Precio / Descuento" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="#ccffcc" Visible="false">
                                                                        <ItemTemplate>
                                                                            <table style="width: 100%">
                                                                                <tr>

                                                                                    <td>
                                                                                        <small>Precio</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_precio_2" Style="width: 100px;" ReadOnly="true" Text='<%# Eval("precio_2")%>'></asp:TextBox>
                                                                                        <small>Precio Unitario</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="precio_unitario_2" Style="width: 100px;" ReadOnly="true" Text='<%# Eval("precio_unitario_2")%>'></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Cantidad</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-center" ID="det_cantidad_2" Style="width: 80px;" ReadOnly="true" Text='<%# Eval("cantidad_2")%>'></asp:TextBox>
                                                                                        <span style="display: none"><%# Eval("cantidad_2")%></span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Descuento (%)</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_descuento_2" Style="width: 100px;" ReadOnly="true" Text='<%# Eval("descuento_2")%>'></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Precio (%)</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_precio_descuento_2" Style="width: 100px;" Text='<%# Eval("precio_con_descuento_2")%>' ReadOnly="true"></asp:TextBox>
                                                                                        <small>Precio Unit.(%)</small><br />
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="precio_unitario_desc_2" Style="width: 100px;" Text='<%# Eval("precio_con_descuento_unitario_2")%>' ReadOnly="true"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Total</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_total_2" Style="width: 100px;" Text='<%# Eval("total_2")%>' ReadOnly="true"></asp:TextBox>
                                                                                        <small>Total + IVA</small><br />
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="total_iva_2" Style="width: 100px;" Text='<%# Eval("total_iva_2")%>' ReadOnly="true"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Precio / Descuento" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="#ccffff" Visible="false">
                                                                        <ItemTemplate>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <small>Precio</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_precio_3" Style="width: 100px;" ReadOnly="true" Text='<%# Eval("precio_3")%>'></asp:TextBox>
                                                                                        <small>Precio Unitario</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="precio_unitario_3" Style="width: 100px;" ReadOnly="true" Text='<%# Eval("precio_unitario_3")%>'></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Cantidad</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-center" ID="det_cantidad_3" Style="width: 80px;" ReadOnly="true" Text='<%# Eval("cantidad_3")%>'></asp:TextBox>
                                                                                        <span style="display: none"><%# Eval("cantidad_3")%></span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Descuento (%)</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_descuento_3" Style="width: 100px;" ReadOnly="true" Text='<%# Eval("descuento_3")%>'></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Precio (%)</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_precio_descuento_3" Style="width: 100px;" Text='<%# Eval("precio_con_descuento_3")%>' ReadOnly="true"></asp:TextBox>
                                                                                        <small>Precio Unit.(%)</small><br />
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="precio_unitario_desc_3" Style="width: 100px;" Text='<%# Eval("precio_con_descuento_unitario_3")%>' ReadOnly="true"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Total</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_total_3" Style="width: 100px;" Text='<%# Eval("total_3")%>' ReadOnly="true"></asp:TextBox>
                                                                                        <small>Total + IVA</small><br />
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="total_iva_3" Style="width: 100px;" Text='<%# Eval("total_iva_3")%>' ReadOnly="true"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="unidad_equivale" DataField="unidad_equivale" Visible="false" />
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    No se encontraron resultados.
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group row" id="html_totales">
                                                </div>
                                                <h4 class="form-section"><i class="fa fa-list text-danger"></i>Servicios de la cotización</h4>
                                                <div class="form-group row">
                                                    <div class="col-sm-12">
                                                        <asp:GridView DataKeyNames="" runat="server" ID="G_SERVICIOS" CssClass="table table-bordered table-sm compact" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                            <HeaderStyle CssClass="thead-dark" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Nº">
                                                                    <ItemTemplate>
                                                                        <%# Container.DataItemIndex + 1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="id_cotizacion_servicio" DataField="id_cotizacion_servicio_log" Visible="false" />
                                                                <asp:BoundField HeaderText="id_cotizacion" DataField="id_cotizacion_log" Visible="false" />
                                                                <asp:BoundField HeaderText="Servicio" DataField="nombre_servicio" />
                                                                <asp:BoundField HeaderText="Valor" DataField="valor_servicio" DataFormatString="{0:C0}" />
                                                                <asp:BoundField HeaderText="Descripcion" DataField="tipo_servicio" />

                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                        </div>
                                        <div class="form-actions">
                                            <div class="row">
                                                <div class="col-sm-6 text-left">
                                                    <%--<div onclick="Previsualizar();" class="btn btn-success"><i class="fa fa-eye"></i>&nbsp;Previsualizar Cotización</div>--%>
                                                    <asp:LinkButton ClientIDMode="AutoID" CssClass="btn btn-warning" runat="server" ID="B_VOLVER" OnClick="B_VOLVER_Click"><i class="fa fa-undo"></i>&nbsp;Volver</asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
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
        var table_clientes;

        $(document).ready(function () {
            Datatables();
            chosen();
        });

        function chosen() {
            try {
                $(".chosen-select").chosen({ no_results_text: "No se encuentran registros" });
            }
            catch (ex) {

            }
        }

        function Datatables() {
            relojito(false);
            try {
                $('#<%= G_PRINCIPAL.ClientID %>').DataTable({
                    destroy: true,
                    stateSave: true
                });
            }
            catch (x) {

            }
            try {
                $('#<%= G_DETALLE_COTIZACION.ClientID %>').DataTable({
                    destroy: true,
                    "paging": false
                });
            }
            catch (x) {

            }
        }

        function ImprimeGT(id_cotizacion) {
            //var win = window.open('Cotizacion_pdf.aspx?idctz=' + id_cotizacion, '_blank');
            //win.focus();
        }

        function Previsualizar() {
            //var id_cotizacion = $('#T_ID').val();
            //var win = window.open('Cotizacion_pdf.aspx?idctz=' + id_cotizacion, '_blank');
            //win.focus();
        }
        function calcular_totales(x) {
            relojito(false);
            var parameters = new Object();
            parameters.x = x;
            parameters = JSON.stringify(parameters);
            $.ajax({
                type: "POST",
                url: "COTIZACIONES.aspx/calculartotalajax",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (result) {
                    relojito(false);
                    result = result.d;
                    $('#html_totales').html(result);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    relojito(false);
                    Mensaje("Error al tratar de guardar");
                }
            });
        }
    </script>
</asp:Content>
