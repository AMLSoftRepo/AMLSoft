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
    public class CatTituloController : BaseController
    {

        private readonly ICatTituloBlo _catTituloBlo;

        public CatTituloController(ICatTituloBlo catTituloBlo) 
        {
            _catTituloBlo = catTituloBlo;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetTitulo(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _catTituloBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(LIS_CAT_TITULOS data)
        {
            LIS_CAT_TITULOS titulo = new LIS_CAT_TITULOS();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catTituloBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    titulo = _catTituloBlo.GetById(data.ID);

                titulo.ABREVIATURA = data.ABREVIATURA;
                titulo.DESCRIPCION = data.DESCRIPCION;
                

                _catTituloBlo.Save(titulo);
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
                _catTituloBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _catTituloBlo.Remove(id);
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