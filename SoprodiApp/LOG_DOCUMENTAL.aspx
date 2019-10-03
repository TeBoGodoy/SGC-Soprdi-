<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="LOG_DOCUMENTAL.aspx.cs" Inherits="SoprodiApp.LOG_DOCUMENTAL" %>

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
                    <li class="active">Log Documental</li>
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
                                                <asp:GridView CssClass="table fill-head table-bordered filtrar" ID="G_Documentos" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" Width="100%">
                                                    <HeaderStyle CssClass="test" />
                                                      <Columns>
                                                        <asp:BoundField HeaderText="Documento" DataField="documento" />
                                                        <asp:BoundField HeaderText="Producto" DataField="codigo_producto" />
                                                        <asp:BoundField HeaderText="Fecha" DataField="fecha" />
                                                        <asp:BoundField HeaderText="Usuario" DataField="usuario" />
                                                        <asp:BoundField HeaderText="Estado" DataField="estado" />
                                                        <asp:BoundField HeaderText="Observacion" DataField="observacion" />                                                        
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
