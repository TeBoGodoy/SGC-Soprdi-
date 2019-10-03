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
    public partial class MANT_LAB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "19")
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
                if (T_Cod_Lab.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab1", "<script language='javascript'>alert('El Codigo del Laboratorio no puede estar vacío');</script>", false);
                }
                else if (T_Nom_lab.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El Nombre del Laboratorio no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    string usuario = Session["user"].ToString();
                    string query = "";

                    query += "INSERT INTO THX_LABORATORIOS ( ";
                    query += "COD_LAB , ";
                    query += "NOM_LAB , ";
                    query += "USUARIO , ";
                    query += "FECHA_CREACION ";

                    query += ") VALUES ( ";
                    query += " @COD_LAB , ";
                    query += " @NOM_LAB, ";
                    query += " @USUARIO , ";
                    query += " @FECHA_CREACION ";
                    query += " );";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "COD_LAB", valor = T_Cod_Lab.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "NOM_LAB", valor = T_Nom_lab.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "USUARIO", valor = usuario });
                    vars.Add(new SPVars() { nombre = "FECHA_CREACION", valor = DateTime.Now });

                    db.Scalar2(query, vars);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_lab.Visible = true;
                    T_Cod_Lab.Text = String.Empty;
                    T_Nom_lab.Text = String.Empty;
                    T_Cod_Lab.Enabled = false;
                    T_Nom_lab.Enabled = false;

                }
            }

            else if (B_Guardar.Text == "Modificar")
            {
                if (T_Cod_Lab.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab3", "<script language='javascript'>alert('El Codigo del Laboratorio no puede estar vacío');</script>", false);
                }
                else if (T_Nom_lab.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab4", "<script language='javascript'>alert('El Nombre del Laboratorio no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    string usuario = Session["user"].ToString();
                    string query = "";

                    query += "UPDATE THX_LABORATORIOS SET ";
                    query += "COD_LAB = @COD_LAB ,";
                    query += "NOM_LAB = @NOM_LAB , ";
                    query += "USUARIO = @USUARIO ";
                    query += " WHERE ID = @ID";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "COD_LAB", valor = T_Cod_Lab.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "NOM_LAB", valor = T_Nom_lab.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "USUARIO", valor = usuario });
                    vars.Add(new SPVars() { nombre = "ID", valor = T_ID_LAB.Text.ToString().Trim() });

                    db.Scalar2(query, vars);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_lab.Visible = true;
                    T_Cod_Lab.Text = String.Empty;
                    T_Nom_lab.Text = String.Empty;
                    T_Cod_Lab.Enabled = false;
                    T_Nom_lab.Enabled = false;

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
                    test = Convert.ToInt32(db.Scalar("select count(1) from THX_DOCUMENTOS where id_lab = " + id + " and ESTADO <> 'ELIMINADO'").ToString());
                    if (test > 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_AN1", "<script language='javascript'>alert('Existen documentos asociados al Laboratorio, no es posible eliminar.');</script>", false);
                    }
                    else
                    {
                        db.Scalar("delete from THX_LABORATORIOS where id = " + id);
                        CargarGrilla();

                        B_Guardar.Visible = false;
                        btn_nuevo_lab.Visible = true;
                        T_Cod_Lab.Text = String.Empty;
                        T_Nom_lab.Text = String.Empty;
                        T_Cod_Lab.Enabled = false;
                        T_Nom_lab.Enabled = false;
                    }                    
                }
                if (e.CommandName == "Editar")
                {
                    string id = (G_Lab.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    B_Guardar.Text = "Modificar";
                    B_Guardar.Visible = true;
                    btn_nuevo_lab.Visible = true;
                    T_ID_LAB.Text = id;


                    DBUtil db = new DBUtil();
                    DataTable dt = new DataTable();

                    dt = db.consultar("select cod_lab, nom_lab from THX_LABORATORIOS where id = " + id);
                    if (dt != null)
                    {
                        DataRow dr = dt.Rows[0];
                        T_Cod_Lab.Text = dr[0].ToString();
                        T_Nom_lab.Text = dr[1].ToString();
                        T_Cod_Lab.Enabled = true;
                        T_Nom_lab.Enabled = true;
                    }
                }
            }

            catch (Exception ex)
            {

            }
        }

        protected void btn_nuevo_lab_ServerClick(object sender, EventArgs e)
        {
            B_Guardar.Text = "Crear";
            B_Guardar.Visible = true;


            T_Cod_Lab.Text = String.Empty;
            T_Nom_lab.Text = String.Empty;

            T_Cod_Lab.Enabled = true;
            T_Nom_lab.Enabled = true;
            btn_nuevo_lab.Visible = false;
        }

        public void CargarGrilla()
        {
            DBUtil db = new DBUtil();
            G_Lab.DataSource = db.consultar("Select * from THX_LABORATORIOS");
            G_Lab.DataBind();
        }
    }
}