using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Matriz;

namespace Blo.Matriz
{
    public class CatFactorRiesgoBlo : GenericBlo<MAT_CAT_FACTOR_RIESGO>, ICatFactorRiesgoBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ICatFactorRiesgoDao _catFactorRiesgoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CatFactorRiesgoBlo(ICatFactorRiesgoDao catFactorRiesgoDao)
            : base(catFactorRiesgoDao)
        {
            _catFactorRiesgoDao = catFactorRiesgoDao;
        }
    }
}
