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
    public class CatUnidadBlo : GenericBlo<MAT_CAT_UNIDAD>, ICatUnidadBlo
    {
        /// Instancia de la clase
        /// </summary>
        private ICatUnidadDao _catUnidadDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CatUnidadBlo(ICatUnidadDao catUnidadDao)
            : base(catUnidadDao)
        {
            _catUnidadDao = catUnidadDao;
        }
    }
}
