using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Entities;
using CRM.DataAccess;
using System.Data;

namespace CRM.BusinessLayer
{
  public static class ctz_log_enviadasBO
  {



      public static String registrar(ctz_log_enviadasEntity b)
      {
          return ctz_log_enviadasDAL.Insert(b);
      }

      public static String registrar_scope(ctz_log_enviadasEntity b)
      {
          return ctz_log_enviadasDAL.Insert_Scope(b);
      }

      public static String actualizar(ctz_log_enviadasEntity b)
      {
          return ctz_log_enviadasDAL.Update(b);
      }

      public static String agregar(ctz_log_enviadasEntity b)
      {
          return ctz_log_enviadasDAL.Agregar(b);
      }

      public static String eliminar(ctz_log_enviadasEntity b)
      {
          return ctz_log_enviadasDAL.Delete(b);
      }

      public static string encontrar(ref ctz_log_enviadasEntity u)
      {
          return ctz_log_enviadasDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return ctz_log_enviadasDAL.GetAll(sql_where);
      }


  }
}
