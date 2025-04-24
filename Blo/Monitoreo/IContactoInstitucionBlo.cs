using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Monitoreo
{
    public interface IContactoInstitucionBlo: IGenericBlo<MON_CONTACTO_INSTITUCION>
    {
        /// <summary>
        /// Metodo que permite obtener todos los destinatarios para el documento
        /// de oficios, ademas de excluir la institución predeterminada
        /// </summary>
        /// <param name="idInstitucion">Identificador único de MON_CONTACTO_INSTITUCION</param>
        /// <returns>Lista de contatos </returns>
        List<MON_CONTACTO_INSTITUCION> Destinatarios(int idInstitucion);

        /// <summary>
        /// Metodo que permite obtener todos los remitentes para el documento
        /// de oficios, donde la institución es igual a la institución predeterminada
        /// </summary>
        /// <param name="idInstitucion">Identificador único de MON_CONTACTO_INSTITUCION</param>
        /// <returns>Lista de contatos </returns>
        List<MON_CONTACTO_INSTITUCION> Remitentes(int idInstitucion);

    }
}
