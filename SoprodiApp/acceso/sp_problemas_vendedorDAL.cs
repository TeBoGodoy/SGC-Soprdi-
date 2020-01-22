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

  public static class sp_problemas_vendedorDAL
  {
      private static sp_problemas_vendedorEntity Load(IDataReader reader)
      {
          sp_problemas_vendedorEntity sp_problemas_vendedor = new sp_problemas_vendedorEntity();
          sp_problemas_vendedor.cod_vendedor = Convert.ToString(reader["cod_vendedor"]);
          sp_problemas_vendedor.detalle = Convert.ToString(reader["detalle"]);
          sp_problemas_vendedor.estado = Convert.ToString(reader["estado"]);
          sp_problemas_vendedor.fecha = DateTime.Parse(reader["fecha"].ToString());
          sp_problemas_vendedor.id = int.Parse(reader["id"].ToString());
          sp_problemas_vendedor.num_sp = Convert.ToString(reader["num_sp"]);

          return sp_problemas_vendedor;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from sp_problemas_vendedor "+ sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(sp_problemas_vendedorEntity sp_problemas_vendedor)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into sp_problemas_vendedor(cod_vendedor, detalle, estado, fecha, num_sp) "+
          "values (@cod_vendedor, @detalle, @estado, @fecha, @num_sp)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_vendedor", sp_problemas_vendedor.cod_vendedor);
              cmd.Parameters.AddWithValue("@detalle", sp_problemas_vendedor.detalle);
              cmd.Parameters.AddWithValue("@estado", sp_problemas_vendedor.estado);
              cmd.Parameters.AddWithValue("@fecha", sp_problemas_vendedor.fecha);
              cmd.Parameters.AddWithValue("@num_sp", sp_problemas_vendedor.num_sp);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en sp_problemas_vendedorDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(sp_problemas_vendedorEntity sp_problemas_vendedor)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into sp_problemas_vendedor(cod_vendedor, detalle, estado, fecha, num_sp) "+
          "values (@cod_vendedor, @detalle, @estado, @fecha, @num_sp)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_vendedor", sp_problemas_vendedor.cod_vendedor);
              cmd.Parameters.AddWithValue("@detalle", sp_problemas_vendedor.detalle);
              cmd.Parameters.AddWithValue("@estado", sp_problemas_vendedor.estado);
              cmd.Parameters.AddWithValue("@fecha", sp_problemas_vendedor.fecha);
              cmd.Parameters.AddWithValue("@num_sp", sp_problemas_vendedor.num_sp);
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en sp_problemas_vendedorDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(sp_problemas_vendedorEntity sp_problemas_vendedor)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update sp_problemas_vendedor " +
          "set " +
          "cod_vendedor = @cod_vendedor, " +
          "detalle = @detalle, " +
          "estado = @estado, " +
          "fecha = @fecha, " +
          "num_sp = @num_sp" +
          " where id = @_id ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cod_vendedor", sp_problemas_vendedor.cod_vendedor);
              cmd.Parameters.AddWithValue("@detalle", sp_problemas_vendedor.detalle);
              cmd.Parameters.AddWithValue("@estado", sp_problemas_vendedor.estado);
              cmd.Parameters.AddWithValue("@fecha", sp_problemas_vendedor.fecha);
              cmd.Parameters.AddWithValue("@num_sp", sp_problemas_vendedor.num_sp);
              cmd.Parameters.AddWithValue("@_id", sp_problemas_vendedor.id);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en sp_problemas_vendedorDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(sp_problemas_vendedorEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from sp_problemas_vendedor where id = @id"; 
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


  public static string Delete(sp_problemas_vendedorEntity sp_problemas_vendedor)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from sp_problemas_vendedor " + 
          " where id = @id ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@id", sp_problemas_vendedor.id);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en sp_problemas_vendedorDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref sp_problemas_vendedorEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from sp_problemas_vendedor where id = @id"; 
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
              return "Error en sp_problemas_vendedorDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
