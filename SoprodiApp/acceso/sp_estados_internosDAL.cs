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

  public static class sp_estados_internosDAL
  {
      private static sp_estados_internosEntity Load(IDataReader reader)
      {
          sp_estados_internosEntity sp_estados_internos = new sp_estados_internosEntity();
          sp_estados_internos.cod_usuario = Convert.ToString(reader["cod_usuario"]);
          sp_estados_internos.estado = Convert.ToString(reader["estado"]);
          sp_estados_internos.fecha = DateTime.Parse(reader["fecha"].ToString());
          sp_estados_internos.id = int.Parse(reader["id"].ToString());
          sp_estados_internos.nota_correo = Convert.ToString(reader["nota_correo"]);
          sp_estados_internos.num_sp = Convert.ToString(reader["num_sp"]);

          return sp_estados_internos;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from sp_estados_internos "+ sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(sp_estados_internosEntity sp_estados_internos)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into sp_estados_internos(cod_usuario, estado, fecha, nota_correo, num_sp) "+
          "values (@cod_usuario, @estado, @fecha, @nota_correo, @num_sp)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_usuario", sp_estados_internos.cod_usuario);
              cmd.Parameters.AddWithValue("@estado", sp_estados_internos.estado);
              cmd.Parameters.AddWithValue("@fecha", sp_estados_internos.fecha);
              cmd.Parameters.AddWithValue("@nota_correo", sp_estados_internos.nota_correo);
              cmd.Parameters.AddWithValue("@num_sp", sp_estados_internos.num_sp);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en sp_estados_internosDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(sp_estados_internosEntity sp_estados_internos)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into sp_estados_internos(cod_usuario, estado, fecha, nota_correo, num_sp) "+
          "values (@cod_usuario, @estado, @fecha, @nota_correo, @num_sp)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_usuario", sp_estados_internos.cod_usuario);
              cmd.Parameters.AddWithValue("@estado", sp_estados_internos.estado);
              cmd.Parameters.AddWithValue("@fecha", sp_estados_internos.fecha);
              cmd.Parameters.AddWithValue("@nota_correo", sp_estados_internos.nota_correo);
              cmd.Parameters.AddWithValue("@num_sp", sp_estados_internos.num_sp);
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en sp_estados_internosDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(sp_estados_internosEntity sp_estados_internos)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update sp_estados_internos " +
          "set " +
          "cod_usuario = @cod_usuario, " +
          "estado = @estado, " +
          "fecha = @fecha, " +
          "nota_correo = @nota_correo, " +
          "num_sp = @num_sp" +
          " where id = @_id ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cod_usuario", sp_estados_internos.cod_usuario);
              cmd.Parameters.AddWithValue("@estado", sp_estados_internos.estado);
              cmd.Parameters.AddWithValue("@fecha", sp_estados_internos.fecha);
              cmd.Parameters.AddWithValue("@nota_correo", sp_estados_internos.nota_correo);
              cmd.Parameters.AddWithValue("@num_sp", sp_estados_internos.num_sp);
              cmd.Parameters.AddWithValue("@_id", sp_estados_internos.id);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en sp_estados_internosDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(sp_estados_internosEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from sp_estados_internos where id = @id"; 
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


  public static string Delete(sp_estados_internosEntity sp_estados_internos)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from sp_estados_internos " + 
          " where id = @id ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@id", sp_estados_internos.id);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en sp_estados_internosDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref sp_estados_internosEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from sp_estados_internos where id = @id"; 
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
              return "Error en sp_estados_internosDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
