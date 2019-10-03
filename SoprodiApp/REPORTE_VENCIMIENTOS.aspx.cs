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
    public partial class REPORTE_VENCIMIENTOS : System.Web.UI.Page
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
                    if (u_ne.Trim() == "46")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }

                titulo2.HRef = "menu.aspx";
                titulo2.InnerText = "Vencimiento";

                USER = User.Identity.Name.ToString();
                l_usuario_.Text = USER;

                DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now;
                //////t = new DateTime(t.Year, t.Month - 6, 1);               
                txt_desde.Text = "01" + t.AddMonths(-7).ToShortDateString().Substring(2);
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
            MakeAccessible(G_GRID);
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
            else
            {
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
            else
            {
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
                else
                {
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


            div_report.Visible = true;
            G_GRID.Visible = true;
            G_GRID.DataSource = ReporteRNegocio.productos_compras(where);
            G_GRID.DataBind();

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
                //sorter - false

                if (e.Row.Cells[4].Text.Contains("TOTAL"))
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Attributes["style"] = "background-color: #aaffaa";
                    e.Row.CssClass = "no-sort";

                    e.Row.Cells[0].Attributes["style"] = "background-color: white";
                    e.Row.Cells[1].Attributes["style"] = "background-color: white";
                }





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

            G_GRID.RenderControl(htmlWrite);

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
            string clase = "";
            if (rd_humano.Checked)
            {

                clase = "  b.glclassid in ('ABAR', 'MANI') and b.invtid > '1000' and b.invtid <> '9918' ";
            }
            else
            {
                clase = "  b.glclassid <> 'ABAR' and b.glclassid <>  'MANI'  and b.invtid <> '9905'  and b.invtid <> '9999'  and b.invtid <> '9907'   ";

            }

            DataTable dt; DataView dtv = new DataView();
            cargar_combo_bodega(ReporteRNegocio.listar_ALL_cliente2(""), dtv);
            cargar_combo_producto(ReporteRNegocio.listar_ALL_productos_stock_guion_2("", clase), dtv);

            cargar_bodegas();

            cargar_productos_no_kg(clase);

        }

        private void cargar_productos_no_kg(string clase)
        {
            //                " and FechaFactura <= CONVERT(datetime,'" + HASTA + "',103) and user1 in (" + grupos + ")";

            //DataTable dt; DataView dtv = new DataView();
            //dt = ReporteRNegocio.listar_ALL_productos_stock_guion_2(" and b.stkunit <>'KGR' ", clase);
            //dtv = dt.DefaultView;
            //cb_productos_kg.DataSource = dtv;
            //cb_productos_kg.DataTextField = "descr";
            //cb_productos_kg.DataValueField = "invtid";
            ////d_vendedor_.SelectedIndex = -1;
            //cb_productos_kg.DataBind();
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

        }

        protected void Button1_Click(object sender, EventArgs e)
        {


            string desde_1 = "";
            string desde_2 = "";
            string where = "";
            string fecha_compra = "";

            if (txt_desde.Text == "")
            {

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeees", "<script> alert('Debe elegir DESDE');</script>", false);
                return;

            }

            if (txt_hasta.Text == "")
            {

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeees", "<script> alert('Debe elegir HASTA');</script>", false);
                return;

            }

            string bodega = agregra_comillas(l_grupos.Text);
            string productos = agregra_comillas(l_vendedores.Text);
            string bodega2 = agregra_comillas(lb_bodegas2.Text);
            string where_bodega = "";
            if (productos != "")
            {

                where += " and invtid in (" + productos + ")";
            }
            if (bodega2 != "")
            {

                where += " and siteid in (" + bodega2 + ")";
                where_bodega += " and siteid in (" + bodega2 + ")";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeees", "<script> alert('Debe elegir una bodega');</script>", false);
                return;

            }

            div_report.Visible = true;
            G_GRID.Visible = true;

            string clase = "";
            if (rd_humano.Checked)
            {

                where += " and  glclassid in ('ABAR', 'MANI') and invtid > '1000' and invtid <> '9918' ";
            }
            else
            {
                where += " and  glclassid <> 'ABAR' and glclassid <>  'MANI'  and invtid <> '9905'  and invtid <> '9999'  and invtid <> '9907'   ";

            }

            /////ACA ES _LA GRID

            DataTable dt2 = crear_cobro_(where);

            DataTable dt_final = dt2.Clone();

            DateTime desde = Convert.ToDateTime(txt_desde.Text);
            DateTime hasta = Convert.ToDateTime(txt_hasta.Text);

            foreach (DataRow r in dt2.Rows)
            {
                if (r[5].ToString() != "0")
                {

                    DateTime movimiento = Convert.ToDateTime(r["t"].ToString());

                    if (movimiento >= desde && movimiento <= hasta)
                    {

                        if (r["qty"].ToString() != r["queda"].ToString())
                        {
                            double queda_d = Convert.ToDouble(r["queda"].ToString());
                            if (r["stkunit"].ToString().Trim() == "KGR")
                            {
                                r["ton"] = queda_d / 1000;

                            }
                            else
                            {
                                double equivale = ReporteRNegocio.trae_equivale_kg(r["invtid"].ToString());

                                r["ton"] = (queda_d * equivale) / 1000;
                            }

                        }

                        dt_final.ImportRow(r);

                    }



                }
            }

            Session["dt_final_g_grid"] = dt_final;

            //G_GRID2.DataSource = dt2;
            //G_GRID2.DataBind();
            //JQ_Datatable();

            //DataTable dt2 = ReporteRNegocio.lista_vencimientos(where);

            G_GRID.DataSource = dt_final;
            G_GRID.DataBind();
            JQ_Datatable();

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeees", "<script> CARGANDO_CLOSE(); </script>", false);

            if (rd_humano.Checked)
            {

                clase = "  b.glclassid in ('ABAR', 'MANI') and b.invtid > '1000' and b.invtid <> '9918' ";
            }
            else
            {
                clase = "  b.glclassid <> 'ABAR' and b.glclassid <>  'MANI'  and b.invtid <> '9905'  and b.invtid <> '9999'  and b.invtid <> '9907'   ";

            }

            try
            {

                DataView dtv = new DataView();

                cargar_combo_producto(ReporteRNegocio.listar_ALL_productos_stock_guion_2("", clase), dtv);


                foreach (ListItem item in d_producto.Items)
                {

                    if (l_vendedores.Text.Contains(item.Value.ToString()))
                    {
                        item.Selected = true;
                    }
                }
            }
            catch { }
        }

        private DataTable crear_cobro_(string where)
        {

            DataTable entradas = ReporteRNegocio.productos_entradas(where);
            DataTable salidas = ReporteRNegocio.salidas_producto(where);

            double entrada = 0;
            //double diferencia = 0;
            string PRODUCTO_cambio = "";
            foreach (DataRow entr in entradas.Rows)
            {

                //DataRow[] salidas_bodega = salidas.Select("invtid = '" + entr[4].ToString().Trim() + "' and siteid = '"+ entr[3].ToString().Trim() + "'");

                //DataTable dt1 = salidas_bodega.CopyToDataTable();

                if (PRODUCTO_cambio != entr[4].ToString().Trim())
                {
                    PRODUCTO_cambio = entr[4].ToString().Trim();
                    entrada = 0;
                }
                else
                {


                }
                double diferencia = 0;

                entrada = entrada + Convert.ToDouble(entr[0]);

                foreach (DataRow sal in salidas.Rows)
                {
                    if (sal[1].ToString().Trim() == entr[4].ToString().Trim() && entr[3].ToString().Trim() == sal[3].ToString().Trim())
                    {
                        if (sal[4].ToString() != "SI")
                        {
                            diferencia = entrada + Convert.ToDouble(sal[0]);

                            if (diferencia > 0)
                            {
                                entr[5] = diferencia;
                                entrada = diferencia;
                                sal[4] = "SI";

                            }
                            else
                            {
                                sal[4] = "SI";
                                entr[5] = 0;
                                entrada = diferencia;
                                break;
                            }
                        }
                    }

                }


                string stop = "";


            }




            return entradas;

        }

        protected void carga_d_Click(object sender, EventArgs e)
        {


        }

        protected void Button3_Click(object sender, EventArgs e)
        {
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            string desde_1 = "";
            string desde_2 = "";
            string where = "";

            string bodega = agregra_comillas(l_grupos.Text);
            string productos = agregra_comillas(l_vendedores.Text);
            string bodega2 = agregra_comillas(lb_bodegas2.Text);
            string where_bodega = "";
            if (productos != "")
            {

                where += " and invtid in (" + productos + ")";
            }
            if (bodega2 != "")
            {

                where += " and siteid in (" + bodega2 + ")";
                where_bodega += " and siteid in (" + bodega2 + ")";
            }

            div_report.Visible = true;
            G_GRID.Visible = true;

            DataTable dt2_cobro = new DataTable();
            DataTable dt2 = dt2_cobro;

            total_rows = dt2.Rows.Count;
            G_GRID.DataSource = dt2;
            G_GRID.DataBind();
            JQ_Datatable();
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

        protected void G_GRID_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {



                if (e.Row.Cells[16].Text.Trim() == "si")
                {
                    string combo = "";
                    combo = " <select class=\"form-control input-sm\" style=\"width: 100px;\" id=\"cb_cambio_pago" + e.Row.Cells[0].Text.Split('-')[0].ToString() + "*#" + e.Row.Cells[3].Text + "*#" + e.Row.Cells[4].Text + "*#" + e.Row.Cells[5].Text + "*#" + e.Row.Cells[2].Text + "*#" + e.Row.Cells[1].Text + "\" onchange =\"INGRESO_CERTIFICADO('" + e.Row.Cells[0].Text.Split('-')[0].ToString() + "', '" + e.Row.Cells[3].Text + "', '" + e.Row.Cells[4].Text + "', '" + e.Row.Cells[5].Text + "', '" + e.Row.Cells[2].Text + "', '" + e.Row.Cells[1].Text + "')\"> " +
                                       "                                        <option value = \"\"></option>  " +
                                               "                              <option value = \"si\" selected> SI...</option> " +
                                                "                              <option value=\"no\"> NO...</option> " +
                                                  "                         </select > ";
                    e.Row.Cells[16].Text = combo;
                }
                else if (e.Row.Cells[16].Text.Trim() == "no")
                {

                    double qty = 0;
                    double queda = 0;

                    bool ok2 = false;
                    try
                    {
                        qty = Convert.ToDouble(e.Row.Cells[6].Text);
                        queda = Convert.ToDouble(e.Row.Cells[7].Text);
                        ok2 = true;

                    }
                    catch
                    {

                    }
                    if (ok2)
                    {

                        double result = Math.Round(100 - ((queda * 100) / qty));

                        if (result >= 20)
                        {
                            e.Row.Cells[17].BackColor = Color.FromArgb(125, 60, 152);
                            e.Row.Cells[17].ForeColor = Color.FromArgb(0, 0, 0);
                        }

                        e.Row.Cells[17].Text = Math.Round(100- ((queda * 100) / qty)).ToString() + " %";



                    }



                    string combo = "";
                    combo = " <select class=\"form-control input-sm\"  style=\"width: 100px;\" id=\"cb_cambio_pago" + e.Row.Cells[0].Text.Split('-')[0].ToString() + "*#" + e.Row.Cells[3].Text + "*#" + e.Row.Cells[4].Text + "*#" + e.Row.Cells[5].Text + "*#" + e.Row.Cells[2].Text + "*#" + e.Row.Cells[1].Text + "\" onchange =\"INGRESO_CERTIFICADO('" + e.Row.Cells[0].Text.Split('-')[0].ToString() + "', '" + e.Row.Cells[3].Text + "', '" + e.Row.Cells[4].Text + "', '" + e.Row.Cells[5].Text + "', '" + e.Row.Cells[2].Text + "', '" + e.Row.Cells[1].Text + "')\"> " +
                                      "                                        <option value = \"\"></option>  " +
                                              "                               <option value = \"si\"> SI...</option> " +
                                                "                              <option value=\"no\" selected> NO...</option> " +
                                                 "                         </select > ";

                    e.Row.Cells[16].Text = combo;
                }
                else
                {

                    double qty = 0;
                    double queda = 0;

                    bool ok2 = false;
                    try
                    {
                        qty = Convert.ToDouble(e.Row.Cells[6].Text);
                        queda = Convert.ToDouble(e.Row.Cells[7].Text);
                        ok2 = true;

                    }
                    catch
                    {

                    }
                    if (ok2)
                    {

                        double result = Math.Round(100 - ((queda * 100) / qty));

                        if (result >= 20)
                        {
                            e.Row.Cells[17].BackColor = Color.FromArgb(125, 60, 152);
                            e.Row.Cells[17].ForeColor = Color.FromArgb(0, 0, 0);
                        }

                        e.Row.Cells[17].Text = Math.Round(100 - ((queda * 100) / qty)).ToString() + " %";



                    }
                    string combo = "";
                    combo = " <select class=\"form-control input-sm\" style=\"width: 100px;\" id=\"cb_cambio_pago" + e.Row.Cells[0].Text.Split('-')[0].ToString() + "*#" + e.Row.Cells[3].Text + "*#" + e.Row.Cells[4].Text + "*#" + e.Row.Cells[5].Text + "*#" + e.Row.Cells[2].Text + "*#" + e.Row.Cells[1].Text + "\" onchange =\"INGRESO_CERTIFICADO('" + e.Row.Cells[0].Text.Split('-')[0].ToString() + "', '" + e.Row.Cells[3].Text + "', '" + e.Row.Cells[4].Text + "', '" + e.Row.Cells[5].Text + "', '" + e.Row.Cells[2].Text + "', '" + e.Row.Cells[1].Text + "')\"> " +
                                       "                                        <option value = \"\"></option>  " +
                                               "                              <option value = \"si\"> SI...</option> " +
                                                "                              <option value=\"no\" selected> NO...</option> " +
                                                  "                         </select > ";
                    e.Row.Cells[16].Text = combo;
                }

                double meses = 0;
                double porct = 0;
                double menos_de = 0;
                bool ok = false;
                try
                {
                    meses = Convert.ToDouble(e.Row.Cells[14].Text);
                    porct = Convert.ToDouble(e.Row.Cells[15].Text);

                    menos_de = 100 - porct;
                    ok = true;
                       
                }
                catch {

                }

                if (ok && meses <= 6 && meses > 3) 
                {
                    e.Row.Cells[14].BackColor = Color.FromArgb(211, 84, 0);
                    e.Row.Cells[14].ForeColor = Color.FromArgb(0, 0, 0);

                }
                else if (ok && meses <= 3)
                {
                    e.Row.Cells[14].BackColor = Color.FromArgb(255, 0, 0);
                    e.Row.Cells[14].ForeColor = Color.FromArgb(255, 255, 255);

                }


                if (ok && porct >= 50)
                {
                    e.Row.Cells[15].BackColor = Color.FromArgb(241, 196, 15);
                    e.Row.Cells[15].ForeColor = Color.FromArgb(0, 0, 0);
                }

                if (ok && menos_de <= 30)
                {
                    e.Row.Cells[15].BackColor = Color.FromArgb(255, 0, 0);
                    e.Row.Cells[15].ForeColor = Color.FromArgb(255, 255, 255);

                }

                if (e.Row.Cells[15].Text != "&nbsp;")
                {
                    e.Row.Cells[15].Text = e.Row.Cells[15].Text + "%";

                }




              


            }


        }


        [WebMethod]
        public static string CERTIFICADO(string invtid_, string siteid_, string batnbr_, string refnbr_, string trandate_, string trantype_, string estado)
        {
            string ook = "";

            string existe_id = ReporteRNegocio.buscar_vencimiento(invtid_, siteid_, batnbr_, refnbr_, trandate_, trantype_);
            if (existe_id == "")
            {
                ook = ReporteRNegocio.insert_vencimiento(invtid_, siteid_, batnbr_, refnbr_, trandate_, trantype_, estado, "certificado");

            }
            else
            {
                ook = ReporteRNegocio.update_vencimiento(existe_id, estado, "certificado");


            }

            return "";
        }


        protected void G_GRID_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "click_guarda_lote")
            {
                string invtid = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                string siteid = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();
                string batnbr = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString();
                string refnbr = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString();
                string trandate = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[7].ToString();
                string trantype = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString();
                string id = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[6].ToString();

                string texto_ = ((TextBox)G_GRID.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("txt_lote")).Text;
                string sw_ = "lote";
                string existe_id = ReporteRNegocio.buscar_vencimiento(invtid, siteid, batnbr, refnbr, trandate, trantype);

                string ook = "";
                if (existe_id == "")
                {
                    ook = ReporteRNegocio.insert_vencimiento(invtid, siteid, batnbr, refnbr, trandate, trantype, texto_, sw_);

                }
                else
                {
                    ook = ReporteRNegocio.update_vencimiento(existe_id, texto_, sw_);


                }

                if (ook == "OK")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "id_variable_123", "<script language='javascript'> alert('Guardado') </script>", false);

                }
                else
                {

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "id_variable_1233", "<script language='javascript'> alert('NO Guardado') </script>", false);

                }
                string asd = "";
            }
            if (e.CommandName == "click_guarda_envasado")
            {
                string invtid = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                string siteid = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();
                string batnbr = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString();
                string refnbr = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString();
                string trandate = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[7].ToString();
                string trantype = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString();
                string id = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[6].ToString();

                string texto_ = ((TextBox)G_GRID.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("txt_envasado")).Text;
                string sw_ = "envasado";
                string existe_id = ReporteRNegocio.buscar_vencimiento(invtid, siteid, batnbr, refnbr, trandate, trantype);

                string ook = "";
                if (existe_id == "")
                {
                    ook = ReporteRNegocio.insert_vencimiento(invtid, siteid, batnbr, refnbr, trandate, trantype, texto_, sw_);

                }
                else
                {


                    //Label duracion = new Label();
                    //duracion = (Label)G_GRID.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("duracion");

                    Label elavo = new Label();
                    elavo = (Label)G_GRID.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("elavo");

                    //Label util = new Label();
                    //util = (Label)G_GRID.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("util");

                    DateTime fecha = DateTime.Now;
                    DateTime fecha_envasado = Convert.ToDateTime(texto_);
                    elavo.Text = TotalMonths(fecha, fecha_envasado).ToString();

                    try
                    {
                        string vencimiento = ((TextBox)G_GRID.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("txt_vencimiento")).Text;

                        Label duracion = new Label();
                        duracion = (Label)G_GRID.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("duracion");

                        Label util = new Label();
                        util = (Label)G_GRID.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("util");

                        DateTime fecha_vencimienti = Convert.ToDateTime(vencimiento);

                        duracion.Text = TotalMonths(fecha_vencimienti, fecha_envasado).ToString();
                        util.Text = (Convert.ToInt32(duracion.Text) - Convert.ToInt32(elavo.Text)).ToString();

                    }
                    catch
                    {

                    }



                    ook = ReporteRNegocio.update_vencimiento(existe_id, texto_, sw_);






                }

                if (ook == "OK")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "id_variable_123", "<script language='javascript'> alert('Guardado') </script>", false);

                }
                else
                {

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "id_variable_1233", "<script language='javascript'> alert('NO Guardado') </script>", false);

                }
                string asd = "";
            }
            if (e.CommandName == "click_guarda_vencimiento")
            {
                string invtid = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                string siteid = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();
                string batnbr = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString();
                string refnbr = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString();
                string trandate = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[7].ToString();
                string trantype = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString();
                string id = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[6].ToString();

                string texto_ = ((TextBox)G_GRID.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("txt_Vencimiento")).Text;
                string sw_ = "vencimiento";
                string existe_id = ReporteRNegocio.buscar_vencimiento(invtid, siteid, batnbr, refnbr, trandate, trantype);

                string ook = "";
                if (existe_id == "")
                {
                    ook = ReporteRNegocio.insert_vencimiento(invtid, siteid, batnbr, refnbr, trandate, trantype, texto_, sw_);

                }
                else
                {
                    try
                    {
                        string envasado = ((TextBox)G_GRID.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("txt_envasado")).Text;

                        Label duracion = new Label();
                        duracion = (Label)G_GRID.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("duracion");

                        Label elavo = new Label();
                        elavo = (Label)G_GRID.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("elavo");

                        Label util = new Label();
                        util = (Label)G_GRID.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("util");

                        DateTime fecha = DateTime.Now;
                        DateTime fecha_vencimienti = Convert.ToDateTime(texto_);
                        DateTime fecha_envasado = Convert.ToDateTime(envasado);


                        duracion.Text = TotalMonths(fecha_vencimienti, fecha_envasado).ToString();
                        util.Text = (Convert.ToInt32(duracion.Text) - Convert.ToInt32(elavo.Text)).ToString();

                    }
                    catch
                    {

                    }


                    ook = ReporteRNegocio.update_vencimiento(existe_id, texto_, sw_);


                }

                if (ook == "OK")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "id_variable_123", "<script language='javascript'> alert('Guardado') </script>", false);




                }
                else
                {

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "id_variable_1233", "<script language='javascript'> alert('NO Guardado') </script>", false);

                }
                string asd = "";
            }

            if (e.CommandName == "click_guarda_certificado")
            {
                string invtid = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                string siteid = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();
                string batnbr = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString();
                string refnbr = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString();
                string trandate = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[7].ToString();
                string trantype = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString();
                string id = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[6].ToString();

                string texto_ = ((TextBox)G_GRID.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("txt_lote")).Text;
                string sw_ = "lote";
                string existe_id = ReporteRNegocio.buscar_vencimiento(invtid, siteid, batnbr, refnbr, trandate, trantype);

                string ook = "";
                if (existe_id == "")
                {
                    ook = ReporteRNegocio.insert_vencimiento(invtid, siteid, batnbr, refnbr, trandate, trantype, texto_, sw_);

                }
                else
                {
                    ook = ReporteRNegocio.update_vencimiento(existe_id, texto_, sw_);


                }

                if (ook == "OK")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "id_variable_123", "<script language='javascript'> alert('Guardado') </script>", false);

                }
                else
                {

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "id_variable_1233", "<script language='javascript'> alert('NO Guardado') </script>", false);

                }
                string asd = "";
            }

            if (e.CommandName == "click_modal_lotes")
            {
                string invtid = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString().Trim();
                string siteid = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString().Trim();
                string batnbr = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString().Trim();
                string refnbr = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString().Trim();
                string trandate = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[7].ToString().Trim();
                string trantype = G_GRID.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString().Trim();

                Session["invtid"] = invtid;
                Session["siteid"] = siteid;
                Session["batnbr"] = batnbr;
                Session["refnbr"] = refnbr;
                Session["trandate"] = trandate;
                Session["trantype"] = trantype;

                DataTable dt1 = ReporteRNegocio.trae_lotes_(invtid, siteid, batnbr, refnbr, trandate, trantype);

                G_LOTES.DataSource = dt1;
                G_LOTES.DataBind();


                ScriptManager.RegisterStartupScript(Page, this.GetType(), "id_variable_33", "<script language='javascript'> modal_unidad() </script>", false);

                btn_nuevo_lote.Visible = true;
                B_Guardar.Visible = false;
                t_envasado.Text = string.Empty;
                t_vencimiento.Text = string.Empty;
                t_lote.Text = string.Empty;
                t_envasado.Enabled = false;
                t_vencimiento.Enabled = false;
                t_lote.Enabled = false;

                string nombre_prod = ReporteRNegocio.nombre_producto(invtid.Trim());


                string tabla = "";

                tabla += "<table border=1 id='TABLA_2' border class='table table-advance table-bordered fill-head tablesorter filtrar' style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";
                tabla += "<thead class=\"test\">";
                tabla += "<tr style='background-color:#428bca'>";

                tabla += "<th>Producto</th>";
                tabla += "<th>Bodega</th>";
                tabla += "<th>BatNbr</th>";
                tabla += "<th>RefNbr</th>";
                tabla += "<th>Fecha</th>";
                tabla += "<th>Tipo</th>";

                tabla += "</tr>";
                tabla += "</thead>";
                tabla += "<tbody>";

                tabla += "<tr>";

                tabla += "<td>" + invtid + "-" + nombre_prod.Trim() + "</td>";
                tabla += "<td>" + siteid + "</td>";
                tabla += "<td>" + batnbr + "</td>";
                tabla += "<td>" + refnbr + "</td>";
                tabla += "<td>" + trandate + "</td>";
                tabla += "<td>" + trantype + "</td>";

                tabla += "</tr>";


                tabla += "</tbody>";
                tabla += "</table>";

                titulo_.InnerHtml = tabla;


                modal_updatepanel.Update();

            }



        }


        private object TotalMonths(DateTime start, DateTime end)
        {
            return (start.Year * 12 + start.Month) - (end.Year * 12 + end.Month);
        }

        protected void G_LOTES_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void G_LOTES_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "b_editar_lote")
            {
                string invtid = G_LOTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString().Trim();
                string siteid = G_LOTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString().Trim();
                string batnbr = G_LOTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString().Trim();
                string refnbr = G_LOTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString().Trim();
                string trandate = G_LOTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[4].ToString().Trim();
                string trantype = G_LOTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString().Trim();
                string lote = G_LOTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[6].ToString().Trim();

                string envasado = G_LOTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[7].ToString().Trim();
                string vencimiento = G_LOTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[8].ToString().Trim();

                Session["invtid"] = invtid;
                Session["siteid"] = siteid;
                Session["batnbr"] = batnbr;
                Session["refnbr"] = refnbr;
                Session["trandate"] = trandate;
                Session["trantype"] = trantype;


                t_lote.Text = lote.Trim();
                t_envasado.Text = envasado.Trim();
                t_vencimiento.Text = vencimiento.Trim();

                B_Guardar.Text = "Modificar";
                B_Guardar.Visible = true;
                btn_nuevo_lote.Visible = false;

                t_lote.Enabled = false;
                t_vencimiento.Enabled = true;
                t_envasado.Enabled = true;

            }

            if (e.CommandName == "b_borrar_lote")
            {
                string invtid = G_LOTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString().Trim();
                string siteid = G_LOTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString().Trim();
                string batnbr = G_LOTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString().Trim();
                string refnbr = G_LOTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString().Trim();
                string trandate = G_LOTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[4].ToString().Trim();
                string trantype = G_LOTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString().Trim();
                string lote = G_LOTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[6].ToString().Trim();

                string ok = ReporteRNegocio.delete_lote(invtid, siteid, batnbr, refnbr, trandate, trantype, lote);


                if (ok == "OK")
                {


                    DataTable dt1 = ReporteRNegocio.trae_lotes_(invtid, siteid, batnbr, refnbr, trandate, trantype);

                    G_LOTES.DataSource = dt1;
                    G_LOTES.DataBind();

                    btn_nuevo_lote.Visible = true;
                    B_Guardar.Visible = false;
                    t_envasado.Text = string.Empty;
                    t_vencimiento.Text = string.Empty;
                    t_lote.Text = string.Empty;
                    t_envasado.Enabled = false;
                    t_vencimiento.Enabled = false;
                    t_lote.Enabled = false;

                    modal_updatepanel.Update();
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "id_variable_123", "<script language='javascript'> alert('Eliminado') </script>", false);


                    DataTable dt_fila = ReporteRNegocio.fechas_y_dias_vencimiento(invtid, siteid, batnbr, refnbr, trandate, trantype);
                    DataTable dt_final = (DataTable)Session["dt_final_g_grid"];

                    foreach (DataRow r in dt_final.Rows)
                    {
                        if (r["invtid"].ToString().Trim() == invtid && r["siteid"].ToString().Trim() == siteid && r["batnbr"].ToString().Trim() == batnbr && r["refnbr"].ToString().Trim() == refnbr && r["FECHA_"].ToString().Trim() == trandate && r["trantype"].ToString().Trim() == trantype)
                        {
                            r["vencimiento"] = dt_fila.Rows[0]["vencimiento"];
                            r["envasado"] = dt_fila.Rows[0]["envasado"];
                            r["dif_envasado_vencimiento"] = dt_fila.Rows[0]["dif_envasado_vencimiento"];
                            r["dif_envasado_hoy"] = dt_fila.Rows[0]["dif_envasado_hoy"];
                            r["vida_util"] = dt_fila.Rows[0]["vida_util"];
                            r["porc_vida_util"] = dt_fila.Rows[0]["porc_vida_util"];
                            r["certificado"] = dt_fila.Rows[0]["certificado"];



                            break;
                        }
                    }

                    G_GRID.DataSource = dt_final;
                    G_GRID.DataBind();
                    UpdatePanel2.Update();

                }
                else
                {

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "id_variable_1233", "<script language='javascript'> alert('NO eliminado') </script>", false);

                }


            }






        }

        protected void btn_nuevo_lote_ServerClick(object sender, EventArgs e)
        {
            B_Guardar.Text = "Crear";
            B_Guardar.Visible = true;

            t_lote.Text = String.Empty;


            t_envasado.Enabled = true;
            t_vencimiento.Enabled = true;
            t_lote.Enabled = true;

            btn_nuevo_lote.Visible = false;
        }

        protected void B_Guardar_Click(object sender, EventArgs e)
        {
            //GUARDAR_LOTE

            string invtid = Session["invtid"].ToString();
            string siteid = Session["siteid"].ToString();
            string batnbr = Session["batnbr"].ToString();
            string refnbr = Session["refnbr"].ToString();
            string trandate = Session["trandate"].ToString();
            string trantype = Session["trantype"].ToString();
            string ok = "";
            if (B_Guardar.Text == "Crear")
            {
                ok = ReporteRNegocio.insert_lote(invtid, siteid, batnbr, refnbr, trandate, trantype, t_lote.Text, t_envasado.Text, t_vencimiento.Text);

            }
            else
            {


                ok = ReporteRNegocio.update_lote(invtid, siteid, batnbr, refnbr, trandate, trantype, t_lote.Text, t_envasado.Text, t_vencimiento.Text);


            }

            if (ok == "OK")
            {

                DataTable dt1 = ReporteRNegocio.trae_lotes_(invtid, siteid, batnbr, refnbr, trandate, trantype);

                G_LOTES.DataSource = dt1;
                G_LOTES.DataBind();


                btn_nuevo_lote.Visible = true;
                B_Guardar.Visible = false;
                t_envasado.Text = string.Empty;
                t_vencimiento.Text = string.Empty;
                t_lote.Text = string.Empty;
                t_envasado.Enabled = false;
                t_vencimiento.Enabled = false;
                t_lote.Enabled = false;

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "id_variable_12344", "<script language='javascript'> alert('Guardado') </script>", false);


                DataTable dt_fila = ReporteRNegocio.fechas_y_dias_vencimiento(invtid, siteid, batnbr, refnbr, trandate, trantype);
                DataTable dt_final = (DataTable)Session["dt_final_g_grid"];

                foreach (DataRow r in dt_final.Rows)
                {
                    if (r["invtid"].ToString().Trim() == invtid && r["siteid"].ToString().Trim() == siteid && r["batnbr"].ToString().Trim() == batnbr && r["refnbr"].ToString().Trim() == refnbr && r["FECHA_"].ToString().Trim() == trandate && r["trantype"].ToString().Trim() == trantype)
                    {
                        r["vencimiento"] = dt_fila.Rows[0]["vencimiento"];
                        r["envasado"] = dt_fila.Rows[0]["envasado"];
                        r["dif_envasado_vencimiento"] = dt_fila.Rows[0]["dif_envasado_vencimiento"];
                        r["dif_envasado_hoy"] = dt_fila.Rows[0]["dif_envasado_hoy"];
                        r["vida_util"] = dt_fila.Rows[0]["vida_util"];
                        r["porc_vida_util"] = dt_fila.Rows[0]["porc_vida_util"];
                        r["certificado"] = dt_fila.Rows[0]["certificado"];



                        break;
                    }
                }

                G_GRID.DataSource = dt_final;
                G_GRID.DataBind();
                UpdatePanel2.Update();

            }
            else
            {

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "id_variable_12335", "<script language='javascript'> alert('NO Guardado') </script>", false);

            }

        }
    }
}