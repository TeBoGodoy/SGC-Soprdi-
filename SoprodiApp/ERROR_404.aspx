﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ERROR_404.aspx.cs" Inherits="SoprodiApp.ERROR_404" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title>Página no encontrada</title>
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
        <h4>Página no encontrada<span>404</span></h4>
        <p>Oops! Página no ha sido encontrada.<br />
            ¿Hay algo malo con la URL?.</p>
        <br />
        <form action="index.html" method="post">
            <div class="form-group">

            </div>
        </form>
        <hr />
        <p class="clearfix">
            <a href="javascript:history.back()" class="pull-left">← Página anterior</a>

        </p>

    </div>

    <!-- END Main Content -->

    <!--basic scripts-->
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script>window.jQuery || document.write('<script src="assets/jquery/jquery-2.1.1.min.js"><\/script>')</script>
    <script src="assets/bootstrap/js/bootstrap.min.js"></script>

</body>
</html>
