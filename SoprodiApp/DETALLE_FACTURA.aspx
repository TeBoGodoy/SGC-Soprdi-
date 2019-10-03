<%@ Page Language="C#" ViewStateMode="Enabled" AutoEventWireup="True" UICulture="es-ES" Culture="es-ES" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" CodeBehind="DETALLE_FACTURA.aspx.cs" Inherits="SoprodiApp.DETALLE_FACTURA" %>

<%@ OutputCache Location="None" NoStore="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

    <meta http-equiv="X-UA-Compatible" content="IE=10; IE=9; IE=8; IE=7; IE=EDGE" />

    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="0">

    <title>DETALLE</title>
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

    <script src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>

    <script>

        $(document).ready(function () {
            try {
                superfiltro();

            } catch (e) { }

            try {
                superfiltro2();

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

    <form runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
            <Triggers>
                <asp:PostBackTrigger ControlID="btn_excel" />
            </Triggers>
            <ContentTemplate>

                <div id="main-content">
                    <!-- BEGIN Main Content -->
                    <div class="row" id="normal" runat="server" style="display: none">
                        <div class="col-md-12">
                            <div class="box">
                                <div runat="server" id="div_sp"></div>

                                <div class="box-title" runat="server" id="filtros">
                                    <i class="fa fa-table"></i>
                                    <h3 runat="server" id="detalle" title="Detalle Venta "></h3>
                                    <div class="box-tool">
                                        <a data-action="collapse" href="#"></a>

                                    </div>
                                </div>
                                <asp:Button runat="server" ClientIDMode="Static" ID="descarga_word" Style="visibility: hidden;" OnClick="descarga_word_Click" />
                                <asp:ImageButton runat="server" Width="50" ImageUrl="~/img/Pdf.ico" Visible="false" class="btn btn-circle"  ClientIDMode="Static" ID="descarga_pdf" Style="visibility: visible;" OnClick="descarga_pdf_Click" />
                                <div class="box-content">
                                    <div class="btn-toolbar pull-left">
                                        <input type="text" id="t_filtro_memoria" style="width: 200px; margin-right: 7px; padding: 5px;" placeholder="Filtrar..." class="form-control" onchange="changevalue();" onkeyup="changevalue();" />

                                    </div>
                                    <div class="btn-toolbar pull-right">

                                        <div class="btn-group">
                                            <asp:LinkButton ID="btn_excel" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Exportar a Excel" runat="server" OnClick="btn_excel_Click"></asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="table-responsive" style="border: 0">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box">
                                                    <div class="form-group" style="overflow-x: auto;">

                                                        <asp:GridView ID="G_DETALLE_FACTURA" AutoGenerateColumns="true" CssClass="table table-bordered filtrar" OnRowDataBound="G_DETALLE_FACTURA_RowDataBound" runat="server" Visible="true"
                                                            ShowHeaderWhenEmpty="True" Font-Size="12px">
                                                            <HeaderStyle CssClass="test no-sort" />
                                                            <Columns>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                No existen datos.
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                        <div runat="server" id="table_almace">
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

                    <div class="row" id="factura" runat="server" style="display: none">
                        <div class="col-md-12">
                            <div class="box">
                                <div class="box-content">
                                    <div class="invoice">
                                        <div class="row">

                                            <div class="col-md-6">
                                                <img src="https://ci6.googleusercontent.com/proxy/0IvEZhmBpBwcP5DjENtL0h02HsbIB171IswhVy0SqL-7oeoL7QCOc3dnKgJBq-jufMopy_JcZxg=s0-d-e1-ft#http://a58.imgup.net/Sopro4d9d.png" style="width: 80px">

                                                <h2>Soprodi S.A.</h2>
                                            </div>
                                            <div class="col-md-6 invoice-info">
                                                <p class="font-size-17">
                                                    <strong>Factura #</strong>
                                                    <asp:Label ID="num_factura" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <asp:Label ID="fecha_factura" runat="server"></asp:Label>
                                                </p>
                                            </div>
                                        </div>

                                        <hr class="margin-0" />

                                        <div class="row">
                                            <div class="col-md-6 company-info">
                                                <h4>Vendedor: </h4>
                                                <p>
                                                    <asp:Label ID="vendedor_" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <i class="fa fa-circle"></i>
                                                    <asp:Label ID="codigo_vend" runat="server"></asp:Label>
                                                </p>
                                                <p>
                                                    <i class="fa fa-circle"></i>
                                                    <asp:Label ID="grupo_" runat="server"></asp:Label>
                                                </p>

                                            </div>
                                            <div class="col-md-6 company-info">
                                                <h4>Cliente: </h4>
                                                <p>
                                                    <asp:Label ID="cliente_" runat="server"></asp:Label>
                                                </p>

                                                <p>
                                                    <i class="fa fa-circle"></i>
                                                    <asp:Label ID="rut_cliente" runat="server"></asp:Label>
                                                </p>

                                            </div>
                                        </div>

                                        <br />
                                        <br />
                                        <div class="table-responsive" style="border: 0">
                                            <div class="row">

                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="btn-toolbar pull-left">
                                                            <input type="text" id="t_filtro_memoria_2" style="width: 200px; margin-right: 7px; padding: 5px;" placeholder="Filtrar..." class="form-control" onchange="changevalue();" onkeyup="changevalue();" />

                                                        </div>



                                                        <asp:GridView ID="G_PRODUCTOS" AutoGenerateColumns="true" CssClass="table table-bordered filtrar" OnRowCommand="G_PRODUCTOS_RowCommand" OnRowDataBound="G_PRODUCTOS_RowDataBound" runat="server" Visible="true"
                                                            ShowHeaderWhenEmpty="True" Font-Size="12px" DataKeyNames="Cod.Producto, bodega">
                                                            <HeaderStyle CssClass="test no-sort" />

                                                            <Columns>
                                                                <asp:TemplateField HeaderText="#" HeaderStyle-Wrap="false" ItemStyle-Width="4px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="5px" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                No existen datos.
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>


                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row" style="margin-right: 1%;">

                                                <div class="col-md-6 col-md-offset-6 invoice-amount" style="float: right">
                                                    <p><strong>Subtotal:</strong> <span>$<asp:Label runat="server" ID="subtotal"></asp:Label></span></p>

                                                    <p>
                                                        <strong>Total:</strong> <span class="green font-size-17"><strong>$<asp:Label runat="server" ID="total_"></asp:Label>
                                                        </strong></span>
                                                    </p>
                                                    <p>
                                                        <br />
                                                        <%--<a class="btn btn-success btn-xlarge" href="#">Start Payment Process</a>--%>
                                                    </p>
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
