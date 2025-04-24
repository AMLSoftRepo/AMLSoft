using Model;
using Blo.Seguridad;
using Blo.Alertas;
using Microsoft.Reporting.WebForms;
using Ninject;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Reflection;
using System.IO;
using Blo.Properties;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using View.Reportes_crytal;
using System.Data;

namespace View.Controllers
{
    /// <summary>
    /// Clase compartida para evitar replicar codigo
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Permite el seguimiento de errores
        /// </summary>
        public static readonly log4net.ILog log =
                log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Propiedad que representa el objeto principal de acceso a datos
        /// por medio de esta se prodra acceder a las funciones, procedimientos
        /// almecenados y vistas SQL
        /// </summary>
        public SQLBDEntities _SQLBDEntities = new SQLBDEntities();



        /// <summary>
        /// Propiedad que representa el objeto principal de acceso a logica del 
        /// negocio para la entidad IParametroBlo.
        /// </summary>
        private IParametroBlo _parametrosSegBlo;
        private IEmpresaBlo _empresaBlo;
        private IContactoAlertaBlo _contactoAlertaBlo;


        /// <summary>
        /// Inyeccion de dependencias al objeto de enlace a logica del negocio
        /// para el objeto IParametroBlo
        /// </summary>
        [Inject]
        public IParametroBlo parametrosSegBlo
        {
            get { return _parametrosSegBlo; }
            set { _parametrosSegBlo = value; }
        }

        [Inject]
        public IEmpresaBlo empresaBlo
        {
            get { return _empresaBlo; }
            set { _empresaBlo = value; }
        }

        [Inject]
        public IContactoAlertaBlo contactoAlertaBlo
        {
            get { return _contactoAlertaBlo; }
            set { _contactoAlertaBlo = value; }
        }


        /// <summary>
        /// Valor inicial establecido para los select/dropdowlist 
        /// </summary>
        public string SELECCIONDEFAULT = "";


        /// <summary>
        /// Valor establecido para cargar información de la
        /// empresa actual (Nombre,Logo...)
        /// </summary>
        public string CODIGOEMPRESALOCAL = "LOCAL";


        /// <summary>
        /// valor establecido para cargar los datos de la 
        /// Fiscalia General de la Republica (Nombre,Logo...)
        /// </summary>
        public string CODIGOEMPRESAFGR = "FGR";


        /// <summary>
        /// Metodo que permite obtener un parametro por su codigo
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns> valor del parametro</returns>
        public string ObtenerParametro(string codigo)
        {
            string valor = null;
            try
            {
                valor = _parametrosSegBlo.GetParametroByCodigo(codigo).VALOR;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error intentando obtener un parametro de Base de Datos");
            }
            return valor;
        }


        /*/// <summary>
        /// Permite descargar un reporte en diferentes formatos como PDF, Word y Excel
        /// </summary>
        /// <param name="nombre">Nombre del reporte</param>
        /// <param name="formato">Define si es PDF, Word ó Excel</param>
        /// <param name="codEmpresa">Define para que empresa se genera, ademas de obtener la información de esta</param>
        /// <param name="parametros">Lista de parametros</param>
        /// <param name="query">Consulta</param>
        /// <param name="nombreDataSet">Nombre del Dataset usado en el Reporte</param>
        /// <param name="query2">Consulta 2 es opcional</param>
        /// <param name="nombreDataSet2">Es requerido solo si define Consulta 2</param>
        /// <param name="query3">Consulta 3 es opcional</param>
        /// <param name="nombreDataSet3">Es requerido solo si define Consulta 3</param>
        /// <param name="query4">Consulta 4 es opcional</param>
        /// <param name="nombreDataSet4">Es requerido solo si define Consulta 4</param>
        /// <param name="query5">Consulta 5 es opcional</param>
        /// <param name="nombreDataSet5">Es requerido solo si define Consulta 5</param>
        public void LoadReport(String nombre, string formato, string codEmpresa, List<ReportParameter> parametros,
        IEnumerable<dynamic> query, String nombreDataSet,
        IEnumerable<dynamic> query2 = null, String nombreDataSet2 = null,
        IEnumerable<dynamic> query3 = null, String nombreDataSet3 = null,
        IEnumerable<dynamic> query4 = null, String nombreDataSet4 = null,
        IEnumerable<dynamic> query5 = null, String nombreDataSet5 = null)
        {
            try
            {
                ReportViewer reportViewer = new ReportViewer();

                reportViewer.SizeToReportContent = true;
                reportViewer.ZoomMode = ZoomMode.Percent;
                reportViewer.ZoomPercent = 90;
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.EnableExternalImages = true;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reportes_crytal\" + nombre + ".rpt";
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource(nombreDataSet, query));


                if (query2 != null && nombreDataSet2 != null)
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource(nombreDataSet2, query2));
                if (query3 != null && nombreDataSet3 != null)
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource(nombreDataSet3, query3));
                if (query4 != null && nombreDataSet4 != null)
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource(nombreDataSet4, query4));
                if (query5 != null && nombreDataSet5 != null)
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource(nombreDataSet5, query5));


                if (codEmpresa == CODIGOEMPRESALOCAL || codEmpresa == CODIGOEMPRESAFGR)
                {
                    SEG_EMPRESA empresa = new SEG_EMPRESA();
                    List<SEG_EMPRESA> datosEmpresa = new List<SEG_EMPRESA>();

                    empresa = _empresaBlo.GetEmpresaByCodigo(codEmpresa).FirstOrDefault();
                    empresa.LOGO = @"File:\" + Server.MapPath("/") + empresa.LOGO;
                    datosEmpresa.Add(empresa);

                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Empresa", datosEmpresa));
                }


                //Agregando parametros al reporte
                foreach (var param in parametros) reportViewer.LocalReport.SetParameters(param);


                //Permite la descarga del reporte en diferentes formatos
                switch (formato.ToUpper())
                {
                    case "PDF":
                        ImprimirPDF(reportViewer, nombre);
                        break;
                    case "WORD":
                        ImprimirWord(reportViewer, nombre);
                        break;
                    case "EXCEL":
                        ImprimirExcel(reportViewer, nombre);
                        break;
                }


            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al cargar el Reporte: " + nombre);
            }
        }*/
        /// <summary>
        /// Carga un reporte de Crystal Reports y lo exporta en el formato deseado.
        /// </summary>
        public void VerReporte(string nombreReporte, string formato, Dictionary<string, object> parametros,
            DataTable dtReporte, string nombreTabla)
        {
            ReportDocument report = new ReportDocument();

            try
            {
                //ReportDocument report = new ReportDocument();
                

                string rutaBase = Server.MapPath("/Reportes_crytal");
                string rutaReporte = Path.Combine(rutaBase, nombreReporte + ".rpt");
                Console.WriteLine("Ruta Reporte: " + rutaReporte);

                if (!System.IO.File.Exists(rutaReporte))
                {
                    throw new FileNotFoundException("El archivo del reporte no existe en la ruta: " + rutaReporte);
                }

                report.Load(rutaReporte);

                var coninfo = ReportesConexion.getConexion();
                TableLogOnInfo logoninfo = new TableLogOnInfo();
                Tables tables = report.Database.Tables;

                foreach (Table item in tables)
                {
                    logoninfo = item.LogOnInfo;
                    logoninfo.ConnectionInfo = coninfo;
                    item.ApplyLogOnInfo(logoninfo);
                }

                if (dtReporte == null || dtReporte.Rows.Count == 0)
                {
                    throw new Exception("El DataTable está vacío. No hay datos disponibles para generar el reporte.");
                }

                dtReporte.TableName = nombreTabla;
                report.SetDataSource(dtReporte);

                // Asignar parámetros si existen
                /*if (parametros != null && parametros.Count > 0)
                {
                    foreach (var param in parametros)
                    {
                        report.SetParameterValue(param.Key, param.Value);
                    }
                }*/


                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                switch (formato.ToUpper())
                {
                    case "PDF":
                        ExportarReporte(report, ExportFormatType.PortableDocFormat, nombreReporte);
                        break;

                    case "WORD":
                        ExportarReporte(report, ExportFormatType.WordForWindows, nombreReporte);
                        break;

                    case "EXCEL":
                        ExportarReporte(report, ExportFormatType.Excel, nombreReporte);
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                    throw new ArgumentException("Error al cargar el Reporte: " + nombreReporte);
            }
            finally
            {
                if (report != null)
                {
                    report.Close();
                    report.Dispose();
                }

                // Fuerza la recolección de basura, pero con cuidado:
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }


        /*/// <summary>
        /// Metodo que permite descargar el reporte en formato PDF
        /// </summary>
        /// <param name="reporte">Objeto que contiene la definición del reporte</param>
        /// <param name="nombre">Nombre con el que se descarga el reporte</param>
        public void ImprimirPDF(ReportViewer reporte, string nombre)
        {
            try
            {
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "inline;filename=" + nombre + ".pdf");
                Response.Buffer = true;
                Response.BinaryWrite(reporte.LocalReport.Render("PDF", null));
                Response.End();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al imprimir en PDF el Reporte: " + nombre);
            }
        }*/

        ///<summary>
        ///Método genérico para exportar un reporte en el formato indicado.
        ///</summary>
        private void ExportarReporte(ReportDocument report, ExportFormatType formato, string nombre)
        {
            try
            {
                Stream stream = report.ExportToStream(formato);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();

                Response.Clear();
                Response.ContentType = ObtenerContentType(formato);
                Response.AddHeader("content-disposition", "attachment; filename=" + nombre + ObtenerExtension(formato));
                Response.BinaryWrite(buffer);
                Response.End();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new ArgumentException("Error al exportar el reporte: " + nombre);
            }
     
        }


        /*/// <summary>
        /// Metodo que permite descargar el reporte en formato Word
        /// </summary>
        /// <param name="reporte">Objeto que contiene la definición del reporte</param>
        /// <param name="nombre">Nombre con el que se descarga el reporte</param>
        public void ImprimirWord(ReportViewer reporte, string nombre)
        {
            try
            {
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("content-disposition", "attachment; filename=" + nombre + ".doc");
                Response.Buffer = true;
                Response.BinaryWrite(reporte.LocalReport.Render("WORD", null));
                Response.End();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al imprimir en WORD el Reporte: " + nombre);
            }
        }*/


        /*/// <summary>
        /// Metodo que permite descargar el reporte en formato Excel
        /// </summary>
        /// <param name="reporte">Objeto que contiene la definición del reporte</param>
        /// <param name="nombre">Nombre con el que se descarga el reporte</param>
        public void ImprimirExcel(ReportViewer reporte, string nombre)
        {
            try
            {
                //Response.Clear();
                //Response.ContentType = "application/xls";
                //Response.AddHeader("content-disposition", "attachment; filename=" + nombre + ".xls");
                //Response.Buffer = true;
                //Response.BinaryWrite(reporte.LocalReport.Render("EXCEL", null));
                //Response.End();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + nombre + ".xlsx");
                Response.Buffer = true;
                Response.BinaryWrite(reporte.LocalReport.Render("EXCELOPENXML", null));
                Response.End();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al imprimir en Excel el Reporte: " + nombre);
            }
        }*/

        ///<summary>
        ///Retorna el tipo de contenido según el formato de exportación. 
        ///</summary>
        private string ObtenerContentType(ExportFormatType formato)
        {
            switch (formato)
            {
                case ExportFormatType.PortableDocFormat:
                    return "application/pdf";
                case ExportFormatType.WordForWindows:
                    return "application/msword";
                case ExportFormatType.Excel:
                    return "application/vnd.ms-excel";
                default:
                    return "application/octet-stream";
            }
        }


        ///<summary>
        ///Retorna la extensión de archivo según el formato de exportación
        ///</summary>
        private string ObtenerExtension(ExportFormatType formato)
        {
            switch (formato)
            {
                case ExportFormatType.PortableDocFormat:
                    return ".pdf";
                case ExportFormatType.WordForWindows:
                    return ".doc";
                case ExportFormatType.Excel:
                    return ".xls";
                default:
                    return "";
            }
        }


        /// <summary>
        /// Limpia la cache del navegador 
        /// Evita problemas con el grid usado en esta aplicacion
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public sealed class NoCacheAttribute : ActionFilterAttribute
        {
            public override void OnResultExecuting(ResultExecutingContext filterContext)
            {
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                filterContext.HttpContext.Response.Cache.SetNoStore();

                base.OnResultExecuting(filterContext);
            }
        }

        /// <summary>
        /// Metodo que permite obtener la lista de departamentos por pais
        /// </summary>
        /// <param name="pais">ID del pais</param>
        /// <returns>Lista JSON de los departamentos</returns>
        public JsonResult DepartamentosPorPais(int pais = 0)
        {
            try
            {
                var departamentos = _SQLBDEntities.VIEW_DEPARTAMENTO
                         .Where(x => x.CODIGO_PAIS == pais)
                         .Select(x => new
                         {
                             x.CODIGO_DEPARTAMENTO,
                             x.NOMBRE
                         }).ToList();

                return Json(new { departamentos }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener los datos");
            }
        }

        /// <summary>
        /// Metodo que permite obtener la lista de municipios por departamento
        /// </summary>
        /// <param name="pais">ID del pais</param>
        /// <param name="depto">ID del departamento</param>
        /// <returns>Lista JSON de los municipios</returns>
        public JsonResult MunicipiosPorDepartamento(int pais = 0, int depto = 0)
        {
            try
            {
                var municipios = _SQLBDEntities.VIEW_MUNICIPIO
                         .Where(x => x.CODIGO_DEPARTAMENTO == depto && x.CODIGO_PAIS == pais)
                         .Select(x => new
                         {
                             x.CODIGO_MUNICIPIO,
                             x.NOMBRE
                         }).ToList();

                return Json(new { municipios }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener los datos");
            }
        }

        /// <summary>
        /// Metodo que permite obtener la lista de urbanizaciones por municipio
        /// </summary>
        /// <param name="pais">ID del pais</param>
        /// <param name="depto">ID del departamento</param>
        /// <param name="municipio">ID del municipio</param>
        /// <returns>Lista JSON de las urbanizaciones</returns>
        public JsonResult UrbanizacionesPorMunicipio(int pais = 0, int depto = 0, int municipio = 0)
        {
            try
            {
                var urbanizaciones = _SQLBDEntities.VIEW_URBANIZACION
                         .Where(x => x.CODIGO_MUNICIPIO == municipio && x.CODIGO_DEPARTAMENTO == depto && x.CODIGO_PAIS == pais)
                         .Select(x => new
                         {
                             x.CODIGO_SECTOR,
                             x.DESCRIPCION
                         }).ToList();

                return Json(new { urbanizaciones }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener los datos");
            }
        }


    }
}



