using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CRM.Entities;
using System.Data;


namespace CRM.DataAccess
{

  public static class ctz_cotizacion_det_2DAL
  {
      private static ctz_cotizacion_det_2Entity Load(IDataReader reader)
      {
          ctz_cotizacion_det_2Entity ctz_cotizacion_det_2 = new ctz_cotizacion_det_2Entity();
          ctz_cotizacion_det_2.descuento = double.Parse(reader["descuento"].ToString());
          ctz_cotizacion_det_2.id_cotizacion = int.Parse(reader["id_cotizacion"].ToString());
          ctz_cotizacion_det_2.id_cotizacion_det = int.Parse(reader["id_cotizacion_det"].ToString());
          ctz_cotizacion_det_2.id_cotizacion_det_2 = int.Parse(reader["id_cotizacion_det_2"].ToString());
          ctz_cotizacion_det_2.precio = double.Parse(reader["precio"].ToString());
          ctz_cotizacion_det_2.precio_con_descuento = double.Parse(reader["precio_con_descuento"].ToString());
          ctz_cotizacion_det_2.precio_con_descuento_unitario = double.Parse(reader["precio_con_descuento_unitario"].ToString());
          ctz_cotizacion_det_2.precio_unitario = double.Parse(reader["precio_unitario"].ToString());
          ctz_cotizacion_det_2.total = double.Parse(reader["total"].ToString());
          ctz_cotizacion_det_2.total_iva = double.Parse(reader["total_iva"].ToString());

          return ctz_cotizacion_det_2;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from ctz_cotizacion_det_2 " + sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(ctz_cotizacion_det_2Entity ctz_cotizacion_det_2)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_cotizacion_det_2(descuento, id_cotizacion, id_cotizacion_det, precio, precio_con_descuento, precio_con_descuento_unitario, precio_unitario, total, total_iva) "+
          "values (@descuento, @id_cotizacion, @id_cotizacion_det, @precio, @precio_con_descuento, @precio_con_descuento_unitario, @precio_unitario, @total, @total_iva)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@descuento", ctz_cotizacion_det_2.descuento);
              cmd.Parameters.AddWithValue("@id_cotizacion", ctz_cotizacion_det_2.id_cotizacion);
              cmd.Parameters.AddWithValue("@id_cotizacion_det", ctz_cotizacion_det_2.id_cotizacion_det);
              cmd.Parameters.AddWithValue("@precio", ctz_cotizacion_det_2.precio);
              cmd.Parameters.AddWithValue("@precio_con_descuento", ctz_cotizacion_det_2.precio_con_descuento);
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario", ctz_cotizacion_det_2.precio_con_descuento_unitario);
              cmd.Parameters.AddWithValue("@precio_unitario", ctz_cotizacion_det_2.precio_unitario);
              cmd.Parameters.AddWithValue("@total", ctz_cotizacion_det_2.total);
              cmd.Parameters.AddWithValue("@total_iva", ctz_cotizacion_det_2.total_iva);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en ctz_cotizacion_det_2DAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(ctz_cotizacion_det_2Entity ctz_cotizacion_det_2)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_cotizacion_det_2(descuento, id_cotizacion, id_cotizacion_det, precio, precio_con_descuento, precio_con_descuento_unitario, precio_unitario, total, total_iva) "+
          "values (@descuento, @id_cotizacion, @id_cotizacion_det, @precio, @precio_con_descuento, @precio_con_descuento_unitario, @precio_unitario, @total, @total_iva);  SELECT SCOPE_IDENTITY(); ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@descuento", ctz_cotizacion_det_2.descuento); 
              cmd.Parameters.AddWithValue("@id_cotizacion", ctz_cotizacion_det_2.id_cotizacion); 
              cmd.Parameters.AddWithValue("@id_cotizacion_det", ctz_cotizacion_det_2.id_cotizacion_det); 
              cmd.Parameters.AddWithValue("@precio", ctz_cotizacion_det_2.precio); 
              cmd.Parameters.AddWithValue("@precio_con_descuento", ctz_cotizacion_det_2.precio_con_descuento); 
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario", ctz_cotizacion_det_2.precio_con_descuento_unitario); 
              cmd.Parameters.AddWithValue("@precio_unitario", ctz_cotizacion_det_2.precio_unitario); 
              cmd.Parameters.AddWithValue("@total", ctz_cotizacion_det_2.total); 
              cmd.Parameters.AddWithValue("@total_iva", ctz_cotizacion_det_2.total_iva); 
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en ctz_cotizacion_det_2DAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(ctz_cotizacion_det_2Entity ctz_cotizacion_det_2)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update ctz_cotizacion_det_2 " +
          "set " +
          "descuento = @descuento, " +
          "id_cotizacion = @id_cotizacion, " +
          "id_cotizacion_det = @id_cotizacion_det, " +
          "precio = @precio, " +
          "precio_con_descuento = @precio_con_descuento, " +
          "precio_con_descuento_unitario = @precio_con_descuento_unitario, " +
          "precio_unitario = @precio_unitario, " +
          "total = @total, " +
          "total_iva = @total_iva" +
          " where id_cotizacion_det_2 = @_id_cotizacion_det_2 ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@descuento", ctz_cotizacion_det_2.descuento);
              cmd.Parameters.AddWithValue("@id_cotizacion", ctz_cotizacion_det_2.id_cotizacion);
              cmd.Parameters.AddWithValue("@id_cotizacion_det", ctz_cotizacion_det_2.id_cotizacion_det);
              cmd.Parameters.AddWithValue("@precio", ctz_cotizacion_det_2.precio);
              cmd.Parameters.AddWithValue("@precio_con_descuento", ctz_cotizacion_det_2.precio_con_descuento);
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario", ctz_cotizacion_det_2.precio_con_descuento_unitario);
              cmd.Parameters.AddWithValue("@precio_unitario", ctz_cotizacion_det_2.precio_unitario);
              cmd.Parameters.AddWithValue("@total", ctz_cotizacion_det_2.total);
              cmd.Parameters.AddWithValue("@total_iva", ctz_cotizacion_det_2.total_iva);
              cmd.Parameters.AddWithValue("@_id_cotizacion_det_2", ctz_cotizacion_det_2.id_cotizacion_det_2);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_cotizacion_det_2DAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(ctz_cotizacion_det_2Entity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from ctz_cotizacion_det_2 where id_cotizacion_det_2 = @id_cotizacion_det_2"; 
          SqlCommand cmd_encontrar = new SqlCommand(sql_encontrar, conn);
           cmd_encontrar.Parameters.AddWithValue("@id_cotizacion_det_2", u.id_cotizacion_det_2);
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


  public static string Delete(ctz_cotizacion_det_2Entity ctz_cotizacion_det_2)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from ctz_cotizacion_det_2 " + 
          " where id_cotizacion_det_2 = @id_cotizacion_det_2 ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@id_cotizacion_det_2", ctz_cotizacion_det_2.id_cotizacion_det_2);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_cotizacion_det_2DAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref ctz_cotizacion_det_2Entity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from ctz_cotizacion_det_2 where id_cotizacion_det_2 = @id_cotizacion_det_2"; 
          SqlCommand cmd = new SqlCommand(sql, conn);
           cmd.Parameters.AddWithValue("@id_cotizacion_det_2", u.id_cotizacion_det_2);
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
              return "Error en ctz_cotizacion_det_2DAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
