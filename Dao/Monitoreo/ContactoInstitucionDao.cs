using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Monitoreo
{
    public class ContactoInstitucionDao : GenericDao<MON_CONTACTO_INSTITUCION>, IContactoInstitucionDao
    {

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
                listDestinatarios = _SQLBDEntities.MON_CONTACTO_INSTITUCION.AsNoTracking()
                                .Where(x => x.MON_CARGO_INSTITUCION.MON_CAT_INSTITUCION.ID != idInstitucion)
                                .ToList();
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
                listRemitentes = _SQLBDEntities.MON_CONTACTO_INSTITUCION
                    .Where(x => x.MON_CARGO_INSTITUCION.MON_CAT_INSTITUCION.ID == idInstitucion)
                    .ToList();
            }
            catch (Exception ex)
            {
                log.Error("Error al listar los remitentes", ex);
            }
            return listRemitentes;
        }

    }
}
