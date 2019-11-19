<%@ Page ValidateRequest="false" Title="" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" Language="C#" MasterPageFile="~/Base.Master" Culture="es-AR" UICulture="es-AR" AutoEventWireup="True" CodeBehind="Calendario.aspx.cs" Inherits="SoprodiApp.Calendario" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ OutputCache Location="None" NoStore="true" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="900000" EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>

    <style>
        .fa-input {
            font-family: FontAwesome, 'Helvetica Neue', Helvetica, Arial, sans-serif;
        }

        input[type=checkbox] {
            /* Double-sized Checkboxes */
            -ms-transform: scale(2); /* IE */
            -moz-transform: scale(2); /* FF */
            -webkit-transform: scale(2); /* Safari and Chrome */
            -o-transform: scale(2); /* Opera */
            transform: scale(2);
            padding: 10px;
        }

        /* Might want to wrap a span around your checkbox text */
        .checkboxtext {
            /* Checkbox text */
            font-size: 110%;
            display: inline;
        }

        .gif {
            background: url('/img/carg.gif');
            background-size: cover;
            position: fixed;
            margin: auto;
            top: 0;
            left: 0;
            bottom: 0;
            right: 0;
            opacity: 0.5;
            z-index: 999;
            margin-top: 5%;
        }


        input[type=checkbox].css-checkbox {
            position: absolute;
            z-index: -1000;
            left: -1000px;
            overflow: hidden;
            clip: rect(0 0 0 0);
            height: 1px;
            width: 1px;
            margin: -1px;
            padding: 0;
            border: 0;
        }

            input[type=checkbox].css-checkbox + label.css-label {
                padding-left: 55px;
                height: 50px;
                display: inline-block;
                line-height: 50px;
                background-repeat: no-repeat;
                background-position: 0 0;
                font-size: 50px;
                vertical-align: middle;
                cursor: pointer;
            }

            input[type=checkbox].css-checkbox:checked + label.css-label {
                background-position: 0 -50px;
            }

        label.css-label {
            background-image: url('/img/checkbox.png');
            -webkit-touch-callout: none;
            -webkit-user-select: none;
            -khtml-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }

        th.sort-header::-moz-selection {
            background: transparent;
        }

        th.sort-header::selection {
            background: transparent;
        }

        registra th.sort-header {
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

        table.dataTable tbody th, table.dataTable tbody td {
            padding: 0px 5px;
        }

        .estado20 {
            border-left-color: #4e53ff !important;
            background-color: greenyellow;
        }
    </style>



    <script>
        //  ---------------------------------------- GLOBALES  --------------------------------------------- //
        //  ********************************************************************************************************** 
        var calendar;
        var cheques = new Array();
        var e_id;
        var e_rut_cliente;
        var e_factura;
        var e_tipo_doc;
        var sum_cheques_2 = 0;
        var cont_cheques = 0;
        var nume_cheques_2 = "";
        var monto_cheques = "";

        var g_init_datatable;

        var scrollLeft;
        var scrollTop;
        //  ---------------------------------------- LOAD JAVASCRIPT --------------------------------------------- //
        //  **********************************************************************************************************
        $(document).ready(function () {

            //LlenarCalendario();
            //document.querySelector('form').onkeypress = checkEnter;



            $(document).keypress(function (event) {
                var keycode = (event.keyCode ? event.keyCode : event.which);
                if (keycode == '13') {

                    checkEnter(event);
                    return false;
                }
            });


            //superfiltro();
            superfiltro2();
            //creagrilla();


            var elem3 = document.getElementById("ContentPlaceHolder_Contenido_calendar");
            elem3.style.display = "none";
            //.;

            $('#btn_tabla').hide();


            //$('#iniciar_cerrado').click();
        });


        $("input").change(function () {
            var value = $(this).val();

            alert(value.length);


        });

        $(document).keypress(function (event) {
            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                checkEnter(event);
                return false;
            }
        });

        function checkEnter(e) {
            try {
                if (e.keyCode == 13) {
                    document.getElementById("ContentPlaceHolder_Contenido_btn_filtra_grilla").click();
                    return false;
                }
            } catch{ }
            try {
                if (e.keyCode == 13) {
                    document.getElementById("btn_filtro_nuevo3").click();
                    return false;
                }

            } catch{ }

        }


        function cerrar_modal5() {
            $("#modal-77").modal("hide");
            $('#div_trael_scroll').click();
        }


        function cerrar_modal5_2() {

            scrollLeft = $(window).scrollLeft();
            scrollTop = $(window).scrollTop();
            $("#modal-5").modal("hide");




            setTimeout(function () { $(window).scrollLeft(scrollLeft); $(window).scrollTop(scrollTop); }, 500);

        }


        function volver_scroll() {
            scrollLeft = $(window).scrollLeft();
            scrollTop = $(window).scrollTop();
            setTimeout(function () { $(window).scrollLeft(scrollLeft); $(window).scrollTop(scrollTop); }, 500);
        }

        function devul_fal() {
            return false;
        }

        function modal_unidad() {

            document.getElementById("div_unidad_").click();
            //chosen_update();
        }
        function creagrilla() {


            (function ($) {
                // You pass-in jQuery and then alias it with the $-sign
                // So your internal code doesn't change

                try {

                    //alert('ac');
                    g_init_datatable = $("#G_INIT").DataTable({

                        "dom": 'T<"clear">lfrtip',
                        "tableTools": {
                            "sSwfPath": "js/sq/media/swf/copy_csv_xls_pdf.swf",
                            "aButtons": [
                                "copy",
                                "print",
                                {
                                    "sExtends": "collection",
                                    "sButtonText": "Save",
                                    "aButtons": ["csv", {
                                        "sExtends": "xls",
                                        "sTitle": "Listado"
                                    }, {
                                            "sExtends": "pdf",
                                            "sPdfOrientation": "landscape",
                                            "sTitle": "Listado"
                                        }]
                                }
                            ]
                        }
                        ,
                        "stateSave": true,
                        "lengthChange": true,
                        "searching": false,
                        "destroy": true,
                        "pageLength": -1,
                        "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                        "paging": true,

                        columnDefs: [
                            { type: 'date-uk', targets: [5] }
                        ],
                        columnDefs: [
                            { type: 'date-uk', targets: [6] }
                        ],
                        columnDefs: [
                            { type: 'date-uk', targets: [7] }
                        ],
                        "language": {
                            "decimal": ",",
                            "thousands": "."
                        }
                    });

                    $("#G_DETALLE_ESTIMADOS").DataTable({
                        "lengthChange": false,
                        "searching": false,
                        "destroy": true,
                        "stateSave": true,
                        "pageLength": -1,
                        "paging": false,
                        columnDefs: [
                            { type: 'date-uk', targets: 5 }
                        ],
                        "language": {
                            "decimal": ",",
                            "thousands": "."
                        }
                    });
                    $("#G_MOV_SOL").DataTable({
                        "lengthChange": false,
                        "searching": false,
                        "destroy": true,
                        "stateSave": true,
                        "pageLength": -1,
                        "paging": false,
                        "order": [[1, "asc"]],
                        columnDefs: [
                            { type: 'date-uk', targets: [10] }
                        ],
                        columnDefs: [
                            { type: 'date-uk', targets: [11] }
                        ],
                        "language": {
                            "decimal": ",",
                            "thousands": "."
                        }
                    });


                    $("#G_CHEQUES").DataTable({
                        "lengthChange": false,
                        "searching": false,
                        "destroy": true,
                        "stateSave": true,
                        "pageLength": -1,
                        "paging": false,
                        "order": [[0, "asc"]],
                        "language": {
                            "decimal": ",",
                            "thousands": "."
                        }
                    });



                    super_ff();
                    super_ff3();
                } catch (e) {

                    //alert('catch' + e);

                }


            })(jQuery);
        }

        function creagrilla_g_init() {

            $("#G_INIT").DataTable({
                "lengthChange": false,
                "searching": false,
                "destroy": true,
                "pageLength": -1,
                "paging": false,
                "language": {
                    "decimal": ",",
                    "thousands": "."
                }
            });

        }


        function checkEnter(e) {
            if (e.keyCode == 13) {

                return false;
            }
        }

        function carga_tablax() {
            var elem3 = document.getElementById("carga_tablax");
            elem3.style.display = "block";

            var gif = document.getElementById("GIF_COMPLETO");
            gif.style.display = "block";

        }
        function carga_tablax_2() {
            //try {

            //    creagrilla();

            //    g_init_datatable.destroy();

            //} catch (e) {
            //    alert('catch');
            //    console.log(e);
            //}


            var gif = document.getElementById("GIF_COMPLETO");
            gif.style.display = "block";

            document.getElementById("BTN_NETEO").style.display = "none";


        }

        function combos_refresh_directo() {


            try {
                $("#<%=cb_cliente_3.ClientID%>").chosen();
                $("#<%=cb_cliente_3.ClientID%>").trigger("chosen:updated");

            } catch (e) {

                console.log(e);
            }

        }


        function combos_refresh() {


            try {
                $("#<%=CB_TIPO_DOC_GRILLA.ClientID%>").chosen();
                //$("#<%=CB_TIPO_DOC_GRILLA.ClientID%>").trigger("chosen:updated");

                $("#<%=CB_CLIENTE_GRILLA.ClientID%>").chosen();
                $("#<%=CB_CLIENTE_GRILLA.ClientID%>").trigger("chosen:updated");
                $("#<%=CB_VENDEDOR_GRILLA.ClientID%>").chosen();
                $("#<%=CB_VENDEDOR_GRILLA.ClientID%>").trigger("chosen:updated");

                $("#<%=CB_CLIENTE_CHEQ.ClientID%>").chosen();
                $("#<%=CB_CLIENTE_CHEQ.ClientID%>").trigger("chosen:updated");
                $("#<%=CB_VENDEDOR_CHEQ.ClientID%>").chosen();
                $("#<%=CB_VENDEDOR_CHEQ.ClientID%>").trigger("chosen:updated");

                $("#<%=cb_banco_soprodi.ClientID%>").chosen();
                $("#<%=cb_banco_soprodi.ClientID%>").trigger("chosen:updated");


            } catch (e) {

                console.log(e);
            }

        }
        function Tabla() {
            $('#btn_mov_sol2').show();
            $("#check_fechas").show();
            carga_combo();
            $("#<%=Ocultar.ClientID%>").hide();
            $("#<%=grilla.ClientID%>").show();
            $("#<%=calendar.ClientID%>").hide();
            $("#<%=MOV_SOL.ClientID%>").hide();
            $("#<%=panel_cheques.ClientID%>").hide();

            $('#ContentPlaceHolder_Contenido_btn_mov_sol').show();
            $('#btn_tabla').hide();
            $('#btn_calendario').show();
            $('#btn_cheques').show();
            $("#<%=CB_TIPO_DOC_GRILLA.ClientID%>").chosen();
            //$("#<%=CB_TIPO_DOC_GRILLA.ClientID%>").trigger("chosen:updated");

            $("#<%=CB_CLIENTE_GRILLA.ClientID%>").chosen();
            $("#<%=CB_CLIENTE_GRILLA.ClientID%>").trigger("chosen:updated");
            $("#<%=CB_VENDEDOR_GRILLA.ClientID%>").chosen();
            $("#<%=CB_VENDEDOR_GRILLA.ClientID%>").trigger("chosen:updated");


            recargar_chosen_tabla();

            if ($('#sw_mu').val() == "2") {

                $('#sw_mu').val("1");
            }
            else {
                $('#sw_mu').val("0");
            }

            bloquear();
            $('#PANEL_ACCION_RESPUESTA2').show();
            $('#PANEL_DATOS_CLIENTE2').hide();
            try {
                document.getElementById("CB_ACCIONES2").selectedIndex = 0;
                document.getElementById("CB_TIPO_PAGO2").selectedIndex = 0;
            }

            catch (e) {

            }
            $('#tabla_html2').html(this.tabla_html);
            $('#ver_cliente2').html(this.ver_cliente);

            //document.getElementById("PANEL_VER_ACCIONES2").style.display = "none";
            $('#PANEL_VER_ACCIONES2').hide();

            //document.getElementById("PANEL_ENVIAR_CORREO2").style.display = "none";
            $('#PANEL_ENVIAR_CORREO2').hide();
            $('#PANEL_ESTIMADO_2').hide();

            try {
                cheques = [];
                sum_cheques_2 = 0;
                cont_cheques = 0;
                nume_cheques_2 = "";
                monto_cheques = "";
                $('#tabla_cheques2').html("");
            } catch
            (e) {

            }

            $('#titulo_panel_finanza').html('<i class="fa fa-list"></i>Cobranza -- Tabla');

            //creagrilla();
        }





        function sss() {
            $("#<%=oculta_cuando_mov.ClientID%>").show();


        }

        function lod() {

            $('#btn_mov_sol2').hide();
            document.getElementById("gif_del_info").style.display = "block";
        }



        function Calendario() {


            $('#btn_mov_sol2').show();
            $("#<%=Ocultar.ClientID%>").show();
            $("#check_fechas").hide();
            $('#btn_cheques').hide();

            $("#<%=grilla.ClientID%>").hide();
            $("#<%=calendar.ClientID%>").show();
            $("#<%=MOV_SOL.ClientID%>").hide();
            $("#<%=panel_cheques.ClientID%>").hide();

            $('#btn_tabla').show();
            $('#ContentPlaceHolder_Contenido_btn_mov_sol').show();
            $('#btn_calendario').hide();
            $('#btn_cheques').show();
            //CARGANDO();

            //$('#CB_VISTA_CALENDARIO').show();
            //$('#CB_TIPO_DOC').show();

            $('#CB_VISTA_CALENDARIO').chosen();
            $('#CB_VISTA_CALENDARIO').trigger("chosen:updated");

            $('#CB_TIPO_DOC').chosen();
            $('#CB_TIPO_DOC').trigger("chosen:updated");


            $('#CB_CLIENTE').chosen();
            $('#CB_CLIENTE').trigger("chosen:updated");


            try { $("#<%=calendar.ClientID%>").fullCalendar('destroy'); } catch (e) { }


            $('#titulo_panel_finanza').html('<i class="fa fa-list"></i>Cobranza -- Calendario');

        }


        function Ingresar_Solomon() {
            $("#<%=MOV_SOL.ClientID%>").show();
            $('#btn_mov_sol2').hide();
            $("#<%=MOV_SOL.ClientID%>").show();
            $('#btn_cheques').show();

            $("#check_fechas").hide();
            $("#<%=Ocultar.ClientID%>").hide();
            $("#<%=grilla.ClientID%>").hide();
            $("#<%=calendar.ClientID%>").hide();
            $("#<%=btn_listos.ClientID%>").show();
            $('#btn_tabla').show();
            $('#btn_calendario').show();
            $('#btn_mov_sol').hide();

            $("#<%=panel_cheques.ClientID%>").hide();

            superfiltro2();
            $('#sw_mu').val("1");


            $('#titulo_panel_finanza').html('<i class="fa fa-list"></i>Cobranza -- infor');


        }

        function pagar_cheques() {

            $("#<%=MOV_SOL.ClientID%>").hide();
            $("#<%=Ocultar.ClientID%>").hide();
            $("#<%=grilla.ClientID%>").hide();
            $("#<%=calendar.ClientID%>").hide();

            $("#<%=panel_cheques.ClientID%>").show();

            $('#btn_tabla').show();
            $('#btn_calendario').show();
            $('#btn_mov_sol').show();
            $('#btn_mov_sol2').show();
            $('#btn_cheques').hide();

            $("#check_fechas").show();

            $("#<%=CB_CLIENTE_CHEQ.ClientID%>").chosen({ width: "300px" });
            //$("#<%=CB_CLIENTE_CHEQ.ClientID%>").trigger("chosen:updated");
            $("#<%=CB_VENDEDOR_CHEQ.ClientID%>").chosen({ width: "300px" });
            //$("#<%=CB_VENDEDOR_CHEQ.ClientID%>").trigger("chosen:updated");
            recargar_chosen_cheques();

            $('#titulo_panel_finanza').html('<i class="fa fa-list"></i>Cobranza -- Cheques');
        }


        function cierra_gif() {
            document.getElementById("gif_del_info").style.display = "none";
            $("#check_fechas").hide();
            $("#<%=Ocultar.ClientID%>").hide();
            $("#<%=grilla.ClientID%>").hide();
            $("#<%=calendar.ClientID%>").hide();
            $("#<%=btn_listos.ClientID%>").show();
            $('#btn_tabla').show();
            $('#btn_calendario').show();
            $('#btn_mov_sol').hide();
            $("#<%=MOV_SOL.ClientID%>").show();
            superfiltro2();
            $('#sw_mu').val("1");
        }


        function LoadAcciones(result) {


            $("#CB_ACCIONES").html("");

            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {
                $("#CB_ACCIONES").append($("<option></option>").attr("value", this.id_accion).text(this.nom_accion))
            });


            $("#CB_ACCIONES2").html("");

            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {
                $("#CB_ACCIONES2").append($("<option></option>").attr("value", this.id_accion).text(this.nom_accion))
            });

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

        //}
        function cambia_sw() {
            $('#sw_mu').val("1");
        }

        function cambia_sw2() {
            $('#sw_mu').val("2");
        }

        function ESTADO_DOC(id_factur, id, rut_cliente) {

            var parameters = new Object();
            parameters.id_factur = id_factur;
            parameters.id_2 = id;
            parameters.rutclient = rut_cliente;
            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/cambia_estado_doc",
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
                $('#tabla_html').html(resp);


            });
        }



        function comision(num_doc) {

            alert(num_doc);
            var urlPdf = "/ListadoProductosPlanificador.aspx?";
            //var path2 = "P=" + path;
            //var filename2 = "&N=" + filename;
            //var urlPdf_Final = urlPdf + path2 + filename2;
            var param = "c=" + num_doc;
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


        function cambia_tipo_pago3(id_factur) {

            var estado = document.getElementById("cb_cambio_pago" + id_factur).value;
            var parameters = new Object();
            parameters.factura = id_factur;
            parameters.estado = estado;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/cambia_tipo_pago_",
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


        function super_ff3() {

            $(".filtrar3 tr:has(td)").each(function () {
                var t = $(this).text().toLowerCase();
                $("<td class='indexColumn'></td>")
                    .hide().text(t).appendTo(this);
            });
            //Agregar el comportamiento al texto (se selecciona por el ID) 
            $("#btn_filtro_nuevo3").click(function () {
                var s = $('#t_filtro_memoria3').val().toLowerCase().split(" ");
                $(".filtrar3 tr:hidden").show();
                $.each(s, function () {
                    $(".filtrar3 tr:visible .indexColumn:not(:contains('"
                        + this + "'))").parent().hide();
                });
            });

            $("#t_filtro_memoria3").keyup(function (event) {
                if (event.keyCode == 13) {
                    var s = $('#t_filtro_memoria3').val().toLowerCase().split(" ");
                    $(".filtrar3 tr:hidden").show();
                    $.each(s, function () {
                        $(".filtrar3 tr:visible .indexColumn:not(:contains('"
                            + this + "'))").parent().hide();
                    });
                }
            });



        }

        function superfiltro2() {

            $(".filtrar2 tr:has(td)").each(function () {
                var t = $(this).text().toLowerCase();
                $("<td class='indexColumn'></td>")
                    .hide().text(t).appendTo(this);
            });
            //Agregar el comportamiento al texto (se selecciona por el ID) 
            $("#t_filtro_memoria2").keyup(function () {
                var s = $(this).val().toLowerCase().split(" ");
                $(".filtrar2 tr:hidden").show();
                $.each(s, function () {
                    $(".filtrar2 tr:visible .indexColumn:not(:contains('"
                        + this + "'))").parent().hide();
                });
            });

        }

        function carga_combo() {

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/carga_acciones",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadAcciones,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al cargar acciones");
                }
            });
        }

        function CARGANDO() {
            var elem3 = document.getElementById("carga_tablax");
            elem3.style.display = "block";
            $.ajax({
                type: "POST",
                url: "Calendario.aspx/ve_session",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (XMLHttpRequest.status == 401) {
                        alert("Fin de la session");
                        location.href = "Acceso.aspx";
                    }
                }
            });
            $("#<%=calendar.ClientID%>").fullCalendar('destroy');
            var f1 = document.getElementById('T_FECHA_FILTRO1').value;
            var f2 = document.getElementById('T_FECHA_FILTRO2').value;
            var vc = document.getElementById("CB_VISTA_CALENDARIO").value;
            var cl = document.getElementById("CB_CLIENTE").value;
            var td = '';
            var x = document.getElementById("CB_TIPO_DOC");
            for (var i = 0; i < x.options.length; i++) {
                if (x.options[i].selected) {
                    td = td + "'" + x.options[i].value + "'" + ' ,';
                }
            }
            if (td.indexOf("-1") > -1) {
                td = '';
                for (var i = 0; i < x.options.length; i++) {
                    if (x.options[i].value != "-1") {
                        td = td + "'" + x.options[i].value + "'" + ' ,';
                    }
                }
            }
            //if (vc == '5') {
            //    document.getElementById("Leyenda").style.display = "inline";
            //} else {
            //    document.getElementById("Leyenda").style.display = "none";
            //}

            calendar = $("#<%=calendar.ClientID%>").fullCalendar({
                // HEADER 
                header: {
                    left: 'title',
                    center: 'year,month,basicWeek,basicDay',
                    right: 'prev,next today'
                },
                // ESPAÑOL
                lang: 'es',
                // TAMAÑO
                aspectRatio: 2,
                // ES EDITABLE
                editable: false,
                // EVENTOS - JSON
                events: 'JSON.ASPX?f1=' + f1 + '&f2=' + f2 + '&vc=' + vc + '&td=' + td + '&cl=' + cl,

                eventClick: function (e, jsEvent, view) {
                    if (e.id != 0) {
                        if (vc == '11') {
                            CargarEvento(e);
                            $('#modal_1').click();
                        }
                        if (vc == '12') {
                            CargarEvento3(e);
                            $('#modal_1').click();
                        }
                        if (vc == '14') {
                            CargarEvento3(e);
                            $('#modal_1').click();
                        }
                        if (vc == '13') {
                            CargarEvento(e);
                            $('#modal_1').click();
                        }

                        //PAGO - F
                        if (vc == '55') {
                            CargarEvento(e);
                            $('#modal_1').click();
                        }

                        if (vc == '21') {
                            CargarEvento2(e);
                            $('#modal_1').click();
                        }
                        //agendamiento
                        if (vc == '22') {
                            //alert("agend");
                            CargarEvento2(e);
                            $('#modal_1').click();
                        }
                        if (vc == '23') {
                            CargarEvento2(e);
                            $('#modal_1').click();
                        }
                        if (vc == '24') {
                            CargarEvento2(e);
                            $('#modal_1').click();
                        }
                    }
                }
            });
            $("#<%=calendar.ClientID%>").fullCalendar('changeView', 'basicWeek');

            if ($('#sw_mu').val() == "1") {
                $('#CB_VISTA_CALENDARIO').show();
                $('#CB_TIPO_DOC').show();
                $('#CB_VISTA_CALENDARIO').chosen();
                $('#CB_VISTA_CALENDARIO').trigger("chosen:updated");

                $('#CB_TIPO_DOC').chosen();
                $('#CB_TIPO_DOC').trigger("chosen:updated");

            }
            else {

                $('#CB_VISTA_CALENDARIO').hide();
                $('#CB_TIPO_DOC').hide();
            }
            var elem3 = document.getElementById("carga_tablax");
            elem3.style.display = "none";
        }

        function modal_cheque(num_cheque) {
            document.getElementById("<%=l_num_cheque.ClientID %>").value = num_cheque;
            $('#h3_num_che').html(num_cheque);
            $('#MODAL_CHEQUE').click();

        }


        function grafico() {
            $.ajax({
                type: "POST",
                url: "Calendario.aspx/grafico",

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,

            }).done(function (resp) {
                resp = resp.d;

                $('#tabla_semanas').html(resp.split('*')[0]);


                $('#tabla_semanas2').html(resp.split('*')[1]);

                $('#moda_35').click();
                $(function () {
                    $('#container').highcharts({
                        data: {
                            table: 'datatable'
                        },
                        chart: {
                            type: 'column'
                        },
                        title: {
                            text: ''
                        },
                        yAxis: {
                            allowDecimals: false,
                            title: {
                                text: 'VALOR'
                            }
                        }
                        ,

                        column: {

                            dataLabels: {
                                enabled: true
                            }
                        }


                        //,
                        //tooltip: {
                        //    formatter: function () {
                        //        return '<b>' + this.series.name + '</b><br/>' +
                        //            this.point.y + ' ' + this.point.name.toLowerCase();
                        //    }
                        //}
                    });
                    $('#container2').highcharts({
                        data: {
                            table: 'datatable2'
                        },
                        colors: ['#aaeeee'],
                        chart: {
                            backgroundColor: null,
                            type: 'column'
                        },
                        title: {
                            text: ''
                        },
                        yAxis: {
                            allowDecimals: false,
                            title: {
                                text: 'VALOR'
                            }
                        }
                        ,

                        column: {

                            dataLabels: {
                                enabled: true
                            }
                        }


                        //,
                        //tooltip: {
                        //    formatter: function () {
                        //        return '<b>' + this.series.name + '</b><br/>' +
                        //            this.point.y + ' ' + this.point.name.toLowerCase();
                        //    }
                        //}
                    });
                });




            });

        }

        function GuardaAccion() {
            var obs = "";

            var id_cob = $('#T_ID_EVENTO').val();
            var id_acc = document.getElementById("CB_ACCIONES").selectedIndex;
            if (id_acc == "1") {

                obs = $('#OBS_LLAMAR').val();

            }
            else if (id_acc == "2") {
                obs = $('#OBS_AGENDA').val();

            }
            else if (id_acc == "3") {
                obs = $('#T_DESCRIPCION_PAGO').val();

            }
            else if (id_acc == "4") {
                obs = $('#OBS_AGENDAR_PAGO').val();

            }
            else if (id_acc == "5") {
                obs = "Destino: " + $('#t_destino').val() + " CC: " + $('#t_cc').val() + " Contenido: " + $('#t_contenido').val();

            }
            var parameters = new Object();
            parameters.id_cobranza = id_cob;
            parameters.id_accion = id_acc;
            //parameters.fecha_accion = id;
            //parameters.usuario = id;
            parameters.obs = obs;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/guardaraccion",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                succes: llamado(),
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al Agendar");
                }
            });
        }

        function accion_eliminar(fact) {
            var id_cob = fact;
            var id_acc = "7";
            var obs = "";
            var parameters = new Object();
            parameters.id_cobranza = id_cob;
            parameters.id_accion = id_acc;
            //parameters.fecha_accion = id;
            //parameters.usuario = id;
            parameters.obs = obs;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/guardaraccion2",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error ");
                }
            });

        }

        function accion_eliminar2_varias(fact) {
            //alert(fact);
            var aux = fact;
            var id_cob = aux;
            var id_acc = "7";
            var obs = "";
            var parameters = new Object();
            parameters.id_cobranza = id_cob;
            parameters.id_accion = id_acc;
            //parameters.fecha_accion = id;
            //parameters.usuario = id;
            parameters.obs = obs;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/guardaraccion2",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error ");
                }
            });

        }

        function GuardaAccion2() {
            var obs = "";

            var id_cob = $('#fact_sele').val();
            var id_acc = document.getElementById("CB_ACCIONES2").selectedIndex;
            if (id_acc == "1") {

                obs = $('#OBS_LLAMAR2').val();

            }
            else if (id_acc == "2") {
                obs = "*" + document.getElementById('T_FECHA_AGENDA2').value + "*" + $('#OBS_AGENDA2').val();

            }
            else if (id_acc == "3") {
                obs = $('#T_DESCRIPCION_PAGO2').val();

            }
            else if (id_acc == "4") {
                //obs = $('#OBS_AGENDAR_PAGO2').val();
                obs = "Estimado manual : Fec:" + document.getElementById('t_estim').value + " Valor: " + $('#t_monto_estim').val() + "  TPago:" + cb_tipo_pago_estim.value;

            }
            else if (id_acc == "5") {
                obs = "Destino: " + $('#t_destino2').val() + " CC: " + $('#t_cc2').val() + " Contenido: " + $('#t_contenido2').val();

            }
            var parameters = new Object();
            parameters.id_cobranza = id_cob;
            parameters.id_accion = id_acc;
            //parameters.fecha_accion = id;
            //parameters.usuario = id;
            parameters.obs = obs;

            parameters = JSON.stringify(parameters);
            //alert(id_cob +"----" + id_acc + "....." + obs);
            $.ajax({
                type: "POST",
                url: "Calendario.aspx/guardaraccion2",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                succes: llamado2(),
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error ");
                }
            });
        }

        function AGENDAR() {
            var obs = $('#OBS_AGENDA').val()
            var start = document.getElementById('T_FECHA_AGENDA').value
            var id = $('#T_ID_EVENTO').val()

            var parameters = new Object();
            parameters.id = id;
            parameters.obs = obs;
            parameters.start = start;
            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/GuardarEvento",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                succes: agendado(),
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al Agendar");
                }
            });
            GuardaAccion();
            $('#OBS_AGENDA').val("");
            document.getElementById('T_FECHA_AGENDA').value = "";
        }
        function AGENDAR2() {
            var obs = $('#OBS_AGENDA2').val();
            var start = document.getElementById('T_FECHA_AGENDA2').value;
            var id = $('#fact_sele').val();
            if (start.trim() != "") {
                var parameters = new Object();
                parameters.id = id;
                parameters.obs = obs;
                parameters.start = start;
                parameters = JSON.stringify(parameters);

                $.ajax({
                    type: "POST",
                    url: "Calendario.aspx/GuardarEvento2",
                    data: parameters,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    succes: agendado2(),
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Error al Agendar");
                    }
                });
                GuardaAccion();
                $('#OBS_AGENDA2').val("");
                document.getElementById('T_FECHA_AGENDA2').value = "";
            } else { alert("Debe agregar fecha"); }
        }
        function CARGA_FECHA() {

            var elem2 = document.getElementById("<%=b.ClientID%>");
            elem2.style.display = "none";
            var elem3 = document.getElementById("<%=carga_fecha.ClientID%>");
            elem3.style.display = "block";



        }


        function CARGANDO2() {

            //$('#moda_4').click();

            var ELEM = document.getElementById('carga_2');
            ELEM.style.display = "block";
        }

        function REGISTRAR_AGENDAR_PAGO() {


            var FECHA_AGENDAR_PAGO = $('#FECHA_AGENDAR_PAGO').val();
            var OBS_AGENDAR_PAGO = $('#OBS_AGENDAR_PAGO').val();
            var MONTO_AGENDAR_PAGO = $('#MONTO_AGENDAR_PAGO').val();
            var id = $('#T_ID_EVENTO').val();
            var moneda = document.getElementById("<%=cb_tipo_pago_2.ClientID%>").value;
            var tipo_doc = "temporal";

            var parameters = new Object();
            parameters.id = id + "***" + FECHA_AGENDAR_PAGO;
            parameters.monto = MONTO_AGENDAR_PAGO;
            parameters.moneda = moneda;
            parameters.tipo_doc = tipo_doc;
            parameters.descripcion = OBS_AGENDAR_PAGO;
            parameters.cerrar = "no";

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/Registrar_Pago_efectivo",
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
                if (resp == 'OK') {
                    GuardaAccion2();
                    $('#Cerrar_modal').click();
                    sleep(1000);
                    Pagado();
                    GuardaAccion2();

                } else {
                    alert(resp);
                }

            });
        }

        function REGISTRAR_AGENDAR_PAGO2() {


            var FECHA_AGENDAR_PAGO = $('#t_estim').val();
            var OBS_AGENDAR_PAGO = "";
            var MONTO_AGENDAR_PAGO = $('#t_monto_estim').val();
            var id = $('#fact_sele').val();
            var moneda = document.getElementById("cb_tipo_pago_estim").value;
            var tipo_doc = "temporal";

            var parameters = new Object();
            parameters.id = id + "***" + FECHA_AGENDAR_PAGO;
            parameters.monto = MONTO_AGENDAR_PAGO;
            parameters.moneda = moneda;
            parameters.tipo_doc = tipo_doc;
            parameters.descripcion = OBS_AGENDAR_PAGO;
            parameters.cerrar = "no";

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/Registrar_Pago_efectivo",
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
                if (resp == 'OK') {
                    GuardaAccion2();

                    Pagado2();


                } else {
                    alert(resp);
                }

            });
        }


        function Vencidos() {
            $('#moda_2').click();

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/Vencidos_",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (XMLHttpRequest.status == 401) {
                        alert("Fin de la session");
                        location.href = "Acceso.aspx";
                    } else {
                        alert("Error al cargar vencidos");
                    }
                }
            }).done(function (resp) {
                resp = resp.d;
                $('#vencidos').html(resp);
            });
        }

        function Cerrados_por_app() {
            $('#moda_3').click();

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/Cerrados_app",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (XMLHttpRequest.status == 401) {
                        alert("Fin de la session");
                        location.href = "Acceso.aspx";
                    } else {
                        alert("Error al cargar vencidos");
                    }
                }
            }).done(function (resp) {
                resp = resp.d;
                $('#cerrados').html(resp);
            });
        }

        function ENVIAR_CORREO() {

            var destino = document.getElementById("t_destino").value;
            var cc = document.getElementById("t_cc").value;
            var contenido = document.getElementById("t_contenido").value;


            var parameters = new Object();
            parameters.destino = destino;
            parameters.cc = cc;
            parameters.contenido = contenido;
            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/Enviar_Correo",
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
                if (resp == 'OK') {
                    GuardaAccion();
                    alert("Correo enviado!");
                } else {
                    alert(resp);
                }

            });
        }

        function ENVIAR_CORREO2() {

            var destino = document.getElementById("t_destino2").value;
            var cc = document.getElementById("t_cc2").value;
            var contenido = document.getElementById("t_contenido2").value;


            var parameters = new Object();
            parameters.destino = destino;
            parameters.cc = cc;
            parameters.contenido = contenido;
            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/Enviar_Correo",
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
                if (resp == 'OK') {
                    GuardaAccion2();
                    alert("Correo enviado!");
                } else {
                    alert(resp);
                }

            });
        }



        function REGISTRAR_PAGO3() {

            //$('#fact_sele').val();
            var tipo_doc = document.getElementById("CB_TIPO_PAGO3").value;

            var rut_cliente = document.getElementById("cb_cliente_3").value;
            var monto_final = 0;
            var num_cheques = 0;
            if (tipo_doc == "efectivo") {


                monto_final = $('#T_MONTO_PAGO3').val();


            }
            else {

                var suma = 0;
                for (var i = 0; i < cheques.length; i++) {
                    suma = parseInt(suma) + parseInt(cheques[i].monto);

                }
                monto_final = suma;
                //num_cheques = cheques.substring(1);
                //alert("numeros_cheqe:" + num_cheques);
            }


            if (tipo_doc == "efectivo") {

                var id = $('#fact_sele').val();
                var monto = $('#T_MONTO_PAGO3').val();
                var moneda = document.getElementById("CB_TIPO_MONEDA3").value;
                var tipo_doc = document.getElementById("CB_TIPO_PAGO3").value;
                var descripcion = $('#T_DESCRIPCION_PAGO3').val();
                var fecha = document.getElementById('t_fecha_efec3').value;

                //if (id == "") { alert("error"); return false; }

                if (moneda == "") { alert("Seleccione tipo Moneda"); return false; }
                if (tipo_doc == "") { alert("Seleccione tipo de Documento"); return false; }

                var parameters = new Object();
                parameters.id = id;
                parameters.monto = monto;
                parameters.moneda = moneda;
                parameters.tipo_doc = tipo_doc;
                parameters.descripcion = descripcion;
                parameters.cerrar = "no";
                parameters.fecha = fecha;
                parameters.rut_cliente = rut_cliente;
                parameters = JSON.stringify(parameters);

                $.ajax({
                    type: "POST",
                    url: "Calendario.aspx/Registrar_Pago_efectivo3",
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
                    sleep(1000);
                    resp = resp.d;
                    Pagado23(resp);
                    if (resp != "Falló el pago") {
                        GuardaAccion2();
                        try {
                            document.getElementById("CB_DEPOSITOS_BANCO3").value = "-1";
                        } catch (e) { }
                        try {
                            document.getElementById('t_fecha_efec3').value = "";
                        } catch (e) { }
                        try {
                            document.getElementById('cb_bancos3').value = "-1";
                        } catch (e) { }
                    }
                        //$(<%=btn_filtra_grilla.ClientID%>).click();

                    //alert(resp);
                    //CARGANDO();

                    //$("#ContentPlaceHolder_btn_filtra_grilla").click();





                });

            }
            else if (tipo_doc == "cheque") {
                var cont = 0;

                var respuesta = "";
                if (cheques.length > 0) {
                    for (var i = 0; i < cheques.length; i++) {

                        var parameters = new Object();
                        parameters.id = cheques[i].id;
                        parameters.monto = cheques[i].monto;
                        parameters.moneda = cheques[i].moneda;
                        parameters.tipo_doc = cheques[i].tipo_doc;
                        parameters.banco = cheques[i].banco;
                        parameters.vencimiento = cheques[i].vencimiento;
                        parameters.num_cheque = cheques[i].num_cheque;
                        parameters.cerrar = "no";
                        parameters.total = sum_cheques_2;
                        parameters.num_cheques = nume_cheques_2;
                        parameters.cont_cheq = cont_cheques;
                        parameters.monto_cheques = monto_cheques;
                        parameters.rutcliente = rut_cliente;
                        parameters = JSON.stringify(parameters);

                        $.ajax({
                            type: "POST",
                            url: "Calendario.aspx/Registrar_Pago_cheque3",
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
                            respuesta = respuesta + resp;
                        });
                    }
                    if (respuesta == "Pago Realizado !") {

                        Pagado23(respuesta);
                        GuardaAccion2();
                            //$(<%=btn_filtra_grilla.ClientID%>).click();



                    } else {

                        alert("Error al pagar con cheques.  (COD:02)");

                    }
                    // FINAL FOR

                }
                else {
                    alert("Agregue cheques al pago");
                }
            }
            else {
                alert("error");
                return false;
            }


        }

        function CARGANDO_G() {
            var gif = document.getElementById("GIF_COMPLETO");
            gif.style.display = "block";
        }

        function REGISTRAR_PAGO2() {

            var gif = document.getElementById("GIF_COMPLETO");
            gif.style.display = "block";

            //$('#fact_sele').val();
            var tipo_doc = document.getElementById("CB_TIPO_PAGO2").value;

            if (tipo_doc == "tarjeta") {
                tipo_doc = "efectivo";
            }

            var chk_enviar_erp = document.getElementById("<%=chk_enviar_erp.ClientID %>").checked;

            var monto_final = 0;
            var num_cheques = 0;
            if (tipo_doc == "efectivo") {
                monto_final = $('#T_MONTO_PAGO2').val();
            }
            else {
                var suma = 0;
                for (var i = 0; i < cheques.length; i++) {
                    suma = parseInt(suma) + parseInt(cheques[i].monto);

                }
                monto_final = suma;
            }


            if (tipo_doc == "efectivo") {

                var id = $('#fact_sele').val();
                var monto = $('#T_MONTO_PAGO2').val();
                var moneda = document.getElementById("CB_TIPO_MONEDA2").value;
                var tipo_doc = document.getElementById("CB_TIPO_PAGO2").value;
                var descripcion = $('#T_BANCO_2_LABEL').text() + $('#T_DESCRIPCION_PAGO2').val();

                var fecha = document.getElementById('t_fech_efec').value;

                if (id == "") {
                    alert("error");
                    var gif = document.getElementById("GIF_COMPLETO");
                    gif.style.display = "none";
                    return false;
                }

                if (moneda == "") {
                    var gif = document.getElementById("GIF_COMPLETO");
                    gif.style.display = "none";
                    alert("Seleccione tipo Moneda");
                    return false;
                }
                if (tipo_doc == "") {
                    var gif = document.getElementById("GIF_COMPLETO");
                    gif.style.display = "none";
                    alert("Seleccione tipo de Documento");
                    return false;
                }
                if (descripcion.length > 8) {
                    var gif = document.getElementById("GIF_COMPLETO");
                    gif.style.display = "none";
                    alert("Descripción no debe superar 8 letras como máximo. (considerando letras banco)");
                    return false;
                }
                if (fecha == "") {
                    var gif = document.getElementById("GIF_COMPLETO");
                    gif.style.display = "none";
                    alert("Seleccione fecha");
                    return false;
                }
                if (descripcion.includes('--')) {
                    var gif = document.getElementById("GIF_COMPLETO");
                    gif.style.display = "none";
                    alert("Descripción no puede tener doble guion (--)");
                    return false;
                }


                var parameters = new Object();
                parameters.id = id;
                parameters.monto = monto;
                parameters.moneda = moneda;
                parameters.tipo_doc = tipo_doc;
                parameters.descripcion = descripcion;
                parameters.cerrar = "no";
                parameters.fecha = fecha;
                parameters.enviar_erp = chk_enviar_erp;

                parameters = JSON.stringify(parameters);

                $.ajax({
                    type: "POST",
                    url: "Calendario.aspx/Registrar_Pago_efectivo2",
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
                    sleep(1000);
                    resp = resp.d;
                    Pagado23(resp);
                    if (resp != "Falló el pago") {
                        GuardaAccion2();
                        try {
                            document.getElementById("CB_DEPOSITOS_BANCO2").value = "-1";
                        } catch (e) { }
                        try {
                            document.getElementById('t_fech_efec').value = "";
                        } catch (e) { }
                        try {
                            document.getElementById('CB_BANCOS2').value = "-1";
                        } catch (e) { }
                    }
                    $(<%=btn_filtra_grilla.ClientID%>).click();
                });
            }
            else if (tipo_doc == "cheque") {
                var cont = 0;
                var descripcion_CHEQUE = $('#T_DESCRIPCION_CHEQUES_3').val();
                var respuesta = "";
                if (cheques.length > 0) {
                    for (var i = 0; i < cheques.length; i++) {

                        var parameters = new Object();
                        parameters.id = cheques[i].id;
                        parameters.monto = cheques[i].monto;
                        parameters.moneda = cheques[i].moneda;
                        parameters.tipo_doc = cheques[i].tipo_doc;
                        parameters.banco = cheques[i].banco;
                        parameters.vencimiento = cheques[i].vencimiento;
                        parameters.num_cheque = cheques[i].num_cheque;
                        parameters.tcamb = cheques[i].tcamb;
                        parameters.tobs = cheques[i].tobs;
                        parameters.ttdolar = cheques[i].ttdolar;
                        parameters.cerrar = "no";
                        parameters.total = sum_cheques_2;
                        parameters.num_cheques = nume_cheques_2;
                        parameters.cont_cheq = cont_cheques;
                        parameters.monto_cheques = monto_cheques;
                        parameters.enviar_erp = chk_enviar_erp;
                        parameters.descrip_cheque = descripcion_CHEQUE;

                        parameters = JSON.stringify(parameters);

                        $.ajax({
                            type: "POST",
                            url: "Calendario.aspx/Registrar_Pago_cheque2",
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
                            respuesta = respuesta + resp;
                        });
                    }
                    if (respuesta == "Pago Realizado !") {

                        Pagado23(respuesta);
                        GuardaAccion2();
                        $(<%=btn_filtra_grilla.ClientID%>).click();



                    } else {

                        alert("Error al pagar con cheques.  (COD:02)");

                    }
                    // FINAL FOR

                }
                else {
                    alert("Agregue cheques al pago");
                }
            }
            else {
                alert("error");
                return false;
            }


        }




        function REGISTRAR_PAGO() {

            //$('#fact_sele').val();
            var tipo_doc = document.getElementById("CB_TIPO_PAGO").value;
            //var x = document.getElementById("ch_cerrar");
            var monto_doc = $('#T_MONTO_DOC').val();
            //alert(monto_doc);
            var monto_final = 0;
            if (tipo_doc == "efectivo") {
                monto_final = $('#T_MONTO_PAGO').val();
                //alert("monto_txt:" + monto_final);
            }
            else {
                var suma = 0;
                for (var i = 0; i < cheques.length; i++) {
                    suma = parseInt(suma) + parseInt(cheques[i].monto);
                }
                monto_final = suma;
                //alert("monto_cheqe:" + monto_final);
            }


            if (tipo_doc == "efectivo") {

                var id = $('#T_ID_EVENTO').val();
                var monto = $('#T_MONTO_PAGO').val();
                var moneda = document.getElementById("CB_TIPO_MONEDA").value;
                var tipo_doc = document.getElementById("CB_TIPO_PAGO").value;
                var descripcion = $('#T_DESCRIPCION_PAGO').val();

                if (id == "") { alert("error"); return false; }
                if (monto == "") { alert("Ingrese un monto"); return false; }
                if (moneda == "") { alert("Seleccione tipo Moneda"); return false; }
                if (tipo_doc == "") { alert("Seleccione tipo de Documento"); return false; }

                var parameters = new Object();
                parameters.id = id;
                parameters.monto = monto;
                parameters.moneda = moneda;
                parameters.tipo_doc = tipo_doc;
                parameters.descripcion = descripcion;
                parameters.cerrar = "no";

                parameters = JSON.stringify(parameters);

                $.ajax({
                    type: "POST",
                    url: "Calendario.aspx/Registrar_Pago_efectivo",
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
                    if (resp == 'OK') {
                        $('#Cerrar_modal').click();
                        sleep(1000);
                        Pagado();
                        guardaracccion2();
                    } else {
                        alert(resp);
                    }

                });

            }
            else if (tipo_doc == "cheque") {
                var cont = 0;
                if (cheques.length > 0) {
                    for (var i = 0; i < cheques.length; i++) {

                        var parameters = new Object();
                        parameters.id = cheques[i].id;
                        parameters.monto = cheques[i].monto;
                        parameters.moneda = cheques[i].moneda;
                        parameters.tipo_doc = cheques[i].tipo_doc;
                        parameters.descripcion = cheques[i].descripcion;
                        parameters.banco = cheques[i].banco;
                        parameters.vencimiento = cheques[i].vencimiento;
                        parameters.cuenta = cheques[i].cuenta;
                        parameters.num_cheque = cheques[i].num_cheque;

                        parameters.cerrar = "no";

                        parameters = JSON.stringify(parameters);

                        $.ajax({
                            type: "POST",
                            url: "Calendario.aspx/Registrar_Pago_cheque",
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
                            if (resp == "OK") {
                                cont = cont + 1;
                            } else {
                                alert("Error al registrar pago con cheques : " + resp);
                            }
                        });
                    }
                    if (cont == cheques.length) {



                        $('#Cerrar_modal').click();
                        sleep(1000);
                        Pagado();


                    }
                    // FINAL FOR

                }
                else {
                    alert("Agregue cheques al pago");
                }
            }
            else {
                alert("error");
                return false;
            }


        }

        function sleep(milliseconds) {
            var start = new Date().getTime();
            for (var i = 0; i < 1e7; i++) {
                if ((new Date().getTime() - start) > milliseconds) {
                    break;
                }
            }
        }


        function Cierra_accio() {
            $('#PANEL_VER_ACCIONES').hide();
            $('#B_VER_ACCIONES').show();
        }

        function CerrarFacturaCheque() {
            var suma = 0;
            var x = document.getElementById("ch_cerrar");

            for (var i = 0; i < cheques.length; i++) {
                suma = parseInt(suma) + parseInt(cheques[i].monto);

            }

            var parameters = new Object();
            parameters.id = cheques[0].id;
            parameters.monto = suma
            parameters.moneda = cheques[0].moneda;
            parameters.tipo_doc = "PA";
            parameters.descripcion = cheques[0].descripcion;
            parameters.banco = cheques[0].banco;
            parameters.vencimiento = cheques[0].vencimiento;
            parameters.cuenta = cheques[0].cuenta;
            parameters.num_cheque = cheques[0].num_cheque;
            parameters.cerrar = "si";

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/Registrar_Pago_cerrar",
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
                if (resp == "OK") {
                    Pagado();
                    $('#Cerrar_modal').click();
                } else {
                    alert("Error al registrar pago con cheques : " + resp);
                }
            });
        }


        function rb_click_accion(id2, id_cobranza2) {
            var parameters = new Object();
            parameters.id = id2;
            parameters.id_cobranza = id_cobranza2;
            parameters = JSON.stringify(parameters)

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/accion_prioridad",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (XMLHttpRequest.status == 401) {
                        alert("Fin de la session");
                        location.href = "Acceso.aspx";
                    } else {
                        alert("No se encuentran acciones");
                    }
                }
            }).done(function (resp) {
                resp = resp.d;


            });


        }


        function Agregar_Cheque() {
            var id = $('#T_ID_EVENTO').val();
            var monto = $('#T_Cuenta').val();
            var moneda = document.getElementById("CB_TIPO_MONEDA").value;
            var tipo_doc = document.getElementById("CB_TIPO_PAGO").value;
            var descripcion = document.getElementById("T_DESCRIPCION_PAGO").value;
            var x_banco = document.getElementById("<%=CB_BANCOS.ClientID%>");
            var banco = x_banco.value;
            var nombre_banco = x_banco.options[x_banco.selectedIndex].text;
            var vencimiento = $('#T_VENCIMIENTO_CHEQUE').val();
            var cuenta = $('#T_Cuenta').val();
            var num_cheque = $('#T_NUM_CHEQUE').val();

            if (id == "") { alert("error"); return false; }

            if (monto == "") { alert("Ingrese un monto"); return false; }

            if (moneda == "") { alert("Seleccione tipo Moneda"); return false; }

            if (tipo_doc == "") { alert("Seleccione tipo de Documento"); return false; }

            if (banco == "-1") { alert("Seleccione un Banco"); return false; }

            if (vencimiento == "") { alert("Ingrese una fecha de vencimiento"); return false; }

            if (num_cheque == "") { alert("Ingrese un numero de cheque"); return false; }

            var parameters = new Object();
            parameters.id = id;
            parameters.monto = monto;
            parameters.moneda = moneda;
            parameters.tipo_doc = tipo_doc;
            parameters.descripcion = descripcion;
            parameters.banco = banco;
            parameters.nombre_banco = nombre_banco;
            parameters.vencimiento = vencimiento;
            parameters.cuenta = cuenta;
            parameters.num_cheque = num_cheque;
            cheques.push(parameters);

            var tabla_html = "";
            tabla_html += "<table stlye='width:100%;' class='table fill-head table-bordered'>";
            tabla_html += "<thead class='test'>"
            tabla_html += "<tr>";
            tabla_html += "<th>Banco</th>";
            tabla_html += "<th>Nº Cheque</th>";
            tabla_html += "<th>Monto</th>";
            tabla_html += "<th>Moneda</th>";
            tabla_html += "<th>Descripcion</th>";
            tabla_html += "<th>Vencimiento</th>";
            tabla_html += "</tr>";
            tabla_html += "</thead>";
            for (var i = 0; i < cheques.length; i++) {
                tabla_html += "<tr>";
                tabla_html += "<td>" + cheques[i].nombre_banco + "</td>";
                tabla_html += "<td>" + cheques[i].num_cheque + "</td>";
                tabla_html += "<td>" + cheques[i].monto + "</td>";
                tabla_html += "<td>" + cheques[i].moneda + "</td>";
                tabla_html += "<td>" + cheques[i].descripcion + "</td>";
                tabla_html += "<td>" + cheques[i].vencimiento + "</td>";
                tabla_html += "</tr>";
            }
            tabla_html += "</table>";

            $('#tabla_cheques').html(tabla_html);
        }
        function Borrar_Cheques() {
            cheques = [];
            $('#tabla_cheques').html("");
        }



        function valida_num_cheq(banco_, num_cheque_) {
            var respuesta;

            var parameters = new Object();

            parameters.banco = banco_;
            parameters.num_cheque = num_cheque_;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/validar_cheque_",
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

                if (resp == "0") {
                    //ok cheque
                    respuesta = "OK";
                }
                else {
                    respuesta = "EXISTE";

                }

            });


            return respuesta;

        }
        function quitar_cheque(numero_cheque, monto_cheque) {
            sum_cheques_2 = sum_cheques_2 - monto_cheque;
            cheques = cheques.filter(x => x.num_cheque != [numero_cheque]);

            var tabla_html = "";
            tabla_html += "<table stlye='width:100%;' class='table fill-head table-bordered'>";
            tabla_html += "<thead class='test'>"
            tabla_html += "<tr>";
            tabla_html += "<th>Quitar</th>";
            tabla_html += "<th>Banco</th>";
            tabla_html += "<th>Nº Cheque</th>";
            tabla_html += "<th>Monto</th>";
            tabla_html += "<th>Moneda</th>";
            tabla_html += "<th>Vencimiento</th>";
            tabla_html += "<th>TCamb</th>";
            tabla_html += "<th>Obs</th>";
            tabla_html += "<th>TipoDolar</th>";
            tabla_html += "<th>*Total</th>";

            tabla_html += "</tr>";
            tabla_html += "</thead>";
            for (var i = 0; i < cheques.length; i++) {
                tabla_html += "<tr>";
                tabla_html += "<td><a style='background-color: rgb(255, 97, 97);' class=\"btn btn-circle show-tooltip fa fa-trash\"   onclick=\"quitar_cheque(\'" + cheques[i].num_cheque + "\'," + cheques[i].monto + ")\"></a></td>";
                tabla_html += "<td>" + cheques[i].nombre_banco + "</td>";
                tabla_html += "<td>" + cheques[i].num_cheque + "</td>";
                tabla_html += "<td>" + cheques[i].monto + "</td>";
                tabla_html += "<td>" + cheques[i].moneda + "</td>";
                tabla_html += "<td>" + cheques[i].vencimiento + "</td>";
                tabla_html += "<td>" + cheques[i].tcamb + "</td>";
                tabla_html += "<td>" + cheques[i].tobs + "</td>";
                tabla_html += "<td>" + cheques[i].ttdolar + "</td>";
                tabla_html += "<td>" + sum_cheques_2 + "</td>";

                tabla_html += "</tr>";
            }
            tabla_html += "</table>";

            $('#tabla_cheques2').html(tabla_html);
        }



        function Agregar_Cheque2() {

            //<input type="text" class="form-control" id="T_OBS_CHEQUES" placeholder="Obs..." />

            //<input type="text" class="form-control" id="T_CAMBIO_CHEQUES" placeholder="TipoCambio..." />
            //28/08/2019




            var id = $('#fact_sele').val();
            var moneda = document.getElementById("<%=cb_tipo_pago_cheque.ClientID%>").value;
            var tipo_doc = document.getElementById("CB_TIPO_PAGO2").value;
            var x_banco = document.getElementById("<%=CB_BANCOS2.ClientID%>");
            var banco = x_banco.value;
            var nombre_banco = x_banco.options[x_banco.selectedIndex].text;
            var id_banco = x_banco.options[x_banco.selectedIndex].value;
            var vencimiento = $('#T_VENCIMIENTO_CHEQUE2').val();
            var num_cheque = $('#T_NUM_CHEQUE2').val();
            var tcamb_ = $('#T_CAMBIO_CHEQUES').val();
            var tobs_ = $('#T_OBS_CHEQUES').val();
            var tcamb_OBS_VEN = $('#T_TIPO_DOLAR').val();

            var tcamb_OBS_VEN_CB = document.getElementById("<%=CB_TIPO_DOLAR.ClientID%>");
            var tcamb_OBS_VEN_2 = tcamb_OBS_VEN_CB.options[tcamb_OBS_VEN_CB.selectedIndex].text;

            if (id == "") { alert("error"); return false; }

            if (moneda == "") { alert("Seleccione tipo Moneda"); return false; }

            if (tipo_doc == "") { alert("Seleccione tipo de Documento"); return false; }

            if (banco == "-1") { alert("Seleccione un Banco"); return false; }

            if (vencimiento == "") { alert("Ingrese una fecha de vencimiento"); return false; }

            if (monto == "") { alert("Ingrese monto de cheque"); return false; }

            if (num_cheque == "") { alert("Ingrese un numero de cheque"); return false; }

            if (tcamb_ == "") { alert("Ingrese un tipo cambio de cheque"); return false; }

            if (tobs_ == "") { alert("Ingrese un observación de cheque"); return false; }

            if (tcamb_OBS_VEN_2 == "") { alert("Ingrese un tipo dolar de cheque"); return false; }


            var respt = valida_num_cheq(banco, num_cheque);
            if (respt == "OK") { } else { alert('Ya Existe número cheque con mismo banco'); return false; }

            var monto = $('#T_Cuenta2').val().replace(',', '.');
            sum_cheques_2 = sum_cheques_2 + parseFloat(monto);
            cont_cheques++;

            var ss = "(" + id_banco + ")" + $('#T_NUM_CHEQUE2').val() + ",";
            var mont = monto + "**";
            nume_cheques_2 = nume_cheques_2 + ss;
            monto_cheques = monto_cheques + mont;

            var parameters = new Object();
            parameters.id = id;
            parameters.monto = monto;
            parameters.moneda = moneda;
            parameters.tipo_doc = tipo_doc;
            parameters.banco = banco;
            parameters.nombre_banco = nombre_banco;
            parameters.vencimiento = vencimiento;
            parameters.num_cheque = num_cheque;
            parameters.tcamb = tcamb_;
            parameters.tobs = tobs_;
            parameters.ttdolar = tcamb_OBS_VEN_2;

            cheques.push(parameters);

            var tabla_html = "";
            tabla_html += "<table stlye='width:100%;' class='table fill-head table-bordered'>";
            tabla_html += "<thead class='test'>"
            tabla_html += "<tr>";
            tabla_html += "<th>Quitar</th>";
            tabla_html += "<th>Banco</th>";
            tabla_html += "<th>Nº Cheque</th>";
            tabla_html += "<th>Monto</th>";
            tabla_html += "<th>Moneda</th>";
            tabla_html += "<th>Vencimiento</th>";
            tabla_html += "<th>TCamb</th>";
            tabla_html += "<th>Obs</th>";
            tabla_html += "<th>TipoDolar</th>";
            tabla_html += "<th>*Total</th>";

            tabla_html += "</tr>";
            tabla_html += "</thead>";
            for (var i = 0; i < cheques.length; i++) {
                tabla_html += "<tr>";
                tabla_html += "<td><a style='background-color: rgb(255, 97, 97);' class=\"btn btn-circle show-tooltip fa fa-trash\"  onclick=\"quitar_cheque(\'" + cheques[i].num_cheque + "\'," + cheques[i].monto + ")\"></a></td>";
                tabla_html += "<td>" + cheques[i].nombre_banco + "</td>";
                tabla_html += "<td>" + cheques[i].num_cheque + "</td>";
                tabla_html += "<td>" + cheques[i].monto + "</td>";
                tabla_html += "<td>" + cheques[i].moneda + "</td>";
                tabla_html += "<td>" + cheques[i].vencimiento + "</td>";
                tabla_html += "<td>" + cheques[i].tcamb + "</td>";
                tabla_html += "<td>" + cheques[i].tobs + "</td>";
                tabla_html += "<td>" + cheques[i].ttdolar + "</td>";
                tabla_html += "<td>" + sum_cheques_2 + "</td>";

                tabla_html += "</tr>";
            }
            tabla_html += "</table>";

            $('#tabla_cheques2').html(tabla_html);

            //aca LIMPIAR CAMPOS CHEQUE

            try {
                document.getElementById("CB_BANCOS2").value = "-1";
            } catch (e) { }
            try {
                document.getElementById('T_NUM_CHEQUE2').value = "";
            } catch (e) { }
            try {
                document.getElementById('T_VENCIMIENTO_CHEQUE2').value = "";
            } catch (e) { }
            try {
                document.getElementById('T_Cuenta2').value = "";
            } catch (e) { }

            try {

            } catch (e) { }
            try {
                document.getElementById('T_OBS_CHEQUES').value = "";
            } catch (e) { }

            try {
                document.getElementById('T_TIPO_DOLAR').value = "";
            } catch (e) { }

        }


        function Agregar_Cheque3() {
            var id = $('#fact_sele').val();
            var monto = $('#T_Cuenta3').val().replace(',', '.');
            sum_cheques_2 = sum_cheques_2 + parseFloat(monto);
            cont_cheques++;
            var moneda = document.getElementById("<%=cb_moneda_3.ClientID%>").value;

            var tipo_doc = document.getElementById("CB_TIPO_PAGO3").value;
            var x_banco = document.getElementById("<%=cb_bancos3.ClientID%>");
            var banco = x_banco.value;
            var nombre_banco = x_banco.options[x_banco.selectedIndex].text;
            var id_banco = x_banco.options[x_banco.selectedIndex].value;
            var vencimiento = $('#T_VENCIMIENTO_CHEQUE3').val();
            var num_cheque = $('#T_NUM_CHEQUE3').val();

            var ss = "(" + id_banco + ")" + $('#T_NUM_CHEQUE3').val() + ",";
            var mont = monto + "**";
            nume_cheques_2 = nume_cheques_2 + ss;
            monto_cheques = monto_cheques + mont;

            //if (id == "") { alert("error"); return false; }

            if (moneda == "") { alert("Seleccione tipo Moneda"); return false; }

            if (tipo_doc == "") { alert("Seleccione tipo de Documento"); return false; }

            if (banco == "-1") { alert("Seleccione un Banco"); return false; }

            if (vencimiento == "") { alert("Ingrese una fecha de vencimiento"); return false; }

            if (monto == "") { alert("Ingrese monto de cheque"); return false; }

            if (num_cheque == "") { alert("Ingrese un numero de cheque"); return false; }

            var parameters = new Object();
            parameters.id = id;
            parameters.monto = monto;
            parameters.moneda = moneda;
            parameters.tipo_doc = tipo_doc;
            parameters.banco = banco;
            parameters.nombre_banco = nombre_banco;
            parameters.vencimiento = vencimiento;
            parameters.num_cheque = num_cheque;
            cheques.push(parameters);

            var tabla_html = "";
            tabla_html += "<table stlye='width:100%;' class='table fill-head table-bordered'>";
            tabla_html += "<thead class='test'>"
            tabla_html += "<tr>";
            tabla_html += "<th>Banco</th>";
            tabla_html += "<th>Nº Cheque</th>";
            tabla_html += "<th>Monto</th>";
            tabla_html += "<th>Moneda</th>";
            tabla_html += "<th>Vencimiento</th>";
            tabla_html += "<th>*Total</th>";

            tabla_html += "</tr>";
            tabla_html += "</thead>";
            for (var i = 0; i < cheques.length; i++) {
                tabla_html += "<tr>";
                tabla_html += "<td>" + cheques[i].nombre_banco + "</td>";
                tabla_html += "<td>" + cheques[i].num_cheque + "</td>";
                tabla_html += "<td>" + cheques[i].monto + "</td>";
                tabla_html += "<td>" + cheques[i].moneda + "</td>";
                tabla_html += "<td>" + cheques[i].vencimiento + "</td>";
                tabla_html += "<td>" + sum_cheques_2 + "</td>";

                tabla_html += "</tr>";
            }
            tabla_html += "</table>";

            $('#tabla_cheques3').html(tabla_html);

            //aca LIMPIAR CAMPOS CHEQUE

            try {
                document.getElementById("cb_bancos3").value = "-1";
            } catch (e) { alert("ac"); }
            try {
                document.getElementById('T_NUM_CHEQUE3').value = "";
            } catch (e) { }
            try {
                document.getElementById('T_VENCIMIENTO_CHEQUE3').value = "";
            } catch (e) { }
            try {
                document.getElementById('T_Cuenta3').value = "";
            } catch (e) { }

        }

        function Borrar_Cheques2() {
            cheques = [];
            sum_cheques_2 = 0;
            cont_cheques = 0;
            nume_cheques_2 = "";
            monto_cheques = "";
            $('#tabla_cheques2').html("");
        }

        function Borrar_Cheques3() {
            cheques = [];
            sum_cheques_2 = 0;
            cont_cheques = 0;
            nume_cheques_2 = "";
            monto_cheques = "";
            $('#tabla_cheques3').html("");
        }


        function ver_acciones() {
            var parameters = new Object();
            parameters.id = $('#T_ID_EVENTO').val();
            parameters = JSON.stringify(parameters)

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/ver_acciones",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (XMLHttpRequest.status == 401) {
                        alert("Fin de la session");
                        location.href = "Acceso.aspx";
                    } else {
                        alert("No se encuentran acciones");
                    }
                }
            }).done(function (resp) {
                resp = resp.d;
                $('#PANEL_VER_ACCIONES').show();
                $('#B_VER_ACCIONES').hide();
                $('#tabla_acciones').html(resp);

            });


        }


        function ver_acciones2(fact) {
            var parameters = new Object();
            parameters.id = fact;

            parameters = JSON.stringify(parameters)

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/cargarevento",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (XMLHttpRequest.status == 401) {
                        alert("Fin de la session");
                        location.href = "Acceso.aspx";
                    } else {
                        alert("No se encuentran acciones");
                    }
                }
            }).done(function (resp) {
                resp = resp.d;
                $('#modal_5').click();

                $('#acciones_f').html(resp);

            });


        }


        function Seguimiento() {
            var id = $('#T_ID_EVENTO').val();
            var parameters = new Object();
            parameters.id = id;
            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/Seguimiento",
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
                if (resp != 'ERROR') {
                    Seguido(resp);
                } else {
                    alert(resp);
                }
                CARGANDO();
            });
        }

        function agendado() {
            alert('Agendado');

        }
        function agendado2() {
            alert('Agendados');

        }


        function llamado() {
            var id_acc = document.getElementById("CB_ACCIONES").selectedIndex;
            if (id_acc == "1") {
                alert("Llamado registrado");
            }

        }
        function llamado2() {
            var id_acc = document.getElementById("CB_ACCIONES2").selectedIndex;
            if (id_acc == "1") {
                alert("Llamado registrado");
            }

        }
        function Pagado23(mensaje) {
            alert(mensaje);
        }

        function Pagado2() {


            //var id_acc = document.getElementById("CB_ACCIONES").selectedIndex;
            //if (id_acc == "4") {
            //    alert('Agendar Pago registrado');
            //}
            //else {
            alert('Pago registrado');
            //}

            //GuardaAccion();
            //limpiarPago();

            //CargarEvento_rest(e_id, e_rut_cliente, e_factura, e_tipo_doc);
            //$('#modal_1').click();

            //if ($('#sw_mu').val() == "1") {
            //    $('#CB_VISTA_CALENDARIO').show();
            //    $('#CB_TIPO_DOC').show();
            //    $('#CB_VISTA_CALENDARIO').chosen();
            //    $('#CB_VISTA_CALENDARIO').trigger("chosen:updated");

            //    $('#CB_TIPO_DOC').chosen();
            //    $('#CB_TIPO_DOC').trigger("chosen:updated");
            //    $('#sw_mu').val("0");
            //}
            //else {

            //    $('#CB_VISTA_CALENDARIO').hide();
            //    $('#CB_TIPO_DOC').hide();
            //}
            //CARGANDO();

        }

        function Seguido() {
            alert('Seguimiento Iniciado');
            document.getElementById("B_SEGUIMIENTO").style.display = "none";
            document.getElementById("B_VER_ACCIONES").style.display = "inline";
            bloquear();
            $('#PANEL_ACCION_RESPUESTA').show();
            try {
                document.getElementById("CB_ACCIONES").selectedIndex = 0;
                document.getElementById("CB_TIPO_PAGO").selectedIndex = 0;

            }

            catch (e) {

            }


        }

        function irfecha(textControl) {
            var fecha = textControl.value;

            if (fecha != "") {
                fecha = fecha.replace('/', '-');
                fecha = fecha.replace('/', '-');
                from = fecha.split("-");
                f = new Date(from[2], from[1] - 1, from[0]);
                calendar.fullCalendar('gotoDate', f);
            }

        }

        //  ---------------------------------------- LLENAR CALENDARIO --------------------------------------------- //
        //  **********************************************************************************************************
        function LlenarCalendario() {

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/ve_session",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (XMLHttpRequest.status == 401) {
                        alert("Fin de la session");
                        location.href = "Acceso.aspx";
                    }
                }
            });
            //var calendar;
            $("#<%=calendar.ClientID%>").fullCalendar('destroy');
            var f1 = document.getElementById('T_FECHA_FILTRO1').value;
            var f2 = document.getElementById('T_FECHA_FILTRO2').value;
            var vc = document.getElementById("CB_VISTA_CALENDARIO").value;
            var cl = document.getElementById("CB_CLIENTE").value;

            var td = '';
            var x = document.getElementById("CB_TIPO_DOC");
            for (var i = 0; i < x.options.length; i++) {
                if (x.options[i].selected) {
                    td = td + "'" + x.options[i].value + "'" + ' ,';
                }
            }

            vc = "11";

            if (td.indexOf("-1") > -1) {
                td = '';
                for (var i = 0; i < x.options.length; i++) {
                    if (x.options[i].value != "-1") {
                        td = td + "'" + x.options[i].value + "'" + ' ,';
                    }

                }
            }
            td = "'DM', 'IN','CM', 'ND' ";

            var value = ['Cheques', 'Facturas', 'NotasCrédito', 'NotasDébito'],
                el = document.getElementById("CB_TIPO_DOC");

            for (var j = 0; j < value.length; j++) {
                for (var i = 0; i < el.length; i++) {
                    if (el[i].innerHTML == value[j]) {
                        el[i].selected = true;
                        //alert("option should be selected");
                    }
                }
            }


            calendar = $("#<%=calendar.ClientID%>").fullCalendar({
                // HEADER 
                header: {
                    left: 'title',
                    center: 'year,month,basicWeek,basicDay',
                    right: 'prev,next today'
                },
                // ESPAÑOL
                lang: 'es',
                // TAMAÑO
                aspectRatio: 3,
                // ES EDITABLE
                editable: false,
                // EVENTOS - JSON
                events: 'JSON.ASPX?f1=' + f1 + '&f2=' + f2 + '&vc=' + vc + '&td=' + td + '&cl=' + cl,

                eventClick: function (e, jsEvent, view) {


                    if (e.id != 0) {
                        //FACTURA - CLIENTE
                        if (vc == '11') {
                            CargarEvento(e);
                            $('#modal_1').click();
                        }
                        //if (vc == '12') {
                        //    CargarEvento3(e);
                        //    $('#modal_1').click();
                        //}
                        //if (vc == '14') {
                        //    CargarEvento3(e);
                        //    $('#modal_1').click();
                        //}

                        //VENCIDOS
                        if (vc == '13') {
                            CargarEvento(e);
                            $('#modal_1').click();
                        }

                        //PAGO - F
                        if (vc == '55') {
                            CargarEvento(e);
                            $('#modal_1').click();
                        }


                        //GESTIONADOS
                        if (vc == '21') {
                            CargarEvento2(e);
                            $('#modal_1').click();
                        }
                        //AGENDADOS
                        if (vc == '22') {
                            CargarEvento2(e);
                            $('#modal_1').click();
                        }
                        //if (vc == '23') {
                        //    CargarEvento2(e);
                        //    $('#modal_1').click();
                        //}
                        //if (vc == '24') {
                        //    CargarEvento2(e);
                        //    $('#modal_1').click();
                        //}

                    }
                }


            });

            calendar.fullCalendar('changeView', 'basicWeek');
        }

        //  ---------------------------------------- LLENAR EVENTO --------------------------------------------- //
        //  **********************************************************************************************************
        function CargarEvento(e) {
            var parameters = new Object();
            parameters.id = e.id;
            e_id = e.id;
            parameters.rut = e.rut_cliente;
            e_rut_cliente = e.rut_cliente;
            parameters.factura = e.factura;
            e_factura = e.factura;
            parameters.tipo_doc = e.tipo_doc;
            e_tipo_doc = e.tipo_doc;
            parameters = JSON.stringify(parameters)

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/CargarEvento",
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

                LlenarEvento(resp);
                carga_combo();
                $('#PANEL_DATOS_CLIENTE').hide();
            });
        }

        function CargarEvento_rest(id, rut, fact, tipo_doc) {
            var parameters = new Object();
            parameters.id = id;
            parameters.rut = rut;
            parameters.factura = fact;
            parameters.tipo_doc = tipo_doc;
            parameters = JSON.stringify(parameters)

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/CargarEvento",
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
                LlenarEvento(resp);
                carga_combo();
                $('#PANEL_DATOS_CLIENTE').hide();
            });
        }

        function CargarEvento_Tabla(id, rut, fact, tipo_doc) {

            //alert(id + "---" + rut + "---" + fact + "---" + tipo_doc)
            var parameters = new Object();
            parameters.id = id;
            parameters.rut = rut;
            parameters.factura = fact;
            parameters.tipo_doc = tipo_doc;
            parameters = JSON.stringify(parameters)

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/CargarEvento55",
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

                $.each(result.d, function () {
                    //alert(this.tabla_html);
                    $('#acciones_f').html(this.tabla_html);
                    $('#modal_5').click();
                });
            });
        }



        function click_es_vi(factura) {

            var checkbox = $('#checkboxG1').is(":checked");

            //alert(id + "---" + rut + "---" + fact + "---" + tipo_doc)
            var parameters = new Object();
            parameters.es_vi = checkbox;
            parameters.factura = factura;

            parameters = JSON.stringify(parameters)

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/guarda_cliente_vi",
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
                if (resp == "OK") {



                }
                else {
                    alert('Error guardar VI : ' + resp);

                }


            });



        }


        function click_no_importacion(factura) {


            var checkbox = $('#checkboxG2').is(":checked");

            //alert(id + "---" + rut + "---" + fact + "---" + tipo_doc)
            var parameters = new Object();
            parameters.es_ewos = checkbox;
            parameters.factura = factura;

            parameters = JSON.stringify(parameters)

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/guarda_ewos",
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
                if (resp == "OK") {



                }
                else {
                    alert('Error ewos : ' + resp);

                }


            });


        }


        function comision(id, rut, fact, tipo_doc, saldo_peso_) {


            //alert(id + "---" + rut + "---" + fact + "---" + tipo_doc)
            var parameters = new Object();
            parameters.id = id;
            parameters.rut = rut;
            parameters.factura = fact;
            parameters.tipo_doc = tipo_doc;
            parameters.saldo_peso = saldo_peso_;
            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/comision",
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
                $('#acciones_f2').html(resp);
                $('#modal_77').click();
            });

        }



        function guardar_importacion(factura) {

            //alert(id + "---" + rut + "---" + fact + "---" + tipo_doc)
            var parameters = new Object();
            parameters.factura = factura;
            parameters.contrato = $('#contrato').val();
            parameters.toneladas = $('#toneladas').val();
            parameters.negocio = $('#negocio').val();
            parameters.neto_peso = document.getElementById("neto_peso_comision").innerHTML;

            parameters = JSON.stringify(parameters)

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/guardar_importacion",
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
                if (resp == "OK") {

                }
                else {
                    alert('Error immportacion : ' + resp);

                }


            });


        }
        function eliminar_pago_directo(id_seguimiento) {
            var parameters = new Object();
            parameters.id_seguimiento_ = id_seguimiento;
            parameters = JSON.stringify(parameters)

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/EliminarPagoDirecto",
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
                result = result.d;
                if (result == "OK") {
                    alert("PAGO DIRECTO ELIMINADO!");
                }
                else {

                    alert(result);
                }
            });
        }


        function eliminar_pago_en_pag(factura, fecha, obser) {
            var parameters = new Object();
            parameters.factura = factura;
            parameters.fecha = fecha;
            parameters.obs = obser

            parameters = JSON.stringify(parameters)

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/EliminarPago_2",
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
            });

        }

        function eliminar_pago_en_s(id2, id, rut, fact, tipo_doc, fecha) {
            var parameters = new Object();
            parameters.id = id2;
            parameters = JSON.stringify(parameters);
            //alert(id2, id, rut, fact, tipo_doc);
            $.ajax({
                type: "POST",
                url: "Calendario.aspx/EliminarPagoSegui",
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

                result = result.d;

                //alert(result);
                accion_eliminar2_varias(result);
                alert("PAGO ELIMINADO!");
                $('#btn_modal_5_cerrar').click();
                //alert("PAGO ELIMINADO!");
                CargarEvento_Tabla(id, rut, fact, tipo_doc);
                //CargarEvento_Tabla(id, rut, fact, tipo_doc);
            });
        }



        function CargarEvento2(e) {
            var parameters = new Object();
            parameters.id = e.id;
            parameters = JSON.stringify(parameters)

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/CargarEvento2",
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
                LlenarEvento2(resp);
                carga_combo();
                $('#PANEL_DATOS_CLIENTE').hide();
                $('#PANEL_ENVIAR_CORREO').hide();

                //$.each(result.d, function () {
                //    //alert(this.tabla_html);
                //    $('#acciones_f').html(this.tabla_html);
                //    $('#modal_5').click();
                //});
            });
        }

        function CargarEvento3(e) {
            var parameters = new Object();
            parameters.rut_cliente = e.rut_cliente;
            parameters.start = e.start;
            parameters.tipo_doc = e.tipo_doc;
            parameters = JSON.stringify(parameters)

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/CargarEvento3",
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
                LlenarEvento3(resp);
                carga_combo();
                $('#PANEL_DATOS_CLIENTE').hide();
            });
        }
        function tiene_letras(texto) {

            var letras = "abcdefghyjklmnñopqrstuvwxyzABCDEFGHYJKLMNÑOPQRSTUVWXYZ";
            texto = texto.toLowerCase();
            for (i = 0; i < texto.length; i++) {
                if (letras.indexOf(texto.charAt(i), 0) != -1) {
                    return 1;
                }
            }
            return 0;
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



        function LlenarEvento(result) {

            $.each(result.d, function () {

                $('#T_NOMBRE_EVENTO').val(this.title);
                $('#T_ID_EVENTO').val(this.id);
                document.getElementById('T_F_INICIO').value = (this.start);
                $('#T_FACTURA').html(this.factura);
                $('#T_RUT_CLIENTE').val(this.rut_cliente);
                $('#T_MONTO_DOC').val(this.monto_doc);
                $('#titulo_modal').html(this.title);

                if (this.estado == 0) {
                    document.getElementById("B_SEGUIMIENTO").style.display = "block";
                    document.getElementById("B_VER_ACCIONES").style.display = "none";
                    $('#PANEL_ACCION_RESPUESTA').hide();
                    bloquear();
                } else {
                    document.getElementById("B_SEGUIMIENTO").style.display = "none";
                    document.getElementById("B_VER_ACCIONES").style.display = "block";
                    bloquear();
                    $('#PANEL_ACCION_RESPUESTA').show();
                }
                if (tiene_letras(this.id) == "1") {
                    document.getElementById("B_SEGUIMIENTO").style.display = "none";
                    document.getElementById("B_VER_ACCIONES").style.display = "block";
                    $('#PANEL_ACCION_RESPUESTA').hide();
                    bloquear();
                }
                try {
                    document.getElementById("CB_ACCIONES").selectedIndex = 0;
                    document.getElementById("CB_TIPO_PAGO").selectedIndex = 0;
                }

                catch (e) {

                }

                $('#tabla_html').html(this.tabla_html);
                $('#ver_cliente').html(this.ver_cliente);
                $('#PANEL_VER_ACCIONES').hide();
                $('#PANEL_AGENDAR_PAGO').hide();

                $('#PANEL_ENVIAR_CORREO').hide();
            });
        }

        function LlenarEvento2(result) {

            $.each(result.d, function () {
                $('#T_NOMBRE_EVENTO').val(this.title);
                $('#T_ID_EVENTO').val(this.id);
                document.getElementById('T_F_INICIO').value = (this.start);
                $('#T_FACTURA').html(this.factura);
                $('#T_RUT_CLIENTE').val(this.rut_cliente);
                $('#titulo_modal').html(this.title);
                $('#tabla_html').html(this.tabla_html);

                document.getElementById("B_SEGUIMIENTO").style.display = "none";
                desbloquear();

                if (tiene_letras(this.factura) == "1") {
                    document.getElementById("B_SEGUIMIENTO").style.display = "none";
                    document.getElementById("B_VER_ACCIONES").style.display = "none";
                    $('#PANEL_ACCION_RESPUESTA').hide();
                    bloquear();

                }
                document.getElementById("B_SEGUIMIENTO").style.display = "none";
                document.getElementById("B_VER_ACCIONES").style.display = "inline";
                bloquear();
                $('#PANEL_ACCION_RESPUESTA').show();

                try {
                    document.getElementById("CB_ACCIONES").selectedIndex = 0;
                    document.getElementById("CB_TIPO_PAGO").selectedIndex = 0;

                }

                catch (e) {

                }


                $('#ver_cliente').html(this.ver_cliente);
                $('#PANEL_VER_ACCIONES').hide();
                $('#PANEL_AGENDAR_PAGO').hide();
            });
        }

        function LlenarEvento3(result) {
            $('modal_1').click();
            $.each(result.d, function () {
                $('#T_NOMBRE_EVENTO').val(this.title);
                document.getElementById('T_F_INICIO').value = (this.start);
                $('#T_RUT_CLIENTE').val(this.rut_cliente);
                $('#titulo_modal').html(this.title);
                $('#tabla_html').html(this.tabla_html);
                $('#T_FACTURA').html(this.factura);

                document.getElementById("B_SEGUIMIENTO").style.display = "none";
                bloquear();
                $('#ver_cliente').html(this.ver_cliente);
                $('#PANEL_VER_ACCIONES').hide();
                $('#PANEL_AGENDAR_PAGO').hide();

                if (this.estado == 0) {
                    document.getElementById("B_SEGUIMIENTO").style.display = "block";
                    document.getElementById("B_VER_ACCIONES").style.display = "none";
                    $('#PANEL_ACCION_RESPUESTA').hide();
                    bloquear();
                } else {
                    document.getElementById("B_SEGUIMIENTO").style.display = "none";
                    document.getElementById("B_VER_ACCIONES").style.display = "block";
                    bloquear();
                    $('#PANEL_ACCION_RESPUESTA').show();
                }


                if (tiene_letras(this.id) == "1") {
                    document.getElementById("B_SEGUIMIENTO").style.display = "none";
                    document.getElementById("B_VER_ACCIONES").style.display = "none";
                    $('#PANEL_ACCION_RESPUESTA').hide();
                    bloquear();

                }

            });

        }

        //  ---------------------------------------- GUARDAR EVENTO FROM MODAL  --------------------------------------------- //
        //  **********************************************************************************************************

        function bloquear() {
            $('#PANEL_SEGUIMIENTO').hide();
            try {
                $('#PANEL_SEGUIMIENTO2').hide();
            } catch (e) { }
        }

        function desbloquear() {
            $('#PANEL_SEGUIMIENTO').show();
            limpiarPago();
            $('#PANEL_MONTO').hide();
            $('#PANEL_BANCO').hide();

        }

        function CAMBIA_TARJETA() {
            document.getElementById("CB_DEPOSITOS_BANCO2").style.display = "none";

            $('#CB_DEPOSITOS_BANCO2_chosen').hide();

            var id = $('#T_ID_EVENTO').val();
            var tipo_DEPOSITO = 'TARJ';

            $('#T_BANCO_2_LABEL').text(tipo_DEPOSITO);

            $('#T_BANCO_2_LABEL_LETRAS').text('(' + tipo_DEPOSITO.length + ')');

            if (tipo_DEPOSITO != "-1") {
                document.getElementById("T_DESCRIPCION_PAGO").value = id;
            }
            else {
                document.getElementById("T_DESCRIPCION_PAGO").value = "";

            }
            try {

                var id2 = $('#fact_sele').val();
                var tipo_DEPOSITO2 = 'TARJ';

                if (tipo_DEPOSITO2 != "-1") {
                    document.getElementById("T_DESCRIPCION_PAGO2").value = id2;
                }
                else {
                    document.getElementById("T_DESCRIPCION_PAGO2").value = "";

                }
            } catch (e) { }

            try {
                var id2 = $('#fact_sele').val();
                var tipo_DEPOSITO2 = 'TARJ';

                if (tipo_DEPOSITO2 != "-1") {
                    document.getElementById("T_DESCRIPCION_PAGO3").value = id2;
                }
                else {
                    document.getElementById("T_DESCRIPCION_PAGO3").value = "";
                }
            } catch (e) { }
            $('#CB_DEPOSITOS_BANCO2_chosen').hide();
        }

        function CAMBIA_TIPO_DEPOSITO() {
            //CAMBIA BANCO --->
            var id = $('#T_ID_EVENTO').val();

            var tipo_DEPOSITO = document.getElementById("CB_DEPOSITOS_BANCO").value;

            if (tipo_DEPOSITO != "-1") {
                document.getElementById("T_DESCRIPCION_PAGO").value = id;
            }
            else {
                document.getElementById("T_DESCRIPCION_PAGO").value = "";

            }
            try {

                var id2 = $('#fact_sele').val();
                var tipo_DEPOSITO2 = document.getElementById("CB_DEPOSITOS_BANCO2").value;

                if (tipo_DEPOSITO2 != "-1") {


                    $('#T_BANCO_2_LABEL').text(tipo_DEPOSITO2);
                    $('#T_BANCO_2_LABEL_LETRAS').text('(' + tipo_DEPOSITO2.length + ')');

                    var largo_cod_banco = tipo_DEPOSITO2.length;

                    var maximo_permitido = 8 - largo_cod_banco;

                    $("#T_DESCRIPCION_PAGO2").attr('maxlength', maximo_permitido);

                    document.getElementById("T_DESCRIPCION_PAGO2").value = id2;

                }
                else {
                    document.getElementById("T_DESCRIPCION_PAGO2").value = "";
                    $('#T_BANCO_2_LABEL').text("");
                    $('#T_BANCO_2_LABEL_LETRAS').text("");
                }
            } catch (e) { }

            try {

                var id2 = $('#fact_sele').val();
                var tipo_DEPOSITO2 = document.getElementById("CB_DEPOSITOS_BANCO3").value;

                if (tipo_DEPOSITO2 != "-1") {
                    document.getElementById("T_DESCRIPCION_PAGO3").value = id2;
                }
                else {
                    document.getElementById("T_DESCRIPCION_PAGO3").value = "";
                    //$('#T_BANCO_2_LABEL').text("");
                    //$('#T_BANCO_2_LABEL_LETRAS').text("");
                }
            } catch (e) { }





            try {
                CAMBIA_PESO_DOLAR_CHEQ();
                CAMBIA_PESO_DOLAR();
            } catch (e) { }
        }



        function CAMBIA_TIPO_PAGO() {
            var tipo = document.getElementById("CB_TIPO_PAGO").value;


            if (tipo != "-1") {
                if (tipo == "efectivo") {

                    $('#PANEL_MONTO').show();
                    $('#PANEL_BANCO').hide();
                }
                if (tipo == "cheque") {

                    $('#PANEL_MONTO').hide();
                    $('#PANEL_BANCO').show();
                }
            } else {
                $('#PANEL_MONTO').hide();
                $('#PANEL_BANCO').hide();
            }
        }


        function CAMBIA_TIPO_PAGO2() {

            try {
                $('#CB_DEPOSITOS_BANCO2_chosen').show();
            } catch{ }
            var tipo = document.getElementById("CB_TIPO_PAGO2").value;
            document.getElementById("T_DESCRIPCION_PAGO2").value = "";
            $('#T_BANCO_2_LABEL').text("");
            $('#T_BANCO_2_LABEL_LETRAS').text("");

            $("#CB_DEPOSITOS_BANCO2").val('-1');
            $('#CB_BANCOS2').val('-1');
            if (tipo != "-1") {
                if (tipo == "efectivo") {
                    try {
                        Borrar_Cheques2();
                    } catch (e) { }
                    $('#PANEL_MONTO2').show();
                    $('#PANEL_BANCO2').hide();
                    $('#P_REGISTRA_PAGO').show();

                }
                if (tipo == "cheque") {
                    try {
                        Borrar_Cheques2();
                    } catch (e) { }
                    $('#PANEL_MONTO2').hide();
                    $('#PANEL_BANCO2').show();
                    $('#P_REGISTRA_PAGO').show();

                }

                if (tipo == "tarjeta") {
                    try {
                        Borrar_Cheques2();
                    } catch (e) { }
                    $('#PANEL_MONTO2').show();
                    $('#PANEL_BANCO2').hide();
                    $('#P_REGISTRA_PAGO').show();


                    CAMBIA_TARJETA();
                }

            } else {


                $('#PANEL_MONTO2').hide();
                $('#PANEL_BANCO2').hide();
                $('#P_REGISTRA_PAGO').hide();

            }
            try {
                Borrar_Cheques2();
            } catch (e) { }

            if (tipo != "tarjeta") {

                try {

                    $('#CB_DEPOSITOS_BANCO2').chosen({ "width": "100%" });
                    $('#CB_DEPOSITOS_BANCO2').trigger("chosen:updated");

                } catch (e) { }
                try {
                    $('#CB_BANCOS2').chosen({ "width": "100%" });
                    $('#CB_BANCOS2').trigger("chosen:updated");
                } catch (e) { }
            }
        }
        function CAMBIA_TIPO_PAGO_directo() {
            var tipo = document.getElementById("CB_TIPO_PAGO3").value;

            if (tipo != "-1") {
                if (tipo == "efectivo") {
                    try {
                        Borrar_Cheques2();
                    } catch (e) { }
                    $('#PANEL_MONTO3').show();
                    $('#PANEL_BANCO3').hide();
                    $("#<%=CB_DEPOSITOS_BANCO3.ClientID%>").chosen();
                    $("#<%=CB_DEPOSITOS_BANCO3.ClientID%>").chosen({ width: "300px" });
                }
                if (tipo == "cheque") {
                    try {
                        Borrar_Cheques2();
                    } catch (e) { }
                    $('#PANEL_MONTO3').hide();
                    $('#PANEL_BANCO3').show();
                }
                if (tipo == "tarjeta") {
                    try {
                        Borrar_Cheques2();
                    } catch (e) { }
                    $('#PANEL_MONTO3').show();
                    $('#PANEL_BANCO3').hide();

                    CAMBIA_TARJETA();
                }
            } else {
                $('#PANEL_MONTO3').hide();
                $('#PANEL_BANCO3').hide();
            }
            try {
                Borrar_Cheques3();
            } catch (e) { }
        }


        function CAMBIA_PESO_DOLAR_CHEQ() {
            var MONEDA = document.getElementById("ContentPlaceHolder_Contenido_cb_tipo_pago_cheque").value;
            if (MONEDA == "peso") {
                var total_peso = document.getElementById("monto_total_peso_f").value;
                $('#T_Cuenta2').val('');
            }
            else {
                var total_dolar = document.getElementById("monto_total_dolar_f").value;
                $('#T_Cuenta2').val('');
            }
        }

        function CAMBIA_PESO_DOLAR() {
            var MONEDA = document.getElementById("CB_TIPO_MONEDA2").value;
            if (MONEDA == "peso") {
                var total_peso = document.getElementById("monto_total_peso_f").value;
                $('#T_MONTO_PAGO2').val('');
            }
            else {
                var total_dolar = document.getElementById("monto_total_dolar_f").value;
                $('#T_MONTO_PAGO2').val('');
            }
        }


        function CAMBIA_ACCION() {
            var tipo = document.getElementById("CB_ACCIONES").value;

            if (tipo != "-1") {
                if (tipo == "1") {


                    var RUT = $('#T_RUT_CLIENTE').val();
                    var parameters = new Object();
                    parameters.RUT = RUT;
                    parameters = JSON.stringify(parameters);

                    $.ajax({
                        type: "POST",
                        url: "Calendario.aspx/DATOS_CLIENTE",
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

                        $.each(resp.d, function () {

                            if (this.tabla != "") {
                                $('#tabla_contacto').show();
                                $('#solomon_contacto').hide();

                                $.each(resp.d, function () {
                                    $('#contact').html(this.tabla);

                                });
                            }
                            else {

                                $('#tabla_contacto').hide();
                                $('#solomon_contacto').show();

                                $.each(resp.d, function () {
                                    $('#vendedor_').val(this.nom_vendedor);
                                    $('#codvendedor').val(this.vendedor);
                                    $('#direccion_').val(this.direccion);
                                    $('#ciudad_').val(this.ciudad + ", " + this.pais);
                                    $('#fono_').val(this.fono);
                                    $('#l_credito').val(this.LC + "(D: " + this.LD + " )");

                                });
                            }
                        });

                    });
                    $('#PANEL_DATOS_CLIENTE').show();

                    document.getElementById("OBS_LLAMAR").style.display = "block";
                    document.getElementById("aceptar").style.display = "block";

                    document.getElementById("PANEL_SEGUIMIENTO").style.display = "none";
                    $('#PANEL_AGENDAR_PAGO').hide();
                    $('#PANEL_ENVIAR_CORREO').hide();
                }
                else if (tipo == "2") {
                    $('#PANEL_DATOS_CLIENTE').hide();
                    $('#PANEL_ENVIAR_CORREO').hide();
                    $('#PANEL_AGENDA').show();
                    $('#PANEL_MONTO').hide();
                    $('#PANEL_BANCO').hide();
                    $('#PANEL_SEGUIMIENTO').show();
                    $('#aceptar').hide();
                    $('#OBS_LLAMAR').hide();
                    $('#PANEL_PAGO').hide();
                    $('#PANEL_AGENDAR_PAGO').hide();
                }
                else if (tipo == "3") {
                    $('#PANEL_DATOS_CLIENTE').hide();
                    $('#PANEL_ENVIAR_CORREO').hide();
                    $('#OBS_LLAMAR').hide();
                    $('#PANEL_AGENDA').hide();
                    $('#PANEL_MONTO').hide();
                    $('#PANEL_BANCO').hide();
                    $('#PANEL_SEGUIMIENTO').show();
                    $('#aceptar').hide();
                    $('#PANEL_PAGO').show();
                    $('#PANEL_AGENDAR_PAGO').hide();
                }
                else if (tipo == "4") {
                    $('#PANEL_AGENDAR_PAGO').show();
                    $('#PANEL_DATOS_CLIENTE').hide();
                    $('#PANEL_ENVIAR_CORREO').hide();
                    $('#OBS_LLAMAR').hide();
                    $('#PANEL_AGENDA').hide();
                    $('#PANEL_MONTO').hide();
                    $('#PANEL_BANCO').hide();
                    $('#PANEL_SEGUIMIENTO').show();
                    $('#aceptar').hide();
                    $('#PANEL_PAGO').hide();
                }
                else if (tipo == "6") {
                    $('#PANEL_AGENDAR_PAGO').hide();
                    $('#PANEL_DATOS_CLIENTE').hide();
                    $('#PANEL_ENVIAR_CORREO').show();
                    $('#OBS_LLAMAR').hide();
                    $('#PANEL_AGENDA').hide();
                    $('#PANEL_MONTO').hide();
                    $('#PANEL_BANCO').hide();
                    $('#PANEL_SEGUIMIENTO').show();
                    $('#aceptar').hide();
                    $('#PANEL_PAGO').hide();
                }

            } else {
                $('#PANEL_MONTO').hide();
                $('#PANEL_BANCO').hide();
            }
        }

        function loadClientes(result) {

            //quito los options que pudiera tener previamente el combo

            $("#<%=CB_CLIENTE_GRILLA.ClientID%>").html("");


            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {

                $("#<%=CB_CLIENTE_GRILLA.ClientID%>").append($("<option></option>").attr("value", this.rut).text(this.nombre))

            });


            $("#<%=CB_VENDEDOR_GRILLA.ClientID%>").chosen();
            $("#<%=CB_VENDEDOR_GRILLA.ClientID%>").trigger("chosen:updated");
            $("#<%=CB_CLIENTE_GRILLA.ClientID%>").chosen();
            $("#<%=CB_CLIENTE_GRILLA.ClientID%>").trigger("chosen:updated");


        }


        function APLICAR_NETEO() {

            $('#T_MONTO_PAGO2').val('0');
            $('#CB_TIPO_PAGO2').val('efectivo');

            try {
                var num = $('#fact_sele').val().substring(3, 7);
            } catch
            {
                $('#T_DESCRIPCION_PAGO2').val('NET' + num);
                $('#T_DESCRIPCION_PAGO2').val($('#T_DESCRIPCION_PAGO2').val().substring(1, 8));
            }
            $('#T_DESCRIPCION_PAGO2').val('NET' + num);

            let date = new Date();

            let day = date.getDate();
            let month = date.getMonth();
            let year = date.getFullYear();

            var fecha_hoy;
            if (month < 10) {
                fecha_hoy = day + '/0' + month + '/' + year;
            } else {
                fecha_hoy = day + '/' + month + '/' + year;
            }

            var fecha_neteo = $('#T_FECHA_NETEO').val();

            $('#t_fech_efec').val(fecha_neteo);
            document.getElementById("BTN_NETEO").style.display = "block";
        }


        function rescatarfecha() {

            var fecha_neteo = $('#T_FECHA_NETEO').val();

            $('#t_fech_efec').val(fecha_neteo);
        }


        function CAMBIA_ACCION2() {
            var tipo = document.getElementById("CB_ACCIONES2").value;

            if (tipo != "-1") {

                if (tipo == "1") {


                    var RUT = document.getElementById("T_RUT_CLIENTE2").value;


                    var parameters = new Object();
                    parameters.RUT = RUT;
                    parameters = JSON.stringify(parameters);

                    $.ajax({
                        type: "POST",
                        url: "Calendario.aspx/DATOS_CLIENTE",
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

                        $.each(resp.d, function () {

                            if (this.tabla != "") {
                                $('#tabla_contacto2').show();
                                $('#solomon_contacto2').hide();

                                $.each(resp.d, function () {
                                    $('#contact2').html(this.tabla);

                                });
                            }
                            else {

                                $('#tabla_contacto2').hide();
                                $('#solomon_contacto2').show();

                                $.each(resp.d, function () {
                                    $('#vendedor_2').val(this.nom_vendedor);
                                    $('#codvendedor2').val(this.vendedor);
                                    $('#direccion_2').val(this.direccion);
                                    $('#ciudad_2').val(this.ciudad + ", " + this.pais);
                                    $('#fono_2').val(this.fono);
                                    $('#l_credito2').val(this.LC + "(D: " + this.LD + " )");

                                });
                            }
                        });

                    });
                    $('#PANEL_DATOS_CLIENTE2').show();

                    document.getElementById("OBS_LLAMAR2").style.display = "block";
                    document.getElementById("aceptar2").style.display = "block";

                    document.getElementById("PANEL_SEGUIMIENTO2").style.display = "none";
                    $('#PANEL_AGENDAR_PAGO2').hide();
                    $('#PANEL_ENVIAR_CORREO2').hide();
                    $('#PANEL_ESTIMADO_2').hide();
                }
                else if (tipo == "2") {

                    $('#PANEL_DATOS_CLIENTE2').hide();
                    $('#PANEL_ENVIAR_CORREO2').hide();
                    $('#PANEL_AGENDA2').show();
                    $('#PANEL_MONTO2').hide();
                    $('#PANEL_BANCO2').hide();
                    $('#P_REGISTRA_PAGO').hide();

                    $('#PANEL_SEGUIMIENTO2').show();
                    $('#aceptar2').hide();
                    $('#OBS_LLAMAR2').hide();
                    $('#PANEL_PAGO2').hide();
                    $('#PANEL_ESTIMADO_2').hide();
                    $('#PANEL_AGENDAR_PAGO2').hide();
                }
                else if (tipo == "3") {

                    $('#PANEL_DATOS_CLIENTE2').hide();
                    $('#PANEL_ENVIAR_CORREO2').hide();
                    $('#OBS_LLAMAR2').hide();
                    $('#PANEL_AGENDA2').hide();
                    $('#PANEL_MONTO2').hide();
                    $('#PANEL_BANCO2').hide();
                    $('#P_REGISTRA_PAGO').hide();

                    $('#PANEL_SEGUIMIENTO2').show();
                    $('#aceptar2').hide();
                    $('#PANEL_PAGO2').show();
                    $('#PANEL_AGENDAR_PAGO2').hide();
                    $('#PANEL_ESTIMADO_2').hide();
                }
                else if (tipo == "4") {

                    $('#PANEL_AGENDAR_PAGO2').show();
                    $('#PANEL_DATOS_CLIENTE2').hide();
                    $('#PANEL_ENVIAR_CORREO2').hide();
                    $('#OBS_LLAMAR2').hide();
                    $('#PANEL_AGENDA2').hide();
                    $('#PANEL_MONTO2').hide();
                    $('#PANEL_BANCO2').hide();
                    $('#P_REGISTRA_PAGO').hide();
                    $('#PANEL_SEGUIMIENTO2').show();
                    $('#aceptar2').hide();
                    $('#PANEL_PAGO2').hide();
                    $('#PANEL_ESTIMADO_2').show();
                }
                else if (tipo == "6") {
                    $('#PANEL_AGENDAR_PAGO2').hide();
                    $('#PANEL_DATOS_CLIENTE2').hide();
                    $('#PANEL_ENVIAR_CORREO2').show();
                    $('#OBS_LLAMAR2').hide();
                    $('#PANEL_AGENDA2').hide();
                    $('#PANEL_MONTO2').hide();
                    $('#PANEL_BANCO2').hide();
                    $('#P_REGISTRA_PAGO').hide();
                    $('#PANEL_SEGUIMIENTO2').show();
                    $('#aceptar2').hide();
                    $('#PANEL_PAGO2').hide();
                    $('#PANEL_ESTIMADO_2').hide();
                }

            } else {

                $('#PANEL_ESTIMADO_2').hide();
                $('#PANEL_MONTO2').hide();
                $('#PANEL_BANCO2').hide();
                $('#PANEL_AGENDAR_PAGO2').hide();
                $('#PANEL_DATOS_CLIENTE2').hide();
                $('#PANEL_ENVIAR_CORREO2').hide();
                $('#OBS_LLAMAR2').hide();
                $('#PANEL_AGENDA2').hide();
                $('#PANEL_MONTO2').hide();
                $('#PANEL_BANCO2').hide();
                $('#P_REGISTRA_PAGO').hide();
                $('#PANEL_SEGUIMIENTO2').hide();
                $('#aceptar2').hide();
                $('#PANEL_PAGO2').hide();


                document.getElementById("OBS_LLAMAR2").style.display = "none";
                document.getElementById("aceptar2").style.display = "none";
                $('#tabla_contacto2').hide();
                $('#PANEL_DATOS_CLIENTE2').hide();
                $('#solomon_contacto2').hide();

                //aca
            }
            $('#CB_TIPO_PAGO2').val("-1");
            $('#CB_TIPO_PAGO2').change();
        }
        function AbrirURL(a, b) {
            alert("llegue");
            window.open("FICHA_CLIENTE.aspx?R=' + a + '&i=' + b + '", "MsgWindow", "width=200, height=100");
        }

        function limpiarPago() {

            cheques = [];

            $("#CB_TIPO_PAGO").val("-1");
            $("#CB_TIPO_PAGO").change();

            $("#CB_BANCOS").val("-1");
            $("#CB_BANCOS").change();

            $('#T_DESCRIPCION_PAGO').val("");
            $('#T_MONTO_PAGO').val("");
            $('#T_Cuenta').val("");
            $('#T_NUM_CHEQUE').val("");
            $('#T_VENCIMIENTO_CHEQUE').val("");

            $('#tabla_cheques').html("");

        }

        function MARCACOMOPAGADO() {
            var id = $('#T_ID_EVENTO').val();
            var parameters = new Object();
            parameters.id = id;
            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/MARCACOMOPAGADO",
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
                if (resp == 'OK') {
                    alert("Documento Pagado");
                } else {
                    alert(resp);
                }
            });
        }


        // Convierte un String en un objeto JSON
        //*****************************************************
        function stringtojson(string) {
            return eval('(' + string + ')');
        }
        function cargando_en_grilla() {
            var elem3 = document.getElementById("cargando_en_filtrar");
            elem3.style.display = "block";

            var gif = document.getElementById("GIF_COMPLETO");
            gif.style.display = "block";

            $('#sw_mu').val("1");
            //$("#ContentPlaceHolder_cobranza2").hide();
            //$("#ContentPlaceHolder_montos_totales").hide();
            $("#fact_sele").val("");

            //var elem32 = document.getElementById("ContentPlaceHolder_Contenido_montos_totales");
            //elem32.style.display = "none;";

            //var elem322 = document.getElementById("ContentPlaceHolder_Contenido_cobranza2");
            //elem322.style.display = "none";
        }

        function cargar_cheques() {
            var elem3 = document.getElementById("cargando_en_filtrar_");
            elem3.style.display = "block";
        }

        function Reporte_() {

            $('#moda_4').click();

        }

        function pago_directo_() {


            $("#<%=cb_cliente_3.ClientID%>").chosen();
            $("#<%=cb_cliente_3.ClientID%>").trigger("chosen:updated");
            $("#<%=cb_cliente_3.ClientID%>").chosen({ width: "300px" });

            $('#modaL_DIRECTO33').click();
            $('#PANEL_MONTO3').hide();
            $('#PANEL_BANCO3').hide();
            $('#PANEL_PAGO3').show();
        }

        function reporte_estima_detalle_cargando() {
            var elem3 = document.getElementById("carga_2");
            elem3.style.display = "block";

        }

        function reporte_estima() {
            var elem3 = document.getElementById("carga_2");
            elem3.style.display = "block";

            var desde = $("#<%=txt_desde.ClientID%>").val();
            var hasta = $("#<%=txt_hasta.ClientID%>").val();

            var parameters = new Object();
            parameters.desde = desde;
            parameters.hasta = hasta;
            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "Calendario.aspx/Reporte_Estimados",
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

                $('#tabla_html2').html(resp);
                var elem3 = document.getElementById("carga_2");
                elem3.style.display = "none";

            });
        }


        function recargar_chosen_cheques() {
            $("#<%=cb_banco_soprodi.ClientID%>").chosen({ width: "300px" });
        }


        function recargar_chosen_tabla() {
            $("#<%=CB_VENDEDOR_GRILLA.ClientID%>").chosen({ width: "300px" });
            $("#<%=CB_CLIENTE_GRILLA.ClientID%>").chosen({ width: "300px" });
            $("#<%=CB_TIPO_DOC_GRILLA.ClientID%>").chosen({ width: "300px" });
        }



    </script>
    <style>
        .test {
            background-color: #A9D7FF !important;
            color: white !important;
        }
        /* The switch - the box around the slider */
        .switch {
            position: relative;
            display: inline-block;
            width: 60px;
            height: 34px;
        }

            /* Hide default HTML checkbox */
            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        /* The slider */
        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 26px;
                width: 26px;
                left: 4px;
                bottom: 4px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #2196F3;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 34px;
        }

            .slider.round:before {
                border-radius: 50%;
            }
    </style>

    <%--  <div class="page-title" style="margin-top: -27px">
        <div>
            <i class="fa fa-file-o fa-3x"></i><a class="h1" href="MENU_finanzas.aspx">Finanzas</a>
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
                <a href="MENU_finanzas.aspx">Finanzas</a>
                <span class="divider"><i class="fa fa-angle-right"></i></span>
            </li>
            <li class="active">Calendario</li>
        </ul>
    </div>
    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_cobrar" />
            <asp:PostBackTrigger ControlID="btn_excel_3" />
            <asp:PostBackTrigger ControlID="btn_excel2" />

        </Triggers>
        <ContentTemplate>


            <script type="text/javascript">



                Sys.Application.add_load(BindEvents);

                function BindEvents() {
                    $(document).keypress(function (event) {
                        var keycode = (event.keyCode ? event.keyCode : event.which);
                        if (keycode == '13') {
                            checkEnter(event);
                            return false;
                        }
                    });
                    $("#<%=CB_VENDEDOR_GRILLA.ClientID%>").change(function () {
                        document.getElementById("<%=L_CLIENTES.ClientID %>").value = "";
                        // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                        //este parametro mapeara con el definido en el web service
                        var arr = $(this).val();
                        document.getElementById("<%=L_VENDEDORES.ClientID %>").value = arr;

                        var parameters = new Object();


                        parameters.vendedores = document.getElementById("<%=L_VENDEDORES.ClientID %>").value;

                        if (document.getElementById("<%=rd_abi.ClientID %>").checked) {
                            //alert("true");
                            parameters.cerrado_o_abierto = "1";
                        }
                        else {
                            parameters.cerrado_o_abierto = "0";
                        }
                        parameters = JSON.stringify(parameters)
                        $.ajax({
                            type: "POST",
                            url: "Calendario.aspx/CLIENTES_X_VENDEDOR",
                            data: parameters,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: loadClientes,
                            error: function (request, status, error) {
                                alert(request.responseText);
                            }
                        });


                    });


                    $("#<%=CB_CLIENTE_GRILLA.ClientID%>").change(function () {

                        // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                        //este parametro mapeara con el definido en el web service
                        var arr = $(this).val();
                        document.getElementById("<%=L_CLIENTES.ClientID %>").value = arr;





                    });
                }
            </script>

            <div id="main-content">
                <i id="GIF_COMPLETO" class="gif" style="display: none; font-size: 3em;"></i>
                <input type="text" id="sw_gif" value="1" style="visibility: hidden; position: absolute;">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box">
                            <div class="box-title">
                                <h3 id="titulo_panel_finanza"><i class="fa fa-list"></i>Cobranza -- Tabla</h3>
                                <div class="box-tool">
                                </div>
                            </div>
                            <div class="box-content">
                                <!-- CALENDARIO DIV -->
                                <div class="row">
                                    <div class="col-sm-12">

                                        <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_gif" style="display: none; font-size: 3em;"></i>


                                        <div class="row">
                                            <div class="pull-right" style="position: relative; left: -5%;">
                                            </div>
                                            <div class="pull-right" id="btn_mov_sol2" style="position: relative; left: -6%;">
                                                <input id="bn_mov" type="button" class="btn btn-yellow" value="Infor" onclick="Ingresar_Solomon()" style="display: block;" />
                                            </div>
                                            <div class="pull-right" id="btn_tabla" style="position: relative; left: -7%;">
                                                <input id="Button4" type="button" class="btn btn-lime" value="Tabla" onclick="Tabla()" style="display: block;" />
                                            </div>
                                            <div class="pull-right" id="btn_calendario" style="display: block; position: relative; left: -8%;">
                                                <input id="Button5" type="button" class="btn btn-primary" value="Calendario" onclick="Calendario()" style="display: block;" />
                                            </div>
                                            <div class="pull-right" id="btn_pago directo" style="display: block; position: relative; left: -9%;">
                                                <input id="Button35" type="button" class="btn btn-success" value="Pago Directo" onclick="pago_directo_()" style="display: block;" />
                                            </div>
                                            <div class="pull-right" id="btn_cheques" style="position: relative; left: -10%;">
                                                <input id="bn_cheques" type="button" class="btn btn-warning" value="Cheques" onclick="pagar_cheques()" style="display: block;" />
                                            </div>
                                            <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom gif" id="carga_tablax" style="display: none; font-size: 3em;"></i>
                                        </div>

                                    </div>
                                </div>
                                <asp:Panel runat="server" ID="Ocultar" Style="display: none; width: 100%;">

                                    <!-- ********************************************************************* -->
                                    <div class="form-group">
                                        <div style="visibility: hidden; position: absolute">
                                            <label class="col-sm-1 control-label">
                                                <b>Desde: </b>
                                            </label>
                                            <div class="col-sm-3 controls">
                                                <asp:TextBox runat="server" ID="T_FECHA_FILTRO1" ClientIDMode="Static" CssClass="form-control input-sm"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" runat="server" ID="CalendarExtender1" TargetControlID="T_FECHA_FILTRO1"></ajaxToolkit:CalendarExtender>
                                            </div>

                                            <label class="col-sm-1 control-label">
                                                <b>Hasta: </b>
                                            </label>
                                            <div class="col-sm-3 controls">
                                                <asp:TextBox runat="server" ID="T_FECHA_FILTRO2" ClientIDMode="Static" CssClass="form-control input-sm"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" runat="server" ID="CalendarExtender2" TargetControlID="T_FECHA_FILTRO2"></ajaxToolkit:CalendarExtender>
                                            </div>

                                            <label class="col-sm-1 control-label">
                                                <b>ir a:</b>
                                            </label>
                                            <div class="col-sm-3 controls">
                                                <asp:TextBox runat="server" ID="T_FECHA" ClientIDMode="Static" CssClass="form-control input-sm" onchange="javascript: irfecha( this );"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" runat="server" ID="cc" TargetControlID="T_FECHA"></ajaxToolkit:CalendarExtender>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-1 control-label">
                                            <b>Vista:</b>
                                        </label>
                                        <div class="col-sm-3 controls">
                                            <select data-placeholder="Tipo de Vista.." class="form-control chosen" tabindex="-1" id="CB_VISTA_CALENDARIO">
                                                <optgroup label="SOLOMON(Doc.Abiertos)">
                                                    <option value="11" selected>Factura - Cliente</option>
                                                    <%--             <option value="12">Monto - Cliente (facturas)</option>
                                                    <option value="14">Monto - Cliente (cheques)</option>--%>
                                                    <option value="13">Vencimientos</option>
                                                </optgroup>
                                                <optgroup label="EN SEGUIMIENTO">
                                                    <option value="55">Pagos - F</option>
                                                    <option value="21">Gestionados</option>
                                                    <option value="22">Agendamientos</option>
                                                    <%--    <option value="23">Vencimientos</option>
                                                    <option value="24">Pagos - Abonos</option>--%>
                                                </optgroup>
                                            </select>
                                        </div>
                                        <label class="col-sm-1 control-label">
                                            <b>Tipo:</b>
                                        </label>
                                        <div class="col-sm-3 controls">
                                            <select data-placeholder="Tipo Doc." class="form-control chosen" tabindex="-1" id="CB_TIPO_DOC" multiple="multiple">
                                                <option value="-1">Todos</option>
                                                <option value="DM">Cheques</option>
                                                <option value="ND">NotasDébito</option>
                                                <option value="IN">Facturas</option>
                                                <option value="PA">Pagos</option>
                                                <option value="CM">NotasCrédito</option>

                                            </select>
                                        </div>
                                        <label class="col-sm-1 control-label">
                                            <b>Cliente:</b>
                                        </label>
                                        <div class="col-sm-3 controls">
                                            <asp:DropDownList runat="server" ID="CB_CLIENTE" ClientIDMode="Static" CssClass="form-control chosen"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <br />

                                    <div class="form-group" style="float: right; margin-top: 2%">
                                        <div class="col-sm-1 col-sm-offset-1 controls">
                                            <input type="button" onclick="CARGANDO();" value="Filtrar" class="btn btn-primary" />
                                        </div>

                                        <div class="col-sm-6 col-sm-offset-4 controls">
                                            <div id="Leyenda" style="display: none">
                                                Leyenda : <span class="label" style="background-color: #3ADF00;">Vigente </span><span class="label" style="background-color: #BFFF00;">0 > 5 </span><span class="label" style="background-color: #FFFF00;">6 > 10</span> <span class="label" style="background-color: #FFBF00;">11 > 15</span> <span class="label" style="background-color: #FF4000;">16 > 20</span> <span class="label" style="background-color: #DF0101;">21 > 25</span> <span class="label" style="background-color: #FF0000;">26 + </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="cargando">
                                        <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_gifs" runat="server" style="display: none; font-size: 3em;"></i>
                                    </div>

                                    <div class="modal">
                                        <!-- Place at bottom of page -->
                                    </div>



                                </asp:Panel>


                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="input-group" id="check_fechas">
                                            <asp:RadioButton ID="rd_em" Text="&nbsp;&nbsp;Emisión" Style="padding-right: 20px;" Checked="True"
                                                GroupName="tipo_usuario" runat="server" />

                                            <asp:RadioButton ID="rd_ven" Style="padding-right: 20px;" Text="&nbsp;&nbsp;Vencimi."
                                                GroupName="tipo_usuario" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-sm-12">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div style="font-size: 10px;">
                                            <asp:Panel runat="server" ID="calendar" Style="width: 100%"></asp:Panel>
                                        </div>

                                        <asp:Panel runat="server" ID="MOV_SOL" Style="display: none; width: 100%;">


                                            <div class="form-group">

                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;"><b>Periodo Desde</b></label>

                                                <div class="col-sm-3 col-lg-3 controls">
                                                    <div class="input-group">

                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                        <asp:TextBox ID="t_desde_mov" AutoCompleteType="Disabled" autocomplete="off" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" ID="CalendarExtender15" runat="server" TargetControlID="t_desde_mov" Format="dd/MM/yyyy" />

                                                    </div>
                                                </div>
                                                <label class="col-sm-1 col-lg-1 control-label"><b>Hasta</b></label>
                                                <div class="col-sm-3 col-lg-3 controls">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                        <asp:TextBox ID="t_hasta_mov" AutoCompleteType="Disabled" autocomplete="off" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" ID="CalendarExtender16" runat="server" TargetControlID="t_hasta_mov" Format="dd/MM/yyyy" />

                                                    </div>
                                                </div>
                                                <div class="pull-right" id="Div1" style="position: relative;">
                                                    <asp:Button ID="btn_filtra_mov" CssClass="btn btn-lime" OnClientClick="lod()" Text="Filtrar" runat="server" OnClick="btn_filtra_mov_Click" />
                                                    <%--<i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_en_filtrar" style="display: none; font-size: 3em;"></i>--%>
                                                    <input id="btn_grafico_cobr" type="button" class="btn btn-success" value="Gráfico" onclick="grafico()" style="display: block; margin-top: 20%;" />
                                                </div>

                                            </div>

                                            <div id="gif_del_info" style="display: none;">
                                                <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" style="font-size: 3em;"></i>
                                            </div>


                                            <br />
                                            <hr />



                                            <div class="form-group" runat="server" id="oculta_cuando_mov" style="display: none; width: 100%;">



                                                <div class="box-content">
                                                    <div class="row">
                                                        <div class="input-group">
                                                            <%--<input type="text" id="t_filtro_memoria2" style="width: 200px; margin-right: 7px; padding: 5px;" placeholder="Filtrar..." class="form-control" />--%>
                                                            <input type="text" id="t_filtro_memoria3" placeholder="Filtrar..." class="form-control" style="width: 300px">
                                                            <button class="btn-sm btn btn-primary" type="button" id="btn_filtro_nuevo3"><i class="fa fa-search"></i></button>
                                                        </div>


                                                        <div class="btn-toolbar pull-right">

                                                            <div class="btn-group">
                                                                <asp:LinkButton ID="btn_excel_3" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Exportar a Excel" runat="server" OnClick="btn_excel_3_Click"></asp:LinkButton>
                                                            </div>

                                                        </div>
                                                        <div class="input-group">
                                                        </div>
                                                        <div class="row"></div>

                                                        <div class="row"></div>
                                                        <div style="overflow-y: auto; height: 630px">
                                                            <asp:GridView ID="G_MOV_SOL" ClientIDMode="Static" CssClass="table table-advance tablesorter filtrar3" OnRowDataBound="G_MOV_SOL_RowDataBound" runat="server" Visible="true"
                                                                ShowHeaderWhenEmpty="True" Font-Size="13px">
                                                                <HeaderStyle CssClass="test no-sort" />
                                                                <Columns>

                                                                    <asp:TemplateField HeaderText="">
                                                                        <ItemTemplate>
                                                                            <%--   <asp:CheckBox ID="chkAccept2" runat="server" EnableViewState="true" Checked='<%#Convert.ToBoolean(Eval("estado_ingresado")) %>'
                                                                                OnCheckedChanged="chkAccept_CheckedChanged" />--%>
                                                                            <asp:CheckBox ID="chkAccept2" runat="server" EnableViewState="true" Checked="false"
                                                                                OnCheckedChanged="chkAccept_CheckedChanged" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>




                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    No existen datos.
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </div>

                                                        <div class="row">
                                                            <hr />
                                                            <hr />
                                                            <div class="col-sm-12">
                                                                <div class="col-sm-2 controls">
                                                                    <asp:Button ID="btn_listos" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-primary" OnClick="btn_listos_Click" Text="Ingresado y Actualizar saldos" />
                                                                </div>
                                                                <div class="col-sm-2 controls">
                                                                    <asp:Button ID="btn_actualizar_saldos" runat="server" OnClientClick="CARGANDO();" Style="color: white; visibility: hidden;" Class="btn btn-success" OnClick="btn_actualizar_saldos_Click" Text="Actualizar Saldos" />
                                                                </div>
                                                                <div class="btn-toolbar pull-right">
                                                                    <div class="btn-group">
                                                                        <input id="btn_cerrrados_en_App" type="button" class="btn btn-warning" value="Cerrados en app" onclick="Cerrados_por_app()" style="display: block; margin-left: 2%;" />
                                                                    </div>
                                                                    <div id="tabla_semanas"></div>
                                                                    <div id="tabla_semanas2"></div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="panel_cheques" Style="display: none; width: 100%;">
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btn_cheques_pagar" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <div class="form-group">
                                                        <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;"><b>Periodo Desde</b></label>
                                                        <div class="col-sm-3 col-lg-3 controls">
                                                            <div class="input-group">
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <asp:TextBox ID="t_desde_cheq" AutoCompleteType="Disabled" autocomplete="off" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                                <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" ID="CalendarExtender21" runat="server" TargetControlID="t_desde_cheq" Format="dd/MM/yyyy" />
                                                            </div>
                                                        </div>
                                                        <label class="col-sm-1 col-lg-1 control-label"><b>Hasta</b></label>
                                                        <div class="col-sm-3 col-lg-3 controls">
                                                            <div class="input-group">
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <asp:TextBox ID="t_hasta_cheq" AutoCompleteType="Disabled" autocomplete="off" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                                <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" ID="CalendarExtender22" runat="server" TargetControlID="t_hasta_cheq" Format="dd/MM/yyyy" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <label class="col-sm-1 control-label">
                                                            <b>Vendedor:</b>
                                                        </label>
                                                        <div class="col-sm-3 controls">
                                                            <%--<asp:ListBox ID="CB_VENDEDOR_CHEQ" Width="300" SelectionMode="Multiple" ClientIDMode="Static" data-placeholder=" -- Seleccione -- " runat="server" class="form-control chosen-select"></asp:ListBox>--%>
                                                            <asp:DropDownList runat="server" ID="CB_VENDEDOR_CHEQ" ClientIDMode="Static" CssClass="form-control chosen"></asp:DropDownList>
                                                        </div>
                                                        <label class="col-sm-1 control-label">
                                                            <b>Cliente:</b>
                                                        </label>
                                                        <div class="col-sm-3 controls">
                                                            <%--<asp:ListBox ID="CB_CLIENTE_CHEQ" Width="300" SelectionMode="Multiple" ClientIDMode="Static" data-placeholder=" -- Seleccione -- " runat="server" class="form-control chosen-select"></asp:ListBox>--%>
                                                            <asp:DropDownList runat="server" ID="CB_CLIENTE_CHEQ" ClientIDMode="Static" CssClass="form-control chosen"></asp:DropDownList>
                                                        </div>
                                                        <br />
                                                        <br />
                                                        <br />

                                                        <label class="col-sm-1 control-label">
                                                            <b>Nº CHEQUE.(separar por coma):</b>
                                                        </label>
                                                        <div class="col-sm-3 controls">
                                                            <asp:TextBox runat="server" CssClass="form-control" ID="txt_num_cheq"></asp:TextBox>
                                                        </div>

                                                    </div>
                                                    <div class="pull-right" id="div_btn" style="position: relative;">
                                                        <asp:Button ID="btn_cheques_pagar" CssClass="btn btn-lime" OnClientClick="combos_refresh();cargar_cheques();" Text="Filtrar" runat="server" OnClick="btn_cheques_Click" />
                                                        <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_en_filtrar_" style="display: none; font-size: 3em;"></i>
                                                    </div>
                                                    <br />
                                                    <br />
                                                    <br />


                                                    <div style="overflow: auto;">
                                                        <asp:GridView ID="G_CHEQUES" ClientIDMode="Static" CssClass="table table-advance tablesorter filtrar" OnRowDataBound="G_CHEQUES_RowDataBound" runat="server"
                                                            Visible="true" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" Font-Size="13px" DataKeyNames="">
                                                            <HeaderStyle CssClass="test no-sort" />
                                                            <Columns>

                                                                <asp:BoundField DataField="factura" HeaderText="N° CHEQUE" />
                                                                <asp:BoundField DataField="fecha_trans" DataFormatString="{0:dd/MM/yyyy}" HeaderText="FECHA CREACIÓN" />
                                                                <asp:BoundField DataField="fecha_venc" DataFormatString="{0:dd/MM/yyyy}" HeaderText="FECHA VENCIMIENTO" />
                                                                <asp:TemplateField HeaderText="CLIENTE">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" Text='<%#Eval("NomClien")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="VENDEDOR">
                                                                    <ItemTemplate>
                                                                        (<asp:Label runat="server" Text='<%#Eval("vendedor").ToString().Trim() %>'>
                                                                        </asp:Label>)
                                                                        <asp:Label runat="server" Text='<%#Eval("nombrevendedor").ToString().Trim() %>'>
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="monto_doc" DataFormatString="{0:C0}" HeaderText="MONTO(PESO)" />
                                                                <asp:BoundField DataField="monto_usd_original" DataFormatString="{0:$ #,0.00}" HeaderText="MONTO(DOLAR)" />
                                                                <asp:BoundField DataField="descr" HeaderText="DESCR" />
                                                                <asp:TemplateField HeaderText="ESTADO">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" Text='<%# estado_return_( Eval("estado_doc").ToString() )%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:BoundField DataField="TIENE_PAGO" DataFormatString="{0:dd/MM/yyyy}" HeaderText="FECHA PAGO (ULT)" />
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                No existen datos.
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </div>

                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>

                                        <asp:Panel runat="server" ID="grilla" Style="display: block; width: 100%;">
                                            <div class="form-group">
                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;"><b>Periodo Desde</b></label>
                                                <div class="col-sm-3 col-lg-3 controls">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <asp:TextBox ID="txt_desde2" AutoCompleteType="Disabled" autocomplete="off" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" ID="CalendarExtender9" runat="server" TargetControlID="txt_desde2" Format="dd/MM/yyyy" />
                                                    </div>
                                                </div>
                                                <label class="col-sm-1 col-lg-1 control-label"><b>Hasta</b></label>
                                                <div class="col-sm-3 col-lg-3 controls">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                        <asp:TextBox ID="txt_hasta2" AutoCompleteType="Disabled" autocomplete="off" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" ID="CalendarExtender10" runat="server" TargetControlID="txt_hasta2" Format="dd/MM/yyyy" />
                                                    </div>
                                                </div>

                                                <label class="col-sm-1 control-label">
                                                    <b>Tipo Documento:</b>
                                                </label>

                                                <div class="col-sm-3 controls">
                                                    <asp:ListBox runat="server" ID="CB_TIPO_DOC_GRILLA" data-placeholder=" -- Todos -- " SelectionMode="Multiple" CssClass="form-control chosen">

                                                        <asp:ListItem Text="ChequeProtestado" Value="ChequeProtestado"> </asp:ListItem>
                                                        <asp:ListItem Text="NotaDébito" Value="NotaDebito"> </asp:ListItem>
                                                        <asp:ListItem Text="Factura" Value="Factura"> </asp:ListItem>
                                                        <asp:ListItem Text="Saldo a favor" Value="SaldoFavor"> </asp:ListItem>
                                                        <asp:ListItem Text="NotaCrédito" Value="NotaCredito"> </asp:ListItem>
                                                    </asp:ListBox>
                                                </div>
                                                <br />
                                                <br />
                                                <br />


                                                <label class="col-sm-1 control-label">
                                                    <b>Vendedor:</b>
                                                </label>
                                                <div class="col-sm-3 controls">
                                                    <%--     <asp:DropDownList runat="server" ID="CB_VENDEDOR_GRILLA" data-placeholder=" -- Seleccione -- " ClientIDMode="Static" CssClass="form-control chosen">
                                                    </asp:DropDownList>--%>

                                                    <asp:ListBox ID="CB_VENDEDOR_GRILLA" SelectionMode="Multiple" ClientIDMode="Static" data-placeholder=" -- Seleccione -- " runat="server" class="form-control chosen-select"></asp:ListBox>

                                                </div>


                                                <label class="col-sm-1 control-label">
                                                    <b>Cliente:</b>
                                                </label>
                                                <div class="col-sm-3 controls">
                                                    <%-- <asp:DropDownList runat="server" ID="CB_CLIENTE_GRILLA" data-placeholder=" -- Seleccione -- " ClientIDMode="Static" CssClass="form-control chosen">
                                                    </asp:DropDownList>--%>

                                                    <asp:ListBox ID="CB_CLIENTE_GRILLA" SelectionMode="Multiple" ClientIDMode="Static" data-placeholder=" -- Seleccione -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                </div>



                                                <label class="col-sm-1 control-label">
                                                    <b>EstadoDoc.:</b>
                                                </label>
                                                <div class="col-sm-3 controls">
                                                    <div class="input-group" id="check_cerrados">
                                                        <asp:RadioButton ID="rd_cerr" Text="&nbsp;&nbsp;Cerrados" Style="padding-right: 20px;"
                                                            GroupName="cerrados" runat="server" />

                                                        <asp:RadioButton ID="rd_abi" Style="padding-right: 20px;" Text="&nbsp;&nbsp;Abiertos" Checked="True"
                                                            GroupName="cerrados" runat="server" />
                                                    </div>
                                                    <div style="float: right; visibility: hidden; position: absolute;">
                                                        Cargar filtros
                                                        <asp:ImageButton ID="b" ImageUrl="~/img/Ticket_verde.png" runat="server" OnClientClick="CARGA_FECHA();" OnClick="b_Click" />
                                                        <i class="fa fa-circle-o-notch fa-spin" id="carga_fecha" runat="server" style="font-size: 2em; display: none;"></i>

                                                    </div>
                                                </div>

                                                <br />
                                                <br />
                                                <br />
                                                <label class="col-sm-1 control-label">
                                                    <b>Nº Docum.(separar por coma):</b>
                                                </label>
                                                <div class="col-sm-3 controls">
                                                    <asp:TextBox runat="server" CssClass="form-control" ID="num_docum"></asp:TextBox>
                                                </div>

                                                <label class="col-sm-1 control-label">
                                                    <b>Ocultar cajas de texto:</b>
                                                </label>
                                                <div class="col-sm-3 controls">
                                                    <asp:CheckBox runat="server" CssClass="" Checked="true" ID="chk_ocultar_txt"></asp:CheckBox>
                                                </div>


                                            </div>
                                            <div class="pull-right" id="Div1" style="position: relative;">
                                                <asp:Button ID="btn_filtra_grilla" CssClass="btn btn-lime" OnClientClick="cargando_en_grilla()" Text="Filtrar" runat="server" OnClick="btn_filtra_grilla_Click" />
                                                <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_en_filtrar" style="display: none; font-size: 3em;"></i>
                                            </div>
                                            <br />
                                            <hr />

                                            <div id="ocultar_principio" style="display: none;">
                                                <div class="btn-toolbar pull-left">
                                                    <%--<input type="text" id="t_filtro_memoria" style="width: 200px; margin-right: 7px; padding: 5px;" placeholder="Filtrar..." class="form-control" onchange="changevalue();" onkeyup="changevalue();" />--%>
                                                </div>
                                                <div class="btn-toolbar pull-right">

                                                    <div class="btn-group">
                                                        <asp:LinkButton ID="btn_excel2" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Exportar a Excel" runat="server" OnClick="btn_excel2_Click"></asp:LinkButton>
                                                    </div>

                                                </div>
                                                <div class="input-group">
                                                    <input type="text" id="t_filtro_memoria" placeholder="Filtrar..." class="form-control" style="width: 300px">
                                                    <button class="btn-sm btn btn-primary" type="button" id="btn_filtro_nuevo"><i class="fa fa-search"></i></button>
                                                </div>
                                                <br />
                                                <div class="row"></div>
                                                <div class="row"></div>
                                                <div style="overflow: auto;">
                                                    <asp:GridView ID="G_INIT" ClientIDMode="Static" CssClass="table table-advance tablesorter filtrar" OnRowDataBound="G_INIT_RowDataBound" runat="server" Visible="true"
                                                        ShowHeaderWhenEmpty="True" Font-Size="13px" DataKeyNames="rutcliente, id, Saldo_Peso, Saldo_Dolar, TDoc, NºDoc, FVenc, neto_peso, tcamb">
                                                        <HeaderStyle CssClass="test no-sort" />
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkAccept" runat="server" EnableViewState="true"
                                                                        OnCheckedChanged="chkAccept_CheckedChanged" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <%--ItemStyle-HorizontalAlign="Center"--%>
                                                            <asp:TemplateField HeaderText="Abon(Peso)">
                                                                <ItemTemplate>
                                                                    <div id="prueba_div" style="height: 0px; width: 0px; overflow: hidden">
                                                                    </div>
                                                                    <asp:TextBox runat="server" ID="txt_peso" Style="width: 78px;" Text='<%# Eval("Saldo_Peso") %>' CssClass="txtgrilla" />
                                                                    <%--    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="/images/save-128.png" Width="17"
                                                                        CommandName="Actualiza_invoice" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="15px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--ItemStyle-HorizontalAlign="Center"--%>

                                                            <asp:TemplateField HeaderText="Abon(Dolar)">
                                                                <ItemTemplate>
                                                                    <div id="prueba_div" style="height: 0px; width: 0px; overflow: hidden">
                                                                    </div>
                                                                    <asp:TextBox runat="server" ID="txt_dolar" Style="width: 78px;" Text='<%# Eval("Saldo_Dolar") %>' CssClass="txtgrilla" />
                                                                    <%--    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="/images/save-128.png" Width="17"
                                                                        CommandName="Actualiza_invoice" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="15px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>




                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            No existen datos.
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>


                                                    <asp:GridView ID="G_INIT_EXCEL" ClientIDMode="Static" CssClass="table table-advance tablesorter filtrar" OnRowDataBound="G_INIT_EXCEL_RowDataBound" runat="server" Visible="false"
                                                        ShowHeaderWhenEmpty="True" Font-Size="12px" DataKeyNames="rutcliente, id, Saldo_Peso, Saldo_Dolar, TDoc, NºDoc">
                                                        <HeaderStyle CssClass="test no-sort" />
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkAccept" runat="server" EnableViewState="true"
                                                                        OnCheckedChanged="chkAccept_CheckedChanged" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <%--ItemStyle-HorizontalAlign="Center"--%>
                                                            <asp:TemplateField HeaderText="Abon(Peso)">
                                                                <ItemTemplate>
                                                                    <div id="prueba_div" style="height: 0px; width: 0px; overflow: hidden">
                                                                    </div>
                                                                    <asp:TextBox runat="server" ID="txt_peso" Style="width: 78px;" Text='<%# Eval("Saldo_Peso") %>' CssClass="txtgrilla" />
                                                                    <%--    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="/images/save-128.png" Width="17"
                                                                        CommandName="Actualiza_invoice" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="15px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--ItemStyle-HorizontalAlign="Center"--%>

                                                            <asp:TemplateField HeaderText="Abon(Dolar)">
                                                                <ItemTemplate>
                                                                    <div id="prueba_div" style="height: 0px; width: 0px; overflow: hidden">
                                                                    </div>
                                                                    <asp:TextBox runat="server" ID="txt_dolar" Style="width: 78px;" Text='<%# Eval("Saldo_Dolar") %>' CssClass="txtgrilla" />
                                                                    <%--    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="/images/save-128.png" Width="17"
                                                                        CommandName="Actualiza_invoice" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="15px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>




                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            No existen datos.
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>

                                                </div>
                                                <div id="cerrad_abier" runat="server" class="col-sm-12">

                                                    <div class="row grid-demo">
                                                        <div class="col-md-1">
                                                            <asp:Button ID="btn_cobranza_2" CssClass="btn btn-primary" OnClientClick="carga_tablax_2();" Text="Iniciar Cobranza" runat="server" OnClick="INICIAR_COBRANZA_Click" />
                                                        </div>

                                                        <div class="col-md-8">
                                                        </div>
                                                        <div class="col-md-3">
                                                            <label class="col-sm-2 control-label">
                                                                <b>ENVIAR SOLOMON</b>
                                                            </label>

                                                            <!-- Rounded switch -->
                                                            <label class="switch">
                                                                <%--<input type="checkbox"  id="chk_enviar_erp">--%>
                                                                <asp:CheckBox runat="server" ClientIDMode="Static" ID="chk_enviar_erp" />
                                                                <span class="slider round" style="margin: 0px 0;"></span>
                                                            </label>
                                                        </div>

                                                    </div>
                                                    <hr />
                                                    <asp:TextBox runat="server" ID="fact_sele" Enabled="false" ClientIDMode="Static" Style="color: red; width: 100%; font-size: 12px; border: none; background-color: transparent;"></asp:TextBox>

                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                                                        <Triggers>
                                                            <%--<asp:AsyncPostBackTrigger ControlID="btn_cheques_pagar" />--%>
                                                        </Triggers>
                                                        <ContentTemplate>


                                                            <div style="overflow-y: auto;">
                                                                <asp:GridView ID="G_FACTURAS_PAGABLES" ClientIDMode="Static" CssClass="table table-advance tablesorter" OnRowDataBound="G_FACTURAS_PAGABLES_RowDataBound" runat="server" Visible="true"
                                                                    DataKeyNames="factura, tipo_doc, saldo_peso, saldo_dolar, fvenc, tasacambio,sw_abono " ShowHeaderWhenEmpty="True" Font-Size="13px" AutoGenerateColumns="false">
                                                                    <HeaderStyle CssClass="test no-sort" Font-Bold="True" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="CH_CM" runat="server" Visible='<%# es_visible( Eval("tipo_doc").ToString(),Eval("factura").ToString()  ) %>' EnableViewState="true" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:BoundField DataField="factura" HeaderText="NºDoc" />
                                                                        <asp:BoundField DataField="tipo_doc" HeaderText="TDoc">
                                                                            <ControlStyle Font-Bold="True" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="saldo_peso" HeaderText="SaldoPeso" />
                                                                        <asp:BoundField DataField="saldo_dolar" HeaderText="SaldoDolar" />
                                                                        <asp:BoundField DataField="fvenc" HeaderText="FVenc" />
                                                                        <asp:BoundField DataField="tasacambio" HeaderText="TasaCambio" />

                                                                        <asp:BoundField DataField="saldo_final_peso" Visible="true" HeaderText="Saldo a Aplicar(Peso)" />
                                                                        <asp:BoundField DataField="saldo_final_dolar" Visible="true" HeaderText="Saldo a Aplicar(Dolar)" />
                                                                        <asp:BoundField DataField="facturas_aplicadas" Visible="true" HeaderText="FacturasAplicadas" />

                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        No existen datos.
                                                                    </EmptyDataTemplate>
                                                                </asp:GridView>
                                                            </div>

                                                            <asp:Button ID="btn_pagables" runat="server" Text="ASIGNAR NOTA CREDITO" Visible="false" OnClick="btn_pagables_Click" class="btn btn-success" />
                                                            <%--<asp:Button ID="BTN_NETEO" runat="server" Text="NETEO" Visible="false" OnClientClick="REGISTRAR_PAGO2();" class="btn btn-success" />--%>
                                                            <%--<input type="button" id="BTN_NETEO" style="display:none;" class="btn btn-primary" value="NETEO" onclick="REGISTRAR_PAGO2();" />--%>

                                                            <asp:Panel runat="server" Visible="false" ID="P_FECHA_NET" ClientIDMode="Static">
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div class="form-group">
                                                                            <asp:TextBox runat="server" ID="T_FECHA_NETEO" ClientIDMode="Static" CssClass="form-control input-sm" placeholder="Fecha"></asp:TextBox>
                                                                            <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" runat="server" ID="CalendarExtender23" TargetControlID="T_FECHA_NETEO"></ajaxToolkit:CalendarExtender>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>
                                                            <asp:LinkButton ID="BTN_NETEO_2" Visible="false" runat="server" OnClientClick="CARGANDO_G();rescatarfecha();"
                                                                Style="color: white;" Class="btn btn-lime"
                                                                OnClick="btn_pago2_Click" Text="NETEO" />


                                                            <div id="montos_totales" runat="server"></div>

                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>


                                                    <hr />
                                                    <div class="row" id="cobranza2" runat="server" visible="false">


                                                        <div class="col-sm-12">

                                                            <asp:Panel runat="server" ID="PANEL_DATOS_CLIENTE2" ClientIDMode="Static">
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div class="box">
                                                                            <div class="form-group">

                                                                                <div class="row">
                                                                                    <div class="text-right" style="padding-right: 3%;">
                                                                                        <b></b>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                                    <div class="panel panel-primary" style="border-radius: 34px;">
                                                                                        <div class="panel-body">
                                                                                            <div class="form-group">
                                                                                                <div class="row" id="tabla_contacto2">
                                                                                                    <div class="col-md-12">
                                                                                                        <div id="contact2"></div>
                                                                                                    </div>
                                                                                                </div>

                                                                                                <div class="row" id="solomon_contacto2">

                                                                                                    <div class="col-md-1">
                                                                                                        <p>
                                                                                                            <input type="text" id="Text1" style="border: none; background-color: transparent; visibility: hidden" />
                                                                                                            <input type="text" id="Text2" style="border: none; background-color: transparent; visibility: hidden" />
                                                                                                        </p>
                                                                                                    </div>
                                                                                                    <div class="col-md-2 company-info">
                                                                                                        <p>
                                                                                                            <i class="fa fa-user"></i>
                                                                                                            <input type="text" id="vendedor_2" style="border: none; background-color: transparent;" />
                                                                                                            <input type="text" id="codvendedor2" style="border: none; background-color: transparent; visibility: hidden" />
                                                                                                        </p>
                                                                                                    </div>
                                                                                                    <div class="col-md-2 company-info">
                                                                                                        <p>
                                                                                                            <i class="fa fa-location-arrow"></i>
                                                                                                            <input type="text" id="direccion_2" style="border: none; background-color: transparent;" />
                                                                                                        </p>
                                                                                                    </div>
                                                                                                    <div class="col-md-2 company-info">
                                                                                                        <p>
                                                                                                            <i class="fa fa-flag"></i>
                                                                                                            <input type="text" id="ciudad_2" style="border: none; background-color: transparent;" />
                                                                                                        </p>
                                                                                                    </div>
                                                                                                    <div class="col-md-2 company-info">
                                                                                                        <p>
                                                                                                            <i class="fa fa-phone"></i>
                                                                                                            <input type="text" id="fono_2" style="border: none; background-color: transparent;" />
                                                                                                        </p>
                                                                                                    </div>

                                                                                                    <div class="col-md-2 company-info">
                                                                                                        <p>
                                                                                                            <i class="fa fa-money"></i>
                                                                                                            <input type="text" id="l_credito2" style="border: none; background-color: transparent;" />
                                                                                                        </p>
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
                                                            </asp:Panel>
                                                            <asp:Panel runat="server" ID="PANEL_ACCION_RESPUESTA2" ClientIDMode="Static">
                                                                <div class="box box-green" style="margin-top: 1%;">

                                                                    <div class="box-title">
                                                                        <h3><i class="fa fa-bars"></i>ACCION</h3>
                                                                        <div class="box-tool">
                                                                            <a data-action="collapse" href="#"><i class="fa fa-chevron-up"></i></a>
                                                                        </div>
                                                                    </div>

                                                                    <div class="box-content">
                                                                        <div class="form-group">
                                                                            <div class="col-md-12">
                                                                                <label class="col-sm-2 control-label">
                                                                                    <b>Acción :</b>
                                                                                </label>
                                                                                <div class="col-sm-3 controls">
                                                                                    <select class="form-control input-sm" id="CB_ACCIONES2" onchange="CAMBIA_ACCION2()">
                                                                                        <option value="-1" selected>-- Seleccione --</option>
                                                                                        <option value="llamar">Llamado...</option>
                                                                                        <option value="agendar">Agendar...</option>
                                                                                        <option value="pagar">Pagar...</option>

                                                                                        <%--<option value="agendar">Agendar...</option>--%>
                                                                                    </select>
                                                                                </div>

                                                                                <div class="col-sm-4 controls">
                                                                                    <input type="text" class="form-control input-sm" id="OBS_LLAMAR2" style="display: none;" placeholder="Observación..." />
                                                                                </div>
                                                                                <div class="col-sm-2 controls">
                                                                                    <input type="button" id="aceptar2" class="btn btn-primary" value="Aceptar" style="display: none;" onclick="GuardaAccion2()" />
                                                                                </div>
                                                                            </div>

                                                                        </div>
                                                                    </div>
                                                            </asp:Panel>

                                                            <asp:Panel runat="server" ID="PANEL_SEGUIMIENTO2" ClientIDMode="Static">


                                                                <asp:Panel runat="server" ID="PANEL_AGENDA2" ClientIDMode="Static">

                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <div class="box">
                                                                                <div class="form-group">

                                                                                    <label class="col-sm-2 control-label">
                                                                                        <b>Agendar el día :</b>
                                                                                    </label>
                                                                                    <div class="col-sm-2 controls">
                                                                                        <asp:TextBox runat="server" ID="T_FECHA_AGENDA2" ClientIDMode="Static" CssClass="form-control input-sm"></asp:TextBox>
                                                                                        <%--<span class="help-inline">Fecha</span>--%>
                                                                                        <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" runat="server" ID="CalendarExtender11" TargetControlID="T_FECHA_AGENDA2"></ajaxToolkit:CalendarExtender>
                                                                                    </div>
                                                                                    <div class="col-sm-4 controls">
                                                                                        <input type="text" class="form-control input-sm" id="OBS_AGENDA2" placeholder="Observación..." />
                                                                                        <%--<span class="help-inline">Observación</span>--%>
                                                                                    </div>
                                                                                    <div class="col-sm-2 controls">
                                                                                        <input type="button" class="btn btn-primary" value="Agendar" onclick=" GuardaAccion2(); AGENDAR2();" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                </asp:Panel>

                                                                <asp:Panel runat="server" ID="PANEL_PAGO2" ClientIDMode="Static">
                                                                    <hr />
                                                                    <div class="row">
                                                                        <div class="col-md-12" style="left: 1%">
                                                                            <div class="box">
                                                                                <div class="form-group">

                                                                                    <input type="text" style="visibility: hidden; position: absolute;" class="form-control input-sm" id="monto_total_peso_f" placeholder="peso..." />
                                                                                    <input type="text" style="visibility: hidden; position: absolute;" class="form-control input-sm" id="monto_total_dolar_f" placeholder="dolar..." />


                                                                                    <label class="col-sm-2 control-label">
                                                                                        <b>Tipo de Pago :</b>
                                                                                    </label>
                                                                                    <div class="col-sm-3 controls">
                                                                                        <select class="form-control input-sm" id="CB_TIPO_PAGO2" onchange="CAMBIA_TIPO_PAGO2()">
                                                                                            <option value="-1" selected>-- Seleccione --</option>
                                                                                            <option value="efectivo">Deposito</option>
                                                                                            <option value="cheque">Cheque</option>

                                                                                            <option value="tarjeta">Tarjeta</option>
                                                                                        </select>
                                                                                    </div>
                                                                                    <%--<div class="col-sm-4 controls">
                                                                <input type="checkbox" id="ch_cerrar" />
                                                                Cerrar Documento?
                                                            </div>--%>
                                                                                </div>
                                                                                <asp:Panel runat="server" ID="PANEL_MONTO2" ClientIDMode="Static">
                                                                                    <div class="row">
                                                                                        <div class="col-md-12">
                                                                                            <div class="box box-green" style="margin-top: 1%;">

                                                                                                <div class="box-title">
                                                                                                    <h3><i class="fa fa-bars"></i>PAGO</h3>
                                                                                                    <div class="box-tool">
                                                                                                        <a data-action="collapse" href="#"><i class="fa fa-chevron-up"></i></a>
                                                                                                    </div>
                                                                                                </div>

                                                                                                <div class="box-content">
                                                                                                    <div class="form-horizontal">
                                                                                                        <div class="row" style="margin-right: 10px; margin-left: 10px;">
                                                                                                            <div class="col-md-5">

                                                                                                                <div class="form-group">
                                                                                                                    <asp:DropDownList runat="server" ID="CB_DEPOSITOS_BANCO2" ClientIDMode="Static" onchange="CAMBIA_TIPO_DEPOSITO()" CssClass="form-control input-sm chosen"></asp:DropDownList>
                                                                                                                </div>
                                                                                                                <div class="form-group">
                                                                                                                    <select class="form-control input-sm" id="CB_TIPO_MONEDA2" onchange="CAMBIA_PESO_DOLAR()">
                                                                                                                        >
                                                                                                                        <option value="peso" selected>Peso</option>
                                                                                                                        <option value="dolar">Dolar</option>
                                                                                                                    </select>
                                                                                                                </div>

                                                                                                                <i id="T_BANCO_2_LABEL"></i>
                                                                                                                <i style="font-size: 13px; color: yellowgreen;" id="T_BANCO_2_LABEL_LETRAS"></i>
                                                                                                                <%--                <h4 style="font-size:13px;color:yellowgreen;" id="T_BANCO_2_LABEL_LETRAS"></h4>--%>

                                                                                                                <div class="form-group">
                                                                                                                    <input type="text" maxlength="10" class="form-control input-sm" id="T_DESCRIPCION_PAGO2" placeholder="Descripcion..." />
                                                                                                                </div>

                                                                                                            </div>
                                                                                                            <div class="col-md-5" style="left: 5%;">

                                                                                                                <div class="form-group">
                                                                                                                    <input type="number" class="form-control input-sm" step="any" pattern="[0-9]+([\.,][0-9]+)?" id="T_MONTO_PAGO2" placeholder="Monto..." />                                                                                                                    <span class="help-inline">Monto</span>
                                                                                                                </div>

                                                                                                                <div class="form-group">
                                                                                                                    <asp:TextBox runat="server" ID="t_fech_efec" ClientIDMode="Static" CssClass="form-control input-sm" placeholder="Fecha"></asp:TextBox>
                                                                                                                    <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" runat="server" ID="CalendarExtender14" TargetControlID="t_fech_efec"></ajaxToolkit:CalendarExtender>
                                                                                                                </div>
                                                                                                            </div>

                                                                                                        </div>
                                                                                                    </div>

                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>

                                                                                </asp:Panel>

                                                                                <asp:Panel runat="server" ID="PANEL_BANCO2" ClientIDMode="Static">
                                                                                    <br />
                                                                                    <div class="row">
                                                                                        <div class="col-md-12">
                                                                                            <div class="box box-green" style="margin-top: 1%;">

                                                                                                <div class="box-title">
                                                                                                    <h3><i class="fa fa-bars"></i>PAGO CHEQUE</h3>
                                                                                                    <div class="box-tool">
                                                                                                        <a data-action="collapse" href="#"><i class="fa fa-chevron-up"></i></a>
                                                                                                    </div>
                                                                                                </div>

                                                                                                <div class="box-content">
                                                                                                    <div class="form-horizontal">
                                                                                                        <div class="row" style="margin-right: 10px; margin-left: 10px;">
                                                                                                            <div class="col-md-5">
                                                                                                                <div class="form-group">
                                                                                                                    <asp:DropDownList runat="server" ID="CB_BANCOS2" ClientIDMode="Static" CssClass="form-control chosen"></asp:DropDownList>
                                                                                                                    <%--<asp:DropDownList runat="server" ID="cb_productos_kg" onchange="esconde()" ClientIDMode="Static" CssClass="form-control chosen"></asp:DropDownList>--%>
                                                                                                                </div>
                                                                                                                <div class="form-group">
                                                                                                                    <select class="form-control" runat="server" id="cb_tipo_pago_cheque" onchange="CAMBIA_PESO_DOLAR_CHEQ()">
                                                                                                                        <option value="peso" selected="selected">Peso</option>
                                                                                                                        <option value="dolar">Dolar</option>
                                                                                                                    </select>
                                                                                                                </div>
                                                                                                                <div class="form-group">
                                                                                                                    <input type="text" maxlength="8" class="form-control" id="T_NUM_CHEQUE2" placeholder="Nº Cheque..." />
                                                                                                                </div>
                                                                                                                <div class="form-group">
                                                                                                                    <input type="number" class="form-control" step="any" pattern="[0-9]+([\.,][0-9]+)?"  id="T_Cuenta2" placeholder="Monto..." />
                                                                                                                    <span class="help-inline">Monto</span>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <div class="col-md-2">
                                                                                                            </div>
                                                                                                            <div class="col-md-5">
                                                                                                                <div class="form-group">
                                                                                                                    <asp:TextBox runat="server" ID="T_VENCIMIENTO_CHEQUE2" ClientIDMode="Static" CssClass="form-control" placeholder="Fecha Vencimiento"></asp:TextBox>
                                                                                                                    <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" runat="server" ID="CalendarExtender12" TargetControlID="T_VENCIMIENTO_CHEQUE2"></ajaxToolkit:CalendarExtender>
                                                                                                                </div>
                                                                                                                <div class="form-group">
                                                                                                                    <input type="text" class="form-control" maxlength="30" id="T_OBS_CHEQUES" placeholder="Obs..." />
                                                                                                                </div>
                                                                                                                <div class="form-group">
                                                                                                                    <input type="text" class="form-control" id="T_CAMBIO_CHEQUES" placeholder="TipoCambio..." />
                                                                                                                </div>
                                                                                                                <div class="form-group">
                                                                                                                    <input type="text" class="form-control" id="T_TIPO_DOLAR" placeholder="OBSER-VEND-ADUANERO" style="visibility: hidden; position: absolute;" />
                                                                                                                    <asp:DropDownList runat="server" ID="CB_TIPO_DOLAR" ClientIDMode="Static" CssClass="form-control chosen"></asp:DropDownList>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>


                                                                                                <div class="row">
                                                                                                    <div class="col-md-12">
                                                                                                        <div class="box">
                                                                                                            <div class="form-group">
                                                                                                                <div class="col-sm-2 controls">
                                                                                                                    <input type="text" maxlength="8" class="form-control" id="T_DESCRIPCION_CHEQUES_3" placeholder="Descrip.paraFactura ..." />
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>



                                                                                                <div class="row">
                                                                                                    <div class="col-md-12">
                                                                                                        <div class="box">
                                                                                                            <div class="form-group">
                                                                                                                <div class="col-sm-2 controls">
                                                                                                                    <input type="button" class="btn btn-primary fa-input" value="&#xf0fe; Agregar Cheque" onclick="Agregar_Cheque2()" />
                                                                                                                </div>

                                                                                                                <div class="col-sm-2 controls">
                                                                                                                    <input type="button" class="btn btn-danger fa-input" value="&#xf00d; Borrar Todo" onclick="Borrar_Cheques2()" />
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>

                                                                                                <div class="row">
                                                                                                    <div class="col-md-12">
                                                                                                        <div class="box">
                                                                                                            <div class="form-group">
                                                                                                                <div class="col-sm-12 controls">
                                                                                                                    <div id="tabla_cheques2" name="tabla_cheques"></div>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>


                                                                                                </div>
                                                                                                <div style="visibility: hidden; position: absolute">
                                                                                                    <label class="col-sm-1 control-label">
                                                                                                        <b>Banco Destino</b>
                                                                                                    </label>
                                                                                                    <div class="col-sm-6 controls">
                                                                                                        <asp:DropDownList runat="server" ID="CB_BANCO_SOPRODI3" ClientIDMode="Static" CssClass="form-control input-sm chosen"></asp:DropDownList>
                                                                                                    </div>
                                                                                                </div>

                                                                                            </div>
                                                                                        </div>
                                                                                    </div>

                                                                                </asp:Panel>

                                                                                <asp:Panel runat="server" ID="P_REGISTRA_PAGO" ClientIDMode="Static">
                                                                                    <asp:UpdatePanel runat="server" ID="up_rgs_pag" UpdateMode="Conditional">
                                                                                        <Triggers>
                                                                                            <asp:AsyncPostBackTrigger ControlID="btn_pago2" />
                                                                                        </Triggers>
                                                                                        <ContentTemplate>
                                                                                            <hr style="margin-top: 0px; margin-bottom: 20px; border: 0; border-top: 4px solid #b5b5b5;">
                                                                                            <div class="row">
                                                                                                <div class="col-md-12">
                                                                                                    <div class="box">
                                                                                                        <div class="form-group">

                                                                                                            <div class="col-sm-3 controls">

                                                                                                                <input type="button" id="btn_pago_2" style="visibility: hidden; position: absolute;" class="btn btn-primary" value="Registrar Pago" onclick="REGISTRAR_PAGO2();" />

                                                                                                                <asp:LinkButton ID="btn_pago2" runat="server" OnClientClick="CARGANDO_G();"
                                                                                                                    Style="color: white;" Class="btn btn-lime"
                                                                                                                    OnClick="btn_pago2_Click" Text="Registrar Pago" />

                                                                                                            </div>

                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>


                                                                                            </div>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </asp:Panel>


                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                </asp:Panel>
                                                            </asp:Panel>
                                                            <asp:Panel runat="server" ID="PANEL_ENVIAR_CORREO2" ClientIDMode="Static">
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div class="box">
                                                                            <div class="form-group">
                                                                                <div class="col-sm-3 controls">
                                                                                    <input type="text" class="form-control input-sm" id="t_destino2" placeholder="Destino..." />
                                                                                </div>
                                                                                <div class="col-sm-3 controls">
                                                                                    <input type="text" class="form-control input-sm" id="t_cc2" placeholder="CC..." />
                                                                                </div>
                                                                                <div class="col-sm-4 controls">
                                                                                    <textarea class="form-control input-sm" id="t_contenido2" placeholder="Contenido..."></textarea>
                                                                                </div>

                                                                                <div class="col-sm-2 controls">
                                                                                    <input type="button" class="btn btn-primary fa-input" value="&#xf1d8; Enviar" onclick="ENVIAR_CORREO2()" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </asp:Panel>

                                                            <asp:Panel runat="server" ID="PANEL_ESTIMADO_2" ClientIDMode="Static">
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div class="box">
                                                                            <div class="form-group">
                                                                                <div class="col-sm-2 controls">
                                                                                    <asp:TextBox runat="server" ID="t_estim" ClientIDMode="Static" placeholder="Fecha" CssClass="form-control input-sm"></asp:TextBox>
                                                                                    <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" runat="server" ID="CalendarExtender13" TargetControlID="t_estim"></ajaxToolkit:CalendarExtender>
                                                                                </div>
                                                                                <div class="col-sm-4 controls">
                                                                                    <input type="text" class="form-control" id="t_monto_estim" placeholder="Monto..." />
                                                                                </div>

                                                                                <div class="col-sm-2 controls">
                                                                                    <select class="form-control input-sm" id="cb_tipo_pago_estim">
                                                                                        <option value="peso" selected="selected">Peso</option>
                                                                                        <option value="dolar">Dolar</option>
                                                                                    </select>

                                                                                </div>

                                                                                <div class="col-sm-2 controls">
                                                                                    <input type="button" class="btn btn-primary" value="Registrar Agendar Pago" onclick="REGISTRAR_AGENDAR_PAGO2()" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </asp:Panel>

                                                        </div>
                                                    </div>
                                                    <hr />


                                                    <div class="col-md-12">
                                                        <hr />
                                                        <hr />
                                                        <div class="box box-green" style="margin-top: 1%;">

                                                            <div class="box-title">
                                                                <h3><i class="fa fa-bars"></i>Agendar Estimado</h3>
                                                                <div class="box-tool">
                                                                    <a data-action="collapse" href="#"><i class="fa fa-chevron-up"></i></a>
                                                                </div>
                                                            </div>

                                                            <div class="box-content">
                                                                <div class="form-group">
                                                                    <div class="col-sm-2 controls">
                                                                        <asp:TextBox runat="server" ID="t_cobro" ClientIDMode="Static" placeholder="Fecha" CssClass="form-control input-sm"></asp:TextBox>
                                                                        <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" runat="server" ID="CalendarExtender6" TargetControlID="t_cobro"></ajaxToolkit:CalendarExtender>
                                                                    </div>
                                                                    <div class="col-sm-4 controls">
                                                                        <%--<asp:TextBox runat="server" ID="t_ob_cobro" ClientIDMode="Static" CssClass="form-control input-sm"></asp:TextBox>--%>
                                                                        <textarea runat="server" id="t_ob_cobro" class="form-control" placeholder="Observación" rows="2"></textarea>

                                                                    </div>

                                                                    <div class="col-sm-2 controls">
                                                                        <select class="form-control input-sm" runat="server" id="cb_tipo_pago_1">
                                                                            <option value="peso" selected="selected">Peso</option>
                                                                            <option value="dolar">Dolar</option>
                                                                        </select>

                                                                    </div>

                                                                    <div class="col-sm-2 controls">
                                                                        <asp:Button ID="btn_cobrar" runat="server" OnClientClick="CARGANDO();" Style="color: white; visibility: hidden; position: absolute;" Class="btn btn-primary" OnClick="btn_cobrar_Click" Text="Agendar Urgencia" />
                                                                    </div>
                                                                    <div class="col-sm-2 controls">
                                                                        <asp:Button ID="btn_estimado" runat="server" Style="color: white;" Class="btn btn-lime" OnClientClick="carga_tablax();combos_refresh();" OnClick="btn_estimado_Click" Text="Agendar Estimado" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                            <div class="row">
                                            </div>
                                        </asp:Panel>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="box-content">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="box">
                                    <div class="box-title">
                                        <h3><i class="fa fa-list"></i>Colores</h3>
                                        <div class="box-tool" style="margin-top: -0.5%;">
                                            <%--    <a data-action="collapse" id="iniciar_cerrado" href="#"><i class="fa fa-chevron-up"></i></a>--%>
                                            <%--<a data-action="close" href="#"><i class="fa fa-times"></i></a>--%>
                                        </div>

                                    </div>
                                    <div class="box-content" style="display: none;">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <h4>General</h4>
                                                <ul class="list-unstyled">
                                                    <li><span class="label label-important" style="background-color: #3A01DF;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Facturas</li>
                                                    <li><span class="label label-important" style="background-color: #6A0888;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Cheques</li>
                                                    <li><span class="label label-important" style="background-color: #00A5CD;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Facturas(PRIORI)</li>
                                                    <li><span class="label label-important" style="background-color: #F801CF;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Cheques(PRIORI)</li>
                                                    <li><span class="label label-important" style="background-color: #04B404;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Doc.Pagados</li>
                                                    <li><span class="label label-important" style="background-color: #B40404;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Agendados</li>
                                                    <li><span class="label label-important" style="background-color: #95A5A6;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Gestionados</li>
                                                </ul>
                                            </div>

                                            <div class="col-md-3">
                                                <h4>Vencimientos</h4>
                                                <ul class="list-unstyled">
                                                    <li><span class="label label-important" style="background-color: #3ADF00;">&nbsp;&nbsp;&nbsp;&nbsp;</span> NoVencidos</li>
                                                    <li><span class="label label-important" style="background-color: #BFFF00;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Vencido(<5 Días)</li>
                                                    <li><span class="label label-important" style="background-color: #FFFF00;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Vencido(>5 Días < 10 Días)</li>
                                                    <li><span class="label label-important" style="background-color: #FFBF00;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Vencido(>10 Días < 15 Días)</li>
                                                </ul>
                                            </div>
                                            <div class="col-md-3">
                                                <h4>&nbsp;&nbsp;&nbsp;</h4>
                                                <ul class="list-unstyled">
                                                    <li><span class="label label-important" style="background-color: #FF4000;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Vencido(>15 Días < 20 Días)</li>
                                                    <li><span class="label label-important" style="background-color: #DF0101;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Vencido(>20 Días < 25 Días)</li>
                                                    <li><span class="label label-important" style="background-color: #FF0000;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Vencido(>25 Días)</li>
                                                    <li><span class="label label-important" style="background-color: #000;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Vencido(>45 Días)</li>

                                                </ul>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <input id="sw_mu" type="text" style="visibility: hidden;" />
                        <div id="T_FACTURA2" style="border: none; background-color: transparent;"></div>

                        <asp:TextBox type="text" ID="T_RUT_CLIENTE2" name="T_RUT_CLIENTE" runat="server" ClientIDMode="Static" Style="visibility: hidden; border: none; background-color: transparent;" />


                    </div>
                    <%--  --%>
                    <!-- MODAL DE EDICION ( I ) -->
                    <!-- ... -->
                    <!-- ... -->
                    <!-- ... -->
                    <a id="modal_1" name="modal_1" href="#modal-1" role="button" class="btn" data-toggle="modal" style="display: none;"></a>
                    <div id="modal-1" class="modal fade">
                        <div class="modal-dialog modal-lg" style="width: 93%;">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" id="cerrar_modal_1" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                    <h3 id="myModalLabel"><i id="titulo_modal"></i></h3>
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="form-horizontal">
                                                <!-- ********************************************************************* -->
                                                <input type="hidden" id="T_ID_EVENTO" />
                                                <input type="hidden" id="T_MONTO_DOC" />
                                                <div class="form-group">
                                                    <label class="col-sm-1 control-label">
                                                        <i class="fa fa-calendar fa-2x"></i>
                                                    </label>
                                                    <div class="col-sm-2 controls">
                                                        <asp:TextBox runat="server" ID="T_F_INICIO" ClientIDMode="Static" BorderStyle="None" BackColor="Transparent" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <label class="col-sm-2 control-label">
                                                        <i class="fa fa-file-text-o fa-2x"></i>
                                                    </label>
                                                    <div class="col-sm-2 controls">
                                                        <div id="T_FACTURA" style="border: none; background-color: transparent;"></div>
                                                    </div>
                                                    <label class="col-sm-2 control-label">
                                                        <i class="fa fa-user fa-2x"></i>
                                                    </label>
                                                    <div class="col-sm-2 controls">
                                                        <input type="text" id="T_RUT_CLIENTE" name="T_RUT_CLIENTE" readonly="readonly" style="border: none; background-color: transparent;" />
                                                        <br />
                                                        <div id="ver_cliente"></div>
                                                    </div>
                                                </div>
                                                <hr />
                                                <div class="form-group">
                                                    <div class="col-sm-12">
                                                        <h3>Movimientos asociados al documento</h3>
                                                        <div id="tabla_html"></div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-sm-3 controls">
                                                        <input id="B_SEGUIMIENTO" type="button" class="btn btn-primary" value="Iniciar cobranza" onclick="Seguimiento()" />
                                                        <input id="B_VER_ACCIONES" type="button" class="btn btn-warning" value="Ver acciones cobranza" onclick="ver_acciones()" style="display: none;" />

                                                    </div>
                                                </div>

                                                <asp:Panel runat="server" ID="PANEL_DATOS_CLIENTE" ClientIDMode="Static">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="box">
                                                                <div class="form-group">

                                                                    <div class="row">
                                                                        <div class="text-right" style="padding-right: 3%;">
                                                                            <b></b>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                        <div class="panel panel-primary" style="border-radius: 34px;">
                                                                            <div class="panel-body">
                                                                                <div class="form-group">
                                                                                    <div class="row" id="tabla_contacto">
                                                                                        <div class="col-md-12">
                                                                                            <div id="contact"></div>
                                                                                        </div>
                                                                                    </div>

                                                                                    <div class="row" id="solomon_contacto">

                                                                                        <div class="col-md-1">
                                                                                            <p>

                                                                                                <input type="text" id="Text1" style="border: none; background-color: transparent; visibility: hidden" />
                                                                                                <input type="text" id="Text2" style="border: none; background-color: transparent; visibility: hidden" />
                                                                                            </p>
                                                                                        </div>
                                                                                        <div class="col-md-2 company-info">
                                                                                            <p>
                                                                                                <i class="fa fa-user"></i>
                                                                                                <input type="text" id="vendedor_" style="border: none; background-color: transparent;" />
                                                                                                <input type="text" id="codvendedor" style="border: none; background-color: transparent; visibility: hidden" />
                                                                                            </p>
                                                                                        </div>
                                                                                        <div class="col-md-2 company-info">
                                                                                            <p>
                                                                                                <i class="fa fa-location-arrow"></i>
                                                                                                <input type="text" id="direccion_" style="border: none; background-color: transparent;" />
                                                                                            </p>
                                                                                        </div>
                                                                                        <div class="col-md-2 company-info">
                                                                                            <p>
                                                                                                <i class="fa fa-flag"></i>
                                                                                                <input type="text" id="ciudad_" style="border: none; background-color: transparent;" />
                                                                                            </p>
                                                                                        </div>
                                                                                        <div class="col-md-2 company-info">
                                                                                            <p>
                                                                                                <i class="fa fa-phone"></i>
                                                                                                <input type="text" id="fono_" style="border: none; background-color: transparent;" />
                                                                                            </p>
                                                                                        </div>

                                                                                        <div class="col-md-2 company-info">
                                                                                            <p>
                                                                                                <i class="fa fa-money"></i>
                                                                                                <input type="text" id="l_credito" style="border: none; background-color: transparent;" />
                                                                                            </p>
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
                                                </asp:Panel>


                                                <asp:Panel runat="server" ID="PANEL_VER_ACCIONES" ClientIDMode="Static">
                                                    <div class="pull-right">

                                                        <div class="btn-group">

                                                            <input id="cierra_acciones" type="button" class="btn btn-warning" value="Cerrar Acciones" onclick="Cierra_accio()" style="display: block;" />

                                                        </div>
                                                    </div>
                                                    <h3>Acciones de la cobranza</h3>
                                                    <div id="tabla_acciones">
                                                    </div>


                                                </asp:Panel>


                                                <hr />
                                                <asp:Panel runat="server" ID="PANEL_ACCION_RESPUESTA" ClientIDMode="Static">

                                                    <div class="box box-green" style="margin-top: 1%;">

                                                        <div class="box-title">
                                                            <h3><i class="fa fa-bars"></i></h3>
                                                            <div class="box-tool">
                                                                <a data-action="collapse" href="#"><i class="fa fa-chevron-up"></i></a>
                                                            </div>
                                                        </div>

                                                        <div class="box-content">

                                                            <div class="form-group">
                                                                <label class="col-sm-2 control-label">
                                                                    <b>Acción :</b>
                                                                </label>
                                                                <div class="col-sm-3 controls">
                                                                    <select class="form-control input-sm" id="CB_ACCIONES" onchange="CAMBIA_ACCION()">
                                                                        <option value="-1" selected>-- Seleccione --</option>
                                                                        <option value="llamar">Llamado...</option>
                                                                        <option value="agendar">Agendar...</option>
                                                                        <option value="pagar">Pagar...</option>

                                                                        <%--<option value="agendar">Agendar...</option>--%>
                                                                    </select>
                                                                </div>

                                                                <div class="col-sm-4 controls">
                                                                    <input type="text" class="form-control input-sm" id="OBS_LLAMAR" style="display: none;" placeholder="Observación..." />
                                                                </div>
                                                                <div class="col-sm-2 controls">
                                                                    <input type="button" id="aceptar" class="btn btn-primary" value="Aceptar" style="display: none;" onclick="GuardaAccion()" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                </asp:Panel>

                                                <asp:Panel runat="server" ID="PANEL_SEGUIMIENTO" ClientIDMode="Static">


                                                    <asp:Panel runat="server" ID="PANEL_AGENDA" ClientIDMode="Static">
                                                        <div class="form-group">
                                                            <hr />
                                                            <label class="col-sm-2 control-label">
                                                                <b>Agendar el día :</b>
                                                            </label>
                                                            <div class="col-sm-2 controls">
                                                                <asp:TextBox runat="server" ID="T_FECHA_AGENDA" ClientIDMode="Static" CssClass="form-control input-sm"></asp:TextBox>
                                                                <%--<span class="help-inline">Fecha</span>--%>
                                                                <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" runat="server" ID="CalendarExtender3" TargetControlID="T_FECHA_AGENDA"></ajaxToolkit:CalendarExtender>
                                                            </div>
                                                            <div class="col-sm-4 controls">
                                                                <input type="text" class="form-control input-sm" id="OBS_AGENDA" placeholder="Observación..." />
                                                                <%--<span class="help-inline">Observación</span>--%>
                                                            </div>
                                                            <div class="col-sm-2 controls">
                                                                <input type="button" class="btn btn-primary" value="Agendar" onclick="AGENDAR()" />
                                                            </div>
                                                        </div>
                                                    </asp:Panel>


                                                    <asp:Panel runat="server" ID="PANEL_PAGO" ClientIDMode="Static">
                                                        <hr />
                                                        <div class="form-group">
                                                            <label class="col-sm-3 control-label" style="margin-left: 1%;">
                                                                <b>Tipo de Pago :</b>
                                                            </label>
                                                            <div class="col-sm-2 controls">
                                                                <select class="form-control input-sm" id="CB_TIPO_PAGO" onchange="CAMBIA_TIPO_PAGO()">
                                                                    <option value="-1" selected>-- Seleccione --</option>
                                                                    <option value="efectivo">Efectivo</option>
                                                                    <option value="cheque">Cheque</option>
                                                                </select>
                                                            </div>
                                                            <%--<div class="col-sm-4 controls">
                                                                <input type="checkbox" id="ch_cerrar" />
                                                                Cerrar Documento?
                                                            </div>--%>
                                                        </div>
                                                        <asp:Panel runat="server" ID="PANEL_MONTO" ClientIDMode="Static">
                                                            <div class="form-group">
                                                                <div class="col-sm-3 controls">
                                                                    <asp:DropDownList runat="server" ID="CB_DEPOSITOS_BANCO" ClientIDMode="Static" onchange="CAMBIA_TIPO_DEPOSITO()" CssClass="form-control input-sm"></asp:DropDownList>
                                                                </div>

                                                                <div class="col-sm-4 controls">
                                                                    <input type="text" class="form-control input-sm" id="T_DESCRIPCION_PAGO" placeholder="Descripcion..." />
                                                                </div>
                                                                <div class="col-sm-2 controls">
                                                                    <input type="text" class="form-control input-sm" id="T_MONTO_PAGO" placeholder="Monto..." />
                                                                </div>
                                                                <div class="col-sm-3 controls">
                                                                    <select class="form-control input-sm" id="CB_TIPO_MONEDA">
                                                                        <option value="peso" selected>Peso</option>
                                                                        <option value="dolar">Dolar</option>
                                                                    </select>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>

                                                        <asp:Panel runat="server" ID="PANEL_BANCO" ClientIDMode="Static">
                                                            <div class="form-group">
                                                                <div class="col-sm-3 controls">
                                                                    <asp:DropDownList runat="server" ID="CB_BANCOS" ClientIDMode="Static" CssClass="form-control input-sm"></asp:DropDownList>
                                                                </div>

                                                                <div class="col-sm-3 controls">
                                                                    <input type="text" class="form-control input-sm" id="T_NUM_CHEQUE" placeholder="Nº Cheque..." />
                                                                </div>
                                                                <div class="col-sm-3 controls">
                                                                    <input type="number" class="form-control input-sm" id="T_Cuenta" placeholder="Monto..." />
                                                                </div>
                                                                <div class="col-sm-3 controls">
                                                                    <asp:TextBox runat="server" ID="T_VENCIMIENTO_CHEQUE" ClientIDMode="Static" CssClass="form-control input-sm" placeholder="Fecha Vencimiento"></asp:TextBox>
                                                                    <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" runat="server" ID="CalendarExtender4" TargetControlID="T_VENCIMIENTO_CHEQUE"></ajaxToolkit:CalendarExtender>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <div class="col-sm-2 controls">
                                                                    <input type="button" class="btn btn-primary fa-input" value="&#xf0fe; Agregar Cheque" onclick="Agregar_Cheque()" />
                                                                </div>

                                                                <div class="col-sm-2 controls">
                                                                    <input type="button" class="btn btn-danger fa-input" value="&#xf00d; Borrar" onclick="Borrar_Cheques()" />
                                                                </div>
                                                            </div>

                                                            <div class="form-group">
                                                                <div class="col-sm-12 controls">
                                                                    <div id="tabla_cheques" name="tabla_cheques"></div>
                                                                </div>


                                                            </div>
                                                        </asp:Panel>
                                                        <div class="form-group">
                                                            <div class="col-sm-3 controls">
                                                                <input type="button" class="btn btn-primary" value="Registrar Pago" onclick="REGISTRAR_PAGO()" />
                                                            </div>

                                                            <%--  <div class="col-sm-3 controls">
                                                                <input type="button" class="btn btn-danger" value="Registrar Pago" onclick="MARCACOMOPAGADO()" />
                                                            </div>--%>
                                                        </div>
                                                    </asp:Panel>
                                                </asp:Panel>


                                                <asp:Panel runat="server" ID="PANEL_ENVIAR_CORREO" ClientIDMode="Static">
                                                    <div class="form-group">
                                                        <div class="col-sm-3 controls">
                                                            <input type="text" class="form-control input-sm" id="t_destino" placeholder="Destino..." />
                                                        </div>
                                                        <div class="col-sm-3 controls">
                                                            <input type="text" class="form-control input-sm" id="t_cc" placeholder="CC..." />
                                                        </div>
                                                        <div class="col-sm-4 controls">
                                                            <textarea class="form-control input-sm" id="t_contenido" placeholder="Contenido..."></textarea>
                                                        </div>

                                                        <div class="col-sm-2 controls">
                                                            <input type="button" class="btn btn-primary fa-input" value="&#xf1d8; Enviar" onclick="ENVIAR_CORREO()" />
                                                        </div>
                                                    </div>
                                                </asp:Panel>

                                                <asp:Panel runat="server" ID="PANEL_AGENDAR_PAGO" ClientIDMode="Static">
                                                    <div class="form-group">
                                                        <div class="col-sm-2 controls">
                                                            <asp:TextBox runat="server" ID="FECHA_AGENDAR_PAGO" ClientIDMode="Static" CssClass="form-control input-sm"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" runat="server" ID="CalendarExtender5" TargetControlID="FECHA_AGENDAR_PAGO"></ajaxToolkit:CalendarExtender>
                                                        </div>

                                                        <div class="col-sm-4 controls">
                                                            <input type="text" class="form-control input-sm" id="OBS_AGENDAR_PAGO" placeholder="Descripcion..." />
                                                        </div>
                                                        <div class="col-sm-3 controls">
                                                            <input type="number" class="form-control input-sm" id="MONTO_AGENDAR_PAGO" placeholder="Monto..." />
                                                        </div>

                                                        <div class="col-sm-2 controls">
                                                            <select class="form-control input-sm" runat="server" id="cb_tipo_pago_2">
                                                                <option value="peso" selected>Peso</option>
                                                                <option value="dolar">Dolar</option>
                                                            </select>

                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="col-sm-3 controls">
                                                            <input type="button" class="btn btn-primary" value="Registrar Agendar Pago" onclick="REGISTRAR_AGENDAR_PAGO()" />
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="modal-footer">
                                    <button class="btn" data-dismiss="modal" aria-hidden="true" id="Cerrar_modal">Cerrar</button>
                                </div>
                            </div>

                        </div>

                        <asp:TextBox ID="L_CLIENTES" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                        <asp:TextBox ID="L_VENDEDORES" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                    </div>
                </div>
            </div>

            <a id="moda_35" name="moda_35" href="#modall_35" role="button" class="btn" data-toggle="modal" style="display: none;"></a>
            <div id="modall_35" class="modal fade">
                <div class="modal-dialog modal-lg" style="width: 85%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" id="Button75" class="close" data-dismiss="modal" aria-hidden="true">×</button>

                        </div>
                        <div class="modal-body">
                            <div class="row">

                                <div class="col-sm-12">
                                    <h3>Gráfico</h3>
                                    <div class="col-sm-6">
                                        <div class="form-horizontal">

                                            <div id="container" style="min-width: 510px; height: 400px; margin: 0 auto">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-horizontal">

                                            <div id="container2" style="min-width: 510px; height: 400px; margin: 0 auto">
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" aria-hidden="true" id="Button834">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>
            <!-- MODAL DE PAGO DIRECTO ( F ) -->
            <a id="modaL_DIRECTO33" name="modaL_DIRECTO" href="#modaL_DIRECTO" role="button" class="btn" data-toggle="modal" style="display: none;"></a>
            <div id="modaL_DIRECTO" class="modal fade">
                <div class="modal-dialog modal-lg" style="width: 85%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" id="Buwtton75" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h3>Pago Directo Cliente
                                        <div class="col-sm-6">
                                            <asp:DropDownList runat="server" ID="cb_cliente_3" ClientIDMode="Static" CssClass="form-control chosen"></asp:DropDownList>
                                        </div>
                            </h3>

                        </div>
                        <div class="modal-body">
                            <div class="row">


                                <div class="col-sm-12">



                                    <asp:Panel runat="server" ID="PANEL_PAGO3" ClientIDMode="Static">
                                        <hr />
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box">
                                                    <div class="form-group">
                                                        <div class="box box-green" style="margin-top: 1%;">
                                                            <div class="box-title">
                                                                <label class="col-sm-2 control-label">
                                                                    <b>Tipo de Pago :</b>
                                                                </label>
                                                                <div class="col-sm-2 controls">
                                                                    <select class="form-control input-sm" id="CB_TIPO_PAGO3" onchange="CAMBIA_TIPO_PAGO_directo()">
                                                                        <option value="-1" selected>-- Seleccione --</option>
                                                                        <option value="efectivo">Deposito</option>
                                                                        <option value="cheque">Cheque</option>
                                                                        <option value="efectivo">Tarjeta</option>
                                                                    </select>
                                                                </div>
                                                                <%--<div class="col-sm-4 controls">
                                                                <input type="checkbox" id="ch_cerrar" />
                                                                Cerrar Documento?
                                                            </div>--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <asp:Panel runat="server" ID="PANEL_MONTO3" ClientIDMode="Static">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box">
                                                                    <div class="form-group">

                                                                        <div class="col-sm-3 controls">
                                                                            <asp:DropDownList runat="server" ID="CB_DEPOSITOS_BANCO3" ClientIDMode="Static" onchange="CAMBIA_TIPO_DEPOSITO()" CssClass="form-control input-sm"></asp:DropDownList>
                                                                        </div>

                                                                        <div class="col-sm-3 controls">
                                                                            <input type="text" class="form-control input-sm" id="T_DESCRIPCION_PAGO3" placeholder="Descripcion..." />
                                                                        </div>
                                                                        <div class="col-sm-2 controls">
                                                                            <input type="text" class="form-control input-sm" id="T_MONTO_PAGO3" placeholder="Monto..." />
                                                                        </div>
                                                                        <div class="col-sm-2 controls">
                                                                            <select class="form-control input-sm" id="CB_TIPO_MONEDA3">
                                                                                <option value="peso" selected>Peso</option>
                                                                                <option value="dolar">Dolar</option>
                                                                            </select>
                                                                        </div>
                                                                        <div class="col-sm-2 controls">
                                                                            <asp:TextBox runat="server" ID="t_fecha_efec3" ClientIDMode="Static" CssClass="form-control input-sm" placeholder="Fecha"></asp:TextBox>
                                                                            <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" runat="server" ID="CalendarExtender17" TargetControlID="t_fecha_efec3"></ajaxToolkit:CalendarExtender>
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </asp:Panel>

                                                    <asp:Panel runat="server" ID="PANEL_BANCO3" ClientIDMode="Static">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box">
                                                                    <div class="form-group">

                                                                        <div class="col-sm-3 controls">
                                                                            <asp:DropDownList runat="server" ID="cb_bancos3" ClientIDMode="Static" CssClass="form-control input-sm"></asp:DropDownList>
                                                                        </div>

                                                                        <div class="col-sm-2 controls">
                                                                            <input type="text" class="form-control input-sm" id="T_NUM_CHEQUE3" placeholder="Nº Cheque..." />
                                                                        </div>
                                                                        <div class="col-sm-2 controls">
                                                                            <input type="text" class="form-control input-sm" id="T_Cuenta3" placeholder="Monto..." />
                                                                        </div>

                                                                        <div class="col-sm-2 controls">
                                                                            <select class="form-control input-sm" runat="server" id="cb_moneda_3">
                                                                                <option value="peso" selected="selected">Peso</option>
                                                                                <option value="dolar">Dolar</option>
                                                                            </select>
                                                                        </div>
                                                                        <div class="col-sm-2 controls">
                                                                            <asp:TextBox runat="server" ID="T_VENCIMIENTO_CHEQUE3" ClientIDMode="Static" CssClass="form-control input-sm" placeholder="Fecha Vencimiento"></asp:TextBox>
                                                                            <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" runat="server" ID="CalendarExtender18" TargetControlID="T_VENCIMIENTO_CHEQUE3"></ajaxToolkit:CalendarExtender>
                                                                        </div>

                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <div class="box">
                                                                                <div class="form-group">
                                                                                    <div class="col-sm-2 controls">
                                                                                        <input type="button" class="btn btn-primary fa-input" value="&#xf0fe; Agregar Cheque" onclick="Agregar_Cheque3()" />
                                                                                    </div>

                                                                                    <div class="col-sm-2 controls">
                                                                                        <input type="button" class="btn btn-danger fa-input" value="&#xf00d; Borrar" onclick="Borrar_Cheques3()" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <div class="box">
                                                                                <div class="form-group">
                                                                                    <div class="col-sm-12 controls">
                                                                                        <div id="tabla_cheques3" name="tabla_cheques3"></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>


                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </asp:Panel>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="box">
                                                                <div class="form-group">
                                                                    <div class="col-sm-3 controls">
                                                                        <input type="button" class="btn btn-primary" value="Registrar Pago" onclick="REGISTRAR_PAGO3()" />

                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" aria-hidden="true" id="Buttoyn83">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>

            <!-- MODAL DE acciones ( F ) -->
            <a id="modal_5" name="modal_5" href="#modal-5" role="button" class="btn" data-toggle="modal"></a>
            <div id="modal-5" class="modal fade">
                <div class="modal-dialog modal-lg" style="width: 90%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <%--<button type="button" id="btn_modal_5_cerrar" class="close" onclick="cerrar_modal5_2();" aria-hidden="true">×</button>--%>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-horizontal">
                                        <h3>Documento</h3>
                                        <div id="acciones_f">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">

                            <div id="div3" class="btn-primary" style="width: 7% !important; float: right; cursor: pointer;" onclick="cerrar_modal5_2();">
                                <h6 style="margin-right: 20%;">CERRAR</h6>
                            </div>

                        </div>

                    </div>
                </div>
            </div>
            <!-- ... -->
            <!-- ... -->
            <!-- ... -->
            <!-- MODAL DE EDICION ( F ) -->
            <a id="moda_2" name="moda_2" href="#modal-2" role="button" class="btn" data-toggle="modal" style="display: none;"></a>
            <div id="modal-2" class="modal fade">
                <div class="modal-dialog modal-lg" style="width: 25%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" id="Button2" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h3 id="H1"><i id="I1"></i></h3>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-horizontal">
                                        <h3>Monto Vencidos</h3>
                                        <div id="vencidos">
                                        </div>



                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" aria-hidden="true" id="Button3">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>


            <a id="moda_3" name="moda_3" href="#modall_3" role="button" class="btn" data-toggle="modal" style="display: none;"></a>
            <div id="modall_3" class="modal fade">
                <div class="modal-dialog modal-lg" style="width: 90%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" id="Button7" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h3 id="H2"><i id="I2"></i></h3>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-horizontal">
                                        <h3>Cerrados por aplicación</h3>
                                        <div id="cerrados" style="overflow-x: auto; overflow-y: auto">
                                        </div>



                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" aria-hidden="true" id="Button8">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>



            <a id="moda_4" href="#modal_4" role="button" class="btn" data-toggle="modal" style="display: none;"></a>
            <div id="modal_4" class="modal fade">
                <div class="modal-dialog modal-lg" style="width: 90%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" id="Button6" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h3 id="H3"><i id="I3"></i></h3>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box">
                                        <div class="box-title">
                                            <h3><i class="fa fa-table"></i>Estimado a pagar</h3>
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
                                                            <div class="form-group">
                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Periodo Desde</label>

                                                                <div class="col-sm-3 col-lg-3 controls">
                                                                    <div class="input-group">

                                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                                        <asp:TextBox ID="txt_desde" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                                        <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" ID="CalendarExtender7" runat="server" TargetControlID="txt_desde" Format="dd/MM/yyyy" />

                                                                    </div>
                                                                </div>
                                                                <label class="col-sm-1 col-lg-1 control-label">Hasta</label>
                                                                <div class="col-sm-3 col-lg-3 controls">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                                        <asp:TextBox ID="txt_hasta" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                                        <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" ID="CalendarExtender8" runat="server" TargetControlID="txt_hasta" Format="dd/MM/yyyy" />

                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-1 col-lg-1 controls">
                                                                    <%--<asp:ImageButton ID="b" ImageUrl="~/img/Ticket_verde.png" runat="server" OnClientClick="CARGA_FECHA();" OnClick="b_Click" />--%>
                                                                    <%--<asp:Button ID="btn_estimados_report" runat="server" OnClientClick="CARGANDO2();" Style="color: white; margin-left: 2%;" Class="btn btn-primary" OnClick="btn_estimados_report_Click" Text="Reporte" />--%>
                                                                    <a class="btn btn-primary fa-input" id="btn_reporte_esti" href="javascript:;" onclick="reporte_estima();">&#xf02d; Reporte</a>
                                                                    <%--<asp:LinkButton runat="server" ID="btn_detalle" OnClientClick="reporte_estima_detalle_cargando();" CssClass="btn btn-success fa-input" Text="&#xf02d; Detalle" OnClick="btn_detalle_Click" />--%>

                                                                    <%-- <asp:Button runat="server" ID="etest" CssClass ="btn btn-primary fa-input" Text="&#xf02d; Reporte"/>--%>
                                                                    <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="carga_2" style="display: none; font-size: 3em;"></i>
                                                                </div>

                                                            </div>



                                                        </div>
                                                    </div>
                                                </div>
                                                <%--                    <a class="btn btn-primary fa-input" id="btn_reporte_total" href="javascript:;" onclick="reporte_estima();">&#xf02d; Reporte</a>
                                                               <a class="btn btn-primary fa-input" id="btn_reporte_detalle" href="javascript:;" onclick="reporte_estima();">&#xf02d; Reporte</a>--%>
                                                <br />
                                                <br />
                                                <%--<asp:Button Visible="false" runat="server" ID="btn_detalle" CssClass="btn btn-primary fa-input" Text="&#xf02d; Detalle" OnClick="btn_detalle_Click" />--%>

                                                <div id="tabla_html2"></div>
                                                <div visible="false" runat="server" id="div_detalle">
                                                    <div class="col-sm-3 controls">
                                                        <asp:RadioButton ID="chk_adm2" Text="&nbsp;&nbsp;Estimado." Style="padding-right: 20px;" Checked="True"
                                                            GroupName="tipo_usuario" runat="server" />

                                                        <asp:RadioButton ID="chk_vend2" Style="padding-right: 20px;" Text="&nbsp;&nbsp;Gestionado"
                                                            GroupName="tipo_usuario" runat="server" />

                                                        <asp:DropDownList runat="server" ID="CB_DIAS_ELEGIDOS" ClientIDMode="Static" CssClass="form-control chosen"></asp:DropDownList>

                                                        <asp:Button runat="server" ID="btn_detalle_2" OnClientClick="reporte_estima_detalle_cargando();" CssClass="btn btn-success fa-input" Text="&#xf02d; Detalle2" OnClick="btn_detalle_Click" />/>
                                                    </div>

                                                    <asp:GridView ID="G_DETALLE_ESTIMADOS_FACTURAS" ClientIDMode="Static" CssClass="table table-advance tablesorter filtrar" OnRowDataBound="G_DETALLE_ESTIMADOS_RowDataBound" runat="server" Visible="true"
                                                        ShowHeaderWhenEmpty="True" Font-Size="12px">
                                                        <HeaderStyle CssClass="test no-sort" />
                                                        <Columns>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            No existen datos.
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                    <asp:GridView ID="G_DETALLE_CHEQUES_CARTERA" ClientIDMode="Static" CssClass="table table-advance tablesorter filtrar" OnRowDataBound="G_DETALLE_CHEQUES_CARTERA_RowDataBound" runat="server" Visible="true"
                                                        ShowHeaderWhenEmpty="True" Font-Size="12px">
                                                        <HeaderStyle CssClass="test no-sort" />
                                                        <Columns>
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

                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" aria-hidden="true" id="Button9">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>




            <a href="#div_unidad" id="div_unidad_" role="button" class="btn" style="visibility: hidden; position: absolute;" data-toggle="modal"></a>

            <div id="div_unidad" class="modal fade">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content" style="height: 700px;">

                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="modal_updatepanel">
                            <ContentTemplate>
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                    <h3 id="myModalLabel2">COMISIÓN</h3>
                                    <div id="titulo_" runat="server"></div>
                                </div>
                                <div class="modal-body" style="height: 70%;">


                                    <div class="row">
                                        <div class="form-horizontal">
                                            <!-- ********************************************************************* -->
                                            <div class="form-group">
                                                <asp:TextBox runat="server" ID="T_ID_LOTE" Visible="false"></asp:TextBox>
                                                <label class="col-sm-2 control-label">
                                                    <b>LOTE</b>
                                                </label>
                                                <div class="col-sm-3 controls">
                                                    <asp:TextBox runat="server" ID="t_lote" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                </div>


                                            </div>

                                            <div class="form-group">
                                                <label class="col-sm-2 control-label">
                                                    <b>Fecha envasado</b>
                                                </label>


                                                <div class="col-sm-3 col-lg-3 controls">
                                                    <div class="input-group">

                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                        <asp:TextBox ID="t_envasado" CssClass="form-control" runat="server" Width="100%" Enabled="false"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" ID="CalendarExtender19" runat="server" TargetControlID="t_envasado" Format="dd/MM/yyyy" />

                                                    </div>
                                                </div>


                                                <label class="col-sm-2 control-label">
                                                    <b>Fecha vencimiento</b>
                                                </label>
                                                <div class="col-sm-3 col-lg-3 controls">
                                                    <div class="input-group">

                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                        <asp:TextBox ID="t_vencimiento" CssClass="form-control" runat="server" Width="100%" Enabled="false"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender FirstDayOfWeek="Monday" ID="CalendarExtender20" runat="server" TargetControlID="t_vencimiento" Format="dd/MM/yyyy" />

                                                    </div>
                                                </div>


                                                <%--         <div class="col-sm-1 col-sm-offset-1 controls">
                                                    <button id="btn_nuevo_lote" class="btn btn-success icon-plus btn-circle" runat="server" onserverclick="btn_nuevo_lote_ServerClick"><i class="fa fa-plus"></i></button>
                                                </div>--%>
                                            </div>

                                            <div class="form-group">
                                                <div class="col-sm-2 col-sm-offset-2 controls">
                                                    <div class="input-group">
                                                        <%--<asp:Button runat="server" ID="B_Guardar" CssClass="btn btn-primary" Text="Crear" OnClick="B_Guardar_Click" Visible="false" />--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" style="overflow: auto; height: 300px;">
                                        <div class="col-sm-12">
                                            <%-- <asp:GridView CssClass="table fill-head table-bordered filtrar" ID="G_LOTES" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" Width="100%" OnRowCommand="G_LOTES_RowCommand"
                                                DataKeyNames="invtid, siteid, batnbr, refnbr, trandate, trantype, lote, envasado, vencimiento">
                                                <HeaderStyle CssClass="test" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Editar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="editar" runat="server" ImageUrl="img/pencil.png" Width="25"
                                                                CommandName="b_editar_lote" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Borrar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="borrar" runat="server" ImageUrl="img/delete.png" Width="25"
                                                                CommandName="b_borrar_lote" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>



                                                    <asp:BoundField DataField="lote" HeaderText="Lote" HeaderStyle-HorizontalAlign="Center" Visible="true">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>


                                                    <asp:BoundField DataField="envasado" HeaderText="Fecha Envasado" HeaderStyle-HorizontalAlign="Center" Visible="true">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>


                                                    <asp:BoundField DataField="vencimiento" HeaderText="Fecha Vencimiento" HeaderStyle-HorizontalAlign="Center" Visible="true">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>

                                                </Columns>
                                                <EmptyDataTemplate>
                                                    No existen datos.
                                                </EmptyDataTemplate>
                                            </asp:GridView>--%>
                                        </div>
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



            <a id="MODAL_CHEQUE" name="modal_cheque_name" href="#MODAL_CHEQUE_div" role="button" class="btn" data-toggle="modal" style="display: none;"></a>
            <div id="MODAL_CHEQUE_div" class="modal fade">
                <div class="modal-dialog modal-lg" style="width: 50%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h3>INGRESAR FECHA DE PAGO AL CHEQUE </h3>
                            <h3><i id="h3_num_che"></i></h3>
                        </div>
                        <asp:TextBox ID="l_num_cheque" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btn_paga_cheque" />
                            </Triggers>
                            <ContentTemplate>

                                <div class="modal-body">
                                    <div class="row">

                                        <div class="col-sm-12">

                                            <div class="col-sm-6">
                                                <div class="form-horizontal">
                                                </div>
                                            </div>

                                            <div class="col-sm-9 col-lg-10 controls">
                                                <%--<input class="form-control" type="text" placeholder="Disabled input here..." disabled="">--%>
                                                <asp:TextBox ID="t_dia_pago" type="date" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>

                                                <span class="help-inline">DD/MM/YYYY</span>
                                            </div>

                                            <div class="col-sm-6 controls">
                                                <asp:DropDownList runat="server" ID="cb_banco_soprodi" ClientIDMode="Static" CssClass="form-control input-sm chosen"></asp:DropDownList>
                                            </div>


                                            <div class="col-sm-6">
                                                <div class="form-horizontal">

                                                    <asp:Button ID="btn_paga_cheque" CssClass="btn btn-success" Text="Guardar" runat="server" OnClick="btn_paga_cheque_Click" />

                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <button class="btn" data-dismiss="modal" aria-hidden="true" id="Button83">Cerrar</button>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>


            <!-- MODAL DE acciones ( F ) -->
            <a id="modal_77" name="modal_77" href="#modal-77" role="button" class="btn" data-toggle="modal"></a>
            <div id="modal-77" class="modal fade">
                <div class="modal-dialog modal-lg" style="width: 90%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <%--<button type="button" id="btn_modal_7_cerrar" class="close" onclick="cerrar_modal5();" aria-hidden="true">×</button>--%>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-horizontal">

                                        <div id="acciones_f2">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">

                            <div id="div2" class="btn-primary" style="width: 7% !important; float: right; cursor: pointer;" onclick="cerrar_modal5();">
                                <h6 style="margin-right: 20%;">CERRAR</h6>
                            </div>

                        </div>

                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:UpdateProgress ID="prgLoadingStatus" runat="server" DynamicLayout="true">
        <ProgressTemplate>
            <div id="overlay" class="gif">
                <div id="modalprogress">
                    <div id="theprogress">
                        <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_gif_22" style="background-size: cover; font-size: 3em;"></i>
                    </div>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>



    <div id="div_trael_scroll" onclick="volver_scroll();">
    </div>

    <script>
        jQuery(function ($) {
            $('#T_MONTO_PAGO2').autoNumeric('init');
        });
    </script>

    <script>

</script>
    <script src="js/NUMERO_SEPARADOR.JS"></script>
</asp:Content>
