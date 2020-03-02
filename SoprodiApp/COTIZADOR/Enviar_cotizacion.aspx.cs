using CRM.BusinessLayer;
using CRM.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
//using CreatePDF;
using System.IO;
using System.Net;
using ThinxsysFramework;
using System.Collections.Generic;
using SoprodiApp.entidad;
using SoprodiApp.negocio;
using System.Web.Security;
using System.Web.UI;

namespace CRM
{
    public partial class Enviar_cotizacion : System.Web.UI.Page
    {
        iTextSharp.text.Font fuente_normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuente_negrita = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font titulo_font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font subtitulo_font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                bool enviar_correo = false;
                string id_cotizacion = "";
                DBUtil db = new DBUtil();
                usuarioEntity vend = new usuarioEntity();
                vend = (usuarioEntity)(Session["usuario"]);
                if (vend != null)
                {
                    if (Request.QueryString["idctz"] != null)
                    {
                        id_cotizacion = Request.QueryString["idctz"].ToString();
                    }
                    if (Request.QueryString["cr"] != null)
                    {
                        if (Request.QueryString["cr"].ToString() == "SI")
                        {
                            enviar_correo = true;
                        }
                    }
                    if (id_cotizacion != "")
                    {
                        ctz_cotizacionEntity ctz = new ctz_cotizacionEntity();
                        ctz.id_cotizacion = int.Parse(id_cotizacion);

                        if (ctz_cotizacionBO.encontrar(ref ctz) == "OK")
                        {
                            if (ctz.cod_vendedor == vend.cod_usuario)
                            {
                                // VEND CORREO DE REYES
                                DataTable dt_reyes = db.consultar("select * from usuariowebphone where cod_usuario = '" + vend.cod_usuario + "'");
                                string correo_de_reyes = "";
                                string pass_de_reyes = "";
                                string telefono_vendedor = "";
                                if (dt_reyes.Rows.Count > 0)
                                {
                                    try
                                    {
                                        correo_de_reyes = dt_reyes.Rows[0]["correo_reyes"].ToString();
                                        pass_de_reyes = dt_reyes.Rows[0]["pass_correo"].ToString();
                                        telefono_vendedor = dt_reyes.Rows[0]["phone"].ToString();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                //
                                // CREAR EL RESPALDO DE LA COTIZACION
                                ctz_log_cotizacionEntity ctz_log = new ctz_log_cotizacionEntity();
                                ctz_log.id_cotizacion = ctz.id_cotizacion;
                                ctz_log.nombre_cotizacion = ctz.nombre_cotizacion;
                                ctz_log.descripcion = ctz.descripcion;
                                ctz_log.num_columnas = ctz.num_columnas;
                                ctz_log.columnas_precio = ctz.columnas_precio;
                                ctz_log.cod_vendedor = ctz.cod_vendedor;
                                ctz_log.fecha_envio = DateTime.Now;
                                ctz_log.rut_cliente = " ";
                                ctz_log.correo_cliente = " ";
                                ctz_log.estado_correo = " ";
                                string scope = "";
                                scope = ctz_log_cotizacionBO.registrar_scope(ctz_log);
                                if (scope != "")
                                {
                                    ctz_log.correlativo = int.Parse(db.Scalar("select correlativo from ctz_log_cotizacion where id_cotizacion_log = " + scope).ToString());
                                    string sql_query = "";
                                    sql_query += "INSERT INTO CTZ_LOG_Cotizacion_det ( " +
                                          " [id_cotizacion_log] " +
                                          " ,[producto] ,[nom_producto] ,[cod_bodega_1] ,[cod_bodega_2] ,[cod_bodega_3] ,[cantidad_1] ,[cantidad_2] ,[cantidad_3] ,[precio_1] ,[precio_2] " +
                                          " ,[precio_3] ,[precio_unitario_1] ,[precio_unitario_2] ,[precio_unitario_3] ,[descuento_1] ,[descuento_2] ,[descuento_3] ,[precio_con_descuento_1] ,[precio_con_descuento_2] " +
                                          " ,[precio_con_descuento_3] ,[precio_con_descuento_unitario_1] ,[precio_con_descuento_unitario_2] ,[precio_con_descuento_unitario_3] ,[total_1] ,[total_2] ,[total_3] " +
                                          " ,[total_iva_1] ,[total_iva_2] ,[total_iva_3] ,[unidad_equivale]) " +

                                          " SELECT " + scope + " " +
                                          " ,[producto] ,[nom_producto] ,[cod_bodega_1] ,[cod_bodega_2] ,[cod_bodega_3] ,[cantidad_1] ,[cantidad_2] ,[cantidad_3] ,[precio_1] ,[precio_2] " +
                                          " ,[precio_3] ,[precio_unitario_1] ,[precio_unitario_2] ,[precio_unitario_3] ,[descuento_1] ,[descuento_2] ,[descuento_3] ,[precio_con_descuento_1] ,[precio_con_descuento_2] " +
                                          " ,[precio_con_descuento_3] ,[precio_con_descuento_unitario_1] ,[precio_con_descuento_unitario_2] ,[precio_con_descuento_unitario_3] ,[total_1] ,[total_2] ,[total_3] " +
                                          " ,[total_iva_1] ,[total_iva_2] ,[total_iva_3] ,[unidad_equivale] " +

                                          " FROM CTZ_Cotizacion_det WHERE id_cotizacion = " + ctz.id_cotizacion + ";" +
                                          // SERVICIOS
                                          " insert into CTZ_LOG_Cotizaciones_servicios (id_cotizacion_log, cod_servicio, nombre_servicio, valor_servicio, tipo_servicio) " +
                                          " select " + scope + ",  cod_servicio, nombre_servicio, valor_servicio, tipo_servicio from CTZ_Cotizaciones_servicios where id_cotizacion = " + ctz.id_cotizacion + ";";

                                    db.Scalar(sql_query);
                                    // FIN CREACION LOG DE LA COTIZACION                               
                                    List<correos_pdf> correos_pdf = new List<correos_pdf>();
                                    correos_pdf = (List<correos_pdf>)Session["ctz_correos_pdf"];
                                    foreach (correos_pdf x in correos_pdf)
                                    {
                                        Document doc = new Document();
                                        string path = Server.MapPath("COTIZACIONES");
                                        string nombre_pdf = "ctz_" + ctz_log.correlativo + "_" + vend.cod_usuario + "_" + x.rutcliente;
                                        string n_file = "/" + nombre_pdf.Replace(",", "").Replace(".", "").Replace(" ", "").Replace("/", "") + ".pdf";
                                        try
                                        {
                                            System.IO.File.Delete(path + n_file);
                                        }
                                        catch (System.IO.IOException ex)
                                        {

                                        }
                                        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path + n_file, FileMode.Create));
                                        try
                                        {
                                            ITextEvents PageEventHandler = new ITextEvents()
                                            {
                                                ImageFooter = iTextSharp.text.Image.GetInstance(Server.MapPath("~/COTIZADOR/LOGOS/banner.jpg"))
                                            };
                                            writer.PageEvent = PageEventHandler;

                                            DataTable dt_detalle = new DataTable();
                                            dt_detalle = db.consultar("select * from V_CTZ_COTIZACION_DET2 where id_cotizacion = " + ctz.id_cotizacion + " order by nom_categ, nom_producto");

                                            // CONFIGURACION PDF
                                            //doc.SetPageSize(PageSize.LEGAL.Rotate());
                                            doc.SetMargins(10f, 10f, 20f, 200f);
                                            doc.AddTitle("Cotización Soprodi");
                                            doc.AddCreator(vend.nombre_);
                                            // ------------ FONTS
                                            doc.Open();

                                            // ENCABEZADO (2 Logos y el encabezado superior).
                                            PdfPTable tabla_encabezado = new PdfPTable(3);
                                            tabla_encabezado.LockedWidth = true;
                                            tabla_encabezado.TotalWidth = 575f;
                                            float[] widths_enc = new float[] { 100f, 375f, 100f };
                                            tabla_encabezado.SetWidths(widths_enc);

                                            // AGREGAR LOGOS                                    
                                            iTextSharp.text.Image imagen = creaimagen("~/COTIZADOR/LOGOS/dereyes.png", 80, "c");
                                            iTextSharp.text.Image imagen2 = creaimagen("~/COTIZADOR/LOGOS/soprodi.jpg", 80, "c");

                                            // PRIMERA IMAGEN
                                            agregaimagen(ref tabla_encabezado, imagen, 0, "c");
                                            // ************************************************

                                            // ENCABEZADO (DATOS SOPRODI)
                                            PdfPTable titulo = new PdfPTable(1);
                                            titulo.WidthPercentage = 100;
                                            agregacelda(ref titulo, "COTIZACION DE PRODUCTOS", titulo_font, 0, "c");
                                            agregacelda(ref titulo, " ", fuente_negrita, 0, "c");
                                            agregacelda(ref titulo, "Vendedor: " + vend.nombre_, fuente_negrita, 0, "c");
                                            agregacelda(ref titulo, "Correo: " + vend.correo, fuente_negrita, 0, "c");
                                            try
                                            {
                                                agregacelda(ref titulo, "Telefono: " + telefono_vendedor, fuente_negrita, 0, "c");
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                            //agregacelda(ref titulo, "SOCIEDAD PRODUCTORA Y DISTRIBUIDORA S.A", titulo_font, 0, "c");
                                            //agregacelda(ref titulo, "SOPRODI S.A", titulo_font, 0, "c");
                                            //agregacelda(ref titulo, "IMPORTACIONES, EXPORTACIONES,", fuente_negrita, 0, "c");
                                            //agregacelda(ref titulo, "DISTRIBUCIÓN DE PRODUCTOS AGRÍCOLAS", fuente_negrita, 0, "c");
                                            //agregacelda(ref titulo, "TRANSPORTES DE CARGA TERRESTRE,", fuente_negrita, 0, "c");
                                            //agregacelda(ref titulo, "ALMACENAJES, ABARROTES, TRANSPORTE MARÍTIMO Y DE CABOTAJE DE CARGA", fuente_negrita, 0, "c");
                                            //agregacelda(ref titulo, "Casa Matriz: Paradero 9 1/2, Camino Troncal San Pedro - Quillota - Casilla 7 Correo Quillota", fuente_normal, 0, "c");
                                            //agregacelda(ref titulo, "Fono: (56 33) 2292500 - Fax: (56 33) 2318139", fuente_normal, 0, "c");
                                            //agregacelda(ref titulo, "Email: soprodi@soprodi.cl", fuente_normal, 0, "c");
                                            agregatabla(ref tabla_encabezado, titulo, 0, "c");
                                            // ***********************************************************************************************************************************************

                                            // SEGUNDA IMAGEN Y NUMERO DE COTIZACION
                                            PdfPTable columna3 = new PdfPTable(1);
                                            columna3.WidthPercentage = 100;
                                            agregaimagen(ref columna3, imagen2, 0, "c");
                                            agregacelda(ref columna3, " ", titulo_font, 0, "c");

                                            PdfPTable td_columna3 = new PdfPTable(1);
                                            td_columna3.WidthPercentage = 100;

                                            agregacelda(ref td_columna3, "COTIZACIÓN #", titulo_font, 0, "c");
                                            agregacelda(ref td_columna3, ctz_log.correlativo.ToString(), titulo_font, 0, "c");
                                            agregacelda(ref td_columna3, "Válido hasta", fuente_normal, 0, "c");
                                            agregacelda(ref td_columna3, ctz.fecha_creacion.AddDays(2).ToString("dd/MM/yyyy"), fuente_negrita, 0, "c");
                                            agregatabla(ref columna3, td_columna3, 1, "c");

                                            agregatabla(ref tabla_encabezado, columna3, 0, "c");
                                            // *****************************************************************************

                                            // Insertamos el encabezado
                                            doc.Add(tabla_encabezado);
                                            doc.Add(Chunk.NEWLINE);
                                            SaltoLinea(ref doc);

                                            // SUB ENCABEZADO
                                            PdfPTable tabla_subencabezado = new PdfPTable(1);
                                            tabla_subencabezado.WidthPercentage = 100;

                                            PdfPTable td_tabla_subencabezado = new PdfPTable(6);
                                            td_tabla_subencabezado.TotalWidth = 575f;
                                            float[] widthsubencabezado = new float[] { 50f, 10f, 170f, 50f, 10f, 170f };
                                            td_tabla_subencabezado.SetWidths(widthsubencabezado);

                                            agregacelda(ref td_tabla_subencabezado, "Señor(es)", fuente_negrita, 0, "i");
                                            agregacelda(ref td_tabla_subencabezado, ":", fuente_negrita, 0, "i");
                                            agregacelda(ref td_tabla_subencabezado, x.nombrecliente, fuente_normal, 0, "i");

                                            agregacelda(ref td_tabla_subencabezado, "R.U.T", fuente_negrita, 0, "i");
                                            agregacelda(ref td_tabla_subencabezado, ":", fuente_negrita, 0, "i");
                                            agregacelda(ref td_tabla_subencabezado, x.rutcliente, fuente_normal, 0, "i");

                                            agregacelda(ref td_tabla_subencabezado, "Giro", fuente_negrita, 0, "i");
                                            agregacelda(ref td_tabla_subencabezado, ":", fuente_negrita, 0, "i");
                                            agregacelda(ref td_tabla_subencabezado, x.giro, fuente_normal, 0, "i");

                                            agregacelda(ref td_tabla_subencabezado, "Comuna", fuente_negrita, 0, "i");
                                            agregacelda(ref td_tabla_subencabezado, ":", fuente_negrita, 0, "i");
                                            agregacelda(ref td_tabla_subencabezado, x.ciudad, fuente_normal, 0, "i");

                                            agregacelda(ref td_tabla_subencabezado, "Dirección", fuente_negrita, 0, "i");
                                            agregacelda(ref td_tabla_subencabezado, ":", fuente_negrita, 0, "i");
                                            agregacelda(ref td_tabla_subencabezado, x.direccion, fuente_normal, 0, "i");

                                            agregacelda(ref td_tabla_subencabezado, "Ciudad", fuente_negrita, 0, "i");
                                            agregacelda(ref td_tabla_subencabezado, ":", fuente_negrita, 0, "i");
                                            agregacelda(ref td_tabla_subencabezado, x.ciudad, fuente_normal, 0, "i");

                                            agregatabla(ref tabla_subencabezado, td_tabla_subencabezado, 1, "i");

                                            doc.Add(tabla_subencabezado);

                                            // SUB ENCABEZADO 2
                                            PdfPTable tabla_subencabezado2 = new PdfPTable(3);
                                            tabla_subencabezado2.WidthPercentage = 100;

                                            PdfPTable td_sub2_1 = new PdfPTable(1);
                                            agregacelda(ref td_sub2_1, "FECHA DE EMISION", fuente_negrita, 0, "c");
                                            agregacelda(ref td_sub2_1, DateTime.Now.ToString("dd/MM/yyyy"), fuente_normal, 0, "c");
                                            agregatabla(ref tabla_subencabezado2, td_sub2_1, 1, "i");


                                            PdfPTable td_sub2_2 = new PdfPTable(1);
                                            agregacelda(ref td_sub2_2, "PLAZO DE PAGO", fuente_negrita, 0, "c");
                                            agregacelda(ref td_sub2_2, "CONVENIDO ENTRE LAS PARTES", fuente_normal, 0, "c");
                                            agregatabla(ref tabla_subencabezado2, td_sub2_2, 1, "i");

                                            PdfPTable td_sub2_5 = new PdfPTable(1);
                                            agregacelda(ref td_sub2_5, "VENDEDOR", fuente_negrita, 0, "c");
                                            agregacelda(ref td_sub2_5, vend.nombre_, fuente_normal, 0, "c");
                                            agregatabla(ref tabla_subencabezado2, td_sub2_5, 1, "i");

                                            doc.Add(tabla_subencabezado2);
                                            // ****************************************************************************

                                            SaltoLinea(ref doc);

                                            // RECORRER DETALLE// DETALLE
                                            string tabla_correo = ""; // <--------------- PARA EL CORREO
                                            tabla_correo += "<hr><table style='width:100%' border=1><thead><tr>";
                                            int contador_bodegas = 0;
                                            int num_columnas = 2;
                                            for (int i = 1; i <= 3; i++)
                                            {
                                                if (dt_detalle.Rows[0]["cod_bodega_" + i].ToString() != "NO")
                                                {
                                                    contador_bodegas++;
                                                    num_columnas = num_columnas + 5;
                                                }
                                            }
                                            PdfPTable tablaOTZ = new PdfPTable(num_columnas);
                                            tablaOTZ.LockedWidth = true;
                                            tablaOTZ.TotalWidth = 575f;
                                            float[] widthtablaOTZ = new float[] { 0f };
                                            if (num_columnas == 7)
                                            {
                                                widthtablaOTZ = new float[] { 25f, 200f, 86f, 86f, 86f, 86f, 86f };
                                            }
                                            else if (num_columnas == 12)
                                            {
                                                widthtablaOTZ = new float[] { 25f, 200f, 43f, 43f, 43f, 43f, 43f, 43f, 43f, 43f, 43f, 43f };
                                            }
                                            else
                                            {
                                                widthtablaOTZ = new float[] { 25f, 60f, 29f, 29f, 29f, 29f, 29f, 29f, 29f, 29f, 29f, 29f, 29f, 29f, 29f, 29f, 29f };
                                            }
                                            tablaOTZ.SetWidths(widthtablaOTZ);
                                            //tablaOTZ.WidthPercentage = 100;

                                            // COLSPANS SUPERIORES
                                            PdfPCell td1 = new PdfPCell(new Phrase(" ", fuente_negrita));
                                            td1.Colspan = 2;
                                            td1.BorderWidth = 1;
                                            tablaOTZ.AddCell(td1);

                                            for (int i = 1; i <= contador_bodegas; i++)
                                            {
                                                PdfPCell td2 = new PdfPCell(new Phrase(dt_detalle.Rows[0]["bod" + i].ToString(), fuente_negrita));
                                                td2.Colspan = 5;
                                                td2.BorderWidth = 1;
                                                tablaOTZ.AddCell(td2);
                                            }
                                            // FIN COLSPANS

                                            agregacelda(ref tablaOTZ, "Nº", fuente_negrita, 1, "c");
                                            tabla_correo += "<th>Nº</th>";
                                            //agregacelda(ref tablaOTZ, "Cod. Producto", fuente_negrita, 1, "c");
                                            agregacelda(ref tablaOTZ, "Producto", fuente_negrita, 1, "c");
                                            tabla_correo += "<th style='text-align:left'>Producto</th>";
                                            for (int i = 1; i <= contador_bodegas; i++)
                                            {
                                                agregacelda(ref tablaOTZ, "Precio", fuente_negrita, 1, "c");
                                                agregacelda(ref tablaOTZ, "Precio Unit.", fuente_negrita, 1, "c");
                                                agregacelda(ref tablaOTZ, "Precio Unit c/IVA", fuente_negrita, 1, "c");
                                                agregacelda(ref tablaOTZ, "Cantidad", fuente_negrita, 1, "c");
                                                agregacelda(ref tablaOTZ, "Subtotal Neto", fuente_negrita, 1, "c");

                                                tabla_correo += "<th style='text-align:right'>Precio</th>";
                                                tabla_correo += "<th style='text-align:right'>Precio Unit.</th>";
                                                tabla_correo += "<th style='text-align:right'>Precio Unit c/IVA</th>";
                                                tabla_correo += "<th style='text-align:right'>Cantidad</th>";
                                                tabla_correo += "<th style='text-align:right'>Subtotal Neto</th>";
                                            }

                                            tabla_correo += "</tr></thead><tbody>";
                                            int contador = 1;
                                            string aux_cat = "";
                                            foreach (DataRow dr in dt_detalle.Rows)
                                            {

                                                if (aux_cat != dr["nom_categ"].ToString())
                                                {
                                                    //
                                                    PdfPCell td_categoria2 = new PdfPCell(new Phrase(" ", fuente_negrita));
                                                    PdfPCell td_categoria = new PdfPCell(new Phrase(dr["nom_categ"].ToString(), fuente_negrita));
                                                    if (contador_bodegas == 1)
                                                    {
                                                        td_categoria.Colspan = 7;
                                                        td_categoria2.Colspan = 7;
                                                        tabla_correo += "<tr><td colspan=7 style='border:0px solid black'> </td></tr>";
                                                        tabla_correo += "<tr><td colspan=7 style='border:0px solid black'>" + dr["nom_categ"].ToString() + "</td></tr>";
                                                    }
                                                    else if (contador_bodegas == 2)
                                                    {
                                                        td_categoria.Colspan = 12;
                                                        td_categoria2.Colspan = 12;
                                                        tabla_correo += "<tr><td colspan=12 style='border:0px solid black'> </td></tr>";
                                                        tabla_correo += "<tr><td colspan=12 style='border:0px solid black'>" + dr["nom_categ"].ToString() + "</td></tr>";
                                                    }
                                                    else
                                                    {
                                                        td_categoria.Colspan = 17;
                                                        td_categoria2.Colspan = 17;
                                                        tabla_correo += "<tr><td colspan=17 style='border:0px solid black'> </td></tr>";
                                                        tabla_correo += "<tr><td colspan=17 style='border:0px solid black'>" + dr["nom_categ"].ToString() + "</td></tr>";
                                                    }
                                                    td_categoria.BorderWidth = 0;
                                                    td_categoria2.BorderWidth = 0;

                                                    tablaOTZ.AddCell(td_categoria2);
                                                    tablaOTZ.AddCell(td_categoria);
                                                    //
                                                    tabla_correo += "<tr>";
                                                    agregacelda(ref tablaOTZ, contador.ToString(), fuente_normal, 1, "c");
                                                    tabla_correo += "<td>" + contador.ToString() + "</td>";
                                                    //agregacelda(ref tablaOTZ, dr["producto"].ToString(), fuente_normal, 1, "c");
                                                    agregacelda(ref tablaOTZ, dr["nomcod"].ToString(), fuente_normal, 1, "i");
                                                    tabla_correo += "<td>" + dr["nomcod"].ToString() + "</td>";
                                                    for (int i = 1; i <= contador_bodegas; i++)
                                                    {
                                                        agregacelda(ref tablaOTZ, "$ " + int.Parse(dr["precio_con_descuento_" + i].ToString()).ToString("#,##0"), fuente_normal, 1, "d");
                                                        agregacelda(ref tablaOTZ, "$ " + int.Parse(dr["precio_con_descuento_unitario_" + i].ToString()).ToString("#,##0"), fuente_normal, 1, "d");
                                                        agregacelda(ref tablaOTZ, "$ " + (int.Parse(dr["precio_con_descuento_unitario_" + i].ToString()) + ((int.Parse(dr["precio_con_descuento_unitario_" + i].ToString()) * 19) / 100)).ToString("#,##0"), fuente_normal, 1, "d");
                                                        agregacelda(ref tablaOTZ, dr["cantidad_" + i].ToString(), fuente_normal, 1, "d");
                                                        agregacelda(ref tablaOTZ, "$ " + int.Parse(dr["total_" + i].ToString()).ToString("#,##0"), fuente_negrita, 1, "d");

                                                        tabla_correo += "<td style='text-align:right'><b>$ " + int.Parse(dr["precio_con_descuento_" + i].ToString()).ToString("#,##0") + "</b></td>";
                                                        tabla_correo += "<td style='text-align:right'><b>$ " + int.Parse(dr["precio_con_descuento_unitario_" + i].ToString()).ToString("#,##0") + "</b></td>";
                                                        tabla_correo += "<td style='text-align:right'><b>$ " + (int.Parse(dr["precio_con_descuento_unitario_" + i].ToString()) + ((int.Parse(dr["precio_con_descuento_unitario_" + i].ToString()) * 19) / 100)).ToString("#,##0") + "</b></td>";
                                                        tabla_correo += "<td style='text-align:right'><b> " + dr["cantidad_" + i].ToString() + "</b></td>";
                                                        tabla_correo += "<td style='text-align:right'><b>$ " + int.Parse(dr["total_" + i].ToString()).ToString("#,##0") + "</b></td>";
                                                    }
                                                    aux_cat = dr["nom_categ"].ToString();
                                                }
                                                else
                                                {
                                                    tabla_correo += "<tr>";
                                                    agregacelda(ref tablaOTZ, contador.ToString(), fuente_normal, 1, "c");
                                                    tabla_correo += "<td>" + contador.ToString() + "</td>";
                                                    //agregacelda(ref tablaOTZ, dr["producto"].ToString(), fuente_normal, 1, "c");
                                                    agregacelda(ref tablaOTZ, dr["nomcod"].ToString(), fuente_normal, 1, "i");
                                                    tabla_correo += "<td>" + dr["nomcod"].ToString() + "</td>";
                                                    for (int i = 1; i <= contador_bodegas; i++)
                                                    {
                                                        agregacelda(ref tablaOTZ, "$ " + int.Parse(dr["precio_con_descuento_" + i].ToString()).ToString("#,##0"), fuente_normal, 1, "d");
                                                        agregacelda(ref tablaOTZ, "$ " + int.Parse(dr["precio_con_descuento_unitario_" + i].ToString()).ToString("#,##0"), fuente_normal, 1, "d");
                                                        agregacelda(ref tablaOTZ, "$ " + (int.Parse(dr["precio_con_descuento_unitario_" + i].ToString()) + ((int.Parse(dr["precio_con_descuento_unitario_" + i].ToString()) * 19) / 100)).ToString("#,##0"), fuente_normal, 1, "d");
                                                        agregacelda(ref tablaOTZ, dr["cantidad_" + i].ToString(), fuente_normal, 1, "d");
                                                        agregacelda(ref tablaOTZ, "$ " + int.Parse(dr["total_" + i].ToString()).ToString("#,##0"), fuente_negrita, 1, "d");

                                                        tabla_correo += "<td style='text-align:right'><b>$ " + int.Parse(dr["precio_con_descuento_" + i].ToString()).ToString("#,##0") + "</b></td>";
                                                        tabla_correo += "<td style='text-align:right'><b>$ " + int.Parse(dr["precio_con_descuento_unitario_" + i].ToString()).ToString("#,##0") + "</b></td>";
                                                        tabla_correo += "<td style='text-align:right'><b>$ " + (int.Parse(dr["precio_con_descuento_unitario_" + i].ToString()) + ((int.Parse(dr["precio_con_descuento_unitario_" + i].ToString()) * 19) / 100)).ToString("#,##0") + "</b></td>";
                                                        tabla_correo += "<td style='text-align:right'><b> " + dr["cantidad_" + i].ToString() + "</b></td>";
                                                        tabla_correo += "<td style='text-align:right'><b>$ " + int.Parse(dr["total_" + i].ToString()).ToString("#,##0") + "</b></td>";
                                                    }
                                                }
                                                contador++;
                                                tabla_correo += "</tr>";
                                            }
                                            tabla_correo += "</tbody></table><br>";
                                            doc.Add(tablaOTZ);
                                            SaltoLinea(ref doc);

                                            // TOTALES 
                                            DataTable dt_totales = new DataTable();
                                            string sql = "";
                                            sql += " select bod1.nombre_bodega as nom1, bod2.nombre_bodega as nom2, bod3.nombre_bodega as nom3,  sum(total_1) as 'total_1', sum(total_2) as 'total_2' , sum(total_3) as 'total_3'  " +
                                                " ,sum(total_iva_1) as 'total_iva_1', sum(total_iva_2) as 'total_iva_2', sum(total_iva_3) as 'total_iva_3' " +
                                                " from CTZ_Cotizacion_det ctz left join CTZ_Bodegas bod1 on ctz.cod_bodega_1 = bod1.cod_bodega " +
                                                " left join CTZ_Bodegas bod2 on ctz.cod_bodega_2 = bod2.cod_bodega " +
                                                " left join CTZ_Bodegas bod3 on ctz.cod_bodega_3 = bod3.cod_bodega  " +
                                                " where id_cotizacion = " + id_cotizacion +
                                                " group by bod1.nombre_bodega, bod2.nombre_bodega,bod3.nombre_bodega ";
                                            dt_totales = db.consultar(sql);

                                            if (dt_totales.Rows.Count > 0)
                                            {
                                                PdfPTable tablaTotales = new PdfPTable(contador_bodegas);
                                                tablaTotales.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                if (contador_bodegas == 1) { tablaTotales.WidthPercentage = 20; } else if (contador_bodegas == 2) { tablaTotales.WidthPercentage = 40; } else if (contador_bodegas == 3) { tablaTotales.WidthPercentage = 60; }
                                                for (int i = 1; i <= contador_bodegas; i++)
                                                {
                                                    try
                                                    {
                                                        int total = int.Parse(dt_totales.Rows[0]["total_" + i].ToString());
                                                        int totaliva = int.Parse(dt_totales.Rows[0]["total_iva_" + i].ToString());
                                                        if (dt_totales.Rows[0]["nom" + i].ToString() != "NO")
                                                        {
                                                            PdfPTable tablaTotales_td = new PdfPTable(2);
                                                            tabla_correo += "<table style='width:30%' border=1>";
                                                            tabla_correo += "<tr>";
                                                            PdfPCell td_total = new PdfPCell(new Phrase(dt_totales.Rows[0]["nom" + i].ToString(), fuente_negrita));
                                                            tabla_correo += "<td colspan=2><b>" + dt_totales.Rows[0]["nom" + i].ToString() + "</b></td></tr>";
                                                            td_total.Colspan = 2;
                                                            td_total.BorderWidth = 1;
                                                            tablaTotales_td.AddCell(td_total);

                                                            agregacelda(ref tablaTotales_td, "Total Neto", fuente_negrita, 1, "i");
                                                            agregacelda(ref tablaTotales_td, "$ " + total.ToString("#,##0"), fuente_normal, 1, "d");
                                                            tabla_correo += "<tr><td>Total Neto</td><td><b>$" + total.ToString("#,##0") + "</b></td></tr>";
                                                            agregacelda(ref tablaTotales_td, "Total c/IVA", fuente_negrita, 1, "i");
                                                            agregacelda(ref tablaTotales_td, "$ " + totaliva.ToString("#,##0"), fuente_normal, 1, "d");
                                                            tabla_correo += "<tr><td>Total c/IVA</td><td><b>$" + totaliva.ToString("#,##0") + "</b></td></tr>";
                                                            tabla_correo += "</table><hr>";
                                                            agregatabla(ref tablaTotales, tablaTotales_td, 1, "c");
                                                        }
                                                    }
                                                    catch (Exception)
                                                    {

                                                    }
                                                }
                                                doc.Add(tablaTotales);
                                            }
                                            else
                                            {

                                            }
                                            SaltoLinea(ref doc);
                                            // SERVICIOS
                                            DataTable dt_servicios = new DataTable();
                                            dt_servicios = ctz_cotizaciones_serviciosBO.GetAll(" where id_cotizacion = " + id_cotizacion + " and tipo_servicio = 'POR SEPARADO' ");
                                            if (dt_servicios.Rows.Count > 0)
                                            {
                                                PdfPTable tabla_servicios = new PdfPTable(2);
                                                tabla_servicios.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                tabla_servicios.WidthPercentage = 40;

                                                agregacelda(ref tabla_servicios, "Servicio", fuente_negrita, 1, "i");
                                                agregacelda(ref tabla_servicios, "Valor", fuente_negrita, 1, "d");
                                                foreach (DataRow dr_servicio in dt_servicios.Rows)
                                                {
                                                    agregacelda(ref tabla_servicios, dr_servicio["nombre_servicio"].ToString(), fuente_normal, 1, "i");
                                                    agregacelda(ref tabla_servicios, "$" + int.Parse(dr_servicio["valor_servicio"].ToString()).ToString("#,##0"), fuente_normal, 1, "d"); ;
                                                }
                                                doc.Add(tabla_servicios);
                                            }
                                            //
                                            // FIN                                   
                                            SaltoLinea(ref doc);
                                            enc_correo_pdf enc_pdf = (enc_correo_pdf)Session["ENC_CORREO_PDF"];

                                            // COMENTARIO AGREGADO AL PDF
                                            PdfPTable tablacomentario = new PdfPTable(1);
                                            tablacomentario.HorizontalAlignment = Element.ALIGN_LEFT;
                                            tablacomentario.WidthPercentage = 100;
                                            agregacelda(ref tablacomentario, enc_pdf.COMENTARIO, fuente_normal);
                                            doc.Add(tablacomentario);
                                            //
                                            //
                                            SaltoLinea(ref doc);
                                            doc.NewPage();
                                            doc.Close();
                                            writer.Close();
                                            string pdfPath = Server.MapPath("~/COTIZADOR/COTIZACIONES/" + n_file);
                                            if (enviar_correo)
                                            {
                                                string htmlcorreo = "";
                                                htmlcorreo += "<div style='text-align:center;     display: block !important;' > ";
                                                htmlcorreo += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                                                htmlcorreo += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                                                htmlcorreo += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                                                htmlcorreo += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                                                htmlcorreo += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                                                htmlcorreo += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                                                htmlcorreo += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                                                htmlcorreo += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div> ";
                                                htmlcorreo += "</div>";
                                                htmlcorreo += "<div><img src='http://a58.imgup.net/Sopro4d9d.png' style='float:right; width:90px;'> </div>";
                                                if (enc_pdf.COMENTARIO != "")
                                                {
                                                    htmlcorreo += "<h4>" + enc_pdf.COMENTARIO + "</h4>";
                                                }
                                                else
                                                {
                                                    htmlcorreo += "<h4>Cliente: " + x.nombrecliente + " </h4>";
                                                }
                                                htmlcorreo += "<p>Con fecha: " + DateTime.Now.ToString("dd/MM/yyyy") + " envío la siguiente cotización adjunta. </p>";
                                                //htmlcorreo += "<p>" + enc_pdf.COMENTARIO + "</p>";
                                                // AGREGAR LISTA AQUI //
                                                htmlcorreo += tabla_correo;
                                                //
                                                htmlcorreo += "<p>Atte.,</p>";
                                                htmlcorreo += "<h4>" + vend.nombre_.ToUpper() + "</h4>";
                                                htmlcorreo += "<h4>" + telefono_vendedor + "</h4>";
                                                htmlcorreo += "<p>------------------------------------------</p>";
                                                try
                                                {
                                                    string fecha_lista_precio = db.Scalar("select convert(date,MAX(fecha),103) as 'fecha' from Stock_Excel_2").ToString();
                                                    htmlcorreo += "<p>Precios vigentes desde: <b>" + Convert.ToDateTime(fecha_lista_precio).ToString("dd/MM/yyyy") + "</b></p>";
                                                }
                                                catch (Exception ex)
                                                {

                                                }
                                                htmlcorreo += "<p>Precios válidos hasta: <b>" + DateTime.Now.AddDays(2).ToString("dd/MM/yyyy") + "</b></p>";
                                                htmlcorreo += "<p><b>Condiciones de entrega: </b></p>";
                                                htmlcorreo += "<p>1. Despachos por pallets (algunos ítems se pueden entregar por medio pallets).</p>";
                                                htmlcorreo += "<p>2. Plazo de pago de acuerdo a lo convenido entre las partes.</p>";
                                                htmlcorreo += "<br> Para más detalles diríjase a:  <a href='http://www.dereyes.cl/productos.php' > DeReyes.cl  </a> <span style='font:5px; color:gray; float:right;'>No Responder Este Correo</span> <br><br>";
                                                htmlcorreo += "<div style='text-align:center;     display: block !important;' > ";
                                                htmlcorreo += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                                                htmlcorreo += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                                                htmlcorreo += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                                                htmlcorreo += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                                                htmlcorreo += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                                                htmlcorreo += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
                                                htmlcorreo += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
                                                htmlcorreo += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div> ";
                                                htmlcorreo += "</div>";

                                                Correo cr = new Correo();
                                                if (x.correocliente != "")
                                                {
                                                    // PARA SOPRODI
                                                    string AUX_CC = "";
                                                    if (enc_pdf.CC == "")
                                                    {
                                                        AUX_CC = "rmc@soprodi.cl, mazocar@soprodi.cl, ialfaro@soprodi.cl, " + vend.correo;
                                                    }
                                                    else
                                                    {
                                                        AUX_CC = "rmc@soprodi.cl, mazocar@soprodi.cl, ialfaro@soprodi.cl, " + vend.correo + ", " + enc_pdf.CC;
                                                    }



                                                    string aux_asunto = enc_pdf.ASUNTO + " " + x.nombrecliente;
                                                    // PARA PRODUCCION
                                                    string respuesta_correo = cr.EnviarCorreoCotizacion(correo_de_reyes, pass_de_reyes, correo_de_reyes, x.correocliente, aux_asunto, htmlcorreo, AUX_CC, pdfPath, "COTIZADOR SOPRODI");
                                                    // PARA TESTING
                                                    //string respuesta_correo = cr.EnviarCorreoCotizacion(correo_de_reyes, pass_de_reyes, correo_de_reyes, "contacto.pveliz@gmail.com", enc_pdf.ASUNTO, htmlcorreo, enc_pdf.CC, pdfPath, "COTIZADOR SOPRODI");
                                                    if (respuesta_correo == "OK")
                                                    {
                                                        x.enviado_ok = true;
                                                    }
                                                    else
                                                    {
                                                       
                                                        //ScriptManager.RegisterStartupScript(Page, this.GetType(), "test", "<script language='javascript'> alert('"+respuesta_correo+"') </script>", false);
                                                        x.enviado_ok = false;
                                                        x.resp_error = respuesta_correo;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                WebClient client = new WebClient();
                                                Byte[] buffer = client.DownloadData(pdfPath);
                                                Response.ContentType = "application/pdf";
                                                Response.AddHeader("content-length", buffer.Length.ToString());
                                                Response.BinaryWrite(buffer);
                                                Response.Flush();
                                                Response.End();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            doc.Close();
                                            writer.Close();
                                            div_malo.Visible = true;
                                        }
                                    }
                                    //
                                    string html_result = "<h4>Resultados del envío de correos</h4><hr>" +
                                        "<table style='width:100%' class='table table-xs hide-columns-dynamically table-bordered compact'><thead>" +
                                        "<tr>" +
                                        "<th>CLIENTE</th>" +
                                        "<th>CORREO</th>" +
                                        "<th>ESTADO</th>" +
                                        "</tr>" +
                                        "<tbody>";
                                    div_bueno.Visible = true;
                                    foreach (correos_pdf x in correos_pdf)
                                    {
                                        ctz_log_enviadasEntity enviadas = new ctz_log_enviadasEntity();
                                        enviadas.id_cotizacion_log = int.Parse(scope);
                                        enviadas.cod_vendedor = vend.cod_usuario;
                                        enviadas.rut_cliente = x.rutcliente;
                                        enviadas.fecha_envio = DateTime.Now;
                                        enviadas.correo_cliente = x.correocliente;
                                        if (x.enviado_ok)
                                        {
                                            html_result += "<tr><td>" + x.nombrecliente + "</td><td>" + x.correocliente + "</td><td>ENVIADO OK</td></tr>";
                                            enviadas.estado_correo = "OK";
                                        }
                                        else
                                        {
                                            html_result += "<tr><td>" + x.nombrecliente + "</td><td>" + x.correocliente + "</td><td>NO SE PUDO ENVIAR: " + x.resp_error + "</td></tr>";
                                            enviadas.estado_correo = "NO";
                                        }
                                        // AGREGAR AL LOG
                                        try
                                        {
                                            ctz_log_enviadasBO.registrar(enviadas);
                                        }
                                        catch (Exception)
                                        {

                                        }
                                    }
                                    html_result += "</tbody></table>";
                                    div_bueno.InnerHtml = html_result;
                                }
                            }
                            else
                            {
                                div_malo.Visible = true;
                                div_malo.InnerHtml = "Esta cotización no es de su propiedad por favor evite ver otras cotizaciones que no le corresponden.";
                            }
                        }
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
            catch (Exception ex)
            {
                div_malo.Visible = true;
            }
        }

        public void SaltoLinea(ref Document x)
        {
            PdfPTable espacio = new PdfPTable(1);
            PdfPCell blanco = new PdfPCell(new Phrase(" "));
            blanco.BorderWidth = 0;
            espacio.AddCell(blanco);
            x.Add(espacio);
        }

        public iTextSharp.text.Image creaimagen(string url, int size, string alineamiento = "i")
        {
            iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(Server.MapPath(url));
            imagen.BorderWidth = 0;
            if (alineamiento == "i")
            {
                imagen.Alignment = Element.ALIGN_LEFT;
            }
            else if (alineamiento == "c")
            {
                imagen.Alignment = Element.ALIGN_CENTER;
            }
            else if (alineamiento == "d")
            {
                imagen.Alignment = Element.ALIGN_RIGHT;
            }
            float percentage = 0.0f;
            percentage = 100 / imagen.Width;
            imagen.ScalePercent(percentage * size);
            return imagen;
        }

        public void agregatabla(ref PdfPTable tabla, PdfPTable tabla_a_insertar, int borde = 0, string alineamiento = "i")
        {
            // BORDE 0 (sin borde) , 1 Con borde
            // ALINEAMIENTO i = Izq , C = Center , D = Derecha
            PdfPCell td = new PdfPCell(tabla_a_insertar);
            if (alineamiento == "i")
            {
                td.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else if (alineamiento == "c")
            {
                td.HorizontalAlignment = Element.ALIGN_CENTER;
            }
            else if (alineamiento == "d")
            {
                td.HorizontalAlignment = Element.ALIGN_RIGHT;
            }
            td.BorderWidth = borde;
            tabla.AddCell(td);
        }

        public void agregaimagen(ref PdfPTable tabla, iTextSharp.text.Image imagen, int borde = 0, string alineamiento = "i")
        {
            // BORDE 0 (sin borde) , 1 Con borde
            // ALINEAMIENTO i = Izq , C = Center , D = Derecha
            PdfPCell td = new PdfPCell(imagen);
            if (alineamiento == "i")
            {
                td.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else if (alineamiento == "c")
            {
                td.HorizontalAlignment = Element.ALIGN_CENTER;
            }
            else if (alineamiento == "d")
            {
                td.HorizontalAlignment = Element.ALIGN_RIGHT;
            }
            td.BorderWidth = borde;
            tabla.AddCell(td);
        }

        public void agregacelda(ref PdfPTable tabla, string texto, iTextSharp.text.Font fuente, int borde = 0, string alineamiento = "i", int tamaño = 100)
        {
            // BORDE 0 (sin borde) , 1 Con borde
            // ALINEAMIENTO i = Izq , C = Center , D = Derecha
            PdfPCell td = new PdfPCell(new Phrase(texto, fuente));
            if (alineamiento == "i")
            {
                td.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else if (alineamiento == "c")
            {
                td.HorizontalAlignment = Element.ALIGN_CENTER;
            }
            else if (alineamiento == "d")
            {
                td.HorizontalAlignment = Element.ALIGN_RIGHT;
            }
            td.BorderWidth = borde;
            //td.Width = tamaño;
            tabla.AddCell(td);
        }

        public class ITextEvents : PdfPageEventHelper
        {
            public Image ImageFooter { get; set; }
            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                ImageFooter.SetAbsolutePosition(0, 0);
            }
            public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
            {
                float percentage = 0f;
                percentage = 595 / ImageFooter.Width;
                ImageFooter.ScalePercent(percentage * 100);
                writer.DirectContent.AddImage(ImageFooter);
            }
        }
    }
}
