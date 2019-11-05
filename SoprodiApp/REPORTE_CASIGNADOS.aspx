<%@ Page Title="" ViewStateMode="Enabled" Language="C#" MasterPageFile="~/Base.Master" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" AutoEventWireup="True" UICulture="es-ES" Culture="es-ES" CodeBehind="REPORTE_CASIGNADOS.aspx.cs" Inherits="SoprodiApp.REPORTE_CASIGNADOS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ OutputCache Location="None" NoStore="true" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">


    <%--INCLUIR JQUERY !!--%>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>

    <script>

        $(document).ready(function () {
            creagrilla();
        });

        $(document).keypress(function (event) {
            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {

                checkEnter(event);
                return false;
            }
        });


        function checkEnter(e) {

            if (e.keyCode == 13) {
                document.getElementById("ContentPlaceHolder_Contenido_Button1").click();
                return false;
            }
        }

        //var config = {
        //    '.chosen-select': {},
        //    '.chosen-select-deselect': { allow_single_deselect: true },
        //    '.chosen-select-no-single': { disable_search_threshold: 10 },
        //    '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' },
        //    '.chosen-select-width': { width: "90%" }
        //}
        //for (var selector in config) {
        //    $(selector).chosen(config[selector]);
        //}

        //function SORT_GRILLA() {
        //    new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_PRODfUCTOS'));
        //}

        function modal_unidad_1(cod_doc) {
            //alert("aca" + cod_doc);
            $('#titulo_modal').html("<h3 style='color: cornflowerblue;font-size: -webkit-xxx-large;'>" + cod_doc + "</h3>");
            document.getElementById("div_unidad_1").click();
            //load_chosen_combos();
        }
        function modal_unidad_titulo(cod_doc) {
            //alert("aca" + cod_doc);
            $('#titulo_modal').html("<h3 style='color: cornflowerblue;font-size: -webkit-xxx-large;'>" + cod_doc + "</h3>");
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
        function guarda() {

            alert('Correo Enviado');

        }

        function incluir_producto() {
            var parameters = new Object();
<%--            parameters.PARA = document.getElementById("<%=tx_para.ClientID%>").value;;
            parameters.TEXT = document.getElementById("<%=tx_text_.ClientID%>").value;;--%>
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
            });
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
        function cerrado() {

            var elem3 = document.getElementById("div_cierre_camion");
            elem3.style = "background:#ff000026";

        }

        function devul_fal() {
            //return false;
        }

        function CargarEvento_Tabla(id) {

            //alert(id);
            //alert(id + "---" + rut + "---" + fact + "---" + tipo_doc)
            var parameters = new Object();
            parameters.id = id;
            parameters = JSON.stringify(parameters)

            $.ajax({
                type: "POST",
                url: "REPORTE_CASIGNADOS.aspx/CargarEvento55",
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
            }).done(function (result) {



                $('#acciones_f').html(result.d);
                $('#modal_5').click();

            });
        }

        function CargarEvento_Tabla2(id) {

            //alert(id);
            //alert(id + "---" + rut + "---" + fact + "---" + tipo_doc)
            var parameters = new Object();
            parameters.id = id;
            parameters = JSON.stringify(parameters)

            $.ajax({
                type: "POST",
                url: "REPORTE_CASIGNADOS.aspx/CargarEventolog",
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
            }).done(function (result) {


                $('#acciones_f').html(result.d);
                $('#modal_5').click();

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
                    "order": [[10, "asc"]],
                    "lengthChange": false,
                    "searching": false,
                    "destroy": true,
                    "stateSave": true,
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
            }
            catch (e) {
                //alert(e.message);
                console.log(e.message);
            }

        }

        function sp_select(cod_prod) {


            var urlPdf = "/REPORTE_CASIGNADO.aspx?";
            //var path2 = "P=" + path;
            //var filename2 = "&N=" + filename;
            //var urlPdf_Final = urlPdf + path2 + filename2;
            var param = "c=" + cod_prod;
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

        function checkEnter(e) {
            if (e.keyCode == 13) {
                document.getElementById("ContentPlaceHolder_Contenido_Button1").click();
                return false;
            }
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

<%--            var elem5 = document.getElementById("<%=div1.ClientID%>");
            elem5.style.visibility = "hidden";
            var elem6 = document.getElementById("<%=div2.ClientID%>");
            elem6.style.visibility = "hidden";--%>


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

            $("#<%=CB_BODEGA.ClientID%>").chosen();
            $("#<%=CB_BODEGA.ClientID%>").trigger("chosen:updated");

            $("#<%=cb_productos_kg.ClientID%>").chosen();
            $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");

            $("#<%=d_bodega.ClientID%>").chosen();
            $("#<%=d_bodega.ClientID%>").trigger("chosen:updated");

            $("#<%=d_bodega_2.ClientID%>").chosen();
            $("#<%=d_bodega_2.ClientID%>").trigger("chosen:updated");
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



        function LoadTranspor(result) {

            //quito los options que pudiera tener previamente el combo

            $("#<%=d_transpor.ClientID%>").html("");
            $("#<%=d_camion.ClientID%>").html("");
            $("#<%=d_chofer.ClientID%>").html("");

            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {

                $("#<%=d_transpor.ClientID%>").append($("<option></option>").attr("value", this.cod_trans).text(this.nombre_trans))

            });

            $("#<%=d_bodega_3.ClientID%>").chosen();
            $("#<%=d_bodega_3.ClientID%>").trigger("chosen:updated");
            $("#<%=d_transpor.ClientID%>").chosen();
            $("#<%=d_transpor.ClientID%>").trigger("chosen:updated");
            $("#<%=d_camion.ClientID%>").chosen();
            $("#<%=d_camion.ClientID%>").trigger("chosen:updated");
            $("#<%=d_chofer.ClientID%>").chosen();
            $("#<%=d_chofer.ClientID%>").trigger("chosen:updated");

        }


        function LoadCamion(result) {

            //quito los options que pudiera tener previamente el combo

            $("#<%=d_camion.ClientID%>").html("");

            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {

                $("#<%=d_camion.ClientID%>").append($("<option></option>").attr("value", this.cod_camion).text(this.patente))

            });

            $("#<%=d_bodega_3.ClientID%>").chosen();
            $("#<%=d_bodega_3.ClientID%>").trigger("chosen:updated");
            $("#<%=d_transpor.ClientID%>").chosen();
            $("#<%=d_transpor.ClientID%>").trigger("chosen:updated");
            $("#<%=d_camion.ClientID%>").chosen();
            $("#<%=d_camion.ClientID%>").trigger("chosen:updated");
            $("#<%=d_chofer.ClientID%>").chosen();
            $("#<%=d_chofer.ClientID%>").trigger("chosen:updated");

        }



        function LoadChofer(result) {

            //quito los options que pudiera tener previamente el combo

            $("#<%=d_chofer.ClientID%>").html("");

            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {

                $("#<%=d_chofer.ClientID%>").append($("<option></option>").attr("value", this.cod_chofer).text(this.nombre_chofer))

            });

            $("#<%=d_bodega_3.ClientID%>").chosen();
            $("#<%=d_bodega_3.ClientID%>").trigger("chosen:updated");
            $("#<%=d_transpor.ClientID%>").chosen();
            $("#<%=d_transpor.ClientID%>").trigger("chosen:updated");
            $("#<%=d_camion.ClientID%>").chosen();
            $("#<%=d_camion.ClientID%>").trigger("chosen:updated");
            $("#<%=d_chofer.ClientID%>").chosen();
            $("#<%=d_chofer.ClientID%>").trigger("chosen:updated");

        }


        function load_chosen_combos() {
            $("#<%=d_bodega_3.ClientID%>").chosen();
            $("#<%=d_bodega_3.ClientID%>").trigger("chosen:updated");
            $("#<%=d_transpor.ClientID%>").chosen();
            $("#<%=d_transpor.ClientID%>").trigger("chosen:updated");
            $("#<%=d_camion.ClientID%>").chosen();
            $("#<%=d_camion.ClientID%>").trigger("chosen:updated");
            $("#<%=d_chofer.ClientID%>").chosen();
            $("#<%=d_chofer.ClientID%>").trigger("chosen:updated");

            $("#ContentPlaceHolder_Contenido_d_bodega_3_chosen").css({ "width": "400px" });


            $("#ContentPlaceHolder_Contenido_d_transpor_chosen").css({ "width": "400px" });


            $("#ContentPlaceHolder_Contenido_d_camion_chosen").css({ "width": "400px" });


            $("#ContentPlaceHolder_Contenido_d_chofer_chosen").css({ "width": "400px" });



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
            <asp:AsyncPostBackTrigger ControlID="G_INFORME_TOTAL_VENDEDOR" EventName="RowCommand" />
            <%--<asp:PostBackTrigger ControlID="btn_estimado" />--%>
        </Triggers>
        <ContentTemplate>


            <!-- MODAL DE acciones ( F ) -->
            <a id="modal_5" name="modal_5" href="#modal-5" role="button" class="btn" data-toggle="modal"></a>
            <div id="modal-5" class="modal fade">
                <div class="modal-dialog modal-lg" style="width: 90%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" id="btn_modal_5_cerrar" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <%--  <h3 id="H1"><i id="I1"></i></h3>--%>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-horizontal">
                                        <h3>DETALLE PLANIFICACIÓN</h3>
                                        <div id="acciones_f">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" aria-hidden="true" id="Button98">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>




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



                    $("#<%=CB_BODEGA.ClientID%>").change(function () {

                        // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                        //este parametro mapeara con el definido en el web service

                        var arr = $(this).val();
                        document.getElementById("<%=l_bodega_2.ClientID %>").value = arr;

                    });



                    $("#<%=d_bodega_2.ClientID%>").chosen();
                    $("#<%=d_bodega_2.ClientID%>").trigger("chosen:updated");
                    $("#<%=d_bodega.ClientID%>").chosen();
                    $("#<%=d_bodega.ClientID%>").trigger("chosen:updated");

                    $("#<%=d_grupo.ClientID%>").chosen();
                    $("#<%=d_grupo.ClientID%>").trigger("chosen:updated");
                    $("#<%=d_vendedor.ClientID%>").chosen();
                    $("#<%=d_vendedor.ClientID%>").trigger("chosen:updated");

                    $("#<%=CB_BODEGA.ClientID%>").chosen();
                    $("#<%=CB_BODEGA.ClientID%>").trigger("chosen:updated");

                    $("#<%=d_bodega_3.ClientID%>").change(function () {

                        // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                        //este parametro mapeara con el definido en el web service
                        var arr = $(this).val();
                        document.getElementById("<%=l_bodega.ClientID %>").value = arr;

                        var parameters = new Object();
                        parameters.BODEGA = document.getElementById("<%=l_bodega.ClientID %>").value;

                        parameters = JSON.stringify(parameters);

                        $.ajax({
                            type: "POST",
                            url: "REPORTE_CASIGNADOS.aspx/BODEGA",
                            data: parameters,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: LoadTranspor,
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert("Error al cargar transporte");
                            }
                        });
                        $("#<%=d_bodega_3.ClientID%>").chosen();
                        $("#<%=d_bodega_3.ClientID%>").trigger("chosen:updated");
                    });

                    $("#<%=d_transpor.ClientID%>").change(function () {

                        // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                        //este parametro mapeara con el definido en el web service
                        var arr = $(this).val();
                        document.getElementById("<%=l_transpor.ClientID %>").value = arr;
                        var parameters = new Object();
                        parameters.TRANSPOR = document.getElementById("<%=l_transpor.ClientID %>").value;
                        parameters = JSON.stringify(parameters);
                        $.ajax({
                            type: "POST",
                            url: "REPORTE_CASIGNADOS.aspx/TRANSPOR_CHOFER",
                            data: parameters,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: LoadChofer,
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert("Error al cargar chofer");
                            }
                        });

                        $.ajax({
                            type: "POST",
                            url: "REPORTE_CASIGNADOS.aspx/TRANSPOR_CAMION",
                            data: parameters,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: LoadCamion,
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert("Error al cargar camion");
                            }
                        });


                        $.ajax({
                            type: "POST",
                            url: "REPORTE_CASIGNADOS.aspx/CARGA_INICIAL_TRANSPORTE",
                            data: parameters,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert("Error al cargar carga incial");
                            }
                        }).done(function (resp) {
                            resp = resp.d;
                            document.getElementById("<%=inputTitle.ClientID %>").value = resp;


                        });

                        $("#<%=d_transpor.ClientID%>").chosen();
                        $("#<%=d_transpor.ClientID%>").trigger("chosen:updated");

                    });


                    $("#<%=d_camion.ClientID%>").change(function () {

                        // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                        //este parametro mapeara con el definido en el web service
                        var arr = $(this).val();
                        document.getElementById("<%=l_camion.ClientID %>").value = arr;
                        var parameters = new Object();
                        parameters.CAMION = document.getElementById("<%=l_camion.ClientID %>").value;
                        parameters = JSON.stringify(parameters);

                        $.ajax({
                            type: "POST",
                            url: "REPORTE_CASIGNADOS.aspx/CAMION_CARGA",
                            data: parameters,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert("Error al cargar camion");
                            }
                        }).done(function (resp) {
                            resp = resp.d;
                            document.getElementById("<%=inputTitle.ClientID %>").value = resp;


                        });



                        $("#<%=d_camion.ClientID%>").chosen();
                        $("#<%=d_camion.ClientID%>").trigger("chosen:updated");

                    });


                    $("#<%=d_chofer.ClientID%>").change(function () {

                        // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                        //este parametro mapeara con el definido en el web service
                        var arr = $(this).val();
                        document.getElementById("<%=l_chofer.ClientID %>").value = arr;
                        var parameters = new Object();

                        $("#<%=d_chofer.ClientID%>").chosen();
                        $("#<%=d_chofer.ClientID%>").trigger("chosen:updated");

                    });


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
                    <li class="active">SP Asignados</li>
                </ul>
                <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_gif" runat="server" style="display: none; font-size: 3em;"></i>

            </div>


            <div id="main-content">
                <!-- BEGIN Main Content -->
                <div class="row">
                    <div class="col-md-12" runat="server" id="div_superior">
                        <div class="box">
                            <div class="box-title">
                                <h3><i class="fa fa-table"></i>SP Asignados</h3>
                                <div class="box-tool">
                                    <a data-action="collapse" href="#"></a>

                                </div>

                            </div>
                            <div class="box-content">
                                <div class="clearfix"></div>
                                <div class="table-responsive" style="border: 0">



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
                                        <h3 runat="server" id="h3_transporte"></h3>
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
                                        <asp:Button ID="btn_eliminar_check" runat="server" OnClientClick="CARGANDO();" Style="color: white; visibility: hidden;" Class="btn btn-warning" OnClick="btn_eliminar_check_Click" Text="Planificar grupo" />

                                        <div class="clearfix"></div>
                                        <div class="table-responsive" style="border: 0">
                                            <%--<asp:TextBox ID="Txt_VENDEDOR" runat="server" Text="" OnTextChanged="Unnamed_TextChanged"></asp:TextBox>--%>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="form-group" style="overflow: auto">




                                                            <%--table table-advance tablesorter filtrar3--%>
                                                            <asp:GridView ID="G_INFORME_TOTAL_VENDEDOR" ClientIDMode="Static" CssClass="table table-bordered table-advance filtrar" OnRowCommand="G_INFORME_TOTAL_VENDEDOR_RowCommand" OnRowDataBound="G_INFORME_TOTAL_VENDEDOR_RowDataBound" runat="server"
                                                                ShowHeaderWhenEmpty="True" AutoGenerateColumns="false"
                                                                DataKeyNames="id, coddocumento, cod_trans, estado, nombre_trans, codbodega, nombrecliente, fecha_despacho2, tiene_log, cod_camion, cod_chofer, observacion, bodega_plani, carga_inicial, patente, nombre_chofer, fecha_type_date, vuelta"
                                                                Font-Size="12px">
                                                                <HeaderStyle CssClass="test no-sort" />
                                                                <Columns>
                                                                    <%--     -----------------------------------------------------------------------------------------------------------     0    --------------%>
                                                                    <asp:TemplateField HeaderText="Quitar">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btn_quitar" runat="server" ImageUrl="img/delete.png" Width="25"
                                                                                CommandName="Eliminar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClientClick='<%# confirmDelete( Eval("CODDOCUMENTO").ToString() ) %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <%--     -----------------------------------------------------------------------------------------------------------     1    --------------%>
                                                                    <asp:TemplateField HeaderText="Editar">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btn_img" runat="server" ImageUrl="img/pencil.png" Width="25"
                                                                                CommandName="Editar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <%--     -----------------------------------------------------------------------------------------------------------     2    --------------%>
                                                                    <asp:TemplateField HeaderText="Completar">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton runat="server" ImageUrl="img/accept.png" Width="25"
                                                                                CommandName="Enviar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <%--     -----------------------------------------------------------------------------------------------------------     3    --------------%>
                                                                    <asp:BoundField DataField="coddocumento" HeaderText="CodDocum.">
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <%--     -----------------------------------------------------------------------------------------------------------     4    --------------%>
                                                                    <asp:BoundField DataField="nombrecliente" HeaderText="Cliente">
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <%--     -----------------------------------------------------------------------------------------------------------     5    --------------%>
                                                                    <asp:BoundField DataField="nombrevendedor" HeaderText="Vendedor">
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <%--     -----------------------------------------------------------------------------------------------------------     6    --------------%>
                                                                    <asp:BoundField DataField="nombre_transporte_todo" HeaderText="Transporte">
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>

                                                                    <%--                      <asp:BoundField DataField="patente" HeaderText="Patente Camión">
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>

                                                                    <asp:BoundField DataField="nombre_chofer" HeaderText="Chofer">
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>--%>
                                                                    <%--     -----------------------------------------------------------------------------------------------------------     7    --------------%>
                                                                    <asp:BoundField DataField="observacion" HeaderText="OBS">
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <%--     -----------------------------------------------------------------------------------------------------------     8    --------------%>
                                                                    <asp:BoundField DataField="carga_total" DataFormatString="{0:N0}" HeaderText="CargaCamion">
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <%--     -----------------------------------------------------------------------------------------------------------     9    --------------%>
                                                                    <asp:BoundField DataField="disponible_2" DataFormatString="{0:N0}" HeaderText="DisponibleCamion">
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <%--     -----------------------------------------------------------------------------------------------------------     10    --------------%>
                                                                    <asp:BoundField DataField="fin" DataFormatString="{0:N0}" HeaderText="NetoPeso">
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <%--     -----------------------------------------------------------------------------------------------------------     11    --------------%>
                                                                    <asp:BoundField DataField="fecha_despacho2" HeaderText="FechaDespacho">
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <%--     -----------------------------------------------------------------------------------------------------------     12    --------------%>
                                                                    <asp:BoundField DataField="codbodega" HeaderText="Bodega">
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <%--     -----------------------------------------------------------------------------------------------------------     13    --------------%>
                                                                    <asp:BoundField DataField="descbodega" HeaderText="DescBodega">
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>

                                                                    <%----------%>

                                                                    <%--                                                                    <asp:BoundField DataField="cod_camion" HeaderText="1">
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>

                                                                    <asp:BoundField DataField="cod_chofer" HeaderText="2">
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>--%>
                                                                </Columns>



                                                                <EmptyDataTemplate>
                                                                    No existen datos.
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>

                                                        </div>

                                                        <div id="div_totales" runat="server" class="row" visible="false">
                                                            <div class="col-md-4 col-md-offset-8">
                                                                <div class="box box-blue" style="margin-top: 1%;">

                                                                    <div class="box-title">
                                                                        <h3><i class="fa fa-bars"></i>TOTALES</h3>
                                                                        <div class="box-tool">
                                                                        </div>
                                                                    </div>
                                                                 
                                                                    <div class="box-content" style="background-color: #f7f7f7;">
                                                                        <div class="form-horizontal">
                                                                            <div class="row" style="margin-right: 10px; margin-left: 10px;">
                                                                                <div class="col-md-5">
                                                                                    <div class="form-group">
                                                                                        <div class="img">
                                                                                            <i class="fa fa-truck" style="font-size: 30px;"></i>
                                                                                        </div>
                                                                                        <div class="content">
                                                                                            <p class="big" id="total_carga" style="font-size: 20px;"></p>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-2">
                                                                                </div>
                                                                                <div class="col-md-5">
                                                                                    <div class="form-group">
                                                                                        <div class="img">                                                 
                                                                                            <span class="glyphicon glyphicon-usd" style="font-size: 30px;"></span>
                                                                                        </div>
                                                                                        <div class="content">
                                                                                            <p class="big" id="total_peso" style="font-size: 20px;"></p>                                                    
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="box-content">
                                                            <div class="row">
                                                                <div class="col-md-3">
                                                                    <h4>Estados</h4>
                                                                    <ul class="list-unstyled">
                                                                        <li><span class="label label-important" style="background-color: #4e53ff;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Pendiente</li>
                                                                        <li><span class="label label-important" style="background-color: #fa5a35;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Finalizado</li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <asp:UpdatePanel runat="server" ID="UpdatePanel5" UpdateMode="Conditional">
                                                            <Triggers>
                                                                <asp:PostBackTrigger ControlID="enviar_Correo_camion" />
                                                            </Triggers>
                                                            <ContentTemplate>

                                                                <div id="div_adjuntar_documentos" runat="server" class="row" visible="false">
                                                                    <div class="col-md-5">
                                                                        <div class="box box-green">
                                                                            <div class="box-title">
                                                                                <h3><i class="fa fa-check"></i>ADJUNTAR DOCUMENTOS AL CORREO</h3>
                                                                                <div class="box-tool">
                                                                                </div>
                                                                            </div> 
                                                                            <div class="box-content" style="background-color: #f7f7f7;height: 100px; ">
                                                                                <div class="form-group">
                                                                                    <label class="col-sm-3 col-lg-2 control-label">Archivos</label>
                                                                                    <div class="col-sm-9 col-lg-10 controls">
                                                                                        <asp:FileUpload runat="server" multiple="true" ID="fu_archivos_camion" CssClass="form-control" />
                                                                                    </div>


                                                                                    <label class="col-sm-3 col-lg-2 control-label">Correos por Bodega</label>
                                                                                    <div class="col-sm-9 col-lg-10 controls">
                                                                                        <asp:TextBox ID="t_correos" TextMode="multiline" Columns="63" Rows="5" runat="server" />
                                                                                    </div>

                                                                                </div>
                                                                                <div class="form-group">
                                                                                </div>
                                                                                <%--<asp:Button ID="bn_subir_excel" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-primary" OnClick="bn_subir_excel_Click" Text="Subir" />--%>
                                                                                <asp:ImageButton runat="server" Width="50" ImageUrl="~/img/email.png" class="btn btn-circle" ClientIDMode="Static" ID="enviar_Correo_camion" Style="visibility: visible;" OnClick="enviar_Correo_camion_Click" />

                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>

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


                    <div id="div3" runat="server" class="row" style="visibility: visible">
                        <div id="div_agregar_facturas" runat="server" class="row" visible="false">
                            <div class="col-md-12">
                                <div class="box">
                                    <div class="box-title">
                                        <h3><i class="fa fa-table"></i>Agregar Factura al Camión</h3>
                                        <div class="box-tool">
                                            <a data-action="collapse" href="#"></a>
                                        </div>
                                    </div>
                                    <div class="box-content" id="div_cierre_camion">
                                        <div class="btn-toolbar pull-left">
                                        </div>
                                        <div class="btn-toolbar pull-right">

                                            <div class="btn-group">
                                                <%--<asp:LinkButton ID="excel" OnClientClick="no_filtro();" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Exportar a Excel" runat="server" OnClick="btn_excel2_Click"></asp:LinkButton>--%>
                                            </div>

                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="table-responsive" style="border: 0">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="form-group" style="overflow-x: auto; overflow-y: auto;">
                                                            <asp:TextBox runat="server" ID="tx_factura" class="form-control"></asp:TextBox>
                                                            <asp:Button runat="server" class="btn btn-success" ID="btn_validar_factura" OnClick="btn_validar_factura_Click" Text="Validar y Agregar Factura" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">



                                                        <asp:GridView ID="G_FACTURAS" AutoGenerateColumns="false" CssClass="table table-bordered" OnRowDataBound="G_FACTURAS_RowDataBound" OnRowCommand="G_FACTURAS_RowCommand" runat="server" Visible="true"
                                                            ShowHeaderWhenEmpty="True" Font-Size="12px" DataKeyNames="factura">
                                                            <HeaderStyle CssClass="test" />

                                                            <Columns>

                                                                <asp:TemplateField HeaderText="Quitar" HeaderStyle-Width="30px">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="5%" CssClass="text-center" />
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btn_quitar" runat="server" ImageUrl="img/delete.png" Width="25"
                                                                            CommandName="Eliminar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:BoundField DataField="factura" HeaderText="Factura">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>

                                                                <asp:BoundField DataField="estado" HeaderText="Estado">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>

                                                            </Columns>

                                                            <EmptyDataTemplate>
                                                                No existen datos.
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>


                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">

                                                <asp:Button runat="server" class="btn btn-warning" ID="btn_cerrar_camion" OnClick="btn_cerrar_camion_Click" Text="Cerrar Camión" />


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

                                    <input id="btn_guardar_prod_equi" type="button" class="btn btn-success" value="Guardar" onclick="guardar_solo_kg()" style="display: block; float: right;" />

                                    <%--<asp:Button ID="btn_guardar_prod_equi" runat="server" Style="color: white; float: right; margin-right: 1.3%" OnClick="Button3_Click" Class="btn btn-success" Text="Guardar" />--%>
                                </div>
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
    <asp:TextBox ID="l_bodega_2" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>

    <asp:TextBox ID="l_grupo_vm" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_vendedor_vm" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>


    <asp:TextBox ID="solo_kg" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>


    <a href="#div_unidad1" id="div_unidad_1" role="button" class="btn" style="visibility: hidden; position: absolute;" data-toggle="modal"></a>

    <div id="div_unidad1" class="modal fade">
        <div class="modal-dialog modal-lg">
            <div class="modal-content" style="height: auto; width: 120%; left: -10%;">
                <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                    <Triggers>

                        <%--<asp:AsyncPostBackTrigger ControlID="carga_d" />--%>

                        <%--<asp:AsyncPostBackTrigger ControlID="btn_guardar_prod_equi" />--%>
                        <asp:AsyncPostBackTrigger ControlID="BTN_REFRESH" />
                    </Triggers>
                    <ContentTemplate>


                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <div id="titulo_modal"></div>

                            <%--<a id="BTN_REFRESH" runat="server" class="btn btn-info icon-plus btn-circle" onserverclick="refresh_edit_"><i class="fa fa-refresh"></i></a>--%>

                            <asp:LinkButton ID="BTN_REFRESH" class="btn btn-info icon-plus btn-circle fa fa-refresh" Visible="true" title="Refrescar" runat="server" OnClick="refresh_edit_"></asp:LinkButton>

                        </div>
                        <div class="modal-body" style="height: 70%;">

                            <div class="table-responsive" style="border: 0">

                                <asp:TextBox ID="l_bodega" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                                <asp:TextBox ID="l_transpor" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                                <asp:TextBox ID="l_camion" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                                <asp:TextBox ID="l_chofer" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>



                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="form-group" style="display: block">

                                                <div class="col-sm-6 col-lg-6">
                                                    <label>Bodega: </label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="d_bodega_3" SelectionMode="Single" data-placeholder=" -- Todos -- " Style="width: 100% !important;" runat="server" class="form-control chosen-select"></asp:DropDownList>
                                                    </div>
                                                </div>


                                                <div class="col-sm-6 col-lg-6">
                                                    <label>Transportista: </label>
                                                    <div class="controls">
                                                        <asp:ListBox ID="d_transpor" SelectionMode="Single" data-placeholder=" -- Todos -- " Style="width: 100% !important;" runat="server" class="form-control chosen-select"></asp:ListBox>
                                                    </div>
                                                    <a id="btn_plus_transpo" class="btn btn-success icon-plus btn-circle" href="MANT_TRANSPORT.aspx" target='_blank'><i class="fa fa-plus"></i></a>
                                                </div>

                                                <div class="col-sm-6 col-lg-6">
                                                    <label>Camión: </label>
                                                    <div class="controls">
                                                        <asp:ListBox ID="d_camion" SelectionMode="Single" data-placeholder=" -- Todos -- " Style="width: 100% !important;" runat="server" class="form-control chosen-select"></asp:ListBox>
                                                    </div>
                                                    <%--<button id="btn_plus_camion" class="btn btn-success icon-plus btn-circle" runat="server" onserverclick="btn_plus_camion_ServerClick"><i class="fa fa-plus"></i></button>--%>
                                                    <a id="btn_plus_camion" class="btn btn-success icon-plus btn-circle" href="MANT_CAMION.aspx" target='_blank'><i class="fa fa-plus"></i></a>
                                                </div>

                                                <div class="col-sm-6 col-lg-6">
                                                    <label>Chofer: </label>
                                                    <div class="controls">
                                                        <asp:ListBox ID="d_chofer" SelectionMode="Single" data-placeholder=" -- Todos -- " Style="width: 100% !important;" runat="server" class="form-control chosen-select"></asp:ListBox>
                                                        <a id="btn_plus_chofer" class="btn btn-success icon-plus btn-circle" href="MANT_CHOFER.aspx" target='_blank'><i class="fa fa-plus"></i></a>
                                                    </div>
                                                </div>

                                                <div class="col-sm-6 col-lg-6">
                                                    <label>Fecha: </label>
                                                    <div class="controls">
                                                        <asp:TextBox runat="server" type="date" class="form-control" ID="t_fecha_despach2"></asp:TextBox>
                                                        <%--<ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="t_fecha_despach2" Format="dd/MM/yyyy" />--%>
                                                    </div>
                                                </div>

                                                <div class="col-sm-6 col-lg-6">
                                                    <label>Carga Inicial: </label>
                                                    <div class="controls">
                                                        <asp:TextBox ClientIDMode="Static" type="number" onkeydown="return(event.keyCode!=13);" class="form-control" runat="server" ID="inputTitle"></asp:TextBox>

                                                    </div>
                                                </div>

                                                <div class="col-sm-6 col-lg-6">
                                                    <label>Vuelta: </label>
                                                    <div class="controls">
                                                        <asp:TextBox ClientIDMode="Static" type="number" onkeydown="return(event.keyCode!=13);" class="form-control" runat="server" ID="tx_vuelta"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="col-sm-6 col-lg-6">
                                                    <label>Observación: </label>
                                                    <div class="controls">
                                                        <asp:TextBox ClientIDMode="Static" TextMode="multiline" Columns="50" Rows="5" class="form-control" runat="server" ID="tx_obs_plani"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="form-group" style="display: block">

                                                <asp:GridView ID="G_DETALLE_PLANIFICADO" AutoGenerateColumns="false" CssClass="table table-bordered filtrar" OnRowDataBound="G_DETALLE_PLANIFICADO_RowDataBound" runat="server" Visible="true"
                                                    ShowHeaderWhenEmpty="True" Font-Size="12px">
                                                    <HeaderStyle CssClass="test no-sort" />
                                                    <Columns>


                                                        <asp:TemplateField HeaderText="Quitar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btn_quitar" runat="server" ImageUrl="img/delete.png" Width="25"
                                                                    CommandName="Eliminar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClientClick='<%# confirmDelete2( Eval("codproducto").ToString() ) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:BoundField DataField="codproducto" HeaderText="CodProducto">
                                                            <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="nomb" HeaderText="Producto">
                                                            <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="tipo_" HeaderText="Tipo">
                                                            <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="facturado" HeaderText="Facturado">
                                                            <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>


                                                        <asp:TemplateField HeaderText="Planificado" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>

                                                                <asp:TextBox ID="txt_planificado" Text='<%# Eval("despachado") %>' runat="server"></asp:TextBox>

                                                                <%--                                                                        <asp:ImageButton ID="ImageButton13442" runat="server" ImageUrl="images/save-128.png" Width="13"
                                                                            CommandName="Actualiza_fact_comercial" OnClientClick="relojito(true);" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                                            </ItemTemplate>
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




                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="form-group" style="display: block">
                                            </div>

                                            <asp:Button runat="server" class="btn btn-success" Text="RE-PLANIFICAR" OnClick="Unnamed_Click" OnClientClick="CARGANDO_ENVIAR();"></asp:Button>

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
                                    <div class="col-sm-12">

                                        <div id="tabla_productos"></div>

                                    </div>
                                </div>

                            </div>

                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" onclick="esconde(); chosen_update();" aria-hidden="true">Cerrar</button>
                        </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>





    <%-- <a id="btn-scrollup" class="btn btn-circle btn-lg" href="#"> </a>--%>
</asp:Content>
