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
    public class CatTipoRiesgoBlo : GenericBlo<MAT_CAT_TIPO_RIESGO>, ICatTipoRiesgoBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ICatTipoRiesgoDao _catTipoRiesgoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CatTipoRiesgoBlo(ICatTipoRiesgoDao catTipoRiesgoDao)
            : base(catTipoRiesgoDao)
        {
            _catTipoRiesgoDao = catTipoRiesgoDao;
        }
    }
}
