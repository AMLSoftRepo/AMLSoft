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
    public class SDNDocumentoBlo : GenericBlo<LIS_SDN_DOCUMENTO>, ISDNDocumentoBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ISDNDocumentoDao _sdnDocumentoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public SDNDocumentoBlo(ISDNDocumentoDao sdnDocumentoDao)
            : base(sdnDocumentoDao)
        {
            _sdnDocumentoDao = sdnDocumentoDao;
        }
    }
}
