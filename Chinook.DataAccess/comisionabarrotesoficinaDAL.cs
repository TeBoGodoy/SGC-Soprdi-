using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SoprodiApp.Entities;
using System.Data;


namespace SoprodiApp.DataAccess
{

    public static class comisionabarrotesoficinaDAL
    {
        private static comisionabarrotesoficinaEntity Load(IDataReader reader)
        {
            comisionabarrotesoficinaEntity comisionabarrotesoficina = new comisionabarrotesoficinaEntity();

            try
            {
                comisionabarrotesoficina.cod_periodo = Convert.ToString(reader["cod_periodo"]);
                comisionabarrotesoficina.monto_arica = int.Parse(reader["monto_arica"].ToString());
                comisionabarrotesoficina.monto_ro = int.Parse(reader["monto_ro"].ToString());
                comisionabarrotesoficina.monto_total_abarr = int.Parse(reader["monto_total_abarr"].ToString());
                comisionabarrotesoficina.porcentaje_Azo_1 = int.Parse(reader["porcentaje_Azo_1"].ToString());
                comisionabarrotesoficina.porcentaje_Azo_2 = int.Parse(reader["porcentaje_Azo_2"].ToString());
                comisionabarrotesoficina.porcentaje_Pcat = int.Parse(reader["porcentaje_Pcat"].ToString());
            }
            catch { }
            return comisionabarrotesoficina;
        }

        public static DataTable GetAll(string sql_where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT *  from comisionabarrotesoficina " + sql_where;
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        public static string Insert(comisionabarrotesoficinaEntity comisionabarrotesoficina)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into comisionabarrotesoficina(cod_periodo, monto_arica, monto_ro, monto_total_abarr, porcentaje_Azo_1, porcentaje_Azo_2, porcentaje_Pcat) " +
                "values (@cod_periodo, @monto_arica, @monto_ro, @monto_total_abarr, @porcentaje_Azo_1, @porcentaje_Azo_2, @porcentaje_Pcat)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_periodo", comisionabarrotesoficina.cod_periodo);
                    cmd.Parameters.AddWithValue("@monto_arica", comisionabarrotesoficina.monto_arica);
                    cmd.Parameters.AddWithValue("@monto_ro", comisionabarrotesoficina.monto_ro);
                    cmd.Parameters.AddWithValue("@monto_total_abarr", comisionabarrotesoficina.monto_total_abarr);
                    cmd.Parameters.AddWithValue("@porcentaje_Azo_1", comisionabarrotesoficina.porcentaje_Azo_1);
                    cmd.Parameters.AddWithValue("@porcentaje_Azo_2", comisionabarrotesoficina.porcentaje_Azo_2);
                    cmd.Parameters.AddWithValue("@porcentaje_Pcat", comisionabarrotesoficina.porcentaje_Pcat);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en comisionabarrotesoficinaDAL.Insert Detalle: " + EX.Message;
                    }
                }
            }
        }

        public static string Insert_Scope(comisionabarrotesoficinaEntity comisionabarrotesoficina)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into comisionabarrotesoficina(cod_periodo, monto_arica, monto_ro, monto_total_abarr, porcentaje_Azo_1, porcentaje_Azo_2, porcentaje_Pcat) " +
                "values (@cod_periodo, @monto_arica, @monto_ro, @monto_total_abarr, @porcentaje_Azo_1, @porcentaje_Azo_2, @porcentaje_Pcat)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_periodo", comisionabarrotesoficina.cod_periodo);
                    cmd.Parameters.AddWithValue("@monto_arica", comisionabarrotesoficina.monto_arica);
                    cmd.Parameters.AddWithValue("@monto_ro", comisionabarrotesoficina.monto_ro);
                    cmd.Parameters.AddWithValue("@monto_total_abarr", comisionabarrotesoficina.monto_total_abarr);
                    cmd.Parameters.AddWithValue("@porcentaje_Azo_1", comisionabarrotesoficina.porcentaje_Azo_1);
                    cmd.Parameters.AddWithValue("@porcentaje_Azo_2", comisionabarrotesoficina.porcentaje_Azo_2);
                    cmd.Parameters.AddWithValue("@porcentaje_Pcat", comisionabarrotesoficina.porcentaje_Pcat);
                    try
                    {
                        string respuesta = cmd.ExecuteScalar().ToString();
                        return respuesta;
                    }
                    catch (Exception EX)
                    {
                        return "Error en comisionabarrotesoficinaDAL.Insert Detalle: " + EX.Message;
                    }
                }
            }
        }

        public static string Update(comisionabarrotesoficinaEntity comisionabarrotesoficina)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"update comisionabarrotesoficina " +
                "set " +
                "cod_periodo = @cod_periodo, " +
                "monto_arica = @monto_arica, " +
                "monto_ro = @monto_ro, " +
                "monto_total_abarr = @monto_total_abarr, " +
                "porcentaje_Azo_1 = @porcentaje_Azo_1, " +
                "porcentaje_Azo_2 = @porcentaje_Azo_2, " +
                "porcentaje_Pcat = @porcentaje_Pcat" +
                " where cod_periodo = @_cod_periodo ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_periodo", comisionabarrotesoficina.cod_periodo);
                    cmd.Parameters.AddWithValue("@monto_arica", comisionabarrotesoficina.monto_arica);
                    cmd.Parameters.AddWithValue("@monto_ro", comisionabarrotesoficina.monto_ro);
                    cmd.Parameters.AddWithValue("@monto_total_abarr", comisionabarrotesoficina.monto_total_abarr);
                    cmd.Parameters.AddWithValue("@porcentaje_Azo_1", comisionabarrotesoficina.porcentaje_Azo_1);
                    cmd.Parameters.AddWithValue("@porcentaje_Azo_2", comisionabarrotesoficina.porcentaje_Azo_2);
                    cmd.Parameters.AddWithValue("@porcentaje_Pcat", comisionabarrotesoficina.porcentaje_Pcat);
                    cmd.Parameters.AddWithValue("@_cod_periodo", comisionabarrotesoficina.cod_periodo);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en comisionabarrotesoficinaDAL.Update Detalle: " + EX.Message;
                    }
                }
            }
        }
        public static string Agregar(comisionabarrotesoficinaEntity u)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql_encontrar = @"SELECT count(1) from comisionabarrotesoficina where cod_periodo = @cod_periodo";
                SqlCommand cmd_encontrar = new SqlCommand(sql_encontrar, conn);
                cmd_encontrar.Parameters.AddWithValue("@cod_periodo", u.cod_periodo);
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


        public static string Delete(comisionabarrotesoficinaEntity comisionabarrotesoficina)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from comisionabarrotesoficina " +
                " where cod_periodo = @cod_periodo ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_periodo", comisionabarrotesoficina.cod_periodo);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en comisionabarrotesoficinaDAL.Delete Detalle: " + EX.Message;
                    }
                }
            }
        }
        public static string encontrar(ref comisionabarrotesoficinaEntity u)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT * from comisionabarrotesoficina where cod_periodo = @cod_periodo";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cod_periodo", u.cod_periodo);
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
                    return "Error en comisionabarrotesoficinaDAL.Validar Detalle: " + EX.Message;
                }
            }
            return "No encontrado en DAL";
        }
    }
}
