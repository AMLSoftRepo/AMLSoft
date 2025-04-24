using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Monitoreo;

namespace Blo.Monitoreo
{
   public  class OperacionesRosBlo:GenericBlo<MON_ROS_OPERACION>,IOperacionesRosBlo
    {
         /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IOperacionesRosDao _OperacionesRosDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public OperacionesRosBlo(IOperacionesRosDao operacionesRosDao)
            : base(operacionesRosDao)
        {
            _OperacionesRosDao = operacionesRosDao;
        }
    }
}
