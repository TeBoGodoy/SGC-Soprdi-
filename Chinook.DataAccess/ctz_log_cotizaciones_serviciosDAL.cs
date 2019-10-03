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

  public static class ctz_log_cotizaciones_serviciosDAL
  {
      private static ctz_log_cotizaciones_serviciosEntity Load(IDataReader reader)
      {
          ctz_log_cotizaciones_serviciosEntity ctz_log_cotizaciones_servicios = new ctz_log_cotizaciones_serviciosEntity();
          ctz_log_cotizaciones_servicios.cod_servicio = Convert.ToString(reader["cod_servicio"]);
          ctz_log_cotizaciones_servicios.id_cotizacion_log = int.Parse(reader["id_cotizacion_log"].ToString());
          ctz_log_cotizaciones_servicios.id_cotizacion_servicio_log = int.Parse(reader["id_cotizacion_servicio_log"].ToString());
          ctz_log_cotizaciones_servicios.nombre_servicio = Convert.ToString(reader["nombre_servicio"]);
          ctz_log_cotizaciones_servicios.tipo_servicio = Convert.ToString(reader["tipo_servicio"]);
          ctz_log_cotizaciones_servicios.valor_servicio = double.Parse(reader["valor_servicio"].ToString());

          return ctz_log_cotizaciones_servicios;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from ctz_log_cotizaciones_servicios " + sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(ctz_log_cotizaciones_serviciosEntity ctz_log_cotizaciones_servicios)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_log_cotizaciones_servicios(cod_servicio, id_cotizacion_log, nombre_servicio, tipo_servicio, valor_servicio) "+
          "values (@cod_servicio, @id_cotizacion_log, @nombre_servicio, @tipo_servicio, @valor_servicio)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_servicio", ctz_log_cotizaciones_servicios.cod_servicio);
              cmd.Parameters.AddWithValue("@id_cotizacion_log", ctz_log_cotizaciones_servicios.id_cotizacion_log);
              cmd.Parameters.AddWithValue("@nombre_servicio", ctz_log_cotizaciones_servicios.nombre_servicio);
              cmd.Parameters.AddWithValue("@tipo_servicio", ctz_log_cotizaciones_servicios.tipo_servicio);
              cmd.Parameters.AddWithValue("@valor_servicio", ctz_log_cotizaciones_servicios.valor_servicio);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en ctz_log_cotizaciones_serviciosDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(ctz_log_cotizaciones_serviciosEntity ctz_log_cotizaciones_servicios)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_log_cotizaciones_servicios(cod_servicio, id_cotizacion_log, nombre_servicio, tipo_servicio, valor_servicio) "+
          "values (@cod_servicio, @id_cotizacion_log, @nombre_servicio, @tipo_servicio, @valor_servicio);  SELECT SCOPE_IDENTITY(); ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_servicio", ctz_log_cotizaciones_servicios.cod_servicio); 
              cmd.Parameters.AddWithValue("@id_cotizacion_log", ctz_log_cotizaciones_servicios.id_cotizacion_log); 
              cmd.Parameters.AddWithValue("@nombre_servicio", ctz_log_cotizaciones_servicios.nombre_servicio); 
              cmd.Parameters.AddWithValue("@tipo_servicio", ctz_log_cotizaciones_servicios.tipo_servicio); 
              cmd.Parameters.AddWithValue("@valor_servicio", ctz_log_cotizaciones_servicios.valor_servicio); 
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en ctz_log_cotizaciones_serviciosDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(ctz_log_cotizaciones_serviciosEntity ctz_log_cotizaciones_servicios)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update ctz_log_cotizaciones_servicios " +
          "set " +
          "cod_servicio = @cod_servicio, " +
          "id_cotizacion_log = @id_cotizacion_log, " +
          "nombre_servicio = @nombre_servicio, " +
          "tipo_servicio = @tipo_servicio, " +
          "valor_servicio = @valor_servicio" +
          " where id_cotizacion_servicio_log = @_id_cotizacion_servicio_log ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cod_servicio", ctz_log_cotizaciones_servicios.cod_servicio);
              cmd.Parameters.AddWithValue("@id_cotizacion_log", ctz_log_cotizaciones_servicios.id_cotizacion_log);
              cmd.Parameters.AddWithValue("@nombre_servicio", ctz_log_cotizaciones_servicios.nombre_servicio);
              cmd.Parameters.AddWithValue("@tipo_servicio", ctz_log_cotizaciones_servicios.tipo_servicio);
              cmd.Parameters.AddWithValue("@valor_servicio", ctz_log_cotizaciones_servicios.valor_servicio);
              cmd.Parameters.AddWithValue("@_id_cotizacion_servicio_log", ctz_log_cotizaciones_servicios.id_cotizacion_servicio_log);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_log_cotizaciones_serviciosDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(ctz_log_cotizaciones_serviciosEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from ctz_log_cotizaciones_servicios where id_cotizacion_servicio_log = @id_cotizacion_servicio_log"; 
          SqlCommand cmd_encontrar = new SqlCommand(sql_encontrar, conn);
           cmd_encontrar.Parameters.AddWithValue("@id_cotizacion_servicio_log", u.id_cotizacion_servicio_log);
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


  public static string Delete(ctz_log_cotizaciones_serviciosEntity ctz_log_cotizaciones_servicios)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from ctz_log_cotizaciones_servicios " + 
          " where id_cotizacion_servicio_log = @id_cotizacion_servicio_log ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@id_cotizacion_servicio_log", ctz_log_cotizaciones_servicios.id_cotizacion_servicio_log);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_log_cotizaciones_serviciosDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref ctz_log_cotizaciones_serviciosEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from ctz_log_cotizaciones_servicios where id_cotizacion_servicio_log = @id_cotizacion_servicio_log"; 
          SqlCommand cmd = new SqlCommand(sql, conn);
           cmd.Parameters.AddWithValue("@id_cotizacion_servicio_log", u.id_cotizacion_servicio_log);
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
              return "Error en ctz_log_cotizaciones_serviciosDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
