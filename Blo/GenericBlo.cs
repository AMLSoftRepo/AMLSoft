using Dao;
using log4net;
using Model;
using Ninject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Transactions;
using System.Web;


namespace Blo
{
    /// <summary>
    /// Clase Base para la logica del negocio (Business Logic Layer - BLL) que 
    /// implementa las interfaces generales de acceso a la logica del negocio, 
    /// permitiendo los métodos generales de: CRUD (Create, Read, Update y Delete).
    /// Implementa el control de transacciones a nivel de servicios con
    /// el objetivo de mantener la integridad de las diferentes acciones
    /// de persistencia.
    /// </summary>
    /// <typeparam name="T">Parámetro que define el objeto generico sobre el 
    /// cual opera la clase en tiempo de ejecución.</typeparam>
    public abstract class GenericBlo<T> : IGenericBlo<T> where T : BaseEntity
    {

        /// <summary>
        /// Propiedad para administrar el mecanismo de logeo 
        /// de informacion y fallas
        /// </summary>
        public static readonly ILog log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Propiedad que representa el objeto principal de acceso a datos.
        /// </summary>
        public IGenericDao<T> _dao;


        /// <summary>
        /// Constructor que permite la inyección de dependencias en lo 
        /// referente al acceso a datos
        /// </summary>
        /// <param name="dao">Objeto de acceso a datos que se inyecta al
        /// instanciarse la clase</param>
        public GenericBlo(IGenericDao<T> dao)
        {
            _dao = dao;
        }


        /// <summary>
        /// Metodo para validar si la conexion a la base de datos es correcta
        /// </summary>
        /// <returns>True: si la conexion a la base de datos es correcta, false: 
        /// en caso contrario.</returns>
        public bool DatabaseIsValid()
        {
            bool isValid = false;

            try
            {
                isValid = _dao.DatabaseIsValid();
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }

            return true;
        }

        /// <summary>
        /// Método generico que permite obtener el total de registros
        /// entidad
        /// </summary>
        /// <returns>numero de registros</returns>
        public virtual int GetCount()
        {
            int total = 0;
            try
            {
                total = _dao.GetCount();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }

            return total;
        }


        /// <summary>
        /// Método generico que permite obtener todos los elementos de una 
        /// entidad
        /// </summary>
        /// <returns>un listado de objetos del tipo especificado</returns>
        public IList<T> GetAll(bool relaciones = false)
        {
            try
            {
                return _dao.GetAll(relaciones);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Método generico que permite obtener todos los elementos de una 
        /// entidad paginados ademas de retornar el total de registros existentes
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <returns>Lista de datos con paginación</returns>
        public virtual IList<T> GetDatosGrid(out int total, int? page, int? limit, string sortBy, string direction, bool relaciones = false)
        {
            try
            {
                return _dao.GetDatosGrid(out total, page, limit, sortBy, direction, relaciones);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Método generico que permite obtener el detelle de una entidad
        /// filtrados  por el identificador del padre, paginados 
        /// ademas de retornar el total de registros existentes
        /// </summary>
        /// <param name="total">Total de reguistros</param>
        /// <param name="page">Numero de pagina</param>
        /// <param name="limit">Top de reguistros a mostrar</param>
        /// <param name="sortBy">Nombre del campo a ordenar</param>
        /// <param name="direction">Indica el tipo de orden (asc,desc)</param>
        /// <param name="nombreId">Nombre del campo identificador del padre</param>
        /// <param name="idPadre">Identificador del padre</param>
        /// <returns>Lista de datos con paginación</returns>
        public virtual IList<T> GetDatosDetalle(out int total, int? page, int? limit, string sortBy, string direction, string nombreId, object idPadre, bool relaciones = false)
        {
            try
            {
                return _dao.GetDatosDetalle(out total,page,limit,sortBy,direction,nombreId,idPadre,relaciones);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método generico que permite obtener el detelle de una entidad
        /// filtrados  por el identificador del padre
        /// </summary>
        /// <param name="nombreId">Nombre del campo identificador del padre</param>
        /// <param name="idPadre">Identificador del padre</param>
        /// <returns></returns>
        public virtual IList<T> GetDatosDetalle(string nombreId, object idPadre, bool relaciones = false)
        {
            try
            {

                return _dao.GetDatosDetalle(nombreId,idPadre,relaciones);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Método genérico que permite obtener una instancia de la entidad
        /// especificada a traves de su respectivo Id
        /// </summary>
        /// <param name="id">Identificador unico de la instancia que se 
        /// necesita obtener</param>
        /// <returns>El objeto obtenido</returns>
        public virtual T GetById(object id, bool relaciones = false)
        {
            try
            {
                return _dao.GetById(id, relaciones);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Metodo generico que permite cambiar el estado de una entidad 
        /// </summary>
        /// <param name="_id">Identificador unico de la instancia que sera 
        /// removida de la base de datos.</param>
        //public virtual void Inactivo(object _id)
        //{
        //    try
        //    {
        //        _dao.Inactivo(_id);
        //    }
        //    catch (Exception e)
        //    {
        //        log.Error("Error en removiendo objeto: " + typeof(T) + " : " + _id, e);
        //        throw new Exception(e.Message);
        //    }
        //}


        /// <summary>
        /// Método genérico que permite agrear una nueva instancia a la entidad
        /// especificada
        /// </summary>
        /// <param name="entity">Nueva instancia a ser persistida en la base de 
        /// datos</param>
        public virtual void Save(T entity)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _dao.Save(entity);
                    scope.Complete();
                }
                catch (Exception e)
                {
                    log.Error(e);
                    throw new Exception(e.Message);
                }
            }
            return;
        }


        /// <summary>
        /// Método genérico que permite agrear una nueva instancia a la entidad
        /// especificada. En este no se incluye la auditoria
        /// </summary>
        /// <param name="entity">Nueva instancia a ser persistida en la base de datos</param>
        public virtual void SaveNoTrack(T entity)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _dao.SaveNoTrack(entity);
                    scope.Complete();
                }
                catch (Exception e)
                {
                    log.Error(e);
                    throw new Exception(e.Message);
                }
            }
            return;
        }


        /// <summary>
        /// Permite cargar de manera eficiente de grandes cantidades de datos
        /// una tabla de SQL Server con datos de otra fuentes
        /// </summary>
        /// <param name="list">Lista de datos</param>
        /// <param name="TabelName">Nombre de la tabla de destino</param>
        public virtual void InsertData(List<T> list, string TabelName)
        {
            try
            {
                _dao.InsertData(list, TabelName);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método genérico que permite eliminar una instancia de la entidad 
        /// especificada, validando si el usuario y la opcion permite el 
        /// proceso de eliminacion.
        /// </summary>
        /// <param name="_id">Identificador único de la instancia que sera 
        /// removida de la base de datos.</param>
        public virtual void Remove(object _id)
        {
            try
            {
                _dao.Remove(_id);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Método genérico que permite eliminar una instancia de la entidad 
        /// especificada, validando si el usuario y la opcion permite el 
        /// proceso de eliminacion. En este no se incluye la auditoria
        /// </summary>
        /// <param name="_id"></param>
        public virtual void RemoveNoTrack(object _id)
        {
            try
            {
                _dao.RemoveNoTrack(_id);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Permite reseteaar el ID de una tabla
        /// </summary>
        /// <param name="nombreTabla"></param>
        public void ResetID(string nombreTabla)
        {
            try
            {
                _dao.ResetID(nombreTabla);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Permite ejecutar consultas SQL
        /// </summary>
        /// <param name="sql">consulta sql</param>
        public void EjecutarQuerySQL(string sql)
        {
            try
            {
                _dao.EjecutarQuerySQL(sql);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Permite validar los permisos del un suario por el rol
        /// </summary>
        /// <param name="codigo">código del permiso</param>
        public void ValidarPermiso(string codigo)
        {
            try
            {
                _dao.ValidarPermiso(codigo);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Permite validar los permisos para actualizar o agregar 
        /// registros esto por rol de usuario
        /// </summary>
        /// <param name="id">Identificador para</param>
        public void ValidarSave(long id)
        {

            try
            {
                _dao.ValidarSave(id);

            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception(e.Message);
            }
        }

    }
}
