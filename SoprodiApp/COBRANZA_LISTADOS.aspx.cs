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

namespace SoprodiApp
{
    public partial class COBRANZA_LISTADOS : System.Web.UI.Page
    {

        private static string where;
        private static string hoy;


        const int minTotalDiff = 200;    // parameter used in new color acceptance criteria
        const int okSingleDiff = 100;    // id.

        private static int prevR, prevG, prevB;  // R, G and B components of the previously issued color.
        Random RandGen = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //container.Attributes["style"] = "min-width: 310px; height: 400px; max-width: 600px; margin: 0 auto; display:block;";
                //ScriptManager.RegisterStartupScript(Page, this.GetType(), "xwte", "<script>llena_hich() </script>", false);
                DateTime t = DateTime.Now;
                DateTime t2 = DateTime.Now;
                //t = new DateTime(t.Year, t.Month - 6, 1);
                txt_desde.Text = ReporteRNegocio.CALCULA_DESDE(t.Month, t.Year);
                txt_hasta.Text = t2.ToShortDateString();
            }
        }

        public void CargarGrilla()
        {

            DBUtil db = new DBUtil();
            string query = "";

            query += " select VC.FACTURA, CP.tipo_doc as Tipo, Monto, Cuenta, Moneda, Descripcion, CB.NOM_BANCO AS Banco  from cobranza_pagos CP INNER JOIN COBRANZA_BANCOS CB ON CB.ID = CP.BANCO INNER JOIN V_COBRANZA VC ON CP.ID_COBRANZA = VC.ID   ";

            G_Listado.DataSource = db.consultar(query);
            G_Listado.DataBind();

        }

        protected void FILTRAR_Click(object sender, EventArgs e)
        {

            DBUtil db = new DBUtil();
            string query = "";
            DateTime t = DateTime.Now;
            t = new DateTime(t.Year, t.Month, t.Day);
            hoy = t.ToShortDateString();


            if (CB_Opcion.SelectedValue == "1")
            {
                where = "";

                //query = "SELECT cast(ROUND(SUM(monto_doc),0,1) as float) FROM v_cobranza where fecha_venc >= convert(datetime, '" + hoy + "', 103) and factura not in (SELECT factura FROM v_cobranza where fecha_venc >= convert(datetime, '" + hoy + "', 103) and tipo_doc = 'PA') ";
                if (IN_DM.SelectedValue == "1")
                {
                    //query += " and tipo_doc in ('IN') ";
                    where += " and tipo_doc in ('IN') ";
                }
                else if (IN_DM.SelectedValue == "2")
                {
                    //query += " and tipo_doc in ('DM') ";
                    where += " and tipo_doc in ('DM') and isnumeric(num_factura) <> 1 ";
                }
                else if (IN_DM.SelectedValue == "3")
                {
                    //query += " and tipo_doc in ('DM', 'IN') ";
                    where += " and tipo_doc in ('DM', 'IN') ";
                }
                else if (IN_DM.SelectedValue == "4")
                {
                    //query += " and tipo_doc in ('DM') ";
                    where += " and tipo_doc in ('DM') and isnumeric(num_factura) = 1 ";
                }

                if (D_CLIENTES.SelectedValue != "-1")
                {
                    //query += " and rutcliente like '%"+D_CLIENTES.SelectedValue+"%'";
                    where += " and rutcliente like '%" + D_CLIENTES.SelectedValue + "%'";
                }

                G_Listado.DataSource = ReporteRNegocio.privot_semanas(hoy, where);
                G_Listado.DataBind();
                muestra_total.Visible = true;
                G_Listado.Visible = true;
                Label1.Visible = true;
                Label2.Visible = true;
                total_estimado.Visible = true;
                total_vencido.Visible = true;

            }

            if (CB_Opcion.SelectedValue == "2")
            {
                query = "SELECT  SUM(saldo)  FROM v_cobranza where fecha_venc >= convert(datetime, '" + txt_desde.Text + "', 103) and fecha_venc <= convert(datetime, '" + txt_hasta.Text + "', 103) and tipo_doc in ('PA') ";
                if (D_CLIENTES2.SelectedValue != "-1")
                {
                    query += " and rutcliente like '%" + D_CLIENTES2.SelectedValue + "%'";
                }

                //G_Listado.DataSource = db.consultar(query);
                //G_Listado.DataBind();
                foreach (DataRow r in db.consultar(query).Rows)
                {

                    total_.Text = r[0].ToString();

                    double d;
                    double.TryParse(total_.Text, out d);
                    string aux = "";
                    if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                    total_.Text = aux.Replace("-", "");

                    muestra_total.Visible = true;
                    G_Listado.Visible = false;
                    Label1.Visible = false;
                    Label2.Visible = false;
                    total_estimado.Visible = false;
                    total_vencido.Visible = false;

                }

            }

            if (CB_Opcion.SelectedValue == "3")
            {
                //query = "select VC.FACTURA, CP.tipo_doc as Tipo, Monto, Cuenta, Moneda, Descripcion, CB.NOM_BANCO AS Banco  from cobranza_pagos CP INNER JOIN COBRANZA_BANCOS CB ON CB.ID = CP.BANCO INNER JOIN V_COBRANZA VC ON CP.ID_COBRANZA = VC.ID ";

                //G_Listado.DataSource = db.consultar(query);
                //G_Listado.DataBind();
            }


            //CargarGrilla();
        }

        protected void CB_Opcion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_Opcion.SelectedValue == "1")
            {
                doc_abiertos.Visible = true; FECHAS.Visible = false;
            }
            else if (CB_Opcion.SelectedValue == "2")
            {
                doc_abiertos.Visible = false;
                FECHAS.Visible = true;

                DBUtil db = new DBUtil();
                DataTable dt = new DataTable();
                dt = db.consultar("SELECT DISTINCT RUTCLIENTE, NOMBRECLIENTE FROM V_COBRANZA WHERE TIPO_DOC = 'PA' AND fecha_venc >= convert(datetime, '" + txt_desde.Text + "', 103) and fecha_venc <= convert(datetime, '" + txt_hasta.Text + "', 103)");
                dt.Rows.Add(new Object[] { -1, "-- Seleccione --" });
                DataView dv = dt.DefaultView;
                dv.Sort = "NOMBRECLIENTE asc";
                DataTable sortedDT = dv.ToTable();
                D_CLIENTES2.DataSource = sortedDT;
                D_CLIENTES2.DataValueField = "RUTCLIENTE";
                D_CLIENTES2.DataTextField = "NOMBRECLIENTE";
                D_CLIENTES2.DataBind();
                D_CLIENTES2.SelectedValue = "-1";

                clientes.Visible = true;
            }
            else
            {
                FECHAS.Visible = false;
                doc_abiertos.Visible = false;
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime t = DateTime.Now;
            t = new DateTime(t.Year, t.Month, t.Day - 1);
            string hoy = t.ToShortDateString();




            if (IN_DM.SelectedValue == "1")
            {
                DBUtil db = new DBUtil();
                DataTable dt = new DataTable();
                dt = db.consultar("SELECT DISTINCT RUTCLIENTE, NOMBRECLIENTE FROM V_COBRANZA WHERE TIPO_DOC = 'IN' AND fecha_venc >= convert(datetime, '" + hoy + "', 103)");
                dt.Rows.Add(new Object[] { -1, "-- Seleccione --" });
                DataView dv = dt.DefaultView;
                dv.Sort = "NOMBRECLIENTE asc";
                DataTable sortedDT = dv.ToTable();
                D_CLIENTES.DataSource = sortedDT;
                D_CLIENTES.DataValueField = "RUTCLIENTE";
                D_CLIENTES.DataTextField = "NOMBRECLIENTE";
                D_CLIENTES.DataBind();
                D_CLIENTES.SelectedValue = "-1";

                clientes.Visible = true;
            }
            else if (IN_DM.SelectedValue == "2")
            {
                DBUtil db = new DBUtil();
                DataTable dt = new DataTable();
                dt = db.consultar("SELECT DISTINCT RUTCLIENTE, NOMBRECLIENTE FROM V_COBRANZA WHERE TIPO_DOC = 'DM' AND fecha_venc >= convert(datetime, '" + hoy + "', 103)");
                dt.Rows.Add(new Object[] { -1, "-- Seleccione --" });
                DataView dv = dt.DefaultView;
                dv.Sort = "NOMBRECLIENTE asc";
                DataTable sortedDT = dv.ToTable();
                D_CLIENTES.DataSource = sortedDT;
                D_CLIENTES.DataValueField = "RUTCLIENTE";
                D_CLIENTES.DataTextField = "NOMBRECLIENTE";
                D_CLIENTES.DataBind();
                D_CLIENTES.SelectedValue = "-1";

                clientes.Visible = true;
            }
            else if (IN_DM.SelectedValue == "3")
            {
                DBUtil db = new DBUtil();
                DataTable dt = new DataTable();
                dt = db.consultar("SELECT DISTINCT RUTCLIENTE, NOMBRECLIENTE FROM V_COBRANZA WHERE TIPO_DOC in ('IN','DM') AND fecha_venc >= convert(datetime, '" + hoy + "', 103)");
                dt.Rows.Add(new Object[] { -1, "-- Seleccione --" });
                DataView dv = dt.DefaultView;
                dv.Sort = "NOMBRECLIENTE asc";
                DataTable sortedDT = dv.ToTable();
                D_CLIENTES.DataSource = sortedDT;
                D_CLIENTES.DataValueField = "RUTCLIENTE";
                D_CLIENTES.DataTextField = "NOMBRECLIENTE";
                D_CLIENTES.DataBind();
                D_CLIENTES.SelectedValue = "-1";

                clientes.Visible = true;
            }
            else if (IN_DM.SelectedValue == "-1")
            {
                clientes.Visible = false;
            }
        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void G_Listado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = ReporteRNegocio.semana_0(hoy, where);
                double d1;
                double.TryParse(e.Row.Cells[0].Text, out d1);
                string aux1 = "";
                if (d1 == 0) { aux1 = "0"; } else { aux1 = d1.ToString("N0"); }
                e.Row.Cells[0].Text = aux1;
                double sum = 0;
                for (int q = 0; q < e.Row.Cells.Count - 1; q++)
                {
                    double d;
                    double.TryParse(e.Row.Cells[q + 1].Text, out d);
                    string aux = "";
                    if (d == 0) { aux = "0"; } else { aux = d.ToString("N0"); sum += d; }
                    e.Row.Cells[q + 1].Text = aux;
                }
                total_.Text = (sum + Convert.ToDouble(e.Row.Cells[0].Text)).ToString("N0");
                total_estimado.Text = sum.ToString("N0");
                total_vencido.Text = Convert.ToDouble(e.Row.Cells[0].Text).ToString("N0");
            }
        }

        [WebMethod]
        public static string llena_totales()
        {
            string vencidos = ReporteRNegocio.vencidos_total(" and tipo_doc in ('DM', 'IN') ");
            string estimado = ReporteRNegocio.estimados_total(" and tipo_doc in ('DM', 'IN') ");



            string series = " [{ name: '$ ', colorByPoint: true, data: [{  name: 'Vencidos',   y: " + vencidos + ", }, { name: 'Estimado', y: " + estimado + ", " +
                        " }] " +
                        " }] ";


            return series;
        }
        //[WebMethod]
        //public static string llena_factu()
        //{
        //    string vencidos = ReporteRNegocio.vencidos_total(" and tipo_doc in ('IN') ");
        //    string estimado = ReporteRNegocio.estimados_total(" and tipo_doc in ('IN') ");



        //    string series = " [{ name: '$ ', colorByPoint: true, data: [{  name: 'Vencidos',   y: " + vencidos + ", color : '#F7DC6F'}, { name: 'Estimado', y: " + estimado + ", color : '#27AE60'" +
        //                " }] " +
        //                " }] ";


        //    return series;
        //}

        [WebMethod]
        public static string llena_cheques()
        {
            string vencidos = ReporteRNegocio.vencidos_total(" and tipo_doc in ('IN', 'DM') ");
            string estimado = ReporteRNegocio.estimados_total(" and tipo_doc in ('IN','DM') ");

            DataTable rangos_vencidos = ReporteRNegocio.trae_rangos();
            DataTable rangos_factu_che = ReporteRNegocio.trae_fact_cheque_dm();
            DataTable rangos_abarrotes_y_granos = ReporteRNegocio.trae_abarr_y_granos();
            DataTable rangos_clase_cliente = ReporteRNegocio.trae_clase_clientes(); 

            //TOTALES RANGOS FECHAS
            string series = " [{ name: '$ ', colorByPoint: true, data: "

               + " [{ " +

               string_data_rangos(rangos_vencidos)

                   
               + " }] "
                           + " }] ";


            //double round = Math.Round(Convert.ToDouble(r[0].ToString()), MidpointRounding.AwayFromZero);

            series += "& [{ name: '$ ', colorByPoint: true, data: [{  name: 'Vencidos',   y: " + Math.Round(Convert.ToDouble(vencidos), MidpointRounding.AwayFromZero).ToString() + ", }, { name: 'NoVencido', y: " + Math.Round(Convert.ToDouble(estimado), MidpointRounding.AwayFromZero).ToString() + ", " +
                        " }] " +
                        " }] ";


            //FACTURAS Y CHEQUES
            series += "& [{ name: '$ ', colorByPoint: true, data: "

               + " [{ " +

               string_data_rangos(rangos_factu_che)

               + " }] "
                           + " }] ";

            //ABARROTES Y GRANOS
            series += "& [{ name: '$ ', colorByPoint: true, data: "

              + " [{ " +

              string_data_rangos(rangos_abarrotes_y_granos)

              + " }] "
                          + " }] ";

            //CLASES DE CLIENTES (SALMONERA, ABARROTES, HUEVOS... ETC)
            series += "& [{ name: '$ ', colorByPoint: true, data: "

             + " [{ " +

             string_data_rangos_sin_color(rangos_clase_cliente)

             + " }] "
                         + " }] ";
            return series;
        }
        private static string string_data_rangos_sin_color(DataTable dt)
        {
            string json = "";

            foreach (DataRow r in dt.Rows)
            {

                json += " name: '" + r[0].ToString() + "' , y: " + r[1].ToString() + "}, {";

            }

            json = json.Substring(0, json.Length - 4);
            return json;
        }


        private static string string_data_rangos(DataTable dt)
        {
            string json = "";

            foreach (DataRow r in dt.Rows)
            {
                double round = Math.Round( Convert.ToDouble(r[0].ToString()), MidpointRounding.AwayFromZero);
                json += " name: '" + r[1].ToString().Split('*')[0].ToString() + "' , y: " +round.ToString() + " , color : '" + r[1].ToString().Split('*')[1].ToString() + "' }, {";

            }

            json = json.Substring(0, json.Length - 4);
            return json;
        }



    }
}