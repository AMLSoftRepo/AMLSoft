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
    public class PEPCargoBlo : GenericBlo<LIS_PEP_CARGO>, IPEPCargoBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IPEPCargoDao _pepCargoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public PEPCargoBlo(IPEPCargoDao pepCargoDao)
            : base(pepCargoDao)
        {
            _pepCargoDao = pepCargoDao;
        }
    }
}
