using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Seguridad
{
    public interface IRolUsuarioDao : IGenericDao<SEG_ROL_USUARIO>
    {
        /// <summary>
        /// Obtiene el rol de un usario
        /// </summary>
        /// <param name="user">codigo de usuario</param>
        /// <returns>Lista de SEG_ROL_USUARIO</returns>
        List<SEG_ROL_USUARIO> GetRolUsuario(string user);
    }
}
