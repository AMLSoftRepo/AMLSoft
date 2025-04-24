using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Dao.Matriz;
using Ninject;

namespace Blo.Matriz
{
    public class EventoRiesgoBlo : GenericBlo<MAT_EVENTO_RIESGO>, IEventoRiesgoBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IControlDao _controlDao;
        private IEventoRiesgoDao _eventoRiesgoDao;
        private IControlEventoDao _controlEventoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public EventoRiesgoBlo(IEventoRiesgoDao eventoRiesgoDao, IControlEventoDao controlEventoDao, IControlDao controlDao)
            : base(eventoRiesgoDao)
        {
            _eventoRiesgoDao = eventoRiesgoDao;
            _controlEventoDao = controlEventoDao;
            _controlDao = controlDao;
        }


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
        public IQueryable<dynamic> GetEventos(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                return _eventoRiesgoDao.GetEventos(out total, page, limit, sortBy, direction, searchString);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }


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
        public IQueryable<dynamic> GetControles(out int total, int? page, int? limit, string searchString = null, long idEvento = 0)
        {
            try
            {
                return _eventoRiesgoDao.GetControles(out total, page, limit, searchString, idEvento);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }


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
        public IQueryable<dynamic> GetSeleccionControles(out int total, int? page, int? limit, string searchString = null, long idEvento = 0)
        {
            try
            {
                return _eventoRiesgoDao.GetSeleccionControles(out total, page, limit, searchString, idEvento);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }


    }
}
