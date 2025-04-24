using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Listas
{
    public interface IGeneralPersonalizadaBlo : IGenericBlo<LIS_GENERAL_PERSONALIZADA>
    {
        /// <summary>
        /// Metodo que permite eliminar las relaciones de una lista general con personas
        /// </summary>
        /// <param name="idLista">Identificador único de LISTA GENERAL</param>        
        void EliminarRelacionesLista(long idLista);

        /// <summary>
        /// Metodo que permite eliminar las relaciones de una persona con listas generales
        /// </summary>
        /// <param name="idPersona">Identificador único de LISTA PERSONALIZADA</param>        
        void EliminarRelacionesPersona(long idPersona);

        /// <summary>
        /// Metodo que permite eliminar una persona asociada a una lista
        /// </summary>
        /// <param name="idLista">Identificador único de LISTA GENERAL</param>
        /// <param name="idPersona">Identificador único de LISTA PERSONALIZADA</param>
        void EliminarPersonaEnLista(long idLista, long idPersona);
    }
}
