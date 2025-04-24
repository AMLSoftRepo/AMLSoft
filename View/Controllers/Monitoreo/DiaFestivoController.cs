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
    public class DiaFestivoController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IDiaFestivoBlo _diaFestivoBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public DiaFestivoController(IDiaFestivoBlo diaFestivoBlo)
        {
            _diaFestivoBlo = diaFestivoBlo;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetDiaFestivo(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _diaFestivoBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(MON_DIA_FESTIVO data)
        {
            MON_DIA_FESTIVO diaFestivo = new MON_DIA_FESTIVO();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _diaFestivoBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    diaFestivo = _diaFestivoBlo.GetById(data.ID);

                diaFestivo.DESCRIPCION = data.DESCRIPCION;
                diaFestivo.FECHA_INICIO = data.FECHA_INICIO;
                diaFestivo.FECHA_FIN = data.FECHA_FIN;

                _diaFestivoBlo.Save(diaFestivo);
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
                _diaFestivoBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _diaFestivoBlo.Remove(id);
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