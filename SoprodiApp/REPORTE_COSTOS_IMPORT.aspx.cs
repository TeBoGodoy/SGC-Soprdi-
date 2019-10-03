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
    public partial class REPORTE_COSTOS_IMPORT : System.Web.UI.Page
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

                //TX_AÑO.Text = DateTime.Now.Year.ToString();
                //CB_TIPO_DOC_GRILLA.SelectedValue = DateTime.Now.Month.ToString();
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "30")
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
                //DateTime t = DateTime.Now.AddDays(-1);
                //DateTime t2 = t;
                //////t = new DateTime(t.Year, t.Month - 6, 1);               
                //txt_desde.Text = t.ToShortDateString();
                //txt_hasta.Text = t2.ToShortDateString().Substring(0, 2);

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

        public class PRODUCTO
        {
            public string COD_PROD { get; set; }
            public string NOM_PROD { get; set; }

        }

        [WebMethod]
        public static List<PRODUCTO> PRODUCTO_CAMBIO(string sw)
        {
            DataTable dt = new DataTable();


            string clase = "";
            if (sw == "humano")
            {

                clase = "  b.glclassid in ('ABAR', 'MANI') and b.invtid > '1000' and b.invtid <> '9918' ";
            }
            else {
                clase = "  b.glclassid <> 'ABAR' and b.glclassid <>  'MANI' and b.invtid <> '9905'  and b.invtid <> '9999'  and b.invtid <> '9907'   ";

            }


            try
            {
                dt = ReporteRNegocio.listar_ALL_productos_stock_guion_2("", clase);
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



        [WebMethod]
        public static List<PRODUCTO> PRODUCTO_CAMBIO2(string sw)
        {
            DataTable dt = new DataTable();


            string clase = "";
            if (sw == "humano")
            {

                clase = "  b.glclassid in ('ABAR', 'MANI') and b.invtid > '1000' and b.invtid <> '9918' ";
            }
            else {
                clase = "  b.glclassid <> 'ABAR' and b.glclassid <>  'MANI' and b.invtid <> '9905'  and b.invtid <> '9999'  and b.invtid <> '9907'   ";

            }


            try
            {
                dt = ReporteRNegocio.listar_ALL_productos_stock_guion_2(" and b.stkunit <>'KGR' ", clase);
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


        public class INSERT_SOLO_KG
        {
            public string RESTP { get; set; }

        }

        [WebMethod]
        public static string guardar_solo_kg(string sw, string tipo, string valor)
        {

            DataTable dt = new DataTable();
            string respt = "";
            try
            {

                string oka = ReporteRNegocio.guardar_valor_equivale(
               sw,
                tipo,
                valor
                );

                if (oka == "OK")
                {
                    respt = "ok";
                }
                else {
                    respt = "error";
                }


            }
            catch { }
            //}

            return respt;
        }



        public class RESPUESTA_UNIDAD
        {
            public string stkunit { get; set; }
            public string valor { get; set; }
        }

        [WebMethod]
        public static List<RESPUESTA_UNIDAD> carga_click(string sw)
        {



            DataTable dt = new DataTable();

            try
            {

                dt = ReporteRNegocio.valor_prod_equivale(sw);


            }
            catch { }
            //}
            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new RESPUESTA_UNIDAD
                        {
                            stkunit = Convert.ToString(item["stkunit"]),
                            valor = Convert.ToString(item["valor"])
                        };
            return query.ToList<RESPUESTA_UNIDAD>();
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

            //string grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

            //if (grupos_del_usuario == "")
            //{
            //    grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
            //}

            //List<string> grupos = grupos_del_usuario.Split(',').ToList();


            //string clase = " ";

            //if (grupos.Count == 4)
            //{


            //}
            //else
            //{

            //    if (grupos_del_usuario.ToUpper().Replace("'", "").Trim() == "GRANOS")
            //    {
            //        clase += " where b.glclassid in ('INSU', 'CANI')  ";
            //    }
            //    else if (grupos_del_usuario.Contains("Abarrotes") || grupos_del_usuario.Contains("CMQuillota") || grupos_del_usuario.Contains("MayoristasLV"))
            //    {

            //        clase += " where b.glclassid in ('ABAR', 'MANI') ";
            //    }

            //}




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
                    else if (grupos_del_usuario.Contains("Abarrotes") || grupos_del_usuario.Contains("CMQuillota") || grupos_del_usuario.Contains("MayoristasLV"))
                    {

                        clase += " where b.glclassid in ('ABAR', 'MANI') ";
                    }

                }

                dt = ReporteRNegocio.listar_ALL_productos_stock(where2, clase);
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
                    sub_dolar += double.Parse(e.Row.Cells[13].Text);
                    G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[13].Text = "Dolar = " + sub_dolar.ToString("N0");
                }
                catch (Exception e6)
                {

                }
                //sorter - false
                //if (e.Row.Cells[12].Text != "noootiene")
                //{
                //    e.Row.Cells[1].Text = "<a data-toggle='tooltip' style='color:red;' data-placement='top' title='" + e.Row.Cells[12].Text + "'> " + e.Row.Cells[1].Text + "</a>";
                //}

                //e.Row.Cells[12].Visible = false;
                ////e.Row.Cells[].Visible = false;
                //G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[12].Visible = false;

            }
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


        protected void G_PRODUCTOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


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
            string grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

            if (grupos_del_usuario == "")
            {
                grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
            }
            if (grupos_del_usuario.Trim() == "Granos")
            {
                DataTable dt; DataView dtv = new DataView();
                //cargar_combo_bodega(ReporteRNegocio.listar_ALL_cliente2(""), dtv);
                cargar_combo_producto(ReporteRNegocio.listar_ALL_productos_stock_granos(""), dtv);

            }
            else
            {

                DataTable dt; DataView dtv = new DataView();
                //cargar_combo_bodega(ReporteRNegocio.listar_ALL_cliente2(""), dtv);
                cargar_combo_producto(ReporteRNegocio.listar_ALL_productos_stock_todos(""), dtv);
            }

            //cargar_bodegas();
        }

        private void cargar_productos_no_kg(string clase)
        {
            //                " and FechaFactura <= CONVERT(datetime,'" + HASTA + "',103) and user1 in (" + grupos + ")";

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.listar_ALL_productos_stock_guion_2(" and b.stkunit <>'KGR' ", clase);
            dtv = dt.DefaultView;
            cb_productos_kg.DataSource = dtv;
            cb_productos_kg.DataTextField = "descr";
            cb_productos_kg.DataValueField = "invtid";
            //d_vendedor_.SelectedIndex = -1;
            cb_productos_kg.DataBind();
        }

        private void cargar_bodegas()
        {
            //string DESDE = //txt_desde.Text;
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
            string produc = agregra_comillas(l_vendedores.Text);
            string desde = txt_desde.Text;
            string hasta = txt_hasta.Text;

            string where3 = " where cod >= 1000 and cod <= 9000 ";

            if (desde != "")
            {

                where3 += " and convert(datetime, u.[F.Recib], 103)  >= convert(datetime, '" + desde + "', 103) ";

            }

            if (hasta != "")
            {

                where3 += " and convert(datetime, u.[F.Recib], 103)  <= convert(datetime, '" + hasta + "', 103) ";

            }
            if (produc != "")
            {

                where3 += " and  u.cod in (" + produc + ")";
            }

            div_report.Visible = true;

            DataTable dt2 = ReporteRNegocio.lista_costosimpot(where3);
            sub_dolar = 0;
            G_INFORME_TOTAL_VENDEDOR.DataSource = dt2;
            G_INFORME_TOTAL_VENDEDOR.DataBind();
            JQ_Datatable();

        }




        protected void carga_d_Click(object sender, EventArgs e)
        {


        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
        }

        protected void G_errores_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void BTN_error_Click(object sender, EventArgs e)
        {

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

            G_errores.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());

            Response.End();

        }

        protected void btn_estimado_Click(object sender, EventArgs e)
        {
            string ponbr = "";
            string invtid = "";
            string ok_insert = "";
            foreach (GridViewRow dtgItem in this.G_INFORME_TOTAL_VENDEDOR.Rows)
            {


                CheckBox Sel = ((CheckBox)G_INFORME_TOTAL_VENDEDOR.Rows[dtgItem.RowIndex].FindControl("chkAccept"));
                bool valor = Sel.Checked;
                if (valor)
                {
                    invtid = G_INFORME_TOTAL_VENDEDOR.DataKeys[dtgItem.RowIndex].Values[0].ToString().Trim();
                    ponbr = G_INFORME_TOTAL_VENDEDOR.DataKeys[dtgItem.RowIndex].Values[1].ToString().Trim();

                    string ok = ReporteRNegocio.insert_compra_sys(invtid, ponbr, t_ob_cobro.Value);

                    if (ok == "OK")
                    {
                        ok_insert += "OK";

                    }
                    else {

                        ok_insert += "Error";
                    }


                }
            }
  

            Button1_Click(sender, e);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  var elem3 = document.getElementById(\"<%=carga_2.ClientID%>\"); elem3.style.display = \"none\";</script>", false);



        }
    }
}