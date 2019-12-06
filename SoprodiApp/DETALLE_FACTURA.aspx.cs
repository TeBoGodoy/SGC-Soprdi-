using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoprodiApp.negocio;
using SoprodiApp.entidad;
using System.Text;
using System.Globalization;
using System.Drawing;
using System.Diagnostics;

using Word = Microsoft.Office.Interop.Word;
using System.IO;
using System.Configuration;

namespace SoprodiApp
{
    public partial class DETALLE_FACTURA : System.Web.UI.Page
    {
        public static int cont_periodos;
        public static DataTable aux;
        private static string USER;
        public static double monto = 0;
        public static double monto_almacenamiento = 0;
        public static int cont_periodos_;
        public static bool es_reporte_vendedor = false;
        public static bool es_reporte_cliente = false;
        public static bool almacenamiento_cobro = false;
        public static bool almacenamiento_cobro_dias = false;
        public static bool header_sum2;
        public static bool header_total2;
        public static string vendedor1;
        public static string cliente;
        public static int COLUMNA_DE_FACTURA;
        public static bool busca_columna_fac;
        public static bool columna_fac;
        public static bool es_ficha;
        public static bool es_costo_import;
        public static bool es_historial_compra;
        public static bool es_historial_total;
        public static bool es_venta_movil;
        public static bool es_matriz;
        public static bool es_nuevo_click_montos;
        public static bool es_permanencia;
        public static int cont_det;
        public static double sum_total;
        public static string fecha;

        double monto_cos = 0;
        double monto_import = 0;
        double monto_total = 0;

        protected void Page_Load(object sender, EventArgs e)
        {


            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.descarga_word);
            scriptManager.RegisterPostBackControl(this.descarga_pdf);
     
            if (!IsPostBack)
            {

                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                es_costo_import = false;

                USER = User.Identity.Name.ToString();
                fecha = encriptador.DecryptData(Request.QueryString["F"].ToString().Replace(" ", "+"));
                string vendedor = encriptador.DecryptData(Request.QueryString["V"].ToString().Replace(" ", "+"));
                string grupo = encriptador.DecryptData(Request.QueryString["G"].ToString().Replace(" ", "+"));
                string bit = encriptador.DecryptData(Request.QueryString["i"].ToString().Replace(" ", "+"));

                if (bit != "6")
                {
                    normal.Attributes["style"] = "display:block";
                    factura.Attributes["style"] = "display:none";
                    cargar_detalle(fecha, vendedor.Trim(), grupo.Trim(), bit);
                    monto = 0;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp", "<script language='javascript'>superfiltro();</script>", false);
                }
                else
                {
                    normal.Attributes["style"] = "display:none";
                    factura.Attributes["style"] = "display:block";

                    num_factura.Text = fecha.Trim();
                    DataTable dt = ReporteRNegocio.trae_encabezado(fecha.Trim());
                    foreach (DataRow r in dt.Rows)
                    {
                        fecha_factura.Text = r[1].ToString();
                        vendedor_.Text = r[2].ToString().Trim();
                        codigo_vend.Text = "COD: " + r[1].ToString().Trim();
                        grupo_.Text = "GRUPO: " + r[0].ToString().Trim();
                        cliente_.Text = r[7].ToString().Trim();

                        string rut_ini = r[6].ToString().Trim().Substring(0, r[6].ToString().Trim().Length - 1);
                        double rut = double.Parse(rut_ini);

                        rut_cliente.Text = "RUN: " + rut.ToString("N0") + "-" + r[6].ToString().Trim().Substring(r[6].ToString().Trim().Length - 1);

                    }
                    cont_det = 1;
                    sum_total = 0;
                    G_PRODUCTOS.DataSource = ReporteRNegocio.detalle_Factura(num_factura.Text);
                    G_PRODUCTOS.DataBind();
                    subtotal.Text = sum_total.ToString("N0");
                    total_.Text = sum_total.ToString("N0");
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp", "<script language='javascript'>superfiltro2();</script>", false);

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('G_PRODUCTOS')); </script>", false);

                }


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

        private void cargar_detalle(string fecha, string vendedor, string grupo, string bit)
        {
            DateTime t = DateTime.Now;
            DateTime t2 = DateTime.Now;
            t = new DateTime(t.Year, t.Month, 1);
            string DESDE = t.ToShortDateString();
            string HASTA = t2.ToShortDateString();

            DESDE = DESDE.Replace("-", "/");
            HASTA = HASTA.Replace("-", "/");

            bool es_producto_grilla = false;
            string where = " where 1=1 ";
            es_matriz = false;
            if (fecha == "")
            {
                detalle.InnerText = "Detalle vendedor " + ReporteRNegocio.nombre_vendedor(vendedor.Trim()) + " (desde " + DESDE + " hasta " + HASTA + ")";


                where += " and FechaFactura >= CONVERT(datetime,'" + DESDE + "', 103) " +
                              " and FechaFactura <= CONVERT(datetime,'" + HASTA + "',103) and codvendedor = '" + vendedor.Trim() + "'";
            }
            else if (!fecha.Contains("/"))
            {
                where += " and codvendedor = '" + vendedor.Trim() + "' and periodo = " + fecha.Substring(0, 6);
                detalle.InnerText = "Detalle vendedor " + ReporteRNegocio.nombre_vendedor(vendedor.Trim()) + "  (" + fecha + ")";
            }
            else if (fecha.Contains("*"))

            {
                string desde2 = fecha.Split('*')[0];
                string hasta2 = fecha.Split('*')[1];
                where = " where producto = '" + vendedor + "' and FechaFactura >= CONVERT(datetime,'" + desde2 + "', 103)  and FechaFactura <= CONVERT(datetime,'" + hasta2 + "',103) ";

                detalle.InnerText = "Detalle de Matriz de Ventas  (Cliente " + ReporteRNegocio.nombre_cliente(grupo) + "  Producto " + ReporteRNegocio.nombre_producto(vendedor) + ")";


            }
            else
            {

                where += " and codvendedor = '" + vendedor.Trim() + "' and convert(varchar, fechafactura, 103) = '" + fecha + "'";
                detalle.InnerText = "Detalle vendedor " + ReporteRNegocio.nombre_vendedor(vendedor.Trim()) + "  (" + fecha + ")";
            }
            es_nuevo_click_montos = false;
            es_reporte_vendedor = false;
            es_reporte_cliente = false;
            busca_columna_fac = true;
            columna_fac = false;
            es_ficha = false;
            es_permanencia = false;



            string grupos_del_usuario = "";

            grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

            if (grupos_del_usuario == "")
            {
                grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
            }

            where += " and user1 in (" + grupos_del_usuario + ") ";




            if (bit == "1")
            {
                es_historial_total = false;
                es_historial_compra = false;
                if (grupo != "")
                {
                    where += " and codvendedor = '" + vendedor.Trim() + "' and bodega in (" + agregra_comillas(grupo.Trim()) + ") ";
                }
            }
            else if (bit == "2")
            {
                es_historial_total = false;
                es_historial_compra = false;
                if (grupo != "")
                {
                    where += " and codvendedor = '" + vendedor.Trim() + "' and user1 in (" + agregra_comillas(grupo.Trim()) + ") ";
                }

            }
            else if (bit == "3" || bit == "7")
            {
                es_historial_total = false;
                es_historial_compra = false;
                where = where.Replace("and codvendedor = '" + vendedor.Trim() + "'", "");
                List<string> dato;
                try
                {
                    dato = vendedor.Split('*').ToList();

                }
                catch
                {
                    dato = vendedor.Split('*').ToList();
                }

                where += " and producto = '" + dato[0].Trim() + "'";
                try
                {

                    if (dato[1].Trim() != "")
                    {
                        where += " and codvendedor in (" + agregra_comillas(dato[1].Trim()) + ")";
                    }
                    if (dato[2].Trim() != "")
                    {
                        where += " and bodega in (" + agregra_comillas(dato[2].Trim()) + ")";
                    }
                }
                catch { }






                if (bit == "7")
                {
                    es_ficha = true;
                    where += " and rutcliente like '%" + grupo + "%'";
                }

                string cod_vend = "";
                if (ReporteRNegocio.esvendedor(User.Identity.Name) == "2")
                {
                    cod_vend = User.Identity.Name;
                }
                if (cod_vend != "")
                {

                    where += " and codvendedor in ('" + cod_vend + "')";

                }


                detalle.InnerText = "Detalle Producto " + ReporteRNegocio.nombre_producto(vendedor.Trim()) + "  (" + fecha.Substring(0, 6) + ")";
                es_producto_grilla = true;
                G_DETALLE_FACTURA.DataSource = ReporteRNegocio.cargar_detalle_lv_producto(where);
                G_DETALLE_FACTURA.DataBind();
            }
            else if (bit == "4")
            {
                es_historial_compra = false;
                es_historial_total = false;
                detalle.InnerText = "Detalle Vendedor: " + ReporteRNegocio.nombre_vendedor(vendedor.Trim()) + "  Cliente: " + ReporteRNegocio.nombre_cliente(grupo.Trim());

                div_sp.InnerHtml = "<div class='btn-group'> <a href='REPORTE_SP.aspx?G=912&C=" + grupo.Trim().Substring(0, grupo.Trim().Length - 1) + "*" + vendedor + "' target='_blank'><i class='fa fa-search fa-2x' aria-hidden='true'></i>SP</a> </div>";

                cont_periodos_ = fecha.Split(',').ToList().Count;
                es_producto_grilla = true;
                header_sum2 = true;
                header_total2 = true;
                es_reporte_vendedor = true;
                vendedor1 = vendedor.Trim();
                cliente = grupo.Trim();
                DataTable produc_vend_c = ReporteRNegocio.listar_prod_client(vendedor.Trim(), grupo.Trim(), fecha, grupos_del_usuario);
                produc_vend_c.Columns.Add("Total");
                G_DETALLE_FACTURA.DataSource = produc_vend_c;
                G_DETALLE_FACTURA.DataBind();

            }
            else if (bit == "5")
            {
                es_historial_total = false;
                es_historial_compra = false;
                detalle.InnerText = "Detalle cliente: " + ReporteRNegocio.nombre_cliente(grupo.Trim());
                div_sp.InnerHtml = "<div class='btn-group'> <a href='REPORTE_SP.aspx?G=912&C=" + grupo.Trim().Substring(0, grupo.Trim().Length - 1) + "*" + vendedor + "' target='_blank'><i class='fa fa-search fa-2x' aria-hidden='true'></i>SP</a> </div>";

                es_reporte_cliente = true;
                es_producto_grilla = true;
                es_reporte_vendedor = false;
                G_DETALLE_FACTURA.DataSource = ReporteRNegocio.detalle_cliente(grupo.Trim(), " and user1 in (" + grupos_del_usuario + ") ");
                G_DETALLE_FACTURA.DataBind();

            }
            else if (bit == "8")
            {
                es_historial_total = false;
                es_historial_compra = false;
                vendedor1 = vendedor.Trim();
                cliente = grupo.Trim();
                header_sum2 = true;
                detalle.InnerText = "Periodo(" + fecha + ") Detalle cliente: " + ReporteRNegocio.nombre_cliente(grupo.Trim()) + " Vendedor : " + ReporteRNegocio.nombre_vendedor(vendedor.Trim());
                div_sp.InnerHtml = "<div class='btn-group'> <a href='REPORTE_SP.aspx?G=912&C=" + grupo.Trim().Substring(0, grupo.Trim().Length - 1) + "*" + vendedor.Trim() + "' target='_blank'><i class='fa fa-search fa-2x' aria-hidden='true'></i>SP</a> </div>";

                es_reporte_cliente = false;


                es_producto_grilla = true;
                es_reporte_vendedor = false;
                es_nuevo_click_montos = true;
                where += " and rutcliente like '%" + grupo + "%'";
                G_DETALLE_FACTURA.DataSource = ReporteRNegocio.detalle_monto_click(where);
                G_DETALLE_FACTURA.DataBind();

            }

            else if (bit == "9")
            {
                es_historial_compra = false;
                es_historial_total = false;
                ////MATRIZ DE VENTAS !!! 
                es_matriz = true;
                vendedor1 = vendedor.Trim();
                cliente = grupo.Trim();
                header_sum2 = true;
                detalle.InnerText = "Detalle cliente: " + ReporteRNegocio.nombre_cliente(grupo.Trim()) + " Producto : " + ReporteRNegocio.nombre_producto(vendedor.Trim());
                es_reporte_cliente = false;
                es_producto_grilla = true;
                es_reporte_vendedor = false;
                es_nuevo_click_montos = false;
                where = fecha + "  and rutcliente like '%" + grupo + "%' and producto = '" + vendedor + "'";
                G_DETALLE_FACTURA.DataSource = ReporteRNegocio.detalle_matriz_click(where);
                G_DETALLE_FACTURA.DataBind();

            }

            else if (bit == "10")
            {
                es_historial_total = false;

                es_historial_compra = false;
                ////MATRIZ DE VENTAS !!! 
                ///vendedor = cliente
                /// fecha = el where
                /// grupo = nada
                es_matriz = true;
                vendedor1 = vendedor.Trim();
                cliente = grupo.Trim();
                header_sum2 = true;
                detalle.InnerText = "Detalle cliente: " + ReporteRNegocio.nombre_cliente(vendedor1.Trim());
                es_reporte_cliente = false;
                es_producto_grilla = true;
                es_reporte_vendedor = false;
                es_nuevo_click_montos = false;
                where = fecha + "  and rutcliente like '%" + vendedor1 + "%'";
                G_DETALLE_FACTURA.DataSource = ReporteRNegocio.detalle_matriz_click_cliente(where);
                G_DETALLE_FACTURA.DataBind();

            }

            else if (bit == "11")
            {
                es_historial_total = false;
                es_historial_compra = false;
                ////MATRIZ DE VENTAS !!! 
                ///vendedor = cliente
                /// fecha = el where
                /// grupo = nada
                es_matriz = true;
                vendedor1 = vendedor.Trim();
                cliente = grupo.Trim();
                header_sum2 = true;
                detalle.InnerText = "Detalle cliente: " + ReporteRNegocio.nombre_cliente(vendedor1.Trim());
                es_reporte_cliente = false;
                es_producto_grilla = true;
                es_reporte_vendedor = false;
                es_nuevo_click_montos = false;
                where = fecha + "  and rutcliente like '%" + vendedor1 + "%'";
                G_DETALLE_FACTURA.DataSource = ReporteRNegocio.detalle_matriz_click_cliente(where);
                G_DETALLE_FACTURA.DataBind();

            }

            else if (bit == "12")
            {
                es_historial_total = false;
                es_historial_compra = false;
                almacenamiento_cobro = true;

                ////SERVICIO DE ALMACENAMIENTO !!! 
                ///vendedor = cliente
                /// fecha = el where
                /// grupo = nada
                es_matriz = true;
                string año = fecha.Split('*')[1].ToString();
                string mes = fecha.Split('*')[0].ToString();
                Session["fecha_cobro"] = fecha.Split('*')[2].ToString();
                Session["año"] = año;
                Session["mes"] = mes;
                Session["valor_mensual"] = fecha.Split('*')[3].ToString();
                Session["tipo_cambio"] = fecha.Split('*')[4].ToString();


                monto_almacenamiento = 0;
                monto = 0;
                vendedor1 = vendedor.Split('-')[0].ToString().Trim();
                cliente = agregra_comillas(grupo.Trim());
                header_sum2 = true;
                detalle.InnerText = "Detalle producto: " + ReporteRNegocio.nombre_producto(vendedor1);
                es_reporte_cliente = false;
                es_producto_grilla = true;
                es_reporte_vendedor = false;
                es_nuevo_click_montos = false;
                es_historial_compra = false;
                es_historial_total = false;
                //ACAAAA VOOOOOOOYY

                string condicion = " where 1=1 ";

                if (vendedor1 != "")
                {

                    condicion += " and invtid = '" + vendedor1 + "' ";
                }
                if (cliente != "")
                {

                    condicion += " and siteid in (" + cliente + ") ";
                }




                string codicion_noventa = " and convert(datetime, trandate,103) >= convert(datetime, dateadd(DAY, -90, '" + (Convert.ToInt32(año) - 1).ToString() + "-12-31'),103)  and convert(datetime,trandate,103) <=  convert(datetime, '31/12/" + (Convert.ToInt32(año) - 1).ToString() + "',103) ";


                if (vendedor1 != "")
                {

                    codicion_noventa += " and invtid = '" + vendedor1 + "' ";
                }
                if (cliente != "")
                {

                    codicion_noventa += " and siteid in (" + cliente + ") ";
                }
                try
                {


                    DataTable sobrealmacenaje = ReporteRNegocio.detalle_producto_stock(condicion, mes, año);
                    DataTable noventa_dias_año = ReporteRNegocio.detalle_producto_stock_2(codicion_noventa, mes, (Convert.ToInt32(año) - 1).ToString(), condicion);
                    DateTime fecha_año_pasado = new DateTime();
                    DateTime fecha_año_actual = new DateTime();


                    Double arrastre = 0;
                    bool sw_noventa = false;
                    foreach (DataRow r in noventa_dias_año.Rows)
                    {
                        sw_noventa = true;

                        Session["arraste_año_anterior"] = Convert.ToDouble(r[8]);
                        Session["arraste_año_anterior2"] = Convert.ToDouble(Session["arraste_año_anterior"]);

                        r[8] = Convert.ToDouble(r[7]) + Convert.ToDouble(r[8]);
                        fecha_año_pasado = Convert.ToDateTime(r[5].ToString());

                        break;
                    }


                    foreach (DataRow r in sobrealmacenaje.Rows)
                    {
                        if (sw_noventa)
                        {
                            r[8] = Convert.ToDouble(r[7]);
                        }
                        else
                        {

                            r[8] = Convert.ToDouble(r[7]) + Convert.ToDouble(r[8]);

                        }
                        fecha_año_actual = Convert.ToDateTime(r[5].ToString());
                        break;
                    }



                    DataTable dt_union = sobrealmacenaje.Clone();






                    DataTable union_ambos = noventa_dias_año.AsEnumerable().Union(sobrealmacenaje.AsEnumerable()).Distinct(DataRowComparer.Default).CopyToDataTable<DataRow>();

                    DataTable compras = sobrealmacenaje.Clone();
                    DataTable ventas = sobrealmacenaje.Clone();






                    foreach (DataRow r in sobrealmacenaje.Rows)
                    {


                        if (Convert.ToDouble(r[7].ToString()) > 0)

                        {

                            DataRow desRow = r;

                            compras.ImportRow(desRow);
                        }
                        else
                        {
                            DataRow desRow = r;

                            ventas.ImportRow(desRow);

                        }
                    }


                    foreach (DataRow r in noventa_dias_año.Rows)
                    {


                        if (Convert.ToDouble(r[7].ToString()) > 0)

                        {

                            DataRow desRow = r;

                            compras.ImportRow(desRow);
                        }
                        else
                        {
                            DataRow desRow = r;

                            ventas.ImportRow(desRow);

                        }
                    }

                    DataView dv = ventas.DefaultView;
                    dv.Sort = "trandate ASC";
                    ventas = dv.ToTable();

                    DataView dv2 = compras.DefaultView;
                    dv2.Sort = "trandate ASC";
                    compras = dv2.ToTable();



                    union_ambos.Columns.Add("dias", typeof(String));
                    union_ambos.Columns.Add("cobro", typeof(String));
                    int cont = 1;
                    foreach (DataRow c in compras.Rows)
                    {
                        if (cont == 1)
                        {
                            c[8] = Convert.ToDouble(c[8]) + Convert.ToDouble(c[7]);
                        }
                        else
                        {
                            c[8] = c[7];

                        }
                        cont++;
                    }
                    double _restante_venta = 0;
                    foreach (DataRow v in ventas.Rows)
                    {
                        dt_union.ImportRow(v);

                        double venta = Convert.ToDouble(v[7]);

                        foreach (DataRow c in compras.Rows)
                        {
                            double compra = Convert.ToDouble(c[8]);


                            if (Convert.ToDouble(c[8].ToString()) > 0)


                            {
                                venta += (_restante_venta * -1);
                                c[8] = compra + venta;


                                if (Convert.ToDouble(c[8]) < 0)
                                {

                                    _restante_venta = Convert.ToDouble(c[8].ToString().Replace("-", ""));

                                    c[8] = 0;


                                }
                                else
                                {
                                    _restante_venta = 0;
                                }

                                break;
                            }
                            else
                            {


                            }

                        }


                    }

                    foreach (DataRow c in compras.Rows)
                    {

                        dt_union.ImportRow(c);
                    }
                    DataView dv3 = dt_union.DefaultView;
                    dv3.Sort = "trandate ASC";
                    dt_union = dv3.ToTable();
                    dt_union.Columns.Add("dias", typeof(String));
                    dt_union.Columns.Add("cobro", typeof(String));

                    dt_union.Columns["arrastre"].ColumnName = "Va Quedando";

                    G_DETALLE_FACTURA.DataSource = dt_union;
                    G_DETALLE_FACTURA.DataBind();
                    return;
                }
                catch { }
                //union_ambos.Merge(noventa_dias_año, false, MissingSchemaAction.Add);

                //union_ambos.Columns.RemoveAt(9);
                //union_ambos.Columns.RemoveAt(10);
                //union_ambos.Columns.RemoveAt(11);
                //union_ambos.Columns.RemoveAt(12);
                //union_ambos.Columns.RemoveAt(13);
                ////union_ambos.Columns.RemoveAt(14);

                //table_almace.InnerHtml = crear_tabla_almacenaje(sobrealmacenaje, mes, año);

            }

            else if (bit == "13")
            {
                es_historial_compra = false;
                almacenamiento_cobro_dias = true;

                ////SERVICIO DE ALMACENAMIENTO !!! 
                ///vendedor = cliente
                /// fecha = el where
                /// grupo = nada
                es_matriz = true;
                string año = fecha.Split('*')[1].ToString();
                string mes = fecha.Split('*')[0].ToString();
                Session["fecha_cobro"] = fecha.Split('*')[2].ToString();
                Session["año"] = año;
                Session["mes"] = mes;
                Session["valor_mensual"] = fecha.Split('*')[3].ToString();
                Session["tipo_cambio"] = fecha.Split('*')[4].ToString();
                Session["dias"] = fecha.Split('*')[5].ToString();


                monto_almacenamiento = 0;
                monto = 0;
                vendedor1 = vendedor.Split('-')[0].ToString().Trim();
                cliente = agregra_comillas(grupo.Trim());
                header_sum2 = true;
                detalle.InnerText = "Detalle producto: " + ReporteRNegocio.nombre_producto(vendedor1);
                es_reporte_cliente = false;
                es_producto_grilla = true;
                es_reporte_vendedor = false;
                es_nuevo_click_montos = false;
                almacenamiento_cobro = false;
                //ACAAAA VOOOOOOOYY

                Session["bodegas"] = cliente;
                Session["invtid"] = vendedor1;


                string condicion = " ";

                if (vendedor1 != "")
                {

                    condicion += " and invtid = '" + vendedor1 + "' ";
                }
                if (cliente != "")
                {

                    condicion += " and siteid in (" + cliente + ") ";
                }

                DataTable aux = crear_cobro_(ReporteRNegocio.costo_sobrealmacenaje(condicion, Session["mes"].ToString(), Session["año"].ToString(), ""));


                if (Convert.ToDouble(Session["arrastre_noven"]) < 0)
                {

                    detalle.InnerText = "Detalle producto: " + ReporteRNegocio.nombre_producto(vendedor1) + "  F: " + Session["mes"].ToString() + "/" + Session["año"].ToString() + " SALIDAS A LO COBRABLE: " + Base.monto_format(Session["arrastre_noven"].ToString());
                }
                else
                {
                    detalle.InnerText = "Detalle producto: " + ReporteRNegocio.nombre_producto(vendedor1) + "  F: " + Session["mes"].ToString() + "/" + Session["año"].ToString() + " ARRASTRE COBRABLE: " + Base.monto_format(Session["arrastre_noven"].ToString());
                }
                G_DETALLE_FACTURA.DataSource = aux;
                G_DETALLE_FACTURA.DataBind();
                return;

            }

            else if (bit == "54")
            {
                es_historial_total = false;
                es_historial_compra = false;
                es_reporte_cliente = false;
                es_producto_grilla = false;
                es_reporte_vendedor = false;
                es_nuevo_click_montos = false;
                almacenamiento_cobro = false;
                es_matriz = true;
                es_costo_import = true;

                Session["costo"] = grupo;

                detalle.InnerText = "Detalle PONbr: " + fecha;


                //fecha =  PONbr
                string where3 = " where u.ponbr = '" + fecha + "'";

                DataTable dt2 = ReporteRNegocio.lista_costosimpot(where3);

                foreach (DataRow r1 in dt2.Rows)
                {

                    string num = ((Convert.ToDouble(r1["Dolar"]) * 100) / Convert.ToDouble(grupo)).ToString();
                    r1["Porct"] = num.Substring(0, num.IndexOf(',') + 3) + "%";
                }



                G_DETALLE_FACTURA.DataSource = dt2;
                G_DETALLE_FACTURA.DataBind();



                return;

            }

            else if (bit == "55")
            {
                monto_cos = 0;
                monto_import = 0;
                monto_total = 0;
                es_reporte_cliente = false;
                es_producto_grilla = false;
                es_reporte_vendedor = false;
                es_nuevo_click_montos = false;
                almacenamiento_cobro = false;
                es_matriz = true;
                es_costo_import = false;
                es_historial_compra = true;
                es_historial_total = false;
                detalle.InnerText = "Detalle COD: " + fecha;
                //fecha =  cod
                //vendedor = ponbr

                string where3 = " where u.nbr <> '" + vendedor + "' and u.cod = '" + fecha + "'  order by CONVERT(datetime, u.[F.Recib],103)  desc";

                DataTable dt2 = ReporteRNegocio.lista_compras(where3);
                G_DETALLE_FACTURA.DataSource = dt2;
                G_DETALLE_FACTURA.DataBind();



                return;

            }

            else if (bit == "56")
            {
                monto_cos = 0;
                monto_import = 0;
                monto_total = 0;
                es_reporte_cliente = false;
                es_producto_grilla = false;
                es_reporte_vendedor = false;
                es_nuevo_click_montos = false;
                almacenamiento_cobro = false;
                es_matriz = true;
                es_costo_import = false;
                es_historial_compra = false;
                es_historial_total = true;
                detalle.InnerText = "Detalle COD: " + fecha;
                Session["fecha_color"] = vendedor;

                //fecha =  cod
                //vendedor = fecha


                string where3 = " where u.cod = '" + fecha + "' order by CONVERT(datetime, u.[F.Recib],103)  desc";

                DataTable dt2 = ReporteRNegocio.lista_compras(where3);
                G_DETALLE_FACTURA.DataSource = dt2;
                G_DETALLE_FACTURA.DataBind();



                return;

            }
            else if (bit == "57")
            {

                //aca detalle sp
                monto_cos = 0;
                monto_import = 0;
                monto_total = 0;
                es_reporte_cliente = false;
                es_producto_grilla = false;
                es_reporte_vendedor = false;
                es_nuevo_click_montos = false;
                almacenamiento_cobro = false;
                es_matriz = false;
                es_costo_import = false;
                es_historial_compra = false;
                es_historial_total = false;
                es_venta_movil = true;

                string SP = fecha.Trim();
                detalle.InnerText = "Detalle Venta Movil: " + SP + " (TON A KGR)";
                Session["fecha_color"] = vendedor;
                Session["sp"] = SP.Trim();

                descarga_pdf.Visible = true;

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd411Q21mp", "<script language='javascript'>  document.getElementById('descarga_word').click();  </script>", false);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "PDF_CLick", "<script language='javascript'>  document.getElementById('descarga_pdf').click();  </script>", false);

                string where3 = " where b.coddocumento = '" + SP + "'";

                DataTable dt2 = ReporteRNegocio.VM_listar_detalle(where3);
                G_DETALLE_FACTURA.DataSource = dt2;
                G_DETALLE_FACTURA.DataBind();
                return;

            }

            else if (bit == "58")
            {

                monto_cos = 0;
                monto_import = 0;
                monto_total = 0;
                es_reporte_cliente = false;
                es_producto_grilla = false;
                es_reporte_vendedor = false;
                es_nuevo_click_montos = false;
                almacenamiento_cobro = false;
                es_matriz = false;
                es_costo_import = false;
                es_historial_compra = false;
                es_historial_total = false;
                es_venta_movil = false;
                es_permanencia = true;
                detalle.InnerText = "Detalle Permanencia Producto: " + vendedor;

                string producto = vendedor.Substring(0, vendedor.IndexOf("-") - 1);
                string grupo_a = ReporteRNegocio.trae_grupo_stock(producto);

                string clase = "";
                double producto1 = 0;
                try
                {
                    producto1 = Convert.ToDouble(producto);

                }
                catch { }

                if (grupo_a == "ABAR" || grupo_a == "MANI" && producto != "9918")
                {
                    clase = "  H.glclassid in ('ABAR', 'MANI') and H.invtid > '1000' and H.invtid <> '9918' ";
                }
                else
                {
                    clase = "  H.glclassid <> 'ABAR' and H.glclassid <>  'MANI'  and H.invtid <> '9905'  and H.invtid <> '9999'  and H.invtid <> '9907'   ";
                }

                string desde = fecha.Split('*')[0];
                string hasta = fecha.Split('*')[1];
                string bodegas = fecha.Split('*')[2];

                string where3 = " where  1=1 ";

                string where2 = " where  1=1 ";

                if (producto != "")
                {

                    where3 += " and invtid in (" + agregra_comillas(producto) + ")";
                }

                if (bodegas != "")
                {

                    where3 += " and siteid in (" + agregra_comillas(bodegas) + ") ";

                }

                if (desde != "")
                {

                    where2 = where3 + " and trandate < convert(datetime, '" + desde + "',103)";
                    where3 += " and trandate >= convert(datetime, '" + desde + "',103) ";
                }
                if (hasta != "")
                {
                    where3 += " and trandate <= convert(datetime, '" + hasta + "',103) ";
                }

                Session["diferencia"] = 0;
                DataTable dt2 = ReporteRNegocio.PERMANENCIA2(where3, desde, where2, clase);
                DataTable dt3 = ReporteRNegocio.PERMANENCIA_NEGA(where3);
                double cant = 0;

                double diferencia = 0;

                //int con = 0;
                double resto = 0;
                foreach (DataRow r1 in dt2.Rows)
                {

                    diferencia = Convert.ToDouble(Session["diferencia"].ToString());

                    //if (diferencia < 0)
                    //{

                    //    foreach (DataRow r2 in dt3.Rows)
                    //    {
                    //        if (r2[6].ToString() == "si")
                    //        {
                    //            r2[3] = diferencia;
                    //            r2[4] = "no";

                    //        }

                    //    }

                    //}

                    cant = Convert.ToDouble(r1[3].ToString());
                    int con = 0;
                    if (cant > 0)
                    {

                        foreach (DataRow r2 in dt3.Rows)
                        {
                            //r2[6] = "";
                            if (r2[4].ToString() != "si")
                            {

                                if (con + 1 == dt3.Rows.Count)
                                {

                                    r1[4] = "*" + r2[2].ToString();
                                }

                                cant = cant + Convert.ToDouble(r2[3].ToString());
                                if (cant < 0) { }
                                else
                                {
                                    r1[5] = cant;
                                }
                                if (cant <= 0)
                                {
                                    r1[5] = "-";
                                    r1[4] = r2[2].ToString();
                                    if (con + 1 == dt3.Rows.Count)
                                    {

                                        r1[4] = "*" + r2[2].ToString();
                                    }
                                    //Session["diferencia"] = cant;

                                    if (cant < 0)
                                    {
                                        r2[4] = "no";
                                        r2[3] = cant;
                                    }
                                    else
                                    {
                                        r2[4] = "si";

                                    }
                                    resto = cant;
                                    //r2[6] = "si";
                                    con++;
                                    break;
                                }
                                r2[4] = "si";
                            }

                            con++;
                        }

                    }

                }

                int con1 = 0;
                int sum_perm = 0;
                int cont_perm = 0;

                bool sw_fecha_final = true;
                foreach (DataRow r1 in dt2.Rows)
                {

                    r1[7] = r1[7].ToString().Replace(".00000", "");
                    if (con1 == 0)
                    {
                        r1[1] = "ARRASTE HASTA " + hasta;


                    }
                    else
                    {

                        try
                        {
                            double stock = Convert.ToDouble(r1[7].ToString().Replace(".00000", "").Replace(".00", ""));

                            double total_bodega = Convert.ToDouble(r1[8].ToString());


                            r1[9] = Base.monto_format2((stock / total_bodega) * 100) + "%";

                        }
                        catch
                        {


                        }

                        r1[7] = Base.monto_format(r1[7].ToString().Replace(".00000", "").Replace(".00", ""));
                        r1[8] = Base.monto_format(r1[8].ToString().Replace(".00000", "").Replace(".00", ""));

                    }

                    try
                    {

                        r1[4] = r1[4].ToString().Remove(11).Trim();

                    }
                    catch
                    {


                    }
                    if (r1[4].ToString().Trim() != "")
                    {

                        if (!r1[4].ToString().Contains("*"))
                        {
                            r1[5] = "-";
                        }
                        DateTime en = Convert.ToDateTime(r1[2].ToString());
                        DateTime sal = Convert.ToDateTime(r1[4].ToString().Replace("*", ""));
                        TimeSpan ts = sal - en;
                        // Difference in days.
                        int differenceInDays = ts.Days;
                        r1[6] = differenceInDays;
                        sum_perm += differenceInDays;
                        //if (differenceInDays >= 0)
                        //{
                        //    cont_perm++;
                        //}
                        //else
                        //{
                        //    //r1[4] = DateTime.Now.ToShortDateString() + "HOY";
                        //    //DateTime en1 = Convert.ToDateTime(r1[2].ToString());
                        //    //DateTime sal1 = Convert.ToDateTime(r1[4].ToString().Replace("*", "").Replace("HOY", ""));
                        //    //TimeSpan ts1 = sal1 - en1;
                        //    //// Difference in days.
                        //    //int differenceInDays1 = ts1.Days;
                        //    //r1[6] = differenceInDays1;
                        //    //sum_perm += differenceInDays1;
                        //}

                    }
                    else if (r1[1].ToString() != "ARRASTE HASTA " + hasta)
                    {
                        r1[5] = r1[3].ToString().Replace(",00", "");
                        r1[4] = hasta + "HASTA";
                        DateTime en1 = Convert.ToDateTime(r1[2].ToString());
                        DateTime sal1 = Convert.ToDateTime(r1[4].ToString().Replace("*", "").Replace("HASTA", ""));
                        TimeSpan ts1 = sal1 - en1;
                        // Difference in days.
                        int differenceInDays1 = ts1.Days;
                        r1[6] = differenceInDays1;
                        sum_perm += differenceInDays1;
                        sw_fecha_final = true;

                    }



                    con1++;
                }




                G_DETALLE_FACTURA.DataSource = dt2;
                G_DETALLE_FACTURA.DataBind();



                return;

            }


            else if (bit == "2018")
            {

                //COMISION DETALLE

                es_reporte_cliente = false;
                es_producto_grilla = false;
                es_reporte_vendedor = false;
                es_nuevo_click_montos = false;
                almacenamiento_cobro = false;
                es_matriz = false;
                es_costo_import = false;
                es_historial_compra = false;
                es_historial_total = false;
                es_venta_movil = false;
                detalle.InnerText = "Detalle Comisiones: " + grupo + " ( " + vendedor + ")";


                string where3 = "";

                where3 += " and vendedor in (" + agregra_comillas(grupo) + ")";
                where3 += " and periodo_pago in (" + vendedor + ")";


                DataTable dt2 = ReporteRNegocio.trae_comisiones(where3);
                G_DETALLE_FACTURA.DataSource = dt2;
                G_DETALLE_FACTURA.DataBind();
                return;

            }


            if (!es_producto_grilla && !es_matriz)
            {
                G_DETALLE_FACTURA.DataSource = ReporteRNegocio.cargar_detalle_lv(where);
                G_DETALLE_FACTURA.DataBind();
            }

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('G_DETALLE_FACTURA')); </script>", false);

        }
        //

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

            G_DETALLE_FACTURA.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());

            Response.End();

        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }


        private void AbrirArchivo(string Path)
        {
            Process P = new Process();
            try
            {
                P.StartInfo.FileName = Path;
                P.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                P.Start();
                //Espera el proceso para que lo termine y continuar
                P.WaitForExit();
                //Liberar
                P.Close();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>alert('No se puede abrir el documento " + Path + "');</script>", false);



            }
        }

        private void TheDownload2(string path, string dlDIR, string filename_)
        {
            path = path.Replace("\\\\", "\\");
            System.IO.FileInfo toDownload = new System.IO.FileInfo(path);
            if (toDownload.Exists)
            {
                try
                {
                    path = path.Replace("\\", "//");
                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/msword";
                    Response.AddHeader("Content-Disposition", "inline; filename=" + filename_);
                    Response.TransmitFile(path);
                    Response.Flush();
                    Response.End();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>alert('No se puede descargar el documento " + ex.Message + "');</script>", false);

                }
            }
            else
            {
                path = path.Replace("_33_", "_61_");
                System.IO.FileInfo toDownload2 = new System.IO.FileInfo(path);
                if (toDownload2.Exists)
                {
                    path = path.Replace("\\", "//");
                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "inline; filename=" + filename_);
                    Response.TransmitFile(path);
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    path = path.Replace("_61_", "_56_");
                    System.IO.FileInfo toDownload3 = new System.IO.FileInfo(path);
                    if (toDownload3.Exists)
                    {
                        path = path.Replace("\\", "//");
                        Response.Clear();
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Disposition", "inline; filename=" + filename_);
                        Response.TransmitFile(path);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        path = path.Replace("_56_", "_46_");
                        System.IO.FileInfo toDownload4 = new System.IO.FileInfo(path);
                        if (toDownload4.Exists)
                        {
                            path = path.Replace("\\", "//");
                            Response.Clear();
                            Response.ClearContent();
                            Response.ClearHeaders();
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("Content-Disposition", "inline; filename=" + filename_);
                            Response.TransmitFile(path);
                            Response.Flush();
                            Response.End();
                        }
                    }

                }
            }
        }



        private DataTable crear_cobro_(DataTable dt)
        {


            string condicion = " where 1=1 ";

            DataTable dias_cobro = new DataTable();

            dias_cobro.Columns.Add("Dia");
            dias_cobro.Columns.Add("Entrada");
            dias_cobro.Columns.Add("Salida");
            dias_cobro.Columns.Add("Arrastre");
            dias_cobro.Columns.Add("Cobro");


            double v_me = Convert.ToDouble(Session["valor_mensual"].ToString());
            int an = Convert.ToInt32(Session["año"].ToString());
            int me = Convert.ToInt32(Session["mes"].ToString());
            double daymon = System.DateTime.DaysInMonth(an, me);
            double valor_dia = v_me / daymon;

            if (Session["bodegas"].ToString() != "")
            {

                condicion += " and siteid in (" + agregra_comillas(Session["bodegas"].ToString()) + ") ";

            }
            foreach (DataRow r in dt.Rows)
            {
                //double aux_var = 0;
                //try
                //{
                //    aux_var = Convert.ToDouble(r[3]);
                //}
                //catch { }

                //if (aux_var > 0)
                //{


                string invtid = r[0].ToString().Split('-')[0].Trim();

                try
                {

                    DataTable arrastre_noventa_y_salidas = ReporteRNegocio.arrastre_noventa(condicion + " and invtid = '" + invtid + "' ", Session["mes"].ToString(), Session["año"].ToString(), Session["dias"].ToString());

                    double arrastre_noven = 0;
                    string fecha_inicial = "";

                    arrastre_noven = Convert.ToDouble(arrastre_noventa_y_salidas.Rows[0][1]);
                    //if (arrastre_noven < 0) {
                    //    arrastre_noven = 0;
                    //}

                    fecha_inicial = arrastre_noventa_y_salidas.Rows[0][0].ToString();

                    Session["arrastre_noven"] = arrastre_noven;


                    DataRow row;
                    double arrastre = 0;
                    double SUMA_A_COBRAR = 0;
                    for (int i = 1; i <= Convert.ToInt32(Session["fecha_cobro"]); i++)
                    {
                        row = dias_cobro.NewRow();
                        row[0] = i;
                        DataTable estrada_y_salida = ReporteRNegocio.dia_entrada_noven(condicion + " and invtid = '" + invtid + "' ", Session["mes"].ToString(), Session["año"].ToString(), i, Session["dias"].ToString());
                        double entrada = Convert.ToDouble(estrada_y_salida.Rows[0][0]);
                        double salida = Convert.ToDouble(estrada_y_salida.Rows[0][1]);


                        row[1] = entrada.ToString("N0");
                        row[2] = salida.ToString("N0");

                        arrastre = (arrastre_noven + entrada + salida);
                        if (arrastre < 0)
                        {
                            row[3] = "0";
                        }
                        else
                        {
                            row[3] = arrastre.ToString("N0");
                        }
                        arrastre_noven = arrastre;

                        double kilos = arrastre;
                        double valor_for = (kilos * (valor_dia * Convert.ToDouble(Session["tipo_cambio"].ToString())) / 1000);

                        if (valor_for > 0)
                        {
                            SUMA_A_COBRAR += valor_for;


                            row[4] = valor_for.ToString("N0");
                        }
                        else { row[4] = 0; }
                        dias_cobro.Rows.Add(row);

                    }


                    r[4] = fecha_inicial;
                    r[5] = SUMA_A_COBRAR.ToString("N0");


                    dias_cobro.Columns["Cobro"].ColumnName = "Cobro ( " + SUMA_A_COBRAR.ToString("N0") + ")";

                }

                catch (Exception ex)
                {
                    Console.Write(ex.Message);

                }

            }

            return dias_cobro;
        }


        public static DataTable MergeTablesByIndex(DataTable t1, DataTable t2)
        {
            if (t1 == null || t2 == null) throw new ArgumentNullException("t1 or t2", "Both tables must not be null");

            DataTable t3 = t1.Clone();  // first add columns from table1
            foreach (DataColumn col in t2.Columns)
            {
                string newColumnName = col.ColumnName;
                int colNum = 1;
                while (t3.Columns.Contains(newColumnName))
                {
                    newColumnName = string.Format("{0}_{1}", col.ColumnName, ++colNum);
                }
                t3.Columns.Add(newColumnName, col.DataType);
            }
            var mergedRows = t1.AsEnumerable().Zip(t2.AsEnumerable(),
                (r1, r2) => r1.ItemArray.Concat(r2.ItemArray).ToArray());
            foreach (object[] rowFields in mergedRows)
                t3.Rows.Add(rowFields);

            return t3;
        }

        private string crear_tabla_almacenaje(DataTable sobrealmacenaje, string mes, string año)
        {
            string HTML = "";

            int mes3 = 1;

            while (mes3 <= Convert.ToInt32(mes))
            {
                HTML += "<table id='TABLA_REPORTE55' class='table table-advance table-bordered fill-head tablesorter filtrar' style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";

                int mes4 = 1;
                string nombre_mes = MonthName(mes3);
                //HTML += "<table id='TABLA_REPORTE2' class='table table-advance table-bordered fill-head filtrar' style='width: 98%; border-collapse: collapse;    border-spacing: 1px !important;'  width:98%;>";
                HTML += "<thead>";
                HTML += "<tr>";
                HTML += "<th colspan=1;>" + nombre_mes + "</th>";
                HTML += "<th colspan=1;>" + año + "</th>";
                HTML += "<th colspan=1;></th>";
                while (mes4 <= Convert.ToInt32(mes))
                {
                    HTML += "<th colspan=2;>ENTRADAS</th>";
                    mes4++;
                }
                HTML += "<th colspan=1;></th>";
                HTML += "<th colspan=1;></th>";
                HTML += "</tr>";

                HTML += "<tr>";
                HTML += "<th colspan=1;>Semana 1</th>";
                HTML += "<th colspan=1;></th>";
                HTML += "<th colspan=1;>USD</th>";
                mes4 = 1;
                while (mes4 <= Convert.ToInt32(mes))
                {
                    HTML += "<th colspan=1;>" + MonthName(mes4) + "</th>";
                    HTML += "<th colspan=1;>Arrastre</th>";
                    mes4++;
                }
                HTML += "<th colspan=1;>T/C</th>";
                HTML += "<th colspan=1;>TOTAL</th>";
                HTML += "</tr>";

                HTML += "</thead>";

                HTML += "<tbody>";
                int dia_semana = 1;
                for (int x = 1; x <= 4; x++)
                {
                    //CUERPO_SEMANAS
                    HTML += "<tr>";
                    HTML += "<td colspan=1;>" + dia_semana.ToString("D2") + "/" + mes3.ToString("D2") + "/" + año + "</th>";
                    HTML += "<td colspan=1;>" + (dia_semana + 6).ToString("D2") + "/" + mes3.ToString("D2") + "/" + año + "</th>";
                    dia_semana = dia_semana + 7;
                    HTML += "<td colspan=1;>0,68</th>";
                    mes4 = 1;
                    while (mes4 <= Convert.ToInt32(mes))
                    {
                        string color_mes = color_del_mes(mes4);
                        HTML += "<td colspan=1; style='" + color_mes + "'></td>";
                        HTML += "<td colspan=1; style='" + color_mes + "'></td>";
                        mes4++;
                    }
                    HTML += "<td colspan=1;></td>";
                    HTML += "<td colspan=1;></td>";
                    HTML += "</tr>";

                }


                HTML += "<tr>";
                HTML += "<td colspan=1;>" + dia_semana + "/" + mes3.ToString("D2") + "/" + año + "</th>";
                int final_mes = System.DateTime.DaysInMonth(Convert.ToInt32(año), mes3);
                HTML += "<td colspan=1;>" + final_mes + "/" + mes3.ToString("D2") + "/" + año + "</th>";
                HTML += "<td colspan=1;>0,28</th>";
                mes4 = 1;
                while (mes4 <= Convert.ToInt32(mes))
                {
                    string color_mes = color_del_mes(mes4);
                    HTML += "<td colspan=1; style='" + color_mes + "'></td>";
                    HTML += "<td colspan=1; style='" + color_mes + "'></td>";
                    mes4++;
                }
                HTML += "<td colspan=1;></td>";
                HTML += "<td colspan=1;></td>";
                HTML += "</tr>";



                HTML += "<tr>";
                HTML += "<td colspan=1;></th>";
                HTML += "<td colspan=1;></th>";
                HTML += "<td colspan=1;>3</th>";
                mes4 = 1;
                while (mes4 <= Convert.ToInt32(mes))
                {
                    string color_mes = color_del_mes(mes4);
                    HTML += "<td colspan=1;></td>";
                    HTML += "<td colspan=1; style='" + color_mes + "'></td>";
                    mes4++;
                }
                HTML += "<td colspan=1;></td>";
                HTML += "<td colspan=1;></td>";

                HTML += "</tr>";




                HTML += "</tbody>";

                HTML += "  </table>  <br> <br>";
                mes3++;
            }

            return HTML;
        }

        private string color_del_mes(int mes4)
        {
            string style_ = "";
            if (mes4 == 1)
            {
                style_ = "background-color: #ddffec;";
            }
            if (mes4 == 2)
            {
                style_ = "background-color: #ffdddd;";
            }
            if (mes4 == 3)
            {
                style_ = "background-color: #a2baff;";
            }
            if (mes4 == 4)
            {
                style_ = "background-color: #9de698;";
            }
            if (mes4 == 5)
            {
                style_ = "background-color: #d4d4d4;";
            }
            if (mes4 == 6)
            {
                style_ = "background-color: #ddffec;";
            }
            if (mes4 == 7)
            {
                style_ = "background-color: #ffdddd;";
            }
            if (mes4 == 8)
            {
                style_ = "background-color: #a2baff;";
            }
            if (mes4 == 9)
            {
                style_ = "background-color: #9de698;";
            }
            if (mes4 == 10)
            {
                style_ = "background-color: #d4d4d4;";
            }
            if (mes4 == 11)
            {
                style_ = "background-color: #ddffec;";
            }
            if (mes4 == 12)
            {
                style_ = "background-color: #ffdddd;";
            }




            return style_;
        }

        private string MonthName(int mes3)
        {

            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(dtinfo.GetMonthName(mes3));

        }

        protected void G_DETALLE_FACTURA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (busca_columna_fac)
                {
                    try
                    {
                        for (int x = 0; x <= G_DETALLE_FACTURA.HeaderRow.Cells.Count; x++)
                        {
                            if (G_DETALLE_FACTURA.HeaderRow.Cells[x].Text.Contains("Factura"))
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

                if (es_nuevo_click_montos)
                {
                    //e.Row.Cells[0].Text = cont_det.ToString();
                    //cont_det++;
                    string where_es = " where codvendedor = '" + vendedor1 + "' and rutcliente = '" + cliente + "'  ";
                    if (header_sum2)
                    {
                        G_DETALLE_FACTURA.HeaderRow.Cells[1].Attributes["data-sort-method"] = "number";
                        G_DETALLE_FACTURA.HeaderRow.Cells[1].Text = "Venta  (" + Convert.ToDouble(ReporteRNegocio.Facturación_Mes(fecha, where_es)).ToString("N0") + ")";
                    }
                    if (1 == 1) { header_sum2 = false; }


                    double sum_por_row = 0;
                    double d;
                    double.TryParse(e.Row.Cells[1].Text, out d);
                    string aux = "";
                    if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                    e.Row.Cells[1].Text = aux;
                    sum_por_row += d;


                }
                else if (es_reporte_vendedor)
                {

                    double sum_por_row = 0;
                    G_DETALLE_FACTURA.HeaderRow.Cells[0].Attributes["class"] = "sort-default";
                    string where_es = " where codvendedor = '" + vendedor1 + "' and rutcliente = '" + cliente + "'  ";
                    for (int i = 1; i < e.Row.Cells.Count - 1; i++)
                    {
                        if (header_sum2)
                        {
                            G_DETALLE_FACTURA.HeaderRow.Cells[1].Attributes["data-sort-method"] = "number";
                            G_DETALLE_FACTURA.HeaderRow.Cells[i].Text = G_DETALLE_FACTURA.HeaderRow.Cells[i].Text + "  (" + Convert.ToDouble(ReporteRNegocio.Facturación_Mes(G_DETALLE_FACTURA.HeaderRow.Cells[i].Text.Substring(0, 6), where_es)).ToString("N0") + ")";
                        }
                        if (i == e.Row.Cells.Count - 2) { header_sum2 = false; }

                        double d;
                        double.TryParse(e.Row.Cells[i].Text, out d);
                        string aux = "";
                        if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                        e.Row.Cells[i].Text = aux;
                        sum_por_row += d;
                    }

                    e.Row.Cells[e.Row.Cells.Count - 1].Text = sum_por_row.ToString("N0");

                }
                else if (es_reporte_cliente)
                {
                    try
                    {
                        monto += double.Parse(e.Row.Cells[2].Text);
                        e.Row.Cells[2].Text = Convert.ToInt32(e.Row.Cells[2].Text).ToString("N0");
                    }
                    catch { }
                    G_DETALLE_FACTURA.HeaderRow.Cells[2].Text = "Venta ( " + monto.ToString("N0") + " )";
                    G_DETALLE_FACTURA.HeaderRow.Cells[3].Attributes["data-sort-method"] = "number";
                    G_DETALLE_FACTURA.HeaderRow.Cells[2].Attributes["data-sort-method"] = "number";
                    G_DETALLE_FACTURA.HeaderRow.Cells[1].Attributes["class"] = "sort-default";
                    G_DETALLE_FACTURA.HeaderRow.Cells[0].Attributes["class"] = "sort-default";

                }
                else if (almacenamiento_cobro)
                {


                    if (e.Row.Cells[7].Text.Substring(0, 1).Contains("0") || e.Row.Cells[7].Text.Substring(0, 1).Contains("-"))
                    {

                        e.Row.Cells[8].Text = "0";
                    }
                    if (Convert.ToDouble(e.Row.Cells[8].Text) > 0)
                    {
                        DateTime hoy = Convert.ToDateTime(Session["fecha_cobro"]);
                        TimeSpan t3s = hoy - Convert.ToDateTime(e.Row.Cells[5].Text);
                        int differenceInDays2 = t3s.Days;

                        e.Row.Cells[9].Text = differenceInDays2.ToString();


                        if (differenceInDays2 > 90)
                        {


                            double dias_sobre_almacenaje = differenceInDays2 - 90;
                            double v_me = Convert.ToDouble(Session["valor_mensual"].ToString());
                            int an = Convert.ToInt32(Session["año"]);
                            int me = Convert.ToInt32(Session["mes"]);
                            double daymon = System.DateTime.DaysInMonth(an, me);
                            double valor_dia = v_me / daymon;

                            double kilos = Convert.ToDouble(e.Row.Cells[8].Text);

                            e.Row.Cells[10].Text = (kilos * (dias_sobre_almacenaje * valor_dia)).ToString("N0");

                            double d = Convert.ToDouble(e.Row.Cells[10].Text);
                            Session["SUMA_A_COBRAR"] = Convert.ToDouble(Session["SUMA_A_COBRAR"]) + d;
                        }




                    }





                    try
                    {

                        monto_almacenamiento += Convert.ToDouble(Session["arraste_año_anterior"]);
                        Session["arraste_año_anterior"] = 0;
                        monto += double.Parse(e.Row.Cells[7].Text);
                        monto_almacenamiento += double.Parse(e.Row.Cells[7].Text);
                        e.Row.Cells[7].Text = Convert.ToInt32(e.Row.Cells[7].Text).ToString("N0");
                    }
                    catch { }
                    G_DETALLE_FACTURA.HeaderRow.Cells[7].Text = "Total ( " + monto.ToString("N0") + " +  " + Convert.ToDouble(Session["arraste_año_anterior2"]).ToString("N0") + " = " + monto_almacenamiento.ToString("N0") + " )";
                    G_DETALLE_FACTURA.HeaderRow.Cells[3].Attributes["data-sort-method"] = "number";
                    G_DETALLE_FACTURA.HeaderRow.Cells[2].Attributes["data-sort-method"] = "number";
                    G_DETALLE_FACTURA.HeaderRow.Cells[1].Attributes["class"] = "sort-default";
                    G_DETALLE_FACTURA.HeaderRow.Cells[0].Attributes["class"] = "sort-default";

                }
                else if (almacenamiento_cobro_dias)
                {


                    //try
                    //{

                    //    monto_almacenamiento += Convert.ToDouble(Session["arraste_año_anterior"]);
                    //    Session["arraste_año_anterior"] = 0;
                    //    monto += double.Parse(e.Row.Cells[7].Text);
                    //    monto_almacenamiento += double.Parse(e.Row.Cells[7].Text);
                    //    e.Row.Cells[7].Text = Convert.ToInt32(e.Row.Cells[7].Text).ToString("N0");
                    //}
                    //catch { }
                    //G_DETALLE_FACTURA.HeaderRow.Cells[7].Text = "Total ( " + monto.ToString("N0") + " +  " + Convert.ToDouble(Session["arraste_año_anterior2"]).ToString("N0") + " = " + monto_almacenamiento.ToString("N0") + " )";
                    //G_DETALLE_FACTURA.HeaderRow.Cells[3].Attributes["data-sort-method"] = "number";
                    //G_DETALLE_FACTURA.HeaderRow.Cells[2].Attributes["data-sort-method"] = "number";
                    //G_DETALLE_FACTURA.HeaderRow.Cells[1].Attributes["class"] = "sort-default";
                    //G_DETALLE_FACTURA.HeaderRow.Cells[0].Attributes["class"] = "sort-default";

                }
                else if (es_historial_compra)
                {

                    //e.Row.Cells[0].Visible = false;
                    ////e.Row.Cells[].Visible = false;
                    //G_DETALLE_FACTURA.HeaderRow.Cells[0].Visible = false;

                    try
                    {
                        monto_cos += double.Parse(e.Row.Cells[4].Text);
                        G_DETALLE_FACTURA.HeaderRow.Cells[4].Text = "Costo = " + monto_cos.ToString("N0");
                    }
                    catch (Exception e6)
                    {

                    }
                    try
                    {
                        monto_import += double.Parse(e.Row.Cells[6].Text);
                        G_DETALLE_FACTURA.HeaderRow.Cells[6].Text = "CostoImpor = " + monto_import.ToString("N0");
                    }
                    catch (Exception e7)
                    {

                    }
                    try
                    {
                        monto_total += double.Parse(e.Row.Cells[8].Text);
                        G_DETALLE_FACTURA.HeaderRow.Cells[8].Text = "CostoTotal = " + monto_total.ToString("N0");
                    }
                    catch (Exception e8)
                    {

                    }

                    //sorter - false
                    if (e.Row.Cells[18].Text != "noootiene")
                    {
                        e.Row.Cells[1].Text = "<a data-toggle='tooltip' style='color:red;' data-placement='top' title='" + e.Row.Cells[18].Text + "'> " + e.Row.Cells[1].Text + "</a>";
                    }

                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");


                    string script1 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[13].Text), encriptador.EncryptData(""), encriptador.EncryptData(""), encriptador.EncryptData("54"));
                    e.Row.Cells[13].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[13].Text + " </a>";


                    //string script2 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[1].Text), encriptador.EncryptData(e.Row.Cells[11].Text), encriptador.EncryptData(""), encriptador.EncryptData("55"));
                    //e.Row.Cells[1].Text = "  <a href='javascript:' onclick='" + script2 + "'>" + e.Row.Cells[1].Text + " </a>";




                    e.Row.Cells[18].Visible = false;
                    //e.Row.Cells[].Visible = false;
                    G_DETALLE_FACTURA.HeaderRow.Cells[18].Visible = false;

                }
                else if (es_historial_total)
                {

                    //e.Row.Cells[0].Visible = false;
                    ////e.Row.Cells[].Visible = false;
                    //G_DETALLE_FACTURA.HeaderRow.Cells[0].Visible = false;

                    if (Session["fecha_color"].ToString() == e.Row.Cells[15].Text)
                    {
                        e.Row.BackColor = Color.FromArgb(219, 255, 76);

                    }

                    try
                    {
                        monto_cos += double.Parse(e.Row.Cells[4].Text);
                        G_DETALLE_FACTURA.HeaderRow.Cells[4].Text = "Costo = " + monto_cos.ToString("N0");
                    }
                    catch (Exception e6)
                    {

                    }
                    try
                    {
                        monto_import += double.Parse(e.Row.Cells[6].Text);
                        G_DETALLE_FACTURA.HeaderRow.Cells[6].Text = "CostoImpor = " + monto_import.ToString("N0");
                    }
                    catch (Exception e7)
                    {

                    }
                    try
                    {
                        monto_total += double.Parse(e.Row.Cells[8].Text);
                        G_DETALLE_FACTURA.HeaderRow.Cells[8].Text = "CostoTotal = " + monto_total.ToString("N0");
                    }
                    catch (Exception e8)
                    {

                    }

                    //sorter - false
                    if (e.Row.Cells[17].Text != "noootiene")
                    {
                        e.Row.Cells[1].Text = "<a data-toggle='tooltip' style='color:red;' data-placement='top' title='" + e.Row.Cells[17].Text + "'> " + e.Row.Cells[1].Text + "</a>";
                    }

                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");


                    string script1 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[12].Text), encriptador.EncryptData(""), encriptador.EncryptData(""), encriptador.EncryptData("54"));
                    e.Row.Cells[12].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[12].Text + " </a>";


                    //string script2 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[1].Text), encriptador.EncryptData(e.Row.Cells[11].Text), encriptador.EncryptData(""), encriptador.EncryptData("55"));
                    //e.Row.Cells[1].Text = "  <a href='javascript:' onclick='" + script2 + "'>" + e.Row.Cells[1].Text + " </a>";




                    e.Row.Cells[17].Visible = false;
                    //e.Row.Cells[].Visible = false;
                    G_DETALLE_FACTURA.HeaderRow.Cells[17].Visible = false;


                }
                else if (es_venta_movil)
                {

                    if (e.Row.Cells[0].Text != "0,000")
                    {

                        e.Row.Cells[0].Style.Value = "color:red";
                    }

                }
                else if (es_permanencia)
                {

                    e.Row.Cells[2].Text = e.Row.Cells[2].Text.Remove(11).Trim();
                    e.Row.Cells[8].Text = Base.monto_format(e.Row.Cells[8].Text);

                    e.Row.Cells[9].Visible = false;
                    //e.Row.Cells[].Visible = false;
                    G_DETALLE_FACTURA.HeaderRow.Cells[9].Visible = false;
                }


                else
                {
                    if (es_ficha)
                    {
                        e.Row.Cells[0].Visible = false;
                        G_DETALLE_FACTURA.HeaderRow.Cells[0].Visible = false;

                    }
                    try
                    {
                        monto += double.Parse(e.Row.Cells[1].Text);
                        e.Row.Cells[1].Text = Convert.ToInt32(e.Row.Cells[1].Text).ToString("N0");
                    }
                    catch { }
                    if (!es_costo_import)
                    {
                        G_DETALLE_FACTURA.HeaderRow.Cells[1].Text = "Venta ( " + monto.ToString("N0") + " )";
                        G_DETALLE_FACTURA.HeaderRow.Cells[1].Attributes["data-sort-method"] = "number";
                        G_DETALLE_FACTURA.HeaderRow.Cells[0].Attributes["class"] = "sort-default";
                    }
                    if (!es_matriz)
                    {
                        clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                        string script1 = string.Format("javascript:fuera22(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(ReporteRNegocio.tre_rut_cliente(e.Row.Cells[0].Text).Trim()), encriptador.EncryptData("88"));
                        e.Row.Cells[0].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[0].Text + " </a>";
                    }

                    G_DETALLE_FACTURA.HeaderRow.Cells[2].Attributes["class"] = "sort-default";
                    try
                    {
                        G_DETALLE_FACTURA.HeaderRow.Cells[3].Attributes["class"] = "sort-default";
                        G_DETALLE_FACTURA.HeaderRow.Cells[4].Attributes["class"] = "sort-default";
                    }
                    catch { }
                }





                if (columna_fac)
                {
                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                    string año_factura = ReporteRNegocio.trae_año_factura(e.Row.Cells[COLUMNA_DE_FACTURA].Text);
                    año_factura = año_factura.Substring(0, 4);
                    string script = string.Format("javascript:fuera3(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[COLUMNA_DE_FACTURA].Text), encriptador.EncryptData(año_factura));
                    e.Row.Cells[COLUMNA_DE_FACTURA].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[COLUMNA_DE_FACTURA].Text + " </a>";

                }




            }
        }

        protected void G_PRODUCTOS_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void G_PRODUCTOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                G_PRODUCTOS.HeaderRow.Cells[0].Attributes["data-sort-method"] = "number";
                G_PRODUCTOS.HeaderRow.Cells[1].Attributes["data-sort-method"] = "number";
                G_PRODUCTOS.HeaderRow.Cells[2].Attributes["class"] = "sort-default";
                G_PRODUCTOS.HeaderRow.Cells[3].Attributes["class"] = "sort-default";
                G_PRODUCTOS.HeaderRow.Cells[4].Attributes["data-sort-method"] = "number";

                e.Row.Cells[0].Text = cont_det.ToString();
                cont_det++;

                double d;
                double.TryParse(e.Row.Cells[4].Text, out d);
                string aux = "";
                if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                e.Row.Cells[4].Text = aux;
                sum_total += d;
            }
        }

        protected void descarga_word_Click(object sender, EventArgs e)
        {
            try
            {

                //Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
                //appWord.Documents.Open(@"D:\\bk_thx_tebo\\BK_thx_ventamovil\\Archivos\\" + fecha.Trim() + ".doc").ExportAsFixedFormat(@"D:\\bk_thx_tebo\\BK_thx_ventamovil\\Archivos\\" + fecha.Trim() + ".doc", Word.WdExportFormat.wdExportFormatPDF);


                string nom_factura = Session["sp"].ToString().Trim() + ".doc";
                string ruta = "";

                TheDownload2("D:\\bk_thx_tebo\\BK_thx_ventamovil\\BK_thx_ventamovil\\Archivos\\" + nom_factura.Trim(), ruta, nom_factura.Trim());

                //Word.Application app = new Microsoft.Office.Interop.Word.Application();
                //app.Documents.Add();
                //app.Documents.Open("D:\\bk_thx_tebo\\BK_thx_ventamovil\\Archivos\\" + fecha.Trim() + ".doc");

            }
            catch (Exception ee)
            {

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> alert('No se Ha encontrado el Documento " + ee.Message + "') </script>", false);

            }
        }


        protected void descarga_pdf_Click(object sender, EventArgs e)
        {
            try
            {
                string SP = Session["sp"].ToString().Trim();
                string n_file = "/" + SP.Trim() + ".pdf";
                string pdfPath = Server.MapPath(@"~\PDFs\") + n_file;
                Base.crear_sp_pdf(SP, pdfPath);
                TheDownload2(pdfPath, "", SP.Trim() + ".pdf");
            }
            catch (Exception ee)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> alert('No se Ha encontrado el Documento " + ee.Message + "') </script>", false);
            }
        }



    }
}