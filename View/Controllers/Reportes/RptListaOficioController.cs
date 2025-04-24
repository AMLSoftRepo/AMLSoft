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
    public class RptListaOficioController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reportesBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptListaOficioController(IReportesBlo reportesBlo)
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
            
            Dictionary<string, object> parametersData = new Dictionary<string, object>
            {
                { "FechaInicial", fechaIni },
                { "FechaFinal", fechaFinal }
            };

            try
            {
                string nombreReporte = "RptListaOficio";
                string nombreTabla = "MON_OFICIO_PERSONA";

                var datosReporte = _reportesBlo.ReporteListaOficio(fechaIni, fechaFin);

                if (datosReporte == null || !datosReporte.Any())
                {
                    return Content("No hay datos disponibles para el reporte.");
                }

                var datosLimpios = datosReporte.Select(x => new MON_OFICIO_PERSONA
                {
                    ID_OFICIO = x.ID_OFICIO,
                    NOMBRE = string.IsNullOrEmpty(x.NOMBRE) ? "DESCONOCIDO" : x.NOMBRE,
                    GENERALES = string.IsNullOrEmpty(x.GENERALES) ? "DESCONOCIDO" : x.GENERALES,
                    DUI = string.IsNullOrEmpty(x.DUI) ? "DESCONOCIDO" : x.DUI,
                    NIT = string.IsNullOrEmpty(x.NIT) ? "DESCONOCIDO" : x.NIT,
                    TIPO_DOCUMENTO = x.TIPO_DOCUMENTO ?? 0L,
                    NUMERO_DOCUMENTO = string.IsNullOrEmpty(x.NUMERO_DOCUMENTO) ? "DESCONOCIDO" : x.NUMERO_DOCUMENTO,
                    TIPO_PERSONA = string.IsNullOrEmpty(x.TIPO_PERSONA) ? "DESCONOCIDO" : x.TIPO_PERSONA,
                    RESULTADO = string.IsNullOrEmpty(x.RESULTADO) ? "DESCONOCIDO" : x.RESULTADO,
                    COTIZACIONES = x.COTIZACIONES,
                    SOLICITUDES = x.SOLICITUDES,
                    PRESTAMOS = x.PRESTAMOS,
                    CHEQUES = x.CHEQUES,
                    AEX = x.AEX,
                    NUMERO_OFICIO = string.IsNullOrEmpty(x.NUMERO_OFICIO) ? "DESCONOCIDO" : x.NUMERO_OFICIO
                }).ToList();

                DataTable dtReporte = DataTableHelper.ToDataTable(datosLimpios);

                VerReporte(nombreReporte, formato, parametersData, dtReporte,
                                nombreTabla);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new HttpException(404, null);
            }
        }
    }
}