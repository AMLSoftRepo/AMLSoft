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
    public class RptListaPersonalizadaDetalleController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reportesBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptListaPersonalizadaDetalleController(IReportesBlo reportesBlo)
        {
            _reportesBlo = reportesBlo;
        }

        // GET: RptListaPersonalizadaDetalle
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
                string nombreReporte = "RptListaPersonalizadaDetalle";
                string nombreTabla = "PersonalizadaDatos";

                var personalizadaDetalle = _reportesBlo.ReporteListaPersonalizadaDetalle(idPersona);
                var personalizadaLista = _reportesBlo.ReporteListaPersonalizadaDetalleLista(idPersona);
                var personalizadaDoc = _reportesBlo.ReporteListaPersonalizadaDetalleDocumento(idPersona);
                var personalizadaDir = _reportesBlo.ReporteListaPersonalizadaDetalleDireccion(idPersona);
                var personalizadaAlias = _reportesBlo.ReporteListaPersonalizadaDetalleAlias(idPersona);

                if ((personalizadaDetalle == null || !personalizadaDetalle.Any()) && (personalizadaLista == null || !personalizadaLista.Any()) && (personalizadaDoc == null || !personalizadaDoc.Any()) && (personalizadaDir == null || !personalizadaDir.Any()) && (personalizadaAlias == null || !personalizadaAlias.Any()))
                {
                    return Content("No hay datos disponibles para el reporte.");
                }

                DataTable dtReporte = new DataTable(nombreTabla);

                dtReporte.Columns.Add("ID_UNICO", typeof(string));
                dtReporte.Columns.Add("PRIMER_NOMBRE", typeof(string));
                dtReporte.Columns.Add("SEGUNDO_NOMBRE", typeof(string));
                dtReporte.Columns.Add("TERCER_NOMBRE", typeof(string));
                dtReporte.Columns.Add("PRIMER_APELLIDO", typeof(string));
                dtReporte.Columns.Add("SEGUNDO_APELLIDO", typeof(string));
                dtReporte.Columns.Add("TERCER_APELLIDO", typeof(string));
                dtReporte.Columns.Add("PAIS_NACIMIENTO", typeof(int));
                dtReporte.Columns.Add("RAZON", typeof(string));
                dtReporte.Columns.Add("PAIS_NACIMIENTO_NOMBRE", typeof(string));

                dtReporte.Columns.Add("NOMBRE_LISTA", typeof(string));
                dtReporte.Columns.Add("DESCRIPCION_LISTA", typeof(string));
                dtReporte.Columns.Add("URL_LISTA", typeof(string));
                dtReporte.Columns.Add("LLENADO_AUTOMATICO", typeof(bool));

                dtReporte.Columns.Add("ID_LISTA_PERSONALIZADA", typeof(int));
                dtReporte.Columns.Add("TIPO", typeof(string));
                dtReporte.Columns.Add("NUMERO", typeof(string));
                dtReporte.Columns.Add("PAIS_EXPEDICION", typeof(int));
                dtReporte.Columns.Add("COMENTARIO", typeof(string));
                dtReporte.Columns.Add("PAIS_EXPEDICION_NOMBRE", typeof(string));

                dtReporte.Columns.Add("DIRECCION_ESPECIFICA", typeof(string));
                dtReporte.Columns.Add("AVENIDA_CALLE_PASAJE", typeof(string));
                dtReporte.Columns.Add("ID_URBANIZACION", typeof(int));
                dtReporte.Columns.Add("ID_MUNICIPIO", typeof(int));
                dtReporte.Columns.Add("ID_DEPARTAMENTO", typeof(int));
                dtReporte.Columns.Add("ID_PAIS", typeof(int));
                dtReporte.Columns.Add("URBANIZACION", typeof(string));
                dtReporte.Columns.Add("MUNICIPIO", typeof(string));
                dtReporte.Columns.Add("DEPARTAMENTO", typeof(string));
                dtReporte.Columns.Add("PAIS", typeof(string));

                dtReporte.Columns.Add("ALIAS", typeof(string));
                dtReporte.Columns.Add("CALIDAD", typeof(string));

                foreach (var detalle in personalizadaDetalle)
                {
                    DataRow row = dtReporte.NewRow();
                    row["ID_UNICO"] = detalle.ID_UNICO ?? "N/A";
                    row["PRIMER_NOMBRE"] = detalle.PRIMER_NOMBRE ?? "N/A";
                    row["SEGUNDO_NOMBRE"] = detalle.SEGUNDO_NOMBRE ?? "N/A";
                    row["TERCER_NOMBRE"] = detalle.TERCER_NOMBRE ?? "N/A";
                    row["PRIMER_APELLIDO"] = detalle.PRIMER_APELLIDO ?? "N/A";
                    row["SEGUNDO_APELLIDO"] = detalle.SEGUNDO_APELLIDO ?? "N/A";
                    row["TERCER_APELLIDO"] = detalle.TERCER_APELLIDO ?? "N/A";
                    row["PAIS_NACIMIENTO"] = detalle.PAIS_NACIMIENTO ?? 0;
                    row["RAZON"] = detalle.RAZON ?? "N/A";
                    row["PAIS_NACIMIENTO_NOMBRE"] = detalle.PAIS_NACIMIENTO_NOMBRE ?? "N/A";

                    dtReporte.Rows.Add(row);
                }

                foreach (var lista in personalizadaLista)
                {
                    DataRow row = dtReporte.NewRow();
                    row["NOMBRE_LISTA"] = lista.NOMBRE_LISTA ?? "N/A";
                    row["DESCRIPCION_LISTA"] = lista.DESCRIPCION_LISTA ?? "N/A";
                    row["URL_LISTA"] = lista.URL_LISTA ?? "N/A";
                    row["LLENADO_AUTOMATICO"] = lista.LLENADO_AUTOMATICO ?? false;

                    dtReporte.Rows.Add(row);
                }

                foreach (var documento in personalizadaDoc)
                {
                    DataRow row = dtReporte.NewRow();
                    row["ID_LISTA_PERSONALIZADA"] = documento.ID_LISTA_PERSONALIZADA;
                    row["TIPO"] = documento.TIPO ?? "N/A";
                    row["NUMERO"] = documento.NUMERO ?? "N/A";
                    row["PAIS_EXPEDICION"] = documento.PAIS_EXPEDICION ?? 0;
                    row["COMENTARIO"] = documento.COMENTARIO ?? "N/A";
                    row["PAIS_EXPEDICION_NOMBRE"] = documento.PAIS_EXPEDICION_NOMBRE ?? "N/A";

                    dtReporte.Rows.Add(row);
                }

                foreach (var direccion in personalizadaDir)
                {
                    DataRow row = dtReporte.NewRow();
                    row["DIRECCION_ESPECIFICA"] = direccion.DIRECCION_ESPECIFICA ?? "N/A";
                    row["AVENIDA_CALLE_PASAJE"] = direccion.AVENIDA_CALLE_PASAJE ?? "N/A";
                    row["ID_URBANIZACION"] = direccion.ID_URBANIZACION ?? 0;
                    row["ID_MUNICIPIO"] = direccion.ID_MUNICIPIO ?? 0;
                    row["ID_DEPARTAMENTO"] = direccion.ID_DEPARTAMENTO ?? 0;
                    row["ID_PAIS"] = direccion.ID_PAIS ?? 0;
                    row["URBANIZACION"] = direccion.URBANIZACION ?? "N/A";
                    row["MUNICIPIO"] = direccion.MUNICIPIO ?? "N/A";
                    row["DEPARTAMENTO"] = direccion.DEPARTAMENTO ?? "N/A";
                    row["PAIS"] = direccion.PAIS ?? "N/A";

                    dtReporte.Rows.Add(row);
                }

                foreach (var alias in personalizadaAlias)
                {
                    DataRow row = dtReporte.NewRow();
                    row["ALIAS"] = alias.ALIAS ?? "N/A";
                    row["CALIDAD"] = alias.CALIDAD ?? "N/A";

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