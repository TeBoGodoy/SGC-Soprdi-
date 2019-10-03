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
    public partial class REPORTE_STOCK : System.Web.UI.Page
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
                    if (u_ne.Trim() == "25")
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
                string produc = "";
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
                string fecha_thx = "";
                string hasta_excel_in = "";
                string hasta_excel_fin = "";
                string desde_thx = "";
                string hasta_thx = "";
                if (txt_desde.Text != "")
                {

                    fecharcp += " and rcptdate >= convert(datetime, '" + txt_desde.Text + "',103)";
                    fecha_thx += " and fechafactura >= convert(datetime, '" + txt_desde.Text + "',103)";
                    hasta_excel_in = " and fecha <= convert(datetime, '" + txt_desde.Text + "', 103) ";
                    desde_thx = txt_desde.Text;
                }

                if (txt_hasta.Text != "")
                {

                    fecharcp += " and rcptdate <= convert(datetime, '" + txt_hasta.Text + "',103)";
                    fecharcp2 += " and rcptdate <= convert(datetime, '" + txt_hasta.Text + "',103)";
                    fecha_thx += " and fechafactura <= convert(datetime, '" + txt_hasta.Text + "',103)";
                    hasta_excel_fin = " and fecha <= convert(datetime, '" + txt_hasta.Text + "', 103) ";
                    hasta_thx = txt_hasta.Text;
                }
                if (hasta_excel_in == "")
                {
                    hasta_excel_in = " and fecha=(select min(fecha) from stock_excel where cod_producto = b.invtid) ";

                }
                if (hasta_excel_fin == "")
                {
                    hasta_excel_fin = " and fecha=(select max(fecha) from stock_excel where cod_producto = b.invtid) ";

                }

                DataTable ayer_venta = ReporteRNegocio.ventas_ayer_anteayer(where2, desde_thx, hasta_thx);

                DataTable ajustes = ReporteRNegocio.ajustes(where, fecha_thx);

                if (produc != "") { produc = " and " + produc; }
                DataTable ultcompra = ReporteRNegocio.ultima_compra2(fecharcp2, produc, fecha_thx, desde_thx, hasta_thx);
                div_report.Visible = true;
                //G_INFORME_TOTAL_VENDEDOR.Visible = true;
                DataTable dt_final = new DataTable();
                DataTable dt_pivot = ReporteRNegocio.productos_stock(where, fecharcp, fecha_thx, hasta_excel_in, hasta_excel_fin, desde_thx, hasta_thx);
                DataView view = new DataView(dt_pivot);
                DataTable bodegas = view.ToTable(true, "Bodega");
                DataView view1 = new DataView(dt_pivot);
                DataTable productos1 = view.ToTable(true, "codprod", "descrip", "tipounidad");

                string periodos = ReporteRNegocio.listar_periodos_(" where FechaFactura >= CONVERT(datetime,'" + txt_desde.Text + "', 103)  and FechaFactura <= CONVERT(datetime,'" + txt_hasta.Text + "',103)");
                //aux = ReporteRNegocio.listar_resumen_periodo(where);
                List<string> periodos_list = periodos.Split(',').ToList();
                int colum = periodos_list.Count;



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

                HTML += "<th colspan=3; class='test sorter-false' >" + txt_desde.Text + " a " + txt_hasta.Text + "</th>";
                foreach (DataRow r in bodegas.Rows)
                {
                    HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                    HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                    HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                    HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                    HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                    HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                    HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                    HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                    HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                }
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' >Z_VA_I</th>";
                HTML += "<th colspan=1;  class='test sorter-false' >Z_VA_F</th>";
                HTML += "<th colspan=1;  class='test sorter-false' >Z_COMPRAS_TOTAL</th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' >Z_COSTOS</th>";
                HTML += "<th colspan=1;  class='test sorter-false' >Z_VENTAS</th>";
                HTML += "<th colspan=1;  class='test sorter-false' >Z_PESOS_VENTAS</th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' style='white-space: nowrap;' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' style='white-space: nowrap;' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false'  style='white-space: nowrap;' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false'  style='white-space: nowrap;' >UTILIDAD__TOTAL</th>";
                HTML += "<th colspan=1;  class='test sorter-false'  style='white-space: nowrap;' >PORCENTAJE_TOTAL</th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "<th colspan=1;  class='test sorter-false' ></th>";
                HTML += "</tr>";



                HTML += "<tr>";
                HTML += "<th colspan=3; class='test sorter-false' ></th>";

                foreach (DataRow r in bodegas.Rows)
                {
                    HTML += "<th colspan=9;  class='test sorter-false' >" + r[0].ToString().Trim() + "</th>";
                }
                HTML += "<th colspan=25;  class='test sorter-false'> TOTALES</th>";
                HTML += "</tr>";
                HTML += "<tr>";
                HTML += "<th colspan=1; class='test'  >COD_PRODUCTO</th>";
                HTML += "<th colspan=1; class='test'  >PRODUCTO</th>";
                HTML += "<th colspan=1; class='test'  >T.UNIDAD</th>";
                foreach (DataRow r in bodegas.Rows)
                {
                    HTML += "<th colspan=1;  class='test' >S.I.</th>";
                    HTML += "<th colspan=1;  class='test' >S.F.</th>";
                    HTML += "<th colspan=1;  class='test' >C.I.</th>";
                    HTML += "<th colspan=1;  class='test' >C.F.</th>";
                    HTML += "<th colspan=1;  class='test' >VAL.I.</th>";
                    HTML += "<th colspan=1;  class='test' >VAL.F.</th>";
                    HTML += "<th colspan=1;  class='test' >UniVen</th>";
                    HTML += "<th colspan=1;  class='test' >Vent$</th>";
                    HTML += "<th colspan=1;  class='test' >VentPeso</th>";
                }
                HTML += "<th colspan=1;  class='test' >ZS.I.(A)</th>";
                HTML += "<th colspan=1;  class='test' >ZS.F.(D)</th>";
                HTML += "<th colspan=1;  class='test' >ZVAL.I.(H)</th>";
                HTML += "<th colspan=1;  class='test' >ZVAL.F.(I)</th>";
                HTML += "<th colspan=1;  class='test' >Zcomp(G)</th>";
                HTML += "<th colspan=1;  class='test' >Unicomp(C)</th>";
                HTML += "<th colspan=1;  class='test' >$Uni</th>";
                HTML += "<th colspan=1;  class='test' >Moneda</th>";
                HTML += "<th colspan=1;  class='test' >CostImport</th>";
                HTML += "<th colspan=1;  class='test' >Vent$(F)</th>";
                HTML += "<th colspan=1;  class='test' >VentPeso</th>";
                HTML += "<th colspan=1;  class='test' >UniVen(B)</th>";
                HTML += "<th colspan=1;  class='test' >UniProm(E)</th>";
                HTML += "<th colspan=1;  class='test' style='white-space: nowrap;' >Dif.(A - B + C)</th>";
                HTML += "<th colspan=1;  class='test' style='white-space: nowrap;' >Ajustes</th>";
                HTML += "<th colspan=1;  class='test'  style='white-space: nowrap;' >Rotac.(D/E)</th>";
                HTML += "<th colspan=1;  class='test'  style='white-space: nowrap;' >Utilidad(F-G-H+I)</th>";
                HTML += "<th colspan=1;  class='test'  style='white-space: nowrap;' >Porc%((Ut / F) * 100)</th>";
                HTML += "<th colspan=1;  class='test' >AyerVen$</th>";
                HTML += "<th colspan=1;  class='test' >AyerUtilidad</th>";
                HTML += "<th colspan=1;  class='test' >AntesAyerVen$</th>";
                HTML += "<th colspan=1;  class='test' >AntesAyerUtilidad</th>";

                HTML += "</tr>";
                HTML += "</thead>";
                HTML += "<tbody>";

                foreach (DataRow r in productos1.Rows)
                {

                    DataRow[] ayer_y_antes = ayer_venta.Select("'" + r[0].ToString().Trim() + "'  = producto");


                    double ayer = 0;
                    double antes_ayer = 0;
                    bool ayer_ = true;

                    foreach (DataRow rr in ayer_y_antes)
                    {
                        if (ayer_)
                        {
                            try
                            {
                                ayer = Convert.ToDouble(rr[0].ToString());
                                ayer_ = false;
                            }
                            catch { }
                        }
                        else
                        {
                            try
                            {
                                antes_ayer = Convert.ToDouble(rr[0].ToString());
                                break;
                            }
                            catch { }
                        }
                    }


                    HTML += "<tr>";
                    HTML += "<td>" + r[0].ToString().Trim() + "</td>";
                    HTML += "<td style='white-space: nowrap;'> " + r[1].ToString().Trim() + "</td>";
                    HTML += "<td>" + r[2].ToString().Trim() + "</td>";
                    string color1 = "";
                    double sum_si = 0;
                    double sum_sf = 0;
                    double sum_val = 0;
                    double sum_val_f = 0;

                    double sum_ve_dolar = 0;
                    double sum_ve_peso = 0;
                    double sum_un_vendi = 0;
                    double sum_prom_uni = 0;

                    bool cambia_color = false;
                    string color_rojo = ";color:red;";
                    foreach (DataRow r2 in bodegas.Rows)
                    {
                        if (cambia_color)
                        {
                            color1 = "background-color: bisque;";
                            cambia_color = false;
                        }
                        else
                        {
                            color1 = "background-color: rgb(167, 178, 255);";
                            cambia_color = true;
                        }
                        DataRow[] venta = dt_pivot.Select("'" + r2[0].ToString().Trim() + "'  = bodega and codprod = '" + r[0].ToString().Trim() + "'");
                        if (venta.Length > 0)
                        {
                            foreach (DataRow row in venta)
                            {
                                string stock_inicial = row[5].ToString().Trim();
                                string stock_final = row[6].ToString().Trim();
                                string costo = row[7].ToString().Trim();
                                string costo_f = row[8].ToString().Trim();
                                string unvend = row[9].ToString().Trim();
                                string vent_d = row[10].ToString().Trim();
                                string vent_p = row[11].ToString().Trim();
                                if (stock_inicial == "0,00") { stock_inicial = "0"; }
                                if (stock_final == "0,00") { stock_final = "0"; }

                                if (stock_inicial == "") { stock_inicial = "0"; }
                                if (stock_final == "") { stock_final = "0"; }

                                //STOCK INICIAL
                                if (stock_inicial.Contains("-"))
                                {
                                    HTML += "<td style='" + color1 + color_rojo + "'>" + Base.monto_format(stock_inicial) + "</td>";
                                }
                                else { HTML += "<td style='" + color1 + "'>" + Base.monto_format(stock_inicial) + "</td>"; }
                                //STOCK FINAL
                                if (stock_final.Contains("-"))
                                {
                                    HTML += "<td style='" + color1 + color_rojo + "'>" + Base.monto_format(stock_final) + "</td>";
                                }
                                else { HTML += "<td style='" + color1 + "'>" + Base.monto_format(stock_final) + "</td>"; }
                                //COSTO I
                                HTML += "<td style='" + color1 + "'>" + Base.monto_format(costo) + "</td>";
                                //COSTO F
                                HTML += "<td style='" + color1 + "'>" + Base.monto_format(costo_f) + "</td>";

                                //VA I  Y SUM
                                if (costo != "")
                                {

                                    if ((Convert.ToDouble(stock_inicial) * Convert.ToDouble(costo)).ToString().Contains("-"))
                                    {
                                        HTML += "<td style='" + color1 + color_rojo + "'>" + Base.monto_format2(Convert.ToDouble(stock_inicial) * Convert.ToDouble(costo)) + "</td>";
                                    }
                                    else { HTML += "<td style='" + color1 + "'>" + Base.monto_format2(Convert.ToDouble(stock_inicial) * Convert.ToDouble(costo)) + "</td>"; }
                                    sum_val += Convert.ToDouble(stock_inicial) * Convert.ToDouble(costo);
                                }
                                else { HTML += "<td style='" + color1 + "'></td>"; }
                                //VA F  Y SUM
                                if (costo_f != "")
                                {

                                    if ((Convert.ToDouble(stock_final) * Convert.ToDouble(costo_f)).ToString().Contains("-"))
                                    {
                                        HTML += "<td style='" + color1 + color_rojo + "'>" + Base.monto_format2(Convert.ToDouble(stock_final) * Convert.ToDouble(costo_f)) + "</td>";
                                    }
                                    else { HTML += "<td style='" + color1 + "'>" + Base.monto_format2(Convert.ToDouble(stock_final) * Convert.ToDouble(costo_f)) + "</td>"; }
                                    sum_val_f += Convert.ToDouble(stock_final) * Convert.ToDouble(costo_f);
                                }


                                else { HTML += "<td style='" + color1 + "'></td>"; }
                                // SUMA SI
                                if (stock_inicial == "0")
                                {
                                    stock_inicial = "";
                                }
                                else { sum_si += Convert.ToDouble(stock_inicial); }
                                //SUMA SF
                                if (stock_final == "0")
                                {
                                    stock_final = "";
                                }
                                else { sum_sf += Convert.ToDouble(stock_final); }
                                // UNIDAD VEND X BODEGA
                                if (unvend.Contains("-"))
                                {
                                    HTML += "<td style='" + color1 + color_rojo + "'>" + Base.monto_format(unvend) + "</td>";
                                }
                                else { HTML += "<td style='" + color1 + "'>" + Base.monto_format(unvend) + "</td>"; }
                                //VENTAS DOLAR X BODEGA
                                if (vent_d.Contains("-"))
                                {
                                    HTML += "<td style='" + color1 + color_rojo + "'>" + Base.monto_format(vent_d) + "</td>";
                                }
                                else { HTML += "<td style='" + color1 + "'>" + Base.monto_format(vent_d) + "</td>"; }
                                // VENTAS PESO X BODEGA
                                if (vent_p.Contains("-"))
                                {
                                    HTML += "<td style='" + color1 + color_rojo + "'>" + Base.monto_format(vent_p) + "</td>";
                                }
                                else { HTML += "<td style='" + color1 + "'>" + Base.monto_format(vent_p) + "</td>"; }

                                double dolar_AUX = 0;
                                double peso_AUX = 0;
                                double un_vendi_AUX = 0;


                                //SUM DOLAR
                                try
                                {
                                    dolar_AUX = Convert.ToDouble(vent_d);
                                    sum_ve_dolar += dolar_AUX;
                                }
                                catch
                                {
                                }
                                //SUM PESO
                                try
                                {
                                    peso_AUX = Convert.ToDouble(vent_p);
                                    sum_ve_peso += peso_AUX;
                                }
                                catch
                                {
                                }
                                //SUM UNIDADES VENDIDAS
                                try
                                {
                                    un_vendi_AUX = Convert.ToDouble(unvend);
                                    sum_un_vendi += un_vendi_AUX;
                                }
                                catch
                                {
                                }
                            }
                        }
                        else
                        {
                            HTML += "<td style='" + color1 + "'></td>";
                            HTML += "<td style='" + color1 + "'></td>";
                            HTML += "<td style='" + color1 + "'></td>";
                            HTML += "<td style='" + color1 + "'></td>";
                            HTML += "<td style='" + color1 + "'></td>";
                            HTML += "<td style='" + color1 + "'></td>";
                            HTML += "<td style='" + color1 + "'></td>";
                            HTML += "<td style='" + color1 + "'></td>";
                            HTML += "<td style='" + color1 + "'></td>";
                        }

                    }
                    //SUM SI
                    if (sum_si.ToString().Contains("-"))
                    {
                        HTML += "<td style='background-color: rgb(207, 255, 174); " + color_rojo + "'> " + Base.monto_format2(sum_si) + "</td>";
                    }
                    else { HTML += "<td style='background-color: rgb(207, 255, 174);'>" + Base.monto_format2(sum_si) + "</td>"; }
                    //SUM SF
                    if (sum_sf.ToString().Contains("-"))
                    {
                        HTML += "<td style='background-color: rgb(207, 255, 174); " + color_rojo + "'> " + Base.monto_format2(sum_sf) + "</td>";
                    }
                    else { HTML += "<td style='background-color: rgb(207, 255, 174);'>" + Base.monto_format2(sum_sf) + "</td>"; }
                    //SUM VAL I
                    if (sum_val.ToString().Contains("-"))
                    {
                        HTML += "<td style='background-color: rgb(207, 255, 174); " + color_rojo + "'> " + Base.monto_format2(sum_val) + "</td>";
                    }
                    else { HTML += "<td style='background-color: rgb(207, 255, 174);'>" + Base.monto_format2(sum_val) + "</td>"; }
                    try { Z_VA_I += sum_val; } catch { }


                    //SUM VAL F
                    if (sum_val_f.ToString().Contains("-"))
                    {
                        HTML += "<td style='background-color: rgb(207, 255, 174); " + color_rojo + "'> " + Base.monto_format2(sum_val_f) + "</td>";
                    }
                    else { HTML += "<td style='background-color: rgb(207, 255, 174);'>" + Base.monto_format2(sum_val_f) + "</td>"; }
                    try { Z_VA_F += sum_val_f; } catch { }


                    double cant_vend = 0;
                    double prom_periodo = 0;
                    double venta_ = 0;
                    double compra = 0;
                    double uni_compras = 0;

                    DataRow[] venta2 = ultcompra.Select("invtid = '" + r[0].ToString().Trim() + "'");
                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                    string script4 = string.Format("javascript:fuera_stock(&#39;{0}&#39;, &#39;{1}&#39;);return;", encriptador.EncryptData(r[0].ToString().Trim() + "*" + fecha_thx), "88");

                    if (venta2.Length > 0)
                    {
                        foreach (DataRow row in venta2)
                        {
                            string script2 = string.Format("javascript:fuera_stock(&#39;{0}&#39;, &#39;{1}&#39;);return;", row[4].ToString(), "1");
                            string script3 = string.Format("javascript:fuera_stock(&#39;{0}&#39;, &#39;{1}&#39;);return;", row[4].ToString(), "2");

                            //COMPRAS Z
                            if (row[0].ToString().Trim().Contains("-"))
                            {

                                HTML += "<td style='background-color: rgb(207, 255, 174); " + color_rojo + "' href='javascript:' onclick='" + script2 + "' >" + Base.monto_format(row[0].ToString().Trim()) + "</td>";
                            }
                            else
                            {

                                HTML += "<td style='background-color: rgb(207, 255, 174);' href='javascript:' onclick='" + script2 + "'>" + Base.monto_format(row[0].ToString().Trim()) + "</td>";
                            }
                            try { Z_COMPRAS_TOTAL += Convert.ToDouble(row[0]); } catch { }


                            //CANTIDAD COMPARA
                            if (row[5].ToString().Trim().Contains("-"))
                            {

                                HTML += "<td style='background-color: rgb(207, 255, 174); " + color_rojo + "' >" + Base.monto_format(row[5].ToString().Trim()) + "</td>";
                            }
                            else
                            {

                                HTML += "<td style='background-color: rgb(207, 255, 174);' >" + Base.monto_format(row[5].ToString().Trim()) + "</td>";
                            }
                            //VALOR UNIDAD
                            if (row[1].ToString().Trim().Contains("-"))
                            {

                                HTML += "<td style='background-color: rgb(207, 255, 174); " + color_rojo + "' >" + Base.monto_format(row[1].ToString().Trim()) + "</td>";
                            }
                            else
                            {

                                HTML += "<td style='background-color: rgb(207, 255, 174);' >" + Base.monto_format(row[1].ToString().Trim()) + "</td>";
                            }
                            //MONEDA
                            if (row[2].ToString().Trim().Contains("-"))
                            {

                                HTML += "<td style='background-color: rgb(207, 255, 174); " + color_rojo + "' >" + row[2].ToString().Trim() + "</td>";
                            }
                            else
                            {

                                HTML += "<td style='background-color: rgb(207, 255, 174);' >" + row[2].ToString().Trim() + "</td>";
                            }
                            //COSTO IMPORTA
                            try { Z_COSTOS += Convert.ToDouble(row[3]); } catch { }


                            if (row[3].ToString().Trim().Contains("-"))
                            {

                                HTML += "<td style='background-color: rgb(207, 255, 174); " + color_rojo + "'  href='javascript:' onclick='" + script3 + "' >" + Base.monto_format(row[3].ToString().Trim()) + "</td>";
                            }
                            else
                            {

                                HTML += "<td style='background-color: rgb(207, 255, 174);'  href='javascript:' onclick='" + script3 + "' >" + Base.monto_format(row[3].ToString().Trim()) + "</td>";
                            }
                            //////////VENTAS DOLAR
                            ////////try { Z_VENTAS += Convert.ToDouble(row[4]); } catch { }


                            ////////if (row[4].ToString().Trim().Contains("-"))
                            ////////{

                            ////////    HTML += "<td style='background-color: rgb(207, 255, 174); " + color_rojo + "'  href='javascript:' onclick='" + script4 + "' >" + Base.monto_format(row[4].ToString().Trim()) + "</td>";
                            ////////}
                            ////////else {

                            ////////    HTML += "<td style='background-color: rgb(207, 255, 174);'  href='javascript:' onclick='" + script4 + "' >" + Base.monto_format(row[4].ToString().Trim()) + "</td>";
                            ////////}
                            //////////VENTA PESOS
                            ////////try { Z_PESOS_VENTAS += Convert.ToDouble(row[9]); } catch { }

                            ////////if (row[9].ToString().Trim().Contains("-"))
                            ////////{

                            ////////    HTML += "<td style='background-color: rgb(207, 255, 174); " + color_rojo + "'  href='javascript:' onclick='" + script4 + "' >" + Base.monto_format(row[9].ToString().Trim()) + "</td>";
                            ////////}
                            ////////else {

                            ////////    HTML += "<td style='background-color: rgb(207, 255, 174);'  href='javascript:' onclick='" + script4 + "' >" + Base.monto_format(row[9].ToString().Trim()) + "</td>";
                            ////////}
                            //////////CANT VENDI
                            ////////if (row[5].ToString().Trim().Contains("-"))
                            ////////{

                            ////////    HTML += "<td style='background-color: rgb(207, 255, 174); " + color_rojo + "' >" + Base.monto_format(row[5].ToString().Trim()) + "</td>";
                            ////////}

                            ////////else {

                            ////////    HTML += "<td style='background-color: rgb(207, 255, 174);' >" + Base.monto_format(row[5].ToString().Trim()) + "</td>";
                            ////////}
                            //////////PROMEDIO UNIDAD VENDIDAS
                            ////////if (row[6].ToString().Trim().Contains("-"))
                            ////////{

                            ////////    HTML += "<td style='background-color: rgb(207, 255, 174); " + color_rojo + "' >" + Base.monto_format(row[6].ToString().Trim()) + "</td>";
                            ////////}
                            ////////else {

                            ////////    HTML += "<td style='background-color: rgb(207, 255, 174);' >" + Base.monto_format(row[6].ToString().Trim()) + "</td>";
                            ////////}

                            //HTML += "<td style='background-color: rgb(207, 255, 174);' >" + row[1].ToString().Trim() + "</td>";
                            //HTML += "<td style='background-color: rgb(207, 255, 174);' >" + row[2].ToString().Trim() + "</td>";
                            //HTML += "<td style='background-color: rgb(207, 255, 174);'>" + row[3].ToString().Trim() + "</td>";
                            //HTML += "<td style='background-color: rgb(207, 255, 174);' >" + row[4].ToString().Trim() + "</td>";
                            //HTML += "<td style='background-color: rgb(207, 255, 174);' >" + row[9].ToString().Trim() + "</td>";
                            //HTML += "<td style='background-color: rgb(207, 255, 174);'>" + row[5].ToString().Trim() + "</td>";
                            //HTML += "<td style='background-color: rgb(207, 255, 174);'>" + row[6].ToString().Trim() + "</td>";
                            try
                            {
                                uni_compras = Convert.ToDouble(row[5].ToString().Trim());
                            }
                            catch { uni_compras = 0; }
                            //try
                            //{
                            //    prom_periodo = Convert.ToDouble(row[6].ToString().Trim());
                            //}
                            //catch { prom_periodo = 0; }
                            //try
                            //{
                            //    cant_vend = Convert.ToDouble(row[5].ToString().Trim());
                            //}
                            //catch { cant_vend = 0; }
                            //try
                            //{
                            //    venta_ = Convert.ToDouble(row[4].ToString().Trim());
                            //}
                            //catch { venta_ = 0; }
                            try
                            {
                                compra = Convert.ToDouble(row[0].ToString().Trim());
                            }
                            catch { compra = 0; }

                        }
                    }
                    else
                    {
                        HTML += "<td style='background-color: rgb(207, 255, 174);' ></td>";
                        HTML += "<td style='background-color: rgb(207, 255, 174);' ></td>";
                        HTML += "<td style='background-color: rgb(207, 255, 174);' ></td>";
                        HTML += "<td style='background-color: rgb(207, 255, 174);' ></td>";
                        HTML += "<td style='background-color: rgb(207, 255, 174);' ></td>";
                        //HTML += "<td style='background-color: rgb(207, 255, 174);' ></td>";
                        //HTML += "<td style='background-color: rgb(207, 255, 174);' ></td>";
                        //HTML += "<td style='background-color: rgb(207, 255, 174);' ></td>";
                        //HTML += "<td style='background-color: rgb(207, 255, 174);' ></td>";
                    }

                    //VENTAS DOLAR
                    try { Z_VENTAS += sum_ve_dolar; } catch { }


                    if (sum_ve_dolar < 0)
                    {

                        HTML += "<td style='background-color: rgb(207, 255, 174); " + color_rojo + "'  href='javascript:' onclick='" + script4 + "' >" + Base.monto_format2(sum_ve_dolar) + "</td>";
                    }
                    else
                    {

                        HTML += "<td style='background-color: rgb(207, 255, 174);'  href='javascript:' onclick='" + script4 + "' >" + Base.monto_format2(sum_ve_dolar) + "</td>";
                    }
                    //VENTA PESOS

                    try
                    {
                        venta_ = sum_ve_peso;
                    }
                    catch { venta_ = 0; }


                    try { Z_PESOS_VENTAS += sum_ve_peso; } catch { }

                    if (sum_ve_peso < 0)
                    {

                        HTML += "<td style='background-color: rgb(207, 255, 174); " + color_rojo + "'  href='javascript:' onclick='" + script4 + "' >" + Base.monto_format2(sum_ve_peso) + "</td>";
                    }
                    else
                    {

                        HTML += "<td style='background-color: rgb(207, 255, 174);'  href='javascript:' onclick='" + script4 + "' >" + Base.monto_format2(sum_ve_peso) + "</td>";
                    }
                    //CANT VENDI


                    try
                    {
                        cant_vend = sum_un_vendi;
                    }
                    catch { cant_vend = 0; }

                    if (sum_un_vendi < 0)
                    {

                        HTML += "<td style='background-color: rgb(207, 255, 174); " + color_rojo + "' >" + Base.monto_format2(sum_un_vendi) + "</td>";
                    }


                    else
                    {

                        HTML += "<td style='background-color: rgb(207, 255, 174);' >" + Base.monto_format2(sum_un_vendi) + "</td>";
                    }
                    //PROMEDIO UNIDAD VENDIDAS

                    sum_prom_uni = sum_un_vendi / colum;

                    try
                    {
                        prom_periodo = sum_prom_uni;
                    }
                    catch { prom_periodo = 0; }

                    if (sum_prom_uni < 0)
                    {

                        HTML += "<td style='background-color: rgb(207, 255, 174); " + color_rojo + "' >" + Base.monto_format2(sum_prom_uni) + "</td>";
                    }
                    else
                    {

                        HTML += "<td style='background-color: rgb(207, 255, 174);' >" + Base.monto_format2(sum_prom_uni) + "</td>";
                    }







                    // difencia 
                    if ((Convert.ToDouble(sum_si) - cant_vend + uni_compras).ToString().Contains("-"))
                    {
                        HTML += "<td  style='background-color: rgb(207, 255, 174);" + color_rojo + "'>" + Base.monto_format((Convert.ToDouble(sum_si) - cant_vend + uni_compras).ToString()) + "</td>";
                    }
                    else
                    {
                        HTML += "<td  style='background-color: rgb(207, 255, 174); '>" + Base.monto_format((Convert.ToDouble(sum_si) - cant_vend + uni_compras).ToString()) + "</td>";
                    }
                    string rot = Math.Round(sum_sf / prom_periodo, 3).ToString();
                    if (rot == "NaN" || rot == "-∞" || rot == "∞" || rot == "Infinito") { rot = "0"; }


                    //ajustes
                    DataRow[] ajus = ajustes.Select("invtid = '" + r[0].ToString().Trim() + "'");

                    if (ajus.Length > 0)
                    {
                        foreach (DataRow row in ajus)
                        {
                            if (row[0].ToString().Contains("-"))
                            {
                                HTML += "<td  style='background-color: rgb(207, 255, 174);" + color_rojo + "'>" + Base.monto_format(row[0].ToString()) + "</td>";
                            }
                            else
                            {
                                HTML += "<td  style='background-color: rgb(207, 255, 174); '>" + Base.monto_format(row[0].ToString()) + "</td>";
                            }


                        }
                    }
                    else {
                        HTML += "<td style='background-color: rgb(207, 255, 174);' ></td>";

                    }

                    //rotacion
                    if (rot.Contains("-"))
                    {
                        HTML += "<td  style='background-color: rgb(207, 255, 174);" + color_rojo + "' >" + Base.monto_format(rot) + "</td>";
                    }
                    else
                    {
                        HTML += "<td  style='background-color: rgb(207, 255, 174);' >" + Base.monto_format(rot) + "</td>";
                    }

                    //utilidad
                    if ((venta_ - compra - sum_val + sum_val_f).ToString().Contains("-"))
                    {
                        HTML += "<td  style='background-color: rgb(207, 255, 174);" + color_rojo + "' >" + Base.monto_format((venta_ - compra - sum_val + sum_val_f).ToString()) + "</td>";
                    }
                    else
                    {
                        HTML += "<td  style='background-color: rgb(207, 255, 174);'>" + Base.monto_format((venta_ - compra - sum_val + sum_val_f).ToString()) + "</td>";
                    }

                    //PORCENTAJE
                    string porc = ((((venta_) - (compra) - (sum_val) + (sum_val_f)) / venta_) * 100).ToString("N0") + "%";
                    if (porc == "NaN%" || porc == "-∞%" || porc == "∞%" || porc == "Infinito%") { porc = "0%"; }

                    if (porc.Contains("-"))
                    {
                        HTML += "<td  style='background-color: rgb(207, 255, 174);" + color_rojo + "' >" + porc + "</td>";
                    }
                    else
                    {
                        HTML += "<td  style='background-color: rgb(207, 255, 174);' >" + porc + "</td>";
                    }


                    //venta ayer
                    if (ayer.ToString().Contains("-"))
                    {
                        HTML += "<td  style='background-color: rgb(156, 255, 100);" + color_rojo + "' >" + Base.monto_format2(ayer) + "</td>";
                    }
                    else
                    {
                        HTML += "<td  style='background-color: rgb(156, 255, 100);'>" + Base.monto_format2(ayer) + "</td>";
                    }
                    //utilidad ayer
                    if ((ayer - compra - sum_val + sum_val_f).ToString().ToString().Contains("-"))
                    {
                        HTML += "<td  style='background-color: rgb(156, 255, 100);" + color_rojo + "' >" + Base.monto_format((ayer - compra - sum_val + sum_val_f).ToString()) + "</td>";
                    }
                    else
                    {

                        HTML += "<td  style='background-color: rgb(156, 255, 100);' >" + Base.monto_format((ayer - compra - sum_val + sum_val_f).ToString()) + "</td>";
                    }
                    //venta antes ayer
                    if (antes_ayer.ToString().Contains("-"))
                    {
                        HTML += "<td  style='background-color: rgb(156, 255, 100);" + color_rojo + "' >" + Base.monto_format2(antes_ayer) + "</td>";
                    }
                    else
                    {

                        HTML += "<td  style='background-color: rgb(156, 255, 100);' >" + Base.monto_format2(antes_ayer) + "</td>";
                    }

                    // utilidad antes ayer
                    if ((antes_ayer - compra - sum_val + sum_val_f).ToString().Contains("-"))
                    {
                        HTML += "<td  style='background-color: rgb(156, 255, 100);" + color_rojo + "' >" + Base.monto_format((antes_ayer - compra - sum_val + sum_val_f).ToString()) + "</td>";
                    }
                    else
                    {
                        HTML += "<td  style='background-color: rgb(156, 255, 100);'>" + Base.monto_format((antes_ayer - compra - sum_val + sum_val_f).ToString()) + "</td>";
                    }

                    HTML += "</tr>";


                }

                HTML += "</tbody>";
                HTML += "  </table>";
                HTML = HTML.Replace("Z_VA_I", Base.monto_format2(Z_VA_I));
                HTML = HTML.Replace("Z_VA_F", Base.monto_format2(Z_VA_F));
                HTML = HTML.Replace("Z_COMPRAS_TOTAL", Base.monto_format2(Z_COMPRAS_TOTAL));
                HTML = HTML.Replace("Z_COSTOS", Base.monto_format2(Z_COSTOS));
                HTML = HTML.Replace("Z_VENTAS", Base.monto_format2(Z_VENTAS));
                HTML = HTML.Replace("Z_PESOS_VENTAS", Base.monto_format2(Z_PESOS_VENTAS));
                double utilidad_total = ((Z_VENTAS) - (Z_COMPRAS_TOTAL) - (Z_VA_I) + (Z_VA_F));
                HTML = HTML.Replace("UTILIDAD__TOTAL", Base.monto_format2(utilidad_total));
                HTML = HTML.Replace("PORCENTAJE_TOTAL", Base.monto_format2((utilidad_total / Z_VENTAS) * 100) + "%");



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


            //where b.glclassid in ('ABAR', 'MANI')


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