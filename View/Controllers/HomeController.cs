using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Blo.Alertas;
using Blo.Seguridad;
using Blo.Monitoreo;
using Blo.Matriz;

namespace View.Controllers
{
    [NoCache]
    [Autorizacion]
    public class HomeController : BaseController
    {
        /// <summary>
        /// Propiedades que representan el objeto principal de acceso a logica del negocio.
        /// </summary>
        private readonly IRolUsuarioBlo _rolUsuarioBlo;
        private readonly IOficioBlo _oficioBlo;
        private readonly IControlBlo _controlBlo;
        private readonly IAlertaBlo _alertaBlo;
        private readonly ITipoAlertaBlo _tipoAlertaBlo;
        private readonly IAlertaListaPersonaBlo _alertaListaPersonaBlo;

        /// <summary>
        /// Constructor que permite la inyeccion de los objetos de acceso a 
        /// logica del nogocio utilizados por el controlador
        /// </summary>
        public HomeController(IRolUsuarioBlo rolUsuarioBlo, IOficioBlo oficioBlo,
            IControlBlo controlBlo, IAlertaBlo alertaBlo, ITipoAlertaBlo tipoAlertaBlo,
            IAlertaListaPersonaBlo alertaListaPersonaBlo)
        {
            _rolUsuarioBlo = rolUsuarioBlo;
            _oficioBlo = oficioBlo;
            _controlBlo = controlBlo;
            _alertaBlo = alertaBlo;
            _tipoAlertaBlo = tipoAlertaBlo;
            _alertaListaPersonaBlo = alertaListaPersonaBlo;
        }


        public struct dataPorAlerta
        {
            public int idAlerta { get; set; }
            public int valor { get; set; }
        }


        public ActionResult Index()
        {
            List<string> fechas = new List<string>();
            List<dataPorAlerta> dataPorAlerta = new List<dataPorAlerta>();
            StringBuilder dataChart = new StringBuilder();
            DateTime fechaInicio = DateTime.Now.AddDays(- 7);
            DateTime fechaFin = DateTime.Now;
            int diferencia = Math.Abs((fechaInicio - fechaFin).Days);
            int contador = 0;

            do
            {
                foreach (var item in _tipoAlertaBlo.GetAll())
                {
                    dataPorAlerta alerta = new dataPorAlerta();
                    alerta.idAlerta = item.ID;
                    alerta.valor = _alertaBlo.getAlertasxFechaxTipoAlerta(fechaInicio.AddDays(contador).Date, item.ID);
                    dataPorAlerta.Add(alerta);
                }

                fechas.Add( "'" + fechaInicio.AddDays(contador).ToString("dd/MM/yyyy") + "'");
                contador++;
            } while (contador <= diferencia);


            foreach (var item in _tipoAlertaBlo.GetAll())
            {
                dataChart.AppendLine("{");
                dataChart.AppendLine("label:'" + item.DESCRIPCION +"',");
                dataChart.AppendLine("fill:false,");
                dataChart.AppendLine("backgroundColor:'" + item.COLOR.Trim() + "',");
                dataChart.AppendLine("borderColor:'" + item.COLOR.Trim() + "',");
                dataChart.AppendLine("data:[ " + string.Join(",", dataPorAlerta.Where(x => x.idAlerta == item.ID).Select(x=>x.valor)) + "]" );
                dataChart.AppendLine("},");
            }

            //ViewBag.totalUsuarios = _rolUsuarioBlo.GetAll().Count();
            //ViewBag.totalOficios = _oficioBlo.GetAll().Count();
            //ViewBag.totalControles = _controlBlo.GetAll().Count();
            //ViewBag.totalTipoAlertas = _tipoAlertaBlo.GetAll().Count();
            ViewBag.data = dataChart.ToString();
            ViewBag.fechas = string.Join(",", fechas);

            return View();
        }

        /*public ActionResult EnviarCorreo()
        {
            string destinatario = "rivasashly16@gmail.com";
            string asunto = "Prueba de Gmail API en MVC5";
            string mensaje = "<h1>Hola, este es un correo de pueba.</h1>";

            bool enviado = EmailHelper
        }*/

    }
}