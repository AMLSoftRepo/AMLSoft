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
    public class CatDocumentacionController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICatDocumentacionBlo _catDocumentacionBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public CatDocumentacionController(ICatDocumentacionBlo catDocumentacionBlo)
        {
            _catDocumentacionBlo = catDocumentacionBlo;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetCatDocumentacion(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _catDocumentacionBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(MAT_CAT_DOCUMENTACION data)
        {
            MAT_CAT_DOCUMENTACION catDocumentacion = new MAT_CAT_DOCUMENTACION();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catDocumentacionBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    catDocumentacion = _catDocumentacionBlo.GetById(data.ID);

                catDocumentacion.DESCRIPCION = data.DESCRIPCION;
                catDocumentacion.VALOR = data.VALOR;

                _catDocumentacionBlo.Save(catDocumentacion);
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
                _catDocumentacionBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _catDocumentacionBlo.Remove(id);
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