using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Entities;
using CRM.DataAccess;
using System.Data;

namespace CRM.BusinessLayer
{
  public static class ctz_cotizacionBO
  {



      public static String registrar(ctz_cotizacionEntity b)
      {
          return ctz_cotizacionDAL.Insert(b);
      }

      public static String registrar_scope(ctz_cotizacionEntity b)
      {
          return ctz_cotizacionDAL.Insert_Scope(b);
      }

      public static String actualizar(ctz_cotizacionEntity b)
      {
          return ctz_cotizacionDAL.Update(b);
      }

      public static String agregar(ctz_cotizacionEntity b)
      {
          return ctz_cotizacionDAL.Agregar(b);
      }

      public static String eliminar(ctz_cotizacionEntity b)
      {
          return ctz_cotizacionDAL.Delete(b);
      }

      public static string encontrar(ref ctz_cotizacionEntity u)
      {
          return ctz_cotizacionDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return ctz_cotizacionDAL.GetAll(sql_where);
      }


  }
}
