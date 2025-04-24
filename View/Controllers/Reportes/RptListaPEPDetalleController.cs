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
    public class RptListaPEPDetalleController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reportesBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptListaPEPDetalleController(IReportesBlo reportesBlo)
        {
            _reportesBlo = reportesBlo;
        }

        // GET: RptListaPEP
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Reporte(int idPEP, string formato)
        {
            
            Dictionary<string, object> parametersData = new Dictionary<string, object>
            {
                { "idPEP", idPEP }
            };

            try
            {
                //Crear parametros para la generación de la matriz
                string nombreReporte = "RptListaPEPDetalle";
                string nombreTabla = "PEPDetalleDatos";

                var pepDetalle = _reportesBlo.ReporteListaPEPDetalle(idPEP);
                var pepDetalleCargo = _reportesBlo.ReporteListaPEPDetalleCargo(idPEP);
                var pepDetalleRelacion = _reportesBlo.ReporteListaPEPDetalleRelacion(idPEP);

                if ((pepDetalle == null || !pepDetalle.Any()) && (pepDetalleCargo == null || !pepDetalleCargo.Any()) && (pepDetalleRelacion == null || !pepDetalleRelacion.Any()))
                {
                    return Content("No hay datos disponibles para el reporte.");
                }

                DataTable dtReporte = new DataTable(nombreTabla);

                dtReporte.Columns.Add("PRIMER_NOMBRE", typeof(string));
                dtReporte.Columns.Add("SEGUNDO_NOMBRE", typeof(string));
                dtReporte.Columns.Add("PRIMER_APELLIDO", typeof(string));
                dtReporte.Columns.Add("SEGUNDO_APELLIDO", typeof(string));
                dtReporte.Columns.Add("APELLIDO_CASADA", typeof(string));
                dtReporte.Columns.Add("CONOCIDO_POR", typeof(string));
                dtReporte.Columns.Add("DUI", typeof(string));
                dtReporte.Columns.Add("NIT", typeof(string));
                dtReporte.Columns.Add("PASAPORTE", typeof(string));
                dtReporte.Columns.Add("CARNET_RESIDENTE", typeof(string));
                dtReporte.Columns.Add("FUNCIONARIO_O_RELACION", typeof(string));
                dtReporte.Columns.Add("DECLARACION_JURADA", typeof(bool));
                dtReporte.Columns.Add("TITULO_CARGO_PEP", typeof(bool));
                dtReporte.Columns.Add("ABREVIATURA_TITULO", typeof(string));
                dtReporte.Columns.Add("TITULO", typeof(string));

                dtReporte.Columns.Add("ID_LIS_PEP", typeof(int));
                dtReporte.Columns.Add("ID_ORGANO", typeof(int));
                dtReporte.Columns.Add("ID_ENTIDAD", typeof(int));
                dtReporte.Columns.Add("NOMBRE_CARGO", typeof(string));
                dtReporte.Columns.Add("FECHA_INICIO", typeof(DateTime));
                dtReporte.Columns.Add("FECHA_FIN", typeof(DateTime));

                dtReporte.Columns.Add("NOMBRE_COMPLETO", typeof(string));
                dtReporte.Columns.Add("GRADO_PARENTESCO", typeof(string));
                dtReporte.Columns.Add("DESCRIPCION_RELACION", typeof(string));

                foreach (var pepdetalle in pepDetalle)
                {
                    DataRow row = dtReporte.NewRow();
                    row["PRIMER_NOMBRE"] = pepdetalle.PRIMER_NOMBRE ?? "N/A";
                    row["SEGUNDO_NOMBRE"] = pepdetalle.SEGUNDO_NOMBRE ?? "N/A";
                    row["PRIMER_APELLIDO"] = pepdetalle.PRIMER_APELLIDO ?? "N/A";
                    row["SEGUNDO_APELLIDO"] = pepdetalle.SEGUNDO_APELLIDO ?? "N/A";
                    row["APELLIDO_CASADA"] = pepdetalle.APELLIDO_CASADA ?? "N/A";
                    row["CONOCIDO_POR"] = pepdetalle.CONOCIDO_POR ?? "N/A";
                    row["DUI"] = pepdetalle.DUI ?? "N/A";
                    row["NIT"] = pepdetalle.NIT ?? "N/A";
                    row["PASAPORTE"] = pepdetalle.PASAPORTE ?? "N/A";
                    row["CARNET_RESIDENTE"] = pepdetalle.CARNET_RESIDENTE ?? "N/A";
                    row["FUNCIONARIO_O_RELACION"] = pepdetalle.FUNCIONARIO_O_RELACION ?? "N/A";
                    row["DECLARACION_JURADA"] = pepdetalle.DECLARACION_JURADA ?? false;
                    row["TITULO_CARGO_PEP"] = pepdetalle.TITULO_CARGO_PEP ?? 0;
                    row["ABREVIATURA_TITULO"] = pepdetalle.ABREVIATURA_TITULO ?? "N/A";
                    row["TITULO"] = pepdetalle.TITULO ?? "N/A";

                    dtReporte.Rows.Add(row);
                }

                foreach (var pepdetallecargo in pepDetalleCargo)
                {
                    DataRow row = dtReporte.NewRow();
                    row["ID_LIS_PEP"] = pepdetallecargo.ID_LIS_PEP;
                    row["ID_ORGANO"] = pepdetallecargo.ID_ORGANO;
                    row["ID_ENTIDAD"] = pepdetallecargo.ID_ENTIDAD;
                    row["NOMBRE_CARGO"] = pepdetallecargo.NOMBRE_CARGO ?? "N/A";
                    row["FECHA_INICIO"] = pepdetallecargo.FECHA_INICIO;
                    row["FECHA_FIN"] = pepdetallecargo.FECHA_FIN ?? DateTime.MinValue;

                    dtReporte.Rows.Add(row);
                }

                foreach (var pepdetallerelacion in pepDetalleRelacion)
                {
                    DataRow row = dtReporte.NewRow();
                    row["NOMBRE_COMPLETO"] = pepdetallerelacion.NOMBRE_COMPLETO;
                    row["GRADO_PARENTESCO"] = pepdetallerelacion.GRADO_PARENTESCO;
                    row["DESCRIPCION_RELACION"] = pepdetallerelacion.DESCRIPCION_RELACION;

                    dtReporte.Rows.Add(row);
                }

                VerReporte(nombreReporte, formato, parametersData, dtReporte, nombreTabla);

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