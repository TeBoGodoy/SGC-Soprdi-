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

  public static class sp_saldosDAL
  {
      private static sp_saldosEntity Load(IDataReader reader)
      {
          sp_saldosEntity sp_saldos = new sp_saldosEntity();
          sp_saldos.cantidad_planificada = int.Parse(reader["cantidad_planificada"].ToString());
          sp_saldos.cantidad_real = int.Parse(reader["cantidad_real"].ToString());
          sp_saldos.CodProducto = Convert.ToString(reader["CodProducto"]);
          sp_saldos.CodUnVenta = Convert.ToString(reader["CodUnVenta"]);
          sp_saldos.fecha = DateTime.Parse(reader["fecha"].ToString());
          sp_saldos.id = int.Parse(reader["id"].ToString());
          sp_saldos.num_sp = Convert.ToString(reader["num_sp"]);
          sp_saldos.saldo = int.Parse(reader["saldo"].ToString());

          return sp_saldos;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from sp_saldos "+ sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(sp_saldosEntity sp_saldos)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into sp_saldos(cantidad_planificada, cantidad_real, CodProducto, CodUnVenta, fecha, num_sp, saldo) "+
          "values (@cantidad_planificada, @cantidad_real, @CodProducto, @CodUnVenta, @fecha, @num_sp, @saldo)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cantidad_planificada", sp_saldos.cantidad_planificada);
              cmd.Parameters.AddWithValue("@cantidad_real", sp_saldos.cantidad_real);
              cmd.Parameters.AddWithValue("@CodProducto", sp_saldos.CodProducto);
              cmd.Parameters.AddWithValue("@CodUnVenta", sp_saldos.CodUnVenta);
              cmd.Parameters.AddWithValue("@fecha", sp_saldos.fecha);
              cmd.Parameters.AddWithValue("@num_sp", sp_saldos.num_sp);
              cmd.Parameters.AddWithValue("@saldo", sp_saldos.saldo);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en sp_saldosDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(sp_saldosEntity sp_saldos)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into sp_saldos(cantidad_planificada, cantidad_real, CodProducto, CodUnVenta, fecha, num_sp, saldo) "+
          "values (@cantidad_planificada, @cantidad_real, @CodProducto, @CodUnVenta, @fecha, @num_sp, @saldo)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cantidad_planificada", sp_saldos.cantidad_planificada);
              cmd.Parameters.AddWithValue("@cantidad_real", sp_saldos.cantidad_real);
              cmd.Parameters.AddWithValue("@CodProducto", sp_saldos.CodProducto);
              cmd.Parameters.AddWithValue("@CodUnVenta", sp_saldos.CodUnVenta);
              cmd.Parameters.AddWithValue("@fecha", sp_saldos.fecha);
              cmd.Parameters.AddWithValue("@num_sp", sp_saldos.num_sp);
              cmd.Parameters.AddWithValue("@saldo", sp_saldos.saldo);
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en sp_saldosDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(sp_saldosEntity sp_saldos)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update sp_saldos " +
          "set " +
          "cantidad_planificada = @cantidad_planificada, " +
          "cantidad_real = @cantidad_real, " +
          "CodProducto = @CodProducto, " +
          "CodUnVenta = @CodUnVenta, " +
          "fecha = @fecha, " +
          "num_sp = @num_sp, " +
          "saldo = @saldo" +
          " where id = @_id ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cantidad_planificada", sp_saldos.cantidad_planificada);
              cmd.Parameters.AddWithValue("@cantidad_real", sp_saldos.cantidad_real);
              cmd.Parameters.AddWithValue("@CodProducto", sp_saldos.CodProducto);
              cmd.Parameters.AddWithValue("@CodUnVenta", sp_saldos.CodUnVenta);
              cmd.Parameters.AddWithValue("@fecha", sp_saldos.fecha);
              cmd.Parameters.AddWithValue("@num_sp", sp_saldos.num_sp);
              cmd.Parameters.AddWithValue("@saldo", sp_saldos.saldo);
              cmd.Parameters.AddWithValue("@_id", sp_saldos.id);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en sp_saldosDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(sp_saldosEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from sp_saldos where id = @id"; 
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


  public static string Delete(sp_saldosEntity sp_saldos)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from sp_saldos " + 
          " where id = @id ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@id", sp_saldos.id);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en sp_saldosDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref sp_saldosEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from sp_saldos where id = @id"; 
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
              return "Error en sp_saldosDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
