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
    [Autorizacion]
    public class RptPrimasController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reportesBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptPrimasController(IReportesBlo reportesBlo)
        {
            _reportesBlo = reportesBlo;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult PrimasSuperioresA7500(string fechaInicial, string fechaFinal, string formato)
        {
            DateTime fechaIni = DateTime.Parse(fechaInicial);
            DateTime fechaFin = DateTime.Parse(fechaFinal).AddHours(23);

            Dictionary<string, object> parametersData = new Dictionary<string, object>
            {
                {"FechaInicial", fechaIni },
                {"FechaFinal", fechaFin }
            };

            string nombreReporte = "RptPrimasSuperioresA7500";
            string nombreTabla = "VIEW_REPORTE_PRIMAS";
            var datosReporte = _reportesBlo.ReportePrimasSuperioresA7500(fechaIni, fechaFin).ToList();

            if (datosReporte == null || !datosReporte.Any())
            {
                return Content("No hay datos disponibles para el reporte.");
            }

            var datosLimpios = datosReporte.Select(x => new VIEW_REPORTE_PRIMAS
            {
                TIPO_OPERACION = x.TIPO_OPERACION ?? 0.0m,
                TOTAL_INGRESOS = x.TOTAL_INGRESOS ?? 0.0m

            }).ToList();

            DataTable dtReporte = DataTableHelper.ToDataTable(datosLimpios);

            VerReporte(nombreReporte, formato, parametersData, dtReporte,
                            nombreTabla);

            return RedirectToAction("Index");
           
        }


        [HttpGet]
        public ActionResult PrimasIgualesSuperioresA10000(string fechaInicial, string fechaFinal, string formato)
        {
            DateTime fechaIni = DateTime.Parse(fechaInicial);
            DateTime fechaFin = DateTime.Parse(fechaFinal).AddHours(23);

            Dictionary<string, object> parametersData = new Dictionary<string, object>
            {
                {"FechaInicial", fechaIni },
                {"FechaFinal", fechaFin }
            };

            string nombreReporte = "RptPrimasIgualesSuperioresA10000";
            string nombreTabla = "VIEW_REPORTE_PRIMAS";
            var datosReporte = _reportesBlo.ReportePrimasYComplementosIgualesSuperioresA10000(fechaIni, fechaFin).ToList();

            if (datosReporte == null || !datosReporte.Any())
            {
                return Content("No hay datos disponibles para el reporte.");
            }

            var datosLimpios = datosReporte.Select(x => new VIEW_REPORTE_PRIMAS
            {
                TIPO_OPERACION = x.TIPO_OPERACION ?? 0.0m,
                TOTAL_INGRESOS = x.TOTAL_INGRESOS ?? 0.0m, 
                CODIGO_FINANCIERA = string.IsNullOrEmpty(x.CODIGO_FINANCIERA) ? "DESCONOCIDO" : x.CODIGO_FINANCIERA
            }).ToList();

            DataTable dtReporte = DataTableHelper.ToDataTable(datosLimpios);   

            VerReporte(nombreReporte, formato, parametersData, dtReporte,
                           nombreTabla);

            return RedirectToAction("Index");
            
        }
        

    }
}