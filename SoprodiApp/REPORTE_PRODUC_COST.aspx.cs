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
    public partial class REPORTE_PRODUC_COST : System.Web.UI.Page
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
                string grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

                if (grupos_del_usuario == "")
                {
                    grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
                }

                List<string> grupos = grupos_del_usuario.Split(',').ToList();

                string clase = " ";

                if (grupos.Count == 4)
                {
                }
                else
                {

                    if (grupos_del_usuario.ToUpper().Replace("'", "").Trim() == "GRANOS")
                    {
                        clase += " where b.glclassid in ('INSU', 'CANI')  ";
                    }
                    else if (grupos_del_usuario.Contains("Abarrotes") || grupos_del_usuario.Contains("CMQuillota") || grupos_del_usuario.ToUpper().Contains("MayoristasLV"))
                    {

                        clase += " where b.glclassid in ('ABAR', 'MANI') ";
                    }
                }

                Session["clase"] = clase;
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


                try
                {
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
                }
                catch
                {
                    Response.Redirect("Acceso.aspx");

                }

                USER = User.Identity.Name.ToString();
                l_usuario_.Text = USER;
                DateTime t = DateTime.Now.AddDays(-1);
                DateTime t2 = t;
                //////t = new DateTime(t.Year, t.Month - 6, 1);               
                txt_desde.Text = t.ToShortDateString();
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
                ImageClickEventArgs ex = new ImageClickEventArgs(1, 2);
                b_Click(sender, ex);

            }
        }



        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            MakeAccessible(G_INFORME_TOTAL_VENDEDOR);
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

        private void cargar_combo_bodega(DataTable dt, DataView dtv)
        {
            //dt.Rows.Add(new Object[] { -1, "-- Todos --" });
            dtv = dt.DefaultView;
            dtv.Sort = "nom_cliente";
            d_bodega.DataSource = dtv;
            d_bodega.DataTextField = "nom_cliente";
            d_bodega.DataValueField = "rut_cliente";
            //d_vendedor_.SelectedIndex = -1;
            d_bodega.DataBind();
        }
        private void cargar_combo_producto(DataTable dt, DataView dtv)
        {

            //dt.Rows.Add(new Object[] { "-- Todos --" });
            dtv = dt.DefaultView;
            dtv.Sort = "descr";
            d_producto.DataSource = dtv;
            d_producto.DataTextField = "descr";
            d_producto.DataValueField = "invtid";
            d_producto.DataBind();

        }


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
        //    string ultimo = vend.Substring(vend.Length - 1, 1);
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

        public class PRODUCTO
        {
            public string COD_PROD { get; set; }
            public string NOM_PROD { get; set; }

        }

        [WebMethod]
        public static List<PRODUCTO> PRODUCTO_POR_BODEGA(string bodega)
        {
            DataTable dt = new DataTable();
            string bodega1 = agregra_comillas2(bodega);
            string where = "";
            if (bodega1 != "")
            {

                where = " and  siteid in(" + bodega1 + ") ";

            }
            else
            {
                bodega1 = "";
            }
            //{
            try
            {
                dt = ReporteRNegocio.listar_ALL_productos_stock(where, HttpContext.Current.Session["clase"].ToString());
                ////dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
                DataView dv = dt.DefaultView;
                dv.Sort = "descr";
                dt = dv.ToTable();
            }
            catch { }
            //}
            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new PRODUCTO
                        {
                            COD_PROD = Convert.ToString(item["invtid"]),
                            NOM_PROD = Convert.ToString(item["descr"])
                        };
            return query.ToList<PRODUCTO>();
        }


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


        protected void btn_informe_Click(object sender, EventArgs e)
        {

            string bodega = agregra_comillas(l_grupos.Text);
            string productos = agregra_comillas(l_vendedores.Text);
            string where = " and 1=1";

            if (bodega != "")
            {
                where += " and bodega in (" + agregra_comillas(bodega) + ")";
            }

            if (productos != "")
            {
                where += " and producto in (" + agregra_comillas(productos) + ") ";
            }


            div_report.Visible = true;
            G_INFORME_TOTAL_VENDEDOR.Visible = true;
            G_INFORME_TOTAL_VENDEDOR.DataSource = ReporteRNegocio.productos_compras(where);
            G_INFORME_TOTAL_VENDEDOR.DataBind();

            string where2 = " and 1=1";

            if (l_grupos.Text != "")
            {
                where2 += " and siteid in (" + agregra_comillas(l_grupos.Text) + ")";
            }
            DataTable dt = new DataTable();
            try
            {
                dt = ReporteRNegocio.listar_ALL_productos_stock(where2, HttpContext.Current.Session["clase"].ToString());
                //dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
                DataView dv3 = dt.DefaultView;
                dv3.Sort = "descr";
                dt = dv3.ToTable();
                d_producto.DataSource = dt;
                d_producto.DataTextField = "descr";
                d_producto.DataValueField = "invtid";
                d_producto.DataBind();


                foreach (ListItem item in d_producto.Items)
                {

                    if (l_vendedores.Text.Contains(item.Value.ToString()))
                    {
                        item.Selected = true;
                    }
                }
            }
            catch { }

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeees", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_INFORME_TOTAL_VENDEDOR')); </script>", false);


        }



        protected void G_INFORME_VENDEDOR_RowDataBound(object sender, GridViewRowEventArgs e)
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
                aux.Rows[e.Row.RowIndex]["Total general"] = sum_total.ToString();
            }
        }
        double sub_peso = 0;
        double sub_dolar = 0;

        double cost_excel = 0;
        double cost_impot = 0;
        double cost_compra = 0;

        double utilidad_exce = 0;
        double utilidad_compra = 0;


        double total_rows = 0;
        double cont_row = 0;

        protected void G_INFORME_TOTAL_VENDEDOR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // aca
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                try
                {
                    e.Row.Cells[5].Text = Base.monto_format(e.Row.Cells[5].Text);
                }
                catch { }

                if (e.Row.Cells[9].Text.Trim() == "TON")

                {

                    try
                    {
                        double compra = Convert.ToDouble(e.Row.Cells[5].Text) / 1000;
                        double costo = Convert.ToDouble(e.Row.Cells[8].Text) / 1000;

                        e.Row.Cells[5].Text = Base.monto_format2(compra);
                        e.Row.Cells[8].Text = Base.monto_format2(costo);

                    }
                    catch
                    {


                    }

                }


                try
                {
                    if (e.Row.Cells[4].Text != "&nbsp;" && e.Row.Cells[5].Text != "&nbsp;")
                    {
                        double excel = Convert.ToDouble(e.Row.Cells[4].Text);
                        double compra = Convert.ToDouble(e.Row.Cells[5].Text);
                        e.Row.Cells[6].Text = (Base.monto_format2(((excel - compra) / excel) * 100)).ToString() + " %";

                        //1,744186046511628
                    }
                }
                catch
                {

                }


                e.Row.Cells[11].Text = "<a href='REPORTE_COMPRA.aspx?O=" + e.Row.Cells[11].Text + "' target='_blank'>" + e.Row.Cells[11].Text + " </a> ";
            }
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


        protected void G_PRODUCTOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                double sum_por_row = 0;
                string vendedor = G_PRODUCTOS.DataKeys[e.Row.RowIndex].Values[2].ToString();
                string cliente = G_PRODUCTOS.DataKeys[e.Row.RowIndex].Values[3].ToString();

                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                e.Row.Cells[1].Attributes["onclick"] = "javascript:fuera('" + encriptador.EncryptData(PERIODOS) + "', '" + encriptador.EncryptData(vendedor) + "', '" + encriptador.EncryptData(cliente) + "', '" + encriptador.EncryptData("4") + "');return false;";
                //e.Row.Cells[3].Attributes["onclick"] = "javascript:fuera22('" + encriptador.EncryptData(cliente) + "', '" + encriptador.EncryptData("88") + "');return false;";


                string script1 = string.Format("javascript:fuera22(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(cliente), encriptador.EncryptData("88"));
                e.Row.Cells[5].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[5].Text + " </a>";


                e.Row.Cells[0].Text = cont_det.ToString();
                cont_det++;
                int colum = productos.Columns.Count;

                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                G_PRODUCTOS.HeaderRow.Cells[2].Visible = false;
                G_PRODUCTOS.HeaderRow.Cells[3].Visible = false;
                for (int i = 5; i <= colum; i++)
                {
                    G_PRODUCTOS.HeaderRow.Cells[i + 1].Attributes["data-sort-method"] = "number";
                    if (e.Row.Cells[i + 1].Text != "0")
                    {
                        double d;
                        double.TryParse(e.Row.Cells[i + 1].Text, out d);
                        string aux = "";
                        if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                        e.Row.Cells[i + 1].Text = aux;
                        sum_por_row += d;
                        if (i != colum)
                        {
                            string script = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;);return false;", encriptador.EncryptData(G_PRODUCTOS.HeaderRow.Cells[i + 1].Text.Substring(0, 6)), encriptador.EncryptData(e.Row.Cells[2].Text), encriptador.EncryptData(e.Row.Cells[3].Text), encriptador.EncryptData("8"));
                            e.Row.Cells[i + 1].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[i + 1].Text + " </a>";
                        }
                    }
                }
                e.Row.Cells[colum + 1].Text = sum_por_row.ToString("N0");

                if (header_sum)
                {
                    int colum2 = productos.Columns.Count;
                    for (int i = 5; i <= colum2; i++)
                    {
                        try
                        {
                            G_PRODUCTOS.HeaderRow.Cells[i + 1].Text = G_PRODUCTOS.HeaderRow.Cells[i + 1].Text + "  (" + Convert.ToDouble(ReporteRNegocio.Facturación_Mes(G_PRODUCTOS.HeaderRow.Cells[i + 1].Text, where)).ToString("N0") + ")";

                        }
                        catch { }
                    }
                    header_sum = false;
                }

            }
        }

        protected void btn_excel_Click(object sender, EventArgs e)
        {
            Response.Clear();

            Response.AddHeader("content-disposition", "attachment;filename=SOPRODI_VEND_CLIE_" + DateTime.Now.ToShortDateString() + ".xls");

            Response.Charset = "";

            // If you want the option to open the Excel file without saving than

            // comment out the line below

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.ContentType = "application/vnd.xls";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);

            G_INFORME_TOTAL_VENDEDOR.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());

            Response.End();

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

            G_PRODUCTOS.Columns[0].Visible = false;

            G_PRODUCTOS.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());

            Response.End();
        }


        protected void b_Click(object sender, ImageClickEventArgs e)
        {

            DataTable dt; DataView dtv = new DataView();
            cargar_combo_bodega(ReporteRNegocio.listar_ALL_cliente2(""), dtv);
            cargar_combo_producto(ReporteRNegocio.listar_ALL_productos_stock("", Session["clase"].ToString()), dtv);

            cargar_bodegas();
        }

        private void cargar_bodegas()
        {
            //string DESDE = txt_desde.Text;
            //string HASTA = txt_hasta.Text;

            //DESDE = DESDE.Replace("-", "/");
            //HASTA = HASTA.Replace("-", "/");

            //string where = " where FechaFactura >= CONVERT(datetime,'" + DESDE + "', 103) " +
            //                " and FechaFactura <= CONVERT(datetime,'" + HASTA + "',103) and user1 in (" + grupos + ")";

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.carga_bodega(where);
            dtv = dt.DefaultView;
            d_bodega_2.DataSource = dtv;
            d_bodega_2.DataTextField = "nom_bodega";
            d_bodega_2.DataValueField = "nom_bodega";
            //d_vendedor_.SelectedIndex = -1;
            d_bodega_2.DataBind();
        }

        private void cargar_combo_clientes(DataTable dataTable, DataView dtv)
        {
            ////dt.Rows.Add(new Object[] { -1, "-- Todos --" });
            //dtv = dataTable.DefaultView;
            //dtv.Sort = "nom_cliente";
            //d_cliente.DataSource = dtv;
            //d_cliente.DataTextField = "nom_cliente";
            //d_cliente.DataValueField = "rut_cliente";
            ////d_vendedor_.SelectedIndex = -1;
            //d_cliente.DataBind();
        }

        protected void G_PRODUCTOS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Producto")
            {

                //vendedor = G_PRODUCTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                //cliente = G_PRODUCTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();
                //G_DET_PRODUCTOS.DataSource = ReporteRNegocio.listar_prod_client(vendedor, cliente, PERIODOS);

            }

        }

        protected void G_DET_PRODUCTOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = cont_det.ToString();
                cont_det++;
                string where_es = " where vendedor = '" + vendedor.Trim() + "' and nombrecliente like '" + cliente + "'  ";
                for (int i = 4; i < e.Row.Cells.Count; i++)
                {
                    if (header_sum2)
                    {
                        G_DET_PRODUCTOS.HeaderRow.Cells[i].Text = G_DET_PRODUCTOS.HeaderRow.Cells[i].Text + "  (" + Convert.ToDouble(ReporteRNegocio.Facturación_Mes(G_DET_PRODUCTOS.HeaderRow.Cells[i].Text.Substring(0, 6), where_es)).ToString("N0") + ")";
                    }
                    if (i == e.Row.Cells.Count - 1) { header_sum2 = false; }

                    double d;
                    double.TryParse(e.Row.Cells[i].Text, out d);
                    string aux = "";
                    if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                    e.Row.Cells[i].Text = aux;
                }

            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string desde_1 = "";
            string desde_2 = "";
            string where = "";
            string fecha_compra = "";

            if (txt_desde.Text != "")
            {

                desde_1 += " and r.trandate <= convert(date, '" + txt_desde.Text + "',103)";

                desde_2 = " and f.fecha <= convert(date, '" + txt_desde.Text + "', 103) ";
                fecha_compra = " and x.RcptDate  <= convert(datetime, '" + txt_desde.Text + "', 103) ";





                string bodega = agregra_comillas(l_grupos.Text);
                string productos = agregra_comillas(l_vendedores.Text);
                string bodega2 = agregra_comillas(lb_bodegas2.Text);


                if (productos != "")
                {

                    where += " and t.producto in (" + productos + ")";
                }
                if (bodega != "")
                {

                    where += " and t.rutcliente in (" + bodega + ")";

                }
                if (bodega2 != "")
                {

                    where += " and t.bodega in (" + bodega2 + ")";

                }


            }
            else
            {


            }


            if (txt_hasta.Text != "")
            {

                desde_1 += " and r.trandate <= convert(date, '" + txt_hasta.Text + "',103)";

                desde_2 = " and f.fecha <= convert(date, '" + txt_hasta.Text + "', 103) ";
                fecha_compra = " and x.RcptDate  <= convert(datetime, '" + txt_hasta.Text + "', 103) ";


                sub_peso = 0;
                sub_dolar = 0;
                total_rows = 0;
                cont_row = 0;
                cost_excel = 0;
                cost_impot = 0;
                cost_compra = 0;

                utilidad_exce = 0;
                utilidad_compra = 0;

                div_report.Visible = true;
                G_INFORME_TOTAL_VENDEDOR.Visible = true;


                DataTable dt2 = ReporteRNegocio.costo_producto(desde_1, desde_2, where, fecha_compra, txt_hasta.Text);

                //DataTable dt2 = ReporteRNegocio.stock_facturado(desde_1, desde_2, where, fecha_compra);
                total_rows = dt2.Rows.Count;
                G_INFORME_TOTAL_VENDEDOR.DataSource = dt2;
                G_INFORME_TOTAL_VENDEDOR.DataBind();
                JQ_Datatable();




            }



        }
    }
}