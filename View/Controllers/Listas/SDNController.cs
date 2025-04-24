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
    public class SDNController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ISDNBlo _sdnBlo;
        private readonly IParametroONUSDNBlo _parametroONUSDNBlo;
        private readonly ISDNAkaBlo _sdnAkaBlo;
        private readonly ISDNDocumentoBlo _sdnDocumentoBlo;
        private readonly ISDNDireccionBlo _sdnDireccionBlo;
        private readonly ISDNNacionalidadBlo _sdnNacionalidadBlo;
        private readonly ICargaDatosBlo _cargaDatosBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public SDNController(ISDNBlo sdnBlo, IParametroONUSDNBlo parametroONUSDNBlo, ISDNAkaBlo sdnAkaBlo, ISDNDocumentoBlo sdnDocumentoBlo,
            ISDNDireccionBlo sdnDireccionBlo, ISDNNacionalidadBlo sdnNacionalidadBlo, ICargaDatosBlo cargaDatosBlo)
        {
            _sdnBlo = sdnBlo;
            _parametroONUSDNBlo = parametroONUSDNBlo;
            _sdnAkaBlo = sdnAkaBlo;
            _sdnDocumentoBlo = sdnDocumentoBlo;
            _sdnDireccionBlo = sdnDireccionBlo;
            _sdnNacionalidadBlo = sdnNacionalidadBlo;
            _cargaDatosBlo = cargaDatosBlo;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetSDN(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _sdnBlo.GetAll()
                    .Select(x => new
                    {
                        x.ID,
                        x.UID,
                        x.TIPO,
                        NOMBRE = x.NOMBRES + " " + x.APELLIDOS,
                        x.PROGRAMAS
                    }).AsQueryable();

                //Buscar texto en varios campos 
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    records = records
                              .Where(x => (
                                  x.TIPO + " " +
                                  x.NOMBRE).ToUpper().Contains(searchString.Trim().ToUpper())
                               )
                              .Select(x => new
                              {
                                  x.ID,
                                  x.UID,
                                  x.TIPO,
                                  x.NOMBRE,
                                  x.PROGRAMAS
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
                _sdnBlo.ValidarPermiso(SEG_PERMISO.ALISTAS);

                string urlXML = _parametroONUSDNBlo.GetDatosDetalle("TIPO", "SDN", true)
                    .Select(x => new { x.URL_XML })
                    .FirstOrDefault().URL_XML;

                Thread hiloSDNSave = new Thread(() => _sdnBlo.SaveListaSDN(urlXML));

                hiloSDNSave.Start();
                //hiloSDNSave.Join(); 
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
                _sdnBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _sdnBlo.Remove(id);
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

                newEstado = _cargaDatosBlo.GetById(SEG_CARGAR_DATOS.ID_SDN);
                newEstado.ESTADO = newEstado.ESTADO + ": CARGANDO " + _sdnBlo.GetCount() + " DE " + newEstado.TOTAL_REGISTROS;

                list.Add(newEstado);
                var records = list.ToList();

                return Json(new { records }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| TABLAS HIJAS

        [HttpGet]
        public JsonResult GetSDNAka(int? page, int? limit, string searchString = null, int ID_LIS_SDN = 0)
        {
            try
            {
                int total = 0;
                var records = _sdnAkaBlo.GetDatosDetalle(out total, page, limit, "NOMBRES", "asc", "ID_LIS_SDN", ID_LIS_SDN)
                    .Select(x => new
                    {
                        x.ID,
                        x.ID_LIS_SDN,
                        x.CATEGORIA,
                        ALIAS = x.NOMBRES + " " + x.APELLIDOS,
                        x.TIPO
                    }).ToList();

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpGet]
        public JsonResult GetSDNDocumento(int? page, int? limit, string searchString = null, int ID_LIS_SDN = 0)
        {
            try
            {
                int total = 0;
                var records = _sdnDocumentoBlo.GetDatosDetalle(out total, page, limit, "PAIS", "asc", "ID_LIS_SDN", ID_LIS_SDN).ToList();

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpGet]
        public JsonResult GetSDNDireccion(int? page, int? limit, string searchString = null, int ID_LIS_SDN = 0)
        {
            try
            {
                int total = 0;
                var records = _sdnDireccionBlo.GetDatosDetalle(out total, page, limit, null, null, "ID_LIS_SDN", ID_LIS_SDN).ToList();

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpGet]
        public JsonResult GetSDNNacionalidad(int? page, int? limit, string searchString = null, int ID_LIS_SDN = 0)
        {
            try
            {
                int total = 0;
                var records = _sdnNacionalidadBlo.GetDatosDetalle(out total, page, limit, "PAIS", "asc", "ID_LIS_SDN", ID_LIS_SDN).ToList();

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