using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blo.Alertas;
using Blo.Monitoreo;
using Blo.Properties;
using Model;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Data;

namespace View.Controllers.Monitoreo
{
    public class BaseControlPagosController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IAlertaBlo _alertaBlo;
        private readonly IDatosAdicionalesTransaccionBlo _datosAdicionalesTransaccionBlo;
        private readonly ITipoAlertaBlo _tipoAlertaBlo;
        private readonly IContactoAlertaBlo _contactoAlertaBlo;
        private readonly IBloqueoBlo _bloqueoBlo;
        private readonly IAgenciaBancariaBlo _agenciaBancariaBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public BaseControlPagosController
            (IAlertaBlo alertaBlo, IDatosAdicionalesTransaccionBlo datosAdicionalesBlo, ITipoAlertaBlo tipoAlertaBlo,
            IContactoAlertaBlo contactoAlertaBlo, IBloqueoBlo bloqueoBlo,IAgenciaBancariaBlo agenciaBancariaBlo)
        {
            _alertaBlo = alertaBlo;
            _datosAdicionalesTransaccionBlo = datosAdicionalesBlo;
            _tipoAlertaBlo = tipoAlertaBlo;
            _contactoAlertaBlo = contactoAlertaBlo;
            _bloqueoBlo = bloqueoBlo;
            _agenciaBancariaBlo = agenciaBancariaBlo;
        }


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ALERTAS


        [HttpGet]
        public JsonResult GetTransaccion(string SECUENCIA)
        {
            try
            {
                var records = _SQLBDEntities.VIEW_TRANSACCIONES.AsNoTracking()
                              .Where(x => x.SECUENCIA == SECUENCIA)
                              .AsQueryable();

                return Json(new { records }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        //Parcial para mostrar datos del cliente
        public ActionResult _Cliente(long codigoCliente)
        {
            try
            {
                int edad = 0;
                var record = _SQLBDEntities.VIEW_CLIENTE.AsNoTracking()
                    .Where(x => x.CODIGO_CLIENTE == codigoCliente)
                    .ToList();

                if (record.Any())
                    if (record.FirstOrDefault().FECHA_DE_NACIMIENTO != null)
                        edad = DateTime.Today.AddTicks(-record.FirstOrDefault().FECHA_DE_NACIMIENTO.Value.Ticks).Year - 1;

                ViewBag.edad = edad;
                ViewBag.cliente = record;

                return PartialView("_Cliente");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista de datos");
            }
        }


        [HttpPost]
        public JsonResult CambiarEstado(string secuencia, string estado, string prestamo)
        {
            ALE_BLOQUEO_PRESTAMO newBloqueo = new ALE_BLOQUEO_PRESTAMO();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                //se elimina uno o varios registros esto para validar que solo exita un unico 
                //registro con estdo de ANALIZADA y de esta forma evitar conflictos.
                foreach (var item in _bloqueoBlo.GetDatosDetalle("SECUENCIA", secuencia))
                    _bloqueoBlo.Remove(item.ID);

                //Se agrega solo cuando el estado es ANALIZADA caso contrario no debe agregarse
                if (estado.Trim() == "ANALIZADA")
                {
                    newBloqueo.ESTADO = estado;
                    newBloqueo.PRESTAMO = prestamo;
                    newBloqueo.SECUENCIA = secuencia;
                    _bloqueoBlo.Save(newBloqueo);
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
        public JsonResult BloquarDesbloquear(string SECUENCIA, string PRESTAMO, int BLOQUEO, string OBSERVACION)
        {
            ALE_BLOQUEO_PRESTAMO newBloqueo = new ALE_BLOQUEO_PRESTAMO();
            string mensaje = PropertiesBlo.msgExito;

            string usuarioIdentity = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            usuarioIdentity = usuarioIdentity.Split(new string[] { "\\" }, 2, StringSplitOptions.None)[1];

            try
            {
                ObjectParameter resultado = new ObjectParameter("MENSAJE", "");
                _SQLBDEntities.SP_BLOQUEAR_PRESTAMO(PRESTAMO, usuarioIdentity, BLOQUEO, OBSERVACION, resultado);

                if (resultado.Value.ToString() == "")
                {
                    if (BLOQUEO == 1)
                    {
                        //se elimina uno o varios registros esto para validar que solo exita un unico 
                        //registro con estdo de ANALIZADA y de esta forma evitar conflictos.
                        foreach (var item in _bloqueoBlo.GetDatosDetalle("SECUENCIA", SECUENCIA))
                            _bloqueoBlo.Remove(item.ID);

                        newBloqueo.PRESTAMO = PRESTAMO;
                        newBloqueo.SECUENCIA = SECUENCIA;
                        newBloqueo.ESTADO = "PENDIENTE";
                        _bloqueoBlo.Save(newBloqueo);
                    }
                    else
                    {
                        _bloqueoBlo.EliminarPrestamoBloqueado(PRESTAMO);
                    }

                }
                else
                    mensaje = BLOQUEO == 1 ? "Error al 'BLOQUEAR' el prestamo" : "Error al 'DESBLOQUEAR' el prestamo";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> DATOS ADICIONALES


        public ActionResult _DatosAdicionalesTransaccion(string SECUENCIA = null)
        {
            ViewBag.SECUENCIA = SECUENCIA;
            ViewBag.ESTADOS_CIVILES = _SQLBDEntities.VIEW_ESTADO_CIVIL.ToList();
            ViewBag.PROFESIONES = _SQLBDEntities.VIEW_PROFESIONES.ToList();
            ViewBag.departamentos = _SQLBDEntities.VIEW_DEPARTAMENTO.Where(x => x.CODIGO_PAIS == 1).OrderBy(x => x.NOMBRE).ToList();
            ViewBag.agenciasBancarias = _agenciaBancariaBlo.GetAll().OrderBy(x => x.DESCRIPCION).ToList();

            return PartialView("_DatosAdicionalesTransaccion");
        }


        [HttpGet]
        public JsonResult GetDatosAdicionalesTransaccion(int? page, int? limit, string searchString = null, string SECUENCIA = null)
        {
            try
            {
                int total;
                int start = (page.Value - 1) * limit.Value;
                var records = _datosAdicionalesTransaccionBlo.GetDatosAdicionalesPorSecuencia(SECUENCIA);

                total = records.Count();
                records = records.OrderBy(y => y.SECUENCIA).Skip(start).Take(limit.Value).ToList();

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }


        [HttpPost]
        public JsonResult SaveDatosAdicionalesTransaccion(ALE_DATOS_ADICIONALES_TRANSACCION data)
        {
            ALE_DATOS_ADICIONALES_TRANSACCION datosAdicionalesTransaccion = new ALE_DATOS_ADICIONALES_TRANSACCION();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _datosAdicionalesTransaccionBlo.ValidarSave(data.ID);

                if (data.ID != 0)
                    datosAdicionalesTransaccion = _datosAdicionalesTransaccionBlo.GetById(data.ID);

                datosAdicionalesTransaccion.SECUENCIA = data.SECUENCIA;
                datosAdicionalesTransaccion.PERT_NOMBRE = data.PERT_NOMBRE;
                datosAdicionalesTransaccion.PERT_PRIMER_APELLIDO = data.PERT_PRIMER_APELLIDO;
                datosAdicionalesTransaccion.PERT_SEGUNDO_APELLIDO = data.PERT_SEGUNDO_APELLIDO;
                datosAdicionalesTransaccion.PERT_APELLIDO_CASADA = data.PERT_APELLIDO_CASADA;
                datosAdicionalesTransaccion.PERT_DUI = data.PERT_DUI;
                datosAdicionalesTransaccion.PERT_NIT = data.PERT_NIT;
                datosAdicionalesTransaccion.PERT_NACIONALIDAD = data.PERT_NACIONALIDAD;
                datosAdicionalesTransaccion.PERT_DEPARTAMENTO = data.PERT_DEPARTAMENTO;
                datosAdicionalesTransaccion.PERT_MUNICIPIO = data.PERT_MUNICIPIO;
                datosAdicionalesTransaccion.PERT_DOMICILIO = data.PERT_DOMICILIO;
                datosAdicionalesTransaccion.PERT_FECHA_NACIMIENTO = data.PERT_FECHA_NACIMIENTO;
                datosAdicionalesTransaccion.PERT_LUGAR_NACIMIENTO = data.PERT_LUGAR_NACIMIENTO;
                datosAdicionalesTransaccion.PERT_ESTADO_CIVIL = data.PERT_ESTADO_CIVIL;
                datosAdicionalesTransaccion.PERT_PROFESION = data.PERT_PROFESION;
                datosAdicionalesTransaccion.TRAN_CODIGO_CAJERO = data.TRAN_CODIGO_CAJERO;
                datosAdicionalesTransaccion.TRAN_DEPARTAMENTO = data.TRAN_DEPARTAMENTO;
                datosAdicionalesTransaccion.TRAN_MUNICIPIO = data.TRAN_MUNICIPIO;
                datosAdicionalesTransaccion.TRAN_ORIGEN_FONDOS = data.TRAN_ORIGEN_FONDOS;
                datosAdicionalesTransaccion.INFO_DATOS_RECIBO = data.INFO_DATOS_RECIBO;
                datosAdicionalesTransaccion.INFO_CALIDAD_IMPRESION = data.INFO_CALIDAD_IMPRESION;
                datosAdicionalesTransaccion.INFO_LLENO_DECLARACION = data.INFO_LLENO_DECLARACION;
                datosAdicionalesTransaccion.INFO_MODALIDAD_PAGO = data.INFO_MODALIDAD_PAGO;
                datosAdicionalesTransaccion.INFO_VERSION_DECLARACION_JURADA = data.INFO_VERSION_DECLARACION_JURADA;
                datosAdicionalesTransaccion.INFO_QUIEN_LLENO_DECLARACION_JURADA = data.INFO_QUIEN_LLENO_DECLARACION_JURADA;
                datosAdicionalesTransaccion.INFO_DOCUMENTACION_REQUERIDA = data.INFO_DOCUMENTACION_REQUERIDA;
                datosAdicionalesTransaccion.INFO_TELEFONO_CONTACTO = data.INFO_TELEFONO_CONTACTO;
                datosAdicionalesTransaccion.INFO_SEGUIMIENTO = data.INFO_SEGUIMIENTO;
                datosAdicionalesTransaccion.CLIE_LUGAR_NACIMIENTO = data.CLIE_LUGAR_NACIMIENTO;
                datosAdicionalesTransaccion.CLIE_DEPARTAMENTO = data.CLIE_DEPARTAMENTO;
                datosAdicionalesTransaccion.CLIE_MUNICIPIO = data.CLIE_MUNICIPIO;
                datosAdicionalesTransaccion.TRAN_AGENCIA_BANCARIA = data.TRAN_AGENCIA_BANCARIA;
                datosAdicionalesTransaccion.INFO_FECHA_CONFIRMACION_UIF = data.INFO_FECHA_CONFIRMACION_UIF;
                datosAdicionalesTransaccion.INFO_CONFIRMACION_UIF = data.INFO_CONFIRMACION_UIF;
                datosAdicionalesTransaccion.INFO_COLECTOR = data.INFO_COLECTOR;

                _datosAdicionalesTransaccionBlo.Save(datosAdicionalesTransaccion);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RemoveDatosAdicionalesTransaccion(int id)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                _datosAdicionalesTransaccionBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _datosAdicionalesTransaccionBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult MunicipiosXDepartamento(int depto)
        {
            try
            {
                var municipios = _SQLBDEntities.VIEW_MUNICIPIO
                         .Where(x => x.CODIGO_DEPARTAMENTO == depto && x.CODIGO_PAIS == 1)
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

        #endregion


        #region |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| ACCIONES ===>>> ENVIO DE CORREO


        /// <summary>
        /// Metodo que permite enviar correos
        /// </summary>
        /// <param name="contenido">texto que se desea enviar</param>
        /// <param name="asunto">asunto del correo</param>
        /// <param name="correos">correos separados por ';' </param>
        /// <param name="contactos">Códigos de usuarios de ACTIVE DIRECTORY</param>
        /// <returns>Json con mensaje</returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SendNotification(String contenido, String asunto, String correos, String contactos)
        {
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                string[] idContactos = contactos.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                string correosContactos = string.Join(";", _contactoAlertaBlo.GetAll()
                                                           .Where(x => idContactos.Contains(x.CODIGO))
                                                           .Select(x => x.CORREO));

                _SQLBDEntities.SP_EMVIOCORREO(asunto, contenido, correosContactos + "; " + correos);
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