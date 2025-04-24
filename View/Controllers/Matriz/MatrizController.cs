using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Matriz;
using Blo.Reportes;
using Microsoft.Reporting.WebForms;
using System.Data;

namespace View.Controllers.Matriz
{
    [NoCache]
    [Autorizacion]
    public class MatrizController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IMatrizBlo _matrizBlo;
        private readonly IReportesBlo _reporteBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public MatrizController(IMatrizBlo matrizBlo, IReportesBlo reporteBlo)
        {
            _matrizBlo = matrizBlo;
            _reporteBlo = reporteBlo;
        }


        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetMatriz(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _matrizBlo.GetAll(true).Select(x => new
                {
                    x.ID,
                    FECHA = x.FECHA.ToString("dd/MM/yyyy"),
                    x.USUARIO
                }).OrderByDescending(x=>x.ID).AsQueryable();

                //Buscar
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    records = records.Where(x => (
                        x.ID + " " +
                        x.FECHA + " " +
                        x.USUARIO
                        ).ToUpper().Contains(searchString.Trim().ToUpper())
                    ).AsQueryable();
                }

                total = records.Count();
                records = SortHelper.OrdenarGrid(records, sortBy, direction).Skip(start).Take(limit.Value);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpGet]
        public ActionResult Reporte(long id, string formato)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(formato))
            {
                return new HttpStatusCodeResult(400, "Parámetros inválidos");
            }

            List<long> ids = new List<long> { id };

            var matriz = _matrizBlo.GetById(id);
            if (matriz == null)
            {
                return HttpNotFound("No se encontró la matriz con el ID especificado.");
            }

            DateTime fechaGeneracionMatriz = matriz.FECHA;

            Dictionary<string, object> parametersData = new Dictionary<string, object>
            {
                { "FechaInicial", fechaGeneracionMatriz },
                { "FechaFinal", fechaGeneracionMatriz }
            };

            string nombreReporte = "RptMatriz";
            string nombreTabla = "RptMatrizDatos";

            var controles = _reporteBlo.ReporteMatriz_Controles(ids, 0);
            var eventos = _reporteBlo.ReporteMatriz_Eventos(ids, 0);

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

            VerReporte(nombreReporte, formato, parametersData, dtReporte, nombreTabla);

            return RedirectToAction("Index");
        }

    }
}