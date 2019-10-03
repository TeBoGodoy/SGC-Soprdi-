using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CRM.Entities;
using System.Data;


namespace CRM.DataAccess
{

    public static class ctz_vendedoresDAL
    {
        private static ctz_vendedoresEntity Load(IDataReader reader)
        {
            ctz_vendedoresEntity ctz_vendedores = new ctz_vendedoresEntity();
            ctz_vendedores.cod_vendedor = Convert.ToString(reader["cod_vendedor"]);
            ctz_vendedores.nombre_vendedor = Convert.ToString(reader["nombre_vendedor"]);
            ctz_vendedores.pass = Convert.ToString(reader["pass"]);
            ctz_vendedores.usuario = Convert.ToString(reader["usuario"]);

            return ctz_vendedores;
        }

        public static DataTable GetAll(string sql_where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT *  from ctz_vendedores " + sql_where;
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        public static string Insert(ctz_vendedoresEntity ctz_vendedores)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into ctz_vendedores(cod_vendedor, nombre_vendedor, pass, usuario) " +
                "values (@cod_vendedor, @nombre_vendedor, @pass, @usuario)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_vendedor", ctz_vendedores.cod_vendedor);
                    cmd.Parameters.AddWithValue("@nombre_vendedor", ctz_vendedores.nombre_vendedor);
                    cmd.Parameters.AddWithValue("@pass", ctz_vendedores.pass);
                    cmd.Parameters.AddWithValue("@usuario", ctz_vendedores.usuario);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en ctz_vendedoresDAL.Insert Detalle: " + EX.Message;
                    }
                }
            }
        }

        public static string Insert_Scope(ctz_vendedoresEntity ctz_vendedores)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into ctz_vendedores(cod_vendedor, nombre_vendedor, pass, usuario) " +
                "values (@cod_vendedor, @nombre_vendedor, @pass, @usuario);  SELECT SCOPE_IDENTITY(); ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_vendedor", ctz_vendedores.cod_vendedor);
                    cmd.Parameters.AddWithValue("@nombre_vendedor", ctz_vendedores.nombre_vendedor);
                    cmd.Parameters.AddWithValue("@pass", ctz_vendedores.pass);
                    cmd.Parameters.AddWithValue("@usuario", ctz_vendedores.usuario);
                    try
                    {
                        string respuesta = cmd.ExecuteScalar().ToString();
                        return respuesta;
                    }
                    catch (Exception EX)
                    {
                        return "Error en ctz_vendedoresDAL.Insert Detalle: " + EX.Message;
                    }
                }
            }
        }

        public static string Update(ctz_vendedoresEntity ctz_vendedores)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"update ctz_vendedores " +
                "set " +
                "cod_vendedor = @cod_vendedor, " +
                "nombre_vendedor = @nombre_vendedor, " +
                "pass = @pass, " +
                "usuario = @usuario" +
                " where cod_vendedor = @_cod_vendedor ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_vendedor", ctz_vendedores.cod_vendedor);
                    cmd.Parameters.AddWithValue("@nombre_vendedor", ctz_vendedores.nombre_vendedor);
                    cmd.Parameters.AddWithValue("@pass", ctz_vendedores.pass);
                    cmd.Parameters.AddWithValue("@usuario", ctz_vendedores.usuario);
                    cmd.Parameters.AddWithValue("@_cod_vendedor", ctz_vendedores.cod_vendedor);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en ctz_vendedoresDAL.Update Detalle: " + EX.Message;
                    }
                }
            }
        }
        public static string Agregar(ctz_vendedoresEntity u)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql_encontrar = @"SELECT count(1) from ctz_vendedores where cod_vendedor = @cod_vendedor";
                SqlCommand cmd_encontrar = new SqlCommand(sql_encontrar, conn);
                cmd_encontrar.Parameters.AddWithValue("@cod_vendedor", u.cod_vendedor);
                try
                {
                    string respuesta = cmd_encontrar.ExecuteScalar().ToString();
                    if (Convert.ToInt32(respuesta) > 0)
                    {
                        string resp = Update(u);
                        if (resp == "OK") { return "2"; } else { return "3"; }
                    }
                    else
                    {
                        string resp = Insert(u);
                        if (resp == "OK") { return "1"; } else { return "3"; }
                    }
                }
                catch (Exception EX)
                {
                    return "3";
                }
            }
        }


        public static string Delete(ctz_vendedoresEntity ctz_vendedores)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from ctz_vendedores " +
                " where cod_vendedor = @cod_vendedor ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_vendedor", ctz_vendedores.cod_vendedor);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en ctz_vendedoresDAL.Delete Detalle: " + EX.Message;
                    }
                }
            }
        }
        public static string encontrar(ref ctz_vendedoresEntity u)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT * from ctz_vendedores where cod_vendedor = @cod_vendedor";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cod_vendedor", u.cod_vendedor);
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
                    return "Error en ctz_vendedoresDAL.Validar Detalle: " + EX.Message;
                }
            }
            return "No encontrado en DAL";
        }
        public static string Login(ref ctz_vendedoresEntity u)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT * from ctz_vendedores where usuario = @usuario and pass = @pass";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@usuario", u.usuario);
                cmd.Parameters.AddWithValue("@pass", u.pass);
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
                    return "Error en ctz_vendedoresDAL.Validar Detalle: " + EX.Message;
                }
            }
            return "No encontrado en DAL";
        }
    }
}
