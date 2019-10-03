<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="True" CodeBehind="GESTOR_DOCUMENTAL.aspx.cs" Inherits="SoprodiApp.GESTOR_DOCUMENTAL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
    <script type="text/javascript">

        // FUNCIONES JAVASCRIPT

        $(document).ready(function () {
            try {
                superfiltro();
                CacheItems();
            } catch (e) { }
        });

        function Mensaje(a) {
            alert(a);
        }

        function AbrirURL(a) {
            alert(a);
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

        function fuera(id_archivo) {
            var urlPdf = "/PDF.aspx?";

            var id_archivo = "Ar=" + id_archivo;
            var urlPdf_Final = urlPdf + id_archivo;

            if (navigator.appName.indexOf('Microsoft Internet Explorer') != -1) {
                window.showModelessDialog(urlPdf_Final, '_blank');
            };

            if (navigator.appName.indexOf('Netscape') != -1) {
                window.open(urlPdf_Final, '_blank');
                void 0;
            };
            return false;
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
            <li class="active">Gestor Documental</li>
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
                            <Triggers>
                                <asp:PostBackTrigger ControlID="B_Add_Archivo" />
                                <asp:PostBackTrigger ControlID="Mostrar_Subir" />
                                <asp:AsyncPostBackTrigger ControlID="G_Documentos" EventName="RowCommand" />
                            </Triggers>
                            <ContentTemplate>

                                <!-- SUBIR DOCUMENTOS -->
                                <div runat="server" id="P_SubirDocumento">
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <asp:Label runat="server">Producto: </asp:Label>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:TextBox runat="server" Width="100%" ID="T_FiltraCombo" onkeyup="FilterItems(this.value)"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:DropDownList runat="server" ID="CB_Productos"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <asp:Label ID="Label1" runat="server">Laboratorio: </asp:Label>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:DropDownList runat="server" ID="CB_Laboratorio"></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:Label ID="Label2" runat="server">Tipo Analisis: </asp:Label>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:DropDownList runat="server" ID="CB_Analisis"></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:Label ID="Label3" runat="server">Fecha Analisis: </asp:Label>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:TextBox ID="T_Fecha_analisis" runat="server" Width="100%"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender runat="server" ID="calendar" TargetControlID="T_Fecha_analisis"></ajaxToolkit:CalendarExtender>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row" style="overflow-x: auto">
                                        <div class="col-sm-2">
                                            <asp:Button ID="B_Add_Archivo" class="button btn btn-foursquare btn-primary" runat="server" Text="Subir" OnClick="B_Add_Archivo_Click1"></asp:Button>
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:FileUpload ID="FileUpload_Documento" runat="server" AllowMultiple="True" ClientIDMode="Static" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div style="overflow-x: auto">
                                                <p id="D_status" runat="server"></p>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <br />
                                    <br />
                                    <div class="btn-group">
                                        <asp:Button class="btn show-tooltip" runat="server" ID="Mostrar_Lista" Text="Cerrar" OnClick="Mostrar_Lista_Click" />
                                    </div>
                                </div>

                                <!-- LISTADO DOCUMENTOS -->
                                <div runat="server" id="P_ListaDocumentos">

                                    <div class="row">
                                        <div class="col-sm-3">
                                            <asp:Button class="btn show-tooltip btn-primary" runat="server" ID="Mostrar_Subir" Text="Subir Documentos" OnClick="Mostrar_Subir_Click" />
                                            <br />
                                            <br />
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row form-horizontal">
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label for="textfield1" class="col-sm-3">Filtrar : </label>
                                                <div class="col-sm-9 controls">
                                                    <input type="text" id="t_filtro_memoria" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div style="overflow-x: auto; width: 100%">
                                                <asp:GridView CssClass="table fill-head table-bordered filtrar" ID="G_Documentos" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" Width="100%" OnRowCommand="G_Documentos_RowCommand" OnRowDataBound="G_Documentos_RowDataBound" DataKeyNames="estado, id">
                                                    <HeaderStyle CssClass="test" />
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Id." DataField="id" Visible="false" />
                                                        <asp:BoundField HeaderText="Fecha" DataField="fecha" />
                                                        <asp:BoundField HeaderText="Documento" DataField="nombre" />
                                                        <asp:BoundField HeaderText="Producto" DataField="codigo_producto" />
                                                        <asp:BoundField HeaderText="Usuario" DataField="usuario" />
                                                        <asp:BoundField HeaderText="Estado" DataField="estado" />
                                                        <asp:BoundField HeaderText="Laboratorio" DataField="laboratorio" />
                                                        <asp:BoundField HeaderText="Tipo Analisis" DataField="TIPO_ANALISIS" />
                                                        <asp:BoundField HeaderText="Fecha Analisis" DataField="FECHA_ANALISIS" />
                                                        <asp:TemplateField HeaderText="Ver" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="img/search.png" Width="25"
                                                                    CommandName="Visualizar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Descargar" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="img/download.png" Width="25"
                                                                    CommandName="Descargar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClientClick="RegistrarTriggers(this)" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Aprobar" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="img/accept.png" Width="25"
                                                                    CommandName="Aprobar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Desactivar" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="img/block.png" Width="25"
                                                                    CommandName="Eliminar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Eliminar" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="img/delete_page.png" Width="25"
                                                                    CommandName="Borrar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
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
