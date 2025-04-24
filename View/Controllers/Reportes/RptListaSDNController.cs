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
    public class RptListaSDNController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reportesBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptListaSDNController(IReportesBlo reportesBlo)
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

            string nombreReporte = "RptListaSDN";
            string nombreTabla = "LIS_SDN";

            var datosReporte = _reportesBlo.ReporteListaSDN();

            if (datosReporte == null || !datosReporte.Any())
            {
                return Content("No hay datos disponibles para el reporte.");
            }

            var datosLimpios = datosReporte.Select(x => new LIS_SDN
            {
                TIPO = string.IsNullOrEmpty(x.TIPO) ? "DESCONOCIDO" : x.TIPO,
                NOMBRES = string.IsNullOrEmpty(x.NOMBRES) ? "DESCONOCIDO" : x.NOMBRES,
                APELLIDOS = string.IsNullOrEmpty(x.APELLIDOS) ? "DESCONOCIDO" : x.APELLIDOS,
                PROGRAMAS = string.IsNullOrEmpty(x.PROGRAMAS) ? "DESCONOCIDO" : x.PROGRAMAS,
            }).ToList();

            DataTable dtReporte = DataTableHelper.ToDataTable(datosLimpios);

            VerReporte(nombreReporte, formato, parametersData, dtReporte, nombreTabla);

            return RedirectToAction("Index");
        }
    }
}