using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blo.Reportes;
using Blo.Alertas;
using Microsoft.Reporting.WebForms;
using Model;
using System.Data;

namespace View.Controllers.Reportes
{
    [NoCache]
    [Autorizacion]
    public class RptAlertasController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reportesBlo;
        private readonly ITipoAlertaBlo _tipoAlertaBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptAlertasController(IReportesBlo reportesBlo, ITipoAlertaBlo tipoAlertaBlo)
        {
            _reportesBlo = reportesBlo;
            _tipoAlertaBlo = tipoAlertaBlo;
        }

        public ActionResult Index()
        {
            ViewBag.alertas = _tipoAlertaBlo.GetAll();
            return View();
        }


        [HttpGet]
        public ActionResult  Reporte(string fechaInicial, string fechaFinal, int tipoAlerta, string formato)
        {
            DateTime fechaIni = DateTime.Parse(fechaInicial);
            DateTime fechaFin = DateTime.Parse(fechaFinal).AddHours(23);

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                {"FechaInicial", fechaIni },
                {"FechaFinal", fechaFin }
            };

            string nombreReporte = "RptAlertas";
            string nombreTabla = "ALE_ALERTA";
            var datosReporte = _reportesBlo.ReporteAlertas(fechaIni, fechaFin, tipoAlerta).ToList();

            if (datosReporte == null || !datosReporte.Any())
            {
                return Content("No hay datos disponibles para el reporte.");
            }

            var datosLimpios = datosReporte.Select(x => new ALE_ALERTA
            {
                NOMBRE_CLIENTE = string.IsNullOrEmpty(x.NOMBRE_CLIENTE) ? "DESCONOCIDO" : x.NOMBRE_CLIENTE,
                NUMERO_FACTURA = string.IsNullOrEmpty(x.NUMERO_FACTURA) ? "DESCONOCIDO" : x.NUMERO_FACTURA,
                DESCRIPCION = string.IsNullOrEmpty(x.DESCRIPCION) ? "DESCONOCIDO" : x.DESCRIPCION,
                COLOR = string.IsNullOrEmpty(x.COLOR) ? "DESCONOCIDO" : x.COLOR,
                DESCRIPCION_ALERTA = string.IsNullOrEmpty(x.DESCRIPCION_ALERTA) ? "DESCONOCIDO" : x.DESCRIPCION_ALERTA,
                ESTADO_ANTERIOR = string.IsNullOrEmpty(x.ESTADO_ANTERIOR) ? "DESCONOCIDO" : x.ESTADO_ANTERIOR,
                ESTADO_ACTUAL = string.IsNullOrEmpty(x.ESTADO_ACTUAL) ? "DESCONOCIDO" : x.ESTADO_ACTUAL,
                PRESTAMO = string.IsNullOrEmpty(x.PRESTAMO) ? "DESCONOCIDO" : x.PRESTAMO,
                SECUENCIA = string.IsNullOrEmpty(x.SECUENCIA) ? "DESCONOCIDO" : x.SECUENCIA,
                CLASE_PRODUCTO = string.IsNullOrEmpty(x.CLASE_PRODUCTO) ? "DESCONOCIDO" : x.CLASE_PRODUCTO,
                BANCO_TRANSACCION = string.IsNullOrEmpty(x.BANCO_TRANSACCION) ? "DESCONOCIDO" : x.BANCO_TRANSACCION,
                CODIGO_AGENCIA = string.IsNullOrEmpty(x.CODIGO_AGENCIA) ? "DESCONOCIDO" : x.CODIGO_AGENCIA,
                ANALISIS_CASO = string.IsNullOrEmpty(x.ANALISIS_CASO) ? "DESCONOCIDO" : x.ANALISIS_CASO,
                CODIGO_EJECUTIVO = string.IsNullOrEmpty(x.CODIGO_EJECUTIVO) ? "DESCONOCIDO" : x.CODIGO_EJECUTIVO
            }).ToList();

            DataTable dtReporte = DataTableHelper.ToDataTable(datosLimpios);

            VerReporte(nombreReporte, formato, parametros, dtReporte,
                            nombreTabla);

            return RedirectToAction("Index");

        }
        
    }
}