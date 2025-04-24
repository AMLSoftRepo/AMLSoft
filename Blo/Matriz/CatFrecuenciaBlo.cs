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
    public class CatFrecuenciaBlo : GenericBlo<MAT_CAT_FRECUENCIA>, ICatFrecuenciaBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ICatFrecuenciaDao _catFrecuenciaDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CatFrecuenciaBlo(ICatFrecuenciaDao catFrecuenciaDao)
            : base(catFrecuenciaDao)
        {
            _catFrecuenciaDao = catFrecuenciaDao;
        }
    }
}
