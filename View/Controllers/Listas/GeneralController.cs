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
    public class GeneralController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IGeneralBlo _generalBlo;
        private readonly IGeneralPersonalizadaBlo _generalPersonalizadaBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public GeneralController(IGeneralBlo generalBlo, IGeneralPersonalizadaBlo generalPersonalizadaBlo)
        {
            _generalBlo = generalBlo;
            _generalPersonalizadaBlo = generalPersonalizadaBlo;
        }
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetGeneral(int? page, int? limit, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _generalBlo.GetDatosGrid(out total, page, limit, "NOMBRE_LISTA", "asc");
                    
                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(LIS_GENERAL data)
        {
            LIS_GENERAL general = new LIS_GENERAL();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _generalBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    general = _generalBlo.GetById(data.ID);

                general.NOMBRE_LISTA = data.NOMBRE_LISTA;
                general.DESCRIPCION_LISTA = data.DESCRIPCION_LISTA;
                general.URL_LISTA = data.URL_LISTA;
                general.LLENADO_AUTOMATICO = data.LLENADO_AUTOMATICO;

                _generalBlo.Save(general);
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
                _generalBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                //_generalPersonalizadaBlo.EliminarRelacionesLista(id);
                _generalBlo.Remove(id);
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