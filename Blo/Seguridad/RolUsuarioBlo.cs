using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Seguridad;

namespace Blo.Seguridad
{
    public class RolUsuarioBlo : GenericBlo<SEG_ROL_USUARIO>, IRolUsuarioBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IRolUsuarioDao _rolUsuarioDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public RolUsuarioBlo(IRolUsuarioDao rolUsuarioDao)
            : base(rolUsuarioDao)
        {
            _rolUsuarioDao = rolUsuarioDao;
        }

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
                lista = _rolUsuarioDao.GetRolUsuario(user);
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de roles de usurio", e);
            }

            return lista;
        }
    }
}
