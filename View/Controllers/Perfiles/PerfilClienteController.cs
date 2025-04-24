using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Perfiles;
using Blo.Properties;
using System.Text;

namespace View.Controllers.Perfiles
{
    [NoCache]
    [Autorizacion]
    public class PerfilClienteController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly ITipoCalificacionBlo _tipoCalificacionBlo;
        private readonly IConfiguracionFactorBlo _configuracionFactorBlo;
        private readonly ICalificacionFactorBlo _calificacionFactorBlo;
        private readonly IFactorBlo _factorBlo;
        private readonly IHistorialPerfilBlo _histotialPerfilBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public PerfilClienteController(ITipoCalificacionBlo tipoCalificacionBlo, ICalificacionFactorBlo calificacionFactorBlo,
            IFactorBlo factorBlo, IConfiguracionFactorBlo configuracionFactorBlo, IHistorialPerfilBlo historialPerfilBlo)
        {
            _tipoCalificacionBlo = tipoCalificacionBlo;
            _configuracionFactorBlo = configuracionFactorBlo;
            _calificacionFactorBlo = calificacionFactorBlo;
            _factorBlo = factorBlo;
            _histotialPerfilBlo = historialPerfilBlo;
        }


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> CLIENTE

        public ActionResult Index()
        {

            return View();
        }


        [HttpGet]
        public JsonResult GetCliente(int? limit, int? page, string searchString = null)
        {
            try
            {
                int total = 0;
                int start = (page.Value - 1) * limit.Value;
                List<VIEW_CLIENTE> records = new List<VIEW_CLIENTE>();

                //Buscar
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    records = _SQLBDEntities.VIEW_CLIENTE.AsNoTracking()
                        .Where(x => x.CODIGO_CLIENTE.ToString().Contains(searchString) ||
                            (x.NOMBRES.Trim() + " " +
                            x.PRIMER_APELLIDO.Trim() + " " +
                            x.SEGUNDO_APELLIDO.Trim() + " " +
                            x.APELLIDO_DE_CASADA.Trim()).ToUpper().Contains(searchString.ToUpper()) ||
                            x.NUMERO_IDENTIFICACION_1.Equals(searchString) ||
                            x.NUMERO_IDENTIFICACION_2.Equals(searchString))
                       .OrderBy(x => x.NOMBRES)
                       .Skip(start)
                       .Take(limit.Value)
                       .ToList();

                    total = _SQLBDEntities.VIEW_CLIENTE.AsNoTracking()
                        .Where(x => x.CODIGO_CLIENTE.ToString().Contains(searchString) ||
                           (x.NOMBRES.Trim() + " " +
                            x.PRIMER_APELLIDO.Trim() + " " +
                            x.SEGUNDO_APELLIDO.Trim() + " " +
                            x.APELLIDO_DE_CASADA.Trim()).ToUpper().Contains(searchString.ToUpper()) ||
                            x.NUMERO_IDENTIFICACION_1.Equals(searchString) ||
                            x.NUMERO_IDENTIFICACION_2.Equals(searchString))
                        .Count();
                }

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> TELEFONOS

        [HttpPost]
        public ActionResult _Telefonos(long CODIGO_CLIENTE = 0)
        {
            ViewBag.CODIGO_CLIENTE = CODIGO_CLIENTE;

            return PartialView("_Telefonos");
        }


        [HttpGet]
        public JsonResult GetTelefonos(long CODIGO_CLIENTE = 0)
        {
            try
            {
                var records = _SQLBDEntities.VIEW_CLIENTE_TELEFONOS.AsNoTracking()
                              .Where(x => x.CODIGO_CLIENTE == CODIGO_CLIENTE)
                              .ToList();

                return Json(new { records }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> DIRECCIONES

        [HttpPost]
        public ActionResult _Direcciones(long CODIGO_CLIENTE = 0)
        {
            ViewBag.CODIGO_CLIENTE = CODIGO_CLIENTE;

            return PartialView("_Direcciones");
        }


        [HttpGet]
        public JsonResult GetDirecciones(long CODIGO_CLIENTE = 0)
        {
            try
            {
                var records = _SQLBDEntities.VIEW_CLIENTE_DIRECCIONES.AsNoTracking()
                              .Where(x => x.CODIGO_CLIENTE == CODIGO_CLIENTE)
                              .ToList();

                return Json(new { records }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> EMPLEOS

        [HttpPost]
        public ActionResult _Empleos(long CODIGO_CLIENTE = 0)
        {
            ViewBag.CODIGO_CLIENTE = CODIGO_CLIENTE;

            return PartialView("_Empleos");
        }


        [HttpGet]
        public JsonResult GetEmpleos(long CODIGO_CLIENTE = 0)
        {
            try
            {
                var records = _SQLBDEntities.VIEW_CLIENTE_EMPLEOS.AsNoTracking()
                              .Where(x => x.CODIGO_CLIENTE == CODIGO_CLIENTE)
                              .ToList();

                return Json(new { records }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> CALIFICACION

        private struct Calificacion
        {
            public string factor { get; set; }
            public int valor { get; set; }
        }

        [HttpPost]
        public ActionResult _Calificacion(long CODIGO_CLIENTE = 0)
        {
            ViewBag.CODIGO_CLIENTE = CODIGO_CLIENTE;

            return PartialView("_Calificacion");
        }


        [HttpGet]
        public JsonResult GetCalificacion(int? page, int? limit, string sortBy, string direction, string searchString = null, long CODIGO_CLIENTE = 0)
        {
            List<Calificacion> listCalificacion = new List<Calificacion>();
            try
            {

                var query = _calificacionFactorBlo.calificacionPorCodigoCLiente(CODIGO_CLIENTE);

                if (query == null || !query.Any())
                {
                    return Json(new { records = new List<Calificacion>() }, JsonRequestBehavior.AllowGet);
                }

                var factores = query.AsEnumerable()
                    .Select(x => new
                    {
                        x.PUNTAJE,
                        FactorDescripcion = x.PER_FACTOR != null ? x.PER_FACTOR.DESCRIPCION : "Sin Factor"
                    })
                    .ToList();

                foreach (var item in factores)
                {
                    Calificacion calificacion = new Calificacion();
                    calificacion.valor = item.PUNTAJE;
                    calificacion.factor = item.FactorDescripcion;
                    listCalificacion.Add(calificacion);
                }


                Calificacion calificacionTotal = new Calificacion();
                calificacionTotal.valor = listCalificacion.Sum(x => x.valor);
                int idTipoCalificacion = _tipoCalificacionBlo.ObtenerIdCalificacionXValor(listCalificacion.Sum(x => x.valor));
                calificacionTotal.factor = "CALIFICACION : " + _tipoCalificacionBlo.GetById(idTipoCalificacion).DESCRIPCION;
                listCalificacion.Add(calificacionTotal);


                return Json(new { records = listCalificacion }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpGet]
        public JsonResult ObtenerColor(decimal valor)
        {
            string color = "#ffffff";
            try
            {
                color = _tipoCalificacionBlo.ObtenerColorXValor(valor);

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return Json(new { color }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> HISTORIAL DE CALIFICACION 12 MESES


        [HttpPost]
        public JsonResult _HistorialCalificacion(long CODIGO_CLIENTE = 0)
        {
            List<int> listIdsCalificacion = new List<int>();
            List<string> listFechas = new List<string>();
            StringBuilder dataChart = new StringBuilder();
            DateTime fechaInicio = DateTime.Now.AddMonths(-14);
            DateTime fechaFin = DateTime.Now.AddMonths(-1);
            string data, xLabels, yLabels;

            var historialCalificacion = _histotialPerfilBlo.GetDatosDetalle("ID_CLIENTE", CODIGO_CLIENTE);

            do
            {
                var list = from x in historialCalificacion
                           where (x.FECHA.Month == fechaInicio.AddMonths(1).Month &&
                                  x.FECHA.Year == fechaInicio.AddMonths(1).Year)
                           select x.PUNTAJE;

                listIdsCalificacion.Add(_tipoCalificacionBlo.ObtenerIdCalificacionXValor(list.Sum()));
                fechaInicio = fechaInicio.AddMonths(1);
                listFechas.Add("'" + fechaInicio.ToString("MM-yyyy") + "'");


            } while (fechaInicio < fechaFin);


            dataChart.AppendLine("{");
            dataChart.AppendLine("label:'CALIFICACION',");
            dataChart.AppendLine("fill:false,");
            dataChart.AppendLine("backgroundColor:'#0A11DD',");
            dataChart.AppendLine("borderColor:'#0A11DD',");
            dataChart.AppendLine("data:[ " + string.Join(",",
                                 listIdsCalificacion.Select(x => new
                                 {
                                     calificacion = string.Join("", _tipoCalificacionBlo.GetAll()
                                                    .Where(z => z.ID == x)
                                                    .Select(z => new { DESCRIPCION = "'" + z.DESCRIPCION + "'" })
                                                    .Select(z => z.DESCRIPCION))
                                 }).Select(x => x.calificacion == "" ? "' '" : x.calificacion)) + "]");
            dataChart.AppendLine("},");


            data = dataChart.ToString();
            xLabels = string.Join(",", listFechas);
            yLabels = string.Join(",",
                             _tipoCalificacionBlo.GetAll()
                             .OrderByDescending(z => z.ID)
                             .Select(x => new { DESCRIPCION = "'" + x.DESCRIPCION.Trim() + "'" })
                             .Select(x => x.DESCRIPCION));

            return Json(new { data, xLabels, yLabels }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetHistorialCalificacion(int? page, int? limit, string sortBy, string direction, long CODIGO_CLIENTE = 0)
        {
            try
            {
                int total = 0;
                var records = _histotialPerfilBlo.GetDatosDetalle(out total, page, limit, sortBy, direction, "ID_CLIENTE", CODIGO_CLIENTE, true)
                    .Select(x => new
                    {
                        x.FECHA,
                        x.PER_FACTOR.DESCRIPCION,
                        x.PUNTAJE
                    }).AsQueryable();


                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }



        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> PERFIL TRANSACCIONAL 6 MESES

        public struct dataPorPerfilTransaccional
        {
            public int mes { get; set; }
            public int valor { get; set; }
        }


        [HttpPost]
        public JsonResult _PerfilTransaccional(long CODIGO_CLIENTE = 0)
        {
            List<double> alertatr = new List<double>();
            List<string> listFechastr = new List<string>();
            StringBuilder dataCharttr = new StringBuilder();
            DateTime fechaIniciotr = DateTime.Now.AddMonths(-7);
            DateTime fechaFintr = DateTime.Now.AddMonths(-1);

            do
            {
                fechaIniciotr = fechaIniciotr.AddMonths(1);

                int mes = fechaIniciotr.Month;
                int anio = fechaIniciotr.Year;
                double monto = 0;

                var per = _SQLBDEntities.PER_PERFIL_TRANSACCIONAL
                            .Where(x => x.CODIGO_CLIENTE == CODIGO_CLIENTE && x.MES == mes && x.ANIO == anio);

                if (per.Any())
                    monto = _SQLBDEntities.PER_PERFIL_TRANSACCIONAL
                            .Where(x => x.CODIGO_CLIENTE == CODIGO_CLIENTE && x.MES == mes && x.ANIO == anio)
                            .Sum(x => x.MONTO_MENSUAL == null ? 0 : x.MONTO_MENSUAL.Value);

                alertatr.Add(monto);
                listFechastr.Add("'" + fechaIniciotr.ToString("MM-yyyy") + "'");

            } while (fechaIniciotr < fechaFintr);


            dataCharttr.AppendLine("{");
            dataCharttr.AppendLine("label:'Valor Pagado $',");
            dataCharttr.AppendLine("fill:false,");
            dataCharttr.AppendLine("backgroundColor:'#0A11DD',");
            dataCharttr.AppendLine("borderColor:'#0A11DD',");
            dataCharttr.AppendLine("data:[ " + string.Join(",", alertatr.Select(x => x)) + "]");
            dataCharttr.AppendLine("},");


            return Json(new { data = dataCharttr.ToString(), listFechastr }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetPerfilTransaccional(int? page, int? limit, string sortBy, string direction, long CODIGO_CLIENTE = 0, int meses = 0)
        {
            try
            {

                List<SP_PERFILXCLIENTE_Result> records = new List<SP_PERFILXCLIENTE_Result>();
                // 6 MESES DEFAULT
                meses = 6;
                records = _SQLBDEntities.SP_PERFILXCLIENTE(CODIGO_CLIENTE, meses).ToList();

                return Json(new { records }, JsonRequestBehavior.AllowGet);
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