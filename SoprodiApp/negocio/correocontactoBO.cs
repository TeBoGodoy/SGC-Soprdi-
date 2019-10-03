using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoprodiApp.Entities;
using SoprodiApp.DataAccess;
using System.Data;

namespace SoprodiApp.BusinessLayer
{
  public static class correocontactoBO
  {



      public static String registrar(correocontactoEntity b)
      {
          return correocontactoDAL.Insert(b);
      }

      public static String actualizar(correocontactoEntity b)
      {
          return correocontactoDAL.Update(b);
      }

      public static String agregar(correocontactoEntity b)
      {
          return correocontactoDAL.Agregar(b);
      }

      public static String eliminar(correocontactoEntity b)
      {
          return correocontactoDAL.Delete(b);
      }

      public static string encontrar(ref correocontactoEntity u)
      {
          return correocontactoDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return correocontactoDAL.GetAll(sql_where);
      }


  }
}
