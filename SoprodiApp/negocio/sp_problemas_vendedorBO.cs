using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoprodiApp.Entities;
using SoprodiApp.DataAccess;
using System.Data;

namespace SoprodiApp.BusinessLayer
{
  public static class sp_problemas_vendedorBO
  {



      public static String registrar(sp_problemas_vendedorEntity b)
      {
          return sp_problemas_vendedorDAL.Insert(b);
      }

      public static String actualizar(sp_problemas_vendedorEntity b)
      {
          return sp_problemas_vendedorDAL.Update(b);
      }

      public static String agregar(sp_problemas_vendedorEntity b)
      {
          return sp_problemas_vendedorDAL.Agregar(b);
      }

      public static String eliminar(sp_problemas_vendedorEntity b)
      {
          return sp_problemas_vendedorDAL.Delete(b);
      }

      public static string encontrar(ref sp_problemas_vendedorEntity u)
      {
          return sp_problemas_vendedorDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return sp_problemas_vendedorDAL.GetAll(sql_where);
      }


  }
}
