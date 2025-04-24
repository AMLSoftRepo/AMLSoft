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
    public class ONUDireccionBlo : GenericBlo<LIS_ONU_DIRECCION>, IONUDireccionBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IONUDireccionDao _onuDireccionDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public ONUDireccionBlo(IONUDireccionDao onuDireccionDao)
            : base(onuDireccionDao)
        {
            _onuDireccionDao = onuDireccionDao;
        }
    }
}
