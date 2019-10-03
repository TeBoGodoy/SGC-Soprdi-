using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Entities
{
public class ctz_log_cotizaciones_detEntity
{
  public Double cantidad { get; set; }
  public int id_cotizacion_det_log { get; set; }
  public int id_cotizacion_log { get; set; }
  public String nom_producto { get; set; }
  public String producto { get; set; }
  public int unidades_por_embalaje { get; set; }
}
}
