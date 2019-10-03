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
using SoprodiApp.entidad;
using SoprodiApp.Entities;
using SoprodiApp.BusinessLayer;
using System.Net.Mail;
using System.Net;

namespace SoprodiApp
{
    public partial class REPORTE_VEND_CLIE : System.Web.UI.Page
    {
        public static int cont_periodos;
        public static DataTable aux;
        public static DataTable productos;
        public static DataTable totales;
        public static string PERIODOS;
        private static string USER;
        private static string where = " where 1=1 ";
        private static Page page;

        public static bool header_sum = true;
        public static bool header_sum2 = true;
        private static string vendedor;
        private static string cliente;
        public static int cont_det;
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
                    Session["codvendedor"] = USER.Trim();
                    Session["es_vendedor"] = true;
                    btn_excel.Visible = false;
                    btn_excel2.Visible = false;
                }
                else
                {
                    Session["es_vendedor"] = false;
                    btn_excel.Visible = true;
                    btn_excel2.Visible = true;
                }

                Session["correo_usuario"] = ReporteRNegocio.corr_usuario(USER).Rows[0][1].ToString() + ", rmc@soprodi.cl, mazocar@soprodi.cl";
                txt_cc.Text = ReporteRNegocio.corr_usuario(USER).Rows[0][1].ToString();

                ImageClickEventArgs ex = new ImageClickEventArgs(1, 2);
                b_Click(sender, ex);
            }
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teadsaeee", "<script> SortPrah(); </script>", false);

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



        [WebMethod]
        public static string ELIMINAR_AGENDA(string rutcliente_, string codvendedor_, string fecha_agenda_, string nombre_contacto_)
        {

            agendacontactoEntity ag = new agendacontactoEntity();
            ag.rutcliente = rutcliente_;
            ag.nombre_contacto = nombre_contacto_;
            ag.fecha_agenda = Convert.ToDateTime(fecha_agenda_);
            ag.codvendedor = codvendedor_;
            string ok = agendacontactoBO.eliminar(ag);

            if (ok == "OK")
            {
                return ok;
            }
            else
            {
                return ok;
            }
        }

        public class EDITAR_CALENDARIO
        {
            public string dia { get; set; }
            public string tiempo { get; set; }
            public string obs { get; set; }

        }

        [WebMethod]
        public static EDITAR_CALENDARIO EDITAR_AGENDA(string rutcliente_, string codvendedor_, string fecha_agenda_, string nombre_contacto_ /*string t_dia_edit2, string t_obs_edit2, string t_time_edit2*/)
        {
            string rutcliente = rutcliente_;
            string fecha_Agenda = fecha_agenda_;
            string codvendedor = codvendedor_;
            string nom_contacto = nombre_contacto_;



            HttpContext.Current.Session["rutcliente_"] = rutcliente;
            HttpContext.Current.Session["fecha_agenda"] = fecha_Agenda;
            HttpContext.Current.Session["codvendedor"] = codvendedor;
            HttpContext.Current.Session["nombre_contacto"] = nom_contacto;



            agendacontactoEntity ag = new agendacontactoEntity();
            ag.rutcliente = rutcliente;
            ag.nombre_contacto = nom_contacto;
            ag.fecha_agenda = Convert.ToDateTime(fecha_Agenda);
            ag.codvendedor = codvendedor;
            agendacontactoBO.encontrar(ref ag);
            //t_dia_edit.Text = ag.fecha_agenda.ToString("yyyy-MM-dd");
            //t_time_edit.Text = ag.am_pm;
            //t_obs_edit.InnerText = ag.observacion;



            EDITAR_CALENDARIO lst = new EDITAR_CALENDARIO();
            lst.obs = ag.observacion;
            lst.dia = ag.fecha_agenda.ToString("yyyy-MM-dd");
            lst.tiempo = ag.am_pm;

            return lst;
            //agendacontactoEntity ag = new agendacontactoEntity();
            //ag.rutcliente = rutcliente_;
            //ag.nombre_contacto = nombre_contacto_;
            //ag.codvendedor = codvendedor_;

            //DateTime fecha_antigua =  Convert.ToDateTime( fecha_agenda_) ;

            //ag.fecha_agenda = Convert.ToDateTime(t_dia_edit2);
            //ag.observacion = t_obs_edit2;
            //ag.am_pm = t_time_edit2;

            //string ok = agendacontactoBO.actualizar(ag, fecha_antigua);

            //if (ok == "OK")
            //{
            //    return ok;
            //}
            //else
            //{
            //    return ok;
            //}
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

            string vendedores = agregra_comillas(l_vendedores.Text);
            string clientes = agregra_comillas(l_clientes.Text);

            string es_vendedor = ReporteRNegocio.esvendedor(USER);
            string where = " where 1=1";
            string where_sin_venta = " where 1=1";

            if (clientes != "")
            {
                where = where + " and a.rutcliente in (" + clientes + ") ";
            }
            if (vendedores != "")
            {
                try
                {
                    Session["codvendedor"] = vendedores;
                }
                catch
                {


                }
                where = where + " and a.codvendedor in (" + vendedores + ") ";
                where_sin_venta = where_sin_venta + " and SLSPERID in (" + vendedores + ") ";
            }

            if (es_vendedor == "2")
            {
                where_sin_venta += " and SLSPERID in ('" + USER + "')";
                where += " and a.codvendedor in ('" + USER + "')";
            }

            Session["codvendedor"] = l_vendedores.Text;


            string grupos_del_usuario = "";

            grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

            if (grupos_del_usuario == "")
            {
                grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
            }

            where += " and a.user1 in (" + grupos_del_usuario + ") ";

            Session["to_linea_credi"] = 0;
            Session["to_disponi_linea_credi"] = 0;

            Session["l_vendedores.Text"] = l_vendedores.Text;

            div_report.Visible = true;
            G_INFORME_TOTAL_VENDEDOR.Visible = true;
            DataTable dt1 = ReporteRNegocio.trae_cliente_vendedor(where);
            Session["dt_clientes_vendedor"] = dt1;
            G_INFORME_TOTAL_VENDEDOR.DataSource = dt1;
            G_INFORME_TOTAL_VENDEDOR.DataBind();



            //G_SIN_VENTAS.DataSource = ReporteRNegocio.trae_cliente_vendedor_SINVENTA(where_sin_venta);
            //G_SIN_VENTAS.DataBind();


            //TABLA TOTALES LINEA Y DISPONIBLE (CREDITO)

            long total_linea = Convert.ToInt64(Session["to_linea_credi"]);
            long total_disponible = Convert.ToInt64(Session["to_disponi_linea_credi"]);

            string tabla = "";
            tabla += "<table class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";
            tabla += "<th>Total linea credito</th>";
            tabla += "<th>Total disponible</th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";

            tabla += "<tr>";
            tabla += "<td>" + total_linea.ToString("N0") + "</td>";
            tabla += "<td>" + total_disponible.ToString("N0") + "</td>";
            tabla += "</tr>";

            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");

            div_totales.InnerHtml = tabla;

            DataTable dt = new DataTable();
            string where2 = " where 1=1";
            where2 += " and user1 in (" + grupos_del_usuario + ") ";

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
            where2 = " where 1=1";

            if (clientes != "")
            {

                where2 = where2 + " and rutcliente in (" + clientes + ") ";
            }
            if (vendedores != "")
            {
                where2 = where2 + " and codvendedor in (" + vendedores + ") ";
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

            JQ_Datatable();
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_INFORME_TOTAL_VENDEDOR')); </script>", false);


        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            MakeAccessible(G_INFORME_TOTAL_VENDEDOR);
            MakeAccessible(G_SIN_VENTAS);
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

        protected void G_INFORME_TOTAL_VENDEDOR_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                string rutcliente = G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values[0].ToString().Trim();
                string cod_vend = G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values[5].ToString().Trim();

                //DateTime fecha_ultima_fact = Convert.ToDateTime(G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values[8].ToString().Trim());

                //string meses_atras_3 = primer_dia(fecha_ultima_fact.AddMonths(-3));

                //int promedio_3_meses_atras = ReporteRNegocio.prom_3_meses_atras(meses_atras_3, rutcliente, cod_vend) / 3;

                int promedio_3_meses_atras = Convert.ToInt32(G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values[10].ToString().Trim());
                int monto_ultima_fact = Convert.ToInt32(G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values[9].ToString().Trim());

                string ultimo_cotacto = G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values[6].ToString().Trim();
                int meses_diferencia = Convert.ToInt32(G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values[1].ToString().Trim());

                switch (meses_diferencia)
                {
                    case 0:
                        e.Row.Attributes["class"] = "estado_0_1";
                        break;
                    case 1:
                        e.Row.Attributes["class"] = "estado_0_2";
                        break;
                    case 2:
                        e.Row.Attributes["class"] = "estado_0_3";
                        break;
                    case 3:
                        e.Row.Attributes["class"] = "estado_0_4";
                        break;
                    default:
                        e.Row.Attributes["class"] = "estado_0_max";
                        break;
                }



                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                //mantenedor contactos
                string script1 = string.Format("javascript:fuera22(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(rutcliente), encriptador.EncryptData("88"));
                e.Row.Cells[1].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[1].Text + " </a>";

                G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[3].Attributes["data-sort-method"] = "number";
                G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[5].Attributes["data-sort-method"] = "number";
                G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[6].Attributes["data-sort-method"] = "number";


                //PROMEDIO FACTURACION 12 MESES
                double d;
                double.TryParse(e.Row.Cells[3].Text, out d);
                string aux = "";
                if (d == 0) { aux = e.Row.Cells[3].Text; } else { aux = d.ToString("N0"); }
                e.Row.Cells[3].Text = aux;

                //MONTO ULTIMA FACTURACION
                double.TryParse(e.Row.Cells[5].Text, out d);
                if (d == 0) { aux = e.Row.Cells[5].Text; } else { aux = d.ToString("N0"); }
                e.Row.Cells[5].Text = aux;

                //MESES DESDE ULTIMA
                double.TryParse(e.Row.Cells[6].Text, out d);
                if (d == 0) { aux = e.Row.Cells[6].Text; } else { aux = d.ToString("N0"); }
                e.Row.Cells[6].Text = aux;

                //DIAS DESDE ULTIMA
                double.TryParse(e.Row.Cells[6].Text, out d);
                if (d == 0) { aux = e.Row.Cells[6].Text; } else { aux = d.ToString("N0"); }
                e.Row.Cells[6].Text = aux;

                //LINEA CREDITO
                double.TryParse(e.Row.Cells[10].Text, out d);
                if (d == 0) { aux = e.Row.Cells[10].Text; } else { aux = d.ToString("N0"); }
                e.Row.Cells[10].Text = aux;

                Session["to_linea_credi"] = Convert.ToInt64(Session["to_linea_credi"]) + d;

                //DISPONIBL
                double.TryParse(e.Row.Cells[11].Text, out d);
                if (d == 0) { aux = e.Row.Cells[11].Text; } else { aux = d.ToString("N0"); }
                e.Row.Cells[11].Text = aux;

                Session["to_disponi_linea_credi"] = Convert.ToInt64(Session["to_disponi_linea_credi"]) + d;

                int diferencia_prom = promedio_3_meses_atras - monto_ultima_fact;
                //DIFERENCIA
                double.TryParse(diferencia_prom.ToString(), out d);
                if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                string diferencia_prom_1 = aux;

                //PROMEDIO
                double.TryParse(promedio_3_meses_atras.ToString(), out d);
                if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                string promedio_3_meses_atras_ = aux;

                string ultima_factu = e.Row.Cells[5].Text;

                ///segun promedio de ultimo trimestre 
                if (promedio_3_meses_atras >= monto_ultima_fact)
                {
                    //ROJO   NO SUPERA PRIMEDIO ULTIMO TRIMESTRE
                    e.Row.Cells[5].Text = "<span style='color:#ca4242;'> <b>" + e.Row.Cells[5].Text + "</b>  </span>";
                }
                else
                {
                    //SUPERA PROMEDIO ULTIMO TRIMESTRE
                    e.Row.Cells[5].Text = "<span style='color:#76ca42;'> <b>" + e.Row.Cells[5].Text + "</b>  </span>";
                }
                e.Row.Cells[5].ToolTip = promedio_3_meses_atras_ + " - " + ultima_factu + " = " + diferencia_prom_1;

                ////////////////////////TOOLTIPS
                if (e.Row.Cells[3].Text != "0")
                {
                    e.Row.Cells[2].ToolTip = "PROM :" + e.Row.Cells[3].Text;
                }
                e.Row.Cells[3].Visible = false;
                G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[3].Visible = false;

                e.Row.Cells[6].ToolTip = "DIAS :" + e.Row.Cells[7].Text;
                e.Row.Cells[7].Visible = false;
                G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[7].Visible = false;

                e.Row.Cells[10].ToolTip = e.Row.Cells[12].Text;
                e.Row.Cells[12].Visible = false;
                G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[12].Visible = false;


                ////////LINK A SP 


                //"<a href='REPORTE_SP.aspx?G=912&C=" + rut_ini.Trim() + "*" + vendedor + "' target='_blank'>SP</a> ";

            }
        }

        private string primer_dia(DateTime fe)
        {


            string desde = "";
            desde = "01/" + fe.Month + "/" + fe.Year;
            return desde;


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

        protected void G_INFORME_TOTAL_VENDEDOR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                //CONTACTOS ----> 


                string codvendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString());
                string nomb_vend = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString());

                string clie_rut = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                string nomb_client = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString());


                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                string script1 = string.Format("javascript:fuera22('{0}', '{1}');return false;", encriptador.EncryptData(clie_rut), encriptador.EncryptData("88"));
                string nombre_cliente_ficha = "<a href='javascript:' onclick='" + script1 + "'>" + nomb_client + " </a>";

                Session["rutcliente_"] = clie_rut.Trim();
                Session["codvendedor"] = codvendedor.Trim();
                Session["nombre_vendedor"] = nomb_vend.Trim();
                Session["nombre_cliente"] = nomb_client.Trim();

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
                cargar_grilla_contacto();

                Session["es_sn_venta"] = false;

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  modal_unidad_1('" + nomb_vend + "', '" + nomb_client + "', 'False', '"+ clie_rut + "');  </script>", false);
            }
            else if (e.CommandName == "Histo")
            {

                // correos
                try
                {
                    string codvendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString());
                    string nomb_vend = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString());

                    string clie_rut = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    string nomb_client = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString());

                    Session["rutcliente_"] = clie_rut.Trim();
                    Session["codvendedor"] = codvendedor.Trim();
                    Session["nombre_vendedor"] = nomb_vend.Trim();
                    Session["nombre_cliente"] = nomb_client.Trim();

                    cargar_grilla_correos();

                    contenido_correo.Visible = false;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "test222", "<script language='javascript'>  modal_unidad_2('" + nomb_vend + "', '" + nomb_client + "');  </script>", false);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "test2223", "<script language='javascript'>  alert('Error')  </script>", false);
                }
            }

            else if (e.CommandName == "agenda")
            {
                try
                {
                    string codvendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString());
                    string nomb_vend = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString());

                    string clie_rut = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    string nomb_client = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString());

                    Session["rutcliente_"] = clie_rut.Trim();
                    Session["codvendedor"] = codvendedor.Trim();
                    Session["nombre_vendedor"] = nomb_vend.Trim();
                    Session["nombre_cliente"] = nomb_client.Trim();

                    cargar_grilla_agendas();

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "test222", "<script language='javascript'>  modal_unidad_3('" + nomb_vend + "', '" + nomb_client + "');  </script>", false);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "test2223", "<script language='javascript'>  alert('Error')  </script>", false);
                }
            }


        }

        private void cargar_grilla_agendas()
        {
            string where = " where rutcliente = '" + Session["rutcliente_"].ToString() + "'";
            G_AGENDA.DataSource = agendacontactoBO.GetAll(where);
            G_AGENDA.DataBind();

            panel_agenda.Update();

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "carga_datable_g_agenda", "<script language='javascript'>  cargargrilla_agenda();  </script>", false);

        }

        private void cargar_grilla_contacto()
        {
            string where = " where rutcliente = '" + Session["rutcliente_"].ToString() + "'";
            G_CONTACTOS.DataSource = contactosvendBO.GetAll(where);
            G_CONTACTOS.DataBind();


            UpdatePanel4.Update();


            enviar_correo.Visible = false;
            div_agendar.Visible = false;

        }

        private void cargar_grilla_correos()
        {
            string where = " where rutcliente = '" + Session["rutcliente_"].ToString() + "'";

            G_CORREOS.DataSource = ReporteRNegocio.trae_correos_hist(where);
            G_CORREOS.DataBind();

            panel_histori.Update();
        }

        public string confirmDelete2(string Name)
        {
            return @"javascript:if(!confirm('Esta acción va eliminar el CONTACTO: "
               + Name.ToUpper()
               + @". ¿Estás seguro?')){return false;} ;";
        }

        protected void G_CONTACTOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }


        protected void btn_nuevo_contacto_ServerClick(object sender, EventArgs e)
        {
            B_Guardar.Text = "Crear";
            B_Guardar.Visible = true;
            btn_nuevo_contacto.Visible = false;


            t_nombre_contact.Text = String.Empty;
            t_cargo_contact.Text = String.Empty;
            t_num_contact.Text = String.Empty;
            t_correo_contact.Text = String.Empty;
            t_direcc_contact.Text = String.Empty;


            t_nombre_contact.Enabled = true;
            t_cargo_contact.Enabled = true;
            t_num_contact.Enabled = true;
            t_correo_contact.Enabled = true;
            t_direcc_contact.Enabled = true;

        }

        protected void btn_nuevo_contacto_modal_ServerClick(object sender, EventArgs e)
        {
            B_Guardar_modal.Text = "Crear";
            B_Guardar_modal.Visible = true;
            btn_nuevo_modal.Visible = false;


            t_nombre_modal.Text = String.Empty;
            t_cargo_modal.Text = String.Empty;
            t_num_modal.Text = String.Empty;
            t_correo_modal.Text = String.Empty;
            t_comuna_modal.Text = String.Empty;


            t_nombre_modal.Enabled = true;
            t_cargo_modal.Enabled = true;
            t_num_modal.Enabled = true;
            t_correo_modal.Enabled = true;
            t_comuna_modal.Enabled = true;

        }


        protected void B_Guardar_Click(object sender, EventArgs e)
        {
            if (B_Guardar.Text == "Crear")
            {
                if (t_nombre_contact.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab1", "<script language='javascript'>alert('El nombre no puede estar vacío');</script>", false);
                }
                else if (t_cargo_contact.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El cargo no puede estar vacío');</script>", false);
                }
                else if (t_num_contact.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El número no puede estar vacío');</script>", false);
                }
                else if (t_correo_contact.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El correo no puede estar vacío');</script>", false);
                }
                else if (t_direcc_contact.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('La comuna no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR

                    contactosvendEntity cont = new contactosvendEntity();
                    cont.rutcliente = Session["rutcliente_"].ToString();
                    cont.nombre_contacto = t_nombre_contact.Text;
                    cont.cargo = t_cargo_contact.Text;
                    cont.numero = t_num_contact.Text;
                    cont.correo = t_correo_contact.Text;
                    cont.direccion = t_direcc_contact.Text;

                    string ok = contactosvendBO.agregar(cont);

                    if (ok == "2" || ok == "1")
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Contacto creado! ');</script>", false);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Error al guardar Contacto! ');</script>", false);
                    }
                    B_Guardar.Visible = false;
                    btn_nuevo_contacto.Visible = true;

                    t_nombre_contact.Text = String.Empty;
                    t_cargo_contact.Text = String.Empty;
                    t_num_contact.Text = String.Empty;
                    t_correo_contact.Text = String.Empty;
                    t_direcc_contact.Text = String.Empty;

                    t_nombre_contact.Enabled = false;
                    t_cargo_contact.Enabled = false;
                    t_num_contact.Enabled = false;
                    t_correo_contact.Enabled = false;
                    t_direcc_contact.Enabled = false;

                    cargar_grilla_contacto();
                }
            }
            else
            {
                if (t_nombre_contact.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab1", "<script language='javascript'>alert('El nombre no puede estar vacío');</script>", false);
                }
                else if (t_cargo_contact.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El cargo no puede estar vacío');</script>", false);
                }
                else if (t_num_contact.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El número no puede estar vacío');</script>", false);
                }
                else if (t_correo_contact.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El correo no puede estar vacío');</script>", false);
                }
                else if (t_direcc_contact.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('La comuna no puede estar vacío');</script>", false);
                }
                else
                {
                    // EDITAR

                    contactosvendEntity cont = new contactosvendEntity();
                    cont.rutcliente = Session["rutcliente_"].ToString();
                    cont.nombre_contacto = t_nombre_contact.Text;
                    cont.cargo = t_cargo_contact.Text;
                    cont.numero = t_num_contact.Text;
                    cont.correo = t_correo_contact.Text;
                    cont.direccion = t_direcc_contact.Text;

                    string ok = contactosvendBO.actualizar(cont);

                    if (ok == "OK")
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Contacto Editado! ');</script>", false);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Error al editar Contacto! ');</script>", false);
                    }
                    B_Guardar.Visible = false;
                    btn_nuevo_contacto.Visible = true;

                    t_nombre_contact.Text = String.Empty;
                    t_cargo_contact.Text = String.Empty;
                    t_num_contact.Text = String.Empty;
                    t_correo_contact.Text = String.Empty;
                    t_direcc_contact.Text = String.Empty;

                    t_nombre_contact.Enabled = false;
                    t_cargo_contact.Enabled = false;
                    t_num_contact.Enabled = false;
                    t_correo_contact.Enabled = false;
                    t_direcc_contact.Enabled = false;

                    cargar_grilla_contacto();

                }
            }


        }


        protected void B_Guardar_modal_Click(object sender, EventArgs e)
        {

            //B_Guardar_modal.Visible = true;
            //btn_nuevo_modal.Visible = false;

            //t_nombre_modal.Text = String.Empty;
            //t_cargo_modal.Text = String.Empty;
            //t_num_modal.Text = String.Empty;
            //t_correo_modal.Text = String.Empty;
            //t_comuna_modal.Text = String.Empty;

            if (B_Guardar_modal.Text == "Crear")
            {
                if (t_nombre_modal.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab1", "<script language='javascript'>alert('El nombre no puede estar vacío');</script>", false);
                }
                else if (t_cargo_modal.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El cargo no puede estar vacío');</script>", false);
                }
                else if (t_num_modal.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El número no puede estar vacío');</script>", false);
                }
                else if (t_correo_modal.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El correo no puede estar vacío');</script>", false);
                }
                else if (t_comuna_modal.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('La comuna no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR

                    contactosvendEntity cont = new contactosvendEntity();
                    cont.rutcliente = Session["rutcliente_"].ToString();
                    cont.nombre_contacto = t_nombre_modal.Text;
                    cont.cargo = t_cargo_modal.Text;
                    cont.numero = t_num_modal.Text;
                    cont.correo = t_correo_modal.Text;
                    cont.direccion = t_comuna_modal.Text;

                    string ok = contactosvendBO.agregar(cont);

                    if (ok == "2" || ok == "1")
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Contacto creado! ');</script>", false);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Error al guardar Contacto! ');</script>", false);
                    }
                    B_Guardar_modal.Visible = false;
                    btn_nuevo_modal.Visible = true;

                    t_nombre_modal.Text = String.Empty;
                    t_cargo_modal.Text = String.Empty;
                    t_num_modal.Text = String.Empty;
                    t_correo_modal.Text = String.Empty;
                    t_comuna_modal.Text = String.Empty;

                    t_nombre_modal.Enabled = false;
                    t_cargo_modal.Enabled = false;
                    t_num_modal.Enabled = false;
                    t_correo_modal.Enabled = false;
                    t_comuna_modal.Enabled = false;

                    //cargar contactos
                    btn_contactos__Click(sender, e);
                }
            }
            else
            {
                if (t_nombre_modal.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab1", "<script language='javascript'>alert('El nombre no puede estar vacío');</script>", false);
                }
                else if (t_cargo_modal.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El cargo no puede estar vacío');</script>", false);
                }
                else if (t_num_modal.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El número no puede estar vacío');</script>", false);
                }
                else if (t_correo_modal.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El correo no puede estar vacío');</script>", false);
                }
                else if (t_comuna_modal.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('La comuna no puede estar vacío');</script>", false);
                }
                else
                {
                    // EDITAR

                    contactosvendEntity cont = new contactosvendEntity();
                    cont.rutcliente = Session["rutcliente_"].ToString();
                    cont.nombre_contacto = t_nombre_modal.Text;
                    cont.cargo = t_cargo_modal.Text;
                    cont.numero = t_num_modal.Text;
                    cont.correo = t_correo_modal.Text;
                    cont.direccion = t_comuna_modal.Text;

                    string ok = contactosvendBO.actualizar(cont);

                    if (ok == "OK")
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Contacto Editado! ');</script>", false);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Error al editar Contacto! ');</script>", false);
                    }
                    B_Guardar_modal.Visible = false;
                    btn_nuevo_modal.Visible = true;

                    t_nombre_modal.Text = String.Empty;
                    t_cargo_modal.Text = String.Empty;
                    t_num_modal.Text = String.Empty;
                    t_correo_modal.Text = String.Empty;
                    t_comuna_modal.Text = String.Empty;

                    t_nombre_modal.Enabled = false;
                    t_cargo_modal.Enabled = false;
                    t_num_modal.Enabled = false;
                    t_correo_modal.Enabled = false;
                    t_comuna_modal.Enabled = false;


                    //cargar contactos
                    btn_contactos__Click(sender, e);

                }
            }


        }

        protected void G_CONTACTOS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {

                string rutcliente = G_CONTACTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                string nombre_contacto = G_CONTACTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();
                contactosvendEntity cont = new contactosvendEntity();
                cont.rutcliente = rutcliente;
                cont.nombre_contacto = nombre_contacto;
                string ok = contactosvendBO.eliminar(cont);

                if (ok == "OK")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Contacto Eliminado! ');</script>", false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Error al eliminar Contacto! ');</script>", false);
                }
                cargar_grilla_contacto();
                JQ_Datatable();
                //cliente = G_PRODUCTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();
                //G_DET_PRODUCTOS.DataSource = ReporteRNegocio.listar_prod_client(vendedor, cliente, PERIODOS);
            }

            if (e.CommandName == "correo")
            {
                Session["nombre_contacto"] = G_CONTACTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();
                txt_destino.Text = G_CONTACTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString();
                txt_cc.Text = Session["correo_usuario"].ToString();
                enviar_correo.Visible = true;
                div_agendar.Visible = false;
                JQ_Datatable();
            }

            if (e.CommandName == "agendar")
            {
                //string rutcliente = G_CONTACTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                Session["nombre_contacto"] = G_CONTACTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();

                enviar_correo.Visible = false;
                div_agendar.Visible = true;
                JQ_Datatable();
            }

            if (e.CommandName == "Editar")
            {
                string rutcliente = G_CONTACTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                string nombre_contacto = G_CONTACTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();

                Session["rutcliente_"] = rutcliente;

                contactosvendEntity cont = new contactosvendEntity();
                cont.rutcliente = rutcliente;
                cont.nombre_contacto = nombre_contacto;

                string ok = contactosvendBO.encontrar(ref cont);

                B_Guardar.Text = "Editar";
                B_Guardar.Visible = true;
                btn_nuevo_contacto.Visible = false;

                t_nombre_contact.Text = cont.nombre_contacto;
                t_cargo_contact.Text = cont.cargo;
                t_num_contact.Text = cont.numero;
                t_correo_contact.Text = cont.correo;
                t_direcc_contact.Text = cont.direccion;

                t_nombre_contact.Enabled = false;
                t_cargo_contact.Enabled = true;
                t_num_contact.Enabled = true;
                t_correo_contact.Enabled = true;
                t_direcc_contact.Enabled = true;

                //if (ok == "OK")
                //{
                //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Contacto Editado! ');</script>", false);
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Error al editar Contacto! ');</script>", false);
                //}
                //cargar_grilla_contacto();
                //JQ_Datatable();

            }

        }

        protected void btn_enviar_correo_Click(object sender, EventArgs e)
        {
            //------------------------------------------------------------------- CORREO  -------------------------------------------------------------------

            //System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            //System.Web.UI.HtmlTextWriter htmlWrite =
            //new HtmlTextWriter(stringWrite);

            //G_CRUZADO.RenderControl(htmlWrite);

            //string body = stringWrite.ToString();

            //body = body.Replace("<tr class='test no-sort'>", "<tr style='background-color:#428bca;color:#fff'>");

            DataTable corr = ReporteRNegocio.corr_usuario(User.Identity.Name);
            string nombre_vend = Session["nombre_vendedor"].ToString();
            string nombre_cliente = Session["nombre_cliente"].ToString().Trim();

            string cc = txt_cc.Text;

            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(txt_destino.Text));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = txt_asunto.Text;
            if (cc != "")
            {
                email.CC.Add(cc);
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
            email.Body += "<div><img src='http://a58.imgup.net/Sopro4d9d.png' style='    float: right;     width: 90px;'> </div><br><br><br>";

            //email.Body += "<div> Estimado  " + nombre_cliente + ": </b> <br> <br>";

            string body_correo = tx_contenido.InnerText;
            email.Body += "<div>" + body_correo.Replace("\n", " <br>") + "</div>";

            //email.Body += "<br>" + "<img src='http://mail.thinxsys.cl/Main/frmReadMail_Attachment.aspx?folder=inbox&uid=920&partid=10&filename=image003.jpg' style='    float: right;     width: 90px;'>";
            try
            {
                email.Body += "<br> <br> <b>" + nombre_vend.Substring(0, nombre_vend.IndexOf('(')) + "<b>";
            }
            catch
            {

                email.Body += "<br> <br> <b>" + nombre_vend + "<b>";

            }
            email.Body += "( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) " + " <span style='font:5px; color:gray; float:right;'>No Responder Este Correo</span> <br><br>";
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

            //desde @soprodi

            //email.IsBodyHtml = true;
            //email.Priority = MailPriority.Normal;
            //email.BodyEncoding = System.Text.Encoding.UTF8;
            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = "srv-correo-2.soprodi.cl";
            //smtp.Port = 25;
            //smtp.EnableSsl = false;
            //smtp.UseDefaultCredentials = false;
            //smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            //try
            //{
            //    smtp.Send(email);
            //    email.Dispose();

            //    guardar_envio_correo();

            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Correo Enviado');</script>", false);

            //}
            //catch (Exception ex)
            //{

            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('No enviado');</script>", false);

            //}
            /////desde gmail

            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;
            email.BodyEncoding = System.Text.Encoding.UTF8;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;

            //smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            smtp.Credentials = new NetworkCredential("informes.soprodi@gmail.com", "galaxia1234");
            try
            {
                Console.Write("aca_");
                smtp.Send(email);
                email.Dispose();
                guardar_envio_correo();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Correo Enviado');</script>", false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('No enviado');</script>", false);
                Console.Write(ex);

            }


            enviar_correo.Visible = false;

        }



        protected void btn_enviar_modal_Click(object sender, EventArgs e)
        {
            //------------------------------------------------------------------- CORREO  -------------------------------------------------------------------

            //System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            //System.Web.UI.HtmlTextWriter htmlWrite =
            //new HtmlTextWriter(stringWrite);

            //G_CRUZADO.RenderControl(htmlWrite);

            //string body = stringWrite.ToString();

            //body = body.Replace("<tr class='test no-sort'>", "<tr style='background-color:#428bca;color:#fff'>");

            DataTable corr = ReporteRNegocio.corr_usuario(User.Identity.Name);
            string nombre_vend = Session["nombre_vendedor"].ToString();
            string nombre_cliente = Session["nombre_cliente"].ToString().Trim();

            string cc = t_cc_modal.Text;

            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(t_destino_modal.Text));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = t_asunto_modal.Text;
            if (cc != "")
            {
                email.CC.Add(cc);
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
            email.Body += "<div><img src='http://a58.imgup.net/Sopro4d9d.png' style='    float: right;     width: 90px;'> </div><br><br><br>";

            //email.Body += "<div> Estimado  " + nombre_cliente + ": </b> <br> <br>";

            string body_correo = t_contenido_modal.InnerText;
            email.Body += "<div>" + body_correo.Replace("\n", " <br>") + "</div>";

            //email.Body += "<br>" + "<img src='http://mail.thinxsys.cl/Main/frmReadMail_Attachment.aspx?folder=inbox&uid=920&partid=10&filename=image003.jpg' style='    float: right;     width: 90px;'>";
            try
            {
                email.Body += "<br> <br> <b>" + nombre_vend.Substring(0, nombre_vend.IndexOf('(')) + "<b>";
            }
            catch
            {

                email.Body += "<br> <br> <b>" + nombre_vend + "<b>";

            }
            email.Body += "( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) " + " <span style='font:5px; color:gray; float:right;'>No Responder Este Correo</span> <br><br>";
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

            //desde @soprodi

            //email.IsBodyHtml = true;
            //email.Priority = MailPriority.Normal;
            //email.BodyEncoding = System.Text.Encoding.UTF8;
            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = "srv-correo-2.soprodi.cl";
            //smtp.Port = 25;
            //smtp.EnableSsl = false;
            //smtp.UseDefaultCredentials = false;
            //smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            //try
            //{
            //    smtp.Send(email);
            //    email.Dispose();

            //    guardar_envio_correo();

            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Correo Enviado');</script>", false);

            //}
            //catch (Exception ex)
            //{

            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('No enviado');</script>", false);

            //}
            /////desde gmail

            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;
            email.BodyEncoding = System.Text.Encoding.UTF8;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;

            //smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            smtp.Credentials = new NetworkCredential("informes.soprodi@gmail.com", "galaxia1234");
            try
            {
                Console.Write("aca_");
                smtp.Send(email);
                email.Dispose();
                guardar_envio_correo_modal();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Correo Enviado');</script>", false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('No enviado');</script>", false);
                Console.Write(ex);

            }


            enviar_correo_modal.Visible = false;

        }





        private void guardar_envio_correo()
        {
            correocontactoEntity corr = new correocontactoEntity();
            corr.adjunto = "";
            corr.asunto = txt_asunto.Text;
            corr.cc = "";
            corr.codvendedor = Session["codvendedor"].ToString();
            corr.rutcliente = Session["rutcliente_"].ToString();
            corr.nombre_contacto = Session["nombre_contacto"].ToString();
            corr.destinatario = txt_destino.Text;
            corr.contenido = tx_contenido.InnerText.Replace("\n", " <br>");
            corr.fecha = DateTime.Now;
            string ok = correocontactoBO.agregar(corr);

            Console.WriteLine(ok);




        }



        private void guardar_envio_correo_modal()
        {
            correocontactoEntity corr = new correocontactoEntity();
            corr.adjunto = "";
            corr.asunto = t_asunto_modal.Text;
            corr.cc = "";
            corr.codvendedor = Session["codvendedor"].ToString();
            corr.rutcliente = Session["rutcliente_"].ToString();
            corr.nombre_contacto = Session["nombre_contacto"].ToString();
            corr.destinatario = t_destino_modal.Text;
            corr.contenido = t_contenido_modal.InnerText.Replace("\n", " <br>");
            corr.fecha = DateTime.Now;
            string ok = correocontactoBO.agregar(corr);

            Console.WriteLine(ok);




        }

        protected void btn_cancelar_Click(object sender, EventArgs e)
        {
            div_agendar.Visible = false;
            enviar_correo.Visible = false;
        }


        protected void btn_cancelar_modal_Click(object sender, EventArgs e)
        {
            div_agendar_modal.Visible = false;
            enviar_correo_modal.Visible = false;
        }


        protected void G_CORREOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void G_CORREOS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Abrir")
            {

                string fecha = (G_CORREOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString());
                string contenido = (G_CORREOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString());
                string correo_destino = (G_CORREOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString());


                string dibujar_correo = "";

                dibujar_correo += "<div style='text-align:center;     display: block !important;' > ";
                dibujar_correo += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                dibujar_correo += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                dibujar_correo += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                dibujar_correo += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                dibujar_correo += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                dibujar_correo += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                dibujar_correo += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                dibujar_correo += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div> ";
                dibujar_correo += "</div>";
                dibujar_correo += "<div><img src='http://a58.imgup.net/Sopro4d9d.png' style='    float: right;     width: 90px;'> </div><br><br><br><br><br>";

                dibujar_correo += "<div>" + contenido + "</div>";

                dibujar_correo += "<div style='text-align:center;     display: block !important;' > ";
                dibujar_correo += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                dibujar_correo += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                dibujar_correo += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                dibujar_correo += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                dibujar_correo += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                dibujar_correo += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                dibujar_correo += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                dibujar_correo += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div> ";
                dibujar_correo += "</div>";


                contenido_correo.Visible = true;

                div_contenido_correo.InnerHtml = dibujar_correo;

            }
        }

        public bool es_visible(string ultimo_contacto)
        {
            if (ultimo_contacto == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool es_visible_agenda(string tiene_agenda)
        {
            if (tiene_agenda == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected void btn_masivo_correo_Click(object sender, EventArgs e)
        {
            txt_destino.Text = "";
            bool entro1 = false;
            List<string> correos = new List<string>();
            foreach (GridViewRow dtgItem in this.G_CONTACTOS.Rows)
            {
                CheckBox Sel = ((CheckBox)G_CONTACTOS.Rows[dtgItem.RowIndex].FindControl("chkAccept"));
                bool valor = Sel.Checked;
                if (valor)
                {
                    entro1 = true;
                    txt_destino.Text += (G_CONTACTOS.DataKeys[dtgItem.RowIndex].Values[2].ToString().Trim()) + ", ";
                }
                else
                {

                }
            }
            if (!entro1)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeeqqqe", "<script language='javascript'>alert('Debe seleccionar Contanctos');</script>", false);
            }
            else
            {
                //panel_enviar.Visible = true;
                //btn_enviar_correos.Visible = false;
            }
            txt_destino.Text = txt_destino.Text.Substring(0, txt_destino.Text.Length - 2);
            enviar_correo.Visible = true;
            JQ_Datatable();
        }

        protected void btn_refresh_Click1(object sender, EventArgs e)
        {
            //JQ_Datatable();
        }

        protected void btn_agendar_Click(object sender, EventArgs e)
        {
            agendacontactoEntity ag = new agendacontactoEntity();
            ag.rutcliente = Session["rutcliente_"].ToString();
            ag.nombre_contacto = Session["nombre_contacto"].ToString();

            ag.observacion = tx_obser.InnerText;
            ag.fecha_agenda = Convert.ToDateTime(txt_dia_agenda.Text);
            ag.am_pm = am_pm.Text;
            ag.codvendedor = Session["codvendedor"].ToString();

            string ok = agendacontactoBO.agregar(ag);

            if (ok == "2" || ok == "1")
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Agendado');</script>", false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Error al agendar! ');</script>", false);
            }
        }

        protected void btn_agendar_modal_Click(object sender, EventArgs e)
        {
            agendacontactoEntity ag = new agendacontactoEntity();
            ag.rutcliente = Session["rutcliente_"].ToString();
            ag.nombre_contacto = Session["nombre_contacto"].ToString();

            ag.observacion = t_obs_modal.InnerText;
            ag.fecha_agenda = Convert.ToDateTime(t_dia_modal.Text);
            ag.am_pm = t_hora_modal.Text;
            ag.codvendedor = Session["codvendedor"].ToString();

            string ok = agendacontactoBO.agregar(ag);

            if (ok == "2" || ok == "1")
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>refresh_calendario(); alert('Agendado');</script>", false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Error al agendar! ');</script>", false);
            }
        }


        protected void G_AGENDA_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {

                string nom_contacto = G_AGENDA.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                string fecha_Agenda = G_AGENDA.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();
                string codvendedor = G_AGENDA.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[4].ToString();


                agendacontactoEntity ag = new agendacontactoEntity();
                ag.rutcliente = Session["rutcliente_"].ToString();
                ag.nombre_contacto = nom_contacto;
                ag.fecha_agenda = Convert.ToDateTime(fecha_Agenda);
                ag.codvendedor = codvendedor;
                string ok = agendacontactoBO.eliminar(ag);

                if (ok == "OK")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Agenda Eliminada! ');</script>", false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Error al eliminar agenda! ');</script>", false);
                }
                cargar_grilla_agendas();
                JQ_Datatable();
            }
            if (e.CommandName == "Editar")
            {

                string nom_contacto = G_AGENDA.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                string fecha_Agenda = G_AGENDA.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();
                string codvendedor = G_AGENDA.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[4].ToString();

                Session["nombre_contacto"] = nom_contacto;
                Session["codvendedor"] = codvendedor;
                Session["fecha_agenda"] = Convert.ToDateTime(fecha_Agenda);

                agendacontactoEntity ag = new agendacontactoEntity();
                ag.rutcliente = Session["rutcliente_"].ToString();
                ag.nombre_contacto = nom_contacto;
                ag.fecha_agenda = Convert.ToDateTime(fecha_Agenda);
                ag.codvendedor = codvendedor;
                agendacontactoBO.encontrar(ref ag);

                t_dia_edit.Text = ag.fecha_agenda.ToString("yyyy-MM-dd");
                t_time_edit.Text = ag.am_pm;
                t_obs_edit.InnerText = ag.observacion;

                div_editar_agenda.Visible = true;

                Session["es_sn_venta"] = false;

            }
        }

        private string dia_con_ceros(string fecha)
        {
            string dia = "";
            string mes = "";
            string año = "";

            dia = fecha.Substring(0, fecha.IndexOf("-"));
            fecha = fecha.Substring(fecha.IndexOf("-") + 1);
            mes = fecha.Substring(0, fecha.IndexOf("-"));
            año = fecha.Substring(fecha.LastIndexOf("-") + 1);

            if (dia.Length == 1)
            {
                dia = "0" + dia;

            }

            if (mes.Length == 1)
            {
                mes = "0" + mes;

            }

            return dia + "-" + mes + "-" + año;



        }

        protected void G_AGENDA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                int dias_diferencia = Convert.ToInt32(G_AGENDA.DataKeys[e.Row.RowIndex].Values[2].ToString().Trim());


                switch (dias_diferencia)
                {
                    case 0:
                        e.Row.Attributes["class"] = "estado_0_max";
                        break;

                    default:
                        e.Row.Attributes["class"] = "estado_0_1";
                        break;
                }


            }
        }
        public string confimar_delete(string nombre_contacto)
        {
            return @"javascript:if(!confirm('Esta acción va eliminar la agenda programada de contacto: "
               + nombre_contacto.ToUpper()
               + @". ¿Estás seguro?')){return false;} ;";
        }

        protected void G_SIN_VENTAS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string rutcliente = G_SIN_VENTAS.DataKeys[e.Row.RowIndex].Values[1].ToString().Trim();

                //MONTO LINEA CREDITO
                double d;
                double.TryParse(e.Row.Cells[4].Text, out d);
                string aux = "";
                if (d == 0) { aux = e.Row.Cells[4].Text; } else { aux = d.ToString("N0"); }
                e.Row.Cells[4].Text = aux;


                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                string script1 = string.Format("javascript:fuera22(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(rutcliente), encriptador.EncryptData("88"));
                e.Row.Cells[0].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[0].Text + " </a>";


            }

        }
        protected void b_agenda__Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "calendario_1", "<script language='javascript'>CARGAR_CALENDARIO('" + Session["codvendedor"].ToString() + "');</script>", false);


            bool es_vendedor = (bool)Session["es_vendedor"];

            if (es_vendedor)
            {
                Session["codvendedor"] = USER.Trim();
            }
            else
            {


            }

            string hoy = DateTime.Now.ToString("yyyy-MM-dd"); ;
            string mas_6 = DateTime.Now.AddDays(6).ToString("yyyy-MM-dd"); ;

            txt_desde1.Text = hoy;
            txt_hasta1.Text = mas_6;

            DateTime desde = Convert.ToDateTime(txt_desde1.Text);
            DateTime hasta = Convert.ToDateTime(txt_hasta1.Text);

            List<DateTime> dias_semaa = DiasEnSemana(desde, hasta);

            //string where = " where codvendedor= '" + Session["codvendedor"].ToString() + "' and CONVERT(DATETIME, fecha_agenda,103) between cast( dateadd(day, datediff(day, 0, current_timestamp), 0) as smalldatetime) and GetDate() +6";

            string cod_vendedor = "";
            try
            {

                cod_vendedor = Session["codvendedor"].ToString().Replace("'", "").Substring(0, Session["codvendedor"].ToString().IndexOf(','));
            }
            catch
            {

                cod_vendedor = Session["codvendedor"].ToString().Replace("'", "");
            }

            string where = " where codvendedor= '" + cod_vendedor + "' and CONVERT(DATETIME, fecha_agenda,103) between  " +
                     "  CONVERT(DATETIME, '" + desde.ToShortDateString() + "',103)  and CONVERT(DATETIME, '" + hasta.ToShortDateString() + "',103)           ";

            string html_tabla = crear_calendario(where, dias_semaa);
            agenda_div.InnerHtml = html_tabla;
            string html_excel = formatear_tabla_para_excel(html_tabla);
            R_Excel_1.InnerHtml = html_excel;

            UpdatePanel44.Update();

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "calendario_1", "<script language='javascript'>modal_unidad_4();</script>", false);

        }

        private string formatear_tabla_para_excel(string html_tabla)
        {
            return html_tabla.Replace("TABLA_REPORTE55", "TABLA_PARA_EXCEL")
                              .Replace("style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;' ", "")
                              .Replace("color: #ffffff;", "")
                              .Replace("Miércoles", "Miercoles")
                              .Replace("DÍA", "DIA");
        }

        private string crear_calendario(string where, List<DateTime> dias_semaa)
        {
            DataTable dt_ag = agendacontactoBO.GetAll(where);
            DataTable dt_cliente_vendedor = (DataTable)Session["dt_clientes_vendedor"];

            string HTML = "";
            HTML += " <div style='overflow-x: auto;'> <table id='TABLA_REPORTE55' class='table table-advance table-bordered fill-head tablesorter filtrar' style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";
            HTML += "<thead>";
            HTML += "</tr>";

            HTML += "<tr>";
            HTML += "<th colspan=1; class='test sorter-false' style='background-color: white !important; color:black !important;'>CONTACTO / DÍA</th>";
            foreach (DateTime dia in dias_semaa)
            {
                string dia_semana = dia_semana_español(dia.DayOfWeek.ToString());

                if (dia_semana != "Sábado" && dia_semana != "Domingo")
                {
                    HTML += "<th colspan=1; class='test sorter-false' >" + dia_semana + " " + dia.ToShortDateString() + "</th>";
                    HTML += "<th colspan=1; class='test sorter-false' > &nbsp; </th>";
                }
            }
            HTML += "</tr>";


            //////aca sort ------------>
            HTML += "<tr>";
            HTML += "<th colspan=1; style='background-color: white !important; color:black !important;'> &nbsp; </th>";
            foreach (DateTime dia in dias_semaa)
            {
                string dia_semana = dia_semana_español(dia.DayOfWeek.ToString());

                if (dia_semana != "Sábado" && dia_semana != "Domingo")
                {
                    HTML += "<th colspan=1; class='test'> &nbsp; </th>";
                    HTML += "<th colspan=1; class='test'> &nbsp; </th>";
                    //HTML += "<th colspan=1; class='test sorter-false'> </th>";
                }
            }
            HTML += "</tr>";

            HTML += "</thead>";
            HTML += "<tbody>";
            int dias_agenda = 0;

            foreach (DataRow r in dt_ag.Rows)
            {
                HTML += "<tr>";

                ///NOMBRE DEL CLIENTE ---<
                ///
                string script1 = string.Format("javascript:agregar_agenda(&#39;{0}&#39;,&#39;{1}&#39;,&#39;{2}&#39;,&#39;{3}&#39;,&#39;{4}&#39;, &#39;{5}&#39;, &#39;{6}&#39;,  &#39;{7}&#39;);",
                                        r[5].ToString().Trim(), r[3].ToString().Trim(), r[0].ToString().Trim(), r[1].ToString().Trim(), r[2].ToString().Trim(),
                                          r[4].ToString().Trim(), r[7].ToString().Trim(), r[9].ToString().Trim());

                HTML += "         <td  class='leftfijo test' style='white-space: nowrap;' style='white-space: nowrap;'><a target = '_blank' style='cursor: pointer;color: #ffffff;' onclick='" + script1 + "' > " + r[7].ToString() + "</a></td>";

                foreach (DateTime dia in dias_semaa)
                {
                    string sabado_domingo = dia_semana_español(dia.DayOfWeek.ToString());

                    if (sabado_domingo != "Sábado" && sabado_domingo != "Domingo")
                    {
                        dias_agenda++;

                        string dia_sinceros = dia.ToShortDateString().Replace("-", "/");
                        //string dia_sinceros = "02/04/2019";
                        if (dia_sinceros.Substring(0, 1) == "0")
                        {
                            dia_sinceros = dia_sinceros.Substring(1);
                        }
                        if (dia_sinceros.Substring(dia_sinceros.IndexOf('/') + 1, 1) == "0")
                        {
                            int dia_s = Convert.ToInt32(dia_sinceros.Substring(0, dia_sinceros.IndexOf('/')));
                            int mes_s = Convert.ToInt32(dia_sinceros.Substring(dia_sinceros.IndexOf('/') + 1, 2));
                            int año_s = Convert.ToInt32(dia_sinceros.Substring(dia_sinceros.LastIndexOf('/') + 1));

                            dia_sinceros = dia_s + "/" + mes_s + "/" + año_s;
                        }


                        if (dia_sinceros == r[8].ToString())
                        {
                            string hora = r["am_pm"].ToString().Trim().Replace("hrs", "").Replace("hr", "").Replace(": ", ":").Replace(" :", ":").Replace(";", ":").Trim();
                            if (hora.Length == 4)
                            {
                                hora = hora.Substring(0, 2) + ":" + hora.Substring(2);
                            }

                            ///VER_OBES ( OBS, AM_PM, RUTCLIENTE, FECHA_AGENDA, NOMBRE_CONTACTO, CODVENDEDOR)
                            string script = string.Format("javascript:ver_obs(&#39;{0}&#39;,&#39;{1}&#39;,&#39;{2}&#39;,&#39;{3}&#39;,&#39;{4}&#39;, &#39;{5}&#39;, &#39;{6}&#39;);", r[5].ToString().Trim(), r[3].ToString().Trim(), r[0].ToString().Trim(), r[1].ToString().Trim(), r[2].ToString().Trim(), r[4].ToString().Trim(), r[7].ToString().Trim());
                            HTML += "         <td style='white-space: nowrap;'> " + r[2].ToString().Trim().ToUpper() + "</td>";
                            HTML += "         <td style='white-space: nowrap;'> <a target = '_blank' style='cursor: pointer;color: #ff0000;' onclick='" + script + "' > " + hora + "</a></td>";
                            //HTML += "         <td style='white-space: nowrap;'><a target = '_blank' style='cursor: pointer;' onclick='" + script + "' ><i class='fa fa-search' aria-hidden='true'></i></a></td>";

                        }
                        else
                        {
                            HTML += "         <td style='white-space: nowrap;'>&nbsp;</td>";
                            HTML += "         <td style='white-space: nowrap;'>&nbsp;</td>";
                            //HTML += "         <td style='white-space: nowrap;'></td>";
                        }
                    }
                }
                HTML += "</tr>";
            }
            HTML += "</tr>";
            HTML += "</tbody>";


            //// FOOOOOTERR      ACA  CLIENTES QUE NO HAN SIGO AGENDADO
            HTML += "<tfoot>";

            foreach (DataRow r2 in dt_cliente_vendedor.Rows)
            {

                bool esta_agendado = false;
                foreach (DataRow r3 in dt_ag.Rows)
                {
                    if (r3["rutcliente"].ToString().Trim() == r2["rutcliente"].ToString().Trim())
                    {
                        esta_agendado = true;
                    }
                }
                if (!esta_agendado)
                {
                    HTML += "<tr>";
                    string script1 = string.Format("javascript:agregar_agenda(&#39;{0}&#39;,&#39;{1}&#39;,&#39;{2}&#39;,&#39;{3}&#39;,&#39;{4}&#39;, &#39;{5}&#39;, &#39;{6}&#39;,  &#39;{7}&#39;);",
                                       "", "", r2[1].ToString().Trim(), "", "", r2[4].ToString().Trim(), r2[2].ToString().Trim(), r2[3].ToString().Trim());
                    HTML += "<th colspan=1; class='sorter-false'  style='background-color: rgb(255, 132, 132) !important; color:white !important;'><a target = '_blank' style='cursor: pointer;color: #ffffff;' onclick='" + script1 + "' > " + r2["nombre cliente"].ToString() + "</a></td>";
                    HTML += "<th colspan=" + dias_agenda + "; class='sorter-false' style='border-top: #ffebeb 2px solid; background-color: white !important; color:white !important;' >&nbsp;</th>";
                    HTML += "</tr>";
                }
            }
            HTML += "</tfoot>";
            HTML += "  </table> </div>";
            return HTML;
        }

        private string dia_semana_español(string v)
        {
            string dia_español = "";
            if (v == "Monday")
            {
                dia_español = "Lunes";
            }
            else if (v == "Tuesday")
            {
                dia_español = "Martes";
            }
            else if (v == "Wednesday")
            {
                dia_español = "Miércoles";
            }
            else if (v == "Thursday")
            {
                dia_español = "Jueves";
            }
            else if (v == "Friday")
            {
                dia_español = "Viernes";
            }
            else if (v == "Saturday")
            {
                dia_español = "Sábado";
            }
            else
            {
                dia_español = "Domingo";
            }

            return dia_español;
        }

        public static List<DateTime> DiasEnSemana(DateTime desde, DateTime hasta)
        {

            List<DateTime> days = new List<DateTime>();

            TimeSpan difference = hasta.Subtract(desde.Date);
            DateTime hoy = desde;
            days.Add(hoy);

            if (difference.TotalDays == 0)
            {

            }
            else
            {
                for (var i = 1; i <= difference.TotalDays; i++)
                {
                    days.Add(hoy.AddDays(i));
                }
            }
            return days;
        }

        protected void btn_trae_sin_venta_Click(object sender, EventArgs e)
        {
            string vendedores = agregra_comillas(Session["l_vendedores.Text"].ToString());
            //string clientes = agregra_comillas(l_clientes.Text);

            string es_vendedor = ReporteRNegocio.esvendedor(USER);
            string where = " where 1=1";
            string where_sin_venta = " where 1=1";

            //if (clientes != "")
            //{

            //    where = where + " and a.rutcliente in (" + clientes + ") ";
            //}
            if (vendedores != "")
            {
                where = where + " and a.codvendedor in (" + vendedores + ") ";
                where_sin_venta = where_sin_venta + " and SLSPERID in (" + vendedores + ") ";

            }

            if (es_vendedor == "2")
            {
                where_sin_venta += " and SLSPERID in ('" + USER + "')";
                where += " and a.codvendedor in ('" + USER + "')";
            }

            Session["codvendedor"] = l_vendedores.Text;


            string grupos_del_usuario = "";

            grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

            if (grupos_del_usuario == "")
            {
                grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
            }

            where += " and a.user1 in (" + grupos_del_usuario + ") ";

            G_SIN_VENTAS.DataSource = ReporteRNegocio.trae_cliente_vendedor_SINVENTA(where_sin_venta);
            G_SIN_VENTAS.DataBind();
            JQ_Datatable();

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "test_click", "<script language='javascript'>document.getElementById('li_SIN_VENTA').click();</script>", false);
        }

        protected void G_SIN_VENTAS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                string codvendedor = (G_SIN_VENTAS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString());
                string nomb_vend = (G_SIN_VENTAS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString());

                string clie_rut = (G_SIN_VENTAS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                string nomb_client = (G_SIN_VENTAS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());

                Session["rutcliente_"] = clie_rut.Trim();
                Session["codvendedor"] = codvendedor.Trim();
                Session["nombre_vendedor"] = nomb_vend.Trim();
                Session["nombre_cliente"] = nomb_client.Trim();

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
                cargar_grilla_contacto();

                Session["es_sn_venta"] = true;

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  modal_unidad_1('" + nomb_vend + "', '" + nomb_client + "', 'True', '"+ rut_ini + "');  </script>", false);
            }

            else if (e.CommandName == "Histo")
            {
                try
                {
                    string codvendedor = (G_SIN_VENTAS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString());
                    string nomb_vend = (G_SIN_VENTAS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString());

                    string clie_rut = (G_SIN_VENTAS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                    string nomb_client = (G_SIN_VENTAS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());

                    Session["rutcliente_"] = clie_rut.Trim();
                    Session["codvendedor"] = codvendedor.Trim();
                    Session["nombre_vendedor"] = nomb_vend.Trim();
                    Session["nombre_cliente"] = nomb_client.Trim();

                    cargar_grilla_correos();

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "test222", "<script language='javascript'>  modal_unidad_2('" + nomb_vend + "', '" + nomb_client + "');  </script>", false);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "test2223", "<script language='javascript'>  alert('Error')  </script>", false);
                }
            }

            else if (e.CommandName == "agenda")
            {
                try
                {
                    string codvendedor = (G_SIN_VENTAS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString());
                    string nomb_vend = (G_SIN_VENTAS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString());

                    string clie_rut = (G_SIN_VENTAS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                    string nomb_client = (G_SIN_VENTAS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());


                    Session["rutcliente_"] = clie_rut.Trim();
                    Session["codvendedor"] = codvendedor.Trim();
                    Session["nombre_vendedor"] = nomb_vend.Trim();
                    Session["nombre_cliente"] = nomb_client.Trim();

                    cargar_grilla_agendas();

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "test222", "<script language='javascript'>  modal_unidad_3('" + nomb_vend + "', '" + nomb_client + "');  </script>", false);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "test2223", "<script language='javascript'>  alert('Error')  </script>", false);
                }
            }


        }

        protected void btn_guarda_editar_Click(object sender, EventArgs e)
        {
            agendacontactoEntity ag = new agendacontactoEntity();
            ag.rutcliente = Session["rutcliente_"].ToString();
            ag.nombre_contacto = Session["nombre_contacto"].ToString();
            ag.codvendedor = Session["codvendedor"].ToString();

            DateTime fecha_antigua = (DateTime)Session["fecha_agenda"];

            ag.fecha_agenda = Convert.ToDateTime(t_dia_edit.Text);

            ag.observacion = t_obs_edit.InnerText;
            ag.am_pm = t_time_edit.Text;

            string ok = agendacontactoBO.actualizar(ag, fecha_antigua);

            if (ok == "OK")
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Agenda Editada');</script>", false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Error al editar agendar! ');</script>", false);
            }

            cargar_grilla_agendas();
            div_editar_agenda.Visible = false;
        }

        protected void btn_cancelar_editar_Click(object sender, EventArgs e)
        {
            div_editar_agenda.Visible = false;
        }

        protected void bn_trae_calendario_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "calendario_1", "<script language='javascript'>CARGAR_CALENDARIO('" + Session["codvendedor"].ToString() + "');</script>", false);

            //string where = " where codvendedor= '" + Session["codvendedor"].ToString() + "' and CONVERT(DATETIME, fecha_agenda,103) between cast( dateadd(day, datediff(day, 0, current_timestamp), 0) as smalldatetime) and GetDate() +6";



            DateTime desde = Convert.ToDateTime(txt_desde1.Text);
            DateTime hasta = Convert.ToDateTime(txt_hasta1.Text);

            List<DateTime> dias_semaa = DiasEnSemana(desde, hasta);
            string where = " where codvendedor= '" + Session["codvendedor"].ToString() + "' and CONVERT(DATETIME, fecha_agenda,103) between  " +
                     "  CONVERT(DATETIME, '" + desde.ToShortDateString() + "',103)  and CONVERT(DATETIME, '" + hasta.ToShortDateString() + "',103)           ";

            DataTable dt_ag = agendacontactoBO.GetAll(where);
            DataTable dt_cliente_vendedor = (DataTable)Session["dt_clientes_vendedor"];

            string HTML = "";
            HTML += "<table id='TABLA_REPORTE55' class='table table-advance table-bordered fill-head tablesorter filtrar' style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";
            HTML += "<thead>";
            HTML += "</tr>";

            HTML += "<tr>";
            HTML += "<th colspan=1; class='test sorter-false' style='background-color: white !important; color:black !important;'>CONTACTO / DÍA</th>";
            foreach (DateTime dia in dias_semaa)
            {
                string dia_semana = dia_semana_español(dia.DayOfWeek.ToString());

                if (dia_semana != "Sábado" && dia_semana != "Domingo")
                {
                    HTML += "<th colspan=2; class='test sorter-false' >" + dia_semana + " " + dia.ToShortDateString() + "</th>";
                }
            }
            HTML += "</tr>";


            //////aca sort ------------>
            HTML += "<tr>";
            HTML += "<th colspan=1; style='background-color: white !important; color:black !important;'> </th>";
            foreach (DateTime dia in dias_semaa)
            {
                string dia_semana = dia_semana_español(dia.DayOfWeek.ToString());

                if (dia_semana != "Sábado" && dia_semana != "Domingo")
                {
                    HTML += "<th colspan=1; class='test'> </th>";
                    HTML += "<th colspan=1; class='test'> </th>";
                    //HTML += "<th colspan=1; class='test sorter-false'> </th>";
                }
            }
            HTML += "</tr>";

            HTML += "</thead>";
            HTML += "<tbody>";
            int dias_agenda = 0;

            foreach (DataRow r in dt_ag.Rows)
            {
                HTML += "<tr>";
                ///NOMBRE DEL CLIENTE ---<
                ///
                string script1 = string.Format("javascript:agregar_agenda(&#39;{0}&#39;,&#39;{1}&#39;,&#39;{2}&#39;,&#39;{3}&#39;,&#39;{4}&#39;, &#39;{5}&#39;, &#39;{6}&#39;,  &#39;{7}&#39;);",
                                      r[5].ToString().Trim(), r[3].ToString().Trim(), r[0].ToString().Trim(), r[1].ToString().Trim(), r[2].ToString().Trim(),
                                        r[4].ToString().Trim(), r[7].ToString().Trim(), r[9].ToString().Trim());


                HTML += "         <td  class='leftfijo test' style='white-space: nowrap;' style='white-space: nowrap;'><a target = '_blank' style='cursor: pointer;color: #ffffff;' onclick='" + script1 + "' > " + r[7].ToString() + "</a></td>";

                foreach (DateTime dia in dias_semaa)
                {
                    string sabado_domingo = dia_semana_español(dia.DayOfWeek.ToString());

                    if (sabado_domingo != "Sábado" && sabado_domingo != "Domingo")
                    {
                        dias_agenda++;

                        string dia_sinceros = dia.ToShortDateString().Replace("-", "/");
                        //string dia_sinceros = "02/04/2019";
                        if (dia_sinceros.Substring(0, 1) == "0")
                        {
                            dia_sinceros = dia_sinceros.Substring(1);
                        }
                        if (dia_sinceros.Substring(dia_sinceros.IndexOf('/') + 1, 1) == "0")
                        {
                            int dia_s = Convert.ToInt32(dia_sinceros.Substring(0, dia_sinceros.IndexOf('/')));
                            int mes_s = Convert.ToInt32(dia_sinceros.Substring(dia_sinceros.IndexOf('/') + 1, 2));
                            int año_s = Convert.ToInt32(dia_sinceros.Substring(dia_sinceros.LastIndexOf('/') + 1));

                            dia_sinceros = dia_s + "/" + mes_s + "/" + año_s;
                        }


                        if (dia_sinceros == r[8].ToString())
                        {
                            string hora = r["am_pm"].ToString().Trim().Replace("hrs", "").Replace("hr", "").Replace(": ", ":").Replace(" :", ":").Replace(";", ":").Trim();
                            if (hora.Length == 4)
                            {
                                hora = hora.Substring(0, 2) + ":" + hora.Substring(2);
                            }

                            ///VER_OBES ( OBS, AM_PM, RUTCLIENTE, FECHA_AGENDA, NOMBRE_CONTACTO, CODVENDEDOR)
                            string script = string.Format("javascript:ver_obs(&#39;{0}&#39;,&#39;{1}&#39;,&#39;{2}&#39;,&#39;{3}&#39;,&#39;{4}&#39;, &#39;{5}&#39;, &#39;{6}&#39;);", r[5].ToString().Trim(), r[3].ToString().Trim(), r[0].ToString().Trim(), r[1].ToString().Trim(), r[2].ToString().Trim(), r[4].ToString().Trim(), r[7].ToString().Trim());
                            HTML += "         <td style='white-space: nowrap;'> " + r[2].ToString().Trim().ToUpper() + "</td>";
                            HTML += "         <td style='white-space: nowrap;'> <a target = '_blank' style='cursor: pointer;color: #ff0000;' onclick='" + script + "' > " + hora + "</a></td>";
                            //HTML += "         <td style='white-space: nowrap;'><a target = '_blank' style='cursor: pointer;' onclick='" + script + "' ><i class='fa fa-search' aria-hidden='true'></i></a></td>";

                        }
                        else
                        {
                            HTML += "         <td style='white-space: nowrap;'></td>";
                            HTML += "         <td style='white-space: nowrap;'></td>";
                            //HTML += "         <td style='white-space: nowrap;'></td>";
                        }
                    }
                }
                HTML += "</tr>";
            }
            HTML += "</tr>";
            HTML += "</tbody>";


            //// FOOOOOTERR      ACA  CLIENTES QUE NO HAN SIGO AGENDADO
            HTML += "<tfoot>";

            foreach (DataRow r2 in dt_cliente_vendedor.Rows)
            {
                HTML += "<tr>";
                bool esta_agendado = false;
                foreach (DataRow r3 in dt_ag.Rows)
                {
                    if (r3["rutcliente"].ToString().Trim() == r2["rutcliente"].ToString().Trim())
                    {
                        esta_agendado = true;
                    }
                }
                if (!esta_agendado)
                {
                    //HTML += "<th colspan=1; class='sorter-false' style='background-color: rgb(255, 132, 132) !important; color:white !important;' > " + r2["nombre cliente"].ToString().Trim() + " </th>";

                    ///NOMBRE DEL CLIENTE ---< 
                    /////  agregar_agenda   (   observa, hora, rutcliente, fecha_agenda, nombre_contacto, codvendedor, nombrecliente, nombre_vendedor   )
                    ///   
                    string script1 = string.Format("javascript:agregar_agenda(&#39;{0}&#39;,&#39;{1}&#39;,&#39;{2}&#39;,&#39;{3}&#39;,&#39;{4}&#39;, &#39;{5}&#39;, &#39;{6}&#39;,  &#39;{7}&#39;);",
                                      "", "", r2[1].ToString().Trim(), "", "", r2[4].ToString().Trim(), r2[2].ToString().Trim(), r2[3].ToString().Trim());

                    HTML += "<th colspan=1; class='sorter-false'  style='background-color: rgb(255, 132, 132) !important; color:white !important;'><a target = '_blank' style='cursor: pointer;color: #ffffff;' onclick='" + script1 + "' > " + r2["nombre cliente"].ToString() + "</a></td>";


                    HTML += "<th colspan=" + dias_agenda + "; class='sorter-false' style='border-top: #ffebeb 2px solid; background-color: white !important; color:white !important;' ></th>";
                }
                HTML += "</tr>";
            }

            HTML += "</tfoot>";

            HTML += "  </table>";

            agenda_div.InnerHtml = HTML;

            UpdatePanel44.Update();

        }

        protected void btn_edit_2_Click(object sender, EventArgs e)
        {

            agendacontactoEntity ag = new agendacontactoEntity();
            ag.rutcliente = Session["rutcliente_"].ToString();
            ag.nombre_contacto = Session["nombre_contacto"].ToString();
            ag.codvendedor = Session["codvendedor"].ToString();

            DateTime fecha_antigua = Convert.ToDateTime(Session["fecha_agenda"].ToString());

            ag.fecha_agenda = Convert.ToDateTime(t_dia_edit2.Text);

            ag.observacion = t_obs_edit2.InnerText;
            ag.am_pm = t_time_edit2.Text;

            string ok = agendacontactoBO.actualizar(ag, fecha_antigua);

            if (ok == "OK")
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>agendaEditada();</script>", false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Error al editar agendar! ');</script>", false);
            }
        }

        protected void btn_enviar_Click(object sender, EventArgs e)
        {

            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(tx_correo_calen.Text));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "Calendario Agenda (" + ReporteRNegocio.nombre_vendedor(Session["codvendedor"].ToString()) + " ) ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

            //string correo_vendedor_por_cliente = ReporteRNegocio.trae_corr_vend_por_cliente(rutcliente.Replace("-", "").Replace(".", ""));
            //if (correo_vendedor_por_cliente != "")
            //{
            //    email.CC.Add(correo_vendedor_por_cliente);
            //}

            email.Body += "<div style='text-align:center;     display: block !important;' > ";
            email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div> ";
            email.Body += "</div>";
            email.Body += "<div><img src='http://a58.imgup.net/Sopro4d9d.png' style='    float: right;     width: 90px;'> </div><br><br><br><br><br>";

            email.Body += "<div>" + agenda_div.InnerHtml.Replace("<table ", "<table border=3 ").Replace("color: #ffffff;", "color: #222;") + "</div> <br><br>";

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


            //email.IsBodyHtml = true;
            //email.Priority = MailPriority.Normal;
            //email.BodyEncoding = System.Text.Encoding.UTF8;
            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = "srv-correo-2.soprodi.cl";
            //smtp.Port = 25;
            //smtp.EnableSsl = false;
            //smtp.UseDefaultCredentials = false;
            //smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            //try
            //{


            //    smtp.Send(email);
            //    email.Dispose();
            //    lb_mensaj.Text = "Correo Enviado!";


            //}
            //catch (Exception ex)
            //{
            //    lb_mensaj.Text = "Error al enviar ";
            //}

            /////desde gmail

            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;
            email.BodyEncoding = System.Text.Encoding.UTF8;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;

            smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            smtp.Credentials = new NetworkCredential("informes.soprodi@gmail.com", "galaxia1234");
            try
            {
                smtp.Send(email);
                email.Dispose();
                lb_mensaj.Text = "Correo Enviado!";
            }
            catch (Exception ex)
            {
                lb_mensaj.Text = "Error al enviar ";
            }
        }

        protected void btn_cargar_Click(object sender, EventArgs e)
        {
            DateTime desde = Convert.ToDateTime(txt_desde1.Text);
            DateTime hasta = Convert.ToDateTime(txt_hasta1.Text);

            List<DateTime> dias_semaa = DiasEnSemana(desde, hasta);

            string where = " where codvendedor= '" + Session["codvendedor"].ToString() + "' and CONVERT(DATETIME, fecha_agenda,103) between  " +
                               "  CONVERT(DATETIME, '" + desde.ToShortDateString() + "',103)  and CONVERT(DATETIME, '" + hasta.ToShortDateString() + "',103)           ";

            string html_tabla = crear_calendario(where, dias_semaa);
            agenda_div.InnerHtml = html_tabla;
            string html_excel = formatear_tabla_para_excel(html_tabla);
            R_Excel_1.InnerHtml = html_excel;

            UpdatePanel44.Update();

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "calendario_1", "<script language='javascript'>modal_unidad_4();</script>", false);
        }

        protected void bn_agendar_2_Click(object sender, EventArgs e)
        {
            div_nueva_agenda.Visible = true;
        }

        protected void bt_cancelar_nue_age_Click(object sender, EventArgs e)
        {
            div_nueva_agenda.Visible = false;
        }

        protected void btn_contactos__Click(object sender, EventArgs e)
        {
            Session["rutcliente_"] = t_rutcliente_agenda.Text.Trim();
            string where = " where rutcliente = '" + t_rutcliente_agenda.Text + "'";
            G_CONTACTOS_CAL.DataSource = contactosvendBO.GetAll(where);
            G_CONTACTOS_CAL.DataBind();

            div_agendar_modal.Visible = false;
            enviar_correo_modal.Visible = false;



            ScriptManager.RegisterStartupScript(Page, this.GetType(), "calendario_12", "<script language='javascript'>row_arriba_ordenar_agendar();</script>", false);



            //UpdatePanel4.Update();
            //enviar_correo.Visible = false;
            //div_agendar.Visible = false;


        }

        protected void G_CONTACTOS_CAL_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void G_CONTACTOS_CAL_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            //B_Guardar_modal.Text = "Crear";
            //B_Guardar_modal.Visible = true;
            //btn_nuevo_modal.Visible = false;
            //t_nombre_modal.Text = String.Empty;
            //t_cargo_modal.Text = String.Empty;
            //t_num_modal.Text = String.Empty;
            //t_correo_modal.Text = String.Empty;
            //t_comuna_modal.Text = String.Empty;
            //t_nombre_modal.Enabled = true;
            //t_cargo_modal.Enabled = true;
            //t_num_modal.Enabled = true;
            //t_correo_modal.Enabled = true;
            //t_comuna_modal.Enabled = true;

            if (e.CommandName == "Eliminar")
            {

                string rutcliente = G_CONTACTOS_CAL.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                string nombre_contacto = G_CONTACTOS_CAL.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();
                contactosvendEntity cont = new contactosvendEntity();
                cont.rutcliente = rutcliente;
                cont.nombre_contacto = nombre_contacto;
                string ok = contactosvendBO.eliminar(cont);

                if (ok == "OK")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Contacto Eliminado! ');</script>", false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Error al eliminar Contacto! ');</script>", false);
                }
                btn_contactos__Click(sender, e);

            }

            if (e.CommandName == "correo")
            {
                Session["nombre_contacto"] = G_CONTACTOS_CAL.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();
                t_destino_modal.Text = G_CONTACTOS_CAL.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString();
                t_cc_modal.Text = Session["correo_usuario"].ToString();
                enviar_correo_modal.Visible = true;
                div_agendar_modal.Visible = false;
                JQ_Datatable();
            }

            if (e.CommandName == "agendar")
            {
                //string rutcliente = G_CONTACTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                Session["nombre_contacto"] = G_CONTACTOS_CAL.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();

                enviar_correo_modal.Visible = false;
                div_agendar_modal.Visible = true;
                JQ_Datatable();
            }

            if (e.CommandName == "Editar")
            {
                string rutcliente = G_CONTACTOS_CAL.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                string nombre_contacto = G_CONTACTOS_CAL.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();

                Session["rutcliente_"] = rutcliente;

                contactosvendEntity cont = new contactosvendEntity();
                cont.rutcliente = rutcliente;
                cont.nombre_contacto = nombre_contacto;

                string ok = contactosvendBO.encontrar(ref cont);

                B_Guardar_modal.Text = "Editar";
                B_Guardar_modal.Visible = true;
                btn_nuevo_modal.Visible = false;

                t_nombre_modal.Text = cont.nombre_contacto;
                t_cargo_modal.Text = cont.cargo;
                t_num_modal.Text = cont.numero;
                t_correo_modal.Text = cont.correo;
                t_comuna_modal.Text = cont.direccion;

                t_nombre_modal.Enabled = false;
                t_cargo_modal.Enabled = true;
                t_num_modal.Enabled = true;
                t_correo_modal.Enabled = true;
                t_comuna_modal.Enabled = true;
            }


        }
        protected void btn_excel_calen_Click(object sender, EventArgs e)
        {
            Response.Clear();

            Response.AddHeader("content-disposition", "attachment;filename=Soprodi_AGENDA" + DateTime.Now.ToShortDateString() + ".xls");

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