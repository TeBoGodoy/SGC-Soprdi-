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
    public partial class COBRANZA_MANT_BANCO : System.Web.UI.Page
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
                CargarGrilla_SOPRODI();
            }
        }

        protected void btn_nuevo_banco_ServerClick(object sender, EventArgs e)
        {
            B_Guardar.Text = "Crear";
            B_Guardar.Visible = true;

            T_Cod_banco.Text = String.Empty;
            T_Nom_banco.Text = String.Empty;
            T_cuenta.Text = String.Empty;
            T_moneda.Text = String.Empty;

            T_Cod_banco.Enabled = true;
            T_Nom_banco.Enabled = true;
            T_cuenta.Enabled = true;
            T_moneda.Enabled = true;

            btn_nuevo_banco.Visible = false;
        }

        protected void B_Guardar_Click(object sender, EventArgs e)
        {

            if (rd_cliente.Checked)
            {
                CREA_BANCO_CLIENTE();

            }
            else
            {
                CREA_BANCO_SOPRODI();
            }
        }

        private void CREA_BANCO_SOPRODI()
        {
            if (B_Guardar.Text == "Crear")
            {
                if (T_Cod_banco.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab1", "<script language='javascript'>alert('El Codigo del Banco no puede estar vacío');</script>", false);
                }
                else if (T_Nom_banco.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El Nombre del Banco no puede estar vacío');</script>", false);
                }
                else if (T_cuenta.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Cuenta no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    //string usuario = Session["user"].ToString();
                    string query = "";

                    query += "INSERT INTO Cobranza_bancos ( ";
                    query += "COD_BANCO   , ";
                    query += "NOM_BANCO   , ";
                    query += "ACCT        , ";
                    query += "TIPO_CUENTA , ";
                    query += "TIPO_MONEDA   ";
                    query += ") VALUES ( ";
                    query += " @COD_BANCO   , ";
                    query += " @NOM_BANCO   , ";
                    query += " @ACCT        , ";
                    query += " @TIPO_CUENTA , ";
                    query += " @TIPO_MONEDA   ";
                    query += " );";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "COD_BANCO", valor = T_Cod_banco.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "NOM_BANCO", valor = T_Nom_banco.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "ACCT", valor = T_cuenta.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "TIPO_CUENTA", valor = "SOPRODI" });
                    vars.Add(new SPVars() { nombre = "TIPO_MONEDA", valor = T_moneda.Text.ToString().Trim() });

                    db.Scalar2(query, vars);
                    CargarGrilla_SOPRODI();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    T_Cod_banco.Text = String.Empty;
                    T_Nom_banco.Text = String.Empty;
                    T_cuenta.Text = String.Empty;
                    T_moneda.Text = String.Empty;
                    T_Cod_banco.Enabled = false;
                    T_Nom_banco.Enabled = false;
                    T_cuenta.Enabled = false;
                    T_moneda.Enabled = false;

                }
            }

            else if (B_Guardar.Text == "Modificar")
            {
                if (T_Cod_banco.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab3", "<script language='javascript'>alert('El Codigo del Banco no puede estar vacío');</script>", false);
                }
                else if (T_Nom_banco.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab4", "<script language='javascript'>alert('El Nombre del Banco no puede estar vacío');</script>", false);
                }
                else if (T_cuenta.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Cuenta no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    //string usuario = Session["user"].ToString();
                    string query = "";

                    query += "UPDATE Cobranza_bancos SET ";
                    query += "COD_BANCO = @COD_BANCO        , ";
                    query += "NOM_BANCO = @NOM_BANCO        , ";
                    query += "ACCT      = @ACCT             , ";
                    query += "TIPO_CUENTA = @TIPO_CUENTA    , ";
                    query += "TIPO_MONEDA = @TIPO_MONEDA     ";
                    query += " WHERE ID = @ID";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "COD_BANCO", valor = T_Cod_banco.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "NOM_BANCO", valor = T_Nom_banco.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "ACCT", valor = T_cuenta.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "ID", valor = T_ID_BANCO.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "TIPO_CUENTA", valor = "SOPRODI" });
                    vars.Add(new SPVars() { nombre = "TIPO_MONEDA", valor = T_moneda.Text.ToString().Trim() });

                    db.Scalar2(query, vars);
                    CargarGrilla_SOPRODI();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    T_Cod_banco.Text = String.Empty;
                    T_Nom_banco.Text = String.Empty;
                    T_cuenta.Text = String.Empty;
                    T_moneda.Text = String.Empty;
                    T_Cod_banco.Enabled = false;
                    T_Nom_banco.Enabled = false;
                    T_cuenta.Enabled = false;
                    T_moneda.Enabled = false;
                }
            }
        }

        private void CREA_BANCO_CLIENTE()
        {
            if (B_Guardar.Text == "Crear")
            {
                if (T_Cod_banco.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab1", "<script language='javascript'>alert('El Codigo del Banco no puede estar vacío');</script>", false);
                }
                else if (T_Nom_banco.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El Nombre del Banco no puede estar vacío');</script>", false);
                }           
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    //string usuario = Session["user"].ToString();
                    string query = "";

                    query += "INSERT INTO Cobranza_bancos ( ";
                    query += "COD_BANCO  , ";
                    query += "NOM_BANCO  ,  ";
                    query += "TIPO_CUENTA ";
                    //query += "ACCT  ";
                    query += ") VALUES ( ";
                    query += " @COD_BANCO   ,";
                    query += " @NOM_BANCO   ,";
                    query += " @TIPO_CUENTA ";
                    //query += " @ACCT, ";
                    query += " );";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "COD_BANCO", valor = T_Cod_banco.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "NOM_BANCO", valor = T_Nom_banco.Text.ToString().Trim() });
                    //vars.Add(new SPVars() { nombre = "ACCT", valor = T_cuenta.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "TIPO_CUENTA", valor = "CLIENTE" });

                    db.Scalar2(query, vars);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    T_Cod_banco.Text = String.Empty;
                    T_Nom_banco.Text = String.Empty;
                    T_cuenta.Text = String.Empty;
                    T_moneda.Text = String.Empty;
                    T_Cod_banco.Enabled = false;
                    T_Nom_banco.Enabled = false;
                    T_cuenta.Enabled = false;
                }
            }

            else if (B_Guardar.Text == "Modificar")
            {
                if (T_Cod_banco.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab3", "<script language='javascript'>alert('El Codigo del Banco no puede estar vacío');</script>", false);
                }
                else if (T_Nom_banco.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab4", "<script language='javascript'>alert('El Nombre del Banco no puede estar vacío');</script>", false);
                }
                //else if (T_cuenta.Text == "")
                //{
                //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('Cuenta no puede estar vacío');</script>", false);
                //}
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    //string usuario = Session["user"].ToString();
                    string query = "";

                    query += "UPDATE Cobranza_bancos SET ";
                    query += "COD_BANCO = @COD_BANCO ,  ";
                    query += "TIPO_CUENTA = @TIPO_CUENTA    , ";
                    query += "NOM_BANCO = @NOM_BANCO  ";
                    //query += "ACCT = @ACCT  ";
                    query += " WHERE ID = @ID";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "COD_BANCO", valor = T_Cod_banco.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "NOM_BANCO", valor = T_Nom_banco.Text.ToString().Trim() });
                    //vars.Add(new SPVars() { nombre = "ACCT", valor = T_cuenta.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "ID", valor = T_ID_BANCO.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "TIPO_CUENTA", valor = "CLIENTE" });

                    db.Scalar2(query, vars);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    T_Cod_banco.Text = String.Empty;
                    T_Nom_banco.Text = String.Empty;
                    T_cuenta.Text = String.Empty;
                    T_moneda.Text = String.Empty;
                    T_Cod_banco.Enabled = false;
                    T_Nom_banco.Enabled = false;
                    T_cuenta.Enabled = false;
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

                    db.Scalar("delete from COBRANZA_BANCOS where id = " + id);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    T_Cod_banco.Text = String.Empty;
                    T_Nom_banco.Text = String.Empty;
                    T_cuenta.Text = String.Empty;
                    T_moneda.Text = String.Empty;
                    T_Cod_banco.Enabled = false;
                    T_Nom_banco.Enabled = false;
                    T_cuenta.Enabled = false;
                    T_moneda.Enabled = false;
                }
                if (e.CommandName == "Editar")
                {
                    string id = (G_Banco.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    B_Guardar.Text = "Modificar";
                    B_Guardar.Visible = true;
                    btn_nuevo_banco.Visible = false;
                    T_ID_BANCO.Text = id;

                    DBUtil db = new DBUtil();
                    DataTable dt = new DataTable();

                    dt = db.consultar("select COD_BANCO, NOM_BANCO, ACCT from COBRANZA_BANCOS where id = " + id);
                    if (dt != null)
                    {
                        DataRow dr = dt.Rows[0];
                        T_Cod_banco.Text = dr[0].ToString();
                        T_Nom_banco.Text = dr[1].ToString();
                        T_cuenta.Text = dr[2].ToString();
                        T_Cod_banco.Enabled = true;
                        T_Nom_banco.Enabled = true;
                        T_cuenta.Enabled = true;
                        T_moneda.Enabled = true;
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
            G_Banco.DataSource = db.consultar("Select [id],[cod_banco],[nom_banco] from Cobranza_bancos where tipo_cuenta ='CLIENTE' ");
            G_Banco.DataBind();
        }


        public void CargarGrilla_SOPRODI()
        {
            DBUtil db = new DBUtil();
            G_Banco_soprodi.DataSource = db.consultar("Select [id], [cod_banco] ,[nom_banco],[acct],[tipo_moneda] from Cobranza_bancos where tipo_cuenta ='SOPRODI'");
            G_Banco_soprodi.DataBind();
        }

        protected void G_Banco_soprodi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Eliminar")
                {
                    string id = (G_Banco_soprodi.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    DBUtil db = new DBUtil();

                    db.Scalar("delete from COBRANZA_BANCOS where id = " + id);
                    CargarGrilla_SOPRODI();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    T_Cod_banco.Text = String.Empty;
                    T_Nom_banco.Text = String.Empty;
                    T_cuenta.Text = String.Empty;
                    T_moneda.Text = String.Empty;
                    T_Cod_banco.Enabled = false;
                    T_Nom_banco.Enabled = false;
                    T_cuenta.Enabled = false;
                    T_moneda.Enabled = false;
                }
                if (e.CommandName == "Editar")
                {
                    string id = (G_Banco_soprodi.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString());
                    B_Guardar.Text = "Modificar";
                    B_Guardar.Visible = true;
                    btn_nuevo_banco.Visible = false;
                    T_ID_BANCO.Text = id;

                    DBUtil db = new DBUtil();
                    DataTable dt = new DataTable();

                    dt = db.consultar("select COD_BANCO, NOM_BANCO, ACCT, tipo_moneda from COBRANZA_BANCOS where id = " + id);
                    if (dt != null)
                    {
                        DataRow dr = dt.Rows[0];
                        T_Cod_banco.Text = dr[0].ToString();
                        T_Nom_banco.Text = dr[1].ToString();
                        T_cuenta.Text = dr[2].ToString();
                        T_moneda.Text = dr[3].ToString();
                        T_Cod_banco.Enabled = true;
                        T_Nom_banco.Enabled = true;
                        T_cuenta.Enabled = true;
                        T_moneda.Enabled = true;
                    }
                }
            }

            catch (Exception ex)
            {

            }
        }

        protected void enable_botones_ServerClick(object sender, EventArgs e)
        {
            T_Cod_banco.Text = String.Empty;
            T_Nom_banco.Text = String.Empty;
            T_cuenta.Text = String.Empty;
            T_moneda.Text = String.Empty;
            T_Cod_banco.Enabled = true;
            T_Nom_banco.Enabled = true;
            T_cuenta.Enabled = true;
            T_moneda.Enabled = true;

            B_Guardar.Visible = false;
            btn_nuevo_banco.Visible = true;

        }

        protected void disable_botones_ServerClick(object sender, EventArgs e)
        {
            T_Cod_banco.Text = String.Empty;
            T_Nom_banco.Text = String.Empty;
            T_cuenta.Text = String.Empty;
            T_moneda.Text = String.Empty;
            T_Cod_banco.Enabled = false;
            T_Nom_banco.Enabled = false;
            T_cuenta.Enabled = false;
            T_moneda.Enabled = false;


            B_Guardar.Visible = false;
            btn_nuevo_banco.Visible = true;

        }

        protected void rd_cliente_CheckedChanged(object sender, EventArgs e)
        {
            div_cuenta_acct.Visible = false;
            btn_nuevo_banco.Visible = true;
            B_Guardar.Visible = false;

            T_Cod_banco.Text = String.Empty;
            T_Nom_banco.Text = String.Empty;
            T_cuenta.Text = String.Empty;
            T_moneda.Text = String.Empty;
            T_Cod_banco.Enabled = false;
            T_Nom_banco.Enabled = false;
            T_cuenta.Enabled = false;
            T_moneda.Enabled = false;

            titulo_panel_finanza.InnerHtml = "<i class='fa fa-list'></i>Mantenedor de Bancos -- Cliente";

            div_bancos_clientes.Visible = true;
            div_bancos_soprodi.Visible = false;
        }

        protected void rd_soprodi_CheckedChanged(object sender, EventArgs e)
        {
            div_cuenta_acct.Visible = true;
            btn_nuevo_banco.Visible = true;
            B_Guardar.Visible = false;

            T_Cod_banco.Text = String.Empty;
            T_Nom_banco.Text = String.Empty;
            T_cuenta.Text = String.Empty;
            T_moneda.Text = String.Empty;
            T_Cod_banco.Enabled = false;
            T_Nom_banco.Enabled = false;
            T_cuenta.Enabled = false;
            T_moneda.Enabled = false;

            titulo_panel_finanza.InnerHtml = "<i class='fa fa-list'></i>Mantenedor de Bancos -- Soprodi";

            div_bancos_clientes.Visible = false;
            div_bancos_soprodi.Visible = true;
        }
    }
}