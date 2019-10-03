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
    public partial class REPORTE_SALA : System.Web.UI.Page
    {
        public static int cont_periodos;
        public static DataTable aux;
        public static DataTable productos;
        public static DataTable totales;
        public static string PERIODOS;
        private static string USER;
        public static string periodo_para_nuevos;
        public static DataTable clientes_nuevos;
        public static bool header_sum = true;
        private static string where = " where 1=1 ";
        public static int cont_det;
        public static double suma_no_repite = 0;

        public static int select_meses;
        protected void Page_Load(object sender, EventArgs e)
        {
            JQ_Datatable();
            //Page.RegisterRedirectOnSessionEndScript();
            if (!IsPostBack)
            {
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "3" || u_ne.Trim() == "8")
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
                //cargar_combo_VENDEDOR();
                //cargar_clientes();
                USER = User.Identity.Name.ToString();
                l_usuario_.Text = USER;
                DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now;
                //t = new DateTime(t.Year, t.Month - 6, 1);
                txt_desde.Text = ReporteRNegocio.CALCULA_DESDE(t.Month, t.Year);
                txt_hasta.Text = t2.ToShortDateString();
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
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            MakeAccessible(G_PRODUCTOS);
            //MakeAccessible(G_ASIGNADOS);
            //MakeAccessible(G_QR);
            //MakeAccessible(G_LISTA);
        }
        public void JQ_Datatable()
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd121mp", "<script language='javascript'>creagrilla();</script>", false);
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


        protected void b_Click(object sender, ImageClickEventArgs e)
        {



            string desde = txt_desde.Text;
            string hasta = txt_hasta.Text;
            string periodos = ReporteRNegocio.listar_periodos_(" where FechaFactura >= CONVERT(datetime,'" + desde + "', 103)  and FechaFactura <= CONVERT(datetime,'" + hasta + "',103)");
            PERIODOS = periodos;

            List<string> periodos_list = periodos.Split(',').ToList();

            int colum = periodos_list.Count;

            if (colum >= 3)
            {
                parametro.Visible = true;
                List<string> meses = new List<string>();
                for (int i = 1; i < colum; i++)
                {
                    meses.Add(i.ToString());
                }

                cargar_combo_meses(meses);
            }
            else
            {
                D_param.SelectedValue = "1";

                parametro.Visible = false;
            }

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

        private void cargar_combo_clientes(DataTable dataTable, DataView dtv)
        {
            //dt.Rows.Add(new Object[] { -1, "-- Todos --" });
            dtv = dataTable.DefaultView;
            dtv.Sort = "nom_cliente";
            d_cliente.DataSource = dtv;
            d_cliente.DataTextField = "nom_cliente";
            d_cliente.DataValueField = "rut_cliente";
            //d_vendedor_.SelectedIndex = -1;
            d_cliente.DataBind();
        }

        private void cargar_combo_meses(List<string> meses)
        {
            DataTable dt; DataView dtv;

            dt = ConvertListToDataTable(meses);
            dtv = dt.DefaultView;
            D_param.DataSource = dtv;
            D_param.DataValueField = "Column1";
            D_param.DataBind();
            D_param.SelectedValue = "1";
            D_param.Enabled = true;
        }

        private DataTable ConvertListToDataTable(List<string> list)
        {
            // New table.
            DataTable table = new DataTable();

            // Get max columns.
            int columns = 0;
            foreach (var array in list)
            {
                if (array.Length > columns)
                {
                    columns = array.Length;
                }
            }

            // Add columns.
            for (int i = 0; i < columns; i++)
            {
                table.Columns.Add();
            }

            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array);
            }

            return table;
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

        private void cargar_combo_Cliente(DataTable dt, DataView dtv)
        {
            dt.Rows.Add(new Object[] { -1, "-- Todos --" });
            dtv = dt.DefaultView;
            dtv.Sort = "nom_cliente";
            d_cliente.DataSource = dtv;
            d_cliente.DataTextField = "nom_cliente";
            d_cliente.DataValueField = "rut_cliente";
            //d_vendedor_.SelectedIndex = -1;
            d_cliente.DataBind();
        }
        public string Rvendedores_todos()
        {
            string vend = "";
            foreach (ListItem item in d_vendedor_.Items)
            {
                if (item.Value != "-1")
                {
                    if (vend == "")
                    {
                        vend += item.Value + ",";
                    }
                    else
                    {
                        vend += item.Value + ",";
                    }
                }
            }
            string ultimo = vend.Substring(vend.Length - 1, 1);
            if (ultimo == ",")
            {
                vend = vend.Substring(0, vend.Length - 1);
            }
            return vend;
        }
        private string Rclientes_todos()
        {
            string vend = "";
            foreach (ListItem item in this.d_cliente.Items)
            {
                if (item.Value != "-1")
                {
                    if (vend == "")
                    {
                        vend += item.Value + ",";
                    }
                    else
                    {
                        vend += item.Value + ",";
                    }
                }
            }
            string ultimo = vend.Substring(vend.Length - 1, 1);
            if (ultimo == ",")
            {
                vend = vend.Substring(0, vend.Length - 1);
            }
            return vend;
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

        protected void SetColumnsOrder(DataTable table, params String[] columnNames)
        {
            for (int columnIndex = 2; columnIndex < columnNames.Length; columnIndex++)
            {
                table.Columns[columnNames[columnIndex]].SetOrdinal(columnIndex);
            }
        }


        protected void btn_informe_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_PRODUCTOS')); </script>", false);

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
                string where = " where FechaFactura >= CONVERT(datetime,'" + desde + "', 103)  and FechaFactura <= CONVERT(datetime,'" + hasta + "',103) ";
                string where_es = " where FechaFactura <= CONVERT(datetime,'" + hasta + "',103) ";
                if (grupos != "")
                {
                    where = where + " and user1 in (" + grupos + ") ";
                    where_es = where_es + " and user1 in (" + grupos + ") ";
                }
                else if (es_vendedor != "2")
                {
                    where = where + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario(USER)) + ")";
                    where_es = where_es + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario(USER)) + ")";
                }

                else if (es_vendedor == "2")
                {
                    where = where + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(USER)) + ")";
                    where_es = where_es + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(USER)) + ")";
                }

                if (clientes != "")
                {
                    where = where + " and rutcliente in (" + clientes + ") ";
                    where_es = where_es + " and rutcliente in (" + clientes + ") ";
                }
                if (vendedores != "")
                {
                    where = where + " and codvendedor in (" + vendedores + ") ";
                    where_es = where_es + " and codvendedor in (" + vendedores + ") ";
                }
                if (es_vendedor == "2")
                {
                    where += " and codvendedor in ('" + USER + "')";
                    where_es += " and codvendedor in ('" + USER + "')";
                }

                div_report.Visible = true;
                cont_periodos = 0;
                G_INFORME_VENDEDOR.Visible = false;
                G_INFORME_TOTAL_VENDEDOR.Visible = true;
                string periodos = ReporteRNegocio.listar_periodos_(where);
                //aux = ReporteRNegocio.listar_resumen_periodo(where + " and periodo in ("+agregra_comillas(periodos)+")");
                totales = new DataTable();

                List<string> periodos_list = periodos.Split(',').ToList();

                totales.Columns.Add("FACTORES/Periodos");
                int colum = periodos_list.Count;

                foreach (string r in periodos_list)
                {
                    totales.Columns.Add(r);
                }
                DataRow row;
                List<string> nombre_factores = get_factores();

                // CAPI
                //totales.Columns.Add("Total general");

                for (int i = 0; i <= 18; i++)
                {
                    row = totales.NewRow();
                    if (nombre_factores[i].ToString().Contains("Percentil "))
                    {
                        row["FACTORES/Periodos"] = nombre_factores[i] + t_percentil.Text + "%";
                    }
                    else { row["FACTORES/Periodos"] = nombre_factores[i]; }
                    totales.Rows.Add(row);

                    for (int y = 0; y < colum; y++)
                    {
                        if (i == 0)
                        {

                            if (y == colum)
                            {
                                // CAPI
                                //totales.Rows[i][y + 1] = ReporteRNegocio.Facturación_Mes(agregra_comillas(periodos), where).ToString("N0");
                            }
                            else
                            {
                                string periodo = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio.Facturación_Mes(periodo, where).ToString("N0");
                            }
                        }
                        if (i == 1)
                        {

                            if (y == colum)
                            {
                                // CAPI
                                //totales.Rows[i][y + 1] = ReporteRNegocio._cltes_con_vta(agregra_comillas(periodos), where).ToString("N0");
                            }
                            else
                            {
                                string periodo = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio._cltes_con_vta(periodo, where).ToString("N0");
                            }

                        }
                        if (i == 2)
                        {
                            totales.Rows[i][y + 1] = (+(Double.Parse(totales.Rows[0][y + 1].ToString().Replace(".", ""))) / (Double.Parse(totales.Rows[1][y + 1].ToString().Replace(".", "")))).ToString("N0");
                        }
                        if (i == 3)
                        {
                            if (y == colum)
                            {
                                // CAPI
                                //List<int> percen = (ReporteRNegocio.Datos_para_percentil(agregra_comillas(periodos), where)).ToList();
                                //if (percen.Count == 0) { totales.Rows[i][y + 1] = "0"; }
                                //else
                                //{
                                //    Double por_percentil;
                                //    if (t_percentil.Text == "") { por_percentil = 0.5; }
                                //    else
                                //    {
                                //        por_percentil = Math.Round(Double.Parse(t_percentil.Text) / 100, 2);
                                //    }
                                //    totales.Rows[i][y + 1] = Math.Round(Percentile(percen.ToArray(), por_percentil)).ToString("N0");
                                //}
                            }
                            else
                            {

                                string periodo = totales.Columns[y + 1].ColumnName;
                                List<long> percen = ReporteRNegocio.Datos_para_percentil(periodo, where);
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

                        if (i == 4)
                        {

                            if (y == colum)
                            {
                                totales.Rows[i][y + 1] = ReporteRNegocio.cltes_sobre_este_percentil(agregra_comillas(periodos), where, Double.Parse(totales.Rows[3][y + 1].ToString().Replace(".", ""))).ToString("N0");
                            }
                            else
                            {
                                string periodo = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio.cltes_sobre_este_percentil(periodo, where, Double.Parse(totales.Rows[3][y + 1].ToString().Replace(".", ""))).ToString("N0");
                            }
                        }

                        if (i == 5)
                        {

                            if (y == colum)
                            {
                                totales.Rows[i][y + 1] = ReporteRNegocio.sum_sobre_este_percentil(agregra_comillas(periodos), where, Double.Parse(totales.Rows[3][y + 1].ToString().Replace(".", ""))).ToString("N0");
                            }
                            else
                            {
                                string periodo = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio.sum_sobre_este_percentil(periodo, where, Double.Parse(totales.Rows[3][y + 1].ToString().Replace(".", ""))).ToString("N0");
                            }
                        }
                        if (i == 6)
                        {
                            Double sumatoria = (Double.Parse(totales.Rows[0][y + 1].ToString().Replace(".", "")));
                            Double sum_vent_sobre_percen = (Double.Parse(totales.Rows[5][y + 1].ToString().Replace(".", "")));
                            totales.Rows[i][y + 1] = Math.Round((sum_vent_sobre_percen / sumatoria * 100)).ToString() + " %";
                        }
                        if (i == 7)
                        {
                            if (y <= colum - 2)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = totales.Columns[y + 2].ColumnName;
                                totales.Rows[i][y + 1] = (ReporteRNegocio.cont_si_repite(periodo_apreguntar, periodo_anterior, where)).ToString("N0");
                            }
                        }
                        if (i == 8)
                        {
                            if (y <= colum - 2)
                            {
                                Double cont_si_repite = (Double.Parse(totales.Rows[7][y + 1].ToString().Replace(".", "")));
                                Double clte_con_vta_anterior = (Double.Parse(totales.Rows[1][y + 2].ToString().Replace(".", "")));
                                totales.Rows[i][y + 1] = Math.Round((cont_si_repite / clte_con_vta_anterior * 100)).ToString() + " %";
                            }
                        }
                        if (i == 9)
                        {
                            if (y <= colum - 2)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = totales.Columns[y + 2].ColumnName;
                                totales.Rows[i][y + 1] = (ReporteRNegocio.sum_a_recuperar(periodo_apreguntar, periodo_anterior, where)).ToString("N0");
                            }
                        }


                        if (i == 10)
                        {
                            if (y <= colum - 2)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = totales.Columns[y + 2].ColumnName;
                                totales.Rows[i][y + 1] = (ReporteRNegocio.sum_si_repite(periodo_apreguntar, periodo_anterior, where)).ToString("N0");
                            }
                        }
                        if (i == 11)
                        {
                            if (y <= colum - 2)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = totales.Columns[y + 2].ColumnName;
                                totales.Rows[i][y + 1] = (ReporteRNegocio.sum_si_repite_actual(periodo_apreguntar, periodo_anterior, where)).ToString("N0");
                            }
                        }

                        if (i == 12)
                        {
                            if (y <= colum - 2)
                            {
                                Double sum_mes_c_anterior = (Double.Parse(totales.Rows[10][y + 1].ToString().Replace(".", "")));
                                Double sum_mes_anterior = (Double.Parse(totales.Rows[11][y + 1].ToString().Replace(".", "")));
                                totales.Rows[i][y + 1] = (Math.Round((sum_mes_c_anterior * 100 / sum_mes_anterior)) - 100).ToString() + " %";
                            }
                        }

                        if (i == 13)
                        {
                            if (y <= colum - 1)
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
                                totales.Rows[i][y + 1] = (ReporteRNegocio.cont_sin_x_2(periodo_apreguntar, periodo_anterior, periodo_2_meses, where_es)).ToString("N0"); ;
                            }
                        }
                        if (i == 14)
                        {
                            if (y <= colum - 1)
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
                                totales.Rows[i][y + 1] = Math.Round((Double.Parse(totales.Rows[i - 1][y + 1].ToString().Replace(".", ""))) / ReporteRNegocio._cltes_con_vta(periodo_2_meses, where_es) * 100).ToString() + " %";
                                //totales.Rows[i][y + 1] = Math.Round(((+(Double.Parse(totales.Rows[i-1][y + 1].ToString().Replace(".", ""))) * 100 / ReporteRNegocio._cltes_con_vta(periodo_2_meses, where))) - 100).ToString() + " %";
                            }
                        }

                        if (i == 15)
                        {
                            if (y <= colum - 1)
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
                                totales.Rows[i][y + 1] = (ReporteRNegocio.sum_sin_x_2(periodo_apreguntar, periodo_anterior, periodo_2_meses, where_es)).ToString("N0"); ;
                            }
                        }

                        if (i == 16)
                        {
                            if (y <= colum - 1)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = (ReporteRNegocio.cont_nuevo_cliente(periodo_apreguntar, where_es)).ToString("N0"); ;
                            }
                        }

                        if (i == 17)
                        {
                            if (y <= colum - 1)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = (ReporteRNegocio.sum_nuevo_cliente(periodo_apreguntar, where_es)).ToString("N0"); ;
                            }
                        }

                        if (i == 18)
                        {
                            if (y <= colum - 1)
                            {

                                totales.Rows[i][y + 1] = (+(Double.Parse(totales.Rows[i - 1][y + 1].ToString().Replace(".", ""))) / (Double.Parse(totales.Rows[i - 2][y + 1].ToString().Replace(".", "")))).ToString("N0");

                            }
                        }


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
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_PRODUCTOS')); </script>", false);


        }

        private List<string> get_factores()
        {
            List<string> aux = new List<string>();

            aux.Add("Facturación Mes");
            aux.Add("# cltes con vta");
            aux.Add("Promedio x clte");
            aux.Add("Percentil ");
            aux.Add("# cltes sobre este percentil");
            aux.Add("Venta sobre este percentil");
            aux.Add("% c/r Vta Total");
            aux.Add("#Cltes que NO Repiten r/mes anterior");
            aux.Add("% Cltes que NO repiten r/mes anterior");
            aux.Add("Venta a recuperar mes anterior");
            aux.Add("Vtas $ de cltes que repiten c/r mes anterior");
            aux.Add("Vtas $ de cltes del mes anterior que repiten este mes");
            aux.Add("Var% de la vta en cltes que repiten r/mes ant.");
            aux.Add("#Cltes sin vta x 2 meses");
            aux.Add("% cltes sin vta x 2 meses");
            aux.Add("Venta a recuperar x 2 meses sin vta");
            aux.Add("# cltes NUEVOS");
            aux.Add("$ Vta con cltes NUEVOS");
            aux.Add("Promedio cltes NUEVOS");
            // CAPI
            //aux.Add("Venta con Nuevos de mes anterior");
            //aux.Add("Venta con cltes que alguna vez fueron Nuevos");
            return aux;
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
                double sum_total = 0;
                for (int i = 2; i <= colum - 1; i++)
                {

                    double d;
                    double.TryParse(e.Row.Cells[i + 1].Text, out d);
                    sum_total = sum_total + d;

                }
                e.Row.Cells[e.Row.Cells.Count - 1].Text = sum_total.ToString();
                aux.Rows[e.Row.RowIndex]["Total general"] = sum_total.ToString();
            }
        }

        protected void G_INFORME_TOTAL_VENDEDOR_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //Facturación Mes
                if (e.Row.RowIndex == 0)
                {

                    e.Row.Cells[0].BackColor = Color.FromArgb(255, 245, 210);
                }

                if (e.Row.RowIndex == 3)
                {

                    e.Row.Cells[0].BackColor = Color.FromArgb(255, 245, 210);
                }

                if (e.Row.RowIndex == 7)
                {

                    e.Row.Cells[0].BackColor = Color.FromArgb(255, 245, 210);
                }


                if (e.Row.RowIndex == 10)
                {

                    e.Row.Cells[0].BackColor = Color.FromArgb(255, 245, 210);

                }

                if (e.Row.RowIndex == 13)
                {

                    e.Row.Cells[0].BackColor = Color.FromArgb(216, 197, 253);
                }

                if (e.Row.RowIndex == 16)
                {

                    e.Row.Cells[0].BackColor = Color.FromArgb(255, 245, 210);

                }

            }
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

        protected void btn_productos_Click(object sender, EventArgs e)
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
                string where = " where A.FechaFactura >= CONVERT(datetime,'" + desde + "', 103)  and A.FechaFactura <= CONVERT(datetime,'" + hasta + "',103) ";
                string where2_ = " where 1=1 ";
                if (grupos != "")
                {
                    where = where + " and A.user1 in (" + grupos + ") ";
                    where2_ = where2_ + " and user1 in (" + grupos + ") ";
                }
                else if (es_vendedor != "2")
                {
                    where = where + " and A.user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario(USER)) + ")";
                    where2_ = where2_ + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario(USER)) + ")";
                }
                else if (es_vendedor == "2")
                {
                    where = where + " and A.user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(USER)) + ")";
                    where2_ = where2_ + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(USER)) + ")";
                }

                if (clientes != "")
                {
                    where = where + " and A.rutcliente in (" + clientes + ") ";
                    where2_ = where2_ + " and rutcliente in (" + clientes + ") ";
                }
                if (vendedores != "")
                {
                    where = where + " and A.codvendedor in (" + vendedores + ") ";
                    where2_ = where2_ + " and codvendedor in (" + vendedores + ") ";
                }
                if (es_vendedor == "2")
                {
                    where += " and A.codvendedor in ('" + USER + "')";
                    where2_ += " and codvendedor in ('" + USER + "')";
                }

                div_productos.Visible = true;
                string periodos = ReporteRNegocio.listar_periodos_(" where FechaFactura >= CONVERT(datetime,'" + desde + "', 103)  and FechaFactura <= CONVERT(datetime,'" + hasta + "',103)");

                productos = ReporteRNegocio.listar_ventas_SALA(periodos, where);

                string periodo_mayor = ReporteRNegocio.listar_periodo_nuevo(where.Replace("A.",""));
                string periodo_menor = ReporteRNegocio.listar_periodo_nuevo_(where.Replace("A.", ""));

                clientes_nuevos = ReporteRNegocio.listar_clientes_nuevos(periodos, periodo_mayor, where2_);

                PERIODOS = periodos;
                DataView view = new DataView(productos);
                DataTable distinctValues = view.ToTable(true, "NombreCliente");



                int colum = productos.Columns.Count;
                productos.Columns.Add("Total");
                DataView dv = productos.DefaultView;
                dv.Sort = "NombreCliente ASC";
                productos = dv.ToTable();

                string periodo_nuevo_mes = txt_hasta.Text.Substring(txt_hasta.Text.IndexOf("/") + 1, 2);
                string periodo_nuevo_año = txt_hasta.Text.Substring(txt_hasta.Text.LastIndexOf("/") + 1);

                periodo_para_nuevos = periodo_nuevo_año + periodo_nuevo_mes;

                //periodo_para_nuevos = ReporteRNegocio.listar_periodo_nuevo(txt_hasta.Text);

                header_sum = true;

                cont_det = 1;
                suma_no_repite = 0;

                select_meses = 1;
                try
                {
                    select_meses = Convert.ToInt32(D_param.SelectedValue.ToString());
                }
                catch
                {
                    select_meses = 1;
                }

                G_PRODUCTOS.DataSource = productos;
                G_PRODUCTOS.DataBind();
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
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_PRODUCTOS')); </script>", false);

        }

        protected void G_PRODUCTOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Cells[0].Text = cont_det.ToString();
                cont_det++;

                int cont_vueltas = 0;
                bool se_muestra = true;

                string rutcliente = G_PRODUCTOS.DataKeys[e.Row.RowIndex].Values[1].ToString();



                DataRow[] venta = clientes_nuevos.Select(" rutcliente = '" + rutcliente.Trim() + "'");

                int cont = 0;
                foreach (DataRow row in venta)
                {
                    cont++;
                }
                if (cont > 0)
                {
                    e.Row.BackColor = Color.FromArgb(247, 247, 114);
                }
                string rutcliente_invi = e.Row.Cells[2].Text;
                string codvendedor_invi = e.Row.Cells[1].Text;

                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                G_PRODUCTOS.HeaderRow.Cells[1].Visible = false;
                G_PRODUCTOS.HeaderRow.Cells[2].Visible = false;

                for (int i = 5; cont_vueltas < select_meses; i++)
                {

                    if (e.Row.Cells[i + 1].Text == "&nbsp;")
                    {

                    }
                    else
                    {
                        se_muestra = false;
                        e.Row.Cells[1].Text = "";
                    }
                    cont_vueltas++;
                }
                if (se_muestra)
                {
                    if (e.Row.Cells[6 + select_meses].Text == "&nbsp;")
                    {
                        se_muestra = false;
                    }
               
              
                try
                {
                    if (se_muestra)
                    {
                        int suma = 0;
                        int cont_con_vta = 0;
                        for (int i = 5 + select_meses; i < productos.Columns.Count - 1; i++)
                        {
                            int num;
                            int.TryParse(e.Row.Cells[i + 1].Text.Replace(".", ""), out num);
                            if (num != 0) { cont_con_vta++; }
                            G_PRODUCTOS.HeaderRow.Cells[i + 1].Attributes["data-sort-method"] = "number";
                            suma += num;
                        }
                        string tot = (suma / cont_con_vta).ToString("N0");
                        if (tot == "0") { tot = ""; }
                        e.Row.Cells[3].Text = tot;
                        G_PRODUCTOS.HeaderRow.Cells[6].Attributes["data-sort-method"] = "number";

                    }
                }
                catch { }


                if (e.Row.Cells[3].Text != "" && e.Row.Cells[3].Text != "&nbsp;")
                {
                    suma_no_repite += double.Parse(e.Row.Cells[3].Text.Replace(".", ""));
                }
                }

                double sum_por_row = 0;
                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                string script1 = string.Format("javascript:fuera22(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(rutcliente), encriptador.EncryptData("88"));
                e.Row.Cells[5].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[5].Text + " </a>";


                for (int i = 6; i <= productos.Columns.Count - 1; i++)
                {
                    if (header_sum)
                    {
                        try
                        {
                            G_PRODUCTOS.HeaderRow.Cells[i].Text = G_PRODUCTOS.HeaderRow.Cells[i].Text + "  (" + Convert.ToDouble(ReporteRNegocio.Facturación_Mes(G_PRODUCTOS.HeaderRow.Cells[i].Text, where)).ToString("N0") + ")";

                        }
                        catch { }
                    }
                    if (i == productos.Columns.Count - 1) { header_sum = false; }

                    G_PRODUCTOS.HeaderRow.Cells[i].Attributes["data-sort-method"] = "number";
                    double d;
                    double.TryParse(e.Row.Cells[i].Text, out d);
                    string aux = "";
                    if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                    e.Row.Cells[i].Text = aux;
                    sum_por_row += d;

                    if (i != productos.Columns.Count)
                    {
                        string script = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;);return false;", encriptador.EncryptData(G_PRODUCTOS.HeaderRow.Cells[i].Text.Substring(0, 6)), encriptador.EncryptData(codvendedor_invi), encriptador.EncryptData(rutcliente_invi), encriptador.EncryptData("8"));
                        e.Row.Cells[i].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[i].Text + " </a>";
                    }
                }
                e.Row.Cells[productos.Columns.Count].Text = sum_por_row.ToString("N0");

                G_PRODUCTOS.HeaderRow.Cells[3].Text = "VtaNo_Repite" + "(" + suma_no_repite.ToString("N0") + ")";
                G_PRODUCTOS.HeaderRow.Cells[3].Attributes["data-sort-method"] = "number";
            }

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



    }
}