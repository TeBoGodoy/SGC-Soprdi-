using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SoprodiApp.entidad;
using System.Data;
using System.Reflection;
using System.Web;
using ThinxsysFramework;

namespace SoprodiApp.acceso
{
    public static class ReporteDB
    {
        public static DataTable _SourceTable = new DataTable();
        private static ReporteEntidad Load(IDataReader reader)
        {
            ReporteEntidad REPORT = new ReporteEntidad();
            REPORT.Vendedor = reader["Vendedor"].ToString();
            REPORT.Cliente = reader["Cliente"].ToString();
            REPORT.Producto = reader["Producto"].ToString();
            REPORT.Periodo = reader["Periodo"].ToString();
            return REPORT;
        }

        public static ReporteEntidad GetByPK()
        {
            ReporteEntidad webagente_det_correos_enviados = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT id_archivo, id_correo from webagente_det_correos_enviados where id_archivo = @id_archivo And id_correo = @id_correo ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                //cmd.Parameters.AddWithValue("@id_archivo", Convert.ToString(webagente_det_correos_enviados.id_archivo));
                //cmd.Parameters.AddWithValue("@id_correo", Convert.ToString(webagente_det_correos_enviados.id_correo));
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    webagente_det_correos_enviados = Load(reader);
                }
            }
            return webagente_det_correos_enviados;
        }

        public static List<ReporteEntidad> GetAll_List()
        {
            List<ReporteEntidad> list = new List<ReporteEntidad>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT id_archivo, id_correo from webagente_det_correos_enviados";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(Load(reader));
                }
            }
            return list;
        }

        internal static string trae_fecha_sp(string sp)
        {
            //; select scope_identity();
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT STUFF ((SELECT  CAST(', ' AS varchar(MAX)) + CONVERT(char(10), fechadespacho,126) " +
                            "FROM VPEDIDOCABECERA " +
                            "where coddocumento in (" + sp + ") GROUP BY fechadespacho " +
                            "ORDER BY fechadespacho FOR XML PATH('')), 1, 1, '') AS fechadespacho ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id_asignado", select_scope);



                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable usuarios_firman(string periodo)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"
                              select b.grupo,  c.firma, c.cod_usuario, 
		                            (select top 1 d.nombre_usuario from UsuarioWeb d where d.cod_usuario = c.cod_usuario) as nombre	               
	                            from ComisionUsuarioFirma b     
                                     left outer join V_comisionfirmas c on b.grupo = c.grupo 
                                       and c.codperiodo  = '" + periodo + "' group by b.grupo,  c.firma, c.cod_usuario ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable trae_grupo_firmas()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select grupo from ComisionUsuarioFirma group by grupo";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }


        internal static DataTable firmas_listas(string periodo)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select b.grupo,
		                            (select top 1 d.nombre_usuario from UsuarioWeb d where d.cod_usuario = c.cod_usuario) as nombre	               
	                            from ComisionUsuarioFirma b     
                                     left outer join V_comisionfirmas c on b.grupo = c.grupo 
                                       and c.codperiodo  = '"+periodo+"' and b.grupo <> 'FINANZAS' group by b.grupo,  c.firma, c.cod_usuario";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable traefirmas_periodo(string periodo)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from comisionfirmas where codperiodo = '" + periodo + "' ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string trae_cliente_sp(string sp)
        {
            //; select scope_identity();
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT STUFF ((SELECT  CAST(', ' AS varchar(MAX)) + CONVERT(varchar, nombrecliente) " +
                            "FROM VPEDIDOCABECERA " +
                            "where coddocumento in (" + sp + ") GROUP BY nombrecliente " +
                            "ORDER BY nombrecliente FOR XML PATH('')), 1, 1, '') AS nombrecliente ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id_asignado", select_scope);



                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable listar_transpor(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"
                                select  '('+a.cod_bodega+')' + 
						                                ISNULL(a.nombre_trans,'') + '-' + 
						                                ISNULL(b.patente, '') + '-' + 
						                                ISNULL(c.nombre_chofer, '') ,
						                                a.grupo
		                                from Transportista a 
			                                 LEFT OUTER JOIN
                                             Camion b ON a.cod_trans = b.cod_trans 
			                                 LEFT OUTER JOIN
                                             chofer c ON a.cod_trans = c.cod_trans
                                  " + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable listar_tipo_mov_stock(string v, string clase)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select distinct(trantype) from stock_diario ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable listar_bodegas_vm()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select distinct(codbodega), codbodega from [SoprodiVenta].[dbo].[vpedidocabecera] ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string trae_codigos_sincronizados_por_producto(string cod_prod)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil"].ToString()))
            {
                conn.Open();
                string sql = @"(SELECT STUFF
                               ((SELECT  CAST(',' AS varchar(MAX)) + CONVERT(varchar, X.CodDocumento)
                              FROM            VPEDIDODETALLE_THX_2 X
                             WHERE        X.CodProducto = '" + cod_prod + "' " +
                          "   ORDER BY X.CodDocumento FOR XML PATH('')), 1, 1, ''))";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        public static DataTable GetAll()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT id_archivo, id_correo  from webagente_det_correos_enviados";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable ventas_stock(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from V_REPORTE_THX where 1=1 " + v;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable trae_comisiones(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from V_Comisiones where 1=1 " + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable VM_listar_detalle_sp(string where3)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT * FROM   [NEW_THX].[dbo].[V_DETALLE_SP]   " + where3.Replace("b.", "");

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable trae_transportistas(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from transportista where 1=1 " + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable carga_categoria(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from categoria where 1=1 " + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable listar_ALL_productos_no_kg_(string v, string clase)
        {

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select distinct(codproducto), codproducto + CAST('-' AS varchar(MAX)) + descproducto as 'descr' from [SoprodiVenta].[dbo].[VPEDIDODETALLE] b where 1=1 " + clase + v;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string insert_futuro_stock(string cod_prod, string v, string cant)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into StockFuturo (cod_producto, fecha, cantidad) " +
                "values (@cod_producto, @fecha, @cantidad)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_producto", cod_prod);
                    cmd.Parameters.AddWithValue("@fecha", Convert.ToDateTime(v));
                    cmd.Parameters.AddWithValue("@cantidad", cant);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Insert stockFuturo: " + EX.Message;
                    }
                }
            }
        }

        internal static string eliminar_firma(string usuario_, string periodo, string firma, string grupo)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from [dbo].[comisionFirmas]  where cod_usuario = '" + usuario_.Trim() + "' and codperiodo = '" + periodo + "' and grupo = '" + grupo + "';" +
                   " insert into comisionFirmas values ('" + periodo + "', '" + usuario_ + "', " + firma + ", '" + grupo + "') ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        string sql2 = @"select count(*) from ComisionFirmas where codperiodo = '" + periodo + "'";
                        using (SqlCommand cmd2 = new SqlCommand(sql2, conn))
                        {
                            scalar = cmd2.ExecuteScalar().ToString();
                            if (grupo == "FINANZAS" && usuario_.ToUpper() == "JBRAVOG")
                            {
                                return "0";

                            }
                            return scalar;
                        }
                    }
                    catch (Exception EX)
                    {
                        //return "Error en delete: " + EX.Message;
                        return "ERROR";
                    }
                }
            }
        }

        internal static object trae_cliente_vendedor_SINVENTA(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select NAME, CRLMT, ADDR1, BILLCITY, SLSPERID, RUTCLIENTE, BILLPHONE, MIN(VENDEDOR) AS VENDEDOR, TIENE_AGENDA, ULTIMO_CONTACTO, COMUNA_STUFF
                                  from [dbo].[V_CLIENTES_SIN_VENTA] " + where + "group by NAME, CRLMT, ADDR1, BILLCITY, SLSPERID, RUTCLIENTE, BILLPHONE, TIENE_AGENDA, ULTIMO_CONTACTO, COMUNA_STUFF";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable ultimas_compra2(string id_prod, string compras)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from stock_compra_gen u where u.invtid = '" + id_prod + "' ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable stock_compra_cero(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"  Select *, (select name as Proveedor from [192.168.10.8].[SoprodiUSDapp].[dbo].vendor where vendid = b.vendid) as oc from stock_compra_gen b
         where b.invtid = '" + v + "' or b.invtid = '0" + v + "' ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;

        }

        public static string Insert(ReporteEntidad webagente_det_correos_enviados)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into webagente_det_correos_enviados(id_archivo, id_correo) " +
                "values (@id_archivo, @id_correo)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id_archivo", webagente_det_correos_enviados.id_archivo);
                    //cmd.Parameters.AddWithValue("@id_correo", webagente_det_correos_enviados.id_correo);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Insert Detalle: " + EX.Message;
                    }
                }
            }
        }

        internal static DataTable ajustes(string where, string fecha_thx)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select sum(qty), invtid from stock_diario where trantype = 'AJ' " + where + fecha_thx.Replace("fechafactura", "trandate") + " group by invtid ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable excel_valor(string cod, string fecha, string columna)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"  Select  top 20 " + columna + ", fecha from stock_excel_2 where cod_producto = '" + cod + "' and fecha <> convert(date, '" + fecha + "',103)   order by fecha desc";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;

        }

        internal static DataTable ultimas_compra3(string intvID, string compras)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from stock_compra_gen where " + intvID;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        public static string Update(ReporteEntidad webagente_det_correos_enviados)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"update webagente_det_correos_enviados " +
                "set " +
                "id_archivo = @id_archivo, " +
                "id_correo = @id_correo" +
                " where id_archivo = @_id_archivo And id_correo = @_id_correo ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id_archivo", webagente_det_correos_enviados.id_archivo);
                    //cmd.Parameters.AddWithValue("@id_correo", webagente_det_correos_enviados.id_correo);
                    //cmd.Parameters.AddWithValue("@_id_archivo", webagente_det_correos_enviados.id_archivo);
                    //cmd.Parameters.AddWithValue("@_id_correo", webagente_det_correos_enviados.id_correo);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Update Detalle: " + EX.Message;
                    }
                }
            }
        }

        internal static string trae_reglas(string codvendedor)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"
                            SELECT STUFF ((SELECT  CAST(', ' AS varchar(MAX)) + CONVERT(varchar, cod_comision) 
                                   FROM [V_COMISION_REGLAS] 
                                   where CodVendedor in ('" + codvendedor + "') GROUP BY cod_comision  " +
                                "   ORDER BY cod_comision FOR XML PATH('')), 1, 1, '') AS VENDEDOR";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static int prom_3_meses_atras(string meses_atras_3, string rutcliente, string codvend)
        {
            int scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select sum(facturac) from V_thx_v_reporte_resum where rutcliente = '" + rutcliente + "' and codvendedor = '" + codvend + "'   " +
                                        "    and convert(datetime, mes,103) >= convert(datetime, '" + meses_atras_3 + "',103)   ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }

        internal static DataTable fect_sin_comision(string periodo, string where1, string where2)
        {

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select b.*, '404 - no calcula comisión' as regla_2 from [v_comision_1_sin_comision] b " +
              "        WHERE b.periodo_2 = '" + periodo + "' " + where2;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;



        }

        internal static DataTable bodega_transportista(string where1)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT cod_bodega FROM Transportista " + where1 + " group by cod_bodega";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable historial_stock_diario(string where, string compras)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT  * from stock_diario where 1=1 and " + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        public static string Delete(ReporteEntidad webagente_det_correos_enviados)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from webagente_det_correos_enviados " +
                " where id_archivo = @id_archivo And id_correo = @id_correo ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id_archivo", webagente_det_correos_enviados.id_archivo);
                    //cmd.Parameters.AddWithValue("@id_correo", webagente_det_correos_enviados.id_correo);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Delete Detalle: " + EX.Message;
                    }
                }
            }
        }

        internal static DataTable trae_reglas_2(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from [dbo].[V_COMISION_REGLAS] " + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable listar_notas_credito(string where)
        {

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select rutcliente as RutCliente, nombrecliente as NombCliente, factura as 'Nº Factura', nombrevendedor as NombVendedor,descr as 'Descripción', 
                   monto_doc AS Monto,
                 CONVERT(VARCHAR,fecha_trans,103) as FechaTransc, CONVERT(VARCHAR,fecha_venc,103) as FechaVenc, tipo_credi as TipoCred
                  from v_cobranza_NC " + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable listar_transpor_solo_plani(string where2)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select nombre_transporte_todo, asignado_por from  v_SP_Asignados " +
                                  where2 +
                             "   group by  nombre_transporte_todo, asignado_por ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable listar_mov_stock(string where3)
        {

            DataTable dt = new DataTable();




            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select  t.invtid as Cod, t.descr as Producto,(select top 1 stkunit from Stock_diario where Stock_diario.invtid =  t.invtid ) as Unidad, t.siteid as Bodega, t.trandate as Fecha,  t.qty as Cantidad, t.batnbr as Lote, t.refnbr as Ref," +
                 "   CostoExcel = case   " +
                 "    when rtrim(ltrim(t.siteid)) = 'ZARATESOP' or rtrim(ltrim(t.siteid)) = 'SITRANS' or rtrim(ltrim(t.siteid)) = 'ABARROTES' " +
                 "    THEN(select top 1 j.bod_usd from stock_excel j where j.cod_producto = rtrim(ltrim(t.invtid)) and j.fecha = (select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.invtid))  and f.fecha <= convert(date, t.trandate, 103) ) group by j.fecha, j.bod_usd   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
                 "     when rtrim(ltrim(t.siteid)) = 'LOVALLEDOR' or rtrim(ltrim(t.siteid)) = 'AGUNSOP' or  rtrim(ltrim(t.siteid)) = 'LOGINSA'  or  rtrim(ltrim(t.siteid)) = 'LOGINSASOP'" +
                 "    THEN(select  top 1 j.cm_stgo from stock_excel j where j.cod_producto = rtrim(ltrim(t.invtid)) and j.fecha = (select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.invtid))  and f.fecha <= convert(date, t.trandate, 103) ) group by j.fecha, j.cm_stgo   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
                 "    when rtrim(ltrim(t.siteid)) = 'QUILLOTSOP' or rtrim(ltrim(t.siteid)) = 'LOCAL1'" +
                 "     THEN(select  top 1 j.cm_qta from stock_excel j where j.cod_producto = rtrim(ltrim(t.invtid)) and j.fecha = (select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.invtid))  and f.fecha <= convert(date, t.trandate, 103) ) group by j.fecha, j.cm_qta   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
                 "     when rtrim(ltrim(t.siteid)) = 'QUILLOTA1'" +
                 "     THEN(select  top 1 j.cm_qta from stock_excel j where j.cod_producto = rtrim(ltrim(t.invtid)) and j.fecha = (select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.invtid))  and f.fecha <= convert(date, t.trandate, 103) ) group by j.fecha, j.cm_qta   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
                 "    when rtrim(ltrim(t.siteid)) = 'ARGENTINA'" +
                 "    THEN(select  top 1 j.bod_usd from stock_excel j where j.cod_producto = rtrim(ltrim(t.invtid)) and j.fecha = (select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.invtid))  and f.fecha <= convert(date, t.trandate, 103) ) group by j.fecha, j.bod_usd   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
                 "     when rtrim(ltrim(t.siteid)) = 'ARICASOP'" +
                 "    THEN(select top 1  j.arica from stock_excel j where j.cod_producto = rtrim(ltrim(t.invtid)) and j.fecha = (select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.invtid))  and f.fecha <= convert(date, t.trandate, 103) ) group by j.fecha, j.arica   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
                 "      when rtrim(ltrim(t.siteid)) = 'ARICA1SOP'" +
                 "   THEN(select top 1  j.arica  from stock_excel j where j.cod_producto = rtrim(ltrim(t.invtid)) and j.fecha = (select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.invtid))  and f.fecha <= convert(date, t.trandate, 103) ) group by j.fecha, j.arica   having j.fecha = max(j.fecha) order by j.fecha asc  ) end" +
                 "  , '' as Costo   from [dbo].[v_intran_stock] t where 1=1 " + where3;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable cierre_camion(int existe_id_cierre)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select * from factura_sp_det where id = " + existe_id_cierre;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable trae_docu_calend_CERRADOS(string where, string usuario)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"select 
                                   [NºDoc]
                                  ,[TDoc]
                                  ,[NomClien]
                                  ,[FTrans]
                                  ,[FVenc]
                                  ,[FPago]
                                  ,[UltAcc]
                                  ,[DDía]
                                  ,[Monto(Peso)]
                                  ,[Monto(Dolar)]
                                  ,[Saldo_Peso]
                                  ,[Saldo_Dolar]
                                  ,[TCamb]
                                  ,[Descr]
                                  ,[TMoned]
                                  ,[NombVend]
                                  ,[T.Cr]
                                  ,[Seguimiento]
                                  ,[rutcliente]
                                  ,[id]
                                  ,[TPago]
                                  ,[accion]
                                  ,[obs]
                                  ,[Comisiones]
                                  ,[neto_peso]
                            from v_facturas_cobranza    where estado_doc <> 1 " + where + " order by rutcliente, [FVenc]  -- " + usuario;



                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable productos_para_permanencia(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select RTRIM(LTRIM(INVTID)) + ' - ' + descr AS Producto,'' as Permanencia from Stock_diario " + where + " group by invtid, descr";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable PERMANENCIA_NEGA(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select invtid, descr, trandate, Qty,'no' AS contado, '' as restando, '' as ultimo    from STOCK_DIARIO " + v + " AND Qty < 0 ORDER BY TRANDATE";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable PERMANENCIA2(string v, string desde, string where_arrastre, string clase)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select invtid, descr, trandate, sum(qty) as qty, FechaTermino, saldo, '' as 'Permanencia(Días)', StockALaFecha, stockbodega, porct from ( SElect invtid, descr, trandate, Qty, '' AS FechaTermino, '' as saldo, '' as 'Permanencia(Días)',  " +
                "(   " +
                    "            selecT  " +
                    "                    case when stkunit = 'KGR' then convert(varchar, SUM(G.QTY))   " +
                    "                    else convert(varchar, (select isnull(equivale_kilos, 0.00) from unidad_stock where cod_prod = invtid) * SUM(G.QTY)) end as Kilos from STOCK_DIARIO G where  " +
                    "                      G.trandate <= STOCK_DIARIO.trandate AND G.invtid = STOCK_DIARIO.invtid AND G.siteid = STOCK_DIARIO.siteid GROUP BY G.invtid, G.stkunit, G.descr  " +
                    "            )  " +
                    "            AS StockALaFecha,  " +
                     "           (  " +
                     "           select sum(kilos) from(  " +
                     "           selecT   " +
                     "                        case when H.stkunit = 'KGR' then  SUM(H.QTY)   " +
                     "                        else (select isnull(equivale_kilos, 0.00) from unidad_stock where cod_prod = H.invtid) * SUM(H.QTY) end as Kilos  " +
                     "               from STOCK_DIARIO H where H.trandate <= STOCK_DIARIO.trandate and H.siteid = STOCK_DIARIO.siteid and  " + clase +
                     "                GROUP BY H.invtid, H.stkunit, H.descr  ) tr )  AS StockBodega, '' as Porct  " +
                            " from STOCK_DIARIO " + v + " AND Qty > 0  " +
                              "  UNION " +
                             "    SELECT ''AS invtid, '' AS DESCR, CONVERT(DATETIME, '" + desde + "', 103) AS TRANDATE ,isnull( SUM (QTY),0) AS ARRASTRE, '' AS FechaTermino, '' as saldo, '' as 'Permanencia(Días)','' as StockALaFecha, 0 as StockBodega , '' as Porct  FRom STOCK_DIARIO " + where_arrastre + " " +
                              "  	 ) t group by invtid, trandate, descr, fechatermino, saldo, stockalafecha, stockbodega, porct ORDER BY TRANDATE, QTY";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }


        internal static DataTable PERMANENCIA(string v, string desde, string where_arrastre, string clase)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select invtid, descr, trandate, sum(qty) as qty, FechaTermino, saldo, '' as 'Permanencia(Días)', StockALaFecha from ( " +
 " SElect invtid, descr, trandate, Qty, '' AS FechaTermino, '' as saldo, '' as 'Permanencia(Días)', '' as StockALaFecha  from STOCK_DIARIO " + v + " AND Qty > 0  " +
                                  "  UNION " +
                                 "    SELECT ''AS invtid, '' AS DESCR, CONVERT(DATETIME, '" + desde + "', 103) AS TRANDATE ,isnull( SUM (QTY),0) AS ARRASTRE, '' AS FechaTermino, '' as saldo, '' as 'Permanencia(Días)','' as StockALaFecha  FRom STOCK_DIARIO " + where_arrastre + " " +
                                  "  ) t group by invtid, trandate, descr, fechatermino, saldo, stockalafecha ORDER BY TRANDATE, QTY";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable productos_entradas(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select QTY as q, PRODUCTO as p, TRANDATE as t , siteid as s, invtid as i, QTY as queda, *, '' as Porcentaje_Despachado from [V_Vencimiento_final]  where 1=1 " + where + " ORDER BY PRODUCTO, TRANDATE, siteid ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static double trae_equivale_kg(string invtid)
        {
            double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select isnull(equivale_kilos,0.00) from unidad_stock where cod_prod = '" + invtid + "' ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                try
                {
                    scalar = Double.Parse((cmd.ExecuteScalar().ToString()));
                }
                catch
                {


                }
                conn.Close();
            }
            return scalar;
        }

        internal static DataTable listar_transpor_2(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT [cod_trans]
                              ,[nombre_trans]
                              ,[rut]
                              ,[fono]
                              ,[direccion]
                              ,[carga_inicial]
                              ,[cod_bodega]
                              ,[grupo]
                          FROM[NEW_THX].[dbo].[Transportista]" + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string neto_navarro(string periodo)
        {
            //; select scope_identity();
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select neto_pesos 
                                from Comision_THX 
                                where regla = 'regla21' and codvendedor = 'AN044' and periodo = '" + periodo + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id_asignado", select_scope);



                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable lista_vencimientos(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select top 10 * from v_vencimientos ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string nombre_chofer(string cod_chofer)
        {
            //; select scope_identity();
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select [nombre_chofer] from[NEW_THX].[dbo].[chofer] " + cod_chofer + "";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id_asignado", select_scope);



                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string nombre_camion(string cod_camion)
        {
            //; select scope_identity();
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select [patente] from[NEW_THX].[dbo].[camion]  " + cod_camion + "";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id_asignado", select_scope);



                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable salidas_producto(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select QTY, InvtID, TRANDATE , siteid, '' as restada from  V_INTRAN_STOCK_SAL   where 1=1 " + where + " and qty < 0 ORDER BY TRANDATE, siteid"
                                   ;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable compras_gen(string compra)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select InvtID,  case 
                                when RcptUnitDescr = 'TON' and trantype ='X' then  
                                RcptQty*1000*-1
                                when RcptUnitDescr <> 'TON' and trantype ='X' then  
                               RcptQty*-1
                              when RcptUnitDescr = 'TON' and trantype <> 'X' then  
                                RcptQty*1000
							 when RcptUnitDescr <> 'TON' and trantype <>'X' then 
							 RcptQty
                                 
                                 end as RcptQty, SiteID from Stock_Compra_gen where " + compra + " group by  InvtID, RcptQty, RcptDate, SiteID, RcptUnitDescr,trantype ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable PLANI_CAMIONES(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select nombre_trans, cod_trans from Transportista " + v;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string trae_correo(int id_correo)
        {
            string correo = "";
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "select cc from Lista_Correos where id = " + id_correo;

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            foreach (DataRow r in dt.Rows)
            {

                correo = r[0].ToString();

            }
            return correo;

        }

        internal static DataTable periodos_productos(string periodo)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from V_TOTAL_PRODUCTO_PERIODO " + periodo;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable sincronizadas_detalle(string cod_estado, string productos, string hasta, string bodegas)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil"].ToString()))
            {
                conn.Open();

                //string bodegas = "";
                //string sql = @"select CodProducto, sum(cantidad) from (
                //                select u.CodProducto, sum(u.cantidad_falta) as cantidad from ( " +
                //            "   select * from[NEW_THX].[dbo].[sps_procesadas1] where  1=1 " + productos + " and estado <> '20'   and (select estado from[NEW_THX].[dbo].[Estado_SP] f where f.sp =[NEW_THX].[dbo].[sps_procesadas1].CodDocumento) is null " +
                //     "                   ) u group by u.CodProducto union all  " +
                //      "          select  A.CodProducto, sum(A.cantidad) as cantidad from  " +
                //      "       (select b.codproducto, b.cantidad from VPEDIDODETALLE_THX b   " +
                //      "  left join THX_Sp_Aprobadas c on b.CodDocumento = c.coddocumento where c.coddocumento IS NULL and b.CodEstadoDocumento in ('10') and (select estado from[NEW_THX].[dbo].[Estado_SP] f where f.sp = b.CodDocumento) is null and  ISNUMERIC(b.CodProducto) = 1) A inner join   " +
                //      "         (select invtid from[NEW_THX].[dbo].[Stock_diario] " +
                //      "     where 1= 1  " + cod_estado + " group by invtid ) B on A.CodProducto = B.invtid group by A.CodProducto) f group by f.CodProducto";


                string sql = @"
                                select u.CodProducto, sum(u.cantidad_falta) as cantidad from (
                                 select A.* from[NEW_THX].[dbo].[sps_procesadas1] A
			                     inner join   
                                   (select invtid from[NEW_THX].[dbo].[Stock_diario] where 1=1 " + cod_estado + " group by invtid ) B " +
                        "          on A.CodProducto = B.invtid" +
                        "         where  1=1 " + productos + bodegas + " and estado <> '20'   " +
                         "        and (select estado from[NEW_THX].[dbo].[Estado_SP] f where f.sp = A.CodDocumento) is null " +
                         "    AND (select f.fechaemision from[NEW_THX].[dbo].[VPEDIDOCABECERA_NEW_THX] f where f.CodDocumento = A.CodDocumento) <= " +
                         "        convert(datetime, '" + hasta + "',103)" + bodegas +
                         "        ) u group by u.CodProducto";


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable VM_listar_detalle_sp_2(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                //              string sql = "select  CodDocumento, NombreCliente, NombreVendedor, MontoNeto, DescBodega, convert(varchar, FechaDespacho,103) as FechaDespacho, DATEDIFF(DAY, CONVERT(datetime,GETDATE(),103), FechaDespacho) as DifDias " + 
                //"  , convert(varchar, FechaEmision, 103) as FechaEmision , CodVendedor, NotaLibre, CodBodega , CodMoneda, DescEstadoDocumento, Facturas,  GxEstadoSync,  " + 
                //"     convert(varchar, GxActualizado, 103) as GxActualizado , GxEnviadoERP, convert(varchar, FechaCreacion, 103) as FechaCreacion ,  " + 
                //"     ValorTipoCambio,LimiteSeguro, TipoCredito, CreditoDisponible, CreditoAutorizado, EmailVendedor from VPEDIDOCABECERA  " + where3;

                string sql = "SELECT * FROM VPEDIDODETALLE_new_thx " + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static double _cltes_nuevos_12_mes_factu(string periodo, string where)
        {
            double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                double periodo1 = Convert.ToDouble(periodo.Substring(0, 4));
                string periodo_menos12 = periodo.Replace(periodo.Substring(0, 4), (periodo1 - 1).ToString());

                string sql = @" (select  isnull(sum(neto_pesos),0) from thx_v_reporte a left join  " +
                              "   (select distinct(rutcliente) from thx_v_reporte " + where + " and Periodo >= " + periodo_menos12 + " and periodo < " + periodo + ") b on   " +
                              "    a.rutcliente = b.rutcliente  " +
                              "    " + where.Replace("rutcliente in", "a.rutcliente in") + " and Periodo in (" + periodo + ") and b.rutcliente is null)";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Convert.ToInt64(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }

        internal static double tiene_regla_abarrote(object p)
        {
            int scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select count(*) from[dbo].[V_COMISION_VENDEDORES] where codvendedor = '" + p + "' and id_comision = 16";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }

        internal static double _cltes_nuevos_12_mes(string periodo, string where)
        {
            int scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                double periodo1 = Convert.ToDouble(periodo.Substring(0, 4));
                string periodo_menos12 = periodo.Replace(periodo.Substring(0, 4), (periodo1 - 1).ToString());

                string sql = @"select count(*) as clientes_con_venta from " +
                             "   (select count(*) AS RowNumber from thx_v_reporte a left join " +
                             "   (select distinct(rutcliente) from thx_v_reporte " + where + " and Periodo >= " + periodo_menos12 + " and Periodo < " + periodo + ") b on " +
                             "   a.rutcliente = b.rutcliente" +
                             "  " + where.Replace("rutcliente in", "a.rutcliente in") + " and Periodo in (" + periodo + ") and b.rutcliente is null" +
                             "     group by a.rutcliente, a.periodo) a";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }

        internal static string update_vencimiento(string id, string texto_, string sw_)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                //@"update SP_Asignados set fecha_despacho = Convert(datetime, @fecha_despacho,103), cod_trans = @cod_tr where id = @id";
                string sql = @"update [dbo].[Vencimientos] set " + sw_ + " = @" + sw_ + " where id = @id ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.Parameters.AddWithValue("@" + sw_ + "", texto_);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Insert: " + EX.Message;
                    }
                }
            }
        }

        internal static DataTable trae_correos_hist(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from v_correos_a_contactos " + where + " order by fecha asc";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string insert_vencimiento(string invtid, string siteid, string batnbr, string refnbr, string trandate, string trantype, string texto_, string sw_)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into [dbo].[Vencimientos] (invtid, siteid, batnbr, refnbr, trandate, trantype , " + sw_ + ") " +
                "values (@invtid, @siteid, @batnbr, @refnbr, @trandate, @trantype , @" + sw_ + ")";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@invtid", invtid);
                    cmd.Parameters.AddWithValue("@siteid", siteid);
                    cmd.Parameters.AddWithValue("@batnbr", batnbr);
                    cmd.Parameters.AddWithValue("@refnbr", refnbr);
                    cmd.Parameters.AddWithValue("@trandate", trandate);
                    cmd.Parameters.AddWithValue("@trantype", trantype);
                    cmd.Parameters.AddWithValue("@" + sw_ + "", texto_);


                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Insert: " + EX.Message;
                    }
                }
            }
        }

        internal static string buscar_vencimiento(string invtid, string siteid, string batnbr, string refnbr, string trandate, string trantype)
        {
            //; select scope_identity();
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select id from [dbo].[Vencimientos] where invtid = @invtid and  siteid = @siteid and  " +
                     "batnbr = @batnbr and refnbr = @refnbr and trandate = @trandate and trantype = @trantype";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@invtid", invtid);
                    cmd.Parameters.AddWithValue("@siteid", siteid);
                    cmd.Parameters.AddWithValue("@batnbr", batnbr);
                    cmd.Parameters.AddWithValue("@refnbr", refnbr);
                    cmd.Parameters.AddWithValue("@trandate", trandate);
                    cmd.Parameters.AddWithValue("@trantype", trantype);
                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }


        internal static DataTable actualizar_saldos(string nombre_sp, List<SPVars> toSP)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                SqlCommand sqlComm = new SqlCommand(nombre_sp, conn);
                if (toSP != null)
                {
                    foreach (SPVars ob in toSP)
                    {
                        sqlComm.Parameters.AddWithValue("@" + ob.nombre, ob.valor);
                    }
                }

                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(dt);
                return dt;
            }
        }


        internal static DataTable lista_enc_sp_asignada_in(string ids, string ids_asignados)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "select a.*, b.observacion FROM [VPEDIDOCABECERA_NEW_THX] a left outer join v_sp_asignados b on a.CodDocumento = b.codDocumento and b.ID in (" + ids_asignados + ")" +
                             "        where  a.coddocumento in (" + ids + ") order by  a.coddocumento asc";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string insert_X2POSSL(string facturas_comas)
        {

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["solomon_test"].ToString()))
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"INSERT INTO X2POSSL
                                select *, null AS timestampfalso from [192.168.10.45].[NEW_THX].[dbo].[V_ENLACE_COBRANZA_F_3_X2POSSL] where [Invcnbr] in (  select val from [dbo].[f_split_THX](@FACTURAS, ',')   )";

                //string sql = @"INSERT INTO X2POSSL
                //                 select *, null AS timestampfalso  from [V_ENLACE_COBRANZA_F_3_X2POSSL] where [Invcnbr] in (  select val from [dbo].[f_split](@FACTURAS, ',')   )";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FACTURAS", facturas_comas);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en insert ERP: " + EX.Message;
                    }
                }
            }
        }
        internal static string insert_X2POSSL_CHEQUE(string facturas_comas)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["solomon_test"].ToString()))
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"INSERT INTO X2POSSL
                             select *, null AS timestampfalso from [192.168.10.45].[NEW_THX].[dbo].[V_ENLACE_COBRANZA_F_CHEQUES_2_X2POSSL] where [Invcnbr] in (  select val from [dbo].[f_split_THX](@FACTURAS, ',')   )";

                //string sql = @"INSERT INTO X2POSSL
                //                select *, null AS timestampfalso from V_ENLACE_COBRANZA_F_CHEQUES_2_X2POSSL where [Invcnbr] in (  select val from [dbo].[f_split](@FACTURAS, ',')   )";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FACTURAS", facturas_comas);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en insert ERP: " + EX.Message;
                    }
                }
            }
        }


        internal static string insert_X2POSTROK(string facturas_comas)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["solomon_test"].ToString()))
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"INSERT INTO X2POSTROK
                                 select *, null AS timestampfalso  from [192.168.10.45].[NEW_THX].[dbo].[V_ENLACE_COBRANZA_F_2_1_X2POSTROK] where [Invcnbr] in (  select val from [dbo].[f_split_THX](@FACTURAS, ',')   )";

                //string sql = @"INSERT INTO X2POSTROK
                //                 select *, null AS timestampfalso  from [V_ENLACE_COBRANZA_F_2_1_X2POSTROK] where [Invcnbr] in (  select val from [dbo].[f_split](@FACTURAS, ',')   )";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FACTURAS", facturas_comas);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en insert ERP: " + EX.Message;
                    }
                }
            }
        }

        internal static string insert_X2POSTROK_CHEQUE(string facturas_comas)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["solomon_test"].ToString()))
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"INSERT INTO X2POSTROK
                                 select *, null AS timestampfalso  from [192.168.10.45].[NEW_THX].[dbo].[V_ENLACE_COBRANZA_F_CHEQUES_2_1_X2POSTROK] where [Invcnbr] in (  select val from [dbo].[f_split_THX](@FACTURAS, ',')   )";

                //string sql = @"INSERT INTO X2POSTROK
                //                 select *, null AS timestampfalso  from [V_ENLACE_COBRANZA_F_CHEQUES_2_1_X2POSTROK] where [Invcnbr] in (  select val from [dbo].[f_split](@FACTURAS, ',')   )";




                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FACTURAS", facturas_comas);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en insert ERP: " + EX.Message;
                    }
                }
            }
        }

        internal static string precalcular_regla_11_amaro(string periodo)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete FROM comision_thx  where periodo = '" + periodo + "' and regla = 'regla11';" +
                              "insert into comision_thx  " +
                             " SELECT * FROM [dbo].[V_PARTICIPACION_AMARO_2] where periodo = '" + periodo + "' ;" +
                             " delete  from [dbo].[ComisionPeriodoCierre_Productos]  where cod_regla = 'regla11' and cod_periodo = '" + periodo + "' ; " +
                             " insert into [dbo].[ComisionPeriodoCierre_Productos]" +
                             "SELECT  " +
                             "  [periodo] " +
                             " ,[codvendedor] " +
                             " ,[númfactura] " +
                             " ,[producto] " +
                             " ,[porcentaje] " +
                             " ,[regla] " +
                             " ,null " +
                             " ,null " +
                         "      FROM [dbo].[V_PARTICIPACION_AMARO_2] where periodo = '" + periodo + "' ; ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en delete/insert: " + EX.Message;
                    }
                }
            }
        }

        internal static string trae_correos_bodega_2(string bodegas_sp, string grupo)
        {
            //; select scope_identity();
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT STUFF ((SELECT  CAST(', ' AS varchar(MAX)) + CONVERT(varchar(max), correos) " +
                            "FROM correo_bodega " +
                            "where cod_bodega in (" + bodegas_sp + ")  and grupo in  (" + grupo + ") GROUP BY correos " +
                            "ORDER BY correos FOR XML PATH('')), 1, 1, '') AS correos ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id_asignado", select_scope);
                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string bodegas_sp(string sps)
        {
            //; select scope_identity();
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT STUFF ((SELECT  CAST(', ' AS varchar(MAX)) + CONVERT(varchar, CodBodega) " +
                            "FROM VPEDIDOCABECERA " +
                            "where coddocumento in (" + sps + ") GROUP BY CodBodega " +
                            "ORDER BY CodBodega FOR XML PATH('')), 1, 1, '') AS CodBodega ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id_asignado", select_scope);
                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable lista_det_sp_asignada_in(string ids)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "select a.* FROM V_DET_Sp_Asignados a where  a.ID_ASIGNADO in (" + ids + ") order by  convert(numeric(18,0), a.orden_cargar) asc, a.coddocumento asc";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string recalcular_regla_21_toro(string periodo)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete FROM comision_thx  where periodo = '" + periodo + "' and regla = 'regla21';" +

                              "insert into comision_thx  " +
                             " SELECT * FROM [V_PARTICIPACION_TORO] where periodo = '" + periodo + "' ;" +
                             " delete  from [dbo].[ComisionPeriodoCierre_Productos]  where cod_regla = 'regla21' and cod_periodo = '" + periodo + "' ; " +
                             " insert into [dbo].[ComisionPeriodoCierre_Productos]" +
                             "SELECT  " +
                             "  [periodo] " +
                             " ,[codvendedor] " +
                             " ,[númfactura] " +
                             " ,[producto] " +
                             " ,[porcentaje] " +
                             " ,[regla] " +
                             " ,null " +
                             " ,null " +
                         "      FROM[V_PARTICIPACION_TORO] where periodo = '" + periodo + "' ; ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en delete/insert: " + EX.Message;
                    }
                }
            }
        }

        internal static DataTable trae_docu_calend_cheques(string where, string user)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT *, 
                                CAST('(' AS varchar(MAX)) + dbo.F_Separador_miles(convert(numeric, SUBSTRING(rtrim(rutcliente), 1, Len(rtrim(rutcliente)) - 1))) + CAST('-' AS varchar(MAX)) + SUBSTRING(rtrim(rutcliente), 
                                    Len(rtrim(rutcliente)), Len(rtrim(rutcliente))) +  CAST(') ' AS varchar(MAX)) + nombrecliente 
                                as NomClien ,
                                (select  MAX(FECHA_PAGO) from cobranza_pago_cheque
                                 where cobranza_pago_cheque.num_cheque = V_COBRANZA_TODOS_docs.factura ) 
                                as TIENE_PAGO
                              FROM V_COBRANZA_TODOS_docs " + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static int validar_factura_solomon(string factura)
        {
            int scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["solomon"].ToString()))
            {
                conn.Open();
                string sql = @"select count(númfactura) from v_reporteF where númfactura = '" + factura + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    try
                    {
                        scalar = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception EX)
                    {
                        return 0;
                    }
                }
            }
            return scalar;
        }

        internal static string insert_det_cierre_camion(int existe_id_cierre, string factura)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into [dbo].[FACTURA_SP_det]
                                                       ([id]
                                                       ,[factura]
                                                       ,[estado]) " +
                                          "values (@id, @factura, @estado)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", existe_id_cierre);
                    cmd.Parameters.AddWithValue("@factura", factura);
                    cmd.Parameters.AddWithValue("@estado", "OK");


                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Insert: " + EX.Message;
                    }
                }
            }
        }

        internal static int existe_cierre_camion(string cod_trans, string cod_camion, string cod_chofer, string sps, string dia_plani)
        {


            if (cod_camion == ",0")
            {
                cod_camion = "";
            }

            if (cod_chofer == ",0")
            {
                cod_chofer = "";
            }


            int scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select id from FACTURA_SP_enc where cod_trans = @cod_trans and cod_camion = @cod_camion and cod_chofer = @cod_chofer and dia_planif = @dia_planif and sps = @sps";


                string sql2 = @"insert into FACTURA_SP_enc (cod_trans, cod_camion, cod_chofer,dia_planif,sps ) values "
                                    + " (@cod_trans, @cod_camion, @cod_chofer, @dia_planif, @sps); select scope_identity();";


                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_trans", cod_trans);
                    cmd.Parameters.AddWithValue("@cod_camion", cod_camion);
                    cmd.Parameters.AddWithValue("@cod_chofer", cod_chofer);
                    cmd.Parameters.AddWithValue("@dia_planif", Convert.ToDateTime(dia_plani));
                    cmd.Parameters.AddWithValue("@sps", sps);

                    try
                    {
                        scalar = Convert.ToInt32(cmd.ExecuteScalar());
                        if (scalar == 0)
                        {
                            using (SqlCommand cmd1 = new SqlCommand(sql2, conn))
                            {
                                cmd1.Parameters.AddWithValue("@cod_trans", cod_trans);
                                cmd1.Parameters.AddWithValue("@cod_camion", cod_camion);
                                cmd1.Parameters.AddWithValue("@cod_chofer", cod_chofer);
                                cmd1.Parameters.AddWithValue("@dia_planif", Convert.ToDateTime(dia_plani));
                                cmd1.Parameters.AddWithValue("@sps", sps);

                                try
                                {
                                    scalar = Convert.ToInt32(cmd1.ExecuteScalar());
                                }
                                catch (Exception EX2)
                                {
                                    return 0;
                                }
                            }
                        }
                    }
                    catch (Exception EX)
                    {

                        using (SqlCommand cmd1 = new SqlCommand(sql2, conn))
                        {
                            cmd1.Parameters.AddWithValue("@cod_trans", cod_trans);
                            cmd1.Parameters.AddWithValue("@cod_camion", cod_camion);
                            cmd1.Parameters.AddWithValue("@cod_chofer", cod_chofer);
                            cmd1.Parameters.AddWithValue("@dia_planif", Convert.ToDateTime(dia_plani));
                            cmd1.Parameters.AddWithValue("@sps", sps);

                            try
                            {
                                scalar = Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            catch (Exception EX2)
                            {
                                return 0;
                            }
                        }
                        //return 0;
                    }
                }
            }
            return scalar;
        }


        internal static int existe_cierre_camion_Select(string cod_trans, string cod_camion, string cod_chofer, string sps, string dia_plani)
        {


            if (cod_camion == ",0")
            {
                cod_camion = "";
            }

            if (cod_chofer == ",0")
            {
                cod_chofer = "";
            }


            int scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select id from FACTURA_SP_enc where cod_trans = @cod_trans and cod_camion = @cod_camion and cod_chofer = @cod_chofer and dia_planif = @dia_planif and sps = @sps";


                string sql2 = @"insert into FACTURA_SP_enc (cod_trans, cod_camion, cod_chofer,dia_planif,sps ) values "
                                    + " (@cod_trans, @cod_camion, @cod_chofer, @dia_planif, @sps); select scope_identity();";


                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_trans", cod_trans);
                    cmd.Parameters.AddWithValue("@cod_camion", cod_camion);
                    cmd.Parameters.AddWithValue("@cod_chofer", cod_chofer);
                    cmd.Parameters.AddWithValue("@dia_planif", Convert.ToDateTime(dia_plani));
                    cmd.Parameters.AddWithValue("@sps", sps);

                    try
                    {
                        scalar = Convert.ToInt32(cmd.ExecuteScalar());

                    }
                    catch (Exception EX)
                    {

                        return 0;

                        //return 0;
                    }
                }
            }
            return scalar;
        }

        internal static DataTable trae_sp_planificada(string id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                //q.trandate
                conn.Open();
                string sql = @"select * from [dbo].[V_Sp_Asignados] where id =  " + id;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable trae_sp_re_planificada(string id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                //q.trandate
                conn.Open();
                string sql = @"select *
                                    FROM [V_Sp_Asignados_log] n  where id = " + id +
                                  "  and fecha_update =                                         " +
                                  "  (                                                          " +
                                  "    SELECT  MAX(fecha_update) AS fecha_maxima                " +
                                  "    FROM [V_Sp_Asignados_log]  where id = " + id +
                                  "  ) ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable VM_estados_asignados(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                //q.trandate
                conn.Open();
                string sql = @"select distinct(estado) , case when estado = 1 then 'Pendiente' else 'Finalizado' end as descrestado from sp_asignados ";


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable categoria_cobranza_comis()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                //q.trandate
                conn.Open();
                string sql = @"select distinct(CATEGORIA_COBRANZA), PORC_ROSA_SOLIS, PORC_EVELYN_LEIVA, PORC_MARIANA_SILVA from  comision_thx_cobranza
                                    order by CATEGORIA_COBRANZA
                                    ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable comision_cobranza(string periodo)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                //q.trandate
                conn.Open();
                string sql = @" select G.*, 
                                     ROUND( (G.PESO * G.PORC_ROSA_SOLIS)/100 ,0)AS COMISION_RO ,
                                      ROUND( (G.PESO * G.PORC_EVELYN_LEIVA)/100 ,0)AS COMISION_EV,
                                       ROUND( (G.PESO * G.PORC_MARIANA_SILVA)/100 ,0)AS COMISION_MA
                                     from 
                                     (
                                     select distinct(CATEGORIA_COBRANZA), SUM(PESO) AS PESO, PORC_ROSA_SOLIS, PORC_EVELYN_LEIVA, PORC_MARIANA_SILVA 
                                    from  V_comision_thx_cobranza
                                    WHERE  cod_periodo_cierre = '" + periodo + "' " +
                                "    GROUP BY CATEGORIA_COBRANZA, PORC_ROSA_SOLIS, PORC_EVELYN_LEIVA, PORC_MARIANA_SILVA " +
                                "    ) G " +
                                "    order by G.CATEGORIA_COBRANZA  ";


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable listar_chofer(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                //q.trandate
                conn.Open();
                string sql = @" select * from chofer where 1=1 " + where;


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }


        internal static DataTable listar_camion(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                //q.trandate
                conn.Open();
                string sql = @" select * from camion where 1=1 " + where;


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }


        internal static DataTable listar_camion_dia(string where1)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                //q.trandate
                conn.Open();
                string sql = @" select * from [dbo].[V_CAMION_DIA] where 1=1 " + where1;


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable productos_stock_sp(string where, string fecharcp2, string fechaSaldo, string desde, string hasta, string futuro, string clase)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                //q.trandate
                conn.Open();
                string sql = @"  SELECT b.invtid as CodProd, b.descr as Descrip, b.glclassid as Grupo, b.stkunit as TipoUnidad, b.siteid as Bodega, " +
                           "   (select sum (q.qty) from Stock_diario q where q.siteid = b.siteid and b.invtid = q.invtid " + fecharcp2 + ") as stock, " +
                           "   (select sum (q.qty) from Stock_diario q where q.siteid = b.siteid and b.invtid = q.invtid " + fechaSaldo + ") as SaldoInicial, " +
                           "(select cantidad from StockFuturo  " +
                           "     where cod_producto = invtid " + futuro + "  and id = " +
                           "     (select max(id) from StockFuturo " +
                           "      where cod_producto = invtid " + futuro + " ) ) as Cantidad " +
                           "   from Stock_diario b " + clase + where +
                           "    group by  descr , siteid, invtid, glclassid, stkunit    ORDER BY DESCR";


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable comision_cobranza_2(string periodo)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                //q.trandate
                conn.Open();
                string sql = @" select G.*, 
                                     ROUND( (G.PESO * G.PORC_ROSA_SOLIS)/100 ,0)AS COMISION_RO ,
                                      ROUND( (G.PESO * G.PORC_EVELYN_LEIVA)/100 ,0)AS COMISION_EV,
                                       ROUND( (G.PESO * G.PORC_MARIANA_SILVA)/100 ,0)AS COMISION_MA
                                     from 
                                     (
                                     select distinct(CATEGORIA_COBRANZA), SUM(NETO_FACTURA) AS PESO, PORC_ROSA_SOLIS, PORC_EVELYN_LEIVA, PORC_MARIANA_SILVA 
                                    from    V_COMISIONES_FACTURAS_COBRANZA_2
                                    WHERE  cod_periodo = '" + periodo + "' " +
                                "    GROUP BY CATEGORIA_COBRANZA, PORC_ROSA_SOLIS, PORC_EVELYN_LEIVA, PORC_MARIANA_SILVA " +
                                "    ) G " +
                                "    order by G.CATEGORIA_COBRANZA  ";


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static string trae_grupo_stock(string producto)
        {
            //; select scope_identity();
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select top 1 glclassid from stock_diario where invtid = '" + producto + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id_asignado", select_scope);

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable datos_ids(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                //q.trandate
                conn.Open();
                string sql = @"SELECT top 1 (select count( distinct(coddocumento) ) from V_Sp_Asignados where id in (" + v + ")) as sps, (select count( distinct(rut) ) from V_Sp_Asignados where id in (" + v + ")) as clientes, " +
                "  (select sum(Cant_despachado_kg) from [V_SP_ASIG_KG] where id_asignado in (" + v + ") ) as kilos  from SP_Asignados  where id in (" + v + ")";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable VM_saldos_sp(string where3)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                //q.trandate
                conn.Open();
                string sql = @"select * from V_SALDOS_SP WHERE  CANTIDAD_FALTA > 0 " + where3.Replace("b.", "") + " and estado in ('10P', '10S', '10', '40') order by coddocumento desc";


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static string guardar_valor_equivale_sp(string v, object tex1, string text2, string unidades)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from unidad_stock_sp where cod_prod = '" + v + "' ; " +
                   " insert into unidad_stock_sp values ('" + v + "', " + text2.Replace(",", ".") + ", " + unidades.Replace(",", ".") + ") ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable crear_estado_3()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select 'planificado' as estado union select 'pendiente' as estado ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable trae_lotes_(string invtid, string siteid, string batnbr, string refnbr, string trandate, string trantype)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from [dbo].[Lotes_vencimiento] where  invtid = '" + invtid + "' and siteid = '" + siteid + "' and batnbr='" + batnbr + "' and refnbr = '" + refnbr + "' and convert(varchar, trandate,103)= convert(varchar,'" + trandate + "',103) and trantype = '" + trantype + "'";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;


        }

        internal static string update_importacion_valida(string factura, int v)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"UPDATE [dbo].[ComisionImportaciones]  SET [es_valida] = @valida where factura = @factura";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@factura", factura);
                    cmd.Parameters.AddWithValue("@valida", v);


                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }

        internal static string guarda_cliente_vi(string factura)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from [dbo].[ComisionRegla2]  where factura = '" + factura.Trim() + "';" +

                              "insert into  [dbo].[ComisionRegla2] values ('" + factura + "', 0.17);   ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en delete: " + EX.Message;
                    }
                }
            }
        }

        internal static DataTable trae_datos_importacion_comision(string factura)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from [dbo].[ComisionImportaciones] where  factura = '" + factura.Trim() + "'";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;

        }

        internal static string quitar_importacion(string factura)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from [dbo].[comisionimportaciones]  where factura = '" + factura.Trim() + "';";



                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en delete: " + EX.Message;
                    }
                }
            }
        }

        internal static string guarda_importacion(string factura, string contrato, string toneladas, string negocio, double porcentaje)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from [dbo].[comisionimportaciones]  where factura = '" + factura.Trim() + "';" +

                              "insert into  [dbo].[comisionimportaciones]   ([factura] ,[num_contrato] ,[total_importado] ,[total_negocio], [porcentaje] ) " +
                                 " values ('" + factura + "', '" + contrato + "', " + toneladas + ", " + negocio + ", " + porcentaje + ");   ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en delete: " + EX.Message;
                    }
                }
            }
        }

        internal static string quita_ewos(string factura)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from [dbo].[ComisionRegla6]  where factura = '" + factura.Trim() + "';";



                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en delete: " + EX.Message;
                    }
                }
            }
        }

        internal static string guarda_ewos(string factura)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from [dbo].[ComisionRegla6]  where factura = '" + factura.Trim() + "';" +

                              "insert into  [dbo].[ComisionRegla6] values ('" + factura + "', 0.5);   ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en delete: " + EX.Message;
                    }
                }
            }
        }

        internal static string es_ewos(string factura)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select count (*) from [dbo].[ComisionRegla6] where factura = '" + factura.Trim() + "' ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            return scalar;
        }

        internal static string es_venta_vi(string factura)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select count (*) from [dbo].[ComisionRegla2] where factura = '" + factura.Trim() + "' ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            return scalar;
        }

        internal static string quita_cliente_vi(string factura)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from [dbo].[ComisionRegla2]  where factura = '" + factura.Trim() + "';";



                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en delete: " + EX.Message;
                    }
                }
            }
        }

        internal static string update_lote(string invtid, string siteid, string batnbr, string refnbr, string trandate, string trantype, string lote, string envasado, string vencimiento)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"update Lotes_vencimiento set envasado = @envasado , vencimiento = @vencimiento where invtid = @invtid and " +
                    " siteid = @siteid and batnbr = @batnbr and refnbr = @refnbr and trandate = @trandate and trantype = @trantype and lote= @lote";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@invtid", invtid);
                    cmd.Parameters.AddWithValue("@siteid", siteid);
                    cmd.Parameters.AddWithValue("@batnbr", batnbr);
                    cmd.Parameters.AddWithValue("@refnbr", refnbr);
                    cmd.Parameters.AddWithValue("@trandate", trandate);
                    cmd.Parameters.AddWithValue("@trantype", trantype);

                    cmd.Parameters.AddWithValue("@lote", lote);
                    cmd.Parameters.AddWithValue("@envasado", envasado);
                    cmd.Parameters.AddWithValue("@vencimiento", vencimiento);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable fechas_y_dias_vencimiento(string invtid, string siteid, string batnbr, string refnbr, string trandate, string trantype)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select vencimiento, envasado, dif_envasado_vencimiento, dif_envasado_hoy, vida_util, porc_vida_util, certificado from [dbo].[V_Vencimiento_final] where  invtid = '" + invtid + "' and siteid = '" + siteid + "' and batnbr='" + batnbr + "' and refnbr = '" + refnbr + "' and convert(varchar, trandate,103)= convert(varchar,'" + trandate + "',103) and trantype = '" + trantype + "'";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static string delete_lote(string invtid, string siteid, string batnbr, string refnbr, string trandate, string trantype, string lote)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from [dbo].[Lotes_vencimiento]  where invtid = '" + invtid + "' and siteid = '" + siteid + "' and batnbr='" + batnbr + "' and refnbr = '" + refnbr + "' and convert(varchar, trandate,103)= convert(varchar,'" + trandate + "',103) and trantype = '" + trantype + "' and lote = '" + lote + "'";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en delete: " + EX.Message;
                    }
                }
            }
        }

        internal static string insert_lote(string invtid, string siteid, string batnbr, string refnbr, string trandate, string trantype, string lote, string envasado, string vencimiento)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into [dbo].[Lotes_vencimiento] (invtid, siteid, batnbr, refnbr, trandate, trantype , lote, envasado, vencimiento) " +
                "values (@invtid, @siteid, @batnbr, @refnbr, @trandate, @trantype , @lote, @envasado, @vencimiento)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@invtid", invtid);
                    cmd.Parameters.AddWithValue("@siteid", siteid);
                    cmd.Parameters.AddWithValue("@batnbr", batnbr);
                    cmd.Parameters.AddWithValue("@refnbr", refnbr);
                    cmd.Parameters.AddWithValue("@trandate", trandate);
                    cmd.Parameters.AddWithValue("@trantype", trantype);

                    cmd.Parameters.AddWithValue("@lote", lote);
                    cmd.Parameters.AddWithValue("@envasado", envasado);
                    cmd.Parameters.AddWithValue("@vencimiento", vencimiento);


                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Insert: " + EX.Message;
                    }
                }
            }
        }

        public static List<ReporteEntidad> GetBy()
        {
            List<ReporteEntidad> list = new List<ReporteEntidad>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT id_archivo, id_correo from webagente_det_correos_enviados  where id_archivo = @id_archivo And id_correo = @id_correo ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                //cmd.Parameters.AddWithValue("@id_archivo", id_archivo);
                //cmd.Parameters.AddWithValue("@id_correo", id_correo);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(Load(reader));
                }
            }
            return list;
        }
        public static string validar(ref ReporteEntidad u)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT id_archivo, id_correo from webagente_det_correos_enviados where id_archivo = @id_archivo And id_correo = @id_correo ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                //cmd.Parameters.AddWithValue("@id_archivo", u.id_archivo);
                //cmd.Parameters.AddWithValue("@id_correo", u.id_correo);
                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        u = Load(reader);
                        return "OK";
                    }
                }
                catch (Exception EX)
                {
                    return "Error en Validar Detalle: " + EX.Message;
                }
            }
            return "No encontrado en DB";
        }

        internal static DataTable detalle_matriz_click(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select Vendedor, SUM(NETO_PESOS) as Venta, númfactura as 'NºFactura', convert(varchar, fechafactura, 103) as FechaFactura
	                    from thx_v_reporte " + where + " group by númfactura, fechafactura, vendedor, rutcliente,DescProducto ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable no_planificado(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select sum(y.cant_despachar_kg) as kg, count(distinct(y.rut)), count(distinct(y.coddocumento)) from (
                                    select * from [V_SP_DIAS_KG] where  fechadespacho = CONVERT(datetime, '" + v + "',103) ) y   where y.CodDocumento not in (select coddocumento from[NEW_THX].[dbo].[SP_Asignados]  d where y.CodDocumento = d.coddocumento ) group by fechadespacho";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable crear_estado_2()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select 'pendiente' as estado union select 'cerrado' as estado ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static object detalle_matriz_click_cliente(string where)
        {

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select Vendedor, SUM(NETO_PESOS) as Venta, númfactura as 'NºFactura', convert(varchar, fechafactura, 103) as FechaFactura, DescProducto
	                    from thx_v_reporte " + where + "  and producto <> 'FLETE'  and producto <> 'DESCARGA'  and producto <> 'FLETECAJA'   group by númfactura, fechafactura, vendedor, rutcliente,DescProducto ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable costo_producto(string desde_1, string desde_2, string where, string fecha_compra, string hasta)
        {

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select distinct(t.producto) as 'Cod Prod', t.descproducto as 'Prod',  (select Convert(varchar,max(f.fecha),103) from stock_excel f where f.cod_producto = rtrim(ltrim(t.producto)) " + desde_2 + ") as fecha_excel, " +

   "             ColumnaExcel = case when rtrim(ltrim(t.bodega)) = 'ZARATESOP' or rtrim(ltrim(t.bodega)) = 'SITRANS' or rtrim(ltrim(t.bodega)) = 'ABARROTES' " +
    "            THEN 'bod_usd' " +
    "              when rtrim(ltrim(t.bodega)) = 'LOVALLEDOR' or rtrim(ltrim(t.bodega)) = 'AGUNSOP' or  rtrim(ltrim(t.bodega)) = 'LOGINSA'  or " +
    "               rtrim(ltrim(t.bodega)) = 'LOGINSASOP' " +
    "               THEN 'cm_stgo' " +
    "                when rtrim(ltrim(t.bodega)) = 'QUILLOTSOP' or rtrim(ltrim(t.bodega)) = 'LOCAL1' " +
    "            THEN 'cm_qta' " +
    "         when rtrim(ltrim(t.bodega)) = 'QUILLOTA1' " +
    "          THEN 'cm_qta' " +
    "         when rtrim(ltrim(t.bodega)) = 'ARGENTINA' " +
    "        THEN 'bod_usd' " +
    "               when rtrim(ltrim(t.bodega)) = 'ARICASOP' " +
    "       THEN 'arica' " +
    "             when rtrim(ltrim(t.bodega)) = 'ARICA1SOP' " +
    "      THEN 'arica' " +
    "       end, " +

                    "                      CostoExcel = case   " +
         "            when rtrim(ltrim(t.bodega)) = 'ZARATESOP' or rtrim(ltrim(t.bodega)) = 'SITRANS' or rtrim(ltrim(t.bodega)) = 'ABARROTES' " +
          "           THEN(select top 1 j.bod_usd from stock_excel j where j.cod_producto = rtrim(ltrim(t.producto)) and j.fecha = (select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.producto)) " + desde_2 + ") group by j.fecha, j.bod_usd   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
          "            when rtrim(ltrim(t.bodega)) = 'LOVALLEDOR' or rtrim(ltrim(t.bodega)) = 'AGUNSOP' or  rtrim(ltrim(t.bodega)) = 'LOGINSA'  or  rtrim(ltrim(t.bodega)) = 'LOGINSASOP'    " +
          "           THEN(select  top 1 j.cm_stgo from stock_excel j where j.cod_producto = rtrim(ltrim(t.producto)) and j.fecha=(select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.producto)) " + desde_2 + ") group by j.fecha, j.cm_stgo   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
          "           when rtrim(ltrim(t.bodega)) = 'QUILLOTSOP' or rtrim(ltrim(t.bodega)) = 'LOCAL1'" +
          "            THEN(select  top 1 j.cm_qta from stock_excel j where j.cod_producto = rtrim(ltrim(t.producto)) and j.fecha=(select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.producto)) " + desde_2 + ") group by j.fecha, j.cm_qta   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
          "            when rtrim(ltrim(t.bodega)) = 'QUILLOTA1' " +
          "            THEN(select  top 1 j.cm_qta from  stock_excel j where j.cod_producto = rtrim(ltrim(t.producto)) and j.fecha=(select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.producto)) " + desde_2 + ") group by j.fecha, j.cm_qta   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
          "           when rtrim(ltrim(t.bodega)) = 'ARGENTINA' " +
          "           THEN(select  top 1 j.bod_usd from  stock_excel j where j.cod_producto = rtrim(ltrim(t.producto)) and j.fecha=(select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.producto)) " + desde_2 + ") group by j.fecha, j.bod_usd   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
          "            when rtrim(ltrim(t.bodega)) = 'ARICASOP' " +
          "           THEN(select top 1  j.arica from  stock_excel j where j.cod_producto = rtrim(ltrim(t.producto)) and j.fecha=(select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.producto)) " + desde_2 + ") group by j.fecha, j.arica   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
          "             when rtrim(ltrim(t.bodega)) = 'ARICA1SOP' " +
          "          THEN(select top 1  j.arica  from stock_excel j where j.cod_producto = rtrim(ltrim(t.producto)) and j.fecha=(select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.producto)) " + desde_2 + ") group by j.fecha, j.arica   having j.fecha = max(j.fecha) order by j.fecha asc  ) end, " +

          " (select top 1 u.UnitCost as suma from stock_compra_gen u where t.producto = u.invtid and  " +
          " u.RcptDate = (select max(x.rcptdate) from Stock_Compra_gen x where t.producto = x.invtid  and x.RcptDate  <= convert(datetime, '" + hasta + "', 103) ))  as Compra, '' as Porct   " +
          ",  (select Convert(varchar,max(x.rcptdate),103) from Stock_Compra_gen x where t.producto = x.invtid " + fecha_compra + ") as FechaCompra , " +
               "            (select (select sum(c.pesoslc)   from stock_compra c where c.PONbr = " +
           "              (select  max(u.PONbr) from stock_compra_gen u " +
            "                             where t.producto = u.invtid and   u.RcptDate = " +
            "           (select max(x.rcptdate) from Stock_Compra_gen x where t.producto = x.invtid " +
          "              " + fecha_compra + "))) * " +
         "               (select top 1 u.UnitCost from stock_compra_gen u " +
       "                                    where t.producto = u.invtid and u.RcptDate = " +
       "              (select max(x.rcptdate) from Stock_Compra_gen x where t.producto = x.invtid " +
       "                 " + fecha_compra + "))  / (select sum(valor) from " +
       "                (select top 1 valor = case " +
       "                 when TranType = 'X' then  CuryExtCost * -1  else  CuryExtCost  end from Stock_Compra_gen " +
      "                 WHERE PONbr = (select max(u.PONbr) from stock_compra_gen u where t.producto = u.invtid and   u.RcptDate = " +
       "                (select max(x.rcptdate) from Stock_Compra_gen x where t.producto = x.invtid " +
       "                and x.RcptDate  <= convert(datetime, '" + hasta + "', 103) )) ) as ww) ) as 'CostoImport', " +
          " (select top 1 u.RcptUnitDescr as suma from stock_compra_gen u where t.producto = u.invtid and  " +
          " u.RcptDate = (select max(x.rcptdate) from Stock_Compra_gen x where t.producto = x.invtid " + fecha_compra + ")) " +
         " as UnidadCompra, " +
         "  (select sum(n.qty) from stock_diario n where t.producto = n.invtid  " + fecha_compra.Replace("x.RcptDate", "n.trandate") + ")   as Stock_Total, " +
       " (select top 1 u.POnbr as ponbr from stock_compra_gen u where t.producto = u.invtid and  " +
          " u.RcptDate = (select max(x.rcptdate) from Stock_Compra_gen x where t.producto = x.invtid  and x.RcptDate  <= convert(datetime, '" + hasta + "', 103) ))  as OC " +
         " from thx_v_reporte t  where 1=1 and producto >= '1000' and producto <= '9999' and user1 <> 'Granos'" + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;




        }

        internal static DataTable ventamovil_sp_det_EXT(string sp)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil_real"].ToString()))
            {
                conn.Open();
                string sql = @"	select * from [dbo].[ext_TrnDocumentoDetalle] where [CodDocumento] = '" + sp + "'";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable ventamovil_sp_enc_EXT(string sp)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil_real"].ToString()))
            {
                conn.Open();
                string sql = @"	select * from [dbo].[ext_TrnDocumentoCabecera] where [CodDocumento] = '" + sp + "'";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static double existe_sp(string sp)
        {
            double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select count (*) from sps_procesadas1 where coddocumento = '" + sp + "' ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Double.Parse((cmd.ExecuteScalar().ToString()));
                conn.Close();
            }
            return scalar;
        }

        internal static DataTable ventamovil_sp_det(string sp)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil_real"].ToString()))
            {
                conn.Open();
                string sql = @"	select * from [dbo].[TrnDocumentoDetalle] where [CodDocumento] = '" + sp + "'";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable ventamovil_sp_enc(string sp)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil_real"].ToString()))
            {
                conn.Open();
                string sql = @"	select * from [dbo].[TrnDocumentoCabecera] where [CodDocumento] = '" + sp + "'";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable log_re_planificar(string id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"	 select *, CONVERT(varchar, fecha_despacho, 103) as fecha_despacho2
	                             from v_sp_asignados_log 
	                              where 1=1 
	                             and v_sp_asignados_log.id = " + id;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static string update_asignada_replanificar(string id, string v, string fecha, string carga, string transpor, string camion, string chofer, string obs, string vuelta)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"update SP_Asignados set fecha_despacho = @fecha_despacho, 
                                    cod_trans = @cod_tr,
                                    cod_camion = @cod_camion, 
                                    cod_chofer = @cod_chofer,
                                    observacion = @obs,
                                    carga_inicial = @carga,
                                    vuelta = @vuelta
                                where id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@fecha_despacho", fecha);
                    cmd.Parameters.AddWithValue("@cod_tr", transpor);
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.Parameters.AddWithValue("@cod_camion", camion);
                    cmd.Parameters.AddWithValue("@cod_chofer", chofer);
                    cmd.Parameters.AddWithValue("@obs", obs);
                    cmd.Parameters.AddWithValue("@carga", carga);
                    cmd.Parameters.AddWithValue("@vuelta", vuelta);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }

        internal static string selec_insert_log(string v)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into SP_Asignados_log  " +
                "select [id],[coddocumento],[cod_trans],[carga_inicial],[fecha_despacho],[disponible_camion], " +
                " [estado], GETDATE() as updatet  ,[cod_camion],[cod_chofer],[observacion] from SP_Asignados where id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", v);


                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Insert: " + EX.Message;
                    }
                }
            }
        }

        internal static DataTable listar_clientes_cobranza(string where)
        {

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT ltrim(rtrim(([rutcliente]))) as rut,ltrim(rtrim(([nombrecliente]))) as nombre
                                FROM v_cobranza_todos
                                " + where + " group by rutcliente, nombrecliente   order by ltrim(rtrim(([nombrecliente])))";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable VM_lista_grupo(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil"].ToString()))
            {
                conn.Open();
                string sql = @"select distinct (DescEmisor)  from VPEDIDOCABECERA where DescEmisor <> 'Granos'";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable VM_listaVendedor(string v, string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil"].ToString()))
            {
                conn.Open();
                string sql = @"select distinct (codvendedor), NombreVendedor  from VPEDIDOCABECERA where 1=1 " + v.Replace("b.", "") + where;

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string trae_correos_bodega(string bodega, string grupo)
        {
            //; select scope_identity();
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select correos from correo_bodega where cod_bodega = '" + bodega + "' and grupo in  (" + grupo + ")";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id_asignado", select_scope);

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string delete_estado_sp(string factura)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from estado_sp " +
                " where sp = @sp";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@sp", factura);
                    //cmd.Parameters.AddWithValue("@periodo", periodo);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Delete : " + EX.Message;
                    }
                }
            }
        }

        internal static string insert_estado_sp(string factura, string estado)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into estado_sp (sp, estado) " +
                "values (@sp, @estado)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@sp", factura);
                    cmd.Parameters.AddWithValue("@estado", estado);


                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Insert: " + EX.Message;
                    }
                }
            }
        }

        internal static DataTable listar_camiones_asignados(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select top 100 *, CONVERT(varchar, fecha_despacho, 103) as fecha_despacho2, 
                    isnull ((select  top 1 e.id from [SP_Asignados_log]  e where v_sp_asignados.id = e.id),0) as tiene_log from v_sp_asignados where 1=1 " + where;

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable detalle_producto_stock(string where, string mes, string año)
        {

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "selecT  stock_diario.[invtid] " +
                       "       , stock_diario.[descr] " +
                       "       ,stock_diario.[glclassid] " +
                       "       ,stock_diario.[stkunit] " +
                       "       ,stock_diario.[siteid] " +
                       "       ,stock_diario.[trandate] " +
                       "       ,stock_diario.[trantype] " +
                       "       ,  case when stkunit = 'KGR'   then stock_diario.[Qty]	else stock_diario.[Qty] * isnull(Unidad_stock.equivale_kilos,00) end as qty " +
                       "    , case when stkunit = 'KGR'   then (selecT  sum(qty) from STOCK_DIARIO where  1=1 " +
                        where.Replace("where 1=1", "") + " and convert(datetime, trandate,103) <= convert(datetime, '" + año + "-01-01', 103) " +
                      "    )  " +
                      " else (selecT  sum(qty) from STOCK_DIARIO where  1=1 " +
                        where.Replace("where 1=1", "") + " and convert(datetime, trandate,103) <= convert(datetime, '" + año + "-01-01', 103) " +
                      "   ) * isnull(Unidad_stock.equivale_kilos,00) " +
                      " end as arrastre from STOCK_DIARIO left join unidad_stock on Unidad_stock.cod_prod = invtid where   " +
                    "   convert(datetime , trandate , 103) <= convert(datetime, EOMONTH('" + mes + "/01/" + año + "'),103) and DATEPART(year, trandate) = " + año +
                    where.Replace("where 1=1", "") + " order by  trandate asc";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;



        }

        internal static string total_kg_id(string select_scope)
        {
            //; select scope_identity();
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select sum(cant_despachado_kg) from [dbo].[V_SP_ASIG_KG] where id_asignado = " + select_scope + "";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id_asignado", select_scope);



                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string nombre_transporte(string v)
        {
            //; select scope_identity();
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select [nombre_trans] from[NEW_THX].[dbo].[Transportista] " + v + "";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id_asignado", select_scope);
                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable DESPA_listar_camion(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select cod_camion, patente from camion where 1=1 and " + v;

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;


        }

        internal static DataTable corre_bodega(string dia, string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select distinct( a.codbodega), a.descbodega,
                                        (select correos from [dbo].[Correo_Bodega] c where c.cod_bodega = a.codbodega and c.grupo = a.asignado_por) as correos
                                         from [dbo].[V_Sp_Asignados] a 
                                         " + dia.Replace("b.", "a.") + where.Replace("b.", "a.");
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable detalle_planificado_dia(string dia, string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select a.*,
                          (select top 1 y.descproducto from  [SoprodiVenta].[dbo].[VPEDIDODETALLE] y where a.codproducto =  y.codproducto) as nomb , 
                        (select top 1 y.CodUnVenta from  [SoprodiVenta].[dbo].[VPEDIDODETALLE] y where a.codproducto =  y.codproducto) as tipo_, 
                         (select sum(h.cant_despachado_kg) from V_SP_ASIG_KG h where  a.id_asignado = h.id_asignado and  h.codproducto = a.codproducto and b.coddocumento = h.coddocumento " + dia.Replace("b.fecha_despacho", "h.fecha_despacho").Replace("where", "and") + " ) as cant_kg, " +
                       "  b.fecha_despacho , " +
                       "   (select top 1 k.nombre_transporte_todo from  v_sp_asignados k where a.id_asignado = k.id) as transpo,  " +
               "   (select top 1 y.CodBodega from  [SoprodiVenta].[dbo].[VPEDIDODETALLE] y where a.codproducto =  y.codproducto  and b.coddocumento = y.coddocumento ) as bodega, 	 b.coddocumento, " +
                "  (select top 1 k.NombreCliente from  v_sp_asignados k where a.id_asignado = k.id) as NombreCliente, 											   " +
                " (select top 1 y.PrecioUnitario from  [SoprodiVenta].[dbo].[VPEDIDODETALLE] y where a.codproducto =  y.codproducto  and b.coddocumento = y.coddocumento ) as unitario" +
                "  , b.asignado_por   , b.orden_cargar     from Det_SP_Asignados a inner join SP_Asignados b on a.id_asignado = b.id " + dia + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }


        internal static DataTable detalle_planificado_dia_camiones(string dia, string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"
                      select *, (carga_inicial - total_kg) as diferencia_carga from (
 select sum (cant_kg) as total_kg , transpo, carga_inicial, bodega from (
                        select (select sum(h.cant_despachado_kg) from V_SP_ASIG_KG h  where  a.id_asignado = h.id_asignado and  h.codproducto = a.codproducto and b.coddocumento = h.coddocumento " + dia.Replace("b.fecha_despacho", "h.fecha_despacho").Replace("where", "and") + " ) as cant_kg," +

                     "   b.fecha_despacho , " +
                     "     (select top 1 k.nombre_transporte_todo from  v_sp_asignados k where a.id_asignado = k.id) as transpo," +
                     "    (select top 1 y.CodBodega from  [SoprodiVenta].[dbo].[VPEDIDODETALLE] y where a.codproducto =  y.codproducto  and b.coddocumento = y.coddocumento ) as bodega, " +
                      "              (select top 1 k.carga_inicial from  v_sp_asignados k where a.id_asignado = k.id) as carga_inicial   " +
                      "     from Det_SP_Asignados a inner join SP_Asignados b on a.id_asignado = b.id " + dia + where + " ) g   group by transpo, carga_inicial, bodega) yy order by yy.total_kg desc ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }


        internal static DataTable DESPA_listar_chofer(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select cod_chofer, nombre_chofer from chofer where 1=1 and " + v;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;

        }

        internal static DataTable VM_LISTAR_SP_2(object v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                //              string sql = "select  CodDocumento, NombreCliente, NombreVendedor, MontoNeto, DescBodega, convert(varchar, FechaDespacho,103) as FechaDespacho, DATEDIFF(DAY, CONVERT(datetime,GETDATE(),103), FechaDespacho) as DifDias " + 
                //"  , convert(varchar, FechaEmision, 103) as FechaEmision , CodVendedor, NotaLibre, CodBodega , CodMoneda, DescEstadoDocumento, Facturas,  GxEstadoSync,  " + 
                //"     convert(varchar, GxActualizado, 103) as GxActualizado , GxEnviadoERP, convert(varchar, FechaCreacion, 103) as FechaCreacion ,  " + 
                //"     ValorTipoCambio,LimiteSeguro, TipoCredito, CreditoDisponible, CreditoAutorizado, EmailVendedor from VPEDIDOCABECERA  " + where3;

                string sql = "SELECT * FROM VPEDIDOCABECERA_new_thx " + v;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable VM_listar_sp_select(string where3)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil"].ToString()))
            {
                conn.Open();
                //              string sql = "select  CodDocumento, NombreCliente, NombreVendedor, MontoNeto, DescBodega, convert(varchar, FechaDespacho,103) as FechaDespacho, DATEDIFF(DAY, CONVERT(datetime,GETDATE(),103), FechaDespacho) as DifDias " + 
                //"  , convert(varchar, FechaEmision, 103) as FechaEmision , CodVendedor, NotaLibre, CodBodega , CodMoneda, DescEstadoDocumento, Facturas,  GxEstadoSync,  " + 
                //"     convert(varchar, GxActualizado, 103) as GxActualizado , GxEnviadoERP, convert(varchar, FechaCreacion, 103) as FechaCreacion ,  " + 
                //"     ValorTipoCambio,LimiteSeguro, TipoCredito, CreditoDisponible, CreditoAutorizado, EmailVendedor from VPEDIDOCABECERA  " + where3;

                string sql = "select b.CodDocumento, b.NombreCliente, b.NombreVendedor, b.MontoNeto, b.DescBodega, convert(varchar, b.FechaDespacho,103) as FechaDespacho,  convert(varchar, d.Fecha_Despacho,103) as fPLAN, DATEDIFF(DAY, CONVERT(datetime,GETDATE(),103), b.FechaDespacho) as DifDias " +
                "  , convert(varchar, b.FechaEmision, 103) as FechaEmision , b.CodVendedor, b.NotaLibre, b.CodBodega , b.CodMoneda, b.DescEstadoDocumento, '' as Facturas,  b.GxEstadoSync,  " +
                "     convert(varchar, b.GxActualizado, 103) as GxActualizado , b.GxEnviadoERP, convert(varchar, b.FechaCreacion, 103) as FechaCreacion ,  " +
                "     b.ValorTipoCambio,b.LimiteSeguro, b.TipoCredito, b.CreditoDisponible, b.CreditoAutorizado, b.EmailVendedor, b.CodEstadoDocumento as ESTADO, a.CodProducto, a.Cantidad,isnull(c.coddocumento,'no')  as AprobadoFull " +
                " , isnull(d.coddocumento,'no')  as Asignada,  e.estado as EstadoParcial " +
                "" +
                "" +
                " from VPEDIDODETALLE_THX a inner join VPEDIDOCABECERA b on a.coddocumento = b.coddocumento      left join THX_Sp_Aprobadas c on a.CodDocumento = c.coddocumento  " +
                "  left join [NEW_THX].[dbo].[SP_Asignados]  d on a.CodDocumento = d.coddocumento  left join [NEW_THX].[dbo].[Estado_SP]  e on a.CodDocumento = e.sp " + where3;

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable ultimo_excel_dt(string fecha)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "select * from stock_excel_2 where fecha =(select max(fecha) from Stock_Excel_2 where fecha <= CONVERT(datetime, '" + fecha + "', 103) )";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }


        internal static DataTable arrastre_noventa(string condicion, string selectedValue, string text, string dias)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "select isnull( convert(varchar, min(trandate),103 ) , '') ," +
                         "   isnull((select case when stkunit = 'KGR'   then isnull(sum(stock_diario.[Qty]),0)	else isnull(sum(stock_diario.[Qty]),0) * isnull(Unidad_stock.equivale_kilos,00) end from stock_diario left join unidad_stock on Unidad_stock.cod_prod = invtid  where convert(datetime , trandate , 103) < " +
                         "   convert(datetime, dateadd(DAY, -" + dias + ", '" + text + "-" + selectedValue + "-01'),103) " +
                         "   " + condicion.Replace("where 1=1", "") + " group by stock_diario.stkunit, unidad_stock.equivale_kilos)   , 0)  " +
                         "   + " +
                         "     isnull( (SELECT case when stkunit = 'KGR'   then isnull(sum(stock_diario.[Qty]),0)	else isnull(sum(stock_diario.[Qty]),0) * isnull(Unidad_stock.equivale_kilos,00) end FROM stock_diario left join unidad_stock on Unidad_stock.cod_prod = invtid  where convert(datetime , trandate , 103) >= convert(datetime, dateadd(DAY, -" + dias + ", '" + text + "-" + selectedValue + "-01'),103) and " +
                         "   convert(datetime , trandate , 103) <= convert(datetime, '01/" + selectedValue + "/" + text + "',103)  " +
                         "   " + condicion.Replace("where 1=1", "") + " and Qty < 0  group by stock_diario.stkunit, unidad_stock.equivale_kilos" +
                         "   ), 0) as tete  from Stock_diario where year( convert(datetime , trandate , 103) ) = " + text + "  " + condicion.Replace("where 1=1", "") + "  ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;

        }

        internal static string desplanificar_sp(string codDocumento, int v)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from [dbo].[SP_Asignados] where id = " + codDocumento + ";delete from [dbo].[Det_SP_Asignados] where id_asignado = " + codDocumento + ";";


                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        cmd.ExecuteNonQuery();
                        scalar = "OK";
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string trae_fecha_emision_sp(string sp)
        {
            //; select scope_identity();
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil"].ToString()))
            {
                conn.Open();
                string sql = @"select  convert(varchar, fechaemision,103) from [dbo].[VPEDIDOCABECERA] where coddocumento = '" + sp + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id_asignado", select_scope);



                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable VM_estados(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil"].ToString()))
            {
                conn.Open();
                string sql = "select codestadodocumento, descestadodocumento from MaeEstadoDocumento " + v + " union all select '10S' as codestadodocumento, 'Aprobado S/ Factura' as aprobado   union all select '10P' as codestadodocumento, 'Aprobado Parcial' as aprobado  order by DescEstadoDocumento ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable VM_clientes(string v, string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil"].ToString()))
            {
                conn.Open();
                string sql = "select distinct(RUT) as rut, NombreCliente as nombre from VPEDIDOCABECERA  " + v.Replace("b.", "") + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable dia_entrada_noven_y_salida(string condicion, string selectedValue, string text, int i)
        {
            DataTable dt = new DataTable();
            double valor = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "select (isnull((select case when stock_diario.stkunit = 'KGR'   then isnull( sum(stock_diario.[Qty]) ,0)	else sum(stock_diario.[Qty]) * isnull(Unidad_stock.equivale_kilos,00) end from stock_diario  left join unidad_stock on Unidad_stock.cod_prod = stock_diario.invtid where convert(datetime , stock_diario.trandate , 103) = " +
                         "   convert(datetime, dateadd(DAY, -90, '" + text + "-" + selectedValue + "-" + i + "'),103) " +
                         "   " + condicion.Replace("where 1=1", "") + " and qty >0 group by stock_diario.stkunit, unidad_stock.equivale_kilos ),0)), " +


                         " (isnull( (select case when stock_diario.stkunit = 'KGR'   then  isnull(sum( stock_diario.[Qty]),0)	else sum(stock_diario.[Qty]) * isnull(Unidad_stock.equivale_kilos,00) end from stock_diario  left join unidad_stock on Unidad_stock.cod_prod = stock_diario.invtid  where convert(datetime , stock_diario.trandate , 103) = " +
                         "   convert(datetime, '" + i.ToString().PadLeft(2, '0') + "/" + selectedValue + "/" + text + "',103) " +
                         "   " + condicion.Replace("where 1=1", "") + " and qty < 0 group by stock_diario.stkunit, unidad_stock.equivale_kilos ),0))"

                         ;

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;

        }

        internal static string update_asignada(string id_asinada)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"update SP_Asignados set estado = @estado where id like @id_asig";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@estado", "0");
                    cmd.Parameters.AddWithValue("@id_asig", id_asinada);


                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }

        internal static string trae_correo_sp(string v)
        {
            //; select scope_identity();
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT STUFF ((SELECT  CAST(', ' AS varchar(MAX)) + CONVERT(varchar, EmailVendedor) " +
                            "FROM VPEDIDOCABECERA " +
                            "where coddocumento in (" + v + ") GROUP BY EmailVendedor " +
                            "ORDER BY EmailVendedor FOR XML PATH('')), 1, 1, '') AS EmailVendedor ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id_asignado", select_scope);



                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string insert_det_sp_asig(string select_scope, double sum, string cod_prod)
        {
            //; select scope_identity();
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"INSERT INTO [dbo].[Det_SP_Asignados]
                                   ([id_asignado]
                                   ,[codproducto]
                                   ,[despachado])
                             VALUES
                                   (@id_asignado
                                   ,@codproducto
                                   ,@despachado)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id_asignado", select_scope);
                    cmd.Parameters.AddWithValue("@codproducto", cod_prod);
                    cmd.Parameters.AddWithValue("@despachado", sum);




                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string insert_encab_sp_asig(string sp, string cod_trans, string carga_inicial, string fecha, string disponible, string estado, string cod_camion, string cod_chofer, string obs,
                                                    string asignado_por, string carga_total, string orden_cargar, string vuelta)
        {
            //if (cod_chofer == "")
            //{
            //    cod_chofer = "NULL";
            //}
            //if (cod_camion == "")
            //{
            //    cod_camion = "NULL";
            //}

            //; select scope_identity();

            double d_disponible = 0;
            double d_carga_total = 0;

            try
            {
                d_disponible = Convert.ToDouble(disponible);
            }
            catch { }

            try
            {
                d_carga_total = Convert.ToDouble(carga_total);
            }
            catch { }



            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"INSERT INTO [dbo].[SP_Asignados]
                                       ([coddocumento]
                                       ,[cod_trans]
                                       ,[carga_inicial]
                                       ,[fecha_despacho]
                                       ,[disponible_camion]
                                       ,[estado]
                                       ,[cod_camion]
                                       ,[cod_chofer]
                                       ,[observacion]
                                       ,[asignado_por]
                                       ,[carga_total]  
                                       ,[orden_cargar] 
                                       ,[vuelta] )
                                 VALUES
                                        (@coddocumento
                                       ,@cod_trans
                                       ,@carga_inicial
                                       ,@fecha_despacho
                                       ,@disponible_camion
                                       ,@estado
                                       ,@cod_camion
                                       ,@cod_chofer
                                       ,@obs
                                       ,@asignado_por
                                       ,@carga_total
                                       ,@orden_cargar
                                       ,@vuelta
                                        ); select scope_identity();";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@coddocumento", sp);
                    cmd.Parameters.AddWithValue("@cod_trans", cod_trans);
                    cmd.Parameters.AddWithValue("@carga_inicial", carga_inicial);
                    cmd.Parameters.AddWithValue("@fecha_despacho", fecha);
                    cmd.Parameters.AddWithValue("@disponible_camion", d_disponible);
                    cmd.Parameters.AddWithValue("@estado", estado);
                    cmd.Parameters.AddWithValue("@carga_total", d_carga_total);
                    cmd.Parameters.AddWithValue("@orden_cargar", orden_cargar);
                    cmd.Parameters.AddWithValue("@vuelta", vuelta);
                    if (cod_chofer == "")
                    {
                        cmd.Parameters.AddWithValue("@cod_chofer", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@cod_chofer", cod_chofer);
                    }
                    if (cod_camion == "")
                    {
                        cmd.Parameters.AddWithValue("@cod_camion", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@cod_camion", cod_camion);
                    }

                    cmd.Parameters.AddWithValue("@obs", obs);
                    cmd.Parameters.AddWithValue("@asignado_por", asignado_por);
                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;

        }

        internal static DataTable SP_Marcelo(string v1, string v2, string v3)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "EXEC THX_VALIDA_SP '" + v1 + "', '" + v2 + "', " + Convert.ToDouble(v3).ToString().Replace(",", ".");
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable lista_det_sp_asignada(string id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "select a.* FROM V_DET_Sp_Asignados a where  a.id_asignado = '" + id + "'";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string VM_updateSP(string cod_documento, int v)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                //string sql = @"UPDATE TrnDocumentoCabecera SET CodEstadoDocumento = @estado  WHERE CODDOCUMENTO = @cod_doc";
                string sql = @"insert into sp_eliminada values (@cod_doc, CONVERT(datetime, GETDATE()) , 'N')";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_doc", cod_documento);
                    cmd.Parameters.AddWithValue("@fecha", DateTime.Now);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable cargar_obs_ponbr(string factura)
        {
            DataTable dt = new DataTable();
            double valor = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "select * from compra_sys where ponbr = '" + factura + "'";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static double dia_salida(string condicion, string selectedValue, string text, int i)
        {
            DataTable dt = new DataTable();
            double valor = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "select case when stock_diario.stkunit = 'KGR'   then  isnull(sum( stock_diario.[Qty]),0)	else sum(stock_diario.[Qty]) * isnull(Unidad_stock.equivale_kilos,00) end from stock_diario  left join unidad_stock on Unidad_stock.cod_prod = stock_diario.invtid  where convert(datetime , stock_diario.trandate , 103) = " +
                         "   convert(datetime, '" + i.ToString().PadLeft(2, '0') + "/" + selectedValue + "/" + text + "',103) " +
                         "   " + condicion.Replace("where 1=1", "") + " and qty < 0 group by stock_diario.stkunit, unidad_stock.equivale_kilos";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            foreach (DataRow r in dt.Rows)
            {
                valor = Convert.ToDouble(r[0]);

            }
            return valor;

        }

        internal static DataTable listar_ALL_productos_stock_todos(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select distinct(invtid),  descr from Stock_diario b " + v;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable listar_ALL_productos_stock_granos(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select distinct(invtid),  descr from Stock_diario b where b.glclassid not in ('ABAR', 'MANI') " + v;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable excel_gama(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = " select 'OBSERVACION' as COD, '' as 'TipoCant', u.detalle as 'Descrip.', 0 as 'Costo',0 as 'CostoUnid.', " +
                             "       0 as 'CostImpor', " +
                             "       0 as 'CostoUnidImpor', " +
                            "        0 as 'CostTotal', " +
                            "        0 as 'CostoUnidTotal', " +
                            "        0 as 'Cant.',u.nbr as 'NBR', '' as 'TipoTransc.','' as rcptnbr, null as 'F.Recib', " +
                            "        '' as Proveedor " +
                            "        , '' as detalle, '' as bodega, '' as creafecha      " +
                            "         from[V_COMPRA_SYS_OBSER]  u " + v.Replace("cod", "invtid") +
                            "        UNION " +
                            "        select * from v_compras_sys u " + v +
                            "         order by NBR, COD";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable VM_listar_sp(string where3)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil"].ToString()))
            {
                conn.Open();
                string sql = "select b.CodDocumento, b.NombreCliente, b.NombreVendedor, b.MontoNeto, b.DescBodega, convert(varchar, b.FechaDespacho,103) as FechaDespacho, isnull(convert(varchar, d.fecha_despacho, 103),'----') as FechaPlanificado, DATEDIFF(DAY, CONVERT(datetime,GETDATE(),103), b.FechaDespacho) as DifDias " +
                "  , convert(varchar, b.FechaEmision, 103) as FechaEmision , b.CodVendedor, b.NotaLibre, b.CodBodega , b.CodMoneda, b.DescEstadoDocumento, '' as Facturas,  b.GxEstadoSync,  " +
                "     convert(varchar, b.GxActualizado, 103) as GxActualizado , b.GxEnviadoERP, convert(varchar, b.FechaCreacion, 103) as FechaCreacion ,  " +
                "     b.ValorTipoCambio,b.LimiteSeguro, b.TipoCredito, b.CreditoDisponible, b.CreditoAutorizado, b.EmailVendedor, b.CodEstadoDocumento as ESTADO, a.CodProducto, a.Cantidad,isnull(c.coddocumento,'no')  as AprobadoFull , isnull(m.estado ,'pendiente') as Estado_Cerrado  from VPEDIDODETALLE_THX a inner join VPEDIDOCABECERA b on a.coddocumento = b.coddocumento   " +
                "    left join THX_Sp_Aprobadas c on a.CodDocumento = c.coddocumento   left join[NEW_THX].[dbo].[SP_Asignados] d on a.CodDocumento = d.coddocumento left join [NEW_THX].[dbo].[Estado_SP] m on a.CodDocumento = m.sp " + where3;

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable detalle_producto_stock_2(string where, string mes, string año, string where_simple)
        {

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "selecT  stock_diario.[invtid] " +
                       "       , stock_diario.[descr] " +
                       "       ,stock_diario.[glclassid] " +
                       "       ,stock_diario.[stkunit] " +
                       "       ,stock_diario.[siteid] " +
                       "       ,stock_diario.[trandate] " +
                       "       ,stock_diario.[trantype] " +
                       "       ,case when stkunit = 'KGR'   then stock_diario.[Qty]	else stock_diario.[Qty] * isnull(Unidad_stock.equivale_kilos,00) end as qty " +
                        "    , case when stkunit = 'KGR'   then  (selecT  sum(qty) from STOCK_DIARIO where  1=1 " +
                         where_simple.Replace("where 1=1", "") + " and convert(datetime, trandate,103) < convert(datetime, dateadd(DAY, -90, '" + año + "-12-31'), 103) " +
                      "    ) else " +
                      " (selecT  sum(qty) from STOCK_DIARIO where 1 = 1 " +
                         where_simple.Replace("where 1=1", "") + " and convert(datetime, trandate,103) < convert(datetime, dateadd(DAY, -90, '" + año + "-12-31'), 103) " +
                      "  ) *  isnull(Unidad_stock.equivale_kilos,00)  " +
                      " end as arrastre  from STOCK_DIARIO 	 left join unidad_stock on Unidad_stock.cod_prod = invtid where  1=1 " +
                     where.Replace("where 1=1", "") + " order by  trandate asc";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable VM_listar_detalle(string where3)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ventamovil"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT 
                                 CONVERT(INT , isnull(cantidad,0) - isnull((select sum(cantidad) from[NEW_THX].[dbo].[V_SP_FACTURA_THX] where b.CodProducto = producto and b.CodDocumento = sp),0)  )  as Pendiente, 

                                 CONVERT(INT , isnull((select sum(cantidad) from[NEW_THX].[dbo].[V_SP_FACTURA_THX] where b.CodProducto = producto and b.CodDocumento = sp),0) ) as Cant_despachado
                                ,b.* from VPEDIDODETALLE_THX b " + where3;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable listar_clientes_cobranza_abiertos(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT ltrim(rtrim(([rutcliente]))) as rut,ltrim(rtrim(([nombrecliente]))) as nombre
                                FROM v_cobranza_docs
                                " + where + " group by rutcliente, nombrecliente   order by ltrim(rtrim(([nombrecliente])))";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable lista_compras(string where3)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from v_compras_sys_3 u " + where3;

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable lista_costosimpot(string where3)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from v_costos_import u " + where3;

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable ultima_compra2(string where, string prod, string fechfac, string hasta, string desde)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select suma, valorunid, moneda, costimport, invtid, qty from (" +
                    "    select max(q.RcptDate)  as rcptdate,(select sum(case when t.CuryID = 'PESO' then t.COSTVOUCHED else t.curyextcost end) as suma from stock_compra_gen t where q.invtid = t.invtid and  t.RcptDate >= convert(datetime, '" + desde + "',103) and t.RcptDate <= convert(datetime, '" + hasta + "',103)) as suma   " +
                  "             , (select sum(case when t.trantype = 'X' then (case when t.CuryID = 'PESO' then t.COSTVOUCHED else t.curyextcost end)*-1 else (case when t.CuryID = 'PESO' then t.COSTVOUCHED else t.curyextcost end) end) as suma from stock_compra_gen t where q.invtid = t.invtid and  t.RcptDate >= convert(datetime, '" + desde + "',103) and t.RcptDate <= convert(datetime, '" + hasta + "',103)) / NULLIF( (select sum(case when t.trantype = 'X' then (case when t.RcptUnitDescr = 'TON' then t.qty *1000 else t.qty end)*-1 else (case when t.RcptUnitDescr = 'TON' then t.qty *1000 else t.qty end) end) " +
                  " as qty from stock_compra_gen t where q.invtid = t.invtid and t.RcptDate >= convert(datetime, '" + desde + "', 103) and t.RcptDate <= convert(datetime, '" + hasta + "', 103)),0) as valorunid, q.curyid as moneda,  " +
                  "              (select sum(c.pesoslc) from stock_compra c where c.Producto = q.InvtID  and  fecharecep >= convert(datetime, '" + desde + "',103) and fecharecep <= convert(datetime, '" + hasta + "',103)) as costimport,q.InvtID , " +
                "   (select sum(case when t.trantype = 'X' then (case when t.RcptUnitDescr = 'TON' then t.qty *1000 else t.qty end)*-1 else (case when t.RcptUnitDescr = 'TON' then t.qty *1000 else t.qty end) end) as qty from stock_compra_gen t where q.invtid = t.invtid and t.RcptDate >= convert(datetime, '" + desde + "', 103) and t.RcptDate <= convert(datetime, '" + hasta + "', 103)) as qty" +
                  " from stock_compra_gen q   where q.invtid >= 1000 and 1 = 1" + where + prod + "   and q.rcptdate <= (select max(rcptdate)  " +
                 "  from stock_compra_gen w where w.invtid = q.invtid ) group by q.InvtID, q.curyid  " +
                 "  ) as test where test.RcptDate= (select max(rcptdate) from stock_compra_gen w where 1=1 " + where.Replace("rcptdate", "w.rcptdate") + " and w.InvtID = test.InvtID group by invtid )  ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static string insert_compra_sys(string invtid, string ponbr, string value)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into compra_sys values ('" + invtid + "', '" + ponbr + "', '" + value + "') ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable peso_dolar_semana(string v, string an)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"      select hh.peso , uu.dolar from (
                                       (select sum(monto) as peso from V_COBRANZA_MOVIMIENTOS  ww where ww.monto > 0  	 and ww.OBS NOT LIKE '%NETEO%' 
                                     and  DATEPART(wk, case when ww.OBS not like '%Pago Con Cheque%' then FechaVenc else FechaEvento end ) = " + v +
                                     " and 	 DATEPART(year, case when ww.OBS not like '%Pago Con Cheque%' then FechaVenc else FechaEvento end ) =" + an +
                                "     and ww.[T.Doc] <> 'DM' and REPLACE(SUBSTRING(obs, len(obs) -5, 5),'(','') = 'peso')) as hh , " +
                                 "    (select sum(monto) as dolar from V_COBRANZA_MOVIMIENTOS  ww where ww.monto > 0  	 and ww.OBS NOT LIKE '%NETEO%' " +
                                  "   and  DATEPART(wk,  case when ww.OBS not like '%Pago Con Cheque%' then FechaVenc else FechaEvento end) = " + v +
                                  " and 	 DATEPART(year, case when ww.OBS not like '%Pago Con Cheque%' then FechaVenc else FechaEvento end ) =" + an +
                                "  and ww.[T.Doc] <> 'DM' and REPLACE(SUBSTRING(obs, len(obs) -5, 5),'(','') = 'dolar')   as uu";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable trae_4_semanas()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * FROM V_5_SEMANAS order by año ASC, semana ASC";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable ventas_ayer_anteayer(string where2, string desde, string hasta)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"	
		select sum(neto_dolar) as qq, 'AYER' as ayer, producto from  v_reporte_thx where fechafactura >= convert(datetime, '" + desde + "', 103) and fechafactura <= DATEADD(d,-1, (convert(datetime,'" + hasta + "', 103)) ) " + where2 +
        "	group by producto union all " +
      "  select sum(neto_dolar) as ee, 'ANTESAYER' as antesayer, producto  from  v_reporte_thx where fechafactura >= convert(datetime, '" + desde + "', 103) and fechafactura <= DATEADD(d,-2, (convert(datetime,'" + hasta + "', 103)) ) " + where2 +
      " 	   group by producto ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string eliminar_accion_prio(string id, string id_cobranza)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from Cobranza_Seguimiento_prioridad where ltrim(rtrim(id_cobranza)) = '" + id_cobranza.Trim() + "'; " +
                            " insert into Cobranza_Seguimiento_prioridad values (" + id.Trim() + ", '" + id_cobranza.Trim() + "')";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string trae_stuff_facturas(string factura)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"(SELECT        STUFF
 ((SELECT        CAST(',' AS varchar(MAX)) + CONVERT(varchar, X.num_factura_origen) FROM     Cobranza_Seguimiento X 
 WHERE  X.num_factura = '" + factura + "'   ORDER BY X.num_factura FOR XML PATH('')), 1, 1, ''))";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static double Facturación__ayer(string where, string dia)
        {
            double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"select isnull(sum(qwq),0) from  (select sum(neto_pesos)  as qwq  
                                from thx_v_reporte " + where + " and convert(datetime, fechafactura,103) = convert(datetime, '" + dia + "', 103)  group by rutcliente, periodo ) assa";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Double.Parse((cmd.ExecuteScalar().ToString()));
                conn.Close();
            }
            return scalar;


        }

        internal static double cant_clientes__ayer(string where, string dia)
        {
            double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select count(distinct(rutcliente)) from  (select rutcliente  from thx_v_reporte " + where + "  and convert(datetime, fechafactura,103) = convert(datetime, '" + dia + "', 103) group by rutcliente, periodo ) assa";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Double.Parse((cmd.ExecuteScalar().ToString()));
                conn.Close();
            }
            return scalar;
        }

        internal static DataTable traer_usd_cobro(string mes, string año)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * FROM cobro_usd where mes = " + mes + " and año = " + año;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string cuenta_banco(string factura)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select top 1 bankacct from ardoc where refnbr = '" + factura + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string guardar_valor_equivale(string v, object tex1, string text2)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from unidad_stock where cod_prod = '" + v + "' ; " +
                   " insert into unidad_stock values ('" + v + "', " + text2.Replace(",", ".") + ") ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable valor_prod_equivale(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select top 1 f.stkunit, (select equivale_kilos from unidad_stock where  rtrim(ltrim(f.invtid)) = rtrim(ltrim(cod_prod)) ) as valor from Stock_diario f where f.invtid = '" + v.Trim() + "'";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }
        internal static DataTable valor_prod_equivale_sp(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select top 1    f.stkunit, 
				                                d.equivale_kilos as valor,
				                                d.unidad_equivale
                                 from Stock_diario f 
	                                inner join unidad_stock_sp d
	                                   on rtrim(ltrim(f.invtid)) = rtrim(ltrim(d.cod_prod)) where f.invtid = '" + v.Trim() + "'";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string trae_stuff_facturas_de_cheque(string factura)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"(SELECT        STUFF
 ((SELECT        CAST(',' AS varchar(MAX)) + CONVERT(varchar, X.num_factura_origen) FROM     Cobranza_Seguimiento X 
 WHERE  X.num_factura = (select num_factura from cobranza_seguimiento where ISNUMERIC(num_factura) = 0 and num_factura_origen = '" + factura + "')     ORDER BY X.num_factura FOR XML PATH('')), 1, 1, ''))";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string guardar_usd_cobro(string mes, string año, string tasa, string valor_cobro)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from cobro_usd where mes =" + mes + " and año = " + año + " ; " +
                   " insert into cobro_usd values (" + mes + ", " + año + ", " + tasa + ", " + valor_cobro + ") ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable fechas_incorrectas(string mes, string año, string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"select * from [192.168.10.8].[SoprodiUSDapp].[dbo].vp_intran 
		                         where convert(varchar, year(trandate),103 ) + right('0000' + convert(varchar, month(trandate),103 ),2)   <> perpost
		                         and trandate >  EOMONTH ( convert(date, '01/" + mes.PadLeft(2, '0') + "/" + año + "',103) ) " +
                              "    and trandate <=  EOMONTH ( convert(date, '01/" + mes.PadLeft(2, '0') + "/" + año + "', 103) ,1 ) " + where.Replace("where 1=1", "");
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string eliminar_por_like_obs(string cheques_obs)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from Cobranza_Seguimiento where observacion like '%" + cheques_obs + "%'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string stuff_ids_pagos22222(string facturas_pagos)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"
   (SELECT        STUFF
 ((SELECT        CAST(',' AS varchar(MAX)) + CONVERT(varchar, X.id) FROM     Cobranza_Seguimiento X 
 WHERE  X.num_factura in ( " + facturas_pagos + "  )  and estado <> 'EN SEGUIMIENTO'     ORDER BY X.id FOR XML PATH('')), 1, 1, ''))";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string stuff_ids_cheques(string num_factura_origen)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"(SELECT        STUFF ((SELECT        CAST(',' AS varchar(MAX)) + CONVERT(varchar, X.id) FROM     Cobranza_Seguimiento X 
 WHERE  X.num_factura = (select num_factura from Cobranza_Seguimiento where estado='CHEQUE' and num_factura_origen = '" + num_factura_origen + "') ORDER BY X.id FOR XML PATH('')), 1, 1, ''))";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string stuff_ids_pagos(string num_factura_origen)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"(SELECT        STUFF
 ((SELECT        CAST(',' AS varchar(MAX)) + CONVERT(varchar, X.num_factura) FROM     Cobranza_Seguimiento X 
 WHERE  X.observacion like  '%" + num_factura_origen + "%' and estado = 'ABONO'  ORDER BY X.id FOR XML PATH('')), 1, 1, ''))";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string trae_ids_segui(string v, string fecha, string obs)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"(SELECT        STUFF
                             ((SELECT        CAST(',' AS varchar(MAX)) + CONVERT(varchar, X.id) FROM     Cobranza_Seguimiento X 
                             WHERE  X.num_factura in (" + v + ") and X.estado = 'ABONO' and convert(datetime, X.fecha,103) = convert(datetime, '" + fecha + "',103)  and observacion = '" + obs + "' ORDER BY X.id FOR XML PATH('')), 1, 1, ''))";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable trae_registro(string id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"	select 
                                       [id]
                                      ,[num_factura]
                                      ,[monto_doc]
                                      ,[rutcliente]
                                      ,[estado]
                                      ,[tipo_doc]
                                      ,[observacion]
                                      ,[usuario]
                                      ,[num_factura_origen]
                                      ,convert(varchar, [fecha],103) as fecha
                                      ,[fecha_venc]
                                      ,[estado_ingresado]
                                from cobranza_seguimiento where id = " + id;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string trae_num_factura_origen(string factura)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select num_factura_origen from Cobranza_Seguimiento where estado = 'ABONO' and num_factura = '" + factura + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string trae_obs_cobranza(string factura, string observacion)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select observacion from Cobranza_Seguimiento where estado = 'ABONO' and num_factura = '" + factura + "' and observacion = '" + observacion + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string eliminar_pago_fac(string factura, string fecha, string obser)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from COBRANZA_PAGOS " +
                " where id_cobranza in (" + factura + ") and  convert(datetime, fecha, 103) = convert(datetime, '" + fecha + "', 103) and descripcion = '" + obser + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id", factura);
                    //cmd.Parameters.AddWithValue("@periodo", periodo); 
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Delete Detalle: " + EX.Message;
                    }
                }
            }
        }

        internal static string eliminar_segui_id(string id)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from cobranza_seguimiento " +
                " where id in (" + id + ")";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("@id", id);
                    //cmd.Parameters.AddWithValue("@periodo", periodo);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Delete Detalle: " + EX.Message;
                    }
                }
            }
        }

        internal static double Facturación_Mes_dolar(string periodo, string where)
        {
            double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select isnull(sum(qwq),0) from  (select sum(neto_dolar)  as qwq  from thx_v_reporte " + where + " and Periodo in (" + periodo + ") group by rutcliente, periodo ) assa";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Double.Parse((cmd.ExecuteScalar().ToString()));
                conn.Close();
            }
            return scalar;
        }

        public static string encontrar(ref ReporteEntidad u)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT id_archivo, id_correo from webagente_det_correos_enviados where id_archivo = @id_archivo And id_correo = @id_correo";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                //cmd.Parameters.AddWithValue("@id_archivo", u.id_archivo);
                //cmd.Parameters.AddWithValue("@id_correo", u.id_correo);
                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        u = Load(reader);
                        return "OK";
                    }
                }
                catch (Exception EX)
                {
                    return "Error en Validar Detalle: " + EX.Message;
                }
            }
            return "No encontrado en DB";
        }

        internal static DataTable costo_almacenaje(string where, string selectedValue, string año, string bodega2)
        {


            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "selecT RTRIM(LTRIM(INVTID)) + ' - ' + descr AS Producto, SUM(QTY) as Stock, stkunit as Unidad,  " +
            "    case when stkunit = 'KGR' then  convert(varchar,SUM(QTY)) else convert(varchar,(select isnull(equivale_kilos,0.00) from unidad_stock where cod_prod = invtid) * SUM(QTY)) end as Kilos " +
            "   ,'' as 'Fecha de inicio', '' as 'Cobro a la fecha' from STOCK_DIARIO where   " +
            "   convert(datetime , trandate , 103) <= convert(datetime, EOMONTH('" + selectedValue + "/01/" + año + "'),103) " + where + "  GROUP BY invtid, stkunit, descr";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;

        }

        internal static DataTable listar_ALL_productos_thx(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select distinct(producto), descproducto from V_REPORTE_THX " + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string accion_prioridad(string v)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select count(*) from Cobranza_Seguimiento_prioridad where id = " + v;
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        public static DataTable Get_vendedor(string desde, string hasta)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT distinct(ltrim(rtrim(([CodVendedor])))) as cod_vend,ltrim(rtrim(([Vendedor]))) as nom_vend FROM[NEW_THX].[dbo].[thx_v_reporte] " +
                             "   where codvendedor <> '' and codvendedor <> '*' and  producto >= '1000' and producto <= '9999' and FechaFactura >= CONVERT(datetime,'" + desde + "', 103) " +
                            " and FechaFactura <= CONVERT(datetime,'" + hasta + "',103) order by ltrim(rtrim(([Vendedor])))";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static string insert_datos_excel(REPORTE_EXCEL_COSTOS.excel_f ex)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"
                                delete from Stock_Excel_2 where cod_producto = @cod_producto and fecha = @fecha;
                                INSERT INTO [dbo].[Stock_Excel_2]
                                       (c_f
                                       ,bod_LZ
                                       ,PUERTO
                                       ,bod_LA
                                       ,arica
                                       ,cod_producto
                                       ,producto
                                       ,pack
                                       ,cajas_pallet
                                       ,cajas_camion
                                       ,entrega_directa_V_RM       
                                       ,in_out_cm_qta
                                       ,quillota
                                       ,bod_LZ_vent
                                       ,arica_vent
                                       ,iquique
                                       ,bod_LA_vent
                                       ,reparto_RM_V
                                       ,fecha)
                         VALUES
                               (@c_f
                               ,@bod_LZ
                               ,@PUERTO
                               ,@bod_LA
                               ,@arica
                               ,@cod_producto
                               ,@producto
                               ,@pack
                               ,@cajas_pallet
                               ,@cajas_camion
                               ,@entrega_directa_V_RM
                               ,@in_out_cm_qta
                               ,@quillota
                               ,@bod_LZ_vent
                               ,@arica_vent
                               ,@iquique
                               ,@bod_LA_vent
                               ,@reparto_RM_V
                               ,@fecha);";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@c_f", ex.c_f);
                    cmd.Parameters.AddWithValue("@bod_LZ", ex.bod_LZ);
                    cmd.Parameters.AddWithValue("@PUERTO", ex.PUERTO);
                    cmd.Parameters.AddWithValue("@bod_LA", ex.bod_LA);
                    cmd.Parameters.AddWithValue("@arica", ex.arica);
                    cmd.Parameters.AddWithValue("@cod_producto", ex.cod_producto);
                    cmd.Parameters.AddWithValue("@producto", ex.producto);
                    cmd.Parameters.AddWithValue("@pack", ex.pack);
                    cmd.Parameters.AddWithValue("@cajas_pallet", ex.cajas_pallet);
                    cmd.Parameters.AddWithValue("@cajas_camion", ex.cajas_camion);
                    cmd.Parameters.AddWithValue("@entrega_directa_V_RM", ex.entrega_directa_V_RM);

                    cmd.Parameters.AddWithValue("@in_out_cm_qta", ex.in_out_cm_qta);
                    cmd.Parameters.AddWithValue("@quillota", ex.quillota);

                    cmd.Parameters.AddWithValue("@bod_LZ_vent", ex.bod_LZ_vent);
                    cmd.Parameters.AddWithValue("@arica_vent", ex.arica_vent);
                    cmd.Parameters.AddWithValue("@iquique", ex.iquique);
                    cmd.Parameters.AddWithValue("@bod_LA_vent", ex.bod_LA_vent);
                    cmd.Parameters.AddWithValue("@reparto_RM_V", ex.reparto_RM_V);
                    cmd.Parameters.AddWithValue("@fecha", Convert.ToDateTime(ex.fecha));
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error (1): " + EX.Message;
                    }
                }
            }
        }

        internal static DataTable clientes_periodo_anterior(string where5, string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT sum(neto_pesos) as neto_pesos, ltrim(rtrim(nombrecliente)) as nombrecliente, rutcliente 
                                FROM[NEW_THX].[dbo].[thx_v_reporte] " + where5 + " and ltrim(rtrim(rutcliente)) not in  " +
                            "	(select rutcliente from[NEW_THX].[dbo].[thx_v_reporte] " + where + " group by rutcliente) and producto <> 'FLETE'  and producto <> 'DESCARGA'  and producto <> 'FLETECAJA'  " +
                         "      group by nombrecliente, rutcliente order by nombrecliente ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable productos_stock_ventas(string where_stock)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select descproducto,producto  from THX_v_reporte " + where_stock + " and user1 <> 'Granos' and producto <> 'FLETE'  and producto <> 'DESCARGA'  and producto <> 'FLETECAJA'    group by descproducto, producto order by sum(neto_pesos) desc";


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable productos_stock2(string where, string where_stock)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select descr, invtid,  " +
                             "    (select sum(neto_pesos) from THX_v_reporte  " + where +
                             "    and user1 <> 'Granos' and invtid = producto)  as ewew" +
                               "  from Stock_diario b where b.glclassid in ('ABAR', 'MANI') and " +
                           "    (select sum(Qty) from Stock_diario" +
                            "     where invtid = b.invtid and " + where_stock +
                             "    ) >= 0  group by descr, invtid order by ewew desc";


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable facturas_gestionadas(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select * from v_flujo_cobranza where convert(datetime,[f.pago],103) = convert(datetime, '" + v + "', 103) order by nombcliente";


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable stock_facturado(string desde_1, string desde_2, string where, string fecha_compra)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select t.númfactura as 'Nº Factura', CONVERT(varchar, t.fechafactura ,103) as Fecha, t.nombrecliente as 'Nomb Cliente', t.vendedor as 'Vendedor', t.producto as 'Cod Prod', t.descproducto as 'Prod', t.cantidad as 'Cantidad', rtrim(ltrim(t.um)) as 'Unidad', dbo.F_Separador_miles(t.neto_pesos) as 'SubTotal(Pesos)', (select top 1 ñ.tipo_cambio from [192.168.10.8].[SoprodiUSDapp].[dbo].v_reporte ñ where ñ.númfactura = t.númfactura) as 'TipoCambio', t.neto_dolar as 'SubTotal(Dolar)','' as ValorUni , t.bodega as 'Bodega'
                    , dbo.F_CostoExcel(t.bodega,t.producto,t.fechafactura) as CostoExcel , " +
           //            case  
           //           when rtrim(ltrim(t.bodega)) = 'ZARATESOP' or rtrim(ltrim(t.bodega)) = 'SITRANS' or rtrim(ltrim(t.bodega)) = 'ABARROTES' 
           //           THEN(select top 1 j.bod_usd from stock_excel j where j.cod_producto = rtrim(ltrim(t.producto)) and j.fecha=(select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.producto)) " + desde_2 + ") group by j.fecha, j.bod_usd   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
           //"            when rtrim(ltrim(t.bodega)) = 'LOVALLEDOR' or rtrim(ltrim(t.bodega)) = 'AGUNSOP' or  rtrim(ltrim(t.bodega)) = 'LOGINSA'  or  rtrim(ltrim(t.bodega)) = 'LOGINSASOP'    " +
           //"           THEN(select  top 1 j.cm_stgo from stock_excel j where j.cod_producto = rtrim(ltrim(t.producto)) and j.fecha=(select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.producto)) " + desde_2 + ") group by j.fecha, j.cm_stgo   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
           //"           when rtrim(ltrim(t.bodega)) = 'QUILLOTSOP' or rtrim(ltrim(t.bodega)) = 'LOCAL1'" +
           //"            THEN(select  top 1 j.cm_qta from stock_excel j where j.cod_producto = rtrim(ltrim(t.producto)) and j.fecha=(select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.producto)) " + desde_2 + ") group by j.fecha, j.cm_qta   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
           //"            when rtrim(ltrim(t.bodega)) = 'QUILLOTA1' " +
           //"            THEN(select  top 1 j.cm_qta from  stock_excel j where j.cod_producto = rtrim(ltrim(t.producto)) and j.fecha=(select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.producto)) " + desde_2 + ") group by j.fecha, j.cm_qta   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
           //"           when rtrim(ltrim(t.bodega)) = 'ARGENTINA' " +
           //"           THEN(select  top 1 j.bod_usd from  stock_excel j where j.cod_producto = rtrim(ltrim(t.producto)) and j.fecha=(select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.producto)) " + desde_2 + ") group by j.fecha, j.bod_usd   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
           //"            when rtrim(ltrim(t.bodega)) = 'ARICASOP' " +
           //"           THEN(select top 1  j.arica from  stock_excel j where j.cod_producto = rtrim(ltrim(t.producto)) and j.fecha=(select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.producto)) " + desde_2 + ") group by j.fecha, j.arica   having j.fecha = max(j.fecha) order by j.fecha asc  ) " +
           //"             when rtrim(ltrim(t.bodega)) = 'ARICA1SOP' " +
           //"          THEN(select top 1  j.arica  from stock_excel j where j.cod_producto = rtrim(ltrim(t.producto)) and j.fecha=(select max(f.fecha) from stock_excel f where f.cod_producto = rtrim(ltrim(t.producto)) " + desde_2 + ") group by j.fecha, j.arica   having j.fecha = max(j.fecha) order by j.fecha asc  ) end, " +

           "            (select (select sum(c.pesoslc)   from stock_compra c where c.PONbr = " +
           "              (select  max(u.PONbr) from stock_compra_gen u " +
            "                             where t.producto = u.invtid and   u.RcptDate = " +
            "           (select max(x.rcptdate) from Stock_Compra_gen x where t.producto = x.invtid " +
          "              " + fecha_compra + "))) * " +
         "               (select top 1 u.UnitCost from stock_compra_gen u " +
       "                                    where t.producto = u.invtid and u.RcptDate = " +
       "              (select max(x.rcptdate) from Stock_Compra_gen x where t.producto = x.invtid " +
       "                 " + fecha_compra + "))  / (select sum(valor) from " +
       "                (select top 1 valor = case " +
       "                 when TranType = 'X' then  CuryExtCost * -1  else  CuryExtCost  end from Stock_Compra_gen " +
      "                 WHERE PONbr = (select max(u.PONbr) from stock_compra_gen u where t.producto = u.invtid and   u.RcptDate = " +
       "                (select max(x.rcptdate) from Stock_Compra_gen x where t.producto = x.invtid " +
       "                and x.RcptDate  <= convert(datetime, t.fechafactura, 103) )) ) as ww) ) as 'CostoImport',  " +
          " (select top 1 u.UnitCost as suma from stock_compra_gen u where t.producto = u.invtid and  " +
          " u.RcptDate = (select max(x.rcptdate) from Stock_Compra_gen x where t.producto = x.invtid  and x.RcptDate  <= convert(datetime, t.fechafactura, 103) ))  as Compra " +
          ",  (select max(x.rcptdate) from Stock_Compra_gen x where t.producto = x.invtid " + fecha_compra + ") as FechaCompra ,  (select top 1 u.RcptUnitDescr as suma from stock_compra_gen u where t.producto = u.invtid and  " +
          " u.RcptDate = (select max(x.rcptdate) from Stock_Compra_gen x where t.producto = x.invtid " + fecha_compra + ")) " +
         " as UnidadCompra " +
 "                 ,'' as 'Utilidad(Excel)' , '' as 'Porc(Excel)', '' as 'Utilidad(Compra)', '' as 'Porc(Compra)', year(t.fechafactura)  from thx_v_reporte t  where 1=1 and user1 <> 'Granos'" + where;



                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);

                ap.Fill(dt);
            }
            return dt;
        }

        internal static string prom_productos_por_factura(string periodo_apreguntar, string where)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select ROUND(AVG(CAST(promedio_productos AS FLOAT)), 2) as prom from 
                                        (select count(distinct(producto)) as promedio_productos  from THX_v_reporte " + where + "  and Periodo in (" + periodo_apreguntar + ")  group by númfactura     ) as selected";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable facturas_solomon(string v)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"	select nombrecliente as NombCliente, vendedor as Vend, descr  as descri, factura as 'Nº Factura' , convert(varchar, fecha_trans,103) as 'F.Trans'
	                                , tipo_credi  as 'T.Credi', convert(varchar, fecha_venc,103) as 'F.Pago', '' as Peso, '' as Dolar, 

	                                Monto = 
	                                case 
		                                when tipo_moneda = 'USD'
			                                then saldo_dolar
		                                when tipo_moneda = 'PESO'
			                                then saldo
	                                 end ,
	                                 Moneda = 
	                                 case 
		                                when tipo_moneda = 'USD'
			                                then 'dolar'
		                                when tipo_moneda = 'PESO'
			                                then 'peso'
	                                 end , ''  as id
	                                from V_COBRANZA where fecha_venc =  convert(datetime, '" + v + "',103) and tipo_doc = 'IN' ORDER BY NOMBCLIENTE";



                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }


        public static DataTable Get_clientes(string vendedores)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT distinct(ltrim(rtrim(([rutcliente])))) as rut_cliente,ltrim(rtrim(([nombrecliente]))) as nom_cliente 
                                FROM[NEW_THX].[dbo].[thx_v_reporte]
                                  where codvendedor <> '' and codvendedor <> '*' and  producto >= '1000' and producto <= '9999' and 
		                                codvendedor in (" + vendedores + ") order by ltrim(rtrim(([nombrecliente])))";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        public static DataTable Get_productos(string clientes, string vendedores)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT distinct(ltrim(rtrim((producto)))) as cod_producto,ltrim(rtrim((descproducto))) as nom_producto 
                                FROM[NEW_THX].[dbo].[thx_v_reporte]
                                  where 
		                                rutcliente in (" + clientes + ") and codvendedor in (" + vendedores + ") order by ltrim(rtrim((descproducto)))";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        public static String periodos()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT STUFF ((SELECT  CAST(', ' AS varchar(MAX)) + CONVERT(varchar, Periodo) " +
                            "FROM thx_v_reporte " +
                            "where FechaFactura > '01/01/2015' GROUP BY Periodo " +
                            "ORDER BY Periodo FOR XML PATH('')), 1, 1, '') AS PERIODOS";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt.Rows[0][0].ToString();
        }

        public static DataTable Get_Resumen(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" SELECT Vendedor, ltrim(rtrim(nombrecliente)) as nombrecliente , DescProducto,cast(Periodo as bigint)  as Periodo, isnull(  cast(sum(Neto_Pesos) as bigint) ,0) as total2" +
                " FROM thx_v_reporte " + where +
                " group by Vendedor, Periodo, NombreCliente, DescProducto order by periodo asc, ltrim(rtrim(nombrecliente))  asc";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            //return Pivot(dt, dt.Columns["Periodo"], dt.Columns["total2"]);

            _SourceTable = dt;

            return PivotData("NombreCliente", "total2",
               AggregateFunction.Sum, "Periodo");
        }

        public static DataTable PIVOT_EXCEL(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select c_f, bod_usd, cm_stgo, cm_qta, arica, cod_producto from stock_excel " + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            //return Pivot(dt, dt.Columns["Periodo"], dt.Columns["total2"]);

            _SourceTable = dt;

            return PivotData("cod_producto", "cod_producto",
               AggregateFunction.First, "bod_usd", "c_f", "cm_stgo", "cm_qta", "arica");
        }

        public static DataTable Pivot(DataTable dt, DataColumn pivotColumn, DataColumn pivotValue)
        {

            try
            {
                if (pivotColumn.DataType == typeof(Int32))
                {
                    pivotColumn.DataType = typeof(Int64);
                }
            }
            catch { }
            // find primary key columns 
            //(i.e. everything but pivot column and pivot value)
            DataTable temp = dt.Copy();
            temp.Columns.Remove(pivotColumn.ColumnName);
            temp.Columns.Remove(pivotValue.ColumnName);
            string[] pkColumnNames = temp.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToArray();

            // prep results table
            DataTable result = temp.DefaultView.ToTable(true, pkColumnNames).Copy();
            result.PrimaryKey = result.Columns.Cast<DataColumn>().ToArray();
            dt.AsEnumerable()
                .Select(r => r[pivotColumn.ColumnName].ToString())
                .Distinct().ToList()
                .ForEach(c => result.Columns.Add(c, pivotColumn.DataType));

            // load it
            foreach (DataRow row in dt.Rows)
            {
                // find row to update
                DataRow aggRow = result.Rows.Find(
                    pkColumnNames
                        .Select(c => row[c])
                        .ToArray());
                // the aggregate used here is LATEST 
                // adjust the next line if you want (SUM, MAX, etc...)
                aggRow[row[pivotColumn.ColumnName].ToString()] = row[pivotValue.ColumnName];

            }

            return result;
        }

        public static DataTable PivotData(string RowField, string DataField,
       AggregateFunction Aggregate, params string[] ColumnFields)
        {
            DataTable dt = new DataTable();
            string Separator = ".";
            var RowList = (from x in _SourceTable.AsEnumerable()
                           select new { Name = x.Field<object>(RowField) }).Distinct();
            var ColList = (from x in _SourceTable.AsEnumerable()
                           select new
                           {
                               Name = ColumnFields.Select(n => x.Field<object>(n))
                                   .Aggregate((a, b) => a += Separator + b.ToString())
                           })
                               .Distinct();
            //.OrderBy(m => m.Name);

            dt.Columns.Add(RowField);
            foreach (var col in ColList)
            {
                dt.Columns.Add(col.Name.ToString());
            }

            foreach (var RowName in RowList)
            {
                DataRow row = dt.NewRow();
                row[RowField] = RowName.Name.ToString();
                foreach (var col in ColList)
                {
                    string strFilter = RowField + " = '" + RowName.Name + "'";
                    string[] strColValues =
                      col.Name.ToString().Split(Separator.ToCharArray(),
                                                StringSplitOptions.None);
                    for (int i = 0; i < ColumnFields.Length; i++)
                        strFilter += " and " + ColumnFields[i] +
                                     " = '" + strColValues[i] + "'";
                    row[col.Name.ToString()] = GetData(strFilter, DataField, Aggregate);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        internal static string ins_en_seg(List<SPVars> toSP = null)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string query = "";
                query += "INSERT INTO COBRANZA_SEGUIMIENTO ( ";
                query += "num_factura, ";
                query += "monto_doc, ";
                query += "rutcliente, ";
                query += "estado, ";
                query += "tipo_doc, ";
                query += "observacion, ";
                query += "usuario, ";
                query += "num_factura_origen, ";
                query += "fecha, ";
                query += "fecha_venc, estado_ingresado, ";
                query += "aux1, aux2,aux3 ";
                query += ") VALUES ( ";
                query += "@_num_factura, ";
                query += "@_monto_doc, ";
                query += "@_rutcliente, ";
                query += "@_estado, ";
                query += "@_tipo_doc, ";
                query += "@_observacion, ";
                query += "@_usuario, ";
                query += "@_num_factura_origen, ";
                query += "CONVERT(datetime, @_fecha, 103), ";
                query += "CONVERT(datetime, @_fecha_venc, 103), 0, ";
                query += "@_tcamb, ";
                query += "@_tobs, ";
                query += "@_ttdolar ";
                query += "); ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    //cmd.Parameters.AddWithValue("@factura", factura);
                    //cmd.Parameters.AddWithValue("@tipo_pago", estado);
                    foreach (SPVars ob in toSP)
                    {
                        //comando.Parameters.AddWithValue("@" + ob.nombre, ob.valor);
                        cmd.Parameters.AddWithValue("@" + ob.nombre, ob.valor);
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "error";
                    }
                }
            }
        }

        internal static string tipo_doc(string fact)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT top 1 tipo_doc  FROM V_COBRANZA WHERE  FACTURA = '" + fact + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        public enum AggregateFunction
        {
            Count = 1,
            Sum = 2,
            First = 3,
            Last = 4,
            Average = 5,
            Max = 6,
            Min = 7,
            Exists = 8
        }

        public static object GetData(string Filter, string DataField, AggregateFunction Aggregate)
        {
            try
            {
                DataRow[] FilteredRows = _SourceTable.Select(Filter);
                object[] objList =
                 FilteredRows.Select(x => x.Field<object>(DataField)).ToArray();

                switch (Aggregate)
                {
                    case AggregateFunction.Average:
                        return GetAverage(objList);
                    case AggregateFunction.Count:
                        return objList.Count();
                    case AggregateFunction.Exists:
                        return (objList.Count() == 0) ? "False" : "True";
                    case AggregateFunction.First:
                        return GetFirst(objList);
                    case AggregateFunction.Last:
                        return GetLast(objList);
                    case AggregateFunction.Max:
                        return GetMax(objList);
                    case AggregateFunction.Min:
                        return GetMin(objList);
                    case AggregateFunction.Sum:
                        return GetSum(objList);
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                return "#Error";
            }
            return null;
        }

        internal static string saldo_dolar(string fact)
        {

            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT saldo_dolar AS saldolar  FROM V_COBRANZA WHERE  FACTURA = '" + fact + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static object saldo_dolar_multiguia(string num_factura)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT origdocamt as saldolar  FROM cobranza_full WHERE  parent = '" + num_factura + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string insert_tipo_pago(string factura, string estado)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into cobranza_tipopago (factura, tipo_pago) " +
                "values (@factura, @tipo_pago)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@factura", factura);
                    cmd.Parameters.AddWithValue("@tipo_pago", estado);


                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Insert Botones: " + EX.Message;
                    }
                }
            }
        }

        internal static string delete_tipo_pago(string factura)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from cobranza_tipopago " +
                " where factura = @factura";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@factura", factura);
                    //cmd.Parameters.AddWithValue("@periodo", periodo);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Delete Detalle: " + EX.Message;
                    }
                }
            }
        }

        internal static string saldo_peso(string fact)
        {

            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT saldo  FROM V_COBRANZA WHERE  FACTURA = '" + fact + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        public static object GetAverage(object[] objList)
        {
            return objList.Count() == 0 ? null : (object)(Convert.ToDecimal(GetSum(objList)) / objList.Count());
        }

        internal static string delete_agendado(string num_factura)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from cobranza_pagos " +
                " where id_cobranza = @id_cobranza And tipo_doc = 'temporal' ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id_cobranza", num_factura);
                    //cmd.Parameters.AddWithValue("@periodo", periodo);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Delete Detalle: " + EX.Message;
                    }
                }
            }
        }

        public static object GetSum(object[] objList)
        {
            return objList.Count() == 0 ? null : (object)(objList.Aggregate(new decimal(), (x, y) => x + Convert.ToDecimal(y)));
        }

        public static object GetFirst(object[] objList)
        {
            return (objList.Count() == 0) ? null : objList.First();
        }

        public static object GetLast(object[] objList)
        {
            return (objList.Count() == 0) ? null : objList.Last();
        }

        public static object GetMax(object[] objList)
        {
            return (objList.Count() == 0) ? null : objList.Max();
        }
        public static object GetMin(object[] objList)
        {
            return (objList.Count() == 0) ? null : objList.Min();
        }

        public static DataTable Get_ALL_clientes()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT distinct(ltrim(rtrim(([rutcliente])))) as rut_cliente,ltrim(rtrim(([nombrecliente]))) as nom_cliente
                                FROM[NEW_THX].[dbo].[thx_v_reporte]
                                  where codvendedor <> '' and codvendedor <> '*' and  producto >= '1000' and producto <= '9999' order by ltrim(rtrim(([nombrecliente])))";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }


        public static DataTable Get_ALL_clientes2(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT distinct(ltrim(rtrim(([rutcliente])))) as rut_cliente,ltrim(rtrim(([nombrecliente]))) as nom_cliente
                                FROM[NEW_THX].[dbo].[thx_v_reporte]
                                " + where + " group by rutcliente, nombrecliente   order by ltrim(rtrim(([nombrecliente])))";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }


        public static DataTable ventas_matriz(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT neto_pesos, ltrim(rtrim(rutcliente)) as rutcliente, ltrim(rtrim(producto)) as producto
                                FROM[NEW_THX].[dbo].[thx_v_reporte]
                                " + where + "";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        public static DataTable Get_ALL_productos()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT distinct(ltrim(rtrim((producto)))) as cod_producto,ltrim(rtrim((descproducto))) as nom_producto 
                                FROM[NEW_THX].[dbo].[thx_v_reporte]
                                  where codvendedor <> '' and codvendedor <> '*' and  producto >= '1000' and producto <= '9999' order by ltrim(rtrim((descproducto)))";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        public static DataTable Get_productos_cliente(string clientes)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT distinct(ltrim(rtrim((producto)))) as cod_producto,ltrim(rtrim((descproducto))) as nom_producto 
                                FROM[NEW_THX].[dbo].[thx_v_reporte]
                                  where 
		                                rutcliente in (" + clientes + ") order by ltrim(rtrim((descproducto)))";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }


        public static DataTable listar_periodos(string periodos)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT '' as titulo_aux, Periodo
                FROM thx_v_reporte  where periodo in (" + periodos + ") and codvendedor in ('JH014') " +
                "  group by Periodo order by periodo desc";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        public static string listar_periodos_(string where)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"(SELECT STUFF((SELECT  CAST(',' AS varchar(MAX)) + CONVERT(varchar, Periodo) 
                                                                  FROM            thx_v_reporte " + where +
                                                                  " group BY Periodo order by periodo desc FOR XML PATH('')), 1, 1, ''))";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            return scalar;
        }

        public static DataTable Get_Resumen_PROD(string periodos, string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"             
                             SELECT customer.slsperId as codvendedor,rutcliente,salesperson.Name as vendedor,  ltrim(rtrim(nombrecliente)) as NombreCliente ,
                                                 cast(Periodo as bigint) as Periodo , isnull(  cast(sum(Neto_Pesos) as bigint) ,0)  as total2
                                FROM thx_v_reporte 
                                    inner join customer on customer.custid = thx_v_reporte.rutcliente 
                                    inner join salesperson on customer.slsperId = salesperson.slsperId " + where.Replace("user1", "thx_v_reporte.user1") + " and Periodo in (" + periodos + ") " +
                            " group by  Periodo,rutcliente, NombreCliente, customer.slsperId, salesperson.Name" +
                            " order by periodo desc, ltrim(rtrim(nombrecliente))  asc ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }

            return Pivot(dt, dt.Columns["Periodo"], dt.Columns["total2"]);
        }

        internal static DataTable cheques_por_cobrar(string p)
        {


            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select rutcliente as RutCliente, nombrecliente as NombCliente, factura as 'Nº Factura', nombrevendedor as NombVendedor, tipo_doc as TipoDocum,descr as 'Descripción', num_ref as NumRef,
                 CONVERT(VARCHAR,fecha_trans,103) as FechaTransc, CONVERT(VARCHAR,fecha_venc,103) as FechaVenc, tipo_credi as TipoCred, 
                (select top 1 lc from V_REPORTE_THX where rutcliente = rutcliente ) as 'L.Crédito', Monto = case when tipo_moneda = 'USD'
				then	
				(select user3 from [192.168.10.8].[SoprodiUSDapp].[dbo].[ardoc] where refnbr = factura and doctype = 'DM')
				when tipo_moneda = 'PESO'
				then monto_doc
				end
                  from v_cobranza where estado_doc <> 0 and tipo_doc = 'DM' and isnumeric(factura) <> 1 and CONVERT(VARCHAR,fecha_venc,103)  = '" + p + "'";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;

        }

        public static double Facturación_Mes(string periodo, string where)
        {
            double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select isnull(sum(qwq),0) from  (select sum(neto_pesos)  as qwq  from thx_v_reporte " + where + " and Periodo in (" + periodo + ") group by rutcliente, periodo ) assa";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Double.Parse((cmd.ExecuteScalar().ToString()));
                conn.Close();
            }
            return scalar;
        }

        public static string listar_clientes_(string vendedores)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"(SELECT STUFF((SELECT  CAST(',' AS varchar(MAX)) + CONVERT(varchar, ltrim(rtrim((RUTCLIENTE)))) 
                                                                  FROM            thx_v_reporte 
                                                                  where codvendedor in (" + vendedores + ") " +
                                                                  " group BY RUTCLIENTE FOR XML PATH('')), 1, 1, ''))";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            return scalar;
        }

        public static int _cltes_con_vta(string periodo, string where)
        {
            int scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select count(*) as clientes_con_venta from (select count(*) AS RowNumber from thx_v_reporte " + where + " and Periodo in (" + periodo + ") group by rutcliente, periodo ) a";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }


        public static int _cltes_con_vta2(string where)
        {
            int scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select count(rutcliente) as clientes_con_venta from thx_v_reporte " + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }

        public static List<long> Datos_para_percentil(string periodo, string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select isnull(sum(neto_pesos),0) as suma
								 from thx_v_reporte " + where + " and periodo in (" + periodo + ") group by rutcliente, periodo ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            List<long> s = dt.AsEnumerable().Select(x => Convert.ToInt64(x[0])).ToList();
            return s;
        }

        internal static string estimado_peso_cobranz_solom(string desde)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select ISNULL((select SUM(saldo) as qwer from v_cobranza a
                             where a.tipo_moneda = 'peso' and a.tipo_doc='IN' and a.estado_doc <> 0 and CONVERT(varchar, a.fecha_venc, 103) = @desde), '0') as qw";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@desde", desde);

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        internal static string estimado_dolar_cobranz_solom(string desde)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select ISNULL((select cast(SUM(saldo_dolar) as numeric(36,2)) from v_cobranza a
                             where a.tipo_moneda = 'USD' and a.tipo_doc='IN' and a.estado_doc <> 0 and CONVERT(varchar, a.fecha_venc, 103) = @desde), '0') as qw";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@desde", desde);

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        public static int cltes_sobre_este_percentil(string p, string where, Double percentil)
        {
            int scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"	 select count(*) as clientes_con_venta_percent from (select sum(neto_pesos) as suma from thx_v_reporte " + where + " and Periodo in (" + p + ") group by rutcliente, periodo  ) a where suma > " + percentil;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }

        public static Double sum_sobre_este_percentil(string periodo, string where, Double percentil)
        {
            Double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"	  select isnull(sum(suma),0) as clientes_con_venta  from (select sum(neto_pesos) as suma from thx_v_reporte " + where + " and Periodo in (" + periodo + ")  group by rutcliente, periodo  ) a where suma > " + percentil;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Double.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }

        public static Double COMPARATIVO_CLTE_VENTAS(string periodo_apreguntar, string periodo_anterior, string where)
        {
            Double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"
                     select count (rutcliente) as cont_a_repite from (" +
                    " select rutcliente from thx_v_reporte " + where + " and Periodo in (" + periodo_apreguntar + ")          " +
                    "	and rutcliente in (" +
                    "	select rutcliente from thx_v_reporte " + where + " and Periodo in (" + periodo_anterior + ") " +
                    "		group by rutcliente 		, periodo				)" +

                    "	 group by rutcliente  ) asas";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Double.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }

        public static double sum_a_recuperar(string periodo_apreguntar, string periodo_anterior, string where)
        {
            double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select isnull(sum (qq),0) as sum_recuperar from (
                    select a.rutcliente, sum(a2) as qq from (
                    select rutcliente, sum(neto_pesos) as a1 from thx_v_reporte " + where + " and Periodo in (" + periodo_apreguntar + ")  group by rutcliente, periodo) a " +
                    " right join ( " +
                     "  select rutcliente,  sum(neto_pesos) as a2 from thx_v_reporte " + where + " and Periodo in (" + periodo_anterior + ") group by rutcliente, periodo ) b " +
                     " on b.rutcliente = a.rutcliente where a.rutcliente is null  group by a.rutcliente ) a";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = double.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }



        public static int cont_si_repite(string periodo_apreguntar, string periodo_anterior, string where)
        {
            int scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select COUNT(a.rutcliente) from   (select rutcliente, isnull(sum(neto_pesos),0) as Venta from THX_v_reporte  " +
                "    " + where + " and Periodo in (" + periodo_anterior + ")   group by rutcliente  ) a " +
                "		 left join " +
                "		( select rutcliente, isnull(sum(neto_pesos),0) as Venta from THX_v_reporte   " + where + " and Periodo in (" + periodo_apreguntar + ") group by rutcliente  ) qw  " +
                "		  on a.rutcliente = qw.rutcliente where qw.rutcliente Is NULL ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }

        //aca
        public static Double sum_no_repite(string periodo_apreguntar, string periodo_anterior, string where)
        {
            Double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();


                string sql = @" select isnull(sum(a1),0) as sum_a_si_repite_mes_anterior from " +
                        " ( select rutcliente, SUM(neto_pesos) as a1 from thx_v_reporte  " + where + " and Periodo in (" + periodo_apreguntar + ")  group by rutcliente, codvendedor 	, periodo         ) a left join   " +
                        " ( 	select rutcliente, SUM(neto_pesos) as a2 from thx_v_reporte  " + where + " and Periodo in (" + periodo_anterior + ")  group by rutcliente, codvendedor 	, periodo	)  qw   " +
                        " on a.rutcliente = qw.rutcliente where qw.rutcliente Is NULL  ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Double.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }


        public static Double sum_si_repite(string periodo_apreguntar, string periodo_anterior, string where)
        {
            Double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select isnull( sum (qwe),0) as sum_si_repite from ( " +
                "    select rutcliente, sum(neto_pesos) as qwe from thx_v_reporte " + where + " and Periodo in (" + periodo_apreguntar + ") " +
                "		and rutcliente in ( " +
                "		select rutcliente from thx_v_reporte " + where + " and Periodo in (" + periodo_anterior + ")  " +
                "			group by rutcliente, vendedor			" +
                "		)" +
                "		 group by rutcliente, vendedor	 " +
               "     ) asas";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Double.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }
        public static Double sum_si_repite_actual(string periodo_apreguntar, string periodo_anterior, string where)
        {
            Double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select isnull(sum(a1),0) as sum_a_si_repite_mes_anterior from ( " +
                    "  select sum(neto_pesos) as a1 from thx_v_reporte " + where + " and Periodo in (" + periodo_anterior + ")         " +
                      "	and rutcliente in ( " +
                      "	select rutcliente from thx_v_reporte " + where + "  and Periodo in (" + periodo_apreguntar + ") " +
                      "		group by rutcliente, codvendedor 	, periodo				)" +
                      "	 group by rutcliente, codvendedor, periodo  " +
                    "  ) asas ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Double.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }


        public static DataTable listar_clientes(string desde, string hasta)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT distinct(ltrim(rtrim(([rutcliente])))) as rut_cliente,ltrim(rtrim(([nombrecliente]))) as nom_cliente FROM[NEW_THX].[dbo].[thx_v_reporte] " +
                             "   where codvendedor <> '' and codvendedor <> '*' and  producto >= '1000' and producto <= '9999' and FechaFactura >= CONVERT(datetime,'" + desde + "', 103) " +
                            " and FechaFactura <= CONVERT(datetime,'" + hasta + "',103) order by ltrim(rtrim(([nombrecliente])))";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static Double cont_sin_x_2(string periodo_apreguntar, string periodo_anterior, string periodo_2_meses, string where)
        {
            Double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select COUNT(a.rutcliente) from " +
                            "	(select rutcliente, isnull(sum(neto_pesos),0) as Venta from THX_v_reporte  " + where + " and Periodo in (" + periodo_2_meses + ") group by rutcliente , ltrim(rtrim(nombrecliente)), vendedor  ) a " +
                            "	 left join " +
                            "	(" +
                            "   select rutcliente, isnull(sum(neto_pesos),0) as Venta from THX_v_reporte " + where + " and Periodo in (" + periodo_anterior + ", " + periodo_apreguntar + ") group by rutcliente , ltrim(rtrim(nombrecliente)), vendedor   ) qw  " +
                            "	    on a.rutcliente = qw.rutcliente where qw.rutcliente Is NULL ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Double.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }

        public static Double sum_sin_x_2(string periodo_apreguntar, string periodo_anterior, string periodo_2_meses, string where)
        {
            Double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select  isnull(sum(a.venta),0) from " +
                             "	(select rutcliente, isnull(sum(neto_pesos),0) as Venta from THX_v_reporte  " + where + " and Periodo in (" + periodo_2_meses + ") group by rutcliente , ltrim(rtrim(nombrecliente)), vendedor  ) a " +
                             "	 left join " +
                             "	(" +
                             "   select rutcliente, isnull(sum(neto_pesos),0) as Venta from THX_v_reporte " + where + " and Periodo in (" + periodo_anterior + ", " + periodo_apreguntar + ") group by rutcliente , ltrim(rtrim(nombrecliente)), vendedor     ) qw  " +
                             "	    on a.rutcliente = qw.rutcliente where qw.rutcliente Is NULL ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Double.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }

        public static Double cont_nuevo_cliente(string periodo_apreguntar, string where)
        {
            Double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select COUNT(a.rutcliente) from    " +
                            "	(select rutcliente from THX_v_reporte " + where + " and Periodo in (" + periodo_apreguntar + ") group by rutcliente) a	" +
                            "	  left join " +
                            " ( select rutcliente from THX_v_reporte " + where + "  and Periodo < " + periodo_apreguntar + "  group by rutcliente    ) qw        on a.rutcliente = qw.rutcliente where qw.rutcliente Is NULL ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Double.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }

        public static Double sum_nuevo_cliente(string periodo_apreguntar, string where)
        {
            Double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select  isnull(sum(a.venta),0) from    " +
               "	(select rutcliente, isnull(sum(neto_pesos),0) as venta from THX_v_reporte " + where + " and Periodo in (" + periodo_apreguntar + ") group by rutcliente) a	" +
               "	  left join " +
               " ( select rutcliente, isnull(sum(neto_pesos),0) as venta from THX_v_reporte " + where + "  and Periodo < " + periodo_apreguntar + "  group by rutcliente    ) qw        on a.rutcliente = qw.rutcliente where qw.rutcliente Is NULL ";


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Double.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }

        public static DataTable listar_clientes_nuevos(string periodo_mayor, string periodo_menor, string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"  select a.Venta, a.rutcliente, a.NombreCliente from   (select rutcliente, isnull(sum(neto_pesos),0) as Venta,  
                                    ltrim(rtrim(nombrecliente)) as NombreCliente from THX_v_reporte " +
                                   where +
                                  "  and Periodo in (" + periodo_mayor + ")  group by rutcliente , ltrim(rtrim(nombrecliente))  ) a left join " +
                                  "   ( select rutcliente, isnull(sum(neto_pesos),0) as Venta, " +
                                  " ltrim(rtrim(nombrecliente)) as NombreCliente from THX_v_reporte " + where +
                                  "  and Periodo not in " +
                                  "   (" + periodo_mayor + " ) group by rutcliente , ltrim(rtrim(nombrecliente))   ) qw " +
                                  "  on a.rutcliente = qw.rutcliente where qw.rutcliente Is NULL order by a.NombreCliente";


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static string listar_periodo_nuevo(string hasta)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"  select max(periodo) as periodo from ( select periodo from thx_v_reporte " + hasta + " group by periodo) as qwe";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            return scalar;
        }


        public static string listar_periodo_nuevo_2(string hasta)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"  select min(periodo) as periodo from ( select periodo from thx_v_reporte " + hasta + " group by periodo) as qwe";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            return scalar;
        }

        public static string total_nuevos(string periodo_para_nuevos, string periodo_menor, string where)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"  select sum(a.Venta) from   (select rutcliente, isnull(sum(neto_pesos),0) as Venta, 
                                    ltrim(rtrim(nombrecliente)) as NombreCliente from THX_v_reporte " +
                                     where +
                                    "  and Periodo in (" + periodo_para_nuevos + ")  group by rutcliente , ltrim(rtrim(nombrecliente))  ) a left join " +
                                    "   ( select rutcliente, isnull(sum(neto_pesos),0) as Venta, " +
                                    " ltrim(rtrim(nombrecliente)) as 'Nombre Cliente' from THX_v_reporte " + where +
                                    "  and Periodo not in " +
                                    "   (" + periodo_para_nuevos + " ) group by rutcliente , ltrim(rtrim(nombrecliente))   ) qw " +
                                    "  on a.rutcliente = qw.rutcliente where qw.rutcliente Is NULL ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            return scalar;
        }
        public static string cont_nuevos(string periodo_para_nuevos, string periodo_menor, string where)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"  select count(*) from   (select rutcliente, isnull(sum(neto_pesos),0) as Venta, 
                                    ltrim(rtrim(nombrecliente)) as NombreCliente from THX_v_reporte " +
                                       where +
                                      "  and Periodo in (" + periodo_para_nuevos + ")  group by rutcliente , ltrim(rtrim(nombrecliente))  ) a left join " +
                                      "   ( select rutcliente, isnull(sum(neto_pesos),0) as Venta, " +
                                      " ltrim(rtrim(nombrecliente)) as 'Nombre Cliente' from THX_v_reporte " + where +
                                      "  and Periodo not in " +
                                      "   (" + periodo_para_nuevos + " ) group by rutcliente , ltrim(rtrim(nombrecliente))   ) qw " +
                                      "  on a.rutcliente = qw.rutcliente where qw.rutcliente Is NULL ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            return scalar;
        }

        public static Double cont_sobre_este_percentil(string periodo, string where, Double percentil)
        {
            Double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select count(*) as clientes_con_venta from (select sum(neto_pesos) as suma from thx_v_reporte " + where + " and Periodo in (" + periodo + ") group by rutcliente, periodo) a where suma > " + percentil;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Double.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }






        public static double cont_nuevo_venta(string periodo_apreguntar, string periodo_anterior, string where)
        {
            Double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                //string sql = @"select isnull(count (qq),0) from ( " +
                //            "	select rutcliente as qq from thx_v_reporte " + where + " and Periodo = " + periodo_apreguntar + " and rutcliente not in (" +
                //            "	select rutcliente from thx_v_reporte " + where + " and Periodo < " + periodo_apreguntar + " group by rutcliente" +
                //            "	)  group by rutcliente " +
                //            " ) asd";

                string sql = @"select isnull(count (a.rutcliente),0) from  " +
                    " ( 	select rutcliente from thx_v_reporte  " + where + " and Periodo = " + periodo_apreguntar +
                    " group by rutcliente) a left join   " +
                    " (	select rutcliente from thx_v_reporte  " + where + " and Periodo < " + periodo_apreguntar +
                    " group by rutcliente	)   qw    on a.rutcliente = qw.rutcliente where qw.rutcliente Is NULL  ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Double.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }

        public static double sum_nueva_venta(string periodo_apreguntar, string periodo_anterior, string where)
        {
            Double scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select isnull(sum(a.venta),0) from 	(select rutcliente, isnull(sum(neto_pesos),0) as Venta from THX_v_reporte " + where + " and Periodo = " + periodo_apreguntar + " group by rutcliente , ltrim(rtrim(nombrecliente)), vendedor  ) a "
                             + "	 left join ("

                             + "   select rutcliente, isnull(sum(neto_pesos),0) as Venta from THX_v_reporte  " + where
                            + " and Periodo < " + periodo_apreguntar + " group by rutcliente , ltrim(rtrim(nombrecliente)), vendedor     ) qw  "
                             + "	    on a.rutcliente = qw.rutcliente where qw.rutcliente Is NULL";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Double.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }

        public static DataTable listar_vendedor_cm(string p1, string p2, string p3)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT distinct(ltrim(rtrim(([CodVendedor])))) as cod_vend,ltrim(rtrim(([Vendedor]))) as nom_vend FROM[NEW_THX].[dbo].[thx_v_reporte] " +
                             "   where codvendedor <> '' and codvendedor <> '*' and  producto >= '1000' and producto <= '9999' and FechaFactura >= CONVERT(datetime,'" + p1 + "', 103) " +
                            " and FechaFactura <= CONVERT(datetime,'" + p2 + "',103) and user1 = '" + p3 + "' order by ltrim(rtrim(([Vendedor])))";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static DataTable listar_vendedor_sala(string p1, string p2, string p3)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT distinct(ltrim(rtrim(([CodVendedor])))) as cod_vend,ltrim(rtrim(([Vendedor]))) as nom_vend FROM[NEW_THX].[dbo].[thx_v_reporte] " +
                             "   where codvendedor <> '' and codvendedor <> '*' and  producto >= '1000' and producto <= '9999' and FechaFactura >= CONVERT(datetime,'" + p1 + "', 103) " +
                            " and FechaFactura <= CONVERT(datetime,'" + p2 + "',103) and codvendedor like '%" + p3 + "%' order by ltrim(rtrim(([Vendedor])))";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static DataTable cargar_usuarios()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT *, (SELECT STUFF ((SELECT  CAST(', ' AS varchar(MAX)) + CONVERT(varchar, grupo_usuario) " +
                            "FROM usuarioweb_det " +
                            "where usuarioweb.cod_usuario = usuarioweb_det.cod_usuario GROUP BY grupo_usuario " +
                            "ORDER BY grupo_usuario FOR XML PATH('')), 1, 1, '')) AS grupos  from usuarioweb order by nombre_usuario asc";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static DataTable carga_grupos()
        {

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select ltrim(rtrim(user1)) as user1 from [192.168.10.8].[SoprodiUSDapp].[dbo].[Salesperson] where user1 <> '' group by user1 ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static DataTable carga_grupos(string desde, string hasta, string grupos)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select ltrim(rtrim(user1)) as user1 from thx_v_reporte where user1 <> '' and user1 in (" + grupos + ") " +
                   " and FechaFactura >= CONVERT(datetime,'" + desde + "', 103) and FechaFactura <= CONVERT(datetime,'" + hasta + "',103)   group by user1 ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static string grupos_usuario(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                //                string sql = @" (SELECT STUFF ((SELECT  CAST(', ' AS varchar(MAX)) + CONVERT(varchar, grupo_usuario) 
                //                            FROM usuarioweb_det 
                //                            where  usuarioweb_det.cod_usuario like '%" + p + "%' GROUP BY grupo_usuario  " +
                //                          "  ORDER BY grupo_usuario FOR XML PATH('')), 1, 1, '')) ";
                string sql = @"select grupos from usuarioweb where cod_usuario = '" + p + "'";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            return scalar;
        }

        public static DataTable listar_ALL_vendedores(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT distinct(ltrim(rtrim(([CodVendedor])))) as cod_vend,ltrim(rtrim(([Vendedor]))) as nom_vend FROM[NEW_THX].[dbo].[thx_v_reporte] " +
                             where + "    order by ltrim(rtrim(([Vendedor]))) ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static DataTable listar_prod_client(string vendedor, string cliente, string periodos, string user1)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" SELECT cast(Periodo as bigint) as Periodo, descproducto, isnull(  cast(sum(Neto_Pesos) as bigint) ,0)   as total2" +
                " FROM thx_v_reporte where codvendedor = '" + vendedor + "' and rutcliente = '" + cliente + "' and Periodo in (" + periodos + ") and user1 in (" + user1 + ")" +
                " group by Vendedor, Periodo, ltrim(rtrim(nombrecliente)) , descproducto order by periodo desc, ltrim(rtrim(nombrecliente))  asc ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            //_SourceTable = dt;

            //return PivotData("NombreCliente", "total2",
            //   AggregateFunction.Sum, "Periodo");
            return Pivot(dt, dt.Columns["Periodo"], dt.Columns["total2"]);
        }

        public static DataTable listar_ventas_SALA(string periodos, string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"
                                SELECT V.CODVENDEDOR,
		                                A.rutcliente,
		                                '' as VtaNo_Repite, 
		                                V.NOMBREVENDEDOR as 'Vendedor', 
		                                ltrim(rtrim(A.nombrecliente)) as NombreCliente , 
		                               cast(Periodo as bigint) as  Periodo, isnull(  cast(sum(Neto_Pesos) as bigint) ,0)    as total2
                                 FROM thx_v_reporte A 
	                                INNER JOIN V_CLIENTE_VENDEDOR_ASIGNADO V ON A.RUTCLIENTE = V.RUTCLIENTE " +
                                      where + " and A.periodo in (" + periodos + ")  " +
                                  " group by A.Periodo, ltrim(rtrim(A.nombrecliente)), A.rutcliente,  V.CODVENDEDOR, V.NOMBREVENDEDOR " +
                                  " order by A.periodo desc, ltrim(rtrim(A.nombrecliente))  asc ";

                string sql2 = @" SELECT codvendedor,rutcliente,'' as VtaNo_Repite, vendedor as 'Vendedor', ltrim(rtrim(nombrecliente)) as NombreCliente , Periodo, sum(Neto_Pesos)   as total2" +
                " FROM thx_v_reporte " + where + " and Periodo in (" + periodos + ") " +
                " group by Periodo, ltrim(rtrim(nombrecliente)), vendedor,codvendedor,rutcliente order by periodo desc, ltrim(rtrim(nombrecliente))  asc ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            //_SourceTable = dt;
            //return PivotData("NombreCliente", "total2",
            //   AggregateFunction.Sum, "Periodo");
            return Pivot(dt, dt.Columns["Periodo"], dt.Columns["total2"]);
        }

        public static string es_su_pass(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" Select clave from usuarioweb where cod_usuario = '" + p + "'";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            return scalar;
        }

        public static string esvendedor(string usuario)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" Select tipo_usuario from usuarioweb where cod_usuario = '" + usuario + "'";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            return scalar;
        }

        public static string grupos_usuario_v_report(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" Select top 1 user1 from thx_v_reporte where codvendedor like '%" + p + "%'";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                try
                {
                    scalar = cmd.ExecuteScalar().ToString();
                }
                catch { }
                conn.Close();
            }
            return scalar;
        }

        public static int es_su_tipo(string USER)
        {
            int scalar = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" Select tipo_usuario from usuarioweb where cod_usuario = '" + USER + "'";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                scalar = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                conn.Close();
            }
            return scalar;
        }

        public static string GUARDA_BOTON(int boton, string usuario, string fecha)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into usuarios_tracking (id_boton, usuario, fecha) " +
                "values (@id_boton, @usuario, @fecha)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id_boton", boton);
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@fecha", fecha);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Insert Botones: " + EX.Message;
                    }
                }
            }
        }

        public static string delete_precalcular2(string cod_usuario, string periodo)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from precalcularreporte2 " +
                " where cod_usuario = @cod_usuario And nombre1 = @periodo or nombre2 =  @periodo ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_usuario", cod_usuario);
                    cmd.Parameters.AddWithValue("@periodo", periodo);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Delete Detalle: " + EX.Message;
                    }
                }
            }
        }

        public static DataTable listar_usuarios()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT * from usuarioweb ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static string delete_precalcular(string cod_usuario, string periodo)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from precalcularreporte " +
                " where cod_usuario = @cod_usuario And nombre1 = @periodo or nombre2 =  @periodo ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_usuario", cod_usuario);
                    cmd.Parameters.AddWithValue("@periodo", periodo);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Delete Detalle: " + EX.Message;
                    }
                }
            }
        }

        public static string insert_precalcular_x_factor(string cod_usuario, string nombre_factor, string factor1, string factor2, string periodo1, string periodo2, string total_g)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into precalcularreporte (cod_usuario, nombre_campo, factor1, factor2, nombre1, nombre2, total_g) " +
                "values (@cod_usuario, @nombre_campo, @factor1, @factor2, @nombre1, @nombre2, @total_g)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_usuario", cod_usuario);
                    cmd.Parameters.AddWithValue("@nombre_campo", nombre_factor);
                    cmd.Parameters.AddWithValue("@factor1", factor1);
                    cmd.Parameters.AddWithValue("@factor2", factor2);
                    cmd.Parameters.AddWithValue("@nombre1", periodo1);
                    cmd.Parameters.AddWithValue("@nombre2", periodo2);
                    cmd.Parameters.AddWithValue("@total_g", total_g);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Insert Detalle: " + EX.Message;
                    }
                }
            }
        }

        public static string insert_precalcular_x_cliente(string cod_usuario, string vendedor, string cliente, string factor1, string factor2, string periodo1, string periodo2)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into precalcularreporte2 (cod_usuario, vendedor,cliente,factor1, factor2, nombre1, nombre2) " +
                "values (@cod_usuario, @vendedor, @cliente ,@factor1, @factor2, @nombre1, @nombre2)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_usuario", cod_usuario);
                    cmd.Parameters.AddWithValue("@vendedor", vendedor);
                    cmd.Parameters.AddWithValue("@cliente", cliente);
                    cmd.Parameters.AddWithValue("@factor1", factor1);
                    cmd.Parameters.AddWithValue("@factor2", factor2);
                    cmd.Parameters.AddWithValue("@nombre1", periodo1);
                    cmd.Parameters.AddWithValue("@nombre2", periodo2);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Insert Detalle: " + EX.Message;
                    }
                }
            }
        }

        public static DataTable listar_diario1(string USER)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT nombre_campo, factor1, factor2, factor3, factor4 from precalcularreporte where cod_usuario = '" + USER + "'";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static DataTable listar_diario2(string USER)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT codvendedor,rutcliente,vendedor, cliente as 'nombrecliente', factor1,factor2,factor3,factor4 from v_precalcularreporte2 where cod_usuario = '" + USER + "'";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static DataTable es_nuevo(string periodo_para_nuevos, string cliente, string vendedor)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select isnull(sum(neto_pesos),0) as Venta, vendedor as 'Vendedor', ltrim(rtrim(nombrecliente)) as 'Nombre Cliente' from thx_v_reporte where nombrecliente like '%" + cliente + "%' and vendedor like '%" + vendedor + "%'" +
                                "	and Periodo = " + periodo_para_nuevos + "  and rutcliente not in ( " +
                             "	select rutcliente from thx_v_reporte where nombrecliente like '%" + cliente + "%' and vendedor like '%" + vendedor + "%'" +
                                " and Periodo <  " + periodo_para_nuevos + " group by rutcliente	, periodo " +
                                " )  group by rutcliente , ltrim(rtrim(nombrecliente)), vendedor , periodo  ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static string Facturación_Mes_precalculado1(string USER)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT isnull(sum( cast(replace(factor1,'.','') as float) ),0) from precalcularreporte2 " +
                " where cod_usuario = @cod_usuario ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_usuario", USER);

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "Error Detalle: " + EX.Message;
                    }
                }
            }
            return scalar;
        }

        public static string Facturación_Mes_precalculado2(string USER)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT isnull(sum( cast(replace(factor2,'.','') as float) ),0) from precalcularreporte2 " +
                " where cod_usuario = @cod_usuario ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_usuario", USER);

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "Error Detalle: " + EX.Message;
                    }
                }
            }
            return scalar;
        }

        public static DataTable carga_vendedor(string p1, string p2, string grupos_del_usuario)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select vendedor as nom_vend, cod_vendedor as cod_vend from thx_v_reporte where user1 in (" + grupos_del_usuario + ") and " +
                    " FechaFactura >= CONVERT(datetime,'" + p1 + "', 103) and FechaFactura <= CONVERT(datetime,'" + p2 + "',103) group by vendedor";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static DataTable carga_cliente(string p1, string p2, string grupos_del_usuario)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select nombrecliente as nom_cli, rutcliente as rut_cli from thx_v_reporte where user1 in (" + grupos_del_usuario + ") and " +
                     " FechaFactura >= CONVERT(datetime,'" + p1 + "', 103) and FechaFactura <= CONVERT(datetime,'" + p2 + "',103) group by vendedor";


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static string Facturación_Mes_precalculado3(string USER)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT isnull(sum( cast(replace(factor3,'.','') as float) ),0) from precalcularreporte2 " +
                " where cod_usuario = @cod_usuario ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_usuario", USER);

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "Error Detalle: " + EX.Message;
                    }
                }
            }
            return scalar;
        }

        public static string Facturación_Mes_precalculado4(string USER)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT isnull(sum( cast(replace(factor4,'.','') as float) ),0) from precalcularreporte2 " +
                " where cod_usuario = @cod_usuario ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_usuario", USER);

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "Error Detalle: " + EX.Message;
                    }
                }
            }
            return scalar;
        }

        public static string pregunta_periodo_prod(string USER, string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT nombre" + p + " from precalcularreporte2 where  cod_usuario = @cod_usuario and nombre" + p + " <> '' group by nombre" + p + "";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_usuario", USER);

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }

        public static DataTable LISTAR_VENDEDORES_POR_BODEGA(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select vendedor from thx_v_reporte " + where + " group by vendedor";


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static string venta_fecha_vendedor(string DESDE, string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT isnull(sum(neto_pesos),0) from thx_v_reporte where vendedor = @vendedor and FechaFactura = CONVERT(datetime, @desde, 103) group by vendedor";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@desde", DESDE);
                    cmd.Parameters.AddWithValue("@vendedor", p);
                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        public static string cont_fact_fecha_vendedor(string DESDE, string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT count(númfactura) from thx_v_reporte where vendedor = @vendedor and FechaFactura = CONVERT(datetime, @desde, 103) group by vendedor";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@desde", DESDE);
                    cmd.Parameters.AddWithValue("@vendedor", p);
                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        public static DataTable table_totales(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"select convert(varchar, fechafactura, 103) as fechafactura, sum(neto_pesos), count(distinct(númfactura)), ltrim(rtrim(([Vendedor]))) as vendedor, user1 , nombrecliente, codvendedor, rutcliente   " +
                        " from thx_v_reporte " + where + " group by fechafactura, vendedor, user1, nombrecliente, codvendedor, rutcliente order by vendedor ";


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static DataTable carga_bodega(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select ltrim(rtrim(([bodega]))) as nom_bodega from thx_v_reporte group by bodega  order by bodega";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;

        }

        public static DataTable cargar_detalle_lv(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select ltrim(rtrim((nombrecliente))) as NombreCliente, SUM(NETO_PESOS) as Venta, númfactura as 'NºFactura'
	                    from thx_v_reporte " + where + " group by nombrecliente, vendedor, númfactura ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static DataTable table_totales_c(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select periodo, sum(neto_pesos), ltrim(rtrim(([Vendedor]))) as vendedor from thx_v_reporte    " + where + " group by  periodo , Vendedor";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static DataTable table_totasles_c(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select periodo, sum(neto_pesos), ltrim(rtrim(([Vendedor])))  as vendedor, rutcliente, nombrecliente from thx_v_reporte    " + where + "  group by  periodo , Vendedor, rutcliente, nombrecliente";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static DataTable carga_unidad_negocio()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from UnidadNegocio ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static DataTable carga_grupo_unidad(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from UnidadNegocio_det_grup  " + where;

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static DataTable carga_app_unidad(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select cod_app, nom_app + CONVERT(varchar, '  -  ') + (select nombre_unineg from UnidadNegocio b where a.cod_unineg = b.cod_unineg) as nom_app   from UnidadNegocio_Det_App a " + where;

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static DataTable carga_unidad_vend(string grupo)
        {



            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" (SELECT        STUFF
                                 ((SELECT        CAST(',' AS varchar(MAX)) + CONVERT(varchar, X.cod_unineg)
                                                        FROM            UnidadNegocio X
                                           WHERE        X.cod_unineg >=3
                                                    ORDER BY X.cod_unineg FOR XML PATH('')), 1, 1, ''))
                                ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        scalar = "";
                    }
                }
            }

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from  UnidadNegocio where cod_unineg in ((
                                select cod_unineg from UnidadNegocio_Det_Grup where cod_grupo = '" + grupo + "')," + scalar + " )";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }



            return dt;
        }

        public static DataTable carga_grupo_vend(string grupo)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"
                                select * from UnidadNegocio_Det_Grup where cod_grupo = '" + grupo + "'";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static DataTable carga_pantallas_vend(string grupo)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"
                                select * from  UnidadNegocio_Det_app where cod_unineg = (
                                select cod_unineg from  UnidadNegocio where cod_unineg = (
                                    select cod_unineg from UnidadNegocio_Det_Grup where cod_grupo = '" + grupo + "'))";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static string trae_u_negocio(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT u_negocio from UsuarioWeb where cod_usuario = @usuario";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", p);
                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        public static string trae_app(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT app from UsuarioWeb where cod_usuario = @usuario";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", p);
                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        public static string obtienenom_unidad(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT nombre_unineg from unidadnegocio where cod_unineg = @cod_unineg";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_unineg", p);
                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        public static string obtiene_todos_u_negocio()
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT STUFF ((SELECT  CAST(', ' AS varchar(MAX)) + CONVERT(varchar, cod_unineg) 
                            FROM UnidadNegocio 
                           GROUP BY cod_unineg 
                            ORDER BY cod_unineg FOR XML PATH('')), 1, 1, '') AS cod_unineg";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        public static string obtiene_todos_app()
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT STUFF ((SELECT  CAST(', ' AS varchar(MAX)) + CONVERT(varchar, cod_app) 
                            FROM UnidadNegocio_Det_App 
                           GROUP BY cod_app 
                            ORDER BY cod_app FOR XML PATH('')), 1, 1, '') AS cod_app";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }
        public static string obtiene_todos_grupos()
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT STUFF ((SELECT  CAST(', ' AS varchar(MAX)) + CONVERT(varchar, cod_grupo) 
                            FROM UnidadNegocio_Det_Grup 
                           GROUP BY cod_grupo 
                            ORDER BY cod_grupo FOR XML PATH('')), 1, 1, '') AS cod_grupo";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        public static DataTable carga_unidad_otro(string p)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from UnidadNegocio where cod_unineg in (" + p + ")";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static string negocio_usuario(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT u_negocio from usuarioweb where cod_usuario = '" + p + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        public static DataTable productos_dias(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select descproducto, SUM(neto_pesos) as suma,periodo,  convert(varchar, fechafactura, 103) as fechafactura from THX_v_reporte " + where + "  group  by descproducto, fechafactura, periodo   order by descproducto";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static DataTable productos_periodos(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select descproducto, SUM(neto_pesos) as suma, periodo, convert(varchar, fechafactura, 103) as fechafactura from THX_v_reporte " + where + "  group  by descproducto, periodo, fechafactura order by descproducto";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        public static string negocio_usuario_por_grupos(string grupo)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT STUFF ((SELECT  CAST(', ' AS varchar(MAX)) + CONVERT(varchar, cod_unineg) 
                            FROM UnidadNegocio 
                            where UnidadNegocio.cod_unineg in (select cod_unineg from UnidadNegocio_Det_Grup where cod_grupo in (" + grupo + "))" +
                           " GROUP BY cod_unineg " +
                         "   ORDER BY cod_unineg FOR XML PATH('')), 1, 1, '') AS cod_unineg";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable cargar_detalle_lv_producto(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select ltrim(rtrim((nombrecliente))) as NombreCliente, SUM(NETO_PESOS) as Venta, númfactura as 'NºFactura', Vendedor, convert(varchar, fechafactura, 103) as FechaFactura
	                    from thx_v_reporte " + where + " group by nombrecliente, vendedor, númfactura, fechafactura ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable detalle_cliente(string grupo, string user1)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select Vendedor, DescProducto, sum(neto_pesos) as Venta, NúmFactura as 'NºFactura', convert(varchar, fechafactura, 103) as Fecha from thx_v_reporte where rutcliente = '" + grupo + "' " + user1 + "  group by vendedor, descproducto, númfactura, convert(varchar, fechafactura, 103) ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static string nombre_vendedor(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select top 1 vendedor from THX_v_reporte where codvendedor = '" + p + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }
        internal static string nombre_cliente(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select top 1 nombrecliente from THX_v_reporte where rutcliente like '%" + p + "%'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string cod_vendedor(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select top 1 codvendedor from THX_v_reporte where vendedor = '" + p + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string cod_producto(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select top 1 producto from THX_v_reporte where descproducto = '" + p + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string nombre_producto(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select top 1 descproducto from THX_v_reporte where producto = '" + p + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable trae_encabezado(string p)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select top 1 *, CONVERT(varchar, fechafactura, 103) as fechafactura1 from THX_v_reporte where númfactura = '" + p + "'";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable detalle_Factura(string p)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select Producto as 'Cod.Producto', DescProducto, Bodega, neto_pesos as Total from THX_v_reporte where númfactura = '" + p + "'";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable list_ficha_cliente(string where, string periodo_desde, string periodo_hasta)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select a.rutcliente as RutCliente, a.nombrecliente as NombCliente, a.vendedor as Vendedor, a.ultimafechafactura as FechaÚltimaFactura, (select sum(neto_pesos) from V_REPORTE_THX  where vendedor = a.vendedor and ltrim(rtrim(a.rutcliente)) = ltrim(rtrim(V_REPORTE_THX.rutcliente)) and fechafactura = CONVERT(datetime,  a.ultimafechafactura, 103)) as Monto, " +
                "	(select top 1 lc from V_REPORTE_THX where rutcliente = a.rutcliente ) as 'L.Crédito', " +
                "	(select ROUND(AVG(CAST(disponible AS FLOAT)), 0) from V_CLIENTE_D where ltrim(rtrim(a.rutcliente)) = ltrim(rtrim(run)) ) as 'L.Cr.Disponible', " +
                "(select ROUND(AVG(CAST(neto_pesos AS FLOAT)), 0) from V_REPORTE_THX c where periodo >= '" + periodo_desde + "'  and periodo <= '" + periodo_hasta + "' " +
                "	 and c.rutcliente= a.rutcliente and c.vendedor= a.vendedor) as PROM   " +
                "	 from v_ultimas_facturas a " + where.Replace("FechaFactura", "convert(datetime,a.ultimafechafactura,103)") + " order by convert(datetime,a.ultimafechafactura,103) desc";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable datos_cliente(string p)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select *,  (select correo from cliente2 where cliente.rutcliente = cliente2.rutcliente) as correo, isnull((select top 1 l_credito from v_cobranza a  where dbo.Quitar_Cero_izquierda('0', a.rutcliente) = dbo.Quitar_Cero_izquierda('0', cliente.rutcliente)),0) as LC, (select  disponible from v_cliente_d where rutcliente = cliente.rutcliente) as disponible  from cliente where dbo.Quitar_Cero_izquierda('0', rutcliente) = '" + p + "'";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable trae_cta_cte_cliente(string p)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select Vendedor, DescProducto, sum(neto_pesos) as Venta, NúmFactura as 'NºFactura', convert(varchar, fechafactura, 103) as Fecha 
                        from thx_v_reporte where rutcliente like '%" + p + "%' group by vendedor, descproducto, númfactura, fechafactura, convert(varchar, fechafactura, 103) " +
                        " order by fechafactura desc";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable listar_resumen_productos_ficha(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" SELECT  DescProducto,Periodo, sum(neto_pesos) as total2" +
                " FROM thx_v_reporte  " + where +
                " group by  descproducto, periodo order by periodo desc";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }

            return Pivot(dt, dt.Columns["Periodo"], dt.Columns["total2"]);
        }

        internal static string update_cliente(string rutcliente, string direccion, string ciudad1, string pais, string fono, string vendedor)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"update Cliente set direccion = @direccion , ciudad = @ciudad, pais = @pais, fono = @fono, codvendedor = @codvendedor where rutcliente like @rutcliente";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@rutcliente", "%" + rutcliente + "%");
                    cmd.Parameters.AddWithValue("@direccion", direccion);
                    cmd.Parameters.AddWithValue("@ciudad", ciudad1);
                    cmd.Parameters.AddWithValue("@pais", pais);
                    cmd.Parameters.AddWithValue("@fono", fono);
                    cmd.Parameters.AddWithValue("@codvendedor", vendedor);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }

        internal static string update_cliente2(string p1, string p2)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"update Cliente2 set  correo= @correo where rutcliente like @rutcliente";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@rutcliente", "%" + p1 + "%");
                    cmd.Parameters.AddWithValue("@correo", p2);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }

        internal static string busca_rut_cliente(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select top 1 rutcliente from cliente2 where replace(  replace( rtrim(rutcliente), '.', '' ) , '-', '' )  = '" + p + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string insert_cliente_nuevo(string rut, string nomb, string dir, string ciu, string pa, string fono, string vende)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into cliente (rutcliente, nombrecliente, direccion, ciudad, pais, fono, codvendedor, estado) values  (@rutcliente, @nombre, @direccion,  @ciudad,  @pais,  @fono,  @vendedor, 0)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@rutcliente", rut);
                    cmd.Parameters.AddWithValue("@direccion", dir);
                    cmd.Parameters.AddWithValue("@nombre", nomb);
                    cmd.Parameters.AddWithValue("@ciudad", ciu);
                    cmd.Parameters.AddWithValue("@pais", pa);
                    cmd.Parameters.AddWithValue("@fono", fono);
                    cmd.Parameters.AddWithValue("@vendedor", vende);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }


        internal static DataTable clientes_nuevos_ficha(string vend)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "";

                if (vend != "")
                {
                    sql = @"select RutCliente, NombreCliente, (select top 1 vendedor from thx_v_reporte a where cliente2.CodVendedor = a.codvendedor) as Vendedor,  convert(varchar, fecha_creaci, 111) as FechaCreac  from cliente2 where estado = 0 and codvendedor = '" + vend + "'";
                }
                else
                {
                    sql = @"select RutCliente, NombreCliente, (select top 1 vendedor from thx_v_reporte a where cliente2.CodVendedor = a.codvendedor) as Vendedor, convert(varchar, fecha_creaci, 111) as FechaCreac from cliente2 where estado = 0 ";
                }
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable tipo_negocio()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from tipos_giro where id <> 11";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static string insert_cliente2(int sw, DateTime fecha_creacio, string rutcliente, string giro, string tipo_giro, string cliente_desde, string credito_act, string tipo_cred_act, string monto_cred_act, string credi_soli, string tipo_credi_soli, string monto_credi_soli, string nombrecliente, string direccion, string ciudad, string pais, string fono, string codvendedor)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into cliente2 values  "
                    + " (@rutcliente, @giro, @tipo_giro, @cliente_desde, @credito_act, @tipo_cred_act, @monto_cred_act, "
                        + "@credi_soli, @tipo_credi_soli, @monto_credi_soli, @fecha_creacio, @nombrecliente, @direccion, @ciudad, @pais, @fono,  @codvendedor, @estado)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@rutcliente", rutcliente);
                    cmd.Parameters.AddWithValue("@giro", giro);
                    cmd.Parameters.AddWithValue("@tipo_giro", tipo_giro);
                    cmd.Parameters.AddWithValue("@cliente_desde", cliente_desde);
                    cmd.Parameters.AddWithValue("@credito_act", credito_act);
                    cmd.Parameters.AddWithValue("@tipo_cred_act", tipo_cred_act);
                    cmd.Parameters.AddWithValue("@monto_cred_act", monto_cred_act);
                    cmd.Parameters.AddWithValue("@credi_soli", credi_soli);
                    cmd.Parameters.AddWithValue("@tipo_credi_soli", tipo_credi_soli);
                    cmd.Parameters.AddWithValue("@monto_credi_soli", monto_credi_soli);
                    cmd.Parameters.AddWithValue("@fecha_creacio", fecha_creacio);
                    cmd.Parameters.AddWithValue("@nombrecliente", nombrecliente);
                    cmd.Parameters.AddWithValue("@direccion", direccion);
                    cmd.Parameters.AddWithValue("@ciudad", ciudad);
                    cmd.Parameters.AddWithValue("@pais", pais);
                    cmd.Parameters.AddWithValue("@fono", fono);
                    cmd.Parameters.AddWithValue("@codvendedor", codvendedor);
                    if (sw == 1)
                    {
                        cmd.Parameters.AddWithValue("@estado", "1");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@estado", "0");
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }

        internal static string insert_contactos_cliente(int sw, string rutclie, string nom1, string nom2, string correo1, string correo2, string cargo1, string cargo2, string fono1, string fono2)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "";
                if (sw == 1)
                {
                    sql = @"insert into cli_contactos values  (@rutcliente, @nom1, @correo1, @cargo1, @fono1);" +
                     "      insert into cli_contactos values  (@rutcliente, @nom2, @correo2, @cargo2, @fono2);";
                }
                else
                {

                    sql = @"insert into cli_contactos values  (@rutcliente, @nom1, @correo1, @cargo1, @fono1);";
                }
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@rutcliente", rutclie);
                    cmd.Parameters.AddWithValue("@nom1", nom1);
                    cmd.Parameters.AddWithValue("@correo1", correo1);
                    cmd.Parameters.AddWithValue("@cargo1", cargo1);
                    cmd.Parameters.AddWithValue("@fono1", fono1);
                    cmd.Parameters.AddWithValue("@nom2", nom2);
                    cmd.Parameters.AddWithValue("@correo2", correo2);
                    cmd.Parameters.AddWithValue("@cargo2", cargo2);
                    cmd.Parameters.AddWithValue("@fono2", fono2);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }

        internal static string insert_ref_banco_cliente(int sw, string rutclie, string ban1, string ban2, string cta1, string cta2)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "";
                if (sw == 1)
                {
                    sql = @"insert into cli_ref_banco values  (@rutcliente, @ban1, @cta1);" +
                     "      insert into cli_ref_banco values  (@rutcliente, @ban2, @cta2);";
                }
                else
                {

                    sql = @"insert into cli_ref_banco values  (@rutcliente, @ban1, @cta1);";
                }
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@rutcliente", rutclie);
                    cmd.Parameters.AddWithValue("@ban1", ban1);
                    cmd.Parameters.AddWithValue("@ban2", ban2);
                    cmd.Parameters.AddWithValue("@cta1", cta1);
                    cmd.Parameters.AddWithValue("@cta2", cta2);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }

        internal static string insert_sociedad_cliente(string rutclie, string rutsocie, string nomb, string direcc, string ciudad, string pais, string fono, string correo)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into cli_sociedad values  (@rutcliente, @rutsocie, @nomb, @direcc, @ciudad, @pais, @fono, @correo);";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@rutcliente", rutclie);
                    cmd.Parameters.AddWithValue("@rutsocie", rutsocie);
                    cmd.Parameters.AddWithValue("@nomb", nomb);
                    cmd.Parameters.AddWithValue("@direcc", direcc);
                    cmd.Parameters.AddWithValue("@ciudad", ciudad);
                    cmd.Parameters.AddWithValue("@pais", pais);
                    cmd.Parameters.AddWithValue("@fono", fono);
                    cmd.Parameters.AddWithValue("@correo", correo);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }

        internal static string insert_socios_cliente(int sw, string rutcli, string rut1, string rut2, string nom1, string nom2, string corre1, string corre2, string porc1, string porc2)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "";
                if (sw == 1)
                {
                    sql = @"insert into cli_socio values  (@rutcliente, @rut1, @nom1, @corre1, @porc1);" +
                     "      insert into cli_socio values  (@rutcliente, @rut2, @nom2, @corre2, @porc2);";
                }
                else
                {

                    sql = @"insert into cli_socio values  (@rutcliente, @rut1, @nom1, @corre1, @porc1);";
                }
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@rutcliente", rutcli);
                    cmd.Parameters.AddWithValue("@rut1", rut1);
                    cmd.Parameters.AddWithValue("@nom1", nom1);
                    cmd.Parameters.AddWithValue("@corre1", corre1);
                    cmd.Parameters.AddWithValue("@porc1", porc1);

                    cmd.Parameters.AddWithValue("@rut2", rut2);
                    cmd.Parameters.AddWithValue("@nom2", nom2);
                    cmd.Parameters.AddWithValue("@corre2", corre2);
                    cmd.Parameters.AddWithValue("@porc2", porc2);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }

        internal static string trae_tipo_negocio_nombre(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select top 1 nombre_tipo from Tipos_giro where id = " + p;
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable corr_nuevos_clie()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select top 1 * from lista_correos where id = 1";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable datos_cliente_ALL(int sw, string rutcliente)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "";
                if (sw == 1)
                {
                    sql = @"select top 1 * from v_cliente where rutcliente = '" + rutcliente + "' and estado <> 0";
                }
                else
                {
                    sql = @"select top 1 * from v_cliente where rutcliente = '" + rutcliente + "' and estado = 0";
                }
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static string count_socios(string clie_rut)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select count(*) from Cli_Socio where rutcliente = '" + clie_rut + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable socio_extra(string clie_rut)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from Cli_Socio where rutcliente = '" + clie_rut + "' and id = (select MAX(id) from Cli_Socio where rutcliente = '" + clie_rut + "' )";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static string count_bancas(string clie_rut)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select count(*) from Cli_Ref_Banco where rutcliente = '" + clie_rut + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable banco_extra(string clie_rut)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from Cli_Ref_Banco where rutcliente = '" + clie_rut + "' and id = (select MAX(id) from Cli_Ref_Banco where rutcliente = '" + clie_rut + "' )";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static string count_contactos(string clie_rut)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select count(*) from Cli_Contactos where rutcliente = '" + clie_rut + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable contacto_extra(string clie_rut)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from Cli_Contactos where rutcliente = '" + clie_rut + "' and id = (select MAX(id) from Cli_Contactos where rutcliente = '" + clie_rut + "' )";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static string delete_cliente2(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from cliente2 where rutcliente = '" + p + "' and estado = 1";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string delete_contactos(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from Cli_Contactos where rutcliente = '" + p + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string delete_cuentas_bank(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from Cli_Ref_Banco where rutcliente = '" + p + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string delete_socios(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from Cli_Socio where rutcliente = '" + p + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string delete_sociedad(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from Cli_Sociedad where rutcliente = '" + p + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static object detalle_monto_click(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select DescProducto, SUM(NETO_PESOS) as Venta, númfactura as 'NºFactura', convert(varchar, fechafactura, 103) as FechaFactura
	                    from thx_v_reporte " + where + " group by númfactura, fechafactura, vendedor, rutcliente,DescProducto ";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static string linea_credito(string clie_rut)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"  select  isnull(      " +
                                  "    (select top 1 convert(varchar,l_credito) from v_cobranza a    " +
                               "       where dbo.Quitar_Cero_izquierda('0', a.rutcliente) = dbo.Quitar_Cero_izquierda('0','" + clie_rut + "')),'0')     " +
                               "         + CONVERT(varchar, '(D: ')+  isnull(   (select LTRIM(Str(disponible, 100, 0)) from v_cliente_d a    " +
                               "         where dbo.Quitar_Cero_izquierda('0', a.rutcliente) = dbo.Quitar_Cero_izquierda('0','" + clie_rut + "')), (  select  isnull(       " +
                              "        (select top 1 convert(varchar,l_credito) from v_cobranza a    " +
                              "        where dbo.Quitar_Cero_izquierda('0', a.rutcliente) = dbo.Quitar_Cero_izquierda('0','" + clie_rut + "')),'0')   ))     " +
                              "          +  CONVERT(varchar, ' )') as LC  ";


                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string tre_rut_cliente(string p)
        {


            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select top 1 rutcliente from cliente  where nombrecliente like '%" + p + "%'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string trae_año_factura(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select top 1 datepart(year,fechafactura) from V_REPORTE_THX where númfactura =  '" + p + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string cod_unineg(string grupo)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from UnidadNegocio_Det_Grup where cod_grupo like '%" + grupo + "%'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable corr_usuario(string p)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select nombre_usuario, correo from usuarioweb where cod_usuario = '" + p + "'";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static object privot_semanas(string hoy, string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select a._0, a.semana, a.debe_monto, a.año  from " +
                            "    (SELECT '' as _0, DATEPART(wk, fecha_venc) as semana , YEAR(fecha_venc) as año, SUM(saldo) as debe_monto, factura FROM v_cobranza where fecha_venc >= convert(datetime, '" + hoy + "', 103) " +
                             "  " + where + " and estado_doc <> 0 and tipo_doc <> 'CM' group by fecha_venc, factura ) a  order by  a.año asc, a.semana asc";



                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            _SourceTable = dt;
            //return Pivot(dt, dt.Columns["semana"], dt.Columns["debe_monto"]);
            return PivotData("_0", "debe_monto",
               AggregateFunction.Sum, "semana");

            //return PivotData("NombreCliente", "total2",
            //   AggregateFunction.Sum, "Periodo");
        }

        internal static string semana_0(string hoy, string where)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select isnull(sum(a.mont),0) from  " +
                  "(select SUM(saldo) as mont, factura from v_cobranza where fecha_venc < convert(datetime, '" + hoy + "', 103)  " + where + " and tipo_doc <> 'CM' and estado_doc <> 0   group by factura ) a  ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string vencidos_total(string where)
        {

            DateTime t = DateTime.Now;
            t = new DateTime(t.Year, t.Month, t.Day);
            string hoy = t.ToShortDateString();

            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select sum(saldo) from v_cobranza_docs where fecha_venc < convert(datetime, getdate(), 103)  " + where;
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static string estimados_total(string where)
        {

            DateTime t = DateTime.Now;
            t = new DateTime(t.Year, t.Month, t.Day);
            string hoy = t.ToShortDateString();

            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select sum(mont) from (select sum( saldo) as mont from v_cobranza where fecha_venc >= convert(datetime, '" + hoy + "', 103) " + where + "  and estado_doc <> 0 and tipo_doc <> 'CM' group by  factura) as qw";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable docu_abier(string clie_rut, string vend)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select rutcliente as RutCliente, nombrecliente as NombCliente, factura as 'Nº Factura', nombrevendedor as NombVendedor, tipo_doc as TipoDocum,descr as 'Descripción', num_ref as NumRef,
                 CONVERT(VARCHAR,fecha_trans,103) as FechaTransc, CONVERT(VARCHAR,fecha_venc,103) as FechaVenc, tipo_credi as TipoCred, 
                (select top 1 lc from V_REPORTE_THX where rutcliente = rutcliente ) as 'L.Crédito', 	monto_doc as Monto, 

                saldo as 'Saldo'


                  from v_cobranza where estado_doc <> 0 and rutcliente like '%" + clie_rut + "%' ";

                if (vend != "")
                {

                    sql += " and vendedor = '" + vend + "' ";
                }
                sql += " order by fecha_venc asc";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static string linea_credito_disponible(string p)
        {
            DateTime t = DateTime.Now;
            t = new DateTime(t.Year, t.Month, t.Day);
            string hoy = t.ToShortDateString();

            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"(select ROUND(AVG(CAST(disponible AS FLOAT)), 0) from v_cliente_d where rutcliente like '%" + p + "%') ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable listar_acciones()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select * from acciones where id_accion <> 5 and id_accion <> 7";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static object quitar_pa_f(string id)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from Cobranza_Seguimiento where num_factura = '" + id + "' and tipo_doc = 'PA-F' ;" +
                                "delete from Cobranza_pagos where id_cobranza = '" + id + "' and tipo_doc = 'temporal';";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@num_factura", id);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Delete " + EX.Message;
                    }
                }
            }
        }

        internal static DataTable trae_docu_calend(string where, string usuario)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"select 
                                   [NºDoc]
                                  ,[TDoc]
                                  ,[NomClien]
                                  ,[FTrans]
                                  ,[FVenc]
                                  ,[FPago]
                                  ,[UltAcc]
                                  ,[DDía]
                                  ,[Monto(Peso)]
                                  ,[Monto(Dolar)]
                                  ,[Saldo_Peso]
                                  ,[Saldo_Dolar]
                                  ,[TCamb]
                                  ,[Descr]
                                  ,[TMoned]
                                  ,[NombVend]
                                  ,[T.Cr]
                                  ,[Seguimiento]
                                  ,[rutcliente]
                                  ,[id]
                                  ,[TPago]
                                  ,[accion]
                                  ,[obs]
                                  ,[Comisiones]
                                  ,[neto_peso]
                            from v_facturas_cobranza where estado_doc <> 0 and saldo <> 0 " + where + " order by rutcliente, [FVenc]   -- " + usuario;



                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static string insert_fecha_cobra(string num_factura, string fecha_cobro, string obs_cobro)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into cobranza_cobros values (@num_factura, convert(datetime, @fecha_cobro, 103), @obs_cobro)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@num_factura", num_factura);
                    cmd.Parameters.AddWithValue("@fecha_cobro", fecha_cobro);
                    cmd.Parameters.AddWithValue("@obs_cobro", obs_cobro);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";


                    }
                }
            }
            return scalar;
        }

        internal static string delete_fecha_cobro(string num_factura)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from cobranza_pagos where convert(varchar, id_cobranza,103) = '" + num_factura + "' and tipo_doc = 'temporal'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "ERROR";
                    }
                }
            }
            return scalar;
        }

        internal static string insert_seguimiento(string num_factura)
        {

            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string query = "";

                query += "DELETE FROM cobranza_seguimiento WHERE num_factura = '" + num_factura + "' and estado = 'EN SEGUIMIENTO' ; ";

                query += "INSERT INTO COBRANZA_SEGUIMIENTO  ( ";
                query += "num_factura, ";
                query += "rutcliente, ";
                query += "tipo_doc, ";
                query += "estado, ";
                query += "monto_doc, ";
                query += "observacion, ";
                query += "usuario, ";
                query += "fecha_venc, ";
                query += "fecha ";
                query += ") SELECT ";
                query += "factura, ";
                query += "rutcliente, ";
                query += "tipo_doc, ";
                query += "'EN SEGUIMIENTO', ";
                query += "monto_doc, ";
                query += "descr, ";
                query += "'" + HttpContext.Current.Session["user"].ToString() + "', ";
                query += "CONVERT(datetime, fecha_venc, 103), ";
                query += "CONVERT(datetime, fecha_trans, 103) from V_COBRANZA where factura = '" + num_factura.Trim() + "'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable trae_acciones_(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select *, '' as Elim from [V_COBRANZA_MOVIMIENTOS] where 1=1 " + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static string update_estado_ingresado(string num_factura, string estado, string id)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"UPDATE Cobranza_Seguimiento SET estado_ingresado = 1 where num_factura = @num_factura and estado = @estado and id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@num_factura", num_factura);
                    cmd.Parameters.AddWithValue("@estado", estado);
                    cmd.Parameters.AddWithValue("@id", id);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }

        internal static DataTable trae_rangos()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"     select SUM(saldo) as monto, rg from (
                                   
                                     SELECT saldo, factura,  
                                     rg=
 
                                     CASE  WHEN fecha_venc < convert(datetime,CONVERT(varchar(10), dateadd(day,0, GETDATE()), 103),103) and fecha_venc >= convert(datetime,CONVERT(varchar(10), dateadd(day,-5, GETDATE()), 103),103)
                                     THEN  '1-5*#6666CC' 
                                     WHEN	fecha_venc <= convert(datetime,CONVERT(varchar(10), dateadd(day,-6, GETDATE()), 103),103) and fecha_venc >= convert(datetime,CONVERT(varchar(10), dateadd(day,-10, GETDATE()), 103),103)
                                     THEN '6-10*#FF9999'   
                                     WHEN	fecha_venc <= convert(datetime,CONVERT(varchar(10), dateadd(day,-11, GETDATE()), 103),103) and fecha_venc >= convert(datetime,CONVERT(varchar(10), dateadd(day,-15, GETDATE()), 103),103)
                                     THEN '11-15*#00CC99' 
                                     WHEN	fecha_venc <= convert(datetime,CONVERT(varchar(10), dateadd(day,-16, GETDATE()), 103),103) and fecha_venc >= convert(datetime,CONVERT(varchar(10), dateadd(day,-20, GETDATE()), 103),103)
                                     THEN '16-20*#CCCC33' 
                                     WHEN	fecha_venc <= convert(datetime,CONVERT(varchar(10), dateadd(day,-21, GETDATE()), 103),103) and fecha_venc >= convert(datetime,CONVERT(varchar(10), dateadd(day,-25, GETDATE()), 103),103)
                                     THEN '21-25*#0000CC'   
                                     WHEN	fecha_venc <= convert(datetime,CONVERT(varchar(10), dateadd(day,-26, GETDATE()), 103),103) and fecha_venc >= convert(datetime,CONVERT(varchar(10), dateadd(day,-45, GETDATE()), 103),103)
                                     THEN  '26-45*#FF6633' 
                                     WHEN fecha_venc < convert(datetime,CONVERT(varchar(10), dateadd(day,-45, GETDATE()), 103),103)
                                     THEN  '>45*#009900' 
                                     WHEN	fecha_venc >= convert(datetime,CONVERT(varchar(10), dateadd(day,0, GETDATE()), 103),103)
                                     THEN  'NoVencido*#99CC00'  
                                     END 
                                       FROM V_COBRANZA ca where 1 = 1 and tipo_doc <> 'CM'  and tipo_doc in ('IN', 'DM') and estado_doc <> 0
                                       )  as qwed group by rg order by rg asc
                                    
                                    ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable trae_fact_cheque_dm()
        {


            DateTime t = DateTime.Now;
            t = new DateTime(t.Year, t.Month, t.Day);
            string hoy = t.ToShortDateString();

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"  select SUM(saldo) as monto, rg from (
                                    
                                     SELECT saldo, factura,
                                     rg =
 
                                     CASE  WHEN  tipo_doc = 'IN'
                                     THEN  'IN*#F39C12' 
                                     WHEN tipo_doc = 'DM' and  isnumeric(factura) <> 1 
                                     THEN 'CHEQUE_PROT-NOACTU*#9B59B6'   
									 WHEN tipo_doc = 'DM' and  isnumeric(factura) = 1 
                                     THEN 'DM*#117864'   
                                     END 
                                       FROM V_COBRANZA ca where 1 = 1 and ca.estado_doc <> 0 and ca.tipo_doc IN ('IN', 'DM') and fecha_venc  < CONVERT(DATETIME, '" + hoy + "',103)  )  as qwed group by rg order by rg asc ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static DataTable trae_abarr_y_granos()
        {
            DateTime t = DateTime.Now;
            t = new DateTime(t.Year, t.Month, t.Day);
            string hoy = t.ToShortDateString();

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"   				
			                select sum(saldo)  as saldo, grupo from (	
				                select SUM(saldo) as saldo, 
				                 grupo= CASE when rtrim(ltrim(grupo)) != 'Granos' and rtrim(ltrim(grupo)) is not null then
				                 'Abarrotes*#E74C3C'
				                 when rtrim(ltrim(grupo)) = 'Granos' then
				                 'Granos*#0033CC'
				                 when rtrim(ltrim(grupo)) is null then
				                 'SINGRUPO*#6666CC'
				                END		
				
				                 from (
                                        select * from (
                                        SELECT saldo,              
									       (select top 1 user1 from THX_v_reporte  a where a.codvendedor = ca.vendedor  ) as grupo
                                           FROM V_COBRANZA ca where 1 = 1 and tipo_doc <> 'CM'  and tipo_doc in ('IN', 'DM') and  fecha_venc  < CONVERT(DATETIME, '" + hoy + "',103) and estado_doc <> 0   " +
                                          " ) as qweasd) as tee2  group by grupo )  as tete  group by grupo";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                try
                {
                    ap.Fill(dt);
                }
                catch { return dt = new DataTable(); }
            }
            return dt;
        }

        internal static string cambia_estado_doc(string num_factura, string id)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into estado_documento values (@id, @num_factura, '0') ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@num_factura", num_factura);
                    cmd.Parameters.AddWithValue("@id", id);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }

        internal static string update_estado_doc(string num_factura, string id, int estado)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = "";
                if (estado == 0)
                {
                    sql = @"update estado_documento set estado = '1' where num_factura=@num_factura and id=@id  ";
                }
                else
                {
                    sql = @"update estado_documento set estado = '0' where num_factura=@num_factura and id=@id  ";
                }
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@num_factura", num_factura);
                    cmd.Parameters.AddWithValue("@id", id);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";
                    }
                }
            }
            return scalar;
        }

        internal static string estado_app(string p, string id, string num_fac)
        {

            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @" select ISNULL ( (select  case when estado = '0' then 'CERRADO' when estado = '1' then 'ABIERTO' END AS estado  from estado_documento where id = @id and num_factura = @num_factura), @estado) AS estado        ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@num_factura", num_fac);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@estado", p);
                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        public static DataTable listar_NOTA_DE_CREDITO(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"   select CustID as 'Rut Cliente', CName  as 'Nombre Cliente', RefNbr  as 'Factura', (select top 1 vendedor from thx_v_reporte where a.Parent = thx_v_reporte.númfactura) as 'Nombre Vendedor' ,
                        (select top 1 [desc] from Cobranza where a.RefNbr = Cobranza.num_factura) as Descripción , convert(varchar, DueDate,103) as Fecha, CuryOrigDocAmt as Monto, Parent as FacturaOrigen
                        , (select top 1 user1 from THX_v_reporte where a.Parent = THX_v_reporte.númfactura) as Grupo 
                         from Cobranza_Full a " + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable trae_cliente_vendedor(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT a.* FROM V_GESTION_VENTAS_FIN a " + where;

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string insert_log_enviar_ficha(string user, string mail_usuario, string fecha_now, string destinos, string rutcliente)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into Log_Correos_Ficha values (@user, @mail_usuario,  @destinos, @fecha_now, @rutcliente, 0)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@user", user);
                    cmd.Parameters.AddWithValue("@mail_usuario", mail_usuario);
                    cmd.Parameters.AddWithValue("@fecha_now", fecha_now);
                    cmd.Parameters.AddWithValue("@destinos", destinos);
                    cmd.Parameters.AddWithValue("@rutcliente", rutcliente.Replace("-", "").Replace(".", ""));
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error";


                    }
                }
            }
            return scalar;
        }

        internal static DataTable trae_log_fich_()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT b.cod_usuario as CodUsuario, b.mail_usuario as Correo, b.Destinos, b.fecha_y_hora_mail as FechaEnvio, (select top 1 grupos from UsuarioWeb a where a.cod_usuario = b.cod_usuario), (select nombrecliente from Cliente where Cliente.rutcliente = b.rutcliente) as ClienteConsultado, (select nombrevendedor from V_CLIENTE_VEND C where b.rutcliente = C.rutcliente) as NombreVendedor  FROM Log_Correos_Ficha b";


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable trae_clase_clientes()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"SELECT * FROM v_cliente_clase";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable busca_factura(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select NúmFactura,RutCliente, NombreCliente ,CodVendedor, Vendedor, FechaFactura, sum(neto_pesos) as Monto from THX_v_reporte " + where +
                                  "  group by númfactura,rutCliente, nombrecliente ,codvendedor, vendedor, fechafactura ";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string estimado_peso_cobranz(string desde)
        {

            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select ISNULL((select SUM(monto) as qwer from Cobranza_pagos where (select estado_doc from cobranza where num_factura = convert(varchar, id_cobranza) and tipo_doc = 'IN') <> 0 and moneda = 'peso' and tipo_doc='temporal' and CONVERT(varchar, fecha, 103) = @desde), '0') as qw ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@desde", desde);

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;

        }

        internal static string estimado_dolar_cobranz(string desde)
        {

            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select ISNULL((select SUM(monto) as qwer from Cobranza_pagos where (select estado_doc from cobranza where num_factura = convert(varchar, id_cobranza) and tipo_doc = 'IN') <> 0 and moneda = 'dolar' and tipo_doc='temporal' and CONVERT(varchar, fecha, 103) = @desde), '0') as qw ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@desde", desde);

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;

        }
        internal static string estimado_peso_cheques(string desde)
        {

            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select ISNULL((select SUM(saldo) from V_COBRANZA where tipo_doc = 'DM' and estado_doc <> 0 and ISNUMERIC( factura ) <> 1 and CONVERT(varchar, fecha_venc, 103) = @desde and tipo_moneda = 'PESO'), '0') as qw ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@desde", desde);

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;

        }

        internal static string estimado_dolar_cheques(string desde)
        {

            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select ISNULL((select SUM(saldo_dolar) from V_COBRANZA where tipo_doc = 'DM' and estado_doc <> 0 and ISNUMERIC( factura ) <> 1 and CONVERT(varchar, fecha_venc, 103) = @desde and tipo_moneda = 'USD'), '0') as qw ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@desde", desde);

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;

        }



        internal static string delete_seguimiento_pa_f(string id)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from Cobranza_Seguimiento where num_factura = '" + id + "' and tipo_doc = 'PA-F' ;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@num_factura", id);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Delete " + EX.Message;
                    }
                }
            }
        }

        internal static string trae_letra_credito(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select top 1 tipo_credi from cliente where rutcliente=@rutcliente ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@rutcliente", p);

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;

        }

        internal static string trae_corr_vend_por_cliente(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select correo from UsuarioWeb where cod_usuario = (select top 1 codvendedor from Cliente where rutcliente = @rutcliente) ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@rutcliente", p);

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;

        }

        internal static DataTable listar_ALL_bodegas_stock()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select distinct(siteid) from Stock_diario b";
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }


        internal static DataTable listar_ALL_productos_stock(string where, string clase)
        {




            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select distinct(invtid),  descr from Stock_diario b  " + clase + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable listar_ALL_productos_stock_guion(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select distinct(invtid), invtid + CAST('-' AS varchar(MAX)) + descr as 'descr' from Stock_diario b where b.glclassid in ('ABAR', 'MANI') " + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }


        internal static DataTable listar_ALL_productos_stock_guion_2(string where, string clase)
        {
            //b.glclassid in ('ABAR', 'MANI') 




            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select distinct(invtid), invtid + CAST('- ' AS varchar(MAX)) + descr as 'descr' from Stock_diario b where " + clase + where;
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable productos_stock(string where, string fecharcp, string fecha_thx, string fechaexcel, string fechaexcelfin, string desde, string hasta)
        {
            DataTable dt = new DataTable();
            string where_extra = " where 1 = 1 and t.producto = b.invtid and t.bodega = b.siteid " + fechaexcel.Replace("fecha", "t.fechafactura").Replace("<=", ">=") + " " + fechaexcelfin.Replace("fecha", "t.fechafactura");
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"
                      select * from ( SELECT b.invtid as CodProd, b.descr as Descrip, b.glclassid as Grupo, b.stkunit as TipoUnidad, b.siteid as Bodega, " +
                        "(select sum(r.qty) FROM Stock_diario r where  r.invtid = b.invtid and r.siteid = b.siteid and  r.trandate < convert(datetime, '" + desde + "', 103)) as INICIAL, " +
                        "(select sum(r.qty) FROM Stock_diario r where  r.invtid = b.invtid and r.siteid = b.siteid and  r.trandate <= convert(datetime, '" + hasta + "', 103) ) as FINAL" +
                    ", costo_ini = " +
                    "  case  " +
                    "  when b.siteid = 'ZARATESOP' or b.siteid = 'SITRANS' or b.siteid = 'ABARROTES' " +
                   "   THEN(select top 1 bod_usd from stock_excel where cod_producto = b.invtid " + fechaexcel + " group by fecha, bod_usd   having fecha = min(fecha) order by fecha asc  ) " +
                   "    when b.siteid = 'LOVALLEDOR' or b.siteid = 'AGUNSOP'" +
                    "  THEN(select  top 1 cm_stgo from stock_excel where cod_producto = b.invtid " + fechaexcel + " group by fecha, cm_stgo   having fecha = min(fecha) order by fecha asc  ) " +
                    "   when b.siteid = 'QUILLOTSOP' or b.siteid = 'LOCAL1'" +
                    "  THEN(select  top 1 cm_qta from stock_excel where cod_producto = b.invtid " + fechaexcel + " group by fecha, cm_qta   having fecha = min(fecha) order by fecha asc  ) " +
                    "   when b.siteid = 'QUILLOTA1' " +
                   "   THEN(select  top 1 cm_qta from  stock_excel where cod_producto = b.invtid " + fechaexcel + " group by fecha, cm_qta   having fecha = min(fecha) order by fecha asc  ) " +
                   "    when b.siteid = 'ARGENTINA' " +
                   "   THEN(select  top 1 bod_usd from  stock_excel where cod_producto = b.invtid " + fechaexcel + " group by fecha, bod_usd   having fecha = min(fecha) order by fecha asc  ) " +
                   "    when b.siteid = 'ARICASOP' " +
                   "   THEN(select top 1  arica from  stock_excel where cod_producto = b.invtid " + fechaexcel + " group by fecha, arica   having fecha = min(fecha) order by fecha asc  ) " +
                    "   when b.siteid = 'ARICA1SOP' " +
                    "  THEN(select top 1  arica  from stock_excel where cod_producto = b.invtid" + fechaexcel + " group by fecha, arica   having fecha = min(fecha) order by fecha asc  ) end " +
                    ", costo_fin = " +
                    "  case  " +
                    "  when b.siteid = 'ZARATESOP' or b.siteid = 'SITRANS' or b.siteid = 'ABARROTES'  " +
                   "   THEN(select  top 1 bod_usd from  stock_excel where cod_producto = b.invtid " + fechaexcelfin + " group by fecha, bod_usd having fecha = max(fecha) order by fecha desc  ) " +
                   "    when b.siteid = 'LOVALLEDOR'  or b.siteid = 'AGUNSOP'" +
                    "  THEN(select  top 1 cm_stgo  from stock_excel where cod_producto = b.invtid " + fechaexcelfin + " group by fecha, cm_stgo having fecha = max(fecha) order by fecha desc  ) " +
                    "   when b.siteid = 'QUILLOTSOP'  or b.siteid = 'LOCAL1'" +
                    "  THEN(select  top 1 cm_qta  from stock_excel where cod_producto = b.invtid " + fechaexcelfin + " group by fecha, cm_qta having fecha = max(fecha) order by fecha desc  ) " +
                    "   when b.siteid = 'QUILLOTA1' " +
                   "   THEN(select  top 1 cm_qta  from stock_excel where cod_producto = b.invtid " + fechaexcelfin + " group by fecha, cm_qta having fecha = max(fecha) order by fecha desc  ) " +
                   "    when b.siteid = 'ARGENTINA' " +
                   "   THEN(select  top 1 bod_usd  from stock_excel where cod_producto = b.invtid " + fechaexcelfin + " group by fecha, bod_usd having fecha = max(fecha) order by fecha desc  ) " +
                   "    when b.siteid = 'ARICASOP' " +
                   "   THEN(select  top 1 arica  from stock_excel where cod_producto = b.invtid " + fechaexcelfin + " group by fecha, arica having fecha = max(fecha) order by fecha desc  ) " +
                    "   when b.siteid = 'ARICA1SOP' " +
                    "  THEN(select  top 1 arica  from stock_excel where cod_producto = b.invtid" + fechaexcelfin + " group by fecha, arica having fecha = max(fecha) order by fecha desc  ) end " +
         "  ,(select sum(t.cantidad) from v_reporte_thx t " + where_extra + "  )	 as UnVend " +
      "  ,  (select sum(t.neto_dolar) from v_reporte_thx t " + where_extra + "  ) as Ven$ " +
      "  ,  (select sum(t.neto_pesos) from v_reporte_thx t " + where_extra + "   ) as VenPeso " +

                        " from Stock_diario b where b.glclassid in ('ABAR', 'MANI') " + where +
                        "     group by  descr , siteid, invtid, glclassid, stkunit  ) t where (t.final <> 0 or t.UnVend is not null)  or (t.INICIAL <> 0 or t.UnVend is not null)  ORDER BY t.Descrip";
                //                ,
                //  MAX(b.trandate) as 'F.UltimaActualización '  ,(select min(rcptdate) fromstock_compra_gen x
                //  where x.invtid = b.invtid ) as InicialFechaCompra ,
                //   (select min(unitcost)  fromstock_compra_gen
                //   where InvtID = b.InvtID and rcptdate =
                // (select  min(rcptdate)  fromstock_compra_gen where InvtID = b.InvtID) 
                //   group by rcptdate    ) as InicialMontoUnitarioCompra,
                //   (select top 1 CONVERT(varchar, a.curyid)  fromstock_compra_gen a
                //where b.invtid = a.invtid  and rcptdate =
                //(select  min(rcptdate)  fromstock_compra_gen where InvtID = b.InvtID    ) ) 
                //as InicialMoneda  ,(select max(rcptdate) fromstock_compra_gen x where x.invtid = b.invtid ) 
                //as UltimaFechaCompra ,(select max(unitcost)  fromstock_compra_gen
                // where InvtID = b.InvtID and rcptdate =
                //(select  max(rcptdate)  fromstock_compra_gen
                // where InvtID = b.InvtID   ) group by rcptdate    ) as UltimoMontoUnitarioCompra, 
                // (select top 1 CONVERT(varchar, a.curyid)  fromstock_compra_gen a
                //where b.invtid = a.invtid  and rcptdate =
                //(select  max(rcptdate)  fromstock_compra_gen where InvtID = b.InvtID    ) ) as UltimaMoneda , 
                // (select sum(neto_pesos) from V_REPORTE_THX where producto = b.invtid and bodega = b.siteid )  as Ventas
                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string busca_rut_cliente2(string p)
        {
            string scalar = "";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"select top 1 rutcliente from cliente where replace(  replace( rtrim(rutcliente), '.', '' ) , '-', '' ) = '" + p.Trim() + "'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    try
                    {
                        scalar = cmd.ExecuteScalar().ToString();
                    }
                    catch (Exception EX)
                    {
                        return "";
                    }
                }
            }
            return scalar;
        }

        internal static object productos_compras(string where)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"
                        SELECT * from Stock_compra b where producto >= 1000 and producto <= 9999 " + where + " order by fecharecep, producto";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable ultima_compra(string id_prod, string fecha)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"Select *, (select name as Proveedor from [192.168.10.8].[SoprodiUSDapp].[dbo].vendor where vendid = ProvOC) as oc, 
	 (select name as Proveedor from [192.168.10.8].[SoprodiUSDapp].[dbo].vendor where vendid = ProvLC) as lc from stock_compra b 
	 where (b.producto  = '" + id_prod.Trim() + "' or b.producto   = '" + "0" + id_prod.Trim() + "')";

                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static DataTable trae_contactos_cobranza(string p)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"	select idcont as Id, nomcont as Nombre, correocont as Correo, numcont as 'Nº', descrip as Descrip from Cobranza_Contacto where rutcliente = '" + p + "' ";


                SqlCommand cmd = new SqlCommand(sql, conn); cmd.CommandTimeout = 999999999;
                SqlDataAdapter ap = new SqlDataAdapter(cmd);
                ap.Fill(dt);
            }
            return dt;
        }

        internal static string insert_contacto_cobranza(string p, MANT_CLIENTES.contacto cont)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"insert into Cobranza_Contacto " +
                "values (@rutcliente, @nom_cont, @corr_cont, @num_cont, @descrip)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@rutcliente", p.Trim());
                    //cmd.Parameters.AddWithValue("@rut_cont", cont.rut_cont);
                    cmd.Parameters.AddWithValue("@nom_cont", cont.nom_cont);
                    cmd.Parameters.AddWithValue("@corr_cont", cont.correo_cont);
                    cmd.Parameters.AddWithValue("@num_cont", cont.num_cont);
                    cmd.Parameters.AddWithValue("@descrip", cont.descrip);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Insert contacto_cobr: " + EX.Message;
                    }
                }
            }
        }

        internal static string delete_contacto1(string rutcontact, string p)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"delete from Cobranza_Contacto " +
                " where rutcliente = @rutcliente And rutcont = @rutconct ";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@rutconct", rutcontact);
                    cmd.Parameters.AddWithValue("@rutcliente", p);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en Delete Detalle: " + EX.Message;
                    }
                }
            }
        }

        internal static string update_contacto_cobranza(string rutCliente, MANT_CLIENTES.contacto cont)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();
                string sql = @"update Cobranza_Contacto " +
                "set nomcont = @nom_cont, correocont = @corr_cont, numcont = @num_cont, descrip = @descrip where idcont = @rut_cont and rutcliente = @rutcliente";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@rutcliente", rutCliente.Trim());
                    cmd.Parameters.AddWithValue("@rut_cont", cont.rut_cont);
                    cmd.Parameters.AddWithValue("@nom_cont", cont.nom_cont);
                    cmd.Parameters.AddWithValue("@corr_cont", cont.correo_cont);
                    cmd.Parameters.AddWithValue("@num_cont", cont.num_cont);
                    cmd.Parameters.AddWithValue("@descrip", cont.descrip);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "OK";
                    }
                    catch (Exception EX)
                    {
                        return "Error en update contacto_cobr: " + EX.Message;
                    }
                }

            }
        }
    }
}
