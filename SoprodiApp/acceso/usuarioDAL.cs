using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SoprodiApp.entidad;



namespace SoprodiApp.acceso
{

    public static class usuarioDAL
    {
        private static usuarioEntity Load(IDataReader reader)
        {
            usuarioEntity usuario = new usuarioEntity();
            usuario.activado = Convert.ToString(reader["activado"]);
            usuario.clave = Convert.ToString(reader["clave"]);
            usuario.cod_usuario = Convert.ToString(reader["cod_usuario"]);
            usuario.correo = Convert.ToString(reader["correo"]);
            usuario.enviar = Convert.ToString(reader["enviar_correo"]);
            usuario.enviar2 = Convert.ToString(reader["enviar_correo2"]);
            usuario.cc = Convert.ToString(reader["cc"]);
            usuario.app = Convert.ToString(reader["app"]);
            usuario.u_negocio = Convert.ToString(reader["u_negocio"]);
            usuario.grupos = Convert.ToString(reader["grupos"]);
            usuario.nombre_ = Convert.ToString(reader["nombre_usuario"]);

            return usuario;
        }

        public static usuarioEntity GetById()
        {
            usuarioEntity usuario = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT * from usuario where  ";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    usuario = Load(reader);
                }
            }
            return usuario;
        }

        public static List<usuarioEntity> GetAll_List()
        {
            List<usuarioEntity> list = new List<usuarioEntity>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT activado, clave, cod_rol, cod_usuario, correo, firma, invitado, rut_persona, wb_clave from usuario";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(Load(reader));
                }
            }
            return list;
        }

        public static DataTable GetAll()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT activado, clave, cod_rol, cod_usuario, correo, firma, invitado, rut_persona, wb_clave  from usuario";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        public static string Insert(usuarioEntity usuario)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into usuarioweb(activado, clave, cod_usuario, tipo_usuario, correo, enviar_correo, enviar_correo2, cc, app, grupos, u_negocio, nombre_usuario) " +
                "values (@activado, @clave, @cod_usuario, @tipo_usuario, @correo, @enviar, @enviar2, @cc, @app, @grupos, @u_negocio, @nombre_usua)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@activado", usuario.activado);
                    cmd.Parameters.AddWithValue("@clave", usuario.clave);
                    cmd.Parameters.AddWithValue("@cod_usuario", usuario.cod_usuario);
                    cmd.Parameters.AddWithValue("@tipo_usuario", usuario.tipo_usuario);
                    cmd.Parameters.AddWithValue("@correo", usuario.correo);
                    cmd.Parameters.AddWithValue("@enviar", usuario.enviar);
                    cmd.Parameters.AddWithValue("@enviar2", usuario.enviar2);
                    cmd.Parameters.AddWithValue("@u_negocio", usuario.u_negocio);
                    cmd.Parameters.AddWithValue("@cc", usuario.cc);
                    cmd.Parameters.AddWithValue("@grupos", usuario.grupos);
                    cmd.Parameters.AddWithValue("@app", usuario.app);
                    cmd.Parameters.AddWithValue("@nombre_usua", usuario.nombre_);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en usuarioDAL.Insert Detalle: " + EX.Message;
                    }
                }
            }
        }

        public static string Update(usuarioEntity usuario, string usuario_)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"update usuarioweb " +
                "set " +
                "activado = @activado, " +
                "clave = @clave, " +
                "cod_usuario = @cod_usuario, " +
                "tipo_usuario = @tipo_usuario, " +
                 "correo = @correo, " +
                "enviar_correo = @enviar, " +
                "enviar_correo2 = @enviar2, " +
                "cc = @cc, " +
                "u_negocio = @u_negocio, " +
                "app = @app, " +
                "grupos = @grupos, " +
                "nombre_usuario = @nombre_usuario " +
                " where  cod_usuario = @cod_usuario_1";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@activado", usuario.activado);
                    cmd.Parameters.AddWithValue("@clave", usuario.clave);
                    cmd.Parameters.AddWithValue("@cod_usuario", usuario.cod_usuario);
                    cmd.Parameters.AddWithValue("@cod_usuario_1", usuario_);
                    cmd.Parameters.AddWithValue("@tipo_usuario", usuario.tipo_usuario);
                    cmd.Parameters.AddWithValue("@correo", usuario.correo);
                    cmd.Parameters.AddWithValue("@enviar", usuario.enviar);
                    cmd.Parameters.AddWithValue("@enviar2", usuario.enviar2);
                    cmd.Parameters.AddWithValue("@cc", usuario.cc);
                    cmd.Parameters.AddWithValue("@app", usuario.app.Trim());
                    cmd.Parameters.AddWithValue("@u_negocio", usuario.u_negocio.Trim());
                    cmd.Parameters.AddWithValue("@grupos", usuario.grupos.Trim());
                    cmd.Parameters.AddWithValue("@nombre_usuario", usuario.nombre_.Trim());

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en usuarioDAL.Update Detalle: " + EX.Message;
                    }
                }
            }
        }

        public static string Delete(usuarioEntity usuario)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from usuarioweb " +
                " where cod_usuario = @cod_usuario;  delete from usuarioweb_det where cod_usuario = @cod_usuario; ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_usuario", usuario.cod_usuario);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en usuarioDAL.Delete Detalle: " + EX.Message;
                    }
                }
            }
        }
        public static string validar(ref usuarioEntity u)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT * from usuarioweb where cod_usuario = '" + u.cod_usuario + "' and clave = '" + u.clave + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        u = Load(reader);
                        return "OK";
                    }
                }
                catch (Exception EX)
                {
                    return "Error en usuarioDAL.Validar Detalle: " + EX.Message;
                }
            }
            return "No encontrado en DAL";
        }
        public static string encontrar(ref usuarioEntity u)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT * from usuarioweb where cod_usuario = @cod_usuario";
                SqlCommand cmd = new SqlCommand(sql, conn);
                try
                {
                    cmd.Parameters.AddWithValue("@cod_usuario", u.cod_usuario);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        u = Load(reader);
                        return "OK";
                    }
                }
                catch (Exception EX)
                {
                    return "Error en usuarioDAL.Validar Detalle: " + EX.Message;
                }
            }
            return "No encontrado en DAL";
        }

        public static string obtener_adm(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select isnull(tipo_usuario, '1') from usuarioweb where cod_usuario = '" + p + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                scalar = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            return scalar;
        }

        public static string registrar_det(string p1, string p2)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into usuarioweb_det(cod_usuario, grupo_usuario) " +
                "values (@cod_usuario, @grupo_usuario)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@grupo_usuario", p2);

                    cmd.Parameters.AddWithValue("@cod_usuario", p1);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en usuarioDAL.Insert Detalle: " + EX.Message;
                    }
                }
            }
        }

        public static string eliminar_grupos_usuario(string p)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from usuarioweb_det  " +
                "where cod_usuario = @cod_usuario ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_usuario", p);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en usuarioDAL.Insert Detalle: " + EX.Message;
                    }
                }
            }
        }

        public static string obtener_check(string p)
        {

            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT enviar_correo from usuarioweb where cod_usuario = '" + p + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                scalar = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            return scalar;

        }
    }
}
