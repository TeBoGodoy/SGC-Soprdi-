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

  public static class ctz_cotizacionDAL
  {
      private static ctz_cotizacionEntity Load(IDataReader reader)
      {
          ctz_cotizacionEntity ctz_cotizacion = new ctz_cotizacionEntity();
          ctz_cotizacion.cod_vendedor = Convert.ToString(reader["cod_vendedor"]);
          ctz_cotizacion.columnas_precio = Convert.ToString(reader["columnas_precio"]);
          ctz_cotizacion.descripcion = Convert.ToString(reader["descripcion"]);
          ctz_cotizacion.estado = Convert.ToString(reader["estado"]);
          ctz_cotizacion.fecha_creacion = DateTime.Parse(reader["fecha_creacion"].ToString());
          ctz_cotizacion.id_cotizacion = int.Parse(reader["id_cotizacion"].ToString());
          ctz_cotizacion.nombre_cotizacion = Convert.ToString(reader["nombre_cotizacion"]);
          ctz_cotizacion.num_columnas = int.Parse(reader["num_columnas"].ToString());
          ctz_cotizacion.servicios_separado = int.Parse(reader["servicios_separado"].ToString());

          return ctz_cotizacion;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from ctz_cotizacion " + sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(ctz_cotizacionEntity ctz_cotizacion)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_cotizacion(cod_vendedor, columnas_precio, descripcion, estado, fecha_creacion, nombre_cotizacion, num_columnas, servicios_separado) "+
          "values (@cod_vendedor, @columnas_precio, @descripcion, @estado, @fecha_creacion, @nombre_cotizacion, @num_columnas, @servicios_separado)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_vendedor", ctz_cotizacion.cod_vendedor);
              cmd.Parameters.AddWithValue("@columnas_precio", ctz_cotizacion.columnas_precio);
              cmd.Parameters.AddWithValue("@descripcion", ctz_cotizacion.descripcion);
              cmd.Parameters.AddWithValue("@estado", ctz_cotizacion.estado);
              cmd.Parameters.AddWithValue("@fecha_creacion", ctz_cotizacion.fecha_creacion);
              cmd.Parameters.AddWithValue("@nombre_cotizacion", ctz_cotizacion.nombre_cotizacion);
              cmd.Parameters.AddWithValue("@num_columnas", ctz_cotizacion.num_columnas);
              cmd.Parameters.AddWithValue("@servicios_separado", ctz_cotizacion.servicios_separado);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en ctz_cotizacionDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(ctz_cotizacionEntity ctz_cotizacion)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_cotizacion(cod_vendedor, columnas_precio, descripcion, estado, fecha_creacion, nombre_cotizacion, num_columnas, servicios_separado) "+
          "values (@cod_vendedor, @columnas_precio, @descripcion, @estado, @fecha_creacion, @nombre_cotizacion, @num_columnas, @servicios_separado);  SELECT SCOPE_IDENTITY(); ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_vendedor", ctz_cotizacion.cod_vendedor); 
              cmd.Parameters.AddWithValue("@columnas_precio", ctz_cotizacion.columnas_precio); 
              cmd.Parameters.AddWithValue("@descripcion", ctz_cotizacion.descripcion); 
              cmd.Parameters.AddWithValue("@estado", ctz_cotizacion.estado); 
              cmd.Parameters.AddWithValue("@fecha_creacion", ctz_cotizacion.fecha_creacion); 
              cmd.Parameters.AddWithValue("@nombre_cotizacion", ctz_cotizacion.nombre_cotizacion); 
              cmd.Parameters.AddWithValue("@num_columnas", ctz_cotizacion.num_columnas); 
              cmd.Parameters.AddWithValue("@servicios_separado", ctz_cotizacion.servicios_separado); 
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en ctz_cotizacionDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(ctz_cotizacionEntity ctz_cotizacion)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update ctz_cotizacion " +
          "set " +
          "cod_vendedor = @cod_vendedor, " +
          "columnas_precio = @columnas_precio, " +
          "descripcion = @descripcion, " +
          "estado = @estado, " +
          "fecha_creacion = @fecha_creacion, " +
          "nombre_cotizacion = @nombre_cotizacion, " +
          "num_columnas = @num_columnas, " +
          "servicios_separado = @servicios_separado" +
          " where id_cotizacion = @_id_cotizacion ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cod_vendedor", ctz_cotizacion.cod_vendedor);
              cmd.Parameters.AddWithValue("@columnas_precio", ctz_cotizacion.columnas_precio);
              cmd.Parameters.AddWithValue("@descripcion", ctz_cotizacion.descripcion);
              cmd.Parameters.AddWithValue("@estado", ctz_cotizacion.estado);
              cmd.Parameters.AddWithValue("@fecha_creacion", ctz_cotizacion.fecha_creacion);
              cmd.Parameters.AddWithValue("@nombre_cotizacion", ctz_cotizacion.nombre_cotizacion);
              cmd.Parameters.AddWithValue("@num_columnas", ctz_cotizacion.num_columnas);
              cmd.Parameters.AddWithValue("@servicios_separado", ctz_cotizacion.servicios_separado);
              cmd.Parameters.AddWithValue("@_id_cotizacion", ctz_cotizacion.id_cotizacion);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_cotizacionDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(ctz_cotizacionEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from ctz_cotizacion where id_cotizacion = @id_cotizacion"; 
          SqlCommand cmd_encontrar = new SqlCommand(sql_encontrar, conn);
           cmd_encontrar.Parameters.AddWithValue("@id_cotizacion", u.id_cotizacion);
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


  public static string Delete(ctz_cotizacionEntity ctz_cotizacion)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from ctz_cotizacion " + 
          " where id_cotizacion = @id_cotizacion ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@id_cotizacion", ctz_cotizacion.id_cotizacion);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_cotizacionDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref ctz_cotizacionEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from ctz_cotizacion where id_cotizacion = @id_cotizacion"; 
          SqlCommand cmd = new SqlCommand(sql, conn);
           cmd.Parameters.AddWithValue("@id_cotizacion", u.id_cotizacion);
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
              return "Error en ctz_cotizacionDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
