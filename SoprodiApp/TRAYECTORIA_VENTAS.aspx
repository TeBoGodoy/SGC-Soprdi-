<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" AutoEventWireup="True" CodeBehind="TRAYECTORIA_VENTAS.aspx.cs" Inherits="SoprodiApp.TRAYECTORIA_VENTAS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ OutputCache Location="None" NoStore="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">

    <style>
        .clickeame {
            cursor: pointer;
        }

        .td-sticky {
            position: -webkit-sticky;
            position: sticky;
            top: 0;
            background-color: #ccebff !important;
        }

        .td-sticky2 {
            position: -webkit-sticky;
            position: sticky;
            left: 0;
            top: 110px;
            background-color: #ccebff !important;
        }

        .td-sticky3 {
            position: -webkit-sticky;
            position: sticky;
            left: 0;
            top: 0;
            background-color: #ccebff !important;
            z-index: 999999;
        }

        .td-sticky4 {
            position: -webkit-sticky;
            position: sticky;
            left: 0;
            top: 78vh;
            background-color: lightgoldenrodyellow !important;
            z-index: 999;
        }

        .td-sticky5 {
            position: -webkit-sticky;
            position: sticky;
            left: 0px;
            top: 78vh;
            background-color: lightgoldenrodyellow !important;
            z-index: 9999999;
        }

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
    <script>
        function relojito(x) {
            if (x) {
                document.getElementById('relojito').style.display = "block";
                document.getElementById('relojito3').style.display = "block";
            } else {
                document.getElementById('relojito').style.display = "none";
                document.getElementById('relojito3').style.display = "none";
            }
        }
        function Datatables() {
            relojito(false);
            try {
                $('#TABLITA_PRODUCTO').DataTable({
                    destroy: true,
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    },
                    "order": [[10, "desc"]],
                    "paging": false                   
                });
            }
            catch (x) {

            }
        }
        function FijarMeta(rutcliente, codvendedor) {
            var aux_valor = document.getElementById("T_META_CLT_" + rutcliente).value;
            relojito(true);
            var parameters = new Object();
            parameters.rutcliente = rutcliente;
            parameters.codvendedor = codvendedor;
            parameters.valor = document.getElementById("T_META_CLT_" + rutcliente).value;
            parameters = JSON.stringify(parameters)
            $.ajax({
                type: "POST",
                url: "TRAYECTORIA_VENTAS.aspx/ActualizarMeta",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (x) {
                    relojito(false);
                    console.log(x);
                    if (x.d == "OK") {
                        document.getElementById("T_META_CLT_" + rutcliente).value = Separadormiles(aux_valor);
                        MostrarNotificacion("Meta fijada con éxito", 1);
                    }
                    else {
                        MostrarNotificacion(x.d, 0);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    relojito(false);
                    Mensajito("No se pudo fijar la meta, intente nuevamente");
                }
            });
        }
        function Mensajito(msg) {
            alert(msg);
        }

        function fuera22(rutcliente, bit) {

            var urlPdf = "/FICHA_CLIENTE.aspx?";
            var param = "R=" + rutcliente + "&i=" + bit;
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

        function fuera(fecha, vendedor, grupos, bit) {


            var urlPdf = "/DETALLE_FACTURA.aspx?";
            //var path2 = "P=" + path;
            //var filename2 = "&N=" + filename;
            //var urlPdf_Final = urlPdf + path2 + filename2;
            var param = "F=" + fecha + "&V=" + vendedor + "&G=" + grupos + "&i=" + bit;
            var urlPdf_Final = urlPdf + param;

            if (navigator.appName.indexOf('Microsoft Internet Explorer') != -1) {
                window.showModelessDialog(urlPdf_Final, '', 'dialogTop:50px;dialogLeft:50px;dialogHeight:700px;dialogWidth:1200px');
            };

            if (navigator.appName.indexOf('Netscape') != -1) {
                window.open(urlPdf_Final, '', 'width=1200,height=700,left=50,top=50');
                void 0;
            };
            return false;
        }

        function Separadormiles(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
        }


    </script>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <div id="relojito" style="display: none">
        <div id="relojito3" style="color: red;" class="text-center">
            <h2>SOPRODI - APP WEB</h2>
            <h4 style="color: red;">Espere un momento...</h4>
            <i class="fa fa-spinner fa-spin fa-2x" style="color: red;"></i>
        </div>
    </div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-12">
                    <div class="box">
                        <div class="box-content">
                            <h4>TRAYECTORIA DE VENTAS</h4>
                            <div class="row">
                                <div class="col-sm-3 text-right">
                                    <asp:DropDownList runat="server" ID="CB_VENDEDORES" CssClass="form-control"></asp:DropDownList>
                                    <small>VENDEDOR</small>
                                    <div style="display: none">
                                        <asp:TextBox runat="server" ID="T_VENDEDOR"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-3  text-right">
                                    <asp:DropDownList runat="server" ID="CB_PERIODO" CssClass="form-control"></asp:DropDownList>
                                    <small>ULTIMO MES</small>
                                </div>
                                <div class="col-sm-3">
                                    <asp:Button runat="server" CssClass="btn btn-primary" ID="B_FILTRAR" OnClick="B_FILTRAR_Click" Text="GENERAR REPORTE" OnClientClick="relojito(true);" />
                                </div>
                            </div>
                            <div class="row" runat="server" id="DIV_ENVIAR_CORREO" visible="false">
                                <hr />
                                <div class="col-sm-3 text-right">
                                    <asp:TextBox runat="server" ID="T_CORREO" CssClass="form-control"></asp:TextBox>
                                    <small>CORREO</small><br />                                 
                                </div>
                                 <div class="col-sm-3 text-right">                                  
                                    <asp:TextBox runat="server" ID="T_CC" CssClass="form-control"></asp:TextBox>
                                    <small>CC</small>
                                </div>
                                <div class="col-sm-3">
                                    <asp:Button runat="server" CssClass="btn btn-success" ID="B_ENVIAR_CORREO" OnClick="B_ENVIAR_CORREO_Click" Text="ENVIAR REPORTE POR CORREO" OnClientClick="relojito(true);" />
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-sm-12">
                                    <div id="HTML_DIV2" runat="server"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div style="width: 100%; height: 86vh; overflow-x: auto">
                                        <div id="HTML_DIV" runat="server"></div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>




