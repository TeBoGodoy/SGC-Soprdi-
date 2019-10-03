using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Entities;
using CRM.DataAccess;
using System.Data;

namespace CRM.BusinessLayer
{
  public static class ctz_cotizacion_detBO
  {



      public static String registrar(ctz_cotizacion_detEntity b)
      {
          return ctz_cotizacion_detDAL.Insert(b);
      }

      public static String registrar_scope(ctz_cotizacion_detEntity b)
      {
          return ctz_cotizacion_detDAL.Insert_Scope(b);
      }

      public static String actualizar(ctz_cotizacion_detEntity b)
      {
          return ctz_cotizacion_detDAL.Update(b);
      }

      public static String agregar(ctz_cotizacion_detEntity b)
      {
          return ctz_cotizacion_detDAL.Agregar(b);
      }

      public static String eliminar(ctz_cotizacion_detEntity b)
      {
          return ctz_cotizacion_detDAL.Delete(b);
      }

      public static string encontrar(ref ctz_cotizacion_detEntity u)
      {
          return ctz_cotizacion_detDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return ctz_cotizacion_detDAL.GetAll(sql_where);
      }


  }
}
