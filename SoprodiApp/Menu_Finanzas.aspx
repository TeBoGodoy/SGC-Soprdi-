<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Menu_Finanzas.aspx.cs" Inherits="SoprodiApp.Menu_Finanzas" %>


<%@ OutputCache Location="None" NoStore="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">

    <script>

        $(document).ready(function () {


        });
    </script>
    <%-- <div class="page-title" style="margin-top: -27px">
            <div>
                <div id="titulo" runat="server"></div>
                <h4></h4>
            </div>
        </div>--%>
    <div id="breadcrumbs">
        <ul class="breadcrumb" style="height: 40px !important;">
            <li>
                <i class="fa fa-home"></i>
                <a href="menu.aspx">Menú</a>
                <span class="divider"><i class="fa fa-angle-right"></i></span>
            </li>
            <li class="active" id="titulo2" runat="server"></li>
        </ul>
    </div>


    <div id="main-content">
        <div class="row">
            <div class="col-md-3" runat="server" id="uno_gestor">
                <a class="tile tile-red" data-stop="0" href="CALENDARIO.aspx">
                    <div class="img img-center">
                        <i class="fa fa-table fa-2x"></i>
                    </div>
                    <p class="title text-center">Calendario Cobranza</p>
                </a>

                <%-- <a class="tile tile-pink" href="CALENDARIO.aspx">
                    <p><i class="fa fa-table fa-3x pull-left fa-border"></i>Calendario Cobranza</p>
                </a>--%>
            </div>

            <div class="col-md-3" runat="server" id="Div1">
                <a class="tile tile-green" data-stop="0" href="COBRANZA_MANT_BANCO.aspx">
                    <div class="img img-center">
                        <i class="fa fa-bank fa-2x"></i>
                    </div>
                    <p class="title text-center">Mantenedor de Bancos</p>
                </a>

                <%--                <a class="tile tile-blue" href="COBRANZA_MANT_BANCO.aspx">
                    <p><i class="fa fa-bank fa-3x pull-left fa-border"></i>Mantenedor de Bancos</p>
                </a>--%>
            </div>

            <div class="col-md-3" runat="server" id="Div2">
                <a class="tile tile-blue" data-stop="0" href="COBRANZA_LISTADOS.aspx">
                    <div class="img img-center">
                        <i class="fa fa-file-o fa-2x"></i>
                    </div>
                    <p class="title text-center">Reportes Cobranza</p>
                </a>

                <%-- <a class="tile tile-red" href="COBRANZA_LISTADOS.aspx">
                    <p><i class="fa fa-bar-chart-o fa-3x pull-left fa-border"></i>Listados Cobranza</p>
                </a>--%>
            </div>

            <div class="col-md-3" runat="server" id="Div3">
                <a class="tile tile-brown" data-stop="0" href="MANT_CLIENTES.aspx">
                    <div class="img img-center">
                        <i class="fa fa-users fa-2x"></i>
                    </div>
                    <p class="title text-center">Mantenedor Clientes</p>
                </a>

                <%--                <a class="tile tile-lime" href="MANT_CLIENTES.aspx">
                    <p><i class="fa fa-users fa-3x pull-left fa-border"></i>Mantenedor Clie 95 ntes</p>
                </a>--%>
            </div>

            <div class="col-md-3" runat="server" id="Div4">
                <a class="tile tile-pink" data-stop="0" href="COBRANZA_REPORT.aspx">
                    <div class="img img-center">
                        <i class="fa fa-users fa-2x"></i>
                    </div>
                    <p class="title text-center">Reporte Flujo Cobranza</p>
                </a>

                <%--                <a class="tile tile-lime" href="MANT_CLIENTES.aspx">
                    <p><i class="fa fa-users fa-3x pull-left fa-border"></i>Mantenedor Clientes</p>
                </a>--%>
            </div>


            <div class="col-md-3" runat="server" id="Div5">
                <a class="tile tile-orange" data-stop="0" href="Comisionesventas.aspx">
                    <div class="img img-center">
                        <img src="img/file.png" />
                    </div>
                    <p class="title text-center">Comisiones</p>
                </a>
            </div>


        </div>
    </div>


</asp:Content>
