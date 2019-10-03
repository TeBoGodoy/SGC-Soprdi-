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

  public static class ctz_log_cotizacionesDAL
  {
      private static ctz_log_cotizacionesEntity Load(IDataReader reader)
      {
          ctz_log_cotizacionesEntity ctz_log_cotizaciones = new ctz_log_cotizacionesEntity();
          ctz_log_cotizaciones.cod_vendedor = Convert.ToString(reader["cod_vendedor"]);
          ctz_log_cotizaciones.comentario_correo = Convert.ToString(reader["comentario_correo"]);
          ctz_log_cotizaciones.correlativo = int.Parse(reader["correlativo"].ToString());
          ctz_log_cotizaciones.correo = Convert.ToString(reader["correo"]);
          ctz_log_cotizaciones.descripcion = Convert.ToString(reader["descripcion"]);
          ctz_log_cotizaciones.fecha_envio = DateTime.Parse(reader["fecha_envio"].ToString());
          ctz_log_cotizaciones.id_cotizacion_log = int.Parse(reader["id_cotizacion_log"].ToString());
          ctz_log_cotizaciones.nombre_cliente = Convert.ToString(reader["nombre_cliente"]);
          ctz_log_cotizaciones.nombre_cotizacion = Convert.ToString(reader["nombre_cotizacion"]);
          ctz_log_cotizaciones.rut_cliente = Convert.ToString(reader["rut_cliente"]);
          ctz_log_cotizaciones.servicios_separado = int.Parse(reader["servicios_separado"].ToString());

          return ctz_log_cotizaciones;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from ctz_log_cotizaciones " + sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

  public static string Insert(ctz_log_cotizacionesEntity ctz_log_cotizaciones)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_log_cotizaciones(cod_vendedor, comentario_correo, correlativo, correo, descripcion, fecha_envio, nombre_cliente, nombre_cotizacion, rut_cliente, servicios_separado) "+
          "values (@cod_vendedor, @comentario_correo, @correlativo, @correo, @descripcion, @fecha_envio, @nombre_cliente, @nombre_cotizacion, @rut_cliente, @servicios_separado)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_vendedor", ctz_log_cotizaciones.cod_vendedor);
              cmd.Parameters.AddWithValue("@comentario_correo", ctz_log_cotizaciones.comentario_correo);
              cmd.Parameters.AddWithValue("@correlativo", ctz_log_cotizaciones.correlativo);
              cmd.Parameters.AddWithValue("@correo", ctz_log_cotizaciones.correo);
              cmd.Parameters.AddWithValue("@descripcion", ctz_log_cotizaciones.descripcion);
              cmd.Parameters.AddWithValue("@fecha_envio", ctz_log_cotizaciones.fecha_envio);
              cmd.Parameters.AddWithValue("@nombre_cliente", ctz_log_cotizaciones.nombre_cliente);
              cmd.Parameters.AddWithValue("@nombre_cotizacion", ctz_log_cotizaciones.nombre_cotizacion);
              cmd.Parameters.AddWithValue("@rut_cliente", ctz_log_cotizaciones.rut_cliente);
              cmd.Parameters.AddWithValue("@servicios_separado", ctz_log_cotizaciones.servicios_separado);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en ctz_log_cotizacionesDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(ctz_log_cotizacionesEntity ctz_log_cotizaciones)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into ctz_log_cotizaciones(cod_vendedor, comentario_correo, correlativo, correo, descripcion, fecha_envio, nombre_cliente, nombre_cotizacion, rut_cliente, servicios_separado) "+
          "values (@cod_vendedor, @comentario_correo, @correlativo, @correo, @descripcion, @fecha_envio, @nombre_cliente, @nombre_cotizacion, @rut_cliente, @servicios_separado);  SELECT SCOPE_IDENTITY(); ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_vendedor", ctz_log_cotizaciones.cod_vendedor); 
              cmd.Parameters.AddWithValue("@comentario_correo", ctz_log_cotizaciones.comentario_correo); 
              cmd.Parameters.AddWithValue("@correlativo", ctz_log_cotizaciones.correlativo); 
              cmd.Parameters.AddWithValue("@correo", ctz_log_cotizaciones.correo); 
              cmd.Parameters.AddWithValue("@descripcion", ctz_log_cotizaciones.descripcion); 
              cmd.Parameters.AddWithValue("@fecha_envio", ctz_log_cotizaciones.fecha_envio); 
              cmd.Parameters.AddWithValue("@nombre_cliente", ctz_log_cotizaciones.nombre_cliente); 
              cmd.Parameters.AddWithValue("@nombre_cotizacion", ctz_log_cotizaciones.nombre_cotizacion); 
              cmd.Parameters.AddWithValue("@rut_cliente", ctz_log_cotizaciones.rut_cliente); 
              cmd.Parameters.AddWithValue("@servicios_separado", ctz_log_cotizaciones.servicios_separado); 
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en ctz_log_cotizacionesDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(ctz_log_cotizacionesEntity ctz_log_cotizaciones)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update ctz_log_cotizaciones " +
          "set " +
          "cod_vendedor = @cod_vendedor, " +
          "comentario_correo = @comentario_correo, " +
          "correlativo = @correlativo, " +
          "correo = @correo, " +
          "descripcion = @descripcion, " +
          "fecha_envio = @fecha_envio, " +
          "nombre_cliente = @nombre_cliente, " +
          "nombre_cotizacion = @nombre_cotizacion, " +
          "rut_cliente = @rut_cliente, " +
          "servicios_separado = @servicios_separado" +
          " where id_cotizacion_log = @_id_cotizacion_log ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cod_vendedor", ctz_log_cotizaciones.cod_vendedor);
              cmd.Parameters.AddWithValue("@comentario_correo", ctz_log_cotizaciones.comentario_correo);
              cmd.Parameters.AddWithValue("@correlativo", ctz_log_cotizaciones.correlativo);
              cmd.Parameters.AddWithValue("@correo", ctz_log_cotizaciones.correo);
              cmd.Parameters.AddWithValue("@descripcion", ctz_log_cotizaciones.descripcion);
              cmd.Parameters.AddWithValue("@fecha_envio", ctz_log_cotizaciones.fecha_envio);
              cmd.Parameters.AddWithValue("@nombre_cliente", ctz_log_cotizaciones.nombre_cliente);
              cmd.Parameters.AddWithValue("@nombre_cotizacion", ctz_log_cotizaciones.nombre_cotizacion);
              cmd.Parameters.AddWithValue("@rut_cliente", ctz_log_cotizaciones.rut_cliente);
              cmd.Parameters.AddWithValue("@servicios_separado", ctz_log_cotizaciones.servicios_separado);
              cmd.Parameters.AddWithValue("@_id_cotizacion_log", ctz_log_cotizaciones.id_cotizacion_log);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_log_cotizacionesDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(ctz_log_cotizacionesEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from ctz_log_cotizaciones where id_cotizacion_log = @id_cotizacion_log"; 
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


  public static string Delete(ctz_log_cotizacionesEntity ctz_log_cotizaciones)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from ctz_log_cotizaciones " + 
          " where id_cotizacion_log = @id_cotizacion_log ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@id_cotizacion_log", ctz_log_cotizaciones.id_cotizacion_log);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en ctz_log_cotizacionesDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref ctz_log_cotizacionesEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from ctz_log_cotizaciones where id_cotizacion_log = @id_cotizacion_log"; 
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
              return "Error en ctz_log_cotizacionesDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
