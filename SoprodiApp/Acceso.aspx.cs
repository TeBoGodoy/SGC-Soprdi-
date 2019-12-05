using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoprodiApp.entidad;
using SoprodiApp.negocio;
using System.Web.Security;
using System.Text;
using System.Net;
using System.IO;

namespace SoprodiApp
{
    public partial class Acceso : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public string Base64Encode(string cadena)
        {
            byte[] cadenaByte = new byte[cadena.Length];
            cadenaByte = System.Text.Encoding.UTF8.GetBytes(cadena);
            string encodedCadena = Convert.ToBase64String(cadenaByte);
            return encodedCadena;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public string Base64Decode(string cadena)
        {
            var encoder = new System.Text.UTF8Encoding();
            var utf8Decode = encoder.GetDecoder();

            byte[] cadenaByte = Convert.FromBase64String(cadena);
            int charCount = utf8Decode.GetCharCount(cadenaByte, 0, cadenaByte.Length);
            char[] decodedChar = new char[charCount];
            utf8Decode.GetChars(cadenaByte, 0, cadenaByte.Length, decodedChar, 0);
            string result = new String(decodedChar);
            return result;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ID"] = Session.SessionID.ToString();
            }
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            validar();
        }


       


        /// <summary>
        /// Retorna o conteudo da pagina a partir de uma URL
        /// </summary>
        /// <param name="url">URL da pagina. Ex: http://www.microsoft.com.br</param>
        /// <returns>Texto da pagina</returns>
        //private string RetornaDocumentText(string url)
        //{

        //    HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
        //    WebResponse webResponse = httpWebRequest.GetResponse();
        //    StreamReader streamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
        //    return streamReader.ReadToEnd();
        //}

      
        protected void validar()
        {
            clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("Somos los mas solomon de quillota 2015");
            string contrasena = encriptador.EncryptData(T_Pass.Text.ToUpper());
            //string contrasena3 = encriptador.EncryptData("zsAy0X+UdTDpaKDwPiz/BA==");
            //string total = Base.monto_format2(Math.Round(1603500.250, MidpointRounding.AwayFromZero));
            string cont2rasena = encriptador.DecryptData("bkJ47VUtVQ/L0SBDQMWicA==");
            string cont3rasena = encriptador.DecryptData("LDLtWndGkdhOF1HGL8aQKSb89UoldoTL");
            // GT015    ANTONIA2106
            // CG032     ITALIA90
            //string documentText = RetornaDocumentText("htt");
            ////MessageBox.Show(s_unicode2);
            usuarioEntity u = new usuarioEntity();
            u.cod_usuario = T_User.Text;
            u.clave = contrasena;

            string respuesta = usuarioBO.validar(ref u);
            if (respuesta == "OK")
            {
                if (u.activado != "False")
                {
                    string login = T_User.Text;
                    Session["user"] = T_User.Text;
                    Session["contraseña"] = contrasena;
                    Session["usuario"] = u;
                    FormsAuthentication.RedirectFromLoginPage(login, false);
                    Response.Redirect("Menu.aspx"); 
                }
                else 
                {
                    L_Sesion.Text = "Usuario desactivado, vuelva a ingresar.";
                    T_User.Focus();
                }
            }
            else
            {
                L_Sesion.Text = "Usuario y/o Clave inválidos, vuelva a ingresar.";
                T_User.Focus();
            }
        }
    }
}