<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" AutoEventWireup="True" UICulture="es-ES" Culture="es-ES" CodeBehind="REPORTE_EXCEL_COSTOS.aspx.cs" Inherits="SoprodiApp.REPORTE_EXCEL_COSTOS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ OutputCache Location="None" NoStore="true" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">
    <script>

        $().ready(function () {
            //superfiltro();
            //LlenarCalendario();
            document.querySelector('form').onkeypress = checkEnter;
            creagrilla();
            //SORT_GRILLA();
        });

        //function SORT_GRILLA() {
        //    new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_PRODUCTOS'));
        //}
        <%--   function LoadProduc_kg(result) {

            //quito los options que pudiera tener previamente el combo

            $("#<%=cb_productos_kg.ClientID%>").html("");


            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {

                $("#<%=cb_productos_kg.ClientID%>").append($("<option></option>").attr("value", this.COD_PROD).text(this.NOM_PROD))

               });


               $("#<%=cb_productos_kg.ClientID%>").chosen();
            $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");



        }--%>
     <%--   function LoadProduc(result) {

            //quito los options que pudiera tener previamente el combo

            $("#<%=d_producto.ClientID%>").html("");


               //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
               $.each(result.d, function () {

                   $("#<%=d_producto.ClientID%>").append($("<option></option>").attr("value", this.COD_PROD).text(this.NOM_PROD))

            });


            $("#<%=d_producto.ClientID%>").chosen();
               $("#<%=d_producto.ClientID%>").trigger("chosen:updated");



           }--%>

        function creagrilla() {
            try {
                $("#G_INFORME_TOTAL_VENDEDOR").DataTable({
                    "lengthChange": false,
                    "searching": false,
                    "destroy": true,
                    "pageLength": -1,
                    "paging": false,
                    "aoColumnDefs": [{
                        "bSortable": false,
                        "aTargets": ["sorting_disabled"]
                    }],

                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    }
                    , "ordering": false

                });

                super_ff();
            } catch (e) { }

        }

        function checkEnter(e) {
            if (e.keyCode == 13) {

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
        function stringtojson(string) {
            //alert(string);
            return eval('(' + string + ')');
        }

        function format(input) {
            var num = input.value.replace(/\./g, '');
            if (!isNaN(num)) {
                num = num.toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1.');
                num = num.split('').reverse().join('').replace(/^[\.]/, '');
                input.value = num;
            }

            else {
                alert('Solo se permiten numeros');
                input.value = input.value.replace(/[^\d\.]*/g, '');
            }
        }

        function grafico(codigo, fecha_, columna_) {
            //alert(cod + "--" + fecha + " -- " + colum);
            var parameters = new Object();


            //string cod, string fecha, string columna

            parameters.cod = codigo;
            parameters.fecha = fecha_;
            parameters.columna = columna_;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_EXCEL_COSTOS.aspx/historial_Excel",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,

            }).done(function (resp) {
                resp = resp.d;

                var encabezados = stringtojson(resp.split('*')[0]);
                var encabezados2 = stringtojson(resp.split('*')[1]);
                //alert(encabezados2);
                $('#moda_35').click();
                $(function () {
                    //$('#container2').highcharts({
                    var chart = Highcharts.chart('container2', {

                        chart: {
                            type: 'line'
                        },
                        title: {
                            text: 'HISTORIAL DE COSTOS'
                        },
                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            categories: encabezados2

                        },
                        yAxis: {
                            title: {
                                text: 'COSTO'
                            }
                        },
                        plotOptions: {
                            line: {
                                dataLabels: {
                                    enabled: true
                                },
                                enableMouseTracking: false
                            }
                        },

                        series:
                            encabezados

                    });
                    chart.setSize(1200, 500);
                });




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


        function producto_cambio_humano() {



            var parameters = new Object();

            parameters.sw = "humano";

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_EXCEL_COSTOS.aspx/PRODUCTO_CAMBIO",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadProduc,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al cargar bodegas");
                }
            });


            var parameters = new Object();

            parameters.sw = "humano";

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_EXCEL_COSTOS.aspx/PRODUCTO_CAMBIO2",
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
                url: "REPORTE_EXCEL_COSTOS.aspx/PRODUCTO_CAMBIO",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadProduc,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al cargar bodegas");
                }
            });



            var parameters = new Object();

            parameters.sw = "animal";

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_EXCEL_COSTOS.aspx/PRODUCTO_CAMBIO2",
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


            <%--               var elem1 = document.getElementById("<%=txt_hasta.ClientID%>");
            elem1.style.zIndex = "0";--%>

            <%--            var elem5 = document.getElementById("<%=div1.ClientID%>");
            elem5.style.visibility = "hidden";--%>
      <%--      var elem6 = document.getElementById("<%=div2.ClientID%>");
            elem6.style.visibility = "hidden";--%>


            var elem3 = document.getElementById("<%=cargando_gif.ClientID%>");
            elem3.style.display = "block";

        }
        function CARGANDO_CLOSE() {


<%--            var elem1 = document.getElementById("<%=txt_hasta.ClientID%>");
            elem1.style.zIndex = "0";

            var elem5 = document.getElementById("<%=div1.ClientID%>");
            elem5.style.visibility = "visible";
            var elem6 = document.getElementById("<%=div2.ClientID%>");
            elem6.style.visibility = "visible";--%>


            var elem3 = document.getElementById("<%=cargando_gif.ClientID%>");
            elem3.style.display = "none";

        }



        function CARGA_FECHA() {

<%--            var elem2 = document.getElementById("<%=b.ClientID%>");
            elem2.style.display = "none";
            var elem3 = document.getElementById("<%=carga_fecha.ClientID%>");
            elem3.style.display = "block";--%>
        }

        function NO_VENDEDORES() {

            alert("No existe vendedores para periodos o ha ingresado una fecha errónea");
        }


        function NO_GRUPOS() {

            alert("No existe ventas para periodos o ha ingresado una fecha errónea");
        }
        function modal_unidad() {

            document.getElementById("div_unidad_").click();
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


        function SIDEBAR_NO_DIARIO() {


            document.getElementById("REPORTE").className = "";

            document.getElementById("REPORTE_VENDEDOR").className = "";
            document.getElementById("REPORTE_SALA").className = "";
            document.getElementById("REPORTE_CM").className = "";
            document.getElementById("REPORTE_COMPARATIVO").className = "";
            try {
                document.getElementById("diario_li").style.display = "none";
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

        #container2 {
            height: 500px;
            min-width: 510px;
            max-width: 1250px;
        }
    </style>

    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="900000" EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_excel" />
            <asp:PostBackTrigger ControlID="bn_subir_excel" />
        </Triggers>
        <ContentTemplate>

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
                    <li class="active">Excel Costo</li>
                </ul>
            </div>

            <div id="main-content">
                <!-- BEGIN Main Content -->
                <div class="row">
                    <div class="col-md-12">
                        <div class="box">
                            <div class="box-title">
                                <h3><i class="fa fa-table"></i>Costo Excel</h3>
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
                                                    <label class="col-sm-1 control-label">
                                                        <b>Día de Excel</b>
                                                    </label>
                                                    <div class="col-sm-6 controls">

                                                        <asp:TextBox ID="txt_desde" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_desde" Format="dd/MM/yyyy" />



                                                    </div>
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
                                                        <%--<asp:Button ID="btn_informe" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-primary" OnClick="btn_informe_Click" Text="Reporte" />--%>
                                                        <asp:Button ID="Button1" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-primary" OnClick="btn_informe_Click" Text="Reporte" />
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
                    <div class="col-md-5">
                        <div class="box box-green">
                            <div class="box-title">
                                <h3><i class="fa fa-check"></i>SUBIR LISTA DE PRECIO</h3>
                                <div class="box-tool">
                                </div>
                            </div>
                            <div class="box-content" style="height: 100px;">


                                <div class="form-group">
                                    <label class="col-sm-3 col-lg-2 control-label">Excel</label>
                                    <div class="col-sm-9 col-lg-10 controls">
                                        <asp:FileUpload runat="server" ID="fu_listaprecio" CssClass="form-control" />
                                    </div>
                                    <div class="col-sm-9 col-lg-10 controls">
                                    </div>


                                </div>
                                <asp:Button ID="bn_subir_excel" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-primary" OnClick="bn_subir_excel_Click" Text="Subir" />

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
                                    <div class="btn-toolbar pull-right">

                                        <div class="btn-group">
                                            <asp:LinkButton ID="btn_excel" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Exportar a Excel" runat="server" OnClick="btn_excel2_Click"></asp:LinkButton>
                                        </div>
                                    </div>

                                    <label runat="server" id="lb_fecha_exc"></label>
                                    <div class="box-content">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box">

                                                    <asp:GridView ID="G_EXCEL_COSTO" ClientIDMode="Static" CssClass="table table-bordered filtrar" OnRowDataBound="G_EXCEL_COSTO_RowDataBound" runat="server"
                                                        ShowHeaderWhenEmpty="True" Font-Size="12px">
                                                        <HeaderStyle CssClass="test no-sort" />
                                                        <Columns>
                                                            <%--  <asp:TemplateField HeaderText="">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkAccept" runat="server" EnableViewState="true" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>--%>
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



                <a id="moda_35" name="moda_35" href="#modall_35" role="button" class="btn" data-toggle="modal" style="display: none;"></a>
                <div id="modall_35" class="modal fade">
                    <div class="modal-dialog modal-lg" style="width: 84%;">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" id="Button75" class="close" data-dismiss="modal" aria-hidden="true">×</button>

                            </div>
                            <div class="modal-body">
                                <div class="row">

                                    <div class="col-sm-12">
                                        <h3>Gráfico</h3>
                                        <%--  <div class="col-sm-6">
                                        <div class="form-horizontal">

                                            <div id="container" style="min-width: 510px; height: 400px; margin: 0 auto">
                                            </div>
                                        </div>
                                    </div>--%>

                                        <div class="form-horizontal">

                                            <div id="container2">
                                            </div>
                                        </div>


                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button class="btn" data-dismiss="modal" aria-hidden="true" id="Button83">Cerrar</button>
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
    <asp:TextBox ID="lb_bodegas2" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>



    <%-- <a id="btn-scrollup" class="btn btn-circle btn-lg" href="#"> </a>--%>
</asp:Content>
