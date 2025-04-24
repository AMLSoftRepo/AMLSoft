using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Seguridad;
using Blo.Properties;
using hbehr.AdAuthentication;
using System.Configuration;


namespace View.Controllers.Seguridad
{
    [NoCache]
    [Autorizacion]
    public class RolUsuarioController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IRolUsuarioBlo _rolUsuarioBlo;
        private readonly IRolBlo _rolBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RolUsuarioController(IRolUsuarioBlo rolUsuarioBlo, IRolBlo rolBlo)
        {
            _rolUsuarioBlo = rolUsuarioBlo;
            _rolBlo = rolBlo;
        }


        public ActionResult Index()
        {
            ViewBag.roles = _rolBlo.GetAll();
            //ViewBag.usuarios = GetAllUsers();

            return View();
        }



        [Autorizacion]
        private List<AdUser> GetAllUsers()
        {
            List<AdUser> adUser = new List<AdUser>();

            try
            {
                //Autenticacion Acitive Directory
                AdAuthenticator adAuthenticator = new AdAuthenticator();
                //Obtiene la Direccion LDAP para obtener los usuarios de Active Directory
                string ldapPath = ConfigurationManager.AppSettings["ldapPath"].ToString();
                //Obtiene el nombre de Dominio
                string ldapDomain = ConfigurationManager.AppSettings["ldapDomain"].ToString();


                adAuthenticator.ConfigureSetLdapPath(ldapPath).ConfigureLdapDomain(ldapDomain);
                adUser = adAuthenticator.GetAllUsers().ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return adUser;
        }


        [HttpGet]
        public JsonResult GetRolUsuarios(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;
                var records = _rolUsuarioBlo.GetDatosGrid(out total, page, limit, sortBy, direction,true)
                             .Select(x => new
                             {
                                 x.ID,
                                 x.ID_ROL,
                                 x.USUARIO,
                                 NOMBREROL = x.SEG_ROL.NOMBRE
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
        public JsonResult Save(SEG_ROL_USUARIO data)
        {
            SEG_ROL_USUARIO segRol = new SEG_ROL_USUARIO();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _rolUsuarioBlo.ValidarSave(data.ID);

                segRol.USUARIO = data.USUARIO;

                if (data.ID != 0)
                    segRol = _rolUsuarioBlo.GetById(data.ID);

                segRol.ID_ROL = data.ID_ROL;

                _rolUsuarioBlo.Save(segRol);
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
                _rolUsuarioBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _rolUsuarioBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        [Autorizacion]
        [HttpPost]
        public JsonResult ValidaUsuario(string usuario)
        {
            bool valid = true;
            string message = "";

            if (_rolUsuarioBlo.GetRolUsuario(usuario).Any())
            {
                valid = false;
                message = "El usuario ya existe";
            }

            return valid ? Json(new { valid }, JsonRequestBehavior.AllowGet)
                         : Json(new { valid, message }, JsonRequestBehavior.AllowGet);
        }


    }
}