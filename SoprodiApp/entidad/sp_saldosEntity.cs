using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoprodiApp.Entities
{
public class sp_saldosEntity
{
  public Double cantidad_planificada { get; set; }
  public Double cantidad_real { get; set; }
  public String CodProducto { get; set; }
  public String CodUnVenta { get; set; }
  public DateTime fecha { get; set; }
  public int id { get; set; }
  public String num_sp { get; set; }
  public Double saldo { get; set; }
}
}
