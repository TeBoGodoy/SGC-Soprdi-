<%@ Page Title="" ViewStateMode="Enabled" Language="C#" MasterPageFile="~/Base.Master" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" AutoEventWireup="True" UICulture="es-ES" Culture="es-ES" CodeBehind="REPORTE_SP_SELECT.aspx.cs" Inherits="SoprodiApp.REPORTE_SP_SELECT" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ OutputCache Location="None" NoStore="true" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>

    <script>

        $(document).keypress(function (event) {
            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {

                checkEnter(event);
                return false;
            }
        });


        $(document).ready(function () {

            $("#div_unidad1").on('hidden.bs.modal', function () {
                document.getElementById("<%=SW_MODAL.ClientID %>").value = "PANEL";
            });

        });



        function checkEnter(e) {

            if (e.keyCode == 13) {

                var sw_modal = document.getElementById("<%=SW_MODAL.ClientID %>").value;
                
                if (sw_modal != "MODAL" ) {
                    document.getElementById("ContentPlaceHolder_Contenido_Button1").click();
                    return false;
                }
            }
        }



        function enter_traer_actualizar_(e) {

            if (e.keyCode == 13)
            {
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

            //quito los options que pudiera tener previamente el combo

            $("#<%=cb_productos_kg.ClientID%>").html("");


            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {

                $("#<%=cb_productos_kg.ClientID%>").append($("<option></option>").attr("value", this.COD_PROD).text(this.NOM_PROD))

            });


            $("#<%=cb_productos_kg.ClientID%>").chosen();
            $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");



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


            $("#<%=tx_tipo_.ClientID%>").value = "";


            $("#<%=tx_valor.ClientID%>").value = "";



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

                $("#<%=cb_productos_kg.ClientID%>").chosen();
                $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");
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

        function creagrilla() {
            try {
                var fecha1 = $('th:contains("FechaDespacho")').index();
                var fecha2 = $('th:contains("FechaEmision")').index();
                var fecha3 = $('th:contains("GxActualizado")').index();
                var fecha4 = $('th:contains("FechaCreacion")').index();

                $("#G_INFORME_TOTAL_VENDEDOR").DataTable({
                    "lengthChange": false,
                    "searching": false,
                    "destroy": true,
                    "pageLength": -1,
                    "paging": false,
                    columnDefs: [
                        { type: 'date-uk', targets: [fecha1, fecha2, fecha3, fecha4] }
                    ],
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    }
                });
                super_ff();
            } catch (e) {
                //alert(e.message);
            }

        }

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
            var elem6 = document.getElementById("<%=div2.ClientID%>");
            elem6.style.visibility = "hidden";


            var elem3 = document.getElementById("<%=cargando_gif.ClientID%>");
            elem3.style.display = "block";

        }


        function CARGANDO2() {


            var elem1 = document.getElementById("<%=txt_hasta.ClientID%>");
            elem1.style.zIndex = "0";

            var elem3 = document.getElementById("<%=carga_2.ClientID%>");
            elem3.style.display = "block";

        }

        function CARGANDO_CLOSE() {


            var elem1 = document.getElementById("<%=txt_hasta.ClientID%>");
            elem1.style.zIndex = "0";

            var elem5 = document.getElementById("<%=div1.ClientID%>");
            elem5.style.visibility = "visible";
            var elem6 = document.getElementById("<%=div2.ClientID%>");
            elem6.style.visibility = "visible";


            var elem3 = document.getElementById("<%=cargando_gif.ClientID%>");
            elem3.style.display = "none";

        }


        function CARGANDO_ENVIAR() {


            var elem3 = document.getElementById("<%=cargando_gif_2.ClientID%>");
            elem3.style.display = "block";
        }



        function CARGA_FECHA() {


            var elem3 = document.getElementById("<%=carga_fecha.ClientID%>");
            elem3.style.display = "block";
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

                $("#<%=cb_productos_kg.ClientID%>").chosen();
                $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");
            } catch (e) { }
        }
        function chosen_update() {

            $("#<%=cb_productos_kg.ClientID%>").chosen();
            $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");

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

            parameters.sw = document.getElementById("<%=cb_productos_kg.ClientID%>").value;;
            parameters.tipo = document.getElementById("<%=tx_tipo_.ClientID%>").value;;
            parameters.valor = document.getElementById("<%=tx_valor.ClientID%>").value;;

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

            parameters.sw = document.getElementById("<%=cb_productos_kg.ClientID%>").value;;

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

    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="900000" EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_excel2" />
            <asp:PostBackTrigger ControlID="btn_excel" />
            <asp:PostBackTrigger ControlID="excel_error" />

            <%--<asp:PostBackTrigger ControlID="btn_estimado" />--%>
        </Triggers>
        <ContentTemplate>


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


                    $("#<%=cb_productos_kg.ClientID%>").change(function () {

                        var arr = $(this).val();
                        document.getElementById("<%=solo_kg.ClientID %>").value = arr;


                    });
                    $("#<%=d_grupo.ClientID%>").change(function () {

                        var arr = $(this).val();
                        document.getElementById("<%=l_grupo_vm.ClientID %>").value = arr;


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

                    $("#<%=d_grupo.ClientID%>").chosen();
                    $("#<%=d_grupo.ClientID%>").trigger("chosen:updated");
                    $("#<%=d_vendedor.ClientID%>").chosen();
                    $("#<%=d_vendedor.ClientID%>").trigger("chosen:updated");

                    $("#<%=CB_BODEGA.ClientID%>").chosen();
                    $("#<%=CB_BODEGA.ClientID%>").trigger("chosen:updated");

              <%--      $("#<%=d_producto.ClientID%>").chosen();
                    $("#<%=d_producto.ClientID%>").trigger("chosen:updated");
--%>


                }

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
                    <li>
                        <a runat="server" id="titulo2"></a>
                        <span class="divider"><i class="fa fa-angle-right"></i></span>
                    </li>
                    <li>
                        <a runat="server" id="titulo3"></a>
                        <span class="divider"><i class="fa fa-angle-right"></i></span>
                    </li>
                    <li class="active">SP Despachos</li>
                </ul>
                <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_gif" runat="server" style="display: none; font-size: 3em;"></i>

            </div>


            <div id="main-content">
                <!-- BEGIN Main Content -->
                <div class="row">
                    <div class="col-md-12" id="div_superior">
                        <div class="box">
                            <asp:Button ID="btn_unidad" runat="server" OnClientClick="modal_unidad();" Style="color: white; margin-right: 1.3%" Class="btn btn-success" Text="Traer//Actualizar SP" />
                            <%--<asp:Button ID="btn_edit_" runat="server" OnClientClick="modal_unidad();" Style="color: white; margin-right: 1.3%" Class="btn btn-warning" Text="Actualizar SP" />--%>

                            <div class="box-title">
                                <h3><i class="fa fa-table"></i>SP Despachos</h3>
                                <div class="box-tool">
                                    <a data-action="collapse" href="#"></a>

                                </div>

                            </div>
                            <div class="box-content">
                                <div class="clearfix"></div>
                                <div class="table-responsive" style="border: 0">

                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="input-group" id="check_fechas">
                                                <asp:RadioButton ID="rd_em" Text="&nbsp;&nbsp;Emisión" Style="padding-right: 20px;" Checked="True"
                                                    GroupName="tipo_usuario" runat="server" />

                                                <asp:RadioButton ID="rd_ven" Style="padding-right: 20px;" Text="&nbsp;&nbsp;Despacho."
                                                    GroupName="tipo_usuario" runat="server" />
                                            </div>


                                        </div>
                                        <div class="col-sm-12">
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box">
                                                <div class="form-group" style="display: block">

                                                    <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;"><b>Desde</b></label>

                                                    <div class="col-sm-3 col-lg-3 controls">
                                                        <div class="input-group">

                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                            <asp:TextBox ID="txt_desde" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender9" runat="server" TargetControlID="txt_desde" Format="dd/MM/yyyy" />

                                                        </div>
                                                    </div>
                                                    <label class="col-sm-1 col-lg-1 control-label"><b>Hasta</b></label>
                                                    <div class="col-sm-3 col-lg-3 controls">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                            <asp:TextBox ID="txt_hasta" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender10" runat="server" TargetControlID="txt_hasta" Format="dd/MM/yyyy" />

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                        <Triggers>
                                        </Triggers>
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="form-group">

                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Cliente</label>
                                                            <div class="col-sm-4 col-lg-4">
                                                                <div class="controls">
                                                                    <asp:ListBox ID="d_bodega" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                </div>
                                                            </div>




                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Estado</label>
                                                            <div class="col-sm-4 col-lg-4">
                                                                <div class="controls">
                                                                    <asp:ListBox ID="d_bodega_2" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                </div>


                                                            </div>



                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="form-group">
                                                            <div style="visibility: hidden; position: absolute;">
                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Grupo</label>
                                                                <div class="col-sm-4 col-lg-4">
                                                                    <div class="controls">
                                                                        <asp:ListBox ID="d_grupo" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                    </div>
                                                                </div>

                                                            </div>


                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Vendedor</label>
                                                            <div class="col-sm-4 col-lg-4">
                                                                <div class="controls">
                                                                    <asp:ListBox ID="d_vendedor" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                </div>


                                                            </div>

                                                            <label class="col-sm-1 control-label">
                                                                Nº SP.(separar por coma):
                                                            </label>
                                                            <div class="col-sm-3 controls">
                                                                <asp:TextBox runat="server" CssClass="form-control" ID="txt_sp"></asp:TextBox>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="form-group">
                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Estado Interno</label>
                                                            <div class="col-sm-4 col-lg-4">
                                                                <div class="controls">
                                                                    <asp:ListBox ID="d_estado_2" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                </div>
                                                            </div>
                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Estado Planificado</label>
                                                            <div class="col-sm-4 col-lg-4">
                                                                <div class="controls">
                                                                    <asp:ListBox ID="d_estado_3" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="form-group">
                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Bodega</label>
                                                            <div class="col-sm-4 controls">
                                                                <%--<asp:DropDownList runat="server" ID="CB_BODEGA" ClientIDMode="Static" Enabled="false" CssClass="form-control chosen"></asp:DropDownList>--%>
                                                                <asp:ListBox ID="CB_BODEGA" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="form-group" style="display: block">




                                                            <div class="col-sm-3 col-lg-3 controls">
                                                                <i class="fa fa-circle-o-notch fa-spin" id="carga_fecha" runat="server" style="font-size: 2em; display: none;"></i>
                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;"></label>

                                                            </div>


                                                        </div>

                                                    </div>
                                                </div>
                                            </div>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box">
                                                <%--  <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Percentil</label>
                                                <div class="col-sm-1 col-lg-1">
                                                    <div class="input-group" style="width: 80px">
                                                        <asp:TextBox ID="t_percentil" runat="server" Text="50" class="form-control" Style="width: 100%" onkeypress="CheckNumeric(event);" MaxLength="2"></asp:TextBox>
                                                        <span class="input-group-addon">%</span>

                                                    </div>
                                                </div>--%>

                                                <div class="pull-right">
                                                    <div class="btn-group">
                                                        <%--<asp:Button ID="btn_productos" runat="server" OnClientClick="CARGANDO();" Style="color: white; float: right;" Class="btn btn-success" OnClick="btn_productos_Click" Text="Productos" />--%>
                                                        <%--<asp:Button ID="btn_informe" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-primary" OnClick="btn_informe_Click" Text="Reporte" />--%>
                                                        <asp:Button ID="Button1" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-primary" OnClick="Button1_Click" Text="Reporte" />
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="div1" runat="server" class="row" style="visibility: visible">
                        <div id="div_report" runat="server" class="row" visible="false">
                            <div class="col-md-12">
                                <div class="box">
                                    <div class="box-title">
                                        <h3><i class="fa fa-table"></i>Detalle</h3>
                                        <div class="box-tool">
                                            <a data-action="collapse" href="#"></a>

                                        </div>
                                    </div>
                                    <div class="box-content">
                                        <%-- <div class="row">
                                            <div class="col-md-12">
                                                <div class="box">
                                                    <div class="col-sm-3 col-lg-3 controls" style="float: right !important;">
                                                        <label>Valor Dolar: </label>
                                                        <asp:TextBox ID="tx_dolar" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                        <label>Valor cobro mensual: </label>
                                                        <asp:TextBox ID="tx_valor_mensual" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                        <asp:Button ID="Button2" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-warning" OnClick="Button2_Click" Text="Calcular Cobro" />
                                                    </div>
                                                    <div class="col-sm-3 col-lg-3 controls">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>--%>



                                        <div class="input-group">
                                            <input type="text" id="t_filtro_memoria" placeholder="Filtrar..." class="form-control" style="width: 300px">
                                            <button class="btn-sm btn btn-primary" type="button" id="btn_filtro_nuevo"><i class="fa fa-search"></i></button>
                                        </div>

                                        <div class="btn-toolbar pull-right">

                                            <div class="btn-group">
                                                <asp:LinkButton ID="btn_excel" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Exportar a Excel" runat="server" OnClick="btn_excel_Click"></asp:LinkButton>
                                            </div>
                                        </div>
                                        <asp:Button ID="btn_eliminar_check" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-warning" OnClick="btn_eliminar_check_Click" Text="Planificar grupo" />

                                        <div class="clearfix"></div>
                                        <div class="table-responsive" style="border: 0">
                                            <%--<asp:TextBox ID="Txt_VENDEDOR" runat="server" Text="" OnTextChanged="Unnamed_TextChanged"></asp:TextBox>--%>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="form-group" style="overflow: auto">




                                                            <%--table table-advance tablesorter filtrar3--%>
                                                            <asp:GridView ID="G_INFORME_TOTAL_VENDEDOR" ClientIDMode="Static" CssClass="table table-bordered table-advance filtrar" OnRowCommand="G_INFORME_TOTAL_VENDEDOR_RowCommand" OnRowDataBound="G_INFORME_TOTAL_VENDEDOR_RowDataBound" runat="server"
                                                                ShowHeaderWhenEmpty="True"
                                                                DataKeyNames="CodDocumento,FechaEmision,CodVendedor,NotaLibre,CodBodega,FechaDespacho,CodMoneda,MontoNeto,DescEstadoDocumento,Facturas,GxEstadoSync,GxActualizado,GxEnviadoERP,NombreVendedor,NombreCliente,DescBodega,FechaCreacion,ValorTipoCambio,LimiteSeguro,TipoCredito,CreditoDisponible,CreditoAutorizado,EmailVendedor"
                                                                Font-Size="12px">
                                                                <HeaderStyle CssClass="test no-sort" />
                                                                <Columns>

                                                                    <asp:TemplateField HeaderText="" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkAccept" runat="server" EnableViewState="true" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Planificar SP">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="B_enviar" runat="server" ImageUrl="img/add_page.png" Width="25"
                                                                                CommandName="Enviar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    No existen datos.
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>

                                                            <asp:GridView ID="G_INFORME_VENDEDOR" CssClass="table table-bordered" OnRowDataBound="G_INFORME_VENDEDOR_RowDataBound" runat="server" Visible="false"
                                                                ShowHeaderWhenEmpty="True" DataKeyNames="cod, ponbr" Font-Size="12px">
                                                                <HeaderStyle CssClass="test no-sort" />
                                                                <Columns>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    No existen datos.
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </div>
                                                        <div id="r_excel" style="visibility: hidden; position: absolute;" runat="server"></div>

                                                        <div class="box-content">
                                                            <div class="row">
                                                                <div class="col-md-3">
                                                                    <h4>Estados</h4>
                                                                    <ul class="list-unstyled">





                                                                        <li><span class="label label-important" style="background-color: #4e53ff;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Aprobado</li>
                                                                        <li><span class="label label-important" style="background-color: #06c5c5;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Aprobado Parcial</li>
                                                                        <li><span class="label label-important" style="background-color: #c2c506;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Aprobado S/ Factura</li>
                                                                        <li><span class="label label-important" style="background-color: #ff10c0;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Pendiente</li>
                                                                        <li><span class="label label-important" style="background-color: #10ff23;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Rechazado</li>
                                                                        <li><span class="label label-important" style="background-color: #fa5a35;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Sincronizado</li>
                                                                        <li><span class="label label-important" style="background-color: #30441f;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Sin Estado</li>
                                                                    </ul>
                                                                </div>

                                                            </div>
                                                        </div>



                                                    </div>
                                                </div>
                                            </div>
                                            <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="carga_2" runat="server" style="display: none; font-size: 3em;"></i>

                                            <hr />
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="form-group" style="overflow: auto; visibility: hidden; position: absolute;">


                                                            <div class="col-sm-4 controls">
                                                                <%--<asp:TextBox runat="server" ID="t_ob_cobro" ClientIDMode="Static" CssClass="form-control input-sm"></asp:TextBox>--%>
                                                                <textarea runat="server" id="t_ob_cobro" class="form-control" placeholder="Observación" rows="2"></textarea>

                                                            </div>
                                                            <div class="col-sm-2 controls">
                                                                <asp:Button ID="btn_estimado" runat="server" Style="color: white;" OnClientClick="CARGANDO2();" Class="btn btn-lime" OnClick="btn_estimado_Click" Text="Agregar Obs" />
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

                    </div>
                    <div id="div2" runat="server" class="row" style="visibility: visible">
                        <div id="div_productos" runat="server" class="row" visible="false">
                            <div class="col-md-12">
                                <div class="box">
                                    <div class="box-title">
                                        <h3><i class="fa fa-table"></i>Detalle Producto/Cliente por vendedor</h3>
                                        <div class="box-tool">
                                            <a data-action="collapse" href="#"></a>

                                        </div>
                                    </div>
                                    <div class="box-content">
                                        <div class="btn-toolbar pull-left">
                                        </div>
                                        <div class="btn-toolbar pull-right">

                                            <div class="btn-group">
                                                <asp:LinkButton ID="btn_excel2" OnClientClick="no_filtro();" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Exportar a Excel" runat="server" OnClick="btn_excel2_Click"></asp:LinkButton>
                                            </div>

                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="table-responsive" style="border: 0">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">

                                                        <div class="form-group" id="DivMainContent" style="overflow-x: auto; overflow-y: auto;">


                                                            <asp:GridView ID="G_PRODUCTOS" AutoGenerateColumns="true" CssClass="table table-bordered filtrar" OnRowCommand="G_PRODUCTOS_RowCommand" OnRowDataBound="G_PRODUCTOS_RowDataBound" runat="server" Visible="true"
                                                                ShowHeaderWhenEmpty="True" Font-Size="12px" DataKeyNames="vendedor, nombrecliente, codvendedor, rutcliente">
                                                                <HeaderStyle CssClass="test no-sort" />

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="#" HeaderStyle-Wrap="false" ItemStyle-Width="4px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="5px" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Productos" HeaderStyle-Wrap="false" ItemStyle-Width="4px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <%-- <asp:ImageButton runat="server" ID="b_edit" ImageUrl="img/icono_carro.png" Width="17"
                                                                                CommandName="Producto" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>

                                                                            <button class="btn" style="background-color: transparent;"><i class="fa fa-shopping-cart"></i></button>

                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="5px" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

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
                    </div>
                </div>




                <a href="#modal_div2" id="div_prod" role="button" class="btn" style="visibility: hidden; position: absolute;" data-toggle="modal"></a>

                <div id="modal_div2" class="modal fade">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <h3 id="myModalLabel">Detalle Productos</h3>
                            </div>
                            <div class="modal-body" style="overflow-x: auto;">



                                <asp:GridView ID="G_DET_PRODUCTOS" AutoGenerateColumns="true" CssClass="table table-bordered" OnRowDataBound="G_DET_PRODUCTOS_RowDataBound" runat="server" Visible="true"
                                    ShowHeaderWhenEmpty="True" Font-Size="12px">
                                    <HeaderStyle CssClass="test" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="#" HeaderStyle-Wrap="false" ItemStyle-Width="4px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <HeaderStyle Width="5px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No existen datos.
                                    </EmptyDataTemplate>
                                </asp:GridView>

                            </div>
                            <div class="modal-footer">
                                <button class="btn" data-dismiss="modal" aria-hidden="true">Cerrar</button>
                            </div>
                        </div>
                    </div>
                </div>




                <a href="#modal_errores" id="errores" role="button" class="btn" style="visibility: hidden; position: absolute;" data-toggle="modal"></a>

                <div id="modal_errores" class="modal fade">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <h3>Errores</h3>
                            </div>
                            <div class="btn-group">
                                <asp:LinkButton ID="excel_error" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Exportar a Excel" runat="server" OnClick="excel_error_Click"></asp:LinkButton>
                            </div>
                            <div class="modal-body" style="overflow-x: auto;">



                                <asp:GridView ID="G_errores" AutoGenerateColumns="true" CssClass="table table-bordered" OnRowDataBound="G_errores_RowDataBound" runat="server" Visible="true"
                                    ShowHeaderWhenEmpty="True" Font-Size="12px">
                                    <HeaderStyle CssClass="test" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="#" HeaderStyle-Wrap="false" ItemStyle-Width="4px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <HeaderStyle Width="5px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No existen datos.
                                    </EmptyDataTemplate>
                                </asp:GridView>

                            </div>
                            <div class="modal-footer">
                                <button class="btn" data-dismiss="modal" aria-hidden="true">Cerrar</button>
                            </div>
                        </div>
                    </div>
                </div>




            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <a href="#div_unidad" id="div_unidad_" role="button" class="btn" style="visibility: hidden; position: absolute;" data-toggle="modal"></a>

    <div id="div_unidad" class="modal fade">
        <div class="modal-dialog modal-lg">
            <div class="modal-content" style="height: 500px;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="myModalLabel">Cambio de unidad</h3>
                </div>
                <div class="modal-body" style="height: 70%;">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                        <Triggers>

                            <%--<asp:AsyncPostBackTrigger ControlID="carga_d" />--%>

                            <%--<asp:AsyncPostBackTrigger ControlID="btn_guardar_prod_equi" />--%>
                        </Triggers>
                        <ContentTemplate>
                            <div class="table-responsive" style="border: 0">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="form-group" style="display: block">

                                                <div class="col-sm-6 controls">
                                                    <div class="input-group" id="check_cerrados2">
                                                        <asp:RadioButton ID="RadioButton1" Text="&nbsp;&nbsp;Cons.Animal" Style="padding-right: 20px;" Checked="True"
                                                            GroupName="rb_productos_2" runat="server" onchange="producto_cambio_animal2()" />

                                                        <asp:RadioButton ID="RadioButton2" Style="padding-right: 20px;" Text="&nbsp;&nbsp;Cons.Humano"
                                                            GroupName="rb_productos_2" runat="server" onchange="producto_cambio_humano2()" />
                                                    </div>
                                                    <div style="float: right; visibility: hidden; position: absolute;">
                                                        Cargar filtros
                                                        <asp:ImageButton ID="ImageButton2" ImageUrl="~/img/Ticket_verde.png" runat="server" OnClientClick="CARGA_FECHA();" OnClick="b_Click" />
                                                        <i class="fa fa-circle-o-notch fa-spin" id="I2" runat="server" style="font-size: 2em; display: none;"></i>

                                                    </div>
                                                </div>

                                                <div class="col-sm-6 col-lg-6">
                                                    <div class="controls">
                                                        <asp:DropDownList runat="server" ID="cb_productos_kg" onchange="esconde()" ClientIDMode="Static" CssClass="form-control chosen"></asp:DropDownList>
                                                    </div>


                                                </div>

                                            </div>

                                            <input id="cierra_acciones" type="button" class="btn btn-warning" value="Aceptar" onclick="carga_d_Click()" style="display: block; float: right;" />

                                            <%--<asp:Button ID="carga_d" runat="server" Style="color: white; float: right; margin-right: 1.3%" OnClick="carga_d_Click" Class="btn btn-warning" Text="Aceptar" />--%>
                                        </div>
                                    </div>
                                </div>

                                <br />
                                <div runat="server" id="R_Excel_1" style="display: none">
                                </div>
                                <br />
                                <div class="row" id="detalle_prod" style="visibility: hidden">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="form-group" style="display: block">
                                                <div class="col-sm-6 col-lg-6">
                                                    <div class="controls">
                                                        <label>Tipo de UNIDAD</label>
                                                        <asp:TextBox runat="server" ID="tx_tipo_"></asp:TextBox>
                                                    </div>


                                                </div>
                                                <div class="col-sm-6 col-lg-6">
                                                    <div class="controls">
                                                        <label>Equivale a </label>
                                                        <asp:TextBox runat="server" ID="tx_valor"></asp:TextBox>
                                                        Kilos
                                                    </div>


                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <%--<asp:Button ID="btn_guardar_prod_equi" runat="server" Style="color: white; float: right; margin-right: 1.3%" OnClick="Button3_Click" Class="btn btn-success" Text="Guardar" />--%>
                                </div>
                                <input id="btn_guardar_prod_equi" type="button" class="btn btn-success" value="Guardar" onclick="guardar_solo_kg()" style="display: block; float: right;" />

                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button class="btn" data-dismiss="modal" onclick="esconde(); chosen_update();" aria-hidden="true">Cerrar</button>
                </div>
            </div>
        </div>
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

    <a href="#div_unidad1" id="div_unidad_1" role="button" class="btn" style="visibility: hidden; position: absolute;" data-toggle="modal"></a>

    <div id="div_unidad1" class="modal fade">
        <div class="modal-dialog modal-lg">
            <div class="modal-content" style="height: auto;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <div id="titulo_modal"></div>
                </div>
                <div class="modal-body" style="height: 70%;">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                        <Triggers>

                            <%--<asp:AsyncPostBackTrigger ControlID="carga_d" />--%>

                            <%--<asp:AsyncPostBackTrigger ControlID="btn_guardar_prod_equi" />--%>
                        </Triggers>
                        <ContentTemplate>
                            <div class="table-responsive" style="border: 0">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="form-group" style="display: block">

                                                <div class="col-sm-6 col-lg-6">
                                                    <label>SP: </label>
                                                    <div class="controls">
                                                        <asp:TextBox runat="server" onkeypress="enter_(event);" class="form-control" ID="tx_sp"></asp:TextBox>
                                                    </div>


                                                </div>
                                                <div runat="server" id="tabla_sp"></div>

                                            </div>


                                            <%--<button id="Button21" class="btn btn-success icon-plus btn-circle" onclick="incluir_producto();"><i class="fa fa-plus"></i></button>--%>
                                            <%--<input id="btn_guardar_prod3_equi" type="button" class="btn btn-success" value="Enviar" onclick="incluir_producto()" style="display: block; float: right;" />--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="row>">

                                    <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_gif_2" runat="server" style="display: none; font-size: 3em;"></i>

                                </div>
                                <br />

                                <%--          <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                <div class="row">
                                    <asp:Button runat="server" class="btn btn-success" ID="guarda_traer_actualizar" Text="Guardar" OnClick="Unnamed_Click" OnClientClick="CARGANDO_ENVIAR();"></asp:Button>
                                    <div class="col-sm-12">

                                        <div id="tabla_productos"></div>

                                    </div>
                                </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

            </div>
            <div class="modal-footer">
                <button class="btn" data-dismiss="modal" onclick="esconde(); chosen_update();" aria-hidden="true">Cerrar</button>
            </div>
        </div>
    </div>
    <%-- <a id="btn-scrollup" class="btn btn-circle btn-lg" href="#"> </a>--%>
</asp:Content>
