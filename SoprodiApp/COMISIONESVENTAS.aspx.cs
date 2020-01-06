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
using ThinxsysFramework;
using SoprodiApp.BusinessLayer;
using SoprodiApp.Entities;
//using FTP_Soprodi.BusinessLayer;

using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
//using CreatePDF;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mail;

namespace SoprodiApp
{
    public partial class COMISIONESVENTAS : System.Web.UI.Page
    {
        public static string grupos;
        private static string grupos2;
        private static string USER;
        private static DataTable comisiones_;
        private static DataTable dt_vendedores;
        private static DataTable comisiones_resumen;

        private static DataTable comisiones_sin_com;

        private static DataTable dt_total_resumen_com;
        private static DataTable dt_total_cobranza;


        private static int cont_comisiones;
        public static int COLUMNA_DE_FACTURA;
        public static bool busca_columna_fac;
        public static bool columna_fac;


        protected void Page_Load(object sender, EventArgs e)
        {

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();

            if (!IsPostBack)
            {
                DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now.AddDays(20);
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
                }
                else
                {
                    grupos = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));
                    if (grupos == "")
                    {
                        grupos = agregra_comillas("Abarrotes, CMQuillota, Granos, MayoristasLV");
                    }
                    grupos2 = grupos;
                }
                definir_que_grupos(grupos2);
                //cargar_bodegas();
                //cargar_vendedores();
                cargar_grupos();
                llenar_listado();
                importacion_agroin();
                abarrotes_oficina();
                //div_agroin_amaro.Visible = false;
            }
        }

        private void abarrotes_oficina()
        {
            comisionabarrotesoficinaEntity ce = new comisionabarrotesoficinaEntity();
            ce.cod_periodo = txt_periodo.Text;
            comisionabarrotesoficinaBO.encontrar(ref ce);


            t_total_abarrotes_c_arica.Text = ce.monto_total_abarr.ToString();
            t_ro_.Text = ce.monto_ro.ToString();
            t_total_abarrotes_s_arica.Text = ce.monto_arica.ToString();


        }

        private void definir_que_grupos(string grupos)
        {
            if (grupos.Replace("'", "").Replace(" ", "") == "Abarrotes,CMQuillota,Granos,MayoristasLV")
            {
                Session["puede_ver"] = "Todo";
            }
            else if (grupos.Replace("'", "").Contains("Granos") && grupos.Replace("'", "").Contains("Abarrotes"))
            {
                Session["puede_ver"] = "Todo";
            }
            else if (grupos.Replace("'", "") == "Granos")
            {
                Session["puede_ver"] = "Granos";
            }
            else if (grupos.Contains(","))
            {
                Session["puede_ver"] = "Abarrotes";
            }
        }

        private void importacion_agroin()
        {
            comisionagroinamaroEntity t = new comisionagroinamaroEntity();
            t.cod_periodo = txt_periodo.Text;
            comisionagroinamaroBO.encontrar(ref t);

            txt_dolar.Text = (t.monto_dolar).ToString();
            txt_cambio.Text = (t.tipo_cambio).ToString();
        }

        private void cargar_grupos()
        {

            DataTable dt = ReporteRNegocio.carga_grupos(txt_desde.Text, txt_hasta.Text, grupos);
            //dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
            DataView dv2 = dt.DefaultView;
            dv2.Sort = "user1";
            dt = dv2.ToTable();
            d_grupos_usuario.DataSource = dt;
            d_grupos_usuario.DataTextField = "user1";
            d_grupos_usuario.DataValueField = "user1";
            //d_vendedor_.SelectedIndex = -1;
            d_grupos_usuario.DataBind();
        }

        [WebMethod]
        public static string firmas(string periodo, string user)
        {

            //DataTable firmas = ReporteRNegocio.traefirmas_periodo(periodo);
            DataTable usuarios_firman = ReporteRNegocio.usuarios_firman(periodo);
            //DataTable grupos = ReporteRNegocio.trae_grupo_firmas();

            string HTML = "";
            HTML += "<table id='tabla_firmas' class='table table-advance table-bordered fill-head tablesorter filtrar' style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";
            HTML += "<thead>";

            HTML += "<tr>";

            foreach (DataRow r1 in usuarios_firman.Rows)
            {
                HTML += "<th colspan=1; class='test' style='border-right: 2px solid rgb(50, 48, 48);' >" + r1[0].ToString().Trim() + "</th>";
            }
            HTML += "</tr>";

            HTML += "</thead>";
            HTML += "<tbody>";

            HTML += "<tr>";

            string check_FINANZA_ = "";
            bool enviar_correo_firmas = true;
            foreach (DataRow r1 in usuarios_firman.Rows)
            {
                string usuario = user;
                //click_firma
                string grupo = r1[0].ToString().Trim();

                string script22 = string.Format("javascript:abre_cargando(); click_firma(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;); return false;", usuario, periodo, "0", grupo);
                string script22_no = string.Format("javascript:abre_cargando(); click_firma(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;); return false;", usuario, periodo, "1", grupo);

                string check = "<a onclick=\"" + script22 + "\" style='cursor: pointer;'><img style='width: 26px;' src='img/accept.png'></a>";
                string uncheck = "<a onclick=\"" + script22_no + "\" style='cursor: pointer;'><img style='width: 26px;' src='img/delete.png'></a>";

                if (r1[1].ToString().Trim() == "True")
                {
                    HTML += "<td colspan=1; >" + check + "(" + r1[3].ToString().Trim().ToUpper() + ")</td>";
                }
                else
                {
                    HTML += "<td colspan=1; >" + uncheck + "</td>";
                }
                if (r1[0].ToString().Trim() == "FINANZAS")
                {
                    if (r1[1].ToString().Trim().Contains("True"))
                    {
                        check_FINANZA_ = "True";
                        //ScriptManager.RegisterStartupScript(Page, this.GetType(), "qe", "<script language='javascript'>listado();</script>", false);
                    }
                }
            }
            HTML += "</tr>";
            HTML += "</tbody>";
            HTML += "  </table>";


            /// activa "caracteres"  para identificar que la firma de "finanza" esta activa y se puede enviar correo con el PDF
            if (check_FINANZA_ == "True")
            {
                HTML += "**@**";
            }

            //return div_firmas.InnerHtml = HTML

            return HTML;

        }

        private static void hacervisible_btn()
        {
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "qe", "<script language='javascript'>listado();</script>", false);

        }

        private void llenar_listado()
        {

            DBUtil db = new DBUtil();
            DataTable dt = new DataTable();
            dt = db.consultar(" SELECT [cod_periodo] " +
                              "        , convert(varchar,[fecha_cierre], 103) as fecha_cierre " +
                              "        ,[cod_usuario] " +
                              "        ,[autoriza] " +
                              "        , convert(varchar, fecha_autoriza, 103) as fecha_autoriza  " +
                              " FROM comisionperiodocierre where fecha_cierre between convert(datetime, '" + txt_desde.Text + "', 103) and convert(datetime, '" + txt_hasta.Text + "', 103)  order by convert(datetime, fecha_autoriza,103) desc");
            G_Documentos.DataSource = dt;
            G_Documentos.DataBind();

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd121mk3mp", "<script language='javascript'>listado();</script>", false);
        }

        private void cargar_bodegas()
        {
            DateTime t = DateTime.Now;
            DateTime t2 = DateTime.Now;
            t = new DateTime(t.Year, t.Month, 1);
            string DESDE = t.ToShortDateString();
            string HASTA = t2.ToShortDateString();

            DESDE = DESDE.Replace("-", "/");
            HASTA = HASTA.Replace("-", "/");

            string where = " where FechaFactura >= CONVERT(datetime,'" + DESDE + "', 103) " +
                            " and FechaFactura <= CONVERT(datetime,'" + HASTA + "',103) and user1 in (" + grupos + ")";

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.carga_grupos(DESDE, HASTA, grupos);
            dtv = dt.DefaultView;
        }

        private void cargar_vendedores()
        {
            DateTime t = DateTime.Now;
            DateTime t2 = DateTime.Now;
            t = new DateTime(t.Year, t.Month, 1);
            string DESDE = t.ToShortDateString();
            string HASTA = t2.ToShortDateString();

            DESDE = DESDE.Replace("-", "/");
            HASTA = HASTA.Replace("-", "/");

            string where = " where user1 in (" + grupos + ")";

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.listar_ALL_vendedores(where);
            DataView dv = dt.DefaultView;
            dv.Sort = "nom_vend";
            dtv = dt.DefaultView;

        }

        protected void G_LV_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        private string crear_comiones(string grupos)
        {
            string HTML = "";

            string where = "";

            if (grupos != "") { where += " and user1 in (" + agregra_comillas(grupos) + ")"; }
            if (l_vendedores.Text != "") { where += " and codvendedor in (" + agregra_comillas(l_vendedores.Text) + ")"; }



            where = " and FECHA_PAGO >= CONVERT(datetime,'" + txt_desde.Text + "', 103) " +
                  " and FECHA_PAGO <= CONVERT(datetime,'" + txt_hasta.Text + "',103) ";


            DataTable valores = ReporteRNegocio.trae_comisiones(where);
            DataView view2 = new DataView(valores);
            DataTable vendedores3 = view2.ToTable(true, "vendedor");
            DataView dv32 = vendedores3.DefaultView;
            dv32.Sort = "vendedor";
            vendedores3 = dv32.ToTable();

            DataTable periodos = view2.ToTable(true, "periodo_pago");
            DataView dv3 = periodos.DefaultView;
            dv3.Sort = "periodo_pago";
            periodos = dv3.ToTable();


            HTML += "<table id='TABLA_REPORTE' class='table table-advance table-bordered fill-head tablesorter filtrar' style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";
            HTML += "<thead>";
            HTML += "<tr>";
            HTML += "<th colspan=1; class='test' style='border-right: 2px solid rgb(50, 48, 48);' ></th>";

            foreach (DataRow r in periodos.Rows)
            {
                HTML += "<th colspan=1; class='test' style='border-right: 2px solid rgb(50, 48, 48);' >" + r[0].ToString().Trim() + "</th>";
            }

            HTML += "</tr>";
            HTML += "</thead>";
            HTML += "<tbody>";

            foreach (DataRow r2 in vendedores3.Rows)
            {

                HTML += "<tr>";
                HTML += "<td colspan=1; class='test'>" + r2[0].ToString().Trim() + "</td>";

                foreach (DataRow r in periodos.Rows)
                {
                    DataRow[] comision = valores.Select("periodo_pago = '" + r[0].ToString().Trim() + "' and vendedor = '" + r2[0].ToString().Trim() + "'");

                    if (comision.Length == 0)
                    {
                        HTML += "<td colspan=1; class=''>0</td>";

                    }
                    else
                    {

                        double sum_comision = 0;
                        foreach (DataRow row in comision)
                        {

                            sum_comision += double.Parse(row[2].ToString());



                        }

                        sum_comision = Math.Round(sum_comision);

                        clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                        string script2 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;,&#39;{2}&#39;, &#39;{3}&#39;)", encriptador.EncryptData(""), encriptador.EncryptData(r[0].ToString().Trim()), encriptador.EncryptData(r2[0].ToString().Trim()), encriptador.EncryptData("2018"));
                        HTML += "<td colspan=1;> <a data-toggle='tooltip' data-placement='bottom' title='" + r[0].ToString().Trim() + "' href='javascript:' onclick='" + script2 + "'> " + Base.monto_format2(sum_comision) + " </a> </td>";



                    }

                }


                HTML += "</tr>";
            }


            //HTML += "<tr>";
            //HTML += "<td colspan=1;  class='' >PERIODO 2</td>";

            ////string script1 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;)", encriptador.EncryptData(""), encriptador.EncryptData(""));
            ////HTML += "<td colspan=1;> <a data-toggle='tooltip' data-placement='bottom' title='toongleee_aqui' href='javascript:' onclick='" + script2 + "'> TEXTOOO AQUII </a> </td>";
            ////HTML += "<td colspan=1;> <a data-toggle='tooltip' data-placement='bottom' title='toongleee_aqui' href='javascript:' onclick='" + script2 + "'> TEXTOOO AQUII </a> </td>";
            ////HTML += "<td colspan=1;> <a data-toggle='tooltip' data-placement='bottom' title='toongleee_aqui' href='javascript:' onclick='" + script2 + "'> TEXTOOO AQUII </a> </td>";

            //HTML += "</tr>";

            HTML += "</tbody>";
            HTML += "  </table>";
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
        public static string guardar_firma(string usuario_, string periodo_, string si_no, string grupo_)
        {
            string ok = "";

            ok = ReporteRNegocio.eliminar_firma(usuario_, periodo_, si_no, grupo_);
            
            return ok;
        }


        [WebMethod]
        public static string CAMBIA_ESTADO_ON_OFF(string cod_periodo, string vendedor, string factura, string producto, string regla, string check, string porcentaje)
        {


            comisionperiodocierre_productosEntity t = new comisionperiodocierre_productosEntity();
            t.cod_periodo = cod_periodo;
            t.vendedor = vendedor;
            t.númfactura = factura;
            t.producto = producto;
            t.cod_regla = regla;
            t.porcentaje = Convert.ToDouble(porcentaje);
            if (check == "True")
            {

                string r2 = comisionperiodocierre_productosBO.agregar(t);
                if (r2 == "1" || r2 == "2")
                {




                }
            }
            else
            {

                string r2 = comisionperiodocierre_productosBO.eliminar(t);
                if (r2 == "1" || r2 == "2")
                {




                }

            }




            string ok = "OK";
            return ok;
        }

        [WebMethod]
        public static List<VENDEDOR> GRUPO_VENDEDOR(string grupos, string desde, string hasta)
        {
            DataTable dt = new DataTable();
            string grupos_ = agregra_comillas2(grupos);

            desde = desde.Replace("-", "/");
            hasta = hasta.Replace("-", "/");

            string where = " where FechaFactura >= CONVERT(datetime,'" + desde + "', 103) " +
                             " and FechaFactura <= CONVERT(datetime,'" + hasta + "',103)";

            if (grupos != "")
            {
                where += " and user1 in (" + grupos_ + ")";
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

        protected void btn_nuevo_Click(object sender, EventArgs e)
        {
            nuevo();
        }

        protected void btn_nuevo_ServerClick(object sender, EventArgs e)
        {
            nuevo();
        }

        protected void nuevo()
        {
            btn_refresh.Visible = false;
            B_GuardarN.Visible = false;
            b_total_.Visible = false;
            b_vovler.Visible = false;
            filtra_vendedor.Visible = false;

            btn_crear_periodo.Visible = true;


            txt_periodo.Text = "";
            txt_fecha.Text = "";
            txt_fecha_autoriza.Text = "";
            txt_nom_autoriza.Text = "";
            llenar_facturas("x", "", 2);

            p_enc.Visible = false;
            p_det.Visible = true;
        }

        protected void B_GuardarN_ServerClick(object sender, EventArgs e)
        {
            comisionperiodocierreEntity c = new comisionperiodocierreEntity();
            string mes = DateTime.Parse(txt_fecha.Text).Month.ToString();
            if (mes.Length < 2)
            {

                mes = "0" + mes;
            }

            c.cod_periodo = DateTime.Parse(txt_fecha.Text).Year.ToString() + mes.Trim();
            c.cod_usuario = User.Identity.Name;
            c.fecha_cierre = DateTime.Parse(txt_fecha.Text);
            c.autoriza = User.Identity.Name;
            c.fecha_autoriza = DateTime.Parse(txt_fecha.Text);
            string r = comisionperiodocierreBO.agregar(c);
            if (r == "1" || r == "2")
            {
                //aca poblar tabla [ComisionPeriodoCierre] y [ComisionPeriodoCierre_Productos]
                comisionperiodocierre_productosEntity t = new comisionperiodocierre_productosEntity();
                t.cod_periodo = c.cod_periodo;
                rd_todos.Checked = true;

                txt_periodo.Text = c.cod_periodo;
                llenar_facturas(txt_periodo.Text, "", 2);

                foreach (DataRow row1 in comisiones_.Rows)
                {
                    t.númfactura = row1["númfactura"].ToString();
                    t.producto = row1["producto"].ToString();
                    t.porcentaje = Convert.ToDouble(row1["porcentaje"].ToString());
                    t.vendedor = row1["codvendedor"].ToString();
                    t.cod_regla = row1["regla"].ToString();
                    //t.porcentaje_edit = null;

                    string r2 = comisionperiodocierre_productosBO.agregar(t);
                    if (r2 == "1" || r2 == "2")
                    {

                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "t12ee3ee", "<script language='javascript'>alert('Guardado');</script>", false);


                    }
                }



            }
        }

        protected void G_Documentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Visualizar")
                {
                    string periodo = (G_Documentos.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    //completar_no_comisiones(periodo);
                    completar(periodo);


                }
                if (e.CommandName == "Borrar")
                {
                    string periodo = (G_Documentos.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    completar(periodo);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void completar_no_comisiones(string periodo, string where1, string where2)
        {
            DBUtil db = new DBUtil();
            DataTable dt = new DataTable();

            dt = ReporteRNegocio.fact_sin_comision(periodo, where1, where2);

            Session["comisiones_sin_com"] = dt;
            G_NOMINA_SN_COMI.DataSource = dt;
            G_NOMINA_SN_COMI.DataBind();

            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "t3mp", "<script language='javascript'>firmas_j('" + p + "', '" + User.Identity.Name + "');</script>", false);

            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "id_var_2", "<script language='javascript'>tabla_refresh();</script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teas1d121m123k3mp", "<script language='javascript'>TABLITA();</script>", false);




        }

        protected void completar(string periodo)
        {

            btn_refresh.Visible = true;
            B_GuardarN.Visible = true;
            b_total_.Visible = true;
            b_vovler.Visible = true;
            filtra_vendedor.Visible = true;

            div_agroin_amaro.Visible = true;

            btn_crear_periodo.Visible = false;

            comisionperiodocierreEntity c = new comisionperiodocierreEntity();
            c.cod_periodo = periodo;
            if (comisionperiodocierreBO.encontrar(ref c) == "OK")
            {
                txt_periodo.Text = c.cod_periodo;
                txt_fecha.Text = c.fecha_cierre.ToString("yyyy-MM-dd");
                txt_nom_autoriza.Text = c.autoriza;
                txt_fecha_autoriza.Text = c.fecha_autoriza.ToString("yyyy-MM-dd");
                rd_todos.Checked = true;
                llenar_facturas(txt_periodo.Text, "", 2);
                p_enc.Visible = false;
                p_det.Visible = true;


                LlenarComboVendedor();
                Llenar_reglas();
                importacion_agroin();
                abarrotes_oficina();

            }
        }

        private void Llenar_reglas()
        {
            DataTable dt2 = ReporteRNegocio.trae_reglas_2(" where codvendedor = '" + c_vendedor.SelectedValue + "'");
            dt2.Rows.Add(new Object[] { " -- Todos -- ", " -- Todos -- " });
            DataView dv2 = dt2.DefaultView;
            dv2.Sort = "cod_comision";
            c_reglas.DataSource = dv2;
            c_reglas.DataTextField = "cod_comision";
            c_reglas.DataValueField = "cod_comision";
            c_reglas.DataBind();
            c_reglas.SelectedIndex = 1;

        }

        private void LlenarComboVendedor()
        {

            DBUtil db = new DBUtil();
            DataTable dt = new DataTable();

            string where = "  where 1=1 ";

            dt = db.consultar("SELECT DISTINCT codvendedor, isnull(NOMBRE,codvendedor) as nombre  FROM [dbo].[V_COMISION_VENDEDORES] " + where);


            //dt.Rows.Add(new Object[] { -1, "-- Seleccione --" });
            DataView dv = dt.DefaultView;
            dv.Sort = "NOMBRE asc";
            DataTable sortedDT = dv.ToTable();

            dt_vendedores = sortedDT;

            c_vendedor.DataSource = sortedDT;
            c_vendedor.DataValueField = "codvendedor";
            c_vendedor.DataTextField = "NOMBRE";
            c_vendedor.DataBind();
            c_vendedor.SelectedIndex = 1;



            c_vendedor_2.DataSource = sortedDT;
            c_vendedor_2.DataValueField = "codvendedor";
            c_vendedor_2.DataTextField = "NOMBRE";
            c_vendedor_2.DataBind();
            c_vendedor_2.SelectedIndex = 1;
        }

        private void llenar_facturas(string p, string where, int int_on_off)
        {
            total_comision2 = 0;
            l_total_comision.Text = total_comision2.ToString();
            DBUtil db = new DBUtil();
            DataTable dt = new DataTable();

            string grupo_para_in = agregra_comillas2(grupos2);
            string grupos = agregra_comillas(l_grupos.Text); ;

            if (grupos != "")
            {
                grupo_para_in = grupos;
                where += " and grupo in (" + grupo_para_in + ") ";
            }

            if (int_on_off == 1)
            {
                dt = db.consultar(" SELECT * FROM V_COMISIONES_FACTURAS where  grupo in (" + grupo_para_in + ") and  CONVERT(DATE,FECHA_PAGO) <= CONVERT(DATE, '" + txt_fecha.Text + "') and (cod_periodo = '" + p + "'" + where + ")");
            }
            else if (int_on_off == 0)
            {
                dt = db.consultar(" SELECT * FROM V_COMISIONES_FACTURAS where  grupo in (" + grupo_para_in + ") and  CONVERT(DATE,FECHA_PAGO) <= CONVERT(DATE, '" + txt_fecha.Text + "') and (cod_periodo is null " + where + ") ");
            }
            else
            {
                dt = db.consultar(" SELECT * FROM V_COMISIONES_FACTURAS where  grupo in (" + grupo_para_in + ") and  CONVERT(DATE,FECHA_PAGO) <= CONVERT(DATE, '" + txt_fecha.Text + "') and (cod_periodo is null " + where + ") or (  grupo in (" + grupo_para_in + ") and cod_periodo = '" + p + "'" + where + ")");
            }
            //firmas(p);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "t3mp", "<script language='javascript'>firmas_j('" + p + "', '" + User.Identity.Name + "');</script>", false);

            comisiones_ = dt;
            cont_comisiones = 0;
            busca_columna_fac = true;
            G_Nomina.DataSource = dt;
            G_Nomina.DataBind();

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd121m123k3mp", "<script language='javascript'>tabla_refresh();</script>", false);

            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd121mk3mp", "<script language='javascript'>tabla();</script>", false);
        }

        private void llenar_facturas_resumen(string p, string where, int int_on_off)
        {
            total_comision2 = 0;
            l_total_comision.Text = total_comision2.ToString();
            DBUtil db = new DBUtil();
            DataTable dt = new DataTable();

            string grupo_para_in = agregra_comillas2(grupos2);
            string grupos = agregra_comillas(l_grupos.Text); ;

            if (grupos != "")
            {
                grupo_para_in = grupos;
            }

            if (int_on_off == 1)
            {
                dt = db.consultar(" SELECT * FROM V_COMISIONES_FACTURAS where grupo in (" + grupo_para_in + ") and  CONVERT(DATE,FECHA_PAGO) <= CONVERT(DATE, '" + txt_fecha.Text + "') and (cod_periodo = '" + p + "'" + where + ")  AND Agregar = 1");
            }
            else if (int_on_off == 0)
            {
                dt = db.consultar(" SELECT * FROM V_COMISIONES_FACTURAS where  grupo in (" + grupo_para_in + ") and  CONVERT(DATE,FECHA_PAGO) <= CONVERT(DATE, '" + txt_fecha.Text + "') and (cod_periodo is null " + where + ")  AND Agregar = 1");
            }
            else
            {
                dt = db.consultar(" SELECT * FROM V_COMISIONES_FACTURAS where  grupo in (" + grupo_para_in + ") and  CONVERT(DATE,FECHA_PAGO) <= CONVERT(DATE, '" + txt_fecha.Text + "') and (cod_periodo is null " + where + ")  AND Agregar = 1 or (  grupo in (" + grupo_para_in + ") and cod_periodo = '" + p + "'" + where + "  AND Agregar = 1)");
            }

            comisiones_resumen = dt;
            cont_comisiones = 0;

            //25% de venta de agroin (se llena en caja de texto separada)
            dt = db.consultar(" SELECT ROUND ( ( (ROUND([monto_dolar] * [tipo_cambio], 1) ) *0.25  )  / 100 ,0 )  FROM ComisionAgroinAmaro where cod_periodo = '" + p + "'");
            try
            {
                Session["comision_amaro_agroin"] = Convert.ToDouble(dt.Rows[0][0].ToString());
            }
            catch
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd121m123k3mp", "<script language='javascript'>alert('Error Agroin AMARO');</script>", false);
            }


            //abarotes azocar Y cataldo
            dt = db.consultar("SELECT * FROM V_COMISION_ABARROTES_OFICINA where cod_periodo = '" + p + "'");
            try
            {
                Session["DT_ABARROTES_OFICINA"] = dt;
            }
            catch
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd121m123k3mp", "<script language='javascript'>alert('Error Agroin AMARO');</script>", false);
            }



        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            MakeAccessible(G_Documentos);
            MakeAccessible(G_Nomina);
            MakeAccessible(G_NOMINA_SN_COMI);

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

        protected void b_vovler_ServerClick(object sender, EventArgs e)
        {
            llenar_listado();
            p_enc.Visible = true;
            p_det.Visible = false;
        }

        protected void btn_productos_Click(object sender, EventArgs e)
        {
            llenar_listado();
        }

        private static string agregar_comillas(string p)
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

        protected void filtra_vendedor_ServerClick(object sender, EventArgs e)
        {
            //FILTRAR VENDEDOR
            try
            {
                string codvendedor = c_vendedor.SelectedValue.ToString();
                string reglas = agregar_comillas(ReporteRNegocio.trae_reglas(codvendedor));



                string mes = DateTime.Parse(txt_fecha.Text).Month.ToString();
                if (mes.Length < 2)
                {

                    mes = "0" + mes;
                }

                string periodo = DateTime.Parse(txt_fecha.Text).Year.ToString() + mes;


                if (codvendedor == "CA001")

                {
                    //[dbo].[ComisionAgroinAmaro]
                    comisionagroinamaroEntity t = new comisionagroinamaroEntity();
                    t.cod_periodo = periodo;
                    comisionagroinamaroBO.encontrar(ref t);

                    txt_dolar.Text = (t.monto_dolar).ToString();
                    txt_cambio.Text = (t.tipo_cambio).ToString();


                    //agroin_amaro_.Visible = true;

                    div_agroin_amaro.Visible = true;
                }
                else
                {
                    txt_dolar.Text = "";
                    txt_cambio.Text = "";
                    div_agroin_amaro.Visible = true;

                }


                string reglas_ = agregar_comillas(l_regla.Text);
                if (reglas_ == "")
                {
                    c_reglas.SelectedIndex = 1;
                }

                string regla_aux = reglas_;

                string where_regla = "";

                int on_off_todos = 0;
                if (rd_on.Checked)
                {
                    on_off_todos = 1;
                }

                if (rd_off.Checked)
                {
                    on_off_todos = 0;

                }
                if (rd_todos.Checked)
                {
                    on_off_todos = 2;

                }
                l_sobre_comision.Text = "";
                l_total_comision.Text = "";

                string codvendedor_aux = codvendedor;
                if (codvendedor == "JAIME BARGETTO")
                {
                    codvendedor = "AN044";
                    reglas = agregar_comillas(ReporteRNegocio.trae_reglas(codvendedor));
                }


                if (codvendedor != "CA001")
                {
                    if (codvendedor != "GT015")
                    {

                        if (codvendedor != "PARRA")
                        {
                            if (reglas_ != "'-- Todos --'")
                            {
                                where_regla += " and regla in (" + reglas_ + ")";
                            }
                            else
                            {

                                where_regla += " and regla in (" + reglas + ")";

                            }
                            where_regla += "  AND CODVENDEDOR = '" + codvendedor + "'";
                        }
                    }
                }

                if (codvendedor == "CA001")
                {
                    //regla 11
                    if (reglas_ != "'-- Todos --'")
                    {
                        if (reglas_ == "'Regla8'")
                        {
                            where_regla = " and regla in ('regla8')";

                        }
                        else if (reglas_ == "'Regla4'")
                        {
                            where_regla = " and regla in ('regla4')";
                        }
                        else if (reglas_ == "'Regla11'")
                        {
                            where_regla = " and regla in ('Regla11')";
                        }
                        else if (reglas_ == "'regla19'")
                        {
                            where_regla = " and regla in ('regla19')";
                        }
                        else
                        {
                            where_regla += " and regla in (" + reglas_ + ")";
                            where_regla += "  AND CODVENDEDOR = '" + codvendedor + "'";

                        }
                    }
                    else
                    {
                        reglas_ = reglas;
                        where_regla += " and ( regla in (" + reglas_ + ")  AND CODVENDEDOR = '" + codvendedor + "' or regla in ('regla4' , 'regla8', 'regla11', 'regla19' ) )";

                    }

                }

                if (codvendedor == "GT015")
                {
                    if (reglas_ != "'-- Todos --'")
                    {
                        if (reglas_ == "'Regla8'")
                        {
                            where_regla = " and regla in ('regla8')";

                        }
                        else if (reglas_ == "'Regla10'")
                        {
                            where_regla = " and regla in ('regla10')";
                        }
                        else if (reglas_ == "'Regla13'")
                        {
                            where_regla = " and regla in ('regla13')";
                        }
                        else
                        {
                            where_regla += " and regla in (" + reglas_ + ")";
                            where_regla += "  AND CODVENDEDOR = '" + codvendedor + "'";
                        }
                    }
                    else
                    {
                        reglas_ = reglas;
                        where_regla += "  and ( regla in (" + reglas_ + ")  AND CODVENDEDOR = '" + codvendedor + "' or regla in ('regla10', 'regla8', 'regla18', 'regla13', 'regla21') ) ";

                    }

                }






                if (codvendedor == "PARRA")
                {
                    if (reglas_ != "'-- Todos --'")
                    {
                        if (reglas_ == "'Regla9'")
                        {
                            where_regla = " and regla in ('regla9')";

                        }
                        else if (reglas_ == "'Regla7'")
                        {
                            where_regla = " and regla in ('regla7')";
                        }
                        else if (reglas_ == "'Regla17'")
                        {
                            where_regla = " and regla in ('regla17')";
                        }
                        else
                        {
                            where_regla += " and regla in (" + reglas_ + ")";

                        }
                    }
                    else
                    {
                        reglas_ = reglas;
                        where_regla += "  and ( regla in (" + reglas_ + ") or regla in ('regla7', 'regla9', 'regla17') )";

                    }

                }




                llenar_facturas(periodo, where_regla, on_off_todos);

                try
                {
                    DataTable dt2 = ReporteRNegocio.trae_reglas_2(" where codvendedor = '" + codvendedor_aux + "'");
                    dt2.Rows.Add(new Object[] { " -- Todos -- ", " -- Todos -- " });
                    DataView dv2 = dt2.DefaultView;
                    dv2.Sort = "cod_comision";
                    c_reglas.DataSource = dv2;
                    c_reglas.DataTextField = "cod_comision";
                    c_reglas.DataValueField = "cod_comision";
                    c_reglas.DataBind();


                    foreach (System.Web.UI.WebControls.ListItem item in c_reglas.Items)
                    {

                        if (l_regla.Text == item.Value.ToString())
                        {
                            item.Selected = true;
                        }
                    }
                }
                catch { }


                // total comisiones
                if (regla_aux == "'-- Todos --'")
                {
                    DataTable totales = new DataTable();

                    totales.Columns.Add("NombreRegla");
                    totales.Columns.Add("Total");

                    DataRow row;

                    List<string> regla = reglas.Split(',').ToList();
                    Double tota_comision_vendedor = 0;
                    foreach (string reg1a in regla)
                    {
                        row = totales.NewRow();

                        DataView dv = new DataView(comisiones_);
                        dv.RowFilter = "regla = " + reg1a; // query example = "id = 10"
                        DataTable filtradacomision = dv.ToTable();
                        if (filtradacomision.Rows.Count > 0)
                        {
                            double total_por_regla = 0;
                            string nombre_regla = "";
                            foreach (DataRow r1 in filtradacomision.Rows)
                            {

                                if (r1["porcentaje_edit"].ToString() != "")
                                {
                                    double neto = Convert.ToDouble(r1["neto_pesos"].ToString());
                                    double porncetaje = Convert.ToDouble(r1["porcentaje_edit"].ToString());
                                    double comision_editada = Math.Round((neto * porncetaje) / 100);
                                    total_por_regla += comision_editada;
                                }
                                else
                                {
                                    total_por_regla += Convert.ToDouble(r1["montocomision"]);
                                }
                                nombre_regla = r1["nombre_comision"].ToString();
                            }


                            if (nombre_regla == "COMISION SOBRE COMISIONES  RAFAEL PINO, ANDRES GONZALES")
                            {

                                //l_sobre_comision.Text = "30% sobre comisión: " + Base.monto_format2(Math.Round(total_por_regla * 0.25));
                                //row["NombreRegla"] = nombre_regla;
                                //row["Total"] = Math.Round(total_por_regla * 0.25);
                                //tota_comision_vendedor += Math.Round(total_por_regla * 0.25);

                                row["NombreRegla"] = nombre_regla;
                                row["Total"] = total_por_regla;
                                tota_comision_vendedor += total_por_regla;
                            }
                            else
                            {
                                row["NombreRegla"] = nombre_regla;
                                row["Total"] = total_por_regla;
                                tota_comision_vendedor += total_por_regla;
                            }
                            totales.Rows.Add(row);
                        }
                    }

                    row = totales.NewRow();
                    row["NombreRegla"] = "::::TOTAL:::: ";
                    row["Total"] = Math.Round(tota_comision_vendedor);
                    totales.Rows.Add(row);

                    if (codvendedor_aux == "AN044")
                    {
                        string neto_navarro = ReporteRNegocio.neto_navarro(periodo);
                        row = totales.NewRow();
                        row["NombreRegla"] = "_:NETO:_";
                        row["Total"] = Base.monto_format(neto_navarro);
                        totales.Rows.Add(row);
                    }

                    string tabla = "";
                    tabla += "<table class=\"table fill-head table-bordered\">";
                    tabla += "<thead class=\"test\">";
                    tabla += "<tr>";
                    tabla += "<th>Nombre de Regla</th>";
                    tabla += "<th>Total</th>";
                    tabla += "</tr>";
                    tabla += "</thead>";
                    tabla += "<tbody>";

                    if (codvendedor_aux == "JAIME BARGETTO")
                    {

                        tabla += "<tr>";
                        tabla += "<td> COMISION SOBRE COMISIONES ANDRES NAVARRO</td>";
                        tabla += "<td>" + l_sobre_comision.Text + "</td>";
                        tabla += "</tr>";
                    }
                    else
                    {
                        foreach (DataRow r2 in totales.Rows)
                        {
                            tabla += "<tr>";
                            tabla += "<td>" + r2["NombreRegla"].ToString() + "</td>";
                            tabla += "<td>" + Base.monto_format2(Convert.ToDouble(r2["Total"].ToString())) + "</td>";
                            tabla += "</tr>";
                        }

                        if (codvendedor_aux == "AN044")
                        {
                            string neto_navarro = ReporteRNegocio.neto_navarro(periodo);
                            tabla += "<tr style='background-color: lavender; '>";
                            tabla += "<td> NETO: </td>";
                            tabla += "<td>" + Base.monto_format(neto_navarro) + "</td>";
                            tabla += "</tr>";
                        }
                    }


                    //if (codvendedor_aux == "CA001")
                    //{

                    //    tabla += "<tr>";
                    //    tabla += "<td>COMISION SOBRE COMISIONES RAFAEL PINO, ANDRES GONZALES</td>";
                    //    tabla += "<td>" + l_sobre_comision.Text + "</td>";
                    //    tabla += "</tr>";
                    //}


                    tabla += "</tbody>";
                    tabla += "</table>";

                    div_tabla_totales.InnerHtml = tabla;

                    UpdatePanel2.Update();


                    string aca = "";
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>click_modal();</script>", false);


                }


            }
            catch (Exception ex)

            {

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>alert('Falta fecha');</script>", false);
            }
        }

        public class REGLA
        {
            public string regla { get; set; }
            public string nombre_regla { get; set; }
        }



        [WebMethod]
        public static List<REGLA> reglas(string vendedor)
        {

            DataTable dt = new DataTable();
            string vende = agregra_comillas2(vendedor);

            string where = " where 1=1 ";

            if (!vende.Contains("'-1'") && vende != "" && !vende.Contains("'-- Todos --'"))
            {
                where += " and codvendedor in (" + vende + ")";
            }

            if (vende == "")
            {
                where += " and 1=1 ";
            }

            dt = ReporteRNegocio.trae_reglas_2(where);
            dt.Rows.Add(new Object[] { " -- Todos -- ", " -- Todos -- " });

            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new REGLA
                        {
                            regla = Convert.ToString(item["cod_comision"]),
                            nombre_regla = Convert.ToString(item["cod_comision"])
                        };

            return query.ToList<REGLA>();
            

        }
        public static double total_comision2;
        public static double porcentaje;
        protected void G_Nomina_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                try
                {
                    //string agregar = e.Row.Cells[9].Text;
                    //string regla = e.Row.Cells[9].Text;
                    double porncetaje_actual = 0;
                    double neto = 0;
                    double comision_final = 0;

                    try
                    {
                        comision_final = Convert.ToDouble(e.Row.Cells[10].Text);
                        porncetaje_actual = Convert.ToDouble(e.Row.Cells[7].Text);
                        neto = Convert.ToDouble(e.Row.Cells[6].Text);
                    }
                    catch
                    {

                    }

                    comisionperiodocierre_productosEntity t = new comisionperiodocierre_productosEntity();
                    t.cod_periodo = e.Row.Cells[12].Text.Trim();
                    t.vendedor = e.Row.Cells[4].Text.Trim();
                    t.númfactura = e.Row.Cells[0].Text.Trim();
                    t.producto = e.Row.Cells[1].Text.Trim();
                    t.cod_regla = e.Row.Cells[8].Text.Trim();
                    t.porcentaje = porncetaje_actual;


                    string tipo_doc = e.Row.Cells[15].Text.Trim();

                    if (tipo_doc != "IN")
                    {

                        string ACA = "";
                    }
                    if (t.númfactura == "3017235")
                    {
                        string ACA2 = "";
                    }



                    string descr = e.Row.Cells[17].Text.Trim();

                    string id_para_cm = "";

                    if (descr.Contains("F:"))
                    {

                        id_para_cm = descr.Substring(descr.IndexOf("F:")).Replace("F:", "").Trim();
                    }
                    else
                    {
                        id_para_cm = descr;
                    }

                    string agregar2 = e.Row.Cells[12].Text;
                    string cod_periodo2 = "";
                    if (agregar2 == "&nbsp;")
                    {

                        string script = string.Format("javascript:click_cambia(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;, &#39;{4}&#39;, &#39;{5}&#39;);return false;", cod_periodo2, t.vendedor, t.númfactura, t.producto, t.cod_regla, porncetaje_actual);
                        e.Row.Cells[14].Text = "             <input type='checkbox' name='checkboxG1' onchange='" + script + "'  " +
                                "   id='" + cod_periodo2 + t.vendedor + t.númfactura + t.producto + t.cod_regla + "' class='css-checkbox' />";


                        e.Row.Cells[18].Text = "";

                    }
                    else
                    {



                        cod_periodo2 = agregar2;
                        string script = string.Format("javascript:click_cambia(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;, &#39;{4}&#39;, &#39;{5}&#39;);return false;", cod_periodo2, t.vendedor, t.númfactura, t.producto, t.cod_regla, porncetaje_actual);

                        e.Row.Cells[14].Text = "         <input type='checkbox' name='checkboxG1' onchange='" + script + "'  " +
                               "   id='" + cod_periodo2 + t.vendedor + t.númfactura + t.producto + t.cod_regla + "' checked class='css-checkbox' />";




                        string porcentaje_editado = e.Row.Cells[18].Text;



                        if (porcentaje_editado != "&nbsp;")
                        {
                            try
                            {
                                double porcentaje_editado_ = Convert.ToDouble(porcentaje_editado);
                                double comision_editada = Math.Round((neto * porcentaje_editado_) / 100);
                                comision_final = comision_editada;
                                e.Row.Cells[10].Text = "<p style='color:red'>" + comision_editada.ToString() + " </p>";

                            }
                            catch
                            {

                            }

                            string script1 = string.Format("javascript:guarda_txt(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;, &#39;{4}&#39;, &#39;{5}&#39;, &#39;{6}&#39;);return false;", cod_periodo2, t.vendedor, t.númfactura, t.producto, t.cod_regla, porncetaje_actual, id_para_cm);
                            //guarda_txt(" + cod_periodo2 + ", " + t.vendedor + ", " + t.númfactura + ", " + t.producto + ", " + t.cod_regla + 
                            string txt_porcentaje = " <table style='width:100%'> <tr> <td style='width: 70%;'> <div> <input type='text' id='txt_porcentaje_" + cod_periodo2 + t.vendedor + t.númfactura + t.producto + t.cod_regla + id_para_cm + "' value='" + porcentaje_editado.Trim() + "' style='width: 100%; display:block;'> ";
                            txt_porcentaje += "</td>                                  <td style='padding: 0px 0px;'><div onclick='" + script1 + "'); ' style='width:20%;cursor: pointer;'><i class='fa fa-save fa-2x'></i></div>";
                            txt_porcentaje += " </div> </td> </tr></table> ";
                            e.Row.Cells[18].Text = txt_porcentaje;


                        }
                        else
                        {

                            string script1 = string.Format("javascript:guarda_txt(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;, &#39;{4}&#39;, &#39;{5}&#39;, &#39;{6}&#39;);return false;", cod_periodo2, t.vendedor, t.númfactura, t.producto, t.cod_regla, porncetaje_actual, id_para_cm);
                            //guarda_txt(" + cod_periodo2 + ", " + t.vendedor + ", " + t.númfactura + ", " + t.producto + ", " + t.cod_regla + 
                            string txt_porcentaje = " <table style='width:100%'> <tr> <td style='width: 70%;'> <div> <input type='text' id='txt_porcentaje_" + cod_periodo2 + t.vendedor + t.númfactura + t.producto + t.cod_regla + id_para_cm + "' value='' style='width: 100%; display:block;'> ";
                            txt_porcentaje += "</td>                                  <td style='padding: 0px 0px;'><div onclick='" + script1 + "'); ' style='width:20%;cursor: pointer;'><i class='fa fa-save fa-2x'></i></div>";
                            txt_porcentaje += " </div> </td> </tr></table> ";
                            e.Row.Cells[18].Text = txt_porcentaje;


                        }


                    }

                    try
                    {


                        double comision = comision_final;
                        total_comision2 += comision;



                    }
                    catch (Exception ex)
                    {
                        string aca = "";
                    }




                    cont_comisiones++;
                    if (cont_comisiones == comisiones_.Rows.Count)
                    {
                        l_total_comision.Text = Base.monto_format2(total_comision2);
                        if (l_regla.Text == "Regla11")
                        {

                            l_sobre_comision.Text = "30% sobre comisión: " + Base.monto_format2(total_comision2 * 0.25);
                        }
                        else
                        {
                            l_sobre_comision.Text = "";
                        }
                        string codvendedor = c_vendedor.SelectedValue.ToString();
                        if (codvendedor == "JAIME BARGETTO")
                        {
                            l_sobre_comision.Text = "30% sobre comisión ANDRES NAVARRO: " + Base.monto_format2(total_comision2 * 0.3);
                        }
                        else if (l_regla.Text != "Regla11")
                        {
                            l_sobre_comision.Text = "";
                        }


                    }

                    if (busca_columna_fac)
                    {
                        try
                        {
                            for (int x = 0; x <= G_Nomina.HeaderRow.Cells.Count; x++)
                            {
                                if (G_Nomina.HeaderRow.Cells[x].Text.Contains("FACTURA"))
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
                    string año_factura = "";
                    try
                    {
                        año_factura = e.Row.Cells[11].Text.Trim().Substring(0, 4);
                    }
                    catch (Exception ex)
                    {
                        string aca2 = "";

                    }
                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                    if (columna_fac)
                    {

                        string script = string.Format("javascript:fuera3(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[COLUMNA_DE_FACTURA].Text.Trim()), encriptador.EncryptData(año_factura));
                        e.Row.Cells[COLUMNA_DE_FACTURA].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[COLUMNA_DE_FACTURA].Text + " </a>";

                    }

                    if (e.Row.Cells[15].Text == "CM")
                    {
                        string script3 = string.Format("javascript:click_modal_cm(&#39;{0}&#39;);return false;", descr);
                        e.Row.Cells[15].Text = "  <a href='javascript:' onclick='" + script3 + "'>" + e.Row.Cells[15].Text.Trim() + " " + descr.Substring(descr.IndexOf("F:")).Trim() + " </a>";
                    }

                    double porcdescu = Convert.ToDouble(e.Row.Cells[16].Text);
                    e.Row.Cells[16].Text = porcdescu.ToString("N2");



                    e.Row.Cells[17].Visible = false;
                    G_Nomina.HeaderRow.Cells[17].Visible = false;

                }
                catch (Exception ex)
                {
                    string aca2 = "";

                }
            }

        }




        [WebMethod]
        public static string txt_porcentaje_editado(string cod_periodo, string vender, string factura, string product, string regla, string porcen, string porcentaje)
        {
            string aasdf = "";

            comisionperiodocierre_productosEntity t = new comisionperiodocierre_productosEntity();
            t.númfactura = factura;
            t.producto = product;
            t.cod_regla = regla;
            t.vendedor = vender;
            t.cod_periodo = cod_periodo;
            t.porcentaje = Convert.ToDouble(porcen);

            comisionperiodocierre_productosBO.encontrar(ref t);


            string ok = "";
            if (porcentaje.Trim() == "")
            {
                t.porcentaje_edit = 0;
                ok = comisionperiodocierre_productosBO.actualizar_sinporcentaje(t);

            }
            else
            {

                t.porcentaje_edit = Convert.ToDouble(porcentaje.Trim());
                ok = comisionperiodocierre_productosBO.actualizar_2(t);
            }
            //ReporteRNegocio.delete_estado_sp(factura);
            //ReporteRNegocio.insert_estado_sp(factura, estado);

            return ok;
        }

        protected void G_Nomina_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Agregar")
            {


            }
        }

        protected void ch_add_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                double total_suma = 0;
                string cod_periodo = "";
                string vendedor = "";
                string factura = "";
                string producto = "";
                string cod_regla = "";

                //foreach (GridViewRow row in G_Nomina.Rows)
                //{

                //    CheckBox ChkBoxHeader = (CheckBox)row.FindControl("ch_add2");

                //    comisionperiodocierre_productosEntity t = new comisionperiodocierre_productosEntity();
                //    t.cod_periodo = row.Cells[12].Text.Trim();
                //    t.vendedor = row.Cells[4].Text.Trim();
                //    t.númfactura = row.Cells[0].Text.Trim();
                //    t.producto = row.Cells[1].Text.Trim();
                //    t.cod_regla = row.Cells[8].Text.Trim();

                //    if (ChkBoxHeader.Checked)
                //    {

                //        string r2 = comisionperiodocierre_productosBO.agregar(t);
                //        if (r2 == "1" || r2 == "2")
                //        {

                //            ScriptManager.RegisterStartupScript(Page, this.GetType(), "t132ee3ee", "<script language='javascript'>alert('Guardado');</script>", false);


                //        }
                //    }
                //    else
                //    {

                //        string r2 = comisionperiodocierre_productosBO.eliminar(t);
                //        if (r2 == "1" || r2 == "2")
                //        {

                //            ScriptManager.RegisterStartupScript(Page, this.GetType(), "t124ee3ee", "<script language='javascript'>alert('Quitado');</script>", false);

                //        }
                //    }

                //}
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "MMM", "<script language='javascript'>TABLITA();</script>", false);
                string aca = "";
            }
            catch { }

        }

        public string on_off(string factura, string producto, string regla, string codvendedor, string cod_periodo)
        {
            comisionperiodocierre_productosEntity t = new comisionperiodocierre_productosEntity();
            t.númfactura = factura;
            t.producto = producto;
            t.cod_regla = regla;
            t.vendedor = codvendedor;
            t.cod_periodo = cod_periodo;

            return @"javascript: alert(" + factura + ");";
        }

        protected void btn_excel2_Click(object sender, EventArgs e)
        {
            Response.Clear();

            llenar_excel();
            G_NOMINA_eXCEL.Visible = true;

            Response.AddHeader("content-disposition", "attachment;filename=COMISION" + DateTime.Now.ToShortDateString() + ".xls");

            Response.Charset = "";

            // If you want the option to open the Excel file without saving than

            // comment out the line below

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.ContentType = "application/vnd.xls";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);

            G_NOMINA_eXCEL.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());


            Response.End();
            G_NOMINA_eXCEL.Visible = false;
        }

        private void llenar_excel()
        {

            cont_comisiones = 0;
            G_NOMINA_eXCEL.DataSource = comisiones_;
            G_NOMINA_eXCEL.DataBind();

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void b_total__ServerClick(object sender, EventArgs e)
        {
            string tabla_final = "";
            string que_puede_ver = Session["puede_ver"].ToString();
            string mes = DateTime.Parse(txt_fecha.Text).Month.ToString();
            if (mes.Length < 2)
            {
                mes = "0" + mes;
            }
            string periodo = DateTime.Parse(txt_fecha.Text).Year.ToString() + mes;
            ///ACA INCLUIR CALCULO COMISION COBRANZA ----->
            ///
            ///CALCULO EN BASE A SISTEMA DE COBRANZA (CALENDARIO)
            //DataTable comision_cobranza = ReporteRNegocio.comision_cobranza(periodo);

            ///CALCULO EN BASE A SISTEMA DE COMISIONES
            DataTable comision_cobranza = ReporteRNegocio.comision_cobranza_2(periodo);
            //no lo uso ->

            //DataTable categorias_cobranza = ReporteRNegocio.categoria_cobranza_comis();

            //------------RE CALCULAR PARTICIPACION DE VENDEDORES PARA REGLA 21 GONZALO DE TORO
            string ok = ReporteRNegocio.recalcular_regla_21_toro(periodo);
            string ok1 = ReporteRNegocio.precalcular_regla_11_amaro(periodo);

            try
            {
                dt_vendedores.Columns.Add("es_abarrotes", typeof(double));
            }
            catch
            {
            }
            foreach (DataRow c_vendedor_r in dt_vendedores.Rows)
            {
                //select* from[dbo].[V_COMISION_VENDEDORES]
                double abarrotes = ReporteRNegocio.tiene_regla_abarrote(c_vendedor_r[0].ToString());
                c_vendedor_r[2] = abarrotes;
            }


            DataView dv3 = dt_vendedores.DefaultView;
            dv3.Sort = "es_abarrotes";
            dt_vendedores = dv3.ToTable();

            dt_total_resumen_com = new DataTable();
            //dt_total_resumen_com.NewRow();
            dt_total_resumen_com.Columns.Add("codvendedor");
            dt_total_resumen_com.Columns.Add("nombre_vendedor");
            dt_total_resumen_com.Columns.Add("nombreregla");
            dt_total_resumen_com.Columns.Add("total");
            dt_total_resumen_com.Columns.Add("es_abarrotes");

            DataRow row2;


            foreach (DataRow c_vendedor_r in dt_vendedores.Rows)
            {
                string codvendedor = c_vendedor_r[0].ToString();
                string reglas = agregar_comillas(ReporteRNegocio.trae_reglas(codvendedor));

                string reglas_ = "'-- Todos --'";

                string regla_aux = reglas_;

                string where_regla = "";

                int on_off_todos = 2;

                l_sobre_comision.Text = "";
                l_total_comision.Text = "";

                string codvendedor_aux = codvendedor;
                if (codvendedor == "JAIME BARGETTO")
                {
                    codvendedor = "AN044";
                    reglas = agregar_comillas(ReporteRNegocio.trae_reglas(codvendedor));
                }
                if (codvendedor != "CA001")
                {
                    if (codvendedor != "GT015")
                    {


                        if (codvendedor != "PARRA")
                        {
                            if (reglas_ != "'-- Todos --'")
                            {
                                where_regla += " and regla in (" + reglas_ + ")";
                            }
                            else
                            {

                                where_regla += " and regla in (" + reglas + ")";

                            }
                            where_regla += "  AND CODVENDEDOR = '" + codvendedor + "'";
                        }

                    }
                }

                if (codvendedor == "CA001")
                {
                    //regla 11
                    if (reglas_ != "'-- Todos --'")
                    {
                        if (reglas_ == "'Regla8'")
                        {
                            where_regla = " and regla in ('regla8')";

                        }
                        else if (reglas_ == "'Regla4'")
                        {
                            where_regla = " and regla in ('regla4')";
                        }
                        else if (reglas_ == "'Regla11'")
                        {
                            where_regla = " and regla in ('Regla11')";
                        }
                        else if (reglas_ == "'regla19'")
                        {
                            where_regla = " and regla in ('regla19')";
                        }
                        else
                        {
                            where_regla += " and regla in (" + reglas_ + ")";
                            where_regla += "  AND CODVENDEDOR = '" + codvendedor + "'";

                        }
                    }
                    else
                    {
                        reglas_ = reglas;
                        where_regla += " and ( regla in (" + reglas_ + ")  AND CODVENDEDOR = '" + codvendedor + "' or regla in ('regla4' , 'regla8', 'regla11', 'regla19' ) )";
                    }
                }
                if (codvendedor == "GT015")
                {
                    if (reglas_ != "'-- Todos --'")
                    {
                        if (reglas_ == "'Regla8'")
                        {
                            where_regla = " and regla in ('regla8')";
                        }
                        else if (reglas_ == "'Regla10'")
                        {
                            where_regla = " and regla in ('regla10')";
                        }
                        else if (reglas_ == "'Regla13'")
                        {
                            where_regla = " and regla in ('regla13')";
                        }
                        else
                        {
                            where_regla += " and regla in (" + reglas_ + ")";
                            where_regla += "  AND CODVENDEDOR = '" + codvendedor + "'";

                        }
                    }
                    else
                    {
                        reglas_ = reglas;
                        where_regla += "  and ( regla in (" + reglas_ + ")  AND CODVENDEDOR = '" + codvendedor + "'   or regla in ('regla10', 'regla8', 'regla18', 'regla13', 'regla21') )";
                    }
                }
                if (codvendedor == "PARRA")
                {
                    if (reglas_ != "'-- Todos --'")
                    {
                        if (reglas_ == "'Regla9'")
                        {
                            where_regla = " and regla in ('regla9')";

                        }
                        else if (reglas_ == "'Regla7'")
                        {
                            where_regla = " and regla in ('regla7')";
                        }
                        else if (reglas_ == "'Regla17'")
                        {
                            where_regla = " and regla in ('regla17')";
                        }
                        else
                        {
                            where_regla += " and regla in (" + reglas_ + ")";

                        }
                    }
                    else
                    {
                        reglas_ = reglas;
                        where_regla += "  and ( regla in (" + reglas_ + ") or regla in ('regla7', 'regla9', 'regla17') )";

                    }

                }

                llenar_facturas_resumen(periodo, where_regla, on_off_todos);

                DataTable totales = new DataTable();

                totales.Columns.Add("NombreRegla");
                totales.Columns.Add("Total");

                DataRow row;

                List<string> regla = reglas.Split(',').ToList();
                Double tota_comision_vendedor = 0;

                double agroin_amaro = 0;
                try

                {
                    agroin_amaro = (double)Session["comision_amaro_agroin"];
                }
                catch
                {
                    agroin_amaro = 0;

                }


                bool es_abarrote = false;
                //que_puede_ver
                foreach (string reg1a in regla)
                {
                    row = totales.NewRow();

                    DataView dv = new DataView(comisiones_resumen);
                    dv.RowFilter = "regla = " + reg1a; // query example = "id = 10"

                    if (reg1a.ToUpper() == "'REGLA16'")
                    {
                        es_abarrote = true;
                    }


                    DataTable filtradacomision = dv.ToTable();
                    if (filtradacomision.Rows.Count > 0)
                    {
                        double total_por_regla = 0;
                        double total_comision = 0;
                        string nombre_regla = "";
                        foreach (DataRow r1 in filtradacomision.Rows)
                        {


                            if (r1["porcentaje_edit"].ToString() != "")
                            {
                                double neto = Convert.ToDouble(r1["neto_pesos"].ToString());
                                double porcentaje = Convert.ToDouble(r1["porcentaje_edit"].ToString());
                                double comision_editada = Math.Round((neto * porcentaje) / 100);
                                total_por_regla += comision_editada;
                                total_comision += comision_editada;
                            }
                            else
                            {
                                total_por_regla += Convert.ToDouble(r1["montocomision"]);
                                total_comision += Convert.ToDouble(r1["montocomision"]);
                            }
                            nombre_regla = r1["nombre_comision"].ToString();
                        }

                        if (reg1a.Contains("regla19"))
                        {
                            total_por_regla += agroin_amaro;
                            total_comision += agroin_amaro;
                        }

                        if (nombre_regla == "COMISION SOBRE COMISIONES RAFAEL PINO, ANDRES GONZALES")
                        {
                            //l_sobre_comision.Text = "30% sobre comisión: " + Base.monto_format2(Math.Round(total_por_regla * 0.25));
                            //row["NombreRegla"] = nombre_regla;
                            //row["Total"] = Math.Round(total_por_regla * 0.25);
                            //tota_comision_vendedor += Math.Round(total_por_regla * 0.25);

                            row["NombreRegla"] = nombre_regla;
                            row["Total"] = total_por_regla;
                            tota_comision_vendedor += total_por_regla;

                        }
                        else
                        {
                            row["NombreRegla"] = nombre_regla;
                            row["Total"] = total_por_regla;
                            tota_comision_vendedor += total_por_regla;

                        }
                        totales.Rows.Add(row);
                    }

                    else
                    {

                        if (reg1a.Contains("Regla19"))
                        {
                            double total_por_regla = agroin_amaro;
                            double total_comision = agroin_amaro;

                            row["NombreRegla"] = "0.25% ADICIONAL SOBRE VENTAS PROD. AGROIN";
                            row["Total"] = total_por_regla;
                            tota_comision_vendedor += total_por_regla;
                            totales.Rows.Add(row);
                        }
                    }
                }

                row = totales.NewRow();
                row["NombreRegla"] = "::::TOTAL:::: ";
                row["Total"] = Math.Round(tota_comision_vendedor);
                totales.Rows.Add(row);


                if (codvendedor_aux == "AN044")
                {
                    string neto_navarro = ReporteRNegocio.neto_navarro(periodo);
                    row = totales.NewRow();
                    row["NombreRegla"] = "_:NETO:_";
                    row["Total"] = Base.monto_format(neto_navarro);
                    totales.Rows.Add(row);

                }


                string tabla = "";

                //---------------------------------------------------------------------------------ACA RELLENA VENDEDORES  GRANOS Y ABARROTES

                string color_th = "";
                string visible_th_si = "style='visibility: block; position: absolute;'";
                string visible_th_no = "style='visibility: hidden; position: absolute;'";

                string visible_th = "";

                if (que_puede_ver == "Abarrotes")
                {

                }
                else if (que_puede_ver == "Granos")
                {

                }

                tabla += "<table class=\"table fill-head table-bordered\">";
                if (c_vendedor_r[2].ToString() == "1")
                {
                    tabla += "<thead style='background-color: #e49191 !important; color: white !important; '>";
                    color_th = "background-color: #e49191 !important; color: white !important;";
                }
                else
                {
                    tabla += "<thead class=\"test\">";
                    color_th = "";
                }

                tabla += "<tr>";
                tabla += "<th style='width: 33%;" + color_th + "'>  NOMBRE  </th>";
                tabla += "<th style='width: 50%;" + color_th + "'>Nombre de Regla</th>";
                tabla += "<th style='width: 17%;" + color_th + "'>Total</th>";
                tabla += "</tr>";
                tabla += "</thead>";
                tabla += "<tbody>";

                if (codvendedor_aux == "JAIME BARGETTO")
                {
                    row2 = dt_total_resumen_com.NewRow();

                    tabla += "<tr>";
                    tabla += "<td>" + c_vendedor_r[1].ToString() + "</td>";
                    tabla += "<td> COMISION SOBRE COMISIONES ANDRES NAVARRO</td>";
                    tabla += "<td>" + Base.monto_format2(tota_comision_vendedor * 0.3) + "</td>";
                    tabla += "</tr>";


                    row2["codvendedor"] = c_vendedor_r[0].ToString().Trim();
                    row2["nombre_vendedor"] = c_vendedor_r[1].ToString().Trim();
                    row2["nombreregla"] = "COMISION SOBRE COMISIONES ANDRES NAVARRO";
                    row2["total"] = Base.monto_format2(tota_comision_vendedor * 0.3);
                    row2["es_abarrotes"] = es_abarrote;
                    dt_total_resumen_com.Rows.Add(row2);
                }
                else
                {
                    foreach (DataRow r2 in totales.Rows)
                    {
                        row2 = dt_total_resumen_com.NewRow();
                        if (r2["NombreRegla"].ToString() == "_:NETO:_")
                            tabla += "<tr style='background-color: lavender; '>";
                        else
                            tabla += "<tr>";
                        tabla += "<td>" + c_vendedor_r[1].ToString() + "</td>";
                        tabla += "<td>" + r2["NombreRegla"].ToString() + "</td>";
                        tabla += "<td>" + Base.monto_format2(Convert.ToDouble(r2["Total"].ToString())) + "</td>";
                        tabla += "</tr>";

                        row2["codvendedor"] = c_vendedor_r[0].ToString().Trim();
                        row2["nombre_vendedor"] = c_vendedor_r[1].ToString().Trim();
                        row2["nombreregla"] = r2["NombreRegla"].ToString();
                        row2["total"] = Base.monto_format2(Convert.ToDouble(r2["Total"].ToString()));
                        row2["es_abarrotes"] = es_abarrote;
                        dt_total_resumen_com.Rows.Add(row2);
                    }
                }
                tabla += "</tbody>";
                tabla += "</table>";
                tabla += "</br> ";
                tabla_final += tabla;
            }

            ////aca temporal se agrega AZOCAR Y CATALDO
            if (que_puede_ver != "Granos")
            {
                //Session["puede_ver"]

                DataTable dt_oficina_abarro = (DataTable)Session["DT_ABARROTES_OFICINA"];

                int sum1 = 0;
                int sum2 = 0;
                try
                {
                    sum1 = Convert.ToInt32(dt_oficina_abarro.Rows[0][0].ToString().Replace(",000000", ""));
                    sum2 = Convert.ToInt32(dt_oficina_abarro.Rows[0][1].ToString().Replace(",000000", ""));
                }
                catch
                {

                }

                string total_abarrote_con_arica = "";
                string desarrollo_mercado_ro = "";
                string total_abarrote_sin_Arica = "";
                try
                {
                    total_abarrote_con_arica = Base.monto_format(dt_oficina_abarro.Rows[0][0].ToString().Replace(",000000", ""));
                    desarrollo_mercado_ro = Base.monto_format(dt_oficina_abarro.Rows[0][1].ToString().Replace(",000000", ""));
                    total_abarrote_sin_Arica = Base.monto_format(dt_oficina_abarro.Rows[0][2].ToString().Replace(",000000", ""));
                }
                catch
                {


                }

                row2 = dt_total_resumen_com.NewRow();
                row2["codvendedor"] = "ABARROTES";
                row2["nombre_vendedor"] = "MARCO ANTONIO AZOCAR";
                row2["nombreregla"] = "TOTAL ABARROTES (C/ARICA)";
                row2["total"] = total_abarrote_con_arica;
                row2["es_abarrotes"] = true;
                dt_total_resumen_com.Rows.Add(row2);

                row2 = dt_total_resumen_com.NewRow();
                row2["codvendedor"] = "ABARROTES";
                row2["nombre_vendedor"] = "MARCO ANTONIO AZOCAR";
                row2["nombreregla"] = "DESARROLLO DE MERCADO (RO)";
                row2["total"] = desarrollo_mercado_ro;
                row2["es_abarrotes"] = true;
                dt_total_resumen_com.Rows.Add(row2);

                row2 = dt_total_resumen_com.NewRow();
                row2["codvendedor"] = "ABARROTES";
                row2["nombre_vendedor"] = "MARCO ANTONIO AZOCAR";
                row2["nombreregla"] = "::::TOTAL::::";
                row2["total"] = Base.monto_format2(sum1 + sum2);
                row2["es_abarrotes"] = true;
                dt_total_resumen_com.Rows.Add(row2);

                row2 = dt_total_resumen_com.NewRow();
                row2["codvendedor"] = "ABARROTES";
                row2["nombre_vendedor"] = "PATRICIA CATALDO";
                row2["nombreregla"] = "TOTAL ABARROTES (S/ARICA)";
                row2["total"] = total_abarrote_sin_Arica;
                row2["es_abarrotes"] = true;
                dt_total_resumen_com.Rows.Add(row2);

                row2 = dt_total_resumen_com.NewRow();
                row2["codvendedor"] = "ABARROTES";
                row2["nombre_vendedor"] = "PATRICIA CATALDO";
                row2["nombreregla"] = "::::TOTAL::::";
                row2["total"] = total_abarrote_sin_Arica;
                row2["es_abarrotes"] = true;
                dt_total_resumen_com.Rows.Add(row2);

                string tabla_azocar_y_cataldo = "";
                string color_th1 = "background-color: #e49191 !important; color: white !important;";

                tabla_azocar_y_cataldo += "<table class=\"table fill-head table-bordered\">";
                tabla_azocar_y_cataldo += "<thead class=\"test\">";
                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<th style='width: 33%;" + color_th1 + "'>  NOMBRE  </th>";
                tabla_azocar_y_cataldo += "<th style='width: 50%;" + color_th1 + "'>Nombre de Regla</th>";
                tabla_azocar_y_cataldo += "<th style='width: 17%;" + color_th1 + "'>Total</th>";
                tabla_azocar_y_cataldo += "</tr>";
                tabla_azocar_y_cataldo += "</thead>";
                tabla_azocar_y_cataldo += "<tbody>";

                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "MARCO ANTONIO AZOCAR";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "TOTAL ABARROTES (C/ARICA)";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += total_abarrote_con_arica;
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "</tr>";

                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "MARCO ANTONIO AZOCAR";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "DESARROLLO DE MERCADO (RO)";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += desarrollo_mercado_ro;
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "</tr>";

                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";

                tabla_azocar_y_cataldo += "::::TOTAL::::";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += Base.monto_format2(sum1 + sum2);
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "</tr>";

                tabla_azocar_y_cataldo += "</tbody>";
                tabla_azocar_y_cataldo += "</table>";
                tabla_azocar_y_cataldo += "</br> ";

                ///// cataldo

                tabla_azocar_y_cataldo += "<table class=\"table fill-head table-bordered\">";
                tabla_azocar_y_cataldo += "<thead >";
                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<th style='width: 33%;" + color_th1 + "'>  NOMBRE  </th>";
                tabla_azocar_y_cataldo += "<th style='width: 50%;" + color_th1 + "'>Nombre de Regla</th>";
                tabla_azocar_y_cataldo += "<th style='width: 17%;" + color_th1 + "'>Total</th>";
                tabla_azocar_y_cataldo += "</tr>";
                tabla_azocar_y_cataldo += "</thead>";
                tabla_azocar_y_cataldo += "<tbody>";

                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "PATRICIA CATALDO";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "TOTAL ABARROTES S/ARICA";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += total_abarrote_sin_Arica;
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "</tr>";

                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "::::TOTAL::::";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += total_abarrote_sin_Arica;
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "</tr>";

                tabla_azocar_y_cataldo += "</tbody>";
                tabla_azocar_y_cataldo += "</table>";
                tabla_azocar_y_cataldo += "</br> ";

                tabla_final += tabla_azocar_y_cataldo;

            }


            Session["dt_total_resumen"] = dt_total_resumen_com;
            Session["periodo"] = txt_periodo.Text;

            string tabla_cobranza = "";
            double total_neto = 0;
            ////---------------------------------------------------------------------------------------RESUMEN COMISIONES DE COBRANZA

            if (que_puede_ver == "Todo")
            {
                dt_total_cobranza = new DataTable();
                //dt_total_resumen_com.NewRow();
                dt_total_cobranza.Columns.Add("nombre_cobran");
                dt_total_cobranza.Columns.Add("neto");
                dt_total_cobranza.Columns.Add("porct");
                dt_total_cobranza.Columns.Add("total");

                DataRow row3;

                //ROSA SOLIS .....................------------------------------------------------------------------------------------------

                //pdf
                row3 = dt_total_cobranza.NewRow();
                row3["nombre_cobran"] = "ROSA SOLIS V.";
                row3["neto"] = "NETO";
                row3["porct"] = "PORCT";
                row3["total"] = "TOTAL";
                dt_total_cobranza.Rows.Add(row3);


                string color_cobranza = "background-color: #4d9850 !important; color: white !important; ";
                //web
                tabla_cobranza += "<table class=\"table fill-head table-bordered\">";
                tabla_cobranza += "<thead>";
                tabla_cobranza += "<tr>";
                tabla_cobranza += "<th style='width: 40%;" + color_cobranza + "'>  ROSA SOLIS V. </th>";
                tabla_cobranza += "<th style='width: 28%;" + color_cobranza + "'>NETO </th>";
                tabla_cobranza += "<th style='width: 15%;" + color_cobranza + "'>PORCT </th>";
                tabla_cobranza += "<th style='width: 17%;" + color_cobranza + "'>TOTAL</th>";
                tabla_cobranza += "</tr>";
                tabla_cobranza += "</thead>";
                tabla_cobranza += "<tbody>";

                double total_comision_cobranza = 0;
                foreach (DataRow r4 in comision_cobranza.Rows)
                {

                    row3 = dt_total_cobranza.NewRow();

                    string nombre_Categoria = r4[0].ToString();

                    double neto_categoria = Convert.ToDouble(r4[1].ToString());
                    total_neto += neto_categoria;

                    string porcentaje = r4[2].ToString() + " %";

                    double comision_categoria = Convert.ToDouble(r4[5].ToString());
                    total_comision_cobranza += comision_categoria;

                    //pdf
                    row3 = dt_total_cobranza.NewRow();
                    row3["nombre_cobran"] = nombre_Categoria;
                    row3["neto"] = Base.monto_format2(neto_categoria);
                    row3["porct"] = porcentaje;
                    row3["total"] = Base.monto_format2(comision_categoria);
                    dt_total_cobranza.Rows.Add(row3);

                    //web
                    tabla_cobranza += "<tr>";
                    tabla_cobranza += "<td>" + nombre_Categoria + "</td>";
                    tabla_cobranza += "<td>" + Base.monto_format2(neto_categoria) + "</td>";
                    if (Convert.ToDouble(r4[2].ToString()) == 0)
                    {
                        tabla_cobranza += "<td></td>";
                    }
                    else
                    {
                        tabla_cobranza += "<td>" + porcentaje + "</td>";
                    }
                    tabla_cobranza += "<td>" + Base.monto_format2(comision_categoria) + "</td>";
                    tabla_cobranza += "</tr>";
                }
                //pdf
                row3 = dt_total_cobranza.NewRow();
                row3["nombre_cobran"] = ":::::TOTAL::::::";
                row3["neto"] = Base.monto_format2(total_neto);
                row3["porct"] = "";
                row3["total"] = Base.monto_format2(total_comision_cobranza);
                dt_total_cobranza.Rows.Add(row3);

                //web
                tabla_cobranza += "<tr>";
                tabla_cobranza += "<td>:::::TOTAL::::::</td>";
                tabla_cobranza += "<td>" + Base.monto_format2(total_neto) + "</td>";
                tabla_cobranza += "<td></td>";
                tabla_cobranza += "<td>" + Base.monto_format2(total_comision_cobranza) + "</td>";
                tabla_cobranza += "</tr>";

                tabla_cobranza += "</tbody>";
                tabla_cobranza += "</table>";
                tabla_cobranza += "</br> ";


                //EVELYN LEIVA .....................------------------------------------------------------------------------------------------

                //pdf
                row3 = dt_total_cobranza.NewRow();
                row3["nombre_cobran"] = "EVELYN LEIVA";
                row3["neto"] = "NETO";
                row3["porct"] = "PORCT";
                row3["total"] = "TOTAL";
                dt_total_cobranza.Rows.Add(row3);

                //web
                tabla_cobranza += "<table class=\"table fill-head table-bordered\">";
                tabla_cobranza += "<thead style='background-color: #4d9850 !important; color: white !important; '>";
                tabla_cobranza += "<tr>";
                tabla_cobranza += "<th style='width: 40%;" + color_cobranza + "'>EVELYN LEIVA </th>";
                tabla_cobranza += "<th style='width: 28%;" + color_cobranza + "'>NETO </th>";
                tabla_cobranza += "<th style='width: 15%;" + color_cobranza + "'>PORCT </th>";
                tabla_cobranza += "<th style='width: 17%;" + color_cobranza + "'>TOTAL</th>";
                tabla_cobranza += "</tr>";
                tabla_cobranza += "</thead>";
                tabla_cobranza += "<tbody>";

                total_comision_cobranza = 0;
                foreach (DataRow r4 in comision_cobranza.Rows)
                {

                    row3 = dt_total_cobranza.NewRow();

                    string nombre_Categoria = r4[0].ToString();

                    double neto_categoria = Convert.ToDouble(r4[1].ToString());

                    string porcentaje = r4[3].ToString() + " %";

                    double comision_categoria = Convert.ToDouble(r4[6].ToString());
                    total_comision_cobranza += comision_categoria;

                    //pdf
                    row3 = dt_total_cobranza.NewRow();
                    row3["nombre_cobran"] = nombre_Categoria;
                    row3["neto"] = Base.monto_format2(neto_categoria);
                    row3["porct"] = porcentaje;
                    row3["total"] = Base.monto_format2(comision_categoria);
                    dt_total_cobranza.Rows.Add(row3);

                    //web
                    tabla_cobranza += "<tr>";
                    tabla_cobranza += "<td>" + nombre_Categoria + "</td>";
                    tabla_cobranza += "<td>" + Base.monto_format2(neto_categoria) + "</td>";

                    if (Convert.ToDouble(r4[3].ToString()) == 0)
                    {
                        tabla_cobranza += "<td></td>";
                    }
                    else
                    {
                        tabla_cobranza += "<td>" + porcentaje + "</td>";
                    }

                    tabla_cobranza += "<td>" + Base.monto_format2(comision_categoria) + "</td>";
                    tabla_cobranza += "</tr>";
                }
                //pdf
                row3 = dt_total_cobranza.NewRow();
                row3["nombre_cobran"] = ":::::TOTAL::::::";
                row3["neto"] = Base.monto_format2(total_neto);
                row3["porct"] = "";
                row3["total"] = Base.monto_format2(total_comision_cobranza);
                dt_total_cobranza.Rows.Add(row3);

                //web
                tabla_cobranza += "<tr>";
                tabla_cobranza += "<td>:::::TOTAL::::::</td>";
                tabla_cobranza += "<td>" + Base.monto_format2(total_neto) + "</td>";
                tabla_cobranza += "<td></td>";
                tabla_cobranza += "<td>" + Base.monto_format2(total_comision_cobranza) + "</td>";
                tabla_cobranza += "</tr>";


                tabla_cobranza += "</tbody>";
                tabla_cobranza += "</table>";
                tabla_cobranza += "</br> ";


                //MARIANA SILVA .....................------------------------------------------------------------------------------------------
                //pdf
                //row3 = dt_total_cobranza.NewRow();
                //row3["nombre_cobran"] = "MARIANA SILVA";
                //row3["neto"] = "NETO";
                //row3["porct"] = "PORCT";
                //row3["total"] = "TOTAL";
                //dt_total_cobranza.Rows.Add(row3);

                //tabla_cobranza += "<table class=\"table fill-head table-bordered\">";
                //tabla_cobranza += "<thead style='background-color: #4d9850 !important; color: white !important; '>";
                //tabla_cobranza += "<tr>";
                //tabla_cobranza += "<th style='width: 40%;" + color_cobranza + "'>MARIANA SILVA</th>";
                //tabla_cobranza += "<th style='width: 28%;" + color_cobranza + "'>NETO </th>";
                //tabla_cobranza += "<th style='width: 15%;" + color_cobranza + "'>PORCT </th>";
                //tabla_cobranza += "<th style='width: 17%;" + color_cobranza + "'>TOTAL</th>";
                //tabla_cobranza += "</tr>";
                //tabla_cobranza += "</thead>";
                //tabla_cobranza += "<tbody>";

                //total_comision_cobranza = 0;
                //foreach (DataRow r4 in comision_cobranza.Rows)
                //{
                //    row3 = dt_total_cobranza.NewRow();

                //    string nombre_Categoria = r4[0].ToString();

                //    double neto_categoria = Convert.ToDouble(r4[1].ToString());

                //    string porcentaje = r4[4].ToString() + " %";

                //    double comision_categoria = Convert.ToDouble(r4[7].ToString());
                //    total_comision_cobranza += comision_categoria;

                //    //pdf
                //    row3 = dt_total_cobranza.NewRow();
                //    row3["nombre_cobran"] = nombre_Categoria;
                //    row3["neto"] = Base.monto_format2(neto_categoria);
                //    row3["porct"] = porcentaje;
                //    row3["total"] = Base.monto_format2(comision_categoria);
                //    dt_total_cobranza.Rows.Add(row3);

                //    //web
                //    tabla_cobranza += "<tr>";
                //    tabla_cobranza += "<td>" + r4[0].ToString() + "</td>";
                //    tabla_cobranza += "<td>" + Base.monto_format2(neto_categoria) + " </td>";

                //    if (Convert.ToDouble(r4[4].ToString()) == 0)
                //    {
                //        tabla_cobranza += "<td></td>";
                //    }
                //    else
                //    {
                //        tabla_cobranza += "<td>" + porcentaje + "</td>";
                //    }

                //    tabla_cobranza += "<td>" + Base.monto_format2(comision_categoria) + "</td>";
                //    tabla_cobranza += "</tr>";
                //}
                ////pdf
                //row3 = dt_total_cobranza.NewRow();
                //row3["nombre_cobran"] = ":::::TOTAL::::::";
                //row3["neto"] = Base.monto_format2(total_neto);
                //row3["porct"] = "";
                //row3["total"] = Base.monto_format2(total_comision_cobranza);
                //dt_total_cobranza.Rows.Add(row3);

                //web
                //tabla_cobranza += "<tr>";
                //tabla_cobranza += "<td>:::::TOTAL::::::</td>";
                //tabla_cobranza += "<td>" + Base.monto_format2(total_neto) + "</td>";
                //tabla_cobranza += "<td></td>";
                //tabla_cobranza += "<td>" + Base.monto_format2(total_comision_cobranza) + "</td>";
                //tabla_cobranza += "</tr>";
                //tabla_cobranza += "</tbody>";
                //tabla_cobranza += "</table>";
                //tabla_cobranza += "</br> ";


                Session["dt_cobranza_com_resu"] = dt_total_cobranza;

                tabla_final += tabla_cobranza;


            }
            //comisiones cobranza

            div_tabla_totales.InnerHtml = tabla_final;

            UpdatePanel2.Update();



            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>click_modal();</script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd121mk3mp", "<script language='javascript'>tabla();</script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "t3mp", "<script language='javascript'>firmas_j('" + txt_periodo.Text + "', '" + User.Identity.Name + "');</script>", false);

        }
        //private string llenar_vendores(string periodo, string cod_vendedor, Double tota_comision_vendedor, DataTable totales)
        //{

        //    string codvendedor = cod_vendedor;
        //    string reglas = agregar_comillas(ReporteRNegocio.trae_reglas(codvendedor));
        //    string reglas_ = "'-- Todos --'";
        //    string regla_aux = reglas_;
        //    string where_regla = "";
        //    int on_off_todos = 2;

        //    l_sobre_comision.Text = "";
        //    l_total_comision.Text = "";

        //    string codvendedor_aux = codvendedor;
        //    if (codvendedor == "JAIME BARGETTO")
        //    {
        //        codvendedor = "AN044";
        //        reglas = agregar_comillas(ReporteRNegocio.trae_reglas(codvendedor));
        //    }


        //    if (codvendedor != "CA001")
        //    {
        //        if (codvendedor != "GT015")
        //        {


        //            if (codvendedor != "PARRA")
        //            {
        //                if (reglas_ != "'-- Todos --'")
        //                {
        //                    where_regla += " and regla in (" + reglas_ + ")";
        //                }
        //                else
        //                {

        //                    where_regla += " and regla in (" + reglas + ")";

        //                }
        //                where_regla += "  AND CODVENDEDOR = '" + codvendedor + "'";
        //            }

        //        }
        //    }

        //    if (codvendedor == "CA001")
        //    {
        //        //regla 11
        //        if (reglas_ != "'-- Todos --'")
        //        {
        //            if (reglas_ == "'Regla8'")
        //            {
        //                where_regla = " and regla in ('regla8')";

        //            }
        //            else if (reglas_ == "'Regla4'")
        //            {
        //                where_regla = " and regla in ('regla4')";
        //            }
        //            else if (reglas_ == "'Regla11'")
        //            {
        //                where_regla = " and regla in ('Regla11')";
        //            }
        //            else if (reglas_ == "'regla19'")
        //            {
        //                where_regla = " and regla in ('regla19')";
        //            }
        //            else
        //            {
        //                where_regla += " and regla in (" + reglas_ + ")";
        //                where_regla += "  AND CODVENDEDOR = '" + codvendedor + "'";

        //            }
        //        }
        //        else
        //        {
        //            reglas_ = reglas;
        //            where_regla += " and ( regla in (" + reglas_ + ")  AND CODVENDEDOR = '" + codvendedor + "' or regla in ('regla4' , 'regla8', 'regla11', 'regla19' ) )";
        //        }
        //    }
        //    if (codvendedor == "GT015")
        //    {
        //        if (reglas_ != "'-- Todos --'")
        //        {
        //            if (reglas_ == "'Regla8'")
        //            {
        //                where_regla = " and regla in ('regla8')";
        //            }
        //            else if (reglas_ == "'Regla10'")
        //            {
        //                where_regla = " and regla in ('regla10')";
        //            }
        //            else if (reglas_ == "'Regla13'")
        //            {
        //                where_regla = " and regla in ('regla13')";
        //            }
        //            else
        //            {
        //                where_regla += " and regla in (" + reglas_ + ")";
        //                where_regla += "  AND CODVENDEDOR = '" + codvendedor + "'";

        //            }
        //        }
        //        else
        //        {
        //            reglas_ = reglas;
        //            where_regla += "  and ( regla in (" + reglas_ + ")  AND CODVENDEDOR = '" + codvendedor + "'   or regla in ('regla10', 'regla8', 'regla18', 'regla13') )";
        //        }
        //    }
        //    if (codvendedor == "PARRA")
        //    {
        //        if (reglas_ != "'-- Todos --'")
        //        {
        //            if (reglas_ == "'Regla9'")
        //            {
        //                where_regla = " and regla in ('regla9')";

        //            }
        //            else if (reglas_ == "'Regla7'")
        //            {
        //                where_regla = " and regla in ('regla7')";
        //            }
        //            else if (reglas_ == "'Regla17'")
        //            {
        //                where_regla = " and regla in ('regla17')";
        //            }
        //            else
        //            {
        //                where_regla += " and regla in (" + reglas_ + ")";
        //            }
        //        }
        //        else
        //        {
        //            reglas_ = reglas;
        //            where_regla += "  and ( regla in (" + reglas_ + ") or regla in ('regla7', 'regla9', 'regla17') )";
        //        }
        //    }


        //    llenar_facturas_resumen(periodo, where_regla, on_off_todos);

        //    //DataTable totales = new DataTable();

        //    //totales.Columns.Add("NombreRegla");
        //    //totales.Columns.Add("Total");

        //    DataRow row;

        //    List<string> regla = reglas.Split(',').ToList();
        //    //Double tota_comision_vendedor = 0;

        //    double agroin_amaro = 0;
        //    try

        //    {
        //        agroin_amaro = (double)Session["comision_amaro_agroin"];
        //    }
        //    catch
        //    {
        //        agroin_amaro = 0;

        //    }


        //    bool es_abarrote = false;
        //    foreach (string reg1a in regla)
        //    {
        //        row = totales.NewRow();

        //        DataView dv = new DataView(comisiones_resumen);
        //        dv.RowFilter = "regla = " + reg1a; // query example = "id = 10"

        //        if (reg1a.ToUpper() == "'REGLA16'")
        //        {
        //            es_abarrote = true;
        //        }


        //        DataTable filtradacomision = dv.ToTable();
        //        if (filtradacomision.Rows.Count > 0)
        //        {
        //            double total_por_regla = 0;
        //            double total_comision = 0;
        //            string nombre_regla = "";
        //            foreach (DataRow r1 in filtradacomision.Rows)
        //            {


        //                if (r1["porcentaje_edit"].ToString() != "")
        //                {
        //                    double neto = Convert.ToDouble(r1["neto_pesos"].ToString());
        //                    double porncetaje = Convert.ToDouble(r1["porcentaje_edit"].ToString());
        //                    double comision_editada = Math.Round((neto * porncetaje) / 100);
        //                    total_por_regla += comision_editada;
        //                    total_comision += comision_editada;
        //                }
        //                else
        //                {
        //                    total_por_regla += Convert.ToDouble(r1["montocomision"]);
        //                    total_comision += Convert.ToDouble(r1["montocomision"]);
        //                }
        //                nombre_regla = r1["nombre_comision"].ToString();

        //            }

        //            if (reg1a.Contains("regla19"))
        //            {
        //                total_por_regla += agroin_amaro;
        //                total_comision += agroin_amaro;
        //            }

        //            if (nombre_regla == "COMISION SOBRE COMISIONES  RAFAEL PINO, ANDRES GONZALES")
        //            {
        //                l_sobre_comision.Text = "30% sobre comisión: " + Base.monto_format2(total_por_regla * 0.3);
        //                row["NombreRegla"] = nombre_regla;
        //                row["Total"] = total_por_regla * 0.3;
        //                tota_comision_vendedor += total_por_regla * 0.3;
        //            }
        //            else
        //            {
        //                row["NombreRegla"] = nombre_regla;
        //                row["Total"] = total_por_regla;
        //                tota_comision_vendedor += total_por_regla;
        //            }
        //            totales.Rows.Add(row);
        //        }

        //        else
        //        {


        //            if (reg1a.Contains("Regla19"))
        //            {
        //                double total_por_regla = agroin_amaro;
        //                double total_comision = agroin_amaro;

        //                row["NombreRegla"] = "0.25% ADICIONAL SOBRE VENTAS PROD. AGROIN";
        //                row["Total"] = total_por_regla;
        //                tota_comision_vendedor += total_por_regla;
        //                totales.Rows.Add(row);
        //            }
        //        }
        //    }

        //    row = totales.NewRow();
        //    row["NombreRegla"] = "::::TOTAL:::: ";
        //    row["Total"] = Math.Round(tota_comision_vendedor);
        //    totales.Rows.Add(row);


        //}

        protected void btn_refresh_Click(object sender, EventArgs e)
        {

            //btn_correo_ServerClick(sender, e);


            llenar_facturas(txt_periodo.Text, "", 2);
        }


        protected void btn_correo_2_Click(object sender, EventArgs e)
        {

            btn_correo_ServerClick(sender, e);
            llenar_facturas(txt_periodo.Text, "", 2);
        }


        protected void btn_pdf_Click(object sender, EventArgs e)
        {
            Session["periodo"] = txt_periodo.Text;

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeeasde", "<script language='javascript'>pdf_comision();</script>", false);


            //Session["dt_total_resumen"] = dt_total_resumen_com;


        }



        public class ITextEvents : PdfPageEventHelper
        {
            // Éste es el objeto contentbyte del writer
            PdfContentByte cb;

            // Pondremos el número final de páginas en una plantilla
            PdfTemplate headerTemplate, footerTemplate;

            // Este es el BaseFont que vamos a utilizar para el encabezado / pie de página
            BaseFont bf = null;

            DateTime PrintTime = DateTime.Now;

            #region Fields
            private string _header;
            #endregion

            #region Properties
            public string Header
            {
                get { return _header; }
                set { _header = value; }
            }
            #endregion

            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                try
                {
                    PrintTime = DateTime.Now;
                    bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
                    cb = writer.DirectContent;
                    headerTemplate = cb.CreateTemplate(100, 100);
                    //footerTemplate = cb.CreateTemplate(50, 50);
                }
                catch (DocumentException de)
                {
                    //handle exception here
                }
                catch (System.IO.IOException ioe)
                {
                    //handle exception here
                }
            }

            public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
            {
                base.OnEndPage(writer, document);

                //iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

                //iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

                String text = "Página " + writer.PageNumber + " de ";


                //añadir paginacion al encabezado
                {
                    cb.BeginText();
                    cb.SetFontAndSize(bf, 10);
                    cb.SetTextMatrix(document.PageSize.GetRight(100), document.PageSize.GetTop(20));
                    cb.ShowText(text);
                    cb.EndText();
                    float len = bf.GetWidthPoint(text, 10);
                    //Adds "12" in Page 1 of 12
                    cb.AddTemplate(headerTemplate, document.PageSize.GetRight(100) + len, document.PageSize.GetTop(20));
                }
                //añadir paginacion al pie de pagina
                //{
                //    cb.BeginText();
                //    cb.SetFontAndSize(bf, 12);
                //    cb.SetTextMatrix(document.PageSize.GetRight(180), document.PageSize.GetBottom(30));
                //    cb.ShowText(text);
                //    cb.EndText();
                //    float len = bf.GetWidthPoint(text, 12);
                //    cb.AddTemplate(footerTemplate, document.PageSize.GetRight(180) + len, document.PageSize.GetBottom(30));
                //}

                //Move the pointer and draw line to separate header section from rest of page
                //cb.MoveTo(40, document.PageSize.Height - 100);
                //cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 100);
                //cb.Stroke();

                //Move the pointer and draw line to separate footer section from rest of page
                //cb.MoveTo(40, document.PageSize.GetBottom(50));
                //cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
                //cb.Stroke();
            }

            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);

                headerTemplate.BeginText();
                headerTemplate.SetFontAndSize(bf, 10);
                headerTemplate.SetTextMatrix(0, 0);
                headerTemplate.ShowText((writer.PageNumber - 1).ToString());
                headerTemplate.EndText();

                //footerTemplate.BeginText();
                //footerTemplate.SetFontAndSize(bf, 10);
                //footerTemplate.SetTextMatrix(0, 0);
                //footerTemplate.ShowText((writer.PageNumber - 1).ToString());
                //footerTemplate.EndText();


            }
        }

        protected void btn_crear_periodo_ServerClick(object sender, EventArgs e)
        {
            comisionperiodocierreEntity c = new comisionperiodocierreEntity();
            string mes = DateTime.Parse(txt_fecha.Text).Month.ToString();
            if (mes.Length < 2)
            {

                mes = "0" + mes;
            }

            c.cod_periodo = DateTime.Parse(txt_fecha.Text).Year.ToString() + mes.Trim();
            c.cod_usuario = User.Identity.Name;
            c.fecha_cierre = DateTime.Parse(txt_fecha.Text);
            c.autoriza = User.Identity.Name;
            c.fecha_autoriza = DateTime.Parse(txt_fecha.Text);
            string r = comisionperiodocierreBO.agregar(c);
            if (r == "1" || r == "2")
            {

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "t12ee3ee", "<script language='javascript'>alert('Periodo Guardado, Cargando....');</script>", false);
                completar(DateTime.Parse(txt_fecha.Text).Year.ToString() + mes.Trim());

            }
        }

        protected void btn_jbravo_pdf_correo_ServerClick(object sender, EventArgs e)
        {

            string tabla_final = "";
            string que_puede_ver = Session["puede_ver"].ToString();


            string mes = DateTime.Parse(txt_fecha.Text).Month.ToString();
            if (mes.Length < 2)
            {

                mes = "0" + mes;
            }

            string periodo = DateTime.Parse(txt_fecha.Text).Year.ToString() + mes;




            ///ACA INCLUIR CALCULO COMISION COBRANZA ----->
            ///
            DataTable comision_cobranza = ReporteRNegocio.comision_cobranza(periodo);
            //DataTable categorias_cobranza = ReporteRNegocio.categoria_cobranza_comis();



            try
            {
                dt_vendedores.Columns.Add("es_abarrotes", typeof(double));
            }
            catch
            {
            }
            foreach (DataRow c_vendedor_r in dt_vendedores.Rows)
            {
                //select* from[dbo].[V_COMISION_VENDEDORES]
                double abarrotes = ReporteRNegocio.tiene_regla_abarrote(c_vendedor_r[0].ToString());
                c_vendedor_r[2] = abarrotes;
            }


            DataView dv3 = dt_vendedores.DefaultView;
            dv3.Sort = "es_abarrotes";
            dt_vendedores = dv3.ToTable();

            dt_total_resumen_com = new DataTable();
            //dt_total_resumen_com.NewRow();
            dt_total_resumen_com.Columns.Add("codvendedor");
            dt_total_resumen_com.Columns.Add("nombre_vendedor");
            dt_total_resumen_com.Columns.Add("nombreregla");
            dt_total_resumen_com.Columns.Add("total");
            dt_total_resumen_com.Columns.Add("es_abarrotes");

            DataRow row2;

            foreach (DataRow c_vendedor_r in dt_vendedores.Rows)
            {
                string codvendedor = c_vendedor_r[0].ToString();
                string reglas = agregar_comillas(ReporteRNegocio.trae_reglas(codvendedor));


                string reglas_ = "'-- Todos --'";

                string regla_aux = reglas_;

                string where_regla = "";

                int on_off_todos = 2;

                l_sobre_comision.Text = "";
                l_total_comision.Text = "";

                string codvendedor_aux = codvendedor;
                if (codvendedor == "JAIME BARGETTO")
                {
                    codvendedor = "AN044";
                    reglas = agregar_comillas(ReporteRNegocio.trae_reglas(codvendedor));
                }


                if (codvendedor != "CA001")
                {
                    if (codvendedor != "GT015")
                    {


                        if (codvendedor != "PARRA")
                        {
                            if (reglas_ != "'-- Todos --'")
                            {
                                where_regla += " and regla in (" + reglas_ + ")";
                            }
                            else
                            {

                                where_regla += " and regla in (" + reglas + ")";

                            }
                            where_regla += "  AND CODVENDEDOR = '" + codvendedor + "'";
                        }

                    }
                }

                if (codvendedor == "CA001")
                {
                    //regla 11
                    if (reglas_ != "'-- Todos --'")
                    {
                        if (reglas_ == "'Regla8'")
                        {
                            where_regla = " and regla in ('regla8')";

                        }
                        else if (reglas_ == "'Regla4'")
                        {
                            where_regla = " and regla in ('regla4')";
                        }
                        else if (reglas_ == "'Regla11'")
                        {
                            where_regla = " and regla in ('Regla11')";
                        }
                        else if (reglas_ == "'regla19'")
                        {
                            where_regla = " and regla in ('regla19')";
                        }
                        else
                        {
                            where_regla += " and regla in (" + reglas_ + ")";
                            where_regla += "  AND CODVENDEDOR = '" + codvendedor + "'";

                        }
                    }
                    else
                    {
                        reglas_ = reglas;
                        where_regla += " and ( regla in (" + reglas_ + ")  AND CODVENDEDOR = '" + codvendedor + "' or regla in ('regla4' , 'regla8', 'regla11', 'regla19' ) )";

                    }

                }

                if (codvendedor == "GT015")
                {
                    if (reglas_ != "'-- Todos --'")
                    {
                        if (reglas_ == "'Regla8'")
                        {
                            where_regla = " and regla in ('regla8')";

                        }
                        else if (reglas_ == "'Regla10'")
                        {
                            where_regla = " and regla in ('regla10')";
                        }
                        else if (reglas_ == "'Regla13'")
                        {
                            where_regla = " and regla in ('regla13')";
                        }
                        else
                        {
                            where_regla += " and regla in (" + reglas_ + ")";
                            where_regla += "  AND CODVENDEDOR = '" + codvendedor + "'";

                        }
                    }
                    else
                    {
                        reglas_ = reglas;
                        where_regla += "  and ( regla in (" + reglas_ + ")  AND CODVENDEDOR = '" + codvendedor + "'   or regla in ('regla10', 'regla8', 'regla13', 'regla21')  )";

                    }

                }

                if (codvendedor == "PARRA")
                {
                    if (reglas_ != "'-- Todos --'")
                    {
                        if (reglas_ == "'Regla9'")
                        {
                            where_regla = " and regla in ('regla9')";

                        }
                        else if (reglas_ == "'Regla7'")
                        {
                            where_regla = " and regla in ('regla7')";
                        }
                        else if (reglas_ == "'Regla17'")
                        {
                            where_regla = " and regla in ('regla17')";
                        }
                        else
                        {
                            where_regla += " and regla in (" + reglas_ + ")";

                        }
                    }
                    else
                    {
                        reglas_ = reglas;
                        where_regla += "  and ( regla in (" + reglas_ + ") or regla in ('regla7', 'regla9', 'regla17') )";

                    }

                }




                llenar_facturas_resumen(periodo, where_regla, on_off_todos);


                DataTable totales = new DataTable();

                totales.Columns.Add("NombreRegla");
                totales.Columns.Add("Total");

                DataRow row;

                List<string> regla = reglas.Split(',').ToList();
                Double tota_comision_vendedor = 0;

                bool es_abarrote = false;
                foreach (string reg1a in regla)
                {
                    row = totales.NewRow();

                    DataView dv = new DataView(comisiones_resumen);
                    dv.RowFilter = "regla = " + reg1a; // query example = "id = 10"

                    if (reg1a.ToUpper() == "'REGLA16'")
                    {
                        es_abarrote = true;
                    }

                    DataTable filtradacomision = dv.ToTable();
                    if (filtradacomision.Rows.Count > 0)
                    {
                        double total_por_regla = 0;
                        double total_comision = 0;
                        string nombre_regla = "";
                        foreach (DataRow r1 in filtradacomision.Rows)
                        {


                            if (r1["porcentaje_edit"].ToString() != "")
                            {
                                double neto = Convert.ToDouble(r1["neto_pesos"].ToString());
                                double porncetaje = Convert.ToDouble(r1["porcentaje_edit"].ToString());
                                double comision_editada = Math.Round((neto * porncetaje) / 100);
                                total_por_regla += comision_editada;
                                total_comision += comision_editada;
                            }
                            else
                            {
                                total_por_regla += Convert.ToDouble(r1["montocomision"]);
                                total_comision += Convert.ToDouble(r1["montocomision"]);
                            }
                            nombre_regla = r1["nombre_comision"].ToString();

                        }
                        if (nombre_regla == "COMISION SOBRE COMISIONES RAFAEL PINO, ANDRES GONZALES")
                        {

                            //l_sobre_comision.Text = "30% sobre comisión: " + Base.monto_format2(Math.Round(total_por_regla * 0.25));
                            //row["NombreRegla"] = nombre_regla;
                            //row["Total"] = Math.Round(total_por_regla * 0.25);
                            //tota_comision_vendedor += Math.Round(total_por_regla * 0.25);

                            row["NombreRegla"] = nombre_regla;
                            row["Total"] = total_por_regla;
                            tota_comision_vendedor += total_por_regla;
                        }
                        else
                        {
                            row["NombreRegla"] = nombre_regla;
                            row["Total"] = total_por_regla;
                            tota_comision_vendedor += total_por_regla;
                        }
                        totales.Rows.Add(row);
                    }
                }

                row = totales.NewRow();
                row["NombreRegla"] = "::::TOTAL:::: ";
                row["Total"] = Math.Round(tota_comision_vendedor);
                totales.Rows.Add(row);

                if (codvendedor_aux == "AN044")
                {
                    string neto_navarro = ReporteRNegocio.neto_navarro(periodo);
                    row = totales.NewRow();
                    row["NombreRegla"] = "_:NETO:_";
                    row["Total"] = Base.monto_format(neto_navarro);
                    totales.Rows.Add(row);
                }

                string tabla = "";




                tabla += "<table class=\"table fill-head table-bordered\">";
                if (c_vendedor_r[2].ToString() == "1")
                {
                    tabla += "<thead style='background-color: #e49191 !important; color: white !important; '>";
                }
                else
                {
                    tabla += "<thead class=\"test\">";
                }

                tabla += "<tr>";
                tabla += "<th style='width: 33%;'>  NOMBRE  </th>";
                tabla += "<th style='width: 50%;'>Nombre de Regla</th>";
                tabla += "<th style='width: 17%;'>Total</th>";
                tabla += "</tr>";
                tabla += "</thead>";
                tabla += "<tbody>";

                if (codvendedor_aux == "JAIME BARGETTO")
                {
                    row2 = dt_total_resumen_com.NewRow();

                    tabla += "<tr>";
                    tabla += "<td>" + c_vendedor_r[1].ToString() + "</td>";
                    tabla += "<td> COMISION SOBRE COMISIONES ANDRES NAVARRO</td>";
                    tabla += "<td>" + Base.monto_format2(tota_comision_vendedor * 0.3) + "</td>";
                    tabla += "</tr>";

                    row2["codvendedor"] = c_vendedor_r[0].ToString().Trim();
                    row2["nombre_vendedor"] = c_vendedor_r[1].ToString().Trim();
                    row2["nombreregla"] = "COMISION SOBRE COMISIONES ANDRES NAVARRO";
                    row2["total"] = Base.monto_format2(tota_comision_vendedor * 0.3);
                    row2["es_abarrotes"] = es_abarrote;
                    dt_total_resumen_com.Rows.Add(row2);
                }
                else
                {
                    foreach (DataRow r2 in totales.Rows)
                    {

                        row2 = dt_total_resumen_com.NewRow();

                        tabla += "<tr>";
                        tabla += "<td>" + c_vendedor_r[1].ToString() + "</td>";
                        tabla += "<td>" + r2["NombreRegla"].ToString() + "</td>";
                        tabla += "<td>" + Base.monto_format2(Convert.ToDouble(r2["Total"].ToString())) + "</td>";
                        tabla += "</tr>";

                        row2["codvendedor"] = c_vendedor_r[0].ToString().Trim();
                        row2["nombre_vendedor"] = c_vendedor_r[1].ToString().Trim();
                        row2["nombreregla"] = r2["NombreRegla"].ToString();
                        row2["total"] = Base.monto_format2(Convert.ToDouble(r2["Total"].ToString()));
                        row2["es_abarrotes"] = es_abarrote;
                        dt_total_resumen_com.Rows.Add(row2);
                    }
                }
                tabla += "</tbody>";
                tabla += "</table>";
                tabla += "</br> ";
                tabla_final += tabla;
            }

            ////aca temporal se agrega AZOCAR Y CATALDO

            ////aca temporal se agrega AZOCAR Y CATALDO
            if (que_puede_ver != "Granos")
            {
                //Session["puede_ver"]

                DataTable dt_oficina_abarro = (DataTable)Session["DT_ABARROTES_OFICINA"];

                int sum1 = 0;
                int sum2 = 0;
                try
                {
                    sum1 = Convert.ToInt32(dt_oficina_abarro.Rows[0][0].ToString().Replace(",000000", ""));
                    sum2 = Convert.ToInt32(dt_oficina_abarro.Rows[0][1].ToString().Replace(",000000", ""));
                }
                catch
                {

                }

                string total_abarrote_con_arica = "";
                string desarrollo_mercado_ro = "";
                string total_abarrote_sin_Arica = "";
                try
                {
                    total_abarrote_con_arica = Base.monto_format(dt_oficina_abarro.Rows[0][0].ToString().Replace(",000000", ""));
                    desarrollo_mercado_ro = Base.monto_format(dt_oficina_abarro.Rows[0][1].ToString().Replace(",000000", ""));
                    total_abarrote_sin_Arica = Base.monto_format(dt_oficina_abarro.Rows[0][2].ToString().Replace(",000000", ""));
                }
                catch
                {


                }

                row2 = dt_total_resumen_com.NewRow();
                row2["codvendedor"] = "ABARROTES";
                row2["nombre_vendedor"] = "MARCO ANTONIO AZOCAR";
                row2["nombreregla"] = "TOTAL ABARROTES (C/ARICA)";
                row2["total"] = total_abarrote_con_arica;
                row2["es_abarrotes"] = true;
                dt_total_resumen_com.Rows.Add(row2);

                row2 = dt_total_resumen_com.NewRow();
                row2["codvendedor"] = "ABARROTES";
                row2["nombre_vendedor"] = "MARCO ANTONIO AZOCAR";
                row2["nombreregla"] = "DESARROLLO DE MERCADO (RO)";
                row2["total"] = desarrollo_mercado_ro;
                row2["es_abarrotes"] = true;
                dt_total_resumen_com.Rows.Add(row2);

                row2 = dt_total_resumen_com.NewRow();
                row2["codvendedor"] = "ABARROTES";
                row2["nombre_vendedor"] = "MARCO ANTONIO AZOCAR";
                row2["nombreregla"] = "::::TOTAL::::";
                row2["total"] = Base.monto_format2(sum1 + sum2);
                row2["es_abarrotes"] = true;
                dt_total_resumen_com.Rows.Add(row2);

                row2 = dt_total_resumen_com.NewRow();
                row2["codvendedor"] = "ABARROTES";
                row2["nombre_vendedor"] = "PATRICIA CATALDO";
                row2["nombreregla"] = "TOTAL ABARROTES (S/ARICA)";
                row2["total"] = total_abarrote_sin_Arica;
                row2["es_abarrotes"] = true;
                dt_total_resumen_com.Rows.Add(row2);

                row2 = dt_total_resumen_com.NewRow();
                row2["codvendedor"] = "ABARROTES";
                row2["nombre_vendedor"] = "PATRICIA CATALDO";
                row2["nombreregla"] = "::::TOTAL::::";
                row2["total"] = total_abarrote_sin_Arica;
                row2["es_abarrotes"] = true;
                dt_total_resumen_com.Rows.Add(row2);

                string tabla_azocar_y_cataldo = "";
                string color_th1 = "background-color: #e49191 !important; color: white !important;";

                tabla_azocar_y_cataldo += "<table class=\"table fill-head table-bordered\">";
                tabla_azocar_y_cataldo += "<thead class=\"test\">";
                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<th style='width: 33%;" + color_th1 + "'>  NOMBRE  </th>";
                tabla_azocar_y_cataldo += "<th style='width: 50%;" + color_th1 + "'>Nombre de Regla</th>";
                tabla_azocar_y_cataldo += "<th style='width: 17%;" + color_th1 + "'>Total</th>";
                tabla_azocar_y_cataldo += "</tr>";
                tabla_azocar_y_cataldo += "</thead>";
                tabla_azocar_y_cataldo += "<tbody>";

                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "MARCO ANTONIO AZOCAR";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "TOTAL ABARROTES (C/ARICA)";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += total_abarrote_con_arica;
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "</tr>";

                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "MARCO ANTONIO AZOCAR";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "DESARROLLO DE MERCADO (RO)";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += desarrollo_mercado_ro;
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "</tr>";

                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";

                tabla_azocar_y_cataldo += "::::TOTAL::::";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += Base.monto_format2(sum1 + sum2);
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "</tr>";

                tabla_azocar_y_cataldo += "</tbody>";
                tabla_azocar_y_cataldo += "</table>";
                tabla_azocar_y_cataldo += "</br> ";

                ///// cataldo

                tabla_azocar_y_cataldo += "<table class=\"table fill-head table-bordered\">";
                tabla_azocar_y_cataldo += "<thead >";
                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<th style='width: 33%;" + color_th1 + "'>  NOMBRE  </th>";
                tabla_azocar_y_cataldo += "<th style='width: 50%;" + color_th1 + "'>Nombre de Regla</th>";
                tabla_azocar_y_cataldo += "<th style='width: 17%;" + color_th1 + "'>Total</th>";
                tabla_azocar_y_cataldo += "</tr>";
                tabla_azocar_y_cataldo += "</thead>";
                tabla_azocar_y_cataldo += "<tbody>";

                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "PATRICIA CATALDO";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "TOTAL ABARROTES S/ARICA";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += total_abarrote_sin_Arica;
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "</tr>";

                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "::::TOTAL::::";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += total_abarrote_sin_Arica;
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "</tr>";

                tabla_azocar_y_cataldo += "</tbody>";
                tabla_azocar_y_cataldo += "</table>";
                tabla_azocar_y_cataldo += "</br> ";

                tabla_final += tabla_azocar_y_cataldo;

            }


            ///----

            Session["dt_total_resumen"] = dt_total_resumen_com;
            Session["periodo"] = txt_periodo.Text;


            string tabla_cobranza = "";
            double total_neto = 0;
            ////---------------------------------------------------------------------------------------RESUMEN COMISIONES DE COBRANZA
            dt_total_cobranza = new DataTable();
            //dt_total_resumen_com.NewRow();
            dt_total_cobranza.Columns.Add("nombre_cobran");
            dt_total_cobranza.Columns.Add("neto");
            dt_total_cobranza.Columns.Add("porct");
            dt_total_cobranza.Columns.Add("total");

            DataRow row3;

            //ROSA SOLIS .....................------------------------------------------------------------------------------------------

            //pdf
            row3 = dt_total_cobranza.NewRow();
            row3["nombre_cobran"] = "ROSA SOLIS V.";
            row3["neto"] = "NETO";
            row3["porct"] = "PORCT";
            row3["total"] = "TOTAL";
            dt_total_cobranza.Rows.Add(row3);

            //web
            tabla_cobranza += "<table class=\"table fill-head table-bordered\">";
            tabla_cobranza += "<thead style='background-color: #4d9850 !important; color: white !important; '>";
            tabla_cobranza += "<tr>";
            tabla_cobranza += "<th style='width: 40%;'>  ROSA SOLIS V. </th>";
            tabla_cobranza += "<th style='width: 28%;'>NETO </th>";
            tabla_cobranza += "<th style='width: 15%;'>PORCT </th>";
            tabla_cobranza += "<th style='width: 17%;'>TOTAL</th>";
            tabla_cobranza += "</tr>";
            tabla_cobranza += "</thead>";
            tabla_cobranza += "<tbody>";

            double total_comision_cobranza = 0;
            foreach (DataRow r4 in comision_cobranza.Rows)
            {

                row3 = dt_total_cobranza.NewRow();

                string nombre_Categoria = r4[0].ToString();

                double neto_categoria = Convert.ToDouble(r4[1].ToString());
                total_neto += neto_categoria;

                string porcentaje = r4[2].ToString() + " %";

                double comision_categoria = Convert.ToDouble(r4[5].ToString());
                total_comision_cobranza += comision_categoria;

                //pdf
                row3 = dt_total_cobranza.NewRow();
                row3["nombre_cobran"] = nombre_Categoria;
                row3["neto"] = Base.monto_format2(neto_categoria);
                row3["porct"] = porcentaje;
                row3["total"] = Base.monto_format2(comision_categoria);
                dt_total_cobranza.Rows.Add(row3);

                //web
                tabla_cobranza += "<tr>";
                tabla_cobranza += "<td>" + nombre_Categoria + "</td>";
                tabla_cobranza += "<td>" + Base.monto_format2(neto_categoria) + "</td>";
                if (Convert.ToDouble(r4[2].ToString()) == 0)
                {
                    tabla_cobranza += "<td></td>";
                }
                else
                {
                    tabla_cobranza += "<td>" + porcentaje + "</td>";
                }
                tabla_cobranza += "<td>" + Base.monto_format2(comision_categoria) + "</td>";
                tabla_cobranza += "</tr>";
            }
            //pdf
            row3 = dt_total_cobranza.NewRow();
            row3["nombre_cobran"] = ":::::TOTAL::::::";
            row3["neto"] = Base.monto_format2(total_neto);
            row3["porct"] = "";
            row3["total"] = Base.monto_format2(total_comision_cobranza);
            dt_total_cobranza.Rows.Add(row3);

            //web
            tabla_cobranza += "<tr>";
            tabla_cobranza += "<td>:::::TOTAL::::::</td>";
            tabla_cobranza += "<td>" + Base.monto_format2(total_neto) + "</td>";
            tabla_cobranza += "<td></td>";
            tabla_cobranza += "<td>" + Base.monto_format2(total_comision_cobranza) + "</td>";
            tabla_cobranza += "</tr>";

            tabla_cobranza += "</tbody>";
            tabla_cobranza += "</table>";
            tabla_cobranza += "</br> ";


            //EVELYN LEIVA .....................------------------------------------------------------------------------------------------

            //pdf
            row3 = dt_total_cobranza.NewRow();
            row3["nombre_cobran"] = "EVELYN LEIVA";
            row3["neto"] = "NETO";
            row3["porct"] = "PORCT";
            row3["total"] = "TOTAL";
            dt_total_cobranza.Rows.Add(row3);

            //web
            tabla_cobranza += "<table class=\"table fill-head table-bordered\">";
            tabla_cobranza += "<thead style='background-color: #4d9850 !important; color: white !important; '>";
            tabla_cobranza += "<tr>";
            tabla_cobranza += "<th style='width: 40%;'>EVELYN LEIVA </th>";
            tabla_cobranza += "<th style='width: 28%;'>NETO </th>";
            tabla_cobranza += "<th style='width: 15%;'>PORCT </th>";
            tabla_cobranza += "<th style='width: 17%;'>TOTAL</th>";
            tabla_cobranza += "</tr>";
            tabla_cobranza += "</thead>";
            tabla_cobranza += "<tbody>";

            total_comision_cobranza = 0;
            foreach (DataRow r4 in comision_cobranza.Rows)
            {

                row3 = dt_total_cobranza.NewRow();

                string nombre_Categoria = r4[0].ToString();

                double neto_categoria = Convert.ToDouble(r4[1].ToString());

                string porcentaje = r4[3].ToString() + " %";

                double comision_categoria = Convert.ToDouble(r4[6].ToString());
                total_comision_cobranza += comision_categoria;

                //pdf
                row3 = dt_total_cobranza.NewRow();
                row3["nombre_cobran"] = nombre_Categoria;
                row3["neto"] = Base.monto_format2(neto_categoria);
                row3["porct"] = porcentaje;
                row3["total"] = Base.monto_format2(comision_categoria);
                dt_total_cobranza.Rows.Add(row3);

                //web
                tabla_cobranza += "<tr>";
                tabla_cobranza += "<td>" + nombre_Categoria + "</td>";
                tabla_cobranza += "<td>" + Base.monto_format2(neto_categoria) + "</td>";

                if (Convert.ToDouble(r4[3].ToString()) == 0)
                {
                    tabla_cobranza += "<td></td>";
                }
                else
                {
                    tabla_cobranza += "<td>" + porcentaje + "</td>";
                }

                tabla_cobranza += "<td>" + Base.monto_format2(comision_categoria) + "</td>";
                tabla_cobranza += "</tr>";
            }
            //pdf
            row3 = dt_total_cobranza.NewRow();
            row3["nombre_cobran"] = ":::::TOTAL::::::";
            row3["neto"] = Base.monto_format2(total_neto);
            row3["porct"] = "";
            row3["total"] = Base.monto_format2(total_comision_cobranza);
            dt_total_cobranza.Rows.Add(row3);

            //web
            tabla_cobranza += "<tr>";
            tabla_cobranza += "<td>:::::TOTAL::::::</td>";
            tabla_cobranza += "<td>" + Base.monto_format2(total_neto) + "</td>";
            tabla_cobranza += "<td></td>";
            tabla_cobranza += "<td>" + Base.monto_format2(total_comision_cobranza) + "</td>";
            tabla_cobranza += "</tr>";

            tabla_cobranza += "</tbody>";
            tabla_cobranza += "</table>";
            tabla_cobranza += "</br> ";


            //MARIANA SILVA .....................------------------------------------------------------------------------------------------
            //tabla_cobranza += "<table class=\"table fill-head table-bordered\">";
            //tabla_cobranza += "<thead style='background-color: #4d9850 !important; color: white !important; '>";
            //tabla_cobranza += "<tr>";
            //tabla_cobranza += "<th style='width: 40%;'>MARIANA SILVA</th>";
            //tabla_cobranza += "<th style='width: 28%;'>NETO </th>";
            //tabla_cobranza += "<th style='width: 15%;'>PORCT </th>";
            //tabla_cobranza += "<th style='width: 17%;'>TOTAL</th>";
            //tabla_cobranza += "</tr>";
            //tabla_cobranza += "</thead>";
            //tabla_cobranza += "<tbody>";

            //total_comision_cobranza = 0;
            //foreach (DataRow r4 in comision_cobranza.Rows)
            //{
            //    row3 = dt_total_cobranza.NewRow();

            //    string nombre_Categoria = r4[0].ToString();

            //    double neto_categoria = Convert.ToDouble(r4[1].ToString());

            //    string porcentaje = r4[4].ToString() + " %";

            //    double comision_categoria = Convert.ToDouble(r4[7].ToString());
            //    total_comision_cobranza += comision_categoria;

            //    //pdf
            //    row3 = dt_total_cobranza.NewRow();
            //    row3["nombre_cobran"] = nombre_Categoria;
            //    row3["neto"] = Base.monto_format2(neto_categoria);
            //    row3["porct"] = porcentaje;
            //    row3["total"] = Base.monto_format2(comision_categoria);
            //    dt_total_cobranza.Rows.Add(row3);

            //    //web
            //    tabla_cobranza += "<tr>";
            //    tabla_cobranza += "<td>" + r4[0].ToString() + "</td>";
            //    tabla_cobranza += "<td>" + Base.monto_format2(neto_categoria) + " </td>";

            //    if (Convert.ToDouble(r4[4].ToString()) == 0)
            //    {
            //        tabla_cobranza += "<td></td>";
            //    }
            //    else
            //    {
            //        tabla_cobranza += "<td>" + porcentaje + "</td>";
            //    }

            //    tabla_cobranza += "<td>" + Base.monto_format2(comision_categoria) + "</td>";
            //    tabla_cobranza += "</tr>";
            //}
            ////pdf
            //row3 = dt_total_cobranza.NewRow();
            //row3["nombre_cobran"] = ":::::TOTAL::::::";
            //row3["neto"] = Base.monto_format2(total_neto);
            //row3["porct"] = "";
            //row3["total"] = Base.monto_format2(total_comision_cobranza);
            //dt_total_cobranza.Rows.Add(row3);

            ////web
            //tabla_cobranza += "<tr>";
            //tabla_cobranza += "<td>:::::TOTAL::::::</td>";
            //tabla_cobranza += "<td>" + Base.monto_format2(total_neto) + "</td>";
            //tabla_cobranza += "<td></td>";
            //tabla_cobranza += "<td>" + Base.monto_format2(total_comision_cobranza) + "</td>";
            //tabla_cobranza += "</tr>";
            //tabla_cobranza += "</tbody>";
            //tabla_cobranza += "</table>";
            //tabla_cobranza += "</br> ";

            Session["dt_cobranza_com_resu"] = dt_total_cobranza;

            tabla_final += tabla_cobranza;
            //comisiones cobranza

            div_tabla_totales.InnerHtml = tabla_final;

            string tipo_comision = "3";
            string muestra_o_no = "0";

            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "PDF_CLICK", "<script language='javascript'>pdf_comision(" + tipo_comision + ", " + muestra_o_no + ");</script>", false);

            //UpdatePanel2.Update();

            //
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd121mk3mp", "<script language='javascript'>tabla();</script>", false);


        }

        protected void btn_correo_ServerClick(object sender, EventArgs e)
        {
            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress("comisiones@soprodi.cl"));
            //email.To.Add(new MailAddress("ESTEBAN.GODOY15@GMAIL.COM"));
            email.From = new MailAddress("informes@soprodi.cl");
            string periodo = txt_periodo.Text;

            email.Subject = "COMISION PERIODO (" + periodo + " ) ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
            //string correo_vendedor_por_cliente = ReporteRNegocio.trae_corr_vend_por_cliente(rutcliente.Replace("-", "").Replace(".", ""));
            //if (correo_vendedor_por_cliente != "")
            //{
            //    email.CC.Add(correo_vendedor_por_cliente);
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
            email.Body += "<div> Estimados :<br> <br> Resumen Comisión :  <b>" + "" + "USUARIO SGC(" + User.Identity.Name + ") </b> <br><br>";
            email.Body += "<div>Finanzas a autorizado el pago de las siguientes comisiones (ADJUNTAS)</div> <br><br>";
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
                string ruta2 = AppDomain.CurrentDomain.BaseDirectory + "/PDFs/PDF_SGC.pdf";
                crear_pdf();
                System.IO.FileInfo toDownload = new System.IO.FileInfo(ruta2);
                if (toDownload.Exists)
                {
                    email.Attachments.Add(new System.Net.Mail.Attachment(ruta2));
                }
                smtp.Send(email);
                email.Dispose();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "13", "<script language='javascript'>alert('Correo Enviado!');</script>", false);


            }
            catch (Exception ex)
            {

            }


            /////desde gmail

            //email.IsBodyHtml = true;
            //email.Priority = MailPriority.Normal;
            //email.BodyEncoding = System.Text.Encoding.UTF8;
            //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            //smtp.EnableSsl = true;
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.UseDefaultCredentials = false;

            //////smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            //smtp.Credentials = new NetworkCredential("informes.soprodi@gmail.com", "galaxia1234");
            //try
            //{
            //    string ruta2 = AppDomain.CurrentDomain.BaseDirectory + "/PDFs/PDF_SGC.pdf";
            //    crear_pdf();
            //    System.IO.FileInfo toDownload = new System.IO.FileInfo(ruta2);
            //    if (toDownload.Exists)
            //    {
            //        email.Attachments.Add(new System.Net.Mail.Attachment(ruta2));
            //    }
            //    smtp.Send(email);
            //    email.Dispose();
            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "13", "<script language='javascript'>alert('Correo Enviado!');</script>", false);

            //}
            //catch (Exception ex)
            //{

            //}

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd121m123k3mp", "<script language='javascript'>TABLITA();</script>", false);


        }

        private void crear_pdf()
        {

            calcular_pdf();

            DataTable dt_total_resumen_com = (DataTable)Session["dt_total_resumen"];

            DataTable dt_cobranza_com = (DataTable)Session["dt_cobranza_com_resu"];

            DataTable usuarios_firman = ReporteRNegocio.usuarios_firman(Session["periodo"].ToString());

            string n_file = "/PDF_SGC.pdf";
            string pdfPath = Server.MapPath(@"~\PDFs\") + n_file;

            string tipo_comision = "3";
            string MUESTRA_O_NO = "0";

            using (FileStream msReport = new FileStream(pdfPath, FileMode.Create))
            {
                //step 1
                using (Document pdfDoc = new Document(PageSize.LETTER))
                {
                    try
                    {
                        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, msReport);
                        //pdfWriter.PageEvent = new Common.ITextEvents();

                        ITextEvents PageEventHandler = new ITextEvents();
                        pdfWriter.PageEvent = PageEventHandler;

                        //open the stream 
                        pdfDoc.Open();

                        // Creamos la imagen y le ajustamos el tamaño
                        iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(Server.MapPath("~/img/Sopro2.jpg"));
                        //imagen.BorderWidth = 0;
                        imagen.WidthPercentage = 20;
                        imagen.Alignment = Element.ALIGN_RIGHT;
                        float percentage = 0.0f;
                        percentage = 100 / imagen.Width;
                        imagen.ScalePercent(percentage * 100);


                        PdfPTable tabla_imagen = new PdfPTable(1);
                        tabla_imagen.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tabla_imagen.WidthPercentage = 50;
                        PdfPCell imagen_td = new PdfPCell(imagen);
                        imagen_td.BorderWidth = 0;
                        tabla_imagen.AddCell(imagen_td);

                        //PdfPCell imagen2_td = new PdfPCell(imagen2);
                        //imagen2_td.BorderWidth = 0;
                        //tabla_imagen.AddCell(imagen2_td);

                        //PdfPCell imagen3_td = new PdfPCell(imagen3);
                        //imagen3_td.BorderWidth = 0;
                        //tabla_imagen.AddCell(imagen3_td);

                        PdfPCell vacio = new PdfPCell(new Phrase("  ", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        vacio.BorderWidth = 0;

                        tabla_imagen.AddCell(vacio);
                        tabla_imagen.AddCell(vacio);
                        tabla_imagen.AddCell(vacio);

                        PdfPTable tabla_titulo = new PdfPTable(1);
                        string titulo = "COMISIONES " + Session["periodo"].ToString();
                        PdfPCell TD_TITULO = new PdfPCell(new Phrase(titulo, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)));
                        TD_TITULO.BorderWidth = 0;
                        TD_TITULO.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabla_titulo.AddCell(TD_TITULO);

                        PdfPTable tabla_fecha_hora = new PdfPTable(1);
                        string fecha_hora = DateTime.Now.ToString();
                        PdfPCell td_fecha_hora = new PdfPCell(new Phrase(fecha_hora, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9)));
                        td_fecha_hora.BorderWidth = 0;
                        td_fecha_hora.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabla_fecha_hora.AddCell(td_fecha_hora);


                        PdfPTable tabla_Vacia = new PdfPTable(1);
                        PdfPCell td_vacia = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        td_vacia.BorderWidth = 0;
                        tabla_Vacia.AddCell(td_vacia);

                        pdfDoc.Add(imagen);
                        pdfDoc.Add(tabla_titulo);
                        pdfDoc.Add(tabla_fecha_hora);
                        pdfDoc.Add(tabla_Vacia);

                        ////Escribimos el encabezamiento en el documento
                        PdfPTable tabla = new PdfPTable(3);
                        tabla.WidthPercentage = 100;

                        PdfPCell th1 = new PdfPCell(new Phrase("NOMBRE", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        th1.HorizontalAlignment = Element.ALIGN_CENTER;
                        //th1.BorderWidth = 0;
                        tabla.AddCell(th1).BackgroundColor = new BaseColor(229, 229, 229, 229); ;

                        PdfPCell th2 = new PdfPCell(new Phrase("NOMBRE REGLA", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        th2.HorizontalAlignment = Element.ALIGN_CENTER;
                        //th2.BorderWidth = 0;
                        tabla.AddCell(th2).BackgroundColor = new BaseColor(229, 229, 229, 229); ;

                        PdfPCell th3 = new PdfPCell(new Phrase("TOTAL", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        th3.HorizontalAlignment = Element.ALIGN_CENTER;
                        //th3.BorderWidth = 0;
                        tabla.AddCell(th3).BackgroundColor = new BaseColor(229, 229, 229, 229); ;

                        //pdfDoc.Add(tabla);

                        string nombre_vendedor = "";
                        int cont = 0;
                        //string tipo_comision = Session["tipo_comision"].ToString();


                        DataTable dt_tolta_resume_com_aux = dt_total_resumen_com.Clone();

                        ///quitar los abarrotes
                        foreach (DataRow r2 in dt_total_resumen_com.Rows)
                        {
                            if (tipo_comision == "2")
                            {
                                if (r2[4].ToString() == "True")
                                {

                                    dt_tolta_resume_com_aux.ImportRow(r2);

                                    string aca = "";
                                    //dt_total_resumen_com.Rows.Remove(r2);
                                    //r2.Delete();

                                }
                            }
                            else if (tipo_comision == "1")
                            {
                                if (r2[4].ToString() != "True")
                                {
                                    dt_tolta_resume_com_aux.ImportRow(r2);

                                    string aca2 = "";
                                    //dt_total_resumen_com.Rows.Remove(r2);
                                    //r2.Delete();

                                }
                            }
                            else
                            {
                                dt_tolta_resume_com_aux.ImportRow(r2);

                            }
                        }

                        foreach (DataRow r in dt_tolta_resume_com_aux.Rows)
                        {

                            cont++;


                            if (nombre_vendedor != r[1].ToString())
                            {
                                if (cont > 1)
                                {
                                    tabla.AddCell(vacio);
                                    tabla.AddCell(vacio);
                                    tabla.AddCell(vacio);


                                    tabla.AddCell(vacio);
                                    tabla.AddCell(vacio);
                                    tabla.AddCell(vacio);
                                }

                                if (r[2].ToString().Contains("ABARROTES"))
                                {
                                    PdfPCell td1 = new PdfPCell(new Phrase(r[1].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                    td1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //td1.BorderWidth = 0;
                                    tabla.AddCell(td1).BackgroundColor = new BaseColor(240, 128, 128);
                                }
                                else
                                {


                                    PdfPCell td1 = new PdfPCell(new Phrase(r[1].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                    td1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //td1.BorderWidth = 0;
                                    tabla.AddCell(td1).BackgroundColor = new BaseColor(135, 206, 235);



                                }

                                nombre_vendedor = r[1].ToString();
                            }

                            else
                            {

                                PdfPCell td1 = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                td1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //td1.BorderWidth = 0;
                                tabla.AddCell(td1);
                                nombre_vendedor = r[1].ToString();
                            }


                            PdfPCell td2 = new PdfPCell(new Phrase(r[2].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                            td2.HorizontalAlignment = Element.ALIGN_LEFT;
                            //td2.BorderWidth = 0;
                            tabla.AddCell(td2);

                            PdfPCell td3 = new PdfPCell(new Phrase(r[3].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                            td3.HorizontalAlignment = Element.ALIGN_LEFT;
                            //td3.BorderWidth = 0;
                            tabla.AddCell(td3);
                            //PdfPTable table_ventas = new PdfPTable(11);
                            //table_ventas.WidthPercentage = 100;

                            //table_ventas.AddCell(dia).BackgroundColor = new BaseColor(229, 229, 229, 229);                 


                        }

                        tabla.AddCell(vacio);
                        tabla.AddCell(vacio);


                        ////////------------------------------------------------------------------------------COMISION COBRANZA
                        PdfPTable tabla_COBRANZA = new PdfPTable(4);
                        tabla_COBRANZA.WidthPercentage = 100;

                        tabla_COBRANZA.AddCell(vacio);
                        tabla_COBRANZA.AddCell(vacio);
                        tabla_COBRANZA.AddCell(vacio);
                        tabla_COBRANZA.AddCell(vacio);


                        tabla_COBRANZA.AddCell(vacio);
                        tabla_COBRANZA.AddCell(vacio);
                        tabla_COBRANZA.AddCell(vacio);
                        tabla_COBRANZA.AddCell(vacio);


                        tabla_COBRANZA.AddCell(vacio);
                        tabla_COBRANZA.AddCell(vacio);
                        tabla_COBRANZA.AddCell(vacio);
                        tabla_COBRANZA.AddCell(vacio);

                        cont = 0;
                        bool final = true;
                        foreach (DataRow r3 in dt_cobranza_com.Rows)
                        {
                            if (final)
                            {


                                tabla_COBRANZA.AddCell(vacio);
                                tabla_COBRANZA.AddCell(vacio);
                                tabla_COBRANZA.AddCell(vacio);
                                tabla_COBRANZA.AddCell(vacio);


                                PdfPCell td1 = new PdfPCell(new Phrase(r3[0].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                td1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //td1.BorderWidth = 0;
                                tabla_COBRANZA.AddCell(td1).BackgroundColor = new BaseColor(30, 176, 40);

                                PdfPCell td2 = new PdfPCell(new Phrase(r3[1].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                td1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //td1.BorderWidth = 0;
                                tabla_COBRANZA.AddCell(td2).BackgroundColor = new BaseColor(30, 176, 40);

                                PdfPCell td3 = new PdfPCell(new Phrase(r3[2].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                td1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //td1.BorderWidth = 0;
                                tabla_COBRANZA.AddCell(td3).BackgroundColor = new BaseColor(30, 176, 40);

                                PdfPCell td4 = new PdfPCell(new Phrase(r3[3].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                td1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //td1.BorderWidth = 0;
                                tabla_COBRANZA.AddCell(td4).BackgroundColor = new BaseColor(30, 176, 40);

                                final = false;
                            }
                            else
                            {
                                PdfPCell td1 = new PdfPCell(new Phrase(r3[0].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                td1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //td2.BorderWidth = 0;
                                tabla_COBRANZA.AddCell(td1);

                                PdfPCell td2 = new PdfPCell(new Phrase(r3[1].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                td2.HorizontalAlignment = Element.ALIGN_LEFT;
                                //td2.BorderWidth = 0;
                                tabla_COBRANZA.AddCell(td2);

                                PdfPCell td3 = new PdfPCell(new Phrase(r3[2].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                td3.HorizontalAlignment = Element.ALIGN_LEFT;
                                //td2.BorderWidth = 0;
                                tabla_COBRANZA.AddCell(td3);

                                PdfPCell td4 = new PdfPCell(new Phrase(r3[3].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                td4.HorizontalAlignment = Element.ALIGN_LEFT;
                                //td2.BorderWidth = 0;
                                tabla_COBRANZA.AddCell(td4);

                            }
                            if (r3[0].ToString().Contains("TOTAL"))
                            {
                                final = true;
                            }




                        }


                        float[] medidasCeldas = { 0.30f, 0.50f, 0.15f };
                        tabla.SetWidths(medidasCeldas);

                        float[] medidasCeldas_4 = { 0.38f, 0.28f, 0.15f, 0.16f };
                        tabla_COBRANZA.SetWidths(medidasCeldas_4);

                        pdfDoc.Add(tabla);
                        pdfDoc.Add(tabla_COBRANZA);



                        ////////------------------------------------------------------------------------------FIRMAS
                        //PdfPTable tabla_firmas = new PdfPTable(usuarios_firman.Rows.Count);

                        PdfPTable tabla_firmas = new PdfPTable(3);

                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);

                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);

                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);

                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);

                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);

                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);

                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);

                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);


                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);

                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);

                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);


                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);

                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);

                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);

                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);

                        tabla_firmas.WidthPercentage = 100;

                        PdfPCell th1_F = new PdfPCell(new Phrase("GRUPO", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        th1_F.HorizontalAlignment = Element.ALIGN_CENTER;
                        //th1.BorderWidth = 0;
                        tabla_firmas.AddCell(th1_F).BackgroundColor = new BaseColor(229, 229, 229, 229); ;

                        PdfPCell th2_F = new PdfPCell(new Phrase("FIRMA", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        th2_F.HorizontalAlignment = Element.ALIGN_CENTER;
                        //th2.BorderWidth = 0;
                        tabla_firmas.AddCell(th2_F).BackgroundColor = new BaseColor(229, 229, 229, 229); ;

                        PdfPCell th3_F = new PdfPCell(new Phrase("NOMBRE", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        th3_F.HorizontalAlignment = Element.ALIGN_CENTER;
                        //th3.BorderWidth = 0;
                        tabla_firmas.AddCell(th3_F).BackgroundColor = new BaseColor(229, 229, 229, 229); ;

                        foreach (DataRow r1 in usuarios_firman.Rows)
                        {

                            PdfPCell td1 = new PdfPCell(new Phrase(r1[0].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                            td1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //td2.BorderWidth = 0;
                            tabla_firmas.AddCell(td1);


                            if (r1[1].ToString().Trim() == "True")
                            {
                                PdfPCell td2 = new PdfPCell(new Phrase("OK", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                td2.HorizontalAlignment = Element.ALIGN_LEFT;
                                //td2.BorderWidth = 0;
                                tabla_firmas.AddCell(td2).BackgroundColor = new BaseColor(59, 134, 50);
                            }
                            else
                            {
                                PdfPCell td2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                td2.HorizontalAlignment = Element.ALIGN_LEFT;
                                //td2.BorderWidth = 0;
                                tabla_firmas.AddCell(td2).BackgroundColor = new BaseColor(239, 36, 36);
                            }

                            PdfPCell td3 = new PdfPCell(new Phrase(r1[3].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                            td3.HorizontalAlignment = Element.ALIGN_LEFT;
                            //td2.BorderWidth = 0;
                            tabla_firmas.AddCell(td3);
                        }


                        //agregamos tabla de las firmas
                        tabla_firmas.SetWidths(medidasCeldas);
                        pdfDoc.Add(tabla_firmas);


                        pdfDoc.NewPage();

                        pdfDoc.Close();
                        //writer.Close();
                        //string pdfPath = Server.MapPath("~/PDF_Ventas/" + n_file);
                        WebClient client = new WebClient();
                        Byte[] buffer = client.DownloadData(pdfPath);

                        //document.Add(new Paragraph("Hello World!"));

                        if (MUESTRA_O_NO == "0")
                        {


                        }
                        else
                        {

                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-length", buffer.Length.ToString());
                            Response.BinaryWrite(buffer);
                            Response.Flush();
                            Response.End();


                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }


        }

        private void calcular_pdf()
        {
            string tabla_final = "";
            string que_puede_ver = Session["puede_ver"].ToString();

            string mes = DateTime.Parse(txt_fecha.Text).Month.ToString();
            if (mes.Length < 2)
            {

                mes = "0" + mes;
            }

            string periodo = DateTime.Parse(txt_fecha.Text).Year.ToString() + mes;


            ///ACA INCLUIR CALCULO COMISION COBRANZA ----->
            ///
            ///CALCULO EN BASE A SISTEMA DE COBRANZA (CALENDARIO)
            //DataTable comision_cobranza = ReporteRNegocio.comision_cobranza(periodo);

            ///CALCULO EN BASE A SISTEMA DE COMISIONES
            DataTable comision_cobranza = ReporteRNegocio.comision_cobranza_2(periodo);
            //no lo uso ->

            //DataTable categorias_cobranza = ReporteRNegocio.categoria_cobranza_comis();

            try
            {
                dt_vendedores.Columns.Add("es_abarrotes", typeof(double));
            }
            catch
            {
            }
            foreach (DataRow c_vendedor_r in dt_vendedores.Rows)
            {
                //select* from[dbo].[V_COMISION_VENDEDORES]
                double abarrotes = ReporteRNegocio.tiene_regla_abarrote(c_vendedor_r[0].ToString());
                c_vendedor_r[2] = abarrotes;
            }


            DataView dv3 = dt_vendedores.DefaultView;
            dv3.Sort = "es_abarrotes";
            dt_vendedores = dv3.ToTable();

            dt_total_resumen_com = new DataTable();
            //dt_total_resumen_com.NewRow();
            dt_total_resumen_com.Columns.Add("codvendedor");
            dt_total_resumen_com.Columns.Add("nombre_vendedor");
            dt_total_resumen_com.Columns.Add("nombreregla");
            dt_total_resumen_com.Columns.Add("total");
            dt_total_resumen_com.Columns.Add("es_abarrotes");

            DataRow row2;


            foreach (DataRow c_vendedor_r in dt_vendedores.Rows)
            {
                string codvendedor = c_vendedor_r[0].ToString();
                string reglas = agregar_comillas(ReporteRNegocio.trae_reglas(codvendedor));


                string reglas_ = "'-- Todos --'";

                string regla_aux = reglas_;

                string where_regla = "";

                int on_off_todos = 2;

                l_sobre_comision.Text = "";
                l_total_comision.Text = "";

                string codvendedor_aux = codvendedor;
                if (codvendedor == "JAIME BARGETTO")
                {
                    codvendedor = "AN044";
                    reglas = agregar_comillas(ReporteRNegocio.trae_reglas(codvendedor));
                }


                if (codvendedor != "CA001")
                {
                    if (codvendedor != "GT015")
                    {


                        if (codvendedor != "PARRA")
                        {
                            if (reglas_ != "'-- Todos --'")
                            {
                                where_regla += " and regla in (" + reglas_ + ")";
                            }
                            else
                            {

                                where_regla += " and regla in (" + reglas + ")";

                            }
                            where_regla += "  AND CODVENDEDOR = '" + codvendedor + "'";
                        }

                    }
                }

                if (codvendedor == "CA001")
                {
                    //regla 11
                    if (reglas_ != "'-- Todos --'")
                    {
                        if (reglas_ == "'Regla8'")
                        {
                            where_regla = " and regla in ('regla8')";

                        }
                        else if (reglas_ == "'Regla4'")
                        {
                            where_regla = " and regla in ('regla4')";
                        }
                        else if (reglas_ == "'Regla11'")
                        {
                            where_regla = " and regla in ('Regla11')";
                        }
                        else if (reglas_ == "'regla19'")
                        {
                            where_regla = " and regla in ('regla19')";
                        }
                        else
                        {
                            where_regla += " and regla in (" + reglas_ + ")";
                            where_regla += "  AND CODVENDEDOR = '" + codvendedor + "'";
                        }
                    }
                    else
                    {
                        reglas_ = reglas;
                        where_regla += " and ( regla in (" + reglas_ + ")  AND CODVENDEDOR = '" + codvendedor + "' or regla in ('regla4' , 'regla8', 'regla11', 'regla19' ) )";
                    }
                }
                if (codvendedor == "GT015")
                {
                    if (reglas_ != "'-- Todos --'")
                    {
                        if (reglas_ == "'Regla8'")
                        {
                            where_regla = " and regla in ('regla8')";
                        }
                        else if (reglas_ == "'Regla10'")
                        {
                            where_regla = " and regla in ('regla10')";
                        }
                        else if (reglas_ == "'Regla13'")
                        {
                            where_regla = " and regla in ('regla13')";
                        }
                        else
                        {
                            where_regla += " and regla in (" + reglas_ + ")";
                            where_regla += "  AND CODVENDEDOR = '" + codvendedor + "'";

                        }
                    }
                    else
                    {
                        reglas_ = reglas;
                        where_regla += "  and ( regla in (" + reglas_ + ")  AND CODVENDEDOR = '" + codvendedor + "'   or regla in ('regla10', 'regla8', 'regla18', 'regla13', 'regla21') )";
                    }
                }
                if (codvendedor == "PARRA")
                {
                    if (reglas_ != "'-- Todos --'")
                    {
                        if (reglas_ == "'Regla9'")
                        {
                            where_regla = " and regla in ('regla9')";

                        }
                        else if (reglas_ == "'Regla7'")
                        {
                            where_regla = " and regla in ('regla7')";
                        }
                        else if (reglas_ == "'Regla17'")
                        {
                            where_regla = " and regla in ('regla17')";
                        }
                        else
                        {
                            where_regla += " and regla in (" + reglas_ + ")";

                        }
                    }
                    else
                    {
                        reglas_ = reglas;
                        where_regla += "  and ( regla in (" + reglas_ + ") or regla in ('regla7', 'regla9', 'regla17') )";

                    }

                }

                llenar_facturas_resumen(periodo, where_regla, on_off_todos);

                DataTable totales = new DataTable();

                totales.Columns.Add("NombreRegla");
                totales.Columns.Add("Total");

                DataRow row;

                List<string> regla = reglas.Split(',').ToList();
                Double tota_comision_vendedor = 0;

                double agroin_amaro = 0;
                try

                {
                    agroin_amaro = (double)Session["comision_amaro_agroin"];
                }
                catch
                {
                    agroin_amaro = 0;

                }


                bool es_abarrote = false;

                //que_puede_ver
                foreach (string reg1a in regla)
                {
                    row = totales.NewRow();

                    DataView dv = new DataView(comisiones_resumen);
                    dv.RowFilter = "regla = " + reg1a; // query example = "id = 10"

                    if (reg1a.ToUpper() == "'REGLA16'")
                    {
                        es_abarrote = true;
                    }


                    DataTable filtradacomision = dv.ToTable();
                    if (filtradacomision.Rows.Count > 0)
                    {
                        double total_por_regla = 0;
                        double total_comision = 0;
                        string nombre_regla = "";
                        foreach (DataRow r1 in filtradacomision.Rows)
                        {


                            if (r1["porcentaje_edit"].ToString() != "")
                            {
                                double neto = Convert.ToDouble(r1["neto_pesos"].ToString());
                                double porncetaje = Convert.ToDouble(r1["porcentaje_edit"].ToString());
                                double comision_editada = Math.Round((neto * porncetaje) / 100);
                                total_por_regla += comision_editada;
                                total_comision += comision_editada;
                            }
                            else
                            {
                                total_por_regla += Convert.ToDouble(r1["montocomision"]);
                                total_comision += Convert.ToDouble(r1["montocomision"]);
                            }
                            nombre_regla = r1["nombre_comision"].ToString();

                        }

                        if (reg1a.Contains("regla19"))
                        {
                            total_por_regla += agroin_amaro;
                            total_comision += agroin_amaro;
                        }

                        if (nombre_regla == "COMISION SOBRE COMISIONES  RAFAEL PINO, ANDRES GONZALES")
                        {
                            //l_sobre_comision.Text = "30% sobre comisión: " + Base.monto_format2(Math.Round(total_por_regla * 0.25));
                            //row["NombreRegla"] = nombre_regla;
                            //row["Total"] = Math.Round(total_por_regla * 0.25);
                            //tota_comision_vendedor += Math.Round(total_por_regla * 0.25);

                            row["NombreRegla"] = nombre_regla;
                            row["Total"] = total_por_regla;
                            tota_comision_vendedor += total_por_regla;

                        }
                        else
                        {
                            row["NombreRegla"] = nombre_regla;
                            row["Total"] = total_por_regla;
                            tota_comision_vendedor += total_por_regla;
                        }
                        totales.Rows.Add(row);
                    }

                    else
                    {

                        if (reg1a.Contains("Regla19"))
                        {
                            double total_por_regla = agroin_amaro;
                            double total_comision = agroin_amaro;

                            row["NombreRegla"] = "0.25% ADICIONAL SOBRE VENTAS PROD. AGROIN";
                            row["Total"] = total_por_regla;
                            tota_comision_vendedor += total_por_regla;
                            totales.Rows.Add(row);
                        }
                    }
                }

                row = totales.NewRow();
                row["NombreRegla"] = "::::TOTAL:::: ";
                row["Total"] = Math.Round(tota_comision_vendedor);
                totales.Rows.Add(row);
                string tabla = "";

                if (codvendedor_aux == "AN044")
                {
                    string neto_navarro = ReporteRNegocio.neto_navarro(periodo);
                    row = totales.NewRow();
                    row["NombreRegla"] = "_:NETO:_";
                    row["Total"] = neto_navarro;
                    totales.Rows.Add(row);
                }

                //---------------------------------------------------------------------------------ACA RELLENA VENDEDORES  GRANOS Y ABARROTES

                string color_th = "";
                string visible_th_si = "style='visibility: block; position: absolute;'";
                string visible_th_no = "style='visibility: hidden; position: absolute;'";

                string visible_th = "";

                if (que_puede_ver == "Abarrotes")
                {

                }
                else if (que_puede_ver == "Granos")
                {

                }

                tabla += "<table class=\"table fill-head table-bordered\">";
                if (c_vendedor_r[2].ToString() == "1")
                {
                    tabla += "<thead style='background-color: #e49191 !important; color: white !important; '>";
                    color_th = "background-color: #e49191 !important; color: white !important;";
                }
                else
                {
                    tabla += "<thead class=\"test\">";
                    color_th = "";
                }

                tabla += "<tr>";
                tabla += "<th style='width: 33%;" + color_th + "'>  NOMBRE  </th>";
                tabla += "<th style='width: 50%;" + color_th + "'>Nombre de Regla</th>";
                tabla += "<th style='width: 17%;" + color_th + "'>Total</th>";
                tabla += "</tr>";
                tabla += "</thead>";
                tabla += "<tbody>";

                if (codvendedor_aux == "JAIME BARGETTO")
                {
                    row2 = dt_total_resumen_com.NewRow();

                    tabla += "<tr>";
                    tabla += "<td>" + c_vendedor_r[1].ToString() + "</td>";
                    tabla += "<td> COMISION SOBRE COMISIONES ANDRES NAVARRO</td>";
                    tabla += "<td>" + Base.monto_format2(tota_comision_vendedor * 0.3) + "</td>";
                    tabla += "</tr>";


                    row2["codvendedor"] = c_vendedor_r[0].ToString().Trim();
                    row2["nombre_vendedor"] = c_vendedor_r[1].ToString().Trim();
                    row2["nombreregla"] = "COMISION SOBRE COMISIONES ANDRES NAVARRO";
                    row2["total"] = Base.monto_format2(tota_comision_vendedor * 0.3);
                    row2["es_abarrotes"] = es_abarrote;
                    dt_total_resumen_com.Rows.Add(row2);
                }
                else
                {
                    foreach (DataRow r2 in totales.Rows)
                    {
                        row2 = dt_total_resumen_com.NewRow();
                        tabla += "<tr>";
                        tabla += "<td>" + c_vendedor_r[1].ToString() + "</td>";
                        tabla += "<td>" + r2["NombreRegla"].ToString() + "</td>";
                        tabla += "<td>" + Base.monto_format2(Convert.ToDouble(r2["Total"].ToString())) + "</td>";
                        tabla += "</tr>";

                        row2["codvendedor"] = c_vendedor_r[0].ToString().Trim();
                        row2["nombre_vendedor"] = c_vendedor_r[1].ToString().Trim();
                        row2["nombreregla"] = r2["NombreRegla"].ToString();
                        row2["total"] = Base.monto_format2(Convert.ToDouble(r2["Total"].ToString()));
                        row2["es_abarrotes"] = es_abarrote;
                        dt_total_resumen_com.Rows.Add(row2);

                        if (codvendedor_aux == "AN044")
                        {
                            string neto_navarro = ReporteRNegocio.neto_navarro(periodo);
                            tabla += "<tr style='background-color: lavender; '>";
                            tabla += "<td> NETO: </td>";
                            tabla += "<td>" + Base.monto_format(neto_navarro) + "</td>";
                            tabla += "</tr>";
                        }


                        // aca estoy editando
                    }
                }

                tabla += "</tbody>";
                tabla += "</table>";
                tabla += "</br> ";
                tabla_final += tabla;

            }


            ////aca temporal se agrega AZOCAR Y CATALDO
            if (que_puede_ver != "Granos")
            {
                //Session["puede_ver"]

                DataTable dt_oficina_abarro = (DataTable)Session["DT_ABARROTES_OFICINA"];

                int sum1 = 0;
                int sum2 = 0;
                try
                {
                    sum1 = Convert.ToInt32(dt_oficina_abarro.Rows[0][0].ToString().Replace(",000000", ""));
                    sum2 = Convert.ToInt32(dt_oficina_abarro.Rows[0][1].ToString().Replace(",000000", ""));
                }
                catch
                {

                }

                string total_abarrote_con_arica = "";
                string desarrollo_mercado_ro = "";
                string total_abarrote_sin_Arica = "";
                try
                {
                    total_abarrote_con_arica = Base.monto_format(dt_oficina_abarro.Rows[0][0].ToString().Replace(",000000", ""));
                    desarrollo_mercado_ro = Base.monto_format(dt_oficina_abarro.Rows[0][1].ToString().Replace(",000000", ""));
                    total_abarrote_sin_Arica = Base.monto_format(dt_oficina_abarro.Rows[0][2].ToString().Replace(",000000", ""));
                }
                catch
                {


                }

                row2 = dt_total_resumen_com.NewRow();
                row2["codvendedor"] = "ABARROTES";
                row2["nombre_vendedor"] = "MARCO ANTONIO AZOCAR";
                row2["nombreregla"] = "TOTAL ABARROTES (C/ARICA)";
                row2["total"] = total_abarrote_con_arica;
                row2["es_abarrotes"] = true;
                dt_total_resumen_com.Rows.Add(row2);

                row2 = dt_total_resumen_com.NewRow();
                row2["codvendedor"] = "ABARROTES";
                row2["nombre_vendedor"] = "MARCO ANTONIO AZOCAR";
                row2["nombreregla"] = "DESARROLLO DE MERCADO (RO)";
                row2["total"] = desarrollo_mercado_ro;
                row2["es_abarrotes"] = true;
                dt_total_resumen_com.Rows.Add(row2);

                row2 = dt_total_resumen_com.NewRow();
                row2["codvendedor"] = "ABARROTES";
                row2["nombre_vendedor"] = "MARCO ANTONIO AZOCAR";
                row2["nombreregla"] = "::::TOTAL::::";
                row2["total"] = Base.monto_format2(sum1 + sum2);
                row2["es_abarrotes"] = true;
                dt_total_resumen_com.Rows.Add(row2);

                row2 = dt_total_resumen_com.NewRow();
                row2["codvendedor"] = "ABARROTES";
                row2["nombre_vendedor"] = "PATRICIA CATALDO";
                row2["nombreregla"] = "TOTAL ABARROTES (S/ARICA)";
                row2["total"] = total_abarrote_sin_Arica;
                row2["es_abarrotes"] = true;
                dt_total_resumen_com.Rows.Add(row2);

                row2 = dt_total_resumen_com.NewRow();
                row2["codvendedor"] = "ABARROTES";
                row2["nombre_vendedor"] = "PATRICIA CATALDO";
                row2["nombreregla"] = "::::TOTAL::::";
                row2["total"] = total_abarrote_sin_Arica;
                row2["es_abarrotes"] = true;
                dt_total_resumen_com.Rows.Add(row2);

                string tabla_azocar_y_cataldo = "";
                string color_th1 = "background-color: #e49191 !important; color: white !important;";

                tabla_azocar_y_cataldo += "<table class=\"table fill-head table-bordered\">";
                tabla_azocar_y_cataldo += "<thead class=\"test\">";
                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<th style='width: 33%;" + color_th1 + "'>  NOMBRE  </th>";
                tabla_azocar_y_cataldo += "<th style='width: 50%;" + color_th1 + "'>Nombre de Regla</th>";
                tabla_azocar_y_cataldo += "<th style='width: 17%;" + color_th1 + "'>Total</th>";
                tabla_azocar_y_cataldo += "</tr>";
                tabla_azocar_y_cataldo += "</thead>";
                tabla_azocar_y_cataldo += "<tbody>";

                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "MARCO ANTONIO AZOCAR";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "TOTAL ABARROTES (C/ARICA)";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += total_abarrote_con_arica;
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "</tr>";

                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "MARCO ANTONIO AZOCAR";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "DESARROLLO DE MERCADO (RO)";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += desarrollo_mercado_ro;
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "</tr>";

                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";

                tabla_azocar_y_cataldo += "::::TOTAL::::";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += Base.monto_format2(sum1 + sum2);
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "</tr>";

                tabla_azocar_y_cataldo += "</tbody>";
                tabla_azocar_y_cataldo += "</table>";
                tabla_azocar_y_cataldo += "</br> ";

                ///// cataldo

                tabla_azocar_y_cataldo += "<table class=\"table fill-head table-bordered\">";
                tabla_azocar_y_cataldo += "<thead >";
                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<th style='width: 33%;" + color_th1 + "'>  NOMBRE  </th>";
                tabla_azocar_y_cataldo += "<th style='width: 50%;" + color_th1 + "'>Nombre de Regla</th>";
                tabla_azocar_y_cataldo += "<th style='width: 17%;" + color_th1 + "'>Total</th>";
                tabla_azocar_y_cataldo += "</tr>";
                tabla_azocar_y_cataldo += "</thead>";
                tabla_azocar_y_cataldo += "<tbody>";

                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "PATRICIA CATALDO";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "TOTAL ABARROTES S/ARICA";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += total_abarrote_sin_Arica;
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "</tr>";

                tabla_azocar_y_cataldo += "<tr>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += "::::TOTAL::::";
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "<td>";
                tabla_azocar_y_cataldo += total_abarrote_sin_Arica;
                tabla_azocar_y_cataldo += "</td>";
                tabla_azocar_y_cataldo += "</tr>";

                tabla_azocar_y_cataldo += "</tbody>";
                tabla_azocar_y_cataldo += "</table>";
                tabla_azocar_y_cataldo += "</br> ";

                tabla_final += tabla_azocar_y_cataldo;

            }




            Session["dt_total_resumen"] = dt_total_resumen_com;
            Session["periodo"] = txt_periodo.Text;

            string tabla_cobranza = "";
            double total_neto = 0;
            ////---------------------------------------------------------------------------------------RESUMEN COMISIONES DE COBRANZA

            if (que_puede_ver == "Todo")
            {
                dt_total_cobranza = new DataTable();
                //dt_total_resumen_com.NewRow();
                dt_total_cobranza.Columns.Add("nombre_cobran");
                dt_total_cobranza.Columns.Add("neto");
                dt_total_cobranza.Columns.Add("porct");
                dt_total_cobranza.Columns.Add("total");

                DataRow row3;

                //ROSA SOLIS .....................------------------------------------------------------------------------------------------

                //pdf
                row3 = dt_total_cobranza.NewRow();
                row3["nombre_cobran"] = "ROSA SOLIS V.";
                row3["neto"] = "NETO";
                row3["porct"] = "PORCT";
                row3["total"] = "TOTAL";
                dt_total_cobranza.Rows.Add(row3);


                string color_cobranza = "background-color: #4d9850 !important; color: white !important; ";
                //web
                tabla_cobranza += "<table class=\"table fill-head table-bordered\">";
                tabla_cobranza += "<thead>";
                tabla_cobranza += "<tr>";
                tabla_cobranza += "<th style='width: 40%;" + color_cobranza + "'>  ROSA SOLIS V. </th>";
                tabla_cobranza += "<th style='width: 28%;" + color_cobranza + "'>NETO </th>";
                tabla_cobranza += "<th style='width: 15%;" + color_cobranza + "'>PORCT </th>";
                tabla_cobranza += "<th style='width: 17%;" + color_cobranza + "'>TOTAL</th>";
                tabla_cobranza += "</tr>";
                tabla_cobranza += "</thead>";
                tabla_cobranza += "<tbody>";

                double total_comision_cobranza = 0;
                foreach (DataRow r4 in comision_cobranza.Rows)
                {

                    row3 = dt_total_cobranza.NewRow();

                    string nombre_Categoria = r4[0].ToString();

                    double neto_categoria = Convert.ToDouble(r4[1].ToString());
                    total_neto += neto_categoria;

                    string porcentaje = r4[2].ToString() + " %";

                    double comision_categoria = Convert.ToDouble(r4[5].ToString());
                    total_comision_cobranza += comision_categoria;

                    //pdf
                    row3 = dt_total_cobranza.NewRow();
                    row3["nombre_cobran"] = nombre_Categoria;
                    row3["neto"] = Base.monto_format2(neto_categoria);
                    row3["porct"] = porcentaje;
                    row3["total"] = Base.monto_format2(comision_categoria);
                    dt_total_cobranza.Rows.Add(row3);

                    //web
                    tabla_cobranza += "<tr>";
                    tabla_cobranza += "<td>" + nombre_Categoria + "</td>";
                    tabla_cobranza += "<td>" + Base.monto_format2(neto_categoria) + "</td>";
                    if (Convert.ToDouble(r4[2].ToString()) == 0)
                    {
                        tabla_cobranza += "<td></td>";
                    }
                    else
                    {
                        tabla_cobranza += "<td>" + porcentaje + "</td>";
                    }
                    tabla_cobranza += "<td>" + Base.monto_format2(comision_categoria) + "</td>";
                    tabla_cobranza += "</tr>";
                }
                //pdf
                row3 = dt_total_cobranza.NewRow();
                row3["nombre_cobran"] = ":::::TOTAL::::::";
                row3["neto"] = Base.monto_format2(total_neto);
                row3["porct"] = "";
                row3["total"] = Base.monto_format2(total_comision_cobranza);
                dt_total_cobranza.Rows.Add(row3);

                //web
                tabla_cobranza += "<tr>";
                tabla_cobranza += "<td>:::::TOTAL::::::</td>";
                tabla_cobranza += "<td>" + Base.monto_format2(total_neto) + "</td>";
                tabla_cobranza += "<td></td>";
                tabla_cobranza += "<td>" + Base.monto_format2(total_comision_cobranza) + "</td>";
                tabla_cobranza += "</tr>";

                tabla_cobranza += "</tbody>";
                tabla_cobranza += "</table>";
                tabla_cobranza += "</br> ";


                //EVELYN LEIVA .....................------------------------------------------------------------------------------------------

                //pdf
                row3 = dt_total_cobranza.NewRow();
                row3["nombre_cobran"] = "EVELYN LEIVA";
                row3["neto"] = "NETO";
                row3["porct"] = "PORCT";
                row3["total"] = "TOTAL";
                dt_total_cobranza.Rows.Add(row3);

                //web
                tabla_cobranza += "<table class=\"table fill-head table-bordered\">";
                tabla_cobranza += "<thead style='background-color: #4d9850 !important; color: white !important; '>";
                tabla_cobranza += "<tr>";
                tabla_cobranza += "<th style='width: 40%;" + color_cobranza + "'>EVELYN LEIVA </th>";
                tabla_cobranza += "<th style='width: 28%;" + color_cobranza + "'>NETO </th>";
                tabla_cobranza += "<th style='width: 15%;" + color_cobranza + "'>PORCT </th>";
                tabla_cobranza += "<th style='width: 17%;" + color_cobranza + "'>TOTAL</th>";
                tabla_cobranza += "</tr>";
                tabla_cobranza += "</thead>";
                tabla_cobranza += "<tbody>";

                total_comision_cobranza = 0;
                foreach (DataRow r4 in comision_cobranza.Rows)
                {

                    row3 = dt_total_cobranza.NewRow();

                    string nombre_Categoria = r4[0].ToString();

                    double neto_categoria = Convert.ToDouble(r4[1].ToString());

                    string porcentaje = r4[3].ToString() + " %";

                    double comision_categoria = Convert.ToDouble(r4[6].ToString());
                    total_comision_cobranza += comision_categoria;

                    //pdf
                    row3 = dt_total_cobranza.NewRow();
                    row3["nombre_cobran"] = nombre_Categoria;
                    row3["neto"] = Base.monto_format2(neto_categoria);
                    row3["porct"] = porcentaje;
                    row3["total"] = Base.monto_format2(comision_categoria);
                    dt_total_cobranza.Rows.Add(row3);

                    //web
                    tabla_cobranza += "<tr>";
                    tabla_cobranza += "<td>" + nombre_Categoria + "</td>";
                    tabla_cobranza += "<td>" + Base.monto_format2(neto_categoria) + "</td>";

                    if (Convert.ToDouble(r4[3].ToString()) == 0)
                    {
                        tabla_cobranza += "<td></td>";
                    }
                    else
                    {
                        tabla_cobranza += "<td>" + porcentaje + "</td>";
                    }

                    tabla_cobranza += "<td>" + Base.monto_format2(comision_categoria) + "</td>";
                    tabla_cobranza += "</tr>";
                }
                //pdf
                row3 = dt_total_cobranza.NewRow();
                row3["nombre_cobran"] = ":::::TOTAL::::::";
                row3["neto"] = Base.monto_format2(total_neto);
                row3["porct"] = "";
                row3["total"] = Base.monto_format2(total_comision_cobranza);
                dt_total_cobranza.Rows.Add(row3);

                //web
                tabla_cobranza += "<tr>";
                tabla_cobranza += "<td>:::::TOTAL::::::</td>";
                tabla_cobranza += "<td>" + Base.monto_format2(total_neto) + "</td>";
                tabla_cobranza += "<td></td>";
                tabla_cobranza += "<td>" + Base.monto_format2(total_comision_cobranza) + "</td>";
                tabla_cobranza += "</tr>";


                tabla_cobranza += "</tbody>";
                tabla_cobranza += "</table>";
                tabla_cobranza += "</br> ";


                //MARIANA SILVA .....................------------------------------------------------------------------------------------------
                //pdf
                //row3 = dt_total_cobranza.NewRow();
                //row3["nombre_cobran"] = "MARIANA SILVA";
                //row3["neto"] = "NETO";
                //row3["porct"] = "PORCT";
                //row3["total"] = "TOTAL";
                //dt_total_cobranza.Rows.Add(row3);

                //tabla_cobranza += "<table class=\"table fill-head table-bordered\">";
                //tabla_cobranza += "<thead style='background-color: #4d9850 !important; color: white !important; '>";
                //tabla_cobranza += "<tr>";
                //tabla_cobranza += "<th style='width: 40%;" + color_cobranza + "'>MARIANA SILVA</th>";
                //tabla_cobranza += "<th style='width: 28%;" + color_cobranza + "'>NETO </th>";
                //tabla_cobranza += "<th style='width: 15%;" + color_cobranza + "'>PORCT </th>";
                //tabla_cobranza += "<th style='width: 17%;" + color_cobranza + "'>TOTAL</th>";
                //tabla_cobranza += "</tr>";
                //tabla_cobranza += "</thead>";
                //tabla_cobranza += "<tbody>";

                //total_comision_cobranza = 0;
                //foreach (DataRow r4 in comision_cobranza.Rows)
                //{
                //    row3 = dt_total_cobranza.NewRow();

                //    string nombre_Categoria = r4[0].ToString();

                //    double neto_categoria = Convert.ToDouble(r4[1].ToString());

                //    string porcentaje = r4[4].ToString() + " %";

                //    double comision_categoria = Convert.ToDouble(r4[7].ToString());
                //    total_comision_cobranza += comision_categoria;

                //    //pdf
                //    row3 = dt_total_cobranza.NewRow();
                //    row3["nombre_cobran"] = nombre_Categoria;
                //    row3["neto"] = Base.monto_format2(neto_categoria);
                //    row3["porct"] = porcentaje;
                //    row3["total"] = Base.monto_format2(comision_categoria);
                //    dt_total_cobranza.Rows.Add(row3);

                //    //web
                //    tabla_cobranza += "<tr>";
                //    tabla_cobranza += "<td>" + r4[0].ToString() + "</td>";
                //    tabla_cobranza += "<td>" + Base.monto_format2(neto_categoria) + " </td>";

                //    if (Convert.ToDouble(r4[4].ToString()) == 0)
                //    {
                //        tabla_cobranza += "<td></td>";
                //    }
                //    else
                //    {
                //        tabla_cobranza += "<td>" + porcentaje + "</td>";
                //    }

                //    tabla_cobranza += "<td>" + Base.monto_format2(comision_categoria) + "</td>";
                //    tabla_cobranza += "</tr>";
                //}
                ////pdf
                //row3 = dt_total_cobranza.NewRow();
                //row3["nombre_cobran"] = ":::::TOTAL::::::";
                //row3["neto"] = Base.monto_format2(total_neto);
                //row3["porct"] = "";
                //row3["total"] = Base.monto_format2(total_comision_cobranza);
                //dt_total_cobranza.Rows.Add(row3);

                ////web
                //tabla_cobranza += "<tr>";
                //tabla_cobranza += "<td>:::::TOTAL::::::</td>";
                //tabla_cobranza += "<td>" + Base.monto_format2(total_neto) + "</td>";
                //tabla_cobranza += "<td></td>";
                //tabla_cobranza += "<td>" + Base.monto_format2(total_comision_cobranza) + "</td>";
                //tabla_cobranza += "</tr>";

                //tabla_cobranza += "</tbody>";
                //tabla_cobranza += "</table>";
                //tabla_cobranza += "</br> ";


                Session["dt_cobranza_com_resu"] = dt_total_cobranza;

                //tabla_final += tabla_cobranza;
                //comisiones cobranza

                //div_tabla_totales.InnerHtml = tabla_final;
            }
        }

        public static String formatoMiles(string monto)
        {
            double d;
            double.TryParse(monto, out d);
            string aux = "";
            if (d == 0) { aux = "0"; } else { aux = d.ToString("N0"); }
            return aux;
        }

        private void TheDownload(string path, string file_name)
        {
            try
            {
                path = path.Replace("\\\\", "\\");
                System.IO.FileInfo toDownload = new System.IO.FileInfo(path);
                if (toDownload.Exists)
                {
                    Response.Clear();
                    //if (filename_.Contains(" "))
                    //{

                    //    filename_ = filename_.Replace(" ", "_");
                    //}

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file_name);
                    Response.ContentType = "application/octect-stream";
                    Response.AddHeader("content-transfer-encoding", "binary");
                    Response.WriteFile(path);
                    Response.End();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btn_guardar_agroin_ServerClick(object sender, EventArgs e)
        {

            comisionagroinamaroEntity t = new comisionagroinamaroEntity();
            t.monto_dolar = Convert.ToDouble(txt_dolar.Text.Replace(".", ","));
            t.tipo_cambio = Convert.ToDouble(txt_cambio.Text.Replace(".", ","));
            t.cod_periodo = txt_periodo.Text;

            string ok = "";
            //comisionagroinamaroBO.encontrar(ref t);

            ok = comisionagroinamaroBO.agregar(t);

            if (ok == "1" || ok == "2")
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "qe", "<script language='javascript'>alert('OK');</script>", false);


            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "qe", "<script language='javascript'>alert('Error');</script>", false);

            }


            //firmas(p);

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "t3mp", "<script language='javascript'>firmas_j('" + txt_periodo.Text + "', '" + User.Identity.Name + "');</script>", false);

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd121m123k3mp", "<script language='javascript'>tabla_refresh();</script>", false);

            //comisionagroinamaroBO.encontrar(ref t);
        }

        protected void G_NOMINA_eXCEL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double porncetaje_actual = 0;
                double neto = 0;
                double comision_final = 0;

                try
                {
                    comision_final = Convert.ToDouble(e.Row.Cells[10].Text);
                    porncetaje_actual = Convert.ToDouble(e.Row.Cells[7].Text);
                    neto = Convert.ToDouble(e.Row.Cells[6].Text);
                }
                catch
                {

                }

                comisionperiodocierre_productosEntity t = new comisionperiodocierre_productosEntity();
                t.cod_periodo = e.Row.Cells[12].Text.Trim();
                t.vendedor = e.Row.Cells[4].Text.Trim();
                t.númfactura = e.Row.Cells[0].Text.Trim();
                t.producto = e.Row.Cells[1].Text.Trim();
                t.cod_regla = e.Row.Cells[8].Text.Trim();
                t.porcentaje = porncetaje_actual;

                string descr = e.Row.Cells[17].Text.Trim();

                string agregar2 = e.Row.Cells[12].Text;
                string cod_periodo2 = "";
                if (agregar2 == "&nbsp;")
                {

                    e.Row.Cells[14].Text = "no";

                    e.Row.Cells[18].Text = "";

                }
                else
                {

                    cod_periodo2 = agregar2;

                    e.Row.Cells[14].Text = "si";

                    string porcentaje_editado = e.Row.Cells[18].Text;

                    if (porcentaje_editado != "&nbsp;")
                    {
                        try
                        {
                            double porcentaje_editado_ = Convert.ToDouble(porcentaje_editado);
                            double comision_editada = Math.Round((neto * porcentaje_editado_) / 100);
                            comision_final = comision_editada;
                            e.Row.Cells[10].Text = "<p style='color:red'>" + comision_editada.ToString() + " </p>";

                        }
                        catch
                        {

                        }

                        e.Row.Cells[18].Text = porcentaje_editado.Trim();


                    }
                    else
                    {
                        e.Row.Cells[18].Text = "";


                    }


                }

                try
                {


                    double comision = comision_final;
                    total_comision2 += comision;



                }
                catch (Exception ex)
                {
                    string aca = "";
                }


                if (e.Row.Cells[15].Text == "CM")
                {
                    e.Row.Cells[15].Text = e.Row.Cells[15].Text.Trim() + " " + descr.Substring(descr.IndexOf("F:")).Trim();
                }

                double porcdescu = Convert.ToDouble(e.Row.Cells[16].Text);
                e.Row.Cells[16].Text = porcdescu.ToString("N2");



                e.Row.Cells[17].Visible = false;
                G_Nomina.HeaderRow.Cells[17].Visible = false;

            }
        }

        protected void G_NOMINA_SN_COMI_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void G_NOMINA_SN_COMI_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btn_sn_comis_Click(object sender, EventArgs e)
        {
            completar_no_comisiones(txt_periodo.Text, "", "");


        }

        protected void btn_excel_sn__Click(object sender, EventArgs e)
        {
            Response.Clear();

            llenar_excel_sin_comi();
            G_NOMINA_SN_COMI_excel.Visible = true;

            Response.AddHeader("content-disposition", "attachment;filename=SIN_COMISION" + DateTime.Now.ToShortDateString() + ".xls");

            Response.Charset = "";

            // If you want the option to open the Excel file without saving than

            // comment out the line below

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.ContentType = "application/vnd.xls";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);

            G_NOMINA_SN_COMI_excel.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());


            Response.End();
            G_NOMINA_SN_COMI_excel.Visible = false;
        }

        private void llenar_excel_sin_comi()
        {

            cont_comisiones = 0;
            G_NOMINA_SN_COMI_excel.DataSource = (DataTable)Session["comisiones_sin_com"];
            G_NOMINA_SN_COMI_excel.DataBind();
        }

        protected void G_NOMINA_SN_COMI_excel_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void G_NOMINA_SN_COMI_excel_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btn_filtra_ven_ServerClick(object sender, EventArgs e)
        {
            string where_vendedor_1 = " and a.codvendedor = '" + c_vendedor_2.SelectedValue.ToString() + "'";

            string where_vendedor_2 = " and b.codvendedor = '" + c_vendedor_2.SelectedValue.ToString() + "'";

            completar_no_comisiones(txt_periodo.Text, where_vendedor_1, where_vendedor_2);


        }

        protected void btn_guardar_abarr_Click(object sender, EventArgs e)
        {

            comisionabarrotesoficinaEntity t = new comisionabarrotesoficinaEntity();

            t.monto_total_abarr = Convert.ToInt32(t_total_abarrotes_c_arica.Text.Replace(".", "").Trim());
            t.monto_ro = Convert.ToInt32(t_ro_.Text.Replace(".", "").Trim());

            t.monto_arica = Convert.ToInt32(t_total_abarrotes_s_arica.Text.Replace(".", "").Trim());

            t.cod_periodo = txt_periodo.Text;

            t.porcentaje_Azo_1 = 0.10;
            t.porcentaje_Azo_2 = 0.4;
            t.porcentaje_Pcat = 0.10;


            string ok = "";
            //comisionagroinamaroBO.encontrar(ref t);

            ok = comisionabarrotesoficinaBO.agregar(t);

            if (ok == "1" || ok == "2")
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "qe", "<script language='javascript'>alert('OK');</script>", false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "qe", "<script language='javascript'>alert('Error');</script>", false);
            }


            //firmas(p);

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "t3mp", "<script language='javascript'>firmas_j('" + txt_periodo.Text + "', '" + User.Identity.Name + "');</script>", false);

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd121m123k3mp", "<script language='javascript'>tabla_refresh();</script>", false);
        }

        protected void btn_avisa_firmas_Click(object sender, EventArgs e)
        {
            MailMessage email = new MailMessage();
            //email.To.Add(new MailAddress("comisiones@soprodi.cl"));
            email.To.Add(new MailAddress("informatica@soprodi.cl"));
            email.From = new MailAddress("informes@soprodi.cl");
            string periodo = txt_periodo.Text;

            email.Subject = "FIRMAS LISTAS DE COMISIONES (" + periodo + " ) ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

            string cc = ReporteRNegocio.trae_correo(7);

            //string correo_vendedor_por_cliente = ReporteRNegocio.trae_corr_vend_por_cliente(rutcliente.Replace("-", "").Replace(".", ""));
            //if (correo_vendedor_por_cliente != "")
            //{
            email.CC.Add(cc);
            //}
            email.Body += "<div style='text-align:center;     display: block !important;' > ";
            email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "</div>";
            email.Body += "<div><img src='http://a58.imgup.net/Sopro4d9d.png' style='    float: right;     width: 90px;'> </div><br><br><br><br><br>";

            email.Body += "<div> Estimados :<br> <br> El periodo ya tiene las firmas necesarias para terminar el proceso, favor autorizar para su pago :  </b> <br><br>";

            string HTML = "";
            HTML += "<table class=\"table fill-head table-bordered\" style=\"width:100%;\">";
            HTML += "<thead class=\"test\" style=\"background-color:rgb(156,205,249)\">";
            HTML += "<tr>";

            HTML += "<th>GRUPO</th>";
            HTML += "<th>NOMBRE</th>";

            HTML += "</tr>";
            HTML += "</thead>";
            HTML += "<tbody>";

            DataTable dt = ReporteRNegocio.firmas_listas(periodo);
            foreach (DataRow r2 in dt.Rows)
            {
                if (r2[0].ToString().Trim() != "FINANZAS")
                {
                    HTML += "<tr>";
                    HTML += "<td colspan=1; class='test'>" + r2[0].ToString().Trim() + "</td>";
                    HTML += "<td colspan=1; class='test'>" + r2[1].ToString().Trim() + "</td>";
                    HTML += "</tr>";
                }
            }
            HTML += "</tbody>";
            HTML += "</table><br>";

            email.Body += HTML;
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
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "13", "<script language='javascript'>cierre_firmas();alert('Correo Enviado!');</script>", false);
            }
            catch (Exception ex)
            {

            }

            /////desde gmail

            //email.IsBodyHtml = true;
            //email.Priority = MailPriority.Normal;
            //email.BodyEncoding = System.Text.Encoding.UTF8;
            //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            //smtp.EnableSsl = true;
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.UseDefaultCredentials = false;

            //////smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            //smtp.Credentials = new NetworkCredential("informes.soprodi@gmail.com", "galaxia1234");
            //try
            //{
            //    smtp.Send(email);
            //    email.Dispose();
            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "13", "<script language='javascript'>cierre_firmas();alert('Correo Enviado!');</script>", false);
            //}
            //catch (Exception ex)
            //{

            //}

        }
    }
}