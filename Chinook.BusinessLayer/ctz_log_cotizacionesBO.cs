using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Entities;
using CRM.DataAccess;
using System.Data;

namespace CRM.BusinessLayer
{
  public static class ctz_log_cotizacionesBO
  {



      public static String registrar(ctz_log_cotizacionesEntity b)
      {
          return ctz_log_cotizacionesDAL.Insert(b);
      }

      public static String registrar_scope(ctz_log_cotizacionesEntity b)
      {
          return ctz_log_cotizacionesDAL.Insert_Scope(b);
      }

      public static String actualizar(ctz_log_cotizacionesEntity b)
      {
          return ctz_log_cotizacionesDAL.Update(b);
      }

      public static String agregar(ctz_log_cotizacionesEntity b)
      {
          return ctz_log_cotizacionesDAL.Agregar(b);
      }

      public static String eliminar(ctz_log_cotizacionesEntity b)
      {
          return ctz_log_cotizacionesDAL.Delete(b);
      }

      public static string encontrar(ref ctz_log_cotizacionesEntity u)
      {
          return ctz_log_cotizacionesDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return ctz_log_cotizacionesDAL.GetAll(sql_where);
      }


  }
}
