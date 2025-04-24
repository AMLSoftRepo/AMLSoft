using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Monitoreo
{
    public class PersonasOficioDao: GenericDao<MON_OFICIO_PERSONA>,IPersonasOficioDao
    {

        /// <summary>
        /// Metodo que permite buscar datos de las personas 
        /// que estan en los documentos de oficios
        /// </summary>
        /// <param name="textoBuscar">datos a buscar en personas</param>
        /// <returns>Lista de IDs de ofcios en donde se encuentran las personas</returns>
        public List<long> BuscarPersonasEnOficios(string textoBuscar)
        {
            List<long> idsOficios = new List<long>();
            try
            {
                idsOficios = _SQLBDEntities.MON_OFICIO_PERSONA
                             .Where(x => (
                                    x.NOMBRE + " " +
                                    x.DUI + " " +
                                    x.NIT + " " +
                                    x.NUMERO_DOCUMENTO
                                   ).ToUpper().Contains(textoBuscar))
                               .Select(x => x.ID_OFICIO)
                               .ToList();
            }
            catch (Exception ex)
            {
                log.Error("Error al buscar datos de personas en oficio", ex);
            }
            return idsOficios;
        }

        public List<long> Get(string textoBuscar)
        {
            List<long> idsOficios = new List<long>();
            try
            {
                idsOficios = _SQLBDEntities.MON_OFICIO_PERSONA
                             .Where(x => (
                                    x.NOMBRE + " " +
                                    x.DUI + " " +
                                    x.NIT + " " +
                                    x.NUMERO_DOCUMENTO
                                   ).ToUpper().Contains(textoBuscar))
                               .Select(x => x.ID_OFICIO)
                               .ToList();
            }
            catch (Exception ex)
            {
                log.Error("Error al buscar datos de personas en oficio", ex);
            }
            return idsOficios;
        }

    }
}
