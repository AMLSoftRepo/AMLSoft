using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using Blo.Monitoreo;
using Blo.Reportes;
using Model;
using System.Data;

namespace View.Controllers.Reportes
{
    [NoCache]
    [Autorizacion]
    public class RptRespuestaOficioController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IContactoInstitucionBlo _contactoInstitucionBlo;
        private readonly IConfiguracionOficioBlo _configuracionOficioBlo;
        private readonly IReportesBlo _reportesBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RptRespuestaOficioController(IReportesBlo reportesBlo, IContactoInstitucionBlo contactoInstitucionBlo,
            IConfiguracionOficioBlo configuracionOficioBlo)
        {
            _reportesBlo = reportesBlo;
            _contactoInstitucionBlo = contactoInstitucionBlo;
            _configuracionOficioBlo = configuracionOficioBlo;
        }


        public ActionResult Index()
        {
            int idInstitucion = 0;
            var configuracionOficio = _configuracionOficioBlo.GetAll().ToList();

            //Obteniendo datos de configuracion general para oficios
            if (configuracionOficio.Any())
                idInstitucion = configuracionOficio.FirstOrDefault().ID_INSTITUCION;


            ViewBag.remitentes = _contactoInstitucionBlo.Remitentes(idInstitucion);
            ViewBag.destinatarios = _contactoInstitucionBlo.Destinatarios(idInstitucion);


            return View();
        }


        [HttpGet]
        public ActionResult Reporte(string fechaInicial, string fechaFinal, int idContactoDestino, int idContactoRemitente, string formato)
        {
            DateTime fechaIni = DateTime.Parse(fechaInicial);
            DateTime fechaFin = DateTime.Parse(fechaFinal).AddHours(23);

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "FechaInicial", fechaIni },
                { "FechaFinal", fechaFin }
            };

            try
            {
                var obtenerContactoDestino = _contactoInstitucionBlo.GetById(idContactoDestino, true);
                var obtenerContactoRemitente = _contactoInstitucionBlo.GetById(idContactoRemitente, true);


                //Crear parametros para la generación de la matriz
                parametros.Add("Titulo", obtenerContactoDestino.LIS_CAT_TITULOS.DESCRIPCION);
                parametros.Add("NombreDestino", obtenerContactoDestino.NOMBRE);
                parametros.Add("CargoDestino", obtenerContactoDestino.MON_CARGO_INSTITUCION.NOMBRE);
                parametros.Add("Unidad", obtenerContactoDestino.MON_CAT_UNIDAD.DESCRIPCION);
                parametros.Add("InstitucionDestino", obtenerContactoDestino.MON_CARGO_INSTITUCION.MON_CAT_INSTITUCION.NOMBRE);

                parametros.Add("NombreRemitente", obtenerContactoRemitente.NOMBRE.ToUpper());
                parametros.Add("CargoRemitente", obtenerContactoRemitente.MON_CARGO_INSTITUCION.NOMBRE.ToUpper());
                parametros.Add("InstitucionRemitente", obtenerContactoRemitente.MON_CARGO_INSTITUCION.MON_CAT_INSTITUCION.NOMBRE.ToUpper());

                string nombreReporte = "RptRespuestaOficio";
                string nombreTabla = "RptOficioDatos";

                var datosReporte = _reportesBlo.ReporteRespuestaOficios(fechaIni, fechaFin, obtenerContactoDestino.ID_UNIDAD);

                if (datosReporte == null || !datosReporte.Any())
                {
                    return Content("No hay datos disponibles para el reporte.");
                }

                DataTable dtReporte = new DataTable(nombreTabla);

                dtReporte.Columns.Add("NOMBRE", typeof(string));

                dtReporte.Columns.Add("ID_INSTITUCION", typeof(int));

                dtReporte.Columns.Add("ID_CONTACTO_SOLICITANTE", typeof(int));
                dtReporte.Columns.Add("ID_CONTACTO_INSTITUCION", typeof(int));
                dtReporte.Columns.Add("ID_CONTACTO_INSTITUCION_OFICIAL", typeof(int));
                dtReporte.Columns.Add("NUMERO_OFICIO", typeof(string));
                dtReporte.Columns.Add("REFERENCIA", typeof(string));
                dtReporte.Columns.Add("ORIGEN", typeof(string));
                dtReporte.Columns.Add("FECHA_OFICIO", typeof(DateTime));
                dtReporte.Columns.Add("FECHA_RECIBIDO", typeof(DateTime));
                dtReporte.Columns.Add("FORMATO_RESPUESTA", typeof(string));
                dtReporte.Columns.Add("FECHA_RESPUESTA_UIF", typeof(DateTime));
                dtReporte.Columns.Add("FECHA_RECIBIDO_UIF", typeof(DateTime));
                dtReporte.Columns.Add("DIAS_HABILES", typeof(int));
                dtReporte.Columns.Add("DIAS_MAX", typeof(int));
                dtReporte.Columns.Add("FECHA_MAXIMA_UIF", typeof(DateTime));
                dtReporte.Columns.Add("PROCESADO", typeof(bool));
                dtReporte.Columns.Add("FECHA_INVESTIGACION_INI", typeof(DateTime));
                dtReporte.Columns.Add("FECHA_INVESTIGACION_FIN", typeof(DateTime));
                dtReporte.Columns.Add("COMENTARIO", typeof(string));

                foreach (var datosreporte in datosReporte)
                {
                    dtReporte.Rows.Add(
                        obtenerContactoDestino.NOMBRE,  // "NOMBRE" (1ra columna)
                        datosreporte.ID_INSTITUCION,
                        datosreporte.ID_CONTACTO_SOLICITANTE,
                        datosreporte.ID_CONTACTO_INSTITUCION,
                        datosreporte.ID_CONTACTO_INSTITUCION_OFICIAL,
                        datosreporte.NUMERO_OFICIO ?? "DESCONOCIDO",
                        datosreporte.REFERENCIA ?? "DESCONOCIDO",
                        datosreporte.ORIGEN ?? "DESCONOCIDO",
                        datosreporte.FECHA_OFICIO,
                        datosreporte.FECHA_RECIBIDO,
                        datosreporte.FORMATO_RESPUESTA ?? "DESCONOCIDO",
                        datosreporte.FECHA_RESPUESTA_UIF,
                        datosreporte.FECHA_RECIBIDO_UIF ?? DateTime.MinValue,
                        datosreporte.DIAS_HABILES,
                        datosreporte.DIAS_MAX,
                        datosreporte.FECHA_MAXIMA_UIF,
                        datosreporte.PROCESADO,
                        datosreporte.FECHA_INVESTIGACION_INI ?? DateTime.MinValue,
                        datosreporte.FECHA_INVESTIGACION_FIN ?? DateTime.MinValue,
                        datosreporte.COMENTARIO ?? "DESCONOCIDO"
                    );
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