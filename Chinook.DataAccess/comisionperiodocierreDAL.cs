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

  public static class comisionperiodocierreDAL
  {
      private static comisionperiodocierreEntity Load(IDataReader reader)
      {
          comisionperiodocierreEntity comisionperiodocierre = new comisionperiodocierreEntity();
          comisionperiodocierre.autoriza = Convert.ToString(reader["autoriza"]);
          comisionperiodocierre.cod_periodo = Convert.ToString(reader["cod_periodo"]);
          comisionperiodocierre.cod_usuario = Convert.ToString(reader["cod_usuario"]);
          comisionperiodocierre.fecha_autoriza = DateTime.Parse(reader["fecha_autoriza"].ToString());
          comisionperiodocierre.fecha_cierre = DateTime.Parse(reader["fecha_cierre"].ToString());

          return comisionperiodocierre;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from comisionperiodocierre "+ sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(comisionperiodocierreEntity comisionperiodocierre)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into comisionperiodocierre(autoriza, cod_periodo, cod_usuario, fecha_autoriza, fecha_cierre) "+
          "values (@autoriza, @cod_periodo, @cod_usuario, @fecha_autoriza, @fecha_cierre)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@autoriza", comisionperiodocierre.autoriza);
              cmd.Parameters.AddWithValue("@cod_periodo", comisionperiodocierre.cod_periodo);
              cmd.Parameters.AddWithValue("@cod_usuario", comisionperiodocierre.cod_usuario);
              cmd.Parameters.AddWithValue("@fecha_autoriza", comisionperiodocierre.fecha_autoriza);
              cmd.Parameters.AddWithValue("@fecha_cierre", comisionperiodocierre.fecha_cierre);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en comisionperiodocierreDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(comisionperiodocierreEntity comisionperiodocierre)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into comisionperiodocierre(autoriza, cod_periodo, cod_usuario, fecha_autoriza, fecha_cierre) "+
          "values (@autoriza, @cod_periodo, @cod_usuario, @fecha_autoriza, @fecha_cierre)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@autoriza", comisionperiodocierre.autoriza);
              cmd.Parameters.AddWithValue("@cod_periodo", comisionperiodocierre.cod_periodo);
              cmd.Parameters.AddWithValue("@cod_usuario", comisionperiodocierre.cod_usuario);
              cmd.Parameters.AddWithValue("@fecha_autoriza", comisionperiodocierre.fecha_autoriza);
              cmd.Parameters.AddWithValue("@fecha_cierre", comisionperiodocierre.fecha_cierre);
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en comisionperiodocierreDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(comisionperiodocierreEntity comisionperiodocierre)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update comisionperiodocierre " +
          "set " +
          "autoriza = @autoriza, " +
          "cod_periodo = @cod_periodo, " +
          "cod_usuario = @cod_usuario, " +
          "fecha_autoriza = @fecha_autoriza, " +
          "fecha_cierre = @fecha_cierre" +
          " where cod_periodo = @_cod_periodo ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@autoriza", comisionperiodocierre.autoriza);
              cmd.Parameters.AddWithValue("@cod_periodo", comisionperiodocierre.cod_periodo);
              cmd.Parameters.AddWithValue("@cod_usuario", comisionperiodocierre.cod_usuario);
              cmd.Parameters.AddWithValue("@fecha_autoriza", comisionperiodocierre.fecha_autoriza);
              cmd.Parameters.AddWithValue("@fecha_cierre", comisionperiodocierre.fecha_cierre);
              cmd.Parameters.AddWithValue("@_cod_periodo", comisionperiodocierre.cod_periodo);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en comisionperiodocierreDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(comisionperiodocierreEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from comisionperiodocierre where cod_periodo = @cod_periodo"; 
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


  public static string Delete(comisionperiodocierreEntity comisionperiodocierre)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from comisionperiodocierre " + 
          " where cod_periodo = @cod_periodo ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@cod_periodo", comisionperiodocierre.cod_periodo);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en comisionperiodocierreDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref comisionperiodocierreEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from comisionperiodocierre where cod_periodo = @cod_periodo"; 
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
              return "Error en comisionperiodocierreDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
