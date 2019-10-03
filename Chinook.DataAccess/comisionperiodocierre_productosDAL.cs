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

  public static class comisionperiodocierre_productosDAL
  {
      private static comisionperiodocierre_productosEntity Load(IDataReader reader)
      {
          comisionperiodocierre_productosEntity comisionperiodocierre_productos = new comisionperiodocierre_productosEntity();
          comisionperiodocierre_productos.cod_periodo = Convert.ToString(reader["cod_periodo"]);
          comisionperiodocierre_productos.cod_regla = Convert.ToString(reader["cod_regla"]);
          comisionperiodocierre_productos.númfactura = Convert.ToString(reader["númfactura"]);
          comisionperiodocierre_productos.porcentaje = int.Parse(reader["porcentaje"].ToString());
          comisionperiodocierre_productos.porcentaje_edit = int.Parse(reader["porcentaje_edit"].ToString());
          comisionperiodocierre_productos.producto = Convert.ToString(reader["producto"]);
          comisionperiodocierre_productos.vendedor = Convert.ToString(reader["vendedor"]);

          return comisionperiodocierre_productos;
      }

  public static DataTable GetAll(string sql_where)
  {
      DataTable dt = new DataTable();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT *  from comisionperiodocierre_productos "+ sql_where;
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter ap = new SqlDataAdapter(cmd);
          ap.Fill(dt);
      }
      return dt;
  }

        public static string Update2(comisionperiodocierre_productosEntity comisionperiodocierre_productos)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"update comisionperiodocierre_productos " +
                "set " +
                "cod_periodo = @cod_periodo, " +
                "cod_regla = @cod_regla, " +
                "númfactura = @númfactura, " +
                "porcentaje = @porcentaje, " +
                "porcentaje_edit = @porcentaje_edit, " +
                "producto = @producto, " +
                "vendedor = @vendedor" +
                " where cod_periodo = @_cod_periodo And cod_regla = @_cod_regla And númfactura = @_númfactura And producto = @_producto And vendedor = @_vendedor ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_periodo", comisionperiodocierre_productos.cod_periodo);
                    cmd.Parameters.AddWithValue("@cod_regla", comisionperiodocierre_productos.cod_regla);
                    cmd.Parameters.AddWithValue("@númfactura", comisionperiodocierre_productos.númfactura);
                    cmd.Parameters.AddWithValue("@porcentaje", comisionperiodocierre_productos.porcentaje);
                    cmd.Parameters.AddWithValue("@porcentaje_edit", comisionperiodocierre_productos.porcentaje_edit);
                    cmd.Parameters.AddWithValue("@producto", comisionperiodocierre_productos.producto);
                    cmd.Parameters.AddWithValue("@vendedor", comisionperiodocierre_productos.vendedor);
                    cmd.Parameters.AddWithValue("@_cod_periodo", comisionperiodocierre_productos.cod_periodo);
                    cmd.Parameters.AddWithValue("@_cod_regla", comisionperiodocierre_productos.cod_regla);
                    cmd.Parameters.AddWithValue("@_númfactura", comisionperiodocierre_productos.númfactura);
                    cmd.Parameters.AddWithValue("@_producto", comisionperiodocierre_productos.producto);
                    cmd.Parameters.AddWithValue("@_vendedor", comisionperiodocierre_productos.vendedor);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en comisionperiodocierre_productosDAL.Update Detalle: " + EX.Message;
                    }
                }
            }
        }

        public static string Update_sinporcentaje(comisionperiodocierre_productosEntity comisionperiodocierre_productos)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"update comisionperiodocierre_productos " +
                "set " +
                "cod_periodo = @cod_periodo, " +
                "cod_regla = @cod_regla, " +
                "númfactura = @númfactura, " +
                "porcentaje = @porcentaje, " +
                "porcentaje_edit = NULL, " +
                "producto = @producto, " +
                "vendedor = @vendedor" +
                " where cod_periodo = @_cod_periodo And cod_regla = @_cod_regla And númfactura = @_númfactura And producto = @_producto And vendedor = @_vendedor ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_periodo", comisionperiodocierre_productos.cod_periodo);
                    cmd.Parameters.AddWithValue("@cod_regla", comisionperiodocierre_productos.cod_regla);
                    cmd.Parameters.AddWithValue("@númfactura", comisionperiodocierre_productos.númfactura);
                    cmd.Parameters.AddWithValue("@porcentaje", comisionperiodocierre_productos.porcentaje);
                    cmd.Parameters.AddWithValue("@porcentaje_edit", comisionperiodocierre_productos.porcentaje);
                    cmd.Parameters.AddWithValue("@producto", comisionperiodocierre_productos.producto);
                    cmd.Parameters.AddWithValue("@vendedor", comisionperiodocierre_productos.vendedor);
                    cmd.Parameters.AddWithValue("@_cod_periodo", comisionperiodocierre_productos.cod_periodo);
                    cmd.Parameters.AddWithValue("@_cod_regla", comisionperiodocierre_productos.cod_regla);
                    cmd.Parameters.AddWithValue("@_númfactura", comisionperiodocierre_productos.númfactura);
                    cmd.Parameters.AddWithValue("@_producto", comisionperiodocierre_productos.producto);
                    cmd.Parameters.AddWithValue("@_vendedor", comisionperiodocierre_productos.vendedor);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en comisionperiodocierre_productosDAL.Update_sinporcentaje Detalle: " + EX.Message;
                    }
                }
            }
        }

        public static string Insert(comisionperiodocierre_productosEntity comisionperiodocierre_productos)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into comisionperiodocierre_productos(cod_periodo, cod_regla, númfactura, porcentaje, porcentaje_edit, producto, vendedor) "+
          "values (@cod_periodo, @cod_regla, @númfactura, @porcentaje, NULL, @producto, @vendedor)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
                    //comisionperiodocierre_productos.porcentaje_edit = null;
              cmd.Parameters.AddWithValue("@cod_periodo", comisionperiodocierre_productos.cod_periodo);
              cmd.Parameters.AddWithValue("@cod_regla", comisionperiodocierre_productos.cod_regla);
              cmd.Parameters.AddWithValue("@númfactura", comisionperiodocierre_productos.númfactura);
              cmd.Parameters.AddWithValue("@porcentaje", comisionperiodocierre_productos.porcentaje);
              cmd.Parameters.AddWithValue("@porcentaje_edit", comisionperiodocierre_productos.porcentaje_edit);
              cmd.Parameters.AddWithValue("@producto", comisionperiodocierre_productos.producto);
              cmd.Parameters.AddWithValue("@vendedor", comisionperiodocierre_productos.vendedor);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
              return "Error en comisionperiodocierre_productosDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Insert_Scope(comisionperiodocierre_productosEntity comisionperiodocierre_productos)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"insert into comisionperiodocierre_productos(cod_periodo, cod_regla, númfactura, porcentaje, porcentaje_edit, producto, vendedor) "+
          "values (@cod_periodo, @cod_regla, @númfactura, @porcentaje, @porcentaje_edit, @producto, @vendedor)";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@cod_periodo", comisionperiodocierre_productos.cod_periodo);
              cmd.Parameters.AddWithValue("@cod_regla", comisionperiodocierre_productos.cod_regla);
              cmd.Parameters.AddWithValue("@númfactura", comisionperiodocierre_productos.númfactura);
              cmd.Parameters.AddWithValue("@porcentaje", comisionperiodocierre_productos.porcentaje);
              cmd.Parameters.AddWithValue("@porcentaje_edit", comisionperiodocierre_productos.porcentaje_edit);
              cmd.Parameters.AddWithValue("@producto", comisionperiodocierre_productos.producto);
              cmd.Parameters.AddWithValue("@vendedor", comisionperiodocierre_productos.vendedor);
              try
              {
                  string respuesta = cmd.ExecuteScalar().ToString(); 
                  return respuesta;
              }
              catch (Exception EX)
              {
              return "Error en comisionperiodocierre_productosDAL.Insert Detalle: " + EX.Message;
              }
          }
      }
  }

  public static string Update(comisionperiodocierre_productosEntity comisionperiodocierre_productos)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"update comisionperiodocierre_productos " +
          "set " +
          "cod_periodo = @cod_periodo, " +
          "cod_regla = @cod_regla, " +
          "númfactura = @númfactura, " +
          "porcentaje = @porcentaje, " +
          "porcentaje_edit = (SELECT porcentaje_edit from comisionperiodocierre_productos where cod_periodo = @_cod_periodo And cod_regla = @_cod_regla And númfactura = @_númfactura And producto = @_producto And vendedor = @_vendedor ), " +
          "producto = @producto, " +
          "vendedor = @vendedor" +
          " where cod_periodo = @_cod_periodo And cod_regla = @_cod_regla And númfactura = @_númfactura And producto = @_producto And vendedor = @_vendedor ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
              {
              cmd.Parameters.AddWithValue("@cod_periodo", comisionperiodocierre_productos.cod_periodo);
              cmd.Parameters.AddWithValue("@cod_regla", comisionperiodocierre_productos.cod_regla);
              cmd.Parameters.AddWithValue("@númfactura", comisionperiodocierre_productos.númfactura);
              cmd.Parameters.AddWithValue("@porcentaje", comisionperiodocierre_productos.porcentaje);
              cmd.Parameters.AddWithValue("@porcentaje_edit", comisionperiodocierre_productos.porcentaje_edit);
              cmd.Parameters.AddWithValue("@producto", comisionperiodocierre_productos.producto);
              cmd.Parameters.AddWithValue("@vendedor", comisionperiodocierre_productos.vendedor);
              cmd.Parameters.AddWithValue("@_cod_periodo", comisionperiodocierre_productos.cod_periodo);
              cmd.Parameters.AddWithValue("@_cod_regla", comisionperiodocierre_productos.cod_regla);
              cmd.Parameters.AddWithValue("@_númfactura", comisionperiodocierre_productos.númfactura);
              cmd.Parameters.AddWithValue("@_producto", comisionperiodocierre_productos.producto);
              cmd.Parameters.AddWithValue("@_vendedor", comisionperiodocierre_productos.vendedor);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en comisionperiodocierre_productosDAL.Update Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string Agregar(comisionperiodocierre_productosEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql_encontrar = @"SELECT count(1) from comisionperiodocierre_productos where cod_periodo = @cod_periodo And cod_regla = @cod_regla And númfactura = @númfactura And producto = @producto And vendedor = @vendedor"; 
          SqlCommand cmd_encontrar = new SqlCommand(sql_encontrar, conn);
           cmd_encontrar.Parameters.AddWithValue("@cod_periodo", u.cod_periodo);
           cmd_encontrar.Parameters.AddWithValue("@cod_regla", u.cod_regla);
           cmd_encontrar.Parameters.AddWithValue("@númfactura", u.númfactura);
           cmd_encontrar.Parameters.AddWithValue("@producto", u.producto);
           cmd_encontrar.Parameters.AddWithValue("@vendedor", u.vendedor);
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


  public static string Delete(comisionperiodocierre_productosEntity comisionperiodocierre_productos)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"delete from comisionperiodocierre_productos " + 
          " where cod_periodo = @cod_periodo And cod_regla = @cod_regla And númfactura = @númfactura And producto = @producto And vendedor = @vendedor ";
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          { 
              cmd.Parameters.AddWithValue("@cod_periodo", comisionperiodocierre_productos.cod_periodo);
              cmd.Parameters.AddWithValue("@cod_regla", comisionperiodocierre_productos.cod_regla);
              cmd.Parameters.AddWithValue("@númfactura", comisionperiodocierre_productos.númfactura);
              cmd.Parameters.AddWithValue("@producto", comisionperiodocierre_productos.producto);
              cmd.Parameters.AddWithValue("@vendedor", comisionperiodocierre_productos.vendedor);
              try
              {
                  cmd.ExecuteNonQuery();
                  return "OK";
              }
              catch (Exception EX)
              {
                  return "Error en comisionperiodocierre_productosDAL.Delete Detalle: " + EX.Message;
              }
          }
      }
  }
  public static string encontrar(ref comisionperiodocierre_productosEntity u)
  {
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
      {
          conn.Open();
          string sql = @"SELECT * from comisionperiodocierre_productos where cod_periodo = @cod_periodo And cod_regla = @cod_regla And númfactura = @númfactura And producto = @producto And vendedor = @vendedor"; 
          SqlCommand cmd = new SqlCommand(sql, conn);
           cmd.Parameters.AddWithValue("@cod_periodo", u.cod_periodo);
           cmd.Parameters.AddWithValue("@cod_regla", u.cod_regla);
           cmd.Parameters.AddWithValue("@númfactura", u.númfactura);
           cmd.Parameters.AddWithValue("@producto", u.producto);
           cmd.Parameters.AddWithValue("@vendedor", u.vendedor);
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
              return "Error en comisionperiodocierre_productosDAL.Validar Detalle: " + EX.Message;
          }
      }
      return "No encontrado en DAL";
  }
}
}
