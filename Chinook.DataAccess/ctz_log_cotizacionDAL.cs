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

  public static class ctz_log_cotizacionDAL
  {
      private static ctz_log_cotizacionEntity Load(IDataReader reader)
      {
          ctz_log_cotizacionEntity ctz_log_cotizacion = new ctz_log_cotizacionEntity();
          ctz_log_cotizacion.cod_vendedor = Convert.ToString(reader["cod_vendedor"]);
          ctz_log_cotizacion.columnas_precio = Convert.ToString(reader["columnas_precio"]);
          ctz_log_cotizacion.correlativo = int.Parse(reader["correlativo"].ToString());
          ctz_log_cotizacion.correo_cliente = Convert.ToString(reader["correo_cliente"]);
          ctz_log_cotizacion.descripcion = Convert.ToString(reader["descripcion"]);
          ctz_log_cotizacion.estado_correo = Convert.ToString(reader["estado_correo"]);
          ctz_log_cotizacion.fecha_envio = DateTime.Parse(reader["fecha_envio"].ToString());
          ctz_log_cotizacion.id_cotizacion = int.Parse(reader["id_cotizacion"].ToString());
          ctz_log_cotizacion.id_cotizacion_log = int.Parse(reader["id_cotizacion_log"].ToString());
          ctz_log_cotizacion.nombre_cotizacion = Convert.ToString(reader["nombre_cotizacion"]);
          ctz_log_cotizacion.num_columnas = int.Parse(reader["num_columnas"].ToString());
          ctz_log_cotizacion.rut_cliente = Convert.ToString(reader["rut_cliente"]);

          return ctz_log_cotizacion;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from v_ctz_log_cotizacion " + sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(ctz_log_cotizacionEntity ctz_log_cotizacion)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_log_cotizacion(cod_vendedor, columnas_precio, correlativo, correo_cliente, descripcion, estado_correo, fecha_envio, id_cotizacion, nombre_cotizacion, num_columnas, rut_cliente) "+
          "values (@cod_vendedor, @columnas_precio, (select MAX(isnull(correlativo,0))+1 from ctz_log_cotizacion), @correo_cliente, @descripcion, @estado_correo, @fecha_envio, @id_cotizacion, @nombre_cotizacion, @num_columnas, @rut_cliente)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_vendedor", ctz_log_cotizacion.cod_vendedor);
              cmd.Parameters.AddWithValue("@columnas_precio", ctz_log_cotizacion.columnas_precio);
              //cmd.Parameters.AddWithValue("@correlativo", ctz_log_cotizacion.correlativo);
              cmd.Parameters.AddWithValue("@correo_cliente", ctz_log_cotizacion.correo_cliente);
              cmd.Parameters.AddWithValue("@descripcion", ctz_log_cotizacion.descripcion);
              cmd.Parameters.AddWithValue("@estado_correo", ctz_log_cotizacion.estado_correo);
              cmd.Parameters.AddWithValue("@fecha_envio", ctz_log_cotizacion.fecha_envio);
              cmd.Parameters.AddWithValue("@id_cotizacion", ctz_log_cotizacion.id_cotizacion);
              cmd.Parameters.AddWithValue("@nombre_cotizacion", ctz_log_cotizacion.nombre_cotizacion);
              cmd.Parameters.AddWithValue("@num_columnas", ctz_log_cotizacion.num_columnas);
              cmd.Parameters.AddWithValue("@rut_cliente", ctz_log_cotizacion.rut_cliente);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en ctz_log_cotizacionDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(ctz_log_cotizacionEntity ctz_log_cotizacion)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_log_cotizacion(cod_vendedor, columnas_precio, correlativo, correo_cliente, descripcion, estado_correo, fecha_envio, id_cotizacion, nombre_cotizacion, num_columnas, rut_cliente) "+
          "values (@cod_vendedor, @columnas_precio, (select MAX(isnull(correlativo,0))+1 from ctz_log_cotizacion), @correo_cliente, @descripcion, @estado_correo, @fecha_envio, @id_cotizacion, @nombre_cotizacion, @num_columnas, @rut_cliente);  SELECT SCOPE_IDENTITY(); ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_vendedor", ctz_log_cotizacion.cod_vendedor); 
              cmd.Parameters.AddWithValue("@columnas_precio", ctz_log_cotizacion.columnas_precio); 
              //cmd.Parameters.AddWithValue("@correlativo", ctz_log_cotizacion.correlativo); 
              cmd.Parameters.AddWithValue("@correo_cliente", ctz_log_cotizacion.correo_cliente); 
              cmd.Parameters.AddWithValue("@descripcion", ctz_log_cotizacion.descripcion); 
              cmd.Parameters.AddWithValue("@estado_correo", ctz_log_cotizacion.estado_correo); 
              cmd.Parameters.AddWithValue("@fecha_envio", ctz_log_cotizacion.fecha_envio); 
              cmd.Parameters.AddWithValue("@id_cotizacion", ctz_log_cotizacion.id_cotizacion); 
              cmd.Parameters.AddWithValue("@nombre_cotizacion", ctz_log_cotizacion.nombre_cotizacion); 
              cmd.Parameters.AddWithValue("@num_columnas", ctz_log_cotizacion.num_columnas); 
              cmd.Parameters.AddWithValue("@rut_cliente", ctz_log_cotizacion.rut_cliente); 
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en ctz_log_cotizacionDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(ctz_log_cotizacionEntity ctz_log_cotizacion)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update ctz_log_cotizacion " +
          "set " +
          "cod_vendedor = @cod_vendedor, " +
          "columnas_precio = @columnas_precio, " +
          "correlativo = @correlativo, " +
          "correo_cliente = @correo_cliente, " +
          "descripcion = @descripcion, " +
          "estado_correo = @estado_correo, " +
          "fecha_envio = @fecha_envio, " +
          "id_cotizacion = @id_cotizacion, " +
          "nombre_cotizacion = @nombre_cotizacion, " +
          "num_columnas = @num_columnas, " +
          "rut_cliente = @rut_cliente" +
          " where id_cotizacion_log = @_id_cotizacion_log ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cod_vendedor", ctz_log_cotizacion.cod_vendedor);
              cmd.Parameters.AddWithValue("@columnas_precio", ctz_log_cotizacion.columnas_precio);
              cmd.Parameters.AddWithValue("@correlativo", ctz_log_cotizacion.correlativo);
              cmd.Parameters.AddWithValue("@correo_cliente", ctz_log_cotizacion.correo_cliente);
              cmd.Parameters.AddWithValue("@descripcion", ctz_log_cotizacion.descripcion);
              cmd.Parameters.AddWithValue("@estado_correo", ctz_log_cotizacion.estado_correo);
              cmd.Parameters.AddWithValue("@fecha_envio", ctz_log_cotizacion.fecha_envio);
              cmd.Parameters.AddWithValue("@id_cotizacion", ctz_log_cotizacion.id_cotizacion);
              cmd.Parameters.AddWithValue("@nombre_cotizacion", ctz_log_cotizacion.nombre_cotizacion);
              cmd.Parameters.AddWithValue("@num_columnas", ctz_log_cotizacion.num_columnas);
              cmd.Parameters.AddWithValue("@rut_cliente", ctz_log_cotizacion.rut_cliente);
              cmd.Parameters.AddWithValue("@_id_cotizacion_log", ctz_log_cotizacion.id_cotizacion_log);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_log_cotizacionDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(ctz_log_cotizacionEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from ctz_log_cotizacion where id_cotizacion_log = @id_cotizacion_log"; 
          SqlCommand cmd_encontrar = new SqlCommand(sql_encontrar, conn);
           cmd_encontrar.Parameters.AddWithValue("@id_cotizacion_log", u.id_cotizacion_log);
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


  public static string Delete(ctz_log_cotizacionEntity ctz_log_cotizacion)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from ctz_log_cotizacion " + 
          " where id_cotizacion_log = @id_cotizacion_log ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@id_cotizacion_log", ctz_log_cotizacion.id_cotizacion_log);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_log_cotizacionDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref ctz_log_cotizacionEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from ctz_log_cotizacion where id_cotizacion_log = @id_cotizacion_log"; 
          SqlCommand cmd = new SqlCommand(sql, conn);
           cmd.Parameters.AddWithValue("@id_cotizacion_log", u.id_cotizacion_log);
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
              return "Error en ctz_log_cotizacionDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
