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

namespace SoprodiApp
{
    public partial class USUARIO_PRODUCTO : System.Web.UI.Page
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
                    if (u_ne.Trim() == "13")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }


                CargarComboUsuario();
                CargarComboProducto();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp", "<script language='javascript'>superfiltro();</script>", false);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp", "<script language='javascript'>CacheItems();</script>", false);
            }
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp", "<script language='javascript'>CacheItems();</script>", false);
        }
        public void CargarGrilla()
        {
            string usuario = CB_USUARIO.SelectedValue.ToString();
            DBUtil db = new DBUtil();
            string query = "";
            query += "SELECT I.InvtID, I.DESCR from THX_USUARIO_PRODUCTO P INNER JOIN ";
            query += "[192.168.10.11].SoprodiUSDapp.dbo.INVENTORY I ON P.COD_PRODUCTO = I.INVTID ";
            query += "WHERE P.COD_USUARIO = '" + usuario + "' ORDER BY I.DESCR";
            G_Documentos.DataSource = db.consultar(query);
            G_Documentos.DataBind();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp", "<script language='javascript'>superfiltro();</script>", false);
        }

        protected void B_CargaUsuario_Click(object sender, EventArgs e)
        {
            if (CB_USUARIO.SelectedValue != "-1")
            {
                DIV_GRILLA.Visible = true;
                DIV_PRODUCTOS.Visible = true;
                T_FiltraCombo.Text = "";
                CB_Productos.SelectedValue = "-2";

                string usuario = CB_USUARIO.SelectedValue.ToString();
                L_USUARIO.InnerHtml = "<h3>Usuario : " + usuario + "</h3>";
                CargarGrilla();
            }

        }
        protected void B_AgregaProducto_Click(object sender, EventArgs e)
        {
            try
            {
                L_ERROR.InnerHtml = "";
                if (CB_Productos.SelectedValue != "-1")
                {
                    // TODOS
                    if (CB_Productos.SelectedValue == "-2")
                    {
                        string usuario = CB_USUARIO.SelectedValue.ToString();
                        string producto = CB_Productos.SelectedValue.ToString();
                        DBUtil db = new DBUtil();
                        string query = "INSERT INTO THX_USUARIO_PRODUCTO (cod_usuario, cod_producto) select '" + usuario + "', cod_producto from THX_PRODUCTOS_ACTIVOS where activo = 1";
                        object result = db.Scalar(query);
                    }
                    else
                    {
                        string usuario = CB_USUARIO.SelectedValue.ToString();
                        string producto = CB_Productos.SelectedValue.ToString();
                        DBUtil db = new DBUtil();
                        string query = "INSERT INTO THX_USUARIO_PRODUCTO VALUES ('" + usuario + "', '" + producto + "')";
                        object result = db.Scalar(query);
                    }
                    
                    CargarGrilla();
                }
            }
            catch (Exception ex)
            {
                string usuario = CB_USUARIO.SelectedValue.ToString();
                string producto = CB_Productos.SelectedValue.ToString();
                L_ERROR.InnerHtml = "<h4 style='color:red'>Ya existe " + producto + " para : " + usuario + "</h4>";
            }

        }
        public void CargarComboProducto()
        {
            Utiles util = new Utiles();
            DBUtil db = new DBUtil();
            DataTable dt = db.consultar("select INV.InvtID, rtrim(ltrim(INV.InvtID)) + CAST(' - ' AS varchar(MAX)) + INV.descr as descr from [192.168.10.11].SoprodiUSDapp.dbo.Inventory INV INNER JOIN THX_PRODUCTOS_ACTIVOS PA on PA.cod_producto = INV.InvtID where INV.InvtID BETWEEN '0000' AND '0999' and PA.activo = 1 order by descr ");

            dt.Rows.Add(new Object[] { -1, "-- Seleccione --" });
            dt.Rows.Add(new Object[] { -2, "-- Todos --" });
            CB_Productos.DataSource = dt;
            CB_Productos.DataTextField = "descr";
            CB_Productos.DataValueField = "InvtID";
            CB_Productos.DataBind();
            CB_Productos.SelectedValue = "-2";
            CB_Productos.Enabled = true;
            //util.CargarCombo(CB_Productos, "[192.168.10.11].SoprodiUSDapp.dbo.Inventory", "InvtID", "rtrim(ltrim(InvtID)) + CAST(' - ' AS varchar(MAX)) + descr as descr", "InvtID BETWEEN '0000' AND '0999' order by descr ");
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "temp", "<script language='javascript'>CacheItems();</script>", false);
        }
        public void CargarComboUsuario()
        {
            //Utiles util = new Utiles();
            //util.CargarCombo(CB_USUARIO, "UsuarioWeb", "cod_usuario", "cod_usuario", " 1=1 order by cod_usuario");
            //CB_USUARIO.SelectedIndex = 1;

            // GESTOR DOCUMENTAL
            DBUtil db = new DBUtil();
            DataTable dt = new DataTable();
            dt = db.consultar("Select cod_usuario, cod_usuario as nombre_usuario from UsuarioWeb order by cod_usuario");
            CB_USUARIO.DataSource = dt;
            CB_USUARIO.DataValueField = "cod_usuario";
            CB_USUARIO.DataTextField = "nombre_usuario";
            CB_USUARIO.DataBind();
            CB_USUARIO.SelectedValue = "0";
        }

        protected void G_Documentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Eliminar")
                {
                    string usuario = CB_USUARIO.SelectedValue.ToString();
                    string producto = G_Documentos.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                    DBUtil db = new DBUtil();
                    string query = "DELETE FROM THX_USUARIO_PRODUCTO WHERE COD_USUARIO = '" + usuario + "' AND COD_PRODUCTO = '" + producto + "'";
                    db.Scalar(query);
                    CargarGrilla();
                }
            }

            catch (Exception ex)
            {

            }
        }

        protected void CB_USUARIO_TextChanged(object sender, EventArgs e)
        {
            DIV_GRILLA.Visible = false;
            DIV_PRODUCTOS.Visible = false;
        }
    }
}