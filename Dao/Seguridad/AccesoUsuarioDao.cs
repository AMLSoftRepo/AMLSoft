using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Dao.Seguridad
{
    public class AccesoUsuarioDao : GenericDao<SEG_ACCESO_USUARIO>, IAccesoUsuarioDao
    {
        /// <summary>
        /// Obtiene una lista de opciones filtradas por rol
        /// </summary>
        /// <param name="rol">ID del rol</param>
        /// <returns>Lista SEG_OPCION</returns>
        public List<SEG_OPCION> GetOpcionesxPerfil(int rol)
        {
            List<SEG_OPCION> lista = new List<SEG_OPCION>();
            List<int> ids = new List<int>();

            try
            {
                ids = _SQLBDEntities.SEG_ACCESO_USUARIO
                                    .Where(x => x.ID_ROL == rol && x.ACCESO)
                                    .Select(x => x.ID_OPCION)
                                    .ToList();

                lista = _SQLBDEntities.SEG_OPCION
                                      .Where(x => ids.Contains(x.ID)).ToList();
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de opciones por perfil", e);
            }

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
            List<SEG_ACCESO_USUARIO> listaOpciones = new List<SEG_ACCESO_USUARIO>();
            try
            {
                var accesos = (from a in _SQLBDEntities.SEG_ACCESO_USUARIO
                               where a.ID_ROL == perfil && a.ID_MODULO == modulo 
                               select a).ToList();

                var opciones = (from o in _SQLBDEntities.SEG_OPCION
                                where o.ID_MODULO == modulo
                                select o).ToList();

                //left join
                var lista = (from o in opciones
                             join a in accesos
                             on new { o.ID_MODULO, o.ID }
                             equals new { a.ID_MODULO, ID = a.ID_OPCION } into oa
                             from oanew in oa.DefaultIfEmpty()
                             select new
                             {
                                 ID = oanew == null ? 0 : oanew.ID,
                                 o.ID_MODULO,
                                 ID_OPCION = o.ID,
                                 ID_ROL = oanew == null ? 0 : oanew.ID_ROL,
                                 ACCESO = oanew == null ? false : oanew.ACCESO
                             }).Where(x => x.ACCESO == acceso).ToList();

                //llenar lista
                foreach (var item in lista)
                {
                    SEG_ACCESO_USUARIO neww = new SEG_ACCESO_USUARIO();
                    neww.ID = item.ID;
                    neww.ID_MODULO = item.ID_MODULO;
                    neww.ID_OPCION = item.ID_OPCION;
                    neww.ID_ROL = item.ID_ROL;
                    neww.ACCESO = item.ACCESO;

                    SEG_OPCION opcion = (from o in _SQLBDEntities.SEG_OPCION
                                         where o.ID == item.ID_OPCION
                                         select o).First();

                    neww.SEG_OPCION = opcion;

                    listaOpciones.Add(neww);
                }
            }
            catch (Exception e)
            {
                log.Error("Error cargando listado de opciones por modulo,perfil y acceso", e);
            }

            return listaOpciones.ToList();
        }
    }
}
