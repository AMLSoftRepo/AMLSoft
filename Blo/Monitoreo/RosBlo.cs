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
    public class RosBlo:GenericBlo<MON_ROS>,IRosBlo 
    {
                /// <summary>
        /// Instancia de la clase
        /// </summary>        
        private IRosDao _rosDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>        
        [Inject]
        public RosBlo(IRosDao rosDao)
            : base(rosDao)
        {
            _rosDao = rosDao;
        }

        public List<MON_ROS> GetCodUIF(string codigo)
        {
            List<MON_ROS> lista = new List<MON_ROS>();

            try
            {
                lista = _rosDao.GetCodUIF(codigo);
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de codigos de ROS UIF", e);
            }

            return lista;
        }
    }
}
