<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="REPORTES.aspx.cs" Inherits="SoprodiApp.REPORTES" %>


<%@ OutputCache Location="None" NoStore="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>



<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">
    <script>
   


    </script>

    <div id="main-content">





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


        <asp:TextBox ID="sw_permiso" runat="server" Style="visibility: visible; position: absolute;"></asp:TextBox>
        <div class="row">
            <div class="col-md-3" runat="server" id="uno_REPORTE_VENDEDOR">
                <a class="tile tile-pink" data-stop="0" href="REPORTE_VENDEDOR.aspx">
                    <div class="img img-center">
                        <img src="img/veneddor.png" />
                    </div>
                    <p class="title text-center">Reporte por vendedor</p>
                </a>

                <%--<a class="tile tile-green" href="REPORTE_VENDEDOR.aspx">
                    <p><i class="fa fa-table fa-3x pull-left fa-border"></i>Reporte de periodos, clientes y productos de un vendedor</p>
                </a>--%>
            </div>
            <div class="col-md-3" runat="server" id="dos_REPORTE_SALA">
                <a class="tile tile-orange" data-stop="0" href="REPORTE_SALA.aspx">
                    <div class="img img-center">
                        <img src="img/sala.png" />
                    </div>
                    <p class="title text-center">Reporte análisis por sala</p>
                </a>

                <%-- <a class="tile tile-yellow" href="REPORTE_SALA.aspx">
                    <p><i class="fa fa-shopping-cart fa-3x pull-left fa-border"></i>Reporte de totales por sala/clientes</p>
                </a>--%>
            </div>
            <div class="col-md-3" runat="server" id="tres_REPORTE_CM">
                <a class="tile tile-dark-blue" data-stop="0" href="REPORTE_CM.aspx">
                    <div class="img img-center">
                        <img src="img/file.png" />
                    </div>
                    <p class="title text-center">Análisis Clientes Nuevos</p>
                </a>

                <%--  <a class="tile tile-red" href="REPORTE_CM.aspx">
                    <p><i class="fa fa-columns fa-3x pull-left fa-border"></i>Clientes que no hayan presentado venta con anterioridad</p>
                </a>--%>
            </div>
            <div class="col-md-3" runat="server" id="cuatro_REPORTE_COMPARATIVO">
                <a class="tile tile-brown" data-stop="0" href="REPORTE_COMPARATIVO.aspx">
                    <div class="img img-center">
                        <img src="img/imagen2.png" />
                    </div>
                    <p class="title text-center">Reporte Comparativo</p>
                </a>

                <%-- <a class="tile tile-pink" href="REPORTE_COMPARATIVO.aspx">
                    <p><i class="fa fa-columns fa-3x pull-left fa-border"></i>Reporte comparativo de periodos con ventas repetidas y nuevas</p>
                </a>--%>
            </div>

            <div class="col-md-3" runat="server" id="cinco_REPORTE_LV_B">
                <a class="tile tile-red" data-stop="0" href="REPORTE_LV_B.aspx">
                    <div class="img img-center">
                        <img src="img/sala.png" />
                    </div>
                    <p class="title text-center">Reporte por bodega</p>
                </a>

                <%-- <a class="tile tile-orange" href="REPORTE_LV_B.aspx">
                    <p><i class="fa fa-table fa-3x pull-left fa-border"></i>Reporte de vendedores ultimo periodo por bodega</p>
                </a>--%>
            </div>
            <div class="col-md-3" runat="server" id="seis_REPORTE_LV_C">
                <a class="tile tile-light-blue" data-stop="0" href="REPORTE_LV_C.aspx">
                    <div class="img img-center">

                        <img src="img/veneddor.png" />
                    </div>
                    <p class="title text-center">Reporte por grupo</p>
                </a>

                <%--        <a class="tile tile-brown" href="REPORTE_LV_C.aspx">
                    <p><i class="fa fa-table fa-3x pull-left fa-border"></i>Reporte de vendedores ultimo periodo por grupo</p>
                </a>--%>
            </div>


            <div id="diario" runat="server" class="col-md-3">
                <a class="tile tile-magenta" data-stop="0" href="REPORTE_DIARIO.aspx">
                    <div class="img img-center">
                        <img src="img/imagen1.png" />
                    </div>
                    <p class="title text-center">Reporte Diario</p>
                </a>

                <%--  <a class="tile tile-blue" href="REPORTE_DIARIO.aspx">
                    <p><i class="fa fa-columns fa-3x pull-left fa-border"></i>Reporte diario según el usuario</p>
                </a>--%>
            </div>


            <div id="siete_REPORTE_LV_P" runat="server" class="col-md-3">
                <a class="tile tile-green" data-stop="0" href="REPORTE_LV_P.aspx">
                    <div class="img img-center">
                        <img src="img/carro_2.png" />
                    </div>
                    <p class="title text-center">Reporte análisis de productos</p>
                </a>

                <%--       <a class="tile tile-light-red" href="REPORTE_LV_P.aspx">
                    <p><i class="fa fa-columns fa-3x pull-left fa-border"></i>Reporte análisis de productos</p>
                </a>--%>
            </div>

            <div id="ocho_REPORTE_NC" class="col-md-3" runat="server">
                <a class="tile tile-brown" data-stop="0" href="REPORTE_NC.aspx">
                    <div class="img img-center">
                        <i class="fa fa-th-list fa-2x"></i>
                    </div>
                    <p class="title text-center">Listar Notas de Crédito</p>
                </a>

                <%--    <a class="tile tile-yellow" href="REPORTE_NC.aspx">
                    <p><i class="fa fa-th-list fa-3x pull-left fa-border"></i>Listar Notas de Crédito</p>
                </a>--%>
            </div>

            <div id="nueve_REPORTE_VEND_CLIE" class="col-md-3" runat="server">
                <a class="tile tile-lime" data-stop="0" href="REPORTE_VEND_CLIE.aspx">
                    <div class="img img-center">
                        <i class="fa fa-users fa-2x"></i>
                    </div>
                    <p class="title text-center">Vendedor - Cliente</p>
                </a>

                <%-- <a class="tile tile-magenta" href="REPORTE_VEND_CLIE.aspx">
                    <p><i class="fa fa-users fa-3x pull-left fa-border"></i>Vendedor - Cliente</p>
                </a>--%>
            </div>

            <div id="diez_REPORTE_LOG_CORR_FICH" class="col-md-3" runat="server">
                <a class="tile tile-orange" data-stop="0" href="REPORTE_LOG_CORR_FICH.aspx">
                    <div class="img img-center">
                        <i class="fa fa-list-alt fa-2x"></i>
                    </div>
                    <p class="title text-center">Log Correos Ficha</p>
                </a>

                <%--  <a class="tile tile-gray" href="REPORTE_LOG_CORR_FICH.aspx">
                    <p><i class="fa fa-list-alt fa-3x pull-left fa-border"></i>Log Correos Ficha</p>
                </a>--%>
            </div>
            <div id="once_REPORTE_LISTADO_DOC" class="col-md-3" runat="server">
                <a class="tile tile-gray" data-stop="0" href="REPORTE_LISTADO_DOC.aspx">
                    <div class="img img-center">
                        <i class="fa fa-files-o fa-2x"></i>
                    </div>
                    <p class="title text-center">Nº Documento</p>
                </a>

                <%--       <a class="tile tile-lime" href="REPORTE_LISTADO_DOC.aspx">
                    <p><i class="fa fa-list-alt fa-3x pull-left fa-border"></i>Nº Documento</p>
                </a>--%>
            </div>
            <div id="doce_REPORTE_STOCK" class="col-md-3" runat="server">
                <a class="tile tile-dark-yellow" data-stop="0" href="REPORTE_STOCK.aspx">
                    <div class="img img-center">
                        <i class="fa fa-briefcase fa-2x"></i>
                    </div>
                    <p class="title text-center">Stock</p>
                </a>

                <%--       <a class="tile tile-lime" href="REPORTE_LISTADO_DOC.aspx">
                    <p><i class="fa fa-list-alt fa-3x pull-left fa-border"></i>Nº Documento</p>
                </a>--%>
            </div>

            <div id="trece_REPORTE_EXCEL" class="col-md-3" runat="server">
                <a class="tile tile-green" data-stop="0" href="REPORTE_EXCEL_COSTOS.aspx">
                    <div class="img img-center">
                        <i class="fa fa-file-text fa-2x"></i>
                    </div>
                    <p class="title text-center">Listado de Precios</p>
                </a>

                <%--       <a class="tile tile-lime" href="REPORTE_LISTADO_DOC.aspx">
                    <p><i class="fa fa-list-alt fa-3x pull-left fa-border"></i>Nº Documento</p>
                </a>--%>
            </div>


            <div id="catorce_REPORTE_RENT_FACT" class="col-md-3" runat="server">
                <a class="tile tile-red" data-stop="0" href="REPORTE_RENT_FACT.aspx">
                    <div class="img img-center">
                        <i class="fa fa-briefcase fa-2x"></i>
                    </div>
                    <p class="title text-center">Productos Facturados</p>
                </a>

                <%--       <a class="tile tile-lime" href="REPORTE_LISTADO_DOC.aspx">
                    <p><i class="fa fa-list-alt fa-3x pull-left fa-border"></i>Nº Documento</p>
                </a>--%>
            </div>

            <div id="quince_REPORTE_MATRIZ" class="col-md-3" runat="server">
                <a class="tile tile-red" style="background-color: #290000 !important;" data-stop="0" href="REPORTE_MATRIZ.aspx">
                    <div class="img img-center">
                        <i class="fa fa-table fa-2x"></i>
                    </div>
                    <p class="title text-center">Matriz de Ventas</p>
                </a>

                <%--       <a class="tile tile-lime" href="REPORTE_LISTADO_DOC.aspx">
                    <p><i class="fa fa-list-alt fa-3x pull-left fa-border"></i>Nº Documento</p>
                </a>--%>
            </div>

            <div id="dieciseis_REPORTE_COMPRAS" class="col-md-3" runat="server">
                <a class="tile tile-pink" style="background-color: #519cde !important;" data-stop="0" href="REPORTE_COMPRA.aspx">
                    <div class="img img-center">
                        <img src="img/carro_2.png" />
                    </div>
                    <p class="title text-center">Compras </p>
                </a>

                <%--       <a class="tile tile-lime" href="REPORTE_LISTADO_DOC.aspx">
                    <p><i class="fa fa-list-alt fa-3x pull-left fa-border"></i>Nº Documento</p>
                </a>--%>
            </div>

            <div id="diecisiete_REPORTE_COSTOS_IMPORT" class="col-md-3" runat="server">
                <a class="tile tile-pink" style="background-color: #ff6161 !important;" data-stop="0" href="REPORTE_COSTOS_IMPORT.aspx">
                    <div class="img img-center">

                        <img src="img/imagen2.png" />

                    </div>
                    <p class="title text-center">Costos Importación </p>
                </a>

                <%--       <a class="tile tile-lime" href="REPORTE_LISTADO_DOC.aspx">
                    <p><i class="fa fa-list-alt fa-3x pull-left fa-border"></i>Nº Documento</p>
                </a>--%>
            </div>



            <div id="dieciocho_REPORTE_CATEGORIA" class="col-md-3" runat="server">
                <a class="tile tile-brown" style="background-color: #8666d4 !important;" data-stop="0" href="REPORTE_CATEGORIA.aspx">
                    <div class="img img-center">
                        <i class="fa fa-list-alt fa-2x"></i>
                    </div>


                    <p class="title text-center">Categoría </p>
                </a>

                <%--       <a class="tile tile-lime" href="REPORTE_LISTADO_DOC.aspx">
                    <p><i class="fa fa-list-alt fa-3x pull-left fa-border"></i>Nº Documento</p>
                </a>--%>
            </div>

            <div id="diecinueve_REPORTE_SP" class="col-md-3" runat="server">
                <a class="tile tile-brown" style="background-color: #1fa67a !important;" data-stop="0" href="REPORTE_SP.aspx?G=NO">
                    <div class="img img-center">
                        <i class="fa fa-link fa-2x"></i>
                    </div>


                    <p class="title text-center">SP </p>
                </a>

                <%--       <a class="tile tile-lime" href="REPORTE_LISTADO_DOC.aspx">
                    <p><i class="fa fa-list-alt fa-3x pull-left fa-border"></i>Nº Documento</p>
                </a>--%>
            </div>


            <div id="veinti_tres_REPORTE_SALDOS_SP" class="col-md-3" runat="server">
                <a class="tile tile-brown" style="background-color: #6eb158 !important;" data-stop="0" href="REPORTE_SALDOS_SP.aspx?G=NO">
                    <div class="img img-center">
                        <i class="fa fa-link fa-2x"></i>
                    </div>


                    <p class="title text-center">Saldos SP </p>
                </a>

                <%--       <a class="tile tile-lime" href="REPORTE_LISTADO_DOC.aspx">
                    <p><i class="fa fa-list-alt fa-3x pull-left fa-border"></i>Nº Documento</p>
                </a>--%>
            </div>


            <div id="veinte_REPORTE_STOCKF" class="col-md-3" runat="server">
                <a class="tile tile-brown" style="background-color: #eaca4c !important;" data-stop="0" href="REPORTE_STOCKF.aspx">
                    <div class="img img-center">
                        <i class="fa fa-link fa-2x"></i>
                    </div>


                    <p class="title text-center">stock/sp </p>
                </a>

                <%--       <a class="tile tile-lime" href="REPORTE_LISTADO_DOC.aspx">
                    <p><i class="fa fa-list-alt fa-3x pull-left fa-border"></i>Nº Documento</p>
                </a>--%>
            </div>

            <div id="veinti_uno_Menu_Planificador" class="col-md-3" runat="server">
                <a class="tile tile-yellow" style="background-color: #b200ff !important;" data-stop="0" href="Menu_Planificador.aspx">
                    <div class="img img-center">
                        <i class="fa fa-truck fa-2x"></i>
                    </div>


                    <p class="title text-center">Planificador de Despachos </p>
                </a>

                <%--       <a class="tile tile-lime" href="REPORTE_LISTADO_DOC.aspx">
                    <p><i class="fa fa-list-alt fa-3x pull-left fa-border"></i>Nº Documento</p>
                </a>--%>
            </div>

            <div id="veinti_dos_produc" class="col-md-3" runat="server">
                <a class="tile tile-yellow" style="background-color: #271fa6 !important;" data-stop="0" href="REPORTE_PRODUC_COST.aspx">
                    <div class="img img-center">

                        <img src="img/carro_2.png" />
                    </div>

                    <p class="title text-center">Costo de Producto </p>
                </a>

                <%--       <a class="tile tile-lime" href="REPORTE_LISTADO_DOC.aspx">
                    <p><i class="fa fa-list-alt fa-3x pull-left fa-border"></i>Nº Documento</p>
                </a>--%>
            </div>


            <div id="veinti_cuatro_stock_log" class="col-md-3" runat="server">
                <a class="tile tile-yellow" style="background-color: #86611e !important;" data-stop="0" href="REPORTE_MOV_STOCK.aspx">
                    <div class="img img-center">

                        <i class="fa fa-th-list fa-2x"></i>
                    </div>

                    <p class="title text-center">Movimientos Stock </p>
                </a>

                <%--       <a class="tile tile-lime" href="REPORTE_LISTADO_DOC.aspx">
                    <p><i class="fa fa-list-alt fa-3x pull-left fa-border"></i>Nº Documento</p>
                </a>--%>
            </div>
        </div>
    </div>


</asp:Content>
