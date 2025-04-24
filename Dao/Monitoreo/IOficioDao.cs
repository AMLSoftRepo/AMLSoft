using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Monitoreo
{
    public interface IOficioDao : IGenericDao<MON_OFICIO>
    {
        /// <summary>
        /// Metodo que permite obtener una lista de oficios pendientes de recibir una respuesta de la UIF.
        /// Ademas realiza la paginación de los datos, permite buscar numero de oficio o referencia y busca
        /// datos de las personas como lo son nombre y documentos el resultado sera una lista de oficios.
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <returns>Lista de oficios</returns>
        IQueryable<dynamic> GetOficios(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null);


        /// <summary>
        /// Metodo que permite obtener una lista de oficios Procesados y con respuesta UIF.
        /// Ademas realiza la paginación de los datos, permite buscar numero de oficio o referencia y busca
        /// datos de las personas como lo son nombre y documentos el resultado sera una lista de oficios.
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="searchString">Permite buscar por todos los campos</param>
        /// <returns>Lista de oficios Historicos</returns>
        IQueryable<dynamic> GetHistorialOficios(out int total, int? page, int? limit, string sortBy, string direction, string searchString = null);


        /// <summary>
        /// Metodo que permite obtener el total de oficios pendientes de procesar y
        /// donde la fecha actual mas los dias aplicados en configuración sea mayor
        /// o igual a la fecha maxima UIF. Entonces sera notificada en el sistema.
        /// </summary>
        /// <returns>Int total de oficios</returns>
        int GetTotalNotificarOficio();


        /// <summary>
        /// Metodo que obtiene una lista de oficios pendientes de procesar, 
        /// con fecha de recibido UIF igual a null y filtradas por un rango de fecha
        /// en este caso seria el establecido por el plugin del calendario.
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista de oficios</returns>
        List<MON_OFICIO> GetCalendarioOficios(DateTime fechaIni, DateTime fechaFin);


        /// <summary>
        /// Metodo que obtiene una lista de oficios en HISTORIAL, 
        /// con fecha de recibido UIF igual a null y filtradas por un rango de fecha
        /// en este caso seria el establecido por el plugin del calendario.
        /// </summary>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns>Lista de oficios</returns>
        List<MON_OFICIO> GetHistorialCalendarioOficios(DateTime fechaIni, DateTime fechaFin);

    }
}
