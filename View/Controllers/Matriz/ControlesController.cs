using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blo.Matriz;
using Model;
using Blo.Properties;

namespace View.Controllers.Matriz
{
    [NoCache]
    [Autorizacion]
    public class ControlesController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IControlBlo _controlBlo;
        private readonly ICatAgenciaBlo _catAgenciaBlo;
        private readonly ICatAutomatizacionBlo _catAutomatizacionBlo;
        private readonly ICatDisenoBlo _catDisenoBlo;
        private readonly ICatDocumentacionBlo _catDocumentacionBlo;
        private readonly ICatFrecuenciaBlo _catFrecuenciaBlo;
        private readonly ICatMezclaBlo _catMezclaBlo;
        private readonly ICatTipoControlBlo _catTipoControlBlo;


        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public ControlesController(ICatAgenciaBlo catAgenciaBlo, ICatAutomatizacionBlo catAutomatizacionBlo,
          ICatDisenoBlo catDisenoBlo, ICatDocumentacionBlo catDocumentacionBlo, ICatFrecuenciaBlo catFrecuenciaBlo,
          ICatMezclaBlo catMezclaBlo, ICatTipoControlBlo catTipoControlBlo, IControlBlo controlBlo)
        {
            _controlBlo = controlBlo;
            _catAgenciaBlo = catAgenciaBlo;
            _catAutomatizacionBlo = catAutomatizacionBlo;
            _catDisenoBlo = catDisenoBlo;
            _catDocumentacionBlo = catDocumentacionBlo;
            _catFrecuenciaBlo = catFrecuenciaBlo;
            _catMezclaBlo = catMezclaBlo;
            _catTipoControlBlo = catTipoControlBlo;
        }


        public ActionResult Index()
        {
            ViewBag.agencia = _catAgenciaBlo.GetAll().Where(x => x.ID != 0).OrderBy(x => x.NOMBRE).ToList();
            ViewBag.automatizacion = _catAutomatizacionBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.deseno = _catDisenoBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.documentacion = _catDocumentacionBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.frecuencia = _catFrecuenciaBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.mezcla = _catMezclaBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.tipoControl = _catTipoControlBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();

            return View();
        }

        [HttpGet]
        public JsonResult GetControles(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _controlBlo.GetControles(out total, page, limit, sortBy, direction, searchString);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(MAT_CONTROL data)
        {
            MAT_CONTROL control = new MAT_CONTROL();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _controlBlo.ValidarSave(data.ID);

                //Calcular total de porcentaje
                _controlBlo.CalcularTotalPorcentaje(ref data);

                if (data.ID != 0)
                    control = _controlBlo.GetById(data.ID);

                control.ID_AGENCIA = data.ID_AGENCIA;
                control.ID_AUTOMATIZACION = data.ID_AUTOMATIZACION;
                control.ID_DISENO = data.ID_DISENO;
                control.ID_DOCUMENTACION = data.ID_DOCUMENTACION;
                control.ID_FRECUENCIA = data.ID_FRECUENCIA;
                control.ID_MEZCLA = data.ID_MEZCLA;
                control.ID_TIPO_CONTROL = data.ID_TIPO_CONTROL;
                control.DESCRIPCION = data.DESCRIPCION;
                control.OBSERVACIONES = data.OBSERVACIONES;
                control.TOTAL_POR = data.TOTAL_POR;

                _controlBlo.Save(control);
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
                _controlBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _controlBlo.Remove(id);
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