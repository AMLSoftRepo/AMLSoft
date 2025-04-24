using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blo.Alertas;
using Blo.Matriz;
using Blo.Properties;
using Model;

namespace View.Controllers.Alertas
{
    [NoCache]
    [Autorizacion]
    public class OperacionIrregularController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IOperacionIrregularBlo _operacionIrregularBlo;
        private readonly ITipoAlertaBlo _tipoAlertaBlo;
        private readonly ICatAgenciaBlo _catAgenciaBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public OperacionIrregularController(IOperacionIrregularBlo operacionIrregularBlo,ITipoAlertaBlo tipoAlertaBlo, ICatAgenciaBlo catAgenciaBlo)
        {
            _operacionIrregularBlo = operacionIrregularBlo;
            _tipoAlertaBlo = tipoAlertaBlo;
            _catAgenciaBlo = catAgenciaBlo;
        }

        public ActionResult Index()
        {
            ViewBag.tipoAlerta = _tipoAlertaBlo.GetAll();
            ViewBag.catAgencia = _catAgenciaBlo.GetAll();

            return View();
        }


        [HttpGet]
        public JsonResult GetOperacionIrregular(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _operacionIrregularBlo.GetOperacionesIrregulares(out total,page,limit,sortBy,direction,searchString);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Save(ALE_OPERACION_IRREGULAR data)
        {
            ALE_OPERACION_IRREGULAR operacionIrregular = new ALE_OPERACION_IRREGULAR();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _tipoAlertaBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    operacionIrregular = _operacionIrregularBlo.GetById(data.ID);

                operacionIrregular.ID_TIPO_ALERTA = data.ID_TIPO_ALERTA;
                operacionIrregular.ID_AGENCIA = data.ID_AGENCIA;
                operacionIrregular.EXPRESION = data.EXPRESION;
                operacionIrregular.DESCRIPCION = data.DESCRIPCION;

                _operacionIrregularBlo.Save(operacionIrregular);
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
                _operacionIrregularBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _operacionIrregularBlo.Remove(id);
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