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
    public class CatOrganoController : BaseController
    {
        private readonly ICatOrganoBlo _catOrganoBlo;

        public CatOrganoController(ICatOrganoBlo catOrganoBlo)
        {
            _catOrganoBlo =catOrganoBlo;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCatOrgano(int? page, int? limit, string sortBy, string direction, string searchString =null)
        {
            try
            {
                int total = 0;
                var records = _catOrganoBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(LIS_CAT_ORGANOS data)
        {
            LIS_CAT_ORGANOS catOrgano = new LIS_CAT_ORGANOS();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catOrganoBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    catOrgano = _catOrganoBlo.GetById(data.ID);

                catOrgano.DESCRIPCION = data.DESCRIPCION;

                _catOrganoBlo.Save(catOrgano);
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
                _catOrganoBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _catOrganoBlo.Remove(id);
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