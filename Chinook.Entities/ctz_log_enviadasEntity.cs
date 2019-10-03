using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Entities
{
public class ctz_log_enviadasEntity
{
  public String cod_vendedor { get; set; }
  public String correo_cliente { get; set; }
  public String estado_correo { get; set; }
  public DateTime fecha_envio { get; set; }
  public int id { get; set; }
  public int id_cotizacion_log { get; set; }
  public String rut_cliente { get; set; }
}
}
