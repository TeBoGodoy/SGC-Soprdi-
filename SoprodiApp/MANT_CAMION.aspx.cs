﻿using System;
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
    public partial class MANT_CAMION : System.Web.UI.Page
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


                if (ReporteRNegocio.esvendedor(User.Identity.Name) == "2") {

                    Response.Redirect("MENU.aspx");
                }



                CargarGrilla();
                cargar_trans();
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


        private void cargar_trans()
        {
            DataTable dt; DataView dtv = new DataView();
            string where1 = "";
            if (Session["SW_PERMI"].ToString() == "1")
            {
                where1 += " where grupo = 'ABARROTES' ";
            }
            else
            {
                where1 += " where grupo = 'GRANOS' ";
            }

            dt = ReporteRNegocio.listar_transpor_2(where1);
            dtv = dt.DefaultView;
            d_trans.DataSource = dtv;
            d_trans.DataTextField = "trans_bodeg";
            d_trans.DataValueField = "cod_trans";
            //d_vendedor_.SelectedIndex = -1;
            d_trans.DataBind();
        }

        protected void btn_nuevo_banco_ServerClick(object sender, EventArgs e)
        {
            B_Guardar.Text = "Crear";
            B_Guardar.Visible = true;

            t_nombre.Text = String.Empty;
            
            t_carga.Text = String.Empty;
            t_tara.Text = String.Empty;



            d_trans.Enabled = true;
            t_nombre.Enabled = true;
            t_tara.Enabled = true;
            t_carga.Enabled = true;

                
            btn_nuevo_banco.Visible = false;

            JQ_Datatable();
        }

        protected void B_Guardar_Click(object sender, EventArgs e)
        {
            if (B_Guardar.Text == "Crear")
            {
        
                if (t_nombre.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El patente no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    string usuario = Session["user"].ToString();
                    string query = "";

                    query += "INSERT INTO camion ( ";
                    query += "patente , ";
                    query += "carga,  ";
                    query += "tara, ";
                    query += "cod_trans ";

                    query += ") VALUES ( ";
                    query += "@NOM , ";
                    query += "@CARGA , ";
                    query += "@TARA , ";
                    query += "@cod_trans ";
                    query += " );";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "NOM", valor = t_nombre.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "CARGA", valor = t_carga.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "TARA", valor = t_tara.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "cod_trans", valor = d_trans.SelectedValue.ToString().Trim() });



                    db.Scalar2(query, vars);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;

                    t_nombre.Text = String.Empty;

                    t_carga.Text = String.Empty;
                    t_tara.Text = String.Empty;


                    d_trans.Enabled = false;
                    t_nombre.Enabled = false;
                    t_carga.Enabled = false;
                    t_tara.Enabled = false;

                }
            }

            else if (B_Guardar.Text == "Modificar")
            {
               if (t_nombre.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab4", "<script language='javascript'>alert('El Nombre del Banco no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    string usuario = Session["user"].ToString();
                    string query = "";

                    query += "UPDATE CAMION SET ";
                    query += "PATENTE = @NOM, ";
                    query += "CARGA = @CARGA, ";
                    query += "TARA = @TARA, ";
                    query += "cod_trans = @cod_trans ";
                    query += " WHERE cod_camion = @COD";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "NOM", valor = t_nombre.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "CARGA", valor = t_carga.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "TARA", valor = t_carga.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "cod_trans", valor = d_trans.SelectedValue.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "COD", valor = T_ID_BANCO.Text.ToString().Trim() });

                    db.Scalar2(query, vars);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;

                    t_nombre.Text = String.Empty;

                    t_carga.Text = String.Empty;
                    
                    d_trans.Enabled = false;
                    t_nombre.Enabled = false;
                    t_carga.Enabled = false;
                   
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
                    string id = (G_Banco.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                    DBUtil db = new DBUtil();

                    db.Scalar("delete from CAMION where cod_camion = " + id);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;

                    t_nombre.Text = String.Empty;

                    t_nombre.Enabled = false;

                }
                if (e.CommandName == "Editar")
                {
                    string id = (G_Banco.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                    B_Guardar.Text = "Modificar";
                    B_Guardar.Visible = true;
                    btn_nuevo_banco.Visible = true;
                    T_ID_BANCO.Text = id;

                    DBUtil db = new DBUtil();
                    DataTable dt = new DataTable();

                    dt = db.consultar("select COD_TRANS, patente, carga, tara from camion where cod_camion = " + id);
                    if (dt != null)
                    {
                        DataRow dr = dt.Rows[0];
                        t_nombre.Text = dr[1].ToString();
                        d_trans.SelectedValue= dr[0].ToString();
                        t_tara.Text = dr[3].ToString();
                        t_carga.Text = dr[2].ToString();

                        d_trans.Enabled = true;
                        t_nombre.Enabled = true;
                        t_tara.Enabled = true;
                        t_carga.Enabled = true;

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
            update_panel_.Update();
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
            G_Banco.DataSource = db.consultar("Select * from V_Camion_Trans"+ where1);
            G_Banco.DataBind();

            JQ_Datatable();
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {

        }
    }
}