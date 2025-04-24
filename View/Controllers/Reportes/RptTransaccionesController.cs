using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blo.Reportes;
using Microsoft.Reporting.WebForms;
using Model;

namespace View.Controllers.Reportes
{
    public class RptTransaccionesController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reportesBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptTransaccionesController(IReportesBlo reportesBlo)
        {
            _reportesBlo = reportesBlo;
        }

        // GET: RptTransacciones
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// METODO PARA GENERAR EL REPORTE DE TRANSACCIONES DEL CLIENTE
        /// </summary>
        /// <param name="codigoCliente"></param>
        /// <param name="NombreCliente"></param>
        /// <param name="formato"></param>
        [HttpGet]
        public ActionResult ReporteTransaccionesCliente(string FECHA_INI_T, string FECHA_FIN_T, int codigoCliente, string NOMBRE_T, string formato)
        {
            DateTime fi = DateTime.Parse(FECHA_INI_T);
            DateTime ff = DateTime.Parse(FECHA_FIN_T);

            Dictionary<string, object> parametersData = new Dictionary<string, object>
            {
                { "FECHA_INI_T", fi },
                { "FECHA_FIN_T", ff }
            };

            parametersData.Add("codigoCliente", codigoCliente.ToString());
            parametersData.Add("NOMBRE_T", NOMBRE_T.ToString());
            parametersData.Add("FECHA_INI_T", fi.Date.ToString());
            parametersData.Add("FECHA_FIN_T", ff.Date.ToString());

            string nombreReporte = "RptTransaccionesCliente";
            string nombreTabla = "VIEW_TRANSACCIONES";
            var datosReporte = _reportesBlo.ReporteTransaccionesCliente(fi.Date.ToString(), ff.Date.ToString(), codigoCliente, NOMBRE_T).ToList();

            if (datosReporte == null || !datosReporte.Any())
            {
                return Content("No hay datos disponibles para el reporte.");
            }

            var datosLimpios = datosReporte.Select(x => new VIEW_TRANSACCIONES
            {
                CLASE_PRODUCTO = string.IsNullOrEmpty(x.CLASE_PRODUCTO) ? "DESCONOCIDO" : x.CLASE_PRODUCTO,
                CODIGO_CLIENTE = x.CODIGO_CLIENTE ?? 0.0m,
                NUMERO_PRODUCTO = string.IsNullOrEmpty(x.NUMERO_PRODUCTO) ? "DESCONOCIDO" : x.NUMERO_PRODUCTO,
                CODIGO_USUARIO = string.IsNullOrEmpty(x.CODIGO_USUARIO) ? "DESCONOCIDO" : x.CODIGO_USUARIO,
                SECUENCIA = string.IsNullOrEmpty(x.SECUENCIA) ? "DESCONOCIDO" : x.SECUENCIA,
                FECHA_APLICACION = x.FECHA_APLICACION ?? DateTime.MinValue,
                CODIGO_FINANCIERA = string.IsNullOrEmpty(x.CODIGO_FINANCIERA) ? "DESCONOCIDO" : x.CODIGO_FINANCIERA,
                COLECTOR_MOVIMIENTO = string.IsNullOrEmpty(x.COLECTOR_MOVIMIENTO) ? "DESCONOCIDO" : x.COLECTOR_MOVIMIENTO,
                TIPO_TRANSACCION = string.IsNullOrEmpty(x.TIPO_TRANSACCION) ? "DESCONOCIDO" : x.TIPO_TRANSACCION,
                TIPO_OPERACION = string.IsNullOrEmpty(x.TIPO_OPERACION) ? "DESCONOCIDO" : x.TIPO_OPERACION,
                FORMA_PAGO = string.IsNullOrEmpty(x.FORMA_PAGO) ? "DESCONOCIDO" : x.FORMA_PAGO,
                FECHA_CALENDARIO = x.FECHA_CALENDARIO ?? DateTime.MinValue,
                CODIGO_AGENCIA_BANCO = string.IsNullOrEmpty(x.CODIGO_AGENCIA_BANCO) ? "DESCONOCIDO" : x.CODIGO_AGENCIA_BANCO,
                VALOR_TRANSACCION = x.VALOR_TRANSACCION ?? 0.0,
                NUMERO_RECIBO = string.IsNullOrEmpty(x.NUMERO_RECIBO) ? "DESCONOCIDO" : x.NUMERO_RECIBO,
                CODIGO_PATRONO = string.IsNullOrEmpty(x.CODIGO_PATRONO) ? "DESCONOCIDO" : x.CODIGO_PATRONO,
                ESTADO_CARTERA = string.IsNullOrEmpty(x.ESTADO_CARTERA) ? "DESCONOCIDO" : x.ESTADO_CARTERA,
                COLECTOR_CONVENIO = string.IsNullOrEmpty(x.COLECTOR_CONVENIO) ? "DESCONOCIDO" : x.COLECTOR_CONVENIO,
                FACTURA_CONVENIO = string.IsNullOrEmpty(x.FACTURA_CONVENIO) ? "DESCONOCIDO" : x.FACTURA_CONVENIO,
                SALDO_CAPITAL = x.SALDO_CAPITAL ?? 0.0,
                SALDO_INTERES = x.SALDO_INTERES ?? 0.0,
                SALDO_SEGUROS_IVA = x.SALDO_SEGUROS_IVA ?? 0.0,
                SALDO_OTROS = x.SALDO_OTROS ?? 0.0,
                EXCEDENTE = x.EXCEDENTE ?? 0.0,
                CAPITAL_VIGENTE = x.CAPITAL_VIGENTE ?? 0.0,
                CAPITAL_MORA = x.CAPITAL_MORA ?? 0.0,
                CAPITAL_VENCIDO = x.CAPITAL_VENCIDO ?? 0.0,
                INTERES_VIGENTE = x.INTERES_VIGENTE ?? 0.0,
                INTERES_MORA = x.INTERES_MORA ?? 0.0,
                INTERES_VENCIDO = x.INTERES_VENCIDO ?? 0.0,
                FECHA_CANCELACION = x.FECHA_CANCELACION ?? DateTime.MinValue,
                FECHA_APERTURA = x.FECHA_APERTURA ?? DateTime.MinValue,
                FECHA_VENCIMIENTO = x.FECHA_VENCIMIENTO ?? DateTime.MinValue,
                VALOR_CUOTA = x.VALOR_CUOTA ?? 0.0,
                MONTO_OTORGADO = x.MONTO_OTORGADO ?? 0.0,
                VALOR_VALUO = x.VALOR_VALUO ?? 0.0m,
                VALOR_CONTABLE = x.VALOR_CONTABLE ?? 0.0m,
                FECHA_SOLICITUD = x.FECHA_SOLICITUD ?? DateTime.MinValue,
                NOMBRE_COMPLETO = string.IsNullOrEmpty(x.NOMBRE_COMPLETO) ? "DESCONOCIDO" : x.NOMBRE_COMPLETO,
                TOTAL_INGRESOS = x.TOTAL_INGRESOS ?? 0.0m,
                CODIGO_AGENCIA = string.IsNullOrEmpty(x.CODIGO_AGENCIA) ? "DESCONOCIDO" : x.CODIGO_AGENCIA
            }).ToList();

            DataTable dtReporte = DataTableHelper.ToDataTable(datosLimpios);

            VerReporte(nombreReporte, formato, parametersData, dtReporte,
                            nombreTabla);

            return RedirectToAction("Index");

        }

        /// <summary>
        /// METODO PARA GENERAR REPORTE DE TRANSACCIONES POR RANGO DE FECHAS
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="formato"></param>
        [HttpGet]
        public ActionResult ReporteTransacciones(string fechaInicial, string fechaFinal, string formato)
        {
            DateTime fi = DateTime.Parse(fechaInicial);
            DateTime ff = DateTime.Parse(fechaFinal);

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                {"fechaInicial", fi },
                {"fechaFinal", ff }
            };

            string nombreReporte = "RptTransacciones";
            string nombreTabla = "VIEW_TRANSACCIONES";
            var datosReporte = _reportesBlo.ReporteTransacciones(fechaInicial, fechaFinal);

            if (datosReporte == null || !datosReporte.Any())
            {
                return Content("No hay datos disponibles para el reporte.");
            }

            var datosLimpios = datosReporte.Select(x => new VIEW_TRANSACCIONES
            {
                CLASE_PRODUCTO = string.IsNullOrEmpty(x.CLASE_PRODUCTO) ? "DESCONOCIDO" : x.CLASE_PRODUCTO,
                CODIGO_CLIENTE = x.CODIGO_CLIENTE ?? 0.0m,
                NUMERO_PRODUCTO = string.IsNullOrEmpty(x.NUMERO_PRODUCTO) ? "DESCONOCIDO" : x.NUMERO_PRODUCTO,
                CODIGO_USUARIO = string.IsNullOrEmpty(x.CODIGO_USUARIO) ? "DESCONOCIDO" : x.CODIGO_USUARIO,
                SECUENCIA = string.IsNullOrEmpty(x.SECUENCIA) ? "DESCONOCIDO" : x.SECUENCIA,
                FECHA_APLICACION = x.FECHA_APLICACION ?? DateTime.MinValue,
                CODIGO_FINANCIERA = string.IsNullOrEmpty(x.CODIGO_FINANCIERA) ? "DESCONOCIDO" : x.CODIGO_FINANCIERA,
                COLECTOR_MOVIMIENTO = string.IsNullOrEmpty(x.COLECTOR_MOVIMIENTO) ? "DESCONOCIDO" : x.COLECTOR_MOVIMIENTO,
                TIPO_TRANSACCION = string.IsNullOrEmpty(x.TIPO_TRANSACCION) ? "DESCONOCIDO" : x.TIPO_TRANSACCION,
                TIPO_OPERACION = string.IsNullOrEmpty(x.TIPO_OPERACION) ? "DESCONOCIDO" : x.TIPO_OPERACION,
                FORMA_PAGO = string.IsNullOrEmpty(x.FORMA_PAGO) ? "DESCONOCIDO" : x.FORMA_PAGO,
                FECHA_CALENDARIO = x.FECHA_CALENDARIO ?? DateTime.MinValue,
                CODIGO_AGENCIA_BANCO = string.IsNullOrEmpty(x.CODIGO_AGENCIA_BANCO) ? "DESCONOCIDO" : x.CODIGO_AGENCIA_BANCO,
                VALOR_TRANSACCION = x.VALOR_TRANSACCION ?? 0.0,
                NUMERO_RECIBO = string.IsNullOrEmpty(x.NUMERO_RECIBO) ? "DESCONOCIDO" : x.NUMERO_RECIBO,
                CODIGO_PATRONO = string.IsNullOrEmpty(x.CODIGO_PATRONO) ? "DESCONOCIDO" : x.CODIGO_PATRONO,
                ESTADO_CARTERA = string.IsNullOrEmpty(x.ESTADO_CARTERA) ? "DESCONOCIDO" : x.ESTADO_CARTERA,
                COLECTOR_CONVENIO = string.IsNullOrEmpty(x.COLECTOR_CONVENIO) ? "DESCONOCIDO" : x.COLECTOR_CONVENIO,
                FACTURA_CONVENIO = string.IsNullOrEmpty(x.FACTURA_CONVENIO) ? "DESCONOCIDO" : x.FACTURA_CONVENIO,
                SALDO_CAPITAL = x.SALDO_CAPITAL ?? 0.0,
                SALDO_INTERES = x.SALDO_INTERES ?? 0.0,
                SALDO_SEGUROS_IVA = x.SALDO_SEGUROS_IVA ?? 0.0,
                SALDO_OTROS = x.SALDO_OTROS ?? 0.0,
                EXCEDENTE = x.EXCEDENTE ?? 0.0,
                CAPITAL_VIGENTE = x.CAPITAL_VIGENTE ?? 0.0,
                CAPITAL_MORA = x.CAPITAL_MORA ?? 0.0,
                CAPITAL_VENCIDO = x.CAPITAL_VENCIDO ?? 0.0,
                INTERES_VIGENTE = x.INTERES_VIGENTE ?? 0.0,
                INTERES_MORA = x.INTERES_MORA ?? 0.0,
                INTERES_VENCIDO = x.INTERES_VENCIDO ?? 0.0,
                FECHA_CANCELACION = x.FECHA_CANCELACION ?? DateTime.MinValue,
                FECHA_APERTURA = x.FECHA_APERTURA ?? DateTime.MinValue,
                FECHA_VENCIMIENTO = x.FECHA_VENCIMIENTO ?? DateTime.MinValue,
                VALOR_CUOTA = x.VALOR_CUOTA ?? 0.0,
                MONTO_OTORGADO = x.MONTO_OTORGADO ?? 0.0,
                VALOR_VALUO = x.VALOR_VALUO ?? 0.0m,
                VALOR_CONTABLE = x.VALOR_CONTABLE ?? 0.0m,
                FECHA_SOLICITUD = x.FECHA_SOLICITUD ?? DateTime.MinValue,
                NOMBRE_COMPLETO = string.IsNullOrEmpty(x.NOMBRE_COMPLETO) ? "DESCONOCIDO" : x.NOMBRE_COMPLETO,
                TOTAL_INGRESOS = x.TOTAL_INGRESOS ?? 0.0m,
                CODIGO_AGENCIA = string.IsNullOrEmpty(x.CODIGO_AGENCIA) ? "DESCONOCIDO" : x.CODIGO_AGENCIA
            }).ToList();

            DataTable dtReporte = DataTableHelper.ToDataTable(datosLimpios);

            VerReporte(nombreReporte, formato, parametros, dtReporte,
                            nombreTabla);

            return RedirectToAction("Index");
        }
    }
}