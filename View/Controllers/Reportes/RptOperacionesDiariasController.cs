using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blo.Reportes;
using Microsoft.Reporting.WebForms;


namespace View.Controllers.Reportes
{
    [NoCache]
    [Autorizacion]
    public class RptOperacionesDiariasController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reporteBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptOperacionesDiariasController(IReportesBlo reporteBlo)
        {
            _reporteBlo = reporteBlo;
        }

        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public JsonResult GetOperaciones(string fechaInicial = null, string fechaFinal = null, string searchString = null)
        {
            try
            {
                DateTime fechaIni = fechaInicial == null? DateTime.Now: DateTime.Parse(fechaInicial);
                DateTime fechaFin = fechaFinal == null? DateTime.Now.AddHours(23): DateTime.Parse(fechaFinal).AddHours(23);

                var records = _SQLBDEntities.VIEW_TRANSACCIONES.AsNoTracking()
                           .Where(x => x.VALOR_TRANSACCION >= 10000 &&
                                  x.FECHA_CALENDARIO.Value >= fechaIni &&
                                  x.FECHA_CALENDARIO.Value <= fechaFin &&
                                  (x.CODIGO_CLIENTE.ToString() + " " + x.NUMERO_PRODUCTO).Contains(searchString.Trim()))
                           .Select(x => new
                            {
                                x.VALOR_TRANSACCION,
                                x.NUMERO_PRODUCTO,
                                x.CODIGO_CLIENTE,
                                x.NOMBRE_COMPLETO,
                                x.SECUENCIA,
                                x.NUMERO_RECIBO
                            })
                           .Select(x => x)
                           .AsQueryable();


                return Json(new { records }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpGet]
        public ActionResult Reporte(decimal codigoCliente, string secuencia, string formato)
        {
            try
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>
                {
                    { "CodigoCliente", codigoCliente },
                    { "Secuencia", secuencia }
                };

                string nombreReporte = "RptOperacionesDiariasIgualesSuperioresA10000";
                string nombreTabla = "OperacionesDiariasDatos";

                var datosCliente = _reporteBlo.ReporteClienteOperacionesDiariasIgualesSuperioresA10000(codigoCliente);
                var datosTransaccion = _reporteBlo.ReporteTransaccionOperacionesDiariasIgualesSuperioresA10000(secuencia);
                var datosInfoAdicional = _reporteBlo.ReporteInfoAdicionalOperacionesDiariasIgualesSuperioresA10000(secuencia);

                if ((datosCliente == null || !datosCliente.Any()) && (datosTransaccion == null || !datosTransaccion.Any()) && (datosInfoAdicional == null || !datosInfoAdicional.Any()))
                {
                    return Content("No hay datos disponibles para el reporte.");
                }

                DataTable dtReporte = new DataTable(nombreTabla);

                dtReporte.Columns.Add("CODIGO_CLIENTE", typeof(decimal));
                dtReporte.Columns.Add("TIPO_IDENTIFICACION_1", typeof(string));
                dtReporte.Columns.Add("NUMERO_IDENTIFICACION_1", typeof(string));
                dtReporte.Columns.Add("TIPO_IDENTIFICACION_2", typeof(string));
                dtReporte.Columns.Add("NUMERO_IDENTIFICACION_2", typeof(string));
                dtReporte.Columns.Add("CODIGO_TIPO_CLIENTE", typeof(decimal));
                dtReporte.Columns.Add("TIPO_CLIENTE", typeof(string));
                dtReporte.Columns.Add("CODIGO_PAIS", typeof(decimal));
                dtReporte.Columns.Add("PAIS", typeof(string));
                dtReporte.Columns.Add("NACIONALIDAD", typeof(string));
                dtReporte.Columns.Add("CODIGO_SECTOR_ECONOMICO", typeof(decimal));
                dtReporte.Columns.Add("SECTOR_ECONOMICO", typeof(string));
                dtReporte.Columns.Add("CODIGO_ACTIVIDAD_ECONOMICA", typeof(decimal));
                dtReporte.Columns.Add("ACTIVIDAD_ECONOMICA", typeof(string));
                dtReporte.Columns.Add("CODIGO_CLASE_ACTIVIDAD_ECONOMICA", typeof(string));
                dtReporte.Columns.Add("CLASE_ACTIVIDAD_ECONOMICA", typeof(string));
                dtReporte.Columns.Add("CODIGO_SUB_ACTIVIDAD_ECONOMICA", typeof(string));
                dtReporte.Columns.Add("SUB_ACTIVIDAD_ECONOMICA", typeof(string));
                dtReporte.Columns.Add("TIPO_DE_PERSONA", typeof(string));
                dtReporte.Columns.Add("NOMBRES", typeof(string));
                dtReporte.Columns.Add("PRIMER_APELLIDO", typeof(string));
                dtReporte.Columns.Add("SEGUNDO_APELLIDO", typeof(string));
                dtReporte.Columns.Add("APELLIDO_DE_CASADA", typeof(string));
                dtReporte.Columns.Add("SEXO", typeof(string));
                dtReporte.Columns.Add("FECHA_DE_NACIMIENTO", typeof(DateTime));
                dtReporte.Columns.Add("CODIGO_ESTADO_CIVIL", typeof(string));
                dtReporte.Columns.Add("ESTADO_CIVIL", typeof(string));
                dtReporte.Columns.Add("TOTAL_INGRESOS", typeof(string));
                dtReporte.Columns.Add("CODIGO_PROFESION", typeof(decimal));
                dtReporte.Columns.Add("PROFESION", typeof(string));
                dtReporte.Columns.Add("CODIGO_OCUPACION", typeof(decimal));
                dtReporte.Columns.Add("OCUPACION", typeof(string));
                dtReporte.Columns.Add("CODIGO_EMPRESA", typeof(decimal));
                dtReporte.Columns.Add("CONOCIDO_COMO", typeof(string));
                dtReporte.Columns.Add("ALIAS", typeof(string));
                dtReporte.Columns.Add("DOMICILIO", typeof(string));

                dtReporte.Columns.Add("FECHA_CALENDARIO", typeof(DateTime));
                dtReporte.Columns.Add("NUMERO_PRODUCTO", typeof(string));
                dtReporte.Columns.Add("TIPO_TRANSACCION", typeof(string));
                dtReporte.Columns.Add("VALOR_TRANSACCION", typeof(double));
                dtReporte.Columns.Add("CLASE_PRODUCTO", typeof(string));
                dtReporte.Columns.Add("NUMERO_RECIBO", typeof(string));
                dtReporte.Columns.Add("CODIGO_FINANCIERA", typeof(string));

                dtReporte.Columns.Add("SECUENCIA", typeof(string));
                dtReporte.Columns.Add("PERT_NOMBRE", typeof(string));
                dtReporte.Columns.Add("PERT_PRIMER_APELLIDO", typeof(string));
                dtReporte.Columns.Add("PERT_SEGUNDO_APELLIDO", typeof(string));
                dtReporte.Columns.Add("PERT_APELLIDO_CASADA", typeof(string));
                dtReporte.Columns.Add("PERT_DUI", typeof(string));
                dtReporte.Columns.Add("PERT_NIT", typeof(string));
                dtReporte.Columns.Add("PERT_NACIONALIDAD", typeof(string));
                dtReporte.Columns.Add("PERT_DEPARTAMENTO", typeof(int));
                dtReporte.Columns.Add("PERT_MUNICIPIO", typeof(int));
                dtReporte.Columns.Add("PERT_DOMICILIO", typeof(string));
                dtReporte.Columns.Add("PERT_FECHA_NACIMIENTO", typeof(DateTime));
                dtReporte.Columns.Add("PERT_LUGAR_NACIMIENTO", typeof(string));
                dtReporte.Columns.Add("PERT_ESTADO_CIVIL", typeof(string));
                dtReporte.Columns.Add("PERT_PROFESION", typeof(string));
                dtReporte.Columns.Add("TRAN_CODIGO_CAJERO", typeof(string));
                dtReporte.Columns.Add("TRAN_DEPARTAMENTO", typeof(int));
                dtReporte.Columns.Add("TRAN_MUNICIPIO", typeof(int));
                dtReporte.Columns.Add("TRAN_ORIGEN_FONDOS", typeof(string));
                dtReporte.Columns.Add("INFO_DATOS_RECIBO", typeof(bool));
                dtReporte.Columns.Add("INFO_CALIDAD_IMPRESION", typeof(bool));
                dtReporte.Columns.Add("INFO_LLENO_DECLARACION", typeof(bool));
                dtReporte.Columns.Add("INFO_MODALIDAD_PAGO", typeof(string));
                dtReporte.Columns.Add("INFO_VERSION_DECLARACION_JURADA", typeof(string));
                dtReporte.Columns.Add("INFO_QUIEN_LLENO_DECLARACION_JURADA", typeof(string));
                dtReporte.Columns.Add("INFO_DOCUMENTACION_REQUERIDA", typeof(string));
                dtReporte.Columns.Add("INFO_TELEFONO_CONTACTO", typeof(string));
                dtReporte.Columns.Add("INFO_SEGUIMIENTO", typeof(string));
                dtReporte.Columns.Add("CLIE_LUGAR_NACIMIENTO", typeof(string));
                dtReporte.Columns.Add("CLIE_DEPARTAMENTO", typeof(int));
                dtReporte.Columns.Add("CLIE_MUNICIPIO", typeof(int));
                dtReporte.Columns.Add("TRAN_AGENCIA_BANCARIA", typeof(int));
                dtReporte.Columns.Add("INFO_CONFIRMACION_UIF", typeof(string));
                dtReporte.Columns.Add("INFO_FECHA_CONFIRMACION_UIF", typeof(DateTime));
                dtReporte.Columns.Add("INFO_COLECTOR", typeof(bool));
                
                foreach (var datoscliente in datosCliente)
                {
                    DataRow row = dtReporte.NewRow();
                    row["CODIGO_CLIENTE"] = datoscliente.CODIGO_CLIENTE;
                    row["TIPO_IDENTIFICACION_1"] = datoscliente.TIPO_IDENTIFICACION_1 ?? "Sin Documento";
                    row["NUMERO_IDENTIFICACION_1"] = datoscliente.NUMERO_IDENTIFICACION_1 ?? "Sin Documento";
                    row["TIPO_IDENTIFICACION_2"] = datoscliente.TIPO_IDENTIFICACION_2 ?? "N/A";
                    row["NUMERO_IDENTIFICACION_2"] = datoscliente.NUMERO_IDENTIFICACION_2 ?? "N/A";
                    row["CODIGO_TIPO_CLIENTE"] = datoscliente.CODIGO_TIPO_CLIENTE;
                    row["TIPO_CLIENTE"] = datoscliente.TIPO_CLIENTE ?? "N/A";
                    row["CODIGO_PAIS"] = datoscliente.CODIGO_PAIS ?? (object)DBNull.Value;
                    row["PAIS"] = datoscliente.PAIS ?? "N/A";
                    row["NACIONALIDAD"] = datoscliente.NACIONALIDAD ?? "N/A";
                    row["CODIGO_SECTOR_ECONOMICO"] = datoscliente.CODIGO_SECTOR_ECONOMICO ?? (object)DBNull.Value;
                    row["SECTOR_ECONOMICO"] = datoscliente.SECTOR_ECONOMICO ?? "N/A";
                    row["CODIGO_ACTIVIDAD_ECONOMICA"] = datoscliente.CODIGO_ACTIVIDAD_ECONOMICA;
                    row["ACTIVIDAD_ECONOMICA"] = datoscliente.ACTIVIDAD_ECONOMICA ?? "N/A";
                    row["CODIGO_CLASE_ACTIVIDAD_ECONOMICA"] = datoscliente.CODIGO_CLASE_ACTIVIDAD_ECONOMICA ?? "N/A";
                    row["CLASE_ACTIVIDAD_ECONOMICA"] = datoscliente.CLASE_ACTIVIDAD_ECONOMICA ?? "N/A";
                    row["CODIGO_SUB_ACTIVIDAD_ECONOMICA"] = datoscliente.CODIGO_SUB_ACTIVIDAD_ECONOMICA ?? "N/A";
                    row["SUB_ACTIVIDAD_ECONOMICA"] = datoscliente.SUB_ACTIVIDAD_ECONOMICA ?? "N/A";
                    row["TIPO_DE_PERSONA"] = datoscliente.TIPO_DE_PERSONA ?? "N/A";
                    row["NOMBRES"] = datoscliente.NOMBRES ?? "N/A";
                    row["PRIMER_APELLIDO"] = datoscliente.PRIMER_APELLIDO ?? "N/A";
                    row["SEGUNDO_APELLIDO"] = datoscliente.SEGUNDO_APELLIDO ?? "N/A";
                    row["APELLIDO_DE_CASADA"] = datoscliente.APELLIDO_DE_CASADA ?? "N/A";
                    row["SEXO"] = datoscliente.SEXO ?? "N/A";
                    row["FECHA_DE_NACIMIENTO"] = datoscliente.FECHA_DE_NACIMIENTO ?? (object)DBNull.Value;
                    row["CODIGO_ESTADO_CIVIL"] = datoscliente.CODIGO_ESTADO_CIVIL ?? "N/A";
                    row["ESTADO_CIVIL"] = datoscliente.ESTADO_CIVIL ?? "N/A";
                    row["TOTAL_INGRESOS"] = datoscliente.TOTAL_INGRESOS ?? 0.0m;
                    row["CODIGO_PROFESION"] = datoscliente.CODIGO_PROFESION ?? (object)DBNull.Value;
                    row["PROFESION"] = datoscliente.PROFESION ?? "N/A";
                    row["CODIGO_OCUPACION"] = datoscliente.CODIGO_OCUPACION ?? (object)DBNull.Value;
                    row["OCUPACION"] = datoscliente.OCUPACION ?? "N/A";
                    row["CODIGO_EMPRESA"] = datoscliente.CODIGO_EMPRESA;
                    row["CONOCIDO_COMO"] = datoscliente.CONOCIDO_COMO ?? "N/A";
                    row["ALIAS"] = datoscliente.ALIAS ?? "N/A";
                    row["DOMICILIO"] = datoscliente.DOMICILIO ?? "N/A";

                    dtReporte.Rows.Add(row);

                }

                foreach (var datostransaccion in datosTransaccion)
                {
                    DataRow row = dtReporte.NewRow();

                    row["FECHA_CALENDARIO"] = datostransaccion.FECHA_CALENDARIO ?? (object)DBNull.Value;
                    row["NUMERO_PRODUCTO"] = datostransaccion.NUMERO_PRODUCTO ?? "N/A";
                    row["TIPO_TRANSACCION"] = datostransaccion.TIPO_TRANSACCION ?? "N/A";
                    row["VALOR_TRANSACCION"] = datostransaccion.VALOR_TRANSACCION ?? 0.0;
                    row["CLASE_PRODUCTO"] = datostransaccion.CLASE_PRODUCTO ?? "N/A";
                    row["NUMERO_RECIBO"] = datostransaccion.NUMERO_RECIBO ?? "N/A";
                    row["CODIGO_FINANCIERA"] = datostransaccion.CODIGO_FINANCIERA ?? "N/A";

                    dtReporte.Rows.Add(row);

                }

                foreach (var datosinfoAdicional in datosInfoAdicional)
                {
                    DataRow row = dtReporte.NewRow();

                    row["SECUENCIA"] = datosinfoAdicional.SECUENCIA ?? "N/A";
                    row["PERT_NOMBRE"] = datosinfoAdicional.PERT_NOMBRE ?? "N/A";
                    row["PERT_PRIMER_APELLIDO"] = datosinfoAdicional.PERT_PRIMER_APELLIDO ?? "N/A";
                    row["PERT_SEGUNDO_APELLIDO"] = datosinfoAdicional.PERT_SEGUNDO_APELLIDO ?? "N/A";
                    row["PERT_APELLIDO_CASADA"] = datosinfoAdicional.PERT_APELLIDO_CASADA ?? "N/A";
                    row["PERT_DUI"] = datosinfoAdicional.PERT_DUI ?? "N/A";
                    row["PERT_NIT"] = datosinfoAdicional.PERT_NIT ?? "N/A";
                    row["PERT_NACIONALIDAD"] = datosinfoAdicional.PERT_NACIONALIDAD ?? "N/A";
                    row["PERT_DEPARTAMENTO"] = datosinfoAdicional.PERT_DEPARTAMENTO ?? (object)DBNull.Value;
                    row["PERT_MUNICIPIO"] = datosinfoAdicional.PERT_MUNICIPIO ?? (object)DBNull.Value;
                    row["PERT_DOMICILIO"] = datosinfoAdicional.PERT_DOMICILIO ?? "N/A";
                    row["PERT_FECHA_NACIMIENTO"] = datosinfoAdicional.PERT_FECHA_NACIMIENTO ?? (object)DBNull.Value;
                    row["PERT_LUGAR_NACIMIENTO"] = datosinfoAdicional.PERT_LUGAR_NACIMIENTO ?? "N/A";
                    row["PERT_ESTADO_CIVIL"] = datosinfoAdicional.PERT_ESTADO_CIVIL ?? "N/A";
                    row["PERT_PROFESION"] = datosinfoAdicional.PERT_PROFESION ?? "N/A";
                    row["TRAN_CODIGO_CAJERO"] = datosinfoAdicional.TRAN_CODIGO_CAJERO ?? "N/A";
                    row["TRAN_DEPARTAMENTO"] = datosinfoAdicional.TRAN_DEPARTAMENTO ?? (object)DBNull.Value;
                    row["TRAN_MUNICIPIO"] = datosinfoAdicional.TRAN_MUNICIPIO ?? (object)DBNull.Value;
                    row["TRAN_ORIGEN_FONDOS"] = datosinfoAdicional.TRAN_ORIGEN_FONDOS ?? "N/A";
                    row["INFO_DATOS_RECIBO"] = datosinfoAdicional.INFO_DATOS_RECIBO ?? false;
                    row["INFO_CALIDAD_IMPRESION"] = datosinfoAdicional.INFO_CALIDAD_IMPRESION ?? false;
                    row["INFO_LLENO_DECLARACION"] = datosinfoAdicional.INFO_LLENO_DECLARACION ?? false;
                    row["INFO_MODALIDAD_PAGO"] = datosinfoAdicional.INFO_MODALIDAD_PAGO ?? "N/A";
                    row["INFO_VERSION_DECLARACION_JURADA"] = datosinfoAdicional.INFO_VERSION_DECLARACION_JURADA ?? "N/A";
                    row["INFO_QUIEN_LLENO_DECLARACION_JURADA"] = datosinfoAdicional.INFO_QUIEN_LLENO_DECLARACION_JURADA ?? "N/A";
                    row["INFO_DOCUMENTACION_REQUERIDA"] = datosinfoAdicional.INFO_DOCUMENTACION_REQUERIDA ?? "N/A";
                    row["INFO_TELEFONO_CONTACTO"] = datosinfoAdicional.INFO_TELEFONO_CONTACTO ?? "N/A";
                    row["INFO_SEGUIMIENTO"] = datosinfoAdicional.INFO_SEGUIMIENTO ?? "N/A";
                    row["CLIE_LUGAR_NACIMIENTO"] = datosinfoAdicional.CLIE_LUGAR_NACIMIENTO ?? "N/A";
                    row["CLIE_DEPARTAMENTO"] = datosinfoAdicional.CLIE_DEPARTAMENTO ?? (object)DBNull.Value;
                    row["CLIE_MUNICIPIO"] = datosinfoAdicional.CLIE_MUNICIPIO ?? (object)DBNull.Value;
                    row["TRAN_AGENCIA_BANCARIA"] = datosinfoAdicional.TRAN_AGENCIA_BANCARIA ?? (object)DBNull.Value;
                    row["INFO_CONFIRMACION_UIF"] = datosinfoAdicional.INFO_CONFIRMACION_UIF ?? "N/A";
                    row["INFO_FECHA_CONFIRMACION_UIF"] = datosinfoAdicional.INFO_FECHA_CONFIRMACION_UIF ?? (object)DBNull.Value;
                    row["INFO_COLECTOR"] = datosinfoAdicional.INFO_COLECTOR ?? false;

                    dtReporte.Rows.Add(row);
                    
                }

                VerReporte(nombreReporte, formato, parametros, dtReporte, nombreTabla);

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