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
    public class RptListaSDNDetalleController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reportesBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptListaSDNDetalleController(IReportesBlo reportesBlo)
        {
            _reportesBlo = reportesBlo;
        }

        // GET: RptListaSDNDetalle
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Reporte(int idPersona, string formato)
        {
            Dictionary<string, object> parametersData = new Dictionary<string, object>();

            try
            {
                string nombreReporte = "RptListaSDNDetalle";
                string nombreTabla = "RptSDNDatos";

                var detalles = _reportesBlo.ReporteListaSDNDetalle(idPersona);
                var documentos = _reportesBlo.ReporteListaSDNDetalleDocumento(idPersona);
                var direcciones = _reportesBlo.ReporteListaSDNDetalleDireccion(idPersona);
                var aliasL = _reportesBlo.ReporteListaSDNDetalleAlias(idPersona);
                var nacionalidades = _reportesBlo.ReporteListaSDNDetalleNacionalidad(idPersona);

                if ((detalles == null || !detalles.Any()) && (documentos == null || documentos.Any()) && (direcciones == null || !direcciones.Any()) && (aliasL == null || !aliasL.Any()) && (nacionalidades == null || !nacionalidades.Any()))
                {
                    return Content("No hay datos disponibles para el reporte.");
                }

                DataTable dtReporte = new DataTable(nombreTabla);
                dtReporte.Columns.Add("UID", typeof(int));
                dtReporte.Columns.Add("TIPO", typeof(string));
                dtReporte.Columns.Add("NOMBRES", typeof(string));
                dtReporte.Columns.Add("APELLIDOS", typeof(string));
                dtReporte.Columns.Add("PROGRAMAS", typeof(string));

                dtReporte.Columns.Add("ID_LIS_SDN", typeof(int));
                dtReporte.Columns.Add("NUMERO", typeof(string));
                dtReporte.Columns.Add("PAIS", typeof(string));

                dtReporte.Columns.Add("DIRECCION", typeof(string));
                dtReporte.Columns.Add("ESTADO_PROVINCIA", typeof(string));
                dtReporte.Columns.Add("CIUDAD", typeof(string));

                dtReporte.Columns.Add("CATEGORIA", typeof(string));

                dtReporte.Columns.Add("PRINCIPAL", typeof(bool));

                foreach (var detalle in detalles)
                {
                    DataRow row = dtReporte.NewRow();
                    row["UID"] = detalle.UID;
                    row["TIPO"] = detalle.TIPO ?? "N/A";
                    row["NOMBRES"] = detalle.NOMBRES ?? "N/A";
                    row["APELLIDOS"] = detalle.APELLIDOS ?? "N/A";
                    row["PROGRAMAS"] = detalle.PROGRAMAS ?? "N/A";

                    dtReporte.Rows.Add(row);
                }

                foreach (var documento in documentos)
                {
                    DataRow row = dtReporte.NewRow();
                    row["ID_LIS_SDN"] = documento.ID_LIS_SDN;
                    row["NUMERO"] = documento.NUMERO ?? "N/A";
                    row["PAIS"] = documento.PAIS ?? "N/A";

                    dtReporte.Rows.Add(row);
                }

                foreach (var direccion in direcciones)
                {
                    DataRow row = dtReporte.NewRow();
                    row["DIRECCION"] = direccion.DIRECCION ?? "N/A";
                    row["ESTADO_PROVINCIA"] = direccion.ESTADO_PROVINCIA ?? "N/A";
                    row["CIUDAD"] = direccion.CIUDAD ?? "N/A";

                    dtReporte.Rows.Add(row);
                }

                foreach (var alias in aliasL)
                {
                    DataRow row = dtReporte.NewRow();
                    row["CATEGORIA"] = alias.CATEGORIA ?? "N/A";

                    dtReporte.Rows.Add(row);
                }

                foreach (var nacionalidad in nacionalidades)
                {
                    DataRow row = dtReporte.NewRow();
                    row["PRINCIPAL"] = nacionalidad.PRINCIPAL ?? false;

                    dtReporte.Rows.Add(row);
                }

                if (dtReporte == null || dtReporte.Rows.Count == 0)
                    throw new Exception("El DataTable está vacío...");

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