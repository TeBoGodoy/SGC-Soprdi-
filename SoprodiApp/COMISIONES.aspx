<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="True" UICulture="es-ES" Culture="es-ES" CodeBehind="COMISIONES.aspx.cs" Inherits="SoprodiApp.COMISIONES" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ OutputCache Location="None" NoStore="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">
    <script>


        $(document).ready(function () {

            try {
                superfiltro();
                superfiltro2();
            } catch (e) { }

        });


        //function SORT_GRILLA() {
        //    new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_PRODUCTOS'));
        //}

        function LoadVendedores(result) {

            //quito los options que pudiera tener previamente el combo

            $("#<%=d_vendedor_.ClientID%>").html("");


            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {

                $("#<%=d_vendedor_.ClientID%>").append($("<option></option>").attr("value", this.cod_vendedor).text(this.nom_vendedor))

            });


            $("#<%=d_vendedor_.ClientID%>").chosen();
            $("#<%=d_vendedor_.ClientID%>").trigger("chosen:updated");


        }

        function SortPrah() {
            $(document).ready(function () {
                $("#TABLA_REPORTE").tablesorter({
                    usNumberFormat: false
                });
                $("#TABLA_REPORTE2").tablesorter({
                    usNumberFormat: false,
                    sortList: [[3, 0]]
                });
            });
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

        function superfiltro2() {
            try {
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
            } catch (e) { }
        }
        function CARGA_FECHA() {

      
            var elem3 = document.getElementById("<%=carga_fecha.ClientID%>");
            elem3.style.display = "block";
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

        function CARGANDO() {

            var elem2 = document.getElementById("<%=btn_productos.ClientID%>");
            elem2.style.visibility = "hidden";

            var elem3 = document.getElementById("<%=cargando_gif.ClientID%>");
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
                        if (e.keyCode == 44) { } else
                        {
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

        .Grheader {
            overflow: hidden;
            height: 82px;
            position: relative;
            top: 0px;
            z-index: 10;
            vertical-align: top;
            margin-right: 17px;
        }

        .GrCuerpo {
            width: 100%;
            position: relative;
            top: -82px;
            z-index: 1;
            overflow: auto;
            height: 100%;
        }
    </style>
    <script src="js/jquery.fixedTblHdrLftCol.js"></script>
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
        <Triggers>

            <asp:PostBackTrigger ControlID="LinkButton1" />

        </Triggers>
        <ContentTemplate>

            <script src="assets/gritter/js/jquery.gritter.js"></script>


            <script type="text/javascript">



                Sys.Application.add_load(BindEvents);

                function BindEvents() {

                    superfiltro();

                    $(document).ready(function () {
                        $('[data-toggle="tooltip"]').tooltip();
                    });

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
                        parameters = JSON.stringify(parameters)

                        $.ajax({
                            type: "POST",
                            url: "REPORTE_LV_C.aspx/GRUPO_VENDEDOR",
                            data: parameters,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: LoadVendedores,
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert("Error al cargar vendedores");
                            }
                        });

                        $("#<%=d_vendedor_.ClientID%>").chosen();
                        $("#<%=d_vendedor_.ClientID%>").trigger("chosen:updated");


                    });


                    $("#<%=d_vendedor_.ClientID%>").change(function () {

                        // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                        //este parametro mapeara con el definido en el web service

                        var arr = $(this).val();
                        document.getElementById("<%=l_vendedores.ClientID %>").value = arr;

                    });



                    $("#<%=d_vendedor_.ClientID%>").chosen();
                    $("#<%=d_vendedor_.ClientID%>").trigger("chosen:updated");



                }

            </script>
            <asp:TextBox ID="l_grupos" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
            <asp:TextBox ID="l_vendedores" runat="server" Style="visibility: hidden; position: absolute; left: 500PX"></asp:TextBox>

            <%--  <div class="page-title" style="margin-top: -27px">
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
                        <a href="MENU_finanzas.aspx">Finanzas</a>
                        <span class="divider"><i class="fa fa-angle-right"></i></span>
                    </li>
                       <%--  <li>
                        <a href="menu_comisiones.aspx">Comisiones</a>
                        <span class="divider"><i class="fa fa-angle-right"></i></span>
                    </li>--%>
                    <li class="active">Comisiones</li>
                </ul>
            </div>
            <div id="main-content">

                <div class="row">
                    <div class="col-md-12">
                        <div class="box" runat="server" id="filtros">
                            <div class="box-title">
                                <h3><i class="fa fa-table"></i>Comisión</h3>
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
                                                    <div class="col-sm-1 col-lg-1 controls" style="visibility:hidden;">
                                                        <%--<asp:ImageButton ID="b" ImageUrl="~/img/Ticket_verde.png" runat="server" OnClientClick="CARGA_FECHA();" OnClick="b_Click" />--%>
                                                        <i class="fa fa-circle-o-notch fa-spin" id="carga_fecha" runat="server" style="font-size: 2em; display: none;"></i>

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

                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Grupo</label>
                                                            <div class="col-sm-3 col-lg-3">
                                                                <div class="controls">
                                                                    <asp:ListBox ID="d_grupos_usuario" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                </div>
                                                            </div>
                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Vendedor</label>
                                                            <div class="col-sm-3 col-lg-3">
                                                                <div class="controls">
                                                                    <asp:ListBox ID="d_vendedor_" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                </div>
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
                                                <div class="pull-right">
                                                    <div class="btn-group">
                                                        <asp:Button ID="btn_productos" runat="server" OnClientClick="CARGANDO();" Style="color: white; float: right;" Class="btn btn-success" OnClick="btn_productos_Click" Text="REPORTE" />
                                                        <%--<asp:Button ID="btn_informe" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-primary" OnClick="btn_informe_Click" Text="Reporte" />--%>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                    </div>
                                    <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_gif" runat="server" style="display: none; font-size: 3em;"></i>


                                </div>
                            </div>
                        </div>
                        <div id="div1" runat="server" class="row" style="visibility: visible">
                            <div id="div3" runat="server" class="row" visible="false">
                                <div class="col-md-12">
                                    <div class="box">
                                        <div class="box-title">
                                            <h3><i class="fa fa-table"></i>Detalle diario de grupos</h3>
                                            <div class="box-tool">
                                                <a data-action="collapse" href="#"></a>

                                            </div>
                                        </div>
                                        <div class="box-content">
                                            <div id="filtro_memoria_div" runat="server"></div>
                                            <div class="btn-toolbar pull-right">

                                                <div class="btn-group">
                                                    <asp:LinkButton ID="LinkButton1" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Exportar a Excel" runat="server" OnClick="LinkButton1_Click"></asp:LinkButton>
                                                </div>
                                            </div>

                                            <div class="clearfix"></div>
                                            <div class="table-responsive" style="border: 0">
                                                <%--<asp:TextBox ID="Txt_VENDEDOR" runat="server" Text="" OnTextChanged="Unnamed_TextChanged"></asp:TextBox>--%>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="box">
                                                            <div class="form-group">
                                                                <div class="DivRoot" id="Div5" runat="server" visible="false">
                                                                    <%-- <div class="Grheader" runat="server" id="DivHeaderRow1">
                                                                    </div>--%>
                                                                    <div runat="server" style="overflow-y: auto" id="DivMainContent1">
                                                                    </div>
                                                                    <div runat="server" id="R_Excel_1" style="display: none">
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


            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
