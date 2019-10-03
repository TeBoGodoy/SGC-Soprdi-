using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoprodiApp.entidad;
using SoprodiApp.acceso;
using System.Data;

namespace SoprodiApp.negocio
{
  public static class usuarioBO
  {

      public static string validar(ref usuarioEntity u) 
      {
          return usuarioDAL.validar(ref u);
      }


      public static String registrar(usuarioEntity b)
      {
          return usuarioDAL.Insert(b);
      }

      public static String actualizar(usuarioEntity b, string usuario)
      {
          return usuarioDAL.Update(b, usuario);
      }

      public static String eliminar(usuarioEntity b)
      {
          return usuarioDAL.Delete(b);
      }

      public static string encontrar(ref usuarioEntity u)
      {
          return usuarioDAL.encontrar(ref u);
      }

      public static DataTable listar()
      {
          return usuarioDAL.GetAll();
      }

      public static string obtener_adm(string p)
      {
          return usuarioDAL.obtener_adm(p);
      }

      public static string registrar_det(string p1, string p2)
      {
          return usuarioDAL.registrar_det(p1, p2);
      }

      public static string eliminar_grupos_usuario(string p)
      {
          return usuarioDAL.eliminar_grupos_usuario(p);
      }

      public static string obtener_check(string p)
      {
          return usuarioDAL.obtener_check(p);
      }
  }
}
