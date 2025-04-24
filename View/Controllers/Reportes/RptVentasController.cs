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
    [NoCache]
    [Authorize]
    public class RptVentasController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reportesBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptVentasController(IReportesBlo reportesBlo)
        {
            _reportesBlo = reportesBlo;
        }


        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult VentasIgualesSuperioresA10000(string fechaInicial, string fechaFinal, string formato)
        {
            DateTime fechaIni = DateTime.Parse(fechaInicial);
            DateTime fechaFin = DateTime.Parse(fechaFinal).AddHours(23);

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "FechaInicial", fechaIni },
                { "FechaFinal", fechaFin }
            };

            string nombreReporte = "RptVentasIgualesSuperioresA10000";
            string nombreTabla = "  VIEW_REPORTE_VENTAS";
            var datosReporte = _reportesBlo.ReporteVentasIgualesSuperioresA10000(fechaIni, fechaFin).ToList();

            if (datosReporte == null || !datosReporte.Any())
            {
                return Content("No hay datos disponibles para el reporte.");
            }

            var datosLimpios = datosReporte.Select(x => new VIEW_REPORTE_VENTAS
            {
                CODIGO_USUARIO = string.IsNullOrEmpty(x.CODIGO_USUARIO) ? "DESCONOCIDO" : x.CODIGO_USUARIO,
                NOMBRE_COMPLETO = string.IsNullOrEmpty(x.NOMBRE_COMPLETO) ? "DESCONOCIDO" : x.NOMBRE_COMPLETO,
                TOTAL_INGRESOS = x.TOTAL_INGRESOS ?? 0.0m,
            }).ToList();

            DataTable dtReporte = DataTableHelper.ToDataTable(datosLimpios);

            VerReporte(nombreReporte, formato, parametros, dtReporte,
                            nombreTabla);

            return RedirectToAction("Index");

        }

    }
}