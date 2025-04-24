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
    public class CatDisenoBlo : GenericBlo<MAT_CAT_DISENO>, ICatDisenoBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>        
        private ICatDisenoDao _catDisenoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>        
        [Inject]
        public CatDisenoBlo(ICatDisenoDao catDisenoDao)
            : base(catDisenoDao)
        {
            _catDisenoDao = catDisenoDao;
        }
    }
}
