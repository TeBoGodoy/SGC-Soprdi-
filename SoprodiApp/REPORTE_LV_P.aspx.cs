using SoprodiApp.negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
//using FTP_Soprodi.BusinessLayer;

namespace SoprodiApp
{
    public partial class REPORTE_LV_P : System.Web.UI.Page
    {

        public static string grupos;
        private static string grupos2;
        private static string USER;
        public static string vendedor;
        public static string bodega;




        protected void Page_Load(object sender, EventArgs e)
        {

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();

            if (!IsPostBack)
            {
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "18" || u_ne.Trim() == "17")
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

                DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now;
                //t = new DateTime(t.Year, t.Month - 6, 1);
                txt_desde.Text = ReporteRNegocio.CALCULA_DESDE(t.Month, t.Year);
                txt_hasta.Text = t2.ToShortDateString();

                USER = User.Identity.Name.ToString();

                string vendedor = ReporteRNegocio.esvendedor(User.Identity.Name.ToString());
                if (vendedor == "2")
                {
                    filtros.Visible = false;
                    l_vendedores.Text = User.Identity.Name.ToString();
                    grupos = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
                    grupos2 = grupos;
                    l_grupos.Text = grupos;
                    btn_productos_Click(sender, e);

                }
                else
                {
                    grupos = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));
                    grupos2 = grupos;
                }

                if (grupos2.Contains("Granos")) {

                    categoria.Visible = false;

                }


                cargar_vendedores();
                //cargar_combo_clientes();
                cargar_bodegas();
                cargar_categoria();
            }
        }

        private void cargar_categoria()
        {

            string where = " ";

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.carga_categoria(where);
            dtv = dt.DefaultView;
            cb_categoria.DataSource = dtv;
            cb_categoria.DataTextField = "nom_categ";
            cb_categoria.DataValueField = "id";
            //d_vendedor_.SelectedIndex = -1;
            cb_categoria.DataBind();
        }

        private void cargar_bodegas()
        {

            string where = " where FechaFactura >= CONVERT(datetime,'" + txt_desde.Text + "', 103) " +
                            " and FechaFactura <= CONVERT(datetime,'" + txt_hasta.Text + "',103) and user1 in (" + grupos + ")";

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.carga_bodega(where);
            dtv = dt.DefaultView;
            d_grupos_usuario.DataSource = dtv;
            d_grupos_usuario.DataTextField = "nom_bodega";
            d_grupos_usuario.DataValueField = "nom_bodega";
            //d_vendedor_.SelectedIndex = -1;
            d_grupos_usuario.DataBind();
        }

        private void cargar_vendedores()
        {

            string where = " where FechaFactura >= CONVERT(datetime,'" + txt_desde.Text + "', 103) " +
                            " and FechaFactura <= CONVERT(datetime,'" + txt_hasta.Text + "',103) and user1 in (" + grupos + ")";

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.listar_ALL_vendedores(where);
            DataView dv = dt.DefaultView;
            dv.Sort = "nom_vend";
            dtv = dt.DefaultView;
            d_vendedor_.DataSource = dtv;
            d_vendedor_.DataTextField = "nom_vend";
            d_vendedor_.DataValueField = "cod_vend";
            //d_vendedor_.SelectedIndex = -1;
            d_vendedor_.DataBind();

        }

        protected void G_LV_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btn_productos_Click(object sender, EventArgs e)
        {

            string DESDE = txt_desde.Text;
            string HASTA = txt_hasta.Text;

            DESDE = DESDE.Replace("-", "/");
            HASTA = HASTA.Replace("-", "/");

            DateTime hasta_ = Convert.ToDateTime(DateTime.Now.ToShortDateString(), new CultureInfo("es-ES"));
            string ayer = hasta_.AddDays(-1).ToShortDateString().Replace("-", "/");

            DateTime hasta_2 = Convert.ToDateTime(DateTime.Now.ToShortDateString(), new CultureInfo("es-ES"));
            string ante_ayer = hasta_2.AddDays(-2).ToShortDateString().Replace("-", "/");

            string where_dias = " where FechaFactura in (CONVERT(datetime,'" + DateTime.Now.ToShortDateString().Replace("-", "/") + "', 103),CONVERT(datetime,'" + ayer + "', 103) ,CONVERT(datetime,'" + ante_ayer + "', 103)) and user1 in (" + grupos + ")";


            string bodegas = l_grupos.Text;
            string vendedo = l_vendedores.Text;

            string where = " where FechaFactura >= CONVERT(datetime,'" + DESDE + "', 103) " +
                            " and FechaFactura <= CONVERT(datetime,'" + HASTA + "',103) and user1 in (" + grupos + ")";


            if (bodegas != "")
            {
                where_dias = where_dias + " and bodega in (" + agregra_comillas(bodegas) + ")";
                where = where + " and bodega in (" + agregra_comillas(bodegas) + ")";
                bodega = bodegas;
            }
            if (vendedo != "")
            {
                where = where + " and codvendedor in (" + agregra_comillas(vendedo) + ")";
                where_dias = where_dias + " and codvendedor in (" + agregra_comillas(vendedo) + ")";
                vendedor = vendedo;
            }
            if (l_categoria.Text != "") {

                where = where + " and producto in (SELECT producto from V_PRODUCTO_CATEGORIA where categoria in ( " + l_categoria.Text+ ")) ";
                where_dias = where_dias + " and producto in (SELECT producto from V_PRODUCTO_CATEGORIA where categoria in ( " + l_categoria.Text + ")) ";
            }


            DataTable productos_dias = ReporteRNegocio.productos_dias(where_dias);
            DataTable productos_periodos = ReporteRNegocio.productos_periodos(where );

            DataTable productos_peridos = new DataTable();
            productos_peridos.Columns.Add("descproducto");
            productos_peridos.Columns.Add("suma");
            productos_peridos.Columns.Add("periodo");
            productos_peridos.Columns.Add("fechafactura");

            foreach (DataRow r in productos_dias.Rows) 
            {
                DataRow newRow = productos_peridos.NewRow();
                newRow["descproducto"] = r["descproducto"];
                newRow["suma"] = r["suma"];
                newRow["periodo"] = r["periodo"];
                newRow["fechafactura"] = r["fechafactura"];

                productos_peridos.Rows.Add(newRow);
            }

            foreach (DataRow r in productos_periodos.Rows)
            {
                DataRow newRow = productos_peridos.NewRow();
                newRow["descproducto"] = r["descproducto"];
                newRow["suma"] = r["suma"];
                newRow["periodo"] = r["periodo"];
                newRow["fechafactura"] = r["fechafactura"];

                productos_peridos.Rows.Add(newRow);
            }



            DataView view = new DataView(productos_periodos);
            DataTable periodos = view.ToTable(true, "periodo");
            DataView dv3 = periodos.DefaultView;
            dv3.Sort = "periodo desc";
            periodos = dv3.ToTable();

            //PRODUCTOS
            string html_body = crear_reporte_correo2(productos_dias, productos_periodos, periodos);
            tabla.Visible = true;
            div4.Visible = true;
            DivMainContent.InnerHtml = html_body;

            filtro_memoria_div.InnerHtml = "<div class='btn-toolbar pull-left'><input type='text' id='t_filtro_memoria' style='width: 200px; margin-right: 7px; padding: 5px;' placeholder='Filtrar...' class='form-control' /></div>";


            //volver a cargar


            DataTable dt = new DataTable();

            string where2 = " where FechaFactura >= CONVERT(datetime,'" + DESDE + "', 103) " +
                          " and FechaFactura <= CONVERT(datetime,'" + HASTA + "',103)  and user1 in (" + grupos + ")";


            if (l_grupos.Text != "") { where2 += " and bodega in (" + agregra_comillas(l_grupos.Text) + ")"; }


            try
            {
                dt = ReporteRNegocio.listar_ALL_vendedores(where2);
                //dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
                DataView dv2 = dt.DefaultView;
                dv2.Sort = "cod_vend";
                dt = dv2.ToTable();
                d_vendedor_.DataSource = dt;
                d_vendedor_.DataTextField = "nom_vend";
                d_vendedor_.DataValueField = "cod_vend";
                //d_vendedor_.SelectedIndex = -1;
                d_vendedor_.DataBind();


                foreach (ListItem item in d_vendedor_.Items)
                {

                    if (l_vendedores.Text.Contains(item.Value.ToString()))
                    {
                        item.Selected = true;
                    }
                }
            }
            catch { }


            foreach (ListItem item in d_grupos_usuario.Items)
            {

                if (l_grupos.Text.Contains(item.Value.ToString()))
                {
                    item.Selected = true;
                }
            }


            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teassxxaeee", "<script> SortPrah(); </script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "temdas1213p", "<script language='javascript'>superfiltro();</script>", false);

        }


        public string crear_reporte_correo2(DataTable total_dias, DataTable total_periodos, DataTable periodos)
        {

            DataView view = new DataView(total_periodos);
            DataTable productos = view.ToTable(true, "descproducto");
            DataView dv3 = productos.DefaultView;
            dv3.Sort = "descproducto";
            productos = dv3.ToTable();

            List<string> dias = new List<string>();

            dias.Add(DateTime.Now.ToShortDateString().Replace("-", "/"));

            DateTime hasta_ = Convert.ToDateTime(DateTime.Now.ToShortDateString(), new CultureInfo("es-ES"));
            string ayer = hasta_.AddDays(-1).ToShortDateString().Replace("-", "/");
            dias.Add(ayer);
            DateTime hasta_2 = Convert.ToDateTime(DateTime.Now.ToShortDateString(), new CultureInfo("es-ES"));
            string ante_ayer = hasta_2.AddDays(-2).ToShortDateString().Replace("-", "/");
            dias.Add(ante_ayer);

            //HTML_EXCEL
            string HTML_EXCEL = "";
            string color_letra_excel = "white";
            string color_fondo_excel = "#428BCA";
            string HTML = "";

            HTML += "<table id='TABLA_REPORTE' class='table table-advance table-bordered fill-head tablesorter filtrar' style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";
            HTML += "<thead>";
            HTML += "<tr>";

            HTML_EXCEL += "<table id='T_EXCEL1' border=1>";
            HTML_EXCEL += "<tr style='background-color:" + color_fondo_excel + "; color:" + color_letra_excel + "'>";


            bool primer = true;

            HTML += "<th colspan=1; class='test sorter-false' style='border-right: 2px solid rgb(50, 48, 48); border-bottom: 2px solid rgb(50, 48, 48);' ></th>";
            HTML += "<th colspan=3;  class='test2 sorter-false' style='border-right: 2px solid rgb(50, 48, 48); border-bottom: 2px solid rgb(50, 48, 48);' >3 Dias</th>";
            HTML += "<th colspan=" + 3 * periodos.Rows.Count + 1+";  class='test sorter-false' style='border-right: 2px solid rgb(50, 48, 48); border-bottom: 2px solid rgb(50, 48, 48);' >Periodos</th>";
            HTML += "</tr>";
            HTML += "<tr>";


            HTML_EXCEL += "<td></td>";
            HTML_EXCEL += "<td colspan=3>3 Dias</td>";
            HTML_EXCEL += "<td colspan=" + 3 * periodos.Rows.Count +1+ ">Periodos</td>";
            HTML_EXCEL += "</tr>";
            HTML_EXCEL += "<tr style='background-color:" + color_fondo_excel + "; color:" + color_letra_excel + "'>";

            int con = 0;

            while (con < 3)
            {
                if (primer)
                {
                    HTML += "<th colspan=1; class='test sorter-false' style='border-right: 2px solid rgb(50, 48, 48);' ><b><p class='text-left'>DescProducto </p></b></th>";
                    HTML_EXCEL += "<td>DescProducto</td>";
                    primer = false;
                }


                DataRow[] venta = total_dias.Select("'" + dias[con] + "'  = fechafactura");

                double suma = 0;

                if (venta.Length > 0)
                {
                    foreach (DataRow row in venta)
                    {
                        suma += double.Parse(row[1].ToString());
                    }

                }

                HTML += "<th colspan=1;  class='test2 sorter-false' style='border-right: 2px solid rgb(50, 48, 48);' > " + suma.ToString("N0") + "</th>";

                HTML_EXCEL += "<td> " + suma.ToString("N0") + "</td>";
                con++;
            }


            foreach (DataRow r in periodos.Rows)
            {
                DataRow[] venta = total_periodos.Select("'" + r[0].ToString() + "'  = periodo");

                double suma = 0;

                if (venta.Length > 0)
                {
                    foreach (DataRow row in venta)
                    {
                        suma += double.Parse(row[1].ToString());
                    }

                }

                HTML += "<th colspan=1;  class='test sorter-false' style='border-right: 2px solid rgb(50, 48, 48);' > " + suma.ToString("N0") + "</th>";
                HTML_EXCEL += "<td> " + suma.ToString("N0") + "</td>";

            }
            HTML += "<th colspan=1;  class='test sorter-false' style='border-right: 2px solid rgb(50, 48, 48);' > </th>";
            HTML_EXCEL += "<td> </td>";

            HTML += "</tr>";
            HTML += "<tr>";

            HTML_EXCEL += "</tr>";
            HTML_EXCEL += "<tr style='background-color:" + color_fondo_excel + "; color:" + color_letra_excel + "'>";

            bool primer2 = true;
            con = 0;
            while (con < 3)
            {
                if (primer2)
                {

                    HTML += "<th  class='test' style='border-right: 2px solid rgb(50, 48, 48);'> </th>";
                    HTML_EXCEL += "<td> </td>";
                    primer2 = false;
                }


                HTML += "<th colspan=1;  class='test2' style='border-right: 2px solid rgb(50, 48, 48);' > " + dias[con] + "</th>";
                HTML_EXCEL += "<td> " + dias[con] + "</td>";

                con++;
            }

            foreach (DataRow r in periodos.Rows)
            {
                HTML += "<th colspan=1;  class='test' style='border-right: 2px solid rgb(50, 48, 48);' > " + r[0].ToString() + "</th>";
                HTML_EXCEL += "<td> " + r[0].ToString() + "</td>";
            }
            HTML += "<th colspan=1;  class='test' style='border-right: 2px solid rgb(50, 48, 48);' > Total </th>";
            HTML_EXCEL += "<td> Total </td>";
            HTML += "</tr>";
            HTML += "</thead>";
            HTML += "<tbody>";
            HTML += "<tr>";

            HTML_EXCEL += "</tr>";
            HTML_EXCEL += "<tr>";

            con = 0;

            foreach (DataRow s in productos.Rows)
            {
                string cod_produc1 = ReporteRNegocio.cod_producto(s[0].ToString().Trim()) + "*" + vendedor + "*"+ bodega;

                string cod_produc = ReporteRNegocio.cod_producto(s[0].ToString().Trim());
                HTML += "<td style='border-right: 2px solid rgb(50, 48, 48);' > " + cod_produc + " - " + s[0].ToString() + "</td>";
                HTML_EXCEL += "<td> " + cod_produc + " - " + s[0].ToString() + "</td>";
               
                con = 0;

                double sum_por_row = 0;
                while (con < 3)
                {

                    DataRow[] venta = total_dias.Select("'" + dias[con] + "'  = fechafactura and descproducto = '" + s[0].ToString() + "'");

                    double suma = 0;

                    if (venta.Length > 0)
                    {
                        foreach (DataRow row in venta)
                        {
                            suma += double.Parse(row[1].ToString());
                        }

                    }
                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                    //encriptador.EncryptData(



                    string script2 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;)", encriptador.EncryptData(dias[con]), encriptador.EncryptData(cod_produc1), encriptador.EncryptData(grupos.Trim().Replace("'", "")), encriptador.EncryptData("3"));

                    if (suma != 0)
                    {
                        HTML += "<td colspan=1;> <a data-toggle='tooltip' data-placement='top' title='" + s[0].ToString() + "' href='javascript:' onclick='" + script2 + "'>" + suma.ToString("N0") + " </a> </td>";
                        HTML_EXCEL += "<td>" + suma.ToString("N0") + "</td>";
                    }
                    else
                    {
                        HTML += "<td colspan=1;> " + suma.ToString("N0") + "</td>";
                        HTML_EXCEL += "<td> " + suma.ToString("N0") + "</td>";

                    }

                    con++;
                }

                foreach (DataRow r in periodos.Rows)
                {
                    DataRow[] venta = total_periodos.Select("'" + r[0].ToString() + "'  = periodo and descproducto = '" + s[0].ToString() + "'");
                    double suma = 0;

                    if (venta.Length > 0)
                    {
                        foreach (DataRow row in venta)
                        {
                            suma += double.Parse(row[1].ToString());
                        }
                    }
                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                    //encriptador.EncryptData(
                    sum_por_row += suma;
                    string script2 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;)", encriptador.EncryptData(r[0].ToString()), encriptador.EncryptData(cod_produc1), encriptador.EncryptData(grupos.Trim().Replace("'", "")), encriptador.EncryptData("3"));

                    if (suma != 0)
                    {
                        HTML += "<td colspan=1;> <a data-toggle='tooltip' data-placement='top' title='" + s[0].ToString() + "' href='javascript:' onclick='" + script2 + "'>" + suma.ToString("N0") + " </a> </td>";
                        HTML_EXCEL += "<td>" + suma.ToString("N0") + "</td>";
                    }
                    else
                    {
                        HTML += "<td colspan=1;> " + suma.ToString("N0") + "</td>";
                        HTML_EXCEL += "<td> " + suma.ToString("N0") + "</td>";
                    }
                   
                }
                HTML += "<td colspan=1;> " + sum_por_row.ToString("N0") + "</td>";
                HTML += "</tr>";


                HTML_EXCEL += "<td> " + sum_por_row.ToString("N0") + "</td>";

                HTML_EXCEL += "</tr>";
            }
            HTML += "</tbody>";
            HTML += "  </table>";
            //HTML += "</div>";
            HTML += "</div>";


            HTML_EXCEL += "</table>";
            R_Excel_1.InnerHtml = HTML_EXCEL;

            return HTML;

        }

        private static string crear_reporte_correo(DataTable dataTable, DataTable totales, string DESDE, string HASTA, string grupos)
        {

            //HTML += " </div>";

            string HTML = "";
            HTML += "<table id='TABLA_REPORTE' class='table table-advance table-bordered fill-head tablesorter' style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";
            //HTML += "     <thead>";

            HTML += " <tr>";


            foreach (DataRow r in dataTable.Rows)
            {
                DataRow[] venta = totales.Select("vendedor = '" + r[0].ToString().Trim() + "'");


                int sum_cont = 0;
                if (venta.Length > 0)
                {
                    DataTable ta = new DataTable();
                    DataColumn column;
                    column = new DataColumn();
                    column.ColumnName = "cont";
                    ta.Columns.Add(column);


                    foreach (DataRow row in venta)
                    {
                        DataRow row2 = ta.NewRow();
                        row2["cont"] = row[5].ToString().Trim();
                        ta.Rows.Add(row2);
                    }
                    DataView view = new DataView(ta);
                    DataTable clientes = view.ToTable(true, "cont");
                    sum_cont = clientes.Rows.Count;
                }


                HTML += "<td colspan=2;  class='test' ># cltes con fact.</td>";
                HTML += "<td colspan=1;  class='test' style='border-right: 2px solid rgb(50, 48, 48);' > " + sum_cont + "</td>";


            }
            HTML += "</tr>";

            HTML += " <tr>";

            foreach (DataRow r in dataTable.Rows)
            {

                HTML += "<td colspan=3; class='test' style='border-right: 2px solid rgb(50, 48, 48);' > " + r[0].ToString().Trim() + "</td>";
            }
            HTML += "</tr>";

            int cont2 = 1;
            foreach (DataRow r in dataTable.Rows)
            {
                DataRow[] venta = totales.Select("vendedor = '" + r[0].ToString().Trim() + "'");
                double SUM_VENTA = 0;
                int sum_cont = 0;
                foreach (DataRow row in venta)
                {
                    SUM_VENTA += double.Parse(row[1].ToString());
                    sum_cont += Convert.ToInt32(row[2].ToString());
                }

                HTML += "<td  class='test' style='font-weight: bold; border-bottom: 2px solid rgb(50, 48, 48);'> TOTAL </td>";
                HTML += "<td  class='test' style='font-weight: bold; border-bottom: 2px solid rgb(50, 48, 48);'> " + SUM_VENTA.ToString("N0") + "  </td>";
                HTML += "<td   class='test' style='font-weight: bold; border-right: 2px solid rgb(50, 48, 48); border-bottom: 2px solid rgb(50, 48, 48); '> " + sum_cont + "  </td>";
                cont2++;
            }
            HTML += "</tr>";


            while (DESDE != HASTA)
            {
                HTML += "<tr>";
                int cont3 = 1;
                foreach (DataRow r in dataTable.Rows)
                {

                    DataRow[] venta = totales.Select("'" + DESDE + "'  = fechafactura and vendedor = '" + r[0].ToString().Trim() + "'");

                    string v = "-";
                    string c = "0";

                    double venta_ = 0;
                    int cont = 0;
                    foreach (DataRow row in venta)
                    {
                        venta_ += double.Parse(row[1].ToString());
                        cont += Convert.ToInt32(row[2].ToString());
                    }
                    string script = "";
                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                    //encriptador.EncryptData(
                    if (venta_ != 0) { v = venta_.ToString("N0"); script = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;)", encriptador.EncryptData(DESDE), encriptador.EncryptData(r[0].ToString().Trim()), encriptador.EncryptData(grupos.Trim().Replace("'", "")), encriptador.EncryptData("2")); }
                    if (cont != 0) { c = cont.ToString(); }


                    HTML += "<td > " + DESDE + "  </td>";

                    if (script == "")
                    {
                        HTML += "<td > " + v + "</td>";
                    }
                    else
                    {
                        HTML += "<td > <a href='javascript:' onclick='" + script + "'>" + v + " </a> </td>";
                    }

                    HTML += "<td style='border-right: 2px solid rgb(50, 48, 48);' > " + c + "  </td>";
                    cont3++;
                }
                HTML += "</tr>";

                DateTime desde_ = Convert.ToDateTime(DESDE, new CultureInfo("es-ES"));
                DESDE = desde_.AddDays(1).ToShortDateString().Replace("-", "/");
            }

            //HTML += " </tbody>";
            HTML += "  </table>";
            //HTML += "</div>";
            HTML += "</div>";

            return HTML;

        }

        public class VENDEDOR
        {
            public string cod_vendedor { get; set; }
            public string nom_vendedor { get; set; }

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
        [WebMethod]
        public static List<VENDEDOR> BODEGA_VENDEDOR(string grupos)
        {
            DataTable dt = new DataTable();
            string grupos_ = agregra_comillas2(grupos);

            DateTime t = DateTime.Now;
            DateTime t2 = DateTime.Now;
            t = new DateTime(t.Year, t.Month - 1, 1);
            string DESDE = t.ToShortDateString();
            string HASTA = t2.ToShortDateString();

            DESDE = DESDE.Replace("-", "/");
            HASTA = HASTA.Replace("-", "/");

            string where = " where FechaFactura >= CONVERT(datetime,'" + DESDE + "', 103) " +
                             " and FechaFactura <= CONVERT(datetime,'" + HASTA + "',103) and user1 in (" + grupos2 + ")";

            if (grupos != "")
            {
                where += " and bodega in (" + grupos_ + ")";
            }

            try
            {
                dt = ReporteRNegocio.listar_ALL_vendedores(where);
                ////dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
                DataView dv = dt.DefaultView;
                dv.Sort = "nom_vend";
                dt = dv.ToTable();
            }
            catch { }

            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new VENDEDOR
                        {
                            cod_vendedor = Convert.ToString(item["cod_vend"]),
                            nom_vendedor = Convert.ToString(item["nom_vend"])
                        };
            return query.ToList<VENDEDOR>();
        }



        protected void b_Click(object sender, ImageClickEventArgs e)
        {

            string grupos_del_usuario = "";

            grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

            if (grupos_del_usuario == "")
            {
                grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
            }


            DataTable dt; DataView dtv = new DataView();


            string where1 = " where FechaFactura >= CONVERT(datetime,'" + txt_desde.Text + "', 103) " +
                        " and FechaFactura <= CONVERT(datetime,'" + txt_hasta.Text + "',103) and user1 in (" + grupos_del_usuario + ")";

            dt = ReporteRNegocio.carga_bodega(where1);
            if (dt.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>NO_GRUPOS();</script>", false);
            }
            else
            {

                cargar_combo_Grupo(dt, dtv);
                string es_vend = ReporteRNegocio.esvendedor(User.Identity.Name.ToString());
                if (es_vend != "2")
                {

                    string where = " where FechaFactura >= CONVERT(datetime,'" + txt_desde.Text + "', 103) " +
                         " and FechaFactura <= CONVERT(datetime,'" + txt_hasta.Text + "',103) ";
                    where += "and user1 in (" + grupos_del_usuario + ")";

                    cargar_combo_VENDEDOR(ReporteRNegocio.listar_ALL_vendedores(where), dtv);
                    //cargar_combo_clientes(ReporteRNegocio.listar_ALL_cliente2(where), dtv);

                }
                else
                {
                    string where = " where FechaFactura >= CONVERT(datetime,'" + txt_desde.Text + "', 103) " +
                        " and FechaFactura <= CONVERT(datetime,'" + txt_hasta.Text + "',103) ";
                    where += "and user1 in (" + grupos_del_usuario + ")";

                    cargar_combo_VENDEDOR(ReporteRNegocio.listar_ALL_vendedores(where + " and codvendedor = '" + USER + "' "), dtv);
                    //cargar_combo_clientes(ReporteRNegocio.listar_ALL_cliente2(where + " and codvendedor = '" + USER + "' "), dtv);
                }
            }
        }

        private void cargar_combo_VENDEDOR(DataTable dt, DataView dtv)
        {
            //dt.Rows.Add(new Object[] { -1, "-- Todos --" });
            dtv = dt.DefaultView;
            dtv.Sort = "cod_vend";
            d_vendedor_.DataSource = dtv;
            d_vendedor_.DataTextField = "nom_vend";
            d_vendedor_.DataValueField = "cod_vend";
            //d_vendedor_.SelectedIndex = -1;
            d_vendedor_.DataBind();
        }
        private void cargar_combo_Grupo(DataTable dt, DataView dtv)
        {
            //dt.Rows.Add(new Object[] { "-- Todos --" });
            dtv = dt.DefaultView;
            dtv.Sort = "nom_bodega";
            d_grupos_usuario.DataSource = dtv;
            d_grupos_usuario.DataTextField = "nom_bodega";
            d_grupos_usuario.DataValueField = "nom_bodega";
            d_grupos_usuario.DataBind();
        }

        protected void btn_excel_Click(object sender, EventArgs e)
        {
            Response.Clear();

            Response.AddHeader("content-disposition", "attachment;filename=SOPRODI_2_" + DateTime.Now.ToShortDateString() + ".xls");

            Response.Charset = "";

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.ContentType = "application/vnd.xls";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);

            R_Excel_1.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());

            Response.End();
        }
    }
}