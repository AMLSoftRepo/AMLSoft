using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Dao.Seguridad;
using Ninject;

namespace Blo.Seguridad
{
    public class AccesoUsuarioBlo : GenericBlo<SEG_ACCESO_USUARIO>, IAccesoUsuarioBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IAccesoUsuarioDao _accesoUsuarioDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public AccesoUsuarioBlo(IAccesoUsuarioDao accesoUsuarioDao)
            : base(accesoUsuarioDao)
        {
            _accesoUsuarioDao = accesoUsuarioDao;
        }

        /// <summary>
        /// Obtiene una lista de opciones filtradas por rol
        /// </summary>
        /// <param name="rol">ID del rol</param>
        /// <returns>Lista SEG_OPCION</returns>
        public List<SEG_OPCION> GetOpcionesxPerfil(int rol)
        {
            var lista = _accesoUsuarioDao.GetOpcionesxPerfil(rol);

            return lista;
        }

        /// <summary>
        /// Obtiene una lista accesos
        /// </summary>
        /// <param name="modulo">ID del modulo</param>
        /// <param name="perfil">ID del rol</param>
        /// <param name="acceso">Estado del activo o inactivo</param>
        /// <returns>Lista</returns>
        public List<SEG_ACCESO_USUARIO> GetOpcionesxPerfil(int modulo, int perfil, bool acceso)
        {
            var lista = _accesoUsuarioDao.GetOpcionesxPerfil(modulo, perfil, acceso);

            return lista;
        }
    }
}
