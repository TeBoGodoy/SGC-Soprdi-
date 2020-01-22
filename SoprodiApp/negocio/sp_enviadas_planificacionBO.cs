using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoprodiApp.Entities;
using SoprodiApp.DataAccess;
using System.Data;

namespace SoprodiApp.BusinessLayer
{
  public static class sp_enviadas_planificacionBO
  {



      public static String registrar(sp_enviadas_planificacionEntity b)
      {
          return sp_enviadas_planificacionDAL.Insert(b);
      }

      public static String actualizar(sp_enviadas_planificacionEntity b)
      {
          return sp_enviadas_planificacionDAL.Update(b);
      }

      public static String agregar(sp_enviadas_planificacionEntity b)
      {
          return sp_enviadas_planificacionDAL.Agregar(b);
      }

      public static String eliminar(sp_enviadas_planificacionEntity b)
      {
          return sp_enviadas_planificacionDAL.Delete(b);
      }

      public static string encontrar(ref sp_enviadas_planificacionEntity u)
      {
          return sp_enviadas_planificacionDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return sp_enviadas_planificacionDAL.GetAll(sql_where);
      }


  }
}
