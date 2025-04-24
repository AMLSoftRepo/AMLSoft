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
    public class AgenciaBancariaBlo : GenericBlo<MON_AGENCIA_BANCARIA>,IAgenciaBancariaBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IAgenciaBancariaDao _agenciaBancariaDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public AgenciaBancariaBlo(IAgenciaBancariaDao agenciaBancariaDao)
            : base(agenciaBancariaDao)
        {
            _agenciaBancariaDao = agenciaBancariaDao;
        }
    }
}
