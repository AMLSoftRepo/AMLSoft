using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Listas;
using Blo.Properties;

namespace View.Controllers.Listas
{
    [NoCache]
    [Autorizacion]

    public class CatEntidadController : BaseController
    {
        private readonly ICatEntidadBlo _catEntidadBlo;
        private readonly ICatOrganoBlo _catOrganoBlo;
        public CatEntidadController(ICatEntidadBlo catEntidadBlo, ICatOrganoBlo catOrganoBlo)
        {
            _catEntidadBlo = catEntidadBlo;
            _catOrganoBlo = catOrganoBlo;

        }
        public ActionResult Index()
        {
            ViewBag.organo = _catOrganoBlo.GetAll();

            return View();
        }

        [HttpGet]
        public JsonResult GetEntidad(int? page, int? limit, string sortBy, string direction)
        {

            try
            {
                int total = 0;
                var records = _catEntidadBlo.GetDatosGrid(out total, page, limit, sortBy, direction, true)
                    .Select(e => new
                    {
                        e.ID,
                        e.ID_ORGANO,
                        ORGANO = e.LIS_CAT_ORGANOS.DESCRIPCION,
                        e.DESCRIPCION
                    }).ToList();

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpPost]
        public JsonResult Save(LIS_CAT_ENTIDADES data)
        {
            LIS_CAT_ENTIDADES catEntidad = new LIS_CAT_ENTIDADES();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catEntidadBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    catEntidad = _catEntidadBlo.GetById(data.ID);
                catEntidad.ID_ORGANO = data.ID_ORGANO;
                catEntidad.DESCRIPCION = data.DESCRIPCION;

                _catEntidadBlo.Save(catEntidad);
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
                _catEntidadBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _catEntidadBlo.Remove(id);
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