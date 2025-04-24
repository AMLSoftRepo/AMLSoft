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
    public class CatProbabilidadController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICatProbabilidadOcurrenciaBlo _catProbabilidadOcurrenciaBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public CatProbabilidadController(ICatProbabilidadOcurrenciaBlo catProbabilidadOcurrenciaBlo)
        {
            _catProbabilidadOcurrenciaBlo = catProbabilidadOcurrenciaBlo;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetCatProbabilidad(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _catProbabilidadOcurrenciaBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(MAT_CAT_PROBABILIDAD_OCURRENCIA data)
        {
            MAT_CAT_PROBABILIDAD_OCURRENCIA probabilidad = new MAT_CAT_PROBABILIDAD_OCURRENCIA();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catProbabilidadOcurrenciaBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    probabilidad = _catProbabilidadOcurrenciaBlo.GetById(data.ID);

                probabilidad.CLASIFICACION = data.CLASIFICACION;
                probabilidad.DESCRIPCION = data.DESCRIPCION;
                probabilidad.NIVEL = data.NIVEL;

                _catProbabilidadOcurrenciaBlo.Save(probabilidad);
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
                _catProbabilidadOcurrenciaBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _catProbabilidadOcurrenciaBlo.Remove(id);
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