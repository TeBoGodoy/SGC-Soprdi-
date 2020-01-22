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

  public static class sp_enviadas_planificacionDAL
  {
      private static sp_enviadas_planificacionEntity Load(IDataReader reader)
      {
          sp_enviadas_planificacionEntity sp_enviadas_planificacion = new sp_enviadas_planificacionEntity();
          sp_enviadas_planificacion.cod_vendedor = Convert.ToString(reader["cod_vendedor"]);
          sp_enviadas_planificacion.detalle = Convert.ToString(reader["detalle"]);
          sp_enviadas_planificacion.fecha = DateTime.Parse(reader["fecha"].ToString());
          sp_enviadas_planificacion.fecha_planificacion = DateTime.Parse(reader["fecha_planificacion"].ToString());
          sp_enviadas_planificacion.id = int.Parse(reader["id"].ToString());
          sp_enviadas_planificacion.num_sp = Convert.ToString(reader["num_sp"]);

          return sp_enviadas_planificacion;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from sp_enviadas_planificacion "+ sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(sp_enviadas_planificacionEntity sp_enviadas_planificacion)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into sp_enviadas_planificacion(cod_vendedor, detalle, fecha, fecha_planificacion, num_sp) "+
          "values (@cod_vendedor, @detalle, @fecha, @fecha_planificacion, @num_sp)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_vendedor", sp_enviadas_planificacion.cod_vendedor);
              cmd.Parameters.AddWithValue("@detalle", sp_enviadas_planificacion.detalle);
              cmd.Parameters.AddWithValue("@fecha", sp_enviadas_planificacion.fecha);
              cmd.Parameters.AddWithValue("@fecha_planificacion", sp_enviadas_planificacion.fecha_planificacion);
              cmd.Parameters.AddWithValue("@num_sp", sp_enviadas_planificacion.num_sp);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en sp_enviadas_planificacionDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(sp_enviadas_planificacionEntity sp_enviadas_planificacion)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into sp_enviadas_planificacion(cod_vendedor, detalle, fecha, fecha_planificacion, num_sp) "+
          "values (@cod_vendedor, @detalle, @fecha, @fecha_planificacion, @num_sp)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_vendedor", sp_enviadas_planificacion.cod_vendedor);
              cmd.Parameters.AddWithValue("@detalle", sp_enviadas_planificacion.detalle);
              cmd.Parameters.AddWithValue("@fecha", sp_enviadas_planificacion.fecha);
              cmd.Parameters.AddWithValue("@fecha_planificacion", sp_enviadas_planificacion.fecha_planificacion);
              cmd.Parameters.AddWithValue("@num_sp", sp_enviadas_planificacion.num_sp);
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en sp_enviadas_planificacionDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(sp_enviadas_planificacionEntity sp_enviadas_planificacion)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update sp_enviadas_planificacion " +
          "set " +
          "cod_vendedor = @cod_vendedor, " +
          "detalle = @detalle, " +
          "fecha = @fecha, " +
          "fecha_planificacion = @fecha_planificacion, " +
          "num_sp = @num_sp" +
          " where id = @_id ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cod_vendedor", sp_enviadas_planificacion.cod_vendedor);
              cmd.Parameters.AddWithValue("@detalle", sp_enviadas_planificacion.detalle);
              cmd.Parameters.AddWithValue("@fecha", sp_enviadas_planificacion.fecha);
              cmd.Parameters.AddWithValue("@fecha_planificacion", sp_enviadas_planificacion.fecha_planificacion);
              cmd.Parameters.AddWithValue("@num_sp", sp_enviadas_planificacion.num_sp);
              cmd.Parameters.AddWithValue("@_id", sp_enviadas_planificacion.id);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en sp_enviadas_planificacionDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(sp_enviadas_planificacionEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from sp_enviadas_planificacion where id = @id"; 
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


  public static string Delete(sp_enviadas_planificacionEntity sp_enviadas_planificacion)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from sp_enviadas_planificacion " + 
          " where id = @id ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@id", sp_enviadas_planificacion.id);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en sp_enviadas_planificacionDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref sp_enviadas_planificacionEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from sp_enviadas_planificacion where id = @id"; 
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
              return "Error en sp_enviadas_planificacionDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
