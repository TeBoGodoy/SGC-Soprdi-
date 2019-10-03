<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" AutoEventWireup="True" UICulture="es-ES" Culture="es-ES" CodeBehind="COMISIONESVENTAS.aspx.cs" Inherits="SoprodiApp.COMISIONESVENTAS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ OutputCache Location="None" NoStore="true" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">

    <script>
        function tabla_refresh() {

            try {
                $('#G_Documentos').DataTable({

                });
                $('#G_NOMINA_SN_COMI').DataTable({

                });

            } catch (e) { }
        }

        function CARGANDO() {
            var elem2 = document.getElementById("<%=btn_productos.ClientID%>");
            elem2.style.visibility = "hidden";

            var elem3 = document.getElementById("<%=cargando_gif.ClientID%>");
            elem3.style.display = "block";
        }

        function CLICK_MODAL() {
            document.getElementById("div_prod").click();
            tabla();
        }


        function click_modal() {
            document.getElementById("modal_comision_btn").click();
        }

        function listado() {
            $('#G_Documentos').DataTable({

            });
        }

        function tabla() {

            try {
                $('#G_Nomina').DataTable({
                    "destroy": true
                    //"tableTools": {
                    //    "aButtons": [
                    //        "copy",
                    //    ]
                    //},
                });
                $('#G_NOMINA_SN_COMI').DataTable({
                    "destroy": true
                    //"tableTools": {
                    //    "aButtons": [
                    //        "copy",
                    //    ]
                    //},
                });


            } catch (e) { }



        }
        function cargando_2() {

            var elem3 = document.getElementById("<%=cargando_gif_2.ClientID%>");
            elem3.style.display = "block";
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

        function click_modal_cm(descr) {

            document.getElementById("modal_cm_h3").innerHTML = descr;
            document.getElementById("moda_cm").click();

        }


        function inner_html_div_firmas(descr) {



            document.getElementById("div_firmas").innerHTML = descr;


        }







        function cargarfirmas_on(usuario, periodo, sn) {



            var elem3 = document.getElementById("cargando_firma");
            elem3.style.display = "block";

        }




        function cargarfirmas_off() {


            var elem3 = document.getElementById("cargando_firma");
            elem3.style.display = "none";
        }


        function firmas_j(periodo_, user_) {


            //setTimeout(function () {

            //    cargarfirmas_on();
            //}, 500);
            document.getElementById("<%=periodo.ClientID %>").value = periodo_;
            document.getElementById("<%=user_q.ClientID %>").value = user_;

            var parameters = new Object();

            parameters.periodo = periodo_;
            parameters.user = user_;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "comisionesventas.aspx/firmas",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (XMLHttpRequest.status == 401) {
                        alert("Fin de la session");
                        location.href = "Acceso.aspx";
                    } else {
                        alert("Error al crear firmas");
                    }
                }
            }).done(function (resp) {
                resp = resp.d;

                var swe = String(resp.includes('**@**')).trim();

                if (swe == "true") {

                    mostrar_btn_envio_correo();

                }
                resp = resp.replace('**@**', '');

                $('#div_firmas').html(resp);
                tabla();

            });

        }

        function mostrar_btn_envio_correo() {


            document.getElementById('ContentPlaceHolder_Contenido_btn_correo_pdf').style.visibility = "initial";



        }




        function click_firma(usuario, periodo, sn) {


            var parameters = new Object();

            parameters.usuario_ = usuario;
            parameters.periodo_ = periodo;
            parameters.si_no = sn;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "comisionesventas.aspx/guardar_firma",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: guarda,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al guardar porcentaje");
                }
            });

        }

        function guarda(resp) {
            $("#cargando_firma").show();
            resp = resp.d;
            if (resp == "OK") {

                alert("Guardado");
                var perido = document.getElementById("<%=periodo.ClientID %>").value;
                var use = document.getElementById("<%=user_q.ClientID %>").value;
                firmas_j(perido, use);
                cierra_cargando();
            }
            else {
                alert("error");

            }

        }


        function guarda2(resp) {

            resp = resp.d;
            if (resp == "OK") {

                alert("Guardado");

            }
            else {
                alert("error");

            }

        }

        function abre_cargando() {
            $("#cargando_firma").show();
            return false;

        }


        function cierra_cargando() {
            $("#cargando_firma").hide();
            return false;
        }
        function guarda_txt(cod_periodo2, vendedor, num_factura, producto, cod_regla, porcentaje, tipo_doc) {


            var parameters = new Object();

            parameters.cod_periodo = cod_periodo2;
            parameters.vender = vendedor;
            parameters.factura = num_factura;
            parameters.product = producto;
            parameters.regla = cod_regla;
            parameters.porcen = porcentaje;
            parameters.porcentaje = document.getElementById("txt_porcentaje_" + cod_periodo2 + vendedor + num_factura + producto + cod_regla + tipo_doc + "").value;


            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "comisionesventas.aspx/txt_porcentaje_editado",
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: guarda2,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error al guardar porcentaje");
                }
            });

        }





        function preguntar_antes_de_enviar()
        {
            if (!confirm('Esta acción va enviar correo con calculo de comisiones. ¿Estás seguro?'))
            {

                return false;
            };
            cargando_2();
        }

    </script>

    <style>
        .dataTables_filter {
            float: right !important;
            animation: reverse;
        }

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

        .modal-dialog {
            width: 98%;
            height: 92%;
            padding: 0;
        }

        .modal-content {
            height: 99%;
        }

        /*teble {
            table-layout: fixed;
            width: 250px;
        }

        teble th, td {
            border: 1px solid blue;
            width: 100px;
            word-wrap: break-word;
        }*/
    </style>

    <script>
        function Modal_Nomina() {
            $('#validar').click();
            tabla();
        }

        function Modal_Editar_Close() {
            $('#cerrar_modal').click();
            $('#cerrar_modal_').click();

        }

        function pdf_comision() {

            var agregar_cobranza = document.getElementById("chk_cobran").checked;

            var tipo_comision = document.getElementById("c_grano_abarrote").value;

            window.open("PDF_COMISION.aspx?C=" + tipo_comisio + "&H=1&T=" + agregar_cobranza);

        }

        function pdf_comision(TIPO_COMISION_, muestra) {

            var agregar_cobranza = document.getElementById("chk_cobran").checked;

            window.open("PDF_COMISION.aspx?C=" + TIPO_COMISION_ + "&H=" + muestra + "&T=" + agregar_cobranza);

        }




        function LoadReglas(result) {

            //quito los options que pudiera tener previamente el combo

            $("#<%=c_reglas.ClientID%>").html("");


            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {

                $("#<%=c_reglas.ClientID%>").append($("<option></option>").attr("value", this.regla).text(this.nombre_regla))

            });


            $("#<%=c_reglas.ClientID %>").change(function () {
                var arr = $(this).val();
                document.getElementById("<%=l_regla.ClientID %>").value = arr;
            })

            $("#<%=c_reglas.ClientID%>").chosen();
            $("#<%=c_reglas.ClientID%>").trigger("chosen:updated");

            $("#<%=c_reglas.ClientID%>").val(" -- Todos -- ");

            document.getElementById("<%=l_regla.ClientID %>").value = " -- Todos -- ";
        }

    </script>

    <link rel="stylesheet" href="assets/data-tables/bootstrap3/dataTables.bootstrap.css" />
    <script type="text/javascript" src="assets/data-tables/jquery.dataTables.js"></script>
    <script type="text/javascript" src="assets/data-tables/bootstrap3/dataTables.bootstrap.js"></script>
    <script src="js/jquery.fixedTblHdrLftCol.js"></script>
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" AsyncPostBackTimeout="360000" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>

    <%-- modal de la nomina --%>
    <asp:Label ID="loc" Visible="false" runat="server" />
    <a id="validar" name="moda_2" href="#modal_valida" role="button" class="btn" data-toggle="modal" style="display: none;"></a>
    <div id="modal_valida" class="modal  fade" role="dialog">
        <div class="modal-dialog modal-lg box-red" role="document">
            <div class="modal-content">
            </div>
        </div>
    </div>


    <a id="moda_cm" name="moda_2" href="#moda_cm_m" role="button" class="btn" data-toggle="modal" style="display: none;"></a>
    <div id="moda_cm_m" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg" style="width: 40%; height: auto;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="Button42" class="close" data-dismiss="modal" aria-hidden="true">×</button>

                </div>
                <div class="modal-body">
                    <div class="row">
                        <h3 id="modal_cm_h3"></h3>
                    </div>
                </div>
            </div>
        </div>

    </div>


    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
        <Triggers>

            <asp:PostBackTrigger ControlID="btn_excel2" />

            <asp:PostBackTrigger ControlID="btn_excel_sn_" />


            <%--<asp:AsyncPostBackTrigger ControlID="btn_pdf" />--%>
            <asp:AsyncPostBackTrigger ControlID="btn_refresh" />
            <asp:AsyncPostBackTrigger ControlID="btn_sn_comision" />

            <asp:AsyncPostBackTrigger ControlID="B_GuardarN" />

            <asp:AsyncPostBackTrigger ControlID="btn_guardar_agroin" />


            <%--        <asp:AsyncPostBackTrigger ControlID="btn_correo2" />

            <asp:AsyncPostBackTrigger ControlID="btn_correo" />--%>

            <asp:AsyncPostBackTrigger ControlID="btn_correo_pdf" />




        </Triggers>
        <ContentTemplate>

            <script type="text/javascript">

                Sys.Application.add_load(BindEvents);

                function BindEvents() {

                    $("#<%=c_vendedor.ClientID%>").chosen();
                    $("#<%=c_vendedor.ClientID%>").trigger("chosen:updated");

                    $("#<%=c_reglas.ClientID%>").chosen();
                    $("#<%=c_reglas.ClientID%>").trigger("chosen:updated");

                    $("#<%=d_grupos_usuario.ClientID%>").chosen();
                    $("#<%=d_grupos_usuario.ClientID%>").trigger("chosen:updated");

                    $("#<%=c_vendedor.ClientID %>").change(function () {

                        var arr = $(this).val();
                        document.getElementById("<%=l_vendedores.ClientID %>").value = arr;

                        var parameters = new Object();

                        parameters.vendedor = document.getElementById("<%=l_vendedores.ClientID %>").value;
                        //alert(vendedor);
                        parameters = JSON.stringify(parameters);
                        $.ajax({
                            type: "POST",
                            url: "COMISIONESVENTAS.aspx/reglas",
                            data: parameters,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: LoadReglas,
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert("Error al cargar reglas");
                            }
                        });
                    })


                    $("#<%=c_reglas.ClientID %>").change(function () {

                        var arr = $(this).val();
                        document.getElementById("<%=l_regla.ClientID %>").value = arr;

                    });


                    $("#<%=d_grupos_usuario.ClientID%>").change(function () {

                        // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                        //este parametro mapeara con el definido en el web service
                        var arr = $(this).val();
                        document.getElementById("<%=l_grupos.ClientID %>").value = arr;

                    });

                }


                function TABLITA() {
                    try {
                        $('#G_Nomina').DataTable({
                            "bDestroy": true,

                            "stateSave": true,
                            "pageLength": 100
                        });

                        $('#G_NOMINA_SN_COMI').DataTable({
                            "bDestroy": true,

                            "stateSave": true,
                            "pageLength": 100
                        });

                    } catch (e) { }
                }


                function click_cambia(cod_periodo_, vendedor_, factura_, producto_, regla_, porcentaje_) {

                    var checkbox = $('#' + cod_periodo_ + vendedor_ + factura_ + producto_ + regla_).is(":checked");

                    //alert(checkbox);

                    if (cod_periodo_ == "") {
                        cod_periodo_ = document.getElementById("<%=txt_periodo.ClientID %>").value;
                    }

                    //alert(id + "---" + rut + "---" + fact + "---" + tipo_doc)
                    var parameters = new Object();
                    parameters.cod_periodo = cod_periodo_;
                    parameters.vendedor = vendedor_;
                    parameters.factura = factura_;
                    parameters.producto = producto_;
                    parameters.regla = regla_;
                    parameters.check = checkbox;
                    parameters.porcentaje = porcentaje_;



                    parameters = JSON.stringify(parameters)

                    $.ajax({
                        type: "POST",
                        url: "COMISIONESVENTAS.aspx/CAMBIA_ESTADO_ON_OFF",
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

                            alert('OK');

                        }
                        else {
                            alert('Error guardar ON_OFF : ' + resp);

                        }


                    });



                }

            </script>



            <%--            <asp:Button runat="server" ID="btn_correo2" OnClick="btn_correo_ServerClick" />
            <button type="button" class="btn btn-info btn-rounded btn-sm" runat="server" id="btn_correo" onclick="cargando_2();" onserverclick="btn_correo_ServerClick">correopdf</button>--%>


            <asp:TextBox ID="l_grupos" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
            <asp:TextBox ID="l_vendedores" runat="server" Style="visibility: hidden; position: absolute; left: 500PX"></asp:TextBox>
            <asp:TextBox ID="l_regla" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>

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
                    <%--         <li>
                        <a href="menu_comisiones.aspx">Comisiones</a>
                        <span class="divider"><i class="fa fa-angle-right"></i></span>
                    </li>--%>
                    <li class="active">Comisiones</li>
                </ul>
            </div>

            <div id="main-content">
                <div class="row">
                    <div class="col-lg-12">
                        <div class="box" runat="server" id="filtros">
                            <div class="box-title">
                                <h3><i class="fa fa-table"></i>Comisión</h3>
                                <div class="box-tool">
                                    <a data-action="collapse" href="#"></a>
                                </div>
                            </div>

                            <div class="box-content">
                                <div class="clearfix"></div>
                                <asp:Panel runat="server" ID="p_enc">
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
                                                        <div class="col-sm-1 col-lg-1 controls" style="visibility: hidden;">
                                                            <i class="fa fa-circle-o-notch fa-spin" id="carga_fecha" runat="server" style="font-size: 2em; display: none;"></i>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="box">
                                                    <div class="pull-right">
                                                        <div class="btn-group">
                                                            <asp:Button ID="btn_productos" runat="server" OnClientClick="CARGANDO();" Style="color: white; float: right;" Class="btn btn-success" Text="FILTRAR" OnClick="btn_productos_Click" />
                                                            <asp:Button ID="btn_nuevo" runat="server" OnClientClick="CARGANDO();" Style="color: white; float: right;" Class="btn btn-info " Text="NUEVO" OnClick="btn_nuevo_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_gif" runat="server" style="display: none; font-size: 3em;"></i>
                                    </div>
                                    <div class="row">




                                        <div class="col-lg-12">

                                            <div class="table-primary table-responsive">
                                                <asp:GridView ID="G_Documentos" runat="server" AutoGenerateColumns="false" ClientIDMode="Static" BorderColor="White" CssClass="table table-advance" DataKeyNames="COD_PERIODO" ShowHeaderWhenEmpty="True" Width="100%" OnRowCommand="G_Documentos_RowCommand">
                                                    <HeaderStyle CssClass="test" />
                                                    <Columns>
                                                        <asp:BoundField DataField="COD_PERIODO" HeaderText="PERIODO" />
                                                        <asp:BoundField DataField="FECHA_CIERRE" HeaderText="FECHA CIERRE" />
                                                        <asp:BoundField DataField="COD_USUARIO" HeaderText="CREADO POR" />
                                                        <asp:BoundField DataField="AUTORIZA" HeaderText="AUTORIZADO POR" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImageButton1" OnClientClick="CARGANDO();" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Visualizar" ImageUrl="img/search.png" Width="25" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        No existen datos.
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel runat="server" ID="p_det" Visible="false">



                                    <asp:UpdatePanel ID="panel_firmas" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="b_vovler" />
                                            <%--                     <asp:AsyncPostBackTrigger ControlID="btn_correo2" />--%>
                                            <asp:AsyncPostBackTrigger ControlID="btn_correo_pdf" />


                                        </Triggers>
                                        <ContentTemplate>
                                            <div class="row">
                                                <%--     <a onclick='abre_cargando()' style='cursor: pointer;'><img style='width: 26px;' src='img/delete.png'></a>
                                                         <a onclick='cierra_cargando()' style='cursor: pointer;'><img style='width: 26px;' src='img/delete.png'></a>--%>
                                                <div class="col-md-10" id="div_firmas">
                                                </div>
                                                <asp:LinkButton ID="btn_correo_pdf" class="btn btn-circle show-tooltip fa fa-envelope" OnClientClick="if(!confirm('¿Estas seguro de enviar correo?')) return false;" Style="margin-left: 5px; visibility: hidden" title="Correo" runat="server" OnClick="btn_correo_2_Click"></asp:LinkButton>
                                                <asp:TextBox ID="periodo" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                                                <asp:TextBox ID="user_q" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                                            </div>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>



                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="b_vovler" />
                                            <%--<asp:AsyncPostBackTrigger ControlID="btn_correo2" />--%>
                                            <asp:AsyncPostBackTrigger ControlID="btn_correo_pdf" />


                                        </Triggers>
                                        <ContentTemplate>
                                            <%--                                            <div class="modal-header text-center box-red">
                                                <h4 class="modal-title w-100 font-weight-bold">Calculo de Comisiones</h4>
                                            </div>--%>

                                            <div class="modal-body mx-3">
                                                <div class="panel-body">



                                                    <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_firma" style="display: none; font-size: 3em;"></i>

                                                    <div class="row">
                                                        <legend class="text-semibold">Datos de la comisión</legend>
                                                        <div class="col-md-12">
                                                            <fieldset>
                                                                <div class="row">
                                                                    <div class="col-md-4">
                                                                        <div class="form-group">
                                                                            <label for="textfield1" class="col-xs-3 col-lg-2 control-label">Fecha cierre:</label>
                                                                            <div class="col-sm-9 col-lg-10 controls">
                                                                                <asp:TextBox runat="server" type="text" class="form-control" ID="txt_fecha" TextMode="Date"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <div class="form-group">
                                                                            <label for="textfield1" class="col-xs-3 col-lg-2 control-label">Periodo:</label>
                                                                            <div class="col-sm-9 col-lg-10 controls">
                                                                                <asp:TextBox ID="txt_periodo" runat="server" class="form-control" ReadOnly="true" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-4">
                                                                        <div class="form-group">
                                                                            <label for="textfield11" class="col-xs-3 col-lg-2 control-label">Autoriza:</label>
                                                                            <div class="col-sm-9 col-lg-10 controls">
                                                                                <asp:TextBox ID="txt_nom_autoriza" runat="server" class="form-control" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <div class="form-group">
                                                                            <label for="textfield12" class="col-xs-3 col-lg-2 control-label">Fecha autoriza:</label>
                                                                            <div class="col-sm-9 col-lg-10 controls">
                                                                                <asp:TextBox runat="server" type="text" class="form-control" ID="txt_fecha_autoriza" TextMode="Date"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-4" style="border: medium;">
                                                                            <div class="form-group">
                                                                                <%--<label for="textfield12" class="col-xs-3 col-lg-2 control-label">Vendedor:</label>--%>
                                                                                <div class="col-sm-9 col-lg-10 controls">
                                                                                    <button type="button" id="btn_crear_periodo" class="btn btn-success btn-rounded btn-sm" runat="server" onclick="cargando_2();" onserverclick="btn_crear_periodo_ServerClick"><i class="fa fa-floppy-o"></i>Crear Periodo</button>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-4" style="border: medium;">
                                                                        <div class="form-group">
                                                                            <label for="textfield12" class="col-xs-3 col-lg-2 control-label">Vendedor:</label>
                                                                            <div class="col-sm-9 col-lg-10 controls">
                                                                                <asp:DropDownList runat="server" type="text" class="form-control" ID="c_vendedor"></asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-md-4" style="border: medium;">
                                                                        <div class="form-group">
                                                                            <label for="textfield12" class="col-xs-3 col-lg-2 control-label">Reglas:</label>
                                                                            <div class="col-sm-9 col-lg-10 controls">
                                                                                <asp:DropDownList runat="server" type="text" class="form-control" ID="c_reglas"></asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>


                                                                </div>

                                                                <div class="row">

                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <label for="textfield1" class="col-xs-3 col-lg-2 control-label">IncluidosPeriodo:</label>
                                                                            <div class="col-sm-9 col-lg-10 controls">

                                                                                <div class="input-group" style="margin-left: 30%" id="check_cerrados">
                                                                                    <asp:RadioButton ID="rd_on" Text="&nbsp;&nbsp;ON" Style="padding-right: 20px;"
                                                                                        GroupName="cerrados" runat="server" />

                                                                                    <asp:RadioButton ID="rd_off" Style="padding-right: 20px;" Text="&nbsp;&nbsp;OFF"
                                                                                        GroupName="cerrados" runat="server" />

                                                                                    <asp:RadioButton ID="rd_todos" Style="padding-right: 20px;" Text="&nbsp;&nbsp;Todos" Checked="True"
                                                                                        GroupName="cerrados" runat="server" />

                                                                                </div>

                                                                            </div>
                                                                        </div>
                                                                    </div>


                                                                    <div class="col-md-4" style="border: medium;">
                                                                        <div class="form-group">
                                                                            <%--<label for="textfield12" class="col-xs-3 col-lg-2 control-label">Vendedor:</label>--%>
                                                                            <div class="col-sm-9 col-lg-10 controls">
                                                                                <button type="button" id="filtra_vendedor" class="btn btn-warning btn-rounded btn-sm" runat="server" onclick="cargando_2();" onserverclick="filtra_vendedor_ServerClick"><i class="fa fa-floppy-o"></i>Filtrar Vendedor</button>
                                                                            </div>
                                                                        </div>
                                                                    </div>


                                                                </div>


                                                                <div class="row" runat="server" id="div_agroin_amaro" visible="true">
                                                                    <hr />
                                                                    <legend class="text-semibold">Importación Agroin</legend>
                                                                    <div class="col-md-4">
                                                                        <div class="form-group">
                                                                            <label for="textfield12" class="col-xs-3 col-lg-3  control-label">Dolar:</label>
                                                                            <div class="col-sm-9 col-lg-10 controls">
                                                                                <asp:TextBox runat="server" type="text" class="form-control" ID="txt_dolar"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <div class="form-group">
                                                                            <label for="textfield12" class="col-xs-3 col-lg-3 control-label">TipoCambio:</label>
                                                                            <div class="col-sm-9 col-lg-10 controls">
                                                                                <asp:TextBox runat="server" type="text" class="form-control" ID="txt_cambio"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <div class="form-group">

                                                                            <button type="button" id="btn_guardar_agroin" class="btn btn-success btn-rounded btn-sm" style="background-color: #bd761d !important" runat="server" onclick="cargando_2();" onserverclick="btn_guardar_agroin_ServerClick"><i class="fa fa-floppy-o"></i></button>
                                                                            <%--<button type="button" id="Button1" class="btn btn-success btn-rounded btn-sm" runat="server" onclick="cargando_2();" onserverclick="B_GuardarN_ServerClick"><i class="fa fa-floppy-o"></i>Guardar</button>--%>
                                                                        </div>
                                                                    </div>
                                                                </div>


                                                                <div class="row" runat="server" id="div1" visible="true">
                                                                    <hr />
                                                                    <legend class="text-semibold">Grupos</legend>

                                                                    <div class="col-md-4">
                                                                        <div class="form-group">
                                                                            <%--                                          <label for="textfield1" class="col-xs-3 col-lg-2 control-label">Grupos:</label>--%>
                                                                            <div class="col-sm-9 col-lg-10 controls">
                                                                                <asp:ListBox ID="d_grupos_usuario" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>

                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="btn-toolbar pull-right">

                                                                        <asp:LinkButton ID="btn_refresh" class="btn btn-circle show-tooltip fa fa-refresh" Visible="true" OnClientClick="cargando_2();" Style="margin-left: 5px; color: white !important; background-color: #0b78ff !important;" title="Cargar todo" runat="server" OnClick="btn_refresh_Click"></asp:LinkButton>

                                                                    </div>
                                                                </div>


                                                                <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_gif_2" runat="server" style="display: none; font-size: 3em;"></i>
                                                            </fieldset>
                                                            <hr />
                                                            <div class="text-left">
                                                                <%--<asp:LinkButton ID="btn_refresh" class="btn btn-circle show-tooltip fa fa-refresh" Visible="true" OnClientClick="cargando_2();" Style="margin-left: 5px" title="Cargar todo" runat="server" OnClick="btn_refresh_Click"></asp:LinkButton>--%>



                                                                <button type="button" id="B_GuardarN" class="btn btn-success btn-rounded btn-sm" runat="server" onclick="cargando_2();" onserverclick="B_GuardarN_ServerClick"><i class="fa fa-floppy-o"></i>Guardar Periodo</button>
                                                                <button type="button" class="btn btn-info btn-rounded btn-sm" runat="server" id="b_total_" onclick="cargando_2();" onserverclick="b_total__ServerClick">Resumen</button>
                                                                <button type="button" class="btn btn-inverse btn-rounded btn-sm" runat="server" id="b_vovler" onserverclick="b_vovler_ServerClick">Salir</button>


                                                                <%--<button type="button" class="btn btn-info btn-rounded btn-sm" runat="server" id="btn_jbravo_pdf_correo" onclick="cargando_2();" onserverclick="btn_jbravo_pdf_correo_ServerClick">CreaPDF</button>--%>

                                                                <%--<button type="button" class="btn btn-info btn-rounded btn-sm" runat="server" id="btn_correo" onclick="cargando_2();" onserverclick="btn_correo_ServerClick">correopdf</button>--%>
                                                            </div>
                                                            <br></br>

                                                            <div class="row">


                                                                <asp:Label runat="server" ID="l_total_comision"></asp:Label>
                                                                <br />
                                                                <asp:Label runat="server" ID="l_sobre_comision"></asp:Label>

                                                            </div>

                                                            <div class="row">
                                                                <div class="btn-toolbar pull-right">

                                                                    <div class="btn-group">
                                                                        <asp:LinkButton ID="btn_sn_comision" class="btn btn-circle show-tooltip fa fa-plus" Visible="true" OnClientClick="cargando_2();" Style="margin-left: 5px; background-color: #8cff94 !important;" title="Traer sin comisión" runat="server" OnClick="btn_sn_comis_Click"></asp:LinkButton>

                                                                    </div>

                                                                </div>



                                                                <div class="col-md-12">

                                                                    <div class="tabbable">
                                                                        <ul id="myTab1" class="nav nav-tabs">
                                                                            <li class="active"><a href="#home1" data-toggle="tab"><i class="fa fa-home"></i>Detalle Comisiones</a></li>
                                                                            <li class=""><a href="#profile1" data-toggle="tab"><i class="fa fa-user"></i>Detalle sin clasificación a comisión</a>

                                                                            </li>
                                                                        </ul>

                                                                        <div id="myTabContent1" class="tab-content">
                                                                            <div class="tab-pane fade active in" id="home1">
                                                                                <div class="table-primary table-responsive" style="overflow: auto;">
                                                                                    <div class="btn-toolbar pull-right">
                                                                                        <asp:LinkButton ID="btn_excel2" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Exportar a Excel" runat="server" OnClick="btn_excel2_Click"></asp:LinkButton>
                                                                                    </div>
                                                                                    <asp:GridView ID="G_Nomina" runat="server" OnRowCommand="G_Nomina_RowCommand" AutoGenerateColumns="false" OnRowDataBound="G_Nomina_RowDataBound" ClientIDMode="Static" BorderColor="White" CssClass=""
                                                                                        DataKeyNames="númfactura" ShowHeaderWhenEmpty="True" Width="100%" Font-Size="XX-Small">
                                                                                        <HeaderStyle CssClass="test" />
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="númfactura" HeaderText="FACTURA" />
                                                                                            <asp:BoundField DataField="producto" HeaderText="CODPROD" />
                                                                                            <asp:BoundField DataField="descproducto" HeaderText="PROD" />
                                                                                            <asp:BoundField DataField="nombrecliente" HeaderText="CLIENTE" />
                                                                                            <asp:BoundField DataField="codvendedor" HeaderText="CODVENDEDOR" />
                                                                                            <asp:BoundField DataField="vendedor" HeaderText="VENDEDOR" />

                                                                                            <asp:BoundField DataField="neto_pesos" HeaderText="NETO" />
                                                                                            <asp:BoundField DataField="porcentaje" HeaderText="PORCENTAJE" />
                                                                                            <asp:BoundField DataField="regla" HeaderText="REGLA" />

                                                                                            <asp:BoundField DataField="nombre_comision" HeaderText="NOMBRE REGLA" />
                                                                                            <asp:BoundField DataField="montocomision" HeaderText="COMISION" />

                                                                                            <asp:BoundField DataField="periodo" HeaderText="PERIODO FACTURA" />
                                                                                            <asp:BoundField DataField="cod_periodo" HeaderText="PERIODO COMISION" />
                                                                                            <asp:BoundField DataField="fecha_pago2" HeaderText="FECHA PAGO" />

                                                                                            <asp:BoundField DataField="cod_periodo" HeaderText="OnOff" />

                                                                                            <asp:BoundField DataField="tipo_doc" HeaderText="T.Doc" />
                                                                                            <asp:BoundField DataField="PorcDescuento" HeaderText="Descuento" />

                                                                                            <asp:BoundField DataField="descr" HeaderText="descr" />
                                                                                            <asp:BoundField DataField="porcentaje_edit" HeaderText="NUEVO_PORCEN(,)" />

                                                                                            <asp:BoundField DataField="grupo" HeaderText="Grupo" />

                                                                                        </Columns>
                                                                                        <EmptyDataTemplate>
                                                                                            No existen datos.
                                                                                        </EmptyDataTemplate>
                                                                                    </asp:GridView>


                                                                                    <asp:GridView ID="G_NOMINA_eXCEL" Visible="false" runat="server" OnRowCommand="G_Nomina_RowCommand" AutoGenerateColumns="false" OnRowDataBound="G_NOMINA_eXCEL_RowDataBound" ClientIDMode="Static" BorderColor="White" CssClass=""
                                                                                        DataKeyNames="númfactura" ShowHeaderWhenEmpty="True" Width="100%" Font-Size="XX-Small">
                                                                                        <HeaderStyle CssClass="test" />
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="númfactura" HeaderText="FACTURA" />
                                                                                            <asp:BoundField DataField="producto" HeaderText="CODPROD" />
                                                                                            <asp:BoundField DataField="descproducto" HeaderText="PROD" />
                                                                                            <asp:BoundField DataField="nombrecliente" HeaderText="CLIENTE" />
                                                                                            <asp:BoundField DataField="codvendedor" HeaderText="CODVENDEDOR" />
                                                                                            <asp:BoundField DataField="vendedor" HeaderText="VENDEDOR" />
                                                                                            <asp:BoundField DataField="neto_pesos" HeaderText="NETO" />
                                                                                            <asp:BoundField DataField="porcentaje" HeaderText="PORCENTAJE" />
                                                                                            <asp:BoundField DataField="regla" HeaderText="REGLA" />
                                                                                            <asp:BoundField DataField="nombre_comision" HeaderText="NOMBRE REGLA" />
                                                                                            <asp:BoundField DataField="montocomision" HeaderText="COMISION" />
                                                                                            <asp:BoundField DataField="periodo" HeaderText="PERIODO FACTURA" />
                                                                                            <asp:BoundField DataField="cod_periodo" HeaderText="PERIODO COMISION" />
                                                                                            <asp:BoundField DataField="fecha_pago2" HeaderText="FECHA PAGO" />
                                                                                            <asp:BoundField DataField="cod_periodo" HeaderText="OnOff" />
                                                                                            <asp:BoundField DataField="tipo_doc" HeaderText="T.Doc" />
                                                                                            <asp:BoundField DataField="PorcDescuento" HeaderText="Descuento" />
                                                                                            <asp:BoundField DataField="descr" HeaderText="descr" />
                                                                                            <asp:BoundField DataField="porcentaje_edit" HeaderText="NUEVO_PORCEN(,)" />

                                                                                        </Columns>
                                                                                        <EmptyDataTemplate>
                                                                                            No existen datos.
                                                                                        </EmptyDataTemplate>
                                                                                    </asp:GridView>


                                                                                </div>


                                                                            </div>
                                                                            <div class="tab-pane fade" id="profile1">

                                                                                <div class="table-primary table-responsive" style="overflow: auto;">
                                                                                    <div class="btn-toolbar pull-right">
                                                                                        <asp:LinkButton ID="btn_excel_sn_" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px" title="Exportar a Excel" runat="server" OnClick="btn_excel_sn__Click"></asp:LinkButton>
                                                                                    </div>
                                                                                    <div class="col-md-6" style="border: medium;">
                                                                                        <div class="form-group">
                                                                                            <label for="textfield12" class="col-xs-3 col-lg-2 control-label">Vendedor:</label>
                                                                                            <div class="col-sm-6 col-lg-6 controls">
                                                                                                <asp:DropDownList runat="server" type="text" class="form-control" ID="c_vendedor_2"></asp:DropDownList>
                                                                                            </div>
                                                                                            <div class="col-sm-4 col-lg-4 controls">
                                                                                                <button type="button" id="btn_filtra_ven" class="btn btn-warning btn-rounded btn-sm" runat="server" onclick="cargando_2();" onserverclick="btn_filtra_ven_ServerClick"><i class="fa fa-floppy-o"></i>Filtrar Vendedor</button>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>

                                                                                    <asp:GridView ID="G_NOMINA_SN_COMI" runat="server" OnRowCommand="G_NOMINA_SN_COMI_RowCommand" AutoGenerateColumns="false" OnRowDataBound="G_NOMINA_SN_COMI_RowDataBound" ClientIDMode="Static" BorderColor="White" CssClass=""
                                                                                        DataKeyNames="númfactura" ShowHeaderWhenEmpty="True" Width="100%" Font-Size="XX-Small">
                                                                                        <HeaderStyle CssClass="test" />
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="númfactura" HeaderText="FACTURA" />
                                                                                            <asp:BoundField DataField="producto" HeaderText="CODPROD" />
                                                                                            <asp:BoundField DataField="descproducto" HeaderText="PROD" />
                                                                                            <asp:BoundField DataField="nombrecliente" HeaderText="CLIENTE" />
                                                                                            <asp:BoundField DataField="codvendedor" HeaderText="CODVENDEDOR" />
                                                                                            <asp:BoundField DataField="vendedor" HeaderText="VENDEDOR" />
                                                                                            <asp:BoundField DataField="neto_pesos" HeaderText="NETO" />

                                                                                            <asp:BoundField DataField="periodo" HeaderText="PERIODO FACTURA" />
                                                                                            <asp:BoundField DataField="periodo_pago" HeaderText="PERIODO COMISION" />
                                                                                            <asp:BoundField DataField="fecha_pago" HeaderText="FECHA PAGO" />

                                                                                            <asp:BoundField DataField="tipo_doc" HeaderText="T.Doc" />
                                                                                            <asp:BoundField DataField="PorcDescuento" HeaderText="Descuento" />
                                                                                            <asp:BoundField DataField="regla_2" HeaderText="Tipo" />

                                                                                        </Columns>
                                                                                        <EmptyDataTemplate>
                                                                                            No existen datos.
                                                                                        </EmptyDataTemplate>
                                                                                    </asp:GridView>




                                                                                    <asp:GridView ID="G_NOMINA_SN_COMI_excel" Visible="false" runat="server" OnRowCommand="G_NOMINA_SN_COMI_excel_RowCommand" AutoGenerateColumns="false" OnRowDataBound="G_NOMINA_SN_COMI_excel_RowDataBound" ClientIDMode="Static" BorderColor="White" CssClass=""
                                                                                        DataKeyNames="númfactura" ShowHeaderWhenEmpty="True" Width="100%" Font-Size="XX-Small">
                                                                                        <HeaderStyle CssClass="test" />
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="númfactura" HeaderText="FACTURA" />
                                                                                            <asp:BoundField DataField="producto" HeaderText="CODPROD" />
                                                                                            <asp:BoundField DataField="descproducto" HeaderText="PROD" />
                                                                                            <asp:BoundField DataField="nombrecliente" HeaderText="CLIENTE" />
                                                                                            <asp:BoundField DataField="codvendedor" HeaderText="CODVENDEDOR" />
                                                                                            <asp:BoundField DataField="vendedor" HeaderText="VENDEDOR" />
                                                                                            <asp:BoundField DataField="neto_pesos" HeaderText="NETO" />

                                                                                            <asp:BoundField DataField="periodo" HeaderText="PERIODO FACTURA" />
                                                                                            <asp:BoundField DataField="periodo_pago" HeaderText="PERIODO COMISION" />
                                                                                            <asp:BoundField DataField="fecha_pago" HeaderText="FECHA PAGO" />

                                                                                            <asp:BoundField DataField="tipo_doc" HeaderText="T.Doc" />
                                                                                            <asp:BoundField DataField="PorcDescuento" HeaderText="Descuento" />
                                                                                            <asp:BoundField DataField="regla_2" HeaderText="Tipo" />

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

                                                <div class="box-content">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="box">
                                                                <div class="box-title">
                                                                    <h3><i class="fa fa-list"></i>Reglas</h3>
                                                                    <div class="box-tool" style="margin-top: -0.5%;">
                                                                        <%--    <a data-action="collapse" id="iniciar_cerrado" href="#"><i class="fa fa-chevron-up"></i></a>--%>
                                                                        <%--<a data-action="close" href="#"><i class="fa fa-times"></i></a>--%>
                                                                    </div>

                                                                </div>
                                                                <div class="box-content" style="display: block;">
                                                                    <div class="row">
                                                                        <div class="col-md-3">

                                                                            <ul class="list-unstyled">
                                                                                <li><span class="label label-important" style="background-color: #3A01DF;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_1 NORMAL MATERIAS PRIMAS</li>
                                                                                <li><span class="label label-important" style="background-color: #6A0888;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_2 CLIENTES VI</li>
                                                                                <li><span class="label label-important" style="background-color: #00A5CD;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_3 IMPORTACIONES USA Y AGROIN</li>
                                                                                <li><span class="label label-important" style="background-color: #3A01DF;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_4 CLIENTE COMPARTIDO</li>
                                                                                <li><span class="label label-important" style="background-color: #6A0888;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_5 LOGISTICA MARITIMA + NUTRECO</li>
                                                                                <li><span class="label label-important" style="background-color: #00A5CD;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_6 DESESTIBA EWOS NO IMPORTACIONES</li>

                                                                            </ul>
                                                                        </div>

                                                                        <div class="col-md-3">
                                                                            <ul class="list-unstyled">

                                                                                <li><span class="label label-important" style="background-color: #3A01DF;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_7 LOGISTICA CHIMOLSA</li>
                                                                                <li><span class="label label-important" style="background-color: #6A0888;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_8 VENTAS TRIGO COMPARTIDA </li>
                                                                                <li><span class="label label-important" style="background-color: #00A5CD;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_9 COMPRAS CONSUMO ANIMAL, SERVICIOS</li>
                                                                                <li><span class="label label-important" style="background-color: #3A01DF;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_10 COMISION SOBRE VENTAS CHARRIER, PINO, NAVARRO, A.GONZALES, CRISTINA (TORO)</li>
                                                                                <li><span class="label label-important" style="background-color: #6A0888;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_11 COMISION SOBRE COMISIONES  RAFAEL PINO, ANDRES GONZALES (AMARO)</li>

                                                                            </ul>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <ul class="list-unstyled">

                                                                                <li><span class="label label-important" style="background-color: #00A5CD;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_12 FLETES TERRESTRES SOPRODI A.GONZALES, RAFAEL PINO</li>
                                                                                <li><span class="label label-important" style="background-color: #3A01DF;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_13 LOGISTICA CARGILL</li>
                                                                                <li><span class="label label-important" style="background-color: #6A0888;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_14 COMISION SOBRE COMISIONES ANDRES NAVARRO (BARGETTO)</li>
                                                                                <li><span class="label label-important" style="background-color: #00A5CD;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_15 VENTAS AGROIN A CHILE (AMARO)</li>
                                                                                <li><span class="label label-important" style="background-color: #6A0888;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_16 ABARROTES  </li>

                                                                            </ul>
                                                                        </div>

                                                                        <div class="col-md-3">
                                                                            <ul class="list-unstyled">

                                                                                <li><span class="label label-important" style="background-color: #00A5CD;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_17 SERVICIOS JORGE PARRA </li>
                                                                                <li><span class="label label-important" style="background-color: #3A01DF;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_18 IMPORTACION DIRECTA</li>
                                                                                <li><span class="label label-important" style="background-color: #00A5CD;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_19 VENTA AGROIN (AMARO)</li>
                                                                                <li><span class="label label-important" style="background-color: #3A01DF;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_20 AGRICOLA TARAPACA (COBRANZAS)</li>
                                                                                <li><span class="label label-important" style="background-color: #3A01DF;">&nbsp;&nbsp;&nbsp;&nbsp;</span>REGLA_21 PARTICIPACION DE VENDEDORES (TORO) </li>

                                                                                <li><span class="label label-important" style="background-color: #6A0888;">&nbsp;&nbsp;&nbsp;&nbsp;</span>CM NOTAS DE CREDITO  </li>

                                                                            </ul>
                                                                        </div>
                                                                    </div>
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
                                                                    <h3><i class="fa fa-list"></i>Abarrotes Valores de periodo anterior</h3>
                                                                    <div class="box-tool" style="margin-top: -0.5%;">
                                                                    </div>

                                                                </div>
                                                                <div class="box-content" style="display: block;">
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">FACTURACION TOTAL ABARROTES(C/ARICA)</label>
                                                                            <div class="col-sm-3 col-lg-3 controls">
                                                                                <div class="input-group">

                                                                                    <asp:TextBox ID="t_total_abarrotes_c_arica" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>

                                                                                </div>
                                                                            </div>

                                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">DESARROLLO DE MERCADO ( < mm$9) (RO)</label>
                                                                            <div class="col-sm-3 col-lg-3 controls">
                                                                                <div class="input-group">

                                                                                    <asp:TextBox ID="t_ro_" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>

                                                                                </div>
                                                                            </div>

                                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">FACTURACION TOTAL ABARROTES(S/ARICA)</label>
                                                                            <div class="col-sm-3 col-lg-3 controls">
                                                                                <div class="input-group">

                                                                                    <asp:TextBox ID="t_total_abarrotes_s_arica" CssClass="form-control" runat="server" Width="100%"></asp:TextBox>

                                                                                </div>
                                                                            </div>



                                                                        </div>
                                                                    </div>
                                                                    <div class="row">

                                                                        <div class="col-sm-3 col-lg-3 controls">
                                                                            <div class="input-group">
                                                                                <br />
                                                                                <asp:Button ID="btn_guardar_abarr" runat="server" OnClientClick="CARGANDO();" Style="color: white; float: right;" Class="btn btn-info " Text="Guardar" OnClick="btn_guardar_abarr_Click" />

                                                                            </div>
                                                                        </div>

                                                                    </div>
                                                                </div>


                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>




                                            </div>
                                            <div class="modal-footer">
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>

            </div>




            <a id="modal_comision_btn" name="moda_35" href="#modal_comision" role="button" class="btn" data-toggle="modal" style="display: none;"></a>
            <div id="modal_comision" class="modal fade">
                <div class="modal-dialog modal-lg" style="width: 40%; height: auto;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" id="Button75" class="close" data-dismiss="modal" aria-hidden="true">×</button>

                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <%--<asp:LinkButton ID="btn_pdf" class="btn btn-circle show-tooltip fa fa-file-text-o" Visible="true" Style="margin-left: 5px; background-color: red;" title="PDF" runat="server" OnClientClick="pdf_comision()"></asp:LinkButton>--%>

                                <div class="col-sm-9 col-lg-10 controls">
                                    <%--             <asp:DropDownList runat="server" type="text" class="form-control" ID="c_grano_abarrote">

                                        <asp:ListItem Value="1"> Granos </asp:ListItem>
                                        <asp:ListItem Value="2"> Abarrotes </asp:ListItem>
                                        <asp:ListItem Value="3"> Todos </asp:ListItem>

                                    </asp:DropDownList>--%>


                                    <select class="form-control input-sm" id="c_grano_abarrote">
                                        <option value="1">Granos</option>
                                        <option value="2">Abarrotes</option>
                                        <option value="3" selected>Todos</option>

                                    </select>
                                </div>

                                <div class="form-check">
                                </div>

                                <%--BOTON QUE DIBUJA EL PDF, GUARDA Y LEVANTA PAGINA BLANK MOSTRAR--%>
                                <button type="button" id="btn_pdf2" class="btn btn-circle show-tooltip fa fa-file-text-o" onclick="pdf_comision();" aria-hidden="true">PDF</button>

                                <div class="col-sm-9 col-lg-10 controls">
                                    <label class="form-check-label">
                                        <input class="form-check-input" id="chk_cobran" type="checkbox" value="">
                                        <%--                                        <asp:CheckBox runat="server" ID="chk_cobranza" Checked="true" AutoPostBack="true" OnCheckedChanged="chk_cobranza_CheckedChanged" ClientIDMode="AutoID" CssClass="form-check-input" />--%>
                                        <span class="form-check-sign"></span>
                                        Agregar Cobranza
                                    </label>
                                </div>


                                <div class="col-sm-12">
                                    <h3>Comisión</h3>
                                    <div class="form-horizontal">
                                        <div id="div_tabla_totales" runat="server">
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





        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        $(document).ready(function () {

            try {
                $('#G_Documentos').DataTable({

                });
            } catch (e) { }

        })

        //function tabla_refresh() {

        //    try {
        //        $('#G_Documentos').DataTable({
        //            "dom": 'T<"clear">lfrtip',
        //            "tableTools": {
        //                "aButtons": [
        //                    "copy",
        //                    "print",
        //                    {
        //                        "sExtends": "collection",
        //                        "sButtonText": "Save",
        //                        "aButtons": ["csv", {
        //                            "sExtends": "xls",
        //                            "sTitle": "Listado"
        //                        }, {
        //                            "sExtends": "pdf",
        //                            "sPdfOrientation": "landscape",
        //                            "sTitle": "Listado"
        //                        }]
        //                    }

        //                ]
        //            },
        //        });
        //    } catch (e) { }
        //}

    </script>

</asp:Content>
