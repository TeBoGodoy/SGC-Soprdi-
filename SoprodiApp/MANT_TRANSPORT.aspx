﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="MANT_TRANSPORT.aspx.cs" Inherits="SoprodiApp.MANT_TRANSPORT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
    
    <%--INCLUIR JQUERY !!--%>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>

    <script>

        var g_init_datatable;
            //  **********************************************************************************************************
     
        $(document).ready(function () {
            creagrilla();
        });

        function chosen_upd() {

            $("#CB_BODEGA").chosen();
            $("#CB_BODEGA").trigger("chosen:updated");
        }
        function CARGANDO() {

            var elem3 = document.getElementById("<%=cargando_gif.ClientID%>");
            elem3.style.display = "block";

        }

        function super_ff() {

            $(".filtrar tr:has(td)").each(function () {
                var t = $(this).text().toLowerCase();
                $("<td class='indexColumn'></td>")
                    .hide().text(t).appendTo(this);
            });
            //Agregar el comportamiento al texto (se selecciona por el ID) 
            $("#btn_filtro_nuevo").click(function () {
                var s = $('#t_filtro_memoria').val().toLowerCase().split(" ");
                $(".filtrar tr:hidden").show();
                $.each(s, function () {
                    $(".filtrar tr:visible .indexColumn:not(:contains('"
                        + this + "'))").parent().hide();
                });
            });

            $("#t_filtro_memoria").keyup(function (event) {
                if (event.keyCode == 13) {
                    var s = $('#t_filtro_memoria').val().toLowerCase().split(" ");
                    $(".filtrar tr:hidden").show();
                    $.each(s, function () {
                        $(".filtrar tr:visible .indexColumn:not(:contains('"
                            + this + "'))").parent().hide();
                    });
                }
            });
        }


         function creagrilla() {


            (function ($) {
                // You pass-in jQuery and then alias it with the $-sign
                // So your internal code doesn't change

                try {

                    //alert('ac');
                   $("#G_Banco").DataTable({
                        "lengthChange": false,
                        "searching": true,
                        "destroy": true,
                        "stateSave": true,
                        "pageLength": -1,
                        "paging": false,
                        "order": [[2, "asc"]],
                        "language": {
                            "decimal": ",",
                            "thousands": "."
                        }
                    });

                    
                } catch (e) {

                     //alert('catch' + e);

                }


            })(jQuery);
        }



    </script>

   <style>
        .test {
            background-color: #428bca !important;
            color: white !important;
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

        .Grheader {
            overflow: hidden;
            width: 26px;
            position: relative;
            top: 0px;
            z-index: 10;
            vertical-align: top;
            margin-right: 16px;
        }

        .GrCuerpo {
            width: 80%;
            position: relative;
            left: 16px;
            z-index: 1;
            overflow: auto;
            height: 100%;
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


        .estado10 {
            border-left-color: #fa5a35 !important;
        }

        .estado20 {
            border-left-color: #4e53ff !important;
        }

        .estado30 {
            border-left-color: #10ff23 !important;
        }

        .estado40 {
            border-left-color: #ff10c0 !important;
        }

        .estado10S {
            border-left-color: #c2c506 !important;
        }

        .estado10P {
            border-left-color: #06c5c5 !important;
        }

        .estadoGFX {
            border-left-color: #30441f !important;
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
                                                <button id="btn_nuevo_banco" class="btn btn-success icon-plus btn-circle" runat="server" onclientclick="creagrilla();" onserverclick="btn_nuevo_banco_ServerClick"><i class="fa fa-plus"></i></button>
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
                                        <asp:GridView ID="G_Banco" ClientIDMode="Static" CssClass="table table-bordered table-advance filtrar"
                                            runat="server" AutoGenerateColumns="true" ShowHeaderWhenEmpty="True" Width="100%" OnRowCommand="G_Banco_RowCommand" DataKeyNames="cod_trans">
                                            <HeaderStyle CssClass="test no-sort" />
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

