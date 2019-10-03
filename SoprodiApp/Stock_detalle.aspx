<%@ Page Language="C#" AutoEventWireup="True" UICulture="es-ES" Culture="es-ES" CodeBehind="Stock_detalle.aspx.cs" Inherits="SoprodiApp.Stock_detalle" %>

<%@ OutputCache Location="None" NoStore="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

    <meta http-equiv="X-UA-Compatible" content="IE=10; IE=9; IE=8; IE=7; IE=EDGE" />

    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="0">

    <title>DETALLE_STOCK</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <!-- Place favicon.ico and apple-touch-icon.png in the root directory -->
    <link href="img/icono.ico" rel="shortcut icon">
    <!--base css styles-->
    <link rel="stylesheet" href="assets/bootstrap/css/bootstrap.min.css">
    <link rel="stylesheet" href="assets/font-awesome/css/font-awesome.min.css">

    <!--page specific css styles-->

    <link rel="stylesheet" href="CHOSEN/docsupport/style.css">
    <link rel="stylesheet" href="CHOSEN/docsupport/prism.css">
    <link rel="stylesheet" href="CHOSEN/chosen.css">

    <!--base css styles-->
    <link rel="stylesheet" href="assets/bootstrap/css/bootstrap.min.css">
    <link rel="stylesheet" href="assets/font-awesome/css/font-awesome.min.css">

    <!--page specific css styles-->

    <!--flaty css styles-->
    <link rel="stylesheet" href="css/flaty.css">
    <link rel="stylesheet" href="css/flaty-responsive.css">

    <script type="text/javascript">


    </script>
    <script src="js/tablesort.js"></script>
    <script src="js/prettify.js"></script>

    <script src='js/sorts/tablesort.date.js'></script>
    <script src='js/sorts/tablesort.dotsep.js'></script>
    <script src='js/sorts/tablesort.filesize.js'></script>
    <script src='js/sorts/tablesort.numeric.js'></script>



    <link href="js/DataTable/Datatable.css" rel="stylesheet" />
    <link href="js/DataTable/data-table.css" rel="stylesheet" />


    <script src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>

    <script src="js/DataTable/jquery.dataTables.js"></script>
    <script src="js/DataTable/date.js"></script>


    <script>

        $(document).ready(function () {
            try {
                superfiltro();

            } catch (e) { }

            try {
                superfiltro2();

            } catch (e) { }
            try {
                superfiltro3();

            } catch (e) { }
        });

        function superfiltro() {
            try {
                $(".filtrar tr:has(td)").each(function () {
                    var t = $(this).text().toLowerCase();
                    $("<td class='indexColumn'></td>")
                    .hide().text(t).appendTo(this);
                });
                //Agregar el comportamiento al texto (se selecciona por el ID) 
                $("#t_filtro_memoria").keyup(function () {
                    var s = $(this).val().toLowerCase().split(" ");
                    $(".filtrar tr:hidden").show();
                    $.each(s, function () {
                        $(".filtrar tr:visible .indexColumn:not(:contains('"
                        + this + "'))").parent().hide();
                    });
                });
            } catch (e) { }


        }

        function SortPrah() {
            $(document).ready(function () {

                $("#TABLA_REPORTE2").tablesorter({
                    usNumberFormat: false,
                    sortList: [[3, 0]]
                });
            });
        }



        function creagrilla() {
            

        }

        function fuera3(id_archivo, año) {
            var urlPdf = "/PDF.aspx?";

            var id_archivo = "Ar=" + id_archivo;
            var año_fact = "&A=" + año;
            var urlPdf_Final = urlPdf + id_archivo + año_fact;



            if (navigator.appName.indexOf('Microsoft Internet Explorer') != -1) {
                window.showModelessDialog(urlPdf_Final, '_blank');
            };

            if (navigator.appName.indexOf('Netscape') != -1) {
                window.open(urlPdf_Final, '_blank');
                void 0;
            };
            return false;
        }

        function fuera22(rutcliente, bit) {

            var urlPdf = "/FICHA_CLIENTE.aspx?";
            //var path2 = "P=" + path;
            //var filename2 = "&N=" + filename;
            //var urlPdf_Final = urlPdf + path2 + filename2;
            var param = "R=" + rutcliente + "&i=" + bit;
            var urlPdf_Final = urlPdf + param;

            if (navigator.appName.indexOf('Microsoft Internet Explorer') != -1) {
                window.showModelessDialog(urlPdf_Final, '', 'dialogTop:50px;dialogLeft:50px;dialogHeight:500px;dialogWidth:1100px');
            };

            if (navigator.appName.indexOf('Netscape') != -1) {
                window.open(urlPdf_Final, '', 'width=1100,height=500,left=50,top=50');
                void 0;
            };
            return false;
        }


        function superfiltro2() {
            try {
                $(".filtrar tr:has(td)").each(function () {
                    var t = $(this).text().toLowerCase();
                    $("<td class='indexColumn'></td>")
                    .hide().text(t).appendTo(this);
                });
                //Agregar el comportamiento al texto (se selecciona por el ID) 
                $("#t_filtro_memoria_2").keyup(function () {
                    var s = $(this).val().toLowerCase().split(" ");
                    $(".filtrar tr:hidden").show();
                    $.each(s, function () {
                        $(".filtrar tr:visible .indexColumn:not(:contains('"
                        + this + "'))").parent().hide();
                    });
                });
            } catch (e) { }


        }


        function superfiltro3() {
            try {
                $(".filtrar tr:has(td)").each(function () {
                    var t = $(this).text().toLowerCase();
                    $("<td class='indexColumn'></td>")
                    .hide().text(t).appendTo(this);
                });
                //Agregar el comportamiento al texto (se selecciona por el ID) 
                $("#t_filtro_memoria_3").keyup(function () {
                    var s = $(this).val().toLowerCase().split(" ");
                    $(".filtrar tr:hidden").show();
                    $.each(s, function () {
                        $(".filtrar tr:visible .indexColumn:not(:contains('"
                        + this + "'))").parent().hide();
                    });
                });
            } catch (e) { }


        }

        function fuera(fecha, vendedor, grupos, bit) {


            var urlPdf = "/DETALLE_FACTURA.aspx?";
            //var path2 = "P=" + path;
            //var filename2 = "&N=" + filename;
            //var urlPdf_Final = urlPdf + path2 + filename2;
            var param = "F=" + fecha + "&V=" + vendedor + "&G=" + grupos + "&i=" + bit;
            var urlPdf_Final = urlPdf + param;

            if (navigator.appName.indexOf('Microsoft Internet Explorer') != -1) {
                window.showModelessDialog(urlPdf_Final, '', 'dialogTop:50px;dialogLeft:50px;dialogHeight:500px;dialogWidth:1100px');
            };

            if (navigator.appName.indexOf('Netscape') != -1) {
                window.open(urlPdf_Final, '', 'width=1100,height=500,left=50,top=50');
                void 0;
            };
            return false;
        }

    </script>

    <style>
        .test {
            background-color: #428bca !important;
            color: white !important;
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
    </style>

</head>



<body>

    <form id="Form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
            <ContentTemplate>

                <div id="main-content">
                    <!-- BEGIN Main Content -->

                    <div class="row" id="GENERAL_DIV" runat="server" style="display: none">
                        <div class="col-md-12">
                            <div class="box">
                                <div class="box-title" runat="server" id="Div2">
                                    <i class="fa fa-table"></i>
                                    <h3 runat="server" id="H1" title="Detalle Stock">Detalle General</h3>
                                    <div class="box-tool">
                                        <a data-action="collapse" href="#"></a>

                                    </div>
                                </div>
                                <div class="box-content">
                                    <div class="btn-toolbar pull-left">
                                        <input type="text" id="t_filtro_memoria_2" style="width: 200px; margin-right: 7px; padding: 5px;" placeholder="Filtrar..." class="form-control" onchange="changevalue();" onkeyup="changevalue();" />

                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="table-responsive" style="border: 0">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box">
                                                    <div class="form-group" style="overflow-x: auto;">

                                                        <asp:GridView ID="G_GENERAL" ClientIDMode="Static" AutoGenerateColumns="true" CssClass="table table-bordered filtrar" OnRowDataBound="G_GENERAL_RowDataBound" runat="server" Visible="true"
                                                            ShowHeaderWhenEmpty="True" Font-Size="12px">
                                                            <HeaderStyle CssClass="test no-sort" />
                                                            <Columns>

                                                                <%--                                                                <asp:BoundField DataField="PONbr" HeaderText="OC" HeaderStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="provoc" HeaderText="Proveedor" HeaderStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="oc" HeaderText="NombreProveedor" HeaderStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="IVA" HeaderText="IVA" HeaderStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                </asp:BoundField>--%>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                No existen datos.
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>

                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>



                    <div class="row" id="normal" runat="server" style="display: none">
                        <div class="col-md-12">
                            <div class="box">
                                <div class="box-title" runat="server" id="filtros">
                                    <i class="fa fa-table"></i>
                                    <h3 runat="server" id="detalle" title="Detalle Stock">Detalle Stock</h3>
                                    <div class="box-tool">
                                        <a data-action="collapse" href="#"></a>

                                    </div>
                                </div>
                                <div class="box-content">
                                    <div class="btn-toolbar pull-left">
                                        <input type="text" id="t_filtro_memoria" style="width: 200px; margin-right: 7px; padding: 5px;" placeholder="Filtrar..." class="form-control" onchange="changevalue();" onkeyup="changevalue();" />

                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="table-responsive" style="border: 0">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box">
                                                    <div class="form-group" style="overflow-x: auto;">

                                                        <asp:GridView ID="G_DETALLE_STOCK" ClientIDMode="Static" AutoGenerateColumns="true" CssClass="table table-bordered filtrar" OnRowDataBound="G_DETALLE_STOCK_RowDataBound" runat="server" Visible="true"
                                                            ShowHeaderWhenEmpty="True" Font-Size="12px">
                                                            <HeaderStyle CssClass="test no-sort" />
                                                            <Columns>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                No existen datos.
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>

                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row" id="VENTAS" runat="server" style="display: none">
                        <div class="col-md-12">
                            <div class="box">
                                <div class="box-title" runat="server" id="Div3">
                                    <i class="fa fa-table"></i>
                                    <h3 runat="server" id="H2" title="Detalle Stock">Detalle Stock</h3>
                                    <div class="box-tool">
                                        <a data-action="collapse" href="#"></a>

                                    </div>
                                </div>
                                <div class="box-content">
                                    <div class="btn-toolbar pull-left">
                                        <input type="text" id="t_filtro_memoria_3" style="width: 200px; margin-right: 7px; padding: 5px;" placeholder="Filtrar..." class="form-control" onchange="changevalue();" onkeyup="changevalue();" />

                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="table-responsive" style="border: 0">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box">
                                                    <div class="form-group" style="overflow-x: auto;">

                                                        <asp:GridView ID="G_VENTAS" ClientIDMode="Static" AutoGenerateColumns="true" CssClass="table table-bordered filtrar" OnRowDataBound="G_VENTAS_RowDataBound" runat="server" Visible="true"
                                                            ShowHeaderWhenEmpty="True" Font-Size="12px">
                                                            <HeaderStyle CssClass="test no-sort" />
                                                            <Columns>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                No existen datos.
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>

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
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- END Main Content -->

        <a id="btn-scrollup" class="btn btn-circle btn-lg" href="#"></a>
    </form>
</body>
