<%@ Page Title="" ViewStateMode="Enabled" Language="C#" MasterPageFile="~/Base.Master" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" AutoEventWireup="True" UICulture="es-ES" Culture="es-ES" CodeBehind="ABARROTES_INSTRUIR.aspx.cs" Inherits="SoprodiApp.ABARROTES_INSTRUIR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ OutputCache Location="None" NoStore="true" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="900000" EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="breadcrumbs">
                <ul class="breadcrumb" style="height: 35px !important;">
                    <li>
                        <i class="fa fa-home"></i>
                        <a href="menu.aspx">Menú</a>
                        <span class="divider"><i class="fa fa-angle-right"></i></span>
                    </li>
                    <li class="active">SP</li>
                </ul>
            </div>
            <div class="row" id="DIV_FILTROS" style="margin-top: -15px;">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-content">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-4 col-lg-4 controls">
                                        <small><b>DESDE</b></small>
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txt_desde" CssClass="form-control" runat="server"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender9" runat="server" TargetControlID="txt_desde" Format="dd/MM/yyyy" />
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-lg-4 controls">
                                        <small><b>HASTA</b></small>
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txt_hasta" CssClass="form-control" runat="server"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender10" runat="server" TargetControlID="txt_hasta" Format="dd/MM/yyyy" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 col-lg-4">
                                        <div class="controls">
                                            <small><b>CLIENTE</b></small>
                                            <asp:ListBox ID="cb_cliente" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-lg-4">
                                        <div class="controls">
                                            <small><b>VENDEDOR</b></small>
                                            <asp:ListBox ID="d_vendedor" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-lg-4">
                                        <div class="controls">
                                            <small><b>ESTADO</b></small>
                                            <asp:ListBox ID="d_bodega_2" SelectionMode="Multiple" data-placeholder=" -- Todos -- " runat="server" class="form-control chosen-select"></asp:ListBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-4 controls">
                                        <small><b>Nº SP (SEPARE POR COMAS)</b></small>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txt_sp"></asp:TextBox>
                                    </div>
                                    <div class="col-md-8 text-right">
                                        <br />
                                        <asp:Button ID="Button1" runat="server" OnClientClick="CARGANDO();" Style="color: white;" Class="btn btn-primary" OnClick="Button1_Click" Text="Filtrar" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div runat="server" class="row" id="DIV_LISTADO" visible="false">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-title">
                            <h3><i class="fa fa-table"></i>LISTADO DE SP'S</h3>
                        </div>
                        <div class="box-content">
                            <div class="input-group">
                                <input type="text" id="t_filtro_memoria" placeholder="Filtrar..." class="form-control" style="width: 300px">
                                <button class="btn-sm btn btn-primary" type="button" id="btn_filtro_nuevo"><i class="fa fa-search"></i></button>
                            </div>
                            <br />
                            <div class="table-responsive" style="border: 0">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box">
                                            <div class="form-group" style="overflow: auto">
                                                <asp:GridView ID="G_INFORME_TOTAL_VENDEDOR" ClientIDMode="Static" CssClass="table table-bordered table-advance filtrar" runat="server"
                                                    ShowHeaderWhenEmpty="True"
                                                    DataKeyNames="CodDocumento,FechaEmision,CodVendedor,NotaLibre,CodBodega,FechaDespacho,CodMoneda,MontoNeto,DescEstadoDocumento,Facturas,GxEstadoSync,GxActualizado,GxEnviadoERP,NombreVendedor,NombreCliente,DescBodega,FechaCreacion,ValorTipoCambio,LimiteSeguro,TipoCredito,CreditoDisponible,CreditoAutorizado,EmailVendedor"
                                                    Font-Size="12px">
                                                    <HeaderStyle CssClass="test no-sort" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkAccept" runat="server" EnableViewState="true" />
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
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- MODALES -->
    <a href="#modal_instruir" id="b_modal_instruir" role="button" class="btn" style="visibility: hidden; position: absolute;" data-toggle="modal"></a>
    <div id="modal_instruir" class="modal fade">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel runat="server" ID="UP_MODAL_INSTRUIR" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h3>Instruir SP'S</h3>
                        </div>
                        <div class="modal-body" style="overflow-x: auto;">
                        </div>
                        <div class="modal-footer">
                            <button class="btn" data-dismiss="modal" aria-hidden="true">Cerrar</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
