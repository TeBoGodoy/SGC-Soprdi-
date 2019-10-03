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
    public partial class REPORTE_LOG_CORR_FICH : System.Web.UI.Page
    {
        public static int cont_periodos;
        public static DataTable aux;
        public static DataTable productos;
        public static DataTable totales;
        public static string PERIODOS;
        private static string USER;
        private static string where = " where 1=1 ";
        private static Page page;
        public static string GRUPOS;
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
                    if (u_ne.Trim() == "1" || u_ne.Trim() == "23")
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
                //////t = new DateTime(t.Year, t.Month - 6, 1);               
                txt_desde.Text = ReporteRNegocio.CALCULA_DESDE(t.Month, t.Year);
                txt_hasta.Text = t2.ToShortDateString();


                string grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

                if (grupos_del_usuario == "")
                {
                    grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
                }
                GRUPOS = grupos_del_usuario.Replace("'", "");

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
                btn_informe_Click(sender, ex);
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
            dtv.Sort = "user1";
            d_grupos_usuario.DataSource = dtv;
            d_grupos_usuario.DataTextField = "user1";
            d_grupos_usuario.DataValueField = "user1";
            d_grupos_usuario.DataBind();

        }


        [WebMethod(EnableSession = true)]
        public static int end_session()
        {
            page.Session.Abandon();
            return 0;
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


            string where = " where " +
                             "  FechaFactura <= CONVERT(datetime,'" + hasta + "',103) ";




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

            div_report.Visible = true;
            G_INFORME_TOTAL_VENDEDOR.Visible = true;
            G_INFORME_TOTAL_VENDEDOR.DataSource = ReporteRNegocio.trae_log_fich_();
            G_INFORME_TOTAL_VENDEDOR.DataBind();

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_INFORME_TOTAL_VENDEDOR')); </script>", false);


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
            e.Row.Cells[4].Attributes["style"] = "display:none";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                bool tiene = false;
                foreach (string q in GRUPOS.Split(','))
                {
                    if (e.Row.Cells[4].Text.Contains(q))
                    {
                        tiene = true;
                       
                    } 
                }
                if (tiene)
                {
                    e.Row.Cells[4].Attributes["style"] = "display:none";

                }
                else
                {
                    e.Row.Cells[4].Attributes["style"] = "display:none";
                    e.Row.Attributes["style"] = "display:none";

                }
                //string cliente = e.Row.Cells[0].Text;

                //clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                //string script1 = string.Format("javascript:fuera22(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(cliente), encriptador.EncryptData("88"));
                //e.Row.Cells[1].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[1].Text + " </a>";


                //string valor = e.Row.Cells[0].Text;
                //string rut_ini = valor.Trim().Substring(0, valor.Trim().Length - 1);
                //double rut = 0;
                //try { rut = double.Parse(rut_ini); e.Row.Cells[0].Text = rut.ToString("N0") + "-" + valor.Trim().Substring(valor.Trim().Length - 1); }
                //catch { rut = double.Parse(valor); e.Row.Cells[0].Text = rut.ToString("N0"); }


         

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
                where = " where FechaFactura >= CONVERT(datetime,'" + desde + "', 103)  and FechaFactura <= CONVERT(datetime,'" + hasta + "',103) ";
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

                div_productos.Visible = true;
                string periodos = ReporteRNegocio.listar_periodos_(" where FechaFactura >= CONVERT(datetime,'" + desde + "', 103)  and FechaFactura <= CONVERT(datetime,'" + hasta + "',103)");
                productos = ReporteRNegocio.listar_resumen_productos(periodos, where);
                PERIODOS = periodos;
                DataView view = new DataView(productos);
                DataTable distinctValues = view.ToTable(true, "NombreCliente");

                productos.Columns.Add("Total");

                int colum = productos.Columns.Count;

                DataView dv = productos.DefaultView;
                dv.Sort = "NombreCliente ASC";
                productos = dv.ToTable();

                Session["TaskTable"] = productos;

                header_sum = true;
                cont_det = 1;
                G_PRODUCTOS.DataSource = productos;
                //G_PRODUCTOS.Columns[colum - 1].Visible = false;
                //G_PRODUCTOS.Columns[colum - 2].Visible = false;
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

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_PRODUCTOS')); </script>", false);

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

            string grupos_del_usuario = "";

            grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

            if (grupos_del_usuario == "")
            {
                grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
            }


            DataTable dt; DataView dtv = new DataView();

            //cargar_combo_Grupo(dt, dtv);
            string es_vend = ReporteRNegocio.esvendedor(User.Identity.Name.ToString());
            if (es_vend != "2")
            {

                string where = " where ";
                where += " user1 in (" + grupos_del_usuario + ")";

                cargar_combo_VENDEDOR(ReporteRNegocio.listar_ALL_vendedores(where), dtv);
                cargar_combo_clientes(ReporteRNegocio.listar_ALL_cliente2(where), dtv);

            }
            else
            {
                string where = " where ";
                where += " user1 in (" + grupos_del_usuario + ")";

                cargar_combo_VENDEDOR(ReporteRNegocio.listar_ALL_vendedores(where + " and codvendedor = '" + USER + "' "), dtv);
                cargar_combo_clientes(ReporteRNegocio.listar_ALL_cliente2(where + " and codvendedor = '" + USER + "' "), dtv);
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