using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoprodiApp.Entities;
using SoprodiApp.DataAccess;
using System.Data;

namespace SoprodiApp.BusinessLayer
{
  public static class sp_saldosBO
  {



      public static String registrar(sp_saldosEntity b)
      {
          return sp_saldosDAL.Insert(b);
      }

      public static String actualizar(sp_saldosEntity b)
      {
          return sp_saldosDAL.Update(b);
      }

      public static String agregar(sp_saldosEntity b)
      {
          return sp_saldosDAL.Agregar(b);
      }

      public static String eliminar(sp_saldosEntity b)
      {
          return sp_saldosDAL.Delete(b);
      }

      public static string encontrar(ref sp_saldosEntity u)
      {
          return sp_saldosDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return sp_saldosDAL.GetAll(sql_where);
      }


  }
}
