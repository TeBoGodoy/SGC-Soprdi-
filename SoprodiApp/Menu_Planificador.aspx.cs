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
    public partial class Menu_Planificador : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                abarrotes_sp_aspx_capi.Visible = false;

                List<string> u_negocio = ReporteRNegocio.trae_u_negocio(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "1" || u_ne.Trim() == "2")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }
                string sw = Session["SW_PERMI"].ToString();

                if (sw =="")
                {

                    sw = Request.QueryString["s"];
                    Session["SW_PERMI"] = sw;


                }

                try
                {
                    if (Session["SW_PERMI"].ToString() == "4")
                    {
                        titulo2.InnerHtml = "Planificador de Despachos";
                        A1.HRef = "menu.aspx";
                        A1.InnerText = "Back";
                    }
                    else if (Session["SW_PERMI"].ToString() == "1")
                    {
                        titulo2.InnerHtml = "Planificador de Despachos";
                        A1.HRef = "reportes.aspx?s=1";
                        A1.InnerText = "Abarrotes";

                        abarrotes_sp_aspx_capi.Visible = true;

                    }
                    else if (Session["SW_PERMI"].ToString() == "2")
                    {
                        titulo2.InnerHtml = "Planificador de Despachos";
                        A1.HRef = "reportes.aspx?s=2";
                        A1.InnerText = "Granos";
                    }
                }
                catch { }

                //try
                //{
                //    string sw = Request.QueryString["s"];
                //    Session["SW_PERMI"] = sw;


                //    if (Session["SW_PERMI"].ToString() == "1")
                //    {
                //        titulo2.InnerHtml = "Planificador de Despachos";
                //        A1.HRef = "reportes.aspx?s=1";
                //        A1.InnerText = "Abarrotes";
                //    }
                //    else if (Session["SW_PERMI"].ToString() == "2")
                //    {
                //        titulo2.InnerHtml = "Planificador de Despachos";


                //        A1.HRef = "reportes.aspx?s=2";
                //        A1.InnerText = "Granos";
                //    }

                //}
                //catch { }


                if (Session["contraseña"] == null)
                {
                    Response.Redirect("Acceso.aspx");
                }

                //List<string> app = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();

                mant_camion.Attributes["style"] = "display:block";
                mant_chofer.Attributes["style"] = "display:block";
                mant_correo.Attributes["style"] = "display:block";
                mant_trans.Attributes["style"] = "display:block";
                string sw_permi = "";
                try
                {
                    sw_permi = Session["SW_PERMI"].ToString();
                }
                catch
                {

                }
                if (ReporteRNegocio.esvendedor(User.Identity.Name) == "2" || sw_permi == "4")
                {

                    mant_camion.Attributes["style"] = "display:none";
                    mant_chofer.Attributes["style"] = "display:none";
                    mant_correo.Attributes["style"] = "display:none";
                    mant_trans.Attributes["style"] = "display:none";
                    instruir.Attributes["style"] = "display:none";
                }


                if (sw_permi == "4")
                {
                    instruir.Attributes["style"] = "display:block";
                    uno_gestor.Attributes["style"] = "display:none";
                    dos_visor.Attributes["style"] = "display:block";
                }

                //foreach (string u_ne in app)
                //{
                //    if (u_ne.Trim() == "13")
                //    {
                //        cuatro_usu_prod.Attributes["style"] = "display:block";
                //    }

                //    if (u_ne.Trim() == "14")
                //    {
                //        dos_visor.Attributes["style"] = "display:block";
                //    }

                //    if (u_ne.Trim() == "15")
                //    {
                //        uno_gestor.Attributes["style"] = "display:block";
                //    }
                //    if (u_ne.Trim() == "16")
                //    {
                //        tres_log.Attributes["style"] = "display:block";
                //    }

                //}


                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now);
                Response.Cache.SetNoServerCaching();
                Response.Cache.SetNoStore();
            }
        }

    }
}