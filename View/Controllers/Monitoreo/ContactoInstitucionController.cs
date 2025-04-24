using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Monitoreo;
using Blo.Properties;
using Blo.Listas;


namespace View.Controllers.Monitoreo
{
    [NoCache]
    [Autorizacion]
    public class ContactoInstitucionController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ICatInstitucionBlo _catInstitucionBlo;
        private readonly IContactoInstitucionBlo _contactoInstitucionBlo;
        private readonly ICargoInstitucionBlo _cargoInstitucionBlo;
        private readonly ICatUnidadInstitucionBlo _catUnidadInstitucionBlo;
        private readonly ICatTituloBlo _catTitulosBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public ContactoInstitucionController(IContactoInstitucionBlo contactoInstitucionBlo, ICargoInstitucionBlo cargoInstitucionBlo,
            ICatUnidadInstitucionBlo catUnidadInstitucionBlo, ICatInstitucionBlo catInstitucionBlo,ICatTituloBlo catTituloBlo)
        {
            _catInstitucionBlo = catInstitucionBlo;
            _contactoInstitucionBlo = contactoInstitucionBlo;
            _cargoInstitucionBlo = cargoInstitucionBlo;
            _catUnidadInstitucionBlo = catUnidadInstitucionBlo;
            _catTitulosBlo = catTituloBlo;
        }

        public ActionResult Index()
        {
            ViewBag.institucion = _catInstitucionBlo.GetAll().OrderBy(x => x.NOMBRE).ToList();
            ViewBag.titulo = _catTitulosBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();

            return View();
        }


        [HttpGet]
        public JsonResult GetContactoInstitucion(int? page, int? limit, string sortBy, string direction)
        {
            try
            {
                int total = 0;
                var records = _contactoInstitucionBlo.GetDatosGrid(out total, page, limit, sortBy, direction, true)
                             .Select(x => new
                             {
                                 x.ID,
                                 x.ID_TITULO,
                                 x.NOMBRE,
                                 x.MON_CAT_UNIDAD.ID_INSTITUCION,
                                 NOMBREINSTITUCION = x.MON_CAT_UNIDAD.MON_CAT_INSTITUCION.NOMBRE,
                                 x.ID_CARGO_INSTITUCION,
                                 NOMBRECARGOINST = x.MON_CARGO_INSTITUCION.NOMBRE,
                                 x.ID_UNIDAD,
                                 NOMBREUNIDAD = x.MON_CAT_UNIDAD.DESCRIPCION
                             });

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult Save(MON_CONTACTO_INSTITUCION data)
        {
            MON_CONTACTO_INSTITUCION contactoInstitucion = new MON_CONTACTO_INSTITUCION();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _contactoInstitucionBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    contactoInstitucion = _contactoInstitucionBlo.GetById(data.ID);

                contactoInstitucion.ID_TITULO = data.ID_TITULO;
                contactoInstitucion.ID_UNIDAD = data.ID_UNIDAD;
                contactoInstitucion.ID_CARGO_INSTITUCION = data.ID_CARGO_INSTITUCION;
                contactoInstitucion.NOMBRE = data.NOMBRE;

                _contactoInstitucionBlo.Save(contactoInstitucion);
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
                _contactoInstitucionBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _contactoInstitucionBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ObtenerUnidadesCargosPorInstitucion(int idInstitucion)
        {
            try
            {
                var unidades = _catUnidadInstitucionBlo.GetDatosDetalle("ID_INSTITUCION", idInstitucion)
                    .Select(x => new { x.ID, x.DESCRIPCION });

                var cargos = _cargoInstitucionBlo.GetDatosDetalle("ID_INSTITUCION", idInstitucion)
                    .Select(x => new { x.ID, x.NOMBRE });

                return Json(new { unidades, cargos }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener los datos");
            }
        }

    }
}