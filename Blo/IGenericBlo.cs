using System.Collections.Generic;
using Model;
using System.Linq;

namespace Blo
{
    /// <summary>
    /// Interface Base para la logica del negocio (Business Logic Layer - BLL) que 
    /// expone los metodos de acceso a la logica del negocio, permitiendo los 
    /// métodos generales de: CRUD (Create, Read, Update y Delete).
    /// Implementa el control de transacciones a nivel de servicios con
    /// el objetivo de mantener la integridad de las diferentes acciones
    /// de persistencia.
    /// </summary>
    /// <typeparam name="T">Parámetro que define el objeto generico sobre el 
    /// cual opera la clase en tiempo de ejecución.</typeparam>
    public interface IGenericBlo<T> where T : BaseEntity
    {

        /// <summary>
        /// Metodo para validar si la conexion a la base de datos es correcta
        /// </summary>
        /// <returns>True: si la conexion a la base de datos es correcta, false: 
        /// en caso contrario.</returns>
        bool DatabaseIsValid();


        /// <summary>
        /// Método generico que permite obtener el total de registros
        /// entidad
        /// </summary>
        /// <returns>numero de registros</returns>
        int GetCount();



        /// <summary>
        /// Método generico que permite obtener todos los elementos de una 
        /// entidad
        /// </summary>
        /// <returns>un listado de objetos del tipo especificado</returns>
        IList<T> GetAll(bool relaciones = false);


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
        IList<T> GetDatosGrid(out int total, int? page, int? limit, string sortBy, string direction, bool relaciones = false);


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
        IList<T> GetDatosDetalle(out int total, int? page, int? limit, string sortBy, string direction, string nombreId, object idPadre, bool relaciones = false);


        /// <summary>
        /// Método generico que permite obtener el detelle de una entidad
        /// filtrados  por el identificador del padre
        /// </summary>
        /// <param name="nombreId">Nombre del campo identificador del padre</param>
        /// <param name="idPadre">Identificador del padre</param>
        /// <returns></returns>
        IList<T> GetDatosDetalle(string nombreId, object idPadre, bool relaciones = false);


        /// <summary>
        /// Método genérico que permite obtener una instancia de la entidad
        /// especificada a traves de su respectivo Id
        /// </summary>
        /// <param name="id">Identificador unico de la instancia que se 
        /// necesita obtener</param>
        /// <returns>El objeto obtenido</returns>
        T GetById(object id, bool relaciones = false);


        /// <summary>
        /// Método genérico que permite agrear una nueva instancia a la entidad
        /// especificada
        /// </summary>
        /// <param name="entity">Nueva instancia a ser persistida en la base de 
        /// datos</param>
        void Save(T entity);


        /// <summary>
        /// Método genérico que permite agrear una nueva instancia a la entidad
        /// especificada. En este no se incluye la auditoria
        /// </summary>
        /// <param name="entity">Nueva instancia a ser persistida en la base de datos</param>
        void SaveNoTrack(T entity);


        /// <summary>
        /// Permite cargar de manera eficiente de grandes cantidades de datos
        /// una tabla de SQL Server con datos de otra fuentes
        /// </summary>
        /// <param name="list">Lista de datos</param>
        /// <param name="TabelName">Nombre de la tabla de destino</param>
        void InsertData(List<T> list, string TabelName);


        /// <summary>
        /// Metodo generico que permite cambiar el estado de una entidad 
        /// </summary>
        /// <param name="_id">Identificador unico de la instancia que sera 
        /// removida de la base de datos.</param>
        //void Inactivo(object _id);


        /// <summary>
        /// Método genérico que permite eliminar una instancia de la entidad 
        /// especificada, validando si el usuario y la opcion permite el 
        /// proceso de eliminacion.
        /// </summary>
        /// <param name="_id">Identificador único de la instancia que sera 
        /// removida de la base de datos.</param>
        void Remove(object _id);


        /// <summary>
        /// Método genérico que permite eliminar una instancia de la entidad 
        /// especificada, validando si el usuario y la opcion permite el 
        /// proceso de eliminacion. En este no se incluye la auditoria
        /// </summary>
        /// <param name="_id"></param>
        void RemoveNoTrack(object _id);


        /// <summary>
        /// Permite reseteaar el ID de una tabla
        /// </summary>
        /// <param name="nombreTabla"></param>
        void ResetID(string nombreTabla);


        /// <summary>
        /// Permite ejecutar consultas SQL
        /// </summary>
        /// <param name="sql">consulta sql</param>
        void EjecutarQuerySQL(string sql);


        /// <summary>
        /// Permite validar los permisos del un suario por el rol
        /// </summary>
        /// <param name="codigo">código del permiso</param>
        void ValidarPermiso(string codigo);


        /// <summary>
        /// Permite validar los permisos para actualizar o agregar 
        /// registros esto por rol de usuario
        /// </summary>
        /// <param name="id">Identificador para</param>
        void ValidarSave(long id);

    }
}
