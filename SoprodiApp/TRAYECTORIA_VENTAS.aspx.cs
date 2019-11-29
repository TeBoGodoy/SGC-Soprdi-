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
using ThinxsysFramework;

namespace SoprodiApp
{
    public partial class TRAYECTORIA_VENTAS : System.Web.UI.Page
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargar_combo_VENDEDOR();
            }
        }

        private void cargar_combo_VENDEDOR()
        {
            DBUtil db = new DBUtil();
            CB_VENDEDORES.DataSource = db.consultar("select cod_usuario, nombre_usuario from [V_CTZ_VENDEDORES] order by nombre_usuario ");
            CB_VENDEDORES.DataValueField = "cod_usuario";
            CB_VENDEDORES.DataTextField = "nombre_usuario";
            CB_VENDEDORES.DataBind();
        }

        [WebMethod]
        public static string ActualizarMeta(string rutcliente, string codvendedor, string valor)
        {
            DBUtil db = new DBUtil();
            DateTime fechahoy = DateTime.Now;
            try
            {
                db.Scalar("insert into capi_meta_vendedor values ('" + codvendedor + "','" + rutcliente + "'," + fechahoy.ToString("MM") + ", " + fechahoy.ToString("yyyy") + ", getdate(), " + valor.Replace(",", "").Replace(".", "") + ");");
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        protected void B_FILTRAR_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable Final = new DataTable();
                Final.Columns.Add("CLIENTE");
                Final.Columns.Add("FACTACUMULADA", typeof(double));
                Final.Columns.Add("PROMFACT", typeof(double));
                Final.Columns.Add("MES1", typeof(double));
                Final.Columns.Add("MES2", typeof(double));
                Final.Columns.Add("MES3", typeof(double));
                Final.Columns.Add("MES4", typeof(double));
                Final.Columns.Add("MES5", typeof(double));
                Final.Columns.Add("MES6", typeof(double));
                Final.Columns.Add("TENDENCIA", typeof(double));
                Final.Columns.Add("MESESSINVENTA");
                Final.Columns.Add("FACTACUMULADAPERANTERIOR");
                Final.Columns.Add("VARANOANTERIOR", typeof(double));
                Final.Columns.Add("LCDISPONIBLE");
                Final.Columns.Add("METAMES", typeof(double));
                Final.Columns.Add("PER_VARIACION", typeof(double));
                Final.Columns.Add("RUTCLIENTE");
                Final.Columns.Add("FECHAMETA");
                DBUtil db = new DBUtil();
                string html = "";
                string cod_vendedor = CB_VENDEDORES.SelectedValue;

                string nombre_vendedor = CB_VENDEDORES.SelectedItem.Text;
                DateTime fechaactual = DateTime.Now;
                DateTime primerdiadelmes = Convert.ToDateTime("01/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("yyyy"));

                int mesanterior1 = int.Parse((DateTime.Now.ToString("MM")));
                int mesanterior2 = int.Parse((DateTime.Now.AddMonths(-1)).ToString("MM"));
                int mesanterior3 = int.Parse((DateTime.Now.AddMonths(-2)).ToString("MM"));
                int mesanterior4 = int.Parse((DateTime.Now.AddMonths(-3)).ToString("MM"));
                int mesanterior5 = int.Parse((DateTime.Now.AddMonths(-4)).ToString("MM"));
                int mesanterior6 = int.Parse((DateTime.Now.AddMonths(-5)).ToString("MM"));

                DateTime inicio_1 = Convert.ToDateTime("01/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("yyyy"));
                DateTime fin_1 = Convert.ToDateTime("" + DateTime.DaysInMonth(DateTime.Now.Year, mesanterior1) + "/" + mesanterior1 + "/" + DateTime.Now.ToString("yyyy"));

                DateTime inicio_2 = Convert.ToDateTime("01/" + DateTime.Now.AddMonths(-1).ToString("MM") + "/" + DateTime.Now.AddMonths(-1).ToString("yyyy"));
                DateTime fin_2 = Convert.ToDateTime("" + DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year, mesanterior2) + "/" + mesanterior2 + "/" + DateTime.Now.AddMonths(-1).ToString("yyyy"));

                DateTime inicio_3 = Convert.ToDateTime("01/" + DateTime.Now.AddMonths(-2).ToString("MM") + "/" + DateTime.Now.AddMonths(-2).ToString("yyyy"));
                DateTime fin_3 = Convert.ToDateTime("" + DateTime.DaysInMonth(DateTime.Now.AddMonths(-2).Year, mesanterior3) + "/" + mesanterior3 + "/" + DateTime.Now.AddMonths(-2).ToString("yyyy"));

                DateTime inicio_4 = Convert.ToDateTime("01/" + DateTime.Now.AddMonths(-3).ToString("MM") + "/" + DateTime.Now.AddMonths(-3).ToString("yyyy"));
                DateTime fin_4 = Convert.ToDateTime("" + DateTime.DaysInMonth(DateTime.Now.AddMonths(-3).Year, mesanterior4) + "/" + mesanterior4 + "/" + DateTime.Now.AddMonths(-3).ToString("yyyy"));

                DateTime inicio_5 = Convert.ToDateTime("01/" + DateTime.Now.AddMonths(-4).ToString("MM") + "/" + DateTime.Now.AddMonths(-4).ToString("yyyy"));
                DateTime fin_5 = Convert.ToDateTime("" + DateTime.DaysInMonth(DateTime.Now.AddMonths(-4).Year, mesanterior5) + "/" + mesanterior5 + "/" + DateTime.Now.AddMonths(-4).ToString("yyyy"));

                DateTime inicio_6 = Convert.ToDateTime("01/" + DateTime.Now.AddMonths(-5).ToString("MM") + "/" + DateTime.Now.AddMonths(-5).ToString("yyyy"));
                DateTime fin_6 = Convert.ToDateTime("" + DateTime.DaysInMonth(DateTime.Now.AddMonths(-5).Year, mesanterior6) + "/" + mesanterior6 + "/" + DateTime.Now.AddMonths(-5).ToString("yyyy"));

                DataTable dt_clientes = new DataTable();
                dt_clientes = db.consultar("select distinct X.rutcliente, X.[Nombre Cliente], X.[Periodo Ultima Factur.], " +
                    " X.[Monto Ultima Factur.], X.[Dias Diferencia], X.LC, isnull(X.disponible,0) as'disponible' " +
                    " , isnull((select top(1) meta.valor from capi_meta_vendedor meta where meta.codvendedor = '" + cod_vendedor + "' and meta.rutcliente = X.rutcliente order by meta.id desc),0) as 'valor' " +
                    " , isnull((select top(1) convert(varchar,meta.fecha_actualizacion,103) from capi_meta_vendedor meta where meta.codvendedor = '" + cod_vendedor + "' and meta.rutcliente = X.rutcliente order by meta.id desc),'n/a') as 'fechameta' " +
                    " from V_CTZ_GESTION_VENTAS_FIN X where codvendedor = '" + cod_vendedor + "'");

                double ss1 = 0;
                double ss2 = 0;
                double ss3 = 0;
                double ss4 = 0;
                double ss5 = 0;
                double ss6 = 0;
                double ss7 = 0;
                double ss8 = 0;
                double ss9 = 0;
                double suma_lc = 0;
                double suma_var_ano_anterior = 0;
                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                int contador_ventas1 = 0;
                int contador_ventas2 = 0;
                int contador_ventas3 = 0;
                int contador_ventas4 = 0;
                int contador_ventas5 = 0;
                int contador_ventas6 = 0;

                contador_ventas1 = db.consultar("select distinct rutcliente from THX_v_reporte " +
                        " where fechafactura >= convert(date,'" + inicio_1.ToString("dd/MM/yyyy") + "',103)" +
                    " and fechafactura <= convert(date,'" + fin_1.ToString("dd/MM/yyyy") + "',103)" +
                    " and codvendedor = '" + cod_vendedor + "'").Rows.Count;

                contador_ventas2 = db.consultar("select distinct rutcliente from THX_v_reporte " +
                        " where fechafactura >= convert(date,'" + inicio_2.ToString("dd/MM/yyyy") + "',103)" +
                    " and fechafactura <= convert(date,'" + fin_2.ToString("dd/MM/yyyy") + "',103)" +
                    " and codvendedor = '" + cod_vendedor + "'").Rows.Count;

                contador_ventas3 = db.consultar("select distinct rutcliente from THX_v_reporte " +
                        " where fechafactura >= convert(date,'" + inicio_3.ToString("dd/MM/yyyy") + "',103)" +
                    " and fechafactura <= convert(date,'" + fin_3.ToString("dd/MM/yyyy") + "',103)" +
                    " and codvendedor = '" + cod_vendedor + "'").Rows.Count;

                contador_ventas4 = db.consultar("select distinct rutcliente from THX_v_reporte " +
                        " where fechafactura >= convert(date,'" + inicio_4.ToString("dd/MM/yyyy") + "',103)" +
                    " and fechafactura <= convert(date,'" + fin_4.ToString("dd/MM/yyyy") + "',103)" +
                    " and codvendedor = '" + cod_vendedor + "'").Rows.Count;

                contador_ventas5 = db.consultar("select distinct rutcliente from THX_v_reporte " +
                        " where fechafactura >= convert(date,'" + inicio_5.ToString("dd/MM/yyyy") + "',103)" +
                    " and fechafactura <= convert(date,'" + fin_5.ToString("dd/MM/yyyy") + "',103)" +
                    " and codvendedor = '" + cod_vendedor + "'").Rows.Count;

                contador_ventas6 = db.consultar("select distinct rutcliente from THX_v_reporte " +
                        " where fechafactura >= convert(date,'" + inicio_6.ToString("dd/MM/yyyy") + "',103)" +
                    " and fechafactura <= convert(date,'" + fin_6.ToString("dd/MM/yyyy") + "',103)" +
                    " and codvendedor = '" + cod_vendedor + "'").Rows.Count;

                foreach (DataRow dr in dt_clientes.Rows)
                {
                    string suma1 = "";
                    string suma2 = "";
                    string suma3 = "";
                    string suma4 = "";
                    string suma5 = "";
                    string suma6 = "";

                    string sql_query_suma = "" +
                       "   select isnull(sum(neto_pesos),0) as 'neto_pesos', isnull(count(1),0) as 'cont' from [THX_v_reporte] " +
                       " where fechafactura >= convert(date,'" + inicio_1.ToString("dd/MM/yyyy") + "',103)" +
                       " and fechafactura <= convert(date,'" + fin_1.ToString("dd/MM/yyyy") + "',103)" +
                       " and rutcliente = '" + dr["rutcliente"].ToString() + "' and codvendedor = '" + cod_vendedor + "'";
                    DataTable dt1 = new DataTable();
                    dt1 = db.consultar(sql_query_suma);
                    if (dt1.Rows.Count > 0)
                    {
                        suma1 = dt1.Rows[0][0].ToString();

                    }



                    string sql_query_suma2 = "" +
                      "   select isnull(sum(neto_pesos),0) as 'neto_pesos' , isnull(count(1),0) as 'cont' from [THX_v_reporte] " +
                      " where fechafactura >= convert(date,'" + inicio_2.ToString("dd/MM/yyyy") + "',103)" +
                      " and fechafactura <= convert(date,'" + fin_2.ToString("dd/MM/yyyy") + "',103)" +
                      " and rutcliente = '" + dr["rutcliente"].ToString() + "' and codvendedor = '" + cod_vendedor + "'";
                    dt1 = db.consultar(sql_query_suma2);
                    if (dt1.Rows.Count > 0)
                    {
                        suma2 = dt1.Rows[0][0].ToString();

                    }

                    string sql_query_suma3 = "" +
                      "   select isnull(sum(neto_pesos),0) as 'neto_pesos' , isnull(count(1),0) as 'cont' from [THX_v_reporte] " +
                      " where fechafactura >= convert(date,'" + inicio_3.ToString("dd/MM/yyyy") + "',103)" +
                      " and fechafactura <= convert(date,'" + fin_3.ToString("dd/MM/yyyy") + "',103)" +
                      " and rutcliente = '" + dr["rutcliente"].ToString() + "' and codvendedor = '" + cod_vendedor + "'";
                    dt1 = db.consultar(sql_query_suma3);
                    if (dt1.Rows.Count > 0)
                    {
                        suma3 = dt1.Rows[0][0].ToString();

                    }

                    string sql_query_suma4 = "" +
                      "   select isnull(sum(neto_pesos),0) as 'neto_pesos', isnull(count(1),0) as 'cont' from [THX_v_reporte] " +
                      " where fechafactura >= convert(date,'" + inicio_4.ToString("dd/MM/yyyy") + "',103)" +
                      " and fechafactura <= convert(date,'" + fin_4.ToString("dd/MM/yyyy") + "',103)" +
                      " and rutcliente = '" + dr["rutcliente"].ToString() + "' and codvendedor = '" + cod_vendedor + "'";
                    dt1 = db.consultar(sql_query_suma4);
                    if (dt1.Rows.Count > 0)
                    {
                        suma4 = dt1.Rows[0][0].ToString();

                    }
                    string sql_query_suma5 = "" +
                      "   select isnull(sum(neto_pesos),0) as 'neto_pesos', isnull(count(1),0) as 'cont' from [THX_v_reporte] " +
                      " where fechafactura >= convert(date,'" + inicio_5.ToString("dd/MM/yyyy") + "',103)" +
                      " and fechafactura <= convert(date,'" + fin_5.ToString("dd/MM/yyyy") + "',103)" +
                      " and rutcliente = '" + dr["rutcliente"].ToString() + "' and codvendedor = '" + cod_vendedor + "'";
                    dt1 = db.consultar(sql_query_suma5);
                    if (dt1.Rows.Count > 0)
                    {
                        suma5 = dt1.Rows[0][0].ToString();

                    }


                    string sql_query_suma6 = "" +
                      "   select isnull(sum(neto_pesos),0) as 'neto_pesos', isnull(count(1),0) as 'cont' from [THX_v_reporte] " +
                      " where fechafactura >= convert(date,'" + inicio_6.ToString("dd/MM/yyyy") + "',103)" +
                      " and fechafactura <= convert(date,'" + fin_6.ToString("dd/MM/yyyy") + "',103)" +
                      " and rutcliente = '" + dr["rutcliente"].ToString() + "' and codvendedor = '" + cod_vendedor + "'";
                    dt1 = db.consultar(sql_query_suma6);
                    if (dt1.Rows.Count > 0)
                    {
                        suma6 = dt1.Rows[0][0].ToString();

                    }

                    // FACTURACION ACTUAL
                    //sql_query_suma = "" +
                    //    "   select isnull(sum(neto_pesos),0) as 'neto_pesos' from [THX_v_reporte] " +
                    //    " where fechafactura >= convert(date,'" + primerdiadelmes.ToString("dd/MM/yyyy") + "',103)" +
                    //    " and fechafactura <= convert(date,'" + DateTime.Now.ToString("dd/MM/yyyy") + "',103)" +
                    //    " and rutcliente = '" + dr["rutcliente"].ToString() + "' and codvendedor = '" + cod_vendedor + "'";
                    //string facturacion_actual = db.Scalar(sql_query_suma).ToString();

                    // FACTURACION ACUMULADA AÑO ANTERIOR
                    sql_query_suma = "" +
                        "   select isnull(sum(neto_pesos),0) as 'neto_pesos' from [THX_v_reporte] " +
                        " where fechafactura >= convert(date,'" + inicio_6.AddYears(-1).ToString("dd/MM/yyyy") + "',103)" +
                        " and fechafactura <= convert(date,'" + fin_1.AddYears(-1).ToString("dd/MM/yyyy") + "',103)" +
                        " and rutcliente = '" + dr["rutcliente"].ToString() + "' and codvendedor = '" + cod_vendedor + "'";
                    string facturacion_ano_anterior = db.Scalar(sql_query_suma).ToString();

                    double C1 = double.Parse(suma1);
                    double C2 = double.Parse(suma2);
                    double C3 = double.Parse(suma3);
                    double C4 = double.Parse(suma4);
                    double C5 = double.Parse(suma5);
                    double C6 = double.Parse(suma6);
                    double FACT_ACUMULADA = C1 + C2 + C3 + C4 + C5 + C6;
                    double FACT_ACUMULADA_ANO_ANTERIOR = double.Parse(facturacion_ano_anterior);
                    double promedio_cuatrimestres = ((C1 + C2 + C3 + C4 + C5 + C6) / 6);

                    // TOTALES
                    ss1 += C1;
                    ss2 += C2;
                    ss3 += C3;
                    ss4 += C4;
                    ss5 += C5;
                    ss6 += C6;
                    ss7 += FACT_ACUMULADA;
                    ss8 += FACT_ACUMULADA_ANO_ANTERIOR;
                    ss9 += promedio_cuatrimestres;
                    // ******************************

                    double VAR_ANO_ANTERIOR = 0;
                    if (FACT_ACUMULADA > 0)
                    {
                        double division = FACT_ACUMULADA_ANO_ANTERIOR / FACT_ACUMULADA;
                        division = division - 1;
                        VAR_ANO_ANTERIOR = (division * 100) * -1;
                    }

                    DataRow dr_final = Final.NewRow();
                    dr_final["CLIENTE"] = dr[1].ToString();
                    dr_final["FACTACUMULADA"] = FACT_ACUMULADA;
                    dr_final["PROMFACT"] = promedio_cuatrimestres;
                    dr_final["MES1"] = C6;
                    dr_final["MES2"] = C5;
                    dr_final["MES3"] = C4;
                    dr_final["MES4"] = C3;
                    dr_final["MES5"] = C2;
                    dr_final["MES6"] = C1;

                    // c1 c6 - c2 c5 - c3 c4 -c4 c3 -c5 c2 - c6 c1
                    try
                    {
                        if ((C4 + C3) == 0 && (C6 + C5) == 0)
                        {
                            dr_final["TENDENCIA"] = 0;
                        }
                        else if ((C4 + C3) == 0 && (C6 + C5) > 0)
                        {
                            dr_final["TENDENCIA"] = ((((C4 + C3) / (C6 + C5)) - 1) * 100);
                        }
                        else if ((C4 + C3) > 0 && (C6 + C5) == 0)
                        {
                            dr_final["TENDENCIA"] = ((((C2 + C1) / (C4 + C3)) - 1) * 100);
                        }
                        else
                        {
                            dr_final["TENDENCIA"] = (((((C2 + C1) / (C4 + C3)) - 1) + (((C4 + C3) / (C6 + C5)) - 1)) * 100);

                        }
                    }
                    catch (Exception ex)
                    {
                        dr_final["TENDENCIA"] = 0;
                    }

                    dr_final["MESESSINVENTA"] = int.Parse(dr[4].ToString());
                    dr_final["FACTACUMULADAPERANTERIOR"] = FACT_ACUMULADA_ANO_ANTERIOR;
                    dr_final["VARANOANTERIOR"] = VAR_ANO_ANTERIOR;
                    dr_final["LCDISPONIBLE"] = int.Parse(dr["disponible"].ToString());
                    dr_final["METAMES"] = int.Parse(dr["valor"].ToString());
                    //
                    try
                    {
                        if (FACT_ACUMULADA == 0)
                        {
                            dr_final["PER_VARIACION"] = 0;
                        }
                        else
                        {
                            double var_meta = (((double.Parse(dr["valor"].ToString()) / FACT_ACUMULADA) - 1) * 100);
                            dr_final["PER_VARIACION"] = var_meta;
                        }
                    }
                    catch (Exception ex)
                    {
                        dr_final["PER_VARIACION"] = 0;
                    }
                    //

                    dr_final["RUTCLIENTE"] = dr["rutcliente"].ToString();
                    dr_final["FECHAMETA"] = dr["fechameta"].ToString();
                    Final.Rows.Add(dr_final);
                }
                html += "<table id='TABLITA_PRODUCTO' border=1 style='width:1500px;' class='table table-sm table-bordered table-stripped condensed'>";
                html += "<thead>";
                html += "   <tr>";
                html += "       <th class='td-sticky3'><b>Cliente</b></th>";
                html += "       <th>Meta Mes</th>";
                html += "       <th>% Var</th>";
                html += "       <th>Promedio facturacion mensual</th>";
                html += "       <th>" + inicio_1.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper() + "</th>";
                html += "       <th>" + inicio_2.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper() + "</th>";
                html += "       <th>" + inicio_3.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper() + "</th>";
                html += "       <th>" + inicio_4.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper() + "</th>";
                html += "       <th>" + inicio_5.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper() + "</th>";
                html += "       <th>" + inicio_6.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper() + "</th>";
                html += "       <th>Fact. Acumulada Periodo</th>";
                html += "       <th>Tendencia</th>";
                html += "       <th># Meses sin Venta</th>";
                html += "       <th>Fact. Acumulada periodo año anterior</th>";
                html += "       <th>% Var r/año anterior</th>";
                html += "       <th>L/C Disponible</th>";

                html += "   </tr>";
                html += "</thead>";
                html += "<tbody>";
                Final.DefaultView.Sort = "FACTACUMULADA DESC";
                Final = Final.DefaultView.ToTable();
                foreach (DataRow dr in Final.Rows)
                {
                    html += "<tr>";
                    html += "   <td class='td-sticky2'><a onclick='fuera22(\"" + encriptador.EncryptData(dr[16].ToString()) + "\",\"" + encriptador.EncryptData("88") + "\");'><b>" + dr[0].ToString() + "</b></a></td>";
                    html += "<td>";
                    html += "<table style='width:100%'>";
                    html += "   <tr>";
                    html += "       <td>$</td>";
                    html += "       <td><input type='number' class='text-right' style='width:100px;' id='T_META_CLT_" + dr[16].ToString() + "' value='" + dr[14].ToString() + "' />";

                    html += "       <br><small>" + dr[17].ToString() + "</small></td>";

                    html += "       <td><a class='btn btn-primary'  onclick='FijarMeta(\"" + dr[16].ToString() + "\", \"" + cod_vendedor + "\");'><i class='fa fa-save'></i></a></td>";
                    html += "   </tr>";
                    html += "</table>";
                    html += "</td>";
                    html += "   <td class='text-right'>" + double.Parse(dr[15].ToString()).ToString("#,##0") + "%</td>";
                    html += "   <td class='text-right'>$" + double.Parse(dr[2].ToString()).ToString("#,##0") + "</td>";
                    html += "   <td class='text-right'>$" + double.Parse(dr[8].ToString()).ToString("#,##0") + "</td>";
                    html += "   <td class='text-right'>$" + double.Parse(dr[7].ToString()).ToString("#,##0") + "</td>";
                    html += "   <td class='text-right'>$" + double.Parse(dr[6].ToString()).ToString("#,##0") + "</td>";
                    html += "   <td class='text-right'>$" + double.Parse(dr[5].ToString()).ToString("#,##0") + "</td>";
                    html += "   <td class='text-right'>$" + double.Parse(dr[4].ToString()).ToString("#,##0") + "</td>";
                    html += "   <td class='text-right'>$" + double.Parse(dr[3].ToString()).ToString("#,##0") + "</td>";
                    html += "   <td class='text-right'><b>$" + double.Parse(dr[1].ToString()).ToString("#,##0") + "</b></td>";
                    if (double.Parse(dr[9].ToString()) >= 0)
                    {
                        html += "   <td class='text-right'>" + double.Parse(dr[9].ToString()).ToString("#,##0") + "% <i class='fa fa-arrow-up' style='color:green;'></i></td>"; // TENDENCIA
                    }
                    else
                    {
                        html += "   <td class='text-right'>" + double.Parse(dr[9].ToString()).ToString("#,##0") + "% <i class='fa fa-arrow-down' style='color:red;'></i></td>"; // TENDENCIA
                    }

                    html += "   <td class='text-right'>" + (double.Parse(dr[10].ToString()) / 30).ToString("#,##0") + "</td>"; // DIAS SIN VENTA
                    html += "   <td class='text-right'>$" + double.Parse(dr[11].ToString()).ToString("#,##0") + "</td>"; // ACUMULADO AÑO ANTERIOR
                    if (double.Parse(dr[12].ToString()) >= 0)
                    {
                        html += "   <td class='text-right'>" + double.Parse(dr[12].ToString()).ToString("#,##0") + "% <i class='fa fa-arrow-up' style='color:green;'></i></td>"; // TENDENCIA
                    }
                    else
                    {
                        html += "   <td class='text-right'>" + double.Parse(dr[12].ToString()).ToString("#,##0") + "% <i class='fa fa-arrow-down' style='color:red;'></i></td>"; // TENDENCIA
                    }
                    html += "   <td class='text-right'>$" + double.Parse(dr[13].ToString()).ToString("#,##0") + "</td>"; // LC DISPONIBLE
                    suma_lc += double.Parse(dr[13].ToString());
                    html += "</tr>";
                }
                html += "<tr>";
                html += "   <td class='td-sticky5'><b>TOTAL:</b></td>";
                html += "   <td class='td-sticky4'></td>"; // META
                html += "   <td class='td-sticky4'></td>"; // VAR META
                html += "   <td class='td-sticky4'><b>$" + ss9.ToString("#,##0") + "</b></td>";
                html += "   <td class='td-sticky4'><b>$" + ss1.ToString("#,##0") + "</b></td>";
                html += "   <td class='td-sticky4'><b>$" + ss2.ToString("#,##0") + "</b></td>";
                html += "   <td class='td-sticky4'><b>$" + ss3.ToString("#,##0") + "</b></td>";
                html += "   <td class='td-sticky4'><b>$" + ss4.ToString("#,##0") + "</b></td>";
                html += "   <td class='td-sticky4'><b>$" + ss5.ToString("#,##0") + "</b></td>";
                html += "   <td class='td-sticky4'><b>$" + ss6.ToString("#,##0") + "</b></td>";
                html += "   <td class='td-sticky4'><b>$" + ss7.ToString("#,##0") + "</b></td>"; // FACT ACUMULADA
                try
                {
                    double var_total = 0;
                    //if ((C3 + C4) == 0 && (C1 + C2) == 0)
                    //{
                    //    dr_final["TENDENCIA"] = 0;
                    //}
                    //else if ((C3 + C4) == 0 && (C1 + C2) > 0)
                    //{
                    //    dr_final["TENDENCIA"] = ((((C3 + C4) / (C1 + C2)) - 1) * 100) * -1;
                    //}
                    //else if ((C3 + C4) > 0 && (C1 + C2) == 0)
                    //{
                    //    dr_final["TENDENCIA"] = ((((C5 + C6) / (C3 + C4)) - 1) * 100) * -1;
                    //}
                    //else
                    //{
                    //    dr_final["TENDENCIA"] = (((((C5 + C6) / (C3 + C4)) - 1) + (((C3 + C4) / (C1 + C2)) - 1)) * 100) * -1;
                    //}
                    if ((ss3 + ss4) == 0 && (ss1 + ss2) > 0)
                    {
                        var_total = ((((ss3 + ss4) / (ss1 + ss2)) - 1) * 100);
                    }
                    else if ((ss3 + ss4) > 0 && (ss1 + ss2) == 0)
                    {
                        var_total = ((((ss5 + ss6) / (ss3 + ss4)) - 1) * 100);
                    }
                    else
                    {
                        var_total = (((((ss5 + ss6) / (ss3 + ss4)) - 1) + (((ss3 + ss4) / (ss1 + ss2)) - 1)) * 100);
                    }
                    if (var_total >= 0)
                    {
                        html += "   <td class='td-sticky4 text-right'>" + var_total.ToString("#,##0") + "% <i class='fa fa-arrow-up' style='color:green;'></i></td>"; // TENDENCIA     
                    }
                    else
                    {
                        html += "   <td class='td-sticky4 text-right'>" + var_total.ToString("#,##0") + "% <i class='fa fa-arrow-down' style='color:red;'></i></td>"; // TENDENCIA     
                    }
                }
                catch (Exception ex)
                {

                }
                html += "   <td class='td-sticky4'></td>"; // meses SIN VENTA              
                html += "   <td class='td-sticky4'><b>$" + ss8.ToString("#,##0") + "</b></td>"; // ACUMULADO AÑO ANTERIOR
                html += "   <td class='td-sticky4'></td>"; // TENDENCIA
                html += "   <td class='td-sticky4'><b>$" + suma_lc.ToString("#,##0") + "</b></td>"; // LC


                html += "</tr>";
                html += "</tbody>";
                html += "</table>";

                string html2 = "";
                html2 += "<table id='TABLITA_TOTALES' border=1 style='width:100%;' class='table table-sm table-bordered table-stripped condensed'>";
                html2 += "<thead>";
                html2 += "   <tr>";
                html2 += "       <th>&nbsp;</th>";
                html2 += "       <th>" + inicio_1.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper() + "</th>";
                html2 += "       <th>" + inicio_2.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper() + "</th>";
                html2 += "       <th>" + inicio_3.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper() + "</th>";
                html2 += "       <th>" + inicio_4.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper() + "</th>";
                html2 += "       <th>" + inicio_5.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper() + "</th>";
                html2 += "       <th>" + inicio_6.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper() + "</th>";
                html2 += "   </tr>";
                html2 += "</thead>";
                html2 += "<tbody>";
                html2 += "   <tr>";
                html2 += "       <td><b>Clientes con Ventas</b></td>";
                html2 += "       <td>" + contador_ventas1 + "</td>";
                html2 += "       <td>" + contador_ventas2 + "</td>";
                html2 += "       <td>" + contador_ventas3 + "</td>";
                html2 += "       <td>" + contador_ventas4 + "</td>";
                html2 += "       <td>" + contador_ventas5 + "</td>";
                html2 += "       <td>" + contador_ventas6 + "</td>";
                html2 += "   </tr>";
                html2 += "</tbody>";
                html2 += "</table>";
                HTML_DIV2.InnerHtml = html2;

                DataTable dt_productos = new DataTable();
                //string sql_query_productos = "  " +
                //    " select X.[producto], X.descproducto, X.neto_pesos from ( " +
                //    " select producto, descproducto, sum(neto_pesos) as 'neto_pesos' from [THX_v_reporte]" +
                //    " where fechafactura >= convert(date,'" + inicio_cuatrimestre4.ToString("dd/MM/yyyy") + "',103)" +
                //     " and fechafactura <= convert(date,'" + fin_cuatrimestre1.ToString("dd/MM/yyyy") + "',103)" +
                //     " and isnumeric(producto) = 1 and producto < 9900 " +
                //     " and codvendedor = '" + cod_vendedor + "' " +
                //    " group by producto, descproducto" +
                //    " ) as X order by X.neto_pesos asc" +
                //    " ";
                //dt_productos = db.consultar(sql_query_productos);
                //// VENDEDOR PRODUCTOS
                //// ****************************************************************************************************
                //// ****************************************************************************************************
                //html += "<b>" + nombre_vendedor + "</b>";
                //html += "<hr>";
                //html += "<table border=1 style='width:100%;'>";
                //html += "   <tr>";
                //html += "       <td><b>Facturación Mes Anterior</b></td>";
                //html += "       <td>$ " + facturacion_mes_anterior.ToString("#,##0") + "</td>";
                //html += "   </tr>";
                //html += "   <tr>";
                //html += "       <td><b>Facturación Promedio (4 meses)</b></td>";
                //html += "       <td>$ " + facturacion_promedio_4_meses.ToString("#,##0") + "</td>";
                //html += "   </tr>";
                //html += "   <tr>";
                //html += "       <td><b>Facturación Acumulado Mes Actual</b></td>";
                //html += "       <td>$ " + facturacion_acumulada.ToString("#,##0") + "</td>";
                //html += "   </tr>";
                //html += "</table>";
                //html += "<hr>";
                //html += "<h4>Variación Negativa de Productos Ultimos 4 Cuatrimestres</h4>";
                //html += "<table border=1 style='width:100%;'>";
                //html += "<thead>";
                //html += "   <tr>";
                //html += "       <th colspan='3'></th>";
                //html += "       <th colspan='5'>Cuatrimestres</th>  ";
                //html += "   </tr>";
                //html += "   <tr>";
                //html += "       <th>Producto</th>";
                //html += "       <th>Stock Actual</th>";
                //html += "       <th>Fact. Acumulada</th>";
                //html += "       <th>" + fin_cuatrimestre4.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")) + "</th>";
                //html += "       <th>" + fin_cuatrimestre3.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")) + "</th>";
                //html += "       <th>" + fin_cuatrimestre2.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")) + "</th>";
                //html += "       <th>" + fin_cuatrimestre1.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")) + "</th>";
                //html += "       <th>Tendencia</th>";
                //html += "   </tr>";
                //html += "</thead>";
                //html += "<tbody>";
                //foreach (DataRow dr in dt_productos.Rows)
                //{
                //    string cuatrimestre1 = "";
                //    string cuatrimestre2 = "";
                //    string cuatrimestre3 = "";
                //    string cuatrimestre4 = "";

                //    // CUATRIMESTRE 1
                //    string sql_query_suma_productos = "" +
                //        "   select isnull(sum(neto_pesos),0) as 'neto_pesos' from [THX_v_reporte] " +
                //        " where fechafactura >= convert(date,'" + inicio_cuatrimestre1.ToString("dd/MM/yyyy") + "',103)" +
                //        " and fechafactura <= convert(date,'" + fin_cuatrimestre1.ToString("dd/MM/yyyy") + "',103)" +
                //        " and producto = '" + dr["producto"].ToString() + "' and codvendedor = '" + cod_vendedor + "'";
                //    cuatrimestre1 = db.Scalar(sql_query_suma_productos).ToString();
                //    // CUATRIMESTRE 2
                //    sql_query_suma_productos = "" +
                //        "   select isnull(sum(neto_pesos),0) as 'neto_pesos' from [THX_v_reporte] " +
                //        " where fechafactura >= convert(date,'" + inicio_cuatrimestre2.ToString("dd/MM/yyyy") + "',103)" +
                //        " and fechafactura <= convert(date,'" + fin_cuatrimestre2.ToString("dd/MM/yyyy") + "',103)" +
                //        " and producto = '" + dr["producto"].ToString() + "' and codvendedor = '" + cod_vendedor + "'";
                //    cuatrimestre2 = db.Scalar(sql_query_suma_productos).ToString();
                //    // CUATRIMESTRE 3
                //    sql_query_suma_productos = "" +
                //        "   select isnull(sum(neto_pesos),0) as 'neto_pesos' from [THX_v_reporte] " +
                //        " where fechafactura >= convert(date,'" + inicio_cuatrimestre3.ToString("dd/MM/yyyy") + "',103)" +
                //        " and fechafactura <= convert(date,'" + fin_cuatrimestre3.ToString("dd/MM/yyyy") + "',103)" +
                //        " and producto = '" + dr["producto"].ToString() + "' and codvendedor = '" + cod_vendedor + "'";
                //    cuatrimestre3 = db.Scalar(sql_query_suma_productos).ToString();
                //    // CUATRIMESTRE 4
                //    sql_query_suma_productos = "" +
                //        "   select isnull(sum(neto_pesos),0) as 'neto_pesos' from [THX_v_reporte] " +
                //        " where fechafactura >= convert(date,'" + inicio_cuatrimestre4.ToString("dd/MM/yyyy") + "',103)" +
                //        " and fechafactura <= convert(date,'" + fin_cuatrimestre4.ToString("dd/MM/yyyy") + "',103)" +
                //        " and producto = '" + dr["producto"].ToString() + "' and codvendedor = '" + cod_vendedor + "'";
                //    cuatrimestre4 = db.Scalar(sql_query_suma_productos).ToString();
                //    // FACTURACION ACTUAL
                //    sql_query_suma_productos = "" +
                //        "   select isnull(sum(neto_pesos),0) as 'neto_pesos' from [THX_v_reporte] " +
                //        " where fechafactura >= convert(date,'" + primerdiadelmes.ToString("dd/MM/yyyy") + "',103)" +
                //        " and fechafactura <= convert(date,'" + DateTime.Now.ToString("dd/MM/yyyy") + "',103)" +
                //        " and producto = '" + dr["producto"].ToString() + "' and codvendedor = '" + cod_vendedor + "'";
                //    string facturacion_actual = db.Scalar(sql_query_suma_productos).ToString();

                //    int C1 = int.Parse(cuatrimestre1);
                //    int C2 = int.Parse(cuatrimestre2);
                //    int C3 = int.Parse(cuatrimestre3);
                //    int C4 = int.Parse(cuatrimestre4);
                //    int FACT_ACTUAL = int.Parse(facturacion_actual);
                //    int promedio_cuatrimestres = ((C1 + C2 + C3 + C4) / 4);

                //    html += "<tr>";
                //    html += "   <td>" + dr["descproducto"].ToString() + "</td>";
                //    html += "   <td>Stock</td>";
                //    html += "   <td>$ " + FACT_ACTUAL.ToString("#,##0") + "</td>";
                //    html += "   <td>$ " + C1.ToString("#,##0") + "</td>";
                //    html += "   <td>$ " + C2.ToString("#,##0") + "</td>";
                //    html += "   <td>$ " + C3.ToString("#,##0") + "</td>";
                //    html += "   <td>$ " + C4.ToString("#,##0") + "</td>";
                //    html += "   <td>$ " + promedio_cuatrimestres.ToString("#,##0") + "</td>";
                //    html += "</tr>";
                //}
                //html += "</tbody>";
                //html += "</table>";
                // VENDEDOR CLIENTES
                // ****************************************************************************************************
                // ****************************************************************************************************


                // FIN
                HTML_DIV.InnerHtml = html;
                ScriptManager.RegisterStartupScript(this, typeof(Page), "asdasdcas", "<script>javascript:Datatables();</script>", false);
            }
            catch (Exception ex)
            {
                HTML_DIV.InnerHtml = ex.Message.ToString();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "cerrarreloj", "<script>javascript:relojito(false);</script>", false);
            }
        }
    }
}


//string sql_query_clientes = "  " +
//    " select X.[rutcliente], X.nombrecliente, X.neto_pesos from ( " +
//    " select rutcliente, nombrecliente, sum(neto_pesos) as 'neto_pesos' from [THX_v_reporte]" +
//    " where fechafactura >= convert(date,'" + inicio_cuatrimestre4.ToString("dd/MM/yyyy") + "',103)" +
//     " and fechafactura <= convert(date,'" + fin_cuatrimestre1.ToString("dd/MM/yyyy") + "',103)" +
//     " and codvendedor = '" + cod_vendedor + "' " +
//    " group by rutcliente, nombrecliente" +
//    " ) as X order by X.neto_pesos asc" +
//    " ";
//dt_clientes = db.consultar(sql_query_clientes);





//// CUATRIMESTRE 1
//string sql_query_suma_productos = "" +
//    "   select isnull(sum(neto_pesos),0) as 'neto_pesos' from [THX_v_reporte] " +
//    " where fechafactura >= convert(date,'" + inicio_1.ToString("dd/MM/yyyy") + "',103)" +
//    " and fechafactura <= convert(date,'" + fin_1.ToString("dd/MM/yyyy") + "',103)" +
//    " and rutcliente = '" + dr["rutcliente"].ToString() + "' and codvendedor = '" + cod_vendedor + "'";
//cuatrimestre1 = db.Scalar(sql_query_suma_productos).ToString();
//// CUATRIMESTRE 2
//sql_query_suma_productos = "" +
//    "   select isnull(sum(neto_pesos),0) as 'neto_pesos' from [THX_v_reporte] " +
//    " where fechafactura >= convert(date,'" + inicio_cuatrimestre2.ToString("dd/MM/yyyy") + "',103)" +
//    " and fechafactura <= convert(date,'" + fin_cuatrimestre2.ToString("dd/MM/yyyy") + "',103)" +
//  " and rutcliente = '" + dr["rutcliente"].ToString() + "' and codvendedor = '" + cod_vendedor + "'";
//cuatrimestre2 = db.Scalar(sql_query_suma_productos).ToString();
//// CUATRIMESTRE 3
//sql_query_suma_productos = "" +
//    "   select isnull(sum(neto_pesos),0) as 'neto_pesos' from [THX_v_reporte] " +
//    " where fechafactura >= convert(date,'" + inicio_cuatrimestre3.ToString("dd/MM/yyyy") + "',103)" +
//    " and fechafactura <= convert(date,'" + fin_cuatrimestre3.ToString("dd/MM/yyyy") + "',103)" +
//     " and rutcliente = '" + dr["rutcliente"].ToString() + "' and codvendedor = '" + cod_vendedor + "'";
//cuatrimestre3 = db.Scalar(sql_query_suma_productos).ToString();
//// CUATRIMESTRE 4
//sql_query_suma_productos = "" +
//    "   select isnull(sum(neto_pesos),0) as 'neto_pesos' from [THX_v_reporte] " +
//    " where fechafactura >= convert(date,'" + inicio_cuatrimestre4.ToString("dd/MM/yyyy") + "',103)" +
//    " and fechafactura <= convert(date,'" + fin_cuatrimestre4.ToString("dd/MM/yyyy") + "',103)" +
//    " and rutcliente = '" + dr["rutcliente"].ToString() + "' and codvendedor = '" + cod_vendedor + "'";
//cuatrimestre4 = db.Scalar(sql_query_suma_productos).ToString();