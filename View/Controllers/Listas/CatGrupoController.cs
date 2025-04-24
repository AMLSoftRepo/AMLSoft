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
    public class CatGrupoController : BaseController
    {
        private readonly ICatGrupoBlo _catGrupoBlo;
        
        public CatGrupoController(ICatGrupoBlo catGrupoBlo)
        {
            _catGrupoBlo = catGrupoBlo;
        }
        // GET: CatGrupo
        public ActionResult Index()
        {
            ViewBag.grupo = _catGrupoBlo.GetAll();
            return View();
        }
        [HttpGet]
        public JsonResult GetGrupo(int? page, int? limit, string sortBy, string direction, string searchString =null,long ID_GRUPO=0)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;

                var records = _catGrupoBlo.GetAll(true)
                    .Select(e => new
                    {
                        e.ID,
                        e.NOMBRE,
                        e.DESCRIPCION,
                        e.WEB
                    })
                    .AsQueryable();
                total = records.Count();
                records = SortHelper.OrdenarGrid(records, sortBy, direction).Skip(start).Take(limit.Value);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo los datos");

            }
        }

        [HttpPost]
        public JsonResult Save(LIS_CAT_GRUPO_FATF data)
        {
            LIS_CAT_GRUPO_FATF catGrupo = new LIS_CAT_GRUPO_FATF();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catGrupoBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    catGrupo = _catGrupoBlo.GetById(data.ID);
                catGrupo.NOMBRE = data.NOMBRE;
                catGrupo.DESCRIPCION = data.DESCRIPCION;
                catGrupo.WEB = data.WEB;

                _catGrupoBlo.Save(catGrupo);
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
                _catGrupoBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _catGrupoBlo.Remove(id);
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