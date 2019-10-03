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

  public static class contactosvendDAL
  {
      private static contactosvendEntity Load(IDataReader reader)
      {
          contactosvendEntity contactosvend = new contactosvendEntity();
          contactosvend.cargo = Convert.ToString(reader["cargo"]);
          contactosvend.correo = Convert.ToString(reader["correo"]);
          contactosvend.direccion = Convert.ToString(reader["direccion"]);
          contactosvend.nombre_contacto = Convert.ToString(reader["nombre_contacto"]);
          contactosvend.numero = Convert.ToString(reader["numero"]);
          contactosvend.rutcliente = Convert.ToString(reader["rutcliente"]);

          return contactosvend;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from contactosvend "+ sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(contactosvendEntity contactosvend)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into contactosvend(cargo, correo, direccion, nombre_contacto, numero, rutcliente) "+
          "values (@cargo, @correo, @direccion, @nombre_contacto, @numero, @rutcliente)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cargo", contactosvend.cargo);
              cmd.Parameters.AddWithValue("@correo", contactosvend.correo);
              cmd.Parameters.AddWithValue("@direccion", contactosvend.direccion);
              cmd.Parameters.AddWithValue("@nombre_contacto", contactosvend.nombre_contacto);
              cmd.Parameters.AddWithValue("@numero", contactosvend.numero);
              cmd.Parameters.AddWithValue("@rutcliente", contactosvend.rutcliente);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en contactosvendDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(contactosvendEntity contactosvend)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into contactosvend(cargo, correo, direccion, nombre_contacto, numero, rutcliente) "+
          "values (@cargo, @correo, @direccion, @nombre_contacto, @numero, @rutcliente)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cargo", contactosvend.cargo);
              cmd.Parameters.AddWithValue("@correo", contactosvend.correo);
              cmd.Parameters.AddWithValue("@direccion", contactosvend.direccion);
              cmd.Parameters.AddWithValue("@nombre_contacto", contactosvend.nombre_contacto);
              cmd.Parameters.AddWithValue("@numero", contactosvend.numero);
              cmd.Parameters.AddWithValue("@rutcliente", contactosvend.rutcliente);
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en contactosvendDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(contactosvendEntity contactosvend)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update contactosvend " +
          "set " +
          "cargo = @cargo, " +
          "correo = @correo, " +
          "direccion = @direccion, " +
          "nombre_contacto = @nombre_contacto, " +
          "numero = @numero, " +
          "rutcliente = @rutcliente" +
          " where nombre_contacto = @_nombre_contacto And rutcliente = @_rutcliente ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cargo", contactosvend.cargo);
              cmd.Parameters.AddWithValue("@correo", contactosvend.correo);
              cmd.Parameters.AddWithValue("@direccion", contactosvend.direccion);
              cmd.Parameters.AddWithValue("@nombre_contacto", contactosvend.nombre_contacto);
              cmd.Parameters.AddWithValue("@numero", contactosvend.numero);
              cmd.Parameters.AddWithValue("@rutcliente", contactosvend.rutcliente);
              cmd.Parameters.AddWithValue("@_nombre_contacto", contactosvend.nombre_contacto);
              cmd.Parameters.AddWithValue("@_rutcliente", contactosvend.rutcliente);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en contactosvendDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(contactosvendEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from contactosvend where nombre_contacto = @nombre_contacto And rutcliente = @rutcliente"; 
          SqlCommand cmd_encontrar = new SqlCommand(sql_encontrar, conn);
           cmd_encontrar.Parameters.AddWithValue("@nombre_contacto", u.nombre_contacto);
           cmd_encontrar.Parameters.AddWithValue("@rutcliente", u.rutcliente);
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


  public static string Delete(contactosvendEntity contactosvend)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from contactosvend " + 
          " where nombre_contacto = @nombre_contacto And rutcliente = @rutcliente ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@nombre_contacto", contactosvend.nombre_contacto);
              cmd.Parameters.AddWithValue("@rutcliente", contactosvend.rutcliente);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en contactosvendDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref contactosvendEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from contactosvend where nombre_contacto = @nombre_contacto And rutcliente = @rutcliente"; 
          SqlCommand cmd = new SqlCommand(sql, conn);
           cmd.Parameters.AddWithValue("@nombre_contacto", u.nombre_contacto);
           cmd.Parameters.AddWithValue("@rutcliente", u.rutcliente);
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
              return "Error en contactosvendDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
