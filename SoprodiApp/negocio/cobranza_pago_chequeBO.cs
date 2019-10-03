using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoprodiApp.Entities;
using SoprodiApp.DataAccess;
using System.Data;

namespace SoprodiApp.BusinessLayer
{
  public static class cobranza_pago_chequeBO
  {



      public static String registrar(cobranza_pago_chequeEntity b)
      {
          return cobranza_pago_chequeDAL.Insert(b);
      }

      public static String actualizar(cobranza_pago_chequeEntity b)
      {
          return cobranza_pago_chequeDAL.Update(b);
      }

      public static String agregar(cobranza_pago_chequeEntity b)
      {
          return cobranza_pago_chequeDAL.Agregar(b);
      }

      public static String eliminar(cobranza_pago_chequeEntity b)
      {
          return cobranza_pago_chequeDAL.Delete(b);
      }

      public static string encontrar(ref cobranza_pago_chequeEntity u)
      {
          return cobranza_pago_chequeDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return cobranza_pago_chequeDAL.GetAll(sql_where);
      }


  }
}
