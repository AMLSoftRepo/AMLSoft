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
    public class CargoInstitucionController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICargoInstitucionBlo _cargoInstitucionBlo;
        private readonly ICatInstitucionBlo _catInstitucionBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public CargoInstitucionController(ICargoInstitucionBlo cargoInstitucionBlo, ICatInstitucionBlo catInstitucionBlo)
        {
            _cargoInstitucionBlo = cargoInstitucionBlo;
            _catInstitucionBlo = catInstitucionBlo;
        }

        public ActionResult Index()
        {
            ViewBag.institucion = _catInstitucionBlo.GetAll().OrderBy(x => x.NOMBRE).ToList();

            return View();
        }


        [HttpGet]
        public JsonResult GetCargoInstitucion(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _cargoInstitucionBlo.GetDatosGrid(out total, page, limit, sortBy, direction, true)
                             .Select(x => new
                             {
                                 x.ID,
                                 x.NOMBRE,
                                 x.ID_INSTITUCION,
                                 NOMBREINSTITUCION = x.MON_CAT_INSTITUCION.NOMBRE
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
        public JsonResult Save(MON_CARGO_INSTITUCION data)
        {
            MON_CARGO_INSTITUCION cargoInstitucion = new MON_CARGO_INSTITUCION();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _cargoInstitucionBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    cargoInstitucion = _cargoInstitucionBlo.GetById(data.ID);

                cargoInstitucion.ID_INSTITUCION = data.ID_INSTITUCION;
                cargoInstitucion.NOMBRE = data.NOMBRE;

                _cargoInstitucionBlo.Save(cargoInstitucion);
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
                _cargoInstitucionBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _cargoInstitucionBlo.Remove(id);
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