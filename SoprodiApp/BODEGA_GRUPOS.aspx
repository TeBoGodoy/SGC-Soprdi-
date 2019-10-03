<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="BODEGA_GRUPOS.aspx.cs" Inherits="SoprodiApp.BODEGA_GRUPO" %>


<%@ OutputCache Location="None" NoStore="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">

    <script>

        $(document).ready(function () {

     
        });
    </script>

    <div id="main-content">
        <div class="row">
            <div class="col-md-3">
                <a class="tile tile-red" data-stop="0" href="REPORTE_LV_B.aspx">
                    <div class="img img-center">
                        <img src="img/sala.png" />
                    </div>
                    <p class="title text-center">Reporte por bodega</p>
                </a>

                <a class="tile tile-pink" href="REPORTE_LV_B.aspx">
                    <p><i class="fa fa-table fa-3x pull-left fa-border"></i>Reporte de vendedores ultimo periodo por bodega</p>
                </a>
            </div>
            <div class="col-md-3">
                <a class="tile tile-orange" data-stop="0" href="REPORTE_LV_C.aspx">
                    <div class="img img-center">

                        <img src="img/veneddor.png" />
                    </div>
                    <p class="title text-center">Reporte por grupo</p>
                </a>

                <a class="tile tile-brown" href="REPORTE_LV_C.aspx">
                    <p><i class="fa fa-shopping-cart fa-3x pull-left fa-border"></i>Reporte de vendedores ultimo periodo por grupo</p>
                </a>
            </div>
           
        </div>
    </div>


</asp:Content>
