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

  public static class comisionperiodocierre_cobranzaDAL
  {
      private static comisionperiodocierre_cobranzaEntity Load(IDataReader reader)
      {
          comisionperiodocierre_cobranzaEntity comisionperiodocierre_cobranza = new comisionperiodocierre_cobranzaEntity();
          comisionperiodocierre_cobranza.cod_periodo = Convert.ToString(reader["cod_periodo"]);
          comisionperiodocierre_cobranza.númfactura = Convert.ToString(reader["númfactura"]);

          return comisionperiodocierre_cobranza;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from comisionperiodocierre_cobranza "+ sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(comisionperiodocierre_cobranzaEntity comisionperiodocierre_cobranza)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into comisionperiodocierre_cobranza(cod_periodo, númfactura) "+
          "values (@cod_periodo, @númfactura)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_periodo", comisionperiodocierre_cobranza.cod_periodo);
              cmd.Parameters.AddWithValue("@númfactura", comisionperiodocierre_cobranza.númfactura);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en comisionperiodocierre_cobranzaDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(comisionperiodocierre_cobranzaEntity comisionperiodocierre_cobranza)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into comisionperiodocierre_cobranza(cod_periodo, númfactura) "+
          "values (@cod_periodo, @númfactura)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_periodo", comisionperiodocierre_cobranza.cod_periodo);
              cmd.Parameters.AddWithValue("@númfactura", comisionperiodocierre_cobranza.númfactura);
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en comisionperiodocierre_cobranzaDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(comisionperiodocierre_cobranzaEntity comisionperiodocierre_cobranza)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update comisionperiodocierre_cobranza " +
          "set " +
          "cod_periodo = @cod_periodo, " +
          "númfactura = @númfactura" +
          " where cod_periodo = @_cod_periodo And númfactura = @_númfactura ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cod_periodo", comisionperiodocierre_cobranza.cod_periodo);
              cmd.Parameters.AddWithValue("@númfactura", comisionperiodocierre_cobranza.númfactura);
              cmd.Parameters.AddWithValue("@_cod_periodo", comisionperiodocierre_cobranza.cod_periodo);
              cmd.Parameters.AddWithValue("@_númfactura", comisionperiodocierre_cobranza.númfactura);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en comisionperiodocierre_cobranzaDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(comisionperiodocierre_cobranzaEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from comisionperiodocierre_cobranza where cod_periodo = @cod_periodo And númfactura = @númfactura"; 
          SqlCommand cmd_encontrar = new SqlCommand(sql_encontrar, conn);
           cmd_encontrar.Parameters.AddWithValue("@cod_periodo", u.cod_periodo);
           cmd_encontrar.Parameters.AddWithValue("@númfactura", u.númfactura);
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


  public static string Delete(comisionperiodocierre_cobranzaEntity comisionperiodocierre_cobranza)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from comisionperiodocierre_cobranza " + 
          " where cod_periodo = @cod_periodo And númfactura = @númfactura ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@cod_periodo", comisionperiodocierre_cobranza.cod_periodo);
              cmd.Parameters.AddWithValue("@númfactura", comisionperiodocierre_cobranza.númfactura);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en comisionperiodocierre_cobranzaDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref comisionperiodocierre_cobranzaEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from comisionperiodocierre_cobranza where cod_periodo = @cod_periodo And númfactura = @númfactura"; 
          SqlCommand cmd = new SqlCommand(sql, conn);
           cmd.Parameters.AddWithValue("@cod_periodo", u.cod_periodo);
           cmd.Parameters.AddWithValue("@númfactura", u.númfactura);
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
              return "Error en comisionperiodocierre_cobranzaDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
