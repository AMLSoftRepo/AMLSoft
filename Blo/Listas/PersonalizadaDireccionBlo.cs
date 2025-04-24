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
    public class PersonalizadaDireccionBlo : GenericBlo<LIS_PERSONALIZADA_DIRECCION>, IPersonalizadaDireccionBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IPersonalizadaDireccionDao _personalizadaDireccionDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public PersonalizadaDireccionBlo(IPersonalizadaDireccionDao personalizadaDireccionDao)
            : base(personalizadaDireccionDao)
        {
            _personalizadaDireccionDao = personalizadaDireccionDao;
        }
    }
}
