using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Monitoreo;
using Blo.Properties;


namespace View.Controllers.Monitoreo
{
    [NoCache]
    [Autorizacion]
    public class CatInstitucionController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICatInstitucionBlo _catInstitucionBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public CatInstitucionController(ICatInstitucionBlo catInstitucionBlo)
        {
            _catInstitucionBlo = catInstitucionBlo;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetCatInstitucion(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _catInstitucionBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(MON_CAT_INSTITUCION data)
        {
            MON_CAT_INSTITUCION catInstitusion = new MON_CAT_INSTITUCION();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catInstitucionBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    catInstitusion = _catInstitucionBlo.GetById(data.ID);

                catInstitusion.NOMBRE = data.NOMBRE;

                _catInstitucionBlo.Save(catInstitusion);
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
                _catInstitucionBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _catInstitucionBlo.Remove(id);
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