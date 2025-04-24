using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Monitoreo;
using System.Data.Entity;

namespace Blo.Monitoreo
{
    public class AdjuntosRosBlo : GenericBlo<MON_ROS_ARCHIVOS>, IAdjuntosRosBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IAdjuntosRosDao _AdjuntosRosDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public AdjuntosRosBlo(IAdjuntosRosDao adjuntosRosDao)
            : base(adjuntosRosDao)
        {
            _AdjuntosRosDao = adjuntosRosDao;
        }

    }
}
