<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Menu_Comisiones.aspx.cs" Inherits="SoprodiApp.Menu_Comisiones" %>


<%@ OutputCache Location="None" NoStore="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">

    <script>

        $(document).ready(function () {


        });
    </script>
 
    <div id="breadcrumbs">
        <ul class="breadcrumb" style="height: 40px !important;">
            <li>
                <i class="fa fa-home"></i>
                <a href="menu.aspx">Menú</a>
                <span class="divider"><i class="fa fa-angle-right"></i></span>
            </li>
            <li>
                <i class="fa fa-home"></i>
                <a href="menu_finanzas.aspx">Finanzas</a>
                <span class="divider"><i class="fa fa-angle-right"></i></span>
            </li>
            <li class="active" id="titulo2" runat="server"></li>
        </ul>
    </div>


    <div id="main-content">
        <div class="row">
            <div class="col-md-3" runat="server" id="uno_gestor">
                <a class="tile tile-green" data-stop="0" href="Comisionesventas.aspx">
                    <div class="img img-center">
                        <i class="fa fa-table fa-2x"></i>
                    </div>
                    <p class="title text-center">Comisiones</p>
                </a>

      
            </div>

            <div class="col-md-3" runat="server" id="Div3" style="visibility:hidden;position:absolute;">
                <a class="tile tile-brown" data-stop="0" href="COMISIONES_REGLA.aspx">
                    <div class="img img-center">

                        <i class="fa fa-file-o fa-2x"></i>
                    </div>
                    <p class="title text-center">Reglas</p>
                </a>

   
            </div>


            <div class="col-md-3" runat="server" id="Div1"  style="visibility:hidden;position:absolute;">
                <a class="tile tile-red" data-stop="0" href="COMISIONES_VENDEDORES.aspx">
                    <div class="img img-center">

                        <i class="fa fa-users fa-2x"></i>
                    </div>
                    <p class="title text-center">Vendedores</p>
                </a>


            </div>

            <div class="col-md-3" runat="server" id="Div2"  style="visibility:hidden;position:absolute;">
                <a class="tile tile-blue" data-stop="0" href="COBRANZA_LISTADOS.aspx">
                    <div class="img img-center">
                        <i class="fa fa-file-o fa-2x"></i>
                    </div>
                    <p class="title text-center">Materias Prima (1)</p>
                </a>


            </div>



            <div class="col-md-3" runat="server" id="Div4"  style="visibility:hidden;position:absolute;">
                <a class="tile tile-yellow" data-stop="0" href="COBRANZA_REPORT.aspx">
                    <div class="img img-center">
                        <i class="fa fa-users fa-2x"></i>
                    </div>
                    <p class="title text-center">Clientes Compartidos (4)</p>
                </a>

            </div>




            <%--            <div class="col-md-3" runat="server" id="Div5">
                <a class="tile tile-pink" data-stop="0" href="COMISIONES.aspx">
                    <div class="img img-center">
                        <img src="img/file.png" />
                    </div>
                    <p class="title text-center">Comisiones</p>
                </a>
            </div>--%>
        </div>
    </div>


</asp:Content>
