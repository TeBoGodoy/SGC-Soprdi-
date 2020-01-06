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
using System.Net.Mail;
using System.Net;
using System.Diagnostics;
//using Microsoft.Office.Interop.Word;

namespace SoprodiApp
{
    public partial class ABARROTES_INSTRUIR : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }



        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            MakeAccessible(G_INFORME_TOTAL_VENDEDOR);
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

        private void cargar_combo_cliente(DataTable dt, DataView dtv)
        {
            //dt.Rows.Add(new Object[] { -1, "-- Todos --" });
            dtv = dt.DefaultView;
            dtv.Sort = "nom_cliente";
            cb_cliente.DataSource = dtv;
            cb_cliente.DataTextField = "nom_cliente";
            cb_cliente.DataValueField = "rut_cliente";
            //d_vendedor_.SelectedIndex = -1;
            cb_cliente.DataBind();
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            string desde = txt_desde.Text;
            string hasta = txt_hasta.Text;
            string grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

            if (grupos_del_usuario == "")
            {
                grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
            }
            List<string> grupos = grupos_del_usuario.Split(',').ToList();
            string where3 = "";
            if (grupos.Count == 4)
            {

                where3 += "  where  1=1 ";
            }
            else
            {

                if (grupos_del_usuario.ToUpper().Replace("'", "").Trim() == "GRANOS")
                {
                    where3 += "  where  b.DescEmisor = 'Granos' ";
                }
                else if (grupos_del_usuario.Contains("Abarrotes") || grupos_del_usuario.Contains("CMQuillota") || grupos_del_usuario.Contains("MayoristasLV"))
                {

                    where3 += " where  b.DescEmisor <> 'Granos' ";
                }

            }
            //select * from[VPEDIDOCABECERA] where FechaEmision >= CONVERT(datetime, '21/07/2017', 103)
            if (desde != "")
            {
                where3 += " and convert(datetime,b.FechaEmision ,103)  >= convert(datetime, '" + desde + "', 103) ";
            }
            if (hasta != "")
            {
                where3 += " and convert(datetime,b.FechaEmision ,103)   <= convert(datetime, '" + hasta + "', 103) ";
            }
            //if (clientes != "")
            //{

            //    where3 += " and b.rut in (" + clientes + ")";
            //} 
            if (txt_sp.Text != "")
            {
                where3 += " and b.CodDocumento in (" + agregra_comillas(txt_sp.Text) + ")";
            }

            //SPSP
            DataTable dt2 = ReporteRNegocio.VM_listar_sp(where3);

            string cod_aux = "";
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

                if (r[13].ToString() == "Aprobado" && r[28].ToString() == "no")
                {
                    if (cod_aux == "")
                    {
                        cod_aux = r[0].ToString();
                        sp_for = new DataTable();
                        sp_for.Columns.Add("sp");
                        sp_for.Columns.Add("estado");
                        sp_for.Columns.Add("facturas");
                    }

                    DataTable procesado = ReporteRNegocio.SP_Marcelo(r[0].ToString().Trim(), r[26].ToString().Trim(), r[27].ToString().Trim());
                    DataRow row = procesado.Rows[0];

                    if (r[0].ToString() == cod_aux)
                    {
                        //if (row["estado"].ToString().Substring(0, 3) == "10P")
                        //{
                        facturas_aux = row["facturas"].ToString();

                        if (facturas_aux.Trim() != "")
                        {
                            facturas_x_sps += row["facturas"].ToString() + ", ";
                            r[14] = facturas_x_sps.Substring(0, facturas_x_sps.Length - 3);
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
                                try
                                {
                                    row_sp["facturas"] = facturas_x_sps.Substring(0, facturas_x_sps.Length - 2);
                                }
                                catch
                                {
                                    row_sp["facturas"] = "";

                                }
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
                        r[14] = facturas_x_sps.Substring(0, facturas_x_sps.Length - 2);

                        DataRow row_sp1 = sp_for.NewRow();
                        row_sp1["sp"] = cod_aux;
                        row_sp1["estado"] = row["estado"].ToString().Substring(0, 3);
                        row_sp1["facturas"] = facturas_x_sps.Substring(0, facturas_x_sps.Length - 2);
                        sp_for.Rows.Add(row_sp1);
                    }
                }
                else if (r[13].ToString() == "Aprobado")
                {
                    DataTable procesado = ReporteRNegocio.SP_Marcelo(r[0].ToString().Trim(), r[26].ToString().Trim(), r[27].ToString().Trim());
                    DataRow row = procesado.Rows[0];
                    r[14] = row["facturas"].ToString();
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
                if (r[13].ToString() == "Aprobado")
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
                                r[14] = cadd;
                                r[25] = "10P";
                            }
                            else
                            {
                                r[14] = cadd;
                                r[25] = r2[2].ToString();
                            }
                        }
                    }
                }
                r[14] = r[14].ToString().Replace(",", ", ");
            }
            if (dt2.Rows.Count > 0)
            {
                dt2 = dt2.AsEnumerable()
                       .GroupBy(r => new { Col1 = r["CodDocumento"] })
                       .Select(g => g.OrderBy(r => r["CodDocumento"]).First())
                       .CopyToDataTable();
            }
            G_INFORME_TOTAL_VENDEDOR.DataSource = dt2;
            G_INFORME_TOTAL_VENDEDOR.DataBind();
            JQ_Datatable();
            DIV_LISTADO.Visible = true;
        }
    }
}