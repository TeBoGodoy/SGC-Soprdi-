using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoprodiApp.Entities
{
    public class comisionperiodocierreEntity
    {
        public String autoriza { get; set; }
        public String cod_periodo { get; set; }
        public String cod_usuario { get; set; }
        public DateTime fecha_autoriza { get; set; }
        public DateTime fecha_cierre { get; set; }
    }
}
