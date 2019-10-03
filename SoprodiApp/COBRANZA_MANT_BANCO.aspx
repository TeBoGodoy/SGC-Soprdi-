<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="COBRANZA_MANT_BANCO.aspx.cs" Inherits="SoprodiApp.COBRANZA_MANT_BANCO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">



    <style>
        .test {
            background-color: #428bca !important;
            color: white !important;
        }

        .test2 {
            background-color: #e1650e !important;
            color: white !important;
        }

        input[type=radio] {
            border: 0px;
            width: 10%;
            height: 2em;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">

    <script>



</script>


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
            <li class="active">Mantenedor de Bancos</li>
        </ul>
    </div>

    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="main-content">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box">
                            <div class="box-title">
                                <h3 id="titulo_panel_finanza" runat="server"><i class="fa fa-list"></i>Mantenedor de Bancos -- Cliente</h3>
                            </div>
                            <div class="box-content">

                                <div class="row">
                                    <div class="form-horizontal">
                                        <!-- ********************************************************************* -->
                                        <div class="form-group">

                                            <div class="row">
                                                <div id="CHECK_BANCOS">
                                                    <asp:RadioButton ID="rd_cliente" Text="CLIENTE" Style="padding-right: 20px;" Checked="True"
                                                        GroupName="tipo_banco" runat="server" AutoPostBack="true" OnCheckedChanged="rd_cliente_CheckedChanged" />
                                                    <asp:RadioButton ID="rd_soprodi" Style="padding-right: 20px;" Text="SOPRODI"
                                                        GroupName="tipo_banco" runat="server" AutoPostBack="true" OnCheckedChanged="rd_soprodi_CheckedChanged" />
                                                </div>
                                            </div>

                                            <hr />
                                            <br />

                                            <div class="row">
                                                <asp:TextBox runat="server" ID="T_ID_BANCO" Visible="false"></asp:TextBox>
                                                <label class="col-sm-2 control-label">
                                                    <b>Codigo</b>
                                                </label>
                                                <div class="col-sm-2 controls">
                                                    <asp:TextBox runat="server" ID="T_Cod_banco" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                </div>
                                                <label class="col-sm-2 control-label">
                                                    <b>Nombre</b>
                                                </label>
                                                <div class="col-sm-3 controls">
                                                    <asp:TextBox runat="server" ID="T_Nom_banco" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                            <br />

                                            <div class="row" id="div_cuenta_acct" runat="server" visible="false">
                                                <label class="col-sm-2 control-label">
                                                    <b>Cuenta</b>
                                                </label>
                                                <div class="col-sm-2 controls">
                                                    <asp:TextBox runat="server" ID="T_cuenta" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                </div>

                                                <label class="col-sm-2 control-label">
                                                    <b>Tipo Moneda</b>
                                                </label>
                                                <div class="col-sm-2 controls">
                                                    <asp:TextBox runat="server" ID="T_moneda" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                                    <span class="help-inline">USD - PESO</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-2 col-sm-offset-2 controls">
                                                <div class="input-group">

                                                    <button id="btn_nuevo_banco" class="btn btn-success icon-plus btn-circle" runat="server" onserverclick="btn_nuevo_banco_ServerClick"><i class="fa fa-plus"></i></button>
                                                    <asp:Button runat="server" ID="B_Guardar" CssClass="btn btn-primary" Text="Crear" OnClick="B_Guardar_Click" Visible="false" />

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="div_bancos_clientes" runat="server">
                                    <div class="col-sm-12">
                                        <asp:GridView CssClass="table fill-head table-bordered filtrar" ID="G_Banco" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" Width="100%" OnRowCommand="G_Banco_RowCommand" DataKeyNames="id, cod_banco, nom_banco">
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
                                                <asp:BoundField HeaderText="Codigo" DataField="cod_banco" />
                                                <asp:BoundField HeaderText="Nombre" DataField="nom_banco" />

                                            </Columns>
                                            <EmptyDataTemplate>
                                                No existen datos.
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                </div>


                                <div class="row" id="div_bancos_soprodi" visible="false" runat="server">
                                    <div class="col-sm-12">
                                        <asp:GridView CssClass="table fill-head table-bordered filtrar" ID="G_Banco_soprodi" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" Width="100%" OnRowCommand="G_Banco_soprodi_RowCommand"
                                            DataKeyNames="id, cod_banco, nom_banco, ACCT, TIPO_MONEDA">
                                            <HeaderStyle CssClass="test2" />
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
                                                <asp:BoundField HeaderText="Codigo" DataField="cod_banco" />
                                                <asp:BoundField HeaderText="Nombre" DataField="nom_banco" />

                                                <asp:BoundField HeaderText="Cuenta(Acct)" DataField="acct" />
                                                <asp:BoundField HeaderText="Moneda" DataField="tipo_moneda" />

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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

