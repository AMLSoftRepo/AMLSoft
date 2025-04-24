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
    public class ContactoInstitucionBlo : GenericBlo<MON_CONTACTO_INSTITUCION>, IContactoInstitucionBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IContactoInstitucionDao _contactoInstitucionDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public ContactoInstitucionBlo(IContactoInstitucionDao contactoInstitucionDao)
            : base(contactoInstitucionDao)
        {
            _contactoInstitucionDao = contactoInstitucionDao;
        }

        /// <summary>
        /// Metodo que permite obtener todos los destinatarios para el documento
        /// de oficios, ademas de excluir la institución predeterminada
        /// </summary>
        /// <param name="idInstitucion">Identificador único de MON_CONTACTO_INSTITUCION</param>
        /// <returns>Lista de contatos </returns>
        public List<MON_CONTACTO_INSTITUCION> Destinatarios(int idInstitucion)
        {
            List<MON_CONTACTO_INSTITUCION> listDestinatarios = new List<MON_CONTACTO_INSTITUCION>();
            try
            {
                listDestinatarios = _contactoInstitucionDao.Destinatarios(idInstitucion);
            }
            catch (Exception ex)
            {
                log.Error("Error al listar los destinatarios", ex);
            }
            return listDestinatarios;
        }


        /// <summary>
        /// Metodo que permite obtener todos los remitentes para el documento
        /// de oficios, donde la institución es igual a la institución predeterminada
        /// </summary>
        /// <param name="idInstitucion">Identificador único de MON_CONTACTO_INSTITUCION</param>
        /// <returns>Lista de contatos </returns>
        public List<MON_CONTACTO_INSTITUCION> Remitentes(int idInstitucion)
        {
            List<MON_CONTACTO_INSTITUCION> listRemitentes = new List<MON_CONTACTO_INSTITUCION>();
            try
            {
                listRemitentes = _contactoInstitucionDao.Remitentes(idInstitucion);
            }
            catch (Exception ex)
            {
                log.Error("Error al listar los remitentes", ex);
            }
            return listRemitentes;
        }
    }
}
