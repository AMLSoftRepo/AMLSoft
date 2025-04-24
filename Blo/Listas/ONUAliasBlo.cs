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
    public class ONUAliasBlo : GenericBlo<LIS_ONU_ALIAS>, IONUAliasBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IONUAliasDao _onuAliasDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public ONUAliasBlo(IONUAliasDao onuAliasDao)
            : base(onuAliasDao)
        {
            _onuAliasDao = onuAliasDao;
        }
    }
}
