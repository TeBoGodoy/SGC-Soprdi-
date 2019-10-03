<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="True" UICulture="es-ES" Culture="es-ES" CodeBehind="REPORTE_CAMBIO_PASS.aspx.cs" Inherits="SoprodiApp.REPORTE_CAMBIO_PASS" %>

<%@ OutputCache Location="None" NoStore="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Master" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Contenido" runat="server">

    <script type="text/javascript">
        function error_pass() {
            bootbox.alert("Error en su actual contraseña!");
        }
    </script>
    <script type="text/javascript">
        function guardada() {
            bootbox.alert("Contraseña guardada con exito!");
        }
    </script>

    <style>
    </style>
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>

            <div id="main-content">
                <!-- BEGIN Main Content -->
                <div class="row">
                    <div class="col-md-12">
                        <div class="box">
                            <div class="box-title">
                                <h3><i class="fa fa-table"></i>Cambiar Contraseña</h3>
                                <div class="box-tool">
                                    <a data-action="collapse" href="#"></a>

                                </div>
                            </div>
                            <div class="box-content">
                                <div class="clearfix"></div>
                                <div class="table-responsive" style="border: 0">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box">
                                                <div class="form-group">
                                                    <label for="textfield1" class="col-xs-3 col-lg-3 control-label">Ingrese actual Contraseña</label>
                                                    <div class="col-sm-4 col-lg-4 controls">
                                                        <input type="password" name="textfield1" runat="server" id="pass_antigua" placeholder="" class="form-control" />
                                                    </div>

                                                    <div class="col-sm-4 col-lg-4 controls">
                                                        <asp:Label runat="server" ID="mensaje_pass" ForeColor="Red" Visible="true"></asp:Label>
                                                    </div>
                                                </div>
                                                <br />
                                                <br />
                                                <div class="form-group">
                                                    <label for="password1" class="col-xs-3 col-lg-3 control-label">Ingrese su nueva Contraseña</label>
                                                    <div class="col-sm-4 col-lg-4 controls">
                                                        <input type="password" name="password1" runat="server" id="pass_nueva" placeholder="" class="form-control" />
                                                    </div>
                                                </div>

                                                <!-- END Left Side -->
                                            </div>

                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="box">
                                                <asp:Button runat="server" ID="cerrar_pass" class="btn btn" Style="float: left" Text="Cerrar" OnClientClick="javascript:history.back()" />
                                                <asp:Button runat="server" ID="guarda_pass" class="btn btn-primary" Style="float: right" Text="Guardar" OnClick="guarda_Click" />
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
    <!-- END Main Content -->

    <a id="btn-scrollup" class="btn btn-circle btn-lg" href="#"></a>

</asp:Content>
