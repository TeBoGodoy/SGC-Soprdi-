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

    public static class agendacontactoDAL
    {
        private static agendacontactoEntity Load(IDataReader reader)
        {
            agendacontactoEntity agendacontacto = new agendacontactoEntity();
            agendacontacto.am_pm = Convert.ToString(reader["am_pm"]);
            agendacontacto.codvendedor = Convert.ToString(reader["codvendedor"]);
            agendacontacto.fecha_agenda = DateTime.Parse(reader["fecha_agenda"].ToString());
            agendacontacto.nombre_contacto = Convert.ToString(reader["nombre_contacto"]);
            agendacontacto.observacion = Convert.ToString(reader["observacion"]);
            agendacontacto.rutcliente = Convert.ToString(reader["rutcliente"]);

            return agendacontacto;
        }

        public static DataTable GetAll(string sql_where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT *  from V_AGENDADO_VENTA " + sql_where + " order by CONVERT(DATETIME, fecha_AGENDA, 103) DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        public static string Insert(agendacontactoEntity agendacontacto)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into agendacontacto(am_pm, codvendedor, fecha_agenda, nombre_contacto, observacion, rutcliente) " +
                "values (@am_pm, @codvendedor, @fecha_agenda, @nombre_contacto, @observacion, @rutcliente)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@am_pm", agendacontacto.am_pm);
                    cmd.Parameters.AddWithValue("@codvendedor", agendacontacto.codvendedor);
                    cmd.Parameters.AddWithValue("@fecha_agenda", agendacontacto.fecha_agenda);
                    cmd.Parameters.AddWithValue("@nombre_contacto", agendacontacto.nombre_contacto);
                    cmd.Parameters.AddWithValue("@observacion", agendacontacto.observacion);
                    cmd.Parameters.AddWithValue("@rutcliente", agendacontacto.rutcliente);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en agendacontactoDAL.Insert Detalle: " + EX.Message;
                    }
                }
            }
        }

        public static string Insert_Scope(agendacontactoEntity agendacontacto)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into agendacontacto(am_pm, codvendedor, fecha_agenda, nombre_contacto, observacion, rutcliente) " +
                "values (@am_pm, @codvendedor, @fecha_agenda, @nombre_contacto, @observacion, @rutcliente)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@am_pm", agendacontacto.am_pm);
                    cmd.Parameters.AddWithValue("@codvendedor", agendacontacto.codvendedor);
                    cmd.Parameters.AddWithValue("@fecha_agenda", agendacontacto.fecha_agenda);
                    cmd.Parameters.AddWithValue("@nombre_contacto", agendacontacto.nombre_contacto);
                    cmd.Parameters.AddWithValue("@observacion", agendacontacto.observacion);
                    cmd.Parameters.AddWithValue("@rutcliente", agendacontacto.rutcliente);
                    try
                    {
                        string respuesta = cmd.ExecuteScalar().ToString();
                        return respuesta;
                    }
                    catch (Exception EX)
                    {
                        return "Error en agendacontactoDAL.Insert Detalle: " + EX.Message;
                    }
                }
            }
        }

        public static string Update(agendacontactoEntity agendacontacto, DateTime fecha_antigua)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"update agendacontacto " +
                "set " +
                "am_pm = @am_pm, " +
                "codvendedor = @codvendedor, " +
                "fecha_agenda = @fecha_agenda, " +
                "nombre_contacto = @nombre_contacto, " +
                "observacion = @observacion, " +
                "rutcliente = @rutcliente" +
                " where codvendedor = @_codvendedor And fecha_agenda = @fecha_pk And nombre_contacto = @_nombre_contacto And rutcliente = @_rutcliente ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@am_pm", agendacontacto.am_pm);
                    cmd.Parameters.AddWithValue("@codvendedor", agendacontacto.codvendedor);
                    cmd.Parameters.AddWithValue("@fecha_agenda", agendacontacto.fecha_agenda);
                    cmd.Parameters.AddWithValue("@nombre_contacto", agendacontacto.nombre_contacto);
                    cmd.Parameters.AddWithValue("@observacion", agendacontacto.observacion);
                    cmd.Parameters.AddWithValue("@rutcliente", agendacontacto.rutcliente);
                    cmd.Parameters.AddWithValue("@_codvendedor", agendacontacto.codvendedor);
                    cmd.Parameters.AddWithValue("@_fecha_agenda", agendacontacto.fecha_agenda);
                    cmd.Parameters.AddWithValue("@_nombre_contacto", agendacontacto.nombre_contacto);
                    cmd.Parameters.AddWithValue("@_rutcliente", agendacontacto.rutcliente);

                    cmd.Parameters.AddWithValue("@fecha_pk", fecha_antigua);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en agendacontactoDAL.Update Detalle: " + EX.Message;
                    }
                }
            }
        }
        public static string Agregar(agendacontactoEntity u)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql_encontrar = @"SELECT count(1) from agendacontacto where codvendedor = @codvendedor And fecha_agenda = @fecha_agenda And nombre_contacto = @nombre_contacto And rutcliente = @rutcliente";
                SqlCommand cmd_encontrar = new SqlCommand(sql_encontrar, conn);
                cmd_encontrar.Parameters.AddWithValue("@codvendedor", u.codvendedor);
                cmd_encontrar.Parameters.AddWithValue("@fecha_agenda", u.fecha_agenda);
                cmd_encontrar.Parameters.AddWithValue("@nombre_contacto", u.nombre_contacto);
                cmd_encontrar.Parameters.AddWithValue("@rutcliente", u.rutcliente);
                try
                {
                    string respuesta = cmd_encontrar.ExecuteScalar().ToString();
                    if (Convert.ToInt32(respuesta) > 0)
                    {
                        string resp = Update(u, u.fecha_agenda);
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


        public static string Delete(agendacontactoEntity agendacontacto)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from agendacontacto " +
                " where codvendedor = @codvendedor And fecha_agenda = @fecha_agenda And nombre_contacto = @nombre_contacto And rutcliente = @rutcliente ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@codvendedor", agendacontacto.codvendedor);
                    cmd.Parameters.AddWithValue("@fecha_agenda", agendacontacto.fecha_agenda);
                    cmd.Parameters.AddWithValue("@nombre_contacto", agendacontacto.nombre_contacto);
                    cmd.Parameters.AddWithValue("@rutcliente", agendacontacto.rutcliente);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en agendacontactoDAL.Delete Detalle: " + EX.Message;
                    }
                }
            }
        }
        public static string encontrar(ref agendacontactoEntity u)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT * from agendacontacto where codvendedor = @codvendedor And fecha_agenda = @fecha_agenda And nombre_contacto = @nombre_contacto And rutcliente = @rutcliente";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@codvendedor", u.codvendedor);
                cmd.Parameters.AddWithValue("@fecha_agenda", u.fecha_agenda);
                cmd.Parameters.AddWithValue("@nombre_contacto", u.nombre_contacto);
                cmd.Parameters.AddWithValue("@rutcliente", u.rutcliente);
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
                    return "Error en agendacontactoDAL.Validar Detalle: " + EX.Message;
                }
            }
            return "No encontrado en DAL";
        }
    }
}
