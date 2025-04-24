using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Ninject;
using Dao.Seguridad;
using System.Transactions;

namespace Blo.Seguridad
{
    public class RolBlo : GenericBlo<SEG_ROL>, IRolBlo
    {
        /// <summary>
        /// Instancia de la clase
        /// </summary>
        private IRolDao _rolDao;
        private IPermisoDao _permisoDao;
        private IRolPermisoDao _rolPermisoDao;

        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        [Inject]
        public RolBlo(IRolDao rolDao, IPermisoDao permisoDao, IRolPermisoDao rolPermisoDao)
            : base(rolDao)
        {
            _rolDao = rolDao;
            _permisoDao = permisoDao;
            _rolPermisoDao = rolPermisoDao;

        }


        /// <summary>
        /// Metodo que permite guadar los roles con los privilegios asignados
        /// </summary>
        /// <param name="data"> objeto de SEG_ROL</param>
        /// <param name="idsPermisos">lista de ids de permisos asignados </param>
        public void SaveRoles(SEG_ROL data, int[] idsPermisos)
        {
            List<long> listRolPermiso = new List<long>();
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _rolDao.Save(data);

                    listRolPermiso = _rolPermisoDao.GetAll().Where(x => x.ID_ROL == data.ID).Select(x => x.ID).ToList();
                    foreach (long item in listRolPermiso)
                        _rolPermisoDao.Remove(item);

                    if (idsPermisos != null)
                        foreach (int idPermiso in idsPermisos)
                        {
                            SEG_ROL_PERMISO permisos = new SEG_ROL_PERMISO();
                            permisos.ID_PERMISO = idPermiso;
                            permisos.ID_ROL = data.ID;

                            _rolPermisoDao.Save(permisos);
                        }


                    scope.Complete();
                }
                catch (Exception e)
                {
                    log.Error("Error guardando Permisos-Rol: ", e);
                    throw new Exception("Error guardando Permisos-Rol:", e);
                }
            }
        }

    }
}
