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
    public partial class REPORTE_LV_B : System.Web.UI.Page
    {

        public static string grupos;
        private static string grupos2;

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
                    if (u_ne.Trim() == "5" || u_ne.Trim() == "11")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }
                DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now;
                //t = new DateTime(t.Year, t.Month - 6, 1);
                txt_desde.Text = ReporteRNegocio.CALCULA_DESDE(t.Month, t.Year);
                txt_hasta.Text = t2.ToShortDateString();

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

            }

            string vendedor = ReporteRNegocio.esvendedor(User.Identity.Name.ToString());
            if (vendedor == "2")
            {
                filtros.Visible = false;
                l_vendedores.Text = User.Identity.Name.ToString();
                grupos = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
                grupos2 = grupos;
                btn_productos_Click(sender, e);

            }
            else
            {
                grupos = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));
                grupos2 = grupos;
            }
            cargar_bodegas();
            cargar_vendedores();
        }

        private void cargar_bodegas()
        {
            string DESDE = txt_desde.Text;
            string HASTA = txt_hasta.Text;

            DESDE = DESDE.Replace("-", "/");
            HASTA = HASTA.Replace("-", "/");

            string where = " where FechaFactura >= CONVERT(datetime,'" + DESDE + "', 103) " +
                            " and FechaFactura <= CONVERT(datetime,'" + HASTA + "',103) and user1 in (" + grupos + ")";

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

            string DESDE = txt_desde.Text;
            string HASTA = txt_hasta.Text;

            DESDE = DESDE.Replace("-", "/");
            HASTA = HASTA.Replace("-", "/");

            string where = " where FechaFactura >= CONVERT(datetime,'" + DESDE + "', 103) " +
                            " and FechaFactura <= CONVERT(datetime,'" + HASTA + "',103) and user1 in (" + grupos + ")";

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

            string vendedores = agregra_comillas(l_vendedores.Text);
            //string clientes = agregra_comillas(l_clientes.Text);
            string desde = txt_desde.Text;
            string hasta = txt_hasta.Text;
            string grupos = agregra_comillas(l_grupos.Text); 

            string where = "";

            string grupos_del_usuario = "";

            grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

            if (grupos_del_usuario == "")
            {
                grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
            }

            string wher = " where user1 in (" + grupos_del_usuario + ") " +
                 " and FechaFactura >= CONVERT(datetime,'" + txt_desde.Text + "', 103) and FechaFactura <= CONVERT(datetime,'" + txt_hasta.Text + "',103)";

            DataTable dt2; DataView dtv = new DataView();
            dt2 = ReporteRNegocio.carga_bodega(wher);
            if (dt2.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>NO_GRUPOS();</script>", false);
            }
            else
            {
                string es_vendedor = ReporteRNegocio.esvendedor(User.Identity.Name.ToString());
                where = " where user1 in (" + grupos_del_usuario + ") " +
                   " and FechaFactura >= CONVERT(datetime,'" + txt_desde.Text + "', 103) and FechaFactura <= CONVERT(datetime,'" + txt_hasta.Text + "',103)";

                if (l_grupos.Text != "") { where += " and bodega in (" + agregra_comillas(l_grupos.Text) + ")"; }
                if (l_vendedores.Text != "") { where += " and codvendedor in (" + agregra_comillas(l_vendedores.Text) + ")"; }

                DataTable table_totales = ReporteRNegocio.table_totales_c(where);
                DataView view = new DataView(table_totales);
                DataTable vendedores2 = view.ToTable(true, "vendedor");
                DataView dv3 = vendedores2.DefaultView;
                dv3.Sort = "vendedor";
                vendedores2 = dv3.ToTable();


                //PERIODOS
                string html_header = crear_reporte_correo(vendedores2, table_totales, desde, hasta, l_grupos.Text);
                string html_body = html_header;

                Div6.InnerHtml = html_body;



                DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now;
                t = new DateTime(t.Year, t.Month, 1);
                string DESDE = t.ToShortDateString();
                string HASTA = t2.ToShortDateString();

                DESDE = DESDE.Replace("-", "/");
                HASTA = HASTA.Replace("-", "/");

                string where2 = " where FechaFactura >= CONVERT(datetime,'" + DESDE + "', 103) " +
                                " and FechaFactura <= CONVERT(datetime,'" + HASTA + "',103) and user1 in (" + grupos_del_usuario + ")";


                if (l_grupos.Text != "") { where2 += " and bodega in (" + agregra_comillas(l_grupos.Text) + ")"; }
                if (l_vendedores.Text != "") { where2 += " and codvendedor in (" + agregra_comillas(l_vendedores.Text) + ")"; }

                DataTable table_totales2 = ReporteRNegocio.table_totales(where2);
                DataView view2 = new DataView(table_totales2);
                DataTable vendedores3 = view2.ToTable(true, "vendedor");
                DataView dv32 = vendedores3.DefaultView;
                dv32.Sort = "vendedor";
                vendedores3 = dv32.ToTable();

                DateTime desde_2 = Convert.ToDateTime(DESDE, new CultureInfo("es-ES"));
                DESDE = desde_2.AddDays(-1).ToShortDateString().Replace("-", "/");

                //MENSUAL
                string html_DIAS = crear_reporte_correo2(vendedores3, table_totales2, DESDE, HASTA, l_grupos.Text);

                tabla.Visible = true;
                div4.Visible = true;
                div3.Visible = true;

                DivMainContent.InnerHtml = html_DIAS;
                filtro_memoria_div.InnerHtml = "<div class='btn-toolbar pull-left'><input type='text' id='t_filtro_memoria' style='width: 200px; margin-right: 7px; padding: 5px;' placeholder='Filtrar...' class='form-control' /></div>";
                filtro_memoria_div2.InnerHtml = "<div class='btn-toolbar pull-left'><input type='text' id='t_filtro_memoria2' style='width: 200px; margin-right: 7px; padding: 5px;' placeholder='Filtrar...' class='form-control' /></div>";

            }

            //volver a cargar

            DataTable dt = new DataTable();

            string wher2 = " where FechaFactura >= CONVERT(datetime,'" + txt_desde.Text + "', 103) " +
                          " and FechaFactura <= CONVERT(datetime,'" + txt_hasta.Text + "',103)  and user1 in (" + grupos_del_usuario + ")";


            if (l_grupos.Text != "") { wher2 += " and bodega in (" + agregra_comillas(l_grupos.Text) + ")"; }


            try
            {
                dt = ReporteRNegocio.listar_ALL_vendedores(wher2);
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


            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasdaqsdsaeee", "<script> SortPrah(); </script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasdas2dsaeee", "<script> superfiltro(); </script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "1dsa1233das", "<script> superfiltro2(); </script>", false);
        }


        public string crear_reporte_correo(DataTable dataTable, DataTable totales, string DESDE, string HASTA, string grupos)
        {



            string periodo_anterior = "";
            string periodo_actual = "";
            DataView view = new DataView(totales);
            DataTable distinctValues = view.ToTable(true, "periodo");
            DataView dv = distinctValues.DefaultView;
            dv.Sort = "periodo desc";
            distinctValues = dv.ToTable();
            int co = 1;
            foreach (DataRow r in distinctValues.Rows)
            {
                if (co == 1)
                {
                    periodo_actual = r[0].ToString();
                }
                if (co == 2)
                {
                    periodo_anterior = r[0].ToString();
                }
                co++;
            }
            if (periodo_anterior == "")
            {

                periodo_anterior = periodo_actual;
            }

            double ACUMULADO = 0;
            int cant_cliete = 0;
            string var1 = "";

            double total_mes_actual1 = 0;

            foreach (DataRow r2 in dataTable.Rows)
            {

                DateTime hasta_ant = Convert.ToDateTime(HASTA, new CultureInfo("es-ES"));
                string anterior_hasta = hasta_ant.AddMonths(-1).ToShortDateString().Replace("-", "/");
                string where_ant = " where vendedor = '" + r2[0].ToString() + "' and FechaFactura <= CONVERT(datetime,'" + anterior_hasta + "',103) ";

                ACUMULADO += ReporteRNegocio.Facturación_Mes(periodo_anterior, where_ant);

                total_mes_actual1 += ReporteRNegocio.Facturación_Mes(periodo_actual, " where vendedor = '" + r2[0].ToString() + "' and  periodo = " + periodo_actual);
                cant_cliete += ReporteRNegocio._cltes_con_vta(periodo_anterior, where_ant);
            }
            var1 = (Math.Round((total_mes_actual1 * 100 / ACUMULADO)) - 100).ToString() + "%";
            if (var1.Contains("-"))
            {
                var1 = "<i style='color:red;'>" + var1 + "</i>";
            }
            //HTML_EXCEL
            string HTML_EXCEL = "";
            string color_letra_excel = "white";
            string color_fondo_excel = "#428BCA";

            string HTML = "";
            HTML += "<table id='TABLA_REPORTE2' class='table table-advance table-bordered fill-head tablesorter filtrar2'  style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";
            HTML += "<thead>";
            HTML += " <tr>";

            HTML_EXCEL += "<table id='T_EXCEL2' border=1>";
            HTML_EXCEL += "<tr style='background-color:" + color_fondo_excel + "; color:" + color_letra_excel + "'>";

            bool primer2 = true;
            foreach (DataRow r in distinctValues.Rows)
            {
                if (primer2)
                {
                    HTML += "<th colspan=4;  class='test sorter-false' style='border-right: 2px solid rgb(50, 48, 48);'></th>";
                    HTML_EXCEL += "<td colspan=4 ></td>";
                    primer2 = false;
                }

                HTML += "<th colspan=2; class='test sorter-false' style='border-right: 2px solid rgb(50, 48, 48);' > " + r[0].ToString().Trim() + "</th>";
                HTML_EXCEL += "<td colspan=2 > " + r[0].ToString().Trim() + "</td>";
            }
            HTML += "</tr>";
            HTML_EXCEL += "</tr>";

            HTML += "<tr>";
            HTML_EXCEL += "<tr style='background-color:" + color_fondo_excel + "; color:" + color_letra_excel + "'>";

            int cont2 = 1;
            bool primer1 = true;
            foreach (DataRow r in distinctValues.Rows)
            {
                if (primer1)
                {
                    HTML += "<th colspan=1;  class='test' style='font-weight: bold; border-bottom: 2px solid rgb(50, 48, 48);'>Vendedores</th>";
                    HTML += "<th colspan=1;  class='test' style='font-weight: bold; border-bottom: 2px solid rgb(50, 48, 48);'>Acum.Mes Anter. (" + ACUMULADO.ToString("N0") + ")</th>";
                    HTML += "<th colspan=1;  class='test' style='font-weight: bold; border-bottom: 2px solid rgb(50, 48, 48);'>clte# (" + cant_cliete + ")</th>";
                    HTML += "<th colspan=1; id='var' class='test sort-ascending' style='font-weight: bold; border-bottom: 2px solid rgb(50, 48, 48); border-right: 2px solid rgb(50, 48, 48);'  width='95px'>%Var (" + var1 + ")&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>";


                    HTML_EXCEL += "<td>Vendedores</td>";
                    HTML_EXCEL += "<td>Acum.Mes Anter. (" + ACUMULADO.ToString("N0") + ")</td>";
                    HTML_EXCEL += "<td>clte# (" + cant_cliete + ")</td>";
                    HTML_EXCEL += "<td width='95px'>%Var (" + var1 + ")&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";

                    primer1 = false;
                }
                DataRow[] venta = totales.Select("periodo = '" + r[0].ToString().Trim() + "'");
                DataTable ta = new DataTable();
                DataColumn column;
                column = new DataColumn();
                column.ColumnName = "cont";
                ta.Columns.Add(column);
                double SUM_VENTA = 0;
                int sum_cont = 0;
                foreach (DataRow row in venta)
                {
                    SUM_VENTA += double.Parse(row[1].ToString());

                    DataRow row2 = ta.NewRow();
                    row2["cont"] = row[4].ToString().Trim();
                    ta.Rows.Add(row2);
                }
                DataView view2 = new DataView(ta);
                DataTable clientes = view2.ToTable(true, "cont");
                sum_cont = clientes.Rows.Count;
                //HTML += "<td colspan=2; class='test' style='font-weight: bold; border-bottom: 2px solid rgb(50, 48, 48);'> TOTAL </td>";

                HTML += "<th colspan=1; class='test' style='font-weight: bold; border-bottom: 2px solid rgb(50, 48, 48);'> " + SUM_VENTA.ToString("N0") + " </th>";
                HTML += "<th colspan=1; class='test' style='font-weight: bold; border-bottom: 2px solid rgb(50, 48, 48); border-right: 2px solid rgb(50, 48, 48);'> " + sum_cont + "  </th>";

                HTML_EXCEL += "<td> " + SUM_VENTA.ToString("N0") + " </td>";
                HTML_EXCEL += "<td> " + sum_cont + "  </td>";
                cont2++;
            }
            HTML += "</tr>";
            HTML += "</thead>";

            HTML_EXCEL += "</tr>";

            foreach (DataRow r2 in dataTable.Rows)
            {

                HTML += "<tr>";
                HTML_EXCEL += "<tr>";
                int cont3 = 1;

                HTML += "<td colspan=1; style='border-right: 2px solid rgb(50, 48, 48);'> " + r2[0].ToString() + "  </td>";
                HTML_EXCEL += "<td> " + r2[0].ToString() + "  </td>";

                DateTime hasta_ant = Convert.ToDateTime(HASTA, new CultureInfo("es-ES"));
                string anterior_hasta = hasta_ant.AddMonths(-1).ToShortDateString().Replace("-", "/");
                string where_ant = " where vendedor = '" + r2[0].ToString() + "' and FechaFactura <= CONVERT(datetime,'" + anterior_hasta + "',103) ";
                string factur_acumulado = ReporteRNegocio.Facturación_Mes(periodo_anterior, where_ant).ToString("N0");
                double total_mes_actual = ReporteRNegocio.Facturación_Mes(periodo_actual, " where vendedor = '" + r2[0].ToString() + "' and  periodo = " + periodo_actual);
                string clntes = ReporteRNegocio._cltes_con_vta(periodo_anterior, where_ant).ToString();

                HTML += "<td colspan=1;> " + factur_acumulado + "  </td>";
                HTML += "<td colspan=1;'> " + clntes + "  </td>";

                HTML_EXCEL += "<td> " + factur_acumulado + "  </td>";
                HTML_EXCEL += "<td> " + clntes + "  </td>";

                string var_ = (Math.Round((total_mes_actual * 100 / double.Parse(factur_acumulado.Replace(".", "")))) - 100).ToString() + "%";
                if (var_.Contains("NaN") || var_.Contains("∞") || var_.Contains("NeuN") || var_.Contains("Infinito")) { var_ = ""; }
                if (var_.Contains("-"))
                {
                    var_ = "<i style='color:red;'> " + var_ + "</i>";
                }

                string var_tool = "(" + total_mes_actual.ToString("N0") + " * 100 / " + factur_acumulado + ") -100";
                HTML += "<td colspan=1; style='border-right: 2px solid rgb(50, 48, 48);'  width='95px'> <a data-toggle='tooltip' style='color:black;' data-placement='top' title='" + var_tool + "'>" + var_ + " </a> </td>";
                HTML_EXCEL += "<td width='70px'>" + var_ + "  </td>";
                string cod_vendedor = ReporteRNegocio.cod_vendedor(r2[0].ToString().Trim());
                foreach (DataRow r in distinctValues.Rows)
                {

                    DataRow[] venta = totales.Select(r[0].ToString() + " = periodo and vendedor = '" + r2[0].ToString().Trim() + "'");

                    string v = "-";
                    string c = "0";

                    DataTable ta = new DataTable();
                    DataColumn column;
                    column = new DataColumn();
                    column.ColumnName = "cont";
                    int sum_cont = 0;
                    ta.Columns.Add(column);
                    double venta_ = 0;
                    int cont = 0;
                    foreach (DataRow row in venta)
                    {
                        venta_ += double.Parse(row[1].ToString());
                        DataRow row2 = ta.NewRow();
                        row2["cont"] = row[4].ToString().Trim();
                        ta.Rows.Add(row2);
                    }
                    DataView view2 = new DataView(ta);
                    DataTable clientes = view2.ToTable(true, "cont");
                    sum_cont = clientes.Rows.Count;

                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                    //encriptador.EncryptData(

                    string script2 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;)", encriptador.EncryptData(r[0].ToString()), encriptador.EncryptData(cod_vendedor), encriptador.EncryptData(grupos.Trim().Replace("'", "")), encriptador.EncryptData("1"));

                    if (venta_ != 0)
                    {
                        HTML += "<td colspan=1;> <a data-toggle='tooltip' data-placement='top' title='" + r2[0].ToString().Trim() + "' href='javascript:' onclick='" + script2 + "'>" + venta_.ToString("N0") + " </a> </td>";
                        HTML_EXCEL += "<td>" + venta_.ToString("N0") + "</td>";
                    }
                    else
                    {
                        HTML += "<td colspan=1;> " + venta_ + "</td>";
                        HTML_EXCEL += "<td> " + venta_ + "</td>";

                    }
                    HTML += "<td colspan=1; style='border-right: 2px solid rgb(50, 48, 48);'> " + sum_cont + "</td>";
                    HTML_EXCEL += "<td> " + sum_cont + "</td>";
                    cont3++;
                }
                HTML += "</tr>";
                HTML_EXCEL += "</tr>";

            }


            HTML += "  </table>";
            HTML_EXCEL += "  </table>";
            HTML += "</div>";

            R_Excel_2.InnerHtml = HTML_EXCEL;
            return HTML;

        }


        public string crear_reporte_correo2(DataTable dataTable, DataTable totales, string DESDE, string HASTA, string grupos)
        {


            double total_suma = 0;
            double cant_fact = 0;
            DataTable ta1 = new DataTable();
            DataColumn column1; DataColumn column2;
            column1 = new DataColumn();
            column2 = new DataColumn();
            column1.ColumnName = "cont";
            column2.ColumnName = "vendedor";
            ta1.Columns.Add(column1);
            ta1.Columns.Add(column2);
            int sum_cont1 = 0;
            foreach (DataRow row in totales.Rows)
            {

                DataRow row2 = ta1.NewRow();
                row2["cont"] = row[5].ToString().Trim();
                row2["vendedor"] = row[3].ToString().Trim();
                ta1.Rows.Add(row2);
                total_suma += double.Parse(row[1].ToString());
                cant_fact += double.Parse(row[2].ToString());
            }
            DataView view1 = new DataView(ta1);
            DataTable clientes1 = view1.ToTable(true, "cont", "vendedor");
            sum_cont1 = clientes1.Rows.Count;


            string DESDE_AUX = HASTA;

            //HTML_EXCEL
            string HTML_EXCEL = "";
            string color_letra_excel = "white";
            string color_fondo_excel = "#428BCA";

            string HTML = "";

            HTML += "<table id='TABLA_REPORTE' class='table table-advance table-bordered fill-head tablesorter filtrar' style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";
            HTML += "<thead>";
            HTML += "<tr>";

            HTML_EXCEL += "<table id='T_EXCEL1' border=1 >";
            HTML_EXCEL += "<tr style='background-color:" + color_fondo_excel + "; color:" + color_letra_excel + "'>";


            bool primer = true;

            while (HASTA != DESDE)
            {
                if (primer)
                {
                    HTML += "<th colspan=4; class='test sorter-false' style='border-right: 2px solid rgb(50, 48, 48);' ></th>";
                    HTML_EXCEL += "<td colspan=4></th>";
                    primer = false;
                }


                DataRow[] venta = totales.Select("'" + HASTA + "'  = fechafactura");

                double suma = 0;
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
                        suma += double.Parse(row[1].ToString());
                    }
                    DataView view = new DataView(ta);
                    DataTable clientes = view.ToTable(true, "cont");
                    sum_cont = clientes.Rows.Count;
                }


                HTML += "<th colspan=1;  class='test sorter-false' >#cltes</th>";
                HTML += "<th colspan=1;  class='test sorter-false' style='border-right: 2px solid rgb(50, 48, 48);' > " + sum_cont + "</th>";

                HTML_EXCEL += "<td>#cltes</td>";
                HTML_EXCEL += "<td> " + sum_cont + "</td>";

                DateTime desde_ = Convert.ToDateTime(HASTA, new CultureInfo("es-ES"));
                HASTA = desde_.AddDays(-1).ToShortDateString().Replace("-", "/");

            }
            HTML += "</tr>";
            HTML += "<tr>";

            HTML_EXCEL += "</tr>";
            HTML_EXCEL += "<tr style='background-color:" + color_fondo_excel + "; color:" + color_letra_excel + "'>";

            HASTA = DESDE_AUX;
            bool primer2 = true;
            while (DESDE != HASTA)
            {
                if (primer2)
                {
                    HTML += "<th colspan=4;  class='test sorter-false' style='border-right: 2px solid rgb(50, 48, 48);'>Total vendedor</th>";
                    HTML_EXCEL += "<td colspan=4>Total vendedor</td>";
                    primer2 = false;
                }


                HTML += "<th colspan=2;  class='test sorter-false' style='border-right: 2px solid rgb(50, 48, 48);' > " + HASTA + "</th>";
                HTML_EXCEL += "<td colspan=2> " + HASTA + "</td>";

                DateTime desde_ = Convert.ToDateTime(HASTA, new CultureInfo("es-ES"));
                HASTA = desde_.AddDays(-1).ToShortDateString().Replace("-", "/");

            }

            HTML += "</tr>";
            HTML += "<tr'>";

            HTML_EXCEL += "</tr>";
            HTML_EXCEL += "<tr style='background-color:" + color_fondo_excel + "; color:" + color_letra_excel + "'>";

            HASTA = DESDE_AUX;
            bool primer3 = true;
            while (DESDE != HASTA)
            {

                DataRow[] venta = totales.Select("'" + HASTA + "'  = fechafactura");

                double suma = 0;
                double sum_cont = 0;
                if (venta.Length > 0)
                {
                    foreach (DataRow row in venta)
                    {
                        sum_cont += double.Parse(row[2].ToString());
                        suma += double.Parse(row[1].ToString());
                    }
                }

                if (primer3)
                {
                    HTML += "<th colspan=1;  class='test' >Vendedor</th>";
                    HTML += "<th colspan=1;  class='test' >Total (" + total_suma.ToString("N0") + ")</th>";
                    HTML += "<th colspan=1;  class='test' >#clte (" + sum_cont1 + ")</th>";
                    HTML += "<th colspan=1;  class='test' style='border-right: 2px solid rgb(50, 48, 48);'>#fact  (" + cant_fact + ")</th>";

                    HTML_EXCEL += "<td>Vendedor</td>";
                    HTML_EXCEL += "<td>Total (" + total_suma.ToString("N0") + ")</td>";
                    HTML_EXCEL += "<td>#clte (" + sum_cont1 + ")</td>";
                    HTML_EXCEL += "<td>#fact  (" + cant_fact + ")</td>";
                    primer3 = false;

                }


                HTML += "<th colspan=1;  class='test'> " + suma.ToString("N0") + "</th>";
                HTML += "<th colspan=1;  class='test' style='border-right: 2px solid rgb(50, 48, 48);' > " + sum_cont + "</th>";

                HTML_EXCEL += "<td> " + suma.ToString("N0") + "</td>";
                HTML_EXCEL += "<td> " + sum_cont + "</td>";

                DateTime desde_ = Convert.ToDateTime(HASTA, new CultureInfo("es-ES"));
                HASTA = desde_.AddDays(-1).ToShortDateString().Replace("-", "/");

            }
            HTML += "</tr>";
            HTML_EXCEL += "</tr>";

            HASTA = DESDE_AUX;
            HTML += "</thead>";
            HTML += "<tbody>";


            foreach (DataRow r in dataTable.Rows)
            {

                DataRow[] total = totales.Select(" vendedor like '%" + r[0].ToString().Trim() + "%'");
                double suma = 0;
                int sum_cont = 0;

                double venta_1 = 0;
                int cont1 = 0;

                if (total.Length > 0)
                {
                    DataTable ta = new DataTable();
                    DataColumn column;
                    column = new DataColumn();
                    column.ColumnName = "cont";
                    ta.Columns.Add(column);

                    foreach (DataRow row in total)
                    {
                        venta_1 += double.Parse(row[1].ToString());
                        cont1 += Convert.ToInt32(row[2].ToString());
                        DataRow row2 = ta.NewRow();
                        row2["cont"] = row[5].ToString().Trim();
                        ta.Rows.Add(row2);
                        suma += double.Parse(row[1].ToString());
                    }
                    DataView view = new DataView(ta);
                    DataTable clientes = view.ToTable(true, "cont");
                    sum_cont = clientes.Rows.Count;
                }


                string cod_vendedor = ReporteRNegocio.cod_vendedor(r[0].ToString().Trim());

                HTML += "<tr>";
                HTML += "<td colspan=1;  class='' >" + r[0].ToString().Trim() + "</td>";

                HTML_EXCEL += "<tr>";
                HTML_EXCEL += "<td>" + r[0].ToString().Trim() + "</td>";
                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                //encriptador.EncryptData(

                string script2 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;)", encriptador.EncryptData(""), encriptador.EncryptData(cod_vendedor), encriptador.EncryptData(grupos.Trim().Replace("'", "")), encriptador.EncryptData("1"));
                HTML += "<td colspan=1;> <a data-toggle='tooltip' data-placement='bottom' title='" + r[0].ToString().Trim() + "' href='javascript:' onclick='" + script2 + "'>" + venta_1.ToString("N0") + " </a> </td>";
                HTML += "<td colspan=1;  class='' >" + sum_cont + " </td>";
                HTML += "<td colspan=1;  class='' style='border-right: 2px solid rgb(50, 48, 48);' >" + cont1 + "</td>";

                HTML_EXCEL += "<td>" + venta_1.ToString("N0") + "</td>";
                HTML_EXCEL += "<td>" + sum_cont + " </td>";
                HTML_EXCEL += "<td>" + cont1 + "</td>";



                HASTA = DESDE_AUX;

                while (DESDE != HASTA)
                {

                    DataRow[] venta = totales.Select("'" + HASTA + "'  = fechafactura and vendedor = '" + r[0].ToString().Trim() + "'");

                    string v = "0";
                    string c = "0";
                    string ven = "0";

                    double venta_ = 0;
                    int cont = 0;
                    foreach (DataRow row in venta)
                    {
                        venta_ += double.Parse(row[1].ToString());
                        cont += Convert.ToInt32(row[2].ToString());
                        ven = r[0].ToString().Trim();
                    }
                    string script = "";

                    if (venta_ != 0) { v = venta_.ToString("N0"); script = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;)", encriptador.EncryptData(HASTA), encriptador.EncryptData(cod_vendedor), encriptador.EncryptData(grupos.Trim().Replace("'", "")), encriptador.EncryptData("1")); }
                    if (cont != 0) { c = cont.ToString(); }

                    if (script == "")
                    {
                        HTML += "<td colspan=1;> " + v + "</td>";
                        HTML_EXCEL += "<td> " + v + "</td>";
                    }
                    else
                    {
                        HTML += "<td colspan=1;> <a data-toggle='tooltip' data-placement='bottom' title='" + r[0].ToString().Trim() + "' href='javascript:' onclick='" + script + "'>" + v + " </a> </td>";
                        HTML_EXCEL += "<td>" + v + "</td>";
                    }

                    HTML += "<td colspan=1; style='border-right: 2px solid rgb(50, 48, 48);' > " + c + "  </td>";
                    HTML_EXCEL += "<td> " + c + "  </td>";

                    DateTime desde_ = Convert.ToDateTime(HASTA, new CultureInfo("es-ES"));
                    HASTA = desde_.AddDays(-1).ToShortDateString().Replace("-", "/");


                }

                HTML += "</tr>";
                HTML_EXCEL += "</tr>";

            }

            HTML += "</tbody>";
            //HTML += " </tbody>";
            HTML += "  </table>";
            //HTML += "</div>";
            HTML += "</div>";

            HTML_EXCEL += "  </table>";
            R_Excel_1.InnerHtml = HTML_EXCEL;
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
            string wher = " where user1 in (" + grupos_del_usuario + ") " +
                   " and FechaFactura >= CONVERT(datetime,'" + txt_desde.Text + "', 103) and FechaFactura <= CONVERT(datetime,'" + txt_hasta.Text + "',103)";
            dt = ReporteRNegocio.carga_bodega(wher);
            if (dt.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>NO_GRUPOS();</script>", false);
            }
            else
            {

                cargar_combo_bodega(dt, dtv);
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

                    cargar_combo_VENDEDOR(ReporteRNegocio.listar_ALL_vendedores(where + " and codvendedor = '" + User.Identity.Name.ToString() + "' "), dtv);
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
        private void cargar_combo_bodega(DataTable dt, DataView dtv)
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

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Clear();

            Response.AddHeader("content-disposition", "attachment;filename=SOPRODI_2_" + DateTime.Now.ToShortDateString() + ".xls");

            Response.Charset = "";

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.ContentType = "application/vnd.xls";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);

            R_Excel_2.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());

            Response.End();
        }

    }
}