using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Monitoreo;

namespace View.Controllers.Listas
{
    [NoCache]
    [Autorizacion]
    public class PersonaOficioController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IPersonasOficioBlo _personasOficioBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public PersonaOficioController(IPersonasOficioBlo personasOficioBlo)
        {
            _personasOficioBlo = personasOficioBlo;
        }

        public ActionResult Index()
        {
            ViewBag.tiposDocumentos = _SQLBDEntities.VIEW_TIPO_DOCUMENTO.ToList();

            return View();
        }


        [HttpGet]
        public JsonResult GetPersonas(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _personasOficioBlo.GetAll().AsQueryable();

                //Buscar texto en varios campos 
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    records = records
                              .Where(x => (
                                  x.NOMBRE + " " +
                                  x.DUI + " " +
                                  x.NIT + " " +
                                  x.GENERALES + " " +
                                  x.TIPO_DOCUMENTO + " " +
                                  x.NUMERO_DOCUMENTO + " " +
                                  x.RESULTADO).ToUpper().Contains(searchString.Trim().ToUpper())
                               ).Select(x => x)
                              .AsQueryable();
                }

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
    }
}