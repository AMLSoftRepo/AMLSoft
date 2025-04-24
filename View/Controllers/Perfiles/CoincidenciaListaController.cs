using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Perfiles;
using Blo.Properties;

namespace View.Controllers.Perfiles
{
    [NoCache]
    [Autorizacion]
    public class CoincidenciaListaController : BaseController
    {
        private readonly ICoincidenciaListaBlo _coincidenciaListaBlo;

        public CoincidenciaListaController(ICoincidenciaListaBlo coincidenciaListaBlo)
        {
            _coincidenciaListaBlo = coincidenciaListaBlo;
        }

        // GET: PaisGrupo
        public ActionResult Index()
        {
            ViewBag.coincidencia = _coincidenciaListaBlo.GetAll();

            return View();
        }
        [HttpGet]
        public JsonResult GetCoincidencia(int? page, int? limit, string sortBy, string direction, string searchString = null, long ID_COINCIDENCIA = 0)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;

                var records = _coincidenciaListaBlo.GetAll(true)
                    .Select(e => new
                    {
                        e.ID,
                        e.ID_CLIENTE,
                        e.ID_LISTA,
                        e.ID_PERSONA,
                        e.FECHA_ALERTA,
                        e.SEGUIMIENTO,
                        e.FECHA_ACTUALIZACION,
                        e.USUARIO_ACTUALIZA
                    })
                    .AsQueryable();
                total = records.Count();
                records = SortHelper.OrdenarGrid(records, sortBy, direction).Skip(start).Take(limit.Value);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo los datos");

            }
        }

        [HttpPost]
        public JsonResult Save(PER_COINCIDENCIA_LISTA data)
        {
            PER_COINCIDENCIA_LISTA coincidencia = new PER_COINCIDENCIA_LISTA();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _coincidenciaListaBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    coincidencia = _coincidenciaListaBlo.GetById(data.ID);
                coincidencia.ID_CLIENTE = data.ID_CLIENTE;
                coincidencia.ID_LISTA = data.ID_LISTA;
                coincidencia.ID_PERSONA = data.ID_PERSONA;
                coincidencia.FECHA_ALERTA = data.FECHA_ALERTA;
                coincidencia.SEGUIMIENTO = data.SEGUIMIENTO;
                coincidencia.FECHA_ACTUALIZACION = data.FECHA_ACTUALIZACION;
                coincidencia.USUARIO_ACTUALIZA = data.USUARIO_ACTUALIZA;

                _coincidenciaListaBlo.Save(coincidencia);
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
                _coincidenciaListaBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _coincidenciaListaBlo.Remove(id);
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