<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Menu_Planificador.aspx.cs" Inherits="SoprodiApp.Menu_Planificador" %>


<%@ OutputCache Location="None" NoStore="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">

    <script>

        $(document).ready(function () {


        });
    </script>
    <%--<div class="page-title" style="margin-top: -27px">
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
            <li>
                <a runat="server" id="A1"></a>
                <span class="divider"><i class="fa fa-angle-right"></i></span>
            </li>
            <li class="active" id="titulo2" runat="server"></li>
        </ul>
        <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_gif" runat="server" style="display: none; font-size: 3em;"></i>

    </div>



    <div id="main-content">
        <div class="row">
            <div class="col-md-3" runat="server" id="uno_gestor">
                <a class="tile tile-red" data-stop="0" href="REPORTE_SP_SELECT.aspx?G=NO">
                    <div class="img img-center">
                        <img src="img/file.png" />
                    </div>
                    <p class="title text-center">Elegir SP</p>
                </a>

                <%--                <a class="tile tile-pink" href="GESTOR_DOCUMENTAL.aspx">
                    <p><i class="fa fa-table fa-3x pull-left fa-border"></i>Gestor Documental</p>
                </a>--%>
            </div>
            <div class="col-md-3" runat="server" id="dos_visor">
                <a class="tile tile-orange" data-stop="0" href="REPORTE_CASIGNADOS.aspx">
                    <div class="img img-center">
                        <i class="fa fa-truck" aria-hidden="true"></i>
                    </div>
                    <p class="title text-center">SP Asignados</p>
                </a>

                <%--   <a class="tile tile-cyan" href="VISOR_DOCUMENTAL.aspx">
                    <p><i class="fa fa-columns fa-3x pull-left fa-border"></i>Visor Documental</p>
                </a>--%>
            </div>

            <div class="col-md-3" runat="server" id="instruir">
                <a class="tile tile-pink" data-stop="0" href="REPORTE_INSTRUIR.aspx">
                    <div class="img img-center">
                        <i class="fa fa-truck" aria-hidden="true"></i>

                    </div>
                    <p class="title text-center">Instruir</p>
                </a>

                <%--   <a class="tile tile-cyan" href="VISOR_DOCUMENTAL.aspx">
                    <p><i class="fa fa-columns fa-3x pull-left fa-border"></i>Visor Documental</p>
                </a>--%>
            </div>


            <div class="col-md-3" runat="server" id="mant_trans">
                <a class="tile tile-green" data-stop="0" href="MANT_TRANSPORT.aspx">
                    <div class="img img-center">
                        <i class="fa fa-wrench" aria-hidden="true"></i>

                    </div>
                    <p class="title text-center">Mantenedor Transporte</p>
                </a>

                <%--  <a class="tile tile-yellow" href="LOG_DOCUMENTAL.aspx">
                    <p><i class="fa fa-table fa-3x pull-left fa-border"></i>Log Documental</p>
                </a>--%>
            </div>


            <div class="col-md-3" runat="server" id="mant_chofer">
                <a class="tile tile-red" data-stop="0" href="MANT_CHOFER.aspx">
                    <div class="img img-center">
                        <i class="fa fa-wrench" aria-hidden="true"></i>

                    </div>
                    <p class="title text-center">Mantenedor Chofer</p>
                </a>

                <%-- <a class="tile tile-magenta" href="USUARIO_PRODUCTO.aspx">
                    <p><i class="fa fa-columns fa-3x pull-left fa-border"></i>Usuario Producto</p>
                </a>--%>
            </div>
            <div class="col-md-3" runat="server" id="mant_camion">
                <a class="tile tile-yellow" data-stop="0" href="MANT_CAMION.aspx">
                    <div class="img img-center">
                        <i class="fa fa-wrench" aria-hidden="true"></i>

                    </div>
                    <p class="title text-center">Mantenedor Camion</p>
                </a>

                <%-- <a class="tile tile-magenta" href="USUARIO_PRODUCTO.aspx">
                    <p><i class="fa fa-columns fa-3x pull-left fa-border"></i>Usuario Producto</p>
                </a>--%>
            </div>


            <div class="col-md-3" runat="server" id="mant_correo">
                <a class="tile tile-brown" data-stop="0" href="MANT_CORREO_BODEGA.aspx">
                    <div class="img img-center">
                        <i class="fa fa-wrench" aria-hidden="true"></i>

                    </div>
                    <p class="title text-center">Correos de Bodegas</p>
                </a>

                <%-- <a class="tile tile-magenta" href="USUARIO_PRODUCTO.aspx">
                    <p><i class="fa fa-columns fa-3x pull-left fa-border"></i>Usuario Producto</p>
                </a>--%>
            </div>
            
            <div class="col-md-3" runat="server" id="abarrotes_sp_aspx_capi">
                <a class="tile tile-green" style="background-color: #b200ff !important;" data-stop="0" href="ABARROTES_SP.aspx">
                    <div class="img img-center">
                        <i class="fa fa-dashboard fa-2x"></i>
                    </div>
                    <p class="title text-center">ABARROTES SP</p>
                </a>
            </div>


            <%--    <div class="col-md-3" runat="server" id="cinco_mant_lab">
                <a class="tile tile-blue" data-stop="0" href="MANT_LAB.aspx">
                    <div class="img img-center">
                        <i class="fa fa-flask fa-2x"></i>
                    </div>
                    <p class="title text-center">Mantenedor Laboratorio</p>
                </a>

            <%--    <a class="tile tile-orange" href="MANT_LAB.aspx">
                    <p><i class="fa fa-flask fa-3x pull-left fa-border"></i>Mantenedor Laboratorio</p>
                </a>
            </div>
           <div class="col-md-3" runat="server" id="seis_mant_analisis">
                <a class="tile tile-yellow" data-stop="0" href="MANT_ANALISIS.aspx">
                    <div class="img img-center">
                        <i class="fa fa-eye fa-2x"></i>
                    </div>
                    <p class="title text-center">Mantenedor Tipo Analisis</p>
                </a>

               <%-- <a class="tile tile-green" href="MANT_ANALISIS.aspx">
                    <p><i class="fa fa-eye fa-3x pull-left fa-border"></i>Mantenedor Tipo Analisis</p>
                </a>--%>
        </div>
    </div>


</asp:Content>
