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
    public class CatMezclaController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICatMezclaBlo _catMezclaBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public CatMezclaController(ICatMezclaBlo catMezclaBlo)
        {
            _catMezclaBlo = catMezclaBlo;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCatMezcla(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _catMezclaBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpPost]
        public JsonResult Save(MAT_CAT_MEZCLA data)
        {
            MAT_CAT_MEZCLA catMezcla = new MAT_CAT_MEZCLA();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catMezclaBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    catMezcla = _catMezclaBlo.GetById(data.ID);

                catMezcla.DESCRIPCION = data.DESCRIPCION;
                catMezcla.VALOR = data.VALOR;

                _catMezclaBlo.Save(catMezcla);
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
                _catMezclaBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _catMezclaBlo.Remove(id);
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