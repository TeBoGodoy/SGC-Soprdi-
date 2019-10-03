<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="VISOR_DOCUMENTAL.aspx.cs" Inherits="SoprodiApp.VISOR_DOCUMENTAL" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
    <script type="text/javascript">
        // FUNCIONES JAVASCRIPT

        $(document).ready(function () {
            superfiltro();
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
 <%--   <div class="page-title" style="margin-top: -27px">
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
            <li class="active">Visor Documental</li>
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
                                <asp:AsyncPostBackTrigger ControlID="G_Documentos" EventName="RowCommand" />
                            </Triggers>
                            <ContentTemplate>

                                <!-- LISTADO DOCUMENTOS -->
                                <div runat="server" id="P_ListaDocumentos">
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
                                                <asp:GridView CssClass="table fill-head table-bordered filtrar" ID="G_Documentos" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" Width="100%" OnRowCommand="G_Documentos_RowCommand" DataKeyNames="id">
                                                    <HeaderStyle CssClass="test" />
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Id." DataField="id" Visible="false" />
                                                        <asp:BoundField HeaderText="Fecha" DataField="fecha" />
                                                        <asp:BoundField HeaderText="Documento" DataField="nombre" />
                                                        <asp:BoundField HeaderText="Producto" DataField="codigo_producto" />
                                                        <asp:BoundField HeaderText="Aprobado por" DataField="usuario" />
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
