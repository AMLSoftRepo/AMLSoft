using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blo.Reportes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Model;
using View.Reportes_crytal;

namespace View.Controllers.Reportes
{
    [NoCache]
    [Autorizacion]
    public class RptPagosController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reportesBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptPagosController(IReportesBlo reportesBlo)
        {
            _reportesBlo = reportesBlo;
        }


        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public ActionResult PagosDiariosIgualesSuperiores571_5000(string fechaInicial, string fechaFinal, string formato)
        {
            DateTime fechaIni = DateTime.Parse(fechaInicial);
            DateTime fechaFin = DateTime.Parse(fechaFinal).AddHours(23);

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                {"FechaInicial", fechaIni },
                {"FechaFinal", fechaFin }
            };

            string nombreReporte = "RptPagosDiariosIgualesSuperiores571_5000";
            string nombreTabla = "VIEW_REPORTE_CONTROLPAGOS";
            var datosReporte = _reportesBlo.ReportePagosDiariosIgualesSuperiores571_5000(fechaIni, fechaFin).ToList();

            if (datosReporte == null || !datosReporte.Any())
            {
                return Content("No hay datos disponibles para el reporte.");
            }

            var datosLimpios = datosReporte.Select(x => new VIEW_REPORTE_CONTROLPAGOS
            {
                FECHA_APLICACION = x.FECHA_APLICACION ?? DateTime.MinValue,
                VALOR_TRANSACCION = x.VALOR_TRANSACCION ?? 0.0,
                FECHA_CALENDARIO = x.FECHA_CALENDARIO ?? DateTime.MinValue,
                FINANCIERA = string.IsNullOrEmpty(x.FINANCIERA) ? "DESCONOCIDO" : x.FINANCIERA,
                NUMERO_RECIBO = string.IsNullOrEmpty(x.NUMERO_RECIBO) ? "SIN RECIBO" : x.NUMERO_RECIBO,
                INFO_DATOS_RECIBO = x.INFO_DATOS_RECIBO ?? false,
                TRAN_AGENCIA_BANCARIA = string.IsNullOrEmpty(x.TRAN_AGENCIA_BANCARIA) ? "DESCONOCIDO" : x.TRAN_AGENCIA_BANCARIA,
                TRAN_CODIGO_CAJERO = string.IsNullOrEmpty(x.TRAN_CODIGO_CAJERO) ? "DESCONOCIDO" : x.TRAN_CODIGO_CAJERO,
                INFO_CALIDAD_IMPRESION = x.INFO_CALIDAD_IMPRESION ?? false,
                INFO_LLENO_DECLARACION = x.INFO_LLENO_DECLARACION  ?? false,
                INFO_COLECTOR = x.INFO_COLECTOR ?? false,
                INFO_VERSION_DECLARACION_JURADA = string.IsNullOrEmpty(x.INFO_VERSION_DECLARACION_JURADA) ? "DESCONOCIDO" : x.INFO_VERSION_DECLARACION_JURADA,
                CODIGO_CLIENTE = string.IsNullOrEmpty(x.CODIGO_CLIENTE) ? "DESCONOCIDO" : x.CODIGO_CLIENTE,
                NUMERO_PRODUCTO = string.IsNullOrEmpty(x.NUMERO_PRODUCTO) ? "DESCONOCIDO" : x.NUMERO_PRODUCTO,
                SECUENCIA = string.IsNullOrEmpty(x.SECUENCIA) ? "DESCONOCIDO" : x.SECUENCIA,
                ESTADO = string.IsNullOrEmpty(x.ESTADO) ? "DESCONOCIDO" : x.ESTADO,
                PERT_NOMBRE = string.IsNullOrEmpty(x.PERT_NOMBRE) ? "DESCONOCIDO" : x.PERT_NOMBRE,
                PERT_PRIMER_APELLIDO = string.IsNullOrEmpty(x.PERT_PRIMER_APELLIDO) ? "DESCONOCIDO" : x.PERT_PRIMER_APELLIDO,
                PERT_SEGUNDO_APELLIDO = string.IsNullOrEmpty(x.PERT_SEGUNDO_APELLIDO) ? "DESCONOCIDO" : x.PERT_SEGUNDO_APELLIDO,
                PERT_APELLIDO_CASADA = string.IsNullOrEmpty(x.PERT_APELLIDO_CASADA) ? "DESCONOCIDO" : x.PERT_APELLIDO_CASADA,
                PERT_DUI = string.IsNullOrEmpty(x.PERT_DUI) ? "DESCONOCIDO" : x.PERT_DUI,
                PERT_NIT = string.IsNullOrEmpty(x.PERT_NIT) ? "DESCONOCIDO" : x.PERT_NIT,
                PERT_NACIONALIDAD = string.IsNullOrEmpty(x.PERT_NACIONALIDAD) ? "DESCONOCIDO" : x.PERT_NACIONALIDAD,
                PERT_DEPARTAMENTO = x.PERT_DEPARTAMENTO ?? 0,
                PERT_MUNICIPIO = x.PERT_MUNICIPIO ?? 0,
                PERT_DOMICILIO = string.IsNullOrEmpty(x.PERT_DOMICILIO) ? "DESCONOCIDO" : x.PERT_DOMICILIO,
                PERT_FECHA_NACIMIENTO = x.PERT_FECHA_NACIMIENTO ?? DateTime.MinValue,
                PERT_LUGAR_NACIMIENTO = string.IsNullOrEmpty(x.PERT_LUGAR_NACIMIENTO) ? "DESCONOCIDO" : x.PERT_LUGAR_NACIMIENTO,
                PERT_ESTADO_CIVIL = string.IsNullOrEmpty(x.PERT_ESTADO_CIVIL) ? "DESCONOCIDO" : x.PERT_ESTADO_CIVIL,
                PERT_PROFESION = string.IsNullOrEmpty(x.PERT_PROFESION) ? "DESCONOCIDO" : x.PERT_PROFESION,
                TRAN_ORIGEN_FONDOS = string.IsNullOrEmpty(x.TRAN_ORIGEN_FONDOS) ? "DESCONOCIDO" : x.TRAN_ORIGEN_FONDOS,
                TRAN_DEPARTAMENTO = x.TRAN_DEPARTAMENTO ?? 0,
                TRAN_MUNICIPIO = x.TRAN_MUNICIPIO ?? 0,
                INFO_MODALIDAD_PAGO = string.IsNullOrEmpty(x.INFO_MODALIDAD_PAGO) ? "DESCONOCIDO" : x.INFO_MODALIDAD_PAGO,
                INFO_DOCUMENTACION_REQUERIDA = string.IsNullOrEmpty(x.INFO_DOCUMENTACION_REQUERIDA) ? "DESCONOCIDO" : x.INFO_DOCUMENTACION_REQUERIDA,
                INFO_TELEFONO_CONTACTO = string.IsNullOrEmpty(x.INFO_TELEFONO_CONTACTO) ? "DESCONOCIDO" : x.INFO_TELEFONO_CONTACTO,
                INFO_SEGUIMIENTO = string.IsNullOrEmpty(x.INFO_SEGUIMIENTO) ? "DESCONOCIDO" : x.INFO_SEGUIMIENTO,
                CLIE_LUGAR_NACIMIENTO = string.IsNullOrEmpty(x.CLIE_LUGAR_NACIMIENTO) ? "DESCONOCIDO" : x.CLIE_LUGAR_NACIMIENTO,
                CLIE_DEPARTAMENTO = x.CLIE_DEPARTAMENTO ?? 0,
                CLIE_MUNICIPIO = x.CLIE_MUNICIPIO ?? 0,
                INFO_CONFIRMACION_UIF = string.IsNullOrEmpty(x.INFO_CONFIRMACION_UIF) ? "DESCONOCIDO" : x.INFO_CONFIRMACION_UIF,
                INFO_FECHA_CONFIRMACION_UIF = x.INFO_FECHA_CONFIRMACION_UIF ?? DateTime.MinValue,
                INFO_QUIEN_LLENO_DECLARACION_JURADA = string.IsNullOrEmpty(x.INFO_QUIEN_LLENO_DECLARACION_JURADA) ? "DESCONOCIDO" : x.INFO_QUIEN_LLENO_DECLARACION_JURADA,
            }).ToList();

            DataTable dtReporte = DataTableHelper.ToDataTable(datosLimpios);

            VerReporte(nombreReporte, formato, parametros, dtReporte,
                            nombreTabla);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult PagosDiariosIgualesSuperioresA5000(string fechaInicial, string fechaFinal, string formato)
        {
            DateTime fechaIni = DateTime.Parse(fechaInicial);
            DateTime fechaFin = DateTime.Parse(fechaFinal).AddHours(23);

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                {"FechaInicial", fechaIni },
                {"FechaFinal", fechaFin }
            };

            string nombreReporte = "RptPagosDiariosIgualesSuperioresA5000";
            string nombreTabla = "VIEW_REPORTE_CONTROLPAGOS";
            var datosReporte = _reportesBlo.ReportePagosDiariosIgualesSuperioresA5000(fechaIni, fechaFin).ToList();

            if (datosReporte == null || !datosReporte.Any())
            {
                return Content("No hay datos disponibles para el reporte.");
            }

            var datosLimpios = datosReporte.Select(x => new VIEW_REPORTE_CONTROLPAGOS
            {
                FECHA_APLICACION = x.FECHA_APLICACION ?? DateTime.MinValue,
                VALOR_TRANSACCION = x.VALOR_TRANSACCION ?? 0.0,
                FECHA_CALENDARIO = x.FECHA_CALENDARIO ?? DateTime.MinValue,
                FINANCIERA = string.IsNullOrEmpty(x.FINANCIERA) ? "DESCONOCIDO" : x.FINANCIERA,
                NUMERO_RECIBO = string.IsNullOrEmpty(x.NUMERO_RECIBO) ? "SIN RECIBO" : x.NUMERO_RECIBO,
                INFO_DATOS_RECIBO = x.INFO_DATOS_RECIBO ?? false,
                TRAN_AGENCIA_BANCARIA = string.IsNullOrEmpty(x.TRAN_AGENCIA_BANCARIA) ? "DESCONOCIDO" : x.TRAN_AGENCIA_BANCARIA,
                TRAN_CODIGO_CAJERO = string.IsNullOrEmpty(x.TRAN_CODIGO_CAJERO) ? "DESCONOCIDO" : x.TRAN_CODIGO_CAJERO,
                INFO_CALIDAD_IMPRESION = x.INFO_CALIDAD_IMPRESION ?? false,
                INFO_LLENO_DECLARACION = x.INFO_LLENO_DECLARACION ?? false,
                INFO_COLECTOR = x.INFO_COLECTOR ?? false,
                INFO_VERSION_DECLARACION_JURADA = string.IsNullOrEmpty(x.INFO_VERSION_DECLARACION_JURADA) ? "DESCONOCIDO" : x.INFO_VERSION_DECLARACION_JURADA,
                CODIGO_CLIENTE = string.IsNullOrEmpty(x.CODIGO_CLIENTE) ? "DESCONOCIDO" : x.CODIGO_CLIENTE,
                NUMERO_PRODUCTO = string.IsNullOrEmpty(x.NUMERO_PRODUCTO) ? "DESCONOCIDO" : x.NUMERO_PRODUCTO,
                SECUENCIA = string.IsNullOrEmpty(x.SECUENCIA) ? "DESCONOCIDO" : x.SECUENCIA,
                ESTADO = string.IsNullOrEmpty(x.ESTADO) ? "DESCONOCIDO" : x.ESTADO,
                PERT_NOMBRE = string.IsNullOrEmpty(x.PERT_NOMBRE) ? "DESCONOCIDO" : x.PERT_NOMBRE,
                PERT_PRIMER_APELLIDO = string.IsNullOrEmpty(x.PERT_PRIMER_APELLIDO) ? "DESCONOCIDO" : x.PERT_PRIMER_APELLIDO,
                PERT_SEGUNDO_APELLIDO = string.IsNullOrEmpty(x.PERT_SEGUNDO_APELLIDO) ? "DESCONOCIDO" : x.PERT_SEGUNDO_APELLIDO,
                PERT_APELLIDO_CASADA = string.IsNullOrEmpty(x.PERT_APELLIDO_CASADA) ? "DESCONOCIDO" : x.PERT_APELLIDO_CASADA,
                PERT_DUI = string.IsNullOrEmpty(x.PERT_DUI) ? "DESCONOCIDO" : x.PERT_DUI,
                PERT_NIT = string.IsNullOrEmpty(x.PERT_NIT) ? "DESCONOCIDO" : x.PERT_NIT,
                PERT_NACIONALIDAD = string.IsNullOrEmpty(x.PERT_NACIONALIDAD) ? "DESCONOCIDO" : x.PERT_NACIONALIDAD,
                PERT_DEPARTAMENTO = x.PERT_DEPARTAMENTO ?? 0,
                PERT_MUNICIPIO = x.PERT_MUNICIPIO ?? 0,
                PERT_DOMICILIO = string.IsNullOrEmpty(x.PERT_DOMICILIO) ? "DESCONOCIDO" : x.PERT_DOMICILIO,
                PERT_FECHA_NACIMIENTO = x.PERT_FECHA_NACIMIENTO ?? DateTime.MinValue,
                PERT_LUGAR_NACIMIENTO = string.IsNullOrEmpty(x.PERT_LUGAR_NACIMIENTO) ? "DESCONOCIDO" : x.PERT_LUGAR_NACIMIENTO,
                PERT_ESTADO_CIVIL = string.IsNullOrEmpty(x.PERT_ESTADO_CIVIL) ? "DESCONOCIDO" : x.PERT_ESTADO_CIVIL,
                PERT_PROFESION = string.IsNullOrEmpty(x.PERT_PROFESION) ? "DESCONOCIDO" : x.PERT_PROFESION,
                TRAN_ORIGEN_FONDOS = string.IsNullOrEmpty(x.TRAN_ORIGEN_FONDOS) ? "DESCONOCIDO" : x.TRAN_ORIGEN_FONDOS,
                TRAN_DEPARTAMENTO = x.TRAN_DEPARTAMENTO ?? 0,
                TRAN_MUNICIPIO = x.TRAN_MUNICIPIO ?? 0,
                INFO_MODALIDAD_PAGO = string.IsNullOrEmpty(x.INFO_MODALIDAD_PAGO) ? "DESCONOCIDO" : x.INFO_MODALIDAD_PAGO,
                INFO_DOCUMENTACION_REQUERIDA = string.IsNullOrEmpty(x.INFO_DOCUMENTACION_REQUERIDA) ? "DESCONOCIDO" : x.INFO_DOCUMENTACION_REQUERIDA,
                INFO_TELEFONO_CONTACTO = string.IsNullOrEmpty(x.INFO_TELEFONO_CONTACTO) ? "DESCONOCIDO" : x.INFO_TELEFONO_CONTACTO,
                INFO_SEGUIMIENTO = string.IsNullOrEmpty(x.INFO_SEGUIMIENTO) ? "DESCONOCIDO" : x.INFO_SEGUIMIENTO,
                CLIE_LUGAR_NACIMIENTO = string.IsNullOrEmpty(x.CLIE_LUGAR_NACIMIENTO) ? "DESCONOCIDO" : x.CLIE_LUGAR_NACIMIENTO,
                CLIE_DEPARTAMENTO = x.CLIE_DEPARTAMENTO ?? 0,
                CLIE_MUNICIPIO = x.CLIE_MUNICIPIO ?? 0,
                INFO_CONFIRMACION_UIF = string.IsNullOrEmpty(x.INFO_CONFIRMACION_UIF) ? "DESCONOCIDO" : x.INFO_CONFIRMACION_UIF,
                INFO_FECHA_CONFIRMACION_UIF = x.INFO_FECHA_CONFIRMACION_UIF ?? DateTime.MinValue,
                INFO_QUIEN_LLENO_DECLARACION_JURADA = string.IsNullOrEmpty(x.INFO_QUIEN_LLENO_DECLARACION_JURADA) ? "DESCONOCIDO" : x.INFO_QUIEN_LLENO_DECLARACION_JURADA,
            }).ToList();

            DataTable dtReporte = DataTableHelper.ToDataTable(datosLimpios);

            VerReporte(nombreReporte, formato, parametros, dtReporte,
                            nombreTabla);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult PagosDiariosIgualesSuperioresA10000(string fechaInicial, string fechaFinal, string formato)
        {
            DateTime fechaIni = DateTime.Parse(fechaInicial);
            DateTime fechaFin = DateTime.Parse(fechaFinal).AddHours(23);

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "FechaInicial", fechaIni },
                { "FechaFinal", fechaFin }
            };

            string nombreReporte = "RptPagosDiariosIgualesSuperioresA10000";
            string nombreTabla = "VIEW_TRANSACCIONES";
            var datosReporte = _reportesBlo.ReportePagosDiariosIgualesSuperioresA10000(fechaIni, fechaFin).ToList();

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


        [HttpGet]
        public ActionResult PagosAcumuladosIgualesSuperioresA10000(string fechaInicial, string fechaFinal, string formato)
        {
            DateTime fechaIni = DateTime.Parse(fechaInicial);
            DateTime fechaFin = DateTime.Parse(fechaFinal).AddHours(23);

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "FechaInicial", fechaIni },
                { "FechaFinal", fechaFin }
            };

            string nombreReporte = "RptPagosAcumuladosIgualesSuperioresA10000";
            string nombreTabla = "VIEW_TRANSACCIONES";
            var datosReporte = _reportesBlo.ReportePagosAcumuladosIgualesSuperioresA10000(fechaIni, fechaFin).ToList();

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

        [HttpGet]
        public ActionResult PagosDiariosIgualesSuperioresA25000(string fechaInicial, string fechaFinal, string formato)
        {
            DateTime fechaIni = DateTime.Parse(fechaInicial);
            DateTime fechaFin = DateTime.Parse(fechaFinal).AddHours(23);

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "FechaInicial", fechaIni },
                { "FechaFinal", fechaFin }
            };

            string nombreReporte = "RptPagosDiariosIgualesSuperioresA25000";
            string nombreTabla = "VIEW_TRANSACCIONES";
            var datosReporte = _reportesBlo.ReportePagosDiariosIgualesSuperioresA25000(fechaIni, fechaFin).ToList();

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


        [HttpGet]
        public ActionResult PagosAcumuladosIgualesSuperioresA25000(string fechaInicial, string fechaFinal, string formato)
        {
            DateTime fechaIni = DateTime.Parse(fechaInicial);
            DateTime fechaFin = DateTime.Parse(fechaFinal).AddHours(23);

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "FechaInicial", fechaIni },
                { "FechaFinal", fechaFin }
            };

            string nombreReporte = "RptPagosAcumuladosIgualesSuperioresA25000";
            string nombreTabla = "VIEW_TRANSACCIONES";
            var datosReporte = _reportesBlo.ReportePagosDiariosIgualesSuperioresA25000(fechaIni, fechaFin).ToList();

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