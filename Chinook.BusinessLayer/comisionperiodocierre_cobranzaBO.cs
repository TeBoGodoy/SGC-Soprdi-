using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoprodiApp.Entities;
using SoprodiApp.DataAccess;
using System.Data;

namespace SoprodiApp.BusinessLayer
{
  public static class comisionperiodocierre_cobranzaBO
  {



      public static String registrar(comisionperiodocierre_cobranzaEntity b)
      {
          return comisionperiodocierre_cobranzaDAL.Insert(b);
      }

      public static String actualizar(comisionperiodocierre_cobranzaEntity b)
      {
          return comisionperiodocierre_cobranzaDAL.Update(b);
      }

      public static String agregar(comisionperiodocierre_cobranzaEntity b)
      {
          return comisionperiodocierre_cobranzaDAL.Agregar(b);
      }

      public static String eliminar(comisionperiodocierre_cobranzaEntity b)
      {
          return comisionperiodocierre_cobranzaDAL.Delete(b);
      }

      public static string encontrar(ref comisionperiodocierre_cobranzaEntity u)
      {
          return comisionperiodocierre_cobranzaDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return comisionperiodocierre_cobranzaDAL.GetAll(sql_where);
      }


  }
}
