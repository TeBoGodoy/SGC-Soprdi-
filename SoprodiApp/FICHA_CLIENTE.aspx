<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" EnableEventValidation="false" AutoEventWireup="True" UICulture="es-ES" Culture="es-ES" CodeBehind="FICHA_CLIENTE.aspx.cs" Inherits="SoprodiApp.FICHA_CLIENTE" %>

<%@ OutputCache Location="None" NoStore="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">
    <link href="assets/jquery-tags-input/jquery.tagsinput.css" rel="stylesheet" />
    <link href="assets/bootstrap-wysihtml5/bootstrap-wysihtml5.css" rel="stylesheet" />
    <script src="js/sorts/tablesort.date.js"></script>
    <script>

        $().ready(function () {

            superfiltro();

        });


        function creagrilla() {
            $("#g_nuevos_clientes1").DataTable({
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

        function checkEnter(e) {
            e = e || event;
            var txtArea = /textarea/i.test((e.target || e.srcElement).tagName);
            return txtArea || (e.keyCode || e.which || e.charCode || 0) !== 13;
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
                window.open(urlPdf_Final, '', 'menubar=no, width=1100,height=500,left=50,top=50');
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

        function superfiltro() {

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


        function superfiltro2() {

            $(".filtra tr:has(td)").each(function () {
                var t = $(this).text().toLowerCase();
                $("<td class='indexColumn'></td>")
                .hide().text(t).appendTo(this);
            });
            //Agregar el comportamiento al texto (se selecciona por el ID) 
            $("#t_filtra").keyup(function () {
                var s = $(this).val().toLowerCase().split(" ");
                $(".filtra tr:hidden").show();
                $.each(s, function () {
                    $(".filtra tr:visible .indexColumn:not(:contains('"
                    + this + "'))").parent().hide();
                });
            });

        }
        function superfiltro3() {

            $(".filtra3 tr:has(td)").each(function () {
                var t = $(this).text().toLowerCase();
                $("<td class='indexColumn'></td>")
                .hide().text(t).appendTo(this);
            });
            //Agregar el comportamiento al texto (se selecciona por el ID) 
            $("#t_filtra3").keyup(function () {
                var s = $(this).val().toLowerCase().split(" ");
                $(".filtra3 tr:hidden").show();
                $.each(s, function () {
                    $(".filtra3 tr:visible .indexColumn:not(:contains('"
                    + this + "'))").parent().hide();
                });
            });

        }

        function CARGANDO() {
            try {
                var elem3 = document.getElementById("<%=cargando_gif.ClientID%>");
                elem3.style.display = "block";
            } catch (e) { }
            try {
                var elem4 = document.getElementById("<%=cargando2.ClientID%>");
                elem4.style.display = "block";
            } catch (e) { }

            try {
                var elem22 = document.getElementById("<%=carga_edit.ClientID%>");
                elem22.style.display = "block";
            } catch (e) { }
            try {
                var elem232 = document.getElementById("<%=crear_cargando.ClientID%>");
                elem232.style.display = "block";
            } catch (e) { }


        }
        function CARGA_FECHA() {

            var elem2 = document.getElementById("ContentPlaceHolder_Contenido_ImageButton1");
            elem2.style.display = "none";
            var elem3 = document.getElementById("ContentPlaceHolder_Contenido_I1");
            elem3.style.display = "block";
        }
        function NO_VENDEDORES() {

            alert("No existe vendedores para periodos o ha ingresado una fecha errónea");
        }


        function NO_GRUPOS() {

            alert("No existe ventas para periodos o ha ingresado una fecha errónea");
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

        function fuera(rutcliente, bit) {


            var urlPdf = "/FICHA_C.aspx?";
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

        function fuera1(fecha, vendedor, grupos, bit) {


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
    </style>

    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel33" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_excel" />
            <asp:AsyncPostBackTrigger ControlID="excel" />
            <asp:AsyncPostBackTrigger ControlID="excel2" />
            <asp:PostBackTrigger ControlID="b_edSit" />
            <asp:PostBackTrigger ControlID="b_doc_abiertos" />
        </Triggers>
        <ContentTemplate>

            <script type="text/javascript">

                Sys.Application.add_load(BindEvents);

                function BindEvents() {

                    superfiltro();


                    $("#<%=vendedor_NEW_.ClientID%>").chosen();
                    $("#<%=vendedor_NEW_.ClientID%>").trigger("chosen:updated");

                    $("#<%=vendedor_NEW_.ClientID%>").change(function () {

                        // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                        //este parametro mapeara con el definido en el web service

                        var arr = $(this).val();
                        document.getElementById("<%=l_vendedores.ClientID %>").value = arr;


                    });



                }

            </script>

            <%--  <div class="page-title" style="margin-top: -27px">
                <div>
                    <i class="fa fa-file-o fa-3x"></i><a class="h1" href="FICHA_CLIENTE.aspx">Ficha Cliente</a>
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
                        <li class="active">Ficha Cliente</li>

                    </li>
                    <%--           <li class="active">Ficha Cliente</li>--%>
                </ul>
            </div>

            <asp:MultiView ID="CLIENTES_FICHA" runat="server" ActiveViewIndex="0">
                <%-- 0 clientes todos --%>
                <asp:View ID="Grilla" runat="server">

                    <div id="main-content">
                        <!-- BEGIN Main Content -->
                        <div class="row" style="display: block;">
                            <div class="col-md-12">
                                <div class="box">
                                    <div class="box-title">
                                        <h3><i class="fa fa-table"></i>Ficha Cliente</h3>
                                        <div class="box-tool">
                                            <a data-action="collapse" href="#"></a>

                                        </div>
                                    </div>
                                    <div class="box-content">
                                        <div class="clearfix"></div>
                                        <div class="table-responsive" style="border: 0">

                                            <div class="row" style="display: block;">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="form-group">
                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Ventas Desde</label>

                                                            <div class="col-sm-3 col-lg-3 controls">
                                                                <div class="input-group">

                                                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                    <div class="controls">
                                                                        <asp:TextBox ID="txt_desde" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_desde" Format="dd/MM/yyyy" />
                                                                    </div>
                                                                </div>
                                                            </div>



                                                            <div class="pull-right">
                                                                <div class="btn-group">
                                                                    <asp:LinkButton ID="nuevo_cliente" class="btn btn-circle btn-success fa fa-plus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Nuevo Cliente" runat="server" OnClick="nuevo_cliente_Click"></asp:LinkButton>
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
                                                            <div class="col-sm-1 col-lg-1 controls" style="display: none;">
                                                                <asp:ImageButton ID="b" ImageUrl="~/img/Ticket_verde.png" runat="server" OnClientClick="CARGA_FECHA();" OnClick="b_Click1" />
                                                                <i class="fa fa-circle-o-notch fa-spin" id="carga_fecha" runat="server" style="font-size: 2em; display: none;"></i>

                                                            </div>



                                                        </div>
                                                    </div>
                                                </div>
                                            </div>



                                            <div class="row" style="display: block;">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="form-group">

                                                            <div runat="server" id="div_sw_permiso" style="display: none;">
                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px; display: block;">$ Mayor a </label>
                                                                <div class="col-sm-3 col-lg-3" style="display: block;">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">$</span>
                                                                        <asp:TextBox runat="server" ID="mayor_" class="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px; display: none;">Vendedor</label>
                                                            <div id="Div1" class="col-sm-3 col-lg-3" runat="server" style="display: none;">
                                                                <div class="controls">
                                                                    <asp:ListBox ID="d_vendedor_" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                </div>
                                                            </div>
                                                            <div id="Div2" class="col-sm-1 col-lg-1" runat="server" visible="false">
                                                                <div class="controls">
                                                                    <%-- <asp:ImageButton ID="b" ImageUrl="~/img/Ticket_verde.png" Width="30%" runat="server" OnClick="btn_filtro_vend_Click" />--%>
                                                                </div>
                                                            </div>
                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px; display: none;">Cliente</label>
                                                            <div class="col-sm-3 col-lg-3" style="display: none;">
                                                                <div class="controls">
                                                                    <asp:ListBox ID="d_cliente" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-3 col-lg-3 controls" style="display: none;">

                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;"></label>

                                                            </div>

                                                        </div>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box" style="display: block;">
                                                        <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Promedio Venta Meses</label>
                                                        <div class="col-sm-1 col-lg-1">
                                                            <div class="input-group" style="width: 80px">
                                                                <asp:TextBox ID="t_meses" runat="server" Text="3" class="form-control" Style="width: 100%" onkeypress="CheckNumeric(event);" MaxLength="2"></asp:TextBox>


                                                            </div>
                                                        </div>

                                                        <div class="pull-right">
                                                            <div class="btn-group">
                                                                <asp:Button ID="btn_productos" ClientIDMode="Static" runat="server" OnClientClick="CARGANDO();" Style="color: white; float: right;" Class="btn btn-success" OnClick="btn_productos_Click" Text="NuevosClientes" />
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

                        <div id="div_report" runat="server" class="row" visible="false">
                            <div class="col-md-12">
                                <div class="box">
                                    <div class="box-title">
                                        <h3><i class="fa fa-table"></i>Fichas Clientes</h3>
                                        <div class="box-tool">
                                            <a data-action="collapse" href="#"></a>

                                        </div>
                                    </div>
                                    <div class="box-content">
                                        <div class="btn-toolbar pull-left input-group">
                                            <input type="text" id="t_filtro_memoria" style="width: 200px; margin-right: 7px; padding: 5px;" placeholder="Filtrar..." class="form-control" />
                                            <button class="btn btn-primary" type="button" id="btn_filtro_nuevo"><i class="fa fa-search"></i></button>
                                        </div>
                                        <div class="btn-toolbar pull-right">

                                            <div class="btn-group">
                                                <asp:LinkButton ID="btn_excel" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Editar" runat="server" OnClick="btn_excel_Click"></asp:LinkButton>
                                            </div>
                                        </div>

                                        <div class="clearfix"></div>
                                        <div class="table-responsive" style="border: 0">
                                            <%--<asp:TextBox ID="Txt_VENDEDOR" runat="server" Text="" OnTextChanged="Unnamed_TextChanged"></asp:TextBox>--%>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="form-group">
                                                            <asp:GridView ID="G_INFORME_TOTAL_VENDEDOR" CssClass="table table-bordered filtrar" OnRowDataBound="G_INFORME_TOTAL_VENDEDOR_RowDataBound" OnRowCommand="G_INFORME_TOTAL_VENDEDOR_RowCommand" runat="server" Visible="false"
                                                                ShowHeaderWhenEmpty="True" ShowHeader="true" Font-Size="12px" DataKeyNames="rutcliente, L.Crédito, L.Cr.Disponible">
                                                                <HeaderStyle CssClass="test no-sort" />

                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="#" HeaderStyle-Wrap="false" ItemStyle-Width="4px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="5px" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <%--    <asp:TemplateField HeaderText="Ficha" HeaderStyle-Wrap="false" ItemStyle-Width="4px" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton runat="server" ID="b_edit" ImageUrl="img/user.png" Width="17"
                                                                                    CommandName="ficha" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="5px" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    --%>
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

                        <div id="div_nuevos" runat="server" class="row" visible="false">
                            <div class="col-md-12">
                                <div class="box">
                                    <div class="box-title">
                                        <h3><i class="fa fa-table"></i>Nuevos Clientes</h3>
                                        <div class="box-tool">
                                            <a data-action="collapse" href="#"></a>

                                        </div>
                                    </div>
                                    <div class="box-content">
                                        <div class="btn-toolbar pull-left input-group">
                                            <input type="text" id="Text2" style="width: 200px; margin-right: 7px; padding: 5px;" placeholder="Filtrar..." class="form-control" />
                                            <button class="btn btn-primary" type="button" id="Button2"><i class="fa fa-search"></i></button>
                                        </div>
                                        <div class="btn-toolbar pull-right">

                                            <div class="btn-group">
                                                <asp:LinkButton ID="LinkButton8" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Editar" runat="server" OnClick="btn_excel_Click"></asp:LinkButton>
                                            </div>
                                        </div>

                                        <div class="clearfix"></div>
                                        <div class="table-responsive" style="border: 0">
                                            <%--<asp:TextBox ID="Txt_VENDEDOR" runat="server" Text="" OnTextChanged="Unnamed_TextChanged"></asp:TextBox>--%>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="box">
                                                        <div class="form-group">
                                                            <asp:GridView ID="g_nuevos_clientes1" ClientIDMode="Static" CssClass="table table-bordered filtrar" OnRowDataBound="g_nuevos_clientes_RowDataBound1" OnRowCommand="g_nuevos_clientes_RowCommand1" runat="server" Visible="false"
                                                                ShowHeaderWhenEmpty="True" Font-Size="12px" DataKeyNames="rutcliente">
                                                                <HeaderStyle CssClass="test no-sort" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="#" HeaderStyle-Wrap="false" ItemStyle-Width="4px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="5px" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="FichaCompleta" HeaderStyle-Wrap="false" ItemStyle-Width="4px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton runat="server" ID="b_edit" ImageUrl="img/user.png" Width="17"
                                                                                CommandName="ficha" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
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
                </asp:View>

                <%-- 1 Cliente en --%>
                <asp:View ID="View1" runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="box">
                                <div class="box-title">
                                    <h3><i class="fa fa-table"></i>Datos Cliente</h3>
                                    <div class="box-tool">
                                        <a data-action="collapse" href="#"></a>

                                        <button id="btn_volver_usuario" visible="true" style="float: right; margin-top: -6px;" class="btn btn-warning icon-plus btn-circle" runat="server" onserverclick="btn_volver_usuario_ServerClick"><i class="fa fa-arrow-circle-left"></i></button>


                                    </div>
                                </div>
                                <div class="box-content">

                                    <div class="clearfix"></div>
                                    <div class="table-responsive" style="border: 0">
                                        <%--<asp:TextBox ID="Txt_VENDEDOR" runat="server" Text="" OnTextChanged="Unnamed_TextChanged"></asp:TextBox>--%>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box" runat="server" id="div_datos_clientes">
                                                    <div class="form-group">
                                                        <div class="row">
                                                            <div class="col-md-6 company-info">
                                                                <h4>
                                                                    <p>
                                                                        <asp:Label ID="nombrecliente_" runat="server"></asp:Label>
                                                                    </p>
                                                                    <div class="company-info">
                                                                        <p>
                                                                            <i class="fa fa-circle"></i>
                                                                            <asp:Label ID="rutcliente_" runat="server"></asp:Label>
                                                                        </p>
                                                                    </div>

                                                                </h4>
                                                            </div>

                                                            <div class="col-md-3 col-md-offset-3">
                                                                <div class="pull-right">
                                                                    <div class="btn-group">
                                                                        <asp:LinkButton ID="editar" class="btn btn-circle btn-yellow fa fa-pencil" Visible="true" Style="margin-left: 5px" title="Editar Cliente" runat="server" OnClick="editar_Click"></asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div class="row">
                                                            <div class="text-right" style="padding-right: 3%;">
                                                                <b></b>
                                                            </div>
                                                        </div>
                                                        <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                            <div class="panel panel-primary" style="border-radius: 34px;">
                                                                <div class="panel-body">
                                                                    <div class="form-group">
                                                                        <div class="row">


                                                                            <div class="col-md-2 company-info">
                                                                                <p>
                                                                                    <i class="fa fa-user"></i>
                                                                                    <asp:Label ID="vendedor_" runat="server"></asp:Label>
                                                                                    <asp:Label ID="codvendedor" Visible="false" runat="server"></asp:Label>
                                                                                </p>
                                                                            </div>
                                                                            <div class="col-md-2 company-info">
                                                                                <p>
                                                                                    <i class="fa fa-location-arrow"></i>
                                                                                    <asp:Label ID="direccion_" runat="server"></asp:Label>
                                                                                </p>
                                                                            </div>
                                                                            <div class="col-md-3 company-info">
                                                                                <p>
                                                                                    <i class="fa fa-flag"></i>
                                                                                    <asp:Label ID="ciudad_" runat="server"></asp:Label>
                                                                                </p>
                                                                            </div>
                                                                            <div class="col-md-2 company-info">
                                                                                <p>
                                                                                    <i class="fa fa-phone"></i>
                                                                                    <asp:Label ID="fono_" runat="server"></asp:Label>
                                                                                </p>
                                                                            </div>

                                                                            <div class="col-md-2 company-info">
                                                                                <p>
                                                                                    <i class="fa fa-money"></i>
                                                                                    <asp:Label ID="l_credito" runat="server"></asp:Label>
                                                                                </p>
                                                                            </div>


                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>



                                                    </div>
                                                </div>


                                                <%--<asp:LinkButton ID="ficha_completa" class="btn btn-circle btn-yellow fa fa-pencil" Visible="true" Style="margin-left: 5px" title="Editar Cliente" runat="server" OnClick="editar_Click"></asp:LinkButton>--%>
                                                <asp:ImageButton runat="server" ID="b_edSit" ImageUrl="img/user.png" OnClick="b_edSit_Click" Width="17" />
                                                <asp:Label runat="server" ID="ficha_completa" Text="Último Cambio"></asp:Label>

                                                <div class="pull-right">
                                                    <div class="btn-group">
                                                        <asp:ImageButton runat="server" ID="b_doc_abiertos" ImageUrl="img/images.jpg" OnClick="b_doc_abiertos_Click" Width="17" />
                                                        <asp:Label runat="server" ID="Label3" Text="Cobranza"></asp:Label>
                                                    </div>

                                                    <asp:Label runat="server" ID="l_rut_cliente" Style="visibility:hidden;"></asp:Label>

                                                    <%--<a href='REPORTE_SP.aspx?G=91&C=" + r[0].ToString().Trim() + " * " + txt_desde.Text + " * " + txt_hasta.Text + " * " + l_grupos.Text + "' target='_blank'>" + Base.monto_format2(total_stock) + " </a>--%>
                                                    <div class="btn-group">
                                                        <asp:ImageButton runat="server" ID="ImageButton2" Enabled="false" ImageUrl="img/images.jpg" Width="17" />
                                                        <div runat="server" id="div_sp"></div>

                                                    </div>


                                                </div>
                                                <div runat="server" id="muestra_nue" visible="true">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="box">
                                                                <div class="form-group">
                                                                    <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Periodo Desde</label>

                                                                    <div class="col-sm-3 col-lg-3 controls">
                                                                        <div class="input-group">

                                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                                            <asp:TextBox ID="txt_desde2" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txt_desde2" Format="dd/MM/yyyy" />

                                                                        </div>
                                                                    </div>
                                                                    <label class="col-sm-1 col-lg-1 control-label">Hasta</label>
                                                                    <div class="col-sm-3 col-lg-3 controls">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                                            <asp:TextBox ID="txt_hasta2" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txt_hasta2" Format="dd/MM/yyyy" />

                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="pull-right">
                                                        <div class="btn-group">
                                                            <asp:Button ID="btn_cta_corriente" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-success" OnClick="btn_cta_corriente_Click" Text="Histórico" />
                                                            <asp:Button ID="btn_cruzado" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-primary" OnClick="btn_cruzado_Click" Text="Productos" />
                                                            <asp:Button ID="btn_info_clien" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-warning" OnClick="btn_info_clien_Click" Text="Enviar Infor Cliente" />

                                                        </div>
                                                        <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando2" runat="server" style="display: none; font-size: 3em;"></i>

                                                    </div>
                                                    <asp:Label runat="server" Style="color: red;" ID="lb_mensaj"></asp:Label>
                                                    <div runat="server" id="div_destinos_" class="col-sm-8 col-lg-8- controls" visible="false">
                                                        <div class="input-group">
                                                            <%--<asp:TextBox runat="server" CssClass="form-control" ID="tx_destinos"></asp:TextBox>--%>
                                                            <textarea runat="server" placeholder="Correos Destinos..." id="tx_destinos" class="form-control" rows="3"></textarea>
                                                            <asp:Button ID="btn_enviar" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-primary" OnClick="btn_enviar_Click" Text="Enviar" />
                                                            <asp:Button ID="btn_cerrar" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-warning" OnClick="btn_cerrar_Click" Text="Cerrar" />

                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                            <div class="row">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="div_historico" runat="server" class="row" visible="false">
                        <div class="col-md-12">
                            <div class="box">
                                <div class="box-title">
                                    <h3><i class="fa fa-table"></i>Cuenta Corriente Cliente</h3>
                                    <div class="box-tool">
                                        <a data-action="collapse" href="#"></a>

                                    </div>
                                </div>
                                <div class="box-content">
                                    <div class="btn-toolbar pull-left input-group">
                                        <input type="text" id="t_filtra" style="width: 200px; margin-right: 7px; padding: 5px;" placeholder="Filtrar..." class="form-control" />
                                        <%--<button class="btn btn-primary" type="button" id="Button2"><i class="fa fa-search"></i></button>--%>
                                    </div>
                                    <div class="btn-toolbar pull-right">

                                        <div class="btn-group">
                                            <asp:LinkButton ID="excel" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Editar" runat="server" OnClick="excel_Click"></asp:LinkButton>
                                        </div>
                                    </div>

                                    <div class="clearfix"></div>
                                    <div class="table-responsive" style="border: 0">

                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box">
                                                    <div class="form-group">
                                                        <asp:GridView ID="G_CTA_CORRIENTE" CssClass="table table-bordered filtra" OnRowDataBound="G_CTA_CORRIENTE_RowDataBound" OnRowCommand="G_CTA_CORRIENTE_RowCommand" runat="server" Visible="false"
                                                            ShowHeaderWhenEmpty="True" Font-Size="12px" DataKeyNames="">
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

                    <div id="DIV_CRUZADO" runat="server" class="row" visible="false">
                        <div class="col-md-12">
                            <div class="box">
                                <div class="box-title">
                                    <h3><i class="fa fa-table"></i>Periodos/Productos</h3>
                                    <div class="box-tool">
                                        <a data-action="collapse" href="#"></a>

                                    </div>
                                </div>
                                <div class="box-content">
                                    <div class="btn-toolbar pull-left input-group">
                                        <input type="text" id="t_filtra3" style="width: 200px; margin-right: 7px; padding: 5px;" placeholder="Filtrar..." class="form-control" />
                                        <%--<button class="btn btn-primary" type="button" id="Button1"><i class="fa fa-search"></i></button>--%>
                                    </div>
                                    <div class="btn-toolbar pull-right">
                                        <asp:TextBox runat="server" ID="tx_enviar_" class="form-control"></asp:TextBox>


                                        <div class="btn-group">
                                            <label runat="server" id="correo_env" style="color: red;"></label>
                                            <asp:LinkButton ID="excel2" class="btn btn-circle show-tooltip fa fa-envelope" Visible="true" Style="margin-left: 5px" title="Correo" runat="server" OnClick="excel2_Click"></asp:LinkButton>
                                        </div>
                                    </div>

                                    <div class="clearfix"></div>
                                    <div class="table-responsive" style="border: 0">

                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box">
                                                    <div class="form-group" style="overflow-x: auto">
                                                        <asp:GridView ID="G_CRUZADO" CssClass="table table-bordered filtra3" OnRowDataBound="G_CRUZADO_RowDataBound" OnRowCommand="G_CRUZADO_RowCommand" runat="server" Visible="false"
                                                            ShowHeaderWhenEmpty="True" Font-Size="12px" DataKeyNames="">
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

                                                        <asp:GridView ID="g_doc" AutoGenerateColumns="true" CssClass="table table-bordered filtrar" OnRowDataBound="g_doc_RowDataBound" runat="server" Visible="false"
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
                        </div>
                    </div>

                </asp:View>

                <%-- 2 Editar--%>
                <asp:View ID="View2" runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="box">
                                <div class="box-title">
                                    <h3 runat="server" id="h3_"><i class="fa fa-table"></i>Editar Cliente</h3>
                                    <div class="box-tool">
                                        <a data-action="collapse" href="#"></a>

                                        <button id="volver_2" visible="true" style="float: right; margin-top: -6px;" class="btn btn-warning icon-plus btn-circle" runat="server" onserverclick="volver_2_ServerClick"><i class="fa fa-arrow-circle-left"></i></button>


                                    </div>
                                </div>
                                <div class="box-content">

                                    <div class="clearfix"></div>
                                    <div class="table-responsive" style="border: 0">
                                        <%--<asp:TextBox ID="Txt_VENDEDOR" runat="server" Text="" OnTextChanged="Unnamed_TextChanged"></asp:TextBox>--%>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box">
                                                    <div class="form-group">
                                                        <div class="row">
                                                            <div class="col-md-6 company-info">
                                                                <h4>
                                                                    <p>
                                                                        <asp:Label ID="nombrecliente_edit" runat="server"></asp:Label>
                                                                    </p>
                                                                    <div class="company-info">
                                                                        <p>
                                                                            <i class="fa fa-circle"></i>
                                                                            <asp:Label ID="rutcliente_edit" runat="server"></asp:Label>
                                                                        </p>
                                                                    </div>
                                                                </h4>
                                                            </div>

                                                        </div>
                                                        <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                            <div class="panel panel-primary" style="border-radius: 34px;">
                                                                <div class="panel-body">
                                                                    <div class="form-group">

                                                                        <div class="col-md-2 company-info">
                                                                            <p>
                                                                                <i class="fa fa-location-arrow"></i>
                                                                                <textarea rows="2" class="form-control" wrap="soft" id="direccion_edit" runat="server"></textarea>

                                                                            </p>

                                                                        </div>
                                                                        <div class="col-md-2 company-info">
                                                                            <p>
                                                                                <i class="fa fa-flag"></i>
                                                                                <textarea rows="2" class="form-control" wrap="soft" id="ciudad_edit" runat="server"></textarea>
                                                                                <span class="help-inline">"Ciudad" , "Pais"</span>
                                                                            </p>
                                                                        </div>
                                                                        <div class="col-md-2 company-info">
                                                                            <p>
                                                                                <i class="fa fa-phone"></i>
                                                                                <textarea rows="2" class="form-control" wrap="soft" id="fono_edit" runat="server"></textarea>

                                                                            </p>
                                                                        </div>
                                                                        <div class="col-md-2 company-info">
                                                                            <p>
                                                                                <i class="fa fa-envelope"></i>
                                                                                <textarea rows="2" class="form-control" wrap="soft" id="correo_edit" runat="server"></textarea>
                                                                            </p>
                                                                        </div>

                                                                        <div class="col-md-2 company-info">
                                                                            <p>
                                                                                <i class="fa fa-envelope"></i>
                                                                                <asp:DropDownList runat="server" ID="vendedor_edit"></asp:DropDownList>
                                                                                <span runat="server" id="help_vendedor" class="help-inline"></span>
                                                                            </p>
                                                                        </div>



                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="pull-right">
                                                            <div class="btn-group">
                                                                <asp:Button ID="save_edit" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-success" OnClick="save_edit_Click" Text="Guardar" />
                                                            </div>
                                                        </div>
                                                        <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="carga_edit" runat="server" style="display: none; font-size: 3em;"></i>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>




                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>

                </asp:View>


                <%-- 3 Crear--%>
                <asp:View ID="View3" runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="box">
                                <div class="box-title">
                                    <h3 runat="server" id="h1"><i class="fa fa-table"></i>Crear Cliente</h3>
                                    <div class="box-tool">
                                        <a data-action="collapse" href="#"></a>

                                        <button id="Button1" visible="true" style="float: right; margin-top: -6px;" class="btn btn-warning icon-plus btn-circle" runat="server" onserverclick="volver3_ServerClick"><i class="fa fa-arrow-circle-left"></i></button>


                                    </div>
                                </div>
                                <div class="box-content">

                                    <div class="clearfix"></div>
                                    <div class="table-responsive" style="border: 0">
                                        <%--<asp:TextBox ID="Txt_VENDEDOR" runat="server" Text="" OnTextChanged="Unnamed_TextChanged"></asp:TextBox>--%>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box">
                                                    <div class="form-group">
                                                        <div class="row">
                                                            <div class="col-md-6 company-info">
                                                                <h4>


                                                                    <div class="company-info">
                                                                        <p>

                                                                            <asp:TextBox ID="rutcliente_NEW" class="form-control" Width="50%" runat="server"></asp:TextBox>
                                                                            <span class="help-inline">RUN</span>
                                                                        </p>
                                                                        <p>

                                                                            <asp:TextBox class="form-control" ID="nombre_cliente_NEW" runat="server"></asp:TextBox>
                                                                            <span class="help-inline">NOMBRE</span>
                                                                        </p>
                                                                    </div>

                                                                    <div class="col-sm-1 col-lg-1 controls">
                                                                        <asp:ImageButton ID="ImageButton1" ImageUrl="~/img/Ticket_verde.png" runat="server" OnClientClick="CARGA_FECHA();" OnClick="b_Click1" />
                                                                        <i class="fa fa-circle-o-notch fa-spin" id="I1" runat="server" style="font-size: 2em; display: none;"></i>

                                                                    </div>
                                                                    <div class="company-info">
                                                                    </div>

                                                                </h4>
                                                            </div>

                                                        </div>
                                                        <div runat="server" id="no_muestra" visible="false">
                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label1" runat="server" Text="Vendedor"></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">
                                                                        <div class="form-group">
                                                                            <div class="row">


                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-user"></i>
                                                                                        <div class="controls">
                                                                                            <asp:ListBox ID="vendedor_NEW_" SelectionMode="Multiple" Width="120%" data-placeholder=" -- Otro -- " runat="server" Visible="false" class="form-control chosen-select"></asp:ListBox>

                                                                                            <asp:DropDownList runat="server" Width="120%" ID="vendedor_NNEW"></asp:DropDownList>
                                                                                        </div>
                                                                                        <textarea runat="server" class="form-control" id="vendedor_NEW" visible="false"></textarea>
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label38" runat="server" Text="Antecedentes Generales"></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">

                                                                        <div class="form-group">
                                                                            <div class="row">

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-location-arrow"></i>
                                                                                        <textarea class="form-control" id="direccion_NEW" runat="server"></textarea>
                                                                                        <span class="help-inline">Dirección&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                                </div>
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-flag"></i>
                                                                                        <textarea class="form-control" id="ciudad_NEW" runat="server"></textarea>
                                                                                        <span class="help-inline">"Ciudad/Región" , "País"</span>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-phone"></i>
                                                                                        <textarea class="form-control" id="fono_NEW" runat="server"></textarea>
                                                                                        <span class="help-inline">Teléfono&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-barcode"></i>
                                                                                        <textarea class="form-control" id="giro_NEW" runat="server"></textarea>
                                                                                        <span class="help-inline">Giro&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                                    </p>
                                                                                </div>



                                                                            </div>
                                                                        </div>
                                                                        <hr />
                                                                        <hr />
                                                                        <div class="row">
                                                                            <div class="text-left" style="padding-left: 2%;">
                                                                                <b>
                                                                                    <asp:Label ID="Label43" runat="server" Text="Contacto 1"></asp:Label>
                                                                                </b>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <div class="row">
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-dot-circle-o"></i>
                                                                                        <textarea class="form-control" id="nombre_NEW_1" runat="server"></textarea>
                                                                                        <span class="help-inline">Nombre</span>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-envelope"></i>
                                                                                        <textarea class="form-control" id="correo_NEW_1" runat="server"></textarea>
                                                                                        <span class="help-inline">Correo</span>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-gavel"></i>
                                                                                        <textarea class="form-control" id="cargo_NEW_1" runat="server"></textarea>
                                                                                        <span class="help-inline">Cargo</span>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-phone"></i>
                                                                                        <textarea class="form-control" id="fono_NEW_1" runat="server"></textarea>
                                                                                        <span class="help-inline">Teléfono</span>
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="pull-right">
                                                                            <div class="btn-group">
                                                                                <asp:LinkButton ID="LinkButton2" class="btn btn-circle btn-success fa fa-plus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Nuevo Contacto" runat="server" OnClick="LinkButton2_Click"></asp:LinkButton>
                                                                            </div>
                                                                        </div>
                                                                        <div runat="server" id="cont_2" style="display: none;">
                                                                            <hr />
                                                                            <div class="row">
                                                                                <div class="text-left" style="padding-left: 2%;">
                                                                                    <b>
                                                                                        <asp:Label ID="Label48" runat="server" Text="Contacto 2"></asp:Label>
                                                                                    </b>
                                                                                </div>
                                                                            </div>

                                                                            <div class="form-group">
                                                                                <div class="row">

                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa  fa-dot-circle-o"></i>
                                                                                            <textarea class="form-control" id="nombre_NEW_2" runat="server"></textarea>
                                                                                            <span class="help-inline">Nombre</span>
                                                                                        </p>
                                                                                    </div>

                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa fa-envelope"></i>
                                                                                            <textarea class="form-control" id="correo_NEW_2" runat="server"></textarea>
                                                                                            <span class="help-inline">Correo</span>
                                                                                        </p>
                                                                                    </div>
                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa  fa-gavel"></i>
                                                                                            <textarea class="form-control" id="cargo_NEW_2" runat="server"></textarea>
                                                                                            <span class="help-inline">Cargo</span>
                                                                                        </p>
                                                                                    </div>
                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa fa-phone"></i>
                                                                                            <textarea class="form-control" id="fono_NEW_2" runat="server"></textarea>
                                                                                            <span class="help-inline">Teléfono</span>
                                                                                        </p>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="pull-right">
                                                                                <div class="btn-group">
                                                                                    <asp:LinkButton ID="LinkButton3" class="btn btn-circle btn-danger fa fa-minus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Quitar Contacto" runat="server" OnClick="LinkButton3_Click"></asp:LinkButton>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label58" runat="server" Text="Tipo Negocio"></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">
                                                                        <div class="form-group">
                                                                            <div class="row">


                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-briefcase"></i>
                                                                                        <asp:DropDownList runat="server" Width="120%" ID="tipo_negocio"></asp:DropDownList>

                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label60" runat="server" Text="Refencia Bancaria"></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">
                                                                        <div class="form-group">
                                                                            <div class="row">


                                                                                <div class="col-md-5 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-btc"></i>
                                                                                        <textarea class="form-control" id="banco_1" runat="server"></textarea>
                                                                                        <span class="help-inline">Banco</span>
                                                                                    </p>
                                                                                </div>



                                                                                <div class="col-md-5 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-credit-card"></i>
                                                                                        <textarea class="form-control" id="cta_1" runat="server"></textarea>
                                                                                        <span class="help-inline">CTA.CTE.</span>

                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="pull-right">
                                                                            <div class="btn-group">
                                                                                <asp:LinkButton ID="LinkButton4" class="btn btn-circle btn-success fa fa-plus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Nueva CTA" runat="server" OnClick="LinkButton4_Click"></asp:LinkButton>
                                                                            </div>
                                                                        </div>

                                                                        <div runat="server" id="otra_cta_cte" style="display: none;">
                                                                            <hr />
                                                                            <div class="form-group">
                                                                                <div class="row">


                                                                                    <div class="col-md-5 company-info">
                                                                                        <p>
                                                                                            <i class="fa  fa-btc"></i>
                                                                                            <textarea class="form-control" id="banco_2" runat="server"></textarea>
                                                                                            <span class="help-inline">Banco</span>
                                                                                        </p>
                                                                                    </div>



                                                                                    <div class="col-md-5 company-info">
                                                                                        <p>
                                                                                            <i class="fa fa-credit-card"></i>
                                                                                            <textarea class="form-control" id="cta_2" runat="server"></textarea>
                                                                                            <span class="help-inline">CTA.CTE.</span>

                                                                                        </p>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="pull-right">
                                                                                <div class="btn-group">
                                                                                    <asp:LinkButton ID="LinkButton5" class="btn btn-circle btn-danger fa fa-minus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Quitar CTA" runat="server" OnClick="LinkButton5_Click"></asp:LinkButton>
                                                                                </div>
                                                                            </div>

                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label63" runat="server" Text="Antecedentes de la Sociedad"></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">
                                                                        <div class="form-group">
                                                                            <div class="row">

                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-dot-circle-o"></i>
                                                                                        <textarea class="form-control" id="nombre_socie" runat="server"></textarea>
                                                                                        <span class="help-inline">Nombre</span>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-circle"></i>
                                                                                        <textarea class="form-control" id="rut_socie" runat="server"></textarea>
                                                                                        <span class="help-inline">RUN</span>
                                                                                    </p>
                                                                                </div>


                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-location-arrow"></i>
                                                                                        <textarea class="form-control" id="direcc_socie" runat="server"></textarea>
                                                                                        <span class="help-inline">Dirección</span>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-flag"></i>
                                                                                        <textarea class="form-control" id="ciudad_socie" runat="server"></textarea>
                                                                                        <span class="help-inline">"Ciudad" , "País"</span>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-phone"></i>
                                                                                        <textarea class="form-control" id="fono_socie" runat="server"></textarea>
                                                                                        <span class="help-inline">Teléfono</span>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-2 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-envelope"></i>
                                                                                        <textarea class="form-control" id="correo_socie" runat="server"></textarea>
                                                                                        <span class="help-inline">Correo</span>
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <hr />
                                                                        <hr />

                                                                        <div class="row">
                                                                            <div class="text-left" style="padding-left: 2%;">
                                                                                <b>
                                                                                    <asp:Label ID="Label70" runat="server" Text="Socio 1"></asp:Label>
                                                                                </b>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <div class="row">
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-circle"></i>
                                                                                        <textarea class="form-control" id="rut_soc_1" runat="server"></textarea>
                                                                                        <span class="help-inline">RUN</span>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa  fa-dot-circle-o"></i>
                                                                                        <textarea class="form-control" id="nombre_soc_1" runat="server"></textarea>
                                                                                        <span class="help-inline">Nombre</span>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-envelope"></i>
                                                                                        <textarea class="form-control" id="correo_soc_1" runat="server"></textarea>
                                                                                        <span class="help-inline">Correo</span>
                                                                                    </p>
                                                                                </div>

                                                                                <div class="col-md-3 company-info">
                                                                                    <p>
                                                                                        <i class="fa">%</i>
                                                                                        <textarea class="form-control" id="porcent_soc_1" runat="server"></textarea>
                                                                                        <span class="help-inline">Porcentaje</span>
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="pull-right">
                                                                            <div class="btn-group">
                                                                                <asp:LinkButton ID="LinkButton6" class="btn btn-circle btn-success fa fa-plus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Nuevo Socio" runat="server" OnClick="LinkButton6_Click"></asp:LinkButton>
                                                                            </div>
                                                                        </div>

                                                                        <div runat="server" id="nuevo_socio" style="display: none;">
                                                                            <hr />
                                                                            <div class="row">
                                                                                <div class="text-left" style="padding-left: 2%;">
                                                                                    <b>
                                                                                        <asp:Label ID="Label75" runat="server" Text="Socio 2"></asp:Label>
                                                                                    </b>
                                                                                </div>
                                                                            </div>

                                                                            <div class="form-group">
                                                                                <div class="row">

                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa  fa-circle"></i>
                                                                                            <textarea class="form-control" id="rut_soc_2" runat="server"></textarea>
                                                                                            <span class="help-inline">RUN</span>
                                                                                        </p>
                                                                                    </div>
                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa  fa-dot-circle-o"></i>
                                                                                            <textarea class="form-control" id="nombre_soc_2" runat="server"></textarea>
                                                                                            <span class="help-inline">Nombre</span>
                                                                                        </p>
                                                                                    </div>

                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa fa-envelope"></i>
                                                                                            <textarea class="form-control" id="correo_soc_2" runat="server"></textarea>
                                                                                            <span class="help-inline">Correo</span>
                                                                                        </p>
                                                                                    </div>

                                                                                    <div class="col-md-3 company-info">
                                                                                        <p>
                                                                                            <i class="fa">%</i>
                                                                                            <textarea class="form-control" id="porcent_soc_2" runat="server"></textarea>
                                                                                            <span class="help-inline">Porcentaje</span>
                                                                                        </p>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="pull-right">
                                                                                <div class="btn-group">
                                                                                    <asp:LinkButton ID="LinkButton7" class="btn btn-circle btn-danger fa fa-minus" Style="padding: 7px; line-height: 10px;" Visible="true" title="Quitar Socio" runat="server" OnClick="LinkButton7_Click"></asp:LinkButton>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>

                                                            <div class="row">
                                                                <div class="text-right" style="padding-right: 3%;">
                                                                    <b>
                                                                        <asp:Label ID="Label2" runat="server" Text="Antecedentes SOPRODI S.A."></asp:Label>
                                                                    </b>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="margin-right: 2px; margin-left: 2px;">
                                                                <div class="panel panel-primary" style="border-radius: 34px;">
                                                                    <div class="panel-body">
                                                                        <div class="form-group">
                                                                            <div class="row">


                                                                                <div class="col-sm-3 col-lg-3 controls">
                                                                                    <p>
                                                                                        <div class="input-group">

                                                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                                                            <asp:TextBox ID="t_desde_cliente" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="t_desde_cliente" Format="dd/MM/yyyy" />

                                                                                        </div>
                                                                                        <span class="help-inline">Cliente desde</span>
                                                                                    </p>
                                                                                </div>



                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <div class="row">

                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-credit-card"></i>
                                                                                        <textarea class="form-control" id="credito_actual" runat="server"></textarea>
                                                                                        <span class="help-inline">Crédito Actual</span>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-bars"></i>
                                                                                        <textarea class="form-control" id="tipo_credito_actual" runat="server"></textarea>
                                                                                        <span class="help-inline">Tipo Crédito Actual</span>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-money"></i>
                                                                                        <textarea class="form-control" id="monto_credito_actual" runat="server"></textarea>
                                                                                        <span class="help-inline">Monto Crédito Actual</span>
                                                                                    </p>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <div class="row">

                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-credit-card"></i>
                                                                                        <textarea class="form-control" id="credito_soli" runat="server"></textarea>
                                                                                        <span class="help-inline">Crédito Solicitado</span>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-bars"></i>
                                                                                        <textarea class="form-control" id="tipo_credito_soli" runat="server"></textarea>
                                                                                        <span class="help-inline">Tipo Crédito Solicitado</span>
                                                                                    </p>
                                                                                </div>
                                                                                <div class="col-md-4 company-info">
                                                                                    <p>
                                                                                        <i class="fa fa-money"></i>
                                                                                        <textarea class="form-control" id="monto_credito_soli" runat="server"></textarea>
                                                                                        <span class="help-inline">Monto Crédito Solicitado</span>
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

                                            <div class="pull-right">
                                                <div class="btn-group">
                                                    <%--<asp:Button ID="btn_productos" runat="server" OnClientClick="CARGANDO();" Style="color: white; float: right;" Class="btn btn-success" OnClick="btn_productos_Click" Text="NuevosClientes" />--%>


                                                    <asp:Button ID="Crear_cliente" runat="server" OnClientClick="CARGANDO();" Style="color: white; right: 16%;" Class="btn btn-primary" OnClick="Crear_cliente_Click1" Text="Enviar creación de cliente" />
                                                    <br />
                                                    <asp:Label runat="server" ID="l_h1" Style="color: red"></asp:Label>

                                                </div>
                                            </div>
                                        </div>

                                        <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="crear_cargando" runat="server" style="display: none; font-size: 3em;"></i>

                                    </div>
                                </div>
                            </div>
                            <div id="div8" runat="server" class="row" style="visibility: visible">
                                <div id="div9" runat="server" class="row" visible="false">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="box-title">
                                                <h3><i class="fa fa-table"></i>Clientes nuevos</h3>
                                                <div class="box-tool">
                                                    <a data-action="collapse" href="#"></a>

                                                </div>
                                            </div>
                                            <div class="box-content">
                                                <div class="btn-toolbar pull-left input-group">
                                                    <input type="text" id="Text1" style="width: 200px; margin-right: 7px; padding: 5px;" placeholder="Filtrar..." class="form-control" />
                                                    <%--<button class="btn btn-primary" type="button" id="Button1"><i class="fa fa-search"></i></button>--%>
                                                </div>
                                                <div class="btn-toolbar pull-right">

                                                    <div class="btn-group">
                                                        <asp:LinkButton ID="LinkButton1" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Editar" runat="server" OnClick="excel2_Click"></asp:LinkButton>
                                                    </div>
                                                </div>

                                                <div class="clearfix"></div>
                                                <div class="table-responsive" style="border: 0">

                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="box">
                                                                <div class="form-group">
                                                                    <asp:GridView ID="g_NUEVOS_CLIENTES" ClientIDMode="Static" CssClass="table table-bordered filtrar" OnRowDataBound="g_NUEVOS_CLIENTES_RowDataBound" OnRowCommand="g_NUEVOS_CLIENTES_RowCommand" runat="server" Visible="false"
                                                                        ShowHeaderWhenEmpty="True" Font-Size="12px" DataKeyNames="rutcliente">
                                                                        <HeaderStyle CssClass="test no-sort" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="#" HeaderStyle-Wrap="false" ItemStyle-Width="4px" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="5px" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Ficha" HeaderStyle-Wrap="false" ItemStyle-Width="4px" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton runat="server" ID="b_edSit" ImageUrl="img/user.png" Width="17"
                                                                                        CommandName="ficha" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
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


                    </div>



                </asp:View>

            </asp:MultiView>
            </div>

          


        </ContentTemplate>
    </asp:UpdatePanel>
    <script src="js/tablesort.js"></script>

    <!--flaty scripts-->

    <script src="assets/bootstrap-inputmask/bootstrap-inputmask.min.js"></script>
    <script src='js/sorts/tablesort.numeric.js'></script>
    <script src="assets/jquery-tags-input/jquery.tagsinput.min.js"></script>

    <script src="assets/bootstrap-wysihtml5/wysihtml5-0.3.0.js"></script>
    <script src="assets/bootstrap-wysihtml5/bootstrap-wysihtml5.js"></script>


    <asp:TextBox ID="l_usuario_" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_grupos" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_vendedores" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_clientes" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_productos" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="l_periodos" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>

</asp:Content>
