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
    public class CatDisenoController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICatDisenoBlo _catDisenoBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public CatDisenoController(ICatDisenoBlo catDisenoBlo)
        {
            _catDisenoBlo = catDisenoBlo;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetCatDiseno(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _catDisenoBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(MAT_CAT_DISENO data)
        {
            MAT_CAT_DISENO catDiseno = new MAT_CAT_DISENO();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catDisenoBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    catDiseno = _catDisenoBlo.GetById(data.ID);

                catDiseno.DESCRIPCION = data.DESCRIPCION;
                catDiseno.VALOR = data.VALOR;

                _catDisenoBlo.Save(catDiseno);
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
                _catDisenoBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _catDisenoBlo.Remove(id);
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