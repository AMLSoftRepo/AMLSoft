//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;

    public partial class PER_PERFIL_TRANSACCIONAL : Entity<int>
    {
        public decimal CODIGO_CLIENTE { get; set; }
        public Nullable<int> ANIO { get; set; }
        public Nullable<int> MES { get; set; }
        public Nullable<double> MONTO_MENSUAL { get; set; }
        public Nullable<int> NO_PAGOS_MENSUAL { get; set; }
        public string TIPO_TRANSACCION { get; set; }
        public string TIPO_OPERACION { get; set; }
        public string FORMA_PAGO { get; set; }
        public Nullable<double> POR_DISPONIBILIDAD { get; set; }
        public string COLECTOR { get; set; }
        public string LINEA_CREDITO { get; set; }
        public Nullable<double> TOTAL_INGRESOS { get; set; }
        public Nullable<double> VALOR_CUOTA { get; set; }
    }
}
