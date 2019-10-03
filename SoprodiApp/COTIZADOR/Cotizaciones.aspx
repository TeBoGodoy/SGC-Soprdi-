<%@ Page Title="" Language="C#" MasterPageFile="~/COTIZADOR/Cotizador.Master" AutoEventWireup="true" CodeBehind="Cotizaciones.aspx.cs" Inherits="COTIZADOR.Cotizaciones" %>

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
                        <h4 class="card-title">SOPRODI | Mis Cotizaciones </h4>
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
                                    <asp:AsyncPostBackTrigger ControlID="B_GUARDAR" />
                                    <asp:AsyncPostBackTrigger ControlID="B_VOLVER" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="PANEL_PRINCIPAL">
                                        <asp:LinkButton runat="server" ID="B_NUEVO" OnClick="B_NUEVO_Click" ClientIDMode="AutoID" class="btn btn-outline-danger btn-sm" ToolTip="Nueva Cotización"><i class="fa fa-plus"></i> CREAR NUEVA COTIZACIÓN</asp:LinkButton>
                                        <hr />
                                        <div class="table-responsive">
                                            <asp:GridView DataKeyNames="id_cotizacion" runat="server" ID="G_PRINCIPAL" CssClass="table table-bordered compact table-xs" OnRowCommand="G_PRINCIPAL_RowCommand" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                <HeaderStyle CssClass="thead-dark" />
                                                <Columns>
                                                    <asp:BoundField HeaderText="Id" DataField="id_cotizacion" />
                                                    <asp:BoundField HeaderText="Nombre" DataField="nombre_cotizacion" />
                                                    <asp:BoundField HeaderText="Descripción" DataField="descripcion" />
                                                    <asp:BoundField HeaderText="Fecha Creación" DataField="fecha_creacion" />
                                                    <asp:BoundField HeaderText="Estado" DataField="estado" />
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Opciones" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ClientIDMode="AutoID" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Ver">
                                                                   <i class="icon-pencil"></i>
                                                            </asp:LinkButton>
                                                            &nbsp; &nbsp; &nbsp;   
                                                         <asp:LinkButton ClientIDMode="AutoID" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Borrar" OnClientClick="return confirm('Desea eliminar el registro seleccionado?');">
                                                                   <i class="icon-trash"></i>
                                                         </asp:LinkButton>
                                                            &nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ToolTip="Imprimir PDF" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Print" OnClientClick='<%# String.Format("return ImprimeGT({0});", Eval("id_cotizacion")) %>'>
                                                        <i class="fa fa-print"></i>
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
                                                    <label class="col-md-3 label-control" for="projectinput1">Columnas de Precio</label>
                                                    <div class="col-sm-6">
                                                        <asp:ListBox SelectionMode="Multiple" runat="server" ID="CB_BODEGA" CssClass="chosen-select form-control input-sm"></asp:ListBox><br />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 label-control" for="projectinput1">Nombre de la cotización</label>
                                                    <div class="col-md-9">
                                                        <asp:TextBox runat="server" ID="T_NOMBRE_CTZ" CssClass="form-control input-sm" placeholder="Ej: Cotizacion Abarrotes."></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 label-control" for="projectinput1">Descripcion de la cotización</label>
                                                    <div class="col-md-9">
                                                        <asp:TextBox runat="server" ID="T_DESCRIPCION_CTZ" CssClass="form-control input-sm" placeholder="Ej: Cotización para clientes Zona Sur Santiago."></asp:TextBox>
                                                    </div>
                                                </div>
                                                <h4 class="form-section"><i class="fa fa-th text-danger"></i>Productos de la Cotización</h4>
                                                <div class="form-group row">
                                                    <div class="col-sm-3">
                                                        <span>
                                                            <asp:LinkButton runat="server" ID="B_ABRE_MODAL_PRODUCTOS" OnClick="B_ABRE_MODAL_PRODUCTOS_Click" ClientIDMode="AutoID" class="btn btn-outline-danger btn-sm" ToolTip="Agregar" OnClientClick="relojito(true);"><i class="fa fa-plus"></i>&nbsp;AGREGAR PRODUCTOS A LA COTIZACIÓN</asp:LinkButton></span>
                                                    </div>
                                                    <div class="col-sm-6 text-center">
                                                        <asp:Label runat="server" ID="LBL_FECHA_PRECIO" Font-Size="Large"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <div class="col-md-12" style="overflow-x: auto">

                                                        <div class="table-responsive">
                                                            <button style="display: none" id="btn_abre_modal" type="button" data-toggle="modal" data-target="#modalproducto" name="#focusable"></button>
                                                            <asp:GridView DataKeyNames="id_cotizacion_det, id_cotizacion, producto" runat="server" ID="G_DETALLE_COTIZACION" CssClass="table table-bordered table-xs compact" OnRowCommand="G_DETALLE_COTIZACION_RowCommand" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" Width="100%">
                                                                <HeaderStyle CssClass="thead-dark" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Nº">
                                                                        <ItemTemplate>
                                                                            <%# Container.DataItemIndex + 1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="id_cotizacion" DataField="id_cotizacion" Visible="false" />
                                                                    <asp:BoundField HeaderText="id_cotizacion_det" DataField="id_cotizacion_det" Visible="false" />
                                                                    <asp:BoundField HeaderText="Cod. Producto" DataField="producto" />
                                                                    <asp:BoundField HeaderText="Producto" DataField="nom_producto" />
                                                                    <asp:TemplateField HeaderText="Precio / Descuento" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="#ffffcc">
                                                                        <ItemTemplate>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <small>Precio</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_precio_1" Style="width: 100px;" Text='<%# Eval("precio_1")%>' OnTextChanged="det_precio_1_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                        <small>Precio Unitario</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="precio_unitario_1" Style="width: 100px;" Text='<%# Eval("precio_unitario_1")%>' ReadOnly="true"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Cantidad</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-center" ID="det_cantidad_1" Style="width: 80px;" Text='<%# Eval("cantidad_1")%>' OnTextChanged="det_cantidad_1_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                        <span style="display: none"><%# Eval("cantidad_1")%></span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Descuento (%)</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_descuento_1" Style="width: 100px;" Text='<%# Eval("descuento_1")%>' OnTextChanged="det_descuento_1_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Precio (%)</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_precio_descuento_1" Style="width: 100px;" Text='<%# Eval("precio_con_descuento_1")%>' ReadOnly="true"></asp:TextBox>
                                                                                        <small>Precio Unit.(%)</small><br />
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="precio_unitario_desc_1" Style="width: 100px;" Text='<%# Eval("precio_con_descuento_unitario_1")%>' ReadOnly="true"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Sub Total Neto</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_total_1" Style="width: 100px;" Text='<%# Eval("total_1")%>' ReadOnly="true"></asp:TextBox>
                                                                                        <small>Total c/IVA</small><br />
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
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_precio_2" Style="width: 100px;" Text='<%# Eval("precio_2")%>' OnTextChanged="det_precio_2_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                        <small>Precio Unitario</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="precio_unitario_2" Style="width: 100px;" Text='<%# Eval("precio_unitario_2")%>' ReadOnly="true"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Cantidad</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-center" ID="det_cantidad_2" Style="width: 80px;" Text='<%# Eval("cantidad_2")%>' OnTextChanged="det_cantidad_2_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                        <span style="display: none"><%# Eval("cantidad_2")%></span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Descuento (%)</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_descuento_2" Style="width: 100px;" Text='<%# Eval("descuento_2")%>' OnTextChanged="det_descuento_2_TextChanged" AutoPostBack="true"></asp:TextBox>
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
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_precio_3" Style="width: 100px;" Text='<%# Eval("precio_3")%>' OnTextChanged="det_precio_3_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                        <small>Precio Unitario</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="precio_unitario_3" Style="width: 100px;" Text='<%# Eval("precio_unitario_3")%>' ReadOnly="true"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Cantidad</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-center" ID="det_cantidad_3" Style="width: 80px;" Text='<%# Eval("cantidad_3")%>' OnTextChanged="det_cantidad_3_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                        <span style="display: none"><%# Eval("cantidad_3")%></span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <small>Descuento (%)</small>
                                                                                        <asp:TextBox runat="server" CssClass="form-control input-sm text-right" ID="det_descuento_3" Style="width: 100px;" Text='<%# Eval("descuento_3")%>' OnTextChanged="det_descuento_3_TextChanged" AutoPostBack="true"></asp:TextBox>
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
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Opciones" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Borrar" OnClientClick="return confirm('Desea eliminar el registro seleccionado?');">
                                                                            <i class="icon-trash"></i>
                                                                            </asp:LinkButton>
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
                                                <h4 class="form-section"><i class="fa fa-list text-info"></i>Servicios de la cotización</h4>
                                                <div class="form-group row">
                                                    <div class="col-sm-3">
                                                        <label class="col-md-12 label-control" for="projectinput1">Servicio</label>
                                                        <div class="col-md-12">
                                                            <asp:DropDownList runat="server" ID="CB_SERVICIO" CssClass="selectize-select form-control input-sm"></asp:DropDownList>
                                                        </div>
                                                        <label class="col-md-12 label-control" for="projectinput1">Valor ($)</label>
                                                        <div class="col-md-12">
                                                            <asp:TextBox runat="server" ID="T_PRECIO_SERVICIO" CssClass="form-control input-sm"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <br />
                                                            <asp:RadioButton runat="server" ID="RB_NOSEPARAR" GroupName="service" Text=" Agregar valor a los productos" Checked="true" /><br />
                                                            <asp:RadioButton runat="server" ID="RB_SEPARAR" GroupName="service" Text=" Por Separado" />
                                                        </div>
                                                        <div class="col-md-12">
                                                            <br />
                                                            <asp:LinkButton runat="server" ID="B_AGREGAR_SERVICIO" OnClick="B_AGREGAR_SERVICIO_Click" CssClass="btn btn-outline-info btn-sm"><i class="fa fa-plus"></i>&nbsp;AGREGAR SERVICIO</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-9">
                                                        <asp:GridView DataKeyNames="id_cotizacion_servicio, id_cotizacion, valor_servicio" runat="server" ID="G_SERVICIOS" CssClass="table table-bordered table-sm compact" OnRowCommand="G_SERVICIOS_RowCommand" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                            <HeaderStyle CssClass="thead-dark" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Nº">
                                                                    <ItemTemplate>
                                                                        <%# Container.DataItemIndex + 1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="id_cotizacion_servicio" DataField="id_cotizacion_servicio" Visible="false" />
                                                                <asp:BoundField HeaderText="id_cotizacion" DataField="id_cotizacion" Visible="false" />
                                                                <asp:BoundField HeaderText="Servicio" DataField="nombre_servicio" />
                                                                <asp:BoundField HeaderText="Valor" DataField="valor_servicio" DataFormatString="{0:C0}" />
                                                                <asp:BoundField HeaderText="Descripcion" DataField="tipo_servicio" />
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Opciones" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Borrar" OnClientClick="return confirm('Desea eliminar el registro seleccionado?');">
                                                                            <i class="icon-trash"></i>
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
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
                                                    <div onclick="Previsualizar();" class="btn btn-success"><i class="fa fa-eye"></i>&nbsp;Previsualizar Cotización</div>
                                                    <asp:LinkButton runat="server" ID="B_ABRE_MODAL_CLIENTE" OnClick="B_ABRE_MODAL_CLIENTE_Click" ClientIDMode="AutoID" class="btn btn-success" ToolTip="Enviar Cotización" OnClientClick="relojito(true);"><i class="fa fa-envelope"></i> Enviar Cotización </asp:LinkButton></h4>                                                        
                                                        <button style="display: none" id="btn_abre_modal_clientes" type="button" data-toggle="modal" data-target="#modalclientes"></button>
                                                </div>
                                                <div class="col-sm-6 text-right">
                                                    <asp:LinkButton ClientIDMode="AutoID" CssClass="btn btn-danger" runat="server" ID="B_GUARDAR" OnClick="B_GUARDAR_Click"><i class="fa fa-save"></i>&nbsp;Guardar Cotización</asp:LinkButton>
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

    <!-- MODAL PRODUCTO -->
    <div class="modal animated bounceInLeft text-left" id="modalproducto" tabindex="-1" role="dialog" aria-labelledby="myModalLabel16" aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <div class="modal-header bg-danger">
                    <h4 class="modal-title text-white" id="myModalLabel18"><i class="fa fa-th"></i>&nbsp; Productos</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="UP_PRODUCTOS" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="B_AGREGAR_PRODUCTOS" />
                        </Triggers>
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-sm-6">
                                    <table class="table table-xs compact">
                                        <tr>
                                            <td><b>COLUMNA PRECIO: </b></td>
                                            <td>
                                                <asp:Label runat="server" ID="LB_BODEGA" Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><b>FECHA LISTA PRECIO: </b></td>
                                            <td>
                                                <asp:Label runat="server" ID="LB_FECHA_PRECIOS" Font-Size="Medium" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="col-sm-4">
                                    <asp:DropDownList runat="server" ID="CB_CATEGORIAS" CssClass="selectize-select form-control input-sm"></asp:DropDownList>
                                    <asp:LinkButton runat="server" ID="B_TODOS_CATEGORIA" CssClass="btn btn-sm btn-block btn-danger" OnClick="B_TODOS_CATEGORIA_Click" OnClientClick="AgregarProductosJS();"><i class="fa fa-check"></i>&nbsp;SELECCIONAR TODA LA CATEGORÍA</asp:LinkButton>
                                </div>
                                <div class="col-sm-2 text-right">
                                    <div class="form-group">
                                        <!-- Icon Button group -->
                                        <div class="btn-group" role="group" aria-label="Basic example">
                                            <asp:LinkButton runat="server" ID="B_TODOS_PRODUCTOS" CssClass="btn btn-icon btn-danger" OnClick="B_TODOS_PRODUCTOS_Click"><i class="fa fa-check-square-o"></i></asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="B_NINGUNO_PRODUCTOS" CssClass="btn btn-icon btn-secondary" OnClick="B_NINGUNO_PRODUCTOS_Click"><i class="fa fa-square"></i></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <hr />
                                    <h4 class="text-center"><i class="fa fa-check text-success"></i>Productos con stock</h4>
                                    <hr />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12" style="overflow: auto; height: 65vh;">
                                    <div class="table-responsive">
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
                            <div class="row">
                                <div class="col-sm-12">
                                    <hr />
                                    <h4 class="text-center"><i class="fa fa-close text-danger"></i>Productos sin stock</h4>
                                    <hr />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12" style="overflow: auto; height: 65vh;">
                                    <div class="table-responsive">
                                        <asp:GridView DataKeyNames="cod_producto, producto, id_categoria, precioventa1, precioventa2, precioventa3, preciounitario1, preciounitario2, preciounitario3, unidad_equivale" runat="server" ID="G_PRODUCTOS2" CssClass="table table-xs hide-columns-dynamically table-bordered compact" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton runat="server" ID="B_AGREGAR_PRODUCTOS" CssClass="btn btn-icon btn-danger" OnClick="B_AGREGAR_PRODUCTOS_Click" OnClientClick="AgregarProductosJS();"><i class="fa fa-save"></i>&nbsp;Agregar a la cotización</asp:LinkButton>
                    <button type="button" class="btn grey btn-outline-secondary" data-dismiss="modal" id="cerrar_productos">Volver</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL CLIENTES -->
    <div class="modal animated bounceInLeft text-left" id="modalclientes" tabindex="-1" role="dialog" aria-labelledby="myModalLabel16" aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <asp:UpdatePanel runat="server" ID="UP_CLIENTES" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="B_ENVIAR_CORREO" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="modal-header bg-success">
                            <h4 class="modal-title text-white" id="asdasdas"><i class="fa fa-users"></i>Clientes</h4>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <h4><strong>Seleccione</strong> los clientes a los cuales enviará la cotización. </h4>
                            <div class="row">
                                <div class="col-sm-6">
                                    <ul class="nav nav-tabs nav-top-border no-hover-bg">
                                        <li class="nav-item"><a id="base-tab1" class="nav-link active" aria-controls="tab1" href="#tab1" aria-expanded="true" data-toggle="tab"><i class="fa fa-home"></i>CON VENTA</a></li>
                                        <li class="nav-item"><a id="base-tab2" class="nav-link" aria-controls="tab2" href="#tab2" aria-expanded="false" data-toggle="tab"><i class="fa fa-user"></i>SIN VENTA</a></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="tab-content px-1 pt-1">
                                <div role="tabpanel" class="tab-pane active" id="tab1" aria-expanded="true" aria-labelledby="base-tab1">
                                    <asp:UpdatePanel runat="server" ID="UP_CLIENTESVENTA" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="G_CLIENTES" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                </div>
                                                <div class="col-sm-6 text-right">
                                                    <div class="form-group">
                                                        <!-- Icon Button group -->
                                                        <div class="btn-group" role="group" aria-label="Basic example">
                                                            <asp:LinkButton runat="server" ID="B_MARCAR_TODOS_CLIENTES" CssClass="btn btn-icon btn-success" OnClick="B_MARCAR_TODOS_CLIENTES_Click"><i class="fa fa-check-square-o"></i></asp:LinkButton>
                                                            <asp:LinkButton runat="server" ID="B_DESMARCAR_TODOS_CLIENTES" CssClass="btn btn-icon btn-secondary" OnClick="B_DESMARCAR_TODOS_CLIENTES_Click"><i class="fa fa-square"></i></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="table-responsive" style="overflow-x: auto;">
                                                <button style="display: none" id="btn_abre_modal_contactos" type="button" data-toggle="modal" data-target="#modalcontactos" name="#focusable"></button>
                                                <asp:GridView DataKeyNames="rutcliente, correo, Nombre Cliente, ciudad, direccion, giro" runat="server" ID="G_CLIENTES" OnRowCommand="G_CLIENTES_RowCommand" CssClass="table compact table-bordered table-xs" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                    <HeaderStyle CssClass="thead-dark" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Seleccione" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="CHK_ENVIAR" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Rut" DataField="rutcliente" Visible="false" />
                                                        <asp:BoundField ItemStyle-Wrap="false" HeaderText="Nombre Vendedor" DataField="Nombre Vendedor" Visible="false" />
                                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Nombre Cliente">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ClientIDMode="AutoID" runat="server" OnClientClick="relojito(true);" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="EditarCliente">
                                                                   <i class="success fa fa-edit"></i>
                                                                </asp:LinkButton><span>&nbsp;<%# Eval("Nombre Cliente")%></span>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField ItemStyle-Wrap="false" HeaderText="Nombre Cliente" DataField="Nombre Cliente" Visible="false" />
                                                        <asp:BoundField ItemStyle-Wrap="false" HeaderText="Correo" DataField="correo" />
                                                        <asp:BoundField ItemStyle-Wrap="false" HeaderText="Comuna" DataField="ciudad" />
                                                        <asp:BoundField ItemStyle-Wrap="false" HeaderText="Ult. Factur." DataField="Periodo Ultima Factur." />
                                                        <asp:BoundField ItemStyle-Wrap="false" HeaderText="Monto Ult. Fact." DataField="Monto Ultima Factur." DataFormatString="{0:C0}" />
                                                        <asp:BoundField ItemStyle-Wrap="false" HeaderText="Dias Dif." DataField="Dias Diferencia" />
                                                        <asp:BoundField ItemStyle-Wrap="false" HeaderText="LC" DataField="LC" DataFormatString="{0:C0}" />
                                                        <asp:BoundField ItemStyle-Wrap="false" HeaderText="Disponible" DataField="disponible" DataFormatString="{0:C0}" />
                                                        <asp:BoundField ItemStyle-Wrap="false" HeaderText="direccion" DataField="direccion" Visible="false" />
                                                        <asp:BoundField ItemStyle-Wrap="false" HeaderText="Giro" DataField="Giro" Visible="false" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        No se encontraron resultados.
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="tab-pane" id="tab2" aria-labelledby="base-tab2">
                                    <asp:UpdatePanel runat="server" ID="UP_CLIENTESSINVENTAS" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="G_CLIENTES2" />
                                            <asp:AsyncPostBackTrigger ControlID="B_CARGAR_SIN_VENTAS" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <div id="DIV_CARGAR_SIN_VENTA" runat="server">
                                                        <asp:LinkButton runat="server" ID="B_CARGAR_SIN_VENTAS" OnClientClick="relojito(true);" OnClick="B_CARGAR_SIN_VENTAS_Click" CssClass="btn btn-success">Cargar Clientes sin Venta</asp:LinkButton>
                                                        <br />
                                                        <small>&nbsp;&nbsp;<i class="fa fa-info-circle"></i>&nbsp;Puede demorar en cargar*</small>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 text-right">
                                                    <div class="form-group">
                                                        <!-- Icon Button group -->
                                                        <div class="btn-group" role="group" aria-label="Basic example">
                                                            <asp:LinkButton runat="server" ID="B_MARCAR_TODOS_CLIENTES2" CssClass="btn btn-icon btn-success" OnClick="B_MARCAR_TODOS_CLIENTES2_Click"><i class="fa fa-check-square-o"></i></asp:LinkButton>
                                                            <asp:LinkButton runat="server" ID="B_DESMARCAR_TODOS_CLIENTES2" CssClass="btn btn-icon btn-secondary" OnClick="B_DESMARCAR_TODOS_CLIENTES2_Click"><i class="fa fa-square"></i></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="table-responsive" style="overflow-x: auto;">
                                                <asp:GridView DataKeyNames="rutcliente, correo, Name, billcity, addr1" runat="server" ID="G_CLIENTES2" OnRowCommand="G_CLIENTES2_RowCommand" CssClass="table compact table-bordered table-xs" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                    <HeaderStyle CssClass="thead-dark" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Seleccione" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="CHK_ENVIAR" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Rut" DataField="rutcliente" Visible="false" />
                                                        <asp:BoundField ItemStyle-Wrap="false" HeaderText="Nombre Vendedor" DataField="SlsperId" Visible="false" />
                                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Nombre Cliente">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ClientIDMode="AutoID" runat="server" OnClientClick="relojito(true);" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="EditarCliente">
                                                                   <i class="success fa fa-users"></i>
                                                                </asp:LinkButton><span>&nbsp;<%# Eval("Name")%></span>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField ItemStyle-Wrap="false" HeaderText="Nombre Cliente" DataField="Name" Visible="false" />
                                                        <asp:BoundField ItemStyle-Wrap="false" HeaderText="Correo" DataField="correo" />
                                                        <asp:BoundField ItemStyle-Wrap="false" HeaderText="Comuna" DataField="billcity" />
                                                        <asp:BoundField ItemStyle-Wrap="false" HeaderText="LC" DataField="crlmt" DataFormatString="{0:C0}" />
                                                        <asp:BoundField ItemStyle-Wrap="false" HeaderText="direccion" DataField="addr1" Visible="false" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        No se encontraron resultados.
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form form-horizontal">
                                        <div class="form-body">
                                            <h4 class="form-section"><i class="fa fa-edit text-success"></i>Opciones para el Correo</h4>
                                            <div class="form-group row">
                                                <label class="col-md-3 label-control" for="projectinput1"><b>Clientes no registrados</b></label>
                                                <div class="col-sm-6">
                                                    <small>Nombre Cliente</small>
                                                    <asp:TextBox runat="server" ID="T_NOREGISTRADOS_NOMBRE" CssClass="form-control input-sm"></asp:TextBox><br />
                                                    <small>Correo Cliente</small>
                                                    <asp:TextBox runat="server" ID="T_NOREGISTRADOS_CORREO" CssClass="form-control input-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                            <hr />
                                            <div class="form-group row">
                                                <label class="col-md-3 label-control" for="projectinput1"><b>Opciones del Correo</b></label>
                                                <div class="col-sm-6">
                                                    <small>Asunto</small>
                                                    <asp:TextBox runat="server" ID="T_ASUNTO_CORREO" CssClass="form-control input-sm" Text="Cotización SOPRODI: "></asp:TextBox><br />
                                                    <small>CC</small>
                                                    <asp:TextBox runat="server" ID="T_CC_CORREO" CssClass="form-control input-sm"></asp:TextBox><br />
                                                    <small>Comentario</small>
                                                    <asp:TextBox runat="server" ID="T_COMENTARIO_CORREO" Columns="3" TextMode="MultiLine" CssClass="form-control input-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <br />

                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ClientIDMode="AutoID" CssClass="btn btn-success mr-1" runat="server" ID="B_ENVIAR_CORREO" OnClick="B_ENVIAR_CORREO_Click"><i class="fa fa-envelope"></i>&nbsp;Enviar</asp:LinkButton>
                            <button type="button" class="btn grey btn-outline-secondary" data-dismiss="modal" id="cerrar_correo">Volver</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!-- MODAL CONTACTOS DEL CLIENTE -->
    <div class="modal animated bounceInLeft text-left" id="modalcontactos" tabindex="-1" role="dialog" aria-labelledby="myModalLabel16" aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <div class="modal-header bg-warning">
                    <h4 class="modal-title text-white"><i class="fa fa-users"></i>&nbsp; Contactos del Cliente</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UP_CONTACTOS" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="B_CREAR_NUEVO_CONTACTO" />
                            <asp:AsyncPostBackTrigger ControlID="G_CONTACTOS" />
                            <asp:AsyncPostBackTrigger ControlID="B_GUARDAR_CONTACTO" />
                        </Triggers>
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div runat="server" id="MODA_CONTACTO_NOMBRE_CLIENTE"></div>
                                    <asp:LinkButton runat="server" ID="B_CREAR_NUEVO_CONTACTO" CssClass="btn btn-warning btn-sm" OnClick="B_CREAR_NUEVO_CONTACTO_Click"><i class="fa fa-plus"></i> CREAR NUEVO CONTACTO</asp:LinkButton>
                                    <hr />
                                    <div class="form form-horizontal">
                                        <div class="form-body">
                                            <div id="DIV_DETALLE_CONTACTO" runat="server" visible="false">
                                                <div class="form-group row">
                                                    <div class="col-sm-6">
                                                        <asp:Label runat="server" ID="LBL_NOMBRE_CONTACTO" Font-Size="Large" CssClass="text-warning"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <div style="display: none">
                                                        <asp:TextBox runat="server" ID="viene_de"></asp:TextBox>
                                                        <asp:TextBox runat="server" ID="t_rut_cliente_contact"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <small>NOMBRE CLIENTE</small>
                                                        <asp:TextBox runat="server" ID="t_nombre_contact" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <small>CARGO</small>
                                                        <asp:TextBox runat="server" ID="t_cargo_contact" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <div class="col-sm-4">
                                                        <small>NÚMERO</small>
                                                        <asp:TextBox runat="server" ID="t_num_contact" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <small>CORREO</small>
                                                        <asp:TextBox runat="server" ID="t_correo_contact" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <small>COMUNA</small>
                                                        <asp:TextBox runat="server" ID="t_direcc_contact" CssClass="form-control input-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <div class="col-sm-6">
                                                        <asp:LinkButton runat="server" ID="B_GUARDAR_CONTACTO" CssClass="btn btn-warning btn-sm" OnClick="B_GUARDAR_CONTACTO_Click"><i class="fa fa-save"></i>&nbsp; GUARDAR CONTACTO</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="table-responsive" style="overflow-x: auto;">
                                        <asp:GridView ID="G_CONTACTOS" AutoGenerateColumns="false" CssClass="table compact table-bordered table-xs" runat="server"
                                            OnRowCommand="G_CONTACTOS_RowCommand" DataKeyNames="rutcliente, nombre_contacto, CORREO" ShowHeaderWhenEmpty="True">
                                            <HeaderStyle CssClass="thead-dark" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="B_EDITAR" CssClass="" CommandName="Editar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"><i class="fa fa-edit fa-2x text-warning"></i></asp:LinkButton>
                                                        &nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton runat="server" ID="B_BORRAR" CssClass="" CommandName="Eliminar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClientClick='<%# confirmDelete2( Eval("nombre_contacto").ToString() ) %>'><i class="fa fa-close fa-2x text-warning"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="nombre_contacto" HeaderText="NOMBRE"></asp:BoundField>
                                                <asp:BoundField DataField="cargo" HeaderText="CARGO"></asp:BoundField>
                                                <asp:BoundField DataField="NUMERO" HeaderText="NÚMERO"></asp:BoundField>
                                                <asp:BoundField DataField="CORREO" HeaderText="CORREO"></asp:BoundField>
                                                <asp:BoundField DataField="DIRECCION" HeaderText="COMUNA"></asp:BoundField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                No existen datos.
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn grey btn-outline-secondary" data-dismiss="modal" id="cerrar_contactos">Volver</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CODIGO_JAVASCRIPT" runat="server">
    <script>

        var table_productos;
        var table_productos2;
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

        function focusonme(x) {
            document.getElementById(x).focus();
        }

        function abremodalproductos() {
            $('#btn_abre_modal').click();
            relojito(false);
        }
        function cerrarmodalproductos() {
            $('#cerrar_productos').click();
            relojito(false);
        }
        function abremodalclientes() {
            $('#btn_abre_modal_clientes').click();
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

            try {
                table_clientes = $('#<%= G_CLIENTES.ClientID %>').DataTable({
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    },
                    "order": [[0, "asc"]],
                    destroy: true,
                    "paging": false
                });
                filtros_por_columna('<%= G_CLIENTES.ClientID %>', table_clientes);
            }
            catch (x) {

            }
            try {
                table_clientes = $('#<%= G_CLIENTES2.ClientID %>').DataTable({
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    },
                    "order": [[0, "asc"]],
                    destroy: true,
                    "paging": false
                });
                filtros_por_columna('<%= G_CLIENTES2.ClientID %>', table_clientes);
            }
            catch (x) {

            }
            try {
                table_productos2 = $('#<%= G_PRODUCTOS2.ClientID %>').DataTable({
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    },
                    "order": [[1, "asc"]],
                    destroy: true,
                    "paging": false

                });
                filtros_por_columna('<%= G_PRODUCTOS2.ClientID %>', table_productos2);
            }
            catch (x) {

            }
        }

        function ImprimeGT(id_cotizacion) {
            var win = window.open('Cotizacion_pdf.aspx?idctz=' + id_cotizacion, '_blank');
            win.focus();
        }

        function Previsualizar() {
            var id_cotizacion = $('#T_ID').val();
            var win = window.open('Cotizacion_pdf.aspx?idctz=' + id_cotizacion, '_blank');
            win.focus();
        }

        function AgregarProductosJS() {
            table_productos.destroy();
            table_productos2.destroy();
            relojito(true);
        }

        function EnviarCorreoClientes() {
            table_clientes.destroy();
            var id_cotizacion = $('#T_ID').val();
            window.open('Enviar_cotizacion.aspx?idctz=' + id_cotizacion + '&cr=SI', '_blank');
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

        function ABREMODALEDITARCLIENTES() {
            $('#btn_abre_modal_contactos').click();
            relojito(false);
        }


    </script>
</asp:Content>
