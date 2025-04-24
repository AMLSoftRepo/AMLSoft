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
    public class CatFinancieraController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICatFinancieraBlo _catFinancieraBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public CatFinancieraController(ICatFinancieraBlo catFinancieraBlo)
        {
            _catFinancieraBlo = catFinancieraBlo;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetCatFinanciera(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _catFinancieraBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        /// <summary>
        /// validar existencia de codigo de financiera
        /// </summary>
        /// <param name="CODIGO_FINANCIERA"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult ValidaCatFinanciera(int CODIGO_FINANCIERA, int ID = 0)
        {
            bool valid = true;
            string message = "";

            if (_catFinancieraBlo.ExistCodFinanciera(ID, CODIGO_FINANCIERA).Any())
            {
                valid = false;
                message = "El codigo de financiera ya existe";
            }

            return valid ? Json(new { valid }, JsonRequestBehavior.AllowGet)
                         : Json(new { valid, message }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Save(MON_CAT_FINANCIERA data)
        {
            MON_CAT_FINANCIERA financiera = new MON_CAT_FINANCIERA();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catFinancieraBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    financiera = _catFinancieraBlo.GetById(data.ID);

                financiera.CODIGO_FINANCIERA = data.CODIGO_FINANCIERA;
                financiera.DESCRIPCION = data.DESCRIPCION;

                _catFinancieraBlo.Save(financiera);
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
                _catFinancieraBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _catFinancieraBlo.Remove(id);
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