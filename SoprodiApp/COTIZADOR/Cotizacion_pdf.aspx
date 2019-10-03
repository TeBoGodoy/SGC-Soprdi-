<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cotizacion_pdf.aspx.cs" Inherits="CRM.Cotizacion_pdf" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script>
        function Mensaje(mensaje) {
            alert(mensaje);
        }
    </script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <script>
        function Cierrame() {
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label runat="server" ID="ERROR" Font-Size="Large"></asp:Label>
        </div>
    </form>
</body>
</html>
