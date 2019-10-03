﻿using System;
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
    public partial class REPORTE_INSTRUIR : System.Web.UI.Page
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

            JQ_Datatable();
            page = this.Page;
            if (!IsPostBack)
            {

                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "35" || u_ne.Trim() == "44")
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
                    titulo3.HRef = "Menu_Planificador.aspx";
                    titulo3.InnerText = "Planificador de Despachos";
                }
                else if (Session["SW_PERMI"].ToString() == "2")
                {
                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=2'>Granos</a>";
                    titulo2.HRef = "reportes.aspx?s=2";
                    titulo2.InnerText = "Granos";
                    titulo3.HRef = "Menu_Planificador.aspx";
                    titulo3.InnerText = "Planificador de Despachos";
                }

                else if (Session["SW_PERMI"].ToString() == "4")
                {
                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=2'>Granos</a>";
                    //titulo2.HRef = "reportes.aspx?s=2";
                    //titulo2.InnerText = "Granos";
                    titulo3.HRef = "Menu_Planificador.aspx?s=4";
                    titulo3.InnerText = "Planificador de Despachos";
                }


                USER = User.Identity.Name.ToString();
                l_usuario_.Text = USER;
                DateTime t = DateTime.Now.AddDays(-2);
                DateTime t2 = DateTime.Now.AddDays(+5);

                txt_desde.Text = t.ToShortDateString();
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
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teadsaeee", "<script> SortPrah(); </script>", false);

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
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd1Q21mp", "<script language='javascript'>creagrilla();</script>", false);
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
            ////dt.Rows.Add(new Object[] { -1, "-- Todos --" });
            //dtv = dt.DefaultView;
            //dtv.Sort = "nom_cliente";
            //d_bodega.DataSource = dtv;
            //d_bodega.DataTextField = "nom_cliente";
            //d_bodega.DataValueField = "rut_cliente";
            ////d_vendedor_.SelectedIndex = -1;
            //d_bodega.DataBind();
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


                if (G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values[3].ToString().Trim() == "1")
                {
                    e.Row.Attributes["class"] = "estado20";
                }
                else
                {
                    e.Row.Cells[1].Text = "";

                    e.Row.Attributes["class"] = "estado10";
                }

                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                string script = string.Format("javascript:CargarEvento_Tabla(&#39;{0}&#39;);", G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values[0].ToString().Trim());
                e.Row.Cells[2].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[2].Text + " </a>";

                if (ReporteRNegocio.esvendedor(User.Identity.Name) == "2")
                {

                    e.Row.Cells[0].Visible = false;
                    G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[0].Visible = false;
                    e.Row.Cells[1].Visible = false;
                    G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[1].Visible = false;



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

            tabla_instruir.RenderControl(htmlWrite);

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

                Session["codvendedor"] = " and codvendedor = '" + User.Identity.Name.ToString() + "'";
            }




            cargar_clientes_SP();
            cargar_bodega_trans();
            //cargar_estado_SP();
            //cargar_grupo_sp();
            //cargar_vendedor_SP();


            //lb_bodegas2.Text = "10, 10S, 10P";

            //foreach (ListItem item in d_bodega_2.Items)
            //{

            //    if (lb_bodegas2.Text.Contains(item.Value.ToString()))
            //    {
            //        item.Selected = true;
            //    }
            //}

            ImageClickEventArgs ex = new ImageClickEventArgs(1, 2);

            Button1_Click(sender, ex);



        }

        private void cargar_vendedor_SP()
        {
            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.VM_listaVendedor(Session["codvendedor"].ToString(), "");
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

        private void cargar_clientes_SP()
        {

            DataTable dt; DataView dtv = new DataView();
            string where1 = "";
            if (Session["SW_PERMI"].ToString() == "1")
            {
                where1 = " where grupo = 'ABARROTES' ";
            }
            else if (Session["SW_PERMI"].ToString() == "2")
            {
                where1 = " where grupo = 'GRANOS' ";
            }
            else
            {
                where1 = " where grupo in ( 'GRANOS' , 'ABARROTES' ) ";
            }

            dt = ReporteRNegocio.PLANI_CAMIONES(where1);
            dtv = dt.DefaultView;
            d_camion_.DataSource = dtv;
            d_camion_.DataTextField = "nombre_trans";
            d_camion_.DataValueField = "cod_trans";
            //d_vendedor_.SelectedIndex = -1;
            d_camion_.DataBind();
        }

        private void cargar_bodega_trans()
        {

            DataTable dt; DataView dtv = new DataView();
            string where1 = "";
            if (Session["SW_PERMI"].ToString() == "1")
            {
                where1 = " where grupo = 'ABARROTES' ";
            }
            else if (Session["SW_PERMI"].ToString() == "2")
            {
                where1 = " where grupo = 'GRANOS' ";
            }
            else
            {
                where1 = " where grupo in ( 'GRANOS' , 'ABARROTES' ) ";
            }

            dt = ReporteRNegocio.bodega_transportista(where1);
            dtv = dt.DefaultView;
            d_bodega.DataSource = dtv;
            d_bodega.DataTextField = "cod_bodega";
            d_bodega.DataValueField = "cod_bodega";
            d_bodega.DataBind();

            //d_vendedor_.SelectedIndex = -1;
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

            string grupo = agregra_comillas(l_grupo_vm.Text);
            string vendedor = agregra_comillas(l_vendedor_vm.Text);
            string bodega = agregra_comillas(lb_bodegas2.Text);


            div_report.Visible = true;

            string where1 = "";

            string es_vend = ReporteRNegocio.esvendedor(User.Identity.Name.ToString());
            if (es_vend != "2")
            {

            }
            else
            {
                where1 = " and codvendedor = '" + User.Identity.Name.ToString() + "'";
            }
            if (desde != "")
            {
                where1 += " and convert(datetime,fecha_despacho ,103)  >= convert(datetime, '" + desde + "', 103) ";
            }
            if (hasta != "")
            {
                where1 += " and convert(datetime,fecha_despacho ,103)   <= convert(datetime, '" + hasta + "', 103) ";
            }

            if (vendedor != "")
            {

                where1 += " and codvendedor IN (" + vendedor + ")";
            }

            if (clientes != "")
            {

                where1 += " and cod_trans IN (" + clientes + ")";
            }
            if (estados != "")
            {

                where1 += " and estado IN (" + estados + ")";
            }
            //if (bodega != "")
            //{
            //    where1 += " and estado IN (" + estados + ")";
            //}

            if (Session["SW_PERMI"].ToString() == "1")
            {
                where1 += " and ASIGNADO_POR = 'ABARROTES' ";
            }
            else if (Session["SW_PERMI"].ToString() == "2")
            {
                where1 += " and ASIGNADO_POR = 'GRANOS' ";
            }
            else
            {
                where1 += " and ASIGNADO_POR in ('GRANOS', 'ABARROTES') ";
            }

            if (bodega != "")
            {   
                where1 += " and codbodega IN (" + bodega + ")";
            }


            DataTable dt2 = ReporteRNegocio.listar_camion_dia(where1);
            DataView view = new DataView(dt2);
            view.Sort = "fecha_despacho asc";
            DataTable dias = view.ToTable(true, "fecha_despacho");

            //DataView view1 = new DataView(dt2);
            //view1.Sort = "cod_trans asc";
            //DataTable camiones = view1.ToTable(true, "cod_trans", "nombre_trans");

            string where2 = "";
            if (Session["SW_PERMI"].ToString() == "1")
            {
                where2 += " where ASIGNADO_POR = 'ABARROTES' ";
            }
            else if (Session["SW_PERMI"].ToString() == "2")
            {
                where2 += " where ASIGNADO_POR = 'GRANOS' ";
            }
            else
            {
                where2 += " where ASIGNADO_POR in ('GRANOS', 'ABARROTES') ";
            }

            if (bodega != "")
            {
                where1 += " and codbodega IN (" + bodega + ")";
            }

            if (desde != "")
            {
                where2 += " and convert(datetime,fecha_despacho ,103)  >= convert(datetime, '" + desde + "', 103) ";
            }

            if (hasta != "")
            {
                where2 += " and convert(datetime,fecha_despacho ,103)   <= convert(datetime, '" + hasta + "', 103) ";
            }

            DataTable camiones = ReporteRNegocio.listar_transpor_solo_plani(where2);
            //DataView view1 = new DataView(camiones1);
            //view1.Sort = "cod_trans asc";
            //DataTable camiones = view1.ToTable(true, "cod_trans", "nombre_trans");

            DataTable dias_suma = new DataTable();
            dias_suma.Columns.Add("dia");
            dias_suma.Columns.Add("sp");
            dias_suma.Columns.Add("cliente");
            dias_suma.Columns.Add("kilos");




            string HTML = "";
            HTML += "<table id='TABLA_REPORTE55' class='table table-advance table-bordered fill-head tablesorter filtrar' style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";

            //HTML += "<table id='TABLA_REPORTE2' class='table table-advance table-bordered fill-head filtrar' style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";
            HTML += "<thead>";
            HTML += "<tr>";
            HTML += "        <th colspan=1; class='test sorter-false' style='background-color: white !important; color:black !important;'></th>";
            foreach (DataRow r1 in dias.Rows)
            {

                //
                string script1 = string.Format("javascript:instruir_jv(&#39;{0}&#39;);return false;", r1[0].ToString());
                //e.Row.Cells[2].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[2].Text + " </a>";
                HTML += "<th colspan=4;  class='test sorter-false'><button style='width:100%;height:100%;' onclick='" + script1 + "'>&nbsp;&nbsp;</button></th>";
                DataRow row_sp = dias_suma.NewRow();
                row_sp["dia"] = r1[0].ToString();
                row_sp["sp"] = 0;
                row_sp["cliente"] = 0;
                row_sp["kilos"] = 0;
                dias_suma.Rows.Add(row_sp);
            }

            HTML += "</tr>";
            HTML += "<tr>";
            //HTML += "        <th colspan=1; style='background-color: white !important; color:black !important;'> </th>";
            HTML += "        <th colspan=1; class='test sorter-false' style='background-color: white !important; color:black !important;'></th>";
            foreach (DataRow r1 in dias.Rows)
            {
                HTML += "<th colspan=4; class='test sorter-false' >" + r1[0].ToString().Substring(0, 10).Trim() + "</th>";

            }

            HTML += "</tr>";

            HTML += "<tr>";
            //HTML += "        <th colspan=1; style='background-color: white !important; color:black !important;'> </th>";
            HTML += "        <th colspan=1; style='background-color: white !important; color:black !important;'>CAMION / DÍA</th>";
            foreach (DataRow r1 in dias.Rows)
            {
                HTML += "<th colspan=1;  class='test' >SP's</th>";
                HTML += "<th colspan=1;  class='test' >Clte</th>";
                HTML += "<th colspan=1;  class='test' >Kilos</th>";
                HTML += "<th colspan=1;  class='test' ></th>";
            }

            HTML += "</tr>";



            HTML += "</thead>";
            HTML += "<tbody>";

            foreach (DataRow r2 in camiones.Rows)
            {
                HTML += "<tr>";

                HTML += "         <td  class='leftfijo test' style='white-space: nowrap;' style='white-space: nowrap;'>" + r2[0].ToString() + "</td>";





                foreach (DataRow r1 in dias.Rows)
                {
                    DataRow[] venta = dt2.Select("'" + r1[0].ToString().Trim() + "'  = FECHA_DESPACHO and nombre_transporte_todo = '" + r2[0].ToString().Trim() + "'");
                    if (venta.Length > 0)
                    {
                        foreach (DataRow row in venta)
                        {

                            DataTable datos_despacho = ReporteRNegocio.datos_ids(row[4].ToString().Trim());
                            if (datos_despacho.Rows.Count > 0)
                            {
                                foreach (DataRow dtas in datos_despacho.Rows)
                                {

                                    HTML += "         <td style='white-space: nowrap;'> " + dtas[0].ToString().Trim() + "</td>";
                                    HTML += "         <td style='white-space: nowrap;'> " + dtas[1].ToString().Trim() + "</td>";
                                    HTML += "         <td style='white-space: nowrap;'> " + Base.monto_format(dtas[2].ToString().Trim().Replace(",000", "")) + "</td>";

                                    foreach (DataRow dia in dias_suma.Rows)
                                    {
                                        if (dia[0].ToString() == r1[0].ToString().Trim())
                                        {
                                            double aa = 0;
                                            try
                                            {
                                                aa = Convert.ToDouble(Base.monto_format(dtas[2].ToString().Trim().Replace(",000", "")).Replace(".", ""));
                                                dia[1] = Convert.ToInt32(dia[1].ToString()) + Convert.ToInt32(dtas[0].ToString());
                                                dia[2] = Convert.ToInt32(dia[2].ToString()) + Convert.ToInt32(dtas[1].ToString());
                                                dia[3] = Convert.ToInt32(dia[3].ToString()) + aa;


                                            }
                                            catch { }

                                        }

                                    }

                                }



                            }
                            else
                            {

                                HTML += "         <td style='white-space: nowrap;'></td>";
                                HTML += "         <td style='white-space: nowrap;'></td>";
                                HTML += "         <td style='white-space: nowrap;'></td>";


                            }

                            HTML += "         <td style='white-space: nowrap;'><a href='REPORTE_CASIGNADOS.aspx?F=" + row[4].ToString().Trim() + "' target = '_blank' ><i class='fa fa-search' aria-hidden='true'></i></a></td>";




                            break;
                        }
                    }
                    else
                    {
                        HTML += "         <td style='white-space: nowrap;'></td>";
                        HTML += "         <td style='white-space: nowrap;'></td>";
                        HTML += "         <td style='white-space: nowrap;'></td>";
                        HTML += "         <td style='white-space: nowrap;'></td>";
                    }

                }


                HTML += "</tr>";
            }

            HTML += "</tr>";
            HTML += "</tbody>";
            HTML += "<tfoot>";
            HTML += "<tr>";
            HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(255, 132, 132) !important; color:white !important;' >TOTAL : : </th>";
            foreach (DataRow r1 in dias.Rows)
            {
                foreach (DataRow dia in dias_suma.Rows)
                {
                    if (dia[0].ToString() == r1[0].ToString().Trim())
                    {
                        HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(255, 132, 132) !important; color:white !important;' >" + Base.monto_format(dia[1].ToString()) + "</th>";
                        HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(255, 132, 132) !important; color:white !important;' >" + Base.monto_format(dia[2].ToString()) + "</th>";
                        HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(255, 132, 132) !important; color:white !important;' >" + Base.monto_format(dia[3].ToString()) + "</th>";
                        HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(255, 132, 132) !important; color:white !important;' ></th>";

                    }

                }
            }
            HTML += "</tr>";

            if (chk_no_plan.Checked)
            {

                HTML += "<tr>";
                HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(168, 255, 132) !important; color:black !important;' >NoPlanificado : : </th>";
                foreach (DataRow r1 in dias.Rows)
                {

                    DataTable datos_no_planifi = ReporteRNegocio.no_planificado(r1[0].ToString());

                    string sps = "";
                    string cltes = "";
                    string kilos = "";
                    foreach (DataRow t1 in datos_no_planifi.Rows)
                    {

                        sps = t1[2].ToString();
                        cltes = t1[1].ToString();
                        kilos = t1[0].ToString();

                    }
                    HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(168, 255, 132) !important; color:black !important;' >" + Base.monto_format(sps.ToString()) + " </th>";
                    HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(168, 255, 132) !important; color:black !important;' >" + Base.monto_format(cltes.ToString()) + "</th>";
                    HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(168, 255, 132) !important; color:black !important;' >" + Base.monto_format(kilos.ToString()).Replace(",000000", "") + "</th>";
                    HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(168, 255, 132) !important; color:black !important;' ><a href='REPORTE_SP_SELECT.aspx?F=" + r1[0].ToString().Trim() + "' target = '_blank' ><i class='fa fa-search' aria-hidden='true'></i></a></th>";
                }
                HTML += "</tr>";

            }


            HTML += "  </table>";

            tabla_instruir.InnerHtml = HTML;

            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teweees", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_INFORME_TOTAL_VENDEDOR')); </script>", false);

            //G_INFORME_TOTAL_VENDEDOR.DataSource = dt2;
            //G_INFORME_TOTAL_VENDEDOR.DataBind();
            JQ_Datatable();

        }

        private bool IsNumeric(string v)
        {
            float output;
            return float.TryParse(v, out output);
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


                    string id_select_scope = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    DataTable produ_desch = ReporteRNegocio.lista_det_sp_asignada(id_select_scope);

                    string update_ok = ReporteRNegocio.desplanificar_sp(id_select_scope, 30);
                    if (update_ok == "OK")
                    {

                        //id, coddocumento, cod_trans, estado, nombre_trans, codbodega
                        string CODdocumetno = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                        string EmailVendedor = ReporteRNegocio.trae_correo_sp(CODdocumetno);
                        string fechaEmision = ReporteRNegocio.trae_fecha_emision_sp(CODdocumetno);

                        string tabla = "";
                        tabla += "<table class=\"table fill-head table-bordered\" style=\"width:100%;\">";
                        tabla += "<thead class=\"test\" style=\"background-color:rgb(156,205,249)\">";
                        tabla += "<tr>";

                        tabla += "<th>Código</th>";
                        tabla += "<th>Nombre</th>";
                        tabla += "<th>CantDespachado</th>";
                        tabla += "<th>TipoUnidad</th>";
                        tabla += "<th>PrecioUnitario</th>";
                        tabla += "<th>Descuento</th>";
                        tabla += "<th>PrecioUnitarioFinal</th>";
                        tabla += "<th>Neto</th>";

                        tabla += "</tr>";
                        tabla += "</thead>";
                        tabla += "<tbody>";
                        foreach (DataRow dr in produ_desch.Rows)
                        {
                            tabla += "<tr>";
                            tabla += "<td>" + dr["codproducto"].ToString() + "</td>";
                            tabla += "<td>" + dr["SP_descproducto"].ToString() + "</td>";
                            tabla += "<td>" + dr["despachado"].ToString() + "</td>";
                            tabla += "<td>" + dr["SP_CodUnVenta"].ToString() + "</td>";
                            tabla += "<td>" + dr["SP_preciounitario"].ToString() + "</td>";
                            tabla += "<td>" + dr["SP_descuento"].ToString() + "</td>";
                            tabla += "<td>" + dr["SP_preciounitariofinal"].ToString() + "</td>";
                            tabla += "<td>" + Base.monto_format(dr["montonetofinal"].ToString()) + "</td>";
                            tabla += "</tr>";
                        }
                        tabla += "</tbody>";
                        tabla += "</table>";
                        tabla = tabla.Replace("'", "");



                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  document.getElementById('ContentPlaceHolder_Contenido_Button1').click();  </script>", false);


                        notificar_vendedor(EmailVendedor, id_select_scope, CODdocumetno, fechaEmision, tabla);


                        //enviar_email_cambio(CodDocumento, FechaEmision, CodVendedor, NotaLibre, CodBodega, FechaDespacho, CodMoneda, MontoNeto, DescEstadoDocumento, Facturas, GxEstadoSync, GxActualizado, GxEnviadoERP, NombreVendedor, NombreCliente, DescBodega, FechaCreacion, ValorTipoCambio, LimiteSeguro, TipoCredito, CreditoDisponible, CreditoAutorizado, EmailVendedor);

                        //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  document.getElementById('ContentPlaceHolder_Contenido_Button1').click();  </script>", false);

                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'> alert('Quitado') </script>", false);


                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'> alert('Algo ocurrió :(') </script>", false);


                    }

                }
                if (e.CommandName == "Enviar")
                {

                    string trans_ = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString());
                    string nom_trans_ = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[4].ToString());
                    string coddocum = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                    string bodega = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString());

                    Session["id_asignada"] = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());


                    //string correos = ReporteRNegocio.trae_correos_bodega(bodega.Trim());

                    //tx_para.Text = correos;

                    carga_camion(" cod_trans = '" + trans_ + "'");
                    carga_chofer(" cod_trans = '" + trans_ + "'");


                    UpdatePanel4.Update();
                    //string script1 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[2].Text), encriptador.EncryptData(""), encriptador.EncryptData(""), encriptador.EncryptData("57"));
                    //e.Row.Cells[2].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[2].Text + " </a>";

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  modal_unidad_1('" + coddocum + "').click();  </script>", false);

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  document.getElementById('ContentPlaceHolder_Contenido_Button1').click();  </script>", false);
                    //modal_unidad_1(" + Name + ");

                    //string sUrl = "/ListadoProductosPlanificador.aspx?C=" + "12542;";
                    //string sScript = "<script language =javascript> ";
                    //sScript += "window.open('" + sUrl + "',null,'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=100%,height=100%,left=100,top=100');";
                    //sScript += "</script> ";
                    //Response.Write(sScript);
                    //Response.Redirect("ListadoProductosPlanificador.aspx?C=10346");

                }



            }
            catch (Exception ex)
            {

            }
        }

        private void notificar_vendedor(string emailVendedor, string v, string codDocumento, string fechaEmision, string tabla)
        {
            string aca = "";


            enviar_email(tabla, emailVendedor, codDocumento, fechaEmision);


            ScriptManager.RegisterStartupScript(Page, this.GetType(), "tee6ee", "<script> alert('SP Quitada');</script>", false);
        }

        private void enviar_email(string tabla, string emailVendedor, string codDocumento, string fechaEmision)
        {
            MailMessage email = new MailMessage();
            //CORREO-CAMBIAR
            email.To.Add(new MailAddress("esteban.godoy15@gmail.com"));
            //email.To.Add(new MailAddress("MRAMIREZ@soprodi.cl"));
            //email.To.Add(new MailAddress(emailVendedor));
            email.From = new MailAddress("informes@soprodi.cl");
            string cliente_2 = Session["cliente"].ToString();
            email.Subject = "QUITADA SP Asignada " + codDocumento + " " + cliente_2 + "( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

            email.CC.Add("informatica@soprodi.cl");

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

            string fecha_emi = ReporteRNegocio.trae_fecha_emision_sp(codDocumento);

            email.Body += "<div> Estimado :<br> <br>  <b> </b> <br><br>";
            email.Body += "<div>  <b> Fecha Emision: " + fecha_emi + "</b> <br><br>";
            email.Body += "<div> Se ha quitado de planificación </b> el siguiente detalle de SP:  <br><br>";
            email.Body += tabla;
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

        private void carga_chofer(string trans)
        {
            //DataTable dt; DataView dtv = new DataView();
            //dt = ReporteRNegocio.DESPA_listar_chofer(trans);
            //dtv = dt.DefaultView;
            //d_chofer.DataSource = dtv;
            //d_chofer.DataTextField = "nombre_chofer";
            //d_chofer.DataValueField = "cod_chofer";
            ////d_vendedor_.SelectedIndex = -1;
            //d_chofer.DataBind();
        }

        private void carga_camion(string trans)
        {
            //DataTable dt; DataView dtv = new DataView();
            //dt = ReporteRNegocio.DESPA_listar_camion(trans);
            //dtv = dt.DefaultView;
            //d_camion.DataSource = dtv;
            //d_camion.DataTextField = "patente";
            //d_camion.DataValueField = "cod_camion";
            ////d_vendedor_.SelectedIndex = -1;
            //d_camion.DataBind();
        }

        private void enviar_email_cambio(string CodDocumento, string FechaEmision, string CodVendedor, string NotaLibre, string CodBodega, string FechaDespacho, string CodMoneda, string MontoNeto, string DescEstadoDocumento, string Facturas, string GxEstadoSync, string GxActualizado, string GxEnviadoERP, string NombreVendedor, string NombreCliente, string DescBodega, string FechaCreacion, string ValorTipoCambio, string LimiteSeguro, string TipoCredito, string CreditoDisponible, string CreditoAutorizado, string EmailVendedor)
        {


            MailMessage email = new MailMessage();
            //CORREO-CAMBIAR
            //email.To.Add(new MailAddress("ESTEBAN.GODOY15@GMAIL.COM"));
            email.To.Add(new MailAddress("MRAMIREZ@soprodi.cl"));


            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "SP Rechazada desde Sistema ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

            //email.CC.Add(EmailVendedor + " , mazocar@soprodi.cl, jcorrea@soprodi.cl, gmorales@soprodi.cl, esteban.godoy15@gmail.com");
            email.CC.Add("informatica@soprodi.cl");


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

        public string sp_selector(string Name)
        {
            return @"javascript: sp_select(" + Name + ");";
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


        [WebMethod]
        public static string INSTRUIR(string dia)
        {

            dia = " where convert(datetime, b.fecha_despacho,103) = convert(datetime, '" + dia + "',103) ";

            string where1 = "";
            if (HttpContext.Current.Session["SW_PERMI"].ToString() == "1")
            {
                where1 += " and b.ASIGNADO_POR = 'ABARROTES' ";
            }
            else if (HttpContext.Current.Session["SW_PERMI"].ToString() == "2")
            {
                where1 += " and b.ASIGNADO_POR = 'GRANOS' ";
            }
            else
            {
                where1 += " and b.ASIGNADO_POR in ('GRANOS', 'ABARROTES')  ";
            }


            DataTable dt = ReporteRNegocio.detalle_planificado_dia(dia, where1);
            DataView dv2 = dt.DefaultView;
            dv2.Sort = "coddocumento";
            dt = dv2.ToTable();

            string tabla = "";
            tabla += "<table class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";
            tabla += "<th>SP</th>";
            tabla += "<th>CodProducto</th>";
            tabla += "<th>DescProducto</th>";
            tabla += "<th>Tipo</th>";
            tabla += "<th>Cantid.KG</th>";
            tabla += "<th>Transporte</th>";
            tabla += "<th>Bodega</th>";
            tabla += "<th>Grupo</th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            double tota_kg = 0;
            foreach (DataRow r in dt.Rows)
            {
                tabla += "<tr>";
                tabla += "<td>" + r["coddocumento"].ToString() + "</td>";
                tabla += "<td>" + r[1].ToString() + "</td>";
                tabla += "<td>" + r[3].ToString() + "</td>";
                tabla += "<td>" + r[4].ToString() + "</td>";
                tabla += "<td>" + r[5].ToString().Replace(",000", "") + "</td>";
                double aux = 0;
                try
                {
                    aux = Convert.ToDouble(r[5].ToString().Replace(",000", ""));
                }
                catch { }
                tota_kg += aux;
                tabla += "<td>" + r[7].ToString() + "</td>";
                tabla += "<td>" + r[8].ToString() + "</td>";
                tabla += "<td>" + r["asignado_por"].ToString() + "</td>";
                tabla += "</tr>";
            }
            tabla += "</tbody>";
            tabla += "</table>";
            DataTable dt_corre = ReporteRNegocio.corre_bodega(dia, where1);
            tabla += "<br><br><h3>Bodegas Asociadas</h3>";
            tabla += "<table class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";
            tabla += "<th>Bodega</th>";
            tabla += "<th>DescBodega</th>";
            tabla += "<th>Correos</th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            string correos = "";

            foreach (DataRow r in dt_corre.Rows)
            {
                tabla += "<tr>";

                tabla += "<td>" + r[0].ToString() + "</td>";
                tabla += "<td>" + r[1].ToString() + "</td>";
                tabla += "<td>" + r[2].ToString() + "</td>";
                if (r[2].ToString().Trim() != "")
                {
                    correos += r[2].ToString() + ",";
                }
                tabla += "</tr>";
            }
            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("Cantid.KG", "Cantid.KG " + Base.monto_format2(tota_kg));
            tabla += "*" + correos.Substring(0, correos.Length - 1) + "*" + dia;
            //tabla = tabla.Replace("'", "");
            return tabla;

        }



        protected void Unnamed_Click(object sender, EventArgs e)
        {

            //string para = tx_para.Text;
            //string obs = tx_text_.Value;
            //string chofer = d_chofer.SelectedValue.ToString();
            //string camion = d_camion.SelectedValue.ToString();

            //string id_asinada = Session["id_asignada"].ToString();
            //string ok = ReporteRNegocio.update_asignada(id_asinada);

            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  document.getElementById('ContentPlaceHolder_Contenido_Button1').click();  </script>", false);


            //string BODEGA = "";
            //MailMessage email = new MailMessage();
            //email.To.Add(new MailAddress(para));
            //email.From = new MailAddress("informes@soprodi.cl");
            //email.Subject = "CORREO BODEGA : " + BODEGA + " ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

            ////email.CC.Add();

            //email.Body += "<div style='text-align:center;     display: block !important;' > ";
            //email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            //email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            //email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            //email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            //email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            //email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            //email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            //email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div> ";
            //email.Body += "</div>";
            //email.Body += "<div><img src='http://a58.imgup.net/Sopro4d9d.png' style='    float: right;     width: 90px;'> </div><br><br><br><br><br>";

            //email.Body += "<div> Estimado :<br> <br>  <b> </b> <br><br>";
            //email.Body += "<div> Datos de la siguiente SP:  <br><br>";
            ////email.Body += Session["tabla_html"].ToString();
            //email.Body += "<br> <b> OBS:  </b> <br> " + obs;

            //email.Body += "</br><br><br> Para más detalles diríjase a:  <a href='http://srv-app.soprodi.cl' > srv-app.soprodi.cl  </a> <span style='font:5px; color:gray; float:right;'>No Responder Este Correo</span> <br><br>";
            //email.Body += "<div style='text-align:center;     display: block !important;' > ";
            //email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            //email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            //email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            //email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            //email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            //email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            //email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            //email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div> ";
            //email.Body += "</div>";

            //try
            //{
            //    //Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            //    //Microsoft.Office.Interop.Word.Document doc = word.Documents.Open("\\\\192.168.10.22\\Archivos\\" + Session["codigo_documento"].ToString().Trim() + ".doc");
            //    //doc.Activate();
            //    //doc.SaveAs2("C:\\DOCUMENTOS_APP_SOPRODI\\SP\\" + Session["codigo_documento"].ToString().Trim() +".pdf", Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
            //    //doc.Close();
            //    //System.Net.Mail.Attachment adj = new System.Net.Mail.Attachment("\\\\192.168.10.22\\Archivos\\" + Session["codigo_documento"].ToString().Trim() + ".doc");
            //    //adj.Name = Session["codigo_documento"].ToString().Trim() + ".doc";
            //    //email.Attachments.Add(adj);
            //}
            //catch (Exception ex)
            //{

            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "man3t_qqqAqN2", "<script>alert('Error: ! " + ex.Message.ToString() + "');</script>", false);

            //}
            //email.IsBodyHtml = true;
            //email.Priority = MailPriority.Normal;
            //email.BodyEncoding = System.Text.Encoding.UTF8;
            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = "srv-correo-2.soprodi.cl";
            //smtp.Port = 25;
            //smtp.EnableSsl = false;


            //smtp.UseDefaultCredentials = false;
            //smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            //try
            //{
            //    //smtp.Send(email);
            //    email.Dispose();

            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>alert('CORREO ENVIADO!');</script>", false);

            //}
            //catch (Exception ex)
            //{
            //    //lb_mensaj.Text = "Error al enviar ";
            //}


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


        public class Evento_objeto
        {

            public string tabla_html { get; set; }

        }


        [WebMethod]
        public static string CargarEvento55(string id)
        {
            DataTable dt = new DataTable();

            dt = ReporteRNegocio.lista_det_sp_asignada(id);


            string tabla = "";
            tabla += "<table class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";

            tabla += "<th>Código</th>";
            tabla += "<th>Nombre</th>";
            tabla += "<th>TipoUnidad</th>";
            tabla += "<th>CantDespachado</th>";
            tabla += "<th>CantFacturado</th>";

            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            foreach (DataRow dr in dt.Rows)
            {
                tabla += "<tr>";
                tabla += "<td>" + dr["codproducto"].ToString() + "</td>";
                tabla += "<td>" + dr["SP_descproducto"].ToString() + "</td>";
                tabla += "<td>" + dr["SP_CodUnVenta"].ToString() + "</td>";
                tabla += "<td>" + dr["despachado"].ToString() + "</td>";
                tabla += "<td>" + dr["facturado"].ToString() + "</td>";
                tabla += "</tr>";
            }
            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");



            return tabla;
        }

        protected void G_DETALLE_PLANI_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void instruir_ServerClick(object sender, EventArgs e)
        {


            String nameOfControl = (sender as HtmlAnchor).Name.ToString();
            string aca = txt_instruir.Text;

            string asd = "";
        }

        protected void Button2_Click1(object sender, EventArgs e)
        {

            string dia = titulo_modal_22.Text.Split('\'')[1].ToString();
            string where1 = "";
            string grupo = "";
            if (HttpContext.Current.Session["SW_PERMI"].ToString() == "1")
            {
                where1 += " and b.ASIGNADO_POR = 'ABARROTES' ";
                grupo = "'ABARROTES'";
            }
            else if (HttpContext.Current.Session["SW_PERMI"].ToString() == "2")
            {
                where1 += " and b.ASIGNADO_POR = 'GRANOS' ";
                grupo = "'GRANOS'";
            }
            else
            {
                where1 += " and b.ASIGNADO_POR in ('GRANOS', 'ABARROTES')  ";
                grupo = "'ABARROTES', 'GRANOS'";
            }



            DataTable dt = ReporteRNegocio.detalle_planificado_dia(titulo_modal_22.Text, where1);
            DataTable dt_camiones = ReporteRNegocio.detalle_planificado_dia_camiones(titulo_modal_22.Text, where1);

            DataView view = new DataView(dt);
            DataTable bodegas = view.ToTable(true, "bodega");
            //DataTable dt_corre = ReporteRNegocio.corre_bodega(titulo_modal_22.InnerText);

            double tota_kg = 0;

            foreach (DataRow r1 in bodegas.Rows)
            {
                List<string> sps = new List<string>();

                string correos = ReporteRNegocio.trae_correos_bodega(r1[0].ToString(), grupo);
                if (correos == "")
                {
                    correos = "esteban.godoy15@gmai.com, gmorales@soprodi.cl";
                }


                string tabla = "";
                tabla += "<table border=2 class=\"table fill-head table-bordered\">";
                tabla += "<thead class=\"test\">";
                tabla += "<tr>";

                tabla += "<th>Orden Entrega</th>";
                tabla += "<th>SP</th>";
                tabla += "<th>Transporte</th>";
                tabla += "<th>NomCliente</th>";
                tabla += "<th>CodProducto</th>";
                tabla += "<th>DescProducto</th>";
                tabla += "<th>Tipo</th>";
                tabla += "<th>Cantid</th>";
                tabla += "<th>$   </th>";
                tabla += "<th>Grupo</th>";

                tabla += "</tr>";

                tabla += "</thead>";
                tabla += "<tbody>";


                foreach (DataRow r in dt.Rows)
                {
                    double precio = 0;
                    if (r[8].ToString() == r1[0].ToString())

                    {

                        tabla += "<tr>";
                        tabla += "<td>" + r["orden_cargar"].ToString() + "</td>";
                        tabla += "<td>" + r["coddocumento"].ToString() + "</td>";
                        tabla += "<td>" + r["transpo"].ToString() + "</td>";
                        tabla += "<td>" + r["nombrecliente"].ToString() + "</td>";
                        tabla += "<td>" + r["CODPRODUCTO"].ToString() + "</td>";
                        tabla += "<td>" + r["nomb"].ToString() + "</td>";
                        tabla += "<td>" + r["tipo_"].ToString().Replace(",000", "") + "</td>";

                        double aux = 0;
                        try
                        {
                            aux = Convert.ToDouble(r["cant_kg"].ToString().Replace(",000", ""));
                        }
                        catch { }
                        tota_kg += aux;

                        double cantidad = 0;
                        try
                        {
                            cantidad = Convert.ToDouble(r[2].ToString().Replace(",000", ""));
                        }
                        catch { }
                        tabla += "<td>" + r["despachado"].ToString().Replace(",00", "") + "</td>";
                        //tabla += "<td>" + r[10].ToString() + "</td>";
                        double precio_unidad = 0;
                        try
                        {
                            precio_unidad = Convert.ToDouble(r[11].ToString().Replace(",000", ""));
                        }
                        catch { }
                        precio += cantidad * precio_unidad;
                        tabla += "<td>" + Base.monto_format2(precio) + "</td>";
                        tabla += "<td>" + r["asignado_por"].ToString() + "</td>";
                        sps.Add(r[9].ToString().Trim());
                        tabla += "</tr>";
                    }
                }

                tabla += "</tbody>";
                tabla += "</table>";

                tabla += "</br></br>";

                tabla += "<table border=2 class=\"table fill-head table-bordered\">";
                tabla += "<thead class=\"test\">";
                tabla += "<tr>";
                tabla += "<th>Transporte</th>";
                tabla += "<th>Total.KG</th>";
                tabla += "<th>CargaCamión</th>";
                tabla += "<th>Dif.Carga</th>";


                tabla += "</tr>";

                tabla += "</thead>";
                tabla += "<tbody>";
                foreach (DataRow r in dt_camiones.Rows)
                {
                    if (r[3].ToString() == r1[0].ToString())

                    {
                        tabla += "<tr>";

                        tabla += "<td>" + r[1].ToString() + "</td>";
                        tabla += "<td>" + r[0].ToString().Replace(",000", "") + " </td>";
                        tabla += "<td>" + r[2].ToString() + "</td>";
                        tabla += "<td>" + r[4].ToString().Replace(",000", "") + "</td>";
                        tabla += "</tr>";

                    }

                }
                tabla += "</tbody>";
                tabla += "</table>";

                tabla += "</br></br>";

                tabla += "<b> Total Kilos:  </b>" + Base.monto_format2(tota_kg);





                List<string> sps_aux = new List<string>();

                // vuelco a temporal
                foreach (var temp in sps)
                    sps_aux.Add(temp);
                // filtro
                sps = sps_aux.Distinct().ToList();
                notificar_bodega(correos, tabla, r1[0].ToString(), Textarea1.Value, sps, dia);
            }
        }

        private void notificar_bodega(string correos, string tabla, string bodega, string obs, List<string> sps, string dia)
        {
            try
            {
                correos = correos.Replace(";", ",");
            }
            catch { }
            MailMessage email = new MailMessage();
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "Planificación Sp's BODEGA - : " + bodega + " ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

            if (correos == "")
            {
                correos = "informatica@soprodi.cl";
            }
            else
            {
                //email.CC.Add(correos);
            }
            //CORREO-CAMBIAR
            //email.To.Add(new MailAddress("esteban.godoy15@gmail.com"));
            email.To.Add(new MailAddress("informatica@soprodi.cl"));

            if (Session["SW_PERMI"].ToString() == "1")
            {
                email.CC.Add("transporte@soprodi.cl, mazocar@soprodi.cl, rmc@soprodi.cl, gmorales@soprodi.cl, pcataldo@soprodi.cl, " + correos);
            }
            else
            {
                email.CC.Add("transporte@soprodi.cl,MRAMIREZ@soprodi.cl," + correos);
            }

            //email.CC.Add("informatica@soprodi.cl");

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
            email.Body += "<div> Se ha planificado el despacho con fecha <b>" + dia + "</b> el siguiente detalle:  <br><br>";
            email.Body += tabla;
            email.Body += "<br> <b style='background-color: yellow;'> OBS:  </b> <br> " + obs;

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


                foreach (string te in sps)
                {
                    try
                    {

                        string n_file = te.Trim() + ".pdf";
                        string pdfPath = Server.MapPath(@"~\PDFs\") + n_file;
                        Base.crear_sp_pdf(te, pdfPath);
                        System.IO.FileInfo toDownload = new System.IO.FileInfo(pdfPath);
                        if (toDownload.Exists)
                        {
                            email.Attachments.Add(new System.Net.Mail.Attachment(pdfPath));
                        }
                    }
                    catch
                    {


                    }
                }
            }
            catch (Exception ex)
            {

                //ScriptManager.RegisterStartupScript(Page, this.GetType(), "man3t_qqqAqN2", "<script>alert('Error: ! " + ex.Message.ToString() + "');</script>", false);

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
                notifi.Text = "Correo enviado! ";
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>   document.getElementById(\"Button98\").click(); ;</script>", false);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ma3nt_qqqAqN2", "<script>  alert('Correo Enviado') </script>", false);
            }
            catch (Exception ex)
            {
                notifi.Text = "Error al enviar ";

            }

            /////desde gmail

            //email.IsBodyHtml = true;
            //email.Priority = MailPriority.Normal;
            //email.BodyEncoding = System.Text.Encoding.UTF8;
            //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            //smtp.EnableSsl = true;
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.UseDefaultCredentials = false;

            //////smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            //smtp.Credentials = new NetworkCredential("informes.soprodi@gmail.com", "galaxia1234");
            //try
            //{
            //    smtp.Send(email);
            //    email.Dispose();
            //    notifi.Text = "Correo enviado! ";
            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>   document.getElementById(\"Button98\").click(); ;</script>", false);
            //    //ScriptManager.RegisterStartupScript(Page, this.GetType(), "ma3nt_qqqAqN2", "<script>  alert('Correo Enviado') </script>", false);
            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "ma3nt_qqqAqN2", "<script>  alert('Correo Enviado') </script>", false);

            //    //display: none;
            //}
            //catch (Exception ex)
            //{
            //    notifi.Text = "Error al enviar ";
            //    //ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>alert('Error: '" + ex.Message + ");</script>", false);

            //}

        }
    }
}