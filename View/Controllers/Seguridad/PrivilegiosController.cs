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
    public class PrivilegiosController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IAccesoUsuarioBlo _accesoUsuarioBlo;
        private readonly IModuloBlo _moduloBlo;
        private readonly IRolBlo _rolBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public PrivilegiosController(IAccesoUsuarioBlo accesoUsuarioBlo, IModuloBlo moduloBlo, IRolBlo rolBlo)
        {
            _accesoUsuarioBlo = accesoUsuarioBlo;
            _moduloBlo = moduloBlo;
            _rolBlo = rolBlo;
        }

        public ActionResult Index()
        {
            getRol();

            return View();
        }


        [HttpGet]
        public JsonResult GetModulos()
        {
            try
            {
                var records = _moduloBlo.GetAll();
                return Json(new { records }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo modulos");
            }
        }


        [HttpGet]
        public JsonResult GetOpcionesDenegadas(int modulo, int perfil)
        {
            try
            {
                var records = _accesoUsuarioBlo.GetOpcionesxPerfil(modulo, perfil, false)
                            .Select(x => new
                            {
                                x.ID,
                                x.ID_MODULO,
                                x.ID_OPCION,
                                x.ID_ROL,
                                DESCRIPCION = x.SEG_OPCION.URL.Trim() == "#" ? "[*] " + x.SEG_OPCION.DESCRIPCION : x.SEG_OPCION.DESCRIPCION,
                                ORDENAR_DESC = x.SEG_OPCION.DESCRIPCION
                            }).OrderBy(x => x.ORDENAR_DESC);

                return Json(new { records }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo opciones denegadas");
            }
        }

        [HttpGet]
        public JsonResult GetOpcionesOtorgadas(int modulo, int perfil)
        {
            try
            {
                var records = _accesoUsuarioBlo.GetOpcionesxPerfil(modulo, perfil, true)
                            .Select(x => new
                            {
                                x.ID,
                                x.ID_MODULO,
                                x.ID_OPCION,
                                x.ID_ROL,
                                DESCRIPCION = x.SEG_OPCION.URL.Trim() == "#" ? "[*] " + x.SEG_OPCION.DESCRIPCION : x.SEG_OPCION.DESCRIPCION,
                                ORDENAR_DESC =x.SEG_OPCION.DESCRIPCION
                            }).OrderBy(x=>x.ORDENAR_DESC);

                return Json(new { records }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo opciones otorgadas");
            }
        }

        [HttpPost]
        public JsonResult GuardarOpciones(SEG_ACCESO_USUARIO data, string[] opcionAcceso)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _accesoUsuarioBlo.ValidarPermiso(SEG_PERMISO.APRIVILEGIOS);

                if (opcionAcceso != null)
                    foreach (string item in opcionAcceso)
                    {
                        SEG_ACCESO_USUARIO segAccesoUsuario = new SEG_ACCESO_USUARIO();
                        data.ID = int.Parse(item.Split('|')[1]);

                        if (data.ID != 0)
                            segAccesoUsuario = _accesoUsuarioBlo.GetById(data.ID);

                        segAccesoUsuario.ID_OPCION = int.Parse(item.Split('|')[0]);
                        segAccesoUsuario.ACCESO = data.ACCESO;
                        segAccesoUsuario.ID_MODULO = data.ID_MODULO;
                        segAccesoUsuario.ID_ROL = data.ID_ROL;

                        _accesoUsuarioBlo.Save(segAccesoUsuario);
                    }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        public void getRol()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Seleccione ...", Value = "0" });

            try
            {
                foreach (var c in _rolBlo.GetAll())
                {
                    items.Add(new SelectListItem { Text = c.NOMBRE, Value = c.ID.ToString() });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            SelectList Lista = new SelectList(items, "Value", "Text", "0");
            ViewBag.roles = Lista;
        }

    }
}