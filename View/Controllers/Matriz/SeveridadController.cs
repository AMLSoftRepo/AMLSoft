using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Matriz;
using Blo.Properties;

namespace View.Controllers.Matriz
{
    [NoCache]
    [Autorizacion]
    public class SeveridadController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ISeveridadBlo _severidadBlo;
        private readonly ICatTipoRiesgoBlo _tipoRiesgoBlo;
        private readonly ICatSeveridadBlo _catSeverdidadBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public SeveridadController(ISeveridadBlo severidadBlo, ICatTipoRiesgoBlo tipoRiesgoBlo, ICatSeveridadBlo catSeverdidadBlo)
        {
            _severidadBlo = severidadBlo;
            _tipoRiesgoBlo = tipoRiesgoBlo;
            _catSeverdidadBlo = catSeverdidadBlo;
        }


        public ActionResult Index()
        {
            ViewBag.tipoRiesgo = _tipoRiesgoBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.impacto = _catSeverdidadBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();

            return View();
        }

        [HttpGet]
        public JsonResult GetSeveridad(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _severidadBlo.GetDatosGrid(out total, page, limit, sortBy, direction, true)
                             .Select(x => new
                             {
                                 x.ID,
                                 x.DESCRIPCION,
                                 x.ID_TIPO_RIESGO,
                                 DESCTIPORIESGO = x.MAT_CAT_TIPO_RIESGO.DESCRIPCION,
                                 x.ID_SEVERIDAD,
                                 DESCIMPACTO = x.MAT_CAT_SEVERIDAD.DESCRIPCION
                             });

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(MAT_SEVERIDAD data)
        {
            MAT_SEVERIDAD severidad = new MAT_SEVERIDAD();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _severidadBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    severidad = _severidadBlo.GetById(data.ID);

                severidad.ID_TIPO_RIESGO = data.ID_TIPO_RIESGO;
                severidad.ID_SEVERIDAD = data.ID_SEVERIDAD;
                severidad.DESCRIPCION = data.DESCRIPCION;

                _severidadBlo.Save(severidad);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Remove(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _severidadBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _severidadBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


    }
}