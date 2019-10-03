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
    public partial class REPORTE_DIARIO : System.Web.UI.Page
    {
        public static int cont_periodos;
        public static DataTable aux;
        public static DataTable productos;
        public static DataTable totales;
        public static string PERIODOS;
        private static string USER;
        public static int colum;

        public static bool header_sum = true;
        public static bool header_sum2 = true;
        public static string periodo1 = "";
        public static string periodo2 = "";
        public static string periodo3= "";
        public static string periodo4 = "";

        public static string periodo1_ = "";
        public static string periodo2_ = "";
        public static string periodo3_ = "";
        public static string periodo4_ = "";

        private static string vendedor;
        private static string cliente;

        public static int cont_det;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.RegisterRedirectOnSessionEndScript();
            if (!IsPostBack)
            {
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

                header_sum = true;
             
                l_usuario_.Text = USER;
                //DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now;
                //t = new DateTime(t.Year, t.Month - 1, 1);
                //string DESDE = t.ToShortDateString();
                string HASTA = t2.ToShortDateString();

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

                string periodo_nuevo_mes1 = HASTA.Substring(HASTA.IndexOf("/") + 1, 2);
                string periodo_nuevo_año1 = HASTA.Substring(HASTA.LastIndexOf("/") + 1);
                string periodo_1 = periodo_nuevo_año1 + periodo_nuevo_mes1;

                //string periodo_nuevo_mes1 = "01";
                //string periodo_nuevo_año1 = "2016";
                //string periodo_1 = periodo_nuevo_año1 + periodo_nuevo_mes1;


                string periodo_nuevo_mes2 = "";
                string periodo_nuevo_año2 = "";
                string periodo_2 = "";

                string periodo_nuevo_mes3 = "";
                string periodo_nuevo_año3 = "";
                string periodo_3 = "";

                string periodo_nuevo_mes4 = "";
                string periodo_nuevo_año4 = "";
                string periodo_4 = "";

                if (Convert.ToInt32(periodo_nuevo_mes1) == 1)
                {
                    periodo_nuevo_mes2 = "12";
                    periodo_nuevo_año2 = (Convert.ToInt32(periodo_nuevo_año1) - 1).ToString();
                }
                else
                {
                    periodo_nuevo_mes2 = (Convert.ToInt32(periodo_nuevo_mes1) - 1).ToString();
                    if (periodo_nuevo_mes2.Length == 1) { periodo_nuevo_mes2 = "0" + periodo_nuevo_mes2; }
                    periodo_nuevo_año2 = (Convert.ToInt32(periodo_nuevo_año1)).ToString();
                }
                periodo_2 = periodo_nuevo_año2 + periodo_nuevo_mes2;

                if (Convert.ToInt32(periodo_nuevo_mes2) == 1)
                {
                    periodo_nuevo_mes3 = "12";
                    periodo_nuevo_año3 = (Convert.ToInt32(periodo_nuevo_año2) - 1).ToString();

                }
                else
                {
                    periodo_nuevo_mes3 = (Convert.ToInt32(periodo_nuevo_mes2) - 1).ToString();
                    if (periodo_nuevo_mes3.Length == 1) { periodo_nuevo_mes3 = "0" + periodo_nuevo_mes3; }
                    periodo_nuevo_año3 = (Convert.ToInt32(periodo_nuevo_año2)).ToString();
                }
                periodo_3 = periodo_nuevo_año3 + periodo_nuevo_mes3;

                if (Convert.ToInt32(periodo_nuevo_mes3) == 1)
                {
                    periodo_nuevo_mes4 = "12";
                    periodo_nuevo_año4 = (Convert.ToInt32(periodo_nuevo_año3) - 1).ToString();

                }
                else
                {
                    periodo_nuevo_mes4 = (Convert.ToInt32(periodo_nuevo_mes3) - 1).ToString();
                    if (periodo_nuevo_mes4.Length == 1) { periodo_nuevo_mes4 = "0" + periodo_nuevo_mes4; }
                    periodo_nuevo_año4 = (Convert.ToInt32(periodo_nuevo_año3)).ToString();

                }
                periodo_4 = periodo_nuevo_año4 + periodo_nuevo_mes4;


                PERIODOS = periodo_1 + "," + periodo_2 + "," + periodo_3 + "," + periodo_4;

                h3_.InnerText = "Reporte Diario (desde " + "01/" + periodo_4.Substring(4, 2) + "/" + periodo_4.Substring(0, 4) +" hasta " + HASTA + ")";


                CARGAR_DIARIO1();
                CARGAR_DIARIO2();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_PRODUCTOS')); </script>", false);

            }
        }

        private void CARGAR_DIARIO1()
        {
            G_INFORME_TOTAL_VENDEDOR.Visible = true;
            G_INFORME_TOTAL_VENDEDOR.DataSource = ReporteRNegocio.listar_diario1(USER);
            G_INFORME_TOTAL_VENDEDOR.DataBind();

        }

        private void CARGAR_DIARIO2()
        {
            cont_det = 1;
            G_PRODUCTOS.Visible = true;
            DataTable productos_diario = ReporteRNegocio.listar_diario2(USER);
            colum = productos_diario.Columns.Count;
            productos_diario.Columns.Add("Total");
            G_PRODUCTOS.DataSource = productos_diario;
            G_PRODUCTOS.DataBind();

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_PRODUCTOS')); </script>", false);
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

                // CAPI
                //int sum_total = 0;
                //for (int i = 2; i <= colum - 1; i++)
                //{
                //    try
                //    {
                //        sum_total = sum_total + Convert.ToInt32(e.Row.Cells[i].Text);
                //    }
                //    catch { }
                //}
                //e.Row.Cells[e.Row.Cells.Count - 1].Text = sum_total.ToString();
                //aux.Rows[e.Row.RowIndex]["Total general"] = sum_total.ToString();
            }
        }

        protected void G_INFORME_TOTAL_VENDEDOR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<string> periodos_list = PERIODOS.Split(',').ToList();
                G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[0].Text = "FACTORES/Periodos";
                G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[1].Text = periodos_list[0].ToString();
                G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[2].Text = periodos_list[1].ToString();
                G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[3].Text = periodos_list[2].ToString();
                G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[4].Text = periodos_list[3].ToString();
                //G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[5].Text = "Total";
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


        protected void G_PRODUCTOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string vendedor = G_PRODUCTOS.DataKeys[e.Row.RowIndex].Values[2].ToString();
                string cliente = G_PRODUCTOS.DataKeys[e.Row.RowIndex].Values[3].ToString();

                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                 //encriptador.EncryptData(


                e.Row.Cells[1].Attributes["onclick"] = "javascript:fuera('" + encriptador.EncryptData(PERIODOS) + "', '" + encriptador.EncryptData(vendedor) + "', '" + encriptador.EncryptData(cliente) + "', '" + encriptador.EncryptData("4") + "');return false;";

                double sum_por_row = 0;
                
                for (int i = 5; i <= colum; i++)
                {

                    double d;
                    double.TryParse(e.Row.Cells[i + 1].Text, out d);
                    sum_por_row += d;
                }
                e.Row.Cells[colum+2].Text = sum_por_row.ToString("N0");

                e.Row.Cells[0].Text = cont_det.ToString();
                cont_det++;

                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                G_PRODUCTOS.HeaderRow.Cells[2].Visible = false;
                G_PRODUCTOS.HeaderRow.Cells[3].Visible = false;

                if (header_sum)
                {
                    periodo1 = ReporteRNegocio.Facturación_Mes_precalculado1(USER); ;
                    periodo2 = ReporteRNegocio.Facturación_Mes_precalculado2(USER); ;
                    periodo3 = ReporteRNegocio.Facturación_Mes_precalculado3(USER); ;
                    periodo4 = ReporteRNegocio.Facturación_Mes_precalculado4(USER); ;

                    periodo1_ = ReporteRNegocio.pregunta_periodo_prod(USER, "1");
                    periodo2_ = ReporteRNegocio.pregunta_periodo_prod(USER, "2");
                    periodo3_ = ReporteRNegocio.pregunta_periodo_prod(USER, "3");
                    periodo4_ = ReporteRNegocio.pregunta_periodo_prod(USER, "4");

                    header_sum = false;
                }
                List<string> periodos_list = PERIODOS.Split(',').ToList();



                G_PRODUCTOS.HeaderRow.Cells[4].Text = "Vendedor";
                G_PRODUCTOS.HeaderRow.Cells[3].Text = "Producto";
                G_PRODUCTOS.HeaderRow.Cells[5].Text = "Cliente";
                G_PRODUCTOS.HeaderRow.Cells[6].Text = periodo1_ + " (" + Double.Parse( periodo1).ToString("N0") + ") ";
                G_PRODUCTOS.HeaderRow.Cells[7].Text = periodo2_ + " (" + Double.Parse(periodo2).ToString("N0") + ") ";
                G_PRODUCTOS.HeaderRow.Cells[8].Text = periodo3_ + " (" + Double.Parse(periodo3).ToString("N0") + ") ";
                G_PRODUCTOS.HeaderRow.Cells[9].Text = periodo4_ + " (" + Double.Parse(periodo4).ToString("N0") + ") ";

               
                G_PRODUCTOS.HeaderRow.Cells[6].Attributes["data-sort-method"] = "number";
                G_PRODUCTOS.HeaderRow.Cells[7].Attributes["data-sort-method"] = "number";
                G_PRODUCTOS.HeaderRow.Cells[8].Attributes["data-sort-method"] = "number";
                G_PRODUCTOS.HeaderRow.Cells[9].Attributes["data-sort-method"] = "number";
                G_PRODUCTOS.HeaderRow.Cells[10].Attributes["data-sort-method"] = "number";

                try
                {
                    if (G_PRODUCTOS.HeaderRow.Cells[6].Text.Contains("Error"))
                    {
                        G_PRODUCTOS.HeaderRow.Cells[6].Attributes["style"] = "display: none;";
                        e.Row.Cells[6].Attributes["style"] = "display: none;";
                    }
                    if (G_PRODUCTOS.HeaderRow.Cells[7].Text.Contains("Error"))
                    {
                        G_PRODUCTOS.HeaderRow.Cells[7].Attributes["style"] = "display: none;";
                        e.Row.Cells[7].Attributes["style"] = "display: none;";
                    }
                    if (G_PRODUCTOS.HeaderRow.Cells[8].Text.Contains("Error"))
                    {
                        G_PRODUCTOS.HeaderRow.Cells[8].Attributes["style"] = "display: none;";
                        e.Row.Cells[8].Attributes["style"] = "display: none;";
                    }
                    if (G_PRODUCTOS.HeaderRow.Cells[9].Text.Contains("Error"))
                    {
                        G_PRODUCTOS.HeaderRow.Cells[9].Attributes["style"] = "display: none;";
                        e.Row.Cells[9].Attributes["style"] = "display: none;";
                    }
                }
                catch { }
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

        protected void G_PRODUCTOS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Producto")
            {
                vendedor = G_PRODUCTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                cliente = G_PRODUCTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();

                string grupos_del_usuario = "";

                grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

                if (grupos_del_usuario == "")
                {
                    grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
                }


                header_sum2 = true;
                cont_det = 1;
                DataTable aux = ReporteRNegocio.listar_prod_client(G_PRODUCTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString(), G_PRODUCTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString(), PERIODOS, grupos_del_usuario);
                G_DET_PRODUCTOS.DataSource = aux;
                productos = aux;
                G_DET_PRODUCTOS.DataBind();

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>CLICK_MODAL();</script>", false);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_PRODUCTOS')); </script>", false);

            }

        }

        protected void G_DET_PRODUCTOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = cont_det.ToString();
                cont_det++;
                int colum = 6;
                for (int i = 2; i <= colum; i++)
                {
                    try
                    {
                        e.Row.Cells[i + 1].Text = Convert.ToInt32(e.Row.Cells[i + 1].Text).ToString("N0");
                    }
                    catch { }
                }
                string where_es = " where vendedor = '" + vendedor.Trim() + "' and nombrecliente like '" + cliente + "'  ";
                if (header_sum2)
                {
                    int colum2 = productos.Columns.Count;
                    for (int i = 3; i <= colum2; i++)
                    {
                        try
                        {
                            G_DET_PRODUCTOS.HeaderRow.Cells[i + 1].Text = G_DET_PRODUCTOS.HeaderRow.Cells[i + 1].Text + "  (" + Convert.ToDouble(ReporteRNegocio.Facturación_Mes(G_DET_PRODUCTOS.HeaderRow.Cells[i + 1].Text, where_es)).ToString("N0") + ")";

                        }
                        catch { }
                    }
                    header_sum2 = false;
                }
            }
        }
    }
}