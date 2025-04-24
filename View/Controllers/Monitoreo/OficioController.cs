using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Monitoreo;
using System.Globalization;
using Novacode;
using System.Text;
using System.Data;
using System.IO;
using System.Drawing;
using Blo.Properties;

namespace View.Controllers.Monitoreo
{
    [NoCache]
    [Autorizacion]
    public class OficioController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IOficioBlo _oficioBlo;
        private readonly IPersonasOficioBlo _personasOficioBlo;
        private readonly IContactoInstitucionBlo _contactoInstitucionBlo;
        private readonly ICatInstitucionBlo _catInstitucionBlo;
        private readonly IConfiguracionOficioBlo _configuracionOficioBlo;
        private readonly ICatUnidadInstitucionBlo _catUnidadInstitucionBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public OficioController(IOficioBlo oficioBlo, IPersonasOficioBlo personasOficioBlo,
            IContactoInstitucionBlo contactoInstitucionBlo, ICatInstitucionBlo catInstitucionBlo,
            IConfiguracionOficioBlo configuracionOficioBlo, ICatUnidadInstitucionBlo catUnidadInstitucionBlo)
        {
            _oficioBlo = oficioBlo;
            _personasOficioBlo = personasOficioBlo;
            _contactoInstitucionBlo = contactoInstitucionBlo;
            _catInstitucionBlo = catInstitucionBlo;
            _configuracionOficioBlo = configuracionOficioBlo;
            _catUnidadInstitucionBlo = catUnidadInstitucionBlo;
        }

        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> OFICIOS
        public ActionResult Index()
        {
            int idInstitucion = 0;
            int idContacto = 0;
            var configuracionOficio = _configuracionOficioBlo.GetAll();


            ViewBag.institucion_cof = _catInstitucionBlo.GetAll().OrderBy(x => x.NOMBRE).ToList();
            //Obteniendo datos de configuracion general para oficios
            if (configuracionOficio.Any())
            {
                idInstitucion = configuracionOficio.FirstOrDefault().ID_INSTITUCION;
                idContacto = configuracionOficio.FirstOrDefault().ID_CONTACTO_INSTITUCION_OFICIAL;
            }

            ViewBag.idContacto = idContacto;
            ViewBag.solicitante = _contactoInstitucionBlo.Destinatarios(idInstitucion);
            ViewBag.remitentes = _contactoInstitucionBlo.Remitentes(idInstitucion);
            ViewBag.destinatarios = _contactoInstitucionBlo.Destinatarios(idInstitucion);

            return View();
        }


        [HttpGet]
        public JsonResult GetOficio(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0; ;
                var records = _oficioBlo.GetOficios(out total, page, limit, sortBy, direction, searchString);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(MON_OFICIO data)
        {
            MON_OFICIO oficio = new MON_OFICIO();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _oficioBlo.ValidarSave(data.ID);

                //Obtener datos calculados
                _oficioBlo.GetDiasHabiles(ref data);
                _oficioBlo.GetFechaMaxima(ref data);
                _oficioBlo.GetCumplimiento(ref data);


                if (data.ID != 0) oficio = _oficioBlo.GetById(data.ID);
                else oficio.FECHA_RESPUESTA_UIF = DateTime.Now;

                oficio.ID_CONTACTO_SOLICITANTE = data.ID_CONTACTO_SOLICITANTE;
                oficio.ID_CONTACTO_INSTITUCION = data.ID_CONTACTO_INSTITUCION;
                oficio.ID_CONTACTO_INSTITUCION_OFICIAL = data.ID_CONTACTO_INSTITUCION_OFICIAL;
                oficio.NUMERO_OFICIO = data.NUMERO_OFICIO;
                oficio.REFERENCIA = data.REFERENCIA;
                oficio.FECHA_OFICIO = data.FECHA_OFICIO;
                oficio.FECHA_RECIBIDO = data.FECHA_RECIBIDO;
                oficio.FECHA_RECIBIDO_UIF = data.FECHA_RECIBIDO_UIF;
                oficio.FECHA_INVESTIGACION_INI = data.FECHA_INVESTIGACION_INI;
                oficio.FECHA_INVESTIGACION_FIN = data.FECHA_INVESTIGACION_FIN;
                oficio.FORMATO_RESPUESTA = data.FORMATO_RESPUESTA;
                oficio.DIAS_MAX = data.DIAS_MAX;
                oficio.ORIGEN = data.ORIGEN;
                oficio.COMENTARIO=data.COMENTARIO;

                //Calculados
                oficio.CUMPLIMIENTO = data.CUMPLIMIENTO;
                oficio.DIAS_HABILES = data.DIAS_HABILES;
                oficio.FECHA_MAXIMA_UIF = data.FECHA_MAXIMA_UIF;

                _oficioBlo.Save(oficio);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje, oficio.ID }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Remove(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _oficioBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _oficioBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public void ImprimirOficio(int id)
        {
            try
            {
                string path = Request.MapPath(Request.ApplicationPath) + @"Plantillas\RespuestaOficio.docx";
                var datos = _oficioBlo.ImprimirOficio(id, path);

                //Descargar documento
                MemoryStream ms = new MemoryStream();
                datos.DocX.SaveAs(ms);
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename=\"Oficio_" + datos.NumeroOficio + ".docx\"");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                ms.WriteTo(Response.OutputStream);
                Response.End();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                string pathError = Request.MapPath(Request.ApplicationPath) + @"Plantillas\ModeloError.docx";
                DocX doc = DocX.Load(pathError);

                //Descargar documento
                MemoryStream ms = new MemoryStream();
                doc.SaveAs(ms);
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("content-disposition", "attachment; filename=Oficio_Error.doc");
                ms.WriteTo(Response.OutputStream);
                Response.End();
            }
        }

        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> IMPLEMENTACION DE PANEL DE CONFIGURACION
        [HttpPost]
        public JsonResult ObtenerContactosPorInstitucion(int id)
        {
            try
            {
                var contacto = _contactoInstitucionBlo.Remitentes(id)
                    .Select(x => new { x.ID,x.NOMBRE});

                return Json(new { contacto }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener los datos");
            }
        }



        [HttpPost]
        public JsonResult AplicarConfiguracion(int ID_INSTITUCION_CONF, int ID_CONTACTO_CONF, int NUMDIAS_CONF)
        {
            string mensaje = PropertiesBlo.msgExito;
            MON_OFICIO_CONFIGURACION conf = new MON_OFICIO_CONFIGURACION();
            try
            {
                _configuracionOficioBlo.ValidarPermiso(SEG_PERMISO.ACONFGOFICIO);
                var configuracionOficio = _configuracionOficioBlo.GetAll();

                if (configuracionOficio.Any())
                    conf = configuracionOficio.FirstOrDefault();

                conf.ID_INSTITUCION = ID_INSTITUCION_CONF;
                conf.ID_CONTACTO_INSTITUCION_OFICIAL = ID_CONTACTO_CONF;
                conf.DIAS_NOTIFICACION = NUMDIAS_CONF;
                _configuracionOficioBlo.Save(conf);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ObtenerDatosConfi()
        {
            try
            {
                MON_OFICIO_CONFIGURACION conf = new MON_OFICIO_CONFIGURACION();
                var configuracionOficio = _configuracionOficioBlo.GetAll();

                if (configuracionOficio.Any())
                    conf = configuracionOficio.FirstOrDefault();

                return Json(new { conf }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener los datos");
            }
        }

        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> PERSONAS
        [HttpPost]
        public ActionResult _Personas(long ID_OFICIO = 0)
        {
            ViewBag.ID_OFICIO = ID_OFICIO;
            ViewBag.tiposDocumentos = _SQLBDEntities.VIEW_TIPO_DOCUMENTO.ToList();

            return PartialView("_Personas");
        }


        [HttpGet]
        public JsonResult GetPersonas(int? page, int? limit, string sortBy, string direction, long ID_OFICIO = 0)
        {
            try
            {
                int total = 0;

                var records = _personasOficioBlo.GetDatosDetalle(out total, page, limit, sortBy, direction, "ID_OFICIO", ID_OFICIO);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult SavePersonas(MON_OFICIO_PERSONA data)
        {
            MON_OFICIO_PERSONA personas = new MON_OFICIO_PERSONA();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _personasOficioBlo.ValidarSave(data.ID);

                personas.ID_OFICIO = data.ID_OFICIO;

                if (data.ID != 0)
                    personas = _personasOficioBlo.GetById(data.ID);

                personas.NOMBRE = data.NOMBRE;
                personas.TIPO_PERSONA = data.TIPO_PERSONA;
                personas.GENERALES = data.GENERALES;
                personas.DUI = data.DUI;
                personas.NIT = data.NIT;
                personas.TIPO_DOCUMENTO = data.TIPO_DOCUMENTO;
                personas.NUMERO_DOCUMENTO = data.NUMERO_DOCUMENTO;
                personas.RESULTADO = data.RESULTADO;
                personas.COTIZACIONES = data.COTIZACIONES;
                personas.SOLICITUDES = data.SOLICITUDES;
                personas.PRESTAMOS = data.PRESTAMOS;
                personas.CHEQUES = data.CHEQUES;
                personas.AEX = data.AEX;

                _personasOficioBlo.Save(personas);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RemovePersonas(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _personasOficioBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _personasOficioBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}