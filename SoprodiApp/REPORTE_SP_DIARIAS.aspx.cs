using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoprodiApp.negocio;
using SoprodiApp.entidad;
using System.Drawing;
using Microsoft.Office.Interop;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.Services;
using ThinxsysFramework;


namespace SoprodiApp
{
    public partial class REPORTE_SP_DIARIAS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "47")
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

                    cargar_productos_SP("Abarrotes");
                }
                else if (Session["SW_PERMI"].ToString() == "2")
                {
                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=2'>Granos</a>";
                    titulo2.HRef = "reportes.aspx?s=2";
                    titulo2.InnerText = "Granos";

                    cargar_productos_SP("Granos");


                }

                string es_vend = ReporteRNegocio.esvendedor(User.Identity.Name.ToString());
                if (es_vend != "2")
                {
                    Session["codvendedor"] = "";
                }
                else
                {
                    Session["codvendedor"] = " and codvendedor = '" + User.Identity.Name.ToString() + "'";
                }

                txt_desde.Text = DateTime.Now.AddDays(-1).ToShortDateString();
                txt_hasta.Text = DateTime.Now.AddDays(-1).ToShortDateString();

                cargar_vendedor_SP();
                cargar_estado_SP();
                cargar_clientes_SP();

            }

        }

        private void cargar_clientes_SP()
        {

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.VM_clientes(" where 1=1 " + Session["codvendedor"], "");
            dtv = dt.DefaultView;
            d_cliente.DataSource = dtv;
            d_cliente.DataTextField = "nombre";
            d_cliente.DataValueField = "rut";
            d_cliente.DataBind();
        }

        private void cargar_estado_SP()
        {

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.VM_estados_2("");
            dtv = dt.DefaultView;
            d_estado_sp.DataSource = dtv;
            d_estado_sp.DataTextField = "descestadodocumento";
            d_estado_sp.DataValueField = "descestadodocumento";
            d_estado_sp.DataBind();
        }
        private void cargar_vendedor_SP()
        {
            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.VM_listaVendedor(Session["codvendedor"].ToString(), "");
            dtv = dt.DefaultView;
            d_vendedor.DataSource = dtv;
            d_vendedor.DataTextField = "nombrevendedor";
            d_vendedor.DataValueField = "codvendedor";
            d_vendedor.DataBind();
        }

        private void cargar_productos_SP(string depto)
        {
            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.VM_productos(depto, "");
            dtv = dt.DefaultView;
            d_producto.DataSource = dtv;
            d_producto.DataTextField = "NOMBRE_COMPLETO";
            d_producto.DataValueField = "codproducto";
            d_producto.DataBind();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            MakeAccessible(G_SP_DIARIA);
            //MakeAccessible(G_ASIGNADOS);
            //MakeAccessible(G_QR);
            //MakeAccessible(G_LISTA);
        }
        public void JQ_Datatable()
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "datatable", "<script language='javascript'>creagrilla();</script>", false);
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


        protected void Reporte_Click(object sender, EventArgs e)
        {
            string hoy_ = "02/12/2019";

            string desde = txt_desde.Text;
            string hasta = txt_hasta.Text;
            string vendedores = Base.agrega_comillas(l_vendedor.Text);
            string clientes = Base.agrega_comillas(l_cliente.Text);
            string productos = Base.agrega_comillas(l_producto.Text);
            string estados = Base.agrega_comillas(d_estado_sp.Text);
            string sps_num = Base.agrega_comillas(txt_sp.Text);

            string where_ = " ";

            string es_vend = ReporteRNegocio.esvendedor(User.Identity.Name.ToString());
            if (es_vend != "2")
            {

                if (vendedores != "")
                    where_ += " and codvendedor in (" + vendedores + ")";
                if (Session["SW_PERMI"].ToString() == "1")
                {

                    where_ += " and descemisor <> 'Granos'";
                }
                else
                {
                    where_ += " and descemisor = 'Granos'";
                }
            }
            else
            {
                where_ += " and codvendedor = '" + User.Identity.Name.ToString() + "'";
            }

            if (clientes != "")
                where_ += " and RUT in (" + clientes + ")";

            if (productos != "")
                where_ += " and CODPRODUCTO in (" + productos + ")";

            if (estados != "")
                where_ += " and ESTADO in (" + estados + ")";

            if (desde != "")
                where_ += " and FECHAEMISION >= CONVERT(DATETIME, '" + desde + "', 103) ";

            if (hasta != "")
                where_ += " and FECHAEMISION <= CONVERT(DATETIME, '" + hasta + "', 103) ";

            if (sps_num != "")
                where_ += " and [N°SP] IN (" + sps_num + ") ";

            DataTable DT = ReporteRNegocio.sp_diaria(where_);


            int total_monto = 0;
            foreach (DataRow r in DT.Rows)
            {
                int d;
                int.TryParse(r["MONTO NETO"].ToString().Replace(".", ""), out d);
                total_monto += d;
            }


            string tabla = "";
            tabla += "<table class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";
            tabla += "<th>Total monto</th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";

            tabla += "<tr>";
            tabla += "<td>" + total_monto.ToString("N0") + "</td>";
            tabla += "</tr>";

            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");

            div_totales.InnerHtml = tabla;


            G_SP_DIARIA.DataSource = DT;
            G_SP_DIARIA.DataBind();
            JQ_Datatable();

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "chosen", "<script language='javascript'>  load_chosen_combos();  </script>", false);

        }

        protected void G_SP_DIARIA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string stock_zarate = e.Row.Cells[12].Text;
                e.Row.Cells[12].Text = Base.monto_format(stock_zarate.Replace(",00", ""));

                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                string script1 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[1].Text), encriptador.EncryptData(""), encriptador.EncryptData(""), encriptador.EncryptData("57"));
                e.Row.Cells[1].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[1].Text + " </a>";

                if (Session["codvendedor"].ToString() == "")
                {

                    string costo_compra = e.Row.Cells[16].Text;
                    string utlidad_excel = e.Row.Cells[17].Text;
                    string utlidad_compra = e.Row.Cells[18].Text;

                    double d3;
                    double.TryParse(costo_compra, out d3);
                    double d4;
                    double.TryParse(utlidad_excel, out d4);
                    double d5;
                    double.TryParse(utlidad_compra, out d5);


                    d3 = Math.Round(d3, 3, MidpointRounding.ToEven);
                    d4 = Math.Round(d4, 3, MidpointRounding.ToEven);
                    d5 = Math.Round(d5, 3, MidpointRounding.ToEven);

                    e.Row.Cells[16].Text = Base.monto_format2(d3);
                    e.Row.Cells[17].Text = Base.monto_format2(d4);
                    e.Row.Cells[18].Text = Base.monto_format2(d5);

                    e.Row.Cells[0].Text = e.Row.Cells[21].Text;
                    e.Row.Cells[21].Visible = false;
                    G_SP_DIARIA.HeaderRow.Cells[21].Visible = false;
                }
                else
                {


                    e.Row.Cells[14].Visible = false;
                    G_SP_DIARIA.HeaderRow.Cells[14].Visible = false;

                    e.Row.Cells[15].Visible = false;
                    G_SP_DIARIA.HeaderRow.Cells[15].Visible = false;

                    e.Row.Cells[16].Visible = false;
                    G_SP_DIARIA.HeaderRow.Cells[16].Visible = false;

                    e.Row.Cells[17].Visible = false;
                    G_SP_DIARIA.HeaderRow.Cells[17].Visible = false;

                    e.Row.Cells[18].Visible = false;
                    G_SP_DIARIA.HeaderRow.Cells[18].Visible = false;

                    e.Row.Cells[19].Visible = false;
                    G_SP_DIARIA.HeaderRow.Cells[19].Visible = false;

                    e.Row.Cells[20].Visible = false;
                    G_SP_DIARIA.HeaderRow.Cells[20].Visible = false;

                    e.Row.Cells[21].Visible = false;
                    G_SP_DIARIA.HeaderRow.Cells[21].Visible = false;


                }
            }
        }

        protected void G_SP_DIARIA_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}