using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

public static class helper
{
    public static void RegisterRedirectOnSessionEndScript(this Page page)
    {
        /// Login Page, We can retrieve for configuration file (Web.Config)
        string loginPage = "Acceso.aspx";
        /// Session Timeout (Default 20 minutes)
        int sessionTimeout = HttpContext.Current.Session.Timeout;
        /// Timeout for Redirect to Login Page (10 milliseconds before)
        int redirectTimeout = (sessionTimeout * 90000) - 10;

        /// JavaScript Code
        StringBuilder javascript = new StringBuilder();
        javascript.Append("var redirectTimeout;");
        javascript.Append("clearTimeout(redirectTimeout);");
        javascript.Append(String.Format("setTimeout(\"window.location.href='{0}'\",{1});", loginPage, redirectTimeout));

        /// Register JavaScript Code on WebPage
        page.ClientScript.RegisterStartupScript(page.GetType(),
                                                "RegisterRedirectOnSessionEndScript",
                                                javascript.ToString(),
                                                true);
    }
}
