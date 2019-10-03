<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ERROR.aspx.cs" Inherits="SoprodiApp.ERROR" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title>¡¡  !!</title>
    <meta name="description" content="">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <!-- Place favicon.ico and apple-touch-icon.png in the root directory -->

    <!--base css styles-->
    <link rel="stylesheet" href="assets/bootstrap/css/bootstrap.min.css">
    <link rel="stylesheet" href="assets/font-awesome/css/font-awesome.min.css">

    <!--page specific css styles-->

    <!--flaty css styles-->
    <link rel="stylesheet" href="css/flaty.css">
    <link rel="stylesheet" href="css/flaty-responsive.css">

    <link rel="shortcut icon" href="img/favicon.png">
</head>
<body class="error-page">

    <!-- BEGIN Main Content -->
    <div class="error-wrapper">
        <h5>Ha superado el tiempo de sesión <span>:(</span></h5>
        <p>Vuelva a ingresar al sistema o informe el evento <br />
           </p>
        <hr />
        <p class="clearfix">
            <a href="acceso.aspx" class="pull-left">← Página inicio</a>
        </p>
    </div>
    <!-- END Main Content -->

    <!--basic scripts-->
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script>window.jQuery || document.write('<script src="assets/jquery/jquery-2.1.1.min.js"><\/script>')</script>
    <script src="assets/bootstrap/js/bootstrap.min.js"></script>

</body>
</html>
