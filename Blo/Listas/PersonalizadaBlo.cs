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
    public class PersonalizadaBlo : GenericBlo<LIS_PERSONALIZADA>, IPersonalizadaBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IPersonalizadaDao _personalizadaDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public PersonalizadaBlo(IPersonalizadaDao personalizadaDao)
            : base(personalizadaDao)
        {
            _personalizadaDao = personalizadaDao;
        }
    }
}
