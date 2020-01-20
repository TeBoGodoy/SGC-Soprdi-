<%@ Page Title="" ViewStateMode="Enabled" Language="C#" MasterPageFile="~/Base.Master" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" AutoEventWireup="True" UICulture="es-ES" Culture="es-ES" CodeBehind="REPORTE_SP_DIARIAS.aspx.cs" Inherits="SoprodiApp.REPORTE_SP_DIARIAS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ OutputCache Location="None" NoStore="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">


    <%--INCLUIR JQUERY !!--%>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>

    <script>

        $(document).ready(function () {
            creagrilla();
        });

        function abremodal_(id_categ, nom_categ) {

            document.getElementById("<%=tx_id_categ.ClientID %>").value = id_categ;
            document.getElementById("<%=nombre_titu.ClientID %>").value = nom_categ;

            producto_cb(id_categ);

            tabla_productos_3(id_categ);


            chosen_update();



            $('#titulo_modal').html("<h3 style='color: cornflowerblue;font-size: -webkit-xxx-large;'>" + nom_categ + "</h3>");

            //chosen_update();

        }


        function carga_producs() {

            var id = document.getElementById("<%=tx_id_categ.ClientID %>").value;
            //alert(id);
            tabla_productos_3(id);



        }

        function creagrilla() {

            try {
                var fecha1 = $('th:contains("ENTREGA")').index();
                var fecha2 = $('th:contains("FECHAEMISION")').index();

                $("#G_SP_DIARIA").DataTable({
                    "order": [[0, "asc"]],
                    "lengthChange": false,
                    "searching": false,
                    "destroy": true,
                    "stateSave": true,
                    "pageLength": -1,
                    "paging": false,
                    columnDefs: [
                        { type: 'date-uk', targets: [fecha1, fecha2] }
                    ],
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    }
                });

            }
            catch (e) {
                //alert(e.message);
                console.log(e.message);
            }

        }

        function producto_cb(categ) {

            var parameters = new Object();

            parameters.sw = categ;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_CATEGORIA.aspx/PRODUCTO_CB",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadProduc_kg,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al cargar bodegas");
                }
            });

        }

        function tabla_productos_3(categ_) {

            var parameters = new Object();

            parameters.categ = categ_;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_CATEGORIA.aspx/CargarProductos",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (XMLHttpRequest.status == 401) {
                        alert("Fin de la session");
                        location.href = "Acceso.aspx";
                    } else {
                        alert("Error al cargar evento");
                    }
                    modal_unidad();
                }
            }).done(function (result) {

                if (result.d == "") {

                    $('#tabla_productos').html("<h4>No hay productos en esta categoría</h4>");

                }

                $.each(result.d, function () {

                    $('#tabla_productos').html(this.tabla_html);

                });

                modal_unidad();
            });
        }



        function fuera(fecha, vendedor, grupos, bit) {

            var urlPdf = "/DETALLE_FACTURA.aspx?";
       
            var param = "F=" + fecha + "&V=" + vendedor + "&G=" + grupos + "&i=" + bit;
            var urlPdf_Final = urlPdf + param;

            if (navigator.appName.indexOf('Microsoft Internet Explorer') != -1) {
                window.showModelessDialog(urlPdf_Final, '', 'dialogTop:50px;dialogLeft:50px;dialogHeight:700px;dialogWidth:1200px');
            };

            if (navigator.appName.indexOf('Netscape') != -1) {
                window.open(urlPdf_Final, '', 'width=1200,height=700,left=50,top=50');
                void 0;
            };
            return false;
        }


        function LoadProduc_kg(result) {

            //quito los options que pudiera tener previamente el combo

            $("#<%=cb_productos_kg.ClientID%>").html("");


            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {

                $("#<%=cb_productos_kg.ClientID%>").append($("<option></option>").attr("value", this.producto).text(this.descproducto))

            });


            $("#<%=cb_productos_kg.ClientID%>").chosen();
            $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");



        }

        function load_chosen_combos() {
            $("#<%=d_producto.ClientID%>").chosen();
            $("#<%=d_producto.ClientID%>").trigger("chosen:updated");

            $("#<%=d_cliente.ClientID%>").chosen();
            $("#<%=d_cliente.ClientID%>").trigger("chosen:updated");

            $("#<%=d_vendedor.ClientID%>").chosen();
            $("#<%=d_vendedor.ClientID%>").trigger("chosen:updated");

            $("#<%=d_estado_sp.ClientID%>").chosen();
            $("#<%=d_estado_sp.ClientID%>").trigger("chosen:updated");

            $("#ContentPlaceHolder_Contenido_d_estado_sp_chosen").css({ "width": "400px" });
            $("#ContentPlaceHolder_Contenido_d_vendedor_chosen").css({ "width": "400px" });
            $("#ContentPlaceHolder_Contenido_d_cliente_chosen").css({ "width": "400px" });
            $("#ContentPlaceHolder_Contenido_d_producto_chosen").css({ "width": "400px" });
        }


        function CARGANDO() {

            var elem3 = document.getElementById("<%=cargando_gif.ClientID%>");
            elem3.style.display = "block";

        }

        function incluir_producto() {


            var parameters = new Object();

            parameters.id_cat = document.getElementById("<%=tx_id_categ.ClientID%>").value;;
            parameters.id_prod = document.getElementById("<%=producto_modal.ClientID%>").value;;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_CATEGORIA.aspx/INSERT_PRODUCTO",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: guarda,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al cargar bodegas");
                }
            });

        }



        function guarda(resp) {
            resp = resp.d;
            if (resp == "ok") {

                chosen_update();
                var id_catg_aux = document.getElementById("<%=tx_id_categ.ClientID%>").value;

                producto_cb(id_catg_aux);
                var parameters = new Object();

                parameters.categ = id_catg_aux;

                parameters = JSON.stringify(parameters);

                $.ajax({
                    type: "POST",
                    url: "REPORTE_CATEGORIA.aspx/CargarProductos",
                    data: parameters,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        if (XMLHttpRequest.status == 401) {
                            alert("Fin de la session");
                            location.href = "Acceso.aspx";
                        } else {
                            alert("Error al cargar evento");
                        }

                    }
                }).done(function (result) {

                    if (result.d == "") {

                        $('#tabla_productos').html("<h4>No hay productos en esta categoría</h4>");

                    }

                    $.each(result.d, function () {

                        $('#tabla_productos').html(this.tabla_html);

                    });


                });



            }
            else {
                alert("error");
                chosen_update();
            }

        }



        function insert() {
            var id = document.getElementById("<%=tx_id_categ.ClientID %>").value;
            var nomb = document.getElementById("<%=nombre_titu.ClientID %>").value;
            abremodal_(id, nomb);


        }
        function modal_unidad() {
            //alert("aca");
            document.getElementById("div_unidad_").click();
            chosen_update();
        }
        function chosen_update() {
            try {

                $("#<%=cb_productos_kg.ClientID%>").chosen();
                $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");
            } catch (r) { }

        }

        function esconde() {
            try {
                //var elem3 = document.getElementById("detalle_prod");
                //elem3.style = "visibility:hidden";
                //document.getElementById("detalle_prod").style = "visible:hidden";

                $("#<%=cb_productos_kg.ClientID%>").chosen();
                $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");
            } catch (e) { }
        }


    </script>


    <style>
        .test {
            background-color: #428bca !important;
            color: white !important;
        }

        .overlay {
            position: fixed;
            z-index: 0;
            top: 0px;
            left: 0px;
            right: 0px;
            bottom: 0px;
            background-color: #aaa;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }

        .Grheader {
            overflow: hidden;
            width: 26px;
            position: relative;
            top: 0px;
            z-index: 10;
            vertical-align: top;
            margin-right: 16px;
        }

        .GrCuerpo {
            width: 80%;
            position: relative;
            left: 16px;
            z-index: 1;
            overflow: auto;
            height: 100%;
        }



        td.locked, th.locked {
            position: relative;
            left: expression((this.parentElement.parentElement.parentElement.parentElement.scrollLeft-2)+'px');
        }

        th.sort-header::-moz-selection {
            background: transparent;
        }

        th.sort-header::selection {
            background: transparent;
        }

        th.sort-header {
            cursor: pointer;
        }

        table th.sort-header:after {
            content: '';
            float: right;
            margin-top: -4px;
            border-width: 0 6px 6px;
            border-style: solid;
            border-color: #404040 transparent;
            visibility: hidden;
        }

        table th.sort-header:hover:after {
            visibility: visible;
        }

        table th.sort-up:after,
        table th.sort-down:after,
        table th.sort-down:hover:after {
            visibility: visible;
            opacity: 0.9;
        }

        table th.sort-up:after {
            border-bottom: none;
            border-width: 6px 6px 0;
        }


        .estado10 {
            border-left-color: #fa5a35 !important;
        }

        .estado20 {
            border-left-color: #4e53ff !important;
        }

        .estado30 {
            border-left-color: #10ff23 !important;
        }

        .estado40 {
            border-left-color: #ff10c0 !important;
        }

        .estado10S {
            border-left-color: #c2c506 !important;
        }

        .estado10P {
            border-left-color: #06c5c5 !important;
        }

        .estadoGFX {
            border-left-color: #30441f !important;
        }
    </style>

    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="900000" EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>


    <script type="text/javascript">
        Sys.Application.add_load(BindEvents);
        function BindEvents() {

            $("#<%=cb_productos_kg.ClientID%>").change(function () {
                var arr = $(this).val();
                document.getElementById("<%=producto_modal.ClientID %>").value = arr;
            });

            $("#<%=d_cliente.ClientID%>").change(function () {
                var arr = $(this).val();
                document.getElementById("<%=l_cliente.ClientID %>").value = arr;
            });
            $("#<%=d_vendedor.ClientID%>").change(function () {
                var arr = $(this).val();
                document.getElementById("<%=l_vendedor.ClientID %>").value = arr;
            });
            $("#<%=d_producto.ClientID%>").change(function () {
                var arr = $(this).val();
                document.getElementById("<%=l_producto.ClientID %>").value = arr;
            });
            $("#<%=d_estado_sp.ClientID%>").change(function () {
                var arr = $(this).val();
                document.getElementById("<%=l_estado.ClientID %>").value = arr;
            });

        }


    </script>
    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <div id="breadcrumbs">
                <ul class="breadcrumb" style="height: 40px !important;">
                    <li>
                        <i class="fa fa-home"></i>
                        <a href="menu.aspx">Menú</a>
                        <span class="divider"><i class="fa fa-angle-right"></i></span>
                    </li>
                    <li>
                        <a runat="server" id="titulo2"></a>
                        <span class="divider"><i class="fa fa-angle-right"></i></span>
                    </li>
                    <li class="active">SP Diarias</li>
                </ul>
            </div>

            <div id="main-content">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box">
                            <div class="box-title">
                                <h3><i class="fa fa-table"></i>SP Diarias</h3>
                            </div>

                            <div class="box-content">
                                <div class="clearfix"></div>
                                <div class="table-responsive" style="border: 0">


                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box">
                                                <div class="form-group" style="display: block">

                                                    <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;"><b>Desde</b></label>

                                                    <div class="col-sm-3 col-lg-3 controls">
                                                        <div class="input-group">

                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                            <asp:TextBox ID="txt_desde" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender9" runat="server" TargetControlID="txt_desde" Format="dd/MM/yyyy" />

                                                        </div>
                                                    </div>
                                                    <label class="col-sm-1 col-lg-1 control-label"><b>Hasta</b></label>
                                                    <div class="col-sm-3 col-lg-3 controls">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                            <asp:TextBox ID="txt_hasta" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender10" runat="server" TargetControlID="txt_hasta" Format="dd/MM/yyyy" />

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                        <Triggers>
                                        </Triggers>
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="form-group">

                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Cliente</label>
                                                            <div class="col-sm-4 col-lg-4">
                                                                <div class="controls">
                                                                    <asp:ListBox ID="d_cliente" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                </div>
                                                            </div>




                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Estado</label>
                                                            <div class="col-sm-4 col-lg-4">
                                                                <div class="controls">
                                                                    <asp:ListBox ID="d_estado_sp" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                </div>
                                                            </div>



                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="form-group">
                                                            <div style="visibility: hidden; position: absolute;">
                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Grupo</label>
                                                                <div class="col-sm-4 col-lg-4">
                                                                    <div class="controls">
                                                                        <asp:ListBox ID="d_grupo" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                    </div>
                                                                </div>

                                                            </div>


                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Vendedor</label>
                                                            <div class="col-sm-4 col-lg-4">
                                                                <div class="controls">
                                                                    <asp:ListBox ID="d_vendedor" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                </div>


                                                            </div>

                                                            <label class="col-sm-1 control-label">
                                                                Nº SP.(separar por coma):
                                                            </label>
                                                            <div class="col-sm-3 controls">
                                                                <asp:TextBox runat="server" CssClass="form-control" ID="txt_sp"></asp:TextBox>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="form-group">
                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Productos</label>
                                                            <div class="col-sm-4 col-lg-4">
                                                                <div class="controls">
                                                                    <asp:ListBox ID="d_producto" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                             <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_gif" runat="server" style="display: none; font-size: 3em;"></i>


                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box">
                                                <div class="pull-right">
                                                    <div class="btn-group">
                                                        <asp:Button ID="Reporte" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-primary" OnClick="Reporte_Click" Text="Reporte" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>



                <div class="row">
                    <div class="col-md-12">
                        <div class="box">
                            <div class="box-title">
                                <h3><i class="fa fa-table"></i>SP Diarias</h3>
                            </div>

                            <div class="box-content">
                                <div class="clearfix"></div>
                                <div class="table-responsive" style="border: 0">

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box">
                                                <div class="form-group" style="display: block">

                                                    <div class="box-content">
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="col-md-6">
                                                                    <div class="box">
                                                                        <div class="form-group">

                                                                            <div runat="server" id="div_totales"></div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-sm-12" style="overflow: auto;">

                                                                        <%--                                                                <asp:GridView ID="G_SP_DIARIA" ClientIDMode="Static" AutoGenerateColumns="false" CssClass="table table-bordered filtrar" OnRowDataBound="G_SP_DIARIA_RowDataBound" runat="server" Visible="true"
                                                                    ShowHeaderWhenEmpty="True" Font-Size="12px" Settings-HorizontalScrollBarMode="Auto" SettingsBehavior-AllowSort="false"  
                                                                Styles-Cell-CssClass="textAlignLeft" ClientInstanceName="grid" Width="100%">  
                                                               
                                                                    <HeaderStyle CssClass="test no-sort" />
                                                                    <Columns>--%>

                                                                        <asp:GridView ID="G_SP_DIARIA" ClientIDMode="Static" CssClass="table table-bordered table-advance filtrar" OnRowCommand="G_SP_DIARIA_RowCommand" OnRowDataBound="G_SP_DIARIA_RowDataBound" runat="server"
                                                                            ShowHeaderWhenEmpty="True" AutoGenerateColumns="false"
                                                                            DataKeyNames="N°SP, CODPRODUCTO"
                                                                            Font-Size="12px">
                                                                            <HeaderStyle CssClass="test no-sort" />
                                                                            <Columns>

                                                                                <asp:BoundField DataField="CLIENTE" HeaderText="CLIENTE">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="N°SP" HeaderText="N°SP">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="PRODUCTO" HeaderText="PRODUCTO">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="ESTADO" HeaderText="ESTADO">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="BODEGA" HeaderText="BODEGA">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="ENTREGA" HeaderText="ENTREGA">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="PRECIO LISTA" HeaderText="PRECIO LISTA">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="PRECIO UNIT" HeaderText="PRECIO UNIT">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="DESCTO" HeaderText="DESCTO">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="CANT." HeaderText="CANT.">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="UN. VENTA" HeaderText="UN. VENTA">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="MONTO NETO" HeaderText="MONTO NETO">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="STOCKZT" HeaderText="STOCKZT">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="FECHAEMISION" HeaderText="FECHAEMISION">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>



                                                                                <asp:BoundField DataField="ValorTipoCambio" HeaderText="VALOR DOLAR">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="costoexcel" HeaderText="COSTO EXCEL">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="costocompra" HeaderText="COSTO COMPRA">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="utilidad_excel" HeaderText="UTILIDAD EXCEL">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="utilidad_compra" HeaderText="UTILIDAD COMPRA">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="porc_utilidad_excel" HeaderText="% UTILIDAD EXCEL">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="porc_utilidad_compra" HeaderText="% UTILIDAD COMPRA">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="CLIENTE_RMC" HeaderText="CLIENTE">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:BoundField>




                                                                            </Columns>

                                                                            <EmptyDataTemplate>
                                                                                No existen datos.
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>



                <asp:TextBox ID="tx_id_categ" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                <asp:TextBox ID="producto_modal" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                <asp:TextBox ID="nombre_titu" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>


                <asp:TextBox ID="l_cliente" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                <asp:TextBox ID="l_vendedor" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                <asp:TextBox ID="l_producto" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                <asp:TextBox ID="l_estado" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>


                <a href="#div_unidad" id="div_unidad_" role="button" class="btn" style="visibility: hidden; position: absolute;" data-toggle="modal"></a>

                <div id="div_unidad" class="modal fade">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content" style="height: auto;">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <div id="titulo_modal"></div>
                            </div>
                            <div class="modal-body" style="height: 70%;">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                                    <Triggers>
                                    </Triggers>
                                    <ContentTemplate>
                                        <div class="table-responsive" style="border: 0">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="form-group" style="display: block">
                                                            <div class="col-sm-6 col-lg-6">
                                                                <div class="controls">
                                                                    <asp:DropDownList runat="server" Style="width: 200px !important;" ID="cb_productos_kg" onchange="esconde()" ClientIDMode="Static" CssClass="form-control chosen"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <input id="btn_guardar_prod_equi" type="button" class="btn btn-success" value="Guardar" onclick="incluir_producto()" style="display: block; float: right;" />
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-sm-12">

                                                    <div id="tabla_productos"></div>

                                                </div>
                                            </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" onclick="esconde(); chosen_update();" aria-hidden="true">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>

