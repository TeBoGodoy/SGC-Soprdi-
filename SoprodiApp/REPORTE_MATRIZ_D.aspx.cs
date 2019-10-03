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
    public partial class REPORTE_MATRIZ_D : System.Web.UI.Page
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
                t = new DateTime(t.Year, t.Month, 1);
                txt_desde.Text = t.ToShortDateString();
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



                string grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

                if (grupos_del_usuario == "")
                {
                    grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
                }

                List<string> grupos = grupos_del_usuario.Split(',').ToList();


                string clase = " ";

                if (grupos.Count == 4)
                {


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
        public static List<CLIENTE> CLIENTE_POR_BODEGA(string bodegas, string grupos, string desde, string hasta, string usuario)
        {
            DataTable dt = new DataTable();
            string bodegas_ = agregra_comillas2(bodegas);
            string grupos_ = agregra_comillas2(grupos);

            string es_vendedor = ReporteRNegocio.esvendedor(usuario);


            string where = " where FechaFactura >= CONVERT(datetime,'" + desde + "', 103) " +
                             " and FechaFactura <= CONVERT(datetime,'" + hasta + "',103) ";




            if (es_vendedor == "2")
            {
                where += " and codvendedor in ('" + usuario + "')";
            }

            if (!bodegas_.Contains("'-1'") && bodegas_ != "" && !bodegas_.Contains("'-- Todos --'"))
            {
                where += " and bodega in (" + bodegas_ + ")";
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

        [WebMethod]
        public static List<VENDEDOR> VENDEDOR_POR_BODEGA(string bodegas, string grupos, string desde, string hasta, string usuario)
        {
            DataTable dt = new DataTable();
            string grupos_ = agregra_comillas2(grupos);
            string bodegas_ = agregra_comillas2(bodegas);


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
            if (bodegas_ != "")

            {

                where += " and bodega in (" + bodegas_ + ")";
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



        [WebMethod]
        public static List<VENDEDOR> BODEGA_POR_GRUPO(string grupos, string desde, string hasta, string usuario)
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
                dt = ReporteRNegocio.carga_bodega(where);
                ////dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
                DataView dv = dt.DefaultView;
                dv.Sort = "nom_bodega";
                dt = dv.ToTable();
            }
            catch { }


            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new VENDEDOR
                        {
                            cod_vendedor = Convert.ToString(item["nom_bodega"]),
                            nom_vendedor = Convert.ToString(item["nom_bodega"])
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
                string periodos = ReporteRNegocio.listar_periodos_(" where FechaFactura >= CONVERT(datetime,'" + desde + "', 103)  and FechaFactura <= CONVERT(datetime,'" + hasta + "',103)");
                //aux = ReporteRNegocio.listar_resumen_periodo(where);
                PERIODOS = periodos;
                totales = new DataTable();

                List<string> periodos_list = periodos.Split(',').ToList();

                totales.Columns.Add("FACTORES/Periodos");
                int colum = periodos_list.Count;

                List<DateTime> fecha_ayer = new List<DateTime>();
                //string dia = DateTime.Now.Day.ToString();
                //string dia = DateTime.Now.Day.ToString();
                DateTime t5 = DateTime.Now;

                fecha_ayer.Add(new DateTime(t5.Year, t5.Month, t5.Day - 1));
                int uno_mas = 1;
                foreach (string r in periodos_list)
                {
                    if (r != "0")
                    {
                        totales.Columns.Add(r);
                    }


                    fecha_ayer.Add(new DateTime(t5.Year, t5.Month - uno_mas, t5.Day - 1));
                    uno_mas++;

                }
                DataRow row;
                List<string> nombre_factores = get_factores();

                //CAPI
                //totales.Columns.Add("Total general");

                //RECORRE TABLA "TOTALES" PARA AGREGAR DATOS

                for (int i = 0; i <= 17; i++)
                {
                    //row = totales.NewRow();
                    //if (nombre_factores[i].ToString().Contains("Percentil "))
                    //{
                    //    row["FACTORES/Periodos"] = nombre_factores[i] + t_percentil.Text + "%";
                    //}
                    //else { row["FACTORES/Periodos"] = nombre_factores[i]; }
                    //totales.Rows.Add(row);
                    double sum = 0;
                    for (int y = 0; y < colum; y++)
                    {





                        if (i == 0)
                        {

                            if (y == colum)
                            {
                                // CAPI
                                //totales.Rows[i][y + 1] = ReporteRNegocio.Facturación_Mes(periodos, where).ToString("N0");

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
                                // CAPI
                                //totales.Rows[i][y + 1] = ReporteRNegocio.Facturación_Mes(periodos, where).ToString("N0");

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
                                // CAPI
                                //totales.Rows[i][y + 1] = ReporteRNegocio.Facturación_Mes(periodos, where).ToString("N0");

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
                                // CAPI
                                //totales.Rows[i][y + 1] = ReporteRNegocio.Facturación_Mes(periodos, where).ToString("N0");

                            }
                            else
                            {
                                string periodo = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio.Facturación_Mes_dolar(periodo, where).ToString("N0");
                            }
                        }


                        if (i == 4)
                        {
                            if (y <= colum - 4)
                            {


                                string dia_anterior = DateTime.Now.ToShortDateString().Substring(0, 2) + "/";
                                string mes_anterior = totales.Columns[y + 2].ColumnName.ToString().Substring(4) + "/";
                                string año_anterior = totales.Columns[y + 2].ColumnName.ToString().Substring(0, 4);
                                where_2 += " and FechaFactura <= CONVERT(datetime,'" + dia_anterior + mes_anterior + año_anterior + "',103) ";

                                string factura = ReporteRNegocio.Facturación_Mes(totales.Columns[y + 2].ColumnName.ToString(), where_2).ToString("N0");
                                totales.Rows[i][y + 1] = factura;
                            }
                        }

                        if (i == 5)
                        {

                            if (y == colum)
                            {
                                // CAPI
                                //totales.Rows[i][y + 1] = ReporteRNegocio._cltes_con_vta(periodos, where).ToString("N0");
                            }
                            else
                            {
                                string periodo = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio._cltes_con_vta(periodo, where).ToString("N0");
                            }

                        }
                        if (i == 6)
                        {
                            totales.Rows[i][y + 1] = (+(Double.Parse(totales.Rows[2][y + 1].ToString().Replace(".", ""))) / (Double.Parse(totales.Rows[5][y + 1].ToString().Replace(".", "")))).ToString("N0");
                        }



                        if (i == 7)
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
                                    //Double por_percentil;
                                    //if (t_percentil.Text == "") { por_percentil = 0.5; }
                                    //else
                                    //{
                                    //    por_percentil = Math.Round(Double.Parse(t_percentil.Text) / 100, 2);
                                    //}
                                    //totales.Rows[i][y + 1] = Math.Round(Percentile(percen.ToArray(), por_percentil)).ToString("N0");
                                }
                            }
                        }

                        if (i == 8)
                        {

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
                            Double sumatoria = (Double.Parse(totales.Rows[2][y + 1].ToString().Replace(".", "")));
                            Double sum_vent_sobre_percen = (Double.Parse(totales.Rows[7][y + 1].ToString().Replace(".", "")));
                            totales.Rows[i][y + 1] = Math.Round((sum_vent_sobre_percen / sumatoria * 100)).ToString() + " %";
                        }
                        if (i == 11)
                        {
                            if (y <= colum - 2)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = totales.Columns[y + 2].ColumnName;
                                totales.Rows[i][y + 1] = (ReporteRNegocio.cont_si_repite(periodo_apreguntar, periodo_anterior, where)).ToString("N0");
                            }
                        }
                        if (i == 12)
                        {
                            if (y <= colum - 2)
                            {
                                Double cont_si_repite = (Double.Parse(totales.Rows[11][y + 1].ToString().Replace(".", "")));
                                Double clte_con_vta_anterior = (Double.Parse(totales.Rows[5][y + 2].ToString().Replace(".", "")));
                                totales.Rows[i][y + 1] = Math.Round((cont_si_repite / clte_con_vta_anterior * 100)).ToString() + " %";
                            }
                        }
                        if (i == 13)
                        {
                            if (y <= colum - 2)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = totales.Columns[y + 2].ColumnName;
                                totales.Rows[i][y + 1] = (ReporteRNegocio.sum_a_recuperar(periodo_apreguntar, periodo_anterior, where)).ToString("N0");
                            }
                        }


                        if (i == 14)
                        {
                            if (y <= colum - 2)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = totales.Columns[y + 2].ColumnName;
                                totales.Rows[i][y + 1] = (ReporteRNegocio.sum_si_repite(periodo_apreguntar, periodo_anterior, where)).ToString("N0");
                            }
                        }
                        if (i == 15)
                        {
                            if (y <= colum - 2)
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                string periodo_anterior = totales.Columns[y + 2].ColumnName;
                                totales.Rows[i][y + 1] = (ReporteRNegocio.sum_si_repite_actual(periodo_apreguntar, periodo_anterior, where)).ToString("N0");
                            }
                        }


                        if (i == 16)
                        {
                            if (y == colum)
                            {
                                // CAPI
                                //totales.Rows[i][y + 1] = ReporteRNegocio.cltes_sobre_este_percentil(agregra_comillas(periodos), where, Double.Parse(totales.Rows[3][y + 1].ToString().Replace(".", ""))).ToString("N0");
                            }
                            else
                            {
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = ReporteRNegocio.prom_productos_por_factura(periodo_apreguntar, where);
                            }
                        }


                        if (i == 17)
                        {
                            if (y == colum)
                            {
                                // CAPI
                                //totales.Rows[i][y + 1] = ReporteRNegocio.cltes_sobre_este_percentil(agregra_comillas(periodos), where, Double.Parse(totales.Rows[3][y + 1].ToString().Replace(".", ""))).ToString("N0");
                            }
                            else
                            {

                                Double perc = Double.Parse(totales.Rows[7][y + 1].ToString().Replace(".", ""));
                                Double prom_cl = Double.Parse(totales.Rows[6][y + 1].ToString().Replace(".", ""));
                                string periodo_apreguntar = totales.Columns[y + 1].ColumnName;
                                totales.Rows[i][y + 1] = Math.Round((prom_cl / perc), 3);
                            }
                        }

                        //if (i == 15)
                        //{
                        //    if (y <= colum - 2)
                        //    {
                        //        Double sum_mes_c_anterior = (Double.Parse(totales.Rows[12][y + 1].ToString().Replace(".", "")));
                        //        Double sum_mes_anterior = (Double.Parse(totales.Rows[13][y + 1].ToString().Replace(".", "")));
                        //        totales.Rows[i][y + 1] = (Math.Round((sum_mes_c_anterior * 100 / sum_mes_anterior)) - 100).ToString() + " %";
                        //    }
                        //}



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

            //btn_infor aca
            //if (l_vendedores.Text != "" && l_clientes.Text != "" && txt_desde.Text != "" && txt_hasta.Text != "" && l_grupos.Text != "")
            //{
            string grupos = agregra_comillas(l_grupos.Text); ;
            string vendedores = agregra_comillas(l_vendedores.Text);
            string clientes = agregra_comillas(l_clientes.Text);
            string bodegas = agregra_comillas(l_bodegas.Text);
            string productos_2 = agregra_comillas(l_productos2.Text);
            string desde = txt_desde.Text.Replace("-", "/");
            string hasta = txt_hasta.Text.Replace("-", "/");

            int desde_dia = Convert.ToInt32(desde.Substring(0, 2));
            int desde_mes = Convert.ToInt32(desde.Substring(3, 2));
            int desde_año = Convert.ToInt32(desde.Substring(6, 4));
            string desde_menos1_mes = new DateTime(desde_año, desde_mes - 1, desde_dia).ToShortDateString();



            string mes = desde_menos1_mes.Substring(3, 2);
            string año = desde_menos1_mes.Substring(6, 4);


            string periodo_anterior = año + mes;

            string grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

            if (grupos_del_usuario == "")
            {
                grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
            }

            string where_detalle = "";
            DataTable dt2; DataView dtv = new DataView();
            dt2 = ReporteRNegocio.carga_grupos(txt_desde.Text, txt_hasta.Text, grupos_del_usuario);
            if (dt2.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>NO_GRUPOS();</script>", false);
            }
            else
            {
                string where_stock = " trandate <= convert(datetime, '" + txt_hasta.Text + "', 103)";
                string es_vendedor = ReporteRNegocio.esvendedor(USER);
                where = " where FechaFactura >= CONVERT(datetime,'" + desde + "', 103)  and FechaFactura <= CONVERT(datetime,'" + hasta + "',103) ";
                where_detalle = " where FechaFactura >= CONVERT(datetime, '" + desde + "', 103)  and FechaFactura <= CONVERT(datetime, '" + hasta + "', 103) ";
                string where5 = " where periodo = '" + periodo_anterior + "' ";

                if (grupos != "")
                {
                    where = where + " and user1 in (" + grupos + ") ";
                    where5 = where5 + " and user1 in (" + grupos + ") ";
                    where_detalle = where_detalle + " and user1 in (" + grupos + ") ";
                }
                else if (es_vendedor != "2")
                {
                    where = where + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario(USER)) + ")";
                    where5 = where5 + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario(USER)) + ")";
                    where_detalle = where_detalle + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario(USER)) + ")";
                }
                else if (es_vendedor == "2")
                {
                    where = where + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(USER)) + ")";
                    where5 = where5 + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(USER)) + ")";
                    where_detalle = where_detalle + " and user1 in (" + agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(USER)) + ")";
                }

                if (clientes != "")
                {
                    where = where + " and rutcliente in (" + clientes + ") ";
                    where5 = where5 + " and rutcliente in (" + clientes + ") ";
                    //where5 = where5 + " and user1 in (" + grupos + ") ";
                }
                if (vendedores != "")
                {
                    where = where + " and codvendedor in (" + vendedores + ") ";
                    where5 = where5 + " and codvendedor in (" + vendedores + ") ";
                    where_detalle = where_detalle + " and codvendedor in (" + vendedores + ") ";
                }

                if (productos_2 != "")
                {
                    where = where + " and producto in (" + productos_2 + ") ";
                    where5 = where5 + " and producto in (" + productos_2 + ") ";
                    //where_detalle = where_detalle + " and producto in (" + productos_2 + ") ";
                    where_stock = where_stock + " and invtid in (" + productos_2 + ") ";
                }
                if (es_vendedor == "2")
                {
                    where += " and codvendedor in ('" + USER + "')";
                    where5 += " and codvendedor in ('" + USER + "')";
                    where_detalle = where_detalle + " and codvendedor in (" + USER + ") ";
                }

                div_productos.Visible = true;
                DataTable productos = new DataTable();

                if (rd_abi.Checked)
                {
                    productos = ReporteRNegocio.productos_stock2(where, where_stock);

                }
                else {
                    productos = ReporteRNegocio.productos_stock_ventas(where);

                }

                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                DataTable clientes_Dt = ReporteRNegocio.listar_ALL_cliente2(where);
                DataTable ventas = ReporteRNegocio.ventas_matriz(where);
                DataTable ventas_periodo_anterior = ReporteRNegocio.ventas_matriz(where5);
                DataTable clientes_anteriores = ReporteRNegocio.clientes_periodo_anterior(where5, where);


                //DataTable dt_Cuentas = (DataTable)Session["DT_CUENTAS"];
                //foreach (DataRow j in dt_Cuentas.Rows)
                //{
                //    cont_filas++;
                //    if (j[5].ToString() == id.ToString())
                //    {
                //        row = cont_filas - 1;

                //    }
                //}

                //try
                //{
                //    dt_Cuentas.Rows.Remove(dt_Cuentas.Rows[row]);
                //    Session["DT_CUENTAS"] = dt_Cuentas;
                //}
                //catch
                //{

                //}




                string rut_clientes = "";

                string HTML = "";
                HTML += "<table id='TABLA_REPORTE55' class='table table-advance table-bordered fill-head tablesorter filtrar' style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";

                //HTML += "<table id='TABLA_REPORTE2' class='table table-advance table-bordered fill-head filtrar' style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";
                HTML += "<thead>";
                HTML += "<tr>";

                HTML += "<th colspan=1; class='test sorter-false' >" + txt_desde.Text + " a " + txt_hasta.Text + "</th>";



                double total__total_2 = 0;
                //(Math.Round((total_mes_actual1 * 100 / ACUMULADO)) - 100).ToString() + "%";
                foreach (DataRow r in ventas.Rows)
                {

                    total__total_2 += double.Parse(r[0].ToString());
                }



                HTML += "<th colspan=1; class='test sorter-false' style='background-color: rgb(148, 148, 148) !important;' > T.ANTE(" + periodo_anterior.Trim() + ") </th>";
                HTML += "<th colspan=1; class='test sorter-false' style='background-color: rgb(220, 220, 220) !important;color:black !important;' > %      </th>";
                HTML += "<th colspan=1; class='test sorter-false' style='background-color: rgb(220, 220, 220) !important; color:black !important;' > TOTAL </th>";

                foreach (DataRow r in productos.Rows)
                {
                    HTML += "<th colspan=1; class='test sorter-false' >" + r[0].ToString() + "</th>";
                    //HTML += "<th colspan=1; style='background-color: rgb(220, 220, 220) !important; color:white !important;' > TOTAL_CLIENTES</th>";
                    //HTML += "<th colspan=1; style='background-color: rgb(220, 220, 220) !important; color:white !important;' > TOTAL_CLIENTES_ANTERIOR</th>";

                    //TOTAL CLIENTES Y ANTERIOR
                    //HTML += "<td colspan=1; style='background-color: rgb(220, 220, 220) !important; color:white !important;'>" + total_cliente.ToString("N0") + " </td>";
                    //HTML += "<td colspan=1; style='background-color: rgb(148, 148, 148) !important; color:white !important;'>" + total_cliente2.ToString("N0") + " </td>";


                }



                HTML += "</tr>";

                HTML += "<tr>";
                HTML += "<th colspan=1; class='sorter-false' style='background-color: white !important; color:white !important;'></th>";
                HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(148, 148, 148) !important; color:black !important;'>TOTAL__ANTERIOR_</th>";
                HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(220, 220, 220) !important; color:black !important;'></th>";
                HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(220, 220, 220) !important; color:black !important;'></th>";

                foreach (DataRow r in productos.Rows)
                {
                    HTML += "<th colspan=1;  class='sorter-false' style='background-color: rgb(148, 148, 148) !important; color:white !important;' >ANTERIOR_" + r[1].ToString().Trim() + "_ANTERIOR</th>";

                    //HTML += "<th colspan=1; style='background-color: rgb(220, 220, 220) !important; color:white !important;' > TOTAL_CLIENTES</th>";
                    //HTML += "<th colspan=1; style='background-color: rgb(220, 220, 220) !important; color:white !important;' > TOTAL_CLIENTES_ANTERIOR</th>";

                    //TOTAL CLIENTES Y ANTERIOR
                    //HTML += "<td colspan=1; style='background-color: rgb(220, 220, 220) !important; color:white !important;'>" + total_cliente.ToString("N0") + " </td>";
                    //HTML += "<td colspan=1; style='background-color: rgb(148, 148, 148) !important; color:white !important;'>" + total_cliente2.ToString("N0") + " </td>";


                }

                HTML += "</tr>";


                HTML += "</tr>";
                HTML += "<tr>";
                HTML += "<th colspan=1; style='background-color: white !important; color:white !important;'></th>";
                HTML += "<th colspan=1; style='background-color: rgb(148, 148, 148) !important; color:white !important;'></th>";
                HTML += "<th colspan=1; style='background-color: rgb(220, 220, 220) !important; color:white !important;'></th>";

                HTML += "<th colspan=1; style='background-color: rgb(220, 220, 220) !important; color:black !important;'>TOTAL__TOTAL</th>";


                foreach (DataRow r in productos.Rows)
                {
                    HTML += "<th colspan=1; style='background-color: rgb(220, 220, 220) !important; color:white !important;' >" + r[1].ToString().Trim() + "TOTAL_</th>";

                    //HTML += "<th colspan=1; style='background-color: rgb(220, 220, 220) !important; color:white !important;' > TOTAL_CLIENTES</th>";
                    //HTML += "<th colspan=1; style='background-color: rgb(220, 220, 220) !important; color:white !important;' > TOTAL_CLIENTES_ANTERIOR</th>";

                    //TOTAL CLIENTES Y ANTERIOR
                    //HTML += "<td colspan=1; style='background-color: rgb(220, 220, 220) !important; color:white !important;'>" + total_cliente.ToString("N0") + " </td>";
                    //HTML += "<td colspan=1; style='background-color: rgb(148, 148, 148) !important; color:white !important;'>" + total_cliente2.ToString("N0") + " </td>";


                }




                //HTML += "<th colspan=1; class='test' > TOTAL_anterior" + r[1].ToString() + "</th>";

                HTML += "</thead>";
                HTML += "<tbody>";
                foreach (DataRow r in clientes_Dt.Rows)
                {
                    HTML += "<tr>";


                    HTML += "<tr style='background-color: rgb(234, 216, 216) !important; color:white !important;'>";
                    string script3 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;)", encriptador.EncryptData(where), encriptador.EncryptData(r[0].ToString().Trim()), encriptador.EncryptData(r[0].ToString().Trim()), encriptador.EncryptData("11"));
                    string script1 = string.Format("javascript:fuera22(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(r[0].ToString().Trim()), encriptador.EncryptData("88"));

                    HTML += "<td style='white-space:nowrap'><a data-toggle='tooltip' data-placement='top' title='" + r[1].ToString().Trim() + "' href='javascript:' onclick='" + script1 + "'>" + r[1].ToString().Trim() + " </a></td>";

                    //HTML += "<td style='white-space:nowrap'>" + r[1].ToString().Trim() + "</td>";
                    HTML += "<td style='background-color: rgb(148, 148, 148) !important; color:white !important;'>_anterior_" + r[1].ToString().Trim() + "_anterior_</td>";

                    HTML += "<td style='white-space: nowrap;background-color: rgb(220, 220, 220) !important; color:black !important;'>" + r[1].ToString().Trim() + "porct__</td>";

                    HTML += "<td style='white-space: nowrap;background-color: rgb(220, 220, 220) !important; color:white !important;'>" + r[1].ToString().Trim() + "total_</td>";

                    double total_cliente = 0;
                    double total_cliente2 = 0;
                    rut_clientes += "," + r[0].ToString().Trim();
                    foreach (DataRow r2 in productos.Rows)
                    {


                        DataRow[] venta = ventas.Select("producto = '" + r2[1].ToString().Trim() + "' and rutcliente = '" + r[0].ToString().Trim() + "'");
                        double SUM_VENTA = 0;
                        int sum_cont = 0;
                        foreach (DataRow row in venta)
                        {
                            SUM_VENTA += double.Parse(row[0].ToString());

                        }
                        total_cliente += SUM_VENTA;

                        string script2 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;)", encriptador.EncryptData(where_detalle), encriptador.EncryptData(r2[1].ToString().Trim()), encriptador.EncryptData(r[0].ToString().Trim()), encriptador.EncryptData("9"));


                        if (SUM_VENTA.ToString("N0") != "0")
                        {

                            HTML += "<td colspan=1; > <a data-toggle='tooltip' data-placement='top' title='" + r2[0].ToString().Trim() + "' href='javascript:' onclick='" + script2 + "'>" + SUM_VENTA.ToString("N0") + " </a></td>";
                        }
                        else {

                            HTML += "<td colspan=1;>" + SUM_VENTA.ToString("N0") + " </td>";

                        }


                        ///----------------------------------
                        ///
                        DataRow[] venta2 = ventas_periodo_anterior.Select("producto = '" + r2[1].ToString().Trim() + "' and rutcliente = '" + r[0].ToString().Trim() + "'");
                        double SUM_VENTA2 = 0;
                        //int sum_cont2 = 0;
                        foreach (DataRow row in venta2)
                        {
                            SUM_VENTA2 += double.Parse(row[0].ToString());

                        }
                        total_cliente2 += SUM_VENTA2;




                    }

                    string porct = (Math.Round((total_cliente * 100 / total__total_2))).ToString() + "%";
                    string porct2 = (Math.Round((total_cliente * 100 / total_cliente2))).ToString() + "%";
                    if (porct2 == "∞%" || porct2.Contains("Infinito%") || porct2.Contains("NeuN") || porct2.Contains("NaN")) { porct2 = ""; }
                    if (total_cliente > total_cliente2)
                    {
                        HTML = HTML.Replace(r[1].ToString().Trim() + "total_", "<a style='color:blue'>" + total_cliente.ToString("N0") + " (" + porct + ")</a>");
                    }
                    else {
                        HTML = HTML.Replace(r[1].ToString().Trim() + "total_", "<a style='color:red'>" + total_cliente.ToString("N0") + " (" + porct + ")</a>");
                    }
                    HTML = HTML.Replace(r[1].ToString().Trim() + "porct__", porct2);

                    HTML = HTML.Replace("_anterior_" + r[1].ToString().Trim() + "_anterior_", total_cliente2.ToString("N0"));


                    //HTML += "<td colspan=1; style='background-color: rgb(220, 220, 220) !important; color:white !important;'>" + total_cliente.ToString("N0") + " </td>";
                    //HTML += "<td colspan=1; style='background-color: rgb(148, 148, 148) !important; color:white !important;'>" + total_cliente2.ToString("N0") + " </td>";

                    HTML += "</tr>";
                }


                ///anterior
                ///
                double Total_clientes_anteriores = 0;
                foreach (DataRow r in clientes_anteriores.Rows)
                {
                    HTML += "<tr style='background-color: rgb(234, 216, 216) !important; color:white !important;'>";
                    string script2 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;)", encriptador.EncryptData(where5), encriptador.EncryptData(r[2].ToString().Trim()), encriptador.EncryptData(r[0].ToString().Trim()), encriptador.EncryptData("10"));
                    string script1 = string.Format("javascript:fuera22(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(r[2].ToString().Trim()), encriptador.EncryptData("88"));


                    HTML += "<td style='background-color: rgb(234, 216, 216) !important; white-space:nowrap'><a data-toggle='tooltip' data-placement='top' title='" + r[1].ToString().Trim() + "' href='javascript:' onclick='" + script1 + "'>" + r[1].ToString().Trim() + " </a></td>";
                    HTML += "<td style='background-color: rgb(234, 216, 216) !important; color:black !important;'>" + Convert.ToDouble(r[0]).ToString("N0") + "</td>";
                    HTML += "<td style='white-space: nowrap;background-color: rgb(234, 216, 216) !important; color:black !important;'></td>";

                    HTML += "<td style='white-space: nowrap;background-color: rgb(234, 216, 216) !important; color:white !important;'></td>";

                    Total_clientes_anteriores += Convert.ToDouble(r[0]);
                    //double total_cliente = 0;
                    //double total_cliente2 = 0;
                    //rut_clientes += "," + r[0].ToString().Trim();
                    foreach (DataRow r2 in productos.Rows)
                    {


                        //DataRow[] venta = ventas.Select("producto = '" + r2[1].ToString().Trim() + "' and rutcliente = '" + r[0].ToString().Trim() + "'");
                        //double SUM_VENTA = 0;
                        //int sum_cont = 0;
                        //foreach (DataRow row in venta)
                        //{
                        //    SUM_VENTA += double.Parse(row[0].ToString());

                        //}
                        //total_cliente += SUM_VENTA;

                        //string script2 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;)", encriptador.EncryptData(where_detalle), encriptador.EncryptData(r2[1].ToString().Trim()), encriptador.EncryptData(r[0].ToString().Trim()), encriptador.EncryptData("9"));


                        //if (SUM_VENTA.ToString("N0") != "0")
                        //{

                        //    HTML += "<td colspan=1; > <a data-toggle='tooltip' data-placement='top' title='" + r2[0].ToString().Trim() + "' href='javascript:' onclick='" + script2 + "'>" + SUM_VENTA.ToString("N0") + " </a></td>";
                        //}
                        //else {

                        HTML += "<td style='background-color: rgb(234, 216, 216) !important;'  colspan=1;></td>";

                        //}


                        /////----------------------------------
                        /////
                        //DataRow[] venta2 = ventas_periodo_anterior.Select("producto = '" + r2[1].ToString().Trim() + "' and rutcliente = '" + r[0].ToString().Trim() + "'");
                        //double SUM_VENTA2 = 0;
                        ////int sum_cont2 = 0;
                        //foreach (DataRow row in venta2)
                        //{
                        //    SUM_VENTA2 += double.Parse(row[0].ToString());

                        //}
                        //total_cliente2 += SUM_VENTA2;




                    }

                    //string porct = (Math.Round((total_cliente * 100 / total__total_2))).ToString() + "%";
                    //string porct2 = (Math.Round((total_cliente * 100 / total_cliente2))).ToString() + "%";
                    //if (porct2 == "∞%" || porct2.Contains("Infinito%") || porct2.Contains("NeuN") || porct2.Contains("NaN")) { porct2 = ""; }
                    //if (total_cliente > total_cliente2)
                    //{
                    //    HTML = HTML.Replace(r[1].ToString().Trim() + "total_", "<a style='color:blue'>" + total_cliente.ToString("N0") + " (" + porct + ")</a>");
                    //}
                    //else {
                    //    HTML = HTML.Replace(r[1].ToString().Trim() + "total_", "<a style='color:red'>" + total_cliente.ToString("N0") + " (" + porct + ")</a>");
                    //}
                    //HTML = HTML.Replace(r[1].ToString().Trim() + "porct__", porct2);

                    //HTML = HTML.Replace("_anterior_" + r[1].ToString().Trim() + "_anterior_", total_cliente2.ToString("N0"));


                    ////HTML += "<td colspan=1; style='background-color: rgb(220, 220, 220) !important; color:white !important;'>" + total_cliente.ToString("N0") + " </td>";
                    ////HTML += "<td colspan=1; style='background-color: rgb(148, 148, 148) !important; color:white !important;'>" + total_cliente2.ToString("N0") + " </td>";

                    HTML += "</tr>";
                }






                HTML += "</tbody>";
                //HTML += "<tfoot>";
                //HTML += "<tr>";
                //HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(220, 220, 220) !important; color:white !important;' ></th>";

                //foreach (DataRow r2 in productos.Rows)
                //{





                //}

                ////HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(220, 220, 220) !important; color:white !important;'>" + total_total.ToString("N0") + "</th>";
                ////HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(148, 148, 148) !important; color:white !important;'></th>";
                //HTML += "</tr>";

                //HTML += "<tr>";
                ////HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(148, 148, 148) !important; color:white !important;' ></th>";
                double total_total2 = 0;
                double total_total = 0;
                foreach (DataRow r2 in productos.Rows)
                {
                    double SUM_VENTA = 0;
                    int sum_cont = 0;
                    //DataRow[] venta = new DataRow;
                    try
                    {
                        ///////////////////anterior x prod


                        DataRow[] venta = ventas_periodo_anterior.Select("producto = '" + r2[1].ToString().Trim() + "'");



                        foreach (DataRow row in venta)
                        {
                            SUM_VENTA += double.Parse(row[0].ToString());

                        }
                
                    total_total2 += SUM_VENTA;
                    HTML = HTML.Replace("ANTERIOR_" + r2[1].ToString().Trim() + "_ANTERIOR", SUM_VENTA.ToString("N0"));
                    }
                    catch { HTML = HTML.Replace("ANTERIOR_" + r2[1].ToString().Trim() + "_ANTERIOR","0"); }

                    ///////////////////total x prod



                    DataRow[] venta1 = ventas.Select("producto = '" + r2[1].ToString().Trim() + "'");
                    double SUM_VENTA1 = 0;
                    int sum_cont1 = 0;
                    foreach (DataRow row in venta1)
                    {
                        SUM_VENTA1 += double.Parse(row[0].ToString());

                    }

                    total_total += SUM_VENTA1;


                    string porct = (Math.Round((SUM_VENTA1 * 100 / total__total_2))).ToString() + "%";

                    if (SUM_VENTA1 > SUM_VENTA)
                    {

                        HTML = HTML.Replace(r2[1].ToString().Trim() + "TOTAL_", "<a style='color:blue'>" + SUM_VENTA1.ToString("N0") + " " + porct + " </a>");
                    }
                    else {

                        HTML = HTML.Replace(r2[1].ToString().Trim() + "TOTAL_", "<a style='color:red'>" + SUM_VENTA1.ToString("N0") + " " + porct + " </a>");
                    }
                    //HTML = HTML.Replace("total_anterior_" + r[1].ToString().Trim(), total_cliente2.ToString("N0"));




                }
                HTML = HTML.Replace("TOTAL__TOTAL", total_total.ToString("N0"));
                HTML = HTML.Replace("TOTAL__ANTERIOR_", (Total_clientes_anteriores + total_total2).ToString("N0"));
                //HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(148, 148, 148) !important; color:white !important;'></th>";
                //HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(148, 148, 148) !important; color:white !important;'>" + total_total2.ToString("N0") + "</th>";
                //HTML += "</tr>";








                //HTML += "</tfoot>";

                HTML += "  </table>";

                table_matriz.InnerHtml = HTML;
                R_Excel_1.InnerHtml = HTML;

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasdaqsdsaeee", "<script> SortPrah(); </script>", false);
                //string periodos = ReporteRNegocio.listar_periodos_(" where FechaFactura >= CONVERT(datetime,'" + desde + "', 103)  and FechaFactura <= CONVERT(datetime,'" + hasta + "',103)");
                //productos = ReporteRNegocio.listar_resumen_productos(periodos, where);
                //PERIODOS = periodos;
                //DataView view = new DataView(productos);
                //DataTable distinctValues = view.ToTable(true, "NombreCliente");

                //productos.Columns.Add("Total");

                //int colum = productos.Columns.Count;

                //DataView dv = productos.DefaultView;
                //dv.Sort = "NombreCliente ASC";
                //productos = dv.ToTable();

                //Session["TaskTable"] = productos;

                //header_sum = true;
                //cont_det = 1;
                //G_PRODUCTOS.DataSource = productos;
                ////G_PRODUCTOS.Columns[colum - 1].Visible = false;
                ////G_PRODUCTOS.Columns[colum - 2].Visible = false;
                //G_PRODUCTOS.DataBind();

                //VOLVER A CARGAR LOS MULTISELECT

                try
                {

                    cargar_combo_bodegas(grupos);

                    foreach (ListItem item in d_bodegas2.Items)
                    {

                        if (l_bodegas.Text.Contains(item.Value.ToString()))
                        {
                            item.Selected = true;
                        }
                    }
                }
                catch
                {


                }





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

                //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_PRODUCTOS')); </script>", false);

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

            R_Excel_1.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());

            Response.End();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
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

            //G_PRODUCTOS.Columns[0].Visible = false;

            R_Excel_1.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());

            Response.End();
        }


        protected void b_Click(object sender, ImageClickEventArgs e)
        {
            //aca carga_fechas
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
                    cargar_combo_bodegas(grupos_del_usuario);
                }
                else
                {
                    string where = " where FechaFactura >= CONVERT(datetime,'" + txt_desde.Text + "', 103) " +
                        " and FechaFactura <= CONVERT(datetime,'" + txt_hasta.Text + "',103) ";
                    where += "and user1 in (" + grupos_del_usuario + ")";

                    cargar_combo_VENDEDOR(ReporteRNegocio.listar_ALL_vendedores(where + " and codvendedor = '" + USER + "' "), dtv);
                    cargar_combo_clientes(ReporteRNegocio.listar_ALL_cliente2(where + " and codvendedor = '" + USER + "' "), dtv);
                }
                cargar_combo_producto(ReporteRNegocio.listar_ALL_productos_stock("", HttpContext.Current.Session["clase"].ToString()), dtv);

            }
        }

        private void cargar_combo_bodegas(string grupos)
        {
            string DESDE = txt_desde.Text;
            string HASTA = txt_hasta.Text;

            DESDE = DESDE.Replace("-", "/");
            HASTA = HASTA.Replace("-", "/");

            string where_l = " where FechaFactura >= CONVERT(datetime,'" + DESDE + "', 103) " +
                            " and FechaFactura <= CONVERT(datetime,'" + HASTA + "',103) and user1 in (" + grupos + ")";

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.carga_bodega(where_l);
            dtv = dt.DefaultView;
            d_bodegas2.DataSource = dtv;
            d_bodegas2.DataTextField = "nom_bodega";
            d_bodegas2.DataValueField = "nom_bodega";
            //d_vendedor_.SelectedIndex = -1;
            d_bodegas2.DataBind();
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



        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //MakeAccessible(G_PRODUCTOS);
            //MakeAccessible(G_ASIGNADOS);
            //MakeAccessible(G_QR);
            //MakeAccessible(G_LISTA);
        }
        public void JQ_Datatable()
        {
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd121mp", "<script language='javascript'>creagrilla();</script>", false);
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

    }
}