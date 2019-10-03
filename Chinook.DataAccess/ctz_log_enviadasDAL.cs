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

  public static class ctz_log_enviadasDAL
  {
      private static ctz_log_enviadasEntity Load(IDataReader reader)
      {
          ctz_log_enviadasEntity ctz_log_enviadas = new ctz_log_enviadasEntity();
          ctz_log_enviadas.cod_vendedor = Convert.ToString(reader["cod_vendedor"]);
          ctz_log_enviadas.correo_cliente = Convert.ToString(reader["correo_cliente"]);
          ctz_log_enviadas.estado_correo = Convert.ToString(reader["estado_correo"]);
          ctz_log_enviadas.fecha_envio = DateTime.Parse(reader["fecha_envio"].ToString());
          ctz_log_enviadas.id = int.Parse(reader["id"].ToString());
          ctz_log_enviadas.id_cotizacion_log = int.Parse(reader["id_cotizacion_log"].ToString());
          ctz_log_enviadas.rut_cliente = Convert.ToString(reader["rut_cliente"]);

          return ctz_log_enviadas;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from ctz_log_enviadas " + sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(ctz_log_enviadasEntity ctz_log_enviadas)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_log_enviadas(cod_vendedor, correo_cliente, estado_correo, fecha_envio, id_cotizacion_log, rut_cliente) "+
          "values (@cod_vendedor, @correo_cliente, @estado_correo, @fecha_envio, @id_cotizacion_log, @rut_cliente)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_vendedor", ctz_log_enviadas.cod_vendedor);
              cmd.Parameters.AddWithValue("@correo_cliente", ctz_log_enviadas.correo_cliente);
              cmd.Parameters.AddWithValue("@estado_correo", ctz_log_enviadas.estado_correo);
              cmd.Parameters.AddWithValue("@fecha_envio", ctz_log_enviadas.fecha_envio);
              cmd.Parameters.AddWithValue("@id_cotizacion_log", ctz_log_enviadas.id_cotizacion_log);
              cmd.Parameters.AddWithValue("@rut_cliente", ctz_log_enviadas.rut_cliente);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en ctz_log_enviadasDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(ctz_log_enviadasEntity ctz_log_enviadas)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_log_enviadas(cod_vendedor, correo_cliente, estado_correo, fecha_envio, id_cotizacion_log, rut_cliente) "+
          "values (@cod_vendedor, @correo_cliente, @estado_correo, @fecha_envio, @id_cotizacion_log, @rut_cliente);  SELECT SCOPE_IDENTITY(); ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_vendedor", ctz_log_enviadas.cod_vendedor); 
              cmd.Parameters.AddWithValue("@correo_cliente", ctz_log_enviadas.correo_cliente); 
              cmd.Parameters.AddWithValue("@estado_correo", ctz_log_enviadas.estado_correo); 
              cmd.Parameters.AddWithValue("@fecha_envio", ctz_log_enviadas.fecha_envio); 
              cmd.Parameters.AddWithValue("@id_cotizacion_log", ctz_log_enviadas.id_cotizacion_log); 
              cmd.Parameters.AddWithValue("@rut_cliente", ctz_log_enviadas.rut_cliente); 
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en ctz_log_enviadasDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(ctz_log_enviadasEntity ctz_log_enviadas)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update ctz_log_enviadas " +
          "set " +
          "cod_vendedor = @cod_vendedor, " +
          "correo_cliente = @correo_cliente, " +
          "estado_correo = @estado_correo, " +
          "fecha_envio = @fecha_envio, " +
          "id_cotizacion_log = @id_cotizacion_log, " +
          "rut_cliente = @rut_cliente" +
          " where id = @_id ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cod_vendedor", ctz_log_enviadas.cod_vendedor);
              cmd.Parameters.AddWithValue("@correo_cliente", ctz_log_enviadas.correo_cliente);
              cmd.Parameters.AddWithValue("@estado_correo", ctz_log_enviadas.estado_correo);
              cmd.Parameters.AddWithValue("@fecha_envio", ctz_log_enviadas.fecha_envio);
              cmd.Parameters.AddWithValue("@id_cotizacion_log", ctz_log_enviadas.id_cotizacion_log);
              cmd.Parameters.AddWithValue("@rut_cliente", ctz_log_enviadas.rut_cliente);
              cmd.Parameters.AddWithValue("@_id", ctz_log_enviadas.id);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_log_enviadasDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(ctz_log_enviadasEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from ctz_log_enviadas where id = @id"; 
          SqlCommand cmd_encontrar = new SqlCommand(sql_encontrar, conn);
           cmd_encontrar.Parameters.AddWithValue("@id", u.id);
              try
              {
                  string respuesta = cmd_encontrar.ExecuteScalar().ToString(); 
                  if (Convert.ToInt32(respuesta) > 0){ 
                       string resp = Update(u); 
                       if (resp == "OK"){return "2";}else { return "3"; } 
                  }else{ 
                       string resp = Insert(u);
                       if (resp == "OK"){return "1";}else { return "3"; } 
                  } 
              }
              catch (Exception EX)
              {
              return "3";
              }
          }
  }


  public static string Delete(ctz_log_enviadasEntity ctz_log_enviadas)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from ctz_log_enviadas " + 
          " where id = @id ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@id", ctz_log_enviadas.id);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_log_enviadasDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref ctz_log_enviadasEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from ctz_log_enviadas where id = @id"; 
          SqlCommand cmd = new SqlCommand(sql, conn);
           cmd.Parameters.AddWithValue("@id", u.id);
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
              return "Error en ctz_log_enviadasDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
