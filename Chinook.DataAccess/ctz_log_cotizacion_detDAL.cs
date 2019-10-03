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

  public static class ctz_log_cotizacion_detDAL
  {
      private static ctz_log_cotizacion_detEntity Load(IDataReader reader)
      {
          ctz_log_cotizacion_detEntity ctz_log_cotizacion_det = new ctz_log_cotizacion_detEntity();
          ctz_log_cotizacion_det.cantidad_1 = int.Parse(reader["cantidad_1"].ToString());
          ctz_log_cotizacion_det.cantidad_2 = int.Parse(reader["cantidad_2"].ToString());
          ctz_log_cotizacion_det.cantidad_3 = int.Parse(reader["cantidad_3"].ToString());
          ctz_log_cotizacion_det.cod_bodega_1 = Convert.ToString(reader["cod_bodega_1"]);
          ctz_log_cotizacion_det.cod_bodega_2 = Convert.ToString(reader["cod_bodega_2"]);
          ctz_log_cotizacion_det.cod_bodega_3 = Convert.ToString(reader["cod_bodega_3"]);
          ctz_log_cotizacion_det.descuento_1 = double.Parse(reader["descuento_1"].ToString());
          ctz_log_cotizacion_det.descuento_2 = double.Parse(reader["descuento_2"].ToString());
          ctz_log_cotizacion_det.descuento_3 = double.Parse(reader["descuento_3"].ToString());
          ctz_log_cotizacion_det.id_cotizacion_det_log = int.Parse(reader["id_cotizacion_det_log"].ToString());
          ctz_log_cotizacion_det.id_cotizacion_log = int.Parse(reader["id_cotizacion_log"].ToString());
          ctz_log_cotizacion_det.nom_producto = Convert.ToString(reader["nom_producto"]);
          ctz_log_cotizacion_det.precio_1 = double.Parse(reader["precio_1"].ToString());
          ctz_log_cotizacion_det.precio_2 = double.Parse(reader["precio_2"].ToString());
          ctz_log_cotizacion_det.precio_3 = double.Parse(reader["precio_3"].ToString());
          ctz_log_cotizacion_det.precio_con_descuento_1 = double.Parse(reader["precio_con_descuento_1"].ToString());
          ctz_log_cotizacion_det.precio_con_descuento_2 = double.Parse(reader["precio_con_descuento_2"].ToString());
          ctz_log_cotizacion_det.precio_con_descuento_3 = double.Parse(reader["precio_con_descuento_3"].ToString());
          ctz_log_cotizacion_det.precio_con_descuento_unitario_1 = double.Parse(reader["precio_con_descuento_unitario_1"].ToString());
          ctz_log_cotizacion_det.precio_con_descuento_unitario_2 = double.Parse(reader["precio_con_descuento_unitario_2"].ToString());
          ctz_log_cotizacion_det.precio_con_descuento_unitario_3 = double.Parse(reader["precio_con_descuento_unitario_3"].ToString());
          ctz_log_cotizacion_det.precio_unitario_1 = double.Parse(reader["precio_unitario_1"].ToString());
          ctz_log_cotizacion_det.precio_unitario_2 = double.Parse(reader["precio_unitario_2"].ToString());
          ctz_log_cotizacion_det.precio_unitario_3 = double.Parse(reader["precio_unitario_3"].ToString());
          ctz_log_cotizacion_det.producto = Convert.ToString(reader["producto"]);
          ctz_log_cotizacion_det.total_1 = double.Parse(reader["total_1"].ToString());
          ctz_log_cotizacion_det.total_2 = double.Parse(reader["total_2"].ToString());
          ctz_log_cotizacion_det.total_3 = double.Parse(reader["total_3"].ToString());
          ctz_log_cotizacion_det.total_iva_1 = double.Parse(reader["total_iva_1"].ToString());
          ctz_log_cotizacion_det.total_iva_2 = double.Parse(reader["total_iva_2"].ToString());
          ctz_log_cotizacion_det.total_iva_3 = double.Parse(reader["total_iva_3"].ToString());
          ctz_log_cotizacion_det.unidad_equivale = int.Parse(reader["unidad_equivale"].ToString());

          return ctz_log_cotizacion_det;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from ctz_log_cotizacion_det " + sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(ctz_log_cotizacion_detEntity ctz_log_cotizacion_det)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_log_cotizacion_det(cantidad_1, cantidad_2, cantidad_3, cod_bodega_1, cod_bodega_2, cod_bodega_3, descuento_1, descuento_2, descuento_3, id_cotizacion_log, nom_producto, precio_1, precio_2, precio_3, precio_con_descuento_1, precio_con_descuento_2, precio_con_descuento_3, precio_con_descuento_unitario_1, precio_con_descuento_unitario_2, precio_con_descuento_unitario_3, precio_unitario_1, precio_unitario_2, precio_unitario_3, producto, total_1, total_2, total_3, total_iva_1, total_iva_2, total_iva_3, unidad_equivale) "+
          "values (@cantidad_1, @cantidad_2, @cantidad_3, @cod_bodega_1, @cod_bodega_2, @cod_bodega_3, @descuento_1, @descuento_2, @descuento_3, @id_cotizacion_log, @nom_producto, @precio_1, @precio_2, @precio_3, @precio_con_descuento_1, @precio_con_descuento_2, @precio_con_descuento_3, @precio_con_descuento_unitario_1, @precio_con_descuento_unitario_2, @precio_con_descuento_unitario_3, @precio_unitario_1, @precio_unitario_2, @precio_unitario_3, @producto, @total_1, @total_2, @total_3, @total_iva_1, @total_iva_2, @total_iva_3, @unidad_equivale)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cantidad_1", ctz_log_cotizacion_det.cantidad_1);
              cmd.Parameters.AddWithValue("@cantidad_2", ctz_log_cotizacion_det.cantidad_2);
              cmd.Parameters.AddWithValue("@cantidad_3", ctz_log_cotizacion_det.cantidad_3);
              cmd.Parameters.AddWithValue("@cod_bodega_1", ctz_log_cotizacion_det.cod_bodega_1);
              cmd.Parameters.AddWithValue("@cod_bodega_2", ctz_log_cotizacion_det.cod_bodega_2);
              cmd.Parameters.AddWithValue("@cod_bodega_3", ctz_log_cotizacion_det.cod_bodega_3);
              cmd.Parameters.AddWithValue("@descuento_1", ctz_log_cotizacion_det.descuento_1);
              cmd.Parameters.AddWithValue("@descuento_2", ctz_log_cotizacion_det.descuento_2);
              cmd.Parameters.AddWithValue("@descuento_3", ctz_log_cotizacion_det.descuento_3);
              cmd.Parameters.AddWithValue("@id_cotizacion_log", ctz_log_cotizacion_det.id_cotizacion_log);
              cmd.Parameters.AddWithValue("@nom_producto", ctz_log_cotizacion_det.nom_producto);
              cmd.Parameters.AddWithValue("@precio_1", ctz_log_cotizacion_det.precio_1);
              cmd.Parameters.AddWithValue("@precio_2", ctz_log_cotizacion_det.precio_2);
              cmd.Parameters.AddWithValue("@precio_3", ctz_log_cotizacion_det.precio_3);
              cmd.Parameters.AddWithValue("@precio_con_descuento_1", ctz_log_cotizacion_det.precio_con_descuento_1);
              cmd.Parameters.AddWithValue("@precio_con_descuento_2", ctz_log_cotizacion_det.precio_con_descuento_2);
              cmd.Parameters.AddWithValue("@precio_con_descuento_3", ctz_log_cotizacion_det.precio_con_descuento_3);
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario_1", ctz_log_cotizacion_det.precio_con_descuento_unitario_1);
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario_2", ctz_log_cotizacion_det.precio_con_descuento_unitario_2);
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario_3", ctz_log_cotizacion_det.precio_con_descuento_unitario_3);
              cmd.Parameters.AddWithValue("@precio_unitario_1", ctz_log_cotizacion_det.precio_unitario_1);
              cmd.Parameters.AddWithValue("@precio_unitario_2", ctz_log_cotizacion_det.precio_unitario_2);
              cmd.Parameters.AddWithValue("@precio_unitario_3", ctz_log_cotizacion_det.precio_unitario_3);
              cmd.Parameters.AddWithValue("@producto", ctz_log_cotizacion_det.producto);
              cmd.Parameters.AddWithValue("@total_1", ctz_log_cotizacion_det.total_1);
              cmd.Parameters.AddWithValue("@total_2", ctz_log_cotizacion_det.total_2);
              cmd.Parameters.AddWithValue("@total_3", ctz_log_cotizacion_det.total_3);
              cmd.Parameters.AddWithValue("@total_iva_1", ctz_log_cotizacion_det.total_iva_1);
              cmd.Parameters.AddWithValue("@total_iva_2", ctz_log_cotizacion_det.total_iva_2);
              cmd.Parameters.AddWithValue("@total_iva_3", ctz_log_cotizacion_det.total_iva_3);
              cmd.Parameters.AddWithValue("@unidad_equivale", ctz_log_cotizacion_det.unidad_equivale);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en ctz_log_cotizacion_detDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(ctz_log_cotizacion_detEntity ctz_log_cotizacion_det)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_log_cotizacion_det(cantidad_1, cantidad_2, cantidad_3, cod_bodega_1, cod_bodega_2, cod_bodega_3, descuento_1, descuento_2, descuento_3, id_cotizacion_log, nom_producto, precio_1, precio_2, precio_3, precio_con_descuento_1, precio_con_descuento_2, precio_con_descuento_3, precio_con_descuento_unitario_1, precio_con_descuento_unitario_2, precio_con_descuento_unitario_3, precio_unitario_1, precio_unitario_2, precio_unitario_3, producto, total_1, total_2, total_3, total_iva_1, total_iva_2, total_iva_3, unidad_equivale) "+
          "values (@cantidad_1, @cantidad_2, @cantidad_3, @cod_bodega_1, @cod_bodega_2, @cod_bodega_3, @descuento_1, @descuento_2, @descuento_3, @id_cotizacion_log, @nom_producto, @precio_1, @precio_2, @precio_3, @precio_con_descuento_1, @precio_con_descuento_2, @precio_con_descuento_3, @precio_con_descuento_unitario_1, @precio_con_descuento_unitario_2, @precio_con_descuento_unitario_3, @precio_unitario_1, @precio_unitario_2, @precio_unitario_3, @producto, @total_1, @total_2, @total_3, @total_iva_1, @total_iva_2, @total_iva_3, @unidad_equivale);  SELECT SCOPE_IDENTITY(); ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cantidad_1", ctz_log_cotizacion_det.cantidad_1); 
              cmd.Parameters.AddWithValue("@cantidad_2", ctz_log_cotizacion_det.cantidad_2); 
              cmd.Parameters.AddWithValue("@cantidad_3", ctz_log_cotizacion_det.cantidad_3); 
              cmd.Parameters.AddWithValue("@cod_bodega_1", ctz_log_cotizacion_det.cod_bodega_1); 
              cmd.Parameters.AddWithValue("@cod_bodega_2", ctz_log_cotizacion_det.cod_bodega_2); 
              cmd.Parameters.AddWithValue("@cod_bodega_3", ctz_log_cotizacion_det.cod_bodega_3); 
              cmd.Parameters.AddWithValue("@descuento_1", ctz_log_cotizacion_det.descuento_1); 
              cmd.Parameters.AddWithValue("@descuento_2", ctz_log_cotizacion_det.descuento_2); 
              cmd.Parameters.AddWithValue("@descuento_3", ctz_log_cotizacion_det.descuento_3); 
              cmd.Parameters.AddWithValue("@id_cotizacion_log", ctz_log_cotizacion_det.id_cotizacion_log); 
              cmd.Parameters.AddWithValue("@nom_producto", ctz_log_cotizacion_det.nom_producto); 
              cmd.Parameters.AddWithValue("@precio_1", ctz_log_cotizacion_det.precio_1); 
              cmd.Parameters.AddWithValue("@precio_2", ctz_log_cotizacion_det.precio_2); 
              cmd.Parameters.AddWithValue("@precio_3", ctz_log_cotizacion_det.precio_3); 
              cmd.Parameters.AddWithValue("@precio_con_descuento_1", ctz_log_cotizacion_det.precio_con_descuento_1); 
              cmd.Parameters.AddWithValue("@precio_con_descuento_2", ctz_log_cotizacion_det.precio_con_descuento_2); 
              cmd.Parameters.AddWithValue("@precio_con_descuento_3", ctz_log_cotizacion_det.precio_con_descuento_3); 
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario_1", ctz_log_cotizacion_det.precio_con_descuento_unitario_1); 
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario_2", ctz_log_cotizacion_det.precio_con_descuento_unitario_2); 
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario_3", ctz_log_cotizacion_det.precio_con_descuento_unitario_3); 
              cmd.Parameters.AddWithValue("@precio_unitario_1", ctz_log_cotizacion_det.precio_unitario_1); 
              cmd.Parameters.AddWithValue("@precio_unitario_2", ctz_log_cotizacion_det.precio_unitario_2); 
              cmd.Parameters.AddWithValue("@precio_unitario_3", ctz_log_cotizacion_det.precio_unitario_3); 
              cmd.Parameters.AddWithValue("@producto", ctz_log_cotizacion_det.producto); 
              cmd.Parameters.AddWithValue("@total_1", ctz_log_cotizacion_det.total_1); 
              cmd.Parameters.AddWithValue("@total_2", ctz_log_cotizacion_det.total_2); 
              cmd.Parameters.AddWithValue("@total_3", ctz_log_cotizacion_det.total_3); 
              cmd.Parameters.AddWithValue("@total_iva_1", ctz_log_cotizacion_det.total_iva_1); 
              cmd.Parameters.AddWithValue("@total_iva_2", ctz_log_cotizacion_det.total_iva_2); 
              cmd.Parameters.AddWithValue("@total_iva_3", ctz_log_cotizacion_det.total_iva_3); 
              cmd.Parameters.AddWithValue("@unidad_equivale", ctz_log_cotizacion_det.unidad_equivale); 
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en ctz_log_cotizacion_detDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(ctz_log_cotizacion_detEntity ctz_log_cotizacion_det)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update ctz_log_cotizacion_det " +
          "set " +
          "cantidad_1 = @cantidad_1, " +
          "cantidad_2 = @cantidad_2, " +
          "cantidad_3 = @cantidad_3, " +
          "cod_bodega_1 = @cod_bodega_1, " +
          "cod_bodega_2 = @cod_bodega_2, " +
          "cod_bodega_3 = @cod_bodega_3, " +
          "descuento_1 = @descuento_1, " +
          "descuento_2 = @descuento_2, " +
          "descuento_3 = @descuento_3, " +
          "id_cotizacion_log = @id_cotizacion_log, " +
          "nom_producto = @nom_producto, " +
          "precio_1 = @precio_1, " +
          "precio_2 = @precio_2, " +
          "precio_3 = @precio_3, " +
          "precio_con_descuento_1 = @precio_con_descuento_1, " +
          "precio_con_descuento_2 = @precio_con_descuento_2, " +
          "precio_con_descuento_3 = @precio_con_descuento_3, " +
          "precio_con_descuento_unitario_1 = @precio_con_descuento_unitario_1, " +
          "precio_con_descuento_unitario_2 = @precio_con_descuento_unitario_2, " +
          "precio_con_descuento_unitario_3 = @precio_con_descuento_unitario_3, " +
          "precio_unitario_1 = @precio_unitario_1, " +
          "precio_unitario_2 = @precio_unitario_2, " +
          "precio_unitario_3 = @precio_unitario_3, " +
          "producto = @producto, " +
          "total_1 = @total_1, " +
          "total_2 = @total_2, " +
          "total_3 = @total_3, " +
          "total_iva_1 = @total_iva_1, " +
          "total_iva_2 = @total_iva_2, " +
          "total_iva_3 = @total_iva_3, " +
          "unidad_equivale = @unidad_equivale" +
          " where id_cotizacion_det_log = @_id_cotizacion_det_log ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cantidad_1", ctz_log_cotizacion_det.cantidad_1);
              cmd.Parameters.AddWithValue("@cantidad_2", ctz_log_cotizacion_det.cantidad_2);
              cmd.Parameters.AddWithValue("@cantidad_3", ctz_log_cotizacion_det.cantidad_3);
              cmd.Parameters.AddWithValue("@cod_bodega_1", ctz_log_cotizacion_det.cod_bodega_1);
              cmd.Parameters.AddWithValue("@cod_bodega_2", ctz_log_cotizacion_det.cod_bodega_2);
              cmd.Parameters.AddWithValue("@cod_bodega_3", ctz_log_cotizacion_det.cod_bodega_3);
              cmd.Parameters.AddWithValue("@descuento_1", ctz_log_cotizacion_det.descuento_1);
              cmd.Parameters.AddWithValue("@descuento_2", ctz_log_cotizacion_det.descuento_2);
              cmd.Parameters.AddWithValue("@descuento_3", ctz_log_cotizacion_det.descuento_3);
              cmd.Parameters.AddWithValue("@id_cotizacion_log", ctz_log_cotizacion_det.id_cotizacion_log);
              cmd.Parameters.AddWithValue("@nom_producto", ctz_log_cotizacion_det.nom_producto);
              cmd.Parameters.AddWithValue("@precio_1", ctz_log_cotizacion_det.precio_1);
              cmd.Parameters.AddWithValue("@precio_2", ctz_log_cotizacion_det.precio_2);
              cmd.Parameters.AddWithValue("@precio_3", ctz_log_cotizacion_det.precio_3);
              cmd.Parameters.AddWithValue("@precio_con_descuento_1", ctz_log_cotizacion_det.precio_con_descuento_1);
              cmd.Parameters.AddWithValue("@precio_con_descuento_2", ctz_log_cotizacion_det.precio_con_descuento_2);
              cmd.Parameters.AddWithValue("@precio_con_descuento_3", ctz_log_cotizacion_det.precio_con_descuento_3);
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario_1", ctz_log_cotizacion_det.precio_con_descuento_unitario_1);
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario_2", ctz_log_cotizacion_det.precio_con_descuento_unitario_2);
              cmd.Parameters.AddWithValue("@precio_con_descuento_unitario_3", ctz_log_cotizacion_det.precio_con_descuento_unitario_3);
              cmd.Parameters.AddWithValue("@precio_unitario_1", ctz_log_cotizacion_det.precio_unitario_1);
              cmd.Parameters.AddWithValue("@precio_unitario_2", ctz_log_cotizacion_det.precio_unitario_2);
              cmd.Parameters.AddWithValue("@precio_unitario_3", ctz_log_cotizacion_det.precio_unitario_3);
              cmd.Parameters.AddWithValue("@producto", ctz_log_cotizacion_det.producto);
              cmd.Parameters.AddWithValue("@total_1", ctz_log_cotizacion_det.total_1);
              cmd.Parameters.AddWithValue("@total_2", ctz_log_cotizacion_det.total_2);
              cmd.Parameters.AddWithValue("@total_3", ctz_log_cotizacion_det.total_3);
              cmd.Parameters.AddWithValue("@total_iva_1", ctz_log_cotizacion_det.total_iva_1);
              cmd.Parameters.AddWithValue("@total_iva_2", ctz_log_cotizacion_det.total_iva_2);
              cmd.Parameters.AddWithValue("@total_iva_3", ctz_log_cotizacion_det.total_iva_3);
              cmd.Parameters.AddWithValue("@unidad_equivale", ctz_log_cotizacion_det.unidad_equivale);
              cmd.Parameters.AddWithValue("@_id_cotizacion_det_log", ctz_log_cotizacion_det.id_cotizacion_det_log);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_log_cotizacion_detDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(ctz_log_cotizacion_detEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from ctz_log_cotizacion_det where id_cotizacion_det_log = @id_cotizacion_det_log"; 
          SqlCommand cmd_encontrar = new SqlCommand(sql_encontrar, conn);
           cmd_encontrar.Parameters.AddWithValue("@id_cotizacion_det_log", u.id_cotizacion_det_log);
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


  public static string Delete(ctz_log_cotizacion_detEntity ctz_log_cotizacion_det)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from ctz_log_cotizacion_det " + 
          " where id_cotizacion_det_log = @id_cotizacion_det_log ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@id_cotizacion_det_log", ctz_log_cotizacion_det.id_cotizacion_det_log);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_log_cotizacion_detDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref ctz_log_cotizacion_detEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from ctz_log_cotizacion_det where id_cotizacion_det_log = @id_cotizacion_det_log"; 
          SqlCommand cmd = new SqlCommand(sql, conn);
           cmd.Parameters.AddWithValue("@id_cotizacion_det_log", u.id_cotizacion_det_log);
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
              return "Error en ctz_log_cotizacion_detDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
