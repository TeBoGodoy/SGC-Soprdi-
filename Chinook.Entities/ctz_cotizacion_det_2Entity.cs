using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Entities
{
public class ctz_cotizacion_det_2Entity
{
  public Double descuento { get; set; }
  public int id_cotizacion { get; set; }
  public int id_cotizacion_det { get; set; }
  public int id_cotizacion_det_2 { get; set; }
  public Double precio { get; set; }
  public Double precio_con_descuento { get; set; }
  public Double precio_con_descuento_unitario { get; set; }
  public Double precio_unitario { get; set; }
  public Double total { get; set; }
  public Double total_iva { get; set; }
}
}
