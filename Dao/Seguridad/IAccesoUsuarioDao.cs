using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Seguridad
{
    public interface IAccesoUsuarioDao : IGenericDao<SEG_ACCESO_USUARIO>
    {
        /// <summary>
        /// Obtiene una lista de opciones filtradas por rol
        /// </summary>
        /// <param name="rol">ID del rol</param>
        /// <returns>Lista SEG_OPCION</returns>
         List<SEG_OPCION> GetOpcionesxPerfil(int rol);

        /// <summary>
        /// Obtiene una lista accesos
        /// </summary>
        /// <param name="modulo">ID del modulo</param>
        /// <param name="perfil">ID del rol</param>
        /// <param name="acceso">Estado del activo o inactivo</param>
        /// <returns>Lista</returns>
         List<SEG_ACCESO_USUARIO> GetOpcionesxPerfil(int modulo, int perfil, bool acceso);
    }
}
