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
    public class MatrizControlBlo : GenericBlo<MAT_MATRIZ_CONTROL>, IMatrizControlBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IMatrizControlDao _matrizControlDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public MatrizControlBlo(IMatrizControlDao matrizControlDao)
            : base(matrizControlDao)
        {
            _matrizControlDao = matrizControlDao;
        }
    }
}
