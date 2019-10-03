using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ThinxsysFramework;

namespace SoprodiApp
{
    public partial class PDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["A"] == null)
            {
                string id_archivo = Request.QueryString["AR"];
                string file_name = EncontrarArchivo(id_archivo);
                string ruta = ConfigurationManager.AppSettings["RUTA"];

                if (file_name.Contains(".pdf"))
                {
                    TheDownload2(ruta + id_archivo + ".pdf", ruta, file_name);
                }
                else if (file_name.Contains(".jpg"))
                {
                    TheDownload3(ruta + id_archivo + ".jpg", ruta, file_name);
                }
            }
            else
            {
                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                string nom_factura = encriptador.DecryptData(Request.QueryString["Ar"].ToString().Replace(" ", "+").Trim());
                string año_factura = encriptador.DecryptData(Request.QueryString["A"].ToString().Replace(" ", "+").Trim());
                nom_factura = año_factura + "_" + "33_" + nom_factura;
                string ruta = ConfigurationManager.AppSettings["RUTA2"] + año_factura + ConfigurationManager.AppSettings["RUTA3"];
                TheDownload2(ruta + nom_factura.Trim() + ".pdf", ruta, nom_factura.Trim());
            }



        }
        private void TheDownload2(string path, string dlDIR, string filename_)
        {
            path = path.Replace("\\\\", "\\");
            System.IO.FileInfo toDownload = new System.IO.FileInfo(path);
            if (toDownload.Exists)
            {
                path = path.Replace("\\", "//");
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "inline; filename=" + filename_);
                Response.TransmitFile(path);
                Response.Flush();
                Response.End();
            }
            else
            {
                path = path.Replace("_33_", "_61_");
                System.IO.FileInfo toDownload2 = new System.IO.FileInfo(path);
                if (toDownload2.Exists)
                {
                    path = path.Replace("\\", "//");
                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "inline; filename=" + filename_);
                    Response.TransmitFile(path);
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    path = path.Replace("_61_", "_56_");
                    System.IO.FileInfo toDownload3 = new System.IO.FileInfo(path);
                    if (toDownload3.Exists)
                    {
                        path = path.Replace("\\", "//");
                        Response.Clear();
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Disposition", "inline; filename=" + filename_);
                        Response.TransmitFile(path);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        path = path.Replace("_56_", "_46_");
                        System.IO.FileInfo toDownload4 = new System.IO.FileInfo(path);
                        if (toDownload4.Exists)
                        {
                            path = path.Replace("\\", "//");
                            Response.Clear();
                            Response.ClearContent();
                            Response.ClearHeaders();
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("Content-Disposition", "inline; filename=" + filename_);
                            Response.TransmitFile(path);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }
        }

        private void TheDownload22(string path, string dlDIR, string filename_)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "xste", "<script> fuera('"+path+ "') </script>", false);
        }


        private void TheDownload3(string path, string dlDIR, string filename_)
        {
            path = path.Replace("\\\\", "\\");
            System.IO.FileInfo toDownload = new System.IO.FileInfo(path);
            if (toDownload.Exists)
            {
                path = path.Replace("\\", "//");
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "image/jpeg";
                Response.AddHeader("Content-Disposition", "inline; filename=" + filename_);
                Response.TransmitFile(path);
                Response.Flush();
                Response.End();
            }
        }

        public string EncontrarArchivo(string id)
        {
            DBUtil db = new DBUtil();
            string x = "";
            x = db.Scalar("SELECT NOMBRE FROM THX_DOCUMENTOS WHERE ID=" + id).ToString();
            return x;
        }
    }
}