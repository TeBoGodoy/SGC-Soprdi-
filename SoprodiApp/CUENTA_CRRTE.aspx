<%@ Page Language="C#" AutoEventWireup="True" UICulture="es-ES" Culture="es-ES" CodeBehind="CUENTA_CRRTE.aspx.cs" Inherits="SoprodiApp.CUENTA_CRRTE" %>

<%@ OutputCache Location="None" NoStore="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

    <meta http-equiv="X-UA-Compatible" content="IE=10; IE=9; IE=8; IE=7; IE=EDGE" />

    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="0">

    <title>Ficha</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>

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
    <script src="js/tablesort.js"></script>
    <script src="js/prettify.js"></script>

    <script src='js/sorts/tablesort.date.js'></script>
    <script src='js/sorts/tablesort.dotsep.js'></script>
    <script src='js/sorts/tablesort.filesize.js'></script>
    <script src='js/sorts/tablesort.numeric.js'></script>
    <script src="js/jquery.tablesorter.js"></script>

    <script>


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
        function CARGANDO() {

            try {
                var elem232 = document.getElementById("crear_cargando");
                elem232.style.display = "block";
            } catch (e) { }


        }

        $().ready(function () {

            superfiltro();




            //SORT_GRILLA();

        });

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
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>

    <form runat="server" id="qwer">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
            <ContentTemplate>


                <script type="text/javascript">

                    Sys.Application.add_load(BindEvents);

                    function BindEvents() {

                    }

                </script>

                <div id="main-content">
                    <asp:MultiView ID="CLIENTES_FICHA" runat="server" ActiveViewIndex="0">
                        <%-- 0 ver ficha completa --%>
                        <asp:View ID="VER" runat="server">
                            <!-- BEGIN Main Content -->
                            <div class="row" id="normal" runat="server" style="display: none">
                                <div class="col-md-12">
                                    <div class="box">
                                        <div class="box-title" runat="server" id="filtros">
                                            <i class="fa fa-table"></i>
                                            <h3 runat="server" id="detalle" title="Detalle Venta "></h3>
                                            <div class="box-tool">
                                                <a data-action="collapse" href="#"></a>

                                            </div>
                                        </div>
                                        <div class="box-content">
                                            <div class="clearfix"></div>
                                            <div class="table-responsive" style="border: 0">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="box">

                                                            <div class="row">
                                                                <div class="col-md-6 company-info">
                                                                    <h4>


                                                                        <div class="company-info">
                                                                            <p>

                                                                                <asp:Label class="form-control-static" ID="rut_clie" runat="server"></asp:Label>

                                                                            </p>
                                                                            <p>

                                                                                <asp:Label class="form-control-static" ID="nom_clie" runat="server"></asp:Label>

                                                                            </p>
                                                                        </div>


                                                                        <div class="company-info">
                                                                        </div>

                                                                    </h4>
                                                                </div>

                                                            </div>
                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label1" runat="server" Text="Vendedor"></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">
                                                                        <div class="form-group">
                                                                            <div class="row">


                                                                                <div class="col-md-8 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-user"></i>
                                                                                        <div class="controls">
                                                                                            <asp:Label class="form-control-static" runat="server" ID="vendedor"></asp:Label>
                                                                                        </div>
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label38" runat="server" Text="Antecedentes Generales"></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">

                                                                        <div class="form-group">
                                                                            <div class="row">

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-location-arrow"></i>
                                                                                        <asp:Label class="form-control-static" ID="direccion_clie" runat="server"></asp:Label>
                                                                                </div>
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-flag"></i>
                                                                                        <asp:Label class="form-control-static" ID="ciu_clie" runat="server"></asp:Label>

                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-phone"></i>
                                                                                        <asp:Label class="form-control-static" ID="fono_clie" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-barcode"></i>
                                                                                        &nbsp;&nbsp;
                                                                                        <asp:Label class="form-control-static" ID="giro_clie" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>



                                                                            </div>
                                                                        </div>
                                                                        <hr />
                                                                        <hr />
                                                                        <div class="row">
                                                                            <div class="text-left" style="padding-left: 2%;">
                                                                                <b>
                                                                                    <asp:Label ID="Label43" runat="server" Text="Contacto 1"></asp:Label>
                                                                                </b>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <div class="row">
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-dot-circle-o"></i>
                                                                                        <asp:Label class="form-control-static" ID="nomb_cont1" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-envelope"></i>
                                                                                        <asp:Label class="form-control-static" ID="correo_cont1" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-gavel"></i>
                                                                                        <asp:Label class="form-control-static" ID="cargo_cont1" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-phone"></i>
                                                                                        <asp:Label class="form-control-static" ID="fono_cont1" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <%--<div class="pull-right">
                                                                            <div class="btn-group">
                                                                                <asp:LinkButton ID="LinkButton2" class="btn btn-circle btn-success fa fa-plus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Nuevo Contacto" runat="server" OnClick="LinkButton2_Click"></asp:LinkButton>
                                                                            </div>
                                                                        </div>--%>
                                                                        <div runat="server" id="cont_2" style="display: none;">
                                                                            <hr />
                                                                            <div class="row">
                                                                                <div class="text-left" style="padding-left: 2%;">
                                                                                    <b>
                                                                                        <asp:Label ID="Label48" runat="server" Text="Contacto 2"></asp:Label>
                                                                                    </b>
                                                                                </div>
                                                                            </div>

                                                                            <div class="form-group">
                                                                                <div class="row">

                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa  fa-dot-circle-o"></i>
                                                                                            <asp:Label class="form-control-static" ID="nomb_cont2" runat="server"></asp:Label>
                                                                                        </p>
                                                                                    </div>

                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa fa-envelope"></i>
                                                                                            <asp:Label class="form-control-static" ID="correo_cont2" runat="server"></asp:Label>
                                                                                        </p>
                                                                                    </div>
                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa  fa-gavel"></i>
                                                                                            <asp:Label class="form-control-static" ID="cargo_cont2" runat="server"></asp:Label>
                                                                                        </p>
                                                                                    </div>
                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa fa-phone"></i>
                                                                                            <asp:Label class="form-control-static" ID="fono_cont2" runat="server"></asp:Label>
                                                                                        </p>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <%-- <div class="pull-right">
                                                                                <div class="btn-group">
                                                                                    <asp:LinkButton ID="LinkButton3" class="btn btn-circle btn-danger fa fa-minus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Quitar Contacto" runat="server" OnClick="LinkButton3_Click"></asp:LinkButton>
                                                                                </div>
                                                                            </div>--%>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label58" runat="server" Text="Tipo Negocio"></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">
                                                                        <div class="form-group">
                                                                            <div class="row">


                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-briefcase"></i>
                                                                                        <asp:Label class="form-control-static" ID="tipo_negoc" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label60" runat="server" Text="Refencia Bancaria"></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">
                                                                        <div class="form-group">
                                                                            <div class="row">


                                                                                <div class="col-md-5 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-btc"></i>
                                                                                        <asp:Label class="form-control-static" ID="bank_1" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>



                                                                                <div class="col-md-5 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-credit-card"></i>
                                                                                        <asp:Label class="form-control-static" ID="l_cta1" runat="server"></asp:Label>

                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <%-- <div class="pull-right">
                                                                            <div class="btn-group">
                                                                                <asp:LinkButton ID="LinkButton4" class="btn btn-circle btn-success fa fa-plus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Nueva CTA" runat="server" OnClick="LinkButton4_Click"></asp:LinkButton>
                                                                            </div>
                                                                        </div>--%>

                                                                        <div runat="server" id="otra_cta_cte" style="display: none;">
                                                                            <hr />
                                                                            <div class="form-group">
                                                                                <div class="row">


                                                                                    <div class="col-md-5 company-info">
                                                                                        <p>
                                                                                            <i class="fa  fa-btc"></i>
                                                                                            <asp:Label class="form-control-static" ID="bank_2" runat="server"></asp:Label>
                                                                                        </p>
                                                                                    </div>



                                                                                    <div class="col-md-5 company-info">
                                                                                        <p>
                                                                                            <i class="fa fa-credit-card"></i>
                                                                                            <asp:Label class="form-control-static" ID="l_cta2" runat="server"></asp:Label>
                                                                                        </p>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <%-- <div class="pull-right">
                                                                                <div class="btn-group">
                                                                                    <asp:LinkButton ID="LinkButton5" class="btn btn-circle btn-danger fa fa-minus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Quitar CTA" runat="server" OnClick="LinkButton5_Click"></asp:LinkButton>
                                                                                </div>
                                                                            </div>--%>
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label63" runat="server" Text="Antecedentes de la Sociedad"></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">
                                                                        <div class="form-group">
                                                                            <div class="row">

                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-circle"></i>
                                                                                        <asp:Label class="form-control-static" ID="rut_socied" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-dot-circle-o"></i>
                                                                                        <asp:Label class="form-control-static" ID="nomb_socied" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-location-arrow"></i>
                                                                                        <asp:Label class="form-control-static" ID="direcc_socied" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-flag"></i>
                                                                                        <asp:Label class="form-control-static" ID="ciu_socied" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-phone"></i>
                                                                                        <asp:Label class="form-control-static" ID="fono_socied" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-envelope"></i>
                                                                                        <asp:Label class="form-control-static" ID="correo_socied" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <hr />
                                                                        <hr />

                                                                        <div class="row">
                                                                            <div class="text-left" style="padding-left: 2%;">
                                                                                <b>
                                                                                    <asp:Label ID="Label70" runat="server" Text="Socio 1"></asp:Label>
                                                                                </b>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <div class="row">
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-circle"></i>
                                                                                        <asp:Label class="form-control-static" ID="rut_soc1" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-dot-circle-o"></i>
                                                                                        <asp:Label class="form-control-static" ID="nom_soc1" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-envelope"></i>
                                                                                        <asp:Label class="form-control-static" ID="corr_soc1" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa">%</i>
                                                                                        <asp:Label class="form-control-static" ID="porc_soc1" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <%--  <div class="pull-right">
                                                                            <div class="btn-group">
                                                                                <asp:LinkButton ID="LinkButton6" class="btn btn-circle btn-success fa fa-plus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Nuevo Socio" runat="server" OnClick="LinkButton6_Click"></asp:LinkButton>
                                                                            </div>
                                                                        </div>--%>

                                                                        <div runat="server" id="nuevo_socio" style="display: none;">
                                                                            <hr />
                                                                            <div class="row">
                                                                                <div class="text-left" style="padding-left: 2%;">
                                                                                    <b>
                                                                                        <asp:Label ID="Label75" runat="server" Text="Socio 2"></asp:Label>
                                                                                    </b>
                                                                                </div>
                                                                            </div>

                                                                            <div class="form-group">
                                                                                <div class="row">

                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa  fa-circle"></i>
                                                                                            <asp:Label class="form-control-static" ID="rut_soc2" runat="server"></asp:Label>
                                                                                        </p>
                                                                                    </div>
                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa  fa-dot-circle-o"></i>
                                                                                            <asp:Label class="form-control-static" ID="nom_soc2" runat="server"></asp:Label>
                                                                                        </p>
                                                                                    </div>

                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa fa-envelope"></i>
                                                                                            <asp:Label class="form-control-static" ID="corr_soc2" runat="server"></asp:Label>
                                                                                        </p>
                                                                                    </div>

                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa">%</i>
                                                                                            <asp:Label class="form-control-static" ID="porc_soc2" runat="server"></asp:Label>
                                                                                        </p>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <%-- <div class="pull-right">
                                                                                <div class="btn-group">
                                                                                    <asp:LinkButton ID="LinkButton7" class="btn btn-circle btn-danger fa fa-minus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Quitar Socio" runat="server" OnClick="LinkButton7_Click"></asp:LinkButton>
                                                                                </div>
                                                                            </div>--%>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>

                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label2" runat="server" Text="Antecedentes SOPRODI S.A."></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">
                                                                        <div class="form-group">
                                                                            <div class="row">


                                                                                <div class="col-sm-3 col-lg-3 controls">
                                                                                    <p>
                                                                                        <div class="input-group">

                                                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                                            <asp:Label class="form-control-static" ID="cliente_desde" runat="server"></asp:Label>
                                                                                        </div>

                                                                                    </p>
                                                                                </div>



                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <div class="row">

                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-credit-card"></i>
                                                                                        <asp:Label class="form-control-static" ID="cred_actual" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-bars"></i>
                                                                                        <asp:Label class="form-control-static" ID="t_cred_act" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-money"></i>
                                                                                        <asp:Label class="form-control-static" ID="mon_cred_act" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <div class="row">

                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-credit-card"></i>
                                                                                        <asp:Label class="form-control-static" ID="cred_sol" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-bars"></i>
                                                                                        <asp:Label class="form-control-static" ID="t_cred_sol" runat="server"></asp:Label>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-money"></i>
                                                                                        <asp:Label class="form-control-static" ID="mon_cred_sol" runat="server"></asp:Label>
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
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>



                        </asp:View>
                        <%-- 1 editar ficha completa --%>
                        <asp:View ID="editar" runat="server">
                            <div class="row" id="factura" runat="server" style="display: none">
                                <div class="col-md-12">
                                    <div class="box">
                                        <div class="box-title" runat="server" id="Div1">
                                            <i class="fa fa-table"></i>
                                            <h3 runat="server" id="H1" title="Detalle Venta "></h3>
                                            <div class="box-tool">
                                                <a data-action="collapse" href="#"></a>

                                            </div>
                                        </div>
                                        <div class="box-content">

                                            <div class="clearfix"></div>
                                            <div class="table-responsive" style="border: 0">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="box">

                                                            <div class="row">
                                                                <div class="col-md-6 company-info">
                                                                    <h4>


                                                                        <div class="company-info">
                                                                            <p>

                                                                                <asp:Label class="form-control-static" ID="ed_rut" runat="server"></asp:Label>

                                                                            </p>
                                                                            <p>

                                                                                <asp:Label class="form-control-static" ID="ed_nombre" runat="server"></asp:Label>

                                                                            </p>
                                                                        </div>


                                                                        <div class="company-info">
                                                                        </div>


                                                                    </h4>
                                                                </div>

                                                            </div>
                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label3" runat="server" Text="Vendedor"></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">
                                                                        <div class="form-group">
                                                                            <div class="row">


                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-user"></i>
                                                                                        <div class="controls">
                                                                                            <asp:ListBox ID="vendedor_NEW_" SelectionMode="Multiple" Width="120%" data-placeholder=" -- Otro -- " runat="server" Visible="false" class="form-control chosen-select"></asp:ListBox>

                                                                                            <asp:DropDownList runat="server" Width="120%" ID="vendedor_NNEW"></asp:DropDownList>
                                                                                        </div>
                                                                                        <textarea runat="server" class="form-control" id="Textarea1" visible="false"></textarea>
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label4" runat="server" Text="Antecedentes Generales"></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">

                                                                        <div class="form-group">
                                                                            <div class="row">

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-location-arrow"></i>
                                                                                        <textarea class="form-control" id="ed_direcc" runat="server"></textarea>
                                                                                        <span class="help-inline">Dirección&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                                </div>
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-flag"></i>
                                                                                        <textarea class="form-control" id="ed_ciudad" runat="server"></textarea>
                                                                                        <span class="help-inline">"Ciudad/Región" , "País"</span>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-phone"></i>
                                                                                        <textarea class="form-control" id="ed_fono" runat="server"></textarea>
                                                                                        <span class="help-inline">Teléfono&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-barcode"></i>
                                                                                        <textarea class="form-control" id="ed_giro" runat="server"></textarea>
                                                                                        <span class="help-inline">Giro&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                                    </p>
                                                                                </div>



                                                                            </div>
                                                                        </div>
                                                                        <hr />
                                                                        <hr />
                                                                        <div class="row">
                                                                            <div class="text-left" style="padding-left: 2%;">
                                                                                <b>
                                                                                    <asp:Label ID="Label5" runat="server" Text="Contacto 1"></asp:Label>
                                                                                </b>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <div class="row">
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-dot-circle-o"></i>
                                                                                        <textarea class="form-control" id="ed_nom_cont1" runat="server"></textarea>
                                                                                        <span class="help-inline">Nombre</span>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-envelope"></i>
                                                                                        <textarea class="form-control" id="ed_corre_cont1" runat="server"></textarea>
                                                                                        <span class="help-inline">Correo</span>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-gavel"></i>
                                                                                        <textarea class="form-control" id="ed_cargo_cont1" runat="server"></textarea>
                                                                                        <span class="help-inline">Cargo</span>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-phone"></i>
                                                                                        <textarea class="form-control" id="ed_fono_cont1" runat="server"></textarea>
                                                                                        <span class="help-inline">Teléfono</span>
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="pull-right">
                                                                            <div class="btn-group">
                                                                                <asp:LinkButton ID="LinkButton1" class="btn btn-circle btn-success fa fa-plus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Nuevo Contacto" runat="server" OnClick="LinkButton2_Click"></asp:LinkButton>
                                                                            </div>
                                                                        </div>
                                                                        <div runat="server" id="Div2" style="display: none;">
                                                                            <hr />
                                                                            <div class="row">
                                                                                <div class="text-left" style="padding-left: 2%;">
                                                                                    <b>
                                                                                        <asp:Label ID="Label6" runat="server" Text="Contacto 2"></asp:Label>
                                                                                    </b>
                                                                                </div>
                                                                            </div>

                                                                            <div class="form-group">
                                                                                <div class="row">

                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa  fa-dot-circle-o"></i>
                                                                                            <textarea class="form-control" id="ed_nom_cont2" runat="server"></textarea>
                                                                                            <span class="help-inline">Nombre</span>
                                                                                        </p>
                                                                                    </div>

                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa fa-envelope"></i>
                                                                                            <textarea class="form-control" id="ed_correo_cont2" runat="server"></textarea>
                                                                                            <span class="help-inline">Correo</span>
                                                                                        </p>
                                                                                    </div>
                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa  fa-gavel"></i>
                                                                                            <textarea class="form-control" id="ed_cargo_cont2" runat="server"></textarea>
                                                                                            <span class="help-inline">Cargo</span>
                                                                                        </p>
                                                                                    </div>
                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa fa-phone"></i>
                                                                                            <textarea class="form-control" id="ed_fono_cont2" runat="server"></textarea>
                                                                                            <span class="help-inline">Teléfono</span>
                                                                                        </p>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="pull-right">
                                                                                <div class="btn-group">
                                                                                    <asp:LinkButton ID="LinkButton8" class="btn btn-circle btn-danger fa fa-minus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Quitar Contacto" runat="server" OnClick="LinkButton3_Click"></asp:LinkButton>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label7" runat="server" Text="Tipo Negocio"></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">
                                                                        <div class="form-group">
                                                                            <div class="row">


                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-briefcase"></i>
                                                                                        <asp:DropDownList runat="server" Width="120%" ID="tipo_negocio"></asp:DropDownList>

                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label8" runat="server" Text="Refencia Bancaria"></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">
                                                                        <div class="form-group">
                                                                            <div class="row">


                                                                                <div class="col-md-5 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-btc"></i>
                                                                                        <textarea class="form-control" id="ed_bank1" runat="server"></textarea>
                                                                                        <span class="help-inline">Banco</span>
                                                                                    </p>
                                                                                </div>



                                                                                <div class="col-md-5 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-credit-card"></i>
                                                                                        <textarea class="form-control" id="ed_cta1" runat="server"></textarea>
                                                                                        <span class="help-inline">CTA.CTE.</span>

                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="pull-right">
                                                                            <div class="btn-group">
                                                                                <asp:LinkButton ID="LinkButton9" class="btn btn-circle btn-success fa fa-plus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Nueva CTA" runat="server" OnClick="LinkButton4_Click"></asp:LinkButton>
                                                                            </div>
                                                                        </div>

                                                                        <div runat="server" id="Div3" style="display: none;">
                                                                            <hr />
                                                                            <div class="form-group">
                                                                                <div class="row">


                                                                                    <div class="col-md-5 company-info">
                                                                                        <p>
                                                                                            <i class="fa  fa-btc"></i>
                                                                                            <textarea class="form-control" id="ed_bank2" runat="server"></textarea>
                                                                                            <span class="help-inline">Banco</span>
                                                                                        </p>
                                                                                    </div>



                                                                                    <div class="col-md-5 company-info">
                                                                                        <p>
                                                                                            <i class="fa fa-credit-card"></i>
                                                                                            <textarea class="form-control" id="ed_cta2" runat="server"></textarea>
                                                                                            <span class="help-inline">CTA.CTE.</span>

                                                                                        </p>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="pull-right">
                                                                                <div class="btn-group">
                                                                                    <asp:LinkButton ID="LinkButton10" class="btn btn-circle btn-danger fa fa-minus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Quitar CTA" runat="server" OnClick="LinkButton5_Click"></asp:LinkButton>
                                                                                </div>
                                                                            </div>

                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label9" runat="server" Text="Antecedentes de la Sociedad"></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">
                                                                        <div class="form-group">
                                                                            <div class="row">

                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-dot-circle-o"></i>
                                                                                        <textarea class="form-control" id="ed_nom_socie" runat="server"></textarea>
                                                                                        <span class="help-inline">Nombre</span>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-circle"></i>
                                                                                        <textarea class="form-control" id="ed_rut_socie" runat="server"></textarea>
                                                                                        <span class="help-inline">RUN</span>
                                                                                    </p>
                                                                                </div>


                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-location-arrow"></i>
                                                                                        <textarea class="form-control" id="ed_direcc_socie" runat="server"></textarea>
                                                                                        <span class="help-inline">Dirección</span>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-flag"></i>
                                                                                        <textarea class="form-control" id="ed_ciudad_socie" runat="server"></textarea>
                                                                                        <span class="help-inline">"Ciudad" , "País"</span>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-phone"></i>
                                                                                        <textarea class="form-control" id="ed_fono_socie" runat="server"></textarea>
                                                                                        <span class="help-inline">Teléfono</span>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-envelope"></i>
                                                                                        <textarea class="form-control" id="ed_correo_socie" runat="server"></textarea>
                                                                                        <span class="help-inline">Correo</span>
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <hr />
                                                                        <hr />

                                                                        <div class="row">
                                                                            <div class="text-left" style="padding-left: 2%;">
                                                                                <b>
                                                                                    <asp:Label ID="Label10" runat="server" Text="Socio 1"></asp:Label>
                                                                                </b>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <div class="row">
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-circle"></i>
                                                                                        <textarea class="form-control" id="ed_rut_soc1" runat="server"></textarea>
                                                                                        <span class="help-inline">RUN</span>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-dot-circle-o"></i>
                                                                                        <textarea class="form-control" id="ed_nom_soc1" runat="server"></textarea>
                                                                                        <span class="help-inline">Nombre</span>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-envelope"></i>
                                                                                        <textarea class="form-control" id="ed_correo_soc1" runat="server"></textarea>
                                                                                        <span class="help-inline">Correo</span>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa">%</i>
                                                                                        <textarea class="form-control" id="ed_porc_soc1" runat="server"></textarea>
                                                                                        <span class="help-inline">Porcentaje</span>
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="pull-right">
                                                                            <div class="btn-group">
                                                                                <asp:LinkButton ID="LinkButton11" class="btn btn-circle btn-success fa fa-plus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Nuevo Socio" runat="server" OnClick="LinkButton6_Click"></asp:LinkButton>
                                                                            </div>
                                                                        </div>

                                                                        <div runat="server" id="Div4" style="display: none;">
                                                                            <hr />
                                                                            <div class="row">
                                                                                <div class="text-left" style="padding-left: 2%;">
                                                                                    <b>
                                                                                        <asp:Label ID="Label11" runat="server" Text="Socio 2"></asp:Label>
                                                                                    </b>
                                                                                </div>
                                                                            </div>

                                                                            <div class="form-group">
                                                                                <div class="row">

                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa  fa-circle"></i>
                                                                                            <textarea class="form-control" id="ed_rut_soc2" runat="server"></textarea>
                                                                                            <span class="help-inline">RUN</span>
                                                                                        </p>
                                                                                    </div>
                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa  fa-dot-circle-o"></i>
                                                                                            <textarea class="form-control" id="ed_nom_soc2" runat="server"></textarea>
                                                                                            <span class="help-inline">Nombre</span>
                                                                                        </p>
                                                                                    </div>

                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa fa-envelope"></i>
                                                                                            <textarea class="form-control" id="ed_correo_soc2" runat="server"></textarea>
                                                                                            <span class="help-inline">Correo</span>
                                                                                        </p>
                                                                                    </div>

                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa">%</i>
                                                                                            <textarea class="form-control" id="ed_porc_soc2" runat="server"></textarea>
                                                                                            <span class="help-inline">Porcentaje</span>
                                                                                        </p>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="pull-right">
                                                                                <div class="btn-group">
                                                                                    <asp:LinkButton ID="LinkButton12" class="btn btn-circle btn-danger fa fa-minus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Quitar Socio" runat="server" OnClick="LinkButton7_Click"></asp:LinkButton>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>

                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label12" runat="server" Text="Antecedentes SOPRODI S.A."></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">
                                                                        <div class="form-group">
                                                                            <div class="row">


                                                                                <div class="col-sm-3 col-lg-3 controls">
                                                                                    <p>
                                                                                        <div class="input-group">

                                                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                                            <asp:TextBox ID="ed_cliente_desde" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="ed_cliente_desde" Format="dd/MM/yyyy" />

                                                                                        </div>
                                                                                        <span class="help-inline">Cliente desde</span>
                                                                                    </p>
                                                                                </div>



                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <div class="row">

                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-credit-card"></i>
                                                                                        <textarea class="form-control" id="ed_cred_act" runat="server"></textarea>
                                                                                        <span class="help-inline">Crédito Actual</span>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-bars"></i>
                                                                                        <textarea class="form-control" id="ed_ti_cred_act" runat="server"></textarea>
                                                                                        <span class="help-inline">Tipo Crédito Actual</span>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-money"></i>
                                                                                        <textarea class="form-control" id="ed_mon_cred_act" runat="server"></textarea>
                                                                                        <span class="help-inline">Monto Crédito Actual</span>
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <div class="row">

                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-credit-card"></i>
                                                                                        <textarea class="form-control" id="ed_cred_soli" runat="server"></textarea>
                                                                                        <span class="help-inline">Crédito Solicitado</span>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-bars"></i>
                                                                                        <textarea class="form-control" id="ed_ti_cred_soli" runat="server"></textarea>
                                                                                        <span class="help-inline">Tipo Crédito Solicitado</span>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-money"></i>
                                                                                        <textarea class="form-control" id="ed_mon_cred_soli" runat="server"></textarea>
                                                                                        <span class="help-inline">Monto Crédito Solicitado</span>
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="pull-right">
                                                        <div class="btn-group">
                                                            <asp:Button ID="Editar_cliente" ClientIDMode="Static" runat="server" OnClientClick="CARGANDO();" Style="color: white; right: 16%;" Class="btn btn-yellow" OnClick="Editar_cliente_Click" Text="Enviar Edición de cliente" />
                                                            <br />
                                                            <asp:Label runat="server" ID="l_h1" Style="color: red"></asp:Label>

                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="crear_cargando" runat="server" style="display: none; font-size: 3em;"></i>

                                        </div>
                                    </div>
                                </div>
                            </div>

                        </asp:View>
                        <%-- 2 DOC_ABIERTOS--%>

                        <asp:View ID="v_doc_abiertos" runat="server">
                            <div id="div6" runat="server" class="row" style="visibility: visible">
                                <div id="div7" runat="server" class="row" visible="true">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="box-title">
                                                <h3 runat="server" id="titulo"><i class="fa fa-table"></i>Documentos Abiertos</h3>
                                                <div class="box-tool">
                                                    <a data-action="collapse" href="#"></a>
                                                </div>
                                            </div>
                                            <div class="box-content">
                                                <div class="row">
                                                    <div class="col-sm-12">

                                                        <input type="text" id="t_filtro_memoria" style="width: 200px; margin-right: 7px; padding: 5px;" placeholder="Filtrar..." class="form-control" />
                                                        <br />
                                                        <div class="btn-toolbar pull-right" style="display: none;">
                                                            <asp:TextBox runat="server" ID="tx_enviar_" class="form-control"></asp:TextBox>

                                                            <div class="btn-group" style="display: none;">
                                                                <label runat="server" id="correo_env" style="color: red;"></label>
                                                                <asp:LinkButton ID="excel2" class="btn btn-circle show-tooltip fa fa-envelope" Visible="true" Style="margin-left: 5px" title="Correo" runat="server" OnClick="excel2_Click"></asp:LinkButton>
                                                            </div>
                                                        </div>


                                                        <div class="clearfix">
                                                            <div class="table-responsive" style="border: 0">
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div class="box">
                                                                            <div class="form-group" style="overflow-x: auto">
                                                                                <asp:GridView ID="g_doc" AutoGenerateColumns="false" CssClass="table table-bordered filtrar table-sm table-condensed condensed" runat="server" Visible="true"
                                                                                    ShowHeaderWhenEmpty="True" Font-Size="12px">
                                                                                    <HeaderStyle CssClass="test no-sort" />
                                                                                    <Columns>
                                                                                        <asp:BoundField HeaderText="Cliente" DataField="RutCliente" />
                                                                                        <asp:BoundField HeaderText="Nombre" DataField="NombCliente" />
                                                                                        <asp:BoundField HeaderText="Factura" DataField="Nº Factura" />
                                                                                        <asp:BoundField HeaderText="Vend." DataField="vendedor" />
                                                                                        <asp:BoundField HeaderText="Doc" DataField="TipoDocum" />
                                                                                        <asp:BoundField HeaderText="Descripción" DataField="Descripción" />
                                                                                        <asp:BoundField HeaderText="Nº Ref" DataField="NumRef" />
                                                                                        <asp:BoundField HeaderText="Fec. Tran" DataField="FechaTransc" />
                                                                                        <asp:BoundField HeaderText="Fec. Venc." DataField="FechaVenc" />
                                                                                        <asp:BoundField HeaderText="T.Cred" DataField="TipoCred" />
                                                                                        <asp:BoundField HeaderText="L.Credito" DataField="L.Crédito" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                                                                        <asp:BoundField HeaderText="L.Seguro" DataField="l_seguro" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                                                                        <asp:BoundField HeaderText="Monto Doc" DataField="Monto" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                                                                        <asp:BoundField HeaderText="Descuento" DataField="descuento" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                                                                        <asp:BoundField HeaderText="Saldo" DataField="Saldo" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
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
                                    </div>
                                </div>
                            </div>
                        </asp:View>

                    </asp:MultiView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- END Main Content -->
        <asp:TextBox ID="l_vendedores" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
        <a id="btn-scrollup" class="btn btn-circle btn-lg" href="#"></a>
    </form>


    <script src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script>window.jQuery || document.write('<script src="assets/jquery/jquery-2.1.1.min.js"><\/script>')</script>
    <script src="assets/bootstrap/js/bootstrap.min.js"></script>
    <script src="assets/jquery-slimscroll/jquery.slimscroll.min.js"></script>
    <script src="assets/jquery-cookie/jquery.cookie.js"></script>

    <script src="CHOSEN/chosen.jquery.js" type="text/javascript"></script>
    <script src="CHOSEN/docsupport/prism.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript">
        var config = {
            '.chosen-select': {},
            '.chosen-select-deselect': { allow_single_deselect: true },
            '.chosen-select-no-single': { disable_search_threshold: 10 },
            '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' },
            '.chosen-select-width': { width: "90%" }
        }
        for (var selector in config) {
            $(selector).chosen(config[selector]);
        }

    </script>


</body>
