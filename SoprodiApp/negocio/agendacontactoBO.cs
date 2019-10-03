using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoprodiApp.Entities;
using SoprodiApp.DataAccess;
using System.Data;

namespace SoprodiApp.BusinessLayer
{
  public static class agendacontactoBO
  {



      public static String registrar(agendacontactoEntity b)
      {
          return agendacontactoDAL.Insert(b);
      }

      public static String actualizar(agendacontactoEntity b, DateTime fecha_antigua)
      {
          return agendacontactoDAL.Update(b, fecha_antigua);
      }

      public static String agregar(agendacontactoEntity b)
      {
          return agendacontactoDAL.Agregar(b);
      }

      public static String eliminar(agendacontactoEntity b)
      {
          return agendacontactoDAL.Delete(b);
      }

      public static string encontrar(ref agendacontactoEntity u)
      {
          return agendacontactoDAL.encontrar(ref u);
      }

      public static DataTable GetAll(string sql_where = "")
      {
          return agendacontactoDAL.GetAll(sql_where);
      }


  }
}
