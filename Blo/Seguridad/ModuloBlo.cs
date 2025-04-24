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
    public class ModuloBlo : GenericBlo<SEG_MODULO>, IModuloBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IModuloDao _moduloDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public ModuloBlo(IModuloDao moduloDao)
            : base(moduloDao)
        {
            _moduloDao = moduloDao;
        }
    }
}
