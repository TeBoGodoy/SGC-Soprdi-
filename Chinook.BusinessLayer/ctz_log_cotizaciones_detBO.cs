using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Entities;
using CRM.DataAccess;
using System.Data;

namespace CRM.BusinessLayer
{
  public static class ctz_log_cotizaciones_detBO
  {



      public static String registrar(ctz_log_cotizaciones_detEntity b)
      {
          return ctz_log_cotizaciones_detDAL.Insert(b);
      }

      public static String registrar_scope(ctz_log_cotizaciones_detEntity b)
      {
          return ctz_log_cotizaciones_detDAL.Insert_Scope(b);
      }

      public static String actualizar(ctz_log_cotizaciones_detEntity b)
      {
          return ctz_log_cotizaciones_detDAL.Update(b);
      }

      public static String agregar(ctz_log_cotizaciones_detEntity b)
      {
          return ctz_log_cotizaciones_detDAL.Agregar(b);
      }

      public static String eliminar(ctz_log_cotizaciones_detEntity b)
      {
          return ctz_log_cotizaciones_detDAL.Delete(b);
      }

      public static string encontrar(ref ctz_log_cotizaciones_detEntity u)
      {
          return ctz_log_cotizaciones_detDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return ctz_log_cotizaciones_detDAL.GetAll(sql_where);
      }


  }
}
