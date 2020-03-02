using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Services;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using CRM.BusinessLayer;
using CRM.Entities;


namespace ThinxsysFramework
{
    public class LOG_DATA
    {
        public object cod_usuario { get; set; }
        public object nom_usuario { get; set; }
        public object nivel { get; set; }
        public object correo { get; set; }
    }

    public class SPVars
    {
        public object nombre { get; set; }
        public object valor { get; set; }
    }

    public class VariablesJson
    {
        private string nombre;
        private string valor;

        //' PROPIEDADES ''
        public string name
        {
            get { return nombre; }
            set { nombre = value; }
        }
        public string value
        {
            get { return valor; }
            set { valor = value; }
        }
    }

    public class JSON
    {
        private JSON()
        {
        }
        public static string Form(VariablesJson[] variables, string name)
        {
            VariablesJson vars = null;
            foreach (VariablesJson vars_loopVariable in variables)
            {
                vars = vars_loopVariable;
                if (vars.name == name)
                    return vars.value;
            }
            return string.Empty;
        }
    }

    public class ClassMenu
    {
        public static string LlenarMenu(DataTable dt)
        {
            string menu = "";
            menu += "<li><a href='#'></a></li>";

            foreach (DataRow dtRow in dt.Rows)
            {
                menu += "<li><a href='" + dtRow["url"] + "?app=" + dtRow["cod_aplicacion"] + "'><i class='fa fa-users'></i><span>" + dtRow["nombre_aplicacion"] + "</span></a></li>";
            }

            return menu;
        }

        public string LlenarMenu2(string nombre_menu, string url, string nombre)
        {
            string menu = "";

            menu += "<li class='has-sub' name='Recepcion'>";
            menu += "   <a href='javascript:;'>";
            menu += "   <b class='caret pull-right'></b>";
            menu += "   <i class='fa fa-download'></i>";
            menu += "   <span>Recepcion</span>";
            menu += "   </a>";
            menu += "   <ul class='sub-menu'>";
            menu += "       <li><a href='Ingreso_Recepcion.aspx'>Ingreso Recepcion</a></li>";
            menu += "       <li><a href='Lista_Recepcion.aspx'>Lista Recepcion</a></li>";
            menu += "   </ul>";
            menu += "</li>";

            menu += "<li class='has-sub' name='Pedidos'>";
            menu += "   <a href='javascript:;'>";
            menu += "   <b class='caret pull-right'></b>";
            menu += "   <i class='fa fa-truck'></i>";
            menu += "   <span>Pedidos</span>";
            menu += "   </a>";
            menu += "   <ul class='sub-menu'>";
            menu += "       <li><a href='Estados_Pedido.aspx'>Estados de Pedidos</a></li>";
            menu += "       <li><a href='Lista_Pedidos.aspx'>Listado de Pedido</a></li>";
            menu += "       <li><a href='Ingreso_Pedido.aspx'>Ingreso de Pedido</a></li>";
            menu += "   </ul>";
            menu += "</li>";

            menu += "<li class='has-sub' name='Informes'>";
            menu += "   <a href='javascript:;'>";
            menu += "   <b class='caret pull-right'></b>";
            menu += "   <i class='fa fa-file-text-o'></i>";
            menu += "   <span>Informes</span>";
            menu += "   </a>";
            menu += "   <ul class='sub-menu'>";
            menu += "       <li><a href='Informe_Pedidos.aspx'>Informe</a></li>";
            menu += "   </ul>";
            menu += "</li>";

            // Reemplaza has-sub //           
            menu = menu.Replace("<li class='has-sub' name='" + nombre_menu + "'>", "<li class='has-sub active' name='" + nombre_menu + "'>");
            // Reemplaza <li> //
            menu = menu.Replace("<li><a href='" + url + "'>" + nombre + "</a></li>", "<li class='active'><a href='" + url + "'>" + nombre + "</a></li>");
            return menu;
        }
    }

    public class Correo
    {
        public string EnviarCorreo(string from, string to, string asunto, string mensaje, string CC = "", string ruta = "", string from_alias = "")
        {
            /*-------------------------MENSAJE DE CORREO----------------------*/
            using (System.Net.Mail.MailMessage MailSetup = new System.Net.Mail.MailMessage())
            {

                MailSetup.Subject = asunto;
                MailSetup.To.Add(to);
                if (CC != "")
                {
                    MailSetup.CC.Add(CC);
                }

                if (from_alias != "")
                {
                    MailSetup.From = new System.Net.Mail.MailAddress(from, from_alias);
                }
                else
                {
                    MailSetup.From = new System.Net.Mail.MailAddress(from);
                }

                MailSetup.Body = mensaje;
                MailSetup.IsBodyHtml = true;
                MailSetup.BodyEncoding = System.Text.Encoding.UTF8;
                if (ruta != "")
                {
                    System.IO.FileInfo toDownload = new System.IO.FileInfo(ruta);
                    if (toDownload.Exists)
                    {
                        MailSetup.Attachments.Add(new System.Net.Mail.Attachment(ruta));
                    }
                }

                SmtpClient SMTP = new SmtpClient("smtp.gmail.com");
                SMTP.Port = 587;
                SMTP.EnableSsl = true;
                SMTP.Credentials = new System.Net.NetworkCredential("tcontrolsistema@gmail.com", "tcontrol2016");
                SMTP.Send(MailSetup);

                //SmtpClient smtp = new SmtpClient();
                //smtp.Host = "srv-correo-2.soprodi.cl";
                //smtp.Port = 25;
                //smtp.EnableSsl = false;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
                //smtp.Send(MailSetup);
                // SMTP
                return "OK";

            }
        }

        public string EnviarCorreoCotizacion(string correo_enviante, string pass_correo, string from, string to, string asunto, string mensaje, string CC = "", string ruta = "", string from_alias = "")
        {
            try
            {
                /*-------------------------MENSAJE DE CORREO----------------------*/
                using (System.Net.Mail.MailMessage MailSetup = new System.Net.Mail.MailMessage())
                {

                    MailSetup.Subject = asunto;
                    MailSetup.To.Add(to);
                    if (CC != "")
                    {
                        MailSetup.CC.Add(CC);
                    }

                    if (from_alias != "")
                    {
                        MailSetup.From = new System.Net.Mail.MailAddress(from);
                    }
                    else
                    {
                        MailSetup.From = new System.Net.Mail.MailAddress(from);
                    }

                    MailSetup.Body = mensaje;
                    MailSetup.IsBodyHtml = true;
                    MailSetup.BodyEncoding = System.Text.Encoding.UTF8;
                    if (ruta != "")
                    {
                        System.IO.FileInfo toDownload = new System.IO.FileInfo(ruta);
                        if (toDownload.Exists)
                        {
                            MailSetup.Attachments.Add(new System.Net.Mail.Attachment(ruta));
                        }
                    }

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.dereyes.cl";
                    smtp.Port = 26;
                    smtp.EnableSsl = false;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(correo_enviante, pass_correo);
                    smtp.Send(MailSetup);
                    // SMTP
                    return "OK";

                    //email.IsBodyHtml = true;
                    //email.Priority = MailPriority.Normal;
                    //email.BodyEncoding = System.Text.Encoding.UTF8;
                    //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    //smtp.EnableSsl = true;
                    //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtp.UseDefaultCredentials = false;
                    //////smtp.Credentials = new NetworkCredential("informes@soprodi.cl", "galaxia");
                    //smtp.Credentials = new NetworkCredential("informes.soprodi@gmail.com", "galaxia1234");
                    //try
                    //{                   
                    //}
                    //catch (Exception ex)
                    //{
                    ////}

                    //MailSetup.IsBodyHtml = true;
                    //MailSetup.Priority = MailPriority.Normal;
                    //MailSetup.BodyEncoding = System.Text.Encoding.UTF8;
                    //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    //smtp.EnableSsl = true;
                    //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtp.UseDefaultCredentials = false;
                    //smtp.Credentials = new NetworkCredential("informes.soprodi@gmail.com", "galaxia1234");
                    //smtp.Send(MailSetup);
                    //return "OK";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    public class DBUtil
    {
        private string cadena = ConfigurationManager.ConnectionStrings["default"].ToString();
        public SqlConnection cn;

        private SqlCommandBuilder cmb;
        private void conectar()
        {
            cn = new SqlConnection(cadena);
        }
        public DBUtil()
        {
            conectar();
        }


        private SqlCommand comando;
        public bool insertar(string sql)
        {
            try
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                    cn.BeginTransaction();
                    comando = new SqlCommand(sql, cn);
                    int i = comando.ExecuteNonQuery();
                    if (i > 0)
                    {
                        comando.Transaction.Commit();
                        cn.Close();
                        return true;
                    }
                    else
                    {
                        comando.Transaction.Rollback();
                        cn.Close();
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (cn != null)
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        comando.Transaction.Rollback();
                        cn.Close();
                    }
                }
                return false;
            }
        }

        public bool eliminar(string tabla, string condicion)
        {

            try
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                    cn.BeginTransaction();
                    string sql = "delete from " + tabla + "where " + condicion;
                    comando = new SqlCommand(sql, cn);
                    int i = comando.ExecuteNonQuery();
                    cn.Close();
                    if (i > 0)
                    {
                        comando.Transaction.Commit();
                        cn.Close();
                        return true;
                    }
                    else
                    {
                        comando.Transaction.Rollback();
                        cn.Close();
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (cn != null)
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        comando.Transaction.Rollback();
                        cn.Close();
                    }
                }
                return false;
            }
        }

        public bool actualizar(string tabla, string campos, string condicion)
        {

            try
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                    cn.BeginTransaction();
                    string sql = "update " + tabla + "set " + campos + " where " + condicion;
                    comando = new SqlCommand(sql, cn);
                    int i = comando.ExecuteNonQuery();
                    cn.Close();
                    if (i > 0)
                    {
                        comando.Transaction.Commit();
                        cn.Close();
                        return true;
                    }
                    else
                    {
                        comando.Transaction.Rollback();
                        cn.Close();
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (cn != null)
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        comando.Transaction.Rollback();
                        cn.Close();
                    }
                }
                return false;
            }
        }

        public DataTable ds = new DataTable();

        public SqlDataAdapter da;
        public DataTable consultar(string sql)
        {
            da = new SqlDataAdapter(sql, cn);
            da.SelectCommand.CommandTimeout = 350;
            DataSet dts = new DataSet();
            da.Fill(dts, sql);
            DataTable dt = new DataTable();
            dt = dts.Tables[0];
            return dt;
        }

        public object Scalar(string sql)
        {
            cn.Open();
            comando = new SqlCommand(sql, cn);
            object obj = comando.ExecuteScalar();
            cn.Close();
            return obj;
        }

        public object StorageProcedure(string sql)
        {
            cn.Open();
            SqlDataReader var_sp;
            comando = new SqlCommand(sql, cn);
            var_sp = comando.ExecuteReader();
            cn.Close();
            return var_sp.ToString();
        }


        public object Scalar2(string sql, List<SPVars> toSP = null)
        {
            cn.Open();
            comando = new SqlCommand(sql, cn);
            if (toSP != null)
            {
                foreach (SPVars ob in toSP)
                {
                    comando.Parameters.AddWithValue("@" + ob.nombre, ob.valor);
                }
            }
            object obj = comando.ExecuteScalar();
            cn.Close();
            return obj;
        }

        public DataTable consultar2(string tabla)
        {
            string sql = "select * from " + tabla;
            da = new SqlDataAdapter(sql, cn);
            DataSet dts = new DataSet();
            da.Fill(dts, tabla);
            DataTable dt = new DataTable();
            dt = dts.Tables[tabla];
            return dt;
        }

        public DataTable consultar3(string sql, List<SPVars> toSP = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (toSP != null)
                {
                    foreach (SPVars ob in toSP)
                    {
                        cmd.Parameters.AddWithValue("@" + ob.nombre, ob.valor);
                    }
                }
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        public DataTable LlamaSP(string Nombre, List<SPVars> toSP = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(cadena))
            {
                SqlCommand sqlComm = new SqlCommand(Nombre, conn);
                if (toSP != null)
                {
                    foreach (SPVars ob in toSP)
                    {
                        sqlComm.Parameters.AddWithValue("@" + ob.nombre, ob.valor);
                    }
                }

                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(dt);
                return dt;
            }
        }
    }

    public class Utiles
    {
        public void CargarCombo(DropDownList DropDown, string tabla, string display, string value, string filtro = "")
        {
            DBUtil db = new DBUtil();
            DataTable dt;
            string query = "";

            if (filtro != "")
            {
                query += "select " + value + "," + display + " from " + tabla + " where " + filtro;
            }
            else
            {
                query += "select " + value + "," + display + " from " + tabla;
            }

            dt = db.consultar(query);
            dt.Rows.Add(new Object[] { -1, "-- Seleccione --" });

            DropDown.DataSource = dt;
            DropDown.DataTextField = display;
            DropDown.DataValueField = value;
            DropDown.DataBind();
            DropDown.SelectedValue = "-1";
            DropDown.Enabled = true;
        }


    }

    public class enc_correo_pdf
    {
        public String ASUNTO { get; set; }
        public String CC { get; set; }
        public String COMENTARIO { get; set; }
        public String PASS_CORREO { get; set; }
    }

    public class correos_pdf
    {
        public String rutcliente { get; set; }
        public String nombrecliente { get; set; }
        public String correocliente { get; set; }
        public String ciudad { get; set; }
        public String direccion { get; set; }
        public String giro { get; set; }
        public string resp_error { get; set; }
        public bool enviado_ok { get; set; }
    }


}
