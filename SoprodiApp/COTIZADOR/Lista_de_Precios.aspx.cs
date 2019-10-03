using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.BusinessLayer;
using CRM.Entities;
using SoprodiApp.entidad;
using SoprodiApp.negocio;
using System.Web.Security;
using ThinxsysFramework;

namespace COTIZADOR
{
    public partial class Lista_de_Precios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Usuario"] == null)
                {
                    Response.Redirect("../Acceso.aspx");
                }
                LlenarBodegas();
            }
            else
            {
                JQ_Datatable();
            }
        }

        public void JQ_Datatable()
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "SP_COTIZACIONES22", "<script language='javascript'>Datatables();</script>", false);
        }

        public void LlenarBodegas()
        {
            DBUtil db = new DBUtil();
            CB_BODEGA.DataSource = db.consultar("select cod_bodega, nombre_bodega from CTZ_Bodegas order by nombre_bodega");
            CB_BODEGA.DataValueField = "cod_bodega";
            CB_BODEGA.DataTextField = "nombre_bodega";
            CB_BODEGA.DataBind();          
        }

        protected void B_FILTRAR_PRECIOS_Click(object sender, EventArgs e)
        {
            FiltrarProductos();
        }

        public void FiltrarProductos()
        {
            G_PRODUCTOS.Columns[8].Visible = false;
            G_PRODUCTOS.Columns[9].Visible = false;
            G_PRODUCTOS.Columns[10].Visible = false;
            G_PRODUCTOS.Columns[11].Visible = false;


            DBUtil db = new DBUtil();

            string columnas = "";
            int contador_columnas = 0;
            string precioabuscar = "";
            string stockabuscar = " 'XXXXXX' ";
            foreach (ListItem item in CB_BODEGA.Items)
            {
                if (item.Selected)
                {
                    contador_columnas++;
                    columnas += columnas == "" ? item.Text : ", " + item.Text;
                    if (item.Value == "ARICA")
                    {
                        precioabuscar += ", stock.arica_vent as 'precioventa" + contador_columnas + "', ISNULL(CAST((stock.arica_vent/und.unidad_equivale) as numeric), stock.arica_vent) as 'preciounitario" + contador_columnas + "' ";
                        stockabuscar += ", 'ARICASOP' ";
                    }
                    else if (item.Value == "ETREDIRECT")
                    {
                        precioabuscar += ", stock.entrega_directa_V_RM as 'precioventa" + contador_columnas + "' , ISNULL(CAST((stock.entrega_directa_V_RM/und.unidad_equivale)  as numeric), 0) as 'preciounitario" + contador_columnas + "' ";
                    }
                    else if (item.Value == "IQUIQUE")
                    {
                        precioabuscar += ", stock.iquique as 'precioventa" + contador_columnas + "' , ISNULL(CAST((stock.iquique/und.unidad_equivale)  as numeric), stock.iquique) as 'preciounitario" + contador_columnas + "'  ";
                        stockabuscar += ", 'ARICASOP' ";
                    }
                    else if (item.Value == "LOSANGELES")
                    {
                        precioabuscar += ", stock.bod_LA_vent as 'precioventa" + contador_columnas + "' , ISNULL(CAST((stock.bod_LA_vent/und.unidad_equivale)  as numeric), 0) as 'preciounitario" + contador_columnas + "'  ";
                        stockabuscar += ", 'LOSANGELES' ";
                    }
                    else if (item.Value == "LZ_DESPACHO")
                    {
                        precioabuscar += ", stock.reparto_RM_V as 'precioventa" + contador_columnas + "' , ISNULL(CAST((stock.reparto_RM_V/und.unidad_equivale)  as numeric), 0) as 'preciounitario" + contador_columnas + "'  ";
                        stockabuscar += ", 'ZARATESOP' ";
                    }
                    else if (item.Value == "ZARATE")
                    {
                        precioabuscar += ", stock.bod_LZ_vent as 'precioventa" + contador_columnas + "' , ISNULL(CAST((stock.bod_LZ_vent/und.unidad_equivale)  as numeric), 0) as 'preciounitario" + contador_columnas + "'  ";
                        stockabuscar += ", 'ZARATESOP' ";
                    }
                    // 3
                    if (contador_columnas == 1)
                    {
                        G_PRODUCTOS.Columns[6].HeaderText = "($ Venta) " + item.Text;
                        G_PRODUCTOS.Columns[7].HeaderText = "($ Unit.) " + item.Text;
                    }
                    if (contador_columnas == 2)
                    {
                        G_PRODUCTOS.Columns[8].HeaderText = "($ Venta) " + item.Text;
                        G_PRODUCTOS.Columns[9].HeaderText = "($ Unit.) " + item.Text;
                        G_PRODUCTOS.Columns[8].Visible = true;
                        G_PRODUCTOS.Columns[9].Visible = true;                     
                    }
                    if (contador_columnas == 3)
                    {
                        G_PRODUCTOS.Columns[10].HeaderText = "($ Venta) " + item.Text;
                        G_PRODUCTOS.Columns[11].HeaderText = "($ Unit.) " + item.Text;
                        G_PRODUCTOS.Columns[10].Visible = true;
                        G_PRODUCTOS.Columns[11].Visible = true;

                    }
                }
            }
            if (contador_columnas > 3)
            {
                //alert("No se pueden seleccionar mas de 3 columnas de precio", "Atención", "rojo", 3000);
            }
            else if (contador_columnas >= 1)
            {
                if (contador_columnas == 1)
                {
                    precioabuscar += ", 0 as 'precioventa2', 0 as 'preciounitario2', 0 as 'precioventa3', 0 as 'preciounitario3' ";
                }
                else if (contador_columnas == 2)
                {
                    precioabuscar += ", 0 as 'precioventa3', 0 as 'preciounitario3' ";
                }

                //LB_BODEGA.Text = ": " + columnas;

                string query = "";
                query += " select X.* from (select stock.cod_producto, stock.producto, isnull(cat.nom_categ,'Sin Categoría') as 'nom_categ', ";
                // STOCK
                if (stockabuscar != " 'XXXXXX' ")
                {
                    query += " (select isnull(sum(sdc.TOTAL),0) from Stock_diario_CALCULADO sdc where sdc.invtid = stock.cod_producto and siteid in (" + stockabuscar + ")) as stock, ";
                }
                else
                {
                    query += " 99999 as stock, ";
                }
                query += " (select top(1) isnull([stkunit],'N/A') from Stock_diario_CALCULADO sdc where sdc.invtid = stock.cod_producto) as medida, " +
                    //
                    " isnull(catprod.id_categoria,19) as 'id_categoria', isnull(und.unidad_equivale,1) as unidad_equivale " + precioabuscar + " from Stock_Excel_2 stock" +
                    " left join Categoria_producto catprod on catprod.id_producto = stock.cod_producto " +
                    " left join categoria cat on cat.id = catprod.id_categoria " +
                    " left join unidad_stock_sp und on und.cod_prod = stock.cod_producto " +
                    " where stock.fecha = (select MAX(fecha) from stock_excel_2)) as X ";
                //if (stockabuscar != " 'XXXXXX' ")
                //{
                //    query += " where X.stock > 0  ";
                //}
                query += " order by X.stock desc, X.producto ";

                G_PRODUCTOS.DataSource = db.consultar(query);
                G_PRODUCTOS.DataBind();

                //if (stockabuscar != " 'XXXXXX' ")
                //{
                //    //
                //    query = query.Replace(" where X.stock > 0  ", "  where X.stock <= 0 ");
                //    G_PRODUCTOS2.DataSource = db.consultar(query);
                //    G_PRODUCTOS2.DataBind();
                //}
                //else
                //{
                //    query = query.Replace(" where X.stock > 0  ", "  where 1=2 ");
                //    G_PRODUCTOS2.DataSource = db.consultar(query);
                //    G_PRODUCTOS2.DataBind();
                //}

                string fecha = db.Scalar("select CONVERT(varchar,MAX(fecha), 103) from stock_excel_2").ToString();
                LB_FECHA_PRECIOS.Text = "FECHA LISTA DE PRECIO: " + fecha;
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "AbreModal", "<script>javascript:abremodalproductos();</script>", false);
            }
            else
            {
                //alert("Seleccione al menos una columna precio", "Atención", "rojo", 2000);
            }

            // QUERY ANTIGUA DE RESPALDO
            //***************************
            //query += "select X.* from (select stock.cod_producto, stock.producto, isnull(cat.nom_categ,'Sin Categoría') as 'nom_categ', " +
            //   // STOCK
            //   " (select isnull(sum(sdc.TOTAL),0) from Stock_diario_CALCULADO sdc where sdc.invtid = stock.cod_producto) as stock, " +
            //   " (select top(1) isnull([stkunit],'N/A') from Stock_diario_CALCULADO sdc where sdc.invtid = stock.cod_producto) as medida, " +
            //   //
            //   " isnull(catprod.id_categoria,19) as 'id_categoria', isnull(und.unidad_equivale,1) as unidad_equivale " + precioabuscar + " from Stock_Excel_2 stock" +
            //   " left join Categoria_producto catprod on catprod.id_producto = stock.cod_producto " +
            //   " left join categoria cat on cat.id = catprod.id_categoria " +
            //   " left join unidad_stock_sp und on und.cod_prod = stock.cod_producto " +
            //   " where stock.fecha = (select MAX(fecha) from stock_excel_2)) as X where X.stock > 0 " +
            //   " order by X.nom_categ, X.producto ";

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);         
            MakeAccessible(G_PRODUCTOS);      
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