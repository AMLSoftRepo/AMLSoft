using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blo.Reportes;
using Microsoft.Reporting.WebForms;
using Model;
using Blo.Seguridad;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;

namespace View.Controllers.Reportes
{
    [NoCache]
    [Autorizacion]
    public class RptOficioDetalleController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IEmpresaBlo _empresaBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptOficioDetalleController(IEmpresaBlo empresaBlo)
        {
            _empresaBlo = empresaBlo;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult ReporteOficioDetalle(string fechaInicial, string fechaFinal, string formato)
        {
            DateTime fechaIni = DateTime.Parse(fechaInicial);
            DateTime fechaFin = DateTime.Parse(fechaFinal);

            Dictionary<string, object> parametersData = new Dictionary<string, object>
            {
                { "FechaInicial", fechaIni.ToString() },
                { "FechaFinal", fechaFinal.ToString() }
            };

            try
            {
                var datosReporte = _SQLBDEntities.MON_OFICIO.ToList();
                if (datosReporte == null || !datosReporte.Any())
                {
                    return Content("No hay datos disponibles para el reporte.");
                }

                ReportDocument reportDocument = new ReportDocument();
                string rutaReporte = Server.MapPath("~/Reportes_crytal/RptOficioDetalle.rpt");
                reportDocument.Load(rutaReporte);

                reportDocument.SetDataSource(datosReporte);

                reportDocument.SetParameterValue("FechaInicial", fechaIni);
                reportDocument.SetParameterValue("FechaFinal", fechaFin);

                var empresa = _empresaBlo.GetEmpresaByCodigo(CODIGOEMPRESALOCAL).FirstOrDefault();
                if (empresa != null)
                {
                    empresa.LOGO = Server.MapPath("~/" + empresa.LOGO);
                    List<SEG_EMPRESA> datosEmpresa = new List<SEG_EMPRESA> { empresa };
                    reportDocument.Subreports["Empresa"].SetDataSource(datosEmpresa);
                }

                // Exportar el reporte al formato solicitado
                Stream reporteStream;
                switch (formato.ToUpper())
                {
                    case "PDF":
                        reporteStream = reportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
                        return File(reporteStream, "application/pdf", "ReporteOficioDetalle.pdf");
                    case "WORD":
                        reporteStream = reportDocument.ExportToStream(ExportFormatType.WordForWindows);
                        return File(reporteStream, "application/msword", "ReporteOficioDetalle.doc");
                    case "EXCEL":
                        reporteStream = reportDocument.ExportToStream(ExportFormatType.Excel);
                        return File(reporteStream, "application/vnd.ms-excel", "ReporteOficioDetalle.xls");
                    default:
                        return Content("Formato no soportado.");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new HttpException(404, null);
            }
        }

        /// <summary>
        /// Permite Agregar un sub reporte que muestra del detalle de personas por oficio
        /// </summary>
        void SubRptOficiosDetalle_SubPersonas(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("Personas", _SQLBDEntities.MON_OFICIO_PERSONA.ToList()));
        }

    }
}