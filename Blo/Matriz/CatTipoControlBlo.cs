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
    public class CatTipoControlBlo : GenericBlo<MAT_CAT_TIPO_CONTROL>, ICatTipoControlBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ICatTipoControlDao _catTipoControlDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CatTipoControlBlo(ICatTipoControlDao catTipoControlDao)
            : base(catTipoControlDao)
        {
            _catTipoControlDao = catTipoControlDao;
        }
    }
}
