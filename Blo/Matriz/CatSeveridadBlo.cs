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
    public class CatSeveridadBlo : GenericBlo<MAT_CAT_SEVERIDAD>, ICatSeveridadBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ICatSeveridadDao _catSeveridadDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CatSeveridadBlo(ICatSeveridadDao catSeveridadDao)
            : base(catSeveridadDao)
        {
            _catSeveridadDao = catSeveridadDao;
        }
    }
}
