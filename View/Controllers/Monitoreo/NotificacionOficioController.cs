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
    public class NotificacionOficioController : OficioController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IOficioBlo _oficioBlo;
        private readonly IContactoInstitucionBlo _contactoInstitucionBlo;
        private readonly ICatInstitucionBlo _catInstitucionBlo;
        private readonly IConfiguracionOficioBlo _configuracionOficioBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public NotificacionOficioController(IOficioBlo oficioBlo, IPersonasOficioBlo personasOficioBlo,
            IContactoInstitucionBlo contactoInstitucionBlo, ICatInstitucionBlo catInstitucionBlo,
            IConfiguracionOficioBlo configuracionOficioBlo, ICatUnidadInstitucionBlo catUnidadInstitucionBlo)
            : base(oficioBlo, personasOficioBlo, contactoInstitucionBlo, catInstitucionBlo, configuracionOficioBlo, catUnidadInstitucionBlo)
        {
            _oficioBlo = oficioBlo;
            _contactoInstitucionBlo = contactoInstitucionBlo;
            _catInstitucionBlo = catInstitucionBlo;
            _configuracionOficioBlo = configuracionOficioBlo;
        }


        public ActionResult Notificacion()
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
        public JsonResult ObtenerEventos(double fechaIni, double fechaFin)
        {
            try
            {
                //convertir su marca de tiempo Unix
                //http://www.convert-unix-time.com/?t=1494201600
                //En este sitio hay ejemplos de conversiones unix usadas por el calendario
                DateTime vFechaInicial = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(fechaIni);
                DateTime vFechaFinal = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(fechaFin);


                int diasNotificarOficio = 0;
                var configuracion = _configuracionOficioBlo.GetAll();
                var eventos = _oficioBlo.GetCalendarioOficios(vFechaInicial, vFechaFinal);

                if (configuracion.Any())
                    diasNotificarOficio = configuracion.FirstOrDefault().DIAS_NOTIFICACION;

                var keyValues = eventos.Select(x => new
                {
                    title = x.NUMERO_OFICIO,
                    start = x.FECHA_MAXIMA_UIF.ToString("yyyy-MM-dd"),
                    backgroundColor = DateTime.Now.AddDays(diasNotificarOficio) >= x.FECHA_MAXIMA_UIF ? "#dd4b39" : "#00a65a",//Rojo y Verde
                    url = "/NotificacionOficio/EditOficio?" + x.ID
                }).ToList();


                return Json(keyValues, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                log.Error(e);
                return null;
            }

        }


        [HttpPost]
        public JsonResult EditOficio(long id)
        {
            MON_OFICIO oficio = new MON_OFICIO();
            try
            {
                if (id != 0)
                    oficio = _oficioBlo.GetById(id);

                return Json(new { oficio }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener el registro");
            }
        }


        [HttpPost]
        public JsonResult ProcesarOficio(int id)
        {
            MON_OFICIO oficio = new MON_OFICIO();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _oficioBlo.ValidarPermiso(SEG_PERMISO.POFICIOS);

                if (id != 0)
                    oficio = _oficioBlo.GetById(id);

                oficio.FECHA_RESPUESTA_UIF = DateTime.Now;
                oficio.PROCESADO = true;
                _oficioBlo.Save(oficio);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje, oficio.PROCESADO, oficio.ID }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Metodo que se ejecuta cada 30 segundos para notificar al usuario
        /// el número de oficios que necesitan ser procesados
        /// </summary>
        /// <returns>int cantidad de oficios pendientes</returns>
        [HttpPost]
        public JsonResult NotificarOficios()
        {
            int cantidad = 0;

            try
            {
                cantidad = _oficioBlo.GetTotalNotificarOficio();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return Json(new { cantidad }, JsonRequestBehavior.AllowGet);
        }


    }
}