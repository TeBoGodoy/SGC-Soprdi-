using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoprodiApp.BusinessLayer;
using SoprodiApp.Entities;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
//using CreatePDF;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SoprodiApp.negocio;

namespace SoprodiApp
{
    public partial class PDF_COMISION : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            DataTable dt_total_resumen_com = (DataTable)Session["dt_total_resumen"];

            DataTable dt_cobranza_com = (DataTable)Session["dt_cobranza_com_resu"];

            string n_file = "/PDF_SGC.pdf";
            string pdfPath = Server.MapPath(@"~\PDFs\") + n_file;

            string tipo_comision = Request.QueryString["C"].ToString();
            string MUESTRA_O_NO = Request.QueryString["H"].ToString();
            string agregar_cobranza = Request.QueryString["T"].ToString().ToUpper();
            bool agregar_cobr = false; 

            DataTable usuarios_firman = ReporteRNegocio.usuarios_firman(Session["periodo"].ToString());

            if (agregar_cobranza == "TRUE")
            {
                agregar_cobr = true;
            }

            using (FileStream msReport = new FileStream(pdfPath, FileMode.Create))
            {
                //step 1
                using (Document pdfDoc = new Document(PageSize.LETTER))
                {
                    try
                    {

                        //pdfDoc.newPage();
                        //// Creamos el documento con el tamaño de página tradicional
                        //Document doc = new Document(PageSize.LETTER);
                        //doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                        //// Indicamos donde vamos a guardar el documento

                        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, msReport);
                        //pdfWriter.PageEvent = new Common.ITextEvents();

                        ITextEvents PageEventHandler = new ITextEvents();
                        pdfWriter.PageEvent = PageEventHandler;

                        //open the stream 
                        pdfDoc.Open();

                        // Creamos la imagen y le ajustamos el tamaño
                        iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(Server.MapPath("~/img/Sopro2.jpg"));
                        //imagen.BorderWidth = 0;
                        imagen.WidthPercentage = 20;
                        imagen.Alignment = Element.ALIGN_RIGHT;
                        float percentage = 0.0f;
                        percentage = 100 / imagen.Width;
                        imagen.ScalePercent(percentage * 100);


                        ///imagen check para firmas

                        // Creamos la imagen y le ajustamos el tamaño
                        iTextSharp.text.Image imagen2_check = iTextSharp.text.Image.GetInstance(Server.MapPath("~/img/accept.png"));
                        imagen.BorderWidth = 0;
                        imagen2_check.WidthPercentage = 60;
                        imagen2_check.Alignment = Element.ALIGN_RIGHT;
                        percentage = 10 / imagen2_check.Width;
                        imagen2_check.ScalePercent(percentage * 10);

                        //iTextSharp.text.Image imagen2_uncheck = iTextSharp.text.Image.GetInstance(Server.MapPath("~/img/delete.png"));
                        //imagen.BorderWidth = 0;
                        //imagen2_uncheck.WidthPercentage = 60;
                        //imagen2_uncheck.Alignment = Element.ALIGN_RIGHT;
                        //percentage = 10 / imagen2_uncheck.Width;
                        //imagen2_uncheck.ScalePercent(percentage * 10);

                        iTextSharp.text.Image imagen2_uncheck = iTextSharp.text.Image.GetInstance(Server.MapPath("~/img/delete.png"));
                        imagen2_uncheck.BorderWidth = 0;
                        imagen2_uncheck.Alignment = Element.ALIGN_CENTER;
                        //float percentage = 0.0f;
                        percentage = 100 / imagen.Width;
                        imagen2_uncheck.ScalePercent(percentage * 100);



                        //// Creamos la imagen y le ajustamos el tamaño
                        //iTextSharp.text.Image imagen2 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/img/Sopro.png"));
                        ////imagen.BorderWidth = 0;
                        //imagen.WidthPercentage = 50;
                        //imagen.Alignment = Element.ALIGN_LEFT;
                        //float percentage2 = 0.0f;
                        //percentage2 = 100 / imagen.Width;
                        //imagen.ScalePercent(percentage2 * 100);

                        //// Creamos la imagen y le ajustamos el tamaño
                        //iTextSharp.text.Image imagen3 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/img/Sopro.png"));
                        ////imagen.BorderWidth = 0;
                        //imagen.WidthPercentage = 50;
                        //imagen.Alignment = Element.ALIGN_LEFT;
                        //float percentage3 = 0.0f;
                        //percentage3 = 100 / imagen.Width;
                        //imagen.ScalePercent(percentage3 * 100);


                        PdfPTable tabla_imagen = new PdfPTable(1);
                        tabla_imagen.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tabla_imagen.WidthPercentage = 50;
                        PdfPCell imagen_td = new PdfPCell(imagen);
                        imagen_td.BorderWidth = 0;
                        tabla_imagen.AddCell(imagen_td);

                        //PdfPCell imagen2_td = new PdfPCell(imagen2);
                        //imagen2_td.BorderWidth = 0;
                        //tabla_imagen.AddCell(imagen2_td);

                        //PdfPCell imagen3_td = new PdfPCell(imagen3);
                        //imagen3_td.BorderWidth = 0;
                        //tabla_imagen.AddCell(imagen3_td);

                        PdfPCell vacio = new PdfPCell(new Phrase("  ", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        vacio.BorderWidth = 0;

                        tabla_imagen.AddCell(vacio);
                        tabla_imagen.AddCell(vacio);
                        tabla_imagen.AddCell(vacio);

                        PdfPTable tabla_titulo = new PdfPTable(1);
                        string titulo = "COMISIONES " + Session["periodo"].ToString();
                        PdfPCell TD_TITULO = new PdfPCell(new Phrase(titulo, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)));
                        TD_TITULO.BorderWidth = 0;
                        TD_TITULO.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabla_titulo.AddCell(TD_TITULO);

                        PdfPTable tabla_fecha_hora = new PdfPTable(1);
                        string fecha_hora = DateTime.Now.ToString();
                        PdfPCell td_fecha_hora = new PdfPCell(new Phrase(fecha_hora, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9)));
                        td_fecha_hora.BorderWidth = 0;
                        td_fecha_hora.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabla_fecha_hora.AddCell(td_fecha_hora);

                        PdfPTable tabla_Vacia = new PdfPTable(1);
                        PdfPCell td_vacia = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        td_vacia.BorderWidth = 0;
                        tabla_Vacia.AddCell(td_vacia);

                        pdfDoc.Add(imagen);
                        pdfDoc.Add(tabla_titulo);
                        pdfDoc.Add(tabla_fecha_hora);
                        pdfDoc.Add(tabla_Vacia);

                        ////Escribimos el encabezamiento en el documento
                        PdfPTable tabla = new PdfPTable(3);
                        tabla.WidthPercentage = 100;

                        PdfPCell th1 = new PdfPCell(new Phrase("NOMBRE", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        th1.HorizontalAlignment = Element.ALIGN_CENTER;
                        //th1.BorderWidth = 0;
                        tabla.AddCell(th1).BackgroundColor = new BaseColor(229, 229, 229, 229); ;

                        PdfPCell th2 = new PdfPCell(new Phrase("NOMBRE REGLA", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        th2.HorizontalAlignment = Element.ALIGN_CENTER;
                        //th2.BorderWidth = 0;
                        tabla.AddCell(th2).BackgroundColor = new BaseColor(229, 229, 229, 229); ;

                        PdfPCell th3 = new PdfPCell(new Phrase("TOTAL", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        th3.HorizontalAlignment = Element.ALIGN_CENTER;
                        //th3.BorderWidth = 0;
                        tabla.AddCell(th3).BackgroundColor = new BaseColor(229, 229, 229, 229); ;

                        //pdfDoc.Add(tabla);

                        string nombre_vendedor = "";
                        int cont = 0;
                        //string tipo_comision = Session["tipo_comision"].ToString();


                        DataTable dt_tolta_resume_com_aux = dt_total_resumen_com.Clone();

                        ///quitar los abarrotes
                        foreach (DataRow r2 in dt_total_resumen_com.Rows)
                        {
                            if (tipo_comision == "2")
                            {
                                if (r2[4].ToString() == "True")
                                {

                                    dt_tolta_resume_com_aux.ImportRow(r2);

                                    string aca = "";
                                    //dt_total_resumen_com.Rows.Remove(r2);
                                    //r2.Delete();

                                }
                            }
                            else if (tipo_comision == "1")
                            {
                                if (r2[4].ToString() != "True")
                                {
                                    dt_tolta_resume_com_aux.ImportRow(r2);

                                    string aca2 = "";
                                    //dt_total_resumen_com.Rows.Remove(r2);
                                    //r2.Delete();

                                }
                            }
                            else
                            {
                                dt_tolta_resume_com_aux.ImportRow(r2);

                            }
                        }

                        foreach (DataRow r in dt_tolta_resume_com_aux.Rows)
                        {

                            cont++;


                            if (nombre_vendedor != r[1].ToString())
                            {
                                if (cont > 1)
                                {
                                    tabla.AddCell(vacio);
                                    tabla.AddCell(vacio);
                                    tabla.AddCell(vacio);


                                    tabla.AddCell(vacio);
                                    tabla.AddCell(vacio);
                                    tabla.AddCell(vacio);
                                }

                                if (r[2].ToString().Contains("ABARROTES"))
                                {
                                    PdfPCell td1 = new PdfPCell(new Phrase(r[1].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                    td1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //td1.BorderWidth = 0;
                                    tabla.AddCell(td1).BackgroundColor = new BaseColor(240, 128, 128);
                                }
                                else
                                {


                                    PdfPCell td1 = new PdfPCell(new Phrase(r[1].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                    td1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //td1.BorderWidth = 0;
                                    tabla.AddCell(td1).BackgroundColor = new BaseColor(135, 206, 235);



                                }

                                nombre_vendedor = r[1].ToString();
                            }

                            else
                            {

                                PdfPCell td1 = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                td1.HorizontalAlignment = Element.ALIGN_LEFT;
                                //td1.BorderWidth = 0;
                                tabla.AddCell(td1);
                                nombre_vendedor = r[1].ToString();
                            }


                            PdfPCell td2 = new PdfPCell(new Phrase(r[2].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                            td2.HorizontalAlignment = Element.ALIGN_LEFT;
                            //td2.BorderWidth = 0;
                            tabla.AddCell(td2);

                            PdfPCell td3 = new PdfPCell(new Phrase(r[3].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                            td3.HorizontalAlignment = Element.ALIGN_LEFT;
                            //td3.BorderWidth = 0;
                            tabla.AddCell(td3);
                            //PdfPTable table_ventas = new PdfPTable(11);
                            //table_ventas.WidthPercentage = 100;

                            //table_ventas.AddCell(dia).BackgroundColor = new BaseColor(229, 229, 229, 229);                 


                        }

                        tabla.AddCell(vacio);
                        tabla.AddCell(vacio);


                        ////////------------------------------------------------------------------------------COMISION COBRANZA
                        PdfPTable tabla_COBRANZA = new PdfPTable(4);
                        if (agregar_cobr)
                        {
                            tabla_COBRANZA.WidthPercentage = 100;

                            tabla_COBRANZA.AddCell(vacio);
                            tabla_COBRANZA.AddCell(vacio);
                            tabla_COBRANZA.AddCell(vacio);
                            tabla_COBRANZA.AddCell(vacio);


                            tabla_COBRANZA.AddCell(vacio);
                            tabla_COBRANZA.AddCell(vacio);
                            tabla_COBRANZA.AddCell(vacio);
                            tabla_COBRANZA.AddCell(vacio);


                            tabla_COBRANZA.AddCell(vacio);
                            tabla_COBRANZA.AddCell(vacio);
                            tabla_COBRANZA.AddCell(vacio);
                            tabla_COBRANZA.AddCell(vacio);

                            cont = 0;
                            bool final = true;
                            foreach (DataRow r3 in dt_cobranza_com.Rows)
                            {
                                if (final)
                                {


                                    tabla_COBRANZA.AddCell(vacio);
                                    tabla_COBRANZA.AddCell(vacio);
                                    tabla_COBRANZA.AddCell(vacio);
                                    tabla_COBRANZA.AddCell(vacio);


                                    PdfPCell td1 = new PdfPCell(new Phrase(r3[0].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                    td1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //td1.BorderWidth = 0;
                                    tabla_COBRANZA.AddCell(td1).BackgroundColor = new BaseColor(30, 176, 40);

                                    PdfPCell td2 = new PdfPCell(new Phrase(r3[1].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                    td1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //td1.BorderWidth = 0;
                                    tabla_COBRANZA.AddCell(td2).BackgroundColor = new BaseColor(30, 176, 40);

                                    PdfPCell td3 = new PdfPCell(new Phrase(r3[2].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                    td1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //td1.BorderWidth = 0;
                                    tabla_COBRANZA.AddCell(td3).BackgroundColor = new BaseColor(30, 176, 40);

                                    PdfPCell td4 = new PdfPCell(new Phrase(r3[3].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                    td1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //td1.BorderWidth = 0;
                                    tabla_COBRANZA.AddCell(td4).BackgroundColor = new BaseColor(30, 176, 40);

                                    final = false;
                                }
                                else
                                {
                                    PdfPCell td1 = new PdfPCell(new Phrase(r3[0].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                    td1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //td2.BorderWidth = 0;
                                    tabla_COBRANZA.AddCell(td1);

                                    PdfPCell td2 = new PdfPCell(new Phrase(r3[1].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                    td2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //td2.BorderWidth = 0;
                                    tabla_COBRANZA.AddCell(td2);

                                    PdfPCell td3 = new PdfPCell(new Phrase(r3[2].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                    td3.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //td2.BorderWidth = 0;
                                    tabla_COBRANZA.AddCell(td3);

                                    PdfPCell td4 = new PdfPCell(new Phrase(r3[3].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                    td4.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //td2.BorderWidth = 0;
                                    tabla_COBRANZA.AddCell(td4);

                                }
                                if (r3[0].ToString().Contains("TOTAL"))
                                {
                                    final = true;
                                }




                            }
                        }

                        float[] medidasCeldas = { 0.30f, 0.50f, 0.15f };
                        tabla.SetWidths(medidasCeldas);

                        float[] medidasCeldas_4 = { 0.38f, 0.28f, 0.15f, 0.16f };

                        float[] medidasCeldas_5 = { 0.20f, 0.20f, 0.20f, 0.20f, 0.20f };


                        if (agregar_cobr)
                        {
                            tabla_COBRANZA.SetWidths(medidasCeldas_4);
                        }
                        pdfDoc.Add(tabla);
                        if (agregar_cobr)
                        {
                            pdfDoc.Add(tabla_COBRANZA);
                        }


                        ////////------------------------------------------------------------------------------FIRMAS
                        //PdfPTable tabla_firmas = new PdfPTable(usuarios_firman.Rows.Count);


                        PdfPTable tabla_firmas = new PdfPTable(3);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);
                        tabla_firmas.AddCell(vacio);


                        tabla_firmas.WidthPercentage = 100;

                        PdfPCell th1_F = new PdfPCell(new Phrase("GRUPO", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        th1_F.HorizontalAlignment = Element.ALIGN_CENTER;
                        //th1.BorderWidth = 0;
                        tabla_firmas.AddCell(th1_F).BackgroundColor = new BaseColor(229, 229, 229, 229); ;

                        PdfPCell th2_F = new PdfPCell(new Phrase("FIRMA", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        th2_F.HorizontalAlignment = Element.ALIGN_CENTER;
                        //th2.BorderWidth = 0;
                        tabla_firmas.AddCell(th2_F).BackgroundColor = new BaseColor(229, 229, 229, 229); ;

                        PdfPCell th3_F = new PdfPCell(new Phrase("NOMBRE", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        th3_F.HorizontalAlignment = Element.ALIGN_CENTER;
                        //th3.BorderWidth = 0;
                        tabla_firmas.AddCell(th3_F).BackgroundColor = new BaseColor(229, 229, 229, 229); ;

                        foreach (DataRow r1 in usuarios_firman.Rows)
                        {

                            PdfPCell td1 = new PdfPCell(new Phrase(r1[0].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                            td1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //td2.BorderWidth = 0;
                            tabla_firmas.AddCell(td1);

                        
                            if (r1[1].ToString().Trim() == "True")
                            {
                                PdfPCell td2 = new PdfPCell(new Phrase("OK", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                td2.HorizontalAlignment = Element.ALIGN_LEFT;
                                //td2.BorderWidth = 0;
                                tabla_firmas.AddCell(td2).BackgroundColor = new BaseColor(59, 134, 50); 
                            }
                            else
                            {
                                PdfPCell td2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                                td2.HorizontalAlignment = Element.ALIGN_LEFT;
                                //td2.BorderWidth = 0;
                                tabla_firmas.AddCell(td2).BackgroundColor = new BaseColor(239, 36, 36);
                            }

                            PdfPCell td3 = new PdfPCell(new Phrase(r1[3].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                            td3.HorizontalAlignment = Element.ALIGN_LEFT;
                            //td2.BorderWidth = 0;
                            tabla_firmas.AddCell(td3);
                        }


                        //agregamos tabla de las firmas
                        tabla_firmas.SetWidths(medidasCeldas);
                        pdfDoc.Add(tabla_firmas);


                        pdfDoc.NewPage();

                        pdfDoc.Close();
                        //writer.Close();
                        //string pdfPath = Server.MapPath("~/PDF_Ventas/" + n_file);
                        WebClient client = new WebClient();
                        Byte[] buffer = client.DownloadData(pdfPath);

                        //document.Add(new Paragraph("Hello World!"));

                        if (MUESTRA_O_NO == "0")
                        {

                        }
                        else
                        {
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-length", buffer.Length.ToString());
                            Response.BinaryWrite(buffer);
                            Response.Flush();
                            Response.End();
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private void TheDownload(string path, string file_name)
        {
            try
            {
                path = path.Replace("\\\\", "\\");
                System.IO.FileInfo toDownload = new System.IO.FileInfo(path);
                if (toDownload.Exists)
                {
                    Response.Clear();
                    //if (filename_.Contains(" "))
                    //{

                    //    filename_ = filename_.Replace(" ", "_");
                    //}

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file_name);
                    Response.ContentType = "application/octect-stream";
                    Response.AddHeader("content-transfer-encoding", "binary");
                    Response.WriteFile(path);
                    Response.End();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static String formatoMiles(string monto)
        {
            double d;
            double.TryParse(monto, out d);
            string aux = "";
            if (d == 0) { aux = "0"; } else { aux = d.ToString("N0"); }
            return aux;
        }

        public string formatearRut(string rut)
        {
            int cont = 0;
            string format;
            if (rut.Length == 0)
            {
                return "";
            }
            else
            {
                rut = rut.Replace(".", "");
                rut = rut.Replace("-", "");
                format = "-" + rut.Substring(rut.Length - 1);
                for (int i = rut.Length - 2; i >= 0; i--)
                {
                    format = rut.Substring(i, 1) + format;
                    cont++;
                    if (cont == 3 && i != 0)
                    {
                        format = "." + format;
                        cont = 0;
                    }
                }
                return format;
            }
        }

        public class ITextEvents : PdfPageEventHelper
        {
            // Éste es el objeto contentbyte del writer
            PdfContentByte cb;

            // Pondremos el número final de páginas en una plantilla
            PdfTemplate headerTemplate, footerTemplate;

            // Este es el BaseFont que vamos a utilizar para el encabezado / pie de página
            BaseFont bf = null;

            DateTime PrintTime = DateTime.Now;

            #region Fields
            private string _header;
            #endregion

            #region Properties
            public string Header
            {
                get { return _header; }
                set { _header = value; }
            }
            #endregion

            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                try
                {
                    PrintTime = DateTime.Now;
                    bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
                    cb = writer.DirectContent;
                    headerTemplate = cb.CreateTemplate(100, 100);
                    //footerTemplate = cb.CreateTemplate(50, 50);
                }
                catch (DocumentException de)
                {
                    //handle exception here
                }
                catch (System.IO.IOException ioe)
                {
                    //handle exception here
                }
            }

            public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
            {
                base.OnEndPage(writer, document);

                //iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

                //iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

                String text = "Página " + writer.PageNumber + " de ";


                //añadir paginacion al encabezado
                {
                    cb.BeginText();
                    cb.SetFontAndSize(bf, 10);
                    cb.SetTextMatrix(document.PageSize.GetRight(100), document.PageSize.GetTop(20));
                    cb.ShowText(text);
                    cb.EndText();
                    float len = bf.GetWidthPoint(text, 10);
                    //Adds "12" in Page 1 of 12
                    cb.AddTemplate(headerTemplate, document.PageSize.GetRight(100) + len, document.PageSize.GetTop(20));
                }
                //añadir paginacion al pie de pagina
                //{
                //    cb.BeginText();
                //    cb.SetFontAndSize(bf, 12);
                //    cb.SetTextMatrix(document.PageSize.GetRight(180), document.PageSize.GetBottom(30));
                //    cb.ShowText(text);
                //    cb.EndText();
                //    float len = bf.GetWidthPoint(text, 12);
                //    cb.AddTemplate(footerTemplate, document.PageSize.GetRight(180) + len, document.PageSize.GetBottom(30));
                //}

                //Move the pointer and draw line to separate header section from rest of page
                //cb.MoveTo(40, document.PageSize.Height - 100);
                //cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 100);
                //cb.Stroke();

                //Move the pointer and draw line to separate footer section from rest of page
                //cb.MoveTo(40, document.PageSize.GetBottom(50));
                //cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
                //cb.Stroke();
            }

            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);

                headerTemplate.BeginText();
                headerTemplate.SetFontAndSize(bf, 10);
                headerTemplate.SetTextMatrix(0, 0);
                headerTemplate.ShowText((writer.PageNumber - 1).ToString());
                headerTemplate.EndText();

                //footerTemplate.BeginText();
                //footerTemplate.SetFontAndSize(bf, 10);
                //footerTemplate.SetTextMatrix(0, 0);
                //footerTemplate.ShowText((writer.PageNumber - 1).ToString());
                //footerTemplate.EndText();


            }
        }
    }


}