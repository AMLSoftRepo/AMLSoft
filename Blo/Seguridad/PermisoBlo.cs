using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Seguridad;

namespace Blo.Seguridad
{
    public class PermisoBlo : GenericBlo<SEG_PERMISO>, IPermisoBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IPermisoDao _permisoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public PermisoBlo(IPermisoDao permisoDao)
            : base(permisoDao)
        {
            _permisoDao = permisoDao;
        }
    }
}
