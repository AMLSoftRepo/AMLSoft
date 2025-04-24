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
    public class CatDocumentacionBlo : GenericBlo<MAT_CAT_DOCUMENTACION>, ICatDocumentacionBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>        
        private ICatDocumentacionDao _catDocumentacionDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public CatDocumentacionBlo(ICatDocumentacionDao catDocumentacionDao)
            : base(catDocumentacionDao)
        {
            _catDocumentacionDao = catDocumentacionDao;
        }
    }
}
