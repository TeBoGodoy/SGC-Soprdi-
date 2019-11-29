using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoprodiApp.negocio;
using ThinxsysFramework;

namespace SoprodiApp
{
    public partial class Menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["contraseña"] == null)
            {
                Response.Redirect("Acceso.aspx");

            }

            List<string> u_negocio = ReporteRNegocio.trae_u_negocio(User.Identity.Name).Split(',').ToList();
            dos_grano.Attributes["style"] = "display:none";
            uno_abarrote.Attributes["style"] = "display:none";
            tres_finanzas.Attributes["style"] = "display:none";
            cuatro_gestor.Attributes["style"] = "display:none";
            cinco_usuarios.Attributes["style"] = "display:none";
            seis_ficha_cliente.Attributes["style"] = "display:none";
            siete_nacho.Attributes["style"] = "display:none";
            ocho_gama.Attributes["style"] = "display:none";
            nueve_permanencia.Attributes["style"] = "display:none";
            diez_vencimientos.Attributes["style"] = "display:none";
            once_planificador.Attributes["style"] = "display:none";
            doce_cotizador.Attributes["style"] = "display:none";
            trece_admin_cotizador_.Attributes["style"] = "display:none";
            catorce_trayectora.Attributes["style"] = "display:none";


            foreach (string u_ne in u_negocio)
            {
                if (u_ne.Trim() == "1")
                {
                    uno_abarrote.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "2")
                {
                    dos_grano.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "3")
                {
                    tres_finanzas.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "4")
                {
                    cuatro_gestor.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "5")
                {
                    cinco_usuarios.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "6")
                {
                    seis_ficha_cliente.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "7")
                {
                    siete_nacho.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "8")
                {
                    Session["SW_PERMI"] = "1";
                    ocho_gama.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "9")
                {
                    nueve_permanencia.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "10")
                {
                    diez_vencimientos.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "11")
                {
                    Session["SW_PERMI"] = "";
                    once_planificador.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "12")
                {                   
                    doce_cotizador.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "13")
                {
                    trece_admin_cotizador_.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "14")
                {
                    catorce_trayectora.Attributes["style"] = "display:block";
                }

            }
            //List<string> u_app = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
            //bool correcto_app = false;
            //foreach (string u_ne in u_app)
            //{
            //if (User.Identity.Name.ToUpper() == "GAMA")
            //{
            //    Session["SW_PERMI"] = "1";
            //    ocho_gama.Attributes["style"] = "display:block";
            //}
            //}
            //Response.Redirect("REPORTES.aspx?s=3");            
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();

        }


    }
}