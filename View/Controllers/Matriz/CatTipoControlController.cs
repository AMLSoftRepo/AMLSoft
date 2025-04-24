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
    public class CatTipoControlController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICatTipoControlBlo _catTipoControlBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public CatTipoControlController(ICatTipoControlBlo catTipoControlBlo)
        {
            _catTipoControlBlo = catTipoControlBlo;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetCatTipoControl(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _catTipoControlBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(MAT_CAT_TIPO_CONTROL data)
        {
            MAT_CAT_TIPO_CONTROL tipoControl = new MAT_CAT_TIPO_CONTROL();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catTipoControlBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    tipoControl = _catTipoControlBlo.GetById(data.ID);

                tipoControl.DESCRIPCION = data.DESCRIPCION;

                _catTipoControlBlo.Save(tipoControl);
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
                _catTipoControlBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _catTipoControlBlo.Remove(id);
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