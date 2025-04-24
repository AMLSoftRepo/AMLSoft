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
    public class PEPController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IPEPBlo _pepBlo;
        private readonly IPEPCargoBlo _pepCargoBlo;
        private readonly IPEPRelacionBlo _pepRelacionBlo;
        private readonly ICatTituloBlo _catTituloBlo;
        private readonly ICatOrganoBlo _catOrganoBlo;
        private readonly ICatEntidadBlo _catEntidadBlo;
        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public PEPController(IPEPBlo pepBlo, IPEPCargoBlo pepCargoBlo, IPEPRelacionBlo pepRelacionBlo, ICatTituloBlo catTituloBlo, ICatOrganoBlo catOrganoBlo, ICatEntidadBlo catEntidadBlo)
        {
            _pepBlo = pepBlo;
            _pepCargoBlo = pepCargoBlo;
            _pepRelacionBlo = pepRelacionBlo;
            _catTituloBlo = catTituloBlo;
            _catOrganoBlo = catOrganoBlo;
            _catEntidadBlo = catEntidadBlo;
        }

        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> LISTA PEP
        public ActionResult Index()
        {
            ViewBag.titulo = _catTituloBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();


            return View();
        }

        [HttpGet]
        public JsonResult GetPEP(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total =0;
                var records = _pepBlo.GetPEP(out total, page, limit, sortBy, direction, searchString);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpPost]
        public JsonResult Save(LIS_PEP data)
        {
            LIS_PEP pep = new LIS_PEP();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _pepBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    pep = _pepBlo.GetById(data.ID);


                pep.TITULO_CARGO_PEP = data.TITULO_CARGO_PEP;
                pep.PRIMER_NOMBRE = data.PRIMER_NOMBRE;
                pep.SEGUNDO_NOMBRE = data.SEGUNDO_NOMBRE;
                pep.PRIMER_APELLIDO = data.PRIMER_APELLIDO;
                pep.SEGUNDO_APELLIDO = data.SEGUNDO_APELLIDO;
                pep.APELLIDO_CASADA = data.APELLIDO_CASADA;
                pep.CONOCIDO_POR = data.CONOCIDO_POR;
                pep.DUI = data.DUI;
                pep.NIT = data.NIT;
                pep.PASAPORTE = data.PASAPORTE;
                pep.CARNET_RESIDENTE = data.CARNET_RESIDENTE;
                pep.FUNCIONARIO_O_RELACION = data.FUNCIONARIO_O_RELACION;
                pep.DECLARACION_JURADA = data.DECLARACION_JURADA;

                _pepBlo.Save(pep);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje, pep.ID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Remove(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _pepBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _pepBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> CARGOS

        public ActionResult _Cargo(int ID_LIS_PEP = 0)
        {
            ViewBag.ID_LIS_PEP = ID_LIS_PEP;
            ViewBag.organo = _catOrganoBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            ViewBag.entidad = _catEntidadBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();
            return PartialView("_Cargo");
        }

        [HttpGet]
        public JsonResult GetPEPCargo(int? page, int? limit, string searchString = null, int ID_LIS_PEP = 0)
        {
            try
            {
                int total = 0;

                var records = _pepCargoBlo.GetDatosDetalle(out total, page, limit, null, null, "ID_LIS_PEP", ID_LIS_PEP,true)
                    .Select(y => new
                    {
                        y.ID,
                        y.ID_ORGANO,
                        ORGANO = y.LIS_CAT_ORGANOS.DESCRIPCION,
                        y.ID_ENTIDAD,
                        ENTIDAD = y.LIS_CAT_ENTIDADES.DESCRIPCION,
                        y.NOMBRE_CARGO,
                        y.FECHA_INICIO,
                        y.FECHA_FIN
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
        public JsonResult SaveCargo(LIS_PEP_CARGO data)
        {
            LIS_PEP_CARGO pepCargo = new LIS_PEP_CARGO();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _pepCargoBlo.ValidarSave(data.ID);
                pepCargo.ID_LIS_PEP = data.ID_LIS_PEP;

                if (data.ID != 0)
                    pepCargo = _pepCargoBlo.GetById(data.ID);

                pepCargo.ID_ORGANO = data.ID_ORGANO;
                pepCargo.ID_ENTIDAD = data.ID_ENTIDAD;
                pepCargo.NOMBRE_CARGO = data.NOMBRE_CARGO;
                pepCargo.FECHA_INICIO = data.FECHA_INICIO;
                pepCargo.FECHA_FIN = data.FECHA_FIN;

                _pepCargoBlo.Save(pepCargo);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveCargo(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _pepCargoBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _pepCargoBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> RELACIONES

        public ActionResult _Relacion(int ID_LIS_PEP = 0)
        {
            ViewBag.ID_LIS_PEP = ID_LIS_PEP;
            return PartialView("_Relacion");
        }

        [HttpGet]
        public JsonResult GetPEPRelacion(int? page, int? limit, string searchString = null, int ID_LIS_PEP = 0)
        {
            try
            {
                int total = 0;

                var records = _pepRelacionBlo.GetDatosDetalle(out total, page, limit, "NOMBRE_COMPLETO","asc", "ID_LIS_PEP", ID_LIS_PEP);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpPost]
        public JsonResult SaveRelacion(LIS_PEP_RELACION data)
        {
            LIS_PEP_RELACION pepRelacion = new LIS_PEP_RELACION();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _pepRelacionBlo.ValidarSave(data.ID);
                pepRelacion.ID_LIS_PEP = data.ID_LIS_PEP;

                if (data.ID != 0)
                    pepRelacion = _pepRelacionBlo.GetById(data.ID);

                pepRelacion.NOMBRE_COMPLETO = data.NOMBRE_COMPLETO;
                pepRelacion.GRADO_PARENTESCO = data.GRADO_PARENTESCO;
                pepRelacion.DESCRIPCION_RELACION = data.DESCRIPCION_RELACION;

                _pepRelacionBlo.Save(pepRelacion);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveRelacion(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _pepRelacionBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _pepRelacionBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}