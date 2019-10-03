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
    public partial class REPORTE_COBRO_BODEGA : System.Web.UI.Page
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

                TX_AÑO.Text = DateTime.Now.Year.ToString();
                CB_TIPO_DOC_GRILLA.SelectedValue = DateTime.Now.Month.ToString();
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "45")
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

                titulo2.HRef = "menu.aspx";
                titulo2.InnerText = "Sobre Estadia";

                USER = User.Identity.Name.ToString();
                l_usuario_.Text = USER;
                DateTime t = DateTime.Now.AddDays(-1);
                DateTime t2 = t;
                //////t = new DateTime(t.Year, t.Month - 6, 1);               
                //txt_desde.Text = t.ToShortDateString();
                txt_hasta.Text = t2.ToShortDateString().Substring(0, 2);

                tx_dias_sobre.Text = "90";

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
        public static  string guardar_solo_kg(string sw, string tipo, string valor)
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
                //sorter - false

                if (e.Row.Cells[4].Text.Contains("TOTAL"))
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Attributes["style"] = "background-color: #aaffaa";
                    e.Row.CssClass = "no-sort";

                    e.Row.Cells[0].Attributes["style"] = "background-color: white";
                    e.Row.Cells[1].Attributes["style"] = "background-color: white";
                }

                try
                {
                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                    string año_mes = CB_TIPO_DOC_GRILLA.SelectedValue + "*" + TX_AÑO.Text + "*" + txt_hasta.Text + "*" + tx_valor_mensual.Text + "*" + tx_dolar.Text + "*" + tx_dias_sobre.Text;
                    string script = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;);return false;", encriptador.EncryptData(año_mes), encriptador.EncryptData(e.Row.Cells[0].Text), encriptador.EncryptData(lb_bodegas2.Text), encriptador.EncryptData("13"));
                    e.Row.Cells[0].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[0].Text + " </a>";
                }
                catch { }

                try
                {

                    e.Row.Cells[1].Text = e.Row.Cells[1].Text.Replace(".", ",");

                    e.Row.Cells[1].Text = e.Row.Cells[1].Text.Substring(0, e.Row.Cells[1].Text.IndexOf(",") + 3);

                    e.Row.Cells[1].Text = e.Row.Cells[1].Text.Replace(",00", "");

                    e.Row.Cells[1].Text = Base.monto_format(e.Row.Cells[1].Text.Trim());


                    //e.Row.Cells[3].Text = e.Row.Cells[3].Text.Substring(0, e.Row.Cells[3].Text.IndexOf(",") + 3);


                    e.Row.Cells[3].Text = e.Row.Cells[3].Text.Replace(".", ",");

                    e.Row.Cells[3].Text = e.Row.Cells[3].Text.Replace(".00", "");

                    e.Row.Cells[3].Text = Base.monto_format(e.Row.Cells[3].Text.Trim());

                    if (e.Row.Cells[5].Text.Substring(0, 1).Contains('-'))
                    {
                        e.Row.Cells[5].Text = "0";
                    }


                    e.Row.Cells[3].Text = Convert.ToDouble(e.Row.Cells[3].Text.Substring(0, e.Row.Cells[3].Text.IndexOf(","))).ToString("N0");


                    //try
                    //{
                    //    if (Convert.ToDouble(e.Row.Cells[5].Text) > 0)
                    //    {


                    //    }
                }
                catch { }



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


            string HTML_EXCEL = "<label> TASA CAMBIO: " + tx_dolar.Text + " </label>    -   <label> " + CB_TIPO_DOC_GRILLA.SelectedItem.ToString() + "  </label>  <label> " + TX_AÑO.Text + " </label>";

            HTML_EXCEL += "";
            //HTML += "</div>";

            R_Excel_1.InnerHtml = HTML_EXCEL;

            R_Excel_1.RenderControl(htmlWrite);

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
            string clase = "";
            if (rd_humano.Checked)
            {

                clase = "  b.glclassid in ('ABAR', 'MANI') and b.invtid > '1000' and b.invtid <> '9918' ";
            }
            else {
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

            tx_valor_mensual.Text = "";
            tx_dolar.Text = "";
            DataTable dt = ReporteRNegocio.traer_usd_cobro(CB_TIPO_DOC_GRILLA.SelectedValue, TX_AÑO.Text);

            foreach (DataRow r in dt.Rows)
            {
                tx_dolar.Text = r[3].ToString();
                tx_valor_mensual.Text = r[4].ToString();

            }


            string desde_1 = "";
            string desde_2 = "";
            string where = "";
            string fecha_compra = "";

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
            G_INFORME_TOTAL_VENDEDOR.Visible = true;

            string clase = "";
            if (rd_humano.Checked)
            {

                where += " and  glclassid in ('ABAR', 'MANI') and invtid > '1000' and invtid <> '9918' ";
            }
            else {
                where += " and  glclassid <> 'ABAR' and glclassid <>  'MANI'  and invtid <> '9905'  and invtid <> '9999'  and invtid <> '9907'   ";

            }


            DataTable dt2_cobro = crear_cobro_(ReporteRNegocio.costo_sobrealmacenaje(where, CB_TIPO_DOC_GRILLA.SelectedValue, TX_AÑO.Text, where_bodega));





            DataTable dt2 = dt2_cobro;
            total_rows = dt2.Rows.Count;
            G_INFORME_TOTAL_VENDEDOR.DataSource = dt2;
            G_INFORME_TOTAL_VENDEDOR.DataBind();
            JQ_Datatable();


            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeees", "<script> CARGANDO_CLOSE(); </script>", false);



            if (rd_humano.Checked)
            {

                clase = "  b.glclassid in ('ABAR', 'MANI') and b.invtid > '1000' and b.invtid <> '9918' ";
            }
            else {
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

        private DataTable crear_cobro_(DataTable dt)
        {


            string condicion = " where 1=1 ";

            if (lb_bodegas2.Text != "")
            {

                condicion += " and siteid in (" + agregra_comillas(lb_bodegas2.Text) + ") ";

            }
            string clase = "";
            if (rd_humano.Checked)
            {

                condicion += " and  glclassid in ('ABAR', 'MANI') and invtid > '1000' and invtid <> '9918' ";
            }
            else {
                condicion += " and  glclassid <> 'ABAR' and glclassid <>  'MANI'  and invtid <> '9905'  and invtid <> '9999'  and invtid <> '9907'   ";

            }

            double v_me = 0;
            int an = 0;
            int me = 0;
            double daymon = 0;
            double valor_dia = 0;
            int dias_hasta = 0;


            try
            {

                v_me = Convert.ToDouble(tx_valor_mensual.Text);
                an = Convert.ToInt32(TX_AÑO.Text);
                me = Convert.ToInt32(CB_TIPO_DOC_GRILLA.SelectedValue.ToString());
                daymon = System.DateTime.DaysInMonth(an, me);
                valor_dia = v_me / daymon;
                dias_hasta = Convert.ToInt32(txt_hasta.Text.Substring(0, 2));

            }
            catch (Exception ex)

            {

                foreach (DataRow r in dt.Rows)
                {
                    string invtid = r[0].ToString().Split('-')[0].Trim();

                    DataTable arrastre_noventa_y_salidas = ReporteRNegocio.arrastre_noventa(condicion + " and invtid = '" + invtid + "' ", CB_TIPO_DOC_GRILLA.SelectedValue, TX_AÑO.Text, tx_dias_sobre.Text);

                    string fecha_inicial = "";

                    fecha_inicial = arrastre_noventa_y_salidas.Rows[0][0].ToString();

                    r[4] = fecha_inicial;
                }
                return dt;

            }

            double SUMA_TOTAL_COBRO = 0;
            double SUMA_TOTAL_KILOS = 0;
            //double SUMA_TOTAL_
            foreach (DataRow r in dt.Rows)
            {
                //double aux_var = 0;
                //try
                //{
                //    aux_var = Convert.ToDouble(r[3]);
                //}
                //catch { }

                //if (aux_var > 0)
                //{

                //DataTable dias_cobro = new DataTable();

                //dias_cobro.Columns.Add("Dia");
                //dias_cobro.Columns.Add("Entrada");
                //dias_cobro.Columns.Add("Salida");
                //dias_cobro.Columns.Add("Arrastre");
                //dias_cobro.Columns.Add("Cobro");

                string invtid = r[0].ToString().Split('-')[0].Trim();

                try
                {

                    DataTable arrastre_noventa_y_salidas = ReporteRNegocio.arrastre_noventa(condicion + " and invtid = '" + invtid + "' ", CB_TIPO_DOC_GRILLA.SelectedValue, TX_AÑO.Text, tx_dias_sobre.Text);

                    double arrastre_noven = 0;
                    string fecha_inicial = "";

                    arrastre_noven = Convert.ToDouble(arrastre_noventa_y_salidas.Rows[0][1]);
                    fecha_inicial = arrastre_noventa_y_salidas.Rows[0][0].ToString();

                    if (Convert.ToDouble(r[3].ToString().Replace(".", ",")) < 0)
                    {
                        r[1] = "0";
                        r[3] = "0";
                    }
                    else {
                        SUMA_TOTAL_KILOS += Convert.ToDouble(r[3].ToString().Replace(".", ","));
                    }
                    double arrastre = 0;
                    double SUMA_A_COBRAR = 0;
                    for (int i = 1; i <= dias_hasta; i++)
                    {
                        //row = dias_cobro.NewRow();
                        //row[0] = i;
                        DataTable estrada_y_salida = ReporteRNegocio.dia_entrada_noven(condicion + " and invtid = '" + invtid + "' ", CB_TIPO_DOC_GRILLA.SelectedValue, TX_AÑO.Text, i);
                        double entrada = Convert.ToDouble(estrada_y_salida.Rows[0][0]);
                        double salida = Convert.ToDouble(estrada_y_salida.Rows[0][1]);

                        //if (arrastre_noven < 0)
                        //{


                        //}


                        //row[1] = entrada.ToString("N0");
                        //row[2] = salida.ToString("N0");

                        arrastre = (arrastre_noven + entrada + salida);
                        //row[3] = arrastre.ToString("N0");
                        arrastre_noven = arrastre;


                        double kilos = arrastre;





                        double valor_for = (kilos * (valor_dia * Convert.ToDouble(tx_dolar.Text)) / 1000);

                        if (valor_for > 0)
                        {


                            SUMA_A_COBRAR += valor_for;
                        }

                        //row[4] = (kilos * (valor_dia * Convert.ToDouble(tx_dolar.Text)) / 1000).ToString("N0");
                        //dias_cobro.Rows.Add(row);

                    }


                    r[4] = fecha_inicial;
                    r[5] = SUMA_A_COBRAR.ToString("N0");
                    SUMA_TOTAL_COBRO += SUMA_A_COBRAR;

                }



                catch (Exception ex)
                {
                    Console.Write(ex.Message);

                }

            }

            DataView dv2 = dt.DefaultView;
            dv2.Sort = "producto ASC";
            dt = dv2.ToTable();


            DataRow row = dt.NewRow();


            row[0] = "";
            row[1] = 0;
            row[2] = "TONELADAS   :   "; ;
            row[3] = Base.monto_format2(SUMA_TOTAL_KILOS / 1000);
            row[4] = "TOTAL PESO CL :   ";
            row[5] = SUMA_TOTAL_COBRO.ToString("N0");
            dt.Rows.Add(row);

            DataRow row2 = dt.NewRow();

            row2[0] = "";
            row2[1] = 0;
            row2[2] = ""; ;
            row2[3] = "";
            row2[4] = "TOTAL USD:   ";
            row2[5] = (SUMA_TOTAL_COBRO / Convert.ToDouble(tx_dolar.Text)).ToString("N0");
            dt.Rows.Add(row2);

            return dt;
        }




        protected void carga_d_Click(object sender, EventArgs e)
        {


        }

        protected void Button3_Click(object sender, EventArgs e)
        {

            string oka = ReporteRNegocio.guardar_valor_equivale(
           solo_kg.Text,
            tx_tipo_.Text,
            tx_valor.Text
            );

            if (oka == "OK")
            {
                //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teassd1Q21mp", "<script language='javascript'>alert('Guardado!!')</script>", false);

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeees", "<script> alert('Guardado!!'); chosen_update();</script>", false);

            }
            else {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeees", "<script> alert('Error!!'); chosen_update(); </script>", false);

                //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd1Q2w21mp", "<script language='javascript'>alert('Error!!');chosen_update();</script>", false);
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {


            string okey = ReporteRNegocio.guardar_usd_cobro(CB_TIPO_DOC_GRILLA.SelectedValue, TX_AÑO.Text, tx_dolar.Text, tx_valor_mensual.Text);


            string desde_1 = "";
            string desde_2 = "";
            string where = "";
            string fecha_compra = "";

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
            G_INFORME_TOTAL_VENDEDOR.Visible = true;

            DataTable dt2_cobro = crear_cobro_(ReporteRNegocio.costo_sobrealmacenaje(where, CB_TIPO_DOC_GRILLA.SelectedValue, TX_AÑO.Text, where_bodega));
            DataTable dt2 = dt2_cobro;

            total_rows = dt2.Rows.Count;
            G_INFORME_TOTAL_VENDEDOR.DataSource = dt2;
            G_INFORME_TOTAL_VENDEDOR.DataBind();
            JQ_Datatable();
        }

        protected void G_errores_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void BTN_error_Click(object sender, EventArgs e)
        {
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
            DataTable td2 = ReporteRNegocio.fechas_incorrectas(CB_TIPO_DOC_GRILLA.SelectedValue, TX_AÑO.Text, where);
            G_errores.DataSource = td2;
            G_errores.DataBind();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "tee1ees", "<script> CLICK_MODAL_ERRORES(); </script>", false);


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
    }
}