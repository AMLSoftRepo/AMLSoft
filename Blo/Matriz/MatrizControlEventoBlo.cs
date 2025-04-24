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
    public class MatrizControlEventoBlo : GenericBlo<MAT_MATRIZ_CONTROL_EVENTO>, IMatrizControlEventoBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IMatrizControlEventoDao _matrizControlEventoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public MatrizControlEventoBlo(IMatrizControlEventoDao matrizControlEventoDao)
            : base(matrizControlEventoDao)
        {
            _matrizControlEventoDao = matrizControlEventoDao;
        }
    }
}
