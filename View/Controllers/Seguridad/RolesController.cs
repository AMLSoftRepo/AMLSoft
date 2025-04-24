using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Seguridad;
using Blo.Properties;


namespace View.Controllers.Seguridad
{
    [NoCache]
    [Autorizacion]
    public class RolesController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IRolBlo _rolBlo;
        private readonly IPermisoBlo _permisosBlo;
        private readonly IRolPermisoBlo _rolPermisoBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RolesController(IRolBlo rolBlo, IPermisoBlo permisoBlo, IRolPermisoBlo rolPermisoBlo)
        {
            _rolBlo = rolBlo;
            _permisosBlo = permisoBlo;
            _rolPermisoBlo = rolPermisoBlo;

        }

        public ActionResult Index()
        {
            ViewBag.permiso = _permisosBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();

            return View();
        }


        [HttpGet]
        public JsonResult GetRoles(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                var listaPermisos = _rolPermisoBlo.GetAll(true).Select(x => new
                {
                    x.ID_PERMISO,
                    x.ID_ROL,
                    PERMISO = x.SEG_PERMISO.DESCRIPCION
                });

                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _rolBlo.GetAll()
                              .Select(x => new
                              {
                                  x.ID,
                                  x.NOMBRE,
                                  x.DESCRIPCION,
                                  x.ALERTA_NOTIFICADA,
                                  x.ALERTA_ANALIZADA,
                                  x.ALERTA_OFICIO,
                                  x.ALERTA_PERSONA_LISTA,
                                  x.ALERTA_PERFIL_TRANSACCIONAL,
                                  ID_PERMISO = listaPermisos.Where(z => z.ID_ROL == x.ID).Select(z => z.ID_PERMISO),
                                  PERMISOS = string.Join(", ", listaPermisos.Where(z => z.ID_ROL == x.ID).Select(z => z.PERMISO))
                              })
                              .AsQueryable();

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
        public JsonResult Save(SEG_ROL data, int[] ID_PERMISO)
        {
            SEG_ROL segRol = new SEG_ROL();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _rolBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    segRol = _rolBlo.GetById(data.ID);

                segRol.DESCRIPCION = data.DESCRIPCION;
                segRol.NOMBRE = data.NOMBRE;
                segRol.ALERTA_NOTIFICADA = data.ALERTA_NOTIFICADA;
                segRol.ALERTA_ANALIZADA = data.ALERTA_ANALIZADA;
                segRol.ALERTA_OFICIO = data.ALERTA_OFICIO;
                segRol.ALERTA_PERSONA_LISTA = data.ALERTA_PERSONA_LISTA;
                segRol.ALERTA_PERFIL_TRANSACCIONAL = data.ALERTA_PERFIL_TRANSACCIONAL;

                _rolBlo.SaveRoles(segRol, ID_PERMISO);
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
                _rolBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _rolBlo.Remove(id);
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