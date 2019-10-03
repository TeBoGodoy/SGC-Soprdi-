namespace SoprodiApp
{
    internal class Sp_datos
    {

        public string sp  { get; set; }

        public string CodEmisor  { get; set; }
        public string estado_sp  { get; set; }
        //datos proveedoor;
        public string proveedor_dir = "Parad. 9 1/2 S/N Camino Troncal San Pedro";
        public string proveedor_fono = "33-292701";
        public string proveedor_fax = "33-292701";
        public string proveedor_email = "soprodi@soprodi.cl";
        public string proveedor_URL = "http://www.soprodi.cl";
        public string vendedor  { get; set; }
        public string fecha_emi  { get; set; }

        //facturar a
        public string nombre_cliente  { get; set; }
        public string rut_cliente  { get; set; }
        public string fono  { get; set; }
        public string giro  { get; set; }
        public string ciudad  { get; set; }
        public string direccion  { get; set; }

        //despachar a 
        public string fecha_desp  { get; set; }
        public string atenciona  { get; set; }
        public string horario_recept  { get; set; }
        public string cond_despacho  { get; set; }
        public string direccion_despacho { get; set; }
        public string comuna_despacho { get; set; }

        //pago
        public string bodega  { get; set; }
        public string destino  { get; set; }

        //PAGO
        public string forma_pago  { get; set; }
        public string cond_pago  { get; set; }
        public string moneda_sp  { get; set; }
        public string t_cambio  { get; set; }

        //flete
        public string bodega_o_sucurs  { get; set; }
        public string valor_flete  { get; set; }
        public string comentario  { get; set; }

        public string valor_t_cambio  { get; set; }
        public string nota_libre  { get; set; }


    }
}