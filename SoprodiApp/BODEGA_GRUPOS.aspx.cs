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
    public partial class BODEGA_GRUPO : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["contraseña"] == null)
            {
                Response.Redirect("Acceso.aspx");

            
            }

            string es_vendedor = ReporteRNegocio.esvendedor(User.Identity.Name.ToString());
            if (es_vendedor == "2") 
            {
                Response.Redirect("REPORTE_LV_G.aspx");
            
            }


            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();

        }

    }
}