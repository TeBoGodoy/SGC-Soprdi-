using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoprodiApp.negocio;
using SoprodiApp.entidad;

namespace SoprodiApp
{
    public partial class REPORTE_CAMBIO_PASS : System.Web.UI.Page
    {
        public static int cont_periodos;
        public static DataTable aux;
        private static string USER;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.RegisterRedirectOnSessionEndScript();
            if (!IsPostBack)
            {

              

                USER = User.Identity.Name.ToString();
                mensaje_pass.Text = "";
            }
        }
        protected void guarda_Click(object sender, EventArgs e)
        {
            string es_su_pass = ReporteRNegocio.es_su_pass(USER);

            clsCrypto.CL_Crypto encriptador = new clsCrypto.CL_Crypto("Somos los mas solomon de quillota 2015");
            string contrasena = encriptador.EncryptData(pass_antigua.Value.ToUpper());

            if (es_su_pass != contrasena)
            {
                mensaje_pass.Text = "Contraseña actual errónea, reingrese";
            }
            else
            {
                usuarioEntity us = new usuarioEntity();
                us.cod_usuario = USER.ToUpper();

                usuarioBO.encontrar(ref us);        

                clsCrypto.CL_Crypto encriptador2 = new clsCrypto.CL_Crypto("Somos los mas solomon de quillota 2015");
                string contrasena2 = encriptador2.EncryptData(pass_nueva.Value.ToUpper());

                us.clave = contrasena2;
                us.activado = "true";
                us.tipo_usuario = ReporteRNegocio.es_su_tipo(USER);

                

                if (usuarioBO.actualizar(us, USER) == "OK")
                {
                    mensaje_pass.Text = "Contraseña cambiada ! ";
                }
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }


    }
}