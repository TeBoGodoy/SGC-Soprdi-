using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoprodiApp.negocio;
using ThinxsysFramework;

namespace SoprodiApp
{
    public partial class REPORTES : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["contraseña"] == null)
            {
                Response.Redirect("Acceso.aspx");

            }

            string sw = Request.QueryString["s"];

            Session["SW_PERMI"] = sw;

            if (sw == "1")
            {

                List<string> u_negocio = ReporteRNegocio.trae_u_negocio(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "1")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }

                muestra_app_abarrotes();
                //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=1'>Abarrotes</a>";
                titulo2.InnerHtml = "Abarrotes";

                string trae_ch = usuarioBO.obtener_check(User.Identity.Name.ToString());
                if (trae_ch == "True")
                {
                    diario.Visible = true;

                }
                else if (trae_ch == "False")
                {
                    diario.Visible = false;

                }
            }
            else if (sw == "2")
            {
                List<string> u_negocio = ReporteRNegocio.trae_u_negocio(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "2")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }

                muestra_app_granos();
                //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=2'>Granos</a>";
                titulo2.InnerHtml = "Granos";

                string trae_ch = usuarioBO.obtener_check(User.Identity.Name.ToString());
                if (trae_ch == "True")
                {
                    diario.Visible = true;

                }
                else if (trae_ch == "False")
                {
                    diario.Visible = false;

                }
            }

            else if (sw == "3")
            {
                titulo2.InnerHtml = "Finanzas";
                //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=2'>Finanzas</a>";
            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();

            Menu();

        }

        private void muestra_app_granos()
        {
            List<string> app = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();

            uno_REPORTE_VENDEDOR.Attributes["style"] = "display:none";
            dos_REPORTE_SALA.Attributes["style"] = "display:none";
            tres_REPORTE_CM.Attributes["style"] = "display:none";
            cuatro_REPORTE_COMPARATIVO.Attributes["style"] = "display:none";
            cinco_REPORTE_LV_B.Attributes["style"] = "display:none";
            seis_REPORTE_LV_C.Attributes["style"] = "display:none";
            seis_REPORTE_LV_C.Attributes["style"] = "display:none";
            siete_REPORTE_LV_P.Attributes["style"] = "display:none";
            ocho_REPORTE_NC.Attributes["style"] = "display:none";
            nueve_REPORTE_VEND_CLIE.Attributes["style"] = "display:none";
            diez_REPORTE_LOG_CORR_FICH.Attributes["style"] = "display:none";
            once_REPORTE_LISTADO_DOC.Attributes["style"] = "display:none";
            doce_REPORTE_STOCK.Attributes["style"] = "display:none";
            trece_REPORTE_EXCEL.Attributes["style"] = "display:none";
            catorce_REPORTE_RENT_FACT.Attributes["style"] = "display:none";
            quince_REPORTE_MATRIZ.Attributes["style"] = "display:none";
            dieciseis_REPORTE_COMPRAS.Attributes["style"] = "display:none";
            diecisiete_REPORTE_COSTOS_IMPORT.Attributes["style"] = "display:none";
            dieciocho_REPORTE_CATEGORIA.Attributes["style"] = "display:none";
            diecinueve_REPORTE_SP.Attributes["style"] = "display:none";
            veinte_REPORTE_STOCKF.Attributes["style"] = "display:none";
            veinti_uno_Menu_Planificador.Attributes["style"] = "display:none";
            veinti_dos_produc.Attributes["style"] = "display:none";
            veinti_tres_REPORTE_SALDOS_SP.Attributes["style"] = "display:none";
            veinti_cuatro_stock_log.Attributes["style"] = "display:none";
            veinti_cinco_sp_diarias.Attributes["style"] = "display:none";

            foreach (string u_ne in app)
            {
                if (u_ne.Trim() == "9")
                {
                    tres_REPORTE_CM.Attributes["style"] = "display:block";
                }

                if (u_ne.Trim() == "7")
                {
                    uno_REPORTE_VENDEDOR.Attributes["style"] = "display:block";
                }

                if (u_ne.Trim() == "8")
                {
                    dos_REPORTE_SALA.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "10")
                {
                    cuatro_REPORTE_COMPARATIVO.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "11")
                {
                    cinco_REPORTE_LV_B.Attributes["style"] = "display:block";
                }             
                if (u_ne.Trim() == "41")
                {
                    ocho_REPORTE_NC.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "18")
                {
                    siete_REPORTE_LV_P.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "42")
                {
                    nueve_REPORTE_VEND_CLIE.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "43")
                {
                    once_REPORTE_LISTADO_DOC.Attributes["style"] = "display:block";
                }

                if (u_ne.Trim() == "40")
                {
                    veinte_REPORTE_STOCKF.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "39")
                {
                    diecinueve_REPORTE_SP.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "44")
                {
                    veinti_uno_Menu_Planificador.Attributes["style"] = "display:block";
                }

                if (u_ne.Trim() == "45")
                {
                    veinti_tres_REPORTE_SALDOS_SP.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "47")
                {
                    veinti_cinco_sp_diarias.Attributes["style"] = "display:block";
                }

                //if (u_ne.Trim() == "35")
                //{
                //    veinti_uno_Menu_Planificador.Attributes["style"] = "display:block";
                //}

            }
        }

        private void muestra_app_abarrotes()
        {
            List<string> app = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();

            uno_REPORTE_VENDEDOR.Attributes["style"] = "display:none";
            dos_REPORTE_SALA.Attributes["style"] = "display:none";
            tres_REPORTE_CM.Attributes["style"] = "display:none";
            cuatro_REPORTE_COMPARATIVO.Attributes["style"] = "display:none";
            cinco_REPORTE_LV_B.Attributes["style"] = "display:none";
            seis_REPORTE_LV_C.Attributes["style"] = "display:none";
            seis_REPORTE_LV_C.Attributes["style"] = "display:none";
            siete_REPORTE_LV_P.Attributes["style"] = "display:none";
            ocho_REPORTE_NC.Attributes["style"] = "display:none";
            nueve_REPORTE_VEND_CLIE.Attributes["style"] = "display:none";
            diez_REPORTE_LOG_CORR_FICH.Attributes["style"] = "display:none";
            once_REPORTE_LISTADO_DOC.Attributes["style"] = "display:none";
            doce_REPORTE_STOCK.Attributes["style"] = "display:none";
            trece_REPORTE_EXCEL.Attributes["style"] = "display:none";
            catorce_REPORTE_RENT_FACT.Attributes["style"] = "display:none";
            quince_REPORTE_MATRIZ.Attributes["style"] = "display:none";
            dieciseis_REPORTE_COMPRAS.Attributes["style"] = "display:none";
            diecisiete_REPORTE_COSTOS_IMPORT.Attributes["style"] = "display:none";
            dieciocho_REPORTE_CATEGORIA.Attributes["style"] = "display:none";
            diecinueve_REPORTE_SP.Attributes["style"] = "display:none";
            veinte_REPORTE_STOCKF.Attributes["style"] = "display:none";
            veinti_uno_Menu_Planificador.Attributes["style"] = "display:none";
            veinti_dos_produc.Attributes["style"] = "display:none";
            veinti_tres_REPORTE_SALDOS_SP.Attributes["style"] = "display:none";
            veinti_cuatro_stock_log.Attributes["style"] = "display:none";
            veinti_cinco_sp_diarias.Attributes["style"] = "display:none";


            foreach (string u_ne in app)
            {
                if (u_ne.Trim() == "1")
                {
                    tres_REPORTE_CM.Attributes["style"] = "display:block";
                }

                if (u_ne.Trim() == "2")
                {
                    uno_REPORTE_VENDEDOR.Attributes["style"] = "display:block";
                }

                if (u_ne.Trim() == "3")
                {
                    dos_REPORTE_SALA.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "4")
                {
                    cuatro_REPORTE_COMPARATIVO.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "5")
                {
                    cinco_REPORTE_LV_B.Attributes["style"] = "display:block";
                }

                if (u_ne.Trim() == "6")
                {
                    seis_REPORTE_LV_C.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "17")
                {
                    siete_REPORTE_LV_P.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "21")
                {
                    ocho_REPORTE_NC.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "22")
                {
                    nueve_REPORTE_VEND_CLIE.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "23")
                {
                    diez_REPORTE_LOG_CORR_FICH.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "24")
                {
                    once_REPORTE_LISTADO_DOC.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "25")
                {
                    doce_REPORTE_STOCK.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "26")
                {
                    trece_REPORTE_EXCEL.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "27")
                {
                    catorce_REPORTE_RENT_FACT.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "28")
                {
                    quince_REPORTE_MATRIZ.Attributes["style"] = "display:block";
                }

                if (u_ne.Trim() == "29")
                {
                    dieciseis_REPORTE_COMPRAS.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "30")
                {
                    diecisiete_REPORTE_COSTOS_IMPORT.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "32")
                {
                    dieciocho_REPORTE_CATEGORIA.Attributes["style"] = "display:block";
                }

                if (u_ne.Trim() == "33")
                {
                    veinte_REPORTE_STOCKF.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "34")
                {
                    diecinueve_REPORTE_SP.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "35")
                {
                    veinti_uno_Menu_Planificador.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "36")
                {
                    veinti_dos_produc.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "37")
                {
                    veinti_tres_REPORTE_SALDOS_SP.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "38")
                {
                    veinti_cuatro_stock_log.Attributes["style"] = "display:block";
                }
                if (u_ne.Trim() == "47")
                {
                    veinti_cinco_sp_diarias.Attributes["style"] = "display:block";
                }
            }
        }

        private void Menu()
        {

        }
    }
}