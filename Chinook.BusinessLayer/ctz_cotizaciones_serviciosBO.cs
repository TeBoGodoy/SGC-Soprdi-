using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Entities;
using CRM.DataAccess;
using System.Data;

namespace CRM.BusinessLayer
{
  public static class ctz_cotizaciones_serviciosBO
  {



      public static String registrar(ctz_cotizaciones_serviciosEntity b)
      {
          return ctz_cotizaciones_serviciosDAL.Insert(b);
      }

      public static String registrar_scope(ctz_cotizaciones_serviciosEntity b)
      {
          return ctz_cotizaciones_serviciosDAL.Insert_Scope(b);
      }

      public static String actualizar(ctz_cotizaciones_serviciosEntity b)
      {
          return ctz_cotizaciones_serviciosDAL.Update(b);
      }

      public static String agregar(ctz_cotizaciones_serviciosEntity b)
      {
          return ctz_cotizaciones_serviciosDAL.Agregar(b);
      }

      public static String eliminar(ctz_cotizaciones_serviciosEntity b)
      {
          return ctz_cotizaciones_serviciosDAL.Delete(b);
      }

      public static string encontrar(ref ctz_cotizaciones_serviciosEntity u)
      {
          return ctz_cotizaciones_serviciosDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return ctz_cotizaciones_serviciosDAL.GetAll(sql_where);
      }


  }
}
