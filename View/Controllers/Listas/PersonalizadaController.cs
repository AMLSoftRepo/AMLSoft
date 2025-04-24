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
    public class PersonalizadaController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IPersonalizadaBlo _personalizadaBlo;
        private readonly IPersonalizadaDocumentoBlo _personalizadaDocumentoBlo;
        private readonly IPersonalizadaDireccionBlo _personalizadaDireccionBlo;
        private readonly IPersonalizadaAliasBlo _personalizadaAliasBlo;
        private readonly IGeneralPersonalizadaBlo _generalPersonalizadaBlo;
        private readonly IGeneralBlo _generalBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public PersonalizadaController(IPersonalizadaBlo personalizadaBlo, IGeneralPersonalizadaBlo generalPersonalizadaBlo, IPersonalizadaDocumentoBlo personalizadaDocumentoBlo, IPersonalizadaDireccionBlo personalizadaDireccionBlo, IPersonalizadaAliasBlo personalizadaAliasBlo, IGeneralBlo generalBlo)
        {
            _personalizadaBlo = personalizadaBlo;
            _generalPersonalizadaBlo = generalPersonalizadaBlo;
            _personalizadaDocumentoBlo = personalizadaDocumentoBlo;
            _personalizadaDireccionBlo = personalizadaDireccionBlo;
            _personalizadaAliasBlo = personalizadaAliasBlo;
            _generalBlo = generalBlo;
        }

        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> LISTA PERSONALIZADA
        public ActionResult Index(int idgen)
        {
            ViewBag.ID_LISTA_GENERAL = idgen;
            ViewBag.NombreLista = _generalBlo.GetDatosDetalle("ID",idgen).Select(a => a.NOMBRE_LISTA).First();
            ViewBag.nacionalidades = _SQLBDEntities.VIEW_PAISNACIONALIDAD.ToList();
            ViewBag.persona = metodo(idgen);

            return View();
        }

        public Dictionary<string, string> metodo(int idgen)
        {
            Dictionary<string, string> parametersData = new Dictionary<string, string>();

            try
            {
                List<int> idsper = _generalPersonalizadaBlo.GetDatosDetalle("ID_LISTA_GENERAL",idgen)
                    .Select(a => a.ID_LISTA_PERSONALIZADA)
                    .ToList();

                var nombreListas = _generalBlo.GetAll()
                    .Select(b => new
                    {
                        IDLISTA = b.ID,
                        NOMBRELISTA = b.NOMBRE_LISTA
                    }).ToList();

                var personaLista = _generalPersonalizadaBlo.GetAll().Where(c => !idsper.Contains(c.ID_LISTA_PERSONALIZADA))
                    .Select(c => new
                   {
                       IDPERSONA = c.ID_LISTA_PERSONALIZADA,
                       PERSONALISTA = string.Join(",", nombreListas.Where(d => d.IDLISTA == c.ID_LISTA_GENERAL).Select(d => d.NOMBRELISTA).ToList())
                   }
                   ).ToList();

                var records = _personalizadaBlo.GetAll().Where(x => !idsper.Contains(x.ID))
                    .Select(x => new
                    {
                        x.ID,
                        x.ID_UNICO,
                        x.PRIMER_NOMBRE,
                        x.SEGUNDO_NOMBRE,
                        x.TERCER_NOMBRE,
                        x.PRIMER_APELLIDO,
                        x.SEGUNDO_APELLIDO,
                        x.TERCER_APELLIDO,
                        x.PAIS_NACIMIENTO,
                        ENLISTA = string.Join(", ", personaLista.Where(e => e.IDPERSONA == x.ID).Select(e => e.PERSONALISTA).ToList())
                    }
                    ).OrderBy(x => x.PRIMER_NOMBRE).ToList();

                foreach (var item in records)
                {
                    parametersData.Add(item.ID.ToString(), item.PRIMER_NOMBRE + " " + item.SEGUNDO_NOMBRE + " " + item.TERCER_NOMBRE
                        + " " + item.PRIMER_APELLIDO + " " + item.SEGUNDO_APELLIDO + " " + item.TERCER_APELLIDO + "|" + item.ENLISTA);
                }

                return parametersData;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new HttpException(404, null);
            }
        }


        public List<LIS_PERSONALIZADA> GetPersonas(int idgen)
        {
            try
            {
                List<int> idsper = _generalPersonalizadaBlo.GetDatosDetalle("ID_LISTA_GENERAL",idgen)
                    .Select(a => a.ID_LISTA_PERSONALIZADA)
                    .ToList();

                var records = _personalizadaBlo.GetAll().Where(x => !idsper.Contains(x.ID))
                    .OrderBy(x => x.PRIMER_NOMBRE).ToList();

                return records;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpGet]
        public JsonResult GetPersonalizada(int? page, int? limit, string sortBy, string direction, string searchString = null, int ID_LISTA_GENERAL = 0)
        {
            try
            {
                List<int> idsper = _generalPersonalizadaBlo.GetDatosDetalle("ID_LISTA_GENERAL", ID_LISTA_GENERAL)
                    .Select(a => a.ID_LISTA_PERSONALIZADA)
                    .ToList();

                var paises = _SQLBDEntities.VIEW_PAISNACIONALIDAD.ToList();

                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _personalizadaBlo.GetAll(true)
                    .Where(x => idsper.Contains(x.ID))
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.ID,
                        x.ID_UNICO,
                        x.PRIMER_NOMBRE,
                        x.SEGUNDO_NOMBRE,
                        x.TERCER_NOMBRE,
                        NOMBRES = x.PRIMER_NOMBRE + " " + x.SEGUNDO_NOMBRE + " " + x.TERCER_NOMBRE,
                        x.PRIMER_APELLIDO,
                        x.SEGUNDO_APELLIDO,
                        x.TERCER_APELLIDO,
                        APELLIDOS = x.PRIMER_APELLIDO + " " + x.SEGUNDO_APELLIDO + " " + x.TERCER_APELLIDO,
                        x.PAIS_NACIMIENTO,
                        NOMBRE_PAIS_NACIMIENTO = string.Join(" ",
                            _SQLBDEntities.VIEW_PAISNACIONALIDAD
                                .AsEnumerable() // Mueve la consulta a memoria
                                .Where(z => z.CODIGO_PAIS == Convert.ToDecimal(x.PAIS_NACIMIENTO))
                                .Select(y => y.NOMBRE)),
                        x.RAZON
                    }).AsQueryable();

                //Buscar texto en varios campos 
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    records = records
                              .Where(x => (
                                  x.ID_UNICO + " " +
                                  x.NOMBRES + " " +
                                  x.APELLIDOS + " " +
                                  x.NOMBRE_PAIS_NACIMIENTO).ToUpper().Contains(searchString.Trim().ToUpper())
                               ).AsQueryable();
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

        [HttpPost]
        public JsonResult Save(LIS_PERSONALIZADA data, int ID_LISTA_GENERAL)
        {
            Boolean nueva = true;
            string mensaje = PropertiesBlo.msgExito;
            LIS_PERSONALIZADA personalizada = new LIS_PERSONALIZADA();

            try
            {
                _personalizadaBlo.ValidarSave(data.ID);
                if (data.ID != 0)
                {
                    nueva = false;
                    personalizada = _personalizadaBlo.GetById(data.ID);
                }

                personalizada.ID_UNICO = data.ID_UNICO;
                personalizada.PRIMER_NOMBRE = data.PRIMER_NOMBRE;
                personalizada.SEGUNDO_NOMBRE = data.SEGUNDO_NOMBRE;
                personalizada.TERCER_NOMBRE = data.TERCER_NOMBRE;
                personalizada.PRIMER_APELLIDO = data.PRIMER_APELLIDO;
                personalizada.SEGUNDO_APELLIDO = data.SEGUNDO_APELLIDO;
                personalizada.TERCER_APELLIDO = data.TERCER_APELLIDO;
                personalizada.PAIS_NACIMIENTO = data.PAIS_NACIMIENTO;
                personalizada.RAZON = data.RAZON;

                _personalizadaBlo.Save(personalizada);

                if (nueva)
                {
                    LIS_GENERAL_PERSONALIZADA generalPersonalizada = new LIS_GENERAL_PERSONALIZADA();
                    generalPersonalizada.ID_LISTA_GENERAL = ID_LISTA_GENERAL;
                    generalPersonalizada.ID_LISTA_PERSONALIZADA = personalizada.ID;
                    generalPersonalizada.FECHA_RELACION = DateTime.Now;

                    _generalPersonalizadaBlo.Save(generalPersonalizada);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje, personalizada.ID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SavePersonaExistente(int ID_LISTA_GENERAL, int[] ID_PERSONA)
        {
            LIS_GENERAL_PERSONALIZADA generalPersonalizada = new LIS_GENERAL_PERSONALIZADA();
            string mensaje = PropertiesBlo.msgExito;

            foreach (int idpersona in ID_PERSONA)
            {
                generalPersonalizada = new LIS_GENERAL_PERSONALIZADA();
                try
                {
                    _generalPersonalizadaBlo.ValidarSave(ID_LISTA_GENERAL);

                    generalPersonalizada.ID_LISTA_GENERAL = ID_LISTA_GENERAL;
                    generalPersonalizada.FECHA_RELACION = DateTime.Now;

                    generalPersonalizada.ID_LISTA_PERSONALIZADA = idpersona;
                    _generalPersonalizadaBlo.Save(generalPersonalizada);

                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    mensaje = ex.Message;
                }
            }

            return Json(new { mensaje, generalPersonalizada.ID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Remove(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _personalizadaBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _generalPersonalizadaBlo.EliminarRelacionesPersona(id);
                _personalizadaBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarDeLista(int id, int ID_LISTA_GENERAL)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _personalizadaBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _generalPersonalizadaBlo.EliminarPersonaEnLista(ID_LISTA_GENERAL, id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> LISTA PERSONALIZADA DOCUMENTO
        public ActionResult _Documento(int ID_LISTA_PERSONALIZADA = 0)
        {
            ViewBag.ID_LISTA_PERSONALIZADA = ID_LISTA_PERSONALIZADA;
            ViewBag.nacionalidades = _SQLBDEntities.VIEW_PAISNACIONALIDAD.ToList();

            return PartialView("_Documento");
        }

        [HttpGet]
        public JsonResult GetPersonalizadaDocumento(int? page, int? limit, int ID_LISTA_PERSONALIZADA = 0)
        {
            try
            {
                var paises = _SQLBDEntities.VIEW_PAISNACIONALIDAD.ToList();

                int total = 0;
                var records = _personalizadaDocumentoBlo.GetDatosDetalle(out total, page, limit, "TIPO", "asc", "ID_LISTA_PERSONALIZADA", ID_LISTA_PERSONALIZADA)
                    .AsEnumerable()
                    .Select(x => new { 
                        x.ID,
                        x.COMENTARIO,
                        x.ID_LISTA_PERSONALIZADA,
                        x.NUMERO,
                        x.PAIS_EXPEDICION,
                        x.TIPO,
                        NOMBRE_PAIS_EXPEDICION = string.Join(" ",
                            _SQLBDEntities.VIEW_PAISNACIONALIDAD
                            .AsEnumerable()
                            .Where(z => z.CODIGO_PAIS == Convert.ToDecimal(x.PAIS_EXPEDICION))
                            .Select(y => y.NOMBRE))
                     }).ToList();

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpPost]
        public JsonResult SavePersonalizadaDocumento(LIS_PERSONALIZADA_DOCUMENTO data, int ID_LISTA_PERSONALIZADA)
        {
            LIS_PERSONALIZADA_DOCUMENTO personalizadaDocumento = new LIS_PERSONALIZADA_DOCUMENTO();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _personalizadaDocumentoBlo.ValidarSave(data.ID);
                personalizadaDocumento.ID_LISTA_PERSONALIZADA = ID_LISTA_PERSONALIZADA;

                if (data.ID != 0)
                    personalizadaDocumento = _personalizadaDocumentoBlo.GetById(data.ID);

                personalizadaDocumento.TIPO = data.TIPO;
                personalizadaDocumento.NUMERO = data.NUMERO;
                personalizadaDocumento.PAIS_EXPEDICION = data.PAIS_EXPEDICION;
                personalizadaDocumento.COMENTARIO = data.COMENTARIO;

                _personalizadaDocumentoBlo.Save(personalizadaDocumento);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemovePersonalizadaDocumento(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _personalizadaDocumentoBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _personalizadaDocumentoBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> LISTA PERSONALIZADA DIRECCION
        public ActionResult _Direccion(int ID_LISTA_PERSONALIZADA = 0)
        {
            ViewBag.ID_LISTA_PERSONALIZADA = ID_LISTA_PERSONALIZADA;
            ViewBag.nacionalidades = _SQLBDEntities.VIEW_PAISNACIONALIDAD.ToList();
            return PartialView("_Direccion");
        }

        [HttpGet]
        public JsonResult GetPersonalizadaDireccion(int? page, int? limit, int ID_LISTA_PERSONALIZADA = 0)
        {
            try
            {
                var paises = _SQLBDEntities.VIEW_PAISNACIONALIDAD.ToList();

                int total = 0;
                var records = _personalizadaDireccionBlo.GetDatosDetalle(out total, page, limit, null, null, "ID_LISTA_PERSONALIZADA", ID_LISTA_PERSONALIZADA)
                    .AsEnumerable()
                    .Select(a => new
                    {
                        a.ID,
                        a.ID_LISTA_PERSONALIZADA,
                        a.DIRECCION_ESPECIFICA,
                        a.AVENIDA_CALLE_PASAJE,
                        a.ID_URBANIZACION,
                        URBANIZACION = string.Join("", _SQLBDEntities.VIEW_URBANIZACION.Where(b => b.CODIGO_PAIS == a.ID_PAIS && b.CODIGO_DEPARTAMENTO == a.ID_DEPARTAMENTO && b.CODIGO_MUNICIPIO == a.ID_MUNICIPIO && b.CODIGO_SECTOR == a.ID_URBANIZACION).Select(b => b.DESCRIPCION).ToList()),
                        a.ID_MUNICIPIO,
                        MUNICIPIO = string.Join("", _SQLBDEntities.VIEW_MUNICIPIO.Where(b => b.CODIGO_PAIS == a.ID_PAIS && b.CODIGO_DEPARTAMENTO == a.ID_DEPARTAMENTO && b.CODIGO_MUNICIPIO == a.ID_MUNICIPIO).Select(b => b.NOMBRE).ToList()),
                        a.ID_DEPARTAMENTO,
                        DEPARTAMENTO = string.Join("", _SQLBDEntities.VIEW_DEPARTAMENTO.Where(b => b.CODIGO_PAIS == a.ID_PAIS && b.CODIGO_DEPARTAMENTO == a.ID_DEPARTAMENTO).Select(b => b.NOMBRE).ToList()),
                        a.ID_PAIS,
                        PAIS = string.Join("", 
                            _SQLBDEntities.VIEW_PAISNACIONALIDAD
                            .AsEnumerable()
                            .Where(b => b.CODIGO_PAIS == Convert.ToDecimal(a.ID_PAIS))
                            .Select(b => b.NOMBRE).ToList()),
                        a.COMENTARIO
                    }).ToList();

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpPost]
        public JsonResult SavePersonalizadaDireccion(LIS_PERSONALIZADA_DIRECCION data, int ID_LISTA_PERSONALIZADA)
        {
            LIS_PERSONALIZADA_DIRECCION personalizadaDireccion = new LIS_PERSONALIZADA_DIRECCION();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _personalizadaDireccionBlo.ValidarSave(data.ID);
                personalizadaDireccion.ID_LISTA_PERSONALIZADA = ID_LISTA_PERSONALIZADA;

                if (data.ID != 0)
                    personalizadaDireccion = _personalizadaDireccionBlo.GetById(data.ID);

                personalizadaDireccion.DIRECCION_ESPECIFICA = data.DIRECCION_ESPECIFICA;
                personalizadaDireccion.AVENIDA_CALLE_PASAJE = data.AVENIDA_CALLE_PASAJE;
                personalizadaDireccion.ID_URBANIZACION = data.ID_URBANIZACION;
                personalizadaDireccion.ID_MUNICIPIO = data.ID_MUNICIPIO;
                personalizadaDireccion.ID_DEPARTAMENTO = data.ID_DEPARTAMENTO;
                personalizadaDireccion.ID_PAIS = data.ID_PAIS;
                personalizadaDireccion.COMENTARIO = data.COMENTARIO;

                _personalizadaDireccionBlo.Save(personalizadaDireccion);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemovePersonalizadaDireccion(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _personalizadaDireccionBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _personalizadaDireccionBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        // OBTENER DEPARTAMENTOS POR PAIS
        [HttpPost]
        public JsonResult DepartamentosXPais(int pais = 0)
        {
            return this.DepartamentosPorPais(pais);
        }

        // OBTENER MUNICIPIOS POR DEPARTAMENTO
        [HttpPost]
        public JsonResult MunicipiosXDepartamento(int pais = 0, int depto = 0)
        {
            return this.MunicipiosPorDepartamento(pais, depto);
        }

        // OBTENER URBANIZACIONES POR MUNICIPIO
        [HttpPost]
        public JsonResult UrbanizacionesXMunicipio(int pais = 0, int depto = 0, int municipio = 0)
        {
            return this.UrbanizacionesPorMunicipio(pais, depto, municipio);
        }

        [HttpPost]
        public JsonResult GetDetpMuniUrbanizacion(int pais = 0, int depto = 0, int municipio = 0)
        {
            try
            {
                var departamentos = _SQLBDEntities.VIEW_DEPARTAMENTO.AsNoTracking()
                    .Where(x => x.CODIGO_PAIS == pais)
                    .Select(x => new
                    {
                        x.CODIGO_DEPARTAMENTO,
                        x.NOMBRE
                    }).ToList();

                var municipios = _SQLBDEntities.VIEW_MUNICIPIO.AsNoTracking()
                    .Where(x => x.CODIGO_DEPARTAMENTO == depto && x.CODIGO_PAIS == pais)
                    .Select(x => new
                    {
                        x.CODIGO_MUNICIPIO,
                        x.NOMBRE
                    }).ToList();

                var urbanizaciones = _SQLBDEntities.VIEW_URBANIZACION.AsNoTracking()
                    .Where(x => x.CODIGO_MUNICIPIO == municipio && x.CODIGO_DEPARTAMENTO == depto && x.CODIGO_PAIS == pais)
                    .Select(x => new
                    {
                        x.CODIGO_SECTOR,
                        x.DESCRIPCION
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

        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> LISTA PERSONALIZADA ALIAS
        public ActionResult _Alias(int ID_LISTA_PERSONALIZADA = 0)
        {
            ViewBag.ID_LISTA_PERSONALIZADA = ID_LISTA_PERSONALIZADA;
            return PartialView("_Alias");
        }

        [HttpGet]
        public JsonResult GetPersonalizadaAlias(int? page, int? limit, string searchString = null, int ID_LISTA_PERSONALIZADA = 0)
        {
            try
            {
                int total = 0;
                var records = _personalizadaAliasBlo.GetDatosDetalle(out total, page, limit, "ALIAS", "asc", "ID_LISTA_PERSONALIZADA", ID_LISTA_PERSONALIZADA);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpPost]
        public JsonResult SavePersonalizadaAlias(LIS_PERSONALIZADA_ALIAS data, int ID_LISTA_PERSONALIZADA)
        {
            LIS_PERSONALIZADA_ALIAS personalizadaAlias = new LIS_PERSONALIZADA_ALIAS();
            string mensaje = PropertiesBlo.msgExito;

            try
            {
                _personalizadaAliasBlo.ValidarSave(data.ID);
                personalizadaAlias.ID_LISTA_PERSONALIZADA = ID_LISTA_PERSONALIZADA;

                if (data.ID != 0)
                    personalizadaAlias = _personalizadaAliasBlo.GetById(data.ID);

                personalizadaAlias.ALIAS = data.ALIAS;
                personalizadaAlias.CALIDAD = data.CALIDAD;
                personalizadaAlias.COMENTARIO = data.COMENTARIO;

                _personalizadaAliasBlo.Save(personalizadaAlias);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemovePersonalizadaAlias(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _personalizadaAliasBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _personalizadaAliasBlo.Remove(id);
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