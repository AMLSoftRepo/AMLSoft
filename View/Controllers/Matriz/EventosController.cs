using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blo.Matriz;
using Blo.Alertas;
using Model;
using Blo.Properties;

namespace View.Controllers.Matriz
{
    [NoCache]
    [Autorizacion]
    public class EventosController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IEventoRiesgoBlo _eventoRiesgoBlo;
        private readonly ICatAgenciaBlo _catAgenciaBlo;
        private readonly ICatUnidadBlo _catUnidadBlo;
        private readonly ICatFactorRiesgoBlo _catFactorRiesgoBlo;
        private readonly IRiesgoBlo _riesgoBlo;
        private readonly ICausaRiesgoBlo _causaRiesgoBlo;
        private readonly ICatProbabilidadOcurrenciaBlo _catProbabilidadOcurrenciaBlo;
        private readonly ICatSeveridadBlo _catSeveridadBlo;
        private readonly IControlBlo _controlBlo;
        private readonly IControlEventoBlo _controlEventoBlo;
        private readonly ICatAutomatizacionBlo _catAutomatizacionBlo;
        private readonly ICatDisenoBlo _catDisenoBlo;
        private readonly ICatDocumentacionBlo _catDocumentacionBlo;
        private readonly ICatFrecuenciaBlo _catFrecuenciaBlo;
        private readonly ICatMezclaBlo _catMezclaBlo;
        private readonly ICatTipoControlBlo _catTipoControlBlo;
        private readonly IEscalaCalificacionBlo _escalaCalificacionBlo;
        private readonly IMatrizBlo _matrizBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public EventosController(IEventoRiesgoBlo eventoRiesgoBlo, ICatAgenciaBlo catAgenciaBlo, ICatUnidadBlo catUnidadBlo,
               ICatFactorRiesgoBlo catFactorRiesgoBlo, IRiesgoBlo riesgoBlo, ICausaRiesgoBlo causaRiesgoBlo,
               ICatProbabilidadOcurrenciaBlo catProbabilidadOcurrenciaBlo, ICatSeveridadBlo catSeveridadBlo,
               IControlBlo controlBlo, IControlEventoBlo controlEventoBlo, ICatAutomatizacionBlo catAutomatizacionBlo,
               ICatDisenoBlo catDisenoBlo, ICatDocumentacionBlo catDocumentacionBlo, ICatFrecuenciaBlo catFrecuenciaBlo,
               ICatMezclaBlo catMezclaBlo, ICatTipoControlBlo catTipoControlBlo, IEscalaCalificacionBlo escalaCalificacionBlo,
               IMatrizBlo matrizBlo)
        {
            _eventoRiesgoBlo = eventoRiesgoBlo;
            _catAgenciaBlo = catAgenciaBlo;
            _catUnidadBlo = catUnidadBlo;
            _catFactorRiesgoBlo = catFactorRiesgoBlo;
            _riesgoBlo = riesgoBlo;
            _causaRiesgoBlo = causaRiesgoBlo;
            _catProbabilidadOcurrenciaBlo = catProbabilidadOcurrenciaBlo;
            _catSeveridadBlo = catSeveridadBlo;
            _controlBlo = controlBlo;
            _controlEventoBlo = controlEventoBlo;
            _catAutomatizacionBlo = catAutomatizacionBlo;
            _catDisenoBlo = catDisenoBlo;
            _catDocumentacionBlo = catDocumentacionBlo;
            _catFrecuenciaBlo = catFrecuenciaBlo;
            _catMezclaBlo = catMezclaBlo;
            _catTipoControlBlo = catTipoControlBlo;
            _escalaCalificacionBlo = escalaCalificacionBlo;
            _matrizBlo = matrizBlo;
        }


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> EVENTOS
        public ActionResult Index()
        {
            ViewBag.agencia = _catAgenciaBlo.GetAll().Where(x => x.ID != 0).OrderBy(x => x.NOMBRE).ToList();
            ViewBag.unidad = _catUnidadBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.factorRiesgo = _catFactorRiesgoBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.riesgo = _riesgoBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.causaRiesgo = _causaRiesgoBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.probabilidadOcurrencia = _catProbabilidadOcurrenciaBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.impacto = _catSeveridadBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();

            return View();
        }


        [HttpGet]
        public JsonResult GetEventos(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _eventoRiesgoBlo.GetEventos(out total, page, limit, sortBy, direction, searchString);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(MAT_EVENTO_RIESGO data)
        {
            MAT_EVENTO_RIESGO evento = new MAT_EVENTO_RIESGO();
            string mensaje = PropertiesBlo.msgExito;
            string descripcionTipoRiesgo_riesgo;
            string descripcionTipoRiesgo_causaRiesgo;
            string descripcionRiesgo;
            string descripcionCausaRiesgo;
            try
            {
                _eventoRiesgoBlo.ValidarSave(data.ID);


                if (data.ID != 0)
                    evento = _eventoRiesgoBlo.GetById(data.ID);
                else
                    evento.ID_AGENCIA = data.ID_AGENCIA;

                evento.ID_UNIDAD = data.ID_UNIDAD;
                evento.ID_FACTOR_RIESGO = data.ID_FACTOR_RIESGO;
                evento.ID_RIESGO = data.ID_RIESGO;
                evento.ID_CAUSA_RIESGO = data.ID_CAUSA_RIESGO;
                evento.COMO = data.COMO;
                evento.ID_PROBABILIDAD_OCURRENCIA = data.ID_PROBABILIDAD_OCURRENCIA;
                evento.ID_IMPACTO = data.ID_IMPACTO;

                //Obtener datos para formar la descripción del evento
                descripcionTipoRiesgo_riesgo = _riesgoBlo.GetById(data.ID_RIESGO, true).MAT_CAT_TIPO_RIESGO.DESCRIPCION;
                descripcionTipoRiesgo_causaRiesgo = _causaRiesgoBlo.GetById(data.ID_CAUSA_RIESGO, true).MAT_CAT_TIPO_RIESGO.DESCRIPCION;
                descripcionRiesgo = _riesgoBlo.GetById(data.ID_RIESGO).DESCRIPCION;
                descripcionCausaRiesgo = _causaRiesgoBlo.GetById(data.ID_CAUSA_RIESGO).DESCRIPCION;

                //Campos calculados
                evento.DESCRIPCION = descripcionRiesgo + " " +
                                     descripcionCausaRiesgo + ". ¿Cómo podría pasar?: " +
                                     data.COMO + ", lo cual puede derivar en un riesgo " +
                                     ((descripcionTipoRiesgo_riesgo == descripcionTipoRiesgo_causaRiesgo) ? descripcionTipoRiesgo_riesgo :
                                     descripcionTipoRiesgo_riesgo + " y " + descripcionTipoRiesgo_causaRiesgo);


                _eventoRiesgoBlo.Save(evento);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje, evento.ID }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Remove(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _eventoRiesgoBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _eventoRiesgoBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerColor(decimal valor)
        {
            string color = "#ffffff";
            try
            {
                color = _escalaCalificacionBlo.ObtenerColorXValor(valor);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return Json(new { color }, JsonRequestBehavior.AllowGet);
        }


        //OBTENER UNIDADES POR AGENCIAS
        [HttpPost]
        public JsonResult GetUnidadesAgencia(int agencia)
        {
            try
            {
                var unidades = _SQLBDEntities.MAT_CAT_UNIDAD
                    .Where(x => x.ID_AGENCIA == agencia)
                    .Select(x => new
                    {
                        x.ID,
                        x.DESCRIPCION
                    }).ToList();

                return Json(new { unidades }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener los datos");
            }
        }

        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> CONTROLES
        [HttpPost]
        public ActionResult _Controles(long ID_EVENTO = 0)
        {
            ViewBag.automatizacion = _catAutomatizacionBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.deseno = _catDisenoBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.documentacion = _catDocumentacionBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.frecuencia = _catFrecuenciaBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.mezcla = _catMezclaBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.tipoControl = _catTipoControlBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.ID_EVENTO = ID_EVENTO;

            return PartialView("_Controles");
        }


        [HttpGet]
        public JsonResult GetControles(int? page, int? limit, string searchString = null, long ID_EVENTO = 0)
        {
            try
            {
                int total = 0;

                var records = _eventoRiesgoBlo.GetControles(out total, page, limit, searchString, ID_EVENTO);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpGet]
        public JsonResult GetSeleccionControles(int? page, int? limit, string searchString = null, long ID_EVENTO = 0)
        {
            try
            {
                int total = 0;

                var records = _eventoRiesgoBlo.GetSeleccionControles(out total, page, limit, searchString, ID_EVENTO);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult SaveControles(long ID_EVENTO, long ID_CONTROL)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _controlEventoBlo.ValidarPermiso(SEG_PERMISO.AGREGAR);
                _controlEventoBlo.SaveControl(ID_EVENTO, ID_CONTROL);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RemoveControles(long ID_EVENTO, long ID_CONTROL)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _controlEventoBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _controlEventoBlo.EliminarControles(ID_EVENTO, ID_CONTROL);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GuardarNuevoControl(long ID_EVENTO, MAT_CONTROL data)
        {
            MAT_CONTROL control = new MAT_CONTROL();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _controlEventoBlo.ValidarPermiso(SEG_PERMISO.AGREGAR);

                //Cargar agencia del evento actual
                int idAgencia = _eventoRiesgoBlo.GetById(ID_EVENTO).ID_AGENCIA;

                //Calcular total de porcentaje
                _controlBlo.CalcularTotalPorcentaje(ref data);
                control.ID_AGENCIA = idAgencia;
                control.ID_AUTOMATIZACION = data.ID_AUTOMATIZACION;
                control.ID_DISENO = data.ID_DISENO;
                control.ID_DOCUMENTACION = data.ID_DOCUMENTACION;
                control.ID_FRECUENCIA = data.ID_FRECUENCIA;
                control.ID_MEZCLA = data.ID_MEZCLA;
                control.ID_TIPO_CONTROL = data.ID_TIPO_CONTROL;
                control.DESCRIPCION = data.DESCRIPCION;
                control.OBSERVACIONES = data.OBSERVACIONES;
                control.TOTAL_POR = data.TOTAL_POR;

                _controlEventoBlo.GuargarNuevoControl(ID_EVENTO, control);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> GENERAR MATRIZ DE RIESGO
        [HttpPost]
        public JsonResult GenerarMatriz()
        {
            long idMatriz = 0;
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _controlEventoBlo.ValidarPermiso(SEG_PERMISO.GMATRIZ);

                idMatriz = _matrizBlo.GenerarMatriz();

            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje, idMatriz }, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}