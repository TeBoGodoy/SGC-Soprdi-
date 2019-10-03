using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Entities
{
public class ctz_log_cotizaciones_serviciosEntity
{
  public String cod_servicio { get; set; }
  public int id_cotizacion_log { get; set; }
  public int id_cotizacion_servicio_log { get; set; }
  public String nombre_servicio { get; set; }
  public String tipo_servicio { get; set; }
  public Double valor_servicio { get; set; }
}
}
