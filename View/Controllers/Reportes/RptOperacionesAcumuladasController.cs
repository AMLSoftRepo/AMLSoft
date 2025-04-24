using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blo.Reportes;
using Microsoft.Reporting.WebForms;
using System.Globalization;
using Model;
using System.Data;

namespace View.Controllers.Reportes
{
    [NoCache]
    [Autorizacion]
    public class RptOperacionesAcumuladasController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reporteBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptOperacionesAcumuladasController(IReportesBlo reporteBlo)
        {
            _reporteBlo = reporteBlo;
        }


        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetOperaciones(string searchString = null, int txtMes = 0, int txtAnio = 0)
        {
            try
            {
                DateTime fechaOperacionIni = DateTime.Parse("01/" + txtMes + "/" + txtAnio);
                DateTime fechaOperacionFin = fechaOperacionIni.AddMonths(1).AddDays(-1);


                var records = _SQLBDEntities.SP_RPT_OPERACIONES_ACUMULADAS(fechaOperacionIni,fechaOperacionFin,searchString).ToList();


                return Json(new { records }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpGet]
        public ActionResult Reporte(long codigoCliente, int totalPagos, decimal monto,int mes,string anio, string formato)
        {
            DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
            string nombreMes = formatoFecha.GetMonthName(mes);

            Dictionary<string, object> parametersData = new Dictionary<string, object>
            {
                { "MES", nombreMes },
                { "ANIO", anio }
            };

            try
            {
                string nombreReporte = "RptOperacionesAcumuladasIgualesSuperioresA10000";
                string nombreTabla = "VIEW_CLIENTE";
                var datosReporte = _reporteBlo.ReporteOperacionesAcumuladasIgualesSuperioresA10000(codigoCliente, totalPagos, monto);

                if (datosReporte == null || !datosReporte.Any())
                {
                    return Content("No hay datos disponibles para el reporte");
                }

                var datosLimpios = datosReporte.Select(x => new VIEW_CLIENTE
                {
                    CODIGO_CLIENTE = x.CODIGO_CLIENTE,
                    TIPO_IDENTIFICACION_1 = string.IsNullOrEmpty(x.TIPO_IDENTIFICACION_1) ? "SIN DOCUMENTO" : x.TIPO_IDENTIFICACION_1,
                    NUMERO_IDENTIFICACION_1 = string.IsNullOrEmpty(x.NUMERO_IDENTIFICACION_1) ? "SIN DOCUMENTO" : x.NUMERO_IDENTIFICACION_1,
                    TIPO_IDENTIFICACION_2 = string.IsNullOrEmpty(x.TIPO_IDENTIFICACION_2) ? "SIN DOCUMENTO" : x.TIPO_IDENTIFICACION_2,
                    NUMERO_IDENTIFICACION_2 = string.IsNullOrEmpty(x.NUMERO_IDENTIFICACION_2) ? "SIN DOCUMENTO" : x.NUMERO_IDENTIFICACION_2,
                    CODIGO_TIPO_CLIENTE = x.CODIGO_TIPO_CLIENTE,
                    TIPO_CLIENTE = string.IsNullOrEmpty(x.TIPO_CLIENTE) ? "SIN DOCUMENTO" : x.TIPO_CLIENTE,
                    CODIGO_PAIS = x.CODIGO_PAIS ?? 0.0m,
                    PAIS = string.IsNullOrEmpty(x.PAIS) ? "SIN DOCUMENTO" : x.PAIS,
                    NACIONALIDAD = string.IsNullOrEmpty(x.NACIONALIDAD) ? "SIN DOCUMENTO" : x.NACIONALIDAD,
                    CODIGO_SECTOR_ECONOMICO = x.CODIGO_SECTOR_ECONOMICO ?? 0.0m,
                    SECTOR_ECONOMICO = string.IsNullOrEmpty(x.SECTOR_ECONOMICO) ? "SIN DOCUMENTO" : x.SECTOR_ECONOMICO,
                    CODIGO_ACTIVIDAD_ECONOMICA = x.CODIGO_ACTIVIDAD_ECONOMICA,
                    ACTIVIDAD_ECONOMICA = string.IsNullOrEmpty(x.ACTIVIDAD_ECONOMICA) ? "SIN DOCUMENTO" : x.ACTIVIDAD_ECONOMICA,
                    CODIGO_CLASE_ACTIVIDAD_ECONOMICA = string.IsNullOrEmpty(x.CODIGO_CLASE_ACTIVIDAD_ECONOMICA) ? "SIN DOCUMENTO" : x.CODIGO_CLASE_ACTIVIDAD_ECONOMICA,
                    SUB_ACTIVIDAD_ECONOMICA = string.IsNullOrEmpty(x.SUB_ACTIVIDAD_ECONOMICA) ? "SIN DOCUMENTO" : x.SUB_ACTIVIDAD_ECONOMICA,
                    TIPO_DE_PERSONA = string.IsNullOrEmpty(x.TIPO_DE_PERSONA) ? "SIN DOCUMENTO" : x.TIPO_DE_PERSONA,
                    NOMBRES = string.IsNullOrEmpty(x.NOMBRES) ? "SIN DOCUMENTO" : x.NOMBRES,
                    PRIMER_APELLIDO = string.IsNullOrEmpty(x.PRIMER_APELLIDO) ? "SIN DOCUMENTO" : x.PRIMER_APELLIDO,
                    SEGUNDO_APELLIDO = string.IsNullOrEmpty(x.SEGUNDO_APELLIDO) ? "SIN DOCUMENTO" : x.SEGUNDO_APELLIDO,
                    APELLIDO_DE_CASADA = string.IsNullOrEmpty(x.APELLIDO_DE_CASADA) ? "SIN DOCUMENTO" : x.APELLIDO_DE_CASADA,
                    SEXO = string.IsNullOrEmpty(x.SEXO) ? "SIN DOCUMENTO" : x.SEXO,
                    FECHA_DE_NACIMIENTO = x.FECHA_DE_NACIMIENTO ?? DateTime.MinValue,
                    CODIGO_ESTADO_CIVIL = string.IsNullOrEmpty(x.CODIGO_ESTADO_CIVIL) ? "SIN DOCUMENTO" : x.CODIGO_ESTADO_CIVIL,
                    ESTADO_CIVIL = string.IsNullOrEmpty(x.ESTADO_CIVIL) ? "SIN DOCUMENTO" : x.ESTADO_CIVIL,
                    TOTAL_INGRESOS = x.TOTAL_INGRESOS ?? 0.0m,
                    CODIGO_PROFESION = x.CODIGO_PROFESION ?? 0.0m,
                    PROFESION = string.IsNullOrEmpty(x.PROFESION) ? "SIN DOCUMENTO" : x.PROFESION,
                    CODIGO_OCUPACION = x.CODIGO_OCUPACION ?? 0.0m,
                    OCUPACION = string.IsNullOrEmpty(x.OCUPACION) ? "SIN DOCUMENTO" : x.OCUPACION,
                    CODIGO_EMPRESA = x.CODIGO_EMPRESA,
                    CONOCIDO_COMO = string.IsNullOrEmpty(x.CONOCIDO_COMO) ? "SIN DOCUMENTO" : x.CONOCIDO_COMO,
                    ALIAS = string.IsNullOrEmpty(x.ALIAS) ? "SIN DOCUMENTO" : x.ALIAS,
                    DOMICILIO = string.IsNullOrEmpty(x.DOMICILIO) ? "SIN DOCUMENTO" : x.DOMICILIO,
                    TOTAL_TRANSACCIONES = x.TOTAL_TRANSACCIONES,
                    TOTAL_MONTO = x.TOTAL_MONTO
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