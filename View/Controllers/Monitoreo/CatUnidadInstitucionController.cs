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
    public class CatUnidadInstitucionController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICatUnidadInstitucionBlo _catUnidadInstitucionBlo;
        private readonly ICatInstitucionBlo _catInstitucionBlo;
        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public CatUnidadInstitucionController(ICatUnidadInstitucionBlo catUnidadInstitucionBlo, ICatInstitucionBlo catInstitucionBlo)
        {
            _catUnidadInstitucionBlo = catUnidadInstitucionBlo;
            _catInstitucionBlo = catInstitucionBlo;
        }

        public ActionResult Index()
        {
            ViewBag.institucion = _catInstitucionBlo.GetAll().ToList();
 

            return View();
        }


        [HttpGet]
        public JsonResult GetCatUnidad(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _catUnidadInstitucionBlo.GetAll(true)
                  
                    .Select(u => new 
                    {
                        u.ID,
                        u.ID_INSTITUCION,
                        INSTITUCION = u.MON_CAT_INSTITUCION.NOMBRE,
                        u.DESCRIPCION 
                    })
                    .AsQueryable();
              
                total = records.Count();
                records = SortHelper.OrdenarGrid(records, sortBy, direction).Skip(start).Take(limit.Value);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(MON_CAT_UNIDAD data)
        {
            MON_CAT_UNIDAD unidad = new MON_CAT_UNIDAD();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catUnidadInstitucionBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    unidad = _catUnidadInstitucionBlo.GetById(data.ID);

                unidad.ID_INSTITUCION = data.ID_INSTITUCION;
                unidad.DESCRIPCION = data.DESCRIPCION;

                _catUnidadInstitucionBlo.Save(unidad);
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
                _catUnidadInstitucionBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _catUnidadInstitucionBlo.Remove(id);
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