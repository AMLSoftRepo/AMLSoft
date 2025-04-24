using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Alertas;

namespace Blo.Alertas
{
    public class AlertaBlo : GenericBlo<ALE_ALERTA>, IAlertaBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>        
        private IAlertaDao _alertaDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>        
        [Inject]
        public AlertaBlo(IAlertaDao alertaDao)
            : base(alertaDao)
        {
            _alertaDao = alertaDao;
        }


        /// <summary>
        /// Metodo que permite obtener una lista de alertas con estado de notificadas.
        /// Ademas realiza la paginación de los datos, permite buscar por todos los campos
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="estado">Estado actual de la alerta</param>
        /// <param name="tipoAlerta">Identificador unico de tipo alerta</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista de alertas con estado de notificadas</returns>
        public IQueryable<dynamic> GetAlertasNotificadas(out int total, int? page, int? limit, string sortBy, string direction, string estado, int tipoAlerta = 0, string searchString = null, Nullable<DateTime> fechaInicio = null, Nullable<DateTime> fechaFin = null)
        {
            try
            {
                return _alertaDao.GetAlertasNotificadas(out total, page, limit, sortBy, direction, estado, tipoAlerta, searchString, fechaInicio, fechaFin);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Metodo que permite obtener una lista de alertas en historial
        /// Ademas realiza la paginación de los datos, permite buscar por todos los campos
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="tipoAlerta">Identificador unico de tipo alerta</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <param name="fechaInicio">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista de alertas en historial</returns>
        public IQueryable<dynamic> GetAlertasHistorial(out int total, int? page, int? limit, string sortBy, string direction, int tipoAlerta = 0, string searchString = null, Nullable<DateTime> fechaInicio = null, Nullable<DateTime> fechaFin = null)
        {
            try
            {
                return _alertaDao.GetAlertasHistorial(out total, page, limit, sortBy, direction, tipoAlerta, searchString, fechaInicio, fechaFin);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Metodo que permite obtener  el total de las alertas en una determinada fecha y un tipo de alerta
        /// </summary>
        /// <param name="fecha">Fecha en que se genera la alerta</param>
        /// <param name="tipoAlerta">Tipo de lerta</param>
        /// <returns>Numero de lertas</returns>
        public int getAlertasxFechaxTipoAlerta(DateTime fecha, int tipoAlerta)
        {
            int total = 0;

            try
            {
                total = _alertaDao.getAlertasxFechaxTipoAlerta(fecha, tipoAlerta);
            }
            catch (Exception e)
            {
                log.Error("Error al obtener el total de alertas por fecha y tipo de alerta", e);
            }

            return total;
        }


        /// <summary>
        /// Metodo que se ejecuta cada minuto para notificar al usuario
        /// el número de alertas notificadas
        /// </summary>
        /// <returns>int cantidad de alertas notificadas</returns>
        public int NotificarAlertas()
        {
            return _alertaDao.NotificarAlertas();
        }


        /// <summary>
        // Metodo que se ejecuta cada minuto para notificar al usuario
        /// el número de alertas pendientes de analizar
        /// </summary>
        /// <returns>int cantidad de alertas pendientes de analizar</returns>
        public int NotificarAlertasAnalizar()
        {
            return _alertaDao.NotificarAlertasAnalizar();
        }

    }
}
