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
    public class AgenciaBancariaController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IAgenciaBancariaBlo _agenciaBancariaBlo;
        private readonly ICatFinancieraBlo _catFinancieraBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public AgenciaBancariaController(IAgenciaBancariaBlo agenciaBancariaBlo, ICatFinancieraBlo catFinancieraBlo)
        {
            _agenciaBancariaBlo = agenciaBancariaBlo;
            _catFinancieraBlo = catFinancieraBlo;
        }

        public ActionResult Index()
        {
            ViewBag.financiera = _catFinancieraBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();

            return View();
        }


        [HttpGet]
        public JsonResult GetAgenciaBancaria(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _agenciaBancariaBlo.GetDatosGrid(out total, page, limit, sortBy, direction, true)
                             .Select(x => new
                             {
                                 x.ID,
                                 x.DESCRIPCION,
                                 x.ID_FINANCIERA,
                                 DESCFINANCIERA = x.MON_CAT_FINANCIERA.DESCRIPCION
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
        public JsonResult Save(MON_AGENCIA_BANCARIA data)
        {
            MON_AGENCIA_BANCARIA agenciaBancaria = new MON_AGENCIA_BANCARIA();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _agenciaBancariaBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    agenciaBancaria = _agenciaBancariaBlo.GetById(data.ID);

                agenciaBancaria.ID_FINANCIERA = data.ID_FINANCIERA;
                agenciaBancaria.DESCRIPCION = data.DESCRIPCION;

                _agenciaBancariaBlo.Save(agenciaBancaria);
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
                _agenciaBancariaBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _agenciaBancariaBlo.Remove(id);
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