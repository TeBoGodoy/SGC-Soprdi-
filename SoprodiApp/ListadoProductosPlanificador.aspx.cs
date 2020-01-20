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
using System.Web.Services;
using System.Net.Mail;
using System.Net;

namespace SoprodiApp
{
    public partial class ListadoProductosPlanificador : System.Web.UI.Page
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
        public static int cont_det;
        public static double sum_total;
        public static string fecha;

        double monto_cos = 0;
        double monto_import = 0;
        double monto_total = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                es_costo_import = false;

                USER = User.Identity.Name.ToString();

                Session["user"] = USER;

                string sp = Request.QueryString["c"].ToString();
                string codbodega = Request.QueryString["b"].ToString().Trim();
                string asignado_por = Request.QueryString["a"].ToString().Trim();

                if (asignado_por == "ABARROTES")
                {
                    Session["SW_PERMI"] = "1";
                }
                else if (asignado_por == "GRANOS")
                {
                    Session["SW_PERMI"] = "2";
                }

                Session["cod_codega"] = codbodega;
                Session["asignado_por"] = asignado_por;
                bodega.InnerText = "(" + codbodega + ")";

                Session["sp_planificada"] = sp.Trim();

                //string estado = Request.QueryString["c"].ToString();

                correo_vendedor.InnerText = ReporteRNegocio.trae_correo_sp(sp);
                cliente_2.InnerText = ReporteRNegocio.trae_cliente_sp(sp);
                t_fecha_despacho.Text = ReporteRNegocio.trae_fecha_sp(sp).Trim();

                estado_sp.Text = buscar_Estado(sp);

                cargar_detalle(sp, asignado_por);
                try
                {

                    check_tran.Text = "";
                }
                catch { }
                cargar_bodega(codbodega);

                tx_vuelta.Text = "1";

                DataTable dt2 = ReporteRNegocio.listar_camiones_asignados("  and coddocumento in (" + agregra_comillas(sp) + ")");
                G_INFORME_TOTAL_VENDEDOR.DataSource = dt2;
                G_INFORME_TOTAL_VENDEDOR.DataBind();
                JQ_Datatable();


            }
        }
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

        private void cargar_bodega(string bodega)
        {
            string where = " where 1=1 ";

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.carga_bodega(where);
            dtv = dt.DefaultView;
            d_bodega.DataSource = dtv;
            d_bodega.DataTextField = "nom_bodega";
            d_bodega.DataValueField = "nom_bodega";
            d_bodega.DataBind();
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

        private void cargar_detalle(string sp, string asignado_por)
        {
            normal.Attributes["style"] = "display:block";
            factura.Attributes["style"] = "display:none";
            DateTime t = DateTime.Now;
            DateTime t2 = DateTime.Now;
            t = new DateTime(t.Year, t.Month, 1);
            string DESDE = t.ToShortDateString();
            string HASTA = t2.ToShortDateString();

            DESDE = DESDE.Replace("-", "/");
            HASTA = HASTA.Replace("-", "/");

            //aca detalle sp
            monto_cos = 0;
            monto_import = 0;
            monto_total = 0;
            es_reporte_cliente = false;
            //es_producto_grilla = false;
            es_reporte_vendedor = false;
            es_nuevo_click_montos = false;
            almacenamiento_cobro = false;
            es_matriz = false;
            es_costo_import = false;
            es_historial_compra = false;
            es_historial_total = false;
            es_venta_movil = true;
            detalle.InnerText = "Detalle Venta Movil: " + sp;
            //Session["fecha_color"] = vendedor;

            //fecha =  cod
            //vendedor = fecha
            //AbrirArchivo("\\\\192.168.10.22\\Archivos\\" + fecha.Trim() + ".doc");
            //AbrirArchivo("\\\\192.168.10.22\\Archivos\\" + fecha.Trim() + ".doc");

            string where3 = " where b.coddocumento in (" + agregra_comillas(sp) + ")";

            DataTable dt2 = ReporteRNegocio.VM_listar_detalle_sp(where3);
            DataTable dt1 = ReporteRNegocio.VM_LISTAR_SP_2(where3.Replace("b.", ""));

            string nota_libre = dt1.Rows[0]["NotaLibre"].ToString();
            tx_obs_plani.Text = nota_libre;

            G_DETALLE_FACTURA.DataSource = dt2;
            G_DETALLE_FACTURA.DataBind();

            string bodega = "";
            if (dt2.Rows.Count > 0)
            {
                //string html = cargar_camiones(bodega);

                //div_camiones.InnerHtml = html;

                string clase = "";

                if (asignado_por == "GRANOS")
                {
                    clase = "  and b.codproducto < '1000' ";
                }
                else
                {
                    clase = "  and b.codproducto > '1000' ";
                }

                cargar_productos_no_kg(clase);
            }

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('G_DETALLE_FACTURA')); </script>", false);

            return;



        }

        private void cargar_productos_no_kg(string clase)
        {
            //                " and FechaFactura <= CONVERT(datetime,'" + HASTA + "',103) and user1 in (" + grupos + ")";

            DataTable dt; DataView dtv = new DataView();

            dt = ReporteRNegocio.listar_ALL_productos_no_kg_(" and b.codunventa <>'KGR' and b.codunventa <>'TON' ", clase);
            dtv = dt.DefaultView;
            cb_productos_kg.DataSource = dtv;
            cb_productos_kg.DataTextField = "descr";
            cb_productos_kg.DataValueField = "codproducto";
            //d_vendedor_.SelectedIndex = -1;
            cb_productos_kg.DataBind();
        }


        private string cargar_camiones(string bodega)
        {

            string tabla = "";


            //tabla += "<div class='container'>";
            //tabla += "<div class='row'>";
            //tabla += "<div class='col-md-6 col-md-offset-3'>";
            //tabla += "<div class='row'>";

            DataTable dt_transpor = ReporteRNegocio.trae_transportistas("");


            tabla += "		    <div class='row'> ";


            foreach (DataRow r in dt_transpor.Rows)
            {

                tabla += "	    		<div class='col-xs-2' style='height: 24%;'>";
                tabla += "	    		    <img src='img/icon.png' class='img-responsive img-radio' style='max-width: 38% !important;'>";
                string script1 = string.Format("javascript:check_trans(&#39;{0}&#39;);return false;", r[0].ToString().Trim() + " * " + r[5].ToString().Trim());
                tabla += "	   				<button type='button' onclick='" + script1 + "' class='btn btn-primary btn-radio' style='white-space: normal;' style='font-size: 13px !important;'>" + r[1].ToString().Trim() + " </button>";
                tabla += "	   				<input type='checkbox' name='transportistas' class='hidden'>";
                tabla += "	  			</div>";

            }

            tabla += "			</div>";

            return tabla;
        }

        //

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

        public class RESPUESTA_UNIDAD
        {
            public string stkunit { get; set; }
            public string valor { get; set; }
            public string unidades { get; set; }
        }


        [WebMethod]
        public static List<RESPUESTA_UNIDAD> carga_click(string sw)
        {



            DataTable dt = new DataTable();

            try
            {

                dt = ReporteRNegocio.valor_prod_equivale_sp(sw);


            }
            catch { }
            //}
            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new RESPUESTA_UNIDAD
                        {
                            stkunit = Convert.ToString(item["stkunit"]),
                            valor = Convert.ToString(item["valor"]),
                            unidades = Convert.ToString(item["unidad_equivale"]),
                        };
            return query.ToList<RESPUESTA_UNIDAD>();
        }


        [WebMethod]
        public static string guardar_solo_kg(string sw, string tipo, string valor, string unidades)
        {

            DataTable dt = new DataTable();
            string respt = "";
            try
            {
                string oka = ReporteRNegocio.guardar_valor_equivale_sp(sw, tipo, valor, unidades);

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


        protected void G_DETALLE_FACTURA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                if (e.Row.Cells[2].Text == "KGR")
                {
                    e.Row.Cells[3].Text = "";
                }


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
        protected void sellectAll(object sender, EventArgs e)
        {

        }

        protected void chkb1_CheckedChanged(object sender, EventArgs e)
        {
            double total_suma = 0;
            CheckBox ChkBoxHeader = (CheckBox)G_DETALLE_FACTURA.HeaderRow.FindControl("chkb1");
            foreach (GridViewRow row in G_DETALLE_FACTURA.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkAccept");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;

                    TextBox tx = (TextBox)G_DETALLE_FACTURA.Rows[row.RowIndex].FindControl("txt_a_desp");

                    string tipo_unidad = G_DETALLE_FACTURA.Rows[row.RowIndex].Cells[2].Text.Trim();

                    string equivale = G_DETALLE_FACTURA.Rows[row.RowIndex].Cells[3].Text;
                    double sum = Convert.ToDouble(tx.Text);

                    //if (tipo_unidad == "TON")
                    //{
                    //    sum = sum * 1000;
                    //}

                    if (equivale != "0,00" && equivale != "")
                    {
                        double equivale_ = Convert.ToDouble(equivale);
                        sum = sum * equivale_;

                    }



                    total_suma += sum;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('G_DETALLE_FACTURA')); </script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "tee6ee", "<script> activa_camiones(); </script>", false);


            l_suma.Text = total_suma.ToString();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "load_chosen_combos", "<script> load_chosen_combos();</script>", false);

        }


        protected void btn_refresh_Click(object sender, EventArgs e)
        {
            string where3 = " where b.coddocumento in (" + agregra_comillas(detalle.InnerText.Replace("Detalle Venta Movil: ", "").Trim()) + ")";

            DataTable dt2 = ReporteRNegocio.VM_listar_detalle_sp(where3);
            G_DETALLE_FACTURA.DataSource = dt2;
            G_DETALLE_FACTURA.DataBind();
        }

        protected void chkAccept_CheckedChanged(object sender, EventArgs e)
        {

            double total_suma = Convert.ToDouble(l_suma.Text);
            CheckBox rb = (CheckBox)sender;
            if (rb != null)
            {
                if (rb.Checked)
                {
                    // Only one radio button will be checked
                    //Console.WriteLine("Changed: " + rb.Name);

                    GridViewRow row = ((GridViewRow)((DataControlFieldCell)rb.Parent).Parent);

                    TextBox tx = (TextBox)G_DETALLE_FACTURA.Rows[row.RowIndex].FindControl("txt_a_desp");
                    string equivale = G_DETALLE_FACTURA.Rows[row.RowIndex].Cells[3].Text;
                    double sum = Convert.ToDouble(tx.Text);

                    if (equivale != "0,00" && equivale != "")
                    {
                        double equivale_ = Convert.ToDouble(equivale);
                        sum = sum * equivale_;

                    }

                    total_suma += sum;
                }
                else
                {

                    GridViewRow row = ((GridViewRow)((DataControlFieldCell)rb.Parent).Parent);

                    TextBox tx = (TextBox)G_DETALLE_FACTURA.Rows[row.RowIndex].FindControl("txt_a_desp");
                    string equivale = G_DETALLE_FACTURA.Rows[row.RowIndex].Cells[3].Text;
                    double sum = Convert.ToDouble(tx.Text);


                    if (equivale != "0,00" && equivale != "")
                    {
                        double equivale_ = Convert.ToDouble(equivale);
                        sum = sum * equivale_;
                    }

                    total_suma -= sum;

                }
            }

            l_suma.Text = total_suma + "";


            ScriptManager.RegisterStartupScript(Page, this.GetType(), "load_chosen_combos", "<script> load_chosen_combos();</script>", false);




        }

        protected void B_enviar_Click(object sender, ImageClickEventArgs e)
        {
            string fecha = t_fecha_despacho.Text;
            string carga_inicial = inputTitle.Text;
            string cod_trans = l_transpor.Text;
            string patente = ReporteRNegocio.nombre_camion(" where cod_camion = " + l_camion.Text);
            string chofer = ReporteRNegocio.nombre_chofer(" where cod_chofer = " + l_chofer.Text);
            string nombre_trans = ReporteRNegocio.nombre_transporte(" where cod_trans = " + cod_trans);

            string transporte_completo = "(" + patente + ") " + nombre_trans + " Chofer: " + chofer;

            string sp = detalle.InnerText.Replace("Detalle Venta Movil: ", "").Trim();
            string disponible = "";

            string cod_camion = l_camion.Text;
            string cod_chofer = l_chofer.Text;
            string obs = tx_obs_plani.Text;
            string carga_total = l_suma.Text;
            string orden_cargar = tx_orden.Text;
            string vuelta = tx_vuelta.Text;

            try
            {
                disponible = (Convert.ToDouble(carga_inicial) - Convert.ToDouble(l_suma.Text)).ToString();
            }
            catch { }
            if (fecha != "")
            {
                if (carga_inicial != "")
                {

                    if (cod_trans != "")
                    {
                        if (l_suma.Text != "0")
                        {

                            //bool permite_seguir = validar_disponibles_planificados();
                            bool permite_seguir = true;
                            if (permite_seguir)
                            {
                                string asignado_por = Session["asignado_por"].ToString();


                                string select_scope = ReporteRNegocio.insert_encab_sp_asig(sp, cod_trans, carga_inicial, fecha, disponible, "1", cod_camion, cod_chofer, obs, asignado_por, carga_total, orden_cargar, vuelta);
                                //string select_scope = "45";
                                double total_suma = 0;
                                foreach (GridViewRow row in G_DETALLE_FACTURA.Rows)
                                {
                                    CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkAccept");

                                    if (ChkBoxRows.Checked)
                                    {
                                        TextBox tx = (TextBox)G_DETALLE_FACTURA.Rows[row.RowIndex].FindControl("txt_a_desp");
                                        double sum = Convert.ToDouble(tx.Text);
                                        string cod_prod = G_DETALLE_FACTURA.Rows[row.RowIndex].Cells[1].Text;
                                        string tipo_unidad = G_DETALLE_FACTURA.Rows[row.RowIndex].Cells[2].Text.Trim();

                                        //if (tipo_unidad == "TON")
                                        //{
                                        //    sum = sum * 1000;
                                        //}

                                        total_suma += sum;

                                        string ok = ReporteRNegocio.insert_det_sp_asig(select_scope, sum, cod_prod);


                                    }


                                }
                                if (select_scope != "")
                                {

                                    //string correo_vend = ReporteRNegocio.trae_correo_sp( agregra_comillas( sp ) );

                                    notificar_vendedor(correo_vendedor.InnerText, select_scope, sp, fecha, transporte_completo, obs, nombre_trans, patente, chofer);

                                    DataTable dt2 = ReporteRNegocio.listar_camiones_asignados("  and coddocumento in (" + agregra_comillas(sp) + ")");
                                    G_INFORME_TOTAL_VENDEDOR.DataSource = dt2;
                                    G_INFORME_TOTAL_VENDEDOR.DataBind();
                                    JQ_Datatable();



                                }

                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, this.GetType(), "tee6ee", "<script> alert('Ha ingresado valor sobre lo disponible a Planificar');</script>", false);



                            }
                        }
                        else
                        {

                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "tee6ee", "<script> alert('Seleccionar productos');</script>", false);
                        }

                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "tee6ee", "<script> alert('Ingresar al menos un transportista');</script>", false);
                    }
                }
                else
                {

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "tee6ee", "<script> alert('Ingresar Carga Inicial');</script>", false);
                }
            }
            else
            {

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "tee6ee", "<script> alert('Ingresar Fecha Despacho');</script>", false);
            }

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "load_chosen_combos2", "<script> load_chosen_combos();</script>", false);

            string aux = "";


        }

        private bool validar_disponibles_planificados()
        {
            bool valida = true;
            double total_suma = 0;
            foreach (GridViewRow row in G_DETALLE_FACTURA.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkAccept");

                if (ChkBoxRows.Checked)
                {
                    TextBox tx = (TextBox)G_DETALLE_FACTURA.Rows[row.RowIndex].FindControl("txt_a_desp");
                    double planific = Convert.ToDouble(tx.Text);
                    double disponible_planif = Convert.ToDouble(G_DETALLE_FACTURA.Rows[row.RowIndex].Cells[11].Text);

                    if (planific > disponible_planif)
                    {
                        return false;
                    }
                }
            }
            return valida;
        }

        private void notificar_vendedor(string correo_vend, string select_scope, string sp, string fecha, string nombre_trans, string obs, string transportista, string camion, string chofer)
        {

            DataTable produ_desch = ReporteRNegocio.lista_det_sp_asignada(select_scope);
            string total_kg = ReporteRNegocio.total_kg_id(select_scope);


            DataTable dat_sp = ReporteRNegocio.VM_LISTAR_SP_2(" where coddocumento = '" + sp + "'");
            DataTable dat_detalle_Sp = ReporteRNegocio.VM_listar_detalle_sp_2(" where coddocumento = '" + sp + "'");

            string tabla_transporte = "";

            tabla_transporte += "<div>  <b> Transportista: " + transportista + "</b> <br><br>";
            tabla_transporte += "<div>  <b> Camión:        " + camion + "</b> <br><br>";
            tabla_transporte += "<div>  <b> Chofer:        " + chofer + "</b> <br><br>";


            string tabla = "";
            tabla += "<table class=\"table fill-head table-bordered\" style=\"width:100%;\">";
            tabla += "<thead class=\"test\" style=\"background-color:rgb(156,205,249)\">";
            tabla += "<tr>";

            tabla += "<th>Código</th>";
            tabla += "<th>Nombre</th>";
            tabla += "<th>CantDespachado</th>";
            tabla += "<th>TipoUnidad</th>";
            tabla += "<th>PrecioUnitario</th>";
            tabla += "<th>Descuento</th>";
            tabla += "<th>PrecioUnitarioFinal</th>";
            tabla += "<th>Neto</th>";

            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            double total_neto = 0;
            foreach (DataRow dr in produ_desch.Rows)
            {
                string tipo_unidad = dr["SP_CodUnVenta"].ToString().Replace(",000", "");
                if (tipo_unidad == "GxDef")
                {
                    tipo_unidad = "Otra";
                }
                double despachado = 0;
                try
                {
                    despachado = Convert.ToDouble(dr["despachado"].ToString().Replace(",000", ""));
                }
                catch
                {

                }
                if (tipo_unidad == "TON")
                {
                    despachado = despachado / 1000;
                }
                tabla += "<tr>";
                tabla += "<td>" + dr["codproducto"].ToString() + "</td>";
                tabla += "<td>" + dr["SP_descproducto"].ToString().Replace(",000", "") + "</td>";
                tabla += "<td>" + Math.Round(despachado, 2) + "</td>";
                tabla += "<td>" + tipo_unidad + "</td>";
                tabla += "<td>" + dr["SP_preciounitario"].ToString().Replace(",000", "") + "</td>";
                tabla += "<td>" + dr["SP_descuento"].ToString().Replace(",000", "") + "</td>";
                tabla += "<td>" + dr["SP_preciounitariofinal"].ToString().Replace(",000", "") + "</td>";
                double monto_neto = 0;
                try
                {
                    monto_neto = Convert.ToDouble(dr["montonetofinal"].ToString());
                    total_neto += monto_neto;
                }
                catch
                {
                }
                tabla += "<td>" + Base.monto_format2(monto_neto) + "</td>";
                tabla += "</tr>";
            }
            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");

            string SP_formato = Base.crear_sp_formato(dat_sp, dat_detalle_Sp);
            //agregamos correos de bodegas, 
            string grupo = "";
            if (correo_vend != "")
            {
                correo_vend += ",";
            }
            try
            {
                if (Session["asignado_por"].ToString().Contains("ABARROTES"))
                {
                    grupo = "'ABARROTES'";
                }
                else if (Session["asignado_por"].ToString().Contains("GRANOS"))
                {
                    grupo = "'GRANOS'";
                }
                string correos_bodega = ReporteRNegocio.trae_correos_bodega(dat_sp.Rows[0]["CodBodega"].ToString().Trim(), grupo);

                if (correos_bodega != "")
                {
                    correo_vend += correos_bodega;
                }
                else
                {
                    correo_vend = correo_vend.Trim();
                    correo_vend = correo_vend.Substring(0, correo_vend.Length - 1);
                }
            }
            catch { }

            //enviar_email(tabla, correo_vend, sp, fecha, fecha_emi, nombre_trans, fecha_desp, total_kg, SP_formato, total_neto, datos_comuna_, obs, vendedor, moneda_sp);
            enviar_email(tabla, correo_vend, sp, fecha, "", tabla_transporte, "", total_kg, SP_formato, total_neto, "", obs, "", "");
            
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "tee6ee", "<script> alert('SP ASIGNADA'); document.getElementById('BTN_REFRESH_2').click();</script>", false);
        }


        private string buscar_Estado(string sp)
        {
            string estado_ = "";

            //SPSP
            DataTable dt2 = ReporteRNegocio.VM_listar_sp_select("where  1=1 and b.CodDocumento in (" + agregra_comillas(sp) + ")");

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

                if (r[12].ToString() == "Aprobado" && r[27].ToString() == "no")
                {
                    if (cod_aux == "")
                    {
                        cod_aux = r[0].ToString();
                        sp_for = new DataTable();
                        sp_for.Columns.Add("sp");
                        sp_for.Columns.Add("estado");
                        sp_for.Columns.Add("facturas");
                    }

                    DataTable procesado = ReporteRNegocio.SP_Marcelo(r[0].ToString().Trim(), r[25].ToString().Trim(), r[26].ToString().Trim());
                    DataRow row = procesado.Rows[0];

                    if (r[0].ToString() == cod_aux)
                    {
                        //if (row["estado"].ToString().Substring(0, 3) == "10P")
                        //{

                        facturas_aux = row["facturas"].ToString();

                        if (facturas_aux.Trim() != "")
                        {
                            facturas_x_sps += row["facturas"].ToString() + ", ";
                            r[13] = facturas_x_sps.Substring(0, facturas_x_sps.Length - 3);
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

                        cod_aux = r[0].ToString();
                        sp_for = new DataTable();
                        sp_for.Columns.Add("sp");
                        sp_for.Columns.Add("estado");
                        sp_for.Columns.Add("facturas");
                        facturas_x_sps = "";

                        facturas_x_sps += row["facturas"].ToString() + ", ";
                        r[13] = facturas_x_sps.Substring(0, facturas_x_sps.Length - 2);

                        DataRow row_sp1 = sp_for.NewRow();
                        row_sp1["sp"] = cod_aux;
                        row_sp1["estado"] = row["estado"].ToString().Substring(0, 3);
                        row_sp1["facturas"] = facturas_x_sps.Substring(0, facturas_x_sps.Length - 2);
                        sp_for.Rows.Add(row_sp1);


                    }



                }
                else if (r[12].ToString() == "Aprobado")
                {


                    DataTable procesado = ReporteRNegocio.SP_Marcelo(r[0].ToString().Trim(), r[25].ToString().Trim(), r[26].ToString().Trim());
                    DataRow row = procesado.Rows[0];
                    r[13] = row["facturas"].ToString();




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
                if (r[12].ToString() == "Aprobado")
                {



                    foreach (DataRow r2 in sp_malas.Rows)
                    {
                        if (r[0].ToString() == r2[0].ToString())
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

                                r[13] = cadd;
                                r[24] = "10P";

                            }
                            else
                            {

                                r[13] = cadd;
                                r[24] = r2[2].ToString();
                            }


                        }
                    }
                }
                r[13] = r[13].ToString().Replace(",", ", ");
            }

            if (dt2.Rows.Count > 0)
            {

                dt2 = dt2.AsEnumerable()
                       .GroupBy(r => new { Col1 = r["CodDocumento"] })
                       .Select(g => g.OrderBy(r => r["CodDocumento"]).First())
                       .CopyToDataTable();
            }


            foreach (DataRow r33 in dt2.Rows)
            {

                estado_ = r33[24].ToString();
                if (estado_.Trim() == "10P")
                {
                    estado_ = "Aprob. Parcial";
                }
                else if (estado_.Trim() == "10S")
                {
                    estado_ = "Aprob. S/Fact";
                }
                else if (estado_.Trim() == "10")
                {
                    estado_ = "Sincronizado";
                }
                else if (estado_.Trim() == "20")
                {
                    estado_ = "Aprobado";
                }
                else if (estado_.Trim() == "30")
                {

                    estado_ = "Rechazado";
                }
                else
                {

                    estado_ = estado_;
                }
                break;
            }
            return estado_;
        }

        private void enviar_email(string html, string EmailVendedor, string sp, string fecha, string fecha_emi, string nombre_trans,
            string fecha_despah, string total_kg, string sp_tabla, double total_neto, string dato_comuna, string obs, string vendedor, string moneda)
        {
            MailMessage email = new MailMessage();

            EmailVendedor = EmailVendedor.Replace(";", ",");

            if (EmailVendedor.Trim() == "")
            {
                //EmailVendedor = "rmc@soprodi.cl";
            }
            //try
            //{
            //    //correos = correos.Replace(";", ",");
            //}
            //catch { }
            //CORREO-CAMBIAR
            //email.To.Add(new MailAddress("egodoy@soprodi.cl"));
            //email.To.Add(new MailAddress("daniel@soprodi.cl"));
            //email.To.Add(new MailAddress("esteban.godoy15@gmail.com"));
            email.To.Add(new MailAddress("informatica@soprodi.cl"));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "SP Asignada " + sp + " " + cliente_2.InnerText + "( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
            //email.CC.Add("informatica@soprodi.cl");
            //email.CC.Add("informatica@soprodi.cl");
            //CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCccccccccccccccccc

            if (Session["asignado_por"].ToString() == "ABARROTES")
            {
                if (EmailVendedor.Trim() == "")
                {
                    email.CC.Add("mazocar@soprodi.cl, rmc@soprodi.cl, gmorales@soprodi.cl, pcataldo@soprodi.cl");
                }
                else
                {
                    email.CC.Add("mazocar@soprodi.cl, rmc@soprodi.cl, gmorales@soprodi.cl, pcataldo@soprodi.cl, " + EmailVendedor);
                }
            }
            else
            {
                if (EmailVendedor.Trim() == "")
                {
                    email.CC.Add("MRAMIREZ@soprodi.cl");
                }
                else
                {
                    email.CC.Add("MRAMIREZ@soprodi.cl, " + EmailVendedor);
                }
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
            //usuario que envia
            email.Body += "<div><span style='float: left;font-size:7pt;'>" + ReporteRNegocio.corr_usuario(User.Identity.Name).Rows[0][0] + " </span></div>";
            email.Body += "<div><img src='http://a58.imgup.net/Sopro4d9d.png' style='    float: right;     width: 90px;'> </div><br><br><br>";
            email.Body += "<div> <p style='font-size: 22px;' ><b>PLANIFICACIÓN </b></p><br> <br>  <b> </b>";
            email.Body += "<div>  <b style='font-size: 20px;color: #d25557;'> SP: " + sp + "</b> <br>";
            email.Body += nombre_trans + "<br>";
            email.Body += "<div> Se ha planificiado con fecha <b style='background-color: yellow;'> " + fecha + "</b> el siguiente detalle de SP:  <br><br>";
            email.Body += html;
            email.Body += "<div>   <b style='background-color: yellow;'> Observación      : " + obs + "</b> <br><br>";
            email.Body += "<div><br><br> </div> ";
            email.Body += sp_tabla;
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
                string n_file = sp.Trim() + ".pdf";
                string pdfPath = Server.MapPath(@"~\PDFs\") + n_file;
                Base.crear_sp_pdf(sp, pdfPath);
                System.IO.FileInfo toDownload = new System.IO.FileInfo(pdfPath);
                if (toDownload.Exists)
                {
                    email.Attachments.Add(new System.Net.Mail.Attachment(pdfPath));
                }
                smtp.Send(email);
                email.Dispose();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>alert('CORREO ENVIADO!');</script>", false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "CORREO_PLANI", "<script>alert('ERROR AL ENVIAR CORREO!');</script>", false);
            }

            /////desde gmail

            //email.IsBodyHtml = true;
            //email.Priority = MailPriority.Normal;
            //email.BodyEncoding = System.Text.Encoding.UTF8;
            //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            //smtp.EnableSsl = true;
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.UseDefaultCredentials = false;
            //smtp.Credentials = new NetworkCredential("informes.soprodi@gmail.com", "galaxia1234");
            //try
            //{
            //    string n_file = sp.Trim() + ".pdf";
            //    string pdfPath = Server.MapPath(@"~\PDFs\") + n_file;
            //    Base.crear_sp_pdf(sp, pdfPath);

            //    System.IO.FileInfo toDownload = new System.IO.FileInfo(pdfPath);
            //    if (toDownload.Exists)
            //    {
            //        email.Attachments.Add(new System.Net.Mail.Attachment(pdfPath));
            //    }
            //    smtp.Send(email);
            //    email.Dispose();
            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>alert('CORREO ENVIADO!');</script>", false);
            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "CORREO_PLANI", "<script>alert('ERROR AL ENVIAR CORREO!');</script>", false);
            //}

        }


        public class Transportista
        {
            public string cod_trans { get; set; }
            public string nombre_trans { get; set; }

        }
        public class Camion
        {
            public string cod_camion { get; set; }
            public string patente { get; set; }
            public string carga { get; set; }
        }

        public class Chofer
        {
            public string cod_chofer { get; set; }
            public string nombre_chofer { get; set; }
        }





        [WebMethod]
        public static List<Transportista> BODEGA(string BODEGA)
        {
            DataTable dt = new DataTable();

            string where = " where 1=1 and  cod_bodega = '" + BODEGA + "'";
            //Session["asignado_por"]

            where += " and grupo = '" + HttpContext.Current.Session["asignado_por"].ToString() + "' ";

            try
            {
                dt = ReporteRNegocio.listar_transpor_2(where);
                DataView dv = dt.DefaultView;
                dv.Sort = "nom_vend";
                dt = dv.ToTable();
            }
            catch { }


            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new Transportista
                        {
                            cod_trans = Convert.ToString(item["cod_trans"]),
                            nombre_trans = Convert.ToString(item["nombre_trans"])


                        };
            return query.ToList<Transportista>();
        }


        [WebMethod]
        public static List<Chofer> TRANSPOR_CHOFER(string TRANSPOR)
        {
            DataTable dt = new DataTable();

            string where = " and cod_trans = '" + TRANSPOR + "'";

            try
            {
                dt = ReporteRNegocio.listar_chofer(where);
                DataView dv = dt.DefaultView;
                dv.Sort = "nombre_chofer";
                dt = dv.ToTable();
            }
            catch { }

            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new Chofer
                        {
                            cod_chofer = Convert.ToString(item["cod_chofer"]),
                            nombre_chofer = Convert.ToString(item["nombre_chofer"])


                        };
            return query.ToList<Chofer>();
        }

        [WebMethod]
        public static List<Camion> TRANSPOR_CAMION(string TRANSPOR)
        {
            DataTable dt = new DataTable();

            string where = " and cod_trans = '" + TRANSPOR + "'";

            try
            {
                dt = ReporteRNegocio.listar_camion(where);
                DataView dv = dt.DefaultView;
                dv.Sort = "patente";
                dt = dv.ToTable();
            }
            catch { }

            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new Camion
                        {
                            cod_camion = Convert.ToString(item["cod_camion"]),
                            patente = Convert.ToString(item["patente"])


                        };
            return query.ToList<Camion>();
        }



        [WebMethod]
        public static string CAMION_CARGA(string CAMION)
        {
            string carga_inicial = "";

            DataTable dt_camion = ReporteRNegocio.listar_camion(" and cod_camion = '" + CAMION + "'");

            carga_inicial = dt_camion.Rows[0][3].ToString();

            return carga_inicial;
        }

        [WebMethod]
        public static string CARGA_INICIAL_TRANSPORTE(string TRANSPOR)
        {
            string carga_inicial = "";

            DataTable dt_transporte = ReporteRNegocio.trae_transportistas(" and cod_trans = '" + TRANSPOR + "'");

            carga_inicial = dt_transporte.Rows[0][5].ToString();

            return carga_inicial;
        }


        protected void d_transpor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string where = " and cod_trans = '" + d_transpor.SelectedValue + "'";
            DataTable dt_chofer = new DataTable();
            DataTable dt_camion = new DataTable();
            DataView dtv = new DataView();
            try
            {
                dt_chofer = ReporteRNegocio.listar_chofer(where);
                DataView dv = dt_chofer.DefaultView;
                dv.Sort = "nombre_chofer";
                dt_chofer = dv.ToTable();
            }
            catch { }

            try
            {
                dt_camion = ReporteRNegocio.listar_camion(where);
                DataView dv = dt_camion.DefaultView;
                dv.Sort = "patente";
                dt_camion = dv.ToTable();
            }
            catch { }


            dtv = dt_chofer.DefaultView;
            d_chofer.DataSource = dtv;
            d_chofer.DataTextField = "nombre_chofer";
            d_chofer.DataValueField = "cod_chofer";
            d_chofer.DataBind();

            dtv = dt_camion.DefaultView;
            d_bodega.DataSource = dtv;
            d_bodega.DataTextField = "patente";
            d_bodega.DataValueField = "cod_camion";
            d_bodega.DataBind();


        }


        protected void refresh_edit_(object sender, EventArgs e)
        {

            string respt = "";
            try
            {
                cargar_bodega("");

            }
            catch { respt = "Error "; }

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "chosen", "<script language='javascript'>  load_chosen_combos();  </script>", false);

        }

        protected void b_sumar_Click(object sender, ImageClickEventArgs e)
        {
            //string fecha = t_fecha_despacho.Text;
            //string carga_inicial = inputTitle.Text;
            //string cod_trans = l_transpor.Text;
            //string patente = ReporteRNegocio.nombre_camion(" where cod_camion = " + l_camion.Text);
            //string chofer = ReporteRNegocio.nombre_chofer(" where cod_chofer = " + l_chofer.Text);
            //string nombre_trans = ReporteRNegocio.nombre_transporte(" where cod_trans = " + cod_trans);
            //string transporte_completo = "(" + patente + ") " + nombre_trans + " Chofer: " + chofer;
            //string sp = detalle.InnerText.Replace("Detalle Venta Movil: ", "").Trim();
            //string disponible = "";
            //string cod_camion = l_camion.Text;
            //string cod_chofer = l_chofer.Text;
            //string obs = tx_obs_plani.Text;
            //string carga_total = l_suma.Text;
            //string orden_cargar = tx_orden.Text;
            //string vuelta = tx_vuelta.Text;
            //try
            //{
            //    disponible = (Convert.ToDouble(carga_inicial) - Convert.ToDouble(l_suma.Text)).ToString();
            //}
            //catch { }
            //string asignado_por = Session["asignado_por"].ToString();


        }


        protected void G_INFORME_TOTAL_VENDEDOR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Eliminar")
                {

                    string id_select_scope = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    DataTable produ_desch = ReporteRNegocio.lista_det_sp_asignada(id_select_scope);

                    string update_ok = ReporteRNegocio.desplanificar_sp(id_select_scope, 30);
                    //string update_ok = "OK";
                    if (update_ok == "OK")
                    {
                        ////id, coddocumento, cod_trans, estado, nombre_trans, codbodega
                        string CODdocumetno = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                        //string EmailVendedor = ReporteRNegocio.trae_correo_sp(CODdocumetno);
                        //string fechaEmision = ReporteRNegocio.trae_fecha_emision_sp(CODdocumetno);
                        //string cliente_1 = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[6].ToString());

                        //string tabla = "";
                        //tabla += "<table class=\"table fill-head table-bordered\" style=\"width:100%;\">";
                        //tabla += "<thead class=\"test\" style=\"background-color:rgb(156,205,249)\">";
                        //tabla += "<tr>";

                        //tabla += "<th>Código</th>";
                        //tabla += "<th>Nombre</th>";
                        //tabla += "<th>CantDespachado</th>";
                        //tabla += "<th>TipoUnidad</th>";
                        //tabla += "<th>PrecioUnitario</th>";
                        //tabla += "<th>Descuento</th>";
                        //tabla += "<th>PrecioUnitarioFinal</th>";
                        //tabla += "<th>Neto</th>";

                        //tabla += "</tr>";
                        //tabla += "</thead>";
                        //tabla += "<tbody>";
                        //foreach (DataRow dr in produ_desch.Rows)
                        //{
                        //    tabla += "<tr>";
                        //    tabla += "<td>" + dr["codproducto"].ToString() + "</td>";
                        //    tabla += "<td>" + dr["SP_descproducto"].ToString() + "</td>";
                        //    tabla += "<td>" + dr["despachado"].ToString() + "</td>";
                        //    tabla += "<td>" + dr["SP_CodUnVenta"].ToString() + "</td>";
                        //    tabla += "<td>" + dr["SP_preciounitario"].ToString() + "</td>";
                        //    tabla += "<td>" + dr["SP_descuento"].ToString() + "</td>";
                        //    tabla += "<td>" + dr["SP_preciounitariofinal"].ToString() + "</td>";
                        //    tabla += "<td>" + Base.monto_format(dr["montonetofinal"].ToString()) + "</td>";
                        //    tabla += "</tr>";

                        //}
                        //tabla += "</tbody>";
                        //tabla += "</table>";
                        //tabla = tabla.Replace("'", "");

                        //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  document.getElementById('ContentPlaceHolder_Contenido_Button1').click();  </script>", false);

                        //notificar_vendedor(EmailVendedor, id_select_scope, CODdocumetno, fechaEmision, tabla, cliente_1);

                        //enviar_email_cambio(CodDocumento, FechaEmision, CodVendedor, NotaLibre, CodBodega, FechaDespacho, CodMoneda, MontoNeto, DescEstadoDocumento, Facturas, GxEstadoSync, GxActualizado, GxEnviadoERP, NombreVendedor, NombreCliente, DescBodega, FechaCreacion, ValorTipoCambio, LimiteSeguro, TipoCredito, CreditoDisponible, CreditoAutorizado, EmailVendedor);

                        //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  document.getElementById('ContentPlaceHolder_Contenido_Button1').click();  </script>", false);
                        DataTable dt2 = ReporteRNegocio.listar_camiones_asignados("  and coddocumento in (" + agregra_comillas(CODdocumetno) + ")");
                        G_INFORME_TOTAL_VENDEDOR.DataSource = dt2;
                        G_INFORME_TOTAL_VENDEDOR.DataBind();
                        JQ_Datatable();

                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'> alert('Quitado');document.getElementById('BTN_REFRESH_2').click(); </script>", false);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'> alert('Algo ocurrió :(') </script>", false);


                    }

                }
                if (e.CommandName == "Enviar")
                {

                    //string trans_ = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString());
                    //string nom_trans_ = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[4].ToString());
                    //string coddocum = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                    //string bodega = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString());

                    //Session["id_asignada"] = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());

                    ////string correos = ReporteRNegocio.trae_correos_bodega(bodega.Trim());

                    ////tx_para.Text = correos;

                    //carga_camion(" cod_trans = '" + trans_ + "'");
                    ////carga_chofer(" cod_trans = '" + trans_ + "'");

                    //UpdatePanel4.Update();
                    ////string script1 = string.Format("javascript:fuera(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{2}&#39;, &#39;{3}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[2].Text), encriptador.EncryptData(""), encriptador.EncryptData(""), encriptador.EncryptData("57"));
                    ////e.Row.Cells[2].Text = "  <a href='javascript:' onclick='" + script1 + "'>" + e.Row.Cells[2].Text + " </a>";

                    //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  modal_unidad_1('" + coddocum + "').click();  </script>", false);

                    //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  document.getElementById('ContentPlaceHolder_Contenido_Button1').click();  </script>", false);
                    ////modal_unidad_1(" + Name + ");

                    ////string sUrl = "/ListadoProductosPlanificador.aspx?C=" + "12542;";
                    ////string sScript = "<script language =javascript> ";
                    ////sScript += "window.open('" + sUrl + "',null,'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=100%,height=100%,left=100,top=100');";
                    ////sScript += "</script> ";
                    ////Response.Write(sScript);
                    ////Response.Redirect("ListadoProductosPlanificador.aspx?C=10346");

                    ////G_INFORME_TOTAL_VENDEDOR.DataBind();

                }

                if (e.CommandName == "Editar")
                {
                    //string trans_ = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString());
                    //string nom_trans_ = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[4].ToString());
                    //string coddocum = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                    //string bodega = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString());
                    //string fecha = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[7].ToString());
                    //string camion = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[9].ToString());
                    //string chofer = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[10].ToString());
                    //string observacion = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[11].ToString());
                    //string bodega_plani = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[12].ToString());
                    //string carga_inicial = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[13].ToString());
                    //string fecha_type_date = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[16].ToString());
                    //string vuelta = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[17].ToString());

                    //Session["id_asignada"] = (G_INFORME_TOTAL_VENDEDOR.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());

                    //cargar_bodega_3(bodega_plani);
                    //cargar_trans(bodega_plani, trans_);

                    //try
                    //{
                    //    cargar_camion(trans_, camion);
                    //    cargar_chofer(trans_, chofer);
                    //}
                    //catch { }
                    //tx_obs_plani.Text = observacion;
                    //inputTitle.Text = carga_inicial;
                    //t_fecha_despach2.Text = fecha_type_date;

                    //l_bodega.Text = bodega_plani;
                    //l_transpor.Text = trans_;
                    //l_camion.Text = camion;
                    //l_chofer.Text = chofer;

                    //tx_vuelta.Text = vuelta;

                    //Session["bodega_plani"] = bodega_plani;
                    //Session["trans_"] = trans_;
                    //Session["camion"] = camion;
                    //Session["chofer"] = chofer;
                    ////DataTable dt = new DataTable();

                    ////dt = ReporteRNegocio.lista_det_sp_asignada(Session["id_asignada"].ToString());

                    ////G_DETALLE_PLANIFICADO.DataSource = dt;
                    ////G_DETALLE_PLANIFICADO.DataBind();
                    //ScriptManager.RegisterStartupScript(Page, this.GetType(), "chosen", "<script language='javascript'>  load_chosen_combos();  </script>", false);

                    //UpdatePanel4.Update();

                    //Session["coddocumento"] = coddocum;

                    //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  modal_unidad_1('" + coddocum + "');  </script>", false);
                    //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd11Q21mp", "<script language='javascript'>  document.getElementById('ContentPlaceHolder_Contenido_Button1').click();  </script>", false);
                }
            }
            catch (Exception ex)
            {

            }
        }


        public string confirmDelete(string Name)
        {
            return @"javascript:if(!confirm('Esta acción va eliminar la planificación del documento: "
               + Name.ToUpper()
               + @". ¿Estás seguro?')){return false;} ; CARGANDO();";
        }

        protected void G_INFORME_TOTAL_VENDEDOR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // aca
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values[3].ToString().Trim() == "1")
                {
                    e.Row.Attributes["class"] = "estado20";
                }
                else
                {
                    e.Row.Cells[2].Text = "";
                    //e.Row.Cells[0].Text = "";
                    //e.Row.Cells[0].Visible = false;
                    ImageButton ts = new ImageButton();
                    ts = (ImageButton)e.Row.Cells[0].FindControl("btn_quitar");
                    //ts.Attributes["style"] = "visibility:hidden;";
                    e.Row.Attributes["class"] = "estado10";
                }

                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                string script = string.Format("javascript:CargarEvento_Tabla(&#39;{0}&#39;);", G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values[0].ToString().Trim());
                e.Row.Cells[3].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[3].Text + " </a>";

                if (ReporteRNegocio.esvendedor(User.Identity.Name) == "2")
                {

                    e.Row.Cells[0].Visible = false;
                    G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[0].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[2].Visible = false;



                }
                e.Row.Cells[2].Visible = false;
                G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[2].Visible = false;



                Session["fecha_transporte"] = e.Row.Cells[9].Text;
                //h3_transporte.InnerText = e.Row.Cells[6].Text + " --FECHA : " + e.Row.Cells[9].Text;



                if (G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values[8].ToString().Trim() != "0")
                {

                    string script2 = string.Format("javascript:CargarEvento_Tabla2(&#39;{0}&#39;);", G_INFORME_TOTAL_VENDEDOR.DataKeys[e.Row.RowIndex].Values[8].ToString().Trim());
                    e.Row.Cells[9].Text = "  <a href='javascript:' onclick='" + script2 + "'>" + e.Row.Cells[9].Text + " </a>";

                }



                //e.Row.Cells[1].Visible = false;
                //G_INFORME_TOTAL_VENDEDOR.HeaderRow.Cells[1].Visible = false;
            }
        }
    }
}