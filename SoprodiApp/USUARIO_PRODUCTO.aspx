<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="USUARIO_PRODUCTO.aspx.cs" Inherits="SoprodiApp.USUARIO_PRODUCTO" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
    <script type="text/javascript">
        // FUNCIONES JAVASCRIPT

        $(document).ready(function () {
            superfiltro();
            CacheItems();
        });

        function Mensaje(a) {
            alert(a);
        }

        function AbrirURL(a) {
            window.open(a, "_blank");
        }

        function superfiltro() {
            // ****************  FUNCION PRAH DE CAPISITO PARA FILTRAR GRIDVIEWS EN MEMORIA (INICIO)
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
            // ****************  FUNCION PRAH DE CAPISITO PARA FILTRAR GRIDVIEWS EN MEMORIA (FINAL)
        }

        var ddlText, ddlValue, ddl;
        function CacheItems() {
            ddlText = new Array();
            ddlValue = new Array();
            ddl = document.getElementById("<%=CB_Productos.ClientID %>");
            for (var i = 0; i < ddl.options.length; i++) {
                ddlText[ddlText.length] = ddl.options[i].text;
                ddlValue[ddlValue.length] = ddl.options[i].value;
            }
        }

        function FilterItems(value) {
            ddl.options.length = 0;
            for (var i = 0; i < ddlText.length; i++) {
                if (ddlText[i].toLowerCase().indexOf(value) != -1) {
                    AddItem(ddlText[i], ddlValue[i]);
                }
            }
            if (ddl.options.length == 0) {
                AddItem("No items found.", "");
            }
        }

        function AddItem(text, value) {
            var opt = document.createElement("option");
            opt.text = text;
            opt.value = value;
            ddl.options.add(opt);
        }

    </script>
      <style>
         .test {
            background-color: #428bca !important;
            color: white !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>
  <%--  <div class="page-title" style="margin-top: -27px">
                <div>
                    <i class="fa fa-file-o fa-3x"></i> <a class="h1" href="MENU_GESTOR.aspx">Gestor</a>
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
                    <li class="active">Usuario - producto</li>
                </ul>
            </div>

    <div id="main-content">
        <div class="row">
            <div class="col-md-12">
                <div class="box">
                    <div class="box-title">
                        <h3><i class="fa fa-table"></i>Documentos</h3>
                        <div class="box-tool">
                         
                        </div>
                    </div>
                    <div class="box-content">

                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UP_SubirArchivo">
                            <ContentTemplate>

                                <!-- LISTADO DOCUMENTOS -->
                                <div runat="server" id="P_ListaDocumentos">
                                    <div class="row form-horizontal">
                                        <div class="col-sm-12">
                                            <div class="form-group">
                                                <label for="textfield1" class="col-sm-1">Usuario : </label>
                                                <div class="col-sm-2 controls">
                                                    <asp:DropDownList runat="server" ID="CB_USUARIO" OnTextChanged="CB_USUARIO_TextChanged" AutoPostBack="true"></asp:DropDownList>
                                                </div>
                                                <div class="col-sm-1 controls">
                                                    <asp:Button ID="B_CargaUsuario" CssClass="btn btn-primary" runat="server" Text="Cargar Usuario" OnClick="B_CargaUsuario_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row form-horizontal" id="DIV_PRODUCTOS" runat="server" visible="false">
                                        <div class="col-sm-12">
                                            <div class="form-group">
                                                <div class="col-sm-1">
                                                    <asp:Label ID="Label1" runat="server">Producto: </asp:Label>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:TextBox runat="server" Width="100%" ID="T_FiltraCombo" onkeyup="FilterItems(this.value)"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-5">
                                                    <asp:DropDownList runat="server" ID="CB_Productos"></asp:DropDownList>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Button CssClass="btn btn-primary" runat="server" ID="B_AgregaProducto" OnClick="B_AgregaProducto_Click" Text="Agregar" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" id="DIV_GRILLA" runat="server">
                                        <div class="col-sm-12">
                                            <div style="overflow-x: auto; width: 100%">
                                                <p id="L_USUARIO" runat="server"></p>
                                                <p id="L_ERROR" runat="server"></p>
                                                <asp:GridView CssClass="table fill-head table-bordered filtrar" ID="G_Documentos" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" Width="100%" DataKeyNames="InvtID" OnRowCommand="G_Documentos_RowCommand">
                                                      <HeaderStyle CssClass="test" />
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Codigo" DataField="InvtID" />
                                                        <asp:BoundField HeaderText="Nombre" DataField="descr" />
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="B_BorrarProducto" runat="server" ImageUrl="img/block.png" Width="25"
                                                                    CommandName="Eliminar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
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
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
