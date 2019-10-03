using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoprodiApp.negocio;
using SoprodiApp.entidad;
using System.Drawing;
using Microsoft.Office.Interop;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.Services;
using ThinxsysFramework;
using System.Configuration;

namespace SoprodiApp
{
    public partial class VISOR_DOCUMENTAL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            if (!IsPostBack)
            {
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "14")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }

                CargarGrilla();               
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp", "<script language='javascript'>superfiltro();</script>", false);
            }  
        }
        protected void G_Documentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Visualizar")
                {
                    string archivo = (G_Documentos.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    InsertaLog(G_Documentos.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString(), "VISTA");
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp34321", "<script language='javascript'>fuera('" + archivo + "');</script>", false);                  

                }
                if (e.CommandName == "Descargar")
                {
                    string archivo = (G_Documentos.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    string file_name = EncontrarArchivo(archivo);
                    if (file_name != "")
                    {
                        string ruta = ConfigurationManager.AppSettings["RUTA"];
                        InsertaLog(G_Documentos.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString(), "DESCARGA");
                        
                        if (file_name.Contains(".pdf"))
                        {
                            TheDownload(ruta + archivo + ".pdf", file_name);
                        }
                        else if (file_name.Contains(".jpg"))
                        {
                            TheDownload(ruta + archivo + ".jpg", file_name);
                        }                        
                        // EN EL THEDOWNLOAD SE CAE AL CATCH POR ESO INSERTO EN EL LOG PRIMERO
                    }
                }                
            }

            catch (Exception ex)
            {

            }
        }
        protected void AbrirPestaña(string url)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "myScript", "<script>javascript:AbrirURL('" + url + "');</script>", false);
        }
        public void TheDownload(string path, string filename_)
        {
            try
            {
                path = path.Replace("\\\\", "\\");
                System.IO.FileInfo toDownload = new System.IO.FileInfo(path);
                if (toDownload.Exists)
                {

                    Response.Clear();
                    if (filename_.Contains(" "))
                    {

                        filename_ = filename_.Replace(" ", "_");
                    }

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filename_);
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
        public string EncontrarArchivo(string id)
        {
            DBUtil db = new DBUtil();
            string x = "";

            x = db.Scalar("SELECT NOMBRE FROM THX_DOCUMENTOS WHERE ID=" + id).ToString();
            return x;
        }
        public void InsertaLog(string id, string observacion)
        {
            DBUtil db = new DBUtil();
            int x = 0;           
            string query = "";
            string usuario = Session["user"].ToString();

            try
            {
                x = Convert.ToInt32(db.Scalar("Select count(1) from THX_DOCUMENTOS where id = " + id));
                if (x > 0)
                {   
                    query += "INSERT INTO THX_Documento_Log_Visualizador ( ";
                    query += "ID_DOCUMENTO , ";
                    query += "FECHA , ";
                    query += "USUARIO, ";
                    query += "ESTADO, ";    
                    query += "OBSERVACION ";

                    query += ") VALUES ( ";
                    query += " @ID_DOCUMENTO , ";
                    query += " @FECHA, ";
                    query += " @USUARIO, ";
                    query += " @ESTADO, ";  
                    query += " @OBSERVACION ";
                    query += " );";

                    List<SPVars> vars2 = new List<SPVars>();
                    vars2.Add(new SPVars() { nombre = "ID_DOCUMENTO", valor = id });
                    vars2.Add(new SPVars() { nombre = "FECHA", valor = DateTime.Now });
                    vars2.Add(new SPVars() { nombre = "USUARIO", valor = usuario });
                    vars2.Add(new SPVars() { nombre = "ESTADO", valor = "ACTIVO" });    
                    vars2.Add(new SPVars() { nombre = "OBSERVACION", valor = observacion });

                    db.Scalar2(query, vars2);                  
                }
            }
            catch (Exception e)
            {
               
            }
      
        }
        public void CargarGrilla()
        {
            DBUtil db = new DBUtil();
            string query = "";
            string usuario = Session["user"].ToString();

            query += "SELECT D.ID, D.FECHA, D.NOMBRE, rtrim(ltrim(I.InvtID)) + CAST(' - ' AS varchar(MAX)) + I.descr AS CODIGO_PRODUCTO, D.USUARIO, ll.nom_lab as LABORATORIO, aa.nom_analisis as TIPO_ANALISIS, D.fecha_analisis as FECHA_ANALISIS FROM THX_DOCUMENTOS D ";
            query += " INNER JOIN THX_USUARIO_PRODUCTO P ";
            query += " ON D.CODIGO_PRODUCTO = P.COD_PRODUCTO ";
            query += " INNER JOIN [192.168.10.11].SoprodiUSDapp.dbo.INVENTORY I ";
            query += " ON D.CODIGO_PRODUCTO = I.INVTID ";
            query += " LEFT JOIN THX_Analisis aa ON D.id_analisis = aa.id ";
            query += " LEFT JOIN THX_Laboratorios ll ON D.id_lab = ll.id ";
            query += " WHERE P.COD_USUARIO = '" + usuario + "' ";
            query += " AND D.ESTADO = 'ACTIVO' ORDER BY D.FECHA "; 

            G_Documentos.DataSource = db.consultar(query);
            G_Documentos.DataBind();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp", "<script language='javascript'>superfiltro();</script>", false);
        }
    }
}