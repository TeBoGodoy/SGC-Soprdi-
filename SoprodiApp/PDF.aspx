<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PDF.aspx.cs" Inherits="SoprodiApp.PDF" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>

<script>
    
    function fuera22(url) {
        alert("aca");
        var urlPdf = url;

        if (navigator.appName.indexOf('Microsoft Internet Explorer') != -1) {
            window.showModelessDialog(urlPdf_Final, '', 'dialogTop:50px;dialogLeft:50px;dialogHeight:500px;dialogWidth:1100px');
        };

        if (navigator.appName.indexOf('Netscape') != -1) {
            window.open(urlPdf_Final, '', 'width=1100,height=500,left=50,top=50');
            void 0;
        };
        return false;
    }
</script>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
