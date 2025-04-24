using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Blo.Seguridad
{
    public interface IRolBlo : IGenericBlo<SEG_ROL>
    {
        /// <summary>
        /// Metodo que permite guadar los roles con los privilegios asignados
        /// </summary>
        /// <param name="data"> objeto de SEG_ROL</param>
        /// <param name="idsPermisos">lista de ids de permisos asignados </param>
        void SaveRoles(SEG_ROL data, int[] idsPermisos);
    }
}
