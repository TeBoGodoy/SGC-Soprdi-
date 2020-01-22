using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoprodiApp.Entities;
using SoprodiApp.DataAccess;
using System.Data;

namespace SoprodiApp.BusinessLayer
{
  public static class sp_estados_internosBO
  {



      public static String registrar(sp_estados_internosEntity b)
      {
          return sp_estados_internosDAL.Insert(b);
      }

      public static String actualizar(sp_estados_internosEntity b)
      {
          return sp_estados_internosDAL.Update(b);
      }

      public static String agregar(sp_estados_internosEntity b)
      {
          return sp_estados_internosDAL.Agregar(b);
      }

      public static String eliminar(sp_estados_internosEntity b)
      {
          return sp_estados_internosDAL.Delete(b);
      }

      public static string encontrar(ref sp_estados_internosEntity u)
      {
          return sp_estados_internosDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return sp_estados_internosDAL.GetAll(sql_where);
      }


  }
}
