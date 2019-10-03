using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thinxsys.Ejemplo.Generador
{
    public partial class F_Generador : Form
    {
        public F_Generador()
        {
            InitializeComponent();
        }

        private void B_Cargar_DAL_Click(object sender, EventArgs e)
        {
            SqlConnection conn = Conexion.ConnectionManager.getSqlConnection();
            conn.Open();
            encontrar(conn);
            conn.Close();
        }

        public bool encontrar(SqlConnection cnn)
        {
            String query = "SELECT distinct sys.tables.name, sys.tables.object_id, convert (bit,0,103) as Seleccionar ";
            query += "FROM sys.tables inner JOIN sys.all_columns ";
            query += "on sys.tables.object_id = sys.all_columns .object_id INNER JOIN ";
            query += "sys.index_columns on  sys.index_columns .column_id = sys.index_columns.column_id and ";
            query += "sys.index_columns.object_id = sys.tables .object_id ";
            query += "INNER JOIN sys.systypes on sys.systypes.xtype = sys.all_columns .system_type_id ";
            query += "where sys.tables.name <> 'sysdiagrams'";

            SqlCommand cmdSQL = new SqlCommand(query, cnn);

            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter adaptador = new SqlDataAdapter();
                adaptador.SelectCommand = cmdSQL;
                adaptador.Fill(dt);
                G_DB.DataSource = "";
                if (dt.Rows.Count > 0)
                {
                    G_DB.DataSource = dt;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en método encontrar \n\nDetalle error:" + ex.Message); ;
            }
        }

        private void B_Crear_DAL_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_DB.ShowDialog() == DialogResult.OK)
            {
                T_Ruta_DB.Text = folderBrowserDialog_DB.SelectedPath;
                crear_archivos_DAL();
            }
        }

        private void B_Crear_Entity_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_DB.ShowDialog() == DialogResult.OK)
            {
                T_Ruta_DB.Text = folderBrowserDialog_DB.SelectedPath;
                crear_archivos_entidad();
            }
        }

        private void B_Crear_RN_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_DB.ShowDialog() == DialogResult.OK)
            {
                T_Ruta_DB.Text = folderBrowserDialog_DB.SelectedPath;
                crear_archivos_RN();
            }
        }

        private void crear_archivos_DAL()
        {
            int i = 0;
            while (i < G_DB.RowCount)
            {
                if (bool.Parse(G_DB.Rows[i].Cells["Seleccionar"].Value.ToString()) == true)
                {
                    string newPath = T_Ruta_DB.Text;
                    string codigo = G_DB.Rows[i].Cells[1].Value.ToString();
                    string nombre = G_DB.Rows[i].Cells["name"].Value.ToString();
                    newPath = System.IO.Path.Combine(newPath, nombre.ToLower() + "DAL.cs");
                    System.IO.FileStream fs = System.IO.File.Create(newPath);
                    fs.Close();
                    DataTable t = new DataTable();
                    SqlConnection conn = Conexion.ConnectionManager.getSqlConnection();
                    conn.Open();
                    t = encontrar_columnas(conn, codigo);
                    conn.Close();
                    archiv_DAL(newPath, nombre.ToLower() + "DAL", t);
                }
                i = i + 1;
            }
        }

        private void crear_archivos_entidad()
        {
            int i = 0;
            while (i < G_DB.RowCount)
            {
                if (bool.Parse(G_DB.Rows[i].Cells["Seleccionar"].Value.ToString()) == true)
                {
                    string newPath = T_Ruta_DB.Text;
                    string codigo = G_DB.Rows[i].Cells[1].Value.ToString();
                    string nombre = G_DB.Rows[i].Cells["name"].Value.ToString();
                    newPath = System.IO.Path.Combine(newPath, nombre.ToLower() + "Entity.cs");
                    System.IO.FileStream fs = System.IO.File.Create(newPath);
                    fs.Close();
                    DataTable t = new DataTable();
                    SqlConnection conn = Conexion.ConnectionManager.getSqlConnection();
                    conn.Open();
                    t = encontrar_columnas(conn, codigo);
                    if (G_DB.Rows[i].Cells["name"].Value.ToString() == "MOVIMIENTOS")
                    {
                        MessageBox.Show("");
                    }
                    conn.Close();
                    archiv_Entity(newPath, nombre.ToLower() + "DAL", t);
                }
                i = i + 1;
            }
        }

        private void crear_archivos_RN()
        {
            int i = 0;
            while (i < G_DB.RowCount)
            {
                if (bool.Parse(G_DB.Rows[i].Cells["Seleccionar"].Value.ToString()) == true)
                {
                    string newPath = T_Ruta_DB.Text;
                    string codigo = G_DB.Rows[i].Cells[1].Value.ToString();
                    string nombre = G_DB.Rows[i].Cells["name"].Value.ToString();
                    newPath = System.IO.Path.Combine(newPath, nombre.ToLower() + "BO.cs");
                    System.IO.FileStream fs = System.IO.File.Create(newPath);
                    fs.Close();
                    DataTable t = new DataTable();
                    SqlConnection conn = Conexion.ConnectionManager.getSqlConnection();
                    conn.Open();
                    t = encontrar_columnas(conn, codigo);
                    conn.Close();
                    archiv_RN(newPath, nombre.ToLower() + "DAL", t);
                }
                i = i + 1;
            }
        }

        public DataTable encontrar_columnas(SqlConnection cnn, String tabla)
        {
            String query = "SELECT distinct sys.tables.name, sys.all_columns.name, ISNULL(sys.index_columns.index_id, 0) as index_id, sys.systypes.name, sys.all_columns.column_id, sys.all_columns.is_identity  ";
            query += "FROM sys.tables inner JOIN sys.all_columns ";
            query += "on sys.tables.object_id = sys.all_columns .object_id LEFT OUTER JOIN ";
            query += "sys.index_columns on  sys.index_columns.column_id = sys.all_columns.column_id and ";
            query += "sys.index_columns.object_id = sys.tables .object_id ";
            query += "INNER JOIN sys.systypes on sys.systypes.xusertype = sys.all_columns.system_type_id ";
            query += "where sys.tables.name <> 'sysdiagrams' and sys.tables.object_id = " + tabla;

            SqlCommand cmdSQL = new SqlCommand(query, cnn);

            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter adaptador = new SqlDataAdapter();
                adaptador.SelectCommand = cmdSQL;
                adaptador.Fill(dt);
                //G_DB.DataSource = "";
                if (dt.Rows.Count > 0)
                {
                    //G_DB.DataSource = dt;
                    return dt;
                }
                else
                {
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en método encontrar \n\nDetalle error:" + ex.Message); ;
            }
        }

        public void archiv_DAL(String Path, String tabla, DataTable T)
        {
            FileStream stream = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using System.Linq;");
            writer.WriteLine("using System.Text;");
            writer.WriteLine("using System.Data;");
            writer.WriteLine("using System.Data.SqlClient;");
            writer.WriteLine("using System.Configuration;");
            writer.WriteLine("using SoprodiApp.Entities;");
            writer.WriteLine("using System.Data;");
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("namespace SoprodiApp.DataAccess");
            writer.WriteLine("{");
            writer.WriteLine("");
            String clase = tabla.Replace("DAL", "");//nombre de la tabla
            writer.WriteLine("  public static class " + tabla);
            writer.WriteLine("  {");

            //LOAD
            writer.WriteLine("      private static " + clase + "Entity Load(IDataReader reader)");
            writer.WriteLine("      {");
            writer.WriteLine("          " + clase + "Entity " + clase + " = new " + clase + "Entity();");
            int i = 0;
            while (i < T.Rows.Count)
            {
                if (T.Rows[i][3].ToString() == "varchar" || T.Rows[i][3].ToString() == "text")
                {
                    writer.WriteLine("          " + clase + "." + T.Rows[i][1].ToString() + " = Convert.ToString(reader[\"" + T.Rows[i][1].ToString() + "\"]);");
                }
                if (T.Rows[i][3].ToString() == "bit")
                {
                    writer.WriteLine("          " + clase + "." + T.Rows[i][1].ToString() + " = bool.Parse(reader[\"" + T.Rows[i][1].ToString() + "\"].ToString());");
                }
                if (T.Rows[i][3].ToString() == "date" || T.Rows[i][3].ToString() == "datetime")
                {
                    writer.WriteLine("          " + clase + "." + T.Rows[i][1].ToString() + " = DateTime.Parse(reader[\"" + T.Rows[i][1].ToString() + "\"].ToString());");
                }
                if (T.Rows[i][3].ToString() == "int" || T.Rows[i][3].ToString() == "smallint" || T.Rows[i][3].ToString() == "bigint" || T.Rows[i][3].ToString() == "tinyint")
                {
                    writer.WriteLine("          " + clase + "." + T.Rows[i][1].ToString() + " = int.Parse(reader[\"" + T.Rows[i][1].ToString() + "\"].ToString());");
                }
                if (T.Rows[i][3].ToString() == "decimal")
                {
                    writer.WriteLine("          " + clase + "." + T.Rows[i][1].ToString() + " = int.Parse(reader[\"" + T.Rows[i][1].ToString() + "\"].ToString());");
                }
                if (T.Rows[i][3].ToString() == "numeric")
                {
                    writer.WriteLine("          " + clase + "." + T.Rows[i][1].ToString() + " = int.Parse(reader[\"" + T.Rows[i][1].ToString() + "\"].ToString());");
                }
                if (T.Rows[i][3].ToString() == "float")
                {
                    writer.WriteLine("          " + clase + "." + T.Rows[i][1].ToString() + " = float.Parse(reader[\"" + T.Rows[i][1].ToString() + "\"].ToString());");
                }
                i += 1;
            }
            writer.WriteLine("");
            writer.WriteLine("          return " + clase + ";");
            writer.WriteLine("      }");
            writer.WriteLine("");
            i = 0;
            string insert = "";
            string select = "";
            string where = "";
            string llaves = "";
            while (i < T.Rows.Count)
            {
                // SI ES AUTONUMERADO NO AGREGAR AL INSERT
                if (T.Rows[i]["is_identity"].ToString() != "True")
                {
                    if (select == "")
                    {
                        select = T.Rows[i][1].ToString();
                    }
                    else
                    {
                        select = select + ", " + T.Rows[i][1].ToString();
                    }
                }
                // SI ES AUTONUMERADO NO AGREGAR AL INSERT
                if (T.Rows[i]["is_identity"].ToString() != "True")
                {
                    if (insert == "")
                    {
                        insert = "@" + T.Rows[i][1].ToString();
                    }
                    else
                    {
                        insert = insert + ", @" + T.Rows[i][1].ToString();
                    }
                }

                if (T.Rows[i][2].ToString() == "1")
                {

                    if (where == "")
                    {
                        where = T.Rows[i][1].ToString() + " = @" + T.Rows[i][1].ToString();
                    }
                    else
                    {
                        where = where + " And " + T.Rows[i][1].ToString() + " = @" + T.Rows[i][1].ToString();
                    }

                    if (T.Rows[i][3].ToString() == "varchar")
                    {
                        if (llaves == "")
                        {
                            llaves = "string " + T.Rows[i][1].ToString();
                        }
                        else
                        {
                            llaves = llaves + ", string " + T.Rows[i][1].ToString();
                        }
                    }
                    if (T.Rows[i][3].ToString() == "date" || T.Rows[i][3].ToString() == "datetime")
                    {
                        if (llaves == "")
                        {
                            llaves = "DateTime " + T.Rows[i][1].ToString();
                        }
                        else
                        {
                            llaves = llaves + ", DateTime " + T.Rows[i][1].ToString();
                        }
                    }
                    if (T.Rows[i][3].ToString() == "bit")
                    {
                        if (llaves == "")
                        {
                            llaves = "bool " + T.Rows[i][1].ToString();
                        }
                        else
                        {
                            llaves = llaves + ", bool " + T.Rows[i][1].ToString();
                        }
                    }
                    if (T.Rows[i][3].ToString() == "numeric")
                    {
                        if (llaves == "")
                        {
                            llaves = "int " + T.Rows[i][1].ToString();
                        }
                        else
                        {
                            llaves = llaves + ", int " + T.Rows[i][1].ToString();
                        }
                    }
                    if (T.Rows[i][3].ToString() == "decimal" || T.Rows[i][3].ToString() == "int" || T.Rows[i][3].ToString() == "smallint")
                    {
                        if (llaves == "")
                        {
                            llaves = "int " + T.Rows[i][1].ToString();
                        }
                        else
                        {
                            llaves = llaves + ", int " + T.Rows[i][1].ToString();
                        }
                    }
                    if (T.Rows[i][3].ToString() == "float")
                    {
                        if (llaves == "")
                        {
                            llaves = "Double " + T.Rows[i][1].ToString();
                        }
                        else
                        {
                            llaves = llaves + ", Double " + T.Rows[i][1].ToString();
                        }
                    }
                }

                i += 1;
            }

            //LISTAR DATA TABLE
            writer.WriteLine("  public static DataTable GetAll(string sql_where)");
            writer.WriteLine("  {");
            writer.WriteLine("      DataTable dt = new DataTable();");
            writer.WriteLine("      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[\"default\"].ToString()))");
            writer.WriteLine("      {");
            writer.WriteLine("          conn.Open();");
            writer.WriteLine("          string sql = @\"SELECT *  from " + clase + " \"+ sql_where;");
            writer.WriteLine("          SqlCommand cmd = new SqlCommand(sql, conn);");
            writer.WriteLine("          SqlDataAdapter ap = new SqlDataAdapter(cmd);");
            writer.WriteLine("          ap.Fill(dt);");
            writer.WriteLine("      }");
            writer.WriteLine("      return dt;");
            writer.WriteLine("  }");
            writer.WriteLine("");

            //INSERT
            writer.WriteLine("  public static string Insert(" + clase + "Entity " + clase + ")");
            writer.WriteLine("  {");
            writer.WriteLine("      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[\"default\"].ToString()))");
            writer.WriteLine("      {");
            writer.WriteLine("          conn.Open();");
            writer.WriteLine("          string sql = @\"insert into " + clase + "(" + select + ") \"+");
            writer.WriteLine("          \"values (" + insert + ")\";");
            writer.WriteLine("          using (SqlCommand cmd = new SqlCommand(sql, conn))");
            writer.WriteLine("          {");
            i = 0;
            while (i < T.Rows.Count)
            {
                if (T.Rows[i]["is_identity"].ToString() != "True")
                {
                    writer.WriteLine("              cmd.Parameters.AddWithValue(\"@" + T.Rows[i][1].ToString() + "\", " + clase + "." + T.Rows[i][1].ToString() + ");");
                }

                i = i + 1;
            }

            writer.WriteLine("              try");
            writer.WriteLine("              {");
            writer.WriteLine("                  cmd.ExecuteNonQuery();");
            writer.WriteLine("                  return \"OK\";");
            writer.WriteLine("              }");
            writer.WriteLine("              catch (Exception EX)");
            writer.WriteLine("              {");
            writer.WriteLine("              return \"Error en " + clase + "DAL.Insert Detalle: \" + EX.Message;");
            writer.WriteLine("              }");
            writer.WriteLine("          }");
            writer.WriteLine("      }");
            writer.WriteLine("  }");
            writer.WriteLine("");

            //INSERT SCOPE IDENTITY : PV
            writer.WriteLine("  public static string Insert_Scope(" + clase + "Entity " + clase + ")");
            writer.WriteLine("  {");
            writer.WriteLine("      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[\"default\"].ToString()))");
            writer.WriteLine("      {");
            writer.WriteLine("          conn.Open();");
            writer.WriteLine("          string sql = @\"insert into " + clase + "(" + select + ") \"+");
            writer.WriteLine("          \"values (" + insert + ")\";");
            writer.WriteLine("          using (SqlCommand cmd = new SqlCommand(sql, conn))");
            writer.WriteLine("          {");
            i = 0;
            while (i < T.Rows.Count)
            {
                if (T.Rows[i]["is_identity"].ToString() != "True")
                {
                    writer.WriteLine("              cmd.Parameters.AddWithValue(\"@" + T.Rows[i][1].ToString() + "\", " + clase + "." + T.Rows[i][1].ToString() + ");");
                }

                i = i + 1;
            }

            writer.WriteLine("              try");
            writer.WriteLine("              {");
            writer.WriteLine("                  string respuesta = cmd.ExecuteScalar().ToString(); ");
            writer.WriteLine("                  return respuesta;");
            writer.WriteLine("              }");
            writer.WriteLine("              catch (Exception EX)");
            writer.WriteLine("              {");
            writer.WriteLine("              return \"Error en " + clase + "DAL.Insert Detalle: \" + EX.Message;");
            writer.WriteLine("              }");
            writer.WriteLine("          }");
            writer.WriteLine("      }");
            writer.WriteLine("  }");
            writer.WriteLine("");

            //UPDATE
            writer.WriteLine("  public static string Update(" + clase + "Entity " + clase + ")");
            writer.WriteLine("  {");
            writer.WriteLine("      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[\"default\"].ToString()))");
            writer.WriteLine("      {");
            writer.WriteLine("          conn.Open();");
            writer.WriteLine("          string sql = @\"update " + clase + " \" +");
            writer.WriteLine("          \"set \" +");
            i = 0;
            while (i < T.Rows.Count)
            {
                if (T.Rows[i]["is_identity"].ToString() != "True")
                {
                    if (i == T.Rows.Count - 1)
                    {
                        writer.WriteLine("          \"" + T.Rows[i][1].ToString() + " = @" + T.Rows[i][1].ToString() + "\" +");
                    }
                    else
                    {
                        writer.WriteLine("          \"" + T.Rows[i][1].ToString() + " = @" + T.Rows[i][1].ToString() + ", \" +");
                    }
                }

                i = i + 1;
            }
            string where2 = where;
            where2 = where.Replace("@", "@_");
            writer.WriteLine("          \" where " + where2 + " \";");
            writer.WriteLine("          using (SqlCommand cmd = new SqlCommand(sql, conn))");
            writer.WriteLine("              {");
            i = 0;
            while (i < T.Rows.Count)
            {
                if (T.Rows[i]["is_identity"].ToString() != "True")
                {
                    writer.WriteLine("              cmd.Parameters.AddWithValue(\"@" + T.Rows[i][1].ToString() + "\", " + clase + "." + T.Rows[i][1].ToString() + ");");
                }                    
                i = i + 1;
            }
            i = 0;
            while (i < T.Rows.Count)
            {
                if (T.Rows[i][2].ToString() == "1")
                {
                    writer.WriteLine("              cmd.Parameters.AddWithValue(\"@_" + T.Rows[i][1].ToString() + "\", " + clase + "." + T.Rows[i][1].ToString() + ");");
                }
                i = i + 1;
            }
            writer.WriteLine("              try");
            writer.WriteLine("              {");
            writer.WriteLine("                  cmd.ExecuteNonQuery();");
            writer.WriteLine("                  return \"OK\";");
            writer.WriteLine("              }");
            writer.WriteLine("              catch (Exception EX)");
            writer.WriteLine("              {");
            writer.WriteLine("                  return \"Error en " + clase + "DAL.Update Detalle: \" + EX.Message;");
            writer.WriteLine("              }");
            writer.WriteLine("          }");
            writer.WriteLine("      }");
            writer.WriteLine("  }");


            //AGREGAR (INSERT Y UPDATE)
            writer.WriteLine("  public static string Agregar(" + clase + "Entity u)");
            writer.WriteLine("  {");
            writer.WriteLine("      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[\"default\"].ToString()))");
            writer.WriteLine("      {");
            writer.WriteLine("          conn.Open();");
          
            writer.WriteLine("          string sql_encontrar = @\"SELECT count(1) from " + clase + " where " + where + "\"; ");
            writer.WriteLine("          SqlCommand cmd_encontrar = new SqlCommand(sql_encontrar, conn);");
            i = 0;
            while (i < T.Rows.Count)
            {
                if (T.Rows[i][2].ToString() == "1")
                {
                    writer.WriteLine("           cmd_encontrar.Parameters.AddWithValue(\"@" + T.Rows[i][1].ToString() + "\", u." + T.Rows[i][1].ToString() + ");");
                }
                i = i + 1;
            }
            writer.WriteLine("              try");
            writer.WriteLine("              {");
            writer.WriteLine("                  string respuesta = cmd_encontrar.ExecuteScalar().ToString(); ");
            writer.WriteLine("                  if (Convert.ToInt32(respuesta) > 0){ ");
            writer.WriteLine("                       string resp = Update(u); ");
            writer.WriteLine("                       if (resp == \"OK\"){return \"2\";}else { return \"3\"; } ");
            writer.WriteLine("                  }else{ ");
            writer.WriteLine("                       string resp = Insert(u);");
            writer.WriteLine("                       if (resp == \"OK\"){return \"1\";}else { return \"3\"; } ");
            writer.WriteLine("                  } ");
            writer.WriteLine("              }");
            writer.WriteLine("              catch (Exception EX)");
            writer.WriteLine("              {");
            writer.WriteLine("              return \"3\";");
            writer.WriteLine("              }");  
            writer.WriteLine("          }");           
            writer.WriteLine("  }");
            writer.WriteLine("");


            //DELETE
            writer.WriteLine("");
            writer.WriteLine("  public static string Delete(" + clase + "Entity " + clase + ")");
            writer.WriteLine("  {");
            writer.WriteLine("      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[\"default\"].ToString()))");
            writer.WriteLine("      {");
            writer.WriteLine("          conn.Open();");
            writer.WriteLine("          string sql = @\"delete from " + clase + " \" + ");
            writer.WriteLine("          \" where " + where + " \";");
            writer.WriteLine("          using (SqlCommand cmd = new SqlCommand(sql, conn))");
            writer.WriteLine("          { ");
            i = 0;
            while (i < T.Rows.Count)
            {
                if (T.Rows[i][2].ToString() == "1")
                {
                    writer.WriteLine("              cmd.Parameters.AddWithValue(\"@" + T.Rows[i][1].ToString() + "\", " + clase + "." + T.Rows[i][1].ToString() + ");");
                }
                i = i + 1;
            }

            writer.WriteLine("              try");
            writer.WriteLine("              {");
            writer.WriteLine("                  cmd.ExecuteNonQuery();");
            writer.WriteLine("                  return \"OK\";");
            writer.WriteLine("              }");
            writer.WriteLine("              catch (Exception EX)");
            writer.WriteLine("              {");
            writer.WriteLine("                  return \"Error en " + clase + "DAL.Delete Detalle: \" + EX.Message;");
            writer.WriteLine("              }");
            writer.WriteLine("          }");
            writer.WriteLine("      }");
            writer.WriteLine("  }");


            //ENCONTRAR, LO MISMO QUE VALIDAR
            writer.WriteLine("  public static string encontrar(ref " + clase + "Entity u)");
            writer.WriteLine("  {");
            writer.WriteLine("      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[\"default\"].ToString()))");
            writer.WriteLine("      {");
            writer.WriteLine("          conn.Open();");
            writer.WriteLine("          string sql = @\"SELECT * from " + clase + " where " + where + "\"; ");
            writer.WriteLine("          SqlCommand cmd = new SqlCommand(sql, conn);");

            i = 0;
            while (i < T.Rows.Count)
            {
                if (T.Rows[i][2].ToString() == "1")
                {
                    writer.WriteLine("           cmd.Parameters.AddWithValue(\"@" + T.Rows[i][1].ToString() + "\", u." + T.Rows[i][1].ToString() + ");");
                }
                i = i + 1;
            }
            writer.WriteLine("          try");
            writer.WriteLine("          {");
            writer.WriteLine("              SqlDataReader reader = cmd.ExecuteReader();");
            writer.WriteLine("              if (reader.Read())");
            writer.WriteLine("              {");
            writer.WriteLine("                  u = Load(reader);");
            writer.WriteLine("                  return \"OK\";");
            writer.WriteLine("              }");
            writer.WriteLine("          }");
            writer.WriteLine("          catch (Exception EX)");
            writer.WriteLine("          {");
            writer.WriteLine("              return \"Error en " + clase + "DAL.Validar Detalle: \" + EX.Message;");
            writer.WriteLine("          }");
            writer.WriteLine("      }");
            writer.WriteLine("      return \"No encontrado en DAL\";");
            writer.WriteLine("  }");
            writer.WriteLine("}");
            writer.WriteLine("}");
            writer.Close();
        }

        public void archiv_Entity(String Path, String tabla, DataTable T)
        {
            String clase = tabla.Replace("DAL", "");//nombre de la tabla
            FileStream stream = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using System.Linq;");
            writer.WriteLine("using System.Text;");
            writer.WriteLine("");
            writer.WriteLine("namespace SoprodiApp.Entities");
            writer.WriteLine("{");
            writer.WriteLine("public class " + clase + "Entity");
            writer.WriteLine("{");
            int i = 0;
            while (i < T.Rows.Count)
            {
                if (T.Rows[i][3].ToString() == "varchar" || T.Rows[i][3].ToString() == "text")
                {
                    writer.WriteLine("  public String " + T.Rows[i][1].ToString() + " { get; set; }");
                }
                if (T.Rows[i][3].ToString() == "bit")
                {
                    writer.WriteLine("  public bool  " + T.Rows[i][1].ToString() + " { get; set; }");
                }
                if (T.Rows[i][3].ToString() == "date" || T.Rows[i][3].ToString() == "datetime")
                {
                    writer.WriteLine("  public DateTime " + T.Rows[i][1].ToString() + " { get; set; }");
                }
                if (T.Rows[i][3].ToString() == "int" || T.Rows[i][3].ToString() == "smallint" || T.Rows[i][3].ToString() == "bigint" || T.Rows[i][3].ToString() == "tinyint")
                {
                    writer.WriteLine("  public int " + T.Rows[i][1].ToString() + " { get; set; }");
                }
                if (T.Rows[i][3].ToString() == "float" || T.Rows[i][3].ToString() == "decimal")
                {
                    writer.WriteLine("  public Double " + T.Rows[i][1].ToString() + " { get; set; }");
                }
                if (T.Rows[i][3].ToString() == "numeric")
                {
                    writer.WriteLine("  public int " + T.Rows[i][1].ToString() + " { get; set; }");
                }
                i += 1;
            }
            writer.WriteLine("}");
            writer.WriteLine("}");
            writer.Close();
        }

        public void archiv_RN(String Path, String tabla, DataTable T)
        {
            String clase = tabla.Replace("DAL", "");//nombre de la tabla
            FileStream stream = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using System.Linq;");
            writer.WriteLine("using System.Text;");
            writer.WriteLine("using SoprodiApp.Entities;");
            writer.WriteLine("using SoprodiApp.DataAccess;");
            writer.WriteLine("using System.Data;");
            writer.WriteLine("");
            writer.WriteLine("namespace SoprodiApp.BusinessLayer");
            writer.WriteLine("{");
            writer.WriteLine("  public static class " + clase + "BO");
            writer.WriteLine("  {");
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("      public static String registrar(" + clase + "Entity b)");
            writer.WriteLine("      {");
            writer.WriteLine("          return " + clase + "DAL.Insert(b);");
            writer.WriteLine("      }");
            writer.WriteLine("");
            writer.WriteLine("      public static String actualizar(" + clase + "Entity b)");
            writer.WriteLine("      {");
            writer.WriteLine("          return " + clase + "DAL.Update(b);");
            writer.WriteLine("      }");
            writer.WriteLine("");
            writer.WriteLine("      public static String agregar(" + clase + "Entity b)");
            writer.WriteLine("      {");
            writer.WriteLine("          return " + clase + "DAL.Agregar(b);");
            writer.WriteLine("      }");
            writer.WriteLine("");
            writer.WriteLine("      public static String eliminar(" + clase + "Entity b)");
            writer.WriteLine("      {");
            writer.WriteLine("          return " + clase + "DAL.Delete(b);");
            writer.WriteLine("      }");
            writer.WriteLine("");
            writer.WriteLine("      public static string encontrar(ref " + clase + "Entity u)");
            writer.WriteLine("      {");
            writer.WriteLine("          return " + clase + "DAL.encontrar(ref u);");
            writer.WriteLine("      }");
            writer.WriteLine("");
            writer.WriteLine("      public static DataTable GetAll(string sql_where = \"\")");
            writer.WriteLine("      {");
            writer.WriteLine("          return " + clase + "DAL.GetAll(sql_where);");
            writer.WriteLine("      }");
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine("  }");
            writer.WriteLine("}");
            writer.Close();
        }

        private void F_Generador_Load(object sender, EventArgs e)
        {

        }
    }
}
