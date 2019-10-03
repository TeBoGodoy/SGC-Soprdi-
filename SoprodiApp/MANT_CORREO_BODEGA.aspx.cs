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
    public partial class MANT_CORREO_BODEGA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["SW_PERMI"].ToString() == "1")
                {
                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=1'>Abarrotes</a>";
                    titulo2.HRef = "reportes.aspx?s=1";
                    titulo2.InnerText = "Abarrotes";
                    A2.HRef = "Menu_Planificador.aspx";
                    A2.InnerText = "Planificador de Despachos";
                }
                else {

                    titulo2.HRef = "reportes.aspx?s=2";
                    titulo2.InnerText = "Granos";
                    A2.HRef = "Menu_Planificador.aspx";
                    A2.InnerText = "Planificador de Despachos";
                }
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
                cargar_bodegas();

            }
        }

        private void cargar_bodegas()
        {


            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.listar_bodegas_vm();
            dtv = dt.DefaultView;
            d_bodega.DataSource = dtv;
            d_bodega.DataTextField = "codbodega";
            d_bodega.DataValueField = "codbodega";
            //d_vendedor_.SelectedIndex = -1;
            d_bodega.DataBind();
        }

        protected void btn_nuevo_banco_ServerClick(object sender, EventArgs e)
        {
            B_Guardar.Text = "Crear";
            B_Guardar.Visible = true;


            //d_bodega.SelectedValue= String.Empty;
            t_correos.Text = String.Empty;

            d_bodega.Enabled = true;
            t_correos.Enabled = true;
            btn_nuevo_banco.Visible = false;
        }

        protected void B_Guardar_Click(object sender, EventArgs e)
        {

            string where1 = "";
            if (Session["SW_PERMI"].ToString() == "1")
            {
                where1 = "ABARROTES";
            }
            else
            {
                where1 = "GRANOS";
            }

            if (B_Guardar.Text == "Crear")
            {
                if (d_bodega.SelectedValue== "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab1", "<script language='javascript'>alert('Bodega no puede estar vacío');</script>", false);
                }
                else if (t_correos.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Correos no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    string usuario = Session["user"].ToString();
                    string query = "";



                    query += "INSERT INTO correo_bodega ( ";
                    query += "cod_bodega , ";
                    query += "correos , ";
                    query += "grupo  ";


                    query += ") VALUES ( ";
                    query += " @cod_bodega , ";
                    query += " @correos, ";
                    query += " @grupo ";
                    query += " );";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "cod_bodega", valor = d_bodega.SelectedValue.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "correos", valor = t_correos.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "grupo", valor = where1 });

                    db.Scalar2(query, vars);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    //d_bodega.SelectedValue= String.Empty;
                    t_correos.Text = String.Empty;
                    d_bodega.Enabled = false;
                    t_correos.Enabled = false;

                }
            }

            else if (B_Guardar.Text == "Modificar")
            {
                if (d_bodega.SelectedValue== "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab3", "<script language='javascript'>alert('El Codigo del Banco no puede estar vacío');</script>", false);
                }
                else if (t_correos.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab4", "<script language='javascript'>alert('El Nombre del Banco no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    string usuario = Session["user"].ToString();
                    string query = "";

                    query += "UPDATE correo_bodega SET ";
                    query += "cod_bodega = @cod_bodega ,";
                    query += "correos = @correos  ";
                    query += " WHERE cod_bodega = @cod_bodega and grupo = @grupo";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "cod_bodega", valor = d_bodega.SelectedValue.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "correos", valor = t_correos.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "ID", valor = T_ID_BANCO.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "grupo", valor = where1 });

                    db.Scalar2(query, vars);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    //d_bodega.SelectedValue= String.Empty;
                    t_correos.Text = String.Empty;
                    d_bodega.Enabled = false;
                    t_correos.Enabled = false;

                }
            }
        }

        protected void G_Banco_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string where1 = "";
                if (Session["SW_PERMI"].ToString() == "1")
                {
                    where1 += " and grupo = 'ABARROTES' ";
                }
                else
                {
                    where1 += " and grupo = 'GRANOS' ";
                }


                if (e.CommandName == "Eliminar")
                {
                    string id = (G_Banco.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    DBUtil db = new DBUtil();

                    db.Scalar("delete from correo_bodega where cod_bodega = '" + id+ "'"+ where1);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    //d_bodega.SelectedValue= String.Empty;
                    t_correos.Text = String.Empty;
                    d_bodega.Enabled = false;
                    t_correos.Enabled = false;

                }
                if (e.CommandName == "Editar")
                {
                    string id = (G_Banco.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    B_Guardar.Text = "Modificar";
                    B_Guardar.Visible = true;
                    btn_nuevo_banco.Visible = true;
                    T_ID_BANCO.Text = id;

                    DBUtil db = new DBUtil();
                    DataTable dt = new DataTable();
                 
                    dt = db.consultar("select cod_bodega, correos from correo_bodega where cod_bodega = '" + id+ "'" + where1);
                    if (dt != null)
                    {
                        DataRow dr = dt.Rows[0];
                        d_bodega.SelectedValue = dr[0].ToString();
                        t_correos.Text = dr[1].ToString();
                        d_bodega.Enabled = true;
                        t_correos.Enabled = true;
                    }
                }
            }

            catch (Exception ex)
            {

            }
        }

        public void CargarGrilla()
        {

            string where1 = "";
            if (Session["SW_PERMI"].ToString() == "1")
            {
                where1 += " where grupo = 'ABARROTES' ";
            }
            else
            {
                where1 += " where grupo = 'GRANOS' ";
            }
            DBUtil db = new DBUtil();
            G_Banco.DataSource = db.consultar("Select * from correo_bodega " + where1);
            G_Banco.DataBind();
        }
    }
}