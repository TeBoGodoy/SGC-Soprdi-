<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" AutoEventWireup="True" UICulture="es-ES" Culture="es-ES" CodeBehind="REPORTE_CATEGORIA.aspx.cs" Inherits="SoprodiApp.REPORTE_CATEGORIA" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
    <style>
        .test {
            background-color: #428bca !important;
            color: white !important;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">



    <script>
        function abremodal_(id_categ, nom_categ) {

            document.getElementById("<%=tx_id_categ.ClientID %>").value = id_categ;
            document.getElementById("<%=nombre_titu.ClientID %>").value = nom_categ;

            producto_cb(id_categ);

            tabla_productos_3(id_categ);


            chosen_update();

            
            
            $('#titulo_modal').html("<h3 style='color: cornflowerblue;font-size: -webkit-xxx-large;'>" + nom_categ + "</h3>");

            //chosen_update();

        }


        function carga_producs() {

            var id = document.getElementById("<%=tx_id_categ.ClientID %>").value;
            //alert(id);
            tabla_productos_3(id);



        }


        function producto_cb(categ) {

            var parameters = new Object();

            parameters.sw = categ;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_CATEGORIA.aspx/PRODUCTO_CB",
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

        function tabla_productos_3(categ_) {

            var parameters = new Object();

            parameters.categ = categ_;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_CATEGORIA.aspx/CargarProductos",
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
                    modal_unidad();
                }
            }).done(function (result) {

                if (result.d == "") {

                    $('#tabla_productos').html("<h4>No hay productos en esta categoría</h4>");

                }

                $.each(result.d, function () {

                    $('#tabla_productos').html(this.tabla_html);

                });

                modal_unidad();
            });
        }



        function LoadProduc_kg(result) {

            //quito los options que pudiera tener previamente el combo

            $("#<%=cb_productos_kg.ClientID%>").html("");


            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {

                $("#<%=cb_productos_kg.ClientID%>").append($("<option></option>").attr("value", this.producto).text(this.descproducto))

            });


            $("#<%=cb_productos_kg.ClientID%>").chosen();
            $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");



        }


        function QuitarProducto(id_prod_) {


            var parameters = new Object();

            parameters.id_prod = id_prod_;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_CATEGORIA.aspx/DELETE_PRODUCTO",
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

        function incluir_producto() {


            var parameters = new Object();

            parameters.id_cat =  document.getElementById("<%=tx_id_categ.ClientID%>").value;;
            parameters.id_prod =  document.getElementById("<%=producto_modal.ClientID%>").value;;

            parameters = JSON.stringify(parameters);

            $.ajax({
                type: "POST",
                url: "REPORTE_CATEGORIA.aspx/INSERT_PRODUCTO",
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
 
                chosen_update();
                var id_catg_aux = document.getElementById("<%=tx_id_categ.ClientID%>").value;

                producto_cb(id_catg_aux);
                var parameters = new Object();

                parameters.categ = id_catg_aux;

                parameters = JSON.stringify(parameters);

                $.ajax({
                    type: "POST",
                    url: "REPORTE_CATEGORIA.aspx/CargarProductos",
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

                    if (result.d == "") {

                        $('#tabla_productos').html("<h4>No hay productos en esta categoría</h4>");

                    }

                    $.each(result.d, function () {

                        $('#tabla_productos').html(this.tabla_html);

                    });

                  
                });



            }
            else {
                alert("error");
                chosen_update();
            }

        }



        function insert() {
            var id = document.getElementById("<%=tx_id_categ.ClientID %>").value;
            var nomb = document.getElementById("<%=nombre_titu.ClientID %>").value;
            abremodal_(id, nomb);


        }
        function modal_unidad() {
            //alert("aca");
            document.getElementById("div_unidad_").click();
            chosen_update();
        }
        function chosen_update() {
            try {

                $("#<%=cb_productos_kg.ClientID%>").chosen();
                $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");
            } catch (r) { }

        }

        function esconde() {
            try {
                //var elem3 = document.getElementById("detalle_prod");
                //elem3.style = "visibility:hidden";
                //document.getElementById("detalle_prod").style = "visible:hidden";

                $("#<%=cb_productos_kg.ClientID%>").chosen();
                $("#<%=cb_productos_kg.ClientID%>").trigger("chosen:updated");
            } catch (e) { }
        }


    </script>





    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>



    <script type="text/javascript">



        Sys.Application.add_load(BindEvents);

        function BindEvents() {

            $("#<%=cb_productos_kg.ClientID%>").change(function () {

                var arr = $(this).val();
                document.getElementById("<%=producto_modal.ClientID %>").value = arr;


            });
        }


    </script>
    <%-- <div class="page-title" style="margin-top: -27px">
        <div>
            <i class="fa fa-file-o fa-3x"></i><a class="h1" href="MENU_Finanzas.aspx">Finanzas</a>
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
            <li class="active">Categoría</li>
        </ul>
    </div>

    <div id="main-content">
        <div class="row">
            <div class="col-md-12">
                <div class="box">
                    <div class="box-title">
                        <h3><i class="fa fa-table"></i>Categorias</h3>
                    </div>
                    <div class="box-content">

                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="form-horizontal">
                                        <!-- ********************************************************************* -->
                                        <div class="form-group">
                                            <asp:TextBox runat="server" ID="T_ID_BANCO" Visible="false"></asp:TextBox>
                                            <div style="visibility: hidden; position: absolute;">
                                                <label class="col-sm-2 control-label">
                                                    <b>Codigo</b>
                                                </label>
                                                <div class="col-sm-3 controls">
                                                    <asp:TextBox runat="server" ID="T_Cod_banco" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                            <label class="col-sm-2 control-label">
                                                <b>Nombre</b>
                                            </label>
                                            <div class="col-sm-3 controls">
                                                <asp:TextBox runat="server" ID="T_Nom_banco" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1 col-sm-offset-1 controls">
                                                <button id="btn_nuevo_banco" class="btn btn-success icon-plus btn-circle" runat="server" onserverclick="btn_nuevo_banco_ServerClick"><i class="fa fa-plus"></i></button>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-2 col-sm-offset-2 controls">
                                                <div class="input-group">
                                                    <asp:Button runat="server" ID="B_Guardar" CssClass="btn btn-primary" Text="Crear" OnClick="B_Guardar_Click" Visible="false" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:GridView CssClass="table fill-head table-bordered filtrar" OnRowDataBound="G_Banco_DataBound" ID="G_Banco" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" Width="100%" OnRowCommand="G_Banco_RowCommand" DataKeyNames="id, nom_categ">
                                            <HeaderStyle CssClass="test" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Editar">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="B_Editar_Banco" runat="server" ImageUrl="img/pencil.png" Width="25"
                                                            CommandName="Editar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Borrar">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="B_Borrar_Banco" runat="server" ImageUrl="img/delete.png" Width="25"
                                                            CommandName="Eliminar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="id" DataField="id" Visible="false" />
                                                <asp:BoundField HeaderText="Nombre" DataField="nom_categ" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                No existen datos.
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                </div>

                                <%--<asp:Button runat="server" ClientIDMode="Static" ID="Button2" Style="visibility: hidden; position: absolute;" Text="Crear" OnClick="Button2_Click" />--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:TextBox ID="tx_id_categ" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="producto_modal" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
    <asp:TextBox ID="nombre_titu" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>


    <a href="#div_unidad" id="div_unidad_" role="button" class="btn" style="visibility: hidden; position: absolute;" data-toggle="modal"></a>

    <div id="div_unidad" class="modal fade">
        <div class="modal-dialog modal-lg">
            <div class="modal-content" style="height: auto;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <div id="titulo_modal"></div>
                </div>
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
                                                
                                                <div class="col-sm-6 col-lg-6">
                                                    <div class="controls">
                                                        <asp:DropDownList runat="server" Style="width: 200px !important;" ID="cb_productos_kg" onchange="esconde()" ClientIDMode="Static" CssClass="form-control chosen"></asp:DropDownList>
                                                    </div>


                                                </div>

                                            </div>
                                            <%--<button id="Button1" class="btn btn-success icon-plus btn-circle" onclick="incluir_producto();"><i class="fa fa-plus"></i></button>--%>
                                    <input id="btn_guardar_prod_equi" type="button" class="btn btn-success" value="Guardar" onclick="incluir_producto()" style="display: block; float: right;" />

                                        </div>
                                    </div>
                                </div>
                                <br />

                                <%--          <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                <div class="row">
                                    <div class="col-sm-12">

                                        <div id="tabla_productos"></div>

                                    </div>
                                </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

            </div>
            <div class="modal-footer">
                <button class="btn" data-dismiss="modal" onclick="esconde(); chosen_update();" aria-hidden="true">Cerrar</button>
            </div>
        </div>
    </div>
    </div>


</asp:Content>

