using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Monitoreo;
using Ninject;
using Blo.Properties;

namespace View.Controllers.Monitoreo
{
    [NoCache]
    [Autorizacion]
    public class HistorialOficioController : NotificacionOficioController
    { /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IContactoInstitucionBlo _contactoInstitucionBlo;
        private readonly ICatInstitucionBlo _catInstitucionBlo;
        private readonly IConfiguracionOficioBlo _configuracionOficioBlo;
        private readonly IOficioBlo _oficioBlo;
        private readonly IPersonasOficioBlo _personasOficioBlo;
        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public HistorialOficioController(IOficioBlo oficioBlo, IPersonasOficioBlo personasOficioBlo,
            IContactoInstitucionBlo contactoInstitucionBlo, ICatInstitucionBlo catInstitucionBlo,
            IConfiguracionOficioBlo configuracionOficioBlo, ICatUnidadInstitucionBlo catUnidadInstitucionBlo)
            : base(oficioBlo, personasOficioBlo, contactoInstitucionBlo, catInstitucionBlo, configuracionOficioBlo, catUnidadInstitucionBlo)
        {
            _contactoInstitucionBlo = contactoInstitucionBlo;
            _catInstitucionBlo = catInstitucionBlo;
            _configuracionOficioBlo = configuracionOficioBlo;
            _oficioBlo = oficioBlo;
            _personasOficioBlo = personasOficioBlo;
        }


        public ActionResult Historial()
        {
            int idInstitucion = 0;
            var configuracionOficio = _configuracionOficioBlo.GetAll();


            ViewBag.institucion_cof = _catInstitucionBlo.GetAll().OrderBy(x => x.NOMBRE).ToList();
            //Obteniendo datos de configuracion general para oficios
            if (configuracionOficio.Any())
                idInstitucion = configuracionOficio.FirstOrDefault().ID_INSTITUCION;

            ViewBag.solicitante = _contactoInstitucionBlo.Destinatarios(idInstitucion);
            ViewBag.remitentes = _contactoInstitucionBlo.Remitentes(idInstitucion);
            ViewBag.destinatarios = _contactoInstitucionBlo.Destinatarios(idInstitucion);


            return View();
        }


        /// <summary>
        /// Obtener eventos
        /// </summary>
        /// <returns>JsonResult con un array de los eventos</returns>
        [HttpGet]
        public JsonResult ObtenerEventosHistorial(double fechaIni, double fechaFin)
        {
            try
            {
                //http://www.convert-unix-time.com/?t=1494201600
                //En este sitio hay ejemplos de conversiones unix usadas por el calendario
                DateTime vFechaInicial = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(fechaIni);
                DateTime vFechaFinal = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(fechaFin);

                var eventos = _oficioBlo.GetHistorialCalendarioOficios(vFechaInicial, vFechaFinal);


                var keyValues = eventos.Select(x => new
                {
                    title = x.NUMERO_OFICIO,
                    start = x.FECHA_MAXIMA_UIF.ToString("yyyy-MM-dd"),
                    backgroundColor = x.CUMPLIMIENTO <= 0 ? "#00a65a" : "#dd4b39",//Verde y Rojo
                    url = "/HistorialOficio/EditOficio?" + x.ID
                }).ToList();


                return Json(keyValues, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                log.Error(e);
                return null;
            }
        }


        [HttpGet]
        public JsonResult GetOficioHistorial(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0; ;
                var records = _oficioBlo.GetHistorialOficios(out total, page, limit, sortBy, direction, searchString);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }



    }
}