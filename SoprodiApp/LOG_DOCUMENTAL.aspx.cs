using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoprodiApp.negocio;
using SoprodiApp.entidad;
using System.Drawing;
using Microsoft.Office.Interop;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.Services;
using ThinxsysFramework;

namespace SoprodiApp
{
    public partial class LOG_DOCUMENTAL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            if (!IsPostBack)
            {

                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "16")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }

                CargarGrilla();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp", "<script language='javascript'>superfiltro();</script>", false);
            }  
        }
        public void CargarGrilla()
        {
            DBUtil db = new DBUtil();
            string query = "";
            string usuario = Session["user"].ToString();

            query += "SELECT D.NOMBRE AS DOCUMENTO, rtrim(ltrim(I.InvtID)) + CAST(' - ' AS varchar(MAX)) + I.descr AS CODIGO_PRODUCTO, T.FECHA, T.USUARIO, T.ESTADO, T.OBSERVACION FROM THX_Documento_Log_Visualizador T ";           
            query += "INNER JOIN THX_DOCUMENTOS D ";
            query += "ON T.ID_DOCUMENTO = D.ID ";
            query += " INNER JOIN [192.168.10.11].SoprodiUSDapp.dbo.INVENTORY I ";
            query += " ON D.CODIGO_PRODUCTO = I.INVTID ";
            query += "ORDER BY T.ID DESC "; 

            G_Documentos.DataSource = db.consultar(query);
            G_Documentos.DataBind();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp", "<script language='javascript'>superfiltro();</script>", false);
        }
    }
}