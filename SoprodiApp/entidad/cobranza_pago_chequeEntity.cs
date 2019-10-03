using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoprodiApp.Entities
{
public class cobranza_pago_chequeEntity
{
  public int cuenta_banco { get; set; }
  public DateTime fecha_pago { get; set; }
  public int id { get; set; }
  public String num_cheque { get; set; }
  public String usuario { get; set; }
}
}
