using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoprodiApp.Entities;
using SoprodiApp.DataAccess;
using System.Data;

namespace SoprodiApp.BusinessLayer
{
  public static class comisionagroinamaroBO
  {



      public static String registrar(comisionagroinamaroEntity b)
      {
          return comisionagroinamaroDAL.Insert(b);
      }

      public static String actualizar(comisionagroinamaroEntity b)
      {
          return comisionagroinamaroDAL.Update(b);
      }

      public static String agregar(comisionagroinamaroEntity b)
      {
          return comisionagroinamaroDAL.Agregar(b);
      }

      public static String eliminar(comisionagroinamaroEntity b)
      {
          return comisionagroinamaroDAL.Delete(b);
      }

      public static string encontrar(ref comisionagroinamaroEntity u)
      {
          return comisionagroinamaroDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return comisionagroinamaroDAL.GetAll(sql_where);
      }


  }
}
