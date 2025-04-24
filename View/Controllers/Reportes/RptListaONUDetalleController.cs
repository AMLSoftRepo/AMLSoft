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
    public class RptListaONUDetalleController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reportesBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptListaONUDetalleController(IReportesBlo reportesBlo)
        {
            _reportesBlo = reportesBlo;
        }

        // GET: RptListaONUDetalle
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Reporte(int idPersona, string formato)
        {
            Dictionary<string, object> parametersData = new Dictionary<string, object>();

            string nombreReporte = "RptListaONUDetalle";
            string nombreTabla = "RptListaONUDatos";

            // Cargar datos filtrados por persona
            var detalles = _reportesBlo.ReporteListaONUDetalle(idPersona);
            var documentos = _reportesBlo.ReporteListaONUDetalleDocumento(idPersona);
            var direcciones = _reportesBlo.ReporteListaONUDetalleDireccion(idPersona);
            var alias = _reportesBlo.ReporteListaONUDetalleAlias(idPersona);

            if ((detalles == null || !detalles.Any()) &&
                (documentos == null || !documentos.Any()) &&
                (direcciones == null || !direcciones.Any()) &&
                (alias == null || !alias.Any()))
            {
                return Content("No hay datos disponibles para el reporte de la persona seleccionada.");
            }

            DataTable dtReporte = new DataTable(nombreTabla);

            // Definir columnas
            dtReporte.Columns.Add("DATA_ID", typeof(int));
            dtReporte.Columns.Add("TIPO", typeof(string));
            dtReporte.Columns.Add("VERSION_NUM", typeof(int));
            dtReporte.Columns.Add("FIRST_NAME", typeof(string));
            dtReporte.Columns.Add("SECOND_NAME", typeof(string));
            dtReporte.Columns.Add("THIRD_NAME", typeof(string));
            dtReporte.Columns.Add("FOURTH_NAME", typeof(string));
            dtReporte.Columns.Add("UN_LIST_TYPE", typeof(string));
            dtReporte.Columns.Add("REFERENCE_NUMBER", typeof(string));
            dtReporte.Columns.Add("LISTED_ON", typeof(DateTime));
            dtReporte.Columns.Add("COMMENTS", typeof(string));
            dtReporte.Columns.Add("NATIONALITY", typeof(string));

            dtReporte.Columns.Add("ID_LIS_ONU", typeof(int));
            dtReporte.Columns.Add("TYPE", typeof(string));
            dtReporte.Columns.Add("NUMBER", typeof(string));
            dtReporte.Columns.Add("NOTE", typeof(string));

            dtReporte.Columns.Add("STREET", typeof(string));
            dtReporte.Columns.Add("CITY", typeof(string));
            dtReporte.Columns.Add("STATE_PROVINCE", typeof(string));
            dtReporte.Columns.Add("COUNTRY", typeof(string));

            dtReporte.Columns.Add("QUALITY", typeof(string));
            dtReporte.Columns.Add("ALIAS_NAME", typeof(string));
            dtReporte.Columns.Add("CITY_OF_BIRTH", typeof(string));
            dtReporte.Columns.Add("COUNTRY_OF_BIRTH", typeof(string));

            // Insertar detalles
            foreach (var detalle in detalles)
            {
                DataRow row = dtReporte.NewRow();
                row["DATA_ID"] = detalle.DATA_ID;
                row["TIPO"] = detalle.TIPO ?? "N/A";
                row["VERSION_NUM"] = detalle.VERSION_NUM ?? 0;
                row["FIRST_NAME"] = detalle.FIRST_NAME ?? "N/A";
                row["SECOND_NAME"] = detalle.SECOND_NAME ?? "N/A";
                row["THIRD_NAME"] = detalle.THIRD_NAME ?? "N/A";
                row["FOURTH_NAME"] = detalle.FOURTH_NAME ?? "N/A";
                row["UN_LIST_TYPE"] = detalle.UN_LIST_TYPE ?? "N/A";
                row["REFERENCE_NUMBER"] = detalle.REFERENCE_NUMBER ?? "N/A";
                row["LISTED_ON"] = detalle.LISTED_ON ?? DateTime.MinValue;
                row["COMMENTS"] = detalle.COMMENTS ?? "N/A";
                row["NATIONALITY"] = detalle.NATIONALITY ?? "N/A";

                dtReporte.Rows.Add(row);
            }

            // Insertar documentos
            foreach (var documento in documentos)
            {
                DataRow row = dtReporte.NewRow();
                row["ID_LIS_ONU"] = documento.ID_LIS_ONU;
                row["TYPE"] = documento.NUMBER ?? "N/A";
                row["NUMBER"] = documento.ID_LIS_ONU;
                row["NOTE"] = documento.NUMBER ?? "N/A";

                dtReporte.Rows.Add(row);
            }

            // Insertar direcciones
            foreach (var direccion in direcciones)
            {
                DataRow row = dtReporte.NewRow();
                row["STREET"] = direccion.STREET ?? "N/A";
                row["CITY"] = direccion.CITY ?? "N/A";
                row["STATE_PROVINCE"] = direccion.STATE_PROVINCE ?? "N/A";
                row["COUNTRY"] = direccion.COUNTRY ?? "N/A";

                dtReporte.Rows.Add(row);
            }

            // Insertar alias
            foreach (var alia in alias)
            {
                DataRow row = dtReporte.NewRow();
                row["QUALITY"] = alia.QUALITY ?? "N/A";
                row["ALIAS_NAME"] = alia.ALIAS_NAME ?? "N/A";
                row["CITY_OF_BIRTH"] = alia.CITY_OF_BIRTH ?? "N/A";
                row["COUNTRY_OF_BIRTH"] = alia.COUNTRY_OF_BIRTH ?? "N/A";

                dtReporte.Rows.Add(row);
            }

            VerReporte(nombreReporte, formato, parametersData, dtReporte, nombreTabla);

            return RedirectToAction("Index");
        }
    }
}