<%@ Page Language="C#" UICulture="es-ES" Culture="es-ES" CodeBehind="ListadoProductosPlanificador.aspx.cs" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" AutoEventWireup="True" Inherits="SoprodiApp.ListadoProductosPlanificador" %>

<%@ OutputCache Location="None" NoStore="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

    <meta http-equiv="X-UA-Compatible" content="IE=10; IE=9; IE=8; IE=7; IE=EDGE" />

    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="0">

    <title>DETALLE</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <!-- Place favicon.ico and apple-touch-icon.png in the root directory -->
    <link href="img/icono.ico" rel="shortcut icon">
    <!--base css styles-->
    <link rel="stylesheet" href="assets/bootstrap/css/bootstrap.min.css">
    <link rel="stylesheet" href="assets/font-awesome/css/font-awesome.min.css">

    <!--page specific css styles-->

    <link rel="stylesheet" href="CHOSEN/docsupport/style.css">
    <link rel="stylesheet" href="CHOSEN/docsupport/prism.css">
    <link rel="stylesheet" href="CHOSEN/chosen.css">

    <!--base css styles-->
    <link rel="stylesheet" href="assets/bootstrap/css/bootstrap.min.css">
    <link rel="stylesheet" href="assets/font-awesome/css/font-awesome.min.css">


    <!--page specific css styles-->

    <!--flaty css styles-->
    <link rel="stylesheet" href="css/flaty.css">
    <link rel="stylesheet" href="css/flaty-responsive.css">

    <script type="text/javascript">




</script>
    <script src="js/tablesort.js"></script>
    <script src="js/prettify.js"></script>

    <script src='js/sorts/tablesort.date.js'></script>
    <script src='js/sorts/tablesort.dotsep.js'></script>
    <script src='js/sorts/tablesort.filesize.js'></script>
    <script src='js/sorts/tablesort.numeric.js'></script>

    <script src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>

    <script>




        $(document).ready(function () {

            document.onkeypress = stopRKey;
            try {
                superfiltro();

            } catch (e) { }

            try {
                superfiltro2();

            } catch (e) { }


            $('.btn-radio').click(function (e) {
                $('.btn-radio').not(this).removeClass('active')
                    .siblings('input').prop('checked', false)
                    .siblings('.img-radio').css('opacity', '0.2')
                    .siblings('.btn-radio').css('background-color', '#0090ff');
                $(this).addClass('active')
                    .siblings('input').prop('checked', true)
                    .siblings('.img-radio').css('opacity', '1')
                    .siblings('.btn-radio').css('background-color', 'green');
            });

            $('#inputTitle').keyup(function () {
                try {
                    var carga_inicial = $(this).val();
                    var asignado = document.getElementById("l_suma").innerHTML;
                    document.getElementById("l_disponible").innerHTML = carga_inicial - asignado;


                } catch (e) {
                }
            });
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            // re-bind your jQuery events here
            $('.btn-radio').click(function (e) {
                $('.btn-radio').not(this).removeClass('active')
                    .siblings('input').prop('checked', false)
                    .siblings('.img-radio').css('opacity', '0.2')
                    .siblings('.btn-radio').css('background-color', '#0090ff');
                $(this).addClass('active')
                    .siblings('input').prop('checked', true)
                    .siblings('.img-radio').css('opacity', '1')
                    .siblings('.btn-radio').css('background-color', 'green');
            });
        });

        function stopRKey(evt) {
            var evt = (evt) ? evt : ((event) ? event : null);
            var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
            if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
        }


        function CARGANDO() {

            var elem3 = document.getElementById("<%=cargando_gif.ClientID%>");
            elem3.style.display = "block";

        }

        function check_trans(id) {

            var tx = id;
            //alert(id);
            document.getElementById('check_tran').value = tx.split('*')[0].trim();
            document.getElementById('inputTitle').value = tx.split('*')[1].trim();

            try {
                var carga_inicial = tx.split('*')[1].trim();
                var asignado = document.getElementById("l_suma").innerHTML;

                //alert(carga_inicial);
                //alert($(this).val());

                document.getElementById("l_disponible").innerHTML = carga_inicial - asignado;


            } catch (e) {
            }


        }



        function validar_disponibles() {
            var col1;
            var col2;
            var grid = document.getElementById('<%=G_DETALLE_FACTURA.ClientID%>');
            var permite = true;

            for (i = 0; i < grid.rows.length; i++) {
                col1 = grid.rows[i].cells[12];
                col2 = grid.rows[i].cells[11];


                //col2 = grid.rows[i].cells[1];

                for (j = 0; j < col1.childNodes.length; j++) {
                    if (col1.childNodes[j].type == "text") {

                        var int_col1 = parseFloat(col2.innerHTML);
                        var int_col2 = parseFloat(col1.childNodes[j].value);

                        //alert(int_col1);
                        //alert(int_col2);

                        if (int_col2 > int_col1) {
                            permite = false;
                        }
                    }
                }
            }


            if (!permite) {

                if (!confirm('Esta sobrepasando lo disponible para Planificar ¿Estás seguro de continuar?')) { return false; }; CARGANDO();
            }



        }


        function activa_camiones() {

            $('.btn-radio').click(function (e) {
                $('.btn-radio').not(this).removeClass('active')
                    .siblings('input').prop('checked', false)
                    .siblings('.img-radio').css('opacity', '0.2')
                    .siblings('.btn-radio').css('background-color', '#0090ff');
                $(this).addClass('active')
                    .siblings('input').prop('checked', true)
                    .siblings('.img-radio').css('opacity', '1')
                    .siblings('.btn-radio').css('background-color', 'green');
            });
        }

        function justNumbers(e) {
            var keynum = window.event ? window.event.keyCode : e.which;
            if ((keynum == 8) || (keynum == 46))
                return true;

            return /\d/.test(String.fromCharCode(keynum));
        }
        function calu(a) {
            try {
                var carga_inicial = document.getElementById("inputTitle").value;
                var asignado = document.getElementById("l_suma").innerHTML;

                //alert(carga_inicial);
                //alert(asignado);

                document.getElementById("l_disponible").innerHTML = carga_inicial - asignado;


            } catch (e) {
            }
        }

        function superfiltro() {
            try {
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
            } catch (e) { }


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
        function modal_unidad() {

            document.getElementById("div_unidad_").click();
            chosen_update();
        }
        function chosen_update() {


            $("#<%=cb_productos_kg.ClientID%>").chosen();
            $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");


        }
        function guardar_solo_kg() {


            var parameters = new Object();

            parameters.sw = document.getElementById("<%=cb_productos_kg.ClientID%>").value;;
            parameters.tipo = document.getElementById("tx_tipo_").value;;
            parameters.valor = document.getElementById("tx_valor").value;;
            parameters.unidades = document.getElementById("tx_unidades").value;;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "ListadoProductosPlanificador.aspx/guardar_solo_kg",
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
        function guarda(resp) {
            resp = resp.d;
            if (resp == "ok") {
                $("#<%=btn_refresh.ClientID%>").click();
                alert("Guardado");
                chosen_update();
                //click refresh


            }
            else {
                alert("error");
                chosen_update();
            }

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
        function carga_d_Click() {



            var parameters = new Object();

            parameters.sw = document.getElementById("<%=cb_productos_kg.ClientID%>").value;;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "ListadoProductosPlanificador.aspx/carga_click",
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



        function LoadDetalle(result) {

            try {
                var elem3 = document.getElementById("detalle_prod");
                elem3.style = "visibility:block";
                //document.getElementById("detalle_prod").style = "visible:hidden";

                $("#<%=cb_productos_kg.ClientID%>").chosen();
                $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");
            } catch (e) { }

            $("#<%=tx_tipo_.ClientID%>").value = "";

            $("#<%=tx_valor.ClientID%>").value = "";

            $("#<%=tx_unidades.ClientID%>").value = "";


            $.each(result.d, function () {

                //alert('aqui' + this.stkunit);


                //alert('aqui' + this.valor);

                $("#tx_tipo_").val(this.stkunit);
                $("#tx_valor").val(this.valor);
                $("#tx_unidades").val(this.unidades);


                ;
            });



        }


        function producto_cambio_animal2() {



            var parameters = new Object();

            parameters.sw = "animal";

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "ListadoProductosPlanificador.aspx/PRODUCTO_CAMBIO2",
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
        function modal_unidad() {

            document.getElementById("div_unidad_").click();
            chosen_update();
        }

        function superfiltro2() {
            try {
                $(".filtrar tr:has(td)").each(function () {
                    var t = $(this).text().toLowerCase();
                    $("<td class='indexColumn'></td>")
                        .hide().text(t).appendTo(this);
                });
                //Agregar el comportamiento al texto (se selecciona por el ID) 
                $("#t_filtro_memoria_2").keyup(function () {
                    var s = $(this).val().toLowerCase().split(" ");
                    $(".filtrar tr:hidden").show();
                    $.each(s, function () {
                        $(".filtrar tr:visible .indexColumn:not(:contains('"
                            + this + "'))").parent().hide();
                    });
                });
            } catch (e) { }


        }

        function fuera(fecha, vendedor, grupos, bit) {


            var urlPdf = "/DETALLE_FACTURA.aspx?";
            //var path2 = "P=" + path;
            //var filename2 = "&N=" + filename;
            //var urlPdf_Final = urlPdf + path2 + filename2;
            var param = "F=" + fecha + "&V=" + vendedor + "&G=" + grupos + "&i=" + bit;
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

        function load_chosen_combos() {
            $("#<%=d_bodega.ClientID%>").chosen();
            $("#<%=d_bodega.ClientID%>").trigger("chosen:updated");
            $("#<%=d_transpor.ClientID%>").chosen();
            $("#<%=d_transpor.ClientID%>").trigger("chosen:updated");
            $("#<%=d_camion.ClientID%>").chosen();
            $("#<%=d_camion.ClientID%>").trigger("chosen:updated");
            $("#<%=d_chofer.ClientID%>").chosen();
            $("#<%=d_chofer.ClientID%>").trigger("chosen:updated");
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

            $("#<%=d_bodega.ClientID%>").chosen();
            $("#<%=d_bodega.ClientID%>").trigger("chosen:updated");
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

            $("#<%=d_bodega.ClientID%>").chosen();
            $("#<%=d_bodega.ClientID%>").trigger("chosen:updated");
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

            $("#<%=d_bodega.ClientID%>").chosen();
            $("#<%=d_bodega.ClientID%>").trigger("chosen:updated");
            $("#<%=d_transpor.ClientID%>").chosen();
            $("#<%=d_transpor.ClientID%>").trigger("chosen:updated");
            $("#<%=d_camion.ClientID%>").chosen();
            $("#<%=d_camion.ClientID%>").trigger("chosen:updated");
            $("#<%=d_chofer.ClientID%>").chosen();
            $("#<%=d_chofer.ClientID%>").trigger("chosen:updated");

        }

        function creagrilla() {
            try {
                var fecha1 = $('th:contains("FechaDespacho")').index();
                var fecha2 = $('th:contains("FechaEmision")').index();
                var fecha3 = $('th:contains("GxActualizado")').index();
                var fecha4 = $('th:contains("FechaCreacion")').index();


                $("#G_INFORME_TOTAL_VENDEDOR").DataTable({
                    "order": [[7, "asc"]],
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
            } catch (e) {
                //alert(e.message);
            }

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

    </script>

    <style>
        .test {
            background-color: #428bca !important;
            color: white !important;
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

        .btn-radio {
            width: 105%;
        }

        .img-radio {
            opacity: 0.2;
            margin-bottom: 5px;
        }

        .space-20 {
            margin-top: 20px;
        }
    </style>

</head>



<body>

    <form runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </ajaxToolkit:ToolkitScriptManager>



        <a href="#div_unidad" id="div_unidad_" role="button" class="btn" data-toggle="modal"></a>

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
                            </Triggers>
                            <ContentTemplate>
                                <div class="table-responsive" style="border: 0">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box">
                                                <div class="form-group" style="display: block">

                                                    <div class="col-sm-6 controls" style="visibility: hidden; position: absolute;">
                                                        <div class="input-group" id="check_cerrados2">
                                                            <asp:RadioButton ID="RadioButton1" Text="&nbsp;&nbsp;Cons.Animal" Style="padding-right: 20px;" Checked="True"
                                                                GroupName="rb_productos_2" runat="server" onchange="producto_cambio_animal2()" />

                                                            <asp:RadioButton ID="RadioButton2" Style="padding-right: 20px;" Text="&nbsp;&nbsp;Cons.Humano"
                                                                GroupName="rb_productos_2" runat="server" onchange="producto_cambio_humano2()" />
                                                        </div>
                                                        <div style="float: right; visibility: hidden; position: absolute;">
                                                            Cargar filtros
                                                        <%--<asp:ImageButton ID="ImageButton2" ImageUrl="~/img/Ticket_verde.png" runat="server" OnClientClick="CARGA_FECHA();" OnClick="b_Click" />--%>
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
                                                    <div class="col-sm-4 col-lg-4">
                                                        <div class="controls">
                                                            <label>Tipo de UNIDAD</label>
                                                            <asp:TextBox runat="server" ID="tx_tipo_"></asp:TextBox>
                                                        </div>


                                                    </div>
                                                    <div class="col-sm-4 col-lg-4">
                                                        <div class="controls">
                                                            <label>Equivale a </label>
                                                            <asp:TextBox runat="server" ID="tx_valor"></asp:TextBox>
                                                            Kilos
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-4 col-lg-4">
                                                        <div class="controls">
                                                            <label>Unidades </label>
                                                            <asp:TextBox runat="server" ID="tx_unidades"></asp:TextBox>
                                                            UNID
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--<asp:Button ID="btn_guardar_prod_equi" runat="server" Style="color: white; float: right; margin-right: 1.3%" OnClick="Button3_Click" Class="btn btn-success" Text="Guardar" />--%>
                                    </div>
                                    <div class="row">
                                        <br />
                                        <input id="btn_guardar_prod_equi" type="button" class="btn btn-success" value="Guardar" onclick="guardar_solo_kg()" style="display: block; float: right;" />
                                    </div>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <button class="btn" data-dismiss="modal" onclick="esconde(); chosen_update(); load_chosen_combos();" aria-hidden="true">Cerrar</button>
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


        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:TextBox ID="l_bodega" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                <asp:TextBox ID="l_transpor" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                <asp:TextBox ID="l_camion" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                <asp:TextBox ID="l_chofer" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>

                <script type="text/javascript">

                    Sys.Application.add_load(BindEvents);

                    function BindEvents() {
                        $('.btn-radio').click(function (e) {
                            $('.btn-radio').not(this).removeClass('active')
                                .siblings('input').prop('checked', false)
                                .siblings('.img-radio').css('opacity', '0.2')
                                .siblings('.btn-radio').css('background-color', '#0090ff');
                            $(this).addClass('active')
                                .siblings('input').prop('checked', true)
                                .siblings('.img-radio').css('opacity', '1')
                                .siblings('.btn-radio').css('background-color', 'green');
                        });

                        try {
                            new Tablesort(document.getElementById('G_DETALLE_FACTURA'));

                        }
                        catch (r) { }


                        $("#<%=d_bodega.ClientID%>").change(function () {

                            // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                            //este parametro mapeara con el definido en el web service
                            var arr = $(this).val();
                            document.getElementById("<%=l_bodega.ClientID %>").value = arr;

                            var parameters = new Object();
                            parameters.BODEGA = document.getElementById("<%=l_bodega.ClientID %>").value;

                            parameters = JSON.stringify(parameters);

                            $.ajax({
                                type: "POST",
                                url: "ListadoProductosPlanificador.aspx/BODEGA",
                                data: parameters,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: false,
                                success: LoadTranspor,
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    alert("Error al cargar transporte");
                                }
                            });
                            $("#<%=d_bodega.ClientID%>").chosen();
                            $("#<%=d_bodega.ClientID%>").trigger("chosen:updated");
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
                                url: "ListadoProductosPlanificador.aspx/TRANSPOR_CHOFER",
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
                                url: "ListadoProductosPlanificador.aspx/TRANSPOR_CAMION",
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
                                url: "ListadoProductosPlanificador.aspx/CARGA_INICIAL_TRANSPORTE",
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

                                if (resp == "") {
                                    resp = 0;
                                }


                                try {
                                    var carga_inicial = resp;
                                    var asignado = document.getElementById("l_suma").innerHTML;
                                    var resta_precisa = parseFloat(carga_inicial) - parseFloat(asignado);
                                    if (resta_precisa != "NaN") {
                                        document.getElementById("l_disponible").innerHTML = resta_precisa;
                                    } else {
                                        document.getElementById("l_disponible").innerHTML = "0";
                                    }


                                } catch (e) {
                                    var carga_inicial = 0;
                                    var asignado = 0;
                                    var resta_precisa = parseFloat(carga_inicial) - parseFloat(asignado);
                                    if (resta_precisa != "NaN") {
                                        document.getElementById("l_disponible").innerHTML = resta_precisa;
                                    } else {
                                        document.getElementById("l_disponible").innerHTML = "0";
                                    }
                                }

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
                                url: "ListadoProductosPlanificador.aspx/CAMION_CARGA",
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



                                try {
                                    var carga_inicial = resp;
                                    var asignado = document.getElementById("l_suma").innerHTML;
                                    var resta_precisa = parseFloat(carga_inicial) - parseFloat(asignado);
                                    if (resta_precisa != "NaN") {
                                        document.getElementById("l_disponible").innerHTML = resta_precisa;
                                    } else {
                                        document.getElementById("l_disponible").innerHTML = "0";
                                    }


                                } catch (e) {
                                    var carga_inicial = 0;
                                    var asignado = 0;
                                    var resta_precisa = parseFloat(carga_inicial) - parseFloat(asignado);
                                    if (resta_precisa != "NaN") {
                                        document.getElementById("l_disponible").innerHTML = resta_precisa;
                                    } else {
                                        document.getElementById("l_disponible").innerHTML = "0";
                                    }
                                }

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




                        $('#inputTitle').keyup(function () {
                            try {
                                var carga_inicial = $(this).val();
                                var asignado = document.getElementById("l_suma").innerHTML;

                                //alert(carga_inicial);
                                //alert($(this).val());

                                document.getElementById("l_disponible").innerHTML = carga_inicial - asignado;


                            } catch (e) {
                            }
                        });
                    }

                </script>


                <div id="main-content">
                    <!-- BEGIN Main Content -->
                    <div class="row" id="normal" runat="server" style="display: none">
                        <div class="col-md-12">
                            <div class="box">
                                <asp:Button ID="btn_unidad" runat="server" OnClientClick="modal_unidad();" Style="color: white; margin-right: 1.3%" Class="btn btn-warning" Text="Cambio de unidad" />
                                <asp:Button ID="btn_refresh" runat="server" Style="color: white; visibility: hidden; position: absolute; margin-right: 1.3%" Class="btn btn-warning" Text="refresh" OnClick="btn_refresh_Click" />

                                <asp:TextBox ClientIDMode="Static" ID="check_tran" runat="server" Style="visibility: hidden"></asp:TextBox>
                                <div class="box-title" runat="server" id="filtros">
                                    <i class="fa fa-table"></i>
                                    <h3 runat="server" id="detalle" title="Detalle Venta "></h3>

                                    <h3 runat="server" id="bodega"></h3>

                                    <h3 runat="server" style="float: right" id="correo_vendedor" title=""></h3>
                                    <h3 runat="server" style="float: right" id="H1" title="-">- -</h3>
                                    <h3 runat="server" style="float: right" id="cliente_2" title=""></h3>
                                    <div class="box-tool">
                                        <a data-action="collapse" href="#"></a>

                                    </div>
                                </div>
                                <div class="box-content">
                                    <div class="btn-toolbar pull-left">
                                        <input type="text" id="t_filtro_memoria" style="width: 200px; margin-right: 7px; padding: 5px;" placeholder="Filtrar..." class="form-control" onchange="changevalue();" onkeyup="changevalue();" />

                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="table-responsive" style="border: 0">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box">
                                                    <div class="form-group" style="overflow-x: auto;">

                                                        <asp:GridView ID="G_DETALLE_FACTURA" AutoGenerateColumns="false" CssClass="table table-bordered filtrar" OnRowDataBound="G_DETALLE_FACTURA_RowDataBound" runat="server" Visible="true"
                                                            ShowHeaderWhenEmpty="True" Font-Size="12px">
                                                            <HeaderStyle CssClass="test no-sort" />
                                                            <Columns>


                                                                <asp:TemplateField HeaderText="Uno" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkb1" runat="server" AutoPostBack="true" OnCheckedChanged="chkb1_CheckedChanged" />
                                                                    </HeaderTemplate>

                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkAccept" runat="server" AutoPostBack="true" OnCheckedChanged="chkAccept_CheckedChanged" CssClass="center" EnableViewState="true" Checked="false"
                                                                            CommandName="chk" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="45" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:BoundField DataField="codproducto" HeaderText="CodProducto">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CodUnVenta" HeaderText="CodUnVenta">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                    <ItemStyle HorizontalAlign="Center" />

                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Equivale" HeaderText="KG Equivale">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>

                                                                <asp:BoundField DataField="DescProducto" HeaderText="Producto">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Cantidad" HeaderText="CantidadSP">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Cant_despachado" HeaderText="CantidadDESP">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>

                                                                <asp:BoundField DataField="Cantidad_kg" HeaderText="KGCantidadSP">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Cant_despachado_kg" HeaderText="KGCantidadDESP">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Disponible_kg" HeaderText="KGDisponible">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>

                                                                <asp:BoundField DataField="planificado" HeaderText="Planificado">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DISPO_PLANIFICA" HeaderText="Disponi.Planific.">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" CssClass="text-center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>


                                                                <asp:TemplateField HeaderText="A Despachar" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>

                                                                        <asp:TextBox ID="txt_a_desp" Text='<%# Eval("DISPO_PLANIFICA") %>' runat="server"></asp:TextBox>

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


                                                        <div runat="server" id="table_almace">
                                                        </div>
                                                        <br />
                                                        <asp:Label runat="server" ID="l_suma" Style="font-size: 60;" Text="0"></asp:Label>
                                                        KG Totales
                                                    </div>
                                                </div>

                                            </div>
                                        </div>


                                        <div class="row">


                                            <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="BTN_REFRESH_2" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <asp:LinkButton ID="BTN_REFRESH_2" class="btn btn-info icon-plus btn-circle fa fa-refresh" Visible="true" title="Refrescar" runat="server" OnClick="refresh_edit_"></asp:LinkButton>

                                                    <div runat="server" id="div_camiones"></div>

                                                    <div class="form-group">
                                                        <div class="col-sm-1 col-lg-1 controls">
                                                        </div>
                                                        <label for="inputTitle" runat="server" class="col-sm-2 control-label">Bodega</label>
                                                        <div class="col-sm-6 col-lg-7 controls">
                                                            <asp:ListBox ID="d_bodega" SelectionMode="Single" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                        </div>
                                                        <hr style="margin-top: 3.5%;">
                                                    </div>

                                                    <div class="form-group">
                                                        <div class="col-sm-1 col-lg-1 controls">
                                                            <a id="btn_plus_transpo" class="btn btn-success icon-plus btn-circle" href="MANT_TRANSPORT.aspx" target='_blank'><i class="fa fa-plus"></i></a>
                                                        </div>
                                                        <label class="col-sm-3 col-lg-2 control-label">Transportista</label>
                                                        <div class="col-sm-6 col-lg-7 controls">
                                                            <asp:ListBox ID="d_transpor" SelectionMode="Single" OnSelectedIndexChanged="d_transpor_SelectedIndexChanged" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                        </div>

                                                        <hr style="margin-top: 3.5%;">
                                                    </div>

                                                    <div class="form-group">
                                                        <div class="col-sm-1 col-lg-1 controls">
                                                            <a id="btn_plus_camion" class="btn btn-success icon-plus btn-circle" href="MANT_CAMION.aspx" target='_blank'><i class="fa fa-plus"></i></a>
                                                        </div>
                                                        <label class="col-sm-3 col-lg-2 control-label">Camión</label>
                                                        <div class="col-sm-6 col-lg-7 controls">
                                                            <asp:ListBox ID="d_camion" SelectionMode="Single" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                        </div>

                                                        <hr style="margin-top: 3.5%;">
                                                    </div>

                                                    <div class="form-group">
                                                        <div class="col-sm-1 col-lg-1 controls">
                                                            <a id="btn_plus_chofer" class="btn btn-success icon-plus btn-circle" href="MANT_CHOFER.aspx" target='_blank'><i class="fa fa-plus"></i></a>
                                                        </div>
                                                        <label class="col-sm-3 col-lg-2 control-label">Chofer</label>
                                                        <div class="col-sm-6 col-lg-7 controls">
                                                            <asp:ListBox ID="d_chofer" SelectionMode="Single" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                        </div>

                                                        <hr style="margin-top: 3.5%;">
                                                    </div>

                                                    <div class="form-group">
                                                        <div class="col-sm-1 col-lg-1 controls">
                                                        </div>
                                                        <label class="col-sm-3 col-lg-2 control-label">Vuelta</label>
                                                        <div class="col-sm-6 col-lg-7 controls">
                                                            <asp:TextBox ClientIDMode="Static" TextMode="Number" class="form-control" runat="server" ID="tx_vuelta"></asp:TextBox>
                                                        </div>
                                                        <hr style="margin-top: 3.5%;">
                                                    </div>

                                                    <div class="form-group">
                                                        <div class="col-sm-1 col-lg-1 controls">
                                                        </div>
                                                        <label class="col-sm-3 col-lg-2 control-label">Carga Inicial</label>
                                                        <div class="col-sm-6 col-lg-7 controls">
                                                            <asp:TextBox ClientIDMode="Static" type="number" onkeydown="return(event.keyCode!=13);" class="form-control" runat="server" ID="inputTitle"></asp:TextBox>
                                                        </div>
                                                        <hr style="margin-top: 3.5%;">
                                                    </div>

                                                    <div class="form-group">
                                                        <div class="col-sm-1 col-lg-1 controls">
                                                        </div>
                                                        <label class="col-sm-3 col-lg-2 control-label">Orden para entregar</label>
                                                        <div class="col-sm-6 col-lg-7 controls">
                                                            <asp:TextBox ClientIDMode="Static" TextMode="Number" class="form-control" runat="server" ID="tx_orden"></asp:TextBox>
                                                        </div>
                                                        <hr style="margin-top: 3.5%;">
                                                    </div>

                                                    <div class="form-group">
                                                        <div class="col-sm-1 col-lg-1 controls">
                                                        </div>
                                                        <label class="col-sm-3 col-lg-2 control-label">Fecha</label>
                                                        <div class="col-sm-6 col-lg-7 controls">
                                                            <asp:TextBox ClientIDMode="Static" type="date" class="form-control" runat="server" ID="t_fecha_despacho"></asp:TextBox>
                                                            <asp:TextBox ClientIDMode="Static" type="text" class="form-control" Style="visibility: hidden; position: absolute;" runat="server" ID="estado_sp"></asp:TextBox>
                                                        </div>
                                                        <hr style="margin-top: 3.5%;">
                                                    </div>

                                                    <div class="form-group">
                                                        <div class="col-sm-1 col-lg-1 controls">
                                                        </div>
                                                        <label class="col-sm-3 col-lg-2 control-label">Observación</label>
                                                        <div class="col-sm-6 col-lg-7 controls">
                                                            <asp:TextBox ClientIDMode="Static" TextMode="multiline" Columns="50" Rows="5" class="form-control" runat="server" ID="tx_obs_plani"></asp:TextBox>
                                                        </div>
                                                        <hr style="margin-top: 3.5%;">
                                                    </div>

                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>


                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box">
                                                    <div class="form-group" style="overflow-x: auto;">


                                                        <asp:ImageButton ID="B_enviar" runat="server" ImageUrl="img/accept.png" OnClientClick="return validar_disponibles();CARGANDO();" OnClick="B_enviar_Click" />




                                                        <asp:ImageButton ID="b_sumar" runat="server" ImageUrl="img/add.png" visible="false" OnClientClick="return validar_disponibles();CARGANDO();" OnClick="B_enviar_Click" />


                                                        <%--<img src="img/accept.png" style="cursor: pointer;" onclick="alert('SP Asignado')" OnClick="B_enviar_Click"   />--%>

                                                        <asp:Label runat="server" Style="float: right; margin-right: 5%" Font-Size="25" ID="l_disponible" Text="0"></asp:Label>

                                                        <asp:Label runat="server" Style="float: right; margin-right: 5%" Text="KG Disponible"></asp:Label>
                                                    </div>
                                                </div>
                                                <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_gif" runat="server" style="display: none; font-size: 3em;"></i>

                                            </div>

                                        </div>


                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box box-green" style="margin-top: 1%;">
                                                    <div class="box-title">
                                                        <h3><i class="fa fa-bars"></i>PLANIFICACIONES DE ESTA SP</h3>
                                                        <div class="box-tool">
                                                            <a data-action="collapse" href="#"><i class="fa fa-chevron-up"></i></a>
                                                        </div>
                                                    </div>

                                                    <div class="box-content">
                                                        <div class="form-horizontal">
                                                            <div class="row" style="margin-right: 10px; margin-left: 10px;">


                                                                <%--table table-advance tablesorter filtrar3--%>
                                                                <asp:GridView ID="G_INFORME_TOTAL_VENDEDOR" ClientIDMode="Static" CssClass="table table-bordered table-advance filtrar" OnRowCommand="G_INFORME_TOTAL_VENDEDOR_RowCommand" OnRowDataBound="G_INFORME_TOTAL_VENDEDOR_RowDataBound" runat="server"
                                                                    ShowHeaderWhenEmpty="True" AutoGenerateColumns="false"
                                                                    DataKeyNames="id, coddocumento, cod_trans, estado, nombre_trans, codbodega, nombrecliente, fecha_despacho2, tiene_log, cod_camion, cod_chofer, observacion, bodega_plani, carga_inicial, patente, nombre_chofer, fecha_type_date, vuelta"
                                                                    Font-Size="12px">
                                                                    <HeaderStyle CssClass="test no-sort" />
                                                                    <Columns>

                                                                        <asp:TemplateField HeaderText="Quitar">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton  AutoPostBack="true" ID="btn_quitar" runat="server" ImageUrl="img/delete.png" Width="25"
                                                                                    CommandName="Eliminar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClientClick='<%# confirmDelete( Eval("CODDOCUMENTO").ToString() ) %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Editar" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btn_img" runat="server" ImageUrl="img/pencil.png" Width="25"
                                                                                    CommandName="Editar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Completar" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton runat="server" ImageUrl="img/accept.png" Width="25"
                                                                                    CommandName="Enviar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:BoundField DataField="coddocumento" HeaderText="CodDocum.">
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>

                                                                        <asp:BoundField DataField="nombrecliente" HeaderText="Cliente">
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>

                                                                        <asp:BoundField DataField="nombrevendedor" HeaderText="Vendedor">
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>

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

                                                                        <asp:BoundField DataField="observacion" HeaderText="OBS">
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>

                                                                        <asp:BoundField DataField="disponible_2" HeaderText="DisponibleCamion">
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>

                                                                        <asp:BoundField DataField="fecha_despacho2" HeaderText="FechaDespacho">
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>

                                                                        <asp:BoundField DataField="codbodega" HeaderText="Bodega">
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="text-center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>

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
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="row" id="factura" runat="server" style="display: none">
                                            <div class="col-md-12">
                                                <div class="box">
                                                    <div class="box-content">
                                                        <div class="invoice">
                                                            <div class="row">

                                                                <div class="col-md-6">
                                                                    <img src="https://ci6.googleusercontent.com/proxy/0IvEZhmBpBwcP5DjENtL0h02HsbIB171IswhVy0SqL-7oeoL7QCOc3dnKgJBq-jufMopy_JcZxg=s0-d-e1-ft#http://a58.imgup.net/Sopro4d9d.png" style="width: 80px">

                                                                    <h2>Soprodi S.A.</h2>
                                                                </div>
                                                                <div class="col-md-6 invoice-info">
                                                                    <p class="font-size-17">
                                                                        <strong>Factura #</strong>
                                                                        <asp:Label ID="num_factura" runat="server"></asp:Label>
                                                                    </p>
                                                                    <p>
                                                                        <asp:Label ID="fecha_factura" runat="server"></asp:Label>
                                                                    </p>
                                                                </div>
                                                            </div>

                                                            <hr class="margin-0" />

                                                            <div class="row">
                                                                <div class="col-md-6 company-info">
                                                                    <h4>Vendedor: </h4>
                                                                    <p>
                                                                        <asp:Label ID="vendedor_" runat="server"></asp:Label>
                                                                    </p>
                                                                    <p>
                                                                        <i class="fa fa-circle"></i>
                                                                        <asp:Label ID="codigo_vend" runat="server"></asp:Label>
                                                                    </p>
                                                                    <p>
                                                                        <i class="fa fa-circle"></i>
                                                                        <asp:Label ID="grupo_" runat="server"></asp:Label>
                                                                    </p>

                                                                </div>
                                                                <div class="col-md-6 company-info">
                                                                    <h4>Cliente: </h4>
                                                                    <p>
                                                                        <asp:Label ID="cliente_" runat="server"></asp:Label>
                                                                    </p>

                                                                    <p>
                                                                        <i class="fa fa-circle"></i>
                                                                        <asp:Label ID="rut_cliente" runat="server"></asp:Label>
                                                                    </p>

                                                                </div>
                                                            </div>

                                                            <br />
                                                            <br />
                                                            <div class="table-responsive" style="border: 0">
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div class="box">
                                                                            <div class="btn-toolbar pull-left">
                                                                                <input type="text" id="t_filtro_memoria_2" style="width: 200px; margin-right: 7px; padding: 5px;" placeholder="Filtrar..." class="form-control" onchange="changevalue();" onkeyup="changevalue();" />

                                                                            </div>



                                                                            <asp:GridView ID="G_PRODUCTOS" AutoGenerateColumns="true" CssClass="table table-bordered filtrar" OnRowCommand="G_PRODUCTOS_RowCommand" OnRowDataBound="G_PRODUCTOS_RowDataBound" runat="server" Visible="true"
                                                                                ShowHeaderWhenEmpty="True" Font-Size="12px" DataKeyNames="Cod.Producto, bodega">
                                                                                <HeaderStyle CssClass="test no-sort" />

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
                                                                    </div>
                                                                </div>

                                                                <div class="row" style="margin-right: 1%;">

                                                                    <div class="col-md-6 col-md-offset-6 invoice-amount" style="float: right">
                                                                        <p><strong>Subtotal:</strong> <span>$<asp:Label runat="server" ID="subtotal"></asp:Label></span></p>

                                                                        <p>
                                                                            <strong>Total:</strong> <span class="green font-size-17"><strong>$<asp:Label runat="server" ID="total_"></asp:Label>
                                                                            </strong></span>
                                                                        </p>
                                                                        <p>
                                                                            <br />
                                                                            <%--<a class="btn btn-success btn-xlarge" href="#">Start Payment Process</a>--%>
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
                            </div>
                        </div>
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
        <a id="btn-scrollup" class="btn btn-circle btn-lg" href="#"></a>
    </form>
    <!--basic scripts-->
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script>window.jQuery || document.write('<script src="assets/jquery/jquery-2.1.1.min.js"><\/script>')</script>
    <script src="assets/bootstrap/js/bootstrap.min.js"></script>
    <script src="assets/jquery-slimscroll/jquery.slimscroll.min.js"></script>
    <script src="assets/jquery-cookie/jquery.cookie.js"></script>

    <!--page specific plugin scripts-->
    <script src="assets/flot/jquery.flot.js"></script>
    <script src="assets/flot/jquery.flot.resize.js"></script>
    <script src="assets/flot/jquery.flot.pie.js"></script>
    <script src="assets/flot/jquery.flot.stack.js"></script>
    <script src="assets/flot/jquery.flot.crosshair.js"></script>
    <script src="assets/flot/jquery.flot.tooltip.min.js"></script>



    <script src="js/jquery.tablesorter.js"></script>


    <link rel="stylesheet" type="text/css" href="assets/bootstrap-fileupload/bootstrap-fileupload.css" />
    <script src="assets/sparkline/jquery.sparkline.min.js"></script>

    <script src="assets/gritter/js/jquery.gritter.js"></script>
    <script src="js/jquery.tooltipster.js"></script>

    <script src="js/jquery.tooltipster.min.js"></script>

    <!--flaty scripts-->
    <script src="js/flaty.js"></script>

    <script src="js/DataTable/jquery.dataTables.js"></script>
    <script src="js/DataTable/date.js"></script>


    <script src="CHOSEN/chosen.jquery.js" type="text/javascript"></script>
    <script src="CHOSEN/docsupport/prism.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript">
        var config = {
            '.chosen-select': {},
            '.chosen-select-deselect': { allow_single_deselect: true },
            '.chosen-select-no-single': { disable_search_threshold: 10 },
            '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' },
            '.chosen-select-width': { width: "90%" }
        }
        for (var selector in config) {
            $(selector).chosen(config[selector]);
        }



    </script>



    <script type="text/javascript" src="js/dataTables.tableTools.js"></script>

    <link href="js/theme.default.min.css" rel="stylesheet" />
    <script src="js/highcharts.js"></script>
    <script src="js/modules/exporting.js"></script>
    <script src="js/modules/exporting.src.js"></script>
    <script src="js/modules/data.js"></script>
    <!-- CALENDAR -->
    <script src="fullcalendar/lib/moment.min.js"></script>
    <script src="fullcalendar/fullcalendar.js"></script>
    <script src="fullcalendar/lang/es.js"></script>
    <script src="fullcalendar/lang-all.js"></script>
    <script src="assets/bootstrap-timepicker/js/bootstrap-timepicker.js"></script>
</body>
