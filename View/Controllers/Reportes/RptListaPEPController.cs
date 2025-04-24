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
    public class RptListaPEPController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reportesBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptListaPEPController(IReportesBlo reportesBlo)
        {
            _reportesBlo = reportesBlo;
        }

        // GET: RptListaPEP
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Reporte(string formato)
        {
            Dictionary<string, object> parametersData = new Dictionary<string, object>();

            string nombreReporte = "RptListaPEP";
            string nombreTabla = "LIS_PEP";

            var datosReporte = _reportesBlo.ReporteListaPEP();

            if (datosReporte == null || !datosReporte.Any())
            {
                return Content("No hay datos disponibles para el reporte.");
            }

            var datosLimpios = datosReporte.Select(x => new LIS_PEP
            {
                PRIMER_NOMBRE = string.IsNullOrEmpty(x.PRIMER_NOMBRE) ? "DESCONOCIDO" : x.PRIMER_NOMBRE,
                SEGUNDO_NOMBRE = string.IsNullOrEmpty(x.SEGUNDO_NOMBRE) ? "DESCONOCIDO" : x.SEGUNDO_NOMBRE,
                PRIMER_APELLIDO = string.IsNullOrEmpty(x.PRIMER_APELLIDO) ? "DESCONOCIDO" : x.PRIMER_APELLIDO,
                SEGUNDO_APELLIDO = string.IsNullOrEmpty(x.SEGUNDO_APELLIDO) ? "DESCONOCIDO" : x.SEGUNDO_APELLIDO,
                APELLIDO_CASADA = string.IsNullOrEmpty(x.APELLIDO_CASADA) ? "NO CONTIENE" : x.APELLIDO_CASADA,
                CONOCIDO_POR = string.IsNullOrEmpty(x.CONOCIDO_POR) ? "DESCONOCIDO" : x.CONOCIDO_POR,
                DUI = string.IsNullOrEmpty(x.DUI) ? "SIN DOCUMENTO" : x.DUI,
                NIT = string.IsNullOrEmpty(x.NIT) ? "SIN DOCUMENTO" : x.NIT,
                PASAPORTE = string.IsNullOrEmpty(x.PASAPORTE) ? "SIN DOCUMENTO" : x.PASAPORTE,
                CARNET_RESIDENTE = string.IsNullOrEmpty(x.CARNET_RESIDENTE) ? "SIN DOCUMENTO" : x.CARNET_RESIDENTE,
                FUNCIONARIO_O_RELACION = string.IsNullOrEmpty(x.FUNCIONARIO_O_RELACION) ? "DESCONOCIDO" : x.FUNCIONARIO_O_RELACION,
                DECLARACION_JURADA = x.DECLARACION_JURADA ?? false,
                TITULO_CARGO_PEP = x.TITULO_CARGO_PEP ?? 0
            }).ToList();

            DataTable dtReporte = DataTableHelper.ToDataTable(datosLimpios);

            VerReporte(nombreReporte, formato, parametersData, dtReporte, nombreTabla);
            return RedirectToAction("Index");
        }
    }
}