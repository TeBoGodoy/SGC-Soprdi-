using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoprodiApp.negocio;
using SoprodiApp.entidad;
using SoprodiApp.acceso;
using System.Data;
using ThinxsysFramework;
using System.Data.SqlClient;

namespace SoprodiApp.negocio
{
    public static class ReporteRNegocio
    {
        public static string validar(ref ReporteEntidad u)
        {
            return ReporteDB.validar(ref u);
        }
        internal static string CALCULA_DESDE(int mes, int año)
        {

            string desde = "";


            if (mes == 1)
            {
                desde = "01/08/" + (año - 1).ToString();
            }
            if (mes == 2)
            {
                desde = "01/09/" + (año - 1).ToString();
            }
            if (mes == 3)
            {
                desde = "01/10/" + (año - 1).ToString();
            }
            if (mes == 4)
            {
                desde = "01/11/" + (año - 1).ToString();
            }
            if (mes == 5)
            {
                desde = "01/12/" + (año - 1).ToString();
            }
            if (mes == 6)
            {
                desde = "01/01/" + (año).ToString();
            }
            if (mes == 7)
            {
                desde = "01/02/" + (año).ToString();
            }
            if (mes == 8)
            {
                desde = "01/03/" + (año).ToString();
            }
            if (mes == 9)
            {
                desde = "01/04/" + (año).ToString();
            }
            if (mes == 10)
            {
                desde = "01/05/" + (año).ToString();
            }
            if (mes == 11)
            {
                desde = "01/06/" + (año).ToString();
            }
            if (mes == 12)
            {
                desde = "01/07/" + (año).ToString();
            }

            return desde;
        }

        internal static DataTable listar_transpor(string where)
        {
            return ReporteDB.listar_transpor(where);
        }

        internal static DataTable listar_bodegas_vm()
        {
            return ReporteDB.listar_bodegas_vm();
        }

        public static String registrar(ReporteEntidad b)
        {
            return ReporteDB.Insert(b);
        }

        internal static string trae_cliente_sp(string sp)
        {
            return ReporteDB.trae_cliente_sp(sp);
        }

        internal static string trae_fecha_sp(string sp)
        {
            return ReporteDB.trae_fecha_sp(sp);
        }

        public static String actualizar(ReporteEntidad b)
        {
            return ReporteDB.Update(b);
        }

        public static String eliminar(ReporteEntidad b)
        {
            return ReporteDB.Delete(b);
        }

        public static string encontrar(ref ReporteEntidad u)
        {
            return ReporteDB.encontrar(ref u);
        }

        internal static string trae_codigos_sincronizados_por_producto(string cod_prod)
        {
            return ReporteDB.trae_codigos_sincronizados_por_producto(cod_prod);
        }

        internal static DataTable usuarios_firman(string periodo)
        {
            return ReporteDB.usuarios_firman(periodo);
        }

        internal static DataTable traefirmas_periodo(string periodo)
        {
            return ReporteDB.traefirmas_periodo(periodo);
        }

        internal static DataTable VM_productos(string WHERE1, string WHERE2)
        {
            return ReporteDB.VM_productos(WHERE1, WHERE2);
        }

        public static DataTable listar()
        {
            return ReporteDB.GetAll();
        }

        public static DataTable listar_vendedor(string desde, string hasta)
        {
            return ReporteDB.Get_vendedor(desde, hasta);
        }

        internal static DataTable trae_grupo_firmas()
        {
            return ReporteDB.trae_grupo_firmas();
        }

        public static DataTable listar_cliente(string vendedores)
        {
            return ReporteDB.Get_clientes(vendedores);
        }

        public static DataTable listar_productos(string clientes, string vendedores)
        {
            return ReporteDB.Get_productos(clientes, vendedores);
        }

        internal static DataTable ventas_stock(string v)
        {
            return ReporteDB.ventas_stock(v);
        }

        public static DataTable listar_resumen_periodo(string where)
        {
            return ReporteDB.Get_Resumen(where);
        }

        public static DataTable listar_ALL_cliente()
        {
            return ReporteDB.Get_ALL_clientes();
        }

        internal static DataTable ultimas_compra2(string id_prod, string compras)
        {
            return ReporteDB.ultimas_compra2(id_prod, compras);
        }

        internal static DataTable carga_categoria(string where)
        {
            return ReporteDB.carga_categoria(where);
        }

        public static DataTable listar_ALL_productos()
        {
            return ReporteDB.Get_ALL_productos();
        }

        public static DataTable listar_productos_cliente(string clientes)
        {
            return ReporteDB.Get_productos_cliente(clientes);
        }

        public static DataTable listar_periodos(string perio)
        {
            return ReporteDB.listar_periodos(perio);
        }

        public static string listar_periodos_(string where)
        {
            return ReporteDB.listar_periodos_(where);
        }

        public static DataTable listar_resumen_productos(string periodos, string where)
        {
            return ReporteDB.Get_Resumen_PROD(periodos, where);
        }

        internal static DataTable listar_tipo_mov_stock(string v, string clase)
        {
            return ReporteDB.listar_tipo_mov_stock(v, clase);
        }

        internal static DataTable VM_listar_detalle_sp(string where3)
        {
            return ReporteDB.VM_listar_detalle_sp(where3);
        }

        public static DataTable listar_ALL_cliente2(string p)
        {
            return ReporteDB.Get_ALL_clientes2(p);
        }

        public static string listar_ALL_cliente_stuff(string vendedores)
        {
            return ReporteDB.listar_clientes_(vendedores);
        }

        internal static string insert_futuro_stock(string cod_prod, string v, string cant)
        {
            return ReporteDB.insert_futuro_stock(cod_prod, v, cant);
        }

        public static double Facturación_Mes(string periodo, string where)
        {
            return ReporteDB.Facturación_Mes(periodo, where);

        }

        public static int _cltes_con_vta(string periodo, string where)
        {
            return ReporteDB._cltes_con_vta(periodo, where);
        }

        public static List<long> Datos_para_percentil(string periodo, string where)
        {
            return ReporteDB.Datos_para_percentil(periodo, where);
        }

        internal static DataTable listar_ALL_productos_no_kg_(string v, string clase)
        {
            return ReporteDB.listar_ALL_productos_no_kg_(v, clase);
        }

        public static int cltes_sobre_este_percentil(string periodo, string where, Double percentil)
        {

            return ReporteDB.cltes_sobre_este_percentil(periodo, where, percentil);
        }

        public static Double sum_sobre_este_percentil(string periodo, string where, Double percentil)
        {
            return ReporteDB.sum_sobre_este_percentil(periodo, where, percentil);
        }

        internal static DataTable trae_transportistas(string where)
        {
            return ReporteDB.trae_transportistas(where);
        }

        internal static DataTable excel_valor(string cod, string fecha, string columna)
        {
            return ReporteDB.excel_valor(cod, fecha, columna);
        }

        public static int cont_si_repite(string periodo_apreguntar, string periodo_anterior, string where)
        {
            return ReporteDB.cont_si_repite(periodo_apreguntar, periodo_anterior, where);
        }

        public static double sum_a_recuperar(string periodo_apreguntar, string periodo_anterior, string where)
        {
            return ReporteDB.sum_a_recuperar(periodo_apreguntar, periodo_anterior, where);
        }

        public static DataTable listar_clientes(string p1, string p2)
        {
            return ReporteDB.listar_clientes(p1, p2);
        }

        public static Double cont_sin_x_2(string periodo_apreguntar, string periodo_anterior, string periodo_2_meses, string where)
        {
            return ReporteDB.cont_sin_x_2(periodo_apreguntar, periodo_anterior, periodo_2_meses, where);
        }

        public static Double sum_sin_x_2(string periodo_apreguntar, string periodo_anterior, string periodo_2_meses, string where)
        {
            return ReporteDB.sum_sin_x_2(periodo_apreguntar, periodo_anterior, periodo_2_meses, where);
        }

        public static Double cont_nuevo_cliente(string periodo_apreguntar, string where)
        {
            return ReporteDB.cont_nuevo_cliente(periodo_apreguntar, where);
        }

        internal static DataTable trae_comisiones(string where)
        {
            return ReporteDB.trae_comisiones(where);
        }

        public static Double sum_nuevo_cliente(string periodo_apreguntar, string where)
        {
            return ReporteDB.sum_nuevo_cliente(periodo_apreguntar, where);
        }

        internal static DataTable ultimas_compra3(string intvID, string compras)
        {
            return ReporteDB.ultimas_compra3(intvID, compras);
        }

        public static DataTable listar_clientes_nuevos(string periodo_mayor, string periodo_menor, string where)
        {
            return ReporteDB.listar_clientes_nuevos(periodo_mayor, periodo_menor, where);
        }

        public static string listar_periodo_nuevo(string hasta)
        {
            return ReporteDB.listar_periodo_nuevo(hasta);
        }

        public static string total_nuevos(string periodo_para_nuevos, string periodo_menor, string where)
        {
            return ReporteDB.total_nuevos(periodo_para_nuevos, periodo_menor, where);
        }

        public static string cont_nuevos(string periodo_para_nuevos, string periodo_menor, string where)
        {
            return ReporteDB.cont_nuevos(periodo_para_nuevos, periodo_menor, where);
        }

        public static Double COMPARATIVO_CLTE_VENTAS(string periodo_apreguntar, string periodo_anterior, string where)
        {
            return ReporteDB.COMPARATIVO_CLTE_VENTAS(periodo_apreguntar, periodo_anterior, where);
        }

        public static Double sum_si_repite(string periodo_apreguntar, string periodo_anterior, string where)
        {
            return ReporteDB.sum_si_repite(periodo_apreguntar, periodo_anterior, where);
        }

        internal static DataTable trae_docu_calend_CERRADOS(string tipo_doc, string usuario)
        {
            return ReporteDB.trae_docu_calend_CERRADOS(tipo_doc, usuario);
        }

        public static Double sum_no_repite(string periodo_apreguntar, string periodo_anterior, string where)
        {
            return ReporteDB.sum_no_repite(periodo_apreguntar, periodo_anterior, where);
        }

        public static Double sum_si_repite_actual(string periodo_apreguntar, string periodo_anterior, string where)
        {

            return ReporteDB.sum_si_repite_actual(periodo_apreguntar, periodo_anterior, where);
        }

        internal static DataTable listar_notas_credito(string where)
        {
            return ReporteDB.listar_notas_credito(where);
        }

        public static Double cont_sobre_este_percentil(string periodo, string where, double p)
        {
            return ReporteDB.cont_sobre_este_percentil(periodo, where, p);
        }

        public static Double cont_nuevo_venta(string periodo_apreguntar, string periodo_anterior, string where)
        {
            return ReporteDB.cont_nuevo_venta(periodo_apreguntar, periodo_anterior, where);
        }

        public static Double sum_nuevo_venta(string periodo_apreguntar, string periodo_anterior, string where)
        {
            return ReporteDB.sum_nueva_venta(periodo_apreguntar, periodo_anterior, where);
        }

        internal static DataTable ultima_compra2(string v, string prod, string fechthx, string desde, string hasta)
        {
            return ReporteDB.ultima_compra2(v, prod, fechthx, hasta, desde);
        }

        public static DataTable listar_vendedor_cm(string p1, string p2, string p3)
        {
            return ReporteDB.listar_vendedor_cm(p1, p2, p3);
        }

        internal static DataTable historial_stock_diario(string where, string compras)
        {
            return ReporteDB.historial_stock_diario(where, compras);
        }

        public static DataTable listar_vendedor_sala(string p1, string p2, string p3)
        {
            return ReporteDB.listar_vendedor_sala(p1, p2, p3);
        }

        internal static DataTable PIVOT_EXCEL(string v)
        {
            return ReporteDB.PIVOT_EXCEL(v);
        }

        public static DataTable cargar_usuarios()
        {
            return ReporteDB.cargar_usuarios();
        }


        public static DataTable carga_grupos()
        {
            return ReporteDB.carga_grupos();

        }

        public static DataTable carga_grupos(string desde, string hasta, string grupo_usuario)
        {
            return ReporteDB.carga_grupos(desde, hasta, grupo_usuario);

        }


        public static string grupos_usuario(string p)
        {
            return ReporteDB.grupos_usuario(p);
        }

        internal static DataTable periodos_productos(string periodo)
        {
            return ReporteDB.periodos_productos(periodo);
        }

        internal static DataTable sincronizadas_detalle(string cod, string productos, string hasta, string bodegas)
        {
            return ReporteDB.sincronizadas_detalle(cod, productos, hasta, bodegas);
        }

        public static DataTable listar_ALL_vendedores(string where)
        {
            return ReporteDB.listar_ALL_vendedores(where);
        }

        internal static string eliminar_firma(string usuario_, string periodo, string si_no, string grupo)
        {
            return ReporteDB.eliminar_firma(usuario_, periodo, si_no, grupo);
        }

        public static DataTable listar_prod_client(string vendedor, string cliente, string periodos, string user1)
        {
            return ReporteDB.listar_prod_client(vendedor, cliente, periodos, user1);
        }

        public static DataTable listar_ventas_SALA(string periodos, string where)
        {
            return ReporteDB.listar_ventas_SALA(periodos, where);
        }

        internal static DataTable productos_stock_sp(string where, string fecharcp2, string fechaSaldo, string desde, string hasta, string futuro, string clase)
        {
            return ReporteDB.productos_stock_sp(where, fecharcp2, fechaSaldo, desde, hasta, futuro, clase);
        }

        public static string es_su_pass(string p)
        {
            return ReporteDB.es_su_pass(p);
        }

        internal static object trae_cliente_vendedor_SINVENTA(string where)
        {
            return ReporteDB.trae_cliente_vendedor_SINVENTA(where);
        }

        public static string esvendedor(string usuario)
        {
            return ReporteDB.esvendedor(usuario);
        }

        internal static DataTable ajustes(string where, string fecha_thx)
        {
            return ReporteDB.ajustes(where, fecha_thx);
        }

        internal static DataTable ventas_ayer_anteayer(string where2, string desde, string hasta)
        {
            return ReporteDB.ventas_ayer_anteayer(where2, desde, hasta);
        }

        public static string grupos_usuario_v_report(string p)
        {
            return ReporteDB.grupos_usuario_v_report(p);
        }

        internal static DataTable compras_gen(string compra, string v)
        {
            return ReporteDB.compras_gen(compra);
        }

        internal static string insert_datos_excel(REPORTE_EXCEL_COSTOS.excel_f ex)
        {
            return ReporteDB.insert_datos_excel(ex);
        }

        public static int es_su_tipo(string USER)
        {
            return ReporteDB.es_su_tipo(USER);
        }

        public static string GUARDA_BOTON(int p, string usuario, string fecha)
        {
            return ReporteDB.GUARDA_BOTON(p, usuario, fecha);
        }

        internal static DataTable trae_4_semanas()
        {
            return ReporteDB.trae_4_semanas();
        }

        internal static DataTable facturas_gestionadas(string v)
        {
            return ReporteDB.facturas_gestionadas(v);
        }

        internal static DataTable detalle_matriz_click(string where)
        {
            return ReporteDB.detalle_matriz_click(where);
        }

        public static DataTable listar_diario1(string USER)
        {
            return ReporteDB.listar_diario1(USER);
        }

        public static DataTable listar_diario2(string USER)
        {
            return ReporteDB.listar_diario2(USER);
        }


        public static DataTable listar_usuarios()
        {
            return ReporteDB.listar_usuarios();
        }

        internal static DataTable sp_diaria(string WHERE)
        {
            return ReporteDB.sp_diaria(WHERE);
        }

        public static string delete_precalcular(string cod_usuario, string periodo)
        {
            return ReporteDB.delete_precalcular(cod_usuario, periodo);
        }

        public static string insert_precalcular_x_factor(string cod_usuario, string nombre_factor, string factor1, string factor2, string periodo1, string periodo2, string total_g)
        {
            return ReporteDB.insert_precalcular_x_factor(cod_usuario, nombre_factor, factor1, factor2, periodo1, periodo2, total_g);
        }

        internal static object detalle_matriz_click_cliente(string where)
        {
            return ReporteDB.detalle_matriz_click_cliente(where);
        }

        public static string insert_precalcular_x_cliente(string cod_usuario, string vendedor, string cliente, string factor1, string factor2, string periodo1, string periodo2)
        {
            return ReporteDB.insert_precalcular_x_cliente(cod_usuario, vendedor, cliente, factor1, factor2, periodo1, periodo2);
        }

        public static string delete_precalcular2(string cod_usuario, string periodo)
        {
            return ReporteDB.delete_precalcular2(cod_usuario, periodo);
        }

        public static DataTable es_nuevo(string periodo_para_nuevos, string cliente, string vendedor)
        {
            return ReporteDB.es_nuevo(periodo_para_nuevos, cliente, vendedor);
        }

        internal static string eliminar_pago_fac(string factura, string fecha, string obs)
        {
            return ReporteDB.eliminar_pago_fac(factura, fecha, obs);
        }

        internal static DataTable listar_clientes_cobranza(string where)
        {
            return ReporteDB.listar_clientes_cobranza(where);
        }

        internal static string trae_reglas(string codvendedor)
        {
            return ReporteDB.trae_reglas(codvendedor);
        }

        internal static string eliminar_segui_id(string id)
        {
            return ReporteDB.eliminar_segui_id(id);
        }

        public static string Facturación_Mes_precalculado1(string USER)
        {
            return ReporteDB.Facturación_Mes_precalculado1(USER);
        }

        internal static DataTable peso_dolar_semana(string v, string an)
        {
            return ReporteDB.peso_dolar_semana(v, an);
        }

        internal static DataTable listar_clientes_cobranza_abiertos(string where)
        {
            return ReporteDB.listar_clientes_cobranza_abiertos(where);
        }

        public static string Facturación_Mes_precalculado2(string USER)
        {
            return ReporteDB.Facturación_Mes_precalculado2(USER);
        }

        public static DataTable carga_vendedor(string p1, string p2, string grupos_del_usuario)
        {
            return ReporteDB.carga_vendedor(p1, p2, grupos_del_usuario);
        }

        internal static DataTable detalle_producto_stock(string where, string mes, string año)
        {
            return ReporteDB.detalle_producto_stock(where, mes, año);
        }

        public static DataTable carga_cliente(string p1, string p2, string grupos_del_usuario)
        {
            return ReporteDB.carga_cliente(p1, p2, grupos_del_usuario);
        }

        public static string Facturación_Mes_precalculado3(string USER)
        {
            return ReporteDB.Facturación_Mes_precalculado3(USER);
        }

        public static string Facturación_Mes_precalculado4(string USER)
        {
            return ReporteDB.Facturación_Mes_precalculado4(USER);
        }

        public static string pregunta_periodo_prod(string USER, string p)
        {
            return ReporteDB.pregunta_periodo_prod(USER, p);
        }

        internal static DataTable detalle_producto_stock_2(string codicion_noventa, string mes, string v, string where_simple)
        {
            return ReporteDB.detalle_producto_stock_2(codicion_noventa, mes, v, where_simple);
        }

        public static string listar_periodo_nuevo_(string where)
        {
            return ReporteDB.listar_periodo_nuevo_2(where);
        }

        public static DataTable LISTAR_VENDEDORES_POR_BODEGA(string where)
        {
            return ReporteDB.LISTAR_VENDEDORES_POR_BODEGA(where);
        }

        public static string venta_fecha_vendedor(string DESDE, string p)
        {
            return ReporteDB.venta_fecha_vendedor(DESDE, p);
        }

        internal static DataTable listar_mov_stock(string where3)
        {
            return ReporteDB.listar_mov_stock(where3);
        }

        internal static int prom_3_meses_atras(string meses_atras_3, string rutcliente, string codvend)
        {
            return ReporteDB.prom_3_meses_atras(meses_atras_3, rutcliente, codvend);
        }

        internal static DataTable fact_sin_comision(string periodo, string where1, string where2)
        {
            return ReporteDB.fect_sin_comision(periodo, where1, where2);
        }

        internal static DataTable trae_reglas_2(string where)
        {
            return ReporteDB.trae_reglas_2(where);
        }

        internal static DataTable ultimo_excel_dt(string fecha_g)
        {
            return ReporteDB.ultimo_excel_dt(fecha_g);
        }

        public static string cont_fact_fecha_vendedor(string DESDE, string p)
        {
            return ReporteDB.cont_fact_fecha_vendedor(DESDE, p);
        }

        public static DataTable table_totales(string where)
        {
            return ReporteDB.table_totales(where);
        }

        public static DataTable carga_bodega(string where)
        {
            return ReporteDB.carga_bodega(where);
        }

        public static DataTable cargar_detalle_lv(string where)
        {
            return ReporteDB.cargar_detalle_lv(where);
        }

        internal static DataTable facturas_solomon(string v)
        {
            return ReporteDB.facturas_solomon(v);
        }

        internal static DataTable arrastre_noventa(string condicion, string selectedValue, string text, string dias)
        {
            return ReporteDB.arrastre_noventa(condicion, selectedValue, text, dias);
        }

        public static int _cltes_con_vta2(string where)
        {
            return ReporteDB._cltes_con_vta2(where);
        }

        internal static string trae_obs_cobranza(string factura, string obs)
        {
            return ReporteDB.trae_obs_cobranza(factura, obs);
        }

        internal static string trae_num_factura_origen(string factura)
        {
            return ReporteDB.trae_num_factura_origen(factura);
        }

        public static DataTable table_totales_c(string where)
        {
            return ReporteDB.table_totasles_c(where);
        }

        internal static string trae_stuff_facturas_de_cheque(string factura, string fecha)
        {
            return ReporteDB.trae_stuff_facturas_de_cheque(factura, fecha);
        }

        internal static DataTable valor_prod_equivale_sp(string sw)
        {
            return ReporteDB.valor_prod_equivale_sp(sw);
        }

        internal static DataTable dia_entrada_noven(string condicion, string selectedValue, string text, int i, string dias)
        {
            return ReporteDB.dia_entrada_noven_y_salida(condicion, selectedValue, text, i, dias);
        }

        internal static double dia_salida(string condicion, string selectedValue, string text, int i)
        {
            return ReporteDB.dia_salida(condicion, selectedValue, text, i);
        }

        public static DataTable carga_unidad_negocio()
        {
            return ReporteDB.carga_unidad_negocio();
        }

        internal static string trae_stuff_facturas(string factura)
        {
            return ReporteDB.trae_stuff_facturas(factura);
        }

        internal static string guardar_valor_equivale_sp(string sw, string tipo, string valor, string unidades)
        {
            return ReporteDB.guardar_valor_equivale_sp(sw, tipo, valor, unidades);
        }

        public static DataTable carga_grupo_unidad(string where)
        {
            return ReporteDB.carga_grupo_unidad(where);
        }

        public static DataTable carga_app_unidad(string where)
        {
            return ReporteDB.carga_app_unidad(where);
        }


        public static DataTable carga_unidad_vend(string grupo)
        {
            return ReporteDB.carga_unidad_vend(grupo);
        }

        public static DataTable carga_grupo_vend(string grupo)
        {
            return ReporteDB.carga_grupo_vend(grupo);
        }

        public static DataTable carga_pantallas_vend(string grupo)
        {
            return ReporteDB.carga_pantallas_vend(grupo);
        }

        public static string trae_u_negocio(string p)
        {
            return ReporteDB.trae_u_negocio(p);
        }

        internal static DataTable bodega_transportista(string where1)
        {
            return ReporteDB.bodega_transportista(where1);
        }

        internal static double Facturación__ayer(string where, string dia)
        {
            return ReporteDB.Facturación__ayer(where, dia);
        }

        public static string trae_app(string p)
        {
            return ReporteDB.trae_app(p);
        }

        public static string obtienenom_unidad(string p)
        {
            return ReporteDB.obtienenom_unidad(p);
        }

        internal static DataTable VM_estados_asignados(string v)
        {
            return ReporteDB.VM_estados_asignados(v);
        }

        public static string obtiene_todos_u_negocio()
        {
            return ReporteDB.obtiene_todos_u_negocio();
        }

        internal static DataTable trae_registro(string id)
        {
            return ReporteDB.trae_registro(id);
        }

        internal static DataTable productos_para_permanencia(string where)
        {
            return ReporteDB.productos_para_permanencia(where);
        }

        internal static double cant_clientes__ayer(string where, string dia)
        {
            return ReporteDB.cant_clientes__ayer(where, dia);
        }

        internal static DataTable PERMANENCIA_NEGA(string v)
        {
            return ReporteDB.PERMANENCIA_NEGA(v);
        }

        internal static DataTable PERMANENCIA(string v, string desde, string where_arrastre, string clase)
        {
            return ReporteDB.PERMANENCIA(v, desde, where_arrastre, clase);
        }

        public static string obtiene_todos_app()
        {
            return ReporteDB.obtiene_todos_app();
        }

        internal static string trae_ids_segui(string v, string fecha, string obs)
        {
            return ReporteDB.trae_ids_segui(v, fecha, obs);
        }

        internal static DataTable PLANI_CAMIONES(string v)
        {
            return ReporteDB.PLANI_CAMIONES(v);
        }

        public static string obtiene_todos_grupos()
        {
            return ReporteDB.obtiene_todos_grupos();
        }

        public static DataTable carga_unidad_otro(string p)
        {
            return ReporteDB.carga_unidad_otro(p);
        }

        internal static string stuff_facturas_pagos(string num_factura_origen)
        {
            return ReporteDB.stuff_ids_pagos(num_factura_origen);
        }

        internal static string eliminar_por_like_obs(string cheques_obs)
        {
            return ReporteDB.eliminar_por_like_obs(cheques_obs);
        }

        internal static string stuff_ids_pagos(string facturas_pagos)
        {
            return ReporteDB.stuff_ids_pagos22222(facturas_pagos);
        }

        internal static string stuff_ids_cheques(string num_factura_origen)
        {
            return ReporteDB.stuff_ids_cheques(num_factura_origen);
        }

        public static string negocio_usuario(string p)
        {
            return ReporteDB.negocio_usuario(p);
        }

        public static DataTable productos_dias(string where)
        {
            return ReporteDB.productos_dias(where);
        }

        internal static DataTable VM_listaVendedor(string v, string where)
        {
            return ReporteDB.VM_listaVendedor(v, where);
        }

        public static DataTable productos_periodos(string where)
        {
            return ReporteDB.productos_periodos(where);
        }

        internal static DataTable cargar_obs_ponbr(string factura)
        {
            return ReporteDB.cargar_obs_ponbr(factura);
        }

        internal static double Facturación_Mes_dolar(string periodo, string where)
        {
            return ReporteDB.Facturación_Mes_dolar(periodo, where);
        }

        internal static DataTable VM_lista_grupo(string v)
        {
            return ReporteDB.VM_lista_grupo(v);
        }

        public static string negocio_usuario_por_grupos(string grupo)
        {
            return ReporteDB.negocio_usuario_por_grupos(grupo);
        }

        internal static DataTable cargar_detalle_lv_producto(string where)
        {
            return ReporteDB.cargar_detalle_lv_producto(where);
        }

        internal static string eliminar_accion_prio(string id, string id_cobranza)
        {
            return ReporteDB.eliminar_accion_prio(id, id_cobranza);
        }

        internal static DataTable detalle_cliente(string grupo, string user1)
        {
            return ReporteDB.detalle_cliente(grupo, user1);
        }

        internal static DataTable listar_transpor_solo_plani(string where2)
        {
            return ReporteDB.listar_transpor_solo_plani(where2);
        }

        internal static string nombre_vendedor(string p)
        {
            return ReporteDB.nombre_vendedor(p);
        }

        internal static string nombre_cliente(string p)
        {
            return ReporteDB.nombre_cliente(p);
        }

        internal static DataTable listar_ALL_productos_stock_granos(string v)
        {
            return ReporteDB.listar_ALL_productos_stock_granos(v);
        }

        internal static string cod_vendedor(string p)
        {
            return ReporteDB.cod_vendedor(p);
        }

        internal static DataTable listar_ALL_productos_stock_todos(string v)
        {
            return ReporteDB.listar_ALL_productos_stock_todos(v);
        }

        internal static string cod_producto(string p)
        {
            return ReporteDB.cod_producto(p);
        }

        internal static string nombre_producto(string p)
        {
            return ReporteDB.nombre_producto(p);
        }

        internal static DataTable VM_estados(string v)
        {
            return ReporteDB.VM_estados(v);
        }
        internal static DataTable VM_estados_2(string v)
        {
            return ReporteDB.VM_estados_2(v);
        }
        internal static DataTable VM_clientes(string v, string where)
        {
            return ReporteDB.VM_clientes(v, where);
        }

        internal static DataTable trae_encabezado(string p)
        {
            return ReporteDB.trae_encabezado(p);
        }

        internal static DataTable detalle_Factura(string p)
        {
            return ReporteDB.detalle_Factura(p);
        }

        internal static DataTable listar_camion_dia(string where1)
        {
            return ReporteDB.listar_camion_dia(where1);
        }

        internal static DataTable list_ficha_cliente(string where, string desde, string hasta)
        {
            return ReporteDB.list_ficha_cliente(where, desde, hasta);
        }

        internal static DataTable datos_cliente(string p)
        {
            return ReporteDB.datos_cliente(p);
        }

        internal static object trae_cta_cte_cliente(string p)
        {
            return ReporteDB.trae_cta_cte_cliente(p);
        }

        internal static DataTable listar_resumen_productos_ficha(string where)
        {
            return ReporteDB.listar_resumen_productos_ficha(where);
        }

        internal static string update_cliente(string rutcliente, string direccion, string ciudad, string pais, string fono, string vendedor)
        {
            return ReporteDB.update_cliente(rutcliente, direccion, ciudad, pais, fono, vendedor);
        }

        internal static string update_cliente2(string p1, string p2)
        {
            return ReporteDB.update_cliente2(p1, p2);
        }

        internal static DataTable excel_gama(string v)
        {
            return ReporteDB.excel_gama(v);
        }

        internal static DataTable cierre_camion(int existe_id_cierre)
        {
            return ReporteDB.cierre_camion(existe_id_cierre);
        }

        internal static string insert_cliente_nuevo(string p1, string nomb, string p2, string p3, string p3_, string p4, string p5)
        {
            return ReporteDB.insert_cliente_nuevo(p1, nomb, p2, p3, p3_, p4, p5);
        }

        internal static string cuenta_banco(string factura)
        {
            return ReporteDB.cuenta_banco(factura);
        }

        internal static string busca_rut_cliente(string p)
        {
            return ReporteDB.busca_rut_cliente(p);
        }

        internal static DataTable clientes_nuevos_ficha(string vend)
        {
            return ReporteDB.clientes_nuevos_ficha(vend);
        }

        internal static DataTable tipo_negocio()
        {
            return ReporteDB.tipo_negocio();
        }

        internal static string insert_cliente2(int sw, DateTime f, string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p12, string p22, string p32, string p42, string p52, string p62)
        {
            return ReporteDB.insert_cliente2(sw, f, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p12, p22, p32, p42, p52, p62);
        }

        internal static DataTable lista_vencimientos(string where)
        {
            return ReporteDB.lista_vencimientos(where);
        }

        internal static DataTable productos_entradas(string where)
        {
            return ReporteDB.productos_entradas(where);
        }

        internal static string insert_contactos_cliente(int sw, string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9)
        {
            return ReporteDB.insert_contactos_cliente(sw, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        }

        internal static DataTable listar_ALL_productos_stock_guion_2(string v, string clase)
        {
            return ReporteDB.listar_ALL_productos_stock_guion_2(v, clase);
        }

        internal static string insert_ref_banco_cliente(int sw, string p2, string p3, string p4, string p5, string p6)
        {
            return ReporteDB.insert_ref_banco_cliente(sw, p2, p3, p4, p5, p6);
        }

        internal static double trae_equivale_kg(string invtid)
        {
            return ReporteDB.trae_equivale_kg(invtid);
        }

        internal static string insert_sociedad_cliente(string p1, string p2, string p3, string p4, string p7, string pais_so, string p5, string p6)
        {
            return ReporteDB.insert_sociedad_cliente(p1, p2, p3, p4, p7, pais_so, p5, p6);
        }

        internal static DataTable datos_ids(string v)
        {
            return ReporteDB.datos_ids(v);
        }

        internal static string insert_socios_cliente(int sw, string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9)
        {
            return ReporteDB.insert_socios_cliente(sw, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        }

        internal static string VM_updateSP(string cod_documento, int v)
        {
            return ReporteDB.VM_updateSP(cod_documento, v);
        }

        internal static DataTable salidas_producto(string where)
        {
            return ReporteDB.salidas_producto(where);
        }

        internal static string trae_tipo_negocio_nombre(string p)
        {
            return ReporteDB.trae_tipo_negocio_nombre(p);
        }

        internal static DataTable corr_nuevos_clie()
        {
            return ReporteDB.corr_nuevos_clie();
        }

        internal static DataTable datos_cliente_ALL(int sw, string rutcliente)
        {
            return ReporteDB.datos_cliente_ALL(sw, rutcliente);
        }

        internal static DataTable lista_compras(string where3)
        {
            return ReporteDB.lista_compras(where3);
        }

        internal static string count_socios(string clie_rut)
        {
            return ReporteDB.count_socios(clie_rut);
        }

        internal static DataTable VM_listar_sp(string where3)
        {
            return ReporteDB.VM_listar_sp(where3);
        }

        internal static DataTable stock_facturado(string desde_1, string desde_2, string where, string fecha_com)
        {
            return ReporteDB.stock_facturado(desde_1, desde_2, where, fecha_com);
        }

        internal static string trae_grupo_stock(string producto)
        {
            return ReporteDB.trae_grupo_stock(producto);
        }

        internal static DataTable lista_costosimpot(string where3)
        {
            return ReporteDB.lista_costosimpot(where3);
        }

        internal static DataTable socio_extra(string clie_rut)
        {
            return ReporteDB.socio_extra(clie_rut);
        }

        internal static DataTable crear_estado_2()
        {
            return ReporteDB.crear_estado_2();
        }

        internal static DataTable crear_estado_3()
        {
            return ReporteDB.crear_estado_3();
        }

        internal static string count_bancas(string clie_rut)
        {
            return ReporteDB.count_bancas(clie_rut);
        }

        internal static DataTable banco_extra(string clie_rut)
        {
            return ReporteDB.banco_extra(clie_rut);
        }

        internal static DataTable listar_camiones_asignados(string where)
        {
            return ReporteDB.listar_camiones_asignados(where);
        }

        internal static string count_contactos(string clie_rut)
        {
            return ReporteDB.count_contactos(clie_rut);
        }

        internal static DataTable contacto_extra(string clie_rut)
        {
            return ReporteDB.contacto_extra(clie_rut);
        }

        internal static DataTable traer_usd_cobro(string mes, string año)
        {
            return ReporteDB.traer_usd_cobro(mes, año);
        }

        internal static DataTable PERMANENCIA2(string where3, string desde, string where2, string clase)
        {
            return ReporteDB.PERMANENCIA2(where3, desde, where2, clase);
        }

        internal static string delete_cliente2(string p)
        {
            return ReporteDB.delete_cliente2(p);
        }

        internal static string delete_contactos(string p)
        {
            return ReporteDB.delete_contactos(p);
        }

        internal static string delete_cuentas_bank(string p)
        {
            return ReporteDB.delete_cuentas_bank(p);
        }

        internal static DataTable VM_listar_detalle(string where3)
        {
            return ReporteDB.VM_listar_detalle(where3);
        }

        internal static string delete_socios(string p)
        {
            return ReporteDB.delete_socios(p);
        }

        internal static string delete_sociedad(string p)
        {
            return ReporteDB.delete_sociedad(p);
        }

        internal static object detalle_monto_click(string p1)
        {
            return ReporteDB.detalle_monto_click(p1);
        }

        internal static DataTable valor_prod_equivale(string v)
        {
            return ReporteDB.valor_prod_equivale(v);
        }

        internal static DataTable SP_Marcelo(string v1, string v2, string v3)
        {
            return ReporteDB.SP_Marcelo(v1, v2, v3);
        }

        internal static string buscar_vencimiento(string invtid, string siteid, string batnbr, string refnbr, string trandate, string trantype)
        {
            return ReporteDB.buscar_vencimiento(invtid, siteid, batnbr, refnbr, trandate, trantype);
        }

        internal static string insert_vencimiento(string invtid, string siteid, string batnbr, string refnbr, string trandate, string trantype, string texto_, string sw_)
        {
            return ReporteDB.insert_vencimiento(invtid, siteid, batnbr, refnbr, trandate, trantype, texto_, sw_);
        }

        internal static string update_vencimiento(string id, string texto_, string sw_)
        {
            return ReporteDB.update_vencimiento(id, texto_, sw_);

        }

        internal static string trae_correos_bodega(string bodega, string grupo)
        {
            return ReporteDB.trae_correos_bodega(bodega, grupo);
        }

        internal static double _cltes_nuevos_12_mes(string periodo_apreguntar, string where)
        {
            return ReporteDB._cltes_nuevos_12_mes(periodo_apreguntar, where);
        }

        internal static string insert_compra_sys(string invtid, string ponbr, string value)
        {
            return ReporteDB.insert_compra_sys(invtid, ponbr, value);
        }

        internal static string linea_credito(string clie_rut)
        {
            return ReporteDB.linea_credito(clie_rut);
        }

        internal static DataTable VM_listar_sp_select(string where3)
        {
            return ReporteDB.VM_listar_sp_select(where3);
        }

        internal static string guardar_valor_equivale(string v, string text1, string text2)
        {
            return ReporteDB.guardar_valor_equivale(v, text1, text2);
        }

        internal static string tre_rut_cliente(string p)
        {
            return ReporteDB.tre_rut_cliente(p);
        }

        internal static string trae_año_factura(string p)
        {
            return ReporteDB.trae_año_factura(p);
        }

        internal static double _cltes_nuevos_12_mes_factu(string periodo_apreguntar, string where)
        {
            return ReporteDB._cltes_nuevos_12_mes_factu(periodo_apreguntar, where);
        }

        internal static DataTable no_planificado(string v, string grupo)
        {
            return ReporteDB.no_planificado(v, grupo);
        }

        internal static string cod_unineg(string grupo)
        {
            return ReporteDB.cod_unineg(grupo);
        }

        internal static string trae_correo_usuario(string p)
        {
            throw new NotImplementedException();
        }

        internal static string insert_estado_sp(string factura, string estado)
        {
            return ReporteDB.insert_estado_sp(factura, estado);
        }

        internal static string delete_estado_sp(string factura)
        {
            return ReporteDB.delete_estado_sp(factura);
        }

        internal static DataTable VM_saldos_sp(string where3)
        {
            return ReporteDB.VM_saldos_sp(where3);
        }

        internal static DataTable corr_usuario(string p)
        {
            return ReporteDB.corr_usuario(p);
        }

        internal static DataTable DESPA_listar_chofer(string v)
        {
            return ReporteDB.DESPA_listar_chofer(v);
        }

        internal static object privot_semanas(string query, string where)
        {
            return ReporteDB.privot_semanas(query, where);
        }

        internal static string semana_0(string hoy, string where)
        {
            return ReporteDB.semana_0(hoy, where);
        }

        internal static DataTable DESPA_listar_camion(string v)
        {
            return ReporteDB.DESPA_listar_camion(v);
        }

        internal static string vencidos_total(string where)
        {
            return ReporteDB.vencidos_total(where);
        }

        internal static DataTable listar_transpor_2(string where)
        {
            return ReporteDB.listar_transpor_2(where);
        }

        internal static string estimados_total(string where)
        {
            return ReporteDB.estimados_total(where);
        }

        internal static DataTable docu_abier(string clie_rut, string vend)
        {
            return ReporteDB.docu_abier(clie_rut, vend);
        }

        internal static string linea_credito_disponible(string p)
        {
            return ReporteDB.linea_credito_disponible(p);
        }

        internal static string guardar_usd_cobro(string mes, string año, string tasa, string valor_cobro)
        {
            return ReporteDB.guardar_usd_cobro(mes, año, tasa, valor_cobro);
        }

        internal static DataTable listar_acciones()
        {
            return ReporteDB.listar_acciones();
        }

        internal static void quitar_pa_f(string id)
        {
            ReporteDB.quitar_pa_f(id);
        }

        internal static DataTable trae_docu_calend(string where, string usuario)
        {
            return ReporteDB.trae_docu_calend(where, usuario);
        }

        internal static string insert_fecha_cobra(string num_factura, string fecha_cobro, string obs_cobro)
        {
            return ReporteDB.insert_fecha_cobra(num_factura, fecha_cobro, obs_cobro);
        }

        internal static string delete_fecha_cobro(string num_factura)
        {
            return ReporteDB.delete_fecha_cobro(num_factura);

        }

        internal static double tiene_regla_abarrote(string v)
        {
            return ReporteDB.tiene_regla_abarrote(v);
        }

        internal static string nombre_camion(string cod_camion)
        {
            return ReporteDB.nombre_camion(cod_camion);
        }

        internal static string nombre_chofer(string cod_chofer)
        {
            return ReporteDB.nombre_chofer(cod_chofer);
        }

        internal static string neto_navarro(string periodo)
        {
            return ReporteDB.neto_navarro(periodo);
        }

        internal static string insert_seguimiento(string num_factura)
        {
            return ReporteDB.insert_seguimiento(num_factura);
        }

        internal static string insert_referencia(string factura, string refer)
        {
            return ReporteDB.insert_referencia(factura, refer);
        }

        internal static DataTable trae_acciones_(string where)
        {
            return ReporteDB.trae_acciones_(where);
        }

        internal static string update_estado_ingresado(string num_factura, string estado, string id)
        {
            return ReporteDB.update_estado_ingresado(num_factura, estado, id);
        }

        internal static string prom_productos_por_factura(string periodo_apreguntar, string where)
        {
            return ReporteDB.prom_productos_por_factura(periodo_apreguntar, where);
        }

        internal static DataTable trae_rangos()
        {
            return ReporteDB.trae_rangos();
        }

        internal static DataTable trae_fact_cheque_dm()
        {
            return ReporteDB.trae_fact_cheque_dm();
        }




        internal static DataTable trae_abarr_y_granos()
        {
            return ReporteDB.trae_abarr_y_granos();
        }

        internal static string cambia_estado_doc(string num_factura, string id)
        {
            return ReporteDB.cambia_estado_doc(num_factura, id);
        }

        internal static DataTable listar_ALL_productos_stock_guion(string v)
        {
            return ReporteDB.listar_ALL_productos_stock_guion(v);
        }

        internal static string estado_app(string p, string id, string num_fac)
        {
            return ReporteDB.estado_app(p, id, num_fac);
        }

        internal static DataTable cheques_por_cobrar(string p)
        {
            return ReporteDB.cheques_por_cobrar(p);
        }

        internal static DataTable trae_cliente_vendedor(string where)
        {
            return ReporteDB.trae_cliente_vendedor(where);
        }

        internal static string insert_log_enviar_ficha(string user, string mail_usuario, string fecha_now, string destinos, string rutcliente)
        {
            return ReporteDB.insert_log_enviar_ficha(user, mail_usuario, fecha_now, destinos, rutcliente);
        }

        internal static DataTable trae_log_fich_()
        {
            return ReporteDB.trae_log_fich_();
        }

        internal static string update_estado_doc(string id_factur, string id_2, int estado)
        {
            return ReporteDB.update_estado_doc(id_factur, id_2, estado);
        }

        internal static DataTable trae_clase_clientes()
        {
            return ReporteDB.trae_clase_clientes();
        }

        internal static DataTable busca_factura(string where)
        {
            return ReporteDB.busca_factura(where);
        }

        internal static string estimado_peso_cobranz(string desde)
        {
            return ReporteDB.estimado_peso_cobranz(desde);
        }

        internal static string estimado_dolar_cobranz(string desde)
        {
            return ReporteDB.estimado_dolar_cobranz(desde);
        }

        internal static string estimado_peso_CHEQUES(string desde)
        {
            return ReporteDB.estimado_peso_cheques(desde);
        }

        internal static string estimado_dolar_CHEQUES(string desde)
        {
            return ReporteDB.estimado_dolar_cheques(desde);
        }

        internal static string delete_seguimiento_pa_f(string id)
        {
            return ReporteDB.delete_seguimiento_pa_f(id);
        }

        internal static string trae_letra_credito(string p)
        {
            return ReporteDB.trae_letra_credito(p);
        }

        internal static string trae_corr_vend_por_cliente(string p)
        {
            return ReporteDB.trae_corr_vend_por_cliente(p);
        }

        internal static DataTable listar_ALL_bodegas_stock()
        {
            return ReporteDB.listar_ALL_bodegas_stock();
        }

        internal static DataTable listar_ALL_productos_stock(string where, string clase)
        {
            return ReporteDB.listar_ALL_productos_stock(where, clase);
        }

        internal static DataTable productos_stock(string where, string fecharcp, string fecha_thx, string fechaexcel, string fechaexcelfin, string desde, string hasta)
        {
            return ReporteDB.productos_stock(where, fecharcp, fecha_thx, fechaexcel, fechaexcelfin, desde, hasta);
        }

        internal static string trae_correo(int id_correo)
        {
            return ReporteDB.trae_correo(id_correo);
        }

        internal static string nombre_transporte(string v)
        {
            return ReporteDB.nombre_transporte(v);
        }

        internal static string busca_rut_cliente2(string p)
        {
            return ReporteDB.busca_rut_cliente2(p);
        }

        internal static object productos_compras(string where)
        {
            return ReporteDB.productos_compras(where);
        }

        internal static DataTable ultima_compra(string id_prod, string fecha)
        {
            return ReporteDB.ultima_compra(id_prod, fecha);
        }

        internal static DataTable costo_producto(string desde_1, string desde_2, string where, string fecha_compra, string hasta)
        {
            return ReporteDB.costo_producto(desde_1, desde_2, where, fecha_compra, hasta);
        }

        internal static DataTable trae_contactos_cobranza(string p)
        {
            return ReporteDB.trae_contactos_cobranza(p);
        }

        internal static string insert_contacto_cobranza(string p, MANT_CLIENTES.contacto cont)
        {
            return ReporteDB.insert_contacto_cobranza(p, cont);
        }

        internal static string delete_contacto1(string rutcontact, string p)
        {
            return ReporteDB.delete_contacto1(rutcontact, p);
        }

        internal static string update_contacto_cobranza(string rutCliente, MANT_CLIENTES.contacto cont)
        {
            return ReporteDB.update_contacto_cobranza(rutCliente, cont);
        }

        internal static string estimado_peso_cobranz_solom(string desde)
        {
            return ReporteDB.estimado_peso_cobranz_solom(desde);
        }

        internal static string estimado_dolar_cobranz_solom(string desde)
        {
            return ReporteDB.estimado_dolar_cobranz_solom(desde);
        }

        internal static string delete_agendado(string num_factura)
        {
            return ReporteDB.delete_agendado(num_factura);
        }

        internal static string insert_encab_sp_asig(string sp, string cod_trans, string carga_inicial, string fecha, string text, string v, string cod_camion, 
                                                    string cod_chofer, string obs, string asignado_por, string carga_total, string orden_cargar, string vuelta)
        {
            return ReporteDB.insert_encab_sp_asig(sp, cod_trans, carga_inicial, fecha, text, v, cod_camion, cod_chofer, obs, asignado_por, carga_total, orden_cargar, vuelta);
        }

        internal static string saldo_peso(string fact)
        {
            return ReporteDB.saldo_peso(fact);
        }

        internal static string saldo_dolar(string fact)
        {
            return ReporteDB.saldo_dolar(fact);
        }

        internal static string delete_tipo_pago(string factura)
        {
            return ReporteDB.delete_tipo_pago(factura);
        }

        internal static string insert_tipo_pago(string factura, string estado)
        {
            return ReporteDB.insert_tipo_pago(factura, estado);
        }

        internal static DataTable trae_correos_hist(string where)
        {
            return ReporteDB.trae_correos_hist(where);
        }

        internal static DataTable VM_listar_detalle_sp_2(string where)
        {
            return ReporteDB.VM_listar_detalle_sp_2(where);
        }

        internal static string insert_det_sp_asig(string select_scope, double sum, string cod_prod)
        {
            return ReporteDB.insert_det_sp_asig(select_scope, sum, cod_prod);
        }

        internal static DataTable categoria_cobranza_comis()
        {
            return ReporteDB.categoria_cobranza_comis();
        }

        internal static DataTable comision_cobranza(string periodo)
        {
            return ReporteDB.comision_cobranza(periodo);
        }

        internal static string trae_correo_sp(string v)
        {
            return ReporteDB.trae_correo_sp(v);
        }

        internal static string total_kg_id(string select_scope)
        {
            return ReporteDB.total_kg_id(select_scope);
        }

        internal static string tiene_pago(string fact)
        {
            throw new NotImplementedException();
        }

        internal static object saldo_dolar_multiguia(string num_factura)
        {
            return ReporteDB.saldo_dolar_multiguia(num_factura);
        }

        internal static DataTable VM_LISTAR_SP_2(string v)
        {
            return ReporteDB.VM_LISTAR_SP_2(v);
        }

        internal static string tipo_doc(string fact)
        {
            return ReporteDB.tipo_doc(fact);
        }

        internal static string ins_en_seg(List<SPVars> toSP = null)
        {
            return ReporteDB.ins_en_seg(toSP);
        }

        internal static DataTable productos_stock2(string where, string where_stock)
        {
            return ReporteDB.productos_stock2(where, where_stock);
        }

        internal static DataTable ventas_matriz(string where)
        {
            return ReporteDB.ventas_matriz(where);
        }

        internal static DataTable productos_stock_ventas(string where_stock)
        {
            return ReporteDB.productos_stock_ventas(where_stock);
        }

        internal static DataTable clientes_periodo_anterior(string where5, string where)
        {
            return ReporteDB.clientes_periodo_anterior(where5, where);
        }

        internal static string accion_prioridad(string v)
        {
            return ReporteDB.accion_prioridad(v);
        }

        internal static DataTable listar_ALL_productos_thx(string where)
        {
            return ReporteDB.listar_ALL_productos_thx(where);
        }

        internal static DataTable costo_sobrealmacenaje(string where, string selectedValue, string año, string bodega)
        {
            return ReporteDB.costo_almacenaje(where, selectedValue, año, bodega);
        }

        internal static string update_asignada(string id_asinada)
        {
            return ReporteDB.update_asignada(id_asinada);
        }

        internal static DataTable fechas_incorrectas(string mes, string año, string where)
        {
            return ReporteDB.fechas_incorrectas(mes, año, where);
        }

        internal static DataTable lista_det_sp_asignada(string id)
        {
            return ReporteDB.lista_det_sp_asignada(id);
        }

        internal static string trae_fecha_emision_sp(string sp)
        {
            return ReporteDB.trae_fecha_emision_sp(sp);
        }

        internal static string desplanificar_sp(string codDocumento, int v)
        {
            return ReporteDB.desplanificar_sp(codDocumento, v);
        }

        internal static DataTable detalle_planificado_dia(string dia, string where)
        {
            return ReporteDB.detalle_planificado_dia(dia, where);
        }

        internal static DataTable corre_bodega(string dia, string where)
        {
            return ReporteDB.corre_bodega(dia, where);
        }

        internal static DataTable detalle_planificado_dia_camiones(string text, string where)
        {
            return ReporteDB.detalle_planificado_dia_camiones(text, where);
        }

        internal static string selec_insert_log(string v)
        {
            return ReporteDB.selec_insert_log(v);
        }

        internal static string update_asignada_replanificar(string id, string d_camion, string fecha, string carga, string transpor, string camion, string chofer, string obs, string vuelta)
        {
            return ReporteDB.update_asignada_replanificar(id, d_camion, fecha, carga, transpor, camion, chofer, obs, vuelta);
        }

        internal static DataTable log_re_planificar(string id)
        {
            return ReporteDB.log_re_planificar(id);
        }

        internal static DataTable ventamovil_sp_enc(string sp)
        {
            return ReporteDB.ventamovil_sp_enc(sp);
        }

        internal static DataTable trae_lotes_(string invtid, string siteid, string batnbr, string refnbr, string trandate, string trantype)
        {
            return ReporteDB.trae_lotes_(invtid, siteid, batnbr, refnbr, trandate, trantype);
        }

        internal static DataTable ventamovil_sp_det(string sp)
        {
            return ReporteDB.ventamovil_sp_det(sp);
        }

        internal static double existe_sp(string sp)
        {
            return ReporteDB.existe_sp(sp);
        }

        internal static DataTable ventamovil_sp_enc_EXT(string sp)
        {
            return ReporteDB.ventamovil_sp_enc_EXT(sp);
        }

        internal static DataTable ventamovil_sp_det_EXT(string sp)
        {
            return ReporteDB.ventamovil_sp_det_EXT(sp);
        }

        internal static string insert_lote(string invtid, string siteid, string batnbr, string refnbr, string trandate, string trantype, string lote, string envasado, string vencimiento)
        {
            return ReporteDB.insert_lote(invtid, siteid, batnbr, refnbr, trandate, trantype, lote, envasado, vencimiento);
        }

        internal static string delete_lote(string invtid, string siteid, string batnbr, string refnbr, string trandate, string trantype, string lote)
        {
            return ReporteDB.delete_lote(invtid, siteid, batnbr, refnbr, trandate, trantype, lote);
        }

        internal static string update_lote(string invtid, string siteid, string batnbr, string refnbr, string trandate, string trantype, string lote, string envasado, string vencimiento)
        {
            return ReporteDB.update_lote(invtid, siteid, batnbr, refnbr, trandate, trantype, lote, envasado, vencimiento);
        }

        internal static DataTable comision_cobranza_2(string periodo)
        {
            return ReporteDB.comision_cobranza_2(periodo);
        }

        internal static DataTable fechas_y_dias_vencimiento(string invtid, string siteid, string batnbr, string refnbr, string trandate, string trantype)
        {
            return ReporteDB.fechas_y_dias_vencimiento(invtid, siteid, batnbr, refnbr, trandate, trantype);
        }

        internal static string guarda_cliente_vi(string factura)
        {
            return ReporteDB.guarda_cliente_vi(factura);
        }

        internal static string quita_cliente_vi(string factura)
        {
            return ReporteDB.quita_cliente_vi(factura);
        }

        internal static string es_venta_vi(string factura)
        {
            return ReporteDB.es_venta_vi(factura);
        }

        internal static string es_ewos(string factura)
        {
            return ReporteDB.es_ewos(factura);
        }

        internal static string guarda_ewos(string factura)
        {
            return ReporteDB.guarda_ewos(factura);
        }

        internal static string quita_ewos(string factura)
        {
            return ReporteDB.quita_ewos(factura);
        }

        internal static DataTable trae_datos_importacion_comision(string factura)
        {
            return ReporteDB.trae_datos_importacion_comision(factura);
        }

        internal static DataTable trae_sp_planificada(string id)
        {
            return ReporteDB.trae_sp_planificada(id);
        }

        internal static string guarda_importacion(string factura, string contrato, string toneladas, string negocio, double porcentaje)
        {
            return ReporteDB.guarda_importacion(factura, contrato, toneladas, negocio, porcentaje);
        }

        internal static DataTable trae_sp_re_planificada(string id)
        {
            return ReporteDB.trae_sp_re_planificada(id);
        }

        internal static string quitar_importacion(string factura)
        {
            return ReporteDB.quitar_importacion(factura);
        }

        internal static string update_importacion_valida(string factura, int v)
        {
            return ReporteDB.update_importacion_valida(factura, v);
        }

        internal static DataTable listar_chofer(string where)
        {
            return ReporteDB.listar_chofer(where);
        }

        internal static DataTable listar_camion(string where)
        {
            return ReporteDB.listar_camion(where);
        }

        internal static int validar_factura_solomon(string factura)
        {
            return ReporteDB.validar_factura_solomon(factura);
        }

        internal static int existe_cierre_camion(string cod_trans, string cod_camion, string cod_chofer, string sps, string dia_plani)
        {
            return ReporteDB.existe_cierre_camion(cod_trans, cod_camion, cod_chofer, sps, dia_plani);
        }

        internal static int existe_cierre_camion_Select(string cod_trans, string cod_camion, string cod_chofer, string sps, string dia_plani)
        {
            return ReporteDB.existe_cierre_camion(cod_trans, cod_camion, cod_chofer, sps, dia_plani);
        }


        internal static string insert_det_cierre_camion(int existe_id_cierre, string factura)
        {
            return ReporteDB.insert_det_cierre_camion(existe_id_cierre, factura);
        }

        internal static DataTable trae_docu_calend_cheques(string where, string user)
        {
            return ReporteDB.trae_docu_calend_cheques(where, user);
        }

        internal static string recalcular_regla_21_toro(string periodo)
        {
            return ReporteDB.recalcular_regla_21_toro(periodo);
        }

        internal static DataTable lista_det_sp_asignada_in(string ids)
        {
            return ReporteDB.lista_det_sp_asignada_in(ids);
        }

        internal static string bodegas_sp(string sps)
        {
            return ReporteDB.bodegas_sp(sps);
        }

        internal static string trae_correos_bodega_2(string bodegas_sp, string grupo)
        {
            return ReporteDB.trae_correos_bodega_2(bodegas_sp, grupo);
        }

        internal static string precalcular_regla_11_amaro(string periodo)
        {
            return ReporteDB.precalcular_regla_11_amaro(periodo);
        }

        internal static string insert_X2POSSL(string facturas_comas)
        {
            return ReporteDB.insert_X2POSSL(facturas_comas);
        }

        internal static string insert_X2POSTROK(string facturas_comas)
        {
            return ReporteDB.insert_X2POSTROK(facturas_comas);
        }

        internal static DataTable lista_enc_sp_asignada_in(string ids, string ids_asignados)
        {
            return ReporteDB.lista_enc_sp_asignada_in(ids, ids_asignados);
        }

        internal static string insert_X2POSSL_CHEQUE(string facturas_comas)
        {
            return ReporteDB.insert_X2POSSL_CHEQUE(facturas_comas);
        }

        internal static string insert_X2POSTROK_CHEQUE(string facturas_comas)
        {
            return ReporteDB.insert_X2POSTROK_CHEQUE(facturas_comas);
        }

  
        internal static DataTable actualizar_saldos(string nombre_sp, List<SPVars> vars)
        {
            return ReporteDB.actualizar_saldos(nombre_sp, vars);
        }

        internal static DataTable firmas_listas(string periodo)
        {
            return ReporteDB.firmas_listas(periodo);
        }

        internal static string tipo_moneda(string v)
        {
            return ReporteDB.tipo_moneda(v);
        }

        internal static DataTable ventamovil_cliente_matriz(string codCliente)
        {
            return ReporteDB.ventamovil_cliente_matriz(codCliente);
        }

        internal static string ventamovil_trae_codcliente(string sp)
        {
            return ReporteDB.ventamovil_trae_codcliente(sp);
        }

        internal static DataTable ventamovil_cliente_sucursal(string codCliente)
        {
            return ReporteDB.ventamovil_cliente_sucursal(codCliente);
        }

        internal static string valida_referecia_cobranza(string descripcion)
        {
            throw new NotImplementedException();
        }
    }
}
