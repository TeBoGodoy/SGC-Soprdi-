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

  public static class ctz_log_cotizaciones_detDAL
  {
      private static ctz_log_cotizaciones_detEntity Load(IDataReader reader)
      {
          ctz_log_cotizaciones_detEntity ctz_log_cotizaciones_det = new ctz_log_cotizaciones_detEntity();
          ctz_log_cotizaciones_det.cantidad = double.Parse(reader["cantidad"].ToString());
          ctz_log_cotizaciones_det.id_cotizacion_det_log = int.Parse(reader["id_cotizacion_det_log"].ToString());
          ctz_log_cotizaciones_det.id_cotizacion_log = int.Parse(reader["id_cotizacion_log"].ToString());
          ctz_log_cotizaciones_det.nom_producto = Convert.ToString(reader["nom_producto"]);
          ctz_log_cotizaciones_det.producto = Convert.ToString(reader["producto"]);
          ctz_log_cotizaciones_det.unidades_por_embalaje = int.Parse(reader["unidades_por_embalaje"].ToString());

          return ctz_log_cotizaciones_det;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from ctz_log_cotizaciones_det " + sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(ctz_log_cotizaciones_detEntity ctz_log_cotizaciones_det)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_log_cotizaciones_det(cantidad, id_cotizacion_log, nom_producto, producto, unidades_por_embalaje) "+
          "values (@cantidad, @id_cotizacion_log, @nom_producto, @producto, @unidades_por_embalaje)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cantidad", ctz_log_cotizaciones_det.cantidad);
              cmd.Parameters.AddWithValue("@id_cotizacion_log", ctz_log_cotizaciones_det.id_cotizacion_log);
              cmd.Parameters.AddWithValue("@nom_producto", ctz_log_cotizaciones_det.nom_producto);
              cmd.Parameters.AddWithValue("@producto", ctz_log_cotizaciones_det.producto);
              cmd.Parameters.AddWithValue("@unidades_por_embalaje", ctz_log_cotizaciones_det.unidades_por_embalaje);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en ctz_log_cotizaciones_detDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(ctz_log_cotizaciones_detEntity ctz_log_cotizaciones_det)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_log_cotizaciones_det(cantidad, id_cotizacion_log, nom_producto, producto, unidades_por_embalaje) "+
          "values (@cantidad, @id_cotizacion_log, @nom_producto, @producto, @unidades_por_embalaje);  SELECT SCOPE_IDENTITY(); ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cantidad", ctz_log_cotizaciones_det.cantidad); 
              cmd.Parameters.AddWithValue("@id_cotizacion_log", ctz_log_cotizaciones_det.id_cotizacion_log); 
              cmd.Parameters.AddWithValue("@nom_producto", ctz_log_cotizaciones_det.nom_producto); 
              cmd.Parameters.AddWithValue("@producto", ctz_log_cotizaciones_det.producto); 
              cmd.Parameters.AddWithValue("@unidades_por_embalaje", ctz_log_cotizaciones_det.unidades_por_embalaje); 
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en ctz_log_cotizaciones_detDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(ctz_log_cotizaciones_detEntity ctz_log_cotizaciones_det)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update ctz_log_cotizaciones_det " +
          "set " +
          "cantidad = @cantidad, " +
          "id_cotizacion_log = @id_cotizacion_log, " +
          "nom_producto = @nom_producto, " +
          "producto = @producto, " +
          "unidades_por_embalaje = @unidades_por_embalaje" +
          " where id_cotizacion_det_log = @_id_cotizacion_det_log ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cantidad", ctz_log_cotizaciones_det.cantidad);
              cmd.Parameters.AddWithValue("@id_cotizacion_log", ctz_log_cotizaciones_det.id_cotizacion_log);
              cmd.Parameters.AddWithValue("@nom_producto", ctz_log_cotizaciones_det.nom_producto);
              cmd.Parameters.AddWithValue("@producto", ctz_log_cotizaciones_det.producto);
              cmd.Parameters.AddWithValue("@unidades_por_embalaje", ctz_log_cotizaciones_det.unidades_por_embalaje);
              cmd.Parameters.AddWithValue("@_id_cotizacion_det_log", ctz_log_cotizaciones_det.id_cotizacion_det_log);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_log_cotizaciones_detDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(ctz_log_cotizaciones_detEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from ctz_log_cotizaciones_det where id_cotizacion_det_log = @id_cotizacion_det_log"; 
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


  public static string Delete(ctz_log_cotizaciones_detEntity ctz_log_cotizaciones_det)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from ctz_log_cotizaciones_det " + 
          " where id_cotizacion_det_log = @id_cotizacion_det_log ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@id_cotizacion_det_log", ctz_log_cotizaciones_det.id_cotizacion_det_log);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_log_cotizaciones_detDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref ctz_log_cotizaciones_detEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from ctz_log_cotizaciones_det where id_cotizacion_det_log = @id_cotizacion_det_log"; 
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
              return "Error en ctz_log_cotizaciones_detDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
