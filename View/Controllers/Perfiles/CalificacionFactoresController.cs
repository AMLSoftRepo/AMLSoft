using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Properties;
using Blo.Perfiles;

namespace View.Controllers.Perfiles
{
    [NoCache]
    [Autorizacion]
    public class CalificacionFactoresController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IConfiguracionFactorBlo _configuracionFactorBlo;
        private readonly ICalificacionFactorBlo _calificacionFactorBlo;
        private readonly ITipoCalificacionBlo _tipoCalificacionBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public CalificacionFactoresController(IConfiguracionFactorBlo configuracionFactorBlo, ICalificacionFactorBlo calificacionFactorBlo,
        ITipoCalificacionBlo tipoCalificacionBlo)
        {
            _configuracionFactorBlo = configuracionFactorBlo;
            _calificacionFactorBlo = calificacionFactorBlo;
            _tipoCalificacionBlo = tipoCalificacionBlo;
        }


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> CALIFICACION DE FACTORES

        public ActionResult Index()
        {
            //ViewBag.valorMin = _tipoCalificacionBlo.GetAll().Min(x => x.VALORMIN);
            //ViewBag.valorMax = _tipoCalificacionBlo.GetAll().Max(x => x.VALORMAX);
            ViewBag.valorMin = 1;
            ViewBag.valorMax = 5;

            return View();
        }


        [HttpGet]
        public JsonResult GetFactores(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;

                var confFactor = _configuracionFactorBlo.GetAll();
                var calificacionFactor = _calificacionFactorBlo.GetAll();
                var factor = _SQLBDEntities.VIEW_FACTORES_PERFIL.ToList();


                //left join
                var records = (from f in factor
                               join cf in calificacionFactor
                               on f.CODIGO
                               equals cf.ID_ITEM into totalFac
                               from totalFacNew in totalFac.DefaultIfEmpty()
                               select new
                               {
                                   ID = totalFacNew == null ? 0 : totalFacNew.ID,
                                   ID_FACTOR = f.CODIGO.Split('-')[0],
                                   f.FACTOR,
                                   f.CODIGO,
                                   f.DESCRIPCION,
                                   VALOR = totalFacNew == null ? confFactor.Where(x => x.ID_FACTOR.ToString() == f.CODIGO.Split('-')[0]).First().VALOR : totalFacNew.PUNTAJE
                               }).AsQueryable();

                //Buscar
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    records = records.Where(x => (
                                  x.CODIGO.Trim() + " " +
                                  x.FACTOR.Trim() + " " +
                                  x.DESCRIPCION.Trim() 
                                  ).ToUpper().Contains(searchString.Trim().ToUpper())
                               ).AsQueryable();
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
        public JsonResult Save(PER_CALIFICACION_FACTOR data)
        {
            PER_CALIFICACION_FACTOR calificacion = new PER_CALIFICACION_FACTOR();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _calificacionFactorBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    calificacion = _calificacionFactorBlo.GetById(data.ID);

                calificacion.ID_FACTOR = data.ID_FACTOR;
                calificacion.ID_ITEM = data.ID_ITEM;
                calificacion.PUNTAJE = data.PUNTAJE;

                _calificacionFactorBlo.Save(calificacion);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> IMPLEMENTACION DE PANEL DE CONFIGURACION

        /// <summary>
        ///ID	DESCRIPCION
        ///1	TIPO DE CLIENTE
        ///2	ACTIVIDAD ECONOMICA
        ///3	SECTOR ECONOMICO
        ///4	PROFESION
        ///5	GEOGRAFICO
        /// </summary>
        public struct DatosConfi
        {
            public int tipoCliente { get; set; }
            public int actividadEconomica { get; set; }
            public int sectorEconomico { get; set; }
            public int profesion { get; set; }
            public int geografico { get; set; }
        }


        [HttpPost]
        public JsonResult AplicarConfiguracion(DatosConfi data)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _configuracionFactorBlo.ValidarPermiso(SEG_PERMISO.ACONFGFACTORES);

                for (int i = 1; i <= 5; i++)
                {
                    PER_CONFIGURACION_FACTOR conf = new PER_CONFIGURACION_FACTOR();

                    conf = _configuracionFactorBlo.GetById(i);

                    if (i == 1) conf.VALOR = data.tipoCliente;
                    if (i == 2) conf.VALOR = data.actividadEconomica;
                    if (i == 3) conf.VALOR = data.sectorEconomico;
                    if (i == 4) conf.VALOR = data.profesion;
                    if (i == 5) conf.VALOR = data.geografico;

                    _configuracionFactorBlo.Save(conf);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ObtenerDatosConfi()
        {
            try
            {
                DatosConfi conf = new DatosConfi();

                foreach (var item in _configuracionFactorBlo.GetAll())
                {
                    if (item.ID_FACTOR == 1) conf.tipoCliente = item.VALOR;
                    if (item.ID_FACTOR == 2) conf.actividadEconomica = item.VALOR;
                    if (item.ID_FACTOR == 3) conf.sectorEconomico = item.VALOR;
                    if (item.ID_FACTOR == 4) conf.profesion = item.VALOR;
                    if (item.ID_FACTOR == 5) conf.geografico = item.VALOR;
                }

                return Json(new { conf }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener los datos");
            }
        }

        #endregion


    }
}