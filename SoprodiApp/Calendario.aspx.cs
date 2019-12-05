using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.Services;
using ThinxsysFramework;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using SoprodiApp.negocio;
using System.Net.Mail;
using System.Net;
using System.Globalization;
using System.Data.SqlClient;
using SoprodiApp.Entities;
using SoprodiApp.BusinessLayer;

namespace SoprodiApp
{
    public partial class Calendario : System.Web.UI.Page
    {
        public static int COLUMNA_DE_FACTURA;
        public static bool busca_columna_fac;
        public static bool columna_fac;

        public static double sum_peso_saldo;
        public static int TOTAL_ROWS;
        public static int cont_rows;
        public static double sum_dolar_saldo;
        public static double sum_peso_monto;
        public static double sum_dolar_monto;
        public static double cont_pagables;

        public static DataTable aux_dt_excel;
        public class Evento_objeto
        {
            public string id { get; set; }
            public string title { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public string backgroundColor { get; set; }
            public string h_inicio { get; set; }
            public string h_termino { get; set; }
            public string factura { get; set; }
            public string rut_cliente { get; set; }
            public string tabla_html { get; set; }
            public string estado { get; set; }
            public string ver_cliente { get; set; }
            public string monto_doc { get; set; }
        }

        public class combo
        {
            public string value { get; set; }
            public string display { get; set; }
        }

        public class c_resp
        {
            public string respuesta { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {


                LlenarComboCliente();
                LlenarComboVendedor();
                LlenarComboBancos();
                LlenarComboBancos_SOPRODI();
                //string ayer = (DateTime.Now).AddDays(-1).ToShortDateString();
                //string hoy = (DateTime.Now).ToShortDateString();

                //txt_desde2.Text = ayer;
                //txt_hasta2.Text = hoy;

                //ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                //scriptManager.RegisterPostBackControl(this.btn_excel2);
                //scriptManager.RegisterPostBackControl(this.btn_excel_3);


                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teee1132131eeee", "<script>combos_refresh(); $('#btn_tabla').hide();</script>", false);

                //LlenarGrilla();
            }
            else
            {
                JQ_Datatable();
            }
        }

        private void LlenarComboVendedor()
        {
            DBUtil db = new DBUtil();
            DataTable dt = new DataTable();

            string where = "  where 1=1 ";

            if (rd_ven.Checked)
            {
                if (txt_desde2.Text != "")
                {

                    where += " and fecha_venc >= convert(datetime, '" + txt_desde2.Text + "', 103) ";
                }
                if (txt_hasta2.Text != "")
                {
                    where += " and fecha_venc <= convert(datetime, '" + txt_hasta2.Text + "', 103) ";

                }
            }
            else
            {
                if (txt_desde2.Text != "")
                {

                    where += " and fecha_trans >= convert(datetime, '" + txt_desde2.Text + "', 103) ";
                }
                if (txt_hasta2.Text != "")
                {
                    where += " and fecha_trans <= convert(datetime, '" + txt_hasta2.Text + "', 103) ";

                }
            }

            if (rd_abi.Checked)
            {



                dt = db.consultar("SELECT DISTINCT vendedor, nombrevendedor FROM V_COBRANZA_docs " + where);

            }
            else
            {
                dt = db.consultar("SELECT DISTINCT vendedor, nombrevendedor FROM V_COBRANZA_todos " + where);

            }

            //dt.Rows.Add(new Object[] { -1, "-- Seleccione --" });
            DataView dv = dt.DefaultView;
            dv.Sort = "nombrevendedor asc";
            DataTable sortedDT = dv.ToTable();
            CB_VENDEDOR_GRILLA.DataSource = sortedDT;
            CB_VENDEDOR_GRILLA.DataValueField = "vendedor";
            CB_VENDEDOR_GRILLA.DataTextField = "nombrevendedor";
            CB_VENDEDOR_GRILLA.DataBind();
            try
            {
                CB_VENDEDOR_GRILLA.SelectedValue = "-1";
            }
            catch { }

            dt.Rows.Add(new Object[] { -1, "-- Seleccione --" });
            dv.Sort = "nombrevendedor asc";
            DataTable sortedDT1 = dv.ToTable();
            CB_VENDEDOR_CHEQ.DataSource = sortedDT1;
            CB_VENDEDOR_CHEQ.DataValueField = "vendedor";
            CB_VENDEDOR_CHEQ.DataTextField = "nombrevendedor";
            CB_VENDEDOR_CHEQ.DataBind();

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            MakeAccessible(G_INIT);
            MakeAccessible(G_MOV_SOL);
            MakeAccessible(G_CHEQUES);
            //MakeAccessible(G_LISTA);
        }
        public void JQ_Datatable()
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teasd121mp", "<script language='javascript'>creagrilla();</script>", false);
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

        //FILTRAR
        private void LlenarGrilla()
        {
            //if (txt_desde2.Text != "" || txt_hasta2.Text != "" || num_docum.Text != "")
            //{
            busca_columna_fac = true;
            columna_fac = false;

            string tipo_doc = "";
            foreach (ListItem item in CB_TIPO_DOC_GRILLA.Items)
            {

                if (item.Selected)
                {
                    tipo_doc += item.Value + ", ";
                }

            }

            if (tipo_doc == "-1")
            {
                tipo_doc = "";
            }
            else if (tipo_doc == "")
            {
                tipo_doc = "";
            }
            else
            {


                string aux_tipo = tipo_doc;
                aux_tipo = aux_tipo.Substring(0, aux_tipo.Length - 2);
                tipo_doc = tipo_doc.Substring(0, tipo_doc.Length - 2);
                tipo_doc = " and [Nombre_Tipo] IN ( " + agregra_comillas(aux_tipo) + ")";

                //if (tipo_doc.Contains("ND") && !tipo_doc.Contains("DM"))
                //{

                //    tipo_doc += "  and isnumeric(factura) = 1 ";
                //    tipo_doc = tipo_doc.Replace("ND", "DM");
                //}

                //else if (!tipo_doc.Contains("ND") && tipo_doc.Contains("DM"))
                //{
                //    tipo_doc += "  and isnumeric(factura) <> 1 ";
                //}
                //else if (tipo_doc.Contains("ND") && tipo_doc.Contains("DM"))
                //{
                //    tipo_doc = " and tipo_doc IN ( " + agregra_comillas(aux_tipo) + ")";
                //}

            }



            if (L_CLIENTES.Text != "")
            {
                tipo_doc += " and [rutcliente] in (" + agregra_comillas(L_CLIENTES.Text) + ") ";
            }


            if (L_VENDEDORES.Text != "")
            {
                tipo_doc += " and [vendedor] in (" + agregra_comillas(L_VENDEDORES.Text) + ") ";
            }
            //if (CB_VENDEDOR_GRILLA.SelectedValue != null && CB_VENDEDOR_GRILLA.SelectedValue != "-1")
            //{
            //    tipo_doc += " and VENDEDOR = '" + CB_VENDEDOR_GRILLA.SelectedValue.ToString() + "'";
            //}



            if (rd_ven.Checked)
            {
                if (txt_desde2.Text != "")
                {

                    tipo_doc += " and [FVenc] >= convert(datetime, '" + txt_desde2.Text + "', 103) ";
                }
                if (txt_hasta2.Text != "")
                {
                    tipo_doc += " and [FVenc] <= convert(datetime, '" + txt_hasta2.Text + "', 103) ";

                }
            }
            else
            {
                if (txt_desde2.Text != "")
                {

                    tipo_doc += " and [FTrans] >= convert(datetime, '" + txt_desde2.Text + "', 103) ";
                }
                if (txt_hasta2.Text != "")
                {
                    tipo_doc += " and [FTrans] <= convert(datetime, '" + txt_hasta2.Text + "', 103) ";

                }
            }

            if (num_docum.Text != "")
            {
                tipo_doc += " and [NºDoc] in (" + agregra_comillas(num_docum.Text) + ") ";

            }


            //if (CB_CLIENTE_GRILLA.SelectedValue != null && CB_CLIENTE_GRILLA.SelectedValue != "-1")
            //{
            //    tipo_doc += " and rutcliente = '" + CB_CLIENTE_GRILLA.SelectedValue.ToString() + "'";
            //}


            sum_peso_saldo = 0;

            sum_dolar_saldo = 0;

            sum_peso_monto = 0;

            sum_dolar_monto = 0;

            cont_rows = 0;
            //CARGA REAL DT
            DataTable au = new DataTable();
            if (rd_abi.Checked)
            {
                au = ReporteRNegocio.trae_docu_calend(tipo_doc, User.Identity.Name);
                TOTAL_ROWS = au.Rows.Count;
                G_INIT.DataSource = au;
                G_INIT.DataBind();
                cerrad_abier.Visible = true;
            }
            else
            {
                au = ReporteRNegocio.trae_docu_calend_CERRADOS(tipo_doc, User.Identity.Name);
                TOTAL_ROWS = au.Rows.Count;
                G_INIT.DataSource = au;
                G_INIT.DataBind();
                ////aca para pagar facturas cerradas
                cerrad_abier.Visible = false;
            }

            Session["aux_dt_excel"] = au;


            //DataTable au = ReporteRNegocio.trae_docu_calend(tipo_doc);
            //TOTAL_ROWS = au.Rows.Count;
            //G_INIT.DataSource = au;
            //G_INIT.DataBind();



            //var elem3 = document.getElementById("ocultar_principio");
            //elem3.style.display = "block";
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "tea2sd121mp", "<script language='javascript'>creagrilla_g_init();</script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "tweeee", "<script> var elem3 = document.getElementById(\"ocultar_principio\");    elem3.style.display = \"block\";</script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "relojito_false1", "<script> relojito(false); </script>", false);
            //}
            //else {

            btn_pagables.Visible = false;
            BTN_NETEO_2.Visible = false;
            P_FECHA_NET.Visible = false;
            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "tq121eeee", "<script> alert('Sin filtro fecha') </script>", false);


            //}
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

        public void LlenarComboCliente()
        {
            DBUtil db = new DBUtil();
            DataTable dt = new DataTable();
            DataTable dt_para_grilla = new DataTable();

            string where = " where 1=1 ";


            if (rd_ven.Checked)
            {
                if (txt_desde2.Text != "")
                {

                    where += " and fecha_venc >= convert(datetime, '" + txt_desde2.Text + "', 103) ";
                }
                if (txt_hasta2.Text != "")
                {
                    where += " and fecha_venc <= convert(datetime, '" + txt_hasta2.Text + "', 103) ";

                }
            }
            else
            {
                if (txt_desde2.Text != "")
                {

                    where += " and fecha_trans >= convert(datetime, '" + txt_desde2.Text + "', 103) ";
                }
                if (txt_hasta2.Text != "")
                {
                    where += " and fecha_trans <= convert(datetime, '" + txt_hasta2.Text + "', 103) ";

                }
            }


            if (rd_abi.Checked)
            {

                dt = dt_para_grilla = db.consultar("SELECT DISTINCT RUTCLIENTE, NOMBRECLIENTE FROM V_COBRANZA_docs " + where);

            }
            else
            {
                dt = dt_para_grilla = db.consultar("SELECT DISTINCT RUTCLIENTE, NOMBRECLIENTE FROM V_COBRANZA_todos " + where);

            }

            //dt2 = dt;
            dt.Rows.Add(new Object[] { -1, "-- Seleccione --" });
            DataView dv = dt.DefaultView;
            dv.Sort = "NOMBRECLIENTE asc";
            DataTable sortedDT = dv.ToTable();
            CB_CLIENTE.DataSource = sortedDT;
            CB_CLIENTE.DataValueField = "RUTCLIENTE";
            CB_CLIENTE.DataTextField = "NOMBRECLIENTE";
            CB_CLIENTE.DataBind();
            //CB_CLIENTE.SelectedValue = "-1";


            DataTable sortedDT2 = dv.ToTable();
            CB_CLIENTE_GRILLA.DataSource = sortedDT2;
            CB_CLIENTE_GRILLA.DataValueField = "RUTCLIENTE";
            CB_CLIENTE_GRILLA.DataTextField = "NOMBRECLIENTE";
            CB_CLIENTE_GRILLA.DataBind();
            //CB_CLIENTE_GRILLA.SelectedValue = "-1";

            CB_CLIENTE_CHEQ.DataSource = sortedDT2;
            CB_CLIENTE_CHEQ.DataValueField = "RUTCLIENTE";
            CB_CLIENTE_CHEQ.DataTextField = "NOMBRECLIENTE";
            CB_CLIENTE_CHEQ.DataBind();

            llenarcombocliente2();

        }

        private void llenarcombocliente2()
        {
            DBUtil db = new DBUtil();
            DataTable dt = new DataTable();
            DataTable dt_para_grilla = new DataTable();
            dt = dt_para_grilla = db.consultar("SELECT DISTINCT CustId, Name  from customer ");

            DataView dv2 = dt.DefaultView;
            dv2.Sort = "Name asc";
            DataTable sortedDT2 = dv2.ToTable();
            cb_cliente_3.DataSource = sortedDT2;
            cb_cliente_3.DataValueField = "CustId";
            cb_cliente_3.DataTextField = "Name";
            cb_cliente_3.DataBind();
            //CB_CLIENTE_GRILLA.SelectedValue = "-1";
        }

        public void LlenarComboBancos()
        {
            DBUtil db = new DBUtil();
            DataTable dt = new DataTable();
            dt = db.consultar("SELECT ID, LTRIM( isnull(ACCT,'')) +' - ' + NOM_BANCO  AS NOM_BANCO FROM COBRANZA_BANCOS  where nom_banco not like '%deposito%'");
            dt.Rows.Add(new Object[] { -1, "-- Seleccione Banco --" });
            DataView dv = dt.DefaultView;
            dv.Sort = "NOM_BANCO asc";
            DataTable sortedDT = dv.ToTable();

            CB_BANCOS2.DataSource = sortedDT;
            CB_BANCOS2.DataValueField = "ID";
            CB_BANCOS2.DataTextField = "NOM_BANCO";
            CB_BANCOS2.DataBind();
            try
            {
                CB_BANCOS2.SelectedValue = "-1";
            }
            catch { }
            CB_BANCOS.DataSource = sortedDT;
            CB_BANCOS.DataValueField = "ID";
            CB_BANCOS.DataTextField = "NOM_BANCO";
            CB_BANCOS.DataBind();
            try
            {
                CB_BANCOS.SelectedValue = "-1";
            }
            catch { }

            cb_bancos3.DataSource = sortedDT;
            cb_bancos3.DataValueField = "ID";
            cb_bancos3.DataTextField = "NOM_BANCO";
            cb_bancos3.DataBind();
            try
            {
                cb_bancos3.SelectedValue = "-1";
            }
            catch { }

            dt = db.consultar("SELECT * FROM COBRANZA_TIPO_DOLAR ");
            dt.Rows.Add(new Object[] { -1, "-- Seleccione TIPO DOLAR --" });
            dv = dt.DefaultView;
            dv.Sort = "TIPO_DOLAR desc";
            sortedDT = dv.ToTable();

            CB_TIPO_DOLAR.DataSource = sortedDT;
            CB_TIPO_DOLAR.DataValueField = "TIPO_DOLAR";
            CB_TIPO_DOLAR.DataTextField = "TIPO_DOLAR";
            CB_TIPO_DOLAR.DataBind();
            try
            {
                CB_TIPO_DOLAR.SelectedValue = "-- Seleccione TIPO DOLAR --";
            }
            catch { }

            LlenarComboBancos_deposito();
        }

        public void LlenarComboBancos_deposito()
        {
            DBUtil db = new DBUtil();
            DataTable dt = new DataTable();
            dt = db.consultar("SELECT COD_BANCO, LTRIM( isnull(ACCT,'')) +' - ' + NOM_BANCO  AS NOM_BANCO FROM COBRANZA_BANCOS where TIPO_CUENTA = 'SOPRODI'");
            dt.Rows.Add(new Object[] { -1, "-- Seleccione Banco --" });
            DataView dv = dt.DefaultView;
            dv.Sort = "NOM_BANCO asc";
            DataTable sortedDT = dv.ToTable();
            CB_DEPOSITOS_BANCO.DataSource = sortedDT;
            CB_DEPOSITOS_BANCO.DataValueField = "COD_BANCO";
            CB_DEPOSITOS_BANCO.DataTextField = "NOM_BANCO";
            CB_DEPOSITOS_BANCO.DataBind();

            try
            {
                CB_DEPOSITOS_BANCO.SelectedValue = "-1";
            }
            catch { }

            CB_DEPOSITOS_BANCO2.DataSource = sortedDT;
            CB_DEPOSITOS_BANCO2.DataValueField = "COD_BANCO";
            CB_DEPOSITOS_BANCO2.DataTextField = "NOM_BANCO";
            CB_DEPOSITOS_BANCO2.DataBind();

            try
            {
                CB_DEPOSITOS_BANCO2.SelectedValue = "-1";
            }
            catch { }

            CB_DEPOSITOS_BANCO3.DataSource = sortedDT;
            CB_DEPOSITOS_BANCO3.DataValueField = "COD_BANCO";
            CB_DEPOSITOS_BANCO3.DataTextField = "NOM_BANCO";
            CB_DEPOSITOS_BANCO3.DataBind();
            try
            {
                CB_DEPOSITOS_BANCO3.SelectedValue = "-1";
            }
            catch { }
        }


        public void LlenarComboBancos_SOPRODI()
        {
            DBUtil db = new DBUtil();
            DataTable dt = new DataTable();
            dt = db.consultar("SELECT ACCT, LTRIM( isnull(ACCT,'')) +' - ' + NOM_BANCO  AS NOM_BANCO FROM COBRANZA_BANCOS  where TIPO_CUENTA = 'SOPRODI'  ");
            dt.Rows.Add(new Object[] { -1, "-- Seleccione Banco --" });
            DataView dv = dt.DefaultView;
            dv.Sort = "NOM_BANCO asc";
            DataTable sortedDT = dv.ToTable();

            cb_banco_soprodi.DataSource = sortedDT;
            cb_banco_soprodi.DataValueField = "ACCT";
            cb_banco_soprodi.DataTextField = "NOM_BANCO";
            cb_banco_soprodi.DataBind();
            try
            {
                cb_banco_soprodi.SelectedValue = "-1";
            }
            catch { }

            CB_BANCO_SOPRODI3.DataSource = sortedDT;
            CB_BANCO_SOPRODI3.DataValueField = "ACCT";
            CB_BANCO_SOPRODI3.DataTextField = "NOM_BANCO";
            CB_BANCO_SOPRODI3.DataBind();
            try
            {
                CB_BANCO_SOPRODI3.SelectedValue = "-1";
            }
            catch { }
        }


        public class TABLA_ACC
        {
            public string TABLA { get; set; }
        }
        public class CLIENTE
        {
            public string nombre { get; set; }
            public string rut { get; set; }
        }


        [WebMethod]
        public static List<CLIENTE> CLIENTES_X_VENDEDOR(string vendedores, string cerrado_o_abierto)

        {

            DataTable dt = new DataTable();
            string vende = agregra_comillas2(vendedores);

            string where = " where 1=1 ";

            if (!vende.Contains("'-1'") && vende != "" && !vende.Contains("'-- Todos --'"))
            {
                where += " and vendedor in (" + vende + ")";
            }

            if (vendedores == "")
            {
                where += " and 1=1 ";

            }
            //else
            //{
            try
            {

                if (cerrado_o_abierto == "1")
                {
                    dt = ReporteRNegocio.listar_clientes_cobranza_abiertos(where);

                }
                else
                {

                    dt = ReporteRNegocio.listar_clientes_cobranza(where);
                }
                ////dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
                DataView dv = dt.DefaultView;
                dv.Sort = "nombre";
                dt = dv.ToTable();
            }
            catch { }
            //}
            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new CLIENTE
                        {
                            rut = Convert.ToString(item["rut"]),
                            nombre = Convert.ToString(item["nombre"])
                        };
            return query.ToList<CLIENTE>();


        }


        private static string agregra_comillas2(string p)
        {
            if (p != "")
            {
                if (p.Substring(0, 1) != "'")
                {
                    string final_comillas = "";
                    if (p.Contains(","))
                    {
                        List<string> CLIENTE = new List<string>();
                        List<string> cliente_ = new List<string>();
                        CLIENTE.AddRange(p.Split(','));
                        foreach (string a in CLIENTE)
                        {
                            cliente_.Add("'" + a.Trim() + "'");
                        }
                        final_comillas = string.Join(",", cliente_);
                    }
                    else { final_comillas = "'" + p.Trim() + "'"; }
                    return final_comillas;
                }
                else { return p; }
            }
            else { return p; }

        }
        [WebMethod]
        public static string grafico()
        {
            string strin = "";
            string strin2 = "";
            DataTable semanas = ReporteRNegocio.trae_4_semanas();



            strin += "<table id='datatable' style='visibility:hidden; position:absolute;'> ";
            strin += " <thead>";
            strin += "     <tr>";
            strin += "     <th></th>";
            strin += "        <th>Peso</th>";
            //strin += "      <th>Dolar</th>";
            strin += "      </tr>";
            strin += "   </thead>";
            strin += "   <tbody>";
            foreach (DataRow r in semanas.Rows)
            {
                DataTable datos_semana = ReporteRNegocio.peso_dolar_semana(r[0].ToString(), r[1].ToString());
                foreach (DataRow r2 in datos_semana.Rows)
                {
                    strin += " <tr>";
                    strin += "   <th> " + r[0].ToString() + " Semana</th>";
                    strin += "   <td> " + r2[0].ToString() + " </td >";
                    //strin += "   <td> " + r2[1].ToString() + " </td >";
                    strin += " </tr>";
                }

            }



            strin += "  </tbody>";
            strin += "     </table>";


            strin2 += "<table id='datatable2' style='visibility:hidden; position:absolute;'> ";
            strin2 += " <thead>";
            strin2 += "     <tr>";
            strin2 += "     <th></th>";
            //strin2 += "        <th>Peso</th>";
            strin2 += "      <th>Dolar</th>";
            strin2 += "      </tr>";
            strin2 += "   </thead>";
            strin2 += "   <tbody>";
            foreach (DataRow r in semanas.Rows)
            {
                DataTable datos_semana = ReporteRNegocio.peso_dolar_semana(r[0].ToString(), r[1].ToString());
                foreach (DataRow r2 in datos_semana.Rows)
                {
                    strin2 += " <tr>";
                    strin2 += "   <th> " + r[0].ToString() + " Semana</th>";
                    //strin2 += "   <td> " + r2[0].ToString() + " </td >";
                    strin2 += "   <td> " + r2[1].ToString() + " </td >";
                    strin2 += " </tr>";
                }

            }



            strin2 += "  </tbody>";
            strin2 += "     </table>";



            return strin.Replace(",", ".") + "*" + strin2.Replace(",", ".");
        }

        private static DataTable calcula_5_semanas(DataTable semanas)
        {

            if (semanas.Rows.Count == 1)
            {
                DataRow sd = semanas.NewRow();
                sd[0] = 53;
                semanas.Rows.Add(sd);
                sd = semanas.NewRow();
                sd[0] = 52;
                semanas.Rows.Add(sd);
                sd = semanas.NewRow();
                sd[0] = 51;
                semanas.Rows.Add(sd);
                sd = semanas.NewRow();
                sd[0] = 50;
                semanas.Rows.Add(sd);
            }

            if (semanas.Rows.Count == 2)
            {
                DataRow sd = semanas.NewRow();
                sd[0] = 53;
                semanas.Rows.Add(sd);
                sd = semanas.NewRow();
                sd[0] = 52;
                semanas.Rows.Add(sd);
                sd = semanas.NewRow();
                sd[0] = 51;
                semanas.Rows.Add(sd);

            }

            if (semanas.Rows.Count == 3)
            {
                DataRow sd = semanas.NewRow();
                sd[0] = 53;
                semanas.Rows.Add(sd);
                sd = semanas.NewRow();
                sd[0] = 52;
                semanas.Rows.Add(sd);

            }
            if (semanas.Rows.Count == 4)
            {
                DataRow sd = semanas.NewRow();
                sd[0] = 53;
                semanas.Rows.Add(sd);

            }
            //DataView dv = semanas.DefaultView;
            //dv.Sort = "semana";
            //semanas = dv.ToTable();
            return semanas;
        }

        [WebMethod]
        public static string ver_acciones(string id)
        {
            DataTable dt = new DataTable();
            DBUtil db = new DBUtil();
            string tabla = "";

            // CAMPOS
            if (!Regex.IsMatch(id.Trim(), @"^[0-9]+$"))
            {
                dt = db.consultar("select num_factura_origen from Cobranza_seguimiento where num_factura = '" + id + "'");
                foreach (DataRow dr in dt.Rows)
                {
                    id = dr["num_factura_origen"].ToString();
                }
            }


            dt = db.consultar("select id_cobranza, (select top 1 nom_accion from acciones a where Cobranza_Acciones.id_accion = a.id_accion) as accion,  fecha_accion as fecha, usuario, obs from Cobranza_Acciones where ID_cobranza = '" + id + "'");

            tabla += "<table class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";
            tabla += "<th>Num Factura</th>";
            tabla += "<th>Acción</th>";
            tabla += "<th>Fecha</th>";
            tabla += "<th>Usuario</th>";
            tabla += "<th>Descripción</th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            foreach (DataRow dr in dt.Rows)
            {
                tabla += "<tr>";
                tabla += "<td>" + dr["id_cobranza"].ToString() + "</td>";
                tabla += "<td>" + dr["accion"].ToString() + "</td>";
                tabla += "<td>" + dr["fecha"].ToString() + "</td>";
                tabla += "<td>" + dr["usuario"].ToString() + "</td>";
                tabla += "<td>" + dr["obs"].ToString() + "</td>";
                tabla += "</tr>";
            }
            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");
            return tabla;
        }
        [WebMethod]
        public static string EliminarPago_2(string factura, string fecha, string obs)
        {


            if (obs.Contains("-"))
            {
                string num_factura_origen = ReporteRNegocio.trae_num_factura_origen(factura);
                string elimina = ReporteRNegocio.eliminar_pago_fac(num_factura_origen.Replace("--", ",").Replace("'", ""), fecha, obs);
                return elimina;
            }
            if (obs.Contains("Cheque"))
            {
                string num_factura_cheque = ReporteRNegocio.trae_stuff_facturas_de_cheque(factura, fecha);
                string elimina = ReporteRNegocio.eliminar_pago_fac(num_factura_cheque.Replace("--", ",").Replace("'", ""), fecha, obs);
                return elimina;
            }

            else
            {
                string stuff_facturas = ReporteRNegocio.trae_stuff_facturas(factura);

                string elimina = ReporteRNegocio.eliminar_pago_fac(stuff_facturas.Replace("--", ",").Replace("'", ""), fecha, obs);
                return elimina;
            }
            return "";
        }

        [WebMethod]
        public static string EliminarPagoDirecto(string id_seguimiento_)
        {
            return ReporteRNegocio.eliminar_segui_id(id_seguimiento_);
        }

        private static string agregar_comillas(string p)
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


        [WebMethod]
        public static string accion_prioridad(string id, string id_cobranza)
        {
            string estado_ = "";
            string elimina = ReporteRNegocio.eliminar_accion_prio(id, id_cobranza);
            return estado_;
        }

        [WebMethod]
        public static string EliminarPagoSegui(string id)
        {
            DataTable dt = ReporteRNegocio.trae_registro(id.Trim());
            string estado = "";
            string num_factura_origen = "";
            string obs = "";
            string fecha = "";
            foreach (DataRow r in dt.Rows)
            {
                estado = r[4].ToString();
                num_factura_origen = r[8].ToString();
                obs = r[6].ToString();
                fecha = r["fecha"].ToString();
            }

            if (estado == "ABONO" && !obs.Contains("Cheque") || estado == "NOTA_CREDITO" || estado == "SALDO_FAVOR")
            {
                string facturas_pagos = ReporteRNegocio.stuff_facturas_pagos(obs.Trim());

                string ids = ReporteRNegocio.trae_ids_segui(agregar_comillas(num_factura_origen.Replace("-", ",")), fecha, obs);
                string elimina = ReporteRNegocio.eliminar_segui_id(ids.Replace("'", ""));
                return facturas_pagos.Replace(",", "-");
            }
            else
            {
                string cheques_obs = obs.Split('*').ToList()[1];
                string facturas_pagos = ReporteRNegocio.stuff_facturas_pagos(cheques_obs.Trim());
                string elim = ReporteRNegocio.eliminar_por_like_obs(cheques_obs);
                return facturas_pagos.Replace(",", "-");
            }
            return "";
        }

        [WebMethod]
        public static List<Evento_objeto> CargarEvento55(string id, string rut, string factura, string tipo_doc)
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt4 = new DataTable();
            DataTable dt3 = new DataTable();
            DBUtil db = new DBUtil();

            string tipo_doc_aux = tipo_doc;
            if (tipo_doc == "temporal") { tipo_doc = "IN"; }
            string tabla = "";
            int estado_result = 0;
            string cheques = "";
            string bankacct = ReporteRNegocio.cuenta_banco(factura);
            string es_cheque_factura = "";
            string factura_aux = factura;
            // CAMPOS

            if (bankacct.Trim() != "110405")
            {

                if (!Regex.IsMatch(factura.Trim(), @"^[0-9]+$") && (factura.Substring(0, 2) != "F "))
                {
                    es_cheque_factura = factura;



                    dt = db.consultar("SELECT id, nombrecliente, factura, rutcliente, CONVERT(varchar(20), fecha_trans, 103) as fecha_trans, CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, monto_doc, transfor FROM V_COBRANZA_TODOS WHERE RUTCLIENTE = '" + rut + "' AND FACTURA = '" + factura + "'  AND tipo_doc <> 'PA'");
                    try
                    {
                        factura = (db.Scalar("select top 1 num_factura_origen from cobranza_seguimiento where num_factura = '" + factura + "' and rutcliente = '" + rut + "' and tipo_doc = '" + tipo_doc + "'").ToString());
                        if (es_cheque_factura != "") { tipo_doc = "IN"; }
                        if (factura == "") { factura = factura_aux; }
                    }
                    catch { }
                }
                else
                {

                    dt = db.consultar("SELECT id, nombrecliente, factura, rutcliente, CONVERT(varchar(20), fecha_trans, 103) as fecha_trans, CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, (select ISNULL( (select sum(monto) * -1 from Cobranza_pagos where id_cobranza  = '" + factura + "'),0) + monto_doc ) as monto_doc FROM V_COBRANZA_TODOS WHERE RUTCLIENTE = '" + rut + "' AND FACTURA = '" + factura + "'");

                }
            }
            else
            {
                dt = db.consultar("SELECT id, nombrecliente, factura, rutcliente, CONVERT(varchar(20), fecha_trans, 103) as fecha_trans, CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, (select ISNULL( (select sum(monto) * -1 from Cobranza_pagos where id_cobranza  = '" + factura + "'),0) + monto_doc ) as monto_doc FROM V_COBRANZA_TODOS WHERE RUTCLIENTE = '" + rut + "' AND FACTURA = '" + factura + "'");

            }


            estado_result = Convert.ToInt32(db.Scalar("select count(*) from cobranza_seguimiento where num_factura = '" + factura + "' and rutcliente = '" + rut + "' ").ToString());


            //// TABLA MOV_ASOCIADOS A LA FACTURA
            //if (es_cheque_factura != "")
            //{
            //    dt2 = db.consultar("SELECT id, CONVERT(varchar(20), fecha_trans, 103) as fecha_trans, CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, '$ ' + dbo.F_Separador_miles(CONVERT(numeric, monto_doc)) as monto_doc, '$ ' + replace(dbo.F_Separador_miles(replace(monto_usd_original, '.',',')),'.,',',') as transfor,descr, factura, tipo_doc, case when estado_doc = '0' then 'CERRADO' when estado_doc = '1' then 'ABIERTO' END AS estado, '$ ' + dbo.F_Separador_miles(CONVERT(numeric, saldo))  as saldo , tipo_moneda , tasa_camb, '$ ' + replace(dbo.F_Separador_miles(replace(saldo_dolar, '.',',')),'.,',',')  AS saldolar   FROM V_COBRANZA WHERE RUTCLIENTE = '" + rut + "' AND FACTURA = '" + es_cheque_factura + "' ORDER BY fecha_trans desc");

            //}
            //else
            //{
            dt2 = db.consultar("SELECT id, CONVERT(varchar(20), fecha_trans, 103) as fecha_trans, CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, '$ ' + dbo.F_Separador_miles(CONVERT(numeric, monto_doc)) as monto_doc, '$ ' + replace(dbo.F_Separador_miles(replace(monto_usd_original, '.',',')),'.,',',') as transfor, descr, factura, tipo_doc, case when estado_doc = '0' then 'CERRADO' when estado_doc = '1' then 'ABIERTO' END AS estado, '$ ' + dbo.F_Separador_miles(CONVERT(numeric, saldo))  as saldo , tipo_moneda, tasa_camb , '$ ' + replace(dbo.F_Separador_miles(replace(saldo_dolar, '.',',')),'.,',',')  AS saldolar  FROM V_COBRANZA_TODOS WHERE RUTCLIENTE = '" + rut + "' AND FACTURA = '" + factura + "' ORDER BY fecha_trans desc");

            //}


            tabla += "<table class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";
            tabla += "<th>Abrir/Cerrar</th>";
            tabla += "<th>Num Factura</th>";
            tabla += "<th>Tipo Doc.</th>";
            tabla += "<th>Descripcion</th>";
            tabla += "<th>Fecha</th>";
            tabla += "<th>Vencimiento</th>";
            tabla += "<th>Monto (Peso)</th>";
            tabla += "<th>Monto (Dolar)</th>";
            tabla += "<th>Saldo (Peso)</th>";
            tabla += "<th>Saldo (Dolar)</th>";
            tabla += "<th>Moneda </th>";
            tabla += "<th>Estado Solomon</th>";
            tabla += "<th>Estado App</th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            foreach (DataRow dr in dt2.Rows)
            {
                tabla += "<tr>";

                string script2 = string.Format("javascript:ESTADO_DOC(&#39;{0}&#39;,&#39;{1}&#39;,&#39;{2}&#39;)", dr["factura"].ToString().Trim(), dr["id"].ToString().Trim(), rut.Trim());

                tabla += "<td> <input type='button' class='btn-red' onclick='" + script2 + "' /> </td>";
                tabla += "<td>" + dr["factura"].ToString() + "</td>";
                tabla += "<td>" + dr["tipo_doc"].ToString() + "</td>";
                tabla += "<td>" + dr["descr"].ToString() + "</td>";
                tabla += "<td>" + dr["fecha_trans"].ToString() + "</td>";
                tabla += "<td>" + dr["fecha_venc"].ToString() + "</td>";
                bool es_cheque = false;
                try
                {
                    int result = int.Parse(dr["factura"].ToString().Trim());
                    tabla += "<td>" + dr["monto_doc"].ToString() + "</td>";
                }
                catch
                {
                    es_cheque = true;
                    tabla += "<td>" + dr["saldo"].ToString() + "</td>";
                }

                tabla += "<td>" + dr["transfor"].ToString() + "</td>";
                tabla += "<td>" + dr["saldo"].ToString() + "</td>";
                tabla += "<td>" + dr["saldolar"].ToString() + "</td>";
                tabla += "<td>" + dr["tipo_moneda"].ToString() + "</td>";
                tabla += "<td>" + dr["estado"].ToString() + "</td>";
                tabla += "<td>" + ReporteRNegocio.estado_app(dr["estado"].ToString(), dr["id"].ToString().Trim(), dr["factura"].ToString().Trim()) + "</td>";
                tabla += "</tr>";
                if (!Regex.IsMatch(factura.Trim(), @"^[0-9]+$"))
                {
                    try
                    {
                        factura = dr["num_factura_origen"].ToString();
                    }
                    catch { }
                }
                else
                {
                }
            }
            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");

            clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
            if (rut.Substring(0, 1) == "0")
            {
                rut = rut.Substring(1, rut.Length - 1);
            }
            var ver_cliente = "<a href='FICHA_CLIENTE.ASPX?R=" + encriptador.EncryptData(rut) + "&i=" + encriptador.EncryptData("88") + "'  target='_blank'>Ver Ficha</a>";


            // TABLA aca RECUPERAR NUM_FACTURA PARA CHEQUE
            string origen = "";
            if (estado_result >= 1)
            {
                if (tipo_doc_aux == "IN" && estado_result == 1)
                {

                }
                else
                {

                    if (tipo_doc == "IN" || tipo_doc == "DM")
                    {
                        dt3 = db.consultar("SELECT id, CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, CONVERT(varchar(20), fecha, 103) as fecha_trans, '$ ' + REPLACE(dbo.F_Separador_miles(monto_doc),'..',',') as monto_doc, " +
                            "observacion, num_factura, tipo_doc, num_factura_origen, '' as sw  FROM COBRANZA_SEGUIMIENTO WHERE NUM_FACTURA = '" + factura + "' or num_factura_origen like '%" + factura.Trim() + "%'  ORDER BY fecha_trans desc");


                        DataTable dt4_cheque_pagado = db.consultar("SELECT id, CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, CONVERT(varchar(20), fecha, 103) as fecha_trans, '$ ' + REPLACE(dbo.F_Separador_miles(monto_doc),'..',',') as monto_doc, " +
                            "observacion, num_factura, tipo_doc, num_factura_origen, '' as sw  FROM COBRANZA_SEGUIMIENTO WHERE NUM_FACTURA = '" + factura_aux + "' and num_factura_origen like '%" + factura_aux.Trim() + "%'  ORDER BY fecha_trans desc");


                        dt3.Merge(dt4_cheque_pagado);

                        var result = from rows in dt3.AsEnumerable()
                                     group rows by new
                                     {
                                         id = rows["id"]
                                                        ,
                                         fecha_venc = rows["fecha_venc"]
                                                        ,
                                         fecha_trans = rows["fecha_trans"]
                                                        ,
                                         monto_doc = rows["monto_doc"]
                                                        ,
                                         observacion = rows["observacion"]
                                                        ,
                                         num_factura = rows["num_factura"]
                                                        ,
                                         tipo_doc = rows["tipo_doc"]
                                                        ,
                                         num_factura_origen = rows["num_factura_origen"]
                                                        ,
                                         sw = rows["sw"]
                                     } into grp
                                     select grp;

                        DataTable dt_final = new DataTable();
                        dt_final.Columns.Add("id");
                        dt_final.Columns.Add("fecha_venc");
                        dt_final.Columns.Add("fecha_trans");
                        dt_final.Columns.Add("monto_doc");
                        dt_final.Columns.Add("observacion");
                        dt_final.Columns.Add("num_factura");
                        dt_final.Columns.Add("tipo_doc");
                        dt_final.Columns.Add("num_factura_origen");
                        dt_final.Columns.Add("sw");

                        foreach (var item in result)
                        {
                            DataRow row = dt_final.NewRow();
                            row[0] = item.Key.id;
                            row[1] = item.Key.fecha_venc;
                            row[2] = item.Key.fecha_trans;
                            row[3] = item.Key.monto_doc;
                            row[4] = item.Key.observacion;
                            row[5] = item.Key.num_factura;
                            row[6] = item.Key.tipo_doc;
                            row[7] = item.Key.num_factura_origen;
                            row[8] = item.Key.sw;
                            dt_final.Rows.Add(row);

                        }


                        tabla += "<h3>Documentos asociados por aplicación</h3>";

                        tabla += "<table class=\"table fill-head table-bordered\">";
                        tabla += "<thead class=\"test\">";
                        tabla += "<tr>";
                        tabla += "<th>Eliminar</th>";
                        tabla += "<th>Num Factura</th>";
                        tabla += "<th>Tipo Doc.</th>";
                        tabla += "<th>Descripcion</th>";
                        tabla += "<th>Fecha</th>";
                        tabla += "<th>Vencimiento</th>";
                        tabla += "<th>Monto Doc.</th>";
                        tabla += "</tr>";
                        tabla += "</thead>";
                        tabla += "<tbody>";
                        string iidd = id;
                        bool swo = false;

                        List<string> che = new List<string>();

                        //procesar los num_factura_origen para que cuadre con la factura en documentos asociados

                        foreach (DataRow dr in dt_final.Rows)
                        {
                            bool es_asociado = false;
                            int cont_fact = 0;
                            List<string> fact = dr["num_factura_origen"].ToString().Split('-').ToList();
                            foreach (string f in fact)
                            {
                                if (f.Trim() == factura.Trim() || f.Trim() == factura_aux.Trim())
                                {

                                    es_asociado = true;
                                }

                                if (fact.Count == cont_fact) { es_asociado = false; }

                                cont_fact++;
                            }
                            if (es_asociado)
                            {

                                dr["sw"] = "si";
                            }
                            else
                            {

                                dr["sw"] = "no";
                            }

                        }

                        foreach (DataRow dr in dt_final.Rows)
                        {
                            if (dr["sw"].ToString() == "si")
                            {
                                if (dr["tipo_doc"].ToString() != "IN")
                                {
                                    if (dr["monto_doc"].ToString().Trim().Replace(",", ".") != "$ 0.00".Trim())
                                    {
                                        if (dr["tipo_doc"].ToString() != "DM")
                                        {
                                            //factura.Trim()
                                            //ACA BORRAR PAGO EN CERO
                                            string script22 = string.Format("eliminar_pago_en_pag(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{8}&#39;);" +
                                                                         "   eliminar_pago_en_s(&#39;{2}&#39;,&#39;{3}&#39;,&#39;{4}&#39;,&#39;{5}&#39;,&#39;{6}&#39;, &#39;{7}&#39;);",
                                                                            dr["num_factura"].ToString().Trim(), dr["fecha_trans"].ToString().Trim(),
                                                                            dr["id"].ToString().Trim(), iidd.Trim(), rut, factura, tipo_doc, dr["fecha_trans"].ToString().Trim()
                                                                            , dr["observacion"].ToString());
                                            tabla += "<tr>";
                                            //< a id = "ContentPlaceHolder_Contenido_btn_excel" class="btn btn-circle show-tooltip fa fa-file-text-o" title="Exportar a Excel" href="javascript:__doPostBack('ctl00$ContentPlaceHolder_Contenido$btn_excel','')" style="margin-left: 5px"></a>
                                            tabla += "<td><a style='background-color: rgb(255, 97, 97);' class=\"btn btn-circle show-tooltip fa fa-trash\" onclick=\"" + script22 + "\"></a></td>";
                                            tabla += "<td>" + dr["num_factura"].ToString() + "</td>";
                                            tabla += "<td>" + dr["tipo_doc"].ToString() + "</td>";
                                            origen = dr["num_factura"].ToString();
                                            tabla += "<td>" + dr["observacion"].ToString() + "</td>";
                                            tabla += "<td>" + dr["fecha_trans"].ToString() + "</td>";
                                            if (dr["tipo_doc"].ToString() == "PA-F")
                                            {
                                                tabla += "<td></td>";
                                            }
                                            else
                                            {
                                                tabla += "<td>" + dr["fecha_venc"].ToString() + "</td>";
                                            }
                                            tabla += "<td>" + dr["monto_doc"].ToString() + "</td>";
                                            tabla += "</tr>";
                                            //}
                                            //}
                                        }
                                        else if (!IsNumeric2(dr["num_factura"].ToString().Trim()))
                                        {
                                            if (dr["tipo_doc"].ToString() == "DM")
                                            {
                                                if (che.Count == 0)
                                                {
                                                    swo = false;

                                                }
                                                else
                                                {

                                                    foreach (string s in che)
                                                    {
                                                        if (s == dr["num_factura"].ToString())
                                                        {
                                                            swo = true;
                                                            break;

                                                        }
                                                        else
                                                        {

                                                            swo = false;
                                                        }

                                                    }

                                                }
                                            }
                                            if (!swo)
                                            {
                                                che.Add(dr["num_factura"].ToString().Trim());

                                                //ACA BORRAR PAGO EN CERO
                                                //string script22 = string.Format("eliminar_pago_en_pag(&#39;{0}&#39;);eliminar_pago_en_s(&#39;{1}&#39;,&#39;{2}&#39;,&#39;{3}&#39;,&#39;{4}&#39;,&#39;{5}&#39;);", dr["num_factura"].ToString().Trim(), dr["id"].ToString().Trim(), iidd.Trim(), rut, factura, tipo_doc);
                                                string script22 = string.Format("eliminar_pago_en_pag(&#39;{0}&#39;, &#39;{1}&#39;, &#39;{8}&#39;);" +
                                                                 "   eliminar_pago_en_s(&#39;{2}&#39;,&#39;{3}&#39;,&#39;{4}&#39;,&#39;{5}&#39;,&#39;{6}&#39;, &#39;{7}&#39;);",
                                                                    dr["num_factura"].ToString().Trim(), dr["fecha_trans"].ToString().Trim(),
                                                                    dr["id"].ToString().Trim(), iidd.Trim(), rut, factura, tipo_doc, dr["fecha_trans"].ToString().Trim()
                                                                    , dr["observacion"].ToString());

                                                tabla += "<tr>";
                                                //< a id = "ContentPlaceHolder_Contenido_btn_excel" class="btn btn-circle show-tooltip fa fa-file-text-o" title="Exportar a Excel" href="javascript:__doPostBack('ctl00$ContentPlaceHolder_Contenido$btn_excel','')" style="margin-left: 5px"></a>
                                                tabla += "<td><a style='background-color: rgb(255, 97, 97);' class=\"btn btn-circle show-tooltip fa fa-trash\" onclick=\"" + script22 + "\"></a></td>";
                                                tabla += "<td>" + dr["num_factura"].ToString() + "</td>";
                                                tabla += "<td>" + dr["tipo_doc"].ToString() + "</td>";
                                                origen = dr["num_factura"].ToString();
                                                tabla += "<td>" + dr["observacion"].ToString() + "</td>";
                                                tabla += "<td>" + dr["fecha_trans"].ToString() + "</td>";
                                                if (dr["tipo_doc"].ToString() == "PA-F")
                                                {
                                                    tabla += "<td></td>";
                                                }
                                                else
                                                {
                                                    tabla += "<td>" + dr["fecha_venc"].ToString() + "</td>";
                                                }
                                                tabla += "<td>" + dr["monto_doc"].ToString() + "</td>";
                                                tabla += "</tr>";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        tabla += "</tbody>";
                        tabla += "</table>";
                    }
                    else
                    {

                        string rutcliente_ = "", num_factura_ = "";

                        if (origen == "")
                        {
                            tabla += "<h3>No existen origen para el documento</h3>";
                        }
                        else
                        {
                            dt4 = db.consultar("SELECT rutcliente, num_factura  FROM COBRANZA_SEGUIMIENTO WHERE ID = " + origen);
                            foreach (DataRow dr in dt4.Rows)
                            {
                                rutcliente_ = dr["rutcliente"].ToString();
                                num_factura_ = dr["num_factura"].ToString();
                            }
                            dt3 = db.consultar("SELECT CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, CONVERT(varchar(20), fecha, 103) as fecha_trans, '$ ' + dbo.F_Separador_miles(CONVERT(numeric, a.monto_doc)) as monto_doc, observacion, num_factura, tipo_doc  FROM COBRANZA_SEGUIMIENTO WHERE RUTCLIENTE = '" + rutcliente_ + "' AND NUM_FACTURA = '" + num_factura_ + "' ORDER BY fecha_trans desc");
                            tabla += "<h3>Origen del pago</h3>";

                            tabla += "<table class=\"table fill-head table-bordered\">";
                            tabla += "<thead class=\"test\">";
                            tabla += "<tr>";
                            tabla += "<th>Num Factura</th>";
                            tabla += "<th>Tipo Doc.</th>";
                            tabla += "<th>Descripcion</th>";
                            tabla += "<th>Fecha</th>";
                            tabla += "<th>Vencimiento</th>";
                            tabla += "<th>Monto Doc.</th>";
                            tabla += "</tr>";
                            tabla += "</thead>";
                            tabla += "<tbody>";
                            foreach (DataRow dr in dt3.Rows)
                            {
                                tabla += "<tr>";
                                tabla += "<td>" + dr["num_factura"].ToString() + "</td>";
                                tabla += "<td>" + dr["tipo_doc"].ToString() + "</td>";
                                tabla += "<td>" + dr["observacion"].ToString() + "</td>";
                                tabla += "<td>" + dr["fecha_trans"].ToString() + "</td>";
                                tabla += "<td>" + dr["fecha_venc"].ToString() + "</td>";
                                tabla += "<td>" + dr["monto_doc"].ToString() + "</td>";
                                tabla += "</tr>";
                            }
                            tabla += "</tbody>";
                            tabla += "</table>";
                        }
                    }
                }
            }

            tabla += "<h3>Acciones</h3>";
            DataTable dt1 = new DataTable();
            string id2 = factura;
            // CAMPOS
            if (!Regex.IsMatch(id.Trim(), @"^[0-9]+$"))
            {
                dt1 = db.consultar("select num_factura_origen from Cobranza_seguimiento where num_factura = '" + id2 + "' and num_factura_origen <> null");
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        id2 = dr["num_factura_origen"].ToString();
                    }
                    catch { }
                }
            }

            //aca se cae CUANDO ES CHEQUE PROTESTADO
            dt1 = db.consultar("select id_cobranza, (select top 1 nom_accion from acciones a where Cobranza_Acciones.id_accion = a.id_accion) as accion,  fecha_accion as fecha, usuario, obs, id from Cobranza_Acciones where rtrim(ltrim(ID_cobranza)) = '" + id2.Trim() + "'");

            tabla += "<table class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";
            tabla += "<th>Acción</th>";
            tabla += "<th>Num Factura</th>";
            tabla += "<th>Acción</th>";
            tabla += "<th>Fecha</th>";
            tabla += "<th>Usuario</th>";
            tabla += "<th>Descripción</th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            foreach (DataRow dr in dt1.Rows)
            {
                tabla += "<tr>";
                string es_el_elegido = ReporteRNegocio.accion_prioridad(dr["id"].ToString());
                if (es_el_elegido != "0")
                {

                    tabla += "<td>  <input type='radio' name='accion' checked='checked' value='male'></td>";

                }
                else { tabla += "<td>  <input type='radio' name='accion' onclick=\"rb_click_accion('" + dr["id"].ToString() + "', '" + dr["id_cobranza"].ToString() + "')\" value='male'></td>"; }


                tabla += "<td>" + dr["id_cobranza"].ToString() + "</td>";
                tabla += "<td>" + dr["accion"].ToString() + "</td>";
                tabla += "<td>" + dr["fecha"].ToString() + "</td>";
                tabla += "<td>" + dr["usuario"].ToString() + "</td>";
                tabla += "<td>" + dr["obs"].ToString() + "</td>";
                tabla += "</tr>";
            }
            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");


            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new Evento_objeto
                        {
                            id = Convert.ToString(item["factura"]).Replace("'", "").Trim(),
                            title = Convert.ToString(item["nombrecliente"]).Replace("'", ""),
                            start = Convert.ToString(item["fecha_trans"]).Replace("'", ""),
                            factura = click_factura(Convert.ToString(item["factura"]).Replace("'", "")),
                            rut_cliente = Convert.ToString(item["rutcliente"]).Replace("'", ""),
                            tabla_html = tabla,
                            estado = estado_result.ToString(),
                            ver_cliente = ver_cliente,
                            monto_doc = Convert.ToString(item["monto_doc"]).Replace("'", "")
                        };

            return query.ToList<Evento_objeto>();
        }






        [WebMethod]
        public static List<Evento_objeto> CargarEvento(string id, string rut, string factura, string tipo_doc)
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt4 = new DataTable();
            DataTable dt3 = new DataTable();
            DBUtil db = new DBUtil();

            string tipo_doc_aux = tipo_doc;
            if (tipo_doc == "temporal") { tipo_doc = "IN"; }
            string tabla = "";
            int estado_result = 0;
            string es_cheque_factura = "";
            // CAMPOS
            if (!Regex.IsMatch(factura.Trim(), @"^[0-9]+$"))
            {
                es_cheque_factura = factura;
                dt = db.consultar("SELECT id, nombrecliente, factura, rutcliente, CONVERT(varchar(20), fecha_trans, 103) as fecha_trans, CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, monto_doc, transfor FROM V_COBRANZA WHERE RUTCLIENTE = '" + rut + "' AND FACTURA = '" + factura + "' AND ID = " + id);
                try
                {
                    factura = (db.Scalar("select top 1 num_factura_origen from cobranza_seguimiento where num_factura = '" + factura + "' and rutcliente = '" + rut + "' and tipo_doc = '" + tipo_doc + "'").ToString());
                    if (es_cheque_factura != "") { tipo_doc = "IN"; }
                }
                catch { }
            }
            else
            {

                dt = db.consultar("SELECT id, nombrecliente, factura, rutcliente, CONVERT(varchar(20), fecha_trans, 103) as fecha_trans, CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, (select ISNULL( (select sum(monto) * -1 from Cobranza_pagos where id_cobranza  = '" + factura + "'),0) + monto_doc ) as monto_doc FROM V_COBRANZA WHERE RUTCLIENTE = '" + rut + "' AND FACTURA = '" + factura + "' AND ID = " + id);

            }


            estado_result = Convert.ToInt32(db.Scalar("select count(*) from cobranza_seguimiento where num_factura = '" + factura + "' and rutcliente = '" + rut + "' ").ToString());


            // TABLA MOV_ASOCIADOS A LA FACTURA
            if (es_cheque_factura != "")
            {
                dt2 = db.consultar("SELECT id, CONVERT(varchar(20), fecha_trans, 103) as fecha_trans, CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, '$ ' + convert(varchar,cast(monto_doc as money),1) as monto_doc, '$ ' + convert(varchar,cast(transfor as money),1) as transfor,descr, factura, tipo_doc, case when estado_doc = '0' then 'CERRADO' when estado_doc = '1' then 'ABIERTO' END AS estado, '$ ' + convert(varchar,cast(saldo as money),1)  as saldo , tipo_moneda , tasa_camb, '$ ' + convert(varchar,cast(round((saldo / tasa_camb),3) as money) ,1) AS saldolar   FROM V_COBRANZA WHERE RUTCLIENTE = '" + rut + "' AND FACTURA = '" + es_cheque_factura + "' ORDER BY fecha_trans desc");

            }
            else
            {
                dt2 = db.consultar("SELECT id, CONVERT(varchar(20), fecha_trans, 103) as fecha_trans, CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, '$ ' + convert(varchar,cast(monto_doc as money),1) as monto_doc, '$ ' + convert(varchar,cast(transfor as money),1) as transfor, descr, factura, tipo_doc, case when estado_doc = '0' then 'CERRADO' when estado_doc = '1' then 'ABIERTO' END AS estado, '$ ' + convert(varchar,cast(saldo as money),1)  as saldo , tipo_moneda, tasa_camb , '$ ' + convert(varchar,cast(round((saldo / tasa_camb),3) as money) ,1) AS saldolar  FROM V_COBRANZA WHERE RUTCLIENTE = '" + rut + "' AND FACTURA = '" + factura + "' ORDER BY fecha_trans desc");

            }


            tabla += "<table class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";
            tabla += "<th>Abrir/Cerrar</th>";
            tabla += "<th>Num Factura</th>";
            tabla += "<th>Tipo Doc.</th>";
            tabla += "<th>Descripcion</th>";
            tabla += "<th>Fecha</th>";
            tabla += "<th>Vencimiento</th>";
            tabla += "<th>Monto (Peso)</th>";
            tabla += "<th>Monto (Dolar)</th>";
            tabla += "<th>Saldo (Peso)</th>";
            tabla += "<th>Saldo (Dolar)</th>";
            tabla += "<th>Moneda </th>";
            tabla += "<th>Estado Solomon</th>";
            tabla += "<th>Estado App</th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            foreach (DataRow dr in dt2.Rows)
            {
                tabla += "<tr>";

                string script2 = string.Format("javascript:ESTADO_DOC(&#39;{0}&#39;,&#39;{1}&#39;,&#39;{2}&#39;)", dr["factura"].ToString().Trim(), dr["id"].ToString().Trim(), rut.Trim());

                tabla += "<td> <input type='button' class='btn-red' onclick='" + script2 + "' /> </td>";
                tabla += "<td>" + dr["factura"].ToString() + "</td>";
                tabla += "<td>" + dr["tipo_doc"].ToString() + "</td>";
                tabla += "<td>" + dr["descr"].ToString() + "</td>";
                tabla += "<td>" + dr["fecha_trans"].ToString() + "</td>";
                tabla += "<td>" + dr["fecha_venc"].ToString() + "</td>";
                bool es_cheque = false;
                try
                {
                    int result = int.Parse(dr["factura"].ToString().Trim());
                    tabla += "<td>" + dr["monto_doc"].ToString() + "</td>";
                }
                catch
                {
                    es_cheque = true;
                    tabla += "<td>" + dr["saldo"].ToString() + "</td>";
                }

                tabla += "<td>" + dr["transfor"].ToString() + "</td>";
                tabla += "<td>" + dr["saldo"].ToString() + "</td>";
                tabla += "<td>" + dr["saldolar"].ToString() + "</td>";
                tabla += "<td>" + dr["tipo_moneda"].ToString() + "</td>";
                tabla += "<td>" + dr["estado"].ToString() + "</td>";
                tabla += "<td>" + ReporteRNegocio.estado_app(dr["estado"].ToString(), dr["id"].ToString().Trim(), dr["factura"].ToString().Trim()) + "</td>";
                tabla += "</tr>";
                if (!Regex.IsMatch(factura.Trim(), @"^[0-9]+$"))
                {
                    try
                    {
                        factura = dr["num_factura_origen"].ToString();
                    }
                    catch { }
                }
                else
                {
                }
            }
            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");

            clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
            if (rut.Substring(0, 1) == "0")
            {
                rut = rut.Substring(1, rut.Length - 1);
            }
            var ver_cliente = "<a href='FICHA_CLIENTE.ASPX?R=" + encriptador.EncryptData(rut) + "&i=" + encriptador.EncryptData("88") + "'  target='_blank'>Ver Ficha</a>";


            // TABLA aca RECUPERAR NUM_FACTURA PARA CHEQUE
            string origen = "";
            if (estado_result >= 1)
            {
                if (tipo_doc_aux == "IN" && estado_result == 1)
                {

                }
                else
                {

                    if (tipo_doc == "IN" || tipo_doc == "DM")
                    {
                        dt3 = db.consultar("SELECT CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, CONVERT(varchar(20), fecha, 103) as fecha_trans, '$ ' + convert(varchar,cast(monto_doc as money),1) as monto_doc, observacion, num_factura, tipo_doc, num_factura_origen  FROM COBRANZA_SEGUIMIENTO WHERE NUM_FACTURA = '" + factura + "' ORDER BY fecha_trans desc");
                        tabla += "<h3>Documentos asociados por aplicación</h3>";

                        tabla += "<table class=\"table fill-head table-bordered\">";
                        tabla += "<thead class=\"test\">";
                        tabla += "<tr>";
                        tabla += "<th>Num Factura</th>";
                        tabla += "<th>Tipo Doc.</th>";
                        tabla += "<th>Descripcion</th>";
                        tabla += "<th>Fecha</th>";
                        tabla += "<th>Vencimiento</th>";
                        tabla += "<th>Monto Doc.</th>";
                        tabla += "</tr>";
                        tabla += "</thead>";
                        tabla += "<tbody>";
                        foreach (DataRow dr in dt3.Rows)
                        {
                            if (dr["tipo_doc"].ToString() != "IN")
                            {
                                tabla += "<tr>";
                                tabla += "<td>" + dr["num_factura"].ToString() + "</td>";
                                tabla += "<td>" + dr["tipo_doc"].ToString() + "</td>";
                                origen = dr["num_factura"].ToString();
                                tabla += "<td>" + dr["observacion"].ToString() + "</td>";
                                tabla += "<td>" + dr["fecha_trans"].ToString() + "</td>";
                                if (dr["tipo_doc"].ToString() == "PA-F")
                                {
                                    tabla += "<td></td>";
                                }
                                else
                                {
                                    tabla += "<td>" + dr["fecha_venc"].ToString() + "</td>";
                                }
                                tabla += "<td>" + dr["monto_doc"].ToString() + "</td>";
                                tabla += "</tr>";
                            }
                        }
                        tabla += "</tbody>";
                        tabla += "</table>";
                    }
                    else
                    {

                        string rutcliente_ = "", num_factura_ = "";

                        if (origen == "")
                        {
                            tabla += "<h3>No existen origen para el documento</h3>";
                        }
                        else
                        {
                            dt4 = db.consultar("SELECT rutcliente, num_factura  FROM COBRANZA_SEGUIMIENTO WHERE ID = " + origen);
                            foreach (DataRow dr in dt4.Rows)
                            {
                                rutcliente_ = dr["rutcliente"].ToString();
                                num_factura_ = dr["num_factura"].ToString();
                            }
                            dt3 = db.consultar("SELECT CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, CONVERT(varchar(20), fecha, 103) as fecha_trans, '$ ' + convert(varchar,cast(monto_doc as money),1) as monto_doc, observacion, num_factura, tipo_doc  FROM COBRANZA_SEGUIMIENTO WHERE RUTCLIENTE = '" + rutcliente_ + "' AND NUM_FACTURA = '" + num_factura_ + "' ORDER BY fecha_trans desc");
                            tabla += "<h3>Origen del pago</h3>";

                            tabla += "<table class=\"table fill-head table-bordered\">";
                            tabla += "<thead class=\"test\">";
                            tabla += "<tr>";
                            tabla += "<th>Num Factura</th>";
                            tabla += "<th>Tipo Doc.</th>";
                            tabla += "<th>Descripcion</th>";
                            tabla += "<th>Fecha</th>";
                            tabla += "<th>Vencimiento</th>";
                            tabla += "<th>Monto Doc.</th>";
                            tabla += "</tr>";
                            tabla += "</thead>";
                            tabla += "<tbody>";
                            foreach (DataRow dr in dt3.Rows)
                            {
                                tabla += "<tr>";
                                tabla += "<td>" + dr["num_factura"].ToString() + "</td>";
                                tabla += "<td>" + dr["tipo_doc"].ToString() + "</td>";
                                tabla += "<td>" + dr["observacion"].ToString() + "</td>";
                                tabla += "<td>" + dr["fecha_trans"].ToString() + "</td>";
                                tabla += "<td>" + dr["fecha_venc"].ToString() + "</td>";
                                tabla += "<td>" + dr["monto_doc"].ToString() + "</td>";
                                tabla += "</tr>";
                            }
                            tabla += "</tbody>";
                            tabla += "</table>";
                        }
                    }
                }
            }


            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new Evento_objeto
                        {
                            id = Convert.ToString(item["factura"]).Replace("'", "").Trim(),
                            title = Convert.ToString(item["nombrecliente"]).Replace("'", ""),
                            start = Convert.ToString(item["fecha_trans"]).Replace("'", ""),
                            factura = click_factura(Convert.ToString(item["factura"]).Replace("'", "")),
                            rut_cliente = Convert.ToString(item["rutcliente"]).Replace("'", ""),
                            tabla_html = tabla,
                            estado = estado_result.ToString(),
                            ver_cliente = ver_cliente,
                            monto_doc = Convert.ToString(item["monto_doc"]).Replace("'", "")
                        };

            return query.ToList<Evento_objeto>();
        }

        [WebMethod]
        public static string comision(string id, string rut, string factura, string tipo_doc, string saldo_peso)
        {


            string es_vi = ReporteRNegocio.es_venta_vi(factura);
            string es_ewos = ReporteRNegocio.es_ewos(factura);
            DataTable datos_importacion = ReporteRNegocio.trae_datos_importacion_comision(factura);
            string contrato = "";
            string tonelada_importad = "";
            string total_negocio = "";

            foreach (DataRow r in datos_importacion.Rows)
            {

                contrato = r[1].ToString();
                tonelada_importad = r[2].ToString().Replace(",00", "");
                total_negocio = r[3].ToString().Replace(",00", "");
            }



            string tabla = "  ";

            tabla += "  <h2 style='color: #ff9b00;'>  " + factura + " </h2>";

            /////   VI  ---------------------------------------------------------
            tabla += "  <br> <h3>Clientes Ventas 'VI'</h3> <br> ";

            tabla += " <div style='background:#c8ffc7; color:black;width: 6%; padding:10px;'><h3></h3> ";
            tabla += "       <table>";
            tabla += " <tr>";
            tabla += "          <td>";


            if (es_vi == "1")
            {
                tabla += "             <input type='checkbox' name='checkboxG1' onclick='click_es_vi(" + factura + ");' id='checkboxG1' checked class='css-checkbox' />";
            }
            else
            {
                tabla += "             <input type='checkbox' name='checkboxG1' onclick='click_es_vi(" + factura + ");' id='checkboxG1' class='css-checkbox' />";
            }
            tabla += "             <label for='checkboxG1' style='color:white;' class='css-label'></label>";

            tabla += "        </td>";

            tabla += "   <td>";
            tabla += "    </tr>";
            tabla += "    </table>";
            tabla += "      </div>";



            /////COMISION ---------------------------------------------------------
            tabla += "  <br> <h3> Datos Comisión Importaciones </h3> <br> ";
            tabla += "   <label id='neto_peso_comision'> " + Base.monto_format(saldo_peso) + " </label> <br> ";

            if (total_negocio != "")
            {
                double saldo_peso_2 = Convert.ToDouble(saldo_peso);
                double total_negocio_2 = Convert.ToDouble(total_negocio);

                double porcentaje = (saldo_peso_2 * 100) / total_negocio_2;

                tabla += "   <label id='porcentaje'> " + porcentaje + " </label> <br> ";


            }

            //tabla += "   <label id='neto_peso_comision'> " + Base.monto_format(saldo_peso) + " </label> <br> ";

            tabla += "<table class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";

            tabla += "<th>Nº de contrato</th>";
            tabla += "<th>Kilos importados </th>";
            tabla += "<th>Total Negocio</th>";

            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";

            tabla += "<tr>";

            tabla += "<td><input type='text' id='contrato' value='" + contrato + "'></input></td>";
            tabla += "<td><input type='text' id='toneladas' value='" + tonelada_importad + "'></input></td>";
            tabla += "<td><input type='text' id='negocio' value='" + total_negocio + "'></input></td>";

            tabla += "</tr>";

            tabla += "</tbody>";
            tabla += "</table>";
            tabla += "  <div id='div3' class='btn-success' style='width: 12% !important;float: right !important;cursor: pointer !important;' onclick='guardar_importacion(" + factura + ");'> ";
            tabla += "       <h6 style='margin-left: 9%;'>Guarda importación</h6> ";
            tabla += "  </div> ";




            ///EWOS  ---------------------------------------------------------
            tabla += "  <br> <h3> Cliente EWOS No asociado a importaciones </h3> <br> ";

            tabla += " <div style='background:#abe0ff; color:black;width: 6%; padding:10px;'><h3></h3> ";
            tabla += "       <table>";
            tabla += " <tr>";
            tabla += "          <td>";
            if (es_ewos == "1")
            {
                tabla += "             <input type='checkbox' name='checkboxG2' onclick='click_no_importacion(" + factura + ");' id='checkboxG2' checked class='css-checkbox' />";
            }
            else
            {
                tabla += "             <input type='checkbox' name='checkboxG2' onclick='click_no_importacion(" + factura + ");' id='checkboxG2' class='css-checkbox' />";

            }
            tabla += "             <label for='checkboxG2' style='color:white;' class='css-label'></label>";

            tabla += "        </td>";

            tabla += "   <td>";
            tabla += "    </tr>";
            tabla += "    </table>";
            tabla += "      </div>";


            //tabla = tabla.Replace("'", "");



            return tabla;
        }


        [WebMethod]
        public static string guarda_cliente_vi(string es_vi, string factura)
        {
            string ok = "";
            if (es_vi == "True")

            {
                ok = ReporteRNegocio.guarda_cliente_vi(factura);


            }
            else
            {

                ok = ReporteRNegocio.quita_cliente_vi(factura);


            }


            return ok;
        }


        [WebMethod]
        public static string guardar_importacion(string factura, string contrato, string toneladas, string negocio, string neto_peso)
        {
            string ok = "";
            string ok2 = "";
            if (contrato == "" && toneladas == "" && negocio == "")
            {

                ok = ReporteRNegocio.quitar_importacion(factura);

            }
            else
            {
                // ACA VALIDAR 1% IMPORTACION CORRESPONDE Y TONELADA +4.000 SE PAGA 0,1
                double tonelada = Convert.ToDouble(toneladas);
                double neto_peso_factura_comision = Convert.ToDouble(neto_peso.Replace(".", ""));
                double monto_importacion = Convert.ToDouble(negocio.Replace(".", ""));

                double porcentaje = (neto_peso_factura_comision * 100) / monto_importacion;


                if (tonelada >= 4000000)
                {
                    ok = ReporteRNegocio.guarda_importacion(factura, contrato, toneladas, negocio, 10);
                }
                else
                {
                    ok = ReporteRNegocio.guarda_importacion(factura, contrato, toneladas, negocio, 20);
                }
                if (porcentaje < 0.5)
                {

                    ok2 = ReporteRNegocio.update_importacion_valida(factura, 0);

                }
                else
                {
                    ok2 = ReporteRNegocio.update_importacion_valida(factura, 1);

                }



            }

            return ok;
        }


        [WebMethod]
        public static string guarda_ewos(string es_ewos, string factura)
        {
            string ok = "";
            if (es_ewos == "True")

            {
                ok = ReporteRNegocio.guarda_ewos(factura);


            }
            else
            {

                ok = ReporteRNegocio.quita_ewos(factura);


            }


            return ok;
        }



        private static string click_factura(string p)
        {
            clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
            try
            {
                string año_factura = ReporteRNegocio.trae_año_factura(p);
                año_factura = año_factura.Substring(0, 4);
                string script = string.Format("javascript:fuera3(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(p), encriptador.EncryptData(año_factura));
                return "  <a href='javascript:' onclick='" + script + "'>" + p + " </a>";
            }
            catch
            {
                return p;

            }

        }

        public class CLIENTE2
        {
            public string rutcliente { get; set; }
            public string nombrecliente { get; set; }
            public string direccion { get; set; }
            public string ciudad { get; set; }
            public string pais { get; set; }
            public string fono { get; set; }
            public string correo { get; set; }
            public string vendedor { get; set; }
            public string nom_vendedor { get; set; }
            public string LC { get; set; }
            public string LD { get; set; }
            public string tabla { get; set; }
            public CLIENTE2(string rut, string nombre, string dir, string ci, string pa, string fo, string co, string ve, string no, string lc, string ld, string tabla)
            {
                this.rutcliente = rut.Trim();
                this.nombrecliente = nombre.Trim();
                this.direccion = dir.Trim();
                this.ciudad = ci.Trim();
                this.pais = pa.Trim();
                this.fono = fo.Trim();
                this.correo = co.Trim();
                this.vendedor = ve.Trim();
                this.nom_vendedor = no.Trim();
                if (lc == "") { lc = "0"; }
                if (ld == "") { ld = "0"; }
                this.LC = lc.Trim();
                this.LD = ld.Trim();
                this.tabla = tabla;
            }
        }

        [WebMethod]
        public static string cambia_tipo_pago_(string factura, string estado)
        {
            string aasdf = "";
            ReporteRNegocio.delete_tipo_pago(factura);
            ReporteRNegocio.insert_tipo_pago(factura, estado);

            return "";
        }


        [WebMethod]
        public static string cambia_estado_doc(string id_factur, string id_2, string rutclient)
        {
            string tabla = "";
            DBUtil db = new DBUtil();

            string existe = db.Scalar("select count(*) from estado_documento  where id = '" + id_2 + "' and num_factura = '" + id_factur + "';").ToString();

            if (existe == "0")
            {
                string cambia_estado = ReporteRNegocio.cambia_estado_doc(id_factur, id_2);
            }
            else
            {

                string estado = db.Scalar("select estado from estado_documento  where id = '" + id_2 + "' and num_factura = '" + id_factur + "';").ToString();
                if (estado == "0") { string cupdate_estado = ReporteRNegocio.update_estado_doc(id_factur, id_2, 0); }
                else { string cupdate_estado = ReporteRNegocio.update_estado_doc(id_factur, id_2, 1); }


            }


            // TABLA

            DataTable dt2 = db.consultar("SELECT id, CONVERT(varchar(20), fecha_trans, 103) as fecha_trans, CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, '$ ' + convert(varchar,cast(monto_doc as money),1) as monto_doc, '$ ' + convert(varchar,cast(transfor as money),1) as transfor, descr, factura, tipo_doc,  case when estado_doc = '0' then 'CERRADO' when estado_doc = '1' then 'ABIERTO' END AS estado, '$ ' + convert(varchar,cast(saldo as money),1)  as saldo, tipo_moneda    FROM V_COBRANZA WHERE RUTCLIENTE = '" + rutclient + "' AND FACTURA = '" + id_factur + "' ORDER BY fecha_trans desc");

            tabla += "<table class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";
            tabla += "<th>Abierto/Cerrado</th>";
            tabla += "<th>Num Factura</th>";
            tabla += "<th>Tipo Doc.</th>";
            tabla += "<th>Descripcion</th>";
            tabla += "<th>Fecha</th>";
            tabla += "<th>Vencimiento</th>";
            tabla += "<th>Monto (Peso)</th>";
            tabla += "<th>Saldo </th>";
            tabla += "<th>Moneda </th>";
            tabla += "<th>Monto (Dolar)</th>";
            tabla += "<th>Estado Solomon</th>";
            tabla += "<th>Estado App</th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            string factura = "";

            string tipo_doc = "";
            foreach (DataRow dr in dt2.Rows)
            {
                tabla += "<tr>";

                string script2 = string.Format("javascript:ESTADO_DOC(&#39;{0}&#39;,&#39;{1}&#39;,&#39;{2}&#39;)", dr["factura"].ToString().Trim(), dr["id"].ToString().Trim(), rutclient.Trim());

                tabla += "<td> <input type='button' class='btn-red' onclick='" + script2 + "' /> </td>";
                tabla += "<td>" + dr["factura"].ToString() + "</td>";
                tabla += "<td>" + dr["tipo_doc"].ToString() + "</td>";
                tipo_doc = dr["tipo_doc"].ToString();
                factura = dr["factura"].ToString();
                tabla += "<td>" + dr["descr"].ToString() + "</td>";
                tabla += "<td>" + dr["fecha_trans"].ToString() + "</td>";
                tabla += "<td>" + dr["fecha_venc"].ToString() + "</td>";
                tabla += "<td>" + dr["monto_doc"].ToString() + "</td>";
                tabla += "<td>" + dr["transfor"].ToString() + "</td>";
                tabla += "<td>" + dr["saldo"].ToString() + "</td>";
                tabla += "<td>" + dr["tipo_moneda"].ToString() + "</td>";
                tabla += "<td>" + dr["estado"].ToString() + "</td>";
                tabla += "<td>" + ReporteRNegocio.estado_app(dr["estado"].ToString(), dr["id"].ToString().Trim(), dr["factura"].ToString().Trim()) + "</td>";


                tabla += "</tr>";
            }
            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");
            string es_cheque_factura = "";
            int estado_result = Convert.ToInt32(db.Scalar("select count(*) from cobranza_seguimiento where num_factura = '" + factura + "' and tipo_doc = '" + tipo_doc + "'").ToString());
            if (!Regex.IsMatch(factura.Trim(), @"^[0-9]+$"))
            {
                es_cheque_factura = factura;
                try
                {
                    factura = (db.Scalar("select top 1 num_factura_origen from cobranza_seguimiento where num_factura = '" + factura + "' and rutcliente = '" + rutclient + "' and tipo_doc = '" + tipo_doc + "'").ToString());
                    if (es_cheque_factura != "") { tipo_doc = "IN"; }
                }
                catch { }
            }

            DataTable dt3 = new DataTable();
            string origen = "";
            if (estado_result >= 1)
            {

                if (tipo_doc == "IN" || tipo_doc == "DM")
                {
                    if (es_cheque_factura != "")
                    {
                        dt3 = db.consultar("SELECT CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, CONVERT(varchar(20), fecha, 103) as fecha_trans, '$ ' + convert(varchar,cast(monto_doc as money),1) as monto_doc, observacion, num_factura, tipo_doc  FROM COBRANZA_SEGUIMIENTO WHERE NUM_FACTURA= '" + factura + "' ORDER BY fecha_trans desc");

                    }
                    else
                    {
                        dt3 = db.consultar("SELECT CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, CONVERT(varchar(20), fecha, 103) as fecha_trans, '$ ' + convert(varchar,cast(monto_doc as money),1) as monto_doc, observacion, num_factura, tipo_doc  FROM COBRANZA_SEGUIMIENTO WHERE NUM_FACTURA_origen = '" + factura + "' ORDER BY fecha_trans desc");

                    }


                    tabla += "<h3>Documentos asociados por aplicación</h3>";

                    tabla += "<table class=\"table fill-head table-bordered\">";
                    tabla += "<thead class=\"test\">";
                    tabla += "<tr>";
                    tabla += "<th>Num Factura</th>";
                    tabla += "<th>Tipo Doc.</th>";
                    tabla += "<th>Descripcion</th>";
                    tabla += "<th>Fecha</th>";
                    tabla += "<th>Vencimiento</th>";
                    tabla += "<th>Monto Doc.</th>";
                    tabla += "</tr>";
                    tabla += "</thead>";
                    tabla += "<tbody>";
                    foreach (DataRow dr in dt3.Rows)
                    {

                        tabla += "<tr>";
                        tabla += "<td>" + dr["num_factura"].ToString() + "</td>";
                        tabla += "<td>" + dr["tipo_doc"].ToString() + "</td>";
                        origen = dr["num_factura"].ToString();
                        tabla += "<td>" + dr["observacion"].ToString() + "</td>";
                        tabla += "<td>" + dr["fecha_trans"].ToString() + "</td>";
                        tabla += "<td>" + dr["fecha_venc"].ToString() + "</td>";
                        tabla += "<td>" + dr["monto_doc"].ToString() + "</td>";
                        tabla += "</tr>";

                    }
                    tabla += "</tbody>";
                    tabla += "</table>";
                    if (estado_result == 1 && dt3.Rows.Count == 0)
                    {
                        tabla = tabla.Substring(0, tabla.IndexOf("<h3>Documentos asociados por aplicación"));
                    }
                }
                else
                {

                    string rutcliente_ = "", num_factura_ = "";

                    if (origen == "")
                    {
                        tabla += "<h3>No existen origen para el documento</h3>";
                    }
                    else
                    {
                        DataTable dt4 = db.consultar("SELECT rutcliente, num_factura  FROM COBRANZA_SEGUIMIENTO WHERE ID = " + origen);
                        foreach (DataRow dr in dt4.Rows)
                        {
                            rutcliente_ = dr["rutcliente"].ToString();
                            num_factura_ = dr["num_factura"].ToString();
                        }
                        dt3 = db.consultar("SELECT CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, CONVERT(varchar(20), fecha, 103) as fecha_trans, '$ ' + convert(varchar,cast(monto_doc as money),1) as monto_doc, observacion, num_factura, tipo_doc  FROM COBRANZA_SEGUIMIENTO WHERE RUTCLIENTE = '" + rutcliente_ + "' AND NUM_FACTURA = '" + num_factura_ + "' ORDER BY fecha_trans desc");
                        tabla += "<h3>Origen del pago</h3>";

                        tabla += "<table class=\"table fill-head table-bordered\">";
                        tabla += "<thead class=\"test\">";
                        tabla += "<tr>";
                        tabla += "<th>Num Factura</th>";
                        tabla += "<th>Tipo Doc.</th>";
                        tabla += "<th>Descripcion</th>";
                        tabla += "<th>Fecha</th>";
                        tabla += "<th>Vencimiento</th>";
                        tabla += "<th>Monto Doc.</th>";
                        tabla += "</tr>";
                        tabla += "</thead>";
                        tabla += "<tbody>";
                        foreach (DataRow dr in dt3.Rows)
                        {
                            tabla += "<tr>";
                            tabla += "<td>" + dr["num_factura"].ToString() + "</td>";
                            tabla += "<td>" + dr["tipo_doc"].ToString() + "</td>";
                            tabla += "<td>" + dr["observacion"].ToString() + "</td>";
                            tabla += "<td>" + dr["fecha_trans"].ToString() + "</td>";
                            tabla += "<td>" + dr["fecha_venc"].ToString() + "</td>";
                            tabla += "<td>" + dr["monto_doc"].ToString() + "</td>";
                            tabla += "</tr>";
                        }
                        tabla += "</tbody>";
                        tabla += "</table>";
                    }
                }
            }




            return tabla;
        }

        [WebMethod]
        public static List<CLIENTE2> DATOS_CLIENTE(string RUT)
        {
            DataTable dt_ = ReporteRNegocio.trae_contactos_cobranza((RUT.Replace("-", "").Replace(".", "")).Trim());

            if (dt_.Rows.Count > 0)
            {
                string tabla = "";

                tabla += "<h3>Contacto</h3>";

                tabla += "<table class=\"table fill-head table-bordered\">";
                tabla += "<thead class=\"test\">";
                tabla += "<tr>";
                tabla += "<th>Id</th>";
                tabla += "<th>Nombre</th>";
                tabla += "<th>Correo</th>";
                tabla += "<th>Número</th>";
                tabla += "<th>Descrip</th>";
                tabla += "</tr>";
                tabla += "</thead>";
                tabla += "<tbody>";
                foreach (DataRow dr in dt_.Rows)
                {
                    tabla += "<tr>";
                    tabla += "<td>" + dr["Id"].ToString() + "</td>";
                    tabla += "<td>" + dr["nombre"].ToString() + "</td>";
                    tabla += "<td>" + dr["correo"].ToString() + "</td>";
                    tabla += "<td>" + dr["Nº"].ToString() + "</td>";
                    tabla += "<td>" + dr["Descrip"].ToString() + "</td>";
                    tabla += "</tr>";
                }
                tabla += "</tbody>";
                tabla += "</table>";


                List<CLIENTE2> CLIENTE = new List<CLIENTE2>();
                CLIENTE.Add(new CLIENTE2("", "", "", "", "", "", "", "", "", "", "", tabla));
                return CLIENTE;
            }
            else
            {

                string rutcliente = RUT.Replace("-", "").Replace(".", "").Trim();
                string rut_ini = rutcliente.Trim().Substring(0, rutcliente.Trim().Length - 1);
                double rut = double.Parse(rut_ini);

                rutcliente = rut.ToString("N0") + "-" + rutcliente.Trim().Substring(rutcliente.Trim().Length - 1);

                string nombrecliente = "";
                string direccion = "";
                string ciudad = "";
                string pais = "";
                string fono = "";
                string correo = "";
                string vendedor = "";
                DataTable dato_clientes = ReporteRNegocio.datos_cliente(rutcliente.Trim().Replace("-", "").Replace(".", ""));
                string l_credi = ReporteRNegocio.linea_credito(RUT.Replace("-", "").Replace(".", ""));
                string lc = l_credi.Substring(0, l_credi.IndexOf("("));
                string ld = l_credi.Substring(l_credi.IndexOf(" ") + 1).Replace(" )", "");
                foreach (DataRow r in dato_clientes.Rows)
                {
                    nombrecliente = r[0].ToString();
                    direccion = r[2].ToString();
                    ciudad = r[3].ToString();
                    pais = r[4].ToString();
                    fono = r[5].ToString();
                    correo = r[9].ToString();
                    vendedor = r[7].ToString();
                }

                string nomb_vend = ReporteRNegocio.nombre_vendedor(vendedor);

                double d;
                double.TryParse(lc, out d);
                string aux = "";
                if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                lc = aux;
                double d2;
                double.TryParse(ld, out d2);
                string aux2 = "";
                if (d2 == 0) { aux2 = ""; } else { aux2 = d2.ToString("N0"); }
                ld = aux2;
                l_credi = lc + "(D: " + ld + " )";

                List<CLIENTE2> CLIENTE = new List<CLIENTE2>();
                CLIENTE.Add(new CLIENTE2(rutcliente.Trim(), nombrecliente.Trim(), direccion.Trim(), ciudad.Trim(), pais.Trim(), fono.Trim(), correo.Trim(), vendedor.Trim(), nomb_vend.Trim(), lc.Trim(), ld.Trim(), ""));
                return CLIENTE;
            }
        }


        [WebMethod]
        public static List<Evento_objeto> CargarEvento2(string id)
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            DBUtil db = new DBUtil();
            string rut = "", tabla = "", tipo_doc = "", origen = "";

            // CAMPOS

            if (!Regex.IsMatch(id.Trim(), @"^[0-9]+$"))
            {
                dt = db.consultar("SELECT cs.id, (select top 1 b.nombrecliente from cobranza b where b.rutcliente = cs.rutcliente)  as nombrecliente, cs.tipo_doc, cs.num_factura, cs.num_factura_origen, cs.rutcliente, CONVERT(varchar(20), cs.fecha, 103) as fecha_trans, CONVERT(varchar(20), cs.fecha_venc, 103) as fecha_venc, monto_doc FROM cobranza_seguimiento cs WHERE NUM_FACTURA LIKE '%" + id + "%'");

            }
            else
            {
                dt = db.consultar("SELECT cs.id, (select top 1 b.nombrecliente from cobranza b where b.rutcliente = cs.rutcliente)  as nombrecliente, cs.tipo_doc, cs.num_factura, cs.num_factura_origen, cs.rutcliente, CONVERT(varchar(20), cs.fecha, 103) as fecha_trans, CONVERT(varchar(20), cs.fecha_venc, 103) as fecha_venc, (select ISNULL( (select sum(monto) * -1 from Cobranza_pagos where id_cobranza  = '" + id + "'),0) + monto_doc ) as monto_doc FROM cobranza_seguimiento cs WHERE NUM_FACTURA LIKE '%" + id + "%'");

            }

            string num_factura = "";

            foreach (DataRow dr in dt.Rows)
            {
                tipo_doc = dr["tipo_doc"].ToString();
                origen = dr["num_factura_origen"].ToString();
                num_factura = id;
                dt2 = db.consultar("SELECT id, CONVERT(varchar(20), fecha_trans, 103) as fecha_trans, CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, '$ ' + convert(varchar,cast(monto_doc as money),1) as monto_doc, '$ ' + convert(varchar,cast(transfor as money),1) as transfor,descr, factura, tipo_doc,  case when estado_doc = '0' then 'CERRADO' when estado_doc = '1' then 'ABIERTO' END AS estado, '$ ' + convert(varchar,cast(saldo as money),1)  as saldo, tipo_moneda     FROM V_COBRANZA WHERE FACTURA = '" + num_factura + "' ORDER BY fecha_trans desc");
                rut = dr["rutcliente"].ToString();
            }

            int estado_result = 0;
            estado_result = Convert.ToInt32(db.Scalar("select count(*) from cobranza_seguimiento where num_factura = '" + num_factura + "' and rutcliente = '" + rut + "' and tipo_doc = '" + tipo_doc + "'").ToString());


            // TABLA MOV_ASOCIADOS A LA FACTURA
            tabla += "<table class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";
            tabla += "<th> &nbsp;&nbsp;</th>";
            tabla += "<th>Num Factura</th>";
            tabla += "<th>Tipo Doc.</th>";
            tabla += "<th>Descripcion</th>";
            tabla += "<th>Fecha</th>";
            tabla += "<th>Vencimiento</th>";
            tabla += "<th>Monto (Peso)</th>";
            tabla += "<th>Monto (Dolar)</th>";
            tabla += "<th>Saldo </th>";
            tabla += "<th>Moneda </th>";
            tabla += "<th>Estado Solomon</th>";
            tabla += "<th>Estado App</th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            foreach (DataRow dr in dt2.Rows)
            {
                tabla += "<tr>";

                string script2 = string.Format("javascript:ESTADO_DOC(&#39;{0}&#39;,&#39;{1}&#39;,&#39;{2}&#39;)", dr["factura"].ToString().Trim(), dr["id"].ToString().Trim(), rut.Trim());

                tabla += "<td> <input type='button' class='btn-red' onclick='" + script2 + "' /> </td>";
                tabla += "<td>" + dr["factura"].ToString() + "</td>";
                tabla += "<td>" + dr["tipo_doc"].ToString() + "</td>";
                tabla += "<td>" + dr["descr"].ToString() + "</td>";
                tabla += "<td>" + dr["fecha_trans"].ToString() + "</td>";
                tabla += "<td>" + dr["fecha_venc"].ToString() + "</td>";
                tabla += "<td>" + dr["monto_doc"].ToString() + "</td>";
                tabla += "<td>" + dr["transfor"].ToString() + "</td>";
                tabla += "<td>" + dr["saldo"].ToString() + "</td>";
                tabla += "<td>" + dr["tipo_moneda"].ToString() + "</td>";
                tabla += "<td>" + dr["estado"].ToString() + "</td>";
                tabla += "<td>" + ReporteRNegocio.estado_app(dr["estado"].ToString(), dr["id"].ToString().Trim(), dr["factura"].ToString().Trim()) + "</td>";
                tabla += "</tr>";
            }
            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");



            dt3 = db.consultar("SELECT CONVERT(varchar(20), fecha_venc, 103) as fecha_venc, CONVERT(varchar(20), fecha, 103) as fecha_trans, '$ ' + convert(varchar,cast(monto_doc as money),1) as monto_doc, observacion, num_factura, tipo_doc  FROM COBRANZA_SEGUIMIENTO WHERE NUM_FACTURA_ORIGEN = '" + num_factura + "' ORDER BY fecha_trans desc");
            tabla += "<h3>Pagos asociados a la factura</h3>";

            tabla += "<table class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";
            tabla += "<th>Num Factura</th>";
            tabla += "<th>Tipo Doc.</th>";
            tabla += "<th>Descripcion</th>";
            tabla += "<th>Fecha</th>";
            tabla += "<th>Vencimiento</th>";
            tabla += "<th>Monto Doc.</th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            foreach (DataRow dr in dt3.Rows)
            {
                tabla += "<tr>";
                tabla += "<td>" + dr["num_factura"].ToString() + "</td>";
                tabla += "<td>" + dr["tipo_doc"].ToString() + "</td>";
                tabla += "<td>" + dr["observacion"].ToString() + "</td>";
                tabla += "<td>" + dr["fecha_trans"].ToString() + "</td>";
                tabla += "<td>" + dr["fecha_venc"].ToString() + "</td>";
                tabla += "<td>" + dr["monto_doc"].ToString() + "</td>";
                tabla += "</tr>";
            }
            tabla += "</tbody>";
            tabla += "</table>";

            tabla = tabla.Replace("'", "");

            clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
            if (rut.Substring(0, 1) == "0")
            {
                rut = rut.Substring(1, rut.Length - 1);
            }
            var ver_cliente = "<a href='FICHA_CLIENTE.ASPX?R=" + encriptador.EncryptData(rut) + "&i=" + encriptador.EncryptData("88") + "'  target='_blank'>Ver Ficha</a>";


            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new Evento_objeto
                        {
                            id = Convert.ToString(item["num_factura"]).Replace("'", ""),
                            title = Convert.ToString(item["nombrecliente"]).Replace("'", ""),
                            start = Convert.ToString(item["fecha_trans"]).Replace("'", ""),
                            factura = Convert.ToString(item["num_factura"]).Replace("'", ""),
                            rut_cliente = Convert.ToString(item["rutcliente"]).Replace("'", ""),
                            tabla_html = tabla,
                            ver_cliente = ver_cliente,
                            monto_doc = Convert.ToString(item["monto_doc"]).Replace("'", "")

                        };

            return query.ToList<Evento_objeto>();
        }

        [WebMethod]
        public static List<Evento_objeto> CargarEvento3(string rut_cliente, string start, string tipo_doc)
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DBUtil db = new DBUtil();
            string rut = "", tabla = "";
            rut = rut_cliente;

            start = start.Substring(0, 10);

            // CAMPOS
            dt = db.consultar("SELECT top 1 nombrecliente, factura FROM V_COBRANZA WHERE rutcliente = '" + rut_cliente + "' ");

            foreach (DataRow dr in dt.Rows)
            {
                if (tipo_doc == "IN")
                {
                    dt2 = db.consultar("SELECT factura as num_factura, tipo_doc, descr as observacion, CONVERT(datetime, fecha_trans, 103) as fecha_trans, CONVERT(datetime, fecha_venc, 103) as fecha_venc, '$ ' + convert(varchar,cast(monto_doc as money),1) as monto_doc, '$ ' + convert(varchar,cast(transfor as money),1) as transfor FROM V_COBRANZA WHERE RUTCLIENTE = '" + rut_cliente + "' AND fecha_trans = CONVERT(datetime, '" + start + "', 111) and tipo_doc = 'IN' ORDER BY fecha_trans desc");
                }
                else if (tipo_doc == "DM")
                {
                    dt2 = db.consultar("SELECT factura as num_factura, tipo_doc, descr as observacion, CONVERT(datetime, fecha_trans, 103) as fecha_trans, CONVERT(datetime, fecha_venc, 103) as fecha_venc, '$ ' + convert(varchar,cast(monto_doc as money),1) as monto_doc, '$ ' + convert(varchar,cast(transfor as money),1) as transfor  FROM V_COBRANZA WHERE RUTCLIENTE = '" + rut_cliente + "' AND fecha_venc = CONVERT(datetime, '" + start + "', 111) and tipo_doc = 'DM' ORDER BY fecha_venc desc");
                }
            }

            // TABLA

            tabla += "<table class=\"table fill-head table-bordered\">";
            tabla += "<thead class=\"test\">";
            tabla += "<tr>";
            tabla += "<th>Num Factura</th>";
            tabla += "<th>Tipo Doc.</th>";
            tabla += "<th>Descripcion</th>";
            tabla += "<th>Fecha</th>";
            tabla += "<th>Vencimiento</th>";
            tabla += "<th>Monto (Peso)</th>";
            tabla += "<th>Monto (Dolar)</th>";
            tabla += "<th>Saldo </th>";
            tabla += "<th>Moneda </th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            bool una = false;
            string estado_result = "";
            string factura2 = "";
            string factura_1 = "";
            foreach (DataRow dr in dt2.Rows)
            {
                tabla += "<tr>";
                tabla += "<td>" + dr["num_factura"].ToString() + "</td>";
                tabla += "<td>" + dr["tipo_doc"].ToString() + "</td>";
                tabla += "<td>" + dr["observacion"].ToString() + "</td>";
                tabla += "<td>" + dr["fecha_trans"].ToString() + "</td>";
                tabla += "<td>" + dr["fecha_venc"].ToString() + "</td>";
                tabla += "<td>" + dr["monto_doc"].ToString() + "</td>";
                tabla += "<td>" + dr["transfor"].ToString() + "</td>";
                tabla += "<td>" + dr["saldo"].ToString() + "</td>";
                tabla += "<td>" + dr["tipo_moneda"].ToString() + "</td>";
                tabla += "</tr>";
                if (!una)
                {
                    estado_result = db.Scalar("select count(*) from cobranza_seguimiento where num_factura = '" + dr["num_factura"].ToString() + "' and rutcliente = '" + rut + "' and tipo_doc = '" + tipo_doc + "'").ToString();
                    una = true;
                    if (!Regex.IsMatch(dr["num_factura"].ToString().Trim(), @"^[0-9]+$"))
                    {
                        factura2 = dr["num_factura"].ToString();
                    }
                    else
                    {
                        factura2 = click_factura(dr["num_factura"].ToString());
                    }
                    factura_1 = dr["num_factura"].ToString();
                }
            }
            tabla += "</tbody>";
            tabla += "</table>";
            tabla = tabla.Replace("'", "");

            clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
            if (rut.Substring(0, 1) == "0")
            {
                rut = rut.Substring(1, rut.Length - 1);
            }
            var ver_cliente = "<a href='FICHA_CLIENTE.ASPX?R=" + encriptador.EncryptData(rut) + "&i=" + encriptador.EncryptData("88") + "'  target='_blank'>Ver Ficha</a>";


            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new Evento_objeto
                        {
                            title = Convert.ToString(item["nombrecliente"]).Replace("'", ""),
                            start = start,
                            rut_cliente = rut_cliente,
                            tabla_html = tabla,
                            ver_cliente = ver_cliente,
                            estado = estado_result,
                            factura = factura2,
                            id = factura_1
                        };

            return query.ToList<Evento_objeto>();
        }


        [WebMethod]
        public static string GuardarEvento(string id, string obs, string start)
        {
            try
            {
                DBUtil db = new DBUtil();
                DataTable dt = new DataTable();
                string query = "";

                query += "INSERT INTO COBRANZA_AGENDA ( ";
                query += "id_cobranza, ";
                query += "descripcion, ";
                query += "usuario, ";
                query += "fecha_trans ";
                query += ") VALUES ( ";
                query += "@id_cobranza, ";
                query += "@descripcion, ";
                query += "@usuario, ";
                query += "CONVERT(datetime, @fecha_trans, 103) ";
                query += ") ";

                List<SPVars> vars = new List<SPVars>();
                vars.Add(new SPVars() { nombre = "id_cobranza", valor = id });
                vars.Add(new SPVars() { nombre = "descripcion", valor = obs.Replace("'", "") });
                vars.Add(new SPVars() { nombre = "usuario", valor = HttpContext.Current.Session["user"].ToString() });
                vars.Add(new SPVars() { nombre = "fecha_trans", valor = start });

                db.Scalar2(query, vars);
                return "OK";
            }
            catch (Exception ex)
            {
                return "Error : " + ex.Message.ToString();
            }
        }



        [WebMethod]
        public static string GuardarEvento2(string id, string obs, string start)
        {

            List<string> facturas = id.Split('-').ToList();

            foreach (string fact in facturas)
            {


                try
                {
                    DBUtil db = new DBUtil();
                    DataTable dt = new DataTable();
                    string query = "";

                    query += "INSERT INTO COBRANZA_AGENDA ( ";
                    query += "id_cobranza, ";
                    query += "descripcion, ";
                    query += "usuario, ";
                    query += "fecha_trans ";
                    query += ") VALUES ( ";
                    query += "@id_cobranza, ";
                    query += "@descripcion, ";
                    query += "@usuario, ";
                    query += "CONVERT(datetime, @fecha_trans, 103) ";
                    query += ") ";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "id_cobranza", valor = fact });
                    vars.Add(new SPVars() { nombre = "descripcion", valor = "*" + start + "*  " + obs.Replace("'", "") });
                    vars.Add(new SPVars() { nombre = "usuario", valor = HttpContext.Current.Session["user"].ToString() });
                    vars.Add(new SPVars() { nombre = "fecha_trans", valor = start });

                    db.Scalar2(query, vars);

                }
                catch (Exception ex)
                {
                    return "Error : " + ex.Message.ToString();
                }
            }
            return "ok";
        }




        [WebMethod]
        public static string guardaraccion(string id_cobranza, string id_accion, string obs)
        {
            try
            {

                if (Convert.ToInt32(id_accion) >= 5)
                {
                    id_accion = (Convert.ToInt32(id_accion) + 1).ToString();
                }

                DBUtil db = new DBUtil();
                DataTable dt = new DataTable();
                string query = "";

                query += "INSERT INTO COBRANZA_ACCIONES VALUES ( ";
                query += "@id_cobranza, ";
                query += "@id_ACCION, ";
                query += "CONVERT(datetime, @_fecha_accion, 103), ";
                query += "@usuario, ";
                query += "@obs ";
                query += ") ;select scope_identity();";

                List<SPVars> vars = new List<SPVars>();
                vars.Add(new SPVars() { nombre = "id_cobranza", valor = id_cobranza });
                vars.Add(new SPVars() { nombre = "id_accion", valor = id_accion });
                vars.Add(new SPVars() { nombre = "usuario", valor = HttpContext.Current.Session["user"].ToString() });
                vars.Add(new SPVars() { nombre = "_fecha_accion", valor = DateTime.Now.ToString() });
                vars.Add(new SPVars() { nombre = "obs", valor = obs });

                string id = db.Scalar2(query, vars).ToString();
                accion_prioridad(id, id_cobranza);
                return "OK";
            }
            catch (Exception ex)
            {
                return "Error : " + ex.Message.ToString();
            }
        }

        [WebMethod]
        public static string guardaraccion2(string id_cobranza, string id_accion, string obs)
        {
            try
            {

                if (Convert.ToInt32(id_accion) >= 5)
                {
                    id_accion = (Convert.ToInt32(id_accion) + 1).ToString();
                }
                if (id_accion == "8") { id_accion = "7"; }
                List<string> facturas = id_cobranza.Split('-').ToList();

                foreach (string fact in facturas)
                {


                    DBUtil db = new DBUtil();
                    DataTable dt = new DataTable();
                    string query = "";

                    query += "INSERT INTO COBRANZA_ACCIONES VALUES ( ";
                    query += "@id_cobranza, ";
                    query += "@id_ACCION, ";
                    query += "CONVERT(datetime, GETDATE(), 103), ";
                    query += "@usuario, ";
                    query += "@obs ";
                    query += "); select scope_identity(); ";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "id_cobranza", valor = fact });
                    vars.Add(new SPVars() { nombre = "id_accion", valor = id_accion });
                    vars.Add(new SPVars() { nombre = "usuario", valor = HttpContext.Current.Session["user"].ToString() });
                    vars.Add(new SPVars() { nombre = "_fecha_accion", valor = DateTime.Now.ToString() });
                    vars.Add(new SPVars() { nombre = "obs", valor = obs });

                    string id = db.Scalar2(query, vars).ToString();

                    accion_prioridad(id, fact);
                    //devuelveme el ID (IDENTIFICADOR) Y id_cobranza

                }
            }
            catch (Exception ex)
            {
                return "Error : " + ex.Message.ToString();
            }
            return "";
        }


        [WebMethod]
        public static string guardaraccion3(string id_cobranza, string id_accion, string obs)
        {
            try
            {

                if (Convert.ToInt32(id_accion) >= 5)
                {
                    id_accion = (Convert.ToInt32(id_accion) + 1).ToString();
                }
                if (id_accion == "8") { id_accion = "7"; }
                List<string> facturas = id_cobranza.Split('-').ToList();

                foreach (string fact in facturas)
                {

                    if (IsNumeric2(fact))
                    {

                        DBUtil db = new DBUtil();
                        DataTable dt = new DataTable();
                        string query = "";

                        query += "INSERT INTO COBRANZA_ACCIONES VALUES ( ";
                        query += "@id_cobranza, ";
                        query += "@id_ACCION, ";
                        query += "CONVERT(datetime, GETDATE(), 103), ";
                        query += "@usuario, ";
                        query += "@obs ";
                        query += ") ;select scope_identity();";

                        List<SPVars> vars = new List<SPVars>();
                        vars.Add(new SPVars() { nombre = "id_cobranza", valor = fact });
                        vars.Add(new SPVars() { nombre = "id_accion", valor = id_accion });
                        vars.Add(new SPVars() { nombre = "usuario", valor = HttpContext.Current.Session["user"].ToString() });
                        vars.Add(new SPVars() { nombre = "_fecha_accion", valor = DateTime.Now.ToString() });
                        vars.Add(new SPVars() { nombre = "obs", valor = obs });

                        string id = db.Scalar2(query, vars).ToString();

                        accion_prioridad(id, fact);
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error : " + ex.Message.ToString();
            }
            return "";
        }

        private static bool IsNumeric2(string fact)
        {
            float output;
            return float.TryParse(fact, out output);
        }

        public class ACCION
        {
            public string id_accion { get; set; }
            public string nom_accion { get; set; }
        }
        [WebMethod]
        public static List<ACCION> carga_acciones()
        {
            DataTable dt = new DataTable();

            try
            {
                dt = ReporteRNegocio.listar_acciones();
                dt.Rows.Add(new Object[] { "-1", "-- Seleccione --" });
                DataView dv = dt.DefaultView;
                dv.Sort = "id_accion";
                dt = dv.ToTable();
            }
            catch { }


            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new ACCION
                        {
                            id_accion = Convert.ToString(item["id_accion"]),
                            nom_accion = Convert.ToString(item["nom_accion"])
                        };
            return query.ToList<ACCION>();
        }


        [WebMethod]
        public static string Registrar_Pago_efectivo(string id, string monto, string moneda, string tipo_doc, string descripcion, string cerrar)
        {
            id = id.Trim();
            if (!id.Contains("-"))
            {
                try
                {
                    string fecha_pago = "";
                    bool es_pa_f = false;
                    if (id.Contains("***"))
                    {
                        es_pa_f = true;
                        fecha_pago = id.Substring(id.IndexOf("***") + 3);
                        id = id.Substring(0, id.IndexOf("***"));
                        tipo_doc = "temporal";
                        //string q = ReporteRNegocio.delete_fecha_cobro(id);
                        string si_insert = ReporteRNegocio.insert_fecha_cobra(id, fecha_pago, "Estimado a pagar (aut.) (" + moneda + ")");
                        //string delete_seguimiento = ReporteRNegocio.delete_seguimiento_pa_f(id);
                    }
                    else
                    {
                        fecha_pago = DateTime.Now.ToShortDateString();
                    }

                    DBUtil db = new DBUtil();
                    DataTable dt = new DataTable();

                    dt = db.consultar("select top 1 num_factura, rutcliente, monto_doc from cobranza_seguimiento  where num_factura like '%" + id + "%'");

                    string query = "";

                    query += "INSERT INTO COBRANZA_PAGOS ( ";
                    query += "id_cobranza, ";
                    query += "tipo_doc, ";
                    query += "monto, ";
                    query += "fecha, ";
                    query += "moneda, ";
                    query += "descripcion ";
                    query += ") VALUES ( ";
                    query += "@id_cobranza, ";
                    query += "@tipo_doc, ";
                    query += "@monto, ";
                    query += "CONVERT(datetime, @fecha, 103), ";
                    query += "@moneda, ";
                    query += "@descripcion ";
                    query += ") ;";

                    query += "INSERT INTO COBRANZA_SEGUIMIENTO ( ";
                    query += "num_factura, ";
                    query += "monto_doc, ";
                    query += "rutcliente, ";
                    query += "estado, ";
                    query += "tipo_doc, ";
                    query += "observacion, ";
                    query += "usuario, ";
                    query += "num_factura_origen, ";
                    query += "fecha, ";
                    query += "fecha_venc, estado_ingresado ";
                    query += ") VALUES ( ";
                    query += "@_num_factura, ";
                    query += "@_monto_doc, ";
                    query += "@_rutcliente, ";
                    query += "@_estado, ";
                    query += "@_tipo_doc, ";
                    query += "@_observacion, ";
                    query += "@_usuario, ";
                    query += "@_num_factura_origen, ";
                    query += "CONVERT(datetime, @_fecha, 103), ";
                    query += "CONVERT(datetime, GETDATE(), 103), 0 ";
                    query += "); ";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "id_cobranza", valor = id });
                    vars.Add(new SPVars() { nombre = "tipo_doc", valor = tipo_doc.Replace("'", "") });
                    vars.Add(new SPVars() { nombre = "monto", valor = monto.Replace("'", "").Replace(",", ".") });
                    vars.Add(new SPVars() { nombre = "fecha", valor = fecha_pago });
                    vars.Add(new SPVars() { nombre = "moneda", valor = moneda.Replace("'", "") });
                    vars.Add(new SPVars() { nombre = "descripcion", valor = descripcion.Replace("'", "") });

                    foreach (DataRow dr in dt.Rows)
                    {
                        vars.Add(new SPVars() { nombre = "_num_factura", valor = dr["num_factura"].ToString().Replace("'", "") });
                        vars.Add(new SPVars() { nombre = "_monto_doc", valor = monto.Replace("'", "").Replace(",", ".") });
                        vars.Add(new SPVars() { nombre = "_rutcliente", valor = dr["rutcliente"].ToString().Replace("'", "") });

                        if (cerrar == "si")
                        {
                            vars.Add(new SPVars() { nombre = "_estado", valor = "PAGADO" });
                            vars.Add(new SPVars() { nombre = "_tipo_doc", valor = "PA" });
                            ReporteRNegocio.quitar_pa_f(id);
                        }
                        else
                        {
                            if (es_pa_f)
                            {
                                vars.Add(new SPVars() { nombre = "_estado", valor = "TEMPORAL" });
                                vars.Add(new SPVars() { nombre = "_tipo_doc", valor = "PA-F" });
                                descripcion = "Estimado a pagar (aut.) (" + moneda + ")";
                            }
                            else
                            {
                                vars.Add(new SPVars() { nombre = "_estado", valor = "ABONO" });
                                vars.Add(new SPVars() { nombre = "_tipo_doc", valor = "PA" });
                            }
                        }
                        vars.Add(new SPVars() { nombre = "_observacion", valor = descripcion.Replace("'", "") });
                        vars.Add(new SPVars() { nombre = "_usuario", valor = HttpContext.Current.Session["user"].ToString() });
                        vars.Add(new SPVars() { nombre = "_num_factura_origen", valor = id });
                        vars.Add(new SPVars() { nombre = "_fecha", valor = fecha_pago });
                        vars.Add(new SPVars() { nombre = "_fecha_venc", valor = DateTime.Now.ToShortDateString() });
                    }

                    db.Scalar2(query, vars);

                    return "OK";
                }
                catch (Exception ex)
                {
                    return "ERROR : " + ex.Message.ToString();
                }
            }
            else { return "ERROR : Permite solo 1 seleccionado"; }

        }
        [WebMethod]
        public static string Registrar_Pago_efectivo3(string id, string monto, string moneda, string tipo_doc, string descripcion, string cerrar, string fecha, string rut_cliente)
        {
            string ok_pago = "Pago Realizado !";
            try
            {
                //CB_DEPOSITOS_BANCO2.SelectedValue = "-1";
                double suma_deposit;
                string monto_aux = monto.Replace(".", ",");
                HttpContext.Current.Session["suma_deposit"] = double.Parse(monto_aux.Replace(".", ","));
                suma_deposit = Convert.ToDouble(HttpContext.Current.Session["suma_deposit"]);


                DBUtil db = new DBUtil();
                DataTable dt = new DataTable();

                //dt = db.consultar("select top 1 rutcliente from [dbo].[V_COBRANZA_TODOS] where RUTCLIENTE =  '" + rut_cliente.Trim() + "'");
                //select top 1 rutcliente from V_COBRANZA where factura = '172'

                string query = "";

                query += "INSERT INTO COBRANZA_PAGOS ( ";
                query += "id_cobranza, ";
                query += "tipo_doc, ";
                query += "monto, ";
                query += "fecha, ";
                query += "moneda, ";
                query += "descripcion ";
                query += ") VALUES ( ";
                query += "@id_cobranza, ";
                query += "@tipo_doc, ";
                query += "@monto, ";
                query += "CONVERT(datetime, @fecha, 103), ";
                query += "@moneda, ";
                query += "@descripcion ";
                query += ") ;";

                query += "INSERT INTO COBRANZA_SEGUIMIENTO ( ";
                query += "num_factura, ";
                query += "monto_doc, ";
                query += "rutcliente, ";
                query += "estado, ";
                query += "tipo_doc, ";
                query += "observacion, ";
                query += "usuario, ";
                query += "num_factura_origen, ";
                query += "fecha, ";
                query += "fecha_venc, estado_ingresado ";
                query += ") VALUES ( ";
                query += "@_num_factura, ";
                query += "@_monto_doc, ";
                query += "@_rutcliente, ";
                query += "@_estado, ";
                query += "@_tipo_doc, ";
                query += "@_observacion, ";
                query += "@_usuario, ";
                query += "@_num_factura_origen, ";
                query += "CONVERT(datetime, @_fecha, 103), ";
                query += "CONVERT(datetime, GETDATE(), 103), 0 ";
                query += "); ";

                List<SPVars> vars = new List<SPVars>();
                vars.Add(new SPVars() { nombre = "id_cobranza", valor = "0000000000" });
                vars.Add(new SPVars() { nombre = "tipo_doc", valor = tipo_doc.Replace("'", "") });
                vars.Add(new SPVars() { nombre = "monto", valor = monto.Replace("'", "").Replace(",", ".") });
                vars.Add(new SPVars() { nombre = "fecha", valor = fecha });
                vars.Add(new SPVars() { nombre = "moneda", valor = moneda.Replace("'", "") });
                vars.Add(new SPVars() { nombre = "descripcion", valor = descripcion.Replace("'", "") + "(" + moneda + ")" });


                vars.Add(new SPVars() { nombre = "_num_factura", valor = "0000000000" });
                vars.Add(new SPVars() { nombre = "_monto_doc", valor = monto.Replace("'", "").Replace(",", ".") });
                vars.Add(new SPVars() { nombre = "_rutcliente", valor = rut_cliente.Trim() });


                vars.Add(new SPVars() { nombre = "_estado", valor = "PAGO DIRECTO" });
                vars.Add(new SPVars() { nombre = "_tipo_doc", valor = "PA" });
                vars.Add(new SPVars() { nombre = "_observacion", valor = descripcion.Replace("'", "") + "(" + moneda + ")" });
                vars.Add(new SPVars() { nombre = "_usuario", valor = HttpContext.Current.Session["user"].ToString() });
                vars.Add(new SPVars() { nombre = "_num_factura_origen", valor = "0000000000" });
                vars.Add(new SPVars() { nombre = "_fecha", valor = fecha });
                vars.Add(new SPVars() { nombre = "_fecha_venc", valor = DateTime.Now.ToShortDateString() });

                db.Scalar2(query, vars);

            }
            catch (Exception ex)
            {
                string ERROR = ex.Message.ToString();
                ok_pago = "Falló el pago";
            }


            return ok_pago;
        }

        //public static double suma_deposit;
        [WebMethod]
        public static string Registrar_Pago_efectivo2(string id, string monto, string moneda, string tipo_doc, string descripcion, string cerrar, string fecha, string enviar_erp, string cuenta_banco)
        {
            string ok_pago = "Pago Realizado !";
            try
            {
                //CB_DEPOSITOS_BANCO2.SelectedValue = "-1";
                double suma_deposit;
                string monto_aux_DOUBLE = monto.Replace(".", ",");
                HttpContext.Current.Session["suma_deposit"] = Convert.ToDouble(monto_aux_DOUBLE.Trim());
                suma_deposit = Convert.ToDouble(HttpContext.Current.Session["suma_deposit"]);

                List<string> facturas = id.Split(new string[] { "--" }, StringSplitOptions.None).ToList();
                facturas.Sort();
                int cont = facturas.Count;
                int cont_x = 0;

                List<string> facturas_al_erp = new List<string>();

                int cont_pagables_2 = 0;
                foreach (string fact in facturas)
                {
                    string tipo_del_docu = ReporteRNegocio.tipo_doc(fact.Trim());
                    if (tipo_del_docu == "IN" || tipo_del_docu == "DM")
                    {
                        cont_pagables_2++;
                    }
                }
                DataTable facturas_pagables = (DataTable)HttpContext.Current.Session["facturas_pagables"];
                foreach (string fact in facturas)
                {
                    //agrega list para enviar al solomon

                    cont_x++;
                    string tipo_del_docu = ReporteRNegocio.tipo_doc(fact.Trim());
                    string rutcliente_ = "";
                    if (tipo_del_docu == "IN" || tipo_del_docu == "DM")
                    {
                        //cont_x++;
                        ////                                                                                            PAGO FACTURAS ----- NOTA DEBITO
                        string monto_fac = "";
                        string facturas_aplicadas = "";

                        foreach (DataRow str in facturas_pagables.Rows)
                        {
                            if (str[0].ToString().Trim() == fact.ToString().Trim())
                            {
                                rutcliente_ = str["rutcliente"].ToString().Trim();

                                string facturas_aplicadas_in = "";
                                try
                                {
                                    if (str["facturas_aplicadas"].ToString().Trim().Substring(str[9].ToString().Trim().Length - 1) == "-")
                                    {
                                        str["facturas_aplicadas"] = str[9].ToString().Trim().Substring(0, str[9].ToString().Trim().Length - 1);
                                        facturas_aplicadas_in = str[9].ToString().Trim().Substring(0, str[9].ToString().Trim().Length - 1);
                                    }
                                }
                                catch
                                {
                                    facturas_aplicadas_in = str["facturas_aplicadas"].ToString().Trim();
                                }
                                if (moneda == "peso")
                                {
                                    monto_fac = str["saldo_final_peso"].ToString().Replace(".", ",");
                                    if (monto_fac == "")
                                    {
                                        monto_fac = str["saldo_peso"].ToString().Replace(".", ",");
                                    }
                                }
                                else
                                {
                                    monto_fac = str["saldo_final_dolar"].ToString().Replace(".", ",");
                                    if (monto_fac == "")
                                    {
                                        monto_fac = str["saldo_dolar"].ToString().Replace(".", ",");
                                    }
                                }
                                facturas_aplicadas = facturas_aplicadas_in;
                                break;
                            }
                        }

                        if (facturas_aplicadas == "")
                        {
                            facturas_aplicadas = fact.Trim();
                        }

                        try
                        {
                            string fecha_pago = "";
                            bool es_pa_f = false;
                            if (id.Contains("***"))
                            {
                                es_pa_f = true;
                                fecha_pago = id.Substring(id.IndexOf("***") + 3);
                                id = id.Substring(0, id.IndexOf("***"));
                                tipo_doc = "temporal";
                                string si_insert = ReporteRNegocio.insert_fecha_cobra(id, fecha_pago, "Estimado a pagar (aut.)");
                            }
                            else
                            {
                                facturas_al_erp.Add(fact.Trim());
                                fecha_pago = fecha;
                            }

                            if (suma_deposit == Convert.ToDouble(monto_fac))
                            {
                                monto = monto_fac;
                                suma_deposit -= double.Parse(monto);
                            }
                            else if (suma_deposit > Convert.ToDouble(monto_fac))
                            {
                                if (cont_x == cont_pagables_2)
                                {
                                    monto = suma_deposit.ToString();
                                    suma_deposit -= double.Parse(monto_fac);
                                }
                                else
                                {
                                    if (!id.Contains("--"))
                                    {
                                        monto = suma_deposit.ToString();
                                        suma_deposit -= double.Parse(monto);
                                    }
                                    else
                                    {
                                        monto = monto_fac.ToString();
                                        suma_deposit -= double.Parse(monto_fac);
                                    }
                                }
                            }
                            else
                            {
                                monto = suma_deposit.ToString();
                                suma_deposit -= double.Parse(monto);
                            }
                            DBUtil db = new DBUtil();
                            //DataTable dt = new DataTable();
                            //dt = db.consultar("select top 1 rutcliente, tipo_moneda from V_COBRANZA where factura =  '" + fact.Trim() + "'");
                            string query = "";
                            query += "INSERT INTO COBRANZA_PAGOS ( ";
                            query += "id_cobranza, ";
                            query += "tipo_doc, ";
                            query += "monto, ";
                            query += "fecha, ";
                            query += "moneda, ";
                            query += "descripcion, ";
                            query += "banco ";
                            query += ") VALUES ( ";
                            query += "@id_cobranza, ";
                            query += "@tipo_doc, ";
                            query += "@monto, ";
                            query += "CONVERT(datetime, @fecha, 103), ";
                            query += "@moneda, ";
                            query += "@descripcion, ";
                            query += "@banco ";
                            query += ") ;";

                            query += "INSERT INTO COBRANZA_SEGUIMIENTO ( ";
                            query += "num_factura, ";
                            query += "monto_doc, ";
                            query += "rutcliente, ";
                            query += "estado, ";
                            query += "tipo_doc, ";
                            query += "observacion, ";
                            query += "usuario, ";
                            query += "num_factura_origen, ";
                            query += "fecha, ";
                            query += "fecha_venc, estado_ingresado, aux3, aux4, aux2, aux5 ";
                            query += ") VALUES ( ";
                            query += "@_num_factura, ";
                            query += "@_monto_doc, ";
                            query += "@_rutcliente, ";
                            query += "@_estado, ";
                            query += "@_tipo_doc, ";
                            query += "@_observacion, ";
                            query += "@_usuario, ";
                            query += "@_num_factura_origen,  ";
                            query += "CONVERT(datetime, @_fecha, 103), ";
                            query += "CONVERT(datetime, GETDATE(), 103), 0 , @_aux3, @_aux4, @_aux2, @_aux5";
                            query += "); ";

                            List<SPVars> vars = new List<SPVars>();
                            vars.Add(new SPVars() { nombre = "id_cobranza", valor = fact.Trim() });
                            vars.Add(new SPVars() { nombre = "tipo_doc", valor = tipo_doc.Replace("'", "") });
                            vars.Add(new SPVars() { nombre = "monto", valor = monto.Replace("'", "").Replace(",", ".") });
                            vars.Add(new SPVars() { nombre = "fecha", valor = fecha_pago });
                            vars.Add(new SPVars() { nombre = "moneda", valor = moneda.Replace("'", "") });
                            if (descripcion.Contains("Cheque"))
                            {
                                string descripcion_cheque = HttpContext.Current.Session["descrip_cheque"].ToString();
                                vars.Add(new SPVars() { nombre = "_aux2", valor = descripcion_cheque });
                            }
                            else
                            {

                                vars.Add(new SPVars() { nombre = "_aux2", valor = DBNull.Value });

                            }

                            if (descripcion.Contains("NET"))
                            {
                                moneda = "";
                            }

                            vars.Add(new SPVars() { nombre = "descripcion", valor = descripcion.Replace("'", "") + "(" + moneda + ")" });
                            vars.Add(new SPVars() { nombre = "banco", valor = fact.Trim() });

                            //foreach (DataRow dr in dt.Rows)
                            //{
                            vars.Add(new SPVars() { nombre = "_num_factura", valor = fact.Trim() });
                            vars.Add(new SPVars() { nombre = "_monto_doc", valor = monto.Replace("'", "").Replace(",", ".") });
                            vars.Add(new SPVars() { nombre = "_rutcliente", valor = rutcliente_ });

                            if (cerrar == "si")
                            {
                                vars.Add(new SPVars() { nombre = "_estado", valor = "PAGADO" });
                                vars.Add(new SPVars() { nombre = "_tipo_doc", valor = "PA" });
                                ReporteRNegocio.quitar_pa_f(id);
                            }
                            else
                            {
                                if (es_pa_f)
                                {
                                    vars.Add(new SPVars() { nombre = "_estado", valor = "TEMPORAL" });
                                    vars.Add(new SPVars() { nombre = "_tipo_doc", valor = "PA-F" });
                                }
                                else
                                {
                                    vars.Add(new SPVars() { nombre = "_estado", valor = "ABONO" });
                                    vars.Add(new SPVars() { nombre = "_tipo_doc", valor = "PA" });
                                }
                            }
                            vars.Add(new SPVars() { nombre = "_observacion", valor = descripcion.Replace("'", "") + "(" + moneda + ")" });
                            vars.Add(new SPVars() { nombre = "_usuario", valor = HttpContext.Current.Session["user"].ToString() });
                            vars.Add(new SPVars() { nombre = "_num_factura_origen", valor = id });
                            vars.Add(new SPVars() { nombre = "_aux3", valor = facturas_aplicadas });
                            vars.Add(new SPVars() { nombre = "_aux5", valor = cuenta_banco });
                            vars.Add(new SPVars() { nombre = "_fecha", valor = fecha_pago });
                            vars.Add(new SPVars() { nombre = "_fecha_venc", valor = DateTime.Now.ToShortDateString() });


                            //misma tasa cambio   Session["tipo_cambio_iguales"]
                            bool misma_tasa = (bool)HttpContext.Current.Session["tipo_cambio_iguales"];
                            if (misma_tasa)
                            {
                                try
                                {
                                    vars.Add(new SPVars() { nombre = "_aux4", valor = descripcion.Replace("'", "").Trim().Substring(0, 10) });
                                }
                                catch
                                {
                                    vars.Add(new SPVars() { nombre = "_aux4", valor = descripcion.Replace("'", "").Trim() });
                                }
                            }
                            else
                            {
                                try
                                {
                                    vars.Add(new SPVars() { nombre = "_aux4", valor = descripcion.Replace("'", "").Trim().Substring(0, 10) });
                                }
                                catch
                                {
                                    vars.Add(new SPVars() { nombre = "_aux4", valor = descripcion.Replace("'", "").Trim() });
                                }
                            }

                            //}

                            db.Scalar2(query, vars);
                        }
                        catch (Exception ex)
                        {
                            string ERROR = ex.Message.ToString();
                            ok_pago = "Falló el pago";
                        }
                    }
                    else if (tipo_del_docu.Contains("CM") || tipo_del_docu.Contains("PA") && !IsNumeric2(fact))
                    {
                        // APLICACION DE PAGO PARA NOTAS DE CREDITO Y SALDOS A FAVOR     28/08/2019
                        ////                                                                                            NOTAS DE CREDITO ----- SALDOS A FAVOR
                        //agrega list para enviar al solomon
                        facturas_al_erp.Add(fact.Trim());

                        string moneda_cm = "";

                        string monto_fac = "";
                        string facturas_aplicadas = "";

                        foreach (DataRow str in facturas_pagables.Rows)
                        {
                            if (str[4].ToString() != "IN")
                            {
                                string facturas_aplicadas_CM = str[9].ToString().Trim();
                                rutcliente_ = str["rutcliente"].ToString().Trim();
                                moneda_cm = str["tipo_moneda"].ToString().Trim();

                                try
                                {
                                    if (str["facturas_aplicadas"].ToString().Trim().Substring(str[9].ToString().Trim().Length - 1) == "-")
                                    {
                                        str["facturas_aplicadas"] = str[9].ToString().Trim().Substring(0, str[9].ToString().Trim().Length - 1);
                                        facturas_aplicadas_CM = str[9].ToString().Trim().Substring(0, str[9].ToString().Trim().Length - 1);
                                    }
                                }
                                catch
                                {
                                    facturas_aplicadas_CM = str["facturas_aplicadas"].ToString().Trim();
                                }


                                if (str[0].ToString().Trim() == fact.ToString().Trim() && str[4].ToString() != "NOTACREDITO-F")
                                {
                                    if (moneda_cm == "peso")
                                    {
                                        monto_fac = str["saldo_final_peso"].ToString().Replace(".", ",");
                                        if (monto_fac == "")
                                        {
                                            monto_fac = str["saldo_peso"].ToString().Replace(".", ",");
                                        }
                                    }
                                    else
                                    {
                                        monto_fac = str["saldo_final_dolar"].ToString().Replace(".", ",");
                                        if (monto_fac == "")
                                        {
                                            monto_fac = str["saldo_dolar"].ToString().Replace(".", ",");
                                        }
                                    }

                                    facturas_aplicadas = facturas_aplicadas_CM;


                                    string fecha_pago = fecha;
                                    monto = monto_fac;
                                    DBUtil db = new DBUtil();
                                    //DataTable dt = new DataTable();
                                    //dt = db.consultar("select top 1 rutcliente, tipo_moneda from V_COBRANZA where factura =  '" + fact.Trim() + "'");
                                    string query = "";
                                    query += "INSERT INTO COBRANZA_PAGOS ( ";
                                    query += "id_cobranza, ";
                                    query += "tipo_doc, ";
                                    query += "monto, ";
                                    query += "fecha, ";
                                    query += "moneda, ";
                                    query += "descripcion, ";
                                    query += "banco ";
                                    query += ") VALUES ( ";
                                    query += "@id_cobranza, ";
                                    query += "@tipo_doc, ";
                                    query += "@monto, ";
                                    query += "CONVERT(datetime, @fecha, 103), ";
                                    query += "@moneda, ";
                                    query += "@descripcion, ";
                                    query += "@banco ";
                                    query += ") ;";

                                    query += "INSERT INTO COBRANZA_SEGUIMIENTO ( ";
                                    query += "num_factura, ";
                                    query += "monto_doc, ";
                                    query += "rutcliente, ";
                                    query += "estado, ";
                                    query += "tipo_doc, ";
                                    query += "observacion, ";
                                    query += "usuario, ";
                                    query += "num_factura_origen, ";
                                    query += "fecha, ";
                                    query += "fecha_venc, estado_ingresado, aux3 ";
                                    query += ") VALUES ( ";
                                    query += "@_num_factura, ";
                                    query += "@_monto_doc, ";
                                    query += "@_rutcliente, ";
                                    query += "@_estado, ";
                                    query += "@_tipo_doc, ";
                                    query += "@_observacion, ";
                                    query += "@_usuario, ";
                                    query += "@_num_factura_origen, ";
                                    query += "CONVERT(datetime, @_fecha, 103), ";
                                    query += "CONVERT(datetime, GETDATE(), 103), 0 , @_aux3";
                                    query += "); ";
                                    List<SPVars> vars = new List<SPVars>();
                                    vars.Add(new SPVars() { nombre = "id_cobranza", valor = fact.Trim() });
                                    vars.Add(new SPVars() { nombre = "tipo_doc", valor = tipo_doc.Replace("'", "") });
                                    vars.Add(new SPVars() { nombre = "monto", valor = monto.Replace("'", "").Replace(",", ".") });
                                    vars.Add(new SPVars() { nombre = "fecha", valor = fecha_pago });
                                    vars.Add(new SPVars() { nombre = "moneda", valor = moneda_cm.Replace("'", "") });
                                    vars.Add(new SPVars() { nombre = "descripcion", valor = descripcion.Replace("'", "") + "(" + moneda_cm + ")" });
                                    vars.Add(new SPVars() { nombre = "banco", valor = facturas_aplicadas });

                                    //foreach (DataRow dr in dt.Rows)
                                    //{
                                    vars.Add(new SPVars() { nombre = "_num_factura", valor = fact.Trim() });
                                    vars.Add(new SPVars() { nombre = "_monto_doc", valor = monto.Replace("'", "").Replace(",", ".") });
                                    vars.Add(new SPVars() { nombre = "_rutcliente", valor = rutcliente_ });

                                    if (cerrar == "si")
                                    {
                                        if (!IsNumeric2(fact))
                                        {
                                            vars.Add(new SPVars() { nombre = "_estado", valor = "SALDO_FAVOR" });
                                            vars.Add(new SPVars() { nombre = "_tipo_doc", valor = "PA" });
                                            ReporteRNegocio.quitar_pa_f(id);
                                        }

                                        else
                                        {
                                            vars.Add(new SPVars() { nombre = "_estado", valor = "NOTA_CREDITO" });
                                            vars.Add(new SPVars() { nombre = "_tipo_doc", valor = "PA" });
                                            ReporteRNegocio.quitar_pa_f(id);
                                        }
                                    }
                                    else
                                    {
                                        if (!IsNumeric2(fact))
                                        {
                                            vars.Add(new SPVars() { nombre = "_estado", valor = "SALDO_FAVOR" });
                                            vars.Add(new SPVars() { nombre = "_tipo_doc", valor = "PA" });
                                            ReporteRNegocio.quitar_pa_f(id);
                                        }
                                        else
                                        {
                                            vars.Add(new SPVars() { nombre = "_estado", valor = "NOTA_CREDITO" });
                                            vars.Add(new SPVars() { nombre = "_tipo_doc", valor = "PA" });
                                            ReporteRNegocio.quitar_pa_f(id);
                                        }
                                    }
                                    vars.Add(new SPVars() { nombre = "_observacion", valor = descripcion.Replace("'", "") + "(" + moneda_cm + ")" });
                                    vars.Add(new SPVars() { nombre = "_usuario", valor = HttpContext.Current.Session["user"].ToString() });
                                    vars.Add(new SPVars() { nombre = "_num_factura_origen", valor = id });
                                    vars.Add(new SPVars() { nombre = "_aux3", valor = facturas_aplicadas });
                                    vars.Add(new SPVars() { nombre = "_fecha", valor = fecha_pago });
                                    vars.Add(new SPVars() { nombre = "_fecha_venc", valor = DateTime.Now.ToShortDateString() });



                                    //}
                                    db.Scalar2(query, vars);
                                }
                            }
                        }

                    }
                    else
                    {
                        ReporteRNegocio.insert_seguimiento(fact);
                    }
                }

                //// ACA VALIDA SI ES ENVIO AL SOLOMON----   
                if (enviar_erp == "True")
                {
                    try
                    {
                        if (facturas_al_erp.Count > 0)
                        {
                            //string seperado por comas a partir de un List < STRING >
                            string facturas_comas = string.Join(",", facturas_al_erp);
                            string ok = ReporteRNegocio.insert_X2POSSL(facturas_comas);
                            ok = ReporteRNegocio.insert_X2POSTROK(facturas_comas);
                        }
                    }
                    catch
                    {


                    }
                    /// validar despues el estado_ingresado...
                    /// 
                }
            }
            catch (Exception ex)
            {
                string ex_ = ex.Message;
                ok_pago = "Falló el pago";
            }
            return ok_pago;
        }

        private static bool lo_demas_db(string v)
        {
            bool son_dm = false;
            v = " - " + v + " - ";



            return son_dm;
        }

        [WebMethod]
        public static string Registrar_Pago_cheque(string id, string monto, string moneda, string tipo_doc, string descripcion, string banco, string vencimiento, string cuenta, string num_cheque, string cerrar)
        {
            try
            {
                DBUtil db = new DBUtil();

                DataTable dt = new DataTable();

                dt = db.consultar("select cs.num_factura, cs.rutcliente, (select top 1 b.nombrecliente from cobranza b where b.rutcliente = cs.rutcliente)  as nombrecliente, cb.cod_banco from cobranza_seguimiento CS INNER JOIN COBRANZA_BANCOS CB ON CB.ID = " + banco + " where cs.num_factura like '%" + id + "%'");

                string query = "";
                query += "INSERT INTO COBRANZA_PAGOS ( ";
                query += "id_cobranza, ";
                query += "tipo_doc, ";
                query += "monto, ";
                query += "cuenta, ";
                query += "fecha, ";
                query += "moneda, ";
                query += "descripcion, ";
                query += "banco, ";
                query += "vencimiento ";
                query += ") VALUES ( ";
                query += "@id_cobranza, ";
                query += "@tipo_doc, ";
                query += "@monto, ";
                query += "@cuenta, ";
                query += "CONVERT(datetime, GETDATE(), 103), ";
                query += "@moneda, ";
                query += "@descripcion, ";
                query += "@banco, ";
                query += "CONVERT(datetime, @vencimiento, 103) ";
                query += "); ";


                query += "INSERT INTO COBRANZA_SEGUIMIENTO ( ";
                query += "num_factura, ";
                query += "monto_doc, ";
                query += "rutcliente, ";
                query += "estado, ";
                query += "tipo_doc, ";
                query += "observacion, ";
                query += "usuario, ";
                query += "num_factura_origen, ";
                query += "fecha, ";
                query += "fecha_venc, estado_ingresado ";
                query += ") VALUES ( ";
                query += "@_num_factura, ";
                query += "@_monto_doc, ";
                query += "@_rutcliente, ";
                query += "@_estado, ";
                query += "@_tipo_doc, ";
                query += "@_observacion, ";
                query += "@_usuario, ";
                query += "@_num_factura_origen, ";
                query += "CONVERT(datetime, GETDATE(), 103), ";
                query += "CONVERT(datetime, @_fecha_venc, 103), 0 ";
                query += "); ";

                List<SPVars> vars = new List<SPVars>();
                vars.Add(new SPVars() { nombre = "id_cobranza", valor = id });
                vars.Add(new SPVars() { nombre = "tipo_doc", valor = tipo_doc.Replace("'", "") });
                vars.Add(new SPVars() { nombre = "monto", valor = monto.Replace("'", "") });
                vars.Add(new SPVars() { nombre = "cuenta", valor = cuenta.Replace("'", "") });
                vars.Add(new SPVars() { nombre = "fecha", valor = DateTime.Now.ToShortDateString() });
                vars.Add(new SPVars() { nombre = "moneda", valor = moneda.Replace("'", "") });
                vars.Add(new SPVars() { nombre = "descripcion", valor = descripcion.Replace("'", "") });
                vars.Add(new SPVars() { nombre = "banco", valor = banco.Replace("'", "") });
                vars.Add(new SPVars() { nombre = "vencimiento", valor = vencimiento.Replace("'", "") });

                foreach (DataRow dr in dt.Rows)
                {
                    vars.Add(new SPVars() { nombre = "_num_factura", valor = dr["cod_banco"].ToString().Replace("'", "") + num_cheque });
                    vars.Add(new SPVars() { nombre = "_monto_doc", valor = monto.Replace("'", "") });
                    vars.Add(new SPVars() { nombre = "_rutcliente", valor = dr["rutcliente"].ToString().Replace("'", "") });

                    vars.Add(new SPVars() { nombre = "_estado", valor = "CHEQUE" });
                    vars.Add(new SPVars() { nombre = "_tipo_doc", valor = "DM" });

                    vars.Add(new SPVars() { nombre = "_observacion", valor = descripcion.Replace("'", "") });
                    vars.Add(new SPVars() { nombre = "_usuario", valor = HttpContext.Current.Session["user"].ToString() });
                    vars.Add(new SPVars() { nombre = "_num_factura_origen", valor = id });
                    vars.Add(new SPVars() { nombre = "_fecha", valor = DateTime.Now.ToShortDateString() });
                    vars.Add(new SPVars() { nombre = "_fecha_venc", valor = vencimiento });
                    break;
                }

                db.Scalar2(query, vars);

                return "OK";
            }
            catch (Exception ex)
            {
                return "ERROR : " + ex.Message.ToString();
            }
        }

        private static double suma_cheques = 0;

        private static int cont_cheq2 = 0;


        private static bool asd = true;
        private static bool un_pago = true;
        private static int cont_facturas = 0;
        private static DataTable facturas_pagables;
        private static int cn_fact = 0;
        private static string cheques_insert;




        [WebMethod]
        public static string validar_cheque_(string banco, string num_cheque)

        {
            DBUtil db = new DBUtil();
            DataTable dt = new DataTable();
            dt = db.consultar(" select COUNT(*) " +
                              "       from cobranza_seguimiento CS " +
                              "      left OUTER JOIN COBRANZA_BANCOS CB ON CB.ID = " + banco +
                              "         where num_factura = CB.cod_banco + '" + num_cheque + "' ");



            return dt.Rows[0][0].ToString();
        }



        [WebMethod]
        public static string Registrar_Pago_cheque2(string id, string monto, string moneda, string tipo_doc, string banco, string vencimiento,
                                                       string num_cheque, string tcamb, string tobs, string ttdolar, string cerrar, string total, string num_cheques,
                                                       string cont_cheq, string monto_cheques, string enviar_erp, string descrip_cheque)
        {
            //APLICACION PARA PAGAR CHEQUE VIGENTE  =  28/08/2019
            HttpContext.Current.Session["descrip_cheque"] = descrip_cheque;
            num_cheques = num_cheques.Substring(0, num_cheques.Length - 1);
            monto = monto.Replace(".", ",");
            List<string> facturas = id.Split('-').ToList();
            HttpContext.Current.Session["cont_facturas"] = facturas.Count;
            cont_cheq2 = Convert.ToInt32(HttpContext.Current.Session["cont_cheq2"]);
            cont_cheq2++;
            HttpContext.Current.Session["cont_cheq2"] = cont_cheq2;
            suma_cheques = double.Parse(total.Replace(".", ","));
            string pago_ok = "";
            if (Convert.ToInt32(cont_cheq) == cont_cheq2)
            {
                pago_ok = Registrar_Pago_efectivo2(id, total.ToString(), moneda, tipo_doc, "Pago Con Cheque *" + num_cheques + "*", "no", vencimiento, enviar_erp, "CHEQUE");
            }
            List<string> facturas_al_erp = new List<string>();
            foreach (string fact in facturas)
            {
                string tipo_del_docu = ReporteRNegocio.tipo_doc(fact.Trim());
                //if (tipo_del_docu == "IN" || (IsNumeric2(fact) && tipo_del_docu == "DM") || (tipo_del_docu == "DM" && fact.Substring(0, 2) == "F ") || tipo_del_docu == "DM" )
                if (tipo_del_docu == "IN" || (IsNumeric2(fact) && tipo_del_docu == "DM") || (tipo_del_docu == "DM" && fact.Substring(0, 2) == "F "))
                {
                    try
                    {
                        DBUtil db = new DBUtil();
                        DataTable dt = new DataTable();
                        dt = db.consultar("select cs.num_factura, cs.rutcliente, (select top 1 b.nombrecliente from cobranza b where b.rutcliente = cs.rutcliente)  as nombrecliente, cb.cod_banco " +
                                      "   from cobranza_seguimiento CS INNER JOIN COBRANZA_BANCOS CB ON CB.ID = " + banco + " where cs.num_factura = '" + fact.Trim() + "' and estado = 'EN SEGUIMIENTO'");
                        //facturas_al_erp.Add(fact.Trim());
                        double aux_monto = 0;
                        try
                        {
                            aux_monto = Convert.ToDouble(monto.Replace("'", "").Trim());
                        }
                        catch
                        {

                        }
                        List<SPVars> vars2 = new List<SPVars>();

                        string cheque_ya_ingresado = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            dt = db.consultar("select CS.num_factura from cobranza_seguimiento CS where CS.num_factura = '" + dr["cod_banco"].ToString().Replace("'", "") + num_cheque + "'");
                            if (dt.Rows.Count >= 1)
                            {
                                cheque_ya_ingresado = dt.Rows[0].ToString();
                            }
                            vars2.Add(new SPVars() { nombre = "_num_factura", valor = dr["cod_banco"].ToString().Replace("'", "") + num_cheque });

                            bool esta_agregada = false;
                            string num_factura = (dr["cod_banco"].ToString().Replace("'", "") + num_cheque).Trim();
                            foreach (string r in facturas_al_erp)
                            {
                                if (r == num_factura)
                                {
                                    esta_agregada = true;
                                }
                            }
                            if (!esta_agregada)
                            {
                                facturas_al_erp.Add((dr["cod_banco"].ToString().Replace("'", "") + num_cheque).Trim());
                            }

                            vars2.Add(new SPVars() { nombre = "_monto_doc", valor = aux_monto });
                            vars2.Add(new SPVars() { nombre = "_rutcliente", valor = dr["rutcliente"].ToString().Replace("'", "") });

                            vars2.Add(new SPVars() { nombre = "_estado", valor = "CHEQUE" });
                            vars2.Add(new SPVars() { nombre = "_tipo_doc", valor = "DM" });

                            vars2.Add(new SPVars() { nombre = "_observacion", valor = moneda + "*" + num_cheques + "*" });
                            vars2.Add(new SPVars() { nombre = "_usuario", valor = HttpContext.Current.Session["user"].ToString() });
                            vars2.Add(new SPVars() { nombre = "_num_factura_origen", valor = id.Trim() });
                            vars2.Add(new SPVars() { nombre = "_fecha", valor = DateTime.Now.ToShortDateString() });
                            vars2.Add(new SPVars() { nombre = "_fecha_venc", valor = vencimiento });

                            vars2.Add(new SPVars() { nombre = "_tcamb", valor = tcamb });
                            vars2.Add(new SPVars() { nombre = "_tobs", valor = tobs });
                            vars2.Add(new SPVars() { nombre = "_ttdolar", valor = ttdolar });
                            break;
                        }


                        //GUARDANDO LOS PARAMETROS DE INSERT DE CHEQUES PARA LUEGO HACERLOS.
                        string ok = "";
                        if (cheque_ya_ingresado == "")
                        {
                            ok = ReporteRNegocio.ins_en_seg(vars2);
                        }
                        else
                        {
                            ok = ReporteRNegocio.ins_en_seg(vars2);
                            //ok = "OK";
                        }

                        if (ok != "OK")
                        {
                            return "Error -";
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                    }
                }
                else
                {
                    ReporteRNegocio.insert_seguimiento(fact);
                }
            }

            //// ACA VALIDA SI ES ENVIO AL SOLOMON----CHEQUE
            if (enviar_erp == "True")
            {
                try
                {
                    //string seperado por comas a partir de un List < STRING >
                    string facturas_comas = string.Join(",", facturas_al_erp);
                    string ok1 = ReporteRNegocio.insert_X2POSSL_CHEQUE(facturas_comas);
                    ok1 = ReporteRNegocio.insert_X2POSTROK_CHEQUE(facturas_comas);
                }
                catch { }
            }

            return pago_ok;

        }

        [WebMethod]
        public static string Registrar_Pago_cheque3(string id, string monto, string moneda, string tipo_doc, string banco, string vencimiento,
                                               string num_cheque, string cerrar, string total, string num_cheques, string cont_cheq, string monto_cheques, string rutcliente)
        {



            num_cheques = num_cheques.Substring(0, num_cheques.Length - 1);

            monto = monto.Replace(".", ",");

            cont_cheq2 = Convert.ToInt32(HttpContext.Current.Session["cont_cheq2"]);

            cont_cheq2++;

            HttpContext.Current.Session["cont_cheq2"] = cont_cheq2;
            suma_cheques = double.Parse(total.Replace(".", ","));

            string pago_ok = "";
            if (Convert.ToInt32(cont_cheq) == cont_cheq2)
            {
                pago_ok = Registrar_Pago_efectivo3(id, total.ToString(), moneda, tipo_doc, "DIRECTO Pago Con Cheque *" + num_cheques + "*", "no", vencimiento, rutcliente);

            }

            try
            {
                DBUtil db = new DBUtil();

                DataTable dt = new DataTable();

                dt = db.consultar("select top 1 CB.cod_banco from COBRANZA_BANCOS CB where CB.ID = " + banco);

                List<SPVars> vars2 = new List<SPVars>();
                foreach (DataRow dr in dt.Rows)
                {
                    vars2.Add(new SPVars() { nombre = "_num_factura", valor = dr["cod_banco"].ToString().Replace("'", "") + num_cheque });
                    vars2.Add(new SPVars() { nombre = "_monto_doc", valor = monto.Replace("'", "") });
                    vars2.Add(new SPVars() { nombre = "_rutcliente", valor = rutcliente.Trim() });

                    vars2.Add(new SPVars() { nombre = "_estado", valor = "CHEQUE" });
                    vars2.Add(new SPVars() { nombre = "_tipo_doc", valor = "DM" });

                    vars2.Add(new SPVars() { nombre = "_observacion", valor = moneda + "*" + num_cheques + "*" });
                    vars2.Add(new SPVars() { nombre = "_usuario", valor = HttpContext.Current.Session["user"].ToString() });
                    vars2.Add(new SPVars() { nombre = "_num_factura_origen", valor = id.Trim() });
                    vars2.Add(new SPVars() { nombre = "_fecha", valor = DateTime.Now.ToShortDateString() });
                    vars2.Add(new SPVars() { nombre = "_fecha_venc", valor = vencimiento });


                    vars2.Add(new SPVars() { nombre = "_tcamb", valor = DBNull.Value });
                    vars2.Add(new SPVars() { nombre = "_tobs", valor = DBNull.Value });
                    break;
                }


                //GUARDANDO LOS PARAMETROS DE INSERT DE CHEQUES PARA LUEGO HACERLOS.
                string ok = ReporteRNegocio.ins_en_seg(vars2);
                if (ok != "OK")
                {


                    return "Error -";
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }


            return pago_ok;

        }


        [WebMethod]
        public static string insert_cheques()
        {
            List<List<SPVars>> aux2 = new List<List<SPVars>>();
            aux2 = (List<List<SPVars>>)HttpContext.Current.Session["inception_prah"];
            string FINAL = "OK";
            foreach (List<SPVars> row in aux2)
            {
                string ok = ReporteRNegocio.ins_en_seg(row);
                if (ok != "OK")
                {
                    FINAL = "error insert_cheques()";

                    return FINAL;
                }

            }



            return FINAL;
        }

        [WebMethod]
        public static string Registrar_Pago_cerrar(string id, string monto, string moneda, string tipo_doc, string descripcion, string banco, string vencimiento, string cuenta, string num_cheque, string cerrar)
        {
            try
            {
                DBUtil db = new DBUtil();
                DataTable dt = new DataTable();
                string query = "";

                dt = db.consultar("select cs.num_factura, cs.rutcliente, (select top 1 b.nombrecliente from cobranza b where b.rutcliente = cs.rutcliente)  as nombrecliente, cb.cod_banco from cobranza_seguimiento CS INNER JOIN COBRANZA_BANCOS CB ON CB.ID = " + banco + " where cs.num_factura like '%" + id + "%'");

                query += "INSERT INTO COBRANZA_SEGUIMIENTO ( ";
                query += "num_factura, ";
                query += "monto_doc, ";
                query += "rutcliente, ";
                query += "estado, ";
                query += "tipo_doc, ";
                query += "observacion, ";
                query += "usuario, ";
                query += "num_factura_origen, ";
                query += "fecha, ";
                query += "fecha_venc, estado_ingresado ";
                query += ") VALUES ( ";
                query += "@_num_factura, ";
                query += "@_monto_doc, ";
                query += "@_rutcliente, ";
                query += "@_estado, ";
                query += "@_tipo_doc, ";
                query += "@_observacion, ";
                query += "@_usuario, ";
                query += "@_num_factura_origen, ";
                query += "CONVERT(datetime, @_fecha, 103), ";
                query += "CONVERT(datetime, @_fecha_venc, 103), 0 ";
                query += "); ";

                List<SPVars> vars = new List<SPVars>();
                foreach (DataRow dr in dt.Rows)
                {
                    vars.Add(new SPVars() { nombre = "_num_factura", valor = dr["num_factura"].ToString().Replace("'", "") });
                    vars.Add(new SPVars() { nombre = "_monto_doc", valor = monto.Replace("'", "") });
                    vars.Add(new SPVars() { nombre = "_rutcliente", valor = dr["rutcliente"].ToString().Replace("'", "") });
                    vars.Add(new SPVars() { nombre = "_estado", valor = "PAGADO" });
                    vars.Add(new SPVars() { nombre = "_tipo_doc", valor = "PA" });
                    vars.Add(new SPVars() { nombre = "_observacion", valor = "Factura Pagada con cheques" });
                    vars.Add(new SPVars() { nombre = "_usuario", valor = HttpContext.Current.Session["user"].ToString() });
                    vars.Add(new SPVars() { nombre = "_num_factura_origen", valor = id });
                    vars.Add(new SPVars() { nombre = "_fecha", valor = DateTime.Now.ToShortDateString() });
                    vars.Add(new SPVars() { nombre = "_fecha_venc", valor = DateTime.Now.ToShortDateString() });
                }
                ReporteRNegocio.quitar_pa_f(id);
                db.Scalar2(query, vars);

                return "OK";
            }
            catch (Exception ex)
            {
                return "ERROR : " + ex.Message.ToString();
            }
        }

        [WebMethod]
        public static string Seguimiento(string id)
        {
            try
            {
                DBUtil db = new DBUtil();
                string query = "";

                query += "INSERT INTO COBRANZA_SEGUIMIENTO  ( ";
                query += "num_factura, ";
                query += "rutcliente, ";
                query += "tipo_doc, ";
                query += "estado, ";
                query += "monto_doc, ";
                query += "observacion, ";
                query += "usuario, ";
                query += "fecha_venc, ";
                query += "fecha, estado_ingresado ";
                query += ") SELECT ";
                query += "factura, ";
                query += "rutcliente, ";
                query += "tipo_doc, ";
                query += "'EN SEGUIMIENTO', ";
                query += "monto_doc, ";
                query += "descr, ";
                query += "'" + HttpContext.Current.Session["user"].ToString() + "', ";
                query += "CONVERT(datetime, fecha_venc, 103), ";
                query += "CONVERT(datetime, fecha_trans, 103), 0 as test from V_COBRANZA where factura like '%" + id + "%'";

                db.Scalar2(query);
                return "OK";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message.ToString();
            }
        }

        [WebMethod]
        public static string ve_session()
        {
            return "OK";
        }

        [WebMethod]
        public static string Enviar_Correo(string destino, string cc, string contenido)
        {

            string user = HttpContext.Current.Session["user"].ToString();
            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(destino));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "Cobranza SOPRODI (" + user + ")( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
            if (cc != "")
            {
                email.CC.Add(cc);
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
            email.Body += "<div><img src='http://a58.imgup.net/Sopro4d9d.png' style='    float: right;     width: 90px;'> </div><br><br><br><br><br>";

            email.Body += "<div> " + contenido.Replace("\n", "<br/>") + " <br><br>";

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
                smtp.Send(email);
                email.Dispose();
                return "OK";
            }
            catch (Exception ex)
            {
                return "Error al enviar";
            }

        }



        [WebMethod]
        public static string MARCACOMOPAGADO(string id)
        {
            try
            {
                DBUtil db = new DBUtil();
                string query = "";

                query += "INSERT INTO COBRANZA_SEGUIMIENTO  ( ";
                query += "num_factura, ";
                query += "rutcliente, ";
                query += "tipo_doc, ";
                query += "estado, ";
                query += "monto_doc, ";
                query += "observacion, ";
                query += "usuario, ";
                query += "fecha_venc, ";
                query += "fecha, estado_ingresado ";
                query += ") SELECT ";
                query += "factura, ";
                query += "rutcliente, ";
                query += "'PA', ";
                query += "'PAGADO CON BOTON DE PAGO', ";
                query += "monto_doc, ";
                query += "'PAGADO CON BOTON DE PAGO', ";
                query += "'" + HttpContext.Current.Session["user"].ToString() + "', ";
                query += "CONVERT(datetime, fecha_venc, 103), ";
                query += "CONVERT(datetime, fecha_trans, 103), 0 as test  from V_COBRANZA where id = " + id;

                db.Scalar2(query);
                return "OK";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message.ToString();
            }
        }



        [WebMethod]
        public static string Reporte_Estimados(string desde, string hasta)
        {
            string tabla = "";
            DBUtil db = new DBUtil();

            //DataTable dt = db.consultar("select id_cobranza, (select top 1 nom_accion from acciones a where Cobranza_Acciones.id_accion = a.id_accion) as accion,  fecha_accion as fecha, usuario, obs from Cobranza_Acciones where ID_cobranza = '" + id + "'");

            tabla += "<table class=\"table fill-head table-bordered\">";
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

                string suma_peso_solom = monto_format((Convert.ToDouble(peso_in_solom) + Convert.ToDouble(peso_cheque)).ToString());
                string suma_dolar_solom = monto_format((Convert.ToDouble(dolar_in_solom) + Convert.ToDouble(dolar_cheque)).ToString());


                string suma_peso = monto_format((Convert.ToDouble(peso_in) + Convert.ToDouble(peso_cheque)).ToString());
                string suma_dolar = monto_format((Convert.ToDouble(dolar_in) + Convert.ToDouble(dolar_cheque)).ToString());

                DateTime aux1 = Convert.ToDateTime(desde, new CultureInfo("es-ES"));
                string dia_sema = dia_semana_ingles(aux1.DayOfWeek.ToString());
                tabla += "<tr>";
                tabla += "<td>" + dia_sema + " " + desde.Substring(0, 2) + "</td>";
                tabla += "<td></td>";
                tabla += "<td></td>";
                tabla += "</tr>";

                tabla += "<tr>";
                tabla += "<td> COBRANZA</td>";
                tabla += "<td> <table width='100%'><tr> <td style='width: 50%; border: 1px solid rgb(221, 221, 221);'>" + monto_format(peso_in_solom) + "</td>";
                tabla += "                              <td style = 'width: 50%; border: 1px solid rgb(221, 221, 221);'>" + monto_format(peso_in) + "</td>   </tr></table></td>";

                tabla += "<td> <table width='100%'><tr> <td style='width: 50%; border: 1px solid rgb(221, 221, 221);'>" + monto_format(dolar_in_solom) + "</td>";
                tabla += "                              <td style = 'width: 50%; border: 1px solid rgb(221, 221, 221);'>" + monto_format(dolar_in) + "</td>   </tr></table></td>";



                tabla += "</tr>";

                tabla += "<tr>";
                tabla += "<td>CHEQUES POR COBRAR </td>";
                tabla += "<td style='text-align: center;'> " + monto_format(peso_cheque) + "</td>";
                tabla += "<td style='text-align: center;'> " + monto_format(dolar_cheque) + "</td>";
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
            return tabla;

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


        [WebMethod]
        public static string Vencidos_()
        {
            string html = "";
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DBUtil db = new DBUtil();

            html = db.Scalar("select ROUND(sum(CAST(a.monto_doc AS FLOAT)), 0) from (select * from v_cobranza where tipo_doc in ('DM', 'IN')) a left join  (SELECT * FROM v_cobranza where tipo_doc in ('PA','CM' ))  b	on a.factura = b.factura where b.factura is null and a.fecha_venc < getdate() ").ToString();
            double d;
            double.TryParse(html, out d);
            string aux = "";
            if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
            html = aux;

            return html;
        }

        [WebMethod]
        public static string Cerrados_app()
        {
            string html = "";
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DBUtil db = new DBUtil();

            string sql = @"select a.rutcliente as Rutcliente, a.nombrecliente as NombCliente, a.factura as Factura, a.nombrevendedor as NombVendedor, a.descr as 'Descripción' 
	                        , CONVERT(varchar, a.fecha_trans, 103) as FechaTrans, CONVERT(varchar, a.fecha_venc, 103) as FechaVenc,  '$ ' + convert(varchar,cast(monto_doc as money),1) as Monto
                        from estado_documento b inner join V_COBRANZA a on a.factura = b.num_factura and a.id =  b.id   where b.estado <> 1 and a.estado_doc <> 0";

            dt2 = db.consultar(sql);
            html += "<h3>Documentos cerrados por app</h3>";

            html += "<table class=\"table fill-head table-bordered\">";
            html += "<thead class=\"test\">";
            html += "<tr>";

            html += "<th>RutCliente</th>";
            html += "<th>NombreCliente</th>";
            html += "<th>Num Factura</th>";
            html += "<th>NombreVendedor</th>";
            html += "<th>Descripción</th>";
            html += "<th>Fecha</th>";
            html += "<th>Vencimiento</th>";
            html += "<th>Monto Doc.</th>";

            html += "</tr>";
            html += "</thead>";
            html += "<tbody>";
            foreach (DataRow dr in dt2.Rows)
            {
                html += "<tr>";
                html += "<td>" + rut_format(dr["Rutcliente"].ToString()) + "</td>";
                html += "<td>" + dr["NombCliente"].ToString() + "</td>";
                html += "<td>" + click_factura(dr["Factura"].ToString()) + "</td>";
                html += "<td>" + dr["NombVendedor"].ToString() + "</td>";
                html += "<td>" + dr["Descripción"].ToString() + "</td>";
                html += "<td>" + dr["FechaTrans"].ToString() + "</td>";
                html += "<td>" + dr["FechaVenc"].ToString() + "</td>";
                html += "<td>" + monto_format(dr["Monto"].ToString()) + "</td>";
                html += "</tr>";
            }
            html += "</tbody>";
            html += "</table>";

            return html;
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
            bool es_dolar = false;
            double d;
            try
            {
                double.TryParse(p.Substring(0, p.IndexOf(",")), out d);
                es_dolar = true;
            }
            catch
            {
                double.TryParse(p, out d);
            }
            string aux = "";
            if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
            if (es_dolar) { return aux + "," + p.Substring(p.IndexOf(",")); } else { return aux; }

        }



        protected void G_INIT_EXCEL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool puede_ = true;


                //if (e.Row.Cells[4].Text == "PA" && IsNumeric(e.Row.Cells[3].Text))
                //{
                //    puede_ = false;
                //    e.Row.Visible = false;
                //}
                if (e.Row.Cells[4].Text == "PA")
                {

                    e.Row.Cells[11].Text = "-" + e.Row.Cells[11].Text;
                    e.Row.Cells[12].Text = "-" + e.Row.Cells[12].Text;
                    e.Row.Cells[13].Text = "-" + e.Row.Cells[13].Text;
                    e.Row.Cells[14].Text = "-" + e.Row.Cells[14].Text;


                    ((TextBox)e.Row.FindControl("txt_peso")).Text = "-" + ((TextBox)e.Row.FindControl("txt_peso")).Text;
                    ((TextBox)e.Row.FindControl("txt_dolar")).Text = "-" + ((TextBox)e.Row.FindControl("txt_dolar")).Text;



                }
                if (e.Row.Cells[4].Text == "CM")
                {

                    e.Row.Cells[11].Text = "-" + e.Row.Cells[11].Text;
                    e.Row.Cells[12].Text = "-" + e.Row.Cells[12].Text;
                    e.Row.Cells[13].Text = "-" + e.Row.Cells[13].Text;
                    e.Row.Cells[14].Text = "-" + e.Row.Cells[14].Text;



                    ((TextBox)e.Row.FindControl("txt_peso")).Text = "-" + ((TextBox)e.Row.FindControl("txt_peso")).Text;
                    ((TextBox)e.Row.FindControl("txt_dolar")).Text = "-" + ((TextBox)e.Row.FindControl("txt_dolar")).Text;



                    if (e.Row.Cells[3].Text.Trim().Substring(0, 1) == "1")
                    {
                        if (e.Row.Cells[3].Text.Trim() != "11737")
                        {
                            puede_ = false;
                            e.Row.Visible = false;
                        }
                    }
                    if (e.Row.Cells[16].Text == "&nbsp;")
                    {
                        e.Row.Cells[14].Text = e.Row.Cells[12].Text;
                    }
                    if (e.Row.Cells[13].Text == "&nbsp;")
                    {
                        e.Row.Cells[13].Text = e.Row.Cells[11].Text;
                    }

                }


                if (puede_)
                {
                    try
                    {
                        sum_peso_saldo += Convert.ToDouble(e.Row.Cells[13].Text.Replace(".", ""));
                    }
                    catch
                    {

                        string error = "aca";
                    }
                    try { sum_dolar_saldo += Convert.ToDouble(e.Row.Cells[14].Text.Replace(".", "")); }
                    catch
                    {
                        string error = "aca";
                    }
                    try { sum_peso_monto += Convert.ToDouble(e.Row.Cells[11].Text.Replace(".", "")); }
                    catch
                    {
                        string error = "aca";
                    }
                    try { sum_dolar_monto += Convert.ToDouble(e.Row.Cells[12].Text.Replace(".", "")); }
                    catch
                    {
                        string error = "aca";
                    }
                }

                cont_rows++;
                if (cont_rows == TOTAL_ROWS)
                {

                    //e.Row.Cells[9].Text = e.Row.Cells[9].Text.Trim().Substring(0, e.Row.Cells[9].Text.Trim().Length - 1);


                    try
                    {
                        G_INIT_EXCEL.HeaderRow.Cells[11].Text = G_INIT_EXCEL.HeaderRow.Cells[11].Text + "  (" + Base.monto_format2(sum_peso_monto).Trim() + ")";
                        G_INIT_EXCEL.HeaderRow.Cells[12].Text = G_INIT_EXCEL.HeaderRow.Cells[12].Text + "  (" + Base.monto_format2(sum_dolar_monto).Trim() + ")";
                        G_INIT_EXCEL.HeaderRow.Cells[13].Text = G_INIT_EXCEL.HeaderRow.Cells[13].Text + "  (" + Base.monto_format2(sum_peso_saldo).Trim() + ")";
                        G_INIT_EXCEL.HeaderRow.Cells[14].Text = G_INIT_EXCEL.HeaderRow.Cells[14].Text + "  (" + Base.monto_format2(sum_dolar_saldo).Trim() + ")";
                    }
                    catch { }
                }




                if (e.Row.Cells[20].Text == "SI")
                {

                    e.Row.BackColor = Color.FromArgb(255, 228, 196);

                    e.Row.Cells[4].Text = e.Row.Cells[4].Text;

                }



                if (e.Row.Cells[23].Text == "&nbsp;")
                {


                    e.Row.Cells[23].Text = "";
                }

                if (e.Row.Cells[23].Text == "peso")
                {

                    e.Row.Cells[23].Text = "PESO";
                }

                if (e.Row.Cells[23].Text == "dolar")
                {

                    e.Row.Cells[23].Text = "DOLAR";
                }



                //if (e.Row.Cells[9].Text != "0")
                //{

                //    e.Row.Cells[9].Text = "<a data-toggle='tooltip' style='color:red;' data-placement='top' title='" + e.Row.Cells[25].Text + "'> <img style='width: 50%;' src='img/speech-bubble.png' /></a>";
                //}
                //else
                //{
                //    e.Row.Cells[9].Text = "";
                //}


                e.Row.Cells[9].Text = e.Row.Cells[25].Text;


                e.Row.Cells[20].Visible = false;
                //e.Row.Cells[].Visible = false;
                G_INIT_EXCEL.HeaderRow.Cells[20].Visible = false;
                e.Row.Cells[21].Visible = false;
                //e.Row.Cells[].Visible = false;
                G_INIT_EXCEL.HeaderRow.Cells[21].Visible = false;

                e.Row.Cells[22].Visible = false;
                //e.Row.Cells[].Visible = false;
                G_INIT_EXCEL.HeaderRow.Cells[22].Visible = false;

                e.Row.Cells[17].Visible = false;
                //e.Row.Cells[].Visible = false;
                G_INIT_EXCEL.HeaderRow.Cells[17].Visible = false;

                e.Row.Cells[15].Visible = false;
                //e.Row.Cells[].Visible = false;
                G_INIT_EXCEL.HeaderRow.Cells[15].Visible = false;


                e.Row.Cells[25].Visible = false;
                //e.Row.Cells[].Visible = false;
                G_INIT_EXCEL.HeaderRow.Cells[25].Visible = false;


                //e.Row.Cells[17].Visible = false;
                ////e.Row.Cells[].Visible = false;
                //G_INIT.HeaderRow.Cells[17].Visible = false;


                if (busca_columna_fac)
                {
                    try
                    {
                        for (int x = 0; x <= G_INIT.HeaderRow.Cells.Count; x++)
                        {
                            if (G_INIT_EXCEL.HeaderRow.Cells[x].Text.Contains("N&#186;Doc"))
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
                string año_factura = e.Row.Cells[6].Text.Substring(0, 4);
                if (columna_fac)
                {

                    e.Row.Cells[COLUMNA_DE_FACTURA].Text = e.Row.Cells[COLUMNA_DE_FACTURA].Text;

                }



                G_INIT_EXCEL.HeaderRow.Cells[11].Attributes["data-sort-method"] = "number";

                G_INIT_EXCEL.HeaderRow.Cells[6].Attributes["data-sort-method"] = "date";
                G_INIT_EXCEL.HeaderRow.Cells[7].Attributes["data-sort-method"] = "date";
                G_INIT_EXCEL.HeaderRow.Cells[8].Attributes["data-sort-method"] = "date";


                if (e.Row.Cells[8].Text == "1900/01/01")
                {
                    e.Row.Cells[8].Text = "";
                }

                DateTime tras = DateTime.Now;
                DateTime venc = Convert.ToDateTime(e.Row.Cells[7].Text);
                TimeSpan ts = tras - venc;
                int differenceInDays = ts.Days;
                if (differenceInDays <= 0)
                {
                    differenceInDays = 0;
                    //e.Row.Cells[0].BackColor = Color.FromArgb(58, 223, 0, 0.16);
                    e.Row.Attributes["class"] = "table-flag-NOVENCIDOS";
                }

                if (differenceInDays > 0 && differenceInDays <= 5)
                {
                    e.Row.Attributes["class"] = "table-flag-5";
                    e.Row.Cells[7].Text = "<a style='color:red'>" + e.Row.Cells[7].Text + "</a>";

                }
                if (differenceInDays > 5 && differenceInDays <= 10)
                {
                    e.Row.Attributes["class"] = "table-flag-5_10";
                    e.Row.Cells[7].Text = "<a style='color:red'>" + e.Row.Cells[7].Text + "</a>";
                }
                if (differenceInDays > 10 && differenceInDays <= 15)
                {
                    e.Row.Attributes["class"] = "table-flag-10_15";
                    e.Row.Cells[7].Text = "<a style='color:red'>" + e.Row.Cells[7].Text + "</a>";
                }
                if (differenceInDays > 15 && differenceInDays <= 20)
                {
                    e.Row.Attributes["class"] = "table-flag-15_20";
                    e.Row.Cells[7].Text = "<a style='color:red'>" + e.Row.Cells[7].Text + "</a>";
                }
                if (differenceInDays > 20 && differenceInDays <= 25)
                {
                    e.Row.Attributes["class"] = "table-flag-20_25";
                    e.Row.Cells[7].Text = "<a style='color:red'>" + e.Row.Cells[7].Text + "</a>";
                }
                if (differenceInDays > 25 && differenceInDays <= 45)
                {
                    e.Row.Attributes["class"] = "table-flag-25";
                    e.Row.Cells[7].Text = "<a style='color:red'>" + e.Row.Cells[7].Text + "</a>";
                }
                if (differenceInDays >= 45)
                {
                    e.Row.Attributes["class"] = "table-flag-45";
                    e.Row.Cells[7].Text = "<a style='color:red'>" + e.Row.Cells[7].Text + "</a>";
                }




                e.Row.Cells[10].Text = differenceInDays.ToString();
                int i = 0;
                try
                {
                    bool result = int.TryParse(e.Row.Cells[3].Text.Split('>')[1].ToString().Replace("</a", "").Trim(), out i);
                }
                catch { }


                e.Row.Cells[24].Visible = false;
                //e.Row.Cells[].Visible = false;
                G_INIT_EXCEL.HeaderRow.Cells[24].Visible = false;



                if (chk_ocultar_txt.Checked)
                {

                    e.Row.Cells[1].Visible = false;
                    //e.Row.Cells[].Visible = false;
                    G_INIT_EXCEL.HeaderRow.Cells[1].Visible = false;

                    e.Row.Cells[2].Visible = false;
                    //e.Row.Cells[].Visible = false;
                    G_INIT_EXCEL.HeaderRow.Cells[2].Visible = false;

                }
                e.Row.Cells[0].Visible = false;
                //e.Row.Cells[].Visible = false;
                G_INIT_EXCEL.HeaderRow.Cells[0].Visible = false;

            }
        }


        protected void G_INIT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool puede_ = true;


                if (busca_columna_fac)
                {
                    try
                    {
                        for (int x = 0; x <= G_INIT.HeaderRow.Cells.Count; x++)
                        {
                            if (G_INIT.HeaderRow.Cells[x].Text.Contains("N&#186;Doc"))
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

                //if (e.Row.Cells[4].Text == "PA" && IsNumeric(e.Row.Cells[3].Text))
                //{
                //    puede_ = false;
                //    e.Row.Visible = false;
                //}
                TextBox tx_peso = ((TextBox)e.Row.FindControl("txt_peso"));
                TextBox tx_dolar = ((TextBox)e.Row.FindControl("txt_dolar"));

                //tx_peso.ClientID = "txt_peso" + e.Row.Cells[COLUMNA_DE_FACTURA].Text;


                if (e.Row.Cells[4].Text == "PA")
                {

                    e.Row.Cells[11].Text = "-" + e.Row.Cells[11].Text;
                    e.Row.Cells[12].Text = "-" + e.Row.Cells[12].Text;
                    e.Row.Cells[13].Text = "-" + e.Row.Cells[13].Text;
                    e.Row.Cells[14].Text = "-" + e.Row.Cells[14].Text;

                    tx_peso.Text = "-" + ((TextBox)e.Row.FindControl("txt_peso")).Text;
                    tx_dolar.Text = "-" + ((TextBox)e.Row.FindControl("txt_dolar")).Text;


                }
                if (e.Row.Cells[4].Text == "CM")
                {

                    e.Row.Cells[11].Text = "-" + e.Row.Cells[11].Text;
                    e.Row.Cells[12].Text = "-" + e.Row.Cells[12].Text;
                    e.Row.Cells[13].Text = "-" + e.Row.Cells[13].Text;
                    e.Row.Cells[14].Text = "-" + e.Row.Cells[14].Text;

                    tx_peso.Text = "-" + ((TextBox)e.Row.FindControl("txt_peso")).Text;
                    tx_dolar.Text = "-" + ((TextBox)e.Row.FindControl("txt_dolar")).Text;

                    if (e.Row.Cells[3].Text.Trim().Substring(0, 1) == "1")
                    {
                        if (e.Row.Cells[3].Text.Trim() != "11737")
                        {
                            puede_ = false;
                            e.Row.Visible = false;
                        }
                    }
                    if (e.Row.Cells[16].Text == "&nbsp;")
                    {
                        e.Row.Cells[14].Text = e.Row.Cells[12].Text;
                    }
                    if (e.Row.Cells[13].Text == "&nbsp;")
                    {
                        e.Row.Cells[13].Text = e.Row.Cells[11].Text;
                    }
                }

                if (puede_)
                {
                    try
                    {
                        sum_peso_saldo += Convert.ToDouble(e.Row.Cells[13].Text.Replace(".", ""));
                    }
                    catch
                    {

                        string error = "aca";
                    }
                    try { sum_dolar_saldo += Convert.ToDouble(e.Row.Cells[14].Text.Replace(".", "")); }
                    catch
                    {
                        string error = "aca";
                    }
                    try { sum_peso_monto += Convert.ToDouble(e.Row.Cells[11].Text.Replace(".", "")); }
                    catch
                    {
                        string error = "aca";
                    }
                    try { sum_dolar_monto += Convert.ToDouble(e.Row.Cells[12].Text.Replace(".", "")); }
                    catch
                    {
                        string error = "aca";
                    }
                }

                cont_rows++;
                if (cont_rows == TOTAL_ROWS)
                {

                    //e.Row.Cells[9].Text = e.Row.Cells[9].Text.Trim().Substring(0, e.Row.Cells[9].Text.Trim().Length - 1);


                    try
                    {
                        G_INIT.HeaderRow.Cells[11].Text = G_INIT.HeaderRow.Cells[11].Text + "  (" + Base.monto_format2(sum_peso_monto).Trim() + ")";
                        G_INIT.HeaderRow.Cells[12].Text = G_INIT.HeaderRow.Cells[12].Text + "  (" + Base.monto_format2(sum_dolar_monto).Trim() + ")";
                        G_INIT.HeaderRow.Cells[13].Text = G_INIT.HeaderRow.Cells[13].Text + "  (" + Base.monto_format2(sum_peso_saldo).Trim() + ")";
                        G_INIT.HeaderRow.Cells[14].Text = G_INIT.HeaderRow.Cells[14].Text + "  (" + Base.monto_format2(sum_dolar_saldo).Trim() + ")";
                    }
                    catch { }
                }


                string tpo_doc_temporal = e.Row.Cells[4].Text.Trim();

                if (e.Row.Cells[20].Text == "SI")
                {

                    e.Row.BackColor = Color.FromArgb(255, 228, 196);
                    //CargarEvento_Tabla(id, rut, fact, tipo_doc)

                    if (e.Row.Cells[4].Text != "PA" && e.Row.Cells[4].Text != "DM" && e.Row.Cells[4].Text != "CM")
                    {
                        string rutcliente = G_INIT.DataKeys[e.Row.RowIndex].Values[0].ToString();
                        string id = G_INIT.DataKeys[e.Row.RowIndex].Values[1].ToString();


                        string script = string.Format("javascript:CargarEvento_Tabla(&#39;{0}&#39;,&#39;{1}&#39;,&#39;{2}&#39;,&#39;{3}&#39;);devul_fal();", id, rutcliente, e.Row.Cells[3].Text, e.Row.Cells[4].Text);
                        e.Row.Cells[4].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[4].Text + " </a>";
                    }
                    else if (e.Row.Cells[4].Text == "DM" /*&& IsNumeric(e.Row.Cells[1].Text)*/)
                    {

                        string rutcliente = G_INIT.DataKeys[e.Row.RowIndex].Values[0].ToString();
                        string id = G_INIT.DataKeys[e.Row.RowIndex].Values[1].ToString();


                        string script = string.Format("javascript:CargarEvento_Tabla(&#39;{0}&#39;,&#39;{1}&#39;,&#39;{2}&#39;,&#39;{3}&#39;);return false;", id, rutcliente, e.Row.Cells[3].Text, e.Row.Cells[4].Text);
                        e.Row.Cells[4].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[4].Text + " </a>";
                    }
                }



                if (e.Row.Cells[23].Text == "&nbsp;")
                {
                    string combo = "";
                    combo = " <select class=\"form-control input-sm\"  style=\"width: 100px;\" id=\"cb_cambio_pago" + e.Row.Cells[3].Text + "\" onchange =\"cambia_tipo_pago3('" + e.Row.Cells[3].Text + "')\"> " +
                                        "                                        <option value = \"\" selected></option>  " +
                                                "                               <option value = \"peso\"> Peso...</option> " +
                                                  "                              <option value=\"dolar\"> Dolar...</option> " +
                                                   "                         </select > ";

                    e.Row.Cells[23].Text = combo;
                }

                if (e.Row.Cells[23].Text == "peso")
                {
                    string combo = "";
                    combo = " <select class=\"form-control input-sm\" style=\"width: 100px;\" id=\"cb_cambio_pago" + e.Row.Cells[3].Text + "\" onchange =\"cambia_tipo_pago3('" + e.Row.Cells[3].Text + "')\"> " +
                                       "                                        <option value = \"\"></option>  " +
                                               "                               <option value = \"peso\" selected> Peso...</option> " +
                                                 "                              <option value=\"dolar\"> Dolar...</option> " +
                                                  "                         </select > ";
                    e.Row.Cells[23].Text = combo;
                }

                if (e.Row.Cells[23].Text == "dolar")
                {
                    string combo = "";
                    combo = " <select class=\"form-control input-sm\"  style=\"width: 100px;\" id=\"cb_cambio_pago" + e.Row.Cells[3].Text + "\" onchange =\"cambia_tipo_pago3('" + e.Row.Cells[3].Text + "')\"> " +
                                      "                                        <option value = \"\"></option>  " +
                                              "                               <option value = \"peso\"> Peso...</option> " +
                                                "                              <option value=\"dolar\" selected> Dolar...</option> " +
                                                 "                         </select > ";

                    e.Row.Cells[23].Text = combo;
                }



                if (e.Row.Cells[9].Text != "0")
                {

                    e.Row.Cells[9].Text = "<a data-toggle='tooltip' style='color:red;' data-placement='top' title='" + e.Row.Cells[25].Text + "'> <img style='width: 50%;' src='img/speech-bubble.png' /></a>";
                }
                else
                {
                    e.Row.Cells[9].Text = "";
                }


                e.Row.Cells[20].Visible = false;
                G_INIT.HeaderRow.Cells[20].Visible = false;

                e.Row.Cells[21].Visible = false;
                G_INIT.HeaderRow.Cells[21].Visible = false;

                e.Row.Cells[22].Visible = false;
                G_INIT.HeaderRow.Cells[22].Visible = false;

                e.Row.Cells[17].Visible = false;
                G_INIT.HeaderRow.Cells[17].Visible = false;

                e.Row.Cells[15].Visible = false;
                G_INIT.HeaderRow.Cells[15].Visible = false;

                e.Row.Cells[25].Visible = false;
                G_INIT.HeaderRow.Cells[25].Visible = false;


                string año_factura = e.Row.Cells[6].Text.Substring(0, 4);
                if (columna_fac)
                {
                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                    string script = string.Format("javascript:fuera3(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[COLUMNA_DE_FACTURA].Text), encriptador.EncryptData(año_factura));
                    e.Row.Cells[COLUMNA_DE_FACTURA].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[COLUMNA_DE_FACTURA].Text + " </a>";

                }


                G_INIT.HeaderRow.Cells[11].Attributes["data-sort-method"] = "number";

                G_INIT.HeaderRow.Cells[6].Attributes["data-sort-method"] = "date";
                G_INIT.HeaderRow.Cells[7].Attributes["data-sort-method"] = "date";
                G_INIT.HeaderRow.Cells[8].Attributes["data-sort-method"] = "date";


                if (e.Row.Cells[8].Text == "1900/01/01")
                {
                    e.Row.Cells[8].Text = "";
                }

                DateTime tras = DateTime.Now;
                DateTime venc = Convert.ToDateTime(e.Row.Cells[7].Text);
                TimeSpan ts = tras - venc;
                int differenceInDays = ts.Days;
                if (differenceInDays <= 0)
                {
                    differenceInDays = 0;
                    //e.Row.Cells[0].BackColor = Color.FromArgb(58, 223, 0, 0.16);
                    e.Row.Attributes["class"] = "table-flag-NOVENCIDOS";
                }

                if (differenceInDays > 0 && differenceInDays <= 5)
                {
                    e.Row.Attributes["class"] = "table-flag-5";
                    e.Row.Cells[7].Text = "<a style='color:red'>" + e.Row.Cells[7].Text + "</a>";

                }
                if (differenceInDays > 5 && differenceInDays <= 10)
                {
                    e.Row.Attributes["class"] = "table-flag-5_10";
                    e.Row.Cells[7].Text = "<a style='color:red'>" + e.Row.Cells[7].Text + "</a>";
                }
                if (differenceInDays > 10 && differenceInDays <= 15)
                {
                    e.Row.Attributes["class"] = "table-flag-10_15";
                    e.Row.Cells[7].Text = "<a style='color:red'>" + e.Row.Cells[7].Text + "</a>";
                }
                if (differenceInDays > 15 && differenceInDays <= 20)
                {
                    e.Row.Attributes["class"] = "table-flag-15_20";
                    e.Row.Cells[7].Text = "<a style='color:red'>" + e.Row.Cells[7].Text + "</a>";
                }
                if (differenceInDays > 20 && differenceInDays <= 25)
                {
                    e.Row.Attributes["class"] = "table-flag-20_25";
                    e.Row.Cells[7].Text = "<a style='color:red'>" + e.Row.Cells[7].Text + "</a>";
                }
                if (differenceInDays > 25 && differenceInDays <= 45)
                {
                    e.Row.Attributes["class"] = "table-flag-25";
                    e.Row.Cells[7].Text = "<a style='color:red'>" + e.Row.Cells[7].Text + "</a>";
                }
                if (differenceInDays >= 45)
                {
                    e.Row.Attributes["class"] = "table-flag-45";
                    e.Row.Cells[7].Text = "<a style='color:red'>" + e.Row.Cells[7].Text + "</a>";
                }







                if (e.Row.Cells[4].Text == "PA" || e.Row.Cells[4].Text == "CM")
                {
                    e.Row.Cells[10].Text = "";
                }
                else
                {
                    e.Row.Cells[10].Text = differenceInDays.ToString();
                }

                int i = 0;
                try
                {
                    bool result = int.TryParse(e.Row.Cells[3].Text.Split('>')[1].ToString().Replace("</a", "").Trim(), out i);
                }
                catch { }


                e.Row.Cells[24].Visible = false;
                //e.Row.Cells[].Visible = false;
                G_INIT.HeaderRow.Cells[24].Visible = false;


                string rutcliente1 = G_INIT.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string id1 = G_INIT.DataKeys[e.Row.RowIndex].Values[1].ToString();
                string factura = G_INIT.DataKeys[e.Row.RowIndex].Values[5].ToString();
                string doc = G_INIT.DataKeys[e.Row.RowIndex].Values[4].ToString();
                //string saldo_peso = G_INIT.DataKeys[e.Row.RowIndex].Values[2].ToString();
                string saldo_peso = G_INIT.DataKeys[e.Row.RowIndex].Values[7].ToString();

                if (tpo_doc_temporal == "IN")
                {
                    string script23 = string.Format("javascript:comision(&#39;{0}&#39;,&#39;{1}&#39;,&#39;{2}&#39;,&#39;{3}&#39;,&#39;{4}&#39;);", id1, rutcliente1, factura.Trim(), doc.Trim(), saldo_peso.Trim());
                    e.Row.Cells[26].Text = "  <a href='javascript:' onclick='" + script23 + "'><img  style='width: 30%;cursor:pointer;' src='img/search_page.png' /></a>";
                }
                else
                {
                    e.Row.Cells[26].Text = "";

                }

                if (chk_ocultar_txt.Checked)
                {

                    e.Row.Cells[1].Visible = false;
                    //e.Row.Cells[].Visible = false;
                    G_INIT.HeaderRow.Cells[1].Visible = false;

                    e.Row.Cells[2].Visible = false;
                    //e.Row.Cells[].Visible = false;
                    G_INIT.HeaderRow.Cells[2].Visible = false;

                }

            }
        }

        private bool IsNumeric(string s)
        {
            float output;
            return float.TryParse(s, out output);
        }

        //public bool IsNumeric(this string s)
        //{
        //    float output;
        //    return float.TryParse(s, out output);
        //}

        protected void chkAccept_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void btn_cobrar_Click(object sender, EventArgs e)
        {

            if (t_cobro.Text != "")
            {

                string fecha_cobro = t_cobro.Text;
                string obs_cobro = t_ob_cobro.Value;
                string num_factura = "";
                List<string> folios = new List<string>();
                string si_insert = "";
                bool trues = false;
                foreach (GridViewRow dtgItem in this.G_INIT.Rows)
                {
                    CheckBox Sel = ((CheckBox)G_INIT.Rows[dtgItem.RowIndex].FindControl("chkAccept"));
                    bool valor = Sel.Checked;
                    if (valor)
                    {

                        num_factura = G_INIT.Rows[dtgItem.RowIndex].Cells[1].Text.Split('>')[1].ToString().Replace("</a", "").Trim();
                        string q = ReporteRNegocio.delete_fecha_cobro(num_factura);
                        si_insert = ReporteRNegocio.insert_fecha_cobra(num_factura, fecha_cobro, obs_cobro);
                        ReporteRNegocio.insert_seguimiento(num_factura);
                        insert_accion_temporal(num_factura);
                    }
                    else
                    {
                    }
                }
                if (si_insert == "OK")
                {

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>alert('Cobro Agendado');cambia_sw2();</script>", false);
                }
            }
        }

        private string insert_accion_temporal(string num_)
        {
            try
            {
                DBUtil db = new DBUtil();
                DataTable dt = new DataTable();
                string query = "";

                query += "INSERT INTO COBRANZA_ACCIONES VALUES ( ";
                query += "@id_cobranza, ";
                query += "@id_ACCION, ";
                query += "CONVERT(datetime, @_fecha_accion, 103), ";
                query += "@usuario, ";
                query += "@obs ";
                query += ");select scope_identity(); ";

                List<SPVars> vars = new List<SPVars>();
                vars.Add(new SPVars() { nombre = "id_cobranza", valor = num_ });
                vars.Add(new SPVars() { nombre = "id_accion", valor = 5 });
                vars.Add(new SPVars() { nombre = "usuario", valor = HttpContext.Current.Session["user"].ToString() });
                vars.Add(new SPVars() { nombre = "_fecha_accion", valor = DateTime.Now.ToString() });
                vars.Add(new SPVars() { nombre = "obs", valor = "Cobro reagendado" });

                string id = db.Scalar2(query, vars).ToString();
                accion_prioridad(id, num_);
                return "OK";
            }
            catch (Exception ex)
            {
                return "Error : " + ex.Message.ToString();
            }
        }

        private string insert_accion_pago_estimado(string num_)
        {
            try
            {
                DBUtil db = new DBUtil();
                DataTable dt = new DataTable();
                string query = "";

                query += "INSERT INTO COBRANZA_ACCIONES VALUES ( ";
                query += "@id_cobranza, ";
                query += "@id_ACCION, ";
                query += "CONVERT(datetime, @_fecha_accion, 103), ";
                query += "@usuario, ";
                query += "@obs ";
                query += ") ;select scope_identity();";

                List<SPVars> vars = new List<SPVars>();
                vars.Add(new SPVars() { nombre = "id_cobranza", valor = num_ });
                vars.Add(new SPVars() { nombre = "id_accion", valor = 4 });
                vars.Add(new SPVars() { nombre = "usuario", valor = HttpContext.Current.Session["user"].ToString() });
                vars.Add(new SPVars() { nombre = "_fecha_accion", valor = DateTime.Now.ToString("dd/MM/yyyy") });
                vars.Add(new SPVars() { nombre = "obs", valor = "Estimado a pagar (aut.)" });

                string id = db.Scalar2(query, vars).ToString();
                accion_prioridad(id, num_);

                return "OK";
            }
            catch (Exception ex)
            {
                return "Error : " + ex.Message.ToString();
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void btn_excel2_Click(object sender, EventArgs e)
        {
            G_INIT_EXCEL.DataSource = (DataTable)Session["aux_dt_excel"];
            G_INIT_EXCEL.DataBind();

            Response.Clear();

            Response.AddHeader("content-disposition", "attachment;filename=Doc_Abiertos_" + DateTime.Now.ToShortDateString() + ".xls");

            Response.Charset = "";

            // If you want the option to open the Excel file without saving than

            // comment out the line below

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.ContentType = "application/vnd.xls";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);
            G_INIT_EXCEL.Visible = true;

            //this.G_INIT_EXCEL.Columns[6].Visible = false;
            //G_INIT_EXCEL.HeaderRow.Cells[13].Attributes["style"] = "display:none;";
            //G_INIT_EXCEL.Row.Cells[13].Attributes["style"] = "display:none;";

            G_INIT_EXCEL.RenderControl(htmlWrite);

            string excel_dep = stringWrite.ToString();
            //string excel_dep = stringWrite.ToString().Replace("<a href='javascript:' onclick='javascript:", "<p>").Replace("</a>", "</p>").Replace("<select class", "<p class").Replace("</select >", "</p >").Replace("<option value = \"\"></option>", "").Replace("<option value = \"peso\"> Peso...</option>", "").Replace("<option value=\"dolar\"> Dolar...</option>", "").Replace("style=\"width: 15px;\"", "style=\"width:15px;display:none\"").Replace("<td align=\"center\">", "<td align=\"center\" style=\"display:none; \">");

            //while (1 < 2)
            //{
            //    try
            //    {
            //        int in2 = excel_dep.ToString().IndexOf("<p>fuera3(");
            //        int in3 = excel_dep.ToString().IndexOf("false");
            //        if (in2 == -1 && in3 == -1)
            //        {
            //            break;
            //        }
            //        else

            //        {
            //            excel_dep = excel_dep.Remove((in2 + 1), (in3 + 3) - in2);
            //        }


            //    }
            //    catch
            //    {
            //        break;
            //    }
            //}
            //while (1 < 2)
            //{
            //    try
            //    {
            //        int in2 = excel_dep.ToString().IndexOf("<p>CargarEvento_Tabla(");
            //        int in3 = excel_dep.ToString().IndexOf("devul_fal()");
            //        if (in2 == -1 && in3 == -1)
            //        {
            //            break;
            //        }
            //        else

            //        {
            //            excel_dep = excel_dep.Remove((in2 + 1), (in3 + 1) - in2);
            //        }


            //    }
            //    catch
            //    {
            //        break;
            //    }
            //}

            ////excel_dep = excel_dep.Replace("<th scope=\"col\">&nbsp;</th>", "");
            //while (1 < 2)
            //{
            //    try
            //    {
            //        int in2 = excel_dep.ToString().IndexOf("<input id=\"chkAccept\"");
            //        int in3 = excel_dep.ToString().IndexOf("$chkAccept");
            //        if (in2 == -1 && in3 == -1)
            //        {
            //            break;
            //        }
            //        else

            //        {
            //            excel_dep = excel_dep.Remove((in2 + -3), (in3 + 5) - in2);
            //        }


            //    }
            //    catch
            //    {
            //        break;
            //    }
            //}
            //excel_dep = excel_dep.Replace("hkAccept\" />", "");


            Response.Write(excel_dep);

            //<option value = "" selected>



            Response.End();
            G_INIT_EXCEL.Visible = false;

        }

        private void LlenarGrilla2()
        {

            //if (txt_desde2.Text != "" || txt_hasta2.Text != "" || num_docum.Text != "")
            //{
            busca_columna_fac = true;
            columna_fac = false;

            string tipo_doc = "";
            foreach (ListItem item in CB_TIPO_DOC_GRILLA.Items)
            {

                if (item.Selected)
                {
                    tipo_doc += item.Value + ", ";
                }

            }

            if (tipo_doc == "-1")
            {
                tipo_doc = "";
            }
            else if (tipo_doc == "")
            {
                tipo_doc = "";
            }
            else
            {


                string aux_tipo = tipo_doc;
                aux_tipo = aux_tipo.Substring(0, aux_tipo.Length - 2);
                tipo_doc = tipo_doc.Substring(0, tipo_doc.Length - 2);
                tipo_doc = " and nombre_tipo IN ( " + agregra_comillas(aux_tipo) + ")";

            }



            if (L_CLIENTES.Text != "")
            {
                tipo_doc += " and rutcliente in (" + agregra_comillas(L_CLIENTES.Text) + ") ";
            }


            if (L_VENDEDORES.Text != "")
            {
                tipo_doc += " and VENDEDOR in (" + agregra_comillas(L_VENDEDORES.Text) + ") ";
            }
            //if (CB_VENDEDOR_GRILLA.SelectedValue != null && CB_VENDEDOR_GRILLA.SelectedValue != "-1")
            //{
            //    tipo_doc += " and VENDEDOR = '" + CB_VENDEDOR_GRILLA.SelectedValue.ToString() + "'";
            //}



            if (rd_ven.Checked)
            {
                if (txt_desde2.Text != "")
                {

                    tipo_doc += " and fecha_venc >= convert(datetime, '" + txt_desde2.Text + "', 103) ";
                }
                if (txt_hasta2.Text != "")
                {
                    tipo_doc += " and fecha_venc <= convert(datetime, '" + txt_hasta2.Text + "', 103) ";

                }
            }
            else
            {
                if (txt_desde2.Text != "")
                {

                    tipo_doc += " and fecha_trans >= convert(datetime, '" + txt_desde2.Text + "', 103) ";
                }
                if (txt_hasta2.Text != "")
                {
                    tipo_doc += " and fecha_trans <= convert(datetime, '" + txt_hasta2.Text + "', 103) ";

                }
            }

            if (num_docum.Text != "")
            {
                tipo_doc += " and factura in (" + agregra_comillas(num_docum.Text) + ") ";

            }


            //if (CB_CLIENTE_GRILLA.SelectedValue != null && CB_CLIENTE_GRILLA.SelectedValue != "-1")
            //{
            //    tipo_doc += " and rutcliente = '" + CB_CLIENTE_GRILLA.SelectedValue.ToString() + "'";
            //}


            sum_peso_saldo = 0;

            sum_dolar_saldo = 0;

            sum_peso_monto = 0;

            sum_dolar_monto = 0;

            cont_rows = 0;

            DataTable au = new DataTable();
            if (rd_abi.Checked)
            {

                au = ReporteRNegocio.trae_docu_calend(tipo_doc, User.Identity.Name);
                TOTAL_ROWS = au.Rows.Count;
                G_INIT_EXCEL.DataSource = au;
                G_INIT_EXCEL.DataBind();
                cerrad_abier.Visible = true;
            }
            else
            {

                au = ReporteRNegocio.trae_docu_calend_CERRADOS(tipo_doc, User.Identity.Name);
                TOTAL_ROWS = au.Rows.Count;
                G_INIT_EXCEL.DataSource = au;
                G_INIT_EXCEL.DataBind();
                cerrad_abier.Visible = false;
            }



            //DataTable au = ReporteRNegocio.trae_docu_calend(tipo_doc);
            //TOTAL_ROWS = au.Rows.Count;
            //G_INIT.DataSource = au;
            //G_INIT.DataBind();



            //var elem3 = document.getElementById("ocultar_principio");
            //elem3.style.display = "block";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "tweeee", "<script> var elem3 = document.getElementById(\"ocultar_principio\");    elem3.style.display = \"block\";</script>", false);
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_PRODUCTOS')); </script>", false);
            //}
            //else
            //{

            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "tq12eeee", "<script> alert('Sin filtro fecha') </script>", false);

            //}




        }

        protected void G_MOV_SOL_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (e.Row.Cells[7].Text == "DM")
                {
                    try
                    {
                        e.Row.Cells[10].Text = e.Row.Cells[10].Text.Substring(0, e.Row.Cells[10].Text.IndexOf(","));
                    }
                    catch { }
                }
                G_MOV_SOL.HeaderRow.Cells[2].Attributes["data-sort-method"] = "number";
                G_MOV_SOL.HeaderRow.Cells[10].Attributes["data-sort-method"] = "date";
                G_MOV_SOL.HeaderRow.Cells[11].Attributes["data-sort-method"] = "date";

                G_MOV_SOL.HeaderRow.Cells[2].CssClass = "sort-default sort-header sort-down";
                if (e.Row.Cells[7].Text == "PA-F" && Convert.ToDateTime(e.Row.Cells[11].Text) <= Convert.ToDateTime(DateTime.Today.ToShortTimeString()))
                {
                    e.Row.Visible = false;

                }
                else
                {
                    if (e.Row.Cells[13].Text == "0")
                    {
                        e.Row.Attributes["class"] = "table-flag-red";

                    }
                    else
                    {
                        //e.Row.BackColor = Color.FromArgb(195, 247, 75);
                        e.Row.Attributes["class"] = "table-flag-green doc_ingresado";
                    }

                    if (busca_columna_fac)
                    {
                        try
                        {
                            for (int x = 0; x <= G_MOV_SOL.HeaderRow.Cells.Count; x++)
                            {
                                if (G_MOV_SOL.HeaderRow.Cells[x].Text == ("N&#186; DOCUMENTO"))
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
                    e.Row.Cells[11].Text = e.Row.Cells[11].Text.Substring(0, e.Row.Cells[11].Text.IndexOf(" "));

                    e.Row.Cells[12].Text = e.Row.Cells[12].Text.Substring(0, e.Row.Cells[12].Text.IndexOf(" "));


                    string año_factura = "";
                    try
                    {
                        año_factura = e.Row.Cells[18].Text.Trim();
                    }
                    catch { }
                    if (columna_fac)
                    {
                        clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                        string script = string.Format("javascript:fuera3(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[COLUMNA_DE_FACTURA].Text), encriptador.EncryptData(año_factura));
                        e.Row.Cells[COLUMNA_DE_FACTURA].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[COLUMNA_DE_FACTURA].Text + " </a>";
                    }
                    G_MOV_SOL.HeaderRow.Cells[3].Attributes["data-sort-method"] = "number";
                    //double d;
                    //double.TryParse(e.Row.Cells[3].Text.Substring(0, e.Row.Cells[3].Text.IndexOf(",")), out d);
                    //string aux = "";
                    //if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                    //e.Row.Cells[3].Text = aux;

                    try
                    {
                        double d3;
                        double.TryParse(e.Row.Cells[3].Text.Substring(0, e.Row.Cells[3].Text.IndexOf(",")), out d3);
                        string aux3 = "";
                        if (d3 == 0) { aux3 = ""; } else { aux3 = d3.ToString("N0"); }
                        e.Row.Cells[3].Text = aux3 + e.Row.Cells[3].Text.Substring(e.Row.Cells[3].Text.IndexOf(","));
                    }
                    catch
                    {
                        double d4;
                        double.TryParse(e.Row.Cells[3].Text, out d4);
                        string aux4 = "";
                        if (d4 == 0) { aux4 = ""; } else { aux4 = d4.ToString("N0"); }
                        e.Row.Cells[3].Text = aux4;
                    }
                    string rut_ini = e.Row.Cells[4].Text.Trim().Substring(0, e.Row.Cells[4].Text.Trim().Length - 1);
                    double rut = 0;
                    try { rut = double.Parse(rut_ini); e.Row.Cells[4].Text = rut.ToString("N0") + "-" + e.Row.Cells[4].Text.Trim().Substring(e.Row.Cells[4].Text.Trim().Length - 1); }
                    catch { rut = double.Parse(e.Row.Cells[4].Text); e.Row.Cells[4].Text = rut.ToString("N0"); }
                }

                ///eliminar pago directo 
                ///
                if (e.Row.Cells[6].Text == "PAGO DIRECTO" || e.Row.Cells[6].Text == "NOTA_CREDITO" || e.Row.Cells[6].Text == "SALDO_FAVOR")
                {
                    string script22 = string.Format("eliminar_pago_directo(&#39;{0}&#39;);", e.Row.Cells[1].Text);
                    string html_eliminar = "<a style='background-color: rgb(255, 97, 97);' class=\"btn btn-circle show-tooltip fa fa-trash\" onclick=\"" + script22 + "\"></a>";
                    e.Row.Cells[23].Text = html_eliminar;
                }
                else
                {

                }

                if (e.Row.Cells[8].Text.Contains("NET") && e.Row.Cells[6].Text == "ABONO")
                {
                    e.Row.Cells[3].Text = "NETEO";
                }

                e.Row.Cells[3].Text = e.Row.Cells[3].Text.Replace(",000", "");

                try
                {

                    e.Row.Cells[13].Visible = false;
                    G_MOV_SOL.HeaderRow.Cells[13].Visible = false;
                    e.Row.Cells[14].Visible = false;
                    G_MOV_SOL.HeaderRow.Cells[14].Visible = false;
                    e.Row.Cells[15].Visible = false;
                    G_MOV_SOL.HeaderRow.Cells[15].Visible = false;
                    e.Row.Cells[16].Visible = false;
                    G_MOV_SOL.HeaderRow.Cells[16].Visible = false;
                    e.Row.Cells[17].Visible = false;
                    G_MOV_SOL.HeaderRow.Cells[17].Visible = false;
                    e.Row.Cells[18].Visible = false;
                    G_MOV_SOL.HeaderRow.Cells[18].Visible = false;
                    e.Row.Cells[19].Visible = false;
                    G_MOV_SOL.HeaderRow.Cells[19].Visible = false;

                }
                catch { }

            }
        }

        protected void btn_excel_3_Click(object sender, EventArgs e)
        {
            Response.Clear();

            Response.AddHeader("content-disposition", "attachment;filename=Ingresar_a_Solomon_" + DateTime.Now.ToShortDateString() + ".xls");

            Response.Charset = "";

            // If you want the option to open the Excel file without saving than

            // comment out the line below

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.ContentType = "application/vnd.xls";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);

            G_MOV_SOL.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());

            Response.End();
        }


        protected void btn_mov_sol_Click(object sender, EventArgs e)
        {
            //    //busca_columna_fac = true;
            //    //columna_fac = false;
            //    //G_MOV_SOL.Visible = true;
            //    //G_MOV_SOL.DataSource = ReporteRNegocio.trae_acciones_();
            //    //G_MOV_SOL.DataBind();
            //    ////ScriptManager.RegisterStartupScript(Page, this.GetType(), "tweeee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_MOV_SOL')); </script>", false);
            //    MOV_SOL.Visible = true;
            //    //ScriptManager.RegisterStartupScript(Page, this.GetType(), "tqweeee", "<script>cierra_gif();</script>", false);
            //    ////gif_del_info.Attributes["style"] = "display:none";
            //    btn_mov_sol.Attributes["style"] = "display:none";

            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "tqweeee", "<script>cierra_gif();</script>", false);
        }

        protected void btn_listos_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "sqqwe", "<script language='javascript'>cambia_sw2();</script>", false);

            string num_factura = "";
            string ESTADO = "";
            string id = "";
            string si_update = "";
            foreach (GridViewRow dtgItem in this.G_MOV_SOL.Rows)
            {
                CheckBox Sel = ((CheckBox)G_MOV_SOL.Rows[dtgItem.RowIndex].FindControl("chkAccept2"));
                bool valor = Sel.Checked;
                if (valor)
                {

                    num_factura = G_MOV_SOL.Rows[dtgItem.RowIndex].Cells[2].Text.Split('>').ToList()[1].ToString().Replace("/a>", "").Replace("</a", "").Trim();
                    ESTADO = G_MOV_SOL.Rows[dtgItem.RowIndex].Cells[6].Text;
                    id = G_MOV_SOL.Rows[dtgItem.RowIndex].Cells[1].Text;
                    si_update = ReporteRNegocio.update_estado_ingresado(num_factura, ESTADO, id);
                }
                else
                {
                }
            }
            if (si_update == "OK")
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>Ingresar_Solomon();</script>", false);
                btn_mov_sol_Click(sender, e);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>alert('Ingresados y actualizados saldos')</script>", false);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "tee2ee", "<script language='javascript'>    $('#ContentPlaceHolder_Contenido_btn_filtra_mov').click(); </script>", false);

                //click a boton actualiza saldo
                btn_actualizar_saldos_Click(sender, e);
                relojito_false();
            }
        }

        protected void btn_filtra_grilla_Click(object sender, EventArgs e)
        {

            LlenarGrilla();
            montos_totales.Visible = false;
            cobranza2.Visible = false;
            G_FACTURAS_PAGABLES.Visible = false;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "tqeeee", "<script language='javascript'>Tabla();</script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "sqwe", "<script language='javascript'>cambia_sw2();</script>", false);

        }

        protected void btn_estimado_Click(object sender, EventArgs e)
        {


            if (t_cobro.Text != "")
            {

                string fecha_cobro = t_cobro.Text;
                string obs_cobro = t_ob_cobro.Value;
                string num_factura = "";
                List<string> folios = new List<string>();
                string si_insert1 = "";
                string si_insert2 = "";
                string si_insert3 = "";

                bool trues = false;
                bool es_peso = true;
                string moneda = cb_tipo_pago_1.Value;
                if (moneda != "peso")
                {
                    es_peso = false;
                }

                foreach (GridViewRow dtgItem in this.G_INIT.Rows)
                {
                    CheckBox Sel = ((CheckBox)G_INIT.Rows[dtgItem.RowIndex].FindControl("chkAccept"));
                    bool valor = Sel.Checked;
                    if (valor)
                    {
                        //acaaaa estimar


                        num_factura = G_INIT.DataKeys[dtgItem.RowIndex].Values[5].ToString().Trim();
                        si_insert1 = Seguimiento(num_factura);
                        //string delete_agendado = ReporteRNegocio.delete_agendado(num_factura);
                        //string monto_peso = G_INIT.Rows[dtgItem.RowIndex].Cells[10].Text.Replace(".", "").Replace(",", "");
                        //string monto_dolar = G_INIT.Rows[dtgItem.RowIndex].Cells[11].Text.Replace(".", "").Replace(",", ".");
                        string monto_peso = "";
                        string monto_dolar = "";
                        if (chk_ocultar_txt.Checked)
                        {

                            monto_peso = G_INIT.DataKeys[dtgItem.RowIndex].Values[2].ToString().Trim().Replace(".", "");
                            monto_dolar = G_INIT.DataKeys[dtgItem.RowIndex].Values[3].ToString().Trim().Replace(".", "");

                        }
                        else
                        {

                            monto_peso = ((TextBox)G_INIT.Rows[Convert.ToInt32(dtgItem.RowIndex)].FindControl("txt_peso")).Text.Replace(".", "");
                            monto_dolar = ((TextBox)G_INIT.Rows[Convert.ToInt32(dtgItem.RowIndex)].FindControl("txt_dolar")).Text.Replace(".", "");
                        }

                        if (es_peso)
                        {
                            si_insert2 = Registrar_Pago_efectivo(num_factura + "***" + t_cobro.Text, monto_peso, moneda, G_INIT.Rows[dtgItem.RowIndex].Cells[2].Text, "Estimado a pagar (Aut.)", "no");

                        }
                        else
                        {
                            si_insert2 = Registrar_Pago_efectivo(num_factura + "***" + t_cobro.Text, monto_dolar, moneda, G_INIT.Rows[dtgItem.RowIndex].Cells[2].Text, "Estimado a pagar (Aut.)", "no");

                        }


                        si_insert3 = insert_accion_pago_estimado(num_factura);


                    }
                    else
                    {
                    }
                }

                if (si_insert1 == "OK" && si_insert2 == "OK" && si_insert3 == "OK")
                {
                    LlenarGrilla();
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "ALERT_ESTIMADOS_ACOBRAR", "<script language='javascript'>alert('Estimados a cobrar'); Tabla();</script>", false);
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "sqwe", "<script language='javascript'>cambia_sw2();</script>", false);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "ALERT_ESTIMADOS_ACOBRAR", "<script language='javascript'>alert('error: " + si_insert1 + si_insert2 + si_insert3 + "'); Tabla();</script>", false);
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "sqwe", "<script language='javascript'>cambia_sw2();</script>", false);
                }

                relojito_false();
            }
        }

        protected void btn_estimados_report_CliReporte_Estimadosck(object sender, EventArgs e)
        {

        }

        protected void btn_detalle_Click(object sender, EventArgs e)
        {
            div_detalle.Visible = true;

            DataTable semana_elegida = obtener_dias_desde_hasta();
            llenarcombo_dias(semana_elegida);

        }

        private void llenarcombo_dias(DataTable dt)
        {
            dt.Rows.Add(new Object[] { "-1", "-- Seleccione --" });

            CB_DIAS_ELEGIDOS.DataSource = dt;
            CB_DIAS_ELEGIDOS.DataValueField = "Fecha";
            CB_DIAS_ELEGIDOS.DataTextField = "Dia";
            CB_DIAS_ELEGIDOS.DataBind();
            CB_DIAS_ELEGIDOS.SelectedValue = "-1";
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

                DateTime aux1 = Convert.ToDateTime(desde, new CultureInfo("es-ES"));
                string dia_sema = dia_semana_ingles(aux1.DayOfWeek.ToString());


                au_dias.Rows.Add(new Object[] { desde, dia_sema });

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

        protected void G_DETALLE_ESTIMADOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void G_DETALLE_CHEQUES_CARTERA_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btn_filtra_mov_Click(object sender, EventArgs e)
        {
            busca_columna_fac = true;
            columna_fac = false;
            G_MOV_SOL.Visible = true;
            string where = "";

            if (t_desde_mov.Text != "")
            {
                where += " and fechaevento >= convert(datetime, '" + t_desde_mov.Text + "',103)";

            }
            if (t_hasta_mov.Text != "")
            {
                where += " and fechaevento <= convert(datetime, '" + t_hasta_mov.Text + "',103)";
            }

            G_MOV_SOL.DataSource = ReporteRNegocio.trae_acciones_(where);
            G_MOV_SOL.DataBind();
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "tweeee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_MOV_SOL')); </script>", false);
            MOV_SOL.Visible = true;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "tqweeqwqqee", "<script>cierra_gif();</script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "www1q121", "<script>sss();</script>", false);
            relojito_false();
            //gif_del_info.Attributes["style"] = "display:none";
            //btn_mov_sol.Attributes["style"] = "display:none";
            //oculta_cuando_mov.Attributes["style"] = "display:block";
        }
        protected void INICIAR_COBRANZA_Click(object sender, EventArgs e)
        {

            //BTN_NETEO.Visible = false;
            BTN_NETEO_2.Visible = false;
            P_FECHA_NET.Visible = false;
            string tabla_documentos = "";
            string rut_cliente = "";
            string si_insert = "";
            bool trues = true;
            string num_factura = "";
            fact_sele.Text = "";
            double sum_peso = 0;
            double sum_dolar = 0;
            cont_pagables = 0;
            HttpContext.Current.Session["cont_cheq2"] = 0;
            cont_facturas = 0;
            DataTable facturas_pagables = new DataTable();
            HttpContext.Current.Session["inception_prah"] = null;
            facturas_pagables.Columns.Add("factura");
            facturas_pagables.Columns.Add("saldo_peso");
            facturas_pagables.Columns.Add("saldo_dolar");
            facturas_pagables.Columns.Add("sw_abono");
            facturas_pagables.Columns.Add("tipo_doc");
            facturas_pagables.Columns.Add("tasacambio");
            facturas_pagables.Columns.Add("fvenc");
            facturas_pagables.Columns.Add("saldo_final_peso");
            facturas_pagables.Columns.Add("saldo_final_dolar");
            facturas_pagables.Columns.Add("facturas_aplicadas");
            facturas_pagables.Columns.Add("tipo_moneda");

            facturas_pagables.Columns.Add("rutcliente");

            tabla_documentos += "<table class=\"table fill-head table-bordered\">";
            tabla_documentos += "<thead class=\"test\">";
            tabla_documentos += "<tr>";

            tabla_documentos += "<th>NºDoc</th>";
            tabla_documentos += "<th>TDoc</th>";
            tabla_documentos += "<th>SaldoPeso</th>";
            tabla_documentos += "<th>SaldoDolar</th>";
            tabla_documentos += "<th>FVenc</th>";
            tabla_documentos += "<th>TasaCambio</th>";

            tabla_documentos += "</tr>";
            tabla_documentos += "</thead>";
            tabla_documentos += "<tbody>";

            cn_fact = 0;

            Boolean existe_nota_credito = false;
            Boolean tasa_cambio_iguales_bool = false;

            string moneda_cm = "";
            string tasas_cambio_cm = "";
            string tipo_cambio_iguales = "";
            double tasa_Cambio_q = 0;
            foreach (GridViewRow dtgItem in this.G_INIT.Rows)
            {
                CheckBox Sel = ((CheckBox)G_INIT.Rows[dtgItem.RowIndex].FindControl("chkAccept"));
                bool valor = Sel.Checked;

                if (valor)
                {
                    //INICIAR COBRANZA ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    DataRow row;
                    row = facturas_pagables.NewRow();

                    string tipo_moneda = G_INIT.DataKeys[dtgItem.RowIndex].Values[9].ToString().Trim();

                    if (trues)
                    { rut_cliente = G_INIT.DataKeys[dtgItem.RowIndex].Values[0].ToString(); trues = false; }

                    string tipo_doc = G_INIT.DataKeys[dtgItem.RowIndex].Values[4].ToString().Trim();
                    string num_fact = G_INIT.DataKeys[dtgItem.RowIndex].Values[5].ToString().Trim();


                    ///TASA CAMBIO 
                    string tasa_cambio = G_INIT.DataKeys[dtgItem.RowIndex].Values[8].ToString().Trim();

                    try
                    {
                        tasa_Cambio_q += Convert.ToDouble(tasa_cambio);
                    }
                    catch
                    {

                    }

                    if (tasa_cambio != tipo_cambio_iguales)
                    {
                        tasa_cambio_iguales_bool = false;
                        tipo_cambio_iguales = G_INIT.DataKeys[dtgItem.RowIndex].Values[8].ToString().Trim();
                    }
                    else
                    {
                        tasa_cambio_iguales_bool = true;
                    }


                    string peso = "";
                    string dolar = "";
                    if (tipo_doc == "CM" || (tipo_doc == "PA" && !IsNumeric(num_fact)))
                    {
                        peso = "-" + G_INIT.DataKeys[dtgItem.RowIndex].Values[2].ToString(); trues = false;
                        dolar = "-" + G_INIT.DataKeys[dtgItem.RowIndex].Values[3].ToString(); trues = false;
                        existe_nota_credito = true;
                        moneda_cm = tipo_moneda;
                        tasas_cambio_cm = tasa_cambio;
                    }
                    else
                    {
                        peso = G_INIT.DataKeys[dtgItem.RowIndex].Values[2].ToString(); trues = false;
                        dolar = G_INIT.DataKeys[dtgItem.RowIndex].Values[3].ToString(); trues = false;
                    }

                    double tasa_cmb;
                    double.TryParse(tasa_cambio, out tasa_cmb);

                    // se modifica valores para aplicar diferente saldo
                    string valors_peso = "";
                    string valors_dolar = "";
                    if (chk_ocultar_txt.Checked)
                    {
                        valors_peso = peso.Replace(".", "");
                        valors_dolar = dolar.Replace(".", "");
                    }
                    else
                    {

                        valors_peso = ((TextBox)G_INIT.Rows[Convert.ToInt32(dtgItem.RowIndex)].FindControl("txt_peso")).Text.Replace(".", "");

                        string valor_dolar = ((TextBox)G_INIT.Rows[Convert.ToInt32(dtgItem.RowIndex)].FindControl("txt_dolar")).Text;
                        if (valor_dolar.Contains(',') && valor_dolar.Contains('.'))
                        {
                            valor_dolar = valor_dolar.Replace(".", "");
                            valors_dolar = valor_dolar;
                        }
                        else if (valor_dolar.Contains('.'))
                        {
                            valor_dolar = valor_dolar.Replace(".", ",");
                            valors_dolar = valor_dolar;
                        }
                        else
                        {
                            valors_dolar = valor_dolar;
                        }



                    }




                    if (tipo_doc == "IN" || tipo_doc == "DM" || tipo_doc == "CM" || (tipo_doc == "PA" && !IsNumeric(num_fact)))
                    {
                        if (tipo_doc == "IN" || tipo_doc == "DM")
                        {
                            cont_pagables++;
                        }

                        row[0] = num_fact;
                        row[1] = valors_peso;
                        row[2] = valors_dolar;
                        if ((valors_peso != peso.Replace(".", "")) || (valors_dolar != dolar.Replace(".", "")))
                        {
                            row[3] = "A";
                        }
                        else
                        {
                            row[3] = "Z";
                        }
                        row[4] = tipo_doc;
                    }
                    else
                    {
                        fact_sele.Text += num_fact + "--";
                    }
                    //if (rut_cliente != G_INIT.DataKeys[dtgItem.RowIndex].Values[0].ToString()) { fact_sele.Text = ""; si_insert = "Error"; ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>alert('Mas de un Cliente');Tabla();</script>", false); break; }
                    num_factura = num_fact;
                    si_insert += ReporteRNegocio.insert_seguimiento(num_factura);
                    sum_peso += Convert.ToDouble(valors_peso.Replace(".", ""));
                    try
                    {
                        sum_dolar += Convert.ToDouble(valors_dolar.Replace(".", ""));
                    }
                    catch { /*sum_dolar += Convert.ToDouble(ReporteRNegocio.saldo_dolar_multiguia(num_factura));*/ }

                    tabla_documentos += "<tr>";
                    tabla_documentos += "<td>" + click_factura(num_fact) + "</td>";
                    tabla_documentos += "<td>" + tipo_doc + "</td>";
                    //tabla_documentos += "<td>" + ReporteRNegocio.nombre_cliente(rut_cliente) + "</td>";
                    tabla_documentos += "<td>" + Base.monto_format(valors_peso) + "</td>";
                    tabla_documentos += "<td>" + Base.monto_format(valors_dolar) + "</td>";


                    tabla_documentos += "<td>" + G_INIT.DataKeys[dtgItem.RowIndex].Values[6].ToString().Trim() + "</td>";
                    tabla_documentos += "<td>" + G_INIT.DataKeys[dtgItem.RowIndex].Values[8].ToString().Trim() + "</td>";
                    row[5] = G_INIT.DataKeys[dtgItem.RowIndex].Values[8].ToString().Trim();
                    row[6] = G_INIT.DataKeys[dtgItem.RowIndex].Values[6].ToString().Trim();
                    row[7] = valors_peso;
                    row[8] = valors_dolar;
                    row[9] = "";
                    row[10] = tipo_moneda;
                    row[11] = rut_cliente;

                    facturas_pagables.Rows.Add(row);

                    tabla_documentos += "</tr>";

                }
                else
                {
                }
            }
            tabla_documentos += "</tbody>";
            tabla_documentos += "</table>";




            if (existe_nota_credito)
            {
                Session["existe_nota_credito"] = true;
                btn_pagables.Visible = true;
                btn_pagables.Enabled = true;
                btn_pagables.Text = "ASIGNAR NOTA CREDITO";
            }
            else
            {
                Session["existe_nota_credito"] = false;
            }

            if (tasa_cambio_iguales_bool)
            {
                Session["tipo_cambio_iguales"] = true;
            }
            else
            {
                Session["tipo_cambio_iguales"] = false;
            }

            DataView dv = facturas_pagables.DefaultView;
            dv.Sort = "sw_abono ASC,  SALDO_PESO asc";
            facturas_pagables = dv.ToTable();

            foreach (DataRow row_1 in facturas_pagables.Rows)
            {
                if (existe_nota_credito)
                {
                    string aux_dolar_texto = row_1[2].ToString();

                    double tas_cambio_cm = Convert.ToDouble(tasas_cambio_cm);
                    double saldo_peso = Convert.ToDouble(row_1[1].ToString()); ;

                    if (row_1[3].ToString() == "A")
                    {

                        float saldo_dolar = float.Parse(aux_dolar_texto);


                        if (moneda_cm.Contains("PESO"))
                        {
                            row_1[2] = Math.Round((saldo_peso / tas_cambio_cm), 3);
                            row_1[8] = Math.Round((saldo_peso / tas_cambio_cm), 3);
                        }
                        else
                        {
                            row_1[1] = Math.Round(saldo_dolar * tas_cambio_cm);
                            row_1[7] = Math.Round(saldo_dolar * tas_cambio_cm);
                        }
                    }
                }
            }

            G_FACTURAS_PAGABLES.Visible = true;
            G_FACTURAS_PAGABLES.DataSource = facturas_pagables;
            G_FACTURAS_PAGABLES.DataBind();

            tabla_documentos = "<br>";

            tabla_documentos += "<input type='submit' name='btn_recalcular_saldos' style='visibility:hidden;position:absolute;' value='ASIGNAR NOTA CREDITO' onclick='recalcular_saldos_cm();' id='btn_recalcular_saldos' class='btn btn-success'> ";
            tabla_documentos += "<br>";
            if (!si_insert.Contains("Error"))
            {
                cont_facturas = 0;
                cn_fact = 0;
                HttpContext.Current.Session["cont_cheq2"] = 0;
                un_pago = true;
                cobranza2.Visible = true;
                if (si_insert != "")
                {

                    //aca al INICIAR COBRANZA se llena facturas_pagables
                    HttpContext.Current.Session["facturas_pagables"] = facturas_pagables;
                    string fac = "";
                    foreach (DataRow r in facturas_pagables.Rows)
                    {
                        fac += r[0].ToString().Trim() + "--";
                    }
                    if (fact_sele.Text == "")
                    {
                        fact_sele.Text = fac;
                    }
                    else
                    {
                        fact_sele.Text = fac + fact_sele.Text;
                    }
                    asd = true;
                    un_pago = true;
                    string mensaje_tipo_cmabio = "";
                    if (tasa_cambio_iguales_bool)
                    {
                        mensaje_tipo_cmabio = "SI";
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee2", "<script language='javascript'>alert('MISMA TASA CAMBIO');</script>", false);
                    }
                    else
                    {
                        mensaje_tipo_cmabio = "NO";
                    }
                    if (existe_nota_credito)
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>alert('Cobranza Iniciada : ** EXISTE NOTA DE CREDITO / SALDO A FAVOR **');Tabla();</script>", false);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>alert('Cobranza Iniciada');Tabla();</script>", false);
                    }



                    try
                    {
                        string tabla = "";
                        tabla += "<table class=\"table fill-head table-bordered\" style=\"width:30%;float:right;\"> ";
                        tabla += "<thead>";
                        tabla += "<tr>";
                        tabla += "<th colspan=2 style=\"background-color: #A9D7FF !important;color:black;\">TOTAL</th>";
                        tabla += "</tr>";
                        tabla += "<th style=\"background-color: #A9D7FF !important;\">Peso</th>";
                        tabla += "<th style=\"background-color: #A9D7FF !important;\">Dolar</th>";
                        tabla += "</tr>";
                        tabla += "</thead>";
                        tabla += "<tbody>";

                        tabla += "<tr>";

                        tabla += "<td>" + Base.monto_format2(Math.Round(sum_peso)) + "</td>";
                        tabla += "<td>" + Base.monto_format2(sum_dolar) + "</td>";

                        tabla += "</tr>";
                        tabla += "</tbody>";
                        tabla += "</table>";
                        tabla = tabla.Replace("'", "");

                        montos_totales.InnerHtml = tabla_documentos + "<br>" + tabla;
                        montos_totales.Visible = true;
                        fact_sele.Text = fact_sele.Text.Substring(0, fact_sele.Text.Length - 2);
                        T_RUT_CLIENTE2.Text = rut_cliente;
                    }
                    catch { }
                }
            }
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "tweeee", "<script> var elem3 = document.getElementById(\"ocultar_principio\");    elem3.style.display = \"block\";</script>", false);

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>Tabla();</script>", false);

            //tasa cambio para cheques 
            tasa_Cambio_q = Math.Round(sum_peso / sum_dolar, 4);

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "t_cambio", "<script language='javascript'> $('#T_CAMBIO_CHEQUES').val('" + tasa_Cambio_q + "'); </script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "peso", "<script language='javascript'> $('#monto_total_peso_f').val('" + Math.Round(sum_peso) + "'); </script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "dolar", "<script language='javascript'> $('#monto_total_dolar_f').val('" + sum_dolar + "'); </script>", false);

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "creagrilla", "<script language='javascript'>creagrilla();</script>", false);

            relojito_false();
        }
        public void relojito_false()
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "relojito_false2", "<script language='javascript'>relojito(false);</script>", false);
        }


        protected void b_Click(object sender, ImageClickEventArgs e)
        {
            LlenarComboCliente();
            LlenarComboVendedor();
            LlenarComboBancos();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teee1132131eeee", "<script>combos_refresh()</script>", false);

        }

        protected void G_CHEQUES_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[9].Text != "&nbsp;")
                {
                    e.Row.BackColor = Color.FromArgb(255, 228, 196);
                }

                string script = string.Format("javascript:modal_cheque(&#39;{0}&#39;);devul_fal();", e.Row.Cells[0].Text);
                e.Row.Cells[0].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[0].Text + " </a>";
            }
        }
        protected void btn_cheques_Click(object sender, EventArgs e)
        {
            string where = "where TIPO_DOC = 'DM' AND nombre_tipo = 'Cheque'";
            string cliente = CB_CLIENTE_CHEQ.SelectedValue.Trim();
            string vendedor = CB_VENDEDOR_CHEQ.SelectedValue.Trim();

            if (cliente != "-1" && cliente != "")
            {
                where += " and rutcliente in (" + agregra_comillas(cliente) + ") ";
            }

            if (vendedor != "-1" && vendedor != "")
            {
                where += " and VENDEDOR in (" + agregra_comillas(vendedor) + ") ";
            }

            if (rd_ven.Checked)
            {
                if (t_desde_cheq.Text != "")
                {

                    where += " and fecha_venc >= convert(datetime, '" + t_desde_cheq.Text + "', 103) ";
                }
                if (t_hasta_cheq.Text != "")
                {
                    where += " and fecha_venc <= convert(datetime, '" + t_hasta_cheq.Text + "', 103) ";

                }
            }
            else
            {
                if (t_desde_cheq.Text != "")
                {

                    where += " and fecha_trans >= convert(datetime, '" + t_desde_cheq.Text + "', 103) ";
                }
                if (t_hasta_cheq.Text != "")
                {
                    where += " and fecha_trans <= convert(datetime, '" + t_hasta_cheq.Text + "', 103) ";
                }
            }
            if (txt_num_cheq.Text != "")
            {
                where += " and factura in (" + agregra_comillas(txt_num_cheq.Text) + ") ";

            }

            where += " AND bankacct = 110401 ";

            DataTable dt = ReporteRNegocio.trae_docu_calend_cheques(where, User.Identity.Name);
            G_CHEQUES.DataSource = dt;
            G_CHEQUES.DataBind();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "refresh1", "<script>combos_refresh()</script>", false);
        }
        public string estado_return_(string estado_)
        {
            if (estado_ == "0")
            {
                return "Cerrado";
            }
            else
            {
                return "Abierto";
            }
        }

        protected void btn_paga_cheque_Click(object sender, EventArgs e)
        {
            string cheque = l_num_cheque.Text;

            cobranza_pago_chequeEntity ent = new cobranza_pago_chequeEntity();
            ent.num_cheque = cheque;
            ent.fecha_pago = Convert.ToDateTime(t_dia_pago.Text);
            ent.usuario = User.Identity.Name;
            ent.cuenta_banco = Convert.ToInt32(cb_banco_soprodi.SelectedValue);

            string ok = cobranza_pago_chequeBO.agregar(ent);

            if (ok == "2" || ok == "1")
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Agregado');recargar_chosen_cheques();</script>", false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Error ! ');</script>", false);
            }
        }

        protected void G_FACTURAS_PAGABLES_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool existe_cm_o_pa = (bool)Session["existe_nota_credito"];
                if (!existe_cm_o_pa)
                {

                    e.Row.Cells[0].Text = "";

                }
                if (e.Row.Cells[1].Text != "&nbsp;")
                {
                    e.Row.Cells[3].Text = Base.monto_format_cero(e.Row.Cells[3].Text);
                    e.Row.Cells[4].Text = Base.monto_format_cero(e.Row.Cells[4].Text);
                    try
                    {

                        e.Row.Cells[7].Text = Base.monto_format_cero(e.Row.Cells[7].Text);
                        e.Row.Cells[8].Text = Base.monto_format_cero(e.Row.Cells[8].Text);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    e.Row.Visible = false;
                }
                if (e.Row.Cells[2].Text == "NOTACREDITO-F")
                {
                    e.Row.Visible = false;
                }


                if (e.Row.Cells[9].Text.Trim() != "&nbsp;")
                {
                    e.Row.Attributes["class"] = "estado20";
                }

                if (e.Row.Cells[9].Text.Trim().Substring(e.Row.Cells[9].Text.Length - 1) == "-")
                {
                    e.Row.Cells[9].Text = e.Row.Cells[9].Text.Trim().Substring(0, e.Row.Cells[9].Text.Length - 1);
                }

            }
        }

        public bool es_visible(string tipo_doc, string factura)
        {
            if (tipo_doc == "CM" || tipo_doc.Contains("CM-") || tipo_doc == "PA" && !IsNumeric(factura))
            {

                return false;
            }
            else
            {
                return true;
            }
        }

        protected void btn_pagables_Click(object sender, EventArgs e)
        {
            DataTable dt_facturas_pagables = (DataTable)Session["facturas_pagables"];
            DataTable notasdecredito = new DataTable();
            notasdecredito.Columns.Add("factura");
            notasdecredito.Columns.Add("saldo_peso");
            notasdecredito.Columns.Add("saldo_dolar");
            notasdecredito.Columns.Add("tasacambio");
            notasdecredito.Columns.Add("fvenc");
            notasdecredito.Columns.Add("tipo_doc");
            notasdecredito.Columns.Add("tipo_moneda");
            notasdecredito.Columns.Add("rutcliente");

            foreach (DataRow r in dt_facturas_pagables.Rows)
            {

                if (r["tipo_doc"].ToString() == "CM" || (r["tipo_doc"].ToString() == "PA" && !IsNumeric(r["factura"].ToString())))
                {
                    DataRow row;
                    row = notasdecredito.NewRow();

                    row[0] = r["factura"].ToString();
                    row[1] = r["saldo_peso"].ToString();
                    row[2] = r["saldo_dolar"].ToString();
                    row[3] = r["tasacambio"].ToString();
                    row[4] = r["fvenc"].ToString();
                    row[5] = r["tipo_doc"].ToString();
                    row[6] = r["tipo_moneda"].ToString();
                    row[7] = r["rutcliente"].ToString();

                    notasdecredito.Rows.Add(row);
                }
            }
            DataTable facturas_pagables = new DataTable();
            facturas_pagables.Columns.Add("factura");
            facturas_pagables.Columns.Add("saldo_peso");
            facturas_pagables.Columns.Add("saldo_dolar");
            facturas_pagables.Columns.Add("sw_abono");
            facturas_pagables.Columns.Add("tipo_doc");
            facturas_pagables.Columns.Add("tasacambio");
            facturas_pagables.Columns.Add("fvenc");
            facturas_pagables.Columns.Add("saldo_final_peso");
            facturas_pagables.Columns.Add("saldo_final_dolar");
            facturas_pagables.Columns.Add("facturas_aplicadas");
            facturas_pagables.Columns.Add("tipo_moneda");
            facturas_pagables.Columns.Add("rutcliente");

            int cont_notas_credito = 0;
            foreach (DataRow r2 in notasdecredito.Rows)
            {
                cont_notas_credito++;
                string nota_credito = r2[0].ToString();
                double saldo_peso_CM = Convert.ToDouble(r2[1].ToString());
                double saldo_dolar_CM = Convert.ToDouble(r2[2].ToString());
                string sw_abono_CM = "";
                string tipo_doc_CM = r2["tipo_doc"].ToString();
                string tasa_cambio_CM = r2["tasacambio"].ToString();
                string fvenc_CM = r2["fvenc"].ToString();
                double saldo_final_peso_CM = saldo_peso_CM;
                double saldo_final_dolar_CM = saldo_dolar_CM;
                string facturas_aplicadas_CM = "";

                double saldo_peso_CM_aux = saldo_peso_CM;
                double saldo_dolar_CM_aux = saldo_dolar_CM;

                string tipo_moneda_CM = r2["tipo_moneda"].ToString().Trim();
                string rutcliente_CM = r2["rutcliente"].ToString().Trim();

                int count_facturas = 0;
                foreach (GridViewRow dtgItem in this.G_FACTURAS_PAGABLES.Rows)
                {
                    try
                    {
                        string factura = G_FACTURAS_PAGABLES.DataKeys[dtgItem.RowIndex].Values[0].ToString();
                        double saldo_peso = Convert.ToDouble(G_FACTURAS_PAGABLES.DataKeys[dtgItem.RowIndex].Values[2].ToString());
                        double saldo_dolar = Convert.ToDouble(G_FACTURAS_PAGABLES.DataKeys[dtgItem.RowIndex].Values[3].ToString());
                        string sw_abono = G_FACTURAS_PAGABLES.DataKeys[dtgItem.RowIndex].Values[6].ToString();
                        string tipo_doc = G_FACTURAS_PAGABLES.DataKeys[dtgItem.RowIndex].Values[1].ToString();
                        string tasacambio = G_FACTURAS_PAGABLES.DataKeys[dtgItem.RowIndex].Values[5].ToString();
                        string fvenc = G_FACTURAS_PAGABLES.DataKeys[dtgItem.RowIndex].Values[4].ToString();
                        double saldo_final_peso = 0;
                        double saldo_final_dolar = 0;
                        string facturas_aplicadas = "";

                        string tipo_moneda_IN = G_FACTURAS_PAGABLES.DataKeys[dtgItem.RowIndex].Values[7].ToString();
                        string rutcliente_IN = G_FACTURAS_PAGABLES.DataKeys[dtgItem.RowIndex].Values[8].ToString();

                        double saldo_peso_aux = Convert.ToDouble(G_FACTURAS_PAGABLES.DataKeys[dtgItem.RowIndex].Values[2].ToString());
                        double saldo_dolar_aux = Convert.ToDouble(G_FACTURAS_PAGABLES.DataKeys[dtgItem.RowIndex].Values[3].ToString());
                        DataRow row_final = facturas_pagables.NewRow();
                        CheckBox Sel = ((CheckBox)G_FACTURAS_PAGABLES.Rows[dtgItem.RowIndex].FindControl("CH_CM"));
                        bool valor = Sel.Checked;
                        if (valor)
                        {
                            count_facturas++;
                            foreach (DataRow r_dt_original in dt_facturas_pagables.Rows)
                            {
                                if (factura == r_dt_original[0].ToString())
                                {

                                    saldo_peso = Convert.ToDouble(r_dt_original[7].ToString());
                                    saldo_dolar = Convert.ToDouble(r_dt_original[8].ToString());

                                    if (saldo_final_dolar_CM * -1 >= 0)
                                    {
                                        saldo_peso_CM = saldo_final_peso_CM;
                                        saldo_dolar_CM = saldo_final_dolar_CM;

                                        facturas_aplicadas_CM += factura + "-";
                                        r_dt_original[9] = r_dt_original[9] + nota_credito + "-";

                                        row_final[0] = factura;
                                        row_final[1] = saldo_peso_aux;
                                        row_final[2] = saldo_dolar_aux;
                                        row_final[3] = sw_abono;
                                        row_final[4] = tipo_doc;
                                        row_final[5] = tasacambio;
                                        row_final[6] = fvenc;
                                        saldo_final_peso = saldo_peso + saldo_peso_CM;
                                        saldo_final_dolar = saldo_dolar + saldo_dolar_CM;
                                        if (saldo_final_peso <= 0 && saldo_final_dolar <= 0)
                                        {
                                            saldo_final_peso_CM = saldo_final_peso;
                                            saldo_final_dolar_CM = saldo_final_dolar;

                                            //agregar historico de lo que paga CM
                                            DataRow row_cm_historico = facturas_pagables.NewRow();
                                            row_cm_historico[0] = nota_credito;
                                            row_cm_historico[1] = saldo_peso_CM_aux;
                                            row_cm_historico[2] = saldo_dolar_CM_aux;
                                            row_cm_historico[3] = sw_abono_CM;
                                            row_cm_historico[4] = tipo_doc_CM + "-" + count_facturas;
                                            row_cm_historico[5] = tasa_cambio_CM;
                                            row_cm_historico[6] = fvenc_CM;
                                            row_cm_historico[7] = saldo_peso * -1;
                                            row_cm_historico[8] = saldo_dolar * -1;
                                            row_cm_historico[9] = factura;
                                            row_cm_historico[10] = tipo_moneda_CM;
                                            row_cm_historico[11] = rutcliente_CM;
                                            facturas_pagables.Rows.Add(row_cm_historico);
                                        }
                                        else
                                        {
                                            //agregar historico de lo que paga CM
                                            DataRow row_cm_historico = facturas_pagables.NewRow();
                                            row_cm_historico[0] = nota_credito;
                                            row_cm_historico[1] = saldo_peso_CM_aux;
                                            row_cm_historico[2] = saldo_dolar_CM_aux;
                                            row_cm_historico[3] = sw_abono_CM;
                                            row_cm_historico[4] = tipo_doc_CM + "-" + count_facturas;
                                            row_cm_historico[5] = tasa_cambio_CM;
                                            row_cm_historico[6] = fvenc_CM;
                                            row_cm_historico[7] = saldo_final_peso_CM;
                                            row_cm_historico[8] = saldo_final_dolar_CM;
                                            row_cm_historico[9] = factura;
                                            row_cm_historico[10] = tipo_moneda_CM;
                                            row_cm_historico[11] = rutcliente_CM;
                                            facturas_pagables.Rows.Add(row_cm_historico);

                                            saldo_final_peso_CM = 0;
                                            saldo_final_dolar_CM = 0;
                                        }

                                        if (saldo_final_peso <= 0)
                                        {
                                            saldo_final_peso = 0;
                                        }
                                        if (saldo_final_dolar <= 0)
                                        {
                                            saldo_final_dolar = 0;
                                        }

                                        r_dt_original[7] = saldo_final_peso;
                                        r_dt_original[8] = saldo_final_dolar;

                                        row_final[7] = saldo_final_peso;
                                        row_final[8] = saldo_final_dolar;
                                        row_final[9] = r_dt_original[9].ToString();
                                        row_final[10] = tipo_moneda_IN;
                                        row_final[11] = rutcliente_IN;
                                    }
                                    else
                                    {
                                        row_final[0] = factura;
                                        row_final[1] = saldo_peso;
                                        row_final[2] = saldo_dolar;
                                        row_final[3] = sw_abono;
                                        row_final[4] = tipo_doc;
                                        row_final[5] = tasacambio;
                                        row_final[6] = fvenc;
                                        row_final[7] = saldo_peso;
                                        row_final[8] = saldo_dolar;
                                        row_final[9] = facturas_aplicadas;
                                        row_final[10] = tipo_moneda_IN;
                                        row_final[11] = rutcliente_IN;
                                    }
                                    if (cont_notas_credito == notasdecredito.Rows.Count)
                                    {
                                        facturas_pagables.Rows.Add(row_final);
                                    }
                                }
                                //if
                            }
                            //foreach
                        }
                        else
                        {
                            if (tipo_doc == "IN" || tipo_doc == "DM" || tipo_doc == "PA" && IsNumeric(factura))
                            {
                                row_final[0] = factura;
                                row_final[1] = saldo_peso;
                                row_final[2] = saldo_dolar;
                                row_final[3] = sw_abono;
                                row_final[4] = tipo_doc;
                                row_final[5] = tasacambio;
                                row_final[6] = fvenc;
                                row_final[7] = saldo_peso;
                                row_final[8] = saldo_dolar;
                                row_final[9] = facturas_aplicadas;
                                row_final[10] = tipo_moneda_IN;
                                row_final[11] = rutcliente_IN;
                                facturas_pagables.Rows.Add(row_final);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string error = "Error : " + ex.Message.ToString();
                    }
                }



                if (saldo_final_peso_CM == 0 && saldo_final_dolar_CM == 0)
                {
                    DataRow row_final = facturas_pagables.NewRow();
                    row_final[0] = nota_credito;
                    row_final[1] = Convert.ToDouble(r2[1].ToString());
                    row_final[2] = Convert.ToDouble(r2[2].ToString());
                    row_final[3] = sw_abono_CM;
                    row_final[4] = "NOTACREDITO-F";
                    row_final[5] = tasa_cambio_CM;
                    row_final[6] = fvenc_CM;
                    row_final[7] = 0;
                    row_final[8] = 0;
                    if (facturas_aplicadas_CM.Contains("-"))
                        row_final[9] = facturas_aplicadas_CM.Substring(0, facturas_aplicadas_CM.Length - 1);
                    else
                        row_final[9] = facturas_aplicadas_CM;

                    row_final[10] = tipo_moneda_CM;
                    row_final[11] = rutcliente_CM;

                    facturas_pagables.Rows.Add(row_final);
                }
                else if (saldo_final_peso_CM < 0 && saldo_final_dolar_CM < 0)
                {
                    DataRow row_final = facturas_pagables.NewRow();
                    row_final[0] = nota_credito;
                    row_final[1] = Convert.ToDouble(r2[1].ToString());
                    row_final[2] = Convert.ToDouble(r2[2].ToString());
                    row_final[3] = sw_abono_CM;
                    row_final[4] = "NOTACREDITO-F";
                    row_final[5] = tasa_cambio_CM;
                    row_final[6] = fvenc_CM;
                    row_final[7] = saldo_final_peso_CM;
                    row_final[8] = saldo_final_dolar_CM;
                    if (facturas_aplicadas_CM.Contains("-"))
                        row_final[9] = facturas_aplicadas_CM.Substring(0, facturas_aplicadas_CM.Length - 1);
                    else
                        row_final[9] = facturas_aplicadas_CM;

                    row_final[10] = tipo_moneda_CM;
                    row_final[11] = rutcliente_CM;
                    facturas_pagables.Rows.Add(row_final);

                }
                else
                {
                    DataRow row_final = facturas_pagables.NewRow();
                    row_final[0] = nota_credito;
                    row_final[1] = Convert.ToDouble(r2[1].ToString());
                    row_final[2] = Convert.ToDouble(r2[2].ToString());
                    row_final[3] = sw_abono_CM;
                    row_final[4] = "NOTACREDITO-F";
                    row_final[5] = tasa_cambio_CM;
                    row_final[6] = fvenc_CM;
                    row_final[7] = Convert.ToDouble(r2[1].ToString());
                    row_final[8] = Convert.ToDouble(r2[2].ToString());
                    if (facturas_aplicadas_CM.Contains("-"))
                        row_final[9] = facturas_aplicadas_CM.Substring(0, facturas_aplicadas_CM.Length - 1);
                    else
                        row_final[9] = facturas_aplicadas_CM;
                    row_final[10] = tipo_moneda_CM;
                    row_final[11] = rutcliente_CM;
                    facturas_pagables.Rows.Add(row_final);
                }
                //final de recorrer CM'S
            }


            //DataTable facturas_pagables_FINAL = new DataTable();
            //facturas_pagables_FINAL.Columns.Add("factura");
            //facturas_pagables_FINAL.Columns.Add("saldo_peso");
            //facturas_pagables_FINAL.Columns.Add("saldo_dolar");
            //facturas_pagables_FINAL.Columns.Add("sw_abono");
            //facturas_pagables_FINAL.Columns.Add("tipo_doc");
            //facturas_pagables_FINAL.Columns.Add("tasacambio");
            //facturas_pagables_FINAL.Columns.Add("fvenc");
            //facturas_pagables_FINAL.Columns.Add("saldo_final_peso");
            //facturas_pagables_FINAL.Columns.Add("saldo_final_dolar");
            //facturas_pagables_FINAL.Columns.Add("facturas_aplicadas");

            //foreach (DataRow r_fact in facturas_pagables.Rows)
            //{
            //    string factura = r_fact[0].ToString().Trim();
            //    double saldo_final_menor_peso = Convert.ToDouble(r_fact[7].ToString().Trim());
            //    double saldo_final_menor_dolar = Convert.ToDouble(r_fact[8].ToString().Trim());
            //    foreach (DataRow r_fact2 in facturas_pagables.Rows)
            //    {
            //        if (factura == r_fact[0].ToString().Trim())
            //        {
            //            double saldo_final_menor_peso_2 = Convert.ToDouble(r_fact2[7].ToString().Trim());
            //            double saldo_final_menor_dolar_2 = Convert.ToDouble(r_fact2[8].ToString().Trim());

            //            if (saldo_final_menor_peso_2< saldo_final_menor_peso)
            //            {


            //            }

            //        }
            //    }

            //}

            Session["facturas_pagables"] = facturas_pagables;

            G_FACTURAS_PAGABLES.DataSource = facturas_pagables;
            G_FACTURAS_PAGABLES.DataBind();

            btn_pagables.Enabled = false;
            btn_pagables.Text = "YA ASIGNADO";

            //BTN_NETEO.Visible = true;
            BTN_NETEO_2.Visible = true;
            P_FECHA_NET.Visible = true;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "neteo", "<script language='javascript'>APLICAR_NETEO();</script>", false);


            // aca asignar valores para aplicar neteo automatico

            //var id = $('#fact_sele').val();
            //var monto = $('#T_MONTO_PAGO2').val();
            //var moneda = document.getElementById("CB_TIPO_MONEDA2").value;
            //var tipo_doc = document.getElementById("CB_TIPO_PAGO2").value;
            //var descripcion = $('#T_DESCRIPCION_PAGO2').val();
            //var fecha = document.getElementById('t_fech_efec').value;

            //var parameters = new Object();
            //parameters.id = id;
            //parameters.monto = monto;
            //parameters.moneda = moneda;
            //parameters.tipo_doc = tipo_doc;
            //parameters.descripcion = descripcion;
            //parameters.cerrar = "no";
            //parameters.fecha = fecha;
            //parameters.enviar_erp = chk_enviar_erp;

        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {

        }

        protected void btn_pago2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "TRY", "<script language='javascript'>REGISTRAR_PAGO2();</script>", false);
        }

        protected void btn_actualizar_saldos_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "sqqwe", "<script language='javascript'>cambia_sw2();</script>", false);

            string num_factura = "";
            string ESTADO = "";
            string id = "";
            DataTable si_update = new DataTable();
            foreach (GridViewRow dtgItem in this.G_MOV_SOL.Rows)
            {
                CheckBox Sel = ((CheckBox)G_MOV_SOL.Rows[dtgItem.RowIndex].FindControl("chkAccept2"));
                bool valor = Sel.Checked;
                if (valor)
                {

                    num_factura = G_MOV_SOL.Rows[dtgItem.RowIndex].Cells[2].Text.Split('>').ToList()[1].ToString().Replace("/a>", "").Replace("</a", "").Trim();

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "documento", valor = num_factura.Trim() });

                    si_update = ReporteRNegocio.actualizar_saldos("TRAER_SALDOS_COBRANZA", vars);
                }
                else
                {
                }
            }
            if (si_update.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>Ingresar_Solomon();</script>", false);
                btn_mov_sol_Click(sender, e);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>alert('Ingresados')</script>", false);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "tee2ee", "<script language='javascript'>    $('#ContentPlaceHolder_Contenido_btn_filtra_mov').click(); </script>", false);
            }
        }

        protected void BTN_NETEO_Click(object sender, EventArgs e)
        {

        }

        protected void txt_peso_TextChanged(object sender, EventArgs e)
        {
            string asd = "";
        }

        protected void txt_dolar_TextChanged(object sender, EventArgs e)
        {
            string asd = "";
        }

        protected void chk_enviar_erp_CheckedChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "cambia_Color", "<script language='javascript'>  cambia_color_fondo(); </script>", false);

        }
    }
}