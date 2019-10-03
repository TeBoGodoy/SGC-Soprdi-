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
    public partial class COMISIONES_REGLA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                //bool correcto_app = false;
                //foreach (string u_ne in u_negocio)
                //{
                //    if (u_ne.Trim() == "19")
                //    {
                //        correcto_app = true;
                //    }
                //}
                //if (!correcto_app)
                //{
                //    Response.Redirect("MENU.aspx");
                //}

                CargarGrilla();
            }
        }

        protected void btn_nuevo_banco_ServerClick(object sender, EventArgs e)
        {
            B_Guardar.Text = "Crear";
            B_Guardar.Visible = true;


            T_COD_COMISION.Text = String.Empty;
            T_NOM_COMISION.Text = String.Empty;

            T_COD_COMISION.Enabled = true;
            T_NOM_COMISION.Enabled = true;
            btn_nuevo_banco.Visible = false;
        }

        protected void B_Guardar_Click(object sender, EventArgs e)
        {
            if (B_Guardar.Text == "Crear")
            {
                if (T_COD_COMISION.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab1", "<script language='javascript'>alert('El Codigo de la regla no puede estar vacío');</script>", false);
                }
                else if (T_NOM_COMISION.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El Nombre de la regla no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    string usuario = Session["user"].ToString();
                    string query = "";

                    query += "INSERT INTO [Comision_Regla] ( ";
                    query += "COD_COMISION , ";
                    query += "NOMBRE_COMISION  ";


                    query += ") VALUES ( ";
                    query += " @COD_COMISION , ";
                    query += " @NOMBRE_COMISION ";
                    query += " );";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "COD_COMISION", valor = T_COD_COMISION.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "NOMBRE_COMISION", valor = T_NOM_COMISION.Text.ToString().Trim() });

                    db.Scalar2(query, vars);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    T_COD_COMISION.Text = String.Empty;
                    T_NOM_COMISION.Text = String.Empty;
                    T_COD_COMISION.Enabled = false;
                    T_NOM_COMISION.Enabled = false;

                }
            }

            else if (B_Guardar.Text == "Modificar")
            {
                if (T_COD_COMISION.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab3", "<script language='javascript'>alert('El Codigo de la regla no puede estar vacío');</script>", false);
                }
                else if (T_NOM_COMISION.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab4", "<script language='javascript'>alert('El Nombre de la regla no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    string usuario = Session["user"].ToString();
                    string query = "";

                    query += "UPDATE [Comision_Regla] SET ";
                    query += "COD_COMISION = @COD_COMISION ,";
                    query += "NOMBRE_COMISION = @NOMBRE_COMISION  ";
                    query += " WHERE ID = @ID";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "COD_COMISION", valor = T_COD_COMISION.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "NOMBRE_COMISION", valor = T_NOM_COMISION.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "ID", valor = T_ID_REGLA.Text.ToString().Trim() });

                    db.Scalar2(query, vars);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    T_COD_COMISION.Text = String.Empty;
                    T_NOM_COMISION.Text = String.Empty;
                    T_COD_COMISION.Enabled = false;
                    T_NOM_COMISION.Enabled = false;

                }
            }
        }

        protected void G_Banco_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Eliminar")
                {
                    string id = (G_Banco.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    DBUtil db = new DBUtil();

                    db.Scalar("delete from Comision_Regla where id = " + id);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    T_COD_COMISION.Text = String.Empty;
                    T_NOM_COMISION.Text = String.Empty;
                    T_COD_COMISION.Enabled = false;
                    T_NOM_COMISION.Enabled = false;

                }
                if (e.CommandName == "Editar")
                {
                    string id = (G_Banco.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    B_Guardar.Text = "Modificar";
                    B_Guardar.Visible = true;
                    btn_nuevo_banco.Visible = true;
                    T_ID_REGLA.Text = id;

                    DBUtil db = new DBUtil();
                    DataTable dt = new DataTable();

                    dt = db.consultar("select COD_COMISION, NOMBRE_COMISION from Comision_Regla where id = " + id);
                    if (dt != null)
                    {
                        DataRow dr = dt.Rows[0];
                        T_COD_COMISION.Text = dr[0].ToString();
                        T_NOM_COMISION.Text = dr[1].ToString();
                        T_COD_COMISION.Enabled = true;
                        T_NOM_COMISION.Enabled = true;
                    }
                }
            }

            catch (Exception ex)
            {

            }
        }

        public void CargarGrilla()
        {
            DBUtil db = new DBUtil();
            G_Banco.DataSource = db.consultar("Select * from [dbo].[Comision_Regla]");
            G_Banco.DataBind();
        }
    }
}