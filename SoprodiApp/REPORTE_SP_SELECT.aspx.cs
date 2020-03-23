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
using System.Diagnostics;
using System.Data.SqlClient;

//using Microsoft.Office.Interop.Word;

namespace SoprodiApp
{
    public partial class REPORTE_SP_SELECT : System.Web.UI.Page
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


            //this.Form.DefaultButton = this.Button1.UniqueID;

            //Page.RegisterRedirectOnSessionEndScript();
            JQ_Datatable();
            page = this.Page;
            if (!IsPostBack)
            {

                string sw_proviene = Request.QueryString["F"];
              
                if (sw_proviene != null)
                {
                    string grupo_ = Request.QueryString["G"];

                    Session["SW_PERMI"] = "1";

                    string datos = Request.QueryString["F"];

                    Session["SW_FILTRAR_PRODUCTO"] = "SI";
                    Session["codvendedor"] = "";
                    Session["WHERE"] = " where 1=1 ";

                    ///PRODUCTOS DEVUELVE DT ... 
                    //string codigos_documentos = ReporteRNegocio.trae_codigos_sincronizados_por_producto(cod_prod);

                    Session["WHERE"] += " and Convert(datetime,b.fechadespacho,103) = Convert(datetime,'" + datos + "',103) and ISNUMERIC( isnull(d.coddocumento,'no')  ) <> 1 and b.DescEmisor in ("+Base.agrega_comillas(grupo_) +")";
                    //Session["estados_param"] = "'10S', '10', '10P'";

                    ImageClickEventArgs e2x = new ImageClickEventArgs(1, 2);
                    Button1_Click(e, e2x);

                    //Session["SW_FILTRAR_PRODUCTO"] = "NO";

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teadsaeee", "<script>  var elem3 = document.getElementById('div_superior'); elem3.style.display = 'none'; </script>", false);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teadsaeee", "<script>  var elem3 = document.getElementById('div_superior'); elem3.style.display = 'block'; </script>", false);
                    Session["SW_FILTRAR_PRODUCTO"] = "NO";

                }



                //TX_AÑO.Text = DateTime.Now.Year.ToString();
                //CB_TIPO_DOC_GRILLA.SelectedValue = DateTime.Now.Month.ToString();
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "35" || u_ne.Trim() == "44")
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
                    titulo2.HRef = "Menu_Planificador.aspx?s=1";
                    titulo2.InnerText = "Planificador";
                }
                else if (Session["SW_PERMI"].ToString() == "2")
                {
                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=2'>Granos</a>";
                    titulo2.HRef = "Menu_Planificador.aspx?s=2";
                    titulo2.InnerText = "Planificador";

                }


                if (Session["SW_PERMI"].ToString() == "1")
                {

                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=1'>Abarrotes</a>";
                    titulo2.HRef = "reportes.aspx?s=1";
                    titulo2.InnerText = "Abarrotes";
                    titulo3.HRef = "Menu_Planificador.aspx";
                    titulo3.InnerText = "Planificador de Despachos";
                }
                else if (Session["SW_PERMI"].ToString() == "2")
                {
                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=2'>Granos</a>";
                    titulo2.HRef = "reportes.aspx?s=2";
                    titulo2.InnerText = "Granos";
                    titulo3.HRef = "Menu_Planificador.aspx";
                    titulo3.InnerText = "Planificador de Despachos";
                }
                USER = User.Identity.Name.ToString();
                l_usuario_.Text = USER;
                //DateTime t = DateTime.Now.AddDays(-1);
                //DateTime t2 = t;
                //////t = new DateTime(t.Year, t.Month - 6, 1);               
                //txt_desde.Text = t.ToShortDateString();
                //txt_hasta.Text = t2.ToShortDateString().Substring(0, 2);


                DateTime t = DateTime.Now.AddDays(-1);
                DateTime t2 = DateTime.Now.AddDays(-1);
                //////t = new DateTime(t.Year, t.Month - 6, 1);    

                txt_desde.Text = "01" + t.AddMonths(-1).ToShortDateString().Substring(2);
                txt_hasta.Text = t2.AddDays(+5).ToShortDateString();
                //txt_desde.Text = "20/09/2017";
                //txt_hasta.Text = "20/09/2017";
                string es_vendedor = ReporteRNegocio.esvendedor(USER);
                if (es_vendedor == "2")
                {
                    btn_excel.Visible = false;
                    btn_excel2.Visible = false;
                    btn_eliminar_check.Visible = false;
                }
                else
                {
                    btn_eliminar_check.Visible = true; ;
                    btn_excel.Visible = true;
                    btn_excel2.Visible = true;
                }
                ImageClickEventArgs ex = new ImageClickEventArgs(1, 2);
                b_Click(sender, ex);
            }
        }

        ///ACA VERSION 04/06/2019

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);


            MakeAccessible(G_INFORME_TOTAL_VENDEDOR);
            //MakeAccessible(G_ASIGNADOS);
            //MakeAccessible(G_QR);
            //MakeAccessible(G_LISTA);
        }
        public void JQ_Datatable()
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd1Q21mp", "<script language='javascript'>creagrilla();</script>", false);
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

        private void cargar_combo_bodega(DataTable dt, DataView dtv)
        {
            //dt.Rows.Add(new Object[] { -1, "-- Todos --" });
            dtv = dt.DefaultView;
            dtv.Sort = "nom_cliente";
            d_bodega.DataSource = dtv;
            d_bodega.DataTextField = "nom_cliente";
            d_bodega.DataValueField = "rut_cliente";
            //d_vendedor_.SelectedIndex = -1;
            d_bodega.DataBind();
        }


        [WebMethod(EnableSession = true)]
        public static int end_session()
        {
            page.Session.Abandon();
            return 0;
        }

        //public string Rvendedores_todos()
        //{

        //    string vend = "";
        //    foreach (ListItem item in d_vendedor_.Items)
        //    {
        //        if (item.Value != "-1")
        //        {
        //            if (vend == "")
        //            {
        //                vend += item.Value + ",";
        //            }
        //            else
        //            {
        //                vend += item.Value + ",";
        //            }
        //        }
        //    }
        //    string ultimo = vend.Substring(veprodnd.Length - 1, 1);
        //    if (ultimo == ",")
        //    {
        //        vend = vend.Substring(0, vend.Length - 1);
        //    }
        //    return vend;
        //}
        //private string Rclientes_todos()
        //{
        //    //string vend = "";
        //    //foreach (ListItem item in this.d_cliente.Items)
        //    //{
        //    //    if (item.Value != "-1")
        //    //    {
        //    //        if (vend == "")
        //    //        {
        //    //            vend += item.Value + ",";
        //    //        }
        //    //        else
        //    //        {
        //    //            vend += item.Value + ",";
        //    //        }
        //    //    }
        //    //}
        //    //string ultimo = vend.Substring(vend.Length - 1, 1);
        //    //if (ultimo == ",")
        //    //{
        //    //    vend = vend.Substring(0, vend.Length - 1);
        //    //}
        //    //return vend;
        //}

        public class PRODUCTO
        {
            public string COD_PROD { get; set; }
            public string NOM_PROD { get; set; }

        }

        [WebMethod]
        public static List<PRODUCTO> PRODUCTO_CAMBIO(string sw)
        {
            DataTable dt = new DataTable();


            string clase = "";
            if (sw == "humano")
            {

                clase = "  b.glclassid in ('ABAR', 'MANI') and b.invtid > '1000' and b.invtid <> '9918' ";
            }
            else
            {
                clase = "  b.glclassid <> 'ABAR' and b.glclassid <>  'MANI' and b.invtid <> '9905'  and b.invtid <> '9999'  and b.invtid <> '9907'   ";

            }


            try
            {
                dt = ReporteRNegocio.listar_ALL_productos_stock_guion_2("", clase);
                ////dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
                DataView dv = dt.DefaultView;
                dv.Sort = "descr";
                dt = dv.ToTable();
            }
            catch { }
            //}
            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new PRODUCTO
                        {
                            COD_PROD = Convert.ToString(item["invtid"]),
                            NOM_PROD = Convert.ToString(item["descr"])
                        };
            return query.ToList<PRODUCTO>();
        }



        [WebMethod]
        public static List<PRODUCTO> PRODUCTO_CAMBIO2(string sw)
        {
            DataTable dt = new DataTable();


            string clase = "";
            if (sw == "humano")
            {

                clase = "  b.glclassid in ('ABAR', 'MANI') and b.invtid > '1000' and b.invtid <> '9918' ";
            }
            else
            {
                clase = "  b.glclassid <> 'ABAR' and b.glclassid <>  'MANI' and b.invtid <> '9905'  and b.invtid <> '9999'  and b.invtid <> '9907'   ";

            }


            try
            {
                dt = ReporteRNegocio.listar_ALL_productos_stock_guion_2(" and b.stkunit <>'KGR' ", clase);
                ////dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
                DataView dv = dt.DefaultView;
                dv.Sort = "descr";
                dt = dv.ToTable();
            }
            catch { }
            //}
            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new PRODUCTO
                        {
                            COD_PROD = Convert.ToString(item["invtid"]),
                            NOM_PROD = Convert.ToString(item["descr"])
                        };
            return query.ToList<PRODUCTO>();
        }


        public class INSERT_SOLO_KG
        {
            public string RESTP { get; set; }

        }

        [WebMethod]
        public static string guardar_solo_kg(string sw, string tipo, string valor)
        {

            DataTable dt = new DataTable();
            string respt = "";
            try
            {

                string oka = ReporteRNegocio.guardar_valor_equivale(
               sw,
                tipo,
                valor
                );

                if (oka == "OK")
                {
                    respt = "ok";
                }
                else
                {
                    respt = "error";
                }


            }
            catch { }
            //}

            return respt;
        }



        public class RESPUESTA_UNIDAD
        {
            public string stkunit { get; set; }
            public string valor { get; set; }
        }

        [WebMethod]
        public static List<RESPUESTA_UNIDAD> carga_click(string sw)
        {



            DataTable dt = new DataTable();

            try
            {

                dt = ReporteRNegocio.valor_prod_equivale(sw);


            }
            catch { }
            //}
            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new RESPUESTA_UNIDAD
                        {
                            stkunit = Convert.ToString(item["stkunit"]),
                            valor = Convert.ToString(item["valor"])
                        };
            return query.ToList<RESPUESTA_UNIDAD>();
        }


        //[WebMethod]
        //public static List<VENDEDOR> VENDEDOR_POR_GRUPO(string grupos, string desde, string hasta, string usuario)
        //{
        //    DataTable dt = new DataTable();
        //    string grupos_ = agregra_comillas2(grupos);


        //    string es_vendedor = ReporteRNegocio.esvendedor(usuario);


        //    string where = " where FechaFactura >= CONVERT(datetime,'" + desde + "', 103) " +
        //                     " and FechaFactura <= CONVERT(datetime,'" + hasta + "',103) ";



        //    if (es_vendedor == "2")
        //    {
        //        where += " and codvendedor in ('" + usuario + "')";
        //    }

        //    if (!grupos_.Contains("'-1'") && grupos_ != "" && !grupos_.Contains("'-- Todos --'"))
        //    {
        //        where += " and user1 in (" + grupos_ + ")";
        //    }
        //    if (grupos_.Contains("'-- Todos --'") && es_vendedor == "2")
        //    {
        //        where += " and user1 in (" + agregra_comillas2(ReporteRNegocio.grupos_usuario_v_report(USER)) + ")";
        //    }
        //    else if (grupos_.Contains("'-- Todos --'"))
        //    {
        //        where += " and user1 in (" + agregra_comillas2(ReporteRNegocio.grupos_usuario(USER)) + ")";
        //    }
        //    if (grupos == "")
        //    {
        //        where += " and 1=1 ";
        //    }

        //    try
        //    {
        //        dt = ReporteRNegocio.listar_ALL_vendedores(where);
        //        ////dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
        //        DataView dv = dt.DefaultView;
        //        dv.Sort = "nom_vend";
        //        dt = dv.ToTable();
        //    }
        //    catch { }


        //    var query = from item in dt.AsEnumerable()
        //                where 1 == 1
        //                select new VENDEDOR
        //                {
        //                    cod_vendedor = Convert.ToString(item["cod_vend"]),
        //                    nom_vendedor = Convert.ToString(item["nom_vend"])
        //                };
        //    return query.ToList<VENDEDOR>();
        //}

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
        double sub_peso = 0;
        double sub_dolar = 0;

        double cost_excel = 0;
        double cost_impot = 0;
        double cost_compra = 0;

        double utilidad_exce = 0;
        double utilidad_compra = 0;


        double total_rows = 0;
        double cont_row = 0;

        int GetColumnIndexByName(GridViewRow row, string columnName)
        {
            int columnIndex = 0;
            foreach (DataControlFieldCell cell in row.Cells)
            {
                if (cell.ContainingField is BoundField)
                    if (((BoundField)cell.ContainingField).DataField.Equals(columnName))
                        break;
                columnIndex++; // keep adding 1 while we don't have the correct name
            }
            return columnIndex;
        }

        protected void G_INFORME_TOTAL_VENDEDOR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //[2] -CodDocumento
                //[3] -NombreCliente
                //[4] -NombreVendedor
                //[5] -MontoNeto
                //[6] -DescBodega
                //[7] -FechaDespacho
                //[8] -DifDias
                //[9] -FechaEmision
                //[10] -CodVendedor
                //[11] -NotaLibre
                //[12]    -CodBodega
                //[13]    -CodMoneda    
                //[14]    -DescEstadoDocumento
                //[15]    -Facturas
                //[16]    -GxEstadoSync
                //[17]    -GxActualizado
                //[18]    -GxEnviadoERP
                //[19]    -FechaCreacion  
                //[20]    -ValorTipoCambio
                //[21]    -LimiteSeguro 
                //[22]    -TipoCredito  
                //[23]    -CreditoDisponible 
                //[24]    -CreditoAutorizado
                //[25]    -EmailVendedor
                //[26]    -ESTADO
                //[27]    -CodProducto
                //[28]    -Cantidad
                //[29]    -AprobadoFull       
                //[30]    -Asignada
                //[31]    -EstadoParcial          
                //[32]    -fPLAN

                int id_26 = GetColumnIndexByName(e.Row, "ESTADO");
                int id_31 = GetColumnIndexByName(e.Row, "EstadoParcial");
                int id_5 = GetColumnIndexByName(e.Row, "MontoNeto");
                int id_2 = GetColumnIndexByName(e.Row, "CodDocumento");
                int id_7 = GetColumnIndexByName(e.Row, "FechaDespacho");
                int id_8 = GetColumnIndexByName(e.Row, "DifDias");
                int id_11 = GetColumnIndexByName(e.Row, "NotaLibre");
                int id_14 = GetColumnIndexByName(e.Row, "DescEstadoDocumento");
                int id_15 = GetColumnIndexByName(e.Row, "Facturas");

                int id_27 = GetColumnIndexByName(e.Row, "CodProducto");
                int id_28 = GetColumnIndexByName(e.Row, "Cantidad");
                int id_29 = GetColumnIndexByName(e.Row, "AprobadoFull");
                int id_30 = GetColumnIndexByName(e.Row, "Asignada");
              
                //if (e.Row.Cells[30].Text != "no")
                if (G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values["Asignada"].ToString() != "no")
                {
                    e.Row.BackColor = Color.FromArgb(255, 228, 196);
                }

                string valor_ = G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values["fPLAN"].ToString();

                ////Session["estados_planif"] 
                //if (Session["estados_planif"].ToString().Contains((e.Row.Cells[30].Text)) || Session["estados_planif"].ToString() == "")
                //{
           

                e.Row.Cells[id_26].Text = e.Row.Cells[id_26].Text.Trim();
                if (Session["estados"].ToString().Contains((e.Row.Cells[id_26].Text.Trim())) || Session["estados"].ToString() == "")
                {
                    if (e.Row.Cells[id_26].Text == "20")
                    {
                        e.Row.Attributes["class"] = "estado20";
                        if (e.Row.Cells[id_31].Text == "&nbsp;")
                        {
                            e.Row.Cells[id_31].Text = "pendiente";
                        }
                    }
                    else if (e.Row.Cells[id_26].Text == "10P")
                    {
                        e.Row.Attributes["class"] = "estado10P";
                        if (e.Row.Cells[id_31].Text == "&nbsp;")
                        {
                            e.Row.Cells[id_31].Text = "pendiente";
                        }
                    }
                    else if (e.Row.Cells[id_26].Text == "10S")
                    {
                        e.Row.Attributes["class"] = "estado10S";
                        if (e.Row.Cells[id_31].Text == "&nbsp;")
                        {
                            e.Row.Cells[id_31].Text = "pendiente";
                        }
                    }
                    else if (e.Row.Cells[id_26].Text == "40")
                    {
                        e.Row.Attributes["class"] = "estado40";
                        if (e.Row.Cells[id_31].Text == "&nbsp;")
                        {
                            e.Row.Cells[id_31].Text = "pendiente";
                        }
                    }
                    else if (e.Row.Cells[id_26].Text == "30")
                    {
                        e.Row.Attributes["class"] = "estado30";
                        if (e.Row.Cells[id_31].Text == "&nbsp;")
                        {
                            e.Row.Cells[id_31].Text = "pendiente";
                        }
                    }
                    else if (e.Row.Cells[id_26].Text == "GxDef")
                    {
                        e.Row.Attributes["class"] = "estadoGFX";
                        if (e.Row.Cells[id_31].Text == "&nbsp;")
                        {
                            e.Row.Cells[id_31].Text = "pendiente";
                        }
                    }
                    else if (e.Row.Cells[id_26].Text == "10")
                    {
                        e.Row.Attributes["class"] = "estado10";
                        if (e.Row.Cells[id_31].Text == "&nbsp;")
                        {
                            e.Row.Cells[id_31].Text = "pendiente";
                        }
                    }

                    if (Session["estados_p_d"].ToString().Contains((e.Row.Cells[id_31].Text.Trim())) || Session["estados_p_d"].ToString() == "")
                    {

                        //20
                        //10P
                        //10S
                        //40
                        //30
                        //GxDef
                        //10
             
                        if (e.Row.Cells[id_31].Text == "&nbsp;")
                        {
                            string combo = "";
                            combo = " <select class=\"form-control input-sm\" style=\"width: 100px;\" id=\"cb_cambio_pago" + e.Row.Cells[id_2].Text + "\" onchange =\"cambia_tipo_pago3('" + e.Row.Cells[id_2].Text + "')\"> " +
                                               "                                        <option value = \"\"></option>  " +
                                                       "                              <option value = \"pendiente\" selected> PENDIENTE...</option> " +
                                                        "                              <option value=\"cerrado\"> CERRADO...</option> " +
                                                          "                         </select > ";
                            e.Row.Cells[id_31].Text = combo;
                        }

                        if (e.Row.Cells[id_31].Text == "pendiente")
                        {
                            string combo = "";
                            combo = " <select class=\"form-control input-sm\" style=\"width: 100px;\" id=\"cb_cambio_pago" + e.Row.Cells[id_2].Text + "\" onchange =\"cambia_tipo_pago3('" + e.Row.Cells[id_2].Text + "')\"> " +
                                               "                                        <option value = \"\"></option>  " +
                                                       "                              <option value = \"pendiente\" selected> PENDIENTE...</option> " +
                                                        "                              <option value=\"cerrado\"> CERRADO...</option> " +
                                                          "                         </select > ";
                            e.Row.Cells[id_31].Text = combo;
                        }

                        if (e.Row.Cells[id_31].Text == "despachado")
                        {
                            string combo = "";
                            combo = " <select class=\"form-control input-sm\"  style=\"width: 100px;\" id=\"cb_cambio_pago" + e.Row.Cells[id_2].Text + "\" onchange =\"cambia_tipo_pago3('" + e.Row.Cells[id_2].Text + "')\"> " +
                                              "                                        <option value = \"\"></option>  " +
                                                      "                               <option value = \"pendiente\"> PENDIENTE...</option> " +
                                                        "                              <option value=\"cerrado\" selected> CERRADO...</option> " +
                                                         "                         </select > ";

                            e.Row.Cells[id_31].Text = combo;
                        }

                        clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                        if (e.Row.Cells[id_8].Text.Contains("-"))
                        {


                            if (e.Row.Cells[id_14].Text == "Sincronizado" || ((e.Row.Cells[id_14].Text == "Aprobado" && e.Row.Cells[id_15].Text == "&nbsp;")))
                            {
                                e.Row.Cells[id_8].Style.Value = "color:red";
                                e.Row.Cells[id_7].Style.Value = "color:red";
                            }
                        }

                        string script1 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[id_2].Text), encriptador.EncryptData(""), encriptador.EncryptData(""), encriptador.EncryptData("57"));
                        e.Row.Cells[id_2].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[id_2].Text + " </a>";

                        if (e.Row.Cells[id_14].Text != "Sincronizado" && !((e.Row.Cells[id_14].Text == "Aprobado" && e.Row.Cells[id_15].Text == "&nbsp;")))
                        {
                            e.Row.Cells[0].Text = "";
                        }

                        e.Row.Cells[id_15].Text = "<a href='REPORTE_LISTADO_DOC.aspx?F=" + e.Row.Cells[id_15].Text + "' target='_blank'>" + e.Row.Cells[id_15].Text + " </a> ";

                        e.Row.Cells[id_5].Text = Base.monto_format(e.Row.Cells[id_5].Text.Replace(",000", ""));
                        //

                        string textoOriginal = e.Row.Cells[id_11].Text;//transformación UNICODE
                        string textoNormalizado = textoOriginal.Normalize(NormalizationForm.FormD);
                        //coincide todo lo que no sean letras y números ascii o espacio

                        e.Row.Cells[id_11].ToolTip = System.Net.WebUtility.HtmlDecode(textoNormalizado);

                        try
                        {
                            e.Row.Cells[id_11].Text = e.Row.Cells[id_11].Text.Substring(0, 15);
                            //
                        }
                        catch
                        { }

                        if (Session["codvendedor"].ToString() != "")
                        {
                            e.Row.Cells[0].Visible = false;
                            G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[0].Visible = false;

                            e.Row.Cells[1].Visible = false;
                            G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[1].Visible = false;

                            e.Row.Cells[id_31].Visible = false;
                            G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[id_31].Visible = false;
                        }

                        e.Row.Cells[id_27].Visible = false;
                        G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[id_27].Visible = false;

                        e.Row.Cells[id_28].Visible = false;
                        G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[id_28].Visible = false;

                        e.Row.Cells[id_29].Visible = false;
                        G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[id_29].Visible = false;

                        e.Row.Cells[id_30].Visible = false;
                        G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[id_30].Visible = false;

                        if (e.Row.Cells[id_14].Text != "Aprobado")
                        {
                            e.Row.Cells[1].Text = "";
                            //G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[1].Visible = false;
                        }

                    }
                    else
                    {
                        e.Row.Visible = false;
                    }
                    //}
                    //else
                    //{
                    //    e.Row.Visible = false;
                    //}
                }
                else
                {
                    e.Row.Visible = false;
                }
            }
        }

        [WebMethod]
        public static string cambia_tipo_pago_(string factura, string estado)
        {
            string aasdf = "";

            if (estado != "pendiente")
            {
                ReporteRNegocio.delete_estado_sp(factura);
                ReporteRNegocio.insert_estado_sp(factura, estado);
            }



            return "";
        }

        private string monto_miles(string p)
        {
            bool es_dolar = false;
            double d;
            try
            {
                double.TryParse(p.Substring(0, p.IndexOf(",")), out d);
                es_dolar = true;
            }
            catch
            {
                double.TryParse(p, out d);
            }
            string aux = "";
            if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
            if (es_dolar) { return aux + "," + p.Substring(p.IndexOf(",")); } else { return aux; }
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


        protected void G_PRODUCTOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


            }
        }

        protected void btn_excel_Click(object sender, EventArgs e)
        {
            Response.Clear();

            Response.AddHeader("content-disposition", "attachment;filename=SOPRODI_ELEGIR_SP" + DateTime.Now.ToShortDateString() + ".xls");

            Response.Charset = "";

            // If you want the option to open the Excel file without saving than

            // comment out the line below

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.ContentType = "application/vnd.xls";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);

            r_excel.RenderControl(htmlWrite);
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
            string es_vend = ReporteRNegocio.esvendedor(User.Identity.Name.ToString());
            if (es_vend != "2")
            {
                Session["codvendedor"] = "";
            }
            else
            {
                Session["codvendedor"] = " and codvendedor = '" + User.Identity.Name.ToString() + "'";
            }
            cargar_clientes_SP();
            CargarBodega();
            cargar_estado_SP();
            //cargar_grupo_sp();
            cargar_vendedor_SP();
            carga_estado_2();
            carga_estado_3();

            lb_bodegas2.Text = "10, 10S, 10P";
            lb_bodegas3.Text = "pendiente";

            foreach (ListItem item in d_bodega_2.Items)
            {

                if (lb_bodegas2.Text.Contains(item.Value.ToString()))
                {
                    item.Selected = true;
                }
            }
            foreach (ListItem item in d_estado_2.Items)
            {

                if (lb_bodegas3.Text.Contains(item.Value.ToString()))
                {
                    item.Selected = true;
                }
            }
        }

        private void CargarBodega()
        {
            string where = " where 1=1 ";

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.carga_bodega(where);
            dtv = dt.DefaultView;
            CB_BODEGA.DataSource = dtv;
            CB_BODEGA.DataTextField = "nom_bodega";
            CB_BODEGA.DataValueField = "nom_bodega";
            CB_BODEGA.DataBind();
        }

        private void carga_estado_2()
        {
            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.crear_estado_2();
            dtv = dt.DefaultView;
            d_estado_2.DataSource = dtv;
            d_estado_2.DataTextField = "estado";
            d_estado_2.DataValueField = "estado";
            //d_vendedor_.SelectedIndex = -1;
            d_estado_2.DataBind();
        }
        private void carga_estado_3()
        {
            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.crear_estado_3();
            dtv = dt.DefaultView;
            d_estado_3.DataSource = dtv;
            d_estado_3.DataTextField = "estado";
            d_estado_3.DataValueField = "estado";
            //d_vendedor_.SelectedIndex = -1;
            d_estado_3.DataBind();
        }
        private void cargar_vendedor_SP()
        {
            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.VM_listaVendedor(Session["codvendedor"].ToString(), "");
            dtv = dt.DefaultView;
            d_vendedor.DataSource = dtv;
            d_vendedor.DataTextField = "nombrevendedor";
            d_vendedor.DataValueField = "codvendedor";
            //d_vendedor_.SelectedIndex = -1;
            d_vendedor.DataBind();
        }
        private void cargar_grupo_sp()
        {
            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.VM_lista_grupo("");
            dtv = dt.DefaultView;
            d_grupo.DataSource = dtv;
            d_grupo.DataTextField = "DescEmisor";
            d_grupo.DataValueField = "DescEmisor";
            //d_vendedor_.SelectedIndex = -1;
            d_grupo.DataBind();
        }
        private void cargar_productos_no_kg(string clase)
        {
            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.listar_ALL_productos_stock_guion_2(" and b.stkunit <>'KGR' ", clase);
            dtv = dt.DefaultView;
            cb_productos_kg.DataSource = dtv;
            cb_productos_kg.DataTextField = "descr";
            cb_productos_kg.DataValueField = "invtid";
            //d_vendedor_.SelectedIndex = -1;
            cb_productos_kg.DataBind();
        }
        private void cargar_estado_SP()
        {
            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.VM_estados("");
            dtv = dt.DefaultView;
            d_bodega_2.DataSource = dtv;
            d_bodega_2.DataTextField = "descestadodocumento";
            d_bodega_2.DataValueField = "codestadodocumento";
            //d_vendedor_.SelectedIndex = -1;
            d_bodega_2.DataBind();
        }
        private void cargar_clientes_SP()
        {
            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.VM_clientes(" where 1=1 " + Session["codvendedor"], "");
            dtv = dt.DefaultView;
            d_bodega.DataSource = dtv;
            d_bodega.DataTextField = "nombre";
            d_bodega.DataValueField = "rut";
            //d_vendedor_.SelectedIndex = -1;
            d_bodega.DataBind();
        }

        private void cargar_combo_clientes(DataTable dataTable, DataView dtv)
        {
            ////dt.Rows.Add(new Object[] { -1, "-- Todos --" });
            //dtv = dataTable.DefaultView;
            //dtv.Sort = "nom_cliente";
            //d_cliente.DataSource = dtv;
            //d_cliente.DataTextField = "nom_cliente";
            //d_cliente.DataValueField = "rut_cliente";
            ////d_vendedor_.SelectedIndex = -1;
            //d_cliente.DataBind();
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
        GridView excel1 = new GridView();
        protected void Button1_Click(object sender, EventArgs e)
        {


            //REPORTE
            //string produc = agregra_comillas(l_vendedores.Text);
            string desde = txt_desde.Text;
            string hasta = txt_hasta.Text;

            string clientes = agregra_comillas(l_clientes.Text);
            string estados = agregra_comillas(lb_bodegas2.Text);
            string estados_p_d = agregra_comillas(lb_bodegas3.Text);
            string estados_planif = agregra_comillas(lb_planificado.Text);
            string bodega = agregra_comillas(l_bodega.Text);

            string grupo = agregra_comillas(l_grupo_vm.Text);
            string vendedor = agregra_comillas(l_vendedor_vm.Text);

            string where3 = " where 1=1 ";

            if (Session["SW_PERMI"].ToString() == "2")
            {
                where3 += " and b.DescEmisor = 'Granos'";
            }
            else
            {
                where3 += " and b.DescEmisor <> 'Granos'";
            }

            //select * from[VPEDIDOCABECERA] where FechaEmision >= CONVERT(datetime, '21/07/2017', 103)
            if (desde != "")
            {
                if (rd_em.Checked)
                {
                    where3 += " and convert(datetime,b.FechaEmision ,103)  >= convert(datetime, '" + desde + "', 103) ";
                }
                else
                {

                    where3 += " and convert(datetime,b.FechaDespacho ,103)  >= convert(datetime, '" + desde + "', 103) ";
                }

            }

            if (hasta != "")
            {
                if (rd_em.Checked)
                {
                    where3 += " and convert(datetime,b.FechaEmision ,103)   <= convert(datetime, '" + hasta + "', 103) ";
                }
                else
                {

                    where3 += " and convert(datetime,b.FechaDespacho ,103)   <= convert(datetime, '" + hasta + "', 103) ";


                }
            }

            if (clientes != "")
            {

                where3 += " and b.rut in (" + clientes + ")";

            }

            if (bodega != "")
            {
                where3 += " and b.CodBodega in (" + bodega + ")";
            }

            string aux_estados = estados;

            Session["estados"] = estados;


            Session["estados_p_d"] = estados_p_d;
            Session["estados_planif"] = estados_planif.Replace("planificado", "si").Replace("pendiente", "no");

            if (estados.Contains("10S"))
            {

                aux_estados = estados.Replace("10S", "20");

            }

            if (estados.Contains("10P"))
            {

                aux_estados = estados.Replace("10P", "20");

            }

            if (estados != "")
            {

                where3 += " and b.CodEstadoDocumento in (" + aux_estados + ")";

            }


            if (vendedor != "")
            {

                where3 += " and b.codvendedor in (" + vendedor + ")";

            }
            if (Session["codvendedor"].ToString() != "")
            {

                where3 += Session["codvendedor"].ToString();

            }


            if (txt_sp.Text != "")
            {

                where3 += " and b.CodDocumento in (" + agregra_comillas(txt_sp.Text) + ")";

            }

            string estado = estados_planif.Replace("planificado", "si").Replace("pendiente", "no").Replace("'", "");

            if (estado != "")
            {
                if (estado.Trim() == "si")
                {
                    where3 += " and ISNUMERIC( isnull(d.coddocumento,'no')  ) = 1 ";
                }
                else if (estado.Trim() == "no")
                {

                    where3 += " and ISNUMERIC( isnull(d.coddocumento,'no')  ) <> 1 ";

                }
            }



            div_report.Visible = true;

            if (Session["SW_FILTRAR_PRODUCTO"].ToString() == "SI")
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teadsaeee", "<script>  var elem3 = document.getElementById('div_superior'); elem3.style.display = 'none'; </script>", false);

                where3 = Session["WHERE"].ToString();
                //Session["estados"] = Session["estados_param"].ToString();
                //Session["WHERE"] = "";
            }
            where3 += " and a.codproducto not in (select distinct(codproducto)  from VPEDIDODETALLE_THX where ISNUMERIC(CodProducto) <> 1) order by convert(datetime, FechaDespacho,103) desc";

            //SPSP
            DataTable dt2 = ReporteRNegocio.VM_listar_sp_select(where3);
            //[0] -CodDocumento
            //[1] -NombreCliente
            //[2] -NombreVendedor
            //[3] -MontoNeto
            //[4] -DescBodega
            //[5] -FechaDespacho
            //[6] -DifDias
            //[7] -FechaEmision
            //[8] -CodVendedor
            //[9] -NotaLibre
            //[10]    -CodBodega
            //[11]    -CodMoneda    
            //[12]    -DescEstadoDocumento
            //[13]    -Facturas
            //[14]    -GxEstadoSync
            //[15]    -GxActualizado
            //[16]    -GxEnviadoERP
            //[17]    -FechaCreacion  
            //[18]    -ValorTipoCambio
            //[19]    -LimiteSeguro 
            //[20]    -TipoCredito  
            //[21]    -CreditoDisponible 
            //[22]    -CreditoAutorizado
            //[23]    -EmailVendedor
            //[24]    -ESTADO
            //[25]    -CodProducto
            //[26]    -Cantidad
            //[27]    -AprobadoFull       
            //[28]    -Asignada
            //[29]    -EstadoParcial          
            //[30]    -fPLAN
            string cod_aux = "";
            string estado_aux = "";
            string facturas_aux = "";

            DataTable sp_malas = new DataTable();
            sp_malas.Columns.Add("sp");
            sp_malas.Columns.Add("facturas");
            sp_malas.Columns.Add("estado");


            DataTable sp_for = new DataTable();
            sp_for.Columns.Add("sp");
            sp_for.Columns.Add("estado");
            sp_for.Columns.Add("facturas");
            string facturas_x_sps = "";
            foreach (DataRow r in dt2.Rows)
            {
                //20 -- OK
                //10S -- SinFactura
                //10P -- Cantidad Distintas      

                //0  coddoc //24 cod //25 cant

                if (r["DescEstadoDocumento"].ToString() == "Aprobado" && r["AprobadoFull"].ToString() == "no")
                {
                    if (cod_aux == "")
                    {
                        cod_aux = r["CodDocumento"].ToString();
                        sp_for = new DataTable();
                        sp_for.Columns.Add("sp");
                        sp_for.Columns.Add("estado");
                        sp_for.Columns.Add("facturas");
                    }

                    DataTable procesado = ReporteRNegocio.SP_Marcelo(r["CodDocumento"].ToString().Trim(), r["CodProducto"].ToString().Trim(), r["Cantidad"].ToString().Trim());
                    DataRow row = procesado.Rows[0];

                    if (r["CodDocumento"].ToString() == cod_aux)
                    {
                        //if (row["estado"].ToString().Substring(0, 3) == "10P")
                        //{

                        facturas_aux = row["facturas"].ToString();

                        if (facturas_aux.Trim() != "")
                        {
                            facturas_x_sps += row["facturas"].ToString() + ", ";
                            r["Facturas"] = facturas_x_sps.Substring(0, facturas_x_sps.Length - 3);
                        }

                        DataRow row_sp1 = sp_for.NewRow();
                        row_sp1["sp"] = cod_aux;
                        row_sp1["estado"] = row["estado"].ToString().Substring(0, 3);
                        string aux_aca = "";
                        try
                        {
                            aux_aca = facturas_x_sps.Substring(0, facturas_x_sps.Length - 2);
                        }
                        catch
                        {


                        }

                        row_sp1["facturas"] = aux_aca;
                        sp_for.Rows.Add(row_sp1);


                    }
                    else
                    {
                        string estado_univ = "";
                        foreach (DataRow r2 in sp_for.Rows)
                        {
                            if (r2[1].ToString() == "10P")
                            {
                                estado_univ = r2[1].ToString();
                                DataRow row_sp = sp_malas.NewRow();
                                row_sp["sp"] = cod_aux;
                                row_sp["facturas"] = facturas_x_sps.Substring(0, facturas_x_sps.Length - 3);
                                row_sp["estado"] = estado_univ;
                                sp_malas.Rows.Add(row_sp);
                                break;
                            }
                            else
                            {
                                estado_univ = r2[1].ToString();
                                DataRow row_sp = sp_malas.NewRow();
                                row_sp["sp"] = cod_aux;

                                string aux_fac = "";
                                try
                                {
                                    aux_fac = facturas_x_sps.Substring(0, facturas_x_sps.Length - 2);


                                }
                                catch
                                {


                                }
                                row_sp["facturas"] = aux_fac;
                                row_sp["estado"] = estado_univ;
                                sp_malas.Rows.Add(row_sp);

                            }
                        }

                        cod_aux = r["CodDocumento"].ToString();
                        sp_for = new DataTable();
                        sp_for.Columns.Add("sp");
                        sp_for.Columns.Add("estado");
                        sp_for.Columns.Add("facturas");
                        facturas_x_sps = "";

                        facturas_x_sps += row["facturas"].ToString() + ", ";
                        r["Facturas"] = facturas_x_sps.Substring(0, facturas_x_sps.Length - 2);

                        DataRow row_sp1 = sp_for.NewRow();
                        row_sp1["sp"] = cod_aux;
                        row_sp1["estado"] = row["estado"].ToString().Substring(0, 3);
                        row_sp1["facturas"] = facturas_x_sps.Substring(0, facturas_x_sps.Length - 2);
                        sp_for.Rows.Add(row_sp1);


                    }



                }
                else if (r["DescEstadoDocumento"].ToString() == "Aprobado")
                {


                    DataTable procesado = ReporteRNegocio.SP_Marcelo(r["CodDocumento"].ToString().Trim(), r["CodProducto"].ToString().Trim(), r["Cantidad"].ToString().Trim());
                    DataRow row = procesado.Rows[0];
                    r["Facturas"] = row["facturas"].ToString();




                }
            }

            string estado_univ1 = "";
            foreach (DataRow r2 in sp_for.Rows)
            {
                if (r2[1].ToString() == "10P")
                {
                    estado_univ1 = r2[1].ToString();
                    DataRow row_sp = sp_malas.NewRow();
                    row_sp["sp"] = cod_aux;
                    row_sp["facturas"] = r2[2].ToString();
                    row_sp["estado"] = estado_univ1;
                    sp_malas.Rows.Add(row_sp);
                    break;
                }
                else
                {
                    estado_univ1 = r2[1].ToString();
                    DataRow row_sp = sp_malas.NewRow();
                    row_sp["sp"] = cod_aux;
                    row_sp["facturas"] = r2[2].ToString();
                    row_sp["estado"] = estado_univ1;
                    sp_malas.Rows.Add(row_sp);

                }
            }


            foreach (DataRow r in dt2.Rows)
            {
                if (r["DescEstadoDocumento"].ToString() == "Aprobado")
                {
                    foreach (DataRow r2 in sp_malas.Rows)
                    {
                        if (r["CodDocumento"].ToString() == r2[0].ToString())
                        {
                            string cad = r2[1].ToString();
                            Dictionary<string, int> contador = new Dictionary<string, int>();

                            foreach (string item in cad.Split(new char[] { ',' }))
                            {
                                if (contador.ContainsKey(item.Trim()))
                                {
                                    contador[item.Trim()] = contador[item.Trim()] + 1;

                                }
                                else
                                {
                                    contador.Add(item.Trim(), 1);
                                }
                            }

                            string resultado = "";
                            foreach (KeyValuePair<string, int> item in contador)
                            {
                                if (item.Value >= 1)
                                {
                                    resultado = string.Format("{0},{1}", resultado, item.Key);
                                }
                            }
                            string cadd = resultado.Remove(0, 1);

                            if (r2[2].ToString() == "10S" && cadd.Trim() != "")
                            {

                                r["Facturas"] = cadd;
                                r["ESTADO"] = "10P";

                            }
                            else
                            {

                                r["Facturas"] = cadd;
                                r["ESTADO"] = r2[2].ToString();
                            }


                        }
                    }
                }
                r["Facturas"] = r["Facturas"].ToString().Replace(",", ", ");
            }

            if (dt2.Rows.Count > 0)
            {

                dt2 = dt2.AsEnumerable()
                       .GroupBy(r => new { Col1 = r["CodDocumento"] })
                       .Select(g => g.OrderBy(r => r["CodDocumento"]).First())
                       .CopyToDataTable();
            }



            sub_dolar = 0;


            //excel1.DataSource = dt2;



            //excel1.DataBind();
            //excel1.Columns[0].Visible = false;

            G_INFORME_TOTAL_VENDEDOR.DataSource = dt2;
            G_INFORME_TOTAL_VENDEDOR.DataBind();
            JQ_Datatable();



            System.IO.StringWriter stringWrite1 = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite1 =
            new HtmlTextWriter(stringWrite1);

            G_INFORME_TOTAL_VENDEDOR.RenderControl(htmlWrite1);

            string truco_salto = string.Format("<td>{0}                                                                            <input type=\"image\"", Environment.NewLine); ;
            string truco_salto2 = string.Format("<td  style=\"visibility:hidden;\">{0}                                                                            <input type=\"hidden\"", Environment.NewLine); ;

            string html = stringWrite1.ToString().Replace("<th scope=\"col\">Planificar", "<th scope=\"col\" style=\"visibility:hidden;\">Planificar").Replace(truco_salto, truco_salto2);

            r_excel.InnerHtml = html;


        }

        private bool IsNumeric(string v)
        {
            float output;
            return float.TryParse(v, out output);
        }

        protected void carga_d_Click(object sender, EventArgs e)
        {


        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
        }

        protected void G_errores_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void BTN_error_Click(object sender, EventArgs e)
        {

        }

        protected void excel_error_Click(object sender, EventArgs e)
        {
            Response.Clear();

            Response.AddHeader("content-disposition", "attachment;filename=FECHAS_VP_INTRAN" + DateTime.Now.ToShortDateString() + ".xls");

            Response.Charset = "";

            // If you want the option to open the Excel file without saving than

            // comment out the line below

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.ContentType = "application/vnd.xls";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);

            G_errores.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());

            Response.End();

        }

        protected void btn_estimado_Click(object sender, EventArgs e)
        {
            string ponbr = "";
            string invtid = "";
            string ok_insert = "";
            foreach (GridViewRow dtgItem in this.G_INFORME_TOTAL_VENDEDOR.Rows)
            {


                CheckBox Sel = ((CheckBox)G_INFORME_TOTAL_VENDEDOR.Rows[dtgItem.RowIndex].FindControl("chkAccept"));
                bool valor = Sel.Checked;
                if (valor)
                {
                    invtid = G_INFORME_TOTAL_VENDEDOR.DataKeys[dtgItem.RowIndex].Values[0].ToString().Trim();
                    ponbr = G_INFORME_TOTAL_VENDEDOR.DataKeys[dtgItem.RowIndex].Values[1].ToString().Trim();

                    string ok = ReporteRNegocio.insert_compra_sys(invtid, ponbr, t_ob_cobro.Value);

                    if (ok == "OK")
                    {
                        ok_insert += "OK";

                    }
                    else
                    {

                        ok_insert += "Error";
                    }


                }
            }


            Button1_Click(sender, e);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  var elem3 = document.getElementById(\"<%=carga_2.ClientID%>\"); elem3.style.display = \"none\";</script>", false);



        }

        protected void G_INFORME_TOTAL_VENDEDOR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Eliminar")
                {


                    string CodDocumento = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());

                    string update_ok = ReporteRNegocio.desplanificar_sp(CodDocumento, 30);
                    if (update_ok == "OK")
                    {
                        string FechaEmision = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                        string CodVendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString());
                        string NotaLibre = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString());
                        string CodBodega = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[4].ToString());
                        string FechaDespacho = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString());
                        string CodMoneda = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[6].ToString());
                        string MontoNeto = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[7].ToString());
                        string DescEstadoDocumento = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[8].ToString());
                        string Facturas = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[9].ToString());
                        string GxEstadoSync = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[10].ToString());
                        string GxActualizado = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[11].ToString());
                        string GxEnviadoERP = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[12].ToString());
                        string NombreVendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[13].ToString());
                        string NombreCliente = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[14].ToString());
                        string DescBodega = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[15].ToString());
                        string FechaCreacion = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[16].ToString());
                        string ValorTipoCambio = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[17].ToString());
                        string LimiteSeguro = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[18].ToString());
                        string TipoCredito = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[19].ToString());
                        string CreditoDisponible = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[20].ToString());
                        string CreditoAutorizado = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[21].ToString());
                        string EmailVendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[22].ToString());


                        notificar_vendedor(EmailVendedor, "", CodDocumento, FechaEmision);

                        //enviar_email_desplanificado(CodDocumento, FechaEmision, CodVendedor, NotaLibre, CodBodega, FechaDespacho, CodMoneda, MontoNeto, DescEstadoDocumento, Facturas, GxEstadoSync, GxActualizado, GxEnviadoERP, NombreVendedor, NombreCliente, DescBodega, FechaCreacion, ValorTipoCambio, LimiteSeguro, TipoCredito, CreditoDisponible, CreditoAutorizado, EmailVendedor);

                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  document.getElementById('ContentPlaceHolder_Contenido_Button1').click();  </script>", false);

                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'> alert('Rechazado') </script>", false);


                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'> alert('Algo ocurrió :(') </script>", false);


                    }

                }
                if (e.CommandName == "Enviar")
                {

                    string CodDocumento = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    string CodBodega = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[4].ToString());

                    string estado_sp = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[12].ToString());
                    //string script1 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[2].Text), encriptador.EncryptData(""), encriptador.EncryptData(""), encriptador.EncryptData("57"));
                    //e.Row.Cells[2].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[2].Text + " </a>";

                    if (estado_sp == "Aprobado")
                    {
                        string asignado_por = "";
                        if (Session["SW_PERMI"].ToString() == "1")
                        {
                            asignado_por = "ABARROTES";

                        }
                        else
                        {
                            asignado_por = "GRANOS";

                        }
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "te1asd11Q21mp", "<script language='javascript'>   sp_select(" + CodDocumento + ", '" + CodBodega + "', '" + asignado_por + "') </script>", false);

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "te1asd11Q21mp", "<script language='javascript'>  alert('Sp no aprobada'); </script>", false);


                    }


                    //string sUrl = "/ListadoProductosPlanificador.aspx?C=" + "12542;";
                    //string sScript = "<script language =javascript> ";
                    //sScript += "window.open('" + sUrl + "',null,'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=100%,height=100%,left=100,top=100');";
                    //sScript += "</script> ";
                    //Response.Write(sScript);
                    //Response.Redirect("ListadoProductosPlanificador.aspx?C=10346");

                }



            }
            catch (Exception ex)
            {

            }
        }

        private void notificar_vendedor(string correo_vend, string select_scope, string sp, string fecha)
        {
            string aca = "";

            DataTable produ_desch = ReporteRNegocio.lista_det_sp_asignada(select_scope);

            string tabla = "";
            tabla += "<table class=\"table fill-head table-bordered\" style=\"width:100%;\">";
            tabla += "<thead class=\"test\" style=\"background-color:rgb(156,205,249)\">";
            tabla += "<tr>";
            tabla += "<th>Código</th>";
            tabla += "<th>Nombre</th>";
            tabla += "<th>TipoUnidad</th>";
            tabla += "<th>CantDespachado</th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            foreach (DataRow dr in produ_desch.Rows)
            {
                tabla += "<tr>";
                tabla += "<td>" + dr["codproducto"].ToString() + "</td>";
                tabla += "<td>" + dr["nomb"].ToString() + "</td>";
                tabla += "<td>" + dr["tipo_"].ToString() + "</td>";
                tabla += "<td>" + dr["despachado"].ToString() + "</td>";
                tabla += "</tr>";
            }
            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");

            enviar_email(tabla, correo_vend, sp, fecha);


            ScriptManager.RegisterStartupScript(Page, this.GetType(), "tee6ee", "<script> alert('SP ASIGNADA');</script>", false);
        }

        private void enviar_email(string html, string correo_vend, string sp, string fecha)
        {
            MailMessage email = new MailMessage();
            //email.To.Add(new MailAddress("egodoy@soprodi.cl"));
            email.To.Add(new MailAddress(correo_vend));
            email.From = new MailAddress("informes@soprodi.cl");
            string cliente_2 = Session["cliente"].ToString();
            email.Subject = "QUITADA SP Asignada " + sp + " " + cliente_2 + "( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";


            if (Session["SW_PERMI"].ToString() == "1")
            {
                email.CC.Add("egodoy@soprodi.cl, rmc@soprodi.cl");
            }
            else if (Session["SW_PERMI"].ToString() == "2")
            {
                email.CC.Add("egodoy@soprodi.cl");
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

            string fecha_emi = ReporteRNegocio.trae_fecha_emision_sp(sp);

            email.Body += "<div> Estimado :<br> <br>  <b> </b> <br><br>";
            email.Body += "<div>  <b> Fecha Emision: " + fecha_emi + "</b> <br><br>";
            email.Body += "<div> Se ha planificiado con fecha <b> " + fecha + "</b> el siguiente detalle de SP:  <br><br>";
            email.Body += html;
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

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>alert('CORREO ENVIADO!');</script>", false);

            }
            catch (Exception ex)
            {
                //lb_mensaj.Text = "Error al enviar ";
            }
        }

        private void enviar_email_desplanificado(string codDocumento, string fechaEmision, string codVendedor, string notaLibre, string codBodega, string fechaDespacho, string codMoneda, string montoNeto, string descEstadoDocumento, string facturas, string gxEstadoSync, string gxActualizado, string gxEnviadoERP, string nombreVendedor, string nombreCliente, string descBodega, string fechaCreacion, string valorTipoCambio, string limiteSeguro, string tipoCredito, string creditoDisponible, string creditoAutorizado, string emailVendedor)
        {
            throw new NotImplementedException();
        }

        private void enviar_email_cambio(string CodDocumento, string FechaEmision, string CodVendedor, string NotaLibre, string CodBodega, string FechaDespacho, string CodMoneda, string MontoNeto, string DescEstadoDocumento, string Facturas, string GxEstadoSync, string GxActualizado, string GxEnviadoERP, string NombreVendedor, string NombreCliente, string DescBodega, string FechaCreacion, string ValorTipoCambio, string LimiteSeguro, string TipoCredito, string CreditoDisponible, string CreditoAutorizado, string EmailVendedor)
        {


            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress("rmc@soprodi.cl"));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "SP Rechazada desde Sistema ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

            email.CC.Add(EmailVendedor + " , mazocar@soprodi.cl, jcorrea@soprodi.cl, gmorales@soprodi.cl, egodoy@soprodi.cl");

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

            email.Body += "<div> Estimado :<br> <br>  <b> </b> <br><br>";
            email.Body += "<div> Se ha RECHAZADO la siguiente SP:  <br><br>";
            email.Body += generar_tabla_con_sp(CodDocumento, FechaEmision, CodVendedor, NotaLibre, CodBodega, FechaDespacho, CodMoneda, MontoNeto, DescEstadoDocumento, Facturas, GxEstadoSync, GxActualizado, GxEnviadoERP, NombreVendedor, NombreCliente, DescBodega, FechaCreacion, ValorTipoCambio, LimiteSeguro, TipoCredito, CreditoDisponible, CreditoAutorizado, EmailVendedor);
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

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>alert('CORREO ENVIADO!');</script>", false);

            }
            catch (Exception ex)
            {
                //lb_mensaj.Text = "Error al enviar ";
            }

        }

        private string generar_tabla_con_sp(string codDocumento, string fechaEmision, string codVendedor,
            string notaLibre, string codBodega, string fechaDespacho, string codMoneda, string montoNeto,
            string descEstadoDocumento, string facturas, string gxEstadoSync, string gxActualizado, string gxEnviadoERP,
            string nombreVendedor, string nombreCliente, string descBodega, string fechaCreacion, string valorTipoCambio,
            string limiteSeguro, string tipoCredito, string creditoDisponible, string creditoAutorizado, string emailVendedor)
        {
            string tabla = "";


            tabla += "<table border=1 id='TABLA_2' border class='table table-advance table-bordered fill-head tablesorter filtrar' style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";
            tabla += "<thead class=\"test\">";
            tabla += "<tr style='background-color:#428bca'>";

            tabla += "<th>codDocumento</th>";
            tabla += "<th>fechaEmision</th>";
            tabla += "<th>codVendedor</th>";
            tabla += "<th>notaLibre</th>";
            tabla += "<th>codBodega</th>";
            tabla += "<th>fechaDespacho</th>";
            tabla += "<th>codMoneda</th>";
            tabla += "<th>montoNeto</th>";
            tabla += "<th>descEstadoDocumento</th>";
            tabla += "<th>facturas</th>";
            tabla += "<th>gxEstadoSync</th>";
            tabla += "<th>gxActualizado</th>";
            tabla += "<th>gxEnviadoERP</th>";
            tabla += "<th>nombreVendedor</th>";
            tabla += "<th>nombreCliente</th>";
            tabla += "<th>descBodega</th>";
            tabla += "<th>fechaCreacion</th>";
            tabla += "<th>valorTipoCambio</th>";
            tabla += "<th>limiteSeguro</th>";
            tabla += "<th>tipoCredito</th>";
            tabla += "<th>creditoDisponible</th>";
            tabla += "<th>creditoAutorizado</th>";
            tabla += "<th>emailVendedor</th>";


            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";

            tabla += "<tr>";

            tabla += "<td>" + codDocumento + "</td>";
            tabla += "<td>" + fechaEmision + "</td>";
            tabla += "<td>" + codVendedor + "</td>";
            tabla += "<td>" + notaLibre + "</td>";
            tabla += "<td>" + codBodega + "</td>";
            tabla += "<td>" + fechaDespacho + "</td>";
            tabla += "<td>" + codMoneda + "</td>";
            tabla += "<td>" + montoNeto + "</td>";
            tabla += "<td>" + descEstadoDocumento + "</td>";
            tabla += "<td>" + facturas + "</td>";
            tabla += "<td>" + gxEstadoSync + "</td>";
            tabla += "<td>" + gxActualizado + "</td>";
            tabla += "<td>" + gxEnviadoERP + "</td>";
            tabla += "<td>" + nombreVendedor + "</td>";
            tabla += "<td>" + nombreCliente + "</td>";
            tabla += "<td>" + descBodega + "</td>";
            tabla += "<td>" + fechaCreacion + "</td>";
            tabla += "<td>" + valorTipoCambio + "</td>";
            tabla += "<td>" + limiteSeguro + "</td>";
            tabla += "<td>" + tipoCredito + "</td>";
            tabla += "<td>" + creditoDisponible + "</td>";
            tabla += "<td>" + creditoAutorizado + "</td>";
            tabla += "<td>" + emailVendedor + "</td>";


            tabla += "</tr>";


            tabla += "</tbody>";
            tabla += "</table>";






            return tabla;
        }

        public string confirmDelete(string Name)
        {
            return @"javascript:if(!confirm('Esta acción va modificar el estado del documento: "
               + Name.ToUpper()
               + @". ¿Estás seguro?')){return false;} ; CARGANDO();";
        }
        public string abrir_modal(string Name)
        {
            return @"javascript: modal_unidad_1(" + Name + ");";
        }

        public string sp_selector(string CodDocumento)
        {
            return @"javascript: sp_select(" + CodDocumento + ");";
        }

        [WebMethod]
        public static string ENVIAR_CORREO(string PARA, string TEXT)
        {


            string respt = "";
            try
            {

            }
            catch { respt = "Error :02 Al guardar Producto"; }
            //}

            return respt;
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            string sp = tx_sp.Text;

            //double count_procesadas = ReporteRNegocio.existe_sp(sp);

            //if (count_procesadas == 0)
            //{

            DataTable dt_sp_encabe = ReporteRNegocio.ventamovil_sp_enc(sp);
            DataTable dt_sp_detalle = ReporteRNegocio.ventamovil_sp_det(sp);

            DataTable dt_sp_encabe_EXT = ReporteRNegocio.ventamovil_sp_enc_EXT(sp);
            DataTable dt_sp_detalle_EXT = ReporteRNegocio.ventamovil_sp_det_EXT(sp);

            string CodCliente = ReporteRNegocio.ventamovil_trae_codcliente(sp);

            DataTable dt_cliente_matriz = ReporteRNegocio.ventamovil_cliente_matriz(CodCliente);
            DataTable dt_cliente_sucursal = ReporteRNegocio.ventamovil_cliente_sucursal(CodCliente);

            string cadena_vm_thx = "Data Source=192.168.10.45;Initial Catalog=SoprodiVenta;Persist Security Info=True;User ID=sa;Password=Soprodi1234";

            bool ok = true;

            ok = borrar_sp(sp);
            ok = borrar_cliente(CodCliente);
            //------------------------------------------------------------------------>   encabezado 1 
            SqlBulkCopy bulkcopy = new SqlBulkCopy(cadena_vm_thx);
            bulkcopy.DestinationTableName = "TrnDocumentoCabecera";
            try
            {
                bulkcopy.WriteToServer(dt_sp_encabe);
            }
            catch (Exception ex)
            {
                ok = false;
                Console.Write(ex.Message);
            }
            //------------------------------------------------------------------------>   detalle 1
            SqlBulkCopy bulkcopy2 = new SqlBulkCopy(cadena_vm_thx);
            bulkcopy2.DestinationTableName = "TrnDocumentoDetalle";
            try
            {
                bulkcopy2.WriteToServer(dt_sp_detalle);
            }
            catch (Exception ex)
            {
                ok = false;
                Console.Write(ex.Message);
            }
            //ext_
            //------------------------------------------------------------------------>   encabezado 2
            SqlBulkCopy bulkcopy3 = new SqlBulkCopy(cadena_vm_thx);
            bulkcopy3.DestinationTableName = "ext_TrnDocumentoCabecera";
            try
            {
                bulkcopy3.WriteToServer(dt_sp_encabe_EXT);
            }
            catch (Exception ex)
            {
                ok = false;
                Console.Write(ex.Message);
            }
            //------------------------------------------------------------------------>   detalle 2
            SqlBulkCopy bulkcopy4 = new SqlBulkCopy(cadena_vm_thx);
            bulkcopy4.DestinationTableName = "ext_TrnDocumentoDetalle";
            try
            {
                bulkcopy4.WriteToServer(dt_sp_detalle_EXT);
            }
            catch (Exception ex)
            {
                ok = false;
                Console.Write(ex.Message);
            }
            //------------------------------------------------------------------------>   cliente matriz 1
            SqlBulkCopy bulkcopy5 = new SqlBulkCopy(cadena_vm_thx);
            bulkcopy5.DestinationTableName = "MaeClienteMatriz";
            try
            {
                bulkcopy5.WriteToServer(dt_cliente_matriz);
            }
            catch (Exception ex)
            {
                ok = false;
                Console.Write(ex.Message);
            }
            //------------------------------------------------------------------------>   cliente sucursal 1
            SqlBulkCopy bulkcopy6 = new SqlBulkCopy(cadena_vm_thx);
            bulkcopy6.DestinationTableName = "MaeClienteSucursal";
            try
            {
                bulkcopy6.WriteToServer(dt_cliente_sucursal);
            }
            catch (Exception ex)
            {
                ok = false;
                Console.Write(ex.Message);
            }
            ok = procesar_sp_traida(sp);
            if (ok)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'> alert('Agregada//Actualizada SP') </script>", false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'> alert('Hubo un error') </script>", false);
            }
            //}
            //else
            //{


            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'> alert('Existe SP') </script>", false);

            //}


        }

        private bool borrar_cliente(string codCliente)
        {
            string cadena_vm_thx = "Data Source=192.168.10.45;Initial Catalog=SoprodiVenta;Persist Security Info=True;User ID=sa;Password=Soprodi1234";

            using (SqlConnection conn = new SqlConnection(cadena_vm_thx))
            {
                conn.Open();
                string sql = @"   delete from MaeClienteSucursal     where codCliente in (@codCliente); " +
                               "  delete from MaeClienteMatriz       where codCliente in (@codCliente);  ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@codCliente", codCliente.Trim());
                    //cmd.Parameters.AddWithValue("@periodo", periodo);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception EX)
                    {
                        return false;
                    }
                }
            }
        }

        private bool borrar_sp(string sp)
        {

            string cadena_vm_thx = "Data Source=192.168.10.45;Initial Catalog=SoprodiVenta;Persist Security Info=True;User ID=sa;Password=Soprodi1234";

            using (SqlConnection conn = new SqlConnection(cadena_vm_thx))
            {
                conn.Open();
                string sql = @"   delete from TrnDocumentoCabecera      where coddocumento in (@sp); " +
                               "  delete from TrnDocumentoDetalle       where coddocumento in (@sp); " +
                               "  delete from ext_TrnDocumentoCabecera  where coddocumento in (@sp); " +
                               "  delete from ext_TrnDocumentoDetalle   where coddocumento in (@sp); ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@sp", sp.Trim());
                    //cmd.Parameters.AddWithValue("@periodo", periodo);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception EX)
                    {
                        return false;
                    }
                }
            }
        }

        private bool procesar_sp_traida(string sp)
        {
            DataTable dt = new DataTable();
            //string cadena_thx = "Data Source=192.168.10.45;Initial Catalog=[new_thx];Persist Security Info=True;User ID=sa;Password=Soprodi1234";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {

                /// LA SP PUEDE ESTAR APROBADA EN VENTAMOVIL (VM)  PERO NO SIGNIFICA QUE ESTE FACTURADA EN SOLOMON....
                /// DE ESO SE TRATA ESTE PROCESO, EL CUAL LE ASIGNA 3 ESTADOS SEGUN LO ANTERIOR....: 
                /// 10S------>   ESTA APROBADA (VM) PERO SIN FACTURA
                /// 10P------>   ESTA APROBADA  (VM) PERO LA CANTIDAD NO COINCIDEN ( OJO QUE PUEDE SER MAYOR LO FACTURADO Y DIRA LO MISMO ) 
                /// 20------->   LAS CANTIDAD COINCIDEN


                conn.Open();
                string sql = @"select y.*, cast(Cantidad - isnull(CANTIDAD_SOLOMON,0) AS decimal(16,2)) as cantidad_falta  from(
       select   A.* ,
                                 case 
            when(select  isnull(sum(cantidad), null) from[new_thx].[dbo].[V_SP_FACTURA_THX] where
                producto = A.CodProducto and sp = A.CodDocumento group by producto) is NULL
                then '10S'
            WHEN(select  isnull(sum(cantidad), null) from[new_thx].[dbo].[V_SP_FACTURA_THX] where
                producto = A.CodProducto and sp = A.CodDocumento group by producto) <> A.Cantidad
                THEN '10P'
            ELSE
                '20'
                end as ESTADO,
            isnull((select  isnull(sum(cantidad), null) from[new_thx].[dbo].[V_SP_FACTURA_THX] where
                producto = A.CodProducto and sp = A.CodDocumento group by producto),0) AS CANTIDAD_SOLOMON
        from
       (select b.CodDocumento, b.codproducto, b.cantidad from[SoprodiVenta].[dbo].[VPEDIDODETALLE_THX]  b

         where b.CodEstadoDocumento in ('20')  and ISNUMERIC(b.codproducto) = 1 AND b.descproducto not like '%serv%' and b.descproducto not like '%descarga%'   and b.coddocumento = '" + sp + "') A) y " +
    "     union " +
    "      select LTRIM(RTRIM(b.CodDocumento)) AS CodDocumento, LTRIM(RTRIM(b.codproducto)) AS codproducto, cast(b.cantidad as decimal(15,3)) as cantidad,  " +
 " rtrim(ltrim(convert(varchar,b.CodEstadoDocumento))) as ESTADO, 0 as CANTIDAD_SOLOMON, case when CodEstadoDocumento in ('40','10') then cast(b.cantidad as decimal(15,3)) else 0 end as cantidad_falta  from[SoprodiVenta].[dbo].[VPEDIDODETALLE_THX]  b " +
" where ISNUMERIC(b.codproducto) = 1 and b.CodEstadoDocumento  not in ('20') " +
" AND b.descproducto not like '%serv%' and b.descproducto not like '%descarga%'  and b.coddocumento = '" + sp + "'";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            try
            {
                string ssqlconnectionstring = ConfigurationManager.ConnectionStrings["default"].ToString();

                string sclearsql = "delete Sps_procesadas1  where CodDocumento = '"+ sp + "'";
                SqlConnection sqlconn = new SqlConnection(ssqlconnectionstring);
                SqlCommand sqlcmd = new SqlCommand(sclearsql, sqlconn);
                sqlconn.Open();
                sqlcmd.ExecuteNonQuery();
                sqlconn.Close();

                SqlBulkCopy bulkcopy = new SqlBulkCopy(ssqlconnectionstring);
                bulkcopy.DestinationTableName = "Sps_procesadas1";
                try
                {
                    bulkcopy.WriteToServer(dt);
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                    return false;

                }

            }
            catch
            {
                return false;
            }
            return true;
        }

        protected void btn_eliminar_check_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow dtgItem in this.G_INFORME_TOTAL_VENDEDOR.Rows)
            {
                CheckBox Sel = ((CheckBox)G_INFORME_TOTAL_VENDEDOR.Rows[dtgItem.RowIndex].FindControl("chkAccept"));
                bool valor = Sel.Checked;
                if (valor)
                {


                    string CodDocumento = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[0].ToString());
                    string DescEstadoDocumento1 = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[8].ToString());
                    string Facturas1 = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[9].ToString());


                    if (DescEstadoDocumento1.Trim() == "Sincronizado" || (DescEstadoDocumento1.Trim() == "Aprobado" && Facturas1 == ""))
                    {

                        string update_ok = ReporteRNegocio.VM_updateSP(CodDocumento, 30);
                        //string update_ok = "OK";
                        if (update_ok == "OK")
                        {
                            string FechaEmision = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[1].ToString());
                            string CodVendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[2].ToString());
                            string NotaLibre = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[3].ToString());
                            string CodBodega = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[4].ToString());
                            string FechaDespacho = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[5].ToString());
                            string CodMoneda = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[6].ToString());
                            string MontoNeto = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[7].ToString());
                            string DescEstadoDocumento = DescEstadoDocumento1;
                            string Facturas = Facturas1;
                            string GxEstadoSync = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[10].ToString());
                            string GxActualizado = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[11].ToString());
                            string GxEnviadoERP = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[12].ToString());
                            string NombreVendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[13].ToString());
                            string NombreCliente = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[14].ToString());
                            string DescBodega = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[15].ToString());
                            string FechaCreacion = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[16].ToString());
                            string ValorTipoCambio = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[17].ToString());
                            string LimiteSeguro = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[18].ToString());
                            string TipoCredito = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[19].ToString());
                            string CreditoDisponible = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[20].ToString());
                            string CreditoAutorizado = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[21].ToString());
                            string EmailVendedor = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(dtgItem.RowIndex)].Values[22].ToString());


                            enviar_email_cambio(CodDocumento, FechaEmision, CodVendedor, NotaLibre, CodBodega, FechaDespacho, CodMoneda, MontoNeto, DescEstadoDocumento, Facturas, GxEstadoSync, GxActualizado, GxEnviadoERP, NombreVendedor, NombreCliente, DescBodega, FechaCreacion, ValorTipoCambio, LimiteSeguro, TipoCredito, CreditoDisponible, CreditoAutorizado, EmailVendedor);


                            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'> alert('Rechazado') </script>", false);


                        }
                    }
                }
                else
                {
                }
            }
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  document.getElementById('ContentPlaceHolder_Contenido_Button1').click();  </script>", false);

        }


        public bool es_visible(string descdocumento)
        {
            if (descdocumento != "Aprobado")
            {
                return false;
            }
            else
            {
                return true;
            }
        }


    }
}