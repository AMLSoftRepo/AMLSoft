using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Alertas
{
    public interface IAlertaListaPersonaBlo : IGenericBlo<ALE_ALERTA_LISTA_PERSONA>
    {
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
        IQueryable<dynamic> GetAlertaListaPersona(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null);


        /// <summary>
        /// Metodo que obtiene el total de clientes en listas
        /// </summary>
        /// <returns>Numero de clientes en listas</returns>
        int NotificarListas();

    }
}
