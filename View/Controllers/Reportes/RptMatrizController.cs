using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using Blo.Matriz;
using Blo.Reportes;
using Model;
using System.Data;

namespace View.Controllers.Reportes
{
    [NoCache]
    [Autorizacion]
    public class RptMatrizController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICatAgenciaBlo _catAgenciaBlo;
        private readonly IReportesBlo _reportesBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptMatrizController(ICatAgenciaBlo catAgenciaBlo,IReportesBlo reporteBlo)
        {
            _catAgenciaBlo = catAgenciaBlo;
            _reportesBlo = reporteBlo;
        }


        public ActionResult Index()
        {
            ViewBag.agencia = _catAgenciaBlo.GetAll();

            return View();
        }


        [HttpGet]
        public ActionResult Reporte(string fechaInicial, string fechaFinal, int idAgencia, string formato)
        {
            DateTime fechaIni = DateTime.Parse(fechaInicial);
            DateTime fechaFin = DateTime.Parse(fechaFinal).AddHours(23);

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "FechaInicial", fechaIni },
                { "FechaFinal", fechaFin }
            };

            string nombreReporte = "RptMatriz";
            string nombreTabla = "RptMatrizDatos";

            List<long> idsMatriz = _reportesBlo.idsMatrizPorRangoFecha(fechaIni, fechaFin);

            var controles = _reportesBlo.ReporteMatriz_Controles(idsMatriz, idAgencia);
            var eventos = _reportesBlo.ReporteMatriz_Eventos(idsMatriz, idAgencia);

            if ((controles == null || !controles.Any()) && (eventos == null || !eventos.Any()))
            {
                return Content("No hay datos disponibles para el reporte.");
            }

            DataTable dtReporte = new DataTable(nombreTabla);

            dtReporte.Columns.Add("ID_CONTROL", typeof(long));
            dtReporte.Columns.Add("ID_AGENCIA", typeof(int));
            dtReporte.Columns.Add("ID_MATRIZ", typeof(long));
            dtReporte.Columns.Add("ID_CONTROL_ANTERIOR", typeof(long));
            dtReporte.Columns.Add("AUTOMATIZACION", typeof(string));
            dtReporte.Columns.Add("DISENO", typeof(string));
            dtReporte.Columns.Add("DOCUMENTACION", typeof(string));
            dtReporte.Columns.Add("FRECUENCIA", typeof(string));
            dtReporte.Columns.Add("MEZCLA", typeof(string));
            dtReporte.Columns.Add("TIPO_CONTROL", typeof(string));
            dtReporte.Columns.Add("DESCRIPCION_CONTROL", typeof(string));
            dtReporte.Columns.Add("TOTAL_POR", typeof(decimal));
            dtReporte.Columns.Add("OBSERVACIONES_CONTROL", typeof(string));
            dtReporte.Columns.Add("ID_EVENTOS", typeof(string));
            dtReporte.Columns.Add("AGENCIA_CONTROL", typeof(string));

            dtReporte.Columns.Add("ID_EVENTO", typeof(long));
            dtReporte.Columns.Add("UNIDAD", typeof(string));
            dtReporte.Columns.Add("FACTOR_RIESGO", typeof(string));
            dtReporte.Columns.Add("RIESGO", typeof(string));
            dtReporte.Columns.Add("CAUSA_RIESGO", typeof(string));
            dtReporte.Columns.Add("COMO", typeof(string));
            dtReporte.Columns.Add("DESCRIPCION_EVENTO", typeof(string));
            dtReporte.Columns.Add("PROBABILIDAD_OCURRENCIA", typeof(string));
            dtReporte.Columns.Add("IMPACTO", typeof(string));
            dtReporte.Columns.Add("RIESGO_INHERENTE", typeof(decimal));
            dtReporte.Columns.Add("COLOR_RIESGO_INHERENTE", typeof(string));
            dtReporte.Columns.Add("EFICACIA_CONTROL", typeof(decimal));
            dtReporte.Columns.Add("COLOR_RIESGO_RESIDUAL", typeof(string));
            dtReporte.Columns.Add("RIESGO_RESIDUAL", typeof(decimal));
            dtReporte.Columns.Add("AGENCIA_EVENTO", typeof(string));
            dtReporte.Columns.Add("TRIMESTRE", typeof(string));
            dtReporte.Columns.Add("ANIO", typeof(int));

            foreach (var control in controles)
            {
                dtReporte.Rows.Add(
                    control.ID,
                    control.ID_AGENCIA,
                    control.ID_MATRIZ,
                    control.ID_CONTROL_ANTERIOR,
                    control.AUTOMATIZACION ?? "DESCONOCIDO",
                    control.DISENO ?? "DESCONOCIDO",
                    control.DOCUMENTACION ?? "DESCONOCIDO",
                    control.FRECUENCIA ?? "DESCONOCIDO",
                    control.MEZCLA ?? "DESCONOCIDO",
                    control.TIPO_CONTROL ?? "DESCONOCIDO",
                    control.DESCRIPCION ?? "DESCONOCIDO",
                    control.TOTAL_POR,
                    control.OBSERVACIONES ?? "DESCONOCIDO",
                    control.ID_EVENTOS ?? "N/A",
                    control.AGENCIA ?? "N/A",

                    null, null, null, null, null, null, null, null, null,
                    null, null, null, null, null, null, null
                );
            }

            foreach (var evento in eventos)
            {
                dtReporte.Rows.Add(
                    null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,

                    evento.ID,
                    evento.UNIDAD ?? "N/A",
                    evento.FACTOR_RIESGO ?? "N/A",
                    evento.RIESGO ?? "N/A",
                    evento.CAUSA_RIESGO ?? "N/A",
                    evento.COMO ?? "N/A",
                    evento.DESCRIPCION ?? "N/A",
                    evento.PROBABILIDAD_OCURRENCIA ?? "N/A",
                    evento.IMPACTO ?? "N/A",
                    evento.RIESGO_INHERENTE,
                    evento.COLOR_RIESGO_INHERENTE ?? "N/A",
                    evento.EFICACIA_CONTROL,
                    evento.COLOR_RIESGO_RESIDUAL ?? "N/A",
                    evento.RIESGO_RESIDUAL,
                    evento.AGENCIA ?? "N/A",
                    evento.TRIMESTRE ?? "N/A",
                    evento.ANIO
                );
            }


            VerReporte(nombreReporte, formato, parametros, dtReporte, nombreTabla);

            return RedirectToAction("Index");
        }
    }
}