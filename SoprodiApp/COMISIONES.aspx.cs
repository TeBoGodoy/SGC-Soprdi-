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
//using FTP_Soprodi.BusinessLayer;

namespace SoprodiApp
{
    public partial class COMISIONES : System.Web.UI.Page
    {

        public static string grupos;
        private static string grupos2;
        private static string USER;

        protected void Page_Load(object sender, EventArgs e)
        {

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();

            if (!IsPostBack)
            {
                //List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                //bool correcto_app = false;
                //foreach (string u_ne in u_negocio)
                //{
                //    if (u_ne.Trim() == "6" || u_ne.Trim() == "12")
                //    {
                //        correcto_app = true;
                //    }
                //}
                //if (!correcto_app)
                //{
                //    Response.Redirect("MENU.aspx");
                //}

                DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now;
                ////t = new DateTime(t.Year, t.Month - 6, 1);
                txt_desde.Text = ReporteRNegocio.CALCULA_DESDE(t.Month, t.Year);
                txt_hasta.Text = t2.ToShortDateString();

                if (Session["SW_PERMI"].ToString() == "1")
                {

                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=1'>Abarrotes</a>";
                    //titulo2.HRef = "reportes.aspx?s=1";
                    //titulo2.InnerText = "Abarrotes";
                }
                else if (Session["SW_PERMI"].ToString() == "2")
                {
                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=2'>Granos</a>";
                    //titulo2.HRef = "reportes.aspx?s=2";
                    //titulo2.InnerText = "Granos";
                }

                USER = User.Identity.Name.ToString();

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
            d_grupos_usuario.DataSource = dtv;
            d_grupos_usuario.DataTextField = "user1";
            d_grupos_usuario.DataValueField = "user1";
            //d_vendedor_.SelectedIndex = -1;
            d_grupos_usuario.DataBind();
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
            string grupos_del_usuario = "";

            grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

            if (grupos_del_usuario == "")
            {
                grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
            }


            if (txt_desde.Text == "" || txt_hasta.Text == "")
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasdas2dsaeee", "<script> alert('Ingrese fechas'); </script>", false);
                return;

            }

            //MENSUAL
            string html_DIAS = crear_comiones(agregra_comillas(l_grupos.Text));

            div3.Visible = true;
            Div5.Visible = true;


            DivMainContent1.InnerHtml = html_DIAS;
            R_Excel_1.InnerHtml = html_DIAS;


            filtro_memoria_div.InnerHtml = "<div class='btn-toolbar pull-left'><input type='text' id='t_filtro_memoria' style='width: 200px; margin-right: 7px; padding: 5px;' placeholder='Filtrar...' class='form-control' /></div>";

            //volver a cargar

            DataTable dt = new DataTable();

            string wher2 = " where user1 in (" + grupos_del_usuario + ")";


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
                        HTML += "<td colspan=1;> <a data-toggle='tooltip' data-placement='bottom' title='"+ r[0].ToString().Trim() + "' href='javascript:' onclick='" + script2 + "'> "+Base.monto_format2( sum_comision )+" </a> </td>";



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


        public void test(object sender, EventArgs e)
        {
            string aca = "";
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
        protected void LinkButton1_Click(object sender, EventArgs e)
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

            R_Excel_1.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());

            Response.End();
        }
    }
}