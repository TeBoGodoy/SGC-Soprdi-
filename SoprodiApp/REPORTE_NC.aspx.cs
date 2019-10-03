using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoprodiApp.negocio;
using System.Drawing;
using Microsoft.Office.Interop;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.Services;
using System.Configuration;
using System.Web.Configuration;
using System.Text;
using System.Web.SessionState;
using System.Globalization;

namespace SoprodiApp
{
    public partial class REPORTE_NC : System.Web.UI.Page
    {
        private static string USER;
        private static Page page;
        public static string clie_rut = "";
        public static int COLUMNA_DE_FACTURA;
        public static bool busca_columna_fac;
        public static bool columna_fac;

        protected void Page_Load(object sender, EventArgs e)
        {
            page = this.Page;
            if (!IsPostBack)
            {
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "2" || u_ne.Trim() == "7")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }

                if (Session["SW_PERMI"].ToString() == "1")
                {

                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=1'>Abarrotes</a>";
                    titulo2.HRef = "reportes.aspx?s=1";
                    titulo2.InnerText = "Abarrotes";
                }
                else if (Session["SW_PERMI"].ToString() == "2")
                {
                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=2'>Granos</a>";
                    titulo2.HRef = "reportes.aspx?s=2";
                    titulo2.InnerText = "Granos";

                }


                USER = User.Identity.Name.ToString();
                l_usuario_.Text = USER;
                DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now;
                //////t = new DateTime(t.Year, t.Month - 6, 1);               
                txt_desde.Text = ReporteRNegocio.CALCULA_DESDE(t.Month, t.Year);
                txt_hasta.Text = t2.ToShortDateString();

                string es_vendedor = ReporteRNegocio.esvendedor(USER);
                if (es_vendedor == "2")
                {
                    btn_excel.Visible = false;
                    btn_excel2.Visible = false;
                }
                else
                {
                    btn_excel.Visible = true;
                    btn_excel2.Visible = true;
                }
            }
        }

        private string agregra_comillas(string p)
        {
            if (p != "")
            {
                if (p.Substring(0, 1) != "'")
                {
                    string final_comillas = "";
                    if (p.Contains(","))
                    {
                        List<string> LIST = new List<string>();
                        List<string> LIST2 = new List<string>();
                        LIST.AddRange(p.Split(','));
                        foreach (string a in LIST)
                        {
                            LIST2.Add("'" + a.Trim() + "'");
                        }
                        final_comillas = string.Join(",", LIST2);
                    }
                    else { final_comillas = "'" + p.Trim() + "'"; }
                    return final_comillas;
                }

                else { return p; }
            }
            else { return p; }
        }

        //private void carga_doc_abiertos(string clie_rut)
        //{
        //    DataTable cr = ReporteRNegocio.corr_usuario(User.Identity.Name);
        //    foreach (DataRow r in cr.Rows)
        //    {
        //        tx_enviar_.Text = r[1].ToString();
        //    }

        //    titulo.InnerText = "Documentos Abiertos del cliente " + ReporteRNegocio.nombre_cliente(clie_rut.Replace(".", "").Replace("-", ""));

        //    CLIENTES_FICHA.ActiveViewIndex = 2;
        //    busca_columna_fac = true;
        //    g_doc.DataSource = ReporteRNegocio.docu_abier(clie_rut.Replace(".", "").Replace("-", ""));
        //    g_doc.DataBind();

        //}



        protected void g_doc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (busca_columna_fac)
                {
                    try
                    {
                        for (int x = 0; x <= G_INFORME_TOTAL_NC.HeaderRow.Cells.Count; x++)
                        {
                            if (G_INFORME_TOTAL_NC.HeaderRow.Cells[x].Text.Contains("Factura"))
                            {
                                columna_fac = true;
                                COLUMNA_DE_FACTURA = x;
                                busca_columna_fac = false;
                                break;
                            }
                        }
                    }
                    catch { }
                }
                if (columna_fac)
                {
                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                    string año_factura = e.Row.Cells[6].Text.Substring(6);
                    if (año_factura != "")
                    {
                        año_factura = año_factura.Substring(0, 4);
                        string script = string.Format("javascript:fuera3(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[COLUMNA_DE_FACTURA].Text), encriptador.EncryptData(año_factura));
                        e.Row.Cells[COLUMNA_DE_FACTURA].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[COLUMNA_DE_FACTURA].Text + " </a>";

                        //string script2 = string.Format("javascript:fuera3(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[COLUMNA_DE_FACTURA + 5].Text), encriptador.EncryptData(año_factura));
                        //e.Row.Cells[COLUMNA_DE_FACTURA + 5].Text = "  <a href='javascript:' onclick='" + script2 + "'>" + e.Row.Cells[COLUMNA_DE_FACTURA + 5].Text + " </a>";
                    }
                }
            }
            try
            {
                string valor = e.Row.Cells[0].Text;
                string rut_ini = valor.Trim().Substring(0, valor.Trim().Length - 1);
                double rut = 0;
                try { rut = double.Parse(rut_ini); valor = rut.ToString("N0") + "-" + valor.Trim().Substring(valor.Trim().Length - 1); }
                catch { rut = double.Parse(valor); valor = rut.ToString("N0"); }
                e.Row.Cells[0].Text = valor;

                double d;
                double.TryParse(e.Row.Cells[5].Text, out d);
                string aux = "";
                if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                e.Row.Cells[5].Text = aux;
            }
            catch { }
        }


        protected void btn_informe_Click(object sender, EventArgs e)

        {
            DateTime t = DateTime.Now;
            string fecha_ahora = t.ToShortDateString();
            string desde = txt_desde.Text;
            string hasta = txt_hasta.Text;

            if (desde != "" && hasta != "")
            {
                div_report.Visible = true;
                G_INFORME_NC.Visible = false;
                G_INFORME_TOTAL_NC.Visible = true;

                string where = " where  tipo_doc='CM' and fecha_trans >= CONVERT(datetime,'" + desde + "', 103)  and fecha_trans <= CONVERT(datetime,'" + hasta + "',103) ";

                string es_vend = ReporteRNegocio.esvendedor(User.Identity.Name);
                string nomb_vend = "";
                string grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

                if (grupos_del_usuario == "")
                {
                    grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
                }
                if (es_vend == "2")
                {
                    nomb_vend = ReporteRNegocio.nombre_vendedor(User.Identity.Name);
                }

                if (nomb_vend != "")
                {
                    where += " and (select top 1 vendedor from thx_v_reporte where factura = thx_v_reporte.númfactura) = '" + nomb_vend.Trim() + "' ";

                }
                where += " and  (select top 1 user1 from THX_v_reporte where factura = THX_v_reporte.númfactura) in (" + grupos_del_usuario + ") ";

                DataTable periodos = ReporteRNegocio.listar_notas_credito(where);

                busca_columna_fac = true;
                G_INFORME_TOTAL_NC.DataSource = periodos;
                G_INFORME_TOTAL_NC.DataBind();

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_INFORME_TOTAL_NC')); </script>", false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeqee", "<script> alert('Llene fechas');</script>", false);

            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void btn_excel_Click(object sender, EventArgs e)
        {

            Response.Clear();

            Response.AddHeader("content-disposition", "attachment;filename=SOPRODI_2_" + DateTime.Now.ToShortDateString() + ".xls");

            Response.Charset = "";

            // If you want the option to open the Excel file without saving than

            // comment out the line below

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.ContentType = "application/vnd.xls";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);

            //G_INFORME_TOTAL_NC.Columns[0].Visible = false;

            G_INFORME_TOTAL_NC.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());

            Response.End();

        }

        protected void btn_excel2_Click(object sender, EventArgs e)
        {

        }

        protected void b_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btn_productos_Click(object sender, EventArgs e)
        {

        }

        protected void G_INFORME_NC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                {
                    //DEBE CAMBIAR ->
                    e.Row.Cells[0].Text = "JUAN";
                }
                else { e.Row.Cells[0].Text = ""; }

                int colum = e.Row.Cells.Count;
                int sum_total = 0;
                for (int i = 2; i <= colum - 1; i++)
                {
                    try
                    {
                        sum_total = sum_total + Convert.ToInt32(e.Row.Cells[i].Text);
                    }
                    catch { }
                }
                e.Row.Cells[e.Row.Cells.Count - 1].Text = sum_total.ToString();
                //aux.Rows[e.Row.RowIndex]["Total general"] = sum_total.ToString();
            }
        }

        protected void G_INFORME_TOTAL_NC_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string valor = e.Row.Cells[0].Text;
                string rut_ini = valor.Trim().Substring(0, valor.Trim().Length - 1);
                double rut = 0;
                try { rut = double.Parse(rut_ini); valor = rut.ToString("N0") + "-" + valor.Trim().Substring(valor.Trim().Length - 1); }
                catch { rut = double.Parse(valor); valor = rut.ToString("N0"); }
                e.Row.Cells[0].Text = valor;

                double d;
                double.TryParse(e.Row.Cells[6].Text, out d);
                string aux = "";
                if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                e.Row.Cells[6].Text = aux;

                //Facturación Mes
                if (e.Row.RowIndex == 0)
                {

                    e.Row.Cells[0].BackColor = Color.FromArgb(255, 245, 210);
                }

                if (e.Row.RowIndex == 3)
                {

                    e.Row.Cells[0].BackColor = Color.FromArgb(255, 245, 210);
                }

                if (e.Row.RowIndex == 7)
                {

                    e.Row.Cells[0].BackColor = Color.FromArgb(255, 245, 210);
                }


                if (e.Row.RowIndex == 10)
                {

                    e.Row.Cells[0].BackColor = Color.FromArgb(255, 245, 210);
                }

            }
        }
    }
}