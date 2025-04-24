using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Listas;
using Blo.Properties;
using Microsoft.Reporting.WebForms;
using System.Data;

namespace View.Controllers.Listas
{
    [NoCache]
    [Autorizacion]
    public class PaisGrupoController : BaseController
    {
        private readonly IPaisGrupoBlo _paisGrupoBlo;
        private readonly ICatGrupoBlo _catGrupoBlo;

        public PaisGrupoController(IPaisGrupoBlo paisGrupoBlo, ICatGrupoBlo catGrupoBlo)
        {
            _paisGrupoBlo = paisGrupoBlo;
            _catGrupoBlo = catGrupoBlo;
        }

        // GET: PaisGrupo
        public ActionResult Index()
        {
            ViewBag.grupo = _catGrupoBlo.GetAll();
            ViewBag.pais = _SQLBDEntities.VIEW_PAISNACIONALIDAD.AsNoTracking().ToList();

            return View();
        }
        [HttpGet]
        public JsonResult GetPaisGrupo(int? page, int? limit, string sortBy, string direction, int idGrupo = 0)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;

                var records = _paisGrupoBlo.GetAll(true)
                    .Where(x => x.ID_GRUPO == (idGrupo == 0 ? x.ID_GRUPO : idGrupo))
                    .Select(e => new
                    {
                        e.ID,
                        e.ID_GRUPO,
                        e.LIS_CAT_GRUPO_FATF.NOMBRE,
                        e.ID_PAIS,
                        PAIS = _SQLBDEntities.VIEW_PAISNACIONALIDAD.Where(y => y.CODIGO_PAIS == e.ID_PAIS).Select(y => y.NOMBRE),
                        e.MOTIVO_INGRESO
                    }).AsQueryable();


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
        public JsonResult Save(LIS_PAIS_GRUPO data)
        {
            LIS_PAIS_GRUPO PaisGrupo = new LIS_PAIS_GRUPO();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _paisGrupoBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    PaisGrupo = _paisGrupoBlo.GetById(data.ID);
                PaisGrupo.ID_GRUPO = data.ID_GRUPO;
                PaisGrupo.ID_PAIS = data.ID_PAIS;
                PaisGrupo.MOTIVO_INGRESO = data.MOTIVO_INGRESO;

                _paisGrupoBlo.Save(PaisGrupo);
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
                _paisGrupoBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _paisGrupoBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Reporte(string formato)
        {
            try
            {
                string nombreReporte = "RptListasPaises";
                string nombreTabla = "LIS_PAIS_GRUPO";

                Dictionary<string, object> parametros = new Dictionary<string, object>();

                var datosReporte = _paisGrupoBlo.GetAll(true)
                   .Select(e => new
                   {
                       GRUPO = e.LIS_CAT_GRUPO_FATF?.NOMBRE ?? "DESCONOCIDO",
                       PAIS = _SQLBDEntities.VIEW_PAISNACIONALIDAD
                        .Where(y => y.CODIGO_PAIS == e.ID_PAIS)
                        .Select(y => y.NOMBRE)
                        .FirstOrDefault() ?? "DESCONOCIDO",
                       MOTIVO = string.IsNullOrEmpty(e.MOTIVO_INGRESO) ? "DESCONOCIDO" : e.MOTIVO_INGRESO
                   }).ToList();

                if (datosReporte == null || !datosReporte.Any())
                {
                    return Content("No hay datos disponibles para el reporte.");
                }

                DataTable dtReporte = DataTableHelper.ToDataTable(datosReporte);

                // Llamar al método que genera el reporte con Crystal Reports
                VerReporte(nombreReporte, formato, parametros, dtReporte, nombreTabla);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new HttpException(404, null);
            }
        }
        
    }
}