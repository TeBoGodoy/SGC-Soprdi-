using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Entities;
using CRM.BusinessLayer;
using ThinxsysFramework;
using System.Data;
using System.Web.Services;
using SoprodiApp.entidad;
using SoprodiApp.negocio;
using System.Web.Security;

namespace SoprodiApp.COTIZADOR
{
    public partial class DashboardAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                T_FECHA_DESDE.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                T_FECHA_HASTA.Text = DateTime.Now.ToString("yyyy-MM-dd");
                filtrartodo();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "cargaknob", "<script>javascript:cargarknob();Datatables();grafico1();</script>", false);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            MakeAccessible(G_DET_COTIZACIONES);
            MakeAccessible(G_PRODUCTOS_COTIZADOS);
            MakeAccessible(G_DET_CLIENTESCOTIZADOS);

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

        protected void B_FILTRAR_Click(object sender, EventArgs e)
        {
            filtrartodo();
        }

        public void filtrartodo()
        {
            try
            {
                DIV_VISIBLE.Visible = true;
                string filtro_fecha = "";

                if (T_FECHA_DESDE.Text != "")
                {
                    filtro_fecha += " and convert(date, fecha_envio, 103) >= convert(date, '" + Convert.ToDateTime(T_FECHA_DESDE.Text).ToString("dd/MM/yyyy") + "',103) ";
                }
                if (T_FECHA_HASTA.Text != "")
                {
                    filtro_fecha += " and convert(date, fecha_envio, 103) <= convert(date, '" + Convert.ToDateTime(T_FECHA_HASTA.Text).ToString("dd/MM/yyyy") + "',103) ";
                }
                vendedor_cotizadas(filtro_fecha);
                graficoscotizadosno(filtro_fecha);
                graficosproductoscotizadosno(filtro_fecha);
                productos_mas_cotizados(filtro_fecha);
                clientes_no_cotizados(filtro_fecha);
                vendedor_clientes(filtro_fecha);
                graf1();
            }
            catch (Exception ex)
            {

            }
        }

        public void graf1()
        {
            //DataTable dt_graf1 = new DataTable();
            //DBUtil db = new DBUtil();

            //dt_graf1 = db.consultar("select nom_categ, count(1) as cant, cast((count(*) * 100.0 / (select count(*) from V_CTZ_LOG_COTIZACION2 w)) as decimal(18,0)) as score from V_CTZ_LOG_COTIZACION2 group by nom_categ");

            //if (dt_graf1.Rows.Count > 0)
            //{
            //    string data = "[";
            //    string cats = "[";

            //    foreach(DataRow dr in dt_graf1.Rows)
            //    {
            //        cats += "\"" + dr["nom_categ"].ToString() + "\",";
            //        data += "{ \"value\": " + dr["cant"].ToString() + ", \"name\": \"" + dr["nom_categ"].ToString() + "\"},";
            //    }

            //    cats = cats.Remove(cats.Length - 1);
            //    data = data.Remove(data.Length - 1);

            //    data += "]";
            //    cats += "]";

            //    T_GRAF1_DATA.Text = data;
            //    T_GRAF1_CATS.Text = cats;
            //}
        }

        public void vendedor_cotizadas(string filtro_fecha)
        {
            try
            {
                DBUtil db = new DBUtil();
                DataTable dt = new DataTable();
                string sql_query = "";

                sql_query += " SELECT usp.cod_usuario, us.nombre_usuario, " +
                    " (select count(1) from CTZ_LOG_ENVIADAS ctzlog where ctzlog.cod_vendedor = usp.cod_usuario " + filtro_fecha + ") as sumaenviadas " +
                    " from UsuarioWebPhone usp left join " +
                    " UsuarioWeb us on us.cod_usuario = usp.cod_usuario " +
                    " order by sumaenviadas desc";

                dt = db.consultar(sql_query);
                string html = "";
                int cont = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    html += "<tr style='cursor:pointer' onclick='vercotizacionesvendedor(\"" + dr["cod_usuario"].ToString() + "\", \"" + dr["nombre_usuario"].ToString() + "\")' >";
                    html += "<td>" + cont.ToString() + "</td>";
                    html += "<td>" + dr["nombre_usuario"].ToString() + "</td>";
                    html += "<td><b class='success'>" + dr["sumaenviadas"].ToString() + "</b></td>";
                    html += "</tr>";
                    cont++;
                }
                div_vend_cotiza.InnerHtml = html;
                up_vend_cot.Update();
            }
            catch (Exception ex)
            {

            }


        }

        public void graficosproductoscotizadosno(string filtro_fecha)
        {
            try
            {
                DBUtil db = new DBUtil();
                int total_clientes = int.Parse(db.Scalar(" select count(1) from Stock_Excel_2 where fecha = (select MAX(fecha) from stock_excel_2) ").ToString());
                DataTable dt_cotizados = db.consultar("select distinct producto as mycount from [dbo].[V_CTZ_LOG_COTIZACION2]  where 1=1  " + filtro_fecha + " ");
                int cotizados = dt_cotizados.Rows.Count;
                int nocotizados = total_clientes - cotizados;

                int porcentajecotizado = (cotizados * 100) / total_clientes;
                int porcentajenocotizado = 100 - porcentajecotizado;

                div_total_productos.InnerHtml = total_clientes.ToString();
                GRAFICO_PRODUCTOS_COTIZADOS.InnerHtml = "<input type='text' value='" + porcentajecotizado.ToString() + "' class='knob hide-value responsive angle-offset' data-angleoffset='0' data-thickness='.15' data-linecap='round' data-width='150' data-height='150' data-inputcolor='#e1e1e1' data-readonly='true' data-fgcolor='#37BC9B' data-knob-icon='ft-trending-up'>";
                div_prod_cotizado.InnerHtml = porcentajecotizado.ToString() + " %";
                div_prod_nocotizado.InnerHtml = porcentajenocotizado.ToString() + " %";
            }
            catch (Exception ex)
            {

            }
        }

        public void graficoscotizadosno(string filtro_fecha)
        {
            try
            {
                DBUtil db = new DBUtil();
                int total_clientes = int.Parse(db.Scalar(" select count(1) from V_CTZ_GESTION_VENTAS_FIN where user1 = 'MayoristasLV' ").ToString());
                DataTable dt_cotizados = db.consultar("select distinct ctz.rut_cliente from CTZ_LOG_ENVIADAS ctz where 1=1 " + filtro_fecha + " ");
                int cotizados = dt_cotizados.Rows.Count;
                int nocotizados = total_clientes - cotizados;

                int porcentajecotizado = (cotizados * 100) / total_clientes;
                int porcentajenocotizado = 100 - porcentajecotizado;

                div_total_cotizados.InnerHtml = total_clientes.ToString();
                GRAFICO_CLIENTES_COTIZADOS.InnerHtml = "<input type='text' value='" + porcentajecotizado.ToString() + "' class='knob hide-value responsive angle-offset' data-angleoffset='0' data-thickness='.15' data-linecap='round' data-width='150' data-height='150' data-inputcolor='#e1e1e1' data-readonly='true' data-fgcolor='#37BC9B' data-knob-icon='ft-trending-up'>";
                div_per_cotizado.InnerHtml = porcentajecotizado.ToString() + " %";
                div_per_nocotizado.InnerHtml = porcentajenocotizado.ToString() + " %";

            }
            catch (Exception ex)
            {

            }



        }

        public void productos_mas_cotizados(string filtro_fecha)
        {
            try
            {
                DBUtil db = new DBUtil();
                DataTable dt = new DataTable();
                dt = db.consultar("SELECT nom_producto, nom_categ, COUNT(1) as mycount, producto FROM V_CTZ_LOG_COTIZACION2 where 1=1 " + filtro_fecha + " GROUP BY nom_producto, nom_categ, producto  order by mycount desc, nom_categ, nom_producto");
                G_PRODUCTOS_COTIZADOS.DataSource = dt;
                G_PRODUCTOS_COTIZADOS.DataBind();
            }
            catch (Exception ex)
            {

            }


        }

        public void clientes_no_cotizados(string filtro_fecha)
        {
            //DBUtil db = new DBUtil();
            //DataTable dt = new DataTable();
            //dt = db.consultar("");
            //string html = "";
            //html += "";
        }

        public void vendedor_clientes(string filtro_fecha)
        {
            //DBUtil db = new DBUtil();
            //DataTable dt = new DataTable();
            //dt = db.consultar("");
            //string html = "";
            //html += "";
        }

        protected void B_LLENARDETALLECOTIZACIONES_Click(object sender, EventArgs e)
        {
            try
            {
                string filtro_fecha = "";
                if (T_FECHA_DESDE.Text != "")
                {
                    filtro_fecha += " and convert(date, fecha_envio, 103) >= convert(date, '" + Convert.ToDateTime(T_FECHA_DESDE.Text).ToString("dd/MM/yyyy") + "',103) ";
                }
                if (T_FECHA_HASTA.Text != "")
                {
                    filtro_fecha += " and convert(date, fecha_envio, 103) <= convert(date, '" + Convert.ToDateTime(T_FECHA_HASTA.Text).ToString("dd/MM/yyyy") + "',103) ";
                }

                G_DET_COTIZACIONES.DataSource = ctz_log_cotizacionBO.GetAll(" where cod_vendedor =  '" + T_COD_VENDEDOR.Text + "' " + filtro_fecha + " ");
                G_DET_COTIZACIONES.DataBind();
                LBL_CTZDET_VENDEDOR.Text = "Vendedor: " + T_NOMBRE_VENDEDOR.Text;
                up_modal_detcotizacion.Update();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "abremodaldetcotizaciones", "<script>javascript:abremodaldetcotizaciones();</script>", false);
            }
            catch (Exception ex)
            {

            }


        }

        protected void b_llenar_clte_cotizados_Click(object sender, EventArgs e)
        {
            try
            {
                string filtro_fecha = "";
                if (T_FECHA_DESDE.Text != "")
                {
                    filtro_fecha += " and convert(date, fecha_envio, 103) >= convert(date, '" + Convert.ToDateTime(T_FECHA_DESDE.Text).ToString("dd/MM/yyyy") + "',103) ";
                }
                if (T_FECHA_HASTA.Text != "")
                {
                    filtro_fecha += " and convert(date, fecha_envio, 103) <= convert(date, '" + Convert.ToDateTime(T_FECHA_HASTA.Text).ToString("dd/MM/yyyy") + "',103) ";
                }

                DBUtil db = new DBUtil();
                string sql_query = "";

                sql_query += " select distinct rut_cliente, [nombre cliente], isnull(nombre_usuario,'NO ASIGNADO') as 'nombre_usuario', count(1) as contador " +
                    "   from V_CTZ_LOG_COTIZACION where rut_cliente <> '0' " + filtro_fecha + "  " +
                    "   group by rut_cliente, [nombre cliente], nombre_usuario " +
                    "   order by contador desc ";

                TITULO_COTIZADO_NOCOTIZADO.InnerHtml = "<h4 class='modal-title' id='myModalLabel18'><b>Clientes Cotizados</b></h4>";
                G_DET_CLIENTESCOTIZADOS.DataSource = db.consultar(sql_query);
                G_DET_CLIENTESCOTIZADOS.DataBind();
                up_modal_clte_cotizados.Update();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "asdfasvfsad", "<script>javascript:abremodaldetclientes();</script>", false);
            }
            catch (Exception ex)
            {

            }
        }

        protected void G_PRODUCTOS_COTIZADOS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Print")
                {
                    string id_producto = (G_PRODUCTOS_COTIZADOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    string nom_producto = (G_PRODUCTOS_COTIZADOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                    string filtro_fecha = "";
                    if (T_FECHA_DESDE.Text != "")
                    {
                        filtro_fecha += " and convert(date, fecha_envio, 103) >= convert(date, '" + Convert.ToDateTime(T_FECHA_DESDE.Text).ToString("dd/MM/yyyy") + "',103) ";
                    }
                    if (T_FECHA_HASTA.Text != "")
                    {
                        filtro_fecha += " and convert(date, fecha_envio, 103) <= convert(date, '" + Convert.ToDateTime(T_FECHA_HASTA.Text).ToString("dd/MM/yyyy") + "',103) ";
                    }
                    DBUtil db = new DBUtil();
                    G_DET_COTIZACIONES.DataSource = db.consultar("select * from V_CTZ_LOG_COTIZACION2 where  producto = '" + id_producto + "' " + filtro_fecha + " ");
                    G_DET_COTIZACIONES.DataBind();
                    LBL_CTZDET_VENDEDOR.Text = "Producto: " + nom_producto;
                    up_modal_detcotizacion.Update();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "abremodaldetcotizaciones", "<script>javascript:abremodaldetcotizaciones();</script>", false);
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void b_llenar_clte_cotizados2_Click(object sender, EventArgs e)
        {
            try
            {
                string filtro_fecha = "";
                if (T_FECHA_DESDE.Text != "")
                {
                    filtro_fecha += " and convert(date, fecha_envio, 103) >= convert(date, '" + Convert.ToDateTime(T_FECHA_DESDE.Text).ToString("dd/MM/yyyy") + "',103) ";
                }
                if (T_FECHA_HASTA.Text != "")
                {
                    filtro_fecha += " and convert(date, fecha_envio, 103) <= convert(date, '" + Convert.ToDateTime(T_FECHA_HASTA.Text).ToString("dd/MM/yyyy") + "',103) ";
                }

                DBUtil db = new DBUtil();
                string sql_query = "";

                sql_query += "   select distinct  rutcliente as 'rut_cliente', [nombre cliente], isnull([nombre vendedor],'NO ASIGNADO') as 'nombre_usuario', 0 as contador  from V_CTZ_GESTION_VENTAS_FIN " +
                    "   where user1 in ('Abarrotes','MayoristasLV') " +
                    "   and rutcliente not in (select rut_cliente from CTZ_LOG_ENVIADAS " + filtro_fecha + ") ";

                ScriptManager.RegisterStartupScript(this, typeof(Page), "asdfasvfsad", "<script>javascript:abremodaldetclientes();</script>", false);
                TITULO_COTIZADO_NOCOTIZADO.InnerHtml = "<h4 class='modal-title' id='myModalLabel18'><b>Clientes No Cotizados</b></h4>";
                G_DET_CLIENTESCOTIZADOS.DataSource = db.consultar(sql_query);
                G_DET_CLIENTESCOTIZADOS.DataBind();
                up_modal_clte_cotizados.Update();
            }
            catch (Exception ex)
            {

            }
        }
    }
}