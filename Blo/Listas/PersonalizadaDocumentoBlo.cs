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
    public class PersonalizadaDocumentoBlo : GenericBlo<LIS_PERSONALIZADA_DOCUMENTO>, IPersonalizadaDocumentoBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IPersonalizadaDocumentoDao _personalizadaDocumentoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public PersonalizadaDocumentoBlo(IPersonalizadaDocumentoDao personalizadaDocumentoDao)
            : base(personalizadaDocumentoDao)
        {
            _personalizadaDocumentoDao = personalizadaDocumentoDao;
        }
    }
}
