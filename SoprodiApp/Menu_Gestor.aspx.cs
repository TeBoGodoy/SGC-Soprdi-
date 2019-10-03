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
    public partial class Menu_Gestor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<string> u_negocio = ReporteRNegocio.trae_u_negocio(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "4")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }

                //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='MENU_GESTOR.aspx?'>Gestor</a>";
                titulo2.InnerHtml = "Gestor";

                if (Session["contraseña"] == null)
                {
                    Response.Redirect("Acceso.aspx");
                }

                List<string> app = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                uno_gestor.Attributes["style"] = "display:none";
                dos_visor.Attributes["style"] = "display:none";
                tres_log.Attributes["style"] = "display:none";
                cuatro_usu_prod.Attributes["style"] = "display:none";
                cinco_mant_lab.Attributes["style"] = "display:none";
                seis_mant_analisis.Attributes["style"] = "display:none";

                foreach (string u_ne in app)
                {
                    if (u_ne.Trim() == "13")
                    {
                        cuatro_usu_prod.Attributes["style"] = "display:block";
                    }

                    if (u_ne.Trim() == "14")
                    {
                        dos_visor.Attributes["style"] = "display:block";
                    }

                    if (u_ne.Trim() == "15")
                    {
                        uno_gestor.Attributes["style"] = "display:block";
                    }
                    if (u_ne.Trim() == "16")
                    {
                        tres_log.Attributes["style"] = "display:block";
                    }
                    if (u_ne.Trim() == "19")
                    {
                        cinco_mant_lab.Attributes["style"] = "display:block";
                    }
                    if (u_ne.Trim() == "20")
                    {
                        seis_mant_analisis.Attributes["style"] = "display:block";
                    }
                }


                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now);
                Response.Cache.SetNoServerCaching();
                Response.Cache.SetNoStore();
            }
        }

    }
}