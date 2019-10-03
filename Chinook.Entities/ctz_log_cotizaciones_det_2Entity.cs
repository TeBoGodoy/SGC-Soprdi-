using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Entities
{
public class ctz_log_cotizaciones_det_2Entity
{
  public String cod_bodega { get; set; }
  public Double descuento { get; set; }
  public int id_cotizacion_det_log { get; set; }
  public int id_cotizacion_det_log_2 { get; set; }
  public int id_cotizacion_log { get; set; }
  public String nombre_bodega { get; set; }
  public Double precio { get; set; }
  public Double precio_con_descuento { get; set; }
  public Double precio_con_descuento_iva { get; set; }
  public Double precio_con_descuento_unitario { get; set; }
  public Double precio_con_descuento_unitario_iva { get; set; }
  public Double precio_iva { get; set; }
  public Double precio_unitario { get; set; }
  public Double precio_unitario_iva { get; set; }
  public Double total { get; set; }
  public Double total_iva { get; set; }
}
}
