using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Entities;
using CRM.DataAccess;
using System.Data;

namespace CRM.BusinessLayer
{
  public static class ctz_log_cotizacionBO
  {



      public static String registrar(ctz_log_cotizacionEntity b)
      {
          return ctz_log_cotizacionDAL.Insert(b);
      }

      public static String registrar_scope(ctz_log_cotizacionEntity b)
      {
          return ctz_log_cotizacionDAL.Insert_Scope(b);
      }

      public static String actualizar(ctz_log_cotizacionEntity b)
      {
          return ctz_log_cotizacionDAL.Update(b);
      }

      public static String agregar(ctz_log_cotizacionEntity b)
      {
          return ctz_log_cotizacionDAL.Agregar(b);
      }

      public static String eliminar(ctz_log_cotizacionEntity b)
      {
          return ctz_log_cotizacionDAL.Delete(b);
      }

      public static string encontrar(ref ctz_log_cotizacionEntity u)
      {
          return ctz_log_cotizacionDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return ctz_log_cotizacionDAL.GetAll(sql_where);
      }


  }
}
