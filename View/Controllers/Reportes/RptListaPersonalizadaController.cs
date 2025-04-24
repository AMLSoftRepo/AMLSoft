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
    public class RptListaPersonalizadaController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reportesBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptListaPersonalizadaController(IReportesBlo reportesBlo)
        {
            _reportesBlo = reportesBlo;
        }

        // GET: RptListaPEP
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Reporte(int idLista, string formato)
        {
            var nombreLista = _SQLBDEntities.LIS_GENERAL.Where(x => x.ID == idLista).Select(x => x.NOMBRE_LISTA).First();
            Dictionary<string, object> parametersData = new Dictionary<string, object>
            {
                { "idLista", idLista },
                { "nombreLista", nombreLista }
            };

            try
            {
                //Crear parametros para la generación de la matriz
                string nombreReporte = "RptListaPersonalizada";
                string nombreTabla = "LIS_PERSONALIZADA";

                var datosReporte = _reportesBlo.ReporteListaPersonalizada(idLista);

                if (datosReporte == null || !datosReporte.Any())
                {
                    return Content("No hay datos disponibles para el reporte.");
                }

                var datosLimpios = datosReporte.Select(x => new LIS_PERSONALIZADA
                {
                    ID_UNICO = string.IsNullOrEmpty(x.ID_UNICO) ? "DESCONOCIDO" : x.ID_UNICO,
                    PRIMER_NOMBRE = string.IsNullOrEmpty(x.PRIMER_NOMBRE) ? "DESCONOCIDO" : x.PRIMER_NOMBRE,
                    SEGUNDO_NOMBRE = string.IsNullOrEmpty(x.SEGUNDO_NOMBRE) ? "DESCONOCIDO" : x.SEGUNDO_NOMBRE,
                    TERCER_NOMBRE = string.IsNullOrEmpty(x.TERCER_NOMBRE) ? "DESCONOCIDO" : x.TERCER_NOMBRE,
                    PRIMER_APELLIDO = string.IsNullOrEmpty(x.PRIMER_APELLIDO) ? "DESCONOCIDO" : x.PRIMER_APELLIDO,
                    SEGUNDO_APELLIDO = string.IsNullOrEmpty(x.SEGUNDO_APELLIDO) ? "DESCONOCIDO" : x.SEGUNDO_APELLIDO,
                    TERCER_APELLIDO = string.IsNullOrEmpty(x.TERCER_APELLIDO) ? "DESCONOCIDO" : x.TERCER_APELLIDO,
                    PAIS_NACIMIENTO = x.PAIS_NACIMIENTO ?? 0,
                    RAZON = string.IsNullOrEmpty(x.RAZON) ? "DESCONOCIDO" : x.RAZON
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