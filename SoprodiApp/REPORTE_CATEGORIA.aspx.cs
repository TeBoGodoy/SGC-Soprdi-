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
    public partial class REPORTE_CATEGORIA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<string> u_negocio = ReporteRNegocio.trae_app(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "32")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }



                if (Session["SW_PERMI"].ToString() == "1")
                {

                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=1'>Abarrotes</a>";
                    titulo2.HRef = "reportes.aspx?s=1";
                    titulo2.InnerText = "Abarrotes";
                }
                else if (Session["SW_PERMI"].ToString() == "2")
                {
                    //titulo.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='reportes.aspx?s=2'>Granos</a>";
                    titulo2.HRef = "reportes.aspx?s=2";
                    titulo2.InnerText = "Granos";

                }


                CargarGrilla();
            }
        }

        protected void btn_nuevo_banco_ServerClick(object sender, EventArgs e)
        {
            B_Guardar.Text = "Crear";
            B_Guardar.Visible = true;


            T_Cod_banco.Text = String.Empty;
            T_Nom_banco.Text = String.Empty;

            T_Cod_banco.Enabled = true;
            T_Nom_banco.Enabled = true;
            btn_nuevo_banco.Visible = false;
        }


        protected void click_categoria(object sender, EventArgs e)
        {


            // GUARDAR
            DBUtil db = new DBUtil();
            string usuario = Session["user"].ToString();
            string query = "";

            query += "INSERT INTO categoria_producto ( ";
            query += " id_categoria, id_producto  ";


            query += ") VALUES ( ";
            query += " @id_categoria, @id_producto ";
            query += " );";

            List<SPVars> vars = new List<SPVars>();
            vars.Add(new SPVars() { nombre = "id_categoria", valor = tx_id_categ.Text.ToString().Trim() });
            vars.Add(new SPVars() { nombre = "id_producto", valor = producto_modal.Text.ToString().Trim() });

            db.Scalar2(query, vars);
            //Cargarmodal();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_l2ab2", "<script language='javascript'>carga_producs();</script>", false);
        }

        protected void B_Guardar_Click(object sender, EventArgs e)
        {
            if (B_Guardar.Text == "Crear")
            {
                //if (T_Cod_banco.Text == "")
                //{
                //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab1", "<script language='javascript'>alert('El Codigo del Banco no puede estar vacío');</script>", false);
                //}
                if (T_Nom_banco.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab2", "<script language='javascript'>alert('El Nombre del Banco no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    string usuario = Session["user"].ToString();
                    string query = "";

                    query += "INSERT INTO categoria ( ";
                    query += "NOM_CATEG  ";


                    query += ") VALUES ( ";
                    query += " @NOM_CATEG ";
                    query += " );";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "NOM_CATEG", valor = T_Nom_banco.Text.ToString().Trim() });

                    db.Scalar2(query, vars);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    T_Cod_banco.Text = String.Empty;
                    T_Nom_banco.Text = String.Empty;
                    T_Cod_banco.Enabled = false;
                    T_Nom_banco.Enabled = false;

                }
            }

            else if (B_Guardar.Text == "Modificar")
            {
                //if (T_Cod_banco.Text == "")
                //{
                //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab3", "<script language='javascript'>alert('El Codigo de la categoría no puede estar vacío');</script>", false);
                //}
                if (T_Nom_banco.Text == "")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "mant_lab4", "<script language='javascript'>alert('El Nombre de la categoría no puede estar vacío');</script>", false);
                }
                else
                {
                    // GUARDAR
                    DBUtil db = new DBUtil();
                    string usuario = Session["user"].ToString();
                    string query = "";

                    query += "UPDATE categoria SET ";
                    query += "NOM_CATEG = @NOM_CATEG  ";
                    query += " WHERE ID = @ID";

                    List<SPVars> vars = new List<SPVars>();
                    vars.Add(new SPVars() { nombre = "NOM_CATEG", valor = T_Nom_banco.Text.ToString().Trim() });
                    vars.Add(new SPVars() { nombre = "ID", valor = T_ID_BANCO.Text.ToString().Trim() });

                    db.Scalar2(query, vars);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    T_Cod_banco.Text = String.Empty;
                    T_Nom_banco.Text = String.Empty;
                    T_Cod_banco.Enabled = false;
                    T_Nom_banco.Enabled = false;

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

                    db.Scalar("delete from categoria where id = " + id);
                    CargarGrilla();

                    B_Guardar.Visible = false;
                    btn_nuevo_banco.Visible = true;
                    T_Cod_banco.Text = String.Empty;
                    T_Nom_banco.Text = String.Empty;
                    T_Cod_banco.Enabled = false;
                    T_Nom_banco.Enabled = false;

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

                    dt = db.consultar("select NOM_CATEG from categoria where id = " + id);
                    if (dt != null)
                    {
                        DataRow dr = dt.Rows[0];
                        T_Nom_banco.Text = dr[0].ToString();
                        //T_Cod_banco.Enabled = true;
                        T_Nom_banco.Enabled = true;
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
            G_Banco.DataSource = db.consultar("Select * from categoria");
            G_Banco.DataBind();
            //modal_unidad
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {

        }

        protected void G_Banco_DataBound(object sender, GridViewRowEventArgs e)
        {
            // aca
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                string id_categoria = G_Banco.DataKeys[e.Row.RowIndex].Values[0].ToString();

                string script = string.Format("javascript:abremodal_(&#39;{0}&#39;,&#39;{1}&#39;);return false;", id_categoria, e.Row.Cells[3].Text);
                e.Row.Cells[3].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[3].Text + " </a>";
            }
        }

        private void cargar_productos_sin_categ()
        {
            //                " and FechaFactura <= CONVERT(datetime,'" + HASTA + "',103) and user1 in (" + grupos + ")";


            DataTable dt; DataView dtv = new DataView();
            DBUtil db = new DBUtil();
            dt = db.consultar("Select producto, descproducto from V_PRODUCTO_CATEGORIA where categoria = (select id from categoria where nom_categ like '%Resto%')");
            dtv = dt.DefaultView;
            cb_productos_kg.DataSource = dtv;
            cb_productos_kg.DataTextField = "descproducto";
            cb_productos_kg.DataValueField = "producto";
            //d_vendedor_.SelectedIndex = -1;
            cb_productos_kg.DataBind();
        }

        public class PRODUCTO
        {
            public string producto { get; set; }
            public string descproducto { get; set; }

        }

        [WebMethod]
        public static List<PRODUCTO> PRODUCTO_CB(string sw)
        {
            DataTable dt = new DataTable();


            string clase = "";

            try
            {
                DBUtil db = new DBUtil();
                dt = db.consultar("Select producto, descproducto from V_PRODUCTO_CATEGORIA where categoria = (select id from categoria where nom_categ like '%Resto%') "); ;
                ////dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
                DataView dv = dt.DefaultView;
                dv.Sort = "descproducto";
                dt = dv.ToTable();
            }
            catch { }
            //}
            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new PRODUCTO
                        {
                            producto = Convert.ToString(item["producto"]),
                            descproducto = Convert.ToString(item["descproducto"])
                        };
            return query.ToList<PRODUCTO>();
        }
        public class Evento_objeto
        {
            public string tabla_html { get; set; }


        }

        [WebMethod]
        public static List<Evento_objeto> CargarProductos(string categ)
        {
            DataTable dt = new DataTable();

            DBUtil db = new DBUtil();

            string tabla = "";
            dt = db.consultar("SELECT * from V_PRODUCTO_CATEGORIA where categoria = " + categ);


            if (dt.Rows.Count <= 0) { tabla = "<h3>No hay productos en esta categoría</h3>"; }
            else {

                tabla += "<table class=\"table fill-head table-bordered\">";
                tabla += "<thead class=\"test\">";
                tabla += "<tr>";
                tabla += "<th>Quitar</th>";
                tabla += "<th>Productos</th>";
                tabla += "</tr>";
                tabla += "</thead>";
                tabla += "<tbody>";
                foreach (DataRow dr in dt.Rows)
                {
                    tabla += "<tr>";

                    string script2 = string.Format("javascript:QuitarProducto(&#39;{0}&#39;)", dr["producto"].ToString().Trim());

                    tabla += "<td> <a style='background-color: rgb(255, 97, 97);' class=\"btn btn-circle show-tooltip fa fa-trash\" onclick=\"" + script2 + "\"></a> </td>";
                    tabla += "<td>" + dr["descproducto"].ToString() + "</td>";
                    tabla += "</tr>";

                }
                tabla += "</tbody>";
                tabla += "</table>";
                tabla = tabla.Replace("'", "");

            }

            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new Evento_objeto
                        {
                            tabla_html = tabla,

                        };

            return query.ToList<Evento_objeto>();
        }


        [WebMethod]
        public static string INSERT_PRODUCTO(string id_cat, string id_prod)
        {

            DataTable dt = new DataTable();
            string respt = "";
            try
            {

                DBUtil db = new DBUtil();
                string query = "";

                query += "INSERT INTO categoria_producto ( ";
                query += " id_categoria, id_producto  ";


                query += ") VALUES ( ";
                query += " @id_categoria, @id_producto ";
                query += " );";

                List<SPVars> vars = new List<SPVars>();
                vars.Add(new SPVars() { nombre = "id_categoria", valor = id_cat.ToString().Trim() });
                vars.Add(new SPVars() { nombre = "id_producto", valor = id_prod.ToString().Trim() });

                db.Scalar2(query, vars);

                respt = "ok";
            }
            catch { respt = "Error :02 Al guardar Producto"; }
            //}

            return respt;
        }
        [WebMethod]
        public static string DELETE_PRODUCTO(string id_prod)
        {

            DataTable dt = new DataTable();
            string respt = "";
            try
            {

                DBUtil db = new DBUtil();
                string query = "";

                query += "DELETE FROM categoria_producto WHERE ";
                query += " id_producto = @id_prod ";

                List<SPVars> vars = new List<SPVars>();
                vars.Add(new SPVars() { nombre = "id_prod", valor = id_prod.ToString().Trim() });


                db.Scalar2(query, vars);

                respt = "ok";
            }
            catch { respt = "Error :02 Al eliminar Producto"; }
            //}

            return respt;
        }


    }
}