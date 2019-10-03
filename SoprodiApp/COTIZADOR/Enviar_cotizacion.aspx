<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Enviar_cotizacion.aspx.cs" Inherits="CRM.Enviar_cotizacion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui">
    <meta name="description" content="">
    <meta name="keywords" content="">
    <meta name="author" content="Thinxsys">
    <title>COTIZADOR | SOPRODI</title>
    <link rel="apple-touch-icon" href="app-assets/images/ico/apple-icon-120.png">
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/ico/favicon.ico">
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,300i,400,400i,600,600i,700,700i%7CMuli:300,400,500,700" rel="stylesheet">
    <!-- BEGIN VENDOR CSS-->
    <link rel="stylesheet" type="text/css" href="app-assets/css/vendors.css">
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/css/tables/datatable/datatables.min.css">
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/css/extensions/toastr.css">
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/css/forms/selects/selectize.css">
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/css/forms/selects/selectize.default.css">
    <!-- END VENDOR CSS-->
    <!-- BEGIN ROBUST CSS-->
    <link rel="stylesheet" type="text/css" href="app-assets/css/app.css">
    <!-- END ROBUST CSS-->
    <!-- BEGIN Page Level CSS-->
    <link rel="stylesheet" type="text/css" href="app-assets/css/core/menu/menu-types/vertical-content-menu.css">
    <link rel="stylesheet" type="text/css" href="app-assets/css/core/colors/palette-gradient.css">
    <link rel="stylesheet" type="text/css" href="app-assets/fonts/simple-line-icons/style.min.css">
    <link rel="stylesheet" type="text/css" href="app-assets/css/plugins/loaders/loaders.min.css">
    <link rel="stylesheet" type="text/css" href="app-assets/css/core/colors/palette-loader.css">
    <link href="FRAMEWORK/Chosen/chosen.min.css" rel="stylesheet" />
    <!-- END Page Level CSS-->

    <style>
        #relojito {
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: rgba(255, 255, 255, 0.7);
            background: rgba(255, 255, 255, 0.7);
            color: rgba(255, 255, 255, 0.7);
            /* change if the mask should have another color then white */
            z-index: 9999;
            /* makes sure it stays on top */
        }

        #relojito3 {
            width: 200px;
            height: 200px;
            position: absolute;
            left: 50%;
            /* centers the loading animation horizontally one the screen */
            top: 60%;
            /* centers the loading animation vertically one the screen */
            margin: -100px 0 0 -100px;
            /* is width and height divided by two */
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="app-content content">
            <div class="content-wrapper">
                <div class="content-body">
                    <div>
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="card">
                                    <div class="card-header">
                                    </div>
                                    <div class="card-content">
                                        <div class="card-body">
                                            <div runat="server" id="div_bueno" visible="false">
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

        <div runat="server" id="div_malo" visible="false">
            <h4>Error al enviar cotización</h4>
        </div>
    </form>
</body>
      <!-- BEGIN VENDOR JS-->
    <script src="app-assets/vendors/js/vendors.min.js"></script>
    <!-- BEGIN VENDOR JS-->
    <!-- BEGIN PAGE VENDOR JS-->
    <script src="app-assets/vendors/js/ui/jquery.sticky.js"></script>
    <script src="app-assets/vendors/js/charts/jquery.sparkline.min.js"></script>
    <script src="app-assets/vendors/js/ui/headroom.min.js"></script>
    <script src="app-assets/vendors/js/tables/datatable/datatables.min.js"></script>
    <script src="app-assets/vendors/js/forms/toggle/bootstrap-switch.min.js"></script>
    <script src="app-assets/vendors/js/extensions/toastr.min.js"></script>


    <!-- END PAGE VENDOR JS-->
    <!-- BEGIN ROBUST JS-->
    <script src="app-assets/js/core/app-menu.js"></script>
    <script src="app-assets/js/core/app.js"></script>
    <!-- END ROBUST JS-->
    <!-- BEGIN PAGE LEVEL JS-->
    <script src="app-assets/js/scripts/ui/breadcrumbs-with-stats.js"></script>
    <script src="app-assets/vendors/js/forms/tags/form-field.js"></script>
    <script src="app-assets/js/scripts/tables/datatables/datatable-basic.js"></script>
    <script src="app-assets/vendors/js/forms/select/selectize.min.js"></script>
    <script src="FRAMEWORK/Chosen/chosen.jquery.min.js"></script>
</html>
