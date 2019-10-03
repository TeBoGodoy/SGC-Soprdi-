using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoprodiApp.Entities;
using SoprodiApp.DataAccess;
using System.Data;

namespace SoprodiApp.BusinessLayer
{
  public static class comisionperiodocierre_productosBO
  {



      public static String registrar(comisionperiodocierre_productosEntity b)
      {
          return comisionperiodocierre_productosDAL.Insert(b);
      }

      public static String actualizar(comisionperiodocierre_productosEntity b)
      {
          return comisionperiodocierre_productosDAL.Update(b);
      }

      public static String agregar(comisionperiodocierre_productosEntity b)
      {
          return comisionperiodocierre_productosDAL.Agregar(b);
      }

      public static String eliminar(comisionperiodocierre_productosEntity b)
      {
          return comisionperiodocierre_productosDAL.Delete(b);
      }

      public static string encontrar(ref comisionperiodocierre_productosEntity u)
      {
          return comisionperiodocierre_productosDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return comisionperiodocierre_productosDAL.GetAll(sql_where);
      }

        public static string actualizar_sinporcentaje(comisionperiodocierre_productosEntity t)
        {
            return comisionperiodocierre_productosDAL.Update_sinporcentaje(t);
        }

        public static string actualizar_2(comisionperiodocierre_productosEntity t)
        {
            return comisionperiodocierre_productosDAL.Update2(t);
        }
    }
}
