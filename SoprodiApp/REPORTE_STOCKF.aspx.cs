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
    public partial class REPORTE_STOCKF : System.Web.UI.Page
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

            page = this.Page;
            if (!IsPostBack)
            {
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "33" || u_ne.Trim() == "40")
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
                //DateTime t = DateTime.Now;
                //DateTime t2 = DateTime.Now;
                ////////t = new DateTime(t.Year, t.Month - 6, 1);               
                //txt_desde.Text = ReporteRNegocio.CALCULA_DESDE(t.Month, t.Year);
                //txt_hasta.Text = t2.ToShortDateString();

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

                DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now;
                //////t = new DateTime(t.Year, t.Month - 6, 1);               
                txt_desde.Text = "01" + t.AddMonths(-3).ToShortDateString().Substring(2);
                txt_hasta.Text = t2.ToShortDateString();



                string grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

                if (grupos_del_usuario == "")
                {
                    grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
                }

                List<string> grupos = grupos_del_usuario.Split(',').ToList();


                string clase = " ";

                if (grupos.Count == 4)
                {

                    clase += " where b.glclassid in ('INSU', 'CANI', 'ABAR', 'MANI')  ";

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

                Session["clase"] = clase;



            }
        }




        private void cargar_combo_bodega(DataTable dt, DataView dtv)
        {
            //dt.Rows.Add(new Object[] { -1, "-- Todos --" });
            dtv = dt.DefaultView;
            dtv.Sort = "siteid";
            d_bodega.DataSource = dtv;
            d_bodega.DataTextField = "siteid";
            d_bodega.DataValueField = "siteid";
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



        [WebMethod]
        public static string GUARDA_STOCK_FUTURO(string cod_prod, string cantidad)
        {


            string respt = "";
            try
            {
                respt = ReporteRNegocio.insert_futuro_stock(cod_prod, DateTime.Now.ToShortDateString(), cantidad);
            }
            catch { respt = "Error :02 Al guardar Producto"; }
            //}

            return respt;
        }


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




        protected void btn_informe_Click(object sender, EventArgs e)
        {
            bool fechas = false;
            if (txt_desde.Text == "")
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "tewqeees", "<script>alert('Debe seleccionar desde') </script>", false);


            }
            if (txt_hasta.Text == "")
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "tewqeees", "<script>alert('Debe seleccionar hasta') </script>", false);


            }
            if (txt_hasta.Text != "" && txt_desde.Text != "")
            {
                fechas = true;

            }

            if (fechas)
            {
                string bodega = agregra_comillas(l_grupos.Text);
                string productos = agregra_comillas(l_vendedores.Text);
                string where = " and 1=1";
                string where2 = " and 1=1";
                string periodo = " where 1=1 ";


                string produc = "";



                string grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

                if (grupos_del_usuario == "")
                {
                    grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
                }

                List<string> grupos = grupos_del_usuario.Split(',').ToList();


                string clase = " ";

                if (grupos.Count == 4)
                {
                    clase += " where b.glclassid in ('INSU', 'CANI', 'ABAR', 'MANI')  ";


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



                if (bodega == "" && productos != "")
                {





                    DataTable dt2 = ReporteRNegocio.listar_ALL_productos_stock(" and  invtid in (" + productos + ")", clase);
                    foreach (DataRow ro in dt2.Rows)
                    {
                        produc += agregra_comillas(ro[0].ToString().Trim()) + ",";
                    }
                    produc = "q.InvtID in (" + produc.Substring(0, produc.Length - 1) + ") ";

                }
                else if (bodega == "" && productos == "")
                {

                }
                else if (bodega != "" && productos == "")
                {
                    DataTable dt2 = ReporteRNegocio.listar_ALL_productos_stock("and siteid in (" + bodega + ")", clase);
                    foreach (DataRow ro in dt2.Rows)
                    {
                        produc += agregra_comillas(ro[0].ToString().Trim()) + ",";
                    }
                    produc = "q.InvtID in (" + produc.Substring(0, produc.Length - 1) + ") ";
                }

                if (bodega != "")
                {
                    where += " and siteid in (" + agregra_comillas(bodega) + ")";
                    where2 += " and bodega in (" + agregra_comillas(bodega) + ")";
                }

                if (productos != "")
                {
                    where2 += " and producto in (" + agregra_comillas(productos) + ") ";
                    where += " and invtid in (" + agregra_comillas(productos) + ") ";
                }

                string fecharcp = "";
                string fecharcp2 = "";
                string fechaSaldo = "";
                string compra = " 1=1 ";

                string desde_thx = "";
                string hasta_thx = "";
                string futuro = "";
                if (txt_desde.Text != "")
                {
                    periodo += " and periodo >= " + obtener_periodo(txt_desde.Text);
                    where += " and trandate >= convert(datetime, '" + txt_desde.Text + "',103)";
                    fechaSaldo += " and q.trandate <= convert(datetime, '" + txt_desde.Text + "',103)";
                    compra += " and RcptDate >= convert(datetime, '" + txt_desde.Text + "',103)";
                    futuro += " and fecha >= convert(datetime, '" + txt_desde.Text + "',103)";
                    desde_thx = txt_desde.Text;
                }

                if (txt_hasta.Text != "")
                {
                    periodo += " and periodo <= " + obtener_periodo(txt_hasta.Text);
                    where += " and trandate <= convert(datetime, '" + txt_hasta.Text + "',103)";
                    fecharcp2 += " and q.trandate <= convert(datetime, '" + txt_hasta.Text + "',103)";
                    compra += " and RcptDate <= convert(datetime, '" + txt_hasta.Text + "',103)";
                    futuro += " and fecha <= convert(datetime, '" + txt_hasta.Text + "',103)";
                    hasta_thx = txt_hasta.Text;
                }

                Session["SW_FILTRAR_PRODUCTO"] = "si";

                DataTable ventas_periodo = ReporteRNegocio.periodos_productos(periodo);
                DataView vie = new DataView(ventas_periodo);
                vie.Sort = "periodo asc";
                DataTable periodos = vie.ToTable(true, "periodo");
                //SPSP
              
                DataTable ayer_venta = new DataTable();

                if (produc != "") { produc = " and " + produc; }
                DataTable ultcompra = new DataTable();

                div_report.Visible = true;

                DataTable dt_final = new DataTable();

                DataTable dt_pivot = ReporteRNegocio.productos_stock_sp(where.Replace(" and trandate >= convert(datetime, '" + txt_desde.Text + "',103)", ""), fecharcp2, fechaSaldo, desde_thx, hasta_thx, futuro, clase);
                DataView view = new DataView(dt_pivot);
                DataTable bodegas = view.ToTable(true, "Bodega");
                DataView view1 = new DataView(dt_pivot);
                DataTable productos1 = view.ToTable(true, "codprod", "descrip", "tipounidad");

                DataTable dt_comp = ReporteRNegocio.compras_gen(compra, "");

                string productos_where = "";

                if (productos != "")
                {
                    string productos_where1 = " and invtid in (";
                    foreach (DataRow r in productos1.Rows)
                    {
                        productos_where1 += agregra_comillas(r[0].ToString()) + ",";
                    }



                    productos_where = productos_where1.Substring(0, productos_where1.Length - 1) + ") ";

                }

                string bodegas_WHERE = "";
                if (bodega != "")
                {
                    string bodegas_WHERE1 = "  and (select f.codbodega from [new_thx].[dbo].[VPEDIDOCABECERA_NEW_THX] f where f.CodDocumento = A.CodDocumento) in (";
                    foreach (DataRow r in bodegas.Rows)
                    {
                        bodegas_WHERE1 += agregra_comillas(r[0].ToString()) + ",";

                    }

                    bodegas_WHERE = bodegas_WHERE1.Substring(0, bodegas_WHERE1.Length - 1) + ") ";

                }

                DataTable SP_SINCRONI = ReporteRNegocio.sincronizadas_detalle(where+productos_where, productos_where.Replace("invtid", "codproducto"), txt_hasta.Text , bodegas_WHERE);
                
                //DataTable excel = ReporteRNegocio.PIVOT_EXCEL(" where 1=1");
                double Z_VA_I = 0;
                double Z_VA_F = 0;
                double Z_COMPRAS_TOTAL = 0;
                double Z_COSTOS = 0;
                double Z_VENTAS = 0;
                double Z_PESOS_VENTAS = 0;
                string HTML = "";

                HTML += "<table id='TABLA_REPORTE2' class='table table-advance table-bordered fill-head tablesorter filtrar' style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";
                HTML += "<thead>";
                HTML += "<tr>";

                HTML += "<th colspan=3; class='test sorter-false headerfijo' >" + txt_desde.Text + " a " + txt_hasta.Text + "</th>";
                foreach (DataRow r in bodegas.Rows)
                {
                    HTML += "<th colspan=1;  class='test sorter-false' ></th>";

                }
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                foreach (DataRow r in periodos.Rows)
                {
                    HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                    HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                    HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                }
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";


                HTML += "</tr>";

                HTML += "<tr>";
                HTML += "<th colspan=3; class='test sorter-false' ></th>";

                foreach (DataRow r in bodegas.Rows)
                {
                    HTML += "<th colspan=1;  class='test sorter-false' >" + r[0].ToString().Trim() + "</th>";
                }
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                foreach (DataRow t in periodos.Rows)
                {
                    HTML += "<th colspan=1;  class='test sorter-false' >" + t[0].ToString().Trim() + "</th>";
                    HTML += "<th colspan=1;  class='test sorter-false' ></th>";

                }
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";

                foreach (DataRow t in periodos.Rows)
                {
                    HTML += "<th colspan=1;  class='test sorter-false' >" + t[0].ToString().Trim() + "</th>";

                }



                HTML += "</tr>";
                HTML += "<tr>";
                HTML += "<th colspan=1; class='test'  >CODPROD.</th>";
                HTML += "<th colspan=1; class='test'  >PRODUCTO</th>";
                HTML += "<th colspan=1; class='test'  >T.UNIDAD</th>";
                foreach (DataRow r in bodegas.Rows)
                {
                    HTML += "<th colspan=1;  class='test' >Stock</th>";
                    //HTML += "<th colspan=1;  class='test' >SaldoInicial</th>";
                    //HTML += "<th colspan=1;  class='test' >Compras</th>";

                }
                HTML += "<th colspan=1;  class='test' >Saldo Inicial </th>";
                HTML += "<th colspan=1;  class='test' >T.Compra </th>";
                HTML += "<th colspan=1;  class='test' >T.Stock Físico </th>";
                HTML += "<th colspan=1;  class='test' >SP Stock </th>";
                HTML += "<th colspan=1;  class='test' >Disponible</th>";
                HTML += "<th colspan=1;  class='test' >C.Transito</th>";
                HTML += "<th colspan=1;  class='test' >Disp.Final</th>";

                foreach (DataRow r in periodos.Rows)
                {
                    HTML += "<th colspan=1;  class='test' ></th>";
                    HTML += "<th colspan=1;  class='test' >AJ</th>";
                }

                HTML += "<th colspan=1;  class='test' >MesesDeVenta</th>";
                HTML += "<th colspan=1;  class='test' >SaldoFinalTeórico</th>";


                foreach (DataRow r in periodos.Rows)
                {
                    HTML += "<th colspan=1;  class='test' ></th>";

                }

                HTML += "</tr>";
                HTML += "</thead>";
                HTML += "<tbody>";

                foreach (DataRow r in productos1.Rows)
                {



                    HTML += "<tr>";
                    HTML += "<td>" + r[0].ToString().Trim() + "</td>";
                    HTML += "<td class='leftfijo' style='white-space: nowrap;'> " + r[1].ToString().Trim() + "</td>";
                    HTML += "<td>" + r[2].ToString().Trim() + "</td>";
                    string color1 = "";

                    double sum_producto_total = 0;
                    double total_prod_compra = 0;
                    double total_prod_saldo_inicial = 0;


                    string colortotal = "background-color: #deb2ff;";
                    string colorSP = "background-color: #deb8ff;";
                    bool cambia_color = false;

                    foreach (DataRow r2 in bodegas.Rows)
                    {

                        if (cambia_color)
                        {
                            color1 = "background-color: rgb(220, 160, 160);";
                            cambia_color = false;
                        }
                        else {
                            color1 = "background-color: #9defba;";
                            cambia_color = true;
                        }
                        DataRow[] venta = dt_pivot.Select("'" + r2[0].ToString().Trim() + "'  = bodega and codprod = '" + r[0].ToString().Trim() + "'");
                        if (venta.Length > 0)
                        {
                            foreach (DataRow row in venta)
                            {
                                ///>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> STOCK  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                string stock_bodega = Base.monto_format(row[5].ToString().Trim());


                                string script6 = string.Format("javascript:fuera_stock(&#39;{0}&#39;, &#39;{1}&#39;);return;", r[0].ToString().Trim() + "*" + txt_desde.Text + "*" + txt_hasta.Text + "*" + r2[0].ToString().Trim(), "91");
                                //HTML += "<td style='" + color1 + "'>  <a href='javascript:' onclick='" + script2 + "'>" + stock_compra + "</a></td>";


                                if (stock_bodega != "0,00")
                                {
                                    HTML += "<td style='" + color1 + "'><a href='javascript:' onclick='" + script6 + "'>" + stock_bodega.Replace(",00", "") + "</a></td>";
                                }
                                else {
                                    HTML += "<td style='" + color1 + "'></td>";

                                }
                                double aux_stock = 0;
                                try
                                {
                                    aux_stock = Convert.ToDouble(stock_bodega.Replace(",00", "").Replace(".", ""));

                                }
                                catch { }
                                ///>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> SALDO INICIAL  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                ///
                                if (r2[0].ToString().Trim() != "ARICA1SOP")
                                {
                                    sum_producto_total += aux_stock;
                                }
                                string stock_inicial = Base.monto_format(row[6].ToString().Trim());

                                //if (stock_inicial != "0,00")
                                //{
                                //    //HTML += "<td style='" + color1 + "'>" + stock_inicial.Replace(",00", "") + "</td>";
                                //}
                                //else {
                                //    //HTML += "<td style='" + color1 + "'></td>";

                                //}

                                double aux_saldo = 0;
                                try
                                {
                                    aux_saldo = Convert.ToDouble(stock_inicial.Replace(",00", "").Replace(".", ""));

                                }
                                catch { }

                                if (r2[0].ToString().Trim() != "ARICA1SOP")
                                {
                                    total_prod_saldo_inicial += aux_saldo;
                                }

                            }
                        }

                        else
                        {
                            //HTML += "<td style='" + color1 + "'></td>";
                            HTML += "<td style='" + color1 + "'></td>";
                        }


                        ///>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> COMPRA  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                        DataRow[] COMPRA = dt_comp.Select("'" + r2[0].ToString().Trim() + "'  = SiteID and InvtID = '" + r[0].ToString().Trim() + "'");
                        if (COMPRA.Length > 0)
                        {
                            double stock_compra = 0;
                            foreach (DataRow row in COMPRA)
                            {
                                double aux_stock = 0;
                                try
                                {
                                    aux_stock = Convert.ToDouble(row[1].ToString());

                                }
                                catch { }
                                stock_compra += aux_stock;
                            }

                            //string script2 = string.Format("javascript:fuera_stock(&#39;{0}&#39;, &#39;{1}&#39;);return;", r[0].ToString().Trim() + "*"+ txt_desde.Text + "*" + txt_hasta.Text + "*"+ r2[0].ToString().Trim(), "89");



                            //HTML += "<td style='" + color1 + "'>  <a href='javascript:' onclick='" + script2 + "'>" + stock_compra + "</a></td>";

                            double aux_COMPRA = 0;
                            try
                            {
                                aux_COMPRA = Convert.ToDouble(stock_compra);

                            }
                            catch { }

                            if (r2[0].ToString().Trim() != "ARICA1SOP")
                            {
                                total_prod_compra += aux_COMPRA;
                            }


                        }
                        else
                        {
                            //HTML += "<td style='" + color1 + "'></td>";
                        }


                    }
                    ///>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> TOTAL SALDOS INICIAL  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    HTML += "<td style='" + colortotal + "'>" + Base.monto_format2(total_prod_saldo_inicial) + "</td>";

                    ///>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> TOTAL COMPRAS  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    string script2 = string.Format("javascript:fuera_stock(&#39;{0}&#39;, &#39;{1}&#39;);return;", r[0].ToString().Trim() + "*" + txt_desde.Text + "*" + txt_hasta.Text + "*", "89");

                    HTML += "<td style='" + colortotal + "'><a href='javascript:' onclick='" + script2 + "'>" + Base.monto_format2(total_prod_compra) + "</a></td>";


                    ///>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> TOTAL STOCK  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    string total = Base.monto_format2(sum_producto_total);

                    HTML += "<td style='" + colortotal + "'>" + total + "</td>";

                    ///>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> SP STOCK  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    double total_stock = 0;
                    DataRow[] sp = SP_SINCRONI.Select("codproducto = '" + r[0].ToString().Trim() + "'");
                    if (sp.Length > 0)
                    {

                        foreach (DataRow row in sp)
                        {
                            double aux_stock = 0;
                            try
                            {
                                aux_stock = Convert.ToDouble(row[1].ToString().Replace(".", ","));

                            }
                            catch { }
                            total_stock += aux_stock;
                        }

                    }
                    string script66 = string.Format("javascript:fuera_sp(&#39;{0}&#39;, &#39;{1}&#39;);return;", r[0].ToString().Trim() + "*" + txt_desde.Text + "*" + txt_hasta.Text + "*" + l_grupos.Text, "91");


                    //href = "REPORTE_SP.aspx?G=NO"
                    HTML += "<td style='" + colorSP + "'>  <a href='REPORTE_SP.aspx?G=91&C=" + r[0].ToString().Trim() + " * " + txt_desde.Text + " * " + txt_hasta.Text + " * " + l_grupos.Text + "' target='_blank'>" + Base.monto_format2(total_stock) + " </a> </td>";
                    ///>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> STOCK DISPONIBLE  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    double disponible_stock = sum_producto_total - total_stock;

                    HTML += "<td style='" + colortotal + "'>" + Base.monto_format2(disponible_stock) + "</td>";




                    ///>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> TEXTBOX COMPRAS STOCK  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    double cmpra_transito = 0;

                    HTML += "<td style='" + colortotal + "'> ";


                    HTML += "<table  style='" + colortotal + "'>";


                    HTML += "<tr  style='" + colortotal + "'>";
                    HTML += "<td  style='" + colortotal + "'>";
                    DataRow[] venta3 = dt_pivot.Select("codprod = '" + r[0].ToString().Trim() + "'");
                    if (venta3.Length > 0)
                    {
                        foreach (DataRow row in venta3)
                        {

                            try
                            {

                                cmpra_transito += Convert.ToDouble(row[7].ToString());
                            }
                            catch (Exception f) { }

                            HTML += "<input type='text' id='txt_cod_" + r[0].ToString().Trim() + "' value='" + Base.monto_format(row[7].ToString().Trim()) + "' style='width: 75px; display:block;'> ";
                            break;
                        }
                    }
                    else {

                        HTML += "<input type='text' id='txt_cod_" + r[0].ToString().Trim() + "' style='width: 75px; display:block;'> ";

                    }
                    HTML += "</td>";
                    HTML += "<td  style='" + colortotal + "'>";
                    HTML += "<div onclick='guarda_txt(" + r[0].ToString().Trim() + ");' style='width:20px;'><i class='fa fa-save fa-2x'></i></div>";
                    HTML += "</td>";
                    HTML += "</tr>";
                    HTML += "</table>";

                    HTML += "  </td> ";

                    ///>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> DISPONIBLE FINAL  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    HTML += "<td style='" + colortotal + "'>" + Base.monto_format2(disponible_stock + cmpra_transito) + "</td>";



                    ///>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> STOCK EN PERIODOS  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    string color_periodos_cant = "background-color: gainsboro;";
                    double suma_cant_meses = 0;
                    double cant_periodos = periodos.Rows.Count;
                    double PROMEDIO_CANTIDAD_VENDIDA = 0;

                    foreach (DataRow pe in periodos.Rows)
                    {
                        DataRow[] venta_x_periodo = ventas_periodo.Select(pe[0].ToString() + " = periodo and producto = '" + r[0].ToString().Trim() + "'");
                        if (venta_x_periodo.Length > 0)
                        {
                            foreach (DataRow row in venta_x_periodo)
                            {


                                double aux_venta2 = 0;
                                try
                                {
                                    aux_venta2 = Convert.ToDouble(row[1].ToString().Replace(",00", "").Replace(".", ""));

                                }
                                catch { }

                                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                                string script4 = string.Format("javascript:fuera_stock(&#39;{0}&#39;, &#39;{1}&#39;);return;", encriptador.EncryptData(r[0].ToString() + "*" + pe[0].ToString() + "*" + l_grupos.Text), "90");



                                HTML += "<td style='" + color_periodos_cant + "'>   <a href='javascript:' onclick='" + script4 + "'>" + Base.monto_format2(aux_venta2) + " </a></td>";
                                suma_cant_meses += aux_venta2;


                                double aux_venta3 = 0;
                                try
                                {
                                    aux_venta3 = Convert.ToDouble(row[4].ToString().Replace(",00", "").Replace(".", ""));

                                }
                                catch { }
                                ///Ajuste

                                string script7 = string.Format("javascript:fuera_stock(&#39;{0}&#39;, &#39;{1}&#39;);return;", r[0].ToString().Trim() + "*" + txt_desde.Text + "*" + txt_hasta.Text + "*" + pe[0].ToString(), "92");
                                HTML += "<td style='" + color_periodos_cant + "'>   <a href='javascript:' onclick='" + script7 + "'>" + Base.monto_format2(aux_venta3) + " </a></td>";

                            }

                        }
                        else {

                            HTML += "<td style='" + color_periodos_cant + "'>0</td>";
                            HTML += "<td style='" + color_periodos_cant + "'>0</td>";
                        }

                    }
                    PROMEDIO_CANTIDAD_VENDIDA = suma_cant_meses / cant_periodos;


                    double stock_d_prom = sum_producto_total / PROMEDIO_CANTIDAD_VENDIDA;

                    string dato_atcmes = Base.monto_format2(stock_d_prom);

                    if (dato_atcmes == "NaN" || dato_atcmes == "∞")
                    {

                        dato_atcmes = "";
                    }
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>ANT MESES>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    HTML += "<td style='background-color: darkgoldenrod;'>" + dato_atcmes + "</td>";

                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>SALDO FINAL TEORICO  :: Saldo Inicial + Compras - Ventas.   >>>>>>>>>>>>>>>>>>>>>>>>>

                    double final_teorico = total_prod_saldo_inicial + total_prod_compra - suma_cant_meses;

                    HTML += "<td style='background-color: darkcyan;'>" + Base.monto_format2(final_teorico) + "</td>";



                    ///>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> VENTA EN PERIODOS  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    string color_periodos = "background-color: #oeb3ff;";
                    foreach (DataRow pe in periodos.Rows)
                    {
                        DataRow[] venta_x_periodo = ventas_periodo.Select(pe[0].ToString() + " = periodo and producto = '" + r[0].ToString().Trim() + "'");
                        if (venta_x_periodo.Length > 0)
                        {
                            foreach (DataRow row in venta_x_periodo)
                            {

                                double aux_venta = 0;
                                try
                                {
                                    aux_venta = Convert.ToDouble(row[0].ToString().Replace(",00", "").Replace(".", ""));

                                }
                                catch { }


                                HTML += "<td style='" + color_periodos + "'>" + Base.monto_format2(aux_venta) + "</td>";

                            }

                        }
                        else {

                            HTML += "<td style='" + color_periodos + "'>0</td>";

                        }

                    }



                }

                HTML += "</tbody>";
                HTML += "  </table>";

                report_stock.InnerHtml = HTML;


                string where3 = " and 1=1";

                if (l_grupos.Text != "")
                {
                    where3 += " and siteid in (" + agregra_comillas(l_grupos.Text) + ")";
                }
                DataTable dt = new DataTable();
                try
                {

                    dt = ReporteRNegocio.listar_ALL_productos_stock(where3, clase);
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

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teweees", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_INFORME_TOTAL_VENDEDOR')); </script>", false);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teadsaeee", "<script> SortPrah(); </script>", false);
            }
        }

        private string obtener_periodo(string text)
        {
            List<string> datos_fecha = text.Split('/').ToList();

            if (datos_fecha[1].ToString().Length == 1)
            {

                datos_fecha[1] = "0" + datos_fecha[1];
            }
            string año_mes = datos_fecha[2] + datos_fecha[1];



            return año_mes;

        }

        [WebMethod]
        public static string cambia_tipo_pago_(string factura, string estado)
        {
            string aasdf = "";
            ReporteRNegocio.delete_estado_sp(factura);
            ReporteRNegocio.insert_estado_sp(factura, estado);

            return "";
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

        protected void G_INFORME_TOTAL_VENDEDOR_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //string cliente = e.Row.Cells[0].Text;

                //clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                string script1 = string.Format("javascript:fuera_stock(&#39;{0}&#39;, &#39;{1}&#39;);return false;", e.Row.Cells[0].Text.Trim(), e.Row.Cells[8].Text.Trim());
                //e.Row.Cells[1].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[1].Text + " </a>";
                string script2 = string.Format("javascript:fuera_stock(&#39;{0}&#39;, &#39;{1}&#39;);return false;", e.Row.Cells[0].Text.Trim(), e.Row.Cells[11].Text.Trim());

                e.Row.Cells[7].Text = e.Row.Cells[7].Text.Substring(0, 10);

                e.Row.Cells[1].Attributes["style"] = "white-space: nowrap;";

                //e.Row.Cells[6].Text = (Convert.ToDouble(e.Row.Cells[6].Text)).ToString("G20");
                //e.Row.Cells[5].Text = (Convert.ToDouble(e.Row.Cells[5].Text)).ToString("G20");
                try
                {
                    e.Row.Cells[8].Text = e.Row.Cells[8].Text.Substring(0, 10);
                }
                catch
                {
                    e.Row.Cells[8].Text = "";
                }
                try
                {
                    e.Row.Cells[8].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[8].Text + " </a>";
                }
                catch
                {
                    e.Row.Cells[8].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[8].Text + " </a>";

                }

                try
                {
                    e.Row.Cells[11].Text = e.Row.Cells[11].Text.Substring(0, 10);
                }
                catch
                {
                    e.Row.Cells[11].Text = "";
                }
                try
                {
                    e.Row.Cells[11].Text = "  <a href='javascript:' onclick='" + script2 + "'>" + e.Row.Cells[11].Text + " </a>";
                }
                catch
                {
                    e.Row.Cells[11].Text = "  <a href='javascript:' onclick='" + script2 + "'>" + e.Row.Cells[11].Text + " </a>";

                }

                e.Row.Cells[14].Text = Base.monto_format(e.Row.Cells[14].Text);


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

            report_stock.RenderControl(htmlWrite);

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

            List<string> grupos = grupos_del_usuario.Split(',').ToList();


            string clase = " ";

            if (grupos.Count == 4)
            {

                clase += " where b.glclassid in ('INSU', 'CANI', 'ABAR', 'MANI')  ";

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

            DataTable dt; DataView dtv = new DataView();
            cargar_combo_bodega(ReporteRNegocio.listar_ALL_bodegas_stock(), dtv);
            cargar_combo_producto(ReporteRNegocio.listar_ALL_productos_stock("", clase), dtv);


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
    }
}