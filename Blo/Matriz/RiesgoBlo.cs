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
    public class RiesgoBlo : GenericBlo<MAT_RIESGO>, IRiesgoBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IRiesgoDao _riesgoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public RiesgoBlo(IRiesgoDao riesgoDao)
            : base(riesgoDao)
        {
            _riesgoDao = riesgoDao;
        }
    }
}
