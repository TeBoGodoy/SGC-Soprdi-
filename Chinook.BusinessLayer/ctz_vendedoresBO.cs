using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Entities;
using CRM.DataAccess;
using System.Data;

namespace CRM.BusinessLayer
{
  public static class ctz_vendedoresBO
  {



      public static String registrar(ctz_vendedoresEntity b)
      {
          return ctz_vendedoresDAL.Insert(b);
      }

      public static String registrar_scope(ctz_vendedoresEntity b)
      {
          return ctz_vendedoresDAL.Insert_Scope(b);
      }

      public static String actualizar(ctz_vendedoresEntity b)
      {
          return ctz_vendedoresDAL.Update(b);
      }

      public static String agregar(ctz_vendedoresEntity b)
      {
          return ctz_vendedoresDAL.Agregar(b);
      }

      public static String eliminar(ctz_vendedoresEntity b)
      {
          return ctz_vendedoresDAL.Delete(b);
      }

      public static string encontrar(ref ctz_vendedoresEntity u)
      {
          return ctz_vendedoresDAL.encontrar(ref u);
      }

        public static string Login(ref ctz_vendedoresEntity u)
        {
            return ctz_vendedoresDAL.Login(ref u);
        }

        public static DataTable GetAll(string sql_where = "")
      {
          return ctz_vendedoresDAL.GetAll(sql_where);
      }


  }
}
