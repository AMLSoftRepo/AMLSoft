using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Dao.Seguridad;
using Ninject;

namespace Blo.Seguridad
{
    public class RolPermisoBlo : GenericBlo<SEG_ROL_PERMISO>, IRolPermisoBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IRolPermisoDao _rolPermisoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public RolPermisoBlo(IRolPermisoDao rolPermisoDao)
            : base(rolPermisoDao)
        {
            _rolPermisoDao = rolPermisoDao;
        }
    }
}
