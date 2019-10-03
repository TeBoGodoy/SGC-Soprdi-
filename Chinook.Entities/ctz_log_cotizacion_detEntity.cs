using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Entities
{
public class ctz_log_cotizacion_detEntity
{
  public int cantidad_1 { get; set; }
  public int cantidad_2 { get; set; }
  public int cantidad_3 { get; set; }
  public String cod_bodega_1 { get; set; }
  public String cod_bodega_2 { get; set; }
  public String cod_bodega_3 { get; set; }
  public Double descuento_1 { get; set; }
  public Double descuento_2 { get; set; }
  public Double descuento_3 { get; set; }
  public int id_cotizacion_det_log { get; set; }
  public int id_cotizacion_log { get; set; }
  public String nom_producto { get; set; }
  public Double precio_1 { get; set; }
  public Double precio_2 { get; set; }
  public Double precio_3 { get; set; }
  public Double precio_con_descuento_1 { get; set; }
  public Double precio_con_descuento_2 { get; set; }
  public Double precio_con_descuento_3 { get; set; }
  public Double precio_con_descuento_unitario_1 { get; set; }
  public Double precio_con_descuento_unitario_2 { get; set; }
  public Double precio_con_descuento_unitario_3 { get; set; }
  public Double precio_unitario_1 { get; set; }
  public Double precio_unitario_2 { get; set; }
  public Double precio_unitario_3 { get; set; }
  public String producto { get; set; }
  public Double total_1 { get; set; }
  public Double total_2 { get; set; }
  public Double total_3 { get; set; }
  public Double total_iva_1 { get; set; }
  public Double total_iva_2 { get; set; }
  public Double total_iva_3 { get; set; }
  public int unidad_equivale { get; set; }
}
}
