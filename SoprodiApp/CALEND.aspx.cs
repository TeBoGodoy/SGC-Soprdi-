using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.Services;
using ThinxsysFramework;
using System.Web.Script.Serialization;

namespace SoprodiApp
{
    public partial class CALEND : System.Web.UI.Page
    {
        public class Eventos_calendario
        {
            public string rutcliente { get; set; }
            public string start { get; set; }
            public string codvendedor { get; set; }
            public string backgroundColor { get; set; }
            public string textColor { get; set; }
            public string nombre_contacto { get; set; }
            public string am_pm { get; set; }
            public string obvservacion { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DBUtil db = new DBUtil();
            string CODVENDEDOR = "";

            string user = Session["user"].ToString();

            // Guardo Valores
            if (Request.QueryString["VEND"] != null)
            {
                CODVENDEDOR = Request.QueryString["VEND"].ToString();
            }

            string consulta = "";

            consulta += "select *, '#04B404' as color from [dbo].[AgendaContacto] where codvendedor = '" + CODVENDEDOR.Trim() + "'";

            dt = db.consultar(consulta);

            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new Eventos_calendario
                        {
                            rutcliente = Convert.ToString(item["rutcliente"]).Replace("'", ""),
                            codvendedor = Convert.ToString(item["codvendedor"]),
                            start = Convert.ToString(item["fecha_agenda"]).Replace("/", "-"),
                            nombre_contacto = Convert.ToString(item["nombre_contacto"]).Replace("'", ""),
                            am_pm = Convert.ToString(item["am_pm"]).Replace("'", ""),
                            backgroundColor = Convert.ToString(item["color"]).Replace("'", ""),
                            obvservacion = Convert.ToString(item["observacion"]).Replace("'", "")
                        };

            JavaScriptSerializer jss = new JavaScriptSerializer();
            String json = jss.Serialize(query);

            Response.Write(json);
            Response.End();
        }
    }

}
