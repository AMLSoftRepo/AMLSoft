using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using Blo.Monitoreo;
using Blo.Reportes;
using Model;
using System.Data;

namespace View.Controllers.Reportes
{
    [NoCache]
    [Autorizacion]
    public class RptInformacionOficioController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reportesBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptInformacionOficioController(IReportesBlo reportesBlo)
        {
            _reportesBlo = reportesBlo;
        }


        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Reporte(string fechaInicial, string fechaFinal, string formato)
        {
            DateTime fechaIni = DateTime.Parse(fechaInicial);
            DateTime fechaFin = DateTime.Parse(fechaFinal).AddHours(23);
            
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                {"FechaInicial", fechaIni },
                {"FechaFinal", fechaFin }
            };

            string nombreReporte = "RptInformacionOficio";
            string nombreTabla = "MON_OFICIO_PERSONA";
            var datosReporte = _reportesBlo.ReporteInformacionOficio(fechaIni, fechaFin).ToList();

            if (datosReporte == null || !datosReporte.Any())
            {
                return Content("No hay datos disponibles para el reporte.");
            }

            var datosLimpios = datosReporte.Select(x => new MON_OFICIO_PERSONA
            {
                NOMBRE = string.IsNullOrEmpty(x.NOMBRE) ? "DESCONOCIDO" : x.NOMBRE,
                GENERALES = string.IsNullOrEmpty(x.GENERALES) ? "DESCONOCIDO" : x.GENERALES,
                DUI = string.IsNullOrEmpty(x.DUI) ? "DESCONOCIDO" : x.DUI,
                NIT = string.IsNullOrEmpty(x.NIT) ? "DESCONOCIDO" : x.NIT,
                TIPO_DOCUMENTO = x.TIPO_DOCUMENTO ?? 0L,
                NUMERO_DOCUMENTO = string.IsNullOrEmpty(x.NUMERO_DOCUMENTO) ? "DESCONOCIDO" : x.NUMERO_DOCUMENTO,
                RESULTADO = string.IsNullOrEmpty(x.RESULTADO) ? "DESCONOCIDO" : x.RESULTADO,
                COTIZACIONES = x.COTIZACIONES,
                SOLICITUDES = x.SOLICITUDES,
                PRESTAMOS = x.PRESTAMOS,
                CHEQUES = x.CHEQUES,
                AEX = x.AEX
            }).ToList();

            DataTable dtReporte = DataTableHelper.ToDataTable(datosLimpios);

            VerReporte(nombreReporte, formato, parametros, dtReporte,
                            nombreTabla);

            return RedirectToAction("Index");

        }
        

    }
}