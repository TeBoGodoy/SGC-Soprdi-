using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Entities
{
public class ctz_cotizacionEntity
{
  public String cod_vendedor { get; set; }
  public String columnas_precio { get; set; }
  public String descripcion { get; set; }
  public String estado { get; set; }
  public DateTime fecha_creacion { get; set; }
  public int id_cotizacion { get; set; }
  public String nombre_cotizacion { get; set; }
  public int num_columnas { get; set; }
  public int servicios_separado { get; set; }
}
}
