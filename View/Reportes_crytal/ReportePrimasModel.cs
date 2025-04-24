using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace View.Reportes_crytal
{
    public class ReportePrimasModel
    {
        public decimal CODIGO_CLIENTE { get; set; }
        public decimal NRO_SOLICITUD { get; set; }
        public decimal SECUENCIA { get; set; }
        public System.DateTime FECHA_APLICACION { get; set; }
        public string CODIGO_FINANCIERA { get; set; }
        public string COLECTOR_MOVIMIENTO { get; set; }
        public decimal TIPO_TRANSACCION { get; set; }
        public Nullable<decimal> TIPO_OPERACION { get; set; }
        public decimal FORMA_DE_PAGO { get; set; }
        public System.DateTime FECHA_CALENDARIO { get; set; }
        public string CODIGO_AGENCIA_BANCO { get; set; }
        public decimal VALOR_TRANSACCION { get; set; }
        public string NUMERO_RECIBO { get; set; }
        public string CODIGO_LINEA_FINANCIERA { get; set; }
        public string NOMBRE_LINEA_FINANCIERA { get; set; }
        public string NOMBRE_COMPLETO { get; set; }
        public decimal CODIGO_AGENCIA { get; set; }
        public Nullable<decimal> TOTAL_INGRESOS { get; set; }
    }
}