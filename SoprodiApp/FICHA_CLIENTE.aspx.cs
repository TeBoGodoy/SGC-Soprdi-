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
using System.Net.Mail;
using System.Net;

namespace SoprodiApp
{
    public partial class FICHA_CLIENTE : System.Web.UI.Page
    {
        public static int cont_periodos;
        public static DataTable aux;
        public static DataTable productos;
        public static DataTable totales;
        public static bool izq = true;
        private static string USER;
        public static int cont_det;
        private static string GRUPOS_USUARIO;
        public static double sum_por_row;
        public static int COLUMNA_DE_FACTURA;
        public static bool busca_columna_fac;
        public static bool columna_fac;

        public static bool contacto_extra = false;
        public static bool cta_extra = false;
        public static bool socio_extra = false;

        public static bool header_sum2;

        public static bool esta_cruzado = false;

        public static int CONT_TABL = 0;

        public static string pais = "";
        public static string ciudad = "";
        public static string pais_so = "";
        public static string ciudad_so = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //excel2.Attributes.Add("onClick", "return false;");
            //Page.RegisterRedirectOnSessionEndScript();
            JQ_Datatable();
            if (!IsPostBack)
            {
                correo_env.InnerText = "";
                USER = User.Identity.Name.ToString();
                string bit = "";
                string clie_rut = "";
                try
                {
                    bit = Request.QueryString["i"].ToString();
                }
                catch
                {

                }

                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                try
                {
                    if (bit == "22")
                    {                              
                        clie_rut = Request.QueryString["R"].ToString();
                        bit = "88";
                    }
                    else
                    {
                        bit = encriptador.DecryptData(Request.QueryString["i"].ToString().Replace(" ", "+"));
                        clie_rut = encriptador.DecryptData(Request.QueryString["R"].ToString().Replace(" ", "+"));
                    }
                }
                catch
                {

                }
                List<string> u_negocio = ReporteRNegocio.trae_u_negocio(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "6")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }

                string grupos_del_usuario = "";

                grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

                if (grupos_del_usuario == "")
                {
                    grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
                }

                string es_vende = ReporteRNegocio.esvendedor(User.Identity.Name);
                GRUPOS_USUARIO = grupos_del_usuario;

                l_usuario_.Text = USER;
                DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now;
                //t = new DateTime(t.Year, t.Month - 6, 1);
                txt_desde.Text = ReporteRNegocio.CALCULA_DESDE(t.Month, t.Year);
                txt_hasta.Text = t2.ToShortDateString();

                if (bit == "88")
                {
                    CONT_TABL++;
                    muestra_nue.Visible = true;
                    txt_desde2.Text = ReporteRNegocio.CALCULA_DESDE(t.Month, t.Year);
                    txt_hasta2.Text = t2.ToShortDateString();




                    //DATOS DEL CLIENTE

                    string rutcliente = clie_rut.Replace("-", "").Replace(".", "").Trim();
                    string rut_ini = rutcliente.Trim().Substring(0, rutcliente.Trim().Length - 1);
                    double rut = double.Parse(rut_ini);

                    rutcliente = rut.ToString("N0") + "-" + rutcliente.Trim().Substring(rutcliente.Trim().Length - 1);



                    string nombrecliente = "";
                    string direccion = "";
                    string ciudad = "";
                    string pais = "";
                    string fono = "";
                    string correo = "";
                    string vendedor = "";
                    string letra_credito = "";

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
                        letra_credito = r["tipo_credi"].ToString();
                    }

                    l_rut_cliente.Text = rutcliente;
                    div_sp.InnerHtml = "<a href='REPORTE_SP.aspx?G=912&C=" + rut_ini.Trim() + "*"+ vendedor + "' target='_blank'>SP</a> ";


                    vendedor_.Text = ReporteRNegocio.nombre_vendedor(vendedor);
                    nombrecliente_.Text = nombrecliente;
                    rutcliente_.Text = rutcliente;
                    direccion_.Text = direccion;
                    fono_.Text = fono;
                    ciudad_.Text = ciudad + ", " + pais;
                    codvendedor.Text = vendedor;
                    l_credito.Text = ReporteRNegocio.linea_credito(clie_rut.Replace("-", "").Replace(".", ""));

                    string lc = l_credito.Text.Substring(0, l_credito.Text.IndexOf("("));
                    string ld = l_credito.Text.Substring(l_credito.Text.IndexOf(" ") + 1).Replace(" )", "");
                    DataTable cr = ReporteRNegocio.corr_usuario(User.Identity.Name);
                    foreach (DataRow r in cr.Rows)
                    {
                        tx_enviar_.Text = r[1].ToString();
                    }

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
                    l_credito.Text = lc + "(D: " + ld + " )" + " - Letra("+ letra_credito + ")";

                    CLIENTES_FICHA.ActiveViewIndex = 1;

                    div_historico.Visible = false;
                    DIV_CRUZADO.Visible = false;

                    cont_det = 1;
                    DIV_CRUZADO.Visible = true;
                    G_CRUZADO.Visible = true;

                    string where = " where user1 in (" + grupos_del_usuario + ") and rutcliente like '%" + rutcliente_.Text.Replace(".", "").Replace("-", "") + "%' and FechaFactura >= CONVERT(datetime,'" + txt_desde2.Text + "', 103)  and FechaFactura <= CONVERT(datetime,'" + txt_hasta2.Text + "',103)";
                    if (es_vende == "2") { where += " and codvendedor like '%" + User.Identity.Name + "%' "; }

                    DataTable cruz = ReporteRNegocio.listar_resumen_productos_ficha(where);
                    cruz.Columns.Add("Total");
                    header_sum2 = true;
                    G_CRUZADO.DataSource = cruz;
                    G_CRUZADO.DataBind();

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeeeeqe", "<script> try{ new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_CRUZADO'), {ascending: true });} catch(e){}; </script>", false);
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "texassdxeee", "<script> superfiltro3(); </script>", false);

                }
            }
        }


        protected void b_Click(object sender, ImageClickEventArgs e)
        {

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.carga_grupos(txt_desde.Text, txt_hasta.Text, GRUPOS_USUARIO);
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
                    where += "and user1 in (" + GRUPOS_USUARIO + ")";

                    cargar_combo_VENDEDOR(ReporteRNegocio.listar_ALL_vendedores(where), dtv);
                    cargar_combo_clientes(ReporteRNegocio.listar_ALL_cliente2(where), dtv);

                }
                else
                {
                    string where = " where FechaFactura >= CONVERT(datetime,'" + txt_desde.Text + "', 103) " +
                        " and FechaFactura <= CONVERT(datetime,'" + txt_hasta.Text + "',103) ";
                    where += "and user1 in (" + GRUPOS_USUARIO + ")";

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
            //d_grupos_usuario.DataSource = dtv;
            //d_grupos_usuario.DataTextField = "user1";
            //d_grupos_usuario.DataValueField = "user1";
            //d_grupos_usuario.DataBind();
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

            //esta_cruzado = false;
            //string mayor_a = mayor_.Text.Replace(".", "").Replace("$", "").Trim();
            string mayor_a = "281675900";
            string desde = txt_desde.Text;
            string hasta = txt_hasta.Text;

            string es_vendedor = ReporteRNegocio.esvendedor(USER);


            string having = "";
            string where = " where user1 in (" + GRUPOS_USUARIO + ") and FechaFactura >= CONVERT(datetime,'" + desde + "', 103)  and FechaFactura <= CONVERT(datetime,'" + hasta + "',103)";
            if (mayor_a != "")
            {
                having += "  having sum(neto_pesos) > " + mayor_a;
            }

            DataTable dt; DataView dtv = new DataView();

            if (es_vendedor == "2")
            {
                btn_excel.Visible = false;
                where += " and codvendedor like  '%" + User.Identity.Name.ToString() + "%'";
            }
            else
            {
                btn_excel.Visible = true;

            }

            cont_det = 1;

            div_report.Visible = true;
            div_nuevos.Visible = false;

            G_INFORME_TOTAL_VENDEDOR.Visible = true;
            CLIENTES_FICHA.ActiveViewIndex = 0;

            DateTime t = DateTime.Now;
            DateTime t2 = DateTime.Now;
            t = new DateTime(t.Year, t.Month, 1);
            string HASTA = t2.ToShortDateString().Replace("-", "/");
            string mes1 = HASTA.Substring(HASTA.IndexOf("/") + 1, 2);
            string año1 = HASTA.Substring(HASTA.LastIndexOf("/") + 1);
            string periodo_hasta = año1 + mes1;

            //t2 = new DateTime(t.Year, t.Month - Convert.ToInt32(t_meses.Text), 1);

            string DESDE = calcula_prom_meses(t.Year, t.Month, Convert.ToInt32(t_meses.Text)).Replace("-", "/");
            string mes2 = DESDE.Substring(DESDE.IndexOf("/") + 1, 2).Replace("/","");
            string año2 = DESDE.Substring(DESDE.LastIndexOf("/") + 1);
            if (mes2.Length == 1) {
                mes2 = "0" + mes2;
            }


            string periodo_desde = año2 + mes2;

            G_INFORME_TOTAL_VENDEDOR.DataSource = ReporteRNegocio.list_ficha_cliente(where, periodo_desde, periodo_hasta);
            G_INFORME_TOTAL_VENDEDOR.DataBind();

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teassdasdsaeee", "<script> superfiltro(); </script>", false);

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeeeeqe", "<script> try{ new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_INFORME_TOTAL_VENDEDOR')) } catch(e){}; </script>", false);

        }

        private string calcula_prom_meses(int año, int mes_actual, int meses_prom)
        {
            string m = "";
            if (mes_actual < meses_prom)
            {
                m = "01/" + (13 - (meses_prom - mes_actual)).ToString() + "/" + (año - 1).ToString();
            }
            else if (mes_actual == meses_prom)
            {
                m = "01/01/" + (año).ToString();

            }
            else
            {
                m = "01/" + (mes_actual - meses_prom).ToString() + "/" + (año).ToString();

            }

            return m;
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



        protected void G_INFORME_TOTAL_VENDEDOR_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].Attributes["data-sort"] = cont_det.ToString();
                e.Row.Cells[0].Text = cont_det.ToString();
                cont_det++;

                string valor = e.Row.Cells[1].Text;
                string rut_ini = valor.Trim().Substring(0, valor.Trim().Length - 1);
                double rut = double.Parse(rut_ini);

                e.Row.Cells[1].Text = rut.ToString("N0") + "-" + valor.Trim().Substring(valor.Trim().Length - 1);

                //string lin_d = ReporteRNegocio.linea_credito_disponible(e.Row.Cells[1].Text.Replace("-","").Replace(".", ""));

                //e.Row.Cells[7].Text = lin_d;

                G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[5].Attributes["data-sort-method"] = "number";
                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                string script1 = string.Format("javascript:fuera22(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[1].Text), encriptador.EncryptData("88"));
                e.Row.Cells[2].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[2].Text + " </a>";


                double d;
                double.TryParse(e.Row.Cells[5].Text, out d);
                string aux = "";
                if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                e.Row.Cells[5].Text = aux;

                double d1;
                double.TryParse(e.Row.Cells[6].Text, out d1);
                string aux1 = "";
                if (d1 == 0) { aux1 = ""; } else { aux1 = d1.ToString("N0"); }
                e.Row.Cells[6].Text = aux1;

                double d2;
                double.TryParse(e.Row.Cells[7].Text, out d2);
                string aux2 = "";
                if (d2 == 0) { aux2 = ""; } else { aux2 = d2.ToString("N0"); }
                e.Row.Cells[7].Text = aux2;

                double d3;
                double.TryParse(e.Row.Cells[8].Text, out d3);
                string aux3 = "";
                if (d3 == 0) { aux3 = ""; } else { aux3 = d3.ToString("N0"); }
                e.Row.Cells[8].Text = aux3;

                if (e.Row.Cells[7].Text == "")
                {
                    e.Row.Cells[7].Text = e.Row.Cells[6].Text;
                }


            }

        }

        protected void G_INFORME_VENDEDOR_RowDataBound(object sender, GridViewRowEventArgs e)
        {



        }

        protected void G_INFORME_TOTAL_VENDEDOR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "txt")
            {


            }

            if (e.CommandName == "ficha")
            {
                muestra_nue.Visible = true;
                DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now;
                t = new DateTime(t.Year, t.Month - 1, 1);
                txt_desde2.Text = t.ToShortDateString();
                txt_hasta2.Text = t2.ToShortDateString();

                string rutcliente = G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                string nombrecliente = "";
                string direccion = "";
                string ciudad = "";
                string pais = "";
                string fono = "";
                string correo = "";
                string vendedor = "";
                DataTable dato_clientes = ReporteRNegocio.datos_cliente(rutcliente.Trim());
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

                string rut_ini = rutcliente.Trim().Substring(0, rutcliente.Trim().Length - 1);
                double rut = double.Parse(rut_ini);

                rutcliente = rut.ToString("N0") + "-" + rutcliente.Trim().Substring(rutcliente.Trim().Length - 1);

                vendedor_.Text = ReporteRNegocio.nombre_vendedor(vendedor); nombrecliente_.Text = nombrecliente; rutcliente_.Text = rutcliente; direccion_.Text = direccion; fono_.Text = fono; ciudad_.Text = ciudad + ", " + pais;
                codvendedor.Text = vendedor;


                l_credito.Text = G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString() + "(D: " + G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString() + ")";

                CLIENTES_FICHA.ActiveViewIndex = 1;
                div_historico.Visible = false;
                DIV_CRUZADO.Visible = false;
            }
        }

        protected void btn_volver_usuario_ServerClick(object sender, EventArgs e)
        {
            if (muestra_nue.Visible.ToString() == "True")
            {
                CLIENTES_FICHA.ActiveViewIndex = 0;
            }
            else
            {
                CLIENTES_FICHA.ActiveViewIndex = 3;
            }
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeeeeqe", "<script> try{ new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_INFORME_TOTAL_VENDEDOR')) } catch (e) {}; </script>", false);

        }

        protected void G_INFORME_TOTAL_VENDEDOR_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void G_CTA_CORRIENTE_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = cont_det.ToString();
                cont_det++;

                double d;
                double.TryParse(e.Row.Cells[3].Text, out d);
                string aux = "";
                if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                e.Row.Cells[3].Text = aux;
                sum_por_row += d;

                if (busca_columna_fac)
                {
                    try
                    {
                        for (int x = 0; x <= G_CTA_CORRIENTE.HeaderRow.Cells.Count; x++)
                        {
                            if (G_CTA_CORRIENTE.HeaderRow.Cells[x].Text.Contains("Factura"))
                            {
                                columna_fac = true;
                                COLUMNA_DE_FACTURA = x;
                                busca_columna_fac = false;
                                break;
                            }
                        }
                    }
                    catch { }
                }

                G_CTA_CORRIENTE.HeaderRow.Cells[3].Text = "Venta ( " + sum_por_row.ToString("N0") + " )";
                G_CTA_CORRIENTE.HeaderRow.Cells[0].Attributes["data-sort-method"] = "number";
                G_CTA_CORRIENTE.HeaderRow.Cells[1].Attributes["class"] = "sort-default";
                G_CTA_CORRIENTE.HeaderRow.Cells[2].Attributes["class"] = "sort-default";
                G_CTA_CORRIENTE.HeaderRow.Cells[3].Attributes["data-sort-method"] = "number";
                G_CTA_CORRIENTE.HeaderRow.Cells[5].Attributes["class"] = "sort-default";

                e.Row.Cells[5].Attributes["data-sort"] = cont_det.ToString();

                if (columna_fac)
                {
                    //clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                    //string script = string.Format("javascript:fuera1(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[COLUMNA_DE_FACTURA].Text), encriptador.EncryptData(""), encriptador.EncryptData(""), encriptador.EncryptData("6"));
                    //e.Row.Cells[COLUMNA_DE_FACTURA].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[COLUMNA_DE_FACTURA].Text + " </a>";

                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                    string año_factura = ReporteRNegocio.trae_año_factura(e.Row.Cells[COLUMNA_DE_FACTURA].Text);
                    año_factura = año_factura.Substring(0, 4);
                    string script = string.Format("javascript:fuera3(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[COLUMNA_DE_FACTURA].Text), encriptador.EncryptData(año_factura));
                    e.Row.Cells[COLUMNA_DE_FACTURA].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[COLUMNA_DE_FACTURA].Text + " </a>";
                }
            }
        }

        protected void G_CTA_CORRIENTE_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btn_cta_corriente_Click(object sender, EventArgs e)
        {
            DIV_CRUZADO.Visible = false;
            //div6.Attributes["style"] = "display:none";
            busca_columna_fac = true;
            columna_fac = false;
            sum_por_row = 0;
            cont_det = 1;
            G_CTA_CORRIENTE.DataSource = ReporteRNegocio.trae_cta_cte_cliente(rutcliente_.Text.Replace("-", "").Replace(".", ""));
            G_CTA_CORRIENTE.DataBind();

            div_historico.Visible = true;
            G_CTA_CORRIENTE.Visible = true;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teassdxeee", "<script> superfiltro2(); </script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeeeaeqe", "<script> try{ new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_CTA_CORRIENTE')) } catch(e){}; </script>", false);
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeqeeeqe", "<script> try{ new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_CRUZADO')) } catch(e){}; </script>", false);
        }

        protected void excel_Click(object sender, EventArgs e)
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

            G_CTA_CORRIENTE.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());

            Response.End();
        }




        protected void btn_cruzado_Click(object sender, EventArgs e)
        {
            div_historico.Visible = false;
            cont_det = 1;
            DIV_CRUZADO.Visible = true;
            header_sum2 = true;
            G_CRUZADO.Visible = true;
            string where = " where rutcliente like '%" + rutcliente_.Text.Replace(".", "").Replace("-", "") + "%' and FechaFactura >= CONVERT(datetime,'" + txt_desde2.Text + "', 103)  and FechaFactura <= CONVERT(datetime,'" + txt_hasta2.Text + "',103)";
            if (ReporteRNegocio.esvendedor(User.Identity.Name) == "2") { where += " and codvendedor like '%" + User.Identity.Name + "%' "; }
            DataTable cruz = ReporteRNegocio.listar_resumen_productos_ficha(where);
            cruz.Columns.Add("Total");
            G_CRUZADO.DataSource = cruz;
            G_CRUZADO.DataBind();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeeeeqe", "<script> try{ new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_CRUZADO')) , {descending: true }}); catch(e){}; </script>", false);
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeeesseqe", "<script> try{ new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_CTA_CORRIENTE')) } catch(e){}; </script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "texassdxeee", "<script> superfiltro3(); </script>", false);
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

                    string where_es = " where rutcliente like '%" + rutcliente_.Text.Replace("-", "").Replace(".", "") + "%'";
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

                    string script2 = string.Format("javascript:fuera1(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;)", encriptador.EncryptData(G_CRUZADO.HeaderRow.Cells[i].Text), encriptador.EncryptData(ReporteRNegocio.cod_producto(e.Row.Cells[1].Text)), encriptador.EncryptData(rutcliente_.Text.Replace(".", "").Replace("-", "")), encriptador.EncryptData("7"));

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

        protected void G_CRUZADO_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void excel2_Click(object sender, EventArgs e)
        {
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);

            G_CRUZADO.RenderControl(htmlWrite);

            string body = stringWrite.ToString();

            body = body.Replace("<tr class='test no-sort'>", "<tr style='background-color:#428bca;color:#fff'>");



            DataTable corr = ReporteRNegocio.corr_usuario(User.Identity.Name);
            string usuario = "";
            string correo = "";
            foreach (DataRow r in corr.Rows)
            {
                usuario = r[0].ToString();
                correo = r[1].ToString();
            }


            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(tx_enviar_.Text));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "Cliente Producto/Periodo ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
            //if (cc != "")
            //{
            //    email.CC.Add(cc);
            //}
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

            email.Body += "<div> Estimado :<br> <br> Productos vendidos dentro de los periodos al Cliente :  <b>" + nombrecliente_.Text + "</b> <br><br>";

            //string body_correo = crear_cuerpo_correo();
            email.Body += "<div>" + body.Replace(",", ".") + "</div>";


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
                correo_env.InnerText = "Correo Enviado!";

            }
            catch (Exception ex)
            {
                correo_env.InnerText = "Error al enviar";
            }
        }

        protected void editar_Click(object sender, EventArgs e)
        {

            clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "xte", "<script> fuera('" + encriptador.EncryptData(rutcliente_.Text) + "', '" + encriptador.EncryptData("2") + "') </script>", false);


            //h3_.InnerText = "Editar Cliente";

            //CLIENTES_FICHA.ActiveViewIndex = 2;

            //DataTable dt; DataView dtv = new DataView();

            //string where = " where user1 in (" + GRUPOS_USUARIO + ")";

            //cargar_combo_VENDEDOR_(ReporteRNegocio.listar_ALL_vendedores(where), dtv);
            //vendedor_edit.SelectedValue = codvendedor.Text.Trim();

            //rutcliente_edit.Text = rutcliente_.Text;
            //nombrecliente_edit.Text = nombrecliente_.Text;
            //direccion_edit.Value = direccion_.Text;
            //ciudad_edit.Value = ciudad_.Text;
            //fono_edit.Value = fono_.Text;
            //correo_edit.Value = correo_.Text;
            ////vendedor_edit.SelectedIndex = vendedor_.Text.Substring(vendedor_.Text.IndexOf("(") + 1, vendedor_.Text.IndexOf(")") - 1).Trim();
            //help_vendedor.InnerText = vendedor_.Text.Substring(vendedor_.Text.IndexOf(")") + 1);
        }

        private void cargar_combo_VENDEDOR_(DataTable dt, DataView dtv, string vended)
        {
            if (vended != "2")
            {
                dt.Rows.Add(new Object[] { "-1", "-- Otro --" });
            }
            dtv = dt.DefaultView;
            dtv.Sort = "cod_vend";
            vendedor_NEW_.DataSource = dtv;
            vendedor_NEW_.DataTextField = "nom_vend";
            vendedor_NEW_.DataValueField = "cod_vend";

            vendedor_NNEW.DataSource = dtv;
            vendedor_NNEW.DataTextField = "nom_vend";
            vendedor_NNEW.DataValueField = "cod_vend";



            string es_vend = ReporteRNegocio.esvendedor(User.Identity.Name);

            vendedor_NEW_.DataBind();
            vendedor_NNEW.DataBind();
            if (es_vend == "2")
            {
                vendedor_NNEW.SelectedIndex = 0;
                vendedor_NEW_.SelectedIndex = 0;
                l_vendedores.Text = User.Identity.Name.Trim();
            }
        }

        protected void volver_2_ServerClick(object sender, EventArgs e)
        {

            DateTime t = DateTime.Now;
            DateTime t2 = DateTime.Now;
            t = new DateTime(t.Year, t.Month - 1, 1);
            txt_desde2.Text = t.ToShortDateString();
            txt_hasta2.Text = t2.ToShortDateString();

            string rutcliente = rutcliente_.Text.Replace("-", "").Replace(".", "");
            string nombrecliente = "";
            string direccion = "";
            string ciudad = "";
            string pais = "";
            string fono = "";
            string correo = "";
            string vendedor = "";
            DataTable dato_clientes = ReporteRNegocio.datos_cliente(rutcliente.Trim());
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

            string rut_ini = rutcliente.Trim().Substring(0, rutcliente.Trim().Length - 1);
            double rut = double.Parse(rut_ini);

            rutcliente = rut.ToString("N0") + "-" + rutcliente.Trim().Substring(rutcliente.Trim().Length - 1);

            vendedor_.Text = "(" + vendedor + ") " + ReporteRNegocio.nombre_vendedor(vendedor); nombrecliente_.Text = nombrecliente; rutcliente_.Text = rutcliente; direccion_.Text = direccion; fono_.Text = fono; ciudad_.Text = ciudad + ", " + pais;

            CLIENTES_FICHA.ActiveViewIndex = 1;
            div_historico.Visible = false;
            DIV_CRUZADO.Visible = false;
        }

        protected void save_edit_Click(object sender, EventArgs e)
        {
            string es_vend = ReporteRNegocio.nombre_vendedor(vendedor_edit.SelectedValue.Trim());
            if (es_vend != "")
            {
                string q = ReporteRNegocio.update_cliente(rutcliente_edit.Text.Replace("-", "").Replace(".", ""), direccion_edit.Value, ciudad_edit.Value.Substring(0, ciudad_edit.Value.IndexOf(",")), ciudad_edit.Value.Substring(ciudad_edit.Value.IndexOf(",") + 1), fono_edit.Value, vendedor_edit.SelectedValue.Trim());

                if (q == "OK")
                {
                    string q2 = ReporteRNegocio.update_cliente2(rutcliente_edit.Text.Replace("-", "").Replace(".", ""), correo_edit.Value);
                    if (q2 == "OK")
                    {
                        h3_.InnerText = "Cliente Editado ! !";
                        codvendedor.Text = vendedor_edit.SelectedValue.Trim();
                    }
                    else
                    {
                        h3_.InnerText = "Error al Editar ! !";
                    }
                }
                else
                {
                    h3_.InnerText = "Error al Editar ! !";
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "te", "<script> alert(no existe vendedor) </script>", false);
            }

        }

        protected void nuevo_cliente_Click(object sender, EventArgs e)
        {
            h1.InnerText = "Crear Cliente";
            l_h1.Text = "";
            CLIENTES_FICHA.ActiveViewIndex = 3;
            DataTable dt; DataView dtv = new DataView();
            string es_vend = ReporteRNegocio.esvendedor(User.Identity.Name);
            string where = " where user1 in (" + GRUPOS_USUARIO + ")";

            if (es_vend == "2")
            {
                where += " and codvendedor = '" + User.Identity.Name + "' ";
            }

            cargar_combo_VENDEDOR_(ReporteRNegocio.listar_ALL_vendedores(where), dtv, es_vend);

            cargar_combo_tipo_negocio(ReporteRNegocio.tipo_negocio(), dtv);

            rutcliente_NEW.Text = "";
            nombre_cliente_NEW.Text = "";
            direccion_NEW.Value = "";
            ciudad_NEW.Value = "";
            fono_NEW.Value = "";
            giro_NEW.Value = "";
            nombre_NEW_1.Value = "";
            correo_NEW_1.Value = "";
            cargo_NEW_1.Value = "";
            fono_NEW_1.Value = "";
            nombre_NEW_2.Value = "";
            correo_NEW_2.Value = "";
            cargo_NEW_2.Value = "";
            fono_NEW_2.Value = "";
            banco_1.Value = "";
            cta_1.Value = "";
            banco_2.Value = "";
            cta_2.Value = "";
            nombre_socie.Value = "";
            rut_socie.Value = "";
            direcc_socie.Value = "";
            ciudad_socie.Value = "";
            fono_socie.Value = "";
            correo_socie.Value = "";
            rut_soc_1.Value = "";
            nombre_soc_1.Value = "";
            correo_soc_1.Value = "";
            porcent_soc_1.Value = "";
            rut_soc_2.Value = "";
            nombre_soc_2.Value = "";
            correo_soc_2.Value = "";
            porcent_soc_2.Value = "";
            t_desde_cliente.Text = "";
            credito_actual.Value = "";
            tipo_credito_actual.Value = "";
            monto_credito_actual.Value = "";
            credito_soli.Value = "";
            tipo_credito_soli.Value = "";
            monto_credito_soli.Value = "";
        }

        private void cargar_combo_tipo_negocio(DataTable dt, DataView dtv)
        {
            dt.Rows.Add(new Object[] { "-1", "-- Otro --" });
            dtv = dt.DefaultView;
            dtv.Sort = "id";
            tipo_negocio.DataSource = dtv;
            tipo_negocio.DataTextField = "nombre_tipo";
            tipo_negocio.DataValueField = "id";
            //d_vendedor_.SelectedIndex = -1;
            tipo_negocio.DataBind();
        }

        protected void crear_cliente_Click(object sender, EventArgs e)
        {




        }
        protected void volver3_ServerClick(object sender, EventArgs e)
        {
            h1.InnerText = "Crear Cliente";


            CLIENTES_FICHA.ActiveViewIndex = 0;
        }


        protected void g_NUEVOS_CLIENTES_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = cont_det.ToString();
                cont_det++;
                string valor = e.Row.Cells[2].Text;
                string rut_ini = valor.Trim().Substring(0, valor.Trim().Length - 1);
                double rut = 0;
                try { rut = double.Parse(rut_ini); e.Row.Cells[2].Text = rut.ToString("N0") + "-" + valor.Trim().Substring(valor.Trim().Length - 1); }
                catch { rut = double.Parse(valor); e.Row.Cells[2].Text = rut.ToString("N0"); }


            }
        }

        protected void g_NUEVOS_CLIENTES_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "ficha")
            {

                DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now;
                t = new DateTime(t.Year, t.Month - 1, 1);
                txt_desde2.Text = t.ToShortDateString();
                txt_hasta2.Text = t2.ToShortDateString();

                string rutcliente = g_NUEVOS_CLIENTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                string nombrecliente = "";
                string direccion = "";
                string ciudad = "";
                string pais = "";
                string fono = "";
                string correo = "";
                string vendedor = "";
                DataTable dato_clientes = ReporteRNegocio.datos_cliente(rutcliente.Trim());
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
                try
                {
                    string rut_ini = rutcliente.Trim().Substring(0, rutcliente.Trim().Length - 1);
                    double rut = double.Parse(rut_ini);

                    rutcliente = rut.ToString("N0") + "-" + rutcliente.Trim().Substring(rutcliente.Trim().Length - 1);
                }
                catch { }
                vendedor_.Text = ReporteRNegocio.nombre_vendedor(vendedor); nombrecliente_.Text = nombrecliente; rutcliente_.Text = rutcliente; direccion_.Text = direccion; fono_.Text = fono; ciudad_.Text = ciudad + ", " + pais;
                codvendedor.Text = vendedor;
                CLIENTES_FICHA.ActiveViewIndex = 1;
                div_historico.Visible = false;
                DIV_CRUZADO.Visible = false;

                muestra_nue.Visible = false;
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            cont_det = 1;
            div9.Visible = true;
            g_NUEVOS_CLIENTES.Visible = true;
            g_NUEVOS_CLIENTES.DataSource = ReporteRNegocio.clientes_nuevos_ficha(GRUPOS_USUARIO);
            g_NUEVOS_CLIENTES.DataBind();
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            contacto_extra = true;
            cont_2.Attributes["style"] = "display:block";
        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            contacto_extra = false; ;
            cont_2.Attributes["style"] = "display:none";
        }
        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            cta_extra = true; ;
            otra_cta_cte.Attributes["style"] = "display:block";
        }

        protected void LinkButton5_Click(object sender, EventArgs e)
        {
            cta_extra = false; ;
            otra_cta_cte.Attributes["style"] = "display:none";
        }

        protected void LinkButton6_Click(object sender, EventArgs e)
        {
            socio_extra = true; ;
            nuevo_socio.Attributes["style"] = "display:block";
        }

        protected void LinkButton7_Click(object sender, EventArgs e)
        {
            socio_extra = false; ;
            nuevo_socio.Attributes["style"] = "display:none";
        }

        protected void Crear_cliente_Click1(object sender, EventArgs e)
        {




            if (rutcliente_NEW.Text == "")
            {
                h1.InnerText = "Faltan RUN!!";
                l_h1.Text = "Faltan RUN!!";
            }

            else if (nombre_cliente_NEW.Text == "")
            {
                h1.InnerText = "Faltan Nombre!!";
                l_h1.Text = "Faltan Nombre!!";
            }
            else
            {
                string busca_rut = ReporteRNegocio.busca_rut_cliente(rutcliente_NEW.Text.Replace("-", "").Replace(".", ""));
                string busca_rut2 = ReporteRNegocio.busca_rut_cliente2(rutcliente_NEW.Text.Replace("-", "").Replace(".", ""));

                if (busca_rut == "" && busca_rut2 == "")
                {

                    try
                    {
                        ciudad = ciudad_NEW.Value.Substring(0, ciudad_NEW.Value.IndexOf(","));
                    }
                    catch { ciudad = ciudad_NEW.Value; }

                    try
                    {
                        if (ciudad_NEW.Value.Contains(","))
                        {

                            pais = ciudad_NEW.Value.Substring(ciudad_NEW.Value.IndexOf(",") + 1);
                        }
                    }
                    catch { pais = ""; }

                    string q = "OK";
                    string q2 = "";
                    if (q == "OK")
                    {
                        q = ReporteRNegocio.insert_cliente2(0, DateTime.Now, rutcliente_NEW.Text.Replace("-", ""), giro_NEW.Value, tipo_negocio.SelectedValue, t_desde_cliente.Text, credito_actual.Value, tipo_credito_actual.Value, monto_credito_actual.Value, credito_soli.Value, tipo_credito_soli.Value, monto_credito_soli.Value, nombre_cliente_NEW.Text, direccion_NEW.Value, ciudad, pais, fono_NEW.Value, vendedor_NNEW.SelectedValue.ToString());
                    }
                    if (q == "OK")
                    {
                        if (contacto_extra)
                        {
                            q = ReporteRNegocio.insert_contactos_cliente(1, rutcliente_NEW.Text.Replace("-", ""), nombre_NEW_1.Value, nombre_NEW_2.Value, correo_NEW_1.Value, correo_NEW_2.Value, cargo_NEW_1.Value, cargo_NEW_2.Value, fono_NEW_1.Value, fono_NEW_2.Value);
                        }
                        else
                        {
                            nombre_NEW_2.Value = "";
                            q = ReporteRNegocio.insert_contactos_cliente(2, rutcliente_NEW.Text.Replace("-", ""), nombre_NEW_1.Value, nombre_NEW_2.Value, correo_NEW_1.Value, correo_NEW_2.Value, cargo_NEW_1.Value, cargo_NEW_2.Value, fono_NEW_1.Value, fono_NEW_2.Value);
                        }

                    }
                    if (q == "OK")
                    {
                        if (cta_extra)
                        {
                            q = ReporteRNegocio.insert_ref_banco_cliente(1, rutcliente_NEW.Text.Replace("-", ""), banco_1.Value, banco_2.Value, cta_1.Value, cta_2.Value);
                        }
                        else
                        {
                            banco_2.Value = "";
                            q = ReporteRNegocio.insert_ref_banco_cliente(2, rutcliente_NEW.Text.Replace("-", ""), banco_1.Value, banco_2.Value, cta_1.Value, cta_2.Value);
                        }

                    }
                    if (q == "OK")
                    {

                        try
                        {
                            ciudad_so = ciudad_socie.Value.Substring(0, ciudad_socie.Value.IndexOf(","));
                        }
                        catch { ciudad_so = ciudad_socie.Value; }

                        try
                        {
                            if (ciudad_socie.Value.Contains(","))
                            {

                                pais_so = ciudad_socie.Value.Substring(ciudad_socie.Value.IndexOf(",") + 1);
                            }
                        }
                        catch { pais = ""; }

                        q = ReporteRNegocio.insert_sociedad_cliente(rutcliente_NEW.Text.Replace("-", ""), rut_socie.Value.Replace("-", "").Replace(".", ""), nombre_socie.Value, direcc_socie.Value, ciudad_so, pais_so, fono_socie.Value, correo_socie.Value);

                    }

                    if (q == "OK")
                    {
                        if (socio_extra)
                        {
                            q = ReporteRNegocio.insert_socios_cliente(1, rutcliente_NEW.Text.Replace("-", ""), rut_soc_1.Value.Replace("-", "").Replace(".", ""), rut_soc_2.Value.Replace("-", "").Replace(".", ""), nombre_soc_1.Value, nombre_soc_2.Value, correo_soc_1.Value, correo_soc_2.Value, porcent_soc_1.Value, porcent_soc_2.Value);
                        }
                        else
                        {
                            rut_soc_2.Value = "";
                            q = ReporteRNegocio.insert_socios_cliente(2, rutcliente_NEW.Text.Replace("-", ""), rut_soc_1.Value.Replace("-", "").Replace(".", ""), rut_soc_2.Value.Replace("-", "").Replace(".", ""), nombre_soc_1.Value, nombre_soc_2.Value, correo_soc_1.Value, correo_soc_2.Value, porcent_soc_1.Value, porcent_soc_2.Value);
                        }

                    }



                    if (q == "OK")
                    {
                        h1.InnerText = "Cliente Creado !";
                        l_h1.Text = "Cliente Creado !";
                        enviar_correo();

                    }
                    else
                    {
                        h1.InnerText = "Error al crear !";
                        l_h1.Text = "Error al crear !";
                    }
                }
                else
                {
                    h1.InnerText = "RUN ya existe !";
                    l_h1.Text = "RUN ya existe !";
                }


            }

        }

        private void enviar_correo()
        {
            DataTable corr = ReporteRNegocio.corr_nuevos_clie();
            string usuario = "";
            string correo = "";
            string cc = "";
            foreach (DataRow r in corr.Rows)
            {
                usuario = r[1].ToString();
                correo = r[3].ToString();
                cc = r[2].ToString();
            }
            string correo1 = "";
            if (vendedor_NNEW.SelectedValue.ToString() != "")
            {
                DataTable corr1 = ReporteRNegocio.corr_usuario(vendedor_NNEW.SelectedValue.ToString());
                string nomusuario1 = "";

                string cc1 = "";
                foreach (DataRow r in corr1.Rows)
                {
                    nomusuario1 = r[0].ToString();
                    correo1 = r[1].ToString();

                }
            }

            DataTable corr2 = ReporteRNegocio.corr_usuario(User.Identity.Name);
            string nomusuario2 = "";
            string correo_user = "";
            foreach (DataRow r in corr2.Rows)
            {
                nomusuario2 = r[0].ToString();
                correo_user = r[1].ToString();

            }


            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(correo));
            string cc2 = "";
            if (correo1 != "")
            {
                cc2 += correo1;
            }
            if (correo_user != "")
            {
                cc2 += "," + correo_user;
            }
            if (correo_user.Trim() == correo1.Trim())
            {
                cc2 = correo_user;
            }

            if (cc2 != "")
            {
                email.CC.Add(cc2+ ","+ correo );
            }
            email.To.Add(new MailAddress(correo_user));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "Nuevo Cliente ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
            //if (cc != "")
            //{
            //    email.CC.Add(cc);
            //}
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

            email.Body += "<div> Estimado " + usuario.Trim() + ":<br> <br> Usuario: <b>" + User.Identity.Name + "</b> ha CREADO un cliente con fecha <b>" + DateTime.Now.ToString("dd/MMM/yyyy") + "</b>";

            string body_correo = crear_cuerpo_correo();
            email.Body += "<div>" + body_correo + "</div>";


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
            }
            catch (Exception ex)
            {

            }
        }

        private string crear_cuerpo_correo()
        {
            string html = "</br>";


            html += "<div style='width:100%'>";
            html += "<table width:100% border=1 cellpadding='8'>";
            html += "<tr>";
            html += "<td colspan=2>";
            html += "Nombre: <b><p style='font-size: 16px;'>" + nombre_cliente_NEW.Text + "</p></b>";
            html += "</td>";
            html += "<td>";
            html += "RUN: " + rutcliente_NEW.Text;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=3 style='background-color:#005D99;'>";
            html += "<b style='color:white'>VENDEDOR</b>";
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=3>";
            string aux = "";
            if (vendedor_NNEW.SelectedValue.ToString() == "") { aux = "Otro"; } else { aux = vendedor_NNEW.SelectedValue.ToString(); }
            html += "Vendedor: " + ReporteRNegocio.nombre_vendedor(vendedor_NNEW.SelectedValue.ToString()) + "(" + aux + ")";
            html += "</td>";
            html += "</tr>";


            html += "<tr>";
            html += "<td colspan=3 style='background-color:#DC1510;'>";
            html += "<b style='color:white'>ANTECEDENTES GENERALES</b>";
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td>";
            html += "Dirección: " + direccion_NEW.Value;
            html += "</td>";
            html += "<td>";
            html += "Ciudad: " + ciudad;
            html += "</td>";
            html += "<td>";
            html += "País: " + pais;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td>";
            html += "Teléfono: " + fono_NEW.Value;
            html += "</td>";
            html += "<td colspan=2>";
            html += "Giro: " + giro_NEW.Value;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=3 style='background-color:#DC1510;'>";
            html += "<b style='color:white'>CONTACTO</b>";
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=2>";
            html += "Nombre: " + nombre_NEW_1.Value;
            html += "</td>";
            html += "<td>";
            html += "Correo: " + correo_NEW_1.Value;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=2>";
            html += "Cargo: " + cargo_NEW_1.Value;
            html += "</td>";
            html += "<td>";
            html += "Teléfono: " + fono_NEW_1.Value;
            html += "</td>";
            html += "</tr>";


            if (contacto_extra)
            {
                html += "<tr>";
                html += "<td colspan=3 style='background-color:#DC1510;'>";
                html += "<b style='color:white'>CONTACTO 2</b>";
                html += "</td>";
                html += "</tr>";

                html += "<tr>";
                html += "<td colspan=2>";
                html += "Nombre2: " + nombre_NEW_2.Value;
                html += "</td>";
                html += "<td>";
                html += "Correo2: " + correo_NEW_2.Value;
                html += "</td>";
                html += "</tr>";

                html += "<tr>";
                html += "<td colspan=2>";
                html += "Cargo2: " + cargo_NEW_2.Value;
                html += "</td>";
                html += "<td>";
                html += "Teléfono2: " + fono_NEW_2.Value;
                html += "</td>";
                html += "</tr>";
            }
            html += "<tr>";
            html += "<td colspan=3 style='background-color:#005D99;'>";
            html += "<b style='color:white'>TIPO NEGOCIO</b>";
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=3>";
            string aux2 = "";
            string q = ReporteRNegocio.trae_tipo_negocio_nombre(tipo_negocio.SelectedValue);
            if (q == "") { aux2 = "Otro"; } else { aux2 = q; }
            html += "Tipo: " + aux2;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=3 style=' background-color:#DC1510;'>";
            html += "<b style='color:white'>REFERENCIA BANCARIA</b>";
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=2>";
            html += "Banco: " + banco_1.Value;
            html += "</td>";
            html += "<td>";
            html += "CTA.CTE.: " + cta_1.Value;
            html += "</td>";
            html += "</tr>";



            if (cta_extra)
            {
                html += "<tr>";
                html += "<td colspan=2>";
                html += "Banco2: " + banco_2.Value;
                html += "</td>";
                html += "<td>";
                html += "CTA.CTE.2: " + cta_2.Value;
                html += "</td>";
                html += "</tr>";
            }

            html += "<tr>";
            html += "<td colspan=3 style='background-color:#005D99;'>";
            html += "<b style='color:white'>ATENCEDENTES DE LA SOCIEDAD</b>";
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=2>";
            html += "Nombre: " + nombre_socie.Value;
            html += "</td>";
            html += "<td>";
            html += "RUN: " + rut_socie.Value;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td>";
            html += "Dirección: " + direcc_socie.Value;
            html += "</td>";
            html += "<td>";
            html += "Ciudad: " + ciudad_so;
            html += "</td>";
            html += "<td>";
            html += "País: " + pais_so;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td>";
            html += "Teléfono: " + fono_socie.Value;
            html += "</td>";
            html += "<td>";
            html += "Correo: " + correo_socie.Value;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=3 style='background-color:#005D99;'>";
            html += "<b style='color:white'>SOCIO</b>";
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td>";
            html += "RUN: " + rut_soc_1.Value;
            html += "</td>";
            html += "<td colspan=2>";
            html += "Nombre: " + nombre_soc_1.Value;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td>";
            html += "Correo: " + correo_soc_1.Value;
            html += "</td>";
            html += "<td>";
            html += "Porcentaje: " + porcent_soc_1.Value;
            html += "</td>";
            html += "</tr>";


            if (socio_extra)
            {
                html += "<tr>";
                html += "<td colspan=3 style='background-color:#005D99;'>";
                html += "<b style='color:white'>SOCIO 2</b>";
                html += "</td>";
                html += "</tr>";

                html += "<tr>";
                html += "<td>";
                html += "RUN2: " + rut_soc_2.Value;
                html += "</td>";
                html += "<td colspan=2>";
                html += "Nombre2: " + nombre_soc_2.Value;
                html += "</td>";
                html += "</tr>";

                html += "<tr>";
                html += "<td>";
                html += "Correo2: " + correo_soc_2.Value;
                html += "</td>";
                html += "<td>";
                html += "Porcentaje2: " + porcent_soc_2.Value;
                html += "</td>";
                html += "</tr>";

            }
            html += "<tr>";
            html += "<td colspan=3 style='background-color:#DC1510;'>";
            html += "<b style='color:white'>ATENCEDENTES SOPRODI S.A.</b>";
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=3>";
            html += "Cliente desde: " + t_desde_cliente.Text;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td>";
            html += "Crédito Actual: " + credito_actual.Value;
            html += "</td>";
            html += "<td>";
            html += "Tipo Crédito Actual: " + tipo_credito_actual.Value;
            html += "</td>";
            html += "<td>";
            html += "Monto Crédito Actual: " + monto_credito_actual.Value;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td>";
            html += "Crédito Solicitado: " + credito_soli.Value;
            html += "</td>";
            html += "<td>";
            html += "Tipo Crédito Solicitado: " + tipo_credito_soli.Value;
            html += "</td>";
            html += "<td>";
            html += "Monto Crédito Solicitado: " + monto_credito_soli.Value;
            html += "</td>";
            html += "</tr>";


            html += "</table>";
            html += "</div>";
            return html;

        }

        protected void btn_productos_Click(object sender, EventArgs e)
        {
            //div6.Attributes["style"] = "display:block";
            div_nuevos.Visible = true;
            g_nuevos_clientes1.Visible = true;
            div_report.Visible = false;
            cont_det = 1;
            string es_vend = ReporteRNegocio.esvendedor(User.Identity.Name);
            if (es_vend == "2")
            {
                g_nuevos_clientes1.DataSource = ReporteRNegocio.clientes_nuevos_ficha(User.Identity.Name);
                g_nuevos_clientes1.DataBind();
            }
            else
            {
                g_nuevos_clientes1.DataSource = ReporteRNegocio.clientes_nuevos_ficha("");
                g_nuevos_clientes1.DataBind();
            }
        }

        protected void g_nuevos_clientes_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //g_nuevos_clientes1.HeaderRow.Cells[4].Attributes["data-sort-method"] = "date";
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = cont_det.ToString();
                cont_det++;
                
                try
                {
                    string valor = e.Row.Cells[2].Text.Replace("-", "").Replace(".", "");
                    string rut_ini = valor.Trim().Substring(0, valor.Trim().Length - 1);
                    double rut = 0;
                    try { rut = double.Parse(rut_ini); e.Row.Cells[2].Text = rut.ToString("N0") + "-" + valor.Trim().Substring(valor.Trim().Length - 1); }
                    catch { rut = double.Parse(valor); e.Row.Cells[2].Text = rut.ToString("N0"); }
                }
                catch { }
                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                e.Row.Cells[1].Attributes["onclick"] = "javascript:fuera('" + encriptador.EncryptData(e.Row.Cells[2].Text.Trim()) + "', '" + encriptador.EncryptData("1") + "');return false;";

            }
        }

        protected void g_nuevos_clientes_RowCommand1(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void b_edSit_Click(object sender, ImageClickEventArgs e)
        {
            DataTable datos_cliente = ReporteRNegocio.datos_cliente_ALL(1, rutcliente_.Text.Replace(".", "").Replace("-", "").Trim());
            if (datos_cliente.Rows.Count > 0)
            {
                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "xte", "<script> fuera('" + encriptador.EncryptData(rutcliente_.Text) + "', '" + encriptador.EncryptData("3") + "') </script>", false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "xte", "<script> alert('No Registra cambios') </script>", false);


            }
        }

        protected void b_doc_abiertos_Click(object sender, ImageClickEventArgs e)
        {

            clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "xwte", "<script> fuera('" + encriptador.EncryptData(rutcliente_.Text) + "', '" + encriptador.EncryptData("25") + "') </script>", false);

        }

        protected void btn_info_clien_Click(object sender, EventArgs e)
        {
            btn_info_clien.Visible = false;
            div_destinos_.Visible = true;
            tx_destinos.Value = tx_enviar_.Text;
        }

        protected void btn_enviar_Click(object sender, EventArgs e)
        {
            btn_info_clien.Visible = true;
            div_destinos_.Visible = false;


            DataTable cr = ReporteRNegocio.corr_usuario(User.Identity.Name);
            foreach (DataRow r in cr.Rows)
            {
                tx_enviar_.Text = r[1].ToString();
            }

            busca_columna_fac = true;
            g_doc.Visible = true;
            string cod_vend = "";
            if (ReporteRNegocio.esvendedor(User.Identity.Name) == "2")
            {
                cod_vend = User.Identity.Name;
            }
            if (cod_vend != "")
            {

                g_doc.DataSource = ReporteRNegocio.docu_abier(rutcliente_.Text.Replace(".", "").Replace("-", ""), cod_vend);

            }
            else
            {
                g_doc.DataSource = ReporteRNegocio.docu_abier(rutcliente_.Text.Replace(".", "").Replace("-", ""), "");
            }
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
            G_CRUZADO.RenderControl(htmlWrite2);

            string body2 = stringWrite2.ToString();

            body2 = body2.Replace("<tr class=\"test no-sort\">", "<tr style='background-color:#428bca;color:#fff'>");



            DataTable corr = ReporteRNegocio.corr_usuario(User.Identity.Name);
            string usuario = "";
            string correo = "";
            foreach (DataRow r in corr.Rows)
            {
                usuario = r[0].ToString();
                correo = r[1].ToString();
            }


            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(tx_destinos.Value));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "Información del Cliente (" + nombrecliente_.Text + " ) ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
            //if (cc != "")
            //{
            //    email.CC.Add(cc);
            //}
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

            email.Body += "<div> Estimado :<br> <br> Productos vendidos dentro de periodos y histórico de documentos cobranzas del cliente:  <b>" + nombrecliente_.Text + "(" + rutcliente_.Text + ") </b> <br><br>";

            email.Body += "<div>" + datos_clientes() + "</div> <br><br>";

            //string body_correo = crear_cuerpo_correo();
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


            //email.Body.Replace("no-sort'>", " style='background-color: #428bca;  color: #fff;'");

            //HTML += "<div style='width: 100%;font-family:Helvetica,Arial,sans-serif;font-size:14px'>";
            //HTML += "<table style='border: 1px solid #ddd; border-collapse: collapse; border-spacing: 1px !important;' border=1>";
            //HTML += "<tr style='background-color: #428bca;  color: #fff;'>";

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
                lb_mensaj.Text = "Correo Enviado!";
                string user = User.Identity.Name;
                //string user = "MCH18";
                string mail_usuario = tx_enviar_.Text;
                string fecha_now = DateTime.Now.ToString();
                string destinos = tx_destinos.Value;

                ReporteRNegocio.insert_log_enviar_ficha(user, mail_usuario, fecha_now, destinos, rutcliente_.Text);
                ///ACA INSERT LOG
            }
            catch (Exception ex)
            {
                lb_mensaj.Text = "Error al enviar";
            }
            g_doc.Visible = false;


        }

        private string datos_clientes()
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
            tabla += "<td>" + vendedor_.Text + "</td>";
            tabla += "<td>" + direccion_.Text + "</td>";
            tabla += "<td>" + ciudad_.Text + "</td>";
            tabla += "<td>" + fono_.Text + "</td>";
            tabla += "<td>" + l_credito.Text + "</td>";
            tabla += "<td>" + ReporteRNegocio.trae_letra_credito(rutcliente_.Text.Replace(".", "").Replace("-", "")).Trim() + "</td>";
            tabla += "</tr>";

            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");
            return tabla;

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            MakeAccessible(g_nuevos_clientes1);
            //MakeAccessible(G_ASIGNADOS);
            //MakeAccessible(G_QR);
            //MakeAccessible(G_LISTA);
        }
        public void JQ_Datatable()
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasad121mp", "<script language='javascript'>creagrilla();</script>", false);
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


        protected void btn_cerrar_Click(object sender, EventArgs e)
        {
            btn_info_clien.Visible = true;
            div_destinos_.Visible = false;
        }

        protected void g_doc_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void b_Click1(object sender, ImageClickEventArgs e)
        {
            string busca_rut = ReporteRNegocio.busca_rut_cliente(rutcliente_NEW.Text.Replace("-", "").Replace(".", ""));
            string busca_rut2 = ReporteRNegocio.busca_rut_cliente2(rutcliente_NEW.Text.Replace("-", "").Replace(".", "")).Trim();

            if (busca_rut == "" && busca_rut2 == "")
            {
                no_muestra.Visible = true;
                h1.InnerText = "Crear Cliente";
            }
            else
            {
                no_muestra.Visible = false;
                h1.InnerText = "RUN ya existe !";
                l_h1.Text = "RUN ya existe !";
            }
        }

    }
}