using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Listas;

namespace Blo.Listas
{
    public class ONUDocumentoBlo : GenericBlo<LIS_ONU_DOCUMENTO>, IONUDocumentoBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IONUDocumentoDao _onuDocumentoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public ONUDocumentoBlo(IONUDocumentoDao onuDocumentoDao)
            : base(onuDocumentoDao)
        {
            _onuDocumentoDao = onuDocumentoDao;
        }
    }
}
