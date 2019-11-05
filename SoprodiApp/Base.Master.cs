using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoprodiApp.negocio;
using SoprodiApp.entidad;
using System.Web.Services;
using System.Configuration;
using System.Web.Configuration;
using System.Data;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using static SoprodiApp.PDF_SP;
using System.Net;

namespace SoprodiApp
{

    public partial class Base : System.Web.UI.MasterPage
    {
        private static Page page;

        protected void Page_Load(object sender, EventArgs e)
        {
            page = this.Page;
            if (!IsPostBack)
            {
                usuario.Text = HttpContext.Current.User.Identity.Name.ToString();
                //if (Request.Cookies["user"] != null)
                //{
                //    Response.Cookies["user"].Expires = DateTime.Now.AddDays(-1);
                //}

                //if (Request.Cookies["UserSettings"] != null)
                //{
                //    HttpCookie myCookie = new HttpCookie("UserSettings");
                //    myCookie.Expires = DateTime.Now.AddDays(-1d);
                //    Response.Cookies.Add(myCookie);
                //}
                //Menu();

            }
            //Page.RegisterRedirectOnSessionEndScript();
            Response.Cache.SetAllowResponseInBrowserHistory(false);

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //Response.Cache.SetExpires(now().AddSeconds(-1));

            Response.Cache.SetNoStore();

            Response.AddHeader("Pragma", "no-cache");

            //Response.Write("" & DateTime.Now.ToLongTimeString());

        }
        private void Menu()
        {

            ////Page.RegisterRedirectOnSessionEndScript();
            //string es_adm = usuarioBO.obtener_adm(usuario.Text);
            //if (es_adm == "1")
            //{

            //}
            //string trae_ch = usuarioBO.obtener_check(usuario.Text);
            //if (trae_ch == "True")
            //{
            //    //diario.Visible = true;

            //}
            //else if (trae_ch == "False")
            //{
            //    //diario.Visible = false;
            //    //diario.Visible = true;
            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>SIDEBAR_NO_DIARIO();</script>", false);

            //}
        }


        [WebMethod(EnableSession = true)]
        public static int end_session()
        {
            page.Session.Abandon();
            return 0;
        }

        protected void guarda_Click(object sender, EventArgs e)
        {
            string es_su_pass = ReporteRNegocio.es_su_pass(usuario.Text);
            if (es_su_pass != pass_antigua.Value)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>error_pass();</script>", false);
            }
            else
            {
                usuarioEntity us = new usuarioEntity();
                us.cod_usuario = usuario.Text;
                us.clave = pass_nueva.Value;
                us.activado = "true";

                if (usuarioBO.actualizar(us, usuario.Text) == "OK")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>guardada();</script>", false);
                }
            }
        }

        private string obtener_periodo(string text)
        {
            List<string> datos_fecha = text.Split('/').ToList();
            if (datos_fecha[1].ToString().Length == 1)
            {
                datos_fecha[1] = "0" + datos_fecha[1];
            }
            string año_mes = datos_fecha[2] + datos_fecha[1];
            return año_mes;
        }

        internal static PdfPTable detalle_sp_pdf(string num_sp)
        {
            PdfPCell celda_bordeTop = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda_bordeTop.BorderWidth = 0;
            celda_bordeTop.BorderWidthTop = 0.5f;

            PdfPCell celda_vaciaSinborde = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda_vaciaSinborde.BorderWidth = 0;



            PdfPTable tabla3 = new PdfPTable(8);
            tabla3.WidthPercentage = 100;

            PdfPCell celda55 = new PdfPCell(new Phrase("CODIGO", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda55.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla3.AddCell(celda55).BackgroundColor = new BaseColor(181, 193, 255);

            PdfPCell celda56 = new PdfPCell(new Phrase("PRODUCTO", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda56.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla3.AddCell(celda56).BackgroundColor = new BaseColor(181, 193, 255);

            PdfPCell celda57 = new PdfPCell(new Phrase("CANTIDAD", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda57.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla3.AddCell(celda57).BackgroundColor = new BaseColor(181, 193, 255);

            PdfPCell celda58 = new PdfPCell(new Phrase("UN. MEDIDA", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda58.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla3.AddCell(celda58).BackgroundColor = new BaseColor(181, 193, 255);

            PdfPCell celda59 = new PdfPCell(new Phrase("PRECIO UNITARIO", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda59.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla3.AddCell(celda59).BackgroundColor = new BaseColor(181, 193, 255);

            PdfPCell celda60 = new PdfPCell(new Phrase("% DESCUENTO", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda60.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla3.AddCell(celda60).BackgroundColor = new BaseColor(181, 193, 255);

            PdfPCell celda61 = new PdfPCell(new Phrase("PRECIO UNITARIO FINAL", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda61.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla3.AddCell(celda61).BackgroundColor = new BaseColor(181, 193, 255);

            PdfPCell celda62 = new PdfPCell(new Phrase("TOTAL", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda62.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla3.AddCell(celda62).BackgroundColor = new BaseColor(181, 193, 255);


            Sp_datos_detalle sp_detalle_class = new Sp_datos_detalle();
            DataTable dat_detalle_Sp = Base.sp_detalle(num_sp);
            double sub_total = 0;

            foreach (DataRow r_det in dat_detalle_Sp.Rows)
            {
                sp_detalle_class.codigo = r_det["codproducto"].ToString();
                sp_detalle_class.producto = r_det["descproducto"].ToString();
                sp_detalle_class.cantidad = r_det["cantidad"].ToString();
                sp_detalle_class.un_medida = r_det["descUnVenta"].ToString();
                sp_detalle_class.precio_unit = r_det["PrecioUnitario"].ToString();
                sp_detalle_class.descuento = r_det["Descuento"].ToString();
                sp_detalle_class.precio_unit_final = r_det["PrecioUnitarioFinal"].ToString();

                string total = "";

                try
                {

                    double temp_CANT = Convert.ToDouble(sp_detalle_class.cantidad);
                    double temp_precio_unit_final = Convert.ToDouble(sp_detalle_class.precio_unit_final);

                    double temp_total = temp_CANT * temp_precio_unit_final;

                    total = Base.monto_format2(Math.Round(temp_total, MidpointRounding.AwayFromZero));

                    sub_total += temp_total;
                }
                catch { }

                PdfPCell celda63 = new PdfPCell(new Phrase(sp_detalle_class.codigo, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
                celda63.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla3.AddCell(celda63);

                PdfPCell celda64 = new PdfPCell(new Phrase(sp_detalle_class.producto, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
                celda64.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla3.AddCell(celda64);

                PdfPCell celda65 = new PdfPCell(new Phrase(sp_detalle_class.cantidad, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
                celda65.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla3.AddCell(celda65);

                PdfPCell celda66 = new PdfPCell(new Phrase(sp_detalle_class.un_medida, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
                celda66.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla3.AddCell(celda66);

                PdfPCell celda67 = new PdfPCell(new Phrase(sp_detalle_class.precio_unit, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
                celda67.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla3.AddCell(celda67);

                PdfPCell celda68 = new PdfPCell(new Phrase(sp_detalle_class.descuento, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
                celda68.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla3.AddCell(celda68);

                PdfPCell celda69 = new PdfPCell(new Phrase(sp_detalle_class.precio_unit_final, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
                celda69.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla3.AddCell(celda69);

                PdfPCell celda70 = new PdfPCell(new Phrase(total, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
                celda70.HorizontalAlignment = Element.ALIGN_CENTER;
                tabla3.AddCell(celda70);

            }


            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            PdfPCell celda71 = new PdfPCell(new Phrase("Subtotal", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda71.HorizontalAlignment = Element.ALIGN_RIGHT;
            tabla3.AddCell(celda71);

            string sub_total_aux = Base.monto_format2(Math.Round(sub_total, MidpointRounding.AwayFromZero));

            int subtotal_int = Convert.ToInt32(Math.Round(sub_total, MidpointRounding.AwayFromZero));
            int iva = Convert.ToInt32(Math.Round((sub_total * 0.19), MidpointRounding.AwayFromZero));
            int total_2 = subtotal_int + iva;

            string sub_total_formato = Base.monto_format2(subtotal_int);
            string iva_formato = Base.monto_format2(iva);
            string total_formato = Base.monto_format2(total_2);

            PdfPCell celda72 = new PdfPCell(new Phrase(sub_total_formato, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda72.HorizontalAlignment = Element.ALIGN_RIGHT;
            tabla3.AddCell(celda72);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            PdfPCell celda73 = new PdfPCell(new Phrase("% Descuento", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda73.HorizontalAlignment = Element.ALIGN_RIGHT;
            tabla3.AddCell(celda73);

            PdfPCell celda74 = new PdfPCell(new Phrase("0.000", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda74.HorizontalAlignment = Element.ALIGN_RIGHT;
            tabla3.AddCell(celda74);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            PdfPCell celda75 = new PdfPCell(new Phrase("I.V.A", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda75.HorizontalAlignment = Element.ALIGN_RIGHT;
            tabla3.AddCell(celda75);

            string iva_aux = Base.monto_format2(Math.Round((sub_total * 0.19), MidpointRounding.AwayFromZero));

            PdfPCell celda76 = new PdfPCell(new Phrase(iva_formato, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda76.HorizontalAlignment = Element.ALIGN_RIGHT;
            tabla3.AddCell(celda76);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            PdfPCell celda77 = new PdfPCell(new Phrase("TOTAL", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda77.HorizontalAlignment = Element.ALIGN_RIGHT;
            tabla3.AddCell(celda77);

            string total_aux = Base.monto_format2(Math.Round(((sub_total * 0.19) + sub_total), MidpointRounding.AwayFromZero));

            PdfPCell celda78 = new PdfPCell(new Phrase(total_formato, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda78.HorizontalAlignment = Element.ALIGN_RIGHT;
            tabla3.AddCell(celda78);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_vaciaSinborde);

            tabla3.AddCell(celda_bordeTop);

            tabla3.AddCell(celda_bordeTop);

            tabla3.AddCell(celda_bordeTop);

            tabla3.AddCell(celda_bordeTop);

            tabla3.AddCell(celda_bordeTop);

            tabla3.AddCell(celda_bordeTop);

            tabla3.AddCell(celda_bordeTop);

            tabla3.AddCell(celda_bordeTop);

            return tabla3;

        }

        internal static PdfPTable encabezado_sp_pdf(Sp_datos sp_datos_class)
        {
            PdfPTable tabla2 = new PdfPTable(4);
            tabla2.WidthPercentage = 100;

            PdfPCell celda_vaciaSinborde = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda_vaciaSinborde.BorderWidth = 0;

            //TITULO---------------------------------->
            PdfPCell celda2 = new PdfPCell(new Phrase("Datos Proveedor", FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.RED)));
            celda2.HorizontalAlignment = Element.ALIGN_LEFT;
            celda2.BorderWidth = 0;
            tabla2.AddCell(celda2);

            tabla2.AddCell(celda_vaciaSinborde);

            tabla2.AddCell(celda_vaciaSinborde);

            tabla2.AddCell(celda_vaciaSinborde);
            //TITULO---------------------------------->
            PdfPCell celda3 = new PdfPCell(new Phrase("Dirección", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda3.HorizontalAlignment = Element.ALIGN_LEFT;
            celda3.BorderWidth = 0;
            tabla2.AddCell(celda3);

            PdfPCell celda4 = new PdfPCell(new Phrase(sp_datos_class.proveedor_dir, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda4.HorizontalAlignment = Element.ALIGN_LEFT;
            celda4.BorderWidth = 0;
            tabla2.AddCell(celda4);
            //TITULO---------------------------------->
            PdfPCell celda5 = new PdfPCell(new Phrase("Vendedor", FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.RED)));
            celda5.HorizontalAlignment = Element.ALIGN_LEFT;
            celda5.BorderWidth = 0;
            tabla2.AddCell(celda5);

            PdfPCell celda6 = new PdfPCell(new Phrase(sp_datos_class.vendedor, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda6.HorizontalAlignment = Element.ALIGN_LEFT;
            celda6.BorderWidth = 0;
            tabla2.AddCell(celda6);
            //TITULO---------------------------------->
            PdfPCell celda7 = new PdfPCell(new Phrase("Teléfono", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda7.HorizontalAlignment = Element.ALIGN_LEFT;
            celda7.BorderWidth = 0;
            tabla2.AddCell(celda7);

            PdfPCell celda8 = new PdfPCell(new Phrase(sp_datos_class.proveedor_fono, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda8.HorizontalAlignment = Element.ALIGN_LEFT;
            celda8.BorderWidth = 0;
            tabla2.AddCell(celda8);
            //TITULO---------------------------------->
            PdfPCell celda9 = new PdfPCell(new Phrase("Fecha Cierre", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda9.HorizontalAlignment = Element.ALIGN_LEFT;
            celda9.BorderWidth = 0;
            tabla2.AddCell(celda9);

            PdfPCell celda10 = new PdfPCell(new Phrase(sp_datos_class.fecha_emi, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda10.HorizontalAlignment = Element.ALIGN_LEFT;
            celda10.BorderWidth = 0;
            tabla2.AddCell(celda10);
            //TITULO---------------------------------->
            PdfPCell celda11 = new PdfPCell(new Phrase("Fax", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda11.HorizontalAlignment = Element.ALIGN_LEFT;
            celda11.BorderWidth = 0;
            tabla2.AddCell(celda11);

            PdfPCell celda12 = new PdfPCell(new Phrase(sp_datos_class.proveedor_fax, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda12.HorizontalAlignment = Element.ALIGN_LEFT;
            celda12.BorderWidth = 0;
            tabla2.AddCell(celda12);
            //TITULO---------------------------------->
            PdfPCell celda13 = new PdfPCell(new Phrase("Fecha Operación", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda13.HorizontalAlignment = Element.ALIGN_LEFT;
            celda13.BorderWidth = 0;
            tabla2.AddCell(celda13);

            PdfPCell celda14 = new PdfPCell(new Phrase(sp_datos_class.fecha_emi, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda14.HorizontalAlignment = Element.ALIGN_LEFT;
            celda14.BorderWidth = 0;
            tabla2.AddCell(celda14);
            //TITULO---------------------------------->
            PdfPCell celda15 = new PdfPCell(new Phrase("Email", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda15.HorizontalAlignment = Element.ALIGN_LEFT;
            celda15.BorderWidth = 0;
            tabla2.AddCell(celda15);

            PdfPCell celda16 = new PdfPCell(new Phrase(sp_datos_class.proveedor_email, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda16.HorizontalAlignment = Element.ALIGN_LEFT;
            celda16.BorderWidth = 0;
            tabla2.AddCell(celda16);

            tabla2.AddCell(celda_vaciaSinborde);

            tabla2.AddCell(celda_vaciaSinborde);
            //TITULO---------------------------------->
            PdfPCell celda17 = new PdfPCell(new Phrase("URL", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda17.HorizontalAlignment = Element.ALIGN_LEFT;
            celda17.BorderWidth = 0;
            tabla2.AddCell(celda17);

            PdfPCell celda18 = new PdfPCell(new Phrase(sp_datos_class.proveedor_URL, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda18.HorizontalAlignment = Element.ALIGN_LEFT;
            celda18.BorderWidth = 0;
            tabla2.AddCell(celda18);

            tabla2.AddCell(celda_vaciaSinborde);

            tabla2.AddCell(celda_vaciaSinborde);

            PdfPCell celda_bordeTop = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda_bordeTop.BorderWidth = 0;
            celda_bordeTop.BorderWidthTop = 0.5f;

            tabla2.AddCell(celda_bordeTop);

            tabla2.AddCell(celda_bordeTop);

            tabla2.AddCell(celda_bordeTop);

            tabla2.AddCell(celda_bordeTop);
            //TITULO---------------------------------->
            PdfPCell celda19 = new PdfPCell(new Phrase("Facturar a", FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.RED)));
            celda19.HorizontalAlignment = Element.ALIGN_LEFT;
            celda19.BorderWidth = 0;
            tabla2.AddCell(celda19);

            tabla2.AddCell(celda_vaciaSinborde);
            //TITULO---------------------------------->
            PdfPCell celda20 = new PdfPCell(new Phrase("Despachar a", FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.RED)));
            celda20.HorizontalAlignment = Element.ALIGN_LEFT;
            celda20.BorderWidth = 0;
            tabla2.AddCell(celda20);

            tabla2.AddCell(celda_vaciaSinborde);
            //TITULO---------------------------------->
            PdfPCell celda21 = new PdfPCell(new Phrase("Razón Social", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda21.HorizontalAlignment = Element.ALIGN_LEFT;
            celda21.BorderWidth = 0;
            tabla2.AddCell(celda21);

            PdfPCell celda22 = new PdfPCell(new Phrase(sp_datos_class.nombre_cliente, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda22.HorizontalAlignment = Element.ALIGN_LEFT;
            celda22.BorderWidth = 0;
            tabla2.AddCell(celda22);
            //TITULO---------------------------------->
            PdfPCell celda23 = new PdfPCell(new Phrase("Fecha Entrega", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda23.HorizontalAlignment = Element.ALIGN_LEFT;
            celda23.BorderWidth = 0;
            tabla2.AddCell(celda23);

            PdfPCell celda24 = new PdfPCell(new Phrase(sp_datos_class.fecha_desp, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda24.HorizontalAlignment = Element.ALIGN_LEFT;
            celda24.BorderWidth = 0;
            tabla2.AddCell(celda24);
            //TITULO---------------------------------->
            PdfPCell celda25 = new PdfPCell(new Phrase("Rut", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda25.HorizontalAlignment = Element.ALIGN_LEFT;
            celda25.BorderWidth = 0;
            tabla2.AddCell(celda25);

            PdfPCell celda26 = new PdfPCell(new Phrase(sp_datos_class.rut_cliente, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda26.HorizontalAlignment = Element.ALIGN_LEFT;
            celda26.BorderWidth = 0;
            tabla2.AddCell(celda26);
            //TITULO---------------------------------->
            PdfPCell celda27 = new PdfPCell(new Phrase("Dirección", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda27.HorizontalAlignment = Element.ALIGN_LEFT;
            celda27.BorderWidth = 0;
            tabla2.AddCell(celda27);

            PdfPCell celda28 = new PdfPCell(new Phrase(sp_datos_class.direccion_despacho, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda28.HorizontalAlignment = Element.ALIGN_LEFT;
            celda28.BorderWidth = 0;
            tabla2.AddCell(celda28);
            //TITULO---------------------------------->
            PdfPCell celda29 = new PdfPCell(new Phrase("Giro", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda29.HorizontalAlignment = Element.ALIGN_LEFT;
            celda29.BorderWidth = 0;
            tabla2.AddCell(celda29);

            PdfPCell celda30 = new PdfPCell(new Phrase(sp_datos_class.giro, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda30.HorizontalAlignment = Element.ALIGN_LEFT;
            celda30.BorderWidth = 0;
            tabla2.AddCell(celda30);
            //TITULO---------------------------------->
            PdfPCell celda31 = new PdfPCell(new Phrase("Ciudad", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda31.HorizontalAlignment = Element.ALIGN_LEFT;
            celda31.BorderWidth = 0;
            tabla2.AddCell(celda31);

            PdfPCell celda32 = new PdfPCell(new Phrase(sp_datos_class.comuna_despacho, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda32.HorizontalAlignment = Element.ALIGN_LEFT;
            celda32.BorderWidth = 0;
            tabla2.AddCell(celda32);

            //TITULO---------------------------------->
            PdfPCell celda32_1 = new PdfPCell(new Phrase("Dirección", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda32_1.HorizontalAlignment = Element.ALIGN_LEFT;
            celda32_1.BorderWidth = 0;
            tabla2.AddCell(celda32_1);

            PdfPCell celda32_2 = new PdfPCell(new Phrase(sp_datos_class.direccion, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda32_2.HorizontalAlignment = Element.ALIGN_LEFT;
            celda32_2.BorderWidth = 0;
            tabla2.AddCell(celda32_2);
            //TITULO---------------------------------->
            PdfPCell celda32_3 = new PdfPCell(new Phrase("Teléfono", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda32_3.HorizontalAlignment = Element.ALIGN_LEFT;
            celda32_3.BorderWidth = 0;
            tabla2.AddCell(celda32_3);

            PdfPCell celda32_4 = new PdfPCell(new Phrase(sp_datos_class.fono, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda32_4.HorizontalAlignment = Element.ALIGN_LEFT;
            celda32_4.BorderWidth = 0;
            tabla2.AddCell(celda32_4);

            PdfPCell celda33 = new PdfPCell(new Phrase("Ciudad", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda33.HorizontalAlignment = Element.ALIGN_LEFT;
            celda33.BorderWidth = 0;
            tabla2.AddCell(celda33);

            PdfPCell celda34 = new PdfPCell(new Phrase(sp_datos_class.ciudad, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda34.HorizontalAlignment = Element.ALIGN_LEFT;
            celda34.BorderWidth = 0;
            tabla2.AddCell(celda34);
            //TITULO---------------------------------->
            PdfPCell celda35 = new PdfPCell(new Phrase("Fax", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda35.HorizontalAlignment = Element.ALIGN_LEFT;
            celda35.BorderWidth = 0;
            tabla2.AddCell(celda35);

            tabla2.AddCell(celda_vaciaSinborde);

            //TITULO---------------------------------->
            PdfPCell celda36 = new PdfPCell(new Phrase("Fax", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda36.HorizontalAlignment = Element.ALIGN_LEFT;
            celda36.BorderWidth = 0;
            tabla2.AddCell(celda36);

            tabla2.AddCell(celda_vaciaSinborde);
            //TITULO---------------------------------->
            PdfPCell celda37 = new PdfPCell(new Phrase("Atención A", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda37.HorizontalAlignment = Element.ALIGN_LEFT;
            celda37.BorderWidth = 0;
            tabla2.AddCell(celda37);

            tabla2.AddCell(celda_vaciaSinborde);

            tabla2.AddCell(celda_vaciaSinborde);

            tabla2.AddCell(celda_vaciaSinborde);
            //TITULO---------------------------------->
            PdfPCell celda38 = new PdfPCell(new Phrase("Horario de Recepción", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda38.HorizontalAlignment = Element.ALIGN_LEFT;
            celda38.BorderWidth = 0;
            tabla2.AddCell(celda38);

            PdfPCell celda38_1 = new PdfPCell(new Phrase(sp_datos_class.horario_recept, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda38_1.HorizontalAlignment = Element.ALIGN_LEFT;
            celda38_1.BorderWidth = 0;
            tabla2.AddCell(celda38_1);

            tabla2.AddCell(celda_vaciaSinborde);

            tabla2.AddCell(celda_vaciaSinborde);
            //TITULO---------------------------------->
            PdfPCell celda39 = new PdfPCell(new Phrase("Cond de Despacho", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda39.HorizontalAlignment = Element.ALIGN_LEFT;
            celda39.BorderWidth = 0;
            tabla2.AddCell(celda39);

            PdfPCell celda39_1 = new PdfPCell(new Phrase(sp_datos_class.cond_despacho, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda39_1.HorizontalAlignment = Element.ALIGN_LEFT;
            celda39_1.BorderWidth = 0;
            tabla2.AddCell(celda39_1);

            tabla2.AddCell(celda_bordeTop);

            tabla2.AddCell(celda_bordeTop);

            tabla2.AddCell(celda_bordeTop);

            tabla2.AddCell(celda_bordeTop);
            //TITULO---------------------------------->
            PdfPCell celda40 = new PdfPCell(new Phrase("Pago", FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.RED)));
            celda40.HorizontalAlignment = Element.ALIGN_LEFT;
            celda40.BorderWidth = 0;
            tabla2.AddCell(celda40);

            tabla2.AddCell(celda_vaciaSinborde);
            //TITULO---------------------------------->
            PdfPCell celda41 = new PdfPCell(new Phrase("Transporte", FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.RED)));
            celda41.HorizontalAlignment = Element.ALIGN_LEFT;
            celda41.BorderWidth = 0;
            tabla2.AddCell(celda41);

            tabla2.AddCell(celda_vaciaSinborde);
            //TITULO---------------------------------->
            PdfPCell celda42 = new PdfPCell(new Phrase("Forma de Pago", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda42.HorizontalAlignment = Element.ALIGN_LEFT;
            celda42.BorderWidth = 0;
            tabla2.AddCell(celda42);

            PdfPCell celda43 = new PdfPCell(new Phrase(sp_datos_class.forma_pago, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda43.HorizontalAlignment = Element.ALIGN_LEFT;
            celda43.BorderWidth = 0;
            tabla2.AddCell(celda43);
            //TITULO---------------------------------->
            PdfPCell celda44 = new PdfPCell(new Phrase("Bodega o Sucursal", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda44.HorizontalAlignment = Element.ALIGN_LEFT;
            celda44.BorderWidth = 0;
            tabla2.AddCell(celda44);

            PdfPCell celda45 = new PdfPCell(new Phrase(sp_datos_class.bodega_o_sucurs, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda45.HorizontalAlignment = Element.ALIGN_LEFT;
            celda45.BorderWidth = 0;
            tabla2.AddCell(celda45);
            //TITULO---------------------------------->
            PdfPCell celda46 = new PdfPCell(new Phrase("Cond. de pago", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda46.HorizontalAlignment = Element.ALIGN_LEFT;
            celda46.BorderWidth = 0;
            tabla2.AddCell(celda46);

            PdfPCell celda47 = new PdfPCell(new Phrase(sp_datos_class.cond_pago, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda47.HorizontalAlignment = Element.ALIGN_LEFT;
            celda47.BorderWidth = 0;
            tabla2.AddCell(celda47);
            //TITULO---------------------------------->
            PdfPCell celda48 = new PdfPCell(new Phrase("Valor Flete", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda48.HorizontalAlignment = Element.ALIGN_LEFT;
            celda48.BorderWidth = 0;
            tabla2.AddCell(celda48);

            PdfPCell celda48_1 = new PdfPCell(new Phrase(sp_datos_class.valor_flete, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda48_1.HorizontalAlignment = Element.ALIGN_LEFT;
            celda48_1.BorderWidth = 0;
            tabla2.AddCell(celda48_1);
            //TITULO---------------------------------->
            PdfPCell celda49 = new PdfPCell(new Phrase("Moneda", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda49.HorizontalAlignment = Element.ALIGN_LEFT;
            celda49.BorderWidth = 0;
            tabla2.AddCell(celda49);

            PdfPCell celda50 = new PdfPCell(new Phrase(sp_datos_class.moneda_sp, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda50.HorizontalAlignment = Element.ALIGN_LEFT;
            celda50.BorderWidth = 0;
            tabla2.AddCell(celda50);
            //TITULO---------------------------------->
            PdfPCell celda51 = new PdfPCell(new Phrase("Comentarios", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda51.HorizontalAlignment = Element.ALIGN_LEFT;
            celda51.BorderWidth = 0;
            tabla2.AddCell(celda51);

            PdfPCell celda51_1 = new PdfPCell(new Phrase(sp_datos_class.comentario, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda51_1.HorizontalAlignment = Element.ALIGN_LEFT;
            celda51_1.BorderWidth = 0;
            tabla2.AddCell(celda51_1);
            //TITULO---------------------------------->
            PdfPCell celda52 = new PdfPCell(new Phrase("T. Cambio", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda52.HorizontalAlignment = Element.ALIGN_LEFT;
            celda52.BorderWidth = 0;
            tabla2.AddCell(celda52);

            PdfPCell celda53 = new PdfPCell(new Phrase(sp_datos_class.t_cambio, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            celda53.HorizontalAlignment = Element.ALIGN_LEFT;
            celda53.BorderWidth = 0;
            tabla2.AddCell(celda53);

            tabla2.AddCell(celda_vaciaSinborde);

            tabla2.AddCell(celda_vaciaSinborde);

            tabla2.AddCell(celda_bordeTop);

            tabla2.AddCell(celda_bordeTop);

            tabla2.AddCell(celda_bordeTop);

            tabla2.AddCell(celda_bordeTop);
            //TITULO---------------------------------->
            PdfPCell celda54 = new PdfPCell(new Phrase("Detalle de Productos", FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.RED)));
            celda54.HorizontalAlignment = Element.ALIGN_LEFT;
            celda54.BorderWidth = 0;
            tabla2.AddCell(celda54);

            tabla2.AddCell(celda_vaciaSinborde);

            float[] MedCeldas = { 0.15f, 0.35f, 0.15f, 0.35f };
            tabla2.SetWidths(MedCeldas);


            return tabla2;
        }

        internal static string monto_format(string p)
        {
            //double d;
            //double.TryParse(p, out d);
            //string aux = "";
            //if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
            //return aux;


            try
            {
                double d3;
                double.TryParse(p.Substring(0, p.IndexOf(",")), out d3);
                string aux3 = "";
                if (d3 == 0) { aux3 = "0"; } else { aux3 = d3.ToString("N0"); }
                return aux3 + p.Substring(p.IndexOf(","));
            }
            catch
            {
                double d4;
                double.TryParse(p, out d4);
                string aux4 = "";
                if (d4 == 0) { aux4 = ""; } else { aux4 = d4.ToString("N0"); }
                return aux4;
            }
        }

        internal static string monto_format_cero(string p)
        {
            //double d;
            //double.TryParse(p, out d);
            //string aux = "";
            //if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
            //return aux;


            try
            {
                double d3;
                double.TryParse(p.Substring(0, p.IndexOf(",")), out d3);
                string aux3 = "";
                if (d3 == 0) { aux3 = "0"; } else { aux3 = d3.ToString("N0"); }
                return aux3 + p.Substring(p.IndexOf(","));
            }
            catch
            {
                double d4;
                double.TryParse(p, out d4);
                string aux4 = "";
                if (d4 == 0) { aux4 = "0"; } else { aux4 = d4.ToString("N0"); }
                return aux4;
            }
        }
        internal static DataTable sp_detalle(string NUM_SP)
        {

            DataTable dat_detalle_Sp = ReporteRNegocio.VM_listar_detalle_sp_2(" where coddocumento = '" + NUM_SP + "'");
            return dat_detalle_Sp;
        }


        internal static Sp_datos sp_encabezado(string NUM_SP)
        {

            DataTable dat_sp = ReporteRNegocio.VM_LISTAR_SP_2(" where coddocumento = '" + NUM_SP + "'");
            //DataTable dat_detalle_Sp = ReporteRNegocio.VM_listar_detalle_sp_2(" where coddocumento = '" + NUM_SP + "'");

            Sp_datos sp_datos_class = new Sp_datos();


            foreach (DataRow r in dat_sp.Rows)
            {
                sp_datos_class.sp = r["CodDocumento"].ToString();
                sp_datos_class.estado_sp = r["DescEstadoDocumento"].ToString();
                sp_datos_class.fecha_emi = ((DateTime)r["FechaCreacion"]).ToString("dd/MM/yyy" + ".").Substring(0, 10).Trim();
                sp_datos_class.bodega = r["CodBodega"].ToString();
                sp_datos_class.destino = r["DirDireccion1"].ToString();
                sp_datos_class.fecha_desp = r["FechaDespacho"].ToString().Substring(0, 10).Trim();
                sp_datos_class.direccion = r["DIRECCION_FACTURAR_A"].ToString();
                sp_datos_class.nombre_cliente = r["RazonSocial"].ToString().Trim();
                sp_datos_class.rut_cliente = r["RUT"].ToString() + "-" + r["DVRUT"].ToString();
                //rut_cliente = rut_cliente.TrimStart('0');
                sp_datos_class.vendedor = r["NombreVendedor"].ToString();
                sp_datos_class.CodEmisor = r["CodEmisor"].ToString();
                sp_datos_class.ciudad = r["COMUNA_FACTURAR_A"].ToString();
                sp_datos_class.fono = r["fono"].ToString();
                sp_datos_class.giro = r["CODgiro"].ToString();
                sp_datos_class.atenciona = r["atenciona"].ToString();
                sp_datos_class.horario_recept = r["HorarioRecepcion"].ToString();
                sp_datos_class.cond_despacho = r["CondDespacho"].ToString();
                sp_datos_class.direccion_despacho = r["DIRECCION_DESPACHAR_A"].ToString();
                sp_datos_class.comuna_despacho = r["COMUNA_DESPACHAR_A"].ToString();

                sp_datos_class.forma_pago = r["DescFormaPago"].ToString();
                sp_datos_class.cond_pago = r["DescCondicionVenta"].ToString();
                sp_datos_class.moneda_sp = r["DescMoneda"].ToString();
                sp_datos_class.t_cambio = r["Descr_Tipo_cambio"].ToString();

                sp_datos_class.bodega_o_sucurs = r["DescBodega"].ToString();
                sp_datos_class.valor_flete = r["ValorFlete"].ToString();
                sp_datos_class.comentario = r["NotaDespacho"].ToString();
                sp_datos_class.valor_t_cambio = r["ValorTipoCambio"].ToString();
                sp_datos_class.nota_libre = r["NotaLibre"].ToString();
                break;
            }
            return sp_datos_class;
        }



        internal static string crear_sp_formato(DataTable dat_sp, DataTable dat_detalle_Sp)
        {

            Sp_datos sp_datos_class = new Sp_datos();


            foreach (DataRow r in dat_sp.Rows)
            {
                sp_datos_class.sp = r["CodDocumento"].ToString();
                sp_datos_class.estado_sp = r["DescEstadoDocumento"].ToString();
                sp_datos_class.fecha_emi = ((DateTime)r["FechaCreacion"]).ToString("dd/MM/yyy" + ".").Substring(0, 10).Trim();
                sp_datos_class.bodega = r["CodBodega"].ToString();
                sp_datos_class.destino = r["DirDireccion1"].ToString();
                sp_datos_class.fecha_desp = r["FechaDespacho"].ToString().Substring(0, 10).Trim();
                sp_datos_class.direccion = r["DIRECCION_FACTURAR_A"].ToString();
                sp_datos_class.nombre_cliente = r["RazonSocial"].ToString().Trim();
                sp_datos_class.rut_cliente = r["RUT"].ToString() + "-" + r["DVRUT"].ToString();
                //rut_cliente = rut_cliente.TrimStart('0');
                sp_datos_class.vendedor = r["NombreVendedor"].ToString();
                sp_datos_class.CodEmisor = r["CodEmisor"].ToString();
                sp_datos_class.ciudad = r["COMUNA_FACTURAR_A"].ToString();
                sp_datos_class.fono = r["fono"].ToString();
                sp_datos_class.giro = r["CODgiro"].ToString();
                sp_datos_class.atenciona = r["atenciona"].ToString();
                sp_datos_class.horario_recept = r["HorarioRecepcion"].ToString();
                sp_datos_class.cond_despacho = r["CondDespacho"].ToString();
                sp_datos_class.direccion_despacho = r["DIRECCION_DESPACHAR_A"].ToString();
                sp_datos_class.comuna_despacho = r["COMUNA_DESPACHAR_A"].ToString();

                sp_datos_class.forma_pago = r["DescFormaPago"].ToString();
                sp_datos_class.cond_pago = r["DescCondicionVenta"].ToString();
                sp_datos_class.moneda_sp = r["DescMoneda"].ToString();
                sp_datos_class.t_cambio = r["Descr_Tipo_cambio"].ToString();

                sp_datos_class.bodega_o_sucurs = r["DescBodega"].ToString();
                sp_datos_class.valor_flete = r["ValorFlete"].ToString();
                sp_datos_class.comentario = r["NotaDespacho"].ToString();
                sp_datos_class.valor_t_cambio = r["ValorTipoCambio"].ToString();
                sp_datos_class.nota_libre = r["NotaLibre"].ToString();
                break;
            }

            string SP_formato = "";

            SP_formato += " <div class='table' align='center' style='text-align:center'> ";
            SP_formato += "         <hr size='4' width='100%' align='center'>";
            SP_formato += " </div>";

            //ENCACEBZADO
            SP_formato += " <table class='table' border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100% !important;'> ";
            SP_formato += "  <tbody> ";
            SP_formato += "   <tr>";
            SP_formato += "       <td style='padding:0cm 0cm 0cm 0cm'>";
            SP_formato += "         <span class='table' align='right' style='text-align:right'><b><span style='font-size:9.5pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>" +
                "                               Solicitud de pedido N° " + sp_datos_class.sp + " (" + sp_datos_class.estado_sp + ")</span></b><u></u><u></u></span>";
            SP_formato += "       </td>";
            SP_formato += "   </tr>";
            SP_formato += "  </tbody>";
            SP_formato += " </table> ";

            SP_formato += " <div class='table' align='center' style='text-align:center'> ";
            SP_formato += "         <hr size='2' width='100%' align='center'>";
            SP_formato += " </div>";

            //DATOS PROVEEDOR Y VENDEDOR
            SP_formato += " <table class='table' border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100% !important;'> ";
            SP_formato += "  <tbody> ";
            SP_formato += "     <tr> ";
            SP_formato += "        <td colspan='4' style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style='COLOR:RED;font-size:9.5pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>Datos Proveedor</span></b><u></u><u></u></span> ";
            SP_formato += "        </td> ";
            SP_formato += "     </tr> ";
            SP_formato += "     <tr> ";
            SP_formato += "         <td width='15%' style='width:15.0%;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>Dirección</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td width='35%' style='width:35.0%;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>" + sp_datos_class.proveedor_dir + "</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td width='15%' style='width:15.0%;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style='COLOR:RED;font-size:9.5pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>Vendedor</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td width='35%' style='width:35.0%;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>" + sp_datos_class.vendedor + "</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "     </tr> ";
            SP_formato += "     <tr> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>Teléfono</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>" + sp_datos_class.proveedor_fono + "</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>Fecha Cierre</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>" + sp_datos_class.fecha_emi + "</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "     </tr> ";
            SP_formato += "     <tr> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>Fax</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>" + sp_datos_class.proveedor_fax + "</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>Fecha Operación</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>" + sp_datos_class.fecha_emi + "</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "     </tr> ";
            SP_formato += "     <tr> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>Email</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td colspan='3' style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'><a href='mailto:" + sp_datos_class.proveedor_email + "' target='_blank'>" + sp_datos_class.proveedor_email + "</a></span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "     </tr> ";
            SP_formato += "     <tr> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>URL</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td colspan='3' style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'><a href='" + sp_datos_class.proveedor_URL + "' target='_blank' data-saferedirecturl='https://www.google.com/url?q=http://www.soprodi.cl&amp;source=gmail&amp;ust=1561124570847000&amp;usg=AFQjCNHUOQ6_utFJQxl8a6v7Wxh17sLp8Q'>" + sp_datos_class.proveedor_URL + "</a></span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "     </tr> ";
            SP_formato += " </tbody> ";
            SP_formato += " </table> ";

            SP_formato += " <div class='table' align='center' style='text-align:center'> ";
            SP_formato += "         <hr size='2' width='100%' align='center'>";
            SP_formato += " </div>";

            //FACTURAR Y DESPACHAR
            SP_formato += " <table class='table' border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100% !important;'> ";
            SP_formato += " <tbody> ";
            SP_formato += "     <tr> ";
            SP_formato += "         <td colspan = '2' style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'COLOR:RED;font-size:9.5pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Facturar a</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td colspan = '2' style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'COLOR:RED;font-size:9.5pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Despachar a</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "     </tr> ";
            SP_formato += "     <tr> ";
            SP_formato += "         <td width = '15%' style='width:15.0%;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Razón Social</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td width = '35%' style='width:35.0%;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' >" + sp_datos_class.nombre_cliente + "</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td width = '20%' style= 'width:20.0%;padding:0cm 0cm 0cm 0cm' > ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Fecha Entrega</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td width = '30%' style='width:30.0%;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' >" + sp_datos_class.fecha_desp + " </span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "     </tr> ";
            SP_formato += "     <tr> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Rut </span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + sp_datos_class.rut_cliente + "  </span></b><u></u><u></u></span>";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Dirección </span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' >" + sp_datos_class.direccion + "</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "     </tr> ";
            SP_formato += "     <tr> ";
            SP_formato += "         <td style = 'padding:0cm 0cm 0cm 0cm' > ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Giro </span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + sp_datos_class.giro + "</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style = 'padding:0cm 0cm 0cm 0cm' > ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Ciudad </span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + sp_datos_class.ciudad + " </span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "     </tr> ";
            SP_formato += "     <tr> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Dirección </span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' >" + sp_datos_class.direccion_despacho + "</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style = 'padding:0cm 0cm 0cm 0cm' > ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Teléfono </span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + sp_datos_class.fono + " </span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "     </tr> ";
            SP_formato += "     <tr > ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Ciudad </span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + sp_datos_class.comuna_despacho + " </span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Fax </span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'></td> ";
            SP_formato += "     </tr> ";
            SP_formato += "     <tr> ";
            SP_formato += "         <td style = 'padding:0cm 0cm 0cm 0cm' > ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Teléfono </span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + sp_datos_class.fono + " </span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Atención A</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style = 'padding:0cm 0cm 0cm 0cm' ></td> ";
            SP_formato += "     </tr> ";
            SP_formato += "     <tr> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Fax </span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style='padding:0cm 0cm 0cm 0cm'>";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > </span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style = 'padding:0cm 0cm 0cm 0cm' > ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Horario de Recepción</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style = 'padding:0cm 0cm 0cm 0cm' >";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + sp_datos_class.horario_recept + " </span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "     </tr> ";
            SP_formato += "     <tr> ";
            SP_formato += "         <td colspan= '2' style= 'padding:0cm 0cm 0cm 0cm' > ";
            SP_formato += "             <span class='table'>&nbsp;<u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style = 'padding:0cm 0cm 0cm 0cm' > ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Cond.de Despacho</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "         <td style = 'padding:0cm 0cm 0cm 0cm' > ";
            SP_formato += "             <span class='table'><b><span style = 'font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' >" + sp_datos_class.cond_despacho + "</span></b><u></u><u></u></span> ";
            SP_formato += "         </td> ";
            SP_formato += "     </tr> ";
            SP_formato += "</tbody> ";
            SP_formato += "</table> ";

            SP_formato += " <div class='table' align='center' style='text-align:center'> ";
            SP_formato += "         <hr size='2' width='100%' align='center'>";
            SP_formato += " </div>";

            //PAGO TRANSPORTE
            SP_formato += " <table class='table' border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100% !important;'> ";
            SP_formato += "<tbody> ";
            SP_formato += "   <tr> ";
            SP_formato += "      <td colspan='2' style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "          <span class='table'><b><span style='COLOR:RED;font-size:9.5pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>Pago</span></b><u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td colspan='2' style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <span class='table'><b><span style='COLOR:RED;font-size:9.5pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>Transporte</span></b><u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += "   </tr> ";
            SP_formato += "   <tr> ";
            SP_formato += "     <td width='15 %' style='width:15.0%;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>Forma de Pago</span></b><u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td width='35 %' style='width:35.0%;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>" + sp_datos_class.forma_pago + " </span></b><u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td width='15 %' style='width:15.0%;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>Bodega o Sucursal</span></b><u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td width='35 %' style='width:35.0%;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>" + sp_datos_class.bodega_o_sucurs + "</span></b><u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += "   </tr> ";
            SP_formato += "   <tr> ";
            SP_formato += "     <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>Cond. de Pago</span></b><u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>" + sp_datos_class.cond_pago + "</span></b><u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>Valor Flete</span></b><u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td style='padding:0cm 0cm 0cm 0cm'>";
            SP_formato += "         <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>" + sp_datos_class.valor_flete + "</span></b><u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += "   </tr> ";
            SP_formato += "   <tr> ";
            SP_formato += "     <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>Moneda</span></b><u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>" + sp_datos_class.moneda_sp + "</span></b><u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>Comentarios</span></b><u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td style='padding:0cm 0cm 0cm 0cm'></td> ";
            SP_formato += "   </tr> ";
            SP_formato += "   <tr> ";
            SP_formato += "     <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>T. Cambio</span></b><u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <span class='table'><b><span style='font-size:8pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>" + sp_datos_class.t_cambio + "</span></b><u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <span class='table'>&nbsp;<u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td style='padding:0cm 0cm 0cm 0cm'></td> ";
            SP_formato += "   </tr> ";
            SP_formato += "</tbody> ";
            SP_formato += "</table> ";

            SP_formato += " <div class='table' align='center' style='text-align:center'> ";
            SP_formato += "         <hr size='2' width='100%' align='center'>";
            SP_formato += " </div>";

            //DETALLE
            SP_formato += " <table class='table' border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100% !important;'> ";
            SP_formato += "     <tbody> ";
            SP_formato += "         <tr> ";
            SP_formato += "             <td colspan = '3' style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "                 <span class='table'><b><span style = 'COLOR:RED;font-size:9.5pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Detalle de Productos</span></b><u></u><u></u></span> ";
            SP_formato += "             </td> ";
            SP_formato += "         </tr> ";
            SP_formato += "         <tr> ";
            SP_formato += "             <td colspan = '3' valign= 'top' style= 'padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "<div align= 'center'> ";

            //tabla detalle ---->

            //head detalle ----->
            SP_formato += "     <table class='table' border='1' cellspacing='0' cellpadding='0' width='90%' style='width:90.0%;border:solid black 1.0pt'> ";
            SP_formato += "       <tbody> ";
            SP_formato += "         <tr> ";
            SP_formato += "             <td style = 'border:solid black 1.0pt;padding:0cm 0cm 0cm 0cm' > ";
            SP_formato += "                 <span class='table' align='center' style='text-align:center'><b><span style = 'font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > CODIGO<u></u><u></u></span></b></span> ";
            SP_formato += "             </td> ";
            SP_formato += "             <td style='border:solid black 1.0pt;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "                 <span class='table' align='center' style='text-align:center'><b><span style = 'font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > PRODUCTO<u></u><u></u></span></b></span> ";
            SP_formato += "             </td> ";
            SP_formato += "             <td style='border:solid black 1.0pt;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "                 <span class='table' align='center' style='text-align:center'><b><span style = 'font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > CANTIDAD<u></u><u></u></span></b></span> ";
            SP_formato += "             </td> ";
            SP_formato += "             <td style='border:solid black 1.0pt;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "                 <span class='table' align='center' style='text-align:center'><b><span style = 'font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > UN.MEDIDA<u></u><u></u></span></b></span> ";
            SP_formato += "             </td> ";
            SP_formato += "             <td style='border:solid black 1.0pt;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "                 <span class='table' align='center' style='text-align:center'><b><span style = 'font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > PRECIO UNITARIO<u></u><u></u></span></b></span> ";
            SP_formato += "             </td> ";
            SP_formato += "             <td style = 'border:solid black 1.0pt;padding:0cm 0cm 0cm 0cm' > ";
            SP_formato += "                 <span class='table' align='center' style='text-align:center'><b><span style = 'font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' >% DESCUENTO<u></u><u></u></span></b></span> ";
            SP_formato += "             </td> ";
            SP_formato += "             <td style='border:solid black 1.0pt;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "                 <span class='table' align='center' style='text-align:center'><b><span style = 'font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > PRECIO UNITARIO FINAL<u></u><u></u></span></b></span> ";
            SP_formato += "             </td> ";
            SP_formato += "             <td style = 'border:solid black 1.0pt;padding:0cm 0cm 0cm 0cm' > ";
            SP_formato += "                 <span class='table' align='center' style='text-align:center'><b><span style = 'font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > TOTAL<u></u><u></u></span></b></span> ";
            SP_formato += "             </td> ";
            SP_formato += "         </tr> ";

            //body detalle ---->
            double sub_total = 0;

            Sp_datos_detalle sp_detalle_class = new Sp_datos_detalle();

            foreach (DataRow r_det in dat_detalle_Sp.Rows)
            {
                sp_detalle_class.codigo = r_det["codproducto"].ToString();
                sp_detalle_class.producto = r_det["descproducto"].ToString();
                sp_detalle_class.cantidad = r_det["cantidad"].ToString();
                sp_detalle_class.un_medida = r_det["descUnVenta"].ToString();
                sp_detalle_class.precio_unit = r_det["PrecioUnitario"].ToString();
                sp_detalle_class.descuento = r_det["Descuento"].ToString();
                sp_detalle_class.precio_unit_final = r_det["PrecioUnitarioFinal"].ToString();

                string total = "";

                try
                {

                    double temp_CANT = Convert.ToDouble(sp_detalle_class.cantidad);
                    double temp_precio_unit_final = Convert.ToDouble(sp_detalle_class.precio_unit_final);

                    double temp_total = temp_CANT * temp_precio_unit_final;

                    total = Base.monto_format2( Math.Round( temp_total, MidpointRounding.AwayFromZero) );

                    sub_total += temp_total;
                }
                catch { }



                SP_formato += "      <tr> ";
                SP_formato += "         <td style='border:solid black 1.0pt;padding:0cm 0cm 0cm 0cm'> ";
                SP_formato += "             <span class='table'><span style = 'font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + sp_detalle_class.codigo + " <u></u><u></u></span></span> ";
                SP_formato += "         </td> ";
                SP_formato += "         <td style='border:solid black 1.0pt;padding:0cm 0cm 0cm 0cm'> ";
                SP_formato += "             <span class='table'><span style = 'font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + sp_detalle_class.producto + "<u></u><u></u></span></span> ";
                SP_formato += "         </td> ";
                SP_formato += "         <td style = 'border:solid black 1.0pt;padding:0cm 0cm 0cm 0cm' > ";
                SP_formato += "             <span class='table'><span style = 'font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + sp_detalle_class.cantidad + "<u></u><u></u></span></span> ";
                SP_formato += "         </td> ";
                SP_formato += "         <td style = 'border:solid black 1.0pt;padding:0cm 0cm 0cm 0cm' > ";
                SP_formato += "             <span class='table'><span style = 'font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + sp_detalle_class.un_medida + "<u></u><u></u></span></span> ";
                SP_formato += "         </td> ";
                SP_formato += "         <td style='border:solid black 1.0pt;padding:0cm 0cm 0cm 0cm'> ";
                SP_formato += "             <span class='table'><span style = 'font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' >" + sp_detalle_class.precio_unit + "<u></u><u></u></span></span> ";
                SP_formato += "         </td> ";
                SP_formato += "         <td style = 'border:solid black 1.0pt;padding:0cm 0cm 0cm 0cm' > ";
                SP_formato += "             <span class='table'><span style = 'font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + sp_detalle_class.descuento + "<u></u><u></u></span></span> ";
                SP_formato += "         </td> ";
                SP_formato += "         <td style = 'border:solid black 1.0pt;padding:0cm 0cm 0cm 0cm' > ";
                SP_formato += "             <span class='table'><span style = 'font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + sp_detalle_class.precio_unit_final + "<u></u><u></u></span></span> ";
                SP_formato += "         </td> ";
                SP_formato += "         <td style = 'border:solid black 1.0pt;padding:0cm 0cm 0cm 0cm' > ";
                SP_formato += "             <span class='table'><span style = 'font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " +  total + "<u></u><u></u></span></span> ";
                SP_formato += "         </td> ";
                SP_formato += "     </tr> ";
            }

            sub_total = Math.Round(sub_total, MidpointRounding.AwayFromZero);

            int subtotal_int = Convert.ToInt32(Math.Round(sub_total, MidpointRounding.AwayFromZero));
            int iva = Convert.ToInt32(Math.Round((sub_total * 0.19), MidpointRounding.AwayFromZero));
            int total_2 = subtotal_int + iva;

            string sub_total_formato = Base.monto_format2(subtotal_int);
            string iva_formato = Base.monto_format2(iva);
            string total_formato = Base.monto_format2(total_2);

            SP_formato += "   </tbody> ";
            SP_formato += " </table> ";
            SP_formato += "</div> ";

            SP_formato += "  </td> ";
            SP_formato += " </tr> ";
            SP_formato += " <tr> ";
            SP_formato += "     <td colspan = '3' style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <span class='table'>&nbsp;<u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += " </tr> ";
            SP_formato += " <tr> ";
            SP_formato += "     <td width = '75 %' style='width:75.0%;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <p class='table' align='right' style='text-align:right'><b><span style = 'font-size:10pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > Subtotal </span></b><u></u><u></u></p> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td width='20 %' style='width:20.0%;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <p class='table' align='right' style='text-align:right'><b><span style = 'font-size:10pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + sub_total_formato + "</span></b><u></u><u></u></p> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td width = '5 %' style='width:5.0%;padding:0cm 0cm 0cm 0cm'></td> ";
            SP_formato += " </tr> ";
            SP_formato += " <tr> ";
            SP_formato += "     <td style = 'padding:0cm 0cm 0cm 0cm' > ";
            SP_formato += "         <p class='table' align='right' style='text-align:right'><b><span style = 'font-size:10pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' >% Descuento </span></b><u></u><u></u></p> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <p class='table' align='right' style='text-align:right'><b><span style = 'font-size:10pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > 0,000</span></b><u></u><u></u></p> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td width = '5 %' style='width:5.0%;padding:0cm 0cm 0cm 0cm'></td> ";
            SP_formato += " </tr> ";
            SP_formato += " <tr> ";
            SP_formato += "     <td style = 'padding:0cm 0cm 0cm 0cm' > ";
            SP_formato += "         <p class='table' align='right' style='text-align:right'><b><span style = 'font-size:10pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > I.V.A </span></b><u></u><u></u></p> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <p class='table' align='right' style='text-align:right'><b><span style = 'font-size:10pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' >" + iva_formato + "</span></b><u></u><u></u></p> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td width = '5 %' style='width:5.0%;padding:0cm 0cm 0cm 0cm'></td> ";
            SP_formato += " </tr> ";
            SP_formato += " <tr> ";
            SP_formato += "     <td style = 'padding:0cm 0cm 0cm 0cm' > ";
            SP_formato += "         <p class='table' align='right' style='text-align:right'><b><span style = 'font-size:10pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > TOTAL </span></b><u></u><u></u></span> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "         <p class='table' align='right' style='text-align:right'><b><span style = 'font-size:10pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;' > " + total_formato   + "</span></b><u></u><u></u></p> ";
            SP_formato += "     </td> ";
            SP_formato += "     <td width = '5 %' style='width:5.0%;padding:0cm 0cm 0cm 0cm'></td> ";
            SP_formato += " </tr> ";
            SP_formato += " </tbody> ";
            SP_formato += " </table> ";

            SP_formato += " <div class='table' align='center' style='text-align:center'> ";
            SP_formato += "         <hr size='2' width='100%' align='center'>";
            SP_formato += " </div>";

            //TRAMOFINAL
            SP_formato += " <table class='table' border='0' cellspacing='0' cellpadding='0'  style='width:100% !important;'> ";
            SP_formato += "    <tbody> ";
            SP_formato += "         <tr> ";
            SP_formato += "             <td colspan='3' style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "                 <span class='table'>&nbsp;<b><span style='font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>" + sp_datos_class.nota_libre + " </span></b><u></u><u></u></span> ";
            SP_formato += "             </td> ";
            SP_formato += "         </tr> ";
            SP_formato += "         <tr> ";
            SP_formato += "             <td width='10%' style='width:10.0%;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "                 <span class='table'>&nbsp;<u></u><u></u></span> ";
            SP_formato += "             </td> ";
            SP_formato += "             <td width='80%' style='width:80.0%;padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "             <table class='table' border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100.0%'> ";
            SP_formato += "                 <tbody> ";
            SP_formato += "                     <tr> ";
            SP_formato += "                         <td style='padding:0cm 0cm 0cm 0cm'> ";
            SP_formato += "                             <span class='table' align='center' style='text-align:center'>&nbsp;<u></u><u></u></span> ";
            SP_formato += "                         </td> ";
            SP_formato += "                     </tr> ";
            SP_formato += "                     <tr> ";
            SP_formato += "                         <td style='padding:0cm 0cm 0cm 0cm'> ";
            //ACA SE SACO A PETICION soprodi
            if (sp_datos_class.moneda_sp.Contains("PESO"))
            {
                SP_formato += "                             <span class='table' align='center' style='text-align:center'><b><span style='font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'>Tipo de Cambio : " + sp_datos_class.valor_t_cambio + "</span></b><u></u><u></u></span> ";
            }
            else
            {
                SP_formato += "                             <span class='table' align='center' style='text-align:center'><b><span style='font-size:9pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'></span></b><u></u><u></u></span> ";
            }
            SP_formato += "                         </td> ";
            SP_formato += "                     </tr> ";
            SP_formato += "                 </tbody> ";
            SP_formato += "             </table> ";
            SP_formato += "             </td> ";
            SP_formato += "             <td style='padding:0cm 0cm 0cm 0cm'></td> ";
            SP_formato += "              </tr> ";
            SP_formato += "     </tbody> ";
            SP_formato += " </table> ";

            return SP_formato;
        }

        internal static void crear_sp_pdf(string sp, string pdfPath_)
        {
            string num_sp = sp;
            string pdfPath = pdfPath_;
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
                        PdfPCell celda1 = new PdfPCell(new Phrase("Solicitud de pedido N° " + num_sp + " (" + sp_datos_class.estado_sp + ")", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
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

                        if (sp_datos_class.moneda_sp.Contains("PESO"))
                        {
                            PdfPCell celda80 = new PdfPCell(new Phrase("Tipo de Cambio: " + sp_datos_class.valor_t_cambio, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
                            celda80.HorizontalAlignment = Element.ALIGN_LEFT;
                            celda80.BorderWidth = 0;
                            tabla5.AddCell(celda80);                 
                        }
                        else
                        {
                            PdfPCell celda80 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
                            celda80.HorizontalAlignment = Element.ALIGN_LEFT;
                            celda80.BorderWidth = 0;
                            tabla5.AddCell(celda80);
                        }
                        
                        pdfDoc.Add(tabla5);
                        pdfDoc.NewPage();
                        pdfDoc.Close();
                        WebClient client = new WebClient();
                        Byte[] buffer = client.DownloadData(pdfPath);


                        ///NO SE MUESTRA ... SOLO LO CREA Y LOS RESPALDA 
                        ///

                        //Response.ContentType = "application/pdf";
                        //Response.AddHeader("content-length", buffer.Length.ToString());
                        //Response.BinaryWrite(buffer);
                        //Response.Flush();
                        //Response.End();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        internal static string monto_format2(double y)
        {
            string p = y.ToString();
            //double d;
            //double.TryParse(p, out d);
            //string aux = "";
            //if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
            //return aux;
            try
            {
                if (p.Substring(p.IndexOf(",")).Length == 3) { p = p + "0"; }
            }
            catch { }
            try
            {
                double d3;
                double.TryParse(p.Substring(0, p.IndexOf(",")), out d3);
                string aux3 = "";
                if (d3 == 0) { aux3 = "0"; } else { aux3 = d3.ToString("N0"); }
                return aux3 + p.Substring(p.IndexOf(","), 4);
            }
            catch
            {
                double d4;
                double.TryParse(p, out d4);
                string aux4 = "";
                if (d4 == 0) { aux4 = ""; } else { aux4 = d4.ToString("N0"); }
                return aux4;
            }
        }

        internal static string format_rut(string p)
        {
            string valor = p;
            string rut_ini = valor.Trim().Substring(0, valor.Trim().Length - 1);
            double rut = 0;
            try { rut = double.Parse(rut_ini); return rut.ToString("N0") + "-" + valor.Trim().Substring(valor.Trim().Length - 1); }
            catch { rut = double.Parse(valor); return rut.ToString("N0"); }
        }


        internal static string ficha_cliente(string rutcliente, string valor_muestra)
        {
            clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

            string script1 = string.Format("javascript:fuera22(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(rutcliente), encriptador.EncryptData("88"));
            return "  <a href='javascript:' onclick='" + script1 + "'>" + valor_muestra + " </a>";
        }

        internal static string click_factura(string año_fact, string valor_muestra)
        {
            clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
            try
            {
                string año_factura = año_fact.Substring(6);
                string script = string.Format("javascript:fuera3(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(valor_muestra.Trim()), encriptador.EncryptData(año_factura));
                return "  <a href='javascript:' onclick='" + script + "'>" + valor_muestra.Trim() + " </a>";
            }
            catch
            {
                return valor_muestra;

            }

        }

    }
}