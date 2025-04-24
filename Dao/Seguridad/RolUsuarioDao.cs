using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Seguridad
{
    public class RolUsuarioDao : GenericDao<SEG_ROL_USUARIO>, IRolUsuarioDao
    {
        /// <summary>
        /// Obtiene el rol de un usario
        /// </summary>
        /// <param name="user">codigo de usuario</param>
        /// <returns>Lista de SEG_ROL_USUARIO</returns>
        public List<SEG_ROL_USUARIO> GetRolUsuario(string user)
        {
            List<SEG_ROL_USUARIO> lista = new List<SEG_ROL_USUARIO>();

            try
            {
                lista = _SQLBDEntities.SEG_ROL_USUARIO
                                    .Where(x => x.USUARIO == user)
                                    .ToList();
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de roles de usurio", e);
            }

            return lista;
        }
    }
}
