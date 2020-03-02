<%@ Page Title="" ViewStateMode="Enabled" Language="C#" MasterPageFile="~/Base.Master" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" AutoEventWireup="True" UICulture="es-ES" Culture="es-ES" CodeBehind="ABARROTES_SP.aspx.cs" Inherits="SoprodiApp.ABARROTES_SP" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ OutputCache Location="None" NoStore="true" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">
    <script>
        function checkEnter(e) {

            if (e.keyCode == 13) {

                var sw_modal = document.getElementById("<%=SW_MODAL.ClientID %>").value;

                if (sw_modal != "MODAL") {
                    document.getElementById("ContentPlaceHolder_Contenido_Button1").click();
                    return false;
                }
            }
        }
        function enter_traer_actualizar_(e) {

            if (e.keyCode == 13) {
                document.getElementById("ContentPlaceHolder_Contenido_guarda_traer_actualizar").click();
                return false;
            }
        }
        //function SORT_GRILLA() {
        //    new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_PRODUCTOS'));
        //}

        function modal_unidad_1(cod_doc) {
            //alert("aca" + cod_doc);
            $('#titulo_modal').html("<h3 style='color: cornflowerblue;font-size: -webkit-xxx-large;'>" + cod_doc + "</h3>");
            document.getElementById("div_unidad_1").click();
        }
        function LoadProduc_kg(result) {

         <%--   //quito los options que pudiera tener previamente el combo

            $("#<%=cb_productos_kg.ClientID%>").html("");


            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {

                $("#<%=cb_productos_kg.ClientID%>").append($("<option></option>").attr("value", this.COD_PROD).text(this.NOM_PROD))

            });


            $("#<%=cb_productos_kg.ClientID%>").chosen();
            $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");--%>



        }

        function guarda() {

            alert('Correo Enviado');

        }

        function incluir_producto() {


<%--            var parameters = new Object();

            parameters.PARA = document.getElementById("<%=tx_para.ClientID%>").value;;
            parameters.TEXT = document.getElementById("<%=tx_text_.ClientID%>").value;;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_SP.aspx/ENVIAR_CORREO",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: guarda,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al cargar bodegas");
                }
            });--%>

        }




        function LoadDetalle(result) {


           <%-- $("#<%=tx_tipo_.ClientID%>").value = "";


            $("#<%=tx_valor.ClientID%>").value = "";--%>



            $.each(result.d, function () {


                var elem3 = document.getElementById("ContentPlaceHolder_Contenido_tx_tipo_");
                elem3.value = this.stkunit;

                var elem4 = document.getElementById("ContentPlaceHolder_Contenido_tx_valor");
                elem4.value = this.valor;


                ;
            });

            try {
                var elem3 = document.getElementById("detalle_prod");
                elem3.style = "visibility:block";
                //document.getElementById("detalle_prod").style = "visible:hidden";

            <%--    $("#<%=cb_productos_kg.ClientID%>").chosen();
                $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");--%>
            } catch (e) { }


        }

        function cambia_tipo_pago3(id_factur) {

            var estado = document.getElementById("cb_cambio_pago" + id_factur).value;
            var parameters = new Object();
            parameters.factura = id_factur;
            parameters.estado = estado;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_SP_SELECT.aspx/cambia_tipo_pago_",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (XMLHttpRequest.status == 401) {
                        alert("Fin de la session");
                        location.href = "Acceso.aspx";
                    } else {
                        alert("Error al cargar evento");
                    }
                }
            }).done(function (resp) {
                resp = resp.d;



            });
        }

        //var entry_date_col_num = $('th:contains("F.")').index();

        //     { type: 'date-uk', targets: [entry_date_col_num] }



        function sp_select(cod_prod, bodega, asignado_por) {

            var urlPdf = "/ListadoProductosPlanificador.aspx?";
            //var path2 = "P=" + path;
            //var filename2 = "&N=" + filename;
            //var urlPdf_Final = urlPdf + path2 + filename2;
            var param = "c=" + cod_prod;
            var param = param + "&b=" + bodega;
            var param = param + "&a=" + asignado_por;
            var urlPdf_Final = urlPdf + param;

            if (navigator.appName.indexOf('Microsoft Internet Explorer') != -1) {
                window.showModelessDialog(urlPdf_Final, '', 'dialogTop:50px;dialogLeft:50px;dialogHeight:700px;dialogWidth:1200px');
            };

            if (navigator.appName.indexOf('Netscape') != -1) {
                window.open(urlPdf_Final, '', 'width=' + screen.availWidth + ',height=' + screen.availHeight);
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

        function fuera22(rutcliente, bit) {

            var urlPdf = "/FICHA_CLIENTE.aspx?";
            //var path2 = "P=" + path;
            //var filename2 = "&N=" + filename;
            //var urlPdf_Final = urlPdf + path2 + filename2;
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




        //function superfiltro() {

        //    $(".filtrar tr:has(td)").each(function () {
        //        var t = $(this).text().toLowerCase();
        //        $("<td class='indexColumn'></td>")
        //        .hide().text(t).appendTo(this);
        //    });
        //    //Agregar el comportamiento al texto (se selecciona por el ID) 
        //    $("#t_filtro_memoria").keyup(function () {
        //        var s = $(this).val().toLowerCase().split(" ");
        //        $(".filtrar tr:hidden").show();
        //        $.each(s, function () {
        //            $(".filtrar tr:visible .indexColumn:not(:contains('"
        //            + this + "'))").parent().hide();
        //        });
        //    });


        function producto_cambio_humano() {



            var parameters = new Object();

            parameters.sw = "humano";

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_COBRO_BODEGA.aspx/PRODUCTO_CAMBIO",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadProduc,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al cargar bodegas");
                }
            });



        }



        function producto_cambio_humano2() {



            var parameters = new Object();

            parameters.sw = "humano";

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_COBRO_BODEGA.aspx/PRODUCTO_CAMBIO2",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadProduc_kg,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al cargar bodegas");
                }
            });

        }


        function producto_cambio_animal() {



            var parameters = new Object();

            parameters.sw = "animal";

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_COBRO_BODEGA.aspx/PRODUCTO_CAMBIO",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadProduc,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al cargar bodegas");
                }
            });




        }

        function producto_cambio_animal2() {



            var parameters = new Object();

            parameters.sw = "animal";

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_COBRO_BODEGA.aspx/PRODUCTO_CAMBIO2",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadProduc_kg,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al cargar bodegas");
                }
            });


        }
        function super_ff() {

            $(".filtrar tr:has(td)").each(function () {
                var t = $(this).text().toLowerCase();
                $("<td class='indexColumn'></td>")
                    .hide().text(t).appendTo(this);
            });
            //Agregar el comportamiento al texto (se selecciona por el ID) 
            $("#btn_filtro_nuevo").click(function () {
                var s = $('#t_filtro_memoria').val().toLowerCase().split(" ");
                $(".filtrar tr:hidden").show();
                $.each(s, function () {
                    $(".filtrar tr:visible .indexColumn:not(:contains('"
                        + this + "'))").parent().hide();
                });
            });

            $("#t_filtro_memoria").keyup(function (event) {
                if (event.keyCode == 13) {
                    var s = $('#t_filtro_memoria').val().toLowerCase().split(" ");
                    $(".filtrar tr:hidden").show();
                    $.each(s, function () {
                        $(".filtrar tr:visible .indexColumn:not(:contains('"
                            + this + "'))").parent().hide();
                    });
                }
            });



        }


        function CARGANDO() {
            var elem1 = document.getElementById("<%=txt_hasta.ClientID%>");
            elem1.style.zIndex = "0";

            var elem5 = document.getElementById("<%=div1.ClientID%>");
            elem5.style.visibility = "hidden";
        }


        function CARGANDO2() {
            var elem1 = document.getElementById("<%=txt_hasta.ClientID%>");
            elem1.style.zIndex = "0";
        }

        function CARGANDO_CLOSE() {


            var elem1 = document.getElementById("<%=txt_hasta.ClientID%>");
            elem1.style.zIndex = "0";

            var elem5 = document.getElementById("<%=div1.ClientID%>");
            elem5.style.visibility = "visible";


        }


        function CARGANDO_ENVIAR() {


         <%--   var elem3 = document.getElementById("<%=cargando_gif_2.ClientID%>");--%>
            elem3.style.display = "block";
        }



        function CARGA_FECHA() {



        }

        function NO_VENDEDORES() {

            alert("No existe vendedores para periodos o ha ingresado una fecha errónea");
        }


        function NO_GRUPOS() {

            alert("No existe ventas para periodos o ha ingresado una fecha errónea");
        }
        function modal_unidad() {

            document.getElementById("div_unidad_1").click();
            chosen_update();

            document.getElementById("<%=SW_MODAL.ClientID %>").value = "MODAL";
        }
        function esconde() {
            try {
                var elem3 = document.getElementById("detalle_prod");
                elem3.style = "visibility:hidden";
                //document.getElementById("detalle_prod").style = "visible:hidden";

            <%--    $("#<%=cb_productos_kg.ClientID%>").chosen();
                $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");--%>
            } catch (e) { }
        }
        function chosen_update() {

            $("#<%=d_bodega.ClientID%>").chosen();
            $("#<%=d_bodega.ClientID%>").trigger("chosen:updated");

            $("#<%=d_bodega_2.ClientID%>").chosen();
            $("#<%=d_bodega_2.ClientID%>").trigger("chosen:updated");

            $("#<%=d_estado_2.ClientID%>").chosen();
            $("#<%=d_estado_2.ClientID%>").trigger("chosen:updated");
            $("#<%=d_estado_3.ClientID%>").chosen();
            $("#<%=d_estado_3.ClientID%>").trigger("chosen:updated");

            $("#<%=CB_BODEGA.ClientID%>").chosen();
            $("#<%=CB_BODEGA.ClientID%>").trigger("chosen:updated");
        }

        function click_reporte() {

            document.getElementById("ContentPlaceHolder_Contenido_Button1").click();

        }

        function CLICK_MODAL() {

            document.getElementById("div_prod").click();

        }


        function CLICK_MODAL_ERRORES() {

            document.getElementById("errores").click();

        }
        function cierra() {

            var table = document.getElementById('ContentPlaceHolder_Contenido_G_PRODUCTOS');
            var sort = new Tablesort(table);

            // Make some Ajax request to fetch new data and on success:
            sort.refresh();

        }


        function fuera(rutcliente, bit) {


            var urlPdf = "/CUENTA_CRRTE.aspx?";
            //var path2 = "P=" + path;
            //var filename2 = "&N=" + filename;
            //var urlPdf_Final = urlPdf + path2 + filename2;
            var param = "R=" + rutcliente + "&i=" + bit;
            var urlPdf_Final = urlPdf + param;

            if (navigator.appName.indexOf('Microsoft Internet Explorer') != -1) {
                window.showModelessDialog(urlPdf_Final, '', 'dialogTop:50px;dialogLeft:50px;dialogHeight:500px;dialogWidth:1100px');
            };

            if (navigator.appName.indexOf('Netscape') != -1) {
                window.open(urlPdf_Final, '');
                void 0;
            };
            return false;
        }


        function fuera3(id_archivo, año) {
            var urlPdf = "/PDF.aspx?";

            var id_archivo = "Ar=" + id_archivo;
            var año_fact = "&A=" + año;
            var urlPdf_Final = urlPdf + id_archivo + año_fact;



            if (navigator.appName.indexOf('Microsoft Internet Explorer') != -1) {
                window.showModelessDialog(urlPdf_Final, '_blank');
            };

            if (navigator.appName.indexOf('Netscape') != -1) {
                window.open(urlPdf_Final, '_blank');
                void 0;
            };
            return false;
        }

        function SIDEBAR_ABARROTE() {


            document.getElementById("REPORTE").className = "";

            document.getElementById("REPORTE_VENDEDOR").className = "";
            document.getElementById("REPORTE_SALA").className = "";
            document.getElementById("REPORTE_CM").className = "";
            document.getElementById("REPORTE_COMPARATIVO").className = "";
            try {
                document.getElementById("diario_li").style.display = "block";
                document.getElementById("diario_li").className = "";
            } catch (e) {

            }

            try {
                document.getElementById("adm").className = "active";
                var elem2 = document.getElementById("adm");
                elem2.style.display = "none";
            } catch (e) {

            }

            try {
                document.getElementById("documentos").className = "";
                var elem2 = document.getElementById("documentos");
                elem2.style.display = "none";
            } catch (e) {

            }
            try {
                document.getElementById("visor").className = "";

            } catch (e) {

            }
            try {
                document.getElementById("gestor").className = "";

            } catch (e) {

            } try {
                document.getElementById("usuarioproducto").className = "";

            } catch (e) {

            } try {
                document.getElementById("logdocu").className = "";

            } catch (e) {

            }

            try {
                document.getElementById("DETALLE").className = "";
                var elem2 = document.getElementById("DETALLE");
                elem2.style.display = "none";

            } catch (e) {
                document.getElementById("DETALLE_").className = "";
                var elem2 = document.getElementById("DETALLE_");
                elem2.style.display = "none";
                document.getElementById("REPORTE_LV_B").className = "";
                document.getElementById("REPORTE_LV_G").className = "";
            }



        }

        function fuera_stock(prod, compras) {

            var urlPdf = "/Stock_detalle.aspx?";
            //var path2 = "P=" + path;
            //var filename2 = "&N=" + filename;
            //var urlPdf_Final = urlPdf + path2 + filename2;
            var param = "C=" + prod + "&";
            var param2 = "F=" + compras;
            var urlPdf_Final = urlPdf + param + param2;

            if (navigator.appName.indexOf('Microsoft Internet Explorer') != -1) {
                window.showModelessDialog(urlPdf_Final, '', 'dialogTop:50px;dialogLeft:50px;dialogHeight:500px;dialogWidth:1100px');
            };

            if (navigator.appName.indexOf('Netscape') != -1) {
                window.open(urlPdf_Final, '', 'width=1100,height=500,left=50,top=50');
                void 0;
            };
            return false;
        }



        (function () {


            var scrolledDivs = [];
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(function (sender, args) {
                //store the scroll positions of all "scrolled" div elements
                //UpdatePanel and Panel both are div elements
                scrolledDivs = [];
                $(document.getElementById('DivMainContent')).each(function () {
                    var div = $(this);
                    if (div.scrollLeft() != 0 || div.scrollTop() != 0) {
                        scrolledDivs.push({ element: this.id, scrollLeft: div.scrollLeft(), scrollTop: div.scrollTop() });
                    }
                });
            });

            prm.add_endRequest(function (sender, args) {
                //restore scroll positions
                $.each(scrolledDivs, function (index, value) {
                    $get(value.element).scrollLeft = value.scrollLeft;
                    $get(value.element).scrollTop = value.scrollTop;
                });

            });
        })();



        //<!-- Si se quiere habilitar que permita comas enviar en la funcion  CheckNumeric(event, 'true') -->   
        function CheckNumeric(e, a) {
            if (window.event) {
                if ((e.keyCode < 48 || e.keyCode > 57) & e.keyCode != 8) {
                    if (typeof a === "undefined") {
                        event.returnValue = false;
                        return false;
                    }
                    else {
                        if (e.keyCode == 44) { } else {
                            event.returnValue = false;
                            return false;
                        }
                    }
                }
            }
            else {
                if ((e.which < 48 || e.which > 57) & e.which != 8) {
                    if (typeof a === "undefined") {
                        e.preventDefault();
                        return false;
                    }
                    else {
                        if (e.which == 44) {

                        } else {
                            e.preventDefault();
                            return false;
                        }
                    }
                }
            }
        }


        function enter_(e, a) {
            if (window.event) {
                if ((e.keyCode < 48 || e.keyCode > 57) & e.keyCode != 8) {
                    enter_traer_actualizar_(event);
                    return false;
                }
            }
        }

        function guarda(resp) {
            resp = resp.d;
            if (resp == "ok") {
                alert("Guardado");
                chosen_update();

            }
            else {
                alert("error");
                chosen_update();
            }

        }
        function guardar_solo_kg() {


            var parameters = new Object();

            <%--parameters.sw = document.getElementById("<%=cb_productos_kg.ClientID%>").value;;--%>
<%--            parameters.tipo = document.getElementById("<%=tx_tipo_.ClientID%>").value;;
            parameters.valor = document.getElementById("<%=tx_valor.ClientID%>").value;;--%>

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_COBRO_BODEGA.aspx/guardar_solo_kg",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: guarda,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al cargar bodegas");
                }
            });


        }
        function carga_d_Click() {



            var parameters = new Object();

            <%--parameters.sw = document.getElementById("<%=cb_productos_kg.ClientID%>").value;;--%>

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_COBRO_BODEGA.aspx/carga_click",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadDetalle,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al cargar bodegas");
                }
            });



        }


    </script>
    <style>
        .badge-azul {
            background-color: steelblue;
            color: white;
        }

        .badge-rojo {
            background-color: darkred;
            color: white;
        }

        .badge-block {
            width: 100%;
            text-align: center;
        }

        .test {
            background-color: #428bca !important;
            color: white !important;
        }

        .overlay {
            position: fixed;
            z-index: 0;
            top: 0px;
            left: 0px;
            right: 0px;
            bottom: 0px;
            background-color: #aaa;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }

        .Grheader {
            overflow: hidden;
            width: 26px;
            position: relative;
            top: 0px;
            z-index: 10;
            vertical-align: top;
            margin-right: 16px;
        }

        .GrCuerpo {
            width: 80%;
            position: relative;
            left: 16px;
            z-index: 1;
            overflow: auto;
            height: 100%;
        }



        td.locked, th.locked {
            position: relative;
            left: expression((this.parentElement.parentElement.parentElement.parentElement.scrollLeft-2)+'px');
        }

        th.sort-header::-moz-selection {
            background: transparent;
        }

        th.sort-header::selection {
            background: transparent;
        }

        th.sort-header {
            cursor: pointer;
        }

        table th.sort-header:after {
            content: '';
            float: right;
            margin-top: -4px;
            border-width: 0 6px 6px;
            border-style: solid;
            border-color: #404040 transparent;
            visibility: hidden;
        }

        table th.sort-header:hover:after {
            visibility: visible;
        }

        table th.sort-up:after,
        table th.sort-down:after,
        table th.sort-down:hover:after {
            visibility: visible;
            opacity: 0.9;
        }

        table th.sort-up:after {
            border-bottom: none;
            border-width: 6px 6px 0;
        }


        .estado10 {
            border-left-color: #fa5a35 !important;
        }

        .estado20 {
            border-left-color: #4e53ff !important;
        }

        .estado30 {
            border-left-color: #10ff23 !important;
        }

        .estado40 {
            border-left-color: #ff10c0 !important;
        }

        .estado10S {
            border-left-color: #c2c506 !important;
        }

        .estado10P {
            border-left-color: #06c5c5 !important;
        }

        .estadoGFX {
            border-left-color: #30441f !important;
        }
    </style>
    <script type="text/javascript">
        Sys.Application.add_load(BindEvents);

        function BindEvents() {

                    //superfiltro();

                    <%--                             
                    $("#<%=cb_productos_kg.ClientID%>").chosen();
                    $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");
--%>


            $("#<%=d_bodega.ClientID%>").change(function () {

                var arr = $(this).val();
                document.getElementById("<%=l_clientes.ClientID %>").value = arr;
            });


            $("#<%=d_vendedor.ClientID%>").change(function () {

                var arr = $(this).val();
                document.getElementById("<%=l_vendedor_vm.ClientID %>").value = arr;


            });


            $("#<%=d_bodega_2.ClientID%>").change(function () {

                // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                //este parametro mapeara con el definido en el web service

                var arr = $(this).val();
                document.getElementById("<%=lb_bodegas2.ClientID %>").value = arr;

            });



            $("#<%=d_estado_2.ClientID%>").change(function () {

                // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                //este parametro mapeara con el definido en el web service

                var arr = $(this).val();
                document.getElementById("<%=lb_bodegas3.ClientID %>").value = arr;

            });

            $("#<%=d_estado_3.ClientID%>").change(function () {

                // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                //este parametro mapeara con el definido en el web service

                var arr = $(this).val();
                document.getElementById("<%=lb_planificado.ClientID %>").value = arr;

            });



            $("#<%=CB_BODEGA.ClientID%>").change(function () {

                // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                //este parametro mapeara con el definido en el web service

                var arr = $(this).val();
                document.getElementById("<%=l_bodega.ClientID %>").value = arr;

            });




            $("#<%=d_bodega_2.ClientID%>").chosen();
            $("#<%=d_bodega_2.ClientID%>").trigger("chosen:updated");
            $("#<%=d_estado_2.ClientID%>").chosen();
            $("#<%=d_estado_2.ClientID%>").trigger("chosen:updated");
            $("#<%=d_estado_3.ClientID%>").chosen();
            $("#<%=d_estado_3.ClientID%>").trigger("chosen:updated");

            $("#<%=d_bodega.ClientID%>").chosen();
            $("#<%=d_bodega.ClientID%>").trigger("chosen:updated");

            $("#<%=d_vendedor.ClientID%>").chosen();
            $("#<%=d_vendedor.ClientID%>").trigger("chosen:updated");

            $("#<%=CB_BODEGA.ClientID%>").chosen();
            $("#<%=CB_BODEGA.ClientID%>").trigger("chosen:updated");

              <%--      $("#<%=d_producto.ClientID%>").chosen();
                    $("#<%=d_producto.ClientID%>").trigger("chosen:updated");
--%>


        }


        // CAPISITO JS
        var table_productos;

        $(document).ready(function () {
            creagrilla();
        });

        function creagrilla() {
            try {
                table_productos = $("#<%=G_INFORME_TOTAL_VENDEDOR.ClientID %>").DataTable({
                    "destroy": true,
                    "pageLength": -1,
                    "paging": false,
                    "order": [[0, "desc"]],
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    }
                });
            } catch (e) {

            }

        }

        function AbreModal_ReportarSP() {
            document.getElementById("btn_modal_repvend").click();
        }
        function CerrarModal_ReportarSP() {
            document.getElementById("cerrar_modal_planificacion").click();
        }

        function VERSP() {
            var x = document.getElementById("<%=LBL_NUM_SP.ClientID %>").innerHTML;
            window.open("PDF_SP.aspx?sp=" + x, '_blank');
        }





    </script>
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="900000" EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="G_INFORME_TOTAL_VENDEDOR" />
        </Triggers>
        <ContentTemplate>
            <div id="breadcrumbs" style="margin-top: -5px;">
                <ul class="breadcrumb" style="height: 40px !important;">
                    <li>
                        <i class="fa fa-home"></i>
                        <a href="menu.aspx">Menú</a>
                        <span class="divider"><i class="fa fa-angle-right"></i></span>
                    </li>
                    <li>
                        <a runat="server" id="titulo2"></a>
                        <span class="divider"><i class="fa fa-angle-right"></i></span>
                    </li>
                    <li>
                        <a runat="server" id="titulo3"></a>
                        <span class="divider"><i class="fa fa-angle-right"></i></span>
                    </li>
                    <li class="active">Abarrotes SP</li>
                </ul>
            </div>
            <!-- BEGIN Main Content -->
            <div class="row">
                <div class="col-md-12" id="div_superior">
                    <div class="box" style="margin-top: -20px;">
                        <%--<asp:Button ID="btn_unidad" runat="server" OnClientClick="modal_unidad();" Style="color: white; margin-right: 1.3%" Class="btn btn-success" Text="Traer//Actualizar SP" />--%>
                        <%--<asp:Button ID="btn_edit_" runat="server" OnClientClick="modal_unidad();" Style="color: white; margin-right: 1.3%" Class="btn btn-warning" Text="Actualizar SP" />--%>

                        <div class="box-content">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box">

                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label" style="top: 5px;">Filtrar por fecha de:</label>

                                                <div class="col-sm-4 controls">
                                                    <div class="input-group" id="check_fechas">
                                                        <asp:RadioButton ID="rd_em" Text="&nbsp;&nbsp;Emisión" Style="padding-right: 20px;" Checked="True"
                                                            GroupName="tipo_usuario" runat="server" />

                                                        <asp:RadioButton ID="rd_ven" Style="padding-right: 20px;" Text="&nbsp;&nbsp;Despacho."
                                                            GroupName="tipo_usuario" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">

                                                <label class="col-sm-2 control-label" style="top: 5px;">Desde</label>

                                                <div class="col-sm-4 controls">
                                                    <div class="input-group">

                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                        <asp:TextBox ID="txt_desde" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender9" runat="server" TargetControlID="txt_desde" Format="dd/MM/yyyy" />

                                                    </div>
                                                </div>
                                                <label class="col-sm-2 control-label">Hasta</label>
                                                <div class="col-sm-4 controls">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                        <asp:TextBox ID="txt_hasta" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender10" runat="server" TargetControlID="txt_hasta" Format="dd/MM/yyyy" />

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">

                                                <label class="col-sm-2 control-label" style="top: 5px;">Cliente</label>
                                                <div class="col-sm-4 col-lg-4">
                                                    <div class="controls">
                                                        <asp:ListBox ID="d_bodega" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                    </div>
                                                </div>
                                                <label class="col-sm-2 control-label" style="top: 5px;">Vendedor</label>
                                                <div class="col-sm-4 col-lg-4">
                                                    <div class="controls">
                                                        <asp:ListBox ID="d_vendedor" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label" style="top: 5px;">Estado Venta Móvil</label>
                                                <div class="col-sm-4 col-lg-4">
                                                    <div class="controls">
                                                        <asp:ListBox ID="d_bodega_2" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                    </div>
                                                </div>
                                                <div style="display: none">
                                                    <label class="col-sm-2 control-label" style="top: 5px;">Estado Instruccion</label>
                                                    <div class="col-sm-4 col-lg-4">
                                                        <div class="controls">
                                                            <asp:ListBox ID="d_estado_2" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-group" style="display: none">
                                                <label class="col-sm-2 control-label" style="top: 5px;">Estado Planificado</label>
                                                <div class="col-sm-4 col-lg-4">
                                                    <div class="controls">
                                                        <asp:ListBox ID="d_estado_3" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                    </div>
                                                </div>
                                                <label class="col-sm-2 control-label" style="top: 5px;">Bodega</label>
                                                <div class="col-sm-4 controls">
                                                    <%--<asp:DropDownList runat="server" ID="CB_BODEGA" ClientIDMode="Static" Enabled="false" CssClass="form-control chosen"></asp:DropDownList>--%>
                                                    <asp:ListBox ID="CB_BODEGA" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label">
                                                    Nº SP.(separar por coma):
                                                </label>
                                                <div class="col-sm-4 controls">
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txt_sp"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-6 controls text-right">
                                                    <asp:Button ID="Button1" runat="server" OnClientClick="relojito(true);" Style="color: white;" Class="btn btn-primary" OnClick="Button1_Click" Text="Filtrar SP" />
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
            <div class="row">
                <div id="div1" runat="server">
                    <div id="div_report" runat="server">
                        <div class="col-md-12">
                            <div class="box">
                                <div class="box-content">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="clearfix"></div>
                                            <div style="width: 100%; overflow-x: auto;">
                                                <asp:GridView ID="G_INFORME_TOTAL_VENDEDOR" CssClass="table table-bordered table-advance table-condensed condensed table-sm" OnRowCommand="G_INFORME_TOTAL_VENDEDOR_RowCommand" OnRowDataBound="G_INFORME_TOTAL_VENDEDOR_RowDataBound" runat="server"
                                                    ShowHeaderWhenEmpty="True" AutoGenerateColumns="false"
                                                    DataKeyNames="
                                                                    CodDocumento,
                                                                    NombreCliente,
                                                                    NombreVendedor,
                                                                    MontoNeto,
                                                                    DescBodega,
                                                                    FechaDespacho,
                                                                    DifDias,
                                                                    FechaEmision,
                                                                    CodVendedor,
                                                                    NotaLibre,
                                                                    CodBodega,
                                                                    CodMoneda,   
                                                                    DescEstadoDocumento,
                                                                    Facturas,
                                                                    GxEstadoSync,
                                                                    GxActualizado,
                                                                    GxEnviadoERP,
                                                                    FechaCreacion,
                                                                    ValorTipoCambio,
                                                                    LimiteSeguro, 
                                                                    TipoCredito , 
                                                                    CreditoDisponible ,
                                                                    CreditoAutorizado,
                                                                    EmailVendedor,
                                                                    ESTADO,
                                                                    CodProducto,
                                                                    Cantidad,
                                                                    AprobadoFull,
                                                                    Asignada,
                                                                    EstadoParcial,   
                                                                    fPLAN,
                                                                    NombreSucursal,
                                                                    NotaDespacho,
                                                                    DescFormaPago,
                                                                        CodDocumentoERP,
                                                    estado_capi, usuario_soprodi, id_interno, codcliente,
                                                    DirDireccion1, IVA, MontoBruto"
                                                    Font-Size="12px">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Nª SP" ItemStyle-Font-Bold="true">
                                                            <ItemTemplate>
                                                                <asp:LinkButton OnClientClick="relojito(true);" ClientIDMode="AutoID" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Reportar"><%# Eval("CodDocumento")%></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Vendedor" DataField="NombreVendedor" />
                                                        <asp:BoundField HeaderText="Cliente" DataField="NombreCliente" />
                                                        <asp:BoundField HeaderText="Emisión" DataField="FechaEmision" />
                                                        <asp:BoundField HeaderText="Desp." DataField="FechaDespacho" />
                                                        <asp:BoundField HeaderText="Usuario" DataField="usuario_soprodi" />
                                                        <asp:BoundField HeaderText="Estado Interno" DataField="estado_capi" ItemStyle-Font-Bold="true" />
                                                        <asp:BoundField HeaderText="Est. Vta Móvil" DataField="DescEstadoDocumento" ItemStyle-Font-Bold="true" />
                                                        <asp:BoundField HeaderText="SP Solomon" DataField="CodDocumentoERP" />
                                                        <asp:BoundField HeaderText="Neto" DataField="MontoNeto" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right" />
                                                        <%--   <asp:BoundField HeaderText="Cod. Prod" DataField="CodProducto" />
                                                        <asp:BoundField HeaderText="Producto" DataField="DescProducto" />
                                                        <asp:BoundField HeaderText="Cantidad" DataField="Cantidad" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right" />
                                                        <asp:BoundField HeaderText="Tipo" DataField="CodUnVenta" />--%>
                                                          <asp:TemplateField HeaderText="Facturas" ItemStyle-Font-Bold="true">
                                                            <ItemTemplate>
                                                              <a href='REPORTE_LISTADO_DOC.aspx?F=<%# Eval("Facturas") %>' target='_blank'><%# Eval("Facturas") %></a>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                     <%--   <asp:BoundField HeaderText="Facturas" DataField="Facturas" />--%>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        No existen datos.
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- MODAL REPORTAR VENDEDOR -->
    <a href="#modalrepvend" id="btn_modal_repvend" role="button" class="btn" style="visibility: hidden; position: absolute;" data-toggle="modal"></a>
    <div id="modalrepvend" class="modal fade" style="width: 90%; margin-left: 5%; margin-top: 10px;">
        <asp:UpdatePanel runat="server" ID="UP_REP_VEND" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="B_ENVIAR_REPORTE_SP" />
            </Triggers>
            <ContentTemplate>
                <div>
                    <div class="modal-content">
                        <div style="display: none">
                            <asp:TextBox runat="server" ID="T_NUM_SP"></asp:TextBox>
                            <asp:TextBox runat="server" ID="ID_INTERNO_SP"></asp:TextBox>
                            <asp:TextBox runat="server" ID="COD_BODEGA"></asp:TextBox>
                        </div>
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h3 id="myModalLabel">Detalle de la SP
                                <asp:LinkButton runat="server" ID="B_VER_SP" CssClass="btn btn-success btn-sm" OnClientClick="VERSP();">Ver SP <i class="fa fa-file-o"></i></asp:LinkButton></h3>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-2">
                                    <small>Nº SP:</small>
                                    <br />
                                    <asp:Label runat="server" ID="LBL_NUM_SP" Font-Bold="true" Font-Size="Large"></asp:Label>
                                    <asp:TextBox runat="server" ID="T_RUT_CLIENTE" Style="visibility:hidden;position:absolute;" Font-Bold="true" Font-Size="Large"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <small>Estado Interno:</small>
                                    <br />
                                    <asp:Label runat="server" ID="LBL_ESTADO_INTERNO" Font-Bold="true" Font-Size="Large"></asp:Label>
                                </div>
                                <div class="col-sm-2">
                                    <small>Est. Venta Móvil:</small>
                                    <br />
                                    <asp:Label runat="server" ID="LBL_ESTADO_VTA_MOVIL" Font-Bold="true" Font-Size="Large"></asp:Label>
                                </div>
                                <div class="col-sm-2">
                                    <small>SP Solomon:</small>
                                    <br />
                                    <asp:Label runat="server" ID="LBL_SP_SOLOMON" Font-Bold="true" Font-Size="Large"></asp:Label>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-sm-12">
                                    <table style="width: 100%; border-color: lightgray;" border="1" class="table">
                                        <tr>
                                            <td><b>Fecha Emisión: </b></td>
                                            <td>
                                                <asp:Label runat="server" ID="LBL_FECHA_EMISION"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><b>Fecha Despacho: </b></td>
                                            <td>
                                                <asp:Label runat="server" ID="LBL_FECHA_DESPACHO"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><b>Vendedor: </b></td>
                                            <td>
                                                <asp:Label runat="server" ID="LBL_VENDEDOR"></asp:Label> <asp:Label runat="server" ID="COD_VENDEDOR" Visible="false"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><b>Cliente: </b></td>
                                            <td>
                                                <asp:Label runat="server" ID="LBL_CLIENTE"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><b>Sucursal Cliente:</b></td>
                                            <td>
                                                <asp:Label runat="server" ID="LBL_SUCURSAL"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><b>Dirección:</b></td>
                                            <td>
                                                <asp:Label runat="server" ID="LBL_DIRECCION"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><b>Bodega:</b></td>
                                            <td>
                                                <asp:Label runat="server" ID="LBL_NOM_BODEGA"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><b>Nota Despacho:</b></td>
                                            <td>
                                                <asp:Label runat="server" ID="LBL_NOTA_DESPACHO"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><b>Nota Libre:</b></td>
                                            <td>
                                                <asp:Label runat="server" ID="LBL_NOTA_LIBRE"></asp:Label></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <hr />
                            <h4><b>Productos de la SP</b></h4>
                            <br />
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:GridView ID="G_DETALLE" CssClass="table table-bordered table-advance table-condensed condensed" runat="server"
                                        ShowHeaderWhenEmpty="True" AutoGenerateColumns="false" OnRowDataBound="G_DETALLE_RowDataBound"
                                        DataKeyNames="CodProducto, PrecioUnitario"
                                        Font-Size="12px">
                                        <HeaderStyle CssClass="test no-sort" />
                                        <Columns>
                                            <asp:BoundField HeaderText="Cod. Prod" DataField="CodProducto" ItemStyle-Font-Bold="true" />
                                            <asp:BoundField HeaderText="Producto" DataField="DescProducto" ItemStyle-Font-Bold="true" />
                                            <asp:BoundField HeaderText="$ Lista" DataField="PrecioLista" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true"  />
                                            <asp:BoundField HeaderText="$ Unit. SP" DataField="PrecioUnitario" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" DataFormatString="{0:N1}" />
                                            <asp:BoundField HeaderText="Stock" DataField="Stock" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" />
                                            <asp:TemplateField HeaderText="Cant. SP" ItemStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <asp:TextBox ClientIDMode="AutoID" ID="T_CANTIDAD_SP" runat="server" ReadOnly="true" Text='<%# Eval("Cantidad", "{0:N0}")%>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cant. a Instruir" ItemStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <asp:TextBox ClientIDMode="AutoID" ID="T_CANTIDAD_PLAN" runat="server" Text='<%# Eval("Cantidad", "{0:N0}")%>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Tipo" DataField="CodUnVenta" ItemStyle-Font-Bold="true" />
                                            <asp:BoundField HeaderText="Neto" DataField="det_monto_neto" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" />
                                            <asp:BoundField HeaderText="% Descuento" DataField="Descuento" DataFormatString="{0:N1}" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" />
                                            <asp:BoundField HeaderText="Neto FInal" DataField="MontoNetoFinal" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No existen datos.
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div style="display: none">
                                        <asp:TextBox runat="server" ID="RUT_CLIENTE"></asp:TextBox>
                                    </div>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td><b>L/C:</b></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="LBL_LC" Width="300px" CssClass="text-right" Font-Size="Large" Font-Bold="true"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td><b>L/C Disponible:</b></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="LBL_LC_DISPONIBLE" Width="300px" CssClass="text-right" Font-Size="Large" Font-Bold="true"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:LinkButton runat="server" Width="300px" ID="B_VER_CUENTA_CORRIENTE" OnClick="B_VER_CUENTA_CORRIENTE_Click" CssClass="btn btn-success btn-block">VER DOCUMENTO ABIERTOS</asp:LinkButton></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="col-sm-6 text-right">
                                    <b>Neto: &nbsp;</b><asp:TextBox runat="server" ID="LBL_NETO" CssClass="text-right" Font-Size="Large" Font-Bold="true"></asp:TextBox><br />
                                    <b>IVA &nbsp;</b><asp:TextBox runat="server" ID="LBL_IVA" CssClass="text-right" Font-Size="Large" Font-Bold="true"></asp:TextBox><br />
                                    <b>Total: &nbsp;</b><asp:TextBox runat="server" ID="LBL_TOTAL" CssClass="text-right" Font-Size="Large" Font-Bold="true"></asp:TextBox><br />
                                    <b>Forma Pago: &nbsp;</b><asp:TextBox runat="server" ID="LBL_FORMA_PAGO" CssClass="text-right" Font-Size="Large" Font-Bold="true"></asp:TextBox>
                                </div>
                            </div>
                            <div id="DIV_PLANIFICAR_SP" runat="server" visible="false">
                                <hr />
                                <h4><b><i class="fa fa-check text-success"></i>&nbsp;Instruir</b></h4>
                                <br />
                                <div class="row">
                                    <div class="col-sm-2">Fecha Instruccion:</div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="T_FECHA_PLAN" CssClass="form-control" TextMode="Date" runat="server" Width="100%"></asp:TextBox>

                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-2">Nota Correo:</div>
                                    <div class="col-sm-10">
                                        <asp:TextBox runat="server" ID="T_NOTA_CORREO" TextMode="MultiLine" Rows="2" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-2">&nbsp;</div>
                                    <div class="col-sm-4">
                                        <asp:LinkButton runat="server" ID="B_PLANIFICAR_SP" CssClass="btn btn-success btn-block" OnClick="B_PLANIFICAR_SP_Click" OnClientClick="relojito(true);"><i class="fa fa-envelope"></i>&nbsp;Instruir SP</asp:LinkButton>
                                    </div>
                                </div>
                                <hr />
                                <h4><b><i class="fa fa-file-o text-primary"></i>&nbsp;Enviar a Cobranza </b></h4>
                                <br />
                                <div class="row">
                                    <div class="col-sm-2">Detalle:</div>
                                    <div class="col-sm-10">
                                        <asp:TextBox runat="server" ID="T_DETALLE_COBRANZA" TextMode="MultiLine" Rows="2" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-2">&nbsp;</div>
                                    <div class="col-sm-4">
                                        <asp:LinkButton runat="server" ID="B_ENVIAR_COBRANZA" CssClass="btn btn-primary btn-block" OnClick="B_ENVIAR_COBRANZA_Click" OnClientClick="relojito(true);"><i class="fa fa-exclamation-triangle"></i>&nbsp;Enviar a Cobranza</asp:LinkButton>
                                    </div>
                                </div>
                                <hr />
                                <h4><b><i class="fa fa-exclamation-triangle text-warning"></i>&nbsp;Reportar Problemas en SP al Vendedor </b></h4>
                                <br />
                                <div class="row">
                                    <div class="col-sm-2">Detalle:</div>
                                    <div class="col-sm-10">
                                        <asp:TextBox runat="server" ID="DETALLE_REPORTE" TextMode="MultiLine" Rows="2" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-2">&nbsp;</div>
                                    <div class="col-sm-4">
                                        <asp:LinkButton runat="server" ID="B_ENVIAR_REPORTE_SP" CssClass="btn btn-warning btn-block" OnClick="B_ENVIAR_REPORTE_SP_Click" OnClientClick="relojito(true);"><i class="fa fa-exclamation-triangle"></i>&nbsp;Reportar Problemas al Vendedor</asp:LinkButton>
                                    </div>
                                </div>
                                <hr />
                                <h4><b><i class="fa fa-close text-danger"></i>&nbsp;Rechazar SP </b></h4>
                                <br />
                                <div class="row">
                                    <div class="col-sm-2">Motivo:</div>
                                    <div class="col-sm-10">
                                        <asp:TextBox runat="server" ID="MOTIVO_RECHAZO" TextMode="MultiLine" Rows="2" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-2">&nbsp;</div>
                                    <div class="col-sm-4">
                                        <asp:LinkButton runat="server" ID="B_RECHAZAR_SP" CssClass="btn btn-danger btn-block" OnClick="B_RECHAZAR_SP_Click" OnClientClick="relojito(true);"><i class="fa fa-close"></i>&nbsp;Rechazar SP</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" aria-hidden="true" id="cerrar_modal_planificacion">Cerrar</button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <asp:TextBox ID="l_usuario_" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_grupos" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_vendedores" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_clientes" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_productos" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_periodos" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="lb_bodegas2" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="lb_bodegas3" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="lb_planificado" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_bodega" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_grupo_vm" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_vendedor_vm" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="solo_kg" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="SW_MODAL" runat="server" Text="PANEL" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <%-- <a id="btn-scrollup" class="btn btn-circle btn-lg" href="#"> </a>--%>
</asp:Content>
