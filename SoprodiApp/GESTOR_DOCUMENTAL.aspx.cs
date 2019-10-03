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
    public partial class GESTOR_DOCUMENTAL : System.Web.UI.Page
    {
        public static string admin = "";
        public static bool puede = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            if (!IsPostBack)
            {
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "15")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }
                DBUtil db = new DBUtil();
                Session["tipo_gestor"] = db.Scalar("SELECT tipo_gestor_documental from USUARIOWEB where cod_usuario = '" + Session["user"].ToString() + "'").ToString();

                P_SubirDocumento.Visible = false;
                P_ListaDocumentos.Visible = false;

                List<string> app = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();

                foreach (string _app in app)
                {
                    //subir
                    if (_app == "97")
                    {
                        P_SubirDocumento.Visible = true;
                        Mostrar_Lista.Visible = false;
                    }
                    //VALIDAR
                    if (_app == "96")
                    {
                        Mostrar_Subir.Visible = false;
                        P_ListaDocumentos.Visible = true;
                    }

                }                
                if (ReporteRNegocio.trae_app(User.Identity.Name).Contains("97") && ReporteRNegocio.trae_app(User.Identity.Name).Contains("96"))
                {
                    Mostrar_Subir.Visible = true;
                    Mostrar_Lista.Visible = true;
                    P_SubirDocumento.Visible = false;
                    P_ListaDocumentos.Visible = true;
                    
                }
                admin = "";
                admin = ReporteRNegocio.esvendedor(User.Identity.Name.ToString());
                puede = false;
                if (admin == "1")
                {
                    puede = true;
                }

                CargarGrilla();
                CargarCombos();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp", "<script language='javascript'>superfiltro();</script>", false);


            }
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp", "<script language='javascript'>superfiltro();</script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp", "<script language='javascript'>CacheItems();</script>", false);
        }

        protected void B_Add_Archivo_Click1(object sender, EventArgs e)
        {
            try
            {
                if (CB_Productos.Text.Contains("-1"))
                {
                    CB_Productos.BorderColor = Color.Red;
                    CB_Productos.Focus();
                    return;
                }             

                if (CB_Laboratorio.Text.Contains("-1"))
                {
                    CB_Laboratorio.BorderColor = Color.Red;
                    CB_Laboratorio.Focus();
                    return;
                }

                if (CB_Analisis.Text.Contains("-1"))
                {
                    CB_Analisis.BorderColor = Color.Red;
                    CB_Analisis.Focus();
                    return;
                }

                if (T_Fecha_analisis.Text == "")
                {
                    T_Fecha_analisis.BorderColor = Color.Red;
                    T_Fecha_analisis.Focus();
                    return;
                }


                DBUtil db = new DBUtil();
                FileUpload FU = new FileUpload();
                FU = FileUpload_Documento;
                string usuario = Session["user"].ToString();

                int cont = FU.PostedFiles.Count;
                int i = 0;
                int scope = 0;
                string status = "";


                while (i <= cont - 1)
                {
                    HttpPostedFile objHttpPostedFile = FU.PostedFiles[i];
                    string extension = objHttpPostedFile.FileName.Substring(objHttpPostedFile.FileName.Length - 4, 4);
                    if (extension == ".pdf")
                    {
                        String file_name = objHttpPostedFile.FileName;

                        scope = Sube_Archivo(file_name, CB_Productos.SelectedValue.ToString(), usuario);

                        if (scope > 0)
                        {
                            string ruta = ConfigurationManager.AppSettings["RUTA"];
                            FU.PostedFiles[i].SaveAs(ruta + scope + ".pdf");

                            if (File.Exists(ruta + scope + ".pdf"))
                            {
                                status += "<tr><td>" + file_name + "</td><td style='color:green'>INGRESADO <i class='fa fa-plus'></i></td></tr>";
                                Sube_Archivo_Log(scope, usuario, "INGRESADO", "ARCHIVO INGRESADO: " + file_name);
                            }
                            else
                            {
                                status += "<tr><td>" + file_name + "</td><td style='color:red'>ERROR <i class='fa fa-exclamation'></i></td></tr>";
                                Sube_Archivo_Log(0, usuario, "ERROR", "NO SE PUDO CARGAR EL ARCHIVO: " + file_name);
                                db.Scalar("delete from THX_DOCUMENTOS WHERE ID = " + scope);
                            }
                        }
                        else
                        {
                            status += "<tr><td>" + file_name + "</td><td style='color:red'>ERROR <i class='fa fa-exclamation'></i></td></tr>";
                            Sube_Archivo_Log(0, usuario, "ERROR", "NO SE PUDO CARGAR EL ARCHIVO: " + file_name);
                        }
                        
                    }
                    else if (extension == ".jpg")
                    {
                        String file_name = objHttpPostedFile.FileName;

                        scope = Sube_Archivo(file_name, CB_Productos.SelectedValue.ToString(), usuario);

                        if (scope > 0)
                        {
                            string ruta = ConfigurationManager.AppSettings["RUTA"];
                            FU.PostedFiles[i].SaveAs(ruta + scope + ".jpg");

                            if (File.Exists(ruta + scope + ".jpg"))
                            {
                                status += "<tr><td>" + file_name + "</td><td style='color:green'>INGRESADO <i class='fa fa-plus'></i></td></tr>";
                                Sube_Archivo_Log(scope, usuario, "INGRESADO", "ARCHIVO INGRESADO: " + file_name);
                            }
                            else
                            {
                                status += "<tr><td>" + file_name + "</td><td style='color:red'>ERROR <i class='fa fa-exclamation'></i></td></tr>";
                                Sube_Archivo_Log(0, usuario, "ERROR", "NO SE PUDO CARGAR EL ARCHIVO: " + file_name);
                                db.Scalar("delete from THX_DOCUMENTOS WHERE ID = " + scope);
                            }
                        }
                        else
                        {
                            status += "<tr><td>" + file_name + "</td><td style='color:red'>ERROR <i class='fa fa-exclamation'></i></td></tr>";
                            Sube_Archivo_Log(0, usuario, "ERROR", "NO SE PUDO CARGAR EL ARCHIVO: " + file_name);
                        }

                    }
                    else
                    {
                        status += "<tr><td>" + objHttpPostedFile.FileName + "</td><td style='color:red'>ERROR <i class='fa fa-exclamation'></i></td></tr>";
                        Sube_Archivo_Log(0, usuario, "ERROR", "NO SE PUDO CARGAR EL ARCHIVO: " + objHttpPostedFile.FileName);
                    }
                    i++;
                }
                if (status != "")
                {
                    string tabla = "";
                    tabla += "<table class='table table-hover fill-head' style='width:100%'>";
                    tabla += "<tr><th><b>Archivo</b></th><th><b>Estado</b></th></tr>";
                    tabla += status;
                    tabla += "</table>";
                    D_status.InnerHtml = tabla;
                }

            }
            catch (Exception ex)
            {

            }
        }
        protected void G_Documentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {      
                if (e.CommandName == "Visualizar")
                {
                    string archivo = (G_Documentos.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp123", "<script language='javascript'>fuera('" + archivo + "');</script>", false);
                }
                if (e.CommandName == "Descargar")
                {
                    string archivo = (G_Documentos.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                    string file_name = EncontrarArchivo(archivo);
                    if (file_name != "")
                    {
                        string ruta = ConfigurationManager.AppSettings["RUTA"];
                        if (file_name.Contains(".pdf"))
                        {
                            TheDownload(ruta + archivo + ".pdf", file_name);
                        }
                        else if (file_name.Contains(".jpg"))
                        {
                            TheDownload(ruta + archivo + ".jpg", file_name);
                        }
                        
                    }
                }
                if (e.CommandName == "Aprobar")
                {
                    string usuario = Session["user"].ToString();
                    Cambiar_Estado_Documento(G_Documentos.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString(), "ACTIVO", usuario, "ARCHIVO ACTIVADO POR : " + usuario);
                    CargarGrilla();
                }
                if (e.CommandName == "Eliminar")
                {
                    string usuario = Session["user"].ToString();
                    Cambiar_Estado_Documento(G_Documentos.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString(), "DESACTIVADO", usuario, "ARCHIVO DESACTIVADO POR : " + usuario);
                    CargarGrilla();
                }
                if (e.CommandName == "Borrar")
                {
                    string usuario = Session["user"].ToString();
                    string archivo = (G_Documentos.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                    string file_name = EncontrarArchivo(archivo);
                    string ruta = ConfigurationManager.AppSettings["RUTA"];
                    DBUtil db = new DBUtil();
                    db.Scalar("update THX_DOCUMENTOS set estado = 'ELIMINADO' where id = " + archivo);
                    Sube_Archivo_Log(Convert.ToInt32(archivo), usuario, "ELIMINADO", "ARCHIVO ELIMINADO : " + file_name);
                    if (file_name.Contains(".pdf"))
                    {
                        TheDelete(ruta + archivo + ".pdf");
                    }
                    else if (file_name.Contains(".jpg"))
                    {
                        TheDelete(ruta + archivo + ".jpg");
                    }                    
                    CargarGrilla();

                }
            }

            catch (Exception ex)
            {

            }
        }
        protected void G_Documentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {           
                string estado = G_Documentos.DataKeys[e.Row.RowIndex].Values[0].ToString();
                if (estado == "INGRESADO")
                {
                    e.Row.Cells[5].ForeColor = Color.Blue;
                }
                if (estado == "ACTIVO")
                {
                    e.Row.Cells[5].ForeColor = Color.Green;
                }
                if (estado == "DESACTIVADO")
                {
                    e.Row.Cells[5].ForeColor = Color.Red;
                }
            }
        }

        public int Sube_Archivo(string nombre_archivo, string codigo_producto, string usuario)
        {
            DBUtil db = new DBUtil();
            int resp = 0;
            string query = "";

            try
            {
                query += "INSERT INTO THX_DOCUMENTOS ( ";
                query += "FECHA , ";
                query += "NOMBRE , ";
                query += "CODIGO_PRODUCTO , ";
                query += "USUARIO , ";
                query += "ESTADO , ";
                query += "FECHA_ANALISIS , ";
                query += "ID_LAB , ";
                query += "ID_ANALISIS  ";

                query += ") VALUES ( ";
                query += " @FECHA , ";
                query += " @NOMBRE, ";
                query += " @CODIGO_PRODUCTO , ";
                query += " @USUARIO , ";
                query += " @ESTADO , ";
                query += " CONVERT(datetime,@FECHA_ANALISIS, 103) , ";
                query += " @ID_LAB , ";
                query += " @ID_ANALISIS ";
                query += " ); SELECT SCOPE_IDENTITY();";

                List<SPVars> vars = new List<SPVars>();
                vars.Add(new SPVars() { nombre = "FECHA", valor = DateTime.Now });
                vars.Add(new SPVars() { nombre = "NOMBRE", valor = nombre_archivo });
                vars.Add(new SPVars() { nombre = "CODIGO_PRODUCTO", valor = codigo_producto });
                vars.Add(new SPVars() { nombre = "USUARIO", valor = usuario });
                vars.Add(new SPVars() { nombre = "ESTADO", valor = "INGRESADO" });
                vars.Add(new SPVars() { nombre = "FECHA_ANALISIS", valor = T_Fecha_analisis.Text.ToString() });
                vars.Add(new SPVars() { nombre = "ID_LAB", valor = CB_Laboratorio.SelectedValue.ToString() });
                vars.Add(new SPVars() { nombre = "ID_ANALISIS", valor = CB_Analisis.SelectedValue.ToString() });

                resp = Convert.ToInt32(db.Scalar2(query, vars));
                return resp;
            }
            catch (Exception e)
            {
                resp = 0;
            }
            return resp;
        }
        public void Sube_Archivo_Log(int id, string usuario, string estado, string observacion)
        {
            DBUtil db = new DBUtil();
            string query = "";
            try
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

                List<SPVars> vars = new List<SPVars>();
                vars.Add(new SPVars() { nombre = "ID_DOCUMENTO", valor = id });
                vars.Add(new SPVars() { nombre = "FECHA", valor = DateTime.Now });
                vars.Add(new SPVars() { nombre = "USUARIO", valor = usuario });
                vars.Add(new SPVars() { nombre = "ESTADO", valor = estado });
                vars.Add(new SPVars() { nombre = "OBSERVACION", valor = observacion });

                db.Scalar2(query, vars);

            }
            catch (Exception e)
            {

            }

        }

        public bool Cambiar_Estado_Documento(string id, string estado, string usuario, string observacion)
        {
            DBUtil db = new DBUtil();
            int x = 0;
            bool resp = true;
            string query = "";

            try
            {
                x = Convert.ToInt32(db.Scalar("Select count(1) from THX_DOCUMENTOS where id = " + id));
                if (x > 0)
                {
                    query += "UPDATE THX_DOCUMENTOS ";
                    query += "SET ESTADO = @ESTADO, USUARIO = @USUARIO ";
                    query += "WHERE ID = @ID ";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "ESTADO", valor = estado });
                    vars.Add(new SPVars() { nombre = "USUARIO", valor = usuario });
                    vars.Add(new SPVars() { nombre = "ID", valor = id });
                    db.Scalar2(query, vars);

                    query = "";
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
                    vars2.Add(new SPVars() { nombre = "ESTADO", valor = estado });
                    vars2.Add(new SPVars() { nombre = "OBSERVACION", valor = observacion });

                    db.Scalar2(query, vars2);
                    return resp;
                }
            }
            catch (Exception e)
            {
                resp = false;
            }
            return resp;
        }
        public void CargarGrilla()
        {
            DBUtil db = new DBUtil();
            string query = "";
            query += " SELECT D.ID, D.FECHA, D.NOMBRE, rtrim(ltrim(I.InvtID)) + CAST(' - ' AS varchar(MAX)) + I.descr AS CODIGO_PRODUCTO, ";
            query += " D.USUARIO, D.ESTADO, ll.nom_lab as LABORATORIO, aa.nom_analisis as TIPO_ANALISIS, D.fecha_analisis as FECHA_ANALISIS ";
            query += " FROM THX_DOCUMENTOS D ";
            query += " INNER JOIN [192.168.10.11].SoprodiUSDapp.dbo.INVENTORY I  ";
            query += " ON D.CODIGO_PRODUCTO = I.INVTID  ";
            query += " LEFT JOIN THX_Analisis aa ON D.id_analisis = aa.id ";
            query += " LEFT JOIN THX_Laboratorios ll ON D.id_lab = ll.id ";
            query += " WHERE D.ESTADO <> 'ELIMINADO' ORDER BY D.ID DESC ";

            G_Documentos.DataSource = db.consultar(query);
            G_Documentos.DataBind();
            if (puede)
            {
                G_Documentos.Columns[10].Visible = true;
            }
            else
            {
                G_Documentos.Columns[10].Visible = false;
            }
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp", "<script language='javascript'>superfiltro();</script>", false);
        }
        public void CargarCombos()
        {
            Utiles util = new Utiles();
            DBUtil db = new DBUtil();
            DataTable dt = db.consultar("select INV.InvtID, rtrim(ltrim(INV.InvtID)) + CAST(' - ' AS varchar(MAX)) + INV.descr as descr from [192.168.10.11].SoprodiUSDapp.dbo.Inventory INV INNER JOIN THX_PRODUCTOS_ACTIVOS PA on PA.cod_producto = INV.InvtID where INV.InvtID BETWEEN '0000' AND '0999' and PA.activo = 1 order by descr ");

            dt.Rows.Add(new Object[] { -1, "-- Seleccione --" });        
            CB_Productos.DataSource = dt;
            CB_Productos.DataTextField = "descr";
            CB_Productos.DataValueField = "InvtID";
            CB_Productos.DataBind();
            CB_Productos.SelectedValue = "-1";
            CB_Productos.Enabled = true;

            DataTable dt1 = db.consultar("select id, rtrim(ltrim(cod_analisis)) + CAST(' - ' AS varchar(MAX)) + nom_analisis as nombre_analisis from THX_Analisis");

            dt1.Rows.Add(new Object[] { -1, "-- Seleccione --" });
            CB_Analisis.DataSource = dt1;
            CB_Analisis.DataTextField = "nombre_analisis";
            CB_Analisis.DataValueField = "id";
            CB_Analisis.DataBind();
            CB_Analisis.SelectedValue = "-1";
            CB_Analisis.Enabled = true;

            DataTable dt2 = db.consultar("select id, rtrim(ltrim(cod_lab)) + CAST(' - ' AS varchar(MAX)) + nom_lab as nombre_lab from THX_LABORATORIOS");

            dt2.Rows.Add(new Object[] { -1, "-- Seleccione --" });
            CB_Laboratorio.DataSource = dt2;
            CB_Laboratorio.DataTextField = "nombre_lab";
            CB_Laboratorio.DataValueField = "id";
            CB_Laboratorio.DataBind();
            CB_Laboratorio.SelectedValue = "-1";
            CB_Laboratorio.Enabled = true;
        }
        public string EncontrarArchivo(string id)
        {
            DBUtil db = new DBUtil();
            string x = "";

            x = db.Scalar("SELECT NOMBRE FROM THX_DOCUMENTOS WHERE ID=" + id).ToString();
            return x;
        }

        protected void alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "myScript", "<script>javascript:Mensaje('" + mensaje + "');</script>", false);
        }
        protected void AbrirPestaña(string url)
        {
            url = "file:///" + url.Replace("\\", "//");
            //ScriptManager.RegisterStartupScript(this, typeof(Page), "myScript", "<script>javascript:AbrirURL('" + url + "');</script>", false);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>AbrirURL('" + url + "');</script>", false);
        }
        protected void Mostrar_Subir_Click(object sender, EventArgs e)
        {
            Utiles util = new Utiles();
            P_SubirDocumento.Visible = true;
            D_status.InnerHtml = "";
            T_FiltraCombo.Text = "";
            CB_Productos.SelectedValue = "-1";
            P_ListaDocumentos.Visible = false;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp", "<script language='javascript'>CacheItems();</script>", false);
        }
        protected void Mostrar_Lista_Click(object sender, EventArgs e)
        {
            P_SubirDocumento.Visible = false;
            CargarGrilla();
            P_ListaDocumentos.Visible = true;
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
    }
}