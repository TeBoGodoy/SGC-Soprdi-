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
using System.Data.SqlClient;
using SoprodiApp.Entities;
using SoprodiApp.entidad;
using SoprodiApp.BusinessLayer;
using CRM.Entities;
using CRM.BusinessLayer;
using ThinxsysFramework;
//using Microsoft.Office.Interop.Word;

namespace SoprodiApp
{
    public partial class ABARROTES_SP : System.Web.UI.Page
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
            //this.Form.DefaultButton = this.Button1.UniqueID;
            //Page.RegisterRedirectOnSessionEndScript();
            page = this.Page;
            if (!IsPostBack)
            {

                string sw_proviene = Request.QueryString["F"];

                if (sw_proviene != null)
                {
                    string grupo_ = Request.QueryString["G"];

                    Session["SW_PERMI"] = "1";

                    string datos = Request.QueryString["F"];

                    Session["SW_FILTRAR_PRODUCTO"] = "SI";
                    Session["codvendedor"] = "";
                    Session["WHERE"] = " where 1=1 ";

                    ///PRODUCTOS DEVUELVE DT ... 
                    //string codigos_documentos = ReporteRNegocio.trae_codigos_sincronizados_por_producto(cod_prod);

                    Session["WHERE"] += " and Convert(datetime,b.fechadespacho,103) = Convert(datetime,'" + datos + "',103) and ISNUMERIC( isnull(d.coddocumento,'no')  ) <> 1 and b.DescEmisor in (" + Base.agrega_comillas(grupo_) + ")";
                    //Session["estados_param"] = "'10S', '10', '10P'";

                    ImageClickEventArgs e2x = new ImageClickEventArgs(1, 2);
                    Button1_Click(e, e2x);

                    //Session["SW_FILTRAR_PRODUCTO"] = "NO";

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teadsaeee", "<script>  var elem3 = document.getElementById('div_superior'); elem3.style.display = 'none'; </script>", false);
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
                    titulo2.HRef = "Menu_Planificador.aspx?s=1";
                    titulo2.InnerText = "Planificador";
                }
                else if (Session["SW_PERMI"].ToString() == "2")
                {
                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=2'>Granos</a>";
                    titulo2.HRef = "Menu_Planificador.aspx?s=2";
                    titulo2.InnerText = "Planificador";
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
                USER = User.Identity.Name.ToString();
                l_usuario_.Text = USER;
                //DateTime t = DateTime.Now.AddDays(-1);
                //DateTime t2 = t;
                //////t = new DateTime(t.Year, t.Month - 6, 1);               
                //txt_desde.Text = t.ToShortDateString();
                //txt_hasta.Text = t2.ToShortDateString().Substring(0, 2);


                DateTime t = DateTime.Now.AddDays(-4);
                DateTime t2 = DateTime.Now;
                //////t = new DateTime(t.Year, t.Month - 6, 1);    

                txt_desde.Text = t.ToShortDateString();
                txt_hasta.Text = t2.ToShortDateString();
                //txt_desde.Text = "20/09/2017";
                //txt_hasta.Text = "20/09/2017";
                string es_vendedor = ReporteRNegocio.esvendedor(USER);
                ImageClickEventArgs ex = new ImageClickEventArgs(1, 2);
                b_Click(sender, ex);
                Button1_Click(sender, ex);
            }
            else
            {
                JQ_Datatable();
            }
        }
        public void relojito(bool x)
        {
            string xx = x ? "true" : "false";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "relojito", "<script language='javascript'>relojito(" + xx + ");</script>", false);
        }
        ///ACA VERSION 04/06/2019

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            MakeAccessible(G_INFORME_TOTAL_VENDEDOR);
            MakeAccessible(G_DETALLE);
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

        [WebMethod]
        public static string cambia_tipo_pago_(string factura, string estado)
        {
            string aasdf = "";

            if (estado != "pendiente")
            {
                ReporteRNegocio.delete_estado_sp(factura);
                ReporteRNegocio.insert_estado_sp(factura, estado);
            }



            return "";
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

            Response.AddHeader("content-disposition", "attachment;filename=SOPRODI_ELEGIR_SP" + DateTime.Now.ToShortDateString() + ".xls");

            Response.Charset = "";

            // If you want the option to open the Excel file without saving than

            // comment out the line below

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.ContentType = "application/vnd.xls";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);


            Response.Write(stringWrite.ToString());

            Response.End();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
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
            CargarBodega();
            cargar_estado_SP();
            cargar_vendedor_SP();
            carga_estado_2();
            carga_estado_3();
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
        private void carga_estado_3()
        {
            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.crear_estado_3();
            dtv = dt.DefaultView;
            d_estado_3.DataSource = dtv;
            d_estado_3.DataTextField = "estado";
            d_estado_3.DataValueField = "estado";
            //d_vendedor_.SelectedIndex = -1;
            d_estado_3.DataBind();
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
            //d_grupo.DataSource = dtv;
            //d_grupo.DataTextField = "DescEmisor";
            //d_grupo.DataValueField = "DescEmisor";
            ////d_vendedor_.SelectedIndex = -1;
            //d_grupo.DataBind();
        }



        private void cargar_productos_no_kg(string clase)
        {
            ////                " and FechaFactura <= CONVERT(datetime,'" + HASTA + "',103) and user1 in (" + grupos + ")";

            //DataTable dt; DataView dtv = new DataView();
            //dt = ReporteRNegocio.listar_ALL_productos_stock_guion_2(" and b.stkunit <>'KGR' ", clase);
            //dtv = dt.DefaultView;
            //cb_productos_kg.DataSource = dtv;
            //cb_productos_kg.DataTextField = "descr";
            //cb_productos_kg.DataValueField = "invtid";
            ////d_vendedor_.SelectedIndex = -1;
            //cb_productos_kg.DataBind();
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
            lb_bodegas2.Text = "10, 40";
            foreach (ListItem item in d_bodega_2.Items)
            {

                if (lb_bodegas2.Text.Contains(item.Value.ToString()))
                {
                    item.Selected = true;
                }
            }
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
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Cells[0].Text = cont_det.ToString();
            //    cont_det++;
            //    string where_es = " where vendedor = '" + vendedor.Trim() + "' and nombrecliente like '" + cliente + "'  ";
            //    for (int i = 4; i < e.Row.Cells.Count; i++)
            //    {
            //        if (header_sum2)
            //        {
            //            G_DET_PRODUCTOS.HeaderRow.Cells[i].Text = G_DET_PRODUCTOS.HeaderRow.Cells[i].Text + "  (" + Convert.ToDouble(ReporteRNegocio.Facturación_Mes(G_DET_PRODUCTOS.HeaderRow.Cells[i].Text.Substring(0, 6), where_es)).ToString("N0") + ")";
            //        }
            //        if (i == e.Row.Cells.Count - 1) { header_sum2 = false; }

            //        double d;
            //        double.TryParse(e.Row.Cells[i].Text, out d);
            //        string aux = "";
            //        if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
            //        e.Row.Cells[i].Text = aux;
            //    }

            //}
        }
        GridView excel1 = new GridView();


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
            //Response.Clear();

            //Response.AddHeader("content-disposition", "attachment;filename=FECHAS_VP_INTRAN" + DateTime.Now.ToShortDateString() + ".xls");

            //Response.Charset = "";

            //// If you want the option to open the Excel file without saving than

            //// comment out the line below

            //Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //Response.ContentType = "application/vnd.xls";

            //System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            //System.Web.UI.HtmlTextWriter htmlWrite =
            //new HtmlTextWriter(stringWrite);

            //G_errores.RenderControl(htmlWrite);

            //Response.Write(stringWrite.ToString());

            //Response.End();

        }



        private void notificar_vendedor(string correo_vend, string select_scope, string sp, string fecha)
        {
            string aca = "";

            DataTable produ_desch = ReporteRNegocio.lista_det_sp_asignada(select_scope);

            string tabla = "";
            tabla += "<table class=\"table fill-head table-bordered\" style=\"width:100%;\">";
            tabla += "<thead class=\"test\" style=\"background-color:rgb(156,205,249)\">";
            tabla += "<tr>";
            tabla += "<th>Código</th>";
            tabla += "<th>Nombre</th>";
            tabla += "<th>TipoUnidad</th>";
            tabla += "<th>CantDespachado</th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            foreach (DataRow dr in produ_desch.Rows)
            {
                tabla += "<tr>";
                tabla += "<td>" + dr["codproducto"].ToString() + "</td>";
                tabla += "<td>" + dr["nomb"].ToString() + "</td>";
                tabla += "<td>" + dr["tipo_"].ToString() + "</td>";
                tabla += "<td>" + dr["despachado"].ToString() + "</td>";
                tabla += "</tr>";
            }
            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");

            enviar_email(tabla, correo_vend, sp, fecha);


            ScriptManager.RegisterStartupScript(Page, this.GetType(), "tee6ee", "<script> alert('SP ASIGNADA');</script>", false);
        }

        private void enviar_email(string html, string correo_vend, string sp, string fecha)
        {
            MailMessage email = new MailMessage();
            //email.To.Add(new MailAddress("esteban.godoy15@gmail.com"));
            email.To.Add(new MailAddress(correo_vend));
            email.From = new MailAddress("informes@soprodi.cl");
            string cliente_2 = Session["cliente"].ToString();
            email.Subject = "QUITADA SP Asignada " + sp + " " + cliente_2 + "( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";


            if (Session["SW_PERMI"].ToString() == "1")
            {
                email.CC.Add("esteban.godoy15@gmail.com, rmc@soprodi.cl");
            }
            else if (Session["SW_PERMI"].ToString() == "2")
            {
                email.CC.Add("esteban.godoy15@gmail.com");
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

            string fecha_emi = ReporteRNegocio.trae_fecha_emision_sp(sp);

            email.Body += "<div> Estimado :<br> <br>  <b> </b> <br><br>";
            email.Body += "<div>  <b> Fecha Emision: " + fecha_emi + "</b> <br><br>";
            email.Body += "<div> Se ha planificiado con fecha <b> " + fecha + "</b> el siguiente detalle de SP:  <br><br>";
            email.Body += html;
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

        private void enviar_email_desplanificado(string codDocumento, string fechaEmision, string codVendedor, string notaLibre, string codBodega, string fechaDespacho, string codMoneda, string montoNeto, string descEstadoDocumento, string facturas, string gxEstadoSync, string gxActualizado, string gxEnviadoERP, string nombreVendedor, string nombreCliente, string descBodega, string fechaCreacion, string valorTipoCambio, string limiteSeguro, string tipoCredito, string creditoDisponible, string creditoAutorizado, string emailVendedor)
        {
            throw new NotImplementedException();
        }

        private void enviar_email_cambio(string CodDocumento, string FechaEmision, string CodVendedor, string NotaLibre, string CodBodega, string FechaDespacho, string CodMoneda, string MontoNeto, string DescEstadoDocumento, string Facturas, string GxEstadoSync, string GxActualizado, string GxEnviadoERP, string NombreVendedor, string NombreCliente, string DescBodega, string FechaCreacion, string ValorTipoCambio, string LimiteSeguro, string TipoCredito, string CreditoDisponible, string CreditoAutorizado, string EmailVendedor)
        {


            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress("rmc@soprodi.cl"));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "SP Rechazada desde Sistema ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

            email.CC.Add(EmailVendedor + " , mazocar@soprodi.cl, jcorrea@soprodi.cl, gmorales@soprodi.cl, esteban.godoy15@gmail.com");

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

        public string sp_selector(string CodDocumento)
        {
            return @"javascript: sp_select(" + CodDocumento + ");";
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
            //string sp = tx_sp.Text;

            ////double count_procesadas = ReporteRNegocio.existe_sp(sp);

            ////if (count_procesadas == 0)
            ////{

            //DataTable dt_sp_encabe = ReporteRNegocio.ventamovil_sp_enc(sp);
            //DataTable dt_sp_detalle = ReporteRNegocio.ventamovil_sp_det(sp);

            //DataTable dt_sp_encabe_EXT = ReporteRNegocio.ventamovil_sp_enc_EXT(sp);
            //DataTable dt_sp_detalle_EXT = ReporteRNegocio.ventamovil_sp_det_EXT(sp);

            //string cadena_vm_thx = "Data Source=192.168.10.45;Initial Catalog=SoprodiVenta;Persist Security Info=True;User ID=sa;Password=Soprodi1234";

            //bool ok = true;

            //ok = borrar_sp(sp);

            //SqlBulkCopy bulkcopy = new SqlBulkCopy(cadena_vm_thx);
            //bulkcopy.DestinationTableName = "TrnDocumentoCabecera";
            //try
            //{
            //    bulkcopy.WriteToServer(dt_sp_encabe);
            //}
            //catch (Exception ex)
            //{
            //    ok = false;
            //    Console.Write(ex.Message);
            //}
            //SqlBulkCopy bulkcopy2 = new SqlBulkCopy(cadena_vm_thx);
            //bulkcopy2.DestinationTableName = "TrnDocumentoDetalle";
            //try
            //{
            //    bulkcopy2.WriteToServer(dt_sp_detalle);
            //}
            //catch (Exception ex)
            //{
            //    ok = false;
            //    Console.Write(ex.Message);
            //}
            ////ext_
            //SqlBulkCopy bulkcopy3 = new SqlBulkCopy(cadena_vm_thx);
            //bulkcopy3.DestinationTableName = "ext_TrnDocumentoCabecera";
            //try
            //{
            //    bulkcopy3.WriteToServer(dt_sp_encabe_EXT);
            //}
            //catch (Exception ex)
            //{
            //    ok = false;
            //    Console.Write(ex.Message);
            //}
            //SqlBulkCopy bulkcopy4 = new SqlBulkCopy(cadena_vm_thx);
            //bulkcopy4.DestinationTableName = "ext_TrnDocumentoDetalle";
            //try
            //{
            //    bulkcopy4.WriteToServer(dt_sp_detalle_EXT);
            //}
            //catch (Exception ex)
            //{
            //    ok = false;
            //    Console.Write(ex.Message);
            //}
            //ok = procesar_sp_traida(sp);
            //if (ok)
            //{
            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'> alert('Agregada//Actualizada SP') </script>", false);
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'> alert('Hubo un error') </script>", false);
            //}
            ////}
            ////else
            ////{


            ////    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'> alert('Existe SP') </script>", false);

            ////}


        }



        private bool procesar_sp_traida(string sp)
        {
            DataTable dt = new DataTable();
            //string cadena_thx = "Data Source=192.168.10.45;Initial Catalog=[new_thx];Persist Security Info=True;User ID=sa;Password=Soprodi1234";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select y.*, cast(Cantidad - isnull(CANTIDAD_SOLOMON,0) AS decimal(16,2)) as cantidad_falta  from(
       select   A.* ,
                                 case 
            when(select  isnull(sum(cantidad), null) from[new_thx].[dbo].[V_SP_FACTURA_THX] where
                producto = A.CodProducto and sp = A.CodDocumento group by producto) is NULL
                then '10S'
            WHEN(select  isnull(sum(cantidad), null) from[new_thx].[dbo].[V_SP_FACTURA_THX] where
                producto = A.CodProducto and sp = A.CodDocumento group by producto) <> A.Cantidad
                THEN '10P'
            ELSE
                '20'
                end as ESTADO,
            isnull((select  isnull(sum(cantidad), null) from[new_thx].[dbo].[V_SP_FACTURA_THX] where
                producto = A.CodProducto and sp = A.CodDocumento group by producto),0) AS CANTIDAD_SOLOMON
        from
       (select b.CodDocumento, b.codproducto, b.cantidad from[SoprodiVenta].[dbo].[VPEDIDODETALLE_THX]  b

         where b.CodEstadoDocumento in ('20')  and ISNUMERIC(b.codproducto) = 1 AND b.descproducto not like '%serv%' and b.descproducto not like '%descarga%'   and b.coddocumento = '" + sp + "') A) y " +
    "     union " +
    "      select LTRIM(RTRIM(b.CodDocumento)) AS CodDocumento, LTRIM(RTRIM(b.codproducto)) AS codproducto, cast(b.cantidad as decimal(15,3)) as cantidad,  " +
 " rtrim(ltrim(convert(varchar,b.CodEstadoDocumento))) as ESTADO, 0 as CANTIDAD_SOLOMON, case when CodEstadoDocumento in ('40','10') then cast(b.cantidad as decimal(15,3)) else 0 end as cantidad_falta  from[SoprodiVenta].[dbo].[VPEDIDODETALLE_THX]  b " +
" where ISNUMERIC(b.codproducto) = 1 and b.CodEstadoDocumento  not in ('20') " +
" AND b.descproducto not like '%serv%' and b.descproducto not like '%descarga%'  and b.coddocumento = '" + sp + "'";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }


            try

            {

                string ssqlconnectionstring = ConfigurationManager.ConnectionStrings["default"].ToString();

                //string sclearsql = "truncate table Sps_procesadas1";
                //SqlConnection sqlconn = new SqlConnection(ssqlconnectionstring);
                //SqlCommand sqlcmd = new SqlCommand(sclearsql, sqlconn);
                //sqlconn.Open();
                //sqlcmd.ExecuteNonQuery();
                //sqlconn.Close();


                SqlBulkCopy bulkcopy = new SqlBulkCopy(ssqlconnectionstring);
                bulkcopy.DestinationTableName = "Sps_procesadas1";
                try
                {
                    bulkcopy.WriteToServer(dt);
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                    return false;

                }

            }
            catch
            {
                return false;
            }
            return true;
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


        public bool es_visible(string descdocumento)
        {
            if (descdocumento != "Aprobado")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //REPORTE
            if (TraerdeVentaMovil())
            {
                //string produc = agregra_comillas(l_vendedores.Text);
                string desde = txt_desde.Text;
                string hasta = txt_hasta.Text;

                string clientes = agregra_comillas(l_clientes.Text);
                string estados = agregra_comillas(lb_bodegas2.Text);
                string estados_p_d = agregra_comillas(lb_bodegas3.Text);
                string estados_planif = agregra_comillas(lb_planificado.Text);
                string bodega = agregra_comillas(l_bodega.Text);

                string grupo = agregra_comillas(l_grupo_vm.Text);
                string vendedor = agregra_comillas(l_vendedor_vm.Text);

                string where3 = " where 1=1 ";

                if (Session["SW_PERMI"].ToString() == "2")
                {
                    where3 += " and b.DescEmisor = 'Granos'";
                }
                else
                {
                    where3 += " and b.DescEmisor <> 'Granos'";
                }

                //select * from[VPEDIDOCABECERA] where FechaEmision >= CONVERT(datetime, '21/07/2017', 103)
                if (desde != "")
                {
                    if (rd_em.Checked)
                    {
                        where3 += " and convert(datetime,b.FechaEmision ,103)  >= convert(datetime, '" + desde + "', 103) ";
                    }
                    else
                    {

                        where3 += " and convert(datetime,b.FechaDespacho ,103)  >= convert(datetime, '" + desde + "', 103) ";
                    }

                }

                if (hasta != "")
                {
                    if (rd_em.Checked)
                    {
                        where3 += " and convert(datetime,b.FechaEmision ,103)   <= convert(datetime, '" + hasta + "', 103) ";
                    }
                    else
                    {

                        where3 += " and convert(datetime,b.FechaDespacho ,103)   <= convert(datetime, '" + hasta + "', 103) ";


                    }
                }

                if (clientes != "")
                {

                    where3 += " and b.rut in (" + clientes + ")";

                }

                if (bodega != "")
                {
                    where3 += " and b.CodBodega in (" + bodega + ")";
                }

                string aux_estados = estados;

                Session["estados"] = estados;


                Session["estados_p_d"] = estados_p_d;
                Session["estados_planif"] = estados_planif.Replace("planificado", "si").Replace("pendiente", "no");

                if (estados.Contains("10S"))
                {

                    aux_estados = estados.Replace("10S", "20");

                }

                if (estados.Contains("10P"))
                {

                    aux_estados = estados.Replace("10P", "20");

                }

                if (estados != "")
                {

                    where3 += " and b.CodEstadoDocumento in (" + aux_estados + ")";

                }


                if (vendedor != "")
                {

                    where3 += " and b.codvendedor in (" + vendedor + ")";

                }
                if (Session["codvendedor"].ToString() != "")
                {

                    where3 += Session["codvendedor"].ToString();

                }


                if (txt_sp.Text != "")
                {

                    where3 += " and b.CodDocumento in (" + agregra_comillas(txt_sp.Text) + ")";

                }

                string estado = estados_planif.Replace("planificado", "si").Replace("pendiente", "no").Replace("'", "");

                if (estado != "")
                {
                    if (estado.Trim() == "si")
                    {
                        where3 += " and ISNUMERIC( isnull(d.coddocumento,'no')  ) = 1 ";
                    }
                    else if (estado.Trim() == "no")
                    {

                        where3 += " and ISNUMERIC( isnull(d.coddocumento,'no')  ) <> 1 ";

                    }
                }
                div_report.Visible = true;

                if (Session["SW_FILTRAR_PRODUCTO"].ToString() == "SI")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teadsaeee", "<script>  var elem3 = document.getElementById('div_superior'); elem3.style.display = 'none'; </script>", false);

                    where3 = Session["WHERE"].ToString();
                }
                where3 += " and a.codproducto not in (select distinct(codproducto)  from VPEDIDODETALLE_THX where ISNUMERIC(CodProducto) <> 1) order by CodDocumento desc";

                //SPSP
                DataTable dt2 = VM_listar_sp_select_capi(where3);
                string cod_aux = "";
                string facturas_aux = "";

                DataTable sp_malas = new DataTable();
                sp_malas.Columns.Add("sp");
                sp_malas.Columns.Add("facturas");
                sp_malas.Columns.Add("estado");


                DataTable sp_for = new DataTable();
                sp_for.Columns.Add("sp");
                sp_for.Columns.Add("estado");
                sp_for.Columns.Add("facturas");
                string facturas_x_sps = "";
                foreach (DataRow r in dt2.Rows)
                {
                    //20 -- OK
                    //10S -- SinFactura
                    //10P -- Cantidad Distintas      

                    //0  coddoc //24 cod //25 cant

                    if (r["DescEstadoDocumento"].ToString() == "Aprobado" && r["AprobadoFull"].ToString() == "no")
                    {
                        if (cod_aux == "")
                        {
                            cod_aux = r["CodDocumento"].ToString();
                            sp_for = new DataTable();
                            sp_for.Columns.Add("sp");
                            sp_for.Columns.Add("estado");
                            sp_for.Columns.Add("facturas");
                        }

                        DataTable procesado = ReporteRNegocio.SP_Marcelo(r["CodDocumento"].ToString().Trim(), r["CodProducto"].ToString().Trim(), r["Cantidad"].ToString().Trim());
                        DataRow row = procesado.Rows[0];

                        if (r["CodDocumento"].ToString() == cod_aux)
                        {
                            //if (row["estado"].ToString().Substring(0, 3) == "10P")
                            //{

                            facturas_aux = row["facturas"].ToString();

                            if (facturas_aux.Trim() != "")
                            {
                                facturas_x_sps += row["facturas"].ToString() + " ,";
                                r["Facturas"] = facturas_x_sps.Substring(0, facturas_x_sps.Length - 2).Trim();
                            }

                            DataRow row_sp1 = sp_for.NewRow();
                            row_sp1["sp"] = cod_aux;
                            row_sp1["estado"] = row["estado"].ToString().Substring(0, 3);
                            string aux_aca = "";
                            try
                            {
                                aux_aca = facturas_x_sps.Substring(0, facturas_x_sps.Length - 2).Trim();
                            }
                            catch
                            {


                            }

                            row_sp1["facturas"] = aux_aca;
                            sp_for.Rows.Add(row_sp1);


                        }
                        else
                        {
                            string estado_univ = "";
                            foreach (DataRow r2 in sp_for.Rows)
                            {
                                if (r2[1].ToString() == "10P")
                                {
                                    estado_univ = r2[1].ToString();
                                    DataRow row_sp = sp_malas.NewRow();
                                    row_sp["sp"] = cod_aux;
                                    row_sp["facturas"] = facturas_x_sps.Substring(0, facturas_x_sps.Length - 2).Trim();
                                    row_sp["estado"] = estado_univ;
                                    sp_malas.Rows.Add(row_sp);
                                    break;
                                }
                                else
                                {
                                    estado_univ = r2[1].ToString();
                                    DataRow row_sp = sp_malas.NewRow();
                                    row_sp["sp"] = cod_aux;

                                    string aux_fac = "";
                                    try
                                    {
                                        aux_fac = facturas_x_sps.Substring(0, facturas_x_sps.Length - 2).Trim();


                                    }
                                    catch
                                    {


                                    }
                                    row_sp["facturas"] = aux_fac;
                                    row_sp["estado"] = estado_univ;
                                    sp_malas.Rows.Add(row_sp);

                                }
                            }

                            cod_aux = r["CodDocumento"].ToString();
                            sp_for = new DataTable();
                            sp_for.Columns.Add("sp");
                            sp_for.Columns.Add("estado");
                            sp_for.Columns.Add("facturas");
                            facturas_x_sps = "";

                            facturas_x_sps += row["facturas"].ToString() + ", ";
                            r["Facturas"] = facturas_x_sps.Substring(0, facturas_x_sps.Length - 2).Trim();

                            DataRow row_sp1 = sp_for.NewRow();
                            row_sp1["sp"] = cod_aux;
                            row_sp1["estado"] = row["estado"].ToString().Substring(0, 3);
                            row_sp1["facturas"] = facturas_x_sps.Substring(0, facturas_x_sps.Length - 2).Trim();
                            sp_for.Rows.Add(row_sp1);


                        }



                    }
                    else if (r["DescEstadoDocumento"].ToString() == "Aprobado")
                    {


                        DataTable procesado = ReporteRNegocio.SP_Marcelo(r["CodDocumento"].ToString().Trim(), r["CodProducto"].ToString().Trim(), r["Cantidad"].ToString().Trim());
                        DataRow row = procesado.Rows[0];
                        r["Facturas"] = row["facturas"].ToString().Trim();




                    }
                }

                string estado_univ1 = "";
                foreach (DataRow r2 in sp_for.Rows)
                {
                    if (r2[1].ToString() == "10P")
                    {
                        estado_univ1 = r2[1].ToString();
                        DataRow row_sp = sp_malas.NewRow();
                        row_sp["sp"] = cod_aux;
                        row_sp["facturas"] = r2[2].ToString().Trim();
                        row_sp["estado"] = estado_univ1;
                        sp_malas.Rows.Add(row_sp);
                        break;
                    }
                    else
                    {
                        estado_univ1 = r2[1].ToString();
                        DataRow row_sp = sp_malas.NewRow();
                        row_sp["sp"] = cod_aux;
                        row_sp["facturas"] = r2[2].ToString().Trim();
                        row_sp["estado"] = estado_univ1;
                        sp_malas.Rows.Add(row_sp);

                    }
                }


                foreach (DataRow r in dt2.Rows)
                {
                    if (r["DescEstadoDocumento"].ToString() == "Aprobado")
                    {
                        foreach (DataRow r2 in sp_malas.Rows)
                        {
                            if (r["CodDocumento"].ToString() == r2[0].ToString())
                            {
                                string cad = r2[1].ToString();
                                Dictionary<string, int> contador = new Dictionary<string, int>();

                                foreach (string item in cad.Split(new char[] { ',' }))
                                {
                                    if (contador.ContainsKey(item.Trim()))
                                    {
                                        contador[item.Trim()] = contador[item.Trim()] + 1;

                                    }
                                    else
                                    {
                                        contador.Add(item.Trim(), 1);
                                    }
                                }

                                string resultado = "";
                                foreach (KeyValuePair<string, int> item in contador)
                                {
                                    if (item.Value >= 1)
                                    {
                                        resultado = string.Format("{0},{1}", resultado, item.Key);
                                    }
                                }
                                string cadd = resultado.Remove(0, 1);

                                if (r2[2].ToString() == "10S" && cadd.Trim() != "")
                                {

                                    r["Facturas"] = cadd;
                                    r["ESTADO"] = "10P";

                                }
                                else
                                {

                                    r["Facturas"] = cadd;
                                    r["ESTADO"] = r2[2].ToString();
                                }


                            }
                        }
                    }
                    r["Facturas"] = r["Facturas"].ToString().Replace(",", ", ");
                }

                if (dt2.Rows.Count > 0)
                {
                    dt2 = dt2.AsEnumerable()
                   .GroupBy(r => new { Col1 = r["CodDocumento"] })
                   .Select(g => g.OrderBy(r => r["CodDocumento"]).First())
                   .CopyToDataTable();
                }

                G_INFORME_TOTAL_VENDEDOR.DataSource = dt2;
                G_INFORME_TOTAL_VENDEDOR.DataBind();
                relojito(false);
            }

        }

        public bool TraerdeVentaMovil()
        {
            string respuesta_servidor = "";
            DataTable dt_sp_encabe = ventamovil_sp_enc();
            string in_sp = "";
            foreach (DataRow dr in dt_sp_encabe.Rows)
            {
                if (in_sp == "")
                {
                    in_sp += "'" + dr["codDocumento"].ToString() + "'";
                }
                else
                {
                    in_sp += ", '" + dr["codDocumento"].ToString() + "'";
                }
            }
            DataTable dt_sp_detalle = ventamovil_sp_det(in_sp);
            DataTable dt_sp_encabe_EXT = ventamovil_sp_enc_EXT(in_sp);
            DataTable dt_sp_detalle_EXT = ventamovil_sp_det_EXT(in_sp);
            //DataTable dt_cliente_matriz = ventamovil_cliente_matriz();
            //DataTable dt_cliente_sucursal = ventamovil_cliente_sucursal();
            string cadena_vm_thx = "Data Source=192.168.10.45;Initial Catalog=SoprodiVenta;Persist Security Info=True;User ID=sa;Password=Soprodi1234";
            bool ok = true;
            if (borrar_sp(in_sp))
            {
                if (borrar_cliente())
                {

                    //------------------------------------------------------------------------>   encabezado 1 
                    SqlBulkCopy bulkcopy = new SqlBulkCopy(cadena_vm_thx);
                    bulkcopy.DestinationTableName = "TrnDocumentoCabecera";
                    try
                    {
                        bulkcopy.WriteToServer(dt_sp_encabe);
                    }
                    catch (Exception ex)
                    {
                        ok = false;
                        respuesta_servidor += "ERROR1: " + ex.Message;
                    }
                    //------------------------------------------------------------------------>   detalle 1
                    SqlBulkCopy bulkcopy2 = new SqlBulkCopy(cadena_vm_thx);
                    bulkcopy2.DestinationTableName = "TrnDocumentoDetalle";
                    try
                    {
                        bulkcopy2.WriteToServer(dt_sp_detalle);
                    }
                    catch (Exception ex)
                    {
                        ok = false;
                        respuesta_servidor += "// ERROR2: " + ex.Message;
                    }
                    //ext_
                    //------------------------------------------------------------------------>   encabezado 2
                    SqlBulkCopy bulkcopy3 = new SqlBulkCopy(cadena_vm_thx);
                    bulkcopy3.DestinationTableName = "ext_TrnDocumentoCabecera";
                    try
                    {
                        bulkcopy3.WriteToServer(dt_sp_encabe_EXT);
                    }
                    catch (Exception ex)
                    {
                        ok = false;
                        respuesta_servidor += "// ERROR3: " + ex.Message;
                    }
                    //------------------------------------------------------------------------>   detalle 2
                    SqlBulkCopy bulkcopy4 = new SqlBulkCopy(cadena_vm_thx);
                    bulkcopy4.DestinationTableName = "ext_TrnDocumentoDetalle";
                    try
                    {
                        bulkcopy4.WriteToServer(dt_sp_detalle_EXT);
                    }
                    catch (Exception ex)
                    {
                        ok = false;
                        respuesta_servidor += "// ERROR4: " + ex.Message;
                    }
                    //------------------------------------------------------------------------>   cliente matriz 1
                    //SqlBulkCopy bulkcopy5 = new SqlBulkCopy(cadena_vm_thx);
                    //bulkcopy5.DestinationTableName = "MaeClienteMatriz";
                    //try
                    //{
                    //    bulkcopy5.WriteToServer(dt_cliente_matriz);
                    //}
                    //catch (Exception ex)
                    //{
                    //    ok = false;
                    //    respuesta_servidor += "// ERROR52: " + ex.Message;
                    //}
                    ////------------------------------------------------------------------------>   cliente sucursal 1
                    //SqlBulkCopy bulkcopy6 = new SqlBulkCopy(cadena_vm_thx);
                    //bulkcopy6.DestinationTableName = "MaeClienteSucursal";
                    //try
                    //{
                    //    bulkcopy6.WriteToServer(dt_cliente_sucursal);
                    //}
                    //catch (Exception ex)
                    //{
                    //    ok = false;
                    //    respuesta_servidor += "// ERROR6: " + ex.Message;
                    //}
                }
                else
                {
                    // ERROR BORRAR CLIENTE
                    respuesta_servidor += "// ERROR AL BORRAR CLIENTES ";
                }
            }
            else
            {
                respuesta_servidor += "// ERROR AL BORRAR SP ";
                // ERROR BORRAR SP
            }
            alert(respuesta_servidor, 0);
            return ok;
        }

        internal static DataTable ventamovil_sp_enc()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil_real"].ToString()))
            {
                conn.Open();
                string sql = @"	select * from [dbo].[TrnDocumentoCabecera] where convert(date, GxActualizado,103) = convert(date,getdate(),103);";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable ventamovil_sp_det(string filtro)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil_real"].ToString()))
            {
                conn.Open();
                string sql = @"	select * from [dbo].[TrnDocumentoDetalle]  where coddocumento in (" + filtro + ");";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable ventamovil_sp_det_EXT(string filtro)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil_real"].ToString()))
            {
                conn.Open();
                string sql = @"	select * from [dbo].[ext_TrnDocumentoDetalle]  where coddocumento in (" + filtro + ");";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable ventamovil_sp_enc_EXT(string filtro)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil_real"].ToString()))
            {
                conn.Open();
                string sql = @"	select * from [dbo].[ext_TrnDocumentoCabecera] where coddocumento in (" + filtro + ");";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable ventamovil_cliente_matriz()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil_real"].ToString()))
            {
                conn.Open();
                string sql = @"	select * from [dbo].[MaeClienteMatriz] where convert(date, GxActualizado,103) = convert(date,getdate(),103);";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable ventamovil_cliente_sucursal()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil_real"].ToString()))
            {
                conn.Open();
                string sql = @"	select * from [dbo].[MaeClienteSucursal] where convert(date, GxActualizado,103) = convert(date,getdate(),103);";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        private bool borrar_sp(string filtro)
        {

            string cadena_vm_thx = "Data Source=192.168.10.45;Initial Catalog=SoprodiVenta;Persist Security Info=True;User ID=sa;Password=Soprodi1234";

            using (SqlConnection conn = new SqlConnection(cadena_vm_thx))
            {

                bool Ok = true;
                conn.Open();
                string sql = @"  delete from TrnDocumentoDetalle  where coddocumento in (" + filtro + ");   ";
                string sql2 = @"  delete from TrnDocumentoCabecera where coddocumento in (" + filtro + "); ";
                string sql3 = @"  delete from ext_TrnDocumentoDetalle where coddocumento in (" + filtro + "); ";
                string sql4 = @"   delete from ext_TrnDocumentoCabecera where coddocumento in (" + filtro + ");";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception EX)
                    {
                        Ok = false;
                    }
                }
                using (SqlCommand cmd = new SqlCommand(sql2, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception EX)
                    {
                        Ok = false;
                    }
                }
                using (SqlCommand cmd = new SqlCommand(sql3, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception EX)
                    {
                        Ok = false;
                    }
                }
                using (SqlCommand cmd = new SqlCommand(sql4, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception EX)
                    {
                        Ok = false;
                    }
                }
                return Ok;
            }
        }

        private bool borrar_cliente()
        {
            //    bool ok = true;
            //    string cadena_vm_thx = "Data Source=192.168.10.45;Initial Catalog=SoprodiVenta;Persist Security Info=True;User ID=sa;Password=Soprodi1234";
            //    using (SqlConnection conn = new SqlConnection(cadena_vm_thx))
            //    {
            //        conn.Open();
            //        string sql = " delete from MaeClienteSucursal where convert(date, GxActualizado,103) = convert(date,getdate(),103); ";   
            //        string sql2 = " delete from MaeClienteMatriz where convert(date, GxActualizado,103) = convert(date,getdate(),103); ";
            //        using (SqlCommand cmd = new SqlCommand(sql, conn))
            //        {
            //            try
            //            {
            //                cmd.ExecuteNonQuery();

            //            }
            //            catch (Exception EX)
            //            {
            //                ok = false;
            //            }
            //        }
            //        using (SqlCommand cmd = new SqlCommand(sql2, conn))
            //        {
            //            try
            //            {
            //                cmd.ExecuteNonQuery();

            //            }
            //            catch (Exception EX)
            //            {
            //                ok = false;
            //            }
            //        }
            //        return ok;
            //    }
            return true;
        }

        //public bool BulkCopy_PRA()
        //{
        //    string cadena_vm_thx = "Data Source=192.168.10.45;Initial Catalog=SoprodiVenta;Persist Security Info=True;User ID=sa;Password=Soprodi1234";
        //    DataTable dt_sp_encabe = ventamovil_sp_enc();
        //    DataTable dt_sp_detalle = ventamovil_sp_det();
        //    DataTable dt_sp_encabe_EXT = ventamovil_sp_enc_EXT();
        //    DataTable dt_sp_detalle_EXT = ventamovil_sp_det_EXT();

        //    SqlBulkCopy bulkcopy = new SqlBulkCopy(cadena_vm_thx);
        //    bulkcopy.DestinationTableName = "TrnDocumentoCabecera";
        //    try
        //    {
        //        bulkcopy.WriteToServer(dt_sp_encabe);
        //    }
        //    catch (Exception ex)
        //    {             
        //        Console.Write(ex.Message);
        //    }
        //    SqlBulkCopy bulkcopy2 = new SqlBulkCopy(cadena_vm_thx);
        //    bulkcopy2.DestinationTableName = "TrnDocumentoDetalle";
        //    try
        //    {
        //        bulkcopy2.WriteToServer(dt_sp_detalle);
        //    }
        //    catch (Exception ex)
        //    {             
        //        Console.Write(ex.Message);
        //    }
        //    //ext_
        //    SqlBulkCopy bulkcopy3 = new SqlBulkCopy(cadena_vm_thx);
        //    bulkcopy3.DestinationTableName = "ext_TrnDocumentoCabecera";
        //    try
        //    {
        //        bulkcopy3.WriteToServer(dt_sp_encabe_EXT);
        //    }
        //    catch (Exception ex)
        //    {            
        //        Console.Write(ex.Message);
        //    }
        //    SqlBulkCopy bulkcopy4 = new SqlBulkCopy(cadena_vm_thx);
        //    bulkcopy4.DestinationTableName = "ext_TrnDocumentoDetalle";
        //    try
        //    {
        //        bulkcopy4.WriteToServer(dt_sp_detalle_EXT);
        //    }
        //    catch (Exception ex)
        //    {             
        //        Console.Write(ex.Message);
        //    }
        //    return true;
        //}

        //internal static DataTable ventamovil_sp_enc()
        //{
        //    DataTable dt = new DataTable();
        //    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil_real"].ToString()))
        //    {
        //        conn.Open();
        //        string sql = @"	select * from [dbo].[TrnDocumentoCabecera] where [CodDocumento] = '" + sp + "'";
        //        SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
        //        SqlDataAdapter ap = new SqlDataAdapter(cmd);
        //        try
        //        {
        //            ap.Fill(dt);
        //        }
        //        catch { return dt = new DataTable(); }
        //    }
        //    return dt;
        //}

        //internal static DataTable ventamovil_sp_det()
        //{
        //    DataTable dt = new DataTable();
        //    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil_real"].ToString()))
        //    {
        //        conn.Open();
        //        string sql = @"	select * from [dbo].[TrnDocumentoDetalle] where [CodDocumento] = '" + sp + "'";
        //        SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
        //        SqlDataAdapter ap = new SqlDataAdapter(cmd);
        //        try
        //        {
        //            ap.Fill(dt);
        //        }
        //        catch { return dt = new DataTable(); }
        //    }
        //    return dt;
        //}

        //internal static DataTable ventamovil_sp_enc_EXT()
        //{
        //    DataTable dt = new DataTable();
        //    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil_real"].ToString()))
        //    {
        //        conn.Open();
        //        string sql = @"	select * from [dbo].[ext_TrnDocumentoCabecera] where [CodDocumento] = '" + sp + "'";
        //        SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
        //        SqlDataAdapter ap = new SqlDataAdapter(cmd);
        //        try
        //        {
        //            ap.Fill(dt);
        //        }
        //        catch { return dt = new DataTable(); }
        //    }
        //    return dt;
        //}

        //internal static DataTable ventamovil_sp_det_EXT()
        //{
        //    DataTable dt = new DataTable();
        //    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil_real"].ToString()))
        //    {
        //        conn.Open();
        //        string sql = @"	select * from [dbo].[ext_TrnDocumentoDetalle] where [CodDocumento] = '" + sp + "'";
        //        SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
        //        SqlDataAdapter ap = new SqlDataAdapter(cmd);
        //        try
        //        {
        //            ap.Fill(dt);
        //        }
        //        catch { return dt = new DataTable(); }
        //    }
        //    return dt;
        //}

        protected void G_INFORME_TOTAL_VENDEDOR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Reportar")
                {
                    //                                                CodDocumento,
                    //                                                NombreCliente,
                    //                                                NombreVendedor,
                    //                                                MontoNeto,
                    //                                                DescBodega,
                    //                                                FechaDespacho, (5)
                    //                                                DifDias,
                    //                                                FechaEmision,
                    //                                                CodVendedor,
                    //                                                NotaLibre,
                    //                                                CodBodega, (10)
                    //                                                CodMoneda,   
                    //                                                DescEstadoDocumento,
                    //                                                Facturas,
                    //                                                GxEstadoSync,
                    //                                                GxActualizado, (15)
                    //                                                GxEnviadoERP,
                    //                                                FechaCreacion,
                    //                                                ValorTipoCambio,
                    //                                                LimiteSeguro, 
                    //                                                TipoCredito , (20)
                    //                                                CreditoDisponible ,
                    //                                                CreditoAutorizado,
                    //                                                EmailVendedor,
                    //                                                ESTADO,
                    //                                                CodProducto, (25)
                    //                                                Cantidad,
                    //                                                AprobadoFull,
                    //                                                Asignada,
                    //                                                EstadoParcial,   
                    //                                                fPLAN, (30)
                    //                                                NombreSucursal,
                    //                                                NotaDespacho,
                    //                                                DescFormaPago,
                    //                                              CodDocumentoERP,
                    //                                                      estado_capi (35)
                    //                                              usuario_soprodi
                    //                                              id_interno
                    //                                              codcliente (38)
                    //                                              DirDireccion1, IVA, MontoBruto
                    string CodDocumento = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    string NombreCliente = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                    string NombreVendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString());
                    string MontoNeto = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString());
                    string FechaEmision = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[7].ToString());
                    string CodVendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[8].ToString());
                    string FechaDespacho = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString());
                    string DescBodega = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[4].ToString());
                    string NombreSucursal = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[31].ToString());
                    string NotaLibre = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[9].ToString());
                    string NotaDespacho = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[32].ToString());
                    string DescFormaPago = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[33].ToString());
                    string DescEstadoDocumento = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[12].ToString());
                    string estado_capi = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[35].ToString());
                    string usuario_soprodi = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[36].ToString());
                    string id_interno = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[37].ToString());
                    string sp_solomon = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[34].ToString());
                    string codcliente = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[38].ToString());
                    string DirDireccion1 = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[39].ToString());
                    string IVA = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[40].ToString());
                    string MontoBruto = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[41].ToString());
                    string CodBodega = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[10].ToString());

                    string RUTCLIENTE_TEBO = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[38].ToString());
                    T_RUT_CLIENTE.Text = RUTCLIENTE_TEBO;

                    if (DescEstadoDocumento == "Aprobado")
                    {
                        DIV_PLANIFICAR_SP.Visible = true;
                    }
                    else
                    {
                        DIV_PLANIFICAR_SP.Visible = true;
                    }
                    COD_BODEGA.Text = CodBodega;
                    ID_INTERNO_SP.Text = id_interno;
                    T_NUM_SP.Text = CodDocumento;
                    LBL_NUM_SP.Text = CodDocumento;
                    LBL_ESTADO_INTERNO.Text = estado_capi;
                    LBL_ESTADO_VTA_MOVIL.Text = DescEstadoDocumento;
                    LBL_SP_SOLOMON.Text = sp_solomon;
                    LBL_CLIENTE.Text = NombreCliente;
                    LBL_VENDEDOR.Text = NombreVendedor;
                    LBL_FECHA_EMISION.Text = FechaEmision;
                    LBL_FECHA_DESPACHO.Text = FechaDespacho;
                    LBL_NOM_BODEGA.Text = DescBodega;
                    LBL_SUCURSAL.Text = NombreSucursal;
                    LBL_NOTA_LIBRE.Text = NotaLibre;
                    LBL_NOTA_DESPACHO.Text = NotaDespacho;
                    LBL_NETO.Text = "$" + double.Parse(MontoNeto).ToString("#,##0");
                    LBL_IVA.Text = "$" + double.Parse(IVA).ToString("#,##0");
                    LBL_TOTAL.Text = "$" + double.Parse(MontoBruto).ToString("#,##0");
                    LBL_FORMA_PAGO.Text = DescFormaPago;
                    LBL_DIRECCION.Text = DirDireccion1;
                    COD_VENDEDOR.Text = CodVendedor;

                    // LIMPIAR CAMPOS MODAL
                    DETALLE_REPORTE.Text = "";
                    T_NOTA_CORREO.Text = "";
                    T_FECHA_PLAN.Text = "";
                    MOTIVO_RECHAZO.Text = "";
                    //

                    // LINEA DE CREDITO
                    DBUtil db = new DBUtil();
                    DataTable dt_credito = db.consultar("select LC, isnull(disponible,0) as'disponible' from V_CTZ_GESTION_VENTAS_FIN where rutcliente = '" + codcliente + "'");
                    if (dt_credito.Rows.Count > 0)
                    {
                        LBL_LC_DISPONIBLE.Text = "$" + int.Parse(dt_credito.Rows[0]["disponible"].ToString()).ToString("#,##0");
                        LBL_LC.Text = "$" + int.Parse(dt_credito.Rows[0]["LC"].ToString()).ToString("#,##0");
                    }
                    else
                    {
                        LBL_LC_DISPONIBLE.Text = "No disponible";
                        LBL_LC.Text = "No disponible";
                    }
                    //
                    RUT_CLIENTE.Text = codcliente;

                    DataTable DT_DETALLE = TraeDetalleSP(CodDocumento);
                    G_DETALLE.DataSource = DT_DETALLE;
                    G_DETALLE.DataBind();

                    UP_REP_VEND.Update();
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Reportar_SP", "<script>AbreModal_ReportarSP();</script>", false);
                    relojito(false);
                }
            }
            catch (Exception ex)
            {

            }
        }

        internal static DataTable VM_listar_sp_select_capi(string where3)
        {
            DataTable dt = new DataTable();
            string bd_respaldo = ConfigurationManager.AppSettings["BD_PRUEBA"];
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil"].ToString()))
            {
                conn.Open();

                string sql = "select a.DescProducto, a.CodProducto, a.CodUnVenta, a.Cantidad, b.CodDocumento, b.NombreCliente, b.NombreVendedor, b.MontoNeto, b.DescBodega, b.DescFormaPago, convert(varchar, b.FechaDespacho,103) as FechaDespacho, DATEDIFF(DAY, CONVERT(datetime,GETDATE(),103), b.FechaDespacho) as DifDias " +
                "  , convert(varchar, b.FechaEmision, 103) as FechaEmision , b.CodVendedor, b.NotaLibre, b.NotaDespacho, b.nombreSucursal, b.CodBodega , isnull(b.CodDocumentoERP, 'n/a') as 'CodDocumentoERP', b.CodMoneda, b.DescEstadoDocumento, '' as Facturas,  b.GxEstadoSync,  " +
                "     convert(varchar, b.GxActualizado, 103) as GxActualizado , b.GxEnviadoERP, convert(varchar, b.FechaCreacion, 103) as FechaCreacion ,  " +
                "     b.ValorTipoCambio,b.LimiteSeguro, b.TipoCredito, b.CreditoDisponible, b.CreditoAutorizado, b.EmailVendedor, b.CodEstadoDocumento as ESTADO, a.CodProducto, a.Cantidad,isnull(c.coddocumento,'no')  as AprobadoFull " +
                " ,  e.estado as EstadoParcial, isnull(eee.estado, 'No Procesada') as 'estado_capi', isnull(eee.cod_usuario, 'n/a') as 'usuario_soprodi', isnull(eee.id,-1) as 'id_interno', b.codcliente, ' ' as 'fPLAN', ' ' as 'Asignada' " +

                " , b.DirDireccion1, b.IVA, b.MontoBruto " +
                "" +
                " from VPEDIDODETALLE_THX a inner join VPEDIDOCABECERA b on a.coddocumento = b.coddocumento left join THX_Sp_Aprobadas c on a.CodDocumento = c.coddocumento  " +
                " left join [" + bd_respaldo + "].[dbo].[Estado_SP]  e on a.CodDocumento = e.sp " +
                " left join [" + bd_respaldo + "].[dbo].[SP_ESTADOS_INTERNOS] eee on eee.num_sp = a.coddocumento and eee.id = (select MAX(xxxxxx.id) from [" + bd_respaldo + "].[dbo].[SP_ESTADOS_INTERNOS] xxxxxx where xxxxxx.num_sp = a.coddocumento)  "
                + where3;

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable TraeDetalleSP(string num_sp)
        {
            DataTable dt = new DataTable();
            string bd_respaldo = ConfigurationManager.AppSettings["BD_PRUEBA"]; using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil"].ToString()))
            {
                conn.Open();

                string sql = "select a.DescProducto, a.CodProducto, a.CodUnVenta, a.PrecioUnitario, a.Cantidad, a.Descuento, a.MontoNetoFinal, convert(decimal, a.MontoNeto) as 'det_monto_neto', b.CodDocumento, b.NombreCliente, b.NombreVendedor, b.MontoNeto, b.DescBodega, convert(varchar, b.FechaDespacho,103) as FechaDespacho,  ' ' as fPLAN, DATEDIFF(DAY, CONVERT(datetime,GETDATE(),103), b.FechaDespacho) as DifDias " +
                "  , convert(varchar, b.FechaEmision, 103) as FechaEmision , b.CodVendedor, b.NotaLibre, b.CodBodega , b.CodMoneda, isnull(b.CodDocumentoERP, 'n/a') as 'CodDocumentoERP', b.DescEstadoDocumento, '' as Facturas,  b.GxEstadoSync,  " +
                "     convert(varchar, b.GxActualizado, 103) as GxActualizado , b.GxEnviadoERP, convert(varchar, b.FechaCreacion, 103) as FechaCreacion ,  " +
                "     b.ValorTipoCambio,b.LimiteSeguro, b.TipoCredito, b.CreditoDisponible, b.CreditoAutorizado, b.EmailVendedor, b.CodEstadoDocumento as ESTADO, a.CodProducto, a.Cantidad,isnull(c.coddocumento,'no')  as AprobadoFull " +
                " , ' '   as Asignada,  e.estado as EstadoParcial, b.codcliente, '' as PrecioLista, '' as Stock " +
                "" +
                "" +
                " from VPEDIDODETALLE_THX a inner join VPEDIDOCABECERA b on a.coddocumento = b.coddocumento      left join THX_Sp_Aprobadas c on a.CodDocumento = c.coddocumento  " +
                " left join [" + bd_respaldo + "].[dbo].[Estado_SP]  e on a.CodDocumento = e.sp where a.CodDocumento = '" + num_sp + "' ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }


        protected void G_INFORME_TOTAL_VENDEDOR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string estado = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.Row.RowIndex)].Values[12].ToString());
                switch (estado)
                {
                    case "Aprobado":
                        e.Row.Cells[7].Text = "<span class='badge badge-success badge-block'><i class='fa fa-check'></i> Aprobado</span>";
                        break;
                    case "Pendiente":
                        e.Row.Cells[7].Text = "<span class='badge badge-warning badge-block'><i class='fa fa-clock-o'></i> Pendiente</span>";
                        break;
                    case "Sincronizado":
                        e.Row.Cells[7].Text = "<span class='badge badge-azul badge-block'><i class='fa fa-refresh'></i> Sincronizado</span>";
                        break;
                    case "Rechazado":
                        e.Row.Cells[7].Text = "<span class='badge badge-rojo badge-block'><i class='fa fa-close'></i> Rechazado</span>";
                        break;
                    default:
                        break;
                }

                Session["rut_cliente"] = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.Row.RowIndex)].Values[12].ToString());

                string estado2 = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.Row.RowIndex)].Values[35].ToString());
                switch (estado2)
                {
                    case "Instruida":
                        e.Row.Cells[6].Text = "<span class='badge badge-success badge-block'><i class='fa fa-check'></i> Instruida</span>";
                        break;
                    case "Instruida Incompleta":
                        e.Row.Cells[6].Text = "<span class='badge badge-success badge-block'><i class='fa fa-clock-o'></i> Instruida Incompleta</span>";
                        break;
                    case "No Procesada":
                        e.Row.Cells[6].Text = "<span class='badge badge-primary badge-block'><i class='fa fa-clock-o'></i> No Procesada</span>";

                        break;
                    case "Pendiente Vendedor":
                        e.Row.Cells[6].Text = "<span class='badge badge-azul badge-block'><i class='fa fa-clock-o'></i> Pendiente Vendedor</span>";

                        break;
                    case "Rechazada":
                        e.Row.Cells[6].Text = "<span class='badge badge-rojo badge-block'><i class='fa fa-close'></i> Rechazada</span>";

                        break;
                    default:
                        break;
                }
            }
        }

        protected void B_ENVIAR_REPORTE_SP_Click(object sender, EventArgs e)
        {
            try
            {
                sp_estados_internosEntity sps = new sp_estados_internosEntity();
                sps.cod_usuario = User.Identity.Name.ToString();
                sps.estado = "Pendiente Vendedor";
                sps.fecha = DateTime.Now;
                sps.num_sp = T_NUM_SP.Text;
                sps.nota_correo = "";

                if (sp_estados_internosBO.registrar(sps) == "OK")
                {
                    sp_problemas_vendedorEntity plan = new sp_problemas_vendedorEntity();
                    plan.cod_vendedor = User.Identity.Name.ToString();
                    plan.detalle = DETALLE_REPORTE.Text;
                    plan.fecha = DateTime.Now;
                    plan.num_sp = T_NUM_SP.Text;
                    plan.estado = "PROBLEMA";
                    if (sp_problemas_vendedorBO.registrar(plan) == "OK")
                    {
                        CorreoPra2();
                        Button1_Click(sender, e);
                        UpdatePanel2.Update();
                        alert("Correo enviado a Vendedor Correctamente", 1);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "cierramodal", "<script>javascript:CerrarModal_ReportarSP();</script>", false);
                    }
                }

            }
            catch (Exception ex)
            {
                alert("Problemas al planificar SP Nº: " + T_NUM_SP.Text, 0);
            }
        }

        protected void B_PLANIFICAR_SP_Click(object sender, EventArgs e)
        {
            try
            {
                bool ok = true;
                foreach (GridViewRow x in G_DETALLE.Rows)
                {
                    TextBox cant_ = (TextBox)x.FindControl("T_CANTIDAD_PLAN");
                    TextBox cant_1 = (TextBox)x.FindControl("T_CANTIDAD_SP");

                    double cplan = Convert.ToDouble(cant_.Text);
                    double csp = Convert.ToDouble(cant_1.Text);

                    if (cplan < csp)
                    {
                        ok = false;
                    }
                }
                if (T_FECHA_PLAN.Text == "")
                {
                    alert("Ingrese fecha para instruir", 0);
                    relojito(false);
                }
                else
                {
                    sp_estados_internosEntity sps = new sp_estados_internosEntity();
                    sps.cod_usuario = User.Identity.Name.ToString();
                    if (ok)
                    {
                        sps.estado = "Instruida";
                    }
                    else
                    {
                        sps.estado = "Instruida Incompleta";
                    }

                    sps.fecha = DateTime.Now;
                    sps.num_sp = T_NUM_SP.Text;
                    sps.nota_correo = "";

                    if (sp_estados_internosBO.registrar(sps) == "OK")
                    {
                        sp_enviadas_planificacionEntity plan = new sp_enviadas_planificacionEntity();
                        plan.cod_vendedor = User.Identity.Name.ToString();
                        plan.detalle = T_NOTA_CORREO.Text;
                        plan.fecha = DateTime.Now;
                        plan.fecha_planificacion = Convert.ToDateTime(T_FECHA_PLAN.Text);
                        plan.num_sp = T_NUM_SP.Text;
                        if (sp_enviadas_planificacionBO.registrar(plan) == "OK")
                        {
                            //
                            foreach (GridViewRow x in G_DETALLE.Rows)
                            {
                                TextBox cant_ = (TextBox)x.FindControl("T_CANTIDAD_PLAN");
                                TextBox cant_1 = (TextBox)x.FindControl("T_CANTIDAD_SP");

                                double cplan = Convert.ToDouble(cant_.Text);
                                double csp = Convert.ToDouble(cant_1.Text);

                                if (cplan < csp)
                                {
                                    sp_saldosEntity saldo = new sp_saldosEntity();
                                    saldo.num_sp = T_NUM_SP.Text;
                                    saldo.cantidad_planificada = cplan;
                                    saldo.cantidad_real = csp;
                                    saldo.CodProducto = x.Cells[0].Text;
                                    saldo.CodUnVenta = x.Cells[5].Text;
                                    saldo.fecha = DateTime.Now;
                                    saldo.saldo = csp - cplan;
                                    sp_saldosBO.registrar(saldo);
                                }
                            }
                            //
                            CorreoPra();
                            Button1_Click(sender, e);
                            UpdatePanel2.Update();
                            alert("SP Planificada con éxito", 1);
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "cierramodal", "<script>javascript:CerrarModal_ReportarSP();</script>", false);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                alert("Problemas al planificar SP Nº: " + T_NUM_SP.Text, 0);
            }
        }

        protected void B_RECHAZAR_SP_Click(object sender, EventArgs e)
        {
            try
            {
                sp_estados_internosEntity sps = new sp_estados_internosEntity();
                sps.cod_usuario = User.Identity.Name.ToString();
                sps.estado = "Rechazada";
                sps.fecha = DateTime.Now;
                sps.num_sp = T_NUM_SP.Text;
                sps.nota_correo = "";
                if (sp_estados_internosBO.registrar(sps) == "OK")
                {
                    CorreoPra3();
                    Button1_Click(sender, e);
                    UpdatePanel2.Update();
                    alert("SP Rechazada con éxito", 1);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "cierramodal", "<script>javascript:CerrarModal_ReportarSP();</script>", false);
                }

            }
            catch (Exception ex)
            {
                alert("Problemas al planificar SP Nº: " + T_NUM_SP.Text, 0);
            }
        }

        public void CorreoPra()
        {
            string html = "";

            html += "<h4>Estimado: Guillermo Morales</h4>";
            html += "<p>Se solicita planificar la siguiente SP:</p>";
            html += "<p><b>SP Nº: " + T_NUM_SP.Text + "</b></p>";
            html += "<p><b>Para el día: " + Convert.ToDateTime(T_FECHA_PLAN.Text).ToString("dd/MM/yyyy") + "</p>";
            html += "<p><b>Nota: " + T_NOTA_CORREO.Text + "</p>";
            html += "<hr>";
            html += "<p>Detalle SP:</p>";
            html += "<table style='width:100%' border=1>";
            html += "   <thead>";
            html += "   <tr>";
            html += "       <th>COD. PRODUCTO</th>";
            html += "       <th>PRODUCTO</th>";
            html += "       <th>CANTIDAD</th>";
            html += "       <th>UND. VENTA</th>";
            html += "   </tr>";
            html += "   </thead>";
            html += "   <tbody>";
            foreach (GridViewRow x in G_DETALLE.Rows)
            {
                html += "   <tr>";
                html += "       <td>" + x.Cells[0].Text + "</td>";
                html += "       <td>" + x.Cells[1].Text + "</td>";
                TextBox cant_ = (TextBox)x.FindControl("T_CANTIDAD_PLAN");
                TextBox cant_1 = (TextBox)x.FindControl("T_CANTIDAD_SP");
                html += "       <td>" + cant_.Text + "</td>";
                html += "       <td>" + x.Cells[7].Text + "</td>";
                html += "   </tr>";
            }
            html += "   </tbody>";
            html += "</table>";
            usuarioEntity u = new usuarioEntity();
            u = (usuarioEntity)(Session["usuario"]);

            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(u.correo));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "Instruccion Despacho Nº SP " + T_NUM_SP.Text + " de cliente: " + LBL_CLIENTE.Text + ", vendedor: " + LBL_VENDEDOR.Text;

            if (LBL_SUCURSAL.Text.Trim() == "BODEGA ARICA SOPRODI")
            {
                // ARICA
                email.CC.Add("ovillalobos@soprodi.cl");
                email.CC.Add("amanez@soprodi.cl");
                email.CC.Add("ggonzalez@soprodi.cl");
                email.CC.Add("caguilera@soprodi.cl");
                email.CC.Add("rflores@soprodi.cl");              
                email.CC.Add("pcataldo@soprodi.cl");
                email.CC.Add("rsolis@soprodi.cl");
                email.CC.Add("contador3@SOPRODI.CL");
            }
            else
            {
                // ZARATE
                email.CC.Add("rmc@soprodi.cl");
                email.CC.Add("gmorales@soprodi.cl");
                email.CC.Add("rramirez@soprodi.cl");
                email.CC.Add("pcataldo @soprodi.cl");
                email.CC.Add("rsolis@soprodi.cl");
                email.CC.Add("contador3@SOPRODI.CL");
            }
          
            DBUtil db = new DBUtil();
            string correo_vendedor = db.Scalar("select correo from usuarioweb where cod_usuario = '" + COD_VENDEDOR.Text + "'").ToString();
            if (correo_vendedor == "")
            {

            }
            else
            {
                email.CC.Add(correo_vendedor);
            }
            email.Body = html;
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;
            email.BodyEncoding = System.Text.Encoding.UTF8;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            // ADJUNTAR PDF DE SP AQUI

            //
            //smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            smtp.Credentials = new NetworkCredential("informes.soprodi@gmail.com", "galaxia1234");
            try
            {
                //ACA ESTUVO EL TEBO
                string n_file = T_NUM_SP.Text.Trim() + ".pdf";
                string pdfPath = Server.MapPath(@"~\PDFs\") + n_file;
                Base.crear_sp_pdf(T_NUM_SP.Text.Trim(), pdfPath);
                System.IO.FileInfo toDownload = new System.IO.FileInfo(pdfPath);
                if (toDownload.Exists)
                {
                    email.Attachments.Add(new System.Net.Mail.Attachment(pdfPath));
                }
                //---------------------------------------------------------
                smtp.Send(email);
                email.Dispose();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('No enviado');</script>", false);
                Console.Write(ex);
            }

            //Correo cr = new Correo();
            //string respuesta_correo = cr.EnviarCorreo("contacto.pveliz@gmail.com", "contacto.pveliz@gmail.com", "PLANIFICACION SP", html);
        }

        public void CorreoPra2()
        {
            usuarioEntity vend = new usuarioEntity();
            vend = (usuarioEntity)(Session["usuario"]);

            DBUtil db = new DBUtil();
            string html = "";

            html += "<h4>Estimado: " + vend.nombre_ + "</h4>";
            html += "<p>Se solicita revisar la siguiente SP:</p>";
            html += "<p><b>SP Nº: " + T_NUM_SP.Text + "</b></p>";
            html += "<p><b>Detalle: " + DETALLE_REPORTE.Text + "</p>";
            html += "<hr>";
            html += "<p>Atte. " + User.Identity.Name.ToString() + "</p>";

            usuarioEntity u = new usuarioEntity();
            u = (usuarioEntity)(Session["usuario"]);
            MailMessage email = new MailMessage();
            string correovendedor = db.Scalar("select correo from UsuarioWeb where cod_usuario = '" + COD_VENDEDOR.Text + "' ").ToString();
            email.To.Add(new MailAddress(u.correo));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "Problemas con SP Nº " + T_NUM_SP.Text + " de cliente: " + LBL_CLIENTE.Text + ", vendedor: " + LBL_VENDEDOR.Text;
            email.CC.Add("rmc@soprodi.cl");
            email.CC.Add("mazocar@soprodi.cl");
            email.CC.Add("pcataldo @soprodi.cl");
            email.CC.Add(correovendedor);
            email.Body = html;
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;
            email.BodyEncoding = System.Text.Encoding.UTF8;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            // ADJUNTAR PDF DE SP AQUI

            //
            //smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            smtp.Credentials = new NetworkCredential("informes.soprodi@gmail.com", "galaxia1234");
            try
            {
                //ACA ESTUVO EL TEBO
                string n_file = T_NUM_SP.Text.Trim() + ".pdf";
                string pdfPath = Server.MapPath(@"~\PDFs\") + n_file;
                Base.crear_sp_pdf(T_NUM_SP.Text.Trim(), pdfPath);
                System.IO.FileInfo toDownload = new System.IO.FileInfo(pdfPath);
                if (toDownload.Exists)
                {
                    email.Attachments.Add(new System.Net.Mail.Attachment(pdfPath));
                }
                //---------------------------------------------------------


                smtp.Send(email);
                email.Dispose();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('No enviado');</script>", false);
                Console.Write(ex);
            }


            //Correo cr = new Correo();
            //string respuesta_correo = cr.EnviarCorreo("contacto.pveliz@gmail.com", "contacto.pveliz@gmail.com", "SP CON PROBLEMAS", html);
        }

        public void CorreoPra4()
        {
            usuarioEntity vend = new usuarioEntity();
            vend = (usuarioEntity)(Session["usuario"]);

            DBUtil db = new DBUtil();
            string html = "";

            html += "<h4>Estimado: </h4>";
            html += "<p>Se solicita revisar la siguiente SP:</p>";
            html += "<p><b>SP Nº: " + T_NUM_SP.Text + "</b></p>";
            html += "<p><b>Detalle: " + T_DETALLE_COBRANZA.Text + "</p>";
            html += "<hr>";
            html += "<p>Atte. " + User.Identity.Name.ToString() + "</p>";

            usuarioEntity u = new usuarioEntity();
            u = (usuarioEntity)(Session["usuario"]);

            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(u.correo)); // <--------------------------- AQUI FALTA EL CORREO DE COBRANZA
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "SP PARA REVISAR Nº " + T_NUM_SP.Text + " de cliente: " + LBL_CLIENTE.Text + ", vendedor: " + LBL_VENDEDOR.Text;
            email.CC.Add("rmc@soprodi.cl");
            email.CC.Add("mazocar@soprodi.cl");
            email.CC.Add("rsolis@soprodi.cl");
            email.CC.Add("contador3@SOPRODI.CL");
            email.CC.Add("pcataldo @soprodi.cl");
            string correo_vendedor = db.Scalar("select correo from usuarioweb where cod_usuario = '" + COD_VENDEDOR.Text + "'").ToString();
            email.CC.Add(correo_vendedor);
            email.Body = html;
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;
            email.BodyEncoding = System.Text.Encoding.UTF8;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            // ADJUNTAR PDF DE SP AQUI

            //
            //smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            smtp.Credentials = new NetworkCredential("informes.soprodi@gmail.com", "galaxia1234");
            try
            {
                //ACA ESTUVO EL TEBO
                string n_file = T_NUM_SP.Text.Trim() + ".pdf";
                string pdfPath = Server.MapPath(@"~\PDFs\") + n_file;
                Base.crear_sp_pdf(T_NUM_SP.Text.Trim(), pdfPath);
                System.IO.FileInfo toDownload = new System.IO.FileInfo(pdfPath);
                if (toDownload.Exists)
                {
                    email.Attachments.Add(new System.Net.Mail.Attachment(pdfPath));
                }
                //---------------------------------------------------------


                smtp.Send(email);
                email.Dispose();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('No enviado');</script>", false);
                Console.Write(ex);
            }


            //Correo cr = new Correo();
            //string respuesta_correo = cr.EnviarCorreo("contacto.pveliz@gmail.com", "contacto.pveliz@gmail.com", "SP CON PROBLEMAS", html);
        }

        public void CorreoPra3()
        {
            string html = "";

            html += "<h4>Estimado: </h4>";
            html += "<p><b>SP Nº: " + T_NUM_SP.Text + "</b></p>";
            html += "<p><b>Estado: Rechazada</p>";
            html += "<hr>";
            html += "<p>Atte. " + User.Identity.Name.ToString() + "</p>";

            usuarioEntity u = new usuarioEntity();
            u = (usuarioEntity)(Session["usuario"]);

            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(u.correo));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "SP RECHAZADA, Nº SP " + T_NUM_SP.Text + " de cliente: " + LBL_CLIENTE.Text + ", vendedor: " + LBL_VENDEDOR.Text;
            email.CC.Add("rmc@soprodi.cl");
            email.CC.Add("mazocar@soprodi.cl");
            email.CC.Add("pcataldo @soprodi.cl");
            DBUtil db = new DBUtil();
            string correo_vendedor = db.Scalar("select correo from usuarioweb where cod_usuario = '" + COD_VENDEDOR.Text + "'").ToString();
            email.CC.Add(correo_vendedor);
            email.Body = html;
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;
            email.BodyEncoding = System.Text.Encoding.UTF8;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            // ADJUNTAR PDF DE SP AQUI

            //
            //smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            smtp.Credentials = new NetworkCredential("informes.soprodi@gmail.com", "galaxia1234");
            try
            {
                //ACA ESTUVO EL TEBO
                string n_file = T_NUM_SP.Text.Trim() + ".pdf";
                string pdfPath = Server.MapPath(@"~\PDFs\") + n_file;
                Base.crear_sp_pdf(T_NUM_SP.Text.Trim(), pdfPath);
                System.IO.FileInfo toDownload = new System.IO.FileInfo(pdfPath);
                if (toDownload.Exists)
                {
                    email.Attachments.Add(new System.Net.Mail.Attachment(pdfPath));
                }
                //---------------------------------------------------------


                smtp.Send(email);
                email.Dispose();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('No enviado');</script>", false);
                Console.Write(ex);
            }

        }

        public void alert(string mensaje, int flag)
        {
            // ROJO - VERDE 
            ScriptManager.RegisterStartupScript(this, typeof(Page), "mosnoti", "<script>javascript:MostrarNotificacion('" + mensaje + "', " + flag + ");</script>", false);
        }

        protected void B_VER_CUENTA_CORRIENTE_Click(object sender, EventArgs e)
        {

            string rutcliente = T_RUT_CLIENTE.Text;

            clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "xwte", "<script> fuera('" + encriptador.EncryptData(rutcliente) + "', '" + encriptador.EncryptData("25") + "') </script>", false);
        }

        protected void G_DETALLE_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string codproducto = (G_DETALLE.DataKeys[Convert.ToInt32(e.Row.RowIndex)].Values[0].ToString());
                string preciounit = (G_DETALLE.DataKeys[Convert.ToInt32(e.Row.RowIndex)].Values[1].ToString());
                string CodBodega = COD_BODEGA.Text;
                string fecha_lista = LBL_FECHA_EMISION.Text;

                DBUtil db = new DBUtil();
                DataTable dt = new DataTable();

                dt = db.consultar("select dbo.F_PrecioExcel('" + CodBodega + "', '" + codproducto + "', convert(date,'" + fecha_lista + "',103)); ");
                try
                {

                    string precio_lista = dt.Rows[0][0].ToString();
                    e.Row.Cells[2].Text = string.Format("{0:N1}", Convert.ToDouble(precio_lista));

                    // PRECIO LISTA
                    if (preciounit != precio_lista)
                    {
                        try
                        {
                            double x1 = Convert.ToDouble(preciounit);
                            double x2 = Convert.ToDouble(precio_lista);

                            if (x1 >= x2)
                            {
                                e.Row.Cells[3].ForeColor = Color.Green;
                            }
                            else
                            {
                                e.Row.Cells[3].ForeColor = Color.Red;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                catch { }

                // STOCK
                string stockabuscar = "";
                if (CodBodega == "ARICA" || CodBodega == "IQUIQUE" || CodBodega == "ARICA1SOP" || CodBodega == "ARICASOP")
                {
                    stockabuscar += " 'ARICASOP' ";
                }
                else if (CodBodega == "LOSANGELES")
                {
                    stockabuscar += " 'LOSANGELES' ";
                }
                else if (CodBodega == "LZ_DESPACHO" || CodBodega == "ZARATE" || CodBodega == "ZARATESOP" || CodBodega == "ABARROTES" || CodBodega == "ZARATE2T" || CodBodega == "ZARATE5T")
                {
                    stockabuscar += " 'ZARATESOP' ";
                }
                if (stockabuscar != "")
                {
                    string query = " select isnull(sum(sdc.TOTAL),0) as 'stock' from Stock_diario_CALCULADO sdc where sdc.invtid = '" + codproducto + "' and siteid in (" + stockabuscar + "); ";
                    string scalar_stock = db.Scalar(query).ToString();
                    if (Convert.ToDouble(scalar_stock) <= 0)
                    {
                        e.Row.Cells[4].Text = Convert.ToDouble(scalar_stock).ToString("0,#") + " <i class='fa fa-close'></i>";
                        e.Row.Cells[4].ForeColor = Color.Red;
                    }
                    else
                    {
                        e.Row.Cells[4].Text = Convert.ToDouble(scalar_stock).ToString("0,#") + " <i class='fa fa-check'></i>";
                        e.Row.Cells[4].ForeColor = Color.Green;
                    }
                }
                else
                {
                    e.Row.Cells[4].Text = "**";
                }
            }
        }

        protected void B_ENVIAR_COBRANZA_Click(object sender, EventArgs e)
        {
            try
            {
                sp_estados_internosEntity sps = new sp_estados_internosEntity();
                sps.cod_usuario = User.Identity.Name.ToString();
                sps.estado = "Pendiente Cobranza";
                sps.fecha = DateTime.Now;
                sps.num_sp = T_NUM_SP.Text;
                sps.nota_correo = T_DETALLE_COBRANZA.Text;

                if (sp_estados_internosBO.registrar(sps) == "OK")
                {
                    CorreoPra4();
                    Button1_Click(sender, e);
                    UpdatePanel2.Update();
                    alert("Correo enviado a Cobranza Correctamente", 1);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "cierramodal", "<script>javascript:CerrarModal_ReportarSP();</script>", false);
                }

            }
            catch (Exception ex)
            {
                alert("Problemas al planificar SP Nº: " + T_NUM_SP.Text, 0);
            }
        }
    }
}