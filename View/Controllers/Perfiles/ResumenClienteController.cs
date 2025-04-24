using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Reportes;
using Blo.Alertas;
using Blo.Listas;
using Blo.Monitoreo;
using Blo.Properties;
using Blo.Perfiles;
using Microsoft.Reporting.WebForms;
using System.Data;

namespace View.Controllers.Reportes
{
    [NoCache]
    [Autorizacion]
    public class ResumenClienteController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IReportesBlo _reportesBlo;
        private readonly IAlertaBlo _alertaBlo;
        private readonly IClientePEPBlo _clientePEPBlo;
        private readonly IPEPBlo _PEPBlo;
        private readonly ICalificacionFactorBlo _calificacionFactorBlo;
        private readonly ITipoCalificacionBlo _tipoCalificacionBlo;
        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public ResumenClienteController(IReportesBlo reportesBlo, IAlertaBlo alertaBlo, IClientePEPBlo clientePEPBlo, 
            IPEPBlo PEPBlo, ICalificacionFactorBlo calificacionFactorBlo,ITipoCalificacionBlo tipoCalificacionBlo)
        {
            _reportesBlo = reportesBlo;
            _alertaBlo = alertaBlo;
            _clientePEPBlo = clientePEPBlo;
            _PEPBlo = PEPBlo;
            _calificacionFactorBlo = calificacionFactorBlo;
            _tipoCalificacionBlo = tipoCalificacionBlo;
        }

        // GET: ResumenCliente
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// METODO PARA OBTENER LOS CLIENTES MEDIENTE UNA BUSQUEDA
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="sortBy"></param>
        /// <param name="direction"></param>
        /// <param name="nombre"></param>
        /// <returns>DEVUELE LA LISTA DE CLIENNTES QUE COINCIDAN CON LA BUSQUEDA</returns>
        [HttpGet]
        public JsonResult GetCliente(int? limit, int? page, string nombre)
        {
            try
            {
                int total = 0;
                int start = (page.Value - 1) * limit.Value;
                List<VIEW_CLIENTE> objeto = new List<VIEW_CLIENTE>();

                //Buscar
                if (!string.IsNullOrWhiteSpace(nombre))
                {
                    objeto = _SQLBDEntities.VIEW_CLIENTE.AsNoTracking()
                        .Where(x => x.CODIGO_CLIENTE.ToString().Contains(nombre) ||
                            (x.NOMBRES.Trim() + " " +
                            x.PRIMER_APELLIDO.Trim() + " " +
                            x.SEGUNDO_APELLIDO.Trim() + " " +
                            x.APELLIDO_DE_CASADA.Trim()).ToUpper().Contains(nombre.ToUpper()) ||
                            x.NUMERO_IDENTIFICACION_1.Equals(nombre) ||
                            x.NUMERO_IDENTIFICACION_2.Equals(nombre))
                       .OrderBy(x => x.NOMBRES)
                       .Skip(start)
                       .Take(limit.Value)
                       .ToList();

                    total = _SQLBDEntities.VIEW_CLIENTE.AsNoTracking()
                        .Where(x => x.CODIGO_CLIENTE.ToString().Contains(nombre) ||
                           (x.NOMBRES.Trim() + " " +
                            x.PRIMER_APELLIDO.Trim() + " " +
                            x.SEGUNDO_APELLIDO.Trim() + " " +
                            x.APELLIDO_DE_CASADA.Trim()).ToUpper().Contains(nombre.ToUpper()) ||
                            x.NUMERO_IDENTIFICACION_1.Equals(nombre) ||
                            x.NUMERO_IDENTIFICACION_2.Equals(nombre))
                        .Count();

                }

                var records = objeto
                    .Select(x => new
                    {
                        x.CODIGO_CLIENTE,
                        x.NOMBRES,
                        x.PRIMER_APELLIDO,
                        x.SEGUNDO_APELLIDO,
                        x.APELLIDO_DE_CASADA,
                        IDENTIFICACION1 = x.TIPO_IDENTIFICACION_1 + ": " + x.NUMERO_IDENTIFICACION_1,
                        IDENTIFICACION2 = x.TIPO_IDENTIFICACION_2 + ": " + x.NUMERO_IDENTIFICACION_2,
                        x.PAIS,
                        x.TIPO_DE_PERSONA,
                        x.ESTADO_CIVIL,
                        x.TIPO_CLIENTE,
                        x.SECTOR_ECONOMICO,
                        x.ACTIVIDAD_ECONOMICA,
                        x.CLASE_ACTIVIDAD_ECONOMICA,
                        x.SUB_ACTIVIDAD_ECONOMICA,
                        x.PROFESION,
                        x.OCUPACION,
                        x.TOTAL_INGRESOS
                    }).AsQueryable();

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| DETALLE CLIENTE

        public ActionResult _ClientePEP(int CODIGO_CLIENTE = 0)
        {
            ViewBag.CODIGO_CLIENTE = CODIGO_CLIENTE;
            ViewBag.PEPS = _PEPBlo.GetAll();
            return PartialView("_ClientePEP");
        }

        [HttpGet]
        public JsonResult GetClientePEP(int? page, int? limit, string searchString = null, long CODIGO_CLIENTE = 0)
        {
            try
            {
                int total = 0;
                var queryAlert = _PEPBlo.GetAll().Select(x => new
                    {
                        x.ID,
                        NOMBRE_COMPLETO = x.PRIMER_NOMBRE + " " + 
                                          x.SEGUNDO_NOMBRE + " " + 
                                          x.PRIMER_APELLIDO + " " +
                                          x.SEGUNDO_APELLIDO + " " + 
                                          x.APELLIDO_CASADA
                    });

                var records = _clientePEPBlo.GetDatosDetalle(out total, page, limit, null, null, "ID_CLIENTE", CODIGO_CLIENTE)
                    .Select(x => new
                    {
                        x.ID,
                        x.ID_CLIENTE,
                        x.ID_PEP,
                        x.FECHA_ASOCIACION,
                        x.DESCRIPCION_ASOCIACION,
                        NOMBRE = queryAlert.Where(y => y.ID == x.ID_PEP).Select(y => y.NOMBRE_COMPLETO)
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
        public JsonResult SaveClientePEP(MON_CLIENTE_PEP data)
        {
            MON_CLIENTE_PEP clientePEP = new MON_CLIENTE_PEP();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                var existe = _clientePEPBlo.GetAll().Where(x => (x.ID_CLIENTE == data.ID_CLIENTE && x.ID_PEP == data.ID_PEP)).Any();

                if (existe)
                {
                    if (data.ID != 0)
                    {
                        clientePEP = _clientePEPBlo.GetById(data.ID);

                        if (data.ID_PEP == clientePEP.ID_PEP)
                        {
                            _clientePEPBlo.ValidarSave(data.ID);

                            clientePEP.ID_CLIENTE = data.ID_CLIENTE;
                            clientePEP.ID_PEP = data.ID_PEP;
                            clientePEP.FECHA_ASOCIACION = DateTime.Now;
                            clientePEP.DESCRIPCION_ASOCIACION = data.DESCRIPCION_ASOCIACION;

                            _clientePEPBlo.Save(clientePEP);
                        }
                        else
                            mensaje = PropertiesBlo.msgRelacionExistente;
                    }
                    else
                        mensaje = PropertiesBlo.msgRelacionExistente;
                }
                else
                {
                    _clientePEPBlo.ValidarSave(data.ID);
                    clientePEP.ID_CLIENTE = data.ID_CLIENTE;

                    if (data.ID != 0)
                        clientePEP = _clientePEPBlo.GetById(data.ID);

                    clientePEP.ID_PEP = data.ID_PEP;
                    clientePEP.FECHA_ASOCIACION = DateTime.Now;
                    clientePEP.DESCRIPCION_ASOCIACION = data.DESCRIPCION_ASOCIACION;

                    _clientePEPBlo.Save(clientePEP);
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
        public JsonResult RemoveClientePEP(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _clientePEPBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _clientePEPBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| VISTAS HIJAS
        /// <summary>
        /// METODO PARA OBTENER LAS DIRECCIONES DEL CLIENTE
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchString"></param>
        /// <param name="CODIGO_CLIENTE"></param>
        /// <returns>DEVUELVE LA LISTA DE DIRECCIONES CORRESPONDIENTES AL CLIENTE</returns>
        [HttpGet]
        public JsonResult GetClienteDirecciones(long CODIGO_CLIENTE = 0)
        {
            try
            {
                var records = _SQLBDEntities.VIEW_CLIENTE_DIRECCIONES.AsNoTracking().Where(x => x.CODIGO_CLIENTE == CODIGO_CLIENTE).ToList();

                return Json(new { records }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos ClienteDirecciones");
            }
        }

        /// <summary>
        /// METODO PARA OBTENER LOS TELEFONOS DEL CLIENTE
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchString"></param>
        /// <param name="CODIGO_CLIENTE"></param>
        /// <returns>DEVUELVE LA LISTA DE TELEFONOS CORRESPONDIENTES AL CLIENTE</returns>
        [HttpGet]
        public JsonResult GetClienteTelefonos(long CODIGO_CLIENTE = 0)
        {
            try
            {
                var records = _SQLBDEntities.VIEW_CLIENTE_TELEFONOS.AsNoTracking().Where(x => x.CODIGO_CLIENTE == CODIGO_CLIENTE).ToList();

                return Json(new { records }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos ClienteTelefonos");
            }
        }

        /// <summary>
        /// METODO PARA OBTENER LAS TRANSACCIONES DEL CLIENTE
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchString"></param>
        /// <param name="CODIGO_CLIENTE"></param>
        /// <returns>DEVUELVE LA LISTA DE TRANSACCIONES CORRESPONDIENTES AL CLIENTE</returns>
        [HttpGet]
        public JsonResult GetClienteTransacciones(int? page, int? limit, string FECHA_INI_T, string FECHA_FIN_T, decimal CODIGO_CLIENTE = 0)
        {
            try
            {
                int total = 0;
                int start = (page.Value - 1) * limit.Value;
                DateTime fi = DateTime.Parse(FECHA_INI_T);
                DateTime ff = DateTime.Parse(FECHA_FIN_T).AddHours(23);

                var records = _SQLBDEntities.VIEW_TRANSACCIONES.AsNoTracking()
                    .Where(x =>
                        x.FECHA_CALENDARIO.Value >= fi &&
                        x.FECHA_CALENDARIO.Value <= ff &&
                        x.CODIGO_CLIENTE == CODIGO_CLIENTE
                    ).OrderByDescending(x => x.FECHA_CALENDARIO)
                    .Skip(start)
                    .Take(limit.Value)
                    .ToList();

                total = _SQLBDEntities.VIEW_TRANSACCIONES.AsNoTracking()
                    .Where(x =>
                        x.FECHA_CALENDARIO.Value >= fi &&
                        x.FECHA_CALENDARIO.Value <= ff &&
                        x.CODIGO_CLIENTE == CODIGO_CLIENTE
                    ).Count();

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos ClienteTransacciones");
            }
        }


        /// <summary>
        /// METODO PARA OBTENER LAS NOTIFICACIONES DEL CLIENTE
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="sortBy"></param>
        /// <param name="direction"></param>
        /// <param name="estado"></param>
        /// <param name="searchString"></param>
        /// <param name="CODIGO_CLIENTE"></param>
        /// <returns>DEVUELVE LA LISTA DE TRANSACCIONES CORRESPONDIENTES AL CLIENTE</returns>
        [HttpGet]
        public JsonResult GetClienteAlertas(int? page, int? limit, string sortBy, string direction, string estado, string searchString = null, long CODIGO_CLIENTE = 0)
        {
            try
            {
                int total = 0;
                int start = (page.Value - 1) * limit.Value;
                var records = _alertaBlo.GetAll().Where(x => x.ESTADO_ACTUAL == estado && x.ID_CLIENTE == CODIGO_CLIENTE).AsQueryable();


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

        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| REPORTES
        /// <summary>
        /// MEOTODO PARA GENERAR EL REPORTE RESUMEN CLIENTE
        /// </summary>
        /// <param name="codigoCliente"></param>
        /// <param name="formato"></param>
        [HttpGet]
        public ActionResult Reporte(int codigoCliente, string formato)
        {
            Dictionary<string, object> parametersData = new Dictionary<string, object>
            {
                { "codigoCliente", codigoCliente }
            };

            List<PER_CALIFICACION_FACTOR> listCalificacion = new List<PER_CALIFICACION_FACTOR>();
            PER_CALIFICACION_FACTOR newCalificacion = new PER_CALIFICACION_FACTOR();
            PER_FACTOR newFactor = new PER_FACTOR();
            try
            {
                string nombreReporte = "RptResumenCliente";
                string nombreTabla = "ResumenDatos";
                var datosCliente = _reportesBlo.ReporteResumenCliente(codigoCliente);
                var datosDirecciones = _reportesBlo.ReporteResumenClienteDirecciones(codigoCliente);
                var datosTelefonos = _reportesBlo.ReporteResumenClienteTelefonos(codigoCliente);
                var calificaciones = _calificacionFactorBlo.calificacionPorCodigoCLiente(codigoCliente) ?? new List<PER_CALIFICACION_FACTOR>();

                var puntajeTotal = calificaciones.Sum(x => x.PUNTAJE);
                int idTipoCalificacion = _tipoCalificacionBlo.ObtenerIdCalificacionXValor(puntajeTotal);
                var tipoCalificacion = _tipoCalificacionBlo.GetById(idTipoCalificacion)?.DESCRIPCION ?? "DESCONOCIDO";

                // Agregar la calificación total como un nuevo registro
                calificaciones.Add(new PER_CALIFICACION_FACTOR
                {
                    PUNTAJE = puntajeTotal,
                    PER_FACTOR = new PER_FACTOR
                    {
                        DESCRIPCION = "CALIFICACION : " + tipoCalificacion
                    }
                });

                // Seleccionar solo lo necesario para el reporte
                var datosCalificacion = calificaciones.Select(x => new
                {
                    FACTOR = x.PER_FACTOR?.DESCRIPCION ?? "SIN FACTOR",
                    x.PUNTAJE
                }).ToList();

                if ((datosCliente == null || !datosCliente.Any()) && (datosDirecciones == null || !datosDirecciones.Any()) && (datosTelefonos == null || !datosTelefonos.Any()))
                {
                    return Content("No hay datos disponibles para el reporte.");
                }

                DataTable dtReporte = new DataTable(nombreTabla);

                dtReporte.Columns.Add("CODIGO_CLIENTE", typeof(decimal));
                dtReporte.Columns.Add("TIPO_IDENTIFICACION_1", typeof(string));
                dtReporte.Columns.Add("NUMERO_IDENTIFICACION_1", typeof(string));
                dtReporte.Columns.Add("TIPO_IDENTIFICACION_2", typeof(string));
                dtReporte.Columns.Add("NUMERO_IDENTIFICACION_2", typeof(string));
                dtReporte.Columns.Add("CODIGO_TIPO_CLIENTE", typeof(decimal));
                dtReporte.Columns.Add("TIPO_CLIENTE", typeof(string));
                dtReporte.Columns.Add("CODIGO_PAIS", typeof(decimal));
                dtReporte.Columns.Add("PAIS", typeof(string));
                dtReporte.Columns.Add("NACIONALIDAD", typeof(string));
                dtReporte.Columns.Add("CODIGO_SECTOR_ECONOMICO", typeof(decimal));
                dtReporte.Columns.Add("SECTOR_ECONOMICO", typeof(string));
                dtReporte.Columns.Add("CODIGO_ACTIVIDAD_ECONOMICA", typeof(decimal));
                dtReporte.Columns.Add("ACTIVIDAD_ECONOMICA", typeof(string));
                dtReporte.Columns.Add("CODIGO_CLASE_ACTIVIDAD_ECONOMICA", typeof(string));
                dtReporte.Columns.Add("CLASE_ACTIVIDAD_ECONOMICA", typeof(string));
                dtReporte.Columns.Add("CODIGO_SUB_ACTIVIDAD_ECONOMICA", typeof(string));
                dtReporte.Columns.Add("SUB_ACTIVIDAD_ECONOMICA", typeof(string));
                dtReporte.Columns.Add("TIPO_DE_PERSONA", typeof(string));
                dtReporte.Columns.Add("NOMBRES", typeof(string));
                dtReporte.Columns.Add("PRIMER_APELLIDO", typeof(string));
                dtReporte.Columns.Add("SEGUNDO_APELLIDO", typeof(string));
                dtReporte.Columns.Add("APELLIDO_DE_CASADA", typeof(string));
                dtReporte.Columns.Add("SEXO", typeof(string));
                dtReporte.Columns.Add("FECHA_DE_NACIMIENTO", typeof(DateTime));
                dtReporte.Columns.Add("CODIGO_ESTADO_CIVIL", typeof(string));
                dtReporte.Columns.Add("ESTADO_CIVIL", typeof(string));
                dtReporte.Columns.Add("TOTAL_INGRESOS", typeof(decimal));
                dtReporte.Columns.Add("CODIGO_PROFESION", typeof(decimal));
                dtReporte.Columns.Add("PROFESION", typeof(string));
                dtReporte.Columns.Add("CODIGO_OCUPACION", typeof(decimal));
                dtReporte.Columns.Add("OCUPACION", typeof(string));
                dtReporte.Columns.Add("CODIGO_EMPRESA", typeof(decimal));
                dtReporte.Columns.Add("CONOCIDO_COMO", typeof(string));
                dtReporte.Columns.Add("ALIAS", typeof(string));
                dtReporte.Columns.Add("DOMICILIO", typeof(string));
                dtReporte.Columns.Add("TOTAL_TRANSACCIONES", typeof(int));
                dtReporte.Columns.Add("TOTAL_MONTO", typeof(decimal));

                dtReporte.Columns.Add("CODIGO_DIRECCION", typeof(decimal));
                dtReporte.Columns.Add("DIRECCION_PRINCIPAL", typeof(string));
                dtReporte.Columns.Add("CODIGO_DEPARTAMENTO", typeof(decimal));
                dtReporte.Columns.Add("CODIGO_MUNICIPIO", typeof(decimal));
                dtReporte.Columns.Add("CODIGO_SECTOR", typeof(decimal));
                dtReporte.Columns.Add("DEPARTAMENTO", typeof(string));
                dtReporte.Columns.Add("MUNICIPIO", typeof(string));
                dtReporte.Columns.Add("URBANIZACION", typeof(string));

                dtReporte.Columns.Add("SECUENCIA", typeof(decimal));
                dtReporte.Columns.Add("TIPO_TELEFONO", typeof(string));
                dtReporte.Columns.Add("AREA_TELEFONO", typeof(string));
                dtReporte.Columns.Add("NUM_TELEFONO", typeof(string));
                dtReporte.Columns.Add("EXTENCION", typeof(string));
                dtReporte.Columns.Add("DESCRIPCION", typeof(string));
                dtReporte.Columns.Add("ACCESO_TELEFONO", typeof(string));
                dtReporte.Columns.Add("TELEFONO_PRINCIPAL", typeof(string));
                dtReporte.Columns.Add("CODIGO_TIPO_DIRECCION", typeof(decimal));

                foreach (var datoscliente in datosCliente)
                {
                    DataRow row = dtReporte.NewRow();
                    row["CODIGO_CLIENTE"] = datoscliente.CODIGO_CLIENTE;
                    row["TIPO_IDENTIFICACION_1"] = datoscliente.TIPO_IDENTIFICACION_1 ?? "Sin Documento";
                    row["NUMERO_IDENTIFICACION_1"] = datoscliente.NUMERO_IDENTIFICACION_1 ?? "Sin Documento";
                    row["TIPO_IDENTIFICACION_2"] = datoscliente.TIPO_IDENTIFICACION_2 ?? "N/A";
                    row["NUMERO_IDENTIFICACION_2"] = datoscliente.NUMERO_IDENTIFICACION_2 ?? "N/A";
                    row["CODIGO_TIPO_CLIENTE"] = datoscliente.CODIGO_TIPO_CLIENTE;
                    row["TIPO_CLIENTE"] = datoscliente.TIPO_CLIENTE ?? "N/A";
                    row["CODIGO_PAIS"] = datoscliente.CODIGO_PAIS ?? (object)DBNull.Value;
                    row["PAIS"] = datoscliente.PAIS ?? "N/A";
                    row["NACIONALIDAD"] = datoscliente.NACIONALIDAD ?? "N/A";
                    row["CODIGO_SECTOR_ECONOMICO"] = datoscliente.CODIGO_SECTOR_ECONOMICO ?? (object)DBNull.Value;
                    row["SECTOR_ECONOMICO"] = datoscliente.SECTOR_ECONOMICO ?? "N/A";
                    row["CODIGO_ACTIVIDAD_ECONOMICA"] = datoscliente.CODIGO_ACTIVIDAD_ECONOMICA;
                    row["ACTIVIDAD_ECONOMICA"] = datoscliente.ACTIVIDAD_ECONOMICA ?? "N/A";
                    row["CODIGO_CLASE_ACTIVIDAD_ECONOMICA"] = datoscliente.CODIGO_CLASE_ACTIVIDAD_ECONOMICA ?? "N/A";
                    row["CLASE_ACTIVIDAD_ECONOMICA"] = datoscliente.CLASE_ACTIVIDAD_ECONOMICA ?? "N/A";
                    row["CODIGO_SUB_ACTIVIDAD_ECONOMICA"] = datoscliente.CODIGO_SUB_ACTIVIDAD_ECONOMICA ?? "N/A";
                    row["SUB_ACTIVIDAD_ECONOMICA"] = datoscliente.SUB_ACTIVIDAD_ECONOMICA ?? "N/A";
                    row["TIPO_DE_PERSONA"] = datoscliente.TIPO_DE_PERSONA ?? "N/A";
                    row["NOMBRES"] = datoscliente.NOMBRES ?? "N/A";
                    row["PRIMER_APELLIDO"] = datoscliente.PRIMER_APELLIDO ?? "N/A";
                    row["SEGUNDO_APELLIDO"] = datoscliente.SEGUNDO_APELLIDO ?? "N/A";
                    row["APELLIDO_DE_CASADA"] = datoscliente.APELLIDO_DE_CASADA ?? "N/A";
                    row["SEXO"] = datoscliente.SEXO ?? "N/A";
                    row["FECHA_DE_NACIMIENTO"] = datoscliente.FECHA_DE_NACIMIENTO ?? (object)DBNull.Value;
                    row["CODIGO_ESTADO_CIVIL"] = datoscliente.CODIGO_ESTADO_CIVIL ?? "N/A";
                    row["ESTADO_CIVIL"] = datoscliente.ESTADO_CIVIL ?? "N/A";
                    row["TOTAL_INGRESOS"] = datoscliente.TOTAL_INGRESOS ?? 0.0m;
                    row["CODIGO_PROFESION"] = datoscliente.CODIGO_PROFESION ?? (object)DBNull.Value;
                    row["PROFESION"] = datoscliente.PROFESION ?? "N/A";
                    row["CODIGO_OCUPACION"] = datoscliente.CODIGO_OCUPACION ?? (object)DBNull.Value;
                    row["OCUPACION"] = datoscliente.OCUPACION ?? "N/A";
                    row["CODIGO_EMPRESA"] = datoscliente.CODIGO_EMPRESA;
                    row["CONOCIDO_COMO"] = datoscliente.CONOCIDO_COMO ?? "N/A";
                    row["ALIAS"] = datoscliente.ALIAS ?? "N/A";
                    row["DOMICILIO"] = datoscliente.DOMICILIO ?? "N/A";
                    row["TOTAL_TRANSACCIONES"] = datoscliente.TOTAL_TRANSACCIONES;
                    row["TOTAL_MONTO"] = datoscliente.TOTAL_MONTO;

                    dtReporte.Rows.Add(row);

                }

                foreach (var datosdirecciones in datosDirecciones)
                {
                    DataRow row = dtReporte.NewRow();

                    row["CODIGO_DIRECCION"] = datosdirecciones.CODIGO_DIRECCION;
                    row["DIRECCION_PRINCIPAL"] = datosdirecciones.DIRECCION_PRINCIPAL ?? "N/A";
                    row["CODIGO_DEPARTAMENTO"] = datosdirecciones.CODIGO_DEPARTAMENTO;
                    row["CODIGO_MUNICIPIO"] = datosdirecciones.CODIGO_MUNICIPIO;
                    row["CODIGO_SECTOR"] = datosdirecciones.CODIGO_SECTOR;
                    row["DEPARTAMENTO"] = datosdirecciones.DEPARTAMENTO ?? "N/A";
                    row["MUNICIPIO"] = datosdirecciones.MUNICIPIO ?? "N/A";
                    row["URBANIZACION"] = datosdirecciones.URBANIZACION ?? "N/A";

                    dtReporte.Rows.Add(row);
                }

                foreach (var datostelefonos in datosTelefonos)
                {
                    DataRow row = dtReporte.NewRow();

                    row["SECUENCIA"] = datostelefonos.SECUENCIA;
                    row["TIPO_TELEFONO"] = datostelefonos.TIPO_TELEFONO ?? "N/A";
                    row["AREA_TELEFONO"] = datostelefonos.AREA_TELEFONO ?? "N/A";
                    row["NUM_TELEFONO"] = datostelefonos.NUM_TELEFONO ?? "N/A";
                    row["EXTENCION"] = datostelefonos.EXTENCION ?? "N/A";
                    row["DESCRIPCION"] = datostelefonos.DESCRIPCION ?? "N/A";
                    row["ACCESO_TELEFONO"] = datostelefonos.ACCESO_TELEFONO ?? "N/A";
                    row["TELEFONO_PRINCIPAL"] = datostelefonos.TELEFONO_PRINCIPAL ?? "N/A";
                    row["CODIGO_TIPO_DIRECCION"] = datostelefonos.CODIGO_TIPO_DIRECCION ?? 0.0m;

                    dtReporte.Rows.Add(row);
                }

                VerReporte(nombreReporte, formato, parametersData, dtReporte, nombreTabla);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new HttpException(404, null);
            }
        }



        /// <summary>
        /// MMETODO PARA GENERAR EL REPORTE DE NOTIFICACIONES POR CLIENTE
        /// </summary>
        /// <param name="codigoCliente"></param>
        /// <param name="NombreCliente"></param>
        /// <param name="formato"></param>
        [HttpGet]
        public ActionResult ReporteAlertasCliente(int codigoCliente, string NombreCliente, string formato)
        {

            Dictionary<string, object> parametersData = new Dictionary<string, object>
            {
                { "codigoCliente", codigoCliente.ToString() },
                { "NombreCliente", NombreCliente.ToString() }
            };

            string nombreReporte = "RptAlertasCliente";
            string nombreTabla = "ALE_ALERTA";

            var datosReporte = _reportesBlo.ReporteAlertasCliente(codigoCliente);
            if (datosReporte == null || !datosReporte.Any())
            {
                return Content("No hay datos disponibles para el reporte.");
            }

            var datosLimpios = datosReporte.Select(x => new ALE_ALERTA
            {
                NOMBRE_CLIENTE = string.IsNullOrEmpty(x.NOMBRE_CLIENTE) ? "DESCONOCIDO" : x.NOMBRE_CLIENTE,
                NUMERO_FACTURA = string.IsNullOrEmpty(x.NUMERO_FACTURA) ? "DESCONOCIDO" : x.NUMERO_FACTURA,
                DESCRIPCION = string.IsNullOrEmpty(x.DESCRIPCION) ? "DESCONOCIDO" : x.DESCRIPCION,
                COLOR = string.IsNullOrEmpty(x.COLOR) ? "DESCONOCIDO" : x.COLOR,
                DESCRIPCION_ALERTA = string.IsNullOrEmpty(x.DESCRIPCION_ALERTA) ? "DESCONOCIDO" : x.DESCRIPCION_ALERTA,
                ESTADO_ANTERIOR = string.IsNullOrEmpty(x.ESTADO_ANTERIOR) ? "DESCONOCIDO" : x.ESTADO_ANTERIOR,
                ESTADO_ACTUAL = string.IsNullOrEmpty(x.ESTADO_ACTUAL) ? "DESCONOCIDO" : x.ESTADO_ACTUAL,
                PRESTAMO = string.IsNullOrEmpty(x.PRESTAMO) ? "DESCONOCIDO" : x.PRESTAMO,
                SECUENCIA = string.IsNullOrEmpty(x.SECUENCIA) ? "DESCONOCIDO" : x.SECUENCIA,
                CLASE_PRODUCTO = string.IsNullOrEmpty(x.CLASE_PRODUCTO) ? "DESCONOCIDO" : x.CLASE_PRODUCTO,
                BANCO_TRANSACCION = string.IsNullOrEmpty(x.BANCO_TRANSACCION) ? "DESCONOCIDO" : x.BANCO_TRANSACCION,
                CODIGO_AGENCIA = string.IsNullOrEmpty(x.CODIGO_AGENCIA) ? "DESCONOCIDO" : x.CODIGO_AGENCIA,
                ANALISIS_CASO = string.IsNullOrEmpty(x.ANALISIS_CASO) ? "DESCONOCIDO" : x.ANALISIS_CASO,
                CODIGO_EJECUTIVO = string.IsNullOrEmpty(x.CODIGO_EJECUTIVO) ? "DESCONOCIDO" : x.CODIGO_EJECUTIVO,
            }).ToList();

            DataTable dtReporte = DataTableHelper.ToDataTable(datosLimpios);

            VerReporte(nombreReporte, formato, parametersData, dtReporte,
                            nombreTabla);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// METODO PARA GENERAR EL REPORTE DE TRANSACCIONES DEL CLIENTE
        /// </summary>
        /// <param name="codigoCliente"></param>
        /// <param name="NombreCliente"></param>
        /// <param name="formato"></param>
        [HttpGet]
        public ActionResult ReporteTransaccionesCliente(string FECHA_INI_T, string FECHA_FIN_T, decimal codigoCliente, string NOMBRE_T, string formato)
        {
            DateTime fi = DateTime.Parse(FECHA_INI_T);
            DateTime ff = DateTime.Parse(FECHA_FIN_T);

            Dictionary<string, object> parametersData = new Dictionary<string, object>
            {
                { "FECHA_INI_T", fi },
                { "FECHA_FIN_T", ff },
                { "CodigoCliente", codigoCliente.ToString() },
                { "NOMBRE_T", NOMBRE_T.ToString() }
            };

            string nombreReporte = "RptTransaccionesCliente";
            string nombreTabla = "VIEW_TRANSACCIONES";

            var datosReporte = _reportesBlo.ReporteTransaccionesCliente(fi.Date.ToString(), ff.Date.ToString(), codigoCliente, NOMBRE_T);

            if (datosReporte == null || !datosReporte.Any())
            {
                return Content("No hay datos disponibles para el reporte.");
            }

            var datosLimpios = datosReporte.Select(x =>  new VIEW_TRANSACCIONES
            {
                CLASE_PRODUCTO = string.IsNullOrEmpty(x.CLASE_PRODUCTO) ? "DESCONOCIDO" : x.CLASE_PRODUCTO,
                CODIGO_CLIENTE = x.CODIGO_CLIENTE ?? 0.0m,
                NUMERO_PRODUCTO = string.IsNullOrEmpty(x.NUMERO_PRODUCTO) ? "DESCONOCIDO" : x.NUMERO_PRODUCTO,
                CODIGO_USUARIO = string.IsNullOrEmpty(x.CODIGO_USUARIO) ? "DESCONOCIDO" : x.CODIGO_USUARIO,
                SECUENCIA = string.IsNullOrEmpty(x.SECUENCIA) ? "DESCONOCIDO" : x.SECUENCIA,
                FECHA_APLICACION = x.FECHA_APLICACION ?? DateTime.MinValue,
                CODIGO_FINANCIERA = string.IsNullOrEmpty(x.CODIGO_FINANCIERA) ? "DESCONOCIDO" : x.CODIGO_FINANCIERA,
                COLECTOR_MOVIMIENTO = string.IsNullOrEmpty(x.COLECTOR_MOVIMIENTO) ? "DESCONOCIDO" : x.COLECTOR_MOVIMIENTO,
                TIPO_TRANSACCION = string.IsNullOrEmpty(x.TIPO_TRANSACCION) ? "DESCONOCIDO" : x.TIPO_TRANSACCION,
                TIPO_OPERACION = string.IsNullOrEmpty(x.TIPO_OPERACION) ? "DESCONOCIDO" : x.TIPO_OPERACION,
                FORMA_PAGO = string.IsNullOrEmpty(x.FORMA_PAGO) ? "DESCONOCIDO" : x.FORMA_PAGO,
                FECHA_CALENDARIO = x.FECHA_CALENDARIO ?? DateTime.MinValue,
                CODIGO_AGENCIA_BANCO = string.IsNullOrEmpty(x.CODIGO_AGENCIA_BANCO) ? "DESCONOCIDO" : x.CODIGO_AGENCIA_BANCO,
                VALOR_TRANSACCION = x.VALOR_TRANSACCION ?? 0.0,
                NUMERO_RECIBO = string.IsNullOrEmpty(x.NUMERO_RECIBO) ? "DESCONOCIDO" : x.NUMERO_RECIBO,
                CODIGO_PATRONO = string.IsNullOrEmpty(x.CODIGO_PATRONO) ? "DESCONOCIDO" : x.CODIGO_PATRONO,
                ESTADO_CARTERA = string.IsNullOrEmpty(x.ESTADO_CARTERA) ? "DESCONOCIDO" : x.ESTADO_CARTERA,
                COLECTOR_CONVENIO = string.IsNullOrEmpty(x.COLECTOR_CONVENIO) ? "DESCONOCIDO" : x.COLECTOR_CONVENIO,
                FACTURA_CONVENIO = string.IsNullOrEmpty(x.FACTURA_CONVENIO) ? "DESCONOCIDO" : x.FACTURA_CONVENIO,
                SALDO_CAPITAL = x.SALDO_CAPITAL ?? 0.0,
                SALDO_INTERES = x.SALDO_INTERES ?? 0.0,
                SALDO_SEGUROS_IVA = x.SALDO_SEGUROS_IVA ?? 0.0,
                SALDO_OTROS = x.SALDO_OTROS ?? 0.0,
                EXCEDENTE = x.EXCEDENTE ?? 0.0,
                CAPITAL_VIGENTE = x.CAPITAL_VIGENTE ?? 0.0,
                CAPITAL_MORA = x.CAPITAL_MORA ?? 0.0,
                CAPITAL_VENCIDO = x.CAPITAL_VENCIDO ?? 0.0,
                INTERES_VIGENTE = x.INTERES_VIGENTE ?? 0.0,
                INTERES_MORA = x.INTERES_MORA ?? 0.0,
                INTERES_VENCIDO = x.INTERES_VENCIDO ?? 0.0,
                FECHA_CANCELACION = x.FECHA_CANCELACION ?? DateTime.MinValue,
                FECHA_APERTURA = x.FECHA_APERTURA ?? DateTime.MinValue,
                FECHA_VENCIMIENTO = x.FECHA_VENCIMIENTO ?? DateTime.MinValue,
                VALOR_CUOTA = x.VALOR_CUOTA ?? 0.0,
                MONTO_OTORGADO = x.MONTO_OTORGADO ?? 0.0,
                VALOR_VALUO = x.VALOR_VALUO ?? 0.0m,
                VALOR_CONTABLE = x.VALOR_CONTABLE ?? 0.0m,
                FECHA_SOLICITUD = x.FECHA_SOLICITUD ?? DateTime.MinValue,
                NOMBRE_COMPLETO = string.IsNullOrEmpty(x.NOMBRE_COMPLETO) ? "DESCONOCIDO" : x.NOMBRE_COMPLETO,
                TOTAL_INGRESOS = x.TOTAL_INGRESOS ?? 0.0m,
                CODIGO_AGENCIA = string.IsNullOrEmpty(x.CODIGO_AGENCIA) ? "DESCONOCIDO" : x.CODIGO_AGENCIA
            }).ToList();

            DataTable dtReporte = DataTableHelper.ToDataTable(datosLimpios);

            VerReporte(nombreReporte, formato, parametersData, dtReporte,
                            nombreTabla);

            return RedirectToAction("Index");
        }

        #endregion


    }
}