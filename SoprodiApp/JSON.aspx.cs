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

namespace SoprodiApp
{
    public partial class JSON : System.Web.UI.Page
    {
        public class Eventos_calendario
        {
            public string id { get; set; }
            public string title { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public string backgroundColor { get; set; }
            public string textColor { get; set; }
            public string factura { get; set; }
            public string rut_cliente { get; set; }
            public string tipo_doc { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DBUtil db = new DBUtil();
            string f1 = "", f2 = "", vc = "", filtro_fecha = "", tipo_doc = "", cl = "";

            string user = Session["user"].ToString();

            // Guardo Valores
            if (Request.QueryString["f1"] != null)
            {
                f1 = Request.QueryString["f1"].ToString();
            }
            if (Request.QueryString["f2"] != null)
            {
                f2 = Request.QueryString["f2"].ToString();
            }
            if (Request.QueryString["vc"] != null)
            {
                vc = Request.QueryString["vc"].ToString();
            }
            if (Request.QueryString["td"] != null)
            {
                tipo_doc = Request.QueryString["td"].ToString();
            }
            if (Request.QueryString["cl"] != null)
            {
                cl = Request.QueryString["cl"].ToString();
            }

            if (cl == "-1")
            {
                cl = "";
            }
            else if (cl == "")
            {
                cl = "";
            }
            else
            {
                cl = " and rutcliente IN ( '" + cl + "' )";
            }

            // Creo Filtro de fechas
            if (f1 != "" || f2 != "")
            {
                filtro_fecha = " and fecha_trans between CONVERT(datetime, '" + f1 + "', 103) and CONVERT(datetime, '" + f2 + "', 103) ";
            }

            // Creo Filtro Tipo Doc
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
                aux_tipo = aux_tipo.Substring(0, aux_tipo.Length - 1);
                tipo_doc = tipo_doc.Substring(0, tipo_doc.Length - 1);
                tipo_doc = " and tipo_doc IN ( " + tipo_doc + ")";

                if (tipo_doc.Contains("ND") && !tipo_doc.Contains("DM"))
                {
                    tipo_doc += "  and isnumeric(factura) = 1 ";
                    tipo_doc = tipo_doc.Replace("ND", "DM");
                }

                else if (!tipo_doc.Contains("ND") && tipo_doc.Contains("DM"))
                {
                    tipo_doc += "  and isnumeric(factura) <> 1 ";
                }
                else if (tipo_doc.Contains("ND") && tipo_doc.Contains("DM"))
                {
                    tipo_doc = " and tipo_doc IN ( " + aux_tipo + ")";
                }
            }

            // CASE

            // Factura - Cliente
            if (vc == "11")
            {
                string consulta = "";
                consulta += "select a.* from (";
                consulta += "SELECT id, rutcliente, nombrecliente, CONVERT(varchar(10) ,  case when CONVERT(varchar, fecha_cobro, 103) = '01/01/1900' then fecha_venc else fecha_cobro end ,111) as fecha_inicio, ";
                consulta += "factura, convert(varchar, (cast( saldo as int) ), 100) as monto_doc, ";

                consulta += " CAST( CASE  ";
                consulta += " when tipo_doc = 'PA'  ";
                consulta += " then '#04B404'   ";
                consulta += " when (tipo_doc = 'IN' and (select COUNT(*) from cobranza_cobros where num_factura = factura) >= 1) ";
                consulta += "  then '#00A5CD'   ";
                consulta += " when (tipo_doc = 'IN' and (select COUNT(*) from Cobranza_Seguimiento where Cobranza_Seguimiento.num_factura = factura) >= 1)    ";
                consulta += " then '#95A5A6' ";
                consulta += " when (tipo_doc = 'IN' and (select COUNT(*) from cobranza_cobros where num_factura = factura) <> 1  and (select COUNT(*)from Cobranza_Seguimiento where Cobranza_Seguimiento.num_factura = factura) = 0)    ";
                consulta += " then '#3A01DF' ";
                consulta += " when (tipo_doc = 'DM' and (select COUNT(*) from cobranza_cobros where num_factura = factura) >= 1)   ";
                consulta += " then '#F801CF'   ";
                consulta += " when (tipo_doc = 'DM' and (select COUNT(*) from cobranza_cobros where num_factura = factura) <> 1)    ";
                consulta += " then '#6A0888'       ";
                consulta += " else '#3A01DF' end as varchar(10)) as color  ";
                consulta += ", tipo_doc FROM V_COBRANZA where 1 = 1";

                if (filtro_fecha != "")
                {
                    consulta += filtro_fecha;
                }
                if (tipo_doc != "")
                {
                    consulta += tipo_doc + " and estado_doc <> 0  ";
                }
                if (cl != "")
                {
                    consulta += cl;
                }
                consulta += " ) a order by CONVERT(datetime, a.fecha_inicio, 111) desc,  CAST(a.monto_doc as NUMERIC(38,2)) desc";

                dt = db.consultar(consulta);

                string fecha = "";
                int cont = 1;
                foreach (DataRow r in dt.Rows)
                {
                    if (cont == 1)
                    {
                        fecha = r[3].ToString();
                    }
                    if (fecha == r[3].ToString())
                    {
                        r[5] = cont + "-" + r[5].ToString();
                        cont++;
                    }
                    else
                    {
                        cont = 1;
                        r[5] = cont + "-" + r[5].ToString();
                        fecha = r[3].ToString();
                        cont++;
                    }
                }

                string consulta2 = "";
                consulta2 += " select 0 as id, '' as rutcliente,  '' as nombrecliente,fecha_inicio,'' as factura,  CAST('    ' AS varchar(MAX)) + convert(varchar, (cast( sum(monto_doc) as int) ), 100) as monto_doc, '' as color, '' as tipo_doc from (  ";
                consulta2 += "     select aa.saldo as monto_doc, aa.fecha_inicio from                 ";
                consulta2 += " (SELECT id, rutcliente, nombrecliente,CONVERT(varchar(10) ,  case when CONVERT(varchar, fecha_cobro, 103) = '01/01/1900' then fecha_venc else fecha_cobro end ,111) as fecha_inicio, factura, monto_doc as saldo,  '#3A01DF' as color , tipo_doc FROM V_COBRANZA where 1 = 1";

                if (filtro_fecha != "")
                {
                    consulta2 += filtro_fecha;
                }
                if (tipo_doc != "")
                {
                    consulta2 += tipo_doc + " and estado_doc <> 0  ";
                }
                if (cl != "")
                {
                    consulta2 += cl;
                }
                consulta2 += "     ) as aa ) as qwed   group by fecha_inicio  ";

                dt2 = db.consultar(consulta2);

                dt.Merge(dt2);

                var query = from item in dt.AsEnumerable()
                            where 1 == 1
                            select new Eventos_calendario
                            {
                                id = Convert.ToString(item["id"]).Replace("'", ""),
                                title = calcula(item["monto_doc"], item["factura"], item["nombrecliente"]),
                                start = Convert.ToString(item["fecha_inicio"]).Replace("/", "-"),
                                factura = Convert.ToString(item["factura"]).Replace("'", ""),
                                rut_cliente = Convert.ToString(item["rutcliente"]).Replace("'", ""),
                                backgroundColor = Convert.ToString(item["color"]).Replace("'", ""),
                                tipo_doc = Convert.ToString(item["tipo_doc"]).Replace("'", "")
                            };

                JavaScriptSerializer jss = new JavaScriptSerializer();
                String json = jss.Serialize(query);

                Response.Write(json);
                Response.End();
            }




            // Vencimientos
            else if (vc == "13")
            {
                string consulta = "";
                consulta += " SELECT id,monto_doc, CONVERT(varchar(10), case when CONVERT(varchar, fecha_cobro, 103) = '01/01/1900' then fecha_venc else fecha_cobro end  ,111) as fecha_inicio, nombrecliente, factura, rutcliente,  ";
                consulta += " color = CASE  ";
                consulta += "WHEN DATEDIFF(day,fecha_venc, GETDATE()) >= 0 and DATEDIFF(day,fecha_venc, GETDATE()) <= 5 THEN  '#BFFF00' ";
                consulta += "WHEN DATEDIFF(day,fecha_venc, GETDATE())  >= 6 and DATEDIFF(day,fecha_venc, GETDATE()) <= 10 THEN '#FFFF00'   ";
                consulta += "WHEN DATEDIFF(day,fecha_venc, GETDATE())  >= 11 and DATEDIFF(day,fecha_venc, GETDATE()) <= 15 THEN '#FFBF00' ";
                consulta += "WHEN DATEDIFF(day,fecha_venc, GETDATE())  >= 16 and DATEDIFF(day,fecha_venc, GETDATE()) <= 20 THEN '#FF4000' ";
                consulta += "WHEN DATEDIFF(day,fecha_venc, GETDATE())  >= 21 and DATEDIFF(day,fecha_venc, GETDATE()) <= 25 THEN '#DF0101'   ";
                consulta += "WHEN DATEDIFF(day,fecha_venc, GETDATE())  >= 26 and DATEDIFF(day,fecha_venc, GETDATE()) <= 45 THEN  '#FF0000' ";
                consulta += "WHEN DATEDIFF(day,fecha_venc, GETDATE())  > 45 THEN  '#000' ";
                consulta += "WHEN DATEDIFF(day,fecha_venc, GETDATE())  < 0 THEN  '#3ADF00'  ";
                consulta += "ELSE '#190707' END ";
                consulta += ", color2 = CASE  ";
                consulta += "WHEN DATEDIFF(day,fecha_venc, GETDATE()) >= 0 and DATEDIFF(day,fecha_venc, GETDATE()) <= 5 THEN  '#000000' ";
                consulta += "WHEN DATEDIFF(day,fecha_venc, GETDATE())  >= 6 and DATEDIFF(day,fecha_venc, GETDATE()) <= 10 THEN '#000000'   ";
                consulta += "WHEN DATEDIFF(day,fecha_venc, GETDATE())  >= 11 and DATEDIFF(day,fecha_venc, GETDATE()) <= 15 THEN '#000000' ";
                consulta += "WHEN DATEDIFF(day,fecha_venc, GETDATE())  >= 16 and DATEDIFF(day,fecha_venc, GETDATE()) <= 20 THEN '#FFFFFF' ";
                consulta += "WHEN DATEDIFF(day,fecha_venc, GETDATE())  >= 21 and DATEDIFF(day,fecha_venc, GETDATE()) <= 25 THEN '#FFFFFF'   ";
                consulta += "WHEN DATEDIFF(day,fecha_venc, GETDATE())  >= 26 and DATEDIFF(day,fecha_venc, GETDATE()) <= 45  THEN  '#FFFFFF' ";
                consulta += "WHEN DATEDIFF(day,fecha_venc, GETDATE())  > 45 THEN  '#FFFFFF' ";
                consulta += "WHEN DATEDIFF(day,fecha_venc, GETDATE())  < 0 THEN  '#000000'  ";
                consulta += "ELSE '#190707' END ";
                consulta += ", tipo_doc FROM V_COBRANZA ca where 1 = 1 AND RUTCLIENTE = ca.RUTCLIENTE ";

                if (filtro_fecha != "")
                {
                    consulta += filtro_fecha;
                }
                if (tipo_doc != "")
                {
                    consulta += tipo_doc + " and estado_doc <> 0  ";
                }
                else
                {
                    consulta += " and tipo_doc IN ('IN', 'DM') ";
                }
                if (cl != "")
                {
                    consulta += cl;
                }

                dt = db.consultar(consulta);

                var query = from item in dt.AsEnumerable()
                            where 1 == 1
                            select new Eventos_calendario
                            {
                                id = Convert.ToString(item["id"]).Replace("'", ""),
                                title = puntos_miles(Convert.ToString(item["monto_doc"]).Replace("'", "").Substring(0, (item["monto_doc"]).ToString().IndexOf(","))) + "(" + Convert.ToString(item["factura"]).Replace("'", "") + ")" + Convert.ToString(item["nombrecliente"]).Replace("'", ""),
                                rut_cliente = Convert.ToString(item["rutcliente"]).Replace("'", ""),
                                start = Convert.ToString(item["fecha_inicio"]).Replace("/", "-"),
                                backgroundColor = Convert.ToString(item["color"]).Replace("'", ""),
                                factura = Convert.ToString(item["factura"]).Replace("'", ""),
                                tipo_doc = Convert.ToString(item["tipo_doc"]).Replace("'", ""),
                                textColor = Convert.ToString(item["color2"]).Replace("'", "")
                            };

                JavaScriptSerializer jss = new JavaScriptSerializer();
                String json = jss.Serialize(query);

                Response.Write(json);
                Response.End();
            }
            // MIS SEGUIMIENTOS
            // FACTURA - CLIENTE
            else if (vc == "21")
            {
                string consulta = "";
                consulta += "SELECT cs.id, cs.rutcliente, cs.monto_doc, (select top 1 b.nombrecliente from cobranza b where b.rutcliente = cs.rutcliente)  as nombrecliente, CONVERT(varchar(10) ,cs.fecha_venc,111) as fecha_inicio, ";
                consulta += "num_factura, CAST( CASE when tipo_doc = 'PA' then '#04B404' when tipo_doc = 'IN' then '#3A01DF' when tipo_doc = 'DM' then '#6A0888' else '#3A01DF' end as varchar(10)) as color, cs.tipo_doc ";
                consulta += "FROM COBRANZA_SEGUIMIENTO cs where usuario = '" + user + "'";

                if (filtro_fecha != "")
                {
                    consulta += filtro_fecha;
                }
                if (tipo_doc != "")
                {
                    consulta += tipo_doc;
                }
                if (cl != "")
                {
                    consulta += cl;
                }
                //consulta += " and tipo_doc = 'IN' ";

                dt = db.consultar(consulta);

                var query = from item in dt.AsEnumerable()
                            where 1 == 1
                            select new Eventos_calendario
                            {
                                id = Convert.ToString(item["num_factura"]).Replace("'", ""),
                                title = puntos_miles(Convert.ToString(item["monto_doc"]).Replace("'", "").Substring(0, (item["monto_doc"]).ToString().IndexOf(","))) + "(" + Convert.ToString(item["num_factura"]).Replace("'", "") + ")" + Convert.ToString(item["nombrecliente"]).Replace("'", ""),
                                start = Convert.ToString(item["fecha_inicio"]).Replace("/", "-"),
                                factura = Convert.ToString(item["num_factura"]).Replace("'", ""),
                                rut_cliente = Convert.ToString(item["rutcliente"]).Replace("'", ""),
                                backgroundColor = Convert.ToString(item["color"]).Replace("'", ""),
                                tipo_doc = Convert.ToString(item["tipo_doc"]).Replace("'", "")
                            };

                JavaScriptSerializer jss = new JavaScriptSerializer();
                String json = jss.Serialize(query);

                Response.Write(json);
                Response.End();
            }
            // Agendamientos
            else if (vc == "22")
            {
                string consulta = "";
                consulta += "SELECT CA.id_cobranza, CA.descripcion, CONVERT(varchar(10) , CA.fecha_trans ,111) as fecha_inicio, ";
                consulta += "'#B40404' as color FROM Cobranza_agenda ca where CA.usuario = '" + user + "'";

                if (filtro_fecha != "")
                {
                    consulta += filtro_fecha;
                }
                if (cl != "")
                {
                    consulta += cl;
                }

                dt = db.consultar(consulta);

                var query = from item in dt.AsEnumerable()
                            where 1 == 1
                            select new Eventos_calendario
                            {
                                id = Convert.ToString(item["id_cobranza"]).Replace("'", ""),
                                title = "(" + Convert.ToString(item["id_cobranza"]).Replace("'", "") + ")" + Convert.ToString(item["descripcion"]).Replace("'", ""),
                                start = Convert.ToString(item["fecha_inicio"]).Replace("/", "-"),
                                backgroundColor = Convert.ToString(item["color"]).Replace("'", "")
                            };

                JavaScriptSerializer jss = new JavaScriptSerializer();
                String json = jss.Serialize(query);

                Response.Write(json);
                Response.End();
            }
            // Vencimientos
            else if (vc == "23")
            {
                string consulta = "";
                consulta += " SELECT cs.id, cs.monto_doc, CONVERT(varchar(10),cs.fecha,111) as fecha_inicio, (select top 1 b.nombrecliente from cobranza b where b.rutcliente = cs.rutcliente)  as nombrecliente, cs.num_factura, cs.rutcliente,  ";
                consulta += " color = CASE  ";
                consulta += "WHEN DATEDIFF(day,cs.fecha_venc, GETDATE()) >= 0 and DATEDIFF(day,cs.fecha_venc, GETDATE()) <= 5 THEN  '#BFFF00' ";
                consulta += "WHEN DATEDIFF(day,cs.fecha_venc, GETDATE())  >= 6 and DATEDIFF(day,cs.fecha_venc, GETDATE()) <= 10 THEN '#FFFF00'   ";
                consulta += "WHEN DATEDIFF(day,cs.fecha_venc, GETDATE())  >= 11 and DATEDIFF(day,cs.fecha_venc, GETDATE()) <= 15 THEN '#FFBF00' ";
                consulta += "WHEN DATEDIFF(day,cs.fecha_venc, GETDATE())  >= 16 and DATEDIFF(day,cs.fecha_venc, GETDATE()) <= 20 THEN '#FF4000' ";
                consulta += "WHEN DATEDIFF(day,cs.fecha_venc, GETDATE())  >= 21 and DATEDIFF(day,cs.fecha_venc, GETDATE()) <= 25 THEN '#DF0101'   ";
                consulta += "WHEN DATEDIFF(day,cs.fecha_venc, GETDATE())  >= 26 THEN  '#FF0000' ";
                consulta += "WHEN DATEDIFF(day,cs.fecha_venc, GETDATE())  < 0 THEN  '#3ADF00'  ";
                consulta += "ELSE '#190707' END ";
                consulta += ", color2 = CASE  ";
                consulta += "WHEN DATEDIFF(day,cs.fecha_venc, GETDATE()) >= 0 and DATEDIFF(day,cs.fecha_venc, GETDATE()) <= 5 THEN  '#000000' ";
                consulta += "WHEN DATEDIFF(day,cs.fecha_venc, GETDATE())  >= 6 and DATEDIFF(day,cs.fecha_venc, GETDATE()) <= 10 THEN '#000000'   ";
                consulta += "WHEN DATEDIFF(day,cs.fecha_venc, GETDATE())  >= 11 and DATEDIFF(day,cs.fecha_venc, GETDATE()) <= 15 THEN '#000000' ";
                consulta += "WHEN DATEDIFF(day,cs.fecha_venc, GETDATE())  >= 16 and DATEDIFF(day,cs.fecha_venc, GETDATE()) <= 20 THEN '#FFFFFF' ";
                consulta += "WHEN DATEDIFF(day,cs.fecha_venc, GETDATE())  >= 21 and DATEDIFF(day,cs.fecha_venc, GETDATE()) <= 25 THEN '#FFFFFF'   ";
                consulta += "WHEN DATEDIFF(day,cs.fecha_venc, GETDATE())  >= 26 THEN  '#FFFFFF' ";
                consulta += "WHEN DATEDIFF(day,cs.fecha_venc, GETDATE())  < 0 THEN  '#000000'  ";
                consulta += "ELSE '#190707' END ";
                consulta += "FROM COBRANZA_SEGUIMIENTO cs where cs.usuario = '" + user + "' AND RUTCLIENTE = CS.RUTCLIENTE ";

                if (filtro_fecha != "")
                {
                    consulta += filtro_fecha;
                }
                if (tipo_doc != "")
                {
                    consulta += tipo_doc + " and estado_doc <> 0  "; ;
                }
                if (cl != "")
                {
                    consulta += cl;
                }

                dt = db.consultar(consulta);

                var query = from item in dt.AsEnumerable()
                            where 1 == 1
                            select new Eventos_calendario
                            {
                                id = Convert.ToString(item["id"]).Replace("'", ""),
                                title = Convert.ToString(item["num_factura"]).Replace("'", "") + " - " + Convert.ToString(item["nombrecliente"]).Replace("'", ""),
                                rut_cliente = Convert.ToString(item["rutcliente"]).Replace("'", ""),
                                start = Convert.ToString(item["fecha_inicio"]).Replace("/", "-"),
                                backgroundColor = Convert.ToString(item["color"]).Replace("'", ""),
                                factura = Convert.ToString(item["num_factura"]).Replace("'", ""),
                                textColor = Convert.ToString(item["color2"]).Replace("'", "")
                            };

                JavaScriptSerializer jss = new JavaScriptSerializer();
                String json = jss.Serialize(query);

                Response.Write(json);
                Response.End();
            }
            // Pagos - Abonos
            else if (vc == "24")
            {
                string consulta = "";
                consulta += "SELECT ca.descripcion, ca.id_cobranza, ca.monto,  CAST( CASE when ca.tipo_doc = 'efectivo' then CONVERT(varchar(10),ca.fecha,111) else CONVERT(varchar(10),ca.vencimiento,111) end as varchar(10)) as fecha_inicio, (select top 1 b.nombrecliente from cobranza b where b.rutcliente = cs.rutcliente)  as nombrecliente,cs.num_factura, ";
                consulta += "'#3A01DF' as color, ISNULL(cb.cod_banco, '') as cod_banco FROM Cobranza_pagos ca INNER JOIN COBRANZA_SEGUIMIENTO cs on cs.id = ca.id_cobranza LEFT JOIN COBRANZA_BANCOS CB on CA.BANCO = CB.ID  where 1=1 ";

                if (filtro_fecha != "")
                {
                    consulta += filtro_fecha;
                }
                if (tipo_doc != "")
                {
                    consulta += tipo_doc;
                }
                if (cl != "")
                {
                    consulta += cl;
                }

                dt = db.consultar(consulta);

                var query = from item in dt.AsEnumerable()
                            where 1 == 1
                            select new Eventos_calendario
                            {
                                id = Convert.ToString(item["id_cobranza"]).Replace("'", ""),
                                title = Convert.ToString(item["cod_banco"]).Replace("'", "") + Convert.ToString(item["num_factura"]).Replace("'", "") + " - " + Convert.ToString(item["descripcion"]).Replace("'", ""),
                                start = Convert.ToString(item["fecha_inicio"]).Replace("/", "-"),
                                backgroundColor = Convert.ToString(item["color"]).Replace("'", "")
                            };

                JavaScriptSerializer jss = new JavaScriptSerializer();
                String json = jss.Serialize(query);

                Response.Write(json);
                Response.End();
            }
            // Pagos - F
            else if (vc == "55")
            {
                string consulta = "";
                consulta += "SELECT ca.descripcion,   (select top 1 b.id from v_cobranza b where b.factura = ca.id_cobranza and b.tipo_doc = 'IN') as id ,ca.id_cobranza as factura,CONVERT(varchar(10),ca.fecha,111) as fecha_inicio,  (select top 1 b.nombrecliente from cobranza b where b.num_factura = ca.id_cobranza)  as nombrecliente, convert(varchar, ca.monto) as monto_doc, (select top 1 b.rutcliente  from cobranza b where b.num_factura = ca.id_cobranza)  as rutcliente,ca.id_cobranza as factura , ";
                consulta += "'#FF9100' as color, ca.tipo_doc FROM Cobranza_pagos ca ";

                if (filtro_fecha != "")
                {
                    consulta += filtro_fecha;
                }
                //if (tipo_doc != "")
                //{
                //    consulta += tipo_doc;
                //}
                if (cl != "")
                {
                    consulta += cl;
                }

                dt = db.consultar(consulta);


                string fecha = "";
                int cont = 1;
                foreach (DataRow r in dt.Rows)
                {
                    if (cont == 1)
                    {
                        fecha = r[3].ToString();
                    }
                    if (fecha == r[3].ToString())
                    {
                        r[5] = cont + "-" + r[5].ToString();
                        cont++;
                    }
                    else
                    {
                        cont = 1;
                        r[5] = cont + "-" + r[5].ToString();
                        fecha = r[3].ToString();
                        cont++;
                    }
                }

                var query = from item in dt.AsEnumerable()
                            where 1 == 1
                            //select new Eventos_calendario
                            //{
                            //    id = Convert.ToString(item["id_cobranza"]).Replace("'", ""),
                            //    title = Convert.ToString(item["cod_banco"]).Replace("'", "") + Convert.ToString(item["num_factura"]).Replace("'", "") + " - " + Convert.ToString(item["descripcion"]).Replace("'", ""),
                            //    start = Convert.ToString(item["fecha_inicio"]).Replace("/", "-"),
                            //    backgroundColor = Convert.ToString(item["color"]).Replace("'", "")
                            //};
                            select new Eventos_calendario
                            {
                                id = Convert.ToString(item["id"]).Replace("'", ""),
                                title = calcula(item["monto_doc"], item["factura"], item["nombrecliente"]),
                                start = Convert.ToString(item["fecha_inicio"]).Replace("/", "-"),
                                factura = Convert.ToString(item["factura"]).Replace("'", ""),
                                rut_cliente = Convert.ToString(item["rutcliente"]).Replace("'", ""),
                                backgroundColor = Convert.ToString(item["color"]).Replace("'", ""),
                                tipo_doc = Convert.ToString(item["tipo_doc"]).Replace("'", "")
                            };

                JavaScriptSerializer jss = new JavaScriptSerializer();
                String json = jss.Serialize(query);

                Response.Write(json);
                Response.End();
            }

        }

        private string calcula(object p1, object p2, object p3)
        {
            string final = "";
            string monto = "";
            try
            {
                monto = p1.ToString().Substring(0, p1.ToString().IndexOf("."));
            }
            catch { monto = p1.ToString(); }
            string factura = p2.ToString();
            string nombrecliente = p3.ToString();
            if (monto.Substring(0, 1) == " ")
            {
                final = "   " + puntos_miles(monto.Trim()) + "(TOTAL)";
            }
            else
            {
                List<string> qwe = monto.Split('-').ToList();
                if (qwe.Count >= 3)
                {
                    final = qwe[0].ToString() + "- (-)" + puntos_miles(qwe[2].ToString()) + "(" + factura.Trim() + ")" + nombrecliente;
                }
                else
                {
                    final = qwe[0].ToString() + "-" + puntos_miles(qwe[1].ToString()) + "(" + factura.Trim() + ")" + nombrecliente;
                }
            }
            //puntos_miles(Convert.ToString(item["monto_doc"]).Replace("'", "").Substring(0, (item["monto_doc"]).ToString().IndexOf(","))) + "(" + Convert.ToString(item["factura"]).Replace("'", "") + ")" + Convert.ToString(item["nombrecliente"]).Replace("'", ""),

            return final;
        }

        private string puntos_miles(string p)
        {

            double d;
            double.TryParse(p, out d);
            string aux = "";
            if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
            return aux;
        }
    }
}