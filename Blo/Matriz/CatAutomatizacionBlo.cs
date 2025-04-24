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
    public class CatAutomatizacionBlo : GenericBlo<MAT_CAT_AUTOMATIZACION>, ICatAutomatizacionBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ICatAutomatizacionDao _catAutomatizacionDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CatAutomatizacionBlo(ICatAutomatizacionDao catAutomatizacionDao)
            : base(catAutomatizacionDao)
        {
            _catAutomatizacionDao = catAutomatizacionDao;
        }
    }
}
