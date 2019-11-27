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
    public partial class REPORTE_CASIGNADOS : System.Web.UI.Page
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
                Session["ids"] = "JJ";
                string ids = Request.QueryString["F"];
                t_correos.Text = "";
                if (ids != "JJ" && ids != null)
                {
                    div_agregar_facturas.Visible = true;
                    div_totales.Visible = true;
                    div_adjuntar_documentos.Visible = true;
                    Session["ids"] = ids;
                    DataTable dt_facturas = new DataTable();
                    txt_desde.Text = "";
                    txt_hasta.Text = "";
                    //TEMPORAL _ PROBAR
                    dt_facturas.Columns.Add("FACTURA");
                    dt_facturas.Columns.Add("ESTADO");
                    G_FACTURAS.DataSource = dt_facturas;
                    G_FACTURAS.DataBind();
                    Session["dt_facturas_camion"] = dt_facturas;
                }
                else
                {
                    DateTime t = DateTime.Now.AddDays(-11);
                    DateTime t2 = DateTime.Now.AddDays(+5);
                    txt_desde.Text = t.ToShortDateString();
                    txt_hasta.Text = t2.ToShortDateString();
                    div_agregar_facturas.Visible = false;
                    div_adjuntar_documentos.Visible = false;
                    h3_transporte.InnerText = "";
                }
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

                try
                {
                    if (Session["SW_PERMI"].ToString() == "1")
                    {
                        //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=1'>Abarrotes</a>";
                        titulo2.HRef = "reportes.aspx?s=1";
                        titulo2.InnerText = "Abarrotes";
                        titulo3.HRef = "Menu_Planificador.aspx";
                        titulo3.InnerText = "Planificador de Despachos";
                        Session["asignado_por"] = "ABARROTES";
                    }
                    else if (Session["SW_PERMI"].ToString() == "2")
                    {
                        //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=2'>Granos</a>";
                        titulo2.HRef = "reportes.aspx?s=2";
                        titulo2.InnerText = "Granos";
                        titulo3.HRef = "Menu_Planificador.aspx";
                        titulo3.InnerText = "Planificador de Despachos";
                        Session["asignado_por"] = "GRANOS";
                    }
                    else if (Session["SW_PERMI"].ToString() == "4")
                    {
                        //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=2'>Granos</a>";
                        //titulo2.HRef = "reportes.aspx?s=2";
                        //titulo2.InnerText = "Granos";
                        titulo3.HRef = "Menu_Planificador.aspx?s=4";
                        titulo3.InnerText = "Planificador de Despachos";
                    }
                }
                catch { }

                USER = User.Identity.Name.ToString();
                l_usuario_.Text = USER;


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


                ScriptManager.RegisterStartupScript(Page, this.GetType(), "datatable_", "<script language='javascript'> creagrilla();</script>", false);

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
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "asd", "<script language='javascript'>creagrilla();</script>", false);
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
                    e.Row.Cells[2].Text = "";
                    //e.Row.Cells[0].Text = "";
                    //e.Row.Cells[0].Visible = false;
                    ImageButton ts = new ImageButton();
                    ts = (ImageButton)e.Row.Cells[0].FindControl("btn_quitar");
                    //ts.Attributes["style"] = "visibility:hidden;";
                    e.Row.Attributes["class"] = "estado10";
                }
                double total_cargado_kg = Convert.ToDouble(Session["total_cargado_kg"]);
                double total_cargado_peso = Convert.ToDouble(Session["total_cargado_peso"]);

                double cargado_kg = 0;
                double cargado_peso = 0;

                double.TryParse(e.Row.Cells[8].Text, out cargado_kg);
                double.TryParse(e.Row.Cells[10].Text, out cargado_peso);

                total_cargado_kg += cargado_kg;
                total_cargado_peso += cargado_peso;

                Session["total_cargado_kg"] = total_cargado_kg;
                Session["total_cargado_peso"] = total_cargado_peso;

                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                string script = string.Format("javascript:CargarEvento_Tabla(&#39;{0}&#39;);", G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values[0].ToString().Trim());
                e.Row.Cells[3].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[3].Text + " </a>";

                if (ReporteRNegocio.esvendedor(User.Identity.Name) == "2")
                {

                    e.Row.Cells[0].Visible = false;
                    G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[0].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[2].Visible = false;



                }
                e.Row.Cells[2].Visible = false;
                G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[2].Visible = false;



                if (Session["ids"].ToString() != "JJ")
                {
                    Session["fecha_transporte"] = e.Row.Cells[11].Text;
                    h3_transporte.InnerText = e.Row.Cells[6].Text + " --FECHA : " + e.Row.Cells[11].Text;


                }

                if (G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values[8].ToString().Trim() != "0")
                {

                    string script2 = string.Format("javascript:CargarEvento_Tabla2(&#39;{0}&#39;);", G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values[8].ToString().Trim());
                    e.Row.Cells[11].Text = "  <a href='javascript:' onclick='" + script2 + "'>" + e.Row.Cells[11].Text + " </a>";

                }



                //e.Row.Cells[1].Visible = false;
                //G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[1].Visible = false;

            }



        }
        int GetColumnIndexByName(GridViewRow row, string columnName)
        {
            int columnIndex = 0;
            foreach (DataControlFieldCell cell in row.Cells)
            {
                if (cell.ContainingField is BoundField)
                    if (((BoundField)cell.ContainingField).DataField.Equals(columnName))
                        break;
                columnIndex++; // keep adding 1 while we don't have the correct name
            }
            return columnIndex;
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

                Session["codvendedor"] = " and codvendedor = '" + User.Identity.Name.ToString() + "'";
            }




            cargar_clientes_SP();
            cargar_estado_SP();
            //cargar_grupo_sp();
            cargar_vendedor_SP();
            CargarBodega();

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
        private void CargarBodega()
        {
            string where = " where 1=1 ";

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.carga_bodega(where);
            dtv = dt.DefaultView;
            CB_BODEGA.DataSource = dtv;
            CB_BODEGA.DataTextField = "nom_bodega";
            CB_BODEGA.DataValueField = "nom_bodega";
            CB_BODEGA.DataBind();
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
        private void cargar_estado_SP()
        {

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.VM_estados_asignados("");
            dtv = dt.DefaultView;
            d_bodega_2.DataSource = dtv;
            d_bodega_2.DataTextField = "descrestado";
            d_bodega_2.DataValueField = "estado";
            //d_vendedor_.SelectedIndex = -1;
            d_bodega_2.DataBind();
        }
        private void cargar_clientes_SP()
        {

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.VM_clientes(" where 1=1 " + Session["codvendedor"], "");
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

            string grupo = agregra_comillas(l_grupo_vm.Text);
            string vendedor = agregra_comillas(l_vendedor_vm.Text);
            string bodega = agregra_comillas(l_bodega_2.Text);


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
                //confirmDelete
                where1 += " and RUT IN (" + clientes + ")";
            }
            if (estados != "")
            {

                where1 += " and estado IN (" + estados + ")";
            }

            if (Session["ids"].ToString() != "JJ")
            {
                //viene desde "instruir" 
                div_superior.Visible = false;
                where1 += "  and id in (" + Session["ids"].ToString() + ")";
            }

            if (txt_sp.Text != "")
            {
                where1 += " and coddocumento in (" + agregra_comillas(txt_sp.Text) + ") ";
            }
            string grupo_2 = "";
            if (Session["SW_PERMI"].ToString() == "1")
            {
                grupo_2 = "ABARROTES";
                where1 += " and ASIGNADO_POR = 'ABARROTES' ";
            }
            else if (Session["SW_PERMI"].ToString() == "2")
            {
                grupo_2 = "GRANOS";
                where1 += " and ASIGNADO_POR = 'GRANOS' ";
            }
            else
            {
                grupo_2 = "GRANOS, ABARROTES";
                where1 += " and ASIGNADO_POR in ('GRANOS', 'ABARROTES') ";
            }


            if (bodega != "")
            {
                where1 += " and CodBodega in (" + bodega + ")";
            }

            DataTable dt2 = ReporteRNegocio.listar_camiones_asignados(where1);

            if (Session["ids"].ToString() != "JJ")
            {
                List<string> id = (dt2.AsEnumerable().Select(x => x["id"].ToString()).Distinct().ToList());
                List<string> sps = (dt2.AsEnumerable().Select(x => x["coddocumento"].ToString()).Distinct().ToList());
                List<string> cod_trans = (dt2.AsEnumerable().Select(x => x["cod_trans"].ToString()).Distinct().ToList());
                List<string> cod_camion = (dt2.AsEnumerable().Select(x => x["cod_camion"].ToString()).Distinct().ToList());
                List<string> cod_chofer = (dt2.AsEnumerable().Select(x => x["cod_chofer"].ToString()).Distinct().ToList());
                List<string> dia_plani = (dt2.AsEnumerable().Select(x => x["fecha_despacho"].ToString()).Distinct().ToList());

                string sps_2 = agregra_comillas(string.Join(",", sps.ToArray()));
                string bodegas_sp = agregra_comillas(ReporteRNegocio.bodegas_sp(sps_2));
                grupo_2 = agregra_comillas(grupo_2);
                string correos_bodega = ReporteRNegocio.trae_correos_bodega_2(bodegas_sp, grupo_2);
                t_correos.Text = correos_bodega;

                Session["sps_LIST"] = sps;
                Session["sps_"] = string.Join(",", sps.ToArray());
                Session["ids_"] = string.Join(",", id.ToArray());
                Session["cod_trans"] = string.Join(",", cod_trans.ToArray());
                Session["cod_camion"] = string.Join(",", cod_camion.ToArray());
                Session["cod_chofer"] = string.Join(",", cod_chofer.ToArray());
                Session["dia_plani"] = string.Join(",", dia_plani.ToArray());

                int existe_id_cierre = ReporteRNegocio.existe_cierre_camion_Select(Session["cod_trans"].ToString(), Session["cod_camion"].ToString(), Session["cod_chofer"].ToString(), Session["sps_"].ToString(), Session["dia_plani"].ToString());
                Session["id_cierre_enc"] = existe_id_cierre;


                DataTable factura_ = ReporteRNegocio.cierre_camion(existe_id_cierre);

                if (factura_.Rows.Count > 0)
                {

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "cerrado_Camion", "<script language='javascript'> cerrado();</script>", false);

                    Session["ya_esta_cerrado"] = "SI";
                    btn_validar_factura.Visible = false;
                    btn_cerrar_camion.Visible = false;
                    tx_factura.Visible = false;
                }
                else
                {
                    Session["ya_esta_cerrado"] = "NO";
                    btn_validar_factura.Visible = true;
                    btn_cerrar_camion.Visible = true;
                    tx_factura.Visible = true;
                }

                G_FACTURAS.DataSource = factura_;
                G_FACTURAS.DataBind();
            }

            try
            {
                Session["transportista"] = dt2.Rows[0]["nombre_trans"].ToString();
                Session["camion"] = dt2.Rows[0]["patente"].ToString();
                Session["chofer"] = dt2.Rows[0]["nombre_chofer"].ToString();
                Session["codbodega"] = dt2.Rows[0]["codbodega"].ToString();
            }
            catch
            { }
            Session["total_cargado_kg"] = 0;
            Session["total_cargado_peso"] = 0;

            G_INFORME_TOTAL_VENDEDOR.DataSource = dt2;

            G_INFORME_TOTAL_VENDEDOR.DataBind();
            JQ_Datatable();


            //totales
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "total_cargado_kg", "<script language='javascript'> document.getElementById('total_carga').innerHTML = '" + Base.monto_format(Session["total_cargado_kg"].ToString()) + " (kg.)'; </script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "total_cargado_peso", "<script language='javascript'> document.getElementById('total_peso').innerHTML = '" + Base.monto_format(Session["total_cargado_peso"].ToString()) + "'; </script>", false);

            //Session["total_cargado_kg"].ToString();
            //Session["total_cargado_peso"].ToString();
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
                    //string update_ok = "OK";
                    if (update_ok == "OK")
                    {
                        //id, coddocumento, cod_trans, estado, nombre_trans, codbodega
                        string CODdocumetno = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                        string EmailVendedor = ReporteRNegocio.trae_correo_sp(CODdocumetno);
                        string fechaEmision = ReporteRNegocio.trae_fecha_emision_sp(CODdocumetno);
                        string cliente_1 = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[6].ToString());

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

                        notificar_vendedor(EmailVendedor, id_select_scope, CODdocumetno, fechaEmision, tabla, cliente_1);

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
                    //carga_chofer(" cod_trans = '" + trans_ + "'");

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

                    //G_INFORME_TOTAL_VENDEDOR.DataBind();

                }

                if (e.CommandName == "Editar")
                {
                    string trans_ = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString());
                    string nom_trans_ = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[4].ToString());
                    string coddocum = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                    string bodega = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString());
                    string fecha = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[7].ToString());
                    string camion = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[9].ToString());
                    string chofer = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[10].ToString());
                    string observacion = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[11].ToString());
                    string bodega_plani = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[12].ToString());
                    string carga_inicial = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[13].ToString());
                    string fecha_type_date = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[16].ToString());
                    string vuelta = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[17].ToString());

                    Session["id_asignada"] = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());

                    cargar_bodega_3(bodega_plani);
                    cargar_trans(bodega_plani, trans_);

                    try
                    {
                        cargar_camion(trans_, camion);
                        cargar_chofer(trans_, chofer);
                    }
                    catch { }
                    tx_obs_plani.Text = observacion;
                    inputTitle.Text = carga_inicial;
                    t_fecha_despach2.Text = fecha_type_date;

                    l_bodega.Text = bodega_plani;
                    l_transpor.Text = trans_;
                    l_camion.Text = camion;
                    l_chofer.Text = chofer;

                    tx_vuelta.Text = vuelta;

                    Session["bodega_plani"] = bodega_plani;
                    Session["trans_"] = trans_;
                    Session["camion"] = camion;
                    Session["chofer"] = chofer;
                    //DataTable dt = new DataTable();

                    //dt = ReporteRNegocio.lista_det_sp_asignada(Session["id_asignada"].ToString());

                    //G_DETALLE_PLANIFICADO.DataSource = dt;
                    //G_DETALLE_PLANIFICADO.DataBind();
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "chosen", "<script language='javascript'>  load_chosen_combos();  </script>", false);

                    UpdatePanel4.Update();

                    Session["coddocumento"] = coddocum;

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  modal_unidad_1('" + coddocum + "');  </script>", false);
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  document.getElementById('ContentPlaceHolder_Contenido_Button1').click();  </script>", false);
                }



            }
            catch (Exception ex)
            {

            }
        }

        private void cargar_chofer(string trans_, string elegido)
        {
            string where = " and cod_trans = '" + trans_ + "'";

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.listar_chofer(where);
            dtv = dt.DefaultView;
            d_chofer.DataSource = dtv;
            d_chofer.DataTextField = "NOMBRE_CHOFER";
            d_chofer.DataValueField = "COD_CHOFER";
            d_chofer.DataBind();
            d_chofer.SelectedValue = elegido;
        }

        private void cargar_camion(string trans_, string elegido)
        {
            string where = " and cod_trans = '" + trans_ + "'";

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.listar_camion(where);
            dtv = dt.DefaultView;
            d_camion.DataSource = dtv;
            d_camion.DataTextField = "PATENTE";
            d_camion.DataValueField = "COD_CAMION";
            d_camion.DataBind();
            d_camion.SelectedValue = elegido;
        }

        public void cargar_bodega_3(string bodega)
        {
            string where = " where 1=1 ";

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.carga_bodega(where);
            dtv = dt.DefaultView;
            d_bodega_3.DataSource = dtv;
            d_bodega_3.DataTextField = "NOM_BODEGA";
            d_bodega_3.DataValueField = "NOM_BODEGA";
            d_bodega_3.DataBind();
            d_bodega_3.SelectedValue = bodega;

        }

        private void cargar_trans(string bodega, string elegido)
        {
            string where = " where cod_bodega = '" + bodega + "'";

            if (Session["SW_PERMI"].ToString() == "1")
            {
                where += " and grupo = 'ABARROTES' ";
            }
            else if (Session["SW_PERMI"].ToString() == "2")
            {
                where += " and grupo = 'GRANOS' ";
            }
            else
            {
                where += " and grupo in ( 'GRANOS' , 'ABARROTES' ) ";
            }


            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.listar_transpor_2(where);
            dtv = dt.DefaultView;
            d_transpor.DataSource = dtv;
            d_transpor.DataTextField = "nombre_trans";
            d_transpor.DataValueField = "cod_trans";
            d_transpor.DataBind();
            d_transpor.SelectedValue = elegido;
        }

        private void notificar_vendedor(string emailVendedor, string v, string codDocumento, string fechaEmision, string tabla, string cliente)
        {
            string aca = "";


            enviar_email(tabla, emailVendedor, codDocumento, fechaEmision, cliente);


            ScriptManager.RegisterStartupScript(Page, this.GetType(), "tee6ee", "<script> alert('SP Quitada');</script>", false);
        }

        private void enviar_email(string tabla, string EmailVendedor, string codDocumento, string fechaEmision, string cliente)
        {
            MailMessage email = new MailMessage();
            //email.To.Add(new MailAddress("esteban.godoy15@gmail.com"));

            EmailVendedor = EmailVendedor.Replace(";", ",");

            if (EmailVendedor.Trim() == "")
            {
                //emailVendedor = "rmc@soprodi.cl";
            }
            //CORREO-CAMBIAR
            //email.To.Add(new MailAddress(emailVendedor));
            //email.To.Add(new MailAddress("esteban.godoy15@gmail.com"));
            email.To.Add(new MailAddress("informatica@soprodi.cl"));
            email.From = new MailAddress("informes@soprodi.cl");
            string cliente_2 = cliente;
            email.Subject = "QUITADA SP Asignada " + codDocumento + " " + cliente_2 + "( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

            //email.CC.Add("esteban.godoy15@gmail.com, rmc@soprodi.cl, gmorales@soprodi.cl");
            //email.CC.Add("informatica@soprodi.cl");

            if (Session["SW_PERMI"].ToString() == "1")
            {
                if (EmailVendedor.Trim() == "")
                {
                    email.CC.Add("transporte@soprodi.cl, mazocar@soprodi.cl, rmc@soprodi.cl, gmorales@soprodi.cl, pcataldo@soprodi.cl");
                }
                else
                {
                    email.CC.Add("transporte@soprodi.cl, mazocar@soprodi.cl, rmc@soprodi.cl, gmorales@soprodi.cl, pcataldo@soprodi.cl, " + EmailVendedor);
                }
            }
            else
            {
                if (EmailVendedor.Trim() == "")
                {
                    email.CC.Add("transporte@soprodi.cl, MRAMIREZ@soprodi.cl");
                }
                else
                {
                    email.CC.Add("transporte@soprodi.cl, MRAMIREZ@soprodi.cl, " + EmailVendedor);
                }
            }

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
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "CORREO_PLANI2", "<script>alert('ERROR AL ENVIAR CORREO!');</script>", false);
            }

            /////desde gmail---------------------------------------------------------------------------------------------------------

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
            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>alert('CORREO ENVIADO!');</script>", false);
            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "CORREO_PLANI", "<script>alert('ERROR AL ENVIAR CORREO!');</script>", false);
            //}

        }



        private void carga_camion(string trans)
        {
            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.DESPA_listar_camion(trans);
            dtv = dt.DefaultView;
            d_camion.DataSource = dtv;
            d_camion.DataTextField = "patente";
            d_camion.DataValueField = "cod_camion";
            //d_vendedor_.SelectedIndex = -1;
            d_camion.DataBind();
        }

        private void enviar_email_cambio(string CodDocumento, string FechaEmision, string CodVendedor, string NotaLibre, string CodBodega, string FechaDespacho, string CodMoneda, string MontoNeto, string DescEstadoDocumento, string Facturas, string GxEstadoSync, string GxActualizado, string GxEnviadoERP, string NombreVendedor, string NombreCliente, string DescBodega, string FechaCreacion, string ValorTipoCambio, string LimiteSeguro, string TipoCredito, string CreditoDisponible, string CreditoAutorizado, string EmailVendedor)
        {


            MailMessage email = new MailMessage();
            //email.To.Add(new MailAddress("rmc@soprodi.cl"));
            email.To.Add(new MailAddress("esteban.godoy15@gmail.com"));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "SP Rechazada desde Sistema ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

            //email.CC.Add(EmailVendedor + " , mazocar@soprodi.cl, gmorales@soprodi.cl, esteban.godoy15@gmail.com");

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
            return @"javascript:if(!confirm('Esta acción va eliminar la planificación del documento: "
               + Name.ToUpper()
               + @". ¿Estás seguro?')){return false;} ; CARGANDO();";
        }

        public string confirmDelete2(string Name)
        {
            return @"javascript:if(!confirm('Esta acción va eliminar el producto planificado: "
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




        public class Transportista
        {
            public string cod_trans { get; set; }
            public string nombre_trans { get; set; }

        }
        public class Camion
        {
            public string cod_camion { get; set; }
            public string patente { get; set; }
            public string carga { get; set; }
        }

        public class Chofer
        {
            public string cod_chofer { get; set; }
            public string nombre_chofer { get; set; }
        }





        [WebMethod]
        public static List<Transportista> BODEGA(string BODEGA)
        {
            DataTable dt = new DataTable();

            string where = " where 1=1 and  cod_bodega = '" + BODEGA + "'";
            //Session["asignado_por"]

            where += " and grupo = '" + HttpContext.Current.Session["asignado_por"].ToString() + "' ";

            try
            {
                dt = ReporteRNegocio.listar_transpor_2(where);
                DataView dv = dt.DefaultView;
                dv.Sort = "nom_vend";
                dt = dv.ToTable();
            }
            catch { }


            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new Transportista
                        {
                            cod_trans = Convert.ToString(item["cod_trans"]),
                            nombre_trans = Convert.ToString(item["nombre_trans"])


                        };
            return query.ToList<Transportista>();
        }


        [WebMethod]
        public static List<Chofer> TRANSPOR_CHOFER(string TRANSPOR)
        {
            DataTable dt = new DataTable();

            string where = " and cod_trans = '" + TRANSPOR + "'";

            try
            {
                dt = ReporteRNegocio.listar_chofer(where);
                DataView dv = dt.DefaultView;
                dv.Sort = "nombre_chofer";
                dt = dv.ToTable();
            }
            catch { }

            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new Chofer
                        {
                            cod_chofer = Convert.ToString(item["cod_chofer"]),
                            nombre_chofer = Convert.ToString(item["nombre_chofer"])


                        };
            return query.ToList<Chofer>();
        }

        [WebMethod]
        public static List<Camion> TRANSPOR_CAMION(string TRANSPOR)
        {
            DataTable dt = new DataTable();

            string where = " and cod_trans = '" + TRANSPOR + "'";

            try
            {
                dt = ReporteRNegocio.listar_camion(where);
                DataView dv = dt.DefaultView;
                dv.Sort = "patente";
                dt = dv.ToTable();
            }
            catch { }

            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new Camion
                        {
                            cod_camion = Convert.ToString(item["cod_camion"]),
                            patente = Convert.ToString(item["patente"])


                        };
            return query.ToList<Camion>();
        }



        [WebMethod]
        public static string CAMION_CARGA(string CAMION)
        {
            string carga_inicial = "";

            DataTable dt_camion = ReporteRNegocio.listar_camion(" and cod_camion = '" + CAMION + "'");

            carga_inicial = dt_camion.Rows[0][3].ToString();

            return carga_inicial;
        }

        [WebMethod]
        public static string CARGA_INICIAL_TRANSPORTE(string TRANSPOR)
        {
            string carga_inicial = "";

            DataTable dt_transporte = ReporteRNegocio.trae_transportistas(" and cod_trans = '" + TRANSPOR + "'");

            carga_inicial = dt_transporte.Rows[0][5].ToString();

            return carga_inicial;
        }



        protected void Unnamed_Click(object sender, EventArgs e)
        {
            string id = Session["id_asignada"].ToString();
            string ok = ReporteRNegocio.selec_insert_log(id);
            if (ok == "OK")
            {
                string carga = inputTitle.Text;
                string transpor = l_transpor.Text;
                string camion = l_camion.Text;
                string chofer = l_chofer.Text;
                string obs = tx_obs_plani.Text;
                string vuelta = tx_vuelta.Text;
                string ok_update = ReporteRNegocio.update_asignada_replanificar(id, d_camion.SelectedValue.ToString(), t_fecha_despach2.Text, carga, transpor, camion, chofer, obs, vuelta);
                if (ok_update == "OK")
                {
                    enviar_correo_re_planificado(id, obs);

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11qQ21mp", "<script language='javascript'> alert('Re-Planificado') </script>", false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11qQ21mp", "<script language='javascript'> alert('Problema al re-planificar.') </script>", false);
                }
            }
            else
            {

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11qQ21mp", "<script language='javascript'> alert('Problema al re-planificar.') </script>", false);

            }

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "chosen_2", "<script language='javascript'> load_chosen_combos();   </script>", false);


        }

        private void enviar_correo_re_planificado(string id, string obs)
        {
            //anterior
            DataTable sp_asginada_log = ReporteRNegocio.trae_sp_re_planificada(id);

            //ahora
            DataTable sp_asginada = ReporteRNegocio.trae_sp_planificada(id);

            //detalle
            DataTable produ_desch = ReporteRNegocio.lista_det_sp_asignada(id);

            string total_kg = ReporteRNegocio.total_kg_id(id);

            string transporte_re_pla = sp_asginada.Rows[0]["nombre_transporte_todo"].ToString();
            string transporte_pla = sp_asginada_log.Rows[0]["nombre_transporte_todo"].ToString();

            string tabla_transporte = "";

            //tabla_transporte += "<div>  <b> Transportista: " + transportista + "</b> <br><br>";
            //tabla_transporte += "<div>  <b> Camión:        " + camion + "</b> <br><br>";
            //tabla_transporte += "<div>  <b> Chofer:        " + chofer + "</b> <br><br>";

            string transportista_antes = sp_asginada_log.Rows[0]["nombre_trans"].ToString();
            string camion_antes = sp_asginada_log.Rows[0]["patente"].ToString();
            string chofer_antes = sp_asginada_log.Rows[0]["nombre_chofer"].ToString();

            string transportista_ahora = sp_asginada.Rows[0]["nombre_trans"].ToString();
            string camion_ahora = sp_asginada.Rows[0]["patente"].ToString();
            string chofer_ahora = sp_asginada.Rows[0]["nombre_chofer"].ToString();


            //FACTURAR Y DESPACHAR
            tabla_transporte += "<table class='' border='1' cellspacing='0' cellpxdding='0' width='634' style='width:100% !important;'> ";
            tabla_transporte += " <tbody> ";
            tabla_transporte += "     <tr> ";
            tabla_transporte += "         <td colspan = '2' style='padding:0cm 0cm 0cm 0cm'> ";
            tabla_transporte += "             <p class=''><b><span style = 'COLOR:GREEN;font-size:13.5pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > AHORA</span></b><u></u><u></u></p> ";
            tabla_transporte += "         </td> ";
            tabla_transporte += "         <td colspan = '2' style='padding:0cm 0cm 0cm 0cm'> ";
            tabla_transporte += "             <p class=''><b><span style = 'COLOR:RED;font-size:13.5pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > ANTES</span></b><u></u><u></u></p> ";
            tabla_transporte += "         </td> ";
            tabla_transporte += "     </tr> ";
            tabla_transporte += "     <tr> ";
            tabla_transporte += "         <td width = '15%' style='width:15.0%;padding:0cm 0cm 0cm 0cm'> ";
            tabla_transporte += "             <p class=''><b><span style = 'font-size:11pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' >Transportista</span></b><u></u><u></u></p> ";
            tabla_transporte += "         </td> ";
            tabla_transporte += "         <td width = '35%' style='width:35.0%;padding:0cm 0cm 0cm 0cm'> ";
            tabla_transporte += "             <p class=''><b><span style = 'font-size:11pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' >" + transportista_ahora + "</span></b><u></u><u></u></p> ";
            tabla_transporte += "         </td> ";
            tabla_transporte += "         <td width = '20%' style= 'width:20.0%;padding:0cm 0cm 0cm 0cm' > ";
            tabla_transporte += "             <p class=''><b><span style = 'font-size:11pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' >Transportista</span></b><u></u><u></u></p> ";
            tabla_transporte += "         </td> ";
            tabla_transporte += "         <td width = '30%' style='width:30.0%;padding:0cm 0cm 0cm 0cm'> ";
            tabla_transporte += "             <p class=''><b><span style = 'font-size:11pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' >" + transportista_antes + " </span></b><u></u><u></u></p> ";
            tabla_transporte += "         </td> ";
            tabla_transporte += "     </tr> ";
            tabla_transporte += "     <tr> ";
            tabla_transporte += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            tabla_transporte += "             <p class=''><b><span style = 'font-size:11pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Camión </span></b><u></u><u></u></p> ";
            tabla_transporte += "         </td> ";
            tabla_transporte += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            tabla_transporte += "             <p class=''><b><span style = 'font-size:11pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + camion_ahora + "  </span></b><u></u><u></u></p>";
            tabla_transporte += "         </td> ";
            tabla_transporte += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            tabla_transporte += "             <p class=''><b><span style = 'font-size:11pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Camión </span></b><u></u><u></u></p> ";
            tabla_transporte += "         </td> ";
            tabla_transporte += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            tabla_transporte += "             <p class=''><b><span style = 'font-size:11pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' >" + camion_antes + "</span></b><u></u><u></u></p> ";
            tabla_transporte += "         </td> ";
            tabla_transporte += "     </tr> ";
            tabla_transporte += "     <tr> ";
            tabla_transporte += "         <td style = 'padding:0cm 0cm 0cm 0cm' > ";
            tabla_transporte += "             <p class=''><b><span style = 'font-size:11pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Chofer </span></b><u></u><u></u></p> ";
            tabla_transporte += "         </td> ";
            tabla_transporte += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            tabla_transporte += "             <p class=''><b><span style = 'font-size:11pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + chofer_ahora + "</span></b><u></u><u></u></p> ";
            tabla_transporte += "         </td> ";
            tabla_transporte += "         <td style = 'padding:0cm 0cm 0cm 0cm' > ";
            tabla_transporte += "             <p class=''><b><span style = 'font-size:11pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Chofer </span></b><u></u><u></u></p> ";
            tabla_transporte += "         </td> ";
            tabla_transporte += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            tabla_transporte += "             <p class=''><b><span style = 'font-size:11pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + chofer_antes + " </span></b><u></u><u></u></p> ";
            tabla_transporte += "         </td> ";
            tabla_transporte += "     </tr> ";
            tabla_transporte += "</tbody> ";
            tabla_transporte += "</table> ";

            string fecha_re_pla = sp_asginada.Rows[0]["fecha_Despacho"].ToString();
            string fecha_pla = sp_asginada_log.Rows[0]["fecha_Despacho"].ToString();

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
            double total_neto = 0;
            foreach (DataRow dr in produ_desch.Rows)
            {

                string tipo_unidad = dr["SP_CodUnVenta"].ToString().Replace(",000", "");
                if (tipo_unidad == "GxDef")
                {
                    tipo_unidad = "Otra";
                }
                double despachado = 0;
                try
                {
                    despachado = Convert.ToDouble(dr["despachado"].ToString().Replace(",000", ""));
                }
                catch
                {

                }
                if (tipo_unidad == "TON")
                {
                    despachado = despachado / 1000;
                }

                tabla += "<tr>";
                tabla += "<td>" + dr["codproducto"].ToString() + "</td>";
                tabla += "<td>" + dr["SP_descproducto"].ToString().Replace(",000", "") + "</td>";
                tabla += "<td>" + Math.Round(despachado, 2) + "</td>";
                tabla += "<td>" + tipo_unidad + "</td>";
                tabla += "<td>" + dr["SP_preciounitario"].ToString().Replace(",000", "") + "</td>";
                tabla += "<td>" + dr["SP_descuento"].ToString().Replace(",000", "") + "</td>";
                tabla += "<td>" + dr["SP_preciounitariofinal"].ToString().Replace(",000", "") + "</td>";
                double monto_neto = 0;
                try
                {
                    monto_neto = Convert.ToDouble(dr["montonetofinal"].ToString());
                    total_neto += monto_neto;
                }
                catch
                {

                }
                tabla += "<td>" + Base.monto_format2(monto_neto) + "</td>";
                tabla += "</tr>";
            }
            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");

            string cliente2 = sp_asginada_log.Rows[0]["nombrecliente"].ToString();

            DataTable dat_sp = ReporteRNegocio.VM_LISTAR_SP_2(" where coddocumento = '" + Session["coddocumento"].ToString() + "'");
            DataTable dat_detalle_Sp = ReporteRNegocio.VM_listar_detalle_sp_2(" where coddocumento = '" + Session["coddocumento"].ToString() + "'");
            string SP_formato = Base.crear_sp_formato(dat_sp, dat_detalle_Sp);

            MailMessage email = new MailMessage();
            //CORREO-CAMBIAR
            //email.To.Add(new MailAddress("esteban.godoy15@gmail.com"));
            email.To.Add(new MailAddress("informatica@soprodi.cl"));
            email.From = new MailAddress("informes@soprodi.cl");

            email.Subject = "RE-PLANIFICADA SP  " + Session["coddocumento"].ToString() + " " + cliente2 + "( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

            //CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC
            string EmailVendedor = dat_sp.Rows[0]["EmailVendedor"].ToString();

            //if (Session["SW_PERMI"].ToString() == "1")
            //{
            //    string correo_vend = ReporteRNegocio.trae_correos_bodega(dat_sp.Rows[0]["CodBodega"].ToString().Trim(), "ABARROTES");

            //    if (EmailVendedor != "" && correo_vend != "")
            //    {
            //        EmailVendedor += ",";
            //    }
            //    EmailVendedor += correo_vend;
            //    EmailVendedor = EmailVendedor.Replace(";", ",");
            //    if (EmailVendedor.Trim() == "")
            //    {
            //        email.CC.Add("mazocar@soprodi.cl, rmc@soprodi.cl, gmorales@soprodi.cl, pcataldo@soprodi.cl");
            //    }
            //    else
            //    {
            //        email.CC.Add("mazocar@soprodi.cl, rmc@soprodi.cl, gmorales@soprodi.cl, pcataldo@soprodi.cl, " + EmailVendedor);
            //    }
            //}
            //else
            //{
            //    string correo_vend = ReporteRNegocio.trae_correos_bodega(dat_sp.Rows[0]["CodBodega"].ToString().Trim(), "GRANOS");
            //    if (EmailVendedor != "" && correo_vend != "")
            //    {
            //        EmailVendedor += ",";
            //    }
            //    EmailVendedor += correo_vend;
            //    EmailVendedor = EmailVendedor.Replace(";", ",");
            //    if (EmailVendedor.Trim() == "")
            //    {
            //        email.CC.Add("MRAMIREZ@soprodi.cl");
            //    }
            //    else
            //    {
            //        email.CC.Add("MRAMIREZ@soprodi.cl, " + EmailVendedor);
            //    }
            //}


            string grupo = "";
            try
            {
                if (EmailVendedor != "")
                {
                    EmailVendedor += ",";
                }

                if (Session["SW_PERMI"].ToString() == "1")
                {
                    grupo = "'ABARROTES'";
                }
                else
                {
                    grupo = "'GRANOS'";
                }
                string correos_bodega = ReporteRNegocio.trae_correos_bodega(dat_sp.Rows[0]["CodBodega"].ToString().Trim(), grupo).Replace(";", ",");

                if (correos_bodega != "")
                {
                    EmailVendedor += correos_bodega;
                }
                else
                {
                    EmailVendedor = EmailVendedor.Trim();
                    EmailVendedor = EmailVendedor.Substring(0, EmailVendedor.Length - 1);
                }
            }
            catch { }

            if (Session["SW_PERMI"].ToString() == "1")
            {
                if (EmailVendedor.Trim() == "")
                {
                    email.CC.Add("mazocar@soprodi.cl, rmc@soprodi.cl, gmorales@soprodi.cl, pcataldo@soprodi.cl");
                }
                else
                {
                    email.CC.Add("mazocar@soprodi.cl, rmc@soprodi.cl, gmorales@soprodi.cl, pcataldo@soprodi.cl, " + EmailVendedor);
                }
            }
            else
            {
                if (EmailVendedor.Trim() == "")
                {
                    email.CC.Add("MRAMIREZ@soprodi.cl");
                }
                else
                {
                    email.CC.Add("MRAMIREZ@soprodi.cl, " + EmailVendedor);
                }
            }


            string sp = Session["coddocumento"].ToString();
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
            //usuario que envia
            email.Body += "<div><span style='float: left;font-size:7pt;'>" + ReporteRNegocio.corr_usuario(User.Identity.Name).Rows[0][0] + " </span></div>";

            email.Body += "<div><img src='http://a58.imgup.net/Sopro4d9d.png' style='    float: right;     width: 90px;'> </div><br><br><br>";
            email.Body += "<div> <p style='font-size: 22px;' ><b>RE-PLANIFICACIÓN </b></p><br> <br>  <b> </b>";
            email.Body += "<div>  <b style='font-size: 20px;color: #d25557;'> SP: " + sp + "</b> <br><br>";

            //email.Body += "<div>  <b style='color: red;'>   Transporte (ANTES): " + transporte_pla + "</b> <br><br>";
            //email.Body += "<div>  <b style='color: green;'> Transporte (AHORA): " + transporte_re_pla + "</b> <br><br>";
            email.Body += tabla_transporte;

            email.Body += "<div> Se ha re-planificiado con fecha <b style='color: green;'>(AHORA) " + fecha_re_pla.Replace(" 0:00:00", "") + "</b>  ///  <b style='color: red;'>(ANTES) " + fecha_pla.Replace(" 0:00:00", "") + "</b> el siguiente detalle de SP:  <br><br>";
            email.Body += tabla;
            //email.Body += "<div>  <b> Total Kg: " + total_kg.Replace(",000", "") + "</b> <br><br>";
            //email.Body += "<div>  <b> Total Neto: " + Base.monto_format2(total_neto) + "</b> <br><br>";
            //email.Body += "<div>  <b> I.V.A.: " + Base.monto_format2(total_neto * 0.19) + "</b> <br><br>";
            //email.Body += "<div>  <b> TOTAL: " + Base.monto_format2((total_neto * 0.19) + total_neto) + "</b> <br><br><br><br>";

            email.Body += "<div>  <b style='background-color: yellow;'> Observación      : " + obs + "</b> <br><br>";

            email.Body += SP_formato;

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
                string n_file = sp.Trim() + ".pdf";
                string pdfPath = Server.MapPath(@"~\PDFs\") + n_file;
                Base.crear_sp_pdf(sp, pdfPath);
                System.IO.FileInfo toDownload = new System.IO.FileInfo(pdfPath);
                if (toDownload.Exists)
                {
                    email.Attachments.Add(new System.Net.Mail.Attachment(pdfPath));
                }
                smtp.Send(email);
                email.Dispose();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>alert('CORREO ENVIADO!');</script>", false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "CORREO_PLANI", "<script>alert('ERROR AL ENVIAR CORREO!');</script>", false);
                //lb_mensaj.Text = "Error al enviar ";
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
            //    string n_file = sp.Trim() + ".pdf";
            //    string pdfPath = Server.MapPath(@"~\PDFs\") + n_file;
            //    Base.crear_sp_pdf(sp, pdfPath);

            //    System.IO.FileInfo toDownload = new System.IO.FileInfo(pdfPath);
            //    if (toDownload.Exists)
            //    {
            //        email.Attachments.Add(new System.Net.Mail.Attachment(pdfPath));
            //    }
            //    smtp.Send(email);
            //    email.Dispose();

            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>alert('CORREO ENVIADO!');</script>", false);

            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "CORREO_PLANI", "<script>alert('ERROR AL ENVIAR CORREO!');</script>", false);
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

            tabla += "<th>Cant.SP</th>";
            tabla += "<th>Cant.Planificada</th>";
            tabla += "<th>Cant.Facturado</th>";
            //tabla += "<th>TipoUnidad</th>";
            //tabla += "<th>PrecioUnitario</th>";
            //tabla += "<th>Descuento</th>";
            //tabla += "<th>PrecioUnitarioFinal</th>";
            //tabla += "<th>Neto</th>";


            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";

            foreach (DataRow dr in dt.Rows)
            {
                tabla += "<tr>";
                tabla += "<td>" + dr["codproducto"].ToString() + "</td>";
                tabla += "<td>" + dr["SP_descproducto"].ToString() + "</td>";
                tabla += "<td>" + dr["ThxCantidad_SP"].ToString().Replace(",000000", "") + "</td>";
                tabla += "<td>" + dr["despachado"].ToString() + "</td>";
                tabla += "<td>" + dr["facturado"].ToString() + "</td>";

                tabla += "</tr>";
            }
            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");



            return tabla;
        }



        [WebMethod]
        public static string CargarEventolog(string id)
        {
            DataTable dt = new DataTable();

            dt = ReporteRNegocio.log_re_planificar(id);


            string tabla = "";

            tabla += "SP";
            tabla += "<table class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";
            tabla += "<th>Sp</th>";
            tabla += "<th>Cliente</th>";
            tabla += "<th>Vendedor</th>";
            //tabla += "<th>NomTransporte</th>";
            //tabla += "<th>DisponibleCamión</th>";
            //tabla += "<th>FechaDespacho</th>";
            tabla += "<th>Bodega</th>";
            tabla += "<th>DescBodega</th>";

            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            foreach (DataRow dr in dt.Rows)
            {
                tabla += "<tr>";
                tabla += "<td>" + dr["coddocumento"].ToString() + "</td>";
                tabla += "<td>" + dr["nombrecliente"].ToString() + "</td>";
                tabla += "<td>" + dr["nombrevendedor"].ToString() + "</td>";
                //tabla += "<td>" + dr["nombre_transporte_todo"].ToString() + "</td>";
                //tabla += "<td>" + dr["disponible_2"].ToString() + "</td>";
                //tabla += "<td>" + dr["fecha_despacho2"].ToString() + "</td>";
                tabla += "<td>" + dr["codbodega"].ToString() + "</td>";
                tabla += "<td>" + dr["descbodega"].ToString() + "</td>";
                tabla += "</tr>";
                break;
            }
            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");

            tabla += "RE-PLANIFICADOS";
            tabla += "<table class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";
            //tabla += "<th>Sp</th>";
            //tabla += "<th>Cliente</th>";
            //tabla += "<th>Vendedor</th>";
            tabla += "<th>NomTransporte</th>";
            tabla += "<th>DisponibleCamión</th>";
            tabla += "<th>FechaDespacho</th>";
            //tabla += "<th>Bodega</th>";
            //tabla += "<th>DescBodega</th>";

            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            foreach (DataRow dr in dt.Rows)
            {
                tabla += "<tr>";
                //tabla += "<td>" + dr["coddocumento"].ToString() + "</td>";
                //tabla += "<td>" + dr["nombrecliente"].ToString() + "</td>";
                //tabla += "<td>" + dr["nombrevendedor"].ToString() + "</td>";
                tabla += "<td>" + dr["nombre_transporte_todo"].ToString() + "</td>";
                tabla += "<td>" + dr["disponible_2"].ToString() + "</td>";
                tabla += "<td>" + dr["fecha_despacho2"].ToString() + "</td>";
                //tabla += "<td>" + dr["codbodega"].ToString() + "</td>";
                //tabla += "<td>" + dr["descbodega"].ToString() + "</td>";


                tabla += "</tr>";

            }
            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");



            return tabla;
        }



        protected void G_DETALLE_PLANIFICADO_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btn_plus_trans_ServerClick(object sender, EventArgs e)
        {

        }

        protected void btn_plus_camion_ServerClick(object sender, EventArgs e)
        {

        }

        protected void btn_validar_factura_Click(object sender, EventArgs e)
        {

            DataTable DT_facturas = (DataTable)Session["dt_facturas_camion"];
            int estado_factura = ReporteRNegocio.validar_factura_solomon(tx_factura.Text);


            DataRow row;
            if (estado_factura > 0)
            {
                bool existe_factura = false;
                foreach (DataRow r in DT_facturas.Rows)
                {
                    if (tx_factura.Text.Trim() == r[0].ToString().Trim())
                    {
                        existe_factura = true;
                        break;
                    }
                }
                if (!existe_factura)
                {
                    row = DT_facturas.NewRow();
                    row[0] = tx_factura.Text.Trim();
                    row[1] = "OK";
                    DT_facturas.Rows.Add(row);

                    string sps = Session["sps_"].ToString();
                    string ids_ = Session["ids_"].ToString();
                    string cod_trans = Session["cod_trans"].ToString();
                    string cod_camion = Session["cod_camion"].ToString();
                    string cod_chofer = Session["cod_chofer"].ToString();
                    string dia_plani = Session["dia_plani"].ToString();

                    //aca insert enc
                    //int existe_id_cierre = Convert.ToInt32(Session["id_cierre_enc"]);
                    //aca insert det
                    Session["dt_facturas_camion"] = DT_facturas;

                    G_FACTURAS.DataSource = DT_facturas;
                    G_FACTURAS.DataBind();


                    tx_factura.Text = "";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert1", "<script language='javascript'>  alert('YA AGREGADA');  </script>", false);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert1", "<script language='javascript'>  alert('FACTURA NO EXISTE');  </script>", false);
            }
        }

        protected void G_FACTURAS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (Session["ya_esta_cerrado"].ToString() == "SI")
                {

                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                    try
                    {
                        string año_factura = ReporteRNegocio.trae_año_factura(e.Row.Cells[1].Text);
                        año_factura = año_factura.Substring(0, 4);
                        string script = string.Format("javascript:fuera3(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[1].Text), encriptador.EncryptData(año_factura));
                        e.Row.Cells[1].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[1].Text + " </a>";

                        e.Row.Cells[0].Visible = false;
                        G_FACTURAS.HeaderRow.Cells[0].Visible = false;
                    }
                    catch { }
                }

            }
        }

        protected void refresh_edit_(object sender, EventArgs e)
        {
            string bodega_plani = HttpContext.Current.Session["bodega_plani"].ToString();
            string trans_ = HttpContext.Current.Session["trans_"].ToString();
            string camion = HttpContext.Current.Session["camion"].ToString();
            string chofer = HttpContext.Current.Session["chofer"].ToString();

            string respt = "";
            try
            {
                cargar_bodega_3(bodega_plani);
                cargar_trans(bodega_plani, trans_);

                try
                {
                    cargar_camion(trans_, camion);
                    cargar_chofer(trans_, chofer);
                }
                catch { }
            }
            catch { respt = "Error "; }

            string coddocum = "";
            coddocum = Session["coddocumento"].ToString();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "chosen", "<script language='javascript'>  load_chosen_combos();  </script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "titulo_modal", "<script language='javascript'>  modal_unidad_titulo('" + coddocum + "');  </script>", false);
        }

        protected void btn_cerrar_camion_Click(object sender, EventArgs e)
        {

            int existe_id_cierre = ReporteRNegocio.existe_cierre_camion(Session["cod_trans"].ToString(), Session["cod_camion"].ToString(), Session["cod_chofer"].ToString(), Session["sps_"].ToString(), Session["dia_plani"].ToString());
            Session["id_cierre_enc"] = existe_id_cierre;

            //aca insert det
            DataTable DT_facturas = (DataTable)Session["dt_facturas_camion"];
            bool TODO_OK = true;
            foreach (DataRow r in DT_facturas.Rows)
            {
                string factura = r[0].ToString();

                string ok_insert = ReporteRNegocio.insert_det_cierre_camion(existe_id_cierre, factura);
                if (ok_insert != "OK")
                {
                    TODO_OK = false;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "chosen", "<script language='javascript'>  alert('ERROR AGREGAR FACTURAS');  </script>", false);
                    break;
                }
            }
            if (TODO_OK)
            {
                if (DT_facturas.Rows.Count < 0)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "chosen", "<script language='javascript'>  alert('Debe agregar facturas');  </script>", false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "chosen", "<script language='javascript'>  alert('OK');  </script>", false);
                }
            }
        }

        protected void G_FACTURAS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {

                //string id_ = (G_FACTURAS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                string factura = (G_FACTURAS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());


                //aca insert det
                DataTable DT_facturas = (DataTable)Session["dt_facturas_camion"];

                foreach (DataRow r in DT_facturas.Rows)
                {
                    if (r[0].ToString() == factura)
                    {
                        DT_facturas.Rows.Remove(r);
                        Session["dt_facturas_camion"] = DT_facturas;

                        G_FACTURAS.DataSource = DT_facturas;
                        G_FACTURAS.DataBind();
                        break;
                    }
                }


            }

        }

        protected void enviar_Correo_camion_Click(object sender, ImageClickEventArgs e)
        {
            cuerpo_correo_camion();
            string correos = t_correos.Text.Replace(",", ";");
            MailMessage email = new MailMessage();

            List<string> correos_list = correos.Split(';').ToList<string>();

            foreach (string corr in correos_list)
            {
                if (corr.Trim() != "")
                {
                    email.To.Add(new MailAddress(corr));
                }
            }

            //email.To.Add(new MailAddress(correos));
            //email.To.Add(new MailAddress("ESTEBAN.GODOY15@GMAIL.COM"));

            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "DESPACHOS  -  " + Session["fecha_transporte"].ToString();
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
            //usuario que envia
            email.Body += "<div><span style='float: left;font-size:7pt;'>" + ReporteRNegocio.corr_usuario(User.Identity.Name).Rows[0][0] + " </span></div>";
            email.Body += "<div><img src='http://a58.imgup.net/Sopro4d9d.png' style='    float: right;     width: 90px;'> </div><br><br><br>";
            string transportista = Session["tabla_transporte"].ToString();
            email.Body += "<div> <p style='font-size: 22px;' ><b>DÍA DE CARGA   -  " + Session["fecha_transporte"].ToString() + " </b><br> <br>  <b> </b>";
            email.Body += "<div> <p style='font-size: 22px;' ><b>" + transportista + " </b><br> <br>  <b> </b>";

            email.Body += "<div>   <b> " + Session["tabla"].ToString() + "</b> <br><br>";
            email.Body += "<div>   <b> " + Session["tabla_obs"].ToString() + "</b> <br><br>";

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
            //smtp.Credentials = new NetworkCredential("soc@soprodi.cl", "soprodi2019");
            try
            {
                email = subir_y_adjuntar_archivos(email);
                List<string> sps = (List<string>)Session["sps_LIST"];
                foreach (string sp in sps)
                {
                    string n_file = sp.Trim() + ".pdf";
                    string pdfPath = Server.MapPath(@"~\PDFs\") + n_file;
                    Base.crear_sp_pdf(sp, pdfPath);
                    System.IO.FileInfo toDownload = new System.IO.FileInfo(pdfPath);
                    if (toDownload.Exists)
                    {
                        email.Attachments.Add(new System.Net.Mail.Attachment(pdfPath));
                    }
                }
                smtp.Send(email);
                email.Dispose();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "correo_bodega1", "<script>alert('CORREO ENVIADO!');</script>", false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "correo_bodega", "<script>alert('ERROR AL ENVIAR CORREO! "+ex.Message+"');</script>", false);
            }

            /////desde gmail

            //email.IsBodyHtml = true;
            //email.Priority = MailPriority.Normal;
            //email.BodyEncoding = System.Text.Encoding.UTF8;
            //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            //smtp.EnableSsl = true;
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.UseDefaultCredentials = false;
            //smtp.Credentials = new NetworkCredential("informes.soprodi@gmail.com", "galaxia1234");
            //try
            //{
            //    email = subir_y_adjuntar_archivos(email);
            //    List<string> sps = (List<string>)Session["sps_LIST"];
            //    foreach (string sp in sps)
            //    {
            //        string n_file = sp.Trim() + ".pdf";
            //        string pdfPath = Server.MapPath(@"~\PDFs\") + n_file;
            //        Base.crear_sp_pdf(sp, pdfPath);
            //        System.IO.FileInfo toDownload = new System.IO.FileInfo(pdfPath);
            //        if (toDownload.Exists)
            //        {
            //            email.Attachments.Add(new System.Net.Mail.Attachment(pdfPath));
            //        }
            //    }
            //    smtp.Send(email);
            //    email.Dispose();
            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "correo_bodega1", "<script>alert('CORREO ENVIADO!');</script>", false);
            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "correo_bodega", "<script>alert('ERROR AL ENVIAR CORREO!');</script>", false);
            //}
        }

        private void cuerpo_correo_camion()
        {
            string cod_trans = l_transpor.Text;
            string ids = agregra_comillas(Session["ids_"].ToString());
            string sps = agregra_comillas(Session["sps_"].ToString());
            DataTable produ_desch = ReporteRNegocio.lista_det_sp_asignada_in(ids);
            DataTable sp_encabezado_os = ReporteRNegocio.lista_enc_sp_asignada_in(sps, ids);

            string tabla_transporte = "";

            tabla_transporte += "<div>  <b> Transportista: " + Session["transportista"].ToString() + "</b> <br><br>";
            tabla_transporte += "<div>  <b> Camión:        " + Session["camion"].ToString() + "</b> <br><br>";
            tabla_transporte += "<div>  <b> Chofer:        " + Session["chofer"].ToString() + "</b> <br><br>";

            Session["tabla_transporte"] = tabla_transporte;

            string tabla = "";
            tabla += "<table class=\"table fill-head table-bordered\" style=\"width:100%;\">";
            tabla += "<thead class=\"test\" style=\"background-color:rgb(156,205,249)\">";
            tabla += "<tr>";

            tabla += "<th>SP</th>";
            tabla += "<th>Cliente</th>";
            tabla += "<th>OrdenCargar</th>";

            tabla += "<th>Código</th>";
            tabla += "<th>Nombre</th>";
            tabla += "<th>CantDespachado</th>";
            tabla += "<th>TipoUnidad</th>";
            //tabla += "<th>PrecioUnitario</th>";
            //tabla += "<th>Descuento</th>";
            //tabla += "<th>PrecioUnitarioFinal</th>";
            //tabla += "<th>Neto</th>";

            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            double total_neto = 0;
            string sp = "";
            string color_1 = "background-color:bisque;";
            string color_2 = "background-color:darkseagreen;";
            string color = "";
            foreach (DataRow dr in produ_desch.Rows)
            {
                string tipo_unidad = dr["SP_CodUnVenta"].ToString().Replace(",000", "");

                if (dr["coddocumento"].ToString() != sp)
                {
                    if (color.Contains("bisque"))
                    { color = color_2; }
                    else
                    { color = color_1; }
                }
                else
                {
                    sp = dr["coddocumento"].ToString();
                }
                tabla += "<tr style='" + color + "'>";

                if (tipo_unidad == "GxDef")
                {
                    tipo_unidad = "Otra";
                }
                double despachado = 0;
                try
                {
                    despachado = Convert.ToDouble(dr["despachado"].ToString().Replace(",000", ""));
                }
                catch
                {

                }
                if (tipo_unidad == "TON")
                {
                    despachado = despachado / 1000;
                }
                //tabla += "<tr>";
                tabla += "<td>" + dr["coddocumento"].ToString() + "</td>";
                tabla += "<td>" + dr["nombrecliente"].ToString() + "</td>";
                tabla += "<td>" + dr["orden_cargar"].ToString() + "</td>";
                tabla += "<td>" + dr["codproducto"].ToString() + "</td>";
                tabla += "<td>" + dr["SP_descproducto"].ToString().Replace(",000", "") + "</td>";
                tabla += "<td>" + Math.Round(despachado, 2) + "</td>";
                tabla += "<td>" + tipo_unidad + "</td>";
                //tabla += "<td>" + dr["SP_preciounitario"].ToString().Replace(",000", "") + "</td>";
                //tabla += "<td>" + dr["SP_descuento"].ToString().Replace(",000", "") + "</td>";
                //tabla += "<td>" + dr["SP_preciounitariofinal"].ToString().Replace(",000", "") + "</td>";
                //double monto_neto = 0;
                //try
                //{
                //    monto_neto = Convert.ToDouble(dr["montonetofinal"].ToString());
                //    total_neto += monto_neto;
                //}
                //catch
                //{

                //}
                //tabla += "<td>" + Base.monto_format2(monto_neto) + "</td>";
                tabla += "</tr>";
            }
            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");

            Session["tabla"] = tabla;


            string tabla2 = "";
            tabla2 += "<table border=1 class=\"table fill-head table-bordered\" style=\"width:100%;\">";
            tabla2 += "<thead class=\"test\" style=\"background-color:rgb(156,205,249)\">";
            tabla2 += "<tr>";

            tabla2 += "<th colspan=1;>SP</th>";
            tabla2 += "<th colspan=1;>CLIENTE</th>";
            tabla2 += "<th colspan=5;>NOTA LIBRE</th>";

            tabla2 += "</tr>";
            tabla2 += "</thead>";
            tabla2 += "<tbody>";
            foreach (DataRow dr1 in sp_encabezado_os.Rows)
            {
                tabla2 += "<tr>";
                string sp1 = dr1["coddocumento"].ToString().Replace(",000", "");
                string notalibre = dr1["observacion"].ToString();
                string nombrecliente = dr1["nombrecliente"].ToString();
                tabla2 += "<td colspan=1;>" + sp1 + "</td>";
                tabla2 += "<td colspan=1;>" + nombrecliente + "</td>";
                tabla2 += "<td colspan=5;>" + notalibre + "</td>";
                tabla2 += "</tr>";
            }
            tabla2 += "</tbody>";
            tabla2 += "</table>";
            tabla2 = tabla2.Replace("'", "");
            Session["tabla_obs"] = tabla2;
        }

        private MailMessage subir_y_adjuntar_archivos(MailMessage email)
        {
            if (fu_archivos_camion.HasFile)
            {
                foreach (HttpPostedFile uploadedFile in fu_archivos_camion.PostedFiles)
                {
                    string ServerPath = HttpContext.Current.Server.MapPath("~").ToString();
                    string subPath = ServerPath + "CAMIONES";
                    bool exists = System.IO.Directory.Exists(subPath);
                    if (!exists)
                    {
                        System.IO.Directory.CreateDirectory(subPath);
                    }
                    var fileSavePath = System.IO.Path.Combine(subPath, uploadedFile.FileName);
                    try
                    {
                        uploadedFile.SaveAs(fileSavePath);
                    }
                    catch
                    {
                        File.Delete(fileSavePath);
                        uploadedFile.SaveAs(fileSavePath);
                        //ya existe el archivo
                    }
                    System.IO.FileInfo toDownload = new System.IO.FileInfo(fileSavePath);
                    if (toDownload.Exists)
                    {
                        email.Attachments.Add(new System.Net.Mail.Attachment(fileSavePath));
                    }

                }
                //fu_archivos_camion.Dispose();

                //foreach (HttpPostedFile uploadedFile in fu_archivos_camion.PostedFiles)
                //{
                //    string ServerPath = HttpContext.Current.Server.MapPath("~").ToString();
                //    string subPath = ServerPath + "CAMIONES";
                //    var fileSavePath = System.IO.Path.Combine(subPath, uploadedFile.FileName);

                //}
            }
            return email;
        }
    }
}