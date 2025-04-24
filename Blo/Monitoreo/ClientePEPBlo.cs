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
    public class ClientePEPBlo : GenericBlo<MON_CLIENTE_PEP>, IClientePEPBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IClientePEPDao _clientePEPDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public ClientePEPBlo(IClientePEPDao clientePEPDao)
            : base(clientePEPDao)
        {
            _clientePEPDao = clientePEPDao;
        }
    }
}
