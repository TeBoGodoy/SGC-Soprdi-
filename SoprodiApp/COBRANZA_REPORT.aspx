<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="True" UICulture="es-ES" Culture="es-ES" EnableEventValidation="false" CodeBehind="COBRANZA_REPORT.aspx.cs" Inherits="SoprodiApp.COBRANZA_REPORT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">

    <style>
        .fa-input {
            font-family: FontAwesome, 'Helvetica Neue', Helvetica, Arial, sans-serif;
        }

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

        $(document).ready(function () {

        });


        function reporte_estima_detalle_cargando() {
            var elem3 = document.getElementById("carga_2");
            elem3.style.display = "block";

        }

        function reporte_estima() {
            <%--  var elem3 = document.getElementById("<%=div_detalle.ClientID%>");
            elem3.style.display = "none";--%>

            var desde = $("#<%=txt_desde.ClientID%>").val();
            var hasta = $("#<%=txt_hasta.ClientID%>").val();

            if (desde != "" && hasta != "") {


                var elem66 = document.getElementById("carga_2");
                elem66.style.display = "block";

                //div_general

                //var elem67 = document.getElementById("btn_excel_facturas");
                //elem67.style.display = "block";

                var desde = $("#<%=txt_desde.ClientID%>").val();
                var hasta = $("#<%=txt_hasta.ClientID%>").val();

                var parameters = new Object();
                parameters.desde = desde;
                parameters.hasta = hasta;
                parameters = JSON.stringify(parameters);

                $.ajax({
                    type: "POST",
                    url: "COBRANZA_REPORT.aspx/Reporte_Estimados",
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

                    $("#<%=tabla_html2.ClientID%>").html(resp);
                    document.getElementById("ContentPlaceHolder_Contenido_tabla_html2").innerHTML = resp;
                    var elem3 = document.getElementById("carga_2");
                    elem3.style.display = "none";
                    try {

                        var es = document.getElementById("ContentPlaceHolder_Contenido_div_general");
                        es.style.display = "block";

                    } catch (e) { }
                    try {
                        $("#<%=div_detalle.ClientID%>").hide();
                        $("#<%=div_general.ClientID%>").show();
                    } catch (e) { }

                });
            } else { alert("Seleccione fechas"); }
        }
    </script>
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <%--<div class="page-title" style="margin-top: -27px">
        <div>
            <i class="fa fa-file-o fa-3x"></i><a class="h1" href="MENU_finanzas.aspx">Finanzas</a>
            <h4></h4>
        </div>
    </div>--%>

    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_detalle" />
            <asp:AsyncPostBackTrigger ControlID="btn_detalle_2" />
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
                        <a href="MENU_finanzas.aspx">Finanzas</a>
                        <span class="divider"><i class="fa fa-angle-right"></i></span>
                    </li>
                    <li class="active">Report</li>
                </ul>
            </div>

            <div id="main-content">
                <asp:Label runat="server" ID="tabla_html33"></asp:Label>
                <div runat="server" id="tabla_export"></div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="box">
                            <div class="box-title">
                                <h3><i class="fa fa-table"></i>Estimado a pagar</h3>
                                <div class="box-tool">
                                    <asp:Label runat="server" ID="vencido" Style="color: white; font-size: 23px !important;"></asp:Label>

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
                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="txt_desde" Format="dd/MM/yyyy" />

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
                                                        <%--<asp:ImageButton ID="b" ImageUrl="~/img/Ticket_verde.png" runat="server" OnClientClick="CARGA_FECHA();" OnClick="b_Click" />--%>
                                                        <%--<asp:Button ID="btn_estimados_report" runat="server" OnClientClick="CARGANDO2();" Style="color: white; margin-left: 2%;" Class="btn btn-primary" OnClick="btn_estimados_report_Click" Text="Reporte" />--%>
                                                        <a class="btn btn-primary fa-input" id="btn_reporte_esti" href="javascript:;" onclick="reporte_estima();">&#xf02d; Reporte</a>

                                                        <%-- <asp:Button runat="server" ID="etest" CssClass ="btn btn-primary fa-input" Text="&#xf02d; Reporte"/>--%>
                                                    </div>
                                                    <div class="col-sm-1 col-lg-1 controls">
                                                        <asp:LinkButton runat="server" ID="btn_detalle" OnClientClick="reporte_estima_detalle_cargando();" CssClass="btn btn-success fa-input" Text="&#xf02d; Detalle" OnClick="btn_detalle_Click" />
                                                    </div>
                                                    <div class="col-sm-1 col-lg-1 controls">
                                                        <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="carga_2" style="display: none; font-size: 3em;"></i>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    &nbsp;&nbsp;
                                            <br />

                                                    <br />
                                                </div>
                                                <div class="row">
                                                    &nbsp;&nbsp;
                                            <br />

                                                    <br />
                                                </div>
                                                <div style="display: none" class="row" runat="server" id="div_general">
                                                    <div class="col-md-12">
                                                        <div class="box">
                                                            <div class="box-title">
                                                                <h3><i class="fa fa-table"></i>General</h3>
                                                                <div class="box-tool">
                                                                    <a data-action="collapse" href="#"></a>

                                                                </div>
                                                            </div>
                                                            <div class="box-content">
                                                                <asp:TextBox runat="server" CssClass="form-control" Style="width: 40%;" ID="tx_destinos"></asp:TextBox>
                                                                <asp:LinkButton ID="LinkButton1" class="btn btn-circle show-tooltip fa fa-envelope-o" Visible="true" Style="margin-left: 5px" title="Enviar Correo" runat="server" OnClick="LinkButton1_Click"></asp:LinkButton>
                                                                <div class="btn-toolbar pull-right">

                                                                    <div class="btn-group">


                                                                        <asp:LinkButton ID="btn_excel_general" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Exportar a Excel" runat="server" OnClick="excel_total_Click"></asp:LinkButton>

                                                                        <%--<button runat="server" id="btn_excel" class="btn btn-circle show-tooltip" title="Exportar Excel" onclick="excel_total_Click"><i class="fa fa-table"></i></button>--%>
                                                                    </div>
                                                                </div>
                                                                <div id="tabla_html2" runat="server"></div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div visible="false" class="row" runat="server" id="div_detalle">

                                                    <div class="col-md-12">
                                                        <div class="box">
                                                            <div class="form-group">
                                                                <div class="col-sm-3 controls">
                                                                    <asp:RadioButton ID="chk_estimado" Text="&nbsp;&nbsp;Estimado." Style="padding-right: 20px;"
                                                                        GroupName="tipo_usuario" runat="server" />

                                                                    <asp:RadioButton ID="chk_gestionado" Style="padding-right: 20px;" Text="&nbsp;&nbsp;Gestionado" Checked="True"
                                                                        GroupName="tipo_usuario" runat="server" />
                                                                </div>
                                                                <div class="col-sm-3 controls">
                                                                    <asp:DropDownList runat="server" ID="CB_DIAS_ELEGIDOS" ClientIDMode="Static" CssClass="form-control chosen"></asp:DropDownList>
                                                                </div>
                                                                <div class="col-sm-3 controls">
                                                                    <asp:Button runat="server" ID="btn_detalle_2" OnClientClick="reporte_estima_detalle_cargando();" CssClass="btn btn-success fa-input" Text="&#xf02d; PorDía" OnClick="btn_detalle_2_Click" />

                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        &nbsp;&nbsp;
                                            <br />

                                                        <br />
                                                    </div>
                                                    <div class="col-md-12" runat="server" id="div_facturas" visible="false">
                                                        <div class="box">
                                                            <div class="box-title">
                                                                <h3><i class="fa fa-table"></i>Facturas</h3>
                                                                <div class="box-tool">
                                                                    <a data-action="collapse" href="#"></a>

                                                                </div>
                                                            </div>
                                                            <div class="box-content">
                                                                <div class="btn-toolbar pull-right">

                                                                    <div class="btn-group">
                                                                        <asp:LinkButton ID="btn_excel_facturas" ClientIDMode="Static" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Exportar a Excel" runat="server" OnClick="btn_excel_facturas_Click"></asp:LinkButton>
                                                                    </div>
                                                                </div>

                                                                <div class="clearfix"></div>
                                                                <div class="table-responsive" style="border: 0">
                                                                    <%--<asp:TextBox ID="Txt_VENDEDOR" runat="server" Text="" OnTextChanged="Unnamed_TextChanged"></asp:TextBox>--%>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <div class="box">
                                                                                <div class="form-group">

                                                                                    <asp:GridView ID="G_DETALLE_ESTIMADOS_FACTURAS" ClientIDMode="Static" CssClass="table table-advance tablesorter filtrar" OnRowDataBound="G_DETALLE_ESTIMADOS_FACTURAS_RowDataBound" runat="server" Visible="true"
                                                                                        ShowHeaderWhenEmpty="True" Font-Size="12px">
                                                                                        <HeaderStyle CssClass="test no-sort" />
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="">
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="chkAccept" runat="server" EnableViewState="true" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                        <EmptyDataTemplate>
                                                                                            No existen datos.
                                                                                        </EmptyDataTemplate>
                                                                                    </asp:GridView>
                                                                                    <div runat="server" id="div_total"></div>
                                                                                </div>
                                                                                <asp:Button runat="server" CssClass="btn btn-warning fa-input" Text="&#xf02d; Quitar Cobro Gestión" ID="btn_quitar_agendado" OnClick="btn_quitar_agendado_Click" />

                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12" runat="server" id="div_cheques" visible="false">
                                                        <div class="box">
                                                            <div class="box-title">
                                                                <h3><i class="fa fa-table"></i>Cheques por pagar</h3>
                                                                <div class="box-tool">
                                                                    <a data-action="collapse" href="#"></a>

                                                                </div>
                                                            </div>
                                                            <div class="box-content">
                                                                <div class="btn-toolbar pull-right">

                                                                    <div class="btn-group">
                                                                        <asp:LinkButton ID="btn_excel_cheques" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Exportar a Excel" runat="server" OnClick="btn_excel_cheques_Click"></asp:LinkButton>
                                                                    </div>
                                                                </div>

                                                                <div class="clearfix"></div>
                                                                <div class="table-responsive" style="border: 0">
                                                                    <%--<asp:TextBox ID="Txt_VENDEDOR" runat="server" Text="" OnTextChanged="Unnamed_TextChanged"></asp:TextBox>--%>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <div class="box">
                                                                                <div class="form-group">

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
                                                                    <asp:Label runat="server" ID="mensaje" Style="color: red;"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
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
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
