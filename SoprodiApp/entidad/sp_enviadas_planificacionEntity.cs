using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoprodiApp.Entities
{
public class sp_enviadas_planificacionEntity
{
  public String cod_vendedor { get; set; }
  public String detalle { get; set; }
  public DateTime fecha { get; set; }
  public DateTime fecha_planificacion { get; set; }
  public int id { get; set; }
  public String num_sp { get; set; }
}
}
