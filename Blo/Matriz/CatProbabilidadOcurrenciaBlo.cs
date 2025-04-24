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
    public class CatProbabilidadOcurrenciaBlo : GenericBlo<MAT_CAT_PROBABILIDAD_OCURRENCIA>, ICatProbabilidadOcurrenciaBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ICatProbabilidadOcurrenciaDao _catProbabilidadOcurrenciaDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CatProbabilidadOcurrenciaBlo(ICatProbabilidadOcurrenciaDao catProbabilidadOcurrenciaDao)
            : base(catProbabilidadOcurrenciaDao)
        {
            _catProbabilidadOcurrenciaDao = catProbabilidadOcurrenciaDao;
        }
    }
}
