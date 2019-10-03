using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ThinxsysFramework;
using SoprodiApp.negocio;
using System.Data;

namespace SoprodiApp
{
    public partial class Stock_detalle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //JQ_Datatable();

            string id_prod = Request.QueryString["C"];
            string compras = Request.QueryString["F"];


            if (compras == "2")
            {
                bool prim = true;
                bool prim2 = true;

                DataTable total_dt = new DataTable();
                //costos
                total_dt = ReporteRNegocio.ultima_compra(id_prod, compras);
                DataTable general_dt = new DataTable();
                DataTable costos_dt = new DataTable();

                //foreach (DataRow r in total_dt.Rows)
                //{
                //    DataRow row;
                //    row = costos_dt.NewRow();


                //    for (int i = 19; i < total_dt.Columns.Count; i++)
                //    {
                //        if (prim)
                //        {
                //            try
                //            {

                //                costos_dt.Columns.Add(total_dt.Columns[i].ColumnName);
                //                costos_dt.Columns.Add("Fecha");
                //            }
                //            catch { }
                //        }
                //        row[total_dt.Columns[i].ColumnName] = r[i].ToString();
                //        row["Fecha"] = r[7].ToString();
                //    }
                //    costos_dt.Rows.Add(row);
                //    prim = false;



                //}

                //foreach (DataRow r in total_dt.Rows)
                //{
                //    DataRow row;
                //    row = general_dt.NewRow();
                //    for (int i = 0; i < 19; i++)
                //    {
                //        if (prim2)
                //        {
                //            try
                //            {
                //                general_dt.Columns.Add(total_dt.Columns[i].ColumnName);

                //            }
                //            catch { }
                //        }
                //        row[total_dt.Columns[i].ColumnName] = r[i].ToString();
                //    }
                //    general_dt.Rows.Add(row);
                //    prim = false;
                //    break;
                //}

                G_GENERAL.DataSource = general_dt;
                G_GENERAL.DataBind();
                G_GENERAL.Visible = true;
                GENERAL_DIV.Attributes["style"] = "display:none";


                G_DETALLE_STOCK.DataSource = total_dt;
                G_DETALLE_STOCK.DataBind();
                G_DETALLE_STOCK.Visible = true;
                normal.Attributes["style"] = "display:block";

            }
            else if (compras == "1")
            {


                DataTable total_dt = new DataTable();
                //costos
                total_dt = ReporteRNegocio.ultimas_compra2(id_prod, compras);
                DataTable general_dt = new DataTable();
                DataTable costos_dt = new DataTable();


                //bool prim = true;
                //bool prim2 = true;

                //foreach (DataRow r in total_dt.Rows)
                //{
                //    DataRow row;
                //    row = costos_dt.NewRow();


                //    for (int i = 19; i < total_dt.Columns.Count; i++)
                //    {
                //        if (prim)
                //        {
                //            try
                //            {

                //                costos_dt.Columns.Add(total_dt.Columns[i].ColumnName);
                //                costos_dt.Columns.Add("Fecha");
                //            }
                //            catch { }
                //        }
                //        row[total_dt.Columns[i].ColumnName] = r[i].ToString();
                //        row["Fecha"] = r[7].ToString();
                //    }
                //    costos_dt.Rows.Add(row);
                //    prim = false;



                //}

                //foreach (DataRow r in total_dt.Rows)
                //{
                //    DataRow row;
                //    row = general_dt.NewRow();
                //    for (int i = 0; i < 19; i++)
                //    {
                //        if (prim2)
                //        {
                //            try
                //            {
                //                general_dt.Columns.Add(total_dt.Columns[i].ColumnName);

                //            }
                //            catch { }
                //        }
                //        row[total_dt.Columns[i].ColumnName] = r[i].ToString();
                //    }
                //    general_dt.Rows.Add(row);
                //    prim = false;
                //    break;
                //}
                ////general_dt = ReporteRNegocio.ventas_stock(id_prod.Trim());

                //G_GENERAL.DataSource = general_dt;
                //G_GENERAL.DataBind();
                //G_GENERAL.Visible = true;
                //GENERAL_DIV.Attributes["style"] = "display:block";

                G_GENERAL.DataSource = total_dt;
                G_GENERAL.DataBind();
                G_GENERAL.Visible = true;
                GENERAL_DIV.Attributes["style"] = "display:block";


                G_DETALLE_STOCK.DataSource = costos_dt;
                G_DETALLE_STOCK.DataBind();
                G_DETALLE_STOCK.Visible = true;
                normal.Attributes["style"] = "display:none";


            }
            else if (compras == "88")
            {
                DataTable total_dt = new DataTable();
                //costos
                total_dt = ReporteRNegocio.ultima_compra(id_prod, compras);
                DataTable general_dt = new DataTable();
                DataTable costos_dt = new DataTable();


                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                string where = " and producto = '" + encriptador.DecryptData(id_prod.Replace(" ", "+")).Split('*')[0] + "' " + encriptador.DecryptData(id_prod.Replace(" ", "+")).Split('*')[1];
                general_dt = ReporteRNegocio.ventas_stock(where);

                G_VENTAS.DataSource = general_dt;
                G_VENTAS.DataBind();
                G_VENTAS.Visible = true;
                GENERAL_DIV.Attributes["style"] = "display:none";
                normal.Attributes["style"] = "display:none";
                VENTAS.Attributes["style"] = "display:block";
            }

            else if (compras == "89")
            {

                string intvID = id_prod.Split('*')[0].ToString();
                string desde = id_prod.Split('*')[1].ToString();
                string hasta = id_prod.Split('*')[2].ToString();
                string siteID = id_prod.Split('*')[3].ToString();

                string RcpDate = "";
                if (desde != "")
                {
                    RcpDate += " and RcptDate >= convert(datetime, '" + desde + "',103)";
                }

                if (hasta != "")
                {

                    RcpDate += " and RcptDate <= convert(datetime, '" + hasta + "',103)";

                }



                string where = " invtID = '" + intvID + "' " + RcpDate;

                DataTable total_dt = new DataTable();
                //costos
                total_dt = ReporteRNegocio.ultimas_compra3(where, compras);
                DataTable general_dt = new DataTable();
                DataTable costos_dt = new DataTable();

                G_GENERAL.DataSource = total_dt;
                G_GENERAL.DataBind();
                G_GENERAL.Visible = true;
                GENERAL_DIV.Attributes["style"] = "display:block";

                G_DETALLE_STOCK.DataSource = costos_dt;
                G_DETALLE_STOCK.DataBind();
                G_DETALLE_STOCK.Visible = true;
                normal.Attributes["style"] = "display:none";

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teardsaeee", "<script> creagrilla(); </script>", false);
            }
            else if (compras == "90")
            {
                DataTable total_dt = new DataTable();
                //costos
                total_dt = ReporteRNegocio.ultima_compra(id_prod, compras);
                DataTable general_dt = new DataTable();
                DataTable costos_dt = new DataTable();


                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                string producto = encriptador.DecryptData(id_prod.Replace(" ", " + ")).Split('*')[0];
                string periodo = encriptador.DecryptData(id_prod.Replace(" ", " + ")).Split('*')[1];
                string bodegas = encriptador.DecryptData(id_prod.Replace(" ", " + ")).Split('*')[2];



                string where = " and producto = '" + producto + "' and periodo = " + periodo;



                if (bodegas != "")
                {

                    where += " and bodega in (" + agregra_comillas(bodegas) + ")";

                }

                general_dt = ReporteRNegocio.ventas_stock(where);

                G_VENTAS.DataSource = general_dt;
                G_VENTAS.DataBind();
                G_VENTAS.Visible = true;
                GENERAL_DIV.Attributes["style"] = "display:none";
                normal.Attributes["style"] = "display:none";
                VENTAS.Attributes["style"] = "display:block";
            }

            else if (compras == "91")
            {

                string intvID = id_prod.Split('*')[0].ToString();
                string desde = id_prod.Split('*')[1].ToString();
                string hasta = id_prod.Split('*')[2].ToString();
                string siteID = id_prod.Split('*')[3].ToString();

                string RcpDate = "";
                if (desde != "")
                {
                    //RcpDate += " and RcptDate >= convert(datetime, '" + desde + "',103)";
                }

                if (hasta != "")
                {

                    RcpDate += " and trandate <= convert(datetime, '" + hasta + "',103)";

                }



                string where = " siteID= '"+siteID.Replace("'", "")+"' and invtID = '" + intvID + "' " + RcpDate;

                DataTable total_dt = new DataTable();
                //costos
                total_dt = ReporteRNegocio.historial_stock_diario(where, compras);
                DataTable general_dt = new DataTable();
                DataTable costos_dt = new DataTable();

                G_GENERAL.DataSource = total_dt;
                G_GENERAL.DataBind();
                G_GENERAL.Visible = true;
                GENERAL_DIV.Attributes["style"] = "display:block";

                G_DETALLE_STOCK.DataSource = costos_dt;
                G_DETALLE_STOCK.DataBind();
                G_DETALLE_STOCK.Visible = true;
                normal.Attributes["style"] = "display:none";

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teardsaeee", "<script> creagrilla(); </script>", false);
            }

            else if (compras == "92")
            {
                //AJUSTES
                string intvID = id_prod.Split('*')[0].ToString();
                string desde = id_prod.Split('*')[1].ToString();
                string hasta = id_prod.Split('*')[2].ToString();
                string PERIODO = id_prod.Split('*')[3].ToString();

                string RcpDate = "";
                //if (desde != "")
                //{
                //    RcpDate += " and RcptDate >= convert(datetime, '" + desde + "',103)";
                //}

                //if (hasta != "")
                //{

                //    RcpDate += " and trandate <= convert(datetime, '" + hasta + "',103)";

                //}


                RcpDate += " and convert(varchar, year(trandate),103 ) + right('0000' + convert(varchar, month(trandate),103 ),2) = '"+PERIODO+"'";


                string where = " TRANTYPE= 'AJ' and invtID = '" + intvID + "' " + RcpDate;

                DataTable total_dt = new DataTable();
                //costos
                total_dt = ReporteRNegocio.historial_stock_diario(where, compras);
                DataTable general_dt = new DataTable();
                DataTable costos_dt = new DataTable();

                G_GENERAL.DataSource = total_dt;
                G_GENERAL.DataBind();
                G_GENERAL.Visible = true;
                GENERAL_DIV.Attributes["style"] = "display:block";

                G_DETALLE_STOCK.DataSource = costos_dt;
                G_DETALLE_STOCK.DataBind();
                G_DETALLE_STOCK.Visible = true;
                normal.Attributes["style"] = "display:none";

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teardsaeee", "<script> creagrilla(); </script>", false);
            }



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
        protected void G_DETALLE_STOCK_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[7].Text = e.Row.Cells[7].Text.Replace("0:00:00", "");
                e.Row.Cells[18].Text = e.Row.Cells[18].Text.Replace("0:00:00", "");


            }


        }

        protected void G_GENERAL_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            MakeAccessible(G_DETALLE_STOCK);
            MakeAccessible(G_GENERAL);
            //MakeAccessible(G_QR);
            //MakeAccessible(G_LISTA);
        }
        public void JQ_Datatable()
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd1q21mp", "<script language='javascript'>creagrilla();</script>", false);
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

        protected void G_VENTAS_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}