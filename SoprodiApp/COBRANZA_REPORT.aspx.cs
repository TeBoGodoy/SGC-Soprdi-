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
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace SoprodiApp
{
    public partial class COBRANZA_REPORT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                vencido.Text = "Vencido : " + Math.Round(Convert.ToDouble(ReporteRNegocio.vencidos_total(" and tipo_doc in ('IN') ")), MidpointRounding.AwayFromZero).ToString("N0");


            }
        }


        private static string variable_tabla;

        [WebMethod]
        public static string Reporte_Estimados(string desde, string hasta)
        {
            string tabla = "";
            DBUtil db = new DBUtil();

            //DataTable dt = db.consultar("select id_cobranza, (select top 1 nom_accion from acciones a where Cobranza_Acciones.id_accion = a.id_accion) as accion,  fecha_accion as fecha, usuario, obs from Cobranza_Acciones where ID_cobranza = '" + id + "'");

            tabla += "<table style='width:100%;' class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";
            tabla += "<th> </th>";
            tabla += "<th>PESO</th>";
            tabla += "<th>DOLARES</th>";
            tabla += "</tr>";

            tabla += "<tr>";
            tabla += "<th> </th>";
            tabla += "<th> <table  width='100%'><tr><td style='width: 50%; border: 1px solid rgb(221, 221, 221);'>Estimado</td><td style='width: 50%; border: 1px solid rgb(221, 221, 221);'>Gestionado</td></tr></table> </th>";
            tabla += "<th> <table  width='100%'><tr><td style='width: 50%; border: 1px solid rgb(221, 221, 221);'>Estimado</td><td style='width: 50%; border: 1px solid rgb(221, 221, 221);'>Gestionado</td></tr></table> </th>";
            tabla += "</tr>";

            tabla += "</thead>";
            tabla += "<tbody>";

            int uno = 1;
            int dos = 2;
            double suma_total_peso = 0;
            double suma_total_dolar = 0;

            double suma_total_peso_solom = 0;
            double suma_total_dolar_solom = 0;
            while (uno <= dos)
            {

                string peso_in = ReporteRNegocio.estimado_peso_cobranz(desde).ToString();
                string dolar_in = ReporteRNegocio.estimado_dolar_cobranz(desde).ToString();
                string peso_cheque = ReporteRNegocio.estimado_peso_CHEQUES(desde).ToString();
                string dolar_cheque = ReporteRNegocio.estimado_dolar_CHEQUES(desde).ToString();


                string peso_in_solom = ReporteRNegocio.estimado_peso_cobranz_solom(desde).ToString();
                string dolar_in_solom = ReporteRNegocio.estimado_dolar_cobranz_solom(desde).ToString();


                if (peso_in == "0,000000" || peso_in == "0,00")
                {
                    peso_in = "0";
                }
                if (dolar_in == "0,000000" || dolar_in == "0,00")
                {
                    dolar_in = "0";
                }

                if (peso_cheque == "0,000000" || peso_cheque == "0,00")
                {
                    peso_cheque = "0";
                }

                if (dolar_cheque == "0,000000" || dolar_cheque == "0,00")
                {
                    dolar_cheque = "0";
                }

                if (peso_in_solom == "0,000000" || peso_in_solom == "0,00")
                {
                    peso_in_solom = "0";
                }

                if (dolar_in_solom == "0,000000" || dolar_in_solom == "0,00")
                {
                    dolar_in_solom = "0";
                }





                string suma_peso_solom = monto_format((Convert.ToDouble(peso_in_solom) + Convert.ToDouble(peso_cheque)).ToString());
                string suma_dolar_solom = monto_format((Convert.ToDouble(dolar_in_solom) + Convert.ToDouble(dolar_cheque)).ToString());


                string suma_peso = monto_format((Convert.ToDouble(peso_in) + Convert.ToDouble(peso_cheque)).ToString());
                string suma_dolar = monto_format((Convert.ToDouble(dolar_in) + Convert.ToDouble(dolar_cheque)).ToString());

                DateTime aux1 = Convert.ToDateTime(desde, new CultureInfo("es-ES"));
                string dia_sema = dia_semana_ingles(aux1.DayOfWeek.ToString());
                DateTime fecha = Convert.ToDateTime(desde);

                tabla += "<tr>";
                tabla += "<td>" + dia_sema + " " + desde.Substring(0, 2) + " " + MonthName2(fecha.Month) + "</td>";
                tabla += "<td></td>";
                tabla += "<td></td>";
                tabla += "</tr>";

                tabla += "<tr>";
                tabla += "<td> COBRANZA</td>";
                tabla += "<td> <table style='width:100%;'><tr> <td style='width: 50%; border: 1px solid rgb(221, 221, 221);'>" + monto_format(peso_in_solom).Replace(",000000", "").Replace(",00", "") + "</td>";
                tabla += "                              <td style = 'width: 50%; border: 1px solid rgb(221, 221, 221);'>" + monto_format(peso_in).Replace(",000000", "").Replace(",00", "") + "</td>   </tr></table></td>";

                tabla += "<td> <table style='width:100%;'><tr> <td style='width: 50%; border: 1px solid rgb(221, 221, 221);'>" + monto_format(dolar_in_solom).Replace(",000000", "").Replace(",00", "") + "</td>";
                tabla += "                              <td style = 'width: 50%; border: 1px solid rgb(221, 221, 221);'>" + monto_format(dolar_in).Replace(",000000", "").Replace(",00", "") + "</td>   </tr></table></td>";



                tabla += "</tr>";

                tabla += "<tr>";
                tabla += "<td>CHEQUES POR COBRAR </td>";


                tabla += "<td> <table width='100%'><tr> <td style='width: 50%; border: 1px solid rgb(221, 221, 221);'>" + monto_format(peso_cheque).Replace(",000000", "").Replace(",00", "") + "</td>";
                tabla += "                              <td style = 'width: 50%; border: 1px solid rgb(221, 221, 221);'>" + monto_format(peso_cheque).Replace(",000000", "").Replace(",00", "") + "</td>   </tr></table></td>";

                tabla += "<td> <table width='100%'><tr> <td style='width: 50%; border: 1px solid rgb(221, 221, 221);'>" + monto_format(dolar_cheque).Replace(",000000", "").Replace(",000", "") + "</td>";
                tabla += "                              <td style = 'width: 50%; border: 1px solid rgb(221, 221, 221);'>" + monto_format(dolar_cheque).Replace(",000000", "").Replace(",000", "") + "</td>   </tr></table></td>";


                //tabla += "<td style='text-align: center;'> " + monto_format(dolar_cheque) + "</td>";
                tabla += "</tr>";



                tabla += "<tr style='background-color: rgb(156, 205, 249);'>";
                tabla += "<td>TOTAL </td>";
                tabla += "<td> <table width='100%'><tr> <td style='width: 50%; border: 1px solid rgb(221, 221, 221);'>" + suma_peso_solom + "</td>";
                tabla += "                              <td style = 'width: 50%; border: 1px solid rgb(221, 221, 221);'>" + suma_peso + "</td>   </tr></table></td>";

                tabla += "<td> <table width='100%'><tr> <td style='width: 50%; border: 1px solid rgb(221, 221, 221);'>" + suma_dolar_solom + "</td>";
                tabla += "                              <td style = 'width: 50%; border: 1px solid rgb(221, 221, 221);'>" + suma_dolar + "</td>   </tr></table></td>";

                tabla += "</tr>";
                if (suma_peso == "") { suma_peso = "0"; }
                if (suma_dolar == "") { suma_dolar = "0"; }

                if (suma_peso_solom == "") { suma_peso_solom = "0"; }
                if (suma_dolar_solom == "") { suma_dolar_solom = "0"; }

                suma_total_peso += Convert.ToDouble(suma_peso.Replace(".", ""));
                suma_total_dolar += Convert.ToDouble(suma_dolar.Replace(".", ""));

                suma_total_peso_solom += Convert.ToDouble(suma_peso_solom.Replace(".", ""));
                suma_total_dolar_solom += Convert.ToDouble(suma_dolar_solom.Replace(".", ""));

                tabla += "<tr>";
                tabla += "<td>&nbsp;</td>";
                tabla += "</tr>";
                if (hasta == desde) {  uno = 2; break; }

                DateTime aux = Convert.ToDateTime(desde, new CultureInfo("es-ES"));
                desde = aux.AddDays(+1).ToShortDateString().Replace("-", "/");
                if (uno == dos) { break; }
             
                if (hasta == desde)
                {
                    uno = 2;
                }


            }

            tabla += "<tr>";
            tabla += "<td> &nbsp; </td>";
            //tabla += "<td> " + monto_format(suma_total_peso.ToString()) + "</td>";

            tabla += "<td  style='background-color : rgb(184, 255, 147);'> <table width='100%'><tr> <td style='width: 50%; border: 1px solid rgb(221, 221, 221);'>" + monto_format(suma_total_peso_solom.ToString()) + "</td>";
            tabla += "                              <td style = 'width: 50%; border: 1px solid rgb(221, 221, 221);'>" + monto_format(suma_total_peso.ToString()) + "</td>   </tr></table></td>";

            tabla += "<td  style='background-color : rgb(184, 255, 147);'> <table width='100%'><tr> <td style='width: 50%; border: 1px solid rgb(221, 221, 221);'>" + monto_format(suma_total_dolar_solom.ToString()) + "</td>";
            tabla += "                              <td style = 'width: 50%; border: 1px solid rgb(221, 221, 221);'>" + monto_format(suma_total_dolar.ToString()) + "</td>   </tr></table></td>";


            tabla += "</tr>";

            tabla += "</tbody>";
            tabla += "</table>";


            //tabla = tabla.Replace("'", "");
            variable_tabla = tabla;
            return tabla;

        }

        private static string MonthName2(int month)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            return dtinfo.GetMonthName(month);
        }

        private static string dia_semana_ingles(string p)
        {
            string dia = "";

            if (p.ToUpper() == "MONDAY")
            {
                dia = "LUNES";
            }

            if (p.ToUpper() == "TUESDAY")
            {
                dia = "MARTES";

            }
            if (p.ToUpper() == "WEDNESDAY")
            {
                dia = "MIÉRCOLES";

            }
            if (p.ToUpper() == "THURSDAY")
            {
                dia = "JUEVES";

            }
            if (p.ToUpper() == "FRIDAY")
            {
                dia = "VIERNES";

            }
            if (p.ToUpper() == "SATURDAY")
            {
                dia = "SÁBADO";

            }
            if (p.ToUpper() == "SUNDAY")
            {
                dia = "DOMINGO";

            }
            return dia;
        }

        private static string rut_format(string p)
        {
            string valor = p;
            string rut_ini = valor.Trim().Substring(0, valor.Trim().Length - 1);
            double rut = 0;
            try { rut = double.Parse(rut_ini); return rut.ToString("N0") + "-" + valor.Trim().Substring(valor.Trim().Length - 1); }
            catch { rut = double.Parse(valor); return rut.ToString("N0"); }
        }

        private static string monto_format(string p)
        {
            double d;
            if (p != "0,00" && p != "")
            {
                bool es_dolar = false;
                if (p.Contains(","))
                {
                    if (p.Substring(p.IndexOf(",")) != ",00")
                    {
                        es_dolar = false;

                    }
                    else
                    {
                        if (p != "0,00")
                        {
                            double.TryParse(p.Substring(0, p.IndexOf(",")), out d);
                            string aux = "";
                            if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                            return aux;
                        }

                        es_dolar = true;
                    }
                }
                else
                {

                    double.TryParse(p, out d);
                    string aux = "";
                    if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                    return aux;
                }

                double.TryParse(p.Substring(0, p.IndexOf(",")), out d);
                string aux1 = "";
                if (d == 0) { aux1 = ""; } else { aux1 = d.ToString("N0"); }
                return aux1 + p.Substring(p.IndexOf(","));

            }
            else { return ""; }

        }


        protected void G_DETALLE_CHEQUES_CARTERA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                cont_filas++;
                if (total_filas + 1 == cont_filas)
                {
                    //e.Row.Cells[11].Text = sum_pesos.ToString("N0");
                    e.Row.Cells[11].Text = Base.monto_format(Math.Round(sum_dolar, MidpointRounding.AwayFromZero).ToString());
                    e.Row.Cells[11].BackColor = Color.FromArgb(51, 255, 51);
                    //e.Row.Cells[12].BackColor = Color.FromArgb(51, 255, 51);
                }
                //sum_pesos += Convert.ToDouble(e.Row.Cells[11].Text);
                sum_dolar += Convert.ToDouble(e.Row.Cells[11].Text);
                e.Row.Cells[10].Text = "";
                e.Row.Cells[11].Text = Base.monto_format(e.Row.Cells[11].Text);
                //e.Row.Cells[12].Text = Base.monto_format(e.Row.Cells[12].Text);

            }
        }
        public static double sum_pesos;
        public static double sum_dolar;
        public static int cont_filas = 0;
        public static int total_filas;
        protected void G_DETALLE_ESTIMADOS_FACTURAS_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                cont_filas++;
                if (total_filas + 1 == cont_filas)
                {
                    if (!chk_gestionado.Checked)
                    {
                        btn_quitar_agendado.Visible = false;
                        e.Row.Cells[0].Text = "";
                        e.Row.Cells[7].Text = sum_pesos.ToString("N0");
                        e.Row.Cells[8].Text = Base.monto_format(Math.Round(sum_dolar, 3).ToString());
                        e.Row.Cells[7].BackColor = Color.FromArgb(51, 255, 51);
                        e.Row.Cells[8].BackColor = Color.FromArgb(51, 255, 51);
                        e.Row.Cells[10].Visible = false;
                        e.Row.Cells[11].Visible = false;
                        e.Row.Cells[12].Visible = false;
                        //e.Row.Cells[].Visible = false;
                        G_DETALLE_ESTIMADOS_FACTURAS.HeaderRow.Cells[10].Visible = false;
                        G_DETALLE_ESTIMADOS_FACTURAS.HeaderRow.Cells[11].Visible = false;
                        G_DETALLE_ESTIMADOS_FACTURAS.HeaderRow.Cells[12].Visible = false;
                        //G_DETALLE_ESTIMADOS_FACTURAS.HeaderRow.Cells[13].Visible = false;
                        e.Row.Cells[3].Text = "";
                    }
                    else {


                        e.Row.Cells[0].Text = "";
                        e.Row.Cells[8].Text = sum_pesos.ToString("N0");
                        e.Row.Cells[9].Text = Base.monto_format(Math.Round(sum_dolar, 3).ToString());
                        e.Row.Cells[8].BackColor = Color.FromArgb(51, 255, 51);
                        e.Row.Cells[9].BackColor = Color.FromArgb(51, 255, 51);
                        e.Row.Cells[10].Visible = false;
                        e.Row.Cells[11].Visible = false;
                        e.Row.Cells[12].Visible = false;
                        G_DETALLE_ESTIMADOS_FACTURAS.HeaderRow.Cells[10].Visible = false;
                        G_DETALLE_ESTIMADOS_FACTURAS.HeaderRow.Cells[11].Visible = false;
                        G_DETALLE_ESTIMADOS_FACTURAS.HeaderRow.Cells[12].Visible = false;
                        e.Row.Cells[4].Text = "";
                    }


                }
                else {
                    if (e.Row.Cells[11].Text == "dolar")
                    {

                        e.Row.Cells[9].Text = e.Row.Cells[10].Text;
                        sum_dolar += Convert.ToDouble(e.Row.Cells[9].Text);
                    }
                    else {

                        e.Row.Cells[8].Text = e.Row.Cells[10].Text;
                        sum_pesos += Convert.ToDouble(e.Row.Cells[8].Text);
                    }





                    e.Row.Cells[8].Text = Base.monto_format(e.Row.Cells[8].Text); ;
                    e.Row.Cells[9].Text = Base.monto_format(e.Row.Cells[9].Text);



                    e.Row.Cells[10].Visible = false;
                    e.Row.Cells[11].Visible = false;
                    e.Row.Cells[12].Visible = false;
                    G_DETALLE_ESTIMADOS_FACTURAS.HeaderRow.Cells[10].Visible = false;
                    G_DETALLE_ESTIMADOS_FACTURAS.HeaderRow.Cells[11].Visible = false;
                    G_DETALLE_ESTIMADOS_FACTURAS.HeaderRow.Cells[12].Visible = false;

                    if (!chk_gestionado.Checked)
                    {
                        e.Row.Cells[0].Visible = false;
                        G_DETALLE_ESTIMADOS_FACTURAS.HeaderRow.Cells[0].Visible = false;
                    }

                }

            }
        }

        private void llena_total()
        {
            string tabla = "";
            tabla += "<div style='float:right;'><table class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";
            tabla += "<th> Total Peso</th>";
            tabla += "<th> Total Dolar</th>";

            tabla += "</tr>";
            tabla += "<tr>";
            tabla += "<td style='background-color : rgb(184, 255, 147); color: #000;'> " + sum_pesos.ToString("N0") + " &nbsp;&nbsp;&nbsp;&nbsp;</td>";
            tabla += "<td style='background-color : rgb(184, 255, 147); color: #000;'>" + Base.monto_format(Math.Round(sum_dolar, 3).ToString()) + " &nbsp;&nbsp;&nbsp;&nbsp;</td>";

            tabla += "</tr>";

            tabla += "</thead>";
            tabla += "<tbody>";

            tabla += "</tbody>";
            tabla += "</table></div>";

            div_total.InnerHtml = tabla;
        }

        protected void btn_detalle_2_Click(object sender, EventArgs e)
        {
            if (chk_gestionado.Checked)
            {
                cont_filas = 0;
                sum_dolar = 0;
                sum_pesos = 0;
                DataTable auss = ReporteRNegocio.facturas_gestionadas(CB_DIAS_ELEGIDOS.SelectedValue.ToString());
                int columnas = auss.Columns.Count;
                total_filas = auss.Rows.Count;
                DataRow row;

                if (total_filas > 0)
                {
                    row = auss.NewRow();
                    for (int i = 0; i < columnas; i++)
                    {
                        try
                        {
                            row[i] = "";
                        }
                        catch
                        {
                            row[i] = 0;
                        }
                    }
                    auss.Rows.Add(row);
                    G_DETALLE_ESTIMADOS_FACTURAS.DataSource = auss;

                    G_DETALLE_ESTIMADOS_FACTURAS.DataBind();
                }
                else { G_DETALLE_ESTIMADOS_FACTURAS.DataSource = auss; G_DETALLE_ESTIMADOS_FACTURAS.DataBind(); }

                cont_filas = 0;
                sum_dolar = 0;
                sum_pesos = 0;
                DataTable auss2 = ReporteRNegocio.cheques_por_cobrar(CB_DIAS_ELEGIDOS.SelectedValue.ToString());
                int columnas2 = auss2.Columns.Count;
                total_filas = auss2.Rows.Count;
                DataRow row2;
                if (total_filas > 0)
                {
                    row2 = auss2.NewRow();
                    for (int i = 0; i < columnas2; i++)
                    {
                        try
                        {
                            row2[i] = "";
                        }
                        catch
                        {
                            row2[i] = 0;
                        }
                    }
                    auss2.Rows.Add(row2);
                    G_DETALLE_CHEQUES_CARTERA.DataSource = auss2;
                    G_DETALLE_CHEQUES_CARTERA.DataBind();
                }
                else { G_DETALLE_CHEQUES_CARTERA.DataSource = auss2; G_DETALLE_CHEQUES_CARTERA.DataBind(); }


                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_AN2", "<script language='javascript'>$('#ContentPlaceHolder_Contenido_tabla_html2').html('');</script>", false);

                div_facturas.Visible = true;
                div_cheques.Visible = true;
            }
            else
            {
                cont_filas = 0;
                sum_dolar = 0;
                sum_pesos = 0;
                DataTable auss1 = ReporteRNegocio.facturas_solomon(CB_DIAS_ELEGIDOS.SelectedValue.ToString());
                int columnas1 = auss1.Columns.Count;
                total_filas = auss1.Rows.Count;
                DataRow row1;
                if (total_filas > 0)
                {
                    row1 = auss1.NewRow();
                    for (int i = 0; i < columnas1; i++)
                    {
                        try
                        {
                            row1[i] = "";
                        }
                        catch
                        {
                            row1[i] = 0;
                        }
                    }
                    auss1.Rows.Add(row1);
                    G_DETALLE_ESTIMADOS_FACTURAS.DataSource = auss1;

                    G_DETALLE_ESTIMADOS_FACTURAS.DataBind();
                }
                else { G_DETALLE_ESTIMADOS_FACTURAS.DataSource = auss1; G_DETALLE_ESTIMADOS_FACTURAS.DataBind(); }



                cont_filas = 0;
                sum_dolar = 0;
                sum_pesos = 0;
                DataTable auss2 = ReporteRNegocio.cheques_por_cobrar(CB_DIAS_ELEGIDOS.SelectedValue.ToString());
                int columnas2 = auss2.Columns.Count;
                total_filas = auss2.Rows.Count;
                DataRow row2;
                if (total_filas > 0)
                {
                    row2 = auss2.NewRow();
                    for (int i = 0; i < columnas2; i++)
                    {
                        try
                        {
                            row2[i] = "";
                        }
                        catch
                        {
                            row2[i] = 0;
                        }
                    }
                    auss2.Rows.Add(row2);
                    G_DETALLE_CHEQUES_CARTERA.DataSource = auss2;
                    G_DETALLE_CHEQUES_CARTERA.DataBind();
                }
                else { G_DETALLE_CHEQUES_CARTERA.DataSource = auss2; G_DETALLE_CHEQUES_CARTERA.DataBind(); }


                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_AN2", "<script language='javascript'>$('#tabla_html2').html('');</script>", false);

                div_facturas.Visible = true;
                div_cheques.Visible = true;


            }
        }

        protected void btn_detalle_Click(object sender, EventArgs e)
        {
            if (txt_desde.Text != "" && txt_hasta.Text != "")
            {
                div_detalle.Visible = true;

                DataTable semana_elegida = obtener_dias_desde_hasta();
                llenarcombo_dias(semana_elegida);

                btn_detalle_2_Click(sender, e);
            }
            else {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_AN2w", "<script language='javascript'>alert('Seleccione fechas');</script>", false);
            }

        }

        private void llenarcombo_dias(DataTable dt)
        {
            //dt.Rows.Add(new Object[] { "-1", "-- Seleccione --" });

            CB_DIAS_ELEGIDOS.DataSource = dt;
            CB_DIAS_ELEGIDOS.DataValueField = "Fecha";
            CB_DIAS_ELEGIDOS.DataTextField = "Dia";
            CB_DIAS_ELEGIDOS.DataBind();
            CB_DIAS_ELEGIDOS.SelectedValue = txt_desde.Text;
        }

        private DataTable obtener_dias_desde_hasta()
        {
            string desde = txt_desde.Text;
            string hasta = txt_hasta.Text;
            DataTable au_dias = new DataTable();
            au_dias.Columns.Add("Fecha");
            au_dias.Columns.Add("Dia");
            int uno = 1;
            int dos = 2;
            while (uno <= dos)
            {
                if (hasta == desde)
                {
                    uno = 2;
                }

                DateTime aux1 = Convert.ToDateTime(desde, new CultureInfo("es-ES"));
                string dia_sema = dia_semana_ingles(aux1.DayOfWeek.ToString());


                au_dias.Rows.Add(new Object[] { desde, dia_sema + " " + desde.Substring(0, 2) });

                DateTime aux = Convert.ToDateTime(desde, new CultureInfo("es-ES"));
                desde = aux.AddDays(+1).ToShortDateString().Replace("-", "/");
                if (uno == dos) { break; }
                if (hasta == desde)
                {
                    uno = 2;
                }
            }
            return au_dias;

        }

        protected void excel_total_Click(object sender, EventArgs e)
        {
            tabla_html33.Text = variable_tabla;
            tabla_export.InnerHtml = variable_tabla;


            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment;filename=GeneralCobranza" + DateTime.Today + ".xls");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            //using (StringWriter sw = new StringWriter())
            //{
            //    HtmlTextWriter hw = new HtmlTextWriter(sw);
            //    HtmlForm frm = new HtmlForm();
            //    // AGREGAR A LA EXPORTACION 
            //    //tabla_html33.Parent.Controls.Add(frm);
            //    tabla_export.Parent.Controls.Add(frm);
            //    //tabla_html33.AllowPaging = false;
            //    tabla_html33.EnableViewState = false;
            //    frm.Attributes["runat"] = "server";
            //    frm.Controls.Add(tabla_html33);
            //    frm.RenderControl(hw);


            //    string style = @"<style> .textmode { } </style>";
            //    Response.Write(style);
            //    Response.Output.Write(sw.ToString());
            //    Response.Flush();
            //    Response.End();}
            //Response.Clear();

            //Response.AddHeader("content-disposition", "attachment;filename=SOPRODI_1_" + DateTime.Now.ToShortDateString() + ".xls");

            //Response.Charset = "";

            //// If you want the option to open the Excel file without saving than

            //// comment out the line below

            //Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //Response.ContentType = "application/vnd.xls";

            //System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            //System.Web.UI.HtmlTextWriter htmlWrite =
            //new HtmlTextWriter(stringWrite);

            //variable_tabla.RenderControl(htmlWrite);

            //Response.Write(stringWrite.ToString());

            //Response.End();
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=FileName.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            tabla_export.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();


        }
        protected void btn_excel_cheques_Click(object sender, EventArgs e)
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

            G_DETALLE_CHEQUES_CARTERA.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());

            Response.End();
        }

        protected void btn_quitar_agendado_Click(object sender, EventArgs e)
        {
            string delete = "";
            foreach (GridViewRow dtgItem in this.G_DETALLE_ESTIMADOS_FACTURAS.Rows)
            {
                CheckBox Sel = ((CheckBox)G_DETALLE_ESTIMADOS_FACTURAS.Rows[dtgItem.RowIndex].FindControl("chkAccept"));
                bool valor = Sel.Checked;
                if (valor)
                {

                    string num_factura = G_DETALLE_ESTIMADOS_FACTURAS.Rows[dtgItem.RowIndex].Cells[4].Text;
                    delete += ReporteRNegocio.delete_fecha_cobro(num_factura);

                }
                else
                {
                }
            }

            if (!delete.Contains("ERROR"))
            {
                cont_filas = 0;
                sum_dolar = 0;
                sum_pesos = 0;
                DataTable auss = ReporteRNegocio.facturas_gestionadas(CB_DIAS_ELEGIDOS.SelectedValue.ToString());
                G_DETALLE_ESTIMADOS_FACTURAS.DataSource = auss;

                total_filas = auss.Rows.Count;
                G_DETALLE_ESTIMADOS_FACTURAS.DataBind();
            }
            else {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_AN2ww", "<script language='javascript'>alert('Error al borrar');</script>", false);
            }



        }

        protected void btn_excel_facturas_Click(object sender, EventArgs e)
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

            G_DETALLE_ESTIMADOS_FACTURAS.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());

            Response.End();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            string qq = variable_tabla;

            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(tx_destinos.Text));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "Cobranza Reporte ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

            //string correo_vendedor_por_cliente = ReporteRNegocio.trae_corr_vend_por_cliente(rutcliente.Replace("-", "").Replace(".", ""));
            //if (correo_vendedor_por_cliente != "")
            //{
            //    email.CC.Add(correo_vendedor_por_cliente);
            //}

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
            email.Body += "<div> <b> " + vencido.Text + "</b> <br><br>";
            email.Body += "<div>" + variable_tabla + "</div> <br><br>";


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
                //string MAIL_USER = "";
                //DataTable cr1 = ReporteRNegocio.corr_usuario(User.Identity.Name);
                //foreach (DataRow r in cr.Rows)
                //{
                //    MAIL_USER = r[1].ToString();
                //}

                smtp.Send(email);
                email.Dispose();
                mensaje.Text = "Correo Enviado";
                ////lb_mensaj.Text = "Correos Enviados!";
                //string user = User.Identity.Name;
                //string mail_usuario = MAIL_USER;
                //string fecha_now = DateTime.Now.ToString();
                //string destinos = tx_destinos.Value;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_qqqAqN2", "<script>alert('CORREO ENVIADO!');</script>", false);

                //ReporteRNegocio.insert_log_enviar_ficha(user, mail_usuario, fecha_now, destinos, rutcliente.Replace("-", "").Replace(".", ""));
                ///ACA INSERT LOG
            }
            catch (Exception ex)
            {
                //lb_mensaj.Text = "Error al enviar ";
            }


        }
    }

}