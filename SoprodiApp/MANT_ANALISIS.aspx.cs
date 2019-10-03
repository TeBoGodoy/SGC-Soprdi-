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
    public partial class MANT_ANALISIS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "20")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }
                CargarGrilla();                
            }
        }

        protected void B_Guardar_Click(object sender, EventArgs e)
        {
            if (B_Guardar.Text == "Crear")
            {
                if (T_Cod_Analisis.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_AN1", "<script language='javascript'>alert('El Codigo del Laboratorio no puede estar vacío');</script>", false);
                }
                else if (T_Nom_Analisis.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_AN2", "<script language='javascript'>alert('El Nombre del Analisis no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    string usuario = Session["user"].ToString();
                    string query = "";

                    query += "INSERT INTO THX_ANALISIS ( ";
                    query += "COD_ANALISIS , ";
                    query += "NOM_ANALISIS , ";
                    query += "USUARIO , ";
                    query += "FECHA_CREACION ";

                    query += ") VALUES ( ";
                    query += " @COD_ANALISIS , ";
                    query += " @NOM_ANALISIS, ";
                    query += " @USUARIO , ";
                    query += " @FECHA_CREACION ";
                    query += " );";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "COD_ANALISIS", valor = T_Cod_Analisis.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "NOM_ANALISIS", valor = T_Nom_Analisis.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "USUARIO", valor = usuario });
                    vars.Add(new SPVars() { nombre = "FECHA_CREACION", valor = DateTime.Now });

                    db.Scalar2(query, vars);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_Analisis.Visible = true;
                    T_Cod_Analisis.Text = String.Empty;
                    T_Nom_Analisis.Text = String.Empty;
                    T_Cod_Analisis.Enabled = false;
                    T_Nom_Analisis.Enabled = false;

                }
            }

            else if (B_Guardar.Text == "Modificar")
            {
                if (T_Cod_Analisis.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_AN3", "<script language='javascript'>alert('El Codigo del Analisis no puede estar vacío');</script>", false);
                }
                else if (T_Nom_Analisis.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_AN4", "<script language='javascript'>alert('El Nombre del Analisis no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    string usuario = Session["user"].ToString();
                    string query = "";

                    query += "UPDATE THX_ANALISIS SET ";
                    query += "COD_ANALISIS = @COD_ANALISIS ,";
                    query += "NOM_ANALISIS = @NOM_ANALISIS , ";
                    query += "USUARIO = @USUARIO ";
                    query += " WHERE ID = @ID";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "COD_ANALISIS", valor = T_Cod_Analisis.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "NOM_ANALISIS", valor = T_Nom_Analisis.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "USUARIO", valor = usuario });
                    vars.Add(new SPVars() { nombre = "ID", valor = T_ID_LAB.Text.ToString().Trim() });

                    db.Scalar2(query, vars);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_Analisis.Visible = true;
                    T_Cod_Analisis.Text = String.Empty;
                    T_Nom_Analisis.Text = String.Empty;
                    T_Cod_Analisis.Enabled = false;
                    T_Nom_Analisis.Enabled = false;

                }
            }

        }

        protected void G_Lab_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Eliminar")
                {
                    string id = (G_Lab.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    DBUtil db = new DBUtil();

                    int test = 0;
                    test = Convert.ToInt32(db.Scalar("select count(1) from THX_DOCUMENTOS where id_analisis = " + id + " and ESTADO <> 'ELIMINADO'").ToString());
                    if (test > 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_AN1", "<script language='javascript'>alert('Existen documentos asociados al tipo de Analisis, no es posible eliminar.');</script>", false);
                    }
                    else
                    {
                        db.Scalar("delete from THX_ANALISIS where id = " + id);
                        CargarGrilla();

                        B_Guardar.Visible = false;
                        btn_nuevo_Analisis.Visible = true;
                        T_Cod_Analisis.Text = String.Empty;
                        T_Nom_Analisis.Text = String.Empty;
                        T_Cod_Analisis.Enabled = false;
                        T_Nom_Analisis.Enabled = false;
                    }


                }
                if (e.CommandName == "Editar")
                {
                    string id = (G_Lab.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    B_Guardar.Text = "Modificar";
                    B_Guardar.Visible = true;
                    btn_nuevo_Analisis.Visible = true;
                    T_ID_LAB.Text = id;


                    DBUtil db = new DBUtil();
                    DataTable dt = new DataTable();

                    dt = db.consultar("select cod_analisis, nom_analisis from THX_ANALISIS where id = " + id);
                    if (dt != null)
                    {
                        DataRow dr = dt.Rows[0];
                        T_Cod_Analisis.Text = dr[0].ToString();
                        T_Nom_Analisis.Text = dr[1].ToString();
                        T_Cod_Analisis.Enabled = true;
                        T_Nom_Analisis.Enabled = true;
                    }
                }
            }

            catch (Exception ex)
            {

            }
        }

        protected void btn_nuevo_Analisis_ServerClick(object sender, EventArgs e)
        {
            B_Guardar.Text = "Crear";
            B_Guardar.Visible = true;


            T_Cod_Analisis.Text = String.Empty;
            T_Nom_Analisis.Text = String.Empty;

            T_Cod_Analisis.Enabled = true;
            T_Nom_Analisis.Enabled = true;
            btn_nuevo_Analisis.Visible = false;
        }

        public void CargarGrilla()
        {
            DBUtil db = new DBUtil();
            G_Lab.DataSource = db.consultar("Select * from THX_ANALISIS");
            G_Lab.DataBind();
        }
    }
}