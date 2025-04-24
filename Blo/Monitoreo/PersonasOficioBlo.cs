using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Monitoreo;

namespace Blo.Monitoreo
{
    public class PersonasOficioBlo : GenericBlo<MON_OFICIO_PERSONA>, IPersonasOficioBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IPersonasOficioDao _personasOficioDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public PersonasOficioBlo(IPersonasOficioDao personasOficioDao)
            : base(personasOficioDao)
        {
            _personasOficioDao = personasOficioDao;
        }

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
                idsOficios = _personasOficioDao.BuscarPersonasEnOficios(textoBuscar);
            }
            catch (Exception ex)
            {
                log.Error( "Error al buscar datos de personas en oficio",ex);
            }
            return idsOficios;
        }

    }
}
