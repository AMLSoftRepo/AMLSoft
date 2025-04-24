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
    public class CatAgenciaController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICatAgenciaBlo _catAgenciaBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public CatAgenciaController(ICatAgenciaBlo catAgenciaBlo)
        {
            _catAgenciaBlo = catAgenciaBlo;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetCatAgencia(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _catAgenciaBlo.GetDatosGrid(out total, page, limit, sortBy, direction);

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
        /// <param name="CODIGO_AGENCIA"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult ValidaCatAgencia( int CODIGO_AGENCIA, int ID = 0)
        {
            bool valid = true;
            string message = "";

            if (_catAgenciaBlo.ExistCodAgencia(ID,CODIGO_AGENCIA).Any())
            {
                valid = false;
                message = "El codigo de Agencia ya existe";
            }

            return valid ? Json(new { valid }, JsonRequestBehavior.AllowGet)
                         : Json(new { valid, message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(MAT_CAT_AGENCIA data)
        {
            MAT_CAT_AGENCIA catAgencia = new MAT_CAT_AGENCIA();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _catAgenciaBlo.ValidarSave(data.ID);

                if(data.CODIGO_AGENCIA == 0 && data.ID == 0)
                    throw new Exception("El registro esta reservado para procesos internos.");

                if (data.ID != 0)
                    catAgencia = _catAgenciaBlo.GetById(data.ID); 
       

                catAgencia.CODIGO_AGENCIA = data.CODIGO_AGENCIA;
                catAgencia.NOMBRE = data.NOMBRE;

                _catAgenciaBlo.Save(catAgencia);
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
                _catAgenciaBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);

                if (id == 0)
                    throw new Exception("El registro esta reservado para procesos internos.");

                _catAgenciaBlo.Remove(id);
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