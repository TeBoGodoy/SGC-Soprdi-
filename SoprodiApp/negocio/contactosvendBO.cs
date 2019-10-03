using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoprodiApp.Entities;
using SoprodiApp.DataAccess;
using System.Data;

namespace SoprodiApp.BusinessLayer
{
  public static class contactosvendBO
  {



      public static String registrar(contactosvendEntity b)
      {
          return contactosvendDAL.Insert(b);
      }

      public static String actualizar(contactosvendEntity b)
      {
          return contactosvendDAL.Update(b);
      }

      public static String agregar(contactosvendEntity b)
      {
          return contactosvendDAL.Agregar(b);
      }

      public static String eliminar(contactosvendEntity b)
      {
          return contactosvendDAL.Delete(b);
      }

      public static string encontrar(ref contactosvendEntity u)
      {
          return contactosvendDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return contactosvendDAL.GetAll(sql_where);
      }


  }
}
