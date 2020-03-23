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
using System.Net.Mail;
using System.Net;
using System.Diagnostics;
//using Microsoft.Office.Interop.Word;

namespace SoprodiApp
{
    public partial class REPORTE_SALDOS_SP : System.Web.UI.Page
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

                string sw_proviene = Request.QueryString["G"];

                if (sw_proviene != "NO")
                {

                    Session["SW_PERMI"] = "1";

                    if (sw_proviene == "912")
                    {
                        string datos = Request.QueryString["C"];

                        string rutcliente = datos.Split('*')[0].ToString();
                        string vendedor = datos.Split('*')[1].ToString();



                        Session["SW_FILTRAR_PRODUCTO"] = "SI";
                        Session["codvendedor"] = "";
                        Session["WHERE"] = " where 1=1 ";



                        ///PRODUCTOS DEVUELVE DT ... 
                        //string codigos_documentos = ReporteRNegocio.trae_codigos_sincronizados_por_producto(cod_prod);

                        Session["WHERE"] += " and b.rut = '" + rutcliente + "' and b.codvendedor = '" + vendedor + "' and b.CodEstadoDocumento in ('20', '10')";
                        Session["estados_param"] = "'10S', '10', '10P'";

                        ImageClickEventArgs e2x = new ImageClickEventArgs(1, 2);
                        Button1_Click(e, e2x);

                        //Session["SW_FILTRAR_PRODUCTO"] = "NO";

                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "teadsaeee", "<script>  var elem3 = document.getElementById('div_superior'); elem3.style.display = 'none'; </script>", false);




                    }
                    else
                    {
                        string datos = Request.QueryString["C"];

                        string cod_prod = datos.Split('*')[0].ToString();
                        string desde = datos.Split('*')[1].ToString();
                        string hasta = datos.Split('*')[2].ToString();
                        string bodegas = datos.Split('*')[3].ToString();

                        Session["SW_FILTRAR_PRODUCTO"] = "SI";
                        Session["codvendedor"] = "";
                        Session["WHERE"] = " where 1=1 ";



                        ///PRODUCTOS DEVUELVE DT ... 
                        //string codigos_documentos = ReporteRNegocio.trae_codigos_sincronizados_por_producto(cod_prod);

                        Session["WHERE"] += " and a.codproducto in ('" + cod_prod + "') and b.CodEstadoDocumento in ('20', '10')";
                        Session["estados_param"] = "'10S', '10', '10P'";

                        ImageClickEventArgs e2x = new ImageClickEventArgs(1, 2);
                        Button1_Click(e, e2x);

                        //Session["SW_FILTRAR_PRODUCTO"] = "NO";

                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "teadsaeee", "<script>  var elem3 = document.getElementById('div_superior'); elem3.style.display = 'none'; </script>", false);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teadsaeee", "<script>  var elem3 = document.getElementById('div_superior'); elem3.style.display = 'block'; </script>", false);
                    Session["SW_FILTRAR_PRODUCTO"] = "NO";

                }





                //TX_AÑO.Text = DateTime.Now.Year.ToString();
                //CB_TIPO_DOC_GRILLA.SelectedValue = DateTime.Now.Month.ToString();
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "37" || u_ne.Trim() == "45")
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


                DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now;
                //////t = new DateTime(t.Year, t.Month - 6, 1);               
                txt_desde.Text = "01" + t.AddMonths(-1).ToShortDateString().Substring(2);
                txt_hasta.Text = t2.ToShortDateString();

                string es_vendedor = ReporteRNegocio.esvendedor(USER);
                if (es_vendedor == "2")
                {
                    btn_excel.Visible = false;
                    btn_excel2.Visible = false;
                    btn_eliminar_check.Visible = false;
                }
                else
                {
                    btn_eliminar_check.Visible = true; ;
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


                if (Session["estado_cerrada"].ToString().Contains((e.Row.Cells[21].Text.Trim())) || Session["estado_cerrada"].ToString() == "")
                {



                    if (Session["sp_anterior"].ToString() == "")
                    {

                        e.Row.Attributes["class"] = Session["color"].ToString();
                        Session["sp_anterior"] = e.Row.Cells[0].Text.Trim();
                    }
                    else
                    {

                        //Session["sp_anterior"] = e.Row.Cells[0].Text;

                        if (e.Row.Cells[0].Text.Trim() == "219280")
                        {
                            string sd = "";
                        }
                        if (e.Row.Cells[0].Text.Trim() == Session["sp_anterior"].ToString())
                        {
                            e.Row.Attributes["class"] = Session["color"].ToString();
                        }
                        else
                        {
                            if (Session["color"].ToString() == "estado10P")
                            {
                                Session["color"] = "estado40";
                            }
                            else
                            {
                                Session["color"] = "estado10P";
                            }

                            Session["sp_anterior"] = e.Row.Cells[0].Text.Trim();
                            e.Row.Attributes["class"] = Session["color"].ToString();
                        }



                    }
                    //





                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                    string script1 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[0].Text), encriptador.EncryptData(""), encriptador.EncryptData(""), encriptador.EncryptData("57"));
                    e.Row.Cells[0].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[0].Text + " </a>";

                }
                else
                {
                    e.Row.Visible = false;


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
            string es_vend = ReporteRNegocio.esvendedor(User.Identity.Name.ToString());
            if (es_vend != "2")
            {
                Session["codvendedor"] = "";
            }
            else
            {

                Session["codvendedor"] = " and b.codvendedor = '" + User.Identity.Name.ToString() + "'";
            }


            string grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

            if (grupos_del_usuario == "")
            {
                grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
            }

            List<string> grupos = grupos_del_usuario.Split(',').ToList();


            string where3 = " ";

            if (grupos.Count == 4)
            {


            }
            else
            {

                if (grupos_del_usuario.ToUpper().Replace("'", "").Trim() == "GRANOS")
                {
                    where3 += " and DescEmisor = 'Granos' and 1=1  ";
                }
                else if (grupos_del_usuario.Contains("Abarrotes") || grupos_del_usuario.Contains("CMQuillota") || grupos_del_usuario.Contains("MayoristasLV"))
                {

                    where3 += " and DescEmisor <> 'Granos' and 1 = 1 ";
                }

            }



            carga_estado_2();
            cargar_clientes_SP(where3);
            cargar_estado_SP();
            //cargar_grupo_sp();
            cargar_vendedor_SP(where3);


            lb_bodegas2.Text = "10, 10S";

            foreach (ListItem item in d_bodega_2.Items)
            {

                if (lb_bodegas2.Text.Contains(item.Value.ToString()))
                {
                    item.Selected = true;
                }
            }
            foreach (ListItem item in d_estado_2.Items)
            {

                if (lb_bodegas3.Text.Contains(item.Value.ToString()))
                {
                    item.Selected = true;
                }
            }


        }

        private void carga_estado_2()
        {
            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.crear_estado_2();
            dtv = dt.DefaultView;
            d_estado_2.DataSource = dtv;
            d_estado_2.DataTextField = "estado";
            d_estado_2.DataValueField = "estado";
            //d_vendedor_.SelectedIndex = -1;
            d_estado_2.DataBind();
        }

        private void cargar_vendedor_SP(string where)
        {
            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.VM_listaVendedor(Session["codvendedor"].ToString(), where);
            dtv = dt.DefaultView;
            d_vendedor.DataSource = dtv;
            d_vendedor.DataTextField = "nombrevendedor";
            d_vendedor.DataValueField = "codvendedor";
            //d_vendedor_.SelectedIndex = -1;
            d_vendedor.DataBind();
        }

        private void cargar_grupo_sp()
        {
            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.VM_lista_grupo("");
            dtv = dt.DefaultView;
            d_grupo.DataSource = dtv;
            d_grupo.DataTextField = "DescEmisor";
            d_grupo.DataValueField = "DescEmisor";
            //d_vendedor_.SelectedIndex = -1;
            d_grupo.DataBind();
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
        private void cargar_estado_SP()
        {

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.VM_estados("");
            dtv = dt.DefaultView;
            d_bodega_2.DataSource = dtv;
            d_bodega_2.DataTextField = "descestadodocumento";
            d_bodega_2.DataValueField = "codestadodocumento";
            //d_vendedor_.SelectedIndex = -1;
            d_bodega_2.DataBind();
        }
        private void cargar_clientes_SP(string where)
        {

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.VM_clientes(" where 1=1 " + Session["codvendedor"], where);
            dtv = dt.DefaultView;
            d_bodega.DataSource = dtv;
            d_bodega.DataTextField = "nombre";
            d_bodega.DataValueField = "rut";
            //d_vendedor_.SelectedIndex = -1;
            d_bodega.DataBind();
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


            //REPORTE
            //string produc = agregra_comillas(l_vendedores.Text);
            string desde = txt_desde.Text;
            string hasta = txt_hasta.Text;

            string clientes = agregra_comillas(l_clientes.Text);
            string estados = agregra_comillas(lb_bodegas2.Text);


            string cerrado = agregra_comillas(lb_bodegas3.Text);
            Session["estado_cerrada"] = cerrado;

            string grupo = agregra_comillas(l_grupo_vm.Text);
            string vendedor = agregra_comillas(l_vendedor_vm.Text);


            string grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

            if (grupos_del_usuario == "")
            {
                grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
            }

            List<string> grupos = grupos_del_usuario.Split(',').ToList();


            string where3 = "";

            if (grupos.Count == 4)
            {


            }
            else
            {

                if (grupos_del_usuario.ToUpper().Replace("'", "").Trim() == "GRANOS")
                {
                    where3 += " and DescEmisor = 'Granos' ";
                }
                else if (grupos_del_usuario.Contains("Abarrotes") || grupos_del_usuario.Contains("CMQuillota") || grupos_del_usuario.Contains("MayoristasLV"))
                {

                    where3 += " and DescEmisor <> 'Granos' ";
                }

            }
            //select * from[VPEDIDOCABECERA] where FechaEmision >= CONVERT(datetime, '21/07/2017', 103)
            if (desde != "")
            {
                where3 += " and convert(datetime,FechaEmision ,103)  >= convert(datetime, '" + desde + "', 103) ";
            }
            if (hasta != "")
            {
                where3 += " and convert(datetime,FechaEmision ,103)   <= convert(datetime, '" + hasta + "', 103) ";
            }
            if (clientes != "")
            {

                where3 += " and rut in (" + clientes + ")";

            }

            if (vendedor != "")
            {

                where3 += " and codvendedor in (" + vendedor + ")";

            }
            if (Session["codvendedor"].ToString() != "")
            {

                where3 += Session["codvendedor"].ToString();

            }


            if (txt_sp.Text != "")
            {

                where3 += " and CodDocumento in (" + agregra_comillas(txt_sp.Text) + ")";

            }



            div_report.Visible = true;
            //SPSP
            DataTable dt2 = ReporteRNegocio.VM_saldos_sp(where3);

            sub_dolar = 0;
            Session["sp_anterior"] = "";
            Session["color"] = "estado10P";
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
                    else
                    {

                        ok_insert += "Error";
                    }


                }
            }


            Button1_Click(sender, e);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  var elem3 = document.getElementById(\"<%=carga_2.ClientID%>\"); elem3.style.display = \"none\";</script>", false);



        }

        protected void G_INFORME_TOTAL_VENDEDOR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Eliminar")
                {


                    string CodDocumento = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());

                    string update_ok = ReporteRNegocio.VM_updateSP(CodDocumento, 30);
                    if (update_ok == "OK")
                    {
                        string FechaEmision = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                        string CodVendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString());
                        string NotaLibre = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString());
                        string CodBodega = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[4].ToString());
                        string FechaDespacho = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString());
                        string CodMoneda = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[6].ToString());
                        string MontoNeto = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[7].ToString());
                        string DescEstadoDocumento = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[8].ToString());
                        string Facturas = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[9].ToString());
                        string GxEstadoSync = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[10].ToString());
                        string GxActualizado = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[11].ToString());
                        string GxEnviadoERP = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[12].ToString());
                        string NombreVendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[13].ToString());
                        string NombreCliente = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[14].ToString());
                        string DescBodega = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[15].ToString());
                        string FechaCreacion = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[16].ToString());
                        string ValorTipoCambio = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[17].ToString());
                        string LimiteSeguro = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[18].ToString());
                        string TipoCredito = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[19].ToString());
                        string CreditoDisponible = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[20].ToString());
                        string CreditoAutorizado = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[21].ToString());
                        string EmailVendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[22].ToString());


                        enviar_email_cambio(CodDocumento, FechaEmision, CodVendedor, NotaLibre, CodBodega, FechaDespacho, CodMoneda, MontoNeto, DescEstadoDocumento, Facturas, GxEstadoSync, GxActualizado, GxEnviadoERP, NombreVendedor, NombreCliente, DescBodega, FechaCreacion, ValorTipoCambio, LimiteSeguro, TipoCredito, CreditoDisponible, CreditoAutorizado, EmailVendedor);

                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  document.getElementById('ContentPlaceHolder_Contenido_Button1').click();  </script>", false);

                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'> alert('Rechazado') </script>", false);


                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'> alert('Algo ocurrió :(') </script>", false);


                    }

                }
                if (e.CommandName == "Enviar")
                {

                    string CodDocumento = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    string FechaEmision = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                    string CodVendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString());
                    string NotaLibre = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString());
                    string CodBodega = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[4].ToString());
                    string FechaDespacho = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString());
                    string CodMoneda = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[6].ToString());
                    string MontoNeto = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[7].ToString());
                    string DescEstadoDocumento = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[8].ToString());
                    string Facturas = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[9].ToString());
                    string GxEstadoSync = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[10].ToString());
                    string GxActualizado = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[11].ToString());
                    string GxEnviadoERP = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[12].ToString());
                    string NombreVendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[13].ToString());
                    string NombreCliente = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[14].ToString());
                    string DescBodega = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[15].ToString());
                    string FechaCreacion = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[16].ToString());
                    string ValorTipoCambio = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[17].ToString());
                    string LimiteSeguro = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[18].ToString());
                    string TipoCredito = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[19].ToString());
                    string CreditoDisponible = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[20].ToString());
                    string CreditoAutorizado = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[21].ToString());
                    string EmailVendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[22].ToString());

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  document.getElementById('ContentPlaceHolder_Contenido_Button1').click();  </script>", false);

                    Session["codigo_documento"] = CodDocumento;
                    Session["tabla_html"] = generar_tabla_con_sp(CodDocumento, FechaEmision, CodVendedor, NotaLibre, CodBodega, FechaDespacho, CodMoneda, MontoNeto, DescEstadoDocumento, Facturas, GxEstadoSync, GxActualizado, GxEnviadoERP, NombreVendedor, NombreCliente, DescBodega, FechaCreacion, ValorTipoCambio, LimiteSeguro, TipoCredito, CreditoDisponible, CreditoAutorizado, EmailVendedor);

                }



            }
            catch (Exception ex)
            {

            }
        }

        private void AbrirArchivo(string Path)
        {
            Process P = new Process();
            try
            {
                P.StartInfo.FileName = Path;
                P.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                P.Start();
                //Espera el proceso para que lo termine y continuar
                P.WaitForExit();
                //Liberar
                P.Close();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>alert('" + ex.Message + " No se puede abrir el documento " + Path + "');</script>", false);

                //MessageBox.Show(ex.Message + " No se puede abrir el documento " + Path, "Error");
            }
        }


        private void enviar_email_cambio(string CodDocumento, string FechaEmision, string CodVendedor, string NotaLibre, string CodBodega, string FechaDespacho, string CodMoneda, string MontoNeto, string DescEstadoDocumento, string Facturas, string GxEstadoSync, string GxActualizado, string GxEnviadoERP, string NombreVendedor, string NombreCliente, string DescBodega, string FechaCreacion, string ValorTipoCambio, string LimiteSeguro, string TipoCredito, string CreditoDisponible, string CreditoAutorizado, string EmailVendedor)
        {


            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress("rmc@soprodi.cl"));
            //email.To.Add(new MailAddress("egodoy@soprodi.cl"));

            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "SP Rechazada desde Sistema ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

            email.CC.Add(EmailVendedor + " , mazocar@soprodi.cl, jcorrea@soprodi.cl, gmorales@soprodi.cl, egodoy@soprodi.cl");

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

            email.Body += "<div> Estimado :<br> <br>  <b> </b> <br><br>";
            email.Body += "<div> Se ha RECHAZADO la siguiente SP:  <br><br>";
            email.Body += generar_tabla_con_sp(CodDocumento, FechaEmision, CodVendedor, NotaLibre, CodBodega, FechaDespacho, CodMoneda, MontoNeto, DescEstadoDocumento, Facturas, GxEstadoSync, GxActualizado, GxEnviadoERP, NombreVendedor, NombreCliente, DescBodega, FechaCreacion, ValorTipoCambio, LimiteSeguro, TipoCredito, CreditoDisponible, CreditoAutorizado, EmailVendedor);
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
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "srv-correo-2.soprodi.cl";
            smtp.Port = 25;
            smtp.EnableSsl = false;


            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            try
            {
                smtp.Send(email);
                email.Dispose();

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>alert('CORREO ENVIADO!');</script>", false);

            }
            catch (Exception ex)
            {
                //lb_mensaj.Text = "Error al enviar ";
            }

        }

        private string generar_tabla_con_sp(string codDocumento, string fechaEmision, string codVendedor,
            string notaLibre, string codBodega, string fechaDespacho, string codMoneda, string montoNeto,
            string descEstadoDocumento, string facturas, string gxEstadoSync, string gxActualizado, string gxEnviadoERP,
            string nombreVendedor, string nombreCliente, string descBodega, string fechaCreacion, string valorTipoCambio,
            string limiteSeguro, string tipoCredito, string creditoDisponible, string creditoAutorizado, string emailVendedor)
        {
            string tabla = "";


            tabla += "<table border=1 id='TABLA_2' border class='table table-advance table-bordered fill-head tablesorter filtrar' style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";
            tabla += "<thead class=\"test\">";
            tabla += "<tr style='background-color:#428bca'>";

            tabla += "<th>codDocumento</th>";
            tabla += "<th>fechaEmision</th>";
            tabla += "<th>codVendedor</th>";
            tabla += "<th>notaLibre</th>";
            tabla += "<th>codBodega</th>";
            tabla += "<th>fechaDespacho</th>";
            tabla += "<th>codMoneda</th>";
            tabla += "<th>montoNeto</th>";
            tabla += "<th>descEstadoDocumento</th>";
            tabla += "<th>facturas</th>";
            tabla += "<th>gxEstadoSync</th>";
            tabla += "<th>gxActualizado</th>";
            tabla += "<th>gxEnviadoERP</th>";
            tabla += "<th>nombreVendedor</th>";
            tabla += "<th>nombreCliente</th>";
            tabla += "<th>descBodega</th>";
            tabla += "<th>fechaCreacion</th>";
            tabla += "<th>valorTipoCambio</th>";
            tabla += "<th>limiteSeguro</th>";
            tabla += "<th>tipoCredito</th>";
            tabla += "<th>creditoDisponible</th>";
            tabla += "<th>creditoAutorizado</th>";
            tabla += "<th>emailVendedor</th>";


            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";

            tabla += "<tr>";

            tabla += "<td>" + codDocumento + "</td>";
            tabla += "<td>" + fechaEmision + "</td>";
            tabla += "<td>" + codVendedor + "</td>";
            tabla += "<td>" + notaLibre + "</td>";
            tabla += "<td>" + codBodega + "</td>";
            tabla += "<td>" + fechaDespacho + "</td>";
            tabla += "<td>" + codMoneda + "</td>";
            tabla += "<td>" + montoNeto + "</td>";
            tabla += "<td>" + descEstadoDocumento + "</td>";
            tabla += "<td>" + facturas + "</td>";
            tabla += "<td>" + gxEstadoSync + "</td>";
            tabla += "<td>" + gxActualizado + "</td>";
            tabla += "<td>" + gxEnviadoERP + "</td>";
            tabla += "<td>" + nombreVendedor + "</td>";
            tabla += "<td>" + nombreCliente + "</td>";
            tabla += "<td>" + descBodega + "</td>";
            tabla += "<td>" + fechaCreacion + "</td>";
            tabla += "<td>" + valorTipoCambio + "</td>";
            tabla += "<td>" + limiteSeguro + "</td>";
            tabla += "<td>" + tipoCredito + "</td>";
            tabla += "<td>" + creditoDisponible + "</td>";
            tabla += "<td>" + creditoAutorizado + "</td>";
            tabla += "<td>" + emailVendedor + "</td>";


            tabla += "</tr>";


            tabla += "</tbody>";
            tabla += "</table>";






            return tabla;
        }

        public string confirmDelete(string Name)
        {
            return @"javascript:if(!confirm('Esta acción va modificar el estado del documento: "
               + Name.ToUpper()
               + @". ¿Estás seguro?')){return false;} ; CARGANDO();";
        }
        public string abrir_modal(string Name)
        {
            return @"javascript: modal_unidad_1(" + Name + ");";
        }


        [WebMethod]
        public static string ENVIAR_CORREO(string PARA, string TEXT)
        {


            string respt = "";
            try
            {

            }
            catch { respt = "Error :02 Al guardar Producto"; }
            //}

            return respt;
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {

            string para = tx_para.Text;
            string obs = tx_text_.Value;


            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(para));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "Datos SP : " + Session["codigo_documento"].ToString() + " ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

            //email.CC.Add();

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

            email.Body += "<div> Estimado :<br> <br>  <b> </b> <br><br>";
            email.Body += "<div> Datos de la siguiente SP:  <br><br>";
            email.Body += Session["tabla_html"].ToString();
            email.Body += "<br> <b> OBS:  </b> <br> " + obs;

            email.Body += "</br><br><br> Para más detalles diríjase a:  <a href='http://srv-app.soprodi.cl' > srv-app.soprodi.cl  </a> <span style='font:5px; color:gray; float:right;'>No Responder Este Correo</span> <br><br>";
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

            try
            {
                //Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
                //Microsoft.Office.Interop.Word.Document doc = word.Documents.Open("\\\\192.168.10.22\\Archivos\\" + Session["codigo_documento"].ToString().Trim() + ".doc");
                //doc.Activate();
                //doc.SaveAs2("C:\\DOCUMENTOS_APP_SOPRODI\\SP\\" + Session["codigo_documento"].ToString().Trim() +".pdf", Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
                //doc.Close();
                System.Net.Mail.Attachment adj = new System.Net.Mail.Attachment("D:\\bk_thx_tebo\\BK_thx_ventamovil\\BK_thx_ventamovil\\Archivos\\" + Session["codigo_documento"].ToString().Trim() + ".doc");
                adj.Name = Session["codigo_documento"].ToString().Trim() + ".doc";
                email.Attachments.Add(adj);
            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "man3t_qqqAqN2", "<script>alert('Error: ! " + ex.Message.ToString() + "');</script>", false);

            }
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
                smtp.Send(email);
                email.Dispose();

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>alert('CORREO ENVIADO!');</script>", false);

            }
            catch (Exception ex)
            {
                //lb_mensaj.Text = "Error al enviar ";
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>alert('Error: '" + ex.Message + ");</script>", false);

            }


        }

        protected void btn_eliminar_check_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow dtgItem in this.G_INFORME_TOTAL_VENDEDOR.Rows)
            {
                CheckBox Sel = ((CheckBox)G_INFORME_TOTAL_VENDEDOR.Rows[dtgItem.RowIndex].FindControl("chkAccept"));
                bool valor = Sel.Checked;
                if (valor)
                {


                    string CodDocumento = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[0].ToString());
                    string DescEstadoDocumento1 = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[8].ToString());
                    string Facturas1 = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[9].ToString());


                    if (DescEstadoDocumento1.Trim() == "Sincronizado" || (DescEstadoDocumento1.Trim() == "Aprobado" && Facturas1 == ""))
                    {

                        string update_ok = ReporteRNegocio.VM_updateSP(CodDocumento, 30);
                        //string update_ok = "OK";
                        if (update_ok == "OK")
                        {
                            string FechaEmision = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[1].ToString());
                            string CodVendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[2].ToString());
                            string NotaLibre = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[3].ToString());
                            string CodBodega = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[4].ToString());
                            string FechaDespacho = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[5].ToString());
                            string CodMoneda = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[6].ToString());
                            string MontoNeto = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[7].ToString());
                            string DescEstadoDocumento = DescEstadoDocumento1;
                            string Facturas = Facturas1;
                            string GxEstadoSync = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[10].ToString());
                            string GxActualizado = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[11].ToString());
                            string GxEnviadoERP = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[12].ToString());
                            string NombreVendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[13].ToString());
                            string NombreCliente = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[14].ToString());
                            string DescBodega = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[15].ToString());
                            string FechaCreacion = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[16].ToString());
                            string ValorTipoCambio = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[17].ToString());
                            string LimiteSeguro = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[18].ToString());
                            string TipoCredito = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[19].ToString());
                            string CreditoDisponible = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[20].ToString());
                            string CreditoAutorizado = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[21].ToString());
                            string EmailVendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[22].ToString());


                            enviar_email_cambio(CodDocumento, FechaEmision, CodVendedor, NotaLibre, CodBodega, FechaDespacho, CodMoneda, MontoNeto, DescEstadoDocumento, Facturas, GxEstadoSync, GxActualizado, GxEnviadoERP, NombreVendedor, NombreCliente, DescBodega, FechaCreacion, ValorTipoCambio, LimiteSeguro, TipoCredito, CreditoDisponible, CreditoAutorizado, EmailVendedor);


                            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'> alert('Rechazado') </script>", false);


                        }
                    }
                }
                else
                {
                }
            }
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  document.getElementById('ContentPlaceHolder_Contenido_Button1').click();  </script>", false);

        }
    }
}