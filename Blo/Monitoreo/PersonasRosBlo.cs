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
    public class PersonasRosBlo:GenericBlo<MON_ROS_ACTOR>,IPersonasRosBlo 
    {

        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IPersonasRosDao _PersonasRosDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public PersonasRosBlo(IPersonasRosDao personasRosDao)
            : base(personasRosDao)
        {
            _PersonasRosDao = personasRosDao;
        }
    }
}
