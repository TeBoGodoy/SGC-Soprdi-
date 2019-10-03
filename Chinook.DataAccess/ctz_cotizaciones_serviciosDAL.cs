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

  public static class ctz_cotizaciones_serviciosDAL
  {
      private static ctz_cotizaciones_serviciosEntity Load(IDataReader reader)
      {
          ctz_cotizaciones_serviciosEntity ctz_cotizaciones_servicios = new ctz_cotizaciones_serviciosEntity();
          ctz_cotizaciones_servicios.cod_servicio = Convert.ToString(reader["cod_servicio"]);
          ctz_cotizaciones_servicios.id_cotizacion = int.Parse(reader["id_cotizacion"].ToString());
          ctz_cotizaciones_servicios.id_cotizacion_servicio = int.Parse(reader["id_cotizacion_servicio"].ToString());
          ctz_cotizaciones_servicios.nombre_servicio = Convert.ToString(reader["nombre_servicio"]);
          ctz_cotizaciones_servicios.tipo_servicio = Convert.ToString(reader["tipo_servicio"]);
          ctz_cotizaciones_servicios.valor_servicio = double.Parse(reader["valor_servicio"].ToString());

          return ctz_cotizaciones_servicios;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from ctz_cotizaciones_servicios " + sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(ctz_cotizaciones_serviciosEntity ctz_cotizaciones_servicios)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_cotizaciones_servicios(cod_servicio, id_cotizacion, nombre_servicio, tipo_servicio, valor_servicio) "+
          "values (@cod_servicio, @id_cotizacion, @nombre_servicio, @tipo_servicio, @valor_servicio)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_servicio", ctz_cotizaciones_servicios.cod_servicio);
              cmd.Parameters.AddWithValue("@id_cotizacion", ctz_cotizaciones_servicios.id_cotizacion);
              cmd.Parameters.AddWithValue("@nombre_servicio", ctz_cotizaciones_servicios.nombre_servicio);
              cmd.Parameters.AddWithValue("@tipo_servicio", ctz_cotizaciones_servicios.tipo_servicio);
              cmd.Parameters.AddWithValue("@valor_servicio", ctz_cotizaciones_servicios.valor_servicio);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en ctz_cotizaciones_serviciosDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(ctz_cotizaciones_serviciosEntity ctz_cotizaciones_servicios)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_cotizaciones_servicios(cod_servicio, id_cotizacion, nombre_servicio, tipo_servicio, valor_servicio) "+
          "values (@cod_servicio, @id_cotizacion, @nombre_servicio, @tipo_servicio, @valor_servicio);  SELECT SCOPE_IDENTITY(); ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_servicio", ctz_cotizaciones_servicios.cod_servicio); 
              cmd.Parameters.AddWithValue("@id_cotizacion", ctz_cotizaciones_servicios.id_cotizacion); 
              cmd.Parameters.AddWithValue("@nombre_servicio", ctz_cotizaciones_servicios.nombre_servicio); 
              cmd.Parameters.AddWithValue("@tipo_servicio", ctz_cotizaciones_servicios.tipo_servicio); 
              cmd.Parameters.AddWithValue("@valor_servicio", ctz_cotizaciones_servicios.valor_servicio); 
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en ctz_cotizaciones_serviciosDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(ctz_cotizaciones_serviciosEntity ctz_cotizaciones_servicios)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update ctz_cotizaciones_servicios " +
          "set " +
          "cod_servicio = @cod_servicio, " +
          "id_cotizacion = @id_cotizacion, " +
          "nombre_servicio = @nombre_servicio, " +
          "tipo_servicio = @tipo_servicio, " +
          "valor_servicio = @valor_servicio" +
          " where id_cotizacion_servicio = @_id_cotizacion_servicio ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cod_servicio", ctz_cotizaciones_servicios.cod_servicio);
              cmd.Parameters.AddWithValue("@id_cotizacion", ctz_cotizaciones_servicios.id_cotizacion);
              cmd.Parameters.AddWithValue("@nombre_servicio", ctz_cotizaciones_servicios.nombre_servicio);
              cmd.Parameters.AddWithValue("@tipo_servicio", ctz_cotizaciones_servicios.tipo_servicio);
              cmd.Parameters.AddWithValue("@valor_servicio", ctz_cotizaciones_servicios.valor_servicio);
              cmd.Parameters.AddWithValue("@_id_cotizacion_servicio", ctz_cotizaciones_servicios.id_cotizacion_servicio);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_cotizaciones_serviciosDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(ctz_cotizaciones_serviciosEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from ctz_cotizaciones_servicios where id_cotizacion_servicio = @id_cotizacion_servicio"; 
          SqlCommand cmd_encontrar = new SqlCommand(sql_encontrar, conn);
           cmd_encontrar.Parameters.AddWithValue("@id_cotizacion_servicio", u.id_cotizacion_servicio);
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


  public static string Delete(ctz_cotizaciones_serviciosEntity ctz_cotizaciones_servicios)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from ctz_cotizaciones_servicios " + 
          " where id_cotizacion_servicio = @id_cotizacion_servicio ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@id_cotizacion_servicio", ctz_cotizaciones_servicios.id_cotizacion_servicio);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_cotizaciones_serviciosDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref ctz_cotizaciones_serviciosEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from ctz_cotizaciones_servicios where id_cotizacion_servicio = @id_cotizacion_servicio"; 
          SqlCommand cmd = new SqlCommand(sql, conn);
           cmd.Parameters.AddWithValue("@id_cotizacion_servicio", u.id_cotizacion_servicio);
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
              return "Error en ctz_cotizaciones_serviciosDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
