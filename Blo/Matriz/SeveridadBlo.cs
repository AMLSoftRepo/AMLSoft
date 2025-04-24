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
    public class SeveridadBlo : GenericBlo<MAT_SEVERIDAD>, ISeveridadBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private ISeveridadDao _severidadDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public SeveridadBlo(ISeveridadDao severidadDao)
            : base(severidadDao)
        {
            _severidadDao = severidadDao;
        }
    }
}
