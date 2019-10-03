<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="COBRANZA_LISTADOS.aspx.cs" Inherits="SoprodiApp.COBRANZA_LISTADOS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">

    <style>
        .test {
            background-color: #428bca !important;
            color: white !important;
        }

        hr.style-four {
            height: 12px;
            border: 0;
            box-shadow: inset 0 12px 12px -12px rgba(0, 0, 0, 0.5);
        }
    </style>
    <script>
        function CARGANDO() {

            var elem3 = document.getElementById("<%=cargando_gif.ClientID%>");
            elem3.style.display = "block";

        }
        function stringtojson(string) {
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

        function llena_hich() {
            $.ajax({
                type: "POST",
                url: "COBRANZA_LISTADOS.aspx/llena_totales",

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,

            }).done(function (resp) {
                resp = resp.d;
                var encabezados = stringtojson(resp);
                $('#container').highcharts({
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'Montos Vencidos y Estimados Cobranzas'
                    },
                    tooltip: {

                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>:  {point.percentage:.1f} %',
                                style: {

                                    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                }
                            },
                            showInLegend: true
                        }
                    },
                    series: encabezados
                });

            });
        }



        function llena_hich3() {
            $.ajax({
                type: "POST",
                url: "COBRANZA_LISTADOS.aspx/llena_cheques",

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,

            }).done(function (resp) {
                resp = resp.d;

                //POR RANGO DE DIAS
                var encabezados1 = stringtojson(resp.split('&')[0]);
                // TOTAL ESTIMADO Y VENCIDO
                var encabezados2 = stringtojson(resp.split('&')[1]);
                //FACTURAS CHEQUES Y DM
                var encabezados3 = stringtojson(resp.split('&')[2]);
                //ABARROTES Y GRANOS
                var encabezados4 = stringtojson(resp.split('&')[3]);
                //CLASES CLIENTE
                var encabezados5 = stringtojson(resp.split('&')[4]);


                $('#container3').highcharts({
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'Montos Vencidos y NoVencidos por Rangos de fechas'
                    },
                    tooltip: {

                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                                style: {

                                    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                }
                            },
                            showInLegend: true
                        }
                    },
                    series: encabezados1
                });

                

                $('#container').highcharts({
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'Montos Vencidos y NoVencidos Cobranzas'
                    },
                    tooltip: {

                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                                style: {

                                    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                }
                            },
                            showInLegend: true
                        }
                    },
                    series: encabezados2
                });


                $('#container4').highcharts({
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'Montos Vencidos Facturas, Cheques y DM Cobranzas '
                    },
                    tooltip: {

                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                                style: {

                                    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                }
                            },
                            showInLegend: true
                        }
                    },
                    series: encabezados3
                });


                $('#container5').highcharts({
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'Montos Vencidos Abarrotes y Granos Cobranzas'
                    },
                    tooltip: {

                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                                style: {

                                    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                }
                            },
                            showInLegend: true
                        }
                    },
                    series: encabezados4
                });

                $('#container6').highcharts({
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'Cantidad de Clases Clientes'
                    },
                    tooltip: {

                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                                style: {

                                    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                }
                            },
                            showInLegend: true
                        }
                    },
                    series: encabezados5
                });



            });
        }
        $(document).ready(function () {
            llena_hich3();

            //llena_hich();
        });


    </script>
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <%--<div class="page-title" style="margin-top: -27px">
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
            <li class="active">Listados</li>
        </ul>
    </div>

    <div id="main-content">


        <div class="row">
            <div class="col-md-12">
                <div class="box">
                    <div class="box-title">
                        <h3><i class="fa fa-table"></i>Listados</h3>
                        <div class="box-tool">
                        </div>
                    </div>
                    <div class="box-content">

<%--                        <hr class="style-four" />
                        <hr class="style-four" />--%>

                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UP_SubirArchivo">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="CB_Opcion" />
                                <asp:AsyncPostBackTrigger ControlID="IN_DM" />

                            </Triggers>
                            <ContentTemplate>




                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-horizontal" id="Ocultar">
                                            <!-- ********************************************************************* -->
                                            <div class="form-group">
                                                <div class="col-sm-3 controls">
                                                    <label class="col-sm-1 col-lg-1 control-label" style="top: -1px; font-size: 22px;">Filtro</label>
                                                    <%--<asp:ListBox ID="CB_Opcion" SelectionMode="Multiple" data-placeholder=" -- Seleccione -- " runat="server" class="form-control chosen-select"></asp:ListBox>--%>

                                                    <asp:DropDownList runat="server" CssClass="form-control sm" ID="CB_Opcion" AutoPostBack="true" OnSelectedIndexChanged="CB_Opcion_SelectedIndexChanged">
                                                        <asp:ListItem Text="-- Seleccione --" Value="-1" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Documentos Abiertos" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Pagos Realizados" Value="2"></asp:ListItem>
                                                        <%--<asp:ListItem Text="Clientes" Value="3"></asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </div>



                                            </div>
                                            <div class="form-group" runat="server" id="doc_abiertos" visible="false">

                                                <div class="col-sm-3 controls">

                                                    <label class="col-sm-1 col-lg-1 control-label" style="top: -1px; font-size: 22px;">TipoDocumento</label>
                                                    <asp:DropDownList runat="server" CssClass="form-control sm" AutoPostBack="true" ID="IN_DM" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                                        <asp:ListItem Text="-- Seleccione --" Value="-1" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Ingreso" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Cheques" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="DM" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="Todos" Value="3"></asp:ListItem>
                                                        <%--<asp:ListItem Text="Clientes" Value="3"></asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </div>


                                                <div class="col-sm-3 controls" runat="server" id="clientes" visible="false">
                                                    <label class="col-sm-1 col-lg-1 control-label" style="top: -1px; font-size: 22px;">Clientes</label>
                                                    <asp:DropDownList runat="server" CssClass="form-control sm" AutoPostBack="true" ID="D_CLIENTES" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged">
                                                        <asp:ListItem Text="-- Seleccione --" Value="-1" Selected="True"></asp:ListItem>
                                                        
                                                        <%--<asp:ListItem Text="Clientes" Value="3"></asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </div>

                                            </div>

                                            <div class="form-group" runat="server" id="FECHAS" visible="false">
                                                <label class="col-sm-1 col-lg-1 control-label" style="top: -1px; font-size: 22px;">Desde</label>

                                                <div class="col-sm-2 col-lg-2 controls">
                                                    <div class="input-group">

                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                        <asp:TextBox ID="txt_desde" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_desde" Format="dd/MM/yyyy" />

                                                    </div>
                                                </div>
                                                <label class="col-sm-1 col-lg-1 control-label" style="top: -1px; font-size: 22px;">Hasta</label>
                                                <div class="col-sm-2 col-lg-2 controls">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>

                                                        <asp:TextBox ID="txt_hasta" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender8" runat="server" TargetControlID="txt_hasta" Format="dd/MM/yyyy" />

                                                    </div>
                                                </div>
                                                <label class="col-sm-1 col-lg-1 control-label" style="top: -1px; font-size: 22px;">Clientes</label>
                                                <div class="col-sm-4 col-lg-4 controls" runat="server" id="clientes2" visible="true">

                                                    <div class="input-group" style="width: 100%">
                                                        <asp:DropDownList runat="server" Width="100%" CssClass="form-control sm" AutoPostBack="true" ID="D_CLIENTES2" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged">
                                                            <asp:ListItem Text="-- Seleccione --" Value="-1" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Cliente1" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Cliente2" Value="2"></asp:ListItem>
                                                            <%--<asp:ListItem Text="Clientes" Value="3"></asp:ListItem>--%>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>



                                            <div class="form-group">
                                                <div class="pull-right">
                                                    <div class="btn-group">
                                                        <label class="col-sm-2 col-sm-offset-1 controls">
                                                            <asp:Button CssClass="btn btn-primary" OnClientClick="CARGANDO();" runat="server" ID="B_Filtrar" Text="Filtrar" OnClick="FILTRAR_Click" />
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                            <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_gif" runat="server" style="display: none; font-size: 3em;"></i>

                                        </div>
                                    </div>
                                </div>

                                <p runat="server" id="muestra_total" visible="false">
                                    <strong>Total:</strong>
                                    <span class="green font-size-17">
                                        <strong>$<asp:Label runat="server" ID="total_"></asp:Label>
                                        </strong>
                                    </span>
                                    <span class="black font-size-17">
                                        <strong>
                                            <asp:Label runat="server" ID="Label2" Text="="></asp:Label>
                                        </strong>
                                    </span>

                                    <span class="red font-size-17">
                                        <strong>
                                            <asp:Label runat="server" ID="total_vencido"></asp:Label>
                                        </strong>
                                    </span>
                                    <span class="black font-size-17">
                                        <strong>
                                            <asp:Label runat="server" ID="Label1" Text="+"></asp:Label>
                                        </strong>
                                    </span>

                                    <span class="blue font-size-17">
                                        <strong>
                                            <asp:Label runat="server" ID="total_estimado"></asp:Label>
                                        </strong>
                                    </span>
                                </p>

                                <div class="row">
                                    <div class="col-sm-12">
                                        <div style="overflow-x: auto; width: 100%">
                                            <asp:GridView CssClass="table fill-head table-bordered filtrar" ID="G_Listado" runat="server" OnRowDataBound="G_Listado_RowDataBound" AutoGenerateColumns="true" ShowHeaderWhenEmpty="True" Width="100%">
                                                <HeaderStyle CssClass="test" />
                                                <EmptyDataTemplate>
                                                    No existen datos.
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>


        <div class="col-md-6">
            <div class="box">
                <div class="box-title">
                    <h3><i class="fa fa-table"></i></h3>
                    <div class="box-tool">
                    </div>
                </div>
                <div class="box-content">

                    <div id="container3" style="min-width: 310px; height: 400px; max-width: 600px; margin: 0 auto;"></div>

                </div>
            </div>
        </div>



        <div class="col-md-6">
            <div class="box">
                <div class="box-title">
                    <h3><i class="fa fa-table"></i></h3>
                    <div class="box-tool">
                    </div>
                </div>
                <div class="box-content">

                    <div id="container" style="min-width: 310px; height: 400px; max-width: 600px; margin: 0 auto;"></div>



                </div>
            </div>
        </div>

          <div class="col-md-6">
            <div class="box">
                <div class="box-title">
                    <h3><i class="fa fa-table"></i></h3>
                    <div class="box-tool">
                    </div>
                </div>
                <div class="box-content">

                    <div id="container4" style="min-width: 310px; height: 400px; max-width: 600px; margin: 0 auto;"></div>

                </div>
            </div>
        </div>


             <div class="col-md-6">
            <div class="box">
                <div class="box-title">
                    <h3><i class="fa fa-table"></i></h3>
                    <div class="box-tool">
                    </div>
                </div>
                <div class="box-content">

                    <div id="container5" style="min-width: 310px; height: 400px; max-width: 600px; margin: 0 auto;"></div>

                </div>
            </div>
        </div>

        <div class="col-md-6" style="visibility:hidden">
            <div class="box">
                <div class="box-title">
                    <h3><i class="fa fa-table"></i></h3>
                    <div class="box-tool">
                    </div>
                </div>
                <div class="box-content">

                    <div id="container6" style="display:none; min-width: 310px; height: 400px; max-width: 600px; margin: 0 auto;"></div>

                </div>
            </div>
        </div>


        
    </div>
</asp:Content>
