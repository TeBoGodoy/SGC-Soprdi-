using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Entities
{
public class ctz_log_cotizacionesEntity
{
  public String cod_vendedor { get; set; }
  public String comentario_correo { get; set; }
  public int correlativo { get; set; }
  public String correo { get; set; }
  public String descripcion { get; set; }
  public DateTime fecha_envio { get; set; }
  public int id_cotizacion_log { get; set; }
  public String nombre_cliente { get; set; }
  public String nombre_cotizacion { get; set; }
  public String rut_cliente { get; set; }
  public int servicios_separado { get; set; }
}
}
