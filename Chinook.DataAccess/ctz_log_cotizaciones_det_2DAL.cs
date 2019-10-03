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

  public static class ctz_log_cotizaciones_det_2DAL
  {
      private static ctz_log_cotizaciones_det_2Entity Load(IDataReader reader)
      {
          ctz_log_cotizaciones_det_2Entity ctz_log_cotizaciones_det_2 = new ctz_log_cotizaciones_det_2Entity();
          ctz_log_cotizaciones_det_2.cod_bodega = Convert.ToString(reader["cod_bodega"]);
          ctz_log_cotizaciones_det_2.descuento = double.Parse(reader["descuento"].ToString());
          ctz_log_cotizaciones_det_2.id_cotizacion_det_log = int.Parse(reader["id_cotizacion_det_log"].ToString());
          ctz_log_cotizaciones_det_2.id_cotizacion_det_log_2 = int.Parse(reader["id_cotizacion_det_log_2"].ToString());
          ctz_log_cotizaciones_det_2.id_cotizacion_log = int.Parse(reader["id_cotizacion_log"].ToString());
          ctz_log_cotizaciones_det_2.nombre_bodega = Convert.ToString(reader["nombre_bodega"]);
          ctz_log_cotizaciones_det_2.precio = double.Parse(reader["precio"].ToString());
          ctz_log_cotizaciones_det_2.precio_con_descuento = double.Parse(reader["precio_con_descuento"].ToString());
          ctz_log_cotizaciones_det_2.precio_con_descuento_iva = double.Parse(reader["precio_con_descuento_iva"].ToString());
          ctz_log_cotizaciones_det_2.precio_con_descuento_unitario = double.Parse(reader["precio_con_descuento_unitario"].ToString());
          ctz_log_cotizaciones_det_2.precio_con_descuento_unitario_iva = double.Parse(reader["precio_con_descuento_unitario_iva"].ToString());
          ctz_log_cotizaciones_det_2.precio_iva = double.Parse(reader["precio_iva"].ToString());
          ctz_log_cotizaciones_det_2.precio_unitario = double.Parse(reader["precio_unitario"].ToString());
          ctz_log_cotizaciones_det_2.precio_unitario_iva = double.Parse(reader["precio_unitario_iva"].ToString());
          ctz_log_cotizaciones_det_2.total = double.Parse(reader["total"].ToString());
          ctz_log_cotizaciones_det_2.total_iva = double.Parse(reader["total_iva"].ToString());

          return ctz_log_cotizaciones_det_2;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from ctz_log_cotizaciones_det_2 " + sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(ctz_log_cotizaciones_det_2Entity ctz_log_cotizaciones_det_2)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_log_cotizaciones_det_2(cod_bodega, descuento, id_cotizacion_det_log, id_cotizacion_log, nombre_bodega, precio, precio_con_descuento, precio_con_descuento_iva, precio_con_descuento_unitario, precio_con_descuento_unitario_iva, precio_iva, precio_unitario, precio_unitario_iva, total, total_iva) "+
          "values (@cod_bodega, @descuento, @id_cotizacion_det_log, @id_cotizacion_log, @nombre_bodega, @precio, @precio_con_descuento, @precio_con_descuento_iva, @precio_con_descuento_unitario, @precio_con_descuento_unitario_iva, @precio_iva, @precio_unitario, @precio_unitario_iva, @total, @total_iva)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_bodega", ctz_log_cotizaciones_det_2.cod_bodega);
              cmd.Parameters.AddWithValue("@descuento", ctz_log_cotizaciones_det_2.descuento);
              cmd.Parameters.AddWithValue("@id_cotizacion_det_log", ctz_log_cotizaciones_det_2.id_cotizacion_det_log);
              cmd.Parameters.AddWithValue("@id_cotizacion_log", ctz_log_cotizaciones_det_2.id_cotizacion_log);
              cmd.Parameters.AddWithValue("@nombre_bodega", ctz_log_cotizaciones_det_2.nombre_bodega);
              cmd.Parameters.AddWithValue("@precio", ctz_log_cotizaciones_det_2.precio);
              cmd.Parameters.AddWithValue("@precio_con_descuento", ctz_log_cotizaciones_det_2.precio_con_descuento);
              cmd.Parameters.AddWithValue("@precio_con_descuento_iva", ctz_log_cotizaciones_det_2.precio_con_descuento_iva);
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario", ctz_log_cotizaciones_det_2.precio_con_descuento_unitario);
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario_iva", ctz_log_cotizaciones_det_2.precio_con_descuento_unitario_iva);
              cmd.Parameters.AddWithValue("@precio_iva", ctz_log_cotizaciones_det_2.precio_iva);
              cmd.Parameters.AddWithValue("@precio_unitario", ctz_log_cotizaciones_det_2.precio_unitario);
              cmd.Parameters.AddWithValue("@precio_unitario_iva", ctz_log_cotizaciones_det_2.precio_unitario_iva);
              cmd.Parameters.AddWithValue("@total", ctz_log_cotizaciones_det_2.total);
              cmd.Parameters.AddWithValue("@total_iva", ctz_log_cotizaciones_det_2.total_iva);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en ctz_log_cotizaciones_det_2DAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(ctz_log_cotizaciones_det_2Entity ctz_log_cotizaciones_det_2)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_log_cotizaciones_det_2(cod_bodega, descuento, id_cotizacion_det_log, id_cotizacion_log, nombre_bodega, precio, precio_con_descuento, precio_con_descuento_iva, precio_con_descuento_unitario, precio_con_descuento_unitario_iva, precio_iva, precio_unitario, precio_unitario_iva, total, total_iva) "+
          "values (@cod_bodega, @descuento, @id_cotizacion_det_log, @id_cotizacion_log, @nombre_bodega, @precio, @precio_con_descuento, @precio_con_descuento_iva, @precio_con_descuento_unitario, @precio_con_descuento_unitario_iva, @precio_iva, @precio_unitario, @precio_unitario_iva, @total, @total_iva);  SELECT SCOPE_IDENTITY(); ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_bodega", ctz_log_cotizaciones_det_2.cod_bodega); 
              cmd.Parameters.AddWithValue("@descuento", ctz_log_cotizaciones_det_2.descuento); 
              cmd.Parameters.AddWithValue("@id_cotizacion_det_log", ctz_log_cotizaciones_det_2.id_cotizacion_det_log); 
              cmd.Parameters.AddWithValue("@id_cotizacion_log", ctz_log_cotizaciones_det_2.id_cotizacion_log); 
              cmd.Parameters.AddWithValue("@nombre_bodega", ctz_log_cotizaciones_det_2.nombre_bodega); 
              cmd.Parameters.AddWithValue("@precio", ctz_log_cotizaciones_det_2.precio); 
              cmd.Parameters.AddWithValue("@precio_con_descuento", ctz_log_cotizaciones_det_2.precio_con_descuento); 
              cmd.Parameters.AddWithValue("@precio_con_descuento_iva", ctz_log_cotizaciones_det_2.precio_con_descuento_iva); 
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario", ctz_log_cotizaciones_det_2.precio_con_descuento_unitario); 
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario_iva", ctz_log_cotizaciones_det_2.precio_con_descuento_unitario_iva); 
              cmd.Parameters.AddWithValue("@precio_iva", ctz_log_cotizaciones_det_2.precio_iva); 
              cmd.Parameters.AddWithValue("@precio_unitario", ctz_log_cotizaciones_det_2.precio_unitario); 
              cmd.Parameters.AddWithValue("@precio_unitario_iva", ctz_log_cotizaciones_det_2.precio_unitario_iva); 
              cmd.Parameters.AddWithValue("@total", ctz_log_cotizaciones_det_2.total); 
              cmd.Parameters.AddWithValue("@total_iva", ctz_log_cotizaciones_det_2.total_iva); 
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en ctz_log_cotizaciones_det_2DAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(ctz_log_cotizaciones_det_2Entity ctz_log_cotizaciones_det_2)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update ctz_log_cotizaciones_det_2 " +
          "set " +
          "cod_bodega = @cod_bodega, " +
          "descuento = @descuento, " +
          "id_cotizacion_det_log = @id_cotizacion_det_log, " +
          "id_cotizacion_log = @id_cotizacion_log, " +
          "nombre_bodega = @nombre_bodega, " +
          "precio = @precio, " +
          "precio_con_descuento = @precio_con_descuento, " +
          "precio_con_descuento_iva = @precio_con_descuento_iva, " +
          "precio_con_descuento_unitario = @precio_con_descuento_unitario, " +
          "precio_con_descuento_unitario_iva = @precio_con_descuento_unitario_iva, " +
          "precio_iva = @precio_iva, " +
          "precio_unitario = @precio_unitario, " +
          "precio_unitario_iva = @precio_unitario_iva, " +
          "total = @total, " +
          "total_iva = @total_iva" +
          " where id_cotizacion_det_log_2 = @_id_cotizacion_det_log_2 ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cod_bodega", ctz_log_cotizaciones_det_2.cod_bodega);
              cmd.Parameters.AddWithValue("@descuento", ctz_log_cotizaciones_det_2.descuento);
              cmd.Parameters.AddWithValue("@id_cotizacion_det_log", ctz_log_cotizaciones_det_2.id_cotizacion_det_log);
              cmd.Parameters.AddWithValue("@id_cotizacion_log", ctz_log_cotizaciones_det_2.id_cotizacion_log);
              cmd.Parameters.AddWithValue("@nombre_bodega", ctz_log_cotizaciones_det_2.nombre_bodega);
              cmd.Parameters.AddWithValue("@precio", ctz_log_cotizaciones_det_2.precio);
              cmd.Parameters.AddWithValue("@precio_con_descuento", ctz_log_cotizaciones_det_2.precio_con_descuento);
              cmd.Parameters.AddWithValue("@precio_con_descuento_iva", ctz_log_cotizaciones_det_2.precio_con_descuento_iva);
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario", ctz_log_cotizaciones_det_2.precio_con_descuento_unitario);
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario_iva", ctz_log_cotizaciones_det_2.precio_con_descuento_unitario_iva);
              cmd.Parameters.AddWithValue("@precio_iva", ctz_log_cotizaciones_det_2.precio_iva);
              cmd.Parameters.AddWithValue("@precio_unitario", ctz_log_cotizaciones_det_2.precio_unitario);
              cmd.Parameters.AddWithValue("@precio_unitario_iva", ctz_log_cotizaciones_det_2.precio_unitario_iva);
              cmd.Parameters.AddWithValue("@total", ctz_log_cotizaciones_det_2.total);
              cmd.Parameters.AddWithValue("@total_iva", ctz_log_cotizaciones_det_2.total_iva);
              cmd.Parameters.AddWithValue("@_id_cotizacion_det_log_2", ctz_log_cotizaciones_det_2.id_cotizacion_det_log_2);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_log_cotizaciones_det_2DAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(ctz_log_cotizaciones_det_2Entity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from ctz_log_cotizaciones_det_2 where id_cotizacion_det_log_2 = @id_cotizacion_det_log_2"; 
          SqlCommand cmd_encontrar = new SqlCommand(sql_encontrar, conn);
           cmd_encontrar.Parameters.AddWithValue("@id_cotizacion_det_log_2", u.id_cotizacion_det_log_2);
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


  public static string Delete(ctz_log_cotizaciones_det_2Entity ctz_log_cotizaciones_det_2)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from ctz_log_cotizaciones_det_2 " + 
          " where id_cotizacion_det_log_2 = @id_cotizacion_det_log_2 ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@id_cotizacion_det_log_2", ctz_log_cotizaciones_det_2.id_cotizacion_det_log_2);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_log_cotizaciones_det_2DAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref ctz_log_cotizaciones_det_2Entity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from ctz_log_cotizaciones_det_2 where id_cotizacion_det_log_2 = @id_cotizacion_det_log_2"; 
          SqlCommand cmd = new SqlCommand(sql, conn);
           cmd.Parameters.AddWithValue("@id_cotizacion_det_log_2", u.id_cotizacion_det_log_2);
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
              return "Error en ctz_log_cotizaciones_det_2DAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
