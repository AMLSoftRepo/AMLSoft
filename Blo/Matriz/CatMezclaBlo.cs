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
    public class CatMezclaBlo : GenericBlo<MAT_CAT_MEZCLA>, ICatMezclaBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ICatMezclaDao _catMezclaDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CatMezclaBlo(ICatMezclaDao catMezclaDao)
            : base(catMezclaDao)
        {
            _catMezclaDao = catMezclaDao;
        }
    }
}
