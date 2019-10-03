<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" EnableEventValidation="false" AutoEventWireup="true" UICulture="es-ES" Culture="es-ES" CodeBehind="REPORTE_CM.aspx.cs" Inherits="SoprodiApp.REPORTE_CM" %>

<%@ OutputCache Location="None" NoStore="true"%>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">

    <script>

        $().ready(function () {

            superfiltro();



        });
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

            var elem2 = document.getElementById("<%=btn_productos.ClientID%>");
            elem2.style.visibility = "hidden";
            var elem2 = document.getElementById("<%=btn_informe.ClientID%>");
            elem2.style.visibility = "hidden";
            var elem3 = document.getElementById("<%=cargando_gif.ClientID%>");
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
            background-color: #428bca;
            color: white;
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
    </style>

    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_excel2" />
            <asp:PostBackTrigger ControlID="btn_excel" />
        </Triggers>
        <ContentTemplate>

            <script type="text/javascript">



                Sys.Application.add_load(BindEvents);

                function BindEvents() {

                    
                    $().ready(function () {

                        superfiltro();


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
                        parameters.usuario = document.getElementById("<%=l_usuario_.ClientID %>").value;
                        parameters = JSON.stringify(parameters)

                        $.ajax({
                            type: "POST",
                            url: "REPORTE_CM.aspx/VENDEDOR_POR_GRUPO",
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
                            url: "REPORTE_CM.aspx/CLIENTE_POR_VENDEDOR",
                            data: parameters,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: LoadClientes,
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert("Error al cargar clientes");
                            }
                        });

                        $("#<%=d_cliente.ClientID%>").chosen();
                        $("#<%=d_cliente.ClientID%>").trigger("chosen:updated");

                        $("#<%=d_vendedor_.ClientID%>").chosen();
                        $("#<%=d_vendedor_.ClientID%>").trigger("chosen:updated");




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
                            url: "REPORTE_CM.aspx/CLIENTE_POR_VENDEDOR",
                            data: parameters,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: LoadClientes,
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert("Error al cargar clientes");
                            }
                        });

                    });

                    $("#<%=d_cliente.ClientID%>").chosen();
                    $("#<%=d_cliente.ClientID%>").trigger("chosen:updated");
                    $("#<%=d_vendedor_.ClientID%>").chosen();
                    $("#<%=d_vendedor_.ClientID%>").trigger("chosen:updated");




                    $("#<%=d_cliente.ClientID %>").change(function () {
                        var arr = $(this).val();
                        document.getElementById("<%=l_clientes.ClientID %>").value = arr;
                    })

                }

            </script>
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
                        <a runat="server" id="titulo2"></a>
                        <span class="divider"><i class="fa fa-angle-right"></i></span>
                    </li>
                    <li class="active">Reporte análisis nuevo cliente</li>
                </ul>
            </div>
            <div id="main-content">
                <!-- BEGIN Main Content -->
                <div class="row">
                    <div class="col-md-12">
                        <div class="box">
                            <div class="box-title">
                                <h3><i class="fa fa-table"></i>Análisis Clientes Nuevos</h3>
                                <div class="box-tool">
                                    <a data-action="collapse" href="#"> </a>

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
                                                            <div class="controls">
                                                                <asp:TextBox ID="txt_desde" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_desde" Format="dd/MM/yyyy" />
                                                            </div>
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
                                                           <div runat="server" id="div_sw_permiso" style="display:block;">
                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Grupo</label>
                                                                <div class="col-sm-3 col-lg-3">
                                                                    <div class="controls">
                                                                        <asp:ListBox ID="d_grupos_usuario" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Vendedor</label>
                                                            <div class="col-sm-3 col-lg-3" runat="server">
                                                                <div class="controls">
                                                                    <asp:ListBox ID="d_vendedor_" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-1 col-lg-1" runat="server" visible="false">
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
                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Percentil</label>
                                                <div class="col-sm-1 col-lg-1">
                                                    <div class="input-group" style="width: 80px">
                                                        <asp:TextBox ID="t_percentil" runat="server" Text="50" class="form-control" Style="width: 100%" onkeypress="CheckNumeric(event);" MaxLength="2"></asp:TextBox>
                                                        <span class="input-group-addon">%</span>

                                                    </div>
                                                </div>

                                                <div class="pull-right">
                                                    <div class="btn-group">
                                                        <asp:Button ID="btn_productos" runat="server" OnClientClick="CARGANDO();" Style="color: white; float: right;" Class="btn btn-success" OnClick="btn_productos_Click" Text="NuevosClientes" />
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
                </div>
                <div id="div1" runat="server" class="row" style="visibility: visible">
                    <div id="div_report" runat="server" class="row" visible="false">
                        <div class="col-md-12">
                            <div class="box">
                                <div class="box-title">
                                    <h3><i class="fa fa-table"></i>Detalle CM</h3>
                                    <div class="box-tool">
                                        <a data-action="collapse" href="#"> </a>

                                    </div>
                                </div>
                                <div class="box-content">
                                    <div class="btn-toolbar pull-right">

                                        <div class="btn-group">
                                            <asp:LinkButton ID="btn_excel" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Exportar a Excel" runat="server" OnClick="btn_excel_Click"></asp:LinkButton>
                                        </div>
                                    </div>

                                    <div class="clearfix"></div>
                                    <div class="table-responsive" style="border: 0">
                                        <%--<asp:TextBox ID="Txt_VENDEDOR" runat="server" Text="" OnTextChanged="Unnamed_TextChanged"></asp:TextBox>--%>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box">
                                                    <div class="form-group">
                                                        <asp:GridView ID="G_INFORME_TOTAL_VENDEDOR" CssClass="table table-bordered" OnRowDataBound="G_INFORME_TOTAL_VENDEDOR_RowDataBound" runat="server" Visible="false"
                                                            ShowHeaderWhenEmpty="True" Font-Size="12px">
                                                            <HeaderStyle CssClass="test" />
                                                            <Columns>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                No existen datos.
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>

                                                        <asp:GridView ID="G_INFORME_VENDEDOR" CssClass="table table-bordered" OnRowDataBound="G_INFORME_VENDEDOR_RowDataBound" runat="server" Visible="false"
                                                            ShowHeaderWhenEmpty="True" Font-Size="12px">
                                                            <HeaderStyle CssClass="test" />
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
                        </div>
                    </div>

                </div>
                <div id="div2" runat="server" class="row" style="visibility: visible">
                    <div id="div_productos" runat="server" class="row" visible="false">
                        <div class="col-md-12">
                            <div class="box">
                                <div class="box-title">
                                    <h3><i class="fa fa-table"></i>Clientes Nuevos por vendedor</h3>
                                    <div class="box-tool">
                                        <a data-action="collapse" href="#"> </a>
                                     
                                    </div>
                                </div>
                                <div class="box-content">
                                    <div class="btn-toolbar pull-left">
                                        <input type="text" id="t_filtro_memoria" style="width: 200px; margin-right: 7px; padding: 5px;" placeholder="Filtrar..." class="form-control" onchange="changevalue();" onkeyup="changevalue();" />

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
                                                    <div class="form-group" style="overflow-x: auto;">
                                                        <label class="label" style="top: 5px;">Total nuevos</label>
                                                        <asp:Label runat="server" ID="l_total_nuevos"></asp:Label>

                                                        <label class="label" style="top: 5px; margin-left: 7%;">#</label>
                                                        <asp:Label runat="server" ID="l_cont"></asp:Label>

                                                        <asp:GridView ID="G_PRODUCTOS" CssClass="table table-bordered filtrar" OnRowDataBound="G_PRODUCTOS_RowDataBound" runat="server" Visible="true"
                                                            ShowHeaderWhenEmpty="True" Font-Size="12px" DataKeyNames="rutcliente">
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
    <asp:TextBox ID="l_usuario_" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_grupos" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_vendedores" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_clientes" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_productos" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_periodos" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>

</asp:Content>
