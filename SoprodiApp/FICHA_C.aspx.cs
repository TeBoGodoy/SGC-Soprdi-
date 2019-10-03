using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoprodiApp.negocio;
using SoprodiApp.entidad;
using System.Net.Mail;
using System.Net;

namespace SoprodiApp
{
    public partial class FICHA_C : System.Web.UI.Page
    {
        private static string USER;

        public static bool contacto_extra = false;
        public static bool cta_extra = false;
        public static bool socio_extra = false;
        private static string GRUPOS_USUARIO;
        public static string pais = "";
        public static string ciudad = "";


        public static string pais_so = "";
        public static string ciudad_so = "";




        public static string clie_rut = "";
        public static string clie_nombre = "";
        public static string clie_ciudad = "";
        public static string clie_pais = "";
        public static string codvendedor = "";
        public static string clie_direcc = "";
        public static string clie_fono = "";
        public static string estado = "";
        public static string socie_nomb = "";
        public static string socie_rut = "";
        public static string socie_direcc = "";
        public static string socie_ciudad = "";
        public static string socie_pais = "";
        public static string socie_fono = "";
        public static string socie_correo = "";
        public static string clie_giro = "";
        public static string clie_tipo_giro = "";
        public static string desde_clie = "";
        public static string fecha_creacion = "";
        public static string credito_actual1 = "";
        public static string tipo_credito_actual1 = "";
        public static string monto_credito_actual1 = "";
        public static string credito_solicit1 = "";
        public static string tipo_credito_solicit1 = "";
        public static string monto_credito_solicit1 = "";
        public static string contact_id1 = "";
        public static string contact_nombre1 = "";
        public static string contact_correo1 = "";
        public static string contact_cargo1 = "";
        public static string contact_fono1 = "";
        public static string banco_id1 = "";
        public static string banco1 = "";
        public static string cta1 = "";
        public static string socio_id1 = "";
        public static string socio_nombre1 = "";
        public static string socio_rut1 = "";
        public static string socio_correo1 = "";
        public static string socio_porcen1 = "";

        public static string contact_id2 = "";
        public static string contact_nombre2 = "";
        public static string contact_correo2 = "";
        public static string contact_cargo2 = "";
        public static string contact_fono2 = "";
        public static string banco_id2 = "";
        public static string banco2 = "";
        public static string cta2 = "";
        public static string socio_id2 = "";
        public static string socio_nombre2 = "";
        public static string socio_rut2 = "";
        public static string socio_correo2 = "";
        public static string socio_porcen2 = "";

        public static int COLUMNA_DE_FACTURA;
        public static bool busca_columna_fac;
        public static bool columna_fac;

        public static int cont;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");

                clie_rut = encriptador.DecryptData(Request.QueryString["R"].ToString().Replace(" ", "+"));
                string bit = encriptador.DecryptData(Request.QueryString["i"].ToString().Replace(" ", "+"));

                if (bit == "25") { carga_doc_abiertos(clie_rut, User.Identity.Name); }
                else
                {
                    USER = User.Identity.Name.ToString();
                    string grupos_del_usuario = "";

                    grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario(User.Identity.Name.ToString()));

                    if (grupos_del_usuario == "")
                    {
                        grupos_del_usuario = agregra_comillas(ReporteRNegocio.grupos_usuario_v_report(User.Identity.Name.ToString()));
                    }
                    GRUPOS_USUARIO = grupos_del_usuario;



                    clie_rut = clie_rut.Replace(".", "").Replace("-", "").Trim();

                    string valor = clie_rut;
                    string rut_ini = valor.Trim().Substring(0, valor.Trim().Length - 1);
                    double rut = 0;
                    try { rut = double.Parse(rut_ini); rut_clie.Text = rut.ToString("N0") + "-" + valor.Trim().Substring(valor.Trim().Length - 1); }
                    catch { rut = double.Parse(valor); rut_clie.Text = rut.ToString("N0"); }

                    ed_rut.Text = rut_clie.Text;


                    if (bit == "1" || bit == "3")
                    {
                        normal.Attributes["style"] = "display:block";
                        factura.Attributes["style"] = "display:none";
                        CLIENTES_FICHA.ActiveViewIndex = 0;

                        detalle.InnerText = "Ficha Completa";
                        DataTable datos_cliente = new DataTable();
                        if (bit == "3")
                        {
                            detalle.InnerText = "Ficha Completa (último cambio:editada)";
                            datos_cliente = ReporteRNegocio.datos_cliente_ALL(1, clie_rut);
                        }
                        else
                        {
                            detalle.InnerText = "Ficha Completa (creada)";
                            datos_cliente = ReporteRNegocio.datos_cliente_ALL(0, clie_rut);
                        }
                        foreach (DataRow r in datos_cliente.Rows)
                        {

                            nom_clie.Text = clie_nombre = r[1].ToString();
                            clie_ciudad = r[2].ToString();
                            clie_pais = r[3].ToString();
                            if (clie_pais != "")
                            {
                                ciu_clie.Text = clie_ciudad + ", " + clie_pais;
                            }
                            else { ciu_clie.Text = clie_ciudad; }
                            codvendedor = r[4].ToString();
                            vendedor.Text = "(" + codvendedor + ")" + ReporteRNegocio.nombre_vendedor(codvendedor);
                            direccion_clie.Text = clie_direcc = r[5].ToString();
                            fono_clie.Text = clie_fono = r[6].ToString();
                            estado = r[7].ToString();
                            if (estado == "3")
                            {
                                detalle.InnerText = "Ficha Completa (último cambio:creada)";
                            }
                            nomb_socied.Text = socie_nomb = r[8].ToString();
                            rut_socied.Text = socie_rut = r[9].ToString();
                            direcc_socied.Text = socie_direcc = r[10].ToString();
                            socie_ciudad = r[11].ToString();
                            socie_pais = r[12].ToString();

                            if (socie_pais == "")
                            {
                                ciu_socied.Text = socie_ciudad; ;
                            }
                            else { ciu_socied.Text = socie_ciudad + ", " + socie_pais; }

                            fono_socied.Text = socie_fono = r[13].ToString();
                            correo_socied.Text = socie_correo = r[14].ToString();
                            giro_clie.Text = clie_giro = r[15].ToString();
                            clie_tipo_giro = r[16].ToString();
                            tipo_negoc.Text = ReporteRNegocio.trae_tipo_negocio_nombre(clie_tipo_giro);
                            cliente_desde.Text = desde_clie = r[17].ToString();
                            fecha_creacion = r[18].ToString();
                            cred_actual.Text = credito_actual1 = r[19].ToString();
                            t_cred_act.Text = tipo_credito_actual1 = r[20].ToString();
                            mon_cred_act.Text = monto_credito_actual1 = r[21].ToString();
                            cred_sol.Text = credito_solicit1 = r[22].ToString();
                            t_cred_sol.Text = tipo_credito_solicit1 = r[23].ToString();
                            mon_cred_sol.Text = monto_credito_solicit1 = r[24].ToString();
                            contact_id1 = r[25].ToString();
                            nomb_cont1.Text = contact_nombre1 = r[26].ToString();
                            correo_cont1.Text = contact_correo1 = r[27].ToString();
                            cargo_cont1.Text = contact_cargo1 = r[28].ToString();
                            fono_cont1.Text = contact_fono1 = r[29].ToString();
                            banco_id1 = r[30].ToString();
                            bank_1.Text = banco1 = r[31].ToString();
                            l_cta1.Text = cta1 = r[32].ToString();
                            socio_id1 = r[33].ToString();
                            nom_soc1.Text = socio_nombre1 = r[34].ToString();
                            rut_soc1.Text = socio_rut1 = r[35].ToString();
                            corr_soc1.Text = socio_correo1 = r[36].ToString();
                            porc_soc1.Text = socio_porcen1 = r[37].ToString();

                        }


                        if (ReporteRNegocio.count_contactos(clie_rut) == "2")
                        {
                            cont_2.Attributes["style"] = "display:block";

                            DataTable contacto_extra = ReporteRNegocio.contacto_extra(clie_rut);
                            foreach (DataRow r in contacto_extra.Rows)
                            {
                                contact_id2 = r[1].ToString();
                                nomb_cont2.Text = contact_nombre2 = r[1].ToString();
                                correo_cont2.Text = contact_correo2 = r[2].ToString();
                                cargo_cont2.Text = contact_cargo2 = r[3].ToString();
                                fono_cont2.Text = contact_fono2 = r[4].ToString();
                            }
                        }
                        else
                        {
                            cont_2.Attributes["style"] = "display:none";
                        }

                        if (ReporteRNegocio.count_bancas(clie_rut) == "2")
                        {


                            otra_cta_cte.Attributes["style"] = "display:block";
                            DataTable banco_extra = ReporteRNegocio.banco_extra(clie_rut);
                            foreach (DataRow r in banco_extra.Rows)
                            {
                                banco_id2 = r[1].ToString();
                                bank_2.Text = banco2 = r[2].ToString();
                                l_cta2.Text = cta2 = r[3].ToString();
                            }
                        }
                        else
                        {
                            otra_cta_cte.Attributes["style"] = "display:none";
                        }


                        if (ReporteRNegocio.count_socios(clie_rut) == "2")
                        {
                            nuevo_socio.Attributes["style"] = "display:block";
                            DataTable socio_extra = ReporteRNegocio.socio_extra(clie_rut);
                            foreach (DataRow r in socio_extra.Rows)
                            {
                                socio_id2 = r[1].ToString();
                                nom_soc2.Text = socio_nombre2 = r[3].ToString();
                                rut_soc2.Text = socio_rut2 = r[2].ToString();
                                corr_soc2.Text = socio_correo2 = r[4].ToString();
                                porc_soc2.Text = socio_porcen2 = r[5].ToString();
                            }
                        }
                        else
                        {
                            nuevo_socio.Attributes["style"] = "display:none";
                        }



                    }
                    else
                    {
                        normal.Attributes["style"] = "display:none";
                        factura.Attributes["style"] = "display:block";
                        CLIENTES_FICHA.ActiveViewIndex = 1;
                        H1.InnerText = "Editar Ficha";
                        DataTable dt; DataView dtv = new DataView();
                        string es_vend = ReporteRNegocio.esvendedor(User.Identity.Name);
                        string where = " where user1 in (" + GRUPOS_USUARIO + ")";

                        if (es_vend == "2")
                        {
                            where += " and codvendedor = '" + User.Identity.Name + "' ";
                        }
                        cargar_combo_VENDEDOR_(ReporteRNegocio.listar_ALL_vendedores(where), dtv);

                        cargar_combo_tipo_negocio(ReporteRNegocio.tipo_negocio(), dtv);



                        DataTable datos_cliente = ReporteRNegocio.datos_cliente_ALL(1, clie_rut);
                        foreach (DataRow r in datos_cliente.Rows)
                        {

                            ////nom_clie.Text = clie_nombre = r[1].ToString();
                            //clie_ciudad = r[2].ToString();
                            //clie_pais = r[3].ToString();
                            //if (clie_pais != "")
                            //{
                            //    ciu_clie.Text = clie_ciudad + ", " + clie_pais;
                            //}
                            //else { ciu_clie.Text = clie_ciudad; }
                            //codvendedor = r[4].ToString();
                            ////vendedor.Text = "(" + codvendedor + ")" + ReporteRNegocio.nombre_vendedor(codvendedor);
                            //direccion_clie.Text = clie_direcc = r[5].ToString();
                            //fono_clie.Text = clie_fono = r[6].ToString();
                            estado = r[7].ToString();
                            ed_nom_socie.InnerText = socie_nomb = r[8].ToString();
                            ed_rut_socie.InnerText = socie_rut = r[9].ToString();
                            ed_direcc_socie.InnerText = socie_direcc = r[10].ToString();
                            socie_ciudad = r[11].ToString();
                            socie_pais = r[12].ToString();
                            if (socie_pais == "")
                            {
                                ed_ciudad.InnerText = socie_ciudad; ;
                            }
                            else { ed_ciudad.InnerText = socie_ciudad + ", " + socie_pais; }

                            ed_fono_socie.InnerText = socie_fono = r[13].ToString();
                            ed_correo_socie.InnerText = socie_correo = r[14].ToString();
                            ed_giro.InnerText = clie_giro = r[15].ToString();
                            tipo_negocio.SelectedValue = clie_tipo_giro = r[16].ToString();

                            ed_cliente_desde.Text = desde_clie = r[17].ToString();
                            fecha_creacion = r[18].ToString();
                            ed_cred_act.InnerText = credito_actual1 = r[19].ToString();
                            ed_ti_cred_act.InnerText = tipo_credito_actual1 = r[20].ToString();
                            ed_mon_cred_act.InnerText = monto_credito_actual1 = r[21].ToString();
                            ed_cred_soli.InnerText = credito_solicit1 = r[22].ToString();
                            ed_ti_cred_soli.InnerText = tipo_credito_solicit1 = r[23].ToString();
                            ed_mon_cred_soli.InnerText = monto_credito_solicit1 = r[24].ToString();
                            contact_id1 = r[25].ToString();
                            ed_nom_cont1.InnerText = contact_nombre1 = r[26].ToString();
                            ed_corre_cont1.InnerText = contact_correo1 = r[27].ToString();
                            ed_cargo_cont1.InnerText = contact_cargo1 = r[28].ToString();
                            ed_fono_cont1.InnerText = contact_fono1 = r[29].ToString();
                            banco_id1 = r[30].ToString();
                            ed_bank1.InnerText = banco1 = r[31].ToString();
                            ed_cta1.InnerText = cta1 = r[32].ToString();
                            socio_id1 = r[33].ToString();
                            ed_nom_soc1.InnerText = socio_nombre1 = r[34].ToString();
                            ed_rut_soc1.InnerText = socio_rut1 = r[35].ToString();
                            ed_correo_soc1.InnerText = socio_correo1 = r[36].ToString();
                            ed_porc_soc1.InnerText = socio_porcen1 = r[37].ToString();

                        }


                        if (ReporteRNegocio.count_contactos(clie_rut) == "2")
                        {
                            Div2.Attributes["style"] = "display:block";

                            DataTable contacto_extra = ReporteRNegocio.contacto_extra(clie_rut);
                            foreach (DataRow r in contacto_extra.Rows)
                            {
                                contact_id2 = r[1].ToString();
                                ed_nom_cont2.InnerText = contact_nombre2 = r[1].ToString();
                                ed_correo_cont2.InnerText = contact_correo2 = r[2].ToString();
                                ed_cargo_cont2.InnerText = contact_cargo2 = r[3].ToString();
                                ed_fono_cont2.InnerText = contact_fono2 = r[4].ToString();
                            }
                        }
                        else
                        {
                            Div2.Attributes["style"] = "display:none";
                        }

                        if (ReporteRNegocio.count_bancas(clie_rut) == "2")
                        {


                            Div3.Attributes["style"] = "display:block";
                            DataTable banco_extra = ReporteRNegocio.banco_extra(clie_rut);
                            foreach (DataRow r in banco_extra.Rows)
                            {
                                banco_id2 = r[1].ToString();
                                ed_bank2.InnerText = banco2 = r[2].ToString();
                                ed_cta2.InnerText = cta2 = r[3].ToString();
                            }
                        }
                        else
                        {
                            Div3.Attributes["style"] = "display:none";
                        }


                        if (ReporteRNegocio.count_socios(clie_rut) == "2")
                        {
                            Div4.Attributes["style"] = "display:block";
                            DataTable socio_extra = ReporteRNegocio.socio_extra(clie_rut);
                            foreach (DataRow r in socio_extra.Rows)
                            {
                                socio_id2 = r[1].ToString();
                                ed_nom_soc2.InnerText = socio_nombre2 = r[3].ToString();
                                ed_rut_soc2.InnerText = socio_rut2 = r[2].ToString();
                                ed_correo_soc2.InnerText = socio_correo2 = r[4].ToString();
                                ed_porc_soc2.InnerText = socio_porcen2 = r[5].ToString();
                            }
                        }
                        else
                        {
                            Div4.Attributes["style"] = "display:none";
                        }
                        string nombrecliente = "";
                        string direccion = "";
                        string ciudad = "";
                        string pais = "";
                        string fono = "";
                        string correo = "";
                        string vendedor = "";
                        string monto_credito_actual = "";
                        string disponible = "";
                        DataTable dato_clientes = ReporteRNegocio.datos_cliente(clie_rut.Replace(".", "").Replace("-", "").Trim());
                        foreach (DataRow r in dato_clientes.Rows)
                        {
                            ed_nombre.Text = nombrecliente = r[0].ToString().Trim();
                            direccion = r[2].ToString();
                            ciudad = r[3].ToString();
                            pais = r[4].ToString();
                            fono = r[5].ToString();
                            correo = r[9].ToString();
                            vendedor = r[7].ToString();
                            monto_credito_actual = r[10].ToString();
                            disponible = r[11].ToString();
                        }

                        vendedor_NNEW.SelectedValue = vendedor.Trim();
                        ed_direcc.InnerText = direccion.Trim();
                        if (pais != "")
                        {
                            ed_ciudad.InnerText = ciudad.Trim() + ", " + pais.Trim();
                        }
                        else { ed_ciudad.InnerText = ciudad.Trim(); }
                        ed_fono.InnerText = fono.Trim();
                        ed_mon_cred_act.InnerText = monto_credito_actual.Trim();
                    }
                    //cont_2.Attributes["style"] = "display:none";
                    //otra_cta_cte.Attributes["style"] = "display:none";
                    //nuevo_socio.Attributes["style"] = "display:none";

                }
            }
        }

        private void carga_doc_abiertos(string clie_rut, string user)
        {
            DataTable cr = ReporteRNegocio.corr_usuario(User.Identity.Name);
            foreach (DataRow r in cr.Rows)
            {
                tx_enviar_.Text = r[1].ToString();
            }

            titulo.InnerText = "Documentos Abiertos del cliente " + ReporteRNegocio.nombre_cliente(clie_rut.Replace(".", "").Replace("-", ""));

            CLIENTES_FICHA.ActiveViewIndex = 2;
            busca_columna_fac = true;

            cont = 1;
            string cod_vend = "";
            if (ReporteRNegocio.esvendedor(User.Identity.Name) == "2")
            {
                cod_vend = User.Identity.Name;
            }
            if (cod_vend != "")
            {

                g_doc.DataSource = ReporteRNegocio.docu_abier(clie_rut.Replace(".", "").Replace("-", ""), cod_vend);

            }
            else
            {
                g_doc.DataSource = ReporteRNegocio.docu_abier(clie_rut.Replace(".", "").Replace("-", ""), "");
            }


            g_doc.DataBind();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "teeqqqeeeqe", "<script> new Tablesort(document.getElementById('g_doc')) </script>", false);


        }

        private void cargar_combo_tipo_negocio(DataTable dt, DataView dtv)
        {
            dt.Rows.Add(new Object[] { "-1", "-- Otro --" });
            dtv = dt.DefaultView;
            dtv.Sort = "id";
            tipo_negocio.DataSource = dtv;
            tipo_negocio.DataTextField = "nombre_tipo";
            tipo_negocio.DataValueField = "id";
            tipo_negocio.DataBind();
        }
        private void cargar_combo_VENDEDOR_(DataTable dt, DataView dtv)
        {
            dt.Rows.Add(new Object[] { "-1", "-- Otro --" });
            dtv = dt.DefaultView;
            dtv.Sort = "cod_vend";
            vendedor_NEW_.DataSource = dtv;
            vendedor_NEW_.DataTextField = "nom_vend";
            vendedor_NEW_.DataValueField = "cod_vend";

            vendedor_NNEW.DataSource = dtv;
            vendedor_NNEW.DataTextField = "nom_vend";
            vendedor_NNEW.DataValueField = "cod_vend";



            string es_vend = ReporteRNegocio.esvendedor(User.Identity.Name);

            vendedor_NEW_.DataBind();
            vendedor_NNEW.DataBind();
            if (es_vend == "2")
            {
                vendedor_NNEW.SelectedIndex = 0;
                vendedor_NEW_.SelectedIndex = 0;
                l_vendedores.Text = User.Identity.Name.Trim();
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

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            contacto_extra = true;
            Div2.Attributes["style"] = "display:block";
        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            contacto_extra = false; ;
            Div2.Attributes["style"] = "display:none";
        }
        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            cta_extra = true; ;
            Div3.Attributes["style"] = "display:block";
        }

        protected void LinkButton5_Click(object sender, EventArgs e)
        {
            cta_extra = false; ;
            Div3.Attributes["style"] = "display:none";
        }

        protected void LinkButton6_Click(object sender, EventArgs e)
        {
            socio_extra = true; ;
            Div4.Attributes["style"] = "display:block";
        }

        protected void LinkButton7_Click(object sender, EventArgs e)
        {
            socio_extra = false; ;
            Div4.Attributes["style"] = "display:none";
        }

        protected void Editar_cliente_Click(object sender, EventArgs e)
        {

            ReporteRNegocio.delete_cliente2(clie_rut.Replace("-", "").Replace(".", ""));
            ReporteRNegocio.delete_contactos(clie_rut.Replace("-", "").Replace(".", ""));
            ReporteRNegocio.delete_cuentas_bank(clie_rut.Replace("-", "").Replace(".", ""));
            ReporteRNegocio.delete_socios(clie_rut.Replace("-", "").Replace(".", ""));
            ReporteRNegocio.delete_sociedad(clie_rut.Replace("-", "").Replace(".", ""));
            try
            {
                ciudad = ed_ciudad.Value.Substring(0, ed_ciudad.Value.IndexOf(","));
            }
            catch { ciudad = ed_ciudad.Value; }

            try
            {
                if (ed_ciudad.Value.Contains(","))
                {

                    pais = ed_ciudad.Value.Substring(ed_ciudad.Value.IndexOf(",") + 1);
                }
            }
            catch { pais = ""; }

            string q = "OK";
            string q2 = "";
            if (q == "OK")
            {
                q = ReporteRNegocio.insert_cliente2(1, DateTime.Now, clie_rut.Replace("-", ""), ed_giro.Value, tipo_negocio.SelectedValue, ed_cliente_desde.Text, ed_cred_act.Value, ed_ti_cred_act.Value, ed_mon_cred_act.Value, ed_cred_soli.Value, ed_ti_cred_soli.Value, ed_mon_cred_soli.Value, ed_nombre.Text, ed_direcc.Value, ciudad, pais, ed_fono.Value, vendedor_NNEW.SelectedValue.ToString());
            }
            if (q == "OK")
            {
                if (contacto_extra)
                {
                    q = ReporteRNegocio.insert_contactos_cliente(1, clie_rut.Replace("-", ""), ed_nom_cont1.Value, ed_nom_cont2.Value, ed_corre_cont1.Value, ed_correo_cont2.Value, ed_cargo_cont1.Value, ed_cargo_cont2.Value, ed_fono_cont1.Value, ed_fono_cont2.Value);
                }
                else
                {
                    ed_nom_cont2.Value = "";
                    q = ReporteRNegocio.insert_contactos_cliente(2, clie_rut.Replace("-", ""), ed_nom_cont1.Value, ed_nom_cont2.Value, ed_corre_cont1.Value, ed_correo_cont2.Value, ed_cargo_cont1.Value, ed_cargo_cont2.Value, ed_fono_cont1.Value, ed_fono_cont2.Value);
                }

            }
            if (q == "OK")
            {
                if (cta_extra)
                {
                    q = ReporteRNegocio.insert_ref_banco_cliente(1, clie_rut.Replace("-", ""), ed_bank1.Value, ed_bank2.Value, ed_cta1.Value, ed_cta2.Value);
                }
                else
                {
                    ed_bank2.Value = "";
                    q = ReporteRNegocio.insert_ref_banco_cliente(2, clie_rut.Replace("-", ""), ed_bank1.Value, ed_bank2.Value, ed_cta1.Value, ed_cta2.Value);
                }

            }
            if (q == "OK")
            {

                try
                {
                    ciudad_so = ed_ciudad_socie.Value.Substring(0, ed_ciudad_socie.Value.IndexOf(","));
                }
                catch { ciudad_so = ed_ciudad_socie.Value; }

                try
                {
                    if (ed_ciudad_socie.Value.Contains(","))
                    {

                        pais_so = ed_ciudad_socie.Value.Substring(ed_ciudad_socie.Value.IndexOf(",") + 1);
                    }
                }
                catch { pais = ""; }

                q = ReporteRNegocio.insert_sociedad_cliente(clie_rut.Replace("-", ""), ed_rut_socie.Value.Replace("-", "").Replace(".", ""), ed_nom_socie.Value, ed_direcc_socie.Value, ciudad_so, pais_so, ed_fono_socie.Value, ed_correo_socie.Value);

            }

            if (q == "OK")
            {
                if (socio_extra)
                {
                    q = ReporteRNegocio.insert_socios_cliente(1, clie_rut.Replace("-", ""), ed_rut_soc1.Value.Replace("-", "").Replace(".", ""), ed_rut_soc2.Value.Replace("-", "").Replace(".", ""), ed_nom_soc1.Value, ed_nom_soc2.Value, ed_correo_soc1.Value, ed_correo_soc2.Value, ed_porc_soc1.Value, ed_porc_soc2.Value);
                }
                else
                {
                    ed_rut_soc2.Value = "";
                    q = ReporteRNegocio.insert_socios_cliente(2, clie_rut.Replace("-", ""), ed_rut_soc1.Value.Replace("-", "").Replace(".", ""), ed_rut_soc2.Value.Replace("-", "").Replace(".", ""), ed_nom_soc1.Value, ed_nom_soc2.Value, ed_correo_soc1.Value, ed_correo_soc2.Value, ed_porc_soc1.Value, ed_porc_soc2.Value);
                }

            }



            if (q == "OK")
            {
                H1.InnerText = "Cliente Editado !";
                l_h1.Text = "Cliente Editado !";
                enviar_correo();

            }
            else
            {
                H1.InnerText = "Error al editar !";
                l_h1.Text = "Error al editar !";
            }



        }
        private void enviar_correo()
        {
            DataTable corr = ReporteRNegocio.corr_nuevos_clie();
            string usuario = "";
            string correo = "";
            string cc = "";
            foreach (DataRow r in corr.Rows)
            {
                usuario = r[1].ToString();
                correo = r[2].ToString();
                cc = r[3].ToString();
            }

            string correo1 = "";
            if (vendedor_NNEW.SelectedValue.ToString() != "")
            {
                DataTable corr1 = ReporteRNegocio.corr_usuario(vendedor_NNEW.SelectedValue.ToString());
                string nomusuario1 = "";

                string cc1 = "";
                foreach (DataRow r in corr1.Rows)
                {
                    nomusuario1 = r[0].ToString();
                    correo1 = r[1].ToString();
                }
            }

            DataTable corr2 = ReporteRNegocio.corr_usuario(User.Identity.Name);
            string nomusuario2 = "";
            string correo_user = "";
            foreach (DataRow r in corr2.Rows)
            {
                nomusuario2 = r[0].ToString();
                correo_user = r[1].ToString();
            }

            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(correo));
            //email.To.Add(new MailAddress("esteban.godoy15@gmail.com"));
            email.From = new MailAddress("informes@soprodi.cl");

            email.Subject = "Edición Cliente ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";

            string cc2 = "";
            if (correo1 != "")
            {
                cc2 += correo1;
            }
            if (correo_user != "")
            {
                cc2 += "," + correo_user;
            }
            if (correo_user.Trim() == correo1.Trim())
            {
                cc2 = correo_user;
            }

            if (cc2 != "")
            {
                email.CC.Add(cc2);
            }

            email.Body += "<div style='text-align:center;     display: block !important;' > ";
            email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div> ";
            email.Body += "</div>";
            email.Body += "<div><img src='http://a58.imgup.net/Sopro4d9d.png' style='    float: right;     width: 90px;'> </div><br><br><br><br><br>";

            email.Body += "<div> Estimado " + usuario.Trim() + ":<br> <br> Se ha EDITADO un cliente con fecha <b>" + DateTime.Now.ToString("dd/MMM/yyyy") + "</b>";

            string body_correo = crear_cuerpo_correo();
            email.Body += "<div>" + body_correo + "</div>";


            email.Body += "<br> Para más detalles diríjase a:  <a href='http://srv-app.soprodi.cl' > srv-app.soprodi.cl  </a> <span style='font:5px; color:gray; float:right;'>No Responder Este Correo</span> <br><br>";
            email.Body += "<div style='text-align:center;     display: block !important;' > ";
            email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div> ";
            email.Body += "</div>";

            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;
            email.BodyEncoding = System.Text.Encoding.UTF8;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "srv-correo-2.soprodi.cl";
            smtp.Port = 25;
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            try
            {
                smtp.Send(email);
                email.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        private string crear_cuerpo_correo()
        {
            string html = "</br>";


            html += "<div style='width:100%'>";
            html += "<table width:100% border=1 cellpadding='8'>";
            html += "<tr>";
            html += "<td colspan=2>";
            html += "Nombre: <b><p style='font-size: 16px;'>" + ed_nombre.Text + "</p></b>";
            html += "</td>";
            html += "<td>";
            html += "RUN: " + ed_rut.Text;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=3 style='background-color:#005D99;'>";
            html += "<b style='color:white'>VENDEDOR</b>";
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=3>";
            html += "Vendedor: " + ReporteRNegocio.nombre_vendedor(vendedor_NNEW.SelectedValue.ToString().Trim());
            html += "</td>";
            html += "</tr>";


            html += "<tr>";
            html += "<td colspan=3 style='background-color:#DC1510;'>";
            html += "<b style='color:white'>ANTECEDENTES GENERALES</b>";
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td>";
            html += "Dirección: " + ed_direcc.Value;
            html += "</td>";
            html += "<td>";
            html += "Ciudad: " + ciudad;
            html += "</td>";
            html += "<td>";
            html += "País: " + pais;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td>";
            html += "Teléfono: " + ed_fono.Value;
            html += "</td>";
            html += "<td colspan=2>";
            html += "Giro: " + ed_giro.Value;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=3 style='background-color:#DC1510;'>";
            html += "<b style='color:white'>CONTACTO</b>";
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=2>";
            html += "Nombre: " + ed_nom_cont1.Value;
            html += "</td>";
            html += "<td>";
            html += "Correo: " + ed_corre_cont1.Value;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=2>";
            html += "Cargo: " + ed_cargo_cont1.Value;
            html += "</td>";
            html += "<td>";
            html += "Teléfono: " + ed_fono_cont1.Value;
            html += "</td>";
            html += "</tr>";


            if (contacto_extra)
            {
                html += "<tr>";
                html += "<td colspan=3 style='background-color:#DC1510;'>";
                html += "<b style='color:white'>CONTACTO 2</b>";
                html += "</td>";
                html += "</tr>";

                html += "<tr>";
                html += "<td colspan=2>";
                html += "Nombre2: " + ed_nom_cont2.Value;
                html += "</td>";
                html += "<td>";
                html += "Correo2: " + ed_correo_cont2.Value;
                html += "</td>";
                html += "</tr>";

                html += "<tr>";
                html += "<td colspan=2>";
                html += "Cargo2: " + ed_cargo_cont2.Value;
                html += "</td>";
                html += "<td>";
                html += "Teléfono2: " + ed_fono_cont2.Value;
                html += "</td>";
                html += "</tr>";
            }
            html += "<tr>";
            html += "<td colspan=3 style='background-color:#005D99;'>";
            html += "<b style='color:white'>TIPO NEGOCIO</b>";
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=3>";
            string aux2 = "";
            string q = ReporteRNegocio.trae_tipo_negocio_nombre(tipo_negocio.SelectedValue);
            if (q == "") { aux2 = "Otro"; } else { aux2 = q; }
            html += "Tipo: " + aux2;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=3 style=' background-color:#DC1510;'>";
            html += "<b style='color:white'>REFERENCIA BANCARIA</b>";
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=2>";
            html += "Banco: " + ed_bank1.Value;
            html += "</td>";
            html += "<td>";
            html += "CTA.CTE.: " + ed_cta1.Value;
            html += "</td>";
            html += "</tr>";



            if (cta_extra)
            {
                html += "<tr>";
                html += "<td colspan=2>";
                html += "Banco2: " + ed_bank2.Value;
                html += "</td>";
                html += "<td>";
                html += "CTA.CTE.2: " + ed_cta2.Value;
                html += "</td>";
                html += "</tr>";
            }

            html += "<tr>";
            html += "<td colspan=3 style='background-color:#005D99;'>";
            html += "<b style='color:white'>ATENCEDENTES DE LA SOCIEDAD</b>";
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=2>";
            html += "Nombre: " + ed_nom_socie.Value;
            html += "</td>";
            html += "<td>";
            html += "RUN: " + ed_rut_socie.Value;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td>";
            html += "Dirección: " + ed_direcc_socie.Value;
            html += "</td>";
            html += "<td>";
            html += "Ciudad: " + ciudad_so;
            html += "</td>";
            html += "<td>";
            html += "País: " + pais_so;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td>";
            html += "Teléfono: " + ed_fono_socie.Value;
            html += "</td>";
            html += "<td>";
            html += "Correo: " + ed_correo_socie.Value;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=3 style='background-color:#005D99;'>";
            html += "<b style='color:white'>SOCIO</b>";
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td>";
            html += "RUN: " + ed_rut_soc1.Value;
            html += "</td>";
            html += "<td colspan=2>";
            html += "Nombre: " + ed_nom_soc1.Value;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td>";
            html += "Correo: " + ed_correo_soc1.Value;
            html += "</td>";
            html += "<td>";
            html += "Porcentaje: " + ed_porc_soc1.Value;
            html += "</td>";
            html += "</tr>";


            if (socio_extra)
            {
                html += "<tr>";
                html += "<td colspan=3 style='background-color:#005D99;'>";
                html += "<b style='color:white'>SOCIO 2</b>";
                html += "</td>";
                html += "</tr>";

                html += "<tr>";
                html += "<td>";
                html += "RUN2: " + ed_rut_soc2.Value;
                html += "</td>";
                html += "<td colspan=2>";
                html += "Nombre2: " + ed_nom_soc2.Value;
                html += "</td>";
                html += "</tr>";

                html += "<tr>";
                html += "<td>";
                html += "Correo2: " + ed_correo_soc2.Value;
                html += "</td>";
                html += "<td>";
                html += "Porcentaje2: " + ed_porc_soc2.Value;
                html += "</td>";
                html += "</tr>";

            }
            html += "<tr>";
            html += "<td colspan=3 style='background-color:#DC1510;'>";
            html += "<b style='color:white'>ATENCEDENTES SOPRODI S.A.</b>";
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=3>";
            html += "Cliente desde: " + ed_cliente_desde.Text;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td>";
            html += "Crédito Actual: " + ed_cred_act.Value;
            html += "</td>";
            html += "<td>";
            html += "Tipo Crédito Actual: " + ed_ti_cred_act.Value;
            html += "</td>";
            html += "<td>";
            html += "Monto Crédito Actual: " + ed_mon_cred_act.Value;
            html += "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td>";
            html += "Crédito Solicitado: " + ed_cred_soli.Value;
            html += "</td>";
            html += "<td>";
            html += "Tipo Crédito Solicitado: " + ed_ti_cred_soli.Value;
            html += "</td>";
            html += "<td>";
            html += "Monto Crédito Solicitado: " + ed_mon_cred_soli.Value;
            html += "</td>";
            html += "</tr>";


            html += "</table>";
            html += "</div>";
            return html;

        }

        protected void g_doc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (busca_columna_fac)
                {
                    try
                    {
                        for (int x = 0; x <= g_doc.HeaderRow.Cells.Count; x++)
                        {
                            if (g_doc.HeaderRow.Cells[x].Text.Contains("Factura"))
                            {
                                columna_fac = true;
                                COLUMNA_DE_FACTURA = x;
                                busca_columna_fac = false;
                                break;
                            }
                        }
                    }
                    catch { }
                }
                if (columna_fac)
                {
                    clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("thi");
                    string año_factura = e.Row.Cells[8].Text.Substring(6).ToString(); ;

                    if (año_factura != "")
                    {
                        año_factura = año_factura.Substring(0, 4);
                        string script = string.Format("javascript:fuera3(&#39;{0}&#39;, &#39;{1}&#39;);return false;", encriptador.EncryptData(e.Row.Cells[COLUMNA_DE_FACTURA].Text), encriptador.EncryptData(año_factura));
                        e.Row.Cells[COLUMNA_DE_FACTURA].Text = "  <a href='javascript:' onclick='" + script + "'>" + e.Row.Cells[COLUMNA_DE_FACTURA].Text + " </a>";
                    }
                }
                string valor = e.Row.Cells[0].Text;
                string rut_ini = valor.Trim().Substring(0, valor.Trim().Length - 1);
                double rut = 0;
                try { rut = double.Parse(rut_ini); e.Row.Cells[0].Text = rut.ToString("N0") + "-" + valor.Trim().Substring(valor.Trim().Length - 1); }
                catch { rut = double.Parse(valor); e.Row.Cells[0].Text = rut.ToString("N0"); }


                double d;
                double.TryParse(e.Row.Cells[11].Text, out d);
                string aux = "";
                if (d == 0) { aux = ""; } else { aux = d.ToString("N0"); }
                e.Row.Cells[11].Text = aux;

                double d2;
                double.TryParse(e.Row.Cells[12].Text, out d2);
                string aux2 = "";
                if (d2 == 0) { aux2 = ""; } else { aux2 = d2.ToString("N0"); }
                e.Row.Cells[12].Text = aux2;



                g_doc.HeaderRow.Cells[12].Attributes["data-sort-method"] = "number";
                g_doc.HeaderRow.Cells[11].Attributes["data-sort-method"] = "number";

                e.Row.Cells[8].Attributes["data-sort-value"] = cont.ToString();
                //data - sort - value = 10;
                cont++;

            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void excel2_Click(object sender, EventArgs e)
        {
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);

            g_doc.RenderControl(htmlWrite);

            string body = stringWrite.ToString();

            body = body.Replace("<tr class='test no-sort'>", "<tr style='background-color:#428bca;color:#fff'>");



            DataTable corr = ReporteRNegocio.corr_usuario(User.Identity.Name);
            string usuario = "";
            string correo = "";
            foreach (DataRow r in corr.Rows)
            {
                usuario = r[0].ToString();
                correo = r[1].ToString();
            }


            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(tx_enviar_.Text));
            email.From = new MailAddress("informes@soprodi.cl");
            email.Subject = "Documentos Abiertos ( " + DateTime.Now.ToString("dd / MMM / yyy hh:mm:ss") + " ) ";
            //if (cc != "")
            //{
            //    email.CC.Add(cc);
            //}
            email.Body += "<div style='text-align:center;     display: block !important;' > ";
            email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div> ";
            email.Body += "</div>";
            email.Body += "<div><img src='http://a58.imgup.net/Sopro4d9d.png' style='    float: right;     width: 90px;'> </div><br><br><br><br><br>";

            email.Body += "<div> Estimado :<br> <br> <b>" + titulo.InnerText.Substring(titulo.InnerText.IndexOf(":")+1) + "</b> <br><br>";

            //string body_correo = crear_cuerpo_correo();
            email.Body += "<div>" + body + "</div>";


            email.Body += "<br> Para más detalles diríjase a:  <a href='http://srv-app.soprodi.cl' > srv-app.soprodi.cl  </a> <span style='font:5px; color:gray; float:right;'>No Responder Este Correo</span> <br><br>";
            email.Body += "<div style='text-align:center;     display: block !important;' > ";
            email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div>";
            email.Body += "  <div style='background-color:#DC1510; float:right; width:12.5%; height:6px'></div>";
            email.Body += "<div style='background-color:#005D99; float:right; width:12.5%; height:6px'></div> ";
            email.Body += "</div>";

            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;
            email.BodyEncoding = System.Text.Encoding.UTF8;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "srv-correo-2.soprodi.cl";
            smtp.Port = 25;
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
            try
            {
                smtp.Send(email);
                email.Dispose();
                correo_env.InnerText = "Correo Enviado!";

            }
            catch (Exception ex)
            {
                correo_env.InnerText = "Error al enviar";
            }
        }

    }
}