<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="MANT_LAB.aspx.cs" Inherits="SoprodiApp.MANT_LAB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
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
            <i class="fa fa-file-o fa-3x"></i><a class="h1" href="MENU_GESTOR.aspx">Gestor</a>
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
                <a href="Menu_Gestor.aspx">Gestor</a>
                <span class="divider"><i class="fa fa-angle-right"></i></span>
            </li>
            <li class="active">Mantenedor Laboratorios</li>
        </ul>
    </div>

    <div id="main-content">
        <div class="row">
            <div class="col-md-12">
                <div class="box">
                    <div class="box-title">
                        <h3><i class="fa fa-table"></i>Mantenedor Laboratorios</h3>
                    </div>
                    <div class="box-content">

                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UP_SubirArchivo">
                            <ContentTemplate>                                
                                <div class="row"> 
                                    <div class="form-horizontal">
                                        <!-- ********************************************************************* -->
                                        <div class="form-group">
                                            <asp:TextBox runat="server" ID="T_ID_LAB" Visible="false"></asp:TextBox>
                                            <label class="col-sm-2 control-label">
                                                <b>Codigo</b>
                                            </label>
                                            <div class="col-sm-3 controls">
                                                <asp:TextBox runat="server" ID="T_Cod_Lab" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                            </div>
                                            <label class="col-sm-2 control-label">
                                                <b>Nombre</b>
                                            </label>
                                            <div class="col-sm-3 controls">
                                                <asp:TextBox runat="server" ID="T_Nom_lab" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1 col-sm-offset-1 controls">
                                                <button id="btn_nuevo_lab" class="btn btn-success icon-plus btn-circle" runat="server" onserverclick="btn_nuevo_lab_ServerClick"><i class="fa fa-plus"></i></button>
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
                                        <asp:GridView CssClass="table fill-head table-bordered filtrar" ID="G_Lab" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" Width="100%" OnRowCommand="G_Lab_RowCommand" DataKeyNames="id, cod_lab, nom_lab">
                                            <HeaderStyle CssClass="test" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Editar">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="B_Editar_lab" runat="server" ImageUrl="img/pencil.png" Width="25"
                                                            CommandName="Editar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Borrar">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="B_Borrar_Lab" runat="server" ImageUrl="img/delete.png" Width="25"
                                                            CommandName="Eliminar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="id" DataField="id" Visible="false" />
                                                <asp:BoundField HeaderText="Codigo" DataField="cod_lab" />
                                                <asp:BoundField HeaderText="Nombre" DataField="nom_lab" />
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
