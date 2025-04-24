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
    public class PersonalizadaAliasBlo : GenericBlo<LIS_PERSONALIZADA_ALIAS>, IPersonalizadaAliasBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IPersonalizadaAliasDao _personalizadaAliasDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public PersonalizadaAliasBlo(IPersonalizadaAliasDao personalizadaAliasDao)
            : base(personalizadaAliasDao)
        {
            _personalizadaAliasDao = personalizadaAliasDao;
        }
    }
}
