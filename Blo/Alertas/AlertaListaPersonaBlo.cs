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
    public class AlertaListaPersonaBlo : GenericBlo<ALE_ALERTA_LISTA_PERSONA>, IAlertaListaPersonaBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>        
        private IAlertaListaPersonaDao _alertaListaPersonaDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>        
        [Inject]
        public AlertaListaPersonaBlo(IAlertaListaPersonaDao alertaListaPersonaDao)
            : base(alertaListaPersonaDao)
        {
            _alertaListaPersonaDao = alertaListaPersonaDao;
        }


        /// <summary>
        /// Metodo que permite listar y buscar clientes que se encontraron en las listas internacionales
        /// como en las personalizadas.
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <returns>Lista de clientes en listas</returns>
        public IQueryable<dynamic> GetAlertaListaPersona(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                return _alertaListaPersonaDao.GetAlertaListaPersona(out total, page, limit, sortBy, direction, searchString);
            }
            catch (Exception e)
            {
                log.Error("Error al obtener lista de clientes en listas: " + e);
                throw new Exception("Error al obtener lista de clientes en listas");
            }
        }


        /// <summary>
        /// Metodo que obtiene el total de clientes en listas
        /// </summary>
        /// <returns>Numero de clientes en listas</returns>
        public int NotificarListas()
        {
            return _alertaListaPersonaDao.NotificarListas();
        }

    }
}
