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
    public partial class MANT_CLIENTES : System.Web.UI.Page
    {

        public static string rutCliente;

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
                CargarGrilla_Clientes();
            }

        }


        public void CargarGrilla_Clientes()
        {
            DBUtil db = new DBUtil();
            DataTable dt = new DataTable();
            dt = db.consultar("select rtrim(ltrim(RIGHT('0000000' + Ltrim(Rtrim(rutcliente)),10))) as RUTCLIENTE,  ( rtrim(NOMBRECLIENTE) + CAST('  ---  ' AS varchar(MAX)) +  rtrim(rtrim(ltrim(RIGHT('0000000' + Ltrim(Rtrim(rutcliente)),10)))) )   AS NOMBRECLIENTE from Cliente");
            dt.Rows.Add(new Object[] { -1, "-- Seleccione --" });


            DataView dv = dt.DefaultView;
            dv.Sort = "NOMBRECLIENTE asc";
            DataTable sortedDT = dv.ToTable();
            CB_CLIENTE.DataSource = sortedDT;
            CB_CLIENTE.DataValueField = "RUTCLIENTE";
            CB_CLIENTE.DataTextField = "NOMBRECLIENTE";
            CB_CLIENTE.DataBind();
            CB_CLIENTE.SelectedValue = "-1";
        }

        protected void G_CONTACTOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void G_CONTACTOS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                string rutcontact = (G_CONTACTOS.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text);
                ReporteRNegocio.delete_contacto1(rutcontact, rutCliente);
                B_carga_contactos_Click1(sender, e);
                div_crear_contacto.Visible = false;
            }

            if (e.CommandName == "Editar")
            {
                h3_titulo.InnerText = "Editar Contacto";

                btn_nuevo.Visible = false;
                div_crear_contacto.Visible = true;

                string id = t_rut_cont.Text = (G_CONTACTOS.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text);
                string nomcontact = t_nom_cont.Text = (G_CONTACTOS.Rows[Convert.ToInt32(e.CommandArgument)].Cells[3].Text);
                string corrcontact = t_corr_cont.Text = (G_CONTACTOS.Rows[Convert.ToInt32(e.CommandArgument)].Cells[4].Text);
                string numcontact = t_num_cont.Text = (G_CONTACTOS.Rows[Convert.ToInt32(e.CommandArgument)].Cells[5].Text);
                string descrip = t_descrip.Value = (G_CONTACTOS.Rows[Convert.ToInt32(e.CommandArgument)].Cells[6].Text).Replace("&nbsp;","");

                t_rut_cont.Enabled = false;
                //ReporteRNegocio.update_contactos(rutcontact, CB_CLIENTE.SelectedValue.Trim());
                //B_carga_contactos_Click1(sender, e);

            }
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>chosen_upd();</script>", false);

        }


        public class contacto
        {

            public string rut_cont { get; set; }
            public string nom_cont { get; set; }
            public string correo_cont { get; set; }
            public string num_cont { get; set; }
            public string descrip { get; set; }
        }


        protected void btn_guarda_cont_Click(object sender, EventArgs e)
        {
            if (h3_titulo.InnerText == "Nuevo Contacto")
            {
                contacto cont = new contacto();
                cont.rut_cont = t_rut_cont.Text;
                cont.nom_cont = t_nom_cont.Text;
                cont.correo_cont = t_corr_cont.Text;
                cont.num_cont = t_num_cont.Text;
                cont.descrip = t_descrip.Value;

                if (ReporteRNegocio.insert_contacto_cobranza(CB_CLIENTE.SelectedValue, cont) == "OK")
                {

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeewe", "<script language='javascript'>alert('Cliente creado');</script>", false);

                    btn_nuevo.Visible = true;
                    div_crear_contacto.Visible = false;
                    div_contactos.Visible = true;
                    G_CONTACTOS.Visible = true;
                    G_CONTACTOS.DataSource = ReporteRNegocio.trae_contactos_cobranza(CB_CLIENTE.SelectedValue);
                    G_CONTACTOS.DataBind();
                    CleanControl(this.Controls);

                }
            }
            else if (h3_titulo.InnerText == "Editar Contacto")
            {

                contacto cont = new contacto();
                cont.rut_cont = t_rut_cont.Text;
                cont.nom_cont = t_nom_cont.Text;
                cont.correo_cont = t_corr_cont.Text;
                cont.num_cont = t_num_cont.Text;
                cont.descrip = t_descrip.Value;
                if (ReporteRNegocio.update_contacto_cobranza(rutCliente, cont) == "OK")
                {

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeewe", "<script language='javascript'>alert('Cliente editado');</script>", false);

                    btn_nuevo.Visible = true;
                    div_crear_contacto.Visible = false;
                    div_contactos.Visible = true;
                    G_CONTACTOS.Visible = true;
                    G_CONTACTOS.DataSource = ReporteRNegocio.trae_contactos_cobranza(CB_CLIENTE.SelectedValue);
                    G_CONTACTOS.DataBind();
                    CleanControl(this.Controls);

                }
            }

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeewe", "<script language='javascript'>alert('Error al crear cliente');</script>", false);


        }

        protected void btn_cierra_cont_Click(object sender, EventArgs e)
        {
            div_crear_contacto.Visible = false;
            btn_nuevo.Visible = true;
        }

        protected void btn_nuevo_Click(object sender, EventArgs e)
        {
            t_rut_cont.Enabled = true;
            h3_titulo.InnerText = "Nuevo Contacto";
            btn_nuevo.Visible = false;
            div_crear_contacto.Visible = true;
            CleanControl(this.Controls);
           
        }

        public string confirmDelete(string Name)
        {
            return @"javascript:if(!confirm('Esta acción va eliminar al Contacto: "
               + Name.ToUpper()
               + @". ¿Estás seguro?')){return false;}";
        }

        protected void B_carga_contactos_Click1(object sender, EventArgs e)
        {
            rutCliente =   CB_CLIENTE.SelectedValue.Trim();

            if (CB_CLIENTE.SelectedValue.Trim() != "-1")
            {

                btn_nuevo.Visible = true;
                div_contactos.Visible = true;
                G_CONTACTOS.Visible = true;
                G_CONTACTOS.DataSource = ReporteRNegocio.trae_contactos_cobranza(CB_CLIENTE.SelectedValue.Trim());
                G_CONTACTOS.DataBind();
                CleanControl(this.Controls);
            }
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>chosen_upd();</script>", false);

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
            t_descrip.Value = "";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>chosen_upd();</script>", false);

        }

    }
}