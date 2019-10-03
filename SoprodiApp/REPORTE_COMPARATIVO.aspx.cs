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

namespace SoprodiApp
{
    public partial class REPORTE_COMPARATIVO : System.Web.UI.Page
    {
        public static int cont_periodos;
        public static DataTable aux;
        public static DataTable productos;
        public static DataTable totales;
        public static bool izq = true;
        private static string USER;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.RegisterRedirectOnSessionEndScript();
            if (!IsPostBack)
            {
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "4" || u_ne.Trim() == "10")
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
                DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now;
                if (t.Month != 1)
                {
                    t = new DateTime(t.Year, t.Month - 1, 1);
                }
                else
                {
                    t = new DateTime(t.Year -1, 12, 1);
                
                }
                txt_desde.Text = t.ToShortDateString();
                txt_hasta.Text = t2.ToShortDateString();
                string es_vendedor = ReporteRNegocio.esvendedor(USER);
                if (es_vendedor == "2")
                {
                    btn_excel.Visible = false;
                    //btn_excel2.Visible = false;
                }
                else
                {
                    btn_excel.Visible = true;
                    //btn_excel2.Visible = true;
                }
            }
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
            dt = ReporteRNegocio.carga_grupos(txt_desde.Text, txt_hasta.Text, grupos_del_usuario);
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
                    cargar_combo_clientes(ReporteRNegocio.listar_ALL_cliente2(where), dtv);

                }
                else
                {
                    string where = " where FechaFactura >= CONVERT(datetime,'" + txt_desde.Text + "', 103) " +
                        " and FechaFactura <= CONVERT(datetime,'" + txt_hasta.Text + "',103) ";
                    where += "and user1 in (" + grupos_del_usuario + ")";

                    cargar_combo_VENDEDOR(ReporteRNegocio.listar_ALL_vendedores(where + " and codvendedor = '" + USER + "' "), dtv);
                    cargar_combo_clientes(ReporteRNegocio.listar_ALL_cliente2(where + " and codvendedor = '" + USER + "' "), dtv);
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

        private void cargar_combo_clientes(DataTable dt, DataView dtv)
        {
            //dt.Rows.Add(new Object[] { -1, "-- Todos --" });
            dtv = dt.DefaultView;
            dtv.Sort = "nom_cliente";
            d_cliente.DataSource = dtv;
            d_cliente.DataTextField = "nom_cliente";
            d_cliente.DataValueField = "rut_cliente";
            //d_vendedor_.SelectedIndex = -1;
            d_cliente.DataBind();
        }


        private void cargar_combo_Grupo(DataTable dt, DataView dtv)
        {

            //dt.Rows.Add(new Object[] { "-- Todos --" });
            dtv = dt.DefaultView;
            dtv.Sort = "user1";
            d_grupos_usuario.DataSource = dtv;
            d_grupos_usuario.DataTextField = "user1";
            d_grupos_usuario.DataValueField = "user1";
            d_grupos_usuario.DataBind();
        }

        public class CLIENTE
        {
            public string rut_cliente { get; set; }
            public string nom_cliente { get; set; }
        }


        public class VENDEDOR
        {
            public string cod_vendedor { get; set; }
            public string nom_vendedor { get; set; }
        }

        [WebMethod]
        public static List<CLIENTE> CLIENTE_POR_VENDEDOR(string vendedor, string desde, string hasta, string usuario, string grupos)
        {
            DataTable dt = new DataTable();
            string vende = agregra_comillas2(vendedor);
            string grupos_ = agregra_comillas2(grupos);

            string es_vendedor = ReporteRNegocio.esvendedor(usuario);


            string where = " where FechaFactura >= CONVERT(datetime,'" + desde + "', 103) " +
                             " and FechaFactura <= CONVERT(datetime,'" + hasta + "',103) ";



            if (es_vendedor == "2")
            {
                where += " and codvendedor in ('" + usuario + "')";
            }

            if (!vende.Contains("'-1'") && vende != "" && !vende.Contains("'-- Todos --'"))
            {
                where += " and codvendedor in (" + vende + ")";
            }

            if (!grupos_.Contains("'-1'") && grupos_ != "" && !grupos_.Contains("'-- Todos --'"))
            {
                where += " and user1 in (" + grupos_ + ")";
            }
            if (grupos_.Contains("'-- Todos --'") && es_vendedor == "2")
            {
                where += " and user1 in (" + agregra_comillas2(ReporteRNegocio.grupos_usuario_v_report(USER)) + ")";
            }
            else if (grupos_.Contains("'-- Todos --'"))
            {
                where += " and user1 in (" + agregra_comillas2(ReporteRNegocio.grupos_usuario(USER)) + ")";
            }

            if (vendedor == "")
            {
                where += " and 1=1 ";
            }
            //else
            //{
            try
            {
                dt = ReporteRNegocio.listar_ALL_cliente2(where);
                ////dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
                DataView dv = dt.DefaultView;
                dv.Sort = "nom_cliente";
                dt = dv.ToTable();
            }
            catch { }
            //}
            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new CLIENTE
                        {
                            rut_cliente = Convert.ToString(item["rut_cliente"]),
                            nom_cliente = Convert.ToString(item["nom_cliente"])
                        };
            return query.ToList<CLIENTE>();
        }


        [WebMethod]
        public static List<VENDEDOR> VENDEDOR_POR_GRUPO(string grupos, string desde, string hasta, string usuario)
        {
            DataTable dt = new DataTable();
            string grupos_ = agregra_comillas2(grupos);


            string es_vendedor = ReporteRNegocio.esvendedor(usuario);


            string where = " where FechaFactura >= CONVERT(datetime,'" + desde + "', 103) " +
                             " and FechaFactura <= CONVERT(datetime,'" + hasta + "',103) ";


            if (es_vendedor == "2")
            {
                where += " and codvendedor in ('" + usuario + "')";
            }

            if (!grupos_.Contains("'-1'") && grupos_ != "" && !grupos_.Contains("'-- Todos --'"))
            {
                where += " and user1 in (" + grupos_ + ")";
            }
            if (grupos_.Contains("'-- Todos --'") && es_vendedor == "2")
            {
                where += " and user1 in (" + agregra_comillas2(ReporteRNegocio.grupos_usuario_v_report(USER)) + ")";
            }
            else if (grupos_.Contains("'-- Todos --'"))
            {
                where += " and user1 in (" + agregra_comillas2(ReporteRNegocio.grupos_usuario(USER)) + ")";
            }
            if (grupos == "")
            {
                where += " and 1=1 ";
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

            //if (l_vendedores.Text != "" && l_clientes.Text != "" && txt_desde.Text != "" && txt_hasta.Text != "" && l_grupos.Text != "")
            //{
            string vendedores = agregra_comillas(l_vendedores.Text);
            string clientes = agregra_comillas(l_clientes.Text);
            string desde = txt_desde.Text;
            string hasta = txt_hasta.Text;
            string grupos = agregra_comillas(l_grupos.Text); ;

            string grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

            if (grupos_del_usuario == "")
            {
                grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
            }


            DataTable dt2; DataView dtv = new DataView();
            dt2 = ReporteRNegocio.carga_grupos(txt_desde.Text, txt_hasta.Text, grupos_del_usuario);
            if (dt2.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>NO_GRUPOS();</script>", false);
            }
            else
            {
                string es_vendedor = ReporteRNegocio.esvendedor(USER);
                string where = " where 1 = 1 ";
                if (grupos != "")
                {
                    where = where + " and user1 in (" + grupos + ") ";
                }
                else if (es_vendedor != "2")
                {
                    where = where + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario(USER)) + ")";
                }
                else if (es_vendedor == "2")
                {
                    where = where + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(USER)) + ")";
                }

                if (clientes != "")
                {
                    where = where + " and rutcliente in (" + clientes + ") ";
                }
                if (vendedores != "")
                {
                    where = where + " and codvendedor in (" + vendedores + ") ";
                }
                if (es_vendedor == "2")
                {
                    where += " and codvendedor in ('" + USER + "')";
                }

                div_report.Visible = true;
                cont_periodos = 0;
                G_INFORME_TOTAL_VENDEDOR.Visible = true;
                string periodos = ReporteRNegocio.listar_periodos_(where);
                //aux = ReporteRNegocio.listar_resumen_periodo(where + " AND PERIODO IN (" + periodos + ")");
                totales = new DataTable();

                List<string> periodos_list = periodos.Split(',').ToList();
                periodos_list.Reverse();
                totales.Columns.Add("FACTORES/Periodos");
                int colum = periodos_list.Count;
                colum = colum * 2;

                // AWWWWWWWWW YEAAAAAAAAAAAHHHHHHHHHHH
                string truco = " ";
                foreach (string r in periodos_list)
                {
                    totales.Columns.Add(r);
                    totales.Columns.Add(truco);
                    truco = truco + " ";
                }
                DataRow row;
                List<string> nombre_factores = get_factores();


                for (int i = 0; i <= 18; i++)
                {
                    row = totales.NewRow();
                    row["FACTORES/Periodos"] = nombre_factores[i];
                    totales.Rows.Add(row);

                    for (int y = 0; y <= colum - 1; y++)
                    {
                        if (i == 0)
                        {
                            if (izq)
                            {
                                totales.Rows[i][y + 1] = "# cltes";
                            }
                            else
                            {
                                totales.Rows[i][y + 1] = "Ventas";
                            }
                        }
                        if (i == 1)
                        {

                            if (izq)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = "";

                                string mes_apreguntar = periodo_apreguntar.Substring(periodo_apreguntar.Length - 2);
                                string año = periodo_apreguntar.Substring(0, periodo_apreguntar.Length - 2);
                                if (mes_apreguntar == "02")
                                {
                                    periodo_anterior = año + "01";
                                }
                                else if (mes_apreguntar == "01")
                                {
                                    periodo_anterior = (Double.Parse(año) - 1).ToString() + "12";
                                }
                                else
                                {
                                    periodo_anterior = (Double.Parse(totales.Columns[y + 1].ColumnName) - 1).ToString();
                                }
                                totales.Rows[i][y + 1] = (ReporteRNegocio.COMPARATIVO_CLTE_VENTAS(periodo_apreguntar, periodo_anterior, where)).ToString("N0");
                            }
                            else
                            {
                                string periodo_apreguntar = totales.Columns[y].ColumnName;
                                string periodo_anterior = "";

                                string mes_apreguntar = periodo_apreguntar.Substring(periodo_apreguntar.Length - 2);
                                string año = periodo_apreguntar.Substring(0, periodo_apreguntar.Length - 2);
                                if (mes_apreguntar == "02")
                                {
                                    periodo_anterior = año + "01";
                                }
                                else if (mes_apreguntar == "01")
                                {
                                    periodo_anterior = (Double.Parse(año) - 1).ToString() + "12";
                                }
                                else
                                {
                                    periodo_anterior = (Double.Parse(totales.Columns[y].ColumnName) - 1).ToString();
                                }
                                totales.Rows[i][y + 1] = (ReporteRNegocio.sum_si_repite(periodo_apreguntar, periodo_anterior, where)).ToString("N0");

                            }
                        }


                        if (i == 2)
                        {

                            if (izq)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = "";

                                string mes_apreguntar = periodo_apreguntar.Substring(periodo_apreguntar.Length - 2);
                                string año = periodo_apreguntar.Substring(0, periodo_apreguntar.Length - 2);
                                if (mes_apreguntar == "02")
                                {
                                    periodo_anterior = año + "01";
                                }
                                else if (mes_apreguntar == "01")
                                {
                                    periodo_anterior = (Double.Parse(año) - 1).ToString() + "12";
                                }
                                else
                                {
                                    periodo_anterior = (Double.Parse(totales.Columns[y + 1].ColumnName) - 1).ToString();
                                }
                                totales.Rows[i][y + 1] = (ReporteRNegocio.cont_si_repite(periodo_apreguntar, periodo_anterior, where)).ToString("N0");
                            }
                            else
                            {

                                string periodo_apreguntar = totales.Columns[y].ColumnName;
                                string periodo_anterior = "";

                                string mes_apreguntar = periodo_apreguntar.Substring(periodo_apreguntar.Length - 2);
                                string año = periodo_apreguntar.Substring(0, periodo_apreguntar.Length - 2);
                                if (mes_apreguntar == "02")
                                {
                                    periodo_anterior = año + "01";
                                }
                                else if (mes_apreguntar == "01")
                                {
                                    periodo_anterior = (Double.Parse(año) - 1).ToString() + "12";
                                }
                                else
                                {
                                    periodo_anterior = (Double.Parse(totales.Columns[y].ColumnName) - 1).ToString();
                                }
                                totales.Rows[i][y + 1] = (ReporteRNegocio.sum_no_repite(periodo_apreguntar, periodo_anterior, where)).ToString("N0");
                            }
                        }

                        if (i == 3)
                        {
                            if (izq)
                            {

                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = "";
                                string periodo_2_meses = "";
                                string mes_apreguntar = periodo_apreguntar.Substring(periodo_apreguntar.Length - 2);
                                string año = periodo_apreguntar.Substring(0, periodo_apreguntar.Length - 2);
                                if (mes_apreguntar == "02")
                                {
                                    periodo_anterior = año + "01";
                                    periodo_2_meses = (Double.Parse(año) - 1).ToString() + "12";
                                }
                                else if (mes_apreguntar == "01")
                                {
                                    periodo_anterior = (Double.Parse(año) - 1).ToString() + "12";
                                    periodo_2_meses = (Double.Parse(año) - 1).ToString() + "11";
                                }
                                else
                                {
                                    periodo_anterior = (Double.Parse(totales.Columns[y + 1].ColumnName) - 1).ToString();
                                    periodo_2_meses = (Double.Parse(totales.Columns[y + 1].ColumnName) - 2).ToString();
                                }
                                totales.Rows[i][y + 1] = (ReporteRNegocio.cont_sin_x_2(periodo_apreguntar, periodo_anterior, periodo_2_meses, where)).ToString("N0"); ;

                            }
                            else
                            {
                                string periodo_apreguntar = totales.Columns[y].ColumnName;
                                string periodo_anterior = "";
                                string periodo_2_meses = "";
                                string mes_apreguntar = periodo_apreguntar.Substring(periodo_apreguntar.Length - 2);
                                string año = periodo_apreguntar.Substring(0, periodo_apreguntar.Length - 2);
                                if (mes_apreguntar == "02")
                                {
                                    periodo_anterior = año + "01";
                                    periodo_2_meses = (Double.Parse(año) - 1).ToString() + "12";
                                }
                                else if (mes_apreguntar == "01")
                                {
                                    periodo_anterior = (Double.Parse(año) - 1).ToString() + "12";
                                    periodo_2_meses = (Double.Parse(año) - 1).ToString() + "11";
                                }
                                else
                                {
                                    periodo_anterior = (Double.Parse(totales.Columns[y].ColumnName) - 1).ToString();
                                    periodo_2_meses = (Double.Parse(totales.Columns[y].ColumnName) - 2).ToString();
                                }
                                totales.Rows[i][y + 1] = (ReporteRNegocio.sum_sin_x_2(periodo_apreguntar, periodo_anterior, periodo_2_meses, where)).ToString("N0"); ;
                            }

                        }

                        if (i == 4)
                        {

                            if (izq)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = "";
                                string periodo_2_meses = "";
                                string mes_apreguntar = periodo_apreguntar.Substring(periodo_apreguntar.Length - 2);
                                string año = periodo_apreguntar.Substring(0, periodo_apreguntar.Length - 2);
                                if (mes_apreguntar == "02")
                                {
                                    periodo_anterior = año + "01";
                                    periodo_2_meses = (Double.Parse(año) - 1).ToString() + "12";
                                }
                                else if (mes_apreguntar == "01")
                                {
                                    periodo_anterior = (Double.Parse(año) - 1).ToString() + "12";
                                    periodo_2_meses = (Double.Parse(año) - 1).ToString() + "11";
                                }
                                else
                                {
                                    periodo_anterior = (Double.Parse(totales.Columns[y + 1].ColumnName) - 1).ToString();
                                    periodo_2_meses = (Double.Parse(totales.Columns[y + 1].ColumnName) - 2).ToString();
                                }
                                totales.Rows[i][y + 1] = (ReporteRNegocio.cont_nuevo_venta(periodo_apreguntar, periodo_anterior, where)).ToString("N0");
                            }
                            else
                            {

                                string periodo_apreguntar = totales.Columns[y].ColumnName;
                                string periodo_anterior = "";
                                string periodo_2_meses = "";
                                string mes_apreguntar = periodo_apreguntar.Substring(periodo_apreguntar.Length - 2);
                                string año = periodo_apreguntar.Substring(0, periodo_apreguntar.Length - 2);
                                if (mes_apreguntar == "02")
                                {
                                    periodo_anterior = año + "01";
                                    periodo_2_meses = (Double.Parse(año) - 1).ToString() + "12";
                                }
                                else if (mes_apreguntar == "01")
                                {
                                    periodo_anterior = (Double.Parse(año) - 1).ToString() + "12";
                                    periodo_2_meses = (Double.Parse(año) - 1).ToString() + "11";
                                }
                                else
                                {
                                    periodo_anterior = (Double.Parse(totales.Columns[y].ColumnName) - 1).ToString();
                                    periodo_2_meses = (Double.Parse(totales.Columns[y].ColumnName) - 2).ToString();
                                }
                                totales.Rows[i][y + 1] = (ReporteRNegocio.sum_nuevo_venta(periodo_apreguntar, periodo_anterior, where)).ToString("N0");
                            }
                        }


                        if (i == 6)
                        {

                            if (izq)
                            {
                                totales.Rows[i][y + 1] = "Var% en cantidad";
                            }
                            else
                            {
                                totales.Rows[i][y + 1] = "Var% en $";
                            }
                        }

                        if (i == 7)
                        {
                            if (izq)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = "";

                                string mes_apreguntar = periodo_apreguntar.Substring(periodo_apreguntar.Length - 2);
                                string año = periodo_apreguntar.Substring(0, periodo_apreguntar.Length - 2);
                                if (mes_apreguntar == "02")
                                {
                                    periodo_anterior = año + "01";
                                }
                                else if (mes_apreguntar == "01")
                                {
                                    periodo_anterior = (Double.Parse(año) - 1).ToString() + "12";
                                }
                                else
                                {
                                    periodo_anterior = (Double.Parse(totales.Columns[y + 1].ColumnName) - 1).ToString();
                                }
                                double resp = ReporteRNegocio._cltes_con_vta(periodo_anterior, where);
                                double dasdsa = Double.Parse(totales.Rows[1][y + 1].ToString().Replace(".", ""));
                                //double resp = Double.Parse(totales.Rows[4][y + 1].ToString().Replace(".", "")) + Double.Parse(totales.Rows[2][y + 1].ToString().Replace(".", "")); 
                                //double dasdsa = Double.Parse(totales.Rows[1][y + 1].ToString().Replace(".", ""));
                                totales.Rows[i][y + 1] = Math.Round((dasdsa * 100 / resp)).ToString() + " % ";
                            }
                            else
                            {
                                string periodo_apreguntar = totales.Columns[y].ColumnName;
                                string periodo_anterior = "";

                                string mes_apreguntar = periodo_apreguntar.Substring(periodo_apreguntar.Length - 2);
                                string año = periodo_apreguntar.Substring(0, periodo_apreguntar.Length - 2);
                                if (mes_apreguntar == "02")
                                {
                                    periodo_anterior = año + "01";
                                }
                                else if (mes_apreguntar == "01")
                                {
                                    periodo_anterior = (Double.Parse(año) - 1).ToString() + "12";
                                }
                                else
                                {
                                    periodo_anterior = (Double.Parse(totales.Columns[y].ColumnName) - 1).ToString();
                                }
                                double resp = ReporteRNegocio.Facturación_Mes(periodo_anterior, where);
                                double dasdsa = Double.Parse(totales.Rows[1][y + 1].ToString().Replace(".", ""));
                                totales.Rows[i][y + 1] = Math.Round((dasdsa * 100 / resp)).ToString() + " % ";
                            }
                        }


                        if (i == 8)
                        {
                            if (izq)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = "";

                                string mes_apreguntar = periodo_apreguntar.Substring(periodo_apreguntar.Length - 2);
                                string año = periodo_apreguntar.Substring(0, periodo_apreguntar.Length - 2);
                                if (mes_apreguntar == "02")
                                {
                                    periodo_anterior = año + "01";
                                }
                                else if (mes_apreguntar == "01")
                                {
                                    periodo_anterior = (Double.Parse(año) - 1).ToString() + "12";
                                }
                                else
                                {
                                    periodo_anterior = (Double.Parse(totales.Columns[y + 1].ColumnName) - 1).ToString();
                                }
                                double resp = ReporteRNegocio._cltes_con_vta(periodo_anterior, where);
                                double dasdsa = Double.Parse(totales.Rows[4][y + 1].ToString().Replace(".", ""));
                                totales.Rows[i][y + 1] = Math.Round((dasdsa * 100 / resp)).ToString() + " % ";
                            }
                            else
                            {
                                string periodo_apreguntar = totales.Columns[y].ColumnName;
                                string periodo_anterior = "";

                                string mes_apreguntar = periodo_apreguntar.Substring(periodo_apreguntar.Length - 2);
                                string año = periodo_apreguntar.Substring(0, periodo_apreguntar.Length - 2);
                                if (mes_apreguntar == "02")
                                {
                                    periodo_anterior = año + "01";
                                }
                                else if (mes_apreguntar == "01")
                                {
                                    periodo_anterior = (Double.Parse(año) - 1).ToString() + "12";
                                }
                                else
                                {
                                    periodo_anterior = (Double.Parse(totales.Columns[y].ColumnName) - 1).ToString();
                                }
                                double resp = ReporteRNegocio.Facturación_Mes(periodo_anterior, where);
                                double dasdsa = Double.Parse(totales.Rows[4][y + 1].ToString().Replace(".", ""));
                                totales.Rows[i][y + 1] = Math.Round((dasdsa * 100 / resp)).ToString() + " % ";
                            }
                        }

                        if (i == 10)
                        {

                            if (izq)
                            {
                                totales.Rows[i][y + 1] = "";
                            }
                            else
                            {
                                string periodo = totales.Columns[y].ColumnName;
                                //totales.Rows[i][y + 1] = ReporteRNegocio.Facturación_Mes(periodo, where).ToString("N0");
                                totales.Rows[i][y + 1] = (Double.Parse(totales.Rows[2][y + 1].ToString().Replace(".", "")) + Double.Parse(totales.Rows[1][y + 1].ToString().Replace(".", ""))).ToString("N0");
                            }
                        }

                        if (i == 11)
                        {
                            if (izq)
                            {

                                totales.Rows[i][y + 1] = "";
                            }
                            else
                            {
                                string periodo = totales.Columns[y].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio._cltes_con_vta(periodo, where).ToString("N0");
                                //totales.Rows[i][y + 1] = (Double.Parse(totales.Rows[2][y].ToString().Replace(".", "")) + Double.Parse(totales.Rows[1][y].ToString().Replace(".", ""))).ToString("N0");
                            }

                        }

                        if (i == 12)
                        {
                            if (izq)
                            {

                                totales.Rows[i][y + 1] = "";
                            }
                            else
                            {
                                totales.Rows[i][y + 1] = (+(Double.Parse(totales.Rows[10][y + 1].ToString().Replace(".", ""))) / (Double.Parse(totales.Rows[11][y + 1].ToString().Replace(".", "")))).ToString("N0");
                            }

                        }

                        if (i == 13)
                        {
                            if (izq)
                            {
                                if (t_percentil.Text == "") { totales.Rows[i][y + 1] = "50%"; }
                                else
                                {
                                    totales.Rows[i][y + 1] = t_percentil.Text + "%";
                                }
                            }
                            else
                            {
                                string periodo = totales.Columns[y].ColumnName;
                                List<long> percen = (ReporteRNegocio.Datos_para_percentil(periodo, where)).ToList();
                                if (percen.Count == 0) { totales.Rows[i][y + 1] = "0"; }
                                else
                                {
                                    Double por_percentil;
                                    if (t_percentil.Text == "") { por_percentil = 0.5; }
                                    else
                                    {
                                        por_percentil = Math.Round(Double.Parse(t_percentil.Text) / 100, 2);
                                    }
                                    totales.Rows[i][y + 1] = Math.Round(Percentile(percen.ToArray(), por_percentil)).ToString("N0");
                                }
                            }

                        }

                        if (i == 14)
                        {
                            if (izq)
                            {

                                totales.Rows[i][y + 1] = "";
                            }
                            else
                            {
                                string periodo = totales.Columns[y].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio.sum_sobre_este_percentil(periodo, where, Double.Parse(totales.Rows[13][y + 1].ToString().Replace(".", ""))).ToString("N0");

                            }

                        }

                        if (i == 15)
                        {
                            if (izq)
                            {

                                totales.Rows[i][y + 1] = "";
                            }
                            else
                            {
                                Double sumatoria = (Double.Parse(totales.Rows[10][y + 1].ToString().Replace(".", "")));
                                Double sum_vent_sobre_percen = (Double.Parse(totales.Rows[14][y + 1].ToString().Replace(".", "")));
                                totales.Rows[i][y + 1] = Math.Round((sum_vent_sobre_percen / sumatoria * 100)).ToString() + " %";
                            }

                        }


                        if (i == 16)
                        {
                            if (izq)
                            {

                                totales.Rows[i][y + 1] = "";
                            }
                            else
                            {
                                string periodo = totales.Columns[y].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio.cont_sobre_este_percentil(periodo, where, Double.Parse(totales.Rows[13][y + 1].ToString().Replace(".", ""))).ToString("N0");

                            }

                        }

                        if (i == 18)
                        {
                            if (izq)
                            {

                                totales.Rows[i][y + 1] = "";
                            }
                            else
                            {
                                string periodo_apreguntar = totales.Columns[y].ColumnName;
                                string periodo_anterior = "";

                                string mes_apreguntar = periodo_apreguntar.Substring(periodo_apreguntar.Length - 2);
                                string año = periodo_apreguntar.Substring(0, periodo_apreguntar.Length - 2);
                                if (mes_apreguntar == "02")
                                {
                                    periodo_anterior = año + "01";
                                }
                                else if (mes_apreguntar == "01")
                                {
                                    periodo_anterior = (Double.Parse(año) - 1).ToString() + "12";
                                }
                                else
                                {
                                    periodo_anterior = (Double.Parse(totales.Columns[y].ColumnName) - 1).ToString();
                                }
                                totales.Rows[i][y + 1] = (ReporteRNegocio.sum_si_repite_actual(periodo_apreguntar, periodo_anterior, where)).ToString("N0");

                            }

                        }


                        // Cambia Switch
                        izq = !izq;
                    }
                }
                G_INFORME_TOTAL_VENDEDOR.DataSource = totales;
                G_INFORME_TOTAL_VENDEDOR.DataBind();
                //VOLVER A CARGAR LOS MULTISELECT

                DataTable dt = new DataTable();

                try
                {
                    dt = ReporteRNegocio.carga_grupos(desde, hasta, grupos_del_usuario);
                    //dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
                    DataView dv2 = dt.DefaultView;
                    dv2.Sort = "user1";
                    dt = dv2.ToTable();
                    d_grupos_usuario.DataSource = dt;
                    d_grupos_usuario.DataTextField = "user1";
                    d_grupos_usuario.DataValueField = "user1";
                    //d_vendedor_.SelectedIndex = -1;
                    d_grupos_usuario.DataBind();


                    foreach (ListItem item in d_grupos_usuario.Items)
                    {

                        if (l_grupos.Text.Contains(item.Value.ToString()))
                        {
                            item.Selected = true;
                        }
                    }
                }
                catch { }

                string where2 = " where FechaFactura >= CONVERT(datetime,'" + desde + "', 103) " +
                                 " and FechaFactura <= CONVERT(datetime,'" + hasta + "',103) ";

                if (grupos != "")
                {
                    where2 = where2 + " and user1 in (" + grupos + ") ";
                }
                else if (es_vendedor != "2")
                {
                    where2 = where2 + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario(USER)) + ")";
                }
                else if (es_vendedor == "2")
                {
                    where2 = where2 + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(USER)) + ")";
                }

                if (es_vendedor == "2")
                {
                    where2 += " and codvendedor in ('" + USER + "')";
                }

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
                where2 = "";
                where2 = " where FechaFactura >= CONVERT(datetime,'" + desde + "', 103) " +
                                 " and FechaFactura <= CONVERT(datetime,'" + hasta + "',103) ";

                if (grupos != "")
                {
                    where2 = where2 + " and user1 in (" + grupos + ") ";
                }
                else if (es_vendedor != "2")
                {
                    where2 = where2 + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario(USER)) + ")";
                }
                else if (es_vendedor == "2")
                {
                    where2 = where2 + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(USER)) + ")";
                }
                if (vendedores != "")
                {
                    where2 += " and codvendedor in (" + vendedores + ")";
                }
                if (es_vendedor == "2")
                {
                    where2 += " and codvendedor in ('" + USER + "')";
                }

                try
                {
                    dt = ReporteRNegocio.listar_ALL_cliente2(where2);
                    //dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
                    DataView dv3 = dt.DefaultView;
                    dv3.Sort = "nom_cliente";
                    dt = dv3.ToTable();
                    d_cliente.DataSource = dt;
                    d_cliente.DataTextField = "nom_cliente";
                    d_cliente.DataValueField = "rut_cliente";
                    //d_vendedor_.SelectedIndex = -1;
                    d_cliente.DataBind();


                    foreach (ListItem item in d_cliente.Items)
                    {

                        if (l_clientes.Text.Contains(item.Value.ToString()))
                        {
                            item.Selected = true;
                        }
                    }
                }
                catch { }
            }
        }

        private List<string> get_factores()
        {
            List<string> aux = new List<string>();
            aux.Add(""); //0
            aux.Add("Si Repite");
            aux.Add("No Repite");
            aux.Add("Sin vta 2 meses");
            aux.Add("Nueva Venta");
            aux.Add(""); // 5
            aux.Add("");
            aux.Add("% cltes que repiten r/mes ant.");
            aux.Add("% Nva venta r/mes ant.");
            aux.Add("");
            aux.Add("Venta Total"); // 10
            aux.Add("# cltes con venta");
            aux.Add("Promedio venta x clte facturado");
            aux.Add("Percentil");
            aux.Add("Venta sobre este percentil");
            aux.Add("% c/r Vta Total"); // 15
            aux.Add("Num. Cltes sobre ese percentil");
            aux.Add("");
            aux.Add("Vta cltes mes anterior q/repiten");
            // CAPI
            //aux.Add("% crecimiento cltes q repiten");
            return aux;
        }


        protected void btn_excel_Click(object sender, EventArgs e)
        {
            Response.Clear();

            Response.AddHeader("content-disposition", "attachment;filename=SOPRODI_1_" + DateTime.Now.ToShortDateString() + ".xls");

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

        public double Percentile(long[] sequence, double excelPercentile)
        {

            Array.Sort(sequence);
            long N = sequence.Length;
            double n = (N - 1) * excelPercentile + 1;
            // Another method: double n = (N + 1) * excelPercentile;
            if (n == 1d) return sequence[0];
            else if (n == N) return sequence[N - 1];
            else
            {
                long k = (long)n;
                double d = n - k;
                return sequence[k - 1] + d * (sequence[k] - sequence[k - 1]);
            }
        }

        protected void G_INFORME_COMPARATIVO_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                {

                    e.Row.BackColor = Color.FromArgb(180, 206, 228);
                    e.Row.ForeColor = Color.White;
                    e.Row.Cells[0].BackColor = Color.White;
                }

                if (e.Row.RowIndex == 6)
                {

                    e.Row.BackColor = Color.FromArgb(180, 206, 228);
                    e.Row.ForeColor = Color.White;
                    e.Row.Cells[0].BackColor = Color.White;
                }


            }
        }

        protected void G_PRODUCTOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void G_INFORME_TOTAL_VENDEDOR_RowDataBound(object sender, GridViewRowEventArgs e)
        {



            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                {

                    e.Row.BackColor = Color.FromArgb(180, 206, 228);
                    e.Row.ForeColor = Color.White;
                    e.Row.Cells[0].BackColor = Color.White;
                }

                if (e.Row.RowIndex == 6)
                {

                    e.Row.BackColor = Color.FromArgb(180, 206, 228);
                    e.Row.ForeColor = Color.White;
                    e.Row.Cells[0].BackColor = Color.White;
                }


            }







        }

        protected void G_INFORME_VENDEDOR_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}