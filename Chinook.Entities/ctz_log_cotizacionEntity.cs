using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Entities
{
public class ctz_log_cotizacionEntity
{
  public String cod_vendedor { get; set; }
  public String columnas_precio { get; set; }
  public int correlativo { get; set; }
  public String correo_cliente { get; set; }
  public String descripcion { get; set; }
  public String estado_correo { get; set; }
  public DateTime fecha_envio { get; set; }
  public int id_cotizacion { get; set; }
  public int id_cotizacion_log { get; set; }
  public String nombre_cotizacion { get; set; }
  public int num_columnas { get; set; }
  public String rut_cliente { get; set; }
}
}
