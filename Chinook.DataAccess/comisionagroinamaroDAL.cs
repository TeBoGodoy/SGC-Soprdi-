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

  public static class comisionagroinamaroDAL
  {
      private static comisionagroinamaroEntity Load(IDataReader reader)
      {
          comisionagroinamaroEntity comisionagroinamaro = new comisionagroinamaroEntity();
          comisionagroinamaro.cod_periodo = Convert.ToString(reader["cod_periodo"]);
          comisionagroinamaro.monto_dolar = Double.Parse(reader["monto_dolar"].ToString());
          comisionagroinamaro.tipo_cambio = Double.Parse(reader["tipo_cambio"].ToString());

          return comisionagroinamaro;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from comisionagroinamaro "+ sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(comisionagroinamaroEntity comisionagroinamaro)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into comisionagroinamaro(cod_periodo, monto_dolar, tipo_cambio) "+
          "values (@cod_periodo, @monto_dolar, @tipo_cambio)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_periodo", comisionagroinamaro.cod_periodo);
              cmd.Parameters.AddWithValue("@monto_dolar", comisionagroinamaro.monto_dolar);
              cmd.Parameters.AddWithValue("@tipo_cambio", comisionagroinamaro.tipo_cambio);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en comisionagroinamaroDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(comisionagroinamaroEntity comisionagroinamaro)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into comisionagroinamaro(cod_periodo, monto_dolar, tipo_cambio) "+
          "values (@cod_periodo, @monto_dolar, @tipo_cambio)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_periodo", comisionagroinamaro.cod_periodo);
              cmd.Parameters.AddWithValue("@monto_dolar", comisionagroinamaro.monto_dolar);
              cmd.Parameters.AddWithValue("@tipo_cambio", comisionagroinamaro.tipo_cambio);
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en comisionagroinamaroDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(comisionagroinamaroEntity comisionagroinamaro)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update comisionagroinamaro " +
          "set " +
          "cod_periodo = @cod_periodo, " +
          "monto_dolar = @monto_dolar, " +
          "tipo_cambio = @tipo_cambio" +
          " where cod_periodo = @_cod_periodo ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cod_periodo", comisionagroinamaro.cod_periodo);
              cmd.Parameters.AddWithValue("@monto_dolar", comisionagroinamaro.monto_dolar);
              cmd.Parameters.AddWithValue("@tipo_cambio", comisionagroinamaro.tipo_cambio);
              cmd.Parameters.AddWithValue("@_cod_periodo", comisionagroinamaro.cod_periodo);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en comisionagroinamaroDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(comisionagroinamaroEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from comisionagroinamaro where cod_periodo = @cod_periodo"; 
          SqlCommand cmd_encontrar = new SqlCommand(sql_encontrar, conn);
           cmd_encontrar.Parameters.AddWithValue("@cod_periodo", u.cod_periodo);
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


  public static string Delete(comisionagroinamaroEntity comisionagroinamaro)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from comisionagroinamaro " + 
          " where cod_periodo = @cod_periodo ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@cod_periodo", comisionagroinamaro.cod_periodo);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en comisionagroinamaroDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref comisionagroinamaroEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from comisionagroinamaro where cod_periodo = @cod_periodo"; 
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
              return "Error en comisionagroinamaroDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
