using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Monitoreo;

using Novacode;
using System.Text;
using System.Data;
using System.IO;
using System.Drawing;
using Blo.Properties;
using MariGold.OpenXHTML;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;


using Blo.Reportes;
using Microsoft.Reporting.WebForms;


namespace View.Controllers.Monitoreo
{
    [NoCache]
    [Autorizacion]
    public class RosController : BaseController
    {

        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IRosBlo _rosBlo;
        private readonly IPersonasRosBlo _personasRosBlo;
        private readonly IOperacionesRosBlo _operacionesRosBlo;
        private readonly IAdjuntosRosBlo _adjuntosRosBlo;
        private readonly IAgenciaBancariaBlo _agenciaBancariaBlo;
        private readonly ICatFinancieraBlo _catFinancieraBlo;
        private readonly IReportesBlo _reportesBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public RosController(IRosBlo rosBlo, IPersonasRosBlo personasRosBlo, IReportesBlo reportesBlo,
            IOperacionesRosBlo operacionesRosBlo, IAdjuntosRosBlo adjuntosRosBlo, IAgenciaBancariaBlo agenciaBancariaBlo, ICatFinancieraBlo catFinancieraBlo)
        {
            _rosBlo = rosBlo;
            _personasRosBlo = personasRosBlo;
            _operacionesRosBlo = operacionesRosBlo;
            _adjuntosRosBlo = adjuntosRosBlo;
            _agenciaBancariaBlo = agenciaBancariaBlo;
            _catFinancieraBlo = catFinancieraBlo;
            _reportesBlo = reportesBlo;
        }


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> ROS
        public ActionResult Index()
        {

            return View();
        }


        /// <summary>
        /// OBTENER LISTA DE ROS //
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="sortBy"></param>
        /// <param name="direction"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRos(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0; ;
                int start = (page.Value - 1) * limit.Value;
                var records = _rosBlo.GetAll()
                              .Select(x => new
                              {
                                  x.ID,
                                  x.CODIGO_UIF,
                                  x.ANALISIS_CASO,
                                  x.FECHA_ELABORACION,
                                  x.DESCRIPCION_OPERACION_REPORTADA
                              }).AsQueryable();


                //Buscar texto en varios campos 
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    records = records
                              .Where(x => (
                                  x.DESCRIPCION_OPERACION_REPORTADA +
                                   x.CODIGO_UIF +
                                  x.ANALISIS_CASO +
                                  x.FECHA_ELABORACION).ToUpper().Contains(searchString.Trim().ToUpper())
                               )
                              .Select(x => new
                              {
                                  x.ID,
                                  x.CODIGO_UIF,
                                  x.ANALISIS_CASO,
                                  x.FECHA_ELABORACION,
                                  x.DESCRIPCION_OPERACION_REPORTADA
                              }).AsQueryable();
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

        /// <summary>
        /// VERIFICAR QUE NO SE INGRESE UN CODIGO UIF DUPLICADO
        /// </summary>
        /// <param name="CODIGO_UIF"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidaCodUIF(string  CODIGO_UIF)
        {
            bool valid = true;
            string message = "";

            if (_rosBlo.GetCodUIF(CODIGO_UIF).Any())
            {
                valid = false;
                message = "El codigo ROS UIF ya existe";
            }

            return valid ? Json(new { valid }, JsonRequestBehavior.AllowGet)
                         : Json(new { valid, message }, JsonRequestBehavior.AllowGet);
        }


        


        /// <summary>
        /// GUARDAR ROS ///
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Save(MON_ROS datos)
        {
            MON_ROS ros = new MON_ROS();
            string mensaje = PropertiesBlo.msgExito;

            try
            {
                _rosBlo.ValidarSave(datos.ID);

                if (datos.ID != 0)
                    ros = _rosBlo.GetById(datos.ID);

                ros.CODIGO_UIF = datos.CODIGO_UIF;
                ros.FECHA_ELABORACION = datos.FECHA_ELABORACION;
                ros.DESCRIPCION_OPERACION_REPORTADA = datos.DESCRIPCION_OPERACION_REPORTADA;
                ros.ANALISIS_CASO = datos.ANALISIS_CASO;
                _rosBlo.Save(ros);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }
            return Json(new { mensaje, ros.ID }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// ELIMINAR ROS ////
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Remove(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _rosBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _rosBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }
            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        //////////// EDITOR /////////////////////////////
        // CONVERTIR ANALISIS DEL ROS A WORD

        [HttpGet]
        [ValidateInput(false)]
        public FileResult Exportar(long id)
        {
          
           

            MON_ROS ros = new MON_ROS();

            ros = _rosBlo.GetById(id);
         
            using (MemoryStream mem = new MemoryStream())
            {

                if (ros.ANALISIS_CASO==null)
                {
                    WordDocument doc = new WordDocument(mem);

                    doc.Process(new HtmlParser("NO SE HA CREADO ANALISIS PARA ESTE ROS " + ros.CODIGO_UIF));
                    doc.Save();

                    return File(mem.ToArray(), "application/msword", "ROS_" + ros.CODIGO_UIF + ".docx");
                }
                else{
                    WordDocument doc = new WordDocument(mem);
                    
                        doc.Process(new HtmlParser(ros.ANALISIS_CASO));
                        doc.Save();                    

                        return File(mem.ToArray(), "application/msword", "ROS_" + ros.CODIGO_UIF + ".docx");
                }   
               

            }                       

        }


        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> PERSONAS
       
        public ActionResult _Personas(long ID_ROS = 0)
        {
            ViewBag.ID_ROS = ID_ROS;
            ViewBag.profesiones = _SQLBDEntities.VIEW_PROFESIONES.AsNoTracking().ToList();
            ViewBag.tiposDocumentos = _SQLBDEntities.VIEW_TIPO_DOCUMENTO.AsNoTracking().ToList();
            ViewBag.nacionalidades = _SQLBDEntities.VIEW_PAISNACIONALIDAD.AsNoTracking().ToList();
            ViewBag.actividadeconomica = _SQLBDEntities.VIEW_ACTIVIDAD_ECONOMICA.AsNoTracking().ToList();
            ViewBag.estadocivil = _SQLBDEntities.VIEW_ESTADO_CIVIL.AsNoTracking().ToList();

            return PartialView("_Personas");
        }


        /// <summary>
        /// OBTENER LISTA DE PERSONAS VINCULADAS A ROS ////
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="sortBy"></param>
        /// <param name="direction"></param>
        /// <param name="searchString"></param>
        /// <param name="ID_ROS"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetPersonas(int? page, int? limit, string sortBy, string direction, string searchString = null, long ID_ROS = 0)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _personasRosBlo.GetAll(true)
                         .Where(x => x.ID_ROS == ID_ROS)
                         .Select(x => new
                         {
                             x.ID,
                             x.ID_ROS,
                             x.RELACION_CON_SO,
                             x.FECHA_NACIMIENTO,
                             x.PRIMER_NOMBRE,
                             x.SEGUNDO_NOMBRE,
                             x.PRIMER_APELLIDO,
                             x.SEGUNDO_APELLIDO,
                             x.APELLIDO_CASADA,
                             x.TIPO_PERSONA,
                             x.GENERO,
                             x.ID_ESTADO_FAMILIAR,
                             x.ID_ACTIVIDAD_ECONOMICA,
                             x.ID_NACIONALIDAD,
                             x.ID_PAIS,
                             x.ID_DEPARTAMENTO,
                             x.ID_MUNICIPIO,
                             x.ID_URBANIZACION,
                             x.ID_TIPO_DOCUMENTO,
                             x.ID_PROFESION,
                             x.NUMERO_DOCUMENTO,
                             x.DIRECCION_ESPECIFICA,
                             NACIONALIDAD = _SQLBDEntities.VIEW_PAISNACIONALIDAD.Where(y => y.CODIGO_PAIS == x.ID_NACIONALIDAD).Select(y => y.NACIONALIDAD),
                             PAIS = _SQLBDEntities.VIEW_PAISNACIONALIDAD.Where(z => z.CODIGO_PAIS == x.ID_PAIS).Select(z => z.NOMBRE),
                             DEPARTAMENTO = _SQLBDEntities.VIEW_DEPARTAMENTO.Where(w => w.CODIGO_DEPARTAMENTO == x.ID_DEPARTAMENTO && w.CODIGO_PAIS == x.ID_PAIS).Select(w => w.NOMBRE),
                             MUNICIPIO = _SQLBDEntities.VIEW_MUNICIPIO.Where(v => v.CODIGO_MUNICIPIO == x.ID_MUNICIPIO && v.CODIGO_DEPARTAMENTO == x.ID_DEPARTAMENTO && v.CODIGO_PAIS == x.ID_PAIS).Select(v => v.NOMBRE),
                             TIPO_DOCUMENTO = _SQLBDEntities.VIEW_TIPO_DOCUMENTO.Where(o => o.CODIGO_TIPO_IDENTIFICACION == x.ID_TIPO_DOCUMENTO).Select(o => o.DESCRIPCION)
                         }).AsQueryable();

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


        /// <summary>
        /// GUARDAR PERSONA VINCULADA A ROS //
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SavePersonas(MON_ROS_ACTOR data)
        {
            MON_ROS_ACTOR personas = new MON_ROS_ACTOR();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _personasRosBlo.ValidarSave(data.ID);

                personas.ID_ROS = data.ID_ROS;

                if (data.ID != 0)
                    personas = _personasRosBlo.GetById(data.ID);

                personas.PRIMER_NOMBRE = data.PRIMER_NOMBRE;
                personas.SEGUNDO_NOMBRE = data.SEGUNDO_NOMBRE;
                personas.PRIMER_APELLIDO = data.PRIMER_APELLIDO;
                personas.SEGUNDO_APELLIDO = data.SEGUNDO_APELLIDO;
                personas.APELLIDO_CASADA = data.APELLIDO_CASADA;
                personas.TIPO_PERSONA = data.TIPO_PERSONA;
                personas.GENERO = data.GENERO;
                personas.RELACION_CON_SO = data.RELACION_CON_SO;
                personas.FECHA_NACIMIENTO = data.FECHA_NACIMIENTO;
                personas.ID_NACIONALIDAD = data.ID_NACIONALIDAD;
                personas.ID_PAIS = data.ID_PAIS;
                personas.ID_DEPARTAMENTO = data.ID_DEPARTAMENTO;
                personas.ID_MUNICIPIO = data.ID_MUNICIPIO;
                personas.ID_URBANIZACION = data.ID_URBANIZACION;
                personas.DIRECCION_ESPECIFICA = data.DIRECCION_ESPECIFICA;
                personas.ID_TIPO_DOCUMENTO = data.ID_TIPO_DOCUMENTO;
                personas.NUMERO_DOCUMENTO = data.NUMERO_DOCUMENTO;
                personas.ID_ESTADO_FAMILIAR = data.ID_ESTADO_FAMILIAR;
                personas.ID_ACTIVIDAD_ECONOMICA = data.ID_ACTIVIDAD_ECONOMICA;
                personas.ID_PROFESION = data.ID_PROFESION;

                _personasRosBlo.Save(personas);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// ELIMINAR PERSONAS VINCULADAS A ROS ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RemovePersonas(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _personasRosBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _personasRosBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// OBTENER DEPARTAMENTOS POR PAIS///
        /// </summary>
        /// <param name="pais"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DepartamentosXPais(int pais)
        {
            try
            {
                var departamentos = _SQLBDEntities.VIEW_DEPARTAMENTO
                         .Where(x => x.CODIGO_PAIS == pais)
                         .Select(x => new
                         {
                             x.CODIGO_DEPARTAMENTO,
                             x.NOMBRE
                         }).ToList();

                return Json(new { departamentos }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener los datos");
            }
        }


        /// <summary>
        /// OBTENER MUNICIPIOS POR DEPARTAMENTO /// 
        /// </summary>
        /// <param name="pais"></param>
        /// <param name="depto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MunicipiosXDepartamento(int pais, int depto)
        {
            try
            {
                var municipios = _SQLBDEntities.VIEW_MUNICIPIO
                         .Where(x => x.CODIGO_DEPARTAMENTO == depto && x.CODIGO_PAIS == pais)
                         .Select(x => new
                         {
                             x.CODIGO_MUNICIPIO,
                             x.NOMBRE
                         }).ToList();

                return Json(new { municipios }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener los datos");
            }
        }


        /// <summary>
        ///OBTENER URBANIZACIONES POR MUNICIPIO ///
        /// </summary>
        /// <param name="pais"></param>
        /// <param name="depto"></param>
        /// <param name="municipio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UrbanizacionXMunicipios(int pais, int depto, int municipio)
        {
            try
            {
                var urbanizaciones = _SQLBDEntities.VIEW_URBANIZACION
                         .Where(x => x.CODIGO_MUNICIPIO == municipio && x.CODIGO_DEPARTAMENTO == depto && x.CODIGO_PAIS == pais)
                         .Select(x => new
                         {
                             x.CODIGO_SECTOR,
                             x.DESCRIPCION
                         }).ToList();

                return Json(new { urbanizaciones }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener los datos");
            }
        }


        /// <summary>
        /// OBTENER ANIDACION DE PAIS, DEPARTAMENTO, MUNICIPIO Y URBANIZACION ///
        /// </summary>
        /// <param name="pais"></param>
        /// <param name="depto"></param>
        /// <param name="municipio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDetpMuniUrbanizacion(decimal pais, decimal depto, decimal municipio)
        {
            try
            {
                var departamentos = _SQLBDEntities.VIEW_DEPARTAMENTO
                                     .Where(x => x.CODIGO_PAIS == pais)
                                     .AsEnumerable()
                                     .Select(x => new
                                     {
                                         CODIGO_DEPARTAMENTO = (decimal?)x.CODIGO_DEPARTAMENTO ?? 0,
                                         NOMBRE = x.NOMBRE ?? "SIN NOMBRE"
                                     }).ToList();

                var municipios = _SQLBDEntities.VIEW_MUNICIPIO
                                 .Where(x => x.CODIGO_DEPARTAMENTO == depto && x.CODIGO_PAIS == pais)
                                 .AsEnumerable()
                                 .Select(x => new
                                 {
                                     CODIGO_MUNICIPIO = (decimal?)x.CODIGO_MUNICIPIO ?? 0,
                                     NOMBRE = x.NOMBRE ?? "SIN NOMBRE"
                                 }).ToList();

                var urbanizaciones = _SQLBDEntities.VIEW_URBANIZACION
                                     .Where(x => x.CODIGO_MUNICIPIO == municipio && x.CODIGO_DEPARTAMENTO == depto && x.CODIGO_PAIS == pais)
                                     .AsEnumerable()
                                     .Select(x => new
                                     {
                                         CODIGO_SECTOR = (decimal?)x.CODIGO_SECTOR ?? 0,
                                         DESCRIPCION = x.DESCRIPCION ?? "SIN DESCRIPCION"
                                     }).ToList();

                return Json(new { departamentos, municipios, urbanizaciones }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener los datos");
            }
        }

        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> OPERACIONES

        public ActionResult _Operaciones(long ID_ROS = 0)
        {
            ViewBag.ID_ROS = ID_ROS;
            ViewBag.tiposDocumentos = _SQLBDEntities.VIEW_TIPO_DOCUMENTO.ToList();
            ViewBag.nacionalidades = _SQLBDEntities.VIEW_PAISNACIONALIDAD.ToList();
            ViewBag.bancos = _SQLBDEntities.MON_CAT_FINANCIERA.ToList();
            ViewBag.tipoProducto = _SQLBDEntities.VIEW_TIPO_CREDITO.ToList();
            ViewBag.AgenciaFinanciera = _agenciaBancariaBlo.GetAll(true).OrderBy(x => x.MON_CAT_FINANCIERA.DESCRIPCION).ToList();

            return PartialView("_Operaciones");
        }


        /// <summary>
        /// OBTENER LISTA DE OPERACIONES DE ROS ////
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="sortBy"></param>
        /// <param name="direction"></param>
        /// <param name="searchString"></param>
        /// <param name="ID_ROS"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetOperaciones(int? page, int? limit, string sortBy, string direction, string searchString = null, long ID_ROS = 0)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _operacionesRosBlo.GetAll(true)
                  .Where(o => o.ID_ROS == ID_ROS)
                  .Select(o => new
                  {
                      o.ID,
                      o.ID_ROS,
                      o.ID_TRANSACCION,
                      o.ID_AGENCIA_BANCO,
                      AGENCIABANCO = o.MON_AGENCIA_BANCARIA.MON_CAT_FINANCIERA.DESCRIPCION + " | " + o.MON_AGENCIA_BANCARIA.DESCRIPCION,
                      o.FECHA_OPERACION,
                      o.NOMBRE_CLIENTE,
                      o.ID_TIPO_DOCUMENTO,
                      o.NUMERO_DOCUMENTO,
                      o.ID_TIPO_PRODUCTO,
                      o.NUMERO_PRESTAMO,
                      o.VALOR_PAGADO,
                      o.ID_PAIS,
                      o.ID_DEPARTAMENTO,
                      o.ID_MUNICIPIO,
                      o.DIRECCION_FINANCIERA,
                      o.PROCEDENCIA_DESTINO,
                      o.RAZON_ROS,
                      PAIS = _SQLBDEntities.VIEW_PAISNACIONALIDAD.Where(z => z.CODIGO_PAIS == o.ID_PAIS).Select(z => z.NOMBRE),
                      DEPARTAMENTO = _SQLBDEntities.VIEW_DEPARTAMENTO.Where(w => w.CODIGO_DEPARTAMENTO == o.ID_DEPARTAMENTO && w.CODIGO_PAIS == o.ID_PAIS).Select(w => w.NOMBRE),
                      MUNICIPIO = _SQLBDEntities.VIEW_MUNICIPIO.Where(v => v.CODIGO_MUNICIPIO == o.ID_MUNICIPIO && v.CODIGO_DEPARTAMENTO == o.ID_DEPARTAMENTO && v.CODIGO_PAIS == o.ID_PAIS).Select(v => v.NOMBRE),
                      TIPO_DOCUMENTO = _SQLBDEntities.VIEW_TIPO_DOCUMENTO.Where(d => d.CODIGO_TIPO_IDENTIFICACION == o.ID_TIPO_DOCUMENTO).Select(d => d.DESCRIPCION)
                  }).AsQueryable();

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


        /// <summary>
        /// GUARDAR OPERACIONES DE ROS ///
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveOperaciones(MON_ROS_OPERACION data)
        {
            MON_ROS_OPERACION operaciones = new MON_ROS_OPERACION();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _operacionesRosBlo.ValidarSave(data.ID);

                operaciones.ID_ROS = data.ID_ROS;

                if (data.ID != 0)
                    operaciones = _operacionesRosBlo.GetById(data.ID);

                operaciones.ID_TRANSACCION = data.ID_TRANSACCION;
                operaciones.ID_AGENCIA_BANCO = data.ID_AGENCIA_BANCO;
                operaciones.FECHA_OPERACION = data.FECHA_OPERACION;
                operaciones.NOMBRE_CLIENTE = data.NOMBRE_CLIENTE;
                operaciones.ID_TIPO_DOCUMENTO = data.ID_TIPO_DOCUMENTO;
                operaciones.NUMERO_DOCUMENTO = data.NUMERO_DOCUMENTO;
                operaciones.ID_TIPO_PRODUCTO = data.ID_TIPO_PRODUCTO;
                operaciones.NUMERO_PRESTAMO = data.NUMERO_PRESTAMO;
                operaciones.VALOR_PAGADO = data.VALOR_PAGADO;
                operaciones.ID_PAIS = data.ID_PAIS;
                operaciones.ID_DEPARTAMENTO = data.ID_DEPARTAMENTO;
                operaciones.ID_MUNICIPIO = data.ID_MUNICIPIO;
                operaciones.DIRECCION_FINANCIERA = data.DIRECCION_FINANCIERA;
                operaciones.PROCEDENCIA_DESTINO = data.PROCEDENCIA_DESTINO;
                operaciones.RAZON_ROS = data.RAZON_ROS;


                _operacionesRosBlo.Save(operaciones);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// ELIMINAR OPERACIONES DE ROS /////
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RemoveOperaciones(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _operacionesRosBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _operacionesRosBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        //OBTENER DEPARTAMENTOS Y MUNICIPIOS
        [HttpPost]
        public JsonResult GetDetpMuni(int pais, int depto, int municipio)
        {
            try
            {
                var departamentos = _SQLBDEntities.VIEW_DEPARTAMENTO
                                     .Where(x => x.CODIGO_PAIS == pais)
                                     .Select(x => new
                                     {
                                         x.CODIGO_DEPARTAMENTO,
                                         x.NOMBRE
                                     }).ToList();

                var municipios = _SQLBDEntities.VIEW_MUNICIPIO
                                 .Where(x => x.CODIGO_DEPARTAMENTO == depto && x.CODIGO_PAIS == pais)
                                 .Select(x => new
                                 {
                                     x.CODIGO_MUNICIPIO,
                                     x.NOMBRE
                                 }).ToList();

                return Json(new { departamentos, municipios }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error al obtener los datos");
            }
        }


        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> ADJUNTOS
        
        public ActionResult _Adjuntos(long ID_ROS = 0)
        {
            ViewBag.ID_ROS = ID_ROS;


            return PartialView("_Adjuntos");
        }


        /// <summary>
        /// OBTENER LISTA DE DOCUMENTOS ADJUNTOS  ////
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="sortBy"></param>
        /// <param name="direction"></param>
        /// <param name="searchString"></param>
        /// <param name="ID_ROS"></param>
        /// <returns></returns>
        public JsonResult GetAdjuntos(int? page, int? limit, string sortBy, string direction, string searchString = null, long ID_ROS = 0)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _adjuntosRosBlo.GetAll()
                              .Where(x => x.ID_ROS == ID_ROS).Select(z => new { z.ID, z.ID_ROS, z.NOMBRE, z.TIPO })
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


        /// <summary>
        /// ELIMINAR DOCUMENTO DE LA BASE DE DATOS ///////////
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RemoveAdjuntos(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _adjuntosRosBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _adjuntosRosBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// GUARDAR DOCUMENTO EN LA BASE DE DATOS EN FORMATO BINARIO ////
        /// </summary>
        /// <param name="Archivos"></param>
        /// <param name="ID_ROS"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveAdjuntos(HttpPostedFileBase Archivos, long ID_ROS)
        {
            MON_ROS_ARCHIVOS ArchivoAdjunto = new MON_ROS_ARCHIVOS();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _adjuntosRosBlo.ValidarPermiso(SEG_PERMISO.AGREGAR);
                String ExtensionArchivo = Path.GetExtension(Archivos.FileName).ToUpper();


                if (ExtensionArchivo == ".PDF" || ExtensionArchivo == ".DOC" || ExtensionArchivo == ".DOCX" || ExtensionArchivo == ".XLS" || ExtensionArchivo == ".XLSX" || ExtensionArchivo == ".PNG" || ExtensionArchivo == ".JPG" || ExtensionArchivo == ".GIF")
                {
                    Stream str = Archivos.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] Adjunto = Br.ReadBytes((Int32)str.Length);

                    ArchivoAdjunto.ID_ROS = ID_ROS;
                    ArchivoAdjunto.NOMBRE = Archivos.FileName;
                    ArchivoAdjunto.ARCHIVO = Adjunto;
                    ArchivoAdjunto.TIPO = ExtensionArchivo;

                    _adjuntosRosBlo.Save(ArchivoAdjunto);
                }
                else
                {
                    mensaje = "Formato de Archivo no válido. Los tipos de archivo permitidos son: .PDF, .DOC, .DOCX, .XLS, .XLSX, .PNG, ,JPEG,.JPG, .GIF";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// DESCARGAR DOCUMENTO VINCULADO A UN ROS //
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>       
        public FileResult DescargarArchivo(long id)
        {
            _adjuntosRosBlo.ValidarPermiso(SEG_PERMISO.DESCARGAR);

            MON_ROS_ARCHIVOS archivo = new MON_ROS_ARCHIVOS();
            try
            {
                archivo = _adjuntosRosBlo.GetById(id);
                return File(archivo.ARCHIVO, archivo.TIPO, archivo.NOMBRE);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }

        }


        #endregion


        #region |||||||||||||||||| REPORTE ROS |||||||||

        [HttpGet]
        public ActionResult Reporte(long idRos, string formato)
        {
            Dictionary<string, object> parametersData = new Dictionary<string, object>
            {
                { "idRos", idRos.ToString() }
            };

            try
            {

                string nombreReporte = "RptRos";
                string nombreTabla = "RptRosDatos";

                var detalles = _reportesBlo.Ros(idRos);
                var actores = _reportesBlo.actorRos(idRos);
                var operaciones = _reportesBlo.operacionRos(idRos);

                if ((detalles == null || !detalles.Any()) && (actores == null || !actores.Any()) && (operaciones == null || !operaciones.Any()))
                {
                    return Content("No hay datos disponibles para el reporte.");
                }

                DataTable dtReporte = new DataTable(nombreTabla);
                dtReporte.Columns.Add("CODIGO_UIF", typeof(string));
                dtReporte.Columns.Add("FECHA_ELABORACION", typeof(DateTime));
                dtReporte.Columns.Add("DESCRIPCION_OPERACION_REPORTADA", typeof(string));
                dtReporte.Columns.Add("ANALISIS_CASO", typeof(string));

                dtReporte.Columns.Add("ID_ROS", typeof(long));
                dtReporte.Columns.Add("RELACION_CON_SO", typeof(string));
                dtReporte.Columns.Add("FECHA_NACIMIENTO", typeof(DateTime));
                dtReporte.Columns.Add("PRIMER_NOMBRE", typeof(string));
                dtReporte.Columns.Add("SEGUNDO_NOMBRE", typeof(string));
                dtReporte.Columns.Add("PRIMER_APELLIDO", typeof(string));
                dtReporte.Columns.Add("SEGUNDO_APELLIDO", typeof(string));
                dtReporte.Columns.Add("APELLIDO_CASADA", typeof(string));
                dtReporte.Columns.Add("TIPO_PERSONA", typeof(string));
                dtReporte.Columns.Add("GENERO", typeof(string));
                dtReporte.Columns.Add("ID_ESTADO_FAMILIAR", typeof(string));
                dtReporte.Columns.Add("ID_ACTIVIDAD_ECONOMICA", typeof(int));
                dtReporte.Columns.Add("ID_NACIONALIDAD", typeof(int));
                dtReporte.Columns.Add("ID_PAIS", typeof(int));
                dtReporte.Columns.Add("ID_DEPARTAMENTO", typeof(int));
                dtReporte.Columns.Add("ID_MUNICIPIO", typeof(int));
                dtReporte.Columns.Add("ID_URBANIZACION", typeof(int));
                dtReporte.Columns.Add("ID_TIPO_DOCUMENTO", typeof(int));
                dtReporte.Columns.Add("ID_PROFESION", typeof(int));
                dtReporte.Columns.Add("NUMERO_DOCUMENTO", typeof(string));
                dtReporte.Columns.Add("DIRECCION_ESPECIFICA", typeof(string));
                dtReporte.Columns.Add("CIVIL", typeof(string));
                dtReporte.Columns.Add("ECONOMICA", typeof(string));
                dtReporte.Columns.Add("NACIONALIDAD", typeof(string));
                dtReporte.Columns.Add("PAIS", typeof(string));
                dtReporte.Columns.Add("DEPARTAMENTO", typeof(string));
                dtReporte.Columns.Add("MUNICIPIO", typeof(string));
                dtReporte.Columns.Add("URBANIZACION", typeof(string));
                dtReporte.Columns.Add("TDOCUMENTO", typeof(string));
                dtReporte.Columns.Add("PROFESION", typeof(string));

                dtReporte.Columns.Add("ID_TRANSACCION", typeof(string));
                dtReporte.Columns.Add("ID_AGENCIA_BANCO", typeof(int));
                dtReporte.Columns.Add("FECHA_OPERACION", typeof(DateTime));
                dtReporte.Columns.Add("NOMBRE_CLIENTE", typeof(string));
                dtReporte.Columns.Add("NUMERO_PRESTAMO", typeof(long));
                dtReporte.Columns.Add("VALOR_PAGADO", typeof(decimal));
                dtReporte.Columns.Add("DIRECCION_FINANCIERA", typeof(string));
                dtReporte.Columns.Add("PROCEDENCIA_DESTINO", typeof(string));
                dtReporte.Columns.Add("RAZON_ROS", typeof(string));
                dtReporte.Columns.Add("ID_TIPO_PRODUCTO", typeof(string));
                dtReporte.Columns.Add("AGENCIABANCO", typeof(string));
                dtReporte.Columns.Add("BANCO", typeof(string));


                foreach (var detalle in detalles)
                {
                    DataRow row = dtReporte.NewRow();
                    // Datos del actor
                    row["CODIGO_UIF"] = detalle.CODIGO_UIF ?? "N/A";
                    row["FECHA_ELABORACION"] = detalle.FECHA_ELABORACION;
                    row["DESCRIPCION_OPERACION_REPORTADA"] = detalle.DESCRIPCION_OPERACION_REPORTADA ?? "N/A";
                    row["ANALISIS_CASO"] = detalle.ANALISIS_CASO ?? "N/A";

                    dtReporte.Rows.Add(row);

                }

                foreach (var actor in actores)
                {
                    DataRow row = dtReporte.NewRow();
                    // Solo datos del actor
                    row["ID_ROS"] = actor.ID_ROS;
                    row["RELACION_CON_SO"] = actor.RELACION_CON_SO ?? "N/A";
                    row["FECHA_NACIMIENTO"] = actor.FECHA_NACIMIENTO ?? DateTime.MinValue;
                    row["PRIMER_NOMBRE"] = actor.PRIMER_NOMBRE ?? "N/A";
                    row["SEGUNDO_NOMBRE"] = actor.SEGUNDO_NOMBRE ?? "N/A";
                    row["PRIMER_APELLIDO"] = actor.PRIMER_APELLIDO ?? "N/A";
                    row["SEGUNDO_APELLIDO"] = actor.SEGUNDO_APELLIDO ?? "N/A";
                    row["APELLIDO_CASADA"] = actor.APELLIDO_CASADA ?? "N/A";
                    row["TIPO_PERSONA"] = actor.TIPO_PERSONA ?? "N/A";
                    row["GENERO"] = actor.GENERO ?? "N/A";
                    row["ID_ESTADO_FAMILIAR"] = actor.ID_ESTADO_FAMILIAR ?? "N/A";
                    row["ID_ACTIVIDAD_ECONOMICA"] = actor.ID_ACTIVIDAD_ECONOMICA ?? 0;
                    row["ID_NACIONALIDAD"] = actor.ID_NACIONALIDAD ?? 0;
                    row["ID_PAIS"] = actor.ID_PAIS ?? 0;
                    row["ID_DEPARTAMENTO"] = actor.ID_DEPARTAMENTO ?? 0;
                    row["ID_MUNICIPIO"] = actor.ID_MUNICIPIO ?? 0;
                    row["ID_URBANIZACION"] = actor.ID_URBANIZACION ?? 0;
                    row["ID_TIPO_DOCUMENTO"] = actor.ID_TIPO_DOCUMENTO ?? 0;
                    row["ID_PROFESION"] = actor.ID_PROFESION ?? 0;
                    row["NUMERO_DOCUMENTO"] = actor.NUMERO_DOCUMENTO ?? "N/A";
                    row["DIRECCION_ESPECIFICA"] = actor.DIRECCION_ESPECIFICA ?? "N/A";
                    row["CIVIL"] = actor.CIVIL ?? "N/A";
                    row["ECONOMICA"] = actor.ECONOMICA ?? "N/A";
                    row["NACIONALIDAD"] = actor.NACIONALIDAD ?? "N/A";
                    row["PAIS"] = actor.PAIS ?? "N/A";
                    row["DEPARTAMENTO"] = actor.DEPARTAMENTO ?? "N/A";
                    row["MUNICIPIO"] = actor.MUNICIPIO ?? "N/A";
                    row["URBANIZACION"] = actor.URBANIZACION ?? "N/A";
                    row["TDOCUMENTO"] = actor.TDOCUMENTO ?? "N/A";
                    row["PROFESION"] = actor.PROFESION ?? "N/A";
                    // (Resto de columnas del actor...)
                    // Las columnas de operación se quedan vacías o "N/A"
                    dtReporte.Rows.Add(row);
                }
                    
                 foreach (var operacion in operaciones)
                 {
                    DataRow row = dtReporte.NewRow();
                    // Datos del actor
                    row["ID_TRANSACCION"] = operacion.ID_TRANSACCION ?? "N/A";
                    row["ID_AGENCIA_BANCO"] = operacion.ID_AGENCIA_BANCO;
                    row["FECHA_OPERACION"] = operacion.FECHA_OPERACION;
                    row["NOMBRE_CLIENTE"] = operacion.NOMBRE_CLIENTE ?? "N/A";
                    row["NUMERO_PRESTAMO"] = operacion.NUMERO_PRESTAMO;
                    row["VALOR_PAGADO"] = operacion.VALOR_PAGADO;
                    row["DIRECCION_FINANCIERA"] = operacion.DIRECCION_FINANCIERA ?? "N/A";
                    row["PROCEDENCIA_DESTINO"] = operacion.PROCEDENCIA_DESTINO ?? "N/A";
                    row["RAZON_ROS"] = operacion.RAZON_ROS ?? "N/A";
                    row["ID_TIPO_PRODUCTO"] = operacion.ID_TIPO_PRODUCTO ?? "N/A";
                    row["AGENCIABANCO"] = operacion.AGENCIABANCO ?? "N/A";
                    row["BANCO"] = operacion.BANCO ?? "N/A";

                    dtReporte.Rows.Add(row);

                 }

                if (dtReporte == null || dtReporte.Rows.Count == 0)
                    throw new Exception("El DataTable está vacío...");

                VerReporte(nombreReporte, formato, parametersData, dtReporte, nombreTabla);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        #endregion
    }
}

