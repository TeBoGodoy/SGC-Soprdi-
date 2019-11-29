<%@ Page Title="" Language="C#" MasterPageFile="~/COTIZADOR/Cotizador2.Master" AutoEventWireup="true" CodeBehind="DashboardAdmin.aspx.cs" Inherits="SoprodiApp.COTIZADOR.DashboardAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CONTENIDO_BODY" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="b_llenar_clte_cotizados" />
            <asp:AsyncPostBackTrigger ControlID="b_llenar_clte_cotizados2" />
        </Triggers>
        <ContentTemplate>
            <section class="horizontal-grid" id="horizontal-grid">
                <div class="row">
                    <div class="col-md-12">
                        <div class="card">
                            <div class="card-content collapse show">
                                <div class="card-body">
                                    <asp:UpdatePanel runat="server" ID="PRINCIPAL" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="B_FILTRAR" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <div class="form-group row">
                                                <div class="col-sm-3">
                                                    <small>Fecha Desde</small>
                                                    <asp:TextBox runat="server" ID="T_FECHA_DESDE" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-3">
                                                    <small>Fecha Hasta</small>
                                                    <asp:TextBox runat="server" ID="T_FECHA_HASTA" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-3">
                                                    <br />
                                                    <asp:LinkButton runat="server" ID="B_FILTRAR" CssClass="btn btn-primary" OnClick="B_FILTRAR_Click">GENERAR</asp:LinkButton>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div runat="server" id="DIV_VISIBLE" visible="false">
                    <%-- <div class="row">
                        <!-- Simple Pie Chart -->
                        <div class="col-xl-12 col-lg-12">
                            <div class="card">
                                <div style="display: none">
                                    <asp:TextBox runat="server" ID="T_GRAF1_CATS"></asp:TextBox>
                                    <asp:TextBox runat="server" ID="T_GRAF1_DATA"></asp:TextBox>                                
                                </div>
                                <div class="card-content collapse show">
                                    <div class="card-body">
                                        <div id="basic-pie" class="height-400 echart-container"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>--%>
                    <div class="row">
                        <div class="col-xl-4 col-lg-4 col-md-4">
                            <div class="card">
                                <div class="card-content">
                                    <div class="card-body text-center">
                                        <div class="card-header mb-2">
                                            <span class="success darken-1"><b>Total Productos</b></span>
                                            <h3 class="font-large-2 grey darken-1 text-bold-200" id="div_total_productos" runat="server"></h3>
                                        </div>
                                        <div class="card-content">
                                            <div runat="server" id="GRAFICO_PRODUCTOS_COTIZADOS"></div>
                                            <ul class="list-inline clearfix mt-2 mb-0">
                                                <li class="border-right-grey border-right-lighten-2 pr-2">
                                                    <h2 class="grey darken-1 text-bold-400" id="div_prod_cotizado" runat="server"></h2>
                                                    <span>Productos Cotizados</span>
                                                </li>
                                                <li class="pl-2">
                                                    <h2 class="grey darken-1 text-bold-400" id="div_prod_nocotizado" runat="server"></h2>
                                                    <span class="danger">No Cotizados</span>
                                                </li>
                                            </ul>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <asp:UpdatePanel runat="server" ID="up_vend_cot" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="B_LLENARDETALLECOTIZACIONES" />
                                </Triggers>
                                <ContentTemplate>
                                    <div style="display: none">
                                        <asp:TextBox runat="server" ID="T_COD_VENDEDOR" ClientIDMode="Static"></asp:TextBox>
                                        <asp:TextBox runat="server" ID="T_NOMBRE_VENDEDOR" ClientIDMode="Static"></asp:TextBox>
                                        <asp:LinkButton runat="server" ID="B_LLENARDETALLECOTIZACIONES" OnClick="B_LLENARDETALLECOTIZACIONES_Click" ClientIDMode="Static"></asp:LinkButton>
                                        <button style="display: none" id="btn_abre_modal" type="button" data-toggle="modal" data-target="#modaldetallecotizaciones" name="#focusable"></button>
                                    </div>
                                    <div class="card">
                                        <div class="card-content collapse show">
                                            <div class="card-body">
                                                <h6 class="success darken-1 text-center"><i class="fa fa-envelope"></i>&nbsp;Cotizaciones Enviadas</h6>
                                                <br />
                                                <div class="table-responsive">
                                                    <table class="table table-hover mb-0  table-xs">
                                                        <thead>
                                                            <tr>
                                                                <th></th>
                                                                <th>Vendedor</th>
                                                                <th>Nº Enviadas</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="div_vend_cotiza" runat="server">
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-xl-4 col-lg-4 col-md-4">
                            <div class="card">
                                <div class="card-content">
                                    <div class="card-body text-center">
                                        <div class="card-header mb-2">
                                            <span class="success darken-1"><b>Total Clientes</b></span>
                                            <h3 class="font-large-2 grey darken-1 text-bold-200" id="div_total_cotizados" runat="server"></h3>
                                        </div>
                                        <div class="card-content">
                                            <div runat="server" id="GRAFICO_CLIENTES_COTIZADOS"></div>
                                            <ul class="list-inline clearfix mt-2 mb-0">
                                                <li class="border-right-grey border-right-lighten-2 pr-2">
                                                    <a onclick="Vercotizados();">
                                                        <h2 class="grey darken-1 text-bold-400" id="div_per_cotizado" runat="server"></h2>
                                                        <span><a onclick="Vercotizados();" class="success">Clientes Cotizados</span>
                                                    </a>
                                                </li>
                                                <li class="pl-2">
                                                    <h2 class="grey darken-1 text-bold-400" id="div_per_nocotizado" runat="server"></h2>
                                                    <span><a onclick="Vercotizados2();" class="danger">No Cotizados</span>
                                                </li>
                                            </ul>
                                        </div>
                                        <div style="display: none">
                                            <asp:LinkButton runat="server" ID="b_llenar_clte_cotizados" OnClick="b_llenar_clte_cotizados_Click" ClientIDMode="Static"></asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="b_llenar_clte_cotizados2" OnClick="b_llenar_clte_cotizados2_Click" ClientIDMode="Static"></asp:LinkButton>
                                            <button style="display: none" id="btn_abre_modal_cltes" type="button" data-toggle="modal" data-target="#modalclientescotizados" name="#focusable"></button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <asp:UpdatePanel runat="server" ID="up_productos" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="G_PRODUCTOS_COTIZADOS" />
                                    <asp:AsyncPostBackTrigger ControlID="B_LLENARDETALLECOTIZACIONES" />
                                </Triggers>
                                <ContentTemplate>
                                    <div class="card">
                                        <div class="card-content collapse show">
                                            <div class="card-body">
                                                <h6 class="success darken-1 text-center"><i class="fa fa-star"></i>&nbsp;Productos Cotizados</h6>
                                                <br />
                                                <div class="table-responsive">
                                                    <asp:GridView runat="server" ID="G_PRODUCTOS_COTIZADOS" DataKeyNames="Producto, nom_producto" OnRowCommand="G_PRODUCTOS_COTIZADOS_RowCommand" CssClass="table table-bordered compact table-xs" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Producto" DataField="Producto" Visible="false" />
                                                            <asp:BoundField HeaderText="Categoria" DataField="nom_categ" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                                            <asp:BoundField HeaderText="Producto" DataField="nom_producto" />
                                                            <asp:BoundField HeaderText="Veces Cotizado" DataField="mycount" />
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ToolTip="Ver Cotizaciones" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Print">
                                                                        <i class="fa fa-eye fa-2x"></i>
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            No se encontraron resultados.
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>


    <!-- MODALES -->
    <div class="modal fade text-left" id="modaldetallecotizaciones" tabindex="-1" role="dialog" aria-labelledby="myModalLabel16" aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel18"><b>Cotizaciones Enviadas</b></h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="up_modal_detcotizacion" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="G_DET_COTIZACIONES" />
                        </Triggers>
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:Label runat="server" ID="LBL_CTZDET_VENDEDOR" Font-Size="Large" CssClass="success"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <hr />
                                    <div class="table-responsive">
                                        <asp:GridView DataKeyNames="id_cotizacion_log, fecha_envio, correo_cliente, nombre cliente" runat="server" ID="G_DET_COTIZACIONES" CssClass="table table-bordered compact table-xs" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                            <HeaderStyle CssClass="thead-dark" />
                                            <Columns>
                                                <asp:BoundField HeaderText="Id" DataField="id_cotizacion_log" />
                                                <asp:BoundField HeaderText="Fecha Envío" DataField="fecha_envio" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                                <asp:BoundField HeaderText="Vendedor" DataField="nombre_usuario" />
                                                <asp:BoundField HeaderText="Nombre" DataField="nombre_cotizacion" />
                                                <asp:BoundField HeaderText="Cliente" DataField="nombre cliente" />
                                                <asp:BoundField HeaderText="Correo" DataField="correo_cliente" />
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ToolTip="VER PDF" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Print" OnClientClick='<%# String.Format("return ImprimeGT({0});", Eval("id_cotizacion_log")) %>'>
                                                        <i class="fa fa-file-pdf-o fa-2x"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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
                    <button type="button" class="btn grey btn-outline-secondary" data-dismiss="modal" id="cerrar_productos">Volver</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade text-left" id="modalclientescotizados" tabindex="-1" role="dialog" aria-labelledby="myModalLabel16" aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <asp:UpdatePanel runat="server" ID="up_modal_clte_cotizados" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="G_DET_CLIENTESCOTIZADOS" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="modal-header">
                            <div id="TITULO_COTIZADO_NOCOTIZADO" runat="server"></div>
                            <%--<h4 class="modal-title" id="myModalLabel18"><b></b></h4>--%>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">

                            <div class="row">
                                <div class="col-sm-12">
                                    <hr />
                                    <div class="table-responsive">
                                        <asp:GridView DataKeyNames="" runat="server" ID="G_DET_CLIENTESCOTIZADOS" CssClass="table table-bordered compact table-xs" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                            <HeaderStyle CssClass="thead-dark" />
                                            <Columns>
                                                <asp:BoundField HeaderText="Nº Cotizaciones" DataField="contador" />
                                                <asp:BoundField HeaderText="Rut" DataField="rut_cliente" />
                                                <asp:BoundField HeaderText="Nombre" DataField="Nombre Cliente" />
                                                <asp:BoundField HeaderText="Vendedor" DataField="nombre_usuario" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                No se encontraron resultados.
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn grey btn-outline-secondary" data-dismiss="modal" id="cerrar_productos2">Volver</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CODIGO_JAVASCRIPT" runat="server">
    <script>
        $(document).ready(function () {
            cargarknob();
            Datatables();
        });

        function Datatables() {
            relojito(false);
            try {
                $('#<%= G_DET_COTIZACIONES.ClientID %>').DataTable({
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    },
                    "order": [[0, "desc"]],
                    destroy: true
                });
            }
            catch (x) {

            }
            try {
                $('#<%= G_PRODUCTOS_COTIZADOS.ClientID %>').DataTable({
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    },
                    "order": [[2, "desc"]],
                    destroy: true
                });
            }
            catch (x) {

            }

            try {
                $('#<%= G_DET_CLIENTESCOTIZADOS.ClientID %>').DataTable({
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    },
                    "order": [[0, "desc"]],
                    destroy: true
                });
            }
            catch (x) {

            }
        }

        function vercotizacionesvendedor(cod_vendedor, nombrevendedor) {
            document.getElementById('<%= T_COD_VENDEDOR.ClientID %>').value = cod_vendedor;
            document.getElementById('<%= T_NOMBRE_VENDEDOR.ClientID %>').value = nombrevendedor;
            document.getElementById('<%= B_LLENARDETALLECOTIZACIONES.ClientID %>').click();
        }

        function abremodaldetcotizaciones() {
            $('#btn_abre_modal').click();
            relojito(false);
        }

        function ImprimeGT(id_cotizacion) {
            var win = window.open('Cotizacion_log_pdf.aspx?idctz=' + id_cotizacion, '_blank');
            win.focus();
        }

        function Vercotizados() {
            document.getElementById('<%= b_llenar_clte_cotizados.ClientID %>').click();
        }

        function Vercotizados2() {
            document.getElementById('<%= b_llenar_clte_cotizados2.ClientID %>').click();
        }

        function abremodaldetclientes() {
            $('#btn_abre_modal_cltes').click();
            relojito(false);
        }

        function cargarknob() {
            try {
                var rtl = false;
                if ($('html').data('textdirection') == 'rtl')
                    rtl = true;

                $(".knob").knob({
                    rtl: rtl,
                    change: function (value) {
                        //console.log("change : " + value);
                    },
                    release: function (value) {
                        //console.log(this.$.attr('value'));
                        // console.log("release : " + value);
                    },
                    cancel: function () {
                        // console.log("cancel : ", this);
                    },
                    /*format : function (value) {
                     return value + '%';
                     },*/
                    draw: function () {

                        // "tron" case
                        if (this.$.data('skin') == 'tron') {

                            this.cursorExt = 0.3;

                            var a = this.arc(this.cv), // Arc
                                pa, // Previous arc
                                r = 1;

                            this.g.lineWidth = this.lineWidth;

                            if (this.o.displayPrevious) {
                                pa = this.arc(this.v);
                                this.g.beginPath();
                                this.g.strokeStyle = this.pColor;
                                this.g.arc(this.xy, this.xy, this.radius - this.lineWidth, pa.s, pa.e, pa.d);
                                this.g.stroke();
                            }

                            this.g.beginPath();
                            this.g.strokeStyle = r ? this.o.fgColor : this.fgColor;
                            this.g.arc(this.xy, this.xy, this.radius - this.lineWidth, a.s, a.e, a.d);
                            this.g.stroke();

                            this.g.lineWidth = 2;
                            this.g.beginPath();
                            this.g.strokeStyle = this.o.fgColor;
                            this.g.arc(this.xy, this.xy, this.radius - this.lineWidth + 1 + this.lineWidth * 2 / 3, 0, 2 * Math.PI, false);
                            this.g.stroke();

                            return false;
                        }
                    }
                });
            }
            catch (ex) {

            }
        }



    </script>
</asp:Content>

