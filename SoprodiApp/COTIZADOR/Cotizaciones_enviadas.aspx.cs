using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Entities;
using CRM.BusinessLayer;
using ThinxsysFramework;
using System.Data;
using System.Web.Services;
using SoprodiApp.entidad;
using SoprodiApp.negocio;
using System.Web.Security;

namespace COTIZADOR
{
    public partial class Cotizaciones_enviadas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Usuario"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    LlenarLista();
                    LlenarServicio();
                }
            }
            else
            {
                JQ_Datatable();
            }
        }

        public void LlenarServicio()
        {
            DBUtil db = new DBUtil();
            G_SERVICIOS.DataSource = db.consultar("select * from CTZ_LOG_COTIZACIONES_SERVICIOS where id_cotizacion_servicio_log = -1");
            G_SERVICIOS.DataBind();
        }

        public void LlenarLista()
        {
            usuarioEntity vend = new usuarioEntity();
            vend = (usuarioEntity)(Session["usuario"]);
            G_PRINCIPAL.DataSource = ctz_log_cotizacionBO.GetAll(" where cod_vendedor =  '" + vend.cod_usuario + "' ");
            G_PRINCIPAL.DataBind();
        }


        public void JQ_Datatable()
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "SP_COTIZACIONES", "<script language='javascript'>chosen();Datatables();</script>", false);
        }

        protected void G_PRINCIPAL_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //editar
                if (e.CommandName == "Ver")
                {
                    int id = int.Parse((G_PRINCIPAL.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString()));
                    string fecha = (G_PRINCIPAL.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[1].ToString());
                    string correo = (G_PRINCIPAL.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString());
                    string nombre = (G_PRINCIPAL.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[3].ToString());
                    COMPLETAR_DETALLE(id, fecha, correo, nombre);
                    PANEL_PRINCIPAL.Visible = false;
                    PANEL_DETALLE.Visible = true;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void COMPLETAR_DETALLE(int id, string fecha, string correo, string nombre)
        {
            DBUtil db = new DBUtil();
            DataTable dt = new DataTable();

            ctz_log_cotizacionEntity ctz = new ctz_log_cotizacionEntity();
            ctz.id_cotizacion_log = id;
            if (ctz_log_cotizacionBO.encontrar(ref ctz) == "OK")
            {

                T_ID.Text = ctz.id_cotizacion.ToString();
                T_CLIENTE.Text = nombre;
                T_FECHA.Text = fecha;
                T_CORREO.Text = correo;
                LlenarDetalle(id);
                G_SERVICIOS.DataSource = ctz_log_cotizaciones_serviciosBO.GetAll(" where id_cotizacion_log = " + id);
                G_SERVICIOS.DataBind();
                CalcularTotales(id);
            }
        }

        public void LlenarDetalle(int id)
        {
            DBUtil db = new DBUtil();
            DataTable detalle = ctz_log_cotizacion_detBO.GetAll(" where id_cotizacion_log = " + id);
            if (detalle.Rows.Count > 0)
            {
                G_DETALLE_COTIZACION.Columns[6].Visible = false;
                G_DETALLE_COTIZACION.Columns[7].Visible = false;
                // MOSTRAR OCULTAR COLUMNAS
                int contttt = 1;
                string columnas_precio = db.Scalar("select columnas_precio from ctz_LOG_cotizacion where id_cotizacion_log = " + id).ToString();
                string[] cms = columnas_precio.Split(new Char[] { ';' });
                foreach (string x in cms)
                {
                    if (x != "")
                    {
                        if (contttt == 1)
                        {
                            G_DETALLE_COTIZACION.Columns[5].HeaderText = db.Scalar("select nombre_bodega from ctz_bodegas where cod_bodega = '" + x + "'").ToString();
                        }
                        if (contttt == 2)
                        {
                            G_DETALLE_COTIZACION.Columns[6].Visible = true;
                            G_DETALLE_COTIZACION.Columns[6].HeaderText = db.Scalar("select nombre_bodega from ctz_bodegas where cod_bodega = '" + x + "'").ToString();
                        }
                        if (contttt == 3)
                        {
                            G_DETALLE_COTIZACION.Columns[7].Visible = true;
                            G_DETALLE_COTIZACION.Columns[7].HeaderText = db.Scalar("select nombre_bodega from ctz_bodegas where cod_bodega = '" + x + "'").ToString();
                        }
                        contttt++;
                    }
                }
            }
            G_DETALLE_COTIZACION.DataSource = detalle;
            G_DETALLE_COTIZACION.DataBind();
        }

        protected void B_VOLVER_Click(object sender, EventArgs e)
        {
            LlenarLista();
            PANEL_PRINCIPAL.Visible = true;
            PANEL_DETALLE.Visible = false;
        }

        #region ---------------- NO CAMBIAR ---------------- 
        public void alert(string mensaje, string titulo, string color, int tiempo)
        {
            // ROJO - VERDE 
            ScriptManager.RegisterStartupScript(this, typeof(Page), "mosnoti", "<script>javascript:Mensaje('" + titulo + "', '" + mensaje + "', '" + color + "', " + tiempo + ");</script>", false);
        }
        public void LIMPIARCAMPOS()
        {
            CleanControl(this.Controls);
        }

        public void CleanControl(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = string.Empty;
                else if (control is DropDownList)
                    ((DropDownList)control).ClearSelection();
                else if (control is ListBox)
                    ((ListBox)control).ClearSelection();
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
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            MakeAccessible(G_PRINCIPAL);
            MakeAccessible(G_DETALLE_COTIZACION);
            MakeAccessible(G_SERVICIOS);
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
        #endregion

        public void CalcularTotales(int id_cotizacion)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "calculartotales", "<script>javascript:calcular_totales(" + id_cotizacion + ");</script>", false);
        }
    }
}