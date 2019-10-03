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
using System.Data.OleDb;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;
using ThinxsysFramework;

namespace SoprodiApp
{
    public partial class REPORTE_EXCEL_COSTOS : System.Web.UI.Page
    {
        public static int cont_periodos;
        public static DataTable aux;
        public static DataTable productos;
        public static DataTable totales;
        public static string PERIODOS;
        private static string USER;
        private static string where = " where 1=1 ";
        private static Page page;

        public static bool header_sum = true;
        public static bool header_sum2 = true;
        private static string vendedor;
        private static string cliente;
        public static int cont_det;
        public static bool permiso;


        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.RegisterRedirectOnSessionEndScript();
            JQ_Datatable();
            page = this.Page;
            if (!IsPostBack)
            {
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "26")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }

                //if (Session["SW_PERMI"].ToString() == "1")
                //{

                //    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=1'>Abarrotes</a>";
                //    titulo2.HRef = "reportes.aspx?s=1";
                //    titulo2.InnerText = "Abarrotes";
                //}
                //else if (Session["SW_PERMI"].ToString() == "2")
                //{
                //    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=2'>Granos</a>";
                //    titulo2.HRef = "reportes.aspx?s=2";
                //    titulo2.InnerText = "Granos";

                //}

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
                DateTime t = DateTime.Now.AddDays(-1);
                DateTime t2 = t;
                //////t = new DateTime(t.Year, t.Month - 6, 1);               
                //txt_desde.Text = t.ToShortDateString();


                string es_vendedor = ReporteRNegocio.esvendedor(USER);
                if (es_vendedor == "2")
                {
                    btn_excel.Visible = false;
                    //btn_excel2.Visible = false;
                }
                else
                {
                    btn_excel.Visible = true;
                    //btn_excel2.Visible = true;
                }
                ImageClickEventArgs ex = new ImageClickEventArgs(1, 2);
                //b_Click(sender, ex);
            }
        }



        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            MakeAccessible(G_EXCEL_COSTO);
            //MakeAccessible(G_ASIGNADOS);
            //MakeAccessible(G_QR);
            //MakeAccessible(G_LISTA);
        }
        public void JQ_Datatable()
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd1Q21mp", "<script language='javascript'>creagrilla();</script>", false);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            Page.ClientScript.RegisterForEventValidation(this.UniqueID);
            base.Render(writer);
        }

        public static void MakeAccessible(GridView grid)
        {
            if (grid.Rows.Count <= 0) return;
            grid.UseAccessibleHeader = true;
            grid.HeaderRow.TableSection = TableRowSection.TableHeader;
            grid.PagerStyle.CssClass = "GridPager";
            if (grid.ShowFooter)
                grid.FooterRow.TableSection = TableRowSection.TableFooter;
        }



        [WebMethod]
        public static string historial_Excel(string cod, string fecha, string columna)
        {

            DataTable dtar = ReporteRNegocio.excel_valor(cod, fecha.Substring(0, 10), columna);

            // 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'



            //data: [7.0, 6.9, 9.5, 14.5, 18.4, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]

            string aux_stri = string_data_rangos(dtar);
            //TOTALES RANGOS FECHAS
            string series =
              "  [{   name: 'Costo', data: [" + aux_stri.Split('*')[0] + "] }] ";

            string fechas = " [" + aux_stri.Split('*')[1] + "] ";


            return series + '*' + fechas;
        }

        private static string string_data_rangos(DataTable datatable)
        {
            string json = "";
            string fechas = "";
            foreach (DataRow r in datatable.Rows)
            {
                double round = Math.Round(Convert.ToDouble(r[0].ToString()), MidpointRounding.AwayFromZero);
                json += r[0].ToString().Replace(",", ".") + ",";
                fechas += "'" + r[1].ToString().Substring(0, 10) + "',";
            }

            json = json.Substring(0, json.Length - 1);
            return json + "*" + fechas.Substring(0, fechas.Length - 1);
        }




        //name: 'Installation',
        //data: [43934, 52503, 57177, 69658, 97031, 119931, 137133, 154175]




        //private void cargar_combo_bodega(DataTable dt, DataView dtv)
        //{
        //    //dt.Rows.Add(new Object[] { -1, "-- Todos --" });
        //    dtv = dt.DefaultView;
        //    dtv.Sort = "nom_cliente";
        //    d_bodega.DataSource = dtv;
        //    d_bodega.DataTextField = "nom_cliente";
        //    d_bodega.DataValueField = "rut_cliente";
        //    //d_vendedor_.SelectedIndex = -1;
        //    d_bodega.DataBind();
        //}
        //private void cargar_combo_producto(DataTable dt, DataView dtv)
        //{

        //    //dt.Rows.Add(new Object[] { "-- Todos --" });
        //    dtv = dt.DefaultView;
        //    dtv.Sort = "descr";
        //    d_producto.DataSource = dtv;
        //    d_producto.DataTextField = "descr";
        //    d_producto.DataValueField = "invtid";
        //    d_producto.DataBind();

        //}


        [WebMethod(EnableSession = true)]
        public static int end_session()
        {
            page.Session.Abandon();
            return 0;
        }

        //public string Rvendedores_todos()
        //{

        //    string vend = "";
        //    foreach (ListItem item in d_vendedor_.Items)
        //    {
        //        if (item.Value != "-1")
        //        {
        //            if (vend == "")
        //            {
        //                vend += item.Value + ",";
        //            }
        //            else
        //            {
        //                vend += item.Value + ",";
        //            }
        //        }
        //    }
        //    string ultimo = vend.Substring(veprodnd.Length - 1, 1);
        //    if (ultimo == ",")
        //    {
        //        vend = vend.Substring(0, vend.Length - 1);
        //    }
        //    return vend;
        //}
        //private string Rclientes_todos()
        //{
        //    //string vend = "";
        //    //foreach (ListItem item in this.d_cliente.Items)
        //    //{
        //    //    if (item.Value != "-1")
        //    //    {
        //    //        if (vend == "")
        //    //        {
        //    //            vend += item.Value + ",";
        //    //        }
        //    //        else
        //    //        {
        //    //            vend += item.Value + ",";
        //    //        }
        //    //    }
        //    //}
        //    //string ultimo = vend.Substring(vend.Length - 1, 1);
        //    //if (ultimo == ",")
        //    //{
        //    //    vend = vend.Substring(0, vend.Length - 1);
        //    //}
        //    //return vend;
        //}

        //public class PRODUCTO
        //{
        //    public string COD_PROD { get; set; }
        //    public string NOM_PROD { get; set; }

        //}

        //[WebMethod]
        //public static List<PRODUCTO> PRODUCTO_CAMBIO(string sw)
        //{
        //    DataTable dt = new DataTable();


        //    string clase = "";
        //    if (sw == "humano")
        //    {

        //        clase = "  b.glclassid in ('ABAR', 'MANI') and b.invtid > '1000' and b.invtid <> '9918' ";
        //    }
        //    else {
        //        clase = "  b.glclassid <> 'ABAR' and b.glclassid <>  'MANI' and b.invtid <> '9905'  and b.invtid <> '9999'  and b.invtid <> '9907'   ";

        //    }


        //    try
        //    {
        //        dt = ReporteRNegocio.listar_ALL_productos_stock_guion_2(" and b.stkunit <> 'KGR' ", clase);
        //        ////dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
        //        DataView dv = dt.DefaultView;
        //        dv.Sort = "descr";
        //        dt = dv.ToTable();
        //    }
        //    catch { }
        //    //}
        //    var query = from item in dt.AsEnumerable()
        //                where 1 == 1
        //                select new PRODUCTO
        //                {
        //                    COD_PROD = Convert.ToString(item["invtid"]),
        //                    NOM_PROD = Convert.ToString(item["descr"])
        //                };
        //    return query.ToList<PRODUCTO>();
        //}


        //[WebMethod]
        //public static List<PRODUCTO> PRODUCTO_CAMBIO2(string sw)
        //{
        //    DataTable dt = new DataTable();


        //    string clase = "";
        //    if (sw == "humano")
        //    {

        //        clase = "  b.glclassid in ('ABAR', 'MANI') and b.invtid > '1000' and b.invtid <> '9918' ";
        //    }
        //    else {
        //        clase = "  b.glclassid <> 'ABAR' and b.glclassid <>  'MANI' and b.invtid <> '9905'  and b.invtid <> '9999'  and b.invtid <> '9907'   ";

        //    }


        //    try
        //    {
        //        dt = ReporteRNegocio.listar_ALL_productos_stock_guion_2("", clase);
        //        ////dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
        //        DataView dv = dt.DefaultView;
        //        dv.Sort = "descr";
        //        dt = dv.ToTable();
        //    }
        //    catch { }
        //    //}
        //    var query = from item in dt.AsEnumerable()
        //                where 1 == 1
        //                select new PRODUCTO
        //                {
        //                    COD_PROD = Convert.ToString(item["invtid"]),
        //                    NOM_PROD = Convert.ToString(item["descr"])
        //                };
        //    return query.ToList<PRODUCTO>();
        //}


        //[WebMethod]
        //public static List<VENDEDOR> VENDEDOR_POR_GRUPO(string grupos, string desde, string hasta, string usuario)
        //{
        //    DataTable dt = new DataTable();
        //    string grupos_ = agregra_comillas2(grupos);


        //    string es_vendedor = ReporteRNegocio.esvendedor(usuario);


        //    string where = " where FechaFactura >= CONVERT(datetime,'" + desde + "', 103) " +
        //                     " and FechaFactura <= CONVERT(datetime,'" + hasta + "',103) ";



        //    if (es_vendedor == "2")
        //    {
        //        where += " and codvendedor in ('" + usuario + "')";
        //    }

        //    if (!grupos_.Contains("'-1'") && grupos_ != "" && !grupos_.Contains("'-- Todos --'"))
        //    {
        //        where += " and user1 in (" + grupos_ + ")";
        //    }
        //    if (grupos_.Contains("'-- Todos --'") && es_vendedor == "2")
        //    {
        //        where += " and user1 in (" + agregra_comillas2(ReporteRNegocio.grupos_usuario_v_report(USER)) + ")";
        //    }
        //    else if (grupos_.Contains("'-- Todos --'"))
        //    {
        //        where += " and user1 in (" + agregra_comillas2(ReporteRNegocio.grupos_usuario(USER)) + ")";
        //    }
        //    if (grupos == "")
        //    {
        //        where += " and 1=1 ";
        //    }

        //    try
        //    {
        //        dt = ReporteRNegocio.listar_ALL_vendedores(where);
        //        ////dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
        //        DataView dv = dt.DefaultView;
        //        dv.Sort = "nom_vend";
        //        dt = dv.ToTable();
        //    }
        //    catch { }


        //    var query = from item in dt.AsEnumerable()
        //                where 1 == 1
        //                select new VENDEDOR
        //                {
        //                    cod_vendedor = Convert.ToString(item["cod_vend"]),
        //                    nom_vendedor = Convert.ToString(item["nom_vend"])
        //                };
        //    return query.ToList<VENDEDOR>();
        //}

        private static string agregra_comillas2(string p)
        {
            if (p != "")
            {
                if (p.Substring(0, 1) != "'")
                {
                    string final_comillas = "";
                    if (p.Contains(","))
                    {
                        List<string> CLIENTE = new List<string>();
                        List<string> cliente_ = new List<string>();
                        CLIENTE.AddRange(p.Split(','));
                        foreach (string a in CLIENTE)
                        {
                            cliente_.Add("'" + a.Trim() + "'");
                        }
                        final_comillas = string.Join(",", cliente_);
                    }
                    else { final_comillas = "'" + p.Trim() + "'"; }
                    return final_comillas;
                }
                else { return p; }
            }
            else { return p; }

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

        protected void SetColumnsOrder(DataTable table, params String[] columnNames)
        {
            for (int columnIndex = 2; columnIndex < columnNames.Length; columnIndex++)
            {
                table.Columns[columnNames[columnIndex]].SetOrdinal(columnIndex);
            }
        }


        public class excel
        {
            public double c_f { get; set; }
            public double bod_usd { get; set; }
            public double cm_stgo { get; set; }
            public double cm_qta { get; set; }
            public double arica { get; set; }
            public String cod_producto { get; set; }
            public String producto { get; set; }
            public String pack { get; set; }
            public int cajas_pallet { get; set; }
            public int pallets_camion { get; set; }
            public int camio_v_metropol { get; set; }
            public int int_out_cm_qt_lv { get; set; }
            //public int camio_has_viii { get; set; }
            public int quillota { get; set; }
            public int bod_sn_anton { get; set; }
            public int arica_ { get; set; }
            public double arica_usd { get; set; }
            public String fecha { get; set; }

        }
        protected void btn_informe_Click(object sender, EventArgs e)
        {

            div_report.Visible = true;
            //ACA CLICK 

            DataTable dt = ReporteRNegocio.ultimo_excel_dt(txt_desde.Text);

            G_EXCEL_COSTO.DataSource = dt;
            G_EXCEL_COSTO.DataBind();


        }

        private string monto_miles(string p)
        {
            bool es_dolar = false;
            double d;
            try
            {
                double.TryParse(p.Substring(0, p.IndexOf(",")), out d);
                es_dolar = true;
            }
            catch
            {
                double.TryParse(p, out d);
            }
            string aux = "";
            if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
            if (es_dolar) { return aux + "," + p.Substring(p.IndexOf(",")); } else { return aux; }
        }

        public double Percentile(int[] sequence, double excelPercentile)
        {

            Array.Sort(sequence);
            int N = sequence.Length;
            double n = (N - 1) * excelPercentile + 1;
            // Another method: double n = (N + 1) * excelPercentile;
            if (n == 1d) return sequence[0];
            else if (n == N) return sequence[N - 1];
            else
            {
                int k = (int)n;
                double d = n - k;
                return sequence[k - 1] + d * (sequence[k] - sequence[k - 1]);
            }
        }




        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void btn_excel2_Click(object sender, EventArgs e)
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

            //G_EXCEL_COSTO.Columns[0].Visible = false;

            G_EXCEL_COSTO.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());

            Response.End();
        }







        protected void excel_error_Click(object sender, EventArgs e)
        {
            Response.Clear();

            Response.AddHeader("content-disposition", "attachment;filename=FECHAS_VP_INTRAN" + DateTime.Now.ToShortDateString() + ".xls");

            Response.Charset = "";

            // If you want the option to open the Excel file without saving than

            // comment out the line below

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.ContentType = "application/vnd.xls";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);

            G_EXCEL_COSTO.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());

            Response.End();

        }

        protected void G_EXCEL_COSTO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string script = string.Format("javascript:grafico(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;);return false;", e.Row.Cells[6].Text, e.Row.Cells[19].Text, G_EXCEL_COSTO.HeaderRow.Cells[1].Text);
                e.Row.Cells[1].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[1].Text + " </a>";

                string script2 = string.Format("javascript:grafico(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;);return false;", e.Row.Cells[6].Text, e.Row.Cells[19].Text, G_EXCEL_COSTO.HeaderRow.Cells[2].Text);
                e.Row.Cells[2].Text = "  <a href='javascript:' onclick='" + script2 + "'>" + e.Row.Cells[2].Text + " </a>";

                string script3 = string.Format("javascript:grafico(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;);return false;", e.Row.Cells[6].Text, e.Row.Cells[19].Text, G_EXCEL_COSTO.HeaderRow.Cells[3].Text);
                e.Row.Cells[3].Text = "  <a href='javascript:' onclick='" + script3 + "'>" + e.Row.Cells[3].Text + " </a>";

                string script4 = string.Format("javascript:grafico(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;);return false;", e.Row.Cells[6].Text, e.Row.Cells[19].Text, G_EXCEL_COSTO.HeaderRow.Cells[4].Text);
                e.Row.Cells[4].Text = "  <a href='javascript:' onclick='" + script4 + "'>" + e.Row.Cells[4].Text + " </a>";

                string script5 = string.Format("javascript:grafico(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;);return false;", e.Row.Cells[6].Text, e.Row.Cells[19].Text, G_EXCEL_COSTO.HeaderRow.Cells[5].Text);
                e.Row.Cells[5].Text = "  <a href='javascript:' onclick='" + script5 + "'>" + e.Row.Cells[5].Text + " </a>";



                string script6 = string.Format("javascript:grafico(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;);return false;", e.Row.Cells[6].Text, e.Row.Cells[19].Text, G_EXCEL_COSTO.HeaderRow.Cells[11].Text);
                e.Row.Cells[11].Text = "  <a href='javascript:' onclick='" + script6 + "'>" + e.Row.Cells[11].Text + " </a>";

                string script7 = string.Format("javascript:grafico(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;);return false;", e.Row.Cells[6].Text, e.Row.Cells[19].Text, G_EXCEL_COSTO.HeaderRow.Cells[12].Text);
                e.Row.Cells[12].Text = "  <a href='javascript:' onclick='" + script7 + "'>" + e.Row.Cells[12].Text + " </a>";


                string script8 = string.Format("javascript:grafico(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;);return false;", e.Row.Cells[6].Text, e.Row.Cells[19].Text, G_EXCEL_COSTO.HeaderRow.Cells[13].Text);
                e.Row.Cells[13].Text = "  <a href='javascript:' onclick='" + script8 + "'>" + e.Row.Cells[13].Text + " </a>";

                string script9 = string.Format("javascript:grafico(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;);return false;", e.Row.Cells[6].Text, e.Row.Cells[19].Text, G_EXCEL_COSTO.HeaderRow.Cells[14].Text);
                e.Row.Cells[14].Text = "  <a href='javascript:' onclick='" + script9 + "'>" + e.Row.Cells[14].Text + " </a>";


                string script10 = string.Format("javascript:grafico(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;);return false;", e.Row.Cells[6].Text, e.Row.Cells[19].Text, G_EXCEL_COSTO.HeaderRow.Cells[15].Text);
                e.Row.Cells[15].Text = "  <a href='javascript:' onclick='" + script10 + "'>" + e.Row.Cells[15].Text + " </a>";


                string script11 = string.Format("javascript:grafico(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;);return false;", e.Row.Cells[6].Text, e.Row.Cells[19].Text, G_EXCEL_COSTO.HeaderRow.Cells[16].Text);
                e.Row.Cells[16].Text = "  <a href='javascript:' onclick='" + script11 + "'>" + e.Row.Cells[16].Text + " </a>";

                string script12 = string.Format("javascript:grafico(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;);return false;", e.Row.Cells[6].Text, e.Row.Cells[19].Text, G_EXCEL_COSTO.HeaderRow.Cells[17].Text);
                e.Row.Cells[17].Text = "  <a href='javascript:' onclick='" + script12 + "'>" + e.Row.Cells[17].Text + " </a>";

                string script13 = string.Format("javascript:grafico(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;);return false;", e.Row.Cells[6].Text, e.Row.Cells[19].Text, G_EXCEL_COSTO.HeaderRow.Cells[18].Text);
                e.Row.Cells[18].Text = "  <a href='javascript:' onclick='" + script13 + "'>" + e.Row.Cells[18].Text + " </a>";

                lb_fecha_exc.InnerText = "Fecha Excel:  " + e.Row.Cells[19].Text.Substring(0, 10);
                e.Row.Cells[0].Visible = false;
                G_EXCEL_COSTO.HeaderRow.Cells[0].Visible = false;


                e.Row.Cells[19].Visible = false;
                G_EXCEL_COSTO.HeaderRow.Cells[19].Visible = false;

            }



        }


        public class excel_f
        {
            public double c_f { get; set; }
            public double bod_LZ { get; set; }
            public double PUERTO { get; set; }
            public double bod_LA { get; set; }
            public double arica { get; set; }
            public String cod_producto { get; set; }
            public String producto { get; set; }
            public String pack { get; set; }
            public int cajas_pallet { get; set; }
            public int cajas_camion { get; set; }

            public double entrega_directa_V_RM { get; set; }
            public double in_out_cm_qta { get; set; }
            public double quillota { get; set; }
            public double bod_LZ_vent { get; set; }
            public double arica_vent { get; set; }
            //public int camio_has_viii { get; set; }
            public double iquique { get; set; }
            public double bod_LA_vent { get; set; }
            public double reparto_RM_V { get; set; }
            public String fecha { get; set; }

        }


        public String fecha_g { get; set; }
        protected void bn_subir_excel_Click(object sender, EventArgs e)
        {

            if (fu_listaprecio.HasFile)
            {
                string ServerPath = HttpContext.Current.Server.MapPath("~").ToString();
                string subPath = ServerPath + "EXCEL_PARAM";
                bool exists = System.IO.Directory.Exists(subPath);
                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(subPath);
                }
                var fileSavePath = System.IO.Path.Combine(subPath, fu_listaprecio.FileName);
                fu_listaprecio.SaveAs(fileSavePath);


                String conStr = "";
                string a = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                string b = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                string Extension = Path.GetExtension(fu_listaprecio.FileName);
                string FilePath = fu_listaprecio.FileName;

                switch (Extension.ToUpper())
                {
                    case ".xls": //Excel 97-03
                        conStr = string.Format(b, fileSavePath, "Yes");
                        break;
                    case ".xlsx": //Excel 07
                        conStr = string.Format(b, fileSavePath, "Yes");
                        break;
                    case ".XLS": //Excel 97-03
                        conStr = string.Format(b, fileSavePath, "Yes");
                        break;
                    case ".XLSX": //Excel 07
                        conStr = string.Format(b, fileSavePath, "Yes");
                        break;
                }

                string sheetName;

                DataTable dt = new DataTable();
                DataTable ultimo_excel = new DataTable();
                //Get the name of the First Sheet.
                using (OleDbConnection con2 = new OleDbConnection(conStr))
                {
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        cmd.Connection = con2;
                        con2.Open();
                        DataTable dtExcelSchema = con2.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        sheetName = dtExcelSchema.Rows[1]["TABLE_NAME"].ToString();
                        con2.Close();
                    }
                }

                //Read Data from the First Sheet.
                using (OleDbConnection con1 = new OleDbConnection(conStr))
                {
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        using (OleDbDataAdapter oda = new OleDbDataAdapter())
                        {

                            cmd.CommandText = "SELECT * From [" + sheetName + "]";
                            cmd.Connection = con1;
                            con1.Open();
                            oda.SelectCommand = cmd;
                            oda.Fill(dt);
                            con1.Close();
                        }
                    }
                }

                int count_row = 0;
                bool error_insert = false;
                string tabla = "";
                tabla += "<div style='width: 100%;font-family:Helvetica,Arial,sans-serif;font-size:11px'>";
                tabla += "<table style='border: 1px solid #ddd; border-collapse: collapse; border-spacing: 1px !important;' border=1>";
                tabla += "<thead class=\"test\">";
                tabla += "<tr style='background-color: #428bca;color: #fff;'>";

                for (int i = 1; i < 20; i++)
                {
                    tabla += "<th> TITULO_" + i + " </th>";
                }
                tabla += "</tr>";
                tabla += "</thead>";
                tabla += "<tbody>";

                string errores = "";
                foreach (DataRow r in dt.Rows)
                {
                    errores += " -Entra : ";
                    if (count_row == 3)
                    {

                        for (int i = 8; i < 20; i++)
                        {
                            tabla = tabla.Replace("TITULO_" + i, r[i].ToString());
                        }
                    }

                    if (count_row == 4)
                    {

                        for (int i = 1; i < 12; i++)
                        {
                            tabla = tabla.Replace("TITULO_" + i, r[i].ToString());

                        }
                    }


                    excel_f ex = new excel_f();

                    count_row++;
                    if (count_row == 1)
                    {

                        ex.fecha = fecha_g = r[1].ToString();
                        try
                        {
                            ultimo_excel =  ReporteRNegocio.ultimo_excel_dt(fecha_g);

                        }
                        catch
                        {
                            errores += "Falló ultimo excel";
                        }
                    }
                    if (count_row >= 6 && r[6].ToString() != "")
                    {
                        ex.fecha = fecha_g;
                        try { ex.c_f = Convert.ToDouble(r[1].ToString().Replace(".", "")); } catch { ex.c_f = 0; errores += "c_f "; }
                        try { ex.bod_LZ = Convert.ToDouble(r[2].ToString().Replace(".", "")); } catch { ex.bod_LZ = 0; errores += "bod_LZ "; }
                        try { ex.PUERTO = Convert.ToDouble(r[3].ToString().Replace(".", "")); } catch { ex.PUERTO = 0; errores += "PUERTO "; }
                        try { ex.bod_LA = Convert.ToDouble(r[4].ToString().Replace(".", "")); } catch { ex.bod_LA = 0; errores += "bod_LA "; }
                        try { ex.arica = Convert.ToDouble(r[5].ToString().Replace(".", "")); } catch { ex.arica = 0; errores += "arica "; }

                        try { ex.cod_producto = r[6].ToString(); } catch { ex.cod_producto = ""; errores += "cod_producto "; }
                        try { ex.producto = r[7].ToString(); } catch { ex.producto = ""; errores += "producto "; }
                        try { ex.pack = r[8].ToString(); } catch { ex.pack = ""; errores += "pack "; }
                        try { ex.cajas_pallet = Convert.ToInt32(r[9].ToString().Replace(".", "")); } catch { ex.cajas_pallet = 0; errores += "cajas_pallet "; }
                        try { ex.cajas_camion = Convert.ToInt32(r[10].ToString().Replace(".", "")); } catch { ex.cajas_camion = 0; errores += "cajas_camion "; }

                        try { ex.entrega_directa_V_RM = Convert.ToDouble(r[11].ToString().Replace(".", "")); } catch { ex.entrega_directa_V_RM = 0; errores += "entrega_directa_V_RM "; }

                        try { ex.in_out_cm_qta = Convert.ToDouble(r[12].ToString().Replace(".", "")); } catch { ex.in_out_cm_qta = 0; errores += "int_out_cm_qta "; }
                        try { ex.quillota = Convert.ToDouble(r[13].ToString().Replace(".", "")); } catch { ex.quillota = 0; errores += "quillota "; }

                        try { ex.bod_LZ_vent = Convert.ToDouble(r[14].ToString().Replace(".", "")); } catch { ex.bod_LZ_vent = 0; errores += "bod_LZ_vent "; }
                        try { ex.arica_vent = Convert.ToDouble(r[15].ToString().Replace(".", "")); } catch { ex.arica_vent = 0; errores += "arica_vent "; }
                        try { ex.iquique = Convert.ToDouble(r[16].ToString().Replace(".", "")); } catch { ex.iquique = 0; errores += "iquique "; }

                        try { ex.bod_LA_vent = Convert.ToDouble(r[18].ToString().Replace(".", "")); } catch { ex.bod_LA_vent = 0; errores += "bod_LA_vent "; }
                        try { ex.reparto_RM_V = Convert.ToDouble(r[19].ToString().Replace(".", "")); } catch { ex.reparto_RM_V = 0; errores += "reparto_RM_V "; }

                        tabla += fila_de_excel(ex, ultimo_excel);

                        string estado = ReporteRNegocio.insert_datos_excel(ex);

                        //string estado = "OK";

                        if (estado != "OK")
                        {
                            enviar_correo_error(estado);
                            //
                            errores += estado + " ---- ";
                            error_insert = true;
                            break;
                        }
                    }
                }

                tabla += "</tbody></div>";
                if (error_insert)
                {
                    enviar_correo_error(errores);
                    //MessageBox.Show(errores);
                    //return false;

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert1", "<script>alert('Hubo un error : '"+errores+");</script>", false);

                    Console.Write("Error al cargar Excel");
                }
                else
                {
                    //string estado_correo = "OK";
                    string estado_correo = correo_excel_cargado(tabla, fecha_g);
                    if (estado_correo == "OK")
                    {
                        //return true;
                        CambiaPreciosCotizaciones();
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert2", "<script>alert(' CARGADO ');</script>", false);
                       
                    }
                    else
                    {
                        enviar_correo_error("Error al enviar correo");
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert4", "<script>alert('Hubo un error al enviar correo');</script>", false);

                    }
                    Console.Write("Excel cargado!");
                }
            }

        }





        private string fila_de_excel(excel_f ex, DataTable ultimo_excel)
        {
            string aux_fila = "<tr>";

            DataRow[] producto = ultimo_excel.Select("cod_producto = '" + ex.cod_producto.Trim() + "'");
            double c_f = 0;
            double bod_LZ = 0;
            double PUERTO = 0;
            double bod_LA = 0;
            double arica = 0;

            double entrega_directa_V_RM = 0;

            double in_out_cm_qta = 0;
            double quillota = 0;
            double bod_LZ_vent = 0;
            double arica_vent = 0;
            double iquique = 0;
            double bod_LA_vent = 0;
            double reparto_RM_V = 0;

            foreach (DataRow r in producto)
            {
                try { c_f = Convert.ToDouble(r[1].ToString().Replace(".", "")); } catch { c_f = 0; }
                try { bod_LZ = Convert.ToDouble(r[2].ToString().Replace(".", "")); } catch { bod_LZ = 0; }
                try { PUERTO = Convert.ToDouble(r[3].ToString().Replace(".", "")); } catch { PUERTO = 0; }
                try { bod_LA = Convert.ToDouble(r[4].ToString().Replace(".", "")); } catch { bod_LA = 0; }
                try { arica = Convert.ToDouble(r[5].ToString().Replace(".", "")); } catch { arica = 0; }

                try { entrega_directa_V_RM = Convert.ToDouble(r[11].ToString().Replace(".", "")); } catch { entrega_directa_V_RM = 0; }

                try { in_out_cm_qta = Convert.ToDouble(r[12].ToString().Replace(".", "")); } catch { in_out_cm_qta = 0; }
                try { quillota = Convert.ToDouble(r[13].ToString().Replace(".", "")); } catch { quillota = 0; }

                try { bod_LZ_vent = Convert.ToDouble(r[14].ToString().Replace(".", "")); } catch { bod_LZ_vent = 0; }
                try { arica_vent = Convert.ToDouble(r[15].ToString().Replace(".", "")); } catch { arica_vent = 0; }
                try { iquique = Convert.ToDouble(r[16].ToString().Replace(".", "")); } catch { iquique = 0; }

                try { bod_LA_vent = Convert.ToDouble(r[18].ToString().Replace(".", "")); } catch { bod_LA_vent = 0; }
                try { reparto_RM_V = Convert.ToDouble(r[19].ToString().Replace(".", "")); } catch { reparto_RM_V = 0; }
            }

            if (c_f > ex.c_f)
                aux_fila += "<td style='background: yellow;'>" + ex.c_f + "</td>";
            else if (c_f < ex.c_f)
                aux_fila += "<td style='background: green;color: cyan;'>" + ex.c_f + "</td>";
            else
                aux_fila += "<td>" + ex.c_f + "</td>";


            if (bod_LZ > ex.bod_LZ)
                aux_fila += "<td style='background: yellow;'>" + ex.bod_LZ + "</td>";
            else if (bod_LZ < ex.bod_LZ)
                aux_fila += "<td style='background: green;color: cyan;'>" + ex.bod_LZ + "</td>";
            else
                aux_fila += "<td>" + ex.bod_LZ + "</td>";

            if (PUERTO > ex.PUERTO)
                aux_fila += "<td style='background: yellow;'>" + ex.PUERTO + "</td>";
            else if (PUERTO < ex.PUERTO)
                aux_fila += "<td style='background: green;color: cyan;'>" + ex.PUERTO + "</td>";
            else
                aux_fila += "<td>" + ex.PUERTO + "</td>";

            if (bod_LA > ex.bod_LA)
                aux_fila += "<td style='background: yellow;'>" + ex.bod_LA + "</td>";
            else if (bod_LA < ex.bod_LA)
                aux_fila += "<td style='background: green;color: cyan;'>" + ex.bod_LA + "</td>";
            else
                aux_fila += "<td>" + ex.bod_LA + "</td>";

            if (arica > ex.arica)
                aux_fila += "<td style='background: yellow;'>" + ex.arica + "</td>";
            else if (arica < ex.arica)
                aux_fila += "<td style='background: green;color: cyan;'>" + ex.arica + "</td>";
            else
                aux_fila += "<td>" + ex.arica + "</td>";

            aux_fila += "<td>" + ex.cod_producto + "</td>";
            aux_fila += "<td>" + ex.producto + "</td>";
            aux_fila += "<td>" + ex.pack + "</td>";
            aux_fila += "<td>" + ex.cajas_pallet + "</td>";
            aux_fila += "<td>" + ex.cajas_camion + "</td>";

            if (entrega_directa_V_RM > ex.entrega_directa_V_RM)
                aux_fila += "<td style='background: yellow;'>" + ex.entrega_directa_V_RM + "</td>";
            else if (entrega_directa_V_RM < ex.entrega_directa_V_RM)
                aux_fila += "<td style='background: green;color: cyan;'>" + ex.entrega_directa_V_RM + "</td>";
            else
                aux_fila += "<td>" + ex.entrega_directa_V_RM + "</td>";

            if (in_out_cm_qta > ex.in_out_cm_qta)
                aux_fila += "<td style='background: yellow;'>" + ex.in_out_cm_qta + "</td>";
            else if (in_out_cm_qta < ex.in_out_cm_qta)
                aux_fila += "<td style='background: green;color: cyan;'>" + ex.in_out_cm_qta + "</td>";
            else
                aux_fila += "<td>" + ex.in_out_cm_qta + "</td>";

            if (quillota > ex.quillota)
                aux_fila += "<td style='background: yellow;'>" + ex.quillota + "</td>";
            else if (entrega_directa_V_RM < ex.quillota)
                aux_fila += "<td style='background: green;color: cyan;'>" + ex.quillota + "</td>";
            else
                aux_fila += "<td>" + ex.quillota + "</td>";

            if (bod_LZ_vent > ex.bod_LZ_vent)
                aux_fila += "<td style='background: yellow;'>" + ex.bod_LZ_vent + "</td>";
            else if (bod_LZ_vent < ex.bod_LZ_vent)
                aux_fila += "<td style='background: green;color: cyan;'>" + ex.bod_LZ_vent + "</td>";
            else
                aux_fila += "<td>" + ex.bod_LZ_vent + "</td>";

            if (arica_vent > ex.arica_vent)
                aux_fila += "<td style='background: yellow;'>" + ex.arica_vent + "</td>";
            else if (arica_vent < ex.arica_vent)
                aux_fila += "<td style='background: green;color: cyan;'>" + ex.arica_vent + "</td>";
            else
                aux_fila += "<td>" + ex.arica_vent + "</td>";

            if (iquique > ex.iquique)
                aux_fila += "<td style='background: yellow;'>" + ex.iquique + "</td>";
            else if (iquique < ex.iquique)
                aux_fila += "<td style='background: green;color: cyan;'>" + ex.iquique + "</td>";
            else
                aux_fila += "<td>" + ex.iquique + "</td>";

            aux_fila += "<td></td>";

            if (bod_LA_vent > ex.bod_LA_vent)
                aux_fila += "<td style='background: yellow;'>" + ex.bod_LA_vent + "</td>";
            else if (bod_LA_vent < ex.bod_LA_vent)
                aux_fila += "<td style='background: green;color: cyan;'>" + ex.bod_LA_vent + "</td>";
            else
                aux_fila += "<td>" + ex.bod_LA_vent + "</td>";

            if (reparto_RM_V > ex.reparto_RM_V)
                aux_fila += "<td style='background: yellow;'>" + ex.reparto_RM_V + "</td>";
            else if (reparto_RM_V < ex.reparto_RM_V)
                aux_fila += "<td style='background: green;color: cyan;'>" + ex.reparto_RM_V + "</td>";
            else
                aux_fila += "<td>" + ex.reparto_RM_V + "</td>";

            //aux_fila += "<td>" + ex.bod_usd + "</td>";
            //aux_fila += "<td>" + ex.cm_stgo + "</td>";
            //aux_fila += "<td>" + ex.cm_qta + "</td>";
            //aux_fila += "<td>" + ex.arica + "</td>";
            //aux_fila += "<td>" + ex.cod_producto + "</td>";
            //aux_fila += "<td>" + ex.producto + "</td>";
            //aux_fila += "<td>" + ex.pack + "</td>";
            //aux_fila += "<td>" + ex.cajas_pallet + "</td>";
            //aux_fila += "<td>" + ex.pallets_camion + "</td>";
            //aux_fila += "<td>" + ex.camio_v_metropol + "</td>";
            //aux_fila += "<td>" + ex.int_out_cm_qt_lv + "</td>";
            ////aux_fila += "<td>" + ex.camio_has_viii + "</td>";
            //aux_fila += "<td>" + ex.quillota + "</td>";
            //aux_fila += "<td>" + ex.bod_sn_anton + "</td>";
            //aux_fila += "<td>" + ex.arica_ + "</td>";
            //aux_fila += "<td>" + ex.arica_usd + "</td>";
            ////aux_fila += "<td>" + Convert.ToDateTime(ex.fecha).ToShortDateString() + "</td>";

            aux_fila += "</tr>";

            return aux_fila;

        }
        private string enviar_correo_error(string estado)
        {
            try
            {
                DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now.AddDays(4);

                string termina = t2.ToShortDateString();
                string empieza = t.ToShortDateString();
                string correos = trae_correo(3);
                string cc = "esteban.godoy15@gmail.com";

                MailMessage email = new MailMessage();
                email.To.Add(new MailAddress(cc));

                email.From = new MailAddress("informes@soprodi.cl");
                email.Subject = "Error correo carga Excel" + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

                email.Body += "<div style='text-align:center;     display: block !important;' > ";
                email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div> ";
                email.Body += "</div>";
                email.Body += "<div><img src='http://a58.imgup.net/Sopro4d9d.png' style='    float: right;     width: 90px;'> </div><br><br><br><br><br>";

                string body_correo = estado;
                email.Body += "<div> Estimados  :<br> <br>ERROR AL CARGAR EXCEL : </div></br> <br><br><div>" + body_correo + "</div> <br>";

                email.Body += "<br> Para más detalles diríjase a:  <a href='http://srv-app.soprodi.cl' > srv-app.soprodi.cl  </a> <span style='font:5px; color:gray; float:right;'>No Responder Este Correo</span> <br><br>";
                email.Body += "<div style='text-align:center;     display: block !important;' > ";
                email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div> ";
                email.Body += "</div>";

                email.IsBodyHtml = true;
                email.Priority = MailPriority.Normal;

                email.BodyEncoding = System.Text.Encoding.UTF8;

                //GMAIL

                //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                //smtp.EnableSsl = true;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtp.UseDefaultCredentials = false;

                ////smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
                //smtp.Credentials = new NetworkCredential("informes.soprodi@gmail.com", "galaxia1234");
                //try
                //{
                //    Console.Write("aca_");
                //    smtp.Send(email);
                //    email.Dispose();
                //    return "OK";
                //}
                //catch (Exception ex)
                //{
                //    string dd = "error correo servidor " + ex.Message;
                //    Console.Write(dd);
                //    return ex.Message;
                //}


                //SOPRODI

                email.IsBodyHtml = true;
                email.Priority = MailPriority.Normal;
                email.BodyEncoding = System.Text.Encoding.UTF8;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "srv-correo-2.soprodi.cl";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
                try
                {
                    Console.Write("aca_");
                    smtp.Send(email);
                    email.Dispose();
                    return "OK";
                }
                catch (Exception ex)
                {
                    string dd = "error correo servidor " + ex.Message;
                    Console.Write(dd);
                    return ex.Message;
                }

            }
            catch (Exception ex)
            {
                Console.Write("aca_error " + ex.Message.ToString());
                return ex.Message;
            }



        }

        private string trae_correo(int v)
        {
            throw new NotImplementedException();
        }

        private static string correo_excel_cargado(string table, string fecha_excel)
        {
            try
            {
                DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now.AddDays(4);

                string termina = t2.ToShortDateString();
                string empieza = t.ToShortDateString();
                string correos = ReporteRNegocio.trae_correo(3);
                string cc = "esteban.godoy15@gmail.com";

                MailMessage email = new MailMessage();
                //email.To.Add(new MailAddress(cc));
                email.To.Add(new MailAddress("rmc@soprodi.cl"));
                //email.To.Add(new MailAddress(cc));
                email.CC.Add(correos + "," + cc);

                email.From = new MailAddress("informes@soprodi.cl");
                email.Subject = "EXCEL DE PRECIOS CARGADO!! " + fecha_excel + " ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

                email.Body += "<div style='text-align:center;     display: block !important;' > ";
                email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div> ";
                email.Body += "</div>";
                email.Body += "<div><img src='http://a58.imgup.net/Sopro4d9d.png' style='    float: right;     width: 90px;'> </div><br><br><br><br><br>";

                string body_correo = table;
                email.Body += "<div> Estimados  :<br> <br> Se ha cargado correctamente los siguientes datos de costos asociados </div></br> <br><br><div>" + body_correo + "</div> <br>";

                email.Body += "<br> Para más detalles diríjase a:  <a href='http://srv-app.soprodi.cl' > srv-app.soprodi.cl  </a> <span style='font:5px; color:gray; float:right;'>No Responder Este Correo</span> <br><br>";
                email.Body += "<div style='text-align:center;     display: block !important;' > ";
                email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div> ";
                email.Body += "</div>";

                email.IsBodyHtml = true;
                email.Priority = MailPriority.Normal;

                email.BodyEncoding = System.Text.Encoding.UTF8;

                //GMAIL

                //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                //smtp.EnableSsl = true;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtp.UseDefaultCredentials = false;

                ////smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
                //smtp.Credentials = new NetworkCredential("informes.soprodi@gmail.com", "galaxia1234");
                //try
                //{
                //    Console.Write("aca_");
                //    smtp.Send(email);
                //    email.Dispose();
                //    return "OK";
                //}
                //catch (Exception ex)
                //{
                //    string dd = "error correo servidor " + ex.Message;
                //    Console.Write(dd);
                //    return ex.Message;
                //}


                //SOPRODI

                email.IsBodyHtml = true;
                email.Priority = MailPriority.Normal;
                email.BodyEncoding = System.Text.Encoding.UTF8;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "srv-correo-2.soprodi.cl";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
                try
                {
                    Console.Write("aca_");
                    smtp.Send(email);
                    email.Dispose();
                    return "OK";
                }
                catch (Exception ex)
                {
                    string dd = "error correo servidor " + ex.Message;
                    Console.Write(dd);
                    return ex.Message;
                }
            }
            catch (Exception ex)
            {
                Console.Write("aca_error " + ex.Message.ToString());
                return ex.Message;
            }



            //email.IsBodyHtml = true;
            //email.Priority = MailPriority.Normal;

            //email.BodyEncoding = System.Text.Encoding.UTF8;

            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = "srv-correo.soprodi.cl";
            //smtp.Port = 25;
            //smtp.EnableSsl = false;
            //smtp.UseDefaultCredentials = false;

            //smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            //try
            //{
            //    smtp.Send(email);
            //    email.Dispose();
            //}
            //catch (Exception ex)
            //{

            //}

        }

        private static string trae_correo_6()
        {
            string correo = "";
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "select cc from Lista_Correos where id = 6";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            foreach (DataRow r in dt.Rows)
            {

                correo = r[0].ToString();

            }
            return correo;      
        }

        private static string trae_correo_3()
        {
            string correo = "";
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "select cc from Lista_Correos where id = 3";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            foreach (DataRow r in dt.Rows)
            {

                correo = r[0].ToString();

            }
            return correo;
        }


        public bool CambiaPreciosCotizaciones()
        {
            try
            {
                DBUtil db = new DBUtil();
                DataTable dt_precios = db.consultar("select * from stock_excel_2 where fecha = (select MAX(fecha) from stock_excel_2)");
                if (dt_precios.Rows.Count > 0)
                {
                    bool ok = true;
                    foreach (DataRow dr in dt_precios.Rows)
                    {
                        try
                        {
                            string sql = "";
                            int precio_arica = Convert.ToInt32(double.Parse(dr["arica_vent"].ToString()));
                            int precio_entre_direct = Convert.ToInt32(double.Parse(dr["entrega_directa_V_RM"].ToString()));
                            int precio_iquique = Convert.ToInt32(double.Parse(dr["iquique"].ToString()));
                            int precio_LA = Convert.ToInt32(double.Parse(dr["bod_LA_vent"].ToString()));
                            int precio_LZdespacho = Convert.ToInt32(double.Parse(dr["reparto_RM_V"].ToString()));
                            int precio_LZ = Convert.ToInt32(double.Parse(dr["bod_LZ_vent"].ToString()));

                            for (int x = 1; x <= 3; x++)
                            {
                                sql += " " +
                                // ARICA                                
                                " update ctz_cotizacion_det set precio_" + x + " = " + precio_arica.ToString() + ", " +
                                " precio_con_descuento_" + x + " = " + precio_arica.ToString() + ", " +
                                " total_" + x + " = " + precio_arica.ToString() + ", " +
                                " total_iva_" + x + " = ((" + precio_arica.ToString() + " * 19)/100) + " + precio_arica.ToString() + ",  " +
                                " precio_unitario_" + x + " = " + precio_arica.ToString() + "/isnull((select isnull(unidad_equivale,1) from unidad_stock_sp where cod_prod = '" + dr["cod_producto"].ToString() + "'),1), " +
                                " precio_con_descuento_unitario_" + x + " = " + precio_arica.ToString() + "/isnull((select isnull(unidad_equivale,1) from unidad_stock_sp where cod_prod = '" + dr["cod_producto"].ToString() + "'),1) " +
                                " where cod_bodega_" + x + " = 'ARICA' and producto = '" + dr["cod_producto"].ToString() + "' ; " +
                                // ETREDIRECT
                                " update ctz_cotizacion_det set precio_" + x + " = " + precio_entre_direct.ToString() + ", " +
                                 " precio_con_descuento_" + x + " = " + precio_entre_direct.ToString() + ", " +
                                 " total_" + x + " = " + precio_entre_direct.ToString() + ", " +
                                 " total_iva_" + x + " = ((" + precio_entre_direct.ToString() + " * 19)/100) + " + precio_entre_direct.ToString() + ",  " +
                                 " precio_unitario_" + x + " = " + precio_entre_direct.ToString() + "/isnull((select isnull(unidad_equivale,1) from unidad_stock_sp where cod_prod = '" + dr["cod_producto"].ToString() + "'),1), " +
                                 " precio_con_descuento_unitario_" + x + " = " + precio_entre_direct.ToString() + "/isnull((select isnull(unidad_equivale,1) from unidad_stock_sp where cod_prod = '" + dr["cod_producto"].ToString() + "'),1) " +
                                 " where cod_bodega_" + x + " = 'ETREDIRECT' and producto = '" + dr["cod_producto"].ToString() + "' ; " +
                                // IQUIQUE
                                " update ctz_cotizacion_det set precio_" + x + " = " + precio_iquique.ToString() + ", " +
                                 " precio_con_descuento_" + x + " = " + precio_iquique.ToString() + ", " +
                                 " total_" + x + " = " + precio_iquique.ToString() + ", " +
                                 " total_iva_" + x + " = ((" + precio_iquique.ToString() + " * 19)/100) + " + precio_iquique.ToString() + ",  " +
                                 " precio_unitario_" + x + " = " + precio_iquique.ToString() + "/isnull((select isnull(unidad_equivale,1) from unidad_stock_sp where cod_prod = '" + dr["cod_producto"].ToString() + "'),1), " +
                                 " precio_con_descuento_unitario_" + x + " = " + precio_iquique.ToString() + "/isnull((select isnull(unidad_equivale,1) from unidad_stock_sp where cod_prod = '" + dr["cod_producto"].ToString() + "'),1) " +
                                 " where cod_bodega_" + x + " = 'IQUIQUE' and producto = '" + dr["cod_producto"].ToString() + "' ; " +
                                // LOSANGELES
                                " update ctz_cotizacion_det set precio_" + x + " = " + precio_LA.ToString() + ", " +
                                 " precio_con_descuento_" + x + " = " + precio_LA.ToString() + ", " +
                                 " total_" + x + " = " + precio_LA.ToString() + ", " +
                                 " total_iva_" + x + " = ((" + precio_LA.ToString() + " * 19)/100) + " + precio_LA.ToString() + ",  " +
                                 " precio_unitario_" + x + " = " + precio_LA.ToString() + "/isnull((select isnull(unidad_equivale,1) from unidad_stock_sp where cod_prod = '" + dr["cod_producto"].ToString() + "'),1), " +
                                 " precio_con_descuento_unitario_" + x + " = " + precio_LA.ToString() + "/isnull((select isnull(unidad_equivale,1) from unidad_stock_sp where cod_prod = '" + dr["cod_producto"].ToString() + "'),1) " +
                                 " where cod_bodega_" + x + " = 'LOSANGELES' and producto = '" + dr["cod_producto"].ToString() + "' ; " +
                                // LZ_DESPACHO
                                " update ctz_cotizacion_det set precio_" + x + " = " + precio_LZdespacho.ToString() + ", " +
                                 " precio_con_descuento_" + x + " = " + precio_LZdespacho.ToString() + ", " +
                                 " total_" + x + " = " + precio_LZdespacho.ToString() + ", " +
                                 " total_iva_" + x + " = ((" + precio_LZdespacho.ToString() + " * 19)/100) + " + precio_LZdespacho.ToString() + ",  " +
                                 " precio_unitario_" + x + " = " + precio_LZdespacho.ToString() + "/isnull((select isnull(unidad_equivale,1) from unidad_stock_sp where cod_prod = '" + dr["cod_producto"].ToString() + "'),1), " +
                                 " precio_con_descuento_unitario_" + x + " = " + precio_LZdespacho.ToString() + "/isnull((select isnull(unidad_equivale,1) from unidad_stock_sp where cod_prod = '" + dr["cod_producto"].ToString() + "'),1) " +
                                 " where cod_bodega_" + x + " = 'LZ_DESPACHO' and producto = '" + dr["cod_producto"].ToString() + "' ; " +
                                // ZARATE
                                " update ctz_cotizacion_det set precio_" + x + " = " + precio_LZ.ToString() + ", " +
                                 " precio_con_descuento_" + x + " = " + precio_LZ.ToString() + ", " +
                                 " total_" + x + " = " + precio_LZ.ToString() + ", " +
                                 " total_iva_" + x + " = ((" + precio_LZ.ToString() + " * 19)/100) + " + precio_LZ.ToString() + ",  " +
                                 " precio_unitario_" + x + " = " + precio_LZ.ToString() + "/isnull((select isnull(unidad_equivale,1) from unidad_stock_sp where cod_prod = '" + dr["cod_producto"].ToString() + "'),1), " +
                                 " precio_con_descuento_unitario_" + x + " = " + precio_LZ.ToString() + "/isnull((select isnull(unidad_equivale,1) from unidad_stock_sp where cod_prod = '" + dr["cod_producto"].ToString() + "'),1) " +
                                 " where cod_bodega_" + x + " = 'ZARATE' and producto = '" + dr["cod_producto"].ToString() + "' ; ";
                            }
                            db.Scalar(sql);
                        }
                        catch (Exception ex)
                        {
                            ok = false;
                        }
                    }
                    if (ok)
                    {
                        // cambiar las cantidades y descuentos 
                        db.Scalar("update ctz_cotizacion_det set cantidad_1 = 1, cantidad_2 = 1, cantidad_3 = 1, descuento_1 = 0, descuento_2 = 0, descuento_3 = 0 ");
                        db.Scalar("delete from ctz_cotizaciones_servicios where tipo_servicio = 'AGREGADO A LOS PRODUCTOS'; ");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected void enviar_Correo__Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}