<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="MANT_TRANSPORT.aspx.cs" Inherits="SoprodiApp.MANT_TRANSPORT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">

    <script>

        function chosen_upd() {

            $("#CB_BODEGA").chosen();
            $("#CB_BODEGA").trigger("chosen:updated");
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
                <a runat="server" id="titulo2"></a>
                <span class="divider"><i class="fa fa-angle-right"></i></span>
            </li>
            <li>
                <a runat="server" id="titulo3"></a>
                <span class="divider"><i class="fa fa-angle-right"></i></span>
            </li>
            <li class="active">Mantenedor de Transportista</li>
        </ul>
        <i class="fa fa-cog fa-spin fa-3x fa-fw margin-bottom" id="cargando_gif" runat="server" style="display: none; font-size: 3em;"></i>

    </div>

    <div id="main-content">
        <div class="row">
            <div class="col-md-12">
                <div class="box">
                    <div class="box-title">
                        <h3><i class="fa fa-table"></i>Mantenedor de Transportista</h3>
                    </div>
                    <div class="box-content">

                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="form-horizontal">
                                        <!-- ********************************************************************* -->
                                        <div class="form-group">
                                            <asp:TextBox runat="server" ID="T_ID_BANCO" Visible="false"></asp:TextBox>
                                            <label class="col-sm-2 control-label">
                                                <b>RUT</b>
                                            </label>
                                            <div class="col-sm-3 controls">
                                                <asp:TextBox runat="server" ID="t_rut" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                            </div>
                                            <label class="col-sm-2 control-label">
                                                <b>Nombre Transportista</b>
                                            </label>
                                            <div class="col-sm-3 controls">
                                                <asp:TextBox runat="server" ID="t_nombre" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                            </div>

                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label">
                                                <b>Direccion</b>
                                            </label>
                                            <div class="col-sm-3 controls">
                                                <asp:TextBox runat="server" ID="t_direcc" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                            </div>
                                            <label class="col-sm-2 control-label">
                                                <b>Fono</b>
                                            </label>
                                            <div class="col-sm-3 controls">
                                                <asp:TextBox runat="server" ID="t_fono" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                            </div>


                                        </div>

                                        <div class="form-group">

                                            <div style="visibility: hidden; position: absolute">
                                                <label class="col-sm-2 control-label">
                                                    <b>Carga inicial</b>
                                                </label>
                                                <div class="col-sm-3 controls">
                                                    <asp:TextBox runat="server" ID="t_inicial" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>

                                            <label class="col-sm-2 control-label">
                                                <b>Bodega relacionada</b>
                                            </label>
                                            <div class="col-sm-3 controls">
                                                <asp:DropDownList runat="server" ID="CB_BODEGA" ClientIDMode="Static" Enabled="false" CssClass="form-control chosen"></asp:DropDownList>
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
                                        <asp:GridView CssClass="table fill-head table-bordered filtrar" ID="G_Banco" runat="server" AutoGenerateColumns="true" ShowHeaderWhenEmpty="True" Width="100%" OnRowCommand="G_Banco_RowCommand" DataKeyNames="cod_trans">
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
                                                <%--                  <asp:BoundField HeaderText="id" DataField="id" Visible="false" />
                                                <asp:BoundField HeaderText="Codigo" DataField="cod_banco" />
                                                <asp:BoundField HeaderText="Nombre" DataField="nom_banco" />--%>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                No existen datos.
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

