using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoprodiApp.negocio;

namespace SoprodiApp
{
    public partial class Menu_Comisiones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<string> u_negocio = ReporteRNegocio.trae_u_negocio(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "3")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }

                //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='MENU_FINANZAS.aspx?'>Finanzas</a>";
                titulo2.InnerHtml = "Comisiones";


                if (Session["contraseña"] == null)
                {
                    Response.Redirect("Acceso.aspx");
                }   

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now);
                Response.Cache.SetNoServerCaching();
                Response.Cache.SetNoStore();
            }

        }

    }
}