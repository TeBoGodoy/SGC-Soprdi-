using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.BusinessLayer;
using CRM.Entities;
using SoprodiApp.entidad;
using SoprodiApp.negocio;
using System.Web.Security;
namespace COTIZADOR
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Usuario"] == null)
                {
                    Response.Redirect("../Acceso.aspx");
                }
               
            }           
        }
    }
}