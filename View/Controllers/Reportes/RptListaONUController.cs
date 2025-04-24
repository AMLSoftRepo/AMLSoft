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
    public class RptListaONUController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reportesBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptListaONUController(IReportesBlo reportesBlo)
        {
            _reportesBlo = reportesBlo;
        }

        // GET: RptListaONU
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Reporte(string formato)
        {
            Dictionary<string, object> parametersData = new Dictionary<string, object>();

            string nombreReporte = "RptListaONU";
            string nombreTabla = "LIS_ONU";

            var datosReporte = _reportesBlo.ReporteListaONU();

            if (datosReporte == null || !datosReporte.Any())
            {
                return Content("No hay datos disponibles para el reporte.");
            }

            var datosLimpios = datosReporte.Select(x => new LIS_ONU
            {
                VERSION_NUM = x.VERSION_NUM ?? 0,
                FIRST_NAME = string.IsNullOrEmpty(x.FIRST_NAME) ? "DESCONOCIDO" : x.FIRST_NAME,
                SECOND_NAME = string.IsNullOrEmpty(x.SECOND_NAME) ? "DESCONOCIDO" : x.SECOND_NAME,
                THIRD_NAME = string.IsNullOrEmpty(x.THIRD_NAME) ? "DESCONOCIDO" : x.THIRD_NAME,
                FOURTH_NAME = string.IsNullOrEmpty(x.FOURTH_NAME) ? "DESCONOCIDO" : x.FOURTH_NAME,
                UN_LIST_TYPE = string.IsNullOrEmpty(x.UN_LIST_TYPE) ? "DESCONOCIDO" : x.UN_LIST_TYPE,
                REFERENCE_NUMBER = string.IsNullOrEmpty(x.REFERENCE_NUMBER) ? "DESCONOCIDO" : x.REFERENCE_NUMBER,
                LISTED_ON = x.LISTED_ON ?? DateTime.MinValue,
                COMMENTS = string.IsNullOrEmpty(x.COMMENTS) ? "DESCONOCIDO" : x.COMMENTS,
                NATIONALITY = string.IsNullOrEmpty(x.NATIONALITY) ? "DESCONOCIDO" : x.NATIONALITY
            }).ToList();

            DataTable dtReporte = DataTableHelper.ToDataTable(datosLimpios);

            VerReporte(nombreReporte, formato, parametersData, dtReporte, nombreTabla);

            return RedirectToAction("Index");
        }
    }
}