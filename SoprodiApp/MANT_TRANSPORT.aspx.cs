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
    public partial class MANT_TRANSPORT : System.Web.UI.Page
    {
        //private static Page page;

        protected void Page_Load(object sender, EventArgs e)
        {
            //JQ_Datatable();
            //page = this.Page;
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


                if (Session["SW_PERMI"].ToString() == "1")
                {

                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=1'>Abarrotes</a>";
                    titulo2.HRef = "reportes.aspx?s=1";
                    titulo2.InnerText = "Abarrotes";
                    titulo3.HRef = "Menu_Planificador.aspx";
                    titulo3.InnerText = "Planificador de Despachos";
                }
                else if (Session["SW_PERMI"].ToString() == "2")
                {
                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=2'>Granos</a>";
                    titulo2.HRef = "reportes.aspx?s=2";
                    titulo2.InnerText = "Granos";
                    titulo3.HRef = "Menu_Planificador.aspx";
                    titulo3.InnerText = "Planificador de Despachos";
                }

                CargarGrilla();
                CargarBodega();

                //CleanControl(this.Controls);

            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            MakeAccessible(G_Banco);
            //MakeAccessible(G_ASIGNADOS);
            //MakeAccessible(G_QR);
            //MakeAccessible(G_LISTA);
        }
        public void JQ_Datatable()
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "asd", "<script language='javascript'>creagrilla();</script>", false);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            Page.ClientScript.RegisterForEventValidation(this.UniqueID);
            base.Render(writer);
        }

        public static void MakeAccessible(GridView grid)
        {
            if (grid.Rows.Count <= 0) return;
            grid.UseAccessibleHeader = true;
            grid.HeaderRow.TableSection = TableRowSection.TableHeader;
            grid.PagerStyle.CssClass = "GridPager";
            if (grid.ShowFooter)
                grid.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        public void CleanControl(ControlCollection controles)
        {

            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = string.Empty;
                //else if (control is DropDownList)
                //    ((DropDownList)control).ClearSelection();
                else if (control is RadioButtonList)
                    ((RadioButtonList)control).ClearSelection();
                else if (control is CheckBoxList)
                    ((CheckBoxList)control).ClearSelection();
                else if (control is RadioButton)
                    ((RadioButton)control).Checked = false;
                else if (control is CheckBox)
                    ((CheckBox)control).Checked = false;
                else if (control.HasControls())
                    CleanControl(control.Controls);
            }

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>chosen_upd();</script>", false);

        }

        private void CargarBodega()
        {
            string where = " where 1=1 ";

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.carga_bodega(where);
            dtv = dt.DefaultView;
            CB_BODEGA.DataSource = dtv;
            CB_BODEGA.DataTextField = "nom_bodega";
            CB_BODEGA.DataValueField = "nom_bodega";
            CB_BODEGA.DataBind();
        }

        protected void btn_nuevo_banco_ServerClick(object sender, EventArgs e)
        {
            B_Guardar.Text = "Crear";
            B_Guardar.Visible = true;


            t_rut.Text = String.Empty;
            t_nombre.Text = String.Empty;
            t_direcc.Text = String.Empty;
            t_fono.Text = String.Empty;
            t_inicial.Text = String.Empty;



            t_rut.Enabled = true;
            t_nombre.Enabled = true;
            t_direcc.Enabled = true;
            t_fono.Enabled = true;
            t_inicial.Enabled = true;
            CB_BODEGA.Enabled = true;
            btn_nuevo_banco.Visible = false;
            JQ_Datatable();
        }

        protected void B_Guardar_Click(object sender, EventArgs e)
        {
            if (B_Guardar.Text == "Crear")
            {
                if (t_rut.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab1", "<script language='javascript'>alert('El rut del transportista no puede estar vacío');</script>", false);
                }
                else if (t_nombre.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El Nombre del transportista no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    string usuario = Session["user"].ToString();
                    string query = "";

                    string grupo = "";
                    if (Session["SW_PERMI"].ToString() == "1")
                    {
                        grupo = "ABARROTES";
                    }
                    else
                    {
                        grupo = "GRANOS";
                    }

                    query += "INSERT INTO transportista ( ";
                    query += "nombre_trans , ";
                    query += "rut,  ";
                    query += "fono , ";
                    query += "direccion, ";
                    query += "cod_bodega, ";
                    query += "GRUPO ";

                    query += ") VALUES ( ";
                    query += "@NOM , ";
                    query += "@RUN,  ";
                    query += "@FON , ";
                    query += "@DIRECC, ";
                    query += "@C_INICIAL, ";
                    query += "@GRUPO ";
                    query += " );";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "NOM", valor = t_nombre.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "RUN", valor = t_rut.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "FON", valor = t_fono.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "DIRECC", valor = t_direcc.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "C_INICIAL", valor = CB_BODEGA.SelectedValue.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "GRUPO", valor = grupo });

                    db.Scalar2(query, vars);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    t_rut.Text = String.Empty;
                    t_nombre.Text = String.Empty;
                    t_direcc.Text = String.Empty;
                    t_fono.Text = String.Empty;
                    t_inicial.Text = String.Empty;
  

                    t_rut.Enabled = false;
                    t_nombre.Enabled = false;
                    t_inicial.Enabled = false;
                    t_direcc.Enabled = false;
                    t_fono.Enabled = false;
                    CB_BODEGA.Enabled = false;
                }
            }

            else if (B_Guardar.Text == "Modificar")
            {
                if (t_rut.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab3", "<script language='javascript'>alert('El Codigo del Banco no puede estar vacío');</script>", false);
                }
                else if (t_nombre.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab4", "<script language='javascript'>alert('El Nombre del Banco no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    string usuario = Session["user"].ToString();
                    string query = "";

                    query += "UPDATE Transportista SET ";
                    query += "nombre_trans = @NOM, ";
                    query += "rut = @RUN,  ";
                    query += "fono = @FON, ";
                    query += "direccion = @DIRECC, ";
                    query += "cod_bodega = @C_INICIAL ";
                    query += " WHERE COD_TRANS = @COD";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "NOM", valor = t_nombre.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "RUN", valor = t_rut.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "FON", valor = t_fono.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "DIRECC", valor = t_direcc.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "C_INICIAL", valor = CB_BODEGA.SelectedValue.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "COD", valor = T_ID_BANCO.Text.ToString().Trim() });

                    db.Scalar2(query, vars);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    t_rut.Text = String.Empty;
                    t_nombre.Text = String.Empty;
                    t_direcc.Text = String.Empty;
                    t_fono.Text = String.Empty;
                    
                    t_rut.Enabled = false;
                    t_nombre.Enabled = false;
                    t_direcc.Enabled = false;
                    t_fono.Enabled = false;
                    CB_BODEGA.Enabled = false;
                }
            }

            JQ_Datatable();
        }

        protected void G_Banco_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Eliminar")
                {
                    string id = (G_Banco.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    DBUtil db = new DBUtil();

                    db.Scalar("delete from Transportista where cod_trans = " + id);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    t_rut.Text = String.Empty;
                    t_nombre.Text = String.Empty;
                    t_rut.Enabled = false;
                    t_nombre.Enabled = false;

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

                    dt = db.consultar("select nombre_trans, rut, fono , direccion, cod_bodega from Transportista where cod_trans = " + id);
                    if (dt != null)
                    {
                        DataRow dr = dt.Rows[0];
                        t_nombre.Text = dr[0].ToString();
                        t_rut.Text = dr[1].ToString();
                        t_fono.Text = dr[2].ToString();
                        t_direcc.Text = dr[3].ToString();
                        CB_BODEGA.SelectedValue = dr[4].ToString();

                        t_rut.Enabled = true;
                        t_nombre.Enabled = true;
                        t_inicial.Enabled = true;
                        t_fono.Enabled = true;
                        t_direcc.Enabled = true;
                        CB_BODEGA.Enabled = true;

                    }
                }

                JQ_Datatable();
            }

            catch (Exception ex)
            {

            }
        }

        public void CargarGrilla()
        {
            DBUtil db = new DBUtil();

            string where1 = "";
            if (Session["SW_PERMI"].ToString() == "1")
            {
                where1 += " where grupo = 'ABARROTES' ";
            }
            else
            {
                where1 += " where grupo = 'GRANOS' ";
            }

            G_Banco.DataSource = db.consultar("Select [cod_trans],[nombre_trans],[rut],[fono],[direccion], [cod_bodega] from transportista" + where1);
            G_Banco.DataBind();
        }
    }
}