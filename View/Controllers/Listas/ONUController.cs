using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Listas;
using Blo.Seguridad;
using System.Threading;
using Blo.Properties;

namespace View.Controllers.Listas
{
    [NoCache]
    [Autorizacion]
    public class ONUController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IONUBlo _onuBlo;
        private readonly IParametroONUSDNBlo _parametroONUSDNBlo;
        private readonly IONUAliasBlo _onuAliasBlo;
        private readonly IONUDocumentoBlo _onuDocumentoBlo;
        private readonly IONUDireccionBlo _onuDireccionBlo;
        private readonly ICargaDatosBlo _cargaDatosBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public ONUController(IONUBlo onuBlo, IParametroONUSDNBlo parametroONUSDNBlo, IONUAliasBlo onuAliasBlo, IONUDocumentoBlo onuDocumentoBlo,
            IONUDireccionBlo onuDireccionBlo, ICargaDatosBlo cargaDatosBlo)
        {
            _onuBlo = onuBlo;
            _parametroONUSDNBlo = parametroONUSDNBlo;
            _onuAliasBlo = onuAliasBlo;
            _onuDocumentoBlo = onuDocumentoBlo;
            _onuDireccionBlo = onuDireccionBlo;
            _cargaDatosBlo = cargaDatosBlo;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetONU(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;

                var records = _onuBlo.GetAll()
                   .Select(x => new
                   {
                       x.ID,
                       x.DATA_ID,
                       x.TIPO,
                       x.VERSION_NUM,
                       NOMBRE = x.FIRST_NAME + " " + x.SECOND_NAME + " " + x.THIRD_NAME + " " + x.FOURTH_NAME,
                       x.UN_LIST_TYPE,
                       x.REFERENCE_NUMBER,
                       x.LISTED_ON,
                       x.COMMENTS,
                       x.NATIONALITY
                   }).AsQueryable();

                //Buscar texto en varios campos 
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    records = records
                              .Where(x => (
                                  x.TIPO + " " +
                                  x.NOMBRE + " " +
                                  x.UN_LIST_TYPE + " " +
                                  x.LISTED_ON.ToString() + " " +
                                  x.COMMENTS + " " +
                                  x.NATIONALITY).ToUpper().Contains(searchString.Trim().ToUpper())
                               )
                              .Select(x => new
                              {
                                  x.ID,
                                  x.DATA_ID,
                                  x.TIPO,
                                  x.VERSION_NUM,
                                  x.NOMBRE,
                                  x.UN_LIST_TYPE,
                                  x.REFERENCE_NUMBER,
                                  x.LISTED_ON,
                                  x.COMMENTS,
                                  x.NATIONALITY
                              }).AsQueryable();
                }

                total = records.Count();
                records = SortHelper.OrdenarGrid(records, sortBy, direction).Skip(start).Take(limit.Value);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpPost]
        public JsonResult Save()
        {
            string mensaje = PropertiesBlo.msgProcesando;
            try
            {
                _onuBlo.ValidarPermiso(SEG_PERMISO.ALISTAS);

                string urlXML = _parametroONUSDNBlo.GetDatosDetalle("TIPO", "ONU", true)
                    .Select(x => new { x.URL_XML })
                    .FirstOrDefault().URL_XML;

                Thread hiloONUSave = new Thread(() => _onuBlo.SaveListaONU(urlXML));

                hiloONUSave.Start();


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
                _onuBlo.ValidarPermiso(SEG_PERMISO.ALISTAS);
                _onuBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetEstadoCargaDatos()
        {
            try
            {
                List<SEG_CARGAR_DATOS> list = new List<SEG_CARGAR_DATOS>();
                SEG_CARGAR_DATOS newEstado = new SEG_CARGAR_DATOS();

                newEstado = _cargaDatosBlo.GetById(SEG_CARGAR_DATOS.ID_ONU);
                newEstado.ESTADO = newEstado.ESTADO + ": CARGANDO " + _onuBlo.GetCount() + " DE " + newEstado.TOTAL_REGISTROS;

                list.Add(newEstado);
                var records = list.ToList();

                return Json(new { records}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| TABLAS HIJAS

        [HttpGet]
        public JsonResult GetONUAlias(int? page, int? limit, string searchString = null, int ID_LIS_ONU = 0)
        {
            try
            {
                int total = 0;
                var records = _onuAliasBlo.GetDatosDetalle(out total, page, limit, "ALIAS_NAME", "asc", "ID_LIS_ONU", ID_LIS_ONU);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpGet]
        public JsonResult GetONUDocumento(int? page, int? limit, string searchString = null, int ID_LIS_ONU = 0)
        {
            try
            {
                int total = 0;
                var records = _onuDocumentoBlo.GetDatosDetalle(out total, page, limit, "TYPE", "asc", "ID_LIS_ONU", ID_LIS_ONU);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpGet]
        public JsonResult GetONUDireccion(int? page, int? limit, string searchString = null, int ID_LIS_ONU = 0)
        {
            try
            {
                int total = 0;
                var records = _onuDireccionBlo.GetDatosDetalle(out total, page, limit, null, null, "ID_LIS_ONU", ID_LIS_ONU);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        #endregion

    }
}