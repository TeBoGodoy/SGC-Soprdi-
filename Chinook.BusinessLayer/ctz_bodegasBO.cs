using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Entities;
using CRM.DataAccess;
using System.Data;

namespace CRM.BusinessLayer
{
  public static class ctz_bodegasBO
  {



      public static String registrar(ctz_bodegasEntity b)
      {
          return ctz_bodegasDAL.Insert(b);
      }

      public static String registrar_scope(ctz_bodegasEntity b)
      {
          return ctz_bodegasDAL.Insert_Scope(b);
      }

      public static String actualizar(ctz_bodegasEntity b)
      {
          return ctz_bodegasDAL.Update(b);
      }

      public static String agregar(ctz_bodegasEntity b)
      {
          return ctz_bodegasDAL.Agregar(b);
      }

      public static String eliminar(ctz_bodegasEntity b)
      {
          return ctz_bodegasDAL.Delete(b);
      }

      public static string encontrar(ref ctz_bodegasEntity u)
      {
          return ctz_bodegasDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return ctz_bodegasDAL.GetAll(sql_where);
      }


  }
}
