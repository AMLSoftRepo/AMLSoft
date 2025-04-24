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
    public class SDNAkaBlo : GenericBlo<LIS_SDN_AKA>, ISDNAkaBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ISDNAkaDao _sdnAkaDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public SDNAkaBlo(ISDNAkaDao sdnAkaDao)
            : base(sdnAkaDao)
        {
            _sdnAkaDao = sdnAkaDao;
        }
    }
}
