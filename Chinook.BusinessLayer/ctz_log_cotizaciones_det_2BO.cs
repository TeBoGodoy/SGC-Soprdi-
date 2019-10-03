using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Entities;
using CRM.DataAccess;
using System.Data;

namespace CRM.BusinessLayer
{
  public static class ctz_log_cotizaciones_det_2BO
  {



      public static String registrar(ctz_log_cotizaciones_det_2Entity b)
      {
          return ctz_log_cotizaciones_det_2DAL.Insert(b);
      }

      public static String registrar_scope(ctz_log_cotizaciones_det_2Entity b)
      {
          return ctz_log_cotizaciones_det_2DAL.Insert_Scope(b);
      }

      public static String actualizar(ctz_log_cotizaciones_det_2Entity b)
      {
          return ctz_log_cotizaciones_det_2DAL.Update(b);
      }

      public static String agregar(ctz_log_cotizaciones_det_2Entity b)
      {
          return ctz_log_cotizaciones_det_2DAL.Agregar(b);
      }

      public static String eliminar(ctz_log_cotizaciones_det_2Entity b)
      {
          return ctz_log_cotizaciones_det_2DAL.Delete(b);
      }

      public static string encontrar(ref ctz_log_cotizaciones_det_2Entity u)
      {
          return ctz_log_cotizaciones_det_2DAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return ctz_log_cotizaciones_det_2DAL.GetAll(sql_where);
      }


  }
}
