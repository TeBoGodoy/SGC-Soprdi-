<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" AutoEventWireup="True" CodeBehind="REPORTE_VEND_CLIE.aspx.cs" Inherits="SoprodiApp.REPORTE_VEND_CLIE" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ OutputCache Location="None" NoStore="true" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">
    <script>


        var calendar;

        var e_rut_cliente;
        var e_codvendedor;
        var e_observacion;
        var e_am_pm;


        $(document).ready(function () {
            $("#div_unidad22").on('hidden.bs.modal', function () {
                alert('aca1');
                creagrilla();
            });
            $("#div_unidad1").on('hidden.bs.modal', function () {
                alert('aca2');
                creagrilla();
            });
        });

        function CARGAR_CALENDARIO(codvendedor) {

            alert('aca');

            alert(codvendedor);

            $("#<%=calendar.ClientID%>").fullCalendar('destroy');

            var f2 = codvendedor;

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
                events: 'CALEND.ASPX?VEND=' + f2,

                eventClick: function (e, jsEvent, view) {
                    CargarEvento(e);
                    //$('#modal_1').click();
                }
            });
            $("#<%=calendar.ClientID%>").fullCalendar('changeView', 'basicWeek');


            //levanta modal
            modal_unidad_4();
        }

        //  ---------------------------------------- LLENAR EVENTO --------------------------------------------- //
        //  **********************************************************************************************************
        function CargarEvento(e) {
            var parameters = new Object();

            parameters.rut = e.rutcliente;
            e_rut_cliente = e.rut_cliente;

            parameters.codvendedor = e.codvendedor;
            e_codvendedor = e.codvendedor;

            parameters.observacion = e.observacion;
            e_observacion = e.observacion;

            parameters.am_pm = e.am_pm;
            e_am_pm = e.am_pm;

            parameters = JSON.stringify(parameters)

            $.ajax({
                type: "POST",
                url: "REPORTE_VEND_CLIE.aspx/CargarEvento",
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
                //$('#PANEL_DATOS_CLIENTE').hide();
            });
        }



        function LlenarEvento(result) {

            $.each(result.d, function () {

                $('#T_NOMBRE_EVENTO').val(this.title);
                $('#T_ID_EVENTO').val(this.id);
                document.getElementById('T_F_INICIO').value = (this.start);
                $('#T_FACTURA').html(this.factura);

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

            });
        }

        var nombre_vendedor_v;
        var nombrecliente_v;

        function agregar_agenda(observa, hora, rutcliente, fecha_agenda, nombre_contacto, codvendedor, nombrecliente, nombre_vendedor) {

            document.getElementById("ContentPlaceHolder_Contenido_t_rutcliente_agenda").value = rutcliente;

            nombre_vendedor_v = nombre_vendedor;
            nombrecliente_v = nombrecliente;
            document.getElementById("ContentPlaceHolder_Contenido_btn_contactos_").click();
        }

        function row_arriba_ordenar_agendar() {
            var elem3 = document.getElementById("row_arriba");
            elem3.style.position = "relative";

            var elem3 = document.getElementById("div_obs_click");
            elem3.style.position = "fixed";

            var elem3 = document.getElementById("div_edit_calen");
            elem3.style.position = "fixed";


            var elem32 = document.getElementById("div_agendar_cal");
            elem32.style.visibility = "visible";
            elem32.style.position = "relative";
            elem32.style.backgroundColor = "gainsboro";

            $('#titulo_modal_cal').html("<h4 style='color: cornflowerblue;font-size: -webkit-xxx-large;'>Vend:" + nombre_vendedor_v + "</h4><br> " +
                "<h4 style='color: cornflowerblue;font-size: -webkit-xxx-large;'>Cliente:  " + nombrecliente_v + "</h4>");

        }


        function row_arriba_ordenar_otro() {
            var elem3 = document.getElementById("row_arriba");
            elem3.style.position = "relative";

            var elem3 = document.getElementById("div_obs_click");
            elem3.style.position = "fixed";

            var elem3 = document.getElementById("div_edit_calen");
            elem3.style.position = "fixed";

            var elem3 = document.getElementById("div_agendar_cal");
            elem3.style.position = "absolute";

        }



        function ver_obs(observa, hora, rutcliente, fecha_agenda, nombre_contacto, codvendedor, nombrecliente) {

            ///ACA MOSTRAR DIV Y PONER NOMBRE      
            var elem2 = document.getElementById("div_obs_click");
            elem2.style.visibility = "visible";
            elem2.style.position = "inherit";
            elem2.style.backgroundColor = "bisque";

            var elem3 = document.getElementById("row_arriba");
            elem3.style.position = "relative";

            var elem4 = document.getElementById("div_edit_calen");
            elem4.style.visibility = "hidden";
            elem4.style.position = "fixed";

            var elem32 = document.getElementById("div_agendar_cal");
            elem32.style.visibility = "hidden";
            elem32.style.position = "fixed";


            try {
                $("#nombre_contacto").html(nombrecliente.toUpperCase() + "<p>- " + nombre_contacto.toUpperCase() + "</p>");
            } catch (e) { }
            ////DIBUJAR BOTONES ELIMINAR Y EDITAR
            var btn_eliminar_y_editar = " " +
                " <a class=\"btn btn-primary\" onclick=\"editar_agenda('" + rutcliente + "','" + codvendedor + "','" + fecha_agenda + "','" + nombre_contacto + "');\"><i class=\"fa fa-edit\"></i>Editar</a> " +
                " <a class=\"btn btn-danger\" onclick=\"CARGANDO_calen();eliminar_agenda('" + rutcliente + "','" + codvendedor + "','" + fecha_agenda + "','" + nombre_contacto + "');\"><i class=\"fa fa-times\"></i>Quitar</a> " +
                "      <a class=\"hidden-inline-xs\" style='cursor:pointer;' onclick=\"cierra_ver_obs()\"><i class=\"fa fa-times\"></i></a>";
            $("#btn_elim_edit").html(btn_eliminar_y_editar);

            //ACA DIBUJAR OBS           
            $("#obs_text").html("<p>" + observa + "</p><br>" + hora);
        }

        function cierra_ver_obs() {

            ///ACA MOSTRAR DIV Y PONER NOMBRE      
            var elem2 = document.getElementById("div_obs_click");
            elem2.style.visibility = "hidden";
            elem2.style.backgroundColor = "bisque";

            var elem3 = document.getElementById("row_arriba");
            elem3.style.position = "fixed";

            var elem4 = document.getElementById("div_edit_calen");
            elem4.style.visibility = "hidden";
            elem4.style.position = "fixed";

        }




        function eliminar_agenda(rutcliente, codvendedor, fecha_agenda, nombre_contacto) {
            var parameters = new Object();
            parameters.rutcliente_ = rutcliente;
            parameters.codvendedor_ = codvendedor;
            parameters.fecha_agenda_ = fecha_agenda;
            parameters.nombre_contacto_ = nombre_contacto;
            parameters = JSON.stringify(parameters)
            $.ajax({
                type: "POST",
                url: "REPORTE_VEND_CLIE.aspx/ELIMINAR_AGENDA",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: agendaQuitada,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al quitar agenda");
                }
            });
        }

        function editar_agenda(rutcliente, codvendedor, fecha_agenda, nombre_contacto) {
            var parameters = new Object();
            parameters.rutcliente_ = rutcliente;
            parameters.codvendedor_ = codvendedor;
            parameters.fecha_agenda_ = fecha_agenda;
            parameters.nombre_contacto_ = nombre_contacto;
            parameters = JSON.stringify(parameters)
            $.ajax({
                type: "POST",
                url: "REPORTE_VEND_CLIE.aspx/EDITAR_AGENDA",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: mostrar_agenda,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al mostrar agendado");
                }
            });
        }


        function mostrar_agenda(result) {

            console.log(result);


            var dia_2 = document.getElementById("ContentPlaceHolder_Contenido_t_dia_edit2");
            var am_pm = document.getElementById("ContentPlaceHolder_Contenido_t_time_edit2");
            var obs = document.getElementById("ContentPlaceHolder_Contenido_t_obs_edit2");

            dia_2.value = result.d.dia;
            am_pm.value = result.d.tiempo;
            obs.value = result.d.obs;


            var elem2 = document.getElementById("div_edit_calen");
            elem2.style.visibility = "visible";
            elem2.style.position = "inherit";

        }



        function agendaQuitada(result) {


            alert('Agenda Quitada');

            document.getElementById("ContentPlaceHolder_Contenido_bn_trae_calendario").click();
        }



        function refresh_calendario() {

            document.getElementById("ContentPlaceHolder_Contenido_bn_trae_calendario").click();
        }



        function agendaEditada(result) {
            alert('Agenda Editada');
            document.getElementById("ContentPlaceHolder_Contenido_bn_trae_calendario").click();
        }

        function cerrar_edit() {
            ///ACA MOSTRAR DIV editar

            var elem2 = document.getElementById("div_obs_click");
            elem2.style.visibility = "visible";
            elem2.style.backgroundColor = "bisque";

            var elem3 = document.getElementById("row_arriba");
            elem3.style.position = "relative";

            var elem2 = document.getElementById("div_edit_calen");
            elem2.style.visibility = "hidden";


            var elem4 = document.getElementById("div_edit_calen");
            elem4.style.visibility = "hidden";


        }


        function cerrar_nueva_ag() {

            alert('aca');


            var elem4 = document.getElementById("ContentPlaceHolder_Contenido_div_nueva_agenda");
            elem4.style.visibility = "hidden";


            var elem2 = document.getElementById("div_obs_click");
            elem2.style.visibility = "visible";
            elem2.style.backgroundColor = "bisque";

            var elem3 = document.getElementById("row_arriba");
            elem3.style.position = "relative";

            var elem2 = document.getElementById("div_edit_calen");
            elem2.style.visibility = "hidden";

        }




        function LoadVendedores(result) {

            //quito los options que pudiera tener previamente el combo

            $("#<%=d_vendedor_.ClientID%>").html("");


            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {

                $("#<%=d_vendedor_.ClientID%>").append($("<option></option>").attr("value", this.cod_vendedor).text(this.nom_vendedor))

            });


            $("#<%=d_vendedor_.ClientID%>").chosen();
            $("#<%=d_vendedor_.ClientID%>").trigger("chosen:updated");
            $("#<%=d_cliente.ClientID%>").chosen();
            $("#<%=d_cliente.ClientID%>").trigger("chosen:updated");


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



        function LoadClientes(result) {

            //quito los options que pudiera tener previamente el combo

            $("#<%=d_cliente.ClientID%>").html("");


            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {

                $("#<%=d_cliente.ClientID%>").append($("<option></option>").attr("value", this.rut_cliente).text(this.nom_cliente))

            });


            $("#<%=d_cliente.ClientID %>").change(function () {
                var arr = $(this).val();
                document.getElementById("<%=l_clientes.ClientID %>").value = arr;
            })

            $("#<%=d_cliente.ClientID%>").chosen();
            $("#<%=d_cliente.ClientID%>").trigger("chosen:updated");

        }



        function superfiltro() {

            $(".filtrar tr:has(td)").each(function () {
                var t = $(this).text().toLowerCase();
                $("<td class='indexColumn'></td>")
                    .hide().text(t).appendTo(this);
            });
            //Agregar el comportamiento al texto (se selecciona por el ID) 
            $("#t_filtro_memoria").keyup(function () {
                var s = $(this).val().toLowerCase().split(" ");
                $(".filtrar tr:hidden").show();
                $.each(s, function () {
                    $(".filtrar tr:visible .indexColumn:not(:contains('"
                        + this + "'))").parent().hide();
                });
            });

        }

        function CARGANDO() {

            var elem = document.getElementById("<%=txt_desde.ClientID%>");
            elem.style.zIndex = "0";
            var elem1 = document.getElementById("<%=txt_hasta.ClientID%>");
            elem1.style.zIndex = "0";

            var elem5 = document.getElementById("<%=div1.ClientID%>");
            elem5.style.visibility = "hidden";
            var elem6 = document.getElementById("<%=div2.ClientID%>");
            elem6.style.visibility = "hidden";

            var elem2 = document.getElementById("<%=btn_informe.ClientID%>");
            elem2.style.visibility = "hidden";
            var elem3 = document.getElementById("<%=cargando_gif.ClientID%>");
            elem3.style.display = "block";

        }


        function cargando_2() {

            var elem3 = document.getElementById("<%=cargando_sinventa.ClientID%>");
            elem3.style.display = "block";

        }

        function CARGANDO_calen() {


            var elem3 = document.getElementById("<%=cargando_calendario.ClientID%>");
            elem3.style.display = "block";
        }
        function cargando_en_modal1() {

            var elem3 = document.getElementById("<%=cargando_modal_1.ClientID%>");
            elem3.style.display = "block";
        }
        function cargando_en_modal2() {

            var elem3 = document.getElementById("<%=cargando_modal_2.ClientID%>");
            elem3.style.display = "block";
        }

        function CARGA_FECHA() {

            var elem2 = document.getElementById("<%=b.ClientID%>");
            elem2.style.display = "none";
            var elem3 = document.getElementById("<%=carga_fecha.ClientID%>");
            elem3.style.display = "block";
        }

        function NO_VENDEDORES() {

            alert("No existe vendedores para periodos o ha ingresado una fecha errónea");
        }


        function NO_GRUPOS() {

            alert("No existe ventas para periodos o ha ingresado una fecha errónea");
        }

        function CLICK_MODAL() {

            document.getElementById("div_prod").click();

        }
        function cierra() {

            var table = document.getElementById('ContentPlaceHolder_Contenido_G_PRODUCTOS');
            var sort = new Tablesort(table);

            // Make some Ajax request to fetch new data and on success:
            sort.refresh();

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


        function SortPrah() {

            setTimeout(function () {


                $("#TABLA_REPORTE55").tablesorter({
                    usNumberFormat: false,
                    sortList: [[1, 0]]
                });



            }, 1000)
        }



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

        function creagrilla() {
            try {
                var fecha1 = $('th:contains("UltimoContacto")').index();

                $("#G_INFORME_TOTAL_VENDEDOR").DataTable({
                    "lengthChange": false,
                    "searching": true,
                    "destroy": true,
                    "stateSave": true,
                    "paging": false,
                    columnDefs: [
                        { type: 'date-uk', targets: [fecha1] }
                    ],
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    }
                });


                $("#G_SIN_VENTAS").DataTable({
                    "lengthChange": false,
                    "searching": true,
                    "destroy": true,
                    "stateSave": true,
                    "paging": false,
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    }
                });

                var es_sin_ve = document.getElementById("<%=es_sin_venta.ClientID %>").value;
                if (es_sin_ve == "True") {

                    document.getElementById("li_SIN_VENTA").click();

                }



                //super_ff();
            } catch (e) {
                console.log(e.message);
                //alert(e.message);
            }

        }


        function cargargrilla_agenda() {
            try {
                var fecha1 = $('th:contains("UltimoContacto")').index();

                $("#G_AGENDA").DataTable({
                    "lengthChange": false,
                    "searching": true,
                    "destroy": true,
                    "stateSave": true,
                    "paging": false,
                    columnDefs: [
                        { type: 'date-uk', targets: [fecha1] }
                    ],
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    }
                });
                //super_ff();
            } catch (e) {
                console.log(e.message);
                //alert(e.message);
            }

        }


        function creagrilla_tiempo() {
            try {

                setTimeout(function () {

                    document.getElementById("ContentPlaceHolder_Contenido_btn_refresh").click();

                }, 180);
            } catch (e) {
                console.log(e.message);

            }

        }




        function modal_unidad_1(nom_vend, nomb_clie, es_sin_venta, rutcliente) {

            $('#titulo_modal').html("<h4 style='color: cornflowerblue;font-size: -webkit-xxx-large;'>Vend:" + nom_vend + "</h4><br> " +
                "<h4 style='color: cornflowerblue;font-size: -webkit-xxx-large;'>Cliente:  " + nomb_clie + "</h4>" + 
                "<a href='javascript: fuera22( \""+ rutcliente +" \", \"22\")'><i class='fa fa-search fa-2x' aria-hidden='true'></i></a>");


            document.getElementById("div_unidad_1").click();
            creagrilla();
            document.getElementById("<%=es_sin_venta.ClientID %>").value = es_sin_venta;
        }

        function modal_unidad_2(nom_vend2, nomb_clie2) {

            $('#titulo_modal2').html("<h4 style='color: cornflowerblue;font-size: -webkit-xxx-large;'>Vend:" + nom_vend2 + "</h4><br> " +
                "<h4 style='color: cornflowerblue;font-size: -webkit-xxx-large;'>Cliente:  " + nomb_clie2 + "</h4>");
            document.getElementById("div_unidad_22").click();
            creagrilla();
        }


        function modal_unidad_3(nom_vend3, nomb_clie3) {

            $('#titulo_modal3').html("<h4 style='color: cornflowerblue;font-size: -webkit-xxx-large;'>Vend:" + nom_vend3 + "</h4><br> " +
                "<h4 style='color: cornflowerblue;font-size: -webkit-xxx-large;'>Cliente:  " + nomb_clie3 + "</h4>");
            document.getElementById("div_unidad_33").click();
            creagrilla();
        }


        function modal_unidad_4() {

            $('#titulo_modal4').html("<h4 style='color: cornflowerblue;font-size: -webkit-xxx-large;'>Calendario</h4><br> ");

            document.getElementById("div_unidad_44").click();
            creagrilla();
        }



        function contactar(var1, var2) {

            $('#titulo_modal').html("<h3 style='color: cornflowerblue;font-size: -webkit-xxx-large;'>Vend:" + nom_vend + "     Cliente:  " + nomb_clie + "</h3>");
            document.getElementById("div_unidad_1").click();

        }

        function update_datatable() {

            creagrilla();
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



        .estado_0_1 {
            border-left-color: #02ff0a !important;
        }

        .estado_0_2 {
            border-left-color: #ffec02 !important;
        }

        .estado_0_3 {
            border-left-color: #ff9a02 !important;
        }

        .estado_0_4 {
            border-left-color: #ff0202 !important;
        }

        .estado_0_max {
            border-left-color: #520400 !important;
        }
    </style>

    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_excel2" />
            <asp:PostBackTrigger ControlID="btn_excel" />
            <asp:AsyncPostBackTrigger ControlID="G_INFORME_TOTAL_VENDEDOR" />
            <asp:AsyncPostBackTrigger ControlID="btn_trae_sin_venta" />

        </Triggers>
        <ContentTemplate>


            <script type="text/javascript">



                Sys.Application.add_load(BindEvents);

                function BindEvents() {

                    superfiltro();


                    $("#<%=d_grupos_usuario.ClientID%>").chosen();
                    $("#<%=d_grupos_usuario.ClientID%>").trigger("chosen:updated");


                    $("#<%=d_grupos_usuario.ClientID%>").change(function () {

                        // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                        //este parametro mapeara con el definido en el web service
                        var arr = $(this).val();
                        document.getElementById("<%=l_grupos.ClientID %>").value = arr;
                        var parameters = new Object();


                        parameters.grupos = document.getElementById("<%=l_grupos.ClientID %>").value;
                        parameters.desde = document.getElementById("<%=txt_desde.ClientID %>").value;
                        parameters.hasta = document.getElementById("<%=txt_hasta.ClientID %>").value;
                        parameters.usuario = document.getElementById("<%=l_usuario_.ClientID %>").value;
                        parameters = JSON.stringify(parameters)

                        $.ajax({
                            type: "POST",
                            url: "REPORTE_VENDEDOR.aspx/VENDEDOR_POR_GRUPO",
                            data: parameters,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: LoadVendedores,
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert("Error al cargar vendedores");
                            }
                        });

                        var parameters = new Object();
                        parameters.vendedor = document.getElementById("<%=l_vendedores.ClientID %>").value;
                        parameters.grupos = document.getElementById("<%=l_grupos.ClientID %>").value;
                        parameters.desde = document.getElementById("<%=txt_desde.ClientID %>").value;
                        parameters.hasta = document.getElementById("<%=txt_hasta.ClientID %>").value;
                        parameters.usuario = document.getElementById("<%=l_usuario_.ClientID %>").value;
                        parameters = JSON.stringify(parameters);

                        $.ajax({
                            type: "POST",
                            url: "REPORTE_VENDEDOR.aspx/CLIENTE_POR_VENDEDOR",
                            data: parameters,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: LoadClientes,
                            error: function (XMLHttpRequest, textStatus, errorThrown) {

                            }
                        });


                        $("#<%=d_vendedor_.ClientID%>").chosen();
                        $("#<%=d_vendedor_.ClientID%>").trigger("chosen:updated");


                        $("#<%=d_cliente.ClientID%>").chosen();
                        $("#<%=d_cliente.ClientID%>").trigger("chosen:updated");


                    });


                    $("#<%=d_vendedor_.ClientID%>").change(function () {

                        // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                        //este parametro mapeara con el definido en el web service

                        var arr = $(this).val();
                        document.getElementById("<%=l_vendedores.ClientID %>").value = arr;

                        var parameters = new Object();

                        parameters.vendedor = document.getElementById("<%=l_vendedores.ClientID %>").value;
                        parameters.grupos = document.getElementById("<%=l_grupos.ClientID %>").value;
                        parameters.desde = document.getElementById("<%=txt_desde.ClientID %>").value;
                        parameters.hasta = document.getElementById("<%=txt_hasta.ClientID %>").value;
                        parameters.usuario = document.getElementById("<%=l_usuario_.ClientID %>").value;
                        parameters = JSON.stringify(parameters);
                        $.ajax({
                            type: "POST",
                            url: "REPORTE_VEND_CLIE.aspx/CLIENTE_POR_VENDEDOR",
                            data: parameters,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: LoadClientes,
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                $("#<%=b.ClientID%>").click();
                            }
                        });

                    });

                    $("#<%=d_cliente.ClientID%>").chosen();
                    $("#<%=d_cliente.ClientID%>").trigger("chosen:updated");

                    $("#<%=d_vendedor_.ClientID%>").chosen();
                    $("#<%=d_vendedor_.ClientID%>").trigger("chosen:updated");


                    $("#<%=d_cliente.ClientID %>").change(function () {

                        var arr = $(this).val();

                        if (arr == null) {
                            $("#<%=b.ClientID%>").click();
                        }

                        document.getElementById("<%=l_clientes.ClientID %>").value = arr;
                    })

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
                    <li class="active">Reporte Vendedor - Cliente</li>
                </ul>
            </div>


            <div id="main-content">
                <!-- BEGIN Main Content -->
                <div class="row">
                    <div class="col-md-12">
                        <div class="box">
                            <div class="box-title">
                                <h3><i class="fa fa-table"></i>Reporte Vendedor - Cliente</h3>
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
                                                <div class="form-group" style="display: none">
                                                    <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Periodo Desde</label>

                                                    <div class="col-sm-3 col-lg-3 controls">
                                                        <div class="input-group">

                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                            <asp:TextBox ID="txt_desde" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_desde" Format="dd/MM/yyyy" />

                                                        </div>
                                                    </div>
                                                    <label class="col-sm-1 col-lg-1 control-label">Hasta</label>
                                                    <div class="col-sm-3 col-lg-3 controls">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                            <asp:TextBox ID="txt_hasta" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender8" runat="server" TargetControlID="txt_hasta" Format="dd/MM/yyyy" />

                                                        </div>
                                                    </div>
                                                    <div class="col-sm-1 col-lg-1 controls">
                                                        <asp:ImageButton ID="b" ImageUrl="~/img/Ticket_verde.png" runat="server" OnClientClick="CARGA_FECHA();" OnClick="b_Click" />

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box">
                                                <div class="form-group">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                                        <Triggers>
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <div runat="server" id="div_sw_permiso" style="display: none;">
                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Grupo</label>
                                                                <div class="col-sm-4 col-lg-4">
                                                                    <div class="controls">
                                                                        <asp:ListBox ID="d_grupos_usuario" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Vendedor</label>
                                                            <div class="col-sm-4 col-lg-4">
                                                                <div class="controls">
                                                                    <asp:ListBox ID="d_vendedor_" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                </div>
                                                            </div>
                                                            <div id="Div3" class="col-sm-1 col-lg-1" runat="server" visible="false">
                                                                <div class="controls">
                                                                    <%--                                                            <asp:ImageButton ID="b" ImageUrl="~/img/Ticket_verde.png" Width="30%" runat="server" OnClick="btn_filtro_vend_Click" />--%>
                                                                </div>
                                                            </div>
                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Cliente</label>
                                                            <div class="col-sm-3 col-lg-3">
                                                                <div class="controls">
                                                                    <asp:ListBox ID="d_cliente" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                </div>


                                                            </div>

                                                            <div class="col-sm-3 col-lg-3 controls">
                                                                <i class="fa fa-circle-o-notch fa-spin" id="carga_fecha" runat="server" style="font-size: 2em; display: none;"></i>
                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;"></label>

                                                            </div>

                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>

                                                </div>

                                            </div>
                                        </div>
                                    </div>

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
                                                        <asp:Button ID="btn_informe" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-primary" OnClick="btn_informe_Click" Text="Reporte" />
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                    </div>
                                    <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_gif" runat="server" style="display: none; font-size: 3em;"></i>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="div1" runat="server" class="row" style="visibility: visible">
                        <div id="div_report" runat="server" class="row" visible="false">
                            <div class="col-md-12">
                                <div class="box">
                                    <div class="box-title">
                                        <h3><i class="fa fa-table"></i>Detalle Vendedor</h3>
                                        <div class="box-tool">
                                            <a data-action="collapse" href="#"></a>

                                        </div>
                                    </div>
                                    <div class="box-content">

                                        <div class="col-md-6">
                                            <div class="box">
                                                <div class="form-group">

                                                    <div runat="server" id="div_totales"></div>
                                                </div>
                                            </div>
                                        </div>


                                        <%--                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="box">
                                                    <div class="box-title">
                                                        <h3><i class="fa fa-bars"></i>TotalesCrédito</h3>
                                                        <div class="box-tool">
                                                            <a data-action="collapse" href="#"><i class="fa fa-chevron-up"></i></a>
                                                        </div>
                                                    </div>
                                                    <div class="box-content" style="display: block;">
                                                        <div class="form-horizontal">
                                                            <div class="form-group">
                                                                   <div runat="server" id="div_totales"></div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>--%>



                                        <div class="btn-toolbar pull-right">

                                            <div class="btn-group">
                                                <asp:LinkButton ID="btn_excel" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Exportar a Excel" runat="server" OnClick="btn_excel_Click"></asp:LinkButton>

                                                <asp:ImageButton ID="btn_agenda" ImageUrl="~/img/calendar_empty.png" Width="50" runat="server" OnClick="b_agenda__Click" />


                                            </div>
                                        </div>

                                        <div class="clearfix"></div>
                                        <div class="table-responsive" style="border: 0">

                                            <div class="row">
                                                <div class="col-md-12">

                                                    <div class="tabbable">
                                                        <ul id="myTab1" class="nav nav-tabs">
                                                            <li class="active"><a href="#home1" data-toggle="tab"><i class="fa fa-home"></i>VENTA</a></li>
                                                            <li class=""><a id="li_SIN_VENTA" href="#profile1" data-toggle="tab"><i class="fa fa-user"></i>CLIENTES SIN VENTA</a></li>
                                                        </ul>

                                                        <div id="myTabContent1" class="tab-content">
                                                            <div class="tab-pane fade active in" id="home1">
                                                                <div class="table-primary table-responsive" style="overflow: auto;">

                                                                    <asp:LinkButton ID="btn_refresh" class="btn btn-circle show-tooltip fa fa-refresh" Visible="true" Style="margin-left: 5px;"
                                                                        title="Ordenamiento..." runat="server" OnClick="btn_refresh_Click1"></asp:LinkButton>


                                                                    <asp:GridView ID="G_INFORME_TOTAL_VENDEDOR" AutoGenerateColumns="false" ClientIDMode="Static" CssClass="table table-bordered table-advance filtrar"
                                                                        OnRowDataBound="G_INFORME_TOTAL_VENDEDOR_RowDataBound" runat="server" Visible="false" OnRowCommand="G_INFORME_TOTAL_VENDEDOR_RowCommand"
                                                                        ShowHeaderWhenEmpty="True" DataKeyNames="rutcliente, Meses Diferencia, Nombre Vendedor, Nombre Cliente,tipo_credi,codvendedor, ultimo_contacto, Dias Diferencia, ultima_fecha_factu, Monto Ultima Factur., PROM Meses Venta 3M" Font-Size="12px">
                                                                        <HeaderStyle CssClass="test no-sort" />
                                                                        <Columns>

                                                                            <asp:BoundField DataField="Nombre Vendedor" HeaderText="Nombre Vendedor">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                            <asp:BoundField DataField="Nombre Cliente" HeaderText="Nombre Cliente">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                            <asp:BoundField DataField="Cantidad Meses Venta 12M" HeaderText="12 Meses con venta">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                            <asp:BoundField DataField="PROM Meses Venta 12M" HeaderText="12 Meses Promedio">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                            <asp:BoundField DataField="Periodo Ultima Factur." HeaderText="Periodo Ultima Factur.">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                            <asp:BoundField DataField="Monto Ultima Factur." HeaderText="Monto Ultima Factur.">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                            <asp:BoundField DataField="Meses Diferencia" HeaderText="Meses Diferencia">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                            <asp:BoundField DataField="Dias Diferencia" HeaderText="Días Diferencia">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                            <asp:TemplateField HeaderText="Contacto">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="btn_img" runat="server" ImageUrl="img/users.png" Width="25"
                                                                                        CommandName="Editar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="UltimoContacto">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" Text='<%#Eval("ultimo_contacto")%>'></asp:Label>
                                                                                    <asp:ImageButton ID="btn_img2" Visible='<%# es_visible( Eval("ultimo_contacto").ToString() ) %>' runat="server" ImageUrl="img/help.png" Width="25"
                                                                                        CommandName="Histo" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:BoundField DataField="lc" HeaderText="Linea Credito">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                            <asp:BoundField DataField="disponible" HeaderText="Disponible LC">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                            <asp:BoundField DataField="tipo_credi" HeaderText="TipoCredito">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                            <asp:TemplateField HeaderText="Agenda">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton runat="server" Visible='<%# es_visible_agenda( Eval("tiene_agenda").ToString() ) %>' ImageUrl="img/calendar_empty.png" Width="25"
                                                                                        CommandName="agenda" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:BoundField DataField="COMUNA_STUFF" HeaderText="Comuna">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                        </Columns>
                                                                        <EmptyDataTemplate>
                                                                            No existen datos.
                                                                        </EmptyDataTemplate>
                                                                    </asp:GridView>



                                                                    <div class="box-content">
                                                                        <div class="row">
                                                                            <div class="col-md-3">
                                                                                <h4>Estados Diferencia meses</h4>
                                                                                <ul class="list-unstyled">
                                                                                    <li><span class="label label-important" style="background-color: #02ff0a;">&nbsp;&nbsp;&nbsp;&nbsp;</span> 0 Este mes</li>
                                                                                    <li><span class="label label-important" style="background-color: #ffec02;">&nbsp;&nbsp;&nbsp;&nbsp;</span> 1 </li>
                                                                                    <li><span class="label label-important" style="background-color: #ff9a02;">&nbsp;&nbsp;&nbsp;&nbsp;</span> 2</li>
                                                                                    <li><span class="label label-important" style="background-color: #ff0202;">&nbsp;&nbsp;&nbsp;&nbsp;</span> 3</li>
                                                                                    <li><span class="label label-important" style="background-color: #520400;">&nbsp;&nbsp;&nbsp;&nbsp;</span> hace 4 o +</li>

                                                                                </ul>
                                                                            </div>
                                                                            <div class="col-md-1"></div>
                                                                            <div class="col-md-3">
                                                                                <h4>Valor de última Facturación</h4>
                                                                                <ul class="list-unstyled">
                                                                                    <li><span class="label label-important" style="background-color: #76ca42;">&nbsp;&nbsp;&nbsp;&nbsp;</span> Supera promedio 3 últimos meses </li>
                                                                                    <li><span class="label label-important" style="background-color: #ca4242;">&nbsp;&nbsp;&nbsp;&nbsp;</span> No supera </li>
                                                                                </ul>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>

                                                            <%--SEGUNDO TAB'S--%>
                                                            <div class="tab-pane fade" id="profile1">
                                                                <div class="table-primary table-responsive" style="overflow: auto;">

                                                                    <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_sinventa" runat="server" style="display: none; font-size: 3em;"></i>

                                                                    <asp:LinkButton ID="btn_trae_sin_venta" class="btn btn-circle show-tooltip fa fa-plus" Visible="true" OnClientClick="cargando_2();" Style="margin-left: 5px; background-color: #8cff94 !important;" title="Traer sin venta" runat="server" OnClick="btn_trae_sin_venta_Click"></asp:LinkButton>


                                                                    <asp:GridView ID="G_SIN_VENTAS" AutoGenerateColumns="false" ClientIDMode="Static" CssClass="table table-bordered table-advance filtrar" OnRowCommand="G_SIN_VENTAS_RowCommand" OnRowDataBound="G_SIN_VENTAS_RowDataBound" runat="server" Visible="true"
                                                                        ShowHeaderWhenEmpty="True" DataKeyNames="name, rutcliente, slsperid, vendedor" Font-Size="12px">
                                                                        <HeaderStyle CssClass="test no-sort" />
                                                                        <Columns>

                                                                            <asp:BoundField DataField="name" HeaderText="Nombre Cliente">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                            <asp:BoundField DataField="addr1" HeaderText="Dirección">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                            <asp:BoundField DataField="billcity" HeaderText="Ciudad">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                            <asp:BoundField DataField="billphone" HeaderText="Teléfono">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                            <asp:BoundField DataField="crlmt" HeaderText="Linea Credito">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                            <asp:BoundField DataField="vendedor" HeaderText="VendAnterior">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>


                                                                            <asp:TemplateField HeaderText="Contacto">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="btn_img" runat="server" ImageUrl="img/users.png" Width="25"
                                                                                        CommandName="Editar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>


                                                                            <asp:TemplateField HeaderText="UltimoContacto">
                                                                                <ItemTemplate>
                                                                                    <asp:Label runat="server" Text='<%#Eval("ultimo_contacto")%>'></asp:Label>
                                                                                    <asp:ImageButton ID="btn_img2" Visible='<%# es_visible( Eval("ultimo_contacto").ToString() ) %>' runat="server" ImageUrl="img/help.png" Width="25"
                                                                                        CommandName="Histo" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>


                                                                            <asp:TemplateField HeaderText="Agenda">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton runat="server" Visible='<%# es_visible_agenda( Eval("tiene_agenda").ToString() ) %>' ImageUrl="img/calendar_empty.png" Width="25"
                                                                                        CommandName="agenda" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>



                                                                            <asp:BoundField DataField="COMUNA_STUFF" HeaderText="Comuna">
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="text-center" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>

                                                                        </Columns>
                                                                        <EmptyDataTemplate>
                                                                            No existen datos.
                                                                        </EmptyDataTemplate>
                                                                    </asp:GridView>

                                                                </div>

                                                            </div>
                                                        </div>

                                                        <%--cierra tab's--%>
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

                                                        <div class="form-group" id="DivMainContent" style="overflow-x: auto;">


                                                            <asp:GridView ID="G_PRODUCTOS" AutoGenerateColumns="true" CssClass="table table-bordered filtrar" OnRowCommand="G_PRODUCTOS_RowCommand" OnRowDataBound="G_PRODUCTOS_RowDataBound" runat="server" Visible="true"
                                                                ShowHeaderWhenEmpty="True" Font-Size="12px" DataKeyNames="vendedor, nombrecliente, codvendedor, rutcliente">
                                                                <HeaderStyle CssClass="test no-sort" />

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="#" HeaderStyle-Wrap="false" ItemStyle-Width="4px" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="5px" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Productos" HeaderStyle-Wrap="false" ItemStyle-Width="4px" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <%-- <asp:ImageButton runat="server" ID="b_edit" ImageUrl="img/icono_carro.png" Width="17"
                                                                                CommandName="Producto" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>

                                                                            <button class="btn" style="background-color: transparent;"><i class="fa fa-shopping-cart"></i></button>

                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="5px" />
                                                                        <ItemStyle HorizontalAlign="Left" />
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
                                        <asp:TemplateField HeaderText="#" HeaderStyle-Wrap="false" ItemStyle-Width="4px" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <HeaderStyle Width="5px" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No existen datos.
                                    </EmptyDataTemplate>
                                </asp:GridView>

                            </div>
                            <div class="modal-footer">
                                <button class="btn" data-dismiss="modal" onclick="cierra();" aria-hidden="true">Cerrar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:TextBox ID="l_usuario_" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_grupos" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_vendedores" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_clientes" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_productos" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_periodos" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>

    <asp:TextBox ID="es_sin_venta" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>

    <%--MODAL PARA VER AAGENDAAA CALENDARIO--%>

    <a href="#div_unidad44" id="div_unidad_44" role="button" class="btn" style="visibility: hidden; position: absolute;" data-toggle="modal"></a>

    <div id="div_unidad44" class="modal fade">
        <div class="modal-dialog modal-lg" style="width: 93%;">
            <div class="modal-content" style="height: auto;">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>


                    <div id="titulo_modal4" style="text-align: center"></div>

                </div>

                <asp:UpdatePanel runat="server" ID="UpdatePanel44" UpdateMode="Conditional">

                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="bn_trae_calendario" />
                        <asp:AsyncPostBackTrigger ControlID="btn_cance_edit2" />
                        <asp:AsyncPostBackTrigger ControlID="btn_edit_2" />
                        <asp:AsyncPostBackTrigger ControlID="btn_enviar" />

                        <asp:AsyncPostBackTrigger ControlID="btn_cargar" />
                        <asp:AsyncPostBackTrigger ControlID="bn_agendar" />

                        <asp:AsyncPostBackTrigger ControlID="bt_cancelar_nue_age" />
                    </Triggers>
                    <ContentTemplate>
                        <div runat="server" id="R_Excel_1" style="display: none">
                        </div>


                        <div class="btn-toolbar pull-right">

                            <div class="btn-group">
                                <asp:LinkButton ID="btn_excel_calen" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Exportar a Excel" runat="server" OnClick="btn_excel_calen_Click"></asp:LinkButton>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-12">
                                <div class="box">
                                    <div class="form-group">
                                        <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Periodo Desde</label>
                                        <div class="col-sm-3 col-lg-3 controls">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txt_desde1" type="date" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                <%--<ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txt_desde1" Format="dd/MM/yyyy" />--%>
                                            </div>
                                        </div>
                                        <label class="col-sm-1 col-lg-1 control-label">Hasta</label>
                                        <div class="col-sm-3 col-lg-3 controls">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                <asp:TextBox ID="txt_hasta1" type="date" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                <%--<ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txt_hasta1" Format="dd/MM/yyyy" />--%>
                                            </div>
                                        </div>
                                        <div class="col-sm-1 col-lg-1 controls">
                                            <asp:Button ID="btn_cargar" runat="server" OnClientClick="CARGANDO_calen();document.getElementById('ContentPlaceHolder_Contenido_bn_trae_calendario').click();" Style="color: white;" Class="btn btn-success" Text="Cargar" />
                                            <%--<asp:ImageButton ID="btn_refresh1" ImageUrl="~/img/Ticket_verde.png" runat="server" OnClientClick="CARGA_FECHA();" OnClick="btn_refresh1_Click" />--%>
                                            <i class="fa fa-circle-o-notch fa-spin" id="I4" runat="server" style="font-size: 2em; display: none;"></i>
                                        </div>
                                    </div>
                                </div>


                                <div class="btn-toolbar pull-right" style="visibility: hidden; position: absolute;">
                                    <asp:LinkButton ID="bn_agendar" class="btn btn-circle btn-xlarge show-tooltip fa fa-plus" Visible="true" OnClientClick="cargando_2();" Style="margin-left: 5px; background-color: #8cff94 !important;" title="Traer sin venta" runat="server"
                                        OnClick="bn_agendar_2_Click"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <hr>

                        <div class="row" runat="server" id="div_nueva_agenda" visible="false">
                            <div class="col-md-12">
                                <div class="box">
                                    <div class="form-group">
                                        acaacacacacacaccacacac

                                        <asp:LinkButton runat="server" ID="bt_cancelar_nue_age" CssClass="btn" Text="Cancelar" OnClick="bt_cancelar_nue_age_Click" />

                                    </div>
                                </div>
                            </div>
                        </div>



                        <div class="col-md-9">
                        </div>


                        <div class="col-md-3" style="display: block; z-index: 5;">
                            <asp:Label runat="server" Style="color: red;" ID="lb_mensaj"></asp:Label>
                            <asp:TextBox runat="server" ID="tx_correo_calen" Style="width: 70%;"></asp:TextBox>
                            <i class="fa fa-send"></i>
                            <asp:Button ID="btn_enviar" runat="server" OnClientClick="CARGANDO_calen();" Style="color: white;" Class="btn btn-primary" OnClick="btn_enviar_Click" Text="Enviar" />
                            <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_calendario" runat="server" style="display: none; font-size: 3em;"></i>
                        </div>
                        <br>

                        <asp:Button ID="btn_contactos_" runat="server" OnClientClick="CARGANDO_calen();" Style="color: white; visibility: hidden; position: absolute;" Class="btn btn-warning" OnClick="btn_contactos__Click" Text="test" />
                        <asp:TextBox ID="t_rutcliente_agenda" type="text" Style="visibility: hidden; position: absolute;" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>

                        <div class="modal-body" style="height: 70%;">


                            <div class="table-responsive" style="border: 0">


                                <%--OBSERVACION DE LA AGENDA   (CLICK EN HORA)--%>
                                <div class="row" id="row_arriba" style="position: fixed;">
                                    <div class="col-md-3">
                                    </div>


                                    <div class="col-md-6" id="div_obs_click" style="background-color: bisque; visibility: hidden;">
                                        <div class="box">
                                            <div class="box-title" style="background-color: #082982;">
                                                <h3><i class="fa fa-user"></i>
                                                    <div id="nombre_contacto"></div>
                                                </h3>
                                                <div class="box-tool">

                                                    <div id="btn_elim_edit"></div>
                                                    <%--                                 <a class="btn btn-success" href="#"><i class="fa fa-plus"></i>Add</a>
                                                    <a class="btn btn-danger" href="#"><i class="fa fa-times"></i></a>--%>
                                                </div>
                                            </div>
                                            <div class="box-content">
                                                <div id="obs_text"></div>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="div_edit_calen" style="visibility: hidden;">
                                        <div class="col-md-12">
                                            <div class="box">
                                                <div class="box-content">
                                                    <div class="row">
                                                        <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Dia Agenda</label>
                                                        <div class="col-sm-3 col-lg-3 controls">
                                                            <div class="input-group">
                                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                <asp:TextBox ID="t_dia_edit2" type="date" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Hora (00:00)</label>
                                                        <div class="col-sm-3 col-lg-3 controls">
                                                            <div class="input-group">
                                                                <a class="input-group-addon" href="#"><i class="fa fa-clock-o"></i></a>
                                                                <asp:TextBox ID="t_time_edit2" type="time" CssClass="form-control timepicker-default" runat="server" Width="100%"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">

                                                        <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Observación</label>
                                                        <div class="col-sm-9 col-lg-9 controls">
                                                            <div class="col-sm-9 col-lg-10 controls">
                                                                <textarea name="t_obs_edit22" runat="server" id="t_obs_edit2" rows="3" class="form-control"></textarea>
                                                            </div>
                                                        </div>
                                                        <div class="form-group last">
                                                            <div class="col-sm-9 col-lg-10">
                                                                <asp:LinkButton runat="server" ID="btn_edit_2" CssClass="btn btn-warning" Text="Editar Agenda" OnClientClick="CARGANDO_calen()" OnClick="btn_edit_2_Click" />
                                                                <asp:LinkButton runat="server" ID="btn_cance_edit2" CssClass="btn" Text="Cancelar" OnClientClick="cerrar_edit()" />
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                    <%--MOSTRAR CONTACTOS DEL CLIENTE--%>
                                    <div id="div_agendar_cal" style="visibility: hidden;">

                                        <%--   <a class="hidden-inline-xs" style='cursor:pointer;' onclick="cierra_ver_obs()"><i class="fa fa-times"></i></a>--%>

                                        <h4 id="titulo_modal_cal"></h4>

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
                                                                        <button id="btn_nuevo_modal" class="btn btn-success icon-plus btn-circle" runat="server" onserverclick="btn_nuevo_contacto_modal_ServerClick"><i class="fa fa-plus"></i></button>
                                                                        <asp:Button runat="server" ID="B_Guardar_modal" CssClass="btn btn-success" Text="Guardar" OnClick="B_Guardar_modal_Click" Visible="false" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="box">
                                                                    <div class="form-group" style="display: block">
                                                                        <!-- *************************                
                                                                                NOMBRE
                                                                                CARGO
                                                                                NÚMERO
                                                                                CORREO
                                                                                DIRECCIÓN  ******************************************** -->
                                                                        <div class="form-group">
                                                                            <div class="col-sm-3 col-lg-3 controls">
                                                                                <div class="input-group">
                                                                                    <label class="col-sm-2 control-label">
                                                                                        <b>Nombre</b>
                                                                                    </label>
                                                                                    <asp:TextBox runat="server" ID="t_nombre_modal" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-sm-3 col-lg-3 controls">
                                                                                <div class="input-group">
                                                                                    <label class="col-sm-2 control-label">
                                                                                        <b>Cargo</b>
                                                                                    </label>
                                                                                    <asp:TextBox runat="server" ID="t_cargo_modal" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-sm-3 col-lg-3 controls">
                                                                                <div class="input-group">
                                                                                    <label class="col-sm-2 control-label">
                                                                                        <b>Número</b>
                                                                                    </label>
                                                                                    <asp:TextBox runat="server" ID="t_num_modal" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-sm-3 col-lg-3 controls">
                                                                                <div class="input-group">
                                                                                    <label class="col-sm-2 control-label">
                                                                                        <b>Correo</b>
                                                                                    </label>
                                                                                    <asp:TextBox runat="server" ID="t_correo_modal" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-sm-6 col-lg-6 controls">
                                                                                <div class="input-group">
                                                                                    <label class="col-sm-2 control-label">
                                                                                        <b>Comuna</b>
                                                                                    </label>
                                                                                    <asp:TextBox runat="server" ID="t_comuna_modal" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                                                </div>
                                                                            </div>

                                                                            <div class="col-sm-3 col-lg-3 controls">
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
                                                                        <asp:GridView ID="G_CONTACTOS_CAL" AutoGenerateColumns="false" CssClass="table table-bordered filtrar" OnRowDataBound="G_CONTACTOS_CAL_RowDataBound" runat="server" Visible="true"
                                                                            OnRowCommand="G_CONTACTOS_CAL_RowCommand" BorderStyle="Solid" DataKeyNames="rutcliente, nombre_contacto, CORREO" ShowHeaderWhenEmpty="True" Font-Size="12px">
                                                                            <HeaderStyle CssClass="test no-sort" />
                                                                            <Columns>


                                                                                <asp:TemplateField HeaderText="">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkAccept" runat="server" EnableViewState="true" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="QUITAR">
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="btn_quitar" runat="server" ImageUrl="img/delete.png" Width="25"
                                                                                            CommandName="Eliminar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClientClick='<%# confirmDelete2( Eval("nombre_contacto").ToString() ) %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="EDITAR">
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton runat="server" ImageUrl="img/pencil.png" Width="25"
                                                                                            CommandName="Editar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:BoundField DataField="nombre_contacto" HeaderText="NOMBRE">
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="cargo" HeaderText="CARGO">
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="NUMERO" HeaderText="NÚMERO">
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="CORREO" HeaderText="CORREO">
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="DIRECCION" HeaderText="COMUNA">
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                </asp:BoundField>

                                                                                <asp:TemplateField HeaderText="ENVIAR CORREO">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton runat="server" ImageUrl="img/email.png" Width="25"
                                                                                            CommandName="correo" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="AGENDAR">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton runat="server" ImageUrl="img/calendar_empty.png" Width="25"
                                                                                            CommandName="agendar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                            </Columns>
                                                                            <EmptyDataTemplate>
                                                                                No existen datos.
                                                                            </EmptyDataTemplate>
                                                                        </asp:GridView>
                                                                        <asp:Button runat="server" ID="btn_masivo_modal" CssClass="btn btn-primary" Text="Correos" OnClick="btn_masivo_correo_Click" Visible="true" />

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="I6" runat="server" style="display: none; font-size: 3em;"></i>

                                                        <div class="row>">
                                                            <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="I7" runat="server" style="display: none; font-size: 3em;"></i>

                                                        </div>
                                                        <br />

                                                        <div class="row" runat="server" id="enviar_correo_modal" visible="false">
                                                            <hr />
                                                            <div class="col-md-12">
                                                                <div class="box box-blue">
                                                                    <div class="box-title">
                                                                        <h3><i class="fa fa-bars"></i>Enviar Correo</h3>
                                                                        <div class="box-tool">
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-content">

                                                                        <div class="form-group">
                                                                            <label for="textfield5" class="col-sm-3 col-lg-2 control-label">Destinatarios</label>
                                                                            <div class="col-sm-9 col-lg-10 controls">
                                                                                <asp:TextBox runat="server" ID="t_destino_modal" CssClass="form-control" Enabled="true"></asp:TextBox>
                                                                            </div>
                                                                        </div>

                                                                        <%--espacio en blanco--%>
                                                                        <div class="row">
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <label for="textfield5" class="col-sm-3 col-lg-2 control-label">Cc</label>
                                                                            <div class="col-sm-9 col-lg-10 controls">
                                                                                <asp:TextBox runat="server" ID="t_cc_modal" CssClass="form-control" Enabled="true"></asp:TextBox>
                                                                            </div>
                                                                        </div>

                                                                        <%--espacio en blanco--%>
                                                                        <div class="row">
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <label for="password5" class="col-sm-3 col-lg-2 control-label">Asunto</label>
                                                                            <div class="col-sm-9 col-lg-10 controls">
                                                                                <asp:TextBox runat="server" ID="t_asunto_modal" CssClass="form-control" Enabled="true"></asp:TextBox>
                                                                            </div>
                                                                        </div>

                                                                        <%--espacio en blanco--%>
                                                                        <div class="row">
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <label for="textarea5" class="col-sm-3 col-lg-2 control-label">Contenido</label>
                                                                            <div class="col-sm-9 col-lg-10 controls">
                                                                                <textarea name="textarea5" runat="server" style="white-space: pre-wrap;" id="t_contenido_modal" rows="5" class="form-control"></textarea>
                                                                            </div>
                                                                        </div>

                                                                        <%--espacio en blanco--%>
                                                                        <div class="row">
                                                                        </div>

                                                                        <div class="form-group last">
                                                                            <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2">
                                                                                <asp:Button runat="server" ID="btn_enviar_modal" CssClass="btn btn-success" OnClientClick="cargando_en_modal1();" Text="Enviar" OnClick="btn_enviar_modal_Click" />
                                                                                <asp:Button runat="server" ID="btn_cancelar_modal1" CssClass="btn" Text="Cancelar" OnClick="btn_cancelar_modal_Click" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" runat="server" id="div_agendar_modal" visible="false">
                                                            <hr />
                                                            <div class="col-md-12">
                                                                <div class="box box-blue">
                                                                    <div class="box-title">
                                                                        <h3><i class="fa fa-bars"></i>Agendar</h3>
                                                                        <div class="box-tool">
                                                                        </div>
                                                                    </div>
                                                                    <div class="box-content">
                                                                        <div class="row">
                                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Dia Agenda</label>

                                                                            <div class="col-sm-3 col-lg-3 controls">
                                                                                <div class="input-group">

                                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                                    <asp:TextBox ID="t_dia_modal" type="date" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>


                                                                                </div>
                                                                            </div>
                                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Hora (00:00)</label>
                                                                            <div class="col-sm-3 col-lg-3 controls">

                                                                                <div class="input-group">
                                                                                    <a class="input-group-addon" href="#"><i class="fa fa-clock-o"></i></a>

                                                                                    <asp:TextBox ID="t_hora_modal" type="time" CssClass="form-control timepicker-default" runat="server" Width="100%"></asp:TextBox>


                                                                                </div>

                                                                            </div>
                                                                        </div>
                                                                        <hr />
                                                                        <div class="row">

                                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Observación</label>
                                                                            <div class="col-sm-9 col-lg-9 controls">

                                                                                <div class="col-sm-9 col-lg-10 controls">
                                                                                    <textarea name="textarea5" runat="server" id="t_obs_modal" rows="3" class="form-control"></textarea>
                                                                                </div>

                                                                            </div>

                                                                            <div class="form-group last">
                                                                                <div class="col-sm-9 col-lg-10">
                                                                                    <asp:Button runat="server" ID="btn_agenda_modal" CssClass="btn btn-success" OnClientClick="cargando_en_modal1();" Text="Agendar" OnClick="btn_agendar_modal_Click" />
                                                                                    <asp:Button runat="server" ID="btn_cancela_modal" CssClass="btn" Text="Cancelar" OnClick="btn_cancelar_modal_Click" />
                                                                                </div>
                                                                            </div>


                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="modal-footer">
                                                            <button class="btn" aria-hidden="true" onclick="cierra_ver_obs();  document.getElementById('div_agendar_cal').style.visibility = 'hidden';">Cerrar</button>
                                                        </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>


                                    </div>




                                </div>




                            </div>
                            <asp:LinkButton runat="server" ID="bn_trae_calendario" OnClick="bn_trae_calendario_Click"></asp:LinkButton>


                            <%--ACA VA TABLA CALENDARIO--%>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box">

                                        <div class="form-group" style="display: block">

                                            <div style="font-size: 10px;">
                                                <asp:Panel runat="server" ID="calendar" Style="width: 100%"></asp:Panel>
                                                <div runat="server" id="agenda_div"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <br />

                            <div class="row>">
                                <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="I5" runat="server" style="display: none; font-size: 3em;"></i>
                            </div>
                            <br />
                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" aria-hidden="true" onclick="creagrilla_tiempo();">Cerrar</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>

    </div>

    </div>




    <%--MODAL PARA VER AGENDA--%>

    <a href="#div_unidad33" id="div_unidad_33" role="button" class="btn" style="visibility: hidden; position: absolute;" data-toggle="modal"></a>

    <div id="div_unidad33" class="modal fade">
        <div class="modal-dialog modal-lg" style="width: 1000px;">
            <div class="modal-content" style="height: auto;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <div id="titulo_modal3"></div>
                </div>
                <div class="modal-body" style="height: 70%;">
                    <asp:UpdatePanel runat="server" ID="panel_agenda" UpdateMode="Conditional">
                        <Triggers>
                        </Triggers>
                        <ContentTemplate>
                            <div class="table-responsive" style="border: 0">


                                <div class="row" id="div_editar_agenda" runat="server" visible="false">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="box-content">
                                                <div class="row">
                                                    <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Dia Agenda</label>
                                                    <div class="col-sm-3 col-lg-3 controls">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <asp:TextBox ID="t_dia_edit" type="date" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Hora (00:00)</label>
                                                    <div class="col-sm-3 col-lg-3 controls">
                                                        <div class="input-group">
                                                            <a class="input-group-addon" href="#"><i class="fa fa-clock-o"></i></a>
                                                            <asp:TextBox ID="t_time_edit" type="time" CssClass="form-control timepicker-default" runat="server" Width="100%"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <hr />
                                                <div class="row">

                                                    <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Observación</label>
                                                    <div class="col-sm-9 col-lg-9 controls">
                                                        <div class="col-sm-9 col-lg-10 controls">
                                                            <textarea name="t_obs_e" runat="server" id="t_obs_edit" rows="3" class="form-control"></textarea>
                                                        </div>
                                                    </div>
                                                    <div class="form-group last">
                                                        <div class="col-sm-9 col-lg-10">
                                                            <asp:Button runat="server" ID="btn_guarda_editar" CssClass="btn btn-warning" OnClientClick="cargando_en_modal1();" Text="Editar Agenda" OnClick="btn_guarda_editar_Click" />
                                                            <asp:Button runat="server" ID="btn_cancelar_editar" CssClass="btn" Text="Cancelar" OnClick="btn_cancelar_editar_Click" />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="I2" runat="server" style="display: none; font-size: 3em;"></i>

                                            <div class="form-group" style="display: block">
                                                <asp:GridView ID="G_AGENDA" AutoGenerateColumns="false" CssClass="table table-bordered filtrar" OnRowDataBound="G_AGENDA_RowDataBound" runat="server" Visible="true"
                                                    OnRowCommand="G_AGENDA_RowCommand" DataKeyNames="nombre_contacto, fecha_agenda, dias diferencia, rutcliente, codvendedor" ShowHeaderWhenEmpty="True" Font-Size="12px">
                                                    <HeaderStyle CssClass="test no-sort" />
                                                    <Columns>


                                                        <asp:TemplateField HeaderText="QUITAR">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ImageUrl="img/delete.png" Width="25"
                                                                    CommandName="Eliminar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClientClick='<%# confimar_delete(Eval("nombre_contacto").ToString() ) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="EDITAR">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ImageUrl="img/pencil.png" Width="25"
                                                                    CommandName="Editar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:BoundField DataField="nombre_contacto" HeaderText="CONTACTO">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Left" />

                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="fecha_agenda" HeaderText="FECHA">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="am_pm" HeaderText="HORA ESTIMADA">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="Dias DIFERENCIA" HeaderText="DIAS QUE FALTAN">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="observacion" HeaderText="OBSERVACION">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>


                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        No existen datos.
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br />

                                <div class="row" runat="server" id="Div4" visible="false">
                                    <hr />
                                    <div class="col-md-12">
                                        <div class="box box-blue">
                                            <div class="box-title">
                                                <h3><i class="fa fa-bars"></i>Contenido Correo</h3>
                                                <div class="box-tool">
                                                </div>
                                            </div>
                                            <div class="box-content">

                                                <div runat="server" id="div5"></div>

                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="row>">
                                    <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="I3" runat="server" style="display: none; font-size: 3em;"></i>
                                </div>
                                <br />
                            </div>
                            <div class="modal-footer">
                                <button class="btn" data-dismiss="modal" aria-hidden="true" onclick="creagrilla_tiempo();">Cerrar</button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>

        </div>
    </div>



    <%--MODAL PARA VER HISTORIAL DE CORREOS--%>

    <a href="#div_unidad22" id="div_unidad_22" role="button" class="btn" style="visibility: hidden; position: absolute;" data-toggle="modal">ACAAAAA</a>

    <div id="div_unidad22" class="modal fade">
        <div class="modal-dialog modal-lg" style="width: 1000px;">
            <div class="modal-content" style="height: auto;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <div id="titulo_modal2"></div>
                </div>
                <div class="modal-body" style="height: 70%;">
                    <asp:UpdatePanel runat="server" ID="panel_histori" UpdateMode="Conditional">
                        <Triggers>
                        </Triggers>
                        <ContentTemplate>
                            <div class="table-responsive" style="border: 0">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_modal_2" runat="server" style="display: none; font-size: 3em;"></i>

                                            <div class="form-group" style="display: block">
                                                <asp:GridView ID="G_CORREOS" AutoGenerateColumns="false" CssClass="table table-bordered filtrar" OnRowDataBound="G_CORREOS_RowDataBound" runat="server" Visible="true"
                                                    OnRowCommand="G_CORREOS_RowCommand" DataKeyNames="rutcliente, CODVENDEDOR, contenido, fecha, destinatario, asunto,nombrecliente" ShowHeaderWhenEmpty="True" Font-Size="12px">
                                                    <HeaderStyle CssClass="test no-sort" />
                                                    <Columns>


                                                        <asp:TemplateField HeaderText="Ver">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btn_abrir_correo" runat="server" ImageUrl="img/search.png" Width="25"
                                                                    CommandName="Abrir" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:BoundField DataField="nombre_contacto" HeaderText="NOMBRE CONTACTO">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="fecha" HeaderText="FECHA">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Left" />

                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="destinatario" HeaderText="DESTINATARIO">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="asunto" HeaderText="ASUNTO">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>


                                                        <%--                                                        <asp:BoundField DataField="contenido" HeaderText="CONTENIDO">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>--%>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        No existen datos.
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br />

                                <div class="row" runat="server" id="contenido_correo" visible="false">
                                    <hr />
                                    <div class="col-md-12">
                                        <div class="box box-blue">
                                            <div class="box-title">
                                                <h3><i class="fa fa-bars"></i>Contenido Correo</h3>
                                                <div class="box-tool">
                                                </div>
                                            </div>
                                            <div class="box-content">

                                                <div runat="server" id="div_contenido_correo"></div>

                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="row>">
                                    <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="I1" runat="server" style="display: none; font-size: 3em;"></i>
                                </div>
                                <br />
                            </div>
                            <div class="modal-footer">
                                <button class="btn" data-dismiss="modal" aria-hidden="true" onclick="creagrilla_tiempo();">Cerrar</button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>

        </div>
    </div>


    <%-- MODAL PARA ENVIAR CORREOS y agendar CONTACTOS --%>

    <a href="#div_unidad1" id="div_unidad_1" role="button" class="btn" style="visibility: hidden; position: absolute;" data-toggle="modal"></a>

    <div id="div_unidad1" class="modal fade">
        <div class="modal-dialog modal-lg" style="width: 1000px;">
            <div class="modal-content" style="height: auto;">
                <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                    <Triggers>

                        <%--<asp:AsyncPostBackTrigger ControlID="carga_d" />--%>

                        <%--<asp:AsyncPostBackTrigger ControlID="btn_guardar_prod_equi" />--%>
                    </Triggers>
                    <ContentTemplate>
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <div id="titulo_modal"></div>
                        </div>
                        <div class="modal-body" style="height: 70%;">
                            <div class="table-responsive" style="border: 0">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="form-group" style="display: block">
                                                <button id="btn_nuevo_contacto" class="btn btn-success icon-plus btn-circle" runat="server" onserverclick="btn_nuevo_contacto_ServerClick"><i class="fa fa-plus"></i></button>
                                                <asp:Button runat="server" ID="B_Guardar" CssClass="btn btn-success" Text="Guardar" OnClick="B_Guardar_Click" Visible="false" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="form-group" style="display: block">
                                                <!-- *************************                
                                                    NOMBRE
                                                    CARGO
                                                    NÚMERO
                                                    CORREO
                                                    DIRECCIÓN  ******************************************** -->
                                                <div class="form-group">
                                                    <div class="col-sm-3 col-lg-3 controls">
                                                        <div class="input-group">
                                                            <label class="col-sm-2 control-label">
                                                                <b>Nombre</b>
                                                            </label>
                                                            <asp:TextBox runat="server" ID="t_nombre_contact" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3 col-lg-3 controls">
                                                        <div class="input-group">
                                                            <label class="col-sm-2 control-label">
                                                                <b>Cargo</b>
                                                            </label>
                                                            <asp:TextBox runat="server" ID="t_cargo_contact" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3 col-lg-3 controls">
                                                        <div class="input-group">
                                                            <label class="col-sm-2 control-label">
                                                                <b>Número</b>
                                                            </label>
                                                            <asp:TextBox runat="server" ID="t_num_contact" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3 col-lg-3 controls">
                                                        <div class="input-group">
                                                            <label class="col-sm-2 control-label">
                                                                <b>Correo</b>
                                                            </label>
                                                            <asp:TextBox runat="server" ID="t_correo_contact" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6 col-lg-6 controls">
                                                        <div class="input-group">
                                                            <label class="col-sm-2 control-label">
                                                                <b>Comuna</b>
                                                            </label>
                                                            <asp:TextBox runat="server" ID="t_direcc_contact" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-3 col-lg-3 controls">
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
                                                <asp:GridView ID="G_CONTACTOS" AutoGenerateColumns="false" CssClass="table table-bordered filtrar" OnRowDataBound="G_CONTACTOS_RowDataBound" runat="server" Visible="true"
                                                    OnRowCommand="G_CONTACTOS_RowCommand" DataKeyNames="rutcliente, nombre_contacto, CORREO" ShowHeaderWhenEmpty="True" Font-Size="12px">
                                                    <HeaderStyle CssClass="test no-sort" />
                                                    <Columns>


                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkAccept" runat="server" EnableViewState="true" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="QUITAR">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btn_quitar" runat="server" ImageUrl="img/delete.png" Width="25"
                                                                    CommandName="Eliminar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClientClick='<%# confirmDelete2( Eval("nombre_contacto").ToString() ) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="EDITAR">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ImageUrl="img/pencil.png" Width="25"
                                                                    CommandName="Editar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:BoundField DataField="nombre_contacto" HeaderText="NOMBRE">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="cargo" HeaderText="CARGO">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="NUMERO" HeaderText="NÚMERO">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="CORREO" HeaderText="CORREO">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="DIRECCION" HeaderText="COMUNA">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10%" CssClass="text-center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:TemplateField HeaderText="ENVIAR CORREO">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ImageUrl="img/email.png" Width="25"
                                                                    CommandName="correo" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="AGENDAR">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ImageUrl="img/calendar_empty.png" Width="25"
                                                                    CommandName="agendar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        No existen datos.
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                                <asp:Button runat="server" ID="btn_masivo_correo" CssClass="btn btn-primary" Text="Correos" OnClick="btn_masivo_correo_Click" Visible="true" />

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_modal_1" runat="server" style="display: none; font-size: 3em;"></i>

                                <div class="row>">
                                    <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_gif_2" runat="server" style="display: none; font-size: 3em;"></i>

                                </div>
                                <br />

                                <div class="row" runat="server" id="enviar_correo" visible="false">
                                    <hr />
                                    <div class="col-md-12">
                                        <div class="box box-blue">
                                            <div class="box-title">
                                                <h3><i class="fa fa-bars"></i>Enviar Correo</h3>
                                                <div class="box-tool">
                                                </div>
                                            </div>
                                            <div class="box-content">

                                                <div class="form-group">
                                                    <label for="textfield5" class="col-sm-3 col-lg-2 control-label">Destinatarios</label>
                                                    <div class="col-sm-9 col-lg-10 controls">
                                                        <asp:TextBox runat="server" ID="txt_destino" CssClass="form-control" Enabled="true"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <%--espacio en blanco--%>
                                                <div class="row">
                                                </div>

                                                <div class="form-group">
                                                    <label for="textfield5" class="col-sm-3 col-lg-2 control-label">Cc</label>
                                                    <div class="col-sm-9 col-lg-10 controls">
                                                        <asp:TextBox runat="server" ID="txt_cc" CssClass="form-control" Enabled="true"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <%--espacio en blanco--%>
                                                <div class="row">
                                                </div>

                                                <div class="form-group">
                                                    <label for="password5" class="col-sm-3 col-lg-2 control-label">Asunto</label>
                                                    <div class="col-sm-9 col-lg-10 controls">
                                                        <asp:TextBox runat="server" ID="txt_asunto" CssClass="form-control" Enabled="true"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <%--espacio en blanco--%>
                                                <div class="row">
                                                </div>

                                                <div class="form-group">
                                                    <label for="textarea5" class="col-sm-3 col-lg-2 control-label">Contenido</label>
                                                    <div class="col-sm-9 col-lg-10 controls">
                                                        <textarea name="textarea5" runat="server" style="white-space: pre-wrap;" id="tx_contenido" rows="5" class="form-control"></textarea>
                                                    </div>
                                                </div>

                                                <%--espacio en blanco--%>
                                                <div class="row">
                                                </div>
                                                <div class="form-group last">
                                                    <div class="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2">
                                                        <asp:Button runat="server" ID="btn_enviar_correo" CssClass="btn btn-success" OnClientClick="cargando_en_modal1();" Text="Enviar" OnClick="btn_enviar_correo_Click" />
                                                        <asp:Button runat="server" ID="btn_cancelar" CssClass="btn" Text="Cancelar" OnClick="btn_cancelar_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" runat="server" id="div_agendar" visible="false">
                                    <hr />
                                    <div class="col-md-12">
                                        <div class="box box-blue">
                                            <div class="box-title">
                                                <h3><i class="fa fa-bars"></i>Agendar</h3>
                                                <div class="box-tool">
                                                </div>
                                            </div>
                                            <div class="box-content">
                                                <div class="row">
                                                    <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Dia Agenda</label>

                                                    <div class="col-sm-3 col-lg-3 controls">
                                                        <div class="input-group">

                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <asp:TextBox ID="txt_dia_agenda" type="date" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>


                                                        </div>
                                                    </div>
                                                    <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Hora (00:00)</label>
                                                    <div class="col-sm-3 col-lg-3 controls">

                                                        <div class="input-group">
                                                            <a class="input-group-addon" href="#"><i class="fa fa-clock-o"></i></a>

                                                            <asp:TextBox ID="am_pm" type="time" CssClass="form-control timepicker-default" runat="server" Width="100%"></asp:TextBox>


                                                        </div>

                                                    </div>
                                                </div>
                                                <hr />
                                                <div class="row">

                                                    <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Observación</label>
                                                    <div class="col-sm-9 col-lg-9 controls">

                                                        <div class="col-sm-9 col-lg-10 controls">
                                                            <textarea name="textarea5" runat="server" id="tx_obser" rows="3" class="form-control"></textarea>
                                                        </div>

                                                    </div>

                                                    <div class="form-group last">
                                                        <div class="col-sm-9 col-lg-10">
                                                            <asp:Button runat="server" ID="btn_agendar" CssClass="btn btn-success" OnClientClick="cargando_en_modal1();" Text="Agendar" OnClick="btn_agendar_Click" />
                                                            <asp:Button runat="server" ID="btn_cancelar_2" CssClass="btn" Text="Cancelar" OnClick="btn_cancelar_Click" />
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button class="btn" data-dismiss="modal" aria-hidden="true" onclick="creagrilla_tiempo();">Cerrar</button>
                                </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </div>

    <img src="img/c8.png" style="visibility: hidden" />

    <%-- <a id="btn-scrollup" class="btn btn-circle btn-lg" href="#"> </a>--%>
</asp:Content>




