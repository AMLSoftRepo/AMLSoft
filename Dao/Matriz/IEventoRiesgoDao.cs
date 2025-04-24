using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Matriz
{
    public interface IEventoRiesgoDao : IGenericDao<MAT_EVENTO_RIESGO>
    {
        /// <summary>
        /// Metodo donde realiza el calculo del riesgo inherente
        /// Formula: Probabilidad * Impacto o Severidad
        /// </summary>
        ///<param name="probabilidad">Nivel de probabilidad</param>
        ///<param name="impacto">Nivel de impacto</param>
        /// <returns>Valor de RiesgoInherente</returns>
        decimal RiesgoInherente(decimal probabilidad, decimal impacto);

        /// <summary>
        /// Metodo donde realiza el calculo de la eficacia del control
        /// Formula: PromedioTotal de los controles aplicados al evento
        /// </summary>
        /// <param name="idEvento">Identificador único de MAT_EVENTO_RIESGO</param>
        /// <returns>Valor de EficaciaControl</returns>
        decimal EficaciaControl(long idEvento);


        /// <summary>
        /// Metodo donde realiza el calculo del riesgo residual
        /// Formula: Riesgo Inherente * (1-Eficacia del Control)
        /// </summary>
        /// <param name="riesgoInherente">Valor de riesgoInherente</param>
        /// <param name="eficaciaControl">Valor de eficaciaControl</param>
        /// <returns>Valor de riesgoResidual</returns>
        decimal RiesgoResidual(decimal riesgoInherente, decimal eficaciaControl);

        /// <summary>
        /// Metodo que permite obtener una lista de eventos.
        /// Ademas realiza la paginación de los datos, permite buscar por todos los campos
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <returns>Lista de eventos</returns>
        IQueryable<dynamic> GetEventos(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null);


        /// <summary>
        /// Metodo que permite obtener una lista de controles.
        /// Ademas realiza la paginación de los datos, permite buscar por todos los campos
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <param name="idEvento">Identificador unico del evento</param>
        /// <returns>Lista de controles</returns>
        IQueryable<dynamic> GetControles(out int total, int? page, int? limit, string searchString = null, long idEvento = 0);


                /// <summary>
        /// Metodo que permite obtener una lista de controles.
        /// Ademas realiza la paginación de los datos, permite buscar por todos los campos
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <param name="idEvento">Identificador unico del evento</param>
        /// <returns>Lista de controles</returns>
        IQueryable<dynamic> GetSeleccionControles(out int total, int? page, int? limit, string searchString = null, long idEvento = 0);
    }
}
