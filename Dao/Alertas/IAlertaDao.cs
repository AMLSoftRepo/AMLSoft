using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Alertas
{
    public interface IAlertaDao : IGenericDao<ALE_ALERTA>
    {
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
        IQueryable<dynamic> GetAlertasNotificadas(out int total, int? page, int? limit, string sortBy, string direction, string estado, int tipoAlerta = 0, string searchString = null, Nullable<DateTime> fechaInicio = null, Nullable<DateTime> fechaFin = null);


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
        IQueryable<dynamic> GetAlertasHistorial(out int total, int? page, int? limit, string sortBy, string direction, int tipoAlerta = 0, string searchString = null, Nullable<DateTime> fechaInicio = null, Nullable<DateTime> fechaFin = null);


        /// <summary>
        /// Metodo que permite obtener  el total de las alertas en una determinada fecha y un tipo de alerta
        /// </summary>
        /// <param name="fecha">Fecha en que se genera la alerta</param>
        /// <param name="tipoAlerta">Tipo de lerta</param>
        /// <returns>Numero de lertas</returns>
        int getAlertasxFechaxTipoAlerta(DateTime fecha, int tipoAlerta);


        /// <summary>
        /// Metodo que se ejecuta cada minuto para notificar al usuario
        /// el número de alertas notificadas
        /// </summary>
        /// <returns>int cantidad de alertas notificadas</returns>
        int NotificarAlertas();


        /// <summary>
        // Metodo que se ejecuta cada minuto para notificar al usuario
        /// el número de alertas pendientes de analizar
        /// </summary>
        /// <returns>int cantidad de alertas pendientes de analizar</returns>
        int NotificarAlertasAnalizar();

    }
}
