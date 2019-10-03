
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoprodiApp.negocio;
using SoprodiApp.entidad;
using System.Web.Services;

namespace SoprodiApp
{
    /// <summary>
    /// Inicio del código
    /// </summary>
    public partial class REPORTE_USUARIOS : System.Web.UI.Page
    {
        private static string COD_USUARIO;
        private static bool es_vendedor_2;
        private static string unidad_;
        private static string grupo_;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.RegisterRedirectOnSessionEndScript();
            if (!IsPostBack)
            {
                List<string> u_negocio = ReporteRNegocio.trae_u_negocio(User.Identity.Name).Split(',').ToList();
                bool correcto_app = false;
                foreach (string u_ne in u_negocio)
                {
                    if (u_ne.Trim() == "5")
                    {
                        correcto_app = true;
                    }
                }
                if (!correcto_app)
                {
                    Response.Redirect("MENU.aspx");
                }

                titulo2.InnerHtml = "<i class='fa fa-file-o fa-3x'></i> <a class='h1' href='REPORTE_USUARIOS.aspx?'>Usuarios</a>";
                titulo2.InnerHtml = "Usuarios";

                string es_vendedor = ReporteRNegocio.esvendedor(User.Identity.Name.ToString());
                if (es_vendedor == "2")
                {
                    Session["bool_es_vendedor_2"] = true;
                    //es_vendedor_2 = true;
                    Response.Redirect("REPORTES.aspx?s=3");
                }
                else
                {
                    Session["bool_es_vendedor_2"] = false;
                    //es_vendedor_2 = false;
                }
                cargar_usuarios();
            }
        }

        private void cargar_unidad_negocio()
        {

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.carga_unidad_negocio();
            dtv = dt.DefaultView;
            dtv.Sort = "nombre_unineg";
            d_unidade_negocio.DataSource = dtv;
            d_unidade_negocio.DataTextField = "nombre_unineg";
            d_unidade_negocio.DataValueField = "cod_unineg";
            //d_vendedor_.SelectedIndex = -1;
            d_unidade_negocio.DataBind();
        }

        private void cargar_usuarios()
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>vaciar_editar();</script>", false);

            DataTable dt;
            dt = ReporteRNegocio.cargar_usuarios();
            G_usuarios.DataSource = dt;
            G_usuarios.DataBind();

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teewqqwwqee", "<script> new Tablesort(document.getElementById('ContentPlaceHolder_Contenido_G_usuarios')); </script>", false);

        }
        public string confirmDelete(string Name)
        {
            return @"javascript:if(!confirm('Esta acción va eliminar al usuario: "
               + Name.ToUpper()
               + @". ¿Estás seguro?')){return false;}";
        }

        protected void G_usuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[5].Text == "1")
                {
                    e.Row.Cells[5].Text = "ADMIN";
                }
                else if (e.Row.Cells[5].Text == "2")
                {
                    e.Row.Cells[5].Text = "VENDEN";
                    //e.Row.Cells[2].Text = "(" + ReporteRNegocio.nombre_vendedor(e.Row.Cells[2].Text) + ")   " + e.Row.Cells[2].Text;
                }
                else { e.Row.Cells[5].Text = "OTRO"; }
            }
            if (e.Row.Cells[7].Text.Split(',').ToList().Count == ReporteRNegocio.carga_grupos().Rows.Count)
            {
                e.Row.Cells[7].Text = "TODOS";
            }
        }

        protected void b_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["SW_CLICK"].ToString() == "NUEVO") { NUEVO_GUARDAR(); }

            else if (Session["SW_CLICK"].ToString() == "EDITAR") { EDITAR_GUARDAR(); }

        }

        protected void b_2_Click(object sender, ImageClickEventArgs e)
        {

            string pantallas = l_pantallas.Text;
            string unidad_negocio = l_unidad_negocio.Text;
            string grupo = l_grupos.Text;
        }


        private void EDITAR_GUARDAR()
        {
            if (txt_cod_usuario.Text == "")
            {
                h3_.InnerText = "Falta completar datos"; if (!chk_adm.Checked)
                {
                    DIV2_3.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                    DIV222.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                }
            }
            else if (chk_otro.Checked && l_unidad_negocio.Text == "")
            {
                h3_.InnerText = "Falta completar datos"; if (!chk_adm.Checked)
                {
                    DIV2_3.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                    DIV222.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                }
            }
            else if (chk_otro.Checked && l_unidad_negocio.Text == "1" && l_grupos.Text == "" || chk_otro.Checked && l_unidad_negocio.Text == "2" && l_grupos.Text == "")
            {
                h3_.InnerText = "Falta completar datos"; if (!chk_adm.Checked)
                {
                    DIV2_3.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                    DIV222.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                }
            }
            else if (chk_otro.Checked && l_pantallas.Text == "")
            {
                h3_.InnerText = "Falta completar datos"; if (!chk_adm.Checked)
                {
                    DIV2_3.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                    DIV222.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                }
            }
            else if (chk_vend.Checked && l_pantallas.Text == "")
            {
                h3_.InnerText = "Falta completar datos"; if (!chk_adm.Checked)
                {
                    DIV2_3.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                    DIV222.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                }
            }
            //else if (r_enviar_correo.Checked && t_correo.Text == "" || r_enviar_2.Checked && t_correo.Text == "")
            //{
            //    h3_.InnerText = "Falta completar datos"; if (!chk_adm.Checked)
            //    {
            //        DIV2_3.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
            //        DIV222.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
            //    }
            //}
            else
            {
                usuarioEntity us = new usuarioEntity();

                us.cod_usuario = txt_cod_usuario.Text;

                if (txt_clave.Text != "")
                {
                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("Somos los mas solomon de quillota 2015");
                    string contrasena = encriptador.EncryptData(txt_clave.Text.ToUpper());
                    us.clave = contrasena;
                }
                else
                {
                    //cambiar variable estatica
                    string cod_usuario_session = Session["COD_USUARIO"].ToString();
                    //us.clave = ReporteRNegocio.es_su_pass(COD_USUARIO);
                    us.clave = ReporteRNegocio.es_su_pass(cod_usuario_session);
                }

                if (chk_adm.Checked)
                {
                    us.admin = "true";
                }
                else
                {
                    us.admin = "false";
                }

                if (chk_vend.Checked) { us.tipo_usuario = 2; us.nombre_ = ReporteRNegocio.nombre_vendedor(us.cod_usuario); }
                else if (chk_otro.Checked) { us.tipo_usuario = 3; us.nombre_ = t_nombre_us.Text; }
                else { us.tipo_usuario = 1; us.nombre_ = t_nombre_us.Text; }

                if (r_activo.Checked) { us.activado = "true"; } else { us.activado = "false"; }
                if (r_enviar_2.Checked) { us.enviar2 = "true"; } else { us.enviar2 = "false"; }
                if (r_enviar_correo.Checked) { us.enviar = "true"; } else { us.enviar = "false"; }

                us.correo = t_correo.Text;
                us.cc = t_cc.Text;

                if (chk_adm.Checked)
                {
                    l_unidad_negocio.Text = ReporteRNegocio.obtiene_todos_u_negocio();
                    l_pantallas.Text = ReporteRNegocio.obtiene_todos_app();
                    l_grupos.Text = ReporteRNegocio.obtiene_todos_grupos();
                }

                us.u_negocio = l_unidad_negocio.Text;
                us.grupos = l_grupos.Text;
                us.app = l_pantallas.Text;
                string cod_usuario_ = txt_cod_usuario.Text;
                
                //if (usuarioBO.actualizar(us, COD_USUARIO) == "OK")
                if (usuarioBO.actualizar(us, cod_usuario_) == "OK")
                {
                    h3_.InnerText = "Usuario Editado.";
                    if (!chk_adm.Checked)
                    {
                        DIV2_3.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                        DIV222.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                    }
                }
                else { h3_.InnerText = "Falló al Editar."; }

            }
        }

        private void NUEVO_GUARDAR()
        {
            if (txt_cod_usuario.Text == "")
            {
                h3_.InnerText = "Falta completar datos"; if (!chk_adm.Checked)
                {
                    DIV2_3.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                    DIV222.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                }
            }
            else if (txt_clave.Text == "")
            {
                h3_.InnerText = "Falta completar datos"; if (!chk_adm.Checked)
                {
                    DIV2_3.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                    DIV222.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                }
            }
            else if (chk_otro.Checked && l_unidad_negocio.Text == "")
            {
                h3_.InnerText = "Falta completar datos"; if (!chk_adm.Checked)
                {
                    DIV2_3.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                    DIV222.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                }
            }
            else if (chk_otro.Checked && l_unidad_negocio.Text == "1" && l_grupos.Text == "" || chk_otro.Checked && l_unidad_negocio.Text == "2" && l_grupos.Text == "")
            {
                h3_.InnerText = "Falta completar datos"; if (!chk_adm.Checked)
                {
                    DIV2_3.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                    DIV222.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                }
            }
            else if (chk_otro.Checked && l_pantallas.Text == "")
            {
                h3_.InnerText = "Falta completar datos"; if (!chk_adm.Checked)
                {
                    DIV2_3.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                    DIV222.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                }
            }
            else if (chk_vend.Checked && l_pantallas.Text == "")
            {
                h3_.InnerText = "Falta completar datos"; if (!chk_adm.Checked)
                {
                    DIV2_3.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                    DIV222.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                }
            }
            else if (r_enviar_correo.Checked && t_correo.Text == "" || r_enviar_2.Checked && t_correo.Text == "")
            {
                h3_.InnerText = "Falta completar datos"; if (!chk_adm.Checked)
                {
                    DIV2_3.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                    DIV222.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                }
            }
            else
            {
                usuarioEntity us = new usuarioEntity();
                us.cod_usuario = txt_cod_usuario.Text;

                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("Somos los mas solomon de quillota 2015");
                string contrasena = encriptador.EncryptData(txt_clave.Text.ToUpper());

                us.clave = contrasena;
                if (chk_adm.Checked)
                {
                    us.admin = "true";
                }
                else
                {
                    us.admin = "false";
                }

                if (chk_vend.Checked) { us.tipo_usuario = 2; us.nombre_ = ReporteRNegocio.nombre_vendedor(us.cod_usuario); }
                else if (chk_otro.Checked) { us.tipo_usuario = 3; us.nombre_ = t_nombre_us.Text; }
                else { us.tipo_usuario = 1; us.nombre_ = t_nombre_us.Text; }

                if (r_activo.Checked) { us.activado = "true"; } else { us.activado = "false"; }
                if (r_enviar_2.Checked) { us.enviar2 = "true"; } else { us.enviar2 = "false"; }
                if (r_enviar_correo.Checked) { us.enviar = "True"; } else { us.enviar = "False"; }


                if (usuarioBO.encontrar(ref us) == "OK")
                {
                    //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>repetido_agregar_adm();</script>", false);
                    h3_.InnerText = "Usuario ya se encuentra.";
                    cargar_usuarios();
                    return;
                }
                else
                {

                    us.correo = t_correo.Text;
                    us.cc = t_cc.Text;

                    if (chk_adm.Checked)
                    {
                        l_unidad_negocio.Text = ReporteRNegocio.obtiene_todos_u_negocio();
                        l_pantallas.Text = ReporteRNegocio.obtiene_todos_app();
                        l_grupos.Text = ReporteRNegocio.obtiene_todos_grupos();
                    }

                    us.u_negocio = l_unidad_negocio.Text;
                    us.grupos = l_grupos.Text;
                    us.app = l_pantallas.Text;

                    if (usuarioBO.registrar(us) == "OK")
                    {
                        h3_.InnerText = "Usuario Agregado.";
                        //chk_adm.Checked = true;
                        t_cc.Text = "";
                        r_enviar_2.Checked = false;
                        DIV222.Attributes["style"] = "display:none;";
                        l_unidad_negocio.Text = "";
                        l_pantallas.Text = "";
                        l_grupos.Text = "";
                        DIV2_3.Attributes["style"] = "display:none;";
                        txt_clave.Text = "";
                        txt_cod_usuario.Text = "";
                        t_correo.Text = "";
                        r_enviar_correo.Checked = false;
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>vaciar_editar();</script>", false);
                        cargar_usuarios();
                    }

                }
            }

        }

        protected void G_usuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delete_")
            {

                usuarioEntity us = new usuarioEntity();



                us.cod_usuario = G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                string es_adm = usuarioBO.obtener_adm(us.cod_usuario);
                if (es_adm != "True")
                {
                    if (usuarioBO.eliminar(us) == "OK")
                    {
                        h3_.InnerText = "Usuario Eliminado.";
                        //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>delete_adm();</script>", false);
                    }
                    cargar_usuarios();
                }
                else { h3_.InnerText = "No puede eliminar un usuario Administrador."; }
                txt_clave.Text = "";
                txt_cod_usuario.Text = "";
                t_correo.Text = "";
                r_enviar_correo.Checked = false;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>vaciar_editar();</script>", false);
            }

            if (e.CommandName == "Editar")
            {

                txt_cod_usuario.Enabled = false;


                //chk_adm.Attributes["AutoPostBack"] = "True";
                h3_.InnerText = "Editar Usuario";
                Session["SW_CLICK"] = "EDITAR";
                USUARIOS.ActiveViewIndex = 1;

                //COD_USUARIO = G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();
                Session["COD_USUARIO"] = G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();

                l_grupos.Text = G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[4].ToString();
                l_unidad_negocio.Text = G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[9].ToString();
                l_pantallas.Text = G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[11].ToString();

                t_correo.Text = G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[6].ToString();
                t_cc.Text = G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[8].ToString();
                txt_cod_usuario.Text = G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0].ToString();

                t_nombre_us.Text = G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[12].ToString();

                txt_clave.Text = "";
                l_grupos.Text = G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[4].ToString();
                if (G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[2].ToString() == "True")
                {
                    r_activo.Checked = true;
                }
                else { r_activo.Checked = false; }

                if (G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[5].ToString() == "True")
                {
                    r_enviar_correo.Checked = true;
                }
                else { r_enviar_correo.Checked = false; }
                if (G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[7].ToString() == "True")
                {
                    r_enviar_2.Checked = true;
                }
                else { r_enviar_2.Checked = false; }

                if (G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[10].ToString() == "1")
                {
                    //es_vendedor_2 = false;
                    Session["bool_es_vendedor_2"] = false;
                    chk_adm.Checked = true;
                    chk_vend.Checked = false;
                    chk_otro.Checked = false;
                    DIV2_3.Attributes["style"] = "display:none;";
                    DIV222.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:none;";
                    DIV555.Attributes["style"] = "display:block;";

                }
                else if (G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[10].ToString() == "2")
                {
                    //es_vendedor_2 = true;
                    Session["bool_es_vendedor_2"] = true;
                    //unidad_ = ReporteRNegocio.trae_u_negocio(txt_cod_usuario.Text.Trim());
                    Session["unidad_"] = ReporteRNegocio.trae_u_negocio(txt_cod_usuario.Text.Trim());
                    string negocio_aux = l_unidad_negocio.Text;
                    DIV555.Attributes["style"] = "display:none;";
                    chk_adm.Checked = false;
                    chk_vend.Checked = true;
                    chk_otro.Checked = false;
                    string grupo = ReporteRNegocio.grupos_usuario_v_report(txt_cod_usuario.Text).Trim();
                    Session["grupo_"] = grupo;
                    if (grupo.Contains("Abarrotes") || grupo.Contains("Granos")) { ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>muestra_check();</script>", false); }
                    cargar_unidad_negocio_vend(grupo);
                    l_unidad_negocio.Text = negocio_aux;
                    cargar_grupo_vend(grupo);
                    cargar_pantallas_vend(grupo);
                    DIV222.Attributes["style"] = "display:block;";
                    DIV2_3.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                    string cod_un = "";
                    if (G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[9].ToString() == "")
                    {
                        cod_un = ReporteRNegocio.cod_unineg(grupo);
                    }
                    else
                    {
                        cod_un = G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[9].ToString();
                    }
                    string where = " where cod_unineg in (" + cod_un + ") ";
                    DataTable app = ReporteRNegocio.carga_app_unidad(where);
                    cargar_app_uni(app);

                    foreach (ListItem item in d_unidade_negocio.Items)
                    {
                        List<string> app2 = G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[9].ToString().Split(',').ToList();
                        foreach (string x in app2)
                        {
                            if (x.Trim() == item.Value.Trim())
                            {
                                item.Selected = true;
                            }
                        }
                    }
                    foreach (ListItem item in d_pantallas.Items)
                    {
                        List<string> app2 = G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[11].ToString().Split(',').ToList();
                        foreach (string x in app2)
                        {
                            if (x.Trim() == item.Value.Trim())
                            {
                                item.Selected = true;
                            }
                        }
                    }

                }
                else
                {
                    //es_vendedor_2 = false;
                    Session["bool_es_vendedor_2"] = false;
                    chk_adm.Checked = false;
                    chk_vend.Checked = false;
                    chk_otro.Checked = true;

                    string grupo = ReporteRNegocio.grupos_usuario(txt_cod_usuario.Text).Trim();
                    string u_negoc = ReporteRNegocio.negocio_usuario(txt_cod_usuario.Text).Trim();
                    if (u_negoc == "" && grupo != "")
                    {
                        u_negoc = ReporteRNegocio.negocio_usuario_por_grupos(agregra_comillas(grupo));

                    }
                    if (grupo.Contains("Abarrotes") || grupo.Contains("Granos")) { ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>muestra_check();</script>", false); }

                    DIV2_3.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                    DIV222.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                    DIV555.Attributes["style"] = "display:block;";
                    cargar_unidad_negocio();


                    foreach (ListItem item in d_unidade_negocio.Items)
                    {
                        List<string> app2 = u_negoc.Split(',').ToList();
                        foreach (string x in app2)
                        {
                            if (x.Trim() == item.Value.Trim())
                            {
                                item.Selected = true;
                            }
                        }
                    }

                    l_unidad_negocio.Text = u_negoc;
                    string where = " where cod_unineg in (" + l_unidad_negocio.Text + ") ";
                    if (grupo == "")
                    {
                        grupo = "-1";

                    }
                    else
                    {

                        DataTable grupos = ReporteRNegocio.carga_grupo_unidad(" where cod_grupo in (" + agregra_comillas(grupo) + ")");
                        cargar_grupo_uni(grupos);
                        foreach (ListItem item in d_grupos.Items)
                        {

                            if (G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[4].ToString().Contains(item.ToString()))
                            {
                                item.Selected = true;
                            }
                        }
                    }
                    if (u_negoc == "")
                    {
                        where = " where 1=2 ";

                    }

                    DataTable app = ReporteRNegocio.carga_app_unidad(where);
                    cargar_app_uni(app);


                    foreach (ListItem item in d_pantallas.Items)
                    {
                        List<string> app2 = G_usuarios.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[11].ToString().Split(',').ToList();
                        foreach (string x in app2)
                        {
                            if (x.Trim() == item.Value.Trim())
                            {
                                item.Selected = true;
                            }
                        }
                    }

                }


            }
        }
        private string agregra_comillas(string p)
        {
            if (p != "")
            {
                if (p.Substring(0, 1) != "'")
                {
                    string final_comillas = "";
                    if (p.Contains(","))
                    {
                        List<string> LIST = new List<string>();
                        List<string> LIST2 = new List<string>();
                        LIST.AddRange(p.Split(','));
                        foreach (string a in LIST)
                        {
                            LIST2.Add("'" + a.Trim() + "'");
                        }
                        final_comillas = string.Join(",", LIST2);
                    }
                    else { final_comillas = "'" + p.Trim() + "'"; }
                    return final_comillas;
                }

                else { return p; }
            }
            else { return p; }
        }

        private void cargar_unidad_negocio(string p)
        {

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.carga_unidad_otro(p);
            if (dt.Rows.Count <= 0) { dt = ReporteRNegocio.carga_unidad_negocio(); }
            foreach (DataRow r in dt.Rows) { l_unidad_negocio.Text = r[0].ToString(); }
            dtv = dt.DefaultView;
            dtv.Sort = "nombre_unineg";
            d_unidade_negocio.DataSource = dtv;
            d_unidade_negocio.DataTextField = "nombre_unineg";
            d_unidade_negocio.DataValueField = "cod_unineg";
            d_unidade_negocio.DataBind();


        }





        protected void G_usuarios_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {

        }

        protected void btn_editar_Click(object sender, EventArgs e)
        {

        }
        //ACA

        public class GRUPOS
        {
            public string COD_GRUPO { get; set; }

        }

        public class app
        {
            public string cod_app { get; set; }
            public string nom_app { get; set; }
        }

        [WebMethod]
        public static List<GRUPOS> GRUPOS_POR_UNIDAD(string u_negocio)
        {
            DataTable dt = new DataTable();

            string where = "";
            if (u_negocio == "") { u_negocio = "-5"; }
            bool es_vendedor_2_session = (bool)HttpContext.Current.Session["bool_es_vendedor_2"];
            if (!es_vendedor_2_session)
            {
                where = " where cod_unineg in (" + u_negocio + ") ";
            }
            else
            {

                where = " where cod_unineg in (" + HttpContext.Current.Session["unidad_"].ToString() + ") and cod_grupo = '" + HttpContext.Current.Session["grupo_"].ToString() + "'";
            }

            try
            {
                dt = ReporteRNegocio.carga_grupo_unidad(where);
                ////dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
                DataView dv = dt.DefaultView;
                dv.Sort = "cod_app";
                dt = dv.ToTable();
            }
            catch { }

            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new GRUPOS
                        {
                            COD_GRUPO = Convert.ToString(item["COD_GRUPO"]),

                        };

            return query.ToList<GRUPOS>();
        }


        [WebMethod]
        public static List<app> APP_POR_UNIDAD(string u_negocio)
        {
            DataTable dt = new DataTable();

            if (u_negocio == "") { u_negocio = "-1"; }
            string where = " where cod_unineg in (" + u_negocio + ") ";

            try
            {
                dt = ReporteRNegocio.carga_app_unidad(where);
                ////dt.Rows.Add(new Object[] { "-1", "-- Todos --" });
                DataView dv = dt.DefaultView;
                dv.Sort = "cod_app";
                dt = dv.ToTable();
            }
            catch { }
            //}
            var query = from item in dt.AsEnumerable()
                        where 1 == 1
                        select new app
                        {
                            cod_app = Convert.ToString(item["cod_app"]),
                            nom_app = Convert.ToString(item["nom_app"])
                        };
            return query.ToList<app>();
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            usuarioEntity us = new usuarioEntity();
            us.cod_usuario = txt_cod_usuario_edit.Text;

            us.correo = t_correo_editar.Text;
            if (txt_clave_edit.Text != "")
            {
                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("Somos los mas solomon de quillota 2015");
                string contrasena = encriptador.EncryptData(txt_clave_edit.Text.ToUpper());
                us.clave = contrasena;
            }
            else
            {
                us.clave = ReporteRNegocio.es_su_pass(usuario.Text);
            }
            if (activado.Checked)
            {
                us.activado = "true";
            }
            else { us.activado = "false"; }


            if (chk_enviar_correo.Checked)
            {
                us.enviar = "true";
            }
            else { us.enviar = "false"; }

            bool elimina = false;
            if (chk_vend2.Checked) { us.tipo_usuario = 2; l_unidad_negocio.Text = ""; elimina = true; }
            else if (chk_otro2.Checked) { us.tipo_usuario = 3; }
            else { us.tipo_usuario = 1; }



            if (usuarioBO.actualizar(us, usuario.Text) == "OK")
            {
                if (elimina)
                {
                    try
                    {
                        usuarioBO.eliminar_grupos_usuario(usuario.Text);
                    }
                    catch { }
                }

                if (l_unidad_negocio.Text != "")
                {
                    try
                    {
                        usuarioBO.eliminar_grupos_usuario(usuario.Text);
                    }
                    catch { }
                    if (l_unidad_negocio.Text.Contains(","))
                    {
                        if (l_unidad_negocio.Text.Contains("-- Todos --"))
                        {
                            foreach (ListItem item in d_grupos_2.Items)
                            {
                                usuarioBO.registrar_det(us.cod_usuario, item.ToString());
                            }

                        }
                        else
                        {

                            string[] grupos = l_unidad_negocio.Text.Split(',');

                            foreach (string grupo in grupos)
                            {
                                usuarioBO.registrar_det(us.cod_usuario, grupo);
                            }
                        }
                    }
                    else
                    {
                        if (l_unidad_negocio.Text == "-- Todos --")
                        {
                            foreach (ListItem item in d_grupos_2.Items)
                            {
                                if (item.ToString() != "-- Todos --")
                                {
                                    usuarioBO.registrar_det(us.cod_usuario, item.ToString());
                                }
                            }
                        }
                        else
                        {
                            usuarioBO.registrar_det(us.cod_usuario, l_unidad_negocio.Text);
                        }
                    }
                }


                //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>editado();</script>", false);
                h3_.InnerText = "Usuario Editado.";
                cargar_usuarios();
            }

            else
            {
                //ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>no_editado();</script>", false);
                h3_.InnerText = "Error al Editar, verifique los datos";
            }
            div_editar_usuario.Visible = false;
            fondo_modal.Visible = false;
            cargar_usuarios();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            cargar_usuarios();
            div_editar_usuario.Visible = false;
            fondo_modal.Visible = false;
        }

        protected void chk_otro_CheckedChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>muestra_check2();</script>", false);
            DIV555.Attributes["style"] = "display:block;";
            DIV2_3.Attributes["style"] = "display:none;";
            DIV222.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
            div_grupos.Visible = true;
            cargar_unidad_negocio();
        }
        protected void chk_adm_CheckedChanged1(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>muestra_check();</script>", false);
            l_grupos.Text = "";
            l_unidad_negocio.Text = "";
            l_pantallas.Text = "";
            DIV2_3.Attributes["style"] = "display:none;";
            DIV222.Attributes["style"] = "display:none;";
            DIV555.Attributes["style"] = "display:block;";
        }

        protected void chk_vend_CheckedChanged(object sender, EventArgs e)
        {

            if (txt_cod_usuario.Text != "" && ReporteRNegocio.grupos_usuario_v_report(txt_cod_usuario.Text).Trim() != "")
            {
                string grupo = ReporteRNegocio.grupos_usuario_v_report(txt_cod_usuario.Text).Trim();
                if (grupo.Contains("Abarrotes") || grupo.Contains("Granos")) { ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>muestra_check();</script>", false); }
                cargar_unidad_negocio_vend(grupo);
                cargar_grupo_vend(grupo);
                cargar_pantallas_vend(grupo);
                DIV222.Attributes["style"] = "display:block;";
                l_pantallas.Text = "";
                DIV2_3.Attributes["style"] = "margin-right: 0px; margin-left: 0px; display:block;";
                DIV555.Attributes["style"] = "display:none;";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>registre_vended();</script>", false);
                chk_vend.Checked = false;
                DIV2_3.Attributes["style"] = "display:none;";
                DIV222.Attributes["style"] = "display:none;";
                DIV555.Attributes["style"] = "display:block;";
            }


        }
        private void cargar_grupo_uni(DataTable grupos)
        {
            DataView dtv = new DataView();
            dtv = grupos.DefaultView;
            dtv.Sort = "cod_grupo";
            d_grupos.DataSource = dtv;
            d_grupos.DataTextField = "cod_grupo";
            d_grupos.DataValueField = "cod_grupo";
            d_grupos.DataBind();

        }

        private void cargar_app_uni(DataTable app)
        {
            DataView dtv = new DataView();
            dtv = app.DefaultView;
            dtv.Sort = "nom_app";
            d_pantallas.DataSource = dtv;
            d_pantallas.DataTextField = "nom_app";
            d_pantallas.DataValueField = "cod_app";
            d_pantallas.DataBind();

        }


        private void cargar_pantallas_vend(string grupo)
        {
            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.carga_pantallas_vend(grupo);
            dtv = dt.DefaultView;
            dtv.Sort = "nom_app";
            d_pantallas.DataSource = dtv;
            d_pantallas.DataTextField = "nom_app";
            d_pantallas.DataValueField = "cod_app";
            d_pantallas.DataBind();

        }

        private void cargar_grupo_vend(string grupo)
        {

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.carga_grupo_vend(grupo);
            foreach (DataRow r in dt.Rows) { l_grupos.Text = r[1].ToString(); }
            dtv = dt.DefaultView;
            dtv.Sort = "cod_grupo";
            d_grupos.DataSource = dtv;
            d_grupos.DataTextField = "cod_grupo";
            d_grupos.DataValueField = "cod_grupo";
            d_grupos.DataBind();
            d_grupos.SelectedIndex = 0;

        }

        private void cargar_unidad_negocio_vend(string grupo)
        {

            DataTable dt; DataView dtv = new DataView();
            dt = ReporteRNegocio.carga_unidad_vend(grupo);
            foreach (DataRow r in dt.Rows)
            {
                l_unidad_negocio.Text = r[0].ToString(); break;
            }
            dtv = dt.DefaultView;
            dtv.Sort = "nombre_unineg";
            d_unidade_negocio.DataSource = dtv;
            d_unidade_negocio.DataTextField = "nombre_unineg";
            d_unidade_negocio.DataValueField = "cod_unineg";
            d_unidade_negocio.DataBind();
            d_unidade_negocio.SelectedIndex = 0;
        }




        protected void chk_adm2_CheckedChanged(object sender, EventArgs e)
        {
            div_grupos2.Visible = true;
        }

        protected void chk_vend2_CheckedChanged(object sender, EventArgs e)
        {
            div_grupos2.Visible = false;

        }

        protected void chk_otro2_CheckedChanged(object sender, EventArgs e)
        {
            div_grupos2.Visible = true;
        }

        protected void btn_nuevo_usuario_ServerClick(object sender, EventArgs e)
        {
            txt_cod_usuario.Enabled = true ;

            h3_.InnerText = "Crear Usuario";
            USUARIOS.ActiveViewIndex = 1;
            Session["SW_CLICK"] = "NUEVO";
            t_cc.Text = "";
            r_enviar_2.Checked = false;
            DIV222.Attributes["style"] = "display:none;";
            l_unidad_negocio.Text = "";
            l_pantallas.Text = "";
            l_grupos.Text = "";
            DIV2_3.Attributes["style"] = "display:none;";
            txt_clave.Text = "";
            txt_cod_usuario.Text = "";
            t_correo.Text = "";
            r_enviar_correo.Checked = false;
            t_nombre_us.Text = "";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeee", "<script language='javascript'>vaciar_editar();</script>", false);
            chk_adm.Checked = true;
            chk_adm2.Checked = true;
            chk_vend.Checked = false;
            chk_otro.Checked = false;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ts", "<script language='javascript'>chk_adm();</script>", false);
        }

        protected void btn_volver_usuario_ServerClick(object sender, EventArgs e)
        {
            USUARIOS.ActiveViewIndex = 0;
            cargar_usuarios();

        }


    }
}