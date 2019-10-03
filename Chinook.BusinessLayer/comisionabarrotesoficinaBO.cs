using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoprodiApp.Entities;
using SoprodiApp.DataAccess;
using System.Data;

namespace SoprodiApp.BusinessLayer
{
  public static class comisionabarrotesoficinaBO
  {



      public static String registrar(comisionabarrotesoficinaEntity b)
      {
          return comisionabarrotesoficinaDAL.Insert(b);
      }

      public static String actualizar(comisionabarrotesoficinaEntity b)
      {
          return comisionabarrotesoficinaDAL.Update(b);
      }

      public static String agregar(comisionabarrotesoficinaEntity b)
      {
          return comisionabarrotesoficinaDAL.Agregar(b);
      }

      public static String eliminar(comisionabarrotesoficinaEntity b)
      {
          return comisionabarrotesoficinaDAL.Delete(b);
      }

      public static string encontrar(ref comisionabarrotesoficinaEntity u)
      {
          return comisionabarrotesoficinaDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return comisionabarrotesoficinaDAL.GetAll(sql_where);
      }


  }
}
