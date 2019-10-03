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

  public static class cobranza_pago_chequeDAL
  {
      private static cobranza_pago_chequeEntity Load(IDataReader reader)
      {
          cobranza_pago_chequeEntity cobranza_pago_cheque = new cobranza_pago_chequeEntity();
          cobranza_pago_cheque.cuenta_banco = int.Parse(reader["cuenta_banco"].ToString());
          cobranza_pago_cheque.fecha_pago = DateTime.Parse(reader["fecha_pago"].ToString());
          cobranza_pago_cheque.id = int.Parse(reader["id"].ToString());
          cobranza_pago_cheque.num_cheque = Convert.ToString(reader["num_cheque"]);
          cobranza_pago_cheque.usuario = Convert.ToString(reader["usuario"]);

          return cobranza_pago_cheque;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from cobranza_pago_cheque "+ sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(cobranza_pago_chequeEntity cobranza_pago_cheque)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into cobranza_pago_cheque(cuenta_banco, fecha_pago, num_cheque, usuario) "+
          "values (@cuenta_banco, @fecha_pago, @num_cheque, @usuario)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cuenta_banco", cobranza_pago_cheque.cuenta_banco);
              cmd.Parameters.AddWithValue("@fecha_pago", cobranza_pago_cheque.fecha_pago);
              cmd.Parameters.AddWithValue("@num_cheque", cobranza_pago_cheque.num_cheque);
              cmd.Parameters.AddWithValue("@usuario", cobranza_pago_cheque.usuario);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en cobranza_pago_chequeDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(cobranza_pago_chequeEntity cobranza_pago_cheque)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into cobranza_pago_cheque(cuenta_banco, fecha_pago, num_cheque, usuario) "+
          "values (@cuenta_banco, @fecha_pago, @num_cheque, @usuario)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cuenta_banco", cobranza_pago_cheque.cuenta_banco);
              cmd.Parameters.AddWithValue("@fecha_pago", cobranza_pago_cheque.fecha_pago);
              cmd.Parameters.AddWithValue("@num_cheque", cobranza_pago_cheque.num_cheque);
              cmd.Parameters.AddWithValue("@usuario", cobranza_pago_cheque.usuario);
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en cobranza_pago_chequeDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(cobranza_pago_chequeEntity cobranza_pago_cheque)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update cobranza_pago_cheque " +
          "set " +
          "cuenta_banco = @cuenta_banco, " +
          "fecha_pago = @fecha_pago, " +
          "num_cheque = @num_cheque, " +
          "usuario = @usuario" +
          " where id = @_id ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cuenta_banco", cobranza_pago_cheque.cuenta_banco);
              cmd.Parameters.AddWithValue("@fecha_pago", cobranza_pago_cheque.fecha_pago);
              cmd.Parameters.AddWithValue("@num_cheque", cobranza_pago_cheque.num_cheque);
              cmd.Parameters.AddWithValue("@usuario", cobranza_pago_cheque.usuario);
              cmd.Parameters.AddWithValue("@_id", cobranza_pago_cheque.id);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en cobranza_pago_chequeDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(cobranza_pago_chequeEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from cobranza_pago_cheque where id = @id"; 
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


  public static string Delete(cobranza_pago_chequeEntity cobranza_pago_cheque)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from cobranza_pago_cheque " + 
          " where id = @id ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@id", cobranza_pago_cheque.id);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en cobranza_pago_chequeDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref cobranza_pago_chequeEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from cobranza_pago_cheque where id = @id"; 
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
              return "Error en cobranza_pago_chequeDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
