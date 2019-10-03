<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Acceso.aspx.cs" Inherits="SoprodiApp.Acceso" %>

<%@ OutputCache Location="None" NoStore="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">


    <title>Login - Soprodi</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <!-- Place favicon.ico and apple-touch-icon.png in the root directory -->

    <!--base css styles-->
    <link rel="stylesheet" href="assets/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="assets/font-awesome/css/font-awesome.min.css" />

    <!--page specific css styles-->
    <!--flaty css styles-->
    <link rel="stylesheet" href="css/flaty.css" />
    <link rel="stylesheet" href="css/flaty-responsive.css" />

    <%--<link rel="shortcut icon" href="img/favicon.png" />--%>
    <script>

        function cambia() {
            var pass = document.getElementById("T_Pass");

            var aux = document.getElementById("t_aux");

            pass.setAttribute("type", "text");
            pass.style.display = "none";
            aux.style.display = "block";







        }

    </script>



</head>
<body class="login-page" style="background: #e85757 !important;">

    <!-- BEGIN Main Content -->
    <div class="login-wrapper" style="background: #e85757 !important;">
        <!-- BEGIN Login Form -->
        <form runat="server">

            <div class="text-center">
                <img src="img/c8.png" style="" />
            </div>
            <h3>Accede con tu cuenta </h3>


            <asp:Label ID="L_Sesion" runat="server" ForeColor="Red" />
            <hr />
            <div class="form-group">
                <div class="controls">
                    <%--<input type="text" placeholder="Username" class="form-control" />--%>
                    <asp:TextBox ID="T_User" runat="server" placeholder="Username" AUTOCOMPLETE="off" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="controls">
                    <%--<input type="password" placeholder="Password" class="form-control" />--%>
                    <asp:TextBox ID="T_Pass" runat="server" placeholder="Password" AUTOCOMPLETE="off" TextMode="Password" CssClass="form-control"></asp:TextBox>
                    <asp:TextBox ID="t_aux" runat="server" Style="display: none;" Text="***********" AUTOCOMPLETE="off" CssClass="form-control"></asp:TextBox>
                </div>
            </div>

                           <div class="g-recaptcha" style="visibility:hidden;position:absolute;" data-sitekey="6LeCFbgUAAAAABUmjuHKe815pSFNkYGmt2Jn1xlN"></div>


            <div class="form-group">
                <div class="controls">
                    <label class="checkbox">
                        <asp:CheckBox ID="chkRememberMe" runat="server" Visible="false" Checked="false" Text="Recordar" /><br />
                        </label>
                </div>
                <div class="g-recaptcha" data-sitekey="6LeCFbgUAAAAABUmjuHKe815pSFNkYGmt2Jn1xlN">
                    <div class="form-group">
                        <div class="controls">
                            <%--<button type="submit" class="btn btn-primary form-control" runat="server" id="btn_acceder">Sign In</button>--%>
                            <asp:Button CssClass="btn btn-primary form-control" runat="server" ID="btn_login" Text="Ingresar" OnClientClick="cambia()" OnClick="btn_login_Click" />
                        </div>
                    </div>
                </div>

                <hr />
            </div>


        </form>
        <!-- END Login Form -->
    </div>
    <!--basic scripts-->
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script>window.jQuery || document.write('<script src="assets/jquery/jquery-2.1.1.min.js"><\/script>')</script>
    <script src="assets/bootstrap/js/bootstrap.min.js"></script>
</body>
</html>
