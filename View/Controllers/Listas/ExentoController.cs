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
    public class ExentoController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IExentoBlo _exentoBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public ExentoController(IExentoBlo exentoBlo)
        {
            _exentoBlo = exentoBlo;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetExento(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total = 0;

                var records = _exentoBlo.GetExentos(out total,page,limit,sortBy,direction,searchString);

                return Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new System.ArgumentException("Error obteniendo lista los datos");
            }
        }

        [HttpPost]
        public JsonResult Save(int CODIGO_CLIENTE, string COMENTARIO, string MOTIVO)
        {
            String usuarioIdentity = null;
            LIS_EXENTO exento = new LIS_EXENTO();
            string mensaje = PropertiesBlo.msgExito;
            try
            {
                VIEW_CLIENTE cliente = new VIEW_CLIENTE();
                List<VIEW_CLIENTE> listCliente = _SQLBDEntities.VIEW_CLIENTE
                    .AsNoTracking()
                    .Where(x => x.CODIGO_CLIENTE == CODIGO_CLIENTE)
                    .ToList();


                usuarioIdentity = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                if (usuarioIdentity.Contains("\\"))
                    usuarioIdentity = usuarioIdentity.Split(new string[] { "\\" }, 2, StringSplitOptions.None)[1];


                if (listCliente.Any())
                {
                    cliente = listCliente.First();
                    _exentoBlo.ValidarSave((long)cliente.CODIGO_CLIENTE);

                    exento.CODIGO_CLIENTE = (long)cliente.CODIGO_CLIENTE;
                    exento.NUMERO_IDENTIFICACION = cliente.NUMERO_IDENTIFICACION_1;
                    exento.NUMERO_IDENTIFICACION_2 = cliente.NUMERO_IDENTIFICACION_2;
                    exento.NOMBRES = cliente.NOMBRES;
                    exento.PRIMER_APELLIDO = cliente.PRIMER_APELLIDO;
                    exento.SEGUNDO_APELLIDO = cliente.SEGUNDO_APELLIDO;
                    exento.APELLIDO_DE_CASADA = cliente.APELLIDO_DE_CASADA;
                    exento.PAIS_NACIMIENTO = cliente.PAIS;
                    exento.COMENTARIO = COMENTARIO;
                    exento.MOTIVO_INGRESO = MOTIVO;
                    exento.USUARIO = usuarioIdentity;
                    exento.FECHA = DateTime.Now;

                    _exentoBlo.Save(exento);
                }
                else
                    mensaje = PropertiesBlo.msgFallo;
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
                _exentoBlo.ValidarPermiso(SEG_PERMISO.ELIMINAR);
                _exentoBlo.Remove(id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                mensaje = ex.Message;
            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult MostrarCliente(long idCliente)
        {
            string cliente = "";
            try
            {
              var nombreCliente =  _SQLBDEntities.VIEW_CLIENTE.AsNoTracking()
                    .Where(x => x.CODIGO_CLIENTE == idCliente)
                    .Select(x => x.NOMBRES + " " + x.PRIMER_APELLIDO + " " + x.SEGUNDO_APELLIDO + " " + x.APELLIDO_DE_CASADA);

              if (nombreCliente.Any())
                  cliente = nombreCliente.FirstOrDefault();
              else
                  cliente = "No se encontro el cliente: "+ idCliente;
            }
            catch (Exception)
            {
                cliente = "Error al buscar el cliente: " + idCliente;
            }
            return Json(new { cliente }, JsonRequestBehavior.AllowGet);
        }

    }
}