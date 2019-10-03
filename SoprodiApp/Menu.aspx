<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="SoprodiApp.Menu" %>

<%@ Register Assembly="SessionTimeoutControl" Namespace="PAB.WebControls" TagPrefix="cc1" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>


<%@ OutputCache Location="None" NoStore="true" %>


<html xmlns="http://www.w3.org/1999/xhtml">


<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

    <meta http-equiv="X-UA-Compatible" content="IE=10; IE=9; IE=8; IE=7; IE=EDGE" />

    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="0">

    <title>Soprodi - Reportes</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <!-- Place favicon.ico and apple-touch-icon.png in the root directory -->
    <link href="img/icono.ico" rel="shortcut icon">
    <!--base css styles-->
    <link rel="stylesheet" href="assets/bootstrap/css/bootstrap.min.css">
    <link rel="stylesheet" href="assets/font-awesome/css/font-awesome.min.css">

    <!--page specific css styles-->

    <link href="css/tooltipster.css" rel="stylesheet" />
    <link rel="stylesheet" href="CHOSEN/docsupport/style.css">
    <link rel="stylesheet" href="CHOSEN/docsupport/prism.css">
    <link rel="stylesheet" href="CHOSEN/chosen.css">

    <link rel="stylesheet" href="assets/gritter/css/jquery.gritter.css">

    <!--base css styles-->

    <link rel="stylesheet" href="assets/bootstrap/css/bootstrap.min.css">
    <link rel="stylesheet" href="assets/font-awesome/css/font-awesome.min.css">

    <!--page specific css styles-->

    <!--flaty css styles-->
    <link rel="stylesheet" href="css/flaty.css">
    <link rel="stylesheet" href="css/flaty-responsive.css">

    <script type="text/javascript">


</script>

</head>



<body>



    <!-- END Theme Setting -->
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <%--   <script src="js/jquery-ui-1.8.24/ui/jquery-ui.js"></script>
    <script src="js/jquery-ui-1.8.24/jquery-1.8.2.js"></script>--%>

    <!-- BEGIN Navbar -->
    <%--<form id="Form1" runat="server">--%>

    <cc1:SessionTimeoutControl ID="SessionTimeoutControl1" runat="server" RedirectUrl="Acceso.aspx">
    </cc1:SessionTimeoutControl>

    <script>

        $().ready(function () {

            var elem2 = document.getElementById("sidebar");

            elem2.style.display = "none !important";

            var elem3 = document.getElementById("main-content");

            elem3.style = "margin-left: 0px; !important";


        });


    </script>
    <form id="Form2" runat="server">


        <div id="navbar" class="navbar">
            <button type="button" class="navbar-toggle navbar-btn collapsed" data-toggle="collapse" data-target="#sidebar">
                <span class="fa fa-bars"></span>
            </button>
            <a class="navbar-brand" href="MENU.aspx">
                <small>
                    <i class="fa fa-desktop"></i>
                    Soprodi - App Web
                </small>
            </a>

            <!-- BEGIN Navbar Buttons -->

            <ul class="nav flaty-nav pull-right">
                <!-- BEGIN Button Tasks -->
                <li class="user-profile">
                    <a data-toggle="dropdown" href="#" class="user-menu dropdown-toggle">
                        <i class="fa fa-user"></i>
                        <span class="hhh" id="user_info">

                            <asp:LoginName ID="LoginName1" runat="server" />

                        </span>
                        <i class="fa fa-caret-down"></i>
                    </a>
                    <!-- BEGIN User Dropdown -->
                    <ul class="dropdown-menu dropdown-navbar" id="user_menu">


                        <li class="divider visible-xs"></li>
                        <li class=""></li>
                        <li>
                            <a href="REPORTE_CAMBIO_PASS.aspx">Cambiar Contraseña.
                            </a>
                        </li>

                        <li>
                            <asp:LoginStatus ID="LoginStatus1" runat="server" />

                        </li>

                    </ul>
                    <!-- BEGIN User Dropdown -->
                </li>

                <!-- END Button User -->
            </ul>

            <!-- END Navbar Buttons -->
        </div>
        <!-- END Navbar -->

        <div id="main-content">


            <div class="page-title">
                <div>
                    <h1><i class="fa fa-file-o"></i>Menú</h1>
                    <h4></h4>
                </div>
            </div>

            <div class="row" style="margin-top: 50px;">
                <div class="col-md-3" runat="server" id="uno_abarrote">
                    <a class="tile tile-pink" data-stop="0" onclick="abarrote();" href="REPORTES.aspx?s=1">
                        <div class="img img-center">
                            <img src="img/carro_2.png" />
                        </div>
                        <p class="title text-center">Abarrotes</p>
                    </a>

                    <%--  <a class="tile tile-green" onclick="abarrote();" href="REPORTES.aspx?s=1">
                        <div class="img img-center">
                            <img src="img/carro_2.png" />
                        </div>
                        <p class="title text-center">Abarrotes</p>
                    </a>--%>
                </div>
                <div class="col-md-3" runat="server" id="dos_grano">
                    <a class="tile tile-orange" data-stop="0" onclick="grano();" href="REPORTES.aspx?s=2">
                        <div class="img img-center">
                            <img src="img/sala.png" />
                        </div>
                        <p class="title text-center">Granos</p>
                    </a>

                    <%--                    <a class="tile tile-yellow" onclick="grano();" href="REPORTES.aspx?s=2">
                        <div class="img img-center">
                            <img src="img/sala.png" />
                        </div>
                        <p class="title text-center">Grano</p>
                    </a>--%>
                </div>
                <div class="col-md-3" runat="server" id="tres_finanzas">
                    <a class="tile tile-dark-blue" data-stop="0" onclick="informes();" href="MENU_FINANZAS.aspx">
                        <div class="img img-center">
                            <img src="img/file.png" />
                        </div>
                        <p class="title text-center">Finanzas</p>
                    </a>

                    <%--    <a class="tile tile-red" onclick="informes();" href="MENU_FINANZAS.aspx">
                        <div class="img img-center">
                            <img src="img/file.png" />
                        </div>
                        <p class="title text-center">Finanzas</p>
                    </a>--%>
                </div>
                <div class="col-md-3" runat="server" id="cuatro_gestor">
                    <a class="tile tile-red" data-stop="0" href="MENU_GESTOR.aspx">
                        <div class="img img-center">
                            <i class="fa fa-file-o fa-2x"></i>
                        </div>
                        <p class="title text-center">Gestor Documental</p>
                    </a>

                    <%--  <a class="tile tile-blue" href="MENU_GESTOR.aspx">
                        <div class="img img-center">
                            <i class="fa fa-file-o fa-2x"></i>
                        </div>
                        <p class="title text-center">Gestor Documental</p>
                    </a>--%>
                </div>

                <div class="col-md-3" runat="server" id="cinco_usuarios">
                    <a class="tile tile-brown" data-stop="0" href="REPORTE_USUARIOS.aspx">
                        <div class="img img-center">
                            <img src="img/veneddor.png" />
                        </div>
                        <p class="title text-center">Administración usuarios</p>
                    </a>

                    <%--                    <a class="tile tile-pink" href="REPORTE_USUARIOS.aspx">
                        <div class="img img-center">
                            <img src="img/veneddor.png" />
                        </div>
                        <p class="title text-center">Administración usuarios</p>
                    </a>--%>
                </div>

                <div class="col-md-3" runat="server" id="seis_ficha_cliente">
                    <a class="tile tile-cyan" data-stop="0" href="FICHA_CLIENTE.aspx">
                        <div class="img img-center">
                            <img src="img/clientes.png" />
                        </div>
                        <p class="title text-center">Ficha Clientes</p>
                    </a>

                    <%--  <a class="tile tile-brown" href="FICHA_CLIENTE.aspx">
                        <div class="img img-center">
                            <img src="img/clientes.png" />
                        </div>
                        <p class="title text-center">Ficha Clientes</p>
                    </a>--%>
                </div>
                <div class="col-md-3" runat="server" id="siete_nacho">
                    <a class="tile tile-green" data-stop="0" href="REPORTE_COBRO_BODEGA.aspx">
                        <div class="img img-center">
                            <img src="img/clientes.png" />
                        </div>
                        <p class="title text-center">Cobro sobrealmacenaje</p>
                    </a>

                    <%--  <a class="tile tile-brown" href="FICHA_CLIENTE.aspx">
                        <div class="img img-center">
                            <img src="img/clientes.png" />
                        </div>
                        <p class="title text-center">Ficha Clientes</p>
                    </a>--%>
                </div>
                <div class="col-md-3" runat="server" id="ocho_gama">
                    <a class="tile tile-pink" data-stop="0" href="REPORTE_COMPRA.aspx">
                        <div class="img img-center">
                            <img src="img/carro_2.png" />
                        </div>
                        <p class="title text-center">Compras</p>
                    </a>

                    <%--  <a class="tile tile-brown" href="FICHA_CLIENTE.aspx">
                        <div class="img img-center">
                            <img src="img/clientes.png" />
                        </div>
                        <p class="title text-center">Ficha Clientes</p>
                    </a>--%>
                </div>
                <div class="col-md-3" runat="server" id="nueve_permanencia">
                    <a class="tile tile-brown" data-stop="0" href="REPORTE_PERMANENCIA.aspx">
                        <div class="img img-center">
                            <i class="fa fa-file-o fa-2x"></i>
                        </div>
                        <p class="title text-center">Permanencia</p>
                    </a>

                    <%--  <a class="tile tile-brown" href="FICHA_CLIENTE.aspx">
                        <div class="img img-center">
                            <img src="img/clientes.png" />
                        </div>
                        <p class="title text-center">Ficha Clientes</p>
                    </a>--%>
                </div>


                <div class="col-md-3" runat="server" id="diez_vencimientos">
                    <a class="tile tile-cyan" style="background-color: #eaca4c !important;" data-stop="0" href="REPORTE_VENCIMIENTOS.aspx">
                        <div class="img img-center">
                            <i class="fa fa-file-o fa-2x"></i>
                        </div>
                        <p class="title text-center">Vencimientos</p>
                    </a>
                </div>
                <div id="once_planificador" class="col-md-3" runat="server">
                    <a class="tile tile-yellow" style="background-color: #b200ff !important;" data-stop="0" href="Menu_Planificador.aspx?s=4">
                        <div class="img img-center">
                            <i class="fa fa-truck fa-2x"></i>
                        </div>
                        <p class="title text-center">Planificador de Despachos </p>
                    </a>
                </div>
                <div class="col-md-3" runat="server" id="doce_cotizador">
                    <a class="tile tile-green" style="background-color: #eaca4c !important;" data-stop="0" href="COTIZADOR/dashboard_vendedor.aspx">
                        <div class="img img-center">
                            <i class="fa fa-file-o fa-2x"></i>
                        </div>
                        <p class="title text-center">Cotizador</p>
                    </a>
                </div>
                 <div class="col-md-3" runat="server" id="trece_admin_cotizador_">
                    <a class="tile tile-green" style="background-color: #b200ff !important;" data-stop="0" href="COTIZADOR/Dashboard_admin.aspx">
                        <div class="img img-center">
                            <i class="fa fa-dashboard fa-2x"></i>
                        </div>
                        <p class="title text-center">Administrador Cotizaciones</p>
                    </a>
                </div>
            </div>
            <footer>
                <p style="margin-left: 23%;">2019 © Soprodi - Thinxsys.</p>
            </footer>
    </form>
    <script type="text/javascript">
        function RegistrarTriggers(control) {
            Sys.WebForms.PageRequestManager.getInstance()._updateControls(['tUpdatePanel1'], [], [control.id], 90);
        }
    </script>
    <script type="text/javascript">
        function msg_agrega() {
            bootbox.alert("Carga realizada correctamente");
        }
    </script>
    <script type="text/javascript">
        function msg_agrega2() {
            bootbox.alert("Carga realizada");
        }
    </script>
    <script type="text/javascript">
        function msg_agrega_no() {
            bootbox.alert("Carga realizada incorrectamente!");
        }
    </script>
    <script type="text/javascript">
        function msg_elimina() {
            bootbox.alert("Eliminación correcta!");
        }
    </script>
    <script type="text/javascript">
        function error_pass() {
            bootbox.alert("Error en su actual contraseña!");
        }
    </script>
    <script type="text/javascript">
        function guardada() {
            bootbox.alert("Contraseña guardada con exito!");
        }
    </script>
    <script type="text/javascript">
        function msg_falta_informacion() {
            bootbox.alert("ALERTA: Falta información, complete los datos.");
        }
    </script>
    <script type="text/javascript">
        function msg_email() {
            bootbox.alert("Correo enviado exitosamente!");
        }
    </script>
    <script type="text/javascript">
        function msg_email_false() {
            bootbox.alert("Correo no fue enviado: (Es posible que la contraseña no sea la correcta)");
        }
    </script>
    <script type="text/javascript">
        function msg_validar_no() {
            bootbox.alert("Validación incorrecta :(");
        }
    </script>
    <script type="text/javascript">
        function msg_validar() {
            bootbox.alert("Validación correcta :)");
        }
    </script>
    <script type="text/javascript">
        function msg_vacio() {
            bootbox.alert("0 archivos para envíar un correo, al menos debe seleccionar 1. (>.<)'");
        }
    </script>
    <script type="text/javascript">
        function msg_vacio_prezarpe() {
            bootbox.alert("No existen archivos Pre-Zarpe para notificar");
        }
    </script>
    <script type="text/javascript">
        function msg_vacio_postzarpe() {
            bootbox.alert("No existen archivos Post-Zarpe para notificar");
        }
    </script>
    <script type="text/javascript">
        function msg_externo() {
            bootbox.alert("Tu cuenta no es válida para acceder a gestión de costos de las agencias de aduana.");
        }
    </script>
    <script type="text/javascript">
        function msg_guardar() {
            bootbox.alert("Datos guardados correctamente.");
        }
    </script>
    <script type="text/javascript">
        function msg_guardar_no() {
            bootbox.alert("Datos guardados incorrectamente :(");
        }
    </script>
    <script type="text/javascript">
        function msg_abre() {
            bootbox.alert("Estado cambiado a Incompleto");
        }
    </script>
    <script type="text/javascript">
        function msg_cierra() {
            bootbox.alert("Estado cambiado a finalizado");
        }
    </script>
    <script type="text/javascript">
        function msg_no() {
            bootbox.alert("Estado cambiado a No se Embarca");
        }
    </script>
    <script type="text/javascript">
        function msg_usuario_existe() {
            bootbox.alert("Usuario(Correo) ya existe!! Intente nuevamente");
        }
    </script>
    <script type="text/javascript">
        function msg_mensaje_eliminar() {
            bootbox.prompt("¿Motivo de la eliminación?", function (result) {
                if (result) {
                    __doPostBack('__EVENTTARGET', '__EVENTARGUMENT');
                }
            });
        }
    </script>
    </div>
    <!--basic scripts-->
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script>window.jQuery || document.write('<script src="assets/jquery/jquery-2.1.1.min.js"><\/script>')</script>
    <script src="assets/bootstrap/js/bootstrap.min.js"></script>
    <script src="assets/jquery-slimscroll/jquery.slimscroll.min.js"></script>
    <script src="assets/jquery-cookie/jquery.cookie.js"></script>

    <!--page specific plugin scripts-->
    <script src="assets/flot/jquery.flot.js"></script>
    <script src="assets/flot/jquery.flot.resize.js"></script>
    <script src="assets/flot/jquery.flot.pie.js"></script>
    <script src="assets/flot/jquery.flot.stack.js"></script>
    <script src="assets/flot/jquery.flot.crosshair.js"></script>
    <script src="assets/flot/jquery.flot.tooltip.min.js"></script>

    <link rel="stylesheet" type="text/css" href="assets/bootstrap-fileupload/bootstrap-fileupload.css" />
    <script src="assets/sparkline/jquery.sparkline.min.js"></script>

    <script src="assets/gritter/js/jquery.gritter.js"></script>
    <script src="js/jquery.tooltipster.js"></script>

    <script src="js/jquery.tooltipster.min.js"></script>

    <!--flaty scripts-->
    <script src="js/flaty.js"></script>
    <script src="js/flaty-demo-codes.js"></script>

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
    <script src="js/tablesort.js"></script>
    <script src="js/prettify.js"></script>

    <script src='js/sorts/tablesort.date.js'></script>
    <script src='js/sorts/tablesort.dotsep.js'></script>
    <script src='js/sorts/tablesort.filesize.js'></script>
    <script src='js/sorts/tablesort.numeric.js'></script>

</body>
</html>
