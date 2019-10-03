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

using SoprodiApp.Entities;
using SoprodiApp.entidad;
using SoprodiApp.BusinessLayer;
using System.Web.Security;


namespace COTIZADOR
{
    public partial class Cotizaciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Usuario"] == null)
                {
                    Response.Redirect("../Acceso.aspx");
                }
                else
                {
                    LlenarLista();
                    LlenarBodegas();
                    LlenarServicio();
                }
            }
            else
            {
                JQ_Datatable();
            }
        }

        public void LlenarServicio()
        {
            DBUtil db = new DBUtil();
            G_SERVICIOS.DataSource = db.consultar("select * from CTZ_COTIZACIONES_SERVICIOS where id_cotizacion_servicio = -1");
            G_SERVICIOS.DataBind();
        }

        public void LlenarLista()
        {
            usuarioEntity vend = new usuarioEntity();
            vend = (usuarioEntity)(Session["usuario"]);
            G_PRINCIPAL.DataSource = ctz_cotizacionBO.GetAll(" where cod_vendedor =  '" + vend.cod_usuario + "' and estado in ('GUARDADA', 'AUTO GUARDADO') ");
            G_PRINCIPAL.DataBind();
        }

        public void LlenarBodegas()
        {
            DBUtil db = new DBUtil();
            CB_BODEGA.DataSource = db.consultar("select cod_bodega, nombre_bodega from CTZ_Bodegas order by nombre_bodega");
            CB_BODEGA.DataValueField = "cod_bodega";
            CB_BODEGA.DataTextField = "nombre_bodega";
            CB_BODEGA.DataBind();

            CB_SERVICIO.DataSource = db.consultar("select cod_servicio, nombre_servicio from CTZ_Servicios order by nombre_servicio");
            CB_SERVICIO.DataValueField = "cod_servicio";
            CB_SERVICIO.DataTextField = "nombre_servicio";
            CB_SERVICIO.DataBind();
        }

        public void FiltrarProductos()
        {
            G_PRODUCTOS.Columns[8].Visible = false;
            G_PRODUCTOS.Columns[9].Visible = false;
            G_PRODUCTOS.Columns[10].Visible = false;
            G_PRODUCTOS.Columns[11].Visible = false;

            G_PRODUCTOS2.Columns[8].Visible = false;
            G_PRODUCTOS2.Columns[9].Visible = false;
            G_PRODUCTOS2.Columns[10].Visible = false;
            G_PRODUCTOS2.Columns[11].Visible = false;
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

                        G_PRODUCTOS2.Columns[6].HeaderText = "($ Venta) " + item.Text;
                        G_PRODUCTOS2.Columns[7].HeaderText = "($ Unit.) " + item.Text;
                    }
                    if (contador_columnas == 2)
                    {
                        G_PRODUCTOS.Columns[8].HeaderText = "($ Venta) " + item.Text;
                        G_PRODUCTOS.Columns[9].HeaderText = "($ Unit.) " + item.Text;
                        G_PRODUCTOS.Columns[8].Visible = true;
                        G_PRODUCTOS.Columns[9].Visible = true;

                        G_PRODUCTOS2.Columns[8].HeaderText = "($ Venta) " + item.Text;
                        G_PRODUCTOS2.Columns[9].HeaderText = "($ Unit.) " + item.Text;
                        G_PRODUCTOS2.Columns[8].Visible = true;
                        G_PRODUCTOS2.Columns[9].Visible = true;
                    }
                    if (contador_columnas == 3)
                    {
                        G_PRODUCTOS.Columns[10].HeaderText = "($ Venta) " + item.Text;
                        G_PRODUCTOS.Columns[11].HeaderText = "($ Unit.) " + item.Text;
                        G_PRODUCTOS.Columns[10].Visible = true;
                        G_PRODUCTOS.Columns[11].Visible = true;

                        G_PRODUCTOS2.Columns[10].HeaderText = "($ Venta) " + item.Text;
                        G_PRODUCTOS2.Columns[11].HeaderText = "($ Unit.) " + item.Text;
                        G_PRODUCTOS2.Columns[10].Visible = true;
                        G_PRODUCTOS2.Columns[11].Visible = true;
                    }
                }
            }
            if (contador_columnas > 3)
            {
                alert("No se pueden seleccionar mas de 3 columnas de precio", "Atención", "rojo", 3000);
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

                LB_BODEGA.Text = ": " + columnas;

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
                if (stockabuscar != " 'XXXXXX' ")
                {
                    query += " where X.stock > 0  ";
                }


                query += " order by X.nom_categ, X.producto";

                G_PRODUCTOS.DataSource = db.consultar(query);
                G_PRODUCTOS.DataBind();

                if (stockabuscar != " 'XXXXXX' ")
                {
                    //
                    query = query.Replace(" where X.stock > 0  ", "  where X.stock <= 0 ");
                    G_PRODUCTOS2.DataSource = db.consultar(query);
                    G_PRODUCTOS2.DataBind();
                }
                else
                {
                    query = query.Replace(" where X.stock > 0  ", "  where 1=2 ");
                    G_PRODUCTOS2.DataSource = db.consultar(query);
                    G_PRODUCTOS2.DataBind();
                }

                string fecha = db.Scalar("select CONVERT(varchar,MAX(fecha), 103) from stock_excel_2").ToString();
                LB_FECHA_PRECIOS.Text = ": " + fecha;
                ScriptManager.RegisterStartupScript(this, typeof(Page), "AbreModal", "<script>javascript:abremodalproductos();</script>", false);
            }
            else
            {
                alert("Seleccione al menos una columna precio", "Atención", "rojo", 2000);
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

        public void JQ_Datatable()
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "SP_COTIZACIONES", "<script language='javascript'>chosen();Datatables();</script>", false);
        }

        protected void G_PRINCIPAL_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //editar
                if (e.CommandName == "Ver")
                {
                    int id = int.Parse((G_PRINCIPAL.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString()));
                    COMPLETAR_DETALLE(id);
                    PANEL_PRINCIPAL.Visible = false;
                    PANEL_DETALLE.Visible = true;
                }
                if (e.CommandName == "Borrar")
                {
                    DBUtil db = new DBUtil();
                    int id = int.Parse((G_PRINCIPAL.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString()));

                    ctz_cotizacionEntity ctz = new ctz_cotizacionEntity();
                    ctz.id_cotizacion = id;
                    if (ctz_cotizacionBO.encontrar(ref ctz) == "OK")
                    {
                        ctz.estado = "INACTIVO";
                        if (ctz_cotizacionBO.actualizar(ctz) == "OK")
                        {
                            db.Scalar("delete from ctz_cotizacion_det  where id_cotizacion = " + ctz.id_cotizacion);
                            alert("Eliminada con éxito", "Cotización", "verde", 2000);
                            LlenarLista();
                        }
                        else
                        {
                            alert("Error al eliminar", "Cotización", "rojo", 2000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void COMPLETAR_DETALLE(int id)
        {
            DBUtil db = new DBUtil();
            DataTable dt = new DataTable();

            ctz_cotizacionEntity ctz = new ctz_cotizacionEntity();
            ctz.id_cotizacion = id;
            if (ctz_cotizacionBO.encontrar(ref ctz) == "OK")
            {
                foreach (ListItem xx in CB_BODEGA.Items)
                {
                    xx.Selected = false;
                }
                // SELECCIONAR EN EL LISTBOX LAS COLUMNAS DE PRECIO QUE USA Y BLOQUEAR
                string columnas_precio = ctz.columnas_precio;
                string[] cms = columnas_precio.Split(new Char[] { ';' });
                foreach (string x in cms)
                {
                    if (x != "")
                    {
                        foreach (ListItem xx in CB_BODEGA.Items)
                        {
                            if (xx.Value == x)
                            {
                                xx.Selected = true;
                            }
                        }
                    }
                }

                T_ID.Text = ctz.id_cotizacion.ToString();
                T_NOMBRE_CTZ.Text = ctz.nombre_cotizacion;
                T_DESCRIPCION_CTZ.Text = ctz.descripcion;
                if (ctz.servicios_separado == 1)
                {
                    RB_SEPARAR.Checked = true;
                }
                else
                {
                    RB_SEPARAR.Checked = false;
                }
                LlenarDetalle(id);
                G_SERVICIOS.DataSource = ctz_cotizaciones_serviciosBO.GetAll(" where id_cotizacion = " + id);
                G_SERVICIOS.DataBind();
                CalcularTotales(id);
                string fecha = db.Scalar("select CONVERT(varchar,MAX(fecha), 103) from stock_excel_2").ToString();
                LBL_FECHA_PRECIO.Text = "<i class='fa fa-calendar'></i> Fecha lista de precios: " + fecha;
            }
        }

        public void LlenarDetalle(int id)
        {
            DBUtil db = new DBUtil();
            DataTable detalle = ctz_cotizacion_detBO.GetAll(" where id_cotizacion = " + id);
            if (detalle.Rows.Count > 0)
            {

                G_DETALLE_COTIZACION.Columns[6].Visible = false;
                G_DETALLE_COTIZACION.Columns[7].Visible = false;
                // MOSTRAR OCULTAR COLUMNAS
                int contttt = 1;
                string columnas_precio = db.Scalar("select columnas_precio from ctz_cotizacion where id_cotizacion = " + id).ToString();
                string[] cms = columnas_precio.Split(new Char[] { ';' });
                foreach (string x in cms)
                {
                    if (x != "")
                    {
                        if (contttt == 1)
                        {
                            G_DETALLE_COTIZACION.Columns[5].HeaderText = db.Scalar("select nombre_bodega from ctz_bodegas where cod_bodega = '" + x + "'").ToString();
                        }
                        if (contttt == 2)
                        {
                            G_DETALLE_COTIZACION.Columns[6].Visible = true;
                            G_DETALLE_COTIZACION.Columns[6].HeaderText = db.Scalar("select nombre_bodega from ctz_bodegas where cod_bodega = '" + x + "'").ToString();
                        }
                        if (contttt == 3)
                        {
                            G_DETALLE_COTIZACION.Columns[7].Visible = true;
                            G_DETALLE_COTIZACION.Columns[7].HeaderText = db.Scalar("select nombre_bodega from ctz_bodegas where cod_bodega = '" + x + "'").ToString();
                        }
                        contttt++;
                    }
                }
            }
            G_DETALLE_COTIZACION.DataSource = detalle;
            G_DETALLE_COTIZACION.DataBind();
        }
        protected void B_NUEVO_Click(object sender, EventArgs e)
        {
            LIMPIARCAMPOS();
            foreach (ListItem x in CB_BODEGA.Items)
            {
                x.Selected = false;
            }
            //CB_BODEGA.Enabled = true;
            G_DETALLE_COTIZACION.DataSource = ctz_cotizacion_detBO.GetAll(" where id_cotizacion = -1 ");
            G_DETALLE_COTIZACION.DataBind();

            G_SERVICIOS.DataSource = ctz_cotizaciones_serviciosBO.GetAll(" where id_cotizacion = -1 ");
            G_SERVICIOS.DataBind();

            PANEL_PRINCIPAL.Visible = false;
            PANEL_DETALLE.Visible = true;
        }

        protected void B_GUARDAR_Click(object sender, EventArgs e)
        {

            GuardarCotizacion();
        }

        protected void B_VOLVER_Click(object sender, EventArgs e)
        {
            LlenarLista();
            PANEL_PRINCIPAL.Visible = true;
            PANEL_DETALLE.Visible = false;
        }

        #region ---------------- NO CAMBIAR ---------------- 
        public void alert(string mensaje, string titulo, string color, int tiempo)
        {
            // ROJO - VERDE 
            ScriptManager.RegisterStartupScript(this, typeof(Page), "mosnoti", "<script>javascript:Mensaje('" + titulo + "', '" + mensaje + "', '" + color + "', " + tiempo + ");</script>", false);
        }
        public void LIMPIARCAMPOS()
        {
            CleanControl(this.Controls);
        }

        public void CleanControl(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = string.Empty;
                else if (control is DropDownList)
                    ((DropDownList)control).ClearSelection();
                else if (control is ListBox)
                    ((ListBox)control).ClearSelection();
                else if (control is RadioButtonList)
                    ((RadioButtonList)control).ClearSelection();
                else if (control is CheckBoxList)
                    ((CheckBoxList)control).ClearSelection();
                else if (control is RadioButton)
                    ((RadioButton)control).Checked = false;
                else if (control is CheckBox)
                    ((CheckBox)control).Checked = false;
                else if (control.HasControls())
                    CleanControl(control.Controls);
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            MakeAccessible(G_PRINCIPAL);
            MakeAccessible(G_DETALLE_COTIZACION);
            MakeAccessible(G_PRODUCTOS);
            MakeAccessible(G_PRODUCTOS2);
            MakeAccessible(G_CLIENTES);
            MakeAccessible(G_CLIENTES2);
            MakeAccessible(G_SERVICIOS);
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
        #endregion
        protected void G_DETALLE_COTIZACION_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Borrar")
                {
                    DBUtil db = new DBUtil();
                    int id_det = int.Parse((G_DETALLE_COTIZACION.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString()));
                    int id_cotizacion = int.Parse((G_DETALLE_COTIZACION.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString()));
                    ctz_cotizacion_detEntity det_ctz = new ctz_cotizacion_detEntity();
                    det_ctz.id_cotizacion_det = id_det;
                    if (ctz_cotizacion_detBO.encontrar(ref det_ctz) == "OK")
                    {
                        if (ctz_cotizacion_detBO.eliminar(det_ctz) == "OK")
                        {
                            alert("Producto retirado con éxito", "PRODUCTO", "rojo", 2000);
                            G_DETALLE_COTIZACION.DataSource = ctz_cotizacion_detBO.GetAll(" where id_cotizacion = " + id_cotizacion);
                            G_DETALLE_COTIZACION.DataBind();
                            CalcularTotales(id_cotizacion);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void B_ABRE_MODAL_PRODUCTOS_Click(object sender, EventArgs e)
        {
            DBUtil db = new DBUtil();
            CB_CATEGORIAS.DataSource = db.consultar("select id, nom_categ from categoria order by nom_categ");
            CB_CATEGORIAS.DataValueField = "id";
            CB_CATEGORIAS.DataTextField = "nom_categ";
            CB_CATEGORIAS.DataBind();
            FiltrarProductos();
            UP_PRODUCTOS.Update();
        }
        public void GuardarCotizacion(bool autoguardar = false)
        {
            try
            {
                string columnas = "";
                foreach (ListItem item in CB_BODEGA.Items)
                {
                    if (item.Selected)
                    {
                        columnas += ";" + item.Value;
                    }
                }
                ctz_cotizacionEntity ctz = new ctz_cotizacionEntity();
                if (T_ID.Text == "")
                {
                    usuarioEntity vend = new usuarioEntity();
                    vend = (usuarioEntity)(Session["usuario"]);

                    ctz.nombre_cotizacion = T_NOMBRE_CTZ.Text;
                    ctz.descripcion = T_DESCRIPCION_CTZ.Text;
                    ctz.fecha_creacion = DateTime.Now;
                    if (autoguardar)
                    {
                        ctz.estado = "AUTO GUARDADO";
                    }
                    else
                    {
                        ctz.estado = "GUARDADA";
                    }
                    ctz.cod_vendedor = vend.cod_usuario;
                    if (RB_SEPARAR.Checked)
                    {
                        ctz.servicios_separado = 1;
                    }
                    else
                    {
                        ctz.servicios_separado = 0;
                    }
                    ctz.num_columnas = 1;
                    ctz.columnas_precio = columnas;
                    // SCOPE
                    string scope = "";
                    scope = ctz_cotizacionBO.registrar_scope(ctz);
                    int scope_aux = int.Parse(scope);
                    T_ID.Text = scope;
                    alert("Creada con éxito", "Cotización", "verde", 2000);
                    editarcolumnasdeprecio();
                    LlenarDetalle(scope_aux);
                    //CB_BODEGA.Enabled = false;
                }
                else
                {
                    // MODIFICAR
                    ctz.id_cotizacion = int.Parse(T_ID.Text);
                    if (ctz_cotizacionBO.encontrar(ref ctz) == "OK")
                    {
                        ctz.nombre_cotizacion = T_NOMBRE_CTZ.Text;
                        ctz.descripcion = T_DESCRIPCION_CTZ.Text;
                        ctz.estado = "GUARDADA";

                        if (RB_SEPARAR.Checked)
                        {
                            ctz.servicios_separado = 1;
                        }
                        else
                        {
                            ctz.servicios_separado = 0;
                        }
                        ctz.num_columnas = 1;
                        if (ctz_cotizacionBO.actualizar(ctz) == "OK")
                        {
                            alert("Modificada con éxito", "Cotización", "verde", 2000);
                            editarcolumnasdeprecio();
                            LlenarDetalle(ctz.id_cotizacion);
                        }
                        else
                        {
                            alert("Error al modificar", "Cotización", "rojo", 2000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                alert(ex.Message, "ERROR", "rojo", 5000);
            }
        }

        public void editarcolumnasdeprecio()
        {
            try
            {
                DBUtil db = new DBUtil();
                string columnas = "";
                int conttt = 1;
                foreach (ListItem item in CB_BODEGA.Items)
                {
                    if (item.Selected)
                    {
                        columnas += ";" + item.Value;
                        db.Scalar("update ctz_cotizacion_det set cod_bodega_" + conttt + " = '" + item.Value + "' where id_cotizacion = " + T_ID.Text);
                        conttt++;
                    }
                }
                string para_borrar = "";
                if (conttt == 2)
                {
                    para_borrar += " cod_bodega_2 = 'NO', cantidad_2 = 0, precio_2 = 0, precio_unitario_2 = 0, descuento_2 = 0, precio_con_descuento_2 = 0, precio_con_descuento_unitario_2 = 0, total_2 = 0, total_iva_2 = 0 ";
                }
                else if (conttt == 3)
                {
                    if (para_borrar != "")
                    {
                        para_borrar += " , ";
                    }
                    para_borrar += " cod_bodega_3 = 'NO', cantidad_3 = 0, precio_3 = 0, precio_unitario_3 = 0, descuento_3 = 0, precio_con_descuento_3 = 0, precio_con_descuento_unitario_3 = 0, total_3 = 0, total_iva_3 = 0 ";
                }
                if (para_borrar.Length > 0)
                {
                    db.Scalar(" update ctz_cotizacion_det set " + para_borrar + " where id_cotizacion = " + T_ID.Text);
                }
                db.Scalar(" update ctz_cotizacion set columnas_precio = '" + columnas + "' where id_cotizacion = " + T_ID.Text);
            }
            catch (Exception ex)
            {

            }
        }

        protected void B_ABRE_MODAL_CLIENTE_Click(object sender, EventArgs e)
        {
            LlenarClientesventa();
            ScriptManager.RegisterStartupScript(this, typeof(Page), "AbreModalcl", "<script>javascript:abremodalclientes();</script>", false);
            DIV_CARGAR_SIN_VENTA.Visible = true;
        }

        public void LlenarClientesventa()
        {
            try
            {
                usuarioEntity vend = new usuarioEntity();
                vend = (usuarioEntity)(Session["usuario"]);
                DBUtil db = new DBUtil();

                if (vend.cod_usuario == "Patricia" || vend.cod_usuario == "PC058")
                {
                    G_CLIENTES.DataSource = db.consultar("select rutcliente, [Nombre Cliente], [Periodo Ultima Factur.], [Monto Ultima Factur.], [Dias Diferencia], LC, isnull(disponible,0) as'disponible', ciudad, direccion, correo, giro  from V_CTZ_GESTION_VENTAS_FIN  where user1 in ('Abarrotes','MayoristasLV');");
                    G_CLIENTES.DataBind();
                    G_CLIENTES.Columns[2].Visible = true;
                }
                else
                {
                    G_CLIENTES.DataSource = db.consultar("select rutcliente, [Nombre Cliente], [Periodo Ultima Factur.], [Monto Ultima Factur.], [Dias Diferencia], LC, isnull(disponible,0) as'disponible', ciudad, direccion, correo, giro  from V_CTZ_GESTION_VENTAS_FIN where codvendedor = '" + vend.cod_usuario + "'");
                    G_CLIENTES.DataBind();
                }            
                UP_CLIENTESVENTA.Update();              
            }
            catch (Exception ex)
            {
                alert(ex.Message, "ERROR", "rojo", 3000);
            }
        }

        public void LlenarClientesSinventa()
        {
            try
            {
                usuarioEntity vend = new usuarioEntity();
                vend = (usuarioEntity)(Session["usuario"]);
                DBUtil db = new DBUtil();

                if (vend.cod_usuario == "Patricia" || vend.cod_usuario == "PC058")
                {
                    G_CLIENTES2.DataSource = db.consultar("select * from V_CTZ_GESTION_VENTAS_FIN_SINVENTA where SlsperId in (select distinct cod_usuario from usuariowebphone); ");
                    G_CLIENTES2.DataBind();
                    G_CLIENTES2.Columns[2].Visible = true;
                }
                else
                {
                    G_CLIENTES2.DataSource = db.consultar("select * from V_CTZ_GESTION_VENTAS_FIN_SINVENTA where SlsperId = '" + vend.cod_usuario + "'");
                    G_CLIENTES2.DataBind();
                }
                UP_CLIENTESSINVENTAS.Update();
            }
            catch (Exception ex)
            {
                alert(ex.Message, "ERROR", "rojo", 3000);
            }
            DIV_CARGAR_SIN_VENTA.Visible = false;
        }

        protected void B_TODOS_PRODUCTOS_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in G_PRODUCTOS.Rows)
            {
                CheckBox x = (CheckBox)row.FindControl("CHK_ENVIAR");
                x.Checked = true;
            }
        }

        protected void B_NINGUNO_PRODUCTOS_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in G_PRODUCTOS.Rows)
            {
                CheckBox x = (CheckBox)row.FindControl("CHK_ENVIAR");
                x.Checked = false;
            }
        }

        protected void B_TODOS_CATEGORIA_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in G_PRODUCTOS.Rows)
            {
                int id_cat = int.Parse((G_PRODUCTOS.DataKeys[Convert.ToInt32(row.RowIndex)].Values[2].ToString()));
                if (id_cat.ToString() == CB_CATEGORIAS.SelectedValue.ToString())
                {
                    CheckBox x = (CheckBox)row.FindControl("CHK_ENVIAR");
                    x.Checked = true;
                }
            }

            foreach (GridViewRow row in G_PRODUCTOS2.Rows)
            {
                int id_cat = int.Parse((G_PRODUCTOS2.DataKeys[Convert.ToInt32(row.RowIndex)].Values[2].ToString()));
                if (id_cat.ToString() == CB_CATEGORIAS.SelectedValue.ToString())
                {
                    CheckBox x = (CheckBox)row.FindControl("CHK_ENVIAR");
                    x.Checked = true;
                }
            }
        }
        protected void B_AGREGAR_PRODUCTOS_Click(object sender, EventArgs e)
        {
            try
            {
                DBUtil db = new DBUtil();
                ctz_cotizacion_detEntity det_ctz = new ctz_cotizacion_detEntity();
                int contador_repetidos = 0;
                bool agrego_producto = false;
                if (T_ID.Text == "")
                {
                    GuardarCotizacion(true);
                }
                foreach (GridViewRow row in G_PRODUCTOS.Rows)
                {
                    CheckBox x = (CheckBox)row.FindControl("CHK_ENVIAR");
                    if (x.Checked)
                    {
                        try
                        {
                            string cod_producto = G_PRODUCTOS.DataKeys[Convert.ToInt32(row.RowIndex)].Values[0].ToString();
                            if (int.Parse(db.Scalar("select count(1) from ctz_cotizacion_det where producto ='" + cod_producto + "' and id_cotizacion = " + T_ID.Text).ToString()) > 0)
                            {
                                contador_repetidos++;
                            }
                            else
                            {
                                string nom_producto = G_PRODUCTOS.DataKeys[Convert.ToInt32(row.RowIndex)].Values[1].ToString();
                                double precio_venta1 = double.Parse(G_PRODUCTOS.DataKeys[Convert.ToInt32(row.RowIndex)].Values[3].ToString());
                                double precio_venta2 = double.Parse(G_PRODUCTOS.DataKeys[Convert.ToInt32(row.RowIndex)].Values[4].ToString());
                                double precio_venta3 = double.Parse(G_PRODUCTOS.DataKeys[Convert.ToInt32(row.RowIndex)].Values[5].ToString());
                                double precio_venta1u = double.Parse(G_PRODUCTOS.DataKeys[Convert.ToInt32(row.RowIndex)].Values[6].ToString());
                                double precio_venta2u = double.Parse(G_PRODUCTOS.DataKeys[Convert.ToInt32(row.RowIndex)].Values[7].ToString());
                                double precio_venta3u = double.Parse(G_PRODUCTOS.DataKeys[Convert.ToInt32(row.RowIndex)].Values[8].ToString());
                                int unidad_equivale = int.Parse(G_PRODUCTOS.DataKeys[Convert.ToInt32(row.RowIndex)].Values[9].ToString());

                                det_ctz.id_cotizacion = int.Parse(T_ID.Text);
                                det_ctz.producto = cod_producto;
                                det_ctz.nom_producto = nom_producto;
                                int cont = 0;
                                foreach (ListItem item in CB_BODEGA.Items)
                                {
                                    if (item.Selected)
                                    {
                                        cont++;
                                        if (cont == 1)
                                        {
                                            det_ctz.cod_bodega_1 = item.Value;
                                        }
                                        if (cont == 2)
                                        {
                                            det_ctz.cod_bodega_2 = item.Value;
                                        }
                                        if (cont == 3)
                                        {
                                            det_ctz.cod_bodega_3 = item.Value;
                                        }
                                    }
                                }
                                if (cont == 1)
                                {
                                    det_ctz.cod_bodega_2 = "NO";
                                    det_ctz.cod_bodega_3 = "NO";
                                }
                                if (cont == 2)
                                {
                                    det_ctz.cod_bodega_3 = "NO";
                                }
                                det_ctz.cantidad_1 = 1;
                                det_ctz.cantidad_2 = 1;
                                det_ctz.cantidad_3 = 1;
                                det_ctz.precio_1 = precio_venta1;
                                det_ctz.precio_2 = precio_venta2;
                                det_ctz.precio_3 = precio_venta3;
                                det_ctz.precio_unitario_1 = precio_venta1u;
                                det_ctz.precio_unitario_2 = precio_venta2u;
                                det_ctz.precio_unitario_3 = precio_venta3u;
                                det_ctz.descuento_1 = 0;
                                det_ctz.descuento_2 = 0;
                                det_ctz.descuento_3 = 0;
                                det_ctz.precio_con_descuento_1 = precio_venta1;
                                det_ctz.precio_con_descuento_2 = precio_venta2;
                                det_ctz.precio_con_descuento_3 = precio_venta3;
                                det_ctz.precio_con_descuento_unitario_1 = precio_venta1u;
                                det_ctz.precio_con_descuento_unitario_2 = precio_venta2u;
                                det_ctz.precio_con_descuento_unitario_3 = precio_venta3u;
                                det_ctz.total_1 = precio_venta1;
                                det_ctz.total_2 = precio_venta2;
                                det_ctz.total_3 = precio_venta3;
                                det_ctz.total_iva_1 = precio_venta1 + ((precio_venta1 * 19) / 100);
                                det_ctz.total_iva_2 = precio_venta2 + ((precio_venta2 * 19) / 100);
                                det_ctz.total_iva_3 = precio_venta3 + ((precio_venta3 * 19) / 100);
                                det_ctz.unidad_equivale = unidad_equivale;
                                ctz_cotizacion_detBO.registrar(det_ctz);
                                agrego_producto = true;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                foreach (GridViewRow row in G_PRODUCTOS2.Rows)
                {
                    CheckBox x = (CheckBox)row.FindControl("CHK_ENVIAR");
                    if (x.Checked)
                    {
                        try
                        {
                            string cod_producto = G_PRODUCTOS2.DataKeys[Convert.ToInt32(row.RowIndex)].Values[0].ToString();
                            if (int.Parse(db.Scalar("select count(1) from ctz_cotizacion_det where producto ='" + cod_producto + "' and id_cotizacion = " + T_ID.Text).ToString()) > 0)
                            {
                                contador_repetidos++;
                            }
                            else
                            {
                                string nom_producto = G_PRODUCTOS2.DataKeys[Convert.ToInt32(row.RowIndex)].Values[1].ToString();
                                double precio_venta1 = double.Parse(G_PRODUCTOS2.DataKeys[Convert.ToInt32(row.RowIndex)].Values[3].ToString());
                                double precio_venta2 = double.Parse(G_PRODUCTOS2.DataKeys[Convert.ToInt32(row.RowIndex)].Values[4].ToString());
                                double precio_venta3 = double.Parse(G_PRODUCTOS2.DataKeys[Convert.ToInt32(row.RowIndex)].Values[5].ToString());
                                double precio_venta1u = double.Parse(G_PRODUCTOS2.DataKeys[Convert.ToInt32(row.RowIndex)].Values[6].ToString());
                                double precio_venta2u = double.Parse(G_PRODUCTOS2.DataKeys[Convert.ToInt32(row.RowIndex)].Values[7].ToString());
                                double precio_venta3u = double.Parse(G_PRODUCTOS2.DataKeys[Convert.ToInt32(row.RowIndex)].Values[8].ToString());
                                int unidad_equivale = int.Parse(G_PRODUCTOS2.DataKeys[Convert.ToInt32(row.RowIndex)].Values[9].ToString());

                                det_ctz.id_cotizacion = int.Parse(T_ID.Text);
                                det_ctz.producto = cod_producto;
                                det_ctz.nom_producto = nom_producto;
                                int cont = 0;
                                foreach (ListItem item in CB_BODEGA.Items)
                                {
                                    if (item.Selected)
                                    {
                                        cont++;
                                        if (cont == 1)
                                        {
                                            det_ctz.cod_bodega_1 = item.Value;
                                        }
                                        if (cont == 2)
                                        {
                                            det_ctz.cod_bodega_2 = item.Value;
                                        }
                                        if (cont == 3)
                                        {
                                            det_ctz.cod_bodega_3 = item.Value;
                                        }
                                    }
                                }
                                if (cont == 1)
                                {
                                    det_ctz.cod_bodega_2 = "NO";
                                    det_ctz.cod_bodega_3 = "NO";
                                }
                                if (cont == 2)
                                {
                                    det_ctz.cod_bodega_3 = "NO";
                                }
                                det_ctz.cantidad_1 = 1;
                                det_ctz.cantidad_2 = 1;
                                det_ctz.cantidad_3 = 1;
                                det_ctz.precio_1 = precio_venta1;
                                det_ctz.precio_2 = precio_venta2;
                                det_ctz.precio_3 = precio_venta3;
                                det_ctz.precio_unitario_1 = precio_venta1u;
                                det_ctz.precio_unitario_2 = precio_venta2u;
                                det_ctz.precio_unitario_3 = precio_venta3u;
                                det_ctz.descuento_1 = 0;
                                det_ctz.descuento_2 = 0;
                                det_ctz.descuento_3 = 0;
                                det_ctz.precio_con_descuento_1 = precio_venta1;
                                det_ctz.precio_con_descuento_2 = precio_venta2;
                                det_ctz.precio_con_descuento_3 = precio_venta3;
                                det_ctz.precio_con_descuento_unitario_1 = precio_venta1u;
                                det_ctz.precio_con_descuento_unitario_2 = precio_venta2u;
                                det_ctz.precio_con_descuento_unitario_3 = precio_venta3u;
                                det_ctz.total_1 = precio_venta1;
                                det_ctz.total_2 = precio_venta2;
                                det_ctz.total_3 = precio_venta3;
                                det_ctz.total_iva_1 = precio_venta1 + ((precio_venta1 * 19) / 100);
                                det_ctz.total_iva_2 = precio_venta2 + ((precio_venta2 * 19) / 100);
                                det_ctz.total_iva_3 = precio_venta3 + ((precio_venta3 * 19) / 100);
                                det_ctz.unidad_equivale = unidad_equivale;
                                ctz_cotizacion_detBO.registrar(det_ctz);
                                agrego_producto = true;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                if (agrego_producto)
                {
                    try
                    {
                        DataTable dt_serv = new DataTable();
                        dt_serv = db.consultar("select * from ctz_cotizaciones_servicios where id_cotizacion = " + T_ID.Text + " and tipo_servicio = 'AGREGADO A LOS PRODUCTOS';");
                        if (dt_serv.Rows.Count > 0)
                        {
                            db.Scalar("delete from ctz_cotizaciones_servicios where tipo_servicio = 'AGREGADO A LOS PRODUCTOS' and id_cotizacion = " + T_ID.Text + " ; ");
                            G_SERVICIOS.DataSource = ctz_cotizaciones_serviciosBO.GetAll(" where id_cotizacion = " + T_ID.Text);
                            G_SERVICIOS.DataBind();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                LlenarDetalle(int.Parse(T_ID.Text));
                alert("Agregados con éxito.", "Productos", "verde", 2000);
                CalcularTotales(det_ctz.id_cotizacion);
                UP_LISTA.Update();
            }
            catch (Exception ex)
            {
                alert("Ocurrió un problema al agregar los productos", "Error", "rojo", 2000);
            }
        }

        protected void B_MARCAR_TODOS_CLIENTES_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in G_CLIENTES.Rows)
            {
                CheckBox x = (CheckBox)row.FindControl("CHK_ENVIAR");
                x.Checked = true;
            }
        }

        protected void B_DESMARCAR_TODOS_CLIENTES_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in G_CLIENTES.Rows)
            {
                CheckBox x = (CheckBox)row.FindControl("CHK_ENVIAR");
                x.Checked = false;
            }
        }

        protected void G_SERVICIOS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Borrar")
                {
                    DBUtil db = new DBUtil();
                    int id_det = int.Parse((G_SERVICIOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString()));
                    int id_cotizacion = int.Parse((G_SERVICIOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString()));
                    int valor_servicio = int.Parse((G_SERVICIOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString()));

                    ctz_cotizaciones_serviciosEntity det_ctz = new ctz_cotizaciones_serviciosEntity();
                    det_ctz.id_cotizacion_servicio = id_det;
                    if (ctz_cotizaciones_serviciosBO.encontrar(ref det_ctz) == "OK")
                    {
                        if (det_ctz.tipo_servicio == "AGREGADO A LOS PRODUCTOS")
                        {
                            // RESTAR EL VALOR A CADA PRODUCTO
                            RestarValorProductos(det_ctz.id_cotizacion, valor_servicio);
                            //
                        }
                        if (ctz_cotizaciones_serviciosBO.eliminar(det_ctz) == "OK")
                        {
                            alert("Retirado con éxito", "Servicio", "rojo", 2000);
                            G_SERVICIOS.DataSource = ctz_cotizaciones_serviciosBO.GetAll(" where id_cotizacion = " + T_ID.Text);
                            G_SERVICIOS.DataBind();
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void B_AGREGAR_SERVICIO_Click(object sender, EventArgs e)
        {
            if (T_ID.Text == "")
            {
                GuardarCotizacion(true);
            }
            if (T_PRECIO_SERVICIO.Text != "")
            {
                try
                {
                    ctz_cotizaciones_serviciosEntity ctz_det = new ctz_cotizaciones_serviciosEntity();
                    ctz_det.id_cotizacion = int.Parse(T_ID.Text);
                    ctz_det.nombre_servicio = CB_SERVICIO.SelectedItem.Text;
                    ctz_det.valor_servicio = int.Parse(T_PRECIO_SERVICIO.Text);
                    ctz_det.cod_servicio = CB_SERVICIO.SelectedValue;
                    if (RB_SEPARAR.Checked)
                    {
                        ctz_det.tipo_servicio = "POR SEPARADO";
                    }
                    else if (RB_NOSEPARAR.Checked)
                    {
                        ctz_det.tipo_servicio = "AGREGADO A LOS PRODUCTOS";
                    }
                    if (ctz_cotizaciones_serviciosBO.registrar(ctz_det) == "OK")
                    {
                        if (ctz_det.tipo_servicio == "AGREGADO A LOS PRODUCTOS")
                        {
                            AgregarValorProductos(ctz_det.id_cotizacion, ctz_det.valor_servicio);
                        }
                        G_SERVICIOS.DataSource = ctz_cotizaciones_serviciosBO.GetAll(" where id_cotizacion = " + T_ID.Text);
                        G_SERVICIOS.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    alert(ex.Message, "Error", "rojo", 3000);
                }
            }
            else
            {
                alert("Ingrese un valor para el servicio", "Valor", "rojo", 2000);
            }
        }

        public void AgregarValorProductos(int id_ctz, double valor)
        {
            try
            {
                DBUtil db = new DBUtil();
                db.StorageProcedure("exec RecalcularCTZ " + id_ctz + "," + valor + ";").ToString();
                alert("Se agregó el valor a c/u de los productos", "Realizado con éxito", "verde", 3000);
                LlenarDetalle(id_ctz);
            }
            catch (Exception ex)
            {
                alert("Error al calcular", "Error", "rojo", 3000);
            }
        }

        public void RestarValorProductos(int id_ctz, double valor)
        {
            try
            {
                DBUtil db = new DBUtil();
                db.StorageProcedure("exec RecalcularCTZ2 " + id_ctz + "," + valor + ";").ToString();
                alert("Se restó el valor a c/u de los productos", "Realizado con éxito", "verde", 3000);
                LlenarDetalle(id_ctz);
            }
            catch (Exception ex)
            {
                alert("Error al calcular", "Error", "rojo", 3000);
            }
        }

        protected void B_ENVIAR_CORREO_Click(object sender, EventArgs e)
        {
            //if (T_PASS_CORREO_VENDEDOR.Text != "")
            //{
            List<correos_pdf> correos_pdf = new List<correos_pdf>();
            DBUtil db = new DBUtil();
            if (T_ID.Text == "")
            {
                GuardarCotizacion(true);
            }
            foreach (GridViewRow row in G_CLIENTES.Rows)
            {
                CheckBox x = (CheckBox)row.FindControl("CHK_ENVIAR");
                if (x.Checked)
                {
                    try
                    {
                        string rut_cliente = G_CLIENTES.DataKeys[Convert.ToInt32(row.RowIndex)].Values[0].ToString();
                        string correo_cliente = G_CLIENTES.DataKeys[Convert.ToInt32(row.RowIndex)].Values[1].ToString();
                        string nombre_cliente = G_CLIENTES.DataKeys[Convert.ToInt32(row.RowIndex)].Values[2].ToString();
                        string ciudad = G_CLIENTES.DataKeys[Convert.ToInt32(row.RowIndex)].Values[3].ToString();
                        string direccion = G_CLIENTES.DataKeys[Convert.ToInt32(row.RowIndex)].Values[4].ToString();
                        string giro = G_CLIENTES.DataKeys[Convert.ToInt32(row.RowIndex)].Values[5].ToString();
                        correos_pdf cp = new correos_pdf();
                        cp.correocliente = correo_cliente;
                        cp.rutcliente = rut_cliente;
                        cp.nombrecliente = nombre_cliente;
                        cp.ciudad = ciudad;
                        cp.direccion = direccion;
                        cp.giro = giro;
                        cp.enviado_ok = false;
                        correos_pdf.Add(cp);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            foreach (GridViewRow row in G_CLIENTES2.Rows)
            {
                CheckBox x = (CheckBox)row.FindControl("CHK_ENVIAR");
                if (x.Checked)
                {
                    try
                    {
                        string rut_cliente = G_CLIENTES2.DataKeys[Convert.ToInt32(row.RowIndex)].Values[0].ToString();
                        string correo_cliente = G_CLIENTES2.DataKeys[Convert.ToInt32(row.RowIndex)].Values[1].ToString();
                        string nombre_cliente = G_CLIENTES2.DataKeys[Convert.ToInt32(row.RowIndex)].Values[2].ToString();
                        string ciudad = G_CLIENTES2.DataKeys[Convert.ToInt32(row.RowIndex)].Values[3].ToString();
                        string direccion = G_CLIENTES2.DataKeys[Convert.ToInt32(row.RowIndex)].Values[4].ToString();
                        string giro = " ";
                        correos_pdf cp = new correos_pdf();
                        cp.correocliente = correo_cliente;
                        cp.rutcliente = rut_cliente;
                        cp.nombrecliente = nombre_cliente;
                        cp.ciudad = ciudad;
                        cp.direccion = direccion;
                        cp.giro = giro;
                        cp.enviado_ok = false;
                        correos_pdf.Add(cp);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            if (T_NOREGISTRADOS_NOMBRE.Text != "" && T_NOREGISTRADOS_CORREO.Text != "")
            {
                correos_pdf cp = new correos_pdf();
                cp.correocliente = T_NOREGISTRADOS_CORREO.Text.Replace(" ", "");
                cp.rutcliente = "0";
                cp.nombrecliente = T_NOREGISTRADOS_NOMBRE.Text;
                cp.ciudad = " ";
                cp.direccion = " ";
                cp.giro = " ";
                cp.enviado_ok = false;
                correos_pdf.Add(cp);
            }
            enc_correo_pdf enc_correo = new enc_correo_pdf();
            enc_correo.ASUNTO = T_ASUNTO_CORREO.Text;
            enc_correo.CC = T_CC_CORREO.Text.Replace(" ", "");
            enc_correo.COMENTARIO = T_COMENTARIO_CORREO.Text;
            enc_correo.PASS_CORREO = "";
            Session["ENC_CORREO_PDF"] = enc_correo;
            Session["ctz_correos_pdf"] = correos_pdf;
            ScriptManager.RegisterStartupScript(this, typeof(Page), "enviarcotizacionpdf", "<script>javascript:EnviarCorreoClientes();</script>", false);
            //}
            //else
            //{
            //    alert("Ingrese la contraseña de su correo electrónico", "Enviar Correo", "rojo", 3000);
            //}            
        }

        protected void det_cantidad_1_TextChanged(object sender, EventArgs e)
        {
            CalcularRow(sender, e, 1);
        }
        protected void det_precio_1_TextChanged(object sender, EventArgs e)
        {
            CalcularRow(sender, e, 1);
        }
        protected void det_descuento_1_TextChanged(object sender, EventArgs e)
        {
            CalcularRow(sender, e, 1);
        }
        protected void det_cantidad_2_TextChanged(object sender, EventArgs e)
        {
            CalcularRow(sender, e, 2);
        }
        protected void det_precio_2_TextChanged(object sender, EventArgs e)
        {
            CalcularRow(sender, e, 2);
        }
        protected void det_descuento_2_TextChanged(object sender, EventArgs e)
        {
            CalcularRow(sender, e, 2);
        }
        protected void det_cantidad_3_TextChanged(object sender, EventArgs e)
        {
            CalcularRow(sender, e, 3);
        }
        protected void det_precio_3_TextChanged(object sender, EventArgs e)
        {
            CalcularRow(sender, e, 3);
        }
        protected void det_descuento_3_TextChanged(object sender, EventArgs e)
        {
            CalcularRow(sender, e, 3);
        }
        public void CalcularRow(object sender, EventArgs e, int num_columna)
        {
            try
            {

                // INICIALIZAR
                DBUtil db = new DBUtil();
                TextBox txt = (TextBox)sender;
                GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;

                //Datakeys
                int id_det = int.Parse((G_DETALLE_COTIZACION.DataKeys[Convert.ToInt32(gvRow.RowIndex)].Values[0].ToString()));
                int id_cotizacion = int.Parse((G_DETALLE_COTIZACION.DataKeys[Convert.ToInt32(gvRow.RowIndex)].Values[1].ToString()));
                string cod_producto = (G_DETALLE_COTIZACION.DataKeys[Convert.ToInt32(gvRow.RowIndex)].Values[2].ToString());

                // Textbox
                TextBox txt_precio = (TextBox)gvRow.FindControl("det_precio_" + num_columna);
                TextBox txt_precio_unitario = (TextBox)gvRow.FindControl("precio_unitario_" + num_columna);
                TextBox txt_descuento = (TextBox)gvRow.FindControl("det_descuento_" + num_columna);
                TextBox txt_precio_desc = (TextBox)gvRow.FindControl("det_precio_descuento_" + num_columna);
                TextBox txt_precio_unit_desc = (TextBox)gvRow.FindControl("precio_unitario_desc_" + num_columna);
                TextBox txt_total = (TextBox)gvRow.FindControl("det_total_" + num_columna);
                TextBox txt_total_iva = (TextBox)gvRow.FindControl("total_iva_" + num_columna);
                TextBox txt_cantidad = (TextBox)gvRow.FindControl("det_cantidad_" + num_columna);

                // Valores
                double precio = double.Parse(txt_precio.Text);
                double precio_unit = double.Parse(txt_precio_unitario.Text);
                double descuento = double.Parse(txt_descuento.Text.Replace(".", ","));


                // VERIFICAR QUE EL PRECIO NO SEA MENOR
                string cod_bodega = db.Scalar("select cod_bodega_" + num_columna + " from CTZ_Cotizacion_det where id_cotizacion_det = " + id_det).ToString();
                double precio_actual = 0;
                if (cod_bodega == "ARICA")
                {
                    precio_actual = double.Parse(db.Scalar(" select top(1)arica_vent from stock_excel_2 where fecha = (select MAX(fecha) from stock_excel_2) and cod_producto = '" + cod_producto + "' ").ToString());
                }
                else if (cod_bodega == "ETREDIRECT")
                {
                    precio_actual = double.Parse(db.Scalar(" select top(1)entrega_directa_V_RM from stock_excel_2 where fecha = (select MAX(fecha) from stock_excel_2) and cod_producto = '" + cod_producto + "' ").ToString());
                }
                else if (cod_bodega == "IQUIQUE")
                {
                    precio_actual = double.Parse(db.Scalar(" select top(1)iquique from stock_excel_2 where fecha = (select MAX(fecha) from stock_excel_2) and cod_producto = '" + cod_producto + "' ").ToString());
                }
                else if (cod_bodega == "LOSANGELES")
                {
                    precio_actual = double.Parse(db.Scalar(" select top(1)bod_LA_vent from stock_excel_2 where fecha = (select MAX(fecha) from stock_excel_2) and cod_producto = '" + cod_producto + "' ").ToString());
                }
                else if (cod_bodega == "LZ_DESPACHO")
                {
                    precio_actual = double.Parse(db.Scalar(" select top(1)reparto_RM_V from stock_excel_2 where fecha = (select MAX(fecha) from stock_excel_2) and cod_producto = '" + cod_producto + "' ").ToString());
                }
                else if (cod_bodega == "ZARATE")
                {
                    precio_actual = double.Parse(db.Scalar(" select top(1)bod_LZ_vent from stock_excel_2 where fecha = (select MAX(fecha) from stock_excel_2) and cod_producto = '" + cod_producto + "' ").ToString());
                }
                if (precio < precio_actual)
                {
                    precio = precio_actual;
                    txt_precio.Text = precio.ToString();
                }
                //
                if (descuento > 2)
                {
                    alert("El descuento no puede ser mayor a 2%", "Descuento inválido", "rojo", 3000);
                    txt_descuento.Text = "2";
                    descuento = 2;
                }

                int precio_con_descuento = Convert.ToInt32(precio - ((precio * descuento) / 100));
                int precio_unit_con_descuento = Convert.ToInt32(precio_unit - ((precio_unit * descuento) / 100));
                int cantidad = int.Parse(txt_cantidad.Text);
                int total = Convert.ToInt32(precio_con_descuento * cantidad);
                int total_iva = Convert.ToInt32(((total * 19) / 100) + total);
                txt_total.Text = total.ToString("#,##0");
                txt_total_iva.Text = total_iva.ToString("#,##0");
                txt_precio_desc.Text = precio_con_descuento.ToString("#,##0");
                txt_precio_unit_desc.Text = precio_unit_con_descuento.ToString("#,##0");

                // Guardar en BD Automaticamente              
                ctz_cotizacion_detEntity ctz_det = new ctz_cotizacion_detEntity();
                ctz_det.id_cotizacion_det = id_det;

                if (ctz_cotizacion_detBO.encontrar(ref ctz_det) == "OK")
                {
                    if (num_columna == 1)
                    {
                        ctz_det.precio_1 = precio;
                        ctz_det.total_1 = total;
                        ctz_det.total_iva_1 = total_iva;
                        ctz_det.precio_con_descuento_1 = precio_con_descuento;
                        ctz_det.precio_con_descuento_unitario_1 = precio_unit_con_descuento;
                        ctz_det.descuento_1 = descuento;
                        ctz_det.cantidad_1 = cantidad;
                        ctz_cotizacion_detBO.actualizar(ctz_det);
                    }
                    else if (num_columna == 2)
                    {
                        ctz_det.precio_2 = precio;
                        ctz_det.total_2 = total;
                        ctz_det.total_iva_2 = total_iva;
                        ctz_det.precio_con_descuento_2 = precio_con_descuento;
                        ctz_det.precio_con_descuento_unitario_2 = precio_unit_con_descuento;
                        ctz_det.descuento_2 = descuento;
                        ctz_det.cantidad_2 = cantidad;
                        ctz_cotizacion_detBO.actualizar(ctz_det);
                    }
                    else if (num_columna == 3)
                    {
                        ctz_det.precio_3 = precio;
                        ctz_det.total_3 = total;
                        ctz_det.total_iva_3 = total_iva;
                        ctz_det.precio_con_descuento_3 = precio_con_descuento;
                        ctz_det.precio_con_descuento_unitario_3 = precio_unit_con_descuento;
                        ctz_det.descuento_3 = descuento;
                        ctz_det.cantidad_3 = cantidad;
                        ctz_cotizacion_detBO.actualizar(ctz_det);
                    }
                    focus_control(txt);
                    CalcularTotales(id_cotizacion);

                }
            }
            catch (Exception ex)
            {

            }
        }

        public void CalcularTotales(int id_cotizacion)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "calculartotales", "<script>javascript:calcular_totales(" + id_cotizacion + ");</script>", false);
        }

        [WebMethod]
        public static string calculartotalajax(string x)
        {
            try
            {
                DBUtil db = new DBUtil();
                DataTable dt = new DataTable();

                string sql = "";
                sql += " select bod1.nombre_bodega as nom1, bod2.nombre_bodega as nom2, bod3.nombre_bodega as nom3,  sum(total_1) as 'total_1', sum(total_2) as 'total_2' , sum(total_3) as 'total_3'  " +
                    " ,sum(total_iva_1) as 'total_iva_1', sum(total_iva_2) as 'total_iva_2', sum(total_iva_3) as 'total_iva_3' " +
                    " from CTZ_Cotizacion_det ctz left join CTZ_Bodegas bod1 on ctz.cod_bodega_1 = bod1.cod_bodega " +
                    " left join CTZ_Bodegas bod2 on ctz.cod_bodega_2 = bod2.cod_bodega " +
                    " left join CTZ_Bodegas bod3 on ctz.cod_bodega_3 = bod3.cod_bodega " +
                    " where id_cotizacion = " + x +
                    " group by bod1.nombre_bodega, bod2.nombre_bodega,bod3.nombre_bodega ";
                dt = db.consultar(sql);

                if (dt.Rows.Count > 0)
                {
                    string html = "";
                    string html_final = "";
                    string html_espacio = "";
                    int contador_aux = 0;
                    for (int i = 1; i <= 3; i++)
                    {
                        try
                        {
                            int total = int.Parse(dt.Rows[0]["total_" + i].ToString());
                            int totaliva = int.Parse(dt.Rows[0]["total_iva_" + i].ToString());
                            if (dt.Rows[0]["nom" + i].ToString() != "NO" && dt.Rows[0]["nom" + i].ToString() != "")
                            {
                                html += "<div class='col-md-3'>";
                                html += "<p class='lead'>" + dt.Rows[0]["nom" + i].ToString() + "</p>";
                                html += "<table class='table table-sm compact' style='width:100%'>";
                                html += "<tr>";
                                html += "   <td><b>TOTAL: </b></td>";
                                html += "   <td class='text-right'>$ " + total.ToString("#,##0") + "</td>";
                                html += "</tr>";
                                html += "<tr class='bg-grey bg-lighten-4'>";
                                html += "   <td class='text-bold-800'>TOTAL + IVA:</td>";
                                html += "   <td class='text-bold-800 text-right'>$ " + totaliva.ToString("#,##0") + "</td>";
                                html += "</tr>";
                                html += "</table>";
                                html += "</div>";
                                contador_aux++;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    if (contador_aux == 1)
                    {
                        html_espacio += "<div class='col-sm-9'>&nbsp;</div>";
                    }
                    else if (contador_aux == 2)
                    {
                        html_espacio += "<div class='col-sm-6'>&nbsp;</div>";
                    }
                    else if (contador_aux == 3)
                    {
                        html_espacio += "<div class='col-sm-3'>&nbsp;</div>";
                    }
                    html_final = html_espacio + html;
                    return html_final;
                }
                else
                {
                    return "Error al calcular";
                }

            }
            catch (Exception ex)
            {
                return "Error : " + ex.Message.ToString();
            }
        }

        public void focus_control(TextBox x)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "aasdfcscfdadsc", "<script>javascript:focusonme('" + x.ClientID + "');</script>", false);
        }

        protected void B_MARCAR_TODOS_CLIENTES2_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in G_CLIENTES2.Rows)
            {
                CheckBox x = (CheckBox)row.FindControl("CHK_ENVIAR");
                x.Checked = true;
            }
        }

        protected void B_DESMARCAR_TODOS_CLIENTES2_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in G_CLIENTES2.Rows)
            {
                CheckBox x = (CheckBox)row.FindControl("CHK_ENVIAR");
                x.Checked = false;
            }
        }

        protected void B_CARGAR_SIN_VENTAS_Click(object sender, EventArgs e)
        {
            LlenarClientesSinventa();
        }

        private void cargar_grilla_contacto()
        {
            DataTable dt = new DataTable();
            DBUtil db = new DBUtil();
            G_CONTACTOS.DataSource = db.consultar("select * from contactosvend where rutcliente = '" + t_rut_cliente_contact.Text + "'");
            G_CONTACTOS.DataBind();
        }

        protected void G_CLIENTES_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //editar
                if (e.CommandName == "EditarCliente")
                {
                    string rut_cliente = G_CLIENTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                    string nombre_cliente = G_CLIENTES.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString();
                    MODA_CONTACTO_NOMBRE_CLIENTE.InnerHtml = "<h4 class='form-section'><i class='fa fa-user text-warning'></i> Cliente: " + nombre_cliente + " </h4>";
                    t_rut_cliente_contact.Text = rut_cliente;
                    viene_de.Text = "CONVENTA";
                    cargar_grilla_contacto();
                    DIV_DETALLE_CONTACTO.Visible = false;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "AbreModal", "<script>javascript:ABREMODALEDITARCLIENTES();</script>", false);
                    UP_CONTACTOS.Update();
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void G_CLIENTES2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //editar
                if (e.CommandName == "EditarCliente")
                {
                    string rut_cliente = G_CLIENTES2.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                    string nombre_cliente = G_CLIENTES2.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString();
                    MODA_CONTACTO_NOMBRE_CLIENTE.InnerHtml = "<h4 class='form-section'><i class='fa fa-user text-warning'></i> Cliente: " + nombre_cliente + " </h4>";
                    t_rut_cliente_contact.Text = rut_cliente;
                    viene_de.Text = "SINVENTA";
                    cargar_grilla_contacto();
                    DIV_DETALLE_CONTACTO.Visible = false;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "AbreModal", "<script>javascript:ABREMODALEDITARCLIENTES();</script>", false);
                    UP_CONTACTOS.Update();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public string confirmDelete2(string Name)
        {
            return @"javascript:if(!confirm('Esta acción va eliminar el CONTACTO: "
               + Name.ToUpper()
               + @". ¿Estás seguro?')){return false;} ;";
        }

        protected void B_CREAR_NUEVO_CONTACTO_Click(object sender, EventArgs e)
        {
            DIV_DETALLE_CONTACTO.Visible = true;
            LBL_NOMBRE_CONTACTO.Text = "Creando un nuevo contacto";
            limpiardetallecontacto();
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
                    alert("Contacto Eliminado", "Contacto", "verde", 3000);
                    if (viene_de.Text == "CONVENTA")
                    {
                        LlenarClientesventa();
                    }
                    else
                    {
                        LlenarClientesSinventa();
                    }
                }
                else
                {
                    alert("No se pudo eliminar el contacto.", "Contacto", "rojo", 3000);
                }
                cargar_grilla_contacto();
            }
            if (e.CommandName == "Editar")
            {
                string rutcliente = G_CONTACTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                string nombre_contacto = G_CONTACTOS.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString();
                contactosvendEntity cont = new contactosvendEntity();
                cont.rutcliente = rutcliente;
                cont.nombre_contacto = nombre_contacto;
                string ok = contactosvendBO.encontrar(ref cont);
                t_nombre_contact.Text = cont.nombre_contacto;
                t_nombre_contact.ReadOnly = true;
                t_cargo_contact.Text = cont.cargo;
                t_num_contact.Text = cont.numero;
                t_correo_contact.Text = cont.correo;
                t_direcc_contact.Text = cont.direccion;
                LBL_NOMBRE_CONTACTO.Text = "Editando contacto: " + nombre_contacto;
                DIV_DETALLE_CONTACTO.Visible = true;
            }
        }

        public void limpiardetallecontacto()
        {
            t_nombre_contact.Text = "";
            t_nombre_contact.ReadOnly = false;
            t_cargo_contact.Text = "";
            t_num_contact.Text = "";
            t_correo_contact.Text = "";
            t_direcc_contact.Text = "";
        }

        protected void B_GUARDAR_CONTACTO_Click(object sender, EventArgs e)
        {
            if (t_nombre_contact.Text == "")
            {
                alert("El nombre no puede estar vacío", "Completar los campos", "rojo", 3000);
            }
            else if (t_cargo_contact.Text == "")
            {
                alert("El cargo no puede estar vacío", "Completar los campos", "rojo", 3000);
            }
            else if (t_num_contact.Text == "")
            {
                alert("El número no puede estar vacío", "Completar los campos", "rojo", 3000);
            }
            else if (t_correo_contact.Text == "")
            {
                alert("El correo no puede estar vacío", "Completar los campos", "rojo", 3000);
            }
            else if (t_direcc_contact.Text == "")
            {
                alert("La comuna no puede estar vacío", "Completar los campos", "rojo", 3000);
            }
            else
            {
                if (t_nombre_contact.ReadOnly)
                {
                    // EDITAR
                    contactosvendEntity cont = new contactosvendEntity();
                    cont.rutcliente = t_rut_cliente_contact.Text;
                    cont.nombre_contacto = t_nombre_contact.Text;
                    if (contactosvendBO.encontrar(ref cont) == "OK")
                    {
                        cont.cargo = t_cargo_contact.Text;
                        cont.numero = t_num_contact.Text;
                        cont.correo = t_correo_contact.Text;
                        cont.direccion = t_direcc_contact.Text;
                        if (contactosvendBO.actualizar(cont) == "OK")
                        {
                            alert("Contacto modificado con éxito", "Contacto", "verde", 3000);
                            if (viene_de.Text == "CONVENTA")
                            {
                                LlenarClientesventa();
                            }
                            else
                            {
                                LlenarClientesSinventa();
                            }
                            cargar_grilla_contacto();
                          
                        }
                        else
                        {
                            alert("No se pudo modificar el contacto.", "Contacto", "rojo", 3000);
                        }
                    }
                }
                else
                {
                    // NUEVO          
                    contactosvendEntity cont = new contactosvendEntity();
                    cont.rutcliente = t_rut_cliente_contact.Text;
                    cont.nombre_contacto = t_nombre_contact.Text;
                    cont.cargo = t_cargo_contact.Text;
                    cont.numero = t_num_contact.Text;
                    cont.correo = t_correo_contact.Text;
                    cont.direccion = t_direcc_contact.Text;
                    if (contactosvendBO.registrar(cont) == "OK")
                    {
                        alert("Contacto creado con éxito", "Contacto", "verde", 3000);
                        limpiardetallecontacto();
                        cargar_grilla_contacto();
                        if (viene_de.Text == "CONVENTA")
                        {
                            LlenarClientesventa();
                        }
                        else
                        {
                            LlenarClientesSinventa();
                        }
                    }
                    else
                    {
                        alert("No se pudo crear el contacto.", "Contacto", "rojo", 3000);
                    }
                }
            }
        }
    }
}