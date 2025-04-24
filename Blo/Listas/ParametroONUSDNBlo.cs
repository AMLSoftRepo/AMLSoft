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
    public class ParametroONUSDNBlo : GenericBlo<LIS_PARAMETRO_ONU_SDN>, IParametroONUSDNBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IParametroONUSDNDao _parametroONUSDNDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public ParametroONUSDNBlo(IParametroONUSDNDao parametroONUSDNDao)
            : base(parametroONUSDNDao)
        {
            _parametroONUSDNDao = parametroONUSDNDao;
        }
    }
}
