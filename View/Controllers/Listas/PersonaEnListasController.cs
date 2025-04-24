using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;

namespace View.Controllers.Listas
{
    [NoCache]
    [Autorizacion]
    public class PersonaEnListasController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetPersonaEnListas(int? page, int? limit, string sortBy, string direction, string nombre)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                List<FN_PERSONA_EN_LISTAS_Result> resultados = new List<FN_PERSONA_EN_LISTAS_Result>();
                
                resultados = _SQLBDEntities.FN_PERSONA_EN_LISTAS(nombre.ToUpper().Trim()).ToList();
                var records = resultados.AsQueryable();

                total = resultados.Count();
                records = SortHelper.OrdenarGrid(records, sortBy, direction).Skip(start).Take(limit.Value);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }
    }
}