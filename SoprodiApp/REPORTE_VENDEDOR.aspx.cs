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

namespace SoprodiApp
{
    //testadsadsassdfdfsdsfsdf

    public partial class REPORTE_VENDEDOR : System.Web.UI.Page
    {
        public static int cont_periodos;
        public static DataTable aux;
        public static DataTable productos;
        public static DataTable totales;
        public static string PERIODOS;
        private static string USER;
        private static string where = " where 1=1 ";
        private static Page page;
        public static bool busca_columna_fac;
        public static bool header_sum = true;
        public static bool header_sum2 = true;
        private static string vendedor;
        private static string cliente;
        public static int cont_det;

        private static string rutcliente_glob;
        public static bool permiso;


        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.RegisterRedirectOnSessionEndScript();
            JQ_Datatable();
            page = this.Page;
            if (!IsPostBack)
            {
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "2" || u_ne.Trim() == "7")
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


        public class PRODUCTOS
        {
            public string producto { get; set; }
            public string descproducto { get; set; }

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
        public static List<PRODUCTOS> PRODUCTO_POR_(string vendedor, string desde, string hasta, string usuario, string grupos, string clientes)
        {
            DataTable dt = new DataTable();
            string vende = agregra_comillas2(vendedor);
            string grupos_ = agregra_comillas2(grupos);
            string clientes_ = agregra_comillas2(clientes);

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

            if (!clientes_.Contains("'-1'") && clientes_ != "" && !clientes_.Contains("'-- Todos --'"))
            {
                where += " and rutcliente in (" + clientes_ + ")";
            }
            if (vendedor == "")
            {
                where += " and 1=1 ";
            }
            //else
            //{
            try
            {
                dt = ReporteRNegocio.listar_ALL_productos_thx(where);
                ////dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
                DataView dv = dt.DefaultView;
                dv.Sort = "descproducto";
                dt = dv.ToTable();
            }
            catch { }
            //}
            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new PRODUCTOS
                        {
                            producto = Convert.ToString(item["producto"]),
                            descproducto = Convert.ToString(item["descproducto"])
                        };
            return query.ToList<PRODUCTOS>();
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
            DateTime t = DateTime.Now;
            string fecha_ahora = t.ToShortDateString();
            ReporteRNegocio.GUARDA_BOTON(1, USER.ToUpper(), fecha_ahora);


            //if (l_vendedores.Text != "" && l_clientes.Text != "" && txt_desde.Text != "" && txt_hasta.Text != "" && l_grupos.Text != "")
            //{
            string vendedores = agregra_comillas(l_vendedores.Text);
            string clientes = agregra_comillas(l_clientes.Text);
            string desde = txt_desde.Text;
            string hasta = txt_hasta.Text;
            string grupos = agregra_comillas(l_grupos.Text); ;
            string productos = agregra_comillas(l_productos_2.Text); ;
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
                string where_2 = " where 1=1 ";
                string where = " where FechaFactura >= CONVERT(datetime,'" + desde + "', 103)  and FechaFactura <= CONVERT(datetime,'" + hasta + "',103) ";
                if (grupos != "")
                {
                    where_2 = where_2 + " and user1 in (" + grupos + ") ";
                    where = where + " and user1 in (" + grupos + ") ";
                }
                else if (es_vendedor != "2")
                {
                    where = where + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario(USER)) + ")";
                    where_2 = where_2 + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario(USER)) + ")";

                }
                else if (es_vendedor == "2")
                {
                    where_2 = where_2 + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(USER)) + ")";
                    where = where + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(USER)) + ")";
                }

                if (clientes != "")
                {
                    where_2 = where_2 + " and rutcliente in (" + clientes + ") ";

                    where = where + " and rutcliente in (" + clientes + ") ";
                }
                if (productos != "")
                {
                    where_2 = where_2 + " and producto in (" + productos + ") ";

                    where = where + " and producto in (" + productos + ") ";
                }
                if (vendedores != "")
                {
                    where_2 = where_2 + " and codvendedor in (" + vendedores + ") ";
                    where = where + " and codvendedor in (" + vendedores + ") ";
                }
                if (es_vendedor == "2")
                {
                    where_2 += " and codvendedor in ('" + USER + "')";

                    where += " and codvendedor in ('" + USER + "')";
                }
                div_report.Visible = true;
                cont_periodos = 0;
                G_INFORME_VENDEDOR.Visible = false;
                G_INFORME_TOTAL_VENDEDOR.Visible = true;
                string periodos = ReporteRNegocio.listar_periodos_(where);
                //aux = ReporteRNegocio.listar_resumen_periodo(where);
                PERIODOS = periodos;
                totales = new DataTable();

                List<string> periodos_list = periodos.Split(',').ToList();

                totales.Columns.Add("FACTORES/Periodos");
                int colum = periodos_list.Count;

                List<DateTime> fecha_ayer = new List<DateTime>();
                //string dia = DateTime.Now.Day.ToString();
                //string dia = DateTime.Now.Day.ToString();
                DateTime t5 = Convert.ToDateTime(hasta);
                //Date hoy = new Date();
                //Date ayer = new Date(hoy.getTime() - 86400000);


                fecha_ayer.Add(t5.AddDays(-1));
                int uno_mas = 1;
                foreach (string r in periodos_list)
                {
                    if (r != "0")
                    {
                        totales.Columns.Add(r);
                    }

                    DateTime aux = t5.AddDays(-1);
                    aux = aux.AddMonths(-uno_mas);
                    fecha_ayer.Add(aux);
                    uno_mas++;

                }
                DataRow row;
                List<string> nombre_factores = get_factores();

                //CAPI
                //totales.Columns.Add("Total general");

                //RECORRE TABLA "TOTALES" PARA AGREGAR DATOS
                string where_años = where_2;
                for (int i = 0; i <= 20; i++)
                {
                    row = totales.NewRow();
                    if (nombre_factores[i].ToString().Contains("Percentil "))
                    {
                        row["FACTORES/Periodos"] = nombre_factores[i] + t_percentil.Text + "%";
                    }
                    else { row["FACTORES/Periodos"] = nombre_factores[i]; }
                    totales.Rows.Add(row);
                    double sum = 0;
                    for (int y = 0; y < colum; y++)
                    {
                        if (i == 0)
                        {
                            if (y == colum)
                            {
                                //Facturación ultimo día
                            }
                            else
                            {
                                string periodo = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio.Facturación__ayer(where, fecha_ayer[y].ToShortDateString()).ToString("N0");
                            }
                        }

                        if (i == 1)
                        {

                            if (y == colum)
                            {
                                //# cltes ultimo día
                            }
                            else
                            {
                                string periodo = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio.cant_clientes__ayer(where, fecha_ayer[y].ToShortDateString()).ToString("N0");
                            }
                        }

                        if (i == 2)
                        {

                            if (y == colum)
                            {
                                //Facturación Mes(Peso)
                            }
                            else
                            {
                                string periodo = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio.Facturación_Mes(periodo, where).ToString("N0");
                            }
                        }

                        if (i == 3)
                        {

                            if (y == colum)
                            {
                                //Facturación Mes(Dolar)
                            }
                            else
                            {
                                string periodo = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio.Facturación_Mes_dolar(periodo, where).ToString("N0");
                            }
                        }


                        if (i == 4)
                        {
                            if (y <= colum)
                            {
                                try
                                {
                                    //Acumulado mes anterior
                                    string dia_anterior = DateTime.Now.ToShortDateString().Substring(0, 2) + "/";
                                    string mes_anterior = totales.Columns[y + 2].ColumnName.ToString().Substring(4) + "/";
                                    string año_anterior = totales.Columns[y + 2].ColumnName.ToString().Substring(0, 4);
                                    DateTime aux = new DateTime();
                                    try
                                    {
                                        aux = new DateTime(Convert.ToInt32(año_anterior), Convert.ToInt32(mes_anterior.Replace("/", "")), Convert.ToInt32(dia_anterior.Replace("/", "")));
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            aux = new DateTime(Convert.ToInt32(año_anterior), Convert.ToInt32(mes_anterior.Replace("/", "")), Convert.ToInt32(dia_anterior.Replace("/", "")) - 1);
                                        }
                                        catch
                                        {

                                            try
                                            {
                                                aux = new DateTime(Convert.ToInt32(año_anterior), Convert.ToInt32(mes_anterior.Replace("/", "")), Convert.ToInt32(dia_anterior.Replace("/", "")) - 2);
                                            }
                                            catch
                                            {
                                                try
                                                {
                                                    aux = new DateTime(Convert.ToInt32(año_anterior), Convert.ToInt32(mes_anterior.Replace("/", "")), Convert.ToInt32(dia_anterior.Replace("/", "")) - 3);
                                                }
                                                catch
                                                {


                                                }

                                            }
                                        }
                                    }
                                    string where_aux_acum = " and FechaFactura <= CONVERT(datetime,'" + aux.Day + "/" + aux.Month + "/" + aux.Year + "',103) ";
                                    string factura = ReporteRNegocio.Facturación_Mes(totales.Columns[y + 2].ColumnName.ToString(), where_2 + where_aux_acum).ToString("N0");
                                    totales.Rows[i][y + 1] = factura;
                                }
                                catch { }
                            }
                        }

                        if (i == 5)
                        {

                            if (y == colum)
                            {
                                //# cltes con vta
                            }
                            else
                            {
                                string periodo = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio._cltes_con_vta(periodo, where).ToString("N0");
                            }

                        }
                        if (i == 6)
                        {
                            //Promedio x clte
                            totales.Rows[i][y + 1] = (+(Double.Parse(totales.Rows[2][y + 1].ToString().Replace(".", ""))) / (Double.Parse(totales.Rows[5][y + 1].ToString().Replace(".", "")))).ToString("N0");
                        }



                        if (i == 7)
                        {
                            if (y == colum)
                            {
                                // Percentil 50%
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

                        if (i == 8)
                        {

                            //# cltes sobre este percentil

                            if (y == colum)
                            {
                                // CAPI
                                //totales.Rows[i][y + 1] = ReporteRNegocio.cltes_sobre_este_percentil(agregra_comillas(periodos), where, Double.Parse(totales.Rows[3][y + 1].ToString().Replace(".", ""))).ToString("N0");
                            }
                            else
                            {
                                string periodo = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio.cltes_sobre_este_percentil(periodo, where, Double.Parse(totales.Rows[7][y + 1].ToString().Replace(".", ""))).ToString("N0");
                            }
                        }

                        if (i == 9)
                        {
                            //Venta sobre este percentil


                            if (y == colum)
                            {
                                // CAPI
                                //totales.Rows[i][y + 1] = ReporteRNegocio.sum_sobre_este_percentil(agregra_comillas(periodos), where, Double.Parse(totales.Rows[3][y + 1].ToString().Replace(".", ""))).ToString("N0");
                            }
                            else
                            {
                                string periodo = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio.sum_sobre_este_percentil(periodo, where, Double.Parse(totales.Rows[7][y + 1].ToString().Replace(".", ""))).ToString("N0");
                            }
                        }
                        if (i == 10)
                        {

                            //% c/r Vta Total

                            Double sumatoria = (Double.Parse(totales.Rows[2][y + 1].ToString().Replace(".", "")));
                            Double sum_vent_sobre_percen = (Double.Parse(totales.Rows[9][y + 1].ToString().Replace(".", "")));
                            totales.Rows[i][y + 1] = Math.Round((sum_vent_sobre_percen / sumatoria * 100)).ToString() + " %";
                        }
                        if (i == 11)
                        {

                            //#Cltes que NO Repiten r/mes anterior
                            if (y <= colum - 2)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = totales.Columns[y + 2].ColumnName;
                                totales.Rows[i][y + 1] = (ReporteRNegocio.cont_si_repite(periodo_apreguntar, periodo_anterior, where)).ToString("N0");
                            }
                        }
                        if (i == 12)
                        {
                            //% Cltes que NO repiten r/mes anterior
                            if (y <= colum - 2)
                            {
                                Double cont_si_repite = (Double.Parse(totales.Rows[11][y + 1].ToString().Replace(".", "")));
                                Double clte_con_vta_anterior = (Double.Parse(totales.Rows[5][y + 2].ToString().Replace(".", "")));
                                totales.Rows[i][y + 1] = Math.Round((cont_si_repite / clte_con_vta_anterior * 100)).ToString() + " %";
                            }
                        }
                        if (i == 13)
                        {
                            //Venta a recuperar mes anterior
                            if (y <= colum - 2)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = totales.Columns[y + 2].ColumnName;
                                totales.Rows[i][y + 1] = (ReporteRNegocio.sum_a_recuperar(periodo_apreguntar, periodo_anterior, where)).ToString("N0");
                            }
                        }
                        if (i == 14)
                        {
                            //Vtas $ de cltes que repiten c/r mes anterior
                            if (y <= colum - 2)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = totales.Columns[y + 2].ColumnName;
                                totales.Rows[i][y + 1] = (ReporteRNegocio.sum_si_repite(periodo_apreguntar, periodo_anterior, where)).ToString("N0");
                            }
                        }
                        if (i == 15)
                        {

                            //Vtas $ de cltes del mes anterior que repiten este mes

                            if (y <= colum - 2)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = totales.Columns[y + 2].ColumnName;
                                totales.Rows[i][y + 1] = (ReporteRNegocio.sum_si_repite_actual(periodo_apreguntar, periodo_anterior, where)).ToString("N0");
                            }
                        }


                        if (i == 16)
                        {

                            //Promedio de Productos por Factura
                            if (y == colum)
                            {
                            }
                            else
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio.prom_productos_por_factura(periodo_apreguntar, where);
                            }
                        }


                        if (i == 17)
                        {

                            //Concentración Venta
                            if (y == colum)
                            {
                            }
                            else
                            {

                                Double perc = Double.Parse(totales.Rows[7][y + 1].ToString().Replace(".", ""));
                                Double prom_cl = Double.Parse(totales.Rows[6][y + 1].ToString().Replace(".", ""));
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = Math.Round((prom_cl / perc), 3);
                            }
                        }
                        if (i == 18)
                        {

                            //MES ACUMLADO AÑO ANTERIOR
                            if (y == colum)
                            {
                            }
                            else
                            {


                                //Acumulado mes anterior
                                string dia_anterior = DateTime.Now.ToShortDateString().Substring(0, 2) + "/";
                                string mes_anterior = totales.Columns[y + 1].ColumnName.ToString().Substring(4) + "/";
                                string año_anterior = (Convert.ToInt32(totales.Columns[y + 1].ColumnName.ToString().Substring(0, 4)) - 1).ToString();
                                DateTime aux = new DateTime();
                                try
                                {
                                    aux = new DateTime(Convert.ToInt32(año_anterior), Convert.ToInt32(mes_anterior.Replace("/", "")), Convert.ToInt32(dia_anterior.Replace("/", "")));
                                }
                                catch
                                {
                                    try
                                    {
                                        aux = new DateTime(Convert.ToInt32(año_anterior), Convert.ToInt32(mes_anterior.Replace("/", "")), Convert.ToInt32(dia_anterior.Replace("/", "")) - 1);
                                    }
                                    catch
                                    {

                                        try
                                        {
                                            aux = new DateTime(Convert.ToInt32(año_anterior), Convert.ToInt32(mes_anterior.Replace("/", "")), Convert.ToInt32(dia_anterior.Replace("/", "")) - 2);
                                        }
                                        catch
                                        {
                                            try
                                            {
                                                aux = new DateTime(Convert.ToInt32(año_anterior), Convert.ToInt32(mes_anterior.Replace("/", "")), Convert.ToInt32(dia_anterior.Replace("/", "")) - 3);
                                            }
                                            catch
                                            {


                                            }

                                        }
                                    }
                                }
                                bool unavez = true;
                                string where_aux_año = "";
                                if (unavez)
                                {

                                    where_aux_año = " and FechaFactura <= CONVERT(datetime,'" + aux.Day + "/" + aux.Month + "/" + aux.Year + "',103) ";
                                    unavez = false;
                                    where_aux_año = "";
                                }
                                string año_anterior2 = (Convert.ToInt32(totales.Columns[y + 1].ColumnName.ToString().Substring(0, 4)) - 1).ToString();
                                string mes = totales.Columns[y + 1].ColumnName.ToString().Substring(4);

                                string factura = ReporteRNegocio.Facturación_Mes(año_anterior2 + mes, where_años + where_aux_año).ToString("N0");
                                totales.Rows[i][y + 1] = factura;
                            }
                        }


                        if (i == 19)
                        {

                            //"# cltes nuevos

                            if (y == colum)
                            {
                            }
                            else
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string clte_nuevo1 = Math.Round(ReporteRNegocio._cltes_nuevos_12_mes(periodo_apreguntar, where), 3).ToString("N0");
                                totales.Rows[i][y + 1] = clte_nuevo1;
                            }
                        }


                        //
                        //                string clte_nuevo1 = Math.Round(ReporteRNegocio._cltes_nuevos_12_mes(periodos_list[0].ToString(), where),3).ToString("N0"); 
                        //string clte_nuevo_factur1 = Math.Round(ReporteRNegocio._cltes_nuevos_12_mes_factu(periodos_list[0].ToString(), where), 3).ToString("N0");

                        if (i == 20)
                        {

                            //Total facturación con cltes nuevos

                            if (y == colum)
                            {
                            }
                            else
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string clte_nuevo1 = Math.Round(ReporteRNegocio._cltes_nuevos_12_mes_factu(periodo_apreguntar, where), 3).ToString("N0");
                                totales.Rows[i][y + 1] = clte_nuevo1;
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

            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_PRODUCTOS')); </script>", false);
            //}

        }

        private List<string> get_factores()
        {
            List<string> aux = new List<string>();


            aux.Add("Facturación ultimo día");
            aux.Add("# cltes ultimo día");

            aux.Add("Facturación Mes(Peso)");
            aux.Add("Facturación Mes(Dolar)");
            aux.Add("Acumulado mes anterior");
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
            //aux.Add("Var% de la vta en cltes que repiten r/mes ant.");
            aux.Add("Promedio de Productos por Factura");
            aux.Add("Concentración Venta");
            aux.Add("Acumulado mes Año anterior");

            aux.Add("# cltes nuevos");
            aux.Add("Total facturación con cltes nuevos");


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
            string productos22 = agregra_comillas(l_productos_2.Text); ;

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


                if (productos22 != "")
                {
                    where = where + " and producto in (" + productos22 + ") ";
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
                //if (productos22 != "")
                //{
                //    where2 += " and producto in (" + productos22 + ") ";
                //}
                //if (vendedores != "")
                //{
                //    where2 += " and codvendedor in (" + vendedores + ") ";
                //}

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
                //if (productos22 != "")
                //{
                //    where2 += " and producto in (" + productos22 + ") ";
                //}
                //if (vendedores != "")
                //{
                //    where2 += " and codvendedor in (" + vendedores + ") ";
                //}

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

                //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_PRODUCTOS')); </script>", false);

            }
        }

        protected void G_PRODUCTOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                double sum_por_row = 0;
                string vendedor = G_PRODUCTOS.DataKeys[e.Row.RowIndex].Values[2].ToString();
                string cliente = G_PRODUCTOS.DataKeys[e.Row.RowIndex].Values[3].ToString();


                string boton_sp = "<div class='btn-group'> <a href='REPORTE_SP.aspx?G=912&C=" + cliente + "*" + vendedor + "' target='_blank'><i class='fa fa-search fa-2x' aria-hidden='true'></i>SP</a> </div>";


                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                e.Row.Cells[2].Attributes["onclick"] = "javascript:fuera('" + encriptador.EncryptData(PERIODOS) + "', '" + encriptador.EncryptData(vendedor) + "', '" + encriptador.EncryptData(cliente) + "', '" + encriptador.EncryptData("4") + "');return false;";
                //e.Row.Cells[3].Attributes["onclick"] = "javascript:fuera22('" + encriptador.EncryptData(cliente) + "', '" + encriptador.EncryptData("88") + "');return false;";


                string script1 = string.Format("javascript:fuera22(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(cliente), encriptador.EncryptData("88"));
                e.Row.Cells[6].Text = "<a href='REPORTE_SP.aspx?G=912&C=" + cliente.Trim().Substring(0, cliente.Trim().Length - 1) + "*" + vendedor + "' target='_blank'><i class='fa fa-search fa-2x' aria-hidden='true'></i></a>  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[6].Text + " </a>";


                e.Row.Cells[1].Text = cont_det.ToString();
                cont_det++;
                int colum = productos.Columns.Count;

                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                G_PRODUCTOS.HeaderRow.Cells[3].Visible = false;
                G_PRODUCTOS.HeaderRow.Cells[4].Visible = false;
                for (int i = 5; i <= colum; i++)
                {
                    G_PRODUCTOS.HeaderRow.Cells[i + 2].Attributes["data-sort-method"] = "number";
                    if (e.Row.Cells[i + 2].Text != "0")
                    {
                        double d;
                        double.TryParse(e.Row.Cells[i + 2].Text, out d);
                        string aux = "";
                        if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                        e.Row.Cells[i + 2].Text = aux;
                        sum_por_row += d;
                        if (i != colum)
                        {
                            string script = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;);return false;", encriptador.EncryptData(G_PRODUCTOS.HeaderRow.Cells[i + 2].Text.Substring(0, 6)), encriptador.EncryptData(e.Row.Cells[3].Text), encriptador.EncryptData(e.Row.Cells[4].Text), encriptador.EncryptData("8"));
                            //e.Row.Cells[i + 2].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[i + 2].Text + " </a>";


                            e.Row.Cells[i + 2].Attributes["href"] = "'javascript:'";
                            e.Row.Cells[i + 2].Attributes["onclick"] = "javascript:fuera('" + encriptador.EncryptData(G_PRODUCTOS.HeaderRow.Cells[i + 2].Text.Substring(0, 6)) + "', '" + encriptador.EncryptData(e.Row.Cells[3].Text) + "', '" + encriptador.EncryptData(e.Row.Cells[4].Text) + "', '" + encriptador.EncryptData("8") + "');return false;";
                        }
                    }
                }
                e.Row.Cells[colum + 2].Text = sum_por_row.ToString("N0");

                if (header_sum)
                {
                    int colum2 = productos.Columns.Count;
                    for (int i = 5; i <= colum2; i++)
                    {
                        try
                        {
                            G_PRODUCTOS.HeaderRow.Cells[i + 2].Text = G_PRODUCTOS.HeaderRow.Cells[i + 2].Text + "  (" + Convert.ToDouble(ReporteRNegocio.Facturación_Mes(G_PRODUCTOS.HeaderRow.Cells[i + 2].Text, where)).ToString("N0") + ")";

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
                    cargar_combo_productos(ReporteRNegocio.listar_ALL_productos_thx(where), dtv);
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

        private void cargar_combo_productos(DataTable dataTable, DataView dtv)
        {
            //dt.Rows.Add(new Object[] { -1, "-- Todos --" });
            dtv = dataTable.DefaultView;
            dtv.Sort = "descproducto";
            d_productos.DataSource = dtv;
            d_productos.DataTextField = "descproducto";
            d_productos.DataValueField = "producto";
            //d_vendedor_.SelectedIndex = -1;
            d_productos.DataBind();
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

        protected void btn_enviar_correos_Click(object sender, EventArgs e)
        {


            bool entro1 = false;
            List<string> folios = new List<string>();
            foreach (GridViewRow dtgItem in this.G_PRODUCTOS.Rows)
            {
                CheckBox Sel = ((CheckBox)G_PRODUCTOS.Rows[dtgItem.RowIndex].FindControl("chkAccept"));
                bool valor = Sel.Checked;
                if (valor)
                {
                    entro1 = true;
                }
                else
                {
                }
            }
            if (!entro1)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeeqqqe", "<script language='javascript'>alert('Debe seleccionar clientes');</script>", false);
            }
            else
            {
                //panel_enviar.Visible = true;
                //btn_enviar_correos.Visible = false;
            }


        }

        private void correo_de_cliente(string rutcliente)
        {

            //rescata datos del cliente
            string grupos_del_usuario = "";

            grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

            if (grupos_del_usuario == "")
            {
                grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
            }

            string es_vende = ReporteRNegocio.esvendedor(User.Identity.Name);
            //GRUPOS_USUARIO = grupos_del_usuario;



            rutcliente = rutcliente.Replace("-", "").Replace(".", "").Trim();
            string rut_ini = rutcliente.Trim().Substring(0, rutcliente.Trim().Length - 1);
            double rut = double.Parse(rut_ini);

            rutcliente = rut.ToString("N0") + "-" + rutcliente.Trim().Substring(rutcliente.Trim().Length - 1);
            rutcliente_glob = rutcliente;
            string nombrecliente = "";
            string direccion = "";
            string ciudad = "";
            string pais = "";
            string fono = "";
            string correo = "";
            string vendedor = "";
            DataTable dato_clientes = ReporteRNegocio.datos_cliente(rutcliente.Trim().Replace("-", "").Replace(".", ""));
            foreach (DataRow r in dato_clientes.Rows)
            {
                nombrecliente = r[0].ToString();
                direccion = r[2].ToString();
                ciudad = r[3].ToString();
                pais = r[4].ToString();
                fono = r[5].ToString();
                correo = r[9].ToString();
                vendedor = r[7].ToString();
            }

            string nombre_vendedor = ReporteRNegocio.nombre_vendedor(vendedor);
            string ciudad_pais = ciudad + ", " + pais;

            string cred = ReporteRNegocio.linea_credito(rutcliente.Replace("-", "").Replace(".", ""));

            string lc = cred.Substring(0, cred.IndexOf("("));
            string ld = cred.Substring(cred.IndexOf(" ") + 1).Replace(" )", "");

            double d;
            double.TryParse(lc, out d);
            string aux = "";
            if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
            lc = aux;
            double d2;
            double.TryParse(ld, out d2);
            string aux2 = "";
            if (d2 == 0) { aux2 = ""; } else { aux2 = d2.ToString("N0"); }
            ld = aux2;
            string credito_mas_dispon = lc + "(D: " + ld + " )";

            //correo

            DataTable cr = ReporteRNegocio.corr_usuario(User.Identity.Name);
            foreach (DataRow r in cr.Rows)
            {
                //tx_enviar_.Text = r[1].ToString();
            }

            busca_columna_fac = true;
            g_doc.Visible = true;
            g_doc.DataSource = ReporteRNegocio.docu_abier(rutcliente_glob.Replace(".", "").Replace("-", ""), vendedor);
            g_doc.DataBind();


            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);
            g_doc.Visible = true;
            g_doc.RenderControl(htmlWrite);

            string body = stringWrite.ToString();

            body = body.Replace("<tr class=\"test no-sort\">", "<tr style='background-color:#428bca;color:#fff'>");

            System.IO.StringWriter stringWrite2 = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite2 =
            new HtmlTextWriter(stringWrite2);

            G_CRUZADO.Visible = true;

            //string es_vende = ReporteRNegocio.esvendedor(User.Identity.Name);
            DateTime t = DateTime.Now;
            DateTime t2 = DateTime.Now;
            string desde = ReporteRNegocio.CALCULA_DESDE(t.Month, t.Year);
            string hasta = t2.ToShortDateString();

            string where = " where rutcliente like '%" + rutcliente.Replace(".", "").Replace("-", "") + "%' and FechaFactura >= CONVERT(datetime,'" + desde + "', 103)  and FechaFactura <= CONVERT(datetime,'" + hasta + "',103)";
            if (vendedor != "") { where += " and codvendedor like '%" + vendedor + "%' "; }

            //if (es_vende == "2") { where += " and codvendedor like '%" + User.Identity.Name + "%' "; }

            DataTable cruz = ReporteRNegocio.listar_resumen_productos_ficha(where);
            cruz.Columns.Add("Total");
            header_sum2 = true;
            G_CRUZADO.DataSource = cruz;
            G_CRUZADO.DataBind();
            G_CRUZADO.RenderControl(htmlWrite2);

            string body2 = stringWrite2.ToString();

            body2 = body2.Replace("<tr class=\"test no-sort\">", "<tr style='background-color:#428bca;color:#fff'>");

            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(tx_destinos.Value));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "Información del Cliente (" + nombrecliente + " ) ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

            string correo_vendedor_por_cliente = ReporteRNegocio.trae_corr_vend_por_cliente(rutcliente.Replace("-", "").Replace(".", ""));
            if (correo_vendedor_por_cliente != "")
            {
                email.CC.Add(correo_vendedor_por_cliente);
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

            email.Body += "<div> Estimado :<br> <br> Productos vendidos dentro de periodos y histórico de documentos cobranzas del cliente:  <b>" + nombrecliente + "(" + rutcliente + ") </b> <br><br>";

            email.Body += "<div>" + datos_clientes(nombre_vendedor, direccion, ciudad_pais, rutcliente, credito_mas_dispon, fono) + "</div> <br><br>";

            email.Body += "<div>" + body2.Replace(",", ".") + "</div> <br><br>";

            email.Body += "<div>" + body.Replace(",", ".") + "</div> ";

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
                string MAIL_USER = "";
                DataTable cr1 = ReporteRNegocio.corr_usuario(User.Identity.Name);
                foreach (DataRow r in cr.Rows)
                {
                    MAIL_USER = r[1].ToString();
                }

                smtp.Send(email);
                email.Dispose();
                lb_mensaj.Text = "Correos Enviados!";
                string user = User.Identity.Name;
                string mail_usuario = MAIL_USER;
                string fecha_now = DateTime.Now.ToString();
                string destinos = tx_destinos.Value;

                ReporteRNegocio.insert_log_enviar_ficha(user, mail_usuario, fecha_now, destinos, rutcliente.Replace("-", "").Replace(".", ""));
                ///ACA INSERT LOG
            }
            catch (Exception ex)
            {
                lb_mensaj.Text = "Error al enviar ";
            }
            g_doc.Visible = false;
            G_CRUZADO.Visible = false;
            //panel_enviar.Visible = false;
            //btn_enviar_correos.Visible = true;
        }


        private string datos_clientes(string nombre_vendedor, string direccion, string ciudad_pais, string rutcliente, string credito_mas_dispon, string fono)
        {
            string tabla = "";

            tabla += "<table border='1' class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr style=\"background-color: #428bca;  color: #fff;\">";
            tabla += "<th>Vendedor</th>";
            tabla += "<th>Dirección</th>";
            tabla += "<th>Ciudad/País</th>";
            tabla += "<th>Fono</th>";
            tabla += "<th>L.Crédito(Disponible)</th>";
            tabla += "<th>Tipo Crédito</th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";

            tabla += "<tr>";
            tabla += "<td>" + nombre_vendedor + "</td>";
            tabla += "<td>" + direccion + "</td>";
            tabla += "<td>" + ciudad_pais + "</td>";
            tabla += "<td>" + fono + "</td>";
            tabla += "<td>" + credito_mas_dispon + "</td>";
            tabla += "<td>" + ReporteRNegocio.trae_letra_credito(rutcliente.Replace(".", "").Replace("-", "")).Trim() + "</td>";
            tabla += "</tr>";

            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");
            return tabla;

        }

        protected void G_CRUZADO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = cont_det.ToString();
                cont_det++;
                double sum_por_row = 0;
                for (int i = 2; i <= e.Row.Cells.Count - 2; i++)
                {

                    string where_es = " where rutcliente like '%" + rutcliente_glob.Replace("-", "").Replace(".", "") + "%'";
                    if (ReporteRNegocio.esvendedor(User.Identity.Name) == "2") { where_es += " and codvendedor like '%" + User.Identity.Name + "%' "; }
                    if (header_sum2)
                    {
                        G_CRUZADO.HeaderRow.Cells[1].Attributes["data-sort-method"] = "number";
                        G_CRUZADO.HeaderRow.Cells[i].Text = G_CRUZADO.HeaderRow.Cells[i].Text + "  (" + Convert.ToDouble(ReporteRNegocio.Facturación_Mes(G_CRUZADO.HeaderRow.Cells[i].Text.Substring(0, 6), where_es)).ToString("N0") + ")";
                    }
                    if (i == e.Row.Cells.Count - 2) { header_sum2 = false; }


                    G_CRUZADO.HeaderRow.Cells[i].Attributes["data-sort-method"] = "number";
                    bool sw_es_menos = false;
                    if (e.Row.Cells[i].Text != "&nbsp;")
                    {
                        for (int x = i + 1; x <= e.Row.Cells.Count - 2; x++)
                        {

                            if (e.Row.Cells[x].Text != "&nbsp;")
                            {
                                if (Convert.ToDouble(e.Row.Cells[i].Text) < Convert.ToDouble(e.Row.Cells[x].Text))
                                {
                                    sw_es_menos = true;
                                }
                                else
                                {
                                    sw_es_menos = false;
                                }
                                break;
                                //&nbsp;
                            }
                        }
                    }
                    if (e.Row.Cells[i].Text != "0")
                    {
                        double d;
                        double.TryParse(e.Row.Cells[i].Text, out d);
                        string aux = "";
                        if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                        e.Row.Cells[i].Text = aux;
                        sum_por_row += d;
                    }
                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                    string script2 = string.Format("javascript:fuera1(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;)", encriptador.EncryptData(G_CRUZADO.HeaderRow.Cells[i].Text), encriptador.EncryptData(ReporteRNegocio.cod_producto(e.Row.Cells[1].Text)), encriptador.EncryptData(rutcliente_glob.Replace(".", "").Replace("-", "")), encriptador.EncryptData("7"));

                    if (sw_es_menos)
                    {
                        e.Row.Cells[i].Text = "  <a style='color:red;' href='javascript:' onclick='" + script2 + "'>" + e.Row.Cells[i].Text + " </a>";
                    }
                    else
                    {
                        e.Row.Cells[i].Text = "  <a href='javascript:' onclick='" + script2 + "'>" + e.Row.Cells[i].Text + " </a>";

                    }
                }
                e.Row.Cells[e.Row.Cells.Count - 1].Text = sum_por_row.ToString("N0");
                G_CRUZADO.HeaderRow.Cells[e.Row.Cells.Count - 1].CssClass = "sort-default sort-header sort-down";
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

        protected void btn_enviar_Click(object sender, EventArgs e)
        {
            bool entro1 = false;
            string rutcliente = "";
            List<string> folios = new List<string>();
            foreach (GridViewRow dtgItem in this.G_PRODUCTOS.Rows)
            {
                CheckBox Sel = ((CheckBox)G_PRODUCTOS.Rows[dtgItem.RowIndex].FindControl("chkAccept"));
                bool valor = Sel.Checked;
                if (valor)
                {

                    rutcliente = G_PRODUCTOS.Rows[dtgItem.RowIndex].Cells[4].Text;
                    ((CheckBox)G_PRODUCTOS.Rows[dtgItem.RowIndex].FindControl("chkAccept")).Checked = false;
                    entro1 = true;
                    correo_de_cliente(rutcliente);

                }
                else
                {
                }
            }
            if (!entro1)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeeqqqe", "<script language='javascript'>alert('Debe seleccionar clientes');</script>", false);
            }
        }

        protected void btn_cerrar_Click(object sender, EventArgs e)
        {
            //panel_enviar.Visible = false;
            //btn_enviar_correos.Visible = true;
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_PRODUCTOS')); </script>", false);

        }
    }
}