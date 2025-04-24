using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Dao.Matriz;
using Ninject;

namespace Blo.Matriz
{
    public class CausaRiesgoBlo : GenericBlo<MAT_CAUSA_RIESGO>, ICausaRiesgoBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ICausaRiesgoDao _causaRiesgoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CausaRiesgoBlo(ICausaRiesgoDao causaRiesgoDao)
            : base(causaRiesgoDao)
        {
            _causaRiesgoDao = causaRiesgoDao;
        }

    }
}
