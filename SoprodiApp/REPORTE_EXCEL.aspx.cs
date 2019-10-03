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
using System.Data.OleDb;

using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;

namespace SoprodiApp
{
    public partial class REPORTE_EXCEL : System.Web.UI.Page
    {
        public static int cont_periodos;
        public static DataTable aux;
        public static DataTable productos;
        public static DataTable totales;
        public static string PERIODOS;
        private static string USER;
        private static string where = " where 1=1 ";
        private static Page page;

        public static bool header_sum = true;
        public static bool header_sum2 = true;
        private static string vendedor;
        private static string cliente;
        public static int cont_det;
        public static bool permiso;

        public String fecha_g { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.RegisterRedirectOnSessionEndScript();
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            page = this.Page;
            if (!IsPostBack)
            {
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "26")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }

                if (Session["SW_PERMI"].ToString() == "1")
                {

                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=1'>Abarrotes</a>";
                    titulo2.HRef = "reportes.aspx?s=1";
                    titulo2.InnerText = "Abarrotes";
                }
                else if (Session["SW_PERMI"].ToString() == "2")
                {
                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=2'>Granos</a>";
                    titulo2.HRef = "reportes.aspx?s=2";
                    titulo2.InnerText = "Granos";

                }

                USER = User.Identity.Name.ToString();
                l_usuario_.Text = USER;
                //DateTime t = DateTime.Now;
                //DateTime t2 = DateTime.Now;
                ////////t = new DateTime(t.Year, t.Month - 6, 1);               
                //txt_desde.Text = ReporteRNegocio.CALCULA_DESDE(t.Month, t.Year);
                //txt_hasta.Text = t2.ToShortDateString();

                string es_vendedor = ReporteRNegocio.esvendedor(USER);
                if (es_vendedor == "2")
                {

                }
                else
                {

                }
            }
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


        protected void B_Add_Archivo_Click(object sender, EventArgs e)
        {
            if (FileUpload_Documento.HasFile && (Path.GetExtension(FileUpload_Documento.FileName) == ".xls" || Path.GetExtension(FileUpload_Documento.FileName) == ".xlsx"))
            {


                HttpPostedFile objHttpPostedFile = FileUpload_Documento.PostedFile;
                String file_name = objHttpPostedFile.FileName;

                string ruta = ConfigurationManager.AppSettings["RUTA_EXCEL"];
                FileUpload_Documento.PostedFile.SaveAs(ruta + file_name);
                bool exit = Import_To_Grid(ruta + file_name, Path.GetExtension(FileUpload_Documento.FileName), "Yes");
                if (exit)
                {
                    TheDelete(ruta + file_name);
                }
            }
            else
            {
                //UploadStatusLabel.Text = "You did not specify a file to upload.";
            }
        }

        public void TheDelete(string path)
        {
            try
            {
                path = path.Replace("\\\\", "\\");
                System.IO.FileInfo toDownload = new System.IO.FileInfo(path);
                if (toDownload.Exists)
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {

            }
        }


        public class excel
        {
            public double c_f { get; set; }
            public double bod_usd { get; set; }
            public double cm_stgo { get; set; }
            public double cm_qta { get; set; }
            public double arica { get; set; }
            public String cod_producto { get; set; }
            public String producto { get; set; }
            public String pack { get; set; }
            public int cajas_pallet { get; set; }
            public int pallets_camion { get; set; }
            public int camio_v_metropol { get; set; }
            public int int_out_cm_qt_lv { get; set; }
            public int camio_has_viii { get; set; }
            public int quillota { get; set; }
            public int bod_sn_anton { get; set; }
            public int arica_ { get; set; }
            public int arica_usd { get; set; }
            public String fecha { get; set; }

        }

        private bool Import_To_Grid(string FilePath, string Extension, string isHDR)
        {
            string a = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
            string b = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
            string conStr = "";
            string sheetName;
            DataTable dt = new DataTable();
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = string.Format(a, FilePath, isHDR);
                    break;
                case ".xlsx": //Excel 07
                    conStr = string.Format(b, FilePath, isHDR);
                    break;
            }
            //Get the name of the First Sheet.
            using (OleDbConnection con2 = new OleDbConnection(conStr))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    cmd.Connection = con2;
                    con2.Open();
                    DataTable dtExcelSchema = con2.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                    con2.Close();
                }
            }

            //Read Data from the First Sheet.
            using (OleDbConnection con1 = new OleDbConnection(conStr))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    using (OleDbDataAdapter oda = new OleDbDataAdapter())
                    {

                        cmd.CommandText = "SELECT * From [" + sheetName + "]";
                        cmd.Connection = con1;
                        con1.Open();
                        oda.SelectCommand = cmd;
                        oda.Fill(dt);
                        con1.Close();

                    }
                }
            }
            int count_row = 0;
            bool error_insert = false;
            foreach (DataRow r in dt.Rows)
            {
                excel ex = new excel();
      
                count_row++;
                if (count_row == 1)
                {
                    
                    ex.fecha = fecha_g = r[1].ToString(); 
                }
                if (count_row >= 6 && r[6].ToString() != "")
                {
                    ex.fecha = fecha_g;
                    try { ex.c_f = Convert.ToDouble(r[1].ToString().Replace(".", "")); } catch { ex.c_f = 0; }
                    try { ex.bod_usd = Convert.ToDouble(r[2].ToString().Replace(".", "")); } catch { ex.bod_usd = 0; }
                    try { ex.cm_stgo = Convert.ToDouble(r[3].ToString().Replace(".", "")); } catch { ex.cm_stgo = 0; }
                    try { ex.cm_qta = Convert.ToDouble(r[4].ToString().Replace(".", "")); } catch { ex.cm_qta = 0; }
                    try { ex.arica = Convert.ToDouble(r[5].ToString().Replace(".", "")); } catch { ex.arica = 0; }
                    try { ex.cod_producto = r[6].ToString(); } catch { ex.cod_producto = ""; }
                    try { ex.producto = r[7].ToString(); } catch { ex.producto = ""; }
                    try { ex.pack = r[8].ToString(); } catch { ex.pack = ""; }
                    try { ex.cajas_pallet = Convert.ToInt32(r[9].ToString().Replace(".", "")); } catch { ex.cajas_pallet = 0; }
                    try { ex.pallets_camion = Convert.ToInt32(r[10].ToString().Replace(".", "")); } catch { ex.pallets_camion = 0; }
                    try { ex.camio_v_metropol = Convert.ToInt32(r[11].ToString().Replace(".", "")); } catch { ex.camio_v_metropol = 0; }
                    try { ex.int_out_cm_qt_lv = Convert.ToInt32(r[12].ToString().Replace(".", "")); } catch { ex.int_out_cm_qt_lv = 0; }
                    try { ex.camio_has_viii = Convert.ToInt32(r[13].ToString().Replace(".", "")); } catch { ex.camio_has_viii = 0; }
                    try { ex.quillota = Convert.ToInt32(r[14].ToString().Replace(".", "")); } catch { ex.quillota = 0; }
                    try { ex.bod_sn_anton = Convert.ToInt32(r[15].ToString().Replace(".", "")); } catch { ex.bod_sn_anton = 0; }
                    try { ex.arica_ = Convert.ToInt32(r[16].ToString().Replace(".", "")); } catch { ex.arica_ = 0; }
                    try { ex.arica_usd = Convert.ToInt32(r[17].ToString().Replace(".", "")); } catch { ex.arica_usd = 0; }

                    //string estado = ReporteRNegocio.insert_datos_excel(ex);
                    //if (estado != "OK")
                    //{

                    //    error_insert = true;
                    //    break;
                    //}

                }


            }

            if (error_insert) { l_mensaje.Text = "Error al cargar Excel"; }
            else {
                l_mensaje.Text = "Excel cargado!"; }

            return true;
        }
    }
}