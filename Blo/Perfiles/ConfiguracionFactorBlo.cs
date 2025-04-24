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
    public class ConfiguracionFactorBlo : GenericBlo<PER_CONFIGURACION_FACTOR>, IConfiguracionFactorBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IConfiguracionFactorDao _configuracionFactorDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public ConfiguracionFactorBlo(IConfiguracionFactorDao configuracionFactorDao)
            : base(configuracionFactorDao)
        {
            _configuracionFactorDao = configuracionFactorDao;
        }

    }
}
