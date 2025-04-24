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
    public class SDNDireccionBlo : GenericBlo<LIS_SDN_DIRECCION>, ISDNDireccionBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ISDNDireccionDao _sdnDireccionDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public SDNDireccionBlo(ISDNDireccionDao sdnDireccionDao)
            : base(sdnDireccionDao)
        {
            _sdnDireccionDao = sdnDireccionDao;
        }
    }
}
