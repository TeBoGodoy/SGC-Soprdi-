<%@ Page ValidateRequest="false" Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Base.Master" UICulture="es-ES" Culture="es-ES" AutoEventWireup="True" CodeBehind="MANT_CLIENTES.aspx.cs" Inherits="SoprodiApp.MANT_CLIENTES" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
    <script>

        <%-- function LoadVendedores(result) {

            //quito los options que pudiera tener previamente el combo

            $("#<%=vendedor_NEW_.ClientID%>").html("");

            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {

                $("#<%=vendedor_NEW_.ClientID%>").append($("<option></option>").attr("value", this.cod_vendedor).text(this.nom_vendedor))

            });


            $("#<%=vendedor_NEW_.ClientID%>").chosen();
            $("#<%=vendedor_NEW_.ClientID%>").trigger("chosen:updated");


        }--%>

        function chosen_upd() {

            $("#CB_CLIENTE").chosen();
            $("#CB_CLIENTE").trigger("chosen:updated");
        }
        function CARGANDO() {

            var elem3 = document.getElementById("<%=cargando_gif.ClientID%>");
            elem3.style.display = "block";

        }



    </script>
    <style>
        .test {
            background-color: #428bca !important;
            color: white !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>
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
                <a href="MENU_Finanzas.aspx">Finanzas</a>
                <span class="divider"><i class="fa fa-angle-right"></i></span>
            </li>
            <li class="active">Mantenedor de Clientes</li>
        </ul>
    </div>

    <div id="main-content">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-12">
                        <div class="box">
                            <div class="box-title">
                                <h3><i class="fa fa-table"></i>Mantenedor de Clientes</h3>
                            </div>
                            <div class="box-content">
                                <div class="clearfix"></div>
                                <div class="table-responsive" style="border: 0">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box">
                                                <div class="form-group">
                                                    <%--  <asp:TextBox runat="server" ID="T_PK_CLIENTE" Visible="false"></asp:TextBox>
                                            <label class="col-sm-2 control-label">
                                                <b>Rut</b>
                                            </label>
                                            <div class="col-sm-4 controls">
                                                <asp:TextBox runat="server" ID="txt_Rut_cliente" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                            </div>
                                            <label class="col-sm-2 control-label">
                                                <b>Nombre</b>
                                            </label>
                                            <div class="col-sm-4 controls">
                                                <asp:TextBox runat="server" ID="txt_Nom_Cliente" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                            </div>
                                            <label class="col-sm-2 control-label">
                                                <b>Vendedor</b>
                                            </label>
                                            <div class="col-sm-4 controls">
                                                <asp:ListBox ID="vendedor_NEW_" SelectionMode="Multiple" Width="120%" runat="server"  visible="false" class="form-control chosen-select"></asp:ListBox>

                                                <asp:DropDownList runat="server" Width="120%" ID="vendedor_NNEW"></asp:DropDownList>

                                            </div>
                                            <label class="col-sm-2 control-label">
                                                <b>Condicion de pago</b>
                                            </label>
                                            <div class="col-sm-4 controls">
                                                <asp:TextBox runat="server" ID="txt_Cond_Pago" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                            </div>
                                            <label class="col-sm-2 control-label">
                                                <b>Contacto</b>
                                            </label>
                                            <div class="col-sm-4 controls">
                                                <asp:TextBox runat="server" ID="txt_contacto" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                            </div>
                                            <label class="col-sm-2 control-label">
                                                <b>Telefono</b>
                                            </label>
                                            <div class="col-sm-4 controls">
                                                <asp:TextBox runat="server" ID="txt_telefono" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                            </div>
                                            <label class="col-sm-2 control-label">
                                                <b>Email</b>
                                            </label>
                                            <div class="col-sm-4 controls">
                                                <asp:TextBox runat="server" ID="txt_email" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                            </div>
                                            <label class="col-sm-2 control-label">
                                                <b>Cargo</b>
                                            </label>
                                            <div class="col-sm-4 controls">
                                                <asp:TextBox runat="server" ID="txt_cargo" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                            </div>
                                            <label class="col-sm-2 control-label">
                                                <b>Observaciones</b>
                                            </label>
                                            <div class="col-sm-4 controls">
                                                <textarea runat="server" id="TextArea_Obs"></textarea>
                                            </div>
                                            <div class="col-sm-1 col-sm-offset-1 controls">
                                                <button id="btn_nuevo_cliente" class="btn btn-success icon-plus btn-circle" runat="server" onserverclick="btn_nuevo_cliente_ServerClick"><i class="fa fa-plus"></i></button>
                                            </div>
                                        </div>--%>
                                                    <div class="col-sm-9 col-lg-9">
                                                        <div class="controls">
                                                            <asp:DropDownList runat="server" ID="CB_CLIENTE" ClientIDMode="Static" CssClass="form-control chosen"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="form-group" style="float: right;">
                                                        <div class="col-sm-2 col-sm-2 controls">
                                                            <div class="input-group">
                                                                <asp:Button runat="server" ID="B_carga_contactos" CssClass="btn btn-primary" OnClientClick="CARGANDO();" OnClick="B_carga_contactos_Click1" Text="CargarContactos" Visible="true" />
                                                            </div>
                                                        </div>
                                                        <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_gif" runat="server" style="display: none; font-size: 3em;"></i>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="row" runat="server" id="div_contactos" visible="true">
                                    <hr />
                                    <div class="form-group" style="float: right;">
                                        <div class="col-sm-2 col-sm-2 controls">
                                            <div class="input-group">
                                                <asp:Button runat="server" ID="btn_nuevo" CssClass="btn btn-success" OnClick="btn_nuevo_Click" Text="NuevoContacto" Visible="true" />
                                            </div>
                                        </div>
                                    </div>



                                    <div class="col-md-12" runat="server" id="div_crear_contacto" visible="false">
                                        <div class="box box-green">
                                            <div class="box-title">
                                                <h3 runat="server" id="h3_titulo"><i class="fa fa-plus"></i></h3>
                                                <div class="box-tool">
                                                </div>
                                            </div>
                                            <div class="box-content" style="background: rgba(27, 27, 27, 0.06) none repeat scroll 0% 0%;">
                                                <p></p>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="box">
                                                            <div class="row">
                                                               <%-- <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Rut</label>
                                                                <div class="col-sm-3 col-lg-3">
                                                                    <div class="controls">
                                                                        <asp:TextBox ID="t_rut_cont" runat="server" class="form-control" Style="width: 100%"></asp:TextBox>

                                                                    </div>
                                                                </div>--%>
                                                                 <asp:TextBox ID="t_rut_cont" runat="server" class="form-control" Style="width: 100%" Visible="false"></asp:TextBox>
                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">
                                                                    Nombre
                                                                </label>
                                                                <div class="col-sm-3 col-lg-3">
                                                                    <div class="controls">
                                                                        <asp:TextBox ID="t_nom_cont" runat="server" class="form-control" Style="width: 100%"></asp:TextBox>

                                                                    </div>
                                                                </div>

                                                            </div>
                                                            <div class="row">&nbsp;</div>
                                                            <div class="row">
                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Correo</label>
                                                                <div class="col-sm-3 col-lg-3">
                                                                    <div class="controls">
                                                                        <asp:TextBox ID="t_corr_cont" runat="server" class="form-control" Style="width: 100%"></asp:TextBox>

                                                                    </div>
                                                                </div>

                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Número</label>
                                                                <div class="col-sm-3 col-lg-3">
                                                                    <div class="controls">
                                                                        <asp:TextBox ID="t_num_cont" runat="server" class="form-control" Style="width: 100%"></asp:TextBox>

                                                                    </div>
                                                                </div>

                                                                <label class="col-sm-1 col-lg-1 control-label" style="top: 5px;">Descripción</label>
                                                                <div class="col-sm-3 col-lg-3">
                                                                    <div class="controls">
                                                                        <textarea id="t_descrip" runat="server" class="form-control" style="width: 100%"></textarea>

                                                                    </div>
                                                                </div>

                                                            </div>

                                                            <div style="float: right">
                                                                <div class="input-group">
                                                                    <asp:Button runat="server" ID="btn_guarda_cont" CssClass="btn btn-success" OnClick="btn_guarda_cont_Click" Text="Guardar" Visible="true" />
                                                                    <asp:Button runat="server" ID="btn_cierra_cont" CssClass="btn btn-primary" OnClick="btn_cierra_cont_Click" Text="Cancelar" Visible="true" />

                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="col-sm-12">
                                        <asp:GridView CssClass="table fill-head table-bordered filtrar" ID="G_CONTACTOS" runat="server" AutoGenerateColumns="true" Width="100%"
                                            OnRowCommand="G_CONTACTOS_RowCommand" OnRowDataBound="G_CONTACTOS_RowDataBound">

                                            <HeaderStyle CssClass="test" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Editar">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="B_Editar_Clientes" runat="server" ImageUrl="img/pencil.png" Width="20"
                                                            CommandName="Editar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Borrar">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="B_Borrar_Contacto" runat="server" ImageUrl="img/delete.png" Width="20"
                                                            CommandName="Eliminar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClientClick='<%# confirmDelete( Eval("nombre").ToString() ) %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%--<asp:BoundField HeaderText="Nombre" DataField="nom_banco" />--%>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>

