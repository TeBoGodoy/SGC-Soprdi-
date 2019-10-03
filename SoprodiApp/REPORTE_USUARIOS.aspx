<%@ Page MaintainScrollPositionOnPostback="true" Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="REPORTE_USUARIOS.aspx.cs" Inherits="SoprodiApp.REPORTE_USUARIOS" %>

<%@ OutputCache Location="None" NoStore="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">

    <script>

        $(document).ready(function () {

            function BindEvents() {

                superfiltro();
                $("#<%=d_unidade_negocio.ClientID%>").chosen();
                $("#<%=d_unidade_negocio.ClientID%>").trigger("chosen:updated");


                $("#<%=d_grupos_2.ClientID%>").chosen();
                $("#<%=d_grupos_2.ClientID%>").trigger("chosen:updated");

                $("#<%=d_unidade_negocio.ClientID%>").change(function () {


                    var arr = $(this).val();
                    document.getElementById("<%=l_unidad_negocio.ClientID %>").value = arr;

                    if (arr == "") {
                        document.getElementById("<%=DIV2_3.ClientID %>").style.display = "none";
                    } else {

                        document.getElementById("<%=DIV2_3.ClientID %>").style.display = "block";
                    }

                    var parameters = new Object();

                    parameters.u_negocio = document.getElementById("<%=l_unidad_negocio.ClientID %>").value;

                    parameters = JSON.stringify(parameters)

                    $.ajax({
                        type: "POST",
                        url: "REPORTE_USUARIOS.aspx/GRUPOS_POR_UNIDAD",
                        data: parameters,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: LOAD_GRUPOS,
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Error al cargar GRUPOS");
                        }
                    });

                });

                $("#<%=d_grupos.ClientID%>").chosen();

                $("#<%=d_pantallas.ClientID%>").chosen();



                $("#<%=d_grupos.ClientID%>").change(function () {

                    // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                    //este parametro mapeara con el definido en el web service
                    var arr = $(this).val();
                    document.getElementById("<%=l_grupos.ClientID %>").value = arr;

                });

                $("#<%=d_pantallas.ClientID%>").change(function () {

                    // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                    //este parametro mapeara con el definido en el web service
                    var arr = $(this).val();
                    document.getElementById("<%=l_pantallas.ClientID %>").value = arr;

                });


                $("#<%=d_grupos_2.ClientID%>").change(function () {

                    // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                    //este parametro mapeara con el definido en el web service
                    var arr = $(this).val();
                    document.getElementById("<%=l_unidad_negocio.ClientID %>").value = arr;

                });
                document.getElementById("<%=l_unidad_negocio.ClientID %>").value = "";
            }



        });
        $().ready(function () {

            superfiltro();
            $("#<%=d_unidade_negocio.ClientID%>").change(function () {

                // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                //este parametro mapeara con el definido en el web service
                var arr = $(this).val();
                document.getElementById("<%=l_unidad_negocio.ClientID %>").value = arr;

            });

            $("#<%=d_grupos_2.ClientID%>").change(function () {

                // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                //este parametro mapeara con el definido en el web service
                var arr = $(this).val();
                document.getElementById("<%=l_unidad_negocio.ClientID %>").value = arr;

            });


        });
        function registre_vended() {
            document.getElementById("<%=DIV222.ClientID %>").style.display = "none";
            document.getElementById("<%=chk_adm.ClientID %>").checked = true;
            document.getElementById("<%=chk_vend.ClientID %>").checked = false;

            document.getElementById("<%=l_unidad_negocio.ClientID %>").value = "";
            alert("Debe registrar un código valido de vendedor como usuario");



        }
        function muestra_check() { try { document.getElementById("check_enviar_2").style.display = "block"; } catch (e) { } }
        function muestra_check2() { try { document.getElementById("check_enviar_2").style.display = "none"; } catch (e) { } }


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


        function LOAD_GRUPOS(result) {

            //quito los options que pudiera tener previamente el combo
        
              ////$("#<%=d_grupos.ClientID%>").html("");


            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {

                $("#<%=d_grupos.ClientID%>").append($("<option></option>").attr("value", this.COD_GRUPO).text(this.COD_GRUPO))



            });
            document.getElementById("ContentPlaceHolder_Contenido_d_pantallas_chosen").style.width = "auto";
            document.getElementById("ContentPlaceHolder_Contenido_d_grupos_chosen").style.width = "auto";

            $("#<%=d_grupos.ClientID%>").chosen();
            $("#<%=d_grupos.ClientID%>").trigger("chosen:updated");
            $("#<%=d_pantallas.ClientID%>").chosen();
            $("#<%=d_pantallas.ClientID%>").trigger("chosen:updated");


        }

        function LOAD_APP(result) {

            //quito los options que pudiera tener previamente el combo

            ////$("#<%=d_pantallas.ClientID%>").html("");


            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {

                $("#<%=d_pantallas.ClientID%>").append($("<option></option>").attr("value", this.cod_app).text(this.nom_app))

            });

            document.getElementById("ContentPlaceHolder_Contenido_d_grupos_chosen").style.width = "auto";
            document.getElementById("ContentPlaceHolder_Contenido_d_pantallas_chosen").style.width = "auto";
            $("#<%=d_grupos.ClientID%>").chosen();
            $("#<%=d_grupos.ClientID%>").trigger("chosen:updated");
            $("#<%=d_pantallas.ClientID%>").chosen();
            $("#<%=d_pantallas.ClientID%>").trigger("chosen:updated");


        }

        function delete_adm() {

            alert("Usuario eliminado");
        }

        function agregar_adm() {

            alert("Usuario agregado");
        }
        function repetido_agregar_adm() {

            alert("Usuario ya ingresado");
        }
        function editado() {

            alert("Usuario editado");
        }
        function no_editado() {

            alert("Problemas al editar");
        }


        function vaciar_editar() {
            $("option:selected").removeAttr("selected");
            $("option:selected").prop("selected", false)

        }

        function chk_adm()
        {
            document.getElementById("<%=chk_adm.ClientID %>").checked = true;
            document.getElementById("<%=chk_vend.ClientID %>").checked = false;
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

        .centrar {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            top: 40%;
            left: 40%;
            /*determinamos una anchura*/
            width: 400px;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -200px;
            /*determinamos una altura*/
            height: 300px;
            /*indicamos que el margen superior, es la mitad de la altura*/
            margin-top: -150px;
            padding: 5px;
            z-index: 1;
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
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_nuevo_usuario" />
            <asp:PostBackTrigger ControlID="btn_volver_usuario" />
        </Triggers>
        <ContentTemplate>
            
            <script type="text/javascript">
                Sys.Application.add_load(BindEvents);

                function BindEvents() {

                    superfiltro();
                    $("#<%=d_unidade_negocio.ClientID%>").chosen();
                    $("#<%=d_unidade_negocio.ClientID%>").trigger("chosen:updated");


                    $("#<%=d_grupos_2.ClientID%>").chosen();
                    $("#<%=d_grupos_2.ClientID%>").trigger("chosen:updated");

                    $("#<%=d_unidade_negocio.ClientID%>").change(function () {
       
                        var arr = $(this).val();
                        document.getElementById("<%=l_unidad_negocio.ClientID %>").value = arr;


                        if (arr == null) {
                            alert('null');
                            document.getElementById("<%=DIV2_3.ClientID %>").style.display = "none";
                            document.getElementById("check_enviar_2").style.display = "none";

                            $("#<%=d_grupos.ClientID%>").html("");
                            $("#<%=d_pantallas.ClientID%>").html("");

                            document.getElementById("<%=l_pantallas.ClientID %>").value = "";
                            document.getElementById("<%=l_grupos.ClientID %>").value = "";

                        } else {

                            if (arr.indexOf("1") >= 0 || arr.indexOf("2") >= 0) {

                                document.getElementById("check_enviar_2").style.display = "block";
                            }
                            else {
                                document.getElementById("check_enviar_2").style.display = "none";
                            }

                            document.getElementById("<%=DIV2_3.ClientID %>").style.display = "block";
                        }


                        var parameters = new Object();

                        parameters.u_negocio = document.getElementById("<%=l_unidad_negocio.ClientID %>").value;

                        parameters = JSON.stringify(parameters)

                        $.ajax({
                            type: "POST",
                            url: "REPORTE_USUARIOS.aspx/GRUPOS_POR_UNIDAD",
                            data: parameters,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: LOAD_GRUPOS,
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert("Error al cargar GRUPOS");
                            }
                        });


                        $.ajax({
                            type: "POST",
                            url: "REPORTE_USUARIOS.aspx/APP_POR_UNIDAD",
                            data: parameters,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: LOAD_APP,
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert("Error al cargar app");
                            }
                        });

                    });

                    $("#<%=d_grupos.ClientID%>").chosen();

                    $("#<%=d_pantallas.ClientID%>").chosen();



                    $("#<%=d_grupos.ClientID%>").change(function () {

                        // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                        //este parametro mapeara con el definido en el web service
                        var arr = $(this).val();
                        document.getElementById("<%=l_grupos.ClientID %>").value = arr;

                    });

                    $("#<%=d_pantallas.ClientID%>").change(function () {

                        // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                        //este parametro mapeara con el definido en el web service
                        var arr = $(this).val();
                        document.getElementById("<%=l_pantallas.ClientID %>").value = arr;

                    });


                    $("#<%=d_grupos_2.ClientID%>").change(function () {

                        // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
                        //este parametro mapeara con el definido en el web service
                        var arr = $(this).val();
                        document.getElementById("<%=l_unidad_negocio.ClientID %>").value = arr;

                    });

                }

            </script>
            <asp:TextBox ID="l_unidad_negocio" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
            <asp:TextBox ID="l_grupos" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
            <asp:TextBox ID="l_pantallas" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>

            <div id="fondo_modal" runat="server" class="overlay" visible="false" />



           <%-- <div class="page-title" style="margin-top: -27px">
                <div>
                    <div id="titulo" runat="server">Usuarios</div>
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
                    <li class="active" id="titulo2" runat="server"></li>
                </ul>
            </div>


            <asp:MultiView ID="USUARIOS" runat="server" ActiveViewIndex="0">
                <%-- 0 --%>
                <asp:View ID="Grilla" runat="server">

                    <div id="main-content">
                        <div id="div_report" runat="server" class="row" visible="true">
                            <div class="col-md-12">
                                <div class="box">
                                    <div class="box-title">
                                        <h3><i class="fa fa-table"></i>Usuarios</h3>
                                        <div class="box-tool">
                                            <a data-action="collapse" href="#"></a>

                                        </div>
                                    </div>
                                    <div class="box-content">
                                        <div id="div_editar_usuario" visible="false" class="centrar" runat="server">
                                            <div class="modal-dialog">
                                                <div class="modal-content" style="border: 10px solid rgba(0,0,0,.2);">
                                                    <div class="modal-header">
                                                        <h3 id="myModalLabel1">Editar </h3>
                                                        <br />
                                                        (<asp:Label runat="server" ID="usuario"></asp:Label>)
                                                    </div>
                                                    <div class="modal-body">
                                                        <div class="form-group">


                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Usuario</label>
                                                            <asp:TextBox ID="txt_cod_usuario_edit" CssClass="form-control" runat="server"></asp:TextBox>
                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Clave</label>
                                                            <asp:TextBox ID="txt_clave_edit" CssClass="form-control" runat="server"></asp:TextBox>
                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Activo</label><br />
                                                            <asp:CheckBox ID="activado" runat="server" Style="" EnableViewState="true" />
                                                            <br />
                                                            <br />
                                                            <div>

                                                                <asp:RadioButton ID="chk_adm2" Text="&nbsp;&nbsp;Admin." AutoPostBack="true" OnCheckedChanged="chk_adm2_CheckedChanged" Style="padding-right: 20px;" Checked="True"
                                                                    GroupName="tipo_usuario" runat="server" />

                                                                <asp:RadioButton ID="chk_vend2" Style="padding-right: 20px;" AutoPostBack="true" OnCheckedChanged="chk_vend2_CheckedChanged" Text="&nbsp;&nbsp;Vendedor"
                                                                    GroupName="tipo_usuario" runat="server" />

                                                                <asp:RadioButton ID="chk_otro2" Style="padding-right: 20px;" AutoPostBack="true" OnCheckedChanged="chk_otro2_CheckedChanged" Text="&nbsp;&nbsp;Otro"
                                                                    GroupName="tipo_usuario" runat="server" />

                                                            </div>
                                                            <div id="div_grupos2" runat="server" visible="true">
                                                                <label runat="server" id="grupo_edit" class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Grupo</label>

                                                                <asp:ListBox ID="d_grupos_2" SelectionMode="Multiple" data-placeholder="Seleccione" runat="server" class="form-control chosen-select"></asp:ListBox>

                                                            </div>


                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Correo</label>
                                                            <asp:TextBox ID="t_correo_editar" CssClass="form-control" runat="server"></asp:TextBox>

                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">EnviarCorreo</label><br />
                                                            <asp:CheckBox ID="chk_enviar_correo" runat="server" Style="" EnableViewState="true" />


                                                        </div>
                                                    </div>
                                                    <div class="modal-footer">
                                                        <asp:Button ID="Button1" runat="server" data-dismiss="modal" aria-hidden="true" Class="btn" OnClick="Button1_Click" Text="Cerrar" />
                                                        <asp:Button ID="btn_save" runat="server" data-dismiss="modal" aria-hidden="true" Class="btn btn-primary" OnClick="btn_save_Click" Text="Guardar" />

                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="clearfix"></div>
                                        <div class="table-responsive" style="border: 0">

                                            <div class="row">
                                                <div class="col-md-12">

                                                    <div class="box">
                                                        <div class="form-group">


                                                            <button id="btn_nuevo_usuario" visible="true" style="float: right;" class="btn btn-success icon-plus btn-circle" runat="server" onserverclick="btn_nuevo_usuario_ServerClick"><i class="fa fa-plus"></i></button>

                                                            <input type="text" id="t_filtro_memoria" style="width: 200px; margin-right: 7px; padding: 5px;" placeholder="Filtrar..." class="form-control" />
                                                            <div style="overflow-x: auto;">
                                                                <asp:GridView ID="G_usuarios" AutoGenerateColumns="false" CssClass="table table-bordered filtrar" OnRowDataBound="G_usuarios_RowDataBound" OnRowDeleted="G_usuarios_RowDeleted" OnRowCommand="G_usuarios_RowCommand" runat="server" Visible="true"
                                                                    ShowHeaderWhenEmpty="True" Font-Size="12px" DataKeyNames="cod_usuario, clave, activado, adm, grupos, enviar_correo, correo, enviar_correo2, cc, u_negocio, tipo_usuario, app, nombre_usuario">
                                                                    <HeaderStyle CssClass="test no-sort" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Eliminar" ItemStyle-Width="4px" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton runat="server" ID="b_eli" ImageUrl="img/DELETE2.png" Width="17"
                                                                                    CommandName="delete_" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClientClick='<%# confirmDelete( Eval("cod_usuario").ToString() ) %>' />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="5px" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Editar" HeaderStyle-Wrap="false" ItemStyle-Width="4px" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton runat="server" ID="b_edit" ImageUrl="img/pencil.png" Width="17"
                                                                                    CommandName="Editar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="5px" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>

                                                                        <asp:BoundField DataField="cod_usuario" HeaderText="Usuario" HeaderStyle-HorizontalAlign="Center">
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="clave" HeaderText="Clave" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>

                                                                        <asp:BoundField DataField="nombre_usuario" HeaderText="Nomb." HeaderStyle-HorizontalAlign="Center" Visible="true">
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>

                                                                        <asp:BoundField DataField="tipo_usuario" HeaderText="T.Usuario" HeaderStyle-HorizontalAlign="Center" Visible="true">
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>

                                                                        <asp:BoundField DataField="correo" HeaderText="Correo" HeaderStyle-HorizontalAlign="Center" Visible="true">
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="cc" HeaderText="CC" HeaderStyle-HorizontalAlign="Center" Visible="true">
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>

                                                                        <asp:BoundField DataField="grupos" HeaderText="Grupos" HeaderStyle-HorizontalAlign="Center" Visible="true">
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>

                                                                        <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkAccept" runat="server" Style="float: right; margin-right: 5% !important" CssClass="center" EnableViewState="true" Checked='<%#Convert.ToBoolean(Eval("activado")) %>'
                                                                                    CommandName="chk" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Enabled="false" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="45" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="CorreoDiario" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkAccept_correo" runat="server" Style="float: right; margin-right: 5% !important" CssClass="center" EnableViewState="true" Checked='<%#Convert.ToBoolean(Eval("enviar_correo")) %>'
                                                                                    CommandName="chk_q" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Enabled="false" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="45" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="CorreoAnálisis" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkAccept_correo2" runat="server" Style="float: right; margin-right: 5% !important" CssClass="center" EnableViewState="true" Checked='<%#Convert.ToBoolean(Eval("enviar_correo2")) %>'
                                                                                    CommandName="chk_q" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Enabled="false" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="45" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>


                                                                        <asp:BoundField DataField="adm" HeaderText="Adm" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>

                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        No existen datos.
                                                                    </EmptyDataTemplate>
                                                                </asp:GridView>
                                                            </div>
                                                            <asp:Label runat="server" ID="l_estado" ForeColor="Red"></asp:Label>


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

                <%-- 1 --%>
                <asp:View ID="View1" runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="box">
                                <div class="box-title">
                                    <h3 runat="server" id="h3_"><i class="fa fa-table"></i>Crear Usuarios</h3>
                                    <div class="box-tool">
                                        <a data-action="collapse" href="#"></a>
                                        <button id="btn_volver_usuario" visible="true" style="float: right; margin-top: -6px;" class="btn btn-warning icon-plus btn-circle" runat="server" onserverclick="btn_volver_usuario_ServerClick"><i class="fa fa-arrow-circle-left"></i></button>

                                    </div>
                                </div>
                                <div class="box-content">
                                    <div class="clearfix"></div>
                                    <div class="table-responsive form-horizontal" style="border: 0">

                                        <div class="row" runat="server" id="DIV111" style="margin-right: 0px; margin-left: 0px;">
                                            <div class="panel panel-primary">
                                                <div class="col-md-12">
                                                    <div class="panel-body">
                                                        <div class="form-group">
                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Usuario</label>

                                                            <div class="col-sm-3 col-lg-3 controls">
                                                                <asp:TextBox ID="txt_cod_usuario" CssClass="form-control" runat="server"></asp:TextBox>
                                                            </div>
                                                            <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Clave</label>
                                                            <div class="col-sm-3 col-lg-3 controls">
                                                                <asp:TextBox ID="txt_clave" CssClass="form-control" runat="server"></asp:TextBox>
                                                            </div>
                                                            <div class="col-sm-3 col-lg-3 controls">
                                                                <asp:RadioButton ID="chk_adm" Style="padding-right: 20px;" onclick="javascript:setTimeout('__doPostBack(\'ctl00$ContentPlaceHolder_Contenido$chk_adm\',\'\')', 0)" OnCheckedChanged="chk_adm_CheckedChanged1" Text="&nbsp;&nbsp;Adm"
                                                                    GroupName="tipo_usuario" runat="server" />

                                                                <asp:RadioButton ID="chk_vend" Style="padding-right: 20px;" onclick="javascript:setTimeout('__doPostBack(\'ctl00$ContentPlaceHolder_Contenido$chk_vend\',\'\')', 0)" OnCheckedChanged="chk_vend_CheckedChanged" Text="&nbsp;&nbsp;Vendedor"
                                                                    GroupName="tipo_usuario" runat="server" />

                                                                <asp:RadioButton ID="chk_otro" Style="padding-right: 20px;" onclick="javascript:setTimeout('__doPostBack(\'ctl00$ContentPlaceHolder_Contenido$chk_otro\',\'\')', 0)" OnCheckedChanged="chk_otro_CheckedChanged" Text="&nbsp;&nbsp;Otro"
                                                                    GroupName="tipo_usuario" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" runat="server" id="DIV222" style="margin-right: 0px; margin-left: 0px;">
                                            <div class="panel panel-primary">
                                                <div class="col-md-12">
                                                    <div class="panel-body">
                                                        <div class="form-group">
                                                            <div id="div_grupos" runat="server" visible="true">
                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">U.Negocio</label>
                                                                <div class="col-sm-3 col-lg-3">
                                                                    <div class="controls">
                                                                        <asp:ListBox ID="d_unidade_negocio" SelectionMode="Multiple" data-placeholder="Seleccione" runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row" runat="server" id="DIV2_3" style="margin-right: 0px; margin-left: 0px;">
                                            <div class="panel panel-primary">
                                                <div class="col-md-12">
                                                    <div class="panel-body">
                                                        <div class="form-group">
                                                            <div id="div3" runat="server" visible="true">
                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Grupos</label>
                                                                <div class="col-sm-3 col-lg-3">
                                                                    <div class="controls">
                                                                        <asp:ListBox ID="d_grupos" SelectionMode="Multiple" data-placeholder="Seleccione" runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                    </div>
                                                                </div>
                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">App</label>
                                                                <div class="col-sm-3 col-lg-3">
                                                                    <div class="controls">
                                                                        <asp:ListBox ID="d_pantallas" SelectionMode="Multiple" data-placeholder="Seleccione" runat="server" class="form-control chosen-select"></asp:ListBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" runat="server" id="DIV333" style="margin-right: 0px; margin-left: 0px; display: block">
                                            <div class="panel panel-primary">
                                                <div class="col-md-12">
                                                    <div class="panel-body">
                                                        <div class="form-group">
                                                            <div id="div1" runat="server" visible="true">
                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Correo</label>
                                                                <div class="col-sm-3 col-lg-3 controls">
                                                                    <asp:TextBox ID="t_correo" CssClass="form-control" runat="server"></asp:TextBox>
                                                                </div>
                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">CC</label>
                                                                <div class="col-sm-3 col-lg-3 controls">
                                                                    <asp:TextBox ID="t_cc" CssClass="form-control" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-3 col-lg-3 controls">
                                                                <asp:CheckBox ID="r_enviar_correo" Text="&nbsp;&nbsp;Enviar Correo diario." Style="padding-right: 20px;" Checked="False"
                                                                    runat="server" />


                                                            </div>
                                                            <div id="check_enviar_2" style="display: block;" class="col-sm-3 col-lg-3 controls">
                                                                <asp:CheckBox ID="r_enviar_2" Text="&nbsp;&nbsp;Enviar Correo periodo/diario." Style="padding-right: 20px;" Checked="False"
                                                                    runat="server" />


                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row" runat="server" id="DIV555" style="margin-right: 0px; margin-left: 0px; display: block">
                                            <div class="panel panel-primary">
                                                <div class="col-md-12">
                                                    <div class="panel-body">
                                                        <div class="form-group">
                                                            <div id="di" runat="server" visible="true">
                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Nombre</label>
                                                                <div class="col-sm-3 col-lg-3 controls">
                                                                    <asp:TextBox ID="t_nombre_us" CssClass="form-control" runat="server"></asp:TextBox>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row" runat="server" id="DIV444" style="margin-right: 0px; margin-left: 0px; display: block">
                                            <div class="pull-right">
                                                <div class="btn-group">

                                                    <asp:CheckBox ID="r_activo" Text="&nbsp;&nbsp;Activo" Style="padding-right: 20px;" Checked="True"
                                                        runat="server" />

                                                    <asp:ImageButton ID="b" ImageUrl="~/img/save.png" runat="server" OnClick="b_Click" />

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

        </ContentTemplate>
    </asp:UpdatePanel>
    <script src="js/tablesort.js"></script>
    <script src="js/prettify.js"></script>

    <script src='js/sorts/tablesort.date.js'></script>
    <script src='js/sorts/tablesort.dotsep.js'></script>
    <script src='js/sorts/tablesort.filesize.js'></script>
    <script src='js/sorts/tablesort.numeric.js'></script>

</asp:Content>
