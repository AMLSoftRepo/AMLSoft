using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Monitoreo
{
    public interface IPersonasOficioDao : IGenericDao<MON_OFICIO_PERSONA>
    {
        /// <summary>
        /// Metodo que permite buscar datos de las personas 
        /// que estan en los documentos de oficios
        /// </summary>
        /// <param name="textoBuscar">datos a buscar en personas</param>
        /// <returns>Lista de IDs de ofcios en donde se encuentran las personas</returns>
        List<long> BuscarPersonasEnOficios(string textoBuscar);
    }
}
