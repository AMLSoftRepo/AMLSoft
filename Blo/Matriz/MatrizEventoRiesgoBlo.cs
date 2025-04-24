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
    public class MatrizEventoRiesgoBlo : GenericBlo<MAT_MATRIZ_EVENTO_RIESGO>, IMatrizEventoRiesgoBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IMatrizEventoRiesgoDao _matrizEventoRiesgoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public MatrizEventoRiesgoBlo(IMatrizEventoRiesgoDao matrizEventoRiesgoDao)
            : base(matrizEventoRiesgoDao)
        {
            _matrizEventoRiesgoDao = matrizEventoRiesgoDao;
        }
    }
}
