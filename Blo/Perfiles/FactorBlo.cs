using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Perfiles;

namespace Blo.Perfiles
{
    public class FactorBlo : GenericBlo<PER_FACTOR>, IFactorBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IFactorDao _factorDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public FactorBlo(IFactorDao factorDao)
            : base(factorDao)
        {
            _factorDao = factorDao;
        }

    }
}
