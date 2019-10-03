using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using CRM.BusinessLayer;
//using CRM.Entities;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
//using CreatePDF;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ThinxsysFramework;

namespace SoprodiApp
{
    public partial class PDF_SP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string num_sp = Request.QueryString["sp"].ToString();
            string n_file = "/"+ num_sp.Trim()+".pdf";
            string pdfPath = Server.MapPath(@"~\PDFs\") + n_file;        
            using (FileStream msReport = new FileStream(pdfPath, FileMode.Create))
            {
                using (Document pdfDoc = new Document(PageSize.LETTER))
                {
                    try
                    {
                        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, msReport);  
                        ITextEvents PageEventHandler = new ITextEvents();
                        pdfWriter.PageEvent = PageEventHandler;
                        pdfDoc.Open();            
                        PdfPTable tabla1 = new PdfPTable(1);
                        tabla1.WidthPercentage = 100;
                        PdfPCell celda_vaciaSinborde = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
                        celda_vaciaSinborde.BorderWidth = 0;
                        Sp_datos sp_datos_class = Base.sp_encabezado(num_sp);
                        PdfPCell celda1 = new PdfPCell(new Phrase("Solicitud de pedido N° "+num_sp+" ("+ sp_datos_class.estado_sp + ")", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        celda1.HorizontalAlignment = Element.ALIGN_LEFT;
                        celda1.BorderWidth = 0;
                        celda1.BorderWidthBottom = 0.5f;
                        celda1.BorderWidthTop = 0.5f;
                        tabla1.AddCell(celda1);
                        tabla1.AddCell(celda_vaciaSinborde);
                        pdfDoc.Add(tabla1);
                        //ENCABEZADO
                        PdfPTable tabla2 = Base.encabezado_sp_pdf(sp_datos_class);
                        pdfDoc.Add(tabla2);
                        //DETALLE PRODUCTOS
                        PdfPTable tabla3 = Base.detalle_sp_pdf(num_sp);
                        pdfDoc.Add(tabla3);
                        //COMENTARIOS - TIPOCAMBIO
                        PdfPTable tabla4 = new PdfPTable(1);
                        tabla4.WidthPercentage = 100;
                        PdfPCell celda79 = new PdfPCell(new Phrase(sp_datos_class.nota_libre, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
                        celda79.HorizontalAlignment = Element.ALIGN_LEFT;
                        celda79.BorderWidth = 0;
                        tabla4.AddCell(celda79);
                        pdfDoc.Add(tabla4);
                        PdfPTable tabla5 = new PdfPTable(1);
                        tabla5.WidthPercentage = 100;
                        PdfPCell celda80 = new PdfPCell(new Phrase("Tipo de Cambio: " + sp_datos_class.valor_t_cambio, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
                        celda80.HorizontalAlignment = Element.ALIGN_LEFT;
                        celda80.BorderWidth = 0;
                        tabla5.AddCell(celda80);
                        pdfDoc.Add(tabla5);
                        pdfDoc.NewPage();
                        pdfDoc.Close();
                        WebClient client = new WebClient();
                        Byte[] buffer = client.DownloadData(pdfPath);
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-length", buffer.Length.ToString());
                        Response.BinaryWrite(buffer);
                        Response.Flush();
                        Response.End();
                    }
                    catch (Exception ex)
                    {

                    }
                }
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

        public string funcionX(string monto)
        {
            if (monto != "")
            {
                double t_boletas = double.Parse(monto);
                double x = t_boletas / 1.19;
                decimal neto = Math.Round(decimal.Parse(x.ToString()));
                decimal iva = decimal.Parse(t_boletas.ToString()) - neto;
                return neto + "," + iva;
            }
            else
            {
                return "0,0";
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