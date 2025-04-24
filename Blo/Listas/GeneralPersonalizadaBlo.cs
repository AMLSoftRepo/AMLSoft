using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Listas;

namespace Blo.Listas
{
    public class GeneralPersonalizadaBlo : GenericBlo<LIS_GENERAL_PERSONALIZADA>, IGeneralPersonalizadaBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IGeneralPersonalizadaDao _generalPersonalizadaDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public GeneralPersonalizadaBlo(IGeneralPersonalizadaDao generalPersonalizadaDao)
            : base(generalPersonalizadaDao)
        {
            _generalPersonalizadaDao = generalPersonalizadaDao;
        }

        /// <summary>
        /// Metodo que permite eliminar las relaciones de una lista general con personas
        /// </summary>
        /// <param name="idLista">Identificador único de LISTA GENERAL</param>  
        public void EliminarRelacionesLista(long idLista)
        {
            try
            {
                var listRelaciones = _generalPersonalizadaDao.GetAll()
                    .Where(x => x.ID_LISTA_GENERAL == idLista).ToList();

                foreach (var item in listRelaciones)
                    _generalPersonalizadaDao.Remove(item.ID);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception("Error eliminando relaciones de listas", ex);
            }
        }

        /// <summary>
        /// Metodo que permite eliminar las relaciones de una lista general con personas
        /// </summary>
        /// <param name="idPersona">Identificador único de LISTA PERSONALIZADA</param>  
        public void EliminarRelacionesPersona(long idPersona)
        {
            try
            {
                var listRelaciones = _generalPersonalizadaDao.GetAll()
                    .Where(x => x.ID_LISTA_PERSONALIZADA == idPersona).ToList();

                foreach (var item in listRelaciones)
                    _generalPersonalizadaDao.Remove(item.ID);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception("Error eliminando relaciones de personas", ex);
            }
        }

        /// <summary>
        /// Metodo que permite eliminar una persona en una lista
        /// </summary>
        /// <param name="idLista">Identificador único de Lista</param>
        /// <param name="idPersona">Identificador único de Persona</param>
        public void EliminarPersonaEnLista(long idLista, long idPersona)
        {
            try
            {
                var listPersonaEnLista = _generalPersonalizadaDao.GetAll()
                        .Where(x => x.ID_LISTA_GENERAL == idLista && x.ID_LISTA_PERSONALIZADA == idPersona)
                        .ToList();

                foreach (var item in listPersonaEnLista)
                    _generalPersonalizadaDao.Remove(item.ID);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception("Error eliminando persona de lista", ex);
            }
        }
    }
}
