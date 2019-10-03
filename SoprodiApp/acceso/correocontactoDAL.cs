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

  public static class correocontactoDAL
  {
      private static correocontactoEntity Load(IDataReader reader)
      {
          correocontactoEntity correocontacto = new correocontactoEntity();
          correocontacto.adjunto = Convert.ToString(reader["adjunto"]);
          correocontacto.asunto = Convert.ToString(reader["asunto"]);
          correocontacto.cc = Convert.ToString(reader["cc"]);
          correocontacto.codvendedor = Convert.ToString(reader["codvendedor"]);
          correocontacto.contenido = Convert.ToString(reader["contenido"]);
          correocontacto.destinatario = Convert.ToString(reader["destinatario"]);
          correocontacto.fecha = DateTime.Parse(reader["fecha"].ToString());
          correocontacto.id = int.Parse(reader["id"].ToString());
          correocontacto.nombre_contacto = Convert.ToString(reader["nombre_contacto"]);
          correocontacto.rutcliente = Convert.ToString(reader["rutcliente"]);

          return correocontacto;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from correocontacto "+ sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(correocontactoEntity correocontacto)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into correocontacto(adjunto, asunto, cc, codvendedor, contenido, destinatario, fecha, nombre_contacto, rutcliente) "+
          "values (@adjunto, @asunto, @cc, @codvendedor, @contenido, @destinatario, @fecha, @nombre_contacto, @rutcliente)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@adjunto", correocontacto.adjunto);
              cmd.Parameters.AddWithValue("@asunto", correocontacto.asunto);
              cmd.Parameters.AddWithValue("@cc", correocontacto.cc);
              cmd.Parameters.AddWithValue("@codvendedor", correocontacto.codvendedor);
              cmd.Parameters.AddWithValue("@contenido", correocontacto.contenido);
              cmd.Parameters.AddWithValue("@destinatario", correocontacto.destinatario);
              cmd.Parameters.AddWithValue("@fecha", correocontacto.fecha);
              cmd.Parameters.AddWithValue("@nombre_contacto", correocontacto.nombre_contacto);
              cmd.Parameters.AddWithValue("@rutcliente", correocontacto.rutcliente);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en correocontactoDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(correocontactoEntity correocontacto)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into correocontacto(adjunto, asunto, cc, codvendedor, contenido, destinatario, fecha, nombre_contacto, rutcliente) "+
          "values (@adjunto, @asunto, @cc, @codvendedor, @contenido, @destinatario, @fecha, @nombre_contacto, @rutcliente)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@adjunto", correocontacto.adjunto);
              cmd.Parameters.AddWithValue("@asunto", correocontacto.asunto);
              cmd.Parameters.AddWithValue("@cc", correocontacto.cc);
              cmd.Parameters.AddWithValue("@codvendedor", correocontacto.codvendedor);
              cmd.Parameters.AddWithValue("@contenido", correocontacto.contenido);
              cmd.Parameters.AddWithValue("@destinatario", correocontacto.destinatario);
              cmd.Parameters.AddWithValue("@fecha", correocontacto.fecha);
              cmd.Parameters.AddWithValue("@nombre_contacto", correocontacto.nombre_contacto);
              cmd.Parameters.AddWithValue("@rutcliente", correocontacto.rutcliente);
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en correocontactoDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(correocontactoEntity correocontacto)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update correocontacto " +
          "set " +
          "adjunto = @adjunto, " +
          "asunto = @asunto, " +
          "cc = @cc, " +
          "codvendedor = @codvendedor, " +
          "contenido = @contenido, " +
          "destinatario = @destinatario, " +
          "fecha = @fecha, " +
          "nombre_contacto = @nombre_contacto, " +
          "rutcliente = @rutcliente" +
          " where id = @_id ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@adjunto", correocontacto.adjunto);
              cmd.Parameters.AddWithValue("@asunto", correocontacto.asunto);
              cmd.Parameters.AddWithValue("@cc", correocontacto.cc);
              cmd.Parameters.AddWithValue("@codvendedor", correocontacto.codvendedor);
              cmd.Parameters.AddWithValue("@contenido", correocontacto.contenido);
              cmd.Parameters.AddWithValue("@destinatario", correocontacto.destinatario);
              cmd.Parameters.AddWithValue("@fecha", correocontacto.fecha);
              cmd.Parameters.AddWithValue("@nombre_contacto", correocontacto.nombre_contacto);
              cmd.Parameters.AddWithValue("@rutcliente", correocontacto.rutcliente);
              cmd.Parameters.AddWithValue("@_id", correocontacto.id);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en correocontactoDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(correocontactoEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from correocontacto where id = @id"; 
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


  public static string Delete(correocontactoEntity correocontacto)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from correocontacto " + 
          " where id = @id ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@id", correocontacto.id);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en correocontactoDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref correocontactoEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from correocontacto where id = @id"; 
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
              return "Error en correocontactoDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
