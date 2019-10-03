using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoprodiApp.Entities;
using SoprodiApp.DataAccess;
using System.Data;

namespace SoprodiApp.BusinessLayer
{
  public static class comisionperiodocierreBO
  {



      public static String registrar(comisionperiodocierreEntity b)
      {
          return comisionperiodocierreDAL.Insert(b);
      }

      public static String actualizar(comisionperiodocierreEntity b)
      {
          return comisionperiodocierreDAL.Update(b);
      }

      public static String agregar(comisionperiodocierreEntity b)
      {
          return comisionperiodocierreDAL.Agregar(b);
      }

      public static String eliminar(comisionperiodocierreEntity b)
      {
          return comisionperiodocierreDAL.Delete(b);
      }

      public static string encontrar(ref comisionperiodocierreEntity u)
      {
          return comisionperiodocierreDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return comisionperiodocierreDAL.GetAll(sql_where);
      }


  }
}
